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
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;

namespace controlFallos
{

    public partial class Supervisión : Form
    {
        int empresa, area, idUsuario; public Thread hilo;
        static bool res = true;
        validaciones v;
        DataTable dt = new DataTable();
        delegate void El_Delegado(); delegate void El_Delegado1();
        delegate void delunidades(); delegate void deldatos(); delegate void delbusquedas();
        Thread th, exportar, thunidades, thdatos, thbusquedas;
        int unidadAnterior, supervisorAnterior, conductorAnterior, servicioAnterior, TipoFalloAnterior, grupoFalloAnterior, subGrupoAnterior, categoriaAnterior, codigoAnterior, IdRepor, idconductor, idsupervisor;
        string fechaAnterior, kilometrajeAnterior, FalloAnterior, ObservacionesAnterior, consulta_gral = "SET lc_time_names = 'es_ES';select t1.folio as 'FOLIO',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'ECO',upper(Date_format(t1.FechaReporte,'%W %d de %M del %Y')) as 'FECHA DEL REPORTE',(select UPPER(concat(coalesce(x1.ApPaterno,''),' ',coalesce(x1.ApMaterno,''),' ',coalesce(x1.nombres,'')))from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)as 'PERSONA QUE INSERTÓ',coalesce((SELECT x2.Credencial FROM cpersonal AS x2 WHERE  x2.idpersona=t1.CredencialConductorfkCPersonal),'')as 'CREDENCIAL DE CONDUCTOR',if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios))as 'SERVICIO',TIME_FORMAT(t1.HoraEntrada,'%r') as 'HORA DEL REPORTE', t1.KmEntrada as 'KILOMETRAJE DE REPORTE',if(tipofallo='1','CORRECTIVO',(if(tipofallo='2','PREVENTIVO',(if(tipofallo='3','REITERATIVO',(if(tipofallo='4','REPROGRAMADO','SEGUIMIENTO'))))))) as 'TIPO DE FALLO',UPPER(t6.descfallo) as 'SUBGRUPO DE FALLO', t7.codfallo as 'CÓDIGO DE FALLO',UPPER(t1.DescFalloNoCod) as 'DESCRIPCIÓN DE FALLO NO CODIFICADO', upper(t1.ObservacionesSupervision)as 'OBSERVACIONES', upper(date_format(t2.FechaHoraI,'%W %d de %M del %Y / %H:%i')) as 'FECHA/HORA INCIO MANT.', upper(date_format(t2.fechahorat,'%W %d de %M del %Y / %H:%i')) as 'FECHA/HORA TERMINO',if(t2.estatus is null,'',(if(t2.estatus=1,'EN PROCESO',(if(t2.estatus=2,'REPROGRAMADA','LIBERADA'))))) as 'ESTATUS', upper(t2.TrabajoRealizado) as 'TRABAJO REALIZADO',(select upper(concat(coalesce(x8.appaterno,''),' ',coalesce(x8.apmaterno,''),' ',coalesce(x8.nombres,''))) from cpersonal as x8 where x8.idpersona=t2.MecanicofkPersonal) as 'MECÁNICO QUE REALIZO MANTENIMIENTO' ,upper(t2.ObservacionesM) as 'OBSERVACIONES MANT.' from reportesupervicion as t1 left join reportemantenimiento as t2 on t2.FoliofkSupervicion=t1.idReporteSupervicion inner join cunidades as t3 on t1.UnidadfkCUnidades=t3.idunidad  INNER JOIN careas AS t4 on t4.idarea=t3.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas left join cdescfallo as t6 on t1.DescrFallofkcdescfallo=t6.iddescfallo left join cfallosesp as t7 on t1.CodFallofkcfallosesp=t7.idfalloEsp ";
        public Supervisión(int idUsuario, int empresa, int area, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            cmbBuscStatus.DrawItem += v.comboBoxEstatusr_DrwaItem;
            cmbUnidad.DrawItem += v.combos_DrawItem;
            cmbMeses.DrawItem += v.combos_DrawItem;
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
        void Inhabilita_k(bool valor)
        {
            cmbUnidad.Enabled = txtSupervisor.Enabled = txtConductor.Enabled = cmbServicio.Enabled = txtKilometraje.Enabled =  valor;
        }
        //void quitarseen()
        //{
        //    while (res)
        //    {
        //        MySqlConnection dbcon;
        //        dbcon = new MySqlConnection("Server = 192.168.1.67; user=UPT; password = UPT2018; database = sistrefaccmant ;port=3306");
        //        dbcon.Open();
        //        MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET seen = 1 WHERE seen  = 0 AND (Estatus='2' || Estatus='3')", dbcon);
        //        cmd.ExecuteNonQuery();
        //        dbcon.Close();
        //        dbcon.Dispose();
        //        Thread.Sleep(180000);
        //    }
        //}

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
        void busquedas()
        {
            if (this.InvokeRequired)
            {
                delbusquedas d = new delbusquedas(busquedas);
                this.Invoke(d);
            }
            v.comboswithuot(cmbBuscStatus, new string[] { "--SELECCIONES ESTATUS--", "EN PROCESO", "REPROGRAMADA", "LIBERADA" });
            v.comboswithuot(cmbMeses, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "otubre", "Noviembre", "diciembre" });
            v.comboswithuot(cmbTipoFallo, new string[] { "--seleccione tipo de fallo", "correctivo", "preventivo", "reiterativo", "reprogramado", "seguimiento" });
            v.iniCombos("select t1.idempresa as id,UPPER(t1.nombreEmpresa)as nom  from cempresas as t1 where empresa ='1' order by t1.nombreEmpresa;", cmbEmpresa, "id", "nom", "--SELECCIONE EMPRESA--");
            v.iniCombos("SELECT idpersona,UPPER(CONCAT(coalesce(T1.ApPaterno,''),' ',coalesce(T1.ApMaterno,''),' ',coalesce(T1.nombres,''))) AS NOMBRE FROM cpersonal AS T1 INNER JOIN datosistema AS T2 on T2.usuariofkcpersonal=T1.idpersona WHERE t1.area='1' and t1.empresa='1' ORDER BY coalesce(ApPaterno,'');", cmbSupervisores, "idpersona", "NOMBRE", "--SELECCIONE SUPERVISOR--");
            v.iniCombos("Select iddescfallo as id,upper(descfallo) as d  from cdescfallo order by descfallo;", cmbBuscarDescripcion, "id", "d", "--SELECCIONE SUBGRUPO--");
            thbusquedas.Abort();
        }
        public void Unidades() //Metodo para aregar las unidades de la tabla cunidades y mostrarlas en el comboBox para seleccionar una unidad el hacer un nuevo reporte o editar alguno.
        {
            if (this.InvokeRequired)
            {
                delunidades d = new delunidades(Unidades);
                this.Invoke(d);
            }
            string where = "";
            if (idUsuario 
                != 399)
            {
                where = "and (t1.descripcioneco not like '%Cometa%' AND t1.descripcioneco not like '%TRANSIT%' and t1.descripcioneco not like '%Travelers%' and t1.descripcioneco not like '%Urvan%' and t1.descripcioneco not like '%Browser%' and t1.descripcioneco not like '%Nezahualpilli%' and t1.descripcioneco not like '%Yutong%' and t1.descripcioneco not like '%Versa%' and t1.descripcioneco not like '%Atreyo%' and t1.descripcioneco not like '%Chevrolet%' and t1.descripcioneco not like '%Camioneta%')";
            }
            else
            {
                where = "and (t1.descripcioneco  like '%Cometa%' or t1.descripcioneco  like '%TRANSIT%' or t1.descripcioneco  like '%Travelers%' or t1.descripcioneco  like '%Urvan%' or t1.descripcioneco  like '%Browser%' or t1.descripcioneco  like '%Nezahualpilli%' or t1.descripcioneco  like '%Yutong%' or t1.descripcioneco  like '%Versa%' or t1.descripcioneco  like '%Atreyo%' or t1.descripcioneco  like '%Chevrolet%' or t1.descripcioneco  like '%Camioneta%')";
            }
            v.iniCombos("select t1.idunidad, concat(coalesce(t2.identificador,''), LPAD(t1.consecutivo,4,'0')) as eco from cunidades as t1 inner join careas as t2 on t1.areafkcareas = t2.idarea and t1.status='1' "+ where + "  order by eco;", cmbUnidad, "idunidad", "ECo", "--SELECCIONE UNIDAD--");
            thunidades.Abort();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //hilo = new Thread(new ThreadStart(quitarseen));
            //hilo.Start();
           // dtpFechaDe.MinDate = dtpFechaA.MinDate = Convert.ToDateTime(v.getaData("select FechaReporte from reportesupervicion order by FechaReporte limit 1;") ?? DateTime.Today);
            //dtpFechaA.MaxDate = dtpFechaDe.MaxDate = Convert.ToDateTime(v.getaData("select FechaReporte from reportesupervicion order by FechaReporte desc limit 1;") ?? DateTime.Today);
            privilegios();
            lblFechaReporte.Text = DateTime.Now.ToLongDateString().ToUpper();
            Genera_Folio();
            consulta_descripciones();
            thunidades = new Thread(new ThreadStart(Unidades));
            thunidades.Start();
            thdatos = new Thread(new ThreadStart(cargarDAtos));
            thdatos.Start();
            thbusquedas = new Thread(new ThreadStart(busquedas));
            thbusquedas.Start();
            Mostrar();
            consulta_grupos();
            DgvTabla.ClearSelection();
            dtpFechaDe.Enabled = dtpFechaA.Enabled = btnEditar.Visible = LblPDF.Visible = btnpdf.Visible = lblactualizar.Visible = cmbServicio.Enabled = false;
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
                LblPDF.Visible = btnpdf.Visible = true;
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
            v.c.dbconection().Close();
        }
        public void cargarDAtos()//Metodo para cargar los reportes que se encuentra en la base de datos en el datagridview y para gñenerar el folio de reporte autoincrementable
        {
            //Conusulta para pbtener reportes almacenados en la base de datos
            if (this.InvokeRequired)
            {
                deldatos d = new deldatos(cargarDAtos);
                this.Invoke(d);
            }
            MySqlDataAdapter cargar = new MySqlDataAdapter(consulta_gral + " WHERE FechaReporte BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY))AND  curdate() order by t1.FechaReporte desc, t1.HoraEntrada  desc limit 10;", v.c.dbconection());
            DataSet ds = new DataSet();
            cargar.Fill(ds);
            DgvTabla.DataSource = ds.Tables[0];
            DgvTabla.Columns[0].Frozen = true;// mostramos reportes en datagridview
            DgvTabla.ClearSelection();
            v.c.dbconection().Close();
            DgvTabla.ClearSelection();
            thdatos.Abort();
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
                        wheres = (wheres == "" ? " WHERE t1.FechaReporte BETWEEN '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'" : wheres += " AND (t1.FechaReporte BETWEEN '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "')");
                    if (cmbBuscarUnidad.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'" : wheres += " AND t3.idunidad='" + cmbBuscarUnidad.SelectedValue + "'");
                    if (cmbBuscarDescripcion.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where t6.iddescfallo='" + cmbBuscarDescripcion.SelectedValue + "'" : wheres += " AND t6.iddescfallo='" + cmbBuscarDescripcion.SelectedValue + "'");
                    if (cmbSupervisores.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where t1.SupervisorfkCPersonal='" + cmbSupervisores.SelectedValue + "'" : wheres += " AND t1.SupervisorfkCPersonal='" + cmbSupervisores.SelectedValue + "'");
                    if (cmbBuscStatus.SelectedIndex > 0)
                        wheres = (wheres == "" ? " WHERE t2.estatus='" + cmbBuscStatus.SelectedValue + "'" : wheres += " AND t2.estatus='" + cmbBuscStatus.SelectedValue + "'");
                    if (cmbMeses.SelectedIndex > 0)
                        wheres = (wheres == "" ? " WHERE (date_format(t1.fechareporte,'%c')='" + cmbMeses.SelectedValue + "' and date_format(t1.fechareporte,'%Y')=date_format(now(),'%Y'))" : wheres += " AND (date_format(t1.fechareporte,'%c')='" + cmbMeses.SelectedValue + "' and date_format(t1.fechareporte,'%Y')=date_format(now(),'%Y'))");
                    if (cmbEmpresa.SelectedIndex > 0)
                        wheres = (wheres == "" ? " WHERE T5.idempresa='" + cmbEmpresa.SelectedValue + "'" : wheres += " and T5.idempresa='" + cmbEmpresa.SelectedValue + "'");
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
                    v.c.dbconection().Close();
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
            cmbCodFallo.Enabled = bandera = editar = mensaje = btnEditar.Visible = lblactualizar.Visible = btnpdf.Visible = resmod = LblPDF.Visible = cmbServicio.Enabled = false;
            DgvTabla.ClearSelection();
            btnGuardar.Visible = LblGuardar.Visible = true;
            txtKilometraje.MaxLength = 12;
            esta_exportando();
        }

        private void To_pdf()//Método generar PDF
        {
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';Select upper(concat( t1.Folio,'|',concat(t4.identificador,LPAD(consecutivo,4,'0')),'|',date_format(t1.FechaReporte,'%W %d de %M del %Y'),'|',(select concat(coalesce(x1.ApPaterno,''),' ',coalesce(x1.ApMaterno,''),' ',coalesce(x1.nombres,'')) from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal),'|',(select credencial from cpersonal as x2 where x2.idpersona=t1.CredencialConductorfkCPersonal),'|',if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios)),'|',t1.HoraEntrada,'|',t1.kmEntrada,'|',t1.tipoFallo,'|',if(t1.DescrFallofkcdescfallo is null,t1.DescFalloNoCod,concat(t1.DescrFallofkcdescfallo,'|',t1.CodFallofkcfallosesp)),'|',coalesce(t1.ObservacionesSupervision,''),'|',coalesce(concat(date_format(t5.FechaHoraI,'%W %d de %M del %Y'),' / ',time_format(t5.FechaHoraI,'%H:%i')),''),'|',coalesce(date_format(t5.FechaHoraT,'%W %d de %M del %Y / %H:%i'),''),'|',coalesce(t5.Estatus,''),'|',coalesce(t5.TrabajoRealizado,''),'|',coalesce((select concat(coalesce(x5.appaterno,''),' ',coalesce(x5.apmaterno,''),' ',coalesce(x5.nombres,'')) from cpersonal as x5 where t5.MecanicofkPersonal=x5.idpersona),''),'|',coalesce(t5.ObservacionesM,''))) as r from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas as t4 on t4.idarea=t2.areafkcareas left join reportemantenimiento as t5 on t5.FoliofkSupervicion=t1.idReporteSupervicion WHERE t1.folio='" + lblFolio.Text + "'").ToString().Split('|');
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
                    bool haveFallo = (datos.Length > 17);
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
                        tabla.AddCell(v.valorCampo("", 3, 1, 0, arial));
                        tabla.AddCell(v.valorCampo(v.getaData("select upper(t1.nombreFalloGral) from cfallosgrales as t1 inner join cdescfallo as t2 on t1.idFalloGral=t2.falloGralfkcfallosgrales where t2.iddescfallo='" + datos[9] + "'").ToString(), 1, 0, 0, arial));
                        tabla.AddCell(v.valorCampo(v.getaData("select upper(descfallo) from cdescfallo  where iddescfallo='" + datos[9] + "';").ToString(), 1, 0, 0, arial));
                        tabla.AddCell(v.valorCampo(v.getaData("select upper(t1.categoria) from catcategorias as t1 inner join cdescfallo as t2 on t2.iddescfallo=t1.subgrupofkcdescfallo where t2.iddescfallo='" + datos[9] + "';").ToString(), 1, 0, 0, arial));
                        tabla.AddCell(v.valorCampo("\n", 3, 1, 0, arial));
                        tabla.AddCell(v.valorCampo("CÓDIGO DE FALLO:", 1, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo("NOMBRE DE FALLO:", 2, 0, 0, arial2));
                        tabla.AddCell(v.valorCampo("", 3, 1, 0, arial));
                        tabla.AddCell(v.valorCampo(v.getaData("select upper(codfallo) from cfallosesp where idfalloEsp='" + datos[10] + "';").ToString(), 1, 0, 0, arial));
                        tabla.AddCell(v.valorCampo(v.getaData("select upper(falloesp) from cfallosesp where idfalloEsp='" + datos[10] + "';").ToString(), 2, 0, 0, arial));
                    }
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("OBSERVACIONES DE SUPERVISIÓN:", 3, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo((!haveFallo ? datos[10] : datos[11]), 3, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("DATOS DE MANTENIMIENTO", 3, 0, 0, FontFactory.GetFont("ARIAL", 18, iTextSharp.text.Font.BOLD)));
                    tabla.AddCell(v.valorCampo("\n\n", 3, 1, 0, arial));
                    PdfPTable tabla2 = new PdfPTable(2);
                    tabla2.DefaultCell.Border = 0;
                    tabla2.WidthPercentage = 100;
                    tabla2.AddCell(v.valorCampo("FECHA/HORA DE INICIO:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("FECHA/HORA DE LIBERACIÓN:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[11] : datos[12]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[12] : datos[13]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("\n", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("TIEMPO DE ESPERA:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("TIEMPO DE MANTENIMIENTO:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo(v.timetowait(DateTime.Parse(v.getaData("select concat(fechareporte,' ',HoraEntrada) from reportesupervicion where idReporteSupervicion='" + IdRepor + "';").ToString()), DateTime.Parse(v.getaData("select FechaHoraI from reportemantenimiento where FoliofkSupervicion='" + IdRepor + "';").ToString())), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo(v.timetowait(DateTime.Parse(v.getaData("select concat(fechareporte,' ',HoraEntrada) from reportesupervicion where idReporteSupervicion='" + IdRepor + "';").ToString()), DateTime.Parse(v.getaData("select FechaHoraT from reportemantenimiento where FoliofkSupervicion='" + IdRepor + "';").ToString())), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("\n", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("ESTATUS DE UNIDAD:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("TRABAJO REALIZADO:", 1, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo(v.changestatus(Convert.ToInt32(!haveFallo ? datos[13] : datos[14])), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[14] : datos[15]), 1, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("\n", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("MECÁNICO QUE REALIZÓ EL MANTENIMIENTO:", 2, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[15] : datos[16]), 2, 0, 0, arial));
                    tabla2.AddCell(v.valorCampo("\n", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo("OBSERVACIONES DE MANTENIMIENTO:", 2, 0, 0, arial2));
                    tabla2.AddCell(v.valorCampo("", 2, 1, 0, arial));
                    tabla2.AddCell(v.valorCampo((!haveFallo ? datos[16] : datos[17]), 2, 0, 0, arial));
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
        public void DeshabilitarCampos() { txtSupervisor.Enabled = txtConductor.Enabled = cmbServicio.Enabled = txtKilometraje.Enabled = cmbTipoFallo.Enabled = cbgrupo.Enabled = cbcategoria.Enabled = cbSubGrupo.Enabled = cmbCodFallo.Enabled = txtDescFalloNoC.Enabled = txtObserSupervicion.Enabled = false; }
        void restaurar_datos(DataGridViewCellEventArgs e)
        {
            if (DgvTabla.Rows.Count > 0)
            {
                editar = !(btnGuardar.Visible = LblGuardar.Visible = false);
                lblFolio.Text = DgvTabla.Rows[e.RowIndex].Cells[0].Value.ToString();
                string[] datos = v.getaData("SET lc_time_names = 'es_ES';Select upper(concat(t1.UnidadfkCUnidades,'|',date_format(t1.FechaReporte,'%W %d de %M del %Y'),'|',t1.CredencialConductorfkCPersonal,'|',t1.SupervisorfkCPersonal,'|',t1.Serviciofkcservicios,'|',t1.kmEntrada,'|',t1.tipoFallo,'|',coalesce(t1.DescrFallofkcdescfallo,0),'|',coalesce(t1.CodFallofkcfallosesp,0),'|',coalesce(t1.DescFalloNoCod,''),'|',coalesce(t1.ObservacionesSupervision,''),'|',coalesce(date_format(t5.FechaHoraI,'%W %d de %M del %Y / %H:%i'),''),'|',coalesce(date_format(t5.FechaHoraT,'%W %d de %M del %Y / %H:%i'),''),'|',coalesce(t5.Estatus,''),'|',coalesce(t5.TrabajoRealizado,''),'|',coalesce((select concat(coalesce(x5.appaterno,''),' ',coalesce(x5.apmaterno,''),' ',coalesce(x5.nombres,'')) from cpersonal as x5 where t5.MecanicofkPersonal=x5.idpersona),''),'|',coalesce(t5.ObservacionesM,''))) as r from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas as t4 on t4.idarea=t2.areafkcareas left join reportemantenimiento as t5 on t5.FoliofkSupervicion=t1.idReporteSupervicion WHERE Folio='" + lblFolio.Text + "';").ToString().Split('|');

                IdRepor = Convert.ToInt32(v.getaData("SELECT idreportesupervicion FROM reportesupervicion WHERE folio='" + lblFolio.Text + "'").ToString());
                cmbUnidad.SelectedValue = unidadAnterior = Convert.ToInt32(datos[0]);
                lblFechaReporte.Text = fechaAnterior = datos[1];
                lblSupervisor.Text = v.getaData("select concat(coalesce(appaterno,''),' ',coalesce(apmaterno,''),' ',coalesce(nombres,'')) from cpersonal where idpersona='" + (supervisorAnterior = Convert.ToInt32(datos[3])) + "'").ToString();
                txtConductor.Text = v.getaData("select if(count(credencial) > 0 ,coalesce(credencial,'0'), '') as valor from cpersonal where idpersona='" + (idconductor = conductorAnterior = Convert.ToInt32(datos[2])) + "'").ToString();
                if (Convert.ToInt32(v.getaData("select status from cunidades where idunidad='" + unidadAnterior + "';")) == 0)
                    v.iniCombos("SELECT t1.idunidad,concat(coalesce(t2.identificador,''),LPAD(consecutivo,4,'0')) as ECo FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea where t1.status='1' or t1.idunidad='" + unidadAnterior + "' order by eco", cmbUnidad, "idunidad", "eco", "--SELECCIONE ECONÓMICO");
                if (Convert.ToInt32(v.getaData("select status from cservicios where idservicio='" + datos[4] + "';")) == 0)
                {
                    cmbBuscarUnidad.DataSource = null;
                    //t2.empresafkcempresas='" + empresa + "' and
                    DataTable dt = (DataTable)v.getData("select idservicio as id,upper(concat(Nombre,' ',descripcion)) as nombre from cservicios as t1 inner join careas as t2 on t1.AreafkCareas=t2.idarea where  ( t1.status='1' and (select areafkcareas from cunidades where idunidad='" + cmbUnidad.SelectedValue + "')=t1.AreafkCareas) or(idservicio='" + datos[4] + "') order by nombre;");
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
                    v.iniCombos("select iddescfallo as id,upper(descfallo) as d from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idfallogral=t1.falloGralfkcfallosgrales where t2.idfallogral='" + cbgrupo.SelectedValue + "' and t1.status='1' or(t1.iddescfallo='" + datos[7] + "') order by descfallo;", cbSubGrupo, "id", "d", "--SELECCIONE SUBGRUPO--");
                if (Convert.ToInt32(v.getaData("select status from catcategorias where idcategoria='" + (categoriaAnterior = Convert.ToInt32(v.getaData("select coalesce(idcategoria,0) from catcategorias where subgrupofkcdescfallo='" + datos[7] + "';"))) + "'")) == 0)
                    v.iniCombos("select t3.idcategoria as id ,upper(t3.categoria) as c from cdescfallo as t1 inner join catcategorias as t3 on t3.subgrupofkcdescfallo=t1.iddescfallo where iddescfallo='" + Convert.ToInt32(datos[7]) + "' order by categoria;", cbcategoria, "id", "c", "--SELECCIONE CATEGORIA--");
                if (Convert.ToInt32(v.getaData("select status from cfallosesp where idfalloEsp='" + (codigoAnterior = Convert.ToInt32(datos[8])) + "';")) == 0)
                    v.iniCombos("select t1.idfalloEsp as id,upper(t1.falloesp) as c from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t2.idcategoria='" + categoriaAnterior + "' order by c asc;", cmbCodFallo, "id", "c", "--SELECCIONE CÓDIGO");
                cbgrupo.SelectedValue = grupoFalloAnterior;
                cbSubGrupo.SelectedValue = subGrupoAnterior = Convert.ToInt32(datos[7]);
                cbcategoria.SelectedValue = categoriaAnterior;
                cmbCodFallo.SelectedValue = codigoAnterior;
                txtDescFalloNoC.Text = FalloAnterior = datos[9];
                txtObserSupervicion.Text = ObservacionesAnterior = datos[10];
                lblHIM.Text = datos[11];
                lblHTM.Text = datos[12];
                lblEsperaDeMan.Text = (!string.IsNullOrWhiteSpace(datos[11]) ? v.timetowait(DateTime.Parse(v.getaData("select concat(FechaReporte,' ',HoraEntrada) from reportesupervicion where idReporteSupervicion='" + IdRepor + "';").ToString()), DateTime.Parse(v.getaData("select FechaHoraI from reportemantenimiento where FoliofkSupervicion='" + IdRepor + "';").ToString())) : "");
                lblTM.Text = (!string.IsNullOrWhiteSpace(datos[12]) ? v.timetowait(DateTime.Parse(v.getaData("select concat(FechaReporte,' ',HoraEntrada) from reportesupervicion where idReporteSupervicion='" + IdRepor + "';").ToString()), DateTime.Parse(v.getaData("select FechaHoraT from reportemantenimiento where FoliofkSupervicion='" + IdRepor + "';").ToString())) : "");
                lblestatus.Text = (string.IsNullOrWhiteSpace(datos[13]) ? "" : v.changestatus(Convert.ToInt32(datos[13])));
                LblTrabajoRealizado.Text = datos[14];
                lblMecanico.Text = datos[15];
                LblObsevacionesMantenimiento.Text = datos[16];
                btnEditar.Visible = lblactualizar.Visible = false;
                btnpdf.Visible = LblPDF.Visible = (Convert.ToInt32((string.IsNullOrWhiteSpace(datos[13]) ? "0" : datos[13])) == 3 ? true : false);
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
                        if (lblestatus.Text == "LIBERADA")
                        {
                            if (pinsertar && peditar && pconsultar)
                            {
                                cmbUnidad.Enabled = true;
                                HabilitarCampos();
                            }
                            else
                                DeshabilitarCampos();
                        }
                        else
                        {
                            HabilitarCampos();
                            txtKilometraje.MaxLength = 12;
                        }
                    }
                }
                else
                    MessageBox.Show("No cuenta con los privilegios para editar un reporte".ToUpper(), "SIN PRIVILEGIOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void oculta_botones()
        {
            btnEditar.Visible = lblactualizar.Visible = btnpdf.Visible = LblPDF.Visible = false;
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
                {
                    // v.c.writemodification(v.Encriptar(consulta));
                    MessageBox.Show("Registro actualizado exitosamente ".ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
            LblExcel.Text = "Exportando";
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
            //0.-select iddescfallo as id,upper(descfallo) as d from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idfallogral=t1.falloGralfkcfallosgrales where t2.idfallogral='" + cbgrupo.SelectedValue + "' order by descfallo;
            //1.-select iddescfallo as id,upper(descfallo) as d from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idfallogral=t1.falloGralfkcfallosgrales where t2.idfallogral='" + cbgrupo.SelectedValue + "' group by d order by d
            v.iniCombos("select iddescfallo as id, upper(descfallo) as d from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idfallogral=t1.falloGralfkcfallosgrales where t2.idfallogral='" + cbgrupo.SelectedValue + "' group by d order by d", cbSubGrupo, "id", "d", "--SELECCIONE SUBGRUPO--");
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
            //Select UPPER(coalesce(falloesp,'')) as fallo from cfallosesp where idfalloEsp
            lblDescFallo.Text = (cmbCodFallo.SelectedIndex > 0 ? v.getaData("Select UPPER(coalesce(codfallo,'')) as fallo from cfallosesp where idfalloEsp='" + cmbCodFallo.SelectedValue + "'").ToString() : "");
        }
        public void consulta_codigos()
        {
            v.iniCombos("select t1.idfalloEsp as id,upper(t1.falloesp) as c from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t2.idcategoria='" + cbcategoria.SelectedValue + "' order by c asc;", cmbCodFallo, "id", "c", "--SELECCIONE CÓDIGO");
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
            //hilo.Abort();
        }
        private void cmbTipoFallo_DrawItem(object sender, DrawItemEventArgs e)
        {
            System.Drawing.Color c = System.Drawing.Color.BlueViolet;
            System.Drawing.Color color_fuente = System.Drawing.Color.FromArgb(75, 44, 52);
            System.Drawing.Color color = System.Drawing.Color.FromArgb(246, 144, 123);
            SolidBrush s = new SolidBrush(color);
            System.Drawing.Color fondo = System.Drawing.Color.FromArgb(200, 200, 200);
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
                e.Graphics.DrawString(dt.Rows[e.Index].ItemArray[1].ToString(), e.Font, new SolidBrush(System.Drawing.Color.White), e.Bounds, sf);
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
            cmbServicio.DataSource = null;
            //t2.empresafkcempresas='" + empresa + "' and
            DataTable dt = (DataTable)v.getData("select idservicio as id, concat(t2.Nombre, ' ', t2.Descripcion) as nombre from cunidades as t1 inner join cservicios as t2 on t1.serviciofkcservicios = t2.idservicio where t1.idunidad='" + cmbUnidad.SelectedValue + "' order by nombre");
            //DataTable dt = (DataTable)v.getData("select idservicio as id,upper(concat(Nombre,' ',descripcion)) as nombre from cservicios as t1 inner join careas as t2 on t1.AreafkCareas=t2.idarea where t1.status='1' and (select areafkcareas from cunidades where idunidad='" + cmbUnidad.SelectedValue + "')=t1.AreafkCareas order by nombre;");
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
            cmbServicio.Enabled = (cmbUnidad.SelectedIndex == 0 ? false : true);
        }
        public void TextoLargo(object sender, EventArgs e)
        {
            ((Label)sender).Font = ((((Label)sender).Text.Length >= 30) ? new System.Drawing.Font("Garamond", 10) : new System.Drawing.Font("Garamond", 12));
        }
        public void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, System.Drawing.Color.FromArgb(75, 44, 52), System.Drawing.Color.FromArgb(75, 44, 52), this);
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            realiza_busquedas();
        }
        void esta_exportando()
        {
            if (peditar && pinsertar && pconsultar)
                if (LblExcel.Text.Equals("Exportando"))
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
            //estado = true;
            // exportar = new Thread(new ThreadStart(exporta_a_excel));
            // exportar.Start();

            exporta_a_excel();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validaciones de campos vacios al momento de dar click al boton guardar
            if (v.campossupervision(lblFolio.Text, cmbUnidad.SelectedIndex, lblSupervisor.Text, lblCredCond.Text, cmbServicio.SelectedIndex, txtKilometraje.Text, cmbTipoFallo.SelectedIndex, cbgrupo.SelectedIndex, cbSubGrupo.SelectedIndex, cbcategoria.SelectedIndex, cmbCodFallo.SelectedIndex, txtDescFalloNoC.Text))
            {
                string campos = "Insert into reportesupervicion (Folio,UnidadfkCUnidades,FechaReporte, SupervisorfkCPersonal, CredencialConductorfkCPersonal, Serviciofkcservicios,HoraEntrada,KmEntrada,TipoFallo,ObservacionesSupervision";
                string valores = "('" + lblFolio.Text + "' , '" + cmbUnidad.SelectedValue + "' ,(select curdate()) , '" + Convert.ToInt32(idsupervisor) + "' , '" + Convert.ToInt32(idconductor) + "' , '" + cmbServicio.SelectedValue + "',(select curtime()) , '" + txtKilometraje.Text + "' , '" + cmbTipoFallo.SelectedValue + "','" + txtObserSupervicion.Text.Trim() + "' ";
                campos = (cbgrupo.SelectedIndex == 0 ? campos += ",DescFalloNoCod)" : campos += ",DescrFallofkcdescfallo,CodFallofkcfallosesp)");
                valores = (cbgrupo.SelectedIndex == 0 ? valores += " ,'" + txtDescFalloNoC.Text.Trim() + "')" : valores += " ,'" + cbSubGrupo.SelectedValue + "','" + cmbCodFallo.SelectedValue + "')");
                if (v.c.insertar(campos + " values " + valores))
                {
                    ////// v.c.writemodification(campos + " values " + valores);
                    ////MessageBox.Show("Reporte guardado exitosamente ", "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //if (MessageBox.Show("¿Desea continuar con la unidad actual?", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                    //{
                    //    esta_exportando();
                    //    limpia_act();
                    //    cargarDAtos();
                    //    Genera_Folio();
                    //    LimpiarReporte();
                    //}
                    //else
                    //{
                    //    cbgrupo.SelectedIndex = cbSubGrupo.SelectedIndex = 0; cbcategoria.SelectedIndex = cmbCodFallo.SelectedIndex = -1; txtObserSupervicion.Text = txtDescFalloNoC.Text = lblDescFallo.Text = "";
                    //    cargarDAtos();
                    //    Genera_Folio();
                    //}
                    var selectedOption = MessageBox.Show("¿Desea continuar creando folios de la unidad actual?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (selectedOption == DialogResult.Yes)
                    {
                        cbgrupo.SelectedIndex = 0; cbcategoria.SelectedIndex = cmbCodFallo.SelectedIndex = cbSubGrupo.SelectedIndex = -1; txtObserSupervicion.Text = txtDescFalloNoC.Text = lblDescFallo.Text = "";
                        cargarDAtos();
                        Genera_Folio();
                        Inhabilita_k(false);
                    }
                    else if (selectedOption == DialogResult.No)
                    {
                        esta_exportando();
                        limpia_act();
                        cargarDAtos();
                        Genera_Folio();
                        LimpiarReporte();
                        Inhabilita_k(true);
                    }

                }
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
            ((Button)sender).Size = new Size(47, 47);
        }

        private void GpbSupervisión_Enter(object sender, EventArgs e)
        {
            Inhabilita_k(true);
        }

        private void GpbBusquedas_Enter(object sender, EventArgs e)
        {

        }

        private void btnGuardar_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).Size = new Size(45, 45);
        }
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "Exportar";
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

            /*
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
            foreach (DataGridViewColumn column in DgvTabla.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;


            }



            int ir = 9;
            foreach (DataGridViewRow row in DgvTabla.Rows)
            {

                sl.SetCellValue(ir, 2, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[5].Value.ToString());
                sl.SetCellValue(ir, 8, row.Cells[6].Value.ToString());
                sl.SetCellValue(ir, 9, row.Cells[7].Value.ToString());
                sl.SetCellValue(ir, 10, row.Cells[8].Value.ToString());
                sl.SetCellValue(ir, 11, row.Cells[9].Value.ToString());
                sl.SetCellValue(ir, 12, row.Cells[10].Value.ToString());
                sl.SetCellValue(ir, 13, row.Cells[11].Value.ToString());
                sl.SetCellValue(ir, 14, row.Cells[12].Value.ToString());
                sl.SetCellValue(ir, 15, row.Cells[13].Value.ToString());
                sl.SetCellValue(ir, 16, row.Cells[14].Value.ToString());
                sl.SetCellValue(ir, 17, row.Cells[15].Value.ToString());
                sl.SetCellValue(ir, 18, row.Cells[16].Value.ToString());
                sl.SetCellValue(ir, 19, row.Cells[17].Value.ToString());
                sl.SetCellValue(ir, 20, row.Cells[18].Value.ToString());
               

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
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Reporte Supervision");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "T" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "CONSULTA REPORTE DE SUPERVISION");
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
            sl.SetCellStyle("B" + celdaInicial, "T" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "T");
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
        private void cmbBuscStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            System.Drawing.Color c = System.Drawing.Color.BlueViolet;
            System.Drawing.Color color_fuente = System.Drawing.Color.FromArgb(75, 44, 52);
            System.Drawing.Color color = System.Drawing.Color.FromArgb(246, 144, 123);
            SolidBrush s = new SolidBrush(color);
            System.Drawing.Color fondo = System.Drawing.Color.FromArgb(200, 200, 200);
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
                e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(System.Drawing.Color.White), e.Bounds, sf);
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
            if (editar && (idconductor != conductorAnterior || Convert.ToInt32(cmbServicio.SelectedValue) != servicioAnterior || txtKilometraje.Text != kilometrajeAnterior || Convert.ToInt32(cmbTipoFallo.SelectedValue) != TipoFalloAnterior || Convert.ToInt32(grupof) != grupoFalloAnterior || Convert.ToInt32(subgrupof) != subGrupoAnterior || Convert.ToInt32(categoriaf) != categoriaAnterior || Convert.ToInt32(codf) != codigoAnterior || txtDescFalloNoC.Text != FalloAnterior || ObservacionesAnterior != txtObserSupervicion.Text) && idconductor > 0 && cmbServicio.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtKilometraje.Text) && cmbTipoFallo.SelectedIndex > 0 && ((cbgrupo.SelectedIndex > 0 && cbSubGrupo.SelectedIndex > 0 && cbcategoria.SelectedIndex > 0 && cmbCodFallo.SelectedIndex > 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text) || (!string.IsNullOrWhiteSpace(txtDescFalloNoC.Text) && cbgrupo.SelectedIndex == 0))))
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

            //v.iniCombos("select " + v.c.fieldsfallosgrales[0] + " as id, upper(" + v.c.fieldsfallosgrales[1] + ") as grupo from cfallosgrales where " + v.c.fieldsfallosgrales[3] + "='" + empresa + "';", cmbgrupo, "id", "grupo", "--SELECCIONE UN GRUPO--");

        }
        public void consulta_descripciones()
        {
            v.iniCombos("select idFalloGral as id,upper(nombreFalloGral) as n from cfallosgrales where status='1' order by nombreFalloGral;", cbgrupo, "id", "n", "--SELECCIONE GRUPO--");
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

                if (res)
                {
                    var selectedOption = MessageBox.Show("¿Desea guardar las modificaciones ?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (selectedOption == DialogResult.Yes)
                    {
                        resmod = true;
                        mensaje = true;
                        actualiza_datos();
                        btnpdf.Visible = LblPDF.Visible = btnEditar.Visible = lblactualizar.Visible = false;
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
                else
                {
                    var selectedOption = MessageBox.Show("¿Desea concluir el reporte?", "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (selectedOption == DialogResult.Yes)
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

                /*if (MessageBox.Show((res ? : "¿Desea concluir el reporte?"), , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    resmod = true;
                    if (res)
                    {
                        
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
                }*/
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
            Inhabilita_k(true);
            //LimpiarReporte();
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
                    idsupervisor = Convert.ToInt32(v.getaData("call sistrefaccmant.namewithpassword('" + v.Encriptar(txtSupervisor.Text) + "','" + empresa + "','" + area + "')").ToString());
                    lblSupervisor.Text = ((pinsertar || peditar) ? v.getaData("select concat(coalesce(appaterno,''),' ',coalesce(apmaterno,''),' ',coalesce(nombres,'')) from cpersonal where idpersona='" + idsupervisor + "';").ToString() : "");
                }
                else
                {
                    idsupervisor = 0;
                    lblSupervisor.Text = "";
                }
            }
        }
        private void txtConductor_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtConductor.Text))
            {
                if (Convert.ToInt32(v.getaData("select count(*) from cpersonal where credencial='" + txtConductor.Text + "' and empresa='" + empresa + "' and area='" + area + "' and status='1'")) > 0)
                {
                    string[] datos = v.getaData("select UPPER(concat(coalesce(t1.ApPaterno,''),' ',coalesce(t1.ApMaterno,''),' ',coalesce(t1.nombres,''),'|',idpersona)) from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.credencial='" + txtConductor.Text + "' AND t1.status='1'   AND t2.status='1' and t1.empresa='1' and t1.area='1';").ToString().Split('|');
                    lblCredCond.Text = (!string.IsNullOrWhiteSpace(datos[0]) ? datos[0] : "");
                    idconductor = (!string.IsNullOrWhiteSpace(datos[1]) ? Convert.ToInt32(datos[1]) : 0);
                }
                else
                {
                    txtConductor.Text = lblCredCond.Text = "";
                    idconductor = 0;
                }
            }
        }
        private void dtpFechaA_KeyDown(object sender, KeyEventArgs e) { e.SuppressKeyPress = true; }
        private void dataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.DgvTabla.Columns[e.ColumnIndex].Name == "TIPO DE FALLO")
                e.CellStyle.BackColor = (Convert.ToString(e.Value) == "PREVENTIVO" ? System.Drawing.Color.PaleGreen : Convert.ToString(e.Value) == "CORRECTIVO" ? System.Drawing.Color.Khaki : Convert.ToString(e.Value) == "REITERATIVO" ? System.Drawing.Color.LightCoral : Convert.ToString(e.Value) == "REPROGRAMADO" ? System.Drawing.Color.LightBlue : System.Drawing.Color.FromArgb(246, 144, 123));
            if (this.DgvTabla.Columns[e.ColumnIndex].Name == "ESTATUS")
                e.CellStyle.BackColor = (Convert.ToString(e.Value) == "EN PROCESO" ? System.Drawing.Color.Khaki : Convert.ToString(e.Value) == "LIBERADA" ? System.Drawing.Color.PaleGreen : e.Value.ToString() == "REPROGRAMADA" ? System.Drawing.Color.LightCoral : System.Drawing.Color.FromArgb(200, 200, 200));
        }
    }
}