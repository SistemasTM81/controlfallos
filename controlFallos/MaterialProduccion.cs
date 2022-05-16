using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;

namespace controlFallos
{
    public partial class MaterialProduccion : Form
    {
        validaciones v;
        int empresa, area, idUsuario, idEntrega;
        string folio = "";
        double existencia = 0.0;




        public MaterialProduccion(validaciones v, int empresa, int area, int IdUsuario)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = IdUsuario;
            cmbMecanico.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dtFecha.MaxDate = DateTime.Now;
            CargarMecanico();
        }






        public MaterialProduccion()
        {
            InitializeComponent();
        }

        private void codigo_Validate(object sender, EventArgs e)
        {
            buscaref(txtcodigo.Text);
        }

        private void MaterialP_Load(object sender, EventArgs e)
        {
            obtener_folio();
        }

        private void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbDrawable(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void btn_Guardar(object sender, EventArgs e)
        {
            if (v.materialP(folio, txtcodigo.Text, Double.Parse(txtCantidad.Text), cmbMecanico.SelectedIndex, txtMotivo.Text, existencia))
            {
                guardar();
            }
           
        }
        private void nombrealmacen(object sender, EventArgs e)
        {
            obtenerNombre();
        }
        void obtener_folio()
        {
           string consecutivo = v.getFolioP(empresa).ToString();
            if (!string.IsNullOrWhiteSpace(consecutivo))
            {
                folio = "P0-" + (Convert.ToInt32(consecutivo)+1);
            }
            else
            {
                folio = "P0-1";
            }
        }

        public void CargarMecanico()
        {
            cmbMecanico.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';SELECT DISTINCT convert(t2.idPersona, char) id,convert(UPPER(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,''))),char) AS Nombre FROM  cpersonal as t2  where t2.empresa='" + empresa + "' and t2.area = '1'  and t2.cargofkcargos != '2' and t2.status = '1' ORDER BY CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')) asc;");
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila2 = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["Nombre"] = "--SELECCIONE MECANICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            nuevaFila2["id"] = 8000000;
            nuevaFila2["Nombre"] = "OTRO".ToUpper();
            dt.Rows.InsertAt(nuevaFila2, dt.Rows.Count + 1);
            cmbMecanico.DisplayMember = "id";
            cmbMecanico.ValueMember = "Nombre";
            cmbMecanico.DataSource = dt;

        }
        public void buscaref(string codigo)
        {
            string cadenaR = "";

            cadenaR = v.ObtenerRef("SET lc_time_names = 'es_ES';select convert(nombreRefaccion,char), convert(t2.Simbolo, char), convert(t1.existencias, char) from crefacciones as t1 inner join cmarcas as t3 on t1.marcafkcmarcas = t3.idmarca inner join cfamilias as t4 on t3.descripcionfkcfamilias = t4.idfamilia inner join cunidadmedida as t2 on t4.umfkcunidadmedida = t2.idunidadmedida where t1.codrefaccion = '" + codigo + "' and t1.empresa  = '" + empresa + "' and t1.existencias != 0");
            if (cadenaR.ToString().Equals(""))
            {
                MessageBox.Show("No se encontro la refaccion".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtcodigo.Text = "";
            }
            else
            {
                string[] seprar = cadenaR.Split(';');
                lblNomRef.Text = seprar[0].ToString();
                lblMedida.Text = seprar[1].ToString();
                existencia = Convert.ToDouble(seprar[2].ToString());
            }


        }

        void guardar()
        {
            double editar = existencia - Double.Parse(txtCantidad.Text);
            v.Carroceros("insert into materialproduccion (Folio, refaccionfkcrefacciones, cantidad, fechahora, empresa, almacenfkcpersonal, motivo) values ('" + folio.ToString() + "',(select idrefaccion  from crefacciones where codrefaccion ='" + txtcodigo.Text + "' and empresa = '" + empresa + "'),'" + txtCantidad.Text + "', now(), '" + empresa + "', '" + idEntrega + "','"+ txtMotivo.Text + "')");
            /*v.CarroceroE("update crefacciones  set existencias='" + editar + "' Where codrefaccion = '" + txtcodigo.Text + "' and empresa = '" + empresa + "'");*/
            MessageBox.Show("Refaccion agregada correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgImprimir.Rows.Add(txtcodigo.Text, lblNomRef.Text, txtCantidad.Text, lblMedida.Text, dtFecha.Value.ToString("yyyy-MM-dd"), cmbMecanico.SelectedValue.ToString(), lblNomUsuario.Text);
            var selectedOption = MessageBox.Show("¿Desea seguir agregando refacciones?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (selectedOption == DialogResult.Yes)
            {
                txtCantidad.Text = txtcodigo.Text = txtDispenso.Text = lblNomRef.Text = lblNomUsuario.Text = lblMedida.Text = "";
            }
            else
            {
                Expota_PDF();
                limpiar();
            }
        }

        void obtenerNombre()
        {
            MySqlCommand sql = new MySqlCommand("SELECT CONCAT(coalesce(t1.ApPaterno,''),' ',coalesce(t1.ApMaterno,''),' ',coalesce(t1.nombres,'')) AS almacenista, t2.puesto,t1.idPersona as id,t2.idpuesto FROM cpersonal as t1 INNER JOIN puestos AS t2 ON t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal =t1.idpersona WHERE t3.password='" + v.Encriptar(txtDispenso.Text) + "' AND  t1.status='1' AND t2.status='1' and t1.empresa='" + empresa + "' ;", v.c.dbconection());
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
                lblNomUsuario.Text = cmd.GetString("Almacenista").ToString();
                idEntrega = Convert.ToInt32(cmd.GetString("id").ToString());
            }
        }

        public void limpiar()
        {
            DataTable tb = new DataTable();
            txtCantidad.Text = txtcodigo.Text = txtDispenso.Text = lblNomRef.Text = lblNomUsuario.Text = txtNomMecanico.Text = txtMotivo.Text = lblMedida.Text = ""; 
            txtNomMecanico.Visible = label4.Visible = false; cmbMecanico.Visible = true;
            existencia = 0.0;
            while (dgImprimir.RowCount > 0)
            {
                dgImprimir.Rows.Remove(dgImprimir.CurrentRow);
            }
            CargarMecanico();
        }
        public void Expota_PDF()
        {
            byte[] img = null;
            Chunk chunk = new Chunk();
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
                    else if (empresa == 3)
                    {
                        img = Convert.FromBase64String(v.trainsumos);
                    }
                   
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    if (empresa == 2 )
                    {
                        chunk = new Chunk("REPORTE TRI ALMACEN", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    }
                    else if (empresa == 3 )
                    {
                        chunk = new Chunk("REPORTE TRANSINSUMOS ALMACEN", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    }

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
                    tabla.AddCell(v.valorCampo("MATERIAL PARA :", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(Convert.ToString(folio), 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("PRODUCCION", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("FECHA DE ENTREGA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(dtFecha.Value.ToString("yyyy-MM-dd"), 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("MECANICO QUE SOLICITA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(cmbMecanico.SelectedValue.ToString(), 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial));
                    /* tabla.AddCell(v.valorCampo("ENTREGA", 2, 0, 0, arial));
                     tabla.AddCell(v.valorCampo(lblNomRef.Text, 1, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo(lblNomUsuario.Text, 1, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("COMENTARIOS", 2, 0, 0, arial));
                     tabla.AddCell(v.valorCampo("", 2, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));*/




                    /*tabla.AddCell(v.valorCampo("REFACCIONES SOLICITADAS", 2, 1, 0, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
                    tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));*/
                    doc.Add(tabla);
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
        public void GenerarDocumento(Document document)
        {
            int i, j;
            iTextSharp.text.Font arial2 = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
            PdfPTable tabla1 = new PdfPTable(2);
            tabla1.DefaultCell.Border = 0;
            tabla1.WidthPercentage = 100;
            tabla1.AddCell(v.valorCampo("REFACCIONES SOLICITADAS", 2, 1, 0, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
            tabla1.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));


            PdfPTable datatable = new PdfPTable(dgImprimir.ColumnCount);
            datatable.DefaultCell.Padding = 4;


            float[] headerwidths = GetTamañoColumnas(dgImprimir);
            datatable.SetWidths(headerwidths);
            Color color = Color.PaleGreen;
            datatable.WidthPercentage = 100;
            PdfPCell observaciones = new PdfPCell();
            datatable.DefaultCell.BorderWidth = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < dgImprimir.ColumnCount; i++)
            {
                datatable.AddCell(new Phrase(dgImprimir.Columns[i].HeaderText.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
            }
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(250, 250, 250);
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < dgImprimir.RowCount; i++)
            {
                for (j = 0; j < dgImprimir.ColumnCount; j++)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(dgImprimir[j, i].Value.ToString(), FontFactory.GetFont("ARIAL", 8)));
                    celda.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                    /* if (j == 5 && dgAgregados[j, i].Value.ToString() == "EXISTENCIA")
                         celda.BackgroundColor = new iTextSharp.text.BaseColor(Color.PaleGreen);
                     else
                         celda.BackgroundColor = new iTextSharp.text.BaseColor(Color.LightCoral);*/
                    if (dgImprimir[j, i].Value != null)
                        datatable.AddCell(celda);

                }
                datatable.CompleteRow();
            }

            datatable.AddCell(observaciones);
            document.Add(tabla1);
            document.Add(datatable);
            document.Add(new Paragraph("\n\n\n\nOBSERVACIONES:", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph("\n" + txtMotivo.Text, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
        }
        public float[] GetTamañoColumnas(DataGridView dg)
        {
            float[] values = new float[dg.ColumnCount];
            for (int i = 1; i < dgImprimir.ColumnCount; i++)
            {
                values[i] = (float)dg.Columns[i].Width;
            }
            return values;
        }
    }
}
