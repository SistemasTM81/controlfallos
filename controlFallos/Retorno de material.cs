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
using System.Threading;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;

namespace controlFallos
{
    public partial class Retorno_de_material : Form
    {
        validaciones v;
        Thread exportar;
        byte[] img;
        int idUsuario, empresa, area, otro = 0;
        string idEntrega, Folio, codigo, refaccion, economico, cantRefaccion;
        string where = "where  date_format(t2.FechaHora, '%Y-%m-%d') between date_format(now(), '%Y-%m-%d') and date_format(now()-1, '%Y-%m-%d')";
        public Retorno_de_material()
        {
            InitializeComponent();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtcodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                buscarF(txtcodigo.Text);
            }
        }

        private void txtDispenso_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (cmbMecanico.SelectedIndex > 0)
            {

                Agregar();
            }
            else
            {
                MessageBox.Show("Seleccione al personal de mantenimiento", "!!Alerta¡¡", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigobusq.Text))
            {
                material_egresado("where t1.codrefaccion = '" + txtCodigobusq.Text + "' and t2.empresa = '" + empresa + "'");
            }
            else if (!string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text))
            {
                material_egresado("where t1.nombreRefaccion = '" + txtnombrereFaccionbusq.Text + "' and t2.empresa = '" + empresa + "'");
            }
            else if (chbFecha.Checked == true)
            {
                material_egresado("where date_format(t2.FechaHora, '%Y-%m-%d') between '" + dtpFechaInico.Value.ToString("yyyy-MM-dd") + "'and'" + dtpFechaTermino.Value.ToString("yyyy-MM-dd") + "'");
            }
            else if (!string.IsNullOrWhiteSpace(txtCodigobusq.Text) && !string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text))
            {
                material_egresado("where t1.codrefaccion = '" + txtCodigobusq.Text + "' and t1.nombreRefaccion = '" + txtnombrereFaccionbusq.Text + "' and t2.empresa = '" + empresa + "'");
            }
            else if (!string.IsNullOrWhiteSpace(txtCodigobusq.Text) && chbFecha.Checked == true)
            {
                material_egresado("where t1.codrefaccion = '" + txtCodigobusq.Text + "' and date_format(t2.FechaHora, '%Y-%m-%d') between '" + dtpFechaInico.Value.ToString("yyyy-MM-dd") + "'and'" + dtpFechaTermino.Value.ToString("yyyy-MM-dd") + "' and t2.empresa = '" + empresa + "'");
            }
            else if (!string.IsNullOrWhiteSpace(txtnombrereFaccionbusq.Text) && chbFecha.Checked == true)
            {
                material_egresado("where date_format(t2.FechaHora, '%Y-%m-%d') between '" + dtpFechaInico.Value.ToString("yyyy-MM-dd") + "'and'" + dtpFechaTermino.Value.ToString("yyyy-MM-dd") + "' and t1.nombreRefaccion = '" + txtnombrereFaccionbusq.Text + "' and t2.empresa = '" + empresa + "'");
            }
            else
            {
                MessageBox.Show("Debe de seleccionar un criterio de busqieda".ToUpper(), "!!Alerta¡¡", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            // ThreadStart delegado = new ThreadStart(ExportarExcel);
            // exportar = new Thread(delegado);
            // exportar.Start();

            ExportarExcel();
        }

        private void Retorno_de_material_Load(object sender, EventArgs e)
        {
            CargarMecanico();
            cmbMecanico.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dtFecha.MaxDate = DateTime.Now;
            dtpFechaInico.MaxDate = DateTime.Now;
            dtpFechaTermino.MaxDate = DateTime.Now;
            material_egresado(where);
            lblNomRef.Text = refaccion;
            txtcodigo.Text = codigo;
            txtEco.Text = economico;
            if (string.IsNullOrWhiteSpace(codigo))
            {
                cargaEcoBusq(cmbEconomico);
                cmbEconomico.Visible = true;
                txtEco.Visible = false;
                label12.Visible = false;
            }
            else
            {
                txtcodigo.Enabled = txtEco.Enabled = false;

            }
            unidadMedida(codigo);
        }
        public void cargaEcoBusq(ComboBox cmbRecibe)
        {
            // cmbEconomico.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';select convert(idunidad,char) as idunidad, convert(concat(t2.identificador,LPAD(consecutivo,4,'0'),'-', descripcioneco),char) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas order by eco");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "--SELECCIONE ECONÓMICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbRecibe.DisplayMember = "eco";
            cmbRecibe.ValueMember = "idunidad";
            cmbRecibe.DataSource = dt;
        }
        private void cmbMecanico_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        public Retorno_de_material(int idUsuario, int empresa, int area, validaciones v, string refaccion, string codigo, string economico, string canrefaccion, string Folio)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idUsuario;
            this.codigo = codigo;
            this.refaccion = refaccion;
            this.economico = economico;
            this.cantRefaccion = canrefaccion;
            this.Folio = Folio;
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
        public void buscarF(string Codigo)
        {
            string cadena = v.ObtenerRef("SET lc_time_names = 'es_ES';select convert(nombreRefaccion,char) from crefacciones as t1 inner join cmarcas as t3 on t1.marcafkcmarcas = t3.idmarca inner join cfamilias as t4 on t3.descripcionfkcfamilias = t4.idfamilia inner join cunidadmedida as t2 on t4.umfkcunidadmedida = t2.idunidadmedida where t1.codrefaccion = '" + Codigo + "' and t1.empresa  = '" + empresa + "' and t1.existencias != 0");
            if (!string.IsNullOrWhiteSpace(cadena))
            {
                lblNomRef.Text = cadena;
            }
            else
            {
                MessageBox.Show("No se encontro la refaccion".ToUpper(), "SIN EXISTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtcodigo.Text = "";
            }
        }
        private void validcacionNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnGuardar_Click(null, e);
            else
            {
                TextBox txtKilometraje = sender as TextBox;
                char signo_decimal = (char)46;
                if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == 46)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("Solo se aceptan: numéros y (.) en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (e.KeyChar == 46)
                {
                    if (txtKilometraje.Text.LastIndexOf(signo_decimal) >= 0)
                    {
                        e.Handled = true; // Interceptamos la pulsación 
                    }
                }
            }
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
        public void Agregar()
        {
            if (v.validadRetorno(txtcodigo.Text, txtCantidad.Text, txtDispenso.Text, lblNomRef.Text, cmbMecanico.SelectedIndex, textBoxObservaciones.Text, cantRefaccion))
            {

                v.Carroceros("INSERT INTO cretorno(refaccionfkCRefacciones, cantida, usuariofkCPersonal, FechaHora, Empresa, Mecanico, Observacion) value ((select idrefaccion from crefacciones where codrefaccion ='" + txtcodigo.Text + "' and empresa ='" + empresa + "'), '" + txtCantidad.Text + "','" + idEntrega.ToString() + "','" + dtFecha.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','" + empresa + "', '" + cmbMecanico.SelectedValue + "','" + textBoxObservaciones.Text + "')");
                v.Carroceros("update crefacciones set existencias = existencias + '" + txtCantidad.Text + "' where codrefaccion = '" + txtcodigo.Text + "' and empresa = '" + empresa + "'");
                v.Carroceros("update pedidosrefaccion set CantidadEntregada = CantidadEntregada - '" + txtCantidad.Text + "' where FolioPedfkSupervicion = (Select idReporteSupervicion from reportesupervicion where Folio = '" + Folio + "') and RefaccionfkCRefaccion=(select idrefaccion from crefacciones where codrefaccion ='" + txtcodigo.Text + "')");
                MessageBox.Show("Refaccion agregada correctamente", "EXITO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Expota_PDF();
                limpiar();
            }
        }
        public void limpiar()
        {
            txtcodigo.Text = txtCantidad.Text = Folio = idEntrega = textBoxObservaciones.Text = lblNomRef.Text = lblNomUsuario.Text = lblMedida.Text = txtDispenso.Text = txtEco.Text = "";
            cmbEconomico.Visible = false;
            txtEco.Visible = label12.Visible = true;
            CargarMecanico();
            material_egresado(where);
        }
        public void material_egresado(string busquedas)
        {
            MySqlDataAdapter adaptador = (MySqlDataAdapter)v.getReport("SET lc_time_names = 'es_ES';Select convert(t1.codrefaccion, char) as 'Codigo Refacción', convert(t1.nombreRefaccion,char) as 'Nombre Refacción', convert(t2.cantida,char) as 'Cantidad', convert(date_format(t2.FechaHora, '%Y-%M-%d'),char) as Fecha,(select convert(concat(coalesce(x1.ApPaterno, ' ', x1.ApMaterno, ' ',x1.nombres)),char)) as 'Almacen', (select convert(t2.Mecanico,char)) as 'Mecanico' from cretorno as t2 inner join crefacciones as t1 on t2.refaccionfkCRefacciones = t1.idrefaccion inner join cpersonal as x1 on t2.usuariofkCPersonal = x1.idPersona " + busquedas);
            DataSet ds = new DataSet();
            adaptador.Fill(ds);
            dgAgregados.DataSource = ds.Tables[0];
            btnExcel.Visible = true;
            LblExcel.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void activarFecha(object sender, EventArgs e)
        {
            dtpFechaInico.Enabled = dtpFechaTermino.Enabled = true;
        }

        private void txtDispenso_Validating(object sender, CancelEventArgs e)
        {
            obtenerNombre();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void txtcodigo_Validating(object sender, CancelEventArgs e)
        {
            buscaref(txtcodigo.Text);
        }

        private void txtCodigobusq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                button7_Click(sender, e);
            }
        }

        delegate void El_Delegado();
        delegate void El_Delegado1();
        void cargando()
        {
            btnExcel.Visible = false;
            pbgif.Image = Properties.Resources.loader;
            LblExcel.Text = "Exportando";
        }
        void cargando1()
        {
            pbgif.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "Exportar";
        }
        void ExportarExcel()
        {
            /*     
                       if (dgAgregados.Rows.Count > 0)
                       {
                           DataTable dt = new DataTable();
                           for (int i = 0; i < dgAgregados.Columns.Count; i++)
                           {
                               if (dgAgregados.Columns[i].Visible)
                               {
                                   dt.Columns.Add(dgAgregados.Columns[i].HeaderText);
                               }
                           }
                           for (int j = 0; j < dgAgregados.Rows.Count; j++)
                           {
                               DataRow row = dt.NewRow();
                               int indice = 0;
                               for (int i = 0; i < dgAgregados.Columns.Count; i++)
                               {
                                   if (dgAgregados.Columns[i].Visible)
                                   {
                                       row[dt.Columns[indice]] = dgAgregados.Rows[j].Cells[i].Value.ToString().Replace("\n", " ");
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
                           try
                           {
                               if (this.InvokeRequired)
                               {
                                   El_Delegado1 delega = new El_Delegado1(cargando1);
                                   this.Invoke(delega);
                               }
                           }
                           catch (Exception)
                           {

                               throw;
                           }
                       }
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
            foreach (DataGridViewColumn column in dgAgregados.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;


            }



            int ir = 9;
            foreach (DataGridViewRow row in dgAgregados.Rows)
            {

                sl.SetCellValue(ir, 2, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[5].Value.ToString());
               
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

            sl.SetCellValue("H3", "FECHA/HORA DE CONSULTA:");
            SLStyle estiloF = sl.CreateStyle();
            estiloF.Font.FontName = "Arial";
            estiloF.Font.FontSize = 9;
            estiloF.Font.Bold = true;
            sl.SetCellStyle("H3", estiloF);
            sl.MergeWorksheetCells("H3", "I3");


            //Obtener Fecha


            DateTime fecha = DateTime.Now;

            sl.SetCellValue("J3", fecha.ToString());
            SLStyle fecha0 = sl.CreateStyle();
            fecha0.Font.FontName = "Arial";
            fecha0.Font.FontSize = 10;
            fecha0.Font.Bold = true;
            sl.SetCellStyle("J3", fecha0);

            sl.SetCellValue("H4", "RANGO CONSULTA DE:");
            SLStyle estiloF3 = sl.CreateStyle();
            estiloF3.Font.FontName = "Arial";
            estiloF3.Font.FontSize = 9;
            estiloF3.Font.Bold = true;
            sl.SetCellStyle("H4", estiloF3);
            sl.MergeWorksheetCells("H4", "I4");

            sl.SetCellValue("H5", "RANGO CONSULTA A:");
            SLStyle estiloF2 = sl.CreateStyle();
            estiloF2.Font.FontName = "Arial";
            estiloF2.Font.FontSize = 9;
            estiloF2.Font.Bold = true;
            sl.SetCellStyle("H5", estiloF2);
            sl.MergeWorksheetCells("H5", "I5");


            var datestring3 = dtpFechaInico.Value.ToLongDateString();

            sl.SetCellValue("J4", datestring3);
            SLStyle fechaDe = sl.CreateStyle();
            fechaDe.Font.FontName = "Arial";
            fechaDe.Font.FontSize = 10;
            fechaDe.Font.Bold = true;
            sl.SetCellStyle("J4", fechaDe);

            var datestring2 = dtpFechaTermino.Value.ToLongDateString();

            sl.SetCellValue("J5", datestring2);
            SLStyle fechaA = sl.CreateStyle();
            fechaA.Font.FontName = "Arial";
            fechaA.Font.FontSize = 10;
            fechaA.Font.Bold = true;
            sl.SetCellStyle("J5", fechaA);

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
        public void Expota_PDF()
        {

            //Código para generación de archivo pdf
            string nombreHoja = "";
            //string[] cadenafolio = FolioR.Split('-');
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(20f, 20f, 10f, 10f);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ValidateNames = true;
            saveFileDialog1.InitialDirectory = "@C:";
            saveFileDialog1.Title = "Guardar Orden De Compra";
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
                    iTextSharp.text.Font arial12 = FontFactory.GetFont("Arial", 12, BaseColor.BLACK);
                    doc.Open();
                    if (empresa == 2)
                    {
                        img = Convert.FromBase64String(v.tri);
                        nombreHoja = "TRANSINSUMOS S.A. DE C.V.";
                    }
                    else if (empresa == 3)
                    {
                        img = Convert.FromBase64String(v.trainsumos);
                        nombreHoja = "TRANSINSUMOS S.A. DE C.V.";
                    }

                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(24f);
                    imagen.SetAbsolutePosition(440f, 720f);
                    float percentage = 0.0f;
                    percentage = 100 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);
                    Chunk chunk = new Chunk("ECATEPEC ESTADO DE MEXICO A " + dtFecha.Value.ToString("dd 'de' MMMM 'de' yyyy"), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    doc.Add(imagen);
                    doc.Add(new Paragraph(chunk));
                    doc.Add(new Paragraph("                                    "));
                    PdfPTable tabla = new PdfPTable(2);
                    tabla.DefaultCell.Border = 0;
                    tabla.WidthPercentage = 100;
                    /*
                     t1.Folio,'|',UNIDAD,'|',FECHA Y HORA,'|',MECANICO,'|',FECHAHORA ENTREGA,'|',PERSONA QUE ENTREGA,'|',FolioFactura,'|',ObservacionesTrans FolioR
                     */
                    tabla.AddCell(v.valorCampo(nombreHoja.ToString(), 2, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n CARRETERA FEDERAL MÉXICO PACHUCA 26.5, COL. VENTA DE CARPIO ECATEPEC DE MORELOS ESTADO DE MÉXICO C.P. 55060", 2, 1, 0, arial2));
                    tabla.AddCell(v.valorCampo("\n\n Economico: " + txtEco.Text, 1, 0, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n", 1, 0, 0, arial2));
                    tabla.AddCell(v.valorCampo("Refaccion Retorna:", 2, 1, 0, arial));
                    tabla.AddCell(v.valorCampo("\n\n " + txtcodigo.Text + "\t" + lblNomRef.Text + "\t Cantidad: " + txtCantidad.Text + " " + lblMedida.Text, 2, 0, 0, arial12));
                    // tabla.AddCell(v.valorCampo(proveedor.ToString(), 2, 1, 0, arial));
                    /* tabla.AddCell(v.valorCampo("Non. REFACCION", 1, 0, 0, arial));
                     tabla.AddCell(v.valorCampo("ENTREGA", 2, 0, 0, arial));
                     tabla.AddCell(v.valorCampo(lblNomRef.Text, 1, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo(lblNomUsuario.Text, 1, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("\n\n", 2, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("COMENTARIOS", 2, 0, 0, arial));
                     tabla.AddCell(v.valorCampo("", 2, 0, 0, arial2));
                     tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));*/




                    /*tabla.AddCell(v.valorCampo("REFACCIONES SOLICITADAS", 2, 1, 0, FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD)));
                    tabla.AddCell(v.valorCampo("\n\n\n", 2, 0, 0, arial2));*/
                    doc.Add(tabla);
                    // GenerarDocumento(doc);
                    doc.Add(new Paragraph("\n\n\nMOTIVO DE RETORNO:", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("\n\n" + textBoxObservaciones.Text, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL)));
                    doc.Add(new Paragraph("\n\n\n\n" + lblNomUsuario.Text, FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("\n_________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("\n\n" + cmbMecanico.SelectedValue.ToString(), FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("\n_________________________________________", FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD)));
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
        void unidadMedida(string codigo)
        {
            lblMedida.Text = v.ObtenerRefR("SET lc_time_names = 'es_ES';select convert(t1.Simbolo, char) from cunidadmedida as t1 inner join cfamilias as t2 on t1.idunidadmedida = t2.umfkcunidadmedida inner join cmarcas as t3 on t3.descripcionfkcfamilias = t2.idfamilia inner join crefacciones as t4 on t4.marcafkcmarcas = t3.idmarca where t4.codrefaccion = '" + codigo + "'");
        }

        public void buscaref(string codigo)
        {
            string[] seprar = v.ObtenerRefR("SET lc_time_names = 'es_ES';select COALESCE(concat(convert(t1.nombreRefaccion,char),'|', convert(t1.existencias, char), '|',convert(t1.CostoUni, char),'|', convert(t1.modeloRefaccion, char),'|', coalesce(convert(if(x1.empresa = '',concat(x1.aPaterno, ' ', x1.aMaterno, ' ', x1.nombres) , x1.empresa),char),'')),'') from crefacciones as t1 inner join cmarcas as t3 on t1.marcafkcmarcas = t3.idmarca inner join cfamilias as t4 on t3.descripcionfkcfamilias = t4.idfamilia inner join cunidadmedida as t2 on t4.umfkcunidadmedida = t2.idunidadmedida inner join cproveedores as x1 on x1.idproveedor = t1.proveedrofkCProveedores where t1.codrefaccion = '" + codigo + "' and t1.empresa  = '" + empresa + "'").ToString().Split('|');
            if (seprar.Length == 1)
            {
                MessageBox.Show("No se encontro la refaccion".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                lblNomRef.Text = seprar[0].ToString();

            }
        }

    }
}
