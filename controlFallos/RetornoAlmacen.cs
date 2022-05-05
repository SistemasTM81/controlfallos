using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class RetornoAlmacen : Form
    {
        public string id = "", cantmantL = "", refaL = "";
        int empresa, area, cantRealRef, idRefk;
        validaciones val;
        public RetornoAlmacen(int empresa, int area, validaciones v, string refa, string cantmant, int cantReal, int idRef)
        {
            this.empresa = empresa; this.area = area;
            this.cantmantL = cantmant; this.refaL = refa; this.idRefk = idRef;
            this.cantRealRef = cantReal;
            this.val = v;
            InitializeComponent();
            cmbEstatus.DrawItem += v.combos_DrawItem;
        }

        private void RetornoAlmacen_Load(object sender, EventArgs e)
        {
            lblCantMant.Text = cantmantL;
            lblRef.Text = refaL;
            cargaCombo();
            lblMotivo.Visible = txtMotivo.Visible = (verificaPrimerReg() > 0) ? false : true;
        }
        void cargaCombo()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("id");
            tabla.Columns.Add("Estatus");
            string[] arrayk = { "--SELECCIONE--", "CORRECTO", "INCORRECTO" };
            for (int i = 0; i < arrayk.Length; i++)
            {
                DataRow row = tabla.NewRow();
                row["id"] = i;
                row["Estatus"] = arrayk[i];
                tabla.Rows.Add(row);
            }
            cmbEstatus.DataSource = tabla;
        }
        int verificaPrimerReg() 
        {
            return Convert.ToInt32(val.getaData("select count(*) from refacciones_standby where fechahoraR is null and refaccionfkpedidosRefaccion='" + idRefk + "';"));
        }


        private void txtContra_TextChanged(object sender, EventArgs e)
        {
            busquedageneral("SELECT t1.idPersona, UPPER(CONCAT(coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + val.Encriptar(txtContra.Text) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'");
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblUsuario.Text) && cmbEstatus.SelectedIndex>-1 &&!string.IsNullOrWhiteSpace(txtcantidad.Text) && Convert.ToInt32(txtcantidad.Text) > 0)
            {
                if (lblMotivo.Visible == true)
                {
                    if (!string.IsNullOrWhiteSpace(txtMotivo.Text))
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        plasmaNegativa();
                    }
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
            else
            {
                plasmaNegativa();
            }
        }
        void plasmaNegativa() 
        {
            MessageBox.Show("Verifique su información", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            id = "";
            txtContra.Clear(); lblUsuario.Text = "";
            txtContra.Focus();
            DialogResult = DialogResult.None;
            DialogResult = DialogResult.None;
        }
        private void txtcantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            val.numerosDecimales(e);
        }

        private void txtContra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                buttonAceptar_Click(null, e);
            else
                val.letrasynumerossinespacios(e);
        }

        private void txtcantidad_Validating(object sender, CancelEventArgs e)
        {
           
        }

        private void txtcantidad_TextChanged(object sender, EventArgs e)
        {
            if (lblCantMant.Text != txtcantidad.Text && !string.IsNullOrWhiteSpace(txtcantidad.Text))
            {
                if (!validacantidadReal())
                {
                    txtcantidad.Text = "";
                }
                if (!string.IsNullOrWhiteSpace(txtcantidad.Text))
                {
                    if (Convert.ToInt32(txtcantidad.Text) != Convert.ToInt32(lblCantMant.Text))
                    {
                        cmbEstatus.SelectedIndex = 2;
                    }
                }
            }
        }
        bool validacantidadReal()
        {
            return (cantRealRef >= Convert.ToInt32(txtcantidad.Text)  && Convert.ToInt32(txtcantidad.Text) > 0 ? true : false);
        }
        private void txtObservacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            val.enGeneral(e);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public void busquedageneral(string consulta)
        {
            MySqlCommand busqueda = new MySqlCommand(consulta, val.c.dbconection());
            MySqlDataReader dr = busqueda.ExecuteReader();
            if (dr.Read())
            {
                id = dr.GetString(0);
                lblUsuario.Text = dr.GetString(1);
            }
            else
                lblUsuario.Text = id = "";

            dr.Close();
            val.c.dbcon.Close();
        }
    }
}