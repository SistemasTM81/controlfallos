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
    public partial class NombresOC : Form
    {
        int empresa, area, id = 0;
        string alm, aut, iddb;
        conexion c = new conexion();
        validaciones val = new validaciones();

        public NombresOC(int empresa, int area, Image logo)
        {
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
        }

        private void textBoxContras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsNumber(e.KeyChar)) || (Char.IsLetter(e.KeyChar) || (e.KeyChar == 8) || (e.KeyChar == 127)))
            {
                e.Handled = false;
            }
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

        private void textBoxNombres_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((Char.IsLetter(e.KeyChar)) || (e.KeyChar == 8) || (e.KeyChar == 32) || (e.KeyChar == 127))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan letras en este campo", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void limpiar()
        {
            textBoxAlmacen.Text = "";
            textBoxAutoriza.Text = "";
            textBoxUsuario.Text = "";
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if((id != 0) && (!(string.IsNullOrWhiteSpace(textBoxAlmacen.Text)) || !(string.IsNullOrWhiteSpace(textBoxAutoriza.Text))))
            {
                if(iddb == "0" && alm == "" && aut == "" && id != 0 && !string.IsNullOrWhiteSpace(textBoxAlmacen.Text) && !string.IsNullOrWhiteSpace(textBoxAutoriza.Text))
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO nombresoc(Almacen, Autoriza, personafkcpersonal,empresa) VALUES('" + textBoxAlmacen.Text + "', '" + textBoxAutoriza.Text + "', '" + id + "','"+empresa+"')", c.dbconection());
                    cmd.ExecuteNonQuery();
                    c.dbconection().Close();
                    MessageBox.Show("Nombres ingresados correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(alm != "" && aut != "")
                {
                    if (!(string.IsNullOrWhiteSpace(textBoxAlmacen.Text)) && !(string.IsNullOrWhiteSpace(textBoxAutoriza.Text)))
                    {
                        MySqlCommand cmd = new MySqlCommand("UPDATE nombresoc SET Almacen = '" + textBoxAlmacen.Text + "', Autoriza = '" + textBoxAutoriza.Text + "'", c.dbconection());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Nombres actualizados correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if((string.IsNullOrWhiteSpace(textBoxAlmacen.Text)) || !(string.IsNullOrWhiteSpace(textBoxAutoriza.Text)))
                    {
                        MySqlCommand cmd = new MySqlCommand("UPDATE nombresoc SET Autoriza = '" + textBoxAutoriza.Text + "'", c.dbconection());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Nombres actualizados correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if(!(string.IsNullOrWhiteSpace(textBoxAlmacen.Text)) || (string.IsNullOrWhiteSpace(textBoxAutoriza.Text)))
                    {
                        MySqlCommand cmd = new MySqlCommand("UPDATE nombresoc SET Almacen = '" + textBoxAlmacen.Text + "'", c.dbconection());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Nombres actualizados correctamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    c.dbconection().Close();
                }
                else
                {
                    if (!(string.IsNullOrWhiteSpace(textBoxUsuario.Text)) && (id == 0))
                    {
                        MessageBox.Show("Contraseña incorreta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxUsuario.Text = "";
                        textBoxUsuario.Focus();
                    }
                    else if((string.IsNullOrWhiteSpace(textBoxUsuario.Text)) && (id == 0))
                    {
                        MessageBox.Show("Ingrese una contraseña", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("En el primer registro debe llenar todos los datos", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }   
                }
                limpiar();
            }
            else
            {
                if((string.IsNullOrWhiteSpace(textBoxAlmacen.Text)) && (string.IsNullOrWhiteSpace(textBoxAutoriza.Text)))
                {
                    MessageBox.Show("Tiene que llenar el campo de almacen y/o persona que autoriza", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!(string.IsNullOrWhiteSpace(textBoxUsuario.Text)) && (id == 0))
                {
                    MessageBox.Show("Contraseña incorreta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxUsuario.Text = "";
                    textBoxUsuario.Focus();
                }
                else if((string.IsNullOrWhiteSpace(textBoxUsuario.Text)) && (id == 0))
                {
                    MessageBox.Show("Ingrese una contraseña", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } 
        }

        private void NombresOC_Load(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT coalesce((personafkcpersonal), '0') AS persona, Almacen, Autoriza FROM nombresoc WHERE empresa='"+empresa+"'", c.dbconection());
            MySqlDataReader rd = cmd.ExecuteReader();
            if(rd.Read())
            {
                iddb = Convert.ToString(rd.GetString("persona"));
                alm = Convert.ToString(rd.GetString("Almacen"));
                aut = Convert.ToString(rd.GetString("Autoriza"));
            }
            else
            {
                iddb = "0";
                alm = "";
                aut = "";
            }
            rd.Close();
            c.dbconection().Close();
        }

        private void textBoxUsuario_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT t1.idpersona FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idpersona = t2.usuariofkcpersonal WHERE t2.password = '" + val.Encriptar(textBoxUsuario.Text) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "'", c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                id = Convert.ToInt32(dr.GetString("idpersona"));
            }
            else
            {
                id = 0;
            }
            c.dbconection().Close();
        }

        private void textBoxAll_Validated(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            while (txt.Text.Contains("  "))
            {
                txt.Text = txt.Text.Replace("  ", " ");
            }
        }

        private void buttonEditar_MouseMove(object sender, MouseEventArgs e)
        {
            buttonEditar.Size = new Size(59, 56);
        }

        private void buttonEditar_MouseLeave(object sender, EventArgs e)
        {
            buttonEditar.Size = new Size(54, 51);
        }
    }
}
