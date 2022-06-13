using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using System.Drawing.Imaging;
using System.IO;

namespace controlFallos
{
    public partial class nuevaRefaccion : Form
    {
        
        validaciones v;
        int idUsuario, status, empresa, area;
        string idRefaccionTemp, codrefAnterior, nomrefanterior, modrefanterior, marcaAnterior, nivelAnterior, charolaAnterior, ultimacantidad, ultimoabastecimiento, mediaAnterior, abastecimientoAnterior, descripcionAnterior, CostoAnterior, proveedorAnterior,Destino = "";
        public bool editar { private set; get; }
        Thread exportar, th;
        //decimal ultimacantidad;
        public string idRefaccionMediaAbast;
        bool yaAparecioMensaje = false;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        void getCambios(object sender, EventArgs e)
        {
            try
            {
                if (editar)
                {
                    string codrefaccion = txtcodrefaccion.Text.Trim();
                    string nomrefaccion = v.mayusculas(txtnombrereFaccion.Text.Trim().ToLower());
                    string modrefaccion = txtmodeloRefaccion.Text.Trim();
                    int marca = 0; if (cbmarcas.DataSource != null) marca = Convert.ToInt32(cbmarcas.SelectedValue);
                    decimal media = 0; try { if (!string.IsNullOrWhiteSpace(notifmedia.Text)) media = Convert.ToDecimal(notifmedia.Text); } catch { media = 0; }
                    decimal abastecimiento = 0; try { if (!string.IsNullOrWhiteSpace(notifabastecimiento.Text)) abastecimiento = Convert.ToDecimal(notifabastecimiento.Text); } catch { media = 0; }
                    int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
                    decimal cantidadAlmacen = 0; try { if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim()); } catch { cantidadAlmacen = 0; }
                    decimal costoUni = 0; try { if (!string.IsNullOrWhiteSpace(txtCostoUni.Text.Trim())) costoUni = Convert.ToDecimal(txtCostoUni.Text.Trim()); } catch { costoUni = 0; }
                    string observaciones = v.mayusculas(txtdesc.Text.Trim().ToLower());

                    if (status == 1 && 
                        
                        (!string.IsNullOrWhiteSpace(txtcodrefaccion.Text) ||
                        !string.IsNullOrWhiteSpace(txtnombrereFaccion.Text) ||
                        !string.IsNullOrWhiteSpace(txtmodeloRefaccion.Text) ||
                        cbfamilia.SelectedIndex > 0 ||
                        marca > 0 ||
                        (Convert.ToInt32((cbpasillo.SelectedValue ?? "0"))) > 0 ||
                        Convert.ToInt32((cbnivel.SelectedValue ?? "0")) > 0 ||
                        Convert.ToInt32((cbanaquel.SelectedValue ?? "0")) > 0 ||
                        Convert.ToInt32((cmbPROVEEDOR.SelectedValue ?? "0")) > 0 ||
                        charolafkccharolas > 0 || media > 0 || abastecimiento > 0) || 
                        (!codrefAnterior.Equals(codrefaccion) || !nomrefanterior.Equals(nomrefaccion) || !modrefanterior.Equals(modrefaccion) || (proxabastecimiento.Value.ToString("dd / MM / yyyy") != DateTime.Parse(ultimoabastecimiento).ToString("dd / MM / yyyy")) || (marcaAnterior != cbmarcas.SelectedValue.ToString()) || (charolaAnterior != charolafkccharolas.ToString()) || cantidadAlmacen > 0 || ((Convert.ToDecimal(mediaAnterior) != media || Convert.ToDecimal(abastecimientoAnterior) != abastecimiento)) || (Convert.ToDecimal(CostoAnterior) != costoUni) || !descripcionAnterior.Equals(v.mayusculas(txtdesc.Text.Trim().ToLower())) || (marcaAnterior != cbmarcas.SelectedValue.ToString())))
                    {
                        btnsave.Visible = lblsave.Visible = true;
                    }
                    else
                    {
                        btnsave.Visible = lblsave.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public nuevaRefaccion(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.idUsuario = idUsuario;
            cbfamilia.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbmarcas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbnivel.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbpasillo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbanaquel.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbcharola.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbPROVEEDOR.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbfamiliabusq.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbmarcasbusq.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbrefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbrefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbdescfamilia.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbMoneda.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area; DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbrefaccion.ColumnHeadersDefaultCellStyle = d;
            v.comboswithuot(cbmarcasbusq, new string[] { "--SELECCIONA TIPO--", "NUEVA", "REMANOFACTURADA", "PRODUCCION" });
            v.comboswithuot(cmbTipo, new string[] { "--SELECCIONA TIPO--", "TRANSINSUMOS", "TRANSMASIVO", "PRODUCCION"});
            
            if (empresa == 2)
            {
                Destino = @"\\192.168.1.67\Imagenes\REFACCIONES\TRI\";
            }
            if (empresa == 3)
            {
                Destino = @"\\192.168.1.67\Imagenes\REFACCIONES\INSUMOS\";
            }

        }
        public nuevaRefaccion(int idUsuario, string idrefaccion)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.idRefaccionMediaAbast = idrefaccion;
        }
        public void establecerPrivilegios()
        {
            try
            {
                string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "catRefacciones")).ToString().Split('/');
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
            catch (Exception ex)
            {
                MessageBox.Show("No tienes permisos para acceder al modulo".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        void mostrar()
        {
            if (Pinsertar)
            {
                gbaddrefaccion.Visible = true;
            }
            if (Pconsultar)
            {
                gbconsultar.Visible = true;
            }
            if (Peditar)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
        }
        void iniubicaciones()
        {
            v.iniCombos("SELECT idpasillo,UPPER(pasillo) AS pasillo FROM cpasillos WHERE status='1' and empresa='"+empresa+"' ORDER BY pasillo ASC", cbpasillo, "idpasillo", "pasillo", "-- SELECCIONE PASILLO --");
        }
        void iniFamilias()
        {
            v.iniCombos("SELECT idcnfamilia as idfamilia,familia FROM cnfamilias WHERE status='1' and empresa='" + empresa + "' ORDER BY Familia ASC", cbfamilia, "idfamilia", "familia", "-- SELECCIONE FAMILIA --");

        }
        void inifamiliasBusq()
        {
            v.iniCombos("SELECT idcnfamilia as idfamilia,UPPER(familia) as familia FROM cnfamilias where empresa='" + empresa + "' ORDER BY Familia ASC", cbfamiliabusq, "idfamilia", "familia", "-- SELECCIONE FAMILIA --");
        }
        void iniMonedaCambio()
        {
            v.iniCombos("select idTipoCambio as id, Simbolo from ctipocambio order by simbolo desc", cmbMoneda, "id", "Simbolo", "--MONEDA--");
        }
        private void nuevaRefaccion_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar)
            {
                iniFamilias();
                inifamiliasBusq();
                iniubicaciones();
                iniMonedaCambio();
                mostrar_proveedor(empresa);


                proxabastecimiento.Value = DateTime.Now;
            }
            if (Pconsultar)
            {
                insertarRefacciones();
                inifamiliasBusq();
            }
            if (!string.IsNullOrWhiteSpace(idRefaccionMediaAbast))
            {
                BuscarRefaccion();
            }
            tbrefaccion.ClearSelection();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(SplashScreen))
                {
                    if (frm.InvokeRequired)
                    {

                        validaciones.delgado dm = new validaciones.delgado(v.cerrarForm);

                        Invoke(dm, frm);
                    }

                    break;
                }
            }
            th.Abort();
        }
        public void BuscarRefaccion()
        {
            for (int i = 0; i < tbrefaccion.Rows.Count; i++)
            {
                if (tbrefaccion.Rows[i].Cells[0].Value.ToString().Equals(idRefaccionMediaAbast))
                {
                    tbrefaccion.Rows[i].Selected = true;
                    tbrefaccion.FirstDisplayedScrollingRowIndex = i;
                }
            }
        }

          void button7_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text.Trim()) || cbfamiliabusq.SelectedIndex > 0 || cbmarcasbusq.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtCodigobusq.Text.Trim()))
            {
                tbrefaccion.DataSource = null;
                string sql = @"SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT t1.idrefaccion,UPPER(t1.codrefaccion) AS 'CÓDIGO DE REFACCIÓN',UPPER(t1.nombreRefaccion) AS 'NOMBRE DE REFACCIÓN',UPPER(t1.modeloRefaccion) AS 'MODELO DE REFACCIÓN',UPPER(DATE_FORMAT(t1.proximoAbastecimiento, '%d / %M / %Y')) AS 'PRÓXIMO ABASTECIMIENTO',UPPER(t3.Familia) as 'FAMILIA',UPPER(t2.descripcionFamilia) AS 'DESCRIPCIÓN DE FAMILIA', UPPER(t9.Simbolo) as 'UNIDAD DE MEDIDA',UPPER(t8.marca) as 'MARCA',UPPER(CONCAT('PASILLO: ',t7.pasillo,';NIVEL: ',t6.nivel,';ANAQUEL: ',t5.anaquel,';CHAROLA: ',t4.charola)) as 'UBICACIÓN',if(t1.maximo is null, '0', t1.maximo) as 'MAXIMO',t1.existencias as 'EXISTENCIAS',t1.media as 'MEDIA',t1.abastecimiento as 'ABASTECIMIENTO',UPPER(t1.descripcionRefaccion) as 'DESCRIPCIÓN',UPPER(CONCAT(coalesce(t10.nombres,''),' ',coalesce(t10.ApPaterno,''),' ',coalesce(t10.ApMaterno,''))) AS 'USUARIO QUE DIÓ DE ALTA',UPPER(DATE_FORMAT(t1.fechaHoraalta,'%W, %d de %M del %Y  a las %H:%i:%s')) AS 'FECHA/HORA ALTA',IF(t1.status = 1, 'ACTIVO', 'NO ACTIVO') as 'ESTATUS',t2.familiafkcnfamilias,t2.idfamilia,t8.idmarca,t7.idpasillo,t6.idnivel,t5.idanaquel,t4.idcharola, UPPER(coalesce(convert(t1.CostoUni,char),'0')) as 'COSTO REFACCIÓN',convert(t12.Simbolo,char) as 'MONEDA',UPPER(coalesce(convert(if(t11.empresa = '',concat(t11.aPaterno, ' ', t11.aMaterno, ' ', t11.nombres), t11.empresa),char),'0')) as 'PROVEEDOR', if(t1.Tipo = 1, 'NUEVA', if(t1.Tipo = 2, 'REMANOFAACTURADA', if(t1.Tipo=3,'PRODUCCION',0))) as TIPO FROM crefacciones as t1 INNER JOIN ccharolas as t4 ON t1.charolafkcharolas=t4.idcharola INNER JOIN canaqueles as t5 ON t4.anaquelfkcanaqueles = t5.idanaquel INNER JOIN cniveles as t6 ON t5.nivelfkcniveles=t6.idnivel INNER JOIN cpasillos as t7 On t6.pasillofkcpasillos=t7.idpasillo INNER JOIN cmarcas as t8 ON t1.marcafkcmarcas=t8.idmarca INNER JOIN cfamilias as t2 ON t8.descripcionfkcfamilias = t2.idfamilia INNER JOIN cnfamilias as t3 ON t2.familiafkcnfamilias=t3.idcnfamilia INNER JOIN cunidadmedida as t9 ON t2.umfkcunidadmedida=t9.idunidadmedida INNER JOIN cpersonal as t10 ON t1.usuarioaltafkcpersonal=t10.idpersona inner join cproveedores as t11 on t1.proveedrofkCProveedores = t11.idproveedor inner join ctipocambio as t12 on t1.tipoMonedafkCTipoCambio = t12.idTipoCambio ";
                string wheres = "";
                if (!string.IsNullOrWhiteSpace(txtCodigobusq.Text))
                {
                    if (wheres == "")
                    {
                        wheres = "WHERE codrefaccion LIKE '" + txtCodigobusq.Text + "%' ";
                    }
                    else
                    {
                        wheres += " AND codrefaccion LIKE '" + txtCodigobusq.Text + "%' ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text))
                {
                    if (wheres == "")
                    {
                        wheres = "WHERE nombreRefaccion LIKE '" + txtnombrereFaccionbusq.Text + "%' ";
                    }
                    else
                    {
                        wheres += " AND nombreRefaccion LIKE'" + txtnombrereFaccionbusq.Text + "%' ";
                    }
                }
                if (cbfamiliabusq.SelectedIndex > 0)
                {
                    if (wheres == "")
                    {
                        wheres = " WHERE  t3.idcnfamilia='" + cbfamiliabusq.SelectedValue + "' ";
                    }
                    else
                    {
                        wheres += " AND t3.idcnfamilia ='" + cbfamiliabusq.SelectedValue + "' ";
                    }
                }
                if (cbmarcasbusq.SelectedIndex > 0)
                {
                    if (wheres == "")
                    {
                        wheres = "WHERE t1.Tipo = '" + cbmarcasbusq.SelectedValue + "'";
                    }
                    else
                    {
                        wheres += " AND  t1.Tipo = '" + cbmarcasbusq.SelectedValue + "'";
                    }
                }
                sql += wheres + " and t1.empresa='" + empresa + "' and t1.empresa='" + empresa + "' ORDER BY t1.codrefaccion";
                txtCodigobusq.Clear();
                txtnombrereFaccionbusq.Clear();
                cbfamiliabusq.SelectedIndex = 0;
                cbmarcasbusq.SelectedIndex = 0;
                DataTable dt = (DataTable)v.getData(sql);
                int filas = dt.Rows.Count;
                if (filas > 0)
                {
                    pActualizar.Visible = true;
                    tbrefaccion.DataSource = dt;
                    ocultarCeldas();
                    tbrefaccion.ClearSelection();
                    pActualizar.Visible = true;
                    if (!est_expor)
                    {
                        btnExcel.Visible = true;
                    }
                    LblExcel.Visible = true;
                }
                else
                {
                    MessageBox.Show("No se encontraron resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    insertarRefacciones();
                }
                v.c.dbcon.Close();
                tbrefaccion.ClearSelection();

            }
            else
            {
                MessageBox.Show("Seleccione un Criterio de Búsqueda", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        bool est_expor = false;
        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            insertarRefacciones();
        }

        private void cbanaquel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM ccharolas where anaquelfkcanaqueles='" + cbanaquel.SelectedValue + "' and empresa='"+empresa+"'")) > 0)
            {

                v.iniCombos("SELECT idcharola,UPPER(charola) AS charola FROM ccharolas WHERE status='1' and anaquelfkcanaqueles= '" + cbanaquel.SelectedValue + "' and empresa='" + empresa + "' ORDER BY charola ASC", cbcharola, "idcharola", "charola", "--SELECCIONE UN ANAQUEL");
                cbcharola.Enabled = true;
            }
            else
            {

                cbcharola.DataSource = null;
                cbcharola.Enabled = false;
            }
        }

        private void txtcodrefaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = v.codrefaccionValido(txtcodrefaccion.Text);
        }

        private void txtnombrereFaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void txtmodeloRefaccion_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = v.modrefaccionValido(txtmodeloRefaccion.Text);
        }

        private void cbfamilia_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void tbrefaccion_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }



        private void cbmarcas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbnivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM canaqueles where nivelfkcniveles ='" + cbnivel.SelectedValue + "' and empresa='" + empresa + "'")) > 0)
            {

                v.iniCombos("SELECT idanaquel,UPPER(anaquel) AS anaquel FROM canaqueles WHERE status='1' and nivelfkcniveles= '" + cbnivel.SelectedValue + "' and empresa='" + empresa + "' ORDER BY anaquel ASC", cbanaquel, "idanaquel", "anaquel", "--SELECCIONE UN ANAQUEL");
                cbanaquel.Enabled = true;
            }
            else
            {

                cbanaquel.DataSource = null;
                cbanaquel.Enabled = false;
            }
        }

        private void cbum_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbcharola_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gbaddrefaccion_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            insertarRefacciones();
            txtnombrereFaccionbusq.Clear();
            cbfamiliabusq.SelectedIndex = 0;
            cbmarcasbusq.SelectedIndex = 0;
            pActualizar.Visible = false;
            esta_exportando();
        }
        void esta_exportando()
        {
            if (!LblExcel.Text.Equals("Exportando"))
            {
                btnExcel.Visible = LblExcel.Visible = false;
            }
            else
            {
                exportando = true;
            }
        }
        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox txtCantidad = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(txtCantidad.Text.Trim()))
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

        private void validcacionNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button11_Click(null, e);
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

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtcodrefaccion_Validating_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void cbfamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbfamilia.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idfamilia as id, UPPER(descripcionFamilia) as descr FROM cfamilias WHERE familiafkcnfamilias='" + cbfamilia.SelectedValue + "' and status='1' and empresa='" + empresa + "'", cbdescfamilia, "id", "descr", "-- seleccione una descripcion --");
                cbdescfamilia.Enabled = true;
            }
            else
            {
                cbdescfamilia.DataSource = null;
                cbdescfamilia.Enabled = false;
            }
        }

        private void pExistencias_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gbaddrefaccion_Enter(object sender, EventArgs e)
        {

        }

        private void cbdescfamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbdescfamilia.SelectedIndex > 0)
            {
                lblum.Text = "Unidad de Medida: " + v.getaData("SELECT Simbolo FROM cunidadmedida WHERE idunidadmedida=(SELECT umfkcunidadmedida FROM cfamilias WHERE idfamilia='" + cbdescfamilia.SelectedValue + "' and empresa='" + empresa + "')");
                if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cmarcas WHERE descripcionfkcfamilias='" + cbdescfamilia.SelectedValue + "'")) > 0)
                {
                    v.iniCombos("SELECT idmarca as id, UPPER(marca) as marca FROM cmarcas WHERE descripcionfkcfamilias='" + cbdescfamilia.SelectedValue + "' AND status='1' and empresa='" + empresa + "'", cbmarcas, "id", "marca", "--SELECCIONE UNA MARCA--");
                    cbmarcas.Enabled = true;
                }
                else
                {
                    cbmarcas.DataSource = null;
                    cbmarcas.Enabled = false;
                }
            }
            else
            {
                lblum.Text = null;
                cbmarcas.DataSource = null;
                cbmarcas.Enabled = false;
            }
        }

        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cniveles where pasillofkcpasillos ='" + cbpasillo.SelectedValue + "' and empresa='" + empresa + "'")) > 0)
            {
                v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE status='1' and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "' and empresa='" + empresa + "' ORDER BY nivel ASC", cbnivel, "idnivel", "nivel", "--SELECCIONE UN NIVEL");
                cbnivel.Enabled = true;
            }
            else
            {

                cbnivel.DataSource = null;
                cbnivel.Enabled = false;
            }
        }

        private void txtcodrefaccion_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button11_Click(null, e);
            else
                v.enGeneral(e);
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {

            string codrefaccion = txtcodrefaccion.Text.Trim();
            string nomrefaccion = v.mayusculas(txtnombrereFaccion.Text.Trim().ToLower());
            string modrefaccion = txtmodeloRefaccion.Text.Trim();
            int marca = 0; if (cbmarcas.DataSource != null) marca = Convert.ToInt32(cbmarcas.SelectedValue);
            decimal media = 0; try { if (!string.IsNullOrWhiteSpace(notifmedia.Text)) media = Convert.ToDecimal(notifmedia.Text); } catch { media = 0; }
            decimal abastecimiento = 0; try { if (!string.IsNullOrWhiteSpace(notifabastecimiento.Text)) abastecimiento = Convert.ToDecimal(notifabastecimiento.Text); } catch { media = 0; }
            int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
            decimal cantidadAlmacen = 0; try { if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim()); } catch { cantidadAlmacen = 0; }
            string observaciones = v.mayusculas(txtdesc.Text.Trim().ToLower());
            if (status == 1 && (!string.IsNullOrWhiteSpace(txtcodrefaccion.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccion.Text) && !string.IsNullOrWhiteSpace(txtmodeloRefaccion.Text) && cbfamilia.SelectedIndex > 0 && marca > 0 && (Convert.ToInt32((cbpasillo.SelectedValue ?? "0"))) > 0 && Convert.ToInt32((cbnivel.SelectedValue ?? "0")) > 0 && Convert.ToInt32((cbanaquel.SelectedValue ?? "0")) > 0 && charolafkccharolas > 0 && media > 0 && abastecimiento > 0) && (!codrefAnterior.Equals(codrefaccion) || !nomrefanterior.Equals(nomrefaccion) || !modrefanterior.Equals(modrefaccion) || (proxabastecimiento.Value.ToString("dd / MM / yyyy") != DateTime.Parse(ultimoabastecimiento).ToString("dd / MM / yyyy")) || (marcaAnterior != cbmarcas.SelectedValue.ToString()) || (charolaAnterior != charolafkccharolas.ToString()) || cantidadAlmacen > 0 || ((Convert.ToDecimal(mediaAnterior) != media || Convert.ToDecimal(abastecimientoAnterior) != abastecimiento)) || !descripcionAnterior.Equals(v.mayusculas(txtdesc.Text.Trim().ToLower()))))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button11_Click(null, e);
                    label40.Visible = label41.Visible = txtFolioF.Visible = false;
                }
                else
                {
                    limpiar();
                }
            }
            else
            {

                limpiar();
            }
        }

        private void cbfamiliabusq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbfamiliabusq.SelectedIndex > 0)
            {
               /* if (cbmarcasbusq.SelectedIndex == 0)
                {
                    v.iniCombos("select t1.idmarca as id,UPPER(t1.marca) as marca from cmarcas as t1 INNER JOIN cfamilias as t2 ON t1.descripcionfkcfamilias = t2.idfamilia INNER JOIN cnfamilias as t3 ON t2.familiafkcnfamilias=t3.idcnfamilia WHERE t3.idcnfamilia='" + cbfamiliabusq.SelectedValue + "' and t2.empresa='" + empresa + "'", cbmarcasbusq, "id", "marca", "--SELECCIONE UNA MARCA--");
                }*/
            }
            else
            {
               // v.iniCombos("select DISTINCT t1.idmarca as id,UPPER(t1.marca) as marca from cmarcas as t1 where empresa='" + empresa + "' GROUP BY t1.marca", cbmarcasbusq, "id", "marca", "--SELECCIONE UNA MARCA--");
            }
        }

        private void cbmarcasbusq_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btndelcla_Click(object sender, EventArgs e)
        {
             try
            {
                int status;
                string msg;
                if (this.status == 0)
                {
                    status = 1;
                    msg = "Re";
                }
                else
                {
                    status = 0;
                    msg = "Des";
                }

                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación De La Refacción";
                obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 15, obs.lblinfo.Location.Y);
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                    if (v.c.insertar("UPDATE crefacciones SET status ='" + status + "' WHERE  idrefaccion='" + this.idRefaccionTemp + "'"))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones','" + idRefaccionTemp + "','" + msg + "activación de Refacción','" + idUsuario + "',NOW(),'" + msg + "activación de Refacción','" + edicion + "','" + empresa + "','" + area + "')");
                        MessageBox.Show("Se ha " + msg + "activado la Refacción Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtcodrefaccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paracodrefaccion(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateStock up = new updateStock(int.Parse(idRefaccionTemp), empresa, area,v);
            up.Owner = this;
            up.txtstock.Text = (up.stockaNT = stock).ToString();
            up.txtstock.Focus();
            up.ShowDialog();
        }

        public void abrirImagen()
        {
            OpenFileDialog Abrir = new OpenFileDialog();
            Abrir.Filter = "JPG(*.JPG)|*.JPG|BMP(*.BMP)|*.BMP|JPEG(*.JPEG)|*.JPEG";
            if (Abrir.ShowDialog() == DialogResult.OK)
            {
                pbImgRefaccion.Image = Image.FromFile(Abrir.FileName);
                pbImgRefaccion.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        public void pictureOnClick(object sender, EventArgs e)
        {
            abrirImagen();
        }

        public void guardarImagen()
        {
            
            try
            {
                //pictureBox1.Image.Save(Destino, ImageFormat.Jpeg);
                pbImgRefaccion.Image.Save(Destino + txtcodrefaccion.Text + ".Jpg", ImageFormat.Jpeg);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public void traerImagen()
        {
            pbImgRefaccion.Image = Image.FromFile(Path.Combine(Application.StartupPath, Destino + txtcodrefaccion.Text + ".Jpg"));
            pbImgRefaccion.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private void gbbuscar_Enter(object sender, EventArgs e)
        {

        }
        bool activadok = false;
        private void btnExcel_Click(object sender, EventArgs e)
        {
            activadok = true;
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
            //est_expor = true;
            //ThreadStart delegado = new ThreadStart(ExportarExcel);
            //exportar = new Thread(delegado);
            //exportar.Start();

        }
        bool exportando = false;
      
        Thread hiloEx;
        delegate void Loading();
        public void cargando1()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            label35.Text = "EXPORTANDO";
            label35.Visible = true;
            label35.Location = new Point(455, 778);
        }
        delegate void Loading1();
        public void cargando2()
        {
            pictureBox2.Image = null;
            label35.Visible = btnExcel.Visible = true;
            label35.Text = "EXPORTAR";
            if (exportando)
            {
                btnExcel.Visible = false;
                label35.Visible = false;
            }
            exportando = false;
            activadok = false;
            label35.Location = new Point(475, 778);
        }
        delegate void El_Delegado(); delegate void El_Delegado1();
        public void exporta_a_excel()
        {
            if (tbrefaccion.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < tbrefaccion.Columns.Count; i++) if (tbrefaccion.Columns[i].Visible) dt.Columns.Add(tbrefaccion.Columns[i].HeaderText);
                for (int j = 0; j < tbrefaccion.Rows.Count; j++)
                {

                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < tbrefaccion.Columns.Count; i++)
                    {

                        if (tbrefaccion.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = tbrefaccion.Rows[j].Cells[i].Value;
                            indice++;
                        }

                    }
                    dt.Rows.Add(row);
                }
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando1);
                    this.Invoke(delega);
                }

                v.exportaExcel(dt);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando2);
                    this.Invoke(delega);
                }
                _UnidadesExportadas(dt);
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbPROVEEDOR_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        //bool exportando;
        //delegate void El_Delegado();
        //void cargando()
        //{
        //    pictureBox2.Image = Properties.Resources.loader;
        //    btnExcel.Visible = false;
        //    LblExcel.Text = "Exportando";
        //}
        //delegate void El_Delegado1();
        //void cargando1()
        //{
        //    pictureBox2.Image = null;
        //    btnExcel.Visible = true;
        //    if (exportando)
        //    {
        //        LblExcel.Visible = false;
        //        btnExcel.Visible = false;
        //    }
        //    exportando = false;
        //    est_expor = false;
        //    LblExcel.Text = "Exportar";
        //}
        void _UnidadesExportadas(DataTable dt)
        {
            string id;
            int contador = 0;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones','0','";
            foreach (DataRow row in dt.Rows)
            {
                contador++;
                id = v.getaData(string.Format("SELECT idrefaccion FROM crefacciones WHERE codrefaccion='{0}'", row.ItemArray[0])).ToString();
                if (contador < dt.Rows.Count) { id += ";"; }
                sql += id;
            }
            sql += "','" + this.idUsuario + "',NOW(),'Exportación a Excel de Catálogo de Refacciones','" + this.empresa + "','" + this.area + "')";
            MySqlCommand exportacion = new MySqlCommand(sql, v.c.dbconection());
            exportacion.ExecuteNonQuery();
            v.c.dbcon.Close();
        }

        private void cmbTipo_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //void ExportarExcel()
        //{
        //    if (tbrefaccion.Rows.Count > 0)
        //    {
        //        DataTable dt = new DataTable();
        //        for (int i = 0; i < tbrefaccion.Columns.Count; i++) if (tbrefaccion.Columns[i].Visible) dt.Columns.Add(tbrefaccion.Columns[i].HeaderText);
        //        for (int j = 0; j < tbrefaccion.Rows.Count; j++)
        //        {
        //            DataRow row = dt.NewRow();
        //            int indice = 0;
        //            for (int i = 0; i < tbrefaccion.Columns.Count; i++)
        //            {

        //                if (tbrefaccion.Columns[i].Visible)
        //                {
        //                    row[dt.Columns[indice]] = tbrefaccion.Rows[j].Cells[i].Value;
        //                    indice++;
        //                }
        //            }
        //            dt.Rows.Add(row);
        //        }
        //        if (this.InvokeRequired)
        //        {
        //            El_Delegado delega = new El_Delegado(cargando);
        //            this.Invoke(delega);
        //        }

        //        // v.exportaExcel(dt);
        //        if (this.InvokeRequired)
        //        {
        //            El_Delegado1 delega = new El_Delegado1(cargando1);
        //            this.Invoke(delega);
        //        }
        //        _UnidadesExportadas(dt);
        //    }
        //    else
        //    {
        //        MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e){
            v.letrasnumerosdiagonalypunto(e);
        
        
        
        
        }

        private void cantidada_Validated(object sender, EventArgs e){}

        private void gbconsultar_Enter(object sender, EventArgs e)
        {

        }

       
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                   // btnsave.Enabled = false;
                }
                else
                {
                    b_editar();
                    //btnsave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        void  b_editar()
        {
            string codrefaccion = txtcodrefaccion.Text;
            string nombreRefaccion = v.mayusculas(txtnombrereFaccion.Text.ToLower());
            string modeloRefaccion = txtmodeloRefaccion.Text;
            string proxabast = proxabastecimiento.Value.ToString("yyyy/MM/dd");
            string proxabastParaValidacion = proxabastecimiento.Value.ToShortDateString();
            int marca = Convert.ToInt32(cbmarcas.SelectedValue.ToString());
            int proveedor = Convert.ToInt32(cmbPROVEEDOR.SelectedValue);
            int moneda = Convert.ToInt32(cmbMoneda.SelectedValue);
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int Tipo = Convert.ToInt32(cmbTipo.SelectedValue);
            int nivel = 0; if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
            decimal cantidadAlmacen = 0; if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim());
            decimal CostoUni = 0; if (!string.IsNullOrWhiteSpace(txtCostoUni.Text.Trim())) CostoUni = Convert.ToDecimal(txtCostoUni.Text.Trim());
            decimal media = Convert.ToDecimal(notifmedia.Text);
            decimal abastecimiento = Convert.ToDecimal(notifabastecimiento.Text);
            string observa = v.mayusculas(txtdesc.Text.Trim().ToLower());
            string folioFactura = txtFolioF.Text;
            if (!v.formularioRefaciones(codrefaccion, nombreRefaccion, modeloRefaccion, marca, pasillo, nivel, anaquel, charolafkccharolas,proveedor, moneda ,proxabastecimiento.Value, Tipo) && !v.NumericsUpDownRefaccionEdicion(media, abastecimiento))
            {
                if (!v.existeRefaccionActualizar(codrefaccion, codrefAnterior, nombreRefaccion, nomrefanterior, modeloRefaccion, modrefanterior,empresa))
                {
                    if (status == 1)
                    {
                        DialogResult edition = DialogResult.OK;
                        if (mostrarmotivoActualizacion(new string[10, 2] { { codrefAnterior, codrefaccion }, { nomrefanterior, nomrefanterior }, { modrefanterior, modeloRefaccion }, { marcaAnterior, marca.ToString() }, { (charolaAnterior ?? "0"), charolafkccharolas.ToString() }, { Convert.ToDouble(mediaAnterior).ToString(), Convert.ToDouble(media).ToString() }, { Convert.ToDouble(abastecimientoAnterior).ToString(), abastecimiento.ToString() }, { descripcionAnterior, observa }, {CostoAnterior, CostoUni.ToString()}, { (proveedorAnterior ?? "0"), proveedor.ToString() } }))
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            edition = obs.ShowDialog();
                            if (edition == DialogResult.OK)
                            {
                                if (!v.yaExisterf(txtFolioF.Text, codrefaccion.ToString()))
                                {


                                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                    string quees = "";
                                    if (string.IsNullOrWhiteSpace(cantidada.Text)) cantidada.Text = "0.0";
                                    decimal exist = Convert.ToDecimal(cantidada.Text) + Convert.ToDecimal(ultimacantidad);
                                    if (v.c.insertar(@"UPDATE crefacciones SET codrefaccion =LTRIM(RTRIM('" + codrefaccion + "')), nombreRefaccion = LTRIM(RTRIM('" + nombreRefaccion + "')), modeloRefaccion =LTRIM(RTRIM('" + modeloRefaccion + "'))" + (DateTime.Parse(proxabastecimiento.Value.ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd")) ? " , proximoAbastecimiento = '" + proxabast + "'" : "") + ",charolafkcharolas = '" + charolafkccharolas + "', existencias = '" + exist + "', marcafkcmarcas = '" + marca + "', media = '" + media + "', abastecimiento = '" + abastecimiento + "',descripcionRefaccion='" + v.mayusculas(txtdesc.Text.ToLower()) + "', CostoUni='" + CostoUni + "', proveedrofkCProveedores='" + proveedor + "',tipoMonedafkCTipoCambio='" + moneda + "', Tipo = '" + Tipo + "'  WHERE idrefaccion = '" + this.idRefaccionTemp + "'"))
                                    {
                                        if (float.Parse(cantidada.Text) > 0.0 && !string.IsNullOrWhiteSpace(folioFactura))
                                        {
                                            centradas(CostoUni, moneda);
                                        }
                                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones','" + idRefaccionTemp + "','" +codrefAnterior + ";" + nomrefanterior + ";" + modrefanterior + ";" + ultimoabastecimiento + ";" + marcaAnterior + ";" + charolaAnterior + ";" + ultimacantidad + ";" + mediaAnterior + ";" + abastecimientoAnterior + ";" + descripcionAnterior + ";" + CostoAnterior + "','" + idUsuario + "',NOW(),'Actualización de Refacción','" + observaciones + "','" + empresa + "','" + area + "')");

                                         var res3 = v.c.insertar("INSERT INTO cfoliosfactura(codrefaccionfkcrefacciones, Folio, Fecha,  Empresa,usuario) VALUES((select idrefaccion from crefacciones where codrefaccion = '" + codrefaccion + "'), '" + txtFolioF.Text + "', now(), '" + empresa +"','" + idUsuario + "')");
                                        label40.Visible = label41.Visible = txtFolioF.Visible = false;
                                        txtFolioF.Text = "";
                                        if (pbImgRefaccion.Image !=null)
                                        {
                                            guardarImagen();
                                        }
                                    }
                                    if (edition == DialogResult.OK)
                                    {
                                        if (!yaAparecioMensaje) MessageBox.Show("Refacción Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        limpiar();
                                    }
                                }
                            }

                        }
                        else
                        {
                            v.c.insertar("UPDATE crefacciones SET  proximoAbastecimiento = '" + proxabast + "', existencias=existencias+" + (cantidada.Text == "" ? "0" : cantidada.Text) + " WHERE idrefaccion='" + idRefaccionTemp + "'");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("No se Puede Actualizar una Refacción Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        decimal media, abastecimiento;
        void insertar()
        {
            string codrefaccion = txtcodrefaccion.Text;
            string nombreRefaccion = v.mayusculas(txtnombrereFaccion.Text.ToLower());
            string modeloRefaccion = txtmodeloRefaccion.Text;
            string folioFactura = txtFolioF.Text;
            string proxabast = proxabastecimiento.Value.ToString("yyyy/MM/dd");
            int marca = 0; if (cbmarcas.DataSource != null) marca = Convert.ToInt32(cbmarcas.SelectedValue.ToString());
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int proveedor = Convert.ToInt32(cmbPROVEEDOR.SelectedValue);
            int Tipo = Convert.ToInt32(cmbTipo.SelectedValue);
            int moneda = Convert.ToInt32(cmbMoneda.SelectedValue);
            int nivel = 0; if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
            decimal cantidadAlmacen = 0; if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim());

            decimal costoUni = 0; if (!string.IsNullOrWhiteSpace(txtCostoUni.Text.Trim())) costoUni = Convert.ToDecimal(txtCostoUni.Text.Trim());

            if (!string.IsNullOrWhiteSpace(notifmedia.Text)) media = Convert.ToDecimal(notifmedia.Text);
            if (!string.IsNullOrWhiteSpace(notifabastecimiento.Text.Trim())) abastecimiento = Convert.ToDecimal(notifabastecimiento.Text.ToString());
            DateTime paraValidacion = proxabastecimiento.Value;
            if (!v.formularioRefaciones(codrefaccion, nombreRefaccion, modeloRefaccion, marca, pasillo, nivel, anaquel, charolafkccharolas,proveedor, moneda, paraValidacion, Tipo) && !v.existeRefaccion(codrefaccion, nombreRefaccion, modeloRefaccion,empresa) /*&& !v.NumericsUpDownRefaccion(cantidadAlmacen, media, abastecimiento)*/)
            {
                string sql = "INSERT INTO crefacciones(CostoUni, tipoMonedafkCTipoCambio,codrefaccion, nombreRefaccion, modeloRefaccion ";
                if (DateTime.Parse(proxabastecimiento.Value.ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"))) sql += ",proximoAbastecimiento ";
                sql += ", charolafkcharolas, existencias, marcafkcmarcas,fechaHoraalta,usuarioaltafkcpersonal, media, abastecimiento,descripcionRefaccion,empresa,proveedrofkCProveedores, Tipo)  VALUES (LTRIM(RTRIM('" + costoUni + "')), LTRIM(RTRIM('" + moneda + "')),LTRIM(RTRIM('" + codrefaccion + "')),LTRIM(RTRIM('" + nombreRefaccion + "')),LTRIM(RTRIM('" + modeloRefaccion + "'))";
                if (DateTime.Parse(proxabastecimiento.Value.ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd"))) sql += ",'" + proxabast + "'";
                sql += ",'" + charolafkccharolas + "','" + cantidadAlmacen + "','" + marca + "',NOW(),'" + idUsuario + "','" + media + "','" + abastecimiento + "','" + v.mayusculas(txtdesc.Text.ToLower()) + "','"+empresa+"', '" + proveedor + "','" + Tipo + "')";
                if (float.Parse(cantidada.Text) > 0 && !string.IsNullOrEmpty(folioFactura))
                {
                    centradas(costoUni, moneda);
                }
                if (pbImgRefaccion.Image != null)
                {
                    guardarImagen();
                }

                if (v.c.insertar(sql))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones',(SELECT idrefaccion FROM crefacciones WHERE codRefaccion='" + codrefaccion + "' AND nombreRefaccion='" + nombreRefaccion + "' and empresa='" + empresa + "'),'" + codrefaccion + ";" + nombreRefaccion + ";" + modeloRefaccion + ";" + proxabast + ";" + marca + ";" + charolafkccharolas + ";" + cantidadAlmacen + ";" + media + ";" + abastecimiento + ";" + v.mayusculas(txtdesc.Text.ToLower()) + ";" + costoUni + ";" + moneda + "','" + idUsuario + "',NOW(),'Inserción de Refacción','" + empresa + "','" + area + "')");
                    MessageBox.Show("Refacción Agregada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }

            }
        }
        void centradas(decimal costoUni, int moneda)
        {
            var res3 = v.c.insertar("insert into centradasm(refaccionfkCRefacciones,ordcomprafkordencompra,crequicisionfkcrequisicion,FolioFactura, CantidadIngresa, FechaHora, UsuariofkCPersonal, Empresa, proveedor,Costo,tipomonedafkCTipoCambio) value ((Select idRefaccion from crefacciones where codrefaccion ='" + txtcodrefaccion.Text + "' and empresa = '" + empresa + "'), (select max(t1.idOrdCompra) from ordencompra as t1 inner join crequicision as t2 on t1.requicisionfkCRequicision = t2.idcrequicision inner join crefacciones as t3 on t3.idrefaccion = t2.refaccionfkCRefacciones where t3.codrefaccion ='" + txtcodrefaccion.Text + "' and t3.empresa = '" + empresa + "' and (t1.Estatus = 0 or t1.Estatus is null)),(select max(t2.idcrequicision) from crequicision as t2  inner join crefacciones as t3 on t3.idrefaccion = t2.refaccionfkCRefacciones where t3.codrefaccion = '" + txtcodrefaccion.Text + "' and t3.empresa = '" + empresa + "' and(t2.Estatus = 0 or t2.Estatus is null)),'" + txtFolioF.Text + "', '" + cantidada.Text + "',now()," + idUsuario + "," + empresa + ",'" + cmbPROVEEDOR.Text + "','" + costoUni + "','" + moneda + "');");
            string idMaximo = v.getaData("select max(t1.idOrdCompra) from ordencompra as t1 inner join crequicision as t2 on t1.requicisionfkCRequicision = t2.idcrequicision inner join crefacciones as t3 on t3.idrefaccion = t2.refaccionfkCRefacciones where t3.codrefaccion ='" + txtcodrefaccion.Text + "' and t3.empresa = '" + empresa + "' and (t1.Estatus = 0 or t1.Estatus is null)").ToString();
            if (!string.IsNullOrWhiteSpace(idMaximo))
            {
                var rese = v.c.insertar("update ordencompra as t1 left join crequicision as t2 on t1.requicisionfkCRequicision = t2.idcrequicision set t1.Estatus = 1, t2.Estatus = 1 where t1.idOrdCompra = '" + idMaximo + "'");
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                gbaddrefaccion.Text = "Agregar Refacción";
                lblsave.Text = "Guardar";
                txtcodrefaccion.Focus();
            }
            pbImgRefaccion.Image = null;
            txtcodrefaccion.Clear();
            txtnombrereFaccion.Clear();
            txtmodeloRefaccion.Clear();
            txtFolioF.Clear();
            proxabastecimiento.Value = DateTime.Now.Date;
            cbfamilia.SelectedIndex = 0;
            cbpasillo.SelectedIndex = 0;
            cmbTipo.SelectedIndex = 0;
            cantidada.Clear();
            notifmedia.Clear();
            txtdesc.Clear();
            txtCostoUni.Clear();
            notifabastecimiento.Clear();
            pCancelar.Visible = false;
            pdelref.Visible = false;
            yaAparecioMensaje = false;
            if (Pconsultar)
            {
                insertarRefacciones();
            }
            if (Peditar)
            {
                gbaddrefaccion.Visible = false;
            }
            if (Pinsertar)
            {
                gbaddrefaccion.Visible = true;
            }
            idRefaccionTemp = codrefAnterior = nomrefanterior = modrefanterior = marcaAnterior = nivelAnterior = charolaAnterior = ultimoabastecimiento = null;
            btnsave.Visible = lblsave.Visible = true;
            mediaAnterior = abastecimientoAnterior = ultimacantidad = lblexistencias.Text = null;
            pExistencias.Visible = false;
            pStock.Visible = false;
            iniFamilias();
            inifamiliasBusq();
            iniubicaciones();
            iniMonedaCambio();
            mostrar_proveedor(empresa);
            label40.Visible = label41.Visible = txtFolioF.Visible = false;
            txtcodrefaccion.Enabled = txtnombrereFaccion.Enabled = true;
            btnsave.Enabled = true;
            panelM.Visible = false;
        }

        public void insertarRefacciones()
        {
            tbrefaccion.DataSource = null;
            tbrefaccion.DataSource = v.getData("SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT t1.idrefaccion,UPPER(t1.codrefaccion) AS 'CÓDIGO DE REFACCIÓN',UPPER(t1.nombreRefaccion) AS 'NOMBRE DE REFACCIÓN',UPPER(t1.modeloRefaccion) AS 'MODELO DE REFACCIÓN',UPPER(DATE_FORMAT(t1.proximoAbastecimiento, '%d / %M / %Y')) AS 'PRÓXIMO ABASTECIMIENTO',UPPER(t3.Familia) as 'FAMILIA',UPPER(t2.descripcionFamilia) AS 'DESCRIPCIÓN DE FAMILIA', UPPER(t9.Simbolo) as 'UNIDAD DE MEDIDA',UPPER(t8.marca) as 'MARCA',UPPER(CONCAT('PASILLO: ',t7.pasillo,';NIVEL: ',t6.nivel,';ANAQUEL: ',t5.anaquel,';CHAROLA: ',t4.charola)) as 'UBICACIÓN',if(t1.Maximo is null, '0', t1.Maximo) as 'MAXIMO',t1.existencias as 'EXISTENCIAS',t1.media as 'MEDIA',t1.abastecimiento as 'ABASTECIMIENTO',UPPER(t1.descripcionRefaccion) as 'DESCRIPCIÓN',UPPER(CONCAT(coalesce(t10.nombres,''),' ',coalesce(t10.ApPaterno,''),' ',coalesce(t10.ApMaterno,''))) AS 'USUARIO QUE DIÓ DE ALTA',UPPER(DATE_FORMAT(t1.fechaHoraalta,'%W, %d de %M del %Y  a las %H:%i:%s')) AS 'FECHA/HORA ALTA',IF(t1.status = 1, 'ACTIVO', 'NO ACTIVO') as 'ESTATUS',t2.familiafkcnfamilias,t2.idfamilia,t8.idmarca,t7.idpasillo,t6.idnivel,t5.idanaquel,t4.idcharola, UPPER(coalesce(convert(t1.CostoUni,char),'0')) as 'COSTO REFACCIÓN',convert(t12.Simbolo, char) as 'MONEDA',UPPER(coalesce(convert(if(t11.empresa = '',concat(t11.aPaterno, ' ', t11.aMaterno, ' ', t11.nombres), t11.empresa),char),'0')) as 'PROVEEDOR', if(t1.Tipo = 1, 'NUEVA', if(t1.Tipo = 2,'REMANOFACTURADA', '')) as TIPO FROM crefacciones as t1 INNER JOIN ccharolas as t4 ON t1.charolafkcharolas=t4.idcharola INNER JOIN canaqueles as t5 ON t4.anaquelfkcanaqueles = t5.idanaquel INNER JOIN cniveles as t6 ON t5.nivelfkcniveles=t6.idnivel INNER JOIN cpasillos as t7 On t6.pasillofkcpasillos=t7.idpasillo INNER JOIN cmarcas as t8 ON t1.marcafkcmarcas=t8.idmarca INNER JOIN cfamilias as t2 ON t8.descripcionfkcfamilias = t2.idfamilia INNER JOIN cnfamilias as t3 ON t2.familiafkcnfamilias=t3.idcnfamilia INNER JOIN cunidadmedida as t9 ON t2.umfkcunidadmedida=t9.idunidadmedida INNER JOIN cpersonal as t10 ON t1.usuarioaltafkcpersonal=t10.idpersona inner join cproveedores as t11 on t1.proveedrofkCProveedores = t11.idproveedor inner join ctipocambio as t12 on t1.tipoMonedafkCTipoCambio = t12.idTipoCambio where t1.empresa='" + empresa + "' ORDER BY t1.codrefaccion ASC limit 15;");
            //tbrefaccion.DataSource = v.getData("SET NAMES 'utf8';SET lc_time_names = 'es_ES';call karina('" + empresa + "');");
            ocultarCeldas();
            tbrefaccion.ClearSelection();
        }
        void ocultarCeldas()
        {
            tbrefaccion.Columns[24].Visible = tbrefaccion.Columns[23].Visible = tbrefaccion.Columns[22].Visible = tbrefaccion.Columns[21].Visible = tbrefaccion.Columns[20].Visible = tbrefaccion.Columns[19].Visible = tbrefaccion.Columns[18].Visible = tbrefaccion.Columns[17].Visible = tbrefaccion.Columns[0].Visible = false;
            tbrefaccion.Columns[0].Frozen = tbrefaccion.Columns[1].Frozen = tbrefaccion.Columns[2].Frozen = true;

        }
        private void tbrefaccion_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbrefaccion.Columns[e.ColumnIndex].HeaderText == "Estatus".ToUpper())
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                {

                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void tbrefaccion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    btnsave.Enabled = true;
                    string codrefaccion = txtcodrefaccion.Text.Trim();
                    string nomrefaccion = v.mayusculas(txtnombrereFaccion.Text.Trim().ToLower());
                    string modrefaccion = txtmodeloRefaccion.Text.Trim();
                    int marca = 0; if (cbmarcas.DataSource != null) marca = Convert.ToInt32(cbmarcas.SelectedValue);
                    decimal media = 0; try { if (!string.IsNullOrWhiteSpace(notifmedia.Text)) media = Convert.ToDecimal(notifmedia.Text); } catch { media = 0; }
                    decimal abastecimiento = 0; try { if (!string.IsNullOrWhiteSpace(notifabastecimiento.Text)) abastecimiento = Convert.ToDecimal(notifabastecimiento.Text); } catch { media = 0; }
                    int charolafkccharolas = 0; if (cbcharola.DataSource != null) charolafkccharolas = Convert.ToInt32(cbcharola.SelectedValue);
                    decimal cantidadAlmacen = 0; try { if (!string.IsNullOrWhiteSpace(cantidada.Text.Trim())) cantidadAlmacen = Convert.ToDecimal(cantidada.Text.Trim()); } catch { cantidadAlmacen = 0; }
                    int proveedorfkCProveedores = 0; if (cmbPROVEEDOR.DataSource != null) proveedorfkCProveedores = Convert.ToInt32(cmbPROVEEDOR.SelectedValue); 
                    decimal costoRefaccion = 0; try { if (!string.IsNullOrWhiteSpace(txtCostoUni.Text.Trim())) costoRefaccion = Convert.ToDecimal(txtCostoUni.Text.Trim()); } catch { costoRefaccion = 0; }
                    
                    string observaciones = v.mayusculas(txtdesc.Text.Trim().ToLower());
                    if (status == 1 && (!string.IsNullOrWhiteSpace(txtcodrefaccion.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccion.Text) && !string.IsNullOrWhiteSpace(txtmodeloRefaccion.Text) && cbfamilia.SelectedIndex > 0 && marca > 0 && (Convert.ToInt32((cbpasillo.SelectedValue ?? "0"))) > 0 && Convert.ToInt32((cbnivel.SelectedValue ?? "0")) > 0 && Convert.ToInt32((cbanaquel.SelectedValue ?? "0")) > 0 && Convert.ToInt32((cmbPROVEEDOR.SelectedValue ?? "0")) > 0 && charolafkccharolas > 0 && media > 0 && abastecimiento > 0) && (!codrefAnterior.Equals(codrefaccion) || !nomrefanterior.Equals(nomrefaccion) || !modrefanterior.Equals(modrefaccion) || (proxabastecimiento.Value.ToString("dd / MM / yyyy") != DateTime.Parse(ultimoabastecimiento).ToString("dd / MM / yyyy")) || (marcaAnterior != cbmarcas.SelectedValue.ToString()) || (charolaAnterior != charolafkccharolas.ToString()) || cantidadAlmacen > 0 || costoRefaccion > 0 || ((Convert.ToDecimal(mediaAnterior) != media || Convert.ToDecimal(abastecimientoAnterior) != abastecimiento)) || !descripcionAnterior.Equals(v.mayusculas(txtdesc.Text.Trim().ToLower()))))
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            button11_Click(null, e);
                        }
                    }
                    guardarReporte(e);
                    gbaddrefaccion.Visible = true;
                }
                label41.Visible = txtFolioF.Visible = label40.Visible = true;
                txtcodrefaccion.Enabled =  false;
                traerImagen();


            }
            catch (Exception ex){MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);}

        }
        public double stock;
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            iniFamilias();
            inifamiliasBusq();
            try
            {
                cantidada.Clear();
                idRefaccionTemp = tbrefaccion.Rows[e.RowIndex].Cells[0].Value.ToString();
                status = v.getStatusInt(tbrefaccion.Rows[e.RowIndex].Cells[16].Value.ToString());
                pExistencias.Visible = true;
                if (Pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelref.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelref.Text = "Reactivar";
                    }
                    else
                    {
                        btndelref.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelref.Text = "Desactivar";
                    }

                    if (Pinsertar) pCancelar.Visible = true;
                    pdelref.Visible = true;
                }
                if (Peditar)
                {
                    lblexistencias.Text = v.getExistenciasFromIDRefaccion(idRefaccionTemp);
                    //if ((stock = Convert.ToDouble(v.getaData("SELECT existencias FROM crefacciones WHERE idrefaccion='" + idRefaccionTemp + "'"))) > 0)
                    // pStock.Visible = true;
                    //else
                    //pStock.Visible = false;
                    txtcodrefaccion.Text = codrefAnterior = (string)tbrefaccion.Rows[e.RowIndex].Cells[1].Value;
                    txtnombrereFaccion.Text = nomrefanterior = v.mayusculas(tbrefaccion.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    txtmodeloRefaccion.Text = modrefanterior = tbrefaccion.Rows[e.RowIndex].Cells[3].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(tbrefaccion.Rows[e.RowIndex].Cells[4].Value.ToString()))
                        proxabastecimiento.Value = Convert.ToDateTime(ultimoabastecimiento = (string)tbrefaccion.Rows[e.RowIndex].Cells[4].Value);
                    else
                        proxabastecimiento.Value = Convert.ToDateTime(ultimoabastecimiento = DateTime.Today.ToString());
                    marcaAnterior = tbrefaccion.Rows[e.RowIndex].Cells[20].Value.ToString();
                    object familiaAnterior = v.getaData("SELECT t3.idcnfamilia FROM cmarcas as t1 INNER JOIN cfamilias as t2 ON t1.descripcionfkcfamilias=t2.idfamilia INNER JOIN cnfamilias as t3 On t2.familiafkcnfamilias=t3.idcnFamilia WHERE t1.idmarca='" + marcaAnterior + "'and t3.empresa='" + empresa + "'");
                    cbfamilia.SelectedValue = familiaAnterior;
                    if (cbfamilia.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idcnfamilia as idfamilia,familia FROM cnfamilias WHERE (status='1' OR idcnfamilia='" + familiaAnterior + "') and empresa='" + empresa + "' ORDER BY Familia ASC", cbfamilia, "idfamilia", "familia", "-- SELECCIONE FAMILIA --");
                        cbfamilia.SelectedValue = familiaAnterior;
                    }
                    object descripcionnAnterior = v.getaData("SELECT t2.idfamilia FROM cmarcas as t1 INNER JOIN cfamilias as t2 ON t1.descripcionfkcfamilias=t2.idfamilia WHERE t1.idmarca='" + marcaAnterior + "' and t2.empresa='" + empresa + "'");
                    cbdescfamilia.SelectedValue = descripcionnAnterior;
                    if (cbdescfamilia.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idfamilia as id, UPPER(descripcionFamilia) as descr FROM cfamilias WHERE familiafkcnfamilias='" + cbfamilia.SelectedValue + "' and (status='1' OR idfamilia='" + descripcionnAnterior + "') and empresa='" + empresa + "'", cbdescfamilia, "id", "descr", "-- seleccione una descripcion --");
                        cbdescfamilia.Enabled = true;
                        cbdescfamilia.SelectedValue = descripcionnAnterior;
                    }

                    cbmarcas.SelectedValue = marcaAnterior;
                    if (cbmarcas.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idmarca as id, UPPER(marca) as marca FROM cmarcas WHERE descripcionfkcfamilias='" + cbdescfamilia.SelectedValue + "' AND (status='1' OR idmarca='" + marcaAnterior + "') and empresa='" + empresa + "'", cbmarcas, "id", "marca", "--SELECCIONE UNA MARCA--");
                        cbmarcas.SelectedValue = marcaAnterior;
                    }

                    object pasilloAnterior = tbrefaccion.Rows[e.RowIndex].Cells[21].Value;
                    cbpasillo.SelectedValue = pasilloAnterior;
                    if (cbpasillo.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idpasillo,UPPER(pasillo) AS pasillo FROM cpasillos WHERE (status='1' OR idpasillo='" + pasilloAnterior + "') and empresa='" + empresa + "' ORDER BY pasillo ASC", cbpasillo, "idpasillo", "pasillo", "-- SELECCIONE PASILLO --");
                        cbpasillo.SelectedValue = pasilloAnterior;
                    }
                    cbnivel.SelectedValue = nivelAnterior = tbrefaccion.Rows[e.RowIndex].Cells[22].Value.ToString();

                    if (cbnivel.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE (status='1' OR idnivel='" + nivelAnterior + "') and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "'  and empresa='" + empresa + "' ORDER BY nivel ASC", cbnivel, "idnivel", "nivel", "--SELECCIONE UN NIVEL");
                        cbnivel.SelectedValue = nivelAnterior;
                    }
                    object anaquelAnterior = tbrefaccion.Rows[e.RowIndex].Cells[23].Value;
                    cbanaquel.SelectedValue = anaquelAnterior;
                    if (cbanaquel.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idanaquel,UPPER(anaquel) AS anaquel FROM canaqueles WHERE (status='1' OR idanaquel='" + anaquelAnterior + "') and nivelfkcniveles= '" + cbnivel.SelectedValue + "' and empresa='" + empresa + "' ORDER BY anaquel ASC", cbanaquel, "idanaquel", "anaquel", "--SELECCIONE UN ANAQUEL");
                        cbanaquel.SelectedValue = anaquelAnterior;
                    }
                    cbcharola.SelectedValue = charolaAnterior = tbrefaccion.Rows[e.RowIndex].Cells[24].Value.ToString();
                    if (cbcharola.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idcharola,UPPER(charola) AS charola FROM ccharolas WHERE (status='1' OR idcharola ='" + charolaAnterior + "') and anaquelfkcanaqueles= '" + cbanaquel.SelectedValue + "' and empresa='" + empresa + "' ORDER BY charola ASC", cbcharola, "idcharola", "charola", "--SELECCIONE UN ANAQUEL");
                        cbcharola.SelectedValue = charolaAnterior;
                    }
                    cmbPROVEEDOR.SelectedValue = Convert.ToString(v.getaData("select coalesce(convert(proveedrofkCProveedores,char),'0') as idproveedor from crefacciones where idrefaccion='" + idRefaccionTemp + "'"));
                    if (cmbPROVEEDOR.SelectedIndex == -1)
                    {
                        v.iniCombos("Select idproveedor,   if(empresa = '',concat(aPaterno, ' ', aMaterno, ' ', nombres) ,empresa) as Nombre from cproveedores WHERE idproveedor = '" + proveedorAnterior + "' and empresaS = '" + empresa + "'", cmbPROVEEDOR, "idproveedor", "Nombre", "--SELECCIONE UN PROVEEDOR");
                        cmbPROVEEDOR.SelectedValue = proveedorAnterior;
                    }
                    cmbMoneda.SelectedValue = Convert.ToString(v.getaData("Select coalesce(convert(tipoMonedafkCTipoCambio,char),'0') as id from crefacciones where idrefaccion = '" + idRefaccionTemp + "'"));
                    /*if (cmbMoneda.SelectedIndex == -1)
                    {
                        v.iniCombos("select idTipoCambio as id, Simbolo from ctipocambio order by simbolo desc", cmbMoneda, "id", "Simbolo", "--MONEDA--");
                    }*/

                    string tip = Convert.ToString(tbrefaccion.Rows[e.RowIndex].Cells[28].Value.ToString());
                    if (tip.ToString().Equals("NUEVA"))
                    {
                        cmbTipo.SelectedIndex = 1;
                    }
                    else if (tip.ToString().Equals("REMANOFACTURADA"))
                    {
                        cmbTipo.SelectedIndex = 2;
                    }
                    else if (tip.ToString().Equals("PRODUCCION"))
                    {
                        cmbTipo.SelectedIndex = 3;
                    }
                    else if (tip.ToString().Equals("0"))
                    {
                        cmbTipo.SelectedIndex = 0;
                    }
                    CostoAnterior = Convert.ToString(v.getaData("select coalesce(convert(CostoUni,char),'0') as costo from crefacciones where idrefaccion='" + idRefaccionTemp + "'"));
                    txtdesc.Text = descripcionAnterior = v.mayusculas(tbrefaccion.Rows[e.RowIndex].Cells[14].Value.ToString().ToLower());
                    notifmedia.Text = mediaAnterior = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[12].Value).ToString();
                    notifabastecimiento.Text = abastecimientoAnterior = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[13].Value).ToString();
                    ultimacantidad = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[11].Value).ToString();
                    lblMaxio.Text = Convert.ToDecimal(tbrefaccion.Rows[e.RowIndex].Cells[10].Value).ToString() + ' '+ lblum.Text;
                    editar = true;
                    gbaddrefaccion.Text = "Actualizar Refacción";
                    btnsave.BackgroundImage = Properties.Resources.pencil;
                    lblsave.Text = "Guardar"; btnsave.Visible = lblsave.Visible = false;
                    notifmedia.Focus();
                    notifabastecimiento.Focus();
                    txtcodrefaccion.Focus();
                    panelM.Visible = true;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex){MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);}
        }
        bool mostrarmotivoActualizacion(string[,] cambios)
        {
            bool res = false;
            for (int i = 0; i < cambios.GetLength(0); i++)
            {
                if (!cambios[i, 0].Equals(cambios[i, 1])) res = true;
            }
            return res;
        }
        public void mostrar_proveedor(int empresa)
        {
            v.iniCombos("Select idproveedor,  if(t1.empresa = '',concat(t1.aPaterno, ' ', t1.aMaterno, ' ', t1.nombres) , t1.empresa) as Nombre from cproveedores as t1 inner join cempresas as t2 on t1.empresaS = t2.idempresa where t2.idempresa ='" + empresa + "'", cmbPROVEEDOR, "idproveedor", "Nombre", "--SELECCIONE UN PROVEEDOR");
        }

        private void cmbMoneda_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender,e);
        }
    }
}