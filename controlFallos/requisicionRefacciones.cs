using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class requisicionRefacciones : Form
    {
        validaciones v;
        Point arreglar = new Point(33, 273);
        Point locationInitial = new Point(348, 6);
        Size initialSize = new Size(800, 397);
        Point asolicitar = new Point(348, 6);
        Point solicitadas = new Point(348, 205);
        bool editar;
        int idReporte;
        int RowDataRefaccionesASolicitar;
        public requisicionRefacciones(int idReporte, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idReporte = idReporte;
            cbxFamilia.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxFRefaccion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbxFamilia.DrawItem += v.combos_DrawItem;
            cbxFRefaccion.DrawItem += v.combos_DrawItem;
            iniFamilies();
            lbltitle.Text = "Requisición de Refacciones Para El Reporte: " + v.getaData("SELECT Folio FROM reportesupervicion WHERE idReporteSupervicion='" + idReporte + "'");
            lbltitle.Left = (panel2.Width - lbltitle.Width) / 2;
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM pedidosrefaccion WHERE FolioPedfkSupervicion='" + idReporte + "'")) > 0)
            {
                gbxRefaccionesSolicitadas.Location = locationInitial;
                gbxRefaccionesSolicitadas.Size = initialSize;
                gbxRefaccionesSolicitadas.Visible = true;
            }
            else
            {
                gbxRefaccionesASolicitar.Location = locationInitial;
                gbxRefaccionesASolicitar.Size = initialSize;
                gbxRefaccionesASolicitar.Visible = true;
            }
        }

        void iniFamilies() { v.iniCombos("SELECT idcnFamilia AS id, Familia as nombre FROM cnfamilias WHERE status=1 ORDER BY Familia ASC", cbxFamilia, "id", "nombre", "-- SELECCIONE FAMILIA --"); }
        private void cbxFamilia_SelectedIndexChanged(object sender, EventArgs e) { if (cbxFamilia.SelectedIndex > 0) { v.iniCombos("SELECT t1.idrefaccion as id , UPPER(t1.nombreRefaccion) as nombre  FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas = t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias = t3.idfamilia WHERE t3.familiafkcnfamilias = '" + cbxFamilia.SelectedValue + "';", cbxFRefaccion, "id", "nombre", "-- SELECCIONE REFACCIÓN --"); cbxFRefaccion.Enabled = true; } else { cbxFRefaccion.DataSource = null; cbxFRefaccion.Enabled = false; } }
        private void cbxFRefaccion_SelectedIndexChanged(object sender, EventArgs e) { if (cbxFRefaccion.SelectedIndex > 0) lblUM.Text = v.getaData("SELECT Simbolo  FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas = t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias = t3.idfamilia INNER JOIN cunidadmedida as t4 ON t3.umfkcunidadmedida = t4.idunidadmedida WHERE t1.idrefaccion='" + cbxFRefaccion.SelectedValue + "'").ToString(); else lblUM.Text = null; }
        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e) { v.numerosDecimales(e); }
        private void txtCantidad_Validating(object sender, CancelEventArgs e) {
            if (!string.IsNullOrWhiteSpace(txtCantidad.Text.Trim()))
            {
             while (txtCantidad.Text.Contains(".."))
                    txtCantidad.Text = txtCantidad.Text.Replace("..", ".").Trim();
                txtCantidad.Text = txtCantidad.Text.Trim();
                txtCantidad.Text = string.Format("{0:F2}", txtCantidad.Text);
                try { if (Convert.ToDouble(txtCantidad.Text) > 0) {
                        CultureInfo ti = new CultureInfo("es-MX"); ti.NumberFormat.CurrencyDecimalDigits = 2; ti.NumberFormat.CurrencyDecimalSeparator = "."; txtCantidad.Text = string.Format("{0:N2}", Convert.ToDouble(txtCantidad.Text, ti)); } else txtCantidad.Text = "0"; } catch (Exception ex) { txtCantidad.Clear(); MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAgregarRefaccion_Click(object sender, EventArgs e)
        {
            if (!v.camposVaciosSolicitudRefacciones(cbxFamilia.SelectedIndex, cbxFRefaccion.SelectedIndex, txtCantidad.Text))
            {
                if (!editar) insertar();
            }
        }
        void insertar()
        {
          
                if (!existe(Convert.ToInt32(cbxFRefaccion.SelectedValue)) )
                {
                    dgvRefaccionesaSolicitar.Rows.Add(new object[] { cbxFRefaccion.SelectedValue, cbxFRefaccion.Text, txtCantidad.Text });
                    limpiar();
                }
                else
                {
                    MessageBox.Show("La Refacción Ya Se Encuentra en Fila Para Solicitar\nPuede Editar La Cantidad Seleccionando La Refacción de La Tabla \"Refacciones A Solicitar\" ", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbxFRefaccion.Focus();
                }
        }
        void limpiar()
        {
            cbxFamilia.SelectedIndex = 0;
            txtCantidad.Clear();
            editar = false;
            dgvRefaccionesaSolicitar.ClearSelection();
        }
        bool existe(int refaccion)
        {
            foreach (DataGridViewRow row in dgvRefaccionesaSolicitar.Rows)
                if (Convert.ToInt32(row.Cells[0].Value) == refaccion) return true;
            return false;
        }

        private void requisicionRefacciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormFallasMantenimiento ffm = (FormFallasMantenimiento)Owner;
            //if (dgvRefaccionesaSolicitar.Rows.Count>0)
            //{
            //    FormContraFinal fc = new FormContraFinal(2, 1, this);
            //    fc.Owner = this;
            //    fc.LabelTitulo.Text = "Introduzca su Contraseña para Completar\nLa Solicitud de Refaccíones";
            //    if (fc.ShowDialog()== DialogResult.OK)
            //    {
            //        object id = fc.id;
            //        foreach (DataGridViewRow row in dgvRefaccionesaSolicitar.Rows)
            //            v.c.insertar(string.Format("INSERT INTO pedidosrefaccion(FolioPedfkSupervicion, RefaccionfkCRefaccion, Cantidad,  usuariofkcpersonal) VALUES({0},{1},{2},{3})",new object[] {idReporte,row.Cells[0].Value,row.Cells[2].Value,id}));
                   
            //        //ffm.cbxRequierenRefacciones.Enabled = false;
            //        //ffm.cbxExistenciaRefacciones.Enabled = false;
            //        //ffm.cbxExistenciaRefacciones.SelectedIndex = 1;
            //    } else
            //    {
            //        if (MessageBox.Show("Si Cierra El Formulario Se Borrarán las Refacciones a Solicitar ¿Desea Continuar?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //            DialogResult = DialogResult.None;
            //        //else
            //            //ffm.cbxRequierenRefacciones.SelectedIndex = 1;
                       
            //    }

            }
        }
    }
