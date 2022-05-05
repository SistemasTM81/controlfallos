using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using h = Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Paragraph = iTextSharp.text.Paragraph;

namespace controlFallos
{
    public partial class Material_Recuperado : Form
    {
        validaciones v;
        Thread excel;
        delegate void uno();
        delegate void dos();
        int idUsuario, status, empresa, area, idEntrega, familiaanterior,  economicoanterior;
        string inicioCodigo = "", codigoanterior = "", motivoanterior = "", nombreanterior = "", cantidadanterior = "", mecanicoanterior = "";
        public bool editar { private set; get; }
        Thread exportar, th;
        //decimal ultimacantidad;
        public string idRefaccionMediaAbast;
        public bool isexporting, aux, accion = true;
        bool yaAparecioMensaje = false;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        DataTable dt = new DataTable();
        public Material_Recuperado(System.Drawing.Image logo, int idUsuario, int empresa, int area, validaciones v)
        {
            this.v = v;
           
            InitializeComponent();
            pblogo.BackgroundImage = logo;
            this.idUsuario = idUsuario;
            cmbEconomico.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbMantenimiento.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbfamiliabusq.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbrefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbrefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area; DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new System.Drawing.Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbrefaccion.ColumnHeadersDefaultCellStyle = d;
            cadenaCodigo();
            txtcodrefaccionRep.Text = obtenerFolio();
            cargaEcoBusq(cmbEconomico);
            CargarMecanico();
            iniFamilias();
            inifamiliasBusq();
            consultaGeneral(" where t1.empresa = '" + empresa + "' and t1.status = 1 limit 10 ;");
            v.comboswithuot(cmbMotivos, new string[] { "--Seleccione Un Motivo--", "Reparacion", "Reuso"});

        }
        public Material_Recuperado(System.Drawing.Image logo, int idUsuario, string idefaccion, validaciones v)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            pblogo.BackgroundImage = logo;
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.idRefaccionMediaAbast = idefaccion;
        }
        public void establecerPrivilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "catRefacC")).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                if (Convert.ToInt32(privilegiosTemp.Length) > 3)
                {
                    Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }

            }
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar)
            {
                gbaddrefaccion.Visible = true;
            }
            if (Pconsultar)
            {
                gbbuscar.Visible = true;
                tbrefaccion.Visible = true;
            }
            if (Peditar)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
        }
        private void gbaddrefaccion_Enter(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {

            if (!accion)
            {
               
               agregar();
               
            }
            else
            {
                
                if (!string.IsNullOrWhiteSpace(txtSalidas.Text))
                {
                    salida();
                }
                else
                {
                    modificar();
                }
            }
           
            
        }
        void agregar()
        {
            if (!v.formularioRefacionesRecu(txtcodrefaccionRep.Text, txtnombrereFaccionRep.Text, txtCantidad.Text, cmbDescripcion.SelectedIndex, txtContasenna.Text, cmbMantenimiento.SelectedIndex, txtMotivo.Text))
            {
                v.c.insertar("insert into crefaccionesrecu(coderefaccRecu, nomrefaccion, existencias, desfamilicnfamilias, fechaHoraalta, usuarioaltafkcpersonal, empresa, usuariomantenimiento, descripcion, ecofkcunidades,status) values('" + txtcodrefaccionRep.Text + "', '" + txtnombrereFaccionRep.Text + "', '" + txtCantidad.Text + "', '" + Convert.ToInt32(cmbDescripcion.SelectedValue) + "', now(), '" + idUsuario + "', '" + empresa + "', '" + cmbMantenimiento.SelectedValue + "', '" + txtMotivo.Text + "','" + Convert.ToInt32(cmbEconomico.SelectedValue) + "', '1')");
                Limpiar();
            }
        }
        void modificar()
        {
            if (!v.formularioRefacionesRecu(txtcodrefaccionRep.Text, txtnombrereFaccionRep.Text, txtCantidad.Text, cmbDescripcion.SelectedIndex, txtContasenna.Text, cmbMantenimiento.SelectedIndex, txtMotivo.Text))
            {
                double cantidadactualizada = 0.0;
                if (txtCantidad.Text != cantidadanterior)
                {
                    cantidadactualizada = Double.Parse(txtCantidad.Text) + Double.Parse(cantidadanterior);
                }
                else
                {
                    cantidadactualizada = Double.Parse(cantidadanterior);
                }
                v.c.insertar("update crefaccionesrecu set nomrefaccion = '" + txtnombrereFaccionRep.Text + "', existencias ='" + cantidadactualizada + "', desfamilicnfamilias ='" + Convert.ToInt32(Convert.ToInt32(cmbDescripcion.SelectedValue)) + "', usuarioaltafkcpersonal = '" + idUsuario + "', usuariomantenimiento ='" + cmbMantenimiento.SelectedValue + "', descripcion = '" + txtMotivo.Text + "', ecofkcunidades = '" + Convert.ToInt32(cmbEconomico.SelectedValue) + "' where coderefaccRecu = '" + txtcodrefaccionRep.Text + "' and empresa ='" + empresa + "'");
                Limpiar();
            }
        }
        void salida()
        {
            if (!v.SalidaRecu(txtcodrefaccionRep.Text, Convert.ToDouble(txtSalidas.Text), cmbMotivos.SelectedIndex, txtContasenna.Text, lblNombre.Text, cmbBuscarUnidad.SelectedIndex))
            {
                if (Double.Parse(txtCantidad.Text) < Double.Parse(txtSalidas.Text))
                {
                    MessageBox.Show("La cantidad ingresada es mayor a la existencia", "¡Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    v.c.insertar("insert into csmaterial_recu(materialrecuidrecu, unidadfkcunidad, cantidadsalida, fechahorasalida, almacenfkcpersonal, TipoSalida, empresa) value((select idcrefaccionesrecu from crefaccionesrecu where coderefaccRecu = '" + txtcodrefaccionRep.Text + "' and empresa =  '" + empresa + "'), '" + cmbBuscarUnidad.SelectedValue + "', '" + txtSalidas.Text + "', now(), '" + idUsuario + "', '" + cmbMotivos.SelectedValue + "','" + empresa + "')");
                    Double cantidadnueva = Convert.ToDouble(txtCantidad.Text) - Convert.ToDouble(txtSalidas.Text);
                    v.c.insertar("update crefaccionesrecu set existencias = '" + cantidadnueva + "' where coderefaccRecu = '" + txtcodrefaccionRep.Text + "'and empresa = '" + empresa + "'");
                    Expota_PDF();
                    Limpiar();
                }
               
            }
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            Limpiar();
            consultaGeneral(" where t1.empresa = '" + empresa + "' and t1.status = 1 limit 10 ;");
        }

        private void btndelref_Click(object sender, EventArgs e)
        {
            desactivar("update crefaccionesrecu set status = 2 where coderefaccRecu ='" + txtcodrefaccionRep.Text + "'");
            Limpiar();
            MessageBox.Show("Refaccion Dada De Baja Con Exito", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigobusq.Text) && string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text) && cbfamiliabusq.SelectedIndex == 0)
            {
                consultaGeneral(" where t1.coderefaccRecu = '" + txtCodigobusq.Text + "' and t1.empresa ='" + empresa + "'");
            }
            else if(string.IsNullOrWhiteSpace(txtCodigobusq.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text) && cbfamiliabusq.SelectedIndex == 0)
            {
                consultaGeneral(" where t1.nomrefaccion like '%" + txtnombrereFaccionbusq.Text + "%' and t1.empresa ='" + empresa + "'");
            }
            else if (string.IsNullOrWhiteSpace(txtCodigobusq.Text) && string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text) && cbfamiliabusq.SelectedIndex != 0)
            {
                consultaGeneral(" where x2.idcnFamilia = '" + Convert.ToInt32(cbfamiliabusq.SelectedValue) + "' and t1.empresa = '" + empresa + "'");
            }
            else if (!string.IsNullOrWhiteSpace(txtCodigobusq.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text) && cbfamiliabusq.SelectedIndex == 0)
            {
                consultaGeneral(" where t1.coderefaccRecu = '" + txtCodigobusq.Text + "' and t1.nomrefaccion like '%" + txtnombrereFaccionbusq.Text + "%' and t1.empresa = '" + empresa + "'");
            }
            else if (!string.IsNullOrWhiteSpace(txtCodigobusq.Text) && string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text) && cbfamiliabusq.SelectedIndex != 0)
            {
                consultaGeneral(" where t1.coderefaccRecu = '" + txtCodigobusq.Text + "' and t1.empresa ='" + empresa + "' and x2.idcnFamilia = '" + cbfamiliabusq.SelectedIndex + "'");
            }
            else if (string.IsNullOrWhiteSpace(txtCodigobusq.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text) && cbfamiliabusq.SelectedIndex == 0)
            {
                consultaGeneral(" where t1.nomrefaccion like '%" + txtnombrereFaccionbusq.Text + "%' and t1.empresa ='" + empresa + "' and x2.idcnFamilia = '" + cbfamiliabusq.SelectedIndex + "'");
            }
            pActualizar.Visible = btnExcel.Visible = pictureBox2.Visible = LblExcel.Visible= true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            consultaGeneral(" where t1.empresa = '" + empresa + "' and t1.status = 1 limit 10 ;");
            Limpiar();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            excel = new Thread(new ThreadStart(exportar_excel));
            excel.Start();
        }
        void inicio()
        {
            btnExcel.Visible = !(pbgif.Visible = true);
            LblExcel.Text = "Exportando";
            pictureBox2.Visible = true;
            LblExcel.Visible = true;
        }
        void termino()
        {
            LblExcel.Text = "Exportar";
            if (!aux)
                btnExcel.Visible = true;
            else
            pbgif.Visible = isexporting = aux = false;
            pActualizar.Visible = btnExcel.Visible = pictureBox2.Visible = LblExcel.Visible = pbgif.Visible = false;
        }
        private void txtContasenna_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumerossinespacios(e);
           /* if ((int)e.KeyChar == (int)Keys.Enter || (int)e.KeyChar == (int)Keys.Tab)
            {
                obtenerNombre();
            }*/
        }

        private void cmbFamilia_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbEconomico_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbMantenimiento_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        public string obtenerFolio()
        {
            int valorinicial = 1000;
            string codigo = "";
            int idContinuo = v.DatocoSigue("select count(idcrefaccionesrecu) from crefaccionesrecu where empresa ='" + empresa + "'");
            if (idContinuo > 0)
            {
                codigo = inicioCodigo + Convert.ToString(valorinicial + idContinuo);
            }
            else
            {
                codigo = inicioCodigo + valorinicial;
            }

            return codigo.ToString();
        }

        private void txtnombrereFaccionRep_KeyPress(object sender, KeyPressEventArgs e)
        {
          v.enGeneral(e);
        }

        private void txtContasenna_Validated(object sender, EventArgs e)
        {
            obtenerNombre();
        }

        public void cargaEcoBusq(ComboBox cmbRecibe)
        {
           // cmbEconomico.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';select convert(idunidad,char) as idunidad, convert(concat(t2.identificador,LPAD(consecutivo,4,'0'),'-', descripcioneco),char) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas order by eco");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "--SELECCIONE ECONÓMICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbRecibe.DisplayMember = "eco";
            cmbRecibe.ValueMember = "idunidad";
            cmbRecibe.DataSource = dt;
        }

        private void txtMotivo_TextChanged(object sender, EventArgs e)
        {
            v.mayusculas(txtMotivo.Text);
        }

        private void cmbFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFamilia.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idfamilia as id, UPPER(descripcionFamilia) as descr FROM cfamilias WHERE familiafkcnfamilias='" + cmbFamilia.SelectedValue + "' and status='1' and empresa='" + empresa + "'", cmbDescripcion, "id", "descr", "-- seleccione una descripcion --");
                cmbDescripcion.Enabled = true;
            }
            else
            {
                cmbDescripcion.DataSource = null;
                cmbDescripcion.Enabled = false;
            }
        }

        private void cmbDescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUnidadMedida.Text = "Unidad \n de \n Medida: " + v.getaData("SELECT Simbolo FROM cunidadmedida WHERE idunidadmedida=(SELECT umfkcunidadmedida FROM cfamilias WHERE idfamilia='" + cmbDescripcion.SelectedValue + "' and empresa='" + empresa + "')");
        }

        private void Material_Recuperado_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
        }

        public void CargarMecanico()
        {
            cmbMantenimiento.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';SELECT DISTINCT convert(t2.idPersona, char) id,convert(UPPER(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,''))),char) AS Nombre FROM  cpersonal as t2  where t2.empresa='" + empresa + "' and t2.area = '1'  and t2.cargofkcargos != '2' and t2.status = '1' ORDER BY CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')) asc;");
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila2 = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["Nombre"] = "--SELECCIONE MECANICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            nuevaFila2["id"] = 8000000;
            nuevaFila2["Nombre"] = "OTRO".ToUpper();
            dt.Rows.InsertAt(nuevaFila2, dt.Rows.Count + 1);
            cmbMantenimiento.DisplayMember = "id";
            cmbMantenimiento.ValueMember = "Nombre";
            cmbMantenimiento.DataSource = dt;

        }
        public void obtenerNombre()
        {
            string datosRetorna = v.obtenerNombre(txtContasenna.Text, empresa);
            if (!string.IsNullOrWhiteSpace(datosRetorna))
            {
                string[] recortar = datosRetorna.Split('/');
                lblNombre.Text = recortar[0].ToString();
                idEntrega = Convert.ToInt32(recortar[1].ToString());

            }
            else
            {
                MessageBox.Show("La contraseña de almacenista ingresada es incorrecta", "CONTRASEÑA INCORRECTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContasenna.Focus();
                txtContasenna.Clear();
            }
        }
        void iniFamilias()
        {
            v.iniCombos("SELECT idcnfamilia as idfamilia,familia FROM cnfamilias WHERE status='1' and empresa='" + empresa + "' ORDER BY Familia ASC", cmbFamilia, "idfamilia", "familia", "-- SELECCIONE FAMILIA --");

        }

        private void tbrefaccion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tbrefaccion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string codigo = tbrefaccion.Rows[e.RowIndex].Cells[0].Value.ToString();
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';select concat(convert(coderefaccRecu,char),'|',convert(nomrefaccion,char),'|',convert(existencias,char),'|',convert(ecofkcunidades,char),'|',(Select x4.idcnFamilia from  cfamilias as x2  inner join cnfamilias as x4 on x4.idcnFamilia = x2.familiafkcnfamilias where x2.idfamilia=desfamilicnfamilias),'|',convert(desfamilicnfamilias,char),'|',convert(usuarioaltafkcpersonal,char),'|',convert(usuariomantenimiento,char),'|',convert(descripcion,char), '|', (Select convert(x1.Simbolo,char) from cunidadmedida as x1 inner join cfamilias as x2 on x1.idunidadmedida = x2.umfkcunidadmedida inner join cnfamilias as x4 on x4.idcnFamilia = x2.familiafkcnfamilias where x2.idfamilia=desfamilicnfamilias)) from crefaccionesrecu  where coderefaccRecu = '" + codigo + "'  and empresa ='" + empresa + "'").ToString().Split('|');
            txtcodrefaccionRep.Text = datos[0].ToString();
            nombreanterior = txtnombrereFaccionRep.Text = datos[1].ToString();
            cantidadanterior = txtCantidad.Text = datos[2].ToString();
            cmbEconomico.SelectedValue = Convert.ToInt32(datos[3].ToString());
            economicoanterior = Convert.ToInt32(datos[3].ToString());
            cmbFamilia.SelectedValue = Convert.ToInt32(datos[4].ToString());
            cmbDescripcion.SelectedValue = Convert.ToInt32(datos[5].ToString());
            familiaanterior = Convert.ToInt32(datos[5].ToString());
            //cmbFamilia.SelectedValue = Convert.ToInt32(datos[5].ToString());
             cmbMantenimiento.SelectedValue = datos[7].ToString();
            mecanicoanterior = datos[7].ToString();
            motivoanterior = txtMotivo.Text = datos[8].ToString();
            lblUnidadMedida.Text = datos[9].ToString();
            pdelref.Visible = pCancelar.Visible = true;
            txtSalidas.Visible = lblsalida.Visible = cmbMotivos.Visible = lblMotivo.Visible = lblSalidaC.Visible =  true;

            cargaEcoBusq(cmbBuscarUnidad);

        }

        private void validcacionNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsave_Click(null, e);
            else
            {
                TextBox txtKilometraje = sender as TextBox;
                char signo_decimal = (char)46;
                if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == 46 || e.KeyChar == 44)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("Solo se aceptan: numéros y ( , ) en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (e.KeyChar == 46)
                {
                    if (txtKilometraje.Text.LastIndexOf(signo_decimal) >= 0)
                    {
                        e.Handled = true; // Interceptamos la pulsación 
                    }
                }
            }
        }

        private void cmbMotivos_DrawItem(object sender, DrawItemEventArgs e)
        {

            v.combos_DrawItem(sender, e);
        }

        void inifamiliasBusq()
        {
            v.iniCombos("SELECT idcnfamilia as idfamilia,UPPER(familia) as familia FROM cnfamilias where empresa='" + empresa + "' ORDER BY Familia ASC", cbfamiliabusq, "idfamilia", "familia", "-- SELECCIONE FAMILIA --");
        }
        void consultaGeneral(string cadena)
        {
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';select t1.coderefaccRecu as 'CODIGO DE REFACCION', t1.nomrefaccion as 'NOMBRE DE LA REFACCION', t1.existencias as 'EXISTENCIA',(Select x1.Simbolo from cunidadmedida as x1 inner join cfamilias as x2 on x1.idunidadmedida = x2.umfkcunidadmedida inner join cnfamilias as x4 on x4.idcnFamilia = x2.familiafkcnfamilias where x2.idfamilia=t1.desfamilicnfamilias) as 'UNIDAD MEDIA',(select x2.consecutivo from cunidades as x2  where x2.idunidad =t1.ecofkcunidades) as 'UNIDAD DE DONDE SE RECUPERO',x3.Familia as 'FAMILIA',  t1.fechaHoraAlta as 'FECHA/HORA DE ALTA', (select concat(coalesce(a1.ApPaterno, ''), ' ', coalesce(a1.ApMaterno, ''), ' ',coalesce(a1.nombres,'')) from cpersonal as a1 where a1.idPersona = t1.usuarioaltafkcpersonal) as 'ALMACENISTA QUE RECIBE', t1.usuariomantenimiento  as 'MECANICO QUE ENTREGA', t1.descripcion as 'MOTIVO DE RECUPERACION' from crefaccionesrecu as t1 inner join cfamilias as x2 on x2.idfamilia =  t1.desfamilicnfamilias inner join cnfamilias as x3 on x2.familiafkcnfamilias = x3.idcnFamilia" + cadena );
            if (dt.Rows.Count > 0)
            {
                tbrefaccion.DataSource = dt;

            }
            else
            {
                MessageBox.Show("No Hay Datos Que Mostrar", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }


        }
        void Limpiar()
        {
            txtCantidad.Text = txtcodrefaccionRep.Text = txtContasenna.Text = lblNombre.Text = txtContasenna.Text = txtMotivo.Text = txtnombrereFaccionRep.Text = txtCodigobusq.Text = txtnombrereFaccionbusq.Text = txtSalidas.Text ="";
            cmbDescripcion.DataSource = null;
            cmbEconomico.SelectedValue = cmbFamilia.SelectedValue = cmbMantenimiento.SelectedValue = cbfamiliabusq.SelectedValue = cmbMotivos.SelectedValue = cmbBuscarUnidad.SelectedValue = 0;
            pCancelar.Visible = pdelref.Visible = btnCargar.Visible = btnExcel.Visible = false;
            consultaGeneral(" where t1.empresa = '" + empresa + "' and t1.status = 1 limit 10 ;");

        }
        void cadenaCodigo()
        {
            if (empresa == 2)
            {
                inicioCodigo = "RECUVHF";
            }
            else
            {
                inicioCodigo = "RECUTRI";
            }
        }
        void desactivar(string cadena)
        {
            v.c.insertar(cadena);
        }
        void exportar_excel()
        {
            if (tbrefaccion.Rows.Count > 0)
            {
                isexporting = true;
                dt = (DataTable)tbrefaccion.DataSource;
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
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    h.Range rng = (h.Range)sheet.Cells[1, i];
                    sheet.Cells[1, i] = dt.Columns[i].ColumnName.ToUpper();
                    rng.Interior.Color = System.Drawing.Color.Crimson;
                    rng.Borders.Color = System.Drawing.Color.Black;
                    rng.Font.Color = System.Drawing.Color.White;
                    rng.Cells.Font.Name = "Calibri";
                    rng.Cells.Font.Size = 12;
                    rng.Font.Bold = true;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        try
                        {
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j];
                            sheet.Cells[i + 2, j] = dt.Rows[i][j].ToString();
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
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void Expota_PDF()
        {

            //Código para generación de archivo pdf
            byte[] img = null;
            string nombreHoja = "";
            //string[] cadenafolio = FolioR.Split('-');
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(20f, 20f, 10f, 10f);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ValidateNames = true;
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Orden De Compra";
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
                    iTextSharp.text.Font arial12 = FontFactory.GetFont("Arial", 12, BaseColor.BLACK);
                    doc.Open();
                    if (empresa == 2)
                    {
                        img = Convert.FromBase64String(v.tri);
                        nombreHoja = "TRI VEHICULOS FUNCIONALES S.A. DE C.V.";
                    }
                    else if (empresa == 3)
                    {
                        img = Convert.FromBase64String(v.trainsumos);
                        nombreHoja = "TRANSINSUMOS S.A. DE C.V.";
                    }

                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 100 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    Chunk chunk = new Chunk("ECATEPEC ESTADO DE MEXICO A " + System.DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy"), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    doc.Add(imagen);
                    doc.Add(new Paragraph(chunk));
                    doc.Add(new Paragraph("                                    "));
                    PdfPTable tabla = new PdfPTable(2);
                    tabla.DefaultCell.Border = 0;
                    tabla.WidthPercentage = 100;
                    /*
                     t1.Folio,'|',UNIDAD,'|',FECHA Y HORA,'|',MECANICO,'|',FECHAHORA ENTREGA,'|',PERSONA QUE ENTREGA,'|',FolioFactura,'|',ObservacionesTrans FolioR
                     */
                    tabla.AddCell(v.valorCampo(nombreHoja.ToString(), 2, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n Economico: " + cmbBuscarUnidad.Text, 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("Refaccion Entregada:", 2, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n " + txtcodrefaccionRep.Text + "\t" + txtnombrereFaccionRep.Text + "\t Cantidad: " + txtCantidad.Text + " " + lblUnidadMedida.Text, 2, 1, 0, arial12));
                    // tabla.AddCell(v.valorCampo(proveedor.ToString(), 2, 1, 0, arial));
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
                    // GenerarDocumento(doc);
                    doc.Add(new Paragraph("\n\n" + lblNombre.Text , FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL)));
                    doc.Add(new Paragraph("\n_________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("\n\nRECIBE" , FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("\n_________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
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

    }
}
