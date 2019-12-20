using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using h = Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Reflection;

namespace controlFallos
{

    public partial class Supervisión : Form
    {
        int empresa, area, idUsuario; public Thread hilo;
        static bool res = true;
        validaciones v;
        DataTable dt = new DataTable();
        delegate void El_Delegado(); delegate void El_Delegado1();
        Thread th, exportar;
        int unidadAnterior, supervisorAnterior, conductorAnterior, servicioAnterior, TipoFalloAnterior, grupoFalloAnterior, subGrupoAnterior, categoriaAnterior, codigoAnterior, IdRepor, idconductor, idsupervisor;
        string fechaAnterior, kilometrajeAnterior, FalloAnterior, ObservacionesAnterior, consulta_gral = "SET lc_time_names = 'es_ES'; select t1.Folio AS 'FOLIO',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'ECO',(select UPPER(Date_format(t1.FechaReporte,'%W %d de %M del %Y'))) AS 'FECHA DEL REPORTE',(select UPPER(concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres))from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)as 'PERSONA QUE INSERTÓ',coalesce((SELECT x2.Credencial FROM cpersonal AS x2 WHERE  x2.idpersona=t1.CredencialConductorfkCPersonal),'')as 'CREDENCIAL DE CONDUCTOR',(select if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios)))as 'SERVICIO', TIME_FORMAT(t1.HoraEntrada,'%r') as 'HORA DEL REPORTE', t1.KmEntrada as 'KILOMETRAJE DE REPORTE',if(tipofallo='1','CORRECTIVO',(if(tipofallo='2','PREVENTIVO',(if(tipofallo='3','REITERATIVO',(if(tipofallo='4','REPROGRAMADO','SEGUIMIENTO'))))))) as 'TIPO DE FALLO',COALESCE((select UPPER(x3.descfallo) from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo),'')as 'SUBGRUPO DE FALLO',COALESCE((select UPPER(x4.codfallo) from cfallosesp as x4 where x4.idfalloEsp=t1.CodFallofkcfallosesp),'')as 'CÓDIGO DE FALLO',UPPER(t1.DescFalloNoCod) as 'DESCRIPCIÓN DE FALLO NO CODIFICADO',UPPER(t1.ObservacionesSupervision) as 'OBSERVACIONES DE SUPERVISIÓN',(select upper(concat(date_format(x5.HoraInicioM,'%W %d de %M del %Y'),' / ',time_format(x5.HoraInicioM,'%H:%i'))) from reportemantenimiento as x5 where x5.FoliofkSupervicion=t1.idReporteSupervicion) as 'FECHA/HORA INICIO MANTENIMIENTO',(select upper(concat(date_format(x6.HoraTerminoM,'%W %d de %M del %Y'),' / ',time_format(x6.HoraTerminoM,'%H:%i'))) from reportemantenimiento as x6 where x6.FoliofkSupervicion=t1.idReporteSupervicion)as 'FECHA/HORA TERMINO MANTENIMIENTO',COALESCE((SELECT UPPER(x13.EsperaTiempoM) FROM reportemantenimiento AS x13 WHERE x13.FoliofkSupervicion=t1.idReporteSupervicion),'00:00:00' ) AS 'ESPERA DE MANTENIMIENTO',COALESCE((select UPPER(x7.DiferenciaTiempoM)  from reportemantenimiento as x7 where x7.FoliofkSupervicion=t1.idReporteSupervicion),'00:00:00')as 'TIEMPO MANTENIMIENTO', COALESCE((select UPPER(x8.Estatus) from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'ESTATUS',COALESCE((select UPPER(x9.TrabajoRealizado) from reportemantenimiento as x9 where x9.FoliofkSupervicion=t1.idReporteSupervicion),'') as 'TRABAJO REALIZADO',COALESCE((select UPPER(concat(x11.ApPaterno,' ',x11.ApMaterno,' ',x11.nombres)) from cpersonal as x11 inner join reportemantenimiento as x12 on x11.idPersona=x12.MecanicofkPersonal where x12.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'MECÁNICO QUE REALIZÓ EL MANTENIMIENTO',COALESCE((select UPPER(x10.ObservacionesM) from reportemantenimiento as x10 where x10.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'OBSERVACIONES DE MANTENIMIENTO' from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas AS t4 on t4.idarea=t2.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas";
        public Supervisión(int idUsuario, int empresa, int area, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            cmbUnidad.DrawItem += v.combos_DrawItem;
            cmbServicio.DrawItem += v.combos_DrawItem;
            cbgrupo.DrawItem += v.combos_DrawItem;
            cbSubGrupo.DrawItem += v.combos_DrawItem;
            cbcategoria.DrawItem += v.combos_DrawItem;
            cmbCodFallo.DrawItem += v.combos_DrawItem;
            cmbEmpresa.DrawItem += v.combos_DrawItem;
            cmbBuscarUnidad.DrawItem += v.combos_DrawItem;
            cmbBuscarDescripcion.DrawItem += v.combos_DrawItem;
            cmbSupervisores.DrawItem += v.combos_DrawItem;
            cmbUnidad.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbServicio.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbTipoFallo.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cbSubGrupo.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbCodFallo.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbBuscarUnidad.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbBuscarDescripcion.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbBuscStatus.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbMeses.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbSupervisores.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            this.empresa = empresa; this.area = area; this.idUsuario = idUsuario;
            v.ChangeControlStyles(btnGuardar, ControlStyles.Selectable, true);
        }
        void quitarseen()
        {
            while (res)
            {
                MySqlConnection dbcon;
                if (v.c.conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                else
                    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET seen = 1 WHERE seen  = 0 AND (Estatus='LIBERADA' || Estatus='REPROGRAMADA')", dbcon);
                cmd.ExecuteNonQuery();
                dbcon.Close();
                dbcon.Dispose();
                Thread.Sleep(180000);
            }
        }

        bool bandera = false, editar = false, mensaje = false, exportando = false;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool getboolfromint(int i)
        {
            return i == 1;
        }
        public void privilegios()
        {
            string sql = "SELECT privilegios as privilegios FROM privilegios where usuariofkcpersonal='" + idUsuario + "' and namform='Form1'";
            string[] privilegios = v.getaData(sql).ToString().Split('/');
            pinsertar = getboolfromint(Convert.ToInt32(privilegios[0]));
            pconsultar = getboolfromint(Convert.ToInt32(privilegios[1]));
            peditar = getboolfromint(Convert.ToInt32(privilegios[2]));
        }
        void cmbUnidad_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void Muestra_empresas()
        {
            v.iniCombos("select t1.idempresa as id,UPPER(t1.nombreEmpresa)as nom  from cempresas as t1 where empresa ='1' order by t1.nombreEmpresa;", cmbEmpresa, "id", "nom", "--SELECCIONE EMPRESA--");
        }
        public void cargar_supervisor()
        {
            v.iniCombos("SELECT idpersona,UPPER(CONCAT(T1.ApPaterno,' ',T1.ApMaterno,' ',T1.nombres)) AS NOMBRE FROM cpersonal AS T1 INNER JOIN datosistema AS T2 on T2.usuariofkcpersonal=T1.idpersona WHERE t1.area='1' and t1.empresa='1' ORDER BY ApPaterno;", cmbSupervisores, "idpersona", "NOMBRE", "--SELECCIONE SUPERVISOR--");
        }
        public void Unidades() //Metodo para aregar las unidades de la tabla cunidades y mostrarlas en el comboBox para seleccionar una unidad el hacer un nuevo reporte o editar alguno.
        {
            v.iniCombos("SELECT t1.idunidad,concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECo FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea where t1.status='1' order by eco;", cmbUnidad, "idunidad", "ECo", "--SELECCIONE UNIDAD--");
        }

        public void combos_para_otros_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background 
                e.DrawBackground();

                // Drawing one of the items? 
                if (e.Index >= 0)
                {
                    // Set the string alignment. Choices are Center, Near and Far 
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings 
                    // Assumes Brush is solid 
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush 
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        // Draw the string 
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        // Draw the string 
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }

        void Buscar_descripcion()
        {
            v.iniCombos("Select iddescfallo as id,upper(descfallo) as d  from cdescfallo order by descfallo;;", cmbBuscarDescripcion, "id", "d", "--SELECCIONE SUBGRUPO--");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();
            privilegios();
            lblFechaReporte.Text = DateTime.Now.ToLongDateString().ToUpper();
            cargarDAtos();
            Genera_Folio();
            cargar_supervisor();
            Buscar_descripcion();
            consulta_descripciones();
            Unidades();
            Mostrar();
            typeoffallos();
            Muestra_empresas();
            consulta_grupos();
            DgvTabla.ClearSelection();
            cmbTipoFallo.SelectedIndex = cmbMeses.SelectedIndex = cmbBuscStatus.SelectedIndex = 0;
            dtpFechaDe.Enabled = dtpFechaA.Enabled = btnEditar.Visible = LblPDF.Visible = bPdf.Visible = lblactualizar.Visible = cmbServicio.Enabled = false;
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

        public void Mostrar()
        {
            if (pinsertar)
            {
                GpbSupervisión.Visible = LblNuevoR.Visible = btnNuevo.Visible = DgvTabla.Visible = true;
                DgvTabla.Size = new Size(1920, 400);
            }
            if (peditar)
            {
                GpbBusquedas.Visible = btnEditar.Visible = lblactualizar.Visible = LblNota.Visible = LblNota1.Visible = DgvTabla.Visible = true;
                DgvTabla.Size = new Size(1920, 282);
            }
            if (pconsultar)
            {
                GbpMantenimiento.Visible = GpbBusquedas.Visible = DgvTabla.Visible = true;
                DgvTabla.Size = new Size(1920, 282);
            }
            if (pinsertar && pconsultar && peditar)
                LblPDF.Visible = bPdf.Visible = true;
        }
        void Genera_Folio()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(SUBSTRING(Folio,LENGTH(FOLIO)-6,7)+1)AS Folio from reportesupervicion WHERE idReporteSupervicion = (SELECT MAX(idReporteSupervicion) FROM reportesupervicion);", v.c.dbconection());
            string Folio = (string)cmd.ExecuteScalar();
            if (Folio == null)
                Folio = "0000001";
            else
                while (Folio.Length < 7)
                    Folio = "0" + Folio;
            lblFolio.Text = "TRA" + Folio.ToString();
            v.c.dbconection();
        }
        public void cargarDAtos()//Metodo para cargar los reportes que se encuentra en la base de datos en el datagridview y para generar el folio de reporte autoincrementable
        {
            //Conusulta para pbtener reportes almacenados en la base de datos
            MySqlDataAdapter cargar = new MySqlDataAdapter(consulta_gral + " WHERE FechaReporte BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY))AND  curdate() order by t1.FechaReporte desc, t1.HoraEntrada desc;", v.c.dbconection());
            DataSet ds = new DataSet();
            cargar.Fill(ds);
            DgvTabla.DataSource = ds.Tables[0];
            DgvTabla.Columns[0].Frozen = true;// mostramos reportes en datagridview
            DgvTabla.ClearSelection();
            v.c.dbconection().Close();
            DgvTabla.ClearSelection();
        }
        void limpia_act()
        {
            btnActualizar.Visible = LblActTabla.Visible = false;
        }
        private void txtDescFalloNoC_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }
        private void txtConductor_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }
        public void limpiarmant()//Creamos método para limpiar campos donde se muestra información de mantenimiento
        {
            lblHIM.Text = lblHTM.Text = lblestatus.Text = LblTrabajoRealizado.Text = lblMecanico.Text = LblObsevacionesMantenimiento.Text = lblEsperaDeMan.Text = lblTM.Text = "";
        }
        void realiza_busquedas()
        {
            if (checkBox1.Checked || (cmbSupervisores.SelectedIndex > 0 || cmbBuscarUnidad.SelectedIndex > 0 || cmbBuscarDescripcion.SelectedIndex > 0 || cmbBuscStatus.SelectedIndex > 0 || cmbMeses.SelectedIndex > 0 || cmbEmpresa.SelectedIndex > 0))
            {
                if ((dtpFechaA.Value.Date < dtpFechaDe.Value.Date || dtpFechaA.Value.Date > DateTime.Now) && checkBox1.Checked)
                    MessageBox.Show("Las fechas seleccionadas son incorrectas".ToUpper(), "VERIFICAR FECHAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    string wheres = "";
                    if (checkBox1.Checked)
                        wheres = (wheres == "" ? "WHERE (SELECT t1.FechaReporte BETWEEN '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "')" : wheres += " AND (SELECT t1.FechaReporte BETWEEN '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "')");
                    if (cmbBuscarUnidad.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where (select concat(t4.identificador,LPAD(consecutivo,4,'0')))='" + cmbBuscarUnidad.Text + "' " : wheres += " AND (select concat(t4.identificador,LPAD(consecutivo,4,'0')))='" + cmbBuscarUnidad.Text + "'");
                    if (cmbBuscarDescripcion.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where (select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)='" + cmbBuscarDescripcion.Text + "'" : wheres += " AND (select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)='" + cmbBuscarDescripcion.Text + "'");
                    if (cmbSupervisores.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where (select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)='" + cmbSupervisores.Text + "'" : wheres += " AND (select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)='" + cmbSupervisores.Text + "'");
                    if (cmbBuscStatus.SelectedIndex > 0)
                        wheres = (wheres == "" ? " WHERE (select x8.Estatus from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion)='" + cmbBuscStatus.Text + "'" : wheres += " AND (select x8.Estatus from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion)='" + cmbBuscStatus.Text + "'");
                    if (cmbMeses.SelectedIndex > 0)
                        wheres = (wheres == "" ? " WHERE (select Date_format(t1.FechaReporte,'%W %d %M %Y') like '%" + cmbMeses.Text + "%' and (select year(t1.FechaReporte))=( select year(now())))" : wheres += " AND (select Date_format(t1.FechaReporte,'%W %d %M %Y') like '%" + cmbMeses.Text + "%' and (select year(t1.FechaReporte))=( select year(now())))");
                    if (cmbEmpresa.SelectedIndex > 0)
                        wheres = (wheres == "" ? " WHERE T5.idempresa='" + cmbEmpresa.SelectedValue + "'" : wheres += " order by FechaReporte desc, t1.HoraEntrada desc ");
                    if (wheres != "")
                        wheres += " order by FechaReporte desc, t1.HoraEntrada desc ";
                    MySqlDataAdapter DT = new MySqlDataAdapter(consulta_gral + wheres, v.c.dbconection());
                    DataSet ds = new DataSet();
                    DT.Fill(ds);
                    DgvTabla.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron reportes".ToUpper(), "NINGÚN REPORTE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cargarDAtos();
                        esta_exportando();
                        btnActualizar.Visible = LblActTabla.Visible = false;
                    }
                    else
                    {
                        if (peditar && pconsultar && peditar)
                        {
                            if (!estado)
                                btnExcel.Visible = true;
                            LblExcel.Visible = true;
                        }
                        btnActualizar.Visible = LblActTabla.Visible = true;
                    }
                    v.c.dbconection();
                    limpiarbusqueda();
                    checkBox1.Checked = false;
                }
            }
            else
                MessageBox.Show("Seleccione un criterio de búsqueda".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public void limpiarbusqueda()//Creamos método para limpiar campos de busqueda.
        {
            cmbBuscarDescripcion.SelectedIndex = cmbEmpresa.SelectedIndex = cmbBuscStatus.SelectedIndex = cmbMeses.SelectedIndex = cmbSupervisores.SelectedIndex = 0;
            dtpFechaDe.ResetText();
            dtpFechaA.ResetText();
        }
        private void txtSupervisor_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validamos que se permitan ingresar letras, números y ciertos carácteres en la caja de texto.
            v.letrasynumerossinespacios(e);
        }
        public void LimpiarReporte()//Creamos metodo para limpiar campos de reporte de supervisión
        {
            cmbUnidad.SelectedIndex = cmbTipoFallo.SelectedIndex = cbgrupo.SelectedIndex = idsupervisor = 0;
            txtSupervisor.Clear();
            lblSupervisor.Text = lblid.Text = lblCredCond.Text = "";
            txtConductor.Clear();
            txtKilometraje.Clear();
            txtDescFalloNoC.Clear();
            txtObserSupervicion.Clear();
            lblFechaReporte.Text = DateTime.Now.ToLongDateString().ToUpper();
            cmbCodFallo.Enabled = bandera = editar = mensaje = btnEditar.Visible = lblactualizar.Visible = bPdf.Visible = resmod = LblPDF.Visible = cmbServicio.Enabled = false;
            DgvTabla.ClearSelection();
            btnGuardar.Visible = LblGuardar.Visible = true;
            txtKilometraje.MaxLength = 12;
            esta_exportando();
        }
        void typeoffallos()
        {
            ComboBox cbx = cmbTipoFallo;
            cbx.DataSource = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("idtype");
            dt.Columns.Add("type");
            DataRow dr = dt.NewRow();
            dr["idtype"] = 0;
            dr["type"] = "--SELECCIONE TIPO DE FALLO--";
            dt.Rows.InsertAt(dr, 0);
            dr = dt.NewRow();
            dr["idtype"] = 1;
            dr["type"] = "CORRECTIVO";
            dt.Rows.InsertAt(dr, 1);
            dr = dt.NewRow();
            dr["idtype"] = 2;
            dr["type"] = "PREVENTIVO";
            dt.Rows.InsertAt(dr, 2);
            dr = dt.NewRow();
            dr["idtype"] = 3;
            dr["type"] = "REITERATIVO";
            dt.Rows.InsertAt(dr, 3);
            dr = dt.NewRow();
            dr["idtype"] = 4;
            dr["type"] = "REPROGRAMADO";
            dt.Rows.InsertAt(dr, 4);
            dr = dt.NewRow();
            dr["idtype"] = 5;
            dr["type"] = "SEGUIMIENTO";
            dt.Rows.InsertAt(dr, 5);
            cbx.ValueMember = "idtype";
            cbx.DisplayMember = "type";
            cbx.DataSource = dt;
        }
        private void To_pdf()//Método generar PDF
        {
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';Select upper(concat( t1.Folio,'|',concat(t4.identificador,LPAD(consecutivo,4,'0')),'|',date_format(t1.FechaReporte,'%W %d de %M del %Y'),'|',(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal),'|',(select credencial from cpersonal as x2 where x2.idpersona=t1.CredencialConductorfkCPersonal),'|',if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios)),'|',t1.HoraEntrada,'|',t1.kmEntrada,'|',t1.tipoFallo,'|',if(t1.DescrFallofkcdescfallo is null,t1.DescFalloNoCod,concat((select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo),'|',(select x4.codfallo from cfallosesp as x4 where x4.idfalloEsp=t1.CodFallofkcfallosesp))),'|',coalesce(t1.ObservacionesSupervision,''),'|',coalesce(concat(date_format(t5.HoraInicioM,'%W %d de %M del %Y'),' / ',time_format(t5.HoraInicioM,'%H:%i')),''),'|',coalesce(date_format(t5.HoraTerminoM,'%W %d de %M del %Y'),''),'|',coalesce(t5.EsperaTiempoM,''),'|',coalesce(t5.DiferenciaTiempoM,''),'|',coalesce(t5.Estatus,'EN PROCESO'),'|',coalesce(t5.TrabajoRealizado,''),'|',coalesce((select concat(x5.appaterno,' ',x5.apmaterno,' ',x5.nombres) from cpersonal as x5 where t5.MecanicofkPersonal=x5.idpersona),''),'|',coalesce(t5.ObservacionesM,''))) as r from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas as t4 on t4.idarea=t2.areafkcareas left join reportemantenimiento as t5 on t5.FoliofkSupervicion=t1.idReporteSupervicion WHERE t1.folio='" + lblFolio.Text + "'").ToString().Split('|');
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
                        filename = filename + ".pdf";
                    while (filename.ToLower().Contains(".pdf.pdf"))
                        filename = filename.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
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
                    Chunk chunk = new Chunk("REPORTE SUPERVISIÓN", FontFactory.GetFont("Calibri", 18, iTextSharp.text.Font.BOLD));
                    byte[] img = Convert.FromBase64String(v.transmasivo);
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(16f);
                    imagen.SetAbsolutePosition(400f, 705f);
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(new Paragraph(chunk));
                    doc.Add(imagen);
                    PdfPTable tabla = new PdfPTable(3);
                    tabla.DefaultCell.Border = 1;
                    tabla.WidthPercentage = 100;
                    bool haveFallo = (datos.Length > 19);
                    tabla.AddCell(v.valorCampo("\n\n\n", 3, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("FOLIO:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("UNIDAD:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("FECHA DEL REPORTE:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(datos[0], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[1], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[2], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("SUPERVISOR:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("CREDENCIAL DE CONDUCTOR:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("SERVICIO:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(datos[3], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[4], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[5], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("HORA DEL REPORTE: ", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("KILOMETRAJE:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("TIPO DE FALLO:", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(datos[6], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[7], 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo((Convert.ToInt32(datos[8]) == 1 ? "CORRECTIVO" : Convert.ToInt32(datos[8]) == 2 ? "PREVENTIVO" : Convert.ToInt32(datos[8]) == 3 ? "REITERATIVO" : Convert.ToInt32(datos[8]) == 4 ? "REPROGRAMADO" : "SEGUIMIENTO"), 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    if (!haveFallo)
                    {
                        tabla.AddCell(v.valorCampo("DESCRIPCIÓN DE FALLO NO CÓDIFICADO", 3, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo(datos[9], 3, 0, 0, arial));
                    }
                    else
                    {
                        tabla.AddCell(v.valorCampo("GRUPO:", 1, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo("SUBGRUPO:", 1, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo("CATEGORÍA:", 1, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                        tabla.AddCell(v.valorCampo(datos[9], 3, 0, 0, arial));
                        tabla.AddCell(v.valorCampo(datos[10], 3, 0, 0, arial));
                        tabla.AddCell(v.valorCampo(datos[11], 3, 0, 0, arial));
                        tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                        tabla.AddCell(v.valorCampo("CÓDIGO DE FALLO:", 1, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo("NOMBRE DE FALLO:", 2, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                        tabla.AddCell(v.valorCampo("cejmplo", 1, 0, 0, arial));
                        tabla.AddCell(v.valorCampo("nejemplo", 2, 0, 0, arial));
                    }
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("OBSERVACIONES DE SUPERVISIÓN:", 3, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo((!haveFallo ? datos[10] : datos[12]), 3, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("DATOS DE MANTENIMIENTO", 3, 0, 0, FontFactory.GetFont("ARIAL", 18, iTextSharp.text.Font.BOLD)));
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    PdfPTable tabla2 = new PdfPTable(2);
                    tabla2.DefaultCell.Border = 0;
                    tabla2.WidthPercentage = 100;
                    tabla2.AddCell(v.valorCampo("FECHA/HORA DE INICIO:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("FECHA/HORA DE LIBERACIÓN:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[11] : datos[13]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[12] : datos[14]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("TIEMPO DE ESPERA:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("TIEMPO DE MANTENIMIENTO:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[13] : datos[15]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[14] : datos[16]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("ESTATUS DE UNIDAD:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("TRABAJO REALIZADO:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[15] : datos[17]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[16] : datos[18]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("MECÁNICO QUE REALIZÓ EL MANTENIMIENTO:", 2, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[17] : datos[19]), 2, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("OBSERVACIONES DE MANTENIMIENTO:", 2, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("                ", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[18] : datos[19]), 2, 0, 0, arial));
                    doc.Add(tabla);
                    doc.Add(tabla2);
                    doc.Close();
                    System.Diagnostics.Process.Start(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void HabilitarCampos()
        {
            txtSupervisor.Enabled = txtConductor.Enabled = cmbServicio.Enabled = txtKilometraje.Enabled = cmbTipoFallo.Enabled = cbgrupo.Enabled = txtObserSupervicion.Enabled = true;
            txtDescFalloNoC.Enabled = (cbSubGrupo.SelectedIndex > 0 ? false : true);
        }
        public void DeshabilitarCampos()
        {
            txtSupervisor.Enabled = txtConductor.Enabled = cmbServicio.Enabled = txtKilometraje.Enabled = cmbTipoFallo.Enabled = cbgrupo.Enabled = cbcategoria.Enabled = cbSubGrupo.Enabled = cmbCodFallo.Enabled = txtDescFalloNoC.Enabled = txtObserSupervicion.Enabled = false;
        }
        void restaurar_datos(DataGridViewCellEventArgs e)
        {
            if (DgvTabla.Rows.Count > 0)
            {
                btnGuardar.Visible = LblGuardar.Visible = false;
                lblFolio.Text = DgvTabla.Rows[e.RowIndex].Cells[0].Value.ToString();
                string[] datos = v.getaData("SET lc_time_names = 'es_ES';Select upper(concat(t1.UnidadfkCUnidades,'|',date_format(t1.FechaReporte,'%W %d de %M del %Y'),'|',t1.CredencialConductorfkCPersonal,'|',t1.SupervisorfkCPersonal,'|',t1.Serviciofkcservicios,'|',t1.kmEntrada,'|',t1.tipoFallo,'|',coalesce(t1.DescrFallofkcdescfallo,0),'|',coalesce(t1.CodFallofkcfallosesp,0),'|',coalesce(t1.DescFalloNoCod,''),'|',coalesce(t1.ObservacionesSupervision,''),'|',coalesce(t5.EsperaTiempoM,''),'|',coalesce(concat(date_format(t5.HoraInicioM,'%W %d de %M del %Y'),' / ',time_format(t5.HoraInicioM,'%H:%i')),''),'|',coalesce(date_format(t5.HoraTerminoM,'%W %d de %M del %Y'),''),'|',coalesce(t5.DiferenciaTiempoM,''),'|',coalesce(t5.Estatus,''),'|',coalesce(t5.TrabajoRealizado,''),'|',coalesce((select concat(x5.appaterno,' ',x5.apmaterno,' ',x5.nombres) from cpersonal as x5 where t5.MecanicofkPersonal=x5.idpersona),''),'|',coalesce(t5.ObservacionesM,''))) as r from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas as t4 on t4.idarea=t2.areafkcareas left join reportemantenimiento as t5 on t5.FoliofkSupervicion=t1.idReporteSupervicion WHERE Folio='" + lblFolio.Text + "';").ToString().Split('|');
                IdRepor = Convert.ToInt32(v.getaData("SELECT idreportesupervicion FROM reportesupervicion WHERE folio='" + lblFolio.Text + "'").ToString());
                cmbUnidad.SelectedValue = unidadAnterior = Convert.ToInt32(datos[0]);
                lblFechaReporte.Text = fechaAnterior = datos[1];
                lblSupervisor.Text = v.getaData("select concat(appaterno,' ',apmaterno,' ',nombres) from cpersonal where idpersona='" + (supervisorAnterior = Convert.ToInt32(datos[3])) + "'").ToString();
                txtConductor.Text = v.getaData("select credencial from cpersonal where idpersona='" + (idconductor = conductorAnterior = Convert.ToInt32(datos[2])) + "'").ToString();
                if (Convert.ToInt32(v.getaData("select status from cunidades where idunidad='" + unidadAnterior + "';")) == 0)
                    v.iniCombos("SELECT t1.idunidad,concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECo FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea where t1.status='1' or t1.idunidad='" + unidadAnterior + "' order by eco", cmbUnidad, "idunidad", "eco", "--SELECCIONE ECONÓMICO");
                if (Convert.ToInt32(v.getaData("select status from cservicios where idservicio='" + datos[4] + "';")) == 0)
                {
                    cmbBuscarUnidad.DataSource = null;
                    DataTable dt = (DataTable)v.getData("select idservicio as id,upper(concat(Nombre,' ',descripcion)) as nombre from cservicios as t1 inner join careas as t2 on t1.AreafkCareas=t2.idarea where  (t2.empresafkcempresas='" + empresa + "' and t1.status='1' and (select areafkcareas from cunidades where idunidad='" + cmbUnidad.SelectedValue + "')=t1.AreafkCareas) or(idservicio='" + datos[4] + "') order by nombre;;");
                    DataRow nuevaFila = dt.NewRow();
                    DataRow _sin = dt.NewRow();
                    nuevaFila["id"] = 0;
                    nuevaFila["nombre"] = "--Seleccione un servicio--".ToUpper();
                    _sin["id"] = 1;
                    _sin["nombre"] = "sin servicio".ToUpper();
                    dt.Rows.InsertAt(nuevaFila, 0);
                    dt.Rows.InsertAt(_sin, 1);
                    cmbServicio.DisplayMember = "nombre";
                    cmbServicio.ValueMember = "id";
                    cmbServicio.DataSource = dt;
                }
                cmbServicio.SelectedValue = servicioAnterior = Convert.ToInt32(datos[4]);
                txtKilometraje.Text = kilometrajeAnterior = datos[5];
                cmbTipoFallo.SelectedValue = TipoFalloAnterior = Convert.ToInt32(datos[6]);
                if (Convert.ToInt32(v.getaData("select status from cfallosgrales where idFalloGral='" + (grupoFalloAnterior = Convert.ToInt32(v.getaData("select coalesce(falloGralfkcfallosgrales,0) from cdescfallo where iddescfallo='" + datos[7] + "'"))) + "';")) == 0)
                    v.iniCombos("select idFalloGral as id,upper(nombreFalloGral) as n from cfallosgrales where status='1' or idFalloGral='" + grupoFalloAnterior + "' order by nombreFalloGral;", cbgrupo, "id", "n", "--SELECCIONE GRUPO--");
                if (Convert.ToInt32(v.getaData("select status from cdescfallo where iddescfallo='" + datos[7] + "';")) == 0)
                    v.iniCombos("select iddescfallo as id,upper(descfallo) as d from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idfallogral=t1.falloGralfkcfallosgrales where t2.idfallogral='" + cbgrupo.SelectedValue + "' and t1.status='1' or(t1.iddescfallo='" + datos[7] +"') order by descfallo;", cbSubGrupo, "id", "d", "--SELECCIONE SUBGRUPO--");
                if (Convert.ToInt32("select status from catcategorias where idcategoria='" + (categoriaAnterior = Convert.ToInt32(v.getaData("select coalesce(idcategoria,0) from catcategorias where subgrupofkcdescfallo='" + datos[7]) + "';")) + "';") == 0)
                    v.iniCombos("select t3.idcategoria as id ,upper(t3.categoria) as c from cdescfallo as t1 inner join catcategorias as t3 on t3.subgrupofkcdescfallo=t1.iddescfallo where iddescfallo='" + Convert.ToInt32(datos[7]) + "' order by categoria;", cbcategoria, "id", "c", "--SELECCIONE CATEGORIA--");
                if (Convert.ToInt32(v.getaData("select status from cfallosesp where idfalloEsp='" + (codigoAnterior = Convert.ToInt32(datos[8])) + "';")) == 0)
                    v.iniCombos("select t1.idfalloEsp as id,upper(t1.codfallo)as c from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t2.idcategoria='" + categoriaAnterior+ "';", cmbCodFallo, "id", "c", "--SELECCIONE CÓDIGO");
                cbgrupo.SelectedValue = grupoFalloAnterior;
                cbSubGrupo.SelectedValue = subGrupoAnterior = Convert.ToInt32(datos[7]);
                cbcategoria.SelectedValue = categoriaAnterior;
                cmbCodFallo.SelectedValue = codigoAnterior;
                txtDescFalloNoC.Text = FalloAnterior = datos[9];
                txtObserSupervicion.Text = ObservacionesAnterior = datos[10];
                lblEsperaDeMan.Text = datos[11];
                lblHIM.Text = datos[12];
                lblHTM.Text = datos[13];
                lblTM.Text = datos[14];
                lblestatus.Text = datos[15];
                LblTrabajoRealizado.Text = datos[16];
                lblMecanico.Text = datos[17];
                LblObsevacionesMantenimiento.Text = datos[18];
                btnEditar.Visible = lblactualizar.Visible = false;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (peditar)
                {
                    Verifica_modificaciones();
                    if (!resmod)
                    {
                        restaurar_datos(e);
                        editar = bPdf.Visible = LblPDF.Visible = true;
                        if (lblestatus.Text == "LIBERADA")
                        {
                            if (pinsertar && peditar && pconsultar)
                            {
                                cmbUnidad.Enabled = true;
                                HabilitarCampos();
                            }
                            else
                            {
                                btnEditar.Visible = lblactualizar.Visible = false;
                                DeshabilitarCampos();
                            }
                        }
                        else
                        {
                            HabilitarCampos();
                            txtKilometraje.MaxLength = 12;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No cuenta con los privilegios para editar un reporte".ToUpper(), "SIN PRIVILEGIOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void oculta_botones()
        {
            btnEditar.Visible = lblactualizar.Visible = bPdf.Visible = LblPDF.Visible = false;
        }
        void Editar_reporte()
        {
            observacionesEdicion obs = new observacionesEdicion(v);
            obs.Owner = this;
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string motivo = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                string consulta = "Update reportesupervicion as t1 set t1.CredencialConductorfkCPersonal='" + Convert.ToInt32(idconductor) + "', t1.Serviciofkcservicios='" + cmbServicio.SelectedValue + "', t1.KmEntrada='" + txtKilometraje.Text + "', t1.TipoFallo='" + cmbTipoFallo.SelectedValue + "', t1.ObservacionesSupervision='" + txtObserSupervicion.Text.Trim() + "'";
                consulta = (cbgrupo.SelectedIndex == 0 ? consulta += ",t1.DescFalloNoCod = '" + txtDescFalloNoC.Text.Trim() + "',t1.DescrFallofkcdescfallo=null,t1.CodFallofkcfallosesp=null" : consulta += ",t1.DescrFallofkcdescfallo='" + cbSubGrupo.SelectedValue + "', t1.CodFallofkcfallosesp='" + cmbCodFallo.SelectedValue + "',t1.DescFalloNoCod=null");
                consulta += " WHERE t1.Folio='" + lblFolio.Text + "';";
                if (v.c.insertar(consulta))
                    MessageBox.Show("Registro actualizado exitosamente ".ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                v.c.dbconection();
                Modificaciones_tabla(motivo);
                esta_exportando();
                oculta_botones();
                cmbCodFallo.Enabled = !(btnGuardar.Enabled = cmbUnidad.Enabled = true);
                limpiarmant();
                cargarDAtos();
                limpia_act();
                Genera_Folio();
                HabilitarCampos();
                DgvTabla.ClearSelection();
                LimpiarReporte();
            }
        }
        void Modificaciones_tabla(string motivo)
        {
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Reporte de Supervisión','" + IdRepor + "','" + unidadAnterior + ";" + supervisorAnterior + ";" + conductorAnterior + ";" + servicioAnterior + ";" + kilometrajeAnterior + ";" + TipoFalloAnterior + ";";
            sql = (!string.IsNullOrWhiteSpace(FalloAnterior) ? sql += FalloAnterior + ";" : sql += subGrupoAnterior + ";" + codigoAnterior + ";");
            sql = (!string.IsNullOrWhiteSpace(ObservacionesAnterior) ? sql += ObservacionesAnterior : sql += "");
            sql += "','" + idsupervisor + "',NOW(),'Actualización de Reporte de Supervisión','" + motivo + "','1','1')";
            v.c.insertar(sql);
        }
        void actualiza_datos()
        {
            if (v.campossupervision(lblFolio.Text, cmbUnidad.SelectedIndex, txtSupervisor.Text, txtConductor.Text, cmbServicio.SelectedIndex, txtKilometraje.Text, cmbTipoFallo.SelectedIndex, cbgrupo.SelectedIndex, cbSubGrupo.SelectedIndex, cbcategoria.SelectedIndex, cmbCodFallo.SelectedIndex, txtDescFalloNoC.Text))
                Editar_reporte();
        }
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "EXPORTANDO";
        }
        public void consulta_categorias()
        {
            v.iniCombos("select t3.idcategoria as id ,upper(t3.categoria) as c from cdescfallo as t1 inner join catcategorias as t3 on t3.subgrupofkcdescfallo=t1.iddescfallo where iddescfallo='" + cbSubGrupo.SelectedValue + "' order by categoria;", cbcategoria, "id", "c", "--SELECCIONE CATEGORIA--");
        }
        private void cmbDescFallo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubGrupo.SelectedIndex > 0)
            {
                consulta_categorias();
                cbcategoria.Enabled = true;
            }
            else
            {
                cbcategoria.DataSource = null;
                cbcategoria.Enabled = false;
            }
        }
        public void consulta_subgrupos()
        {
            v.iniCombos("select iddescfallo as id,upper(descfallo) as d from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idfallogral=t1.falloGralfkcfallosgrales where t2.idfallogral='" + cbgrupo.SelectedValue + "' order by descfallo;", cbSubGrupo, "id", "d", "--SELECCIONE SUBGRUPO--");
        }
        private void cbgrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbgrupo.SelectedIndex > 0)
            {
                consulta_subgrupos();
                txtDescFalloNoC.Clear();
                cbSubGrupo.Enabled = !(txtDescFalloNoC.Enabled = false);
            }
            else
            {
                cbSubGrupo.DataSource = null;
                txtDescFalloNoC.Enabled = !(cbSubGrupo.Enabled = false);
            }
        }
        private void cmbCodFallo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDescFallo.Text = (cmbCodFallo.SelectedIndex > 0 ? v.getaData("Select UPPER(coalesce(falloesp,'')) as fallo from cfallosesp where idfalloEsp='" + cmbCodFallo.SelectedValue + "'").ToString() : "");
        }
        public void consulta_codigos()
        {
            v.iniCombos("select t1.idfalloEsp as id,upper(t1.codfallo)as c from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t2.idcategoria='" + cbcategoria.SelectedValue + "';", cmbCodFallo, "id", "c", "--SELECCIONE CÓDIGO");
        }
        private void cbcategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbcategoria.SelectedIndex > 0)
            {
                cmbCodFallo.Enabled = true;
                consulta_codigos();
            }
            else
            {
                cmbCodFallo.DataSource = null;
                cmbCodFallo.Enabled = false;
            }
        }
        private void Supervisión_FormClosing(object sender, FormClosingEventArgs e)
        {
            hilo.Abort();
        }
        private void cmbTipoFallo_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color c = Color.BlueViolet;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color color = Color.FromArgb(246, 144, 123);
            SolidBrush s = new SolidBrush(color);
            Color fondo = Color.FromArgb(200, 200, 200);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            DataTable dt = (DataTable)cmbTipoFallo.DataSource;
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    break;
                case 2:
                    e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    break;
                case 3:
                    e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);
                    break;
                case 4:
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
                    break;
                case 5:
                    e.Graphics.FillRectangle(s, e.Bounds);
                    break;
            }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                e.Graphics.DrawString(dt.Rows[e.Index].ItemArray[1].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
                e.Graphics.DrawString(dt.Rows[e.Index].ItemArray[1].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
        }
        private void cmbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmpresa.SelectedIndex > 0)
            {
                if (Convert.ToInt32(v.getaData("SELECT count(*) FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where idempresa='" + cmbEmpresa.SelectedValue + "'")) > 0)
                {
                    v.iniCombos("SELECT  idunidad,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco  FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where idempresa='" + cmbEmpresa.SelectedValue + "'", cmbBuscarUnidad, "idunidad", "eco", "-sELECCIONE UN ECONÓMICO-");
                    cmbBuscarUnidad.Enabled = true;
                }
                else
                {
                    MessageBox.Show("La empresa seleccionada no cuenta con unidades registradas en el sistema", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbEmpresa.SelectedIndex = 0;
                    cmbBuscarUnidad.Enabled = false;
                }
            }
            else
            {
                cmbBuscarUnidad.DataSource = null;
                cmbBuscarUnidad.Enabled = false;
            }
        }
        private void cmbUnidad_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbBuscarUnidad.DataSource = null;
            DataTable dt = (DataTable)v.getData("select idservicio as id,upper(concat(Nombre,' ',descripcion)) as nombre from cservicios as t1 inner join careas as t2 on t1.AreafkCareas=t2.idarea where  t2.empresafkcempresas='" + empresa + "' and t1.status='1' and (select areafkcareas from cunidades where idunidad='" + cmbUnidad.SelectedValue + "')=t1.AreafkCareas order by nombre;");
            DataRow nuevaFila = dt.NewRow();
            DataRow _sin = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione un servicio--".ToUpper();
            _sin["id"] = 1;
            _sin["nombre"] = "sin servicio".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            dt.Rows.InsertAt(_sin, 1);
            cmbServicio.DisplayMember = "nombre";
            cmbServicio.ValueMember = "id";
            cmbServicio.DataSource = dt;
            if (cmbUnidad.SelectedIndex == 0)
                cmbServicio.DataSource = null;
            cmbServicio.Enabled = (cmbBuscarUnidad.SelectedIndex == 0 ? false : true);
        }
        public void TextoLargo(object sender, EventArgs e)
        {
            ((Label)sender).Font = ((((Label)sender).Text.Length >= 30) ? new System.Drawing.Font("Garamond", 10) : new System.Drawing.Font("Garamond", 12));
        }
        public void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            realiza_busquedas();
        }
        void esta_exportando()
        {
            if (peditar && pinsertar && pconsultar)
                if (LblExcel.Text.Equals("EXPORTANDO"))
                    exportando = true;
                else
                    btnExcel.Visible = LblExcel.Visible = false;
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarDAtos();
            btnActualizar.Visible = LblActTabla.Visible = false;
            esta_exportando();
        }
        bool estado = false;
        private void btnExcel_Click(object sender, EventArgs e)
        {
            estado = true;
            ThreadStart delegado = new ThreadStart(exporta_a_excel);
            exportar = new Thread(delegado);
            exportar.Start();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validaciones de campos vacios al momento de dar click al boton guardar
            if (v.campossupervision(lblFolio.Text, cmbUnidad.SelectedIndex, txtSupervisor.Text, txtConductor.Text, cmbServicio.SelectedIndex, txtKilometraje.Text, cmbTipoFallo.SelectedIndex, cbgrupo.SelectedIndex, cbSubGrupo.SelectedIndex, cbcategoria.SelectedIndex, cmbCodFallo.SelectedIndex, txtDescFalloNoC.Text))
            {
                string campos = "Insert into reportesupervicion (Folio,UnidadfkCUnidades,FechaReporte, SupervisorfkCPersonal, CredencialConductorfkCPersonal, Serviciofkcservicios,HoraEntrada,KmEntrada,TipoFallo,ObservacionesSupervision";
                string valores = "('" + lblFolio.Text + "' , '" + cmbUnidad.SelectedValue + "' ,(select curdate()) , '" + Convert.ToInt32(idsupervisor) + "' , '" + Convert.ToInt32(idconductor) + "' , '" + cmbServicio.SelectedValue + "',(select curtime()) , '" + txtKilometraje.Text + "' , '" + cmbTipoFallo.SelectedValue + "','" + txtObserSupervicion.Text.Trim() + "' ";
                campos = (cbgrupo.SelectedIndex == 0 ? campos += ",DescFalloNoCod)" : campos += ",DescrFallofkcdescfallo,CodFallofkcfallosesp)");
                valores = (cbgrupo.SelectedIndex == 0 ? valores += " ,'" + txtDescFalloNoC.Text.Trim() + "')" : valores += " ,'" + cbSubGrupo.SelectedValue + "','" + cmbCodFallo.SelectedValue + "')");
                if (v.c.insertar(campos + " values " + valores))
                    MessageBox.Show("Reporte guardado exitosamente ", "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                esta_exportando();
                limpia_act();
                cargarDAtos();
                Genera_Folio();
                LimpiarReporte();
            }
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            mensaje = !(bandera = true);
            Verifica_modificaciones();
        }

        private void bPdf_Click(object sender, EventArgs e)
        {
            To_pdf();//Llamamos a nuestro método To_pdf               
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (v.getData("select coalesce((select x1.estatus from reportemantenimiento as x1 where x1.FoliofkSupervicion=t1.idreportesupervicion),'') AS ESTATUS FROM reportesupervicion as t1 where t1.folio='" + lblFolio.Text + "'").ToString() != "LIBERADA" && (!peditar && !pinsertar && !pconsultar))
            {
                MessageBox.Show("La unidad ya se encuentra liberada, ya no es posible realizar modificaciones".ToUpper(), "UNIDAD LIBERADA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LimpiarReporte();
                Genera_Folio();
                limpiarmant();
                esta_exportando();
                cargarDAtos();
                limpia_act();
            }
            else
                actualiza_datos();
        }
        private void btnGuardar_MouseMove(object sender, MouseEventArgs e)
        {
            ((Button)sender).Size = new Size(58, 53);
        }
        private void btnGuardar_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).Size = new Size(55, 50);
        }
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "EXPORTAR";
            if (exportando)
                LblExcel.Visible = btnExcel.Visible = false;
            exportando = estado = false;
        }
        private void cmbDescFallo_Validating(object sender, CancelEventArgs e)
        {
            if (cbSubGrupo.SelectedIndex > 0)
                txtDescFalloNoC.Enabled = false;
        }
        private void DgvTabla_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public void exporta_a_excel()//Método para exportar a EXCEL.
        {
            if (DgvTabla.Rows.Count > 0)
            {
                dt = (DataTable)DgvTabla.DataSource;
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando);
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

                            rng.Interior.Color = ((dt.Rows[i][j].ToString() == "PREVENTIVO" || dt.Rows[i][j].ToString() == "LIBERADA") ? System.Drawing.ColorTranslator.ToOle(Color.PaleGreen) : (dt.Rows[i][j].ToString() == "CORRECTIVO" || dt.Rows[i][j].ToString() == "EN PROCESO") ? rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki) : (dt.Rows[i][j].ToString() == "REITERATIVO" || dt.Rows[i][j].ToString() == "REPROGRAMADA") ? rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral) : (dt.Rows[i][j].ToString() == "REPROGRAMADO") ? rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue) : (dt.Rows[i][j].ToString() == "SEGUIMIENTO") ? rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(246, 144, 123)) : System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230)));
                        }
                        catch (System.NullReferenceException EX)
                        { MessageBox.Show(EX.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                }
                Thread.Sleep(400);
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                try
                {
                    if (this.InvokeRequired)
                    {
                        El_Delegado1 delega2 = new El_Delegado1(cargando1);
                        this.Invoke(delega2);
                    }
                }
                catch (Exception ex)
                { MessageBox.Show(ex.ToString(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void cmbBuscStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color c = Color.BlueViolet;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color color = Color.FromArgb(246, 144, 123);
            SolidBrush s = new SolidBrush(color);
            Color fondo = Color.FromArgb(200, 200, 200);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    break;
                case 2:
                    e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    break;
                case 3:
                    e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);
                    break;
            }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
                e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (editar && peditar)
                btnEditar.Visible = lblactualizar.Visible = (existmodifications() ? true : false);
        }
        public bool existmodifications()
        {
            bool res = false;
            object grupof = (cbgrupo.SelectedValue ?? 0), subgrupof = cbSubGrupo.SelectedValue ?? 0, categoriaf = cbcategoria.SelectedValue ?? 0, codf = cmbCodFallo.SelectedValue ?? 0;
            if (editar && (idconductor != conductorAnterior || Convert.ToInt32(cmbServicio.SelectedValue) != servicioAnterior || txtKilometraje.Text != kilometrajeAnterior || Convert.ToInt32(cmbTipoFallo.SelectedValue) != TipoFalloAnterior || Convert.ToInt32(grupof) != grupoFalloAnterior || Convert.ToInt32(subgrupof) != subGrupoAnterior || Convert.ToInt32(categoriaf) != categoriaAnterior || Convert.ToInt32(codf) != codigoAnterior || txtDescFalloNoC.Text != FalloAnterior || ObservacionesAnterior != txtObserSupervicion.Text) && idsupervisor > 0 && idconductor > 0 && cmbServicio.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtKilometraje.Text) && cmbTipoFallo.SelectedIndex > 0 && ((cbgrupo.SelectedIndex > 0 && cbSubGrupo.SelectedIndex > 0 && cbcategoria.SelectedIndex > 0 && cmbCodFallo.SelectedIndex > 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text) || (!string.IsNullOrWhiteSpace(txtDescFalloNoC.Text) && cbgrupo.SelectedIndex == 0))))
                res = true;
            return res;
        }
        private void txtObserSupervicion_Validating_1(object sender, CancelEventArgs e)
        {
            while (txtObserSupervicion.Text.Contains("  ") || txtObserSupervicion.Text.Contains("\n"))
            {
                txtObserSupervicion.Text = txtObserSupervicion.Text.Replace("\n", "").Trim();
                txtObserSupervicion.Text = txtObserSupervicion.Text.Replace("  ", " ").Trim();
                txtObserSupervicion.SelectionStart = txtObserSupervicion.TextLength + 1;
            }
        }
        private void txtDescFalloNoC_Validating_1(object sender, CancelEventArgs e)
        {
            while (txtDescFalloNoC.Text.Contains("  "))
            {
                txtDescFalloNoC.Text = txtDescFalloNoC.Text.Replace("  ", " ").Trim();
                txtDescFalloNoC.SelectionStart = txtDescFalloNoC.TextLength;
            }
        }
        public void consulta_grupos()
        {
            v.iniCombos("select idFalloGral as id,upper(nombreFalloGral) as n from cfallosgrales where status='1' order by nombreFalloGral;", cbgrupo, "id", "n", "--SELECCIONE GRUPO--");
        }
        public void consulta_descripciones()
        {
            v.iniCombos("SELECT UPPER(t1.descfallo) as descfallo,(t1.iddescfallo) from cdescfallo as t1 where t1.status='1'  order by descfallo;", cbSubGrupo, "iddescfallo", "descfallo", "--SELECCIONE DESCRIPCIÓN--");
        }
        bool resmod = false;
        bool nuevor()
        {
            bool res = false;
            if (!editar && (cmbUnidad.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(lblSupervisor.Text) || !string.IsNullOrWhiteSpace(txtConductor.Text) || cmbServicio.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtKilometraje.Text) || cmbTipoFallo.SelectedIndex > 0 || cbSubGrupo.SelectedIndex > 0 || cmbCodFallo.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtDescFalloNoC.Text) || !string.IsNullOrWhiteSpace(txtObserSupervicion.Text) || cbgrupo.SelectedIndex > 0 || cbcategoria.SelectedIndex > 0))
                res = true;
            return res;
        }
        void Verifica_modificaciones()
        {
            if (nuevor() || existmodifications())
            {
                bool res = existmodifications();
                if (MessageBox.Show((res ? "¿Desea guardar las modificaciones?" : "¿Desea concluir el reporte?"), "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    resmod = true;
                    if (res)
                    {
                        mensaje = true;
                        actualiza_datos();
                        bPdf.Visible = LblPDF.Visible = btnEditar.Visible = lblactualizar.Visible = false;
                    }
                }
                else
                {
                    if (res)
                    {
                        Genera_Folio();
                        cmbCodFallo.Enabled = !(cmbUnidad.Enabled = btnGuardar.Enabled = true);
                        LimpiarReporte();
                        limpiarmant();
                        oculta_botones();
                        HabilitarCampos();
                        cmbUnidad.Focus();
                    }
                    else
                    {
                        if (bandera)
                        {
                            LimpiarReporte();
                            limpiarmant();
                        }
                    }
                }
            }
            else
            {
                Genera_Folio();
                cmbCodFallo.Enabled = !(cmbUnidad.Enabled = btnGuardar.Enabled = true);
                LimpiarReporte();
                limpiarmant();
                oculta_botones();
                HabilitarCampos();
                cmbUnidad.Focus();
            }
        }
        private void txtObserSupervicion_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }
        private void dtpFechaDe_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
        private void txtKilometraje_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtKilometraje.Text))
                {
                    double km = double.Parse(txtKilometraje.Text);
                    if (txtKilometraje.TextLength <= 3)
                        txtKilometraje.Text = string.Format("{0:F2}", km);
                    else
                    {
                        txtKilometraje.Text = Convert.ToString((Math.Floor(km * 100) / 100));
                        km = double.Parse(txtKilometraje.Text);
                        txtKilometraje.Text = string.Format("{0:N2}", km);
                        if (km > 2000000)
                            txtKilometraje.Text = "2,000,000.00";
                    }
                    Regex r4 = new Regex(@"^\d{1,3}\,\d{3}\.\d{2,2}$");
                    Regex r5 = new Regex(@"^\d{1,3}\,\d{3}\,\d{3}\.\d{2,2}");
                    Regex r1 = new Regex(@"^\d{0,3}\.\d{1,2}$");
                    if (!r1.IsMatch(txtKilometraje.Text) && !r4.IsMatch(txtKilometraje.Text) && !r5.IsMatch(txtKilometraje.Text))
                    {
                        MessageBox.Show("El formato del kilometraje ingresado es incorrecto".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtKilometraje.Focus();
                        txtKilometraje.Clear();
                    }
                }
            }
            catch
            {
                MessageBox.Show("El formato del kilometraje ingresado es incorrecto".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKilometraje.Focus();
                txtKilometraje.Clear();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cmbMeses.Enabled = !(dtpFechaDe.Enabled = dtpFechaA.Enabled = true);
                cmbMeses.SelectedIndex = 0;
            }
            else
                cmbMeses.Enabled = !(dtpFechaA.Enabled = dtpFechaDe.Enabled = false);
        }
        private void txtKilometraje_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.numerosDecimales(e);
            char signo_decimal = (char)46;
            if (e.KeyChar == 46)
                if (txtKilometraje.Text.LastIndexOf(signo_decimal) >= 0)
                    e.Handled = true; // Interceptamos la pulsación para no permitirla.
        }
        private void txtSupervisor_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSupervisor.Text))
            {
                //Validación de la contraseña del supervisor, 
                if (Convert.ToInt32(v.getaData("SELECT count(*) from cpersonal as t1  inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal=t1.idPersona inner join cempresas as t4 on t4.idempresa=t1.empresa where t3.password='" + v.Encriptar(txtSupervisor.Text) + "'and t1.status='1' and t2.status='1' and t1.empresa='1' and t1.area='1'")) > 0)
                {
                    //en caso correcto mostramos su nombre en en label 
                    idsupervisor = Convert.ToInt32(v.getaData("select t1.idpersona from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where password='" + v.Encriptar(txtSupervisor.Text) + "';").ToString());
                    lblSupervisor.Text = ((pinsertar || peditar) ? v.getaData("select concat(appaterno,' ',apmaterno,' ',nombres) from cpersonal where idpersona='" + idsupervisor + "';").ToString() : "");
                }
                else
                {
                    //En caso contrario no mostramos nada
                    idsupervisor = 0;
                    lblSupervisor.Text = "";
                }
            }
        }
        private void txtConductor_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtConductor.Text))
            {
                string[] datos = v.getaData("select UPPER(concat(t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres,'|',idpersona)) from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.credencial='" + txtConductor.Text + "' AND t1.status='1'   AND t2.status='1' and t1.empresa='1' and t1.area='1';").ToString().Split('|');
                lblCredCond.Text = (!string.IsNullOrWhiteSpace(datos[0]) ? datos[0] : "");
                idconductor = (!string.IsNullOrWhiteSpace(datos[1]) ? Convert.ToInt32(datos[1]) : 0);
            }
        }
        private void dtpFechaA_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
        private void dataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.DgvTabla.Columns[e.ColumnIndex].Name == "TIPO DE FALLO")
                e.CellStyle.BackColor = (Convert.ToString(e.Value) == "PREVENTIVO" ? Color.PaleGreen : Convert.ToString(e.Value) == "CORRECTIVO" ? Color.Khaki : Convert.ToString(e.Value) == "REITERATIVO" ? Color.LightCoral : Convert.ToString(e.Value) == "REPROGRAMADO" ? Color.LightBlue : Color.FromArgb(246, 144, 123));
            if (this.DgvTabla.Columns[e.ColumnIndex].Name == "ESTATUS")
                e.CellStyle.BackColor = (Convert.ToString(e.Value) == "EN PROCESO" ? Color.Khaki : Convert.ToString(e.Value) == "LIBERADA" ? Color.PaleGreen : e.Value.ToString() == "REPROGRAMADA" ? Color.LightCoral : Color.FromArgb(200, 200, 200));
        }
    }
}