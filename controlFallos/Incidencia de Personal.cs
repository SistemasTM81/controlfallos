using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using h = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Threading;

namespace controlFallos
{
    public partial class Incidencia_de_Personal : Form
    {
        int idusuario, idcolaborador, idreporte, _incidenciaAnterior, empresa, area, _status_incidencia;
        public object idf;
        public int ids, idc, idj, ido, idt;
        public int _idsAnterior, _idcAnterior, _idjAnterior, _idoAnterior, _idtAnterior, _idcolaborador;
        string nColaborador, _nombreAnteriror, _credencialAnterior, _lugarAnterior, _sintesisAnterior, _comentarioAnterior, _actaAnterior, _estatus;
        DateTime _fechaAnterior, _horaAnterior;
        string[] _anteriores;
        string[] actuales;
        int[] _idAnteriores;
        int[] _idActuales;
        public bool incidencia_personal = false, supervisor = false, conductor = false, jefe_grupo = false, c_operativo = false, testigo = false, _editar = false, catalogo = false;
        bool res = false;

        bool _mensaje = false, _nuevo = false, _completos = false, _incompletos = false;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool getboolfromint(int i)
        {
            return i == 1;
        }

        validaciones v = new validaciones();
        conexion c = new conexion();
        public Incidencia_de_Personal(int idusuario, int empresa, int area)
        {
            this.idusuario = idusuario;
            this.empresa = empresa;
            this.area = area;
            InitializeComponent();
            cbIncidencia.MouseWheel += new MouseEventHandler(cbIncidencia_MouseWheel);
            cbBIncidencia.MouseWheel += new MouseEventHandler(cbIncidencia_MouseWheel);
            cbMeses.MouseWheel += new MouseEventHandler(cbIncidencia_MouseWheel);
        }
        void cbIncidencia_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        public void privilegios()
        {
            string sql = "SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar) as privilegios FROM privilegios where usuariofkcpersonal='" + idusuario + "' and namform='IncidenciaPersonal'";
            string[] privilegios = v.getaData(sql).ToString().Split(';');
            pinsertar = getboolfromint(Convert.ToInt32(privilegios[0]));
            pconsultar = getboolfromint(Convert.ToInt32(privilegios[1]));
            peditar = getboolfromint(Convert.ToInt32(privilegios[2]));
            mostrar();
        }
        public void mostrar()
        {
            if (pinsertar)
            {
                gbinsertar.Visible = true;
                pGuardar.Visible = true;
                pNuevo.Visible = true;
            }
            if (pconsultar)
            {
                gbBusqueda.Visible = DgvTabla.Visible = true;
            }
            if (peditar && !pinsertar)
            {
                gbinsertar.Visible = true;
            }
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
        public void incidencias()
        {
            v.iniCombos("select idincidencia as id,upper(concat('Incidencia: ',numeroIncidencia)) as i from catincidencias where status='1' order by numeroIncidencia", cbIncidencia, "id", "i", "--SELECCIONE INCIDENCIA--");
        }
        public void _bincidencias()
        {
            v.iniCombos("select idincidencia as id,upper(concat('Incidencia: ',numeroIncidencia)) as i from catincidencias order by numeroIncidencia ", cbBIncidencia, "id", "i", "--SELECCIONE INCIDENCIA--");
        }

        private void btnIncidencias_Click(object sender, EventArgs e)
        {
            CatIncidencias cat = new CatIncidencias(idusuario, empresa, area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void Incidencia_de_Personal_Load(object sender, EventArgs e)
        {
            cbMeses.SelectedIndex = 0;
            privilegios();
            mostrar_datos();
            txtCredencial.Focus();
            genera_consecutivo();
            incidencias();
            _bincidencias();
            DgvTabla.ClearSelection();
        }

        private void cbIncidencia_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cbIncidencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIncidencia.SelectedIndex > 0)
                lblDescripción.Text = v.getaData("select coalesce(concepto,'') from catincidencias where idincidencia='" + cbIncidencia.SelectedValue + "'").ToString();
            else
                lblDescripción.Text = "";
        }

        public void aumentar(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            b.Size = new Size(60, 55);
        }

        private void txtCredencial_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtLugar_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void txtActa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void txtSintesis_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }
        private void escribirFichero(string texto)
        {
            //obtenemos la carpeta y ejecutable de nuestra aplicación 
            string rutaFichero = Application.StartupPath; ;
            //primer parámetro que es el código de solicitud 
            rutaFichero = rutaFichero + "/PDFTempral";
            try
            {
                //si no existe la carpeta temporal la creamos 
                if (!(Directory.Exists(rutaFichero)))
                {
                    Directory.CreateDirectory(rutaFichero);
                }
            }
            catch (Exception errorC)
            {
                MessageBox.Show("Ha habido un error al intentar " +
                         "crear el fichero temporal:" +
                         Environment.NewLine + Environment.NewLine +
                         rutaFichero + Environment.NewLine +
                         Environment.NewLine + errorC.Message,
                         "Error al crear fichero temporal",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public void to_pdf()
        {
            if (Convert.ToInt32(v.getaData("select count(reporte) from encabezadoreportes where reporte='3';")) > 0)
            {
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
                string filename = Application.StartupPath + "/PDFTempral/Orden_" + txtActa.Text + DateTime.Today.ToLongDateString() + ".pdf";
                DialogResult ews = DialogResult.OK;
                try
                {
                    //if (!string.IsNullOrWhiteSpace(estatusOCompra))
                    //{
                    if ((ews = saveFileDialog1.ShowDialog()) == DialogResult.OK)
                    {
                        filename = saveFileDialog1.FileName;
                        string p = Path.GetExtension(filename);
                        if (p.ToLower() != ".pdf")
                        {
                            filename = filename + ".pdf";
                        }
                    }
                    //}
                    if (ews == DialogResult.OK)
                    {
                        if (filename.Trim() != "")
                        {
                            FileStream file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                            PdfWriter writer = PdfWriter.GetInstance(dc, file);
                            dc.Open();
                            PdfContentByte cb = writer.DirectContent;
                            cb.SetLineWidth(0.08f);
                            int y = 472, y2 = 336;
                            for (int i = 0; i < 6; i++)
                            {
                                cb.SetColorStroke(BaseColor.BLACK);
                                cb.SetColorFill(BaseColor.BLACK);
                                y = y - 16;
                                y2 = y2 - 16;
                                cb.MoveTo(82, y);
                                cb.LineTo(530, y);
                                cb.MoveTo(82, y2);
                                cb.LineTo(530, y2);
                            }
                            cb.Stroke();
                            string[] encabezado = v.getaData("SET lc_time_names = 'es_ES';select upper(concat(coalesce(t1.nombrereporte,''),';',coalesce(t1.codigoreporte,''),';',coalesce(t1.revision,''),';',coalesce(date_format(t1.vigencia,'%M %Y'),''))) as r from encabezadoreportes as t1 where t1.reporte='3';").ToString().Split(';');
                            string[] datos = v.getaData("SET lc_time_names = 'es_ES';select concat(concat(t2.ApPaterno,' ',t2.ApMaterno,' ',t2.nombres),';',t2.credencial,';',date_format(t1.Fecha,'%W %d de %M del %Y'),';',time_format(t1.hora,'%h:%i %p'),';',t1.Lugar,';',t1.Acta,';',t1.Sintesis,';',t1.Comentario,';',t1.consecutivo,';',(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.testigofkCpersonal)) from incidenciapersonal as t1 inner join cpersonal as t2 on t2.idpersona=t1.ColaboradorfkCpersonal where t1.idIncidencia='" + idreporte + "';").ToString().Split(';');
                            byte[] img = Convert.FromBase64String(v.transmasivo);
                            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                            imagen.ScalePercent(11f);
                            imagen.SetAbsolutePosition(60, 685);
                            dc.Add(imagen);
                            PdfPTable tball = new PdfPTable(19);
                            tball.DefaultCell.Border = 1;
                            tball.WidthPercentage = 100; // CAMBIAR A 95
                            tball.HorizontalAlignment = Element.ALIGN_CENTER;
                            PdfPCell c1s1 = new PdfPCell();
                            c1s1.Border = 0;
                            c1s1.BorderColorLeft = c1s1.BorderColorTop = BaseColor.BLACK;
                            c1s1.BorderWidthLeft = c1s1.BorderWidthTop = 2f;
                            tball.AddCell(c1s1);
                            PdfPCell c1s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c1s2_18.Colspan = 17;
                            c1s2_18.Border = 0;
                            c1s2_18.BorderColorTop = BaseColor.BLACK;
                            c1s2_18.BorderWidthTop = 2f;
                            tball.AddCell(c1s2_18);
                            PdfPCell c1s19 = new PdfPCell();
                            c1s19.Border = 0;
                            c1s19.BorderColorTop = c1s19.BorderColorRight = BaseColor.BLACK;
                            c1s19.BorderWidthTop = c1s19.BorderWidthRight = 2f;
                            tball.AddCell(c1s19);
                            PdfPCell c2_47s1 = new PdfPCell();
                            c2_47s1.Rowspan = 46; //AQUI SON 45
                            c2_47s1.Border = 0;
                            c2_47s1.BorderColorLeft = c2_47s1.BorderColorBottom = BaseColor.BLACK;
                            c2_47s1.BorderWidthLeft = c2_47s1.BorderWidthBottom = 2f;
                            tball.AddCell(c2_47s1);
                            PdfPCell c2_4s2_7 = new PdfPCell();
                            c2_4s2_7.Colspan = 5;
                            c2_4s2_7.Rowspan = 3;
                            tball.AddCell(c2_4s2_7);
                            PdfPCell c2s7_15 = new PdfPCell(new Phrase("Nombre: " + v.mayusculas(encabezado[0].ToLower()), FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                            c2s7_15.Colspan = 9;
                            c2s7_15.PaddingBottom = 6;
                            c2s7_15.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c2s7_15);
                            PdfPCell c2s16_18 = new PdfPCell(new Phrase(datos[8], FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                            c2s16_18.Colspan = 3;
                            c2s16_18.PaddingBottom = 6;
                            c2s16_18.HorizontalAlignment = 1;
                            tball.AddCell(c2s16_18);
                            PdfPCell c2s19 = new PdfPCell();
                            c2s19.Rowspan = 46; //AQUI SON 45
                            c2s19.Border = 0;
                            c2s19.BorderColorRight = c2s19.BorderColorBottom = BaseColor.BLACK;
                            c2s19.BorderWidthRight = c2s19.BorderWidthBottom = 2f;
                            tball.AddCell(c2s19);
                            PdfPCell c3s8_18 = new PdfPCell(new Phrase("Código: " + encabezado[1], FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c3s8_18.Colspan = 12;
                            c3s8_18.PaddingBottom = 4;
                            c3s8_18.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c3s8_18);
                            PdfPCell c4s7_12 = new PdfPCell(new Phrase("Vigencia: " + v.mayusculas(encabezado[3].ToLower()), FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL))); //POSIBLE CREACIÓN DE VIGENCIA
                            c4s7_12.Colspan = 6;
                            c4s7_12.PaddingBottom = 4;
                            c4s7_12.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c4s7_12);
                            PdfPCell c4s13_15 = new PdfPCell(new Phrase("Revisión: " + encabezado[2], FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                            c4s13_15.Colspan = 3;
                            c4s13_15.PaddingBottom = 4;
                            c4s13_15.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c4s13_15);
                            PdfPCell c4s16_18 = new PdfPCell(new Phrase("Página 1 de 1", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.NORMAL)));
                            c4s16_18.Colspan = 3;
                            c4s16_18.PaddingBottom = 4;
                            c4s16_18.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c4s16_18);
                            PdfPCell c5s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c5s2_18.Colspan = 17;
                            c5s2_18.Border = 0;
                            tball.AddCell(c5s2_18);
                            PdfPCell c6_47s2 = new PdfPCell();
                            c6_47s2.Rowspan = 41; //AQUI SON 41
                            c6_47s2.Border = 0;
                            tball.AddCell(c6_47s2);
                            PdfPCell c6_7s3_17 = new PdfPCell(new Phrase("Incidencia de Personal", FontFactory.GetFont("CALIBRI", 18, iTextSharp.text.Font.BOLD)));
                            c6_7s3_17.Colspan = 15;
                            c6_7s3_17.Rowspan = 2;
                            c6_7s3_17.Border = 0;
                            c6_7s3_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c6_7s3_17);
                            PdfPCell c6s18 = new PdfPCell();
                            c6s18.Rowspan = 13; // AQUI SON 13
                            c6s18.Border = 0;
                            tball.AddCell(c6s18);
                            PdfPCell c7s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c7s3_17.Colspan = 15;
                            c7s3_17.Border = 0;
                            tball.AddCell(c7s3_17);
                            PdfPCell c8s3_17 = new PdfPCell(new Phrase("NOMBRE DEL COLABORADOR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c8s3_17.Colspan = 15;
                            c8s3_17.Border = 0;
                            c8s3_17.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c8s3_17);
                            PdfPCell c9s3_17 = new PdfPCell(new Phrase(datos[0].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //NOMBRE COLABORADOR
                            c9s3_17.Colspan = 15;
                            c9s3_17.Border = 0;
                            c9s3_17.BorderColorBottom = BaseColor.BLACK;
                            c9s3_17.BorderWidthBottom = 0.5f;
                            tball.AddCell(c9s3_17);
                            PdfPCell c10s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c10s3_17.Colspan = 15;
                            c10s3_17.Border = 0;
                            tball.AddCell(c10s3_17);
                            PdfPCell c11s3 = new PdfPCell(new Phrase("CRED", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD))); // CREDENCIAL
                            c11s3.Border = 0;
                            c11s3.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c11s3);
                            PdfPCell c11s4_6 = new PdfPCell(new Phrase(datos[1], FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c11s4_6.Colspan = 3;
                            c11s4_6.Border = 0;
                            c11s4_6.BorderColorBottom = BaseColor.BLACK;
                            c11s4_6.BorderWidthBottom = 0.5f;
                            c11s4_6.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c11s4_6);
                            PdfPCell c11s7_8 = new PdfPCell(new Phrase("FECHA", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD))); //FECHA
                            c11s7_8.Colspan = 2;
                            c11s7_8.Border = 0;
                            c11s7_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c11s7_8);
                            PdfPCell c11s9_13 = new PdfPCell(new Phrase(datos[2].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c11s9_13.Colspan = 5;
                            c11s9_13.Border = 0;
                            c11s9_13.BorderColorBottom = BaseColor.BLACK;
                            c11s9_13.BorderWidthBottom = 0.5f;
                            c11s9_13.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c11s9_13);
                            PdfPCell c11s14_15 = new PdfPCell(new Phrase("HORA", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD))); //HORA
                            c11s14_15.Colspan = 2;
                            c11s14_15.Border = 0;
                            c11s14_15.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c11s14_15);
                            PdfPCell c11s16_17 = new PdfPCell(new Phrase(datos[3].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c11s16_17.Colspan = 2;
                            c11s16_17.Border = 0;
                            c11s16_17.BorderColorBottom = BaseColor.BLACK;
                            c11s16_17.BorderWidthBottom = 0.5f;
                            c11s16_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c11s16_17);
                            PdfPCell c12s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c12s3_17.Colspan = 15;
                            c12s3_17.Border = 0;
                            tball.AddCell(c12s3_17);
                            PdfPCell c13s3_6 = new PdfPCell(new Phrase("LUGAR DEL INCIDENTE", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD))); //LUGAR INCIDENTE
                            c13s3_6.Colspan = 4;
                            c13s3_6.Border = 0;
                            c13s3_6.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c13s3_6);
                            PdfPCell c13s7_17 = new PdfPCell(new Phrase(datos[4].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c13s7_17.Colspan = 11;
                            c13s7_17.Border = 0;
                            c13s7_17.BorderColorBottom = BaseColor.BLACK;
                            c13s7_17.BorderWidthBottom = 0.5f;
                            c13s7_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c13s7_17);
                            PdfPCell c14s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c14s3_17.Colspan = 15;
                            c14s3_17.Border = 0;
                            tball.AddCell(c14s3_17);
                            PdfPCell c15s3_4 = new PdfPCell(new Phrase("ACTA NO.", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD))); //ACTA #
                            c15s3_4.Colspan = 2;
                            c15s3_4.Border = 0;
                            c15s3_4.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c15s3_4);
                            PdfPCell c15s5_17 = new PdfPCell(new Phrase(datos[5].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c15s5_17.Colspan = 13;
                            c15s5_17.Border = 0;
                            c15s5_17.BorderColorBottom = BaseColor.BLACK;
                            c15s5_17.BorderWidthBottom = 0.5f;
                            c15s5_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c15s5_17);
                            PdfPCell c16s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c16s3_17.Colspan = 15;
                            c16s3_17.Border = 0;
                            tball.AddCell(c16s3_17);
                            PdfPCell c17s3_17 = new PdfPCell(new Phrase("SINTESIS DE LO OCURRIDO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD))); //SINTESIS
                            c17s3_17.Colspan = 15;
                            c17s3_17.Border = 0;
                            c17s3_17.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c17s3_17);
                            PdfPCell c18_23s3_17 = new PdfPCell(new Phrase(datos[6].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c18_23s3_17.Colspan = 15;
                            c18_23s3_17.Rowspan = 6;
                            c18_23s3_17.Border = 0;
                            c18_23s3_17.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            c18_23s3_17.SetLeading(0, 2);
                            tball.AddCell(c18_23s3_17);
                            PdfPCell c18_23s18 = new PdfPCell(new Phrase("1\n2\n3\n4\n5\n6", FontFactory.GetFont("CALIBRI", 15, iTextSharp.text.BaseColor.WHITE)));
                            c18_23s18.Rowspan = 6;
                            c18_23s18.Border = 0;
                            tball.AddCell(c18_23s18);
                            PdfPCell c24s3_17 = new PdfPCell(new Phrase(" \n \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c24s3_17.Colspan = 15;
                            c24s3_17.Border = 0;
                            tball.AddCell(c24s3_17);
                            PdfPCell c24_27s18 = new PdfPCell();
                            c24_27s18.Rowspan = 2;
                            c24_27s18.Border = 0;
                            tball.AddCell(c24_27s18);
                            PdfPCell c25s3_17 = new PdfPCell(new Phrase("COMENTARIO DEL CONDUCTOR O SUPERVISOR DETECTADO EN ANOMALÍA", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD))); //COMENTARIOS
                            c25s3_17.Colspan = 15;
                            c25s3_17.Border = 0;
                            c25s3_17.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c25s3_17);
                            PdfPCell c26_36s3_17 = new PdfPCell(new Phrase(datos[7].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c26_36s3_17.Colspan = 15;
                            c26_36s3_17.Rowspan = 11;
                            c26_36s3_17.Border = 0;
                            c26_36s3_17.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            c26_36s3_17.SetLeading(0, 2);
                            tball.AddCell(c26_36s3_17);
                            PdfPCell c26_36s18 = new PdfPCell(new Phrase("1\n2\n3\n4\n5\n6\n7\n8\n9", FontFactory.GetFont("CALIBRI", 15, iTextSharp.text.BaseColor.WHITE)));
                            c26_36s18.Rowspan = 11;
                            c26_36s18.Border = 0;
                            tball.AddCell(c26_36s18);
                            PdfPCell c37s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c37s3_17.Colspan = 15;
                            c37s3_17.Border = 0;
                            tball.AddCell(c37s3_17);
                            PdfPCell c37_45s18 = new PdfPCell();
                            c37_45s18.Rowspan = 9; // AQUI ES 9
                            c37_45s18.Border = 0;
                            tball.AddCell(c37_45s18);
                            PdfPCell c38s3_4 = new PdfPCell();
                            c38s3_4.Colspan = 2;
                            c38s3_4.Border = 0;
                            tball.AddCell(c38s3_4);
                            PdfPCell c38s5_9 = new PdfPCell(new Phrase("NOMBRE Y FIRMA DE TESTIGO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c38s5_9.Colspan = 5;
                            c38s5_9.Border = 0;
                            c38s5_9.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c38s5_9);
                            PdfPCell c38s10_15 = new PdfPCell(new Phrase(datos[9].ToUpper(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c38s10_15.Colspan = 6;
                            c38s10_15.Border = 0;
                            c38s10_15.HorizontalAlignment = 1;
                            c38s10_15.PaddingBottom = 3;
                            c38s10_15.BorderColorBottom = BaseColor.BLACK;
                            c38s10_15.BorderWidthBottom = 1f;
                            tball.AddCell(c38s10_15);
                            PdfPCell c38s16_17 = new PdfPCell();
                            c38s16_17.Colspan = 2;
                            c38s16_17.Border = 0;
                            tball.AddCell(c38s16_17);
                            PdfPCell c39s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c39s3_17.Colspan = 15;
                            c39s3_17.Rowspan = 2;
                            c39s3_17.Border = 0;
                            tball.AddCell(c39s3_17);
                            PdfPCell c40s3_8 = new PdfPCell(new Phrase("  ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //NOMBRE CONDUCTOR
                            c40s3_8.Colspan = 6;
                            c40s3_8.Border = 0;
                            c40s3_8.BorderColorBottom = BaseColor.BLACK;
                            c40s3_8.BorderWidthBottom = 1f;
                            c40s3_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c40s3_8);
                            PdfPCell c40_41s9_11 = new PdfPCell();
                            c40_41s9_11.Colspan = 3;
                            c40_41s9_11.Rowspan = 2;
                            c40_41s9_11.Border = 0;
                            tball.AddCell(c40_41s9_11);
                            PdfPCell c40s12_17 = new PdfPCell(new Phrase("  ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //NOMBRE SUPERVISOR
                            c40s12_17.Colspan = 6;
                            c40s12_17.Border = 0;
                            c40s12_17.BorderColorBottom = BaseColor.BLACK;
                            c40s12_17.BorderWidthBottom = 1f;
                            c40s12_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c40s12_17);
                            PdfPCell c41s3_8 = new PdfPCell(new Phrase("FIRMA DEL CONDUCTOR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c41s3_8.Colspan = 6;
                            c41s3_8.Border = 0;
                            c41s3_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c41s3_8);
                            PdfPCell c41s12_17 = new PdfPCell(new Phrase("FIRMA DEL SUPERVISOR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c41s12_17.Colspan = 6;
                            c41s12_17.Border = 0;
                            c41s12_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c41s12_17);
                            PdfPCell c42s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c42s3_17.Colspan = 15;
                            c42s3_17.Border = 0;
                            tball.AddCell(c42s3_17);
                            PdfPCell c43s3_8 = new PdfPCell(new Phrase("  ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //NOMBRE JEFE DE GRUPO
                            c43s3_8.Colspan = 6;
                            c43s3_8.Border = 0;
                            c43s3_8.BorderColorBottom = BaseColor.BLACK;
                            c43s3_8.BorderWidthBottom = 1f;
                            c43s3_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c43s3_8);
                            PdfPCell c43_44s9_11 = new PdfPCell();
                            c43_44s9_11.Colspan = 3;
                            c43_44s9_11.Rowspan = 2;
                            c43_44s9_11.Border = 0;
                            tball.AddCell(c43_44s9_11);
                            PdfPCell c43s12_17 = new PdfPCell(new Phrase("  ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //NOMBRE C. OPERATIVO
                            c43s12_17.Colspan = 6;
                            c43s12_17.Border = 0;
                            c43s12_17.BorderColorBottom = BaseColor.BLACK;
                            c43s12_17.BorderWidthBottom = 1f;
                            c43s12_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c43s12_17);
                            PdfPCell c44s3_8 = new PdfPCell(new Phrase("FIRMA DEL JEFE DE GRUPO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c44s3_8.Colspan = 6;
                            c44s3_8.Border = 0;
                            c44s3_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c44s3_8);
                            PdfPCell c44s12_17 = new PdfPCell(new Phrase("FIRMA DEL C. OPERATIVO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c44s12_17.Colspan = 6;
                            c44s12_17.Border = 0;
                            c44s12_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c44s12_17);
                            PdfPCell c45s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c45s2_18.Colspan = 17;
                            c45s2_18.Border = 0;
                            c45s2_18.BorderColorBottom = BaseColor.BLACK;
                            c45s2_18.BorderWidthBottom = 2f;
                            tball.AddCell(c45s2_18);

                            dc.Add(tball);

                            dc.AddCreationDate();
                            dc.Close();
                            Process.Start(filename);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Los campos del encabezado se encuentran vacíos.", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReportesVigencias r = new ReportesVigencias(empresa, area, idusuario,v);
                r.Owner = this;
                r.ShowDialog();
            }
        }
        private void btnPDF_Click(object sender, EventArgs e)
        {
            to_pdf();
        }

        private void txtCredencial_Validating(object sender, CancelEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("Select idPersona as id, upper(concat(t1.apPaterno,' ',t1.ApMaterno,' ',t1.nombres)) as n from cpersonal as t1 where t1.credencial='" + txtCredencial.Text.Trim() + "' and t1.status='1' and t1.empresa='1' and t1.area='1';", c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                idcolaborador = dr.GetInt32("id");
                nColaborador = lblColaborador.Text = dr.GetString("n");
            }
            else
            {
                idcolaborador = 0;
                nColaborador = lblColaborador.Text = "";
            }
            dr.Close();
            c.dbconection().Close();
        }

        private void txtLugar_Validating(object sender, CancelEventArgs e)
        {
            while (txtLugar.Text.Contains("  "))
            {
                txtLugar.Text = txtLugar.Text.Replace("  ", " ").Trim();
                txtLugar.SelectionStart = txtLugar.TextLength + 1;
            }
        }

        private void txtActa_Validating(object sender, CancelEventArgs e)
        {
            while (txtActa.Text.Contains("  "))
            {
                txtActa.Text = txtActa.Text.Replace("  ", " ").Trim();
                txtActa.SelectionStart = txtActa.TextLength + 1;
            }
        }

        private void txtSintesis_Validating(object sender, CancelEventArgs e)
        {
            while (txtSintesis.Text.Contains("  ") || txtSintesis.Text.Contains("\n") || txtSintesis.Text.Contains("\r\n"))
            {
                txtSintesis.Text = txtSintesis.Text.Replace("  ", " ").Trim().Replace("\r\n", " ").Replace("\n", " ");
                txtSintesis.SelectionStart = txtSintesis.TextLength + 1;
            }
        }

        private void txtComentario_Validating(object sender, CancelEventArgs e)
        {
            while (txtComentario.Text.Contains("  ") || txtComentario.Text.Contains("\r\n") || txtComentario.Text.Contains("\n"))
            {
                txtComentario.Text = txtComentario.Text.Replace("  ", " ").Trim().Replace("\r\n", " ").Replace("\n", " ");
                txtComentario.SelectionStart = txtComentario.TextLength + 1;
            }
        }
        void esta_exportando()
        {
            if (pinsertar && pconsultar && peditar)
            {
                if (lblExcel.Text == "Exportando")
                {
                    exportando = true;
                }
                else
                {
                    pexcel.Visible = false;
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            estado = true;
            ThreadStart delegado = new ThreadStart(exporta_a_excel);
            exportar = new Thread(delegado);
            exportar.Start();
        }
        Thread exportar;
        delegate void El_Delegado();
        void cargando()
        {
            pGif.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            lblExcel.Text = "Exportando";
        }
        delegate void El_Delegado1();

        public void exporta_a_excel()//Método para exportar a EXCEL.
        {
            if (DgvTabla.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < DgvTabla.Columns.Count; i++) if (DgvTabla.Columns[i].Visible) dt.Columns.Add(DgvTabla.Columns[i].HeaderText);
                for (int j = DgvTabla.Rows.Count - 1; j >= 0; j--)
                {
                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < DgvTabla.Columns.Count; i++)
                    {

                        if (DgvTabla.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = DgvTabla.Rows[j].Cells[i].Value;
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
                        //foreach (DataGridViewColumn colk in dataGridView3.Columns)
                        //{
                        //if (DgvTabla.Columns[j].Visible == true)
                        //{
                        try
                        {
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                            sheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                            rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230));
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Cells.Font.Name = "Calibri";
                            rng.Cells.Font.Size = 11;
                            rng.Font.Bold = false;
                            if (dt.Rows[i][j].ToString() == "FINALIZADO")
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dt.Rows[i][j].ToString() == "EN PROCESO")
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                        }
                        catch (System.NullReferenceException EX)
                        {
                            MessageBox.Show(EX.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //}
                        //else
                        //{
                        //}
                        //}
                    }
                }
                Thread.Sleep(400);
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                // Exportación_Excel();
                try
                {
                    if (this.InvokeRequired)
                    {
                        El_Delegado1 delega2 = new El_Delegado1(cargando1);
                        this.Invoke(delega2);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void cargando1()
        {
            pGif.Image = null;
            btnExcel.Visible = true;
            lblExcel.Text = "Exportar";
            if (exportando)
            {
                lblExcel.Visible = false;
                btnExcel.Visible = false;
            }
            exportando = false;
            estado = false;
        }

        private void DgvTabla_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (peditar)
            {
                if (e.RowIndex >= 0)
                {
                    verifica_modificaciones(null, e);
                }
            }
            else MessageBox.Show("No cuenta con privilegios para editar un reporte", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        bool estado = false, exportando = false;

        public void genera_consecutivo()
        {
            MySqlCommand maximo = new MySqlCommand("select coalesce(SUBSTRING(consecutivo,LENGTH(consecutivo)-4,5)+1,'1') from incidenciapersonal where idincidencia=(select max(idIncidencia)from incidenciapersonal);", c.dbconection());
            string consecutivo = (string)maximo.ExecuteScalar();
            if (consecutivo == null)
                consecutivo = "00001";
            while (consecutivo.Length < 5)
            {
                consecutivo = "0" + consecutivo;
            }
            lblConsecutivo.Text = "IP-" + consecutivo;
            c.dbconection().Close();
        }
        void limpia_ids()
        {
            ids = idcolaborador = _idcolaborador = idc = ido = idj = idt = 0;
            lblSupervisor.Text = lblConductor.Text = lblJefe.Text = lblOperativo.Text = lblTestigo.Text = "";
        }
        bool verifica_finalizar()
        {
            if (!camposvacios())
            {
                if (cbIncidencia.SelectedIndex > 0)
                {
                    if (!string.IsNullOrWhiteSpace(txtLugar.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(txtActa.Text))
                        {
                            if (!string.IsNullOrWhiteSpace(txtSintesis.Text))
                            {
                                return false;
                            }
                            else
                            {
                                MessageBox.Show("El campo \"Sintesis\" se encuentra vacio", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtSintesis.Focus();
                                return true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El campo \"Acta\" se encuentra vacio", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtActa.Focus();
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El campo \"lugar\" se encuentra vacio", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtLugar.Focus();
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione una \"incidencia\" de la lista desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbIncidencia.Focus();
                    return true;
                }
            }
            else return true;
        }
        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (!verifica_finalizar())
            {
                if (ids > 0 || _idsAnterior > 0)
                {
                    if (idc > 0 || _idcAnterior > 0)
                    {
                        if (idj > 0 || _idjAnterior > 0)
                        {
                            if (ido > 0 || _idoAnterior > 0)
                            {
                                if (idt > 0 || _idtAnterior > 0)
                                {
                                    if (!iguales())
                                    {
                                        if (!_editar)
                                        {
                                            insertar();
                                        }
                                        else
                                        {
                                            string campos = "update incidenciapersonal set Fecha='" + dtpFecha.Value.ToString("yyyy-MM-dd") + "',Hora='" + dtpHora.Value.ToString("HH:mm:ss") + "',Lugar='" + txtLugar.Text.Trim() + "',Acta='" + txtActa.Text.Trim() + "',IncidenciafkCatIncidencias='" + cbIncidencia.SelectedValue + "',Sintesis='" + txtSintesis.Text.Trim() + "',Comentario='" + txtComentario.Text.Trim() + "'";
                                            if (ids > 0) campos += " ,SupervisorfkCpersonal='" + ids + "'";
                                            else campos += " ,SupervisorfkCpersonal='" + _idsAnterior + "'";

                                            if (idc > 0) campos += " ,Conductorfkcpersonal='" + idc + "'";
                                            else campos += " ,Conductorfkcpersonal='" + _idcAnterior + "'";

                                            if (idj > 0) campos += " ,JefefkCpersonal='" + idj + "'";
                                            else campos += " ,JefefkCpersonal='" + _idjAnterior + "'";

                                            if (ido > 0) campos += " ,CoperativofkCpersonal='" + ido + "'";
                                            else campos += " ,CoperativofkCpersonal='" + _idoAnterior + "'";

                                            if (idt > 0) campos += " ,testigofkCpersonal='" + idt + "'";
                                            else campos += " ,testigofkCpersonal='" + _idtAnterior + "'";

                                            FormContraFinal f = new FormContraFinal(empresa, area, this);
                                            f.LabelTitulo.Text = "¿Desea finalizar el reporte?";
                                            if (f.ShowDialog() == DialogResult.OK)
                                            {
                                                MySqlCommand edita = new MySqlCommand(campos += ",Estatus='1',usuariofinalFKcpersonal='" + idf + "' where idIncidencia='" + idreporte + "'", c.dbconection());
                                                edita.ExecuteNonQuery();
                                                c.dbconection().Close();
                                                MessageBox.Show("El reporte se ha finalizado correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                limpiar_campos();
                                                incidencias();
                                                limpia_ids();
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Falta la huella del testigo", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Falta la huella de C. Operativo", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Falta la huella del jefe de grupo", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Falta la huella del conductor", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Falta la huella del supervisor", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void DgvTabla_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.DgvTabla.Columns[e.ColumnIndex].Name == "ESTATUS")
            {
                if (Convert.ToString(e.Value) == "FINALIZADO")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.Khaki;
            }
        }

        public void mostrar_datos()
        {
            MySqlDataAdapter obtener = new MySqlDataAdapter("SET lc_time_names = 'es_ES';select t1.idincidencia as id,coalesce((select x6.idincidencia from catincidencias as x6 where x6.idincidencia=t1.IncidenciafkCatIncidencias),0) as idin, Consecutivo as CONSECUTIVO,upper(concat(t2.ApPaterno,' ',t2.ApMaterno,' ',t2.nombres)) as COLABORADOR,t2.credencial as CREDENCIAL,upper(Date_format(t1.Fecha,'%W %d de %M del %Y')) as FECHA,time_format(t1.hora,'%h:%i %p') as HORA,t1.Lugar as 'LUGAR DEL INCIDENTE',t1.Acta as 'ACTA N°',(select concat('INCIDENCIA: ',x7.numeroIncidencia) from catincidencias as x7 where x7.idincidencia=t1.IncidenciafkCatIncidencias) as 'N° DE INCIDENCIA' ,t1.Sintesis as 'SÍNTESIS DE LO OCURRIDO',t1.Comentario as 'COMENTARIO DE LO OCURRIDO',t1.fecha as fecha,t1.hora as hora,(select upper(concat(x2.ApPaterno,' ',x2.ApMaterno,' ',x2.nombres)) from cpersonal as x2 where x2.idpersona=t1.SupervisorfkCpersonal) as SUPERVISOR,(select upper(concat(x3.ApPaterno,' ',x3.ApMaterno,' ',x3.nombres)) from cpersonal as x3 where x3.idpersona=t1.JefefkCpersonal) as 'JEFE DE GRUPO',(select upper(concat(x4.ApPaterno,' ',x4.ApMaterno,' ',x4.nombres)) from cpersonal as x4 where x4.idpersona=t1.CoperativofkCpersonal) as 'C. OPERATIVO',(select upper(concat(x5.ApPaterno,' ',x5.ApMaterno,' ',x5.nombres)) from cpersonal as x5 where x5.idpersona=t1.testigofkCpersonal) as TESTIGO,if(t1.estatus=0,'EN PROCESO','FINALIZADO') as ESTATUS,(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)  from cpersonal as x1 where x1.idpersona=t1.Conductorfkcpersonal)as conductor,coalesce(t1.SupervisorfkCpersonal,0),coalesce(t1.Conductorfkcpersonal,0),coalesce(t1.JefefkCpersonal,0),coalesce(t1.CoperativofkCpersonal,0),coalesce(t1.testigofkCpersonal,0),coalesce(t1.ColaboradorfkCpersonal,0),(select upper(concat(x8.appaterno,' ',x8.apmaterno,' ',x8.nombres)) from cpersonal as x8 where x8.idpersona=t1.usuariofinalFKcpersonal)as 'USUARIO QUE FINALIZA' from incidenciapersonal as t1 inner join cpersonal as t2 on t2.idPersona=t1.ColaboradorfkCpersonal where date_format(t1.Fecha,'%m')=(select month(now()))  order by t1.consecutivo desc;", c.dbconection());
            DataSet ds = new DataSet();
            obtener.Fill(ds);
            DgvTabla.DataSource = ds.Tables[0];
            DgvTabla.Columns[0].Visible = DgvTabla.Columns[1].Visible = DgvTabla.Columns[12].Visible = DgvTabla.Columns[19].Visible = DgvTabla.Columns[20].Visible = DgvTabla.Columns[25].Visible = DgvTabla.Columns[21].Visible = DgvTabla.Columns[22].Visible = DgvTabla.Columns[23].Visible = DgvTabla.Columns[24].Visible = DgvTabla.Columns[13].Visible = false;
            DgvTabla.ClearSelection();
            c.dbconection().Close();
        }

        private void btnConductor_Click(object sender, EventArgs e)
        {
            if (!camposvacios())
            {
                LectorHuellas l = new LectorHuellas(0, 0, null, null, null, null, null);
                incidencia_personal = true;
                conductor = true;
                l.Owner = this;
                l.ShowDialog();
            }
        }

        private void btnJefe_Click(object sender, EventArgs e)
        {
            if (!camposvacios())
            {
                LectorHuellas l = new LectorHuellas(0, 0, null, null, null, null, null);
                incidencia_personal = true;
                jefe_grupo = true;
                l.Owner = this;
                l.ShowDialog();
            }
        }

        private void btnOperativo_Click(object sender, EventArgs e)
        {
            if (!camposvacios())
            {
                LectorHuellas l = new LectorHuellas(0, 0, null, null, null, null, null);
                incidencia_personal = true;
                c_operativo = true;
                l.Owner = this;
                l.ShowDialog();
            }
        }

        private void btnTestigo_Click(object sender, EventArgs e)
        {
            if (!camposvacios())
            {
                LectorHuellas l = new LectorHuellas(0, 0, null, null, null, null, null);
                incidencia_personal = true;
                testigo = true;
                l.Owner = this;
                l.ShowDialog();
            }
        }

        public void insertar()
        {
            if (!iguales())
            {
                try
                {
                    string campos = "Consecutivo,ColaboradorfkCpersonal,Fecha,Hora,Lugar,Acta,IncidenciafkCatIncidencias,Sintesis,Comentario,FechaHoraRegistro,usuarioFKcpersonal";
                    string valores = "'" + lblConsecutivo.Text + "','" + idcolaborador + "','" + dtpFecha.Value.ToString("yyyy-MM-dd") + "','" + dtpHora.Value.ToString("HH:mm:ss") + "','" + txtLugar.Text.Trim() + "','" + txtActa.Text.Trim() + "','" + cbIncidencia.SelectedValue + "','" + txtSintesis.Text.Trim() + "','" + txtComentario.Text.Trim() + "',now(), '" + this.idusuario + "'";
                    if (ids > 0)
                    {
                        campos += ",SupervisorfkCpersonal";
                        valores += ",'" + ids + "'";
                    }
                    if (idj > 0)
                    {
                        campos += ",JefefkCpersonal";
                        valores += ",'" + idj + "'";
                    }
                    if (ido > 0)
                    {
                        campos += ",CoperativofkCpersonal";
                        valores += ",'" + ido + "'";
                    }
                    if (idt > 0)
                    {
                        campos += ",testigofkCpersonal";
                        valores += ",'" + idt + "'";
                    }
                    if (idc > 0)
                    {
                        campos += ",Conductorfkcpersonal";
                        valores += ",'" + idc + "'";
                    }
                    if (ids > 0 && idc > 0 && idj > 0 && ido > 0 && idt > 0 && !string.IsNullOrWhiteSpace(txtCredencial.Text) && cbIncidencia.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtLugar.Text) && !string.IsNullOrWhiteSpace(txtActa.Text) && !string.IsNullOrWhiteSpace(txtSintesis.Text))
                    {
                        FormContraFinal f = new FormContraFinal(empresa, area, this);
                        f.LabelTitulo.Text = "¿Desea finalizar el reporte?";
                        if (f.ShowDialog() == DialogResult.OK) _completos = true;
                        if (idf != null)
                        {
                            campos += ",Estatus";
                            valores += ",'1'";
                            _completos = true;
                        }
                    }
                    else _incompletos = true;
                    if (_completos || _incompletos)
                    {
                        string consulta = "insert into incidenciapersonal(" + campos + ")values(" + valores + ")";
                        MySqlCommand insertar = new MySqlCommand(consulta, c.dbconection());
                        insertar.ExecuteNonQuery();
                        c.dbconection().Close();
                        if (idf != null)
                        {
                            MessageBox.Show("Reporte finalizado correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Reporte insertado correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        esta_exportando();
                        limpiar_campos();
                        limpia_ids();
                        mostrar_datos();
                        genera_consecutivo();
                        incidencias();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        bool cambios(string[] _anteriores, string[] actuales, int[] _idAnteriores, int[] _idActuales)
        {
            for (int i = 0; i < _anteriores.Length; i++)
            {
                if (actuales[i] != _anteriores[i])
                {
                    if (string.IsNullOrWhiteSpace(_anteriores[i])) res = true;
                }
            }
            for (int j = 0; j < _idAnteriores.Length; j++)
            {
                if ((_idActuales[j] != _idAnteriores[j]) && _idActuales[j] > 0)
                {
                    if (_idAnteriores[j] == 0) res = true;
                }
            }
            return res;
        }
        public void editar()
        {
            if (!iguales())
            {
                if (DateTime.Parse(dtpFecha.Value.ToString("yyyy-MM-dd")) < DateTime.Parse(_fechaAnterior.ToString("yyyy-MM-dd")))
                {
                    MessageBox.Show("La fecha seleccionada es incorrecta", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        actuales = new string[] { dtpFecha.Value.ToString("yyyy-MM-dd"), dtpHora.Value.ToString("HH:mm:ss"), txtLugar.Text.Trim(), txtActa.Text.Trim(), txtSintesis.Text.Trim(), txtComentario.Text.Trim() };
                        _idActuales = new int[] { idcolaborador, (int)cbIncidencia.SelectedValue, ids, idc, idj, ido, idt };
                        cambios(_anteriores, actuales, _idAnteriores, _idActuales);
                        txtCredencial_Validating(null, null);
                        string campos = "update incidenciapersonal set ColaboradorfkCpersonal='" + idcolaborador + "',Fecha='" + dtpFecha.Value.ToString("yyyy-MM-dd") + "',Hora='" + dtpHora.Value.ToString("HH:mm:ss") + "'";
                        if (ids > 0) campos += " ,SupervisorfkCpersonal='" + ids + "'";
                        if (idc > 0) campos += " ,Conductorfkcpersonal='" + idc + "'";
                        if (idj > 0) campos += " ,JefefkCpersonal='" + idj + "'";
                        if (ido > 0) campos += " ,CoperativofkCpersonal='" + ido + "'";
                        if (idt > 0) campos += " ,testigofkCpersonal='" + idt + "'";
                        if (!string.IsNullOrWhiteSpace(txtLugar.Text)) campos += " ,Lugar='" + txtLugar.Text.Trim() + "'";
                        if (!string.IsNullOrWhiteSpace(txtActa.Text)) campos += " ,Acta='" + txtActa.Text.Trim() + "'";
                        if (cbIncidencia.SelectedIndex > 0) campos += " ,IncidenciafkCatIncidencias='" + cbIncidencia.SelectedValue + "'";
                        if (!string.IsNullOrWhiteSpace(txtSintesis.Text)) campos += " ,Sintesis='" + txtSintesis.Text.Trim() + "'";
                        if (!string.IsNullOrWhiteSpace(txtComentario.Text)) campos += " ,Comentario='" + txtComentario.Text.Trim() + "'";
                        MySqlCommand edita = new MySqlCommand(campos += "where idIncidencia='" + idreporte + "'", c.dbconection());
                        edita.ExecuteNonQuery();
                        c.dbconection().Close();
                        if (!_mensaje)
                        {
                            if (res)
                                MessageBox.Show("Se agrego correctamente la información", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                            {
                                observacionesEdicion obs = new observacionesEdicion(v);
                                obs.Owner = this;
                                if (obs.ShowDialog() == DialogResult.OK)
                                {
                                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                    string nombre = v.getaData("select concat(ApPaterno,' ',ApMaterno,' ',nombres) from cpersonal where idpersona='" + _idcolaborador + "';").ToString();
                                    MySqlCommand modificaciones = new MySqlCommand("insert into modificaciones_sistema (form,idregistro,ultimaModificacion,usuariofkcpersonal,fechaHora,Tipo,empresa,area,motivoActualizacion)values('Incidencia De Personal','" + idreporte + "','" + nombre + ";" + _credencialAnterior + ";" + _fechaAnterior + ";" + _horaAnterior + ";" + _incidenciaAnterior + ";" + _lugarAnterior + ";" + _actaAnterior + ";" + _sintesisAnterior + ";" + _comentarioAnterior + "','" + this.idusuario + "',now(),'Actualización De Reporte Incidencia Personal','" + empresa + "','" + area + "','" + observaciones + "');", c.dbconection());
                                    modificaciones.ExecuteNonQuery();
                                    MessageBox.Show("El reporte se ha editado correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    esta_exportando();
                                    limpiar_campos();
                                    limpia_ids();
                                    mostrar_datos();
                                    genera_consecutivo();
                                    incidencias();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public bool iguales()
        {
            if (!_editar)
            {
                if ((idcolaborador == idc) || (idc == 0)) return false;
                else
                {
                    MessageBox.Show("El número de credencial no pertenece a la huella de conductor capturada", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    idc = 0;
                    lblConductor.Text = "";
                    return true;
                }
            }
            else
            {
                if ((_idcolaborador == idc) || (idc == 0)) return false;
                else
                {
                    MessageBox.Show("El número de credencial no pertenece a la huella de conductor capturada", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    idc = 0;
                    lblConductor.Text = "";
                    return true;
                }
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!camposvacios())
            {
                if (!_editar)
                    insertar();
                else
                    editar();
            }
        }

        private void btnSupervisor_Click(object sender, EventArgs e)
        {
            if (!camposvacios())
            {
                LectorHuellas l = new LectorHuellas(0, 0, null, null, null, null, null);
                incidencia_personal = true;
                supervisor = true;
                l.Owner = this;
                l.ShowDialog();
            }
        }

        private void DgvTabla_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void txtCredencial_TextChanged(object sender, EventArgs e)
        {
            if ((_editar && peditar) || pinsertar)
            {

                if (!string.IsNullOrWhiteSpace(txtCredencial.Text) && cbIncidencia.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtLugar.Text.Trim()) && !string.IsNullOrWhiteSpace(txtActa.Text.Trim()) && !string.IsNullOrWhiteSpace(txtSintesis.Text.Trim()) && (ids > 0 || _idsAnterior > 0) && (idc > 0 || _idcAnterior > 0) && (idj > 0 || _idjAnterior > 0) && (ido > 0 || _idoAnterior > 0) && (idt > 0 || _idtAnterior > 0) && _estatus != "FINALIZADO")
                {
                    pFInalizar.Visible = true;
                    pGuardar.Visible = false;
                }
                else
                {
                    pFInalizar.Visible = false;
                    pGuardar.Visible = true;
                    if (_editar && !catalogo)
                    {
                        if (((_credencialAnterior != txtCredencial.Text) || (dtpFecha.Value != _fechaAnterior) || (_idsAnterior != ids && ids > 0) || (idc != _idcAnterior && idc > 0) || (idj != _idjAnterior && idj > 0) || (ido != _idoAnterior && ido > 0) || (idt != _idtAnterior && idt > 0) || (_horaAnterior != dtpHora.Value) || (_incidenciaAnterior != (int)cbIncidencia.SelectedValue && cbIncidencia.SelectedIndex > 0 && cbIncidencia.DataSource != null) || (_lugarAnterior != txtLugar.Text.Trim() && !string.IsNullOrWhiteSpace(txtLugar.Text)) || (_actaAnterior != txtActa.Text.Trim() && !string.IsNullOrWhiteSpace(txtActa.Text)) || (_sintesisAnterior != txtSintesis.Text.Trim() && !string.IsNullOrWhiteSpace(txtSintesis.Text)) || (_comentarioAnterior != txtComentario.Text.Trim() && !string.IsNullOrWhiteSpace(txtComentario.Text))) && (!string.IsNullOrWhiteSpace(txtCredencial.Text))) pGuardar.Visible = true;
                        else pGuardar.Visible = false;
                    }
                    else pGuardar.Visible = true;
                }
            }
        }
        void Habilita_botones()
        {
            btnSupervisor.Enabled = btnConductor.Enabled = btnJefe.Enabled = btnOperativo.Enabled = btnTestigo.Enabled = true;
        }
        public void limpiar_campos()
        {
            desbloquea();
            txtCredencial.Clear();
            lblColaborador.Text = "";
            dtpFecha.ResetText();
            dtpHora.ResetText();
            cbIncidencia.SelectedIndex = 0;
            txtLugar.Clear();
            txtActa.Clear();
            txtSintesis.Clear();
            txtComentario.Clear();
            esta_exportando();
            _editar = _nuevo = res = incidencia_personal = _incompletos = _completos = _mensaje = false;
            pPdf.Visible = pFInalizar.Visible = false;
            incidencias();
            if (pinsertar) { Habilita_botones(); pGuardar.Visible = gbfirmas.Enabled = true; btnGuardar.BackgroundImage = controlFallos.Properties.Resources.guardar__6_; genera_consecutivo(); mostrar_datos(); }
        }
        public void limpiar_busqueda()
        {
            txtBCredencial.Clear();
            cbBIncidencia.SelectedIndex = 0;
            cbMeses.SelectedIndex = 0;
        }
        public void verifica_modificaciones(EventArgs e, DataGridViewCellEventArgs ex)
        {
            if (_editar)
            {
                if (((_credencialAnterior != txtCredencial.Text) || (_idsAnterior != ids && ids > 0) || (idc != _idcAnterior && idc > 0) || (idj != _idjAnterior && idj > 0) || (ido != _idoAnterior && ido > 0) || (idt != _idtAnterior && idt > 0) || (_horaAnterior != dtpHora.Value) || (_incidenciaAnterior != (int)cbIncidencia.SelectedValue && cbIncidencia.SelectedIndex > 0) || (_lugarAnterior != txtLugar.Text.Trim() && !string.IsNullOrWhiteSpace(txtLugar.Text)) || (_actaAnterior != txtActa.Text.Trim() && !string.IsNullOrWhiteSpace(txtActa.Text)) || (_sintesisAnterior != txtSintesis.Text.Trim() && !string.IsNullOrWhiteSpace(txtSintesis.Text)) || (_comentarioAnterior != txtComentario.Text.Trim() && !string.IsNullOrWhiteSpace(txtComentario.Text))) && (!string.IsNullOrWhiteSpace(txtCredencial.Text)))
                {
                    if (MessageBox.Show("¿Desea guardar las modificaciones", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        _mensaje = true;
                        btnGuardar_Click(null, e);
                    }
                    else
                    {
                        if (_nuevo)
                        {
                            limpiar_campos();
                            limpia_ids();
                            genera_consecutivo();
                            mostrar_datos();
                        }
                        else cargar(null, ex);
                    }
                }
                else
                {
                    if (!_nuevo)
                        cargar(null, ex);
                    else
                    {
                        limpiar_campos();
                        limpia_ids();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtCredencial.Text) || cbIncidencia.SelectedIndex > 1 || !string.IsNullOrWhiteSpace(txtLugar.Text.Trim()) || !string.IsNullOrWhiteSpace(txtActa.Text.Trim()) || !string.IsNullOrWhiteSpace(txtSintesis.Text.Trim()) || !string.IsNullOrWhiteSpace(txtComentario.Text.Trim()) || ids > 0 || idc > 0 || idj > 0 || ido > 0 || idt > 0)
                {
                    if (MessageBox.Show("¿Desea guardar la información?", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        limpiar_campos();
                        limpia_ids();
                    }
                    else btnGuardar_Click(null, e);
                }
                else
                {
                    if (!_nuevo)
                        cargar(null, ex);
                    else
                    {
                        limpiar_campos();
                        limpia_ids();
                    }
                }
            }
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCredencial.Text) || dtpFecha.Value <= DateTime.Now || cbIncidencia.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtLugar.Text) || !string.IsNullOrWhiteSpace(txtActa.Text) || !string.IsNullOrWhiteSpace(txtSintesis.Text) || !string.IsNullOrWhiteSpace(txtComentario.Text))
            {
                _nuevo = true;
                verifica_modificaciones(e, null);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBCredencial.Text.Trim()) && cbBIncidencia.SelectedIndex == 0 && cbMeses.SelectedIndex == 0)
            {
                MessageBox.Show("Seleccione un criterio de busqueda", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string sql = "SET lc_time_names = 'es_ES';select t1.idincidencia as id,coalesce((select x6.idincidencia from catincidencias as x6 where x6.idincidencia=t1.IncidenciafkCatIncidencias),0) as idin, Consecutivo as CONSECUTIVO,upper(concat(t2.ApPaterno,' ',t2.ApMaterno,' ',t2.nombres)) as COLABORADOR,t2.credencial as CREDENCIAL,upper(Date_format(t1.Fecha,'%W %d de %M del %Y')) as FECHA,time_format(t1.hora,'%h:%i %p') as HORA,t1.Lugar as 'LUGAR DEL INCIDENTE',t1.Acta as 'ACTA N°',(select concat('INCIDENCIA: ',x7.numeroIncidencia) from catincidencias as x7 where x7.idincidencia=t1.IncidenciafkCatIncidencias) as 'N° DE INCIDENCIA' ,t1.Sintesis as 'SÍNTESIS DE LO OCURRIDO',t1.Comentario as 'COMENTARIO DE LO OCURRIDO',t1.fecha as fecha,t1.hora as hora,(select upper(concat(x2.ApPaterno,' ',x2.ApMaterno,' ',x2.nombres)) from cpersonal as x2 where x2.idpersona=t1.SupervisorfkCpersonal) as SUPERVISOR,(select upper(concat(x3.ApPaterno,' ',x3.ApMaterno,' ',x3.nombres)) from cpersonal as x3 where x3.idpersona=t1.JefefkCpersonal) as 'JEFE DE GRUPO',(select upper(concat(x4.ApPaterno,' ',x4.ApMaterno,' ',x4.nombres)) from cpersonal as x4 where x4.idpersona=t1.CoperativofkCpersonal) as 'C. OPERATIVO',(select upper(concat(x5.ApPaterno,' ',x5.ApMaterno,' ',x5.nombres)) from cpersonal as x5 where x5.idpersona=t1.testigofkCpersonal) as TESTIGO,if(t1.estatus=0,'EN PROCESO','FINALIZADO') as ESTATUS,(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)  from cpersonal as x1 where x1.idpersona=t1.Conductorfkcpersonal)as conductor,coalesce(t1.SupervisorfkCpersonal,0),coalesce(t1.Conductorfkcpersonal,0),coalesce(t1.JefefkCpersonal,0),coalesce(t1.CoperativofkCpersonal,0),coalesce(t1.testigofkCpersonal,0),coalesce(t1.ColaboradorfkCpersonal,0),(select upper(concat(x8.appaterno,' ',x8.apmaterno,' ',x8.nombres)) from cpersonal as x8 where x8.idpersona=t1.usuariofinalFKcpersonal)as 'USUARIO QUE FINALIZA' from incidenciapersonal as t1 inner join cpersonal as t2 on t2.idPersona=t1.ColaboradorfkCpersonal ";
                string wheres = "";
                if (!string.IsNullOrWhiteSpace(txtBCredencial.Text.Trim()))
                {
                    if (wheres != "")
                    {
                        wheres += " and t2.idpersona=(select x1.idpersona from cpersonal as x1 where x1.credencial=t2.credencial)";
                    }
                    else
                    {
                        wheres = " where t2.idpersona=(select x1.idpersona from cpersonal as x1 where x1.credencial='" + txtBCredencial.Text + "')";
                    }
                }
                if (cbBIncidencia.SelectedIndex > 0)
                {
                    if (wheres != "")
                    {
                        wheres += " and (select c1.idincidencia from catincidencias as c1 where c1.idincidencia=t1.IncidenciafkCatIncidencias)='" + cbBIncidencia.SelectedValue + "'";
                    }
                    else
                    {
                        wheres += " where (select c1.idincidencia from catincidencias as c1 where c1.idincidencia=t1.IncidenciafkCatIncidencias)='" + cbBIncidencia.SelectedValue + "'";
                    }
                }
                if (cbMeses.SelectedIndex > 0)
                {
                    if (wheres != "")
                    {
                        wheres += " and (select Date_format(t1.Fecha,'%W %d %M %Y') like '%" + cbMeses.Text + "%' and (select year(t1.Fecha))=( select year(now())))";
                    }
                    else
                    {
                        wheres += " where (select Date_format(t1.Fecha,'%W %d %M %Y') like '%" + cbMeses.Text + "%' and (select year(t1.Fecha))=( select year(now())))";
                    }
                }
                MySqlDataAdapter busqueda = new MySqlDataAdapter(sql + wheres + "order by t1.consecutivo desc", c.dbconection());
                DataSet ds = new DataSet();
                busqueda.Fill(ds);
                DgvTabla.DataSource = ds.Tables[0];
                if (DgvTabla.RowCount == 0)
                {
                    pexcel.Visible = false;
                    MessageBox.Show("No se encontraron reportes con los párametros seleccionados", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mostrar_datos();
                    esta_exportando();
                }
                else
                {
                    if (peditar && pconsultar && peditar)
                    {
                        pexcel.Visible = true;
                        if (!estado)
                        {
                            btnExcel.Visible = true;
                        }
                        lblExcel.Visible = true;
                    }
                }
                c.dbconection().Close();
                limpiar_busqueda();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            mostrar_datos();
            genera_consecutivo();
            esta_exportando();
        }
        void bloquea()
        {
            txtCredencial.Enabled = dtpFecha.Enabled = dtpHora.Enabled = cbIncidencia.Enabled = txtLugar.Enabled = txtActa.Enabled = txtSintesis.Enabled = txtComentario.Enabled = false;
        }
        void desbloquea()
        {
            txtCredencial.Enabled = dtpFecha.Enabled = dtpHora.Enabled = cbIncidencia.Enabled = txtLugar.Enabled = txtActa.Enabled = txtSintesis.Enabled = txtComentario.Enabled = true;
        }
        public void cargar(EventArgs ex, DataGridViewCellEventArgs e)
        {
            _editar = false;
            btnSupervisor.Enabled = btnConductor.Enabled = btnJefe.Enabled = btnOperativo.Enabled = btnTestigo.Enabled = true;
            idreporte = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[0].Value.ToString());
            lblConsecutivo.Text = DgvTabla.Rows[e.RowIndex].Cells[2].Value.ToString();
            _credencialAnterior = txtCredencial.Text = DgvTabla.Rows[e.RowIndex].Cells[4].Value.ToString();
            _nombreAnteriror = lblColaborador.Text = DgvTabla.Rows[e.RowIndex].Cells[3].Value.ToString();
            _incidenciaAnterior = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[1].Value.ToString());
            if (_incidenciaAnterior > 0) _status_incidencia = Convert.ToInt32(v.getaData("select status from catincidencias where idincidencia='" + _incidenciaAnterior + "'").ToString());
            string val = cbIncidencia.ValueMember;
            string display = cbIncidencia.DisplayMember;
            _lugarAnterior = txtLugar.Text = DgvTabla.Rows[e.RowIndex].Cells[7].Value.ToString();
            _actaAnterior = txtActa.Text = DgvTabla.Rows[e.RowIndex].Cells[8].Value.ToString();
            _sintesisAnterior = txtSintesis.Text = DgvTabla.Rows[e.RowIndex].Cells[10].Value.ToString();
            _comentarioAnterior = txtComentario.Text = DgvTabla.Rows[e.RowIndex].Cells[11].Value.ToString();
            _fechaAnterior = dtpFecha.Value = Convert.ToDateTime(DgvTabla.Rows[e.RowIndex].Cells[12].Value.ToString());
            _horaAnterior = dtpHora.Value = Convert.ToDateTime(DgvTabla.Rows[e.RowIndex].Cells[13].Value.ToString());
            lblSupervisor.Text = v.mayusculas(DgvTabla.Rows[e.RowIndex].Cells[14].Value.ToString().ToLower());
            lblConductor.Text = DgvTabla.Rows[e.RowIndex].Cells[19].Value.ToString();
            lblJefe.Text = v.mayusculas(DgvTabla.Rows[e.RowIndex].Cells[15].Value.ToString().ToLower());
            lblOperativo.Text = v.mayusculas(DgvTabla.Rows[e.RowIndex].Cells[16].Value.ToString().ToLower());
            lblTestigo.Text = v.mayusculas(DgvTabla.Rows[e.RowIndex].Cells[17].Value.ToString().ToLower());
            _estatus = DgvTabla.Rows[e.RowIndex].Cells[18].Value.ToString();
            _idsAnterior = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[20].Value.ToString());
            _idcAnterior = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[21].Value.ToString());
            _idjAnterior = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[22].Value.ToString());
            _idoAnterior = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[23].Value.ToString());
            _idtAnterior = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[24].Value.ToString());
            _idcolaborador = Convert.ToInt32(DgvTabla.Rows[e.RowIndex].Cells[25].Value.ToString());
            if (_idsAnterior > 0) btnSupervisor.Enabled = false;
            if (_idcAnterior > 0) btnConductor.Enabled = false;
            if (_idjAnterior > 0) btnJefe.Enabled = false;
            if (_idoAnterior > 0) btnOperativo.Enabled = false;
            if (_idtAnterior > 0) btnTestigo.Enabled = false;
            incidencias();
            if (_status_incidencia == 0 && _incidenciaAnterior > 0)
            {
                DataTable ds = new DataTable();
                ds = (DataTable)cbIncidencia.DataSource;
                //cbIncidencia.DataSource = null;
                DataRow row = ds.NewRow();
                row[val] = _incidenciaAnterior;
                row[display] = v.getaData("select concat('INCIDENCIA: ',numeroIncidencia) from catincidencias where idincidencia='" + _incidenciaAnterior + "';");
                ds.Rows.InsertAt(row, _incidenciaAnterior);
                cbIncidencia.DisplayMember = display;
                cbIncidencia.ValueMember = val;
                cbIncidencia.DataSource = ds;
            }
            cbIncidencia.SelectedValue = _incidenciaAnterior;
            _anteriores = new string[] { _fechaAnterior.ToString("yyyy-MM-dd"), _horaAnterior.ToString("HH:mm:ss"), _lugarAnterior, _actaAnterior, _sintesisAnterior, _comentarioAnterior };
            _idAnteriores = new int[] { _idcolaborador, _incidenciaAnterior, _idsAnterior, _idcAnterior, _idjAnterior, _idoAnterior, _idtAnterior };
            if (_estatus == "FINALIZADO")
            {
                pFInalizar.Visible = false;
                gbfirmas.Enabled = false;
                bloquea();
                if (peditar && pinsertar && pconsultar) pPdf.Visible = true;
            }
            else
            {
                desbloquea();
                gbfirmas.Enabled = true;
                pPdf.Visible = false;
            }
            _editar = true;
            pGuardar.Visible = false;
        }
        delegate void permite_actualizar();
        public bool camposvacios()
        {
            if (!string.IsNullOrWhiteSpace(txtCredencial.Text.Trim()))
            {
                if (Convert.ToInt32(txtCredencial.Text) > 0)
                {
                    if (!string.IsNullOrWhiteSpace(txtCredencial.Text) && !string.IsNullOrWhiteSpace(lblColaborador.Text))
                    {
                        if (dtpFecha.Value <= DateTime.Now)
                        {
                            if ((dtpFecha.Value >= DateTime.Now.AddDays(-3) && !_editar) || _editar)
                            {
                                if (dtpHora.Value.Hour < 24 && dtpHora.Value.Hour > 5)
                                {
                                    if (dtpFecha.Value.ToString("dd - MM - yyyy").Equals(DateTime.Today.ToString("dd - MM - yyyy")))
                                    {
                                        if (DateTime.Parse(dtpHora.Value.ToString("HH:mm")) < DateTime.Parse(DateTime.Now.ToString("HH:mm")))
                                            return false;
                                        else
                                        {
                                            MessageBox.Show("La hora seleccionada no puede ser mayor a la hora actual", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            dtpHora.ResetText();
                                            dtpHora.Focus();
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("La hora seleccionada es incorrecta", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    dtpHora.Focus();
                                    return true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("La fecha seleccionda es incorrecta", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                dtpFecha.Focus();
                                return true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("La fecha del reporte no debe ser mayor la fecha actual", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dtpFecha.ResetText();
                            dtpFecha.Focus();
                            return true;
                        }
                    }
                    else

                    {
                        MessageBox.Show("La credencial no se encuentra registrada o no pertenece a esta área", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCredencial.Clear();
                        txtCredencial.Focus();
                        return true;
                    }

                }
                else
                {
                    MessageBox.Show("El número de credencial debe ser mayor a 0", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCredencial.Clear();
                    txtCredencial.Focus();
                    return true;
                }
            }
            else
            {
                MessageBox.Show("El campo \"credencial\" se encuentra vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCredencial.Focus();
                return true;
            }
        }

        public void normal(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Size = new Size(55, 50);
        }

    }
}
