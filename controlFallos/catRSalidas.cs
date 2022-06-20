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
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;
using SpreadsheetLight;
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
        public Thread hilo, th;
        public catRSalidas()
        {
            InitializeComponent();
        }
        public catRSalidas(validaciones v, int empresa, int area, int IdUsuario)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
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
            cadenaBusqueda = " and date_format(fechaHoraPedido, '%Y') = date_format(now(), '%Y')" + "|" + " and date_format(FechaHora, '%Y') = date_format(now(), '%Y')";
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
            /*
                        exportando = true;
                        ThreadStart excel = new ThreadStart(Excel_Export);
                        hiloEx2 = new Thread(excel);
                        hiloEx2.Start();
            */
            Excel_Export();
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
            /*
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
            foreach (DataGridViewColumn column in dgvSalidas.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;


            }



            int ir = 9;
            foreach (DataGridViewRow row in dgvSalidas.Rows)
            {

                sl.SetCellValue(ir, 2, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[4].Value.ToString());

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
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Consulta Entradas");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "G" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "CONSULTA TOTAL ENTRADAS");
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
            sl.SetCellStyle("B" + celdaInicial, "G" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "G");
            //Estilos Para bordes de la tabla

            //Extraer fecha

            sl.SetCellValue("G3", "FECHA/HORA DE CONSULTA:");
            SLStyle estiloF = sl.CreateStyle();
            estiloF.Font.FontName = "Arial";
            estiloF.Font.FontSize = 9;
            estiloF.Font.Bold = true;
            sl.SetCellStyle("G3", estiloF);
            sl.MergeWorksheetCells("G3", "H3");

            sl.SetCellValue("G4", "RANGO CONSULTA DE:");
            SLStyle estiloF3 = sl.CreateStyle();
            estiloF3.Font.FontName = "Arial";
            estiloF3.Font.FontSize = 9;
            estiloF3.Font.Bold = true;
            sl.SetCellStyle("G4", estiloF3);
            sl.MergeWorksheetCells("G4", "H4");

            sl.SetCellValue("G5", "RANGO CONSULTA A:");
            SLStyle estiloF2 = sl.CreateStyle();
            estiloF2.Font.FontName = "Arial";
            estiloF2.Font.FontSize = 9;
            estiloF2.Font.Bold = true;
            sl.SetCellStyle("G5", estiloF2);
            sl.MergeWorksheetCells("G5", "H5");


            var datestring3 = dtpFechaDe.Value.ToLongDateString();

            sl.SetCellValue("I4", datestring3);
            SLStyle fechaDe = sl.CreateStyle();
            fechaDe.Font.FontName = "Arial";
            fechaDe.Font.FontSize = 10;
            fechaDe.Font.Bold = true;
            sl.SetCellStyle("I4", fechaDe);

            var datestring2 = dtpFechaA.Value.ToLongDateString();

            sl.SetCellValue("I5", datestring2);
            SLStyle fechaA = sl.CreateStyle();
            fechaA.Font.FontName = "Arial";
            fechaA.Font.FontSize = 10;
            fechaA.Font.Bold = true;
            sl.SetCellStyle("I5", fechaA);
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
