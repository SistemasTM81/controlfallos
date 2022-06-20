using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using h = Microsoft.Office.Interop.Excel;

namespace controlFallos
{
    public partial class masInfoES : Form
    {
        string proviene, cadena;
        bool activo = true;
        validaciones v;
        int idUsuario, empresa, area;
        DataTable dt;
        mainfoReportes mas;

        public masInfoES(validaciones v, int idUsuario, int empresa, int area, string cadena, string proviene)
        {
            this.v = v;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.cadena = cadena;
            this.proviene = proviene;
            InitializeComponent();
        }


        public masInfoES()
        {
            InitializeComponent();
        }

        public void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }
        bool exportando = false;
        public void Exportar(object sender, EventArgs e)
        {
            //    exportando = true;
            //    ThreadStart excel = new ThreadStart(exportar_excel);
            //    hiloEx2 = new Thread(excel);
            //    hiloEx2.Start();

            exportar_excel();
        }
        Thread hiloEx2;
        public void carga1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            pictureBoxExcelLoad.Visible = true;
            buttonExcel.Visible = false;
            label35.Location = new Point(9, 492);
            label35.Text = "EXPORTANDO";
        }

        delegate void Loading1();
        public void carga2()
        {
            pictureBoxExcelLoad.Image = null;
            pictureBoxExcelLoad.Visible = false;
            buttonExcel.Visible = true;
            label35.Location = new Point(9, 492);
            label35.Text = "EXPORTAR";
            if (activo)
            {
                buttonExcel.Visible = false;
                label35.Visible = false;
            }
            activo = false;
            exportando = false;
        }

        delegate void Loading();
        public void Load_(object sender, EventArgs e)
        {
            if (proviene.ToString().Equals("Entradas"))
            {
                string [] cadena_corta = cadena.ToString().Split('|');
                dgvEntrada.Columns.Add("CODIGO", "CODIGO");
                dgvEntrada.Columns.Add("REFACCION", "REFACCION");
                dgvEntrada.Columns.Add("CANTIDAD", "CANTIDAD");
                dgvEntrada.Columns.Add("SIMBOLO", "SIMBOLO");
                dgvEntrada.Columns.Add("COSTO", "COSTO UNITARIO");
                dgvEntrada.Columns.Add("IVA", "IVA");
                dgvEntrada.Columns.Add("SUBTOTAL", "SUBTOTAL");
                dgvEntrada.Columns.Add("TOL", "TOTAL");
                ConsultaEntradas("where t2.refaccionfkCRefacciones = " + cadena_corta[1].ToString()+ cadena_corta[0].ToString() + " and t1.empresa = '" + empresa + "'");
            }
            else if(proviene.ToString().Equals("Salidas"))
            {
                string[] cadena_corta = cadena.ToString().Split('|');
                dgvEntrada.Columns.Add("ECO", "ECONOMICO");
                dgvEntrada.Columns.Add("CODIGO", "CODIGO");
                dgvEntrada.Columns.Add("REFACCION", "REFACCION");
                dgvEntrada.Columns.Add("CANTIDAD", "CANTIDAD");
                dgvEntrada.Columns.Add("COSTO", "COSTO UNITARIO");
                dgvEntrada.Columns.Add("VENTA", "COSTO VENTA");
                dgvEntrada.Columns.Add("TOL", "TOTAL");
                ConsultaSalidas("where t1.idrefaccion = " + cadena_corta[2].ToString() + cadena_corta[0].ToString() + " and t1.empresa = '" + empresa + "'", "where t1.idrefaccion = " + cadena_corta[2].ToString() + cadena_corta[1].ToString() + " and t1.empresa = '" + empresa + "'"); 
            }
            else if(proviene.ToString().Equals("Unidades"))
            {
                dgvEntrada.Columns.Add("UNIDAD", "UNIDAD");
                dgvEntrada.Columns.Add("FECHA", "FECHA DE REPORTE");
                dgvEntrada.Columns.Add("KM", "KILOMETRAJE DE REPORTE");
                dgvEntrada.Columns.Add("FALLA", "DESCRIPCION DE FALLA");
                mas = new mainfoReportes();
               dt = mas.obtener_eportes("where t1.FechaReporte between '2022-05-01' and '2022-05-31'and t1.TipoFallo = 1 and t2.consecutivo = 85");
                int numFila = dt.Rows.Count;
                dgvEntrada.Visible = true;
                if (numFila > 0)
                {
                    for (int i = 0; i < numFila; i++)
                    {
                        dgvEntrada.Rows.Add(dt.Rows[i].ItemArray);
                    }
                }
            }
            
        }
        void ConsultaEntradas(string cadena)
        {
            dt = (DataTable)v.getData("select t1.codrefaccion, t1.nombreRefaccion, t2.CantidadIngresa, t5.Simbolo, concat('$ ', Round(t2.Costo,2)) as 'Costo Unitario', concat('$ ', ROUND((t2.Costo * t2.CantidadIngresa),2)) as SubTotal, concat('$ ', ROUND((t2.Costo * t2.CantidadIngresa) * 0.16,2)) as 'IVA',concat('$ ',ROUND((t2.Costo * t2.CantidadIngresa) + ((t2.Costo * t2.CantidadIngresa) * 0.16) ,2)) as 'Total', t2.proveedor as'PROVEEDOR', t2.FechaHora FROM crefacciones as t1 inner join centradasm as t2 on t1.idrefaccion = t2.refaccionfkCRefacciones left join cmarcas as t3 on t3.idmarca = t1.marcafkcmarcas inner join cfamilias as t4 on t3.descripcionfkcfamilias =t4.idfamilia inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida " + cadena);
            int numFila = dt.Rows.Count;
            dgvEntrada.Visible = true;
                if (numFila > 0)
                {
                    for (int i = 0; i < numFila; i++)
                    {
                        dgvEntrada.Rows.Add(dt.Rows[i].ItemArray);
                    }
                sumar(dgvEntrada);
                }
        }
        void ConsultaSalidas(string cadena1, string cadena2)
        {
            dt = (DataTable)v.getData("SELECT consecutivo ,codrefaccion , nombreRefaccion, CantidadEntregada, CostoUni, ROUND((CostoUni) * (5 /100) + (CostoUni) ,2) as 'Costo Venta',ROUND(((CostoUni ) * (5 /100) + (CostoUni))* CantidadEntregada,2) as 'Total'  FROM (select t4.consecutivo, t1.codrefaccion, t1.nombreRefaccion, t2.CantidadEntregada, t1.CostoUni,ROUND((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) ,2) as 'Costo Venta',ROUND(((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)))* t2.CantidadEntregada,2) as 'Total' from crefacciones as t1 inner join pedidosrefaccion as t2 on t1.idrefaccion = t2.RefaccionfkCRefaccion inner join reportesupervicion as t3 on t3.idReporteSupervicion = t2.FolioPedfkSupervicion inner join cunidades as t4 on t4.idunidad = t3.UnidadfkCUnidades " + cadena1 + " union all select t4.consecutivo, t1.codrefaccion, t1.nombreRefaccion, t2.CantidadEntregada, t1.CostoUni,ROUND((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) ,2) as 'CostoVenta',ROUND(((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)))* t2.CantidadEntregada,2) as 'Total' from crefacciones as t1 inner join ccarrocero as t2 on t1.idrefaccion = t2.refaccionfkCRefacciones inner join cunidades as t4 on t4.idunidad = t2.unidadfkCUnidades " + cadena2 + ") t");
            int numFila = dt.Rows.Count;
            if (numFila > 0)
            {
                for (int i = 0; i < numFila; i++)
                {
                    dgvEntrada.Rows.Add(dt.Rows[i].ItemArray);
                }
                sumarSalidas(dgvEntrada);
            }
        }

        void sumar(DataGridView data)
        {
            double suma = 0.0;
            foreach (DataGridViewRow row in data.Rows)
            {
                string[] sin_simbolo = row.Cells["TOL"].Value.ToString().Split('$');
                if (row.Cells["TOL"].Value != null)
                    suma += Convert.ToDouble(sin_simbolo[1].ToString());
            }
            lblCostoTotal.Text = lblCostoTotal.Text + " $ " + suma.ToString("n");
        }
        void sumarSalidas(DataGridView data)
        {
            double suma = 0.0;
            foreach (DataGridViewRow row in data.Rows)
            {
                
                if (row.Cells["TOL"].Value != null)
                    suma += Convert.ToDouble(row.Cells["TOL"].Value);
            }
            lblCostoTotal.Text = lblCostoTotal.Text + " $ " + suma.ToString("n");
        }
        void exportar_excel()
        {
            /*
                        if (dt.Rows.Count > 0)
                        {
                            //isexporting = true;
                           // dt = (DataTable)dataGridView2.DataSource;
                            /*  if (this.InvokeRequired)
                              {
                                  uno delega = new uno(inicio);
                                  this.Invoke(delega);
                              }
                            Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                            X.Application.Workbooks.Add(Type.Missing);
                            h.Worksheet sheet = (h.Worksheet)X.ActiveSheet;
                            X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            for (int i = 1; i <= dt.Columns.Count; i++)
                            {
                                h.Range rng = (h.Range)sheet.Cells[1, i];
                                sheet.Cells[1, i] = dt.Columns[i - 1].ColumnName.ToUpper();
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
                                    try
                                    {
                                        h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                                        sheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
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
            foreach (DataGridViewColumn column in dgvEntrada.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;


            }



            int ir = 9;
            foreach (DataGridViewRow row in dgvEntrada.Rows)
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
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Mas Informacion");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "K" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "MAS INFORMACION");
            SLStyle estiloT = sl.CreateStyle();
            estiloT.Font.FontName = "Arial";
            estiloT.Font.FontSize = 15;
            estiloT.Font.Bold = true;
            sl.SetCellStyle("D4", estiloT);
            sl.MergeWorksheetCells("D4", "F4");
            
            //Estilo Titulo

            //Estilos Para bordes de la tabla

            SLStyle EstiloB = sl.CreateStyle();

            EstiloB.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.LeftBorder.Color = System.Drawing.Color.Black;

            EstiloB.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            sl.SetCellStyle("B" + celdaInicial, "K" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "K");
            //Estilos Para bordes de la tabla

            //Extraer fecha

            sl.SetCellValue("G3", "FECHA/HORA DE CONSULTA:");
            SLStyle estiloF = sl.CreateStyle();
            estiloF.Font.FontName = "Arial";
            estiloF.Font.FontSize = 9;
            estiloF.Font.Bold = true;
            sl.SetCellStyle("G3", estiloF);
            sl.MergeWorksheetCells("G3", "H3");


            //Obtener Fecha


            DateTime fecha = DateTime.Now;

            sl.SetCellValue("I3", fecha.ToString());
            SLStyle fecha0 = sl.CreateStyle();
            fecha0.Font.FontName = "Arial";
            fecha0.Font.FontSize = 10;
            fecha0.Font.Bold = true;
            sl.SetCellStyle("I3", fecha0);

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
//Excel Completo 17/06/2022