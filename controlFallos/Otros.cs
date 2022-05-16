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


namespace controlFallos
{
    public partial class Otros : Form
    {
        validaciones v;
        int idUsuario, empresa, area, Fn=0, otros = 0;
        double existencia = 0.0;
        string idEntrega ="" , unidadImprime = "", Folio = "", codigo ="", fecha ="", observaciones = "",cantidad = "", nombreRef="",usuario="", Mecanico="", Mecanico_Principal="";
        string []unidad;
        public Otros()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Otros_Load(object sender, EventArgs e)
        {
            cargaEcoBusq();
            CargarMecanico();
            cmbUnidad.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbMecanico.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbSalida.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dtFecha.MaxDate = DateTime.Now;
            obtenerFolio();
            v.comboswithuot(cmbSalida, new string[] { "--SELECCIONA TIPO SALIDA--", "SALIDA ALMACEN", "VENTA EXTERNA" });
            //cargarDatos();

        }

        private void obtenerFolio()
        {
            Folio = v.getaData("select count(idccarrocero) from ccarrocero").ToString();
            Fn = int.Parse(Folio) + 1;
        }

        public Otros(int idusuario, int empresa, int area, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idusuario;
        }
        public void cargaEcoBusq()
        {
            cmbUnidad.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';select convert(idunidad,char) as idunidad, convert(concat(t2.identificador,LPAD(consecutivo,4,'0'),'-', descripcioneco),char) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas order by eco");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "--SELECCIONE ECONÓMICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbUnidad.DisplayMember = "eco";
            cmbUnidad.ValueMember = "idunidad";
            cmbUnidad.DataSource = dt;
        }
        public void CargarMecanico()
        {
            cmbMecanico.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';SELECT DISTINCT convert(t2.idPersona, char) id,convert(UPPER(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,''))),char) AS Nombre FROM  cpersonal as t2  where t2.empresa='"+ empresa +"' and t2.area = '1'  and t2.cargofkcargos != '2' and t2.status = '1' ORDER BY CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')) asc;");
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila2 = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["Nombre"] = "--SELECCIONE MECANICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            nuevaFila2["id"] = 8000000;
            nuevaFila2["Nombre"] = "OTRO".ToUpper();
            dt.Rows.InsertAt(nuevaFila2, dt.Rows.Count+1);
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

        private void txtcodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paracodrefaccion(e);
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                buscaref(txtcodigo.Text);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbMecanico.SelectedIndex > 0)
            {
                if (otros == 0)
                {
                    Mecanico = cmbMecanico.SelectedValue.ToString();
                }
                else
                {
                    Mecanico = txtNomMecanico.Text;
                }

                validarCantidad(txtcodigo.Text, txtCantidad.Text);
            }
            else
            {
                MessageBox.Show("Seleccione al personal de mantenimiento", "!!Alerta¡¡", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        void validarCantidad(string codigo, string cantidad)
        {
            if (v.ValidarCantidad(codigo, cantidad, empresa))
            {
                Guardar();
            }
            else
            {
                MessageBox.Show("Cantidad De insumos insuficientes", "!!Alerta¡¡", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        public void Guardar()
        {
            

            if (v.validadOtros(txtcodigo.Text, txtCantidad.Text, txtDispenso.Text, cmbUnidad.SelectedValue.ToString(), lblNomRef.Text, Mecanico.ToString(), textBoxObservaciones.Text, Convert.ToInt32(cmbSalida.SelectedValue), existencia))
            {
                double existencianueva = existencia - Convert.ToDouble(txtCantidad.Text);
                v.Carroceros("insert into ccarrocero (refaccionfkCRefacciones, unidadfkCUnidades, CantidadEntregada,usuariofkCPersonal,FechaHora,Empresa,mecanico,observacion, TipoSalida) value ((Select idrefaccion from crefacciones where codrefaccion = '" + txtcodigo.Text + "' and empresa = '" + empresa + "'), '" + cmbUnidad.SelectedValue.ToString() + "', '" + txtCantidad.Text + "', '" + idEntrega.ToString() + "' , now(), '" + empresa + "', '" + Mecanico.ToString() + "','" + textBoxObservaciones.Text + "', '" + cmbSalida.SelectedValue + "')");
                v.CarroceroE("update crefacciones  set existencias='" + existencianueva + "' Where codrefaccion = '" + txtcodigo.Text + "' and empresa = '" + empresa + "'");
                MessageBox.Show("Refaccion agregada correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var selectedOption = MessageBox.Show("¿Desea continuar creando folios de la unidad actual?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (selectedOption == DialogResult.Yes)
                {
                    dgImprimir.Rows.Add(cmbUnidad.Text, txtcodigo.Text, lblNomRef.Text, txtCantidad.Text, lblMedida.Text, dtFecha.Value.ToString("yyyy-MM-dd"), Mecanico.ToString(), lblNomUsuario.Text);
                    txtCantidad.Text = txtcodigo.Text = txtDispenso.Text = lblNomRef.Text = lblNomUsuario.Text = "";
                    cmbUnidad.Enabled = false;
                }
                else
                {
                    dgImprimir.Rows.Add(cmbUnidad.Text, txtcodigo.Text, lblNomRef.Text, txtCantidad.Text, lblMedida.Text, dtFecha.Value.ToString("yyyy-MM-dd"),Mecanico.ToString() ,lblNomUsuario.Text);
                    unidadImprime = cmbUnidad.Text;
                    Expota_PDF();
                    limpiar();
                }

            }
        }
        public void cargarDatos()
        {
            MySqlDataAdapter DT = new MySqlDataAdapter("Select convert(concat(t1.consecutivo, '-', t1.descripcioneco),char) as Eco, t3.codrefaccion as Codigo, t3.nombreRefaccion as Nombre, t2.CantidadEntregada as Cantidad, t2.FechaHora as Fecha, t4.usuario as Entrega from cunidades as t1 inner join ccarrocero as t2 on t1.idunidad = t2.unidadfkCUnidades inner join crefacciones as t3 on t3.idrefaccion = t2.refaccionfkCRefacciones inner join datosistema as t4 on t4.usuariofkcpersonal = t2.usuariofkCPersonal where left(t2.FechaHora,10) = curdate()", v.c.dbconection());
            DataSet ds = new DataSet();
            DT.Fill(ds);
            dgAgregados.DataSource = ds.Tables[0];
        }

        private void txtDispenso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
               obtenerNombre();
            }
        }

        private void cmbUnidad_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbSalida_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtcodigo_Validated(object sender, EventArgs e)
        {
            buscaref(txtcodigo.Text);
        }

        private void txtDispenso_Validated(object sender, EventArgs e)
        {
            obtenerNombre();
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.numerosDecimales(e);
        }

        private void cbMecanico1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMecanico1.Checked == true)
            {
                Mecanico_Principal = cmbMecanico.SelectedValue.ToString();
                cbMecanico1.Enabled = false;
            }
            
        }

        private void cmbMecanico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMecanico.SelectedValue != null)
            {
                if (cmbMecanico.SelectedValue.ToString().Equals("OTRO"))
                {
                    cmbMecanico.Visible = false;
                    label4.Visible = txtNomMecanico.Visible = true;
                    otros = 1;
                }
            }
           
        }

        private void cmbMecanico_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        public void obtenerNombre()
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
                idEntrega = cmd.GetString("id").ToString();
            }
        }

        private void dgImprimir_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult edition = DialogResult.OK;
                var selectedOption = MessageBox.Show("¿Esta seguro de que deseea eliminar la refacción?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (selectedOption == DialogResult.Yes)
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    edition = obs.ShowDialog();
                    if (edition == DialogResult.OK)
                    {
                        observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        this.dgImprimir.Rows.RemoveAt(dgImprimir.CurrentRow.Index);
                        eliminar_dato();
                    }
                    
                }
            }
        }

        public void eliminar_dato()
        {
            v.c.insertar("update ccarrocero as t1 inner join crefacciones as t2 on t1.refaccionfkCRefacciones = t2.idrefaccion inner join cunidades as t3 on t1.unidadfkCUnidades = t3.idunidad set cancelado = '1' where t2.codrefaccion = '"+codigo.ToString()+"' and t3.idunidad = '"+cmbUnidad.SelectedValue+ "' and date_format(t1.FechaHora, '%Y-%m-%d') = '" + fecha.ToString()+ "' and t1.Empresa='" + empresa + "'");
            v.c.insertar("update crefacciones  set existencias=(existencias +'" + cantidad.ToString() + "') Where codrefaccion = '" + codigo.ToString() + "' and empresa = '" + empresa + "'");

            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Carrocero/Produccion',(select t1.idccarrocero from ccarrocero as t1 inner join crefacciones as t2 on t1.refaccionfkCRefacciones = t2.idrefaccion inner join cunidades as t3 on t3.idunidad = t1.unidadfkCUnidades where t2.codrefaccion = '" + codigo + "' and t3.idunidad ='" + cmbUnidad.SelectedValue + "' and date_format(t1.fechaHora, '%Y-%m-%d') = '" + fecha.ToString() + "'),'" + codigo + ";" + nombreRef + ";" + cantidad + ";" + usuario + "','" + idUsuario + "',NOW(),'Quitar Refaccion','" + observaciones + "','" + empresa + "','" + area + "')");
        }
        private void dgImprimir_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            codigo = dgImprimir.Rows[e.RowIndex].Cells[1].Value.ToString();
            nombreRef = dgImprimir.Rows[e.RowIndex].Cells[2].Value.ToString();
            fecha = dgImprimir.Rows[e.RowIndex].Cells[4].Value.ToString();
            cantidad = dgImprimir.Rows[e.RowIndex].Cells[3].Value.ToString();
            usuario = dgImprimir.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        
        {
            this.Close();
        }
        public void limpiar()
        {
            DataTable tb = new DataTable();
            txtCantidad.Text = txtcodigo.Text = txtDispenso.Text = lblNomRef.Text = lblNomUsuario.Text = txtNomMecanico.Text= textBoxObservaciones.Text = "";
            txtNomMecanico.Visible = label4.Visible = false; cmbMecanico.Visible = true;
            existencia = 0.0;
            cbMecanico1.Enabled = true;
            cbMecanico1.Checked = false;
            while (dgImprimir.RowCount > 0)
            {
                dgImprimir.Rows.Remove(dgImprimir.CurrentRow);
            }
            cmbUnidad.Enabled = true;
            cargaEcoBusq();
            cargarDatos();
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
                    if (empresa == 2 && Convert.ToInt32(cmbSalida.SelectedValue) == 1)
                    {
                        img = Convert.FromBase64String(v.tri);
                    }
                    else if (empresa == 3 && Convert.ToInt32(cmbSalida.SelectedValue) == 1)
                    {
                        img = Convert.FromBase64String(v.trainsumos);
                    }
                    else if (Convert.ToInt32(cmbSalida.SelectedValue) == 2)
                    {
                        img = Convert.FromBase64String(v.trainsumos);
                    }
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    if (empresa == 2 && Convert.ToInt32(cmbSalida.SelectedValue) == 1)
                    {
                        chunk = new Chunk("REPORTE TRI ALMACEN", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    }
                    else if (empresa == 3 && Convert.ToInt32(cmbSalida.SelectedValue) == 1)
                    {
                        chunk = new Chunk("REPORTE TRANSINSUMOS ALMACEN", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    }
                    else if (Convert.ToInt32(cmbSalida.SelectedValue) == 2)
                    {
                        chunk = new Chunk("REPORTE VENTA ALMACEN", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
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
                    tabla.AddCell(v.valorCampo("UNIDAD", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(Convert.ToString(Fn), 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(unidadImprime, 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("FECHA DE ENTREGA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(dtFecha.Value.ToString("yyyy-MM-dd"), 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("MECANICO QUE SOLICITA", 1, 0, 0, arial));
                    if (cbMecanico1.Checked == true)
                    {
                        tabla.AddCell(v.valorCampo(Mecanico_Principal.ToString(), 1, 0, 0, arial2));
                    }
                    else
                    {
                        tabla.AddCell(v.valorCampo(Mecanico.ToString(), 1, 0, 0, arial2));
                    }
                    
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
            document.Add(new Paragraph("\n" + textBoxObservaciones.Text, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
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

        private void cmbMecanico_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        
    }
}
