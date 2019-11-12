using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace controlFallos
{
    public partial class catProveedores : Form
    {

        int idUsuario;
        string _idproveedorTemp;
        Thread th;
        public bool _editar;
        bool reactivar, est_expor = false;
        string _empresaAnterior, _amAnterior, _apAnterior, _nombreAnterior, _idladaanterior1, _idladaanterior2, _idladaanterior3, _idladaanterior4, _paginaweb, idSepomexAnterior, idSepomex, giroAnterior, _correoAnterior, ObservacionesAnterior, calleAnterior, NumeroAnterior, referenciasAnterior, extAnterior1, extAnterior2, extAnterior3, extAnterior4, _ladaManual1, _ladaManual2, _ladaManual3, _ladaManual4, _puestoAnterior;
        string[] telefonosAnterioresEmpresa = new string[2], telefonsAnterioresContacto = new string[2], ladasmanuales = new string[4];
        int status;
        bool yaAparecioMensaje;
        public bool pinsertar { set; get; }
        public bool pconsultar { set; get; }
        public bool peditar { set; get; }
        public bool pdesactivar { set; get; }
        public void privilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT CONCAT(insertar,' ',consultar,' ',editar) FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split(' ');
            if (privilegiosTemp.Length > 0)
            {
                pinsertar = v.getBoolFromInt(Convert.ToInt16(privilegiosTemp[0]));
                pconsultar = v.getBoolFromInt(Convert.ToInt16(privilegiosTemp[1]));
                peditar = v.getBoolFromInt(Convert.ToInt16(privilegiosTemp[2]));
                mostrar();
            }
            else Dispose();
        }
        public void Autocompletado_Empresas(TextBox CajaDeTexto)//Metodo para autocompletado de "Folio de porte" en caja de etxto para buscar por folio de reporte
        {
            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
            string consulta = @"select upper(empresa) as empresa from cproveedores WHERE empresaS='" + empresa + "';  ";
            MySqlCommand cmd = new MySqlCommand(consulta, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                    namesCollection.Add(dr["empresa"].ToString());
            }
            v.c.dbcon.Close();
            txtbempresa.AutoCompleteMode = AutoCompleteMode.Suggest;//tipo de autocompletado
            txtbempresa.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbempresa.AutoCompleteCustomSource = namesCollection;
        }
        void getCambios(object sender, EventArgs e)
        {
            try
            {
                if (_editar)
                {
                    string giro = "";
                    if (cbgiros.DataSource != null)
                        giro = cbgiros.SelectedValue.ToString();

                    string asentemiento = "";
                    if (cbasentamiento.DataSource != null) asentemiento = cbasentamiento.SelectedValue.ToString();
                    ladasmanuales[0] = txtladaEmp1.Text.Trim();
                    ladasmanuales[1] = txtladaEmp2.Text.Trim();
                    ladasmanuales[2] = txtLadaCon1.Text.Trim();
                    ladasmanuales[3] = txtLadaCon2.Text.Trim();
                    string idlada1Validacion = "";
                    if (cmbTel_uno.SelectedIndex > 0) idlada1Validacion = cmbTel_uno.SelectedValue.ToString();
                    string idlada2Validacion = "";
                    if (cbTel_dos.SelectedIndex > 0) idlada2Validacion = cbTel_dos.SelectedValue.ToString();
                    string idlada3Validacion = "";
                    if (cbladas.SelectedIndex > 0) idlada3Validacion = cbladas.SelectedValue.ToString();
                    string idlada4Validacion = "";
                    if (cbladas1.SelectedIndex > 0) idlada4Validacion = cbladas1.SelectedValue.ToString();
                    if (status == 1 && ((!string.IsNullOrWhiteSpace(txtgetempresa.Text)) && (!_empresaAnterior.Equals(v.mayusculas(txtgetempresa.Text.Trim().ToLower())) || !_apAnterior.Equals(v.mayusculas(txtgetap.Text.Trim().ToLower())) || !_amAnterior.Equals(v.mayusculas(txtgetam.Text.Trim().ToLower())) || !_nombreAnterior.Equals(v.mayusculas(txtgetnombre.Text.Trim().ToLower())) || !_puestoAnterior.Equals(txtPuesto.Text.Trim()) || !_paginaweb.Equals(txtweb.Text) || !(giroAnterior ?? "").Equals((giro)) || !(_idladaanterior1).Equals(idlada1Validacion) || !telefonosAnterioresEmpresa[0].Equals(txtTel_Uno.Text) || !extAnterior1.Equals(txtext1.Text) || !(_idladaanterior2).Equals(idlada2Validacion)) || !telefonosAnterioresEmpresa[1].Equals(txtTel_dos.Text) || !extAnterior2.Equals(txtext2.Text) || !ObservacionesAnterior.Equals(txtobservaciones.Text.Trim()) || !idSepomexAnterior.Equals((idSepomex ?? "")) || !v.mayusculas(calleAnterior.ToLower()).Equals(v.mayusculas(txtcalle.Text.Trim().ToLower())) || !v.mayusculas(NumeroAnterior.ToLower()).Equals(v.mayusculas(txtnum.Text.Trim().ToLower())) || !v.mayusculas(referenciasAnterior.ToLower()).Equals(v.mayusculas(txtreferencias.Text.Trim().ToLower())) || !_correoAnterior.Equals(txtgetemail.Text) || !(_idladaanterior3).Equals(idlada3Validacion) || !telefonsAnterioresContacto[0].Equals(txtgettelefono.Text) || !extAnterior3.Equals(txtext3.Text) || !(_idladaanterior4).Equals(idlada4Validacion) || !telefonsAnterioresContacto[1].Equals(txtphone.Text) || !extAnterior4.Equals(txtext4.Text) || !txtweb.Text.Equals(_paginaweb) || !ladasmanuales[0].Equals(_ladaManual1) || !ladasmanuales[1].Equals((_ladaManual2)) || !ladasmanuales[2].Equals((_ladaManual3)) || !ladasmanuales[3].Equals((_ladaManual4))))
                    {
                        if (!string.IsNullOrWhiteSpace(txtweb.Text))
                        {
                            if (!v.paginaWebValida(txtweb.Text.Trim()))
                                btnguardar.Visible = lblguardar.Visible = true;

                            else
                                btnguardar.Visible = lblguardar.Visible = false;
                        }
                        else
                            btnguardar.Visible = lblguardar.Visible = true;
                        if (!string.IsNullOrWhiteSpace(txtgetemail.Text))
                        {
                            if (v.validacionCorrero(txtgetemail.Text))
                                btnguardar.Visible = lblguardar.Visible = true;
                            else
                                btnguardar.Visible = lblguardar.Visible = false;
                        }
                        else
                            btnguardar.Visible = lblguardar.Visible = true;
                    }
                    else
                        btnguardar.Visible = lblguardar.Visible = false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbinsertmodif.Visible = true;
                lblopcionales.Visible = true;
            }
            if (pconsultar)
            {
                gbbuscar.Visible = true;
                tbProveedores.Visible = true;
            }
            if (peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
            if (peditar && !pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblguardar.Text = "Editar Proveedor";
                _editar = true;
            }
        }
        int empresa, area;
        validaciones v;
        public catProveedores(int idUsuario, System.Drawing.Image logo, int empresa, int area, validaciones v)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.v = v;
            this.idUsuario = idUsuario;
            pblogo.BackgroundImage = logo;
            cbladas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbasentamiento.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbProveedores.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbTel_dos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbTel_uno.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbgiros.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbgirosb.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            pGiros.Visible = (Convert.ToInt32(v.getaData("SELECT ver FROM privilegios WHERE namform='catGiros' AND usuariofkcpersonal='" + idUsuario + "'")) == 1);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new System.Drawing.Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbProveedores.ColumnHeadersDefaultCellStyle = d;
            v.ChangeControlStyles(btnguardar, ControlStyles.Selectable, false);
        }
        public void insertarums()
        {
            tbProveedores.Rows.Clear();
            string sql = @"SELECT t1.idproveedor, UPPER(t1.empresa) AS empresa, COALESCE(t1.paginaweb, '') AS paginaweb, COALESCE(UPPER(t2.giro), '') AS giro, CONCAT(COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idlada) ,concat('(+',t1.lada1,') ')), t1.telefonoEmpresaUno, IF(t1.ext1 IS NULL, '', CONCAT(' Ext. ', t1.ext1)))), ''),COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('\\n(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idladados),CONCAT('\\n','(+',t1.lada2,') ')), t1.telefonoEmpresaDos, IF(t1.ext2 IS NULL, '', CONCAT(' Ext. ', t1.ext2)))), '')) AS telefonoEmpresa, COALESCE(UPPER(t1.observaciones), '') AS observaciones, COALESCE((SELECT UPPER(CONCAT('Calle: ', t1.calle, ', Número: ', t1.Numero, ', ', t2.tipo, ' ', t2.asentamiento, ', ', municipio, ', ', t2.estado, '. C. P. ', t2.cp)) FROM     sepomex AS t2 WHERE t1.domiciliofksepomex = t2.id), '') AS domicilio, COALESCE(UPPER(t1.aPaterno),'') AS aPaterno, COALESCE( UPPER(t1.AMaterno),'') AS aMaterno, COALESCE( UPPER(t1.nombres),'') AS nombres, COALESCE(t1.correo, '') AS correo, CONCAT(COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idladatres) ,concat('(+',t1.lada3,') ')), t1.telefonoContactoUno, IF(t1.ext3 IS NULL, '', CONCAT(' Ext. ', t1.ext3)))), ''),COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('\\n(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idladacuatro) ,CONCAT('\\n','(+',t1.lada4,') ')), t1.telefonoContactoDos, IF(t1.ext4 IS NULL, '', CONCAT(' Ext. ', t1.ext4)))), '')) AS telefonoContacto, IF(t1.status = 1, UPPER('Activo'), UPPER(CONCAT('No Activo'))), COALESCE(t2.idgiro, '') AS idgiro, t1.idlada AS idlada1, COALESCE(t1.telefonoEmpresaUno, '') AS telempresa1, t1.idladados AS idlada2, COALESCE(t1.telefonoEmpresaDos, '') AS telempresa2, COALESCE(t1.domiciliofksepomex, '') AS iddomicilio, COALESCE(calle, '') AS calle, COALESCE(numero, '') AS numero, COALESCE(referencias, '') AS referencias, t1.idladatres AS idlada3, COALESCE(t1.telefonoContactoUno, '') AS telcontacto1, t1.idladacuatro AS idlada4, COALESCE(t1.telefonoContactoDos, '') AS telcontacto2, COALESCE(ext1, '') AS ext1, COALESCE(ext2, '') AS ext2, COALESCE(ext3, '') AS ext3, COALESCE(ext4, '') AS ext4, COALESCE(lada1,'') as lada1, COALESCE(lada2,'') as lada2, COALESCE(lada3,'') as lada3, COALESCE(lada4,'') as lada4, coalesce(upper(t1.Puesto),'') FROM cproveedores AS t1 LEFT JOIN cgiros AS t2 ON t1.Clasificacionfkcgiros = t2.idgiro WHERE t1.empresaS ='"+empresa+"' ORDER BY t1.empresa ASC;";
            DataTable t = (DataTable)v.getData(sql);
            for (int i = 0; i < t.Rows.Count; i++) tbProveedores.Rows.Add(t.Rows[i].ItemArray);
            tbProveedores.ClearSelection();
        }
        
        public void giros_desactivados(string giro)
        {
            MySqlCommand cmd = new MySqlCommand("Select idgiro, upper(giro) as giro from cgiros where idgiro='" + giro + "' AND empresa='"+empresa+"' and status='0'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                cbgiros.DataSource = null;
                MySqlCommand cmd6 = new MySqlCommand("SELECT idgiro,upper(giro) as giro from cgiros where status='1' AND empresa='"+empresa+"' order by giro asc", v.c.dbconection());
                MySqlDataAdapter da = new MySqlDataAdapter(cmd6);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow nuevaFila = dt.NewRow();
                nuevaFila["idgiro"] = 0;
                nuevaFila["giro"] = "-- seleccione una Clasificacion --".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                nuevaFila = dt.NewRow();
                nuevaFila["idgiro"] = 0;
                nuevaFila["giro"] = "Sin Clasificación ".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 1);
                cbgirosb.DisplayMember = "giro";
                cbgirosb.ValueMember = "idgiro";
                cbgirosb.DataSource = dt;
            }
            else
            {
                v.iniCombos("SELECT idgiro,upper(giro) as giro from cgiros where status='1'  AND empresa ='"+empresa+"' order by giro asc", cbgiros, "idgiro", "giro", "-SELECIONE UN GIRO-");
                cbgiros.SelectedValue = giro;
            }
            dr.Close();
            v.c.dbconection().Close();
        }
        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                string empresa = v.mayusculas(txtgetempresa.Text.ToLower());
                string ap = v.mayusculas(txtgetap.Text.ToLower());
                string am = v.mayusculas(txtgetam.Text.ToLower());
                string nombre = v.mayusculas(txtgetnombre.Text.ToLower());
                string correo = txtgetemail.Text;
                string telefono = txtgettelefono.Text;
                string asentamiento = "";
                if (cbasentamiento.DataSource != null) if (cbasentamiento.SelectedIndex > 0) asentamiento = cbasentamiento.SelectedValue.ToString();
                string Tel_E1 = txtTel_Uno.Text;
                string pagweb = txtweb.Text.Trim();
                string calle = v.mayusculas(txtcalle.Text.ToLower());
                string numeros = v.mayusculas(txtnum.Text.ToLower());
                string puesto = v.mayusculas(txtPuesto.Text.ToLower());
                string giros = ""; if (cbgiros.SelectedIndex > 0) giros = cbgiros.SelectedValue.ToString();
                string referenciass = v.mayusculas(txtreferencias.Text.ToLower());
                string[] telefonosEmpresa = new string[2];
                string[] telefonosContacto = new string[2];
                string observaciones = txtobservaciones.Text.Trim();
                telefonosEmpresa[0] = txtTel_Uno.Text;
                telefonosEmpresa[1] = txtTel_dos.Text;
                telefonosContacto[0] = txtgettelefono.Text;
                telefonosContacto[1] = txtphone.Text;
                ladasmanuales[0] = txtladaEmp1.Text.Trim();
                ladasmanuales[1] = txtladaEmp2.Text.Trim();
                ladasmanuales[2] = txtLadaCon1.Text.Trim();
                ladasmanuales[3] = txtLadaCon2.Text.Trim();
                string ext1 = txtext1.Text.Trim();
                string ext2 = txtext2.Text.Trim();
                string ext3 = txtext3.Text.Trim();
                string ext4 = txtext4.Text.Trim();
                if (!_editar)
                {
                    insertar(empresa, ap, am, nombre, correo, telefonosEmpresa, telefonosContacto, asentamiento, calle, numeros, ladasmanuales);
                }
                else
                {
                    if (v.formularioProveedores(empresa, ap, am, nombre, correo, telefonosEmpresa, telefonosContacto, cmbTel_uno.SelectedValue.ToString(), cbTel_dos.SelectedValue.ToString(), cbladas.SelectedValue.ToString(), cbladas1.SelectedValue.ToString(), asentamiento.ToString(), calle, numeros, ladasmanuales) && !v.yaExisteProveedorActualizar(empresa, _empresaAnterior, ap, _apAnterior, am, _amAnterior, nombre, _nombreAnterior, correo, _correoAnterior) && btnguardar.Visible)
                    {
                        DialogResult res = DialogResult.OK;
                        bool motivo = false;
                        if (mostrarMotivoEdicion(new string[28, 2] { { _empresaAnterior, empresa }, { _paginaweb, pagweb }, { (giroAnterior ?? ""), giros }, { _idladaanterior1, cmbTel_uno.SelectedValue.ToString() }, { _idladaanterior2, cbTel_dos.SelectedValue.ToString() }, { _idladaanterior3, cbladas.SelectedValue.ToString() }, { _idladaanterior4, cbladas1.SelectedValue.ToString() }, { ObservacionesAnterior, observaciones }, { idSepomexAnterior, (idSepomex ?? "") }, { v.mayusculas(calleAnterior.ToLower()), v.mayusculas(calle.ToLower()) }, { v.mayusculas(NumeroAnterior.ToLower()), numeros }, { v.mayusculas(referenciasAnterior.ToLower()), referenciass }, { _apAnterior, ap }, { _amAnterior, am }, { _nombreAnterior, nombre }, { _ladaManual1, ladasmanuales[0] }, { _ladaManual2, ladasmanuales[1] }, { _ladaManual3, ladasmanuales[2] }, { _ladaManual4, ladasmanuales[3] }, { telefonosAnterioresEmpresa[0], telefonosEmpresa[0] }, { telefonosAnterioresEmpresa[1], telefonosEmpresa[1] }, { telefonsAnterioresContacto[0], telefonosContacto[0] }, { telefonsAnterioresContacto[1], telefonosContacto[1] }, { extAnterior1, ext1 }, { extAnterior2, ext2 }, { extAnterior3, ext3 }, { extAnterior4, ext4 }, { _puestoAnterior, puesto } }))
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            res = obs.ShowDialog();
                            if (res == DialogResult.OK)
                            {

                                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                string extension = "";
                                if (extAnterior1 != "")
                                {
                                    extension = "Ext. " + extAnterior1;
                                }

                                string clave = "";
                                if (_idladaanterior1.Length > 0) clave = v.getaData("SELECT clave FROM ladanac WHERE idladanac='" + _idladaanterior1 + "'").ToString();
                                var domicilio = "Calle: " + calle + ", Número: " + numeros + "," + v.getaData("select concat( x2.tipo, ' ', x2.asentamiento, ', ', x2.municipio, ', ', x2.estado, '. C. P. ', x2.cp) from sepomex as x2 where x2.id='" + idSepomexAnterior + "'");
                                var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Proveedores','" + _idproveedorTemp + "','" + _empresaAnterior + ";" + _paginaweb + ";" + giroAnterior + ";(+" + (clave == "" ? _ladaManual1 : clave) + ")" + telefonosAnterioresEmpresa[0] + " " + extension + ";" + observaciones + ";" + domicilio + ";" + (_nombreAnterior + " " + _apAnterior + " " + _amAnterior) + ";" + _correoAnterior + "','" + idUsuario + "',NOW(),'Actualización de Proveedor','" + edicion + "','" + this.empresa + "','" + area + "')");
                                motivo = true;
                            }
                        }
                        if (res == DialogResult.OK)
                        {
                            string idlada1Validacion = "";
                            if (cmbTel_uno.SelectedIndex > 0) idlada1Validacion = cmbTel_uno.SelectedValue.ToString();
                            string idlada2Validacion = "";
                            if (cbTel_dos.SelectedIndex > 0) idlada2Validacion = cbTel_dos.SelectedValue.ToString();
                            string idlada3Validacion = "";
                            if (cbladas.SelectedIndex > 0) idlada3Validacion = cbladas.SelectedValue.ToString();
                            string idlada4Validacion = "";
                            if (cbladas1.SelectedIndex > 0) idlada4Validacion = cbladas1.SelectedValue.ToString();
                            string cambios = "empresa = '" + empresa + "'";
                            if (!_apAnterior.Equals(ap)) cambios += ", aPaterno='" + ap + "'";
                            if (!_amAnterior.Equals(am)) cambios += ", aMaterno='" + am + "'";
                            if (!_nombreAnterior.Equals(nombre)) cambios += ", nombres='" + nombre + "'";
                            if (!_puestoAnterior.Equals(puesto)) cambios += ", puesto='" + puesto + "'";
                            if (!txtweb.Text.Equals(_paginaweb)) cambios += ", paginaweb = '" + txtweb.Text + "'";
                            if (!(giroAnterior ?? "0").Equals(cbgiros.SelectedValue.ToString())) cambios += ",Clasificacionfkcgiros = '" + cbgiros.SelectedValue + "'";
                            if (!idlada1Validacion.Equals(_idladaanterior1)) { cambios += ",idlada ='" + cmbTel_uno.SelectedValue + "'"; if (string.IsNullOrWhiteSpace(_idladaanterior1) && !string.IsNullOrWhiteSpace(_ladaManual1)) cambios += ",lada1=NULL"; }
                            if (!telefonosEmpresa[0].Equals(telefonosAnterioresEmpresa[0])) cambios += ",telefonoEmpresaUno='" + telefonosEmpresa[0] + "'";
                            if (!extAnterior1.Equals(txtext1.Text)) cambios += ",ext1='" + txtext1.Text + "'";
                            if (!idlada2Validacion.Equals(_idladaanterior2)) { cambios += ",idladados='" + cbTel_dos.SelectedValue.ToString() + "'"; if (string.IsNullOrWhiteSpace(_idladaanterior2) && !string.IsNullOrWhiteSpace(_ladaManual2)) cambios += ",lada2=NULL"; }
                            if (!telefonosEmpresa[1].Equals(telefonosAnterioresEmpresa[1])) cambios += ",telefonoEmpresaDos='" + telefonosEmpresa[1] + "'";
                            if (!extAnterior2.Equals(txtext2.Text)) cambios += ",ext2='" + txtext2.Text + "'";
                            if (!ObservacionesAnterior.Equals(v.mayusculas(observaciones.ToLower()))) cambios += ",observaciones ='" + v.mayusculas(observaciones.ToLower()) + "'";
                            if (!idSepomexAnterior.Equals(asentamiento)) cambios += ",domiciliofksepomex = '" + asentamiento + "'";
                            if (!calleAnterior.Equals(v.mayusculas(calle).ToLower())) cambios += ", Calle ='" + v.mayusculas(calle.ToLower()) + "'";
                            if (!NumeroAnterior.Equals(v.mayusculas(numeros).ToLower())) cambios += ", Numero ='" + v.mayusculas(numeros.ToLower()) + "'";
                            if (!referenciasAnterior.Equals(v.mayusculas(referenciass).ToLower())) cambios += ", Referencias ='" + v.mayusculas(referenciass.ToLower()) + "'";
                            if (!_correoAnterior.Equals(correo)) cambios += ",correo='" + correo + "'";
                            if (!idlada3Validacion.Equals(_idladaanterior3)) { cambios += ",idladatres ='" + cbladas.SelectedValue + "'"; if (string.IsNullOrWhiteSpace(_idladaanterior3) && !string.IsNullOrWhiteSpace(_ladaManual3)) cambios += ",lada3=NULL"; }
                            if (!telefonosContacto[0].Equals(telefonsAnterioresContacto[0])) cambios += ",telefonoContactoUno='" + telefonosContacto[0] + "'";
                            if (!extAnterior3.Equals(txtext3.Text)) cambios += ",ext3='" + txtext3.Text + "'";
                            if (!idlada4Validacion.Equals(_idladaanterior4)) { cambios += ",idladacuatro='" + cbladas1.SelectedValue.ToString() + "'"; if (string.IsNullOrWhiteSpace(_idladaanterior3) && !string.IsNullOrWhiteSpace(_ladaManual4)) cambios += ",lada4=NULL"; }
                            if (!telefonosContacto[1].Equals(telefonsAnterioresContacto[1])) cambios += ",telefonoContactoDos='" + telefonosContacto[1] + "'";
                            if (!extAnterior4.Equals(txtext4.Text)) cambios += ",ext4='" + txtext4.Text + "'";
                            if (!_ladaManual1.Equals(txtladaEmp1.Text.Trim())) { cambios += ",lada1='" + txtladaEmp1.Text.Trim() + "'"; if (!string.IsNullOrWhiteSpace(_idladaanterior1) && string.IsNullOrWhiteSpace(_ladaManual1)) cambios += ",idlada=NULL"; }
                            if (!_ladaManual2.Equals(txtladaEmp2.Text.Trim())) { cambios += ",lada2='" + txtladaEmp1.Text.Trim() + "'"; if (!string.IsNullOrWhiteSpace(_idladaanterior2) && string.IsNullOrWhiteSpace(_ladaManual2)) cambios += ",idladados=NULL"; }
                            if (!_ladaManual3.Equals(txtLadaCon1.Text.Trim())) { cambios += ",lada3='" + txtLadaCon1.Text.Trim() + "'"; if (!string.IsNullOrWhiteSpace(_idladaanterior3) && string.IsNullOrWhiteSpace(_ladaManual3)) cambios += ",idladatres=NULL"; }
                            if (!_ladaManual4.Equals(txtLadaCon2.Text.Trim())) { cambios += ",lada4='" + txtLadaCon2.Text.Trim() + "'"; if (!string.IsNullOrWhiteSpace(_idladaanterior4) && string.IsNullOrWhiteSpace(_ladaManual4)) cambios += ",idladacuatro=NULL"; }

                            string sql = "UPDATE cproveedores SET " + cambios + " WHERE idproveedor='" + _idproveedorTemp + "'";
                            if (v.c.insertar(sql))
                            {
                                if (!yaAparecioMensaje)
                                {
                                    if (motivo)
                                        MessageBox.Show("Datos Actualizados Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    else
                                        MessageBox.Show("Se Ha Añadido la información del Proveedor \"" + empresa + "\" Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                limpiar();
                                esta_exportandO();
                                insertarums();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar(string empresa, string ap, string am, string nombre, string correo, string[] telefonosEmpresa, string[] telefonosContacto, object asentamiento, string calle, string numero, string[] ladasManuales)
        {

            if (v.formularioProveedores(empresa, ap, am, nombre, correo, telefonosEmpresa, telefonosContacto, cmbTel_uno.SelectedValue.ToString(), cbTel_dos.SelectedValue.ToString(), cbladas.SelectedValue.ToString(), cbladas1.SelectedValue.ToString(), asentamiento.ToString(), calle, numero, ladasmanuales) && !v.existeProveedor(v.mayusculas(empresa), v.mayusculas(ap), v.mayusculas(am), v.mayusculas(nombre), v.mayusculas(correo)))
            {
                string campos = "";
                string valores = "";
                if (!string.IsNullOrWhiteSpace(txtgetap.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "aPaterno";
                        valores = "'" + txtgetap.Text + "'";
                    }
                    else
                    {
                        campos += ",aPaterno";
                        valores += ",'" + txtgetap.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetam.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "aMaterno";
                        valores = "'" + txtgetam.Text + "'";
                    }
                    else
                    {
                        campos += ",aMaterno";
                        valores += ",'" + txtgetam.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetnombre.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "nombres";
                        valores = "'" + txtgetnombre.Text + "'";
                    }
                    else
                    {
                        campos += ",nombres";
                        valores += ",'" + txtgetnombre.Text + "'";
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtobservaciones.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "observaciones";
                        valores = "'" + txtobservaciones.Text + "'";
                    }
                    else
                    {
                        campos += ",observaciones";
                        valores += ",'" + txtobservaciones.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetemail.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "correo";
                        valores = "'" + txtgetemail.Text + "'";
                    }
                    else
                    {
                        campos += ",correo";
                        valores += ",'" + txtgetemail.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtTel_Uno.Text))
                {
                    if (campos == "")
                    {
                        campos = "telefonoEmpresaUno";
                        valores = "'" + txtTel_Uno.Text + "'";
                    }
                    else
                    {
                        campos += ",telefonoEmpresaUno";
                        valores += ",'" + txtTel_Uno.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtTel_dos.Text))
                {
                    if (campos == "")
                    {
                        campos = "telefonoEmpresaDos";
                        valores = "'" + txtTel_dos.Text + "'";
                    }
                    else
                    {
                        campos += ",telefonoEmpresaDos";
                        valores += ",'" + txtTel_dos.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtgetempresa.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "empresa";
                        valores = "'" + txtgetempresa.Text + "'";
                    }
                    else
                    {
                        campos += ",empresa";
                        valores += ",'" + txtgetempresa.Text + "'";
                    }
                }

                if (!string.IsNullOrWhiteSpace(idSepomex))
                {
                    if (campos == "")
                    {
                        campos = "domiciliofksepomex";
                        valores = "'" + idSepomex + "'";
                    }
                    else
                    {
                        campos += ",domiciliofksepomex";
                        valores += ",'" + idSepomex + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtcalle.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "Calle";
                        valores = "'" + txtcalle.Text + "'";
                    }
                    else
                    {
                        campos += ",Calle";
                        valores += ",'" + txtcalle.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtnum.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "Numero";
                        valores = "'" + txtnum.Text + "'";
                    }
                    else
                    {
                        campos += ",Numero";
                        valores += ",'" + txtnum.Text + "'";
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtweb.Text.Trim()))
                {
                    campos += ",paginaweb";
                    valores += ", '" + txtweb.Text.Trim() + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtreferencias.Text.Trim()))
                {
                    if (campos == "")
                    {
                        campos = "Referencias";
                        valores = "'" + txtreferencias.Text + "'";
                    }
                    else
                    {
                        campos += ",Referencias";
                        valores += ",'" + txtreferencias.Text + "'";
                    }
                }
                if (cbgiros.SelectedIndex > 0)
                {
                    if (campos == "")
                    {
                        campos = "Clasificacionfkcgiros";
                        valores = "'" + cbgiros.SelectedValue + "'";
                    }
                    else
                    {
                        campos += ",Clasificacionfkcgiros";
                        valores += ",'" + cbgiros.SelectedValue + "'";
                    }
                }
                if (cmbTel_uno.SelectedIndex > 0)
                {
                    campos += ",idlada";
                    valores += ",'" + cmbTel_uno.SelectedValue + "'";
                }
                else if (!string.IsNullOrWhiteSpace(txtladaEmp1.Text.Trim()))
                {
                    campos += ",lada1";
                    valores += ",'" + txtladaEmp1.Text.Trim() + "'";
                }
                if (cbTel_dos.SelectedIndex > 0)
                {
                    campos += ",idladados";
                    valores += ",'" + cbTel_dos.SelectedValue + "'";
                }
                else if (!string.IsNullOrWhiteSpace(txtladaEmp2.Text.Trim()))
                {

                    campos += ",lada2";
                    valores += ",'" + txtladaEmp2.Text.Trim() + "'";
                }
                if (cbladas.SelectedIndex > 0)
                {
                    campos += ",idladatres";
                    valores += ",'" + cbladas.SelectedValue + "'";
                }
                else if (!string.IsNullOrWhiteSpace(txtLadaCon1.Text.Trim()))
                {

                    campos += ",lada3";
                    valores += ",'" + txtLadaCon1.Text + "'";
                }
                if (cbladas1.SelectedIndex > 0)
                {
                    campos += ",idladacuatro";
                    valores += ",'" + cbladas.SelectedValue + "'";
                }
                else if (!string.IsNullOrWhiteSpace(txtLadaCon2.Text.Trim()))
                {
                    campos += ",lada4";
                    valores += ",'" + txtLadaCon2.Text.Trim() + "'";
                }

                if (!string.IsNullOrWhiteSpace(txtgettelefono.Text))
                {
                    campos += ",telefonoContactoUno";
                    valores += ",'" + txtgettelefono.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtphone.Text))
                {
                    campos += ",telefonoContactoDos";
                    valores += ",'" + txtphone.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext1.Text))
                {
                    campos += ",ext1";
                    valores += ",'" + txtext1.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext2.Text))
                {
                    campos += ",ext2";
                    valores += ",'" + txtext2.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext3.Text))
                {
                    campos += ",ext3";
                    valores += ",'" + txtext3.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtext4.Text))
                {
                    campos += ",ext4";
                    valores += ",'" + txtext4.Text + "'";
                }
                string sql = "INSERT INTO cproveedores(" + campos + ",usuariofkcpersonal,empresaS) VALUES(" + valores + ",'" + this.idUsuario + "','" + this.empresa + "')";
                if (v.c.insertar(sql))
                {
                    if (v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Proveedores',(SELECT idproveedor FROM cproveedores WHERE empresa='" + empresa + "'),'Inserción de Proveedor','" + idUsuario + "',NOW(),'Inserción de Proveedor','" + this.empresa + "','" + area + "')"))
                    {
                        MessageBox.Show("El Proveedor se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        if (!LblExcel.Text.Equals("Exportando"))
                            insertarums();
                    }
                }
            }
        }
        void borrar()
        {
            if (MessageBox.Show("¿Desea Limpiar Todos los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                limpiar();
            }
        }
        private void txtgetempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraEmpresas(e);
        }

        private void txtgetap_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void txtgettelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtgetemail_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }
        void limpiar()
        {
            if (pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                gbinsertmodif.Text = "Agregar Proveedor";
                lblguardar.Text = "Agregar";
                _editar = false;
                txtgetempresa.Focus();
            }
            cbgiros.SelectedIndex = 0;
            txtTel_Uno.Clear();
            txtTel_dos.Clear();
            txtphone.Clear();
            cmbTel_uno.SelectedIndex = 0;
            cbTel_dos.SelectedIndex = 0;
            cbladas1.SelectedIndex = 0;
            txtcalle.Clear();
            txtnum.Clear();
            txtreferencias.Clear();
            txtgetempresa.Clear();
            txtgetap.Clear();
            txtgetam.Clear();
            txtgetnombre.Clear();
            txtgetemail.Clear();
            txtPuesto.Clear();
            txtgettelefono.Clear();
            pExportar.Visible = false;
            _idproveedorTemp = null;
            txtobservaciones.Clear();
            pcancel.Visible = false;
            peliminarpro.Visible = false;
            cbladas.SelectedIndex = 0;
            txtcp.Clear();
            lblEstado.Text = "";
            lblmunicipio.Text = "";
            lblmunicipio.Text = "";
            lblzona.Text = "";
            lbltipo.Text = "";
            txtext1.Clear();
            txtext2.Clear();
            telefonoEmpresa(true);
            telefonoContacto(true);
            txtext4.Clear();
            cbasentamiento.DataSource = null;
            idSepomexAnterior = "";
            cbasentamiento.Enabled = false;
            idSepomex = null;
            txtweb.Clear();
            txtcp.Clear();
            yaAparecioMensaje = false;
            lblciudad.Text = ""; btnguardar.Visible = lblguardar.Visible = true;
            giroAnterior = null;
            ladasmanuales = new string[4];
            _ladaManual1 = null;
            _ladaManual2 = null;
            _ladaManual3 = null;
            _ladaManual4 = null;
            txtladaEmp1.Clear();
            txtladaEmp2.Clear();
            txtLadaCon1.Clear();
            txtLadaCon2.Clear();
            txtext3.Clear();
            _idladaanterior1 = ""; _idladaanterior2 = ""; _idladaanterior3 = ""; _idladaanterior4 = "";
            ocultarLada1();
            ocultarLada2();
            ocultarLada3();
            ocultarLada4();

        }

        private void catProveedores_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                Autocompletado_Empresas(txtbempresa);
                insertarums();
            }
            if (pinsertar || peditar)
            {
                v.iniCombos("SELECT idgiro,upper(giro) as giro from cgiros where status='1' AND empresa='"+empresa+"' order by giro asc", cbgiros, "idgiro", "giro", "-SELECIONE UNa Clasificacion-");
                inigiros();
                iniLadas();
            }
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
        void inigiros()
        {
            cbgirosb.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idgiro,upper(giro) as giro from cgiros where empresa='"+empresa+"' order by giro asc");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idgiro"] = 0;
            nuevaFila["giro"] = "-- seleccione una Clasificacion --".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            nuevaFila = dt.NewRow();
            nuevaFila["idgiro"] = 0;
            nuevaFila["giro"] = "Sin Clasificación ".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 1);
            cbgirosb.DisplayMember = "giro";
            cbgirosb.ValueMember = "idgiro";
            cbgirosb.DataSource = dt;
        }
        private void tbProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[2].Value.ToString()))
                    {
                        Process.Start(tbProveedores.Rows[e.RowIndex].Cells[2].Value.ToString());
                    }
                }
                if (e.ColumnIndex == 10)
                {
                    tbProveedores.ClearSelection();
                    try
                    {
                        Outlook.Application outlookApp = new Outlook.Application();
                        Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                        mailItem.Subject = "Orden de Compra: " + DateTime.Now.ToLongDateString();
                        mailItem.To = tbProveedores.Rows[e.RowIndex].Cells[10].Value.ToString();
                        mailItem.Body = "Adjunte el Archivo pdf Generado en la Orden de Compra.";
                        mailItem.Importance = Outlook.OlImportance.olImportanceNormal;
                        mailItem.Display(false);
                        tbProveedores.ClearSelection();
                    }
                    catch (Exception eX)
                    {
                        MessageBox.Show(eX.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\t Ocurrió Un Error!", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbProveedores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbProveedores.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void tbProveedores_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    string giro = "";
                    if (cbgiros.DataSource != null) if (cbgiros.SelectedIndex > 0) giro = cbgiros.SelectedValue.ToString();
                    ladasmanuales[0] = txtladaEmp1.Text.Trim();
                    ladasmanuales[1] = txtladaEmp2.Text.Trim();
                    ladasmanuales[2] = txtLadaCon1.Text.Trim();
                    ladasmanuales[3] = txtLadaCon2.Text.Trim();
                    string asentemiento = "";
                    string idlada1Validacion = "";
                    if (cmbTel_uno.SelectedIndex > 0) idlada1Validacion = cmbTel_uno.SelectedValue.ToString();
                    string idlada2Validacion = "";
                    if (cbTel_dos.SelectedIndex > 0) idlada2Validacion = cbTel_dos.SelectedValue.ToString();
                    string idlada3Validacion = "";
                    if (cbladas.SelectedIndex > 0) idlada3Validacion = cbladas.SelectedValue.ToString();
                    string idlada4Validacion = "";
                    if (cbladas1.SelectedIndex > 0) idlada4Validacion = cbladas1.SelectedValue.ToString();
                    if (cbasentamiento.DataSource != null) asentemiento = cbasentamiento.SelectedValue.ToString();
                    if (!string.IsNullOrWhiteSpace(_idproveedorTemp) && status == 1 && ((!string.IsNullOrWhiteSpace(txtgetempresa.Text)) && (!_empresaAnterior.Equals(v.mayusculas(txtgetempresa.Text.Trim().ToLower())) || !_apAnterior.Equals(v.mayusculas(txtgetap.Text.Trim().ToLower())) || !_amAnterior.Equals(v.mayusculas(txtgetam.Text.Trim().ToLower())) || !_nombreAnterior.Equals(v.mayusculas(txtgetnombre.Text.Trim().ToLower())) || !_puestoAnterior.Equals(txtPuesto.Text.Trim()) || !_paginaweb.Equals(txtweb.Text) || !(giroAnterior ?? "").Equals((giro)) || !(_idladaanterior1).Equals(idlada1Validacion) || !telefonosAnterioresEmpresa[0].Equals(txtTel_Uno.Text) || !extAnterior1.Equals(txtext1.Text) || !(_idladaanterior2).Equals(idlada2Validacion)) || !telefonosAnterioresEmpresa[1].Equals(txtTel_dos.Text) || !extAnterior2.Equals(txtext2.Text) || !ObservacionesAnterior.Equals(txtobservaciones.Text.Trim()) || !idSepomexAnterior.Equals((idSepomex ?? "")) || !v.mayusculas(calleAnterior.ToLower()).Equals(v.mayusculas(txtcalle.Text.Trim().ToLower())) || !v.mayusculas(NumeroAnterior.ToLower()).Equals(v.mayusculas(txtnum.Text.Trim().ToLower())) || !v.mayusculas(referenciasAnterior.ToLower()).Equals(v.mayusculas(txtreferencias.Text.Trim().ToLower())) || !_correoAnterior.Equals(txtgetemail.Text) || !(_idladaanterior3).Equals(idlada3Validacion) || !telefonsAnterioresContacto[0].Equals(txtgettelefono.Text) || !extAnterior3.Equals(txtext3.Text) || !(_idladaanterior4).Equals(idlada4Validacion) || !telefonsAnterioresContacto[1].Equals(txtphone.Text) || !extAnterior4.Equals(txtext4.Text) || !txtweb.Text.Equals(_paginaweb) || !ladasmanuales[0].Equals(_ladaManual1) || !ladasmanuales[1].Equals((_ladaManual2)) || !ladasmanuales[2].Equals((_ladaManual3)) || !ladasmanuales[3].Equals((_ladaManual4))))
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            btnguardar_Click(null, e);
                        }
                        else
                        {
                            guardarReporte(e);
                        }
                    }
                    else
                    {
                        guardarReporte(e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Muestra en El Formulario Los Datos De La Celda Seleccionada
        /// </summary>
        /// <param name="e"> El Evento Resultante Al Dar Doble Clic En la Tabla</param>
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                limpiar();
                _idproveedorTemp = tbProveedores.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower();
                status = v.getStatusInt(tbProveedores.Rows[e.RowIndex].Cells[12].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeleteuser.Text = "Reactivar";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeleteuser.Text = "Desactivar";
                    }
                    peliminarpro.Visible = true;
                }
                if (peditar)
                {
                    try
                    {
                        _idproveedorTemp = tbProveedores.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower();
                        txtgetempresa.Text = _empresaAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                        txtweb.Text = _paginaweb = tbProveedores.Rows[e.RowIndex].Cells[2].Value.ToString().Trim();

                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[13].Value.ToString()))
                        {
                            giros_desactivados(tbProveedores.Rows[e.RowIndex].Cells[13].Value.ToString());
                            cbgiros.SelectedValue = giroAnterior = tbProveedores.Rows[e.RowIndex].Cells[13].Value.ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[14].Value.ToString()))
                        {
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[14].Value) > 0)
                                cmbTel_uno.SelectedValue = _idladaanterior1 = tbProveedores.Rows[e.RowIndex].Cells[14].Value.ToString();
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[30].Value.ToString()))
                                {
                                    mostrarLada1();
                                    txtladaEmp1.Text = _ladaManual1 = tbProveedores.Rows[e.RowIndex].Cells[30].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[30].Value.ToString()))
                            {
                                mostrarLada1();
                                txtladaEmp1.Text = _ladaManual1 = tbProveedores.Rows[e.RowIndex].Cells[30].Value.ToString();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[14].Value.ToString()))
                        {
                            _idladaanterior1 = "";
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[14].Value) > 0)
                            {
                                _idladaanterior1 = tbProveedores.Rows[e.RowIndex].Cells[14].Value.ToString();
                            }
                        }

                        _ladaManual1 = tbProveedores.Rows[e.RowIndex].Cells[30].Value.ToString();
                        telefonosAnterioresEmpresa[0] = txtTel_Uno.Text = tbProveedores.Rows[e.RowIndex].Cells[15].Value.ToString();
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[16].Value.ToString()))
                        {
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[16].Value.ToString()) > 0)
                            {
                                cbTel_dos.SelectedValue = _idladaanterior2 = tbProveedores.Rows[e.RowIndex].Cells[16].Value.ToString();
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[31].Value.ToString()))
                                {
                                    mostrarLada2();
                                    txtladaEmp2.Text = _ladaManual2 = tbProveedores.Rows[e.RowIndex].Cells[31].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[31].Value.ToString()))
                            {
                                mostrarLada2();
                                txtladaEmp2.Text = _ladaManual2 = tbProveedores.Rows[e.RowIndex].Cells[31].Value.ToString();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[16].Value.ToString()))
                        {
                            _idladaanterior2 = "";
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[16].Value) > 0)
                            {
                                _idladaanterior2 = tbProveedores.Rows[e.RowIndex].Cells[16].Value.ToString();
                            }
                        }
                        _ladaManual2 = tbProveedores.Rows[e.RowIndex].Cells[31].Value.ToString();
                        telefonosAnterioresEmpresa[1] = txtTel_dos.Text = tbProveedores.Rows[e.RowIndex].Cells[17].Value.ToString();
                        extAnterior1 = txtext1.Text = tbProveedores.Rows[e.RowIndex].Cells[26].Value.ToString();
                        extAnterior2 = txtext2.Text = tbProveedores.Rows[e.RowIndex].Cells[27].Value.ToString();
                        ObservacionesAnterior = txtobservaciones.Text = tbProveedores.Rows[e.RowIndex].Cells[5].Value.ToString();
                        idSepomexAnterior = "";
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[18].Value.ToString()))
                        {
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[18].Value) > 0)
                            {

                                idSepomexAnterior = tbProveedores.Rows[e.RowIndex].Cells[18].Value.ToString();
                            }
                        }
                        calleAnterior = txtcalle.Text = tbProveedores.Rows[e.RowIndex].Cells[19].Value.ToString();
                        NumeroAnterior = txtnum.Text = tbProveedores.Rows[e.RowIndex].Cells[20].Value.ToString();
                        referenciasAnterior = txtreferencias.Text = tbProveedores.Rows[e.RowIndex].Cells[21].Value.ToString();
                        txtgetap.Text = _apAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[7].Value.ToString().ToLower());
                        txtgetam.Text = _amAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[8].Value.ToString().ToLower());
                        txtgetnombre.Text = _nombreAnterior = v.mayusculas(tbProveedores.Rows[e.RowIndex].Cells[9].Value.ToString().ToLower());
                        txtgetemail.Text = _correoAnterior = tbProveedores.Rows[e.RowIndex].Cells[10].Value.ToString();
                        txtPuesto.Text = _puestoAnterior = tbProveedores.Rows[e.RowIndex].Cells[34].Value.ToString();
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[22].Value.ToString()))
                        {
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[22].Value) > 0)
                            {
                                cbladas.SelectedValue = _idladaanterior3 = tbProveedores.Rows[e.RowIndex].Cells[22].Value.ToString();
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[32].Value.ToString()))
                                {
                                    mostrarLada3();
                                    txtLadaCon1.Text = _ladaManual3 = tbProveedores.Rows[e.RowIndex].Cells[32].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[32].Value.ToString()))
                            {
                                mostrarLada3();
                                txtLadaCon1.Text = _ladaManual3 = tbProveedores.Rows[e.RowIndex].Cells[32].Value.ToString();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[22].Value.ToString()))
                        {
                            _idladaanterior3 = "";
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[22].Value) > 0)
                            {

                                _idladaanterior3 = tbProveedores.Rows[e.RowIndex].Cells[22].Value.ToString();
                            }
                        }
                        _ladaManual3 = tbProveedores.Rows[e.RowIndex].Cells[32].Value.ToString();
                        telefonsAnterioresContacto[0] = txtgettelefono.Text = tbProveedores.Rows[e.RowIndex].Cells[23].Value.ToString();
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[24].Value.ToString()))
                        {
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[24].Value.ToString()) > 0)
                            {
                                cbladas1.SelectedValue = _idladaanterior4 = tbProveedores.Rows[e.RowIndex].Cells[24].Value.ToString();
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[33].Value.ToString()))
                                {
                                    mostrarLada4();
                                    txtLadaCon2.Text = _ladaManual4 = tbProveedores.Rows[e.RowIndex].Cells[33].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[33].Value.ToString()))
                            {
                                mostrarLada4();
                                txtLadaCon2.Text = _ladaManual4 = tbProveedores.Rows[e.RowIndex].Cells[33].Value.ToString();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(tbProveedores.Rows[e.RowIndex].Cells[24].Value.ToString()))
                        {
                            _idladaanterior4 = "";
                            if (Convert.ToInt32(tbProveedores.Rows[e.RowIndex].Cells[24].Value) > 0)
                            {
                                _idladaanterior4 = tbProveedores.Rows[e.RowIndex].Cells[24].Value.ToString();
                            }
                        }

                        _ladaManual4 = tbProveedores.Rows[e.RowIndex].Cells[33].Value.ToString();
                        telefonsAnterioresContacto[1] = txtphone.Text = tbProveedores.Rows[e.RowIndex].Cells[25].Value.ToString();
                        extAnterior3 = txtext3.Text = tbProveedores.Rows[e.RowIndex].Cells[28].Value.ToString();
                        extAnterior4 = txtext4.Text = tbProveedores.Rows[e.RowIndex].Cells[29].Value.ToString();
                        if (!string.IsNullOrWhiteSpace(telefonosAnterioresEmpresa[1])) telefonoEmpresa(false);
                        if (!string.IsNullOrWhiteSpace(telefonsAnterioresContacto[1])) telefonoContacto(false);
                        if (!string.IsNullOrWhiteSpace(idSepomexAnterior))
                        {
                            if (Convert.ToInt32(idSepomexAnterior) > 0)
                            {
                                radioButton1.Checked = true;
                                txtcp.Text = v.getcpFromidSepomex(idSepomexAnterior);
                                button2_Click(null, e);
                                cbasentamiento.SelectedValue = idSepomexAnterior;
                            }
                            else
                            {
                                radioButton2.Checked = true;
                            }
                        }
                        else
                        {
                            radioButton2.Checked = true;
                        }

                        this._editar = true;
                        if (pinsertar) pcancel.Visible = true; ;
                        btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        gbinsertmodif.Text = "Actualizar Proveedor";
                        lblguardar.Text = "Guardar";
                        tbProveedores.ClearSelection();
                        pExportar.Visible = true;
                        btnguardar.Visible = lblguardar.Visible = false;
                        if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Usted No Tiene Privilegios Para Editar Éste Catálogo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void telefonoEmpresa(bool aparecer)
        {
            p_AñadeTel.Visible = aparecer;
            psegundo_telefono.Visible = !aparecer;
        }
        void telefonoContacto(bool aparecer)
        {
            psecondphone.Visible = !aparecer;
            paddPhone.Visible = aparecer;
        }
        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            int status;
            string msg;
            if (reactivar)
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
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Proveedor";
            obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 10, obs.lblinfo.Location.Y);
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                try
                {

                    String sql = "UPDATE cproveedores SET status = " + status + " WHERE idproveedor  = " + this._idproveedorTemp;
                    if (v.c.insertar(sql))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion ,empresa,area) VALUES('Catálogo de Proveedores','" + _idproveedorTemp + "','" + msg + "activación de Proveedor','" + idUsuario + "',NOW(),'" + msg + "activación de Proveedor','" + edicion + "','" + empresa + "','" + area + "')");

                        MessageBox.Show("El Proveedor ha sido " + msg + "activado", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        insertarums();
                        esta_exportandO();
                    }
                    else
                    {
                        MessageBox.Show("El Proveedor no ha sido desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void linkcancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbempresa.Text) || !string.IsNullOrWhiteSpace(txtBNombre.Text) || !string.IsNullOrWhiteSpace(txtbap.Text) || cbgirosb.SelectedIndex > 0)
            {
                try
                {
                    //limpiar();
                    tbProveedores.Rows.Clear();
                    string wheres = "WHERE empresaS='"+empresa+"'";
                    string sql = "SELECT t1.idproveedor, UPPER(t1.empresa) AS empresa, COALESCE(t1.paginaweb, '') AS paginaweb, COALESCE(UPPER(t2.giro), '') AS giro, CONCAT(COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idlada) ,concat('(+',t1.lada1,') ')), t1.telefonoEmpresaUno, IF(t1.ext1 IS NULL, '', CONCAT(' Ext. ', t1.ext1)))), ''),COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('\n(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idladados),CONCAT('\n','(+',t1.lada2,') ')), t1.telefonoEmpresaDos, IF(t1.ext2 IS NULL, '', CONCAT(' Ext. ', t1.ext2)))), '')) AS telefonoEmpresa, COALESCE(UPPER(t1.observaciones), '') AS observaciones, COALESCE((SELECT UPPER(CONCAT('Calle: ', t1.calle, ', Número: ', t1.Numero, ', ', t2.tipo, ' ', t2.asentamiento, ', ', municipio, ', ', t2.estado, '. C. P. ', t2.cp)) FROM     sepomex AS t2 WHERE t1.domiciliofksepomex = t2.id), '') AS domicilio, COALESCE(UPPER(t1.aPaterno),'') AS aPaterno, COALESCE( UPPER(t1.AMaterno),'') AS aMaterno, COALESCE( UPPER(t1.nombres),'') AS nombres, COALESCE(t1.correo, '') AS correo, CONCAT(COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idladatres) ,concat('(+',t1.lada3,') ')), t1.telefonoContactoUno, IF(t1.ext3 IS NULL, '', CONCAT(' Ext. ', t1.ext3)))), ''),COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('\n(+',Clave,') ') FROM ladanac WHERE idLadaNac=t1.idladacuatro) ,CONCAT('\n','(+',t1.lada4,') ')), t1.telefonoContactoDos, IF(t1.ext4 IS NULL, '', CONCAT(' Ext. ', t1.ext4)))), '')) AS telefonoContacto, IF(t1.status = 1, UPPER('Activo'), UPPER(CONCAT('No Activo'))), COALESCE(t2.idgiro, '') AS idgiro, t1.idlada AS idlada1, COALESCE(t1.telefonoEmpresaUno, '') AS telempresa1, t1.idladados AS idlada2, COALESCE(t1.telefonoEmpresaDos, '') AS telempresa2, COALESCE(t1.domiciliofksepomex, '') AS iddomicilio, COALESCE(calle, '') AS calle, COALESCE(numero, '') AS numero, COALESCE(referencias, '') AS referencias, t1.idladatres AS idlada3, COALESCE(t1.telefonoContactoUno, '') AS telcontacto1, t1.idladacuatro AS idlada4, COALESCE(t1.telefonoContactoDos, '') AS telcontacto2, COALESCE(ext1, '') AS ext1, COALESCE(ext2, '') AS ext2, COALESCE(ext3, '') AS ext3, COALESCE(ext4, '') AS ext4, COALESCE(lada1,'') as lada1, COALESCE(lada2,'') as lada2, COALESCE(lada3,'') as lada3, COALESCE(lada4,'') as lada4 FROM cproveedores AS t1 LEFT JOIN cgiros AS t2 ON t1.Clasificacionfkcgiros = t2.idgiro  ";
                    if (!string.IsNullOrWhiteSpace(txtbempresa.Text))
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE t1.empresa  LIKE '" + v.mayusculas(txtbempresa.Text.ToLower()) + "%' ";
                        }
                        else
                        {
                            wheres += "AND t1.empresa LIKE '" + v.mayusculas(txtbempresa.Text.ToLower()) + "%'";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtbap.Text))
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE aPaterno  LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%' ";
                        }
                        else
                        {
                            wheres += "AND aPaterno LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%'";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtBNombre.Text))
                    {
                        if (wheres == "")
                        {
                            wheres = "where nombres Like '" + v.mayusculas(txtBNombre.Text.ToLower()) + "%'";
                        }
                        else
                        {
                            wheres += "AND nombres Like '" + v.mayusculas(txtBNombre.Text.ToLower()) + "%'";
                        }
                    }
                    if (cbgirosb.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where Clasificacionfkcgiros  " + (Convert.ToInt32(cbgirosb.SelectedValue) == 0 ? "is NULL" : " = '" + cbgirosb.SelectedValue + "'");
                        }
                        else
                        {
                            wheres += " And Clasificacionfkcgiros " + (Convert.ToInt32(cbgirosb.SelectedValue) == 0 ? "is NULL" : "= '" + cbgirosb.SelectedValue + "'");
                        }
                    }
                    sql += wheres + " ORDER BY t1.empresa ASC";
                    txtbempresa.Clear();
                    txtbap.Clear();
                    txtBNombre.Clear();
                    cbgirosb.SelectedIndex = 0;
                    DataTable dt = (DataTable)v.getData(sql);
                    var res = dt.Rows.Count;
                    if (res == 0)
                    {
                        MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        insertarums();
                        esta_exportandO();
                        pActualizar.Visible = false;
                    }
                    else
                    {
                        for (int i = 0; i < res; i++)
                            tbProveedores.Rows.Add(dt.Rows[i].ItemArray);
                        if (!est_expor)
                        {
                            btnExcel.Visible = true;
                        }
                        LblExcel.Visible = pActualizar.Visible = true;
                    }
                    tbProveedores.ClearSelection();
                    txtbempresa.Clear();
                    txtbap.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un Criterio de Búsqueda", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lnkrestablecerTabla_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            insertarums();
        }

        private void txtobservaciones_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetap_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void txtgetemail_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }

        private void cmbTel_uno_Click(object sender, EventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx.SelectedIndex == 0) cbx.SelectedValue = 10;
        }
        void _ProveedoresExportadas()
        {
            string id;
            int contador = 0;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Proveedores','0','";
            foreach (DataGridViewRow row in tbProveedores.Rows)
            {
                contador++;
                id = row.Cells[0].Value.ToString();
                if (contador < tbProveedores.RowCount)
                {
                    id += ";";
                }
                sql += id;
            }
            sql += "','" + this.idUsuario + "',NOW(),'Exportación a Excel de Catálogo de Proveedores','" + this.empresa + "','" + this.area + "')";
            v.c.insertar(sql);
        }
        void ExportarExcel()
        {
            if (tbProveedores.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < tbProveedores.Columns.Count; i++) if (tbProveedores.Columns[i].Visible) dt.Columns.Add(tbProveedores.Columns[i].HeaderText);
                for (int j = 0; j < tbProveedores.Rows.Count; j++)
                {

                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < tbProveedores.Columns.Count; i++)
                    {

                        if (tbProveedores.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = tbProveedores.Rows[j].Cells[i].Value;
                            indice++;
                        }

                    }
                    dt.Rows.Add(row);
                }
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando);
                    this.Invoke(delega);
                }

                v.exportaExcel(dt);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando1);
                    this.Invoke(delega);
                }
               // _ProveedoresExportadas();
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        delegate void El_Delegado();
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "Exportando";
            pActualizar.Enabled = true;
        }
        delegate void El_Delegado1();
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            if (exportando)
            {
                btnExcel.Visible = false;
                LblExcel.Visible = false;
            }
            exportando = false;
            est_expor = false;
            LblExcel.Text = "Exportar";
        }
        Thread exportar;
        private void btnExcel_Click(object sender, EventArgs e)
        {
            est_expor = true;
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        }

        private void txtobservaciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }

        private void tbProveedores_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                button2.Enabled = txtcp.Enabled = true;
                txtcp.Focus();
                if (_editar)
                {
                    if (!string.IsNullOrWhiteSpace(idSepomexAnterior))
                    {
                        if (Convert.ToInt32(idSepomexAnterior) > 0)
                        {
                            txtcp.Text = v.getcpFromidSepomex(idSepomexAnterior);
                            button2_Click(null, e);
                            cbasentamiento.SelectedValue = idSepomexAnterior;
                        }
                    }
                }
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                button2.Enabled = txtcp.Enabled = false;
                txtcp.Clear();
                if (cbasentamiento.DataSource != null) cbasentamiento.SelectedIndex = 0;
                lblEstado.Text = "";
                lblmunicipio.Text = "";
                lblmunicipio.Text = "";
                lblzona.Text = "";
                lbltipo.Text = "";
                cbasentamiento.DataSource = null;
                lblciudad.Text = "";
                cbasentamiento.Enabled = false;

            }

        }

        private void cmbTel_uno_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void cmbTel_uno_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void txtlada_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbTel_uno.Visible)
                mostrarLada1();
            else
                ocultarLada1();

            p_AñadeTel.Visible = true;
            psegundo_telefono.Visible = false;
            cbTel_dos.SelectedIndex = 0;
            txtTel_dos.Clear();
            txtext2.Clear();
            txtladaEmp2.Clear();
        }
        void mostrarLada1()
        {
            lblLadaCont1.Location = new Point(260, 39);
            lblgLadaCont1.Visible = txtLadaCon1.Visible = true;
            cbladas.Visible = false;
            lnkLadaCon1.Location = new Point(41, 19);
            lnkLadaCon1.Text = "Seleccionar Lada";
            cbladas.SelectedIndex = 0;

            lblLadaEmp1.Location = new Point(231, 39);
            lblgEmp1.Visible = txtladaEmp1.Visible = true;
            cmbTel_uno.Visible = false;
            lnkLada1.Location = new Point(41, 19);
            lnkLada1.Text = "Seleccionar Lada";
            cmbTel_uno.SelectedIndex = 0;
        }
        void ocultarLada1()
        {
            lnkLada1.Location = new Point(213, 43);
            lnkLada1.Text = "Agregar Lada";
            txtladaEmp1.Clear();
            lblgEmp1.Visible = txtladaEmp1.Visible = false;
            lblLadaEmp1.Location = new Point(135, 40);
            cmbTel_uno.Visible = true;
        }
        private void lnklada2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cbTel_dos.Visible)
                mostrarLada2();
            else
                ocultarLada2();
        }
        void mostrarLada2()
        {
            lblLadaEmp2.Location = new Point(231, 39);
            lblgEmp2.Visible = txtladaEmp2.Visible = true;
            cbTel_dos.Visible = false;
            lnklada2.Location = new Point(41, 19);

            lnklada2.Text = "Seleccionar Lada";
            cbTel_dos.SelectedIndex = 0;
        }
        void ocultarLada2()
        {
            lnklada2.Location = new Point(213, 43);
            lnklada2.Text = "Agregar Lada";
            txtladaEmp2.Clear();
            lblgEmp2.Visible = txtladaEmp2.Visible = false;
            lblLadaEmp2.Location = new Point(135, 40);
            cbTel_dos.Visible = true;
        }
        private void txtTel_Uno_TextChanged(object sender, EventArgs e)
        {
            if (_editar) if (string.IsNullOrWhiteSpace(txtTel_Uno.Text.Trim()) && telefonosAnterioresEmpresa[0] != "") cmbTel_uno.SelectedIndex = 0;

            getCambios(null, e);
        }
        private void txtladaEmp2_KeyPress(object sender, KeyPressEventArgs e) { v.ladasManual(e); }
        private void pphone_Paint(object sender, PaintEventArgs e) { }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cbladas.Visible)
                mostrarLada3();
            else
                ocultarLada3();
            paddPhone.Visible = true;
            psecondphone.Visible = false;
            cbladas1.SelectedIndex = 0;
            txtLadaCon2.Clear();
            txtphone.Clear();
            txtext4.Clear();
        }
        void mostrarLada3()
        {
            lblLadaCont1.Location = new Point(260, 39);
            lblgLadaCont1.Visible = txtLadaCon1.Visible = true;
            cbladas.Visible = false;
            lnkLadaCon1.Location = new Point(41, 19);
            lnkLadaCon1.Text = "Seleccionar Lada";
            cbladas.SelectedIndex = 0;
        }

        private void groupBox2_Enter(object sender, EventArgs e) { }
        void ocultarLada3()
        {
            lnkLadaCon1.Location = new Point(213, 43);
            lnkLadaCon1.Text = "Agregar Lada";
            txtLadaCon1.Clear();
            lblgLadaCont1.Visible = txtLadaCon1.Visible = false;
            lblLadaCont1.Location = new Point(135, 40);
            cbladas.Visible = true;
        }
        private void txtTel_dos_TextChanged(object sender, EventArgs e)
        {
            if (_editar) if (string.IsNullOrWhiteSpace(txtTel_dos.Text.Trim())) cbTel_dos.SelectedIndex = 0;
            getCambios(sender, e);
        }
        private void txtgettelefono_TextChanged(object sender, EventArgs e)
        {
            if (_editar) if (string.IsNullOrWhiteSpace(txtgettelefono.Text.Trim())) cbladas.SelectedIndex = 0;
            getCambios(sender, e);
        }
        private void txtphone_TextChanged(object sender, EventArgs e)
        {
            if (_editar) if (string.IsNullOrWhiteSpace(txtphone.Text.Trim())) cbladas1.SelectedIndex = 0;
            getCambios(sender, e);
        }
        public void exportar_pdf()
        {
            string[] datosProveedor = v.getaData(string.Format("SELECT CONCAT(UPPER(t1.empresa),';', COALESCE(t1.paginaweb, '\"SIN PÁGINA\"') ,';', COALESCE(UPPER(t2.giro), '\"SIN CLASIFICACIÓN DE EMPRESA\"'),';', CONCAT(COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('(+',Clave,')') FROM ladanac WHERE idLadaNac=t1.idlada) ,concat('(+',t1.lada1,') ')), t1.telefonoEmpresaUno, IF(t1.ext1 IS NULL, '', CONCAT(' Ext. ', t1.ext1)))), ''),COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('\n(+',Clave,')') FROM ladanac WHERE idLadaNac=t1.idladados),CONCAT('\n',concat('(+',t1.lada2,') '))), t1.telefonoEmpresaDos, IF(t1.ext2 IS NULL, '', CONCAT(' Ext. ', t1.ext2)))), '')) ,';', COALESCE(UPPER(t1.observaciones), '\"DIN OBSERVACIONES\"') ,';', COALESCE((SELECT UPPER(CONCAT('Calle: ', t1.calle, ', Número: ', t1.Numero, ', ', t2.tipo, ' ', t2.asentamiento, ', ', municipio, ', ', t2.estado, '. C. P. ', t2.cp)) FROM sepomex AS t2 WHERE t1.domiciliofksepomex = t2.id), '\"SIN DOMICILIO\"'),';', COALESCE(UPPER(t1.aPaterno),'\"SIN APELLIDO PATERNO\"'),';', COALESCE( UPPER(t1.AMaterno),''),';', COALESCE( UPPER(t1.nombres),''),';', COALESCE(t1.correo, ''),';', CONCAT(COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('(+',Clave,')') FROM ladanac WHERE idLadaNac=t1.idladatres) ,concat('(+',t1.lada3,') ')), t1.telefonoContactoUno, IF(t1.ext3 IS NULL, '', CONCAT(' Ext. ', t1.ext3)))), ''),COALESCE((SELECT CONCAT(COALESCE((SELECT CONCAT('\n(+',Clave,')') FROM ladanac WHERE idLadaNac=t1.idladacuatro) ,CONCAT('\n',concat('(+',t1.lada4,') '))), t1.telefonoContactoDos, IF(t1.ext4 IS NULL, '', CONCAT(' Ext. ', t1.ext4)))), '')) ,';', IF(t1.status = 1, UPPER('Activo'), UPPER(CONCAT('No Activo'))),';',coalesce(upper(t1.Puesto),'')) FROM cproveedores AS t1 LEFT JOIN cgiros AS t2 ON t1.Clasificacionfkcgiros = t2.idgiro where t1.idproveedor ='{0}';", _idproveedorTemp)).ToString().Split(';');
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(20f, 20f, 10f, 10f);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Reporte";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "*.pdf";
            saveFileDialog1.Filter = "Archivos PDF(*.pdf)|*.pdf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            string filename = "";
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    string p = Path.GetExtension(filename);
                    if (p.ToLower() != ".pdf")
                    {
                        filename = filename + ".pdf";
                    }
                    while (filename.ToLower().Contains(".pdf.pdf"))
                    {
                        filename = filename.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
                    }
                }

                if (filename.Trim() != "")
                {
                    FileStream file = new FileStream(filename,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
                    PdfWriter.GetInstance(doc, file);
                    iTextSharp.text.Font arial = FontFactory.GetFont("Calibri", 9, BaseColor.BLACK);
                    iTextSharp.text.Font arial2 = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                    doc.Open();
                    Chunk chunk = new Chunk("PROVEEDOR: " + datosProveedor[0], FontFactory.GetFont("Calibri", 18, iTextSharp.text.Font.BOLD));
                    Phrase salto_linea = new Phrase("\n", arial2);
                    doc.Add(new Phrase(salto_linea));
                    doc.Add(new Paragraph(chunk));
                    doc.Add(new Paragraph(salto_linea));
                    PdfPTable tabla1 = new PdfPTable(3);
                    tabla1.DefaultCell.Border = 0;
                    tabla1.WidthPercentage = 100;
                    PdfPCell celda1 = new PdfPCell();
                    celda1.Border = 0;
                    PdfPCell celda2 = new PdfPCell();
                    celda2.Border = 0;
                    PdfPCell celda3 = new PdfPCell();
                    celda3.Border = 0;
                    PdfPTable tabla2 = new PdfPTable(1);
                    tabla2.DefaultCell.Border = 0;
                    tabla2.WidthPercentage = 100;
                    PdfPCell celda4 = new PdfPCell();
                    celda4.Border = 0;
                    Phrase _empresa = new Phrase("Nombre de Empresa:".ToUpper(), arial2);
                    Phrase empresa = new Phrase(datosProveedor[0], arial);
                    Phrase _pagina = new Phrase("Página Web:".ToUpper(), arial2);
                    Phrase pagina = new Phrase(datosProveedor[1], arial);
                    Phrase _giro = new Phrase("Clasificación:".ToUpper(), arial2);
                    Phrase giro = new Phrase(datosProveedor[2], arial);
                    Phrase _numeros = new Phrase("Numeros teléfonicos:".ToUpper(), arial2);
                    Phrase numeros = new Phrase(datosProveedor[3], arial);
                    Phrase _obser = new Phrase("Observaciones:".ToUpper(), arial2);
                    Phrase obser = new Phrase(datosProveedor[4], arial);
                    Phrase _domicilio = new Phrase("Domicilio de la empresa:".ToUpper(), arial2);
                    Phrase domicilio = new Phrase(datosProveedor[5], arial);
                    Phrase _persona = new Phrase("Persona de contacto:".ToUpper(), arial2);
                    Phrase persona = new Phrase(datosProveedor[6] + " " + datosProveedor[7] + " " + datosProveedor[8], arial);
                    Phrase _correo = new Phrase("Correo electronico".ToUpper(), arial2); ;
                    Phrase correo = new Phrase(datosProveedor[9], arial);
                    Phrase _tel = new Phrase("Telefonos de contacto".ToUpper(), arial2);
                    Phrase tel = new Phrase(datosProveedor[10], arial);
                    Phrase _est = new Phrase("Estatus de proveedor".ToUpper(), arial2);
                    Phrase est = new Phrase(datosProveedor[11], arial);
                    Phrase _puesto = new Phrase("Puesto:".ToUpper(), arial2);
                    Phrase puesto = new Phrase(datosProveedor[12], arial);
                    celda1.AddElement(_empresa);
                    celda1.AddElement(empresa);
                    celda1.AddElement(salto_linea);
                    celda2.AddElement(_pagina);
                    celda2.AddElement(pagina);
                    celda2.AddElement(salto_linea);
                    celda3.AddElement(_giro);
                    celda3.AddElement(giro);
                    celda3.AddElement(salto_linea);
                    celda4.AddElement(_numeros);
                    celda4.AddElement(numeros);
                    celda4.AddElement(salto_linea);
                    celda4.AddElement(_obser);
                    celda4.AddElement(obser);
                    celda4.AddElement(salto_linea);
                    celda4.AddElement(_domicilio);
                    celda4.AddElement(domicilio);
                    celda4.AddElement(salto_linea);
                    celda4.AddElement(_persona);
                    celda4.AddElement(persona);
                    celda4.AddElement(salto_linea);
                    celda4.AddElement(_puesto);
                    celda4.AddElement(puesto);
                    celda4.AddElement(salto_linea);
                    celda4.AddElement(_correo);
                    celda4.AddElement(correo);
                    celda4.AddElement(salto_linea);
                    celda4.AddElement(_tel);
                    celda4.AddElement(tel);
                    celda4.AddElement(salto_linea);
                    celda4.AddElement(_est);
                    celda4.AddElement(est);
                    tabla1.AddCell(celda1);
                    tabla1.AddCell(celda2);
                    tabla1.AddCell(celda3);
                    tabla2.AddCell(celda4);
                    doc.Add(tabla1);
                    doc.Add(tabla2);
                    doc.Close();
                    System.Diagnostics.Process.Start(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtPuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            exportar_pdf();
        }

        private void lnkLadaCon2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cbladas1.Visible)
                mostrarLada4();
            else
                ocultarLada4();
        }
        void mostrarLada4()
        {
            lblLadaCon2.Location = new Point(260, 39);
            lblgLadaCon2.Visible = txtLadaCon2.Visible = true;
            cbladas1.Visible = false;
            lnkLadaCon2.Location = new Point(41, 19);
            lnkLadaCon2.Text = "Seleccionar Lada";
            cbladas1.SelectedIndex = 0;
        }
        void ocultarLada4()
        {
            lnkLadaCon2.Location = new Point(213, 43);
            lnkLadaCon2.Text = "Agregar Lada";
            txtLadaCon2.Clear();
            lblgLadaCon2.Visible = txtLadaCon2.Visible = false;
            lblLadaCon2.Location = new Point(135, 40);
            cbladas1.Visible = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string giro = "";
            if (cbgiros.DataSource != null) if (cbgiros.SelectedIndex > 0) giro = cbgiros.SelectedValue.ToString();
            ladasmanuales[0] = txtladaEmp1.Text.Trim();
            ladasmanuales[1] = txtladaEmp2.Text.Trim();
            ladasmanuales[2] = txtLadaCon1.Text.Trim();
            ladasmanuales[3] = txtLadaCon2.Text.Trim();
            string asentemiento = "";
            string idlada1Validacion = "";
            if (cmbTel_uno.SelectedIndex > 0) idlada1Validacion = cmbTel_uno.SelectedValue.ToString();
            string idlada2Validacion = "";
            if (cbTel_dos.SelectedIndex > 0) idlada2Validacion = cbTel_dos.SelectedValue.ToString();
            string idlada3Validacion = "";
            if (cbladas.SelectedIndex > 0) idlada3Validacion = cbladas.SelectedValue.ToString();
            string idlada4Validacion = "";
            if (cbladas1.SelectedIndex > 0) idlada4Validacion = cbladas1.SelectedValue.ToString();
            if (cbasentamiento.DataSource != null) asentemiento = cbasentamiento.SelectedValue.ToString();
            if (!string.IsNullOrWhiteSpace(_idproveedorTemp) && status == 1 && ((!string.IsNullOrWhiteSpace(txtgetempresa.Text)) && (!_empresaAnterior.Equals(v.mayusculas(txtgetempresa.Text.Trim().ToLower())) || !_apAnterior.Equals(v.mayusculas(txtgetap.Text.Trim().ToLower())) || !_amAnterior.Equals(v.mayusculas(txtgetam.Text.Trim().ToLower())) || !_nombreAnterior.Equals(v.mayusculas(txtgetnombre.Text.Trim().ToLower())) || !_puestoAnterior.Equals(txtPuesto.Text.Trim()) || !_paginaweb.Equals(txtweb.Text) || !(giroAnterior ?? "").Equals((giro)) || !(_idladaanterior1).Equals(idlada1Validacion) || !telefonosAnterioresEmpresa[0].Equals(txtTel_Uno.Text) || !extAnterior1.Equals(txtext1.Text) || !(_idladaanterior2).Equals(idlada2Validacion)) || !telefonosAnterioresEmpresa[1].Equals(txtTel_dos.Text) || !extAnterior2.Equals(txtext2.Text) || !ObservacionesAnterior.Equals(txtobservaciones.Text.Trim()) || !idSepomexAnterior.Equals((idSepomex ?? "")) || !v.mayusculas(calleAnterior.ToLower()).Equals(v.mayusculas(txtcalle.Text.Trim().ToLower())) || !v.mayusculas(NumeroAnterior.ToLower()).Equals(v.mayusculas(txtnum.Text.Trim().ToLower())) || !v.mayusculas(referenciasAnterior.ToLower()).Equals(v.mayusculas(txtreferencias.Text.Trim().ToLower())) || !_correoAnterior.Equals(txtgetemail.Text) || !(_idladaanterior3).Equals(idlada3Validacion) || !telefonsAnterioresContacto[0].Equals(txtgettelefono.Text) || !extAnterior3.Equals(txtext3.Text) || !(_idladaanterior4).Equals(idlada4Validacion) || !telefonsAnterioresContacto[1].Equals(txtphone.Text) || !extAnterior4.Equals(txtext4.Text) || !txtweb.Text.Equals(_paginaweb) || !ladasmanuales[0].Equals(_ladaManual1) || !ladasmanuales[1].Equals((_ladaManual2)) || !ladasmanuales[2].Equals((_ladaManual3)) || !ladasmanuales[3].Equals((_ladaManual4))))
            {

                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnguardar_Click(null, e);
                }
                else
                {
                    esta_exportandO();
                    limpiar();
                }
            }
            else
            {
                esta_exportandO();
                limpiar();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button2_Click(null, e);
            else
                v.Solonumeros(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string cp = txtcp.Text;
                lblEstado.Text = "";
                lblmunicipio.Text = "";
                lblzona.Text = "";
                lbltipo.Text = "";
                lblciudad.Text = "";
                if (!string.IsNullOrWhiteSpace(cp))
                {
                    string sql = "SELECT  DISTINCT COUNT(id) as id, estado,municipio,COALESCE(ciudad,'') AS ciudad FROM sepomex WHERE cp='" + cp + "'";
                    MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());

                    if (Convert.ToInt32(cm.ExecuteScalar()) > 0)
                    {
                        MySqlDataReader dr = cm.ExecuteReader();
                        dr.Read();

                        lblEstado.Text = dr.GetString("estado").ToUpper();
                        lblmunicipio.Text = dr.GetString("municipio").ToUpper();
                        lblciudad.Text = dr.GetString("ciudad").ToUpper();
                        dr.Close();
                        v.c.dbcon.Close();
                        cbasentamiento.Enabled = true;
                        sql = "SELECT id, upper(asentamiento) as asentamiento FROM sepomex WHERE cp ='" + cp + "' order by asentamiento ASC";
                        DataTable dt1 = new DataTable();
                        MySqlCommand cm1 = new MySqlCommand(sql, v.c.dbconection());

                        MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm1);
                        AdaptadorDatos.Fill(dt1);
                        DataRow nuevaFila = dt1.NewRow();
                        nuevaFila["id"] = 0;
                        nuevaFila["asentamiento"] = "--Seleccione un Asentamiento--".ToUpper();
                        dt1.Rows.InsertAt(nuevaFila, 0);
                        cbasentamiento.DataSource = dt1;
                        cbasentamiento.ValueMember = "id";
                        cbasentamiento.DisplayMember = "asentamiento";
                        if (dt1.Rows.Count == 2)
                        {
                            cbasentamiento.SelectedIndex = 1;
                            cbasentamiento.Enabled = false;
                        }
                        else
                        {
                            cbasentamiento.Enabled = true;
                        }
                        v.c.dbcon.Close();

                    }
                    else
                    {
                        MessageBox.Show("No se Encontró el Código Postal", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        vaciarcombo();
                        txtcp.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Teclee un Código Postal Válido", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    vaciarcombo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void vaciarcombo()
        {

            cbasentamiento.DataSource = null;
            cbasentamiento.Items.Clear();
            cbasentamiento.Enabled = false;
            idSepomex = null;
        }
        private void cbasentamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbasentamiento.SelectedIndex > 0)
            {
                String sql = "SELECT zona,tipo from sepomex WHERE id='" + cbasentamiento.SelectedValue + "'";
                MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                if (dr.FieldCount > 0)
                {
                    dr.Read();
                }
                lblzona.Text = dr.GetString("zona").ToUpper();
                lbltipo.Text = dr.GetString("tipo").ToUpper();
                idSepomex = cbasentamiento.SelectedValue.ToString();
                dr.Close();
                v.c.dbcon.Close();
            }
            else
            {
                lblzona.Text = "";
                lbltipo.Text = "";
                idSepomex = null;
            }
         ;

        }

        private void txtweb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnguardar_Click(null, e);
            else
                v.paraPaginasWeb(e);
        }

        private void txtreferencias_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnguardar_Click(null, e);
            else if (e.KeyChar == 46)
                e.Handled = false;
            else
                v.letrasnumerosdiagonalyguion(e);
        }

        private void txtgetempresa_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnguardar_Click(null, e);
            else
                v.paraEmpresas(e);
        }

        private void cbladas_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtgetempresa_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void gbinsertmodif_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        bool exportando = false;
        void esta_exportandO()
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
        private void button4_Click(object sender, EventArgs e)
        {
            insertarums();
            pActualizar.Visible = false;
            esta_exportandO();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((cbladas.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtLadaCon1.Text.Trim())) && !string.IsNullOrWhiteSpace(txtgettelefono.Text))
            {
                psecondphone.Visible = true;
                paddPhone.Visible = false;
            }
            else
            {
                MessageBox.Show("No se Puede Agregar Un Segundo Teléfono Si No Ha Completado el Primero", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                psecondphone.Visible = false;
                paddPhone.Visible = true;

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            catGiros cat = new catGiros(idUsuario, empresa, area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if ((cmbTel_uno.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtladaEmp1.Text)) && !string.IsNullOrWhiteSpace(txtTel_Uno.Text))
            {
                psegundo_telefono.Visible = true;
                p_AñadeTel.Visible = false;

            }
            else
            {
                MessageBox.Show("No se Puede Agregar Un Segundo Teléfono Si No Ha Completado el Primero", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                psegundo_telefono.Visible = false;
                p_AñadeTel.Visible = true;
            }
        }

        private void tbProveedores_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void cbgiros_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtgetemail_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgetemail.Text.Trim()))
            {
                if (!v.validacionCorrero(txtgetemail.Text.Trim()))
                {
                    e.Cancel = true;
                    MessageBox.Show("El formato del email ingresado es incorrecto", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtweb_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtweb.Text.Trim()))
            {
                if (v.paginaWebValida(txtweb.Text.Trim()))
                {
                    e.Cancel = true;
                    MessageBox.Show("Introduzca Una Página Web Válida.", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        void iniLadas()
        {
            String sql = "SET lc_time_names = 'es_ES';SELECT idLadaNac as id, UPPER(CONCAT(Localidad,' - (+',Clave,')')) as lada FROM ladanac ORDER BY Localidad ASC";
            DataTable dt1 = new DataTable();
            MySqlCommand cm1 = new MySqlCommand(sql, v.c.dbconection());

            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm1);
            AdaptadorDatos.Fill(dt1);

            DataRow nuevaFila = dt1.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["lada"] = "--SELECCIONE LADA--";
            dt1.Rows.InsertAt(nuevaFila, 0);
            DataTable t2 = dt1.Copy();
            DataTable t3 = dt1.Copy();
            DataTable t4 = dt1.Copy();
            cbladas.DataSource = dt1;
            cbladas1.ValueMember = cbladas.ValueMember = "id";
            cbladas1.DisplayMember = cbladas.DisplayMember = "lada";
            cbladas1.DataSource = t2;
            cmbTel_uno.ValueMember = cbladas.ValueMember = "id";
            cmbTel_uno.DisplayMember = cbladas.DisplayMember = "lada";
            cmbTel_uno.DataSource = t3;
            cbTel_dos.ValueMember = cbladas.ValueMember = "id";
            cbTel_dos.DisplayMember = cbladas.DisplayMember = "lada";
            cbTel_dos.DataSource = t4;

            v.c.dbconection().Close();
        }
        bool mostrarMotivoEdicion(string[,] cambios)
        {
            bool res = false;
            for (int i = 0; i < cambios.GetLength(0); i++)
            {
                if (!string.IsNullOrWhiteSpace(cambios[i, 0])) if (!cambios[i, 0].Equals(cambios[i, 1])) res = true;
            }
            return res;
        }
    }

}
