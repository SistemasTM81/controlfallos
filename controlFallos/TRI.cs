using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using h = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Reflection;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;

namespace controlFallos
{
    public partial class TRI : Form
    {
        int idUsuario, idusuarioAnterior, empresa, area, idFolioFacturaSeleccionada = 0, indexAnteriork = 0, conteoListBoxAnt = 0; public Thread hilo, th;
        DataTable dtList = new DataTable();
        bool res1 = true, xkList = false;
        //string consulta_gral = "SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT  t2.Folio AS 'Folio', concat(t4.identificador,LPAD(consecutivo,4,'0')) AS 'Unidad' ,(SELECT UPPER(DATE_FORMAT(t1.FechaHoraI,'%W %d de %M del %Y'))) AS 'Fecha De Solicitud', (SELECT UPPER(CONCAT(coalesce(x1.ApPaterno,''),' ',coalesce(x1.ApMaterno,''),' ',coalesce(x1.nombres,''))) FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)AS 'Mecánico Que Solicita',COALESCE((SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento),'') AS 'Folio De Factura' ,COALESCE((SELECT UPPER(DATE_FORMAT( x4.FechaEntrega,'%W %d de %M del %Y')) FROM reportetri AS x4 WHERE t1.FoliofkSupervicion=x4.idreportemfkreportemantenimiento),'')AS 'Fecha De Entrega',COALESCE((SELECT UPPER(CONCAT(coalesce(x5.ApPaterno,''),' ',coalesce(x5.ApMaterno,''),' ',coalesce(x5.nombres,''))) FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento),'') AS 'Persona Que Entrego Refacción',COALESCE((SELECT UPPER(x7.ObservacionesTrans) FROM reportetri as x7 WHERE  t1.FoliofkSupervicion=x7.idreportemfkreportemantenimiento),'') AS 'Observaciones De Almacen' FROM reportemantenimiento AS t1 INNER JOIN reportesupervicion AS t2 ON t1.FoliofkSupervicion=t2.idReporteSupervicion INNER JOIN cunidades AS t3 ON t2.UnidadfkCUnidades=t3.idunidad inner join careas as t4 on t4.idarea=t3.areafkcareas";
        string consulta_gral = "SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT  t2.Folio AS 'Folio', concat(t4.identificador,LPAD(consecutivo,4,'0')) AS 'Unidad' ,(SELECT UPPER(DATE_FORMAT(t1.FechaHoraI,'%W %d de %M del %Y'))) AS 'Fecha De Solicitud',(SELECT UPPER(CONCAT(coalesce(x1.ApPaterno,''),' ',coalesce(x1.ApMaterno,''),' ',coalesce(x1.nombres,''))) FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)AS 'Mecánico Que Solicita',COALESCE((SELECT UPPER(DATE_FORMAT( x4.FechaEntrega,'%W %d de %M del %Y')) FROM reportetri AS x4 WHERE t1.FoliofkSupervicion=x4.idreportemfkreportemantenimiento),'')AS 'Fecha De Entrega',COALESCE((SELECT UPPER(CONCAT(coalesce(x5.ApPaterno,''),' ',coalesce(x5.ApMaterno,''),' ',coalesce(x5.nombres,''))) FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento),'') AS 'Persona Que Entrego Refacción',COALESCE((SELECT UPPER(x7.ObservacionesTrans) FROM reportetri as x7 WHERE  t1.IdReporte= x7.idreportemfkreportemantenimiento),'') AS 'Observaciones De Almacen' FROM reportemantenimiento AS t1 INNER JOIN reportesupervicion AS t2 ON t1.FoliofkSupervicion=t2.idReporteSupervicion INNER JOIN cunidades AS t3 ON t2.UnidadfkCUnidades=t3.idunidad inner join careas as t4 on t4.idarea= t3.areafkcareas";

         string folioAnterior, Observacionesanterior;
        public TRI(int idUsuario, int empresa, int area, System.Drawing.Image logo, validaciones v)
        {
            this.v = v;
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            cmbPersonaEmtrego.DrawItem += v.combos_DrawItem;
            cmbMecanicoSolicito.DrawItem += v.combos_DrawItem;
            cmbBuscarUnidad.DrawItem += v.combos_DrawItem;
            cmbMes.DrawItem += v.combos_DrawItem;
            cmbBuscarUnidad.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            cmbMes.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            cmbMecanicoSolicito.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            cmbPersonaEmtrego.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            pictureBox1.BackgroundImage = logo;
            this.idUsuario = idUsuario;
            lbltitulo.Left = (this.Width - lbltitulo.Width) / 2;
        }
        Thread exportar;
        validaciones v;
        void quitarseen()
        {
            while (res1)
            {
                MySqlConnection dbcon;
                if (v.c.conexionOriginal())
                {
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                    //else
                    //    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                    dbcon.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET seenAlmacen = 1 WHERE seenAlmacen  = 0", dbcon);
                    cmd.ExecuteNonQuery();
                    dbcon.Close();
                }
                Thread.Sleep(180000);
            }
        }
        string cont, obser_t, fol_f, per_d;
        bool bandera_e = false, bandera_c = false, bandera_editar = false, B_Doble = false, editar = false, mensaje = false;
        void cmbBuscarUnidad_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }
        bool getboolfromint(int i)
        {
            return i == 1;
        }
        public string mayusculas(string texto)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(texto);
        }
        public void privilegios()
        {
            string sql = "SELECT  privilegios FROM privilegios where usuariofkcpersonal='" + idUsuario + "' and namform='Almacen'";
            string[] privilegios = getaData(sql).ToString().Split('/');
            pinsertar = getboolfromint(Convert.ToInt32(privilegios[0]));
            pconsultar = getboolfromint(Convert.ToInt32(privilegios[1]));
            peditar = getboolfromint(Convert.ToInt32(privilegios[2]));
            if (privilegios.Length > 3)
            {
                pdesactivar = getboolfromint(Convert.ToInt32(privilegios[3]));
            }
        }
        public object getaData(string sql)
        {
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            var res = cm.ExecuteScalar();
            v.c.dbconection().Close();
            return res;
        }
        void Limpiar_v()
        {
            cont = obser_t = fol_f = per_d = "";
        }
        public void CargarDatos()// Metodo para cargar los reportes de la base de datos al datagridview y poder mostrarlos
        {
            /*  MySqlDataAdapter cargar = new MySqlDataAdapter(consulta_gral + " WHERE t1.StatusRefacciones='1' and(date_format(t1.FechaHoraI,'%Y-%m-%d') BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY))AND  curdate()) and t1.empresa='" + empresa + "' ORDER BY t1.FechaHoraI DESC, t2.folio desc;", v.c.dbconection());
              v.c.dbcon.Close();*/
            DataSet ds = new DataSet();
            MySqlDataAdapter cargar = (MySqlDataAdapter)v.getReport(consulta_gral + " WHERE t1.StatusRefacciones='1' and(date_format(t1.FechaHoraI,'%Y-%m-%d') BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY))AND  curdate()) and t1.empresa='" + empresa + "' ORDER BY t1.FechaHoraI DESC, t2.folio desc;");
            cargar.Fill(ds);
            tbReportes.DataSource = ds.Tables[0];
            tbReportes.ClearSelection();
        }
        public void Persona_entrego()
        {
            v.iniCombos("SELECT DISTINCT  idpersona,UPPER(CONCAT(coalesce(t2.ApPaterno,''),' ',coalesce(t2.ApMaterno,''),' ',coalesce(t2.nombres,''))) AS NOMBRE FROM reportetri as t1 INNER JOIN cpersonal as t2 On t1.PersonaEntregafkcPersonal = t2.idpersona where t2.empresa='" + empresa + "' group by PersonaEntregafkcPersonal;", cmbPersonaEmtrego, "idpersona", "NOMBRE", "-- SELECCIONE UN ALMACENISTA --");

        }
        public void Mecanico_solicito()
        {
            v.iniCombos("SELECT DISTINCT t2.idPersona,UPPER(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,''))) AS Nombre FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.MecanicofkPersonal=t2.idpersona where t2.empresa='" + empresa + "' GROUP BY MecanicofkPersonal ORDER BY CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')) asc;", cmbMecanicoSolicito, "idPersona", "Nombre", "-- seleccione un MECáNICO --");
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });

        }
        public void AutocompletadoFolioReporte(TextBox CajaDeTexto)//Metodo para autocompletado de "Folio de porte" en caja de etxto para buscar por folio de reporte
        {
            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
            string consulta = @"Select t1.Folio As folio ,t1.idReporteSupervicion from reportesupervicion as t1 Inner join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion WHERE t2.StatusRefacciones='Se Requieren Refacciones' and t2.empresa='" + empresa + "' ";
            MySqlCommand cmd = new MySqlCommand(consulta, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            v.c.dbconection().Close();
            if (dr.HasRows == true)
            {
                while (dr.Read())
                    namesCollection.Add(dr["folio"].ToString());
            }
            txtFolioDe.AutoCompleteMode = AutoCompleteMode.Suggest;//tipo de autocompletado
            txtFolioDe.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtFolioDe.AutoCompleteCustomSource = namesCollection;
            txtFolioA.AutoCompleteMode = AutoCompleteMode.Suggest;//tipo de autocompletado
            txtFolioA.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtFolioA.AutoCompleteCustomSource = namesCollection;
        }

        public void cargarUnidad()//Metodo para mostrar las unidades registardas en la base de datos en el comboBox para buscar reporte por uniadad
        {
            v.iniCombos("SELECT distinct t2.idunidad ,concat(t3.identificador, LPAD(t2.consecutivo, 4, '0')) as eco  FROM reportesupervicion as t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idUnidad  INNER JOIN careas as t3 On t2.areafkcareas = t3.idarea  GROUP BY  t1.UnidadfkCUnidades ORDER BY concat(t3.identificador, LPAD(t2.consecutivo, 4, '0')) ASC ", cmbBuscarUnidad, "idunidad", "eco", "-- seleccione económico --");
        }
        void ocultar_botones()
        {
            btnPdf.Visible = LblPDF.Visible = btnEditarReg.Visible = LblEditarR.Visible = false;
        }
        void inhanilitar_campos()
        {
            txtFolioFactura.Enabled = txtDispenso.Enabled = txtObservacionesT.Enabled = false;
        }
        void habilitar()
        {
            txtFolioFactura.Enabled = txtDispenso.Enabled = txtObservacionesT.Enabled = true;
        }
        //insert into 
        public void cargaList()
        {
            if (!string.IsNullOrWhiteSpace(lblidreporte.Text))
            {
                dtList = v.obtenData("select concat('Folio: ', t1.Folio,' Item: ', t1.items) as text from foliosfacturas as t1 inner join reportemantenimiento as t2 on t1. Reportesupfkrepmantenimiento = t2.foliofksupervicion where t1.clasif!='mant' and reportesupfkrepmantenimiento='" + lblidreporte.Text + "'");
                LBxRefacc.DataSource = v.iniList("select t1.idFolioFac as id, concat('Folio: ', t1.Folio,' Item: ', t1.items) as text from foliosfacturas as t1 inner join reportemantenimiento as t2 on t1. Reportesupfkrepmantenimiento = t2.foliofksupervicion where t1.clasif!='mant' and reportesupfkrepmantenimiento='" + lblidreporte.Text + "'");
                conteoListBoxAnt = LBxRefacc.Items.Count;
            }
        }

        private void TransInsumos_Load(object sender, EventArgs e)
        {
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();
            inhanilitar_campos();
            btnGuardar.Enabled = dtpFechaDe.Enabled = dtpFechaA.Enabled = false;
            lblFechaEntrega.Text = DateTime.Now.ToLongDateString().ToUpper();

            dtpFechaDe.MinDate = dtpFechaA.MinDate = Convert.ToDateTime((v.getaData("select min(FechaHoraI) from reportemantenimiento where StatusRefacciones='1' ") != DBNull.Value) ? v.getaData("select min(FechaHoraI) from reportemantenimiento where StatusRefacciones='1' ") : DateTime.Today);

            dtpFechaDe.MaxDate = dtpFechaA.MaxDate = Convert.ToDateTime((v.getaData("select max(FechaHoraI) from reportemantenimiento where StatusRefacciones='1'") != DBNull.Value) ? v.getaData("select max(FechaHoraI) from reportemantenimiento where StatusRefacciones='1'") : DateTime.Today);
            cargarUnidad(); // cargamos las unidades en el comboBox de busqueda por unidad
            Persona_entrego();
            Mecanico_solicito();
            AutocompletadoFolioReporte(txtFolioDe);
            AutocompletadoFolioReporte(txtFolioA);
            CargarDatos(); // llmamos al metodo para cargas los reportes al data
            Mostrar();
            lbltitulo.Left = (this.Width - lbltitulo.Width) / 2;
            tbReportes.ClearSelection();
            ocultar_botones();
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
        //ORIGINAL
         void Mostrar()
         {
             privilegios();
             if (pinsertar)
             {
                 GpbAlmacen.Visible = LblNota.Visible = LblNota1.Visible = tbReportes.Visible = tbRefacciones.Visible = true;
                 tbRefacciones.Size = new Size(592, 360);
                 tbReportes.Size = new Size(1905, 479);
             }
             if (pinsertar && !peditar && !pconsultar)
             {
                 LblNota.Location = new Point(700, 348);
                 LblNota1.Location = new Point(720, 348);
                 LblNota1.Text = "DEBE SELECCIONAR UN REPORTE DE LA TABLA PARA LLENAR LOS DATOS";
             }
             if (peditar)
                 tbReportes.Visible = tbRefacciones.Visible = LblNota.Visible = LblNota1.Visible = btnEditarReg.Visible = LblEditarR.Visible = true;
             if (pconsultar)
             {
                 tbReportes.Visible = tbRefacciones.Visible = GpbBusquedas.Visible = LblNota.Visible = LblNota1.Visible = true;
                 tbRefacciones.Size = new Size(592, 342);
                 tbReportes.Size = new Size(1307, 479);
             }
             if (pconsultar && !pinsertar && !peditar)
             {
                 LblNota.Location = new Point(600, 348);
                 LblNota1.Location = new Point(646, 348);
                 LblNota1.Text = "PARA CONSULTAR LA INFORMACIÓN DE DOBLE CLIC SOBRE EL REPORTE EN LA TABLA.";
             }
             if (peditar && pinsertar && pconsultar)
             {
                 LblPDF.Visible = btnPdf.Visible = true;
                 LblNota1.Text = "PARA CONSULTAR O EDITAR LA INFORMACIÓN DE DOBLE CLIC SOBRE EL REPORTE EN LA TABLA.";
             }
         }
       
        public void LimpiarBusqueda()//Metodo para limpiar todos los campos que se encuentrane en la sección de busqueda.
        {
            txtBuscFolio.Clear();
            cmbMecanicoSolicito.SelectedIndex = cmbBuscarUnidad.SelectedIndex = cmbPersonaEmtrego.SelectedIndex = cmbMes.SelectedIndex = 0;
            txtFolioDe.Clear();
            txtFolioA.Clear();
            //dtpFechaDe.Value = dtpFechaA.Value = dtpFechaDe.MaxDate;
        }
        void realiza_busquedas()
        {
            if (checkBox1.Checked == true || !string.IsNullOrWhiteSpace(txtBuscFolio.Text) || cmbPersonaEmtrego.SelectedIndex > 0 || cmbMecanicoSolicito.SelectedIndex > 0 || cmbMes.SelectedIndex > 0 || cmbBuscarUnidad.SelectedIndex > 0 || (!string.IsNullOrWhiteSpace(txtFolioDe.Text) && !string.IsNullOrWhiteSpace(txtFolioA.Text)))
            {
                //Verificar si el chechBox esta seleccionado para realizar busqueda por rango de fechas
                if ((dtpFechaA.Value.Date < dtpFechaDe.Value.Date || dtpFechaA.Value.Date > DateTime.Now) && checkBox1.Checked) //Validar que las fechas seleccionadas sean correctas, que la fecha 1 no sea mayor a la fecha 2
                {
                    MessageBox.Show("Las fechas seleccionadas son incorrectas".ToUpper(), "VERIFICAR FECHAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpFechaDe.Value = DateTime.Now;
                    dtpFechaA.ResetText();
                }
                else
                {
                    string wheres = "";
                    if (checkBox1.Checked)
                        wheres = (wheres == "" ? " Where date_format(t1.FechaHoraI,'%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'" : wheres += " AND date_format(t1.FechaHoraI,'%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'");
                    if (!string.IsNullOrWhiteSpace(txtBuscFolio.Text))
                        wheres = (wheres == "" ? " where (SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento)='" + txtBuscFolio.Text + "'" : wheres += " and (SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento)='" + txtBuscFolio.Text + "'");
                    if (cmbPersonaEmtrego.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where (SELECT x5.idpersona FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento)='" + cmbPersonaEmtrego.SelectedValue + "'" : wheres += " AND (SELECT x5.idpersona FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento)='" + cmbPersonaEmtrego.SelectedValue + "'");
                    if (cmbMecanicoSolicito.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where(SELECT x1.idpersona FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)='" + cmbMecanicoSolicito.SelectedValue + "'" : wheres += " AND (SELECT x1.idpersona FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)='" + cmbMecanicoSolicito.SelectedValue + "'");
                    if (cmbBuscarUnidad.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'" : wheres += " And t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'");
                    if (!string.IsNullOrWhiteSpace(txtFolioDe.Text) && !string.IsNullOrWhiteSpace(txtFolioA.Text))
                    {
                        int longitud = 0;
                        longitud = txtFolioA.MaxLength;
                        wheres = (wheres == "" ? " where substring(t2.folio,-" + longitud + "," + longitud + ") between " + Convert.ToInt32(txtFolioDe.Text) + " and " + Convert.ToInt32(txtFolioA.Text) + "" : wheres += " and substring(t2.folio,-" + longitud + "," + longitud + ")between " + Convert.ToInt32(txtFolioDe.Text) + " and " + Convert.ToInt32(txtFolioA.Text) + "");
                    }
                    if (cmbMes.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where (select Date_format(t1.FechaHoraI,'%W %d %M %Y') like '%" + cmbMes.Text + "%' and (select year(t1.FechaHoraI))=( select year(now())))" : wheres += " AND (select Date_format(t1.FechaHoraI,'%W %d %M %Y') like '%" + cmbMes.Text + "%' and (select year(t1.FechaHoraI))=( select year(now())))");
                    if (wheres != "")
                        wheres += " and t1.StatusRefacciones=1 and t1.empresa='" + empresa + "' and (select year(t1.FechaHoraI))=( select year(now())) order by t2.folio desc";
                    MySqlDataAdapter DTA = new MySqlDataAdapter(consulta_gral + wheres, v.c.dbconection());
                    v.c.dbconection().Close();
                    DataSet ds = new DataSet();
                    DTA.Fill(ds);
                    tbReportes.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)// si no existen reportes en el datagridview mandamos un mensaje
                    {
                        MessageBox.Show("No se encontraron reportes".ToUpper(), "NINGÚN REPORTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CargarDatos();
                        btnActualizar.Visible = lblactualizar.Visible = false;
                    }
                    else
                    {
                        if (pinsertar || peditar || pconsultar)
                        {
                            LblExcel.Visible = true;
                            if (!est_expor)
                            {
                                btnExcel.Visible = true;
                            }
                        }
                        btnActualizar.Visible = lblactualizar.Visible = true;
                    }
                    checkBox1.Checked = false;
                    LimpiarBusqueda();//LLamamos al metodo LimpiarBusqueda.
                }
            }
            else
            {
                //Mandamos mensaje en caso de que se encuentren vacios los campos
                MessageBox.Show("Seleccione un criterio de búsqueda".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFolioDe.Text) && string.IsNullOrWhiteSpace(txtFolioA.Text))
            {
                MessageBox.Show("El campo \" Folio a \" se encuentra vacio en el apartado rango de folios".ToUpper(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFolioA.Focus();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtFolioA.Text) && string.IsNullOrWhiteSpace(txtFolioDe.Text))
                {
                    MessageBox.Show("EL campo \" Folio de \" se encuentra vacio en el apartado en rango de folios".ToUpper(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFolioDe.Focus();
                }
                else
                    realiza_busquedas();
            }
        }
        public void LimpiarReporteTri()//Metodo para limpiar los campos de la parte de reporte
        {
            lblId.Text = lblUnidad.Text = lblFechaSolicitud.Text = lblMecanicoSolicita.Text = lblFechaEntrega.Text = lblPersonaDis.Text = lblFolio.Text = "";
            txtFolioFactura.Clear();
            txtObservacionesT.Clear();
            txtDispenso.Clear();
            btnPdf.Enabled = btnEditarReg.Enabled = LblGuardar.Visible = btnGuardar.Visible = true;
            ocultar_botones();
            res = editar = mensaje = btnValidar.Visible = B_Doble = bandera_editar = bandera_c = btnGuardar.Enabled = bandera_e = false;
            LblGuardar.Text = "GUARDAR";
            inhanilitar_campos();
            e = null;
            statusDeMantenimiento = null;
            tbRefacciones.Rows.Clear();
            if (pinsertar && peditar && pconsultar)
            {
                LblExcel.Visible = false;
                btnExcel.Visible = false;
            }
            Persona_entrego();
        }

        public void Nuevas_Refacciones()
        {
            MySqlCommand leerid = new MySqlCommand("SELECT t1.idReporteTransinsumos FROM reportetri as t1 inner join reportesupervicion as t2 on t2.idreportesupervicion=t1.idreportemfkreportemantenimiento where t2.folio='" + lblFolio.Text + "' and t1.empresa='" + empresa + "' and t1.idReporteTransinsumos=t1.idReporteTransinsumos", v.c.dbconection());
            MySqlDataReader dr = leerid.ExecuteReader();
            if (dr.Read())
            {
                //En caso de que si este guardado el reporte solamente guardamos el estatus y la cantidad de nuevas refacciones solicitadas.
                MySqlCommand agregar = new MySqlCommand(@"UPDATE pedidosrefaccion AS t1 INNER JOIN crefacciones AS t2 ON t1.RefaccionfkCRefaccion = t2.idrefaccion SET t1.EstatusRefaccion = @EstatusRefaccion, t1.CantidadEntregada = (SELECT IF(t1.cantidadentregada < t1.Cantidad,(SELECT IF(t2.existencias > (t1.Cantidad - t1.CantidadEntregada),(t1.CantidadEntregada + (t1.Cantidad - t1.CantidadEntregada)),(t1.CantidadEntregada + t2.existencias))),(SELECT IF(t1.cantidad > t2.existencias,t2.existencias,t1.cantidad)))),t2.existencias = @existencias WHERE idPedRef = @idPedRef AND(t1.EstatusRefaccion IS NULL OR T1.CantidadEntregada = 0 OR t1.cantidadentregada < t1.cantidad OR T1.CantidadEntregada = ''); ", v.c.dbconection());
                foreach (DataGridViewRow row in tbRefacciones.Rows)// hacemos un ciclo repititivo para ir guardando el estatus de cada refacción solicitada
                {
                    double existencias = Double.Parse(row.Cells[4].Value.ToString());
                    double faltante = Double.Parse(row.Cells[7].Value.ToString());
                    double cantidadSolicitada = Double.Parse(row.Cells[3].Value.ToString());
                    double cantidadentregada = Double.Parse(row.Cells[6].Value.ToString());
                    if (cantidadentregada < cantidadSolicitada)
                    {
                        if (existencias < (cantidadSolicitada - cantidadentregada))
                        {
                            existencias = 0;
                        }
                        else
                        {
                            existencias -= (cantidadSolicitada - cantidadentregada);
                        }
                    }
                    else
                    {
                        if (cantidadSolicitada > existencias)
                        {
                            existencias = 0;
                        }
                        else
                        {
                            existencias -= cantidadSolicitada;
                        }
                    }
                    agregar.Parameters.Clear();
                    agregar.Parameters.AddWithValue("@EstatusRefaccion", Convert.ToString(row.Cells[5].Value));
                    agregar.Parameters.AddWithValue("@idPedRef", Convert.ToString(row.Cells[0].Value));
                    agregar.Parameters.AddWithValue("@existencias", existencias);
                    agregar.ExecuteNonQuery();
                }
                //Nuevas_Refacc(lblFolio.Text);
                MySqlCommand sql = new MySqlCommand("update estatusvalidado set seen =1 where idreportefkreportesupervicion='" + lblidreporte.Text + "' ", v.c.dbconection());
                sql.ExecuteNonQuery();
                if (!edita_valida)
                {
                    MessageBox.Show("Se agregaron las refacciones satisfactoriamente ".ToUpper() + DateTime.Now.ToLongDateString().ToUpper() + " " + DateTime.Now.ToLongTimeString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                    limFolioFact();
                    ocultar_botones();
                    LimpiarReporteTri();
                }
                v.c.dbconection().Close();
            }
            dr.Close();
        }

        public void GuardarRegistro()
        {
            try
            {
                //    //En caso contrario guardamos folio de factura, fecha, persona que dispenso y guardamos el estatus y cantidad entregada de cada una de las refacciones solicitadas.
                MySqlCommand agregar = new MySqlCommand("update pedidosrefaccion as t1 inner join crefacciones as t2 on t1.RefaccionfkCRefaccion=t2.idrefaccion set t1.EstatusRefaccion=@EstatusRefaccion,t1.CantidadEntregada=(select if(t1.cantidad>t2.existencias,t2.existencias,t1.cantidad)),t2.existencias=(Select if(T1.cantidad>t2.existencias,(t2.existencias-t2.existencias),(t2.existencias-t1.cantidad))) Where idPedRef=@idPedRef   ", v.c.dbconection());
                foreach (DataGridViewRow row in tbRefacciones.Rows)// hacemos un ciclo repititivo para ir guardando el estatus de cada refacción solicitada
                {
                    agregar.Parameters.Clear();
                    agregar.Parameters.AddWithValue("@EstatusRefaccion", Convert.ToString(row.Cells[5].Value));
                    agregar.Parameters.AddWithValue("@idPedRef", Convert.ToString(row.Cells[0].Value));
                    agregar.ExecuteNonQuery();
                    v.c.dbconection().Close();
                }
                MySqlCommand sql = new MySqlCommand("insert into estatusvalidado(idreportefkreportesupervicion) values ('" + lblidreporte.Text + "')", v.c.dbconection());
                sql.ExecuteNonQuery();

                // consulta para insertar los datos a la base de datos
                MySqlCommand guardar = new MySqlCommand("insert into reportetri (idreportemfkreportemantenimiento,FechaEntrega,PersonaEntregafkcPersonal,ObservacionesTrans,empresa) VALUES ('" + Convert.ToInt32(lblidreporte.Text) + "', curdate(), '" + Convert.ToInt32(IdDispenso) + "','" + txtObservacionesT.Text.Trim() + "','" + empresa + "') ;", v.c.dbconection());
                guardar.ExecuteNonQuery();
                v.c.dbconection().Close();
                MessageBox.Show("Registro guardado con exito ".ToUpper() + DateTime.Now.ToLongDateString().ToUpper() + " " + DateTime.Now.ToLongTimeString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Modificacion_Crear(lblFolio.Text);
                //insertar_refacciones(lblFolio.Text);
                LimpiarReporteTri();
                limFolioFact();
                CargarDatos();
                lblidreporte.Text = "";

                //}
            }
            catch (Exception ex) //excepción en caso de que no se pueda guardar el reporte
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string Can_s, Can_e, Can_f, existen;

        void Modificacion_Crear(string folio)
        {
            string f = txtObservacionesT.Text;
            if (f == "") f = null;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Reporte de Almacen',(select idReporteTransinsumos from reportetri as t1 inner join reportesupervicion as t2 on t1.idreportemfkreportemantenimiento=t2.idreportesupervicion and t2.folio='" + folio + "' and t1.empresa='" + empresa + "'),CONCAT('" + txtFolioFactura.Text + ";',DATE(NOW()),';" + Convert.ToString(IdDispenso) + ";" + (f ?? " SIN OBSERVACIONES") + "'),'" + '1' + "',NOW(),'Inserción de reporte de almacén','2','2')";
            MySqlCommand modificaciones_inserciones = new MySqlCommand(sql, v.c.dbconection());
            modificaciones_inserciones.ExecuteNonQuery();
        }
        string existenciasR, fecha;
        void boton_guardar()
        {
            try
            {
                //comente la linea 4323, campo : folio, camposAlmacen();
                //if (v.camposAlmacen(txtFolioFactura.Text, txtDispenso.Text, lblFechaEntrega.Text, empresa, area, idrepor))
                if (v.camposAlmacen(txtDispenso.Text, lblFechaEntrega.Text, empresa, area, idrepor))
                {
                    if (tbRefacciones.Rows.Count == 0)
                    {
                        //Si no hay refacciones para validar en el reporte no se puede guardar
                        MessageBox.Show("No se puede guardar el reporte, porque no hay refacciones para validar".ToUpper(), "SIN REFACCIONES SOLICITADAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LimpiarReporteTri();
                    }
                    else
                    {
                        if (tbRefacciones.Rows.Count == 1)
                        {
                            MySqlCommand estatusR = new MySqlCommand("SET lc_time_names = 'es_ES';select t3.existencias,(SELECT DATE_FORMAT(t3.proximoAbastecimiento,'%W %d de %M del %Y')) as fecha from pedidosrefaccion as t1 inner join reportesupervicion as t2 on t1.FolioPedfkSupervicion=t2.idreportesupervicion inner join crefacciones as t3  on t1.RefaccionfkCRefaccion=t3.idrefaccion where t1.FolioPedfkSupervicion='" + lblidreporte.Text + "' ", v.c.dbconection());
                            MySqlDataReader DR1 = estatusR.ExecuteReader();
                            if (DR1.Read())
                            {
                                existenciasR = Convert.ToString(DR1["existencias"]);
                                fecha = Convert.ToString(DR1["fecha"]);

                                if (Convert.ToDouble(existenciasR) == 0.0)
                                {
                                    //si se solicita una sola refaccion y no tiene existencia mandamos mensaje de alerta
                                    MessageBox.Show("No se puede guardar el registro, por que la cantidad en existencias de la refacción se encuentra en 0. \n \n El próximo reabastecimiento es el día:  " + fecha, "SIN EXISTENCIAS DE REFACCIÓN".ToUpper(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    LimpiarReporteTri();
                                }
                                else
                                {
                                    if (LBxRefacc.Items.Count > 0)
                                    {
                                        GuardarRegistro();//Mandamos llamar el metodo GuardarRegisro.
                                        LimpiarReporteTri();
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se puede guardar el registro, Requiere folio de factura", "SIN FOLIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                            DR1.Close();
                        }
                        else
                        {
                            if (LBxRefacc.Items.Count > 0)
                            {
                                GuardarRegistro();//Mandamos llamar el metodo GuardarRegisro.
                                LimpiarReporteTri();
                                btnGuardar.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("No se puede actualizar, Requiere folio de factura", "SIN FOLIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void TextoLargo(object sender, EventArgs e)
        {
            ((Label)sender).Font = (((Label)sender).Text.Length >= 30 ? new System.Drawing.Font("Garamond", 10) : new System.Drawing.Font("Garamond", 12));
        }
        private void btnGuardar_Click(object sender, EventArgs e)//Evento para guardar nuestro reporte en la base de datos.
        {
            boton_guardar();
        }

        private void txtFolioFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);//Llamamos al metodo validar número para que solo se permitan ingresar numeros en la caja de texto
        }

        private void txtObservacionesT_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validamos que solo se permitan ingresar letras, espacio, puntos y comas en este campo
            v.enGeneral(e);
        }

        private void txtBuscFolio_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);//Llamamos al metodo ValidarNumero
        }
        string IdDispenso;

        private void txtDispenso_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumerossinespacios(e);
        }
        public void CargarRefacciones()
        {
            LBxRefacc.DataSource = null;
            conteoListBoxAnt = 0;
            tbRefacciones.ClearSelection();
            //consulta para obtener el id del reporte de supervisión
            MySqlCommand cmd = new MySqlCommand("Select t1.idreportesupervicion as id  from reportesupervicion as t1 inner join reportemantenimiento as t2 on t2.FoliofkSupervicion=t1.idreportesupervicion Where t1.Folio='" + lblFolio.Text + "'", v.c.dbconection());
            MySqlDataReader dr1 = cmd.ExecuteReader();
            v.c.dbconection().Close();
            if (dr1.Read())
            {
                lblidreporte.Text = ((Convert.ToString(dr1["id"])));
                cargaList();
            }
            else
            {
                lblidreporte.Text = "";
            }
            dr1.Close();
        }
        string idrepor;
        bool nuevo_reporte = false;
        string statusDeMantenimiento = "";
        public void restaurar_datos(DataGridViewCellEventArgs e)
        {
            tbRefacciones.Rows.Clear();
            tbRefacciones.Columns.Clear();
            nuevo_reporte = false;
            if (tbReportes.Rows.Count > 0)
            {
                lblFolio.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[0].Value);
                lblUnidad.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[1].Value);
                lblFechaSolicitud.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[2].Value);
                lblMecanicoSolicita.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[3].Value);
                //
                statusDeMantenimiento = v.getaData("SELECT t1.Estatus FROM reportemantenimiento as t1 inner join reportesupervicion as t2 on t1.FoliofkSupervicion=t2.idReporteSupervicion where t2.folio='" + lblFolio.Text + "' and t1.empresa='" + empresa + "'").ToString() ?? "SIN ESTATUS";
                MySqlCommand existencia = new MySqlCommand("Select T1.idreportemfkreportemantenimiento from reportetri as T1 inner join reportesupervicion as t2 on t2.idreportesupervicion=T1.idreportemfkreportemantenimiento where t2.folio='" + lblFolio.Text + "'", v.c.dbconection());
                MySqlDataReader dtr1 = existencia.ExecuteReader();
                if (dtr1.Read())
                {
                    idrepor = getaData("SELECT idReporteTransinsumos FROM REPORTETRI AS T1 INNER JOIN REPORTESUPERVICION AS T2 ON T2.IDREPORTESUPERVICION=T1.idreportemfkreportemantenimiento WHERE T2.FOLIO='" + lblFolio.Text + "' and t1.empresa='" + empresa + "'").ToString();
                    nuevo_reporte = true;
                }
                //Pasamos los datos del datagridview a labels, textbox,comboBox
                if (bandera_e && !bandera_c)
                {
                    LBxRefacc.DataSource = null;
                    btnGuardar.Visible = false;
                    LblGuardar.Visible = false;
                    btnPdf.Visible = true;
                    LblPDF.Visible = true;
                    
                    if ((Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[4].Value) == ""))
                    {
                        lblFechaEntrega.Text = DateTime.Now.ToLongDateString().ToUpper();
                    }
                    else
                    {
                        lblFechaEntrega.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[4].Value).ToUpper();
                    }
                    //txtFolioFactura.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[4].Value);
                    lblPersonaDis.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[5].Value);
                    txtObservacionesT.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[6].Value);
                    per_d = tbReportes.Rows[e.RowIndex].Cells[5].Value.ToString().Trim();
                    obser_t = tbReportes.Rows[e.RowIndex].Cells[6].Value.ToString().Trim();
                    //fol_f = tbReportes.Rows[e.RowIndex].Cells[4].Value.ToString();
                    MySqlCommand cmdestatus = new MySqlCommand("Select UPPER(t1.Estatus) as Estatus from reportemantenimiento as t1 inner join reportesupervicion as t2 on t1.FoliofkSupervicion=t2.idreportesupervicion Where t2.Folio='" + lblFolio.Text + "' and t1.empresa='" + empresa + "' ", v.c.dbconection());
                    MySqlDataReader dtr = cmdestatus.ExecuteReader();
                    string status;
                    if (dtr.Read())
                    {
                        status = Convert.ToString(dtr["Estatus"]);
                        if (status == "LIBERADA")
                        {
                            //puesto_usuario = getaData("SELECT UPPER(T1.puesto) AS 'puesto' FROM PUESTOS AS T1 INNER JOIN CPERSONAL AS T2 ON T2.cargofkcargos=T1.idpuesto WHERE idPersona='1';").ToString();    
                            //puesto_usuario = "ADMINISTRADOR";
                            if (pinsertar && pconsultar && peditar)
                            {
                                habilitar();
                            }
                            else
                            {
                                btnValidar.Visible = false;
                                txtFolioFactura.Enabled = false;
                                btnEditarReg.Visible = false;
                                LblEditarR.Visible = false;
                                txtObservacionesT.Enabled = false;
                                txtDispenso.Enabled = false;
                            }
                            tbRefacciones.Columns.Add("idpedref", "ID PEDIDO REFACCION");
                            tbRefacciones.Columns[0].Visible = false;
                            tbRefacciones.Columns.Add("codref", "CÓDIGO DE REFACCIÓN");
                            tbRefacciones.Columns.Add("nomref", "NOMBRE DE REFACCIÓN");
                            tbRefacciones.Columns.Add("cantsol", "CANTIDAD SOLICITADA");
                            tbRefacciones.Columns.Add("cantentre", "CANTIDAD ENTREGADA");
                            tbRefacciones.Columns.Add("status", "ESTATUS DE REFACCIÓN");
                            tbRefacciones.Columns.Add("cantfalta", "CANTIDAD FALTANTE");
                            tbRefacciones.Columns.Add("retorno", "RETORNO");
                            //conuslta para mostrar las refacciones solicitadas y saber su estatus
                            string sql = "select t1.idPedRef,UPPER(t2.codrefaccion) as 'CÓDIGO DE REFACCIÓN',UPPER(t2.nombreRefaccion) as 'NOMBRE DE REFACIÓN',sum(t1.Cantidad) as 'CANTIDAD SOLICITADA',sum(t1.CantidadEntregada) as 'CANTIDAD ENTREGADA', UPPER(t1.EstatusRefaccion) as 'ESTATUS DE REFACCIÓN' ,Coalesce(t1.cantidad-t1.CantidadEntregada,t1.cantidad) as 'CANTIDAD FALTANTE', UPPER((select if(envio='0', '', if(seen='0', 'Ver', if(AutorizaAlmacen ='0', 'Evaluando', if(AutorizaAlmacen ='1', 'Correcto', 'Incorrecto')))) from refacciones_standby as x1 where t1.idpedRef = x1.refaccionfkpedidosRefaccion)) as retorno from pedidosrefaccion as t1 inner join crefacciones as t2 on t1.RefaccionfkCRefaccion=t2.idrefaccion inner join reportesupervicion as t3 on t1.FolioPedfkSupervicion=t3.idreportesupervicion where t3.folio='" + lblFolio.Text + "' group by(t2.codrefaccion)";

                            DataTable dt = (DataTable)v.getData(sql);
                            int numFilas = dt.Rows.Count;
                            for (int i = 0; i < numFilas; i++)
                            {
                                tbRefacciones.Rows.Add(dt.Rows[i].ItemArray);
                            }
                            tbRefacciones.Visible = true;
                            v.c.dbconection().Close();
                        }
                        else
                        {
                            tbRefacciones.Columns.Add("idpedref", "ID PEDIDO REFACCION");
                            tbRefacciones.Columns[0].Visible = false;
                            tbRefacciones.Columns.Add("codref", "CÓDIGO DE REFACCIÓN");
                            tbRefacciones.Columns.Add("nomref", "NOMBRE DE REFACCIÓN");
                            tbRefacciones.Columns.Add("cantsol", "CANTIDAD SOLICITADA");
                            tbRefacciones.Columns.Add("cantenEXIST", "CANTIDAD EN EXISTENCIAS");
                            tbRefacciones.Columns.Add("status", "ESTATUS DE REFACCIÓN");
                            tbRefacciones.Columns.Add("cantentre", "CANTIDAD ENTREGADA");
                            tbRefacciones.Columns.Add("cantfalta", "CANTIDAD FALTANTE");
                            tbRefacciones.Columns.Add("retorno", "RETORNO");
                            tbRefacciones.DataSource = null;
                            //conuslta para mostrar las refacciones solicitadas y saber su estatus
                            string sql = "SELECT T1.idPedRef, UPPER(T3.codrefaccion) AS 'CÓDIGO DE REFACCIÓN',UPPER(t3.nombreRefaccion)as 'NOMBRE DE REFACCIÓN',T1.Cantidad As 'CANTIDAD SOLICITADA',(select if(T3.existencias<0,'0',T3.existencias)) as 'CANTIDAD EN EXISTENCIAS' ,(if(t1.Cantidad=t1.CantidadEntregada,'EXISTENCIA',if(t3.existencias>0 && t3.existencias<(t1.Cantidad-t1.CantidadEntregada),'INCOMPLETO',if(t3.existencias>=(t1.Cantidad-t1.CantidadEntregada),'EXISTENCIA',t1.EstatusRefaccion)))) as 'ESTATUS DE REFACCIÓN',COALESCE(T1.CantidadEntregada,'0') as 'CANTIDAD ENTREGADA',(T1.Cantidad-T1.CantidadEntregada) as 'CANTIDAD FALTANTE', UPPER((select if(envio='0', '', if(seen='0', 'Ver', if(AutorizaAlmacen ='0', 'Evaluando', if(AutorizaAlmacen ='1', 'Correcto', 'Incorrecto')))) from refacciones_standby as x1 where t1.idpedRef = x1.refaccionfkpedidosRefaccion)) as retorno FROM pedidosrefaccion AS T1  INNER JOIN crefacciones AS T3 ON T1.RefaccionfkCRefaccion=T3.idrefaccion INNER JOIN reportesupervicion AS T4 ON t4.idReporteSupervicion=T1.FolioPedfkSupervicion WHERE t4.Folio='" + lblFolio.Text + "' and (t1.EstatusRefaccion is not null)";
                            DataTable dt = (DataTable)v.getData(sql);
                            int numFilas = dt.Rows.Count;
                            for (int i = 0; i < numFilas; i++)
                            {
                                tbRefacciones.Rows.Add(dt.Rows[i].ItemArray);
                            }
                            tbRefacciones.Visible = true;
                            //conuslta para mostrar las refacciones solicitadas y saber su estatus
                            string sql1 = "SELECT T1.idPedRef,UPPER(T3.codrefaccion) AS 'CÓDIGO DE REFACCIÓN',UPPER(t3.nombreRefaccion) as 'NOMBRE DE REFACCIÓN',T1.Cantidad As 'CANTIDAD SOLICITADA',(select if(T3.existencias<0,'0',T3.existencias)) as 'CANTIDAD EN EXISTENCIAS' ,(select if(T3.existencias<=0,'SIN EXISTENCIA',(if(t3.existencias >0 && t3.existencias < t1.cantidad,'INCOMPLETO','EXISTENCIA')) )) as 'ESTATUS DE REFACCIÓN',COALESCE(T1.CantidadEntregada,'0') as 'CANTIDAD ENTREGADA',(Select if(T1.cantidad>t3.existencias,(T1.cantidad-t3.existencias),(t1.cantidad-t1.cantidad))) as 'CANTIDAD FALTANTE', UPPER((select if(envio='0', '', if(seen='0', 'VER', if(AutorizaAlmacen ='0', 'Evaluando', if(AutorizaAlmacen ='1', 'Correcto', 'Incorrecto')))) from refacciones_standby as x1 where t1.idpedRef = x1.refaccionfkpedidosRefaccion)) as retorno FROM pedidosrefaccion AS T1  INNER JOIN crefacciones AS T3 ON T1.RefaccionfkCRefaccion=T3.idrefaccion INNER JOIN reportesupervicion AS T4 ON t4.idReporteSupervicion=T1.FolioPedfkSupervicion WHERE t4.Folio='" + lblFolio.Text + "' and (T1.EstatusRefaccion is null  )";
                            DataTable dt1 = (DataTable)v.getData(sql1);
                            int numFilas1 = dt1.Rows.Count;
                            for (int i = 0; i < numFilas1; i++)
                            {
                                tbRefacciones.Rows.Add(dt1.Rows[i].ItemArray);
                            }
                            tbRefacciones.Visible = txtObservacionesT.Enabled = txtFolioFactura.Enabled = txtDispenso.Enabled = true;
                            if (!nuevo_reporte)
                            {
                                btnValidar.Visible = false;
                                btnGuardar.Enabled = true;
                                btnGuardar.Visible = true;
                                LblGuardar.Visible = true;
                                btnEditarReg.Visible = false;
                                LblEditarR.Visible = false;
                                btnPdf.Visible = false;
                                LblPDF.Visible = false;
                                LblGuardar.Text = "GUARDAR";
                            }
                            else
                            {
                                /*0 =  'Folio', 
                     *1 =  'Unidad' ,
                     *2 = 'Fecha De Solicitud', 
                     *3 = 'Mecánico Que Solicita',
                     *4 = 'Folio De Factura' ,
                     *5 = 'Fecha De Entrega',
                     *6 = 'Persona Que Entrego Refacción',
                     *7 = 'Observaciones De Almacen';*/
                                foreach (DataGridViewRow row in tbRefacciones.Rows)
                                {
                                    Can_s = row.Cells[3].Value.ToString();
                                    Can_e = row.Cells[6].Value.ToString();
                                    Can_f = row.Cells[7].Value.ToString();
                                    existen = row.Cells[4].Value.ToString();
                                    if (Can_e != Can_s && (Convert.ToInt32(existen) > 0))
                                    {
                                        res = true;
                                    }
                                }
                            }
                            if (res)
                            {
                                btnValidar.Visible = true;
                                LblGuardar.Text = "VALIDAR \n REFACCIONES";
                                LblGuardar.Visible = true;
                            }
                            else
                            {
                                btnValidar.Visible = false;
                            }
                        }
                    }
                    dtr.Close();
                    CargarRefacciones();

                    //COnsulta para obtener nombre y contraseña del almacenista
                    MySqlCommand cmd1 = new MySqlCommand("select t1.password,t2.nombres, t2.idpersona as id from datosistema AS t1 inner join cpersonal AS t2 on t2.idpersona=t1.usuariofkcpersonal  where concat(t2.Appaterno,' ',t2.ApMaterno,' ',t2.nombres)='" + lblPersonaDis.Text + "' and t2.empresa='" + empresa + "'", v.c.dbconection());
                    MySqlDataReader dr2 = cmd1.ExecuteReader();
                    if (dr2.Read())
                    {
                        txtDispenso.Text = v.Desencriptar(Convert.ToString(dr2["password"]));
                        IdDispenso = Convert.ToString(dr2["id"]);
                    }
                    else
                    {
                        txtDispenso.Text = "";
                        IdDispenso = "";
                    }
                    v.c.dbconection().Close();
                    dr2.Close();
                }
                v.c.insertar("update refacciones_standby as t1 inner join pedidosrefaccion as t2 on t1.refaccionfkpedidosRefaccion = t2.idpedref inner join reportesupervicion as t3 on t2.folioPedfkSupervicion=t3.idreportesupervicion set t1.seen = '1' where t3.Folio = '" + lblFolio.Text + "'");
            }
            idFolioFacturaSeleccionada = 0;
            //limFolioFact();
            txtFolioFactura.Text = ""; numUpDownDE.Value = numUpDownHASTA.Value = idFolioFacturaSeleccionada = indexAnteriork = 0; xkList = false;
        }
        string id, folio_f, fecha_d, pers_d, obser, est;
        void verifica_modificaciones()
        {
            DialogResult respuesta;//LBxRefacc
            //if (!string.IsNullOrWhiteSpace(txtFolioFactura.Text) || !string.IsNullOrWhiteSpace(txtDispenso.Text))
            if ((LBxRefacc.Items.Count > 0) || !string.IsNullOrWhiteSpace(txtDispenso.Text))
            {
                //MySqlCommand modificaciones = new MySqlCommand("SET lc_time_names = 'es_ES';SELECT T1.IDREPORTETRANSINSUMOS AS ID, T1.FolioFactura AS FOLIO,upper(Date_format(T1.FechaEntrega,'%W %d de %M del %Y')) AS FECHA,(SELECT upper(CONCAT(coalesce(X1.ApPaterno,''),' ',coalesce(X1.ApMaterno,''),' ',coalesce(X1.nombres,''))) FROM cpersonal AS X1 WHERE X1.idPersona=T1.PersonaEntregafkcPersonal) AS DISPENSO ,UPPER(T1.ObservacionesTrans) AS OBSERVACIONES ,T3.Estatus AS ESTATUS FROM reportetri AS T1 INNER JOIN reportesupervicion AS T2 ON T2.IDREPORTESUPERVICION=T1.idreportemfkreportemantenimiento INNER JOIN REPORTEMANTENIMIENTO AS T3 ON T2.IDREPORTESUPERVICION=T3.FoliofkSupervicion WHERE T2.FOLIO='" + lblFolio.Text + "' and t1.empresa='" + empresa + "';", v.c.dbconection());
                MySqlCommand modificaciones = new MySqlCommand("SET lc_time_names = 'es_ES';SELECT T1.IDREPORTETRANSINSUMOS AS ID,upper(Date_format(T1.FechaEntrega,'%W %d de %M del %Y')) AS FECHA,(SELECT upper(CONCAT(coalesce(X1.ApPaterno,''),' ',coalesce(X1.ApMaterno,''),' ',coalesce(X1.nombres,''))) FROM cpersonal AS X1 WHERE X1.idPersona=T1.PersonaEntregafkcPersonal) AS DISPENSO ,UPPER(T1.ObservacionesTrans) AS OBSERVACIONES ,T3.Estatus AS ESTATUS FROM reportetri AS T1 INNER JOIN reportesupervicion AS T2 ON T2.IDREPORTESUPERVICION=T1.idreportemfkreportemantenimiento INNER JOIN REPORTEMANTENIMIENTO AS T3 ON T2.IDREPORTESUPERVICION=T3.FoliofkSupervicion WHERE T2.FOLIO='" + lblFolio.Text + "' and t1.empresa='" + empresa + "';", v.c.dbconection());
                MySqlDataReader Dr = modificaciones.ExecuteReader();
                if (Dr.Read())
                {
                    id = Convert.ToString(Dr["ID"]);
                    //folio_f = Convert.ToString(Dr["FOLIO"]);
                    fecha_d = Convert.ToString(Dr["FECHA"]);
                    pers_d = Convert.ToString(Dr["DISPENSO"]);
                    obser = Convert.ToString(Dr["OBSERVACIONES"]);
                    est = Dr["ESTATUS"].ToString();

                    //if ((folio_f != txtFolioFactura.Text || fecha_d != lblFechaEntrega.Text || pers_d != lblPersonaDis.Text || obser != txtObservacionesT.Text) && ((est != "LIBERADA") || (est == "LIBERADA" && (pinsertar && pconsultar && peditar))))
                    if ((fecha_d != lblFechaEntrega.Text || pers_d != lblPersonaDis.Text || obser != txtObservacionesT.Text) && ((est != "LIBERADA") || (est == "LIBERADA" && (pinsertar && pconsultar && peditar))))
                    {
                        respuesta = MessageBox.Show("¿Desea guardar las modificaciones?".ToUpper(), "ALERTA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (respuesta == DialogResult.Yes)
                        {
                            mensaje = true;
                            B_Doble = true;
                            boton_edita();
                            bandera_editar = true;
                        }
                        else
                        {
                            restaurar_datos(e);
                        }
                    }
                }
                else
                {
                    respuesta = MessageBox.Show("¿Deseas concluir el reporte?".ToUpper(), "ALERTA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {
                        boton_guardar();
                        bandera_editar = true;
                    }
                    else
                    {
                        editar = false;
                        restaurar_datos(e);
                    }
                }
            }
        }



        DataGridViewCellEventArgs e = null;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    limFolioFact();
                    habilitar();
                    bandera_e = true;
                    editar = false;
                    this.e = e;
                    bandera_editar = false;
                    if (peditar)
                    {
                        verifica_modificaciones();
                    }
                    if (!bandera_editar)
                    {
                        restaurar_datos(e);
                        editar = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string Folio, Id_R;
        delegate void El_Delegado();
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "EXPORTANDO";
        }
        delegate void El_Delegado1();
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "EXPORTAR";
            if (exportando)
                LblExcel.Visible = btnExcel.Visible = false;
            exportando = est_expor = false;
        }
        public void ExportarExcel()//Metodo para exportar datos de datagridview a Excel.
        {
            /*
                        if (tbReportes.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = (DataTable)tbReportes.DataSource;
                            if (this.InvokeRequired)
                            {
                                El_Delegado delega = new El_Delegado(cargando);
                                this.Invoke(delega);
                            }
                            Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();

                            X.Application.Workbooks.Add(Type.Missing);
                            X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            h.Worksheet sheet = X.ActiveSheet;
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
                                        rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230));
                                        rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                        rng.Cells.Font.Name = "Calibri";
                                        rng.Cells.Font.Size = 11;
                                        rng.Font.Bold = false;
                                    }
                                    catch (System.NullReferenceException ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }
                                }
                            }
                            Thread.Sleep(500);
                            X.Columns.AutoFit();
                            X.Rows.AutoFit();
                            X.Visible = true;
                            if (this.InvokeRequired)
                            {
                                El_Delegado1 delega = new El_Delegado1(cargando1);
                                this.Invoke(delega);
                            }
                        }
                        else
                            MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
            */


            //Pruebas 

            //Empezar a usar excel
            SLDocument sl = new SLDocument();

            //Importar imagen

            // System.Drawing.Bitmap bm = new System.Drawing.Bitmap(@"C:\Users\Ing. Osky Lopez\Documents\Pruebas\controlfallos\controlFallos\Resources\logo.png");
            //byte[] ba = null;


            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
            // ba = Convert.FromBase64String(v.trainsumos);
            // bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //ms.Close();
            //ba = ms.ToArray();
            // }
            /*
                         byte[] ba = null;

                         var res = v.getaData("SELECT COALESCE(logo,'') FROM cempresas WHERE idempresa='3'").ToString();

                         if (res == "")
                         {
                             if (empresa == 2)
                                 ba = Convert.FromBase64String(v.tri);

                             else if (empresa == 3)
                                 ba = Convert.FromBase64String(v.trainsumos);

                         }
                         else
                         {
                             System.Drawing.Image temp = v.StringToImage2(res);
                             temp = v.CambiarTamanoImagen(temp, 50, 50);
                             ba = Convert.FromBase64String(v.SerializarImg(temp));
                         }

                         SLPicture pic = new SLPicture(ba, DocumentFormat.OpenXml.Packaging.ImagePartType.Png);
                         pic.SetPosition(0, 0);
                         pic.ResizeInPixels(400, 250);
                         sl.InsertPicture(pic);
                         //Importar imagen
            */


            //Para saber en que celda iniciar
            int celdaCabecera = 8, celdaInicial = 8;

            int ic = 2;
            foreach (DataGridViewColumn column in tbReportes.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;


            }



            int ir = 9;
            foreach (DataGridViewRow row in tbReportes.Rows)
            {

                sl.SetCellValue(ir, 2, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[5].Value.ToString());
                sl.SetCellValue(ir, 8, row.Cells[6].Value.ToString());
                




                ir++;
                celdaInicial++;

            }

            //Formato Estatus
            /*
                        if (dataGridViewOCompra.Rows.ToString() == "En Espera")
                        {
                            ////pendiente

                            SLStyle estiloEs = sl.CreateStyle();
                            estiloEs.Font.FontColor = System.Drawing.Color.White;
                            estiloEs.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
                            sl.SetCellStyle("I" + celdaCabecera, "I" + celdaCabecera, estiloEs);
                                celdaCabecera++;

                        }
                        else if (dataGridViewOCompra.Rows.ToString() == "Entregada")
                        {

                            SLStyle estiloE = sl.CreateStyle();

                            estiloE.Font.FontColor = System.Drawing.Color.White;
                            estiloE.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Green, System.Drawing.Color.Green);
                            sl.SetCellStyle("I" + celdaInicial, "I" + celdaInicial, estiloE);

                            celdaInicial++;
                        }
            */


            //if (this.dataGridViewOCompra.Columns[e.ColumnIndex].Name == "Estatus")
            // e.CellStyle.BackColor = (e.Value.ToString() == "En Espera" ? System.Drawing.Color.Red : e.Value.ToString() == "Entregada" ? System.Drawing.Color.PaleGreen : System.Drawing.Color.LightBlue);

            //Formato Estatus

            //Nombre de la Hoja de Excel
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Reporte Almecen");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "H" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "CONSULTA REPORTE DE ALMACEN");
            SLStyle estiloT = sl.CreateStyle();
            estiloT.Font.FontName = "Arial";
            estiloT.Font.FontSize = 15;
            estiloT.Font.Bold = true;
            sl.SetCellStyle("D4", estiloT);
            sl.MergeWorksheetCells("D4", "E4");

            //Estilo Titulo

            //Estilos Para bordes de la tabla

            SLStyle EstiloB = sl.CreateStyle();

            EstiloB.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.LeftBorder.Color = System.Drawing.Color.Black;

            EstiloB.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            sl.SetCellStyle("B" + celdaInicial, "H" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "H");
            //Estilos Para bordes de la tabla

            //Extraer fecha

            sl.SetCellValue("F3", "FECHA/HORA DE CONSULTA:");
            SLStyle estiloF = sl.CreateStyle();
            estiloF.Font.FontName = "Arial";
            estiloF.Font.FontSize = 9;
            estiloF.Font.Bold = true;
            sl.SetCellStyle("F3", estiloF);
            sl.MergeWorksheetCells("F3", "G3");


            //Obtener Fecha


            DateTime fecha = DateTime.Now;

            sl.SetCellValue("H3", fecha.ToString());
            SLStyle fecha0 = sl.CreateStyle();
            fecha0.Font.FontName = "Arial";
            fecha0.Font.FontSize = 10;
            fecha0.Font.Bold = true;
            sl.SetCellStyle("H3", fecha0);

            sl.SetCellValue("F4", "RANGO CONSULTA DE:");
            SLStyle estiloF3 = sl.CreateStyle();
            estiloF3.Font.FontName = "Arial";
            estiloF3.Font.FontSize = 9;
            estiloF3.Font.Bold = true;
            sl.SetCellStyle("F4", estiloF3);
            sl.MergeWorksheetCells("F4", "G4");

            sl.SetCellValue("F5", "RANGO CONSULTA A:");
            SLStyle estiloF2 = sl.CreateStyle();
            estiloF2.Font.FontName = "Arial";
            estiloF2.Font.FontSize = 9;
            estiloF2.Font.Bold = true;
            sl.SetCellStyle("F5", estiloF2);
            sl.MergeWorksheetCells("F5", "G5");


            var datestring3 = dtpFechaDe.Value.ToLongDateString();

            sl.SetCellValue("H4", datestring3);
            SLStyle fechaDe = sl.CreateStyle();
            fechaDe.Font.FontName = "Arial";
            fechaDe.Font.FontSize = 10;
            fechaDe.Font.Bold = true;
            sl.SetCellStyle("H4", fechaDe);

            var datestring2 = dtpFechaA.Value.ToLongDateString();

            sl.SetCellValue("H5", datestring2);
            SLStyle fechaA = sl.CreateStyle();
            fechaA.Font.FontName = "Arial";
            fechaA.Font.FontSize = 10;
            fechaA.Font.Bold = true;
            sl.SetCellStyle("H5", fechaA);

            //Obtener Fecha

            //Extraer fecha


            //Directorio para Guardar el Excel

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "GUARDAR ARCHIVO";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "*.xlsx";
            saveFileDialog1.Filter = "Archivos de Excel (*.xlsx)|*.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sl.SaveAs(saveFileDialog1.FileName);
                    MessageBox.Show("   **ARCHIVO EXPORTADO CON EXITO**  ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "   **NO SE GUARGO EL ARCHIVO**   ");
                }
            }
            //Directorio para Guardar el Excel

        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            /*
                        est_expor = true;
                        ThreadStart delegado = new ThreadStart(ExportarExcel);
                        exportar = new Thread(delegado);
                        exportar.Start();
            */
            ExportarExcel();
        }
        //*********************************Animación de Botones************************************
        private void TRI_FormClosing(object sender, FormClosingEventArgs e)
        {
            hilo.Abort();
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }
        private void tbRefacciones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (statusDeMantenimiento == "1")
            {
                try
                {
                    if (tbRefacciones.Rows.Count > 1)
                    {
                        bool res = false;
                        for (int i = e.RowIndex; i < tbRefacciones.Rows.Count; i++)
                        {
                            if (tbRefacciones.Rows[i].Cells[1].Value.ToString().Equals(tbRefacciones.Rows[e.RowIndex].Cells[1].Value.ToString()))
                            {
                                double exist = !string.IsNullOrWhiteSpace(tbRefacciones.Rows[e.RowIndex].Cells[4].Value.ToString()) ? Convert.ToDouble(tbRefacciones.Rows[e.RowIndex].Cells[4].Value) : 0;
                                double cs = !string.IsNullOrWhiteSpace(tbRefacciones.Rows[e.RowIndex].Cells[3].Value.ToString()) ? Convert.ToDouble(tbRefacciones.Rows[e.RowIndex].Cells[3].Value) : 0;
                                double ce = !string.IsNullOrWhiteSpace(tbRefacciones.Rows[e.RowIndex].Cells[6].Value.ToString()) ? Convert.ToDouble(tbRefacciones.Rows[e.RowIndex].Cells[6].Value) : 0;
                                for (int j = 0; j < i; j++)
                                {
                                    res = Convert.ToDouble(tbRefacciones.Rows[i].Cells[6].Value) == 0;
                                    if (tbRefacciones.Rows[i].Cells[1].Value.ToString().Equals(tbRefacciones.Rows[j].Cells[1].Value.ToString()))
                                    {
                                        if ((Convert.ToDouble(tbRefacciones.Rows[j].Cells[6].Value) == 0) || (Convert.ToDouble(tbRefacciones.Rows[j].Cells[6].Value) > 0 && Convert.ToDouble(tbRefacciones.Rows[j].Cells[6].Value) < Convert.ToDouble(tbRefacciones.Rows[j].Cells[3].Value)))
                                        {
                                            exist = Convert.ToDouble(tbRefacciones.Rows[j].Cells[4].Value) - (Convert.ToDouble(tbRefacciones.Rows[j].Cells[3].Value) - Convert.ToDouble(tbRefacciones.Rows[j].Cells[6].Value));

                                            if (exist <= 0)
                                            {
                                                exist = 0;
                                            }
                                        }
                                        else
                                        {
                                            exist = Convert.ToDouble(tbRefacciones.Rows[e.RowIndex].Cells[4].Value);
                                        }
                                        if (cs == ce)
                                        {
                                            tbRefacciones.Rows[e.RowIndex].Cells[7].Value = 0;
                                        }
                                        else
                                        {
                                            tbRefacciones.Rows[e.RowIndex].Cells[7].Value = cs - ce;
                                        }
                                        if (exist == 0 && ce == 0)
                                        {
                                            tbRefacciones.Rows[e.RowIndex].Cells[5].Value = "SIN EXISTENCIA";
                                        }
                                        else
                                        {
                                            if (cs == ce)
                                            {
                                                tbRefacciones.Rows[e.RowIndex].Cells[5].Value = "EXISTENCIA";
                                            }
                                            else
                                            {
                                                if (exist >= 0 && exist < cs && ce == 0)
                                                {
                                                    tbRefacciones.Rows[e.RowIndex].Cells[5].Value = "INCOMPLETO";
                                                }
                                                else
                                                {
                                                    if (exist > 0 && exist >= (cs - ce))
                                                    {
                                                        tbRefacciones.Rows[e.RowIndex].Cells[5].Value = "EXISTENCIA";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (res)
                                {
                                    tbRefacciones.Rows[e.RowIndex].Cells[4].Value = exist;
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
        }

        private void txtObservacionesT_Validating_1(object sender, CancelEventArgs e)
        {
            while (txtObservacionesT.Text.Contains("  "))
            {
                txtObservacionesT.Text = txtObservacionesT.Text.Replace("  ", " ").Trim();
                txtObservacionesT.SelectionStart = txtObservacionesT.TextLength + 1;
            }
        }


        private void tbRefacciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 44 || e.KeyChar == 46 || e.KeyChar == 127 || e.KeyChar == 08 || e.KeyChar == 32 || Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("SOLO SE ACEPTAN LETRAS, NUMEROS   ,   Y    .   ", "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnValidar_MouseMove(object sender, MouseEventArgs e)
        {
            btnValidar.Size = new Size(69, 66);
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            Nuevas_Refacciones();
        }

        private void tbRefacciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string idRefk = "", Refaccion = "", codigo = "", cantReal = "";
            if (e.RowIndex >= 0)
            {
                //select t2.Estatus from reportetri as t1 inner join reportemantenimiento as t2 on t1.idreportemfkreportemantenimiento=t2.foliofksupervicion where t1.idreportemfkreportemantenimiento='51'
                if (v.getaData("select t2.Estatus from reportemantenimiento as t2 where t2.FoliofkSupervicion ='" + lblidreporte.Text + "'").ToString() != "3")
                {
                    // if (!string.IsNullOrWhiteSpace(tbRefacciones.Rows[e.RowIndex].Cells[8].Value.ToString()))
                    //{
                    codigo = tbRefacciones.Rows[e.RowIndex].Cells[1].Value.ToString();
                        Refaccion = tbRefacciones.Rows[e.RowIndex].Cells[2].Value.ToString();
                        cantReal = tbRefacciones.Rows[e.RowIndex].Cells[3].Value.ToString();
                    //RetornoMant = v.getaData("select cantRetorno from pedidosrefaccion as t1 inner join refacciones_standby as t2 on t1.idpedref = t2.refaccionfkpedidosrefaccion where t1.idpedref='" + idRefk + "' order by idStanby asc;").ToString();
                    Retorno_de_material O = new Retorno_de_material(idUsuario, empresa, area, v, Refaccion, codigo, lblUnidad.Text,cantReal,lblFolio.Text);
                        //RetornoAlmacen o = new RetornoAlmacen(empresa, area, v, Refaccion,  cantReal, Convert.ToInt32(idRefk));
                        O.Owner = this;
                        if (O.ShowDialog() == DialogResult.OK)
                        {
                            /*string responde = "", estatus = "", cantidad = "", observaciones = "", motivo = "";
                            responde = o.id;
                            estatus = o.cmbEstatus.SelectedIndex.ToString();
                            cantidad = o.txtcantidad.Text;
                            observaciones = o.txtObservacion.Text;
                            motivo = o.txtMotivo.Text;
                            v.c.insertar("update refacciones_standby set AutorizaAlmacen='" + estatus + "', FechaHoraR=now(), ObservacionAlm='" + observaciones + "', UsuarioR='" + responde + "', cantRetornoAlm='" + cantidad + "' where refaccionfkpedidosRefaccion='" + idRefk + "'");

                            string cadenaModificacion = cantidad + ";" + estatus + ";" + responde + ";" + observaciones + ";";
                            v.c.insertar("insert into modificaciones_sistema(form, idregistro, ultimamodificacion, Tipo, MotivoActualizacion, empresa, Area, usuariofkcpersonal, fechaHora) values('Retorno de Refacciones', '" + idRefk + "','" + cadenaModificacion + "','Confirmar Retorno','" + motivo + "','" + empresa + "','" + area + "','" + idUsuario + "',now())");*/
                        }
                   /* }
                    else
                    {
                        MessageBox.Show("Sin Estatus de Retorno", "¡Acción no Realizada!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }*/
                }
                else
                {
                    MessageBox.Show("¡El reporte ha finalizado!", "Acción no Realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LBxRefacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (xkList)
            {
                //if (LBxRefacc.Items.Count > 0)
                if (LBxRefacc.SelectedIndex != -1)
                {
                    if (LBxRefacc.Items.Count > 0 && LBxRefacc.DataSource != null)
                    {
                        if (!string.IsNullOrWhiteSpace(LBxRefacc.SelectedItem.ToString()))
                        {
                            indexAnteriork = LBxRefacc.SelectedIndex;
                            string item = LBxRefacc.SelectedItem.ToString();
                            string[] slocal = item.Split(new char[] { ':', ' ', '-' });
                            txtFolioFactura.Text = slocal[2].Trim();
                            numUpDownDE.Value = Convert.ToInt32(slocal[5].Trim());
                            numUpDownHASTA.Value = Convert.ToInt32(slocal[6].Trim());
                            idFolioFacturaSeleccionada = Convert.ToInt32(v.getaData("SELECT t1.idFolioFac FROM foliosfacturas as t1 inner join reportemantenimiento as t2 on t1. Reportesupfkrepmantenimiento = t2.foliofksupervicion where t1.reportesupfkrepmantenimiento ='" + lblidreporte.Text + "' and t1.Folio='" + txtFolioFactura.Text + "' and t1.items='" + numUpDownDE.Value + "-" + numUpDownHASTA.Value + "';"));
                        }
                    }
                }
            }
            xkList = true;
        }

        private void numUpDownHASTA_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDownHASTA.Value > tbRefacciones.RowCount)
            {
                numUpDownHASTA.Value = 0;
            }
            habilitaGuardado();
        }

        private void numUpDownDE_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDownDE.Value > numUpDownHASTA.Value && numUpDownDE.Value > tbRefacciones.RowCount)
            {
                numUpDownDE.Value = 0;
            }
            habilitaGuardado();
        }

        private void LBxRefacc_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, System.Drawing.Color.White, System.Drawing.Color.Crimson);
                e.DrawBackground();
                e.Graphics.DrawString(LBxRefacc.Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
            else
            {
                e.DrawBackground();
                e.Graphics.DrawString(LBxRefacc.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        private void btnFolioFactura_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFolioFactura.Text) && numUpDownDE.Value > 0 && numUpDownHASTA.Value > 0 && (numUpDownDE.Value <= numUpDownHASTA.Value))
            {
                compruebaDobles();
            }
            else
            {
                MessageBox.Show("Campos obligatorios, faltantes".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void compruebaDobles()
        {
            string nuevaCadena = "Folio: " + txtFolioFactura.Text + " Item: " + numUpDownDE.Value + "-" + numUpDownHASTA.Value;
            int index = LBxRefacc.FindString(nuevaCadena);
            if (index != -1)
            {
                if (idFolioFacturaSeleccionada == 0)
                {
                    MessageBox.Show("Acción Inválida".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (verificaLitbox())
                    {
                        //CHECAR
                        enviaRegistro(1);
                        MessageBox.Show("Información actualizada con éxito".ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (index == indexAnteriork)
                        {
                            enviaRegistro(1);
                            MessageBox.Show("Información actualizada con éxito".ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Acción Inválida, verifica la información".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    //comparar primero con actualización
                }
            }
            else
            {
                if (idFolioFacturaSeleccionada == 0)
                {
                    //registro
                    if (verificaLitbox())
                    {
                        enviaRegistro(0);
                        MessageBox.Show("Información guardada con éxito".ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Acción Inválida, verifica la información".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    //actualización
                    if (verificaLitbox())
                    {
                        //CHECAR
                        if (index == indexAnteriork)
                        {
                            enviaRegistro(1);
                            MessageBox.Show("Información actualizada con éxito".ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Acción Inválida, verifica la información".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Acción Inválida, verifica la información".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }


        bool verificaLitbox()
        {
            string concatena = "";
            bool actualiza = true;
            int index = LBxRefacc.FindString(txtFolioFactura.Text);
            for (int i = 0; i < LBxRefacc.Items.Count; i++)
            {
                string[] slocal = LBxRefacc.Items[i].ToString().Split(new char[] { ':', ' ', '-' });
                int item = Convert.ToInt32(slocal[2].Trim()), RangoI = Convert.ToInt32(slocal[5].Trim()), RangoF = Convert.ToInt32(slocal[6].Trim());
                if (IsBetween(Convert.ToInt32(numUpDownDE.Value), Convert.ToInt32(numUpDownHASTA.Value), RangoI, RangoF) && !xkList)
                {
                    return false;
                }
                else if (IsBetween(Convert.ToInt32(numUpDownDE.Value), Convert.ToInt32(numUpDownHASTA.Value), RangoI, RangoF) && xkList)
                {
                    concatena += i + ",";
                }
            }
            actualiza = revisa(concatena);
            return actualiza;
        }

        public bool revisa(string concatena)
        {
            string[] arrayLocal; bool xf = true;
            if (!string.IsNullOrWhiteSpace(concatena))
            {
                arrayLocal = concatena.Split(',');
                xf = (arrayLocal.Count() >= 1) ? !string.IsNullOrWhiteSpace(arrayLocal[0]) ? false : Convert.ToInt32(arrayLocal[0]) == indexAnteriork && idFolioFacturaSeleccionada != 0 ? true : false : true;
            }
            return xf;
        }

        public static bool IsBetween(int IniciaA, int FinalizaA, int IniciaB, int FinalizaB)
        {
            return (IniciaA <= FinalizaB) && (FinalizaA >= IniciaB) && (IniciaA <= FinalizaB) && (IniciaB <= FinalizaA);
        }

        public void enviaRegistro(int accions)
        {
            //idrepor = idReporteTransinsumos
            string sql = (accions == 0) ? "Insert into foliosfacturas(Folio, items, reportesupfkrepmantenimiento, clasif) values('" + txtFolioFactura.Text + "', '" + numUpDownDE.Value + "-" + numUpDownHASTA.Value + "', '" + lblidreporte.Text + "', 'alm')" : "update foliosfacturas set folio='" + txtFolioFactura.Text + "', items='" + numUpDownDE.Value + "-" + numUpDownHASTA.Value + "' where idFolioFac='" + idFolioFacturaSeleccionada + "'";
            v.c.insertar(sql);
            string idmoment = (accions == 0) ? v.getaData("select idfoliofac from foliosfacturas where reportesupfkrepmantenimiento='" + lblidreporte.Text + "' and folio='" + txtFolioFactura.Text + "' and clasif!='mant' order by idfoliofac desc;").ToString() : idFolioFacturaSeleccionada.ToString();

            //modificacionFolios(idmoment, txtfoliof.Text + ";" + numUpDownDE.Value + "-" + numUpDownHASTA.Value + ";" + idreporte, "Alta de folio Factura", "");
            modificacionFolios(idmoment, txtFolioFactura.Text + ";" + numUpDownDE.Value + "-" + numUpDownHASTA.Value + ";" + lblidreporte.Text, ((accions == 0) ? "Alta" : "Actualización") + " de folio Factura Almacén", "");
            limFolioFact();
            LBxRefacc.DataSource = v.iniList("select t1.idFolioFac as id, concat('Folio: ', t1.Folio,' Item: ', t1.items) as text from foliosfacturas as t1 inner join reportemantenimiento as t2 on t1. Reportesupfkrepmantenimiento = t2.foliofksupervicion where t1.clasif!='mant' and reportesupfkrepmantenimiento='" + lblidreporte.Text + "'");
        }

        private void tbRefacciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.tbRefacciones.Columns[e.ColumnIndex].HeaderText.ToUpper() == "RETORNO")
                e.CellStyle.BackColor = (e.Value.ToString() == "VER" ? System.Drawing.Color.White : e.Value.ToString() == "EVALUANDO" ? System.Drawing.Color.Aquamarine : e.Value.ToString() == "CORRECTO" ? System.Drawing.Color.GreenYellow : e.Value.ToString() == "INCORRECTO" ? System.Drawing.Color.Red : System.Drawing.Color.LightSlateGray);
        }

        public void modificacionFolios(string idFolioFact, string ultimaModificacion, string tipo, string porquemodificacion)
        {
            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area, motivoActualizacion) VALUES('Folio de Factura Almacén', " + idFolioFact + ",'" + ultimaModificacion + "','" + idUsuario + "',NOW(),'" + tipo + "','" + empresa + "','" + area + "', '" + porquemodificacion + "')");
        }

        private void GpbAlmacen_Enter(object sender, EventArgs e)
        {

        }

        public int comparaAntesDespuesFolios()
        {
            //select count(idFolioFac) as id from foliosfacturas where clasif!='mant' and reportesupfkrepmantenimiento='' and folio!=''
            for (int i = 0; i < dtList.Rows.Count;)
            {
                for (int j = 0; j < LBxRefacc.Items.Count; j++)
                {
                    string dtlocal = dtList.Rows[i]["text"].ToString();
                    string listlocal = LBxRefacc.Items[i].ToString();
                    if (!string.IsNullOrWhiteSpace(dtlocal) && !string.IsNullOrWhiteSpace(listlocal))
                    {
                        if (dtlocal != listlocal)
                        {
                            return 1;
                        }
                    }
                    i++;
                }
            }
            return 0;
        }

        private void btnCancelFact_Click(object sender, EventArgs e)
        {
            //limFolioFact();
            txtFolioFactura.Text = ""; numUpDownDE.Value = numUpDownHASTA.Value = idFolioFacturaSeleccionada = indexAnteriork = 0; xkList = false;
        }

        void limFolioFact() { txtFolioFactura.Text = ""; numUpDownDE.Value = numUpDownHASTA.Value = idFolioFacturaSeleccionada = indexAnteriork = 0; xkList = false; LBxRefacc.DataSource = null; }
        public void Expota_PDF()
        {
            byte[] img;
            string[] datos = v.getaData("SET NAMES 'utf8';SET lc_time_names = 'es_ES';select upper(concat(coalesce(t1.Folio,''),'|',coalesce((select concat(x2.identificador,LPAD(consecutivo,4,'0')) from cunidades as x1 inner join careas as x2 on x2.idarea=x1.areafkcareas where x1.idunidad=t1.UnidadfkCUnidades),''),'|',date_format(t2.FechaHoraI,'%W %d de %M del %Y'),'|',coalesce((select concat(coalesce(x3.ApPaterno,''),' ',coalesce(x3.ApMaterno,' '),' ',coalesce(nombres,'')) from cpersonal as x3 where x3.idpersona=t2.MecanicofkPersonal),''),'|',date_format(FechaEntrega,'%W %d de %M del %Y'),'|',coalesce((select concat(coalesce(x4.ApPaterno,''),' ',coalesce(x4.ApMaterno,' '),' ',coalesce(x4.nombres,'')) from cpersonal as x4 where x4.idpersona=t3.PersonaEntregafkcPersonal),''),'|',coalesce(t3.ObservacionesTrans,''))) as r from reportesupervicion as t1 inner join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion inner join reportetri as t3 on t3.idreportemfkreportemantenimiento=t1.idReporteSupervicion where t1.Folio='" + lblFolio.Text + "' and t2.empresa='" + empresa + "'").ToString().Split('|');
            //Código para generación de archivo pdf
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(20f, 20f, 10f, 10f);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ValidateNames = true;
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Reporte";
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
                    else{
                        img = Convert.FromBase64String(v.trainsumos);
                    }
                     
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    Chunk chunk = new Chunk("REPORTE  ALMACEN", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    doc.Add(imagen);
                    doc.Add(new Paragraph(chunk));
                    doc.Add(new Paragraph("                                    "));
                    PdfPTable tabla = new PdfPTable(2);
                    tabla.DefaultCell.Border = 0;
                    tabla.WidthPercentage = 100;
                    /*
                     t1.Folio,'|',UNIDAD,'|',FECHA Y HORA,'|',MECANICO,'|',FECHAHORA ENTREGA,'|',PERSONA QUE ENTREGA,'|',FolioFactura,'|',ObservacionesTrans
                     */
                    tabla.AddCell(v.valorCampo("FOLIO DEL REPORTE", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("UNIDAD", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[0], 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(datos[1], 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("FECHA DE SOLICITUD", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("MECÁNICO QUE SOLICITA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[2], 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(datos[3], 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    //tabla.AddCell(v.valorCampo("FOLIO DE FACTURA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("FECHA DE ENTREGA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("PERSONA QUE ENTREGA", 2, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[4], 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(datos[5], 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("OBSERVACIONES", 2, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(datos[6], 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));

                    tabla.AddCell(v.valorCampo("FOLIOS DE FACTURA ", 2, 1, 0, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
                    //tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));

                    string olk = "FOLIO," +  v.getaData("select coalesce(group_concat(t1.folio),',') as Folio from foliosfacturas as t1 inner join reportemantenimiento as t2 on t1.reportesupfkrepmantenimiento= t2.foliofksupervicion inner join reportesupervicion as t3 on t3.idReporteSupervicion=t2.FoliofkSupervicion where t3.Folio='" + lblFolio.Text + "' and t2.empresa='" + empresa + "'").ToString();
                    string[] arryFolios = olk.Split(',');
                    PdfPTable datatable = new PdfPTable(1);
                    datatable.WidthPercentage = 100;
                    for (int qi = 0; qi < arryFolios.Length; qi++)
                    {
                        if (qi == 0)
                        {
                            datatable.AddCell(new Phrase("FOLIO", FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                        }
                        else
                        {
                            datatable.AddCell(new Phrase(arryFolios[qi], FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
                        }
                    }
                    datatable.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    


                    //tabla.AddCell(v.valorCampo("REFACCIONES SOLICITADAS", 2, 1, 0, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
                    //tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));
                    doc.Add(tabla);
                    doc.Add(datatable);
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

        private void btnPdf_Click(object sender, EventArgs e)
        {
            Expota_PDF();
        }
        private void txtObservacionesT_Validated(object sender, EventArgs e)
        {
            while (txtObservacionesT.Text.Contains("  "))
            {
                txtObservacionesT.Text = txtObservacionesT.Text.Replace("  ", " ").Trim();
                txtObservacionesT.SelectionStart = txtObservacionesT.TextLength + 1;
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


            PdfPTable datatable = new PdfPTable(tbRefacciones.ColumnCount);
            datatable.DefaultCell.Padding = 4;
            

            float[] headerwidths = GetTamañoColumnas(tbRefacciones);
            datatable.SetWidths(headerwidths);
            System.Drawing.Color color = System.Drawing.Color.PaleGreen;
            datatable.WidthPercentage = 100;
            PdfPCell observaciones = new PdfPCell();
            Phrase FechaE = new Phrase("Observaciones:");
            Phrase LFechaE = new Phrase(txtObservacionesT.Text);
            observaciones.AddElement(FechaE);
            observaciones.AddElement(LFechaE);
            datatable.DefaultCell.BorderWidth = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < tbRefacciones.ColumnCount; i++)
            {
                datatable.AddCell(new Phrase(tbRefacciones.Columns[i].HeaderText.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
            }
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(250, 250, 250);
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < tbRefacciones.RowCount; i++)
            {
                for (j = 0; j < tbRefacciones.ColumnCount; j++)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(tbRefacciones[j, i].Value.ToString(), FontFactory.GetFont("ARIAL", 8)));
                    celda.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                    if (j == 5 && tbRefacciones[j, i].Value.ToString() == "EXISTENCIA")
                        celda.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.PaleGreen);
                    else
                        celda.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightCoral);
                    if (tbRefacciones[j, i].Value != null)
                        datatable.AddCell(celda);

                }
                datatable.CompleteRow();
            }

            datatable.AddCell(observaciones);
            document.Add(tabla1);
            document.Add(datatable);
        }
        public float[] GetTamañoColumnas(DataGridView dg)
        {
            float[] values = new float[dg.ColumnCount];
            for (int i = 1; i < tbRefacciones.ColumnCount; i++)
            {
                values[i] = (float)dg.Columns[i].Width;
            }
            return values;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
                if (editar && nuevo_reporte && peditar)
                {
                   /* cont = v.Desencriptar(getaData("SELECT PASSWORD FROM DATOSISTEMA AS T1 INNER JOIN CPERSONAL AS T2 ON T2.IDPERSONA=T1.usuariofkcpersonal WHERE upper(concat(coalesce(APPATERNO,''),' ',coalesce(APMATERNO,''),' ',coalesce(NOMBRES,'')))='" + per_d + "'").ToString());*/
                    if ((obser_t != txtObservacionesT.Text.Trim() || cont != txtDispenso.Text) && (!string.IsNullOrWhiteSpace(txtDispenso.Text)) && comparaAntesDespuesFolios() == 0)
                    {
                        btnEditarReg.Visible = true;
                        LblEditarR.Visible = true;
                    }
                    else
                    {
                        btnEditarReg.Visible = false;
                        LblEditarR.Visible = false;
                    }
                    habilitaGuardado();
                }
        }

        void habilitaGuardado()
        {

            btnFolioFactura.Visible = !string.IsNullOrWhiteSpace(txtFolioFactura.Text) && numUpDownDE.Value > 0 && numUpDownHASTA.Value > 0 ? true : false;
        }

        bool exportando = false, est_expor = false;
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            btnActualizar.Visible = lblactualizar.Visible = false;
            CargarDatos();
            if (pinsertar && pconsultar && peditar)
            {
                if (LblExcel.Text.Equals("EXPORTANDO"))
                    exportando = true;
                else
                    btnExcel.Visible = LblExcel.Visible = false;
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.tbRefacciones.Columns[e.ColumnIndex].HeaderText == "ESTATUS DE REFACCIÓN")
                e.CellStyle.BackColor = (e.Value.ToString() == "EXISTENCIA" ? System.Drawing.Color.PaleGreen : e.Value.ToString() == "SIN EXISTENCIA" ? System.Drawing.Color.LightCoral : System.Drawing.Color.FromArgb(255, 144, 51));
            if (this.tbRefacciones.Columns[e.ColumnIndex].HeaderText == "CANTIDAD FALTANTE" && !string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                e.CellStyle.BackColor = (Convert.ToInt32(e.Value) == 0 ? System.Drawing.Color.PaleGreen : System.Drawing.Color.Khaki);
            }
        }
        //*********************************Animación de Botones************************************
        void actualizar_datos()
        {
            observacionesEdicion obs = new observacionesEdicion(v);
            obs.Owner = this;
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                if (edita_valida && !B_Doble)
                {
                    Nuevas_Refacciones();
                    MySqlCommand actualizar = new MySqlCommand("update reportetri set PersonaEntregafkcPersonal='" + Convert.ToInt32(IdDispenso) + "', ObservacionesTrans='" + txtObservacionesT.Text.Trim() + "' WHERE idreportemfkreportemantenimiento='" + lblidreporte.Text + "'", v.c.dbconection());
                    actualizar.ExecuteNonQuery();
                    MessageBox.Show("Se actualizo el reporte y se validaron las refacciones satisfactoriamente ".ToUpper() + DateTime.Now.ToString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MySqlCommand actualizar = new MySqlCommand("update reportetri set PersonaEntregafkcPersonal='" + Convert.ToInt32(IdDispenso) + "', ObservacionesTrans='" + txtObservacionesT.Text.Trim() + "' WHERE idreportemfkreportemantenimiento='" + lblidreporte.Text + "'", v.c.dbconection());
                    actualizar.ExecuteNonQuery();
                    if (!mensaje)
                    {
                        Modificaciones_tabla(observaciones);
                        MessageBox.Show("Registro actualizado exitosamente ".ToUpper() + DateTime.Now.ToString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                LimpiarReporteTri();
                limFolioFact();
                CargarDatos();
                btnGuardar.Enabled = false;
                v.c.dbconection().Close();
            }
        }

        private void btnEditarReg_MouseMove(object sender, MouseEventArgs e)
        {
            ((Button)sender).Size = new Size(58, 53);
        }

        private void btnEditarReg_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).Size = new Size(55, 50);
        }
        //*********************************Animación de Botones************************************
        string foliof, dispenso, observaciones;
        private void tbReportes_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        bool res = false, edita_valida = false;
        void boton_edita()
        {
            //validación de campos vacios
            //if (string.IsNullOrWhiteSpace(txtFolioFactura.Text))
            //{
            //    MessageBox.Show("El campo folio de factura se encuentra vacio", "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //else
            //{
            if (string.IsNullOrWhiteSpace(txtDispenso.Text))
            {
                MessageBox.Show("El campo contraseña de usuario se encuentra vacio", "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(lblFechaEntrega.Text))
                {
                    MessageBox.Show("El campo fecha de entrega se encuentra vacio", "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //int folio = Convert.ToInt32(txtFolioFactura.Text);//validación folio mayor  a0
                    //if (folio <= 0)
                    //{
                    //    MessageBox.Show("El folio de factura debe ser mayor a 0", "VERIFICAR FOLIO DE FACTURA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    //else
                    //{
                    //consulta para obtener el nombre del almacenista cuando ingrese su contaseña
                    MySqlCommand sql = new MySqlCommand("SELECT CONCAT(coalesce(t1.ApPaterno,''),' ',coalesce(t1.ApMaterno,''),' ',coalesce(t1.nombres,'')) AS almacenista, t2.puesto,t1.idPersona,t2.idpuesto FROM cpersonal as t1 INNER JOIN puestos AS t2 ON t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal =t1.idpersona WHERE t3.password='" + v.Encriptar(txtDispenso.Text) + "'  AND t1.status='1' AND t2.status='1' and t1.empresa='" + empresa + "' ;", v.c.dbconection());
                    MySqlDataReader cmd = sql.ExecuteReader();
                    v.c.dbconection().Close();
                    if (!cmd.Read())
                    {
                        MessageBox.Show("La contraseña de almacenista ingresada es incorrecta", "CONTRASEÑA INCORRECTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDispenso.Focus();
                        txtDispenso.Clear();
                    }
                    else
                    {
                        MySqlCommand ValidarEdiciones = new MySqlCommand("SELECT t2.folio as folio,T1.FolioFactura As Factura, (SELECT concat(coalesce(X1.ApPaterno,''),' ',coalesce(X1.ApMaterno,''),' ',coalesce(X1.nombres,'')) FROM cpersonal AS X1 WHERE X1.idPersona=T1.PersonaEntregafkcPersonal) AS Dispenso, T1.ObservacionesTrans AS Obser FROM REPORTETRI AS T1 INNER JOIN reportesupervicion as t2 on idreportemfkreportemantenimiento=t2.idreportesupervicion WHERE t2.folio='" + lblFolio.Text + "' and t1.empresa='" + empresa + "';", v.c.dbconection());
                        MySqlDataReader DR = ValidarEdiciones.ExecuteReader();
                        if (DR.Read())
                        {
                            foliof = Convert.ToString(DR["Factura"]);
                            dispenso = Convert.ToString(DR["Dispenso"]);
                            observaciones = Convert.ToString(DR["Obser"]);

                            if (comparaAntesDespuesFolios() == 0 && dispenso == lblPersonaDis.Text && observaciones == mayusculas(txtObservacionesT.Text.ToLower()))
                            {
                                //Si no se modifica nada mandamos un mensaje diciendo que no se modifico nado
                                MessageBox.Show("No se modificó ningún dato", "SIN MODIFICACIONES", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                DialogResult resultado;
                                resultado = MessageBox.Show("¿Desea limpiar los campos?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (resultado == DialogResult.Yes)
                                {
                                    LimpiarReporteTri();
                                    txtFolioFactura.Enabled = true;
                                    CargarDatos();
                                    limFolioFact();
                                }
                            }
                            else
                            {
                                actualizar_datos();
                                //if (foliof == txtFolioFactura.Text)
                                //{
                                //    actualizar_datos();
                                //}
                                //else
                                //{
                                //    MySqlCommand editar_folio = new MySqlCommand("select t1.FolioFactura as folio from reportetri as t1 inner join reportesupervicion as t2 on t1.idreportemfkreportemantenimiento= t2.idreportesupervicion where t1.Foliofactura='" + txtFolioFactura.Text + "' and t1.empresa='" + empresa + "'", v.c.dbconection());
                                //    MySqlDataReader DTR = editar_folio.ExecuteReader();
                                //    if (DTR.Read())
                                //    {
                                //        foliof = Convert.ToString(DTR["folio"]);
                                //    }
                                //    DTR.Close();
                                //    if (foliof == txtFolioFactura.Text)
                                //    {
                                //        MessageBox.Show("El folio  de factura ya existe, ingrese un folio diferente", "FOLIO DE FACTURA DUPLICADO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //        txtFolioFactura.Focus();
                                //        txtFolioFactura.Clear();
                                //    }
                                //    else
                                //    {
                                //        actualizar_datos();
                                //        //consulta para actualizar los datos
                                //    }
                                //    v.c.dbconection().Close();
                                //}
                            }
                            DR.Close();
                        }
                    }
                    //}
                }
            }
            //}
        }

        private void btnEditarReg_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(v.getaData("SELECT UPPER(T1.Estatus) AS Estatus FROM reportemantenimiento AS T1 INNER JOIN reportesupervicion AS T2 ON T2.IDREPORTESUPERVICION=T1.FoliofkSupervicion WHERE T2.FOLIO='" + lblFolio.Text + "' and t1.empresa='" + empresa + "';")) == 3 && (pinsertar && peditar && pconsultar))
            {
                MessageBox.Show("La unidad ya se encuentra liberada, ya no es posible realizar modificaciones".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LimpiarReporteTri();
                CargarDatos();
            }
            else
            {
                if (conteoListBoxAnt >= LBxRefacc.Items.Count)
                {
                    boton_edita();
                }
            }
        }
        void Modificaciones_tabla(string observaciones)
        {
            string info = "";
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Reporte de Almacen','" + idrepor + "',concat('" + foliof + "',';',(Select idpersona from cpersonal where concat(ApPaterno, ' ', ApMaterno,' ',nombres)='" + dispenso + "'),';";
            info += (!string.IsNullOrWhiteSpace(observaciones) ? sql += observaciones : sql += "SIN OBSERVACIONES");
            info += "'),'" + idUsuario + "',NOW(),'Actualización de Reporte de Almacén','" + observaciones + "','2','2')";
            MySqlCommand modificaciones = new MySqlCommand(info, v.c.dbconection());
            var res = modificaciones.ExecuteNonQuery();
            v.c.dbconection().Close();
        }

        private void dtpFechaA_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void txtDispenso_Validating(object sender, CancelEventArgs e)
        {
            if (pinsertar)
            {
                if (Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join puestos as t2 on t1.cargofkcargos=t2.idpuesto inner join datosistema as t3 on t3.usuariofkcpersonal=t1.idPersona where t3.password='" + v.Encriptar(txtDispenso.Text.Trim()) + "' and t1.empresa='" + empresa + "' and t1.area='" + area + "' and t1.status='1';")) > 0)
                {
                    string[] datos = v.getaData("select upper(concat(coalesce(t1.ApPaterno,''),' ',coalesce(t1.ApMaterno,''),' ',coalesce(t1.nombres,''),'/',t1.idpersona)) from cpersonal as t1 inner join datosistema as t2 on t2.usuariofkcpersonal=t1.idPersona where t2.password='" + v.Encriptar(txtDispenso.Text.Trim()) + "'").ToString().Split('/');
                    lblPersonaDis.Text = datos[0];
                    IdDispenso = datos[1];
                }
                else
                    lblPersonaDis.Text = IdDispenso = "";
            }
        }

        private void txtFolioDe_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumerossinespacios(e);
        }

        private void txtFolioA_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumerossinespacios(e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cmbMes.Enabled = !(dtpFechaA.Enabled = dtpFechaDe.Enabled = true);
                cmbMes.SelectedIndex = 0;
            }
            else
                cmbMes.Enabled = !(dtpFechaDe.Enabled = dtpFechaA.Enabled = false);
        }

        private void btnOtros_Click(object sender, EventArgs e)
        {
            Otros rep = new Otros(idUsuario, empresa, area,v);
            rep.Owner = this;
            rep.ShowDialog();
        }

        private void LblGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}