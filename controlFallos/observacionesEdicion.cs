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
    public partial class observacionesEdicion : Form
    {
        validaciones v;
        public observacionesEdicion(validaciones v)
        {
            InitializeComponent();
            this.v = v;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtgetedicion.Text.Trim()))
            {
                MessageBox.Show("El Motivo de Edición No Puede Estar Vacío",validaciones.MessageBoxTitle.Error.ToString(),MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void txtgetedicion_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetedicion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (!string.IsNullOrWhiteSpace(txtgetedicion.Text.Trim()))
                    button2_Click(null, e);
                else
                    e.Handled = true;
            }
            else
            {
                v.enGeneral(e);
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
    }
}
