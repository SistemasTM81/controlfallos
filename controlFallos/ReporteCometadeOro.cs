
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using h = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace controlFallos
{
    public partial class ReporteCometadeOro : Form
    {

        validaciones v;
        int idUsuario, empresa, area;

        private void cboxRango_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxRango.Checked)
            {
                dtpFechaA.Enabled = dtpFechaDe.Enabled = !(cmbmes1.Enabled = false);
                cmbmes1.SelectedIndex = 0;
            }
            else
            {
                cmbmes1.Enabled = !(dtpFechaDe.Enabled = dtpFechaA.Enabled = false);
                dtpFechaA.Value = dtpFechaDe.Value = dtpFechaDe.MaxDate;
            }
        }

        private void btnpdf_Click(object sender, EventArgs e)
        {
           // pdf();
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {

        }

        /* void pdf()
         {
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
                     doc.Open();
                     byte[] logo = Convert.FromBase64String((empresa == 2 ? v.tri : v.TSD));
                     iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(logo);
                     imagen.ScalePercent(20f);
                     imagen.SetAbsolutePosition(460f, 720f);
                     imagen.Alignment = Element.ALIGN_RIGHT;
                     doc.Add(new Phrase(new Chunk((rbngeneral.Checked ? "\n\n INFORME DE MANTENIMIENTO" : "\n\n INFORME DE UNIDAD"), FontFactory.GetFont("calibri", 17, iTextSharp.text.Font.BOLD))));
                     doc.Add(imagen);
                     doc.Add(new Phrase("\n"));
                     doc.Add((rbngeneral.Checked ? general() : unidad()));
                     doc.Add(new Phrase("\n", arial));
                     doc.Add(new Phrase((dgvrefacciones.Rows.Count > 0 ? "REFACCIONES SOLICITADAS" : "NO SE REQUIEREN REFACCIONES"), encabezados));
                     if (dgvrefacciones.Rows.Count > 0)
                         Refacciones(doc);
                     doc.Close();
                     System.Diagnostics.Process.Start(filename);
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
         }*/

        private void ReporteCometadeOro_Load(object sender, EventArgs e)
        {

        }

        public ReporteCometadeOro(int idUsuario, int empresa, int area, validaciones v)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.v = v;

        }



        
    }
  }
/*ACTUALIZACION 19-05-2022 REPORTE UNIDADES EXTERNAS*/
