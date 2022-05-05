using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using h = Microsoft.Office.Interop.Excel;

namespace Reportes
{
    public partial class Form1 : Form
    {
        intermedio comunicador = new intermedio();
        DataSet ds = new DataSet();
        DataSet dsE = new DataSet();
        private DataGridView songsDataGridView = new DataGridView();
        DataSet ds2 = new DataSet();
        DataTable dt = new DataTable();
        string ConsultaGeneral = "SET lc_time_names = 'es_ES';select DISTINCT convert(t2.consecutivo,char) as Economico, convert(t1.codrefaccion,char) as Codigo, convert(t1.nombreRefaccion,char) as Nombre, convert(t3.Cantidad,char) as Cantidad,convert(t1.CostoUni,char) as Costo, upper(date_format(convert(t3.fechaHoraPedido, char),'%d/%M/%Y')) as Salida from crefacciones as t1 inner join pedidosrefaccion as t3 on t1.idrefaccion = t3.RefaccionfkCRefaccion inner join reportemantenimiento as t4 inner join reportesupervicion as t5 on t5.idReporteSupervicion = t3.FolioPedfkSupervicion inner join cunidades as t2 on t2.idunidad = t5.UnidadfkCUnidades";
        string[] meses = { "---Seleccione una opcion--", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        string unidades = "SET lc_time_names = 'es_ES';select convert(idunidad,char) as idunidad, convert(consecutivo,char) as Eco from cunidades order by consecutivo asc";

        string EntradasC = "SET lc_time_names='es_ES';SELECT convert(t3.nombreEmpresa, char) as Empresa, convert(t4.codrefaccion, char) as Codigo, convert(t4.nombreRefaccion, char) as Refaccion, convert(t4.modeloRefaccion, char) as Modelo, convert(t1.ultimaModificacion, char) as Cantidad, convert(t4.CostoUni, char) as Costo, convert(t1.motivoActualizacion, char) as Actualizacion, convert(t2.usuario, char) as Usuario,  upper(date_format(convert(t1.fechaHora, char),'%d/%m/%Y')) as Fecha FROM modificaciones_sistema as t1 inner join datosistema as t2 on t1.usuariofkcpersonal = t2.usuariofkcpersonal inner join cempresas as t3 on t1.empresa = t3.idempresa inner join crefacciones as t4 on t4.idrefaccion = t1.idregistro ";
        string ConsultaEntradas = "";
        int inicio;
       
        public Form1()
        {
            InitializeComponent();
            inicio = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rbSalidas.Checked == true)
            {
                ds.Clear();
                //dataGridView1.Columns.Clear();
                Salidas();
            }
            else if (rbtEntradas.Checked == true)
            {
                dsE.Clear();
                //dataGridView1.Columns.Clear();
                Entradas();
            }
            cmbMes.SelectedIndex = 0;
            cmbUnidad.SelectedIndex = 0;
            txtCodigo.Text = "";
            dtFecha1.Enabled = false;
            dtFecha2.Enabled = false;
            cbFecha.Checked = false;
           // ds = comunicador.datosG(ConsultaGeneral);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (rbSalidas.Checked == true)
            {
                ds2 = comunicador.exportar("S");
                dataGridView2.DataSource = ds2.Tables[0];
                exportar_excel();
            }
            else if (rbtEntradas.Checked == true)
            {
                ds2 = comunicador.exportar("E");
                dataGridView2.DataSource = ds2.Tables[0];
                exportar_excel();
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbMes.DataSource = meses;
            comunicador.llenarCombo(unidades, cmbUnidad, "idunidad", "ECo", "----Seleccione unidad----");
            //ds =comunicador.datosG(ConsultaGeneral + " where left(right(t5.FechaReporte,5),2) =left(right(now(),14),2) and t4.StatusRefacciones = 1;");
             //dataGridView1.DataSource = ds.Tables[0];
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (rbSalidas.Checked == true)
            {
                inicio = inicio + 10;
                ds.Clear();
                ds = comunicador.recorrerS(inicio);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    btnSiguiente.Enabled = true;
                    btnAnterior.Enabled = true;
                }
                else
                {
                    btnAnterior.Enabled = true;
                    btnSiguiente.Enabled = false;
                }
                
            }
            else if (rbtEntradas.Checked == true)
            {
                inicio = inicio + 10;
                dsE.Clear();
                dsE = comunicador.recorrerE(inicio);
                if (dsE.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsE.Tables[0];
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

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (rbSalidas.Checked == true)
            {
                inicio = inicio - 10;
                ds.Clear();
                ds = comunicador.recorrerS(inicio);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    btnSiguiente.Enabled = true;
                    btnAnterior.Enabled = true;
                }
                else
                {
                    btnAnterior.Enabled = false;
                    btnSiguiente.Enabled = true;
                }
                
            }
             else if (rbtEntradas.Checked == true)
            {
                  inicio = inicio - 10;
                dsE.Clear();
                dsE = comunicador.recorrerE(inicio);
                if (dsE.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsE.Tables[0];
                    btnSiguiente.Enabled = true;
                    btnAnterior.Enabled = true;
                }
                else
                {
                    btnAnterior.Enabled = false;
                    btnSiguiente.Enabled = true;
                }
             }
        }

        private void cbFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFecha.Checked == true)
            {
                dtFecha1.Enabled = true;
                dtFecha2.Enabled = true;
            }
            
        }

        public void Entradas()
        {
            inicio = 10;
            /*dsE = comunicador.datosEn(EntradasC + " where form = 'Catálogo de Refacciones' and tipo ='Actualización de Existencias' and left(right(fechaHora,14),2) =left(right(now(),14),2)", inicio);
            dataGridView1.DataSource = dsE.Tables[0];*/
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                dsE.Clear();
                dsE = comunicador.datosEn(EntradasC + " where form = 'Catálogo de Refacciones' and tipo ='Actualización de Existencias' and t4.codrefaccion = '" + txtCodigo.Text + "'", inicio);
                if (dsE.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsE.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
            }
            
            if (cmbMes.SelectedIndex != 0)
            {
                string messel = "";
                dsE.Clear();
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }

                dsE = comunicador.datosEn(EntradasC + " where form = 'Catálogo de Refacciones' and tipo ='Actualización de Existencias' and left(right(fechaHora,14),2) ='" + messel + "'", inicio);
                if (dsE.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsE.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cbFecha.Checked == true)
            {
                dsE = comunicador.datosEn(EntradasC + " where form = 'Catálogo de Refacciones' and tipo ='Actualización de Existencias' and date_format(convert(t1.fechaHora, char),'%d/%m/%Y') between'" + dtFecha1.Value.ToString("dd/MM/yyyy") + "' and '" + dtFecha2.Value.ToString("dd/MM/yyyy") + "'", inicio);
                if (dsE.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsE.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void Salidas()
        {
            inicio = 10;
            if (!string.IsNullOrEmpty(txtCodigo.Text))
            {
                ds.Clear();
                ds = comunicador.datosG(ConsultaGeneral + " where t1.codrefaccion = '" + txtCodigo.Text + "'", inicio);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbUnidad.SelectedIndex != 0)
            {
                ds.Clear();
                ds = comunicador.datosG(ConsultaGeneral + " where t2.consecutivo = '" + cmbUnidad.Text + "'", inicio);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbMes.SelectedIndex != 0)
            {
                string messel = "";
                ds.Clear();
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }

                ds = comunicador.datosG(ConsultaGeneral + " where left(right(t5.FechaReporte,5),2) = '" + messel + "'", inicio);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cbFecha.Checked == true)
            {
                ds = comunicador.datosG(ConsultaGeneral + " where date_format(convert(t3.fechaHoraPedido, char),'%d/%m/%Y') between'" + dtFecha1.Value.ToString("dd/MM/yyyy") + "' and '" + dtFecha2.Value.ToString("dd/MM/yyyy") + "'", inicio);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
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
                    sheet.Cells[1, i] = dt.Columns[i-1].ColumnName.ToUpper();
                    /*rng.Interior.Color = System.Drawing.Color.Crimson;
                    rng.Borders.Color = System.Drawing.Color.Black;
                    rng.Font.Color = System.Drawing.Color.White;*/
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
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j +1]; 
                            sheet.Cells[i + 2, j+1] = dt.Rows[i][j].ToString();
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Cells.Font.Name = "Calibri";
                            rng.Cells.Font.Size = 11;
                            rng.Font.Bold = false;
                            //rng.Interior.Color = Color.FromArgb(231, 230, 230);
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
                
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void rbSalidas_CheckedChanged(object sender, EventArgs e)
        {
            activarControles();
        }

        private void rbtEntradas_CheckedChanged(object sender, EventArgs e)
        {
            cmbUnidad.Enabled = false;
            cmbMes.Enabled = true;
            cbFecha.Enabled = true;
            txtCodigo.Enabled = true;
            button1.Enabled = true;
        }
        public void activarPaginado()
        {
            btnAnterior.Enabled = true;
            btnSiguiente.Enabled = true;
        }
        public void activarControles()
        {
            cmbUnidad.Enabled = true;
            cmbMes.Enabled = true;
            cbFecha.Enabled = true;
            txtCodigo.Enabled = true;
            button1.Enabled = true;
        }

    }
}
