using System;
using System.Data;
using System.Windows.Forms;
namespace controlFallos
{
    public partial class privilegiosSupervision : Form
    {
        int idUsuario;
        bool editar = false;
        DataTable t;
        validaciones v;
        string[] id;
        void buscarNombre() { lbltitle.Text = "Nombre del Empleado: " + v.getaData("SELECT CONCAT(coalesce(nombres,''),' ',coalesce(apPaterno,''),' ',coalesce(apMaterno,'')) as Nombre FROM cpersonal WHERE idPersona ='" + idUsuario + "'"); }
        public privilegiosSupervision(validaciones v, int idUsuario)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
        }
        public privilegiosSupervision(validaciones v) { this.v = v; InitializeComponent(); }
        private void CambiarEstado_Click(object sender, EventArgs e) { v.CambiarEstado_Click(sender, e); }
        private void button33_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in panel2.Controls)
            {
                if (ctrl is Button)
                    ctrl.BackgroundImage = Properties.Resources.uncheck;
            }
            catPersonal cat = (catPersonal)Owner;
            cat.privilegios = null;
        }
        private void privilegiosSupervision_Load(object sender, EventArgs e)
        {
            if (idUsuario > 0)
            {
                buscarNombre();
                exitenPrivilegios();
            }
            lbltitle.Left = (panel1.Width - lbltitle.Size.Width) / 2;
        }
        private void btnconsultararea_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultararea.BackgroundImage) != v.check)
            {
                btneliminararea.Enabled = btnmodificararea.Enabled = false;
                btneliminararea.BackgroundImage = btnmodificararea.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminararea.Enabled = btnmodificararea.Enabled = true;
        }
        private void btnconsultarempresa_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempresa.BackgroundImage) != v.check)
            {
                btneliminarempresa.Enabled = btnmodificarempresa.Enabled = false;
                btneliminarempresa.BackgroundImage = btnmodificarempresa.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarempresa.Enabled = btnmodificarempresa.Enabled = true;
        }
        private void btnconsultarempleado_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarempleado.BackgroundImage) != v.check)
            {
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = false;
                btneliminarempleado.BackgroundImage = btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarempleado.Enabled = btnmodificarempleado.Enabled = true;
        }
        private void btnconsultarcargo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarcargo.BackgroundImage) != v.check)
            {
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = false;
                btneliminarcargo.BackgroundImage = btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarcargo.Enabled = btnmodificarcargo.Enabled = true;
        }
        private void btnconsultarservicio_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarservicio.BackgroundImage) != v.check)
            {
                btneliminarservicio.Enabled = btnmodificarservicio.Enabled = false;
                btneliminarservicio.BackgroundImage = btnmodificarservicio.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarservicio.Enabled = btnmodificarservicio.Enabled = true;
        }
        private void btnconsultarunidad_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarunidad.BackgroundImage) != v.check)
            {
                btneliminarunidad.Enabled = btnmodificarunidad.Enabled = false;
                btneliminarunidad.BackgroundImage = btnmodificarunidad.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarunidad.Enabled = btnmodificarunidad.Enabled = true;
        }
        private void btnconsultarsuper_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarsuper.BackgroundImage) != v.check)
            {
                btnmodificarsuper.Enabled = false;
                btnmodificarsuper.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btnmodificarsuper.Enabled = true;
        }
        private void button32_Click(object sender, EventArgs e)
        {
            string[,] privilegios = new string[17, 6];
            string[,] respaldo = new string[17, 5];
            respaldo[0, 0] = privilegios[0, 0] = v.getIntFrombool((v.ImageToString(btninsertararea.BackgroundImage) == v.check || v.ImageToString(btnconsultararea.BackgroundImage) == v.check || v.ImageToString(btnmodificararea.BackgroundImage) == v.check || v.ImageToString(btneliminararea.BackgroundImage) == v.check)).ToString();
            respaldo[0, 1] = privilegios[0, 1] = v.Checked(btninsertararea.BackgroundImage).ToString();
            respaldo[0, 2] = privilegios[0, 2] = v.Checked(btnconsultararea.BackgroundImage).ToString();
            respaldo[0, 3] = privilegios[0, 3] = v.Checked(btnmodificararea.BackgroundImage).ToString();
            respaldo[0, 4] = privilegios[0, 4] = v.Checked(btneliminararea.BackgroundImage).ToString();
            privilegios[0, 5] = "catAreas";
            respaldo[1, 0] = privilegios[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempresa.BackgroundImage) == v.check || v.ImageToString(btnconsultarempresa.BackgroundImage) == v.check || v.ImageToString(btnmodificarempresa.BackgroundImage) == v.check || v.ImageToString(btneliminarempresa.BackgroundImage) == v.check)).ToString();
            respaldo[1, 1] = privilegios[1, 1] = v.Checked(btninsertarempresa.BackgroundImage).ToString();
            respaldo[1, 2] = privilegios[1, 2] = v.Checked(btnconsultarempresa.BackgroundImage).ToString();
            respaldo[1, 3] = privilegios[1, 3] = v.Checked(btnmodificarempresa.BackgroundImage).ToString();
            respaldo[1, 4] = privilegios[1, 4] = v.Checked(btneliminarempresa.BackgroundImage).ToString();
            privilegios[1, 5] = "catEmpresas";
            respaldo[2, 0] = privilegios[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
            respaldo[2, 1] = privilegios[2, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString();
            respaldo[2, 2] = privilegios[2, 2] = v.Checked(btnconsultarempleado.BackgroundImage).ToString();
            respaldo[2, 3] = privilegios[2, 3] = v.Checked(btnmodificarempleado.BackgroundImage).ToString();
            respaldo[2, 4] = privilegios[2, 4] = v.Checked(btneliminarempleado.BackgroundImage).ToString();
            privilegios[2, 5] = "catPersonal";
            respaldo[3, 0] = privilegios[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[3, 1] = privilegios[3, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString();
            respaldo[3, 2] = privilegios[3, 2] = v.Checked(btnconsultarcargo.BackgroundImage).ToString();
            respaldo[3, 3] = privilegios[3, 3] = v.Checked(btnmodificarcargo.BackgroundImage).ToString();
            respaldo[3, 4] = privilegios[3, 4] = v.Checked(btneliminarcargo.BackgroundImage).ToString();
            privilegios[3, 5] = "catPuestos";
            respaldo[4, 0] = privilegios[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarservicio.BackgroundImage) == v.check || v.ImageToString(btnconsultarservicio.BackgroundImage) == v.check || v.ImageToString(btnmodificarservicio.BackgroundImage) == v.check || v.ImageToString(btneliminarservicio.BackgroundImage) == v.check)).ToString();
            respaldo[4, 1] = privilegios[4, 1] = v.Checked(btninsertarservicio.BackgroundImage).ToString();
            respaldo[4, 2] = privilegios[4, 2] = v.Checked(btnconsultarservicio.BackgroundImage).ToString();
            respaldo[4, 3] = privilegios[4, 3] = v.Checked(btnmodificarservicio.BackgroundImage).ToString();
            respaldo[4, 4] = privilegios[4, 4] = v.Checked(btneliminarservicio.BackgroundImage).ToString();
            privilegios[4, 5] = "catServicios";
            respaldo[5, 0] = privilegios[5, 0] = v.getIntFrombool((v.ImageToString(btninsertartipo.BackgroundImage) == v.check || v.ImageToString(btnconsultartipo.BackgroundImage) == v.check || v.ImageToString(btnmodificartipo.BackgroundImage) == v.check || v.ImageToString(btneliminartipo.BackgroundImage) == v.check)).ToString();
            respaldo[5, 1] = privilegios[5, 1] = v.Checked(btninsertartipo.BackgroundImage).ToString();
            respaldo[5, 2] = privilegios[5, 2] = v.Checked(btnconsultartipo.BackgroundImage).ToString();
            respaldo[5, 3] = privilegios[5, 3] = v.Checked(btnmodificartipo.BackgroundImage).ToString();
            respaldo[5, 4] = privilegios[5, 4] = v.Checked(btneliminartipo.BackgroundImage).ToString();
            privilegios[5, 5] = "catTipos";
            respaldo[6, 0] = privilegios[6, 0] = v.getIntFrombool((v.ImageToString(btninsertarincidencia.BackgroundImage) == v.check || v.ImageToString(btnconsultarincidencia.BackgroundImage) == v.check || v.ImageToString(btnmodificarincidencia.BackgroundImage) == v.check || v.ImageToString(btneliminarincidencia.BackgroundImage) == v.check)).ToString();
            respaldo[6, 1] = privilegios[6, 1] = v.Checked(btninsertarincidencia.BackgroundImage).ToString();
            respaldo[6, 2] = privilegios[6, 2] = v.Checked(btnconsultarincidencia.BackgroundImage).ToString();
            respaldo[6, 3] = privilegios[6, 3] = v.Checked(btnmodificarincidencia.BackgroundImage).ToString();
            respaldo[6, 4] = privilegios[6, 4] = v.Checked(btneliminarincidencia.BackgroundImage).ToString();
            privilegios[6, 5] = "catincidencias";
            respaldo[7, 0] = privilegios[7, 0] = v.getIntFrombool((v.ImageToString(btninsertarestacion.BackgroundImage) == v.check || v.ImageToString(btnconsultarestacion.BackgroundImage) == v.check || v.ImageToString(btnmodificarestacion.BackgroundImage) == v.check || v.ImageToString(btneliminarestacion.BackgroundImage) == v.check)).ToString();
            respaldo[7, 1] = privilegios[7, 1] = v.Checked(btninsertarestacion.BackgroundImage).ToString();
            respaldo[7, 2] = privilegios[7, 2] = v.Checked(btnconsultarestacion.BackgroundImage).ToString();
            respaldo[7, 3] = privilegios[7, 3] = v.Checked(btnmodificarestacion.BackgroundImage).ToString();
            respaldo[7, 4] = privilegios[7, 4] = v.Checked(btneliminarestacion.BackgroundImage).ToString();
            privilegios[7, 5] = "catestaciones";
            respaldo[8, 0] = privilegios[8, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check || v.ImageToString(btneliminarunidad.BackgroundImage) == v.check)).ToString();
            respaldo[8, 1] = privilegios[8, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString();
            respaldo[8, 2] = privilegios[8, 2] = v.Checked(btnconsultarunidad.BackgroundImage).ToString();
            respaldo[8, 3] = privilegios[8, 3] = v.Checked(btnmodificarunidad.BackgroundImage).ToString();
            respaldo[8, 4] = privilegios[8, 4] = v.Checked(btneliminarunidad.BackgroundImage).ToString();
            privilegios[8, 5] = "catUnidades";
            respaldo[9, 0] = privilegios[9, 0] = v.getIntFrombool((v.ImageToString(btninsertarmodelo.BackgroundImage) == v.check || v.ImageToString(btnconsultarmodelo.BackgroundImage) == v.check || v.ImageToString(btnmodificarmodelo.BackgroundImage) == v.check || v.ImageToString(btneliminarmodelo.BackgroundImage) == v.check)).ToString();
            respaldo[9, 1] = privilegios[9, 1] = v.Checked(btninsertarmodelo.BackgroundImage).ToString();
            respaldo[9, 2] = privilegios[9, 2] = v.Checked(btnconsultarmodelo.BackgroundImage).ToString();
            respaldo[9, 3] = privilegios[9, 3] = v.Checked(btnmodificarmodelo.BackgroundImage).ToString();
            respaldo[9, 4] = privilegios[9, 4] = v.Checked(btneliminarmodelo.BackgroundImage).ToString();
            privilegios[9, 5] = "catmodelos";
            respaldo[10, 0] = privilegios[10, 0] = v.getIntFrombool((v.ImageToString(btninsertarol.BackgroundImage) == v.check || v.ImageToString(btnconsultarol.BackgroundImage) == v.check || v.ImageToString(btnmodificarol.BackgroundImage) == v.check || v.ImageToString(btndesactivarol.BackgroundImage) == v.check)).ToString();
            respaldo[10, 1] = privilegios[10, 1] = v.Checked(btninsertarol.BackgroundImage).ToString();
            respaldo[10, 2] = privilegios[10, 2] = v.Checked(btnconsultarol.BackgroundImage).ToString();
            respaldo[10, 3] = privilegios[10, 3] = v.Checked(btnmodificarol.BackgroundImage).ToString();
            respaldo[10, 4] = privilegios[10, 4] = v.Checked(btndesactivarol.BackgroundImage).ToString();
            privilegios[10, 5] = "CatRoles";

            respaldo[11, 0] = privilegios[11, 0] = v.getIntFrombool((v.ImageToString(btninsertarsuper.BackgroundImage) == v.check || v.ImageToString(btnconsultarsuper.BackgroundImage) == v.check || v.ImageToString(btnmodificarsuper.BackgroundImage) == v.check)).ToString();
            respaldo[11, 1] = privilegios[11, 1] = v.Checked(btninsertarsuper.BackgroundImage).ToString();
            respaldo[11, 2] = privilegios[11, 2] = v.Checked(btnconsultarsuper.BackgroundImage).ToString();
            respaldo[11, 3] = privilegios[11, 3] = v.Checked(btnmodificarsuper.BackgroundImage).ToString();
            respaldo[11, 4] = privilegios[11, 4] = "0";
            privilegios[11, 5] = "Form1";

            respaldo[12, 0] = privilegios[12, 0] = v.getIntFrombool((v.ImageToString(btninsertarpercances.BackgroundImage) == v.check || v.ImageToString(btnconsultarpercances.BackgroundImage) == v.check || v.ImageToString(btnmodificarpercances.BackgroundImage) == v.check)).ToString();
            respaldo[12, 1] = privilegios[12, 1] = v.Checked(btninsertarpercances.BackgroundImage).ToString();
            respaldo[12, 2] = privilegios[12, 2] = v.Checked(btnconsultarpercances.BackgroundImage).ToString();
            respaldo[12, 3] = privilegios[12, 3] = v.Checked(btnmodificarpercances.BackgroundImage).ToString();
            respaldo[12, 4] = privilegios[12, 4] = "0";
            privilegios[12, 5] = "percances";


            respaldo[13, 0] = privilegios[13, 0] = v.getIntFrombool((v.ImageToString(btninsertarrp.BackgroundImage) == v.check || v.ImageToString(btnconsultarrp.BackgroundImage) == v.check || v.ImageToString(btnmodificarrp.BackgroundImage) == v.check)).ToString();
            respaldo[13, 1] = privilegios[13, 1] = v.Checked(btninsertarrp.BackgroundImage).ToString();
            respaldo[13, 2] = privilegios[13, 2] = v.Checked(btnconsultarrp.BackgroundImage).ToString();
            respaldo[13, 3] = privilegios[13, 3] = v.Checked(btnmodificarrp.BackgroundImage).ToString();
            respaldo[13, 4] = privilegios[13, 4] = "0";
            privilegios[13, 5] = "repPersonal";


            respaldo[14, 0] = privilegios[14, 0] = v.getIntFrombool((v.ImageToString(btninsertarip.BackgroundImage) == v.check || v.ImageToString(btnconsultarip.BackgroundImage) == v.check || v.ImageToString(btnmodificarip.BackgroundImage) == v.check)).ToString();
            respaldo[14, 1] = privilegios[14, 1] = v.Checked(btninsertarip.BackgroundImage).ToString();
            respaldo[14, 2] = privilegios[14, 2] = v.Checked(btnconsultarip.BackgroundImage).ToString();
            respaldo[14, 3] = privilegios[14, 3] = v.Checked(btnmodificarip.BackgroundImage).ToString();
            respaldo[14, 4] = privilegios[14, 4] = "0";
            privilegios[14, 5] = "IncidenciaPersonal";

            respaldo[15, 0] = privilegios[15, 0] = v.getIntFrombool(v.ImageToString(btnmodificarencabezados.BackgroundImage) == v.check).ToString();
            respaldo[15, 1] = privilegios[15, 1] = "0";
            respaldo[15, 2] = privilegios[15, 2] = "0";
            respaldo[15, 3] = privilegios[15, 3] = v.Checked(btnmodificarencabezados.BackgroundImage).ToString();
            respaldo[15, 4] = privilegios[15, 4] = "0";
            privilegios[15, 5] = "encabezados";
            respaldo[16, 0] = privilegios[16, 0] = v.getIntFrombool(v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check).ToString();
            respaldo[16, 1] = privilegios[16, 1] = "0";
            respaldo[16, 2] = privilegios[16, 2] = v.Checked(btnconsultarhistorial.BackgroundImage).ToString();
            respaldo[16, 3] = privilegios[16, 3] = "0";
            respaldo[16, 4] = privilegios[16, 4] = "0";
            privilegios[16, 5] = "historial";
            string mensaje = "";
            if (!v.todosFalsos(respaldo))
            {
                if (!editar)
                {
                    if (idUsuario > 0)
                    {
                        if (Convert.ToInt32(v.getaData(string.Format("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal={0}", idUsuario))) > 0 || editar)
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
                            mensaje = "Asignado";
                            MessageBox.Show("Se Han " + mensaje + " los Privilegios Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            catPersonal cat = (catPersonal)Owner;
                            cat.lblprivilegios.Text = "Actualizar Privilegios";
                        }
                        else
                        {
                            catPersonal Cat = (catPersonal)Owner;
                            Cat.privilegios = privilegios;
                        }
                    }
                    else
                    {
                        catPersonal Cat = (catPersonal)Owner;
                        Cat.privilegios = privilegios;
                    }
                }
                else
                {
                    if (sehicieronModificaciones(t, respaldo))
                    {
                        //if (privilegios.Length >0)
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
                        MessageBox.Show("No se Realizaron Modificaciones", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        public void insertarPrivilegios(string[,] privilegios)
        {
            btninsertararea.BackgroundImage = v.Checked(privilegios[0, 1]);
            btnconsultararea.BackgroundImage = v.Checked(privilegios[0, 2]);
            btnmodificararea.BackgroundImage = v.Checked(privilegios[0, 3]);
            btneliminararea.BackgroundImage = v.Checked(privilegios[0, 4]);
            btninsertarempresa.BackgroundImage = v.Checked(privilegios[1, 1]);
            btnconsultarempresa.BackgroundImage = v.Checked(privilegios[1, 2]);
            btnmodificarempresa.BackgroundImage = v.Checked(privilegios[1, 3]);
            btneliminarempresa.BackgroundImage = v.Checked(privilegios[1, 4]);
            btninsertarempleado.BackgroundImage = v.Checked(privilegios[2, 1]);
            btnconsultarempleado.BackgroundImage = v.Checked(privilegios[2, 2]);
            btnmodificarempleado.BackgroundImage = v.Checked(privilegios[2, 3]);
            btneliminarempleado.BackgroundImage = v.Checked(privilegios[2, 4]);
            btninsertarcargo.BackgroundImage = v.Checked(privilegios[3, 1]);
            btnconsultarcargo.BackgroundImage = v.Checked(privilegios[3, 2]);
            btnmodificarcargo.BackgroundImage = v.Checked(privilegios[3, 3]);
            btneliminarcargo.BackgroundImage = v.Checked(privilegios[3, 4]);
            btninsertarservicio.BackgroundImage = v.Checked(privilegios[4, 1]);
            btnconsultarservicio.BackgroundImage = v.Checked(privilegios[4, 2]);
            btnmodificarservicio.BackgroundImage = v.Checked(privilegios[4, 3]);
            btneliminarservicio.BackgroundImage = v.Checked(privilegios[4, 4]);
            btninsertartipo.BackgroundImage = v.Checked(privilegios[5, 1]);
            btnconsultartipo.BackgroundImage = v.Checked(privilegios[5, 2]);
            btnmodificartipo.BackgroundImage = v.Checked(privilegios[5, 3]);
            btneliminartipo.BackgroundImage = v.Checked(privilegios[5, 4]);
            btninsertarincidencia.BackgroundImage = v.Checked(privilegios[6, 1]);
            btnconsultarincidencia.BackgroundImage = v.Checked(privilegios[6, 2]);
            btnmodificarincidencia.BackgroundImage = v.Checked(privilegios[6, 3]);
            btneliminarincidencia.BackgroundImage = v.Checked(privilegios[6, 4]);
            btninsertarestacion.BackgroundImage = v.Checked(privilegios[7, 1]);
            btnconsultarestacion.BackgroundImage = v.Checked(privilegios[7, 2]);
            btnmodificarestacion.BackgroundImage = v.Checked(privilegios[7, 3]);
            btneliminarestacion.BackgroundImage = v.Checked(privilegios[7, 4]);
            btninsertarunidad.BackgroundImage = v.Checked(privilegios[8, 1]);
            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[8, 2]);
            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[8, 3]);
            btneliminarunidad.BackgroundImage = v.Checked(privilegios[8, 4]);
            btninsertarmodelo.BackgroundImage = v.Checked(privilegios[9, 1]);
            btnconsultarmodelo.BackgroundImage = v.Checked(privilegios[9, 2]);
            btnmodificarmodelo.BackgroundImage = v.Checked(privilegios[9, 3]);
            btneliminarmodelo.BackgroundImage = v.Checked(privilegios[9, 4]);
            btninsertarol.BackgroundImage = v.Checked(privilegios[10, 1]);
            btnconsultarol.BackgroundImage = v.Checked(privilegios[10, 2]);
            btnmodificarol.BackgroundImage = v.Checked(privilegios[10, 3]);
            btndesactivarol.BackgroundImage = v.Checked(privilegios[10, 4]);
            btninsertarsuper.BackgroundImage = v.Checked(privilegios[11, 1]);
            btnconsultarsuper.BackgroundImage = v.Checked(privilegios[11, 2]);
            btnmodificarsuper.BackgroundImage = v.Checked(privilegios[11, 3]);
            btninsertarpercances.BackgroundImage = v.Checked(privilegios[12, 1]);
            btnconsultarpercances.BackgroundImage = v.Checked(privilegios[12, 2]);
            btnmodificarpercances.BackgroundImage = v.Checked(privilegios[12, 3]);
            btninsertarrp.BackgroundImage = v.Checked(privilegios[13, 1]);
            btnconsultarrp.BackgroundImage = v.Checked(privilegios[13, 2]);
            btnmodificarrp.BackgroundImage = v.Checked(privilegios[13, 3]);
            btninsertarip.BackgroundImage = v.Checked(privilegios[14, 1]);
            btnconsultarip.BackgroundImage = v.Checked(privilegios[14, 2]);
            btnmodificarip.BackgroundImage = v.Checked(privilegios[14, 3]);
            btnmodificarencabezados.BackgroundImage = v.Checked(privilegios[15, 3]);
            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[16, 2]);
        }
        void exitenPrivilegios()
        {
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'")) > 0)
            {
                lbltexto.Text = "Actualizar Privilegios";
                t = (DataTable)v.getData("SELECT namform, ver,COALESCE(privilegios,'0/0/0/0') FROM privilegios WHERE usuariofkcpersonal='" + idUsuario + "'");
                id = v.getaData("SELECT group_concat(idprivilegio separator ';') from privilegios WHERE usuariofkcpersonal='" + idUsuario + "';").ToString().Split(';');
                editar = true;
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    object[] pr = t.Rows[i].ItemArray;
                    object[] privilegios = pr[2].ToString().Split('/');
                    switch (pr[0].ToString())
                    {
                        case "catAreas":
                            btninsertararea.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultararea.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificararea.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminararea.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catEmpresas":
                            btninsertarempresa.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarempresa.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarempresa.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarempresa.BackgroundImage = v.Checked(privilegios[3]);
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

                        case "catServicios":
                            btninsertarservicio.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarservicio.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarservicio.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarservicio.BackgroundImage = v.Checked(privilegios[3]);
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
                        case "catincidencias":
                            btninsertarincidencia.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarincidencia.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarincidencia.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarincidencia.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catestaciones":
                            btninsertarestacion.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarestacion.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarestacion.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarestacion.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catUnidades":
                            btninsertarunidad.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarunidad.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarunidad.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarunidad.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catmodelos":
                            btninsertarmodelo.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarmodelo.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarmodelo.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarmodelo.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "CatRoles":
                            btninsertarol.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarol.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarol.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btndesactivarol.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "Form1":
                            btninsertarsuper.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarsuper.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarsuper.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "percances":
                            btninsertarpercances.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarpercances.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarpercances.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "repPersonal":
                            btninsertarrp.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarrp.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarrp.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "IncidenciaPersonal":
                            btninsertarip.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarip.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarip.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "encabezados":
                            btnmodificarencabezados.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "historial":
                            btnconsultarhistorial.BackgroundImage = v.Checked(privilegios[1]);
                            break;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[,] respaldo = new string[17, 2];
            respaldo[0, 0] = v.getIntFrombool((v.ImageToString(btninsertararea.BackgroundImage) == v.check || v.ImageToString(btnconsultararea.BackgroundImage) == v.check || v.ImageToString(btnmodificararea.BackgroundImage) == v.check || v.ImageToString(btneliminararea.BackgroundImage) == v.check)).ToString();
            respaldo[0, 1] = v.Checked(btninsertararea.BackgroundImage).ToString() + "/" + v.Checked(btnconsultararea.BackgroundImage).ToString() + "/" + v.Checked(btnmodificararea.BackgroundImage).ToString() + "/" + v.Checked(btneliminararea.BackgroundImage).ToString();
            respaldo[1, 0] = v.getIntFrombool((v.ImageToString(btninsertarempresa.BackgroundImage) == v.check || v.ImageToString(btnconsultarempresa.BackgroundImage) == v.check || v.ImageToString(btnmodificarempresa.BackgroundImage) == v.check || v.ImageToString(btneliminarempresa.BackgroundImage) == v.check)).ToString();
            respaldo[1, 1] = v.Checked(btninsertarempresa.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarempresa.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarempresa.BackgroundImage).ToString() + "/" + v.Checked(btneliminarempresa.BackgroundImage).ToString();
            respaldo[2, 0] = v.getIntFrombool((v.ImageToString(btninsertarempleado.BackgroundImage) == v.check || v.ImageToString(btnconsultarempleado.BackgroundImage) == v.check || v.ImageToString(btnmodificarempleado.BackgroundImage) == v.check || v.ImageToString(btneliminarempleado.BackgroundImage) == v.check)).ToString();
            respaldo[2, 1] = v.Checked(btninsertarempleado.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarempleado.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarempleado.BackgroundImage).ToString() + "/" + v.Checked(btneliminarempleado.BackgroundImage).ToString();
            respaldo[3, 0] = v.getIntFrombool((v.ImageToString(btninsertarcargo.BackgroundImage) == v.check || v.ImageToString(btnconsultarcargo.BackgroundImage) == v.check || v.ImageToString(btnmodificarcargo.BackgroundImage) == v.check || v.ImageToString(btneliminarcargo.BackgroundImage) == v.check)).ToString();
            respaldo[3, 1] = v.Checked(btninsertarcargo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarcargo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarcargo.BackgroundImage).ToString() + "/" + v.Checked(btneliminarcargo.BackgroundImage).ToString();
            respaldo[4, 0] = v.getIntFrombool((v.ImageToString(btninsertarservicio.BackgroundImage) == v.check || v.ImageToString(btnconsultarservicio.BackgroundImage) == v.check || v.ImageToString(btnmodificarservicio.BackgroundImage) == v.check || v.ImageToString(btneliminarservicio.BackgroundImage) == v.check)).ToString();
            respaldo[4, 1] = v.Checked(btninsertarservicio.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarservicio.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarservicio.BackgroundImage).ToString() + "/" + v.Checked(btneliminarservicio.BackgroundImage).ToString();
            respaldo[5, 0] = v.getIntFrombool((v.ImageToString(btninsertartipo.BackgroundImage) == v.check || v.ImageToString(btnconsultartipo.BackgroundImage) == v.check || v.ImageToString(btnmodificartipo.BackgroundImage) == v.check || v.ImageToString(btneliminartipo.BackgroundImage) == v.check)).ToString();
            respaldo[5, 1] = v.Checked(btninsertartipo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultartipo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificartipo.BackgroundImage).ToString() + "/" + v.Checked(btneliminartipo.BackgroundImage).ToString();
            respaldo[6, 0] = v.getIntFrombool((v.ImageToString(btninsertarincidencia.BackgroundImage) == v.check || v.ImageToString(btnconsultarincidencia.BackgroundImage) == v.check || v.ImageToString(btnmodificarincidencia.BackgroundImage) == v.check || v.ImageToString(btneliminarincidencia.BackgroundImage) == v.check)).ToString();
            respaldo[6, 1] = v.Checked(btninsertarincidencia.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarincidencia.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarincidencia.BackgroundImage).ToString() + "/" + v.Checked(btneliminarincidencia.BackgroundImage).ToString();
            respaldo[7, 0] = v.getIntFrombool((v.ImageToString(btninsertarestacion.BackgroundImage) == v.check || v.ImageToString(btnconsultarestacion.BackgroundImage) == v.check || v.ImageToString(btnmodificarestacion.BackgroundImage) == v.check || v.ImageToString(btneliminarestacion.BackgroundImage) == v.check)).ToString();
            respaldo[7, 1] = v.Checked(btninsertarestacion.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarestacion.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarestacion.BackgroundImage).ToString() + "/" + v.Checked(btneliminarestacion.BackgroundImage).ToString();
            respaldo[8, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check || v.ImageToString(btneliminarunidad.BackgroundImage) == v.check)).ToString();
            respaldo[8, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarunidad.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarunidad.BackgroundImage).ToString() + "/" + v.Checked(btneliminarunidad.BackgroundImage).ToString();

            respaldo[9, 0] = v.getIntFrombool((v.ImageToString(btninsertarmodelo.BackgroundImage) == v.check || v.ImageToString(btnconsultarmodelo.BackgroundImage) == v.check || v.ImageToString(btnmodificarmodelo.BackgroundImage) == v.check || v.ImageToString(btneliminarmodelo.BackgroundImage) == v.check)).ToString();
            respaldo[9, 1] = v.Checked(btninsertarmodelo.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarmodelo.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarmodelo.BackgroundImage).ToString() + "/" + v.Checked(btneliminarmodelo.BackgroundImage).ToString();

            respaldo[10, 0] = v.getIntFrombool((v.ImageToString(btninsertarol.BackgroundImage) == v.check || v.ImageToString(btnconsultarol.BackgroundImage) == v.check || v.ImageToString(btnmodificarol.BackgroundImage) == v.check || v.ImageToString(btndesactivarol.BackgroundImage) == v.check)).ToString();
            respaldo[10, 1] = v.Checked(btninsertarol.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarol.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarol.BackgroundImage).ToString() + "/" + v.Checked(btndesactivarol.BackgroundImage).ToString();

            respaldo[11, 0] = v.getIntFrombool((v.ImageToString(btninsertarsuper.BackgroundImage) == v.check || v.ImageToString(btnconsultarsuper.BackgroundImage) == v.check || v.ImageToString(btnmodificarsuper.BackgroundImage) == v.check)).ToString();
            respaldo[11, 1] = v.Checked(btninsertarsuper.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarsuper.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarsuper.BackgroundImage).ToString();

            respaldo[12, 0] = v.getIntFrombool((v.ImageToString(btninsertarpercances.BackgroundImage) == v.check || v.ImageToString(btnconsultarpercances.BackgroundImage) == v.check || v.ImageToString(btnmodificarpercances.BackgroundImage) == v.check)).ToString();
            respaldo[12, 1] = v.Checked(btninsertarpercances.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarpercances.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarpercances.BackgroundImage).ToString();

            respaldo[13, 0] = v.getIntFrombool((v.ImageToString(btninsertarrp.BackgroundImage) == v.check || v.ImageToString(btnconsultarrp.BackgroundImage) == v.check || v.ImageToString(btnmodificarrp.BackgroundImage) == v.check)).ToString();
            respaldo[13, 1] = v.Checked(btninsertarrp.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarrp.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarrp.BackgroundImage).ToString();

            respaldo[14, 0] = v.getIntFrombool((v.ImageToString(btninsertarip.BackgroundImage) == v.check || v.ImageToString(btnconsultarip.BackgroundImage) == v.check || v.ImageToString(btnmodificarip.BackgroundImage) == v.check)).ToString();
            respaldo[14, 1] = v.Checked(btninsertarip.BackgroundImage).ToString() + "/" + v.Checked(btnconsultarip.BackgroundImage).ToString() + "/" + v.Checked(btnmodificarip.BackgroundImage).ToString();

            respaldo[15, 0] = v.getIntFrombool((v.ImageToString(btninsertarsuper.BackgroundImage) == v.check || v.ImageToString(btnconsultarsuper.BackgroundImage) == v.check || v.ImageToString(btnmodificarsuper.BackgroundImage) == v.check)).ToString();
            respaldo[15, 1] = "0" + "/" + "0" + "/" + v.Checked(btnmodificarencabezados.BackgroundImage).ToString();

            respaldo[16, 0] = v.getIntFrombool(v.ImageToString(btnconsultarhistorial.BackgroundImage) == v.check).ToString();
            respaldo[16, 1] = "0" + "/" + v.Checked(btnconsultarhistorial.BackgroundImage).ToString() + "/" + "0";
            catPersonal cat = (catPersonal)Owner;


            if (editar)
            {
                if (sehicieronModificaciones(t, respaldo))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                    res = sehicieronModificaciones(cat.privilegios, respaldo);
                if (res)
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Privilegios. ¿Desea Guardarlas?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        button32_Click(sender, e);
                    }
                }
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
                btneliminartipo.Enabled = btnmodificartipo.Enabled = true;
        }
        private void btnconsultarincidencia_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarincidencia.BackgroundImage) != v.check)
            {
                btneliminarincidencia.Enabled = btnmodificarincidencia.Enabled = false;
                btneliminarincidencia.BackgroundImage = btnmodificarincidencia.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarincidencia.Enabled = btnmodificarincidencia.Enabled = true;
        }
        private void btnconsultarestacion_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarestacion.BackgroundImage) != v.check)
            {
                btneliminarestacion.Enabled = btnmodificarestacion.Enabled = false;
                btneliminarestacion.BackgroundImage = btnmodificarestacion.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarestacion.Enabled = btnmodificarestacion.Enabled = true;
        }

        private void btnconsultarpercances_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarpercances.BackgroundImage) != v.check)
            {
                btnmodificarpercances.Enabled = false;
                btnmodificarpercances.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btnmodificarpercances.Enabled = true;
        }

        private void btnconsultarrp_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarrp.BackgroundImage) != v.check)
            {
                btnmodificarrp.Enabled = false;
                btnmodificarrp.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btnmodificarrp.Enabled = true;
        }

        private void btnconsultarip_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarip.BackgroundImage) != v.check)
            {
                btnmodificarip.Enabled = false;
                btnmodificarip.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btnmodificarip.Enabled = true;
        }

        private void btnconsultarmodelo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarmodelo.BackgroundImage) != v.check)
            {
                btneliminarmodelo.Enabled = btnmodificarmodelo.Enabled = false;
                btneliminarmodelo.BackgroundImage = btnmodificarmodelo.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btneliminarmodelo.Enabled = btnmodificarmodelo.Enabled = true;
        }

        private void btnconsultarol_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (v.ImageToString(btnconsultarol.BackgroundImage) != v.check)
            {
                btndesactivarol.Enabled = btnmodificarol.Enabled = false;
                btndesactivarol.BackgroundImage = btnmodificarol.BackgroundImage = Properties.Resources.uncheck;
            }
            else
                btndesactivarol.Enabled = btnmodificarol.Enabled = true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
