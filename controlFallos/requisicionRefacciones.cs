using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using h = Microsoft.Office.Interop.Excel;
using System.Threading;


namespace controlFallos
{
    public partial class requisicionRefacciones : Form
    {
        validaciones v;
        Thread excel;
        int idUsuario, empresa, area;
        Point arreglar = new Point(33, 273);
        Point locationInitial = new Point(348, 6);
        Size initialSize = new Size(800, 397);
        Point asolicitar = new Point(348, 6);
        Point solicitadas = new Point(348, 205);
        DataGridView informacion_imprimir = new DataGridView();
        delegate void uno();
        delegate void dos();
        byte[] img;
        bool editar, editarRefaccion, isexporting, aux;
        string idUsuarioR = "",codigoanterior = "", Folio = "", fecha ="";
        string unidadMedida = "Select x1.Simbolo from cunidadmedida as x1 inner join cfamilias as x2 on x1.idunidadmedida = x2.umfkcunidadmedida inner join cmarcas as x3 on x2.idfamilia = x3.descripcionfkcfamilias inner join crefacciones as x4 on x4.marcafkcmarcas = x3.idmarca where x4.codrefaccion = ";
        int RowDataRefaccionesASolicitar;
        DataSet ds = new DataSet();
       
       
        string Consulta = "Select convert(t1.Folio,char) as FOLIO, convert(t2.codrefaccion,char) as CODIGO, convert(t2.nombreRefaccion,  char) as 'Nombre Refaccion', convert(t1.NumParte,char) as 'Numero de Parte', convert(t1.Especificaciones,char) as 'Especificaiones', convert(t2.existencias, char) as 'Existencia',  convert(t1.Cantidad, char) as 'CANTIDAD SOLICITADA', if(t1.estatus = 0,'En Espera', if(t1.estatus=1, 'Aprobada', if(t1.estatus=2, 'Rechazada', if(t1.estatus = '', '','')))) as 'Estatus', convert(t1.Fecha, char) as 'Fecha De solicitud', convert(t1.precio,char) 'Precio de Compra', convert(t4.Simbolo, char) as MONEDA from crequicision as t1 inner join crefacciones as t2 on t1.refaccionfkCRefacciones = t2.idrefaccion INNER JOIN ctipocambio as t4 on t1.tipocambiofkCTipomoneda = t4.idtipocambio";
        string quitar = "delete from crequicision";
        string IVAd = "", proveedor = "", datosO = "";
        DataTable dt = new DataTable();
        DataTable dtvisual = new DataTable();
        DataRow filas;
        DataColumn columnas;
        MySqlDataAdapter adaptador = new MySqlDataAdapter();
        public requisicionRefacciones(int empresa, int area, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            /*cbxFamilia.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxFRefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxFamilia.DrawItem += v.combos_DrawItem;
            cbxFRefaccion.DrawItem += v.combos_DrawItem;*/
            iniProveedor();
            cargrdepartamento();
            Principal();
            iniMonedaCambio();
            //lbltitle.Text = "Requisición de Refacciones";
            //lbltitle.Left = (panel2.Width - lbltitle.Width) / 2;
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "ENRO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" });
            
            /*if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM pedidosrefaccion WHERE FolioPedfkSupervicion='" + idReporte + "'")) > 0)
            {
                gbxRefaccionesSolicitadas.Location = locationInitial;
                gbxRefaccionesSolicitadas.Size = initialSize;
                gbxRefaccionesSolicitadas.Visible = true;
            }
            else
            {
                gbxRefaccionesASolicitar.Location = locationInitial;
                gbxRefaccionesASolicitar.Size = initialSize;
                gbxRefaccionesASolicitar.Visible = true;
            }*/
        }


        void iniMonedaCambio()
        {
            v.iniCombos("select idTipoCambio as id, Simbolo from ctipocambio order by simbolo desc", cmbMoneda, "id", "Simbolo", "--MONEDA--");
        }
        void cargrdepartamento() { v.iniCombos("SET lc_time_names = 'es_ES';select convert(idcciglasReq,char) as idCigla, convert(usu, char) as usu from cciglasreq where cempesasfkempresa = '" + empresa + "'", cmbDepartamento, "idCigla", "usu", "--DEPASRTAMENTO--"); }
        void iniProveedor() { v.iniCombos("SET lc_time_names = 'es_ES';select convert(t1.idproveedor,char) as idunidad, convert(if(t1.empresa = '',concat(t1.aPaterno, ' ', t1.aMaterno, ' ', t1.nombres) , t1.empresa),char) as Nombre from cproveedores as t1 inner join cempresas as t2 on t1.empresaS = t2.idempresa where t1.empresaS = '" + empresa + "' order by Nombre asc", cmbProveedor, "idunidad", "Nombre", "-- SELECCIONE PROVEEDOR --"); }

        /*private void cbxFamilia_SelectedIndexChanged(object sender, EventArgs e) { if (cbxFamilia.SelectedIndex > 0) { v.iniCombos("SELECT t1.idrefaccion as id , UPPER(t1.nombreRefaccion) as nombre  FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas = t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias = t3.idfamilia WHERE t3.familiafkcnfamilias = '" + cbxFamilia.SelectedValue + "';", cbxFRefaccion, "id", "nombre", "-- SELECCIONE REFACCIÓN --"); cbxFRefaccion.Enabled = true; } else { cbxFRefaccion.DataSource = null; cbxFRefaccion.Enabled = false; } }
        private void cbxFRefaccion_SelectedIndexChanged(object sender, EventArgs e) { if (cbxFRefaccion.SelectedIndex > 0) lblUM.Text = v.getaData("SELECT Simbolo  FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas = t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias = t3.idfamilia INNER JOIN cunidadmedida as t4 ON t3.umfkcunidadmedida = t4.idunidadmedida WHERE t1.idrefaccion='" + cbxFRefaccion.SelectedValue + "'").ToString(); else lblUM.Text = null; }*/
        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e) { v.numerosDecimales(e); }
        private void txtCantidad_Validating(object sender, CancelEventArgs e) {
            if (!string.IsNullOrWhiteSpace(txtCantidad.Text.Trim()))
            {
                string datoretorna = v.maximos("select if('" + txtCantidad.Text + "' > maximo,'si','no') as result from crefacciones where codrefaccion = '" + txtCodigo.Text + "' and empresa = '" + empresa + "'").ToString();
                if (datoretorna.ToString().Equals("si"))
                {
                    MessageBox.Show("La cantidad ingresada supera el maximo de la refacción", "¡¡IMPORTANTE!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCantidad.Text = "";
                }
                else
                {
                    while (txtCantidad.Text.Contains(".."))
                        txtCantidad.Text = txtCantidad.Text.Replace("..", ".").Trim();
                    txtCantidad.Text = txtCantidad.Text.Trim();
                    txtCantidad.Text = string.Format("{0:F2}", txtCantidad.Text);
                    try
                    {
                        if (Convert.ToDouble(txtCantidad.Text) > 0)
                        {
                            CultureInfo ti = new CultureInfo("es-MX"); ti.NumberFormat.CurrencyDecimalDigits = 2; ti.NumberFormat.CurrencyDecimalSeparator = "."; txtCantidad.Text = string.Format("{0:N2}", Convert.ToDouble(txtCantidad.Text, ti));
                        }
                        else txtCantidad.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        txtCantidad.Clear(); MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            
            }
        }

        private void btnAgregarRefaccion_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!v.camposVaciosSolicitudRefacciones(txtCodigo.Text, txtExistencia.Text, txtCantidad.Text, txtNumParte.Text, txtEspecificaciones.Text, txtCosto.Text, lblNombreU.Text, txtContrasenna.Text, txtFolio.Text,cmbMoneda.SelectedIndex))
                {
                    //AgregarVisual();
                    var selectedOption = MessageBox.Show("¿Desea Agregar Más Productos a la Requisición?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (selectedOption == DialogResult.Yes)
                    {
                        //AgregarVisual();
                        registrar();
                        limpiar();
                    }
                    else
                    {
                        var selectedOption2 = MessageBox.Show("¿Desea Finalizar la Orden de Requicisión?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (selectedOption2 == DialogResult.Yes)
                        {
                            registrar();
                            fecha = dtpFecha.Value.ToString("yyyy-MM-dd");
                            agregaarDataset();
                            Expota_PDF();
                            limpiar_Definitivo();
                        }
                        else
                        {
                            registrar();
                            limpiar();
                            foilios();
                        }
                    }

                }
            }

            if (editar)
                {
                    if (!v.camposVaciosSolicitudRefacciones(txtCodigo.Text, txtExistencia.Text, txtCantidad.Text, txtNumParte.Text, txtEspecificaciones.Text, txtCosto.Text,lblNombreU.Text, txtContrasenna.Text, txtFolio.Text, cmbMoneda.SelectedIndex))
                    {
                        //AgregarVisual();
                        var selectedOption = MessageBox.Show("¿Desea Agregar Más Productos a la Requisición?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (selectedOption == DialogResult.Yes)
                        {
                            editarRegistro();
                            limpiar();
                        }
                        else
                        {
                            var selectedOption2 = MessageBox.Show("¿Desea Finalizar la Orden de Requicisión?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (selectedOption2 == DialogResult.Yes)
                            {
                                editarRegistro();
                            fecha = dtpFecha.Value.ToString("yyyy-MM-dd");
                            agregaarDataset();
                                Expota_PDF();
                                limpiar_Definitivo();
                            }
                            else
                            {
                                editarRegistro();
                                limpiar();
                                foilios();
                            }
                        }

                    }
                }
            
        }
        public void limpiar_Definitivo()
        {
            DataTable dtL = new DataTable();
            while (dtgRequicision.RowCount > 0)
            {
                dtgRequicision.Rows.Remove(dtgRequicision.CurrentRow);
            }
            txtCantidad.Text = txtNumParte.Text =txtEspecificaciones.Text =txtCodigo.Text =txtCosto.Text = txtRefaccion.Text = txtExistencia.Text = lblCosto.Text = txtContrasenna.Text = lblNombreU.Text = txtProveedor.Text = txtFolio.Text = txtFolioB.Text = txtCodigoB.Text = "";
            cargrdepartamento();
            cmbDepartamento.Enabled = true;
            cmbProveedor.SelectedItem = cmbMes.SelectedItem = 0;
            cbFecha.Checked = false;
            Principal();
            datosO = "";
            dgvRefaccionesaSolicitar.ClearSelection();
            dtgRequicision.ClearSelection();
            btnCancelar.Enabled = false;
            panel2.Visible = false;
            dgvRefaccionesaSolicitar.DataSource = dtL;
            dt.Rows.Clear();
            dt.Columns.Clear();
            editar = false;
            dgvRefaccionesaSolicitar.DataSource = dtL;
        }
        public void limpiar()
        {
           
            txtCantidad.Text = txtNumParte.Text = txtEspecificaciones.Text = txtCodigo.Text = txtCosto.Text = txtRefaccion.Text = txtExistencia.Text = lblCosto.Text = txtContrasenna.Text = lblNombreU.Text = txtProveedor.Text = "";
            cmbDepartamento.Enabled = false;
            Principal();
            //foilios();
           // dgvRefaccionesaSolicitar.ClearSelection();
            btnCancelar.Enabled = false;
            panel2.Visible = false;
            

        }
        public void AgregarVisual()
        {
            dgvRefaccionesaSolicitar.Rows.Add(txtCodigo.Text, txtRefaccion.Text);
        }
        public void foilios()
        {
            MySqlCommand cmd = new MySqlCommand("select Cigla from cciglasreq where cempesasfkempresa = '" + empresa + "' and usu = '" + cmbDepartamento.Text + "'", v.c.dbconection());
            string cigla = (string)cmd.ExecuteScalar();
            if (string.IsNullOrWhiteSpace(cigla) || cigla == null)
            {
                //MessageBox.Show("Departamento no encontrado", "!ALERTA¡", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFolio.Text = "";
            }
            else
            {
                MySqlCommand cmdfolio = new MySqlCommand("SELECT SUBSTRING_INDEX(Folio, '-',-1)  AS Folio from crequicision  where empresa = '" + empresa + "' and departamento = '" + cmbDepartamento.SelectedValue + "' order by idcrequicision desc limit 1;", v.c.dbconection());
                string folio = (string)cmdfolio.ExecuteScalar();
                if (string.IsNullOrWhiteSpace(folio) || folio == null)
                {
                    txtFolio.Text = cigla + "-" + "1";
                    Folio = txtFolio.Text;
                }
                else
                {
                    txtFolio.Text = cigla + "-" + Convert.ToString(Convert.ToInt32(folio) + 1);
                    Folio = txtFolio.Text;
                }
            }
            
        }
        public void agregaarDataset()
        {
            
            if (dt.Columns.Count == 0)
            {
                columnas = new DataColumn();
                columnas.ColumnName = "Codigo Refacción";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Nombre Refacción";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Numero De Parte";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Especificación";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Existencia";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Cantidad Solicitada";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Unidad Medida";
                dt.Columns.Add(columnas);
                
            }
            string[] arr = datosO.ToString().Split('|');
            for (int i = 1; i < arr.Count();)
            {
                string medida = v.getaData(unidadMedida + "'" + arr[i].ToString() + "'").ToString();
                filas = dt.NewRow();
                filas["Codigo Refacción"] = arr[i].ToString();
                filas["Nombre Refacción"] = arr[i + 1].ToString();
                filas["Numero De Parte"] = arr[i + 2].ToString();
                filas["Especificación"] = arr[i + 3].ToString();
                filas["Existencia"] = arr[i + 4].ToString();
                filas["Cantidad Solicitada"] = arr[i + 5].ToString();
                filas["Unidad Medida"] = medida.ToString();
                dt.Rows.Add(filas);
                i = i + 6;
            }
            DataSet dst = new DataSet();
            dst.Tables.Add(dt.Copy());
            dataGridView2.DataSource = dst.Tables[0];
        }
        public void registrar()
        {
            datosO = datosO + "|" + txtCodigo.Text + "|" + txtRefaccion.Text + "|" + txtNumParte.Text + "|" + txtEspecificaciones.Text + "|" + txtExistencia.Text + "|" + txtCantidad.Text;
            v.AgregarRequicision("insert into crequicision(Folio, cantidad, numparte, especificaciones, estatus,  refaccionfkCRefacciones, fecha, precio, empresa, usuariofkCPersonal,tipocambiofkCTipomoneda,departamento,Existencia) value('" + txtFolio.Text + "','" + txtCantidad.Text + "','" + txtNumParte.Text + "','" + txtEspecificaciones.Text + "','0',(select Distinct(idrefaccion) from crefacciones where codrefaccion = '" + txtCodigo.Text + "' and empresa = '" + empresa + "'), now(), '" + txtCosto.Text + "','" + empresa + "','" + idUsuarioR.ToString() + "','" + cmbMoneda.SelectedValue + "','" + cmbDepartamento.SelectedValue + "','" + txtExistencia.Text + "' )");
        }
        public void editarRegistro()
        {
            datosO = datosO + "|" + txtCodigo.Text + "|" + txtRefaccion.Text + "|" + txtNumParte.Text + "|" + txtEspecificaciones.Text + "|" + txtExistencia.Text + "|" + txtCantidad.Text;
            v.AgregarRequicision("update crequicision set cantidad= '" + txtCantidad.Text + "',numparte ='" + txtNumParte.Text + "', especificaciones ='" + txtEspecificaciones.Text + "',fecha=now(),precio='" + txtCosto.Text + "', refaccionfkCRefacciones=(select idrefaccion from crefacciones where codrefaccion = '" + txtCodigo.Text + "' and empresa = '" + empresa + "'), usuariofkCPersonal ='" + idUsuarioR.ToString() + "',tipocambiofkCTipomoneda = '" + cmbMoneda.SelectedValue + "' where Folio = '" + txtFolio.Text + "' and refaccionfkCRefacciones = (select idrefaccion from crefacciones where codrefaccion = '" + codigoanterior.ToString() + "' and empresa = '" + empresa + "')");
        }
        public void Principal()
        {
            dtgRequicision.ClearSelection();
            adaptador = v.getReport(Consulta + " where DATE_FORMAT(t1.fecha, '%Y/%M/%d') = DATE_FORMAT(now(), '%Y/%M/%d') and t1.empresa = '" + empresa + "' ORDER BY t1.fecha DESC");
            adaptador.Fill(ds);
            dtgRequicision.DataSource = ds.Tables[0];
        }
        /*void insertar()
        {
          
                if (!existe(Convert.ToInt32(cbxFRefaccion.SelectedValue)) )
                {
                    dgvRefaccionesaSolicitar.Rows.Add(new object[] { cbxFRefaccion.SelectedValue, cbxFRefaccion.Text, txtCantidad.Text });
                    limpiar();
                }
                else
                {
                    MessageBox.Show("La Refacción Ya Se Encuentra en Fila Para Solicitar\nPuede Editar La Cantidad Seleccionando La Refacción de La Tabla \"Refacciones A Solicitar\" ", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbxFRefaccion.Focus();
                }
        }*/
       
        bool existe(int refaccion)
        {
            foreach (DataGridViewRow row in dgvRefaccionesaSolicitar.Rows)
                if (Convert.ToInt32(row.Cells[0].Value) == refaccion) return true;
            return false;
        }

        private void requisicionRefacciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormFallasMantenimiento ffm = (FormFallasMantenimiento)Owner;
            //if (dgvRefaccionesaSolicitar.Rows.Count>0)
            //{
            //    FormContraFinal fc = new FormContraFinal(2, 1, this);
            //    fc.Owner = this;
            //    fc.LabelTitulo.Text = "Introduzca su Contraseña para Completar\nLa Solicitud de Refaccíones";
            //    if (fc.ShowDialog()== DialogResult.OK)
            //    {
            //        object id = fc.id;
            //        foreach (DataGridViewRow row in dgvRefaccionesaSolicitar.Rows)
            //            v.c.insertar(string.Format("INSERT INTO pedidosrefaccion(FolioPedfkSupervicion, RefaccionfkCRefaccion, Cantidad,  usuariofkcpersonal) VALUES({0},{1},{2},{3})",new object[] {idReporte,row.Cells[0].Value,row.Cells[2].Value,id}));
                   
            //        //ffm.cbxRequierenRefacciones.Enabled = false;
            //        //ffm.cbxExistenciaRefacciones.Enabled = false;
            //        //ffm.cbxExistenciaRefacciones.SelectedIndex = 1;
            //    } else
            //    {
            //        if (MessageBox.Show("Si Cierra El Formulario Se Borrarán las Refacciones a Solicitar ¿Desea Continuar?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //            DialogResult = DialogResult.None;
            //        //else
            //            //ffm.cbxRequierenRefacciones.SelectedIndex = 1;
                       
            //    }

            }
        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void dtgRequicision_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void label62_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dtpFechaA.Enabled = dtpFechaDe.Enabled = true;
        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Folio = txtFolioB.Text;
            metodos_de_busqueda();
        }
        public void metodos_de_busqueda()
        {
            dtgRequicision.ClearSelection();
            ds.Clear();
            if (txtFolioB.Text != "")
            {
                adaptador = v.getReport(Consulta + " where t1.folio = '" + txtFolioB.Text + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
                fecha = ds.Tables[0].Rows[0][8].ToString();
                pImprimir.Visible = true;
                pexcel.Visible = true;
            }
            else if (txtCodigoB.Text != "")
            {
                adaptador = v.getReport(Consulta + " where t2.codrefaccion = '" + txtCodigoB.Text + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
                pexcel.Visible = true;
            }
            else if (Convert.ToInt32(cmbProveedor.SelectedValue) > 0)
            {
                adaptador = v.getReport(Consulta + " where t1.proveedorfkCProveedor = '" + cmbProveedor.SelectedIndex.ToString() + "' or t1.proveedorfkCProveedor = '" + cmbProveedor.SelectedIndex.ToString() + "' or t1.proveedorfkCProveedor = '" + cmbProveedor.SelectedIndex.ToString() + "' and t1.empresa = '" + empresa + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
                pexcel.Visible = true;
            }
            else if (cmbMes.SelectedIndex > 0)
            {
                string messel = "";
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                
                adaptador = v.getReport(Consulta + " where date_format(convert(t1.Fecha, char), '%m') = '" + messel + "'and t1.empresa = '" + empresa + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
                pexcel.Visible = true;
                pexcel.Visible = true;
            }

            else if (cbFecha.Checked == true)
            {
                adaptador = v.getReport(Consulta + "   where date_format(convert(t1.fecha, char),'%d/%m/%Y') between'" + dtpFechaDe.Value.ToString("dd/MM/yyyy") + "' and '" + dtpFechaA.Value.ToString("dd/MM/yyyy") + "' and t1.empresa = '" + empresa + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
                cbFecha.Checked = false;
                dtpFechaA.Enabled = dtpFechaDe.Enabled = false;
                pexcel.Visible = true;
            }
            else if (cbFecha.Checked == true)
            {
                string messel = "";
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                adaptador = v.getReport(Consulta + "   where left(right(t1.fecha,14),2) ='" + messel + "' and t1.empresa = '" + empresa + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
                pexcel.Visible = true;

            }
           
            else
            {
                MessageBox.Show("No hay parametros de busqueda", "Inportante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                pexcel.Visible = false;
            }
        }
        private void dgvRefaccionesaSolicitar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.dgvRefaccionesaSolicitar.Rows.RemoveAt(dgvRefaccionesaSolicitar.CurrentRow.Index);
                //eliminar_dato(dato);

            }
        }

        public void eliminar_dato(DataGridViewCellEventArgs e)
        {
            string codigo = dgvRefaccionesaSolicitar.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.dgvRefaccionesaSolicitar.Rows.RemoveAt(dgvRefaccionesaSolicitar.CurrentRow.Index);
            v.AgregarRequicision(quitar + " where Folio = '" + txtFolio.Text + "' and refaccionfkCRefacciones=(select idrefaccion from crefacciones where codrefaccion='" + codigo + "')");

        }

        private void cmbProveedor1_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbProveedor2_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbProveedor3_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbProveedor_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbMes_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtContrasenna_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumerossinespacios(e);
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                
            }
        }
        public void buscaref(string codigo)
        {
          string[] seprar = v.ObtenerRefR("SET lc_time_names = 'es_ES';select COALESCE(concat(convert(t1.nombreRefaccion,char),'|', convert(t1.existencias, char), '|',convert(t1.CostoUni, char),'|', convert(t1.modeloRefaccion, char),'|', coalesce(convert(if(x1.empresa = '',concat(x1.aPaterno, ' ', x1.aMaterno, ' ', x1.nombres) , x1.empresa),char),'')),'') from crefacciones as t1 inner join cmarcas as t3 on t1.marcafkcmarcas = t3.idmarca inner join cfamilias as t4 on t3.descripcionfkcfamilias = t4.idfamilia inner join cunidadmedida as t2 on t4.umfkcunidadmedida = t2.idunidadmedida inner join cproveedores as x1 on x1.idproveedor = t1.proveedrofkCProveedores where t1.codrefaccion = '" + codigo + "' and t1.empresa  = '" + empresa + "'").ToString().Split('|');
            if (seprar.Length == 1 )
            {
                MessageBox.Show("No se encontro la refaccion".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtRefaccion.Text = seprar[0].ToString();
                txtExistencia.Text = seprar[1].ToString();
                lblCosto.Text = seprar[2].ToString();
                txtNumParte.Text = seprar[3].ToString();
                txtProveedor.Text = seprar[4].ToString();
            }
        }

        private void dgvRefaccionesaSolicitar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedOption = MessageBox.Show("¿Esta seguro de que deseea eliminar la refacción?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (selectedOption == DialogResult.Yes)
            {
                eliminar_dato(e);
            }
        }

        private void btnAgregarMas_Click(object sender, EventArgs e)
        {
            var selectedOption = MessageBox.Show("¿Seguro que quiere cancelar el registro?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (selectedOption == DialogResult.Yes)
            {
                limpiar_Definitivo();
            }
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            btnCancelar.Enabled = true;
            v.mayusculas(txtCodigo.Text);
        }

        private void gbGrupo_Enter(object sender, EventArgs e)
        {

        }

        private void txtContrasenna_Validating(object sender, CancelEventArgs e)
        {
            if (Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join puestos as t2 on t1.cargofkcargos=t2.idpuesto inner join datosistema as t3 on t3.usuariofkcpersonal=t1.idPersona where t3.password='" + v.Encriptar(txtContrasenna.Text.Trim()) + "' and t1.empresa='" + empresa + "' and t1.area='" + area + "' and t1.status='1';")) > 0)
            {
                string[] datos = v.getaData("select upper(concat(coalesce(t1.ApPaterno,''),' ',coalesce(t1.ApMaterno,''),' ',coalesce(t1.nombres,''),'/',t1.idpersona)) from cpersonal as t1 inner join datosistema as t2 on t2.usuariofkcpersonal=t1.idPersona where t2.password='" + v.Encriptar(txtContrasenna.Text.Trim()) + "'").ToString().Split('/');
                lblNombreU.Text = datos[0];
                idUsuarioR = datos[1];
            }
            else
                lblNombreU.Text = idUsuarioR = "";
        }

        public void Expota_PDF()
        {
            
            //Código para generación de archivo pdf
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(20f, 20f, 10f, 10f);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ValidateNames = true;
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Requerimiento de Material";
            saveFileDialog1.Filter = "Archivos PDF (*.pdf)|*.pdf";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            string filename = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
                string p = Path.GetExtension(filename);
                p = p.ToLower();
                if (p.ToLower() != ".pdf")
                    filename = filename + ".pdf";
                while (filename.ToLower().Contains(".pdf.pdf"))
                    filename = filename.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
            }
            try
            {
                if (filename.Trim() != "")
                {
                    FileStream file = new FileStream(filename,
                        FileMode.Create,
                        FileAccess.ReadWrite,
                        FileShare.ReadWrite);
                    PdfWriter.GetInstance(doc, file);
                    iTextSharp.text.Font arial = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font arial2 = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                    doc.Open();
                    if (empresa == 2)
                    {
                         img = Convert.FromBase64String(v.tri);
                    }
                    else
                    {
                        img = Convert.FromBase64String(v.trainsumos);
                    }
                    
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    Chunk chunk = new Chunk("REQUERIMIENTO DE MATERIAL", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    doc.Add(imagen);
                    doc.Add(new Paragraph(chunk));
                    doc.Add(new Paragraph("                                    "));
                    PdfPTable tabla = new PdfPTable(2);
                    tabla.DefaultCell.Border = 0;
                    tabla.WidthPercentage = 100;
                    /*
                     t1.Folio,'|',UNIDAD,'|',FECHA Y HORA,'|',MECANICO,'|',FECHAHORA ENTREGA,'|',PERSONA QUE ENTREGA,'|',FolioFactura,'|',ObservacionesTrans
                     */
                    tabla.AddCell(v.valorCampo("FOLIO DEL REQUERIMIENTO", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("FECHA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(Folio, 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(fecha.ToString(), 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    /*tabla.AddCell(v.valorCampo("FECHA DE ENTREGA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(dtFecha.Value.ToString("yyyy-MM-dd"), 1, 0, 0, arial2));*/
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    //tabla.AddCell(v.valorCampo("FOLIO DE FACTURA", 1, 0, 0, arial));
                    /* tabla.AddCell(v.valorCampo("Non. REFACCION", 1, 0, 0, arial));
                     tabla.AddCell(v.valorCampo("ENTREGA", 2, 0, 0, arial));
                     tabla.AddCell(v.valorCampo(lblNomRef.Text, 1, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo(lblNomUsuario.Text, 1, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("COMENTARIOS", 2, 0, 0, arial));
                     tabla.AddCell(v.valorCampo("", 2, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));*/




                    /*tabla.AddCell(v.valorCampo("REFACCIONES SOLICITADAS", 2, 1, 0, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
                    tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));*/
                    doc.Add(tabla);
                    GenerarDocumento(doc);
                    doc.Close();
                    System.Diagnostics.Process.Start(filename);
                    //    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToUpper(), "ERROR AL EXPORTAR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void GenerarDocumento(Document document)
        {
            int i, j;
            iTextSharp.text.Font arial2 = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
            PdfPTable tabla1 = new PdfPTable(2);
            tabla1.DefaultCell.Border = 0;
            tabla1.WidthPercentage = 100;
            tabla1.AddCell(v.valorCampo("REFACCIONES SOLICITADAS", 2, 1, 0, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
            tabla1.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));


            PdfPTable datatable = new PdfPTable(dataGridView2.ColumnCount);
            datatable.DefaultCell.Padding = 4;


            float[] headerwidths = GetTamañoColumnas(dt);
            datatable.SetWidths(headerwidths);
            Color color = Color.PaleGreen;
            datatable.WidthPercentage = 100;
            PdfPCell observaciones = new PdfPCell();
            datatable.DefaultCell.BorderWidth = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < dataGridView2.ColumnCount; i++)
            {
                datatable.AddCell(new Phrase(dataGridView2.Columns[i].HeaderText.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
            }
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(250, 250, 250);
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < dataGridView2.RowCount; i++)
            {
                for (j = 0; j < dataGridView2.ColumnCount; j++)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(dataGridView2[j,i].Value.ToString(), FontFactory.GetFont("ARIAL", 8)));
                    celda.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                    /* if (j == 5 && dgAgregados[j, i].Value.ToString() == "EXISTENCIA")
                         celda.BackgroundColor = new iTextSharp.text.BaseColor(Color.PaleGreen);
                     else
                         celda.BackgroundColor = new iTextSharp.text.BaseColor(Color.LightCoral);*/
                    if (dataGridView2[j, i].Value != null)
                        datatable.AddCell(celda);

                }
                datatable.CompleteRow();
            }
            datatable.AddCell(observaciones);
            document.Add(tabla1);
            document.Add(datatable);
            document.Add(new Paragraph("\n\n\nELABORO: _________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph("\n\n\nSUPERVISO: _________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph("\n\n\nVoBo: _________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
        }
        public float[] GetTamañoColumnas(DataTable dg)
        {
            float[] values = new float[dataGridView2.ColumnCount];
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                values[i] = (float)dataGridView2.Columns[i].Width;
            }
            return values;
        }

        private void dtgRequicision_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dtgRequicision.Columns[e.ColumnIndex].Name == "Estatus")
                e.CellStyle.BackColor = (e.Value.ToString() == "En Espera" ? Color.Khaki : e.Value.ToString() == "Aprobada" ? Color.PaleGreen : e.Value.ToString() == "Rechazada" ? Color.Red : Color.LightBlue);
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            excel = new Thread(new ThreadStart(exportar_excel));
            excel.Start();
        }

        private void cmbMoneda_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtCodigo_Validating(object sender, CancelEventArgs e)
        {
            buscaref(txtCodigo.Text);
        }
        
        void inicio()
        {
            btnexportar.Visible = !(pbgif.Visible = true);
            lblexcel.Text = "Exportando";
        }

        private void txtFolioB_TextChanged(object sender, EventArgs e)
        {

        }

        void termino()
        {
            lblexcel.Text = "Exportar";
            if (!aux)
                btnexportar.Visible = true;
            else
                pexcel.Visible = false;
            pbgif.Visible = isexporting = aux = false;
        }
        void exportar_excel()
        {
            if (dtgRequicision.Rows.Count > 0)
            {
                isexporting = true;
                dt = (DataTable)dtgRequicision.DataSource;
                if (this.InvokeRequired)
                {
                    uno delega = new uno(inicio);
                    this.Invoke(delega);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                h.Worksheet sheet = X.ActiveSheet;
                X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    h.Range rng = (h.Range)sheet.Cells[1, i + 1];
                    sheet.Cells[1, i + 1] = dt.Columns[i].ColumnName.ToUpper();
                    rng.Interior.Color = System.Drawing.Color.Crimson;
                    rng.Borders.Color = System.Drawing.Color.Black;
                    rng.Font.Color = System.Drawing.Color.White;
                    rng.Cells.Font.Name = "Calibri";
                    rng.Cells.Font.Size = 12;
                    rng.Font.Bold = true;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        try
                        {
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                            sheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Cells.Font.Name = "Calibri";
                            rng.Cells.Font.Size = 11;
                            rng.Font.Bold = false;
                            rng.Interior.Color = Color.FromArgb(231, 230, 230);
                        }
                        catch (System.NullReferenceException EX)
                        { MessageBox.Show(EX.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                }
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                if (this.InvokeRequired)
                {
                    dos delega2 = new dos(termino);
                    this.Invoke(delega2);
                }
                excel.Abort();
                termino();
                pexcel.Visible = false;
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void dtgRequicision_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           codigoanterior = dtgRequicision.Rows[e.RowIndex].Cells[1].Value.ToString();
            string folio = dtgRequicision.Rows[e.RowIndex].Cells[0].Value.ToString();
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';select concat(convert(t1.Folio, char), '|', convert(t1.Cantidad,char),'|', convert(t1.NumParte, char),'|', convert(t1.Especificaciones,char),'|', convert(t1.precio,char),'|', convert(t2.codrefaccion,char), '|', convert(t2.nombreRefaccion,char),'|', convert(t2.existencias, char), '|',convert(t2.CostoUni, char),'|', coalesce(convert(if(x1.empresa = '',concat(x1.aPaterno, ' ', x1.aMaterno, ' ', x1.nombres) , x1.empresa),char),''), '|', convert(t1.departamento, char),'|', convert(t1.tipocambiofkCTipomoneda,char))  from crequicision as t1 inner join crefacciones as t2 on t1.refaccionfkCRefacciones = t2.idrefaccion  inner join cproveedores as x1 on x1.idproveedor = t2.proveedrofkCProveedores  where t2.codrefaccion = '" + codigoanterior + "' and t1.Folio  = '" + folio + "' and t1.empresa  = '" + empresa + "'").ToString().Split('|');
            txtFolio.Text = datos[0].ToString();
            txtCantidad.Text = datos[1].ToString();
            txtNumParte.Text = datos[2].ToString();
            txtEspecificaciones.Text = datos[3].ToString();
            txtCosto.Text = datos[4].ToString();
            txtCodigo.Text = datos[5].ToString();
            txtRefaccion.Text = datos[6].ToString();
            txtExistencia.Text = datos[7].ToString();
            lblCosto.Text = datos[8].ToString();
            txtProveedor.Text = datos[9].ToString();
            //cmbDepartamento.SelectedValue = int.Parse(datos[13].ToString());
            cmbMoneda.SelectedValue = int.Parse(datos[11].ToString());
            cmbDepartamento.Enabled = false;
            panel2.Visible = true;
            btnEliminar.Enabled = true;
            editar = true; 
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var selectedOption = MessageBox.Show("¿Esta seguro de que deseea eliminar la refacción?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (selectedOption == DialogResult.Yes)
            {
                v.AgregarRequicision(quitar + " where Folio = '" + txtFolio.Text + "' and refaccionfkCRefacciones=(select idrefaccion from crefacciones where codrefaccion='" + txtCodigo.Text + "')");
                limpiar();
                panel2.Visible = false;
            }
        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        public void seleccionar_folio(object sender, EventArgs e)
        {
            foilios();
        }

        public void imorimir(object sender, EventArgs e)
        {
            Folio = txtFolioB.Text;
            DataSet ds = (DataSet)v.TraerRequerimiento(txtFolioB.Text, empresa);
            dataGridView2.DataSource = ds.Tables[0];
            pImprimir.Visible = false;
            Expota_PDF();
            limpiar_Definitivo();
        }

    }
    }
