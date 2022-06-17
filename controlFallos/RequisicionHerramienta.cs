using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using h = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;

namespace controlFallos
{
    public partial class RequisicionHerramienta : Form
    {
        validaciones v;
        Thread excel;
        int idUsuario, empresa, area;
        string idUsuarioR = "", codigoanterior = "", datosO = "";
        Point arreglar = new Point(33, 273);
        Point locationInitial = new Point(348, 6);
        Size initialSize = new Size(800, 397);
        Point asolicitar = new Point(348, 6);
        Point solicitadas = new Point(348, 205);
        byte[] img;
        delegate void uno();
        delegate void dos();
        bool editar, editarRefaccion, isexporting, aux;
        DataGridView informacion_imprimir = new DataGridView();
        string Consulta = "Select convert(t1.Folio,char) as FOLIO, convert(t1.Herramienta, char) as 'Nombre Herramienta', convert(t1.NumParte,char) as 'Numero de Parte',   convert(t1.Cantidas, char) as 'CANTIDAD SOLICITADA', if(t1.estatus = 0,'En Espera', if(t1.estatus=1, 'Aprobada', if(t1.estatus=2, 'Rechazada', if(t1.estatus = '', '','')))) as 'Estatus', convert(t1.FechaHora, char) as 'Fecha De solicitud', convert(t1.precio,char) 'Precio de Compra', convert(t4.Simbolo, char) as 'MONEDA', convert(t1.Especificacion,char) as 'Especificaiones' from crequicisionherramienta as t1 INNER JOIN ctipocambio as t4 on t1.TipoCambio = t4.idtipocambio";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtvisual = new DataTable();
        DataRow filas;
        DataColumn columnas;
        MySqlDataAdapter adaptador = new MySqlDataAdapter();
        public RequisicionHerramienta(int empresa, int area, int idUsuario, validaciones v)
        {
            this.v = v;
            this.idUsuario = idUsuario;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            cargrdepartamento();
            Principal();
            iniMonedaCambio();
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "ENRO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" });

        }

        private void btnAgregarRefaccion_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                agregarnuevo();
            }
            else
            {
                Editar();
            }
        }

        private void btnBuscar(object sender, EventArgs e)
        {
            metodos_de_busqueda();
        }

        void iniMonedaCambio()
        {
            v.iniCombos("select idTipoCambio as id, Simbolo from ctipocambio order by simbolo desc", cmbMoneda, "id", "Simbolo", "--MONEDA--");
        }
        void cargrdepartamento() { v.iniCombos("SET lc_time_names = 'es_ES';select convert(idcciglasreqh,char) as idCigla, convert(Departamento, char) as usu from cciglasreqh where empresa = '" + empresa + "'", cmbDepartamento, "idCigla", "usu", "--DEPARTAMENTO--"); }
        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.numerosDecimales(e);
        }

        private void txtCantidad_Validated(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCantidad.Text.Trim()))
            {
                while (txtCantidad.Text.Contains(".."))
                    txtCantidad.Text = txtCantidad.Text.Replace("..", ".").Trim();
                txtCantidad.Text = txtCantidad.Text.Trim();
                txtCantidad.Text = string.Format("{0:F2}", txtCantidad.Text);
                try
                {
                    if (Convert.ToDouble(txtCantidad.Text) > 0)
                    {
                        CultureInfo ti = new CultureInfo("es-MX"); ti.NumberFormat.CurrencyDecimalDigits = 2; ti.NumberFormat.CurrencyDecimalSeparator = "."; txtCantidad.Text = string.Format("{0:N2}", Convert.ToDouble(txtCantidad.Text, ti));
                    }
                    else txtCantidad.Text = "0";
                }
                catch (Exception ex)
                {
                    txtCantidad.Clear(); MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void cmbDepartamento_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }


        private void ObtenrFolios(object sender, EventArgs e)
        {
            foilios();
        }

        private void dtgRequicision_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DobeGridview(e);
            }
        }

        private void txtRefaccion_TextChanged(object sender, EventArgs e)
        {
            btnCancelar.Enabled = true;
            v.mayusculas(txtRefaccion.Text);
        }

        private void txtContrasenna_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumerossinespacios(e);
        }

        private void txtContrasenna_Validated(object sender, EventArgs e)
        {
            if (Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join puestos as t2 on t1.cargofkcargos=t2.idpuesto inner join datosistema as t3 on t3.usuariofkcpersonal=t1.idPersona where t3.password='" + v.Encriptar(txtContrasenna.Text.Trim()) + "' and t1.empresa='" + empresa + "' and t1.area='" + area + "' and t1.status='1';")) > 0)
            {
                string[] datos = v.getaData("select upper(concat(coalesce(t1.ApPaterno,''),' ',coalesce(t1.ApMaterno,''),' ',coalesce(t1.nombres,''),'/',t1.idpersona)) from cpersonal as t1 inner join datosistema as t2 on t2.usuariofkcpersonal=t1.idPersona where t2.password='" + v.Encriptar(txtContrasenna.Text.Trim()) + "'").ToString().Split('/');
                lblNombreU.Text = datos[0];
                idUsuarioR = datos[1];
            }
            else
                lblNombreU.Text = idUsuarioR = "";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var selectedOption = MessageBox.Show("¿Seguro que quiere cancelar el registro?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (selectedOption == DialogResult.Yes)
            {
                limpiar();
            }
        }
        private void btnexportar_Click(object sender, EventArgs e)
        {
        //    excel = new Thread(new ThreadStart(exportar_excel));
        //    excel.Start();

            exportar_excel();
        }
        public void Principal()
        {
            dtgRequicision.ClearSelection();
            adaptador = v.getReport(Consulta + " where DATE_FORMAT(t1.FechaHora, '%Y/%M/%d') = DATE_FORMAT(now(), '%Y/%M/%d') and t1.Empresa = '" + empresa + "' ORDER BY t1.FechaHora DESC");
            adaptador.Fill(ds);
            dtgRequicision.DataSource = ds.Tables[0];
        }

        void agregarnuevo()
        {
            if (!v.camposVaciosSolicitudRefacciones(txtRefaccion.Text, txtCantidad.Text, txtNumParte.Text, txtEspecificaciones.Text, txtCosto.Text, lblNombreU.Text, txtFolio.Text, cmbMoneda.SelectedIndex))
            {
                var selectedOption = MessageBox.Show("¿Desea Agregar Más Productos a la Requisición?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (selectedOption == DialogResult.Yes)
                {
                    registrar();
                    limpiar();
                }
                else
                {
                    var selectedOption2 = MessageBox.Show("¿Desea Finalizar la Orden de Requicisión?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (selectedOption2 == DialogResult.Yes)
                    {
                        registrar();
                        agregaarDataset();
                        Expota_PDF();
                        limpiar_Definitivo();
                    }
                    else
                    {
                        registrar();
                        limpiar();
                        foilios();
                    }
                }
            }
        }
        void Editar()
        {
            if (editar)
            {
                if (!v.camposVaciosSolicitudRefacciones(txtRefaccion.Text, txtCantidad.Text, txtNumParte.Text, txtEspecificaciones.Text, txtCosto.Text, lblNombreU.Text, txtFolio.Text, cmbMoneda.SelectedIndex))
                {
                    //AgregarVisual();
                    var selectedOption = MessageBox.Show("¿Desea Agregar Más Productos a la Requisición?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (selectedOption == DialogResult.Yes)
                    {
                        editarRegistro();
                        limpiar();
                    }
                    else
                    {
                        var selectedOption2 = MessageBox.Show("¿Desea Finalizar la Orden de Requicisión?", "¡¡IMPORTANTE!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (selectedOption2 == DialogResult.Yes)
                        {
                            editarRegistro();
                            agregaarDataset();
                            Expota_PDF();
                            limpiar_Definitivo();
                        }
                        else
                        {
                            editarRegistro();
                            limpiar();
                            foilios();
                        }
                    }

                }
            }
        }
        public void registrar()
        {
            datosO = datosO + "|" + txtRefaccion.Text + "|" + txtNumParte.Text + "|" + txtEspecificaciones.Text + "|" + txtExistencia.Text + "|" + txtCantidad.Text;
            v.AgregarRequicision("insert into crequicisionherramienta(Folio, Cantidas, numparte, Especificacion, heramientafkHerramienta, FechaHora, precio, Empresa, usuariofkPersonal,TipoCambio,Departamento) value('" + txtFolio.Text + "','" + txtCantidad.Text + "','" + txtNumParte.Text + "','" + txtEspecificaciones.Text + "',(select idrefaccion from crefacciones where codrefaccion = '" + txtRefaccion.Text + "' and empresa = '" + empresa + "'), now(), '" + txtCosto.Text + "','" + empresa + "','" + idUsuarioR.ToString() + "','" + cmbMoneda.SelectedValue + "','" + cmbDepartamento.SelectedValue + "' )");
        }
        public void editarRegistro()
        {
            datosO = datosO + "|" + txtRefaccion.Text + "|" + txtNumParte.Text + "|" + txtEspecificaciones.Text + "|" + txtExistencia.Text + "|" + txtCantidad.Text;
            v.AgregarRequicision("update crequicision set Cantidas= '" + txtCantidad.Text + "',numparte ='" + txtNumParte.Text + "', Especificacion ='" + txtEspecificaciones.Text + "',fecha=now(),precio='" + txtCosto.Text + "', refaccionfkCRefacciones=(select idcherramienta from cherramienta where Nombre = '" + txtRefaccion.Text + "' and empresa = '" + empresa + "'), usuariofkCPersonal ='" + idUsuarioR.ToString() + "',tipocambiofkCTipomoneda = '" + cmbMoneda.SelectedValue + "' where Folio = '" + txtFolio.Text + "' and refaccionfkCRefacciones = (select idrefaccion from crefacciones where codrefaccion = '" + codigoanterior.ToString() + "' and empresa = '" + empresa + "')");
        }
        public void limpiar()
        {
            txtCantidad.Text = txtNumParte.Text = txtEspecificaciones.Text = txtCosto.Text = txtRefaccion.Text = txtExistencia.Text = lblCosto.Text = txtContrasenna.Text = lblNombreU.Text = txtProveedor.Text = "";
            cmbDepartamento.Enabled = false;
            Principal();
            btnCancelar.Enabled = false;
            panel2.Visible = false;
            DataTable dtL = new DataTable();

        }

        

        public void limpiar_Definitivo()
        {
            DataTable dtL = new DataTable();
            while (dtgRequicision.RowCount > 0)
            {
                dtgRequicision.Rows.Remove(dtgRequicision.CurrentRow);
            }
            txtCantidad.Text = txtNumParte.Text = txtEspecificaciones.Text = txtCosto.Text = txtRefaccion.Text = txtExistencia.Text = lblCosto.Text = txtContrasenna.Text = lblNombreU.Text = txtProveedor.Text = txtFolio.Text = "";
            cargrdepartamento();
            cmbDepartamento.Enabled = true;
            Principal();
            datosO = "";
            dtgRequicision.ClearSelection();
            btnCancelar.Enabled = false;
            panel2.Visible = false;
            dt.Rows.Clear();
            dt.Columns.Clear();
        }

        

        public void foilios()
        {
            string cigla = "";
            if (!string.IsNullOrEmpty(cigla = v.ObtenerCiglas(empresa, cmbDepartamento.Text)))
            {
                string consecutivo = v.FoliosRh(empresa, cmbDepartamento.SelectedValue.ToString());

                txtFolio.Text = cigla + "-" + consecutivo;
            }
        }

      

        public void agregaarDataset()
        {
            //string medida = v.getaData(unidadMedida + "'" + txtCodigo.Text + "'").ToString();
            if (dt.Columns.Count == 0)
            {
                /*columnas = new DataColumn();
                columnas.ColumnName = "Codigo Refacción";
                dt.Columns.Add(columnas);*/
                columnas = new DataColumn();
                columnas.ColumnName = "Nombre Refacción";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Numero De Parte";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Especificación";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Existencia";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Cantidad Solicitada";
                dt.Columns.Add(columnas);
                columnas = new DataColumn();
                columnas.ColumnName = "Unidad Medida";
                dt.Columns.Add(columnas);

            }
            string[] arr = datosO.ToString().Split('|');
            for (int i = 1; i < arr.Count();)
            {
                filas = dt.NewRow();
                //filas["Codigo Refacción"] = arr[i].ToString();
                filas["Nombre Refacción"] = arr[i].ToString();
                filas["Numero De Parte"] = arr[i + 2].ToString();
                filas["Especificación"] = arr[i + 2].ToString();
                filas["Existencia"] = arr[i + 3].ToString();
                filas["Cantidad Solicitada"] = arr[i + 4].ToString();
                filas["Unidad Medida"] = "PZ";
                dt.Rows.Add(filas);
                i = i + 5;
            }
            DataSet dst = new DataSet();
            dst.Tables.Add(dt.Copy());
            dataGridView2.DataSource = dst.Tables[0];
        }
        public void Expota_PDF()
        {

            //Código para generación de archivo pdf
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(20f, 20f, 10f, 10f);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ValidateNames = true;
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Requerimiento de Material";
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
                    else
                    {
                        img = Convert.FromBase64String(v.trainsumos);
                    }

                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 150 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    Chunk chunk = new Chunk("REQUERIMIENTO DE MATERIAL", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                    doc.Add(imagen);
                    doc.Add(new Paragraph(chunk));
                    doc.Add(new Paragraph("                                    "));
                    PdfPTable tabla = new PdfPTable(2);
                    tabla.DefaultCell.Border = 0;
                    tabla.WidthPercentage = 100;
                    /*
                     t1.Folio,'|',UNIDAD,'|',FECHA Y HORA,'|',MECANICO,'|',FECHAHORA ENTREGA,'|',PERSONA QUE ENTREGA,'|',FolioFactura,'|',ObservacionesTrans
                     */
                    tabla.AddCell(v.valorCampo("FOLIO DEL REQUERIMIENTO", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("FECHA", 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo(txtFolio.Text, 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo(dtpFecha.Value.ToString("yyyy-MM-dd"), 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
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

        private void txtExistencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.SoloNumers(e);
        }

        private void txtCosto_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.numerosDecimales(e);
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


            PdfPTable datatable = new PdfPTable(dataGridView2.ColumnCount);
            datatable.DefaultCell.Padding = 4;


            float[] headerwidths = GetTamañoColumnas(dt);
            datatable.SetWidths(headerwidths);
            System.Drawing.Color color = System.Drawing.Color.PaleGreen;
            datatable.WidthPercentage = 100;
            PdfPCell observaciones = new PdfPCell();
            datatable.DefaultCell.BorderWidth = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < dataGridView2.ColumnCount; i++)
            {
                datatable.AddCell(new Phrase(dataGridView2.Columns[i].HeaderText.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
            }
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(250, 250, 250);
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < dataGridView2.RowCount; i++)
            {
                for (j = 0; j < dataGridView2.ColumnCount; j++)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(dataGridView2[j, i].Value.ToString(), FontFactory.GetFont("ARIAL", 8)));
                    celda.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                    /* if (j == 5 && dgAgregados[j, i].Value.ToString() == "EXISTENCIA")
                         celda.BackgroundColor = new iTextSharp.text.BaseColor(Color.PaleGreen);
                     else
                         celda.BackgroundColor = new iTextSharp.text.BaseColor(Color.LightCoral);*/
                    if (dataGridView2[j, i].Value != null)
                        datatable.AddCell(celda);

                }
                datatable.CompleteRow();
            }
            datatable.AddCell(observaciones);
            document.Add(tabla1);
            document.Add(datatable);
            document.Add(new Paragraph("\n\n\nELABORO: _________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph("\n\n\nSUPERVISO: _________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
            document.Add(new Paragraph("\n\n\nBoVo: _________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
        }
        public float[] GetTamañoColumnas(DataTable dg)
        {
            float[] values = new float[dataGridView2.ColumnCount];
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                values[i] = (float)dataGridView2.Columns[i].Width;
            }
            return values;
        }
        void DobeGridview(DataGridViewCellEventArgs e)
        {
            codigoanterior = dtgRequicision.Rows[e.RowIndex].Cells[1].Value.ToString();
            string folio = dtgRequicision.Rows[e.RowIndex].Cells[0].Value.ToString();
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';select concat(convert(t1.Folio, char), '|', convert(t1.Cantidas,char),'|', convert(t1.Numparte, char),'|', convert(t1.Especificacion,char),'|', convert(t1.Precio,char),'|', convert(t1.Herramienta,char), '|', convert(t1.TipoCambio,char))  from crequicisionherramienta as t1  where t1.Herramienta = '" + codigoanterior + "' and t1.Folio  = '" + folio + "' and t1.Empresa  = '" + empresa + "'").ToString().Split('|');
            txtFolio.Text = datos[0].ToString();
            txtCantidad.Text = datos[1].ToString();
            txtNumParte.Text = datos[2].ToString();
            txtEspecificaciones.Text = datos[3].ToString();
            txtCosto.Text = datos[4].ToString();
            txtRefaccion.Text = datos[5].ToString();
            //lblCosto.Text = datos[7].ToString();
            //cmbDepartamento.SelectedValue = int.Parse(datos[13].ToString());
            cmbMoneda.SelectedValue = int.Parse(datos[6].ToString());
            cmbDepartamento.Enabled = false;
            panel2.Visible = true;
            btnEliminar.Enabled = true;
            editar = true;
        }
        public void metodos_de_busqueda()
        {
            dtgRequicision.ClearSelection();
            ds.Clear();
            if (txtFolioB.Text != "")
            {
                adaptador = v.getReport(Consulta + " where t1.folio = '" + txtFolioB.Text + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
            }
            else if (txtCodigoB.Text != "")
            {
                adaptador = v.getReport(Consulta + " where t2.Nombre = '" + txtCodigoB.Text + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
            }
            else if (cmbMes.SelectedIndex != 0)
            {
                string messel = "";
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }

                adaptador = v.getReport(Consulta + " where date_format(convert(t1.FechaHora, char), '%m') = '" + messel + "'and t1.empresa = '" + empresa + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];

            }

            else if (cbFecha.Checked == true)
            {
                adaptador = v.getReport(Consulta + "   where date_format(convert(t1.FechaHora, char),'%d/%m/%Y') between'" + dtpFechaDe.Value.ToString("dd/MM/yyyy") + "' and '" + dtpFechaA.Value.ToString("dd/MM/yyyy") + "' and t1.empresa = '" + empresa + "' ORDER BY t1.fecha DESC");
                adaptador.Fill(ds);
                dtgRequicision.DataSource = ds.Tables[0];
                cbFecha.Checked = false;
                dtpFechaA.Enabled = dtpFechaDe.Enabled = false;
            }
            else
            {
                MessageBox.Show("No hay parametros de busqueda", "Inportante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        void inicio()
        {
            btnexportar.Visible = !(pbgif.Visible = true);
            lblexcel.Text = "Exportando";
        }
        void termino()
        {
            btnexportar.Visible = !(pbgif.Visible = false);
            lblexcel.Text = "Exportar";
        }
        void exportar_excel()
        {
            /*
                        if (dtgRequicision.Rows.Count > 0)
                        {
                            isexporting = true;
                            dt = (DataTable)dtgRequicision.DataSource;
                            if (this.InvokeRequired)
                            {
                                uno delega = new uno(inicio);
                                this.Invoke(delega);
                            }
                            Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                            X.Application.Workbooks.Add(Type.Missing);
                            h.Worksheet sheet = X.ActiveSheet;
                            X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            for (int i = 1; i < dt.Columns.Count; i++)
                            {
                                h.Range rng = (h.Range)sheet.Cells[1, i];
                                sheet.Cells[1, i] = dt.Columns[i].ColumnName.ToUpper();
                                rng.Interior.Color = System.Drawing.Color.Crimson;
                                rng.Borders.Color = System.Drawing.Color.Black;
                                rng.Font.Color = System.Drawing.Color.White;
                                rng.Cells.Font.Name = "Calibri";
                                rng.Cells.Font.Size = 12;
                                rng.Font.Bold = true;
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                for (int j = 1; j < dt.Columns.Count; j++)
                                {
                                    try
                                    {
                                        h.Range rng = (h.Range)sheet.Cells[i + 2, j];
                                        sheet.Cells[i + 2, j] = dt.Rows[i][j].ToString();
                                        rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                                        rng.Cells.Font.Name = "Calibri";
                                        rng.Cells.Font.Size = 11;
                                        rng.Font.Bold = false;
                                        rng.Interior.Color = Color.FromArgb(231, 230, 230);
                                    }
                                    catch (System.NullReferenceException EX)
                                    { MessageBox.Show(EX.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                                }
                            }
                            X.Columns.AutoFit();
                            X.Rows.AutoFit();
                            X.Visible = true;
                            if (this.InvokeRequired)
                            {
                                dos delega2 = new dos(termino);
                                this.Invoke(delega2);
                            }
                            excel.Abort();
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
            foreach (DataGridViewColumn column in dtgRequicision.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;


            }



            int ir = 9;
            foreach (DataGridViewRow row in dtgRequicision.Rows)
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
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Requerimiento Refacciones");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "H" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "CONSULTA REQUERIMIENTO DE REFACCIONES");
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
            sl.SetCellStyle("B" + celdaInicial, "H" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "AJ");
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

    }
}
