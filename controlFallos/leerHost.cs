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
using System.IO;

namespace controlFallos
{
    public partial class leerHost : Form
    {
        validaciones v = new validaciones();

        public leerHost()
        {
            InitializeComponent();
            lbltitle.Left = (status.Width - lbltitle.Width) / 2;
            lbltitle.Top = (status.Height - lbltitle.Height) / 2;
        }
        public bool tryconection()
        {
            try
            {
                MySqlConnection dbcon = new MySqlConnection("Server = " + txtgethost.Text + "; user=" + txtgetusu.Text + "; password = " + txtgetpass.Text + "; database =sistrefaccmant;");
                dbcon.Open();
                dbcon.Close();
                dbcon.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgethost.Text) && !string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text))
            {
                if (tryconection())
                {
                    MessageBox.Show("Conexion Exitosa", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }else
            {
                MessageBox.Show("Todos los Campos Son Obligatorios", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgethost.Text) && !string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text))
            {

                if (tryconection())
            {
                try
                {
                    StreamWriter esctritor = new StreamWriter(Application.StartupPath + @"\conexion.txt");
                    string conexion = v.Encriptar(txtgethost.Text) + ";" + v.Encriptar(txtgetusu.Text) + ";" + v.Encriptar(txtgetpass.Text) + ";yh8s87np3dFG012MLX28Yg==";
                    esctritor.WriteLine(conexion);
                    esctritor.Close();
                        Application.Restart();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                }
            }
            else
            {
                MessageBox.Show("Todos los Campos Son Obligatorios", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtgethost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar!=39)
            {
                e.Handled = false;
            }else
            {
                e.Handled = true;
            }
        }
    }
}
