using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace controlFallos
{
    public partial class percances : Form
    {
        int idReporteTemp, idconductor, idUsuario, folio, economicoAnterior, idconductorAnterior, servicioenLaborAnterior, direccionAnterior, estacion1, estacion2, estacion3, estacion4, numSeguroTransmasivoAnterior, economicoRecupAnterior, numActaExtraAnterior, estacionAnterior, supervisorAsistenciaPercanceAnterior, unidadAsistenciaMedicaAnterior, statusFinalizado, supervisorFinalizar, conductorFinalizar, dibujoExportado;
        DateTime fechaAccidenteAnterior, horaAccidenteAnterior, horaOtorgnumSegAnterior, horaajustLlegaSiniestro;
        public object usuariofinalizo;
        bool pinsertar { set; get; }
        Thread th;
        bool pconsultar { set; get; }
        bool peditar { set; get; }
        validaciones v = new validaciones();
        MemoryStream pdfFOREDIT;
        conexion c = new conexion();
        List<int> trazosimagenActual = new List<int>();
        List<Point>[] imagenes = new List<Point>[4], imagenesAnterior = new List<Point>[4];
        string binarizedPDF, lugarAccidenteAnterior, sintesisOcurridoAnterior, descripcionAnterior, marcaTerceroAnterior, yearTerceroAnterior, placasTerceroAnterior, nombreCTerceroAnterior, telefonoTerceroAnterior, domicilioTerceroAnterior, nombreAjustadorAnterior, solucionAnterior, pertenecienteaAsistenciaMedicaAnterior, responsabeUnidadAsistenciaMedicaAnterior, encasoLesionadosAnteriorAnterior, comentariosAnterior;
        bool editar, yaAparecioMensaje;
        public percances(int idUsuario)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.idUsuario = idUsuario;
            obtenerPrivilegios();
            cbxgeteco.DrawItem += v.combos_DrawItem;
            cbxgetServicio.DrawItem += v.combos_DrawItem;
            cbxgetdireccion.DrawItem += v.combos_DrawItem;
            cbmes.DrawItem += v.combos_DrawItem;
            cbxgetestacion1.DrawItem += v.combos_DrawItem;
            cbxgetestacion2.DrawItem += v.combos_DrawItem;
            cbxgetestacion3.DrawItem += v.combos_DrawItem;
            cbxgetestacion4.DrawItem += v.combos_DrawItem;
            cbxgetecorecup.DrawItem += v.combos_DrawItem;
            cbxgetestacion.DrawItem += v.combos_DrawItem;
            cbxgetempresa.DrawItem += v.combos_DrawItem;
            cbxgetexoBusq.DrawItem += v.combos_DrawItem;
            cbxgetconductor.DrawItem += v.combos_DrawItem;
            cbxgetarea.DrawItem += v.combos_DrawItem;
            cbxgetEXTsupervisor.DrawItem += v.combos_DrawItem;
            cbxgetempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetarea.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgeteco.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetServicio.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetdireccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetestacion1.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetestacion2.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetestacion3.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetestacion4.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetecorecup.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetEXTsupervisor.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetexoBusq.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetconductor.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbmes.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxgetestacion.MouseWheel += v.paraComboBox_MouseWheel;
            if (pinsertar || peditar)
            {
                initializeDirections();
                initializeCompanies();
                initializeSupervisors();
                DTPgetDate.MinDate = DateTime.Now.Subtract(TimeSpan.FromDays(2));
                DTPgetDate.MaxDate = DateTime.Now;
                generaFolio();
            }
            if (pconsultar)
            {
                v.inimeses(cbmes);
                initializeECOSBusq();
                initializeConductorsBusq();
                initializeReports("WHERE DATE_FORMAT(t1.fechaHoraAccidente, '%m') = " + DateTime.Today.ToString("MM"));
                string res = v.getaData("SELECT MIN(fechaHoraAccidente) FROM reportepercance").ToString();
                string str = ((res == "" ? null : res) ?? DateTime.Today.Subtract(TimeSpan.FromDays(2)).ToString());
                dtpFechaA.MinDate = dtpFechaDe.MinDate = DateTime.Parse(str);
                dtpFechaA.MaxDate = dtpFechaDe.MaxDate = DateTime.Now;
            }
            resetPixels();
        }
        void obtenerPrivilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {
                pinsertar = v.getBoolFromInt(Convert.ToInt16(privilegiosTemp[0]));
                pconsultar = v.getBoolFromInt(Convert.ToInt16(privilegiosTemp[1]));
                peditar = v.getBoolFromInt(Convert.ToInt16(privilegiosTemp[2]));
                mostrar();
            }
            else Dispose();
        }
        void mostrar()
        {
            if (pinsertar || peditar) lblopcionales.Visible = label72.Visible = p1.Visible = true;
            if (pconsultar) groupBox7.Visible = dgvpercances.Visible = true;
            if (peditar && !pinsertar) { btnsave.BackgroundImage = Properties.Resources.pencil; editar = true; }
            if (peditar) label73.Visible = true;
        }
        void initializeSupervisors() { v.iniCombos("SELECT idPersona as id, UPPER(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) as nombre FROM cpersonal WHERE status=1 AND empresa = 1 AND area = 1 ORDER BY CONCAT(nombres,' ',apPaterno,' ',apMaterno) ASC", cbxgetEXTsupervisor, "id", "nombre", "-- SELECCIONE SUPERVISOR --"); }
        private void button5_Click(object sender, EventArgs e)
        {
            Document dc = new Document(PageSize.LETTER);
            dc.SetMargins(21f, 21f, 31f, 31f);
            float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f };
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\Desktop";
            saveFileDialog1.Title = "Guardar reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            string filename = "";
            DialogResult ews = DialogResult.OK;
            try
            {
                if ((ews = saveFileDialog1.ShowDialog(this)) == DialogResult.OK)
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
                        int actual = 1;
                        FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        PdfWriter writer = PdfWriter.GetInstance(dc, file);
                        dc.Open();
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
                        Paragraph saltoDeLinea1 = new Paragraph(Environment.NewLine, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL));
                        dc.Add(saltoDeLinea1);
                        dc.Add(tbheader(1, 2));
                        dc.Add(saltoDeLinea1);
                        byte[] img = Convert.FromBase64String(v.transmasivo);
                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                        imagen.ScalePercent(12f);
                        imagen.SetAbsolutePosition(50f, 687f);
                        imagen.Alignment = Element.ALIGN_LEFT;
                        dc.Add(imagen);
                        string[] res = v.getaData(string.Format("SET lc_time_names='es_ES';SELECT CONVERT ( CONCAT(CONCAT('RP-', LPAD(t1.consecutivo, 7, 0)),'|',DATE_FORMAT(fechaHoraAccidente, '%d - %M - %y'),'|',CONCAT(t3.identificador,LPAD(t2.consecutivo, 4, '0')),'|',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno,'|',credencial) FROM cpersonal WHERE idpersona = t1.conductorfkcpersonal), '|', DATE_FORMAT(fechaHoraAccidente, '%H:%m'), '|', IF(servicioenlaborfkcservicios IS NOT NULL, (SELECT nombre FROM cservicios WHERE idservicio = servicioenlaborfkcservicios), ' '), '|',IF(lugaraccidente IS NOT NULL,lugaraccidente,' '),'|',IF(direccion IS NOT NULL,IF(direccion = 1, 'Norte', 'Sur'),' '),'|',IF(estacion1fkcestaciones IS NOT NULL,(SELECT estacion FROM cestaciones WHERE idestacion = estacion1fkcestaciones), ' '),'|',IF(estacion2fkcestaciones IS NOT NULL,(SELECT estacion FROM cestaciones WHERE idestacion = estacion2fkcestaciones), ' '),'|',IF(estacion3fkcestaciones IS NOT NULL,(SELECT estacion FROM cestaciones WHERE idestacion = estacion3fkcestaciones), ' '), '|', IF(estacion4fkcestaciones IS NOT NULL, (SELECT  estacion FROM cestaciones WHERE idestacion = estacion4fkcestaciones), ' '), '|', IF(ecorecuperacionfkcunidades IS NOT NULL, (SELECT  CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea WHERE t1.idunidad = ecorecuperacionfkcunidades), ' '), '|', IF(estacionfkcestaciones IS NOT NULL, (SELECT  estacion FROM cestaciones WHERE idestacion = estacionfkcestaciones), ' '), '|', IF(sintesisocurrido IS NOT NULL, sintesisocurrido, ' '), '|', IF(coordenadasimagenes IS NOT NULL, coordenadasimagenes, ' '), '|', IF(descripcion IS NOT NULL, descripcion, ' '), '|', IF(marcavehiculotercero IS NOT NULL, marcavehiculotercero, ' '), '|', IF(yearvehiculotercero IS NOT NULL, yearvehiculotercero,' '),'|',IF(placasvehiculotercero IS NOT NULL,placasvehiculotercero,' '),'|',IF(nombreconductortercero IS NOT NULL,nombreconductortercero,' '),'|',IF(telefonoconductortercero IS NOT NULL,telefonoconductortercero,' '),'|',IF(domicilioconductortercero IS NOT NULL,domicilioconductortercero,' '),'|',IF(numreporteseguro IS NOT NULL,numreporteseguro,' '),'|',IF(horaotorgamiento IS NOT NULL,horaotorgamiento,' '),'|',IF(horallegadaseguro IS NOT NULL,horallegadaseguro,' '),'|',IF(nombreajustador IS NOT NULL,nombreajustador,' '),'|',IF(solucion IS NOT NULL,solucion,' '),'|',IF(numacta IS NOT NULL,numacta,' '),'|',IF(supervisorkcpersonal IS NOT NULL,(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = supervisorkcpersonal),' '),'|',IF(unidadmedica IS NOT NULL,unidadmedica,' '),'|',IF(perteneceunidad IS NOT NULL,perteneceunidad,' '),'|',IF(nombreResponsableUnidad IS NOT NULL,nombreResponsableUnidad,' '),'|',IF(encasolesionados IS NOT NULL,encasolesionados,' '),'|',IF(dibujo IS NOT NULL,dibujo,' '),'|',IF(comentarios IS NOT NULL,comentarios,' '),'|',IF(firmaconductorfkcpersonal IS NOT NULL,(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = firmaconductorfkcpersonal),' '),'|',IF(firmasupervisorfkcpersonal IS NOT NULL,(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = firmasupervisorfkcpersonal),' '),'|',t2.modelofkcmodelos,'|',if(evidencia1 is not null,evidencia1,' '),'|',if(evidencia2 is not null,evidencia2,' '),'|',if(evidencia3 is not null, evidencia3,' '),'|', if(evidencia4 is not null, evidencia4,' ')) USING utf8)as r FROM reportepercance AS t1 INNER JOIN cunidades AS t2 ON t1.ecofkcunidades = t2.idunidad INNER JOIN careas AS t3 ON t2.areafkcareas = t3.idarea INNER JOIN cempresas AS t4 ON t3.empresafkcempresas = t4.idempresa where idreportePercance = '{0}'", idReporteTemp)).ToString().Split('|');
                        PdfPTable tbpercance = new PdfPTable(3);
                        tbpercance.WidthPercentage = 95;
                        tbpercance.AddCell(valorCampo("Folio: " + res[0], 1, 1, 0, FontFactory.GetFont("CALIBRI", 14, iTextSharp.text.Font.BOLD)));
                        tbpercance.AddCell(valorCampo("Fecha del Percance: ", 1, 2, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbpercance.AddCell(valorCampo(v.mayusculas(res[1]), 1, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        dc.Add(tbpercance);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontect = new PdfPTable(30);
                        tbcontect.WidthPercentage = 95;
                        tbcontect.AddCell(valorCampo("Económico: ", 3, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontect.AddCell(valorCampo(res[2], 5, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontect.AddCell(valorCampo("Nombre del Conductor: ", 6, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontect.AddCell(valorCampo(res[3], 16, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontect);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontent2 = new PdfPTable(50);
                        tbcontent2.WidthPercentage = 95;
                        tbcontent2.AddCell(valorCampo("Credencial: ", 5, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent2.AddCell(valorCampo(res[4], 7, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontent2.AddCell(valorCampo("Hora Aproximada del Accidente: ", 13, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent2.AddCell(valorCampo(res[5], 7, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontent2.AddCell(valorCampo("Servicio en Labor", 8, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent2.AddCell(valorCampo(res[6], 10, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent2);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontent3 = new PdfPTable(30);
                        tbcontent3.WidthPercentage = 95;
                        tbcontent3.AddCell(valorCampo("Lugar del Accidente", 5, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent3.AddCell(valorCampo(res[7], 15, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontent3.AddCell(valorCampo("Dirección: ", 3, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent3.AddCell(valorCampo(v.mayusculas(res[8]), 7, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent3);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontent4 = new PdfPTable(80);
                        tbcontent4.WidthPercentage = 95;
                        tbcontent4.AddCell(valorCampo("Se Omite Servicio de ", 14, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent4.AddCell(valorCampo(res[9], 14, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontent4.AddCell(valorCampo("a ", 2, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent4.AddCell(valorCampo(res[10], 15, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontent4.AddCell(valorCampo("y de ", 4, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent4.AddCell(valorCampo(res[11], 14, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontent4.AddCell(valorCampo("a ", 2, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent4.AddCell(valorCampo(res[12], 15, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent4);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontent5 = new PdfPTable(60);
                        tbcontent5.WidthPercentage = 95;
                        tbcontent5.AddCell(valorCampo("Recuperando la Jornada con ", 14, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent5.AddCell(valorCampo(res[13], 11, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontent5.AddCell(valorCampo("en ", 2, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontent5.AddCell(valorCampo(res[14], 33, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent5);
                        PdfPTable tbcontent6 = new PdfPTable(1);
                        tbcontent6.WidthPercentage = 95;
                        tbcontent6.AddCell(valorCampo("Sintesis de Lo Ocurrido", 1, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        PdfPCell sintesis1 = new PdfPCell(new Phrase("___________________________________________________________________________________________________________", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        sintesis1.Border = 0;
                        string temp = "", temp2 = "";
                        string[] tempp = res[15].Split(' ');
                        for (int i = 0; i < tempp.Length; i++)
                        {
                            if ((temp + tempp[i]).Length < 120)
                                temp += " " + tempp[i];
                            else
                            {
                                temp2 += temp + "|";
                                temp = "";
                                i--;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(temp)) temp2 += temp;
                        tempp = temp2.Split('|');
                        for (int i = 0; i < tempp.Length; i++)
                            tbcontent6.AddCell(valorCampo(tempp[i], 1, Element.ALIGN_JUSTIFIED, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        for (int i = tempp.Length; i < 5; i++)
                            tbcontent6.AddCell(sintesis1);
                        dc.Add(tbcontent6);
                        PdfPTable tbcontent7 = new PdfPTable(1);
                        tbcontent7.WidthPercentage = 95;
                        tbcontent7.AddCell(valorCampo("Señala las(s) partes(s) dañadas del Bus: ", 1, 1, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent7);
                        List<Point>[] imgtemp = new List<Point>[4];
                        imgtemp[0] = new List<Point>(); imgtemp[1] = new List<Point>(); imgtemp[2] = new List<Point>(); imgtemp[3] = new List<Point>();
                        string[] imagenes = res[16].Split('/');
                        for (int i = 0; i < imagenes.GetLength(0); i++)
                        {
                            string[] image = imagenes[i].Split(':');
                            string[] imagePoints = image[1].Split(';'); foreach (string point in imagePoints)
                            {
                                object[] xy = point.Trim().Split(',');
                                var p = new Point(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1])); imgtemp[i].Add(p);
                            }
                        }
                        byte[] img1 = null; byte[] img2 = null; byte[] img3 = null; byte[] img4 = null;
                        if (res[39] == "1")
                        {
                            img1 = v.ImageToString(ediciionBitmap(Properties.Resources.FRENTE, imgtemp[0]));
                            img2 = v.ImageToString(ediciionBitmap(Properties.Resources.LATERAL_DERECHO, imgtemp[1]));
                            img3 = v.ImageToString(ediciionBitmap(Properties.Resources.LATERAL_IZQUIERDO, imgtemp[2]));
                            img4 = v.ImageToString(ediciionBitmap(Properties.Resources.TRASERO, imgtemp[3]));
                        }
                        else
                        {
                            img1 = v.ImageToString(ediciionBitmap(Properties.Resources.t_frente, imgtemp[0]));
                            img2 = v.ImageToString(ediciionBitmap(Properties.Resources.t_derecho, imgtemp[1]));
                            img3 = v.ImageToString(ediciionBitmap(Properties.Resources.t_izquierdo, imgtemp[2]));
                            img4 = v.ImageToString(ediciionBitmap(Properties.Resources.t_trasero, imgtemp[3]));
                        }

                        dc.Add(imagen1(img1, 70f, 80f, 290f, Element.ALIGN_CENTER));
                        dc.Add(imagen1(img4, 70f, 340f, 290f, Element.ALIGN_CENTER));
                        dc.Add(imagen1(img3, 120f, 80f, 195f, Element.ALIGN_CENTER));
                        dc.Add(imagen1(img2, 120f, 80f, 98f, Element.ALIGN_CENTER));
                        for (int i = 0; i < 27; i++) dc.Add(saltoDeLinea1);
                        actual++;
                        PdfPTable tbcontent8 = new PdfPTable(1);
                        tbcontent8.WidthPercentage = 95;
                        tbcontent8.AddCell(valorCampo("Descripción: " + res[17], 1, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        dc.Add(tbcontent8);
                        for (int i = 0; i < 3; i++) dc.Add(saltoDeLinea1);
                        content = writer.DirectContent;
                        pageBorderRect = new iTextSharp.text.Rectangle(dc.PageSize);
                        pageBorderRect.Left += dc.LeftMargin;
                        pageBorderRect.Right -= dc.RightMargin;
                        pageBorderRect.Top -= dc.TopMargin;
                        pageBorderRect.Bottom += dc.BottomMargin;
                        content.SetColorStroke(BaseColor.BLACK);
                        content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                        content.SetLineWidth(2f);
                        content.Stroke();
                        dc.Add(tbheader(2, 2));
                        dc.Add(imagen);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontent9 = new PdfPTable(1);
                        tbcontent9.WidthPercentage = 95;
                        tbcontent9.AddCell(valorCampo("Datos del Tercero", 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 12, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent9);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataThird = new PdfPTable(60);
                        tbcontentDataThird.WidthPercentage = 95;
                        tbcontentDataThird.AddCell(valorCampo("Marca del Vehiculo: ", 10, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataThird.AddCell(valorCampo(res[18], 15, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontentDataThird.AddCell(valorCampo("Año: ", 3, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataThird.AddCell(valorCampo(res[19], 10, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontentDataThird.AddCell(valorCampo("Placas: ", 5, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataThird.AddCell(valorCampo(res[20], 17, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataThird);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataCThird = new PdfPTable(40);
                        tbcontentDataCThird.WidthPercentage = 95;
                        tbcontentDataCThird.AddCell(valorCampo("Nombre del Conductor: ", 8, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataCThird.AddCell(valorCampo(res[21], 32, Element.ALIGN_LEFT, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataCThird);
                        dc.Add(saltoDeLinea1);
                        PdfPTable Domicilio = new PdfPTable(1);
                        Domicilio.WidthPercentage = 95;
                        temp = ""; temp2 = "Domicilio: ";
                        tempp = res[22].Split(' ');
                        for (int i = 0; i < tempp.Length; i++)
                        {
                            if ((temp + tempp[i]).Length < 120)
                                temp += " " + tempp[i];
                            else
                            {
                                temp2 += temp + "|";
                                temp = "";
                                i--;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(temp)) temp2 += temp;
                        tempp = temp2.Split('|');
                        for (int i = 0; i < tempp.Length; i++) { Domicilio.AddCell(valorCampo(tempp[i], 1, Element.ALIGN_LEFT, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL))); }
                        for (int i = tempp.Length; i < 2; i++) { Domicilio.AddCell(sintesis1); }
                        dc.Add(Domicilio);
                        dc.Add(saltoDeLinea1);
                        PdfPTable telefono = new PdfPTable(50);
                        telefono.WidthPercentage = 95;
                        telefono.AddCell(valorCampo("Teléfono: ", 5, 0, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        telefono.AddCell(valorCampo(res[23], 45, 0, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(telefono);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontent10 = new PdfPTable(1);
                        tbcontent10.WidthPercentage = 95;
                        tbcontent10.AddCell(valorCampo("Seguro de Unidad TransMasivo", 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 12, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent10);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataUnidad = new PdfPTable(50);
                        tbcontentDataUnidad.WidthPercentage = 95;
                        tbcontentDataUnidad.AddCell(valorCampo("Se Reporta al Seguro y se otorga el núm: ", 16, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataUnidad.AddCell(valorCampo(res[24], 5, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontentDataUnidad.AddCell(valorCampo("siendo las ", 5, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataUnidad.AddCell(valorCampo(DateTime.Parse(res[25]).ToString("HH:mm"), 5, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontentDataUnidad.AddCell(valorCampo("horas.", 19, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        dc.Add(tbcontentDataUnidad);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataUnidad2 = new PdfPTable(50);
                        tbcontentDataUnidad2.WidthPercentage = 95;
                        tbcontentDataUnidad2.AddCell(valorCampo("Llega al lugar del sinistro a las: ", 12, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataUnidad2.AddCell(valorCampo(DateTime.Parse(res[26]).ToString("HH:mm"), 5, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontentDataUnidad2.AddCell(valorCampo("horas.", 33, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        dc.Add(tbcontentDataUnidad2);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataUnidad3 = new PdfPTable(20);
                        tbcontentDataUnidad3.WidthPercentage = 95;
                        tbcontentDataUnidad3.AddCell(valorCampo("Asiste el ajustador de nombre:", 5, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataUnidad3.AddCell(valorCampo(res[27], 15, Element.ALIGN_LEFT, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataUnidad3);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontent11 = new PdfPTable(1);
                        tbcontent11.WidthPercentage = 95;
                        tbcontent11.AddCell(valorCampo("Datos Extra", 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 12, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontent11);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataExtra = new PdfPTable(50);
                        tbcontentDataExtra.WidthPercentage = 95;
                        tbcontentDataExtra.AddCell(valorCampo("Solución: ", 4, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra.AddCell(valorCampo(res[28], 29, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontentDataExtra.AddCell(valorCampo("No. De acta ", 5, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra.AddCell(valorCampo(res[29], 12, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataExtra);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataExtra1 = new PdfPTable(20);
                        tbcontentDataExtra1.WidthPercentage = 95;
                        tbcontentDataExtra1.AddCell(valorCampo("Supervisor en asistencia del percance: ", 6, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra1.AddCell(valorCampo(res[30], 14, Element.ALIGN_LEFT, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataExtra1);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataExtra2 = new PdfPTable(50);
                        tbcontentDataExtra2.WidthPercentage = 95;
                        tbcontentDataExtra2.AddCell(valorCampo("En caso de ser necesaria asistencia médica, llega la unidad: ", 23, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra2.AddCell(valorCampo(res[31], 7, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        tbcontentDataExtra2.AddCell(valorCampo(",que pertenece a ", 8, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra2.AddCell(valorCampo(res[32], 12, Element.ALIGN_CENTER, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataExtra2);
                        dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataExtra3 = new PdfPTable(20);
                        tbcontentDataExtra3.WidthPercentage = 95;
                        tbcontentDataExtra3.AddCell(valorCampo("Responsable de La Unidad:", 5, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra3.AddCell(valorCampo(res[33], 15, Element.ALIGN_LEFT, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataExtra3);
                        PdfPTable tbcontentDataExtra6 = new PdfPTable(1);
                        tbcontentDataExtra6.WidthPercentage = 95;
                        tbcontentDataExtra6.AddCell(valorCampo("En caso de haber lesionados y ser necesario su translado:", 30, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra6.AddCell(valorCampo(res[34], 1, Element.ALIGN_LEFT, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        dc.Add(tbcontentDataExtra6);
                        for (int i = 0; i < 3; i++) dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataExtra4 = new PdfPTable(1);
                        tbcontentDataExtra4.WidthPercentage = 95;
                        tbcontentDataExtra4.AddCell(valorCampo("Comentarios:", 5, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra4.AddCell(valorCampo(res[36], 15, Element.ALIGN_LEFT, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                        for (int i = 0; i < 2; i++)
                            tbcontentDataExtra4.AddCell(sintesis1);
                        dc.Add(tbcontentDataExtra4);
                        for (int i = 0; i < 6; i++) dc.Add(saltoDeLinea1);
                        PdfPTable tbcontentDataExtra5 = new PdfPTable(3);
                        tbcontentDataExtra5.WidthPercentage = 95;
                        tbcontentDataExtra5.AddCell(valorCampo(res[37], 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra5.AddCell(valorCampo("                   ", 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra5.AddCell(valorCampo(res[38], 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra5.AddCell(valorCampo("Firma y Credencial del Conductor", 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL), 5));
                        tbcontentDataExtra5.AddCell(valorCampo("                   ", 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                        tbcontentDataExtra5.AddCell(valorCampo("Firma del Supervisor", 1, Element.ALIGN_CENTER, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL), 5));
                        dc.Add(tbcontentDataExtra5);
                        for (int i = 0; i < 8; i++) dc.Add(saltoDeLinea1);
                        PdfContentByte cb = writer.DirectContent;
                        MemoryStream PDFtemp = new MemoryStream(Convert.FromBase64String(res[35]));
                        PdfReader reader = new PdfReader(PDFtemp);
                        PdfImportedPage page = writer.GetImportedPage(reader, 1);
                        cb.AddTemplate(page, 0, 0);
                        dc.AddCreationDate();
                        if (!string.IsNullOrWhiteSpace(res[40]) || !string.IsNullOrWhiteSpace(res[41]) || !string.IsNullOrWhiteSpace(res[42]) || !string.IsNullOrWhiteSpace(res[43]))
                        {
                            dc.NewPage();
                            PdfPTable evidencias = new PdfPTable(1);
                            evidencias.WidthPercentage = 95;
                            evidencias.AddCell(valorCampo("EVIDENCIAS", 1, 1, 0, FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD)));
                            int x = 130, y = 560;
                            if (!string.IsNullOrWhiteSpace(res[40]))
                            {
                                byte[] ev1 = Convert.FromBase64String(res[40]);
                                dc.Add(imagen1(ev1, 85f, x, y, Element.ALIGN_CENTER));
                                y -= 182;
                            }
                            if (!string.IsNullOrWhiteSpace(res[41]))
                            {
                                byte[] ev2 = Convert.FromBase64String(res[41]);
                                dc.Add(imagen1(ev2, 85f, x, y, Element.ALIGN_CENTER));
                                y -= 182;
                            }
                            if (!string.IsNullOrWhiteSpace(res[42]))
                            {
                                byte[] ev3 = Convert.FromBase64String(res[42]);
                                dc.Add(imagen1(ev3, 85f, x, y, Element.ALIGN_CENTER));
                                y -= 182;
                            }
                            if (!string.IsNullOrWhiteSpace(res[43]))
                            {
                                byte[] ev4 = Convert.FromBase64String(res[43]);
                                dc.Add(imagen1(ev4, 85, x, y, Element.ALIGN_CENTER));
                                y -= 182;
                            }
                            dc.Add(evidencias);
                        }
                        dc.Close();
                        Process.Start(filename);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        PdfPTable tbheader(int actual, int total)
        {
            PdfPTable tbheader = new PdfPTable(9);
            try
            {
                string[] encabezado = v.getaData("SELECT CONCAT(nombrereporte,'|',codigoreporte,'|',vigencia,'|',revision) FROM sistrefaccmant.encabezadoreportes WHERE reporte =1").ToString().Split('|');
                tbheader.WidthPercentage = 95;
                tbheader.DefaultCell.PaddingTop = 4;
                tbheader.HorizontalAlignment = 1;
                PdfPCell c01 = new PdfPCell(new Phrase("", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                c01.Rowspan = 4;
                c01.Colspan = 3;
                tbheader.AddCell(c01);
                PdfPCell c02 = new PdfPCell(new Phrase("Nombre: " + encabezado[0], FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                c02.Colspan = 6;
                tbheader.AddCell(c02);
                PdfPCell c12 = new PdfPCell(new Phrase("Código: " + encabezado[1], FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                c12.Colspan = 6;
                tbheader.AddCell(c12);
                PdfPCell c22 = new PdfPCell(new Phrase("Vigencia: " + v.mayusculas(DateTime.Parse(encabezado[2]).ToString("MMMM - yyyy")), FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                c22.Colspan = 3;
                tbheader.AddCell(c22);
                PdfPCell c23 = new PdfPCell(new Phrase("Revisión: " + encabezado[3], FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                c23.Colspan = 1;
                tbheader.AddCell(c23);
                PdfPCell c24 = new PdfPCell(new Phrase("Página " + actual + " de " + total, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                c24.Colspan = 2;
                c24.HorizontalAlignment = 1;
                tbheader.AddCell(c24);
            }
            catch { }
            return tbheader;
        }
        iTextSharp.text.Image imagen1(byte[] img, float ScalePercent, float x, float y, int align)
        {
            iTextSharp.text.Image imagenFRENTE = iTextSharp.text.Image.GetInstance(img);
            imagenFRENTE.ScalePercent(ScalePercent);
            imagenFRENTE.SetAbsolutePosition(x, y);
            imagenFRENTE.Alignment = align;
            return imagenFRENTE;
        }
        Bitmap ediciionBitmap(Bitmap bmp, List<Point> img)
        {
            Point[] coordenadas = img.ToArray();
            if (img.Count > 0)
            {
                Pen blackPen = new Pen(Color.Blue, 3);
                for (int i = 0; i < img.Count; i++)
                {
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        if (coordenadas.Length == 1)
                        {
                            var r = new System.Drawing.Rectangle(new Point(coordenadas[i].X, coordenadas[i].Y), new Size(2, 2));
                            gr.DrawRectangle(blackPen, r);
                        }
                        else
                        {
                            if (coordenadas[i].X == -1 || coordenadas[i].Y == -1 || i == coordenadas.Length - 1) { }
                            else
                            {
                                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                gr.DrawLine(blackPen, coordenadas[i].X, coordenadas[i].Y, (coordenadas[i + 1].X == -1 ? coordenadas[i].X : coordenadas[i + 1].X), (coordenadas[i + 1].Y == -1 ? coordenadas[i].Y : coordenadas[i + 1].Y));
                            }
                        }
                    }

                }
            }
            return bmp;
        }
        List<string> Paths = new List<string>();
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream stream = new FileStream((Application.StartupPath + @"\Report" + lblFolio.Text + ".pdf"), FileMode.Create);
                Paths.Add((Application.StartupPath + @"\Report" + lblFolio.Text + ".pdf"));
                BinaryWriter writer =
                    new BinaryWriter(stream);
                MemoryStream temp = new MemoryStream();
                writer.Write(pdfFOREDIT.ToArray(), 0, pdfFOREDIT.ToArray().Length);
                writer.Close();
                Process.Start((Application.StartupPath + @"\Report" + lblFolio.Text + ".pdf"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void txtgetDescripcion_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) { if (e.KeyCode == Keys.Tab) button1_Click(null, e); }
        private void txtgetComentarios_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) { if (e.KeyCode == Keys.Tab) button2_Click(null, e); }
        private void getCambios(object sender, EventArgs e)
        {
            if (editar)
                btnsave.Visible = lblsave.Visible = getCambios();
            finalizar();
        }
        void finalizar()
        {
            var res = !(btnHDConductor.Visible = lblconductor.Visible = btnHDSupervisor.Visible = lblsupervisor.Visible = pFinalizar.Visible = (statusFinalizado == 0 && cbxgeteco.SelectedIndex > 0 && idconductor > 0 && cbxgetServicio.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetPlaceAccident.Text.Trim()) && cbxgetdireccion.SelectedIndex > 0 && cbxgetestacion1.SelectedIndex > 0 && cbxgetestacion2.SelectedIndex > 0 && cbxgetestacion3.SelectedIndex > 0 && cbxgetestacion4.SelectedIndex > 0 && cbxgetecorecup.SelectedIndex > 0 && cbxgetestacion.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetSintesis.Text) && (imagenes[0].Count > 0 || imagenes[1].Count > 0 || imagenes[2].Count > 0 || imagenes[3].Count > 0) && !string.IsNullOrWhiteSpace(txtgetDescripcion.Text.Trim()) && Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetNumSeguro.Text) ? txtgetNumSeguro.Text.Trim() : "0")) > 0 && !string.IsNullOrWhiteSpace(txtgetajustadorname.Text.Trim()) && !string.IsNullOrWhiteSpace(txtgetEXTsolucion.Text.Trim()) && Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetEXTnumActa.Text) ? txtgetEXTnumActa.Text.Trim() : "0")) > 0 && cbxgetEXTsupervisor.SelectedIndex > 0 && (!string.IsNullOrWhiteSpace(binarizedPDF) || pdfFOREDIT != null)));

        }
        private void buttonNuevoOC_Click(object sender, EventArgs e)
        {
            if (editar && getCambios())
            {
                var res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsave_Click(null, e);
                    limpiar();
                    if (!est_expor) initializeReports(null);
                }
                else if (res == DialogResult.No)
                {
                    limpiar();
                    if (!est_expor) initializeReports(null);
                }
            }
            else
            {
                limpiar();
                if (!est_expor) initializeReports(null);
            }
        }
        private void percances_FormClosing(object sender, FormClosingEventArgs e) { foreach (string path in Paths) { try { File.Delete(path); } catch { } } }
        void initializeCompanies() { v.iniCombos("SELECT idempresa as id,UPPER(nombreEmpresa) as nombre FROM cempresas WHERE status=1 ORDER BY nombreEmpresa ASC", cbxgetempresa, "id", "nombre", "-- SELECCIONE UNA EMPRESA --"); }
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (conductorFinalizar > 0 && supervisorFinalizar > 0)
                {
                    yaAparecioMensaje = true;
                    var ress = true;
                    if (!editar)
                        ress = insertar();
                    else
                        if (getCambios())
                        ress = actualizar();
                    if (ress)
                    {
                        object usuariofinalizo = null;
                        FormContraFinal fc = new FormContraFinal(empresa: 1, area: 1, F: this, v: v);
                        fc.LabelTitulo.Text = "Introduzca Su Contraseña Para Finalizar\nEl Reporte ";
                        if (fc.ShowDialog() == DialogResult.OK)
                        {
                            usuariofinalizo = Convert.ToInt32(fc.id);
                            if (v.c.insertar(string.Format("UPDATE reportepercance SET finalizado=1, firmaconductorfkcpersonal='{0}', firmasupervisorfkcpersonal='{1}',usuarioFinalizofkcpersonal='{2}' WHERE idreportePercance = '" + (idReporteTemp > 0 ? idReporteTemp : v.getaData("SELECT idreportepercance FROM reportepercance WHERE consecutivo = '" + lblFolio.Text.Substring(4) + "'")) + "'", new object[] { conductorFinalizar, supervisorFinalizar, usuariofinalizo })))
                                if (!est_expor && pconsultar) initializeReports("WHERE DATE_FORMAT(t1.fechaHoraAccidente, '%m') = " + DateTime.Today.ToString("MM"));
                            limpiar();
                        }
                    }
                }
                else
                {
                    if (conductorFinalizar == 0) MessageBox.Show("Error:\nSe Requiere Huella del Conductor", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else if (supervisorFinalizar == 0) MessageBox.Show("Error:\nSe Requiere Huella de Supervisor", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { gbxFechas.Enabled = checkBox1.Checked; cbmes.Enabled = !checkBox1.Checked; if (checkBox1.Checked) cbmes.SelectedIndex = 0; }
        private void dgvpercances_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvpercances.Columns[e.ColumnIndex].Name == "status")
            {
                if (Convert.ToString(e.Value) == "EN PROCESO")
                    e.CellStyle.BackColor = Color.Khaki;
                else if (Convert.ToString(e.Value) == "FINALIZADO")
                    e.CellStyle.BackColor = Color.PaleGreen;
            }
        }
        private void cbxgetecorecup_Validated(object sender, EventArgs e)
        {
            if (cbxgetecorecup.SelectedIndex > 0 && cbxgeteco.SelectedIndex > 0 && cbxgeteco.SelectedValue.ToString().Equals(cbxgetecorecup.SelectedValue.ToString()))
            {
                MessageBox.Show(v.mayusculas("Error:\nEl Econóimico ya ha sido ingresado en el reporte".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                (sender as ComboBox).SelectedIndex = 0;
            }
        }
        private void cbxgetestacion_Validated(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex > 0)
            {
                if (cbxgetestacion1 != sender as ComboBox && cbxgetestacion1.SelectedIndex > 0 && cbxgetestacion1.SelectedValue.ToString().Equals((sender as ComboBox).SelectedValue.ToString())) { MessageBox.Show(v.mayusculas("Error:\nLa Estación ya ha sido ingresada en el reporte".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); (sender as ComboBox).SelectedIndex = 0; }
                else if (cbxgetestacion2 != sender as ComboBox && cbxgetestacion2.SelectedIndex > 0 && cbxgetestacion2.SelectedValue.ToString().Equals((sender as ComboBox).SelectedValue.ToString())) { MessageBox.Show(v.mayusculas("Error:\nLa Estación ya ha sido ingresada en el reporte".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); (sender as ComboBox).SelectedIndex = 0; }
                else if (cbxgetestacion3 != sender as ComboBox && cbxgetestacion3.SelectedIndex > 0 && cbxgetestacion3.SelectedValue.ToString().Equals((sender as ComboBox).SelectedValue.ToString())) { MessageBox.Show(v.mayusculas("Error:\nLa Estación ya ha sido ingresada en el reporte".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); (sender as ComboBox).SelectedIndex = 0; }
                else if (cbxgetestacion4 != sender as ComboBox && cbxgetestacion4.SelectedIndex > 0 && cbxgetestacion4.SelectedValue.ToString().Equals((sender as ComboBox).SelectedValue.ToString())) { MessageBox.Show(v.mayusculas("Error:\nLa Estación ya ha sido ingresada en el reporte".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); (sender as ComboBox).SelectedIndex = 0; }
                else if (cbxgetestacion != sender as ComboBox && cbxgetestacion.SelectedIndex > 0 && cbxgetestacion.SelectedValue.ToString().Equals((sender as ComboBox).SelectedValue.ToString())) { MessageBox.Show(v.mayusculas("Error:\nLa Estación ya ha sido ingresada en el reporte".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); (sender as ComboBox).SelectedIndex = 0; }
            }
        }
        private void cbxgetecorecup_SelectedIndexChanged(object sender, EventArgs e) { }
        private void DTPgetTIMEaccident_Validated(object sender, EventArgs e)
        {
            if (DTPgetDate.Value.ToString("dd - MM - yyyy").Equals(DateTime.Today.ToString("dd - MM - yyyy")))
            {
                if (DateTime.Parse((sender as DateTimePicker).Value.ToString("HH:mm")) > DateTime.Parse(DateTime.Now.ToString("HH:mm")))
                {
                    MessageBox.Show("La Hora Ingresada No Puede Ser Mayor a La Hora Actual", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    (sender as DateTimePicker).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                }
            }
        }
        private void p1_Paint(object sender, PaintEventArgs e) { }
        void ExportarExcel()
        {
            if (dgvpercances.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < dgvpercances.Columns.Count; i++) if (dgvpercances.Columns[i].Visible) dt.Columns.Add(dgvpercances.Columns[i].HeaderText);
                for (int j = 0; j < dgvpercances.Rows.Count; j++)
                {

                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < dgvpercances.Columns.Count; i++)
                    {
                        if (dgvpercances.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = dgvpercances.Rows[j].Cells[i].Value;
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
            }
            else { MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        bool est_expor, inhabilitar;
        private void btnExcel_Click(object sender, EventArgs e)
        {
            est_expor = true;
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        }
        void esta_exportando()
        {
            if (!LblExcel.Text.Equals("Exportando"))
                btnExcel.Visible = LblExcel.Visible = false;
            else
                est_expor = true;
        }
        delegate void El_Delegado();
        Thread exportar;
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "Exportando";
        }

        private void btnHDSupervisor_Click(object sender, EventArgs e)
        {
            if (cbxgetEXTsupervisor.SelectedIndex > 0 || supervisorAsistenciaPercanceAnterior > 0)
            {
                readFingerprint rd = new readFingerprint(" WHERE t2.status=1", (cbxgetEXTsupervisor.SelectedIndex > 0 ? Convert.ToInt32(cbxgetEXTsupervisor.SelectedValue) : supervisorAsistenciaPercanceAnterior), "Supervisor", v);
                rd.Owner = this;
                var res = rd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    supervisorFinalizar = Convert.ToInt32(rd.idPersona);
                    lblsupervisor.Text = v.getaData("SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona ='" + supervisorFinalizar + "'").ToString();
                }
            }
            else
                MessageBox.Show("Error\nSeleccione un Supervisor de La Lista Desplegable Para Continuar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        delegate void El_Delegado1();
        private void percances_Load(object sender, EventArgs e)
        {
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
            panel1.Enabled = panel2.Enabled = panel3.Enabled = panel4.Enabled = false;
        }
        private void p2_Paint(object sender, PaintEventArgs e) { }

        private void cbxgetEXTsupervisor_SelectedIndexChanged(object sender, EventArgs e) { lblsupervisor.Text = "Firma Supervisor"; }

        private void DTPgetTimeSeguro_Validating(object sender, CancelEventArgs e)
        {
            var res = false;
            if (DTPgetDate.Value.ToString("dd - MM - yyyy").Equals(DateTime.Today.ToString("dd - MM - yyyy")))
            {
                if (DateTime.Parse((sender as DateTimePicker).Value.ToString("HH:mm")) > DateTime.Parse(DateTime.Now.ToString("HH:mm")))
                {
                    res = true;
                    MessageBox.Show("La Hora Ingresada No Puede Ser Mayor a La Hora Actual", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    (sender as DateTimePicker).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                }
                else if (DateTime.Parse(DTPgetTimeSeguro.Value.ToString("HH:mm")) < DateTime.Parse(DTPgetTIMEaccident.Value.ToString("HH:mm")))
                {
                    if (!res)
                    {
                        res = true;
                        MessageBox.Show("La Hora Ingresada No Puede Ser Menor a la Hora Aproximada del Accidente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        (sender as DateTimePicker).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    }
                }
            }
        }

        private void DTPgetplaceTIME_Validating(object sender, CancelEventArgs e)
        {
            var res = false;
            if (DTPgetDate.Value.ToString("dd - MM - yyyy").Equals(DateTime.Today.ToString("dd - MM - yyyy")))
            {
                if (DateTime.Parse((sender as DateTimePicker).Value.ToString("HH:mm")) > DateTime.Parse(DateTime.Now.ToString("HH:mm")))
                {
                    res = true;
                    MessageBox.Show("La Hora Ingresada No Puede Ser Mayor a La Hora Actual", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    (sender as DateTimePicker).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                }

            }
            if (DateTime.Parse(DTPgetplaceTIME.Value.ToString("HH:mm")) < DateTime.Parse(DTPgetTIMEaccident.Value.ToString("HH:mm")))
            {
                if (!res)
                {
                    res = true;
                    MessageBox.Show("La Hora Ingresada No Puede Ser Menor a la Hora Aproximada del Accidente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    (sender as DateTimePicker).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                }
            }
            else if (DateTime.Parse(DTPgetplaceTIME.Value.ToString("HH:mm")) < DateTime.Parse(DTPgetTimeSeguro.Value.ToString("HH:mm")))
            {
                if (!res)
                {
                    res = true;
                    MessageBox.Show("La Hora Ingresada No Puede Ser Menor a la Hora En Que Se Reporta Al Seguro", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    (sender as DateTimePicker).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                }
            }
        }

        private void cbxgeteco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxgeteco.SelectedIndex > 0 && cbxgeteco.DataSource != null)
            {
                pbdibujar.BackgroundImage = null;
                panel1.Enabled = panel2.Enabled = panel3.Enabled = panel4.Enabled = true;
                string modelo = v.getaData("select modelofkcmodelos from cunidades where idunidad='" + cbxgeteco.SelectedValue + "';").ToString();
                if (Convert.ToInt32(modelo) == 1)
                {
                    panel1.BackgroundImage = controlFallos.Properties.Resources.FRENTE;
                    panel2.BackgroundImage = controlFallos.Properties.Resources.LATERAL_DERECHO;
                    panel3.BackgroundImage = controlFallos.Properties.Resources.LATERAL_IZQUIERDO;
                    panel4.BackgroundImage = controlFallos.Properties.Resources.TRASERO;
                }
                else
                {
                    panel1.BackgroundImage = controlFallos.Properties.Resources.t_frente;
                    panel2.BackgroundImage = controlFallos.Properties.Resources.t_derecho;
                    panel3.BackgroundImage = controlFallos.Properties.Resources.t_izquierdo;
                    panel4.BackgroundImage = controlFallos.Properties.Resources.t_trasero;
                }
            }
            else
            {
                panel1.Enabled = panel2.Enabled = panel3.Enabled = panel4.Enabled = false;
                panel1.BackgroundImage = panel2.BackgroundImage = panel3.BackgroundImage = panel4.BackgroundImage = null;
            }
        }
        private void btnHDConductor_Click(object sender, EventArgs e)
        {
            if (idconductorAnterior > 0 || idconductor > 0)
            {
                readFingerprint rd = new readFingerprint(" WHERE t2.status=1", (idconductor > 0 ? idconductor : idconductorAnterior), "Conductor", v);
                rd.Owner = this;
                var res = rd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    conductorFinalizar = Convert.ToInt32(rd.idPersona);
                    lblconductor.Text = v.getaData("SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona ='" + conductorFinalizar + "'").ToString();
                }
            }
            else
                MessageBox.Show("Error\nIngrese La Credencial Del Conductor Para Continuar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int posicioninicial = trazosimagenActual[trazosimagenActual.Count - 2], posicionFinal = trazosimagenActual[trazosimagenActual.Count - 1];
            trazosimagenActual.RemoveAt(trazosimagenActual.Count - 1);
            trazosimagenActual.RemoveAt(trazosimagenActual.Count - 1);
            for (int i = posicionFinal - 1; i >= posicioninicial; i--)
                imagenes[imagenActual].RemoveAt(posicioninicial);
            pDeshacer.Visible = trazosimagenActual.Count > 0;
            dibujarPuntitos();
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            pEvidencias.Visible = !(p2.Visible = false);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            p2.Visible = !(pEvidencias.Visible = false);
        }

        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            if (inhabilitar) { btnExcel.Visible = LblExcel.Visible = false; inhabilitar = false; }
            est_expor = false;
            if (pconsultar) initializeReports("WHERE DATE_FORMAT(t1.fechaHoraAccidente, '%m') = " + DateTime.Today.ToString("MM"));
            LblExcel.Text = "Exportar";
        }
        private void button6_Click(object sender, EventArgs e) { if (!est_expor) initializeReports("WHERE DATE_FORMAT(t1.fechaHoraAccidente, '%m') = " + DateTime.Today.ToString("MM")); pActualizar.Visible = false; if (!est_expor) btnExcel.Visible = LblExcel.Visible = false; else inhabilitar = true; }
        private void button7_Click(object sender, EventArgs e)
        {
            if (cbxgetexoBusq.SelectedIndex > 0 || cbxgetconductor.SelectedIndex > 0 || checkBox1.Checked || cbmes.SelectedIndex > 0)
            {
                bool res = true;
                if (checkBox1.Checked)
                {
                    if (DateTime.Parse(dtpFechaA.Value.ToString("yyyy-MM-dd")) < DateTime.Parse(dtpFechaDe.Value.ToString("yyyy-MM-dd")))
                    {
                        checkBox1.Enabled = res = false;
                        MessageBox.Show("Error: \n Fechas Incorrectas", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dtpFechaA.Value = dtpFechaDe.Value = DateTime.Today;
                    }
                }
                if (res)
                {
                    string wheres = "";
                    if (cbxgetexoBusq.SelectedIndex > 0)
                    {
                        if (string.IsNullOrWhiteSpace(wheres)) wheres += " WHERE ";
                        else wheres += " AND ";
                        wheres += "t1.ecofkcunidades='" + cbxgetexoBusq.SelectedValue + "'";
                    }
                    if (cbxgetconductor.SelectedIndex > 0)
                    {
                        if (string.IsNullOrWhiteSpace(wheres)) wheres += " WHERE ";
                        else wheres += " AND ";
                        wheres += "t1.conductorfkcpersonal='" + cbxgetconductor.SelectedValue + "'";
                    }
                    if (cbmes.SelectedIndex > 0)
                    {
                        if (string.IsNullOrWhiteSpace(wheres)) wheres += " WHERE ";
                        else wheres += " AND ";
                        wheres += "DATE_FORMAT(t1.fechaHoraAccidente,'%m')=" + cbmes.SelectedValue;
                    }
                    if (checkBox1.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(wheres)) wheres += " WHERE ";
                        else wheres += " AND ";
                        wheres += " DATE(fechaHoraAccidente)  BETWEEN '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'";
                    }
                    cbmes.SelectedIndex = cbxgetexoBusq.SelectedIndex = cbxgetconductor.SelectedIndex = 0;
                    checkBox1.Checked = false;
                    dtpFechaA.Value = dtpFechaDe.Value = DateTime.Today;
                    try { initializeReports(wheres); } catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    if (dgvpercances.Rows.Count == 0)
                    {
                        MessageBox.Show("No Se Encontraron Resultados Con Los Criterios Seleccionados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        if (!est_expor) initializeReports("WHERE DATE_FORMAT(t1.fechaHoraAccidente, '%m') = " + DateTime.Today.ToString("MM"));
                    }
                    else
                        btnExcel.Visible = LblExcel.Visible = pActualizar.Visible = true;
                }
            }
            else
                MessageBox.Show("Seleccione un Criterio de Busqueda", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void cbxgetServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxgetServicio.SelectedIndex > 0)
            {
                v.iniCombos(string.Format("SELECT idestacion as id, UPPER(estacion) AS estacion FROM relacservicioestacion as t1 INNER JOIN cestaciones as t2 ON t1.estacionfkcestaciones = t2.idestacion WHERE t1.serviciofkcservicios='{0}' and t2.status=1", cbxgetServicio.SelectedValue), cbxgetestacion1, "id", "estacion", "--SELECCIONE UN SERVICIO--");
                v.iniCombos(string.Format("SELECT idestacion as id, UPPER(estacion) AS estacion FROM relacservicioestacion as t1 INNER JOIN cestaciones as t2 ON t1.estacionfkcestaciones = t2.idestacion WHERE t1.serviciofkcservicios='{0}' and t2.status=1", cbxgetServicio.SelectedValue), cbxgetestacion2, "id", "estacion", "--SELECCIONE UNA ESTACIÓN--");
                v.iniCombos(string.Format("SELECT idestacion as id, UPPER(estacion) AS estacion FROM relacservicioestacion as t1 INNER JOIN cestaciones as t2 ON t1.estacionfkcestaciones = t2.idestacion WHERE t1.serviciofkcservicios='{0}' and t2.status=1", cbxgetServicio.SelectedValue), cbxgetestacion3, "id", "estacion", "--SELECCIONE UNA ESTACIÓN--");
                v.iniCombos(string.Format("SELECT idestacion as id, UPPER(estacion) AS estacion FROM relacservicioestacion as t1 INNER JOIN cestaciones as t2 ON t1.estacionfkcestaciones = t2.idestacion WHERE t1.serviciofkcservicios='{0}' and t2.status=1", cbxgetServicio.SelectedValue), cbxgetestacion4, "id", "estacion", "--SELECCIONE UNA ESTACIÓN--");
                v.iniCombos(string.Format("SELECT idestacion as id, UPPER(estacion) AS estacion FROM relacservicioestacion as t1 INNER JOIN cestaciones as t2 ON t1.estacionfkcestaciones = t2.idestacion WHERE t1.serviciofkcservicios='{0}' and t2.status=1", cbxgetServicio.SelectedValue), cbxgetestacion, "id", "estacion", "--SELECCIONE UNA ESTACIÓN--");
                cbxgetestacion1.Enabled = cbxgetestacion2.Enabled = cbxgetestacion3.Enabled = cbxgetestacion4.Enabled = cbxgetestacion.Enabled = true;
            }
            else
            {
                cbxgetestacion1.DataSource = cbxgetestacion2.DataSource = cbxgetestacion3.DataSource = cbxgetestacion4.DataSource = cbxgetestacion.DataSource = null;
                cbxgetestacion1.Enabled = cbxgetestacion2.Enabled = cbxgetestacion3.Enabled = cbxgetestacion4.Enabled = false;
            }
        }
        public void resetPixels()
        {
            imagenes[0] = new List<Point>(); imagenesAnterior[0] = new List<Point>(); imagenes[1] = new List<Point>(); imagenesAnterior[1] = new List<Point>(); imagenes[2] = new List<Point>(); imagenesAnterior[2] = new List<Point>(); imagenes[3] = new List<Point>(); imagenesAnterior[3] = new List<Point>();
            panel1.BackgroundImage = panel2.BackgroundImage = panel3.BackgroundImage = panel4.BackgroundImage = pbdibujar.BackgroundImage = null;
            imagenActual = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            p2.Visible = !(p1.Visible = false);
            if (grphcs != null)
                grphcs.Dispose();
            pbdibujar.BackgroundImage = null;
            txtgetmarcaVThird.Focus();
            pFolio.BringToFront();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            p2.Visible = !(p1.Visible = true);
            dibujarPuntitos();
            pFolio.BringToFront();
            cbxgetempresa.Focus();
        }
        bool dibujarPuntitos()
        {

            Bitmap m = null;
            switch (imagenActual)
            {
                case 0:
                    m = new Bitmap(panel1.BackgroundImage);
                    break;
                case 1:
                    m = new Bitmap(panel2.BackgroundImage);
                    break;
                case 2:
                    m = new Bitmap(panel3.BackgroundImage);
                    break;
                case 3:
                    m = new Bitmap(panel4.BackgroundImage);
                    break;
            }
            pbdibujar.BackgroundImage = m;
            Point[] coordenadas = imagenes[imagenActual].ToArray();
            if (imagenes[imagenActual].Count > 0)
            {
                Pen blackPen = new Pen(Color.Blue, 2);
                for (int i = 0; i < imagenes[imagenActual].Count; i++)
                {
                    using (Graphics gr = Graphics.FromImage(m))
                    {
                        if (coordenadas.Length == 1)
                        {
                            var r = new System.Drawing.Rectangle(new Point(coordenadas[i].X, coordenadas[i].Y), new Size(2, 2));
                            gr.DrawRectangle(blackPen, r);
                        }
                        else
                        {
                            if (coordenadas[i].X == -1 || coordenadas[i].Y == -1 || i == coordenadas.Length - 1) { }
                            else
                            {
                                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                gr.DrawLine(blackPen, coordenadas[i].X, coordenadas[i].Y, (coordenadas[i + 1].X == -1 ? coordenadas[i].X : coordenadas[i + 1].X), (coordenadas[i + 1].Y == -1 ? coordenadas[i].Y : coordenadas[i + 1].Y));
                            }
                        }
                    }

                }
                pbdibujar.BackgroundImage = m;
                pbdibujar.Size = pbdibujar.BackgroundImage.Size;
                pbdibujar.Top = (pContains.Height - pbdibujar.Height) / 2;
                pbdibujar.Left = (pContains.Width - pbdibujar.Width) / 2;
                return true;
            }

            else return false;
        }
        private void panel1_MouseHover(object sender, EventArgs e) { Control p = sender as Control; p.Size = new Size(105, 105); }
        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            Control p = sender as Control;
            p.Size = new Size(100, 100);
        }
        private void txtgetcredencial_TextChanged(object sender, EventArgs e)
        {
            idconductor = 0;
            lblshowconductor.Text = null;
            lblconductor.Text = "Firma Conductor";
            object res = v.getaData(string.Format("SELECT CONCAT(idpersona,';',nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE credencial ='{0}' AND empresa='1' AND area='1'", txtgetcredencial.Text.Trim()));
            if (res != null)
            {
                string[] ress = res.ToString().Split(';');
                idconductor = Convert.ToInt32(ress[0]);
                lblshowconductor.Text = ress[1];
            }
            else
            {
                idconductor = 0;
                lblshowconductor.Text = null;
            }
            getCambios(null, e);
        }
        void initializeDirections()
        {
            ComboBox cbx = cbxgetdireccion;
            cbx.DataSource = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("iddireccion");
            dt.Columns.Add("direccion");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["iddireccion"] = 0;
            nuevaFila["direccion"] = "--SELECCIONE DIRECCIÓN--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            nuevaFila = dt.NewRow();
            nuevaFila["iddireccion"] = 1;
            nuevaFila["direccion"] = "NORTE".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 1);
            nuevaFila = dt.NewRow();
            nuevaFila["iddireccion"] = 2;
            nuevaFila["direccion"] = "SUR".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 2);
            cbx.ValueMember = "iddireccion";
            cbx.DisplayMember = "direccion";
            cbx.DataSource = dt;
        }
        int imagenActual;
        private void panel1_Click(object sender, EventArgs e)
        {
            switch (((Panel)sender).Name)
            {
                case "panel1":
                    imagenActual = 0;
                    break;
                case "panel2":
                    imagenActual = 1;
                    break;
                case "panel3":
                    imagenActual = 2;
                    break;
                case "panel4":
                    imagenActual = 3;
                    break;
            }
            if (!dibujarPuntitos())
                pbdibujar.BackgroundImage = ((Panel)sender).BackgroundImage;
            pbdibujar.Size = pbdibujar.BackgroundImage.Size;
            pbdibujar.Top = (pContains.Height - pbdibujar.Height) / 2;
            pbdibujar.Left = (pContains.Width - pbdibujar.Width) / 2;
        }
        bool drawing;
        Graphics grphcs;
        private void pbdibujar_MouseDown(object sender, MouseEventArgs e)
        {
            //imagenes[imagenActual].Add(new Point(e.X, e.Y));
            drawing = true; trazosimagenActual.Add(imagenes[imagenActual].Count);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(binarizedPDF) /*&& dibujoExportado == 0*/)
            {
                Document dc = new Document(PageSize.LETTER);
                dc.SetMargins(21f, 21f, 31f, 31f);
                float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f };
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"C:\Desktop";
                saveFileDialog1.Title = "Guardar reporte";
                saveFileDialog1.DefaultExt = "pdf";
                saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                string filename = "";
                DialogResult ews = DialogResult.OK;
                try
                {
                    if ((ews = saveFileDialog1.ShowDialog(this)) == DialogResult.OK)
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
                            Paragraph saltoDeLinea1 = new Paragraph(Environment.NewLine, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL));
                            dc.Add(saltoDeLinea1);
                            byte[] img = Convert.FromBase64String(v.transmasivo);
                            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                            imagen.ScalePercent(12f);
                            imagen.SetAbsolutePosition(50f, 687f);
                            imagen.Alignment = Element.ALIGN_LEFT;
                            dc.Add(imagen);
                            string[] res = (idReporteTemp > 0 ? v.getaData(string.Format("SET lc_time_names='es_ES';SELECT CONCAT(CONCAT('RP-', LPAD(t1.consecutivo, 7, 0)),'|',DATE_FORMAT(fechaHoraAccidente, '%d - %M - %y')) FROM reportepercance AS t1 INNER JOIN cunidades AS t2 ON t1.ecofkcunidades = t2.idunidad INNER JOIN careas AS t3 ON t2.areafkcareas = t3.idarea INNER JOIN cempresas AS t4 ON t3.empresafkcempresas = t4.idempresa WHERE idreportePercance ='{0}'", idReporteTemp)).ToString().Split('|') : new string[] { lblFolio.Text, DTPgetDate.Value.ToString("dd - MM - yyyy") });
                            for (int i = 0; i < 4; i++) dc.Add(saltoDeLinea1);
                            PdfPTable tbpercance = new PdfPTable(3);
                            tbpercance.WidthPercentage = 95;
                            tbpercance.AddCell(valorCampo("Folio: " + res[0], 1, Element.ALIGN_LEFT, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            tbpercance.AddCell(valorCampo("Fecha del Percance: ", 1, 2, 0, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                            tbpercance.AddCell(valorCampo(v.mayusculas(res[1]), 1, 1, 1, FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                            dc.Add(tbpercance);
                            dc.Add(saltoDeLinea1);
                            PdfPTable tbcontent7 = new PdfPTable(1);
                            tbcontent7.WidthPercentage = 95;
                            tbcontent7.AddCell(valorCampo("Realiza un Dibujo del Suceso: ", 1, 1, 0, FontFactory.GetFont("CALIBRI", 14, iTextSharp.text.Font.BOLD)));
                            dc.Add(tbcontent7);
                            dc.AddCreationDate();
                            dc.Close();
                            Process.Start(filename);
                            dibujoExportado = 1;
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
                MessageBox.Show("Error:\n Dibujo Ya Exportado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        PdfPCell valorCampo(string valor, int Colspan, int HorizontalAligment, int BorderBottom, iTextSharp.text.Font f)
        {
            PdfPCell temp = new PdfPCell(new Phrase(valor, f));
            temp.Colspan = Colspan;
            temp.HorizontalAlignment = HorizontalAligment;
            temp.BorderWidthLeft = 0;
            temp.BorderWidthRight = 0;
            temp.BorderWidthTop = 0;
            temp.BorderWidthBottom = BorderBottom;
            return temp;
        }
        PdfPCell valorCampo(string valor, int Colspan, int HorizontalAligment, int BorderBottom, iTextSharp.text.Font f, int Rowspan)
        {
            PdfPCell temp = new PdfPCell(new Phrase(valor, f));
            temp.Colspan = Colspan;
            temp.HorizontalAlignment = HorizontalAligment;
            temp.BorderWidthLeft = 0;
            temp.BorderWidthRight = 0;
            temp.BorderWidthTop = 1;
            temp.BorderWidthBottom = BorderBottom;
            temp.Rowspan = Rowspan;
            return temp;
        }
        private void cgetempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxgetempresa.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idarea as id, UPPER(nombreArea) as nombre FROM careas WHERE empresafkcempresas='" + cbxgetempresa.SelectedValue + "' AND status=1 ORDER BY nombreArea ASC", cbxgetarea, "id", "nombre", "-- SELECCIONE UN ÁREA --");
                cbxgetarea.Enabled = true;
            }
            else { cbxgetarea.DataSource = null; cbxgetarea.Enabled = false; }
        }
        private void pbdibujar_MouseMove(object sender, MouseEventArgs e)
        {
            pDeshacer.Visible = trazosimagenActual.Count > 0;
            if (drawing)
            {
                if (pbdibujar.BackgroundImage != null && imagenesAnterior[0].Count == 0 && imagenesAnterior[1].Count == 0 && imagenesAnterior[2].Count == 0 && imagenesAnterior[3].Count == 0 && (editar ? idReporteTemp > 0 : true))
                {
                    var r = new System.Drawing.Rectangle(new Point(e.X, e.Y), new Size(2, 2));
                    grphcs = pbdibujar.CreateGraphics();
                    grphcs.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    grphcs.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    Pen lapiz = new Pen(Color.Blue, 2);
                    imagenes[imagenActual].Add(new Point(e.X + 2, e.Y + 2));
                    Point[] puntos = imagenes[imagenActual].ToArray();
                    if (imagenes[imagenActual].Count == 1)
                        grphcs.DrawRectangle(lapiz, r);
                    else
                    {
                        if (puntos[puntos.Length - 2].X != -1)
                            grphcs.DrawLine(lapiz, puntos[puntos.Length - 2].X, puntos[puntos.Length - 2].Y, e.X, e.Y);
                    }
                    grphcs.Dispose();
                    getCambios(null, e);
                }
            }
        }
        private void pbdibujar_MouseUp(object sender, MouseEventArgs e)
        {
            imagenes[imagenActual].Add(new Point(-1, -1));
            drawing = false; trazosimagenActual.Add(imagenes[imagenActual].Count);
        }
        private void cgetare_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxgetarea.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1 AND t2.idarea='" + cbxgetarea.SelectedValue + "'", cbxgeteco, "idunidad", "eco", "--SELECCIONE UN ECONÓMICO--");
                cbxgeteco.Enabled = true;
                v.iniCombos(string.Format("SELECT idservicio,UPPER(nombre) as nombre FROM cservicios WHERE status=1 AND  areafkcareas='{0}'", cbxgetarea.SelectedValue), cbxgetServicio, "idservicio", "nombre", "--SELECCIONE UN SERVICIO--");
                cbxgetServicio.Enabled = true;
                v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1 AND t2.idarea='" + cbxgetarea.SelectedValue + "'", cbxgetecorecup, "idunidad", "eco", "--SELECCIONE UN ECONÓMICO--");
                cbxgetecorecup.Enabled = true;
            }
            else
            {
                cbxgeteco.DataSource = null;
                cbxgeteco.Enabled = false;
                cbxgetServicio.DataSource = null;
                cbxgetecorecup.DataSource = null;
                cbxgetServicio.Enabled = cbxgetestacion.Enabled = cbxgetecorecup.Enabled = false;
            }
        }
        private void onlyNumbers_KeyPress(object sender, KeyPressEventArgs e) { v.Solonumeros(e); }
        private void General_KeyPress(object sender, KeyPressEventArgs e) { v.enGeneral(e); }
        private void onlyLetters_KeyPress(object sender, KeyPressEventArgs e) { v.Sololetras(e); }
        private void LettersNumbersDash_KeyPress(object sender, KeyPressEventArgs e) { v.letrasNumerosGuiones(e); }
        private void White(object sender, CancelEventArgs e) { v.espaciosenblanco(sender, e); }
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                var res = false;
                if (!editar)
                    res = insertar();
                else
                    res = actualizar();
                if (res) { limpiar(); if (!est_expor) initializeReports("WHERE DATE_FORMAT(t1.fechaHoraAccidente, '%m') = " + DateTime.Today.ToString("MM")); }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }
        bool actualizar()
        {
            var resA = false;
            if (getCambios())
            {
                object economico = cbxgeteco.SelectedValue;
                if (!v.camposVaciosPercances(cbxgetempresa.SelectedIndex, cbxgetarea.SelectedIndex, cbxgeteco.SelectedIndex, idconductor, txtgetcredencial.Text.Trim()) && v.camposVaciosServiciosconMensaje(cbxgetestacion1.SelectedIndex, cbxgetestacion2.SelectedIndex, cbxgetestacion3.SelectedIndex, cbxgetestacion4.SelectedIndex, cbxgetecorecup.SelectedIndex, cbxgetestacion.SelectedIndex) && v.camposVacios_DatosTercero_conMensajes(txtgetmarcaVThird.Text.Trim(), txtgetyearVThird.Text.Trim(), txtgetplacasVThird.Text.Trim(), txtgetNameCThird.Text.Trim(), txtgetphoneCThird.Text.Trim(), txtgetaddressCThird.Text.Trim()) && v.camposVacios_SeguroUnidadTransmasivo_conMensajes(txtgetNumSeguro.Text.Trim(), txtgetajustadorname.Text.Trim()) && v.camposvacios_DatosExtra_conMensajes(txtgetEXTsolucion.Text.Trim(), txtgetEXTnumActa.Text, cbxgetEXTsupervisor.SelectedIndex) && v.camposvacios_AsistenciaMedica_conMensajes(txtgetMedicoUnidad.Text.Trim(), txtgetMedico.Text.Trim(), txtgetNameMedico.Text.Trim(), txtgetMedicalesionados.Text.Trim()) && !v.existeNumActa_sNumSeguroActualizar(numSeguroTransmasivoAnterior.ToString(), txtgetNumSeguro.Text.Trim(), numActaExtraAnterior.ToString(), txtgetEXTnumActa.Text.Trim()))
                {
                    DialogResult res = DialogResult.OK;
                    bool motivo = false;
                    if (v.mostrarMotivoEdicion(new string[,] { { economicoAnterior.ToString(), cbxgeteco.SelectedValue.ToString() }, { idconductorAnterior.ToString(), idconductor.ToString() }, { fechaAccidenteAnterior.ToString("yyyy-MM-dd"), DTPgetDate.Value.ToString("yyyy-MM-dd") }, { horaAccidenteAnterior.ToString("HH:mm"), DTPgetTIMEaccident.Value.ToString("HH:mm") }, { (servicioenLaborAnterior > 0 ? servicioenLaborAnterior.ToString() : null), (Convert.ToInt32(cbxgetServicio.SelectedValue) > 0 ? cbxgetServicio.SelectedValue.ToString() : null) }, { lugarAccidenteAnterior, v.mayusculas(txtgetPlaceAccident.Text.Trim().ToLower()) }, { (direccionAnterior > 0 ? direccionAnterior.ToString() : null), (cbxgetdireccion.SelectedIndex > 0 ? cbxgetdireccion.SelectedValue.ToString() : null) }, { (estacion1 > 0 ? estacion1.ToString() : null), (cbxgetestacion1.SelectedIndex > 0 ? cbxgetestacion1.SelectedValue.ToString() : null) }, { (estacion2 > 0 ? estacion2.ToString() : null), (cbxgetestacion2.SelectedIndex > 0 ? cbxgetestacion2.SelectedValue.ToString() : null) }, { (estacion3 > 0 ? estacion3.ToString() : null), (cbxgetestacion3.SelectedIndex > 0 ? cbxgetestacion3.SelectedValue.ToString() : null) }, { (estacion4 > 0 ? estacion4.ToString() : null), (cbxgetestacion4.SelectedIndex > 0 ? cbxgetestacion4.SelectedValue.ToString() : null) }, { (economicoRecupAnterior > 0 ? economicoRecupAnterior.ToString() : null), (cbxgetecorecup.SelectedIndex > 0 ? cbxgetecorecup.SelectedValue.ToString() : null) }, { (estacionAnterior > 0 ? estacionAnterior.ToString() : null), (cbxgetestacion.SelectedIndex > 0 ? cbxgetestacion.SelectedValue.ToString() : null) }, { sintesisOcurridoAnterior, v.mayusculas(txtgetSintesis.Text.Trim().ToLower()) }, { (imagenesAnterior[0].Count > 0 ? imagenesAnterior[0].Count.ToString() : null), (imagenes[0].Count > 0 ? imagenes[0].Count.ToString() : null) }, { (imagenesAnterior[1].Count > 0 ? imagenesAnterior[1].Count.ToString() : null), (imagenes[1].Count > 0 ? imagenes[1].Count.ToString() : null) }, { (imagenesAnterior[2].Count > 0 ? imagenesAnterior[2].Count.ToString() : null), (imagenes[2].Count > 0 ? imagenes[2].Count.ToString() : null) }, { (imagenesAnterior[3].Count > 0 ? imagenesAnterior[3].Count.ToString() : null), (imagenes[3].Count > 0 ? imagenes[3].Count.ToString() : null) }, { descripcionAnterior, v.mayusculas(txtgetDescripcion.Text.Trim().ToLower()) }, { marcaTerceroAnterior, v.mayusculas(txtgetmarcaVThird.Text.Trim().ToLower()) }, { yearTerceroAnterior, txtgetyearVThird.Text }, { placasTerceroAnterior, v.mayusculas(txtgetplacasVThird.Text.Trim().ToLower()) }, { nombreCTerceroAnterior, v.mayusculas(txtgetNameCThird.Text.Trim().ToLower()) }, { telefonoTerceroAnterior, txtgetphoneCThird.Text.Trim() }, { domicilioTerceroAnterior, v.mayusculas(txtgetaddressCThird.Text.Trim().ToLower()) }, { (numSeguroTransmasivoAnterior > 0 ? numSeguroTransmasivoAnterior.ToString() : null), (Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetNumSeguro.Text.Trim()) ? txtgetNumSeguro.Text.Trim() : "0")) > 0 ? txtgetNumSeguro.Text.Trim() : null) }, { horaOtorgnumSegAnterior.ToString("HH:mm"), DTPgetTimeSeguro.Value.ToString("HH:mm") }, { horaajustLlegaSiniestro.ToString("HH:mm"), DTPgetplaceTIME.Value.ToString("HH:mm") }, { solucionAnterior, v.mayusculas(txtgetEXTsolucion.Text.Trim().ToLower()) }, { (numActaExtraAnterior > 0 ? numActaExtraAnterior.ToString() : null), (Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetEXTnumActa.Text.Trim()) ? txtgetEXTnumActa.Text.Trim() : "0")) > 0 ? txtgetEXTnumActa.Text.Trim() : null) }, { (supervisorAsistenciaPercanceAnterior > 0 ? supervisorAsistenciaPercanceAnterior.ToString() : null), (cbxgetEXTsupervisor.SelectedIndex > 0 ? cbxgetEXTsupervisor.SelectedValue.ToString() : null) }, { (unidadAsistenciaMedicaAnterior > 0 ? unidadAsistenciaMedicaAnterior.ToString() : null), (Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetMedicoUnidad.Text.Trim()) ? txtgetMedicoUnidad.Text.Trim() : "0")) > 0 ? txtgetMedicoUnidad.Text.Trim() : null) }, { pertenecienteaAsistenciaMedicaAnterior, v.mayusculas(txtgetMedico.Text.Trim().ToLower()) }, { responsabeUnidadAsistenciaMedicaAnterior, v.mayusculas(txtgetNameMedico.Text.Trim().ToLower()) }, { encasoLesionadosAnteriorAnterior, v.mayusculas(txtgetMedicalesionados.Text.Trim().ToLower()) }, { comentariosAnterior, v.mayusculas(txtgetComentarios.Text.Trim().ToLower()) } }))
                    {
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        res = obs.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower()); motivo = true; v.c.insertar(String.Format("INSERT INTO modificaciones_sistema (form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('Reporte de Percances','{0}','{1}','{2}',NOW(),'{3}','{4}','1','1');", new object[] { idReporteTemp, economicoAnterior + "|" + idconductorAnterior + "|" + fechaAccidenteAnterior.ToString("yyyy-MM-dd") + " " + horaAccidenteAnterior.ToString("HH:mm:ss") + "|" + servicioenLaborAnterior + "|" + lugarAccidenteAnterior + "|" + direccionAnterior + "|" + estacion1 + "|" + estacion2 + "|" + estacion3 + "|" + estacion4 + "|" + economicoRecupAnterior + "|" + estacionAnterior + "|" + sintesisOcurridoAnterior + "|" + descripcionAnterior + "|" + marcaTerceroAnterior + "|" + yearTerceroAnterior + "|" + placasTerceroAnterior + "|" + nombreCTerceroAnterior + "|" + telefonoTerceroAnterior + "|" + domicilioTerceroAnterior + "|" + numSeguroTransmasivoAnterior + "|" + horaOtorgnumSegAnterior.ToString("HH:mm") + "|" + horaajustLlegaSiniestro.ToString("HH:mm") + "|" + nombreAjustadorAnterior + "|" + solucionAnterior + "|" + numActaExtraAnterior + "|" + supervisorAsistenciaPercanceAnterior + "|" + unidadAsistenciaMedicaAnterior + "|" + pertenecienteaAsistenciaMedicaAnterior + "|" + responsabeUnidadAsistenciaMedicaAnterior + "|" + encasoLesionadosAnteriorAnterior + "|" + comentariosAnterior, idUsuario, "Actualización de Reporte de Percance", edicion }));
                        }
                    }
                    if (res == DialogResult.OK)
                    {
                        string sql = "UPDATE reportepercance SET {0} WHERE idreportepercance='" + idReporteTemp + "'", cambios = "";
                        if (economicoAnterior != Convert.ToInt32(cbxgeteco.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "ecofkcunidades= '" + cbxgeteco.SelectedValue + "'"; }
                        if (idconductorAnterior != idconductor) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "conductorfkcpersonal = '" + idconductor + "'"; }
                        if (!fechaAccidenteAnterior.ToString("yyyy-MM-dd").Equals(DTPgetDate.Value.ToString("yyyy-MM-dd")) || !horaAccidenteAnterior.ToString("HH:mm").Equals(DTPgetTIMEaccident.Value.ToString("HH:mm"))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "fechaHoraAccidente = '" + DTPgetDate.Value.ToString("yyyy-MM-dd ") + DTPgetTIMEaccident.Value.ToString("HH:mm:ss") + "'"; }
                        if (servicioenLaborAnterior != Convert.ToInt32(cbxgetServicio.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "servicioenlaborfkcservicios = " + (cbxgetServicio.SelectedIndex > 0 ? "'" + cbxgetServicio.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (!(lugarAccidenteAnterior ?? "").Equals(v.mayusculas(txtgetPlaceAccident.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "lugaraccidente = " + (!string.IsNullOrWhiteSpace(txtgetPlaceAccident.Text.Trim()) ? "'" + v.mayusculas(txtgetPlaceAccident.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (direccionAnterior != Convert.ToInt32(cbxgetdireccion.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "direccion= '" + cbxgetdireccion.SelectedValue + "'"; }
                        if (estacion1 != Convert.ToInt32(cbxgetestacion1.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "estacion1fkcestaciones = " + (cbxgetestacion1.SelectedIndex > 0 ? "'" + cbxgetestacion1.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (estacion2 != Convert.ToInt32(cbxgetestacion2.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "estacion2fkcestaciones = " + (cbxgetestacion2.SelectedIndex > 0 ? "'" + cbxgetestacion2.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (estacion3 != Convert.ToInt32(cbxgetestacion3.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "estacion3fkcestaciones = " + (cbxgetestacion3.SelectedIndex > 0 ? "'" + cbxgetestacion3.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (estacion4 != Convert.ToInt32(cbxgetestacion4.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "estacion4fkcestaciones = " + (cbxgetestacion4.SelectedIndex > 0 ? "'" + cbxgetestacion4.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (economicoRecupAnterior != Convert.ToInt32(cbxgetecorecup.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "ecorecuperacionfkcunidades = " + (cbxgetecorecup.SelectedIndex > 0 ? "'" + cbxgetecorecup.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (estacionAnterior != Convert.ToInt32(cbxgetestacion.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "estacionfkcestaciones = " + (cbxgetestacion.SelectedIndex > 0 ? "'" + cbxgetestacion.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (!(sintesisOcurridoAnterior ?? "").Equals(v.mayusculas(txtgetSintesis.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "sintesisocurrido = " + (!string.IsNullOrWhiteSpace(txtgetSintesis.Text.Trim()) ? "'" + v.mayusculas(txtgetSintesis.Text.Trim().ToLower()).Replace("\'", "") + "'" : "NULL"); }
                        if (imagenes[0].Count != imagenesAnterior[0].Count || imagenes[1].Count != imagenesAnterior[1].Count || imagenes[2].Count != imagenesAnterior[2].Count || imagenes[3].Count != imagenesAnterior[3].Count) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "coordenadasimagenes = '" + getCoordenadas4Imagenes() + "'"; }
                        if (!(descripcionAnterior ?? "").Equals(v.mayusculas(txtgetDescripcion.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "descripcion = " + (!string.IsNullOrWhiteSpace(txtgetDescripcion.Text.Trim()) ? "'" + v.mayusculas(txtgetDescripcion.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(marcaTerceroAnterior ?? "").Equals(v.mayusculas(txtgetmarcaVThird.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "marcavehiculotercero = " + (!string.IsNullOrWhiteSpace(txtgetmarcaVThird.Text.Trim()) ? "'" + v.mayusculas(txtgetmarcaVThird.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(yearTerceroAnterior ?? "").Equals(v.mayusculas(txtgetyearVThird.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "yearvehiculotercero = " + (!string.IsNullOrWhiteSpace(txtgetyearVThird.Text.Trim()) ? "'" + v.mayusculas(txtgetyearVThird.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(placasTerceroAnterior ?? "").Equals(v.mayusculas(txtgetplacasVThird.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "placasvehiculotercero = " + (!string.IsNullOrWhiteSpace(txtgetplacasVThird.Text.Trim()) ? "'" + v.mayusculas(txtgetplacasVThird.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(nombreCTerceroAnterior ?? "").Equals(v.mayusculas(txtgetNameCThird.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "nombreconductortercero = " + (!string.IsNullOrWhiteSpace(txtgetNameCThird.Text.Trim()) ? "'" + v.mayusculas(txtgetNameCThird.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(telefonoTerceroAnterior ?? "").Equals(v.mayusculas(txtgetphoneCThird.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "telefonoconductortercero = " + (!string.IsNullOrWhiteSpace(txtgetphoneCThird.Text.Trim()) ? "'" + v.mayusculas(txtgetphoneCThird.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(domicilioTerceroAnterior ?? "").Equals(v.mayusculas(txtgetaddressCThird.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "domicilioconductortercero = " + (!string.IsNullOrWhiteSpace(txtgetaddressCThird.Text.Trim()) ? "'" + v.mayusculas(txtgetaddressCThird.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (numSeguroTransmasivoAnterior != Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetNumSeguro.Text.Trim()) ? txtgetNumSeguro.Text.Trim() : "0"))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "numreporteseguro = " + (!string.IsNullOrWhiteSpace(txtgetNumSeguro.Text.Trim()) ? "'" + v.mayusculas(txtgetNumSeguro.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (horaOtorgnumSegAnterior.ToString("HH:mm") != DTPgetTimeSeguro.Value.ToString("HH:mm")) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "horaotorgamiento ='" + DTPgetTimeSeguro.Value.ToString("HH:mm:ss") + "'"; }
                        if (horaajustLlegaSiniestro.ToString("HH:mm") != DTPgetplaceTIME.Value.ToString("HH:mm")) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "horallegadaseguro = '" + DTPgetplaceTIME.Value.ToString("HH:mm:ss") + "'"; }
                        if (!(nombreAjustadorAnterior ?? "").Equals(v.mayusculas(txtgetajustadorname.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "nombreajustador = " + (!string.IsNullOrWhiteSpace(txtgetajustadorname.Text.Trim()) ? "'" + v.mayusculas(txtgetajustadorname.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(solucionAnterior ?? "").Equals(v.mayusculas(txtgetEXTsolucion.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "solucion = " + (!string.IsNullOrWhiteSpace(txtgetEXTsolucion.Text.Trim()) ? "'" + v.mayusculas(txtgetEXTsolucion.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (numActaExtraAnterior != Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetEXTnumActa.Text.Trim()) ? txtgetEXTnumActa.Text.Trim() : "0"))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "numacta = " + (!string.IsNullOrWhiteSpace(txtgetEXTnumActa.Text.Trim()) ? "'" + v.mayusculas(txtgetEXTnumActa.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (supervisorAsistenciaPercanceAnterior != Convert.ToInt32(cbxgetEXTsupervisor.SelectedValue)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "supervisorkcpersonal = " + (cbxgetEXTsupervisor.SelectedIndex > 0 ? "'" + cbxgetEXTsupervisor.SelectedValue.ToString() + "'" : "NULL") + ""; }
                        if (unidadAsistenciaMedicaAnterior != Convert.ToInt32((!string.IsNullOrWhiteSpace(txtgetMedicoUnidad.Text.Trim()) ? txtgetMedicoUnidad.Text.Trim() : "0"))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "unidadmedica = " + (!string.IsNullOrWhiteSpace(txtgetMedicoUnidad.Text.Trim()) ? "'" + v.mayusculas(txtgetMedicoUnidad.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(pertenecienteaAsistenciaMedicaAnterior ?? "").Equals(v.mayusculas(txtgetMedico.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "perteneceunidad = " + (!string.IsNullOrWhiteSpace(txtgetMedico.Text.Trim()) ? "'" + v.mayusculas(txtgetMedico.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(responsabeUnidadAsistenciaMedicaAnterior ?? "").Equals(v.mayusculas(txtgetNameMedico.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "nombreResponsableunidad = " + (!string.IsNullOrWhiteSpace(txtgetNameMedico.Text.Trim()) ? "'" + v.mayusculas(txtgetNameMedico.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!(encasoLesionadosAnteriorAnterior ?? "").Equals(v.mayusculas(txtgetMedicalesionados.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "encasolesionados = " + (!string.IsNullOrWhiteSpace(txtgetMedicalesionados.Text.Trim()) ? "'" + v.mayusculas(txtgetMedicalesionados.Text.Trim().ToLower()) + "'" : "NULL"); }
                        if (!string.IsNullOrWhiteSpace(binarizedPDF)) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "dibujo = '" + binarizedPDF + "'"; }
                        if (!(comentariosAnterior ?? "").Equals(v.mayusculas(txtgetComentarios.Text.Trim().ToLower()))) { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += "comentarios = " + (!string.IsNullOrWhiteSpace(txtgetComentarios.Text.Trim()) ? "'" + v.mayusculas(txtgetComentarios.Text.Trim().ToLower()) + "'" : "NULL"); }
                        sql = string.Format(sql, cambios);
                        if (dibujoExportado == 1)
                        { { if (!string.IsNullOrWhiteSpace(cambios)) cambios += ","; cambios += " dibujoExportado='" + dibujoExportado + "'"; } }
                        if (v.c.insertar(sql))
                        {
                            if (!yaAparecioMensaje)
                            {
                                if (motivo)
                                    MessageBox.Show("Datos Actualizados Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                else
                                    MessageBox.Show("Se Ha Añadido la información del Reporte \"" + lblFolio.Text + "\" Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            resA = true;
                        }
                    }
                }
            }
            return resA;
        }
        bool insertar()
        {
            var resI = false;
            object economico = cbxgeteco.SelectedValue;
            if (!v.camposVaciosPercances(cbxgetempresa.SelectedIndex, cbxgetarea.SelectedIndex, cbxgeteco.SelectedIndex, idconductor, txtgetcredencial.Text.Trim()) && v.camposVaciosServiciosconMensaje(cbxgetestacion1.SelectedIndex, cbxgetestacion2.SelectedIndex, cbxgetestacion3.SelectedIndex, cbxgetestacion4.SelectedIndex, cbxgetecorecup.SelectedIndex, cbxgetestacion.SelectedIndex) && v.camposVacios_DatosTercero_conMensajes(txtgetmarcaVThird.Text.Trim(), txtgetyearVThird.Text.Trim(), txtgetplacasVThird.Text.Trim(), txtgetNameCThird.Text.Trim(), txtgetphoneCThird.Text.Trim(), txtgetaddressCThird.Text.Trim()) && v.camposVacios_SeguroUnidadTransmasivo_conMensajes(txtgetNumSeguro.Text.Trim(), txtgetajustadorname.Text.Trim()) && v.camposvacios_DatosExtra_conMensajes(txtgetEXTsolucion.Text.Trim(), txtgetEXTnumActa.Text, cbxgetEXTsupervisor.SelectedIndex) && v.camposvacios_AsistenciaMedica_conMensajes(txtgetMedicoUnidad.Text.Trim(), txtgetMedico.Text.Trim(), txtgetNameMedico.Text.Trim(), txtgetMedicalesionados.Text.Trim()) && !v.existeNumActa_sNumSeguro(txtgetNumSeguro.Text.Trim(), txtgetEXTnumActa.Text.Trim()))
            {
                string sql = "INSERT INTO reportepercance ({0}) VALUES ({1})";
                string values = "consecutivo,ecofkcunidades,conductorfkcpersonal,fechaHoraAccidente";
                string valores = "'" + folio + "','" + economico + "','" + idconductor + "','" + DTPgetDate.Value.ToString("yyyy-MM-dd ") + DTPgetTIMEaccident.Value.ToString("HH:mm:ss") + "'";
                if (cbxgetServicio.SelectedIndex > 0) { values += ",servicioenlaborfkcservicios"; valores += ",'" + cbxgetServicio.SelectedValue + "'"; }
                if (!string.IsNullOrWhiteSpace(txtgetPlaceAccident.Text.Trim())) { values += ",lugaraccidente"; valores += ",'" + v.mayusculas(txtgetPlaceAccident.Text.Trim().ToLower()) + "'"; }
                if (cbxgetdireccion.SelectedIndex > 0) { values += ",direccion"; valores += ",'" + cbxgetdireccion.SelectedValue + "'"; }
                if (v.camposVaciosServiciossinMensaje(cbxgetestacion1.SelectedIndex, cbxgetestacion2.SelectedIndex, cbxgetestacion3.SelectedIndex, cbxgetestacion4.SelectedIndex, cbxgetecorecup.SelectedIndex, cbxgetestacion.SelectedIndex)) { values += ",estacion1fkcestaciones,estacion2fkcestaciones,estacion3fkcestaciones,estacion4fkcestaciones,ecorecuperacionfkcunidades,estacionfkcestaciones"; valores += ",'" + cbxgetestacion1.SelectedValue + "','" + cbxgetestacion2.SelectedValue + "','" + cbxgetestacion3.SelectedValue + "','" + cbxgetestacion4.SelectedValue + "','" + cbxgetecorecup.SelectedValue + "','" + cbxgetestacion.SelectedValue + "'"; }
                if (!string.IsNullOrWhiteSpace(txtgetSintesis.Text.Trim())) { values += ",sintesisocurrido"; valores += ",'" + v.mayusculas(txtgetSintesis.Text.Trim().ToLower()) + "'"; }
                object coordenadas = getCoordenadas4Imagenes();
                if (!string.IsNullOrWhiteSpace(coordenadas.ToString())) { values += ",coordenadasimagenes"; valores += ",'" + coordenadas + "'"; }
                if (!string.IsNullOrWhiteSpace(txtgetDescripcion.Text.Trim())) { values += ",descripcion"; valores += ",'" + v.mayusculas(txtgetDescripcion.Text.Trim().ToLower()) + "'"; };
                if (v.camposVacios_DatosTercero_sinMensajes(txtgetmarcaVThird.Text.Trim(), txtgetyearVThird.Text.Trim(), txtgetplacasVThird.Text.Trim(), txtgetNameCThird.Text.Trim(), txtgetphoneCThird.Text.Trim(), txtgetaddressCThird.Text.Trim()))
                {
                    values += ", marcavehiculotercero, yearvehiculotercero, placasvehiculotercero, nombreconductortercero, telefonoconductortercero, domicilioconductorterceromusuarioinsertofkcpersonal";
                    valores += string.Format(",'{0}','{1}','{2}','{3}','{4}','{5}','{6}'", new object[] { v.mayusculas(txtgetmarcaVThird.Text.Trim().ToLower()), v.mayusculas(txtgetyearVThird.Text.Trim().ToLower()), v.mayusculas(txtgetplacasVThird.Text.Trim().ToLower()), v.mayusculas(txtgetNameCThird.Text.Trim().ToLower()), v.mayusculas(txtgetphoneCThird.Text.Trim().ToLower()), v.mayusculas(txtgetaddressCThird.Text.Trim().ToLower()), idUsuario });
                }
                if (v.camposVacios_SeguroUnidadTransmasivo_sinMensajes(txtgetNumSeguro.Text.Trim(), txtgetajustadorname.Text.Trim())) { values += ", numreporteseguro, horaotorgamiento, horallegadaseguro, nombreajustador"; valores += ",'" + txtgetNumSeguro.Text.Trim() + "','" + DTPgetTimeSeguro.Value.ToString("HH:mm:ss") + "','" + DTPgetplaceTIME.Value.ToString("HH:mm:ss") + "','" + v.mayusculas(txtgetajustadorname.Text.Trim().ToLower()) + "'"; }
                if (v.camposvacios_DatosExtra_sinMensajes(txtgetEXTsolucion.Text.Trim(), txtgetEXTnumActa.Text, cbxgetEXTsupervisor.SelectedIndex)) { values += ", solucion, numacta, supervisorkcpersonal"; valores += ",'" + v.mayusculas(txtgetEXTsolucion.Text.Trim().ToLower()) + "','" + txtgetEXTnumActa.Text.Trim() + "','" + cbxgetEXTsupervisor.SelectedValue + "'"; }
                if (v.camposvacios_AsistenciaMedica_sinMensajes(txtgetMedicoUnidad.Text.Trim(), txtgetMedico.Text.Trim(), txtgetNameMedico.Text.Trim())) { values += ",unidadmedica, perteneceunidad, nombreResponsableunidad"; valores += ",'" + txtgetMedicoUnidad.Text.Trim() + "','" + v.mayusculas(txtgetMedico.Text.Trim().ToLower()) + "','" + v.mayusculas(txtgetNameMedico.Text.Trim().ToLower()) + "'"; if (!string.IsNullOrWhiteSpace(txtgetMedicalesionados.Text)) { values += ",encasolesionados"; valores += ",'" + v.mayusculas(txtgetMedicalesionados.Text.Trim().ToLower()) + "'"; } }
                if (!string.IsNullOrWhiteSpace(binarizedPDF)) { values += ",dibujo"; valores += ",'" + binarizedPDF + "'"; }
                if (!string.IsNullOrWhiteSpace(txtgetComentarios.Text.Trim())) { values += ",comentarios"; valores += ",'" + v.mayusculas(txtgetComentarios.Text.Trim().ToLower()) + "'"; }
                if (dibujoExportado == 1) { values += ",dibujoExportado"; valores += ",'" + dibujoExportado + "'"; }
                sql = string.Format(sql, values, valores);
                if (v.c.insertar(sql)) { if (!yaAparecioMensaje) MessageBox.Show("El Reporte Se Ha Salvado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk); resI = true; }
            }
            return resI;
        }
        void limpiar()
        {
            lblFolio.Text = null;
            if (pinsertar)
            {
                btnsave.BackgroundImage = Properties.Resources.save;
                editar = false;
                folio = 0;
                generaFolio();
            }
            cbxgetempresa.SelectedIndex = idconductor = 0;
            txtgetcredencial.Clear();
            DTPgetDate.Value = DateTime.Today;
            DTPgetTIMEaccident.Value = DTPgetTimeSeguro.Value = DTPgetplaceTIME.Value = DateTime.Now;
            cbxgetdireccion.SelectedIndex = cbxgetEXTsupervisor.SelectedIndex = 0;
            txtgetPlaceAccident.Clear();
            txtgetSintesis.Clear();
            imagenes[0].Clear();
            dibujoExportado = 0;
            cbxgetarea.Enabled = cbxgeteco.Enabled = pnEvidencias.Visible = pEvidencias.Visible = false;
            imagenes[1].Clear();
            imagenes[2].Clear();
            imagenes[3].Clear();
            pbdibujar.Location = new Point(11, 13);
            pbdibujar.Size = new Size(622, 278);
            pFolio.BringToFront();
            txtgetmarcaVThird.Clear();
            initializeECOSBusq();
            initializeConductorsBusq();
            txtgetyearVThird.Clear();
            txtgetplacasVThird.Clear();
            txtgetNameCThird.Clear();
            txtgetphoneCThird.Clear();
            txtgetaddressCThird.Clear();
            txtgetNumSeguro.Clear();
            txtgetajustadorname.Clear();
            txtgetEXTsolucion.Clear();
            txtgetEXTnumActa.Clear();
            txtgetDescripcion.Clear();
            txtgetMedicoUnidad.Clear();
            lblconductor.Text = "Firma del\nConductor";
            lblsupervisor.Text = "Firma del\nSupervisor";
            p1.Visible = DTPgetDate.Enabled = btnsave.Visible = lblsave.Visible = button8.Visible = label60.Visible = button9.Visible = label67.Visible = !(pVisualizar.Visible = pFinalizar.Visible = pGenerar.Visible = pnewreport.Visible = p2.Visible = pFile.Visible = false);
            txtgetMedico.Clear();
            txtgetNameMedico.Clear();
            txtgetMedicalesionados.Clear();
            pbdibujar.BackgroundImage = pEvidencia1.Image = pEvidencia2.Image = pEvidencia3.Image = pEvidencia4.Image = null;
            lblshowconductor.Text = binarizedPDF = lblnameFile.Text = lblsizeFile.Text = null;
            DTPgetDate.MinDate = DateTime.Today.Subtract(TimeSpan.FromDays(2));
            DTPgetDate.Value = DateTime.Today;
            txtgetComentarios.Clear();
            btnsave.BackgroundImage = Properties.Resources.save;
            conductorFinalizar = supervisorFinalizar = 0; idconductor = 0; economicoAnterior = 0; idconductorAnterior = 0; servicioenLaborAnterior = 0; direccionAnterior = 0; estacion1 = 0; estacion2 = 0; estacion3 = 0; estacion4 = 0; numSeguroTransmasivoAnterior = 0; economicoRecupAnterior = 0; numActaExtraAnterior = 0; estacionAnterior = 0; supervisorAsistenciaPercanceAnterior = 0; unidadAsistenciaMedicaAnterior = 0; statusFinalizado = 0; fechaAccidenteAnterior = DateTime.Now; horaAccidenteAnterior = DateTime.Now; horaOtorgnumSegAnterior = DateTime.Now; horaajustLlegaSiniestro = DateTime.Now; pdfFOREDIT = null; resetPixels(); lugarAccidenteAnterior = null; sintesisOcurridoAnterior = null; descripcionAnterior = null; marcaTerceroAnterior = null; yearTerceroAnterior = null; placasTerceroAnterior = null; nombreCTerceroAnterior = null; telefonoTerceroAnterior = null; domicilioTerceroAnterior = null; nombreAjustadorAnterior = null; solucionAnterior = null; pertenecienteaAsistenciaMedicaAnterior = null; responsabeUnidadAsistenciaMedicaAnterior = null; encasoLesionadosAnteriorAnterior = null; comentariosAnterior = null; yaAparecioMensaje = false;
            cbxgetempresa.Enabled = DTPgetDate.Enabled = DTPgetTIMEaccident.Enabled = cbxgetdireccion.Enabled = pbdibujar.Enabled = DTPgetplaceTIME.Enabled = DTPgetTimeSeguro.Enabled = cbxgetEXTsupervisor.Enabled = btnHDConductor.Enabled = btnHDSupervisor.Enabled = !(txtgetcredencial.ReadOnly = txtgetPlaceAccident.ReadOnly = txtgetSintesis.ReadOnly = txtgetDescripcion.ReadOnly = txtgetmarcaVThird.ReadOnly = txtgetyearVThird.ReadOnly = txtgetplacasVThird.ReadOnly = txtgetNameCThird.ReadOnly = txtgetaddressCThird.ReadOnly = txtgetphoneCThird.ReadOnly = txtgetNumSeguro.ReadOnly = txtgetajustadorname.ReadOnly = txtgetEXTsolucion.ReadOnly = txtgetEXTnumActa.ReadOnly = txtgetMedicoUnidad.ReadOnly = txtgetMedico.ReadOnly = txtgetNameMedico.ReadOnly = txtgetMedicalesionados.ReadOnly = txtgetComentarios.ReadOnly = panel1.Enabled = panel2.Enabled = panel3.Enabled = panel4.Enabled = false);
        }
        void generaFolio()
        {
            if (folio == 0)
            {
                string folio = v.getaData("SELECT MAX(consecutivo) FROM reportepercance").ToString();
                if (string.IsNullOrWhiteSpace(folio))
                    folio = "0";
                this.folio = Convert.ToInt32(folio) + 1;
            }
            string Folio = this.folio.ToString();
            while (Folio.Length < 7)
                Folio = "0" + Folio;
            lblFolio.Text = "RP - " + Folio;
        }
        string getCoordenadas4Imagenes()
        {
            string coordenadas = "";
            if (imagenes[0].Count > 0 || imagenes[1].Count > 0 || imagenes[2].Count > 0 || imagenes[3].Count > 0)
            {
                for (int i = 0; i < imagenes.GetLength(0); i++)
                {
                    if (imagenes[i].Count > 0)
                    {
                        if (coordenadas != "") coordenadas += "/";
                        coordenadas += i + ":";
                        string temp = imagenes[i][0].X + "," + imagenes[i][0].Y;
                        for (int j = 1; j < imagenes[i].Count - 1; j++)
                            temp += ";" + imagenes[i][j].X + "," + imagenes[i][j].Y;
                        coordenadas += temp;
                    }
                }
            }
            return coordenadas;
        }
        bool getCambios()

        {
            return (statusFinalizado == 0 && (cbxgeteco.SelectedIndex > 0 && idconductor > 0) && (economicoAnterior != Convert.ToInt32(cbxgeteco.SelectedValue) || idconductorAnterior != idconductor || fechaAccidenteAnterior.ToString("yyyy-MM-dd") != DTPgetDate.Value.ToString("yyyy-MM-dd") || DTPgetTIMEaccident.Value.ToString("HH:mm") != horaAccidenteAnterior.ToString("HH:mm") || servicioenLaborAnterior != Convert.ToInt32(cbxgetServicio.SelectedValue) || !(lugarAccidenteAnterior ?? "").Equals(v.mayusculas(txtgetPlaceAccident.Text.Trim().ToLower())) || direccionAnterior != Convert.ToInt32(cbxgetdireccion.SelectedValue) || (estacion1 != Convert.ToInt32(cbxgetestacion1.SelectedValue) && cbxgetestacion1.DataSource != null) || (estacion2 != Convert.ToInt32(cbxgetestacion2.SelectedValue) && cbxgetestacion2.DataSource != null) || (estacion3 != Convert.ToInt32(cbxgetestacion3.SelectedValue) && cbxgetestacion3.DataSource != null) || (estacion4 != Convert.ToInt32(cbxgetestacion4.SelectedValue) && cbxgetestacion4.DataSource != null) || (economicoRecupAnterior != Convert.ToInt32(cbxgetecorecup.SelectedValue) && cbxgetecorecup.DataSource != null) || (estacionAnterior != Convert.ToInt32(cbxgetestacion.SelectedValue) && cbxgetestacion.DataSource != null) || !(sintesisOcurridoAnterior ?? "").Equals(v.mayusculas(txtgetSintesis.Text.Trim().ToLower())) || imagenesAnterior[0].Count != imagenes[0].Count || imagenesAnterior[1].Count != imagenes[1].Count || imagenesAnterior[2].Count != imagenes[2].Count || imagenesAnterior[3].Count != imagenes[3].Count || !(descripcionAnterior ?? "").Equals(v.mayusculas(txtgetDescripcion.Text.Trim().ToLower())) || !(marcaTerceroAnterior ?? "").Equals(v.mayusculas(txtgetmarcaVThird.Text.Trim().ToLower())) || !(yearTerceroAnterior ?? "").Equals(txtgetyearVThird.Text.Trim()) || !(placasTerceroAnterior ?? "").Equals(v.mayusculas(txtgetplacasVThird.Text.Trim().ToLower())) || !(nombreCTerceroAnterior ?? "").Equals(v.mayusculas(txtgetNameCThird.Text.Trim().ToLower())) || !(telefonoTerceroAnterior ?? "").Equals(v.mayusculas(txtgetphoneCThird.Text.Trim().ToLower())) || !(domicilioTerceroAnterior ?? "").Equals(v.mayusculas(txtgetaddressCThird.Text.Trim().ToLower())) || numSeguroTransmasivoAnterior != Convert.ToInt32((txtgetNumSeguro.Text.Trim() == "" ? "0" : txtgetNumSeguro.Text.Trim())) || DTPgetTimeSeguro.Value.ToString("HH:mm") != horaOtorgnumSegAnterior.ToString("HH:mm") || DTPgetplaceTIME.Value.ToString("HH:mm") != horaajustLlegaSiniestro.ToString("HH:mm") || !(nombreAjustadorAnterior ?? "").Equals(v.mayusculas(txtgetajustadorname.Text.Trim().ToLower())) || !(solucionAnterior ?? "").Equals(v.mayusculas(txtgetEXTsolucion.Text.Trim().ToLower())) || numActaExtraAnterior != Convert.ToInt32((txtgetEXTnumActa.Text.Trim() == "" ? "0" : txtgetEXTnumActa.Text.Trim())) || supervisorAsistenciaPercanceAnterior != Convert.ToInt32(cbxgetEXTsupervisor.SelectedValue) || unidadAsistenciaMedicaAnterior != Convert.ToInt32((txtgetMedicoUnidad.Text.Trim() == "" ? "0" : txtgetMedicoUnidad.Text.Trim())) || !(pertenecienteaAsistenciaMedicaAnterior ?? "").Equals(v.mayusculas(txtgetMedico.Text.Trim().ToLower())) || !(responsabeUnidadAsistenciaMedicaAnterior ?? "").Equals(v.mayusculas(txtgetNameMedico.Text.Trim().ToLower())) || !(encasoLesionadosAnteriorAnterior ?? "").Equals(v.mayusculas(txtgetMedicalesionados.Text.Trim().ToLower())) || !string.IsNullOrWhiteSpace(binarizedPDF) || !(comentariosAnterior ?? "").Equals(v.mayusculas(txtgetComentarios.Text.Trim().ToLower()))));
        }
        private void dgvpercances_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && peditar)
            {
                if (editar && getCambios())
                {
                    var res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsave_Click(null, e);
                        buscarReporte(e);
                    }
                    else if (res == DialogResult.No)
                        buscarReporte(e);
                }
                else
                    buscarReporte(e);
            }
            else { if (!peditar) MessageBox.Show("Usted No Tiene Privilegios Para Editar Un Reporte", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }
        void initializeECOSBusq() { v.iniCombos("SELECT DISTINCT t2.idunidad as id, concat(t3.identificador,LPAD(t2.consecutivo,4,'0')) as eco FROM reportepercance as t1 INNER JOIN cunidades as t2 On t1.ecofkcunidades=t2.idunidad INNER JOIN careas as t3 ON t2.areafkcareas = t3.idarea GROUP BY t1.ecofkcunidades order by concat(t3.identificador,LPAD(t2.consecutivo,4,'0')) asc;", cbxgetexoBusq, "id", "eco", "--SELECCIONE UN ECONÓMICO--"); }
        void initializeConductorsBusq() { v.iniCombos("SELECT DISTINCT t2.idpersona as id, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno, ' ', t2.apMaterno)) as nombres FROM reportepercance as t1 INNER JOIN cpersonal as t2 ON t1.conductorfkcpersonal = t2.idpersona GROUP BY conductorfkcpersonal ORDER BY t2.nombres ASC", cbxgetconductor, "id", "nombres", "--SELECCIONE UN CONDUCTOR--"); }
        private void buscarReporte(DataGridViewCellEventArgs e)
        {
            limpiar();
            idReporteTemp = Convert.ToInt32(dgvpercances.Rows[e.RowIndex].Cells[0].Value);
            folio = (int)v.getaData(string.Format("SELECT consecutivo FROM reportepercance WHERE idreportepercance='{0}'", idReporteTemp));
            generaFolio();
            string[] datosEspecificosReporte = v.getaData(string.Format("SELECT CONVERT( CONCAT(t4.idempresa,'|',t3.idarea,'|',t2.idunidad,'|',(SELECT CONCAT(idpersona,'|',credencial) FROM cpersonal WHERE idpersona = t1.conductorfkcpersonal),'|',IF(servicioenlaborfkcservicios IS NOT NULL,servicioenlaborfkcservicios,''),'|',IF(fechaHoraAccidente IS NOT NULL,fechaHoraAccidente,''),'|',IF(lugaraccidente IS NOT NULL,lugaraccidente,''),'|',IF(direccion IS NOT NULL,direccion,''),'|',IF(estacion1fkcestaciones IS NOT NULL,estacion1fkcestaciones,''),'|',IF(estacion2fkcestaciones IS NOT NULL,estacion2fkcestaciones,''),'|',IF(estacion3fkcestaciones IS NOT NULL,estacion3fkcestaciones,''),'|',IF(estacion4fkcestaciones IS NOT NULL,estacion4fkcestaciones,''),'|',IF(ecorecuperacionfkcunidades IS NOT NULL,ecorecuperacionfkcunidades,''),'|',IF(estacionfkcestaciones IS NOT NULL,estacionfkcestaciones,''),'|',IF(sintesisocurrido IS NOT NULL,sintesisocurrido,''),'|',IF(coordenadasimagenes IS NOT NULL,coordenadasimagenes,''),'|',IF(descripcion IS NOT NULL,descripcion,''),'|',IF(marcavehiculotercero IS NOT NULL,marcavehiculotercero,''),'|',IF(yearvehiculotercero IS NOT NULL,yearvehiculotercero,''),'|',IF(placasvehiculotercero IS NOT NULL,placasvehiculotercero,''),'|',IF(nombreconductortercero IS NOT NULL,nombreconductortercero,''),'|',IF(telefonoconductortercero IS NOT NULL,telefonoconductortercero,''),'|',IF(domicilioconductortercero IS NOT NULL,domicilioconductortercero,''),'|',IF(numreporteseguro IS NOT NULL,numreporteseguro,''),'|',IF(horaotorgamiento IS NOT NULL,horaotorgamiento,''),'|',IF(horallegadaseguro IS NOT NULL,horallegadaseguro,''),'|',IF(nombreajustador IS NOT NULL,nombreajustador,''),'|',IF(solucion IS NOT NULL,solucion,''),'|',IF(numacta IS NOT NULL,numacta,''),'|',IF(supervisorkcpersonal IS NOT NULL,supervisorkcpersonal,''),'|',IF(unidadmedica IS NOT NULL,unidadmedica,''),'|',IF(perteneceunidad IS NOT NULL,perteneceunidad,''),'|',IF(nombreResponsableunidad IS NOT NULL,nombreResponsableunidad,''),'|',IF(encasolesionados IS NOT NULL,encasolesionados,''),'|',IF(dibujo IS NOT NULL,dibujo,''),'|',IF(comentarios IS NOT NULL,comentarios,''),'|',finalizado,'|',dibujoExportado,IF(finalizado = 1,CONCAT('|',firmaconductorfkcpersonal,'|',firmasupervisorfkcpersonal,'|',usuarioFinalizofkcpersonal),''),'|',if(evidencia1 is not null,evidencia1,''),'|',if(evidencia2 is not null, evidencia2,''),'|',if(evidencia3 is not null, evidencia3,''),'|', if(evidencia4 is not null, evidencia4,''))USING UTF8) as r FROM reportepercance AS t1 INNER JOIN cunidades AS t2 ON t1.ecofkcunidades = t2.idunidad INNER JOIN careas AS t3 ON t2.areafkcareas = t3.idarea INNER JOIN cempresas AS t4 ON t3.empresafkcempresas = t4.idempresa WHERE idreportePercance ='{0}'", idReporteTemp)).ToString().Split('|');
            statusFinalizado = Convert.ToInt32(datosEspecificosReporte[37]);
            cbxgetempresa.SelectedValue = datosEspecificosReporte[0];
            if (cbxgetempresa.SelectedIndex == -1) { v.iniCombos("SELECT idempresa as id,UPPER(nombreEmpresa) as nombre FROM cempresas WHERE (status=1 OR idempresa='" + datosEspecificosReporte[0] + "') ORDER BY nombreEmpresa ASC", cbxgetempresa, "id", "nombre", "-- SELECCIONE UNA EMPRESA --"); cbxgetempresa.SelectedValue = datosEspecificosReporte[0]; }
            cbxgetarea.SelectedValue = datosEspecificosReporte[1];
            if (cbxgetarea.SelectedIndex == -1) { v.iniCombos("SELECT idarea as id, UPPER(nombreArea) as nombre FROM careas WHERE empresafkcempresas='" + cbxgetempresa.SelectedValue + "' AND (status=1 OR idarea='" + datosEspecificosReporte[1] + "') ORDER BY nombreArea ASC", cbxgetarea, "id", "nombre", "-- SELECCIONE UN ÁREA --"); cbxgetarea.SelectedValue = datosEspecificosReporte[1]; }
            cbxgeteco.SelectedValue = economicoAnterior = Convert.ToInt32(datosEspecificosReporte[2]);
            if (cbxgeteco.SelectedIndex == -1) { v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE (t1.status =1 OR idunidad='" + datosEspecificosReporte[2] + "') AND t2.idarea='" + cbxgetarea.SelectedValue + "'", cbxgeteco, "idunidad", "eco", "--SELECCIONE UN ECONÓMICO--"); }
            idconductorAnterior = Convert.ToInt32(datosEspecificosReporte[3]);
            txtgetcredencial.Text = datosEspecificosReporte[4];
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[5])) { cbxgetServicio.SelectedValue = servicioenLaborAnterior = Convert.ToInt32(datosEspecificosReporte[5]); if (cbxgetServicio.SelectedIndex == -1) v.iniCombos(string.Format("SELECT idservicio,UPPER(nombre) as nombre FROM cservicios WHERE (status=1 OR idservicio = '{1}') AND  areafkcareas='{0}'", cbxgetarea.SelectedValue, servicioenLaborAnterior), cbxgetServicio, "idservicio", "nombre", "--SELECCIONE UN SERVICIO--"); cbxgetServicio.SelectedValue = servicioenLaborAnterior; }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[6])) { if (DTPgetDate.MinDate > DateTime.Parse(datosEspecificosReporte[6])) { DTPgetDate.MinDate = fechaAccidenteAnterior = DateTime.Parse(datosEspecificosReporte[6]); DTPgetDate.Enabled = false; DTPgetDate.Value = DTPgetDate.MinDate; } else { DTPgetDate.Value = fechaAccidenteAnterior = DateTime.Parse(DateTime.Parse(datosEspecificosReporte[6]).ToString("dd/MM/yyyy")); } DTPgetTIMEaccident.Value = horaAccidenteAnterior = DateTime.Parse(DateTime.Parse(datosEspecificosReporte[6]).ToString("HH:mm")); }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[7])) txtgetPlaceAccident.Text = lugarAccidenteAnterior = v.mayusculas(datosEspecificosReporte[7].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[8])) cbxgetdireccion.SelectedValue = direccionAnterior = Convert.ToInt32(datosEspecificosReporte[8]);
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[9])) { cbxgetestacion1.SelectedValue = estacion1 = Convert.ToInt32(datosEspecificosReporte[9]); if (cbxgetestacion1.SelectedIndex == -1) v.iniCombos(string.Format("SELECT idestacion as id,UPPER(estacion) as nombre FROM cestaciones WHERE (status=1 OR idestacion = '{1}')", cbxgetarea.SelectedValue, estacion1), cbxgetestacion1, "id", "nombre", "--SELECCIONE UNa estacion--"); cbxgetestacion1.SelectedValue = estacion1; }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[10])) { cbxgetestacion2.SelectedValue = estacion2 = Convert.ToInt32(datosEspecificosReporte[10]); if (cbxgetestacion2.SelectedIndex == -1) v.iniCombos(string.Format("SELECT idestacion as id,UPPER(estacion) as nombre FROM cestaciones WHERE (status=1 OR idestacion = '{1}')", cbxgetarea.SelectedValue, estacion2), cbxgetestacion2, "id", "nombre", "--SELECCIONE UNa estacion--"); cbxgetestacion2.SelectedValue = estacion2; }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[11])) { cbxgetestacion3.SelectedValue = estacion3 = Convert.ToInt32(datosEspecificosReporte[11]); if (cbxgetestacion3.SelectedIndex == -1) v.iniCombos(string.Format("SELECT idestacion as id,UPPER(estacion) as nombre FROM cestaciones WHERE (status=1 OR idestacion = '{1}')", cbxgetarea.SelectedValue, estacion3), cbxgetestacion3, "id", "nombre", "--SELECCIONE UNa estacion--"); cbxgetestacion3.SelectedValue = estacion3; }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[12])) { cbxgetestacion4.SelectedValue = estacion4 = Convert.ToInt32(datosEspecificosReporte[12]); if (cbxgetestacion4.SelectedIndex == -1) v.iniCombos(string.Format("SELECT idestacion as id,UPPER(estacion) as nombre FROM cestaciones WHERE (status=1 OR idestacion = '{1}')", cbxgetarea.SelectedValue, estacion4), cbxgetestacion4, "id", "nombre", "--SELECCIONE UNa estacion--"); cbxgetestacion4.SelectedValue = estacion4; }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[13])) { cbxgetecorecup.SelectedValue = economicoRecupAnterior = Convert.ToInt32(datosEspecificosReporte[13]); if (cbxgetecorecup.SelectedIndex == -1) { v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE (t1.status =1 OR t1.idunidad='" + economicoRecupAnterior + "') AND t2.idarea='" + cbxgetarea.SelectedValue + "'", cbxgetecorecup, "idunidad", "eco", "--SELECCIONE UN ECONÓMICO--"); cbxgetecorecup.SelectedValue = economicoRecupAnterior; } }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[14])) { cbxgetestacion.SelectedValue = estacionAnterior = Convert.ToInt32(datosEspecificosReporte[14]); if (cbxgetestacion.SelectedIndex == -1) { v.iniCombos("SELECT idestacion,UPPER(estacion) as estacion FROM cestaciones WHERE (status=1 OR idestacion='" + estacionAnterior + "') ORDER BY estacion ASC", cbxgetestacion, "idestacion", "estacion", "--SELECCIONE UNA ESTACIÓN--"); cbxgetestacion.SelectedValue = estacionAnterior; } }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[15])) txtgetSintesis.Text = sintesisOcurridoAnterior = v.mayusculas(datosEspecificosReporte[15].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[16]))
            {
                string[] imagenes = datosEspecificosReporte[16].Split('/');
                for (int i = 0; i < imagenes.GetLength(0); i++)
                {
                    string[] image = imagenes[i].Split(':'); string[] imagePoints = image[1].Split(';');
                    foreach (string point in imagePoints)
                    {
                        object[] xy = point.Trim().Split(','); var p = new Point(Convert.ToInt32(xy[0]), Convert.ToInt32(xy[1])); this.imagenes[Convert.ToInt32(image[0])].Add(p); imagenesAnterior[Convert.ToInt32(image[0])].Add(p);
                    } /*imagenActual = i; dibujarPuntitos();*/
                }
            }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[17])) txtgetDescripcion.Text = descripcionAnterior = v.mayusculas(datosEspecificosReporte[17].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[18])) txtgetmarcaVThird.Text = marcaTerceroAnterior = v.mayusculas(datosEspecificosReporte[18].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[19])) txtgetyearVThird.Text = yearTerceroAnterior = v.mayusculas(datosEspecificosReporte[19].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[20])) txtgetplacasVThird.Text = placasTerceroAnterior = v.mayusculas(datosEspecificosReporte[20].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[21])) txtgetNameCThird.Text = nombreCTerceroAnterior = v.mayusculas(datosEspecificosReporte[21].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[22])) txtgetphoneCThird.Text = telefonoTerceroAnterior = datosEspecificosReporte[22];
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[23])) txtgetaddressCThird.Text = domicilioTerceroAnterior = v.mayusculas(datosEspecificosReporte[23].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[24])) txtgetNumSeguro.Text = (numSeguroTransmasivoAnterior = Convert.ToInt32(datosEspecificosReporte[24])).ToString();
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[25])) DTPgetTimeSeguro.Value = horaOtorgnumSegAnterior = DateTime.Parse(datosEspecificosReporte[25]);
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[26])) DTPgetplaceTIME.Value = horaajustLlegaSiniestro = DateTime.Parse(datosEspecificosReporte[26]);
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[27])) txtgetajustadorname.Text = nombreAjustadorAnterior = v.mayusculas(datosEspecificosReporte[27].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[28])) txtgetEXTsolucion.Text = solucionAnterior = v.mayusculas(datosEspecificosReporte[28].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[29])) txtgetEXTnumActa.Text = (numActaExtraAnterior = Convert.ToInt32(datosEspecificosReporte[29])).ToString();
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[30])) cbxgetEXTsupervisor.SelectedValue = supervisorAsistenciaPercanceAnterior = Convert.ToInt32(datosEspecificosReporte[30]);
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[31])) txtgetMedicoUnidad.Text = (unidadAsistenciaMedicaAnterior = Convert.ToInt32(datosEspecificosReporte[31])).ToString();
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[32])) txtgetMedico.Text = pertenecienteaAsistenciaMedicaAnterior = v.mayusculas(datosEspecificosReporte[32].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[33])) txtgetNameMedico.Text = responsabeUnidadAsistenciaMedicaAnterior = v.mayusculas(datosEspecificosReporte[33].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[34])) txtgetMedicalesionados.Text = encasoLesionadosAnteriorAnterior = v.mayusculas(datosEspecificosReporte[34].ToLower());
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[35])) { byte[] prueba = Convert.FromBase64String(datosEspecificosReporte[35]); pdfFOREDIT = new MemoryStream(prueba); button8.Visible = label60.Visible = button9.Visible = label67.Visible = !(pVisualizar.Visible = true); }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[36])) txtgetComentarios.Text = comentariosAnterior = datosEspecificosReporte[36];
            dibujoExportado = Convert.ToInt32(datosEspecificosReporte[38]);
            pnewreport.Visible = editar = !(btnsave.Visible = lblsave.Visible = false);
            btnsave.BackgroundImage = Properties.Resources.pencil;
            if (statusFinalizado == 1)
            {
                pGenerar.Visible = true;
                conductorFinalizar = Convert.ToInt32(datosEspecificosReporte[39]);
                supervisorFinalizar = Convert.ToInt32(datosEspecificosReporte[40]);
                lblconductor.Text = v.getaData("SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona ='" + conductorFinalizar + "'").ToString();
                lblsupervisor.Text = v.getaData("SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona ='" + supervisorFinalizar + "'").ToString();
                cbxgetempresa.Enabled = cbxgetarea.Enabled = cbxgeteco.Enabled = DTPgetDate.Enabled = DTPgetTIMEaccident.Enabled = cbxgetServicio.Enabled = cbxgetdireccion.Enabled = cbxgetestacion1.Enabled = cbxgetestacion2.Enabled = cbxgetestacion3.Enabled = cbxgetestacion4.Enabled = cbxgetestacion.Enabled = pbdibujar.Enabled = DTPgetplaceTIME.Enabled = DTPgetTimeSeguro.Enabled = cbxgetEXTsupervisor.Enabled = panel1.Enabled = panel2.Enabled = panel3.Enabled = panel4.Enabled = btnHDConductor.Enabled = btnHDSupervisor.Enabled = !(txtgetcredencial.ReadOnly = txtgetPlaceAccident.ReadOnly = txtgetSintesis.ReadOnly = txtgetDescripcion.ReadOnly = txtgetmarcaVThird.ReadOnly = txtgetyearVThird.ReadOnly = txtgetplacasVThird.ReadOnly = txtgetNameCThird.ReadOnly = txtgetaddressCThird.ReadOnly = txtgetphoneCThird.ReadOnly = txtgetNumSeguro.ReadOnly = txtgetajustadorname.ReadOnly = txtgetEXTsolucion.ReadOnly = txtgetEXTnumActa.ReadOnly = txtgetMedicoUnidad.ReadOnly = txtgetMedico.ReadOnly = txtgetNameMedico.ReadOnly = txtgetMedicalesionados.ReadOnly = txtgetComentarios.ReadOnly = true);
            }
            if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 4]) || !string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 3]) || !string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 2]) || !string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 1]))
            {
                pnEvidencias.Visible = true;
                if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 4]))
                    pEvidencia1.Image = v.StringToImage2(datosEspecificosReporte[datosEspecificosReporte.Length - 4]);
                if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 3]))
                    pEvidencia2.Image = v.StringToImage2(datosEspecificosReporte[datosEspecificosReporte.Length - 3]);
                if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 2]))
                    pEvidencia3.Image = v.StringToImage2(datosEspecificosReporte[datosEspecificosReporte.Length - 2]);
                if (!string.IsNullOrWhiteSpace(datosEspecificosReporte[datosEspecificosReporte.Length - 1]))
                    pEvidencia4.Image = v.StringToImage2(datosEspecificosReporte[datosEspecificosReporte.Length - 1]);
            }
            finalizar();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (importPDF.ShowDialog(this) == DialogResult.OK)
                {
                    FileInfo info = new FileInfo(importPDF.FileName);
                    if (Convert.ToInt32(info.Length) <= 1110000)
                    {
                        MemoryStream ms = new MemoryStream();
                        PdfReader reader = new PdfReader(importPDF.FileName);
                        PdfStamper stamper = new PdfStamper(reader, ms, PdfWriter.VERSION_1_5);
                        stamper.FormFlattening = true;
                        stamper.SetFullCompression();
                        stamper.Close();
                        binarizedPDF = Convert.ToBase64String(ms.ToArray());
                        pdfFOREDIT = ms;
                        pFile.Visible = true;
                        lblnameFile.Text = info.Name;
                        lblsizeFile.Text = string.Format("{0:N2}", ((double)info.Length / 1024)) + " KB";
                        button8.Visible = label60.Visible = button9.Visible = label67.Visible = !(pVisualizar.Visible = true);
                        getCambios(null, e);
                    }
                    else
                    {
                        lblsizeFile.Text = lblnameFile.Text = binarizedPDF = null;
                        pFile.Visible = false;
                        MessageBox.Show("El Tamaño Máximo es 1MB", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch { MessageBox.Show("Error de Formato: El Archivo No Es Un PDF, O Bien Está Dañado", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }
        public void initializeReports(string where)
        {
            dgvpercances.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SET lc_time_names='es_ES';SELECT t1.idreportePercance,concat('RP-',LPAD(t1.consecutivo,7,0)),concat(t3.identificador,LPAD(t2.consecutivo,4,'0')),UPPER((SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t1.conductorfkcpersonal)),UPPER(COALESCE(date_format(t1.fechaHoraAccidente,'%d/%M/%Y %H:%i'),'')), COALESCE((SELECT UPPER(concat(nombre,' - ',Descripcion)) FROM cservicios WHERE t1.servicioenlaborfkcservicios=idservicio),''),COALESCE(UPPER(t1.lugaraccidente),''),if(direccion is not null,UPPER(if(direccion=1,'Norte','Sur')),''),UPPER(if(finalizado = 1,'Finalizado','En Proceso')),coalesce(UPPER(t1.sintesisocurrido),''),coalesce((SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) FROM cpersonal WHERE idpersona=t1.supervisorkcpersonal),'') FROM reportepercance as t1 INNER JOIN cunidades as t2 ON t1.ecofkcunidades=t2.idunidad INNER JOIN careas as t3 ON t2.areafkcareas=t3.idarea " + where + " order by t1.consecutivo DESC;");
            foreach (DataRow row in dt.Rows) dgvpercances.Rows.Add(row.ItemArray);
            dgvpercances.ClearSelection();
        }
    }
}