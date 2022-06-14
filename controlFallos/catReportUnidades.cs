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
using System.Windows.Forms.DataVisualization.Charting;

namespace controlFallos
{
    public partial class catReportUnidades : Form
    {
        validaciones v;
        int area, empresa, usuario, total, valor=0;
        string Consulta = "SELECT concat(t1.consecutivo, '-', t1.descripcioneco) as Unidad, 'CORRECTIVO' AS 'Tipo Falla', count(t2.idReporteSupervicion) AS TOTAL, 'MAS INFORMACION' FROM cunidades as t1 inner join reportesupervicion as t2 on t1.idunidad = t2.UnidadfkCUnidades ";
        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        MySqlDataAdapter adaptador;
             
        public catReportUnidades(validaciones v, int area, int empresa, int usuario)
        {
            InitializeComponent();
            this.v = v;
            this.area = area;
            this.empresa = empresa;
            this.usuario = usuario;
        }
        public catReportUnidades()
        {
            InitializeComponent();
        }

        public void Cargar_Load(object sender, EventArgs e)
        {
            adaptador = (MySqlDataAdapter)v.getReport(Consulta + "where t2.FechaReporte between '2022-05-01' and '2022-05-31' and t2.TipoFallo = 1  group by t1.consecutivo");
            adaptador.Fill(ds);

            total = ds.Tables[0].Rows.Count;
            if (total == 1)
            {
                adaptador.Fill(ds2);
            }
            else
            {
                adaptador.Fill(ds2, valor, 10, "Entradas");
                btnSiguiente.Enabled = true;
                valor = 10;
            }
            dgvReporte.DataSource = ds2.Tables[0];
            Graficas();


        }


        void Graficas()
        {
            string unidad = "", total = "";
            chart1.Titles.Clear();
            foreach (DataRow row in ds2.Tables[0].Rows)
            {
                unidad += Convert.ToString(row["Unidad"]) + ",";
                total += Convert.ToString(row["TOTAL"]) + ",";

            }
            string [] series = unidad.Split(',');
            string[] puntos = total.Split(',');
            chart1.Palette = ChartColorPalette.Excel;
           
            chart1.Titles.Add("Unidades");
            for (int i = 0; i < series.Length - 1; i++)
            {
                Series seri = chart1.Series.Add(series[i]);
                //chart1.Series[series[i]].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                chart1.Series[i].IsValueShownAsLabel = true;
                chart1.Series[i].LabelForeColor = Color.Blue; 
                seri.Label = puntos[i].ToString();
                
                seri.Points.AddXY(10.00, Convert.ToInt32(puntos[i]));

                //seri.Points.Add(Convert.ToInt32(puntos[i]));
            }
        }

        public void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Siguiente(object sender, EventArgs e)
        {
            if (total >= valor && valor > 0)
            {
                ds2.Clear();
                adaptador.Fill(ds2, valor, 10, "Entradas");
                if (ds2.Tables[0].Rows.Count != 0)
                {
                    dgvReporte.DataSource = ds2.Tables[0];
                    btnSiguiente.Enabled = true;
                    btnAnterior.Enabled = true;
                    LimpiarGrafico();
                    Graficas();
                    valor = valor + 10;
                }
                else
                {
                    btnAnterior.Enabled = true;
                    btnSiguiente.Enabled = false;
                }
            }
        }
        void LimpiarGrafico()
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
               // chart1.Series[0][Convert.ToString(series)] = ;
            }
        }
    }
}
