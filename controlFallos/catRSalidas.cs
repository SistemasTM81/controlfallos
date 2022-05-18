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
using MySql.Data.MySqlClient;
using h = Microsoft.Office.Interop.Excel;



namespace controlFallos
{
    public partial class catRSalidas : Form
    {
        validaciones v;
        MySqlDataAdapter adaptador;
        int empresa, area, IdUsuario, total, inicio = 0;
        string messel, cadenaBusqueda;
        DataSet ds = new DataSet();
        DataTable dt;
        public catRSalidas()
        {
            InitializeComponent();
        }
        public catRSalidas(validaciones v, int empresa, int area, int IdUsuario)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.IdUsuario = IdUsuario;
        }

        private void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CargarSalidas(object sender, EventArgs e)
        {
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
            v.comboswithuot(cmbEmpresa, new string[] { "--Seleccione Empresa", "TRANSINSUMOS", "TRANSMASIVO", "PRODUCCION" });
            cmbMes.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbEmpresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            ConsultaGenera("where date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa + "'", "where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.Cancelado = 0 and t1.empresa = '" + empresa + "'");
            dtpFechaDe.MaxDate = DateTime.Now;
            dtpFechaA.MaxDate = DateTime.Now;
        }
        private void Buscar(object sender, EventArgs e)
        {
            ds.Clear();
            if (!string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa + "' and t2.codrefaccion = '" + txtcodigo.Text +"'", "where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.Cancelado = 0 and t2.codrefaccion = '" + txtcodigo.Text + "' and t2.empresa = '" + empresa + "'");
                cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y')" + "|" + " and date_format(FechaHora, '%Y') = date_format(now(), '%Y')";
            }
            else if (string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex > 0 && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa + "' and t2.codrefaccion = '" + txtcodigo.Text + "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "'", "where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.Cancelado = 0 and t2.codrefaccion = '" + txtcodigo.Text + "' and t2.empresa = '" + empresa + "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "'");
                cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y')" + "|" + " and date_format(FechaHora, '%Y') = date_format(now(), '%Y')";
            }
            else if (string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex > 0 && cbFecha.Checked == false)
            {
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa + "' and date_format(fechaHoraPedido, '%m') = '" + messel.ToString() + "'", "where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa + "' and date_format(FechaHora, '%m') = '" + messel.ToString() + "'");
                cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and date_format(fechaHoraPedido, '%m') = '" + messel.ToString() + "'" + "|" + " and date_format(FechaHora, '%Y') = date_format(now(), '%Y') and date_format(FechaHora, '%m') = '" + messel.ToString() + "'";
            }
            else if (string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex > 0 && cmbMes.SelectedIndex > 0 && cbFecha.Checked == false)
            {
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa + "' and t2.codrefaccion = '" + txtcodigo.Text + "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "' and date_format(fechaHoraPedido, '%m') = '" + messel.ToString() + "'", "where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.Cancelado = 0 and t2.codrefaccion = '" + txtcodigo.Text + "' and t2.empresa = '" + empresa + "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "' and date_format(FechaHora, '%m') = '" + messel.ToString() + "'");
                cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and date_format(fechaHoraPedido, '%m') = '" + messel.ToString() + "'" + "|" + " and date_format(FechaHora, '%Y') = date_format(now(), '%Y') and date_format(FechaHora, '%m') = '" + messel.ToString() + "'";
            }
            else if (!string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex > 0 && cbFecha.Checked == false)
            {
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa + "' and date_format(fechaHoraPedido, '%m') = '" + messel.ToString() + "'", "where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.Cancelado = 0 and date_format(FechaHora, '%m') = '" + messel.ToString() + "'");
                cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and date_format(fechaHoraPedido, '%m') = '" + messel.ToString() + "'" + "|" + " and date_format(FechaHora, '%Y') = date_format(now(), '%Y') and date_format(FechaHora, '%m') = '" + messel.ToString() + "'";
            }
            else if (!string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex > 0 && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') and t2.empresa = '" + empresa +  "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "'", "where date_format(FechaHora, '%Y') = date_format(now(), '%Y') and t1.Cancelado = 0 and t2.codrefaccion = '" + txtcodigo.Text + "' and t2.empresa = '" + empresa + "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "' and date_format(FechaHora, '%m') = '" + messel.ToString() + "'");
                cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y') " + "|" + " and date_format(FechaHora, '%Y') = date_format(now(), '%Y')";
            }
            else if (cbFecha.Checked == true && string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex == 0)
            {
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'and t2.empresa = '" + empresa + "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "'", "where date_format(FechaHora, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "' and t1.Cancelado = 0  and t2.empresa = '" + empresa +"'");
                cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'|" + " and date_format(FechaHora, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") +"'";
            }
            else if (cbFecha.Checked == true && !string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex == 0 && cmbMes.SelectedIndex == 0)
            {
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'and t2.empresa = '" + empresa  + "' and t2.codrefaccion = '" + txtcodigo.Text +"'", "where date_format(FechaHora, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "' and t1.Cancelado = 0 and t2.codrefaccion = '" + txtcodigo.Text + "' and t2.empresa = '" + empresa + "'");
            }
            else if (cbFecha.Checked == true && string.IsNullOrWhiteSpace(txtcodigo.Text) && cmbEmpresa.SelectedIndex > 0 && cmbMes.SelectedIndex == 0)
            {
                ConsultaGenera("where date_format(fechaHoraPedido, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'and t2.empresa = '" + empresa + "' and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "'", "where date_format(FechaHora, '%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "' and t1.Cancelado = 0 and t2.Tipo = '" + cmbEmpresa.SelectedIndex + "' and t2.empresa = '" + empresa + "'");
            }
            else
            {
                MessageBox.Show("!Seleccione Parametros De Busqueda", "!ALERTA¡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Limpiar();
        }

        private void Buscar_Fecha(object sender, EventArgs e)
        {
           
        }
        Thread hiloEx2;
        bool exportando = false;
        private void Excel(object sender, EventArgs e)
        {
            exportando = true;
            ThreadStart excel = new ThreadStart(Excel_Export);
            hiloEx2 = new Thread(excel);
            hiloEx2.Start();
        }
        private void Siguiente(object sender, EventArgs e)
        {
            inicio = inicio + 10;
            recorrerE(inicio);
        }
        private void Anterior(object sender, EventArgs e)
        {
            inicio = inicio - 10;
            recorrerE(inicio);
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

        private void PintarCombo(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }


        void ConsultaGenera(string where1, string where2)
        {
            DataSet contar = new DataSet();

            dt = (DataTable)v.getData("SELECT idrefaccion, codrefaccion As CODIGO, nombreRefaccion AS REFACCION, sum(CantidadEntregada) 'Total Salidas', Simbolo, 'MÁS INFORMACIÓN' FROM(SELECT t2.idrefaccion, t2.codrefaccion, t2.nombreRefaccion, t1.CantidadEntregada, t5.Simbolo, 'MÁS INFORMACIÓN' FROM pedidosrefaccion as t1 inner join crefacciones as t2 on t2.idrefaccion = t1.RefaccionfkCRefaccion inner join cmarcas as t3 on t3.idmarca = t2.marcafkcmarcas inner join cfamilias as t4 on t4.idfamilia = t3.descripcionfkcfamilias inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida " + where1 + " union all SELECT t2.idrefaccion, t2.codrefaccion, t2.nombreRefaccion, t1.CantidadEntregada, t5.Simbolo, 'MÁS INFORMACIÓN' FROM ccarrocero as t1 inner join crefacciones as t2 on t2.idrefaccion = t1.refaccionfkCRefacciones inner join cmarcas as t3 on t3.idmarca = t2.marcafkcmarcas inner join cfamilias as t4 on t4.idfamilia = t3.descripcionfkcfamilias inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida " + where2 + ") t GROUP BY idrefaccion");
            dgvSalidas.Rows.Clear();
            int numFila = dt.Rows.Count;
            if (numFila > 0)
            {
                for (int i = 0; i < numFila; i++)
                {
                    dgvSalidas.Rows.Add(dt.Rows[i].ItemArray);
                }
            }
            /* adaptador = (MySqlDataAdapter)v.getReport("SELECT codrefaccion As CODIGO,nombreRefaccion AS REFACCION, sum(CantidadEntregada) 'Total Salidas', Simbolo FROM (SELECT  t2.codrefaccion, t2.nombreRefaccion, t1.CantidadEntregada, t5.Simbolo FROM pedidosrefaccion as t1 inner join crefacciones as t2 on t2.idrefaccion = t1.RefaccionfkCRefaccion inner join cmarcas as t3 on t3.idmarca = t2.marcafkcmarcas inner join cfamilias as t4 on t4.idfamilia = t3.descripcionfkcfamilias inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida " + where1 + " union all SELECT t2.codrefaccion, t2.nombreRefaccion, t1.CantidadEntregada, t5.Simbolo FROM ccarrocero as t1 inner join crefacciones as t2 on t2.idrefaccion = t1.refaccionfkCRefacciones inner join cmarcas as t3 on t3.idmarca = t2.marcafkcmarcas inner join cfamilias as t4 on t4.idfamilia = t3.descripcionfkcfamilias inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida " + where2 + ") t GROUP BY codrefaccion");
             adaptador.Fill(contar);
             if (contar.Tables[0].Rows.Count > 1)
             {
                 adaptador.Fill(ds, 0, 10, "Entradas");
                 dgvSalidas.DataSource = ds.Tables[0];
                 gvimprimir.DataSource = contar.Tables[0];
             }
             else
             {
                 adaptador.Fill(ds, 0, 1, "Entradas");
                 dgvSalidas.DataSource = ds.Tables[0];
                 gvimprimir.DataSource = contar.Tables[0];
             }*/

        }
        void Limpiar()
        {
            txtcodigo.Text = "";
            cmbEmpresa.SelectedIndex = cmbMes.SelectedIndex = 0;
            buttonExcel.Visible = label35.Visible = true;
            cbFecha.Checked = false;
        }
        public void recorrerE(int valor)
        {
            ds.Clear();
            if (valor >= 0)
            {
                adaptador.Fill(ds, valor, 10, "Entradas");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dgvSalidas.DataSource = ds.Tables[0];
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
        bool activo = false;
        public void carga1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            pictureBoxExcelLoad.Visible = true;
            buttonExcel.Visible = false;
            label35.Location = new Point(486, 174);
            label35.Text = "EXPORTANDO";
        }

        private void dgvSalidas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == 5 || e.ColumnIndex == 6))
            {
                bool historial = (e.ColumnIndex == 6);
                string id = v.mayusculas(dgvSalidas.Rows[e.RowIndex].Cells[0].Value.ToString());
                cadenaBusqueda = cadenaBusqueda + "|" + id;
                masInfoES ifmormacion = new masInfoES(v, IdUsuario, empresa, area, cadenaBusqueda, "Salidas");
                ifmormacion.ShowDialog();

            }
        }

        delegate void Loading1();
        public void carga2()
        {
            pictureBoxExcelLoad.Image = null;
            pictureBoxExcelLoad.Visible = false;
            buttonExcel.Visible = true;
            label35.Location = new Point(486, 174);
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
        private void Excel_Export()
        {
            if (dgvSalidas.Rows.Count > 0)
            {
                //isexporting = true;
                //dt = (DataTable)dgvSalidas.DataSource;
                if (this.InvokeRequired)
                {
                    Loading delega = new Loading(carga1);
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
                    Loading1 delega2 = new Loading1(carga2);
                    this.Invoke(delega2);
                }

            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
