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
    public partial class recibirCredencial : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuarioTemp, idUsuario, empresa, area;
        public recibirCredencial(string credencial,int idUsuarioTemp,int idUsuario,int empresa,int area)
        {
            InitializeComponent();
            lblmsg.Text = "La Credencial Num. "+credencial+" ya está en uso. Ingrese una nueva";
            this.idUsuarioTemp = idUsuarioTemp;
            this.idUsuario= idUsuario;
            this.empresa = empresa;
            this.area= area;
        }

        private void recibirCredencial_Load(object sender, EventArgs e)
        {

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtgetcredencial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnguardar_Click(null, e);
            else
                v.Solonumeros(e);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnguardar_Click_1(object sender, EventArgs e)
        {
            if (!v.existecredencialEmpleado(txtgetcredencial.Text) )
            {
                if (Convert.ToInt32(txtgetcredencial.Text) > 0) {
                    if (v.c.insertar("UPDATE cpersonal SET credencial='" + txtgetcredencial.Text + "' WHERE idpersona =" + this.idUsuarioTemp))
                    {
                        MessageBox.Show("Credencial modificada Exitosamente.", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Close();
                    }
                }else
                {
                    MessageBox.Show("La Credencial No Puede Ser Igual a \"0\"", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
