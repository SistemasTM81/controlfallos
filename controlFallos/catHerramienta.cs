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
    public partial class catHerramienta : Form
    {
        validaciones v;
        int empresa, area, usuario;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        public catHerramienta(int empresa, int area, int usuario, validaciones v)
        {
            this.v = v;
            this.empresa = empresa;
            this.area = area;
            this.usuario = usuario;
            InitializeComponent();
        }

        void iniciarFamilia()
        {
            v.iniCombos("select idcfamiliaherramientas AS id, familia as familia from cfamiliaherramientas ORDER BY familia ASC", cbfamilia, "id", "familia", "-- SELECCIONE FAMILIA --");
        }
        void iniciarMarca()

        {
            v.iniCombos("select idcmarcasherramienta AS id, marca as marca from cmarcasherramienta ORDER BY marca ASC", cbmarcas, "id", "marca", "-- SELECCIONE MARCA --");
        }
        void iniMonedaCambio()
        {
            v.iniCombos("select idTipoCambio as id, Simbolo from ctipocambio order by simbolo desc", cmbMoneda, "id", "Simbolo", "--MONEDA--");
        }
        void iniciarProveedor()
        {
            v.iniCombos("SET lc_time_names = 'es_ES';select convert(t1.idproveedor,char) as idunidad, convert(if(t1.empresa = '',concat(t1.aPaterno, ' ', t1.aMaterno, ' ', t1.nombres) , t1.empresa),char) as Nombre from cproveedores as t1 inner join cempresas as t2 on t1.empresaS = t2.idempresa where t1.empresaS = '" + empresa + "' order by idproveedor asc", cmbPROVEEDOR, "idunidad", "Nombre", "-- SELECCIONE PROVEEDOR --");
        }
        public void establecerPrivilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", usuario, "catRefacciones")).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                if (Convert.ToInt32(privilegiosTemp.Length) > 3)
                {
                    Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }

            }
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddrefaccion.Visible = true;
            }
            if (Pconsultar)
            {
                gbbuscar.Visible = true;
            }
            if (Peditar)
            {
                label5.Visible = true;
                label6.Visible = true;
            }
        }
        void iniubicaciones()
        {
            v.iniCombos("SELECT idpasillo,UPPER(pasillo) AS pasillo FROM cpasillos WHERE status='1' and empresa='" + empresa + "' ORDER BY pasillo ASC", cbpasillo, "idpasillo", "pasillo", "-- SELECCIONE PASILLO --");
        }

        void GuardarDatos()
        {
            v.c.insertar("");
        }
        private void validcacionNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button11_Click(null, e);
            else
            {
                TextBox txtKilometraje = sender as TextBox;
                char signo_decimal = (char)46;
                if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == 46 || e.KeyChar == 44)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("Solo se aceptan: numéros y ( , ) en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (e.KeyChar == 46)
                {
                    if (txtKilometraje.Text.LastIndexOf(signo_decimal) >= 0)
                    {
                        e.Handled = true; // Interceptamos la pulsación 
                    }
                }
            }
        }
        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox txtCantidad = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(txtCantidad.Text.Trim()))
            {
                while (txtCantidad.Text.Contains(".."))
                    txtCantidad.Text = txtCantidad.Text.Replace("..", ".").Trim();
                txtCantidad.Text = txtCantidad.Text.Trim();
                txtCantidad.Text = string.Format("{0:F2}", txtCantidad.Text);
                try
                {
                    if (Convert.ToDouble(txtCantidad.Text) > 0)
                    {
                        CultureInfo ti = new CultureInfo("es-MX"); ti.NumberFormat.CurrencyDecimalDigits = 2; ti.NumberFormat.CurrencyDecimalSeparator = "."; txtCantidad.Text = string.Format("{0:N2}", Convert.ToDouble(txtCantidad.Text, ti));
                    }
                    else txtCantidad.Text = "0";
                }
                catch (Exception ex)
                {
                    txtCantidad.Clear(); MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void catHerramienta_Load(object sender, EventArgs e)
        {
            iniciarFamilia();
            iniciarMarca();
            iniciarProveedor();
            iniMonedaCambio();
            iniubicaciones();
            v.comboswithuot(cmbTipo, new string[] { "--SELECCIONA TIPO--", "TRANSINSUMOS", "TRANSMASIVO", "PRODUCCION" });
        }

        private void btnFamilias(object sender, EventArgs e)
        {

        }
        private void button11_Click(object sender, EventArgs e)
        {
            
        }
        private void cbmarcas_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void cmbMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void txtCostoUni_TextChanged(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cniveles where pasillofkcpasillos ='" + cbpasillo.SelectedValue + "' and empresa='" + empresa + "'")) > 0)
            {
                v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE status='1' and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "' and empresa='" + empresa + "' ORDER BY nivel ASC", cbnivel, "idnivel", "nivel", "--SELECCIONE UN NIVEL");
                cbnivel.Enabled = true;
            }
            else
            {

                cbnivel.DataSource = null;
                cbnivel.Enabled = false;
            }
        }

        private void cbnivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM canaqueles where nivelfkcniveles ='" + cbnivel.SelectedValue + "' and empresa='" + empresa + "'")) > 0)
            {

                v.iniCombos("SELECT idanaquel,UPPER(anaquel) AS anaquel FROM canaqueles WHERE status='1' and nivelfkcniveles= '" + cbnivel.SelectedValue + "' and empresa='" + empresa + "' ORDER BY anaquel ASC", cbanaquel, "idanaquel", "anaquel", "--SELECCIONE UN ANAQUEL");
                cbanaquel.Enabled = true;
            }
            else
            {

                cbanaquel.DataSource = null;
                cbanaquel.Enabled = false;
            }
        }

        private void cbanaquel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM ccharolas where anaquelfkcanaqueles='" + cbanaquel.SelectedValue + "' and empresa='" + empresa + "'")) > 0)
            {

                v.iniCombos("SELECT idcharola,UPPER(charola) AS charola FROM ccharolas WHERE status='1' and anaquelfkcanaqueles= '" + cbanaquel.SelectedValue + "' and empresa='" + empresa + "' ORDER BY charola ASC", cbcharola, "idcharola", "charola", "--SELECCIONE UN ANAQUEL");
                cbcharola.Enabled = true;
            }
            else
            {

                cbcharola.DataSource = null;
                cbcharola.Enabled = false;
            }
        }

        private void txtnombrereFaccion_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtnombrereFaccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button11_Click(null, e);
            else
                v.enGeneral(e);
        }

        private void btndelref_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void gbaddrefaccion_Enter(object sender, EventArgs e)
        {

        }
    }
}

//Prueba Jesus subir git