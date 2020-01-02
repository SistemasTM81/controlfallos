using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using h = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace controlFallos
{
    public partial class FormFallasMantenimiento : Form
    {

        validaciones v;
        int idUsuario, empresa, area;
        static bool res = true;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }

        public Thread hilo;


        void quitarseen()
        {
            while (res)
            {
                MySqlConnection dbcon = null;
                if (v.c.conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                else
                    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE reportesupervicion SET seen = 1 WHERE seen  = 0", dbcon);
                cmd.ExecuteNonQuery();
                dbcon.Close();
                Thread.Sleep(180000);
            }
        }

        public FormFallasMantenimiento(int idUsuario, int empresa, int area, System.Drawing.Image newimg, validaciones v)
        {
            InitializeComponent();
            this.v = v;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
        }

        private void FormFallasMantenimiento_Load(object sender, EventArgs e)
        {
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();

        }


<<<<<<< HEAD
=======
        public void GenerarDocumento(Document document, PdfWriter writer) //Genera El Documento
        {
            int i, j;
            metodoverificarrefaccionespdf();
            PdfPTable datatable = new PdfPTable(dgvPMantenimiento.ColumnCount);
            datatable.DefaultCell.Padding = 3;
            float[] headerwidths = GetTamañoColumnas(dgvPMantenimiento);
            datatable.SetWidths(headerwidths);
            datatable.WidthPercentage = 89;
            datatable.DefaultCell.BorderWidth = 2;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < dgvPMantenimiento.ColumnCount; i++)
                datatable.AddCell(new Phrase(dgvPMantenimiento.Columns[i].HeaderText.ToString(), FontFactory.GetFont("CALIBRI", 8, iTextSharp.text.Font.BOLD)));
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(250, 250, 250);
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < dgvPMantenimiento.Rows.Count; i++)
            {
                for (j = 0; j < dgvPMantenimiento.Columns.Count; j++)
                {
                    if (dgvPMantenimiento[j, i].Value != null)
                    {
                        datatable.AddCell(new Phrase(dgvPMantenimiento[j, i].Value.ToString(), FontFactory.GetFont("CALIBRI", 7)));
                        var content1 = writer.DirectContent;
                        var pageBorderRect1 = new iTextSharp.text.Rectangle(document.PageSize);
                        pageBorderRect1.Left += document.LeftMargin;
                        pageBorderRect1.Right -= document.RightMargin;
                        pageBorderRect1.Top -= document.TopMargin;
                        pageBorderRect1.Bottom += document.BottomMargin;
                        content1.SetColorStroke(BaseColor.BLACK);
                        content1.Rectangle(pageBorderRect1.Left, pageBorderRect1.Bottom, pageBorderRect1.Width, pageBorderRect1.Height);
                        content1.SetLineWidth(2f);
                        content1.Stroke();
                    }
                }
                datatable.CompleteRow();
            }
            document.Add(datatable);
        }

        public float[] GetTamañoColumnas(DataGridView dg) //Metodo Del Tamaño De La Tabla PDF
        {
            float[] values = new float[dg.ColumnCount];
            for (int i = 0; i < 8; i++)
                values[i] = (float)dg.Columns[i].Width;
            return values;
        }

        Thread hiloEx;
        delegate void Loading();
        public void cargando1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            buttonExcel.Visible = false;
            label35.Text = "EXPORTANDO";
            label35.Location = new Point(1264, 99);
        }

        delegate void Loading1();
        public void cargando2()
        {
            pictureBoxExcelLoad.Image = null;
            label35.Text = "EXPORTAR";
            label35.Location = new Point(1279, 99);
            buttonExcel.Visible = true;
            if (exportando)
                buttonExcel.Visible = label35.Visible = false;
            exportando = activado = false;
        }


        public void exporta_a_excel() //Metodo Que Genera El Excel
        {
            DataTable dtexcel = new DataTable();
            for (int i = 0; i < dgvMantenimiento.Columns.Count; i++) if (dgvMantenimiento.Columns[i].Visible) dtexcel.Columns.Add(dgvMantenimiento.Columns[i].HeaderText);
            for (int j = dgvMantenimiento.Rows.Count - 1; j >= 0; j--)
            {
                DataRow row = dtexcel.NewRow();
                int indice = 0;
                for (int i = 0; i < dgvMantenimiento.Columns.Count; i++)
                {
                    if (dgvMantenimiento.Columns[i].Visible)
                    {
                        row[dtexcel.Columns[indice]] = dgvMantenimiento.Rows[j].Cells[i].Value;
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
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.FontStyle = "Calibri";
                            rng.Font.Size = 11;
                            rng.Interior.Color = (dtexcel.Rows[i][j].ToString() == "EN PROCESO".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.Khaki) : dtexcel.Rows[i][j].ToString() == "LIBERADA".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.PaleGreen) : dtexcel.Rows[i][j].ToString() == "REPROGRAMADA".ToString() ? rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral) : dtexcel.Rows[i][j].ToString() == "CORRECTIVO".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.Khaki) : dtexcel.Rows[i][j].ToString() == "PREVENTIVO".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.PaleGreen) : dtexcel.Rows[i][j].ToString() == "REITERATIVO".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.LightCoral) : dtexcel.Rows[i][j].ToString() == "REPROGRAMADO".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.LightBlue) : dtexcel.Rows[i][j].ToString() == "SEGUIMIENTO".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.FromArgb(246, 106, 77)) : dtexcel.Rows[i][j].ToString() == "INCOMPLETO".ToString() ? System.Drawing.ColorTranslator.ToOle(Color.FromArgb(255, 144, 51)) : System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230)));
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

        /* Acciones con los botones y gridview *///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonAgregarMasPed_Click(object sender, EventArgs e) //Regresa Para Meter Mas Refacciones
        {
            banderaeditar = buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = false;
            buttonAgregaPed.Visible = label33.Visible = buttonActualizar.Visible = label26.Visible = true;
            metodocargaref();
            limpiarrefacc();
        }

        private void buttonAgregar_Click(object sender, EventArgs e) //Manda A La Ventana De Refacciones
        {
            groupBoxRefacciones.Visible = true;
            metodocargaref();
            if (!registroconteofilaspedref)
            {
                inicolumn = 0;
                inicolumn = dgvPMantenimiento.Rows.Count;
                validacionfinalconteocolumnas = registroconteofilaspedref = true;
            }
            gbxMantenimiento.Visible = buttonGuardar.Visible = label24.Visible = buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = false;
            buttonAgregaPed.Visible = label33.Visible = dgvPMantenimiento.Visible = label1.Visible = true;
            Cancelar(false);
            if (!registroentradapedref) //CHECAR ESTA VALIDACION
                registroentradapedref = label62.Visible = label63.Visible = true;
            metodocargaref();
            conteoiniref();
            conteofinref();
        }

        bool activado = false;
        private void buttonExcel_Click(object sender, EventArgs e) //Genera Un Documento De Excel
        {
            activado = true;
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
        }

        private void buttonFinalizar_Click(object sender, EventArgs e) //Finalizar Matenimiento
        {
            int cfin = 0;
            if (cfin == 0)
            {
                cfin = cfin + 1;
                cargo = 2;
                if (comboBoxEstatusMant.Text.Equals("LIBERADA"))
                {
                    if (estatusmantGV.Equals("REPROGRAMADA"))
                    {
                        MessageBox.Show("La unidad no puede ser liberada porque primero debe pasar por un proceso antes de terminar el reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        labelHoraTerminoM.Text = "";
                        textBoxTerminoMan.Text = "";
                        validar();
                    }
                    else if (comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES"))
                    {
                        if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- ESTATUS --")))
                        {
                            MessageBox.Show("El reporte no puede ser finalizado porque las refacciones solicitadas aun no son entregadas", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            validar();
                        }
                        else if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                        {
                            if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                            {
                                MessageBox.Show("El Folio de Factura no puede quedar vacio si está validada la Existencia de Refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                            }
                            else if (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text.Trim()))
                            {
                                MessageBox.Show("Introduzca un \"Trabajo Realizado\" Válido", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                                textBoxTrabajoRealizado.Focus();
                            }
                            else if (Convert.ToInt32(textBoxFolioFactura.Text) == 0)
                            {
                                MessageBox.Show("El Folio de Factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                validar();
                                textBoxFolioFactura.Focus();
                            }
                            else
                            {
                                if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM reportemantenimiento WHERE FolioFactura='" + textBoxFolioFactura.Text + "'")) > 0 && (textBoxFolioFactura.Enabled == true))
                                {
                                    MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBoxFolioFactura.Text = "";
                                    textBoxFolioFactura.Focus();
                                    validar();
                                }
                                else
                                {
                                    resultadopregunta = 0;
                                    metodobtnfinalizarcref();
                                    if (resultadopregunta == 1)
                                    {
                                        actualizarcbx();
                                        AutoCompletado(textBoxFolioB);
                                        comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonAgregar.Visible = label39.Visible = false;
                                        if (label35.Text == "EXPORTANDO")
                                            valexcel();
                                        else
                                            ocultarexcel();
                                        comboBoxEstatusMant.SelectedIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                    else if (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))
                    {
                        if (!string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text.Trim()))
                        {
                            resultadopregunta = 0;
                            metodobtnfinalizarcref();
                            if (resultadopregunta == 1)
                            {
                                comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonAgregar.Visible = label39.Visible = false;
                                if (label35.Text == "EXPORTANDO")
                                    valexcel();
                                else
                                    ocultarexcel();
                                cfin = comboBoxEstatusMant.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Introduzca un \"Trabajo Realizado\" Válido", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            validar();
                            textBoxTrabajoRealizado.Focus();
                        }
                    }
                }
                else if (comboBoxEstatusMant.SelectedIndex == 0)
                    MessageBox.Show("Seleccione un estatus del mantenimiento", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonGuardar_Click(object sender, EventArgs e) // Guardar
        {
            valdeval();
            if (comboBoxFalloGral.Text.Equals("-- GRUPO --"))
                MessageBox.Show("Seleccione un Grupo de Fallo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if ((textBoxMecanico.Text != "") && (labelNomMecanico.Text.Equals(".")))
            {
                MessageBox.Show("Contraseña del Mecánico Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMecanico.Text = "";
            }
            else if ((textBoxMecanicoApo.Text != "") && (labelNomMecanicoApo.Text.Equals("..")))
            {
                MessageBox.Show("Contraseña del Mecánico de Apoyo Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMecanicoApo.Text = "";
            }
            else if ((textBoxSuperviso.Text != "") && (labelNomSuperviso.Text.Equals("...")))
            {
                MessageBox.Show("Contraseña de la Persona que Supervisó el Mantenimiento Incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxSuperviso.Text = "";
            }
            else if (labelNomMecanico.Text == ".")
                MessageBox.Show("Ingrese la Contraseña del Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if ((labelNomMecanico.Text == ".") && (labelNomMecanicoApo.Text != ".."))
                MessageBox.Show("El Mecánico de Apoyo no puede ser registrado antes de el Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if ((labelNomMecanico.Text == ".") && (labelNomSuperviso.Text != "..."))
                MessageBox.Show("El Supevisor no puede ser registrado antes de el Mecánico", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (validacionfinalconteocolumnas)
                {
                    fincolumn = dgvPMantenimiento.Rows.Count;
                    validacionfinalconteocolumnas = false;
                }
                if ((fgeneralanterior.Equals(comboBoxFalloGral.Text)) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && ((comboBoxExisRefacc.SelectedIndex == 0) || (exisrefaccionanterior.Equals(comboBoxExisRefacc.Text))) && (reqrefanterior.Equals(comboBoxReqRefacc.Text)) && (trabrealizadoanterior.Equals(textBoxTrabajoRealizado.Text)) && (folfacturanterior.Equals(textBoxFolioFactura.Text)) && (estatusmantanterior.Equals(comboBoxEstatusMant.Text)) && (supervisoanterior.Equals(labelNomSuperviso.Text)) && (observacionesmantanterior.Equals(textBoxObsMan.Text)) && (inicolumn == fincolumn))
                {
                    conteo();
                    metodoCarga();
                    limpiarcampos();
                    limpiarstring();
                    ncontreffin();
                    inicolumn = 0;
                    fincolumn = 0;
                    dgvMantenimiento.Refresh();
                    comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = buttonAgregar.Visible = label39.Visible = false;
                    if (label35.Text == "EXPORTANDO")
                        valexcel();
                    else
                        ocultarexcel();
                    timer1.Start();
                    MessageBox.Show("No se realizó ningun cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
                else if (comboBoxEstatusMant.SelectedIndex == 0)
                    MessageBox.Show("Seleccione un estatus del mantenimiento", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if ((comboBoxEstatusMant.Text == "EN PROCESO") || (comboBoxEstatusMant.Text == "REPROGRAMADA"))
                {
                    if ((comboBoxEstatusMant.Text.Equals("REPROGRAMADA")) && (estatusmantGV != "EN PROCESO"))
                        labelHoraInicioM.Text = textBoxEsperaMan.Text = "";
                    if (comboBoxReqRefacc.SelectedIndex == 1 && !metodotxtchref())
                    {
                        if (comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES"))
                        {
                            if ((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))))
                                metodobtnguardar();
                            else if ((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && ((comboBoxExisRefacc.SelectedIndex == 0)))
                                MessageBox.Show("El campo de 'Existencia De Refacciones' no debe quedar en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            else if ((textBoxFolioFactura.Text != "") && ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))) && (textBoxFolioFactura.Enabled))
                            {
                                MessageBox.Show("El Folio de Factura debe quedar en blanco si el apartado 'Existencia De Refacciones' está en espera de las refacciones o no hay existencias", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                textBoxFolioFactura.Text = "";
                            }
                            else if ((comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) && (!textBoxFolioFactura.Enabled)))
                            {
                                if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                                    MessageBox.Show("El Folio de Factura no puede quedar vacío si hay existencias en las refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else if (Convert.ToInt32(textBoxFolioFactura.Text) == 0)
                                {
                                    MessageBox.Show("El Folio de Factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBoxFolioFactura.Text = "";
                                    textBoxFolioFactura.Focus();
                                }
                                else
                                {
                                    MySqlCommand cmd01 = new MySqlCommand("SELECT coalesce((FolioFactura), '') AS FolioFactura FROM reportemantenimiento WHERE FolioFactura = '" + textBoxFolioFactura.Text + "'", v.c.dbconection());
                                    MySqlDataReader dr01 = cmd01.ExecuteReader();
                                    if (dr01.Read())
                                        foliofacturarconsulta = Convert.ToString(dr01.GetString("FolioFactura"));
                                    dr01.Close();
                                    v.c.dbcon.Close();
                                    if ((foliofacturarconsulta == textBoxFolioFactura.Text) && (textBoxFolioFactura.Enabled == true))
                                    {
                                        MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        textBoxFolioFactura.Text = "";
                                    }
                                    else if ((foliofacturarconsulta == textBoxFolioFactura.Text) && (textBoxFolioFactura.Enabled == false))
                                        metodobtnguardar();
                                    else
                                        metodobtnguardar();
                                }
                            }
                        }
                    }
                    else if (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))
                        metodobtnguardar();
                    else
                    {
                        MessageBox.Show("Seleccione una opcion en " + "'SE REQUIEREN REFACCIONES'", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        comboBoxReqRefacc.Enabled = true;
                    }
                }
                else if (!comboBoxEstatusMant.Enabled)
                    metodobtnguardar();
            }
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxTrabajoRealizado.Enabled && /*string.IsNullOrWhiteSpace(trabreak)*/ string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))
                    MessageBox.Show("No puede dejar en blanco el trabajo realizado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (comboBoxFalloGral.SelectedIndex == 0)
                    MessageBox.Show("No puede dejar en blanco el grupo de fallo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if ((folfacturanterior != "") && (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)))
                    MessageBox.Show("No puede dejar en blanco el folio de factura", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (!string.IsNullOrWhiteSpace(textBoxFolioFactura.Text) && Convert.ToInt32(textBoxFolioFactura.Text) == 0)
                    MessageBox.Show("El folio de factura debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (!((fgeneralanterior.Equals(comboBoxFalloGral.Text)) && (trabrealizadoanterior.Equals(textBoxTrabajoRealizado.Text)) && (folfacturanterior.Equals(textBoxFolioFactura.Text)) && (observacionesmantanterior.Equals(textBoxObsMan.Text))))
                {
                    if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text) || Convert.ToInt32(textBoxFolioFactura.Text) >= 1)
                    {
                        MySqlCommand cmd01 = new MySqlCommand("SELECT coalesce((FolioFactura), '') AS FolioFactura FROM reportemantenimiento WHERE FolioFactura = '" + textBoxFolioFactura.Text + "'", v.c.dbconection());
                        MySqlDataReader dr01 = cmd01.ExecuteReader();
                        if (dr01.Read())
                            foliofacturarconsulta = Convert.ToString(dr01.GetString("FolioFactura"));
                        dr01.Close();
                        v.c.dbcon.Close();
                        if (((foliofacturarconsulta == textBoxFolioFactura.Text) && (folfacturanterior != foliofacturarconsulta)) && (textBoxFolioFactura.Enabled))
                        {
                            MessageBox.Show("El Folio de Factura ya esta registrado", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxFolioFactura.Text = "";
                        }
                        else
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            if (obs.ShowDialog() == DialogResult.OK)
                            {
                                string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET FalloGralfkFallosGenerales = '" + comboBoxFalloGral.SelectedValue + "', TrabajoRealizado = '" + textBoxTrabajoRealizado.Text + "', FolioFactura = '" + textBoxFolioFactura.Text + "', ObservacionesM = '" + textBoxObsMan.Text + "' WHERE FoliofkSupervicion = '" + idreportesupervision + "'", v.c.dbconection());
                                cmd.ExecuteNonQuery();
                                v.c.dbcon.Close();

                                MySqlCommand cmd0 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion,empresa, area) VALUES('Reporte de Mantenimiento', (SELECT IdReporte FROM reportemantenimiento WHERE FoliofkSupervicion = '" + idreportesupervision + "'), CONCAT('" + fgeneralanterior + ";', '" + folfacturanterior + ";', '" + trabrealizadoanterior + ";', '" + observacionesmantanterior + "'), '" + idUsuario + "', now(), 'Actualización de Reporte de Mantenimiento','" + observaciones + "', '2', '1')", v.c.dbconection());
                                cmd0.ExecuteNonQuery();
                                v.c.dbcon.Close();
                                ClasFallo();
                                MessageBox.Show("Reporte Editado Correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Cancelar(false);
                                bloquea();
                            }
                        }
                        if (label35.Text == "EXPORTANDO")
                            valexcel();
                        else
                            ocultarexcel();
                    }
                }
                else
                {
                    actualizarcbx();
                    AutoCompletado(textBoxFolioB);
                    MessageBox.Show("Sin Modificaciones", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bloquea();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void bloquea()
        {
            metodoCarga();
            limpiarcampos();
            limpiarstring();
            cantidadrefacciones = inicolumn = fincolumn = 0;
            ncontreffin();
            conteo();
            dgvMantenimiento.Refresh();
            buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = buttonEditar.Visible = label58.Visible = comboBoxFalloGral.Enabled = textBoxMecanico.Enabled = textBoxMecanicoApo.Enabled = textBoxFolioFactura.Enabled = textBoxTrabajoRealizado.Enabled = comboBoxEstatusMant.Enabled = comboBoxExisRefacc.Enabled = comboBoxReqRefacc.Enabled = textBoxSuperviso.Enabled = textBoxObsMan.Enabled = buttonAgregar.Visible = label39.Visible = buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
            timer1.Start();
            comboBoxEstatusMant.SelectedIndex = 0;
        }

        private void dataGridViewMantenimiento_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //Doble Click En GridView De Mantenimiento
        {
            mensaje = false;
            if (e.RowIndex >= 0)
            {
                valdeval();
                if (((fgeneralanterior.Equals(validacionfgeneral) || ((valorfallogeneral == 1) || (string.IsNullOrWhiteSpace(fgeneralanterior)))) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && (exisrefaccionanterior.Equals(validacionexisrefacc)) && (reqrefanterior.Equals(validacionreqrefacc)) && (trabrealizadoanterior.Trim().Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Trim().Equals(textBoxFolioFactura.Text.Trim())) && ((estatusmantanterior.Equals(validacionestatusmant)) || (comboBoxEstatusMant.Text.Equals("EN PROCESO"))) && (supervisoanterior.Trim().Equals(labelNomSuperviso.Text.Trim())) && (observacionesmantanterior.Trim().Equals(textBoxObsMan.Text.Trim())) && ((inicolumn == 0) || (inicolumn == fincolumn))))
                {
                    actualizarcbx();
                    AutoCompletado(textBoxFolioB);
                    limpiarstring();
                    inicolumn = fincolumn = 0;
                    registroconteofilaspedref = false;
                    llamadadatos();
                    ncontrefini();
                }
                else
                {
                    int total;
                    total = fincolumn - inicolumn;
                    if (MessageBox.Show("Si usted cambia de reporte y/o actualiza la tabla se perderan los datos ingresados\n\n ¿Esta seguro de querer continuar?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "", v.c.dbconection());
                        cmd1.ExecuteNonQuery();
                        v.c.dbcon.Close();
                        actualizarcbx();
                        AutoCompletado(textBoxFolioB);
                        inicolumn = fincolumn = 0;
                        registroconteofilaspedref = false;
                        limpiarstring();
                        metodocargaref();
                        llamadadatos();
                    }
                }
            }
        }

        private void dataGridViewMRefaccion_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //Doble Click En GridView De Refacciones
        {
            if (e.RowIndex >= 0)
            {
                if ((registroentradapedref) && (!string.IsNullOrWhiteSpace(fgeneralanterior)))
                {
                    existenciaGV = dgvPMantenimiento.CurrentRow.Cells[7].Value.ToString();
                    if ((existenciaGV == "SIN EXISTENCIA") || (existenciaGV == "EXISTENCIA") || (existenciaGV == "INCOMPLETO"))
                        MessageBox.Show("La Refacción ya fue validada por almacen, esta refacción ya no se puede editar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        banderaeditar = buttonActualizar.Visible = label26.Visible = buttonAgregaPed.Visible = label33.Visible = false;
                        buttonAgregarMasPed.Visible = label29.Visible = true;
                        conta = dgvPMantenimiento.CurrentRow.Cells["PARTIDA"].Value.ToString();
                        string sql = "select upper(t1.descripcionfamilia) as familia, t1.idfamilia from cfamilias as t1 INNER JOIN cmarcas as t3 ON t3.descripcionfkcfamilias = t1.idfamilia inner join crefacciones as t2 on t2.marcafkcmarcas = t3.idmarca where upper(t2.nombreRefaccion) = '" + dgvPMantenimiento.CurrentRow.Cells[1].Value + "'and t1.status = '0'";
                        MySqlCommand familia = new MySqlCommand(sql, v.c.dbconection());
                        MySqlDataReader dtr = familia.ExecuteReader();
                        if (dtr.Read())
                        {
                            comboBoxFamilia.DataSource = null;
                            DataTable dt = new DataTable();
                            MySqlCommand cmd2 = new MySqlCommand("SELECT UPPER(familia) AS familia, idfamilia FROM cfamilias WHERE status = '1' ORDER BY familia", v.c.dbconection());
                            MySqlDataAdapter adap = new MySqlDataAdapter(cmd2);
                            adap.Fill(dt);
                            DataRow row2 = dt.NewRow();
                            DataRow row3 = dt.NewRow();
                            row2["idfamilia"] = 0;
                            row2["familia"] = " -- FAMILIA --";
                            row3["idfamilia"] = dtr["idfamilia"];
                            row3["familia"] = dtr["familia"].ToString();
                            dt.Rows.InsertAt(row2, 0);
                            dt.Rows.InsertAt(row3, 1);
                            comboBoxFamilia.ValueMember = "idfamilia";
                            comboBoxFamilia.DisplayMember = "familia";
                            comboBoxFamilia.DataSource = dt;
                            comboBoxFamilia.SelectedIndex = 0;
                            comboBoxFamilia.Text = dtr["familia"].ToString();

                        }
                        string sql2 = " SELECT t1.idPedRef, t5.idcnfamilia, UPPER(t5.familia) AS Familia, t1.Cantidad, UPPER(t2.nombreRefaccion) AS Refaccion, t1.RefaccionfkCRefaccion AS idRefaccion FROM pedidosrefaccion AS t1 INNER JOIN crefacciones AS t2 ON t1.RefaccionfkCRefaccion = t2.idrefaccion INNER JOIN cmarcas as t4 ON t2.marcafkcmarcas = t4.idmarca INNER JOIN cfamilias AS t3 ON t4.descripcionfkcfamilias = t3.idfamilia INNER JOIN cnfamilias as t5 ON t3.familiafkcnfamilias = t5.idcnfamilia WHERE t1.NumRefacc =  '" + conta + "' AND t1.FolioPedfkSupervicion ='" + idreportesupervision + "'";
                        MySqlCommand cmd = new MySqlCommand(sql2, v.c.dbconection());
                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            foliof = Convert.ToString(dr.GetString("idPedRef"));
                            comboBoxFamilia.Text = Convert.ToString(dr.GetString("Familia"));
                            familianterior = Convert.ToString(dr.GetString("Familia"));
                            idfamilianterior = Convert.ToInt32(dr.GetString("idcnFamilia"));
                            comboBoxFRefaccion.Text = Convert.ToString(dr.GetString("Refaccion"));
                            refaccionanterior = comboBoxFRefaccion.Text;
                            idrefaccionanterior = Convert.ToInt32(dr.GetString("idRefaccion"));
                            textBoxCantidad.Text = Convert.ToString(dr.GetString("Cantidad"));
                            cantidadanterior = Convert.ToDouble(dr.GetString("Cantidad"));
                        }
                        comboBoxFRefaccion.Text = dgvPMantenimiento.CurrentRow.Cells[1].Value.ToString();
                        dr.Close();
                        v.c.dbcon.Close();
                        banderaeditar = true;
                    }
                }
                else
                    MessageBox.Show("Para editar una refacción necesita guardar el reporte por 1ra vez", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonActualizarPed_Click(object sender, EventArgs e) //Actualizar Pedido
        {
            if ((comboBoxFamilia.SelectedIndex == 0) && (comboBoxFRefaccion.SelectedIndex == 0) && (string.IsNullOrWhiteSpace(textBoxCantidad.Text)))
                MessageBox.Show("Alguno de Los campos estan vacíos", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFamilia.SelectedIndex == 0)
                MessageBox.Show("El campo de Familia no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFRefaccion.SelectedIndex == 0)
                MessageBox.Show("El campo de Refacción no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
                MessageBox.Show("El campo de cantidad debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
                MessageBox.Show("El campo debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (Convert.ToDouble(textBoxCantidad.Text) <= 0)
                MessageBox.Show("La cantidad debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((comboBoxFamilia.Text.Equals(familianterior)) && (comboBoxFRefaccion.Text.Equals(refaccionanterior)) && (textBoxCantidad.Text.Equals(cantidadanterior)))
                    MessageBox.Show("No se realizó ningún cambio", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        MySqlCommand cmd = new MySqlCommand("UPDATE pedidosrefaccion SET RefaccionfkCRefaccion = '" + comboBoxFRefaccion.SelectedValue + "', Cantidad = '" + Convert.ToDouble(textBoxCantidad.Text) + "' WHERE NumRefacc = '" + conta + "'", v.c.dbconection());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Refacción actualizada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        v.c.dbcon.Close();
                        MySqlCommand cmd0 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion,empresa, area) VALUES('Reporte de Mantenimiento', COALESCE((SELECT IdReporte FROM reportemantenimiento WHERE FoliofkSupervicion = '" + idreportesupervision + "'), 0), CONCAT('" + idrefaccionanterior + ";', '" + cantidadanterior + "'), '" + idUsuario + "', now(), 'Actualización de Refacción en Reporte de Mantenimiento','" + observaciones + "', '2', '1')", v.c.dbconection());
                        cmd0.ExecuteNonQuery();
                        v.c.dbcon.Close();
                        banderaeditar = false;
                    }
                }
                metodocargaref();
                limpiarrefacc();
                buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = false;
                buttonAgregaPed.Visible = label33.Visible = buttonActualizar.Visible = label26.Visible = true;
            }
        }

        private void buttonActualizar_Click(object sender, EventArgs e)  //Actualizar
        {
            botonactualizar();
        }

        private void buttonPDF_Click(object sender, EventArgs e) //Generar PDF
        {
            string comporacion = "";
            if ((radioButtonGeneral.Checked) && (!radioButtonUnidad.Checked))
                comporacion = "el reporte de fallo del mantenimiento en PDF?";
            else
                comporacion = "los datos de la unidad del mantenimiento en PDF?";
            if ((!radioButtonGeneral.Checked) && (!radioButtonUnidad.Checked))
                MessageBox.Show("Favor de seleccionar entre el Reporte de Fallo y Los Datos de la Unidad", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if ((MessageBox.Show("¿Desea generar " + comporacion, "INFORMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
                {
                    if (dgvPMantenimiento.Rows.Count == 0)
                        validaciontablarefacciones = true;
                    else
                        validaciontablarefacciones = false;
                    if (radioButtonGeneral.Checked)
                    {
                        dgvPMantenimiento.Visible = true;
                        metodocargarefpdf();
                        v.c.dbcon.Close();
                        topdfgeneral();
                        metodocargaref();
                    }
                    else if (radioButtonUnidad.Checked)
                    {
                        dgvPMantenimiento.Visible = true;
                        metodocargarefpdf();
                        v.c.dbcon.Close();
                        topdfconunidades();
                        metodocargaref();
                    }
                }
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e) //Buscar
        {
            if ((string.IsNullOrWhiteSpace(textBoxFolioB.Text)) && (comboBoxUnidadB.Text.Equals("-- ECONÓMICO --")) && (comboBoxMecanicoB.Text.Equals("-- MECÁNICO --")) && (comboBoxEstatusMB.Text.Equals("-- ESTATUS --")) && (!checkBoxFechas.Checked) && (comboBoxMesB.Text.Equals("-- MES --")) && (comboBoxDescpFalloB.Text.Equals("--SELECCIONE DESCRIPCIÓN--")))
                MessageBox.Show("Seleccione Un Criterio De Búqueda", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (dateTimePickerIni.Value > dateTimePickerFin.Value)
                {
                    MessageBox.Show("La fecha inicial no debe superar a la fecha final", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    limpiarcamposbus();
                }
                else if ((dateTimePickerIni.Value >= DateTime.Now) || (dateTimePickerFin.Value >= DateTime.Now))
                {
                    MessageBox.Show("Las fechas no deben superar el día de hoy", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    limpiarcamposbus();
                    checkBoxFechas.Checked = false;
                }
                else
                {
                    String Fini = "";
                    String Ffin = "";
                    string consulta = "SET lc_time_names = 'es_ES'; SELECT t1.idReporteSupervicion AS 'ID', t1.Folio AS 'FOLIO', CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) AS ECONÓMICO, UPPER(DATE_FORMAT(t1.FechaReporte, '%W %d %M %Y')) AS 'FECHA DEL REPORTE', coalesce((SELECT UPPER(r21.Estatus) FROM reportemantenimiento AS r21 WHERE t1.idReporteSupervicion = r21.FoliofkSupervicion), '') AS 'ESTATUS DEL MANTENIMIENTO',  coalesce((SELECT UPPER(CONCAT(r22.codfallo, ' - ', r22.falloesp)) FROM cfallosesp AS r22 WHERE t1.CodFallofkcfallosesp = r22.idfalloEsp), '') AS 'CODIGO DE FALLO', coalesce((SELECT UPPER(DATE_FORMAT(r24.FechaReporteM, '%W %d %M %Y')) FROM reportemantenimiento AS r24 WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'FECHA DEL REPORTE DE MANTENIMIENTO', coalesce((SELECT UPPER(CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres)) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion), '') AS 'MECANICO', coalesce((SELECT UPPER(CONCAT(r9.ApPaterno, ' ', r9.ApMaterno, ' ', r9.nombres)) FROM reportemantenimiento AS r8 INNER JOIN cpersonal AS r9 ON r8.MecanicoApoyofkPersonal = r9.idPersona WHERE t1.idReporteSupervicion = r8.FoliofkSupervicion), '') AS 'MECANICO DE APOYO',  coalesce((SELECT UPPER(CONCAT(r1.ApPaterno, ' ', r1.ApMaterno, ' ', r1.nombres)) FROM cpersonal AS r1 WHERE t1.SupervisorfkCPersonal = r1.idPersona), '') AS Supervisor, UPPER(t1.HoraEntrada) AS 'HORA DE ENTRADA', UPPER(t1.TipoFallo) AS 'TIPO DE FALLO', UPPER(t1.KmEntrada) AS 'KILOMETRAJE', coalesce((SELECT UPPER(r21.descfallo) FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo),'') AS 'SUBGRUPO DE FALLO', UPPER(t1.DescFalloNoCod) AS 'SUBGRUPO DE FALLO NO CODIFICADO', coalesce((UPPER(t1.ObservacionesSupervision)), '') AS 'OBSERVACIONES DE SUPERVISION', coalesce((SELECT UPPER(r4.nombreFalloGral) FROM reportemantenimiento AS r3 INNER JOIN cfallosgrales AS r4 ON r3.FalloGralfkFallosGenerales = r4.idFalloGral WHERE t1.idReporteSupervicion = r3.FoliofkSupervicion), '') AS 'GRUPO DE FALLO', coalesce((SELECT UPPER(r5.TrabajoRealizado) FROM reportemantenimiento AS r5 WHERE t1.idReporteSupervicion = r5.FoliofkSupervicion), '') AS 'TRABAJO REALIZADO', coalesce((SELECT r11.HoraInicioM FROM reportemantenimiento AS r11 WHERE t1.idReporteSupervicion = r11.FoliofkSupervicion), '') AS 'HORA DE INICIO DE MANTENIMIENTO', coalesce((SELECT r12.HoraTerminoM FROM reportemantenimiento AS r12 WHERE t1.idReporteSupervicion = r12.FoliofkSupervicion), '') AS 'HORA DE TERMINO DE MANTENIMIENTO', coalesce((SELECT UPPER(r13.EsperaTiempoM) FROM reportemantenimiento AS r13 WHERE t1.idReporteSupervicion = r13.FoliofkSupervicion), '') AS 'ESPERA DE TIEMPO PARA MANTENIMIENTO', coalesce((SELECT UPPER(r14.DiferenciaTiempoM) FROM reportemantenimiento AS r14 WHERE t1.idReporteSupervicion = r14.FoliofkSupervicion), '') AS 'DIFERENCIA DE TIEMPO EN MANTENIMIENTO', coalesce((SELECT r15.FolioFactura FROM reportemantenimiento AS r15 WHERE t1.idReporteSupervicion = r15.FoliofkSupervicion), '') AS 'FOLIO DE FACTURA', coalesce((SELECT UPPER(CONCAT(r17.ApPaterno, ' ', r17.ApMaterno, ' ', r17.nombres)) FROM reportemantenimiento AS r16 INNER JOIN cpersonal AS r17 ON r16.SupervisofkPersonal = r17.idPersona WHERE t1.idReporteSupervicion = r16.FoliofkSupervicion), '') AS 'SUPERVISO', coalesce((SELECT UPPER(r18.ExistenciaRefaccAlm) FROM reportemantenimiento AS r18 WHERE t1.idReporteSupervicion = r18.FoliofkSupervicion), '') AS 'EXISTENCIA DE REFACCIONES EN ALMACEN', coalesce((SELECT UPPER(r19.StatusRefacciones) FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion), '') AS 'ESTATUS DE REFACCIONES', coalesce((SELECT UPPER(CONCAT(r23.ApPaterno, ' ', r23.ApMaterno, ' ', r23.nombres)) FROM reportemantenimiento AS r24 INNER JOIN cpersonal AS r23 ON r24.PersonaFinal = r23.idPersona WHERE t1.idReporteSupervicion = r24.FoliofkSupervicion), '') AS 'PERSONA QUE FINALIZO EL MANTENIMIENTO', coalesce((SELECT UPPER(r20.ObservacionesM) FROM reportemantenimiento AS r20 WHERE t1.idReporteSupervicion = r20.FoliofkSupervicion), '') AS 'OBSERVACIONES DEL MANTENIMIENTO' FROM reportesupervicion AS t1 INNER JOIN cunidades AS t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN careas AS t4 ON t2.areafkcareas = t4.idarea ";
                    string WHERE = "";
                    if (!string.IsNullOrWhiteSpace(textBoxFolioB.Text))
                        if (WHERE == "")
                            WHERE = " WHERE t1.Folio = '" + textBoxFolioB.Text + "'";
                        else
                            WHERE += " AND t1.Folio = '" + textBoxFolioB.Text + "'";
                    if (comboBoxUnidadB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) = '" + comboBoxUnidadB.Text + "'";
                        else
                            WHERE += " AND CONCAT(t4.identificador, LPAD(consecutivo, 4,'0')) = '" + comboBoxUnidadB.Text + "'";
                    if (comboBoxMecanicoB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion) = '" + comboBoxMecanicoB.Text + "'";
                        else
                            WHERE += " AND (SELECT CONCAT(r7.ApPaterno, ' ', r7.ApMaterno, ' ', r7.nombres) FROM reportemantenimiento AS r6 INNER JOIN cpersonal AS r7 ON r6.MecanicofkPersonal = r7.idPersona WHERE t1.idReporteSupervicion = r6.FoliofkSupervicion) = '" + comboBoxMecanicoB.Text + "'";
                    if (comboBoxEstatusMB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT r19.Estatus FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion) = '" + comboBoxEstatusMB.Text + "'";
                        else
                            WHERE += " AND (SELECT r19.Estatus FROM reportemantenimiento AS r19 WHERE t1.idReporteSupervicion = r19.FoliofkSupervicion) = '" + comboBoxEstatusMB.Text + "'";
                    if (checkBoxFechas.Checked == true)
                        Fini = dateTimePickerIni.Value.ToString("yyyy-MM-dd");
                    Ffin = dateTimePickerFin.Value.ToString("yyyy-MM-dd");
                    if (WHERE == "")
                        WHERE = " WHERE (SELECT t1.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    else
                        WHERE += " AND (SELECT t1.FechaReporte BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    if (comboBoxMesB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT t1.FechaReporte WHERE MONTH(t1.FechaReporte) = '" + month + "' AND YEAR(t1.FechaReporte) = YEAR(Now()))";
                        else
                            WHERE += " AND (SELECT t1.FechaReporte WHERE MONTH(t1.FechaReporte) = '" + month + "' AND YEAR(t1.FechaReporte) = YEAR(Now()))";
                    if (comboBoxDescpFalloB.SelectedIndex > 0)
                        if (WHERE == "")
                            WHERE = " WHERE (SELECT r21.descfallo FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo) = '" + comboBoxDescpFalloB.Text + "'";
                        else
                            WHERE += " AND (SELECT r21.descfallo FROM cdescfallo AS r21 WHERE r21.iddescfallo = t1.DescrFallofkcdescfallo) = '" + comboBoxDescpFalloB.Text + "'";
                    if (WHERE != "")
                        WHERE += " AND  (SELECT empresaMantenimiento FROM cmodelos WHERE idmodelo = t2.modelofkcmodelos) ='" + empresa + "' ORDER BY t1.Folio DESC";
                    MySqlDataAdapter adp = new MySqlDataAdapter(consulta + WHERE, v.c.dbconection());
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    dgvMantenimiento.DataSource = ds.Tables[0];
                    dgvMantenimiento.Columns[0].Frozen = dgvMantenimiento.Columns[1].Frozen = dgvMantenimiento.Columns[2].Frozen = true;
                    dgvMantenimiento.Columns[0].Visible = dgvMantenimiento.Columns[1].Visible = false;
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron reportes", "ADVERTEMCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        limpiarcamposbus();
                        metodoCarga();
                        conteo();
                        buttonExcel.Visible = label35.Visible = false;
                    }
                    else
                    {
                        conteovariable();
                        limpiarcamposbus();
                        if (!activado)
                            buttonExcel.Visible = true;
                        label35.Visible = true;
                    }
                    v.c.dbcon.Close();
                    checkBoxFechas.Checked = false;
                }
            }
        }

        private void comboBoxFamilia_SelectedIndexChanged(object sender, EventArgs e) // UNIR LOS 2 INDEX CHANGUED
        {
            if (banderaeditar)
            {
                if ((idfamilianterior.Equals(comboBoxFamilia.SelectedValue) || comboBoxFamilia.SelectedIndex == 0) && (idrefaccionanterior.Equals(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior.Equals(Convert.ToDouble(textBoxCantidad.Text)) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
            }
        }

        private void comboBoxFRefaccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                if ((idfamilianterior.Equals(comboBoxFamilia.SelectedValue) || comboBoxFamilia.SelectedIndex == 0) && (idrefaccionanterior.Equals(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior.Equals(Convert.ToDouble(textBoxCantidad.Text)) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
            }
        }

        private void textBoxTrabajoRealizado_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text.Contains(Environment.NewLine))
                txt.Text.Replace(Environment.NewLine, "");
        }

        private void labelCerrarHerr_Click(object sender, EventArgs e) //Acciones Del Label Cerrar De Refacciones
        {
            banderaeditar = groupBoxRefacciones.Visible = label1.Visible = false;
            gbxMantenimiento.Visible = buttonGuardar.Visible = label24.Visible = buttonActualizar.Visible = label26.Visible = true;
            limpiarrefacc();
            if (dgvPMantenimiento.Rows.Count == 0)
                comboBoxReqRefacc.SelectedIndex = 2;
            else
                comboBoxReqRefacc.SelectedIndex = 1;
            if (fgeneralanterior == "" && mecanicoanterior == ".")
                Cancelar(false);
            else
                Cancelar(true);
            comboBoxExisRefacc.Enabled = false;
        }

        private void buttonAgregaPed_Click(object sender, EventArgs e) //Agrega Una Nueva Refaccion
        {
            double pedcantidad = 0;
            if (textBoxCantidad.Text == "")
                textBoxCantidad.Text = "0";
            try
            {
                pedcantidad = Convert.ToDouble(textBoxCantidad.Text);
            }
            catch
            { }
            if ((comboBoxFamilia.SelectedIndex == 0) && (comboBoxFRefaccion.SelectedIndex == 0) && (string.IsNullOrWhiteSpace(textBoxCantidad.Text)))
                MessageBox.Show("Los campos están vacios", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFamilia.SelectedIndex == 0)
                MessageBox.Show("El campo de Familia no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (comboBoxFRefaccion.SelectedIndex == 0)
                MessageBox.Show("El campo de Refacción no puede quedarse en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (string.IsNullOrWhiteSpace(textBoxCantidad.Text))
                MessageBox.Show("El campo de cantidad debe de tener al menos un digito", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (pedcantidad == 0)
                MessageBox.Show("La cantidad debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (pedcantidad == 0)
                    MessageBox.Show("La cantidad debe ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MySqlCommand cmd0 = new MySqlCommand("SELECT NumRefacc FROM pedidosrefaccion WHERE FolioPedfkSupervicion= '" + idreportesupervision + "' ORDER BY idPedRef DESC ", v.c.dbconection());
                MySqlDataReader dr0 = cmd0.ExecuteReader();
                if (dr0.Read())
                {
                    enumeradorefacciones = Convert.ToInt32(dr0.GetString("NumRefacc"));
                    enumeradorefacciones = enumeradorefacciones + 1;
                }
                else
                {
                    enumeradorefacciones = 0;
                    enumeradorefacciones = enumeradorefacciones + 1;
                }
                dr0.Close();
                v.c.dbcon.Close();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO pedidosrefaccion(NumRefacc, FolioPedfkSupervicion, RefaccionfkCRefaccion, FechaPedido, HoraPedido, Cantidad,usuariofkcpersonal) VALUES ('" + enumeradorefacciones + "', '" + idreportesupervision + "', '" + comboBoxFRefaccion.SelectedValue + "', curdate(), Time(Now()),'" + textBoxCantidad.Text + "','" + idmecanicoanterior + "')", v.c.dbconection());
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                adp.Fill(dt);
                dgvPMantenimiento.DataSource = dt;
                v.c.dbcon.Close();
                ncontreffin();
                conteofinref();
                metodocargaref();
                MySqlCommand sql = new MySqlCommand("UPDATE reportemantenimiento SET seenalmacen = 0 WHERE FoliofkSupervicion = '" + idreportesupervision + "' ", v.c.dbconection());
                sql.ExecuteNonQuery();
                MessageBox.Show("Refacción agregada correctamente", "COMPLETADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cantidadrefacciones = cantidadrefacciones + 1;
                limpiarrefacc();
                comboBoxExisRefacc.SelectedIndex = 2;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                valdeval();
                if (((fgeneralanterior.Equals(validacionfgeneral) || ((valorfallogeneral == 1) || (string.IsNullOrWhiteSpace(fgeneralanterior)))) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && (exisrefaccionanterior.Equals(validacionexisrefacc)) && (reqrefanterior.Equals(validacionreqrefacc)) && (trabrealizadoanterior.Trim().Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Trim().Equals(textBoxFolioFactura.Text.Trim())) && ((estatusmantanterior.Equals(validacionestatusmant)) || (comboBoxEstatusMant.Text.Equals("EN PROCESO"))) && (supervisoanterior.Trim().Equals(labelNomSuperviso.Text.Trim())) && (observacionesmantanterior.Trim().Equals(textBoxObsMan.Text.Trim())) && ((inicolumn == 0) || (inicolumn == fincolumn))))
                    limpiar();
                else
                {
                    if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        buttonEditar_Click(null, e);
                    limpiar();
                }
            }
            else
            {
                valdeval();
                string exisref = "";
                if (comboBoxExisRefacc.SelectedIndex > 0)
                    exisref = comboBoxExisRefacc.Text;
                string requiereRefac = "";
                if (comboBoxReqRefacc.SelectedIndex > 0)
                    requiereRefac = comboBoxReqRefacc.Text;
                if ((fgeneralanterior.Equals(comboBoxFalloGral.Text)) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && ((comboBoxExisRefacc.SelectedIndex == 0) || (exisrefaccionanterior.Equals(exisref))) && (reqrefanterior.Equals(requiereRefac)) && (trabrealizadoanterior.Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Equals(textBoxFolioFactura.Text)) && (estatusmantanterior.Equals(comboBoxEstatusMant.Text)) && (supervisoanterior.Equals(labelNomSuperviso.Text)) && (observacionesmantanterior.Equals(textBoxObsMan.Text)) && (inicolumn == fincolumn))
                    limpiar();
                else
                {
                    if (!string.IsNullOrWhiteSpace(labelFolio.Text.Trim()))
                    {
                        if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            buttonEditar_Click(null, e);
                        else
                        {
                            int total;
                            if (registroconteofilaspedref)
                            {
                                total = dgvPMantenimiento.Rows.Count - inicolumn;
                                MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "", v.c.dbconection());
                                cmd1.ExecuteNonQuery();
                                v.c.dbcon.Close();
                                botonactualizar();
                            }
                        }
                    }
                    limpiar();
                }
            }
        }

        void limpiar()
        {
            actualizarcbx();
            AutoCompletado(textBoxFolioB);
            limpiarstring();
            inicolumn = 0;
            fincolumn = 0;
            registroconteofilaspedref = buttonPDF.Visible = label36.Visible = radioButtonGeneral.Visible = radioButtonUnidad.Visible = false;
            metodocargaref();
            conteoiniref();
            conteofinref();
            limpiarrefacc();
            limpiarcampos();
            limpiarstring();
            limpiarcampos();
            limpiarcamposbus();
            enabledfalse();
            ocultarexcel();
            metodoCarga();
            comboBoxEstatusMant.SelectedIndex = 0;
            Cancelar(false);
        }

        private void textBoxFolioB_Click(object sender, EventArgs e)
        {
            textBoxFolioB.SelectAll();
        }

        /* Validaciones de los campos de contraseña*/
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxLargo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }

        private void textBoxContras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsNumber(e.KeyChar)) || (Char.IsLetter(e.KeyChar) || (e.KeyChar == 8) || (e.KeyChar == 127)))
                e.Handled = false;
            else if (e.KeyChar == 32)
            {
                e.Handled = true;
                MessageBox.Show("Solo puede ingresar números y letras en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo puede ingresar números y letras en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxFolioFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127 || e.KeyChar == 08)
                e.Handled = false;
            else if (Char.IsNumber(e.KeyChar))
                e.Handled = false;
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan números en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Solo se pueden introducir números y un solo punto decimal", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ya existe un punto decimal en la caja de texto", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            valida_refacciones();
        }

        /* Validaciones de las contraseñas */
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxSuperviso_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(textBoxSuperviso.Text) + "'AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidSuperviso.Text = dr["idPersona"].ToString();
                labelNomSuperviso.Text = dr["Nombre"] as string;
                if ((labelNomSuperviso.Text == labelNomMecanico.Text) || (labelNomSuperviso.Text == labelNomMecanicoApo.Text))
                {
                    MessageBox.Show("El Supervisor no debe ser igual al Mecánico y/o Mecánico de Apoyo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomSuperviso.Text = "...";
                    textBoxSuperviso.Text = "";
                }
            }
            else if (textBoxSuperviso.Text != "")
            {
                labelidSuperviso.Text = "";
                labelNomSuperviso.Text = "...";
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        private void textBoxMecanicoApo_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, coalesce(UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)), '') AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(textBoxMecanicoApo.Text) + "'AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidMecanicoApo.Text = dr["idPersona"].ToString();
                labelNomMecanicoApo.Text = dr["Nombre"] as string;
                if ((labelNomMecanicoApo.Text == labelNomMecanico.Text) || (labelNomMecanicoApo.Text == labelNomSuperviso.Text))
                {
                    MessageBox.Show("El Mecánico de Apoyo no debe ser igual al Mecánico y/o al Supervisor", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomMecanicoApo.Text = "..";
                    textBoxMecanicoApo.Text = "";
                }
            }
            else if (textBoxMecanicoApo.Text != "")
            {
                labelidMecanicoApo.Text = "";
                labelNomMecanicoApo.Text = "..";
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        private void textBoxMecanico_Leave(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idPersona, coalesce(UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)),'') AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(textBoxMecanico.Text) + "' AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "' AND t1.status = '1'", v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                labelidMecanico.Text = dr["idPersona"].ToString();
                labelNomMecanico.Text = dr["Nombre"] as string;
                if ((labelNomMecanico.Text == labelNomMecanicoApo.Text) || (labelNomMecanico.Text == labelNomSuperviso.Text))
                {
                    MessageBox.Show("El Mecánico no debe ser igual al Supervisor y/o al Mecánico de Apoyo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelNomMecanico.Text = ".";
                    textBoxMecanico.Text = "";
                }
                else if (comboBoxFalloGral.SelectedIndex > 0 && banderaeditar == false)
                    comboBoxReqRefacc.Enabled = true;
                else
                    comboBoxReqRefacc.Enabled = false;
            }
            else if (textBoxMecanico.Text != "")
            {
                labelidMecanico.Text = "";
                labelNomMecanico.Text = ".";
            }
            dr.Close();
            v.c.dbcon.Close();
        }

        /*Validaciones extras */
        private void comboBoxFamilia_SelectedValueChanged(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                if ((idfamilianterior.Equals(comboBoxFamilia.SelectedValue) || comboBoxFamilia.SelectedIndex == 0) && (idrefaccionanterior.Equals(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior.Equals(Convert.ToDouble(textBoxCantidad.Text)) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
                if (comboBoxFamilia.SelectedIndex > 0)
                {
                    v.iniCombos("SELECT UPPER(t1.nombreRefaccion) AS 'nombreRefaccion', t1.idrefaccion FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas=t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias=t3.idfamilia INNER JOIN cnfamilias as t4 ON t3.familiafkcnfamilias=t4.idcnfamilia WHERE t4.idcnfamilia='" + comboBoxFamilia.SelectedValue + "' AND t1.status=1;", comboBoxFRefaccion, "idrefaccion", "nombreRefaccion", "-- REFACCION --");
                    comboBoxFRefaccion.Enabled = true;
                }
                else
                {
                    comboBoxFRefaccion.Enabled = false;
                    comboBoxFRefaccion.DataSource = null;
                }
            }
            else
            {
                if (comboBoxFamilia.SelectedIndex > 0)
                {
                    v.iniCombos("SELECT UPPER(t1.nombreRefaccion) AS 'nombreRefaccion', t1.idrefaccion FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas=t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias=t3.idfamilia INNER JOIN cnfamilias as t4 ON t3.familiafkcnfamilias=t4.idcnfamilia WHERE t4.idcnfamilia='" + comboBoxFamilia.SelectedValue + "' AND t1.status=1;", comboBoxFRefaccion, "idrefaccion", "nombreRefaccion", "-- REFACCION --");
                    comboBoxFRefaccion.Enabled = true;
                }
                else
                {
                    comboBoxFRefaccion.Enabled = false;
                    comboBoxFRefaccion.DataSource = null;
                }
            }
        }

        private void comboBoxFRefaccion_SelectedValueChanged(object sender, EventArgs e)
        {
            MySqlCommand cmd0 = new MySqlCommand("select t1.simbolo as 'Unidad De Medida',t1.idunidadmedida,t4.nombreRefaccion from cunidadmedida as t1 inner join cfamilias as t2 on t2.umfkcunidadmedida=t1.idunidadmedida inner join cmarcas as t3 on t3.descripcionfkcfamilias=t2.idfamilia inner join crefacciones as t4 on t4.marcafkcmarcas=t3.idmarca inner join cnfamilias as t5 on t5.idcnFamilia=t2.familiafkcnfamilias WHERE t5.idcnFamilia = '" + comboBoxFamilia.SelectedValue + "' AND t4.idrefaccion = '" + comboBoxFRefaccion.SelectedValue + "'", v.c.dbconection());
            MySqlDataReader dr = cmd0.ExecuteReader();
            if (dr.Read())
            {
                nombrerefaccionconsulta = Convert.ToString(dr.GetString("nombreRefaccion"));
                unidadmedidaconsulta = Convert.ToString(dr.GetString("Unidad De Medida"));
                idunidadmedidaconsulta = Convert.ToInt32(dr.GetString("idunidadmedida"));
            }
            dr.Close();
            v.c.dbcon.Close();
            textBoxUM.Text = unidadmedidaconsulta;
        }

        private void comboBoxReqRefacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBoxRefacciones.Visible = false;
            if ((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES")) && (comboBoxReqRefacc.Enabled == true))
            {
                metodocargaref();
                //registroentradapedref = false;
                if (registroconteofilaspedref == false)
                {
                    inicolumn = 0;
                    inicolumn = dgvPMantenimiento.Rows.Count;
                    validacionfinalconteocolumnas = true;
                    //registroconteofilaspedref = true;
                }
                buttonGuardar.Visible = label24.Visible = buttonActualizarPed.Visible = label3.Visible = buttonAgregarMasPed.Visible = label29.Visible = gbxMantenimiento.Visible = false;
                buttonAgregaPed.Visible = label33.Visible = groupBoxRefacciones.Visible = label1.Visible = true;
                if (!registroentradapedref) // VERIFICAR ESTA VALIDACIÓN
                    registroentradapedref = label62.Visible = label63.Visible = true;
                dgvPMantenimiento.Visible = true;
                conteoiniref();
                conteofinref();
                if (comboBoxReqRefacc.Text == "SE REQUIEREN REFACCIONES")
                    comboBoxExisRefacc.Enabled = true;
                Cancelar(false);
            }
            else if (((comboBoxReqRefacc.Text == "NO SE REQUIEREN REFACCIONES")) && (!buttonEditar.Visible))
            {
                metodocargaref();
                conteoiniref();
                if (labelrefini.Text != "0")
                {
                    MessageBox.Show("Ya existen refacciones en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxReqRefacc.SelectedIndex = 1;
                    groupBoxRefacciones.Visible = label1.Visible = false;
                    gbxMantenimiento.Visible = buttonGuardar.Visible = textBoxSuperviso.Enabled = true;
                    if (fgeneralanterior != "")
                        Cancelar(true);
                }
                else
                {
                    buttonAgregar.Visible = label39.Visible = comboBoxExisRefacc.Enabled = textBoxFolioFactura.Enabled = false;
                    comboBoxExisRefacc.SelectedIndex = 0;
                    textBoxFolioFactura.Text = "";
                    if (fgeneralanterior != "")
                        Cancelar(true);
                }
            }
            else if ((comboBoxReqRefacc.SelectedIndex == 0) && (buttonAgregar.Enabled) && !(comboBoxFalloGral.SelectedIndex == 0))
            {
                metodocargaref();
                conteoiniref();
                if (labelrefini.Text != "0")
                {
                    MessageBox.Show("Ya existen refacciones en este reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBoxReqRefacc.SelectedIndex = 1;
                    groupBoxRefacciones.Visible = label1.Visible = false;
                    gbxMantenimiento.Visible = buttonGuardar.Visible = label24.Visible = true;
                }
                else
                {
                    buttonAgregar.Visible = label39.Visible = comboBoxExisRefacc.Enabled = textBoxFolioFactura.Enabled = false;
                    comboBoxExisRefacc.SelectedIndex = 0;
                    textBoxFolioFactura.Text = "";
                }
            }
        }

        private void comboBoxExisRefacc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((existencias == false) && (comboBoxFalloGral.Text != "") && (buttonActualizar.Visible) && (!buttonGuardar.Visible))
            { }
            else if ((existencias == false) && (comboBoxFalloGral.Text != "") && (buttonActualizar.Visible) && (buttonGuardar.Visible) && ((comboBoxExisRefacc.Enabled) || ((!comboBoxExisRefacc.Enabled) && (comboBoxFalloGral.SelectedIndex == 0))))
            {
                if (((string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) || ((!textBoxFolioFactura.Enabled) || (textBoxFolioFactura.Enabled)) && (textBoxFolioFactura.Text != "")))
                {
                    if ((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")))
                    {
                        if (!textBoxFolioFactura.Enabled == false)
                        {
                            textBoxFolioFactura.Enabled = false;
                            textBoxFolioFactura.Text = "";
                        }
                    }
                    if (!(comboBoxFalloGral.SelectedIndex == 0))
                    {
                        valida_refacciones();
                        if (totalrefacciones == totalexistenciarefaccinoes)
                        {
                            validacionconteo = validacionconteo + 3;
                            if (totalrefacciones == validacionexistenciarefacciones)
                            {
                                validacionconteo = validacionconteo + 2;
                                if (totalfaltante == 0)
                                {
                                    validacionconteo = validacionconteo + 1;
                                    if (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text))
                                    {
                                        textBoxFolioFactura.Enabled = true;
                                    }
                                }
                                else
                                {
                                    validacionconteo = validacionconteo + 2;
                                }
                            }
                            else
                            {
                                validacionconteo = validacionconteo + 1;
                            }
                        }
                        else
                        {
                            validacionconteo = validacionconteo + 1;
                        }
                        if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (!textBoxFolioFactura.Enabled) && (validacionconteo == 1))
                        {
                            if (!groupBoxRefacciones.Visible == true)
                            {
                                if (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES") && !mensaje)
                                    MessageBox.Show("No todas las refacciones solicitadas estan validadas\n Espere hasta que las validen", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else if (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES"))
                                { }
                                else if (comboBoxExisRefacc.SelectedIndex == 0)
                                    MessageBox.Show("Seleccione otra opción valida", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            conteorefaccionesverificadas = 2;
                            validar2();
                            validacionconteo = 0;
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (!textBoxFolioFactura.Enabled) && (validacionconteo == 4))
                        {
                            if (!(comboBoxFalloGral.Text == "-- GRUPO --"))
                            {
                                MessageBox.Show("No todas las refacciones solicitadas están en existencia\n Espere hasta que almacén vuelva a tener existencias", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            conteorefaccionesverificadas = 2;
                            validar2();
                            validacionconteo = 0;
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("-- EXISTENCIA --")) || (comboBoxExisRefacc.SelectedIndex == 0)) && (!textBoxFolioFactura.Enabled) && (validacionconteo == 7))
                        {
                            MessageBox.Show("No todas las refacciones han sido entregadas\nespere hasta que almacén le entregue todas las refacciones", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            conteorefaccionesverificadas = 2;
                            validar2();
                            validacionconteo = 0;
                        }
                        else if (((comboBoxExisRefacc.Text.Equals("EN ESPERA DE LA REFACCIÓN")) || (comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES")) || (comboBoxExisRefacc.Text.Equals("SIN REFACCIONES")) || (comboBoxExisRefacc.SelectedIndex == 0)) && ((!textBoxFolioFactura.Enabled) || (textBoxFolioFactura.Enabled)) && (validacionconteo == 6) && !mensaje)
                        {
                            conteorefaccionesverificadas = 1;
                            validar2();
                            validacionconteo = 0;
                        }
                    }
                    else if ((textBoxFolioFactura.Text != "") && ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) || (comboBoxEstatusMant.Text.Equals("REPROGRAMADA"))))
                        buttonAgregar.Visible = label34.Visible = true;
                }
            }
            else if (existencias)
            {
                existencias = false;
                validacionconteo = 0;
            }
        }

        private void comboBoxEstatusMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((comboBoxReqRefacc.Text.Equals("SE REQUIEREN REFACCIONES") && comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES") && textBoxFolioFactura.Text != "") || (comboBoxReqRefacc.Text.Equals("NO SE REQUIEREN REFACCIONES"))) && (((comboBoxEstatusMant.Text.Equals("LIBERADA") && comboBoxEstatusMant.Enabled && !string.IsNullOrWhiteSpace(labelFolio.Text))) || (!comboBoxReqRefacc.Enabled && !comboBoxExisRefacc.Enabled && comboBoxEstatusMant.Text.Equals("LIBERADA"))))
            {
                if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (!comboBoxEstatusMant.Enabled))
                    buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
                else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (comboBoxEstatusMant.Enabled))
                {
                    if ((string.IsNullOrWhiteSpace(comboBoxFalloGral.Text)) && (string.IsNullOrWhiteSpace(textBoxMecanico.Text)) && (string.IsNullOrWhiteSpace(textBoxFolioFactura.Text)) && (comboBoxEstatusMant.Text != "LIBERADA") && (string.IsNullOrWhiteSpace(comboBoxReqRefacc.Text)) && (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text)))
                    {
                        MessageBox.Show("Algunos campos les faltan información", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        validar();
                    }
                    else if (string.IsNullOrWhiteSpace(textBoxTrabajoRealizado.Text))
                    {
                        MessageBox.Show("El trabajo realizado no puede quedar en blanco", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        validar();
                    }
                    else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (estatusmantGV.Equals("LIBERADA")))
                        buttonGuardar.Visible = label24.Visible = buttonFinalizar.Visible = label37.Visible = false;
                    else
                    {
                        if ((comboBoxFalloGral.Text != "") && (labelNomMecanico.Text != ".") && (textBoxTrabajoRealizado.Text != "") && (comboBoxEstatusMant.Text != ""))
                        {
                            timer2.Stop();
                            sumafech();
                            labelHoraTerminoM.Text = dateTimePicker2.Text;
                            comboBoxReqRefacc.Enabled = comboBoxExisRefacc.Enabled = buttonGuardar.Visible = label24.Visible = buttonAgregar.Visible = label39.Visible = false;
                            buttonFinalizar.Visible = label37.Visible = true;
                            Cancelar(false);
                        }
                        else
                        {
                            MessageBox.Show("Verifique los datos ingresados, puede que falten algunos por llenar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            validar();
                        }
                    }
                }
                else if ((comboBoxEstatusMant.Text.Equals("REPROGRAMADA")) && (string.IsNullOrWhiteSpace(estatusmantGV)) || (estatusmantGV.Equals("REPROGRAMADA")))
                {
                    labelHoraTerminoM.Text = textBoxTerminoMan.Text = "";
                    buttonFinalizar.Visible = label37.Visible = false;
                    buttonGuardar.Visible = label24.Visible = true;
                    Cancelar(true);
                }
                else if ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) && (string.IsNullOrWhiteSpace(estatusmantGV)) || (estatusmantGV.Equals("EN PROCESO")))
                {
                    labelHoraTerminoM.Text = textBoxTerminoMan.Text = "";
                    buttonFinalizar.Visible = label37.Visible = false;
                    buttonGuardar.Visible = label24.Visible = true;
                    Cancelar(true);
                }
            }
            else if ((comboBoxEstatusMant.Text.Equals("LIBERADA")) && (!string.IsNullOrWhiteSpace(labelFolio.Text)))
            {
                string textomensajexistencia = "";
                if (comboBoxReqRefacc.Text.Equals("-- REQUISICIÓN --"))
                    textomensajexistencia = "no se ha seleccionado si se requieren refacciones";
                else if (!comboBoxExisRefacc.Text.Equals("EXISTENCIA DE REFACCIONES"))
                    textomensajexistencia = "falta la entrega de refacciones";
                else if (!(textBoxFolioFactura.Text != ""))
                    textomensajexistencia = "falta el folio de factura";
                MessageBox.Show("Aún no puede poner el reporte en estatus \'LIBERADA\' \nPorque " + textomensajexistencia + "", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxEstatusMant.SelectedIndex = 1;
            }
            else if ((comboBoxEstatusMant.Text.Equals("EN PROCESO")) || (comboBoxEstatusMant.Text.Equals("REPROGRAMADA")))
            {
                if (!(comboBoxReqRefacc.SelectedIndex == 0))
                    if (comboBoxReqRefacc.SelectedIndex != 2)
                        if (!comboBoxFalloGral.Enabled)
                            buttonAgregar.Visible = label39.Visible = true;
                buttonFinalizar.Visible = label37.Visible = false;
                buttonGuardar.Visible = label24.Visible = true;
                if (comboBoxReqRefacc.SelectedIndex > 0 && labelNomMecanico.Text != "" && !comboBoxFalloGral.Enabled)
                    comboBoxReqRefacc.Enabled = true;
            }
        }

        private void checkBoxFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFechas.Checked)
            {
                dateTimePickerIni.Enabled = dateTimePickerFin.Enabled = true;
                comboBoxMesB.Enabled = false;
                comboBoxMesB.SelectedIndex = 0;
            }
            else
            {
                dateTimePickerIni.Enabled = dateTimePickerFin.Enabled = false;
                comboBoxMesB.Enabled = true;
            }
        }

        private void dateTimePickerAll_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        /* Acciones al presionar una tecla o dar click*/
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxSuperviso_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSuperviso.Text))
                labelNomSuperviso.Text = "...";
        }

        private void textBoxMecanicoApo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMecanicoApo.Text))
                labelNomMecanicoApo.Text = "..";
        }

        private void comboBoxMesB_TextChanged(object sender, EventArgs e)
        {
            month = "00";
            if (comboBoxMesB.Text.Equals("ENERO"))
                month = "01";
            else if (comboBoxMesB.Text.Equals("FEBRERO"))
                month = "02";
            else if (comboBoxMesB.Text.Equals("MARZO"))
                month = "03";
            else if (comboBoxMesB.Text.Equals("ABRIL"))
                month = "04";
            else if (comboBoxMesB.Text.Equals("MAYO"))
                month = "05";
            else if (comboBoxMesB.Text.Equals("JUNIO"))
                month = "06";
            else if (comboBoxMesB.Text.Equals("JULIO"))
                month = "07";
            else if (comboBoxMesB.Text.Equals("AGOSTO"))
                month = "08";
            else if (comboBoxMesB.Text.Equals("SEPTIEMBRE"))
                month = "09";
            else if (comboBoxMesB.Text.Equals("OCTUBRE"))
                month = "10";
            else if (comboBoxMesB.Text.Equals("NOVIEMBRE"))
                month = "11";
            else if (comboBoxMesB.Text.Equals("DICIEMBRE"))
                month = "12";
        }

        private void textBoxMecanico_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMecanico.Text))
                labelNomMecanico.Text = ".";
        }

        /* Movimiento de los botónes y bloqueo de la rueda de mouse */
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonAll_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(60, 60);
        }

        private void btnall_MouseLeave(object sender, EventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(55, 55);
        }

        private void btnallbusq_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(40, 40);
        }

        private void btnallbusq_MouseLeave(object sender, EventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(35, 35);
        }

        void comboBoxAll_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void groupBoxAll_Paint(object sender, PaintEventArgs e)
        {
            GroupBox bx = sender as GroupBox;
            DrawGroupBox(bx, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void textBoxAll_Validated(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            while (txt.Text.Contains("  "))
                txt.Text = txt.Text.Replace("  ", " ");
        }

        private void dataGridViewAll_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void FormFallasMantenimiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            int total;
            hilo.Abort();
            if (!registroconteofilaspedref && idreportesupervision > 0)
            {
                total = dgvPMantenimiento.Rows.Count - inicolumn;
                MySqlCommand cmd1 = new MySqlCommand("DELETE FROM pedidosrefaccion WHERE FechaPedido = curdate() ORDER BY idPedRef DESC LIMIT " + total + "", v.c.dbconection());
                cmd1.ExecuteNonQuery();
                v.c.dbcon.Close();
                botonactualizar();
            }

        }

        /* Color a Celdas de GridView y Label */
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void labelCerrarHerr_MouseLeave(object sender, EventArgs e)
        {
            labelCerrarHerr.ForeColor = Color.FromArgb(75, 44, 52);
        }

        private void labelCerrarHerr_MouseDown(object sender, MouseEventArgs e)
        {
            labelCerrarHerr.ForeColor = Color.FromArgb(75, 44, 52);
        }

        private void btnadd_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnadd = sender as Button;
            btnadd.BackgroundImage = Properties.Resources.menos_add;
        }

        private void btnadd_MouseLeave(object sender, EventArgs e)
        {
            Button btnadd = sender as Button;
            btnadd.BackgroundImage = Properties.Resources.menos_add;
        }

        private void dataGridViewMantenimiento_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DEL MANTENIMIENTO")
                e.CellStyle.BackColor = (e.Value.ToString() == "EN PROCESO" ? Color.Khaki : e.Value.ToString() == "LIBERADA" ? Color.PaleGreen : Color.LightCoral);
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "TIPO DE FALLO")
                e.CellStyle.BackColor = (e.Value.ToString() == "CORRECTIVO" ? Color.Khaki : e.Value.ToString() == "PREVENTIVO" ? Color.PaleGreen : e.Value.ToString() == "REPROGRAMADO" ? Color.LightBlue : Color.FromArgb(246, 144, 123));
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DE REFACCIONES")
                e.CellStyle.BackColor = (e.Value.ToString() == "SE REQUIEREN REFACCIONES" ? Color.PaleGreen : Color.LightCoral);
            if (this.dgvMantenimiento.Columns[e.ColumnIndex].Name == "EXISTENCIA DE REFACCIONES EN ALMACEN")
                e.CellStyle.BackColor = (e.Value.ToString() == "EXISTENCIA DE REFACCIONES" ? Color.PaleGreen : e.Value.ToString() == "EN ESPERA DE LA REFACCIÓN" ? Color.Khaki : Color.LightCoral);
        }

        private void dataGridViewMRefaccion_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvPMantenimiento.Columns[e.ColumnIndex].Name == "ESTATUS DE LA REFACCION")
                e.CellStyle.BackColor = (e.Value.ToString() == "EXISTENCIA" ? Color.PaleGreen : e.Value.ToString() == "SIN EXISTENCIA" ? Color.LightCoral : Color.FromArgb(255, 144, 51));
            if (this.dgvPMantenimiento.Columns[e.ColumnIndex].Name == "CANTIDAD POR ENTREGAR")
                if (Convert.ToString(e.Value) != "0")
                    e.CellStyle.BackColor = Color.Khaki;
        }

        private void comboBoxEstatusMant_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color c = Color.BlueViolet;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color color = Color.FromArgb(246, 144, 123);
            SolidBrush s = new SolidBrush(color);
            Color fondo = Color.FromArgb(200, 200, 200);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    break;
                case 2:
                    e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    break;
                case 3:
                    e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);
                    break;
            }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                e.Graphics.DrawString(comboBoxEstatusMant.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
                e.Graphics.DrawString(comboBoxEstatusMant.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
        }

        private void comboBoxReqRefacc_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
            {
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 1:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxReqRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 2:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxReqRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                    }
                }
            }
        }

        private void comboBoxExisRefacc_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color fondo = Color.FromArgb(200, 200, 200);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
            {
                if (e.Index == -1)
                    e.Graphics.DrawString("", e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                else
                {
                    switch (e.Index)
                    {
                        case 0:
                            e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(cbx.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 1:
                            e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 2:
                            e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;

                        case 3:
                            e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                            e.Graphics.DrawString(comboBoxExisRefacc.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                            break;
                    }
                }
            }
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

        public void combo_DrawItem(object sender, DrawItemEventArgs e)
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
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }
        public int rowIndex { get; set; }
        bool mensaje = false;

        private void dataGridViewMantenimiento_MouseClick(object sender, MouseEventArgs e)
        {
            banderaeditar = mensaje = false;
            if (peditar)
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip mn = new System.Windows.Forms.ContextMenuStrip();
                    int xy = dgvMantenimiento.HitTest(e.X, e.Y).RowIndex;
                    if (xy >= 0)
                        mn.Items.Add("Editar".ToUpper(), controlFallos.Properties.Resources.pencil).Name = "Editar".ToUpper();
                    mn.Show(dgvMantenimiento, new Point(e.X, e.Y));
                    mn.ItemClicked += new ToolStripItemClickedEventHandler(mn_ItemClicked);
                }
            }
        }

        public void mn_ItemClicked(object sender, ToolStripItemClickedEventArgs e) // Edicion en el gridview de mantenimiento
        {
            mensaje = false;
            estatusmantGV = dgvMantenimiento.CurrentRow.Cells["ESTATUS DEL MANTENIMIENTO"].Value.ToString();
            if (!(estatusmantGV.Equals("")))
            {
                valdeval();
                if (((fgeneralanterior.Equals(validacionfgeneral) || ((valorfallogeneral == 1) || (string.IsNullOrWhiteSpace(fgeneralanterior)))) && (mecanicoanterior.Equals(labelNomMecanico.Text)) && (mecanicoapoanterior.Equals(labelNomMecanicoApo.Text)) && (exisrefaccionanterior.Equals(validacionexisrefacc)) && (reqrefanterior.Equals(validacionreqrefacc)) && (trabrealizadoanterior.Trim().Equals(textBoxTrabajoRealizado.Text.Trim())) && (folfacturanterior.Trim().Equals(textBoxFolioFactura.Text.Trim())) && ((estatusmantanterior.Equals(validacionestatusmant)) || (comboBoxEstatusMant.Text.Equals("EN PROCESO"))) && (supervisoanterior.Equals(labelNomSuperviso.Text)) && (observacionesmantanterior.Trim().Equals(textBoxObsMan.Text.Trim())) && ((inicolumn == 0) || (inicolumn == fincolumn))))
                {
                    switch (e.ClickedItem.Name.ToString())
                    {
                        case "EDITAR":
                            codedicion();
                            Cancelar(false);
                            break;

                        default:
                            MessageBox.Show("DEFAULT");
                            break;
                    }
                }
                else
                {
                    if (MessageBox.Show("Si usted cambia de reporte sin guardar perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        switch (e.ClickedItem.Name.ToString())
                        {
                            case "EDITAR":
                                codedicion();
                                Cancelar(false);
                                break;

                            default:
                                MessageBox.Show("DEFAULT");
                                break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No puede editar un reporte si no se ha guardado por lo menos una vez", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Cancelar(bool Visible)
        {
            btnCancelar.Visible = lblCancelar.Visible = Visible;
        }

        private void textBoxCantidad_TextChanged(object sender, EventArgs e)
        {
            if (banderaeditar)
            {
                if (string.IsNullOrWhiteSpace(textBoxCantidad.Text) || textBoxCantidad.Text == ".")
                    textBoxCantidad.Text = "0";
                if ((idfamilianterior == Convert.ToInt32(comboBoxFamilia.SelectedValue) || (comboBoxFamilia.SelectedIndex == 0)) && (idrefaccionanterior == Convert.ToInt32(comboBoxFRefaccion.SelectedValue) || comboBoxFRefaccion.SelectedIndex == 0) && (cantidadanterior == Convert.ToDouble(textBoxCantidad.Text) || Convert.ToDouble(textBoxCantidad.Text) == 0.0))
                    buttonActualizarPed.Visible = label3.Visible = false;
                else
                    buttonActualizarPed.Visible = label3.Visible = true;
            }
        }

        private void dataGridViewMantenimiento_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                dgvMantenimiento.CurrentCell = dgvMantenimiento.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void groupBoxSupervision_Enter(object sender, EventArgs e)
        {

        }
>>>>>>> 289438355dcf9ce0a48126f327236d2313a9d884
    }
}