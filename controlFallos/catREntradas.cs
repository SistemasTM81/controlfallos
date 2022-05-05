using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using h = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;

namespace controlFallos
{
    public partial class catREntradas : Form
    {
        validaciones v;
        MySqlDataAdapter adaptador;
        int empresa, area, IdUsuario, total, inicio = 0;
        string consultaGeneral = "SELECT t1.idrefaccion, t1.codrefaccion as CODIGO, t1.nombreRefaccion AS REFACCION, sum(t2.CantidadIngresa) AS TOTAL, t5.Simbolo AS MEDIDA FROM crefacciones as t1 inner join centradasm as t2 on t1.idrefaccion = t2.refaccionfkCRefacciones inner join cmarcas as t3 on t3.idmarca = t1.marcafkcmarcas inner join cfamilias as t4 on t4.idfamilia = t3.descripcionfkcfamilias inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida";
        string messel;
        DataSet ds = new DataSet();
        DataTable dt;
        public catREntradas()
        {
            InitializeComponent();
        }

        public catREntradas(validaciones v, int empresa, int area, int IdUsuario)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.IdUsuario = IdUsuario;
        }
        public void ConsultaGeneral(string cadena)
        {
            DataTable contar = new DataTable();
            adaptador = (MySqlDataAdapter)v.getReport(consultaGeneral + cadena);
            adaptador.Fill(contar);
            if (contar.Rows.Count > 1)
            {
                adaptador.Fill(ds, 0, 10, "Entradas");
                dgvEntrada.DataSource = ds.Tables[0];
            }
            else
            {
                adaptador.Fill(ds, 0, 1, "Entradas");
                dgvEntrada.DataSource = ds.Tables[0];
              
            }
           
        }
        private void CargarInicio(object sender, EventArgs e)
        {
            ConsultaGeneral(" where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.empresa = '" + empresa + "' group by t1.idrefaccion");
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
            v.comboswithuot(cmbEmpresa, new string[] { "--Seleccione Empresa", "TRANSINSUMOS", "TRANSMASIVO", "PRODUCCION" });
            cmbMes.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbEmpresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        private void Buscar(object sender, EventArgs e)
        {
            ds.Clear();
            if (!string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex == 0)
            {
                ConsultaGeneral(" where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.empresa = '" + empresa + "' and t1.codrefaccion = '" + txtcodigo.Text + "' group by t1.idrefaccion");
            }
            else if (string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex > 0 && cmbMes.SelectedIndex == 0)
            {
                ConsultaGeneral(" where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.empresa = '" + empresa + "' and t1.Tipo = '" + cmbEmpresa.SelectedIndex + "' group by t1.idrefaccion");
            }
            else if (string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex > 0)
            {
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                ConsultaGeneral(" where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.empresa = '" + empresa + "' and date_format(FechaHora, '%m') = '" + messel.ToString() + "' group by t1.idrefaccion") ;
            }
            else if (!string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex > 0 && cmbMes.SelectedIndex == 0)
            {
                ConsultaGeneral(" where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.empresa = '" + empresa + "' and t1.codrefaccion = '" + txtcodigo.Text + "' and t1.Tipo = '" + cmbEmpresa.SelectedIndex + "' group by t1.idrefaccion");
            }
            else if (!string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex > 0)
            {
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                ConsultaGeneral(" where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.empresa = '" + empresa + "' and t1.codrefaccion = '" + txtcodigo.Text + "' and date_format(FechaHora, '%m') = '" + messel.ToString() + "' group by t1.idrefaccion");
            }
            else if (string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex > 0 && cmbMes.SelectedIndex > 0)
            {
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                ConsultaGeneral(" where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.empresa = '" + empresa + "' and t1.Tipo = '" + cmbEmpresa.SelectedIndex + "' and date_format(FechaHora, '%m') = '" + messel.ToString() + "' group by t1.idrefaccion");
            }
            else
            {
                MessageBox.Show("!Seleccione Parametros De Busqueda", "!ALERTA¡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Limpiar();
        }
        private void Excel_Export(object sender, EventArgs e)
        {
            if (dgvEntrada.Rows.Count > 0)
            {
                //isexporting = true;
                dt = (DataTable)dgvEntrada.DataSource;
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
                /*if (this.InvokeRequired) 
                {
                    dos delega2 = new dos(termino);
                    this.Invoke(delega2);
                }*/
                buttonExcel.Visible = false;
                label35.Visible = false;
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Siguiente(object sender, EventArgs e)
        {
            inicio =inicio + 10;
            recorrerE(inicio);
        }
        private void Anterior(object sender, EventArgs e)
        {
            inicio = inicio - 10;
            recorrerE(inicio);
        }
        private void DrawIntem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        private void Validar(object sender, KeyPressEventArgs e)
        {
                v.enGeneral(e);
        }

        private void cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        public void recorrerE(int valor)
        {
                
                    ds.Clear();
            if (valor >=0)
            {
                adaptador.Fill(ds, valor, 10, "Entradas");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dgvEntrada.DataSource = ds.Tables[0];
                    btnSiguiente.Enabled = true;
                    btnAnterior.Enabled = true;
                }
                else
                {
                    btnAnterior.Enabled = true;
                    btnSiguiente.Enabled = false;
                }
            }
        }
        void Limpiar()
        {
            txtcodigo.Text = "";
            cmbEmpresa.SelectedItem = cmbMes.SelectedItem = 0;
            buttonExcel.Visible = label35.Visible = true;

        }
    }
}
