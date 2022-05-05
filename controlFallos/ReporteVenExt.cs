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
using h = Microsoft.Office.Interop.Excel;

namespace controlFallos
{
    public partial class ReporteVenExt : Form
    {
        validaciones v;
        int empresa, area, Idusuario;
        intermedio comunicador = new intermedio();
        DataSet dsConsulta = new DataSet();
        DataTable dt = new DataTable();
        public ReporteVenExt(validaciones v, int empresa, int area, int Idusuario)
        {
            this.v = v;
            this.empresa = empresa;
            this.area = area;
            this.Idusuario = Idusuario;

            InitializeComponent();
        }

        private void ReporteVentaExt_Load(object sender, EventArgs e)
        {
            cargaEcoBusq();
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
            cmbMes.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbBuscarUnidad.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dtpFechaDe.MaxDate = DateTime.Now;
            dtpFechaA.MaxDate = DateTime.Now;
            valoresDTP();
        }

        public ReporteVenExt()
        {
            InitializeComponent();
        }

        public void cargaEcoBusq()
        {
            cmbBuscarUnidad.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0'),'-',t1.descripcioneco) as eco  FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas inner join cmodelos as t4 on t1.modelofkcmodelos = t4.idmodelo where (t4.modelo != 'TRANSCARRIER' AND t4.modelo != 'VOLVO' AND t4.modelo != 'EARTBUS') order by eco");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "--SELECCIONE ECONÓMICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbBuscarUnidad.DisplayMember = "eco";
            cmbBuscarUnidad.ValueMember = "idunidad";
            cmbBuscarUnidad.DataSource = dt;
        }
        void valoresDTP()
        {
            try
            {
                dtpFechaDe.Value = DateTime.Now.Subtract(TimeSpan.Parse("1"));
            }
            catch
            {
                dtpFechaDe.Value = dtpFechaDe.MinDate;
            }
            dtpFechaA.Value = dtpFechaA.MaxDate;
        }

        void Salidas()
        {

            if (!string.IsNullOrEmpty(txtcodigo.Text) && cmbBuscarUnidad.SelectedIndex == 0 && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {
                dsConsulta = cargarGridSC(" Where t3.codrefaccion = '" + txtcodigo.Text + "' and  t3.empresa = '" + empresa + "' and t2.Cancelado='0'  and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbBuscarUnidad.SelectedIndex != 0 && string.IsNullOrEmpty(txtcodigo.Text) && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {
                dsConsulta.Clear();
                dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0' and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbMes.SelectedIndex != 0 && cmbBuscarUnidad.SelectedIndex == 0 && string.IsNullOrEmpty(txtcodigo.Text) && cbFecha.Checked == false)
            {
                string messel = "";
                dsConsulta.Clear();
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                dsConsulta = cargarGridSC(" where date_format(convert(t2.FechaHora, char),'%m')='" + messel + "'AND YEAR(t2.FechaHora) = '" + DateTime.Now.Year + "'and t2.empresa = '" + empresa + "' and t2.Cancelado='0'  and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cbFecha.Checked == true && cmbMes.SelectedIndex == 0 && cmbBuscarUnidad.SelectedIndex == 0 && string.IsNullOrEmpty(txtcodigo.Text))
            {
                dsConsulta = cargarGridSC("  where date_format(convert(t2.FechaHora, char),'%Y/%m/%d') between'" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t2.empresa = '" + empresa + "' and t2.Cancelado='0'  and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            ///consultas convinadas
            if (cmbBuscarUnidad.SelectedIndex != 0 && cbFecha.Checked == true)
            {
                dsConsulta.Clear();
                dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and date_format(convert(t2.FechaHora, char),'%Y/%m/%d') between'" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0'  and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbBuscarUnidad.SelectedIndex != 0 && cmbMes.SelectedIndex != 0)
            {
                dsConsulta.Clear();
                string messel = "";
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and date_format(convert(t2.FechaHora, char),'%m')='" + messel + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0'  and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbBuscarUnidad.SelectedIndex != 0 && !string.IsNullOrEmpty(txtcodigo.Text))
            {
                dsConsulta.Clear();
                dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and t3.codrefaccion = '" + txtcodigo.Text + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0'  and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!string.IsNullOrEmpty(txtcodigo.Text) && cbFecha.Checked == true)
            {
                dsConsulta.Clear();
                dsConsulta = cargarGridSC(" Where t3.codrefaccion = '" + txtcodigo.Text + "'and date_format(convert(t2.FechaHora, char),'%Y/%m/%d') between'" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0' and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!string.IsNullOrEmpty(txtcodigo.Text) && cmbMes.SelectedIndex != 0)
            {
                dsConsulta.Clear();
                string messel = "";
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                dsConsulta = cargarGridSC(" Where t3.codrefaccion = '" + txtcodigo.Text + "'and date_format(convert(t2.FechaHora, char),'%m')='" + messel + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0'  and t2.TipoSalida = '2'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        public DataSet cargarGridSC(string Busqueda)
        {
            DataSet ds = new DataSet();
            MySqlDataAdapter adaptador = v.getReport("SET lc_time_names = 'es_ES';Select  convert(concat(t1.consecutivo, '-', t1.descripcioneco),char) as Economico, convert(t3.codrefaccion,char) as Codigo, convert(t3.nombreRefaccion,char) as Nombre, convert(t2.CantidadEntregada,char) as Cantidad, convert(t3.CostoUni,char) as 'Costo Compra', convert(t5.Simbolo, char) as 'Moneda',upper(date_format(convert(t2.FechaHora, char),'%d/%M/%Y %H:%i %p')) as Salida, if(t3.Tipo = 1, 'NUEVA', if(t3.Tipo = 2, 'REMANOFACTURADA', if(t3.Tipo = 3, 'PODUCCION',''))) as 'TIPO'   from cunidades as t1 inner join ccarrocero as t2 on t1.idunidad = t2.unidadfkCUnidades inner join crefacciones as t3 on t3.idrefaccion = t2.refaccionfkCRefacciones inner join datosistema as t4 on t4.usuariofkcpersonal = t2.usuariofkCPersonal inner join ctipocambio as t5 on t3.tipoMonedafkCTipoCambio = t5.idtipoCambio" + Busqueda);
            adaptador.Fill(ds);
            buttonExcel.Visible = label35.Visible = true;
            return ds;
        }
        void exportar_excel()
        {
            if (dataGridView2.Rows.Count > 0)
            {
                //isexporting = true;
                dt = (DataTable)dataGridView2.DataSource;
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

        void Limpiar()
        {
            cmbBuscarUnidad.SelectedIndex = 0;
            cmbMes.SelectedIndex = 0;
            txtcodigo.Text = "";
            cbFecha.Checked = false;
        }

        public void buscar(object sender, EventArgs e)
        {
            Salidas();
            Limpiar();
        }

        public void Exportar(object sender, EventArgs e)
        {
            dataGridView2.DataSource = dsConsulta.Tables[0];
            MessageBox.Show("Iniciando exportacion", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            exportar_excel();
        }

        private void cbFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFecha.Checked == true)
            {
                dtpFechaA.Enabled = dtpFechaDe.Enabled = true;
            }
            else
            {
                dtpFechaA.Enabled = dtpFechaDe.Enabled = false;
            }
        }

        private void cmbDrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
