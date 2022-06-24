
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
    public partial class ReporteUnidadesExternas : Form
    {
        public void ubica()
        {
            pPdf.Location = new Point(gbxbusqueda.Location.X + gbxbusqueda.Width + 10, gbxbusqueda.Location.Y); pguardar.Location = new Point(pPdf.Location.X + pPdf.Width + 10, gbxbusqueda.Location.Y); pfinalizar.Location = new Point(pguardar.Location.X + pguardar.Width + 10, gbxbusqueda.Location.Y); pcancelar.Location = new Point(pfinalizar.Location.X + pfinalizar.Width + 10, gbxbusqueda.Location.Y);
            gbrefacciones.Location = new Point(gbxDiag.Location.X, 11);
        }

        validaciones v;
        int idUsuario, empresa, area, EstatusAnterior;
        conexion con;

        public ReporteUnidadesExternas(int idUsuario, int empresa, int area, validaciones v)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.v = v;

            DateTime fecha = DateTime.Now;
            tbxHoraEnvio.Text = fecha.ToString();

            cmbEstatus1.DrawItem += v.combos_DrawItem;
            EstatusRepa.DrawItem += v.combos_DrawItem;
            cmbEstatus.DrawItem += v.combos_DrawItem;
            cmbTipoR.DrawItem += v.combos_DrawItem;
            cmbEstRep.DrawItem += v.combos_DrawItem;
            cmbRefacciones1.DrawItem += v.combos_DrawItem;
            cmbReTip.DrawItem += v.combos_DrawItem;
        }

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
                                                                            /*/////////////////////PRUEBAS////////////////////*/
        private void button1_Click(object sender, EventArgs e)
        {

            gbxbusqueda.Visible = true;
            gbxDiag.Visible = true;
            gbxUnidad.Visible = true;

            pictureBox1.Visible = false;
            lblText.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            gbxbusqueda.Visible = false;
            gbxDiag.Visible = false;
            gbxUnidad.Visible = false;

            pictureBox1.Visible = true;
            lblText.Visible = true;


        }


        void combo()
        {
            //refacciones
            v.comboswithuot(cmbRefacciones1, new string[] { "--seleccione una opción--", "se requieren refacciones", "no se requieren refacciones" });
            //diagnostico
            v.comboswithuot(cmbEstatus1, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
            v.comboswithuot(cmbEstatus, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada", });
            //mes
            v.comboswithuot(cmbmes1, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
            //tipo de reparacion
            v.comboswithuot(cmbTipoR, new string[] { "--seleccione el Tipo --", "preventivo", "correctivo", "reiterativo" });
            v.comboswithuot(cmbReTip, new string[] { "--seleccione el Tipo --", "preventivo", "correctivo", "reiterativo" });   
            //eststus reparacion
            v.comboswithuot(cmbEstRep, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
            v.comboswithuot(EstatusRepa, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada", });
            //empresa
            v.comboswithuot(cbxEmpresaS, new string[] { "--seleccione una empresa--", "atreyo tour", "browser", "cometa de oro", "nezahualpilli", "travelers" });
            v.comboswithuot(cbxSempresa, new string[] { "--seleccione una empresa--", "atreyo tour", "browser", "cometa de oro", "nezahualpilli", "travelers" });
        }

        private void cmbmes1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbEstatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbDiagTip_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }     

        private void EstatusRepa_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbTipoR_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbRefacciones1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbEstatus1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbEstRep_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbUnidad1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbMecanicob1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cbxSempresa_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cbxEmpresaS_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void pintarcombos(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }


        /*PRUEBA CONEXION A BASE DE DATOS*/

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           button1.Visible = true;
            button2.Visible = true;
        }

        private void btnrefacciones_Click(object sender, EventArgs e)
        {
            gbxDiag.Visible = !(gbrefacciones.Visible = true);
            v.iniCombos("select " + v.c.fieldscrefacciones[0] + " as id,upper(" + v.c.fieldscrefacciones[2] + ") as nombre from crefacciones where " + v.c.fieldscrefacciones[13] + "='1' and empresa='" + empresa + "' order by nombre asc;", cmbrefaccion, "id", "nombre", "--seleccione--");
            pguardar.Visible = false;
            ubica();
        }

        private void cmbRefacciones1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRefacciones1.SelectedIndex > 0 && EstatusAnterior < 3)
                txtfoliof.Enabled = btnrefacciones.Visible = (Convert.ToInt32(cmbRefacciones1.SelectedValue) == 1 ? true : false);
            txtfoliof.Enabled = numUpDownDE.Enabled = numUpDownHASTA.Enabled = LBxRefacc.Enabled = btnFolioFactura.Enabled = btnCancelFact.Enabled = (cmbRefacciones1.SelectedIndex == 1) ? true : false;
        }

        /*/////////////////////PRUEBAS//////////////////*/
        private void ReporteUnidadesExternas_Load(object sender, EventArgs e)
        {
            combo();
            busqueda();
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

        void busqueda()
        {
            v.iniCombos("SELECT t1." + v.c.fieldscunidades[0] + ",concat(t2." + v.c.fieldscareas[2] + ",LPAD(" + v.c.fieldscunidades[1] + ",4,'0')) as ECo FROM cunidades as t1 INNER JOIN careas as t2 ON t1." + v.c.fieldscunidades[3] + "= t2." + v.c.fieldscareas[0] + " order by eco;", cmbUnidad1, "idunidad", "ECo", "--SELECCIONE UNIDAD--");
            
            v.iniCombos("select t1." + v.c.fieldscpersonal[0] + " as id, upper(concat(coalesce(t1." + v.c.fieldscpersonal[2] + ",''),' ',coalesce(t1." + v.c.fieldscpersonal[3] + ",''),' ',t1." + v.c.fieldscpersonal[4] + ")) as nombre from cpersonal as t1 inner join puestos as t2 on t2." + v.c.fieldspuestos[0] + "=t1." + v.c.fieldscpersonal[5] + " where t2." + v.c.fieldspuestos[1] + " like '%Mecánico%'", cmbMecanicob1, "id", "nombre", "--SELECCIONE UN MECÁNICO");
        }

    }
  }
/*ACTUALIZACION 26-06-2022 REPORTE UNIDADES EXTERNAS*/
