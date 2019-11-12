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
namespace controlFallos
{
    public partial class TRI : Form
    {

        int idUsuario, empresa, area; public Thread hilo, th;
        bool res1 = true;
        public TRI(int idUsuario, int empresa, int area)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            cmbBuscarUnidad.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            cmbMes.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            cmbMecanicoSolicito.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            cmbPersonaEmtrego.MouseWheel += new MouseEventHandler(cmbBuscarUnidad_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idUsuario;
            lbltitulo.Left = (this.Width - lbltitulo.Width) / 2;
        }
        Thread exportar;
        validaciones v = new validaciones();
        conexion c = new conexion();
        void quitarseen()
        {
            while (res1)
            {
                MySqlConnection dbcon;
                if (v.c.conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                else
                    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET seenAlmacen = 1 WHERE seenAlmacen  = 0", dbcon);
                cmd.ExecuteNonQuery();
                dbcon.Close();
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
            string sql = "SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar) as privilegios FROM privilegios where usuariofkcpersonal='" + idUsuario + "' and namform='Almacen'";
            string[] privilegios = getaData(sql).ToString().Split(';');
            pinsertar = getboolfromint(Convert.ToInt32(privilegios[0]));
            pconsultar = getboolfromint(Convert.ToInt32(privilegios[1]));
            peditar = getboolfromint(Convert.ToInt32(privilegios[2]));
            pdesactivar = getboolfromint(Convert.ToInt32(privilegios[3]));
        }
        public object getaData(string sql)
        {
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            var res = cm.ExecuteScalar();
            c.dbconection().Close();
            return res;
        }
        void Limpiar_v()
        {
            cont = "";
            obser_t = "";
            fol_f = "";
            per_d = "";
        }
        public void CargarDatos()// Metodo para cargar los reportes de la base de datos al datagridview y poder mostrarlos
        {

            MySqlDataAdapter cargar = new MySqlDataAdapter("SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT  t2.Folio AS 'FOLIO', concat(t4.identificador,LPAD(consecutivo,4,'0')) AS 'UNIDAD' ,(SELECT UPPER(DATE_FORMAT(t1.FechaReporteM,'%W %d de %M del %Y'))) AS 'FECHA DE SOLICITUD', (SELECT UPPER(CONCAT(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)) FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)AS 'MECÁNICO QUE SOLICITA' ,COALESCE((SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento),'') AS 'FOLIO DE FACTURA' ,COALESCE((SELECT UPPER(DATE_FORMAT( x4.FechaEntrega,'%W %d de %M del %Y')) FROM reportetri AS x4 WHERE t1.FoliofkSupervicion=x4.idreportemfkreportemantenimiento),'')AS 'FECHA DE ENTREGA',COALESCE((SELECT UPPER(CONCAT(x5.ApPaterno,' ',x5.ApMaterno,' ',x5.nombres)) FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento),'') AS 'PERSONA QUE ENTREGO REFACCIÓN',COALESCE((SELECT UPPER(x7.ObservacionesTrans) FROM reportetri as x7 WHERE  t1.FoliofkSupervicion=x7.idreportemfkreportemantenimiento),'') AS 'OBSERVACIONES DE ALMACEN' FROM reportemantenimiento AS t1 INNER JOIN reportesupervicion AS t2 ON t1.FoliofkSupervicion=t2.idReporteSupervicion INNER JOIN cunidades AS t3 ON t2.UnidadfkCUnidades=t3.idunidad INNER JOIN careas as  t4 on t4.idarea=t3.areafkcareas WHERE t1.StatusRefacciones='Se Requieren Refacciones' and (t1.FechaReporteM BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY))AND  curdate()) and t1.empresa='" + empresa + "' ORDER BY t1.FechaReporteM DESC, t2.folio desc;", c.dbconection());
            c.dbconection().Close();
            DataSet ds = new DataSet();
            cargar.Fill(ds);
            tbReportes.DataSource = ds.Tables[0];
            tbReportes.ClearSelection();
        }
        public void Persona_entrego()
        {
            v.iniCombos("SELECT DISTINCT  UPPER(CONCAT(t2.ApPaterno,' ',t2.ApMaterno,' ',t2.nombres)) AS NOMBRE, idpersona FROM sistrefaccmant.reportetri as t1 INNER JOIN cpersonal as t2 On t1.PersonaEntregafkcPersonal = t2.idpersona where t2.empresa='" + empresa + "' group by PersonaEntregafkcPersonal;", cmbPersonaEmtrego, "idpersona", "NOMBRE", "-- SELECCIONE UN ALMACENISTA --");

        }
        public void Mecanico_solicito()
        {
            v.iniCombos("SELECT DISTINCT UPPER(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres)) AS Nombre, t2.idPersona FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.MecanicofkPersonal=t2.idpersona where t2.empresa='" + empresa + "' GROUP BY MecanicofkPersonal ORDER BY CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres) asc;", cmbMecanicoSolicito, "idPersona", "Nombre", "-- seleccione un MECáNICO --");

        }
        public void AutocompletadoFolioReporte(TextBox CajaDeTexto)//Metodo para autocompletado de "Folio de porte" en caja de etxto para buscar por folio de reporte
        {
            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
            string consulta = @"Select t1.Folio As folio ,t1.idReporteSupervicion from reportesupervicion as t1 Inner join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion WHERE t2.StatusRefacciones='Se Requieren Refacciones' and t2.empresa='" + empresa + "' ";
            MySqlCommand cmd = new MySqlCommand(consulta, c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            c.dbconection().Close();
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
            v.iniCombos("SELECT distinct concat(t3.identificador, LPAD(t2.consecutivo, 4, '0')) as eco, t2.idunidad   FROM reportesupervicion as t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idUnidad  INNER JOIN careas as t3 On t2.areafkcareas = t3.idarea  GROUP BY  t1.UnidadfkCUnidades ORDER BY concat(t3.identificador, LPAD(t2.consecutivo, 4, '0')) ASC ", cmbBuscarUnidad, "idunidad", "eco", "-- seleccione económico --");
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
        public void combos_DrawItem(object sender, DrawItemEventArgs e)
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
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        // Draw the string 
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }
        void ocultar_botones()
        {
            btnPdf.Visible = false;
            LblPDF.Visible = false;
            btnEditarReg.Visible = false;
            LblEditarR.Visible = false;
        }
        void inhanilitar_campos()
        {
            txtFolioFactura.Enabled = false;
            txtDispenso.Enabled = false;
            txtObservacionesT.Enabled = false;
        }
        void habilitar()
        {
            txtFolioFactura.Enabled = true;
            txtDispenso.Enabled = true;
            txtObservacionesT.Enabled = true;
        }
        private void TransInsumos_Load(object sender, EventArgs e)
        {
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();
            inhanilitar_campos();
            btnGuardar.Enabled = false;
            cmbMes.SelectedIndex = 0;
            dtpFechaDe.Enabled = false;
            dtpFechaA.Enabled = false;
            lblFecha2.Text = DateTime.Now.ToLongDateString().ToUpper();
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
        void Mostrar()
        {
            privilegios();
            if (pinsertar)
            {
                GpbAlmacen.Visible = true;
                LblNota.Visible = true;
                LblNota1.Visible = true;
                tbReportes.Visible = true;
                tbRefacciones.Visible = true;
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
            {
                tbReportes.Visible = true;
                tbRefacciones.Visible = true;
                LblNota.Visible = true;
                LblNota1.Visible = true;
                btnEditarReg.Visible = true;
                LblEditarR.Visible = true;
            }
            if (pconsultar)
            {
                tbReportes.Visible = true;
                tbRefacciones.Visible = true;
                GpbBusquedas.Visible = true;
                LblNota.Visible = true;
                LblNota1.Visible = true;
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
                LblPDF.Visible = true;
                btnPdf.Visible = true;
                LblNota1.Text = "PARA CONSULTAR O EDITAR LA INFORMACIÓN DE DOBLE CLIC SOBRE EL REPORTE EN LA TABLA.";
            }
        }

        public void LimpiarBusqueda()//Metodo para limpiar todos los campos que se encuentrane en la sección de busqueda.
        {
            txtBuscFolio.Clear();
            cmbMecanicoSolicito.SelectedIndex = 0;
            cmbBuscarUnidad.SelectedIndex = 0;
            txtFolioDe.Clear();
            txtFolioA.Clear();
            dtpFechaDe.Value = DateTime.Now;
            dtpFechaA.ResetText();
            cmbPersonaEmtrego.SelectedIndex = 0;
            cmbMes.SelectedIndex = 0;
        }
        void realiza_busquedas()
        {
            //COnsulta para mostrar los reportes que correspondan con los criterios de busqueda
            string consulta = "SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT  t2.Folio AS 'Folio', concat(t4.identificador,LPAD(consecutivo,4,'0')) AS 'Unidad' ,(SELECT UPPER(DATE_FORMAT(t1.FechaReporteM,'%W %d de %M del %Y'))) AS 'Fecha De Solicitud', (SELECT UPPER(CONCAT(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)) FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)AS 'Mecánico Que Solicita',COALESCE((SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento),'') AS 'Folio De Factura' ,COALESCE((SELECT UPPER(DATE_FORMAT( x4.FechaEntrega,'%W %d de %M del %Y')) FROM reportetri AS x4 WHERE t1.FoliofkSupervicion=x4.idreportemfkreportemantenimiento),'')AS 'Fecha De Entrega',COALESCE((SELECT UPPER(CONCAT(x5.ApPaterno,' ',x5.ApMaterno,' ',x5.nombres)) FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento),'') AS 'Persona Que Entrego Refacción',COALESCE((SELECT UPPER(x7.ObservacionesTrans) FROM reportetri as x7 WHERE  t1.FoliofkSupervicion=x7.idreportemfkreportemantenimiento),'') AS 'Observaciones De Almacen' FROM reportemantenimiento AS t1 INNER JOIN reportesupervicion AS t2 ON t1.FoliofkSupervicion=t2.idReporteSupervicion INNER JOIN cunidades AS t3 ON t2.UnidadfkCUnidades=t3.idunidad inner join careas as t4 on t4.idarea=t3.areafkcareas";
            String F1 = "";
            String F2 = "";
            if (checkBox1.Checked == true)
            {
                //Verificar si el chechBox esta seleccionado para realizar busqueda por rango de fechas
                F1 = dtpFechaDe.Value.ToString("yyyy-MM-dd");
                F2 = dtpFechaA.Value.ToString("yyyy-MM-dd");
                if (dtpFechaA.Value.Date < dtpFechaDe.Value.Date || dtpFechaA.Value.Date > DateTime.Now) //Validar que las fechas seleccionadas sean correctas, que la fecha 1 no sea mayor a la fecha 2
                {
                    MessageBox.Show("Las fechas seleccionadas son incorrectas".ToUpper(), "VERIFICAR FECHAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpFechaDe.Value = DateTime.Now;
                    dtpFechaA.ResetText();
                }
                else
                {
                    string wheres = "";
                    if (wheres == "")
                    {
                        wheres = " Where t1.fechaReporteM between '" + F1.ToString() + "' and '" + F2.ToString() + "'";
                    }
                    else
                    {
                        wheres += " AND t1.fechaReporteM between '" + F1.ToString() + "' and '" + F2.ToString() + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(txtBuscFolio.Text))
                    {
                        if (wheres == "")
                        {
                            wheres = " where (SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento)='" + txtBuscFolio.Text + "'";
                        }
                        else
                        {
                            wheres += " and (SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento)='" + txtBuscFolio.Text + "'";
                        }
                    }
                    if (cmbPersonaEmtrego.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (SELECT x5.idpersona FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento)='" + cmbPersonaEmtrego.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " AND (SELECT x5.idpersona FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento)='" + cmbPersonaEmtrego.SelectedValue + "'";
                        }
                    }
                    if (cmbMecanicoSolicito.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where x1.idpersona FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)='" + cmbMecanicoSolicito.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " AND (SELECT x1.idpersona FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)='" + cmbMecanicoSolicito.SelectedValue + "'";
                        }
                    }
                    if (cmbBuscarUnidad.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " And t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtFolioDe.Text) && !string.IsNullOrWhiteSpace(txtFolioA.Text))
                    {
                        int longitud = 0;
                        longitud = txtFolioA.MaxLength;
                        if (wheres == "")
                        {
                            wheres = " where substring(t2.folio,-" + longitud + "," + longitud + ") between " + Convert.ToInt32(txtFolioDe.Text) + " and " + Convert.ToInt32(txtFolioA.Text) + "";
                        }
                        else
                        {
                            wheres += " and substring(t2.folio,-" + longitud + "," + longitud + ")between " + Convert.ToInt32(txtFolioDe.Text) + " and " + Convert.ToInt32(txtFolioA.Text) + "";
                        }
                    }
                    if (wheres != "")
                    {
                        wheres += " and t1.StatusRefacciones='Se Requieren Refacciones' and t1.empresa='" + empresa + "' and (select year(t1.FechaReporteM))=( select year(now())) order by t2.folio desc";
                    }
                    MySqlDataAdapter DTA = new MySqlDataAdapter(consulta + wheres, c.dbconection());
                    c.dbconection().Close();
                    DataSet ds = new DataSet();
                    DTA.Fill(ds);
                    tbReportes.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)// si no existen reportes en el datagridview mandamos un mensaje
                    {
                        MessageBox.Show("No se encontraron reportes".ToUpper(), "NINGÚN REPORTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CargarDatos();
                        btnActualizar.Visible = false;
                        lblactualizar.Visible = false;
                    }
                    else
                    {
                        if (pinsertar && peditar && pconsultar)
                        {
                            LblExcel.Visible = true;
                            if (!est_expor)
                            {
                                btnExcel.Visible = true;
                            }
                        }
                        btnActualizar.Visible = true;
                        lblactualizar.Visible = true;
                    }
                    checkBox1.Checked = false;
                    LimpiarBusqueda();//LLamamos al metodo LimpiarBusqueda.
                }
            }
            else
            {
                //Validamos que los campos no esten vacios en caso de no estar seleccionado el CheckBox y poder realizar una busqueda.
                if (string.IsNullOrWhiteSpace(txtBuscFolio.Text) && cmbPersonaEmtrego.SelectedIndex == 0 && cmbMecanicoSolicito.SelectedIndex == 0 && cmbBuscarUnidad.SelectedIndex == 0 && string.IsNullOrWhiteSpace(txtFolioDe.Text) && string.IsNullOrWhiteSpace(txtFolioA.Text) && F1 == "" && F2 == "" && cmbMes.SelectedIndex == 0)
                {
                    //Mandamos mensaje en caso de que se encuentren vacios los campos
                    MessageBox.Show("Seleccione un criterio de búsqueda".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //si no estan vacios realizamos la busqueda con los datos ingresados o seleccionados
                    string wheres = "";
                    if (!string.IsNullOrWhiteSpace(txtBuscFolio.Text))
                    {
                        if (wheres == "")
                        {
                            wheres = " where (SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento)='" + txtBuscFolio.Text + "'";
                        }
                        else
                        {
                            wheres += " and (SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento)='" + txtBuscFolio.Text + "'";
                        }
                    }
                    if (cmbPersonaEmtrego.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where x5.idpersona FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento)='" + cmbPersonaEmtrego.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " AND (SELECT x5.idpersona FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento)='" + cmbPersonaEmtrego.SelectedValue + "'";
                        }
                    }
                    if (cmbMecanicoSolicito.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (SELECT x1.idpersona FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)='" + cmbMecanicoSolicito.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " AND (SELECT x1.idpersona FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)='" + cmbMecanicoSolicito.SelectedValue + "'";
                        }
                    }
                    if (cmbBuscarUnidad.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " And t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtFolioDe.Text) && !string.IsNullOrWhiteSpace(txtFolioA.Text))
                    {
                        int longitud = 0;
                        longitud = txtFolioA.Text.Length;
                        if (wheres == "")
                        {
                            wheres = " where substring(t2.folio,-" + longitud + "," + longitud + ") between " + Convert.ToInt32(txtFolioDe.Text) + " and " + Convert.ToInt32(txtFolioA.Text) + "";
                        }
                        else
                        {
                            wheres += " and substring(t2.folio,-" + longitud + "," + longitud + ")between " + Convert.ToInt32(txtFolioDe.Text) + " and " + Convert.ToInt32(txtFolioA.Text) + "";
                        }
                    }
                    if (cmbMes.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (select Date_format(t1.FechaReporteM,'%W %d %M %Y') like '%" + cmbMes.Text + "%' and (select year(t1.FechaReporteM))=( select year(now())))";
                        }
                        else
                        {
                            wheres += " AND (select Date_format(t1.FechaReporteM,'%W %d %M %Y') like '%" + cmbMes.Text + "%' and (select year(t1.FechaReporteM))=( select year(now())))";
                        }
                    }
                    if (wheres != "")
                    {
                        wheres += " and t1.StatusRefacciones='Se Requieren Refacciones' and t1.empresa='" + empresa + "' and (select year(t1.FechaReporteM))=( select year(now())) order by t2.folio desc";
                    }
                    c.dbconection().Close();
                    MySqlDataAdapter DTA = new MySqlDataAdapter(consulta + wheres, c.dbconection());
                    DataSet ds = new DataSet();
                    DTA.Fill(ds);
                    tbReportes.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)//En caso de no encontrar ningun reporte, mandamos un mensaje diciendo que no se encontraron reportes
                    {
                        MessageBox.Show("No se encontraron reportes".ToUpper(), "NINGÚN REPORTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        CargarDatos();
                        btnActualizar.Visible = false;
                        lblactualizar.Visible = false;
                    }
                    else
                    {
                        if (pinsertar && peditar && pconsultar)
                        {
                            LblExcel.Visible = true;
                            if (!est_expor)
                            {
                                btnExcel.Visible = true;
                            }
                        }
                        btnActualizar.Visible = true;
                        lblactualizar.Visible = true;
                    }
                    LimpiarBusqueda();//volvemos a mandar llamar nuestro metodo LimpiarBusqueda
                }
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
                {
                    realiza_busquedas();
                }
            }
        }
        public void LimpiarReporteTri()//Metodo para limpiar los campos de la parte de reporte
        {
            lblId.Text = "";
            lblUnidad.Text = "";
            lblFechaSolicitud.Text = "";
            lblMecanicoSolicita.Text = "";
            txtFolioFactura.Clear();
            lblFecha2.Text = "";
            lblPersonaDis.Text = "";
            txtObservacionesT.Clear();
            lblFolio.Text = "";
            txtDispenso.Clear();
            //tbRefacciones.DataSource = null;
            bandera_e = false;
            btnGuardar.Visible = true;
            btnGuardar.Enabled = false;
            LblGuardar.Visible = true;
            btnPdf.Enabled = true;
            btnEditarReg.Enabled = true;
            ocultar_botones();
            bandera_c = false;
            bandera_editar = false;
            B_Doble = false;
            res = false;
            editar = false;
            mensaje = false;
            btnValidar.Visible = false;
            LblGuardar.Text = "GUARDAR";
            inhanilitar_campos();
            e = null;
            statusDeMantenimiento = null;
            tbRefacciones.Rows.Clear();
            idreporS = null;
            if (pinsertar && peditar && pconsultar)
            {
                LblExcel.Visible = false;
                btnExcel.Visible = false;
            }
            Persona_entrego();
        }

        public void Nuevas_Refacciones()
        {
            MySqlCommand leerid = new MySqlCommand("SELECT t1.idReporteTransinsumos FROM reportetri as t1 inner join reportesupervicion as t2 on t2.idreportesupervicion=t1.idreportemfkreportemantenimiento where t2.folio='" + lblFolio.Text + "' and t1.empresa='" + empresa + "' and t1.idReporteTransinsumos=t1.idReporteTransinsumos", c.dbconection());
            MySqlDataReader dr = leerid.ExecuteReader();
            if (dr.Read())
            {
                //En caso de que si este guardado el reporte solamente guardamos el estatus y la cantidad de nuevas refacciones solicitadas.
                MySqlCommand agregar = new MySqlCommand(@"UPDATE pedidosrefaccion AS t1 INNER JOIN crefacciones AS t2 ON t1.RefaccionfkCRefaccion = t2.idrefaccion SET t1.EstatusRefaccion = @EstatusRefaccion, t1.CantidadEntregada = (SELECT IF(t1.cantidadentregada < t1.Cantidad,(SELECT IF(t2.existencias > (t1.Cantidad - t1.CantidadEntregada),(t1.CantidadEntregada + (t1.Cantidad - t1.CantidadEntregada)),(t1.CantidadEntregada + t2.existencias))),(SELECT IF(t1.cantidad > t2.existencias,t2.existencias,t1.cantidad)))),t2.existencias = @existencias WHERE idPedRef = @idPedRef AND(t1.EstatusRefaccion IS NULL OR T1.CantidadEntregada = 0 OR t1.cantidadentregada < t1.cantidad OR T1.CantidadEntregada = ''); ", c.dbconection());
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
                MySqlCommand sql = new MySqlCommand("update estatusvalidado set seen =1 where idreportefkreportesupervicion='" + lblidreporte.Text + "' ", c.dbconection());
                sql.ExecuteNonQuery();
                if (!edita_valida)
                {
                    MessageBox.Show("Se agregaron las refacciones satisfactoriamente ".ToUpper() + DateTime.Now.ToLongDateString().ToUpper() + " " + DateTime.Now.ToLongTimeString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                    ocultar_botones();
                    LimpiarReporteTri();
                }
                c.dbconection().Close();
            }
            dr.Close();
        }

        public void GuardarRegistro()
        {
            try
            {
                //    //En caso contrario guardamos folio de factura, fecha, persona que dispenso y guardamos el estatus y cantidad entregada de cada una de las refacciones solicitadas.
                MySqlCommand agregar = new MySqlCommand("update pedidosrefaccion as t1 inner join crefacciones as t2 on t1.RefaccionfkCRefaccion=t2.idrefaccion set t1.EstatusRefaccion=@EstatusRefaccion,t1.CantidadEntregada=(select if(t1.cantidad>t2.existencias,t2.existencias,t1.cantidad)),t2.existencias=(Select if(T1.cantidad>t2.existencias,(t2.existencias-t2.existencias),(t2.existencias-t1.cantidad))) Where idPedRef=@idPedRef   ", c.dbconection());
                foreach (DataGridViewRow row in tbRefacciones.Rows)// hacemos un ciclo repititivo para ir guardando el estatus de cada refacción solicitada
                {
                    agregar.Parameters.Clear();
                    agregar.Parameters.AddWithValue("@EstatusRefaccion", Convert.ToString(row.Cells[5].Value));
                    agregar.Parameters.AddWithValue("@idPedRef", Convert.ToString(row.Cells[0].Value));
                    agregar.ExecuteNonQuery();
                    c.dbconection().Close();
                }
                MySqlCommand sql = new MySqlCommand("insert into estatusvalidado(idreportefkreportesupervicion) values ('" + lblidreporte.Text + "')", c.dbconection());
                sql.ExecuteNonQuery();

                // consulta para insertar los datos a la base de datos
                MySqlCommand guardar = new MySqlCommand("insert into reportetri (idreportemfkreportemantenimiento,FolioFactura,FechaEntrega,PersonaEntregafkcPersonal,ObservacionesTrans,empresa) VALUES ('" + Convert.ToInt32(lblidreporte.Text) + "','" + Convert.ToInt32(txtFolioFactura.Text.Trim()) + "',curdate(), '" + Convert.ToInt32(IdDispenso) + "','" + txtObservacionesT.Text.Trim() + "','" + empresa + "') ;", c.dbconection());
                guardar.ExecuteNonQuery();
                c.dbconection().Close();
                MessageBox.Show("Registro guardado con exito ".ToUpper() + DateTime.Now.ToLongDateString().ToUpper() + " " + DateTime.Now.ToLongTimeString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Modificacion_Crear(lblFolio.Text);
                //insertar_refacciones(lblFolio.Text);
                LimpiarReporteTri();
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
            MySqlCommand modificaciones_inserciones = new MySqlCommand(sql, c.dbconection());
            modificaciones_inserciones.ExecuteNonQuery();
        }
        string existenciasR, fecha;
        void boton_guardar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFolioFactura.Text))
                {
                    MessageBox.Show("El campo folio de factura se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFolioFactura.Focus();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(lblFecha2.Text))
                    {
                        MessageBox.Show("El campo fecha de entrega se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtDispenso.Text))
                        {
                            MessageBox.Show("El campo contraseña se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtDispenso.Clear();
                        }
                        else
                        {
                            //consulta para obtener el nombre del almacenista cuando ingrese su contaseña
                            MySqlCommand sql = new MySqlCommand("SELECT CONCAT(t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres) AS almacenista, t2.puesto,t1.idPersona,t2.idpuesto FROM cpersonal as t1 INNER JOIN puestos AS t2 ON t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal =t1.idpersona WHERE t3.password='" + v.Encriptar(txtDispenso.Text) + "' AND t2.puesto='Almacenista' AND t1.status='1' AND t2.status='1' and t1.emprsa='" + empresa + "' ;", c.dbconection());
                            MySqlDataReader cmd = sql.ExecuteReader();
                            c.dbconection().Close();
                            if (!cmd.Read())
                            {
                                MessageBox.Show("La contraseña ingresada es incorrecta, verifique la contraseña".ToUpper(), "CONTRASEÑA INCORRECTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDispenso.Focus();
                                txtDispenso.Clear();
                            }
                            else
                            {
                                int folio = Convert.ToInt32(txtFolioFactura.Text);
                                if (folio <= 0)//Validacion de numero de folio de factura mayor a 0.
                                {
                                    MessageBox.Show("El folio de factura debe ser mayor a 0".ToUpper(), "VERIFICAR FOLIO DE FACTURA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtFolioFactura.Clear();
                                    txtFolioFactura.Focus();
                                }
                                else
                                {
                                    //Verificamos si ya se guardo el folio de factura
                                    MySqlCommand FolioRepetido = new MySqlCommand("Select t1.FolioFactura as Folio,t1.idReporteTransinsumos as id  from reportetri as t1 where t1.foliofactura='" + txtFolioFactura.Text + "' and empresa='" + empresa + "'", c.dbconection());
                                    MySqlDataReader DR = FolioRepetido.ExecuteReader();
                                    if (DR.Read())
                                    {
                                        if (txtFolioFactura.Text == Convert.ToString(DR["Folio"]))
                                        {
                                            //En caso correcto mandamos mensaje diciendo que ya existe el folio de factura
                                            MessageBox.Show("El folio de factura ya existe registrado en el sistema, ingrese un folio diferente".ToUpper(), "FOLIO DE FACTURA DUPLICADO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            txtFolioFactura.Focus();
                                            txtFolioFactura.Clear();
                                        }
                                    }
                                    else
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
                                                MySqlCommand estatusR = new MySqlCommand("SET lc_time_names = 'es_ES';select t3.existencias,(SELECT DATE_FORMAT(t3.proximoAbastecimiento,'%W %d de %M del %Y')) as fecha from pedidosrefaccion as t1 inner join reportesupervicion as t2 on t1.FolioPedfkSupervicion=t2.idreportesupervicion inner join crefacciones as t3  on t1.RefaccionfkCRefaccion=t3.idrefaccion where t1.FolioPedfkSupervicion='" + lblidreporte.Text + "' ", c.dbconection());
                                                MySqlDataReader DR1 = estatusR.ExecuteReader();
                                                if (DR1.Read())
                                                {
                                                    existenciasR = Convert.ToString(DR1["existencias"]);
                                                    fecha = Convert.ToString(DR1["fecha"]);

                                                    if (Convert.ToInt32(existenciasR) == 0)
                                                    {
                                                        //si se solicita una sola refaccion y no tiene existencia mandamos mensaje de alerta
                                                        MessageBox.Show("No se puede guardar el registro, por que la cantidad en existencias de la refacción se encuentra en 0. \n \n El próximo reabastecimiento es el día:  " + fecha, "SIN EXISTENCIAS DE REFACCIÓN".ToUpper(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                        LimpiarReporteTri();
                                                    }
                                                    else
                                                    {
                                                        GuardarRegistro();//Mandamos llamar el metodo GuardarRegisro.
                                                        LimpiarReporteTri();
                                                    }
                                                }
                                                DR1.Close();
                                            }
                                            else
                                            {
                                                GuardarRegistro();//Mandamos llamar el metodo GuardarRegisro.
                                                LimpiarReporteTri();
                                                btnGuardar.Enabled = false;
                                            }
                                        }
                                        DR.Close();
                                    }
                                }
                                cmd.Close();
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
            if (((Label)sender).Text.Length >= 30)
            {
                ((Label)sender).Font = new System.Drawing.Font("Garamond", 10);
            }
            else
            {
                ((Label)sender).Font = new System.Drawing.Font("Garamond", 12);
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)//Evento para guardar nuestro reporte en la base de datos.
        {
            boton_guardar();
        }

        private void txtFolioFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarNumero(e);//Llamamos al metodo validar número para que solo se permitan ingresar numeros en la caja de texto
        }

        private void txtObservacionesT_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validamos que solo se permitan ingresar letras, espacio, puntos y comas en este campo
            if (char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || Char.IsSeparator(e.KeyChar) || Char.IsControl(e.KeyChar) || (e.KeyChar == 44) || (e.KeyChar == 46) || (e.KeyChar == 249))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan letras, números   .   y    , en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (e.KeyChar == 13 || (char.IsControl(e.KeyChar) && e.KeyChar == 13))
            {
                e.Handled = true;
            }
        }


        public void ValidarNumero(KeyPressEventArgs e)//Metodo para que se permitan ingresar solamente números en la caja de texto
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan números en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ValidarLetras(KeyPressEventArgs e)//Metodo para que se permitan ingresar solamente letras en la caja de texto
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsSeparator(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtBuscFolio_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarNumero(e);//Llamamos al metodo ValidarNumero
        }
        string IdDispenso;

        private void txtDispenso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras y números en esta campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void CargarRefacciones()
        {
            tbRefacciones.ClearSelection();
            //consulta para obtener el id del reporte de supervisión
            MySqlCommand cmd = new MySqlCommand("Select t1.idreportesupervicion as id  from reportesupervicion as t1 inner join reportemantenimiento as t2 on t2.FoliofkSupervicion=t1.idreportesupervicion Where t1.Folio='" + lblFolio.Text + "'", c.dbconection());
            MySqlDataReader dr1 = cmd.ExecuteReader();
            c.dbconection().Close();
            if (dr1.Read())
            {
                lblidreporte.Text = ((Convert.ToString(dr1["id"])));
            }
            else
            {
                lblidreporte.Text = "";
            }
            dr1.Close();
        }
        string idrepor;
        string idreporS;
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
                statusDeMantenimiento = v.getaData("SELECT t1.Estatus FROM reportemantenimiento as t1 inner join reportesupervicion as t2 on t1.FoliofkSupervicion=t2.idReporteSupervicion where t2.folio='" + lblFolio.Text + "' and t1.empresa='" + empresa + "'").ToString() ?? "SIN ESTATUS";
                MySqlCommand existencia = new MySqlCommand("Select T1.idreportemfkreportemantenimiento from reportetri as T1 inner join reportesupervicion as t2 on t2.idreportesupervicion=T1.idreportemfkreportemantenimiento where t2.folio='" + lblFolio.Text + "'", c.dbconection());
                MySqlDataReader dtr1 = existencia.ExecuteReader();
                if (dtr1.Read())
                {

                    idrepor = getaData("SELECT idReporteTransinsumos FROM REPORTETRI AS T1 INNER JOIN REPORTESUPERVICION AS T2 ON T2.IDREPORTESUPERVICION=T1.idreportemfkreportemantenimiento WHERE T2.FOLIO='" + lblFolio.Text + "' and t1.empresa='" + empresa + "'").ToString();
                    nuevo_reporte = true;
                }
                //Pasamos los datos del datagridview a labels, textbox,comboBox
                if (bandera_e && !bandera_c)
                {
                    btnGuardar.Visible = false;
                    LblGuardar.Visible = false;
                    btnPdf.Visible = true;
                    LblPDF.Visible = true;
                    if ((Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[5].Value) == ""))
                    {
                        lblFecha2.Text = DateTime.Now.ToLongDateString().ToUpper();
                    }
                    else
                    {
                        lblFecha2.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[5].Value).ToUpper();
                    }
                    txtFolioFactura.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[4].Value);
                    lblPersonaDis.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[6].Value);
                    txtObservacionesT.Text = Convert.ToString(tbReportes.Rows[e.RowIndex].Cells[7].Value);
                    per_d = tbReportes.Rows[e.RowIndex].Cells[6].Value.ToString().Trim();
                    obser_t = tbReportes.Rows[e.RowIndex].Cells[7].Value.ToString().Trim();
                    fol_f = tbReportes.Rows[e.RowIndex].Cells[4].Value.ToString();
                    MySqlCommand cmdestatus = new MySqlCommand("Select UPPER(t1.Estatus) as Estatus from reportemantenimiento as t1 inner join reportesupervicion as t2 on t1.FoliofkSupervicion=t2.idreportesupervicion Where t2.Folio='" + lblFolio.Text + "' and t1.empresa='" + empresa + "' ", c.dbconection());
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
                            //conuslta para mostrar las refacciones solicitadas y saber su estatus
                            string sql = "select t1.idPedRef,UPPER(t2.codrefaccion) as 'CÓDIGO DE REFACCIÓN',UPPER(t2.nombreRefaccion) as 'NOMBRE DE REFACIÓN',sum(t1.Cantidad) as 'CANTIDAD SOLICITADA',sum(t1.CantidadEntregada) as 'CANTIDAD ENTREGADA', UPPER(t1.EstatusRefaccion) as 'ESTATUS DE REFACCIÓN' ,Coalesce(t1.cantidad-t1.CantidadEntregada,t1.cantidad) as 'CANTIDAD FALTANTE' from pedidosrefaccion as t1 inner join crefacciones as t2 on t1.RefaccionfkCRefaccion=t2.idrefaccion inner join reportesupervicion as t3 on t1.FolioPedfkSupervicion=t3.idreportesupervicion where t3.folio='" + lblFolio.Text + "' group by(t2.codrefaccion)";

                            DataTable dt = (DataTable)v.getData(sql);
                            int numFilas = dt.Rows.Count;
                            for (int i = 0; i < numFilas; i++)
                            {

                                tbRefacciones.Rows.Add(dt.Rows[i].ItemArray);
                            }
                            tbRefacciones.Visible = true;
                            c.dbconection().Close();
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
                            tbRefacciones.DataSource = null;
                            //conuslta para mostrar las refacciones solicitadas y saber su estatus
                            string sql = "SELECT T1.idPedRef, UPPER(T3.codrefaccion) AS 'CÓDIGO DE REFACCIÓN',UPPER(t3.nombreRefaccion)as 'NOMBRE DE REFACCIÓN',T1.Cantidad As 'CANTIDAD SOLICITADA',(select if(T3.existencias<0,'0',T3.existencias)) as 'CANTIDAD EN EXISTENCIAS' ,(if(t1.Cantidad=t1.CantidadEntregada,'EXISTENCIA',if(t3.existencias>0 && t3.existencias<(t1.Cantidad-t1.CantidadEntregada),'INCOMPLETO',if(t3.existencias>=(t1.Cantidad-t1.CantidadEntregada),'EXISTENCIA',t1.EstatusRefaccion)))) as 'ESTATUS DE REFACCIÓN',COALESCE(T1.CantidadEntregada,'0')as 'CANTIDAD ENTREGADA',(T1.Cantidad-T1.CantidadEntregada) as 'CANTIDAD FALTANTE' FROM pedidosrefaccion AS T1  INNER JOIN crefacciones AS T3 ON T1.RefaccionfkCRefaccion=T3.idrefaccion INNER JOIN reportesupervicion AS T4 ON t4.idReporteSupervicion=T1.FolioPedfkSupervicion WHERE t4.Folio='" + lblFolio.Text + "' and (t1.EstatusRefaccion is not null)";
                            DataTable dt = (DataTable)v.getData(sql);
                            int numFilas = dt.Rows.Count;
                            for (int i = 0; i < numFilas; i++)
                            {

                                tbRefacciones.Rows.Add(dt.Rows[i].ItemArray);
                            }
                            tbRefacciones.Visible = true;
                            //conuslta para mostrar las refacciones solicitadas y saber su estatus
                            string sql1 = "SELECT T1.idPedRef,UPPER(T3.codrefaccion) AS 'CÓDIGO DE REFACCIÓN',UPPER(t3.nombreRefaccion) as 'NOMBRE DE REFACCIÓN',T1.Cantidad As 'CANTIDAD SOLICITADA',(select if(T3.existencias<0,'0',T3.existencias)) as 'CANTIDAD EN EXISTENCIAS' ,(select if(T3.existencias<=0,'SIN EXISTENCIA',(if(t3.existencias >0 && t3.existencias < t1.cantidad,'INCOMPLETO','EXISTENCIA')) )) as 'ESTATUS DE REFACCIÓN',COALESCE(T1.CantidadEntregada,'0') as 'CANTIDAD ENTREGADA',(Select if(T1.cantidad>t3.existencias,(T1.cantidad-t3.existencias),(t1.cantidad-t1.cantidad))) as 'CANTIDAD FALTANTE' FROM pedidosrefaccion AS T1  INNER JOIN crefacciones AS T3 ON T1.RefaccionfkCRefaccion=T3.idrefaccion INNER JOIN reportesupervicion AS T4 ON t4.idReporteSupervicion=T1.FolioPedfkSupervicion WHERE t4.Folio='" + lblFolio.Text + "' and (T1.EstatusRefaccion is null  )";
                            DataTable dt1 = (DataTable)v.getData(sql1);
                            int numFilas1 = dt1.Rows.Count;
                            for (int i = 0; i < numFilas1; i++)
                            {

                                tbRefacciones.Rows.Add(dt1.Rows[i].ItemArray);
                            }
                            tbRefacciones.Visible = true;


                            txtObservacionesT.Enabled = true;
                            txtFolioFactura.Enabled = true;
                            txtDispenso.Enabled = true;
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
                    MySqlCommand cmd1 = new MySqlCommand("select t1.password,t2.nombres, t2.idpersona as id from datosistema AS t1 inner join cpersonal AS t2 on t2.idpersona=t1.usuariofkcpersonal  where concat(t2.Appaterno,' ',t2.ApMaterno,' ',t2.nombres)='" + lblPersonaDis.Text + "' and t2.empresa='" + empresa + "'", c.dbconection());
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
                    c.dbconection().Close();
                    dr2.Close();
                }
            }
        }
        string id, folio_f, fecha_d, pers_d, obser, est;
        void verifica_modificaciones()
        {
            DialogResult respuesta;
            if (!string.IsNullOrWhiteSpace(txtFolioFactura.Text) || !string.IsNullOrWhiteSpace(txtDispenso.Text))
            {
                MySqlCommand modificaciones = new MySqlCommand("SET lc_time_names = 'es_ES';SELECT T1.IDREPORTETRANSINSUMOS AS ID, T1.FolioFactura AS FOLIO,upper(Date_format(T1.FechaEntrega,'%W %d de %M del %Y')) AS FECHA,(SELECT upper(CONCAT(X1.ApPaterno,' ',X1.ApMaterno,' ',X1.nombres)) FROM cpersonal AS X1 WHERE X1.idPersona=T1.PersonaEntregafkcPersonal) AS DISPENSO ,UPPER(T1.ObservacionesTrans) AS OBSERVACIONES ,T3.Estatus AS ESTATUS FROM reportetri AS T1 INNER JOIN reportesupervicion AS T2 ON T2.IDREPORTESUPERVICION=T1.idreportemfkreportemantenimiento INNER JOIN REPORTEMANTENIMIENTO AS T3 ON T2.IDREPORTESUPERVICION=T3.FoliofkSupervicion WHERE T2.FOLIO='" + lblFolio.Text + "' and t1.empresa='" + empresa + "';", c.dbconection());
                MySqlDataReader Dr = modificaciones.ExecuteReader();
                if (Dr.Read())
                {
                    id = Convert.ToString(Dr["ID"]);
                    folio_f = Convert.ToString(Dr["FOLIO"]);
                    fecha_d = Convert.ToString(Dr["FECHA"]);
                    pers_d = Convert.ToString(Dr["DISPENSO"]);
                    obser = Convert.ToString(Dr["OBSERVACIONES"]);
                    est = Dr["ESTATUS"].ToString();

                    if ((folio_f != txtFolioFactura.Text || fecha_d != lblFecha2.Text || pers_d != lblPersonaDis.Text || obser != txtObservacionesT.Text) && ((est != "LIBERADA") || (est == "LIBERADA" && (pinsertar && pconsultar && peditar))))
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
        public void Letras_Numeros(KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan letras y números en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            {
                LblExcel.Visible = false;
                btnExcel.Visible = false;
            }
            exportando = false;
            est_expor = false;
        }
        public void ExportarExcel()//Metodo para exportar datos de datagridview a Excel.
        {
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
                //Exportacion_Excel();
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando1);
                    this.Invoke(delega);
                }
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            est_expor = true;
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        }
        //*********************************Animación de Botones************************************
        private void btnActualizar_MouseMove(object sender, MouseEventArgs e)
        {
            btnActualizar.Size = new Size(55, 55);
        }

        private void btnPdf_MouseMove_1(object sender, MouseEventArgs e)
        {
            btnPdf.Size = new Size(71, 60);
        }

        private void btnPdf_MouseLeave_1(object sender, EventArgs e)
        {
            btnPdf.Size = new Size(65, 55);
        }

        private void btnExcel_MouseLeave_1(object sender, EventArgs e)
        {
            btnExcel.Size = new Size(50, 50);
        }

        private void btnExcel_MouseMove_1(object sender, MouseEventArgs e)
        {
            btnExcel.Size = new Size(55, 55);
        }

        private void btnActualizar_MouseLeave(object sender, EventArgs e)
        {
            btnActualizar.Size = new Size(50, 50);
        }

        //*********************************Animación de Botones************************************
        void Exportacion()
        {
            MySqlCommand exportacion = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Reporte de Almacen','" + idrepor + "','Exportación de reporte en archivo pdf','" + '1' + "',NOW(),'Exportación a PDF de reporte de almacén','2','2')", c.dbconection());
            exportacion.ExecuteNonQuery();
        }
        string fo, uni, fe_so, Mec_soli, Se_requieen, fo_fac, fec_entrega_ref, pers_entr, observaciones_tri;

        private void btnValidar_MouseLeave(object sender, EventArgs e)
        {
            btnValidar.Size = new Size(64, 61);
        }

        private void TRI_FormClosing(object sender, FormClosingEventArgs e)
        {
            hilo.Abort();
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }
        private void tbRefacciones_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (statusDeMantenimiento == "EN PROCESO")
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
                                double exist = Convert.ToDouble(tbRefacciones.Rows[e.RowIndex].Cells[4].Value);
                                double cs = Convert.ToDouble(tbRefacciones.Rows[e.RowIndex].Cells[3].Value);
                                double ce = Convert.ToDouble(tbRefacciones.Rows[e.RowIndex].Cells[6].Value);
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

        public void Expota_PDF()
        {
            MySqlCommand Obetener_Datos = new MySqlCommand("SET lc_time_names = 'es_ES';SELECT  UPPER(t2.Folio) AS 'Folio',UPPER(concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'Unidad' ,(SELECT UPPER(DATE_FORMAT(t1.FechaReporteM,'%W %d de %M del %Y'))) AS 'Fecha De Solicitud', (SELECT UPPER(CONCAT(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)) FROM cpersonal AS x1 WHERE x1.idPersona=t1.MecanicofkPersonal)AS 'Mecánico Que Solicita' , UPPER(t1.StatusRefacciones) AS 'Estatus De Refacciones',COALESCE((SELECT x2.FolioFactura FROM reportetri AS x2 WHERE t1.FoliofkSupervicion=x2.idreportemfkreportemantenimiento),'') AS 'Folio De Factura' ,COALESCE((SELECT UPPER(DATE_FORMAT(x4.FechaEntrega,'%W %d de %M del %Y')) FROM reportetri AS x4 WHERE t1.FoliofkSupervicion=x4.idreportemfkreportemantenimiento),'')AS 'Fecha De Entrega',COALESCE((SELECT UPPER(CONCAT(x5.ApPaterno,' ',x5.ApMaterno,' ',x5.nombres)) FROM cpersonal AS x5 INNER JOIN reportetri AS x6 ON x5.idpersona=x6.PersonaEntregafkcPersonal WHERE t1.FoliofkSupervicion=x6.idreportemfkreportemantenimiento),'') AS 'Persona Que Entrego Refacción',COALESCE((SELECT UPPER(x7.ObservacionesTrans) FROM reportetri as x7 WHERE  t1.FoliofkSupervicion=x7.idreportemfkreportemantenimiento),'') AS 'Observaciones De Almacen' FROM reportemantenimiento AS t1 INNER JOIN reportesupervicion AS t2 ON t1.FoliofkSupervicion=t2.idReporteSupervicion INNER JOIN cunidades AS t3 ON t2.UnidadfkCUnidades=t3.idunidad INNER JOIN careas as  t4 on t4.idarea=t3.areafkcareas WHERE t1.StatusRefacciones='Se Requieren Refacciones' and t2.folio='" + lblFolio.Text + "' an t1.empresa='"+empresa+"'", c.dbconection());
            MySqlDataReader dr = Obetener_Datos.ExecuteReader();
            if (dr.Read())
            {
                fo = Convert.ToString(dr["Folio"]);
                uni = Convert.ToString(dr["Unidad"]);
                fe_so = Convert.ToString(dr["Fecha De Solicitud"]);
                Mec_soli = Convert.ToString(dr["Mecánico Que Solicita"]);
                Se_requieen = Convert.ToString(dr["Estatus De Refacciones"]);
                fo_fac = Convert.ToString(dr["Folio De Factura"]);
                fec_entrega_ref = Convert.ToString(dr["Fecha De Entrega"]);
                pers_entr = Convert.ToString(dr["Persona Que Entrego Refacción"]);
                observaciones_tri = Convert.ToString(dr["Observaciones De Almacen"]);
            }

            //Código para generación de archivo pdf
            Document doc = new Document(PageSize.LETTER);
            string tri = "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";
            doc.SetMargins(20f, 20f, 10f, 10f);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ValidateNames = true;
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Reporte";
            //saveFileDialog1.DefaultExt = ".pdf";
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
                {
                    filename = filename + ".pdf";
                }
                while (filename.ToLower().Contains(".pdf.pdf"))
                {
                    filename = filename.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
                }
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
                    byte[] img = Convert.FromBase64String(tri);
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    Chunk chunk1 = new Chunk("FECHA DE EXPORTACIÓN: " + DateTime.Now.ToLongDateString().ToUpper(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD));
                    Chunk chunk2 = new Chunk("HORA DE EXPORTACIÓN:" + DateTime.Now.ToLongTimeString().ToUpper(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD));
                    Chunk chunk = new Chunk("REPORTE TRI ALMACEN", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    doc.Add(imagen);
                    doc.Add(new Paragraph(chunk));

                    doc.Add(new Paragraph(chunk1));
                    doc.Add(chunk2);
                    doc.Add(new Paragraph("                                    "));
                    PdfPTable tabla = new PdfPTable(2);
                    tabla.DefaultCell.Border = 0;
                    tabla.WidthPercentage = 100;
                    PdfPCell celda = new PdfPCell();
                    celda.Border = 0;
                    Phrase SaltoLinea = new Phrase("           ");
                    Phrase Folio = new Phrase("FOLIO DE REPORTE:", arial);
                    Phrase LFolio = new Phrase(fo, arial2);
                    Phrase Fecha = new Phrase("FECHA DE SOLICITUD", arial);
                    Phrase LFecha = new Phrase(fe_so, arial2);
                    Phrase FolioF = new Phrase("FOLIO DE FACTURA:", arial);
                    Phrase LFolioF = new Phrase(fo_fac, arial2);
                    Phrase PerDis = new Phrase("PERSONA QUE ENTREGO:", arial);
                    Phrase LpersDis = new Phrase(pers_entr, arial2);
                    celda.AddElement(Folio);
                    celda.AddElement(LFolio);
                    celda.AddElement(SaltoLinea);
                    celda.AddElement(Fecha);
                    celda.AddElement(LFecha);
                    celda.AddElement(SaltoLinea);
                    celda.AddElement(FolioF);
                    celda.AddElement(LFolioF);
                    celda.AddElement(SaltoLinea);
                    celda.AddElement(PerDis);
                    celda.AddElement(LpersDis);
                    PdfPCell celda2 = new PdfPCell();
                    celda2.Border = 0;
                    Phrase Unidad = new Phrase("UNIDAD:", arial);
                    Phrase LUnidad = new Phrase(uni, arial2);
                    Phrase Meca = new Phrase("MECÁNICO QUE SOLÍCITO:", arial);
                    Phrase lMeca = new Phrase(Mec_soli, arial2);
                    Phrase FechaE = new Phrase("FECHA DE ENTREGA:", arial);
                    Phrase LFechaE = new Phrase(fec_entrega_ref, arial2);
                    celda2.AddElement(Unidad);
                    celda2.AddElement(LUnidad);
                    celda2.AddElement(SaltoLinea);
                    celda2.AddElement(Meca);
                    celda2.AddElement(lMeca);
                    celda2.AddElement(SaltoLinea);
                    celda2.AddElement(FechaE);
                    celda2.AddElement(LFechaE);
                    PdfPTable tabla1 = new PdfPTable(1);
                    tabla1.DefaultCell.Border = 0;
                    tabla1.WidthPercentage = 100;
                    PdfPCell celda3 = new PdfPCell();
                    celda3.Border = 0;
                    Phrase Observa = new Phrase("OBSERVACIONES:", arial);
                    Phrase LObserva = new Phrase(observaciones_tri, arial2);
                    celda3.AddElement(Observa);
                    celda3.AddElement(LObserva);
                    tabla.AddCell(celda);
                    tabla.AddCell(celda2);
                    tabla1.AddCell(celda3);
                    doc.Add(tabla);
                    doc.Add(new Paragraph("                                    "));
                    doc.Add(tabla1);
                    doc.Add(new Paragraph("                                    "));
                    doc.Add(new Chunk("REFACCIONES SOLICITADAS:", FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("                                   "));
                    GenerarDocumento(doc);
                    doc.Close();
                    System.Diagnostics.Process.Start(filename);
                    Exportacion();
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
            if (string.IsNullOrWhiteSpace(txtFolioFactura.Text))
            {
                MessageBox.Show("EL campo folio de factura se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(lblPersonaDis.Text))
                {
                    MessageBox.Show("EL campo almacenista se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(lblFecha2.Text))
                    {
                        MessageBox.Show("El campo fecha de entrega se encuentra vacio.ToLower()".ToUpper(), "CCAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Expota_PDF();
                    }
                }
            }
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
            PdfPTable datatable = new PdfPTable(tbRefacciones.ColumnCount);
            datatable.DefaultCell.Padding = 4;
            float[] headerwidths = GetTamañoColumnas(tbRefacciones);
            datatable.SetWidths(headerwidths);
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
                    if (tbRefacciones[j, i].Value != null)
                    {
                        datatable.AddCell(new Phrase(tbRefacciones[j, i].Value.ToString(), FontFactory.GetFont("ARIAL", 8)));
                    }
                }
                datatable.CompleteRow();
            }
            datatable.AddCell(observaciones);
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
                cont = v.Desencriptar(getaData("SELECT PASSWORD FROM DATOSISTEMA AS T1 INNER JOIN CPERSONAL AS T2 ON T2.IDPERSONA=T1.usuariofkcpersonal WHERE upper(concat(APPATERNO,' ',APMATERNO,' ',NOMBRES))='" + per_d + "'").ToString());
                if ((obser_t != txtObservacionesT.Text.Trim() || fol_f != txtFolioFactura.Text || cont != txtDispenso.Text) && (!string.IsNullOrWhiteSpace(txtFolioFactura.Text) && !string.IsNullOrWhiteSpace(txtDispenso.Text)))
                {
                    btnEditarReg.Visible = true;
                    LblEditarR.Visible = true;
                }
                else
                {
                    btnEditarReg.Visible = false;
                    LblEditarR.Visible = false;
                }
            }
        }

        bool exportando = false, est_expor = false;
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            btnActualizar.Visible = false;
            lblactualizar.Visible = false;
            CargarDatos();
            if (pinsertar && pconsultar && peditar)
            {
                if (LblExcel.Text.Equals("EXPORTANDO"))
                {
                    exportando = true;
                }
                else
                {
                    LblExcel.Visible = false;
                    btnExcel.Visible = false;
                }
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.tbRefacciones.Columns[e.ColumnIndex].HeaderText == "ESTATUS DE REFACCIÓN")
            {
                if (Convert.ToString(e.Value) == "EXISTENCIA")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "SIN EXISTENCIA")
                    {
                        e.CellStyle.BackColor = Color.LightCoral;
                    }
                    else
                    {
                        if (e.Value.ToString() == "INCOMPLETO")
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 144, 51);
                        }
                    }
                }
            }
            if (this.tbRefacciones.Columns[e.ColumnIndex].HeaderText == "CANTIDAD FALTANTE")
            {
                if (Convert.ToDouble(e.Value) > 0)
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else
                {
                    if (Convert.ToDouble(e.Value) == 0)
                    {
                        e.CellStyle.BackColor = Color.PaleGreen;
                    }
                }
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
                    MySqlCommand actualizar = new MySqlCommand("update reportetri set FolioFactura='" + Convert.ToInt32(txtFolioFactura.Text) + "',PersonaEntregafkcPersonal='" + Convert.ToInt32(IdDispenso) + "', ObservacionesTrans='" + txtObservacionesT.Text.Trim() + "' WHERE idreportemfkreportemantenimiento='" + lblidreporte.Text + "'", c.dbconection());
                    actualizar.ExecuteNonQuery();
                    MessageBox.Show("Se actualizo el reporte y se validaron las refacciones satisfactoriamente ".ToUpper() + DateTime.Now.ToString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MySqlCommand actualizar = new MySqlCommand("update reportetri set FolioFactura='" + Convert.ToInt32(txtFolioFactura.Text) + "',PersonaEntregafkcPersonal='" + Convert.ToInt32(IdDispenso) + "', ObservacionesTrans='" + txtObservacionesT.Text.Trim() + "' WHERE idreportemfkreportemantenimiento='" + lblidreporte.Text + "'", c.dbconection());
                    actualizar.ExecuteNonQuery();
                    if (!mensaje)
                    {
                        Modificaciones_tabla(observaciones);
                        MessageBox.Show("Registro actualizado exitosamente ".ToUpper() + DateTime.Now.ToString().ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            LimpiarReporteTri();
            CargarDatos();
            btnGuardar.Enabled = false;
            c.dbconection().Close();
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
            if (string.IsNullOrWhiteSpace(txtFolioFactura.Text))
            {
                MessageBox.Show("El campo folio de factura se encuentra vacio", "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtDispenso.Text))
                {
                    MessageBox.Show("El campo contraseña de usuario se encuentra vacio", "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(lblFecha2.Text))
                    {
                        MessageBox.Show("El campo fecha de entrega se encuentra vacio", "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        int folio = Convert.ToInt32(txtFolioFactura.Text);//validación folio mayor  a0
                        if (folio <= 0)
                        {
                            MessageBox.Show("El folio de factura debe ser mayor a 0", "VERIFICAR FOLIO DE FACTURA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //consulta para obtener el nombre del almacenista cuando ingrese su contaseña
                            MySqlCommand sql = new MySqlCommand("SELECT CONCAT(t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres) AS almacenista, t2.puesto,t1.idPersona,t2.idpuesto FROM cpersonal as t1 INNER JOIN puestos AS t2 ON t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal =t1.idpersona WHERE t3.password='" + v.Encriptar(txtDispenso.Text) + "' AND t2.puesto='Almacenista' AND t1.status='1' AND t2.status='1' and t1.empresa='"+empresa+"' ;", c.dbconection());
                            MySqlDataReader cmd = sql.ExecuteReader();
                            c.dbconection().Close();
                            if (!cmd.Read())
                            {
                                MessageBox.Show("La contraseña de almacenista ingresada es incorrecta", "CONTRASEÑA INCORRECTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDispenso.Focus();
                                txtDispenso.Clear();
                            }
                            else
                            {
                                MySqlCommand ValidarEdiciones = new MySqlCommand("SELECT t2.folio as folio,T1.FolioFactura As Factura, (SELECT concat(X1.ApPaterno,' ',X1.ApMaterno,' ',X1.nombres) FROM cpersonal AS X1 WHERE X1.idPersona=T1.PersonaEntregafkcPersonal) AS Dispenso, T1.ObservacionesTrans AS Obser FROM REPORTETRI AS T1 INNER JOIN reportesupervicion as t2 on idreportemfkreportemantenimiento=t2.idreportesupervicion WHERE t2.folio='" + lblFolio.Text + "' and t1.empresa='"+empresa+"';", c.dbconection());
                                MySqlDataReader DR = ValidarEdiciones.ExecuteReader();
                                if (DR.Read())
                                {
                                    foliof = Convert.ToString(DR["Factura"]);
                                    dispenso = Convert.ToString(DR["Dispenso"]);
                                    observaciones = Convert.ToString(DR["Obser"]);

                                    if (foliof == txtFolioFactura.Text && dispenso == lblPersonaDis.Text && observaciones == mayusculas(txtObservacionesT.Text.ToLower()))
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
                                        }
                                    }
                                    else
                                    {
                                        if (foliof == txtFolioFactura.Text)
                                        {
                                            actualizar_datos();
                                        }
                                        else
                                        {
                                            MySqlCommand editar_folio = new MySqlCommand("select t1.FolioFactura as folio from reportetri as t1 inner join reportesupervicion as t2 on t1.idreportemfkreportemantenimiento= t2.idreportesupervicion where t1.Foliofactura='" + txtFolioFactura.Text + "' and t1.empresa='"+empresa+"'", c.dbconection());
                                            MySqlDataReader DTR = editar_folio.ExecuteReader();
                                            if (DTR.Read())
                                            {
                                                foliof = Convert.ToString(DTR["folio"]);
                                            }
                                            DTR.Close();
                                            if (foliof == txtFolioFactura.Text)
                                            {
                                                MessageBox.Show("El folio  de factura ya existe, ingrese un folio diferente", "FOLIO DE FACTURA DUPLICADO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                txtFolioFactura.Focus();
                                                txtFolioFactura.Clear();
                                            }
                                            else
                                            {
                                                actualizar_datos();
                                                //consulta para actualizar los datos
                                            }
                                            c.dbconection().Close();
                                        }
                                    }
                                    DR.Close();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnEditarReg_Click(object sender, EventArgs e)
        {
            MySqlCommand Verificar_estatus = new MySqlCommand("SELECT UPPER(T1.Estatus) AS Estatus FROM reportemantenimiento AS T1 INNER JOIN reportesupervicion AS T2 ON T2.IDREPORTESUPERVICION=T1.FoliofkSupervicion WHERE T2.FOLIO='" + lblFolio.Text + "' and t1.empresa='"+empresa+"';", c.dbconection());
            MySqlDataReader DR = Verificar_estatus.ExecuteReader();
            if (DR.Read())
            {
                string ESTATUS = Convert.ToString(DR["Estatus"]);
                if (ESTATUS == "LIBERADA" && (pinsertar && peditar && pconsultar))
                {
                    MessageBox.Show("La unidad ya se encuentra liberada, ya no es posible realizar modificaciones".ToUpper(), "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimpiarReporteTri();
                    CargarDatos();
                }
                else
                {
                    boton_edita();
                }
            }
            DR.Close();
        }
        void Modificaciones_tabla(string observaciones)
        {
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Reporte de Almacen','" + idrepor + "',concat('" + foliof + ";',(Select idpersona from cpersonal where concat(ApPaterno, ' ', ApMaterno,' ',nombres)='" + dispenso + "'),';";
            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                sql += observaciones;
            }
            else
            {
                sql += "SIN OBSERVACIONES";
            }
            sql += "'),'" + idUsuario + "',NOW(),'Actualización de Reporte de Almacén','" + observaciones + "','2','2')";
            MySqlCommand modificaciones = new MySqlCommand(sql, c.dbconection());
            var res = modificaciones.ExecuteNonQuery();
            c.dbconection().Close();
        }

        private void dtpFechaA_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void txtDispenso_Validating(object sender, CancelEventArgs e)
        {
            if (pinsertar)
            {
                //consulta para obtener el nombre del almacenista cuando ingrese su contaseña
                MySqlCommand sql = new MySqlCommand("SELECT UPPER(CONCAT(t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres)) AS almacenista, t2.puesto,t1.idPersona,t2.idpuesto FROM cpersonal as t1 INNER JOIN puestos AS t2 ON t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal =t1.idpersona WHERE t3.password='" + v.Encriptar(txtDispenso.Text) + "' AND t1.status='1' AND t2.status='1' and t1.empresa='"+empresa+"';", c.dbconection());
                MySqlDataReader cmd = sql.ExecuteReader();
                c.dbconection().Close();
                if (cmd.Read())
                {
                    //si es correcta mostramos el nombre
                    lblPersonaDis.Text = Convert.ToString(cmd["almacenista"]);
                    IdDispenso = Convert.ToString(cmd["idpersona"]);
                }
                else
                {
                    //En caso contrario no mostramos nada nada
                    IdDispenso = "";
                    lblPersonaDis.Text = "";
                }
                cmd.Close();
            }
        }

        private void txtFolioDe_KeyPress(object sender, KeyPressEventArgs e)
        {
            Letras_Numeros(e);
        }

        private void txtFolioA_KeyPress(object sender, KeyPressEventArgs e)
        {
            Letras_Numeros(e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                dtpFechaA.Enabled = true;
                dtpFechaDe.Enabled = true;
                cmbMes.Enabled = false;
                cmbMes.SelectedIndex = 0;
            }
            else
            {
                dtpFechaDe.Enabled = false;
                dtpFechaA.Enabled = false;
                cmbMes.Enabled = true;
            }
        }
    }
}


