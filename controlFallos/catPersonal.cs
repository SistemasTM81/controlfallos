using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Threading;
namespace controlFallos
{
    public partial class catPersonal : Form
    {
        Thread exportar;
        int statusTemp, idUsuarioTemp, idUsuario, empresa, area, acces_s, qualityTemplate, qualityTemplateAnterior;
        public int tipoTemp;
        string credencialAnterior = "", apAnterior = "", amAnterior = "", nombresAnterior = "", usuarioAnterior = "", passwordAnterior = "", tipolicenciaAnterior, template, templateAnterior;
        public string puestoAnterior = "";
        bool reactivar, accesSistemaAnterior;
        Thread th;
        public bool editar { protected internal set; get; }
        DateTime expedicionTarjetonAnterior, vencimientoTarjetonAnterior, expedicionlicenciaAnterior, vencimientolicenciaAnterior;
        validaciones v;
        new menuPrincipal Owner;
        public catPersonal(int idUsuario, int empresa, int area, Image logo, menuPrincipal f, validaciones v)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.v = v;
            this.idUsuario = idUsuario;
            csetpuestos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbaccess.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            csetbpuestos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            busqEmpleados.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            busqEmpleados.ColumnAdded += new DataGridViewColumnEventHandler(v.paraDataGridViews_ColumnAdded);
            cbstatus.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.area = area;
            this.empresa = empresa;
            pblogo.BackgroundImage = logo;
            v.ChangeControlStyles(button1, ControlStyles.Selectable, false);
            v.ChangeControlStyles(button3, ControlStyles.Selectable, false);
            v.ChangeControlStyles(btnExcel, ControlStyles.Selectable, false);
            v.ChangeControlStyles(button4, ControlStyles.Selectable, false);
            Owner = f;
            cbstatus.DrawItem += new DrawItemEventHandler(v.comboBoxEstatus_DrwaItem);
        }
        private void catPersonal_Load(object sender, EventArgs e)
        {
            if (empresa != 1) { gbvigencias.Text = ""; gbtarjeton.Visible = false; gbvigencias.Location = new Point(990, 30); }
            pLicencias.Visible = Convert.ToInt32(v.getaData("SELECT ver FROM privilegios WHERE namform='catTipos' AND usuariofkcpersonal='" + idUsuario + "'")) == 1;
            privilegiosPersonal();
            busqPuestos();
            if (Pconsultar) { busemp(); Estatus(); }
            iniacceso();
            txtgetcredencial.Focus();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(SplashScreen))
                {
                    if (frm.InvokeRequired)
                    {
                        validaciones.delgado dm = new validaciones.delgado(v.cerrarForm);
                        Invoke(dm, frm);
                        break;
                    }
                }
            }
            th.Abort();

        }
        public void busqPuestos()
        {
            v.iniCombos("SELECT idpuesto,UPPER(puesto) as puesto FROM puestos WHERE  empresa = " + empresa + " and area = '" + this.area + "' and status= 1 ORDER BY puesto ASC", csetpuestos, "idpuesto", "puesto", "-- seleccione un puesto --");
            v.iniCombos("SELECT idpuesto,UPPER(puesto) as puesto FROM puestos WHERE  empresa = " + empresa + " and area = '" + this.area + "'  ORDER BY puesto ASC", csetbpuestos, "idpuesto", "puesto", "-- seleccione un puesto --");
            csetpuestos.Enabled = csetbpuestos.Enabled = true;
        }
        void Estatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idnivel");
            dt.Columns.Add("Nombre");
            DataRow row = dt.NewRow();
            row["idnivel"] = 0;
            row["Nombre"] = "--Seleccione estatus--".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 1;
            row["Nombre"] = "Activo".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 2;
            row["Nombre"] = "No activo".ToUpper();
            dt.Rows.Add(row);
            cbstatus.ValueMember = "idnivel".ToUpper();
            cbstatus.DisplayMember = "Nombre";
            cbstatus.DataSource = dt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string credencial = txtgetcredencial.Text;
                String ap = v.mayusculas(txtgetap.Text.ToLower());
                String am = v.mayusculas(txtgetam.Text.ToLower());
                string nombre = v.mayusculas(txtgetnombre.Text.ToLower());
                int puesto = Convert.ToInt32(csetpuestos.SelectedValue);
                if (!editar) insertar(credencial, ap, am, nombre, puesto);
                else { if (getcambios()) _editar(credencial, ap, am, nombre, puesto); }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }
        void _editar(string credencial, string ap, string am, string nombre, int puesto)
        {
            if (statusTemp == 0) MessageBox.Show("No Puede Modificar A Un Usuario Inactivo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (!v.camposvacioscPersonal(credencial, ap, am, nombre, puesto, empresa, area, Convert.ToInt32(cbtipo.SelectedValue), dtpexptrajeton.Value, dtpvenctarjeton.Value, dtpexpconducir.Value, dtpvencconducir.Value, Convert.ToInt32(cbaccess.SelectedValue), editar, cbtipo.DataSource != null) && !v.yaExisteActualizar(credencial, CredencialAnterior, ap, ApAnterior, am, AmAnterior, nombre, NombresAnterior))
                {
                    if ((cbaccess.SelectedIndex == 1 && v.formulariousu(txtgetusu.Text.Trim(), txtgetpass.Text.Trim(), txtgetpass2.Text.Trim()))) { }
                    else if (cbaccess.SelectedIndex == 1 && !v.formulariousu(txtgetusu.Text.Trim(), txtgetpass.Text.Trim(), txtgetpass2.Text.Trim()) && !PasswordAnterior.Equals(v.Encriptar(txtgetpass.Text.Trim())) && v.existeusupass(txtgetpass.Text.Trim())) { }
                    else
                    {
                        DialogResult res = DialogResult.OK;
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        string edicion = "";
                        if (empresa == 2 && (area == 1 || area == 2))
                        {
                            string licencia = null;
                            if (cbtipo.DataSource != null) if (cbtipo.SelectedIndex > 1) licencia = cbtipo.SelectedValue.ToString();
                            string LICaNT = null;
                            if (expedicionlicenciaAnterior != DateTime.Today) LICaNT = expedicionlicenciaAnterior.ToString("yyy/MM/dd");
                            string venLicAnt = null;
                            if (vencimientolicenciaAnterior != DateTime.Today) venLicAnt = vencimientolicenciaAnterior.ToString("yyy/MM/dd");
                            if (v.mostrarMotivoEdicion(new string[10, 2] { { CredencialAnterior, credencial }, { ApAnterior, ap }, { AmAnterior, am }, { NombresAnterior, nombre }, { tipoTemp.ToString(), puesto.ToString() }, { UsuarioAnterior, txtgetusu.Text.Trim() }, { v.Desencriptar(passwordAnterior), txtgetpass.Text.Trim() }, { tipolicenciaAnterior, licencia }, { LICaNT, dtpexpconducir.Value.ToString("yyy/MM/dd") }, { venLicAnt, dtpvencconducir.Value.ToString("yyyy/MM/dd") } })) { res = obs.ShowDialog(); edicion = obs.txtgetedicion.Text.Trim(); }
                        }
                        else if (empresa == 1 && area == 1)
                        {
                            if (v.mostrarMotivoEdicion(new string[,] { { templateAnterior, template } }))
                            {
                                res = obs.ShowDialog(); edicion = obs.txtgetedicion.Text.Trim();
                            }
                        }
                        if (res == DialogResult.OK) modificarInfo(credencial, ap, am, nombre, puesto, edicion);
                    }
                }
                if (yaAparecioMensaje) { limpiar(); if (Pconsultar) busemp(); }
            }
        }
        void modificarHuella(string edicion)
        {
            if (!string.IsNullOrWhiteSpace(template))
            {

                if (!string.IsNullOrWhiteSpace(templateAnterior))
                {
                    v.c.insertar("UPDATE huellasupervision SET template =  '" + template + "',calidad = '" + qualityTemplate + "' WHERE PersonafkCpersonal = '" + idUsuarioTemp + "'");
                    if (string.IsNullOrWhiteSpace(usuarioAnterior)) v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','Actualización de Huella','" + idUsuario + "',NOW(),'Actualización DE Huella Digital','" + edicion + "','" + empresa + "','" + area + "')");
                }
                else
                    v.c.insertar("INSERT INTO huellasupervision (PersonafkCpersonal,template,calidad) VALUES ('" + idUsuarioTemp + "','" + template + "','" + qualityTemplate + "')");

                if (!yaAparecioMensaje) { MessageBox.Show("Empleado Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information); yaAparecioMensaje = true; }
            }
        }
        void modificarInfo(string credencial, string ap, string am, string nombre, int puesto, string edicion)
        {
            modificarHuella(edicion);
            actualizarDatosP(credencial, ap, am, nombre, puesto, edicion);
            if (cbaccess.SelectedIndex == 1)
            {
                string usuarioTemp = txtgetusu.Text.Trim();
                string password = txtgetpass.Text.Trim();
                string confirmpassword = txtgetpass2.Text.Trim();
                if (!usuarioAnterior.Equals(usuarioTemp) || !v.Desencriptar(PasswordAnterior).Equals(v.Desencriptar(password)))
                {
                    if (!v.formulariousu(usuarioTemp, password, confirmpassword))
                    {
                        if (!accesSistemaAnterior)
                        {
                            if (!v.existeusupass(password))
                            {
                                if (v.c.insertar("INSERT INTO datosistema(usuariofkcpersonal, usuario, password) VALUES('" + this.idUsuarioTemp + "','" + usuarioTemp + "','" + v.Encriptar(password) + "')"))
                                {
                                    if (string.IsNullOrWhiteSpace(usuarioAnterior)) v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + usuarioTemp + ";" + v.Encriptar(password) + "','" + idUsuario + "',NOW(),'Inserción de Usuario','" + edicion + "','" + empresa + "','" + area + "')");
                                    else v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + usuarioAnterior + ";" + v.Encriptar(PasswordAnterior) + "','" + idUsuario + "',NOW(),'Inserción de Usuario','" + edicion + "','" + empresa + "','" + area + "')");

                                    if (!yaAparecioMensaje) { MessageBox.Show("Empleado Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information); yaAparecioMensaje = true; }
                                }
                            }
                        }
                        else
                        {
                            if (!UsuarioAnterior.Equals(txtgetusu.Text.Trim()) || !PasswordAnterior.Equals(v.Encriptar(txtgetpass.Text.Trim())))
                            {
                                if (!v.existeusupassActualizar(usuarioTemp, UsuarioAnterior, password, v.Desencriptar(PasswordAnterior)))
                                {
                                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + UsuarioAnterior + ";" + PasswordAnterior + "','" + idUsuario + "',NOW(),'Actualización de Usuario','" + edicion + "','" + empresa + "','" + area + "')");
                                    v.c.insertar("UPDATE cpersonal SET credencial=LTRIM(RTRIM('" + Convert.ToInt32(credencial) + "')),apPaterno=LTRIM(RTRIM('" + ap + "')),apMaterno=LTRIM(RTRIM('" + am + "')),nombres=LTRIM(RTRIM('" + nombre + "')),cargofkcargos=LTRIM(RTRIM('" + puesto + "')) WHERE idPersona =" + this.idUsuarioTemp);
                                    var res = v.c.insertar("UPDATE datosistema SET usuario= '" + usuarioTemp + "',password='" + v.Encriptar(password) + "' WHERE usuariofkcpersonal='" + idUsuarioTemp + "'");

                                    if (res) { if (!yaAparecioMensaje) { MessageBox.Show("Empleado Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information); yaAparecioMensaje = true; } }
                                }
                            }
                        }
                    }
                }
                if (privilegios != null)
                {
                    var res = privilegios.GetLength(0);
                    var idUsuarioTemp = Convert.ToInt32(v.getaData("SELECT idpersona FROM cpersonal WHERE credencial = '" + credencial + "'"));
                    for (int i = 0; i < privilegios.GetLength(0); i++)
                    {
                        string ver = privilegios[i, 0];
                        string insertar = privilegios[i, 1];
                        string consultar = privilegios[i, 2];
                        string modificar = privilegios[i, 3];
                        string eliminar = privilegios[i, 4];
                        string nombref = privilegios[i, 5];
                        v.insert(ver, insertar, consultar, modificar, eliminar, nombref, idUsuarioTemp);
                    }
                    if (!yaAparecioMensaje) { MessageBox.Show("Empleado Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information); yaAparecioMensaje = true; }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(usuarioAnterior))
                    eliminarUsuario(edicion);
            }
        }
        void actualizarFechas()
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(idvigencia) FROM vigencias_supervision WHERE usuariofkcpersonal='" + idUsuarioTemp + "'")) > 0)
            {
                string campos = "";
                int ipo = 0; if (cbtipo.DataSource != null) ipo = Convert.ToInt32(cbtipo.SelectedValue);
                if (ipo > 0)
                {
                    if (empresa == 1 && area == 1) campos = "tipolicenciafkcattipos='" + cbtipo.SelectedValue + "',fechaEmisionTarjeton='" + dtpexptrajeton.Value.ToString("yyyy/MM/dd") + "',fechaVencimientoTarjeton='" + dtpvenctarjeton.Value.ToString("yyyy/MM/dd") + "', fechaEmisionConducir='" + dtpexpconducir.Value.ToString("yyyy/MM/dd") + "',fechaVencimientoConducir='" + dtpvencconducir.Value.ToString("yyyy/MM/dd") + "'";
                    else campos = "tipolicenciafkcattipos='" + cbtipo.SelectedValue + "',fechaEmisionConducir='" + dtpexpconducir.Value.ToString("yyyy/MM/dd") + "',fechaVencimientoConducir='" + dtpvencconducir.Value.ToString("yyyy/MM/dd") + "'";
                    if (!string.IsNullOrWhiteSpace(campos)) v.c.insertar("UPDATE vigencias_supervision SET " + campos + " WHERE usuariofkcpersonal='" + idUsuarioTemp + "'");
                }
                else v.c.insertar("DELETE FROM vigencias_supervision WHERE usuariofkcpersonal='" + idUsuarioTemp + "'");
            }
            else
                insertarVigencias(idUsuarioTemp);
        }
        bool yaAparecioMensaje = false;
        void eliminarUsuario(string observaciones)
        {
            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + UsuarioAnterior + ";" + PasswordAnterior + "','" + idUsuario + "',NOW(),'Eliminación de Usuario','" + observaciones + "','" + empresa + "','" + area + "')");
            v.EliminarPrivilegios(idUsuarioTemp);
            var res = v.c.insertar("DELETE FROM datosistema WHERE usuariofkcpersonal =" + idUsuarioTemp);
        }
        public void actualizarDatosP(string credencial, string ap, string am, string nombres, int puesto, string razonEdicion)
        {
            if (cbtipo.SelectedIndex > 1)
            {
                Fecha_expL = string.Format("{0:D}", dtpexpconducir.Value);
                Fecha_venL = string.Format("{0:D}", dtpvencconducir.Value);
                Fecha_expT = string.Format("{0:D}", dtpexptrajeton.Value);
                Fecha_venT = string.Format("{0:D}", dtpvenctarjeton.Value);
            }
            string _idpuestoanterior = v.getaData("select idpuesto from puestos where upper(puesto)='" + puestoAnterior.ToUpper() + "' and empresa ='" + this.empresa + "'and area ='" + this.area + "'").ToString();
            if (!v.camposvacioscPersonal(credencial, ap, am, nombres, puesto, empresa, area, Convert.ToInt32(cbtipo.SelectedValue), dtpexptrajeton.Value, dtpvenctarjeton.Value, dtpexpconducir.Value, dtpvencconducir.Value, Convert.ToInt32(cbaccess.SelectedValue), editar, cbtipo.DataSource != null) && !v.yaExisteActualizar(credencial, CredencialAnterior, ap, ApAnterior, am, AmAnterior, nombres, NombresAnterior))
            {
                if ((!CredencialAnterior.Equals(txtgetcredencial.Text) || !ApAnterior.Equals(v.mayusculas(txtgetap.Text.ToLower()).Trim()) || !AmAnterior.Equals(v.mayusculas(txtgetam.Text.ToLower()).Trim()) || !NombresAnterior.Equals(v.mayusculas(txtgetnombre.Text.ToLower()).Trim()) || tipoTemp != (int)csetpuestos.SelectedValue || cbaccess.SelectedIndex != acces_s || dtpexpconducir.Value != expedicionlicenciaAnterior || vencimientolicenciaAnterior != dtpvencconducir.Value || expedicionTarjetonAnterior != dtpexptrajeton.Value || vencimientoTarjetonAnterior != dtpvenctarjeton.Value || !tipolicenciaAnterior.Equals(cbtipo.SelectedValue.ToString())))
                {
                    actualizarFechas();
                    string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion ,empresa, area) VALUES('Catálogo de Personal', '" + idUsuarioTemp + "', '" + CredencialAnterior + ";" + ApAnterior + ";" + AmAnterior + ";" + NombresAnterior + ";";
                    int tipoAnt = 0; if (!string.IsNullOrWhiteSpace(TipolicenciaAnterior)) tipoAnt = Convert.ToInt32(TipolicenciaAnterior);
                    if (tipoAnt > 0)
                        sql += tipolicenciaAnterior + ";" + expedicionlicenciaAnterior + ";" + vencimientolicenciaAnterior + ";";
                    else
                        sql += ";;;";
                    if (this.area == 1 && this.empresa == 1)
                        sql += expedicionTarjetonAnterior + ";" + vencimientoTarjetonAnterior + ";" + _idpuestoanterior + "','" + idUsuario + "',NOW(),'Actualización de Datos Personales','" + razonEdicion + "','" + empresa + "','" + area + "')";
                    else
                        sql += _idpuestoanterior + "','" + idUsuario + "',NOW(),'Actualización de Datos Personales','" + razonEdicion + "','" + empresa + "','" + area + "')";
                    var res2 = v.c.insertar(sql);
                    var res = false;
                    string cred = "NULL";
                    if (!string.IsNullOrWhiteSpace(txtgetcredencial.Text.Trim())) cred = "'" + txtgetcredencial.Text + "'";

                    res = v.c.insertar("UPDATE cpersonal SET credencial=" + cred + ",apPaterno=LTRIM(RTRIM('" + ap + "')),apMaterno=LTRIM(RTRIM('" + am + "')),nombres=LTRIM(RTRIM('" + nombres + "')),cargofkcargos=LTRIM(RTRIM('" + puesto + "')) WHERE idPersona =" +
                       this.idUsuarioTemp);

                    if (empresa == 1 && area == 1) v.c.insertar("UPDATE vigencias_supervision SET fechaEmisionTarjeton='" + dtpexptrajeton.Value.ToString("yyyy-MM-dd") + "', fechaVencimientoTarjeton='" + dtpvenctarjeton.Value.ToString("yyyy-MM-dd") + "', tipolicenciafkcattipos='" + cbtipo.SelectedValue + "', fechaEmisionConducir='" + dtpexpconducir.Value.ToString("yyyy-MM-dd") + "', fechaVencimientoConducir='" + dtpvencconducir.Value.ToString("yyyy-MM-dd") + "' WHERE usuariofkcpersonal='" + idUsuarioTemp + "'");
                    else
                        v.c.insertar("UPDATE vigencias_supervision SET tipolicenciafkcattipos='" + cbtipo.SelectedValue + "', fechaEmisionConducir='" + dtpexpconducir.Value.ToString("yyyy-MM-dd") + "', fechaVencimientoConducir='" + dtpvencconducir.Value.ToString("yyyy-MM-dd") + "' WHERE usuariofkcpersonal='" + idUsuarioTemp + "'");
                    if (!yaAparecioMensaje) { MessageBox.Show("Empleado Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information); yaAparecioMensaje = true; }
                }
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button1_Click(null, e);
            else
                v.Sololetras(e);
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button1_Click(null, e);
            else
                v.paraUsuarios(e);
        }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button1_Click(null, e);
            else
                v.numeroyLetrasSinAcentos(e);
        }
        private void txtgeteco_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button1_Click(null, e);
            else
                v.Solonumeros(e);
        }
        public void limpiar()
        {
            if (Pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
                lblguardar.Text = "Guardar";
                editar = false;
                gbemp.Text = "Agregar Empleado";
                txtgetcredencial.Focus();
            }
            else
            {
                gbemp.Enabled = false;
                gbemp.Text = "";
            }
            reactivar = peliminarusu.Visible = pprivilegios.Visible = pCancel.Visible = yaAparecioMensaje = !(cbaccess.Enabled = btnguardar.Visible = lblguardar.Visible = true);
            txtgetcredencial.Clear();
            txtgetap.Clear();
            txtgetam.Clear();
            txtgetnombre.Clear();
            txtgetusu.Clear();
            pHD.Visible = false;
            txtgetpass.Clear();
            acces_s = cbaccess.SelectedIndex = this.idUsuarioTemp = 0;
            txtgetpass2.Clear();
            btnlimpiar.BackgroundImage = Properties.Resources.eraser;
            lbllimpiar.Text = "Limpiar";
            TipolicenciaAnterior = PasswordAnterior = UsuarioAnterior = puestoAnterior = CredencialAnterior = NombresAnterior = AmAnterior = ApAnterior = "";
            privilegios = null;
            busqPuestos();
            lblprivilegios.Text = "Asignar Privilegios";
            mostrarLicencias();
            dtpexpconducir.Value = dtpexptrajeton.Value = dtpvencconducir.Value = dtpvenctarjeton.Value = DateTime.Today;
            if (cbtipo.DataSource != null) cbtipo.SelectedIndex = 0;
            templateAnterior = template = null;
            qualityTemplateAnterior = qualityTemplate = 0;
            label17.Text = "Registrar Huella";
        }
        public void busemp()
        {
            busqEmpleados.DataSource = null;
            if (empresa == 1 && area == 1)
            {
                busqEmpleados.DataSource = v.getData(@"Set names 'utf8';SET lc_time_names='es_ES';SELECT t1.idPersona as id, t1.credencial as 'CREDENCIAL', UPPER(t1.ApPaterno) as 'APELLIDO PATERNO', UPPER(t1.ApMaterno) as 'APELLIDO MATERNO', UPPER(t1.nombres) as 'NOMBRES', UPPER(t2.puesto) as 'PUESTO', (SELECT UPPER(CONCAT(nombres, ' ', apPaterno, ' ', ApMaterno)) from cpersonal WHERE idPersona = t1.idPersonalaltafkpersona) as 'PERSONA QUE DIÓ DE ALTA',t1.cargofkcargos as cargo, t1.area,COALESCE(t3.usuario, '') as 'USUARIO',coalesce(upper(CONCAT('Exp: \n', DATE_FORMAT(t4.fechaEmisionTarjeton,   '%d/ %m/%Y'), '\n Vencimiento: \n', DATE_FORMAT(t4.fechaVencimientoTarjeton,  '%d/ %m/%Y'))), '') AS 'TARJETÓN',if(t5.Descripcion='',T5.Tipo,COALESCE(CONCAT(T5.Tipo,' - ',t5.Descripcion),'')) AS 'TIPO DE LICENCIA' ,coalesce(UPPER(CONCAT('Exp: \n', DATE_FORMAT(t4.fechaEmisionConducir,  '%d/ %m/%Y'), '\n Vencimiento: \n', DATE_FORMAT(t4.fechaVencimientoConducir, '%d/%m/%Y'))), '') AS 'LICENCIA DE CONDUCIR', if (t1.status = 1,'ACTIVO',CONCAT('NO ACTIVO')) as 'ESTATUS',COALESCE(t3.password, '') as pass,COALESCE(t4.fechaEmisionTarjeton,''), COALESCE(t4.fechaVencimientoTarjeton,''),COALESCE(t4.fechaEmisionConducir,''), COALESCE(t4.fechaVencimientoConducir,''),COALESCE(t5.idcattipos,'') AS 'idtipo' FROM cpersonal as t1 INNER JOIN  puestos as t2 ON t1.cargofkcargos = t2.idpuesto LEFT JOIN datosistema as t3 ON t3.usuariofkcpersonal = t1.idPersona LEFT JOIN vigencias_supervision as t4 ON t4.usuariofkcpersonal = t1.idPersona left JOIN cattipos as t5 ON t4.tipolicenciafkcattipos=t5.idcattipos WHERE t1.empresa = '" + empresa + "' and t1.area = '" + area + "' and t1.status='1'  ORDER BY t1.credencial ASC");
                if (busqEmpleados.DataSource != null)
                    busqEmpleados.Columns[14].Visible = busqEmpleados.Columns[15].Visible = busqEmpleados.Columns[16].Visible = busqEmpleados.Columns[17].Visible = busqEmpleados.Columns[18].Visible = busqEmpleados.Columns[19].Visible = false;
            }
            else
            {
                busqEmpleados.DataSource = v.getData("Set names 'utf8';SET lc_time_names='es_ES';SELECT t1.idPersona as id, t1.credencial as 'CREDENCIAL', UPPER(t1.ApPaterno) as 'APELLIDO PATERNO', UPPER(t1.ApMaterno) as 'APELLIDO MATERNO', UPPER(t1.nombres) as 'NOMBRES', UPPER(t2.puesto) as 'PUESTO',(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',ApMaterno)) from cpersonal WHERE idPersona=t1.idPersonalaltafkpersona)as 'PERSONA QUE DIÓ DE ALTA',t1.cargofkcargos as cargo, t1.area,COALESCE(t3.usuario,'') as 'USUARIO',COALESCE(t3.password,'') as pass,if(t5.Descripcion='',T5.Tipo,COALESCE(CONCAT(T5.Tipo,' - ',t5.Descripcion),'')) AS 'TIPO DE LICENCIA' ,coalesce(UPPER(CONCAT('Expedición: \n',  DATE_FORMAT(t4.fechaEmisionConducir,  '%d/ %m/%Y'), '\n Vencimiento: \n', DATE_FORMAT(t4.fechaVencimientoConducir, '%d/%m/%Y'))), '') AS 'LICENCIA DE CONDUCIR', if(t1.status=1,'ACTIVO',CONCAT('NO ACTIVO')) as 'ESTATUS',COALESCE(t4.fechaEmisionConducir,''), COALESCE(t4.fechaVencimientoConducir,''),COALESCE(t5.idcattipos,'') AS 'idtipo' FROM cpersonal as t1 INNER JOIN  puestos as t2 ON t1.cargofkcargos = t2.idpuesto LEFT JOIN datosistema as t3 ON t3.usuariofkcpersonal = t1.idPersona LEFT JOIN vigencias_supervision as t4 ON t4.usuariofkcpersonal = t1.idPersona left JOIN cattipos as t5 ON t4.tipolicenciafkcattipos=t5.idcattipos where t1.empresa='" + empresa + "' and t1.area= '" + area + "'and t1.status='1'  ORDER BY t1.credencial ASC");
                if (busqEmpleados.DataSource != null) busqEmpleados.Columns[10].Visible = busqEmpleados.Columns[14].Visible = busqEmpleados.Columns[15].Visible = busqEmpleados.Columns[16].Visible = false;
            }
            if (busqEmpleados.DataSource != null) { busqEmpleados.Columns[0].Visible = busqEmpleados.Columns[7].Visible = busqEmpleados.Columns[8].Visible = false; busqEmpleados.ClearSelection(); }
            pActualizar.Visible = false;
        }
        void ocultarColumnas()
        {
            if (busqEmpleados.DataSource != null)
            {
                if (empresa == 1 && area == 1) busqEmpleados.Columns[14].Visible = busqEmpleados.Columns[15].Visible = busqEmpleados.Columns[16].Visible = busqEmpleados.Columns[17].Visible = busqEmpleados.Columns[18].Visible = busqEmpleados.Columns[19].Visible = false;
                else busqEmpleados.Columns[10].Visible = busqEmpleados.Columns[14].Visible = busqEmpleados.Columns[15].Visible = busqEmpleados.Columns[16].Visible = false;
            }
            busqEmpleados.Columns[0].Visible = busqEmpleados.Columns[7].Visible = busqEmpleados.Columns[8].Visible = false;
            busqEmpleados.ClearSelection();
        }
        void insertarVigencias(object idPersona)
        {
            string campos = "";
            string valores = "";
            if (empresa == 1 && area == 1)
            {
                campos += "fechaEmisionTarjeton,fechaVencimientoTarjeton";
                valores += "'" + dtpexptrajeton.Value.ToString("yyyy/MM/dd") + "','" + dtpvenctarjeton.Value.ToString("yyyy/MM/dd") + "'";
                campos += ",";
                valores += ",";
            }
            campos += "tipolicenciafkcattipos"; valores += "'" + cbtipo.SelectedValue + "'"; campos += ","; valores += ","; campos += "fechaEmisionConducir,fechaVencimientoConducir"; valores += "'" + dtpexpconducir.Value.ToString("yyyy/MM/dd") + "','" + dtpvencconducir.Value.ToString("yyyy/MM/dd") + "'";
            if (!string.IsNullOrWhiteSpace(campos) && Convert.ToInt32(cbtipo.SelectedIndex) > 0) v.c.insertar("INSERT INTO vigencias_supervision(usuariofkcpersonal," + campos + ",empresa,area) VALUES('" + idPersona + "'," + valores + ",'" + empresa + "','" + area + "')");

        }
        string Fecha_expL, Fecha_venL, Fecha_expT, Fecha_venT;
        private void insertar(string credencial, string ap, string am, string nombre, int puesto)
        {
            if (!v.camposvacioscPersonal(credencial, ap, am, nombre, puesto, empresa, area, Convert.ToInt32(cbtipo.SelectedValue), dtpexptrajeton.Value, dtpvenctarjeton.Value, dtpexpconducir.Value, dtpvencconducir.Value, Convert.ToInt32(cbaccess.SelectedIndex), editar, cbtipo.DataSource != null))
            {
                string usu = txtgetusu.Text.Trim();
                string pass1 = txtgetpass.Text.Trim();
                string pass2 = txtgetpass2.Text.Trim();
                if (cbtipo.SelectedIndex > 1)
                {
                    Fecha_expL = string.Format("{0:D}", dtpexpconducir.Value);
                    Fecha_venL = string.Format("{0:D}", dtpvencconducir.Value);
                    Fecha_expT = string.Format("{0:D}", dtpexptrajeton.Value);
                    Fecha_venT = string.Format("{0:D}", dtpvenctarjeton.Value);
                }
                int _idTIpo = Convert.ToInt32(cbtipo.SelectedValue);
                if (v.getAccesoSistemaInt((int)cbaccess.SelectedIndex))
                {
                    if (!v.yaExisteEmpleado(credencial, ap, am, nombre) && !v.formulariousu(usu, pass1, pass2) && !v.existeusupass(pass1))
                    {
                        if (MessageBox.Show("¿Esta seguro de que la información es correcta?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            string sql = "INSERT INTO cpersonal(credencial, ApPaterno, ApMaterno, nombres, cargofkcargos, empresa, idPersonalaltafkpersona, area) VALUES(LTRIM(RTRIM('" + credencial + "')),LTRIM(RTRIM('" + ap + "')),LTRIM(RTRIM('" + am + "')),LTRIM(RTRIM('" + nombre + "')),'" + puesto + "','" + empresa + "','" + idUsuario + "','" + area + "'); ";

                            if (v.c.insertar(sql))
                            {
                                if (v.c.insertar("INSERT INTO datosistema(usuariofkcpersonal, usuario, password) VALUES(LTRIM(RTRIM('" + v.idPersonaparaUsuario(credencial) + "')), LTRIM(RTRIM('" + usu + "')), '" + v.Encriptar(pass1) + "')"))
                                {
                                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal',(SELECT idpersona FROM cpersonal WHERE credencial = '" + credencial + "'),'" + credencial + ";" + ap + ";" + am + ";" + nombre + ";" + puesto + ";" + _idTIpo + ";" + Fecha_expL + ";" + Fecha_venL + ";" + usu + ";" + pass1 + "','" + idUsuario + "',NOW(),'Inserción de Empleado','" + empresa + "','" + area + "')");
                                    if (privilegios != null)
                                    {
                                        var res = privilegios.GetLength(0);
                                        var idUsuarioTemp = Convert.ToInt32(v.getaData("SELECT idpersona FROM cpersonal WHERE credencial = '" + credencial + "'"));
                                        for (int i = 0; i < privilegios.GetLength(0); i++)
                                        {
                                            string ver = privilegios[i, 0];
                                            string insertar = privilegios[i, 1];
                                            string consultar = privilegios[i, 2];
                                            string modificar = privilegios[i, 3];
                                            string eliminar = privilegios[i, 4];
                                            string nombref = privilegios[i, 5];
                                            v.insert(ver, insertar, consultar, modificar, eliminar, nombref, idUsuarioTemp);
                                        }
                                    }
                                    if (Convert.ToInt32(cbtipo.SelectedValue) > 0 && (DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) && (DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today && DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today))) insertarVigencias(v.getaData("SELECT idpersona FROM cpersonal WHERE credencial = '" + credencial + "'"));
                                    if (!string.IsNullOrWhiteSpace(template)) v.c.insertar("INSERT INTO huellasupervision (PersonafkCpersonal,template,calidad) VALUES ('" + v.idPersonaparaUsuario(credencial) + "','" + template + "','" + qualityTemplate + "')");
                                    limpiar();
                                    if (Pconsultar) busemp();
                                }
                            }
                            else MessageBox.Show("Ha Ocurrido Un Error", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    if (!v.yaExisteEmpleado(credencial, ap, am, nombre))
                    {
                        if (MessageBox.Show("¿Esta seguro de que la información es correcta?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            string sql = "INSERT INTO cpersonal({0} ApPaterno, ApMaterno, nombres, cargofkcargos, empresa, idPersonalaltafkpersona, area) VALUES({1}'" + ap + "','" + am + "','" + nombre + "','" + puesto + "','" + empresa + "','" + idUsuario + "','" + area + "')";
                            if (!csetpuestos.Text.Contains("BECARIO"))
                                sql = string.Format(sql, "credencial,", "'" + credencial + "',");
                            else
                                sql = string.Format(sql, "", "");
                            if (v.c.insertar(sql))
                            {
                                object idpersona = null;
                                if (!csetpuestos.Text.Contains("BECARIO"))
                                    idpersona = v.getaData("SELECT idpersona FROM cpersonal WHERE credencial = '" + credencial + "'");
                                else
                                    idpersona = v.getaData(string.Format("SELECT idpersona FROM cpersonal WHERE ApPaterno='{0}' AND ApMaterno='{1}' AND nombres='{2}'", ap, am, nombre));
                                if (Convert.ToInt32(cbtipo.SelectedValue) > 0 && (DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) && (DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today && DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today)))
                                    insertarVigencias(idpersona);
                                if (!string.IsNullOrWhiteSpace(template)) v.c.insertar("INSERT INTO huellasupervision (PersonafkCpersonal,template,calidad) VALUES ('" + v.idPersonaparaUsuario(credencial) + "','" + template + "','" + qualityTemplate + "')");
                                string sql1 = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal','" + idpersona + "','" + credencial + ";" + ap + ";" + am + ";" + nombre + ";" + puesto + ";" + _idTIpo + ";" + Fecha_expL + ";" + Fecha_venL + ";";
                                if (this.empresa == 1 && this.area == 1) sql1 += Fecha_expT + ";" + Fecha_venT + ";" + usu + ";" + pass1 + "','" + idUsuario + "',NOW(),'Inserción de Empleado','" + empresa + "','" + area + "')";
                                else sql1 += usu + ";" + pass1 + "','" + idUsuario + "',NOW(),'Inserción de Empleado','" + empresa + "','" + area + "')";
                                var res2 = v.c.insertar(sql1);
                                limpiar();
                                if (Pconsultar) busemp();
                            }
                            else MessageBox.Show("Ha Ocurrido Un Error", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void busqpersonal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (getcambios())
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button1_Click(null, e);
                }
                else guardarReporte(e);
            }
            else guardarReporte(e);
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                limpiar();
                statusTemp = v.getStatusInt(busqEmpleados.Rows[e.RowIndex].Cells[13].Value.ToString());
                idUsuarioTemp = Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (this.Pdesactivar)
                {
                    if (!this.idUsuario.Equals(Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[0].Value.ToString())))
                    {
                        if (statusTemp == 0)
                        {
                            btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                            lbldeleteuser.Text = "Reactivar";
                            reactivar = true;
                            pHD.Visible = false;
                        }
                        else
                        {
                            btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                            lbldeleteuser.Text = "Desactivar";
                            reactivar = false;
                            if (empresa == 1 && area == 1) pHD.Visible = true;
                        }
                        peliminarusu.Visible = true;
                    }
                    else
                        peliminarusu.Visible = !true;
                }
                if (Peditar)
                {
                    try
                    {
                        txtgetcredencial.Text = CredencialAnterior = busqEmpleados.Rows[e.RowIndex].Cells[1].Value.ToString();
                        txtgetap.Text = ApAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                        txtgetam.Text = AmAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                        txtgetnombre.Text = NombresAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[4].Value.ToString().ToLower());
                        csetpuestos.SelectedValue = tipoTemp = Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[7].Value.ToString());
                        acces_s = (busqEmpleados.Rows[e.RowIndex].Cells[9].Value.ToString() != "" ? 1 : 2);
                        if (csetpuestos.SelectedIndex == -1)
                        {
                            csetpuestos.SelectedIndex = 0;
                            csetpuestos.Focus();
                            v.iniCombos("SELECT idpuesto,UPPER(puesto) as puesto FROM puestos WHERE  empresa = " + empresa + " and area = '" + this.area + "' and (status= 1 OR idpuesto='" + tipoTemp + "') ORDER BY puesto ASC", csetpuestos, "idpuesto", "puesto", "-- SELECCIONE UN PUESTO --");
                            csetpuestos.SelectedValue = tipoTemp;
                        }
                        puestoAnterior = v.mayusculas(busqEmpleados.Rows[e.RowIndex].Cells[5].Value.ToString().ToLower());
                        txtgetusu.Text = UsuarioAnterior = busqEmpleados.Rows[e.RowIndex].Cells[9].Value.ToString();
                        if (string.IsNullOrWhiteSpace(UsuarioAnterior)) { cbaccess.SelectedIndex = 2; accesSistemaAnterior = false; }
                        else
                        {
                            cbaccess.SelectedIndex = 1;
                            mostrarUsuarioContrasena();
                            if (empresa == 1 && area == 1)
                                PasswordAnterior = busqEmpleados.Rows[e.RowIndex].Cells[14].Value.ToString();
                            else
                                PasswordAnterior = busqEmpleados.Rows[e.RowIndex].Cells[10].Value.ToString();
                            accesSistemaAnterior = true;
                            lblprivilegios.Text = (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal='" + idUsuarioTemp + "'")) > 0 ? "Actualizar Privilegios" : "Asignar Privilegios");
                        }
                        txtgetusu.Text = UsuarioAnterior = busqEmpleados.Rows[e.RowIndex].Cells[9].Value.ToString();
                        btnlimpiar.BackgroundImage = Properties.Resources.add;
                        lbllimpiar.Text = "Nuevo";
                        busqEmpleados.ClearSelection();
                        btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        lblguardar.Text = "Guardar";
                        gbemp.Text = "Actualizar Información de: " + NombresAnterior + " " + ApAnterior + " " + AmAnterior;
                        if (Pinsertar) pCancel.Visible = true;
                        gbemp.Enabled = true;
                        txtgetusu.Text = UsuarioAnterior;
                        if (Pinsertar && Pconsultar && Peditar && Pdesactivar) txtgetpass.Text = txtgetpass2.Text = v.Desencriptar(PasswordAnterior);
                        pprivilegios.Visible = (statusTemp == 1 && cbaccess.SelectedIndex == 1);
                        if (empresa == 1 && area == 1)
                        {
                            if (!string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[15].Value.ToString()) && !string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[16].Value.ToString()))
                            {
                                expedicionTarjetonAnterior = dtpexptrajeton.Value = DateTime.Parse(busqEmpleados.Rows[e.RowIndex].Cells[15].Value.ToString());
                                vencimientoTarjetonAnterior = dtpvenctarjeton.Value = DateTime.Parse(busqEmpleados.Rows[e.RowIndex].Cells[16].Value.ToString());
                            }
                        }
                        string n1 = "";
                        string n2 = "";
                        string tipoLicencia = "";
                        if (empresa == 1 && area == 1)
                        {
                            n1 = busqEmpleados.Rows[e.RowIndex].Cells[17].Value.ToString();
                            n2 = busqEmpleados.Rows[e.RowIndex].Cells[18].Value.ToString();
                            if (!string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[19].Value.ToString())) if (Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[19].Value) > 0)
                                    tipoLicencia = busqEmpleados.Rows[e.RowIndex].Cells[19].Value.ToString();
                        }
                        else
                        {
                            n1 = (!string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[14].Value.ToString()) ? busqEmpleados.Rows[e.RowIndex].Cells[14].Value.ToString() : DateTime.Today.ToString("yyy-MM-dd"));
                            if (!string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[15].Value.ToString())) n2 = busqEmpleados.Rows[e.RowIndex].Cells[15].Value.ToString(); else n2 = DateTime.Today.ToString("yyy-MM-dd");
                            if (!string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[16].Value.ToString())) if (Convert.ToInt32(busqEmpleados.Rows[e.RowIndex].Cells[16].Value) > 0)
                                    tipoLicencia = busqEmpleados.Rows[e.RowIndex].Cells[16].Value.ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(n1) && !string.IsNullOrWhiteSpace(n2))
                        {
                            expedicionlicenciaAnterior = dtpexpconducir.Value = DateTime.Parse(n1);
                            vencimientolicenciaAnterior = dtpvencconducir.Value = DateTime.Parse(n2);
                        }
                        if (!string.IsNullOrWhiteSpace(tipoLicencia))
                        {
                            if (Convert.ToInt32(tipoLicencia) > 0)
                            {
                                cbtipo.SelectedValue = tipolicenciaAnterior = tipoLicencia;
                                if (cbtipo.SelectedIndex == -1)
                                {
                                    mostrarLicenciasBusq();
                                    cbtipo.SelectedValue = TipolicenciaAnterior;
                                    if (!string.IsNullOrWhiteSpace(n1) && !string.IsNullOrWhiteSpace(n2))
                                    {
                                        expedicionlicenciaAnterior = dtpexpconducir.Value = DateTime.Parse(n1);
                                        vencimientolicenciaAnterior = dtpvencconducir.Value = DateTime.Parse(n2);
                                    }
                                    if (empresa == 1 && area == 1)
                                    {
                                        if (!string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[15].Value.ToString()) && !string.IsNullOrWhiteSpace(busqEmpleados.Rows[e.RowIndex].Cells[16].Value.ToString()))
                                        {
                                            expedicionTarjetonAnterior = dtpexptrajeton.Value = DateTime.Parse(busqEmpleados.Rows[e.RowIndex].Cells[15].Value.ToString());
                                            vencimientoTarjetonAnterior = dtpvenctarjeton.Value = DateTime.Parse(busqEmpleados.Rows[e.RowIndex].Cells[16].Value.ToString());
                                        }
                                    }
                                }
                            }
                        }
                        else cbtipo.SelectedValue = 0;
                        if (empresa == 1 && area == 1)
                        {
                            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM huellasupervision WHERE PersonafkCpersonal='" + idUsuarioTemp + "'")) > 0)
                            {
                                label17.Text = "Actualizar Huella";
                                string[] datosHuella = v.getaData("SELECT CONCAT(CONVERT(template using utf8),';',calidad) FROM huellasupervision WHERE PersonafkCpersonal='" + idUsuarioTemp + "'").ToString().Split(';');
                                templateAnterior = datosHuella[0];
                                qualityTemplateAnterior = Convert.ToInt32(datosHuella[1]);
                            }
                            else
                            {
                                label17.Text = "Registrar Huella";
                                templateAnterior = null;
                                qualityTemplateAnterior = 0;
                            }
                        }
                        if (Pinsertar) pCancel.Visible = true;
                        btnguardar.Visible = false;
                        lblguardar.Visible = false; editar = true;
                        if (statusTemp == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else
                    MessageBox.Show("Usted No Tiene Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lblbuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbredencial.Text.Trim()) || !string.IsNullOrWhiteSpace(txtbap.Text.Trim()) || csetbpuestos.SelectedIndex > 0 || cbstatus.SelectedIndex > 0)
            {
                busqEmpleados.DataSource = null;
                string sql = "";
                if (empresa == 1 && area == 1) sql = "Set names 'utf8';SET lc_time_names='es_ES';SELECT t1.idPersona as id, t1.credencial as 'CREDENCIAL', UPPER(t1.ApPaterno) as 'APELLIDO PATERNO', UPPER(t1.ApMaterno) as 'APELLIDO MATERNO', UPPER(t1.nombres) as 'NOMBRES', UPPER(t2.puesto) as 'PUESTO', (SELECT UPPER(CONCAT(nombres, ' ', apPaterno, ' ', ApMaterno)) from cpersonal WHERE idPersona = t1.idPersonalaltafkpersona) as 'PERSONA QUE DIÓ DE ALTA',t1.cargofkcargos as cargo, t1.area,COALESCE(t3.usuario, '') as 'USUARIO',coalesce(upper(CONCAT('Expedición: \n', DATE_FORMAT(t4.fechaEmisionTarjeton,   '%d/ %m/%Y'), '\n Vencimiento: \n', DATE_FORMAT(t4.fechaVencimientoTarjeton,  '%d/ %m/%Y'))), '') AS 'TARJETÓN',COALESCE(CONCAT(T5.Tipo,' - ',t5.Descripcion),'') AS 'TIPO DE LICENCIA' ,coalesce(UPPER(CONCAT('Expedición: \n',  DATE_FORMAT(t4.fechaEmisionConducir, '%d/%m/%Y'), '\n Vencimiento: \n', DATE_FORMAT(t4.fechaVencimientoConducir, '%d/%m/%Y'))), '') AS 'LICENCIA DE CONDUCIR', if (t1.status = 1,'ACTIVO',CONCAT('NO ACTIVO')) as 'ESTATUS',COALESCE(t3.password, '') as pass,COALESCE(t4.fechaEmisionTarjeton,''), COALESCE(t4.fechaVencimientoTarjeton,''),COALESCE(t4.fechaEmisionConducir,''), COALESCE(t4.fechaVencimientoConducir,''),COALESCE(t5.idcattipos,'') AS 'idtipo'{0} FROM cpersonal as t1 INNER JOIN  puestos as t2 ON t1.cargofkcargos = t2.idpuesto LEFT JOIN datosistema as t3 ON t3.usuariofkcpersonal = t1.idPersona LEFT JOIN vigencias_supervision as t4 ON t4.usuariofkcpersonal = t1.idPersona left JOIN cattipos as t5 ON t4.tipolicenciafkcattipos=t5.idcattipos WHERE t1.empresa = '1' and t1.area = '1' ";
                else
                    sql = "Set names 'utf8';SET lc_time_names='es_ES';SELECT t1.idPersona as id, t1.credencial as 'CREDENCIAL', UPPER(t1.ApPaterno) as 'APELLIDO PATERNO', UPPER(t1.ApMaterno) as 'APELLIDO MATERNO', UPPER(t1.nombres) as 'NOMBRES', UPPER(t2.puesto) as 'PUESTO',(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',ApMaterno)) from cpersonal WHERE idPersona=t1.idPersonalaltafkpersona)as 'PERSONA QUE DIÓ DE ALTA',t1.cargofkcargos as cargo, t1.area,COALESCE(t3.usuario,'') as 'USUARIO',COALESCE(t3.password,'') as pass,COALESCE(CONCAT(T5.Tipo,' - ',t5.Descripcion),'') AS 'TIPO DE LICENCIA' ,coalesce(UPPER(CONCAT('Expedición: \n',  DATE_FORMAT(t4.fechaEmisionConducir,  '%d/ %m/%Y'), '\n Vencimiento: \n', DATE_FORMAT(t4.fechaVencimientoConducir, '%d/%m/%Y'))), '') AS 'LICENCIA DE CONDUCIR', if(t1.status=1,'ACTIVO',CONCAT('NO ACTIVO')) as 'ESTATUS',COALESCE(t4.fechaEmisionConducir,''), COALESCE(t4.fechaVencimientoConducir,''),COALESCE(t5.idcattipos,'') AS 'idtipo'{0} FROM cpersonal as t1 INNER JOIN  puestos as t2 ON t1.cargofkcargos = t2.idpuesto LEFT JOIN datosistema as t3 ON t3.usuariofkcpersonal = t1.idPersona LEFT JOIN vigencias_supervision as t4 ON t4.usuariofkcpersonal = t1.idPersona left JOIN cattipos as t5 ON t4.tipolicenciafkcattipos=t5.idcattipos WHERE t1.empresa='" + empresa + "' and t1.area= '" + area + "' ";
                string wheres = "";
                if (!string.IsNullOrWhiteSpace(txtbredencial.Text.ToString()))
                {
                    if (wheres == "")

                        wheres += "AND (t1.credencial = '" + txtbredencial.Text + "'";
                    else
                        wheres += "AND t1.credencial = '" + txtbredencial.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtbap.Text.ToString()))
                {
                    if (wheres == "")
                        wheres += "AND (t1.apPaterno LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%'";
                    else
                        wheres += "AND t1.apPaterno LIKE '" + v.mayusculas(txtbap.Text.ToLower()) + "%'";
                }
                if (csetbpuestos.SelectedIndex > 0)
                {
                    if (wheres == "")
                        wheres += "AND ( cargofkcargos = '" + csetbpuestos.SelectedValue + "'";
                    else
                        wheres += "AND cargofkcargos = '" + csetbpuestos.SelectedValue + "'";
                }
                bool res = false;
                if (cbstatus.SelectedIndex > 0)
                {
                    if (wheres == "")
                    {
                        wheres += "AND ( t1.status = '" + v.statusinv(cbstatus.SelectedIndex) + "'";

                        if (cbstatus.SelectedIndex == 2)
                        {
                            sql = string.Format(sql, ",IF(t1.status='0',(SELECT UPPER(DATE_FORMAT(fechahora,'%W, %d de %M del %Y')) FROM modificaciones_sistema WHERE idregistro=t1.idpersona AND Tipo='Desactivación de Empleado' LIMIT 1),'') as 'FECHA DE BAJA' ");
                            res = true;
                        }
                        else
                            sql = string.Format(sql, "");
                    }
                    else
                    {
                        wheres += "AND t1.status = '" + v.statusinv(cbstatus.SelectedIndex - 1) + "'";
                        if (cbstatus.SelectedIndex == 2)
                        {
                            sql = string.Format(sql, ",IF(t1.status='0',(SELECT UPPER(DATE_FORMAT(fechahora,'%W, %d de %M del %Y')) FROM modificaciones_sistema WHERE idregistro=t1.idpersona AND Tipo='Desactivación de Empleado' LIMIT 1),'') as 'FECHA DE BAJA' ");
                            res = true;
                        }
                        else
                            sql = string.Format(sql, "");
                    }
                }
                else
                    sql = string.Format(sql, "");
                if (!string.IsNullOrWhiteSpace(wheres))
                    sql += wheres + ") ORDER BY t1.credencial ASC";
                txtbredencial.Clear();
                txtbap.Clear();
                csetbpuestos.SelectedIndex = 0;
                busqEmpleados.Rows.Clear();
                cbstatus.SelectedIndex = 0;
                DataTable dt = (DataTable)v.getData(sql);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    pActualizar.Visible = false;
                    busemp();
                }
                else
                {
                    busqEmpleados.DataSource = dt;
                    busqEmpleados.ClearSelection();
                    pActualizar.Visible = true;
                    ocultarColumnas();
                    if (res)
                    {
                        if (empresa == 1 && area == 1)
                            busqEmpleados.Columns[9].Visible = busqEmpleados.Columns[10].Visible = busqEmpleados.Columns[11].Visible = busqEmpleados.Columns[12].Visible = false;
                        else if (empresa == 2 && (area == 1 || area == 2))
                            busqEmpleados.Columns[9].Visible = busqEmpleados.Columns[11].Visible = busqEmpleados.Columns[12].Visible = false;
                    }
                }
            }
            else
                MessageBox.Show("Seleccione un Criterio de Búsqueda", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            string msg;
            int status;
            if (reactivar)
            {
                if (v.yaExisteCredencialReactivar(CredencialAnterior))
                {
                    recibirCredencial r = new recibirCredencial(CredencialAnterior, idUsuarioTemp, idUsuario, empresa, area, v);
                    r.ShowDialog();
                }
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
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Empleado";
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                try
                {
                    String sql = "UPDATE cpersonal SET status = " + status + " WHERE idPersona  = " + this.idUsuarioTemp;
                    if (v.c.insertar(sql))
                    {
                        if (msg == "Des") v.EliminarPrivilegios(idUsuarioTemp);
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Personal','" + idUsuarioTemp + "','" + msg + "activación de Empleado','" + idUsuario + "',NOW(),'" + msg + "activación de Empleado','" + edicion + "','" + empresa + "','" + area + "')");
                        MessageBox.Show("El empleado ha sido " + msg + "activado Existosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        if (Pconsultar)
                            busemp();
                    }
                    else
                        MessageBox.Show("**El empleado no ha sido " + msg);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
        private void button2_Click_2(object sender, EventArgs e)
        {
            if (editar)
            {
                if (this.idUsuarioTemp != idUsuario)
                {
                    if (empresa == 1)
                    {
                        privilegiosSupervision ps = new privilegiosSupervision(v, idUsuarioTemp);
                        ps.Owner = this;
                        string nombre = v.mayusculas((txtgetnombre.Text.Trim() + " " + txtgetap.Text.Trim() + " " + txtgetam.Text.Trim()).ToLower());
                        ps.lbltitle.Text = "Nombre del Empleado: " + nombre;
                        if (privilegios != null) ps.insertarPrivilegios(privilegios);
                        ps.ShowDialog();
                    }
                    else
                    {
                        if (area == 1)
                        {
                            privilegiosMantenimiento ps = new privilegiosMantenimiento(idUsuarioTemp, v);
                            string nombre = v.mayusculas((txtgetnombre.Text.Trim() + " " + txtgetap.Text.Trim() + " " + txtgetam.Text.Trim()).ToLower());
                            ps.lbltitle.Text = "Nombre del Empleado: " + nombre;
                            ps.Owner = this;
                            if (privilegios != null) ps.insertarPrivilegios(privilegios);
                            ps.ShowDialog();
                        }
                        else
                        {
                            privilegiosAlmacen ps = new privilegiosAlmacen(idUsuarioTemp,v);
                            string nombre = v.mayusculas((txtgetnombre.Text.Trim() + " " + txtgetap.Text.Trim() + " " + txtgetam.Text.Trim()).ToLower());
                            ps.lbltitle.Text = "Nombre del Empleado: " + nombre;
                            ps.Owner = this;
                            if (privilegios != null) ps.insertarPrivilegios(privilegios);
                            ps.ShowDialog();
                        }
                    }
                    lblguardar.Visible = btnguardar.Visible = getcambios();
                }
                else
                    MessageBox.Show("Sólo otro Usuario con los mismos privilegios puede cambiar sus privilegios", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (empresa == 1)
                {
                    privilegiosSupervision ps = new privilegiosSupervision(v);
                    ps.Owner = this;
                    string nombre = v.mayusculas((txtgetnombre.Text.Trim() + " " + txtgetap.Text.Trim() + " " + txtgetam.Text.Trim()).ToLower());
                    ps.lbltitle.Text = "Nombre del Empleado: " + nombre;
                    if (privilegios != null) ps.insertarPrivilegios(privilegios);
                    ps.ShowDialog();
                }
                else
                {
                    if (area == 1)
                    {
                        privilegiosMantenimiento ps = new privilegiosMantenimiento(idUsuarioTemp, v);
                        string nombre = v.mayusculas((txtgetnombre.Text.Trim() + " " + txtgetap.Text.Trim() + " " + txtgetam.Text.Trim()).ToLower());
                        ps.lbltitle.Text = "Nombre del Empleado: " + nombre;
                        ps.Owner = this;
                        if (privilegios != null) ps.insertarPrivilegios(privilegios);
                        ps.ShowDialog();
                    }
                    else
                    {
                        privilegiosAlmacen ps = new privilegiosAlmacen(idUsuarioTemp,v);
                        string nombre = v.mayusculas((txtgetnombre.Text.Trim() + " " + txtgetap.Text.Trim() + " " + txtgetam.Text.Trim()).ToLower());
                        ps.lbltitle.Text = "Nombre del Empleado: " + nombre;
                        ps.Owner = this;
                        if (privilegios != null) ps.insertarPrivilegios(privilegios);
                        ps.ShowDialog();
                    }
                }
            }
        }

        public string[,] privilegios = null;
        public void privilegiosPersonal()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {
                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
            }
            mostrarPersonal();
            ppuestos.Visible = Convert.ToInt32(v.getaData("SELECT ver FROM privilegios WHERE usuariofkcpersonal = '" + idUsuario + "' and namform = 'catPuestos'")) == 1;
        }
        public void mostrarLicencias()
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cattipos WHERE status=1 AND empresa='" + empresa + "' AND area='" + area + "'")) > 0)
            {
                cbtipo.DataSource = null;
                DataTable dt = (DataTable)v.getData("SELECT idcattipos, if(descripcion='',tipo,CONCAT(Tipo,' - ',Descripcion)) as nombre FROM cattipos WHERE status=1 AND empresa='" + empresa + "' AND area='" + area + "'");
                DataRow nuevaFila = dt.NewRow();
                nuevaFila["idcattipos"] = -1;
                nuevaFila["nombre"] = "-- SELECCIONE UN TIPO DE LICENCIA --".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                nuevaFila = dt.NewRow();
                nuevaFila["idcattipos"] = 0;
                nuevaFila["nombre"] = "-- RESTABLECER FECHAS --".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 1);
                cbtipo.ValueMember = "idcattipos";
                cbtipo.DisplayMember = "nombre";
                cbtipo.DataSource = dt;
            }
            else
                cbtipo.Enabled = false;
        }
        public void mostrarLicenciasBusq()
        {
            cbtipo.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idcattipos, if(descripcion='',tipo,CONCAT(Tipo,' - ',Descripcion)) as nombre FROM cattipos WHERE (status=1 OR idcattipos='" + tipolicenciaAnterior + "') AND empresa='" + empresa + "' AND area='" + area + "'");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idcattipos"] = -1;
            nuevaFila["nombre"] = "-- SELECCIONE UN TIPO DE LICENCIA --".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            nuevaFila = dt.NewRow();
            nuevaFila["idcattipos"] = 0;
            nuevaFila["nombre"] = "-- RESTABLECER FECHAS --".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 1);
            cbtipo.ValueMember = "idcattipos";
            cbtipo.DisplayMember = "nombre";
            cbtipo.DataSource = dt;
        }
        void mostrarPersonal()
        {
            if (Pconsultar)
                gbbuscar.Visible = busqEmpleados.Visible = true;
            if (Pinsertar || Peditar)
            {
                gbemp.Visible = true;
                mostrarLicencias();
            }
            if (Peditar) label23.Visible = label22.Visible = true;
            if (Peditar && !Pinsertar)
            {
                btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblguardar.Text = "Editar Empleado";
                gbemp.Enabled = false;
            }
            if (Pconsultar && Peditar && Pinsertar && Pdesactivar) txtgetpass2.UseSystemPasswordChar = txtgetpass.UseSystemPasswordChar = false;
        }
        private void gbemp_Enter(object sender, EventArgs e) { }
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        private void csetpuestos_DrawItem(object sender, DrawItemEventArgs e) { v.combos_DrawItem(sender, e); }
        private void busqEmpleados_ColumnAdded(object sender, DataGridViewColumnEventArgs e) { v.paraDataGridViews_ColumnAdded(sender, e); }
        private void busqEmpleados_Leave(object sender, EventArgs e) { busqEmpleados.ClearSelection(); }
        private void txtgetap_Validating(object sender, CancelEventArgs e) { v.espaciosenblanco(sender, e); }
        public void BuscarRefaccion(string id)
        {
            for (int i = busqEmpleados.Rows.Count - 1; i >= 0; i--)
            {
                if (busqEmpleados.Rows[i].Cells[0].Value.ToString().Equals(id))
                {
                    busqEmpleados.Rows[i].Selected = true;
                    busqEmpleados.FirstDisplayedScrollingRowIndex = i;
                }
            }
        }
        bool getcambios()
        {
            bool res = false;
            if (!string.IsNullOrWhiteSpace(txtgetcredencial.Text.Trim()))
            {
                if (Convert.ToInt32(txtgetcredencial.Text.Trim()) > 0)
                    res = true;
                else
                    res = false;
            }
            else
            {
                if (csetpuestos.DataSource != null)
                {
                    if (csetpuestos.SelectedIndex > 0)
                    {
                        try
                        {
                            if (v.getaData(string.Format("SELECT puesto FROM puestos WHERE idpuesto='{0}'", csetpuestos.SelectedValue)).ToString().Contains("Becario"))
                                res = true;
                            else
                                res = false;
                        }
                        catch { }
                    }
                }
            }
            bool acces = accesSistemaAnterior;
            bool access = v.getBoolFromInt(Convert.ToInt32(cbaccess.SelectedValue));
            if (editar)
            {

                if (statusTemp == 1)
                {
                    if (empresa == 1 && area == 1)
                    {
                        if (res && (!string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text) && csetpuestos.SelectedIndex > 0 && cbaccess.SelectedIndex > 0 && cbtipo.SelectedIndex > 1 && DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) < DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) && DateTime.Parse(dtpexptrajeton.Value.ToString("yyyy-MM-dd")) < DateTime.Parse(dtpvenctarjeton.Value.ToString("yyyy-MM-dd"))))
                        {
                            pHD.Visible = true;
                            if ((!CredencialAnterior.Equals(txtgetcredencial.Text) || !ApAnterior.Equals(v.mayusculas(txtgetap.Text.ToLower()).Trim()) || !AmAnterior.Equals(v.mayusculas(txtgetam.Text.ToLower()).Trim()) || !NombresAnterior.Equals(v.mayusculas(txtgetnombre.Text.ToLower()).Trim()) || tipoTemp != (int)csetpuestos.SelectedValue || access != accesSistemaAnterior || dtpexpconducir.Value != expedicionlicenciaAnterior || vencimientolicenciaAnterior != dtpvencconducir.Value || expedicionTarjetonAnterior != dtpexptrajeton.Value || vencimientoTarjetonAnterior != dtpvenctarjeton.Value || !tipolicenciaAnterior.Equals(cbtipo.SelectedValue.ToString())) || privilegios != null || !string.IsNullOrWhiteSpace(template))
                            {
                                if (access)
                                {
                                    if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) pprivilegios.Visible = true; else pprivilegios.Visible = false;
                                    if ((!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) && (!v.Desencriptar(txtgetpass.Text.Trim()).Equals(v.Desencriptar(PasswordAnterior)) || !UsuarioAnterior.Equals(txtgetusu.Text.Trim())))
                                        return true;
                                    else
                                        return true;
                                }
                                else
                                {
                                    pprivilegios.Visible = false;
                                    return true;
                                }
                            }
                            else
                            {
                                if (access)
                                {
                                    if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) pprivilegios.Visible = true; else pprivilegios.Visible = false;
                                    if ((!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) && (!v.Desencriptar(txtgetpass.Text.Trim()).Equals(v.Desencriptar(PasswordAnterior)) || !UsuarioAnterior.Equals(txtgetusu.Text.Trim())))
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                {
                                    pprivilegios.Visible = false;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            pHD.Visible = false;
                            return false;
                        }
                    }
                    else
                    {
                        if (res && (!string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text) && csetpuestos.SelectedIndex > 0 && cbaccess.SelectedIndex > 0 || (cbtipo.SelectedIndex > 1 || DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) < DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")))))
                        {
                            int tipo = 0; if (cbtipo.DataSource != null) if (cbtipo.SelectedIndex > 1) tipo = Convert.ToInt32(cbtipo.SelectedValue);
                            int tipoAnt = 0; if (!string.IsNullOrWhiteSpace(TipolicenciaAnterior)) tipoAnt = Convert.ToInt32(TipolicenciaAnterior);
                            if ((!CredencialAnterior.Equals(txtgetcredencial.Text) || !ApAnterior.Equals(v.mayusculas(txtgetap.Text.ToLower()).Trim()) || !AmAnterior.Equals(v.mayusculas(txtgetam.Text.ToLower()).Trim()) || !NombresAnterior.Equals(v.mayusculas(txtgetnombre.Text.ToLower()).Trim()) || tipoTemp != Convert.ToInt32(csetpuestos.SelectedValue) || (tipo != Convert.ToInt32(tipoAnt)) || cbaccess.SelectedIndex != acces_s || dtpexpconducir.Value != expedicionlicenciaAnterior || vencimientolicenciaAnterior != dtpvencconducir.Value || privilegios != null))
                            {
                                if (access)
                                {
                                    if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) pprivilegios.Visible = true; else pprivilegios.Visible = false;
                                    if ((!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) && (!v.Desencriptar(txtgetpass.Text.Trim()).Equals(v.Desencriptar(PasswordAnterior)) || !UsuarioAnterior.Equals(txtgetusu.Text.Trim())))
                                    {
                                        pprivilegios.Visible = true;
                                        return true;
                                    }
                                    else
                                    {
                                        pprivilegios.Visible = false;
                                        return true;
                                    }
                                }
                                else
                                {
                                    pprivilegios.Visible = false;
                                    return true;
                                }
                            }
                            else
                            {
                                if (access)
                                {
                                    if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) pprivilegios.Visible = true; else pprivilegios.Visible = false;
                                    if ((!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) && (!v.Desencriptar(txtgetpass.Text.Trim()).Equals(v.Desencriptar(PasswordAnterior)) || !UsuarioAnterior.Equals(txtgetusu.Text.Trim())))
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                {
                                    pprivilegios.Visible = false;
                                    return false;
                                }
                            }
                        }
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
            else
            {
                if (empresa == 1 && area == 1)
                {
                    if (res && (!string.IsNullOrWhiteSpace(txtgetap.Text) || !string.IsNullOrWhiteSpace(txtgetam.Text) || !string.IsNullOrWhiteSpace(txtgetnombre.Text) || csetpuestos.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtgetusu.Text) || !string.IsNullOrWhiteSpace(txtgetpass.Text) || !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim()) || cbtipo.SelectedIndex > 1 || DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today || DateTime.Parse(dtpexptrajeton.Value.ToString("yyyy-MM-dd")) != DateTime.Today || DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today || DateTime.Parse(dtpvenctarjeton.Value.ToString("yyyy-MM-dd")) != DateTime.Today || privilegios != null))
                    {
                        if (res && !string.IsNullOrWhiteSpace(txtgetap.Text) && !string.IsNullOrWhiteSpace(txtgetam.Text) && !string.IsNullOrWhiteSpace(txtgetnombre.Text) && csetpuestos.SelectedIndex > 0 && cbaccess.SelectedIndex > 0)
                            pHD.Visible = true;
                        else
                            pHD.Visible = false;
                        if (access)
                        {
                            if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) pprivilegios.Visible = true; else pprivilegios.Visible = false;
                            if ((!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) && (!v.Desencriptar(txtgetpass.Text.Trim()).Equals(v.Desencriptar(PasswordAnterior)) || !UsuarioAnterior.Equals(txtgetusu.Text.Trim())))

                                return true;
                            else
                                return true;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        pHD.Visible = false;
                        return (res || (!string.IsNullOrWhiteSpace(txtgetap.Text) || !string.IsNullOrWhiteSpace(txtgetam.Text) || !string.IsNullOrWhiteSpace(txtgetnombre.Text) || csetpuestos.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtgetusu.Text) || !string.IsNullOrWhiteSpace(txtgetpass.Text) || !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim()) || cbtipo.SelectedIndex > 1 || DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today || DateTime.Parse(dtpexptrajeton.Value.ToString("yyyy-MM-dd")) != DateTime.Today || DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today || DateTime.Parse(dtpvenctarjeton.Value.ToString("yyyy-MM-dd")) != DateTime.Today || privilegios != null));
                    }
                }
                else
                {
                    if (res && !string.IsNullOrWhiteSpace(txtgetap.Text) || !string.IsNullOrWhiteSpace(txtgetam.Text) || !string.IsNullOrWhiteSpace(txtgetnombre.Text) || csetpuestos.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtgetusu.Text) || !string.IsNullOrWhiteSpace(txtgetpass.Text) || !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim()) || cbtipo.SelectedIndex > 1 || DateTime.Parse(dtpexpconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today || DateTime.Parse(dtpvencconducir.Value.ToString("yyyy-MM-dd")) != DateTime.Today || privilegios != null)
                    {
                        if (access)
                        {
                            if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) pprivilegios.Visible = true; else pprivilegios.Visible = false;
                            if ((!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text) && !string.IsNullOrWhiteSpace(txtgetpass2.Text.Trim())) && (!v.Desencriptar(txtgetpass.Text.Trim()).Equals(v.Desencriptar(PasswordAnterior)) || !UsuarioAnterior.Equals(txtgetusu.Text.Trim())))
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
        }
        private void txtgetcredencial_TextChanged(object sender, EventArgs e)
        {
            if (editar)
            {
                if (getcambios())
                    lblguardar.Visible = btnguardar.Visible = true;
                else
                    lblguardar.Visible = btnguardar.Visible = false;
            }
            else
                if (getcambios()) pCancel.Visible = true; else pCancel.Visible = false;

        }
        private void button1_Click_3(object sender, EventArgs e)
        {
            CatTipos cat = new CatTipos(idUsuario, empresa, area, v);
            cat.Owner = this;
            cat.ShowDialog();
        }
        private void gbemp_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            busemp();
            txtbredencial.Clear();
            txtbap.Clear();
            csetbpuestos.SelectedIndex = 0;
            cbstatus.SelectedIndex = 0;
            pActualizar.Visible = false;
        }
        public bool Pconsultar { set; get; }
        private void gblicencia_Enter(object sender, EventArgs e) { }
        private void dtpexpconducir_KeyDown(object sender, KeyEventArgs e) { e.SuppressKeyPress = true; }
        private void gblicencia_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(200, 200, 200), this);
        }
        delegate void El_Delegado();
        void cargando()
        {
            btnExcel.Visible = false;
            pictureBox2.Image = Properties.Resources.loader;
            LblExcel.Text = "Exportando";
        }
        delegate void El_Delegado1();
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "Exportar";
        }
        void _PersonalExportadas()
        {
            string id;
            int contador = 0;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Personal','0','";
            foreach (DataGridViewRow row in busqEmpleados.Rows)
            {
                contador++;
                id = row.Cells[0].Value.ToString();
                if (contador < busqEmpleados.RowCount)
                    id += ";";
                sql += id;
            }
            sql += "','" + this.idUsuario + "',NOW(),'Exportación a Excel de Registro de Personal','" + this.empresa + "','" + this.area + "')";
            MySqlCommand exportacion = new MySqlCommand(sql, v.c.dbconection());
            exportacion.ExecuteNonQuery();
        }
        void ExportarExcel()
        {
            if (busqEmpleados.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < busqEmpleados.Columns.Count; i++)
                {
                    if (busqEmpleados.Columns[i].Visible)
                        dt.Columns.Add(busqEmpleados.Columns[i].HeaderText);
                }
                for (int j = 0; j < busqEmpleados.Rows.Count; j++)
                {
                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < busqEmpleados.Columns.Count; i++)
                    {
                        if (busqEmpleados.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = busqEmpleados.Rows[j].Cells[i].Value.ToString().Replace("\n", " ");
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
                try
                {
                    if (this.InvokeRequired)
                    {
                        El_Delegado1 delega = new El_Delegado1(cargando1);
                        this.Invoke(delega);
                    }
                }
                catch { }
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

 
         
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        }
        private void cbtipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbtipo.SelectedIndex == 0 || cbtipo.SelectedIndex == 1)
            {
                dtpexpconducir.Value = dtpvencconducir.Value = DateTime.Today; if (empresa == 1 && area == 1) dtpexptrajeton.Value = dtpvenctarjeton.Value = DateTime.Today;
                if (cbtipo.SelectedIndex == 1)
                    cbtipo.SelectedIndex = 0;
            }
        }
        private void LblExcel_Click(object sender, EventArgs e) { }

        private void button5_Click(object sender, EventArgs e)
        {
            writeFingerprint wf = new writeFingerprint(v);
            wf.Owner = this;
            var res = wf.ShowDialog();
            if (res == DialogResult.OK)
            {
                template = Convert.ToBase64String(wf._template.Buffer);
                qualityTemplate = wf._template.Quality;
                txtgetcredencial_TextChanged(null, e);
            }
        }

        private void csetpuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (csetpuestos.DataSource != null && cbaccess.DataSource != null)
            {
                if (empresa == 1 && area == 1)
                {
                    if (((ComboBox)sender).Text.Contains("Conductor".ToUpper()) || ((ComboBox)sender).Text.Contains("Operador".ToUpper()))
                    {
                        cbaccess.SelectedIndex = 2;
                        cbaccess.Enabled = false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(UsuarioAnterior))
                        {
                            cbaccess.SelectedIndex = 1;
                            pprivilegios.Visible = true;
                        }
                        else
                        {
                            cbaccess.SelectedIndex = 0;
                            pprivilegios.Visible = false;
                        }
                        cbaccess.Enabled = true;
                    }
                }
            }
            privilegios = null;
        }

        private void txtbap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) lblbuscar_Click(null, e);
            else
                v.Sololetras(e);
        }

        private void txtbredencial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) lblbuscar_Click(null, e);
            else
                v.Solonumeros(e);
        }
        public bool Pdesactivar { set; get; }
        public string CredencialAnterior { get { return credencialAnterior; } set { credencialAnterior = value; } }
        public string ApAnterior { get { return apAnterior; } set { apAnterior = value; } }
        public string AmAnterior { get { return amAnterior; } set { amAnterior = value; } }
        public string NombresAnterior { get { return nombresAnterior; } set { nombresAnterior = value; } }
        public string UsuarioAnterior { get { return usuarioAnterior; } set { usuarioAnterior = value; } }
        public string PasswordAnterior { get { return passwordAnterior; } set { passwordAnterior = value; } }
        public string TipolicenciaAnterior { get { return tipolicenciaAnterior; } set { tipolicenciaAnterior = value; } }
        void quitarUsuarioContrasena() { pprivilegios.Visible = lblusu.Visible = lblgusu.Visible = txtgetusu.Visible = txtgetpass.Visible = lblpass.Visible = lblgpass.Visible = lblgetpass2.Visible = txtgetpass2.Visible = lblgpass2.Visible = false; txtgetusu.Clear(); txtgetpass.Clear(); txtgetpass2.Clear(); }
        void mostrarUsuarioContrasena() { lblusu.Visible = lblgusu.Visible = txtgetusu.Visible = txtgetpass.Visible = lblpass.Visible = lblgpass.Visible = lblgetpass2.Visible = txtgetpass2.Visible = lblgpass2.Visible = true; }
        private void busqEmpleados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (busqEmpleados.Columns[e.ColumnIndex].Name == "ESTATUS")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }
        void iniacceso()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Nombre");
            DataRow row = dt.NewRow();
            row["id"] = 0;
            row["Nombre"] = "--Seleccione una Opcion--".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 1;
            row["Nombre"] = "Si".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["id"] = 2;
            row["Nombre"] = "No".ToUpper();
            dt.Rows.Add(row);
            cbaccess.ValueMember = "id";
            cbaccess.DisplayMember = "Nombre";
            cbaccess.DataSource = dt;
        }
        private void cbaccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (Convert.ToInt32(cbaccess.SelectedValue) == 1)
                    mostrarUsuarioContrasena();
                else
                    quitarUsuarioContrasena();
            }
            else
            {
                if (Convert.ToInt32(cbaccess.SelectedValue) == 1)
                {
                    if (accesSistemaAnterior)
                    {
                        mostrarUsuarioContrasena();
                        txtgetusu.Text = UsuarioAnterior;
                        txtgetpass2.Text = txtgetpass.Text = v.Desencriptar(PasswordAnterior);
                    }
                    else
                        mostrarUsuarioContrasena();
                }
                else
                {
                    if (idUsuarioTemp != idUsuario)
                        quitarUsuarioContrasena();
                    else
                    {
                        MessageBox.Show("Solamnete otro Usuario Puede Excluirlo del Sistema", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cbaccess.SelectedIndex = 1;
                        csetpuestos.SelectedValue = tipoTemp;
                    }
                }
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (editar)
            {
                if (getcambios())
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        button1_Click(null, e);
                        limpiar();
                        if (Pconsultar)
                            busemp();
                    }
                    else
                    {
                        limpiar();
                        if (Pconsultar)
                            busemp();
                    }
                }
                else
                {
                    limpiar();
                    if (Pconsultar)
                        busemp();
                }
            }
            else
            {
                limpiar();
                if (Pconsultar)
                    busemp();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            catPuestos cat = new catPuestos(idUsuario, empresa, area, v);
            cat.Owner = this;
            cat.ShowDialog();
        }
    }
}