using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace controlFallos
{
    public partial class ConcentradosComparativas : Form
    {
        validaciones v;
        int idComparativa;
        public ConcentradosComparativas(int Comparativa,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idComparativa = Comparativa;
            v.iniCombos(string.Format("SELECT idrefaccioncomparativa,COALESCE(t2.nombreRefaccion,t1.nombreRefaccion) as refaccion FROM refaccionescomparativa as t1 LEFT JOIN crefacciones as t2 ON t1.refaccionfkcrefacciones = t2.idrefaccion WHERE comparativafkcomparativas='{0}'", Comparativa), cbrefaccion, "idrefaccioncomparativa", "refaccion", "-- SELECCIONE REFACCIÓN --");
          
        }
        private void cbrefaccion_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        private void cbrefaccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbproveedores.DataSource = null;
            if (cbrefaccion.SelectedIndex > 0)
            {
                DataTable dt = (DataTable)v.getData("SELECT idproveedorComparativa, UPPER(CONCAT(t2.empresa,' - $',precioUnitario,'(',t1.observaciones,')')) as empresa FROM proveedorescomparativa as t1 INNER JOIN cproveedores as t2 ON  t1.proveedorfkcproveedores=t2.idproveedor WHERE t1.refaccionfkrefaccionesComparativa = '" + cbrefaccion.SelectedValue + "';"); ;
                lbproveedores.ValueMember = "idproveedorComparativa";
                lbproveedores.DisplayMember = "empresa";
                lbproveedores.DataSource = dt;
                lbproveedores.ClearSelected();
            }
        }
        private void lbproveedores_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (((ListBox)sender).DataSource != null)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, Color.White, Color.Crimson);
                e.DrawBackground();
                DataTable dt = (DataTable)lbproveedores.DataSource;
                e.Graphics.DrawString(dt.Rows[e.Index].ItemArray[1].ToString(), ((ListBox)sender).Font, new SolidBrush(e.ForeColor), e.Bounds.Left + 2, e.Bounds.Top + 2);
                e.DrawFocusRectangle();
            }
        }
        private void lbproveedores_SelectedValueChanged(object sender, EventArgs e)
        {
            btnmejor.Enabled = (lbproveedores.SelectedIndex >= 0);
        }
        private void btnmejor_Click(object sender, EventArgs e)
        {
            object idproveedor = lbproveedores.SelectedValue;
            DataTable dt = null;
            bool res = true;
            if (lbmejores.DataSource != null)
            {
                if (!existe(idproveedor))
                {
                    dt = (DataTable)lbmejores.DataSource;
                    DataRow nuevaFila = dt.NewRow();
                    nuevaFila["idproveedorComparativa"] = idproveedor;
                    nuevaFila["empresa"] = v.getaData("SELECT UPPER(CONCAT(COALESCE(t3.nombreRefaccion,t2.nombreRefaccion),' - ',t4.empresa)) FROM proveedorescomparativa as t1 INNER JOIN refaccionescomparativa as t2 ON t1.refaccionfkrefaccionesComparativa = t2.idrefaccioncomparativa LEFT JOIN crefacciones as t3 ON t2.refaccionfkcrefacciones=t3.idrefaccion INNER JOIN cproveedores as t4 ON t1.proveedorfkcproveedores=t4.idproveedor WHERE t2.idrefaccioncomparativa='" + cbrefaccion.SelectedValue + "'AND idproveedorComparativa='" + idproveedor + "'").ToString().ToUpper();
                    dt.Rows.InsertAt(nuevaFila, dt.Rows.Count);
                }
                else
                {
                    MessageBox.Show("El Proveedor Ya Se Encuentra Seleccionado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    res = false;
                }
            }
            else
            {
                dt = new DataTable();
                DataRow nuevaFila = dt.NewRow();
                dt.Columns.Add("idproveedorComparativa");
                dt.Columns.Add("empresa");
                nuevaFila["idproveedorComparativa"] = idproveedor;
                nuevaFila["empresa"] = v.getaData("SELECT UPPER(CONCAT(COALESCE(t3.nombreRefaccion,t2.nombreRefaccion),' - ',t4.empresa)) FROM proveedorescomparativa as t1 INNER JOIN refaccionescomparativa as t2 ON t1.refaccionfkrefaccionesComparativa = t2.idrefaccioncomparativa LEFT JOIN crefacciones as t3 ON t2.refaccionfkcrefacciones=t3.idrefaccion INNER JOIN cproveedores as t4 ON t1.proveedorfkcproveedores=t4.idproveedor WHERE t2.idrefaccioncomparativa='" + cbrefaccion.SelectedValue + "' AND idproveedorComparativa='"+idproveedor+"'").ToString().ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
            }
            if (res)
            {
                lbmejores.DataSource = null;
                lbmejores.ValueMember = "idproveedorComparativa";
                lbmejores.DisplayMember = "empresa";
                lbmejores.DataSource = dt;
            }
            lbproveedores.ClearSelected();
            lbmejores.ClearSelected();
        }
        bool existe(object idproveedor)
        {
            bool res = false;
            DataTable dt = (DataTable)lbmejores.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].ItemArray[0].ToString().Equals(idproveedor.ToString())) res = true;
            }
            return res;
        }
        private void lbmejores_DataSourceChanged(object sender, EventArgs e)
        {
            lbmejores.ClearSelected();
        }
        private void lbmejores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbmejores.SelectedIndex >= 0) btnquitar.Enabled = true;
            else btnquitar.Enabled = false;
        }
        private void btnquitar_Click(object sender, EventArgs e)
        {
            var idproveedor = Convert.ToInt32(lbmejores.SelectedValue);
            DataTable dt = (DataTable)lbmejores.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].ItemArray[0].ToString().Equals(idproveedor.ToString()))
                {
                    dt.Rows.Remove(dt.Rows[i]);
                    break;
                }
            }
            lbmejores.DataSource = null;
            lbmejores.ValueMember = "idproveedorComparativa";
            lbmejores.DisplayMember = "empresa";
            lbmejores.DataSource = dt;
            lbmejores.ClearSelected();
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            if (lbmejores.DataSource!=null) {
                DataTable dt = (DataTable)lbmejores.DataSource;
                if (MessageBox.Show("¿Desea Guardarlos Como \"Mejores Opciones\"?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                        v.c.insertar("UPDATE proveedorescomparativa SET mejorOpcion ='1' WHERE idproveedorComparativa='" + dt.Rows[i].ItemArray[0] + "'");
                    v.c.insertar("UPDATE comparativas SET status = 3 WHERE idcomparativa='" + idComparativa + "'");
                }
                onCreate(dt);
            }
        }
        void onCreate(DataTable dt)
        {
            Document doc = new Document(PageSize.A4.Rotate());
            doc.SetMargins(50f, 50f, 80f, 40f);
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
                    {
                        filename = filename + ".pdf";
                    }
                    while (filename.ToLower().Contains(".pdf.pdf"))
                    {
                        filename = filename.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
                    }
                }
                if (filename.Trim() != "")
                {

                    string[] datos = v.getaData("Select concat(upper(nombreComparativa),';',upper(descripcionComparativa),';',upper(observacionesComparativa),';',t1.iva) as m from comparativas as t1 where t1.idcomparativa='" + idComparativa + "'").ToString().Split(';');
                    FileStream file = new FileStream(filename,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
                    PdfWriter.GetInstance(doc, file);
                    doc.Open();
                    iTextSharp.text.Font arial = FontFactory.GetFont("Calibri", 8, BaseColor.BLACK);
                    iTextSharp.text.Font arial3 = FontFactory.GetFont("Calibri", 7, BaseColor.BLACK);
                    iTextSharp.text.Font arial2 = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.BOLD);
                    Phrase salto = new Phrase("\n", arial3);

                    PdfPTable tabla = new PdfPTable(24);
                    tabla.WidthPercentage = 100;
                    PdfPCell celda = new PdfPCell(new Phrase(datos[0].ToUpper(), arial2));
                    celda.Colspan = 24;
                    celda.UseDescender = true;
                    celda.UseAscender = false;
                    celda.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    celda.PaddingBottom = 3;
                    tabla.AddCell(celda);

                    PdfPTable tabla1 = new PdfPTable(24);
                    tabla1.WidthPercentage = 100;
                    PdfPCell celda1 = new PdfPCell(new Phrase(datos[1].ToUpper(), arial2));
                    celda1.Colspan = 24;
                    celda1.UseDescender = true;
                    celda1.UseAscender = false;
                    celda1.HorizontalAlignment = 1;
                    celda1.PaddingBottom = 3;
                    tabla1.AddCell(celda1);
                    PdfPCell celda6 = new PdfPCell(new Phrase("\n", arial2));
                    celda6.Colspan = 24;
                    celda6.UseDescender = true;
                    celda6.UseAscender = false;
                    celda6.HorizontalAlignment = 1;
                    tabla1.AddCell(celda6);
                    PdfPTable tabla2 = new PdfPTable(24);
                    tabla2.DefaultCell.PaddingBottom = 3;
                    tabla2.DefaultCell.PaddingLeft = 3;
                    tabla2.DefaultCell.PaddingRight = 3;
                    tabla2.DefaultCell.PaddingTop = 3;
                    tabla2.WidthPercentage = 100;
                    PdfPCell celda2 = new PdfPCell(new Phrase("DESCRIPCIÓN", arial2));
                    celda2.Colspan = 15;
                    celda2.UseDescender = true;
                    celda2.UseAscender = false;
                    celda2.HorizontalAlignment = 1;
                    tabla2.AddCell(celda2);
                    PdfPCell celda3 = new PdfPCell(new Phrase("CANTIDAD", arial2));
                    celda3.Colspan = 3;
                    celda3.UseAscender = false;
                    celda3.UseDescender = true;
                    celda3.HorizontalAlignment = 1;
                    tabla2.AddCell(celda3);
                    PdfPCell celda4 = new PdfPCell(new Phrase("PRECIO UNITARIO", arial2));
                    celda4.Colspan = 3;
                    celda4.UseDescender = true;
                    celda4.UseAscender = false;
                    celda4.HorizontalAlignment = 1;
                    tabla2.AddCell(celda4);
                    PdfPCell celda5 = new PdfPCell(new Phrase("IMPORTE", arial2));
                    celda5.Colspan = 3;
                    celda5.UseDescender = true;
                    celda5.UseAscender = false;
                    celda5.HorizontalAlignment = 1;
                    tabla2.AddCell(celda5);
                    int _num = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string[] res = v.getaData("SELECT CONCAT(coalesce(t2.refaccionfkcrefacciones,t2.nombreRefaccion),' - ',(SELECT empresa FROM cproveedores WHERE idproveedor = t1.proveedorfkcproveedores),';',t2.cantidad,';',t1.preciounitario,';',(t2.cantidad*t1.precioUnitario)) FROM proveedorescomparativa as t1 INNER JOIN refaccionescomparativa as t2 ON t1.refaccionfkrefaccionesComparativa = t2.idrefaccioncomparativa where idproveedorComparativa='" + dt.Rows[i].ItemArray[0] + "'").ToString().Split(';');
                        PdfPCell celda7 = new PdfPCell(new Phrase(res[0], arial2));
                        celda7.Colspan = 15;
                        celda7.HorizontalAlignment = 0;
                        celda7.Padding = 2;
                        celda7.PaddingLeft = 3;
                        tabla2.AddCell(celda7);

                        PdfPCell celda8 = new PdfPCell(new Phrase(res[1], arial));
                        celda8.Colspan = 3;
                        celda8.HorizontalAlignment = 1;
                        celda8.Padding = 2;
                        tabla2.AddCell(celda8);

                        PdfPCell celda22 = new PdfPCell(new Phrase("  $", arial2));
                        celda22.Colspan = 1;
                        celda22.BorderWidthRight = 0;
                        tabla2.AddCell(celda22);

                        PdfPCell celda9 = new PdfPCell(new Phrase((Math.Truncate(Convert.ToDouble(res[2]) * 100) / 100).ToString("N2"), arial2));
                        celda9.Colspan = 2;
                        celda9.HorizontalAlignment = 2;
                        celda9.BorderWidthLeft = 0;
                        celda9.PaddingRight = 3;
                        tabla2.AddCell(celda9);

                        PdfPCell celda23 = new PdfPCell(new Phrase("  $", arial2));
                        celda23.Colspan = 1;
                        celda23.BorderWidthRight = 0;
                        tabla2.AddCell(celda23);

                        PdfPCell celda10 = new PdfPCell(new Phrase((Math.Truncate(Convert.ToDouble(res[3]) * 100) / 100).ToString("N2"), arial2));
                        celda10.Colspan = 2;
                        celda10.HorizontalAlignment = 2;
                        celda10.BorderWidthLeft = 0;
                        celda10.PaddingRight = 3;
                        tabla2.AddCell(celda10);
                        _num++;
                    }
                    while (_num < 26)
                    {
                        PdfPCell celda18 = new PdfPCell(new Phrase(" ", arial2));
                        celda18.Colspan = 15;
                        tabla2.AddCell(celda18);

                        PdfPCell celda19 = new PdfPCell(new Phrase(" ", arial2));
                        celda19.Colspan = 3;
                        tabla2.AddCell(celda19);

                        PdfPCell celda20 = new PdfPCell(new Phrase(" ", arial2));
                        celda20.Colspan = 3;
                        tabla2.AddCell(celda20);

                        PdfPCell celda24 = new PdfPCell(new Phrase(" ", arial2));
                        celda24.Colspan = 1;
                        celda24.BorderWidthRight = 0;
                        tabla2.AddCell(celda24);

                        PdfPCell celda21 = new PdfPCell(new Phrase("                 ", arial2));
                        celda21.Colspan = 2;
                        celda21.HorizontalAlignment = 1;
                        celda21.BorderWidthLeft = 0;
                        tabla2.AddCell(celda21);
                        _num++;
                    }

                    PdfPCell celda11 = new PdfPCell(new Phrase(datos[2], arial3));
                    celda11.Colspan = 18;
                    celda11.Rowspan = 3;
                    celda11.HorizontalAlignment = 1;
                    celda11.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tabla2.AddCell(celda11);
                    double subtotal = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                        subtotal += Convert.ToDouble(v.getaData("SELECT (t1.precioUnitario * t2.cantidad) FROM proveedorescomparativa as t1 INNER JOIN refaccionescomparativa as t2 ON t1.refaccionfkrefaccionesComparativa = t2.idrefaccioncomparativa WHERE t1.idproveedorComparativa = '" + dt.Rows[i].ItemArray[0] + "'"));
                    subtotal = Math.Truncate(subtotal * 100) / 100;

                    PdfPCell celda12 = new PdfPCell(new Phrase("SUBTOTAL", arial2));
                    celda12.Colspan = 3;
                    celda12.HorizontalAlignment = 0;
                    tabla2.AddCell(celda12);

                    PdfPCell celda25 = new PdfPCell(new Phrase("  $", arial2));
                    celda25.Colspan = 1;
                    celda25.BorderWidthRight = 0;
                    tabla2.AddCell(celda25);

                    PdfPCell celda13 = new PdfPCell(new Phrase(subtotal.ToString("N2"), arial2)); //CAMBIAR
                    celda13.Colspan = 2;
                    celda13.HorizontalAlignment = 2;
                    celda13.BorderWidthLeft = 0;
                    celda13.PaddingRight = 3;
                    tabla2.AddCell(celda13);

                    datos[3] = Math.Truncate(Convert.ToDouble(datos[3])).ToString("N2");
                    PdfPCell celda14 = new PdfPCell(new Phrase(datos[3] + "%", arial2));//CAMBIAR
                    celda14.Colspan = 3;
                    celda14.HorizontalAlignment = 2;
                    tabla2.AddCell(celda14);

                    PdfPCell celda26 = new PdfPCell(new Phrase("  $", arial2));
                    celda26.Colspan = 1;
                    celda26.BorderWidthRight = 0;
                    tabla2.AddCell(celda26);
                    double iva = subtotal * (Convert.ToDouble(datos[3]) / 100);
                    PdfPCell celda15 = new PdfPCell(new Phrase(iva.ToString("N2"), arial2));
                    celda15.Colspan = 2;
                    celda15.HorizontalAlignment = 2;
                    celda15.BorderWidthLeft = 0;
                    celda15.PaddingRight = 3;
                    tabla2.AddCell(celda15);

                    PdfPCell celda16 = new PdfPCell(new Phrase("TOTAL", arial2));
                    celda16.Colspan = 3;
                    celda16.HorizontalAlignment = 0;
                    tabla2.AddCell(celda16);

                    PdfPCell celda27 = new PdfPCell(new Phrase("  $", arial2));
                    celda27.Colspan = 1;
                    celda27.BorderWidthRight = 0;
                    tabla2.AddCell(celda27);

                    double total = (subtotal * (Convert.ToDouble(datos[3]) / 100)) + subtotal;

                    PdfPCell celda17 = new PdfPCell(new Phrase(total.ToString("N2"), arial2));
                    celda17.Colspan = 2;
                    celda17.HorizontalAlignment = 2;
                    celda17.BorderWidthLeft = 0;
                    celda17.PaddingRight = 3;
                    tabla2.AddCell(celda17);

                    doc.Add(tabla);
                    doc.Add(salto);
                    doc.Add(tabla1);
                    doc.Add(salto);
                    doc.Add(tabla2);
                    doc.Close();
                    System.Diagnostics.Process.Start(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConcentradosComparativas_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM proveedoresComparativa as t1 INNER JOIN refaccionescomparativa as t2 ON t1.refaccionfkrefaccionesComparativa= t2.idrefaccioncomparativa WHERE t2.comparativafkcomparativas=" + idComparativa + " AND mejorOpcion=1;")) > 0)
            {
                onCreate((DataTable)v.getData("SELECT t1.idproveedorComparativa, (SELECT empresa FROM cproveedores WHERE idproveedor = t1.proveedorfkcproveedores) FROM proveedoresComparativa as t1 INNER JOIN refaccionescomparativa as t2 ON t1.refaccionfkrefaccionesComparativa= t2.idrefaccioncomparativa WHERE t2.comparativafkcomparativas=" + idComparativa + " AND mejorOpcion=1"));
                this.Close();
            }
        }
    }
}