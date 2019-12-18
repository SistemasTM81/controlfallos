using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using h = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace controlFallos
{
    public partial class ReportePersonal : Form
    {
        validaciones v;

        int idcred, idreporteP, consecutivoReporteP, empresa, area, totalvalidaciontxt, reporte, numhuella;
        public int hresponsable = 0, hcoordinador = 0, idUsuario, idFinal;
        string tipo, estatus; 
        bool validaciontxt = false;
        bool exportando = false, banderaeditar;
        Thread th;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }

        public Thread hilo;

        //VARIABLES ANTERIORES

        string credencialanterior, fechaanterior, horanterior, lugarincidenteanterior, tipovehiculobjetoanterior, kilometrajeanterior, observacionesanterior, idcoord, idresp;

        //VARIABLES PDF

        string nombrePDF, credencialPDF, FechaPDF, HoraDF, LugarIncidentePDF, tipovehiculobjetoPDF, KilometrajePDF, observacionesPDF, consecutivoReportePPDF, nombreReporte, Codigo, Vigencia, Revision, responsable, coordinador;

        public ReportePersonal(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idUsuario;
            //dtpFecha.MinDate = DateTime.Today.Subtract(TimeSpan.FromDays(2));
            //dtpFecha.MaxDate = DateTime.Now.AddDays(1);
        }

        private void ReportePersonal_Load(object sender, EventArgs e)
        {
            privilegios();
            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "HH:mm";
            cbxMesB.SelectedIndex = 0;
            chbxFechas.ForeColor = chbxVehiculo.ForeColor = Color.Crimson;
            dtpInicio.Value = dtpFinal.Value = DateTime.Now;
            consultageneral();
            limpiar();
            limpiarvariablesanteriores();
            if (!pinsertar)
                btnGuardar.Visible = label24.Visible = btnFinalizar.Visible = btnHDResponsable.Visible = btnHDCoordinador.Visible = false;
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

        /* MÉTODOS */

        private void txtkm_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtkm = sender as TextBox;
            try
            {
                if (!string.IsNullOrWhiteSpace(txtkm.Text))
                {
                    double km = double.Parse(txtkm.Text);
                    if (txtkm.TextLength <= 3)
                        txtkm.Text = string.Format("{0:F2}", km);
                    else
                    {
                        txtkm.Text = Convert.ToString((Math.Floor(km * 100) / 100));
                        km = double.Parse(txtkm.Text);
                        txtkm.Text = string.Format("{0:N2}", km);
                        if (km > 2000000)
                            txtkm.Text = "2,000,000.00";
                    }
                    Regex r4 = new Regex(@"^\d{1,3}\,\d{3}\.\d{2,2}$");
                    Regex r5 = new Regex(@"^\d{1,3}\,\d{3}\,\d{3}\.\d{2,2}");
                    Regex r1 = new Regex(@"^\d{0,3}\.\d{1,2}$");
                    if (!r1.IsMatch(txtkm.Text) && !r4.IsMatch(txtkm.Text) && !r5.IsMatch(txtkm.Text))
                    {
                        MessageBox.Show("El formato del kilometraje ingresado es incorrecto".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtkm.Focus();
                        txtkm.Clear();
                    }
                }
            }
            catch
            {
                MessageBox.Show("El formato del kilometraje ingresado es incorrecto".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtkm.Focus();
                txtkm.Clear();
            }
        }

        public void privilegios()
        {
            string sql = "SELECT privilegios FROM privilegios where usuariofkcpersonal = '" + idUsuario + "' and namform = 'repPersonal'";
            string[] privilegios = getaData(sql).ToString().Split('/');
            pinsertar = getBoolFromInt(Convert.ToInt32(privilegios[0]));
            pconsultar = getBoolFromInt(Convert.ToInt32(privilegios[1]));
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[2]));
        }

        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }

        public object getaData(string sql)
        {
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            var res = cm.ExecuteScalar();
            v.c.dbconection();
            return res;
        }

        private void escribirFichero(string texto)
        {
            string rutaFichero = Application.StartupPath; ;
            rutaFichero = rutaFichero + "/PDFTempral";
            try
            {
                if (!(Directory.Exists(rutaFichero)))
                    Directory.CreateDirectory(rutaFichero);
            }
            catch (Exception errorC)
            {
                MessageBox.Show("Ha habido un error al intentar " + "crear el fichero temporal:" + Environment.NewLine + Environment.NewLine + rutaFichero + Environment.NewLine + Environment.NewLine + errorC.Message, "Error al crear fichero temporal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void nuevaconsecutiva()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(idreportepersonal) AS CONTADOR FROM reportepersonal", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                consecutivoReporteP = dr.GetInt32("CONTADOR");
                consecutivoReporteP++;
            }
            dr.Close();
            v.c.dbconection().Close();
            gbxReporte.Text = "REPORTE # " + consecutivoReporteP + "";
        }

        public void txtAllNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        public void txtAll_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }

        public void limpiar()
        {
            txtCredencial.Text = txtLugarIncidente.Text = txtTipoVehiculo.Text = txtObservaciones.Text = lblAPaterno.Text = lblAMaterno.Text = lblNombres.Text = lblPaternoResponsable.Text = lblMaternoResponsable.Text = lblNombresResponsable.Text = lblPaternoCoordinador.Text = lblMaternoCoordinador.Text = lblNombresCoordinador.Text = "";
            txtKilometraje.Text = "0";
            ptbxResponsable.Image = ptbxCoordinador.Image = null;
            btnHDResponsable.Enabled = btnHDCoordinador.Enabled = chbxVehiculo.Enabled = true;
            btnNuevo.BackgroundImage = Properties.Resources.eraser1;
            label15.Text = "LIMPIAR CAMPOS";
            dtpFecha.Value = DateTime.Now;
            dtpTime.Text = "00:00";
            chbxVehiculo.Checked = txtKilometraje.Enabled = false;
            nuevaconsecutiva();
        }

        public void limpiarbusq()
        {
            if (lblExcel.Text.Equals("EXPORTANDO"))
                exportando = true;
            else
                btnExcel.Visible = lblExcel.Visible = false;
            btnActualizar.Visible = label37.Visible = false;
            limpiarbusqnormal();
        }

        public void limpiarbusqnormal()
        {
            txtCredencialBusq.Text = txtVehiculoBusq.Text = "";
            cbxMesB.SelectedIndex = 0;
            chbxFechas.Checked = false;
        }

        public void txtkm_KeyPress(object sender, KeyPressEventArgs e)
        {
            char signo_decimal = (char)46;
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == 46)
                e.Handled = false;
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan: numéros y ( . ) en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (e.KeyChar == 46)
                if (txtKilometraje.Text.LastIndexOf(signo_decimal) >= 0)
                    e.Handled = true; // Interceptamos la pulsación para que no permitirla.
        }

        public void combos_para_otros_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    Brush brush = new SolidBrush(cbx.ForeColor);
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();

                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        private void allpeople(string estatuss)
        {
            txtCredencial.Enabled = true; //NO QUITAR, APOYA A LA ACTIVACIÓN DEL EVENTO "LEAVE"
            MySqlCommand cmd = new MySqlCommand("SELECT t1.ConsecutivoRP AS CONSECUTIVO, t2.credencial AS CREDENCIAL, COALESCE(DATE_FORMAT(t1.Fecha, '%d/%m/%Y'), Now()) AS FECHA, COALESCE(t1.Hora, '00:00') AS HORA, t1.LugarIncidente AS 'LUGAR DEL INCIDENTE', t1.TipoVehObj AS 'TIPO DEL VEH / OBJ', COALESCE(t1.Kilometraje, '0') AS KILOMETRAJE, t1.responsablefkcpersonal AS 'IDRESPONSABLE', (SELECT UPPER(b2.ApPaterno) FROM cpersonal AS b2 WHERE b2.idPersona = t1.responsablefkcpersonal) AS 'RESPATERNO', (SELECT UPPER(b3.ApMaterno) FROM cpersonal AS b3 WHERE b3.idPersona = t1.responsablefkcpersonal) AS 'RESMATERNO', (SELECT UPPER(b4.nombres) FROM cpersonal AS b4 WHERE b4.idPersona = responsablefkcpersonal) AS 'RESNOMBRES', t1.coordinadorfkcpersonal AS 'IDCOORDINADOR', (SELECT UPPER(b2.ApPaterno) FROM cpersonal AS b2 WHERE b2.idPersona = t1.coordinadorfkcpersonal) AS 'COOPATERNO', (SELECT UPPER(COALESCE(b3.ApMaterno, '')) FROM cpersonal AS b3 WHERE b3.idPersona = t1.coordinadorfkcpersonal) AS 'COOMATERNO', (SELECT UPPER(b4.nombres) FROM cpersonal AS b4 WHERE b4.idPersona = t1.coordinadorfkcpersonal) AS 'COONOMBRES', COALESCE(t1.Observaciones, '') AS OBSERVACIONES FROM reportepersonal AS t1 INNER JOIN cpersonal AS t2 ON t1.credencialfkcpersonal = t2.idpersona WHERE t1.idreportepersonal = '" + idreporteP + "'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                consecutivoReporteP = dr.GetInt32("CONSECUTIVO");
                txtCredencial.Text = credencialanterior = dr.GetString("CREDENCIAL");
                dtpFecha.Text = fechaanterior = dr.GetString("FECHA");
                dtpTime.Text = horanterior = dr.GetString("HORA");
                txtLugarIncidente.Text = lugarincidenteanterior = dr.GetString("LUGAR DEL INCIDENTE");
                txtTipoVehiculo.Text = tipovehiculobjetoanterior = dr.GetString("TIPO DEL VEH / OBJ");
                txtKilometraje.Text = kilometrajeanterior = dr.GetString("KILOMETRAJE");
                if (string.IsNullOrWhiteSpace(estatus))
                    if (!kilometrajeanterior.Equals("0.00"))
                        if(banderaeditar)
                            chbxVehiculo.Checked = txtKilometraje.Enabled = chbxVehiculo.Enabled = true;
                        else
                            chbxVehiculo.Checked = txtKilometraje.Enabled = chbxVehiculo.Enabled = false;
                    else
                    {
                        chbxVehiculo.Checked = txtKilometraje.Enabled = false;
                        chbxVehiculo.Enabled = true;
                    }
                txtObservaciones.Text = observacionesanterior = dr.GetString("OBSERVACIONES");
                idresp = dr.GetString("IDRESPONSABLE");
                hresponsable = Convert.ToInt16(idresp);
                idcoord = dr.GetString("IDCOORDINADOR");
                hcoordinador = Convert.ToInt16(idcoord);
                if (idresp != "0")
                {
                    lblNombresResponsable.Text = dr.GetString("RESNOMBRES");
                    lblPaternoResponsable.Text = dr.GetString("RESPATERNO");
                    lblMaternoResponsable.Text = dr.GetString("RESMATERNO");
                    ptbxResponsable.Image = Properties.Resources.correct;
                    btnHDResponsable.Enabled = false;
                }
                else
                {
                    lblNombresResponsable.Text = lblPaternoResponsable.Text = lblMaternoResponsable.Text = "";
                    ptbxResponsable.Image = null;
                    btnHDResponsable.Enabled = true;
                }
                if (idcoord != "0")
                {
                    lblNombresCoordinador.Text = dr.GetString("COONOMBRES");
                    lblPaternoCoordinador.Text = dr.GetString("COOPATERNO");
                    lblMaternoCoordinador.Text = dr.GetString("COOMATERNO");
                    ptbxCoordinador.Image = Properties.Resources.correct;
                    btnHDCoordinador.Enabled = false;
                }
                else
                {
                    lblNombresCoordinador.Text = lblNombresCoordinador.Text = lblMaternoCoordinador.Text = "";
                    ptbxCoordinador.Image = null;
                    btnHDCoordinador.Enabled = true;
                }
            }
            dr.Close();
            v.c.dbconection().Close();
            txtCredencial.Focus();
            txtKilometraje.Focus();
            txtCredencial.Focus();
        }

        public void validacioncajastexto()
        {
            if (string.IsNullOrWhiteSpace(txtCredencial.Text))
                txtCredencial.Enabled = true;
            else
                txtCredencial.Enabled = false;
            if (string.IsNullOrWhiteSpace(txtLugarIncidente.Text))
                txtLugarIncidente.Enabled = true;
            else
                txtLugarIncidente.Enabled = false;
            if (string.IsNullOrWhiteSpace(txtTipoVehiculo.Text))
                txtTipoVehiculo.Enabled = true;
            else
                txtTipoVehiculo.Enabled = false;
            if (string.IsNullOrWhiteSpace(txtObservaciones.Text))
                if(estatus == "FINALIZADO")
                    txtObservaciones.Enabled = false;
                else
                    txtObservaciones.Enabled = true;
            else
                txtObservaciones.Enabled = false;
        }

        public void consultageneral()
        {
            DataTable dt = new DataTable();
            MySqlCommand cmd1 = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT t1.idreportepersonal AS 'ID REPORTE', UPPER(CONCAT(DATE_FORMAT(t1.Fecha, '%W %d de %M de %Y'), ' / ', DATE_FORMAT(t1.Hora, '%H:%i'))) AS 'FECHA/HORA', UPPER(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ' , t2.nombres)) AS PERSONAL, IF(t1.estatus = 1, 'FINALIZADO', '') AS ESTATUS, UPPER(t1.LugarIncidente) AS 'LUGAR DEL INCIDENTE', UPPER(t1.Observaciones) AS OBSERVACIONES FROM reportepersonal AS t1 INNER JOIN cpersonal AS t2 ON t1.credencialfkcpersonal = t2.idPersona ORDER BY t1.idreportepersonal DESC", v.c.dbconection());
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd1);
            adp.Fill(dt);
            dgvPMantenimiento.DataSource = null;
            dgvPMantenimiento.DataSource = dt;
            dgvPMantenimiento.ClearSelection();
            dgvPMantenimiento.Columns[0].Visible = false;
        }

        public void dbclick()
        {
            limpiar();
            idreporteP = Convert.ToInt32(dgvPMantenimiento.CurrentRow.Cells["ID REPORTE"].Value);
            estatus = Convert.ToString(dgvPMantenimiento.CurrentRow.Cells["ESTATUS"].Value);
            chbxVehiculo.Enabled = false;
            allpeople(estatus);
            gbxReporte.Text = "REPORTE # " + consecutivoReporteP + "";
            if ((string.IsNullOrWhiteSpace(txtCredencial.Text) || Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd") + " " + dtpTime.Text + ":00") == DateTime.Now || string.IsNullOrWhiteSpace(txtLugarIncidente.Text) || string.IsNullOrWhiteSpace(txtTipoVehiculo.Text) || string.IsNullOrWhiteSpace(txtKilometraje.Text) || btnHDResponsable.Enabled == true || btnHDCoordinador.Enabled == true) && pinsertar)
            {
                btnGuardar.Visible = label24.Visible = true;
                btnFinalizar.Visible = label26.Visible = false;
            }
            else if ((!string.IsNullOrWhiteSpace(txtCredencial.Text) || Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd") + " " + dtpTime.Text + ":00") != DateTime.Now || !string.IsNullOrWhiteSpace(txtLugarIncidente.Text) || !string.IsNullOrWhiteSpace(txtTipoVehiculo.Text) || !string.IsNullOrWhiteSpace(txtKilometraje.Text) || btnHDResponsable.Enabled != true || btnHDCoordinador.Enabled != true) && pinsertar)
            {
                btnGuardar.Visible = label24.Visible = false;
                if (string.IsNullOrWhiteSpace(estatus))
                    btnFinalizar.Visible = label26.Visible = true;
                else
                    btnFinalizar.Visible = label26.Visible = false;
            }
            if (!btnHDResponsable.Enabled && !btnHDCoordinador.Enabled && peditar)
                btnPDF.Visible = label31.Visible = true;
            else if (btnHDResponsable.Enabled && btnHDCoordinador.Enabled && peditar)
                btnPDF.Visible = label31.Visible = false;
            btnGuardar.BackgroundImage = Properties.Resources.guardar__6_;
            label24.Text = "GUARDAR";
            btnNuevo.BackgroundImage = Properties.Resources.test;
            label15.Text = "NUEVO REPORTE";
            dtpFecha.Enabled = dtpTime.Enabled = false;
            validacioncajastexto();
        }

        public void btnuevo()
        {
            consultageneral();
            if (pinsertar)
            {
                btnGuardar.BackgroundImage = Properties.Resources.guardar__6_;
                btnGuardar.Visible = label24.Visible = true;
                btnGuardar.Location = new Point(531, 553);
            }

            btnPDF.Visible = label31.Visible = btnFinalizar.Visible = label26.Visible = false;
            btnNuevo.BackgroundImage = Properties.Resources.eraser1;
            label24.Text = "GUARDAR";
            label15.Text = "LIMPIAR CAMPOS";
            txtCredencial.Enabled = dtpFecha.Enabled = dtpTime.Enabled = txtLugarIncidente.Enabled = txtTipoVehiculo.Enabled = txtKilometraje.Enabled = txtObservaciones.Enabled = true;
            limpiarvariablesanteriores();
            limpiar();
        }

        public void txtaall_TextChanged(object sender, EventArgs e)
        {
            if ((txtCredencial.Enabled || !txtCredencial.Enabled) && !banderaeditar)
            {
                if (txtKilometraje.Text.Equals(".") || string.IsNullOrWhiteSpace(txtKilometraje.Text))
                    txtKilometraje.Text = "0";
                if ((string.IsNullOrWhiteSpace(idcred.ToString()) || Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd") + " " + dtpTime.Text + ":00") == DateTime.Now || string.IsNullOrWhiteSpace(txtLugarIncidente.Text) || string.IsNullOrWhiteSpace(txtTipoVehiculo.Text) || hresponsable == 0 || hcoordinador == 0) && pinsertar)
                {
                    btnGuardar.Visible = label24.Visible = true;
                    btnFinalizar.Visible = label26.Visible = false;
                }
                else if ((!string.IsNullOrWhiteSpace(idcred.ToString()) || Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd") + " " + dtpTime.Text + ":00") != DateTime.Now || !string.IsNullOrWhiteSpace(txtLugarIncidente.Text) || !string.IsNullOrWhiteSpace(txtTipoVehiculo.Text) || hresponsable != 0 || hcoordinador != 0) && pinsertar)
                {
                    btnGuardar.Visible = label24.Visible = false;
                    btnFinalizar.Visible = label26.Visible = true;
                }
            }
        }


        public void exporta_a_excel() //Metodo Que Genera El Excel
        {
            DataTable dtexcel = new DataTable();
            for (int i = 0; i < dgvPMantenimiento.Columns.Count; i++) if (dgvPMantenimiento.Columns[i].Visible) dtexcel.Columns.Add(dgvPMantenimiento.Columns[i].HeaderText);
            for (int j = dgvPMantenimiento.Rows.Count - 1; j >= 0; j--)
            {

                DataRow row = dtexcel.NewRow();
                int indice = 0;
                for (int i = 0; i < dgvPMantenimiento.Columns.Count; i++)
                {

                    if (dgvPMantenimiento.Columns[i].Visible)
                    {
                        row[dtexcel.Columns[indice]] = dgvPMantenimiento.Rows[j].Cells[i].Value;
                        indice++;
                    }

                }
                dtexcel.Rows.Add(row);
            }

            if (dtexcel.Rows.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    Loading load = new Loading(cargando1);
                    this.Invoke(load);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                h.Worksheet sheet = X.ActiveSheet;
                X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                for (int i = 0; i < dtexcel.Columns.Count; i++)
                {
                    h.Range rng = (h.Range)sheet.Cells[1, i + 1];
                    sheet.Cells[1, i + 1] = dtexcel.Columns[i].ColumnName.ToUpper();
                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Crimson);
                    rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                    rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White);
                    rng.Font.FontStyle = "Calibri";
                    rng.Font.Bold = true;
                    rng.Font.Size = 12;
                }

                for (int i = 0; i < dtexcel.Rows.Count; i++)
                {
                    for (int j = 0; j < dtexcel.Columns.Count; j++)
                    {
                        try
                        {
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                            sheet.Cells[i + 2, j + 1] = dtexcel.Rows[i][j].ToString();
                            rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230));
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.FontStyle = "Calibri";
                            rng.Font.Size = 11;
                            if (dtexcel.Rows[i][j].ToString() == "FINALIZADO".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                        }
                        catch (System.NullReferenceException)
                        {
                            hiloEx.Abort();
                        }
                    }
                }
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                Thread.Sleep(500);
                if (this.InvokeRequired)
                {
                    Loading1 load1 = new Loading1(cargando2);
                    this.Invoke(load1);
                }
            }
            else
                MessageBox.Show("Es necesario que existan datos en la tabla para poder generar un archivo de excel \nFavor de actualizar la tabla para que se visualizen los reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        Thread hiloEx;
        delegate void Loading();

        public void cargando1()
        {
            ptbxLoadingExcel.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            lblExcel.Text = "EXPORTANDO";
            lblExcel.Location = new Point(339, 718);
        }

        delegate void Loading1();

        public void cargando2()
        {
            ptbxLoadingExcel.Image = null;
            lblExcel.Text = "EXPORTAR";
            lblExcel.Location = new Point(353, 718);
            btnExcel.Visible = true;
            if (exportando)
                btnExcel.Visible = lblExcel.Visible = false;
            exportando = activado = false;
        }

        private void textBoxAll_Validated(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            while (txt.Text.Contains("  "))
                txt.Text = txt.Text.Replace("  ", " ");
        }

        public void to_pdf()
        {
            MySqlCommand encabezado = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT nombreReporte AS 'NREPORTE', codigoreporte AS 'CODIGO', UPPER(DATE_FORMAT(vigencia, '%M %Y')) AS 'VIGENCIA', revision AS 'REVISION' FROM encabezadoreportes WHERE reporte = 2", v.c.dbconection());
            MySqlDataReader drenca = encabezado.ExecuteReader();
            if (drenca.Read())
            {
                nombreReporte = drenca.GetString("NREPORTE");
                Codigo = drenca.GetString("CODIGO");
                Vigencia = drenca.GetString("VIGENCIA");
                Revision = drenca.GetString("REVISION");
            }
            if (string.IsNullOrWhiteSpace(nombreReporte) && string.IsNullOrWhiteSpace(Codigo) && string.IsNullOrWhiteSpace(Vigencia) && string.IsNullOrWhiteSpace(Revision))
            {
                MessageBox.Show("No existe la información del encabezado en el Reporte de personal, agregue toda la información para poder exportar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReportesVigencias rp = new ReportesVigencias(empresa, area, idUsuario,v);
                rp.Owner = this;
                rp.ShowDialog();
            }
            else
            {
                MySqlCommand cmdbusq = new MySqlCommand("SET lc_time_names = 'ES_ES'; SELECT UPPER(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres)) AS PERSONAL, t1.ConsecutivoRP AS CONSECUTIVO, t2.credencial AS CREDENCIAL, DATE_FORMAT(t1.Fecha, '%W %d de %M de %Y') AS FECHA, DATE_FORMAT(t1.Hora, '%H:%i') AS HORA, CONCAT(UPPER(LEFT(t1.LugarIncidente, 1)), LOWER(SUBSTRING(t1.LugarIncidente, 2))) AS 'LUGAR DE INCIDENTES', 	CONCAT(UPPER(LEFT(t1.TipoVehObj, 1)), LOWER(SUBSTRING(t1.TipoVehObj, 2)))AS 'TIPO DE VEHICULOS / OBJETO INVOLUCRADO',  t1.Kilometraje AS KILOMETRAJES, (SELECT UPPER(CONCAT(b2.ApPaterno, ' ', b2.ApMaterno, ' ', b2.nombres)) FROM cpersonal AS b2 WHERE t1.responsablefkcpersonal = b2.idPersona) AS RESPONSABLE, (SELECT UPPER(CONCAT(b3.ApPaterno, ' ', b3.ApMaterno, ' ', b3.nombres)) FROM cpersonal AS b3 WHERE b3.idPersona = t1.coordinadorfkcpersonal) AS COORDINADOR, COALESCE(CONCAT(UPPER(LEFT(t1.observaciones, 1)), LOWER(SUBSTRING(t1.Observaciones, 2))), '') AS OBSERVACIONES FROM reportepersonal AS t1 INNER JOIN cpersonal AS t2 ON t1.credencialfkcpersonal = t2.idPersona WHERE t1.idreportepersonal = '" + idreporteP + "'", v.c.dbconection());
                MySqlDataReader dr = cmdbusq.ExecuteReader();
                if (dr.Read())
                {
                    nombrePDF = dr.GetString("PERSONAL").ToLower();
                    consecutivoReportePPDF = dr.GetString("CONSECUTIVO");
                    credencialPDF = dr.GetString("CREDENCIAL");
                    FechaPDF = dr.GetString("FECHA");
                    HoraDF = dr.GetString("HORA");
                    LugarIncidentePDF = dr.GetString("LUGAR DE INCIDENTES");
                    tipovehiculobjetoPDF = dr.GetString("TIPO DE VEHICULOS / OBJETO INVOLUCRADO");
                    KilometrajePDF = dr.GetString("KILOMETRAJES");
                    responsable = dr.GetString("RESPONSABLE");
                    coordinador = dr.GetString("COORDINADOR");
                    observacionesPDF = dr.GetString("OBSERVACIONES");
                }
                dr.Close();
                v.c.dbconection().Close();
                drenca.Close();
                v.c.dbconection().Close();
                Document dc = new Document(PageSize.LETTER);
                dc.SetMargins(21f, 21f, 31f, 31f);
                PdfPTable tb = new PdfPTable(4);
                tb.WidthPercentage = 100; // CAMBIAR A 95 SI NO FUNCIONA
                tb.LockedWidth = true;
                float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f, .8f };
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = @"C:\Desktop";
                saveFileDialog1.Title = "Guardar reporte";
                saveFileDialog1.DefaultExt = "pdf";
                saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                escribirFichero("");
                string filename = Application.StartupPath + "/PDFTempral/Orden_" + gbxReporte.Text + DateTime.Today.ToLongDateString() + ".pdf";
                DialogResult ews = DialogResult.OK;
                try
                {
                    if ((ews = saveFileDialog1.ShowDialog()) == DialogResult.OK)
                    {
                        filename = saveFileDialog1.FileName;
                        string p = Path.GetExtension(filename);
                        if (p.ToLower() != ".pdf")
                            filename = filename + ".pdf";
                    }
                    if (ews == DialogResult.OK)
                    {
                        if (filename.Trim() != "")
                        {
                            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                            PdfWriter writer = PdfWriter.GetInstance(dc, file);
                            dc.Open();
                            byte[] img = Convert.FromBase64String(v.transmasivo);
                            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                            imagen.ScalePercent(11f);
                            imagen.SetAbsolutePosition(60, 686);
                            dc.Add(imagen);

                            PdfContentByte cb = writer.DirectContent;
                            cb.SetLineWidth(0.5f);
                            int y = 431;
                            for (int i = 1; i <= 20; i++)
                            {
                                cb.MoveTo(531, y);
                                cb.LineTo(81, y);
                                y = y - 16;
                            }
                            cb.Stroke();
                            PdfPTable tball = new PdfPTable(19);
                            tball.DefaultCell.Border = 1;
                            tball.WidthPercentage = 100; // CAMBIAR A 95
                            tball.HorizontalAlignment = Element.ALIGN_CENTER;
                            PdfPCell c0s1 = new PdfPCell();
                            c0s1.Border = 0;
                            c0s1.BorderColorLeft = c0s1.BorderColorTop = BaseColor.BLACK;
                            c0s1.BorderWidthLeft = c0s1.BorderWidthTop = 2f;
                            tball.AddCell(c0s1);
                            PdfPCell c0s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c0s2_18.Colspan = 16;
                            c0s2_18.Border = 0;
                            c0s2_18.BorderColorTop = BaseColor.BLACK;
                            c0s2_18.BorderWidthTop = 2f;
                            tball.AddCell(c0s2_18);
                            PdfPCell c0s19 = new PdfPCell();
                            c0s19.Border = 0;
                            c0s19.BorderColorRight = c0s19.BorderColorTop = BaseColor.BLACK;
                            c0s19.BorderWidthRight = c0s19.BorderWidthTop = 2f;
                            tball.AddCell(c0s19);
                            PdfPCell c1_47s1 = new PdfPCell();
                            c1_47s1.Rowspan = 49; //AQUI ES 49
                            c1_47s1.Border = 0;
                            c1_47s1.BorderColorLeft = c1_47s1.BorderColorBottom = BaseColor.BLACK;
                            c1_47s1.BorderWidthLeft = c1_47s1.BorderWidthBottom = 2f;
                            tball.AddCell(c1_47s1);
                            PdfPCell c1_4s2_6 = new PdfPCell();
                            c1_4s2_6.Colspan = 5;
                            c1_4s2_6.Rowspan = 4;
                            tball.AddCell(c1_4s2_6);
                            PdfPCell c1s7_15 = new PdfPCell(new Phrase("NONMBRE: " + nombreReporte, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c1s7_15.Colspan = 9;
                            c1s7_15.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c1s7_15);
                            PdfPCell c1s16_18 = new PdfPCell(new Phrase("NÚMERO: " + consecutivoReporteP, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c1s16_18.Colspan = 3;
                            c1s16_18.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c1s16_18);
                            PdfPCell c1_47s19 = new PdfPCell();
                            c1_47s19.Rowspan = 49; //AQUI ES 47
                            c1_47s19.Border = 0;
                            c1_47s19.BorderColorRight = c1_47s19.BorderColorRight = BaseColor.BLACK;
                            c1_47s19.BorderWidthRight = c1_47s19.BorderWidthBottom = 2f;
                            tball.AddCell(c1_47s19);
                            PdfPCell c2_3s7_18 = new PdfPCell(new Phrase("CÓDIGO: " + Codigo, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c2_3s7_18.Colspan = 12;
                            c2_3s7_18.Rowspan = 2;
                            c2_3s7_18.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c2_3s7_18);
                            PdfPCell c4s8 = new PdfPCell(new Phrase("VIGENCIA: " + Vigencia, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //LA FECHA DEBE DE SER DINÁMICA (SI, NUEVO FORMULARIO)
                            c4s8.Colspan = 6;
                            c4s8.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c4s8);
                            PdfPCell c4s13 = new PdfPCell(new Phrase("REVISIÓN: " + Revision, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //PREGUNTAR POR EL NÚMERO DE REVISIONES (POR EDICION)
                            c4s13.Colspan = 3;
                            c4s13.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c4s13);
                            PdfPCell c4s16 = new PdfPCell(new Phrase("PÁGINA 1 DE 1", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c4s16.Colspan = 3;
                            c4s16.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c4s16);
                            PdfPCell c5s2_18 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c5s2_18.Colspan = 17;
                            c5s2_18.Border = 0;
                            tball.AddCell(c5s2_18);
                            PdfPCell c6_46s2 = new PdfPCell();
                            c6_46s2.Rowspan = 43; // AQUI ES 40
                            c6_46s2.Border = 0;
                            c6_46s2.BorderColorBottom = BaseColor.BLACK;
                            c6_46s2.BorderWidthBottom = 2f;
                            tball.AddCell(c6_46s2);
                            PdfPCell c6_7s3_17 = new PdfPCell(new Phrase("Reporte de Personal", FontFactory.GetFont("CALIBRI", 18, iTextSharp.text.Font.BOLD)));
                            c6_7s3_17.Colspan = 15;
                            c6_7s3_17.Rowspan = 2;
                            c6_7s3_17.Border = 0;
                            c6_7s3_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            c6_7s3_17.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tball.AddCell(c6_7s3_17);
                            PdfPCell c6_21s18 = new PdfPCell();
                            c6_21s18.Rowspan = 16; //AQUI ES 40
                            c6_21s18.Border = 0;
                            tball.AddCell(c6_21s18);
                            PdfPCell c8s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c8s3_17.Colspan = 15;
                            c8s3_17.Border = 0;
                            tball.AddCell(c8s3_17);
                            PdfPCell c9s3_17 = new PdfPCell(new Phrase("NOMBRE DEL COLABORADOR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c9s3_17.Colspan = 15;
                            c9s3_17.Border = 0;
                            tball.AddCell(c9s3_17);
                            PdfPCell c10s3_16 = new PdfPCell(new Phrase(v.mayusculas(nombrePDF), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //NOMBRE DINÁMICO
                            c10s3_16.Colspan = 14;
                            c10s3_16.Border = 0;
                            c10s3_16.BorderColorBottom = BaseColor.BLACK;
                            c10s3_16.BorderWidthBottom = 0.5f;
                            c10s3_16.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c10s3_16);
                            PdfPCell c10s17 = new PdfPCell(new Phrase("KO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.BaseColor.WHITE)));
                            c10s17.Border = 0;
                            c10s17.BorderColorBottom = BaseColor.BLACK;
                            c10s17.BorderWidthBottom = 0.5f;
                            tball.AddCell(c10s17);
                            PdfPCell c11s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c11s3_17.Colspan = 15;
                            c11s3_17.Border = 0;
                            tball.AddCell(c11s3_17);
                            PdfPCell c12s3 = new PdfPCell(new Phrase("CRED", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c12s3.Border = 0;
                            c12s3.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c12s3);
                            PdfPCell c12s4_6 = new PdfPCell(new Phrase(credencialPDF, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL))); //CREDENCIAL DINÁMICA
                            c12s4_6.Colspan = 3;
                            c12s4_6.Border = 0;
                            c12s4_6.BorderColorBottom = BaseColor.BLACK;
                            c12s4_6.BorderWidthBottom = 0.5f;
                            c12s4_6.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c12s4_6);
                            PdfPCell c12s7_8 = new PdfPCell(new Phrase("FECHA", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c12s7_8.Colspan = 2;
                            c12s7_8.Border = 0;
                            c12s7_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c12s7_8);
                            PdfPCell c12s9_13 = new PdfPCell(new Phrase(v.mayusculas(FechaPDF), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c12s9_13.Colspan = 5;
                            c12s9_13.Border = 0;
                            c12s9_13.BorderColorBottom = BaseColor.BLACK;
                            c12s9_13.BorderWidthBottom = 0.5f;
                            c12s9_13.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c12s9_13);
                            PdfPCell c12s14_15 = new PdfPCell(new Phrase("HORA", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c12s14_15.Colspan = 2;
                            c12s14_15.Border = 0;
                            c12s14_15.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c12s14_15);
                            PdfPCell c12s16_17 = new PdfPCell(new Phrase(HoraDF, FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.NORMAL)));
                            c12s16_17.Colspan = 2;
                            c12s16_17.Border = 0;
                            c12s16_17.BorderColorBottom = BaseColor.BLACK;
                            c12s16_17.BorderWidthBottom = 0.5f;
                            c12s16_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c12s16_17);
                            PdfPCell c13s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c13s3_17.Colspan = 15;
                            c13s3_17.Border = 0;
                            tball.AddCell(c13s3_17);
                            PdfPCell c14s3_6 = new PdfPCell(new Phrase("LUGAR DEL INCIDENTE", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c14s3_6.Colspan = 4;
                            c14s3_6.Border = 0;
                            c14s3_6.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c14s3_6);
                            PdfPCell c14s7_17 = new PdfPCell(new Phrase(LugarIncidentePDF, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c14s7_17.Colspan = 11;
                            c14s7_17.Border = 0;
                            c14s7_17.BorderColorBottom = BaseColor.BLACK;
                            c14s7_17.BorderWidthBottom = 0.5f;
                            c14s7_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c14s7_17);
                            PdfPCell c15s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c15s3_17.Colspan = 15;
                            c15s3_17.Border = 0;
                            tball.AddCell(c15s3_17);
                            PdfPCell c16s3_9 = new PdfPCell(new Phrase("TIPO DE VEHÍCULO U OBJETO INVOLUCRADO", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c16s3_9.Colspan = 7;
                            c16s3_9.Border = 0;
                            c16s3_9.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c16s3_9);
                            PdfPCell c16s10_17 = new PdfPCell(new Phrase(tipovehiculobjetoPDF, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c16s10_17.Colspan = 8;
                            c16s10_17.Border = 0;
                            c16s10_17.BorderColorBottom = BaseColor.BLACK;
                            c16s10_17.BorderWidthBottom = 0.5f;
                            c16s10_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c16s10_17);
                            PdfPCell c17s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c17s3_17.Colspan = 15;
                            c17s3_17.Border = 0;
                            tball.AddCell(c17s3_17);
                            PdfPCell c18s3_11 = new PdfPCell(new Phrase("KILOMETRAJE ACTUAL (EN CASO DE SER VEHÍCULO)", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c18s3_11.Colspan = 9;
                            c18s3_11.Border = 0;
                            c18s3_11.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c18s3_11);
                            PdfPCell c18s12_17 = new PdfPCell(new Phrase(KilometrajePDF, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c18s12_17.Colspan = 6;
                            c18s12_17.Border = 0;
                            c18s12_17.BorderColorBottom = BaseColor.BLACK;
                            c18s12_17.BorderWidthBottom = 0.5f;
                            c18s12_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c18s12_17);
                            PdfPCell c19s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c19s3_17.Colspan = 15;
                            c19s3_17.Border = 0;
                            tball.AddCell(c19s3_17);
                            PdfPCell c20s3_17 = new PdfPCell(new Phrase("DESPERFECTOS ENCONTRADOS U OBSERVACIONES A REPORTAR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c20s3_17.Colspan = 15;
                            c20s3_17.Border = 0;
                            c20s3_17.HorizontalAlignment = Element.ALIGN_LEFT;
                            tball.AddCell(c20s3_17);
                            PdfPCell c21_42s3_17 = new PdfPCell(new Phrase(observacionesPDF, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c21_42s3_17.Colspan = 15;
                            c21_42s3_17.Rowspan = 22;
                            c21_42s3_17.Border = 0;
                            c21_42s3_17.SetLeading(0, 2);
                            c21_42s3_17.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            tball.AddCell(c21_42s3_17);
                            PdfPCell c21_42s18 = new PdfPCell(new Phrase("1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n11\n12\n13\n14\n15\n16\n17\n18\n19\n20\n21\n22\n23\n24", FontFactory.GetFont("CALIBRI", 15, iTextSharp.text.BaseColor.WHITE)));
                            c21_42s18.Rowspan = 22;
                            c21_42s18.Border = 0;
                            tball.AddCell(c21_42s18);
                            PdfPCell c43_44s3_17 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c43_44s3_17.Colspan = 15;
                            c43_44s3_17.Rowspan = 2;
                            c43_44s3_17.Border = 0;
                            tball.AddCell(c43_44s3_17);
                            PdfPCell c43_47s18 = new PdfPCell();
                            c43_47s18.Rowspan = 4;
                            c43_47s18.Border = 0;
                            tball.AddCell(c43_47s18);
                            PdfPCell c45s3_8 = new PdfPCell(new Phrase(responsable, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c45s3_8.Colspan = 6;
                            c45s3_8.Border = 0;
                            c45s3_8.BorderColorBottom = BaseColor.BLACK;
                            c45s3_8.BorderWidthBottom = 1f;
                            c45s3_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c45s3_8);
                            PdfPCell c45_46s9_11 = new PdfPCell(new Phrase(" \n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c45_46s9_11.Colspan = 3;
                            c45_46s9_11.Rowspan = 3;
                            c45_46s9_11.Border = 0;
                            tball.AddCell(c45_46s9_11);
                            PdfPCell c45s12_17 = new PdfPCell(new Phrase(coordinador, FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.NORMAL)));
                            c45s12_17.Colspan = 6; //HEY
                            c45s12_17.Border = 0;
                            c45s12_17.BorderColorBottom = BaseColor.BLACK;
                            c45s12_17.BorderWidthBottom = 1f;
                            c45s12_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c45s12_17);
                            PdfPCell c46s3_8 = new PdfPCell(new Phrase("FIRMA DEL RESPONSABLE", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c46s3_8.Colspan = 6;
                            c46s3_8.Border = 0;
                            c46s3_8.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c46s3_8);
                            PdfPCell c46s12_17 = new PdfPCell(new Phrase("FIRMA DEL COORDINADOR", FontFactory.GetFont("CALIBRI", 9, iTextSharp.text.Font.BOLD)));
                            c46s12_17.Colspan = 6;
                            c46s12_17.Border = 0;
                            c46s12_17.HorizontalAlignment = Element.ALIGN_CENTER;
                            tball.AddCell(c46s12_17);
                            PdfPCell c47s2_18 = new PdfPCell(new Phrase(" \n\n ", FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
                            c47s2_18.Colspan = 17;
                            c47s2_18.Rowspan = 2;
                            c47s2_18.Border = 0;
                            c47s2_18.BorderColorBottom = BaseColor.BLACK;
                            c47s2_18.BorderWidthBottom = 2f;
                            tball.AddCell(c47s2_18);

                            dc.Add(tball);
                            dc.AddCreationDate();
                            dc.Close();
                            Process.Start(filename);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void generarnuevo()
        {
            limpiar();
        }

        public void validaciones(string credencial, string fecha, string hora, string lugarI, string tipoV, string kilometraje)
        {
            totalvalidaciontxt = 0;
            string horaa = hora + ":00";
            if (string.IsNullOrWhiteSpace(txtKilometraje.Text) || txtKilometraje.Text.Equals("."))
                txtKilometraje.Text = "0";
            if (string.IsNullOrWhiteSpace(credencial))
            {
                MessageBox.Show("Ingrese su número de credencial", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCredencial.Focus(); totalvalidaciontxt++;
            }
            else if (!string.IsNullOrWhiteSpace(credencial) && string.IsNullOrWhiteSpace(lblNombres.Text))
            {
                MessageBox.Show("El número de credencial ingresado esta erróneo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCredencial.Focus(); totalvalidaciontxt++;
            }
            else if (Convert.ToDateTime(fecha).Date > DateTime.Now.Date)
            {
                MessageBox.Show("La fecha no debe de ser mayor a la fecha actual", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFecha.Focus(); totalvalidaciontxt++;
            }
            else if (Convert.ToDateTime(horaa) > Convert.ToDateTime(DateTime.Now.ToString("HH:mm")) && Convert.ToDateTime(dtpFecha.Value).Date == DateTime.Now.Date)
            {
                MessageBox.Show("La hora no debe de ser mayor a la hora actual", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus(); totalvalidaciontxt++;
            }
            else if (string.IsNullOrWhiteSpace(lugarI))
            {
                MessageBox.Show("Ingrese el lugar del incidente", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLugarIncidente.Focus(); totalvalidaciontxt++;
            }
            else if (string.IsNullOrWhiteSpace(tipoV))
            {
                MessageBox.Show("Ingrese el tipo de vehiculo u objeto involucrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTipoVehiculo.Focus(); totalvalidaciontxt++;
            }
            else if (txtKilometraje.Enabled)
            {
                if (string.IsNullOrWhiteSpace(kilometraje))
                {
                    MessageBox.Show("Ingrese el kilometraje", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKilometraje.Focus(); totalvalidaciontxt++;
                }
                else if (Convert.ToDouble(kilometraje) <= 0)
                {
                    MessageBox.Show("El kilometraje debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKilometraje.Focus(); totalvalidaciontxt++;
                }
            }
            if (totalvalidaciontxt == 1)
                validaciontxt = true;
            else
                validaciontxt = false;
        }

        public void guardar()
        {
            validaciones(txtCredencial.Text, dtpFecha.Text, dtpTime.Text, txtLugarIncidente.Text, txtTipoVehiculo.Text, txtKilometraje.Text);
            if (!validaciontxt)
                if (Convert.ToDateTime(dtpFecha.Value).Date <= DateTime.Now.AddDays(-3) && dtpFecha.Enabled)
                {
                    MessageBox.Show("La fecha no debe de ser menor a una fecha anterior de 2 días de la actual", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpFecha.Focus(); validaciontxt = true;
                }
            if (!validaciontxt)
            {
                if (txtCredencial.Enabled)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO reportepersonal(ConsecutivoRP, credencialfkcpersonal, Fecha, Hora, LugarIncidente, TipoVehObj, Kilometraje, responsablefkcpersonal, coordinadorfkcpersonal, Observaciones, FechaHoraRegistro, usuariofkcpersonal) VALUES('" + consecutivoReporteP + "', '" + idcred + "', '" + dtpFecha.Value.ToString("yyyy-MM-dd") + "', '" + dtpTime.Text + "', '" + txtLugarIncidente.Text.Trim() + "', '" + txtTipoVehiculo.Text.Trim() + "', '" + Convert.ToDouble(txtKilometraje.Text) + "', '" + hresponsable + "', '" + hcoordinador + "', '" + txtObservaciones.Text.Trim() + "', now(), '" + idUsuario + "')", v.c.dbconection());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Información guardada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                    consultageneral();
                    limpiarvariablesanteriores();
                }
                else
                {
                    string consulta = "UPDATE reportepersonal SET ";
                    string set = "";
                    if (txtObservaciones.Enabled && !string.IsNullOrWhiteSpace(txtObservaciones.Text))
                        if (string.IsNullOrWhiteSpace(set))
                            set = "Observaciones = '" + txtObservaciones.Text.Trim() + "'";
                    if (btnHDResponsable.Enabled)
                        if (string.IsNullOrWhiteSpace(set))
                            set = "responsablefkcpersonal = '" + hresponsable + "'";
                        else
                            set += ", responsablefkcpersonal = '" + hresponsable + "'";
                    if (btnHDCoordinador.Enabled)
                        if (string.IsNullOrWhiteSpace(set))
                            set = "coordinadorfkcpersonal = '" + hcoordinador + "'";
                        else
                            set += ", coordinadorfkcpersonal = '" + hcoordinador + "'";
                    if (!string.IsNullOrWhiteSpace(set))
                        set += " WHERE idreportepersonal = '" + idreporteP + "'";
                    if ((txtObservaciones.Enabled && txtObservaciones.Text != "") || (btnHDResponsable.Enabled && !string.IsNullOrWhiteSpace(lblNombresResponsable.Text)) || (btnHDCoordinador.Enabled && !string.IsNullOrWhiteSpace(lblNombresCoordinador.Text)))
                    {
                        MySqlCommand cmmd = new MySqlCommand(consulta + set, v.c.dbconection());
                        cmmd.ExecuteNonQuery();
                        MessageBox.Show("Información guardada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        v.c.dbconection().Close();
                    }
                    else
                        MessageBox.Show("Sin modificaciones", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                    v.c.dbconection().Close();
                    consultageneral();
                    limpiarvariablesanteriores();
                }
            }
        }

        public void editar()
        {
            if (txtCredencial.Text == credencialanterior && Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd")) == Convert.ToDateTime(fechaanterior) && txtLugarIncidente.Text == lugarincidenteanterior && txtTipoVehiculo.Text == tipovehiculobjetoanterior && txtObservaciones.Text == (observacionesanterior) && dtpTime.Text + ":00" == horanterior && idresp == hresponsable.ToString() && idcoord == hcoordinador.ToString())
            {
                MessageBox.Show("Sin cambios", "INFOMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                consultageneral();
                btnGuardar.BackgroundImage = Properties.Resources.guardar__6_;
                label24.Text = "GUARDAR";
                btnNuevo.BackgroundImage = Properties.Resources.eraser1;
                label15.Text = "LIMPIAR CAMPOS";
                limpiar();
                limpiarvariablesanteriores();
            }
            else
            {
                validaciones(txtCredencial.Text, dtpFecha.Text, dtpTime.Text, txtLugarIncidente.Text, txtTipoVehiculo.Text, txtKilometraje.Text);
                if (!validaciontxt)
                {
                    if (Convert.ToDateTime(dtpFecha.Value).Date != Convert.ToDateTime(fechaanterior) && Convert.ToDateTime(dtpFecha.Value).Date <= DateTime.Now.AddDays(-3) && dtpFecha.Enabled)
                    {
                        MessageBox.Show("La fecha no debe de ser menor a una fecha anterior de 2 días de la actual", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dtpFecha.Focus(); validaciontxt = true;
                    }
                    else
                    {
                        string consulta = "UPDATE reportepersonal SET ";
                        string set = "";
                        if (credencialanterior != txtCredencial.Text)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "credencialfkcpersonal = '" + idcred + "'";
                            else
                                set += ", credencialfkcpersonal = '" + idcred + "'";
                        if (dtpFecha.Value.ToString("yyyy-MM-dd") != fechaanterior)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "Fecha = '" + dtpFecha.Value.ToString("yyyy-MM-dd") + "'";
                            else
                                set += ", FECHA = '" + dtpFecha.Value.ToString("yyyy-MM-dd") + "'";
                        if (dtpTime.Text != horanterior)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "Hora = '" + dtpTime.Text + "'";
                            else
                                set += ", Hora = '" + dtpTime.Text + "'";
                        if (txtLugarIncidente.Text != lugarincidenteanterior)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "LugarIncidente = '" + txtLugarIncidente.Text + "'";
                            else
                                set += ", LugarIncidente = '" + txtLugarIncidente.Text + "'";
                        if (txtTipoVehiculo.Text != tipovehiculobjetoanterior)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "TipoVehObj = '" + txtTipoVehiculo.Text + "'";
                            else
                                set += ", TipoVehObj = '" + txtTipoVehiculo.Text + "'";
                        if (txtKilometraje.Text != kilometrajeanterior)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "Kilometraje = '" + Convert.ToDouble(txtKilometraje.Text) + "'";
                            else
                                set += ", Kilometraje = '" + Convert.ToDouble(txtKilometraje.Text) + "'";
                        if (btnHDResponsable.Enabled)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "responsablefkcpersonal = '" + hresponsable + "'";
                            else
                                set += ", responsablefkcpersonal = '" + hresponsable + "'";
                        if (btnHDCoordinador.Enabled)
                            if (string.IsNullOrWhiteSpace(set))
                                set = "coordinadorfkcpersonal = '" + hcoordinador + "'";
                            else
                                set += " , coordinadorfkcpersonal = '" + hcoordinador + "'";
                        if (set != "")
                            set += ", Observaciones = '" + txtObservaciones.Text + "' WHERE idreportepersonal = '" + idreporteP + "'";
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                            MySqlCommand actualizar = new MySqlCommand(consulta + set, v.c.dbconection());
                            actualizar.ExecuteNonQuery();
                            v.c.dbconection().Close();
                            MessageBox.Show("Información actualizada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            string nombre = v.getaData("SELECT UPPER(CONCAT(ApPaterno, ' ', ApMaterno, ' ', nombres)) FROM cpersonal WHERE credencial = '" + credencialanterior + "'").ToString();
                            MySqlCommand historial = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('Reporte de Personal', (SELECT b1.idreportepersonal FROM reportepersonal AS b1 WHERE b1.ConsecutivoRP = '" + consecutivoReporteP + "'), '" + nombre + "; " + credencialanterior + "; " + fechaanterior + "; " + horanterior + "; " + lugarincidenteanterior + "; " + tipovehiculobjetoanterior + "; " + kilometrajeanterior + "; " + observacionesanterior + "; " + idresp + "; " + idcoord + "', '" + idUsuario + "', now(), 'Actualización de Reporte de Personal', '" + observaciones + "', '" + empresa + "', '" + area + "')", v.c.dbconection());
                            historial.ExecuteNonQuery();
                            v.c.dbconection().Close();
                            limpiar();
                            consultageneral();
                            btnGuardar.BackgroundImage = Properties.Resources.guardar__6_;
                            label24.Text = "GUARDAR";
                            btnGuardar.Location = new Point(531, 553);
                            btnNuevo.BackgroundImage = Properties.Resources.eraser1;
                            label15.Text = "LIMPIAR CAMPOS";
                            limpiarvariablesanteriores();
                            banderaeditar = false;
                        }
                    }
                }
            }
        }

        /* EVENTOS */

        private void chbxFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (chbxFechas.Checked)
            {
                dtpInicio.Enabled = dtpFinal.Enabled = true;
                cbxMesB.Enabled = false;
                cbxMesB.SelectedIndex = 0;
            }
            else
            {
                dtpInicio.Enabled = dtpFinal.Enabled = false;
                dtpInicio.Value = dtpFinal.Value = DateTime.Now;
                cbxMesB.Enabled = true;
            }
        }

        private void chbxVehiculo_CheckedChanged(object sender, EventArgs e)
        {
            if (chbxVehiculo.Checked)
                txtKilometraje.Enabled = true;
            else
            {
                txtKilometraje.Enabled = false;
                txtKilometraje.Text = "0";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (label24.Text.Equals("GUARDAR"))
                guardar();
            else if (label24.Text.Equals("EDITAR"))
                editar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCredencialBusq.Text) && string.IsNullOrWhiteSpace(txtVehiculoBusq.Text) && cbxMesB.SelectedIndex == 0 && chbxFechas.Checked == false)
                MessageBox.Show("Campos de búsqueda vacíos", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (dtpInicio.Value.Date > dtpFinal.Value.Date)
                {
                    MessageBox.Show("La fecha inicial no debe superar a la fecha final", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    limpiarbusq();
                }
                else if (dtpInicio.Value.Date > DateTime.Now.Date || dtpFinal.Value.Date > DateTime.Now.Date)
                {
                    MessageBox.Show("Las fechas no deben superar el día de hoy", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    limpiarbusq();
                    chbxFechas.Checked = false;
                }
                else
                {
                    string consulta = "SET lc_time_names = 'es_ES'; SELECT t1.idreportepersonal AS 'ID REPORTE', CONCAT(UPPER(DATE_FORMAT(t1.Fecha, '%W %d de %M de %Y')), ' / ', DATE_FORMAT(t1.Hora, '%H:%i')) AS 'FECHA/HORA', UPPER(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres)) AS PERSONAL, IF(t1.estatus = 1, 'FINALIZADO', '') AS ESTATUS, t1.LugarIncidente AS 'LUGAR DEL INCIDENTE', t1.Observaciones AS 'OBSERVACIONES' FROM reportepersonal AS t1 INNER JOIN cpersonal AS t2 ON t1.credencialfkcpersonal = t2.idPersona";
                    string where = "";
                    if (!string.IsNullOrWhiteSpace(txtCredencialBusq.Text))
                    {
                        if (string.IsNullOrWhiteSpace(where))
                            where = " WHERE t2.credencial = '" + txtCredencialBusq.Text + "'";
                        else
                            where += " AND t2.credencial = '" + txtCredencialBusq.Text + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(txtVehiculoBusq.Text))
                    {
                        if (string.IsNullOrWhiteSpace(where))
                            where = " WHERE t1.TipoVehObj = '" + txtVehiculoBusq.Text + "'";
                        else
                            where += " AND t1.TipoVehObj = '" + txtVehiculoBusq.Text + "'";
                    }
                    if (cbxMesB.SelectedIndex != 0)
                    {
                        if (string.IsNullOrWhiteSpace(where))
                            where = " WHERE (SELECT t1.Fecha WHERE MONTH(t1.Fecha) = '" + cbxMesB.SelectedIndex + "' AND YEAR(t1.Fecha) = YEAR(Now()))";
                        else
                            where += " AND (SELECT t1.Fecha WHERE MONTH(t1.Fecha) = '" + cbxMesB.SelectedIndex + "' AND YEAR(t1.Fecha) = YEAR(now()))";
                    }
                    if (chbxFechas.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(where))
                            where = " WHERE (SELECT t1.Fecha BETWEEN '" + dtpInicio.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpFinal.Value.ToString("yyyy-MM-dd") + "')";
                        else
                            where += " AND (SELECT t1.Fecha BETWEEN '" + dtpInicio.Value.ToString("yyyy-MM-dd") + "' AND '" + dtpFinal.Value.ToString("yyyy-MM-dd") + "')";
                    }
                    if (!string.IsNullOrWhiteSpace(where))
                        where += " ORDER BY t1.idreportepersonal DESC";
                    MySqlDataAdapter adpbusq = new MySqlDataAdapter(consulta + where, v.c.dbconection());
                    DataSet ds = new DataSet();
                    adpbusq.Fill(ds);
                    dgvPMantenimiento.DataSource = ds.Tables[0];
                    dgvPMantenimiento.Columns[0].Visible = false;
                    v.c.dbconection().Close();
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron reportes", "ADVERTEMCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        consultageneral();
                    }
                    else
                    {
                        label37.Visible = btnActualizar.Visible = true;
                        if(peditar)
                            btnExcel.Visible = lblExcel.Visible = true;
                    }
                    limpiarbusqnormal();
                }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (label15.Text.Equals("NUEVO REPORTE"))
            {
                if (txtCredencial.Text == credencialanterior && Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd")) == Convert.ToDateTime(fechaanterior) && txtLugarIncidente.Text == lugarincidenteanterior && txtTipoVehiculo.Text == tipovehiculobjetoanterior && txtObservaciones.Text == (observacionesanterior) && dtpTime.Text + ":00" == horanterior && Convert.ToDouble(txtKilometraje.Text) == Convert.ToDouble(kilometrajeanterior))
                    btnuevo();
                else if (MessageBox.Show("Si selecciona la opción de \"ACEPTAR\" borrará los datos ingresados, ¿Desea continuar?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    btnuevo();
                banderaeditar = false;
            }
            else if (label15.Text.Equals("LIMPIAR CAMPOS"))
            {
                if (txtCredencial.Text == credencialanterior && DateTime.Parse(dtpFecha.Value.ToString("yyyy-MM-dd")) == Convert.ToDateTime(DateTime.Parse(fechaanterior).ToString("yyyy-MM-dd")) && txtLugarIncidente.Text == lugarincidenteanterior && txtTipoVehiculo.Text == tipovehiculobjetoanterior && txtObservaciones.Text == (observacionesanterior) && dtpTime.Text == horanterior && Convert.ToDouble(txtKilometraje.Text) == Convert.ToDouble(kilometrajeanterior))
                {
                    limpiar();
                    consultageneral();
                    limpiarvariablesanteriores();
                }
                else if (MessageBox.Show("Si selecciona la opción de \"ACEPTAR\" borrará los datos ingresados, ¿Desea continuar?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    limpiar();
                    consultageneral();
                    limpiarvariablesanteriores();
                }
            }
        }

        public void limpiarvariablesanteriores()
        {
            credencialanterior = lugarincidenteanterior = tipovehiculobjetoanterior = observacionesanterior = "";
            kilometrajeanterior = idresp = idcoord = "0";
            hcoordinador = hresponsable = 0;
            fechaanterior = DateTime.Now.ToString();
            horanterior = "00:00";
        }

        private void txtCredencial_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT idPersona, UPPER(ApPaterno) AS ApPaterno, UPPER(ApMaterno) AS ApMaterno, UPPER(nombres) AS nombres FROM cpersonal WHERE credencial = '" + txtCredencial.Text + "' AND empresa = '" + empresa + "' AND area = '" + area + "' AND status = '1'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                idcred = dr.GetInt32("idPersona");
                lblAPaterno.Text = dr.GetString("ApPaterno");
                lblAMaterno.Text = dr.GetString("ApMaterno");
                lblNombres.Text = dr.GetString("nombres");
            }
            else
            {
                lblAPaterno.Text = lblAMaterno.Text = lblNombres.Text = "";
                idcred = 0;
            }
            dr.Close();
            v.c.dbconection().Close();
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            to_pdf();
        }

        /* DISEÑO */

        public void btnall_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(59, 59);
        }

        public void btnall_MouseLeave(object sender, EventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(54, 54);
        }

        public void btnallb_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(45, 45);
        }

        public void btnallb_MouseLeave(object sender, EventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(40, 40);
        }

        public void gbxall_Painting(object sender, PaintEventArgs e)
        {
            GroupBox gbxall = sender as GroupBox;
            v.DrawGroupBox(gbxall, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        public void dtpall_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dgvAll_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dgvPMantenimiento_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (txtCredencial.Text == credencialanterior && Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd")) == Convert.ToDateTime(fechaanterior) && txtLugarIncidente.Text == lugarincidenteanterior && txtTipoVehiculo.Text == tipovehiculobjetoanterior && txtObservaciones.Text == (observacionesanterior) && dtpTime.Text + ":00" == horanterior && Convert.ToDouble(txtKilometraje.Text) == Convert.ToDouble(kilometrajeanterior) && idresp == hresponsable.ToString() && idcoord == hcoordinador.ToString())
                    dbclick();
                else if (txtCredencial.Text == credencialanterior && DateTime.Parse(dtpFecha.Value.ToString("yyyy-MM-dd")) == Convert.ToDateTime(DateTime.Parse(fechaanterior).ToString("yyyy-MM-dd")) && txtLugarIncidente.Text == lugarincidenteanterior && txtTipoVehiculo.Text == tipovehiculobjetoanterior && txtObservaciones.Text == (observacionesanterior) && dtpTime.Text == horanterior && Convert.ToDouble(txtKilometraje.Text) == Convert.ToDouble(kilometrajeanterior))
                    dbclick();
                else if (MessageBox.Show("Si selecciona la opción de \"ACEPTAR\" borrará los datos ingresados, ¿Desea continuar?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    dbclick();
            }
        }

        private void btnActualizarEnca_Click(object sender, EventArgs e)
        {
            ReportesVigencias rp = new ReportesVigencias(empresa, area, idUsuario,v);
            rp.Owner = this;
            rp.ShowDialog();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            limpiarbusq();
            consultageneral();
        }

        private void btnall_Click(object sender, EventArgs e)
        {
            if (btnHDResponsable.Width == 59 && btnHDResponsable.Height == 59 || (btnHDResponsable.Focus() == true && btnHDCoordinador.Width == 54 && btnHDCoordinador.Height == 54))
            {
                numhuella = 1;
                tipo = "RESPONSABLE";
            }
            else
            {
                numhuella = 2;
                tipo = "COORDINADOR";
            }
            LectorHuellas lh = new LectorHuellas(2, numhuella, tipo, "Nombre", "Paterno", "Materno", idcred.ToString(),v);
            lh.Owner = this;
            lh.ShowDialog();
            if (numhuella == 1)
            {
                hresponsable = Convert.ToInt16(lh.tipoidreporte);
                lblNombresResponsable.Text = lblPaternoResponsable.Text = lblMaternoResponsable.Text = "";
                if (hresponsable == hcoordinador && hresponsable != 0 && hcoordinador != 0)
                {
                    lblNombresResponsable.Text = lblPaternoResponsable.Text = lblMaternoResponsable.Text = "";
                    hresponsable = 0;
                    MessageBox.Show("Esta firma/huella ya existe en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnHDResponsable.Enabled = true;
                }
                else if (hresponsable == 0)
                    btnHDResponsable.Enabled = true;
                else
                {
                    lblNombresResponsable.Text = lh.tiporeporte1;
                    lblPaternoResponsable.Text = lh.tiporeporte2;
                    lblMaternoResponsable.Text = lh.tiporeporte3;
                }
                if (hresponsable != 0)
                    ptbxResponsable.Image = Properties.Resources.correct;
                else
                    ptbxResponsable.Image = Properties.Resources.incorrect;
            }
            else
            {
                hcoordinador = Convert.ToInt16(lh.tipoidreporte);
                lblNombresCoordinador.Text = lblPaternoCoordinador.Text = lblMaternoCoordinador.Text = "";
                if ((hresponsable == hcoordinador) && hresponsable != 0 && hcoordinador != 0) // poner el anterior
                {
                    hcoordinador = 0;
                    MessageBox.Show("Esta firma/huella ya existe en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnHDCoordinador.Enabled = true;
                }
                else if (hcoordinador == 0)
                    btnHDCoordinador.Enabled = true;
                else
                {
                    lblNombresCoordinador.Text = lh.tiporeporte1;
                    lblPaternoCoordinador.Text = lh.tiporeporte2;
                    lblMaternoCoordinador.Text = lh.tiporeporte3;
                }
                if (hcoordinador != 0)
                    ptbxCoordinador.Image = Properties.Resources.correct;
                else
                    ptbxCoordinador.Image = Properties.Resources.incorrect;
            }
        }

        private void dgvPMantenimiento_MouseClick(object sender, MouseEventArgs e)
        {
            banderaeditar = false;
            if (peditar)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView dgv = sender as DataGridView;
                    int xy = dgv.HitTest(e.X, e.Y).RowIndex;
                    if (xy >= 0)
                    {
                        ContextMenuStrip mn = new System.Windows.Forms.ContextMenuStrip();
                        mn.Items.Add("Editar".ToUpper(), controlFallos.Properties.Resources.pencil).Name = "Editar".ToUpper();
                        mn.Show(dgvPMantenimiento, new Point(e.X, e.Y));
                        mn.ItemClicked += new ToolStripItemClickedEventHandler(mn_ItemClicked);
                    }
                }
            }
        }

        private void dgvselect_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dgvselect = sender as DataGridView;
                if (e.Button == MouseButtons.Right)
                {
                    dgvselect.CurrentCell = dgvselect.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvselect.Rows[e.RowIndex].Selected = true;
                    dgvselect.Focus();
                }
            }
        }

        public void mn_ItemClicked(object sender, ToolStripItemClickedEventArgs e) // Edicion en el gridview de mantenimiento
        {
            idreporteP = Convert.ToInt32(dgvPMantenimiento.CurrentRow.Cells["ID REPORTE"].Value);
            estatus = Convert.ToString(dgvPMantenimiento.CurrentRow.Cells["ESTATUS"].Value);
            if (string.IsNullOrWhiteSpace(estatus))
            {
                switch (e.ClickedItem.Name.ToString())
                {
                    case "EDITAR":
                        banderaeditar = true;
                        btnGuardar.Visible = label24.Visible = txtObservaciones.Enabled = chbxVehiculo.Enabled = true;
                        btnFinalizar.Visible = label26.Visible = btnPDF.Visible = label31.Visible = false;
                        btnNuevo.BackgroundImage = Properties.Resources.test;
                        label15.Text = "NUEVO REPORTE";
                        btnGuardar.BackgroundImage = Properties.Resources.document_edit_icon_icons_com_52428;
                        label24.Text = "EDITAR";
                        btnGuardar.Location = new Point(523, 553);
                        allpeople(estatus);
                        if (!string.IsNullOrWhiteSpace(txtCredencial.Text))
                            txtCredencial.Enabled = true;
                        else
                            txtCredencial.Enabled = false;
                        if ((Convert.ToDateTime(dtpFecha.Value.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))) && (Convert.ToDateTime(dtpTime.Value.ToString("H:m:s")) < Convert.ToDateTime(DateTime.Now.ToString("H:m:s"))))
                            dtpFecha.Enabled = dtpTime.Enabled = true;
                        else
                            dtpTime.Enabled = dtpFecha.Enabled = false;
                        if (!string.IsNullOrWhiteSpace(txtLugarIncidente.Text))
                            txtLugarIncidente.Enabled = true;
                        else
                            txtLugarIncidente.Enabled = false;
                        if (!string.IsNullOrWhiteSpace(txtTipoVehiculo.Text))
                            txtTipoVehiculo.Enabled = true;
                        else
                            txtTipoVehiculo.Enabled = false;
                        if (hresponsable != 0)
                            btnHDResponsable.Enabled = true;
                        else
                            btnHDResponsable.Enabled = false;
                        if (hcoordinador != 0)
                            btnHDCoordinador.Enabled = true;
                        else
                            btnHDCoordinador.Enabled = false;
                        break;
                }
            }
            else
                MessageBox.Show("Este reporte ya fue finalizado, por lo cual, ya no se puede editar", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            validaciones(txtCredencial.Text, dtpFecha.Text, dtpTime.Text, txtLugarIncidente.Text, txtTipoVehiculo.Text, txtKilometraje.Text);
            if (!validaciontxt)
            {
                if (Convert.ToDateTime(dtpFecha.Value).Date <= DateTime.Now.AddDays(-3) && dtpFecha.Enabled)
                {
                    MessageBox.Show("La fecha no debe de ser menor a una fecha anterior de 2 días de la actual", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpFecha.Focus(); validaciontxt = true;
                }
                else if (string.IsNullOrWhiteSpace(lblPaternoResponsable.Text))
                {
                    MessageBox.Show("Ingrese la huella digital del responsable", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnHDResponsable.Focus(); validaciontxt = true;
                }
                else if (string.IsNullOrWhiteSpace(lblPaternoCoordinador.Text))
                {
                    MessageBox.Show("Ingrese la huella digital del coordinador", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnHDCoordinador.Focus(); validaciontxt = true;
                }
                else
                {
                    FormContraFinal FCF = new FormContraFinal(empresa, area, this,v);
                    FCF.LabelTitulo.Text = "Introduzca su contraseña para finalizar\nel reporte";
                    var res = FCF.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        idFinal = Convert.ToInt32(FCF.id);
                        if (txtCredencial.Enabled && !validaciontxt)
                        {
                            MySqlCommand cmd = new MySqlCommand("INSERT INTO reportepersonal(ConsecutivoRP, credencialfkcpersonal, Estatus, Fecha, Hora, LugarIncidente, TipoVehObj, Kilometraje, responsablefkcpersonal, coordinadorfkcpersonal, Observaciones, FechaHoraRegistro, usuariofkcpersonal, usuariofinalizofkcpersonal) VALUES('" + consecutivoReporteP + "', '" + idcred + "', '1', '" + dtpFecha.Value.ToString("yyyy-MM-dd") + "', '" + dtpTime.Text + "', '" + txtLugarIncidente.Text.Trim() + "', '" + txtTipoVehiculo.Text.Trim() + "', '" + Convert.ToDouble(txtKilometraje.Text) + "', '" + hresponsable + "', '" + hcoordinador + "', '" + txtObservaciones.Text.Trim() + "', now(), '" + idUsuario + "', '" + idFinal + "')", v.c.dbconection());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Información almacenada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            consultageneral();
                            limpiarvariablesanteriores();
                        }
                        else if (!validaciontxt)
                        {
                            string update = "UPDATE reportepersonal SET Estatus = '" + 1 + "', usuariofinalizofkcpersonal = '" + idFinal + "'";
                            string set = "";
                            if (txtKilometraje.Enabled)
                                if (Convert.ToDouble(txtKilometraje.Text) > 0)
                                    if (string.IsNullOrWhiteSpace(set))
                                        set = ", Kilometraje = '" + Convert.ToDouble(txtKilometraje.Text) + "'";
                                    else
                                        set += ", Kilometraje = '" + Convert.ToDouble(txtKilometraje.Text) + "'";
                            if (!string.IsNullOrWhiteSpace(hresponsable.ToString()))
                                if(Convert.ToDouble(hresponsable) > 0)
                                    set = ", responsablefkcpersonal = '" + hresponsable + "'";
                                else
                                    set += ", responsablefkcpersonal = '" + hresponsable + "'";
                            if (Convert.ToDouble(hcoordinador) > 0)
                                if (string.IsNullOrWhiteSpace(set))
                                    set = ", coordinadorfkcpersonal = '" + hcoordinador + "'";
                                else
                                    set += ", coordinadorfkcpersonal = '" + hcoordinador + "'";
                            if (!string.IsNullOrWhiteSpace(txtObservaciones.Text))
                                if (string.IsNullOrWhiteSpace(set))
                                    set = ", Observaciones = '" + txtObservaciones.Text.Trim() + "'";
                                else
                                    set += ", Observaciones = '" + txtObservaciones.Text.Trim() + "'";
                            if (!string.IsNullOrWhiteSpace(set))
                                set += " WHERE idreportepersonal = '" + idreporteP + "'";
                            MySqlCommand finalizar = new MySqlCommand(update + set, v.c.dbconection());
                            finalizar.ExecuteNonQuery();
                            v.c.dbconection().Close();
                            MessageBox.Show("El reporte ha sido finalizado", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnFinalizar.Visible = label26.Visible = btnPDF.Visible = label31.Visible = false;
                            btnGuardar.Visible = label24.Visible = txtCredencial.Enabled = dtpFecha.Enabled = dtpTime.Enabled = txtLugarIncidente.Enabled = txtTipoVehiculo.Enabled =  true;
                            label24.Text = "GUARDAR";
                            btnGuardar.BackgroundImage = Properties.Resources.guardar__6_;
                            limpiar();
                            consultageneral();
                            limpiarvariablesanteriores();
                        }
                    }
                }
            }
        }

        private void dgvPMantenimiento_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Convert.ToString(e.Value) == "FINALIZADO")
                e.CellStyle.BackColor = Color.PaleGreen;
        }

        bool activado = false;
        private void btnExcel_Click(object sender, EventArgs e)
        {
            activado = true;
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
        }

        public void txtall_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            while (txt.Text.Contains("  ") || txt.Text.Contains(Environment.NewLine) || txt.Text.Contains("\n"))
                txt.Text = txt.Text.Replace("  ", " ").Trim().Replace("\r\n", " ").Replace("\n", " ");
            txt.Text = txt.Text.Trim();
        }
    }
}