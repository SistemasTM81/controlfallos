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
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
        }

        public privilegiosMantenimiento(validaciones v) { this.v = v; InitializeComponent(); }

        void buscarNombre()
        {
            lbltitle.Text = "Nombre del Empleado: " + v.getaData("SELECT CONCAT(coalesce(apPaterno,''),' ',coalesce(apMaterno,''),' ',coalesce(nombres,'')) as Nombre FROM cpersonal WHERE idPersona ='" + idUsuario + "'");
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
        private void btnConsultaRefac_BackgroundImageChanged(object sender, EventArgs e)
        {

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

          /*  if (comboPuesto.Text == "-- SELECCIONE UN PUESTO --")
            {

                btninsertarempleado.BackgroundImage = ; checkBox21.Checked = false;
                checkBox2.Checked = false; checkBox22.Checked = false;
                checkBox3.Checked = false; checkBox23.Checked = false;
                checkBox4.Checked = false; checkBox24.Checked = false;

                checkBox5.Checked = false; checkBox25.Checked = false;
                checkBox6.Checked = false; checkBox26.Checked = false;
                checkBox7.Checked = false; checkBox27.Checked = false;
                checkBox8.Checked = false; checkBox28.Checked = false;

                checkBox9.Checked = false; checkBox29.Checked = false;
                checkBox10.Checked = false; checkBox30.Checked = false;
                checkBox11.Checked = false; checkBox31.Checked = false;
                checkBox12.Checked = false; checkBox32.Checked = false;

                checkBox13.Checked = false; checkBox33.Checked = false;
                checkBox14.Checked = false; checkBox34.Checked = false;
                checkBox15.Checked = false; checkBox35.Checked = false;
                checkBox16.Checked = false; checkBox36.Checked = false;

                checkBox17.Checked = false; checkBox37.Checked = false;
                checkBox18.Checked = false; checkBox38.Checked = false;
                checkBox19.Checked = false; checkBox39.Checked = false;
                checkBox20.Checked = false;
            }*/
            /*
            else if (comboPuesto.Text == "ADMINISTRADOR")
            {
                checkBox1.Checked = true; checkBox21.Checked = true;
                checkBox2.Checked = true; checkBox22.Checked = true;
                checkBox3.Checked = true; checkBox23.Checked = true;
                checkBox4.Checked = true; checkBox24.Checked = true;

                checkBox5.Checked = true; checkBox25.Checked = true;
                checkBox6.Checked = true; checkBox26.Checked = true;
                checkBox7.Checked = true; checkBox27.Checked = true;
                checkBox8.Checked = true; checkBox28.Checked = true;

                checkBox9.Checked = true; checkBox29.Checked = true;
                checkBox10.Checked = true; checkBox30.Checked = true;
                checkBox11.Checked = true; checkBox31.Checked = true;
                checkBox12.Checked = true; checkBox32.Checked = true;

                checkBox13.Checked = true; checkBox33.Checked = true;
                checkBox14.Checked = true; checkBox34.Checked = true;
                checkBox15.Checked = true; checkBox35.Checked = true;
                checkBox16.Checked = true; checkBox36.Checked = true;

                checkBox17.Checked = true; checkBox37.Checked = true;
                checkBox18.Checked = true; checkBox38.Checked = true;
                checkBox19.Checked = true; checkBox39.Checked = true;
                checkBox20.Checked = true;

            }


            else if (comboPuesto.Text == "AUXULIAR DE ALMACEN")
            {

                checkBox1.Checked = false; checkBox21.Checked = false;
                checkBox2.Checked = false; checkBox22.Checked = false;
                checkBox3.Checked = false; checkBox23.Checked = false;
                checkBox4.Checked = false; checkBox24.Checked = false;

                checkBox5.Checked = false; checkBox25.Checked = false;
                checkBox6.Checked = false; checkBox26.Checked = false;
                checkBox7.Checked = false; checkBox27.Checked = false;
                checkBox8.Checked = false; checkBox28.Checked = false;

                checkBox9.Checked = false; checkBox29.Checked = false;
                checkBox10.Checked = false; checkBox30.Checked = false;
                checkBox11.Checked = true; checkBox31.Checked = true;
                checkBox12.Checked = false; checkBox32.Checked = false;

                checkBox13.Checked = false; checkBox33.Checked = true;
                checkBox14.Checked = false; checkBox34.Checked = true;
                checkBox15.Checked = false; checkBox35.Checked = false;
                checkBox16.Checked = false; checkBox36.Checked = false;

                checkBox17.Checked = false; checkBox37.Checked = true;
                checkBox18.Checked = false; checkBox38.Checked = true;
                checkBox19.Checked = false; checkBox39.Checked = false;
                checkBox20.Checked = false;
            }

            else if (comboPuesto.Text == "COMPRAS")
            {

                checkBox1.Checked = false; checkBox21.Checked = false;
                checkBox2.Checked = false; checkBox22.Checked = false;
                checkBox3.Checked = true; checkBox23.Checked = false;
                checkBox4.Checked = false; checkBox24.Checked = false;

                checkBox5.Checked = false; checkBox25.Checked = false;
                checkBox6.Checked = false; checkBox26.Checked = false;
                checkBox7.Checked = false; checkBox27.Checked = false;
                checkBox8.Checked = false; checkBox28.Checked = false;

                checkBox9.Checked = false; checkBox29.Checked = true;
                checkBox10.Checked = true; checkBox30.Checked = false;
                checkBox11.Checked = true; checkBox31.Checked = false;
                checkBox12.Checked = false; checkBox32.Checked = true;

                checkBox13.Checked = false; checkBox33.Checked = false;
                checkBox14.Checked = false; checkBox34.Checked = false;
                checkBox15.Checked = false; checkBox35.Checked = false;
                checkBox16.Checked = false; checkBox36.Checked = true;

                checkBox17.Checked = true; checkBox37.Checked = false;
                checkBox18.Checked = false; checkBox38.Checked = false;
                checkBox19.Checked = false; checkBox39.Checked = false;
                checkBox20.Checked = false;
            }

            else if (comboPuesto.Text == "ENCARGADO DE ALMACEN")
            {

                checkBox1.Checked = false; checkBox21.Checked = true;
                checkBox2.Checked = false; checkBox22.Checked = true;
                checkBox3.Checked = false; checkBox23.Checked = true;
                checkBox4.Checked = false; checkBox24.Checked = true;

                checkBox5.Checked = false; checkBox25.Checked = true;
                checkBox6.Checked = false; checkBox26.Checked = true;
                checkBox7.Checked = false; checkBox27.Checked = true;
                checkBox8.Checked = true; checkBox28.Checked = true;

                checkBox9.Checked = true; checkBox29.Checked = false;
                checkBox10.Checked = true; checkBox30.Checked = true;
                checkBox11.Checked = true; checkBox31.Checked = true;
                checkBox12.Checked = true; checkBox32.Checked = false;

                checkBox13.Checked = true; checkBox33.Checked = true;
                checkBox14.Checked = true; checkBox34.Checked = true;
                checkBox15.Checked = true; checkBox35.Checked = true;
                checkBox16.Checked = true; checkBox36.Checked = false;

                checkBox17.Checked = true; checkBox37.Checked = true;
                checkBox18.Checked = true; checkBox38.Checked = true;
                checkBox19.Checked = true; checkBox39.Checked = true;
                checkBox20.Checked = true;
            }

            else if (comboPuesto.Text == "FINANZAS")
            {

                checkBox1.Checked = false; checkBox21.Checked = false;
                checkBox2.Checked = false; checkBox22.Checked = false;
                checkBox3.Checked = false; checkBox23.Checked = false;
                checkBox4.Checked = false; checkBox24.Checked = false;

                checkBox5.Checked = false; checkBox25.Checked = false;
                checkBox6.Checked = false; checkBox26.Checked = false;
                checkBox7.Checked = false; checkBox27.Checked = false;
                checkBox8.Checked = false; checkBox28.Checked = false;

                checkBox9.Checked = false; checkBox29.Checked = false;
                checkBox10.Checked = false; checkBox30.Checked = false;
                checkBox11.Checked = false; checkBox31.Checked = false;
                checkBox12.Checked = false; checkBox32.Checked = true;

                checkBox13.Checked = false; checkBox33.Checked = false;
                checkBox14.Checked = false; checkBox34.Checked = false;
                checkBox15.Checked = false; checkBox35.Checked = false;
                checkBox16.Checked = false; checkBox36.Checked = false;

                checkBox17.Checked = false; checkBox37.Checked = false;
                checkBox18.Checked = false; checkBox38.Checked = false;
                checkBox19.Checked = false; checkBox39.Checked = false;
                checkBox20.Checked = false;
            }
            */
            string[,] privilegios = new string[8, 7];
            string[,] respaldo = new string[8, 6];
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
            respaldo[7, 0] = privilegios[7, 0] = v.getIntFrombool(v.ImageToString(btnConsultaRefac.BackgroundImage) == v.check).ToString();
            respaldo[7, 1] = privilegios[7, 1] = "0";
            respaldo[7, 2] = privilegios[7, 2] = v.getIntFrombool(v.ImageToString(btnConsultaRefac.BackgroundImage) == v.check).ToString();
            respaldo[7, 3] = privilegios[7, 3] = "0";
            respaldo[7, 4] = privilegios[7, 4] = "0";
            privilegios[7, 5] = "catRefacC";
            if (!v.todosFalsos(respaldo))
            {
                if (!editar)
                {
                    if (idUsuario > 0)
                    {
                        for (int i = 0; i < privilegios.GetLength(0); i++)
                        {
                            string ver = privilegios[i, 0];
                            string insertar = privilegios[i, 1];
                            string consultar = privilegios[i, 2];
                            string modificar = privilegios[i, 3];
                            string eliminar = privilegios[i, 4];
                            string nombre = privilegios[i, 5];
                            v.insert(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
                            //v.c.insertLocal(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
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
                            //if (privilegios.Length > 0)
                            //{
                            //    v.c.EliminarPrivilegiosLocales(idUsuario);
                            //}
                            for (int i = 0; i < privilegios.GetLength(0); i++)
                            {
                                string ver = privilegios[i, 0];
                                string insertar = privilegios[i, 1];
                                string consultar = privilegios[i, 2];
                                string modificar = privilegios[i, 3];
                                string eliminar = privilegios[i, 4];
                                string nombre = privilegios[i, 5];
                                v.edit(id[i], ver, insertar, consultar, modificar, eliminar);
                                //v.c.editLocal(id[i], ver, insertar, consultar, modificar, eliminar);
                                //v.c.insertLocal(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
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
                    //v.c.EliminarPrivilegiosLocales(idUsuario);
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
            for (int i = 0; i < b.GetLength(0); i++)
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


        bool sehicieronModificaciones(string[,] a, string[,] b)
        {
            string[,] temp = new string[17, 5];
            for (int m = 0; m < 17; m++)
            {
                for (int n = 0; n < 5; n++)
                    temp[m, n] = a[m, n];
            }
            a = temp;
            bool res = false;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    if (a[i, j] != b[i, j])
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
            btnConsultaRefac.BackgroundImage = v.Checked(privilegios[7, 2]);
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
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarfallo.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catPersonal":

                            btninsertarempleado.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarempleado.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catPuestos":

                            btninsertarcargo.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarcargo.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catTipos":

                            btninsertartipo.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultartipo.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificartipo.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminartipo.BackgroundImage = v.Checked(privilegios[3]);
                            }
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
                        case "catRefacC":

                            btnConsultaRefac.BackgroundImage = v.Checked(privilegios[1]);

                            break;
                    }
                }
            }
        }
        private void btnconsultarhistorial_BackgroundImageChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {

            string[,] respaldo = new string[8, 2];
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
            respaldo[7, 0] = v.getIntFrombool((v.ImageToString(btnConsultaRefac.BackgroundImage) == v.check)).ToString();
            respaldo[7, 1] = "0" + "/" + v.Checked(btnConsultaRefac.BackgroundImage).ToString();
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


        private void label24_Click(object sender, EventArgs e) { }

        private void label21_Click(object sender, EventArgs e) { }

        private void label12_Click(object sender, EventArgs e) { }

        private void label11_Click(object sender, EventArgs e) { }

        private void label9_Click(object sender, EventArgs e) { }

        private void label8_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label15_Click(object sender, EventArgs e) { }

        private void label17_Click(object sender, EventArgs e) { }


        private void label18_Click(object sender, EventArgs e) { }

        private void label22_Click(object sender, EventArgs e) { }

        private void panel3_Paint(object sender, PaintEventArgs e) { }

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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
