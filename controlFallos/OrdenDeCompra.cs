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
    public partial class OrdenDeCompra : Form
    {
        Thread th;
        conexion co = new conexion();

        /* VAR ANTERIORES */

        String estatusOCompra = "", observacionesanterior = "", proveedoranterior = "", facturaranterior = "", observacionesrefaccanterior = "", codigorefanterior = "";
        int idproveedoranterior, idfacturaranterior;
       
        DateTime fentregaestimadanterior;

        /* VARIABLES */

        int idordencompra, idvalidacionproveedor, idvalidacionfacturar, contadorefacc, empresa, area, res, idfolio, numerorefaccion, nrefacc, totalrefacc, idUsuario;
        double sumaCSolicitada, cantidadrefacc, resultado, iva, resultadototal;
        String personafinal = "", acumobservacionesrefacc, observacionesOCompra, refaccinicial, refaccfinal, codigorefaccionvalidada, codigorefaccionvalidada1;
        bool banderaeditar = false;

        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }

        /* VARIABLES PDF */

        String almacenistapdf = "", autorizapdf = "", proveedorpdf = "", facturarpdf = "";

        new menuPrincipal Owner;
        public OrdenDeCompra(int idUsuario, int empresa, int area, Form fh,System.Drawing.Image logo)
        {
             th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            comboBoxProveedor.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxClave.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxFacturar.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxProveedorB.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            comboBoxEmpresaB.MouseWheel += new MouseEventHandler(comboBoxAll_MouseWheel);
            cbcomparativa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            Owner = (menuPrincipal)fh;
            pictureBox1.BackgroundImage = logo;
        }
        public void comprativas()
        {
            v.iniCombos("select upper(nombreComparativa) as n, idcomparativa as id from comparativas where status='3';", cbcomparativa, "id", "n", "--SELECCIONE COMPARATIVA--");
        }
        private void OrdenDeCompra_Load(object sender, EventArgs e)
        {
            textBoxIVA.Enabled = false;
            timer1.Start();
            comprativas();
            actualizarcbx();
            cargafolio();
            metodocargaorden();
            metodocargadetordenPDF();
            conteoiniref();
            dateTimePickerFechaEntrega.Value = DateTime.Now;
            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;
            dateTimePickerIniBFE.Value = DateTime.Now;
            dateTimePickerFinBFE.Value = DateTime.Now;
            metodocargaiva();
            privilegios();

            AutoCompletado(textBoxOCompraB);

            if ((pinsertar == true) && (peditar == true) && (pconsultar == true))
            {
                label60.Visible = true;
                label61.Visible = true;
                buttonActualizarN.Visible = true;
                label49.Visible = true;
            }
            else
            {
                label60.Visible = false;
                label61.Visible = false;
                buttonActualizarN.Visible = false;
                label49.Visible = false;
            }

            if (checkBoxFechas.Checked == false)
            {
                checkBoxFechas.ForeColor = checkBoxFechas.Checked ? Color.Crimson : Color.Crimson;
            }

            if (checkBoxFechasE.Checked == false)
            {
                checkBoxFechasE.ForeColor = checkBoxFechasE.Checked ? Color.Crimson : Color.Crimson;
            }
            if (Convert.ToInt32(v.getaData("SELECT Coalesce(ver,'0') FROM privilegios WHERE namform='catEmpresas' and usuariofkcpersonal='" + idUsuario + "'")) == 1) pEmpresa.Visible = true;
            //comboBoxProveedor.Enabled = false;
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

        public void CargarEmpresas()
        {
            DataTable dt = (DataTable)v.getData("SELECT UPPER(nombreEmpresa) AS nombreEmpresa, idempresa FROM cempresas WHERE status = 1 order by nombreEmpresa");
            DataRow row2 = dt.NewRow();
            row2["idempresa"] = 0;
            row2["nombreEmpresa"] = "-- SELECCIONE UNA EMPRESA --";
            dt.Rows.InsertAt(row2, 0);
            pverEmpresa = v.getBoolFromInt(Convert.ToInt32(v.getaData("SELECT Coalesce(ver,'0') FROM privilegios WHERE namform='catEmpresas' and usuariofkcpersonal='" + idUsuario + "'")));
            if (v.getIntFrombool(pverEmpresa) == 1)
            {
                row2 = dt.NewRow();
                row2["idempresa"] = res = Convert.ToInt32(v.getaData("SELECT coalesce(MAX(idempresa), '0') AS idempresa FROM cempresas WHERE status = 1")) + 1;
                row2["nombreEmpresa"] = "OTRA EMPRESA";
                dt.Rows.InsertAt(row2, dt.Rows.Count);
            }
            comboBoxFacturar.ValueMember = "idempresa";
            comboBoxFacturar.DisplayMember = "nombreEmpresa";
            comboBoxFacturar.DataSource = dt;
            comboBoxFacturar.SelectedIndex = 0;
            co.dbconection().Close();
        }

        public void CargarEmpresasBusqueda()
        {
            v.iniCombos("SELECT DISTINCT UPPER(t2.nombreEmpresa) AS nombreEmpresa, t2.idempresa FROM ordencompra as t1 INNER JOIN cempresas as t2 ON t1.facturadafkcempresas = t2.idempresa GROUP BY FacturadafkCEmpresas ORDER BY nombreEmpresa ASC", comboBoxEmpresaB, "idempresa", "nombreEmpresa", "-- SELECCIONE UNA EMPRESA --");
        }

        public void CargarProveedores()
        {
            DataTable dt = (DataTable)v.getData("SELECT UPPER(empresa) AS empresa,  idproveedor FROM cproveedores WHERE status = 1 order by empresa");
            DataRow row2 = dt.NewRow();
            row2["idproveedor"] = 0;
            row2["empresa"] = "-- SELECCIONE UN PROVEEDOR --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxProveedor.ValueMember = "idproveedor";
            comboBoxProveedor.DisplayMember = "empresa";
            comboBoxProveedor.DataSource = dt;
            comboBoxProveedor.SelectedIndex = 0;
            co.dbconection().Close();
        }

        public void CargarProveedoresBusqueda()
        {
            v.iniCombos("SELECT distinct UPPER(t2.empresa) AS empresa, idproveedor FROM sistrefaccmant.ordencompra AS t1 inner join cproveedores as t2 ON t1.proveedorfkcproveedores=t2.idproveedor GROUP by t1.ProveedorfkCProveedores order by t2.empresa asc;", comboBoxProveedorB, "idproveedor", "empresa", "-- SELECCIONE UN PROVEEDOR --");

        }

        public void limpiarRefacc()
        {
            comboBoxClave.SelectedIndex = 0;

            comboBoxClave.Visible = true;
            labelDescripcion.Text = "";
            labelExistencia.Text = "";
            lblCantidad.Text = "";
            lblPrecio.Text = "";
            labelSubTotal.Text = "0";
            textBoxObservacionesRefacc.Text = "";
        }

        public void limpiarRefaccN()
        {
            limpiar_variables();
            //comboBoxProveedor.SelectedIndex = 0;
            labelCorreo.Text = "";
            labelRepresentante.Text = "";
            cbcomparativa.SelectedIndex = 0;
            //comboBoxClave.SelectedIndex = 0;
            lblPrecio.Text = "";
            lblCantidad.Text = "";
            labelDescripcion.Text = "";
            labelExistencia.Text = "0";
            labelSubTotal.Text = "0";
            comboBoxFacturar.SelectedIndex = 0;
            textBoxObservaciones.Text = "";
            textBoxObservacionesRefacc.Text = "";
            dateTimePickerFechaEntrega.Value = DateTime.Now;
            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;
            dateTimePickerIniBFE.Value = DateTime.Now;
            dateTimePickerFinBFE.Value = DateTime.Now;
            textBoxIVA.Enabled = false;
        }

        public void limpiarRefaccB()
        {
            textBoxOCompraB.Text = "";
            comboBoxProveedorB.SelectedIndex = 0;
            comboBoxEmpresaB.SelectedIndex = 0;
            dateTimePickerIni.Value = DateTime.Now;
            dateTimePickerFin.Value = DateTime.Now;
            dateTimePickerIniBFE.Value = DateTime.Now;
            dateTimePickerFinBFE.Value = DateTime.Now;
        }

        bool pverEmpresa;

        /* Todos los métodos */
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void validacioneditar()
        {
            if (banderaeditar == true)
            {
                if ((comboBoxClave.Text == codigorefanterior) && textBoxObservacionesRefacc.Text.Trim() == observacionesrefaccanterior)
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        public void actualizarcbx()
        {
            //CargarClave();
            if (dataGridViewPedOCompra.Rows.Count == 0)
            {
                CargarEmpresas();
                //CargarProveedores();
            }
            CargarEmpresasBusqueda();
            CargarProveedoresBusqueda();
        }

        public void mostrarexcel()
        {
            privilegios();
            if (pconsultar == true && pinsertar == true && peditar == true)
            {
                buttonExcel.Visible = true;
                label38.Visible = true;
            }
            else
            {
                buttonExcel.Visible = false;
                label38.Visible = false;
            }
        }

        public void ocultarexcel()
        {
            if (buttonExcel.Visible == true)
            {
                buttonExcel.Visible = false;
                label38.Visible = false;
            }
        }

        public void validacionval()
        {
            if (comboBoxProveedor.SelectedIndex == 0)
            {
                idvalidacionproveedor = 0;
            }
            else
            {
                idvalidacionproveedor = Convert.ToInt32(comboBoxProveedor.SelectedValue);
            }
            if (comboBoxFacturar.SelectedIndex == 0)
            {
                idvalidacionfacturar = 0;
            }
            else
            {
                idvalidacionfacturar = Convert.ToInt32(comboBoxFacturar.SelectedValue);
            }
        }

        public void btnnuevoOC()
        {
            limpiarRefaccN();
            cargafolio();
            conteoiniref();
            limpia_var();
            cbcomparativa.SelectedIndex = 0;
            cbcomparativa.Enabled = true;
            comboBoxProveedor.Enabled = false;
            comboBoxClave.Visible = true;
            comboBoxFacturar.Enabled = true;
            textBoxObservaciones.Enabled = true;
            buttonNuevoOC.Visible = false;
            label17.Visible = false;
            buttonPDF.Visible = false;
            label3.Visible = false;
            buttonEditar.Visible = false;
            label8.Visible = false;
            buttonAgregarMas.Visible = false;
            label29.Visible = false;
            buttonAgregar.Visible = true;
            label9.Visible = true;
            textBoxObservaciones.Visible = true;
            buttonFinalizar.Visible = false;
            label34.Visible = false;
            dataGridViewPedOCompra.Enabled = false;
            Limpiar_labels();
            comboBoxFacturar.SelectedIndex = 0;
            dataGridViewOCompra.ClearSelection();
            Limpia_variables();
            actualizarcbx();
            dateTimePickerFechaEntrega.Enabled = true;
            textBoxObservacionesRefacc.Enabled = true;
            metodocargaiva();
            labelSubTotalOC.Visible = true;
            labelIVAOC.Visible = true;
            labelTotalOC.Visible = true;
            labelSubTotal.Visible = true;
            dataGridViewPedOCompra.DataSource = null;
            metodocargadetordenPDF();
        }

        private void btneditar()
        {
            limpiarRefaccN();
            cargafolio();
            conteoiniref();
            metodocargaorden();
            metodocargadetordenPDF();
            cbcomparativa.SelectedIndex = 0;
            comboBoxClave.Visible = true;
            comboBoxFacturar.Enabled = true;
            textBoxObservaciones.Enabled = true;
            buttonNuevoOC.Visible = false;
            label17.Visible = false;
            buttonPDF.Visible = false;
            label3.Visible = false;
            buttonEditar.Visible = false;
            label8.Visible = false;
            buttonExcel.Visible = false;
            label38.Visible = false;
            buttonAgregarMas.Visible = false;
            label29.Visible = false;
            buttonAgregar.Visible = true;
            label9.Visible = true;
            buttonActualizar.Visible = false;
            label37.Visible = false;
            textBoxObservaciones.Visible = true;
            buttonFinalizar.Visible = false;
            label34.Visible = false;
            dataGridViewPedOCompra.Enabled = false;
            Limpiar_labels();
            comboBoxFacturar.SelectedIndex = 0;
            dataGridViewOCompra.ClearSelection();
            Limpia_variables();
            dateTimePickerFechaEntrega.Enabled = true;
            textBoxObservacionesRefacc.Enabled = true;
            labelSubTotalOC.Visible = true;
            labelIVAOC.Visible = true;
            labelTotalOC.Visible = true;
            banderaeditar = false;
        }

        public void txtchelse()
        {
            labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
            labelSubTotalOC.Text = sumaCSolicitada.ToString("00.00", CultureInfo.InvariantCulture);
            iva = sumaCSolicitada * (Convert.ToDouble(textBoxIVA.Text) / 100);
            labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
            resultadototal = sumaCSolicitada + iva;
            labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
        }

        public void editargroup()
        {
            metodocargadetordenPDF();
            buttonEditar.Visible = false;
            label8.Visible = false;
            buttonAgregarMas.Visible = false;
            label29.Visible = false;
            buttonPDF.Visible = false;
            label3.Visible = false;
            buttonNuevoOC.Visible = true;
            label17.Visible = true;
            buttonFinalizar.Visible = false;
            label34.Visible = false;
            buttonAgregar.Visible = false;
            label9.Visible = false;
            comboBoxProveedor.Enabled = false;
            comboBoxFacturar.Enabled = false;
            comboBoxClave.Visible = true;
            label1.Text = "CÓDIGO DE REFACCIÓN";
            comboBoxClave.Enabled = false;
            dateTimePickerFechaEntrega.Enabled = false;
            textBoxIVA.Enabled = false;
            textBoxObservaciones.Enabled = true;
            textBoxObservacionesRefacc.Enabled = false;
            dataGridViewPedOCompra.Enabled = false;
            labelFolioOC.Text = dataGridViewOCompra.CurrentRow.Cells["ORDEN DE COMPRA"].Value.ToString();
            metodorecid();
            personafinal = dataGridViewOCompra.CurrentRow.Cells["PERSONA FINAL"].Value.ToString();
            estatusOCompra = dataGridViewOCompra.CurrentRow.Cells["ESTATUS"].Value.ToString();

            MySqlCommand cmd = new MySqlCommand("SELECT t1.idOrdCompra, t1.FechaEntregaOCompra, COALESCE(t1.Subtotal, '0') AS Subtotal, COALESCE(t1.IVA, '0') AS IVA, COALESCE(t1.Total, '0') AS Total, COALESCE(UPPER(t1.ObservacionesOC), '') AS Observaciones, UPPER(t2.nombreEmpresa) AS NEmpresa, t1.FacturadafkCEmpresas AS EFacturar, UPPER(t3.empresa) AS Proveedor, t1.ProveedorfkCProveedores AS NProveedores, COALESCE(SUM(t4.Total), '0') AS TotalCS FROM ordencompra AS t1 INNER JOIN cempresas AS t2 ON t1.FacturadafkCEmpresas = t2.idempresa INNER JOIN cproveedores AS t3 ON t1.ProveedorfkCProveedores = t3.idproveedor INNER JOIN detallesordencompra AS t4 ON t1.idOrdCompra = t4.OrdfkOrdenCompra WHERE t1.FolioOrdCompra = '" + labelFolioOC.Text + "' and t1.empresa='"+empresa+"'", co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                idordencompra = Convert.ToInt32(dr.GetString("idOrdCompra"));
                cbcomparativa.SelectedValue = Convert.ToInt32(v.getaData("select t1.idcomparativa as id from comparativas as t1 inner join ordencompra as t2 on t2.ComparativaFKComparativas=t1.idcomparativa where t2.idOrdCompra='" + idordencompra + "';").ToString());
                dateTimePickerFechaEntrega.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                fentregaestimadanterior = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                labelSubTotalOC.Text = dr.GetString("Subtotal");
                if (personafinal.Equals(""))
                {
                    metodocargaiva();
                }
                else
                {
                    textBoxIVA.Text = dr.GetString("IVA");
                }
                labelTotalOC.Text = dr.GetString("Total");
                labelIVAOC.Text = (Math.Truncate((dr.GetDouble("Subtotal")*(dr.GetDouble("IVA")/100))*100) / 100).ToString("N2");
                textBoxObservaciones.Text = dr.GetString("Observaciones");
                observacionesanterior = dr.GetString("Observaciones");
                comboBoxProveedor.Text = dr.GetString("Proveedor");
                proveedoranterior = dr.GetString("Proveedor");
                idproveedoranterior = Convert.ToInt32(dr.GetString("NProveedores"));
                comboBoxFacturar.Text = dr.GetString("NEmpresa");
                facturaranterior = dr.GetString("NEmpresa");
                idfacturaranterior = Convert.ToInt32(dr.GetString("EFacturar"));
                sumaCSolicitada = Convert.ToDouble(dr.GetString("TotalCS"));
            }

            if (estatusOCompra == "FINALIZADA")
            {
                cbcomparativa.Enabled = comboBoxClave.Enabled = comboBoxProveedor.Enabled = false;
                if (!(string.IsNullOrWhiteSpace(comboBoxFacturar.Text)))
                {
                    comboBoxFacturar.Enabled = true;
                }
                if (!(string.IsNullOrWhiteSpace(dateTimePickerFechaEntrega.Text)))
                {
                    dateTimePickerFechaEntrega.Enabled = true;
                }
                if (!(string.IsNullOrWhiteSpace(textBoxObservaciones.Text)))
                {
                    textBoxObservaciones.Enabled = true;
                }
                else
                {
                    textBoxObservaciones.Enabled = true;
                }
                labelSubTotalOC.Visible = labelIVAOC.Visible =labelTotalOC.Visible = labelSubTotal.Visible = true;
            }
            else
            {
                if (comboBoxFacturar.SelectedIndex > 0)
                {
                    comboBoxFacturar.Enabled = true;
                }
                if (!(string.IsNullOrWhiteSpace(dateTimePickerFechaEntrega.Text)))
                {
                    dateTimePickerFechaEntrega.Enabled = true;
                }
                if (!(string.IsNullOrWhiteSpace(textBoxObservaciones.Text)))
                {
                    textBoxObservaciones.Enabled = true;
                }
                cbcomparativa.Enabled = false;
                comboBoxClave.Enabled = false;
                comboBoxProveedor.Enabled = false;
                buttonAgregar.Visible = false;
                label9.Visible = false;
                buttonActualizar.Visible = false;
                label37.Visible = false;
                buttonPDF.Visible = false;
                label3.Visible = false;
                labelSubTotalOC.Visible = false;
                labelIVAOC.Visible = false;
                labelTotalOC.Visible = false;
                labelSubTotal.Visible = false;
            }
            dr.Close();
            co.dbconection().Close();
            banderaeditar = true;
        }

        public void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor, Form f)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);
                g.Clear(f.BackColor);
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        public void textBox_TextChanged(object sender, EventArgs e)
        {
            if (banderaeditar == true)
            {
                validacionval();
                if (((idfacturaranterior == idvalidacionfacturar) || (comboBoxFacturar.SelectedIndex == 0)) && ((observacionesanterior == textBoxObservaciones.Text.Trim())) && (dateTimePickerFechaEntrega.Value.Date == fentregaestimadanterior))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        public void exportacionexcel()
        {
            int contador = 0;
            string Folio, id;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Orden De Compra','0','";
            foreach (DataRow rowexcel in dtexcel.Rows)
            {
                contador++;
                id = rowexcel[0].ToString();
                Folio = v.getaData("SELECT coalesce((t1.idOrdCompra), '') AS idOrdCompra FROM ordencompra AS t1 WHERE '" + id + "' = t1.FolioOrdCompra").ToString();
                //SELECT coalesce((t1.idOrdCompra), '') AS idOrdCompra FROM ordencompra AS t1 WHERE 'OC-0000006' = t1.FolioOrdCompra
                if (contador < dtexcel.Rows.Count)
                {
                    Folio += ";";
                }
                sql += Folio;
            }
            sql += "','" + idUsuario + "',now(),'Exportación a Excel de ordenes de compra','2','2')";
            MySqlCommand exportacion = new MySqlCommand(sql, co.dbconection());
            exportacion.ExecuteNonQuery();
            co.dbconection().Close();
            //dtexcel.Reset();
        }

        public void exportacionpdf()
        {
            metodorecid();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('Orden De Compra', '" + idfolio + "', 'Exportación de orden de compra en archivo pdf', '" + idUsuario + "', NOW(), 'Exportación a PDF de orden de compra de almacen', '2', '2')", co.dbconection());
            cmd.ExecuteNonQuery();
            co.dbconection().Close();
        }

        public void privilegios()
        {
            string sql = "SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar) as privilegios FROM privilegios where usuariofkcpersonal = '" + idUsuario + "' and namform = 'ordencompra'";
            string[] privilegios = v.getaData(sql).ToString().Split(';');
            pinsertar = getBoolFromInt(Convert.ToInt32(privilegios[0]));
            pconsultar = getBoolFromInt(Convert.ToInt32(privilegios[1]));
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[2]));
            pdesactivar = getBoolFromInt(Convert.ToInt32(privilegios[3]));
        }

        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }

        public void AutoCompletado(TextBox cajaTexto) //Metodo De AutoCompletado
        {
            AutoCompleteStringCollection nColl = new AutoCompleteStringCollection();
            MySqlCommand cmd = new MySqlCommand("SELECT FolioOrdCompra AS Folio FROM ordencompra", co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    nColl.Add(dr["Folio"].ToString());
                }
            }
            dr.Close();
            co.dbconection().Close();
            textBoxOCompraB.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxOCompraB.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxOCompraB.AutoCompleteCustomSource = nColl;
        }

        public void cargafolio()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(SUBSTRING(FolioOrdCompra, LENGTH(FolioOrdCompra) -6, 7)+1) AS FolioOC FROM ordencompra WHERE idOrdCompra = (SELECT MAX(idOrdCompra) FROM ordencompra)", co.dbconection());
            string Folio = (string)cmd.ExecuteScalar();
            if (Folio == null)
            {
                Folio = "0000001";
            }
            else
            {
                while (Folio.Length < 7)
                {
                    Folio = "0" + Folio;
                }
            }
            labelFolioOC.Text = "OC-" + Folio.ToString();
            co.dbconection().Close();
        }

        validaciones v = new validaciones();

        public void metodorecid()
        {
            try
            {
                string res = v.getaData("SELECT idOrdCompra FROM ordencompra WHERE FolioOrdCompra = '" + labelFolioOC.Text + "'").ToString() ?? "";
                idfolio = Convert.ToInt32(res);
            }
            catch
            {
                idfolio = 0;
            }
        }

        public void metodocargaiva()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(iva,'0') AS IVA FROM civa", co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBoxIVA.Text = Convert.ToString(dr.GetString("iva"));
            }
            else
            {
                textBoxIVA.Text = "0";
            }
            dr.Close();
            co.dbconection().Close();
        }

        public void metodocargadetordenPDF() //Metodo Para Cargar Los Datos De Las Refacciones
        {
            try
            {
                DataTable dt = (DataTable)v.getData("SET NAMES 'utf8';SELECT t1.NumRefacc AS PARTIDA,(select if(a5.refaccionfkcrefacciones is null,'',(select a8.codrefaccion from crefacciones as a8 where a8.idrefaccion=a5.refaccionfkcrefacciones)) from refaccionescomparativa as a5 inner join proveedorescomparativa as a6 on a5.idrefaccioncomparativa=a6.refaccionfkrefaccionesComparativa inner join comparativas as a7 on a7.idcomparativa=a5.ComparativaFKComparativas where a6.idproveedorComparativa=t1.ClavefkCRefacciones)as CLAVE,(select if(a1.refaccionfkcrefacciones is null,a1.nombreRefaccion,(select upper(a4.nombreRefaccion) from crefacciones as a4 where a4.idrefaccion=a1.refaccionfkcrefacciones)) from refaccionescomparativa as a1 inner join proveedorescomparativa as a2 on a1.idrefaccioncomparativa=a2.refaccionfkrefaccionesComparativa inner join comparativas as a3 on a3.idcomparativa=a1.comparativafkcomparativas where a2.idproveedorComparativa=t1.ClavefkCRefacciones)as 'DESCRIPCIÓN',COALESCE((SELECT x3.existencias FROM crefacciones AS x3 inner join refaccionescomparativa as x4 on x3.idrefaccion=x4.refaccionfkcrefacciones inner join comparativas as x5 on x5.idcomparativa=x4.comparativafkcomparativas inner join proveedorescomparativa as x6 on x6.refaccionfkrefaccionesComparativa=x4.idrefaccioncomparativa where x6.idproveedorComparativa=t1.ClavefkCRefacciones), 0) AS 'CANTIDAD EN EXISTENCIA', t1.Cantidad AS 'CANTIDAD SOLICITADA', t1.Precio AS 'PRECIO COTIZADO', (t1.cantidad*t1.precio)AS TOTAL, t1.ObservacionesRefacc AS OBSERVACIONES FROM detallesordencompra AS t1 INNER JOIN ordencompra AS t2 ON t1.OrdfkOrdenCompra = t2.idOrdCompra WHERE t1.OrdfkOrdenCompra = '" + idfolio + "'");

                dataGridViewPedOCompra.DataSource = dt;
                co.dbconection().Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void metodocargaorden()
        {
            DataTable dt = (DataTable)v.getData("SET NAMES 'utf8';SET lc_time_names = 'es_ES'; SELECT t1.FolioOrdCompra AS 'ORDEN DE COMPRA', UPPER(t2.empresa) AS PROVEEDOR, UPPER(t3.nombreEmpresa) AS 'NOMBRE DE LA EMPRESA', UPPER(DATE_FORMAT(t1.FechaOCompra, '%W %d %M %Y')) AS 'FECHA', UPPER(DATE_FORMAT(t1.FechaEntregaOCompra, '%W %d %M %Y')) AS 'FECHA DE ENTREGA', t1.SUBTOTAL, cast(((t1.IVA/100)*t1.Subtotal) as decimal (18,2)) as IVA, t1.TOTAL, coalesce((UPPER(t1.ESTATUS)), '') AS ESTATUS, coalesce((SELECT UPPER(CONCAT(t4.ApPaterno, ' ', t4.ApMaterno, ' ', t4.nombres)) FROM cpersonal AS t4 WHERE t1.PersonaFinal = t4.idPersona), '') AS 'PERSONA FINAL', UPPER(t1.ObservacionesOC) AS 'OBSERVACIONES' FROM ordencompra AS t1 LEFT JOIN cproveedores AS t2 ON t1.ProveedorfkCProveedores = t2.idproveedor INNER JOIN cempresas AS t3 ON t1.FacturadafkCEmpresas = t3.idempresa where t1.empresa='" + empresa +"'ORDER BY t1.FolioOrdCompra DESC");
            dataGridViewOCompra.DataSource = dt;
            co.dbconection().Close();
        }

        public void conteoiniref() //Realiza Un Conteo Inicial Para Saber Si No Hubo Algun Cambio En El GridView
        {
            string res = v.getaData("SELECT COUNT(NumRefacc) AS Numero FROM detallesordencompra").ToString();
            if (res != null)
            {
                refaccinicial = res;
            }
        }

        public void conteofinref() //Realiza Un Conteo Final Para Saber Si No Hubo Algun Cambio En El GridView
        {
            string res = v.getaData("SELECT COUNT(NumRefacc) AS Numero FROM detallesordencompra").ToString();

            if (res != null)
            {
                refaccfinal = res;
            }
        }

        public void CargarClave()
        {
            DataTable dt = (DataTable)v.getData("SELECT UPPER(CONCAT(nombreRefaccion, ' - ', modeloRefaccion)) AS codrefaccion, idrefaccion FROM crefacciones WHERE status = 1 ORDER BY codrefaccion");
            DataRow row2 = dt.NewRow();
            row2["idrefaccion"] = 0;
            row2["codrefaccion"] = "-- SELECCIONE UNA OPCIÓN --";
            dt.Rows.InsertAt(row2, 0);
            comboBoxClave.ValueMember = "idrefaccion";
            comboBoxClave.DisplayMember = "codrefaccion";
            comboBoxClave.DataSource = dt;
            comboBoxClave.SelectedIndex = 0;
        }

        public bool metodotxtpedcompra() // checar
        {
            if (dataGridViewPedOCompra.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        void limpia_var()
        {
            idproveedoranterior = 0;
            idfacturaranterior = 0;
            observacionesanterior = "";
            idfolio = 0;
        }
        int _idcomp;
        public void llamarorden()
        {
            privilegios();
            limpia_var();
            buttonEditar.Visible = false;
            label8.Visible = false;
            buttonAgregarMas.Visible = false;
            label29.Visible = false;
            buttonPDF.Visible = true;
            label3.Visible = true;
            label37.Visible = false;
            buttonNuevoOC.Visible = true;
            label17.Visible = true;
            buttonFinalizar.Visible = false;
            label34.Visible = false;
            labelFolioOC.Text = dataGridViewOCompra.CurrentRow.Cells["ORDEN DE COMPRA"].Value.ToString();
            _idcomp = Convert.ToInt32(v.getaData("select t1.idcomparativa from comparativas as t1 inner join ordencompra as t2 on t1.idcomparativa=t2.ComparativaFKComparativas where t2.FolioOrdCompra='" + dataGridViewOCompra.CurrentRow.Cells["ORDEN DE COMPRA"].Value + "';").ToString());
            metodorecid();
            personafinal = dataGridViewOCompra.CurrentRow.Cells["PERSONA FINAL"].Value.ToString();
            estatusOCompra = dataGridViewOCompra.CurrentRow.Cells["ESTATUS"].Value.ToString(); if (string.IsNullOrWhiteSpace(estatusOCompra)) label3.Text = "VISUALIZAR ORDEN" + Environment.NewLine + " DE COMPRA"; else label3.Text = "GENERAR ORDEN" + Environment.NewLine + " DE COMPRA";
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idOrdCompra,t1.FechaEntregaOCompra as FechaEntregaOCompra, coalesce((t1.Subtotal), '0') AS Subtotal, coalesce((t1.IVA), '0') AS IVA, coalesce((t1.Total), '0') AS Total, coalesce(upper((t1.ObservacionesOC)), '') AS Observaciones, UPPER(t2.nombreEmpresa) AS NEmpresa, t1.FacturadafkCEmpresas AS EFacturar, UPPER(t3.empresa) AS Proveedor, t1.ProveedorfkCProveedores AS NProveedores,(select sum(x1.Cantidad * x1.Precio) from detallesordencompra as x1 where x1.OrdfkOrdenCompra=t1.idOrdCompra)AS TotalCS from ordencompra AS t1 INNER JOIN cempresas AS t2 ON t1.FacturadafkCEmpresas = t2.idempresa INNER JOIN cproveedores AS t3 ON t1.ProveedorfkCProveedores = t3.idproveedor  WHERE t1.FolioOrdCompra =  '" + labelFolioOC.Text + "'", co.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                idordencompra = Convert.ToInt32(dr.GetString("idOrdCompra"));
                dateTimePickerFechaEntrega.Value = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                fentregaestimadanterior = Convert.ToDateTime(dr.GetString("FechaEntregaOCompra"));
                labelSubTotalOC.Text = dr.GetString("Subtotal");
                if (personafinal.Equals(""))
                {
                    metodocargaiva();
                }
                else
                {
                    textBoxIVA.Text = dr.GetString("IVA");
                }
                labelTotalOC.Text = dr.GetString("Total");
                textBoxObservaciones.Text = dr.GetString("Observaciones");
                observacionesanterior = dr.GetString("Observaciones");
                cbcomparativa.SelectedValue = _idcomp;
                comboBoxProveedor.Text = dr.GetString("Proveedor");
                proveedoranterior = dr.GetString("Proveedor");
                idproveedoranterior = Convert.ToInt32(dr.GetString("NProveedores"));
                comboBoxFacturar.Text = dr.GetString("NEmpresa");
                facturaranterior = dr.GetString("NEmpresa");
                idfacturaranterior = Convert.ToInt32(dr.GetString("EFacturar"));
                sumaCSolicitada = Convert.ToDouble(dr.GetString("TotalCS"));
            }
            dr.Close();
            co.dbconection().Close();
            MySqlCommand cmd1 = new MySqlCommand("select UPPER (t1.nombreEmpresa) as nombreEmpresa, t1.idempresa from cempresas as t1 inner join ordencompra as t2 on t1.idempresa=t2.FacturadafkCEmpresas where t2.FolioOrdCompra='" + dataGridViewOCompra.CurrentRow.Cells[0].Value.ToString() + "' and t1.status='0'", co.dbconection());
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            if (dr1.Read())
            {
                comboBoxFacturar.DataSource = null;
                MySqlCommand comando = new MySqlCommand("SELECT UPPER(nombreEmpresa) AS nombreEmpresa, idempresa FROM cempresas where status='1' order by nombreEmpresa", co.dbconection());
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow row2 = dt.NewRow();
                DataRow row = dt.NewRow();
                row["idempresa"] = 0;
                row["nombreEmpresa"] = "-- EMPRESA --";
                row2["idempresa"] = dr1["idempresa"];
                row2["nombreEmpresa"] = dr1["nombreEmpresa"].ToString();
                dt.Rows.InsertAt(row2, 1);
                dt.Rows.InsertAt(row, 0);
                comboBoxFacturar.ValueMember = "idempresa";
                comboBoxFacturar.DisplayMember = "nombreEmpresa";
                comboBoxFacturar.DataSource = dt;
                comboBoxFacturar.SelectedIndex = 1;
                comboBoxFacturar.Text = dr1["nombreEmpresa"].ToString();
            }
            dr1.Close();
            co.dbconection().Close();
            MySqlCommand proveedor = new MySqlCommand("SELECT UPPER(empresa) AS empresa,  idproveedor FROM cproveedores WHERE status = 0 and upper(concat(aPaterno,' ',aMaterno,' ',nombres))='" + dataGridViewOCompra.CurrentRow.Cells[1].Value.ToString() + "'", co.dbconection());
            MySqlDataReader dr2 = proveedor.ExecuteReader();
            if (dr2.Read())
            {
                comboBoxProveedor.DataSource = null;
                DataTable dt = (DataTable)v.getData("SELECT UPPER(empresa) AS empresa,  idproveedor FROM cproveedores WHERE status = 1 order by empresa");
                DataRow row2 = dt.NewRow();
                DataRow row3 = dt.NewRow();
                row2["idproveedor"] = 0;
                row2["empresa"] = " -- PROVEEDOR --";
                row3["idproveedor"] = dr2["idproveedor"];
                row3["empresa"] = dr2["empresa"].ToString();
                dt.Rows.InsertAt(row3, 1);
                dt.Rows.InsertAt(row2, 0);
                comboBoxProveedor.ValueMember = "idproveedor";
                comboBoxProveedor.DisplayMember = "empresa";
                comboBoxProveedor.DataSource = dt;
                comboBoxProveedor.SelectedIndex = 1;
                comboBoxProveedor.Text = dr2["empresa"].ToString();
                co.dbconection().Close();
            }
            dr2.Close();
            co.dbconection().Close();
            if (personafinal != "")
            {
                buttonFinalizar.Visible = false;
                label34.Visible = false;
                buttonAgregar.Visible = false;
                label9.Visible = false;
                comboBoxClave.Enabled = false;
                dateTimePickerFechaEntrega.Enabled = false;
                if (pconsultar == true)
                {
                    buttonPDF.Visible = true;
                    label3.Visible = true;
                }
                else
                {
                    buttonPDF.Visible = false;
                    label3.Visible = false;
                }
            }
            else
            {
                buttonFinalizar.Visible = true;
                label34.Visible = true;
                buttonAgregar.Visible = true;
                label9.Visible = true;
                comboBoxClave.Enabled = true;
                if (pconsultar == true)
                {
                    buttonPDF.Visible = true;
                    label3.Visible = true;
                }
                else
                {
                    buttonPDF.Visible = false;
                    label3.Visible = false;
                }
                dateTimePickerFechaEntrega.Enabled = true;
            }
            metodorecid();
            metodocargadetordenPDF();
            labelSubTotal.Text = resultado.ToString("00.00", CultureInfo.InvariantCulture);
            labelSubTotalOC.Text = sumaCSolicitada.ToString("00.00", CultureInfo.InvariantCulture);
            iva = sumaCSolicitada * (Convert.ToDouble(textBoxIVA.Text) / 100);
            labelIVAOC.Text = iva.ToString("00.00", CultureInfo.InvariantCulture);
            resultadototal = sumaCSolicitada + iva;
            labelTotalOC.Text = resultadototal.ToString("00.00", CultureInfo.InvariantCulture);
        }

        Thread hiloEx2;
        public void carga1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            pictureBoxExcelLoad.Visible = true;
            buttonExcel.Visible = false;
            label38.Location = new Point(1011, 576);
            label38.Text = "EXPORTANDO";
        }

        delegate void Loading1();
        public void carga2()
        {
            pictureBoxExcelLoad.Image = null;
            pictureBoxExcelLoad.Visible = false;
            buttonExcel.Visible = true;
            label38.Location = new Point(1024, 576);
            label38.Text = "EXPORTAR";
            if (activo)
            {
                buttonExcel.Visible = false;
                label38.Visible = false;
            }
            activo = false;
            exportando = false;
        }

        delegate void Loading();
        DataTable dtexcel = new DataTable();

        public void exporta_a_excel() //Metodo Que Genera El Excel
        {
            dtexcel = (DataTable)dataGridViewOCompra.DataSource;
            if (dtexcel.Rows.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    Loading load = new Loading(carga1);
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
                            if (j == 5 || j == 6 || j == 7)
                            {
                                rng.NumberFormat = "0.00";
                                sheet.Cells[i + 2, j + 1] = dtexcel.Rows[i][j].ToString();
                            }
                            else
                            {
                                sheet.Cells[i + 2, j + 1] = dtexcel.Rows[i][j].ToString();
                            }
                            rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230));
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Font.FontStyle = "Calibri";
                            rng.Font.Size = 11;
                            if (dtexcel.Rows[i][j].ToString() == "FINALIZADA".ToString())
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                        }
                        catch (Exception)
                        {
                            hiloEx2.Abort();
                        }
                    }
                }
                Thread.Sleep(10);
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
            //    exportacionexcel();
                if (this.InvokeRequired)
                {
                    Loading1 load1 = new Loading1(carga2);
                    this.Invoke(load1);
                }
            }
            else
            {
                MessageBox.Show("Es necesario que existan datos en la tabla para poder generar un archivo de excel \n Favor de actualizar la tabla para que existan reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            hiloEx2.Abort();
        }

        public void To_pdf()
        {
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(21f, 21f, 31f, 31f);
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 95;
            table.LockedWidth = true;
            float[] widths = new float[] { .8f, .8f, .8f, .8f, .8f, .8f, .8f, .8f };
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\Desktop";
            saveFileDialog1.Title = "Guardar reporte";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            escribirFichero("");
            string filename = Application.StartupPath + "/PDFTempral/Orden_" + labelFolioOC.Text + DateTime.Today.ToLongDateString() + ".pdf";
            DialogResult ews = DialogResult.OK;
            try
            {
                if (!string.IsNullOrWhiteSpace(estatusOCompra))
                {
                    if ((ews = saveFileDialog1.ShowDialog()) == DialogResult.OK)
                    {
                        filename = saveFileDialog1.FileName;
                        string p = Path.GetExtension(filename);
                        if (p.ToLower() != ".pdf")
                        {
                            filename = filename + ".pdf";
                        }
                    }
                }
                if (ews == DialogResult.OK)
                {
                    if (filename.Trim() != "")
                    {
                        FileStream file = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                        PdfWriter writer = PdfWriter.GetInstance(doc, file);
                        doc.Open();
                        Chunk chunk = new Chunk("REPORTE DE MANTENIMIENTO", FontFactory.GetFont("ARIAL", 20, iTextSharp.text.Font.BOLD));
                        var res = v.getaData("SELECT COALESCE(logo,'') FROM cempresas WHERE idempresa='" + comboBoxFacturar.SelectedValue + "'").ToString();
                        byte[] img=null;
                        if (res == "")
                        {
                            if(empresa==2)
                            img = Convert.FromBase64String(v.tri);
                            else if(empresa==3)
                                img = Convert.FromBase64String(v.TSD);
                        }
                        else
                        {
                            System.Drawing.Image temp = v.StringToImage2(res);
                            temp = v.CambiarTamanoImagen(temp, 622, 261);
                            img = Convert.FromBase64String(v.SerializarImg(temp));
                        }
                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);

                        byte[] img1 = Convert.FromBase64String(v.autobuses);
                        iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(img1);
                        //byte[] imagen3 = Convert.FromBase64String(img2);

                        imagen.ScalePercent(10f);
                        imagen2.ScalePercent(10f);
                        imagen.SetAbsolutePosition(65f, 707f/*600f, 500f*/);
                        imagen2.SetAbsolutePosition(360f, 704f);
                        imagen.Alignment = Element.ALIGN_CENTER;
                        imagen2.Alignment = Element.ALIGN_CENTER;
                        float percentage = 0.0f;
                        float percentage1 = 0.0f;
                        percentage = 150 / imagen.Width;

                        imagen.ScalePercent(percentage * 80);
                        imagen2.ScalePercent(percentage1 * 50);
                        imagen2.ScaleAbsolute(190f, 53.5f);
                        doc.Add(imagen);
                        doc.Add(imagen2);
                        PdfPTable tb0 = new PdfPTable(1);
                        tb0.DefaultCell.Border = 1;
                        tb0.WidthPercentage = 95;
                        tb0.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell c01 = new PdfPCell();
                        c01.HorizontalAlignment = Element.ALIGN_CENTER;
                        c01.Border = 5;
                        c01.BorderColorLeft = BaseColor.BLACK;
                        c01.BorderColorTop = BaseColor.BLACK;
                        c01.BorderColorBottom = BaseColor.BLACK;
                        c01.BorderColorRight = BaseColor.BLACK;
                        c01.BorderWidthLeft = 2f;
                        c01.BorderWidthRight = 2f;
                        c01.BorderWidthTop = 2f;
                        c01.BorderWidthBottom = 2f;
                        Phrase Espacio = new Phrase("\n\n\n");
                        c01.AddElement(Espacio);
                        tb0.AddCell(c01);
                        doc.Add(tb0);
                        doc.Add(new Paragraph("                                                       ORDEN DE COMPRA                                                        ", FontFactory.GetFont("ARIAL", 13, iTextSharp.text.Font.BOLD)));
                        PdfPTable tb2 = new PdfPTable(16);
                        tb2.WidthPercentage = 95;
                        tb2.HorizontalAlignment = Element.ALIGN_CENTER;
                        PdfPCell c00 = new PdfPCell(new Phrase(""));
                        c00.UseAscender = true;
                        c00.Border = 2;
                        c00.BorderColorTop = BaseColor.WHITE;
                        c00.BorderColorLeft = BaseColor.WHITE;
                        c00.BorderColorRight = BaseColor.WHITE;
                        c00.BorderColorBottom = BaseColor.WHITE;
                        c00.HorizontalAlignment = Element.ALIGN_CENTER;
                        c00.Colspan = 16;
                        tb2.AddCell(c00);
                        PdfPCell c11 = new PdfPCell(new Phrase("Carretera Federal México-Pachuca Km. 26.5 Lte. 2 Col. Venta de Carpio, Ecatepec, Estado De México CP: 55060", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                        c11.UseAscender = false;
                        c11.UseDescender = true;
                        c11.HorizontalAlignment = Element.ALIGN_CENTER;
                        c11.VerticalAlignment = Element.ALIGN_CENTER;
                        c11.Rowspan = 2;
                        c11.Colspan = 8;
                        tb2.AddCell(c11);
                        PdfPCell c12 = new PdfPCell(new Phrase(labelFolioOC.Text, FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD)));
                        c12.UseAscender = true;
                        c12.UseDescender = false;
                        c12.HorizontalAlignment = Element.ALIGN_CENTER;
                        c12.Colspan = 8;
                        tb2.AddCell(c12);
                        PdfPCell c13 = new PdfPCell(new Phrase("Fecha Elaborada:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                        c13.UseAscender = true;
                        c13.UseDescender = false;
                        c13.Colspan = 4;
                        c13.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c13);

                        PdfPCell c14 = new PdfPCell(new Phrase((DateTime.Today.ToLongDateString() + ", " + DateTime.Now.ToString("t", DateTimeFormatInfo.InvariantInfo)).ToUpper(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                        c14.Colspan = 4;
                        c14.UseAscender = true;
                        c14.UseDescender = false;
                        c14.Colspan = 4;
                        c14.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c14);

                        PdfPCell c15 = new PdfPCell(new Phrase("Proveedor:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                        c15.UseAscender = false;
                        c15.UseDescender = true;
                        c15.Colspan = 3;
                        c15.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c15);

                        PdfPCell c16 = new PdfPCell(new Phrase(proveedorpdf, FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                        c16.Colspan = 5;
                        c16.UseAscender = false;
                        c16.UseDescender = true;
                        c16.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c16);

                        PdfPCell c17 = new PdfPCell(new Phrase("Fecha De Entrega:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                        c17.UseAscender = false;
                        c17.UseDescender = true;
                        c17.Colspan = 4;
                        c17.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c17);

                        PdfPCell c18 = new PdfPCell(new Phrase(dateTimePickerFechaEntrega.Value.ToString("dddd, dd " + "DE" + " MMMM " + "DE" + " yyyy").ToUpper(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                        c18.Colspan = 4;
                        c18.UseAscender = false;
                        c18.UseDescender = true;
                        c18.Colspan = 4;
                        c18.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c18);

                        PdfPCell c19 = new PdfPCell(new Phrase("Correo:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                        c19.UseAscender = false;
                        c19.UseDescender = true;
                        c19.Colspan = 3;
                        c19.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c19);

                        PdfPCell c20 = new PdfPCell(new Phrase("almacen@tribuses.com", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                        c20.UseAscender = false;
                        c20.UseDescender = true;
                        c20.Colspan = 5;
                        c20.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c20);

                        PdfPCell c21 = new PdfPCell(new Phrase("Facturar A:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD)));
                        c21.UseAscender = false;
                        c21.UseDescender = true;
                        c21.Colspan = 4;
                        c21.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c21);

                        PdfPCell c22 = new PdfPCell(new Phrase(facturarpdf, FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                        c22.Colspan = 4;
                        c22.UseAscender = false;
                        c22.UseDescender = true;
                        c22.HorizontalAlignment = Element.ALIGN_CENTER;
                        tb2.AddCell(c22);
                        doc.Add(tb2);

                        PdfPTable tb3 = new PdfPTable(16);
                        tb3.WidthPercentage = 95;
                        tb3.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell c31 = new PdfPCell(new Phrase("PARTIDA", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c31.UseDescender = true;
                        c31.HorizontalAlignment = Element.ALIGN_CENTER;
                        c31.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c31);

                        PdfPCell c32 = new PdfPCell(new Phrase("CLAVE", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c32.Colspan = 2;
                        c32.UseDescender = true;
                        c32.HorizontalAlignment = Element.ALIGN_CENTER;
                        c32.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c32);

                        PdfPCell c33 = new PdfPCell(new Phrase("DESCRIPCIÓN", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c33.UseDescender = true;
                        c33.Colspan = 5;
                        c33.HorizontalAlignment = Element.ALIGN_CENTER;
                        c33.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c33);

                        PdfPCell c34 = new PdfPCell(new Phrase("CANTIDAD EN EXISTENCIA", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c34.UseAscender = true;
                        c34.UseDescender = false;
                        c34.HorizontalAlignment = Element.ALIGN_CENTER;
                        c34.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c34);

                        PdfPCell c35 = new PdfPCell(new Phrase("CANTIDAD SOLICITADA", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c35.UseAscender = true;
                        c35.UseDescender = false;
                        c35.HorizontalAlignment = Element.ALIGN_CENTER;
                        c35.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c35);

                        PdfPCell c36 = new PdfPCell(new Phrase("PRECIO COTIZADO", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c36.UseAscender = true;
                        c36.UseDescender = false;
                        c36.HorizontalAlignment = Element.ALIGN_CENTER;
                        c36.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c36);

                        PdfPCell c37 = new PdfPCell(new Phrase("TOTAL", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c37.UseDescender = true;
                        c37.Colspan = 2;
                        c37.HorizontalAlignment = Element.ALIGN_CENTER;
                        c37.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c37);

                        PdfPCell c38 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.BOLD)));
                        c38.UseDescender = true;
                        c38.Colspan = 4;
                        c38.HorizontalAlignment = Element.ALIGN_CENTER;
                        c38.BackgroundColor = new BaseColor(234, 231, 231);
                        tb3.AddCell(c38);

                        string cantidad, cod, DescRef, existenciasA, cSOlicitada, pre_cotizado, total_Ref;

                        MySqlCommand maximo = new MySqlCommand("select max(t1.NumRefacc) as tam from detallesordencompra as t1 inner join ordencompra as t2 on t1.OrdfkOrdenCompra=t2.idOrdCompra  where t2.FolioOrdCompra='" + labelFolioOC.Text + "' ", co.dbconection());
                        MySqlDataReader DR1 = maximo.ExecuteReader();

                        MySqlCommand almacenar_datos = new MySqlCommand("SET NAMES 'utf8';SELECT t1.NumRefacc AS partida, IF(t1.ClavefkCRefacciones <> 0, (SELECT UPPER(x1.codrefaccion) FROM crefacciones AS x1 WHERE x1.idrefaccion = t1.ClavefkCRefacciones), '') AS codigo, IF(t1.Refacciones <> '', UPPER(t1.Refacciones), (SELECT UPPER(x2.nombreRefaccion) FROM crefacciones AS x2 WHERE x2.idrefaccion = t1.ClavefkCRefacciones)) AS nombre, COALESCE((SELECT x3.existencias FROM crefacciones AS x3 WHERE x3.idrefaccion = t1.ClavefkCRefacciones), 0) AS existencias_almacen, t1.Cantidad AS canti_soli, t1.Precio AS precio_cot, t1.Total AS Tot_ref FROM detallesordencompra AS t1 INNER JOIN ordencompra AS t2 ON t1.OrdfkOrdenCompra = t2.idOrdCompra WHERE t2.FolioOrdCompra ='" + labelFolioOC.Text + "'", co.dbconection());
                        MySqlDataReader DR = almacenar_datos.ExecuteReader();
                        int i = 0;
                        while (DR.Read())
                        {
                            cantidad = Convert.ToString(DR["Partida"]);
                            cod = Convert.ToString(DR["codigo"]);
                            DescRef = Convert.ToString(DR["nombre"]);
                            existenciasA = Convert.ToString(DR["existencias_almacen"]);
                            cSOlicitada = Convert.ToString(DR["canti_soli"]);
                            pre_cotizado = Convert.ToString(DR["precio_cot"]);
                            total_Ref = Convert.ToString(DR["Tot_ref"]);
                            totalrefacc = Convert.ToInt32(DR.GetString("Partida"));

                            PdfPCell c1 = new PdfPCell(new Phrase(cantidad, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c1.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c1);

                            PdfPCell c2 = new PdfPCell(new Phrase(cod, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c2.Colspan = 2;
                            c2.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c2);

                            PdfPCell c3 = new PdfPCell(new Phrase(DescRef, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c3.Colspan = 5;
                            c3.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c3);

                            PdfPCell c4 = new PdfPCell(new Phrase(existenciasA, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c4.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c4);

                            PdfPCell c5 = new PdfPCell(new Phrase(cSOlicitada, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c5.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c5);

                            PdfPCell c6 = new PdfPCell(new Phrase("$  " + pre_cotizado, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c6.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c6);

                            PdfPCell c7 = new PdfPCell(new Phrase("$  " + total_Ref, FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c7.Colspan = 2;
                            c7.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c7);

                            if (DR1.Read())
                            {

                                totalrefacc = Convert.ToInt32(DR1["tam"]);
                                string obs = "";
                                for (int inn = 1; inn <= totalrefacc; inn++)
                                {
                                    acumobservacionesrefacc = v.getaData("SELECT ObservacionesRefacc FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idfolio + "' AND NumRefacc = '" + inn + "'").ToString();
                                    if (acumobservacionesrefacc != "")
                                    {
                                        obs += "PARTIDA " + inn + "- " + acumobservacionesrefacc + "\n \n";
                                    }
                                    if (inn == totalrefacc)
                                    {
                                        obs += textBoxObservaciones.Text + "\n \n";
                                    }
                                }
                                PdfPCell c39 = new PdfPCell(new Phrase(obs, FontFactory.GetFont("ARIAL", 4, iTextSharp.text.Font.NORMAL)));
                                c39.Colspan = 3;
                                c39.Rowspan = 54;
                                c39.HorizontalAlignment = Element.ALIGN_CENTER;
                                c39.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tb3.AddCell(c39);
                            }
                            i++;
                        }
                        DR.Close();
                        co.dbconection().Close();

                        while (i < 49)
                        {
                            PdfPCell c1 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c1.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c1);

                            PdfPCell c2 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c2.Colspan = 2;
                            c2.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c2);

                            PdfPCell c3 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c3.Colspan = 5;
                            c3.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c3);

                            PdfPCell c4 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c4.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c4);

                            PdfPCell c5 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c5.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c5);

                            PdfPCell c6 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c6.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c6);

                            PdfPCell c7 = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL)));
                            c7.Colspan = 2;
                            c7.HorizontalAlignment = Element.ALIGN_CENTER;
                            tb3.AddCell(c7);
                            i++;
                        }
                        DR1.Close();

                        doc.Add(tb3);
                    }


                    PdfPTable tablefot = new PdfPTable(16);
                    tablefot.WidthPercentage = 95;
                    tablefot.TotalWidth = 541f;
                    tablefot.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell cf01 = new PdfPCell(new Phrase("SUBTOTAL:", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                    cf01.HorizontalAlignment = Element.ALIGN_CENTER;
                    cf01.Colspan = 3;
                    tablefot.AddCell(cf01);

                    PdfPCell cf02 = new PdfPCell(new Phrase("$ " + labelSubTotalOC.Text, FontFactory.GetFont("ARIAl", 7, iTextSharp.text.Font.NORMAL)));
                    cf02.UseDescender = true;
                    cf02.Colspan = 2;
                    cf02.HorizontalAlignment = Element.ALIGN_LEFT;
                    tablefot.AddCell(cf02);

                    PdfPCell cf03 = new PdfPCell(new Phrase("IVA (" + textBoxIVA.Text + "%):", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                    cf03.HorizontalAlignment = Element.ALIGN_CENTER;
                    cf03.Colspan = 3;
                    tablefot.AddCell(cf03);

                    PdfPCell cf04 = new PdfPCell(new Phrase("$ " + labelIVAOC.Text, FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                    cf04.UseDescender = true;
                    cf04.HorizontalAlignment = Element.ALIGN_LEFT;
                    cf04.Colspan = 3;
                    tablefot.AddCell(cf04);

                    PdfPCell cf05 = new PdfPCell(new Phrase("TOTAL:", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                    cf05.HorizontalAlignment = Element.ALIGN_CENTER;
                    cf05.Colspan = 2;
                    tablefot.AddCell(cf05);

                    PdfPCell cf06 = new PdfPCell(new Phrase("$ " + labelTotalOC.Text, FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
                    cf06.UseDescender = true;
                    cf06.Colspan = 3;
                    cf06.HorizontalAlignment = Element.ALIGN_LEFT;
                    tablefot.AddCell(cf06);

                    PdfPCell cf11 = new PdfPCell(new Phrase(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + almacenistapdf, FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                    cf11.Colspan = 8;
                    cf11.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablefot.AddCell(cf11);

                    PdfPCell cf12 = new PdfPCell(new Phrase(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + autorizapdf, FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                    cf12.Colspan = 8;
                    cf12.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablefot.AddCell(cf12);

                    PdfPCell cf13 = new PdfPCell(new Phrase("ALMACÉN TRI", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                    cf13.Colspan = 8;
                    cf13.UseAscender = true;
                    cf13.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablefot.AddCell(cf13);

                    PdfPCell cf14 = new PdfPCell(new Phrase("AUTORIZA", FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
                    cf14.Colspan = 8;
                    cf14.UseAscender = true;
                    cf14.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablefot.AddCell(cf14);

                    doc.Add(tablefot);

                    doc.AddCreationDate();
                    doc.Close();
                    exportacionpdf();
                    Process.Start(filename);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /* Acciones con los botones y gridview */
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void buttonPDF_Click(object sender, EventArgs e)
        {
            if (dataGridViewPedOCompra.Rows.Count > 0)
            {
                privilegios();
                MySqlCommand cmd = new MySqlCommand("SELECT Almacen, Autoriza FROM nombresoc WHERE empresa='"+empresa+"'", co.dbconection());
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    almacenistapdf = dr.GetString("Almacen");
                    autorizapdf = dr.GetString("Autoriza");
                }
                else
                {
                    almacenistapdf = "";
                    autorizapdf = "";
                }
                dr.Close();
                co.dbconection().Close();
                MySqlCommand cmd0 = new MySqlCommand("SELECT t2.nombreEmpresa AS Empresa, t3.empresa AS Proveedor FROM ordencompra AS t1 INNER JOIN cempresas AS t2 ON t1.FacturadafkCEmpresas = t2.idempresa INNER JOIN cproveedores AS t3 ON t1.ProveedorfkCProveedores = t3.idproveedor WHERE t1.FolioOrdCompra = '" + labelFolioOC.Text + "'", co.dbconection());
                MySqlDataReader dr0 = cmd0.ExecuteReader();
                if (dr0.Read())
                {
                    proveedorpdf = dr0.GetString("Proveedor");
                    facturarpdf = dr0.GetString("Empresa");
                }
                else
                {
                    proveedorpdf = "";
                    facturarpdf = "";
                }
                dr0.Close();
                co.dbconection().Close();
                if ((almacenistapdf != "") && (autorizapdf != "") && (facturarpdf != "") && (proveedorpdf != ""))
                {
                    To_pdf();
                }
                else if ((string.IsNullOrWhiteSpace(almacenistapdf) && string.IsNullOrWhiteSpace(almacenistapdf)))
                {
                    MessageBox.Show("Falta el registro de los nombres de los responsables de la Orden De Compra\n", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    NombresOC noc = new NombresOC(empresa, area, pictureBox1.BackgroundImage);
                    noc.Owner = this;
                    noc.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Faltan datos por registrar en la orden de compra para poder exportar en PDF", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                MessageBox.Show("No puede generar una orden de compra en PDF si no existen refacciones agregadas", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        void limpiar_variables()
        {
            observacionesanterior = "";
            idproveedoranterior = 0;
            idfacturaranterior = 0;
            _idcomp = 0;
        }
        string observaciones;
        private void dataGridViewOCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    validacionval();
                    if ((observacionesanterior.Equals(textBoxObservaciones.Text.Trim())) && (idproveedoranterior.Equals(Convert.ToInt32(comboBoxProveedor.SelectedValue))) && (idfacturaranterior.Equals(Convert.ToInt32(comboBoxFacturar.SelectedValue))))
                    {
                        limpiarRefaccN();
                        llamarorden();
                        metodocargadetordenPDF();
                        dataGridViewPedOCompra.Enabled = true;
                        if (comboBoxProveedor.Text != "")
                        {
                            comboBoxProveedor.Enabled = false;
                        }
                        if (comboBoxFacturar.Text != "")
                        {
                            comboBoxFacturar.Enabled = false;
                        }
                        if (cbcomparativa.SelectedIndex > 0)
                        {
                            cbcomparativa.Enabled = false;
                        }
                        if (textBoxObservaciones.Text != "")
                        {
                            textBoxObservaciones.Enabled = false;
                        }
                        else
                        {
                            textBoxObservaciones.Enabled = true;
                        }
                        buttonActualizar.Visible = false;
                        label37.Visible = false;
                        dateTimePickerFechaEntrega.Enabled = false;
                        observaciones = Convert.ToString(dataGridViewOCompra.CurrentRow.Cells[9].Value);
                        comboBoxClave.Visible = true;
                        labelSubTotalOC.Visible = true;
                        labelIVAOC.Visible = true;
                        labelTotalOC.Visible = true;
                        labelSubTotal.Visible = true;
                        comboBoxClave.Enabled = true;
                    }
                    else
                    {
                        if (MessageBox.Show("Si usted cambia de reporte sin guardar perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            limpiarRefaccN();
                            llamarorden();
                            metodocargadetordenPDF();
                            labelSubTotal.Text = "00.00";
                            dataGridViewPedOCompra.Enabled = true;
                            if (comboBoxProveedor.Text != "")
                            {
                                comboBoxProveedor.Enabled = false;
                            }
                            if (comboBoxFacturar.Text != "")
                            {
                                comboBoxFacturar.Enabled = false;
                            }
                            if (textBoxObservaciones.Text != "")
                            {
                                textBoxObservaciones.Enabled = false;
                            }
                            else
                            {
                                textBoxObservaciones.Enabled = true;
                            }
                            buttonActualizar.Visible = false;
                            label37.Visible = false;
                            dateTimePickerFechaEntrega.Enabled = false;
                            observaciones = Convert.ToString(dataGridViewOCompra.CurrentRow.Cells[9].Value);
                            dataGridViewPedOCompra.ClearSelection();
                            //comboBoxClave.SelectedIndex = 0;
                            comboBoxClave.Visible = true;
                            labelSubTotalOC.Visible = true;
                            labelIVAOC.Visible = true;
                            labelTotalOC.Visible = true;
                            labelSubTotal.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewPedOCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {         
        }

        void Limpia_variables()
        {
            sumaCSolicitada = 0;
            resultado = 0;
            resultadototal = 0;
            cantidadrefacc = 0;
            totalrefacc = 0;
            _idcomp = 0;
        }

        private void buttonNuevoOC_Click(object sender, EventArgs e)
        {
            try
            {
                validacionval();
                banderaeditar = false;
                if (banderaeditar && ((idproveedoranterior != idvalidacionproveedor) || (comboBoxProveedor.SelectedIndex == 0)) && ((idfacturaranterior != idvalidacionfacturar) || (comboBoxFacturar.SelectedIndex == 0)) && ((textBoxObservaciones.Text.Trim() != observacionesanterior) || (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))))
                {
                    if (MessageBox.Show("Si usted cambia de orden de compra sin guardar, perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        btnnuevoOC();
                    }
                }
                else
                {
                    if (!banderaeditar && ((comboBoxClave.SelectedIndex == 0) && comboBoxFacturar.SelectedIndex > 0 && comboBoxProveedor.SelectedIndex > 0))
                    {
                        btnnuevoOC();
                    }
                    else
                    {
                        if (MessageBox.Show("Si usted cambia de orden de compra sin guardar, perdera los nuevos datos ingresados \n¿Desea cambiar de reporte?", "ADVERTENCIA", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            btnnuevoOC();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if ((comboBoxProveedor.SelectedIndex == 0) && (comboBoxClave.SelectedIndex == 0) && (comboBoxFacturar.SelectedIndex == 0) && (cbcomparativa.SelectedIndex==0))
                {
                    MessageBox.Show("Tiene que llenar todos los campos para añadir una refacción", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DateTime actual = DateTime.Now.Date.AddMonths(12);
                    if (cbcomparativa.SelectedIndex == 0)
                    {
                        MessageBox.Show("Seleccione una comparativa de la lista desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (comboBoxProveedor.SelectedIndex == 0)
                    {
                        MessageBox.Show("Seleccione un proveedor", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dateTimePickerFechaEntrega.Value.Date < DateTime.Now.Date)
                    {
                        MessageBox.Show("La fecha de entrega no debe de ser menor a la fecha actual", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (dateTimePickerFechaEntrega.Value.Date > actual)
                    {
                        MessageBox.Show("La fecha de entrega no debe superar más de 1 año", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (comboBoxClave.SelectedIndex == 0 && comboBoxClave.Visible == true)
                    {
                        MessageBox.Show("Seleccione una refacción de la lista desplegable", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (comboBoxFacturar.SelectedIndex == 0)
                    {
                        MessageBox.Show("Seleccione una empresa para facturar", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        metodorecid();

                        MySqlCommand cmd0 = new MySqlCommand("SELECT NumRefacc FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idfolio + "' ORDER BY idDetOrdCompra DESC ", co.dbconection());
                        MySqlDataReader dr0 = cmd0.ExecuteReader();
                        if (dr0.Read())
                        {
                            contadorefacc = Convert.ToInt32(dr0.GetString("NumRefacc"));
                            contadorefacc = contadorefacc + 1;
                        }
                        else
                        {
                            contadorefacc = 0;
                            contadorefacc = contadorefacc + 1;
                        }
                        dr0.Close();
                        co.dbconection().Close();
                        if (idfolio == 0)
                        {
                            DataTable dt1 = new DataTable();
                            MySqlCommand cmd1 = new MySqlCommand("INSERT INTO ordencompra(FolioOrdCompra, ProveedorfkCProveedores, FacturadafkCEmpresas, FechaOCompra, FechaEntregaOCompra,IVA,usuariofkcpersonal,ObservacionesOC,ComparativaFKComparativas,empresa) VALUES('" + labelFolioOC.Text + "', '" + comboBoxProveedor.SelectedValue + "', '" + comboBoxFacturar.SelectedValue + "', curdate(), '" + dateTimePickerFechaEntrega.Value.ToString("yyyy-MM-dd") + "','" + textBoxIVA.Text + "','" + idUsuario + "','" + textBoxObservaciones.Text.Trim() + "','" + cbcomparativa.SelectedValue + "','"+empresa+"')", co.dbconection());
                            MySqlDataAdapter adp1 = new MySqlDataAdapter(cmd1);
                            adp1.Fill(dt1);
                            dataGridViewOCompra.DataSource = dt1;
                            co.dbconection().Close();
                            if (label3.Text == "EXPORTANDO")
                            { }
                            else
                            {
                                ocultarexcel();
                            }
                            AutoCompletado(textBoxOCompraB);
                        }
                        metodorecid();

                        if (Convert.ToInt32(comboBoxClave.SelectedValue) > 0)
                        {
                            codigorefaccionvalidada = Convert.ToString(comboBoxClave.SelectedValue);
                            codigorefaccionvalidada1 = "ClavefkCRefacciones";
                        }
                        else
                        {
                            codigorefaccionvalidada = comboBoxClave.Text;
                            codigorefaccionvalidada1 = "Refacciones";
                        }

                        DataTable dt = new DataTable();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO detallesordencompra(OrdfkOrdenCompra, NumRefacc, " + codigorefaccionvalidada1 + ", Cantidad, Precio, Total, ObservacionesRefacc,usuariofkcpersonal,empresa) VALUES('" + idfolio + "', '" + contadorefacc + "', '" + codigorefaccionvalidada + "', '" + lblCantidad.Text + "', '" + lblPrecio.Text + "', '" + resultado + "', '" + textBoxObservacionesRefacc.Text + "','" + idUsuario + "','"+empresa+"')", co.dbconection());
                        cmd.ExecuteNonQuery();
                        co.dbconection().Close();

                        MySqlCommand cmd3 = new MySqlCommand("SELECT COALESCE(SUM(Total), 0) AS Total FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idfolio + "'", co.dbconection());
                        MySqlDataReader dr3 = cmd3.ExecuteReader();
                        if (dr3.Read())
                        {
                            sumaCSolicitada = Convert.ToDouble(dr3.GetString("Total"));
                        }
                        dr3.Close();
                        co.dbconection().Close();
                        if (comboBoxProveedor.SelectedIndex != 0)
                        {
                            comboBoxProveedor.Enabled = false;
                        }
                        if (comboBoxFacturar.SelectedIndex != 0)
                        {
                            comboBoxFacturar.Enabled = false;
                        }
                        if (dateTimePickerFechaEntrega.Value != DateTime.Now)
                        {
                            dateTimePickerFechaEntrega.Enabled = false;
                        }
                        buttonNuevoOC.Visible = true;
                        label17.Visible = true;
                        buttonFinalizar.Visible = true;
                        label34.Visible = true;
                        buttonPDF.Visible = true;
                        label3.Visible = true;
                        dataGridViewPedOCompra.Enabled = true;
                        if (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))
                        {
                            textBoxObservaciones.Enabled = true;
                        }
                        else
                        {
                            textBoxObservaciones.Enabled = false;
                        }


                        if (string.IsNullOrWhiteSpace(textBoxObservaciones.Text))
                        { }
                        else
                        {
                            if ((string.IsNullOrWhiteSpace(textBoxObservaciones.Text)) && (textBoxObservacionesRefacc.Text != ""))
                            {
                                if (acumobservacionesrefacc == "")
                                {
                                    acumobservacionesrefacc = "Partida " + contadorefacc + " " + textBoxObservacionesRefacc.Text;
                                }
                                else
                                {
                                    acumobservacionesrefacc += ", Partida " + contadorefacc + " " + textBoxObservacionesRefacc.Text;
                                }
                                MySqlCommand cmd00 = new MySqlCommand("UPDATE pedidosrefaccion SET ObservacionesRefacc = '" + textBoxObservacionesRefacc.Text + "' WHERE OrdfkOrdenCompra = '" + idfolio + "' AND NumRefacc = '" + contadorefacc + "'", co.dbconection());
                                cmd00.ExecuteNonQuery();
                                co.dbconection().Close();
                                MySqlCommand cmd04 = new MySqlCommand("UPDATE ordencompra SET ObservacionesOC ='" + acumobservacionesrefacc + "' WHERE idOrdCompra = '" + idfolio + "'", co.dbconection());
                                cmd04.ExecuteNonQuery();
                                co.dbconection().Close();
                            }
                            else if ((textBoxObservaciones.Text != "") && (string.IsNullOrWhiteSpace(textBoxObservacionesRefacc.Text)))
                            {
                                observacionesOCompra = textBoxObservaciones.Text + "; ";
                                MySqlCommand cmd03 = new MySqlCommand("UPDATE ordencompra SET ObservacionesOC = '" + textBoxObservaciones.Text + "' WHERE idOrdCompra = '" + idfolio + "'", co.dbconection());
                                cmd03.ExecuteNonQuery();
                                co.dbconection().Close();
                                //observaciones = textBoxObservaciones.Text;
                            }
                        }
                        limpiarRefacc();
                        metodocargaorden();
                        metodocargadetordenPDF();
                        actualizarcbx();
                        MessageBox.Show("Refacción agregada correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (label38.Text.Equals("EXPORTANDO"))
                        {
                            activo = true;
                        }
                        else
                        {
                            buttonExcel.Visible = false;
                            label38.Visible = false;
                        }
                        CargarEmpresasBusqueda();
                        CargarProveedoresBusqueda();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(textBoxOCompraB.Text)) && (comboBoxProveedorB.Text == "-- SELECCIONE UN PROVEEDOR --") && (comboBoxEmpresaB.Text == "-- SELECCIONE UNA EMPRESA --") && (checkBoxFechas.Checked == false) && (checkBoxFechasE.Checked == false))
            {
                MessageBox.Show("Seleccione un criterio de búsqueda", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dateTimePickerIni.Value > dateTimePickerFin.Value)
            {
                MessageBox.Show("La fecha inicial no debe superar a la fecha final \n Favor de cambiar el rango de fechas", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                checkBoxFechas.Checked = false;
                limpiarRefaccB();
            }
            else if (dateTimePickerIniBFE.Value > dateTimePickerFinBFE.Value)
            {
                MessageBox.Show("La fecha de entrega inicial no debe superar a la fecha final \n Favor de cambiar el rango de fechas", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                checkBoxFechasE.Checked = false;
                limpiarRefaccB();
            }
            else
            {
                String Fini = "";
                String Ffin = "";
                String FiniE = "";
                String FfinE = "";
                string consulta = "SET lc_time_names = 'es_ES'; SELECT t1.FolioOrdCompra AS 'Orden De Compra', UPPER(t2.empresa) AS Proveedor, UPPER(t3.nombreEmpresa) AS 'Nombre De La Empresa', UPPER(DATE_FORMAT(t1.FechaOCompra, '%W %d %M %Y')) AS 'Fecha', UPPER(DATE_FORMAT(t1.FechaEntregaOCompra, '%W %d %M %Y')) AS 'Fecha De Entrega', coalesce((t1.Subtotal), '') AS Subtotal, coalesce(IF(((t1.IVA != '') && (t1.Estatus = 'FINALIZADA')), cast(((t1.IVA/100)*t1.Subtotal) as decimal (5,2)), ''), '') AS IVA, coalesce((t1.Total), '') AS Total, coalesce((UPPER(t1.Estatus)), '') AS ESTATUS, coalesce((SELECT UPPER(CONCAT(t4.ApPaterno, ' ', t4.ApMaterno, ' ', t4.nombres)) FROM cpersonal AS t4 WHERE t1.PersonaFinal = t4.idPersona), '') AS 'Persona Final', UPPER(t1.ObservacionesOC) AS 'Observaciones' FROM ordencompra AS t1 LEFT JOIN cproveedores AS t2 ON t1.ProveedorfkCProveedores = t2.idproveedor INNER JOIN cempresas AS t3 ON t1.FacturadafkCEmpresas = t3.idempresa";
                string WHERE = "";
                if (!string.IsNullOrWhiteSpace(textBoxOCompraB.Text))
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (t1.FolioOrdCompra = '" + textBoxOCompraB.Text + "')";
                    }
                    else
                    {
                        WHERE += " AND (t1.FolioOrdCompra = '" + textBoxOCompraB.Text + "')";
                    }
                }
                if (comboBoxProveedorB.SelectedIndex > 0)
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (t2.idproveedor = '" + comboBoxProveedorB.SelectedValue + "')";
                    }
                    else
                    {
                        WHERE += " AND (t2.idproveedor = '" + comboBoxProveedorB.SelectedValue + "')";
                    }
                }
                if (comboBoxEmpresaB.SelectedIndex > 0)
                {
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (t3.nombreEmpresa = '" + comboBoxEmpresaB.Text + "')";
                    }
                    else
                    {
                        WHERE += " AND (t3.nombreEmpresa = '" + comboBoxEmpresaB.Text + "')";
                    }
                }
                if (checkBoxFechas.Checked == true)
                {
                    Fini = dateTimePickerIni.Value.ToString("yyyy-MM-dd");
                    Ffin = dateTimePickerFin.Value.ToString("yyyy-MM-dd");
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (SELECT t1.FechaOCompra BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    }
                    else
                    {
                        WHERE += " AND (SELECT t1.FechaOCompra BETWEEN '" + Fini.ToString() + "' AND '" + Ffin.ToString() + "')";
                    }
                }
                if (checkBoxFechasE.Checked == true)
                {
                    FiniE = dateTimePickerIniBFE.Value.ToString("yyyy-MM-dd");
                    FfinE = dateTimePickerFinBFE.Value.ToString("yyyy-MM-dd");
                    if (WHERE == "")
                    {
                        WHERE = " WHERE (SELECT t1.FechaEntregaOCompra BETWEEN '" + FiniE.ToString() + "' AND '" + FfinE.ToString() + "')";
                    }
                    else
                    {
                        WHERE += " AND (SELECT t1.FechaEntregaOCompra BETWEEN '" + FiniE.ToString() + "' AND '" + FfinE.ToString() + "')";
                    }
                }
                if (WHERE != "")
                {
                    WHERE += " and empresa='"+empresa+"' ORDER BY t1.FolioOrdCompra DESC";
                }
                MySqlDataAdapter adp = new MySqlDataAdapter(consulta + WHERE, co.dbconection());
                DataSet ds = new DataSet();
                adp.Fill(ds);
                dataGridViewOCompra.DataSource = ds.Tables[0];
                co.dbconection().Close();

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron reportes", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    metodocargaorden();
                    actualizarcbx();
                    if (label38.Text == "EXPORTANDO")
                    { }
                    else
                    {
                        ocultarexcel();
                    }
                    limpiarRefaccB();
                }
                else
                {
                    if (!exportando)
                    {
                        buttonExcel.Visible = true;
                    }
                    label38.Visible = true;
                    limpiarRefaccB();
                    metodo();
                }
                co.dbconection().Close();
            }
        }

        void metodo()
        {
            buttonActualizar.Visible = true;
            label37.Visible = true;


            checkBoxFechas.Checked = false;
            checkBoxFechasE.Checked = false;

        }
        bool activo = false;
        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                metodocargadetordenPDF();
                buttonActualizar.Visible = false;
                label37.Visible = false;
                buttonExcel.Visible = false;
                metodocargaorden();
                if (label38.Text.Equals("EXPORTANDO"))
                {
                    activo = true;
                }
                else
                {
                    buttonExcel.Visible = false;
                    label38.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonAgregarMas_Click(object sender, EventArgs e)
        {
            banderaeditar = false;
            MySqlCommand cmd1 = new MySqlCommand("SELECT SUM(Total) AS Total FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idordencompra + "'", co.dbconection());
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            if (dr1.Read())
            {
                sumaCSolicitada = Convert.ToDouble(dr1.GetString("Total"));
            }
            dr1.Close();
            co.dbconection().Close();
            metodocargaiva();
            comboBoxClave.SelectedIndex = 0;
            comboBoxClave.Visible = true;
            buttonActualizar.Visible = false;
            label37.Visible = false;
            buttonNuevoOC.Visible = true;
            label17.Visible = true;
            buttonAgregar.Visible = true;
            label9.Visible = true;
            buttonAgregarMas.Visible = false;
            label29.Visible = false;
            buttonEditar.Visible = false;
            label8.Visible = false;
            buttonFinalizar.Visible = true;
            label34.Visible = true;
            buttonPDF.Visible = true;
            label3.Visible = true;
            labelSubTotalOC.Visible = true;
            labelIVAOC.Visible = true;
            labelTotalOC.Visible = true;
            limpiarRefacc();
            CargarClave();
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            DateTime actual = DateTime.Now.Date.AddMonths(12);
            if (dateTimePickerFechaEntrega.Value.Date >= DateTime.Now.Date)
            {
                if (!(dateTimePickerFechaEntrega.Value.Date > actual))
                {
                    if (comboBoxFacturar.SelectedIndex != 0 && comboBoxProveedor.SelectedIndex != 0 && comboBoxClave.SelectedIndex == 0)
                    {
                        if (!((proveedoranterior.Equals(comboBoxProveedor.Text)) && (facturaranterior.Equals(comboBoxFacturar.Text)) && (observacionesanterior.Equals(textBoxObservaciones.Text)) && (fentregaestimadanterior.Equals(dateTimePickerFechaEntrega.Value.ToString("yyyy-MM-dd")))))
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            if (obs.ShowDialog() == DialogResult.OK)
                            {
                                string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                                MySqlCommand cmd0 = new MySqlCommand("UPDATE ordencompra SET ProveedorfkCProveedores = '" + comboBoxProveedor.SelectedValue + "', FacturadafkCEmpresas = '" + comboBoxFacturar.SelectedValue + "', FechaEntregaOCompra = '" + dateTimePickerFechaEntrega.Value.ToString("yyyy-MM-dd") + "', ObservacionesOC = '" + textBoxObservaciones.Text + "' WHERE idOrdCompra = '" + idordencompra + "'", co.dbconection());
                                cmd0.ExecuteNonQuery();
                                co.dbconection().Close();

                                MySqlCommand cmd00 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion ,empresa, area) VALUES('Orden De Compra', '" + idordencompra + "', CONCAT('" + comboBoxProveedor.SelectedValue + ";', '" + comboBoxFacturar.SelectedValue + ";', '" + fentregaestimadanterior + ";', '" + textBoxObservaciones.Text + "'), '" + idUsuario + "', now(), 'Actualización de Orden de Compra','" + observaciones + "', '2', '2')", co.dbconection());
                                cmd00.ExecuteNonQuery();
                                co.dbconection().Close();

                                MessageBox.Show("Orden de Compra editada correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                btneditar();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sin cambios", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btneditar();
                        }
                    }
                    else
                    {
                        if (comboBoxFacturar.SelectedIndex == 0)
                        {
                            MessageBox.Show("La empresa a facturar no puede quedar vacia", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (comboBoxProveedor.SelectedIndex == 0)
                            {
                                MessageBox.Show("El proveedor no puede quedar vacio", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(lblCantidad.Text.Trim()) && Convert.ToDouble(lblCantidad.Text) <= 0)
                                {
                                    MessageBox.Show("La cantidad solicitada debe de ser mayor a 0", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    if (comboBoxClave.SelectedIndex == 0 && comboBoxClave.Visible == true)
                                    {
                                        MessageBox.Show("No puede dejar en blanco la clave de la refacción", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        if ((comboBoxClave.Text.Equals(codigorefanterior)) && /*(cantidadsolicitadanterior.Equals(valorcantidadsolicitada)) && (preciocotizadoanterior.Equals(valorpreciocotizado)) &&*/ (textBoxObservacionesRefacc.Text.Equals(observacionesrefaccanterior)))
                                        {
                                            MessageBox.Show("No se realizó ningún cambio en los datos", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            observacionesEdicion obs = new observacionesEdicion(v);
                                            obs.Owner = this;
                                            if (obs.ShowDialog() == DialogResult.OK)
                                            {
                                                ////string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                                //numerorefaccion = Convert.ToInt32(dataGridViewPedOCompra.CurrentRow.Cells["PARTIDA"].Value.ToString());
                                                //MySqlCommand cmd = new MySqlCommand("UPDATE detallesordencompra SET ClavefkCRefacciones = '" + comboBoxClave.SelectedValue + "', Cantidad = '" + valorcantidadsolicitada + "', Precio = '" + valorpreciocotizado + "', Total = '" + resultado + "', ObservacionesRefacc = '" + textBoxObservacionesRefacc.Text + "' WHERE NumRefacc = '" + numerorefaccion + "' AND OrdfkOrdenCompra = '" + idordencompra + "'", co.dbconection());
                                                //cmd.ExecuteNonQuery();
                                                //MessageBox.Show("Refacción actualizada con éxito", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                //co.dbconection().Close();
                                                //MySqlCommand cmd1 = new MySqlCommand("SELECT SUM(Total) AS Total FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idordencompra + "'", co.dbconection());
                                                //MySqlDataReader dr1 = cmd1.ExecuteReader();
                                                //if (dr1.Read())
                                                //{
                                                //    sumaCSolicitada = Convert.ToDouble(dr1.GetString("Total"));
                                                //}
                                                //dr1.Close();
                                                //co.dbconection().Close();
                                        //        MySqlCommand cmd2 = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion,empresa, area) VALUES('Orden De Compra', '" + idordencompra + "', CONCAT('" + idcodigorefanterior + ";', '" + preciocotizadoanterior + ";', '" + cantidadsolicitadanterior + ";', '" + observacionesrefaccanterior + "'), '" + idUsuario + "', now(), 'Actualización de Refacción de Orden de Compra','" + observaciones + "','2', '2')", co.dbconection());
                                            //    cmd2.ExecuteNonQuery();
                                                co.dbconection().Close();
                                                btneditar();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("La fecha de entrega no debe superar más de 1 año", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("La fecha no debe de ser menor a la actual, favor de modificar la fecha de entrega", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void Limpiar_labels()
        {
            labelSubTotal.Text = "0";
            labelSubTotalOC.Text = "0";
            labelIVAOC.Text = "0";
            labelTotalOC.Text = "0";
        }
        private void buttonFinalizar_Click(object sender, EventArgs e)
        {
            FormContraFinal FCF = new FormContraFinal(empresa, area, this);
            if (metodotxtpedcompra() == true)
            {
                MessageBox.Show("Tiene que agregar al menos una refaccion en la Orden De Compra", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                FCF.LabelTitulo.Text = "Ingrese su Contraseña para Finalizar" + Environment.NewLine + "La Requisición";
                FCF.LabelTitulo.Left = (FCF.Width - FCF.LabelTitulo.Width) / 2;
                FCF.LabelEncabezado.Left = (FCF.Width - FCF.LabelEncabezado.Width) / 2;

                DialogResult res = FCF.ShowDialog();
                if (res == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(labelidFinal.Text))
                    { }
                    else if (!string.IsNullOrWhiteSpace(labelidFinal.Text))
                    {
                        nrefacc = 0;
                        idordencompra = Convert.ToInt32(v.getaData("select idOrdCompra from ordencompra where FolioOrdCompra='" + labelFolioOC.Text + "'").ToString());
                        string sql = "SELECT MAX(NumRefacc) AS NumRefacc, SUM(Total) AS Cantidad FROM detallesordencompra WHERE OrdfkOrdenCompra = '" + idordencompra + "'";
                        MySqlCommand cmd1 = new MySqlCommand(sql, co.dbconection());
                        MySqlDataReader dr1 = cmd1.ExecuteReader();
                        if (dr1.Read())
                        {
                            nrefacc = Convert.ToInt32(dr1.GetString("NumRefacc"));
                            cantidadrefacc = Convert.ToDouble(dr1.GetString("Cantidad"));
                        }
                        dr1.Close();
                        co.dbconection().Close();
                        double resociva = Convert.ToDouble(labelIVAOC.Text);
                        double totaloc = Convert.ToDouble(labelTotalOC.Text);
                        double subt = Convert.ToDouble(labelSubTotalOC.Text);
                        double i_v_a = Convert.ToDouble(v.getaData("SELECT coalesce(IVA,0)FROM ordencompra where FolioOrdCompra = '" + labelFolioOC.Text + "'").ToString());
                        resociva = subt * (i_v_a / 100);
                        labelIVAOC.Text = resociva.ToString();
                        metodorecid();
                        MySqlCommand cmd = new MySqlCommand("UPDATE ordencompra SET SubTotal = '" + subt + "',Total = '" + totaloc + "', Estatus = 'FINALIZADA',  PersonaFinal = '" + FCF.id+ "', ObservacionesOC='" + textBoxObservaciones.Text + "' WHERE FolioOrdCompra = '" + labelFolioOC.Text + "'", co.dbconection());
                        cmd.ExecuteNonQuery();
                        co.dbconection().Close();
                        cargafolio();
                        metodocargaorden();
                        actualizarcbx();
                        idfolio = 0;
                        metodocargadetordenPDF();
                        MessageBox.Show(v.mayusculas("La orden de compra se finalizó correctamente".ToLower()), "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarRefaccN();
                        buttonAgregar.Visible = true;
                        label9.Visible = true;
                        if (label35.Text == "EXPORTANDO")
                        { }
                        else
                        {
                            ocultarexcel();
                        }
                        buttonNuevoOC.Visible = false;
                        label17.Visible = false;
                        buttonPDF.Visible = false;
                        label3.Visible = false;
                        buttonFinalizar.Visible = false;
                        label34.Visible = false;
                        comboBoxFacturar.Enabled = true;
                        buttonExcel.Visible = false;
                        label38.Visible = false;
                        labelExistencia.Text = "";
                        Limpiar_labels();
                        cbcomparativa.Enabled = true;
                        dateTimePickerFechaEntrega.Enabled = true;
                    }
                }
            }
        }
        bool exportando = false;

        private void buttonExcel_Click(object sender, EventArgs e)
        {
            exportando = true;
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx2 = new Thread(excel);
            hiloEx2.Start();
        }
        public void _refacciones()
        {
            v.iniCombos("select coalesce((select upper(x1.nombreRefaccion) from crefacciones as x1 where x1.idrefaccion=t2.refaccionfkcrefacciones),upper(t2.nombreRefaccion)) as n,t1.idproveedorComparativa as id from proveedorescomparativa as t1 inner join refaccionescomparativa as t2 on t1.refaccionfkrefaccionesComparativa=t2.idrefaccioncomparativa inner join comparativas as t3 on t3.idcomparativa=t2.comparativafkcomparativas inner join cproveedores as t4 on t4.idproveedor=t1.proveedorfkcproveedores where t4.idproveedor='" + comboBoxProveedor.SelectedValue + "' and t3.idcomparativa='" + cbcomparativa.SelectedValue + "' and t1.mejoropcion='1'", comboBoxClave, "id", "n", "--SELECCIONE REFACCIÓN--");
        }
        /*Validaciones extras */
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void comboBoxProveedor_SelectedValueChanged(object sender, EventArgs e)
        {
            MySqlCommand validar = new MySqlCommand("SELECT correo, UPPER(CONCAT(aPaterno, ' ',coalesce( aMaterno,''), ' ', nombres)) AS representante, coalesce((paginaweb), '') AS paginaweb FROM cproveedores WHERE idproveedor = '" + comboBoxProveedor.SelectedValue + "'", co.dbconection());
            MySqlDataReader leer = validar.ExecuteReader();
            if (leer.Read())
            {
                labelCorreo.Text = leer["correo"].ToString();
                labelRepresentante.Text = leer["representante"].ToString();
                labelpaginaweb.Text = leer["paginaweb"].ToString();
            }
            else
            {
                if (!leer.Read())
                {
                    labelCorreo.Text = "";
                    labelRepresentante.Text = "";
                    labelpaginaweb.Text = "";
                }
            }
            leer.Close();
            co.dbconection().Close();

            if (banderaeditar == true)
            {
                validacionval();
                if (((idproveedoranterior == idvalidacionproveedor) || (comboBoxProveedor.SelectedIndex == 0)) && ((idfacturaranterior == idvalidacionfacturar) || (comboBoxFacturar.SelectedIndex == 0)) && ((observacionesanterior == textBoxObservaciones.Text.Trim())) && (dateTimePickerFechaEntrega.Value.Date == fentregaestimadanterior))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        private void comboBoxFacturar_SelectedValueChanged(object sender, EventArgs e)
        {
            if (banderaeditar == true)
            {
                validacionval();
                if (((idproveedoranterior == idvalidacionproveedor) || (comboBoxProveedor.SelectedIndex == 0)) && ((idfacturaranterior == idvalidacionfacturar) || (comboBoxFacturar.SelectedIndex == 0)) && ((observacionesanterior == textBoxObservaciones.Text.Trim())) && (dateTimePickerFechaEntrega.Value.Date == fentregaestimadanterior))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        private void textBoxAll_Leave(object sender, EventArgs e)
        {
            TextBox txtlea = sender as TextBox;
            if (string.IsNullOrWhiteSpace(txtlea.Text))
            {
                txtlea.Text = "0";
            }
        }

        private void buttonCatEmpresas_Click(object sender, EventArgs e)
        {
            aEmpresas();
        }
        void aEmpresas()
        {
            catEmpresas c = new catEmpresas(idUsuario, empresa, area);
            c.Owner = this;
            c.lbllogo.Visible = true;
            c.pblogo.Visible = true;
            DialogResult res = c.ShowDialog();
            if (res == DialogResult.Cancel)
            {
                if (Convert.ToInt32(comboBoxFacturar.SelectedValue) == this.res) comboBoxFacturar.SelectedIndex = 0;
            }
        }


        private void comboBoxClave_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand validar = new MySqlCommand("SELECT existencias, UPPER(nombreRefaccion) AS nombreRefaccion FROM crefacciones WHERE idrefaccion='" + comboBoxClave.SelectedValue + "'", co.dbconection());
            MySqlDataReader leer = validar.ExecuteReader();
            if (leer.Read())
            {
                labelExistencia.Text = leer["existencias"].ToString();
                labelDescripcion.Text = leer["nombreRefaccion"].ToString();
            }
            else
            {
                if (!leer.Read())
                {
                    labelExistencia.Text = "";
                    labelDescripcion.Text = "";
                }
            }
            leer.Close();
            co.dbconection().Close();
        }

        private void checkBoxFechas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFechas.Checked == true)
            {
                dateTimePickerIni.Enabled = true;
                dateTimePickerFin.Enabled = true;
                checkBoxFechasE.Checked = false;
            }
            else
            {
                dateTimePickerIni.Enabled = false;
                dateTimePickerFin.Enabled = false;
            }
        }

        private void checkBoxFechasE_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFechasE.Checked == true)
            {
                dateTimePickerIniBFE.Enabled = true;
                dateTimePickerFinBFE.Enabled = true;
                checkBoxFechas.Checked = false;
            }
            else
            {
                dateTimePickerIniBFE.Enabled = false;
                dateTimePickerFinBFE.Enabled = false;
            }
        }

        private void dateTimePickerAll_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void cbcomparativa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbcomparativa.SelectedIndex > 0)
            {
                v.iniCombos("select distinct UPPER(t1.empresa) as e, t1.idproveedor as id from cproveedores as t1 inner join proveedorescomparativa as t2 on t2.proveedorfkcproveedores=t1.idproveedor inner join refaccionescomparativa as t3 on t3.idrefaccioncomparativa=t2.refaccionfkrefaccionesComparativa inner join comparativas as t4 on t4.idcomparativa=t3.comparativafkcomparativas where t4.idcomparativa='" + cbcomparativa.SelectedValue + "' and t2.mejorOpcion='1' order by t1.empresa ;", comboBoxProveedor, "id", "e", "--SELECCIONE UN PROVEEDOR");
                comboBoxProveedor.Enabled = true;
            }
            else
            {
                comboBoxProveedor.DataSource = null;
                comboBoxProveedor.Enabled = false;
            }
        }

        private void comboBoxClave_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxClave.SelectedIndex > 0)
            {
                string[] datos = v.getaData("select distinct concat(t1.precioUnitario,';',t3.cantidad) from proveedorescomparativa as t1 inner join cproveedores as t2 on t2.idproveedor=t1.proveedorfkcproveedores inner join refaccionescomparativa as t3 on t1.refaccionfkrefaccionesComparativa=t3.idrefaccioncomparativa left join crefacciones as t4 on t4.idrefaccion=t3.refaccionfkcrefacciones inner join comparativas as t5 on t5.idcomparativa=t3.comparativafkcomparativas where t5.idcomparativa='" + cbcomparativa.SelectedValue + "' and t2.idproveedor='" + comboBoxProveedor.SelectedValue + "' and t1.idproveedorComparativa='" + comboBoxClave.SelectedValue + "';").ToString().Split(';');
                lblPrecio.Text = datos[0];
                lblCantidad.Text = datos[1];
                labelSubTotal.Text = (Convert.ToDouble(datos[0]) * (Convert.ToDouble(datos[1]))).ToString();
            }
        }

        private void comboBoxProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProveedor.SelectedIndex > 0)
            {
                _refacciones();
                comboBoxClave.Enabled = true;
            }
            else
            {
                comboBoxClave.DataSource = null;
                comboBoxClave.Enabled = false;
            }
        }

        private void textBoxAll_Click(object sender, EventArgs e)
        {
            TextBox txtA = sender as TextBox;
            txtA.SelectAll();
        }

        /* Validaciones de los campos*/
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void textBoxCantidades_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Solo se pueden introducir números y un solo punto decimal", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ya existe un punto decimal en la caja de texto", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lbltitulo_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void comboBoxClave_Click(object sender, EventArgs e)
        {
            if (comboBoxClave.Text != "-- SELECCIONE UNA OPCIÓN --") comboBoxClave.Text = "";
        }

        private void textBoxOCompraB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != 79 && e.KeyChar != 67 && e.KeyChar != 111 && e.KeyChar != 99)
            {
                e.Handled = true;
                MessageBox.Show("Solo se pueden introducir números, un solo guion y las letras O y C", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ya existe un guion en la caja de texto", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBoxClave_Validating(object sender, CancelEventArgs e)
        {
            //buscarRefaccion(comboBoxClave.Text);
        }

        private void textBoxLargo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsLetter(e.KeyChar)) || (Char.IsNumber(e.KeyChar)) || (e.KeyChar == 32) || (e.KeyChar == 44) || (e.KeyChar == 46) || (e.KeyChar == 47) || (e.KeyChar == 8) || (e.KeyChar == 127))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo puede ingresar números, letras, comas y puntos en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridViewOCompra_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /* Movimiento de los botónes */
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void OrdenDeCompra_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hiloEx2 != null)
                hiloEx2.Abort();
            if ((Directory.Exists(Application.StartupPath + "/PDFTempral"))) try { Directory.Delete(Application.StartupPath + "/PDFTempral", true); }
                catch { return; }
        }

        private void comboBoxFacturar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pverEmpresa)
                {
                    ComboBox cb = sender as ComboBox;
                    int? res1 = (int?)cb.SelectedValue ?? 0;
                    if (res1 == this.res)
                    {
                        aEmpresas();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5451_MouseLeave(object sender, EventArgs e)
        {
            Button btn54 = sender as Button;
            btn54.Size = new Size(54, 51);
        }

        private void buttonAll_MouseLeave(object sender, EventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(54, 54);
        }

        private void buttonAll_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(59, 59);
        }

        private void buttonAllb_MouseLeave(object sender, EventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(44, 44);
        }

        private void buttonAllb_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(49, 49);
        }

        private void buttonCatEmpresas_MouseMove(object sender, MouseEventArgs e)
        {
            buttonCatEmpresas.Size = new Size(45, 45);
        }

        private void buttonCatEmpresas_MouseLeave(object sender, EventArgs e)
        {
            buttonCatEmpresas.Size = new Size(40, 40);
        }

        void comboBoxAll_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void dataGridViewOCompra_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridViewOCompra.Columns[e.ColumnIndex].Name == "ESTATUS")
            {
                if (Convert.ToString(e.Value) == "FINALIZADA")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
            }
        }

        private void dataGridViewAll_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void textBoxAll_Validated(object sender, EventArgs e)
        {
            TextBox txtb = sender as TextBox;
            while (txtb.Text.Contains("  "))
            {
                txtb.Text = txtb.Text.Replace("  ", " ");
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
                    {
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }
        public void combos_DrawItem(object sender, DrawItemEventArgs e)
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
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                    }
                }
            }
        }

        private void dateTimePickerFechaEntrega_ValueChanged(object sender, EventArgs e)
        {
            if (banderaeditar == true)
            {
                validacionval();
                if (((idproveedoranterior == idvalidacionproveedor) || (comboBoxProveedor.SelectedIndex == 0)) && ((idfacturaranterior == idvalidacionfacturar) || (comboBoxFacturar.SelectedIndex == 0)) && ((observacionesanterior == textBoxObservaciones.Text.Trim())) && (dateTimePickerFechaEntrega.Value.Date == fentregaestimadanterior))
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        private void dataGridViewOCompra_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                banderaeditar = false;
                if ((pinsertar == true) && (peditar == true) && (pconsultar == true) && (pdesactivar == true))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ContextMenuStrip mn = new System.Windows.Forms.ContextMenuStrip();
                        int xy = dataGridViewOCompra.HitTest(e.X, e.Y).RowIndex;
                        if (xy >= 0)
                        {
                            mn.Items.Add("Editar".ToUpper(), controlFallos.Properties.Resources.pencil).Name = "Editar".ToUpper();
                        }
                        mn.Show(dataGridViewOCompra, new Point(e.X, e.Y));

                        mn.ItemClicked += new ToolStripItemClickedEventHandler(mn_ItemClicked);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void mn_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            validacionval();
            if (((idproveedoranterior == idvalidacionproveedor) || (comboBoxProveedor.SelectedIndex == 0)) && ((idfacturaranterior == idvalidacionfacturar) || (comboBoxFacturar.SelectedIndex == 0)) && ((textBoxObservaciones.Text.Trim() == observacionesanterior)))
            {
                switch (e.ClickedItem.Name.ToString())
                {
                    case "EDITAR":

                        editargroup();
                        metodocargadetordenPDF();
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

                            editargroup();
                            metodocargadetordenPDF();
                            break;

                        default:

                            MessageBox.Show("DEFAULT");

                            break;
                    }
                }
            }
        }

        private void groupBoxAll_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void groupBoxRefaccion_Enter(object sender, EventArgs e)
        {

        }

        private void buttonActualizarN_Click(object sender, EventArgs e)
        {
            NombresOC noc = new NombresOC(empresa, area,pictureBox1.BackgroundImage);
            noc.Owner = this;
            noc.ShowDialog();
        }

        private void label61_Click(object sender, EventArgs e)
        {

        }
        private void escribirFichero(string texto)
        {
            //obtenemos la carpeta y ejecutable de nuestra aplicación 
            string rutaFichero = Application.StartupPath; ;
            //primer parámetro que es el código de solicitud 
            rutaFichero = rutaFichero + "/PDFTempral";
            try
            {
                //si no existe la carpeta temporal la creamos 
                if (!(Directory.Exists(rutaFichero)))
                {
                    Directory.CreateDirectory(rutaFichero);

                }
            }
            catch (Exception errorC)
            {
                MessageBox.Show("Ha habido un error al intentar " +
                         "crear el fichero temporal:" +
                         Environment.NewLine + Environment.NewLine +
                         rutaFichero + Environment.NewLine +
                         Environment.NewLine + errorC.Message,
                         "Error al crear fichero temporal",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonEditar_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxClave_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (banderaeditar == true)
            {
                if (comboBoxClave.Text == codigorefanterior && textBoxObservacionesRefacc.Text.Trim() == observacionesrefaccanterior /*&& preciocotizadoanterior == valorpreciocotizado && cantidadsolicitadanterior == valorcantidadsolicitada*/)
                {
                    buttonEditar.Visible = false;
                    label8.Visible = false;
                }
                else
                {
                    buttonEditar.Visible = true;
                    label8.Visible = true;
                }
            }
        }

        private void textBoxObservacionesRefacc_TextChanged(object sender, EventArgs e)
        {
            validacioneditar();
        }

        private void dataGridViewOCompra_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewOCompra.CurrentCell = dataGridViewOCompra.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }
        void buscarRefaccion(string texto)
        {
            bool res = false;
            DataTable dataRefaccion = (DataTable)comboBoxClave.DataSource;
            for (int i = 1; i < dataRefaccion.Rows.Count; i++)
            {
                string[] resul = dataRefaccion.Rows[i].ItemArray[0].ToString().Trim().Split('-');

                if (texto.Equals(resul[0].Trim()) || texto.Equals(resul[1].Trim()))
                {
                    comboBoxClave.SelectedValue = dataRefaccion.Rows[i].ItemArray[1];
                    res = true;
                    return;
                }
            }
            if (!res) comboBoxClave.SelectedIndex = 0;
        }

        private void linkLabelAgregarRef_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (comboBoxClave.Visible)
            {
                comboBoxClave.SelectedIndex = 0;
                label1.Text = "NOMBRE DE REFACCIÓN";
                comboBoxClave.Visible = false;
            }
        }

        private void textBoxRefaccion_TextChanged(object sender, EventArgs e)
        {
            validacioneditar();
        }

        private void textBoxRefaccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("Sólo se Aceptan Letras y Números En Este Campo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Handled = true;
            }
        }
    }
}