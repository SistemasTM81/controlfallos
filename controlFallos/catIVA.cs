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

namespace controlFallos
{
    public partial class catIVA : Form
    {
        int empresa, area;

        public catIVA(int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
        }
   
        validaciones v;
        int? idusu;
        double ivabd, moneda_anterior;

        private void catIVA_Load(object sender, EventArgs e)
        {
            cmbMoneda.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            ivabd = Convert.ToDouble((v.getaData("SELECT iva FROM civa where empresa='"+empresa+"'") ?? 0));
            textBoxIVA.Text = ivabd.ToString();
            iniMonedaCambio();

        }
        void iniMonedaCambio()
        {
            v.iniCombos("select idTipoCambio as id, Simbolo from ctipocambio order by simbolo desc", cmbMoneda, "id", "Simbolo", "--MONEDA--");
        }
        // TODOS LOS MÉTODOS //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void limpiar()
        {
            txtMoneda.Text = "";
            textBoxUsuario.Text = "";
            buttonEditar.Visible = true;
            label3.Visible = true;
            iniMonedaCambio();
            txtMoneda.Text = "";
        }
        // ACCIONES CON LOS BOTONES ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void buttonEditar_Click(object sender, EventArgs e)
        {
            observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                if (!ivabd.ToString().Equals(textBoxIVA.Text))
                {
                    if (ivabd == 0) v.c.insertar("INSERT INTO civa (IVA,personaFKcpersonal,empresa) VALUES('" + Convert.ToDouble(textBoxIVA.Text) + "','" + idusu + "','" + empresa + "')"); else v.c.insertar("UPDATE civa SET iva = '" + Convert.ToDouble(textBoxIVA.Text) + "'");

                    if (v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimamodificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion, empresa, area) VALUES('Catalogo De I.V.A.', '1', '" + ivabd + "', '" + idusu + "', now(), 'Modificación De IVA','" + observaciones + "', '2', '2')"))
                    {
                        menuPrincipal m = (menuPrincipal)Owner;

                        if (m.form != null && m.form.GetType() == typeof(OrdenDeCompra))
                        {
                            OrdenDeCompra c1 = (OrdenDeCompra)m.form;
                            c1.metodocargaiva();
                        }
                        MessageBox.Show("El IVA Se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Close();
                    }
                }
                else
                {
                    if (cmbMoneda.SelectedIndex > 0)
                    {
                        if (!txtMoneda.Text.Equals(moneda_anterior))
                        {
                            v.c.insertar("update ctipocambio set costo = '" + txtMoneda.Text + "' where idtipoCambio = '" + cmbMoneda.SelectedValue + "'");
                            v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimamodificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion, empresa, area) VALUES('Catalogo De I.V.A.', '" + cmbMoneda.SelectedIndex + "', '" + moneda_anterior + "', '" + idusu + "', now(), 'Modificación De MONEDA','" + observaciones + "', '2', '2')");
                        }
                    }
                }
            }
            
        }
        // VALIDACIONES EN LAS CAJAS DE TEXTO Y/O LISTAS DESPLEGABLES /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void textBoxIVA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Solo se admiten números y un solo punto decimal", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("No puede poner otro punto decimal", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // DISEÑO DE TODO EL FORMULARIO ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void buttonEditar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonEditar.Size = new Size(59, 56);
        }

        /// <summary>
        /// EY
        /// </summary>
        /// <param name="sender">Apoco ao</param>
        /// <param name="e"></param>
        private void buttonEditar_MouseLeave(object sender, EventArgs e)
        {
            buttonEditar.Size = new Size(54, 51);
        }
        private void getCambios(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(textBoxUsuario.Text))
                    idusu =Convert.ToInt32(v.getaData("SELECT COALESCE(t1.idPersona,'0') FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos AS t3 ON t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(textBoxUsuario.Text) + "'AND t1.empresa='" + empresa + "' AND  t1.area='" + area + "'")); else idusu = null;
                double ivaActual = 0;
                if (!string.IsNullOrWhiteSpace(txtMoneda.Text)) ivaActual =Convert.ToDouble(txtMoneda.Text.Trim());
                if ((!string.IsNullOrWhiteSpace(idusu.ToString()) && Convert.ToInt32(idusu) > 0 && ivaActual > 0) && ivabd != ivaActual && ivaActual<100) buttonEditar.Visible = label3.Visible = true; else buttonEditar.Visible = label3.Visible = false;

            }
            catch 
            {
                txtMoneda.Text = "0.";
                txtMoneda.SelectionStart = txtMoneda.Text.Length;
            }
        }
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void catIVA_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM civa")) == 0)
            {
                menuPrincipal m = (menuPrincipal)Owner;
                if (m.form != null)
                {
                    if (m.form.GetType() == typeof(OrdenDeCompra))
                    {
                        MessageBox.Show("Para Acceder a Requisiciones debe Agregar El Porcentaje de Iva", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void textBoxUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) buttonEditar_Click(null, e);
        }

        private void groupBoxEdicion_Enter(object sender, EventArgs e)
        {

        }

        private void cmbMoneda_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbMoneda_SelectedValueChanged(object sender, EventArgs e)
        {
          
        }

        private void cmbMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMoneda.SelectedIndex > 0)
            {
               moneda_anterior = Convert.ToDouble((v.getaData("SELECT Costo FROM ctipocambio where idtipoCambio='" + cmbMoneda.SelectedValue + "'") ?? 0));
               txtMoneda.Text = Convert.ToString(moneda_anterior);
            }
            
        }

        private void groupBoxEdicion_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        
            
    }
}