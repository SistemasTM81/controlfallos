using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using h = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace controlFallos
{
    public partial class FormFallasMantenimiento : Form
    {

        validaciones v;
        String folio, foliof, conta;
        public string idUsu;
        String month;

        /* VAR ANTERIORES */

        String reqrefanterior = "", mecanicoanterior = "", exisrefaccionanterior = "", fgeneralanterior = "", mecanicoapoanterior = "", folfacturanterior = "", trabrealizadoanterior = "", supervisoanterior = "", estatusmantanterior = "", observacionesmantanterior = "", familianterior = "", refaccionanterior = "";
        int idfgeneralanterior, idmecanicoanterior, idmecanicoapoanterior, idsupervisoanterior, idfamilianterior, idrefaccionanterior;
        double cantidadanterior;

        /* VARIABLES */

        String existenciaGV = "", estatusmantGV = "";
        String validacionfgeneral = "", validacionreqrefacc = "", validacionexisrefacc = "", validacionestatusmant = "", existenciaif = "", foliofacturarconsulta = "", unidadmedidaconsulta = "", nombrerefaccionconsulta = "";
        string idgeneral;
        System.Drawing.Image newimg;
        int idvalidacionfgeneral, idvalidacionreqrefacc, idvalidacionexisrefacc, empresa, area, cargo, idUsuario, idunidadmedidaconsulta, idreportemantenimiento, idreportesupervision, item_temporal, cantidadrefacciones, resultadopregunta, validacionexistenciarefacciones, faltante, totalfaltante, validacionconteo, abcebtn, enumeradorefacciones, cantidadtotalrefacciones, totalrefacciones, conteorefacciones, conteorefaccionesinicial, conteorefaccionesfinal, conteorefaccionesverificadas, totalexistenciarefaccinoes, inicolumn, fincolumn, valorfallogeneral;
        bool registroentradapedref = false, registroconteofilaspedref = false, banderaeditar = false, botoncancelarvalidacion = false, res = true, validaciontablarefacciones = false, validacionfinalconteocolumnas = false, existencias = false;
        DateTime finiciomantenimiento, fterminomantenimiento, difhorasinicio, difhorasfinal;

        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }

        public Thread hilo;

        /* VARIABLES PDF */

        string foliopdf, unidadpdf, kilometrajepdf, codigofallopdf, fechahorapdf, descripcionfallopdf, descripcionfallonocodificadopdf, supervisorpdf, observacionesmantenimientopdf, grupofallopdf, estatuspdf, mecanicopdf, mapoyopdf, supervisopdf, tiempoesperapdf, horainicioterminopdf, diferenciapdf, trabajorealizadopdf, observacionesupervisionpdf, vinpdf, marcamodelopdf, nmotorpdf, ntransmisionpdf;
        string extensionarchivo = "", nombrearchivo = "", nombrearchivosinextension = "";

        void quitarseen()
        {
            while (res)
            {
                MySqlConnection dbcon = null;
                if (v.c.conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                else
                    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE reportesupervicion SET seen = 1 WHERE seen  = 0", dbcon);
                cmd.ExecuteNonQuery();
                dbcon.Close();
                Thread.Sleep(180000);
            }
        }

        public FormFallasMantenimiento(int idUsuario, int empresa, int area, System.Drawing.Image newimg, validaciones v)
        {
            InitializeComponent();
            this.v = v;
            comboBoxFalloGral.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxReqRefacc.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxExisRefacc.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxEstatusMant.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxFamilia.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxFRefaccion.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxUnidadB.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxMecanicoB.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxEstatusMB.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxDescpFalloB.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxMesB.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            ptbxLogo.Image = newimg;
        }

        private void FormFallasMantenimiento_Load(object sender, EventArgs e)
        {
            privilegios();
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();
            finiciomantenimiento.ToLongTimeString();
            dateTimePicker2.Format = dateTimePicker5.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = dateTimePicker5.CustomFormat = "HH:mm:ss";
            timer1.Start();
            timer2.Start();
            metodoCarga();
            v.c.dbcon.Close();
            conteo();
            v.c.dbcon.Close();
            actualizarcbx();
            AutoCompletado(textBoxFolioB);
            comboBoxEstatusMant.SelectedIndex = comboBoxReqRefacc.SelectedIndex = comboBoxExisRefacc.SelectedIndex = comboBoxMecanicoB.SelectedIndex = comboBoxEstatusMB.SelectedIndex = comboBoxMesB.SelectedIndex = 0;
            AutoCompletado(textBoxFolioB);
            dateTimePickerIni.Value = dateTimePickerFin.Value = DateTime.Now;
            ocultarexcel();
            if (pinsertar && pconsultar && peditar)
                label60.Visible = label61.Visible = true;
            else
                label60.Visible = label61.Visible = false;
            if (!checkBoxFechas.Checked)
                checkBoxFechas.ForeColor = checkBoxFechas.Checked ? Color.Crimson : Color.Crimson;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            dateTimePicker2.Text = DateTime.Now.ToLongTimeString();
        }

        /* Métodos Para Cargar Los ComboBox *//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Unidad()
        {
            v.iniCombos("SELECT distinct concat(t3.identificador, LPAD(t2.consecutivo, 4, '0')) as eco, t2.idunidad   FROM reportesupervicion as t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idUnidad  INNER JOIN careas as t3 On t2.areafkcareas = t3.idarea WHERE (SELECT empresaMantenimiento FROM cmodelos WHERE idmodelo = t2.modelofkcmodelos)='" + empresa + "' GROUP BY  t1.UnidadfkCUnidades ORDER BY concat(t3.identificador, LPAD(t2.consecutivo, 4, '0')) ASC ", comboBoxUnidadB, "idunidad", "eco", "-- económico --");
        }

        private void ClasFallo()
        {
            v.iniCombos("SELECT UPPER(nombreFalloGral) AS nombreFalloGral, idFalloGral FROM cfallosgrales WHERE status = 1 AND empresa='" + empresa + "' ORDER BY nombreFalloGral", comboBoxFalloGral, "idFalloGral", "nombreFalloGral", "-- GRUPO --");
        }

        private void Mecanico()
        {
            v.iniCombos("SELECT DISTINCT UPPER(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres)) AS Nombre, t2.idPersona FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.MecanicofkPersonal=t2.idpersona GROUP BY MecanicofkPersonal ORDER BY CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres) asc;", comboBoxMecanicoB, "idPersona", "Nombre", "-- MECÁNICO --");
        }

        private void DescripFallo()
        {
            v.iniCombos("SELECT DISTINCT Upper((t3.descfallo)) as descfallo,(t3.iddescfallo) FROM reportesupervicion as t1 INNER JOIN cfallosesp as t2 ON t1.CodFallofkcfallosesp = t2.idfalloesp INNER JOIN cdescfallo as t3 ON t2.descfallofkcdescfallo=t3.iddescfallo WHERE CodFallofkcfallosesp is not null AND t3.empresa='" + empresa + "' GROUP BY CodFallofkcfallosesp order by t3.descfallo asc;", comboBoxDescpFalloB, "iddescfallo", "descfallo", "--SELECCIONE DESCRIPCIÓN--");
        }

        private void FamiliaRef()
        {
            v.iniCombos("SELECT UPPER(familia) AS familia, idcnfamilia as idfamilia FROM cnfamilias WHERE status = '1' AND empresa='" + empresa + "' ORDER BY familia", comboBoxFamilia, "idfamilia", "familia", "-- FAMILIA --");
        }

        private void RefaccPed()
        {
            v.iniCombos("SELECT UPPER(t2.nombreRefaccion) AS 'nombreRefaccion', t2.idrefaccion FROM cfamilias AS t1 INNER JOIN crefacciones AS t2 ON t1.idfamilia = t2.familiafkcfamilias WHERE t2.familiafkcfamilias = '" + comboBoxFamilia.SelectedValue + "' t2.empresa='" + empresa + "' GROUP BY t2.nombreRefaccion ORDER BY t2.nombreRefaccion", comboBoxFRefaccion, "idrefaccion", "nombreRefaccion", "-- REFACCION --");
        }

        /* Todos los métodos */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void enabledfalse()
        {
            comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = comboBoxReqRefacc.Enabled = comboBoxExisRefacc.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonAgregar.Visible = label39.Visible = buttonGuardar.Visible = label24.Visible = buttonEditar.Visible = label58.Visible = buttonExcel.Visible = label35.Visible = false;
        }

        public void valexcel()
        {
            if (hiloEx.IsAlive)
                abcebtn = 1;
            else
                abcebtn = 0;
        }

        public void actualizarcbx()
        {
            Unidad();
            ClasFallo();
            Mecanico();
            DescripFallo();
            FamiliaRef();
        }

        public void mostrarexcel()
        {
            if (pconsultar && pinsertar && peditar)
                buttonExcel.Visible = label35.Visible = true;
            else
                buttonExcel.Visible = label35.Visible = false;
        }

        public void ocultarexcel()
        {
            if (buttonExcel.Visible)
                buttonExcel.Visible = label35.Visible = false;
        }

        public void validacioneditar()
        {
            if (banderaeditar)
            {
                valdeval();
                if ((((fgeneralanterior == validacionfgeneral) || (comboBoxFalloGral.SelectedIndex == 0)) && ((trabrealizadoanterior.Trim() == textBoxTrabajoRealizado.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text.Trim()))) && ((folfacturanterior == textBoxFolioFactura.Text.Trim()) || (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text.Trim()))) && ((observacionesmantanterior == textBoxObsMan.Text.Trim()))))
                    buttonEditar.Visible = label58.Visible = false;
                else
                    buttonEditar.Visible = label58.Visible = true;
            }
        }

        public void codedicion()
        {
            actualizarcbx();
            AutoCompletado(textBoxFolioB);
            label1.Visible = groupBoxRefacciones.Visible = false;
            gbxMantenimiento.Visible = true;
            timer2.Start();
            limpiarcampos();
            limpiarcamposbus();
            comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = comboBoxReqRefacc.Enabled = comboBoxExisRefacc.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = textBoxSuperviso.Enabled = buttonGuardar.Visible = label24.Visible = buttonEditar.Visible = label58.Visible = buttonExcel.Visible = label35.Visible = buttonAgregar.Visible = label39.Visible = false;
            textBoxObsMan.Enabled = true;
            if (pinsertar && pconsultar && peditar)
                buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = true;
            else
                buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = false;
            labelFolio.Text = dgvMantenimiento.CurrentRow.Cells["FOLIO"].Value.ToString();
            estatusmantGV = dgvMantenimiento.CurrentRow.Cells["ESTATUS DEL MANTENIMIENTO"].Value.ToString();
            if (!(estatusmantGV.Equals("")))
            {
                MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT coalesce((SELECT r25.FoliofkSupervicion FROM reportemantenimiento AS r25 WHERE r25.FoliofkSupervicion = t1.idReporteSupervicion), '') AS Folio, coalesce((SELECT r25.IdReporte FROM reportemantenimiento AS r25 WHERE t1.idReporteSupervicion = r25.FoliofkSupervicion), '') AS IdMantenimiento, coalesce((SELECT r22.MecanicofkPersonal FROM reportemantenimiento AS r22 WHERE t1.idReporteSupervicion = r22.FoliofkSupervicion), 0) AS IdMecanico, coalesce((SELECT r23.MecanicoApoyofkPersonal FROM reportemantenimiento AS r23 WHERE t1.idReporteSupervicion = r23.FoliofkSupervicion), 0) AS IdMecanicoApoyo, coalesce((SELECT r24.SupervisofkPersonal FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), 0) AS IdSuperviso, t1.idReporteSupervicion AS ID, t1.Folio, CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'Fecha Del Reporte', coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, t1.HoraEntrada AS 'Hora De Entrada', t1.KmEntrada AS 'Kilometraje', UPPER(t1.TipoFallo) AS 'Tipo De Fallo', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'Descripcion De Fallo', coalesce((SELECT UPPER(r22.codfallo) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'Codigo De Fallo', coalesce((UPPER(t1.DescFalloNoCod)), '') AS 'Descripcion De Fallo No Codificado', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'Observaciones De Supervision', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'Fallo General', coalesce((SELECT UPPER(r4.idfallogral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), 0) AS 'IdFG',coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'Trabajo Realizado', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'Mecanico', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'Mecanico De Apoyo', coalesce((SELECT UPPER(DATE_FORMAT(r10.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r10 WHERE t1.idReporteSupervicion = r10.FoliofkSupervicion), '') AS 'Fecha Del Reporte De Mantenimiento', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'Hora De Inicio De Mantenimiento', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'Hora De Termino De Mantenimiento', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'Espera De Tiempo Para Mantenimiento', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'Diferencia De Tiempo En Mantenimiento', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'Folio De Factura', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'Estatus Del Mantenimiento', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'Superviso', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'Existencia De Refacciones En Almacen', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'Estatus De Refacciones', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'Observaciones Del Mantenimiento' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN careas AS t4 ON t2.areafkcareas= t4.idarea WHERE t1.Folio = '" + labelFolio.Text + "'", v.c.dbconection());
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    idreportemantenimiento = Convert.ToInt32(dr.GetString("IdMantenimiento"));
                    idreportesupervision = Convert.ToInt32(dr.GetString("ID"));

                    /* SUPERVISIÓN */

                    labelUnidad.Text = dr.GetString("ECO");
                    labelFechaReporte.Text = dr.GetString("Fecha Del Reporte");
                    labelHoraReporte.Text = dr.GetString("Hora De Entrada");
                    labelKm.Text = dr.GetString("Kilometraje");
                    labelSupervisor.Text = dr.GetString("Supervisor");
                    labelcodfallo.Text = dr.GetString("Codigo De Fallo");
                    string fallo = dr.GetString("Descripcion De Fallo No Codificado");
                    if (!string.IsNullOrWhiteSpace(fallo))
                        textBoxDescrpFallo.Text = fallo;
                    else
                        textBoxDescrpFallo.Text = dr.GetString("Fallo General") + "\t -" + dr.GetString("Descripcion De Fallo") + "\t -" + dr.GetString("Codigo De Fallo");
                    textBoxObsSup.Text = dr.GetString("Observaciones De Supervision");

                    /* MANTENIMIENTO */

                    folio = dr.GetString("Folio");
                    labelFecha.Text = dr.GetString("Fecha Del Reporte De Mantenimiento");
                    comboBoxReqRefacc.Text = dr.GetString("Estatus de Refacciones");
                    reqrefanterior = dr.GetString("Estatus de Refacciones");
                    if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                        reqrefanterior = "";
                    labelHoraInicioM.Text = dr.GetString("Hora De Inicio de Mantenimiento");
                    if ((string.IsNullOrWhiteSpace(labelHoraInicioM.Text)) || (labelHoraInicioM.Text == "00:00:00"))
                        timer1.Start();
                    else
                    {
                        timer1.Stop();
                        finiciomantenimiento = Convert.ToDateTime(dr.GetString("Hora De Inicio De Mantenimiento"));
                    }
                    comboBoxExisRefacc.Text = dr.GetString("Existencia De Refacciones En Almacen");
                    exisrefaccionanterior = dr.GetString("Existencia De Refacciones En Almacen");
                    if (comboBoxExisRefacc.SelectedIndex == 0)
                        exisrefaccionanterior = "";
                    comboBoxFalloGral.Enabled = true;
                    comboBoxFalloGral.Text = dr.GetString("Fallo General");
                    fgeneralanterior = dr.GetString("Fallo General");
                    idfgeneralanterior = Convert.ToInt32(dr.GetString("IdFG"));
                    if (comboBoxFalloGral.SelectedIndex == 0)
                        fgeneralanterior = "";
                    validarstatusfallagral(fgeneralanterior);
                    labelNomMecanico.Text = dr.GetString("mecanico");
                    mecanicoanterior = dr.GetString("mecanico");
                    idmecanicoanterior = Convert.ToInt32(dr.GetString("IdMecanico"));
                    if (string.IsNullOrWhiteSpace(mecanicoanterior))
                        labelNomMecanico.Text = mecanicoanterior = ".";
                    labelNomMecanicoApo.Text = dr.GetString("Mecanico De Apoyo");
                    idmecanicoapoanterior = Convert.ToInt32(dr.GetString("IdMecanicoApoyo"));
                    mecanicoapoanterior = dr.GetString("Mecanico De Apoyo");
                    if (string.IsNullOrWhiteSpace(mecanicoapoanterior))
                        labelNomMecanicoApo.Text = mecanicoapoanterior = "..";
                    mecanicoapoanterior = dr.GetString("Mecanico De Apoyo");
                    textBoxEsperaMan.Text = dr.GetString("Espera De Tiempo Para Mantenimiento");
                    textBoxFolioFactura.Text = dr.GetString("Folio De Factura");
                    folfacturanterior = dr.GetString("Folio De Factura");
                    textBoxTrabajoRealizado.Text = dr.GetString("Trabajo Realizado");
                    trabrealizadoanterior = dr.GetString("Trabajo Realizado");
                    labelNomSuperviso.Text = dr.GetString("Superviso");
                    supervisoanterior = dr.GetString("Superviso");
                    idsupervisoanterior = Convert.ToInt32(dr.GetString("IdSuperviso"));
                    if (string.IsNullOrWhiteSpace(supervisoanterior))
                        labelNomSuperviso.Text = supervisoanterior = "...";
                    textBoxObsMan.Text = dr.GetString("Observaciones Del Mantenimiento");
                    observacionesmantanterior = dr.GetString("Observaciones Del Mantenimiento");
                    conteoiniref();
                    comboBoxEstatusMant.Text = dr.GetString("Estatus Del Mantenimiento");
                    estatusmantanterior = dr.GetString("Estatus Del Mantenimiento");
                    if (comboBoxEstatusMant.SelectedIndex == 0)
                    {
                        comboBoxEstatusMant.SelectedIndex = 1;
                        estatusmantanterior = comboBoxEstatusMant.Text;
                    }
                    else if (estatusmantanterior.Equals("LIBERADA") == false)
                        Cancelar(true);
                    metodocargaref();
                    conteofinref();
                    ncontrefini();
                    if (estatusmantGV.Equals("LIBERADA"))
                    {
                        fterminomantenimiento = Convert.ToDateTime(dr.GetString("Hora De Termino De Mantenimiento"));
                        labelHoraTerminoM.Text = fterminomantenimiento.ToString("%H:%m:%s");
                        //dateTimePicker2.Value = dr.GetDateTime("Hora De Termino De Mantenimiento");
                        textBoxTerminoMan.Text = Convert.ToString(dr.GetString("Diferencia De Tiempo En Mantenimiento"));

                        if (!(string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)))
                            comboBoxFalloGral.Enabled = true;
                        if (!(string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                            textBoxTrabajoRealizado.Enabled = true;
                        else
                            textBoxTrabajoRealizado.Enabled = true;
                        if (!(string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                            textBoxFolioFactura.Enabled = true;
                        if ((string.IsNullOrWhiteSpace(textBoxObsMan.Text)))
                            textBoxObsMan.Enabled = true;
                        else
                            textBoxObsMan.Enabled = true;
                    }
                    else
                    {
                        if (!(string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)))
                            comboBoxFalloGral.Enabled = true;
                        if (!(string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                            textBoxTrabajoRealizado.Enabled = true;
                        if (!(string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                            textBoxFolioFactura.Enabled = true;
                        if (!(string.IsNullOrWhiteSpace(textBoxObsMan.Text)))
                            textBoxObsMan.Enabled = true;
                        buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = radioButtonUnidad.Visible = true;
                    }
                }
                buttonGuardar.Visible = label24.Visible = false;
                dr.Close();
                v.c.dbcon.Close();
                banderaeditar = true;
                Cancelar(true);
            }
            else
                MessageBox.Show("No puede editar un reporte si no se ha guardado por lo menos una vez", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void valdeval()
        {
            if (string.IsNullOrWhiteSpace(mecanicoanterior))
                mecanicoanterior = ".";
            if (string.IsNullOrWhiteSpace(mecanicoapoanterior))
                mecanicoapoanterior = "..";
            if (string.IsNullOrWhiteSpace(supervisoanterior))
                supervisoanterior = "...";
            if (validacionfinalconteocolumnas)
            {
                fincolumn = dgvPMantenimiento.Rows.Count;
                validacionfinalconteocolumnas = false;
            }
            if (comboBoxFalloGral.Text.Equals("-- GRUPO --"))
            {
                idvalidacionfgeneral = 0;
                validacionfgeneral = "";
            }
            else
            {
                idvalidacionfgeneral = Convert.ToInt32(comboBoxFalloGral.SelectedValue);
                validacionfgeneral = comboBoxFalloGral.Text;
            }
            if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
            {
                idvalidacionreqrefacc = 0;
                validacionreqrefacc = "";
            }
            else
            {
                idvalidacionreqrefacc = Convert.ToInt32(comboBoxReqRefacc.SelectedValue);
                validacionreqrefacc = comboBoxReqRefacc.Text;
            }
            if (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --"))
            {
                idvalidacionexisrefacc = 0;
                validacionexisrefacc = "";
            }
            else
            {
                idvalidacionexisrefacc = Convert.ToInt32(comboBoxExisRefacc.SelectedValue);
                validacionexisrefacc = comboBoxExisRefacc.Text;
            }
            if (comboBoxEstatusMant.Text.Equals("-- ESTATUS --"))
                validacionestatusmant = "";
            else
                validacionestatusmant = comboBoxEstatusMant.Text;
        }

        public void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor, Form f)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);
                g.Clear(f.BackColor);
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
        bool exportando = false;
        public void botonactualizar()
        {
            inicolumn = 0;
            fincolumn = 0;
            registroconteofilaspedref = false;
            metodocargaref();
            metodoCarga();
            conteo();
            if (label35.Text.Equals("EXPORTANDO"))
                exportando = true;
            else
                buttonExcel.Visible = label35.Visible = false;
            dgvMantenimiento.Refresh();
            limpiarcamposbus();
            idgeneral = "";
            Unidad(); Mecanico(); DescripFallo();
        }

        public void textBox_TextChanged(object sender, EventArgs e)
        {
            validacioneditar();
        }

        private void comboBoxFalloGral_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((comboBoxFalloGral.SelectedIndex > 0) && (labelNomMecanico.Text != ".") && (comboBoxEstatusMant.Enabled == true) && (banderaeditar == false))
                comboBoxReqRefacc.Enabled = true;
            else
                comboBoxReqRefacc.Enabled = false;
            validacioneditar();
        }

        public void combos_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= -1)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        DataTable f = (DataTable)cbx.DataSource;
                        if (e.Index == -1)
                            e.Graphics.DrawString("", cbx.Font, brush, e.Bounds, sf);
                        else
                            e.Graphics.DrawString(f.Rows[e.Index].ItemArray[1].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        DataTable f = (DataTable)cbx.DataSource;
                        if (e.Index == -1)
                            e.Graphics.DrawString("", cbx.Font, brush, e.Bounds, sf);
                        else
                            e.Graphics.DrawString(f.Rows[e.Index].ItemArray[1].ToString(), cbx.Font, brush, e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                }
            }
        }

        public void validarstatusfallagral(string cdf)
        {
            if (string.IsNullOrWhiteSpace(cdf))
            {
                MySqlCommand cmd00 = new MySqlCommand("SELECT UPPER(t1.nombreFalloGral) AS nombreFalloGral, t1.idFalloGral FROM cfallosgrales AS t1 INNER JOIN reportemantenimiento AS t2 ON t1.idFalloGral = t2.FalloGralfkFallosGenerales WHERE t1.nombreFalloGral = '" + dgvMantenimiento.CurrentRow.Cells[15].Value.ToString() + "' AND t1.status = '0' AND t1.empresa='" + empresa + "'", v.c.dbconection());
                MySqlDataReader dr00 = cmd00.ExecuteReader();
                if (dr00.Read())
                {
                    MySqlCommand cmd01 = new MySqlCommand("SELECT UPPER(t1.nombreFalloGral) AS nombreFalloGral, t1.idFalloGral FROM cfallosgrales AS t1 INNER JOIN reportemantenimiento AS t2 ON t1.idFalloGral = t2.FalloGralfkFallosGenerales WHERE t1.nombreFalloGral = '" + dgvMantenimiento.CurrentRow.Cells[15].Value.ToString() + "' AND t1.status = '1' AND t1.empresa='" + empresa + "'", v.c.dbconection());
                    MySqlDataAdapter da01 = new MySqlDataAdapter(cmd01);
                    DataTable dt = new DataTable();
                    da01.Fill(dt);
                    DataRow row = dt.NewRow();
                    DataRow row2 = dt.NewRow();
                    row2["idFalloGral"] = 0;
                    row2["nombreFalloGral"] = "-- FALLA GENERAL --";
                    dt.Rows.InsertAt(row2, 0);
                    row["idFalloGral"] = dr00["idFalloGral"];
                    row["nombreFalloGral"] = dr00["nombreFalloGral"];
                    dt.Rows.InsertAt(row, 1);
                    comboBoxFalloGral.ValueMember = "idFalloGral";
                    comboBoxFalloGral.DisplayMember = "nombreFalloGral";
                    comboBoxFalloGral.DataSource = dt;
                    comboBoxFalloGral.Text = dr00["nombreFalloGral"].ToString();
                    comboBoxFalloGral.Text = dgvMantenimiento.CurrentRow.Cells[15].Value.ToString();
                    v.c.dbcon.Close();
                }
                dr00.Close();
                v.c.dbcon.Close();
                valorfallogeneral = 1;
            }
        }

        public void exportacionpdf1()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Reporte de Mantenimiento', '" + idreportesupervision + "', 'Exportación de reporte de la unidad en archivo pdf', '" + idUsuario +
               "', NOW(), 'Exportación a PDF de reporte en Mantenimiento', '" + empresa + "', '" + area + "')", v.c.dbconection());
            cmd.ExecuteNonQuery();
            v.c.dbcon.Close();
        }

        public void exportacionpdf2()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Reporte de Mantenimiento', '" + idreportesupervision + "', 'Exportación de reporte de fallo en archivo pdf', '" + idUsuario + "', NOW(), 'Exportación a PDF de reporte en Mantenimiento', '" + empresa + "', '" + area + "')", v.c.dbconection());
            cmd.ExecuteNonQuery();
            v.c.dbcon.Close();
        }

        public void valida_refacciones()
        {
            int contador = 0;
            faltante = 0; totalfaltante = 0; validacionexistenciarefacciones = 0; cantidadtotalrefacciones = 0; totalexistenciarefaccinoes = 0;
            DataTable dt = (DataTable)v.getData("select Cantidad, coalesce(CantidadEntregada,0) as 'Cantidad Entregada' from pedidosrefaccion as t1 inner join reportesupervicion as t3 on t3.idReporteSupervicion=t1.FolioPedfkSupervicion where t3.folio='" + labelFolio.Text + "'");
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(EstatusRefaccion) AS estatus FROM pedidosrefaccion WHERE ((FolioPedfkSupervicion = RIGHT('" + idreportesupervision + "', 5)) AND ((EstatusRefaccion LIKE 'EXISTENCIA%') OR (EstatusRefaccion LIKE 'SIN EXISTENCIA%') OR (EstatusRefaccion LIKE 'INCOMPLETO%')))", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                totalexistenciarefaccinoes = Convert.ToInt32(dr.GetString("estatus"));
            }
            totalrefacciones = dt.Rows.Count;
            if (totalrefacciones > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    item_temporal = 0;
                    foreach (var item in row.ItemArray)
                    {
                        if ((contador % 2 != 0 && contador > 0) && (Convert.ToInt32(item) != item_temporal))
                        {
                            faltante = Convert.ToInt32(item) - item_temporal;
                            totalfaltante = totalfaltante + faltante;
                        }
                        item_temporal = Convert.ToInt32(item);
                        contador++;
                    }
                    validacionexistenciarefacciones = validacionexistenciarefacciones + 1;
                }
            }

        }
        public void validarrefacciones()
        {
            int crefacc;
            cantidadtotalrefacciones = conteorefacciones = totalfaltante = faltante = validacionexistenciarefacciones = 0;
            string refacc;
            MySqlCommand cmd0 = new MySqlCommand("SELECT coalesce(MAX(NumRefacc), 0) AS NumRefacc, coalesce((NumRefacc), 0) AS NumRefacc1 FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
            MySqlDataReader dr = cmd0.ExecuteReader();
            if (dr.Read())
            {
                totalrefacciones = Convert.ToInt32(dr.GetString("NumRefacc"));
                conteorefacciones = Convert.ToInt32(dr.GetString("NumRefacc1"));
            }
            else
                totalrefacciones = 0;
            dr.Close();
            v.c.dbcon.Close();
            for (crefacc = 1; crefacc <= totalrefacciones; crefacc++)
            {
                refacc = "";
                MySqlCommand cmd1 = new MySqlCommand("SELECT coalesce((EstatusRefaccion), '') AS EstatusRefaccion, coalesce((Cantidad - CantidadEntregada), 0) AS Faltante FROM pedidosrefaccion WHERE NumRefacc = '" + conteorefacciones + "' AND FolioPedfkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    refacc = Convert.ToString(dr1.GetString("EstatusRefaccion"));
                    faltante = Convert.ToInt32(dr1.GetString("Faltante"));
                }
                else
                    refacc = "";
                dr1.Close();
                v.c.dbcon.Close();
                if ((refacc.Equals("EXISTENCIA")) || (refacc.Equals("SIN EXISTENCIA")) || (refacc.Equals("INCOMPLETO")))
                {
                    if (refacc.Equals("EXISTENCIA"))
                        validacionexistenciarefacciones = validacionexistenciarefacciones + 1;
                    totalfaltante = totalfaltante + faltante;
                    cantidadtotalrefacciones = cantidadtotalrefacciones + 1;
                }
                else
                    cantidadtotalrefacciones = cantidadtotalrefacciones + 0;
                conteorefacciones = conteorefacciones + 1;
            }
        }

        public void privilegios()
        {
            string sql = "SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar) as privilegios FROM privilegios where usuariofkcpersonal = '" + idUsuario + "' and namform = 'Mantenimiento'";
            string[] privilegios = getaData(sql).ToString().Split(';');
            pinsertar = getBoolFromInt(Convert.ToInt32(privilegios[0]));
            pconsultar = getBoolFromInt(Convert.ToInt32(privilegios[1]));
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[2]));
            pdesactivar = getBoolFromInt(Convert.ToInt32(privilegios[3]));
        }

        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }

        public object getaData(string sql)
        {
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            var res = cm.ExecuteScalar();
            v.c.dbconection();
            return res;
        }

        public void AutoCompletado(TextBox cajaTexto) //Metodo De AutoCompletado
        {
            AutoCompleteStringCollection nColl = new AutoCompleteStringCollection();
            MySqlCommand cmd = new MySqlCommand("SELECT Folio FROM reportesupervicion", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows == true)
                while (dr.Read())
                    nColl.Add(dr["Folio"].ToString());
            dr.Close();
            v.c.dbcon.Close();
            textBoxFolioB.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxFolioB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxFolioB.AutoCompleteCustomSource = nColl;
        }

        public void validar() //Metodo para validar el estatus
        {
            if (estatusmantGV.Equals("-- ESTATUS --"))
                comboBoxEstatusMant.SelectedIndex = 1;
            else if (estatusmantGV == "EN PROCESO")
                comboBoxEstatusMant.SelectedIndex = 1;
            else if (estatusmantGV == "REPROGRAMADA")
                comboBoxEstatusMant.SelectedIndex = 2; //Cambio a 3
            else if (estatusmantGV == "")
                comboBoxEstatusMant.SelectedIndex = 1;
            if (comboBoxReqRefacc.SelectedIndex == 1)
                buttonAgregar.Visible = label39.Visible = true;
            else
                buttonAgregar.Visible = label39.Visible = false;
            labelHoraTerminoM.Text = textBoxTerminoMan.Text = "";
        }

        public void validar2()
        {
            if (conteorefaccionesverificadas == 2)
            {
                if (((comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (!textBoxFolioFactura.Enabled) || comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))
                {
                    if (comboBoxReqRefacc.SelectedIndex != 2)
                        buttonAgregar.Visible = label39.Visible = true;
                    timer2.Start();
                    existencias = true;
                    conteorefaccionesverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 2;
                }
                else if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                {
                    buttonAgregar.Visible = label39.Visible = false;
                    timer2.Start();
                    existencias = true;
                    conteorefaccionesverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 2;
                }
                else if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) && (!textBoxFolioFactura.Enabled) && (validacionconteo == 1))
                {
                    buttonAgregar.Visible = label39.Visible = true;
                    timer2.Start();
                    existencias = false;
                    conteorefaccionesverificadas = validacionconteo = 0;
                }
                else if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) && (validacionconteo == 4))
                {
                    buttonAgregar.Visible = label39.Visible = true;
                    timer2.Start();
                    existencias = false;
                    conteorefaccionesverificadas = validacionconteo = 0;
                }
                else if ((comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) && (!textBoxFolioFactura.Enabled) && ((validacionconteo == 1) || (validacionconteo == 7)))
                {
                    buttonAgregar.Visible = label39.Visible = existencias = true;
                    timer2.Start();
                    conteorefaccionesverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 2;
                }
            }
            else if (conteorefaccionesverificadas == 1)
            {
                if ((comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) && (textBoxFolioFactura.Enabled == false) && (validacionconteo == 6))
                {
                    buttonAgregar.Visible = label39.Visible = true;
                    if (textBoxFolioFactura.Text.Equals(""))
                        textBoxFolioFactura.Enabled = true;
                    else
                        textBoxFolioFactura.Enabled = false;
                    timer2.Start();
                    existencias = false;
                    conteorefaccionesverificadas = validacionconteo = 0;
                }
                else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && ((textBoxFolioFactura.Enabled == false) || (textBoxFolioFactura.Enabled == true)) && (validacionconteo == 6))
                {
                    buttonAgregar.Visible = label39.Visible = true;
                    if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                        textBoxFolioFactura.Enabled = true;
                    else if (!string.IsNullOrWhiteSpace(textBoxFolioFactura.Text) && !textBoxFolioFactura.Enabled)
                        textBoxFolioFactura.Enabled = false;
                    timer2.Start();
                    existencias = true;
                    conteorefaccionesverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 1;
                }
                else if (comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN"))
                {
                    buttonAgregar.Visible = label39.Visible = false;
                    timer2.Start();
                    existencias = true;
                    conteorefaccionesverificadas = 0;
                    comboBoxExisRefacc.SelectedIndex = 1;
                }
            }
        }

        public void limpiarcampos() //Metodo Para Limpiar Los Campos
        {
            comboBoxFalloGral.SelectedIndex = comboBoxExisRefacc.SelectedIndex = comboBoxReqRefacc.SelectedIndex = comboBoxEstatusMant.SelectedIndex = 0;
            textBoxMecanico.Text = textBoxMecanicoApo.Text = textBoxTrabajoRealizado.Text = textBoxFolioFactura.Text = textBoxSuperviso.Text = textBoxObsMan.Text = textBoxEsperaMan.Text = textBoxTerminoMan.Text = labelFolio.Text = labelUnidad.Text = labelFechaReporte.Text = labelKm.Text = labelSupervisor.Text = labelcodfallo.Text = labelHoraReporte.Text = textBoxDescrpFallo.Text = textBoxObsSup.Text = labelHoraInicioM.Text = labelHoraTerminoM.Text = "";
            labelNomMecanico.Text = ".";
            labelNomMecanicoApo.Text = "..";
            labelNomSuperviso.Text = "...";
            mensaje = false;
        }

        public void limpiarcamposbus() //Metodo Para Limpiar Los Campos
        {
            textBoxFolioB.Text = "";
            comboBoxUnidadB.SelectedIndex = comboBoxMecanicoB.SelectedIndex = comboBoxEstatusMB.SelectedIndex = comboBoxDescpFalloB.SelectedIndex = comboBoxMesB.SelectedIndex = 0;
            dateTimePickerIni.Value = dateTimePickerFin.Value = DateTime.Now;
        }

        public void limpiarrefacc() //Metodo Para Limpiar Los Campos
        {
            comboBoxFamilia.SelectedIndex = 0;
            comboBoxFRefaccion.DataSource = null;
            textBoxCantidad.Text = textBoxUM.Text = "";
        }

        public void limpiarstring()
        {
            idfgeneralanterior = cantidadrefacciones = valorfallogeneral = idreportesupervision = 0;
            fgeneralanterior = mecanicoanterior = mecanicoapoanterior = supervisoanterior = exisrefaccionanterior = reqrefanterior = trabrealizadoanterior = folfacturanterior = estatusmantanterior = observacionesmantanterior = "";
        }

        public void notcalcul() //Hace La Validacion Si La Unidad Esta Liberada
        {
            String temp = "";
            MySqlCommand cmd = new MySqlCommand("SELECT EsperaTiempoM FROM reportemantenimiento WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                temp = Convert.ToString(dr.GetString("EsperaTiempoM"));
            if (comboBoxEstatusMant.Text.Equals("LIBERADA") || (temp != ""))
                timer1.Stop();
            else
                esperaman();
            dr.Close();
            v.c.dbcon.Close();
        }

        public void metodobtnfinalizarsref()
        {
            FormContraFinal FCF = new FormContraFinal(empresa, area, this,v);
            var res = FCF.ShowDialog();
            if (res == DialogResult.OK)
            {
                labelidFinal.Text = FCF.id;
                if (string.IsNullOrWhiteSpace(labelidFinal.Text))
                {
                    labelHoraTerminoM.Text = textBoxTerminoMan.Text = "";
                    validar();
                    buttonFinalizar.Visible = label37.Visible = false;
                    buttonGuardar.Visible = label24.Visible = true;
                }
                else if (labelidFinal.Text != "")
                {
                    MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET PersonaFinal = '" + labelidFinal.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                    cmd.ExecuteNonQuery();
                    v.c.dbcon.Close();
                    metodoActualizar();
                    metodoCarga();
                    conteo();
                    MessageBox.Show("El Reporte Se Ha Finalizado Exitosamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarcampos();
                    buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
                    ocultarexcel();
                    if (pinsertar && pconsultar && peditar)
                        buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = true;
                    else
                        buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = false;
                    buttonAgregar.Visible = label39.Visible = false;
                }
                timer2.Start();
                groupBoxBusqueda.Enabled = true;
            }
        }

        public bool metodotxtchref() // checar
        {
            if (dgvPMantenimiento.Rows.Count == 0)
                return true;
            else
                return false;
        }

        public void metodobtnfinalizarcref()
        {
            FormContraFinal FCF = new FormContraFinal(empresa, area, this,v);
            FCF.Owner = this;
            FCF.ShowDialog();
            labelidFinal.Text = FCF.id;
            if ((string.IsNullOrWhiteSpace(labelidFinal.Text)))
            {
                labelHoraTerminoM.Text = textBoxTerminoMan.Text = "";
                validar();
                buttonFinalizar.Visible = label37.Visible = false;
                buttonGuardar.Visible = label24.Visible = true;
                resultadopregunta = 0;
            }
            else if ((labelidFinal.Text != ""))
            {
                resultadopregunta = 1;
                MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET PersonaFinal = '" + labelidFinal.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                cmd.ExecuteNonQuery();
                v.c.dbcon.Close();
                metodoActualizar();
                metodoCarga();
                conteo();
                MessageBox.Show("El Reporte Se Ha Finalizado Correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cancelar(false);
                limpiarcampos();
                limpiarstring();
                buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
                ocultarexcel();
                if (pinsertar && pconsultar && peditar)
                    buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = true;
                else
                    buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = false;
                buttonAgregar.Visible = label39.Visible = false;
            }
            timer2.Start();
        }

        public void metodobtnguardar()
        {
            if ((labelNomSuperviso.Text == "...") && (comboBoxEstatusMant.Text.Equals("LIBERADA")))
            {
                MessageBox.Show("No se realizó ningún cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                actualizarcbx();
                limpiarcampos();
                limpiarstring();
                limpiarcampos();
                limpiarcamposbus();
                buttonGuardar.Visible = label24.Visible = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = false;
                ocultarexcel();
                metodoCarga();
                comboBoxEstatusMant.SelectedIndex = 0;
            }
            else
            {
                if (!(estatusmantGV == comboBoxEstatusMant.Text))
                    notcalcul();
                if (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --"))
                    existenciaif = "";
                else
                    existenciaif = comboBoxExisRefacc.Text;
                if ((dgvPMantenimiento.Rows.Count - inicolumn) != 0)
                {
                    MySqlCommand actualizarestatus = new MySqlCommand("UPDATE pedidosrefaccion SET estatus = 1 WHERE FolioPedfkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                    actualizarestatus.ExecuteNonQuery();
                    v.c.dbcon.Close();
                }
                MySqlCommand cmd0 = new MySqlCommand("SELECT t2.FoliofkSupervicion FROM reportesupervicion AS t1 INNER JOIN reportemantenimiento AS t2 ON t1.idReporteSupervicion = t2.FoliofkSupervicion WHERE t2.FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                MySqlDataReader dr0 = cmd0.ExecuteReader();
                if (dr0.Read())
                {
                    metodoActualizar();
                    MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if ((labelNomMecanicoApo.Text.Equals("..")) && (labelNomSuperviso.Text.Equals("...")))
                    {
                        MySqlCommand cmd1 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, StatusRefacciones, ExistenciaRefaccAlm,empresa) VALUES ('" + idreportesupervision + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + comboBoxReqRefacc.Text + "', '" + existenciaif + "','" + empresa + "')", v.c.dbconection());
                        cmd1.ExecuteNonQuery();
                        MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        v.c.dbcon.Close();
                    }
                    else
                    {
                        if (labelNomMecanicoApo.Text.Equals("..") && (labelNomSuperviso.Text != "..."))
                        {
                            MySqlCommand cmd2 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, SupervisofkPersonal, StatusRefacciones, ExistenciaRefaccAlm) VALUES ('" + idreportesupervision + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + labelidSuperviso.Text + "','" + comboBoxReqRefacc.Text + "', '" + existenciaif + "')", v.c.dbconection());
                            cmd2.ExecuteNonQuery();
                            registroconteofilaspedref = true;
                            MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            v.c.dbcon.Close();
                        }
                        else
                        {
                            if (labelNomSuperviso.Text.Equals("...") && (labelNomMecanicoApo.Text != ".."))
                            {
                                labelNomSuperviso.Text = "";
                                MySqlCommand cmd2 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, MecanicoApoyofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, StatusRefacciones, ExistenciaRefaccAlm) VALUES ('" + idreportesupervision + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', '" + labelidMecanicoApo.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + comboBoxReqRefacc.Text + "', '" + existenciaif + "')", v.c.dbconection());
                                cmd2.ExecuteNonQuery();
                                registroconteofilaspedref = true;
                                MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                v.c.dbcon.Close();
                            }
                            else
                            {
                                MySqlCommand cmd2 = new MySqlCommand("SET lc_time_names = 'es_ES'; INSERT INTO reportemantenimiento(FoliofkSupervicion, FalloGralfkFallosGenerales, TrabajoRealizado, MecanicofkPersonal, MecanicoApoyofkPersonal, FechaReporteM, HoraInicioM, EsperaTiempoM, FolioFactura, Estatus, SupervisofkPersonal, StatusRefacciones, ExistenciaRefaccAlm) VALUES ('" + idreportesupervision + "', '" + comboBoxFalloGral.SelectedValue + "', '" + textBoxTrabajoRealizado.Text + "', '" + labelidMecanico.Text + "', '" + labelidMecanicoApo.Text + "', curdate(), '" + labelHoraInicioM.Text + "', '" + textBoxEsperaMan.Text + "', '" + textBoxFolioFactura.Text + "', '" + comboBoxEstatusMant.Text + "', '" + labelidSuperviso.Text + "', '" + comboBoxReqRefacc.Text + "', '" + existenciaif + "')", v.c.dbconection());
                                cmd2.ExecuteNonQuery();
                                registroconteofilaspedref = true;
                                MessageBox.Show("Se ha guardado el registro con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                v.c.dbcon.Close();
                            }
                        }
                    }
                    AutoCompletado(textBoxFolioB);
                }
                actualizarcbx();
                metodoCarga();
                limpiarstring();
                conteo();
                limpiarcampos();
                ncontreffin();
                cantidadrefacciones = 0;
                inicolumn = 0;
                fincolumn = 0;
                buttonGuardar.Visible = label24.Visible = comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonAgregar.Visible = label39.Visible = buttonFinalizar.Visible = label37.Visible = buttonAgregar.Visible = label39.Visible = label1.Visible = false;
                dgvMantenimiento.Refresh();
                ocultarexcel();
                timer1.Start();
                Cancelar(false);
                comboBoxEstatusMant.SelectedIndex = 0;
            }
        }

        public void llamadadatos()
        {
            label1.Visible = groupBoxRefacciones.Visible = buttonAgregar.Visible = label39.Visible = false;
            gbxMantenimiento.Visible = buttonActualizar.Visible = label26.Visible = buttonGuardar.Visible = label24.Visible = true;
            timer2.Start();
            limpiarcampos();
            limpiarcamposbus();
            enabledfalse();
            labelFolio.Text = dgvMantenimiento.CurrentRow.Cells["FOLIO"].Value.ToString();
            estatusmantGV = dgvMantenimiento.CurrentRow.Cells["ESTATUS DEL MANTENIMIENTO"].Value.ToString();
            idgeneral = dgvMantenimiento.CurrentRow.Cells["ID"].Value.ToString();
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT coalesce((SELECT r25.FoliofkSupervicion FROM reportemantenimiento AS r25 WHERE r25.FoliofkSupervicion = t1.idReporteSupervicion), 0) AS Folio, coalesce((SELECT r25.IdReporte FROM reportemantenimiento AS r25 WHERE t1.idReporteSupervicion = r25.FoliofkSupervicion), 0) AS IdMantenimiento, coalesce((SELECT r22.MecanicofkPersonal FROM reportemantenimiento AS r22 WHERE t1.idReporteSupervicion = r22.FoliofkSupervicion), 0) AS IdMecanico, coalesce((SELECT r23.MecanicoApoyofkPersonal FROM reportemantenimiento AS r23 WHERE t1.idReporteSupervicion = r23.FoliofkSupervicion), 0) AS IdMecanicoApoyo, coalesce((SELECT r24.SupervisofkPersonal FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), 0) AS IdSuperviso, t1.idReporteSupervicion AS ID, t1.Folio, CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'Fecha Del Reporte', coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, t1.HoraEntrada AS 'Hora De Entrada', t1.KmEntrada AS 'Kilometraje', UPPER(t1.TipoFallo) AS 'Tipo De Fallo', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'Descripcion De Fallo', coalesce((SELECT UPPER(r22.codfallo) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'Codigo De Fallo', coalesce((UPPER(t1.DescFalloNoCod)), '') AS 'Descripcion De Fallo No Codificado', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'Observaciones De Supervision', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'Fallo General', coalesce((SELECT UPPER(r4.idfallogral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), 0) AS 'IdFG', coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'Trabajo Realizado', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'Mecanico', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'Mecanico De Apoyo', coalesce((SELECT UPPER(DATE_FORMAT(r10.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r10 WHERE t1.idReporteSupervicion = r10.FoliofkSupervicion), '') AS 'Fecha Del Reporte De Mantenimiento', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'Hora De Inicio De Mantenimiento', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'Hora De Termino De Mantenimiento', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'Espera De Tiempo Para Mantenimiento', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'Diferencia De Tiempo En Mantenimiento', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'Folio De Factura', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'Estatus Del Mantenimiento', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'Superviso', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'Existencia De Refacciones En Almacen', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'Estatus De Refacciones', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'Observaciones Del Mantenimiento' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad  INNER JOIN careas AS t4 ON t2.areafkcareas= t4.idarea WHERE t1.Folio = '" + labelFolio.Text + "'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                idreportemantenimiento = Convert.ToInt32(dr.GetString("IdMantenimiento"));
                idreportesupervision = Convert.ToInt32(dr.GetString("ID"));
                labelUnidad.Text = dr.GetString("ECO");
                labelFechaReporte.Text = dr.GetString("Fecha Del Reporte");
                labelHoraReporte.Text = dr.GetString("Hora De Entrada");
                labelKm.Text = dr.GetString("Kilometraje");
                labelSupervisor.Text = dr.GetString("Supervisor");
                labelcodfallo.Text = dr.GetString("Codigo De Fallo");
                string fallo = dr.GetString("Descripcion De Fallo No Codificado");
                if (!string.IsNullOrWhiteSpace(fallo))
                    textBoxDescrpFallo.Text = fallo;
                else
                    textBoxDescrpFallo.Text = dr.GetString("Descripcion De Fallo") + " - " + dr.GetString("Codigo De Fallo");
                textBoxObsSup.Text = dr.GetString("Observaciones De Supervision");
                folio = dr.GetString("Folio");
                labelFecha.Text = dr.GetString("Fecha Del Reporte De Mantenimiento");
                comboBoxReqRefacc.Text = dr.GetString("Estatus de Refacciones");
                reqrefanterior = dr.GetString("Estatus de Refacciones");
                if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                    reqrefanterior = "";
                labelNomMecanico.Text = dr.GetString("mecanico");
                mecanicoanterior = dr.GetString("mecanico");
                idmecanicoanterior = Convert.ToInt32(dr.GetString("IdMecanico"));
                if (string.IsNullOrWhiteSpace(mecanicoanterior))
                    labelNomMecanico.Text = mecanicoanterior = ".";
                labelHoraInicioM.Text = dr.GetString("Hora De Inicio de Mantenimiento");
                if ((string.IsNullOrWhiteSpace(labelHoraInicioM.Text)) || (labelHoraInicioM.Text == "00:00:00"))
                    timer1.Start();
                else
                {
                    timer1.Stop();
                    finiciomantenimiento = Convert.ToDateTime(dr.GetString("Hora De Inicio De Mantenimiento"));
                }
                comboBoxExisRefacc.Text = dr.GetString("Existencia De Refacciones En Almacen");
                exisrefaccionanterior = dr.GetString("Existencia De Refacciones En Almacen");
                if (comboBoxExisRefacc.SelectedIndex == 0)
                    exisrefaccionanterior = "";
                comboBoxFalloGral.Text = dr.GetString("Fallo General");
                fgeneralanterior = dr.GetString("Fallo General");
                idfgeneralanterior = Convert.ToInt32(dr.GetString("IdFG"));
                if (comboBoxFalloGral.SelectedIndex == 0)
                    fgeneralanterior = "";
                validarstatusfallagral(fgeneralanterior);
                labelNomMecanicoApo.Text = dr.GetString("Mecanico De Apoyo");
                mecanicoapoanterior = dr.GetString("Mecanico De Apoyo");
                idmecanicoapoanterior = Convert.ToInt32(dr.GetString("IdMecanicoApoyo"));
                if (string.IsNullOrWhiteSpace(mecanicoapoanterior))
                    labelNomMecanicoApo.Text = mecanicoapoanterior = "..";
                textBoxEsperaMan.Text = dr.GetString("Espera De Tiempo Para Mantenimiento");
                textBoxFolioFactura.Text = dr.GetString("Folio De Factura");
                folfacturanterior = dr.GetString("Folio De Factura");
                textBoxTrabajoRealizado.Text = dr.GetString("Trabajo Realizado");
                trabrealizadoanterior = dr.GetString("Trabajo Realizado");
                labelNomSuperviso.Text = dr.GetString("Superviso");
                supervisoanterior = dr.GetString("Superviso");
                idsupervisoanterior = Convert.ToInt32(dr.GetString("IdSuperviso"));
                if (string.IsNullOrWhiteSpace(supervisoanterior))
                    labelNomSuperviso.Text = supervisoanterior = "...";
                textBoxObsMan.Text = dr.GetString("Observaciones Del Mantenimiento");
                observacionesmantanterior = dr.GetString("Observaciones Del Mantenimiento");
                comboBoxEstatusMant.Text = dr.GetString("Estatus Del Mantenimiento");
                estatusmantanterior = dr.GetString("Estatus Del Mantenimiento");
                if (comboBoxEstatusMant.SelectedIndex == 0)
                {
                    comboBoxEstatusMant.SelectedIndex = 1;
                    estatusmantanterior = comboBoxEstatusMant.Text;
                    Cancelar(false);
                }
                else if (estatusmantanterior.Equals("LIBERADA") && supervisoanterior == "...")
                    Cancelar(true);
                else if (estatusmantanterior.Equals("LIBERADA"))
                    Cancelar(false);
                else
                    Cancelar(true);
                if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (labelNomSuperviso.Text == "..."))
                {
                    timer2.Stop();
                    fterminomantenimiento = Convert.ToDateTime(dr.GetString("Hora De Termino De Mantenimiento"));
                    labelHoraTerminoM.Text = fterminomantenimiento.ToString("T", CultureInfo.CreateSpecificCulture("es-ES"));
                    textBoxTerminoMan.Text = dr.GetString("Diferencia De Tiempo En Mantenimiento");
                    comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = comboBoxReqRefacc.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = textBoxFolioFactura.Enabled = textBoxObsMan.Enabled = buttonFinalizar.Visible = label37.Visible = false;
                    textBoxSuperviso.Enabled = buttonGuardar.Visible = label24.Visible = true;
                    if (labelNomMecanicoApo.Text.Equals(""))
                        labelNomMecanicoApo.Text = "..";
                    labelNomSuperviso.Text = "...";
                }
                else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && ((labelNomSuperviso.Text != "...") || (labelNomSuperviso.Text != "...")))
                {
                    timer2.Stop();
                    fterminomantenimiento = Convert.ToDateTime(dr.GetString("Hora De Termino De Mantenimiento"));
                    labelHoraTerminoM.Text = fterminomantenimiento.ToString("%H:%m:%s");
                    textBoxTerminoMan.Text = dr.GetString("Diferencia De Tiempo En Mantenimiento");
                    comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = comboBoxReqRefacc.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = textBoxFolioFactura.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonEditar.Visible = label58.Visible = false;
                    if (labelNomMecanicoApo.Text.Equals(""))
                        labelNomMecanicoApo.Text = "..";
                }
                dr.Close();
                v.c.dbcon.Close();
                if ((comboBoxFalloGral.Text.Equals("-- GRUPO --")) && (labelNomMecanico.Text.Equals(".")))
                    comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = comboBoxEstatusMant.Enabled = true;
                else if ((comboBoxEstatusMant.Text.Equals("-- ESTATUS --")) || (comboBoxEstatusMant.Text.Equals("EN PROCESO")) || (comboBoxEstatusMant.Text.Equals("REPROGRAMADA")))
                {
                    comboBoxEstatusMant.Enabled = true;
                    if (string.IsNullOrWhiteSpace(labelNomMecanico.Text) || labelNomMecanico.Text.Equals("."))
                    {
                        textBoxMecanico.Enabled = true;
                        labelNomMecanico.Text = ".";
                        mecanicoanterior = labelNomMecanico.Text;
                    }
                    if (string.IsNullOrWhiteSpace(labelNomMecanicoApo.Text) || labelNomMecanicoApo.Text.Equals(".."))
                    {
                        textBoxMecanicoApo.Enabled = true;
                        labelNomMecanicoApo.Text = "..";
                        mecanicoapoanterior = labelNomMecanicoApo.Text;
                    }
                    if (string.IsNullOrWhiteSpace(labelNomSuperviso.Text) || labelNomSuperviso.Text.Equals("..."))
                    {
                        textBoxSuperviso.Enabled = true;
                        labelNomSuperviso.Text = "...";
                        supervisoanterior = labelNomSuperviso.Text;
                    }
                    if (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))
                        textBoxTrabajoRealizado.Enabled = true;
                    if (string.IsNullOrWhiteSpace(textBoxObsMan.Text))
                        textBoxObsMan.Enabled = true;
                    if ((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES")) && (comboBoxReqRefacc.Enabled == false) && (comboBoxEstatusMant.Text != "LIBERADA"))
                        buttonAgregar.Visible = label39.Visible = comboBoxExisRefacc.Enabled = true;
                    if (!string.IsNullOrWhiteSpace(labelNomMecanico.Text))
                        comboBoxReqRefacc.Enabled = true;
                    else if ((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES")) && (comboBoxReqRefacc.Enabled == false) && (estatusmantGV.Equals("LIBERADA")))
                        buttonAgregar.Visible = label39.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonEditar.Visible = label58.Visible = false;
                    else if ((comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES")) && (comboBoxEstatusMant.Text != "LIBERADA"))
                        comboBoxExisRefacc.Enabled = textBoxFolioFactura.Enabled = false;
                    if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                        if ((comboBoxEstatusMant.Text != "LIBERADA") && (textBoxFolioFactura.Text != ""))
                            textBoxFolioFactura.Enabled = false;
                        else
                            textBoxFolioFactura.Enabled = true;
                }
                metodocargaref();
                conteoiniref();
                conteofinref();
                if (((labelrefini.Text != "0") && (labelNomMecanico.Text != ".")) && (pconsultar))
                    buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = true;
                else if ((labelNomMecanico.Text != ".") && (pconsultar))
                    buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = true;
                else
                    buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = false;
            }
        }

        public void esperaman() //Saca El Tiempo Total De En Espera
        {
            DateTime dt = finiciomantenimiento = DateTime.Now;
            DateTime dt2 = Convert.ToDateTime(labelFechaReporte.Text + " " + labelHoraReporte.Text);
            TimeSpan s2 = dt.Subtract(dt2);
            string horaminu = Convert.ToString(s2.Hours.ToString() + ":" + s2.Minutes.ToString() + ":" + s2.Seconds.ToString());
            difhorasinicio = Convert.ToDateTime(horaminu);
            if (s2.Hours.Equals(0))
                textBoxEsperaMan.Text = s2.Days.ToString() + " Dias con " + difhorasinicio.ToString("T", CultureInfo.CreateSpecificCulture("es-ES")) + " Minutos";
            else
                textBoxEsperaMan.Text = s2.Days.ToString() + " Dias con " + difhorasinicio.ToString("T", CultureInfo.CreateSpecificCulture("es-ES")) + " Horas";
            labelHoraInicioM.Text = finiciomantenimiento.ToString("T", CultureInfo.CreateSpecificCulture("es-ES"));
        }

        public void sumafech() //Suma Las Fechas Para Obtener El Tiempo Final
        {
            finiciomantenimiento = Convert.ToDateTime(labelFechaReporte.Text + " " + labelHoraReporte.Text);
            DateTime tdt1 = finiciomantenimiento;
            DateTime tdt2 = fterminomantenimiento = DateTime.Now;
            TimeSpan ts = tdt2.Subtract(tdt1);
            string finhoraminu = Convert.ToString(ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString());
            difhorasfinal = Convert.ToDateTime(finhoraminu);
            if (ts.Hours.Equals(0))
                textBoxTerminoMan.Text = ts.Days.ToString() + " Dias con " + difhorasfinal.ToString("T", CultureInfo.CreateSpecificCulture("es-ES")) + " Minutos";
            else
                textBoxTerminoMan.Text = ts.Days.ToString() + " Dias con " + difhorasfinal.ToString("T", CultureInfo.CreateSpecificCulture("es-ES")) + " Horas";
        }

        public void metodoCarga() //Metodo Que Carga Los Reportes
        {
            DataTable dt = new DataTable();
            MySqlCommand comando = new MySqlCommand("SET NAMES 'utf8'; Set lc_time_names = 'es_ES'; SELECT t1.idReporteSupervicion AS 'ID', t1.Folio AS 'FOLIO', CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS 'ECONÓMICO', UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'FECHA DEL REPORTE', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'ESTATUS DEL MANTENIMIENTO',  coalesce((SELECT UPPER(CONCAT(r22.codfallo, ' - ', r22.falloesp)) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'CÓDIGO DE FALLO', coalesce((SELECT UPPER(DATE_FORMAT(r24.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'FECHA DEL REPORTE DE MANTENIMIENTO', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'MECÁNICO', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'MECÁNICO DE APOYO',  coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS 'SUPERVISOR', UPPER(t1.HoraEntrada) AS 'HORA DE ENTRADA', UPPER(t1.TipoFallo) AS 'TIPO DE FALLO', UPPER(t1.KmEntrada) AS 'KILOMETRAJE', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'SUBGRUPO DE FALLO', UPPER(t1.DescFalloNoCod) AS 'SUBGRUPO DE FALLO NO CODIFICADO', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'OBSERVACIONES DE SUPERVISIÓN', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'GRUPO DE FALLO', coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'TRABAJO REALIZADO', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'HORA DE INICIO DE MANTENIMIENTO', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'HORA DE TÉRMINO DE MANTENIMIENTO', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'ESPERA DE TIEMPO PARA MANTENIMIENTO', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'DIFERENCIA DE TIEMPO EN MANTENIMIENTO', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'FOLIO DE FACTURA', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'SUPERVISÓ', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'EXISTENCIA DE REFACCIONES EN ALMACEN', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'ESTATUS DE REFACCIONES', coalesce((SELECT UPPER(CONCAT(r23.ApPaterno, ' ', r23.ApMaterno, ' ', r23.nombres)) FROM reportemantenimiento AS r24 INNER JOIN cpersonal AS r23 ON r24.PersonaFinal = r23.idPersona WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'PERSONA QUE FINALIZÓ EL MANTENIMIENTO', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'OBSERVACIONES DEL MANTENIMIENTO' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN careas AS t4 ON t2.areafkcareas = t4.idarea WHERE (SELECT t1.FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate()) AND (SELECT empresaMantenimiento FROM cmodelos WHERE idmodelo = t2.modelofkcmodelos)='" + empresa + "' ORDER BY t1.Folio DESC", v.c.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(comando);
            adp.Fill(dt);
            dgvMantenimiento.DataSource = dt;
            dgvMantenimiento.ClearSelection();
            dgvMantenimiento.Columns[0].Frozen = dgvMantenimiento.Columns[1].Frozen = dgvMantenimiento.Columns[2].Frozen = true;
            dgvMantenimiento.Columns[0].Visible = dgvMantenimiento.Columns[1].Visible = false;
            v.c.dbcon.Close();
        }

        public void metodocargaref() //Metodo Para Cargar Los Datos De Las Refacciones
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES';SELECT t1.NumRefacc as 'PARTIDA', UPPER(t2.nombreRefaccion) AS 'REFACCIÓN',coalesce((select concat(r2.Simbolo,' - ',upper(r2.Nombre)) from cunidadmedida as r2 inner join cfamilias as r3 on r2.idunidadmedida=r3.umfkcunidadmedida inner join cmarcas as r4 on r3.idfamilia=r4.descripcionfkcfamilias inner join crefacciones as r5 on r5.marcafkcmarcas=r4.idmarca WHERE r5.idRefaccion = RefaccionfkCRefaccion),'') as 'UNIDAD DE MEDIDA', UPPER(DATE_FORMAT(t1.FechaPedido, '%W %d %M %Y')) AS 'FECHA DE PEDIDO', t1.Cantidad AS 'CANTIDAD SOLICITADA', COALESCE((t1.CantidadEntregada), 0) AS 'CANTIDAD ENTREGADA', COALESCE((t1.Cantidad - t1.CantidadEntregada), 0) AS 'CANTIDAD POR ENTREGAR', COALESCE((UPPER(EstatusRefaccion)), '') AS 'ESTATUS DE LA REFACCIÓN' FROM pedidosrefaccion as t1 INNER JOIN crefacciones AS t2 on t1.RefaccionfkCRefaccion = t2.idrefaccion INNER JOIN cmarcas as t3 ON t2.marcafkcmarcas = t3.idmarca INNER JOIN cfamilias AS t4 ON t3.descripcionfkcfamilias = t4.idfamilia WHERE FolioPedfkSupervicion ='" + idreportesupervision + "' ORDER BY NumRefacc ASC", v.c.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            adp.Fill(dt);
            dgvPMantenimiento.DataSource = dt;
            v.c.dbcon.Close();
        }

        public void metodocargarefpdf() //Metodo Para Cargar Los Datos De Las Refacciones Para El PDF
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT NumRefacc AS 'PARTIDA', coalesce((SELECT UPPER(r1.nombreRefaccion) FROM crefacciones AS r1 WHERE r1.idrefaccion = RefaccionfkCRefaccion), '') AS 'REFACCION',coalesce((select concat(r2.Simbolo,' - ',upper(r2.Nombre)) from cunidadmedida as r2 inner join cfamilias as r3 on r2.idunidadmedida=r3.umfkcunidadmedida inner join cmarcas as r4 on r3.idfamilia=r4.descripcionfkcfamilias inner join crefacciones as r5 on r5.marcafkcmarcas=r4.idmarca WHERE r5.idRefaccion = RefaccionfkCRefaccion),'') as 'Unidad De Medida' ,UPPER(DATE_FORMAT(FechaPedido, '%W %d %M %Y')) AS 'FECHA DE PEDIDO', Cantidad AS 'CANTIDAD SOLICITADA', coalesce((CantidadEntregada), 0) AS 'CANTIDAD ENTREGADA', coalesce((Cantidad - CantidadEntregada), 0) AS 'CANTIDAD POR ENTREGAR', coalesce((UPPER(EstatusRefaccion)), '') AS 'ESTATUS DE LA REFACCION' FROM pedidosrefaccion WHERE FolioPedfkSupervicion ='" + idreportesupervision + "' ORDER BY NumRefacc ASC", v.c.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            adp.Fill(dt);
            dgvPMantenimiento.DataSource = dt;
            v.c.dbcon.Close();
        }

        public void metodoverificarrefaccionespdf()
        {
            DataTable dt = new DataTable();
            MySqlCommand verificar = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT NumRefacc AS PARTIDA, COALESCE((SELECT UPPER(z1.nombreRefaccion) FROM crefacciones AS z1 WHERE z1.idrefaccion = RefaccionfkCRefaccion), '') AS REFACCIÓN, COALESCE((SELECT CONCAT(z2.Simbolo, ' - ', UPPER(z2.Nombre)) FROM cunidadmedida AS z2 INNER JOIN cfamilias AS z3 ON z2.idunidadmedida = z3.umfkcunidadmedida INNER JOIN cmarcas AS z4 ON z3.idfamilia = z4.descripcionfkcfamilias INNER JOIN crefacciones AS z5 ON z5.marcafkcmarcas = z4.idmarca WHERE z5.idrefaccion = RefaccionfkCRefaccion), '') AS 'UNIDAD DE MEDIDA', UPPER(DATE_FORMAT(FechaPedido, '%W %d %M %Y')) AS 'FECHA DE PEDIDO', Cantidad AS 'CANTIDAD SOLICITADA', COALESCE(CantidadEntregada, 0) AS 'CANTIDAD ENTREGADA', COALESCE(Cantidad - CantidadEntregada, 0) AS 'CANTIDAD POR ENTREGAR', COALESCE(UPPER(EstatusRefaccion), '') AS 'ESTATUS DE LA REFACCIÓN' FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + idreportesupervision + "' AND estatus = 1 ORDER BY NumRefacc ASC", v.c.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(verificar);
            adp.Fill(dt);
            dgvPMantenimiento.DataSource = dt;
            v.c.dbcon.Close();
        }

        public void metodoActualizar() //Actualiza algun registro
        {
            MySqlCommand cmdactualizar = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET FoliofkSupervicion = '" + idreportesupervision + "', FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', HoraInicioM = '" + labelHoraInicioM.Text + "', HoraTerminoM = '" + labelHoraTerminoM.Text + "', EsperaTiempoM = '" + textBoxEsperaMan.Text + "', DiferenciaTiempoM = '" + textBoxTerminoMan.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', Estatus = '" + comboBoxEstatusMant.Text + "', ExistenciaRefaccAlm = '" + existenciaif + "', StatusRefacciones= '" + comboBoxReqRefacc.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
            cmdactualizar.ExecuteNonQuery();
            v.c.dbcon.Close();
            if ((labelNomSuperviso.Text != "...") && (labelNomMecanicoApo.Text != ".."))
            {
                MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET MecanicoApoyofkPersonal = '" + labelidMecanicoApo.Text + "',  SupervisofkPersonal = '" + labelidSuperviso.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                cmd.ExecuteNonQuery();
                v.c.dbcon.Close();
            }
            else if (labelNomMecanicoApo.Text.Equals("..") && (labelNomSuperviso.Text != "..."))
            {
                MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET SupervisofkPersonal = '" + labelidSuperviso.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                cmd.ExecuteNonQuery();
                v.c.dbcon.Close();
            }
            else if (labelNomSuperviso.Text.Equals("...") && (labelNomMecanicoApo.Text != ".."))
            {
                MySqlCommand cmd = new MySqlCommand("SET lc_time_names = 'es_ES'; UPDATE reportemantenimiento SET MecanicoApoyofkPersonal = '" + labelidMecanicoApo.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                cmd.ExecuteNonQuery();
                v.c.dbcon.Close();
            }
        }

        public void conteo() //Realiza El Conteo De Los Reportes
        {
            MySqlCommand cmd = new MySqlCommand(string.Format(" SELECT (SELECT COUNT(b1.Estatus) AS EstatusEnProc FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion WHERE Estatus = 'EN PROCESO' && (b2.FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate()) AND (SELECT (SELECT empresamantenimiento FROM cmodelos WHERE idmodelo = modelofkcmodelos) FROM cunidades WHERE idunidad = b2.UnidadfkcUnidades)= '{0}') AS EstatusEnProceso, (SELECT COUNT(Estatus) AS EstatusReprog FROM reportemantenimiento WHERE Estatus = 'REPROGRAMADA' && (FechaReporteM BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate()) AND (SELECT (SELECT empresamantenimiento FROM cmodelos WHERE idmodelo = modelofkcmodelos) FROM cunidades WHERE idunidad = (SELECT UnidadfkcUnidades FROM reportesupervicion WHERE idreportesupervicion = FoliofkSupervicion))= '{0}') AS EstatusReprogramada, (SELECT COUNT(*) AS EstatusEnEspera FROM reportesupervicion AS t1 WHERE t1.idReporteSupervicion NOT IN(SELECT t2.FoliofkSupervicion FROM reportemantenimiento AS t2 WHERE t1.idReporteSupervicion = t2.FoliofkSupervicion) && (FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate()) AND (SELECT (SELECT empresamantenimiento FROM cmodelos WHERE idmodelo = modelofkcmodelos) FROM cunidades WHERE idunidad = T1.UnidadfkcUnidades)= '{0}') AS 'Estatus En Espera', COUNT(r1.Estatus) AS EstatusLib FROM reportemantenimiento AS r1 INNER JOIN reportesupervicion AS r2 ON r1.FoliofkSupervicion = r2.idReporteSupervicion WHERE Estatus = 'LIBERADA' && (r2.FechaReporte BETWEEN (DATE_ADD(curdate(), INTERVAL -1 DAY)) AND curdate()) and (SELECT (SELECT empresamantenimiento FROM cmodelos WHERE idmodelo = modelofkcmodelos) FROM cunidades WHERE idunidad = r2.UnidadfkcUnidades)= '{0}'", empresa), v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBoxLiberadas.Text = Convert.ToString(dr.GetString("EstatusLib"));
                textBoxEnProceso.Text = Convert.ToString(dr.GetString("EstatusEnProceso"));
                textBoxEnEspera.Text = Convert.ToString(dr.GetString("Estatus En Espera"));
                textBoxReprogramados.Text = Convert.ToString(dr.GetString("EstatusReprogramada"));
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        public void conteovariable()
        {
            string varproceso = "", varreprogramada = "", varespera = "", varliberada = "", wh = "";
            String Fini = "", Ffin = "";
            if (!string.IsNullOrWhiteSpace(textBoxFolioB.Text))
            {
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' C AND b2.Folio = '" + textBoxFolioB.Text + "'";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b2.Folio = '" + textBoxFolioB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "'";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (b2.Folio = '" + textBoxFolioB.Text + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades) ";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b2.Folio = '" + textBoxFolioB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    wh = " WHERE FoliofkSupervicion = (SELECT w1.FoliofkSupervicion AS EstatusEnProc FROM reportemantenimiento AS w1 INNER JOIN reportesupervicion AS w2 ON w1.FoliofkSupervicion = w2.idReporteSupervicion WHERE w2.Folio = '" + textBoxFolioB.Text + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
                else
                {
                    varproceso += " AND b2.Folio = '" + textBoxFolioB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada += " AND b2.Folio = '" + textBoxFolioB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera += " AND b2.Folio = '" + textBoxFolioB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada += " AND b2.Folio = '" + textBoxFolioB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
            }

            if (comboBoxUnidadB.SelectedIndex > 0)
            {
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
                else
                {
                    varproceso += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada += " AND b2.UnidadfkCUnidades = '" + comboBoxUnidadB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
            }

            if (comboBoxMecanicoB.SelectedIndex > 0)
            {
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera = " WHERE (b1.Estatus = '' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
                else
                {
                    varproceso += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada += " AND b1.MecanicofkPersonal = '" + comboBoxMecanicoB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
            }

            if (comboBoxEstatusMB.SelectedIndex > 0)
            {
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera = " WHERE (b1.Estatus = '' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b1.Estatus = '" + comboBoxEstatusMB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
                else
                {
                    varproceso += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada += " AND b1.Estatus = '" + comboBoxEstatusMB.Text + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
            }

            if (checkBoxFechas.Checked)
            {
                Fini = dateTimePickerIni.Value.ToString("yyyy-MM-dd");
                Ffin = dateTimePickerFin.Value.ToString("yyyy-MM-dd");
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND ((SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "'))";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
                else
                {
                    varproceso += " AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada += " AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera += " AND ((SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada += " AND (SELECT b2.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
            }

            if (comboBoxDescpFalloB.SelectedIndex > 0)
            {
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "')";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
                else
                {
                    varproceso += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + " AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)'";
                    varliberada += " AND b2.DescrFallofkcdescfallo = '" + comboBoxDescpFalloB.SelectedValue + "' AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
            }

            if (comboBoxMesB.SelectedIndex > 0)
            {
                if (varproceso == "")
                {
                    varproceso = " WHERE b1.Estatus = 'EN PROCESO' AND DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '" + month + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada = " WHERE b1.Estatus = 'REPROGRAMADA' AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '" + month + "')) AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera = " WHERE b2.idReporteSupervicion NOT IN(SELECT b1.FoliofkSupervicion FROM reportemantenimiento AS b1 WHERE b2.idReporteSupervicion = b1.FoliofkSupervicion) AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '" + month + "')) AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada = " WHERE b1.Estatus = 'LIBERADA' and DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '" + month + "') AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
                else
                {
                    varproceso += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11')) AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varreprogramada += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11')) AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varespera += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11')) AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                    varliberada += " AND (DATE_FORMAT(FechaReporte, '%Y-%m') = concat(YEAR(now()), '-', '11')) AND (SELECT (select empresaMantenimiento FROM cmodelos WHERe idmodelo = modelofkcmodelos) ='" + empresa + "' FROM cunidades WHERE idunidad = b2.unidadfkcunidades)";
                }
            }

            string Cconsulta = "SELECT (SELECT COUNT(b1.Estatus) AS EstatusEnProc FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion" + varproceso + ") AS EstatusEnProceso, (SELECT COUNT(Estatus) AS EstatusReprog FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion" + varreprogramada + ") AS EstatusReprogramada, coalesce((SELECT COUNT(b2.idReporteSupervicion) AS EstatusEnEspera FROM reportesupervicion AS b2" + varespera + "), '') AS 'Estatus En Espera', COUNT(Estatus) AS EstatusLib FROM reportemantenimiento AS b1 INNER JOIN reportesupervicion AS b2 ON b1.FoliofkSupervicion = b2.idReporteSupervicion" + varliberada + "" + "";
            MySqlCommand cmd = new MySqlCommand(Cconsulta, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBoxLiberadas.Text = Convert.ToString(dr.GetString("EstatusLib"));
                textBoxEnProceso.Text = Convert.ToString(dr.GetString("EstatusEnProceso"));
                textBoxEnEspera.Text = Convert.ToString(dr.GetString("Estatus En Espera"));
                textBoxReprogramados.Text = Convert.ToString(dr.GetString("EstatusReprogramada"));
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        public void conteoiniref() //Realiza Un Conteo Inicial Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                labelrefini.Text = Convert.ToString(dr.GetString("Folio"));
            dr.Close();
            v.c.dbcon.Close();
        }

        public void conteofinref() //Realiza Un Conteo Final Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                labelreffin.Text = Convert.ToString(dr.GetString("Folio"));
            dr.Close();
            v.c.dbcon.Close();
        }

        public void ncontrefini() //Realiza Un Conteo Inicial Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                conteorefaccionesinicial = Convert.ToInt32(dr.GetString("Folio"));
            dr.Close();
            v.c.dbcon.Close();
        }

        public void ncontreffin() //Realiza Un Conteo Final Para Saber Si No Hubo Algun Cambio En El GridView
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(FolioPedfkSupervicion) AS Folio FROM pedidosrefaccion WHERE FolioPedfkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                conteorefaccionesfinal = Convert.ToInt32(dr.GetString("Folio"));
            dr.Close();
            v.c.dbcon.Close();
        }

        private void escribirFichero(string texto)
        {
            string rutaFichero = Application.StartupPath; ;
            rutaFichero = rutaFichero + "/PDFTempral";
            try
            {
                if (!(Directory.Exists(rutaFichero)))
                    Directory.CreateDirectory(rutaFichero);
            }
            catch (Exception errorC)
            {
                MessageBox.Show("Ha habido un error al intentar " + "crear el fichero temporal:" + Environment.NewLine + Environment.NewLine + rutaFichero + Environment.NewLine + Environment.NewLine + errorC.Message, "Error al crear fichero temporal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void topdfconunidades()
        {
            MySqlCommand unidades = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT COALESCE((SELECT CONCAT(b2.identificador, LPAD(b1.consecutivo, 4, '0')) FROM cunidades AS b1 INNER JOIN careas AS b2 ON b1.areafkcareas = b2.idarea WHERE b1.idunidad = t1.unidadfkcunidades))AS 'Unidad', COALESCE((SELECT b3.bin FROM cunidades AS b3 WHERE b3.idunidad = t1.unidadfkcunidades), '') AS 'Vin', COALESCE((SELECT UPPER(b4.estatus) FROM reportemantenimiento AS b4 WHERE b4.FoliofkSupervicion = t1.idReporteSupervicion ), '') AS 'Estatus', UPPER(CONCAT(COALESCE((SELECT b5.marca FROM cunidades AS b5 WHERE b5.idunidad = t1.unidadfkcunidades), ''), ' / ', COALESCE((SELECT b6.modelo FROM cunidades AS b6 WHERE b6.idunidad = t1.unidadfkcunidades), ''))) AS 'Marca / Modelo', COALESCE((SELECT b7.nmotor FROM cunidades AS b7 WHERE b7.idunidad = t1.unidadfkcunidades), '') AS 'Núm. Motor', COALESCE((SELECT b8.ntransmision FROM cunidades AS b8 WHERE b8.idunidad = t1.unidadfkcunidades), '') AS 'Núm. Transmisión', COALESCE(UPPER((SELECT b9.trabajorealizado FROM reportemantenimiento AS b9 WHERE t1.idReporteSupervicion = b9.FoliofkSupervicion)), '') AS 'Trabajo Realizado' FROM reportesupervicion AS t1 WHERE t1.idReporteSupervicion = '" + idgeneral + "'", v.c.dbconection());
            MySqlDataReader dr = unidades.ExecuteReader();
            if (dr.Read())
            {
                unidadpdf = dr.GetString("Unidad");
                vinpdf = dr.GetString("Vin");
                estatuspdf = dr.GetString("Estatus");
                marcamodelopdf = dr.GetString("Marca / Modelo");
                nmotorpdf = dr.GetString("Núm. Motor");
                ntransmisionpdf = dr.GetString("Núm. Transmisión");
                trabajorealizadopdf = dr.GetString("Trabajo Realizado");
            }
            dr.Close();
            v.c.dbcon.Close();
            Document dc = new Document(PageSize.LETTER);
            dc.SetMargins(21f, 21f, 31f, 31f);
            PdfPTable tb = new PdfPTable(4);
            tb.WidthPercentage = 100; // CAMBIAR A 95 SI NO FUNCIONA
            tb.LockedWidth = true;
            float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f, .8f };
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\Desktop";
            saveFileDialog1.Title = "Guardar reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            escribirFichero("");
            string filename = Application.StartupPath + "/PDFTempral/Orden_" + gbxMantenimiento.Text + DateTime.Today.ToLongDateString() + ".pdf";
            DialogResult ews = DialogResult.OK;
            try
            {
                if ((ews = saveFileDialog1.ShowDialog()) == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    string p = Path.GetExtension(filename);
                    if (p.ToLower() != ".pdf")
                        filename = filename + ".pdf";
                }
                if (ews == DialogResult.OK)
                {
                    if (filename.Trim() != "")
                    {
                        FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        PdfWriter writer = PdfWriter.GetInstance(dc, file);
                        dc.Open();
                        byte[] img = null;
                        if (empresa == 2)
                            img = Convert.FromBase64String(v.tri);
                        else
                            img = Convert.FromBase64String(v.tri);
                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                        imagen.ScalePercent(17f);
                        imagen.SetAbsolutePosition(433, 689);
                        dc.Add(imagen);

                        PdfContentByte cb = writer.DirectContent;
                        cb.SetLineWidth(0.5f);
                        int y = 529;
                        for (int i = 1; i <= 2; i++)
                        {
                            cb.MoveTo(561, y);
                            cb.LineTo(51, y);
                            y = y - 16;
                        }
                        cb.Stroke();
                        var content = writer.DirectContent;
                        var pageBorderRect = new iTextSharp.text.Rectangle(dc.PageSize);
                        pageBorderRect.Left += dc.LeftMargin;
                        pageBorderRect.Right -= dc.RightMargin;
                        pageBorderRect.Top -= dc.TopMargin;
                        pageBorderRect.Bottom += dc.BottomMargin;
                        content.SetColorStroke(BaseColor.BLACK);
                        content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                        content.SetLineWidth(2f);
                        content.Stroke();
                        PdfPTable tball = new PdfPTable(19);
                        tball.DefaultCell.Border = 1;
                        tball.WidthPercentage = 100; // CAMBIAR A 95
                        tball.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell c0s1 = new PdfPCell();
                        c0s1.Border = 0;
                        tball.AddCell(c0s1);
                        PdfPCell c0s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c0s2_18.Border = 0;
                        c0s2_18.Colspan = 17;
                        tball.AddCell(c0s2_18);
                        PdfPCell c0s19 = new PdfPCell();
                        c0s19.Border = 0;
                        tball.AddCell(c0s19);
                        PdfPCell c1_46s1 = new PdfPCell();
                        c1_46s1.Border = 0;
                        c1_46s1.Rowspan = 13;//13
                        tball.AddCell(c1_46s1);
                        PdfPCell c1_2s2_13 = new PdfPCell(new Phrase("\nREPORTE MANTENIMIENTO (UNIDAD)\n\n", FontFactory.GetFont("CALIBRI", 14, iTextSharp.text.Font.BOLD)));
                        c1_2s2_13.Border = 0;
                        c1_2s2_13.Colspan = 12;
                        c1_2s2_13.Rowspan = 2;
                        c1_2s2_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        c1_2s2_13.BorderColorTop = c1_2s2_13.BorderColorRight = c1_2s2_13.BorderColorLeft = BaseColor.BLACK;
                        c1_2s2_13.BorderWidthTop = c1_2s2_13.BorderWidthRight = c1_2s2_13.BorderWidthLeft = 1f;
                        tball.AddCell(c1_2s2_13);
                        PdfPCell c1_3s14_18 = new PdfPCell();
                        c1_3s14_18.Border = 0;
                        c1_3s14_18.Colspan = 5;
                        c1_3s14_18.Rowspan = 3;
                        c1_3s14_18.BorderColorTop = c1_3s14_18.BorderColorRight = c1_3s14_18.BorderColorBottom = BaseColor.BLACK;
                        c1_3s14_18.BorderWidthTop = c1_3s14_18.BorderWidthRight = c1_3s14_18.BorderWidthBottom = 1f;
                        tball.AddCell(c1_3s14_18);
                        PdfPCell c1_46s19 = new PdfPCell();
                        c1_46s19.Border = 0;
                        c1_46s19.Rowspan = 11; //11
                        tball.AddCell(c1_46s19);
                        PdfPCell c3s2_13 = new PdfPCell(new Phrase("FECHA / HORA DEL REPORTE: " + DateTime.Now.ToString("dddd dd - MMMM - yyyy / hh:mm", CultureInfo.CreateSpecificCulture("es-ES")).ToUpper(), FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        c3s2_13.Border = 0;
                        c3s2_13.Colspan = 12;
                        c3s2_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        c3s2_13.BorderColorTop = c3s2_13.BorderColorRight = c3s2_13.BorderColorBottom = c3s2_13.BorderColorLeft = BaseColor.BLACK;
                        c3s2_13.BorderWidthTop = c3s2_13.BorderWidthRight = c3s2_13.BorderWidthBottom = c3s2_13.BorderWidthLeft = 1f;
                        tball.AddCell(c3s2_13);
                        PdfPCell c4s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c4s2_18.Border = 0;
                        c4s2_18.Colspan = 17;
                        tball.AddCell(c4s2_18);
                        PdfPCell c5s2_3 = new PdfPCell(new Phrase("UNIDAD", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c5s2_3.Border = 0;
                        c5s2_3.Colspan = 2;
                        c5s2_3.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c5s2_3);
                        PdfPCell c5s3_5 = new PdfPCell(new Phrase(unidadpdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c5s3_5.Border = 0;
                        c5s3_5.Colspan = 3;
                        c5s3_5.BorderColorBottom = BaseColor.BLACK;
                        c5s3_5.BorderWidthBottom = 0.5f;
                        c5s3_5.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c5s3_5);
                        PdfPCell c5s6_7 = new PdfPCell(new Phrase("VIN", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c5s6_7.Border = 0;
                        c5s6_7.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c5s6_7);
                        PdfPCell c5s8_11 = new PdfPCell(new Phrase(vinpdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c5s8_11.Border = 0;
                        c5s8_11.Colspan = 5;
                        c5s8_11.BorderColorBottom = BaseColor.BLACK;
                        c5s8_11.BorderWidthBottom = 0.5f;
                        c5s8_11.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c5s8_11);
                        PdfPCell c5s12_13 = new PdfPCell(new Phrase("ESTATUS", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c5s12_13.Border = 0;
                        c5s12_13.Colspan = 2;
                        c5s12_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c5s12_13);
                        PdfPCell c5s14_18 = new PdfPCell(new Phrase(estatuspdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c5s14_18.Border = 0;
                        c5s14_18.Colspan = 4;
                        c5s14_18.BorderColorBottom = BaseColor.BLACK;
                        c5s14_18.BorderWidthBottom = 0.5f;
                        c5s14_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c5s14_18);
                        PdfPCell c6s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c6s2_18.Border = 0;
                        c6s2_18.Colspan = 17;
                        tball.AddCell(c6s2_18);
                        PdfPCell c7s2_4 = new PdfPCell(new Phrase("MARCA / MODELO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c7s2_4.Border = 0;
                        c7s2_4.Colspan = 3;
                        c7s2_4.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c7s2_4);
                        PdfPCell c7s5_10 = new PdfPCell(new Phrase(marcamodelopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c7s5_10.Border = 0;
                        c7s5_10.Colspan = 6;
                        c7s5_10.BorderColorBottom = BaseColor.BLACK;
                        c7s5_10.BorderWidthBottom = 0.5f;
                        c7s5_10.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c7s5_10);
                        PdfPCell c7s11_13 = new PdfPCell(new Phrase("NÚM DE MOTOR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c7s11_13.Border = 0;
                        c7s11_13.Colspan = 3;
                        c7s11_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c7s11_13);
                        PdfPCell c7s14_18 = new PdfPCell(new Phrase(nmotorpdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c7s14_18.Border = 0;
                        c7s14_18.Colspan = 5;
                        c7s14_18.BorderColorBottom = BaseColor.BLACK;
                        c7s14_18.BorderWidthBottom = 0.5f;
                        c7s14_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c7s14_18);
                        PdfPCell c8s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c8s2_18.Border = 0;
                        c8s2_18.Colspan = 17;
                        tball.AddCell(c8s2_18);
                        PdfPCell c9s2_5 = new PdfPCell(new Phrase("NÚM DE TRANSMISIÓN", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c9s2_5.Border = 0;
                        c9s2_5.Colspan = 4;
                        c9s2_5.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c9s2_5);
                        PdfPCell c9s6_18 = new PdfPCell(new Phrase(ntransmisionpdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c9s6_18.Border = 0;
                        c9s6_18.Colspan = 13;
                        c9s6_18.BorderColorBottom = BaseColor.BLACK;
                        c9s6_18.BorderWidthBottom = 0.5f;
                        c9s6_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c9s6_18);
                        PdfPCell c10s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c10s2_18.Border = 0;
                        c10s2_18.Colspan = 17;
                        tball.AddCell(c10s2_18);
                        PdfPCell c11s2_5 = new PdfPCell(new Phrase("TRABAJO REALIZADO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c11s2_5.Border = 0;
                        c11s2_5.Colspan = 4;
                        c11s2_5.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c11s2_5);
                        PdfPCell c11s6_18 = new PdfPCell();
                        c11s6_18.Border = 0;
                        c11s6_18.Colspan = 13;
                        tball.AddCell(c11s6_18);
                        PdfPCell c12_13s2_18 = new PdfPCell(new Phrase(trabajorealizadopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c12_13s2_18.Border = 0;
                        c12_13s2_18.Colspan = 17;
                        c12_13s2_18.Rowspan = 2;
                        c12_13s2_18.SetLeading(0, 2);
                        c12_13s2_18.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        tball.AddCell(c12_13s2_18);
                        PdfPCell c12_13s19 = new PdfPCell(new Phrase("1\n2", FontFactory.GetFont("CALIBRI", 15, iTextSharp.text.BaseColor.WHITE)));
                        c12_13s19.Border = 0;
                        c12_13s19.Rowspan = 2;
                        tball.AddCell(c12_13s19);
                        dc.Add(tball);
                        if (validaciontablarefacciones)
                        {
                            Paragraph txt1 = new Paragraph();
                            txt1.Add(new Paragraph("EL MECÁNICO NO SOLICITÓ NINGUNA REFACCIÓN", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            txt1.IndentationLeft = 30;
                            dc.Add(txt1);
                        }
                        else
                            GenerarDocumento(dc, writer);
                        dc.AddCreationDate();
                        dc.Close();
                        Process.Start(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede exportar el archivo en formato PDF", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void topdfgeneral()
        {
            MySqlCommand general = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT UPPER(t1.Folio) AS Folio, UPPER(CONCAT(t3.identificador, LPAD(consecutivo, 4,'0'))) AS Unidad, COALESCE(t1.KmEntrada, '') AS Kilometraje, UPPER(CONCAT(COALESCE(DATE_FORMAT(t1.FechaReporte, '%W, %d %M %Y'), ''), ' / ', COALESCE(TIME_FORMAT(t1.HoraEntrada, '%H:%i')))) AS 'Fecha / Hora', COALESCE(UPPER((SELECT t4.codfallo FROM cfallosesp AS t4 WHERE t1.CodFallofkcfallosesp = t4.idfalloEsp)), '') AS 'Código De Fallo', COALESCE(UPPER((SELECT t5.descfallo FROM cdescfallo AS t5 WHERE t1.DescrFallofkcdescfallo = t5.iddescfallo)), '') AS 'Descripcion De Fallo', COALESCE(UPPER((t1.DescFalloNoCod)), '') AS 'Fallo No Codificado', COALESCE(UPPER((SELECT CONCAT(t4.ApPaterno, ' ', t4.ApMaterno, ' ', t4.nombres) FROM cpersonal AS t4 WHERE t1.SupervisorfkCPersonal = t4.idPersona)), '') AS 'Supervisor', COALESCE(UPPER((t1.ObservacionesSupervision)), '') AS 'Observaciones de Supervisión', COALESCE(UPPER((SELECT t5.nombreFalloGral FROM cfallosgrales AS t5 INNER JOIN reportemantenimiento AS t6 ON t6.FalloGralfkFallosGenerales = t5.idFalloGral WHERE t6.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Grupo de Fallo', COALESCE(UPPER((SELECT t7.Estatus FROM reportemantenimiento AS t7 WHERE t7.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Estatus Del Mantenimiento', COALESCE(UPPER((SELECT CONCAT(t8.ApPaterno, ' ', t8.ApMaterno, ' ', t8.nombres) FROM cpersonal AS t8 INNER JOIN reportemantenimiento AS t9 ON t8.idPersona = t9.MecanicofkPersonal WHERE t9.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Mecánico', COALESCE(UPPER((SELECT CONCAT(t10.ApPaterno, ' ', t10.ApMaterno, ' ', t10.nombres) FROM cpersonal AS t10 INNER JOIN reportemantenimiento AS t11 ON t10.idPersona = t11.MecanicoApoyofkPersonal WHERE t11.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'M. Apoyo', COALESCE(UPPER((SELECT CONCAT(t12.ApPaterno, ' ', t12.ApMaterno, ' ', t12.nombres) FROM cpersonal AS t12 INNER JOIN reportemantenimiento AS t13 ON t12.idPersona = t13.SupervisofkPersonal WHERE t13.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Supervisó', COALESCE(UPPER((SELECT t19.EsperaTiempoM FROM reportemantenimiento AS t19 WHERE t19.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Tiempo De Espera', UPPER(CONCAT(COALESCE((SELECT DATE_FORMAT(t17.HoraInicioM, '%H:%i') FROM reportemantenimiento AS t17 WHERE t17.FoliofkSupervicion = t1.idReporteSupervicion), ''), ' / ', COALESCE((SELECT DATE_FORMAT(t18.HoraTerminoM, '%H:%i') FROM reportemantenimiento AS t18 WHERE t18.FoliofkSupervicion = t1.idReporteSupervicion), ''))) AS 'Hora de Inicio / Término', COALESCE(UPPER((SELECT t20.DiferenciaTiempoM FROM reportemantenimiento AS t20 WHERE t20.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Diferencia De Tiempo', COALESCE(UPPER((SELECT t21.TrabajoRealizado FROM reportemantenimiento AS t21 WHERE t21.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Trabajo Realizado', COALESCE(UPPER((SELECT t22.ObservacionesM FROM reportemantenimiento AS t22 WHERE t22.FoliofkSupervicion = t1.idReporteSupervicion)), '') AS 'Observaciones de Mantenimiento' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN careas AS t3 ON t2.areafkcareas = t3.idarea WHERE t1.idReporteSupervicion = '" + idgeneral + "'", v.c.dbconection());
            MySqlDataReader dr = general.ExecuteReader();
            if (dr.Read())
            {
                foliopdf = dr.GetString("Folio");
                unidadpdf = dr.GetString("Unidad");
                kilometrajepdf = dr.GetString("Kilometraje");
                fechahorapdf = dr.GetString("Fecha / Hora");
                codigofallopdf = dr.GetString("Código De Fallo");
                descripcionfallopdf = dr.GetString("Descripcion De Fallo");
                descripcionfallonocodificadopdf = dr.GetString("Fallo No Codificado");
                supervisorpdf = dr.GetString("Supervisor");
                observacionesupervisionpdf = dr.GetString("Observaciones de Supervisión");
                grupofallopdf = dr.GetString("Grupo de Fallo");
                estatuspdf = dr.GetString("Estatus del Mantenimiento");
                mecanicopdf = dr.GetString("Mecánico");
                mapoyopdf = dr.GetString("M. Apoyo");
                supervisorpdf = dr.GetString("Supervisó");
                tiempoesperapdf = dr.GetString("Tiempo de Espera");
                horainicioterminopdf = dr.GetString("Hora de Inicio / Término");
                diferenciapdf = dr.GetString("Diferencia De Tiempo");
                trabajorealizadopdf = dr.GetString("Trabajo Realizado");
                observacionesmantenimientopdf = dr.GetString("Observaciones de Mantenimiento");
            }
            dr.Close();
            v.c.dbcon.Close();
            Document dc = new Document(PageSize.LETTER);
            dc.SetMargins(21f, 21f, 31f, 31f);
            PdfPTable tb = new PdfPTable(4);
            tb.WidthPercentage = 100; // CAMBIAR A 95 SI NO FUNCIONA
            tb.LockedWidth = true;
            float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f, .8f };
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\Desktop";
            saveFileDialog1.Title = "Guardar reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            escribirFichero("");
            string filename = Application.StartupPath + "/PDFTempral/Orden_" + gbxMantenimiento.Text + DateTime.Today.ToLongDateString() + ".pdf";
            DialogResult ews = DialogResult.OK;
            try
            {
                if ((ews = saveFileDialog1.ShowDialog()) == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    string p = Path.GetExtension(filename);
                    if (p.ToLower() != ".pdf")
                        filename = filename + ".pdf";
                }
                if (ews == DialogResult.OK)
                {
                    if (filename.Trim() != "")
                    {
                        FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        PdfWriter writer = PdfWriter.GetInstance(dc, file);
                        dc.Open();
                        byte[] img = null;
                        if (empresa == 2)
                            img = Convert.FromBase64String(v.tri);
                        else if (empresa == 3)
                            img = Convert.FromBase64String(v.TSD);
                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                        imagen.ScalePercent(17f);
                        imagen.SetAbsolutePosition(433, 689);
                        dc.Add(imagen);
                        var content = writer.DirectContent;
                        var pageBorderRect = new iTextSharp.text.Rectangle(dc.PageSize);
                        pageBorderRect.Left += dc.LeftMargin;
                        pageBorderRect.Right -= dc.RightMargin;
                        pageBorderRect.Top -= dc.TopMargin;
                        pageBorderRect.Bottom += dc.BottomMargin;
                        content.SetColorStroke(BaseColor.BLACK);
                        content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                        content.SetLineWidth(2f);
                        content.Stroke();

                        PdfContentByte cb = writer.DirectContent;
                        cb.SetLineWidth(0.5f);
                        int y = 435;
                        int parte = 0;
                        for (int i = 1; i <= 2 && parte == 0; i++)
                        {
                            cb.MoveTo(561, y);
                            cb.LineTo(51, y);
                            y = y - 16;
                        }
                        parte++;
                        y = 206;
                        for (int i = 1; i <= 4 && parte == 1; i++)
                        {
                            cb.MoveTo(291, y);
                            cb.LineTo(51, y);
                            y = y - 16;
                        }
                        parte++;
                        y = 206;
                        for (int i = 1; i <= 4 && parte == 2; i++)
                        {
                            cb.MoveTo(561, y);
                            cb.LineTo(321, y);
                            y = y - 16;
                        }
                        cb.Stroke();
                        PdfPTable tball = new PdfPTable(19);
                        tball.DefaultCell.Border = 1;
                        tball.WidthPercentage = 100; // CAMBIAR A 95
                        tball.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell c0s1 = new PdfPCell();
                        c0s1.Border = 0;
                        tball.AddCell(c0s1);
                        PdfPCell c0s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c0s2_18.Border = 0;
                        c0s2_18.Colspan = 17;
                        tball.AddCell(c0s2_18);
                        PdfPCell c0s19 = new PdfPCell();
                        c0s19.Border = 0;
                        tball.AddCell(c0s19);
                        PdfPCell c1_46s1 = new PdfPCell();
                        c1_46s1.Border = 0;
                        c1_46s1.Rowspan = 41;//41
                        tball.AddCell(c1_46s1);
                        PdfPCell c1_2s2_13 = new PdfPCell(new Phrase("\nREPORTE MANTENIMIENTO\n\n", FontFactory.GetFont("CALIBRI", 14, iTextSharp.text.Font.BOLD)));
                        c1_2s2_13.Border = 0;
                        c1_2s2_13.Colspan = 12;
                        c1_2s2_13.Rowspan = 2;
                        c1_2s2_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        c1_2s2_13.BorderColorTop = c1_2s2_13.BorderColorRight = c1_2s2_13.BorderColorLeft = BaseColor.BLACK;
                        c1_2s2_13.BorderWidthTop = c1_2s2_13.BorderWidthRight = c1_2s2_13.BorderWidthLeft = 1f;
                        tball.AddCell(c1_2s2_13);
                        PdfPCell c1_3s14_18 = new PdfPCell();
                        c1_3s14_18.Border = 0;
                        c1_3s14_18.Colspan = 5;
                        c1_3s14_18.Rowspan = 3;
                        c1_3s14_18.BorderColorTop = c1_3s14_18.BorderColorRight = c1_3s14_18.BorderColorBottom = BaseColor.BLACK;
                        c1_3s14_18.BorderWidthTop = c1_3s14_18.BorderWidthRight = c1_3s14_18.BorderWidthBottom = 1f;
                        tball.AddCell(c1_3s14_18);
                        PdfPCell c1_46s19 = new PdfPCell();
                        c1_46s19.Border = 0;
                        c1_46s19.Rowspan = 17; //17
                        tball.AddCell(c1_46s19);
                        PdfPCell c3s2_13 = new PdfPCell(new Phrase("FECHA / HORA DEL REPORTE: " + DateTime.Now.ToString("dddd dd - MMMM - yyyy / hh:mm", CultureInfo.CreateSpecificCulture("es-ES")).ToUpper(), FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        c3s2_13.Border = 0;
                        c3s2_13.Colspan = 12;
                        c3s2_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        c3s2_13.BorderColorTop = c3s2_13.BorderColorRight = c3s2_13.BorderColorBottom = c3s2_13.BorderColorLeft = BaseColor.BLACK;
                        c3s2_13.BorderWidthTop = c3s2_13.BorderWidthRight = c3s2_13.BorderWidthBottom = c3s2_13.BorderWidthLeft = 1f;
                        tball.AddCell(c3s2_13);
                        PdfPCell c4s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c4s2_18.Border = 0;
                        c4s2_18.Colspan = 17;
                        tball.AddCell(c4s2_18);
                        PdfPCell c5_6s2_18 = new PdfPCell(new Phrase("SUPERVISIÓN\n\n", FontFactory.GetFont("CALIBRI", 12, iTextSharp.text.Font.BOLD)));
                        c5_6s2_18.Border = 0;
                        c5_6s2_18.Colspan = 17;
                        c5_6s2_18.Rowspan = 2;
                        c5_6s2_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c5_6s2_18);
                        PdfPCell c7s2_3 = new PdfPCell(new Phrase("FOLIO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c7s2_3.Border = 0;
                        c7s2_3.Colspan = 2;
                        c7s2_3.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c7s2_3);
                        PdfPCell c7s4_6 = new PdfPCell(new Phrase(foliopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c7s4_6.Border = 0;
                        c7s4_6.Colspan = 3;
                        c7s4_6.HorizontalAlignment = Element.ALIGN_CENTER;
                        c7s4_6.BorderColorBottom = BaseColor.BLACK;
                        c7s4_6.BorderWidthBottom = 0.5f;
                        tball.AddCell(c7s4_6);
                        PdfPCell c7s7_8 = new PdfPCell(new Phrase("UNIDAD", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c7s7_8.Border = 0;
                        c7s7_8.Colspan = 2;
                        c7s7_8.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c7s7_8);
                        PdfPCell c7s9_11 = new PdfPCell(new Phrase(unidadpdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c7s9_11.Border = 0;
                        c7s9_11.Colspan = 3;
                        c7s9_11.HorizontalAlignment = Element.ALIGN_CENTER;
                        c7s9_11.BorderColorBottom = BaseColor.BLACK;
                        c7s9_11.BorderWidthBottom = 0.5f;
                        tball.AddCell(c7s9_11);
                        PdfPCell c7s12_14 = new PdfPCell(new Phrase("KILOMETRAJE", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c7s12_14.Border = 0;
                        c7s12_14.Colspan = 3;
                        c7s12_14.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c7s12_14);
                        PdfPCell c7s15_18 = new PdfPCell(new Phrase(kilometrajepdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c7s15_18.Border = 0;
                        c7s15_18.Colspan = 4;
                        c7s15_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        c7s15_18.BorderColorBottom = BaseColor.BLACK;
                        c7s15_18.BorderWidthBottom = 0.5f;
                        tball.AddCell(c7s15_18);
                        PdfPCell c8s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c8s2_18.Border = 0;
                        c8s2_18.Colspan = 17;
                        tball.AddCell(c8s2_18);
                        PdfPCell c9s2_4 = new PdfPCell(new Phrase("CÓDIGO DE FALLO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c9s2_4.Border = 0;
                        c9s2_4.Colspan = 3;
                        c9s2_4.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c9s2_4);
                        PdfPCell c9s5_10 = new PdfPCell(new Phrase(codigofallopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c9s5_10.Border = 0;
                        c9s5_10.Colspan = 6;
                        c9s5_10.HorizontalAlignment = Element.ALIGN_CENTER;
                        c9s5_10.BorderColorBottom = BaseColor.BLACK;
                        c9s5_10.BorderWidthBottom = 0.5f;
                        tball.AddCell(c9s5_10);
                        PdfPCell c9s11_13 = new PdfPCell(new Phrase("FECHA / HORA", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c9s11_13.Border = 0;
                        c9s11_13.Colspan = 3;
                        c9s11_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c9s11_13);
                        PdfPCell c9s14_18 = new PdfPCell(new Phrase(fechahorapdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c9s14_18.Border = 0;
                        c9s14_18.Colspan = 5;
                        c9s14_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        c9s14_18.BorderColorBottom = BaseColor.BLACK;
                        c9s14_18.BorderWidthBottom = 0.5f;
                        tball.AddCell(c9s14_18);
                        PdfPCell c10s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c10s2_18.Border = 0;
                        c10s2_18.Colspan = 17;
                        tball.AddCell(c10s2_18);
                        PdfPCell c11s2_6 = new PdfPCell(new Phrase("SUBGRUPO DE FALLO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c11s2_6.Border = 0;
                        c11s2_6.Colspan = 4;
                        c11s2_6.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c11s2_6);
                        PdfPCell c11s7_18 = new PdfPCell(new Phrase(descripcionfallopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c11s7_18.Border = 0;
                        c11s7_18.Colspan = 13;
                        c11s7_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        c11s7_18.BorderColorBottom = BaseColor.BLACK;
                        c11s7_18.BorderWidthBottom = 0.5f;
                        tball.AddCell(c11s7_18);
                        PdfPCell c12s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c12s2_18.Border = 0;
                        c12s2_18.Colspan = 17;
                        tball.AddCell(c12s2_18);
                        PdfPCell c13s2_6 = new PdfPCell(new Phrase("FALLO NO CODIFICADO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c13s2_6.Border = 0;
                        c13s2_6.Colspan = 4;
                        c13s2_6.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c13s2_6);
                        PdfPCell c13s7_18 = new PdfPCell(new Phrase(descripcionfallonocodificadopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c13s7_18.Border = 0;
                        c13s7_18.Colspan = 13;
                        c13s7_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        c13s7_18.BorderColorBottom = BaseColor.BLACK;
                        c13s7_18.BorderWidthBottom = 0.5f;
                        tball.AddCell(c13s7_18);
                        PdfPCell c14s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c14s2_18.Border = 0;
                        c14s2_18.Colspan = 17;
                        tball.AddCell(c14s2_18);
                        PdfPCell c15s2_5 = new PdfPCell(new Phrase("SUPERVISOR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c15s2_5.Border = 0;
                        c15s2_5.Colspan = 3;
                        c15s2_5.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c15s2_5);
                        PdfPCell c15s6_18 = new PdfPCell(new Phrase(supervisorpdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c15s6_18.Border = 0;
                        c15s6_18.Colspan = 14;
                        c15s6_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        c15s6_18.BorderColorBottom = BaseColor.BLACK;
                        c15s6_18.BorderWidthBottom = 0.5f;
                        tball.AddCell(c15s6_18);
                        PdfPCell c16s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c16s2_18.Border = 0;
                        c16s2_18.Colspan = 17;
                        tball.AddCell(c16s2_18);
                        PdfPCell c17s2_5 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c17s2_5.Border = 0;
                        c17s2_5.Colspan = 4;
                        c17s2_5.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c17s2_5);
                        PdfPCell c17s6_18 = new PdfPCell(new Phrase("", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c17s6_18.Border = 0;
                        c17s6_18.Colspan = 13;
                        tball.AddCell(c17s6_18);
                        PdfPCell c18_19s2_18 = new PdfPCell(new Phrase(observacionesupervisionpdf.ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c18_19s2_18.Border = 0;
                        c18_19s2_18.Colspan = 17;
                        c18_19s2_18.Rowspan = 2;
                        c18_19s2_18.SetLeading(0, 2);
                        c18_19s2_18.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        tball.AddCell(c18_19s2_18);
                        PdfPCell c18_19s19 = new PdfPCell(new Phrase("1\n2", FontFactory.GetFont("CALIBRI", 15, iTextSharp.text.BaseColor.WHITE)));
                        c18_19s19.Border = 0;
                        c18_19s19.Rowspan = 2;
                        tball.AddCell(c18_19s19);
                        PdfPCell c20s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c20s2_18.Border = 0;
                        c20s2_18.Colspan = 17;
                        tball.AddCell(c20s2_18);
                        PdfPCell c20_45s19 = new PdfPCell(new Phrase("", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c20_45s19.Border = 0;
                        c20_45s19.Rowspan = 22;  //22
                        tball.AddCell(c20_45s19);
                        PdfPCell c21_22s2_18 = new PdfPCell(new Phrase("MANTENIMIENTO\n\n", FontFactory.GetFont("CALIBRI", 12, iTextSharp.text.Font.BOLD)));
                        c21_22s2_18.Border = 0;
                        c21_22s2_18.Colspan = 17;
                        c21_22s2_18.Rowspan = 2;
                        c21_22s2_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c21_22s2_18);
                        PdfPCell c23s2_4 = new PdfPCell(new Phrase("GRUPO DE FALLO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c23s2_4.Border = 0;
                        c23s2_4.Colspan = 3;
                        c23s2_4.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c23s2_4);
                        PdfPCell c23s5_12 = new PdfPCell(new Phrase(grupofallopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c23s5_12.Border = 0;
                        c23s5_12.Colspan = 8;
                        c23s5_12.BorderColorBottom = BaseColor.BLACK;
                        c23s5_12.BorderWidthBottom = 0.5f;
                        c23s5_12.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c23s5_12);
                        PdfPCell c23s13_15 = new PdfPCell(new Phrase("ESTATUS", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c23s13_15.Border = 0;
                        c23s13_15.Colspan = 2;
                        c23s13_15.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c23s13_15);
                        PdfPCell c23s16_18 = new PdfPCell(new Phrase(estatuspdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c23s16_18.Border = 0;
                        c23s16_18.Colspan = 4;
                        c23s16_18.BorderColorBottom = BaseColor.BLACK;
                        c23s16_18.BorderWidthBottom = 0.5f;
                        c23s16_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c23s16_18);
                        PdfPCell c24s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c24s2_18.Border = 0;
                        c24s2_18.Colspan = 17;
                        tball.AddCell(c24s2_18);
                        PdfPCell c25s2_3 = new PdfPCell(new Phrase("MECÁNICO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c25s2_3.Border = 0;
                        c25s2_3.Colspan = 2;
                        c25s2_3.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c25s2_3);
                        PdfPCell c25s4_10 = new PdfPCell(new Phrase(mecanicopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c25s4_10.Border = 0;
                        c25s4_10.Colspan = 7;
                        c25s4_10.BorderColorBottom = BaseColor.BLACK;
                        c25s4_10.BorderWidthBottom = 0.5f;
                        c25s4_10.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c25s4_10);
                        PdfPCell c25s11_12 = new PdfPCell(new Phrase("M. APOYO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c25s11_12.Border = 0;
                        c25s11_12.Colspan = 2;
                        c25s11_12.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c25s11_12);
                        PdfPCell c25s13_18 = new PdfPCell(new Phrase(mapoyopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c25s13_18.Border = 0;
                        c25s13_18.Colspan = 6;
                        c25s13_18.BorderColorBottom = BaseColor.BLACK;
                        c25s13_18.BorderWidthBottom = 0.5f;
                        c25s13_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c25s13_18);
                        PdfPCell c26s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c26s2_18.Border = 0;
                        c26s2_18.Colspan = 17;
                        tball.AddCell(c26s2_18);
                        PdfPCell c27s2_3 = new PdfPCell(new Phrase("SUPERVISÓ", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c27s2_3.Border = 0;
                        c27s2_3.Colspan = 2;
                        c27s2_3.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c27s2_3);
                        PdfPCell c27s4_10 = new PdfPCell(new Phrase(supervisopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c27s4_10.Border = 0;
                        c27s4_10.Colspan = 6;
                        c27s4_10.BorderColorBottom = BaseColor.BLACK;
                        c27s4_10.BorderWidthBottom = 0.5f;
                        c27s4_10.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c27s4_10);
                        PdfPCell c27s11_13 = new PdfPCell(new Phrase("TIEMPO DE ESPERA", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c27s11_13.Border = 0;
                        c27s11_13.Colspan = 4;
                        c27s11_13.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c27s11_13);
                        PdfPCell c27s14_18 = new PdfPCell(new Phrase(tiempoesperapdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c27s14_18.Border = 0;
                        c27s14_18.Colspan = 5;
                        c27s14_18.BorderColorBottom = BaseColor.BLACK;
                        c27s14_18.BorderWidthBottom = 0.5f;
                        c27s14_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c27s14_18);
                        PdfPCell c28s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c28s2_18.Border = 0;
                        c28s2_18.Colspan = 17;
                        tball.AddCell(c28s2_18);
                        PdfPCell c29s2_6 = new PdfPCell(new Phrase("HORA DE INICIO / TÉRMINO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c29s2_6.Border = 0;
                        c29s2_6.Colspan = 5;
                        c29s2_6.HorizontalAlignment = Element.ALIGN_LEFT;
                        tball.AddCell(c29s2_6);
                        PdfPCell c29s7_10 = new PdfPCell(new Phrase(horainicioterminopdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c29s7_10.Border = 0;
                        c29s7_10.Colspan = 3;
                        c29s7_10.BorderColorBottom = BaseColor.BLACK;
                        c29s7_10.BorderWidthBottom = 0.5f;
                        c29s7_10.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c29s7_10);
                        PdfPCell c29s10_12 = new PdfPCell(new Phrase("DIFERENCIA DE TIEMPO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c29s10_12.Border = 0;
                        c29s10_12.Colspan = 4;
                        c29s10_12.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c29s10_12);
                        PdfPCell c29s13_18 = new PdfPCell(new Phrase(diferenciapdf, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c29s13_18.Border = 0;
                        c29s13_18.Colspan = 5;
                        c29s13_18.BorderColorBottom = BaseColor.BLACK;
                        c29s13_18.BorderWidthBottom = 0.5f;
                        c29s13_18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tball.AddCell(c29s13_18);
                        PdfPCell c30s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                        c30s2_18.Border = 0;
                        c30s2_18.Colspan = 17;
                        tball.AddCell(c30s2_18);
                        PdfPCell c31s2_5 = new PdfPCell(new Phrase("TRABAJO REALIZADO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c31s2_5.Border = 0;
                        c31s2_5.Colspan = 4;
                        tball.AddCell(c31s2_5);
                        PdfPCell c31s6_9 = new PdfPCell();
                        c31s6_9.Border = 0;
                        c31s6_9.Colspan = 4;
                        tball.AddCell(c31s6_9);
                        PdfPCell c31_35s10 = new PdfPCell(new Phrase("1\n2\n3\n4\n5", FontFactory.GetFont("CALIBRI", 15, iTextSharp.text.BaseColor.WHITE)));
                        c31_35s10.Border = 0;
                        c31_35s10.Rowspan = 5;
                        tball.AddCell(c31_35s10);
                        PdfPCell c31s11_13 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        c31s11_13.Border = 0;
                        c31s11_13.Colspan = 3;
                        tball.AddCell(c31s11_13);
                        PdfPCell c31s14_18 = new PdfPCell();
                        c31s14_18.Border = 0;
                        c31s14_18.Colspan = 5;
                        tball.AddCell(c31s14_18);
                        PdfPCell c32_35s2_9 = new PdfPCell(new Phrase(trabajorealizadopdf.ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c32_35s2_9.Border = 0;
                        c32_35s2_9.Colspan = 8;
                        c32_35s2_9.Rowspan = 4;
                        c32_35s2_9.SetLeading(0, 2);
                        c32_35s2_9.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        tball.AddCell(c32_35s2_9);
                        PdfPCell c32_35s11_18 = new PdfPCell(new Phrase(observacionesmantenimientopdf.ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                        c32_35s11_18.Border = 0;
                        c32_35s11_18.Colspan = 8;
                        c32_35s11_18.Rowspan = 4;
                        c32_35s11_18.SetLeading(0, 2);
                        c32_35s11_18.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        tball.AddCell(c32_35s11_18);
                        PdfPCell c36_42s2_18 = new PdfPCell(new Phrase("1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12", FontFactory.GetFont("CALIBRI", 10, iTextSharp.text.BaseColor.WHITE)));
                        c36_42s2_18.Border = 0;
                        c36_42s2_18.Colspan = 17;
                        c36_42s2_18.Rowspan = 6;
                        tball.AddCell(c36_42s2_18);
                        dc.Add(tball);
                        Paragraph txt = new Paragraph();
                        txt.Add(new Paragraph("\nREFACCIONES UTILIZADAS EN EL MANTENIMIENTO\n", FontFactory.GetFont("CALIBRI", 12, iTextSharp.text.Font.BOLD)));
                        txt.IndentationLeft = 30;
                        dc.Add(txt);
                        if (validaciontablarefacciones)
                        {
                            Paragraph txt1 = new Paragraph();
                            txt1.Add(new Paragraph("EL MECÁNICO NO SOLICITÓ NINGUNA REFACCIÓN", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            txt1.IndentationLeft = 30;
                            dc.Add(txt1);
                        }
                        else
                            GenerarDocumento(dc, writer);
                        int pages = dc.PageNumber;
                        dc.AddCreationDate();
                        dc.Close();
                        Process.Start(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede exportar el archivo en formato PDF", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void GenerarDocumento(Document document, PdfWriter writer) //Genera El Documento
        {
            int i, j;
            metodoverificarrefaccionespdf();
            PdfPTable datatable = new PdfPTable(dgvPMantenimiento.ColumnCount);
            datatable.DefaultCell.Padding = 3;
            float[] headerwidths = GetTamañoColumnas(dgvPMantenimiento);
            datatable.SetWidths(headerwidths);
            datatable.WidthPercentage = 89;
            datatable.DefaultCell.BorderWidth = 2;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < dgvPMantenimiento.ColumnCount; i++)
                datatable.AddCell(new Phrase(dgvPMantenimiento.Columns[i].HeaderText.ToString(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(250, 250, 250);
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < dgvPMantenimiento.Rows.Count; i++)
            {
                for (j = 0; j < dgvPMantenimiento.Columns.Count; j++)
                {
                    if (dgvPMantenimiento[j, i].Value != null)
                    {
                        datatable.AddCell(new Phrase(dgvPMantenimiento[j, i].Value.ToString(), FontFactory.GetFont("CALIBRI", 7)));
                        var content1 = writer.DirectContent;
                        var pageBorderRect1 = new iTextSharp.text.Rectangle(document.PageSize);
                        pageBorderRect1.Left += document.LeftMargin;
                        pageBorderRect1.Right -= document.RightMargin;
                        pageBorderRect1.Top -= document.TopMargin;
                        pageBorderRect1.Bottom += document.BottomMargin;
                        content1.SetColorStroke(BaseColor.BLACK);
                        content1.Rectangle(pageBorderRect1.Left, pageBorderRect1.Bottom, pageBorderRect1.Width, pageBorderRect1.Height);
                        content1.SetLineWidth(2f);
                        content1.Stroke();
                    }
                }
                datatable.CompleteRow();
            }
            document.Add(datatable);
        }

        public float[] GetTamañoColumnas(DataGridView dg) //Metodo Del Tamaño De La Tabla PDF
        {
            float[] values = new float[dg.ColumnCount];
            for (int i = 0; i < 8; i++)
                values[i] = (float)dg.Columns[i].Width;
            return values;
        }

        Thread hiloEx;
        delegate void Loading();
        public void cargando1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            buttonExcel.Visible = false;
            label35.Text = "EXPORTANDO";
            label35.Location = new Point(1264, 99);
        }

        delegate void Loading1();
        public void cargando2()
        {
            pictureBoxExcelLoad.Image = null;
            label35.Text = "EXPORTAR";
            label35.Location = new Point(1279, 99);
            buttonExcel.Visible = true;
            if (exportando)
                buttonExcel.Visible = label35.Visible = false;
            exportando = activado = false;
        }


        public void exporta_a_excel() //Metodo Que Genera El Excel
        {
            DataTable dtexcel = new DataTable();
            for (int i = 0; i < dgvMantenimiento.Columns.Count; i++) if (dgvMantenimiento.Columns[i].Visible) dtexcel.Columns.Add(dgvMantenimiento.Columns[i].HeaderText);
            for (int j = dgvMantenimiento.Rows.Count - 1; j >= 0; j--)
            {
                DataRow row = dtexcel.NewRow();
                int indice = 0;
                for (int i = 0; i < dgvMantenimiento.Columns.Count; i++)
                {
                    if (dgvMantenimiento.Columns[i].Visible)
                    {
                        row[dtexcel.Columns[indice]] = dgvMantenimiento.Rows[j].Cells[i].Value;
                        indice++;
                    }
                }
                dtexcel.Rows.Add(row);
            }
            if (dtexcel.Rows.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    Loading load = new Loading(cargando1);
                    this.Invoke(load);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                h.Worksheet sheet = X.ActiveSheet;
                X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                for (int i = 0; i < dtexcel.Columns.Count; i++)
                {
                    h.Range rng = (h.Range)sheet.Cells[1, i + 1];
                    sheet.Cells[1, i + 1] = dtexcel.Columns[i].ColumnName.ToUpper();
                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Crimson);
                    rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                    rng.Font.FontStyle = "Calibri";
                    rng.Font.Bold = true;
                    rng.Font.Size = 12;
                }
                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    for (int j = 0; j < dtexcel.Columns.Count; j++)
                    {
                        try
                        {
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                            sheet.Cells[i + 2, j + 1] = dtexcel.Rows[i][j].ToString();
                            rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230));
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.FontStyle = "Calibri";
                            rng.Font.Size = 11;
                            if (dtexcel.Rows[i][j].ToString() == "EN PROCESO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "LIBERADA".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "REPROGRAMADA".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "CORRECTIVO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "PREVENTIVO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "REITERATIVO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "REPROGRAMADO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "SEGUIMIENTO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(246, 106, 77));
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dtexcel.Rows[i][j].ToString() == "INCOMPLETO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(255, 144, 51));
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                        }
                        catch (System.NullReferenceException)
                        {
                            hiloEx.Abort();
                        }
                    }
                }
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                Thread.Sleep(500);
                if (this.InvokeRequired)
                {
                    Loading1 load1 = new Loading1(cargando2);
                    this.Invoke(load1);
                }
            }
            else
                MessageBox.Show("Es necesario que existan datos en la tabla para poder generar un archivo de excel \nFavor de actualizar la tabla para que se visualizen los reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /* Acciones con los botones y gridview *///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonAgregarMasPed_Click(object sender, EventArgs e) //Regresa Para Meter Mas Refacciones
        {
            banderaeditar = buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = false;
            buttonAgregaPed.Visible = label33.Visible = buttonActualizar.Visible = label26.Visible = true;
            metodocargaref();
            limpiarrefacc();
        }

        private void buttonAgregar_Click(object sender, EventArgs e) //Manda A La Ventana De Refacciones
        {
            groupBoxRefacciones.Visible = true;
            metodocargaref();
            if (!registroconteofilaspedref)
            {
                inicolumn = 0;
                inicolumn = dgvPMantenimiento.Rows.Count;
                validacionfinalconteocolumnas = registroconteofilaspedref = true;
            }
            gbxMantenimiento.Visible = buttonGuardar.Visible = label24.Visible = buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = false;
            buttonAgregaPed.Visible = label33.Visible = dgvPMantenimiento.Visible = label1.Visible = true;
            Cancelar(false);
            if (!registroentradapedref) //CHECAR ESTA VALIDACION
                registroentradapedref = label62.Visible = label63.Visible = true;
            metodocargaref();
            conteoiniref();
            conteofinref();
        }

        bool activado = false;
        private void buttonExcel_Click(object sender, EventArgs e) //Genera Un Documento De Excel
        {
            activado = true;
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
        }

        private void buttonFinalizar_Click(object sender, EventArgs e) //Finalizar Matenimiento
        {
            int cfin = 0;
            if (cfin == 0)
            {
                cfin = cfin + 1;
                cargo = 2;
                if (comboBoxEstatusMant.Text.Equals("LIBERADA"))
                {
                    if (estatusmantGV.Equals("REPROGRAMADA"))
                    {
                        MessageBox.Show("La unidad no puede ser liberada porque primero debe pasar por un proceso antes de terminar el reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        labelHoraTerminoM.Text = "";
                        textBoxTerminoMan.Text = "";
                        validar();
                    }
                    else if (comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES"))
                    {
                        if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- ESTATUS --")))
                        {
                            MessageBox.Show("El reporte no puede ser finalizado porque las refacciones solicitadas aun no son entregadas", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            validar();
                        }
                        else if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                        {
                            if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                            {
                                MessageBox.Show("El Folio de Factura no puede quedar vacio si está validada la Existencia de Refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                            }
                            else if (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text.Trim()))
                            {
                                MessageBox.Show("Introduzca un \"Trabajo Realizado\" Válido", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                                textBoxTrabajoRealizado.Focus();
                            }
                            else if (Convert.ToInt32(textBoxFolioFactura.Text) == 0)
                            {
                                MessageBox.Show("El Folio de Factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                                textBoxFolioFactura.Focus();
                            }
                            else
                            {
                                if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM reportemantenimiento WHERE FolioFactura='" + textBoxFolioFactura.Text + "'")) > 0 && (textBoxFolioFactura.Enabled == true))
                                {
                                    MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBoxFolioFactura.Text = "";
                                    textBoxFolioFactura.Focus();
                                    validar();
                                }
                                else
                                {
                                    resultadopregunta = 0;
                                    metodobtnfinalizarcref();
                                    if (resultadopregunta == 1)
                                    {
                                        actualizarcbx();
                                        AutoCompletado(textBoxFolioB);
                                        comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonAgregar.Visible = label39.Visible = false;
                                        if (label35.Text == "EXPORTANDO")
                                            valexcel();
                                        else
                                            ocultarexcel();
                                        comboBoxEstatusMant.SelectedIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                    else if (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))
                    {
                        if (!string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text.Trim()))
                        {
                            resultadopregunta = 0;
                            metodobtnfinalizarcref();
                            if (resultadopregunta == 1)
                            {
                                comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonAgregar.Visible = label39.Visible = false;
                                if (label35.Text == "EXPORTANDO")
                                    valexcel();
                                else
                                    ocultarexcel();
                                cfin = comboBoxEstatusMant.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Introduzca un \"Trabajo Realizado\" Válido", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            validar();
                            textBoxTrabajoRealizado.Focus();
                        }
                    }
                }
                else if (comboBoxEstatusMant.SelectedIndex == 0)
                    MessageBox.Show("Seleccione un estatus del mantenimiento", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonGuardar_Click(object sender, EventArgs e) // Guardar
        {
            valdeval();
            if (comboBoxFalloGral.Text.Equals("-- GRUPO --"))
                MessageBox.Show("Seleccione un Grupo de Fallo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if ((textBoxMecanico.Text != "") && (labelNomMecanico.Text.Equals(".")))
            {
                MessageBox.Show("Contraseña del Mecánico Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMecanico.Text = "";
            }
            else if ((textBoxMecanicoApo.Text != "") && (labelNomMecanicoApo.Text.Equals("..")))
            {
                MessageBox.Show("Contraseña del Mecánico de Apoyo Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMecanicoApo.Text = "";
            }
            else if ((textBoxSuperviso.Text != "") && (labelNomSuperviso.Text.Equals("...")))
            {
                MessageBox.Show("Contraseña de la Persona que Supervisó el Mantenimiento Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxSuperviso.Text = "";
            }
            else if (labelNomMecanico.Text == ".")
                MessageBox.Show("Ingrese la Contraseña del Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if ((labelNomMecanico.Text == ".") && (labelNomMecanicoApo.Text != ".."))
                MessageBox.Show("El Mecánico de Apoyo no puede ser registrado antes de el Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if ((labelNomMecanico.Text == ".") && (labelNomSuperviso.Text != "..."))
                MessageBox.Show("El Supevisor no puede ser registrado antes de el Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (validacionfinalconteocolumnas)
                {
                    fincolumn = dgvPMantenimiento.Rows.Count;
                    validacionfinalconteocolumnas = false;
                }
                if ((fgeneralanterior.Equals(comboBoxFalloGral.Text)) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && ((comboBoxExisRefacc.SelectedIndex == 0) || (exisrefaccionanterior.Equals(comboBoxExisRefacc.Text))) && (reqrefanterior.Equals(comboBoxReqRefacc.Text)) && (trabrealizadoanterior.Equals(textBoxTrabajoRealizado.Text)) && (folfacturanterior.Equals(textBoxFolioFactura.Text)) && (estatusmantanterior.Equals(comboBoxEstatusMant.Text)) && (supervisoanterior.Equals(labelNomSuperviso.Text)) && (observacionesmantanterior.Equals(textBoxObsMan.Text)) && (inicolumn == fincolumn))
                {
                    conteo();
                    metodoCarga();
                    limpiarcampos();
                    limpiarstring();
                    ncontreffin();
                    inicolumn = 0;
                    fincolumn = 0;
                    dgvMantenimiento.Refresh();
                    comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonAgregar.Visible = label39.Visible = false;
                    if (label35.Text == "EXPORTANDO")
                        valexcel();
                    else
                        ocultarexcel();
                    timer1.Start();
                    MessageBox.Show("No se realizó ningun cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
                else if (comboBoxEstatusMant.SelectedIndex == 0)
                    MessageBox.Show("Seleccione un estatus del mantenimiento", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if ((comboBoxEstatusMant.Text == "EN PROCESO") || (comboBoxEstatusMant.Text == "REPROGRAMADA"))
                {
                    if ((comboBoxEstatusMant.Text.Equals("REPROGRAMADA")) && (estatusmantGV != "EN PROCESO"))
                        labelHoraInicioM.Text = textBoxEsperaMan.Text = "";
                    if (comboBoxReqRefacc.SelectedIndex == 1 && !metodotxtchref())
                    {
                        if (comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES"))
                        {
                            if ((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))))
                                metodobtnguardar();
                            else if ((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && ((comboBoxExisRefacc.SelectedIndex == 0)))
                                MessageBox.Show("El campo de 'Existencia De Refacciones' no debe quedar en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            else if ((textBoxFolioFactura.Text != "") && ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))) && (textBoxFolioFactura.Enabled))
                            {
                                MessageBox.Show("El Folio de Factura debe quedar en blanco si el apartado 'Existencia De Refacciones' está en espera de las refacciones o no hay existencias", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                textBoxFolioFactura.Text = "";
                            }
                            else if ((comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) && (!textBoxFolioFactura.Enabled)))
                            {
                                if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                                    MessageBox.Show("El Folio de Factura no puede quedar vacío si hay existencias en las refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else if (Convert.ToInt32(textBoxFolioFactura.Text) == 0)
                                {
                                    MessageBox.Show("El Folio de Factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBoxFolioFactura.Text = "";
                                    textBoxFolioFactura.Focus();
                                }
                                else
                                {
                                    MySqlCommand cmd01 = new MySqlCommand("SELECT coalesce((FolioFactura), '') AS FolioFactura FROM reportemantenimiento WHERE FolioFactura = '" + textBoxFolioFactura.Text + "'", v.c.dbconection());
                                    MySqlDataReader dr01 = cmd01.ExecuteReader();
                                    if (dr01.Read())
                                        foliofacturarconsulta = Convert.ToString(dr01.GetString("FolioFactura"));
                                    dr01.Close();
                                    v.c.dbcon.Close();
                                    if ((foliofacturarconsulta == textBoxFolioFactura.Text) && (textBoxFolioFactura.Enabled == true))
                                    {
                                        MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        textBoxFolioFactura.Text = "";
                                    }
                                    else if ((foliofacturarconsulta == textBoxFolioFactura.Text) && (textBoxFolioFactura.Enabled == false))
                                        metodobtnguardar();
                                    else
                                        metodobtnguardar();
                                }
                            }
                        }
                    }
                    else if (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))
                        metodobtnguardar();
                    else
                    {
                        MessageBox.Show("Seleccione una opcion en " + "'SE REQUIEREN REFACCIONES'", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        comboBoxReqRefacc.Enabled = true;
                    }
                }
                else if (!comboBoxEstatusMant.Enabled)
                    metodobtnguardar();
            }
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxTrabajoRealizado.Enabled && /*string.IsNullOrWhiteSpace(trabreak)*/ string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))
                    MessageBox.Show("No puede dejar en blanco el trabajo realizado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (comboBoxFalloGral.SelectedIndex == 0)
                    MessageBox.Show("No puede dejar en blanco el grupo de fallo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if ((folfacturanterior != "") && (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                    MessageBox.Show("No puede dejar en blanco el folio de factura", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (!string.IsNullOrWhiteSpace(textBoxFolioFactura.Text) && Convert.ToInt32(textBoxFolioFactura.Text) == 0)
                    MessageBox.Show("El folio de factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (!((fgeneralanterior.Equals(comboBoxFalloGral.Text)) && (trabrealizadoanterior.Equals(textBoxTrabajoRealizado.Text)) && (folfacturanterior.Equals(textBoxFolioFactura.Text)) && (observacionesmantanterior.Equals(textBoxObsMan.Text))))
                {
                    if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text) || Convert.ToInt32(textBoxFolioFactura.Text) >= 1)
                    {
                        MySqlCommand cmd01 = new MySqlCommand("SELECT coalesce((FolioFactura), '') AS FolioFactura FROM reportemantenimiento WHERE FolioFactura = '" + textBoxFolioFactura.Text + "'", v.c.dbconection());
                        MySqlDataReader dr01 = cmd01.ExecuteReader();
                        if (dr01.Read())
                            foliofacturarconsulta = Convert.ToString(dr01.GetString("FolioFactura"));
                        dr01.Close();
                        v.c.dbcon.Close();
                        if (((foliofacturarconsulta == textBoxFolioFactura.Text) && (folfacturanterior != foliofacturarconsulta)) && (textBoxFolioFactura.Enabled))
                        {
                            MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxFolioFactura.Text = "";
                        }
                        else
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            if (obs.ShowDialog() == DialogResult.OK)
                            {
                                string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                                cmd.ExecuteNonQuery();
                                v.c.dbcon.Close();

                                MySqlCommand cmd0 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion,empresa, area) VALUES('Reporte de Mantenimiento', (SELECT IdReporte FROM reportemantenimiento WHERE FoliofkSupervicion = '" + idreportesupervision + "'), CONCAT('" + fgeneralanterior + ";', '" + folfacturanterior + ";', '" + trabrealizadoanterior + ";', '" + observacionesmantanterior + "'), '" + idUsuario + "', now(), 'Actualización de Reporte de Mantenimiento','" + observaciones + "', '2', '1')", v.c.dbconection());
                                cmd0.ExecuteNonQuery();
                                v.c.dbcon.Close();
                                ClasFallo();
                                MessageBox.Show("Reporte Editado Correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Cancelar(false);
                                bloquea();
                            }
                        }
                        if (label35.Text == "EXPORTANDO")
                            valexcel();
                        else
                            ocultarexcel();
                    }
                }
                else
                {
                    actualizarcbx();
                    AutoCompletado(textBoxFolioB);
                    MessageBox.Show("Sin Modificaciones", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bloquea();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void bloquea()
        {
            metodoCarga();
            limpiarcampos();
            limpiarstring();
            cantidadrefacciones = inicolumn = fincolumn = 0;
            ncontreffin();
            conteo();
            dgvMantenimiento.Refresh();
            buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonEditar.Visible = label58.Visible = comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonAgregar.Visible = label39.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
            timer1.Start();
            comboBoxEstatusMant.SelectedIndex = 0;
        }

        private void dataGridViewMantenimiento_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //Doble Click En GridView De Mantenimiento
        {
            mensaje = false;
            if (e.RowIndex >= 0)
            {
                valdeval();
                if (((fgeneralanterior.Equals(validacionfgeneral) || ((valorfallogeneral == 1) || (string.IsNullOrWhiteSpace(fgeneralanterior)))) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && (exisrefaccionanterior.Equals(validacionexisrefacc)) && (reqrefanterior.Equals(validacionreqrefacc)) && (trabrealizadoanterior.Trim().Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Trim().Equals(textBoxFolioFactura.Text.Trim())) && ((estatusmantanterior.Equals(validacionestatusmant)) || (comboBoxEstatusMant.Text.Equals("EN PROCESO"))) && (supervisoanterior.Trim().Equals(labelNomSuperviso.Text.Trim())) && (observacionesmantanterior.Trim().Equals(textBoxObsMan.Text.Trim())) && ((inicolumn == 0) || (inicolumn == fincolumn))))
                {
                    actualizarcbx();
                    AutoCompletado(textBoxFolioB);
                    limpiarstring();
                    inicolumn = fincolumn = 0;
                    registroconteofilaspedref = false;
                    llamadadatos();
                    ncontrefini();
                }
                else
                {
                    int total;
                    total = fincolumn - inicolumn;
                    if (MessageBox.Show("Si usted cambia de reporte y/o actualiza la tabla se perderan los datos ingresados\n\n ¿Esta seguro de querer continuar?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "", v.c.dbconection());
                        cmd1.ExecuteNonQuery();
                        v.c.dbcon.Close();
                        actualizarcbx();
                        AutoCompletado(textBoxFolioB);
                        inicolumn = fincolumn = 0;
                        registroconteofilaspedref = false;
                        limpiarstring();
                        metodocargaref();
                        llamadadatos();
                    }
                }
            }
        }

        private void dataGridViewMRefaccion_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //Doble Click En GridView De Refacciones
        {
            if (e.RowIndex >= 0)
            {
                if ((registroentradapedref) && (!string.IsNullOrWhiteSpace(fgeneralanterior)))
                {
                    existenciaGV = dgvPMantenimiento.CurrentRow.Cells[7].Value.ToString();
                    if ((existenciaGV == "SIN EXISTENCIA") || (existenciaGV == "EXISTENCIA") || (existenciaGV == "INCOMPLETO"))
                        MessageBox.Show("La Refacción ya fue validada por almacen, esta refacción ya no se puede editar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        banderaeditar = buttonActualizar.Visible = label26.Visible = buttonAgregaPed.Visible = label33.Visible = false;
                        buttonAgregarMasPed.Visible = label29.Visible = true;
                        conta = dgvPMantenimiento.CurrentRow.Cells["PARTIDA"].Value.ToString();
                        string sql = "select upper(t1.descripcionfamilia) as familia, t1.idfamilia from cfamilias as t1 INNER JOIN cmarcas as t3 ON t3.descripcionfkcfamilias = t1.idfamilia inner join crefacciones as t2 on t2.marcafkcmarcas = t3.idmarca where upper(t2.nombreRefaccion) = '" + dgvPMantenimiento.CurrentRow.Cells[1].Value + "'and t1.status = '0'";
                        MySqlCommand familia = new MySqlCommand(sql, v.c.dbconection());
                        MySqlDataReader dtr = familia.ExecuteReader();
                        if (dtr.Read())
                        {
                            comboBoxFamilia.DataSource = null;
                            DataTable dt = new DataTable();
                            MySqlCommand cmd2 = new MySqlCommand("SELECT UPPER(familia) AS familia, idfamilia FROM cfamilias WHERE status = '1' ORDER BY familia", v.c.dbconection());
                            MySqlDataAdapter adap = new MySqlDataAdapter(cmd2);
                            adap.Fill(dt);
                            DataRow row2 = dt.NewRow();
                            DataRow row3 = dt.NewRow();
                            row2["idfamilia"] = 0;
                            row2["familia"] = " -- FAMILIA --";
                            row3["idfamilia"] = dtr["idfamilia"];
                            row3["familia"] = dtr["familia"].ToString();
                            dt.Rows.InsertAt(row2, 0);
                            dt.Rows.InsertAt(row3, 1);
                            comboBoxFamilia.ValueMember = "idfamilia";
                            comboBoxFamilia.DisplayMember = "familia";
                            comboBoxFamilia.DataSource = dt;
                            comboBoxFamilia.SelectedIndex = 0;
                            comboBoxFamilia.Text = dtr["familia"].ToString();

                        }
                        string sql2 = " SELECT t1.idPedRef, t5.idcnfamilia, UPPER(t5.familia) AS Familia, t1.Cantidad, UPPER(t2.nombreRefaccion) AS Refaccion, t1.RefaccionfkCRefaccion AS idRefaccion FROM pedidosrefaccion AS t1 INNER JOIN crefacciones AS t2 ON t1.RefaccionfkCRefaccion = t2.idrefaccion INNER JOIN cmarcas as t4 ON t2.marcafkcmarcas = t4.idmarca INNER JOIN cfamilias AS t3 ON t4.descripcionfkcfamilias = t3.idfamilia INNER JOIN cnfamilias as t5 ON t3.familiafkcnfamilias = t5.idcnfamilia WHERE t1.NumRefacc =  '" + conta + "' AND t1.FolioPedfkSupervicion ='" + idreportesupervision + "'";
                        MySqlCommand cmd = new MySqlCommand(sql2, v.c.dbconection());
                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            foliof = Convert.ToString(dr.GetString("idPedRef"));
                            comboBoxFamilia.Text = Convert.ToString(dr.GetString("Familia"));
                            familianterior = Convert.ToString(dr.GetString("Familia"));
                            idfamilianterior = Convert.ToInt32(dr.GetString("idcnFamilia"));
                            comboBoxFRefaccion.Text = Convert.ToString(dr.GetString("Refaccion"));
                            refaccionanterior = comboBoxFRefaccion.Text;
                            idrefaccionanterior = Convert.ToInt32(dr.GetString("idRefaccion"));
                            textBoxCantidad.Text = Convert.ToString(dr.GetString("Cantidad"));
                            cantidadanterior = Convert.ToDouble(dr.GetString("Cantidad"));
                        }
                        comboBoxFRefaccion.Text = dgvPMantenimiento.CurrentRow.Cells[1].Value.ToString();
                        dr.Close();
                        v.c.dbcon.Close();
                        banderaeditar = true;
                    }
                }
                else
                    MessageBox.Show("Para editar una refacción necesita guardar el reporte por 1ra vez", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonActualizarPed_Click(object sender, EventArgs e) //Actualizar Pedido
        {
            if ((comboBoxFamilia.SelectedIndex == 0) && (comboBoxFRefaccion.SelectedIndex == 0) && (string.IsNullOrWhiteSpace(textBoxCantidad.Text)))
                MessageBox.Show("Alguno de Los campos estan vacíos", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFamilia.SelectedIndex == 0)
                MessageBox.Show("El campo de Familia no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFRefaccion.SelectedIndex == 0)
                MessageBox.Show("El campo de Refacción no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
                MessageBox.Show("El campo de cantidad debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
                MessageBox.Show("El campo debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToDouble(textBoxCantidad.Text) <= 0)
                MessageBox.Show("La cantidad debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((comboBoxFamilia.Text.Equals(familianterior)) && (comboBoxFRefaccion.Text.Equals(refaccionanterior)) && (textBoxCantidad.Text.Equals(cantidadanterior)))
                    MessageBox.Show("No se realizó ningún cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        MySqlCommand cmd = new MySqlCommand("UPDATE pedidosrefaccion SET RefaccionfkCRefaccion = '" + comboBoxFRefaccion.SelectedValue + "', Cantidad = '" + Convert.ToDouble(textBoxCantidad.Text) + "' WHERE NumRefacc = '" + conta + "'", v.c.dbconection());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Refacción actualizada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        v.c.dbcon.Close();
                        MySqlCommand cmd0 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion,empresa, area) VALUES('Reporte de Mantenimiento', COALESCE((SELECT IdReporte FROM reportemantenimiento WHERE FoliofkSupervicion = '" + idreportesupervision + "'), 0), CONCAT('" + idrefaccionanterior + ";', '" + cantidadanterior + "'), '" + idUsuario + "', now(), 'Actualización de Refacción en Reporte de Mantenimiento','" + observaciones + "', '2', '1')", v.c.dbconection());
                        cmd0.ExecuteNonQuery();
                        v.c.dbcon.Close();
                        banderaeditar = false;
                    }
                }
                metodocargaref();
                limpiarrefacc();
                buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = false;
                buttonAgregaPed.Visible = label33.Visible = buttonActualizar.Visible = label26.Visible = true;
            }
        }

        private void buttonActualizar_Click(object sender, EventArgs e)  //Actualizar
        {
            botonactualizar();
        }

        private void buttonPDF_Click(object sender, EventArgs e) //Generar PDF
        {
            string comporacion = "";
            if ((radioButtonGeneral.Checked) && (!radioButtonUnidad.Checked))
                comporacion = "el reporte de fallo del mantenimiento en PDF?";
            else
                comporacion = "los datos de la unidad del mantenimiento en PDF?";
            if ((!radioButtonGeneral.Checked) && (!radioButtonUnidad.Checked))
                MessageBox.Show("Favor de seleccionar entre el Reporte de Fallo y Los Datos de la Unidad", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((MessageBox.Show("¿Desea generar " + comporacion, "INFORMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
                {
                    if (dgvPMantenimiento.Rows.Count == 0)
                        validaciontablarefacciones = true;
                    else
                        validaciontablarefacciones = false;
                    if (radioButtonGeneral.Checked)
                    {
                        dgvPMantenimiento.Visible = true;
                        metodocargarefpdf();
                        v.c.dbcon.Close();
                        topdfgeneral();
                        metodocargaref();
                    }
                    else if (radioButtonUnidad.Checked)
                    {
                        dgvPMantenimiento.Visible = true;
                        metodocargarefpdf();
                        v.c.dbcon.Close();
                        topdfconunidades();
                        metodocargaref();
                    }
                }
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e) //Buscar
        {
            if ((string.IsNullOrWhiteSpace(textBoxFolioB.Text)) && (comboBoxUnidadB.Text.Equals("-- ECONÓMICO --")) && (comboBoxMecanicoB.Text.Equals("-- MECÁNICO --")) && (comboBoxEstatusMB.Text.Equals("-- ESTATUS --")) && (!checkBoxFechas.Checked) && (comboBoxMesB.Text.Equals("-- MES --")) && (comboBoxDescpFalloB.Text.Equals("--SELECCIONE DESCRIPCIÓN--")))
                MessageBox.Show("Seleccione Un Criterio De Búqueda", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (dateTimePickerIni.Value > dateTimePickerFin.Value)
                {
                    MessageBox.Show("La fecha inicial no debe superar a la fecha final", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    limpiarcamposbus();
                }
                else if ((dateTimePickerIni.Value >= DateTime.Now) || (dateTimePickerFin.Value >= DateTime.Now))
                {
                    MessageBox.Show("Las fechas no deben superar el día de hoy", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    limpiarcamposbus();
                    checkBoxFechas.Checked = false;
                }
                else
                {
                    String Fini = "";
                    String Ffin = "";
                    string consulta = "SET lc_time_names = 'es_ES'; SELECT t1.idReporteSupervicion AS 'ID', t1.Folio AS 'FOLIO', CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECONÓMICO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'FECHA DEL REPORTE', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'ESTATUS DEL MANTENIMIENTO',  coalesce((SELECT UPPER(CONCAT(r22.codfallo, ' - ', r22.falloesp)) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'CODIGO DE FALLO', coalesce((SELECT UPPER(DATE_FORMAT(r24.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'FECHA DEL REPORTE DE MANTENIMIENTO', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'MECANICO', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'MECANICO DE APOYO',  coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, UPPER(t1.HoraEntrada) AS 'HORA DE ENTRADA', UPPER(t1.TipoFallo) AS 'TIPO DE FALLO', UPPER(t1.KmEntrada) AS 'KILOMETRAJE', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'SUBGRUPO DE FALLO', UPPER(t1.DescFalloNoCod) AS 'SUBGRUPO DE FALLO NO CODIFICADO', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'OBSERVACIONES DE SUPERVISION', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'GRUPO DE FALLO', coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'TRABAJO REALIZADO', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'HORA DE INICIO DE MANTENIMIENTO', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'HORA DE TERMINO DE MANTENIMIENTO', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'ESPERA DE TIEMPO PARA MANTENIMIENTO', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'DIFERENCIA DE TIEMPO EN MANTENIMIENTO', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'FOLIO DE FACTURA', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'SUPERVISO', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'EXISTENCIA DE REFACCIONES EN ALMACEN', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'ESTATUS DE REFACCIONES', coalesce((SELECT UPPER(CONCAT(r23.ApPaterno, ' ', r23.ApMaterno, ' ', r23.nombres)) FROM reportemantenimiento AS r24 INNER JOIN cpersonal AS r23 ON r24.PersonaFinal = r23.idPersona WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'PERSONA QUE FINALIZO EL MANTENIMIENTO', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'OBSERVACIONES DEL MANTENIMIENTO' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN careas AS t4 ON t2.areafkcareas = t4.idarea ";
                    string WHERE = "";
                    if (!string.IsNullOrWhiteSpace(textBoxFolioB.Text))
                        if (WHERE == "")
                            WHERE = " WHERE t1.Folio = '" + textBoxFolioB.Text + "'";
                        else
                            WHERE += " AND t1.Folio = '" + textBoxFolioB.Text + "'";
                    if (comboBoxUnidadB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) = '" + comboBoxUnidadB.Text + "'";
                        else
                            WHERE += " AND CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) = '" + comboBoxUnidadB.Text + "'";
                    if (comboBoxMecanicoB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion) = '" + comboBoxMecanicoB.Text + "'";
                        else
                            WHERE += " AND (SELECT CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion) = '" + comboBoxMecanicoB.Text + "'";
                    if (comboBoxEstatusMB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT r19.Estatus FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion) = '" + comboBoxEstatusMB.Text + "'";
                        else
                            WHERE += " AND (SELECT r19.Estatus FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion) = '" + comboBoxEstatusMB.Text + "'";
                    if (checkBoxFechas.Checked == true)
                        Fini = dateTimePickerIni.Value.ToString("yyyy-MM-dd");
                    Ffin = dateTimePickerFin.Value.ToString("yyyy-MM-dd");
                    if (WHERE == "")
                        WHERE = " WHERE (SELECT t1.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    else
                        WHERE += " AND (SELECT t1.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    if (comboBoxMesB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT t1.FechaReporte WHERE MONTH(t1.FechaReporte) = '" + month + "' AND YEAR(t1.FechaReporte) = YEAR(Now()))";
                        else
                            WHERE += " AND (SELECT t1.FechaReporte WHERE MONTH(t1.FechaReporte) = '" + month + "' AND YEAR(t1.FechaReporte) = YEAR(Now()))";
                    if (comboBoxDescpFalloB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT r21.descfallo FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo) = '" + comboBoxDescpFalloB.Text + "'";
                        else
                            WHERE += " AND (SELECT r21.descfallo FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo) = '" + comboBoxDescpFalloB.Text + "'";
                    if (WHERE != "")
                        WHERE += " AND  (SELECT empresaMantenimiento FROM cmodelos WHERE idmodelo = t2.modelofkcmodelos) ='" + empresa + "' ORDER BY t1.Folio DESC";
                    MySqlDataAdapter adp = new MySqlDataAdapter(consulta + WHERE, v.c.dbconection());
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    dgvMantenimiento.DataSource = ds.Tables[0];
                    dgvMantenimiento.Columns[0].Frozen = dgvMantenimiento.Columns[1].Frozen = dgvMantenimiento.Columns[2].Frozen = true;
                    dgvMantenimiento.Columns[0].Visible = dgvMantenimiento.Columns[1].Visible = false;
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron reportes", "ADVERTEMCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        limpiarcamposbus();
                        metodoCarga();
                        conteo();
                        buttonExcel.Visible = label35.Visible = false;
                    }
                    else
                    {
                        conteovariable();
                        limpiarcamposbus();
                        if (!activado)
                            buttonExcel.Visible = true;
                        label35.Visible = true;
                    }
                    v.c.dbcon.Close();
                    checkBoxFechas.Checked = false;
                }
            }
        }

        private void comboBoxFamilia_SelectedIndexChanged(object sender, EventArgs e) // UNIR LOS 2 INDEX CHANGUED
        {
            if (banderaeditar)
            {
                if ((idfamilianterior.Equals(comboBoxFamilia.SelectedValue) || comboBoxFamilia.SelectedIndex == 0) && (idrefaccionanterior.Equals(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior.Equals(Convert.ToDouble(textBoxCantidad.Text)) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
            }
        }

        private void comboBoxFRefaccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                if ((idfamilianterior.Equals(comboBoxFamilia.SelectedValue) || comboBoxFamilia.SelectedIndex == 0) && (idrefaccionanterior.Equals(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior.Equals(Convert.ToDouble(textBoxCantidad.Text)) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
            }
        }

        private void textBoxTrabajoRealizado_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text.Contains(Environment.NewLine))
                txt.Text.Replace(Environment.NewLine, "");
        }

        private void labelCerrarHerr_Click(object sender, EventArgs e) //Acciones Del Label Cerrar De Refacciones
        {
            banderaeditar = groupBoxRefacciones.Visible = label1.Visible = false;
            gbxMantenimiento.Visible = buttonGuardar.Visible = label24.Visible = buttonActualizar.Visible = label26.Visible = true;
            limpiarrefacc();
            if (dgvPMantenimiento.Rows.Count == 0)
                comboBoxReqRefacc.SelectedIndex = 2;
            else
                comboBoxReqRefacc.SelectedIndex = 1;
            if (fgeneralanterior == "" && mecanicoanterior == ".")
                Cancelar(false);
            else
                Cancelar(true);
            comboBoxExisRefacc.Enabled = false;
        }

        private void buttonAgregaPed_Click(object sender, EventArgs e) //Agrega Una Nueva Refaccion
        {
            double pedcantidad = 0;
            if (textBoxCantidad.Text == "")
                textBoxCantidad.Text = "0";
            try
            {
                pedcantidad = Convert.ToDouble(textBoxCantidad.Text);
            }
            catch
            { }
            if ((comboBoxFamilia.SelectedIndex == 0) && (comboBoxFRefaccion.SelectedIndex == 0) && (string.IsNullOrWhiteSpace(textBoxCantidad.Text)))
                MessageBox.Show("Los campos están vacios", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFamilia.SelectedIndex == 0)
                MessageBox.Show("El campo de Familia no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFRefaccion.SelectedIndex == 0)
                MessageBox.Show("El campo de Refacción no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
                MessageBox.Show("El campo de cantidad debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (pedcantidad == 0)
                MessageBox.Show("La cantidad debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (pedcantidad == 0)
                    MessageBox.Show("La cantidad debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MySqlCommand cmd0 = new MySqlCommand("SELECT NumRefacc FROM pedidosrefaccion WHERE FolioPedfkSupervicion= '" + idreportesupervision + "' ORDER BY idPedRef DESC ", v.c.dbconection());
                MySqlDataReader dr0 = cmd0.ExecuteReader();
                if (dr0.Read())
                {
                    enumeradorefacciones = Convert.ToInt32(dr0.GetString("NumRefacc"));
                    enumeradorefacciones = enumeradorefacciones + 1;
                }
                else
                {
                    enumeradorefacciones = 0;
                    enumeradorefacciones = enumeradorefacciones + 1;
                }
                dr0.Close();
                v.c.dbcon.Close();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO pedidosrefaccion(NumRefacc, FolioPedfkSupervicion, RefaccionfkCRefaccion, FechaPedido, HoraPedido, Cantidad,usuariofkcpersonal) VALUES ('" + enumeradorefacciones + "', '" + idreportesupervision + "', '" + comboBoxFRefaccion.SelectedValue + "', curdate(), Time(Now()),'" + textBoxCantidad.Text + "','" + idmecanicoanterior + "')", v.c.dbconection());
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                adp.Fill(dt);
                dgvPMantenimiento.DataSource = dt;
                v.c.dbcon.Close();
                ncontreffin();
                conteofinref();
                metodocargaref();
                MySqlCommand sql = new MySqlCommand("UPDATE reportemantenimiento SET seenalmacen = 0 WHERE FoliofkSupervicion = '" + idreportesupervision + "' ", v.c.dbconection());
                sql.ExecuteNonQuery();
                MessageBox.Show("Refacción agregada correctamente", "COMPLETADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cantidadrefacciones = cantidadrefacciones + 1;
                limpiarrefacc();
                comboBoxExisRefacc.SelectedIndex = 2;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                valdeval();
                if (((fgeneralanterior.Equals(validacionfgeneral) || ((valorfallogeneral == 1) || (string.IsNullOrWhiteSpace(fgeneralanterior)))) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && (exisrefaccionanterior.Equals(validacionexisrefacc)) && (reqrefanterior.Equals(validacionreqrefacc)) && (trabrealizadoanterior.Trim().Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Trim().Equals(textBoxFolioFactura.Text.Trim())) && ((estatusmantanterior.Equals(validacionestatusmant)) || (comboBoxEstatusMant.Text.Equals("EN PROCESO"))) && (supervisoanterior.Trim().Equals(labelNomSuperviso.Text.Trim())) && (observacionesmantanterior.Trim().Equals(textBoxObsMan.Text.Trim())) && ((inicolumn == 0) || (inicolumn == fincolumn))))
                    limpiar();
                else
                {
                    if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        buttonEditar_Click(null, e);
                    limpiar();
                }
            }
            else
            {
                valdeval();
                string exisref = "";
                if (comboBoxExisRefacc.SelectedIndex > 0)
                    exisref = comboBoxExisRefacc.Text;
                string requiereRefac = "";
                if (comboBoxReqRefacc.SelectedIndex > 0)
                    requiereRefac = comboBoxReqRefacc.Text;
                if ((fgeneralanterior.Equals(comboBoxFalloGral.Text)) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && ((comboBoxExisRefacc.SelectedIndex == 0) || (exisrefaccionanterior.Equals(exisref))) && (reqrefanterior.Equals(requiereRefac)) && (trabrealizadoanterior.Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Equals(textBoxFolioFactura.Text)) && (estatusmantanterior.Equals(comboBoxEstatusMant.Text)) && (supervisoanterior.Equals(labelNomSuperviso.Text)) && (observacionesmantanterior.Equals(textBoxObsMan.Text)) && (inicolumn == fincolumn))
                    limpiar();
                else
                {
                    if (!string.IsNullOrWhiteSpace(labelFolio.Text.Trim()))
                    {
                        if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            buttonEditar_Click(null, e);
                        else
                        {
                            int total;
                            if (registroconteofilaspedref)
                            {
                                total = dgvPMantenimiento.Rows.Count - inicolumn;
                                MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "", v.c.dbconection());
                                cmd1.ExecuteNonQuery();
                                v.c.dbcon.Close();
                                botonactualizar();
                            }
                        }
                    }
                    limpiar();
                }
            }
        }

        void limpiar()
        {
            actualizarcbx();
            AutoCompletado(textBoxFolioB);
            limpiarstring();
            inicolumn = 0;
            fincolumn = 0;
            registroconteofilaspedref = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = false;
            metodocargaref();
            conteoiniref();
            conteofinref();
            limpiarrefacc();
            limpiarcampos();
            limpiarstring();
            limpiarcampos();
            limpiarcamposbus();
            enabledfalse();
            ocultarexcel();
            metodoCarga();
            comboBoxEstatusMant.SelectedIndex = 0;
            Cancelar(false);
        }

        private void textBoxFolioB_Click(object sender, EventArgs e)
        {
            textBoxFolioB.SelectAll();
        }

        /* Validaciones de los campos de contraseña*/
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxLargo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }

        private void textBoxContras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsNumber(e.KeyChar)) || (Char.IsLetter(e.KeyChar) || (e.KeyChar == 8) || (e.KeyChar == 127)))
                e.Handled = false;
            else if (e.KeyChar == 32)
            {
                e.Handled = true;
                MessageBox.Show("Solo puede ingresar números y letras en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo puede ingresar números y letras en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxFolioFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127 || e.KeyChar == 08)
                e.Handled = false;
            else if (Char.IsNumber(e.KeyChar))
                e.Handled = false;
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan números en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Solo se pueden introducir números y un solo punto decimal", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ya existe un punto decimal en la caja de texto", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            valida_refacciones();
        }

        /* Validaciones de las contraseñas */
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxSuperviso_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(textBoxSuperviso.Text) + "'AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidSuperviso.Text = dr["idPersona"].ToString();
                labelNomSuperviso.Text = dr["Nombre"] as string;
                if ((labelNomSuperviso.Text == labelNomMecanico.Text) || (labelNomSuperviso.Text == labelNomMecanicoApo.Text))
                {
                    MessageBox.Show("El Supervisor no debe ser igual al Mecánico y/o Mecánico de Apoyo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomSuperviso.Text = "...";
                    textBoxSuperviso.Text = "";
                }
            }
            else if (textBoxSuperviso.Text != "")
            {
                labelidSuperviso.Text = "";
                labelNomSuperviso.Text = "...";
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        private void textBoxMecanicoApo_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, coalesce(UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)), '') AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(textBoxMecanicoApo.Text) + "'AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidMecanicoApo.Text = dr["idPersona"].ToString();
                labelNomMecanicoApo.Text = dr["Nombre"] as string;
                if ((labelNomMecanicoApo.Text == labelNomMecanico.Text) || (labelNomMecanicoApo.Text == labelNomSuperviso.Text))
                {
                    MessageBox.Show("El Mecánico de Apoyo no debe ser igual al Mecánico y/o al Supervisor", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomMecanicoApo.Text = "..";
                    textBoxMecanicoApo.Text = "";
                }
            }
            else if (textBoxMecanicoApo.Text != "")
            {
                labelidMecanicoApo.Text = "";
                labelNomMecanicoApo.Text = "..";
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        private void textBoxMecanico_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, coalesce(UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)),'') AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(textBoxMecanico.Text) + "' AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidMecanico.Text = dr["idPersona"].ToString();
                labelNomMecanico.Text = dr["Nombre"] as string;
                if ((labelNomMecanico.Text == labelNomMecanicoApo.Text) || (labelNomMecanico.Text == labelNomSuperviso.Text))
                {
                    MessageBox.Show("El Mecánico no debe ser igual al Supervisor y/o al Mecánico de Apoyo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomMecanico.Text = ".";
                    textBoxMecanico.Text = "";
                }
                else if (comboBoxFalloGral.SelectedIndex > 0 && banderaeditar == false)
                    comboBoxReqRefacc.Enabled = true;
                else
                    comboBoxReqRefacc.Enabled = false;
            }
            else if (textBoxMecanico.Text != "")
            {
                labelidMecanico.Text = "";
                labelNomMecanico.Text = ".";
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        /*Validaciones extras */
        private void comboBoxFamilia_SelectedValueChanged(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                if ((idfamilianterior.Equals(comboBoxFamilia.SelectedValue) || comboBoxFamilia.SelectedIndex == 0) && (idrefaccionanterior.Equals(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior.Equals(Convert.ToDouble(textBoxCantidad.Text)) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
                if (comboBoxFamilia.SelectedIndex > 0)
                {
                    v.iniCombos("SELECT UPPER(t1.nombreRefaccion) AS 'nombreRefaccion', t1.idrefaccion FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas=t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias=t3.idfamilia INNER JOIN cnfamilias as t4 ON t3.familiafkcnfamilias=t4.idcnfamilia WHERE t4.idcnfamilia='" + comboBoxFamilia.SelectedValue + "' AND t1.status=1;", comboBoxFRefaccion, "idrefaccion", "nombreRefaccion", "-- REFACCION --");
                    comboBoxFRefaccion.Enabled = true;
                }
                else
                {
                    comboBoxFRefaccion.Enabled = false;
                    comboBoxFRefaccion.DataSource = null;
                }
            }
            else
            {
                if (comboBoxFamilia.SelectedIndex > 0)
                {
                    v.iniCombos("SELECT UPPER(t1.nombreRefaccion) AS 'nombreRefaccion', t1.idrefaccion FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas=t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias=t3.idfamilia INNER JOIN cnfamilias as t4 ON t3.familiafkcnfamilias=t4.idcnfamilia WHERE t4.idcnfamilia='" + comboBoxFamilia.SelectedValue + "' AND t1.status=1;", comboBoxFRefaccion, "idrefaccion", "nombreRefaccion", "-- REFACCION --");
                    comboBoxFRefaccion.Enabled = true;
                }
                else
                {
                    comboBoxFRefaccion.Enabled = false;
                    comboBoxFRefaccion.DataSource = null;
                }
            }
        }

        private void comboBoxFRefaccion_SelectedValueChanged(object sender, EventArgs e)
        {
            MySqlCommand cmd0 = new MySqlCommand("select t1.simbolo as 'Unidad De Medida',t1.idunidadmedida,t4.nombreRefaccion from cunidadmedida as t1 inner join cfamilias as t2 on t2.umfkcunidadmedida=t1.idunidadmedida inner join cmarcas as t3 on t3.descripcionfkcfamilias=t2.idfamilia inner join crefacciones as t4 on t4.marcafkcmarcas=t3.idmarca inner join cnfamilias as t5 on t5.idcnFamilia=t2.familiafkcnfamilias WHERE t5.idcnFamilia = '" + comboBoxFamilia.SelectedValue + "' AND t4.idrefaccion = '" + comboBoxFRefaccion.SelectedValue + "'", v.c.dbconection());
            MySqlDataReader dr = cmd0.ExecuteReader();
            if (dr.Read())
            {
                nombrerefaccionconsulta = Convert.ToString(dr.GetString("nombreRefaccion"));
                unidadmedidaconsulta = Convert.ToString(dr.GetString("Unidad De Medida"));
                idunidadmedidaconsulta = Convert.ToInt32(dr.GetString("idunidadmedida"));
            }
            dr.Close();
            v.c.dbcon.Close();
            textBoxUM.Text = unidadmedidaconsulta;
        }

        private void comboBoxReqRefacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxRefacciones.Visible = false;
            if ((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES")) && (comboBoxReqRefacc.Enabled == true))
            {
                metodocargaref();
                //registroentradapedref = false;
                if (registroconteofilaspedref == false)
                {
                    inicolumn = 0;
                    inicolumn = dgvPMantenimiento.Rows.Count;
                    validacionfinalconteocolumnas = true;
                    //registroconteofilaspedref = true;
                }
                buttonGuardar.Visible = label24.Visible = buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = gbxMantenimiento.Visible = false;
                buttonAgregaPed.Visible = label33.Visible = groupBoxRefacciones.Visible = label1.Visible = true;
                if (!registroentradapedref) // VERIFICAR ESTA VALIDACIÓN
                    registroentradapedref = label62.Visible = label63.Visible = true;
                dgvPMantenimiento.Visible = true;
                conteoiniref();
                conteofinref();
                if (comboBoxReqRefacc.Text == "SE REQUIEREN REFACCIONES")
                    comboBoxExisRefacc.Enabled = true;
                Cancelar(false);
            }
            else if (((comboBoxReqRefacc.Text == "NO SE REQUIEREN REFACCIONES")) && (!buttonEditar.Visible))
            {
                metodocargaref();
                conteoiniref();
                if (labelrefini.Text != "0")
                {
                    MessageBox.Show("Ya existen refacciones en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxReqRefacc.SelectedIndex = 1;
                    groupBoxRefacciones.Visible = label1.Visible = false;
                    gbxMantenimiento.Visible = buttonGuardar.Visible = textBoxSuperviso.Enabled = true;
                    if (fgeneralanterior != "")
                        Cancelar(true);
                }
                else
                {
                    buttonAgregar.Visible = label39.Visible = comboBoxExisRefacc.Enabled = textBoxFolioFactura.Enabled = false;
                    comboBoxExisRefacc.SelectedIndex = 0;
                    textBoxFolioFactura.Text = "";
                    if (fgeneralanterior != "")
                        Cancelar(true);
                }
            }
            else if ((comboBoxReqRefacc.SelectedIndex == 0) && (buttonAgregar.Enabled) && !(comboBoxFalloGral.SelectedIndex == 0))
            {
                metodocargaref();
                conteoiniref();
                if (labelrefini.Text != "0")
                {
                    MessageBox.Show("Ya existen refacciones en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxReqRefacc.SelectedIndex = 1;
                    groupBoxRefacciones.Visible = label1.Visible = false;
                    gbxMantenimiento.Visible = buttonGuardar.Visible = label24.Visible = true;
                }
                else
                {
                    buttonAgregar.Visible = label39.Visible = comboBoxExisRefacc.Enabled = textBoxFolioFactura.Enabled = false;
                    comboBoxExisRefacc.SelectedIndex = 0;
                    textBoxFolioFactura.Text = "";
                }
            }
        }

        private void comboBoxExisRefacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((existencias == false) && (comboBoxFalloGral.Text != "") && (buttonActualizar.Visible) && (!buttonGuardar.Visible))
            { }
            else if ((existencias == false) && (comboBoxFalloGral.Text != "") && (buttonActualizar.Visible) && (buttonGuardar.Visible) && ((comboBoxExisRefacc.Enabled) || ((!comboBoxExisRefacc.Enabled) && (comboBoxFalloGral.SelectedIndex == 0))))
            {
                if (((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) || ((!textBoxFolioFactura.Enabled) || (textBoxFolioFactura.Enabled)) && (textBoxFolioFactura.Text != "")))
                {
                    if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")))
                    {
                        if (!textBoxFolioFactura.Enabled == false)
                        {
                            textBoxFolioFactura.Enabled = false;
                            textBoxFolioFactura.Text = "";
                        }
                    }
                    if (!(comboBoxFalloGral.SelectedIndex == 0))
                    {
                        valida_refacciones();
                        if (totalrefacciones == totalexistenciarefaccinoes)
                        {
                            validacionconteo = validacionconteo + 3;
                            if (totalrefacciones == validacionexistenciarefacciones)
                            {
                                validacionconteo = validacionconteo + 2;
                                if (totalfaltante == 0)
                                {
                                    validacionconteo = validacionconteo + 1;
                                    if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                                    {
                                        textBoxFolioFactura.Enabled = true;
                                    }
                                }
                                else
                                {
                                    validacionconteo = validacionconteo + 2;
                                }
                            }
                            else
                            {
                                validacionconteo = validacionconteo + 1;
                            }
                        }
                        else
                        {
                            validacionconteo = validacionconteo + 1;
                        }
                        if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (!textBoxFolioFactura.Enabled) && (validacionconteo == 1))
                        {
                            if (!groupBoxRefacciones.Visible == true)
                            {
                                if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES") && !mensaje)
                                    MessageBox.Show("No todas las refacciones solicitadas estan validadas\n Espere hasta que las validen", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else if (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))
                                { }
                                else if (comboBoxExisRefacc.SelectedIndex == 0)
                                    MessageBox.Show("Seleccione otra opción valida", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            conteorefaccionesverificadas = 2;
                            validar2();
                            validacionconteo = 0;
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (!textBoxFolioFactura.Enabled) && (validacionconteo == 4))
                        {
                            if (!(comboBoxFalloGral.Text == "-- GRUPO --"))
                            {
                                MessageBox.Show("No todas las refacciones solicitadas están en existencia\n Espere hasta que almacén vuelva a tener existencias", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            conteorefaccionesverificadas = 2;
                            validar2();
                            validacionconteo = 0;
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (!textBoxFolioFactura.Enabled) && (validacionconteo == 7))
                        {
                            MessageBox.Show("No todas las refacciones han sido entregadas\nespere hasta que almacén le entregue todas las refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conteorefaccionesverificadas = 2;
                            validar2();
                            validacionconteo = 0;
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && ((!textBoxFolioFactura.Enabled) || (textBoxFolioFactura.Enabled)) && (validacionconteo == 6) && !mensaje)
                        {
                            conteorefaccionesverificadas = 1;
                            validar2();
                            validacionconteo = 0;
                        }
                    }
                    else if ((textBoxFolioFactura.Text != "") && ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) || (comboBoxEstatusMant.Text.Equals("REPROGRAMADA"))))
                        buttonAgregar.Visible = label34.Visible = true;
                }
            }
            else if (existencias)
            {
                existencias = false;
                validacionconteo = 0;
            }
        }

        private void comboBoxEstatusMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES") && comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES") && textBoxFolioFactura.Text != "") || (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))) && (((comboBoxEstatusMant.Text.Equals("LIBERADA") && comboBoxEstatusMant.Enabled && !string.IsNullOrWhiteSpace(labelFolio.Text))) || (!comboBoxReqRefacc.Enabled && !comboBoxExisRefacc.Enabled && comboBoxEstatusMant.Text.Equals("LIBERADA"))))
            {
                if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (!comboBoxEstatusMant.Enabled))
                    buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
                else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (comboBoxEstatusMant.Enabled))
                {
                    if ((string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)) && (string.IsNullOrWhiteSpace(textBoxMecanico.Text)) && (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && (comboBoxEstatusMant.Text != "LIBERADA") && (string.IsNullOrWhiteSpace(comboBoxReqRefacc.Text)) && (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                    {
                        MessageBox.Show("Algunos campos les faltan información", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        validar();
                    }
                    else if (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))
                    {
                        MessageBox.Show("El trabajo realizado no puede quedar en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        validar();
                    }
                    else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (estatusmantGV.Equals("LIBERADA")))
                        buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
                    else
                    {
                        if ((comboBoxFalloGral.Text != "") && (labelNomMecanico.Text != ".") && (textBoxTrabajoRealizado.Text != "") && (comboBoxEstatusMant.Text != ""))
                        {
                            timer2.Stop();
                            sumafech();
                            labelHoraTerminoM.Text = dateTimePicker2.Text;
                            comboBoxReqRefacc.Enabled = comboBoxExisRefacc.Enabled = buttonGuardar.Visible = label24.Visible = buttonAgregar.Visible = label39.Visible = false;
                            buttonFinalizar.Visible = label37.Visible = true;
                            Cancelar(false);
                        }
                        else
                        {
                            MessageBox.Show("Verifique los datos ingresados, puede que falten algunos por llenar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            validar();
                        }
                    }
                }
                else if ((comboBoxEstatusMant.Text.Equals("REPROGRAMADA")) && (string.IsNullOrWhiteSpace(estatusmantGV)) || (estatusmantGV.Equals("REPROGRAMADA")))
                {
                    labelHoraTerminoM.Text = textBoxTerminoMan.Text = "";
                    buttonFinalizar.Visible = label37.Visible = false;
                    buttonGuardar.Visible = label24.Visible = true;
                    Cancelar(true);
                }
                else if ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) && (string.IsNullOrWhiteSpace(estatusmantGV)) || (estatusmantGV.Equals("EN PROCESO")))
                {
                    labelHoraTerminoM.Text = textBoxTerminoMan.Text = "";
                    buttonFinalizar.Visible = label37.Visible = false;
                    buttonGuardar.Visible = label24.Visible = true;
                    Cancelar(true);
                }
            }
            else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (!string.IsNullOrWhiteSpace(labelFolio.Text)))
            {
                string textomensajexistencia = "";
                if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                    textomensajexistencia = "no se ha seleccionado si se requieren refacciones";
                else if (!comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                    textomensajexistencia = "falta la entrega de refacciones";
                else if (!(textBoxFolioFactura.Text != ""))
                    textomensajexistencia = "falta el folio de factura";
                MessageBox.Show("Aún no puede poner el reporte en estatus \'LIBERADA\' \nPorque " + textomensajexistencia + "", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxEstatusMant.SelectedIndex = 1;
            }
            else if ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) || (comboBoxEstatusMant.Text.Equals("REPROGRAMADA")))
            {
                if (!(comboBoxReqRefacc.SelectedIndex == 0))
                    if (comboBoxReqRefacc.SelectedIndex != 2)
                        if (!comboBoxFalloGral.Enabled)
                            buttonAgregar.Visible = label39.Visible = true;
                buttonFinalizar.Visible = label37.Visible = false;
                buttonGuardar.Visible = label24.Visible = true;
                if (comboBoxReqRefacc.SelectedIndex > 0 && labelNomMecanico.Text != "" && !comboBoxFalloGral.Enabled)
                    comboBoxReqRefacc.Enabled = true;
            }
        }

        private void checkBoxFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFechas.Checked)
            {
                dateTimePickerIni.Enabled = dateTimePickerFin.Enabled = true;
                comboBoxMesB.Enabled = false;
                comboBoxMesB.SelectedIndex = 0;
            }
            else
            {
                dateTimePickerIni.Enabled = dateTimePickerFin.Enabled = false;
                comboBoxMesB.Enabled = true;
            }
        }

        private void dateTimePickerAll_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        /* Acciones al presionar una tecla o dar click*/
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxSuperviso_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSuperviso.Text))
                labelNomSuperviso.Text = "...";
        }

        private void textBoxMecanicoApo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMecanicoApo.Text))
                labelNomMecanicoApo.Text = "..";
        }

        private void comboBoxMesB_TextChanged(object sender, EventArgs e)
        {
            month = "00";
            if (comboBoxMesB.Text.Equals("ENERO"))
                month = "01";
            else if (comboBoxMesB.Text.Equals("FEBRERO"))
                month = "02";
            else if (comboBoxMesB.Text.Equals("MARZO"))
                month = "03";
            else if (comboBoxMesB.Text.Equals("ABRIL"))
                month = "04";
            else if (comboBoxMesB.Text.Equals("MAYO"))
                month = "05";
            else if (comboBoxMesB.Text.Equals("JUNIO"))
                month = "06";
            else if (comboBoxMesB.Text.Equals("JULIO"))
                month = "07";
            else if (comboBoxMesB.Text.Equals("AGOSTO"))
                month = "08";
            else if (comboBoxMesB.Text.Equals("SEPTIEMBRE"))
                month = "09";
            else if (comboBoxMesB.Text.Equals("OCTUBRE"))
                month = "10";
            else if (comboBoxMesB.Text.Equals("NOVIEMBRE"))
                month = "11";
            else if (comboBoxMesB.Text.Equals("DICIEMBRE"))
                month = "12";
        }

        private void textBoxMecanico_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMecanico.Text))
                labelNomMecanico.Text = ".";
        }

        /* Movimiento de los botónes y bloqueo de la rueda de mouse */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonAll_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(60, 60);
        }

        private void btnall_MouseLeave(object sender, EventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(55, 55);
        }

        private void btnallbusq_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(40, 40);
        }

        private void btnallbusq_MouseLeave(object sender, EventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(35, 35);
        }

        void comboBoxAll_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void groupBoxAll_Paint(object sender, PaintEventArgs e)
        {
            GroupBox bx = sender as GroupBox;
            DrawGroupBox(bx, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void textBoxAll_Validated(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            while (txt.Text.Contains("  "))
                txt.Text = txt.Text.Replace("  ", " ");
        }

        private void dataGridViewAll_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void FormFallasMantenimiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            int total;
            hilo.Abort();
            if (!registroconteofilaspedref && idreportesupervision > 0)
            {
                total = dgvPMantenimiento.Rows.Count - inicolumn;
                MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "", v.c.dbconection());
                cmd1.ExecuteNonQuery();
                v.c.dbcon.Close();
                botonactualizar();
            }

        }

        /* Color a Celdas de GridView y Label */
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void labelCerrarHerr_MouseLeave(object sender, EventArgs e)
        {
            labelCerrarHerr.ForeColor = Color.FromArgb(75, 44, 52);
        }

        private void labelCerrarHerr_MouseDown(object sender, MouseEventArgs e)
        {
            labelCerrarHerr.ForeColor = Color.FromArgb(75, 44, 52);
        }

        private void btnadd_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnadd = sender as Button;
            btnadd.BackgroundImage = Properties.Resources.add;
        }

        private void btnadd_MouseLeave(object sender, EventArgs e)
        {
            Button btnadd = sender as Button;
            btnadd.BackgroundImage = Properties.Resources.menos_add;
        }

        private void dataGridViewMantenimiento_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DEL MANTENIMIENTO")
                if (Convert.ToString(e.Value) == "EN PROCESO")
                    e.CellStyle.BackColor = Color.Khaki;
                else if (Convert.ToString(e.Value) == "LIBERADA")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else if (Convert.ToString(e.Value) == "REPROGRAMADA")
                    e.CellStyle.BackColor = Color.LightCoral;
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "TIPO DE FALLO")
                if (Convert.ToString(e.Value) == "CORRECTIVO")
                    e.CellStyle.BackColor = Color.Khaki;
                else if (Convert.ToString(e.Value) == "PREVENTIVO")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else if (Convert.ToString(e.Value) == "REITERATIVO")
                    e.CellStyle.BackColor = Color.LightCoral;
                else if (Convert.ToString(e.Value) == "REPROGRAMADO")
                    e.CellStyle.BackColor = Color.LightBlue;
                else if (Convert.ToString(e.Value) == "SEGUIMIENTO")
                    e.CellStyle.BackColor = Color.FromArgb(246, 144, 123);
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DE REFACCIONES")
                if (Convert.ToString(e.Value) == "SE REQUIEREN REFACCIONES")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else if (Convert.ToString(e.Value) == "NO SE REQUIEREN REFACCIONES")
                    e.CellStyle.BackColor = Color.LightCoral;
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "EXISTENCIA DE REFACCIONES EN ALMACEN")
                if (Convert.ToString(e.Value) == "EXISTENCIA DE REFACCIONES")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else if (Convert.ToString(e.Value) == "EN ESPERA DE LA REFACCIÓN")
                    e.CellStyle.BackColor = Color.Khaki;
                else if (Convert.ToString(e.Value) == "SIN REFACCIONES")
                    e.CellStyle.BackColor = Color.LightCoral;
        }

        private void dataGridViewMRefaccion_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvPMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DE LA REFACCION")
                if (Convert.ToString(e.Value) == "EXISTENCIA")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else if (Convert.ToString(e.Value) == "SIN EXISTENCIA")
                    e.CellStyle.BackColor = Color.LightCoral;
                else if (Convert.ToString(e.Value) == "INCOMPLETO")
                    e.CellStyle.BackColor = Color.FromArgb(255, 144, 51);
            if (this.dgvPMantenimiento.Columns[e.ColumnIndex].Name == "CANTIDAD POR ENTREGAR")
                if (Convert.ToString(e.Value) != "0")
                    e.CellStyle.BackColor = Color.Khaki;
        }

        private void comboBoxEstatusMant_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
            {
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 1:
                            e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 2:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 3:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                    }
                }
            }
        }

        private void comboBoxReqRefacc_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
            {
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 1:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxReqRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 2:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxReqRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                    }
                }
            }
        }

        private void comboBoxExisRefacc_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
            {
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 1:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 2:
                            e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 3:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                    }
                }
            }
        }

        public void combos_para_otros_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    Brush brush = new SolidBrush(cbx.ForeColor);
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();

                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        public void combo_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    Brush brush = new SolidBrush(cbx.ForeColor);
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }
        public int rowIndex { get; set; }
        bool mensaje = false;

        private void dataGridViewMantenimiento_MouseClick(object sender, MouseEventArgs e)
        {
            banderaeditar = mensaje = false;
            if (peditar)
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip mn = new System.Windows.Forms.ContextMenuStrip();
                    int xy = dgvMantenimiento.HitTest(e.X, e.Y).RowIndex;
                    if (xy >= 0)
                        mn.Items.Add("Editar".ToUpper(), controlFallos.Properties.Resources.pencil).Name = "Editar".ToUpper();
                    mn.Show(dgvMantenimiento, new Point(e.X, e.Y));
                    mn.ItemClicked += new ToolStripItemClickedEventHandler(mn_ItemClicked);
                }
            }
        }

        public void mn_ItemClicked(object sender, ToolStripItemClickedEventArgs e) // Edicion en el gridview de mantenimiento
        {
            mensaje = false;
            estatusmantGV = dgvMantenimiento.CurrentRow.Cells["ESTATUS DEL MANTENIMIENTO"].Value.ToString();
            if (!(estatusmantGV.Equals("")))
            {
                valdeval();
                if (((fgeneralanterior.Equals(validacionfgeneral) || ((valorfallogeneral == 1) || (string.IsNullOrWhiteSpace(fgeneralanterior)))) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && (exisrefaccionanterior.Equals(validacionexisrefacc)) && (reqrefanterior.Equals(validacionreqrefacc)) && (trabrealizadoanterior.Trim().Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Trim().Equals(textBoxFolioFactura.Text.Trim())) && ((estatusmantanterior.Equals(validacionestatusmant)) || (comboBoxEstatusMant.Text.Equals("EN PROCESO"))) && (supervisoanterior.Equals(labelNomSuperviso.Text)) && (observacionesmantanterior.Trim().Equals(textBoxObsMan.Text.Trim())) && ((inicolumn == 0) || (inicolumn == fincolumn))))
                {
                    switch (e.ClickedItem.Name.ToString())
                    {
                        case "EDITAR":
                            codedicion();
                            Cancelar(false);
                            break;

                        default:
                            MessageBox.Show("DEFAULT");
                            break;
                    }
                }
                else
                {
                    if (MessageBox.Show("Si usted cambia de reporte sin guardar perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        switch (e.ClickedItem.Name.ToString())
                        {
                            case "EDITAR":
                                codedicion();
                                Cancelar(false);
                                break;

                            default:
                                MessageBox.Show("DEFAULT");
                                break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No puede editar un reporte si no se ha guardado por lo menos una vez", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Cancelar(bool Visible)
        {
            btnCancelar.Visible = lblCancelar.Visible = Visible;
        }

        private void textBoxCantidad_TextChanged(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                if (string.IsNullOrWhiteSpace(textBoxCantidad.Text) || textBoxCantidad.Text == ".")
                    textBoxCantidad.Text = "0";
                if ((idfamilianterior == Convert.ToInt32(comboBoxFamilia.SelectedValue) || (comboBoxFamilia.SelectedIndex == 0)) && (idrefaccionanterior == Convert.ToInt32(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior == Convert.ToDouble(textBoxCantidad.Text) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
            }
        }

        private void dataGridViewMantenimiento_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                dgvMantenimiento.CurrentCell = dgvMantenimiento.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void groupBoxSupervision_Enter(object sender, EventArgs e)
        {

        }
    }
}