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
            exportando = true;
            ThreadStart excel = new ThreadStart(exportar_excel);
            hiloEx2 = new Thread(excel);
            hiloEx2.Start();
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
            if (dt.Rows.Count > 0)
            {
                //isexporting = true;
               // dt = (DataTable)dataGridView2.DataSource;
                /*  if (this.InvokeRequired)
                  {
                      uno delega = new uno(inicio);
                      this.Invoke(delega);
                  }*/
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
        }

    }
}
