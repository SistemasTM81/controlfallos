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
    public partial class privilegiosAlmacen : Form
    {
        int idUsuario;
        bool editar;
        DataTable t;
        string[] id;
        validaciones v;
        public privilegiosAlmacen(int idUsuario,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        private void CambiarEstado_Click(object sender, EventArgs e)
        {
            v.CambiarEstado_Click(sender, e);

        }
        void buscarNombre()
        {
            lbltitle.Text = "Empleado Actual: " + v.getaData("SELECT CONCAT(apPaterno,' ',apMaterno,' ',nombres) as Nombre FROM cpersonal WHERE idPersona ='" + idUsuario + "'");
        }


        private void btnconsultarempleado_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempleado.BackgroundImage) != v.check)
            {
                btneliminarempleado.BackgroundImage = btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = false;
            }
            else
            {
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = true;
            }
        }



        private void btnconsultarcargo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarcargo.BackgroundImage) != v.check)
            {
                btneliminarcargo.BackgroundImage = btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = false;
            }
            else
            {
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = true;
            }
        }


        private void btnconsultarproveedor_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarproveedor.BackgroundImage) != v.check)
            {
                btneliminarproveedor.BackgroundImage = btnmodificarproveedor.BackgroundImage = Properties.Resources.uncheck;
                btneliminarproveedor.Enabled = btnmodificarproveedor.Enabled = false;
            }
            else
            {
                btneliminarproveedor.Enabled = btnmodificarproveedor.Enabled = true;
            }
        }



        private void btnconsultarrefaccion_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarrefaccion.BackgroundImage) != v.check)
            {
                btneliminarrefaccion.BackgroundImage = btnmodificarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btneliminarrefaccion.Enabled = btnmodificarrefaccion.Enabled = false;
            }
            else
            {
                btneliminarrefaccion.Enabled = btnmodificarrefaccion.Enabled = true;
            }
        }



        private void btnconsultarrequisicion_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarrequisicion.BackgroundImage) != v.check)
            {
                btnmodificarrequisicion.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarrequisicion.Enabled = false;
            }
            else
                btnmodificarrequisicion.Enabled = true;
            
        }



        private void btnconsultaralmacen_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultaralmacen.BackgroundImage) != v.check)
            {
                btnmodificaralmacen.BackgroundImage = Properties.Resources.uncheck;
                btnmodificaralmacen.Enabled = false;
            }
            else
            {
                btnmodificaralmacen.Enabled = true;
            }
        }


        private void privilegiosAlmacen_Load(object sender, EventArgs e)
        {
            if (idUsuario > 0)
            {
                buscarNombre();
                exitenPrivilegios();
            }
            lbltitle.Left = (panel1.Width - lbltitle.Size.Width) / 2;
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
            try
            {
                string[,] privilegios = new string[12, 6];
                string[,] respaldo = new string[12, 5];
                respaldo[0, 0] = privilegios[0, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
                respaldo[0, 1] = privilegios[0, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString();
                respaldo[0, 2] = privilegios[0, 2] = v.Checked(btnconsultarempleado.BackgroundImage).ToString();
                respaldo[0, 3] = privilegios[0, 3] = v.Checked(btnmodificarempleado.BackgroundImage).ToString();
                respaldo[0, 4] = privilegios[0, 4] = v.Checked(btneliminarempleado.BackgroundImage).ToString();
                privilegios[0, 5] = "catPersonal";
                respaldo[1, 0] = privilegios[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
                respaldo[1, 1] = privilegios[1, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString();
                respaldo[1, 2] = privilegios[1, 2] = v.Checked(btnconsultarcargo.BackgroundImage).ToString();
                respaldo[1, 3] = privilegios[1, 3] = v.Checked(btnmodificarcargo.BackgroundImage).ToString();
                respaldo[1, 4] = privilegios[1, 4] = v.Checked(btneliminarcargo.BackgroundImage).ToString();
                privilegios[1, 5] = "catPuestos";
                respaldo[2, 0] = privilegios[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
                respaldo[2, 1] = privilegios[2, 1] = v.Checked(btninsertarproveedor.BackgroundImage).ToString();
                respaldo[2, 2] = privilegios[2, 2] = v.Checked(btnconsultarproveedor.BackgroundImage).ToString();
                respaldo[2, 3] = privilegios[2, 3] = v.Checked(btnmodificarproveedor.BackgroundImage).ToString();
                respaldo[2, 4] = privilegios[2, 4] = v.Checked(btneliminarproveedor.BackgroundImage).ToString();
                privilegios[2, 5] = "catProveedores";
                respaldo[3, 0] = privilegios[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarrefaccion.BackgroundImage) == v.check || v.ImageToString(btnconsultarrefaccion.BackgroundImage) == v.check || v.ImageToString(btnmodificarrefaccion.BackgroundImage) == v.check || v.ImageToString(btneliminarrefaccion.BackgroundImage) == v.check)).ToString();
                respaldo[3, 1] = privilegios[3, 1] = v.Checked(btninsertarrefaccion.BackgroundImage).ToString();
                respaldo[3, 2] = privilegios[3, 2] = v.Checked(btnconsultarrefaccion.BackgroundImage).ToString();
                respaldo[3, 3] = privilegios[3, 3] = v.Checked(btnmodificarrefaccion.BackgroundImage).ToString();
                respaldo[3, 4] = privilegios[3, 4] = v.Checked(btneliminarrefaccion.BackgroundImage).ToString();
                privilegios[3, 5] = "catRefacciones";
                respaldo[4, 0] = privilegios[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarempresa.BackgroundImage) == v.check || v.ImageToString(btnconsultarempresa.BackgroundImage) == v.check || v.ImageToString(btnmodificarempresa.BackgroundImage) == v.check || v.ImageToString(btneliminarempresa.BackgroundImage) == v.check)).ToString();
                respaldo[4, 1] = privilegios[4, 1] = v.Checked(btninsertarempresa.BackgroundImage).ToString();
                respaldo[4, 2] = privilegios[4, 2] = v.Checked(btnconsultarempresa.BackgroundImage).ToString();
                respaldo[4, 3] = privilegios[4, 3] = v.Checked(btnmodificarempresa.BackgroundImage).ToString();
                respaldo[4, 4] = privilegios[4, 4] = v.Checked(btneliminarempresa.BackgroundImage).ToString();
                privilegios[4, 5] = "catEmpresas";
                respaldo[5, 0] = privilegios[5, 0] = v.getIntFrombool((v.ImageToString(btninsertargiro.BackgroundImage) == v.check || v.ImageToString(btnconsultargiro.BackgroundImage) == v.check || v.ImageToString(btnmodificargiro.BackgroundImage) == v.check || v.ImageToString(btneliminargiro.BackgroundImage) == v.check)).ToString();
                respaldo[5, 1] = privilegios[5, 1] = v.Checked(btninsertargiro.BackgroundImage).ToString();
                respaldo[5, 2] = privilegios[5, 2] = v.Checked(btnconsultargiro.BackgroundImage).ToString();
                respaldo[5, 3] = privilegios[5, 3] = v.Checked(btnmodificargiro.BackgroundImage).ToString();
                respaldo[5, 4] = privilegios[5, 4] = v.Checked(btneliminargiro.BackgroundImage).ToString();
                privilegios[5, 5] = "catGiros";
                respaldo[6, 0] = privilegios[6, 0] = v.getIntFrombool((v.ImageToString(btninsertartipo.BackgroundImage) == v.check || v.ImageToString(btnconsultartipo.BackgroundImage) == v.check || v.ImageToString(btnmodificartipo.BackgroundImage) == v.check || v.ImageToString(btneliminartipo.BackgroundImage) == v.check)).ToString();
                respaldo[6, 1] = privilegios[6, 1] = v.Checked(btninsertartipo.BackgroundImage).ToString();
                respaldo[6, 2] = privilegios[6, 2] = v.Checked(btnconsultartipo.BackgroundImage).ToString();
                respaldo[6, 3] = privilegios[6, 3] = v.Checked(btnmodificartipo.BackgroundImage).ToString();
                respaldo[6, 4] = privilegios[6, 4] = v.Checked(btneliminartipo.BackgroundImage).ToString();
                privilegios[6, 5] = "catTipos";
                respaldo[7, 0] = privilegios[7, 0] = v.getIntFrombool((v.ImageToString(btninsertarrequisicion.BackgroundImage) == v.check || v.ImageToString(btnconsultarrequisicion.BackgroundImage) == v.check || v.ImageToString(btnmodificarrequisicion.BackgroundImage) == v.check)).ToString();
                respaldo[7, 1] = privilegios[7, 1] = v.Checked(btninsertarrequisicion.BackgroundImage).ToString();
                respaldo[7, 2] = privilegios[7, 2] = v.Checked(btnconsultarrequisicion.BackgroundImage).ToString();
                respaldo[7, 3] = privilegios[7, 3] = v.Checked(btnmodificarrequisicion.BackgroundImage).ToString();
                respaldo[7, 4] = privilegios[7, 4] = "0";
                privilegios[7, 5] = "ordencompra";
                respaldo[8, 0] = privilegios[8, 0] = v.getIntFrombool((v.ImageToString(btninsertarcomparativa.BackgroundImage) == v.check || v.ImageToString(btnconsultarcomparativa.BackgroundImage) == v.check || v.ImageToString(btnmodificarcomparativa.BackgroundImage) == v.check)).ToString();
                respaldo[8, 1] = privilegios[8, 1] = v.Checked(btninsertarrequisicion.BackgroundImage).ToString();
                respaldo[8, 2] = privilegios[8, 2] = v.Checked(btnconsultarrequisicion.BackgroundImage).ToString();
                respaldo[8, 3] = privilegios[8, 3] = v.Checked(btnmodificarrequisicion.BackgroundImage).ToString();
                respaldo[8, 4] = privilegios[8, 4] = "0";
                privilegios[8, 5] = "comparativas";
                respaldo[9, 0] = privilegios[9, 0] = v.getIntFrombool((v.ImageToString(btninsertaralmacen.BackgroundImage) == v.check || v.ImageToString(btnconsultaralmacen.BackgroundImage) == v.check || v.ImageToString(btnmodificaralmacen.BackgroundImage) == v.check)).ToString();
                respaldo[9, 1] = privilegios[9, 1] = v.Checked(btninsertaralmacen.BackgroundImage).ToString();
                respaldo[9, 2] = privilegios[9, 2] = v.Checked(btnconsultaralmacen.BackgroundImage).ToString();
                respaldo[9, 3] = privilegios[9, 3] = v.Checked(btnmodificaralmacen.BackgroundImage).ToString();
                respaldo[9, 4] = privilegios[9, 4] = "0";
                privilegios[9, 5] = "Almacen";
                respaldo[10, 0] = privilegios[10, 0] = v.getIntFrombool((v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check)).ToString();
                respaldo[10, 1] = privilegios[10, 1] = "0";
                respaldo[10, 2] = privilegios[10, 2] = v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
                respaldo[10, 3] = privilegios[10, 3] = "0";
                respaldo[10, 4] = privilegios[10, 4] = "0";
                privilegios[10, 5] = "historial";
                respaldo[11, 0] = privilegios[11, 0] = v.getIntFrombool((v.ImageToString(btnmodificariva.BackgroundImage) == v.check)).ToString();
                respaldo[11, 1] = privilegios[11, 1] = "0";
                respaldo[11, 2] = privilegios[11, 2] = "0";
                respaldo[11, 3] = privilegios[11, 3] = v.Checked(btnmodificariva.BackgroundImage).ToString();
                respaldo[11, 4] = privilegios[11, 4] = "0";
                privilegios[11, 5] = "changeiva";
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

                            }

                            MessageBox.Show("Se Han Asignado los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            catPersonal cat = (catPersonal)Owner;
                            cat.lblprivilegios.Text = "Actualizar Privilegios";
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
                            string mensaje;
                            if (sehicieronModificaciones(t, respaldo))
                            {

                                for (int i = 0; i < 9; i++)
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
                        if (idUsuario > 0)
                        {
                            catPersonal Cat = (catPersonal)Owner;
                            Cat.privilegios = null;
                        }
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public void insertarPrivilegios(string[,] privilegios)
        {
            btninsertarempleado.BackgroundImage = v.Checked(privilegios[0, 1]);
            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[0, 2]);
            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[0, 3]);
            btneliminarempleado.BackgroundImage = v.Checked(privilegios[0, 4]);
            btninsertarcargo.BackgroundImage = v.Checked(privilegios[1, 1]);
            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[1, 2]);
            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[1, 3]);
            btneliminarcargo.BackgroundImage = v.Checked(privilegios[1, 4]);
            btninsertarproveedor.BackgroundImage = v.Checked(privilegios[2, 1]);
            btnconsultarproveedor.BackgroundImage = v.Checked(privilegios[2, 2]);
            btnmodificarproveedor.BackgroundImage = v.Checked(privilegios[2, 3]);
            btneliminarproveedor.BackgroundImage = v.Checked(privilegios[2, 4]);
            btninsertarrefaccion.BackgroundImage = v.Checked(privilegios[3, 1]);
            btnconsultarrefaccion.BackgroundImage = v.Checked(privilegios[3, 2]);
            btnmodificarrefaccion.BackgroundImage = v.Checked(privilegios[3, 3]);
            btneliminarrefaccion.BackgroundImage = v.Checked(privilegios[3, 4]);
            btninsertarempresa.BackgroundImage = v.Checked(privilegios[4, 1]);
            btnconsultarempresa.BackgroundImage = v.Checked(privilegios[4, 2]);
            btnmodificarempresa.BackgroundImage = v.Checked(privilegios[4, 3]);
            btneliminarempresa.BackgroundImage = v.Checked(privilegios[5, 4]);
            btninsertargiro.BackgroundImage = v.Checked(privilegios[5, 1]);
            btnconsultargiro.BackgroundImage = v.Checked(privilegios[5, 2]);
            btnmodificargiro.BackgroundImage = v.Checked(privilegios[5, 3]);
            btneliminargiro.BackgroundImage = v.Checked(privilegios[5, 4]);
            btninsertartipo.BackgroundImage = v.Checked(privilegios[6, 1]);
            btnconsultartipo.BackgroundImage = v.Checked(privilegios[6, 2]);
            btnmodificartipo.BackgroundImage = v.Checked(privilegios[6, 3]);
            btneliminartipo.BackgroundImage = v.Checked(privilegios[6, 4]);
            btninsertarrequisicion.BackgroundImage = v.Checked(privilegios[7, 1]);
            btnconsultarrequisicion.BackgroundImage = v.Checked(privilegios[7, 2]);
            btnmodificarrequisicion.BackgroundImage = v.Checked(privilegios[7, 3]);
            btninsertarcomparativa.BackgroundImage = v.Checked(privilegios[8, 1]);
            btnconsultarcomparativa.BackgroundImage = v.Checked(privilegios[8, 2]);
            btnmodificarcomparativa.BackgroundImage = v.Checked(privilegios[8, 3]);
            btninsertaralmacen.BackgroundImage = v.Checked(privilegios[9, 1]);
            btnconsultaralmacen.BackgroundImage = v.Checked(privilegios[9, 2]);
            btnmodificaralmacen.BackgroundImage = v.Checked(privilegios[9, 3]);
            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[10, 2]);
            btnmodificariva.BackgroundImage = v.Checked(privilegios[11, 3]);

        }
        void exitenPrivilegios()
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'")) > 0)
            {
                lbltexto.Text = "Actualizar Privilegios";
                t = (DataTable)v.getData("SELECT namform,ver,insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'");
                id = v.getaData("SELECT group_concat(idprivilegio separator ';') from privilegios WHERE usuariofkcpersonal='" + idUsuario + "';").ToString().Split(';');
                editar = true;
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    object[] privilegios = t.Rows[i].ItemArray;
                    switch (privilegios[0].ToString())
                    {

                        case "catPersonal":

                            btninsertarempleado.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarempleado.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catPuestos":

                            btninsertarcargo.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarcargo.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catProveedores":

                            btninsertarproveedor.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarproveedor.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarproveedor.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarproveedor.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catRefacciones":

                            btninsertarrefaccion.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarrefaccion.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarrefaccion.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarrefaccion.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catEmpresas":

                            btninsertarempresa.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarempresa.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarempresa.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminarempresa.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "catTipos":

                            btninsertartipo.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultartipo.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificartipo.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminartipo.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "ordencompra":

                            btninsertarrequisicion.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarrequisicion.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarrequisicion.BackgroundImage = v.Checked(privilegios[4]);
                            break;
                        case "comparativas":

                            btninsertarcomparativa.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultarcomparativa.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificarcomparativa.BackgroundImage = v.Checked(privilegios[4]);
                            break;
                        case "Almacen":

                            btninsertaralmacen.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultaralmacen.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificaralmacen.BackgroundImage = v.Checked(privilegios[4]);
                            break;
                        case "historial":
                            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[3]);
                            break;
                        case "catGiros":
                            btninsertargiro.BackgroundImage = v.Checked(privilegios[2]);
                            btnconsultargiro.BackgroundImage = v.Checked(privilegios[3]);
                            btnmodificargiro.BackgroundImage = v.Checked(privilegios[4]);
                            btneliminargiro.BackgroundImage = v.Checked(privilegios[5]);
                            break;
                        case "changeiva":
                            btnmodificariva.BackgroundImage = v.Checked(privilegios[4]);
                            break;

                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[,] respaldo = new string[12, 2];
            respaldo[0, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
            respaldo[0, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarempleado.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarempleado.BackgroundImage).ToString() + "/" + v.Checked(btneliminarempleado.BackgroundImage).ToString();
            respaldo[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[1, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarcargo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarcargo.BackgroundImage).ToString() + "/" + v.Checked(btneliminarcargo.BackgroundImage).ToString();
            respaldo[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[2, 1] = v.Checked(btninsertarproveedor.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarproveedor.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarproveedor.BackgroundImage).ToString() + "/" + v.Checked(btneliminarproveedor.BackgroundImage).ToString();
            respaldo[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarrefaccion.BackgroundImage) == v.check || v.ImageToString(btnconsultarrefaccion.BackgroundImage) == v.check || v.ImageToString(btnmodificarrefaccion.BackgroundImage) == v.check || v.ImageToString(btneliminarrefaccion.BackgroundImage) == v.check)).ToString();
            respaldo[3, 1] = v.Checked(btninsertarrefaccion.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarrefaccion.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarrefaccion.BackgroundImage).ToString() + "/" + v.Checked(btneliminarrefaccion.BackgroundImage).ToString();
            respaldo[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarempresa.BackgroundImage) == v.check || v.ImageToString(btnconsultarempresa.BackgroundImage) == v.check || v.ImageToString(btnmodificarempresa.BackgroundImage) == v.check || v.ImageToString(btneliminarempresa.BackgroundImage) == v.check)).ToString();
            respaldo[4, 1] = v.Checked(btninsertarempresa.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarempresa.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarempresa.BackgroundImage).ToString() + "/" + v.Checked(btneliminarempresa.BackgroundImage).ToString();
            respaldo[5, 0] = v.getIntFrombool((v.ImageToString(btninsertargiro.BackgroundImage) == v.check || v.ImageToString(btnconsultargiro.BackgroundImage) == v.check || v.ImageToString(btnmodificargiro.BackgroundImage) == v.check || v.ImageToString(btneliminargiro.BackgroundImage) == v.check)).ToString();
            respaldo[5, 1] = v.Checked(btninsertargiro.BackgroundImage).ToString() + "/" + v.Checked(btnconsultargiro.BackgroundImage).ToString() + "/" + v.Checked(btnmodificargiro.BackgroundImage).ToString() + "/" + v.Checked(btneliminargiro.BackgroundImage).ToString();
            respaldo[6, 0] = v.getIntFrombool((v.ImageToString(btninsertartipo.BackgroundImage) == v.check || v.ImageToString(btnconsultartipo.BackgroundImage) == v.check || v.ImageToString(btnmodificartipo.BackgroundImage) == v.check || v.ImageToString(btneliminartipo.BackgroundImage) == v.check)).ToString();
            respaldo[6, 1] = v.Checked(btninsertartipo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultartipo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificartipo.BackgroundImage).ToString() + "/" + v.Checked(btneliminartipo.BackgroundImage).ToString();
            respaldo[7, 0] = v.getIntFrombool((v.ImageToString(btninsertarrequisicion.BackgroundImage) == v.check || v.ImageToString(btnconsultarrequisicion.BackgroundImage) == v.check || v.ImageToString(btnmodificarrequisicion.BackgroundImage) == v.check)).ToString();
            respaldo[7, 1] = v.Checked(btninsertarrequisicion.BackgroundImage).ToString()+ "/" +v.Checked(btnconsultarrequisicion.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarrequisicion.BackgroundImage).ToString();
            respaldo[8, 0] = v.getIntFrombool((v.ImageToString(btninsertarcomparativa.BackgroundImage) == v.check || v.ImageToString(btnconsultarcomparativa.BackgroundImage) == v.check || v.ImageToString(btnmodificarcomparativa.BackgroundImage) == v.check)).ToString();
            respaldo[8, 1] = v.Checked(btninsertarcomparativa.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarcomparativa.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarcomparativa.BackgroundImage).ToString();
            respaldo[9, 0] = v.getIntFrombool((v.ImageToString(btninsertaralmacen.BackgroundImage) == v.check || v.ImageToString(btnconsultaralmacen.BackgroundImage) == v.check || v.ImageToString(btnmodificaralmacen.BackgroundImage) == v.check)).ToString();
            respaldo[9, 1] = v.Checked(btninsertaralmacen.BackgroundImage).ToString() + "/" + v.Checked(btnconsultaralmacen.BackgroundImage).ToString() +"/" + v.Checked(btnmodificaralmacen.BackgroundImage).ToString();
            respaldo[10, 0] = v.getIntFrombool((v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check)).ToString();
            respaldo[10, 1] = "0" + "/" + v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
            respaldo[11, 0] = v.getIntFrombool((v.ImageToString(btnmodificariva.BackgroundImage) == v.check)).ToString();
            respaldo[11, 3] = v.Checked(btnmodificariva.BackgroundImage).ToString();
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

        private void btnconsultarempresa_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempresa.BackgroundImage) != v.check)
            {
                btneliminarempresa.BackgroundImage = btnmodificarempresa.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempresa.Enabled = btnmodificarempresa.Enabled = false;
            }
            else
            {
                btneliminarempresa.Enabled = btnmodificarempresa.Enabled = true;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void button4_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultargiro.BackgroundImage) != v.check)
            {
                btneliminargiro.BackgroundImage = btnmodificargiro.BackgroundImage = Properties.Resources.uncheck;
                btneliminargiro.Enabled = btnmodificargiro.Enabled = false;
            }
            else
            {
                btneliminargiro.Enabled = btnmodificargiro.Enabled = true;
            }
        }

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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnconsultarcomparativa_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarcomparativa.BackgroundImage) != v.check)
            {
                btnmodificarcomparativa.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarcomparativa.Enabled = false;
            }
            else
                btnmodificarcomparativa.Enabled = true;

        }
    }
}