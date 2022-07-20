using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class Inventario : Form
    {
        int empresa, usuario, area;
        validaciones v;
        public Thread hilo, th;
        public Inventario()
        {
            InitializeComponent();
        }
        public Inventario(int empresa, int usuario, int area, validaciones v)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            this.empresa = empresa;
            this.usuario = usuario;
            this.area = area;
            this.v = v;
            InitializeComponent();
        }


        public void Loas_Inventario(object sender, EventArgs e)
        {

            if (usuario == 121)
            {
                cargarInventario("(t1.Tipo = 3)");
            }
            else if (usuario != 121)
            {
                cargarInventario("(t1.Tipo = 1 or t1.Tipo = 2)");
            }
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
        }
        public void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Exportar(object sender, EventArgs e)
        {
            Exportar_Excel();
        }


        public void cargarInventario(string Tipo)
        {
            DataTable dt =(DataTable)v.getData("select t1.codrefaccion as 'CODIGO',t1.nombreRefaccion AS 'NOMBRE REFACCION',t1.existencias AS 'EXISTENCIA',t1.CostoUni AS 'COSTO UNITARIO',coalesce(t1.maximo,0) AS 'MAXIMO',t1.media as 'MEDIA',t1.abastecimiento AS 'MINIMO', t5.Simbolo from crefacciones as t1 inner join cmarcas as t3 on t3.idmarca = t1.marcafkcmarcas inner join cfamilias as t4 on t3.descripcionfkcfamilias =t4.idfamilia inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida where t1.empresa = '" + empresa + "' and " + Tipo + "");

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

        



        void Exportar_Excel()
        {
            SLDocument sl = new SLDocument();

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
                ir++;
                celdaInicial++;

            }
            //Nombre de la Hoja de Excel
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Consulta Entradas");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "I" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "INVENTARIO " + System.DateTime.Now.Year);
            SLStyle estiloT = sl.CreateStyle();
            estiloT.Font.FontName = "Arial";
            estiloT.Font.FontSize = 13;
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
            sl.SetCellStyle("B" + celdaInicial, "I" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "I");
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
        }

    }
}
