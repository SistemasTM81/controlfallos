using System;
using System.Data;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class privilegiosMantenimiento : Form
    {
        validaciones v;
        int idUsuario;
        bool editar = false;
        DataTable t;
        string[] id;
        public privilegiosMantenimiento(int idUsuario, validaciones v)
        {
            InitializeComponent();
            this.v = v;
            this.idUsuario = idUsuario;
        }
        void buscarNombre()
        {
            lbltitle.Text = "Nombre del Empleado: " + v.getaData("SELECT CONCAT(apPaterno,' ',apMaterno,' ',nombres) as Nombre FROM cpersonal WHERE idPersona ='" + idUsuario + "'");
        }
        private void CambiarEstado_Click(object sender, EventArgs e)
        {
            v.CambiarEstado_Click(sender, e);
        }

        private void btnconsultarempleado_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempleado.BackgroundImage) != v.check)
            {
                btneliminarempleado.BackgroundImage = btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = false;
            }
            else
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = true;
        }


        private void btnconsultarcargo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarcargo.BackgroundImage) != v.check)
            {
                btneliminarcargo.BackgroundImage = btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = false;
            }
            else
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = true;
        }


        private void btnconsultarunidad_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarunidad.BackgroundImage) != v.check)
            {
                btnmodificarunidad.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarunidad.Enabled = false;
            }
            else
                btnmodificarunidad.Enabled = true;
        }

        private void btnconsultarfallo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarfallo.BackgroundImage) != v.check)
            {

                btneliminarfallo.BackgroundImage = btnmodificarfallo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarfallo.Enabled = btnmodificarfallo.Enabled = false;
            }
            else
                btneliminarfallo.Enabled = btnmodificarfallo.Enabled = true;
        }


        private void btnconsultarmante_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarmante.BackgroundImage) != v.check)
            {
                btnmodificarmante.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarmante.Enabled = false;
            }
            else
                btnmodificarmante.Enabled = true;
        }


        private void button33_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in panel2.Controls)
            {
                if (ctrl is Button)
                {
                    ctrl.BackgroundImage = controlFallos.Properties.Resources.uncheck;
                }
            }
            catPersonal cat = (catPersonal)Owner;
            cat.privilegios = null;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            string[,] privilegios = new string[7, 6];
            string[,] respaldo = new string[7, 5];
            respaldo[0, 0] = privilegios[0, 0] = v.getIntFrombool((v.ImageToString(btninsertarfallo.BackgroundImage) == v.check || v.ImageToString(btnconsultarfallo.BackgroundImage) == v.check || v.ImageToString(btnmodificarfallo.BackgroundImage) == v.check || v.ImageToString(btneliminarfallo.BackgroundImage) == v.check)).ToString();
            respaldo[0, 1] = privilegios[0, 1] = v.Checked(btninsertarfallo.BackgroundImage).ToString();
            respaldo[0, 2] = privilegios[0, 2] = v.Checked(btnconsultarfallo.BackgroundImage).ToString();
            respaldo[0, 3] = privilegios[0, 3] = v.Checked(btnmodificarfallo.BackgroundImage).ToString();
            respaldo[0, 4] = privilegios[0, 4] = v.Checked(btneliminarfallo.BackgroundImage).ToString();
            privilegios[0, 5] = "catfallosGrales";
            respaldo[1, 0] = privilegios[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
            respaldo[1, 1] = privilegios[1, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString();
            respaldo[1, 2] = privilegios[1, 2] = v.Checked(btnconsultarempleado.BackgroundImage).ToString();
            respaldo[1, 3] = privilegios[1, 3] = v.Checked(btnmodificarempleado.BackgroundImage).ToString();
            respaldo[1, 4] = privilegios[1, 4] = v.Checked(btneliminarempleado.BackgroundImage).ToString();
            privilegios[1, 5] = "catPersonal";
            respaldo[2, 0] = privilegios[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[2, 1] = privilegios[2, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString();
            respaldo[2, 2] = privilegios[2, 2] = v.Checked(btnconsultarcargo.BackgroundImage).ToString();
            respaldo[2, 3] = privilegios[2, 3] = v.Checked(btnmodificarcargo.BackgroundImage).ToString();
            respaldo[2, 4] = privilegios[2, 4] = v.Checked(btneliminarcargo.BackgroundImage).ToString();
            privilegios[2, 5] = "catPuestos";
            respaldo[3, 0] = privilegios[3, 0] = v.getIntFrombool((v.ImageToString(btninsertartipo.BackgroundImage) == v.check || v.ImageToString(btnconsultartipo.BackgroundImage) == v.check || v.ImageToString(btnmodificartipo.BackgroundImage) == v.check || v.ImageToString(btneliminartipo.BackgroundImage) == v.check)).ToString();
            respaldo[3, 1] = privilegios[3, 1] = v.Checked(btninsertartipo.BackgroundImage).ToString();
            respaldo[3, 2] = privilegios[3, 2] = v.Checked(btnconsultartipo.BackgroundImage).ToString();
            respaldo[3, 3] = privilegios[3, 3] = v.Checked(btnmodificartipo.BackgroundImage).ToString();
            respaldo[3, 4] = privilegios[3, 4] = v.Checked(btneliminartipo.BackgroundImage).ToString();
            privilegios[3, 5] = "catTipos";
            respaldo[4, 0] = privilegios[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check)).ToString();
            respaldo[4, 1] = privilegios[4, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString();
            respaldo[4, 2] = privilegios[4, 2] = v.Checked(btnconsultarunidad.BackgroundImage).ToString();
            respaldo[4, 3] = privilegios[4, 3] = v.Checked(btnmodificarunidad.BackgroundImage).ToString();
            respaldo[4, 4] = privilegios[4, 4] = "0";
            privilegios[4, 5] = "catUnidades";
            respaldo[5, 0] = privilegios[5, 0] = v.getIntFrombool((v.ImageToString(btninsertarmante.BackgroundImage) == v.check || v.ImageToString(btnconsultarmante.BackgroundImage) == v.check || v.ImageToString(btnmodificarmante.BackgroundImage) == v.check)).ToString();
            respaldo[5, 1] = privilegios[5, 1] = v.Checked(btninsertarmante.BackgroundImage).ToString();
            respaldo[5, 2] = privilegios[5, 2] = v.Checked(btnconsultarmante.BackgroundImage).ToString();
            respaldo[5, 3] = privilegios[5, 3] = v.Checked(btnmodificarmante.BackgroundImage).ToString();
            respaldo[5, 4] = privilegios[5, 4] = "0";
            privilegios[5, 5] = "Mantenimiento";
            respaldo[6, 0] = privilegios[6, 0] = v.getIntFrombool(v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check).ToString();
            respaldo[6, 1] = privilegios[6, 1] = "0";
            respaldo[6, 2] = privilegios[6, 2] = v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
            respaldo[6, 3] = privilegios[6, 3] = "0";
            respaldo[6, 4] = privilegios[6, 4] = "0";
            privilegios[6, 5] = "historial";
            if (!v.todosFalsos(respaldo))
            {
                if (!editar)
                {
                    if (idUsuario > 0)
                    {
                        for (int i = 0; i <privilegios.GetLength(0); i++)
                        {
                            string ver = privilegios[i, 0];
                            string insertar = privilegios[i, 1];
                            string consultar = privilegios[i, 2];
                            string modificar = privilegios[i, 3];
                            string eliminar = privilegios[i, 4];
                            string nombre = privilegios[i, 5];
                            v.insert(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
                        }

                        MessageBox.Show("Se Han Asignado los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        catPersonal Cat = (catPersonal)Owner;

                        Cat.lblprivilegios.Text = "Actualizar Privilegios";
                    }
                    else
                    {
                        catPersonal cat = (catPersonal)Owner;
                        cat.privilegios = privilegios;


                    }
                }
                else
                {
                    if (Convert.ToInt32(v.getaData(string.Format("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal={0}", idUsuario))) > 0)
                    {
                        string mensaje = "Agregado";
                        if (sehicieronModificaciones(t, respaldo))
                        {

                            for (int i = 0; i < privilegios.GetLength(0); i++)
                            {
                                string ver = privilegios[i, 0];
                                string insertar = privilegios[i, 1];
                                string consultar = privilegios[i, 2];
                                string modificar = privilegios[i, 3];
                                string eliminar = privilegios[i, 4];
                                string nombre = privilegios[i, 5];
                                v.edit(id[i], ver, insertar, consultar, modificar, eliminar);
                            }
                            mensaje = "Actualizado";
                            MessageBox.Show("Se Han " + mensaje + " los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        }
                        else
                        {
                            MessageBox.Show("No se Realizaron Modificaciones", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        catPersonal Cat = (catPersonal)Owner;
                        Cat.privilegios = privilegios;
                    }
                    
                }
            }

            else
            {
                if (!editar)
                {
                    catPersonal Cat = (catPersonal)Owner;
                    Cat.privilegios = null;
                }
                else
                {
                    v.EliminarPrivilegios(idUsuario);
                    MessageBox.Show("Se Han Eliminado Los Privilegios", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    catPersonal Cat = (catPersonal)Owner;
                    Cat.lblprivilegios.Text = "Asignar Privilegios";
                }
            }
        }
        bool sehicieronModificaciones(DataTable a, string[,] b)
        {
            bool res = false;
            a.Columns.RemoveAt(0);
            for (int i = 0; i < 7; i++)
            {
                object[] c = a.Rows[i].ItemArray;
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j].ToString() != b[i, j])
                    {
                        res = true;
                    }
                }
            }

            return res;
        }
        private void privilegiosMantenimiento_Load(object sender, EventArgs e)
        {
            if (idUsuario > 0)
            {
                buscarNombre();
                exitenPrivilegios();
            }
            lbltitle.Left = (panel1.Width - lbltitle.Size.Width) / 2;
        }
        public void insertarPrivilegios(string[,] privilegios)
        {
            btninsertarfallo.BackgroundImage = v.Checked(privilegios[0, 1]);
            btnconsultarfallo.BackgroundImage = v.Checked(privilegios[0, 2]);
            btnmodificarfallo.BackgroundImage = v.Checked(privilegios[0, 3]);
            btneliminarfallo.BackgroundImage = v.Checked(privilegios[0, 4]);
            btninsertarempleado.BackgroundImage = v.Checked(privilegios[1, 1]);
            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[1, 2]);
            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[1, 3]);
            btneliminarempleado.BackgroundImage = v.Checked(privilegios[1, 4]);
            btninsertarcargo.BackgroundImage = v.Checked(privilegios[2, 1]);
            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[2, 2]);
            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[2, 3]);
            btneliminarcargo.BackgroundImage = v.Checked(privilegios[2, 4]);
            btninsertartipo.BackgroundImage = v.Checked(privilegios[3, 1]);
            btnconsultartipo.BackgroundImage = v.Checked(privilegios[3, 2]);
            btnmodificartipo.BackgroundImage = v.Checked(privilegios[3, 3]);
            btneliminartipo.BackgroundImage = v.Checked(privilegios[3, 4]);
            btninsertarunidad.BackgroundImage = v.Checked(privilegios[4, 1]);
            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[4, 2]);
            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[4, 3]);
            btninsertarmante.BackgroundImage = v.Checked(privilegios[5, 1]);
            btnconsultarmante.BackgroundImage = v.Checked(privilegios[5, 2]);
            btnmodificarmante.BackgroundImage = v.Checked(privilegios[5, 3]);

            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[6, 2]);
        }
        void exitenPrivilegios()
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'")) > 0)
            {
                lbltexto.Text = "Actualizar Privilegios";
                t = (DataTable)v.getData("SELECT namform,ver,COALESCE(privilegios,'0/0/0/0') FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'");
                id = v.getaData("SELECT group_concat(idprivilegio separator ';') from privilegios WHERE usuariofkcpersonal='" + idUsuario + "';").ToString().Split(';');
                editar = true;
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    object[] pr = t.Rows[i].ItemArray;
                    object[] privilegios = pr[2].ToString().Split('/');
                    switch (pr[0].ToString())
                    {
                        case "catfallosGrales":

                            btninsertarfallo.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarfallo.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarfallo.BackgroundImage = v.Checked(privilegios[2]);
                            btneliminarfallo.BackgroundImage = v.Checked(privilegios[3]);
                            break;
                        case "catPersonal":

                            btninsertarempleado.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[2]);
                            btneliminarempleado.BackgroundImage = v.Checked(privilegios[3]);
                            break;
                        case "catPuestos":

                            btninsertarcargo.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[2]);
                            btneliminarcargo.BackgroundImage = v.Checked(privilegios[3]);
                            break;
                        case "catTipos":

                            btninsertartipo.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultartipo.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificartipo.BackgroundImage = v.Checked(privilegios[2]);
                            btneliminartipo.BackgroundImage = v.Checked(privilegios[3]);
                            break;
                        case "catUnidades":

                            btninsertarunidad.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "Mantenimiento":

                            btninsertarmante.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarmante.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarmante.BackgroundImage = v.Checked(privilegios[2]);

                            break;
                        case "historial":

                            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[1]);

                            break;
                    }
                }
            }
        }
        private void btnconsultarhistorial_BackgroundImageChanged(object sender, EventArgs e){}

        private void button1_Click(object sender, EventArgs e)
        {

            string[,] respaldo = new string[7, 2];
            respaldo[0, 0] = v.getIntFrombool((v.ImageToString(btninsertarfallo.BackgroundImage) == v.check || v.ImageToString(btnconsultarfallo.BackgroundImage) == v.check || v.ImageToString(btnmodificarfallo.BackgroundImage) == v.check || v.ImageToString(btneliminarfallo.BackgroundImage) == v.check)).ToString();
            respaldo[0, 1] = v.Checked(btninsertarfallo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarfallo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarfallo.BackgroundImage).ToString() + "/" + v.Checked(btneliminarfallo.BackgroundImage).ToString();
            respaldo[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
            respaldo[1, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarempleado.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarempleado.BackgroundImage).ToString() + "/" + v.Checked(btneliminarempleado.BackgroundImage).ToString();
            respaldo[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[2, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarcargo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarcargo.BackgroundImage).ToString() + "/" + v.Checked(btneliminarcargo.BackgroundImage).ToString();
            respaldo[3, 0] = v.getIntFrombool((v.ImageToString(btninsertartipo.BackgroundImage) == v.check || v.ImageToString(btnconsultartipo.BackgroundImage) == v.check || v.ImageToString(btnmodificartipo.BackgroundImage) == v.check || v.ImageToString(btneliminartipo.BackgroundImage) == v.check)).ToString();
            respaldo[3, 1] = v.Checked(btninsertartipo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultartipo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificartipo.BackgroundImage).ToString() + "/" + v.Checked(btneliminartipo.BackgroundImage).ToString();

            respaldo[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check)).ToString();
            respaldo[4, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarunidad.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarunidad.BackgroundImage).ToString();

            respaldo[5, 0] = v.getIntFrombool((v.ImageToString(btninsertarmante.BackgroundImage) == v.check || v.ImageToString(btnconsultarmante.BackgroundImage) == v.check || v.ImageToString(btnmodificarmante.BackgroundImage) == v.check)).ToString();
            respaldo[5, 1] = v.Checked(btninsertarmante.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarmante.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarmante.BackgroundImage).ToString();
            respaldo[6, 0] = v.getIntFrombool((v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check)).ToString();
            respaldo[6, 1] = "0" + "/" + v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
            catPersonal cat = (catPersonal)Owner;
            if (editar)
            {
                if (sehicieronModificaciones(t, respaldo))
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Privilegios. ¿Desea Guardarlas?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        button32_Click(sender, e);
                    }
                }
            }
            else
            {
                bool res = false;
                if (cat.privilegios == null) v.seDetectaronModificaciones(respaldo);
                else
                    res = v.sehicieronModificaciones(cat.privilegios, respaldo);
                if (res)
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Privilegios. ¿Desea Guardarlas?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        button32_Click(sender, e);
                    }
                }
            }
        }
        

        private void label24_Click(object sender, EventArgs e){}

        private void label21_Click(object sender, EventArgs e){}

        private void label12_Click(object sender, EventArgs e){}

        private void label11_Click(object sender, EventArgs e){}

        private void label9_Click(object sender, EventArgs e){}

        private void label8_Click(object sender, EventArgs e){}

        private void label1_Click(object sender, EventArgs e){}

        private void label15_Click(object sender, EventArgs e){}

        private void label17_Click(object sender, EventArgs e){}
        

        private void label18_Click(object sender, EventArgs e){}

        private void label22_Click(object sender, EventArgs e){}

        private void panel3_Paint(object sender, PaintEventArgs e){}

        private void btnconsultartipo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultartipo.BackgroundImage) != v.check)
            {
                btneliminartipo.Enabled = btnmodificartipo.Enabled = false;
                btneliminartipo.BackgroundImage = btnmodificartipo.BackgroundImage = Properties.Resources.uncheck;
            }
            else
            {
                btneliminartipo.Enabled = btnmodificartipo.Enabled = true;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
    }
}
