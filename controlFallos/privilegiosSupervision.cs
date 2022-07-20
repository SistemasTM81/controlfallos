using System;
using System.Data;
using System.Windows.Forms;
namespace controlFallos
{
    public partial class privilegiosSupervision : Form
    {
        int idUsuario, proveine, nuevopermiso = 0;
        bool editar = false;
        DataTable t;
        validaciones v;
        checarbotones cb;
        string[,] privilegios = new string[32, 6];
        string[,] respaldo = new string[32, 5];
        string[] id;
        void buscarNombre() { lbltitle.Text = "Nombre del Empleado: " + v.getaData("SELECT CONCAT(coalesce(nombres,''),' ',coalesce(apPaterno,''),' ',coalesce(apMaterno,'')) as Nombre FROM cpersonal WHERE idPersona ='" + idUsuario + "'"); }
        public privilegiosSupervision(validaciones v, int idUsuario, int proviene)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.proveine = proviene;
        }
        public privilegiosSupervision(validaciones v, int proviene) { this.v = v; this.proveine = proviene; InitializeComponent(); }
        private void CambiarEstado_Click(object sender, EventArgs e) { v.CambiarEstado_Click(sender, e); }

        void cambios(object sender, EventArgs e)
        {
            cb = new checarbotones(v);
            cb.cambiarestado(sender, e);

        }

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
            habilitar_privilegios(proveine);
            combo();

        }
        void combo()
        {
            v.comboswithuot(puestoCB, new string[] { "--Seleccione el Puesto--", "Administrador", "Auxiliar de Almacen", "Compras", "Encargado de Almacen", "Finanzas", "Solo Consulta" });
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

            //mantyenimiento
            respaldo[17, 0] = privilegios[17, 0] = v.getIntFrombool((v.ImageToString(btninsertarfallo.BackgroundImage) == v.check || v.ImageToString(btnconsultarfallo.BackgroundImage) == v.check || v.ImageToString(btnmodificarfallo.BackgroundImage) == v.check || v.ImageToString(btneliminarfallo.BackgroundImage) == v.check)).ToString();
            respaldo[17, 1] = privilegios[17, 1] = v.Checked(btninsertarfallo.BackgroundImage).ToString();
            respaldo[17, 2] = privilegios[17, 2] = v.Checked(btnconsultarfallo.BackgroundImage).ToString();
            respaldo[17, 3] = privilegios[17, 3] = v.Checked(btnmodificarfallo.BackgroundImage).ToString();
            respaldo[17, 4] = privilegios[17, 4] = v.Checked(btneliminarfallo.BackgroundImage).ToString();
            privilegios[17, 5] = "catfallosGrales";


            respaldo[18, 0] = privilegios[18, 0] = v.getIntFrombool((v.ImageToString(btninsertarunidad.BackgroundImage) == v.check || v.ImageToString(btnconsultarunidad.BackgroundImage) == v.check || v.ImageToString(btnmodificarunidad.BackgroundImage) == v.check)).ToString();
            respaldo[18, 1] = privilegios[18, 1] = v.Checked(btninsertarunidad.BackgroundImage).ToString();
            respaldo[18, 2] = privilegios[18, 2] = v.Checked(btnconsultarunidad.BackgroundImage).ToString();
            respaldo[18, 3] = privilegios[18, 3] = v.Checked(btnmodificarunidad.BackgroundImage).ToString();
            respaldo[18, 4] = privilegios[18, 4] = "0";
            privilegios[18, 5] = "catUnidades";
            respaldo[19, 0] = privilegios[19, 0] = v.getIntFrombool((v.ImageToString(btninsertarmante.BackgroundImage) == v.check || v.ImageToString(btnconsultarmante.BackgroundImage) == v.check || v.ImageToString(btnmodificarmante.BackgroundImage) == v.check)).ToString();
            respaldo[19, 1] = privilegios[19, 1] = v.Checked(btninsertarmante.BackgroundImage).ToString();
            respaldo[19, 2] = privilegios[19, 2] = v.Checked(btnconsultarmante.BackgroundImage).ToString();
            respaldo[19, 3] = privilegios[19, 3] = v.Checked(btnmodificarmante.BackgroundImage).ToString();
            respaldo[19, 4] = privilegios[19, 4] = "0";
            privilegios[19, 5] = "Mantenimiento";

            //ALMACEN



            respaldo[20, 0] = privilegios[20, 0] = v.getIntFrombool((v.ImageToString(btninsertarproveedor.BackgroundImage) == v.check || v.ImageToString(btnconsultarproveedor.BackgroundImage) == v.check || v.ImageToString(btnmodificarproveedor.BackgroundImage) == v.check || v.ImageToString(btneliminarproveedor.BackgroundImage) == v.check)).ToString();
            respaldo[20, 1] = privilegios[20, 1] = v.Checked(btninsertarproveedor.BackgroundImage).ToString();
            respaldo[20, 2] = privilegios[20, 2] = v.Checked(btnconsultarproveedor.BackgroundImage).ToString();
            respaldo[20, 3] = privilegios[20, 3] = v.Checked(btnmodificarproveedor.BackgroundImage).ToString();
            respaldo[20, 4] = privilegios[20, 4] = v.Checked(btneliminarproveedor.BackgroundImage).ToString();
            privilegios[20, 5] = "catProveedores";
            respaldo[21, 0] = privilegios[21, 0] = v.getIntFrombool((v.ImageToString(btninsertarrefaccion.BackgroundImage) == v.check || v.ImageToString(btnconsultarrefaccion.BackgroundImage) == v.check || v.ImageToString(btnmodificarrefaccion.BackgroundImage) == v.check || v.ImageToString(btneliminarrefaccion.BackgroundImage) == v.check)).ToString();
            respaldo[21, 1] = privilegios[21, 1] = v.Checked(btninsertarrefaccion.BackgroundImage).ToString();
            respaldo[21, 2] = privilegios[21, 2] = v.Checked(btnconsultarrefaccion.BackgroundImage).ToString();
            respaldo[21, 3] = privilegios[21, 3] = v.Checked(btnmodificarrefaccion.BackgroundImage).ToString();
            respaldo[21, 4] = privilegios[21, 4] = v.Checked(btneliminarrefaccion.BackgroundImage).ToString();
            privilegios[21, 5] = "catRefacciones";
            respaldo[22, 0] = privilegios[22, 0] = v.getIntFrombool((v.ImageToString(btninsertarempresa.BackgroundImage) == v.check || v.ImageToString(btnconsultarempresa.BackgroundImage) == v.check || v.ImageToString(btnmodificarempresa.BackgroundImage) == v.check || v.ImageToString(btneliminarempresa.BackgroundImage) == v.check)).ToString();
            respaldo[22, 1] = privilegios[22, 1] = v.Checked(btninsertarempresa.BackgroundImage).ToString();
            respaldo[22, 2] = privilegios[22, 2] = v.Checked(btnconsultarempresa.BackgroundImage).ToString();
            respaldo[22, 3] = privilegios[22, 3] = v.Checked(btnmodificarempresa.BackgroundImage).ToString();
            respaldo[22, 4] = privilegios[22, 4] = v.Checked(btneliminarempresa.BackgroundImage).ToString();
            privilegios[22, 5] = "catEmpresas";
            respaldo[23, 0] = privilegios[23, 0] = v.getIntFrombool((v.ImageToString(btninsertargiro.BackgroundImage) == v.check || v.ImageToString(btnconsultargiro.BackgroundImage) == v.check || v.ImageToString(btnmodificargiro.BackgroundImage) == v.check || v.ImageToString(btneliminargiro.BackgroundImage) == v.check)).ToString();
            respaldo[23, 1] = privilegios[23, 1] = v.Checked(btninsertargiro.BackgroundImage).ToString();
            respaldo[23, 2] = privilegios[23, 2] = v.Checked(btnconsultargiro.BackgroundImage).ToString();
            respaldo[23, 3] = privilegios[23, 3] = v.Checked(btnmodificargiro.BackgroundImage).ToString();
            respaldo[23, 4] = privilegios[23, 4] = v.Checked(btneliminargiro.BackgroundImage).ToString();
            privilegios[23, 5] = "catGiros";
            
            respaldo[24, 0] = privilegios[24, 0] = v.getIntFrombool((v.ImageToString(btninsertarrequisicion.BackgroundImage) == v.check || v.ImageToString(btnconsultarrequisicion.BackgroundImage) == v.check || v.ImageToString(btnmodificarrequisicion.BackgroundImage) == v.check)).ToString();
            respaldo[24, 1] = privilegios[24, 1] = v.Checked(btninsertarrequisicion.BackgroundImage).ToString();
            respaldo[24, 2] = privilegios[24, 2] = v.Checked(btnconsultarrequisicion.BackgroundImage).ToString();
            respaldo[24, 3] = privilegios[24, 3] = v.Checked(btnmodificarrequisicion.BackgroundImage).ToString();
            respaldo[24, 4] = privilegios[24, 4] = "0";
            privilegios[24, 5] = "ordencompra";
            respaldo[25, 0] = privilegios[25, 0] = v.getIntFrombool((v.ImageToString(btninsertarcomparativa.BackgroundImage) == v.check || v.ImageToString(btnconsultarcomparativa.BackgroundImage) == v.check || v.ImageToString(btnmodificarcomparativa.BackgroundImage) == v.check)).ToString();
            respaldo[25, 1] = privilegios[25, 1] = v.Checked(btninsertarcomparativa.BackgroundImage).ToString();
            respaldo[25, 2] = privilegios[25, 2] = v.Checked(btnconsultarcomparativa.BackgroundImage).ToString();
            respaldo[25, 3] = privilegios[25, 3] = v.Checked(btnmodificarcomparativa.BackgroundImage).ToString();
            respaldo[25, 4] = privilegios[25, 4] = "0";
            privilegios[25, 5] = "comparativas";
            respaldo[26, 0] = privilegios[26, 0] = v.getIntFrombool((v.ImageToString(btninsertaralmacen.BackgroundImage) == v.check || v.ImageToString(btnconsultaralmacen.BackgroundImage) == v.check || v.ImageToString(btnmodificaralmacen.BackgroundImage) == v.check)).ToString();
            respaldo[26, 1] = privilegios[26, 1] = v.Checked(btninsertaralmacen.BackgroundImage).ToString();
            respaldo[26, 2] = privilegios[26, 2] = v.Checked(btnconsultaralmacen.BackgroundImage).ToString();
            respaldo[26, 3] = privilegios[26, 3] = v.Checked(btnmodificaralmacen.BackgroundImage).ToString();
            respaldo[26, 4] = privilegios[26, 4] = "0";
            privilegios[26, 5] = "Almacen";
            
            respaldo[27, 0] = privilegios[27, 0] = v.getIntFrombool((v.ImageToString(btnmodificariva.BackgroundImage) == v.check)).ToString();
            respaldo[27, 1] = privilegios[27, 1] = "0";
            respaldo[27, 2] = privilegios[27, 2] = "0";
            respaldo[27, 3] = privilegios[27, 3] = v.Checked(btnmodificariva.BackgroundImage).ToString();
            respaldo[27, 4] = privilegios[27, 4] = "0";
            privilegios[27, 5] = "changeiva";
            respaldo[28, 0] = privilegios[28, 0] = v.getIntFrombool((v.ImageToString(btnIncertarRR.BackgroundImage) == v.check)).ToString();
            respaldo[28, 1] = privilegios[28, 1] = v.getIntFrombool((v.ImageToString(btnIncertarRR.BackgroundImage) == v.check)).ToString();
            respaldo[28, 2] = privilegios[28, 2] = v.getIntFrombool((v.ImageToString(btnConsultaRR.BackgroundImage) == v.check)).ToString();
            respaldo[28, 3] = privilegios[28, 3] = v.getIntFrombool((v.ImageToString(btnEditarRR.BackgroundImage) == v.check)).ToString();
            respaldo[28, 4] = privilegios[28, 4] = v.getIntFrombool((v.ImageToString(btnEliminarRR.BackgroundImage) == v.check)).ToString();
            privilegios[28, 5] = "catRefacC";
            respaldo[29, 0] = privilegios[29, 0] = v.getIntFrombool((v.ImageToString(btnReportes.BackgroundImage) == v.check)).ToString();
            respaldo[29, 1] = privilegios[29, 1] = "0";
            respaldo[29, 2] = privilegios[29, 2] = v.Checked(btnReportes.BackgroundImage).ToString();
            respaldo[29, 3] = privilegios[29, 3] = "0";
            respaldo[29, 4] = privilegios[29, 4] = "0";
            privilegios[29, 5] = "Reportes";
            respaldo[30, 0] = privilegios[30, 0] = v.getIntFrombool((v.ImageToString(btnExternas.BackgroundImage) == v.check)).ToString();
            respaldo[30, 1] = privilegios[30, 1] = "0";
            respaldo[30, 2] = privilegios[30, 2] = v.Checked(btnExternas.BackgroundImage).ToString();
            respaldo[30, 3] = privilegios[30, 3] = "0";
            respaldo[30, 4] = privilegios[30, 4] = "0";
            privilegios[30, 5] = "UnExternas";
            respaldo[31, 0] = privilegios[31, 0] = v.getIntFrombool((v.ImageToString(btnInventario.BackgroundImage) == v.check)).ToString();
            respaldo[31, 1] = privilegios[31, 1] = "0";
            respaldo[31, 2] = privilegios[31, 2] = v.Checked(btnInventario.BackgroundImage).ToString();
            respaldo[31, 3] = privilegios[31, 3] = "0";
            respaldo[31, 4] = privilegios[31, 4] = "0";
            privilegios[31, 5] = "Inventario";





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
                            try
                            {
                                v.edit(id[i], ver, insertar, consultar, modificar, eliminar);
                            }
                            catch
                            {

                            }
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
                try { 
                object[] c = a.Rows[i].ItemArray;
                  for (int j = 0; j < c.Length; j++)
                    {
                        if (c[j].ToString() != b[i, j])
                        {
                            res = true;
                        }
                    }
                }
                catch
                {
                    nuevopermiso = i;
                    agregarNuevoPermisos(); 
                }
               
            }
            return res;
        }

        void agregarNuevoPermisos()
        {
           
                string ver = privilegios[nuevopermiso, 0];
                string insertar = privilegios[nuevopermiso, 1];
                string consultar = privilegios[nuevopermiso, 2];
                string modificar = privilegios[nuevopermiso, 3];
                string eliminar = privilegios[nuevopermiso, 4];
                string nombre = privilegios[nuevopermiso, 5];
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                v.insert(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
            }
                
                //v.c.insertLocal(ver, insertar, consultar, modificar, eliminar, nombre, idUsuario);
           
        }
        bool sehicieronModificaciones(string[,] a, string[,] b)
        {
            string[,] temp = new string[32, 5];
            for (int m = 0; m < 32; m++)
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
                        case "catfallosGrales":

                            btninsertarfallo.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarfallo.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarfallo.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarfallo.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "Mantenimiento":

                            btninsertarmante.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarmante.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarmante.BackgroundImage = v.Checked(privilegios[2]);

                            break;
                        case "catProveedores":

                            btninsertarproveedor.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarproveedor.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarproveedor.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarproveedor.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "catRefacciones":

                            btninsertarrefaccion.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarrefaccion.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarrefaccion.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminarrefaccion.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "ordencompra":

                            btninsertarrequisicion.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarrequisicion.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarrequisicion.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "comparativas":

                            btninsertarcomparativa.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultarcomparativa.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificarcomparativa.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "Almacen":

                            btninsertaralmacen.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultaralmacen.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificaralmacen.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "catGiros":
                            btninsertargiro.BackgroundImage = v.Checked(privilegios[0]);
                            btnconsultargiro.BackgroundImage = v.Checked(privilegios[1]);
                            btnmodificargiro.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btneliminargiro.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "changeiva":
                            btnmodificariva.BackgroundImage = v.Checked(privilegios[2]);
                            break;
                        case "catRefacC":
                            btnIncertarRR.BackgroundImage = v.Checked(privilegios[0]);
                            btnConsultaRR.BackgroundImage = v.Checked(privilegios[1]);
                            btnEditarRR.BackgroundImage = v.Checked(privilegios[2]);
                            if (Convert.ToInt32(privilegios.Length) > 3)
                            {
                                btnEliminarRR.BackgroundImage = v.Checked(privilegios[3]);
                            }
                            break;
                        case "Reportes":
                            btnReportes.BackgroundImage = v.Checked(privilegios[1]);
                            break;
                        case "UnExternas":
                           
                            btnExternas.BackgroundImage = v.Checked(privilegios[1]);
                            break;
                        case "Inventario":
                            btnInventario.BackgroundImage = v.Checked(privilegios[1]);
                            break;
                        case "REntradas":
                            btnConsultaEntrada.BackgroundImage = v.Checked(privilegios[1]);
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

        private void privilegiosSupervision_LocationChanged(object sender, EventArgs e)
        {

        }

        void habilitar_privilegios(int provienen)
        {
            switch (provienen)
            {
                case 1:
                    btninsertararea.Enabled = Enabled = btnmodificararea.Enabled = btneliminararea.Enabled = btninsertarservicio.Enabled = btnmodificarservicio.Enabled = btneliminarservicio.Enabled = btninsertarincidencia.Enabled = btnmodificarincidencia.Enabled = btneliminarincidencia.Enabled = btninsertarestacion.Enabled = btnmodificarestacion.Enabled = btneliminarestacion.Enabled = btninsertarunidad.Enabled = btnmodificarunidad.Enabled = btneliminarunidad.Enabled = btninsertarmodelo.Enabled = btnmodificarmodelo.Enabled = btneliminarmodelo.Enabled = btninsertarol.Enabled = btnmodificarol.Enabled = btndesactivarol.Enabled = btninsertarsuper.Enabled = btnmodificarsuper.Enabled = btninsertarpercances.Enabled = btnmodificarpercances.Enabled = btninsertarrp.Enabled = btnmodificarrp.Enabled = btninsertarip.Enabled = btnmodificarip.Enabled = true;
                    btninsertarfallo.Enabled = btnUnidades.Enabled = btninsertarmante.Enabled = btnmodificarfallo.Enabled = btnmodificarcargo.Enabled = btnmodificaru.Enabled = btnmodificarmante.Enabled = btneliminarfallo.Enabled = false;
                    btninsertarproveedor.Enabled = btninsertarrefaccion.Enabled = btnIncertarRR.Enabled = btninsertargiro.Enabled = btninsertarcomparativa.Enabled = btninsertaralmacen.Enabled = btnmodificarproveedor.Enabled = btnmodificarrefaccion.Enabled = btnEditarRR.Enabled = btnmodificargiro.Enabled = btnmodificarcomparativa.Enabled = btneliminarproveedor.Enabled = btneliminarrefaccion.Enabled = btnEliminarRR.Enabled = btneliminargiro.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = false;
                    break;
                case 2:

                    btninsertarfallo.Enabled = btnUnidades.Enabled = btninsertarmante.Enabled = btnmodificarfallo.Enabled = btnmodificarcargo.Enabled = btnmodificaru.Enabled = btnmodificarmante.Enabled = btneliminarfallo.Enabled = true;

                    btninsertararea.Enabled = btnmodificararea.Enabled = btneliminararea.Enabled = btninsertarservicio.Enabled = btnmodificarservicio.Enabled = btneliminarservicio.Enabled = btninsertarincidencia.Enabled = btnmodificarincidencia.Enabled = btneliminarincidencia.Enabled = btninsertarestacion.Enabled = btnmodificarestacion.Enabled = btneliminarestacion.Enabled = btninsertarunidad.Enabled = btnmodificarunidad.Enabled = btneliminarunidad.Enabled = btninsertarmodelo.Enabled = btnmodificarmodelo.Enabled = btneliminarmodelo.Enabled = btninsertarol.Enabled = btnmodificarol.Enabled = btndesactivarol.Enabled = btninsertarsuper.Enabled = btnmodificarsuper.Enabled = btninsertarpercances.Enabled = btnmodificarpercances.Enabled = btninsertarrp.Enabled = btnmodificarrp.Enabled = btninsertarip.Enabled = btnmodificarip.Enabled = false;


                    btninsertarproveedor.Enabled = btninsertarrefaccion.Enabled = btnIncertarRR.Enabled = btninsertargiro.Enabled = btninsertarcomparativa.Enabled = btninsertaralmacen.Enabled = btnmodificarproveedor.Enabled = btnmodificarrefaccion.Enabled = btnEditarRR.Enabled = btnmodificargiro.Enabled = btnmodificarcomparativa.Enabled = btneliminarproveedor.Enabled = btneliminarrefaccion.Enabled = btnEliminarRR.Enabled = btneliminargiro.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = false;
                    break;
                case 3:

                    btninsertarfallo.Enabled = btnUnidades.Enabled = btninsertarmante.Enabled = btnmodificarfallo.Enabled = btnmodificarcargo.Enabled = btnmodificaru.Enabled = btnmodificarmante.Enabled = btneliminarfallo.Enabled = false;

                    btnmodificarip.Enabled = btninsertararea.Enabled =  btnmodificararea.Enabled = btneliminararea.Enabled = btninsertarservicio.Enabled = btnmodificarservicio.Enabled = btneliminarservicio.Enabled = btninsertarincidencia.Enabled = btnmodificarincidencia.Enabled = btneliminarincidencia.Enabled = btninsertarestacion.Enabled = btnmodificarestacion.Enabled = btneliminarestacion.Enabled = btninsertarunidad.Enabled = btnmodificarunidad.Enabled = btneliminarunidad.Enabled = btninsertarmodelo.Enabled = btnmodificarmodelo.Enabled = btneliminarmodelo.Enabled = btninsertarol.Enabled = btnmodificarol.Enabled = btndesactivarol.Enabled = btninsertarsuper.Enabled = btnmodificarsuper.Enabled = btninsertarpercances.Enabled = btnmodificarpercances.Enabled = btninsertarrp.Enabled = btnmodificarrp.Enabled = btninsertarip.Enabled = false;
                    

                    btninsertarproveedor.Enabled = btninsertarrefaccion.Enabled = btnIncertarRR.Enabled = btninsertargiro.Enabled = btninsertarcomparativa.Enabled = btninsertaralmacen.Enabled = btnmodificarproveedor.Enabled = btnmodificarrefaccion.Enabled = btnEditarRR.Enabled = btnmodificargiro.Enabled = btnmodificarcomparativa.Enabled = btneliminarproveedor.Enabled = btneliminarrefaccion.Enabled = btnEliminarRR.Enabled = btneliminargiro.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = btnmodificaralmacen.Enabled = btninsertarrequisicion.Enabled = btnconsultarrequisicion.Enabled = btnmodificarrequisicion.Enabled = btnConsultaEntrada.Enabled = true;
                    break;
            }
        }
        public void puestoCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (puestoCB.Text == "ADMINISTRADOR")
            {
                administrador();
            }

            if (puestoCB.Text == "AUXILIAR DE ALMACEN")
            {
                auxiliar_almacen();
            }

            if (puestoCB.Text == "COMPRAS")
            {
                //catalogo personal                                                         
                btninsertarempleado.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempleado.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempleado.BackgroundImage = Properties.Resources.uncheck;

                //catalogo cargos                                                         
                btninsertarcargo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarcargo.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarcargo.BackgroundImage = Properties.Resources.uncheck;

                //catalogo proveedores
                btninsertarproveedor.BackgroundImage = Properties.Resources.check;
                btnconsultarproveedor.BackgroundImage = Properties.Resources.check;
                btnmodificarproveedor.BackgroundImage = Properties.Resources.check;
                btneliminarproveedor.BackgroundImage = Properties.Resources.uncheck;

                //catalogo refacciones
                btninsertarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarrefaccion.BackgroundImage = Properties.Resources.check;
                btnmodificarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btneliminarrefaccion.BackgroundImage = Properties.Resources.uncheck;

                //catalogo refacciones recu
                btnIncertarRR.BackgroundImage = Properties.Resources.uncheck;
                btnConsultaRR.BackgroundImage = Properties.Resources.uncheck;
                btnEditarRR.BackgroundImage = Properties.Resources.uncheck;
                btnEliminarRR.BackgroundImage = Properties.Resources.uncheck;

                //catalogo empresas
                btninsertarempresa.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempresa.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarempresa.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempresa.BackgroundImage = Properties.Resources.uncheck;

                //catalogo giro empresas
                btninsertargiro.BackgroundImage = Properties.Resources.uncheck;
                btnconsultargiro.BackgroundImage = Properties.Resources.uncheck;
                btnmodificargiro.BackgroundImage = Properties.Resources.uncheck;
                btneliminargiro.BackgroundImage = Properties.Resources.uncheck;

                //catalogo tipo lice
                btninsertartipo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultartipo.BackgroundImage = Properties.Resources.uncheck;
                btnmodificartipo.BackgroundImage = Properties.Resources.uncheck;
                btneliminartipo.BackgroundImage = Properties.Resources.uncheck;

                //orden compra
                btninsertarrequisicion.BackgroundImage = Properties.Resources.check;
                btnconsultarrequisicion.BackgroundImage = Properties.Resources.check;
                btnmodificarrequisicion.BackgroundImage = Properties.Resources.check;

                //requerimiento
                btninsertarcomparativa.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarcomparativa.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarcomparativa.BackgroundImage = Properties.Resources.uncheck;

                //reporte almacen
                btninsertaralmacen.BackgroundImage = Properties.Resources.uncheck;
                btnconsultaralmacen.BackgroundImage = Properties.Resources.uncheck;
                btnmodificaralmacen.BackgroundImage = Properties.Resources.uncheck;

                //modificaciones

                btnconsultarhistorial.BackgroundImage = Properties.Resources.uncheck;

                //iva
                btnmodificariva.BackgroundImage = Properties.Resources.uncheck;
            }

            if (puestoCB.Text == "ENCARGADO DE ALMACEN")
            {
                //catalogo personal                                                         
                btninsertarempleado.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempleado.BackgroundImage = Properties.Resources.check;
                btnmodificarempleado.BackgroundImage = Properties.Resources.check;
                btneliminarempleado.BackgroundImage = Properties.Resources.check;

                //catalogo cargos                                                         
                btninsertarcargo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarcargo.BackgroundImage = Properties.Resources.check;
                btnmodificarcargo.BackgroundImage = Properties.Resources.check;
                btneliminarcargo.BackgroundImage = Properties.Resources.check;

                //catalogo proveedores
                btninsertarproveedor.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarproveedor.BackgroundImage = Properties.Resources.check;
                btnmodificarproveedor.BackgroundImage = Properties.Resources.check;
                btneliminarproveedor.BackgroundImage = Properties.Resources.check;

                //catalogo refacciones
                btninsertarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarrefaccion.BackgroundImage = Properties.Resources.check;
                btnmodificarrefaccion.BackgroundImage = Properties.Resources.check;
                btneliminarrefaccion.BackgroundImage = Properties.Resources.check;

                //catalogo refacciones recu
                btnIncertarRR.BackgroundImage = Properties.Resources.uncheck;
                btnConsultaRR.BackgroundImage = Properties.Resources.check;
                btnEditarRR.BackgroundImage = Properties.Resources.check;
                btnEliminarRR.BackgroundImage = Properties.Resources.check;

                //catalogo empresas
                btninsertarempresa.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempresa.BackgroundImage = Properties.Resources.check;
                btnmodificarempresa.BackgroundImage = Properties.Resources.check;
                btneliminarempresa.BackgroundImage = Properties.Resources.check;

                //catalogo giro empresas
                btninsertargiro.BackgroundImage = Properties.Resources.uncheck;
                btnconsultargiro.BackgroundImage = Properties.Resources.check;
                btnmodificargiro.BackgroundImage = Properties.Resources.check;
                btneliminargiro.BackgroundImage = Properties.Resources.check;

                //catalogo tipo lice
                btninsertartipo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultartipo.BackgroundImage = Properties.Resources.check;
                btnmodificartipo.BackgroundImage = Properties.Resources.check;
                btneliminartipo.BackgroundImage = Properties.Resources.check;

                //orden compra
                btninsertarrequisicion.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarrequisicion.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarrequisicion.BackgroundImage = Properties.Resources.uncheck;

                //requerimiento
                btninsertarcomparativa.BackgroundImage = Properties.Resources.check;
                btnconsultarcomparativa.BackgroundImage = Properties.Resources.check;
                btnmodificarcomparativa.BackgroundImage = Properties.Resources.check;

                //reporte almacen
                btninsertaralmacen.BackgroundImage = Properties.Resources.check;
                btnconsultaralmacen.BackgroundImage = Properties.Resources.check;
                btnmodificaralmacen.BackgroundImage = Properties.Resources.check;

                //modificaciones

                btnconsultarhistorial.BackgroundImage = Properties.Resources.check;

                //iva
                btnmodificariva.BackgroundImage = Properties.Resources.check;
            }

            if (puestoCB.Text == "FINANZAS")
            {
                //catalogo personal                                                         
                btninsertarempleado.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempleado.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempleado.BackgroundImage = Properties.Resources.uncheck;

                //catalogo cargos                                                         
                btninsertarcargo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarcargo.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarcargo.BackgroundImage = Properties.Resources.uncheck;

                //catalogo proveedores
                btninsertarproveedor.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarproveedor.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarproveedor.BackgroundImage = Properties.Resources.uncheck;
                btneliminarproveedor.BackgroundImage = Properties.Resources.uncheck;

                //catalogo refacciones
                btninsertarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btneliminarrefaccion.BackgroundImage = Properties.Resources.uncheck;

                //catalogo refacciones recu

                btnIncertarRR.BackgroundImage = Properties.Resources.uncheck;
                btnConsultaRR.BackgroundImage = Properties.Resources.uncheck;
                btnEditarRR.BackgroundImage = Properties.Resources.uncheck;
                btnEliminarRR.BackgroundImage = Properties.Resources.uncheck;

                //catalogo empresas
                btninsertarempresa.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempresa.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarempresa.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempresa.BackgroundImage = Properties.Resources.uncheck;

                //catalogo giro empresas
                btninsertargiro.BackgroundImage = Properties.Resources.uncheck;
                btnconsultargiro.BackgroundImage = Properties.Resources.uncheck;
                btnmodificargiro.BackgroundImage = Properties.Resources.uncheck;
                btneliminargiro.BackgroundImage = Properties.Resources.uncheck;

                //catalogo tipo lice
                btninsertartipo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultartipo.BackgroundImage = Properties.Resources.uncheck;
                btnmodificartipo.BackgroundImage = Properties.Resources.uncheck;
                btneliminartipo.BackgroundImage = Properties.Resources.uncheck;

                //orden compra
                btninsertarrequisicion.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarrequisicion.BackgroundImage = Properties.Resources.check;
                btnmodificarrequisicion.BackgroundImage = Properties.Resources.uncheck;

                //requerimiento
                btninsertarcomparativa.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarcomparativa.BackgroundImage = Properties.Resources.uncheck;
                btnmodificarcomparativa.BackgroundImage = Properties.Resources.uncheck;

                //reporte almacen
                btninsertaralmacen.BackgroundImage = Properties.Resources.uncheck;
                btnconsultaralmacen.BackgroundImage = Properties.Resources.uncheck;
                btnmodificaralmacen.BackgroundImage = Properties.Resources.uncheck;

                //modificaciones

                btnconsultarhistorial.BackgroundImage = Properties.Resources.uncheck;

                //iva
                btnmodificariva.BackgroundImage = Properties.Resources.uncheck;
            }
            if (puestoCB.Text == "SOLO CONSULTA")
            {
                //catalogo personal                                                         
                btninsertarempleado.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempleado.BackgroundImage = Properties.Resources.check;
                btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempleado.BackgroundImage = Properties.Resources.uncheck;

                //catalogo cargos                                                         
                btninsertarcargo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarcargo.BackgroundImage = Properties.Resources.check;
                btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
                btneliminarcargo.BackgroundImage = Properties.Resources.uncheck;

                //catalogo proveedores
                btninsertarproveedor.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarproveedor.BackgroundImage = Properties.Resources.check;
                btnmodificarproveedor.BackgroundImage = Properties.Resources.uncheck;
                btneliminarproveedor.BackgroundImage = Properties.Resources.uncheck;

                //catalogo refacciones
                btninsertarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarrefaccion.BackgroundImage = Properties.Resources.check;
                btnmodificarrefaccion.BackgroundImage = Properties.Resources.uncheck;
                btneliminarrefaccion.BackgroundImage = Properties.Resources.uncheck;

                //catalogo refacciones recu
                btnIncertarRR.BackgroundImage = Properties.Resources.uncheck;
                btnConsultaRR.BackgroundImage = Properties.Resources.check;
                btnEditarRR.BackgroundImage = Properties.Resources.uncheck;
                btnEliminarRR.BackgroundImage = Properties.Resources.uncheck;

                //catalogo empresas
                btninsertarempresa.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarempresa.BackgroundImage = Properties.Resources.check;
                btnmodificarempresa.BackgroundImage = Properties.Resources.uncheck;
                btneliminarempresa.BackgroundImage = Properties.Resources.uncheck;

                //catalogo giro empresas
                btninsertargiro.BackgroundImage = Properties.Resources.uncheck;
                btnconsultargiro.BackgroundImage = Properties.Resources.check;
                btnmodificargiro.BackgroundImage = Properties.Resources.uncheck;
                btneliminargiro.BackgroundImage = Properties.Resources.uncheck;

                //catalogo tipo lice
                btninsertartipo.BackgroundImage = Properties.Resources.uncheck;
                btnconsultartipo.BackgroundImage = Properties.Resources.check;
                btnmodificartipo.BackgroundImage = Properties.Resources.uncheck;
                btneliminartipo.BackgroundImage = Properties.Resources.uncheck;

                //orden compra
                btninsertarrequisicion.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarrequisicion.BackgroundImage = Properties.Resources.check;
                btnmodificarrequisicion.BackgroundImage = Properties.Resources.uncheck;

                //requerimiento
                btninsertarcomparativa.BackgroundImage = Properties.Resources.uncheck;
                btnconsultarcomparativa.BackgroundImage = Properties.Resources.check;
                btnmodificarcomparativa.BackgroundImage = Properties.Resources.uncheck;

                //reporte almacen
                btninsertaralmacen.BackgroundImage = Properties.Resources.uncheck;
                btnconsultaralmacen.BackgroundImage = Properties.Resources.check;
                btnmodificaralmacen.BackgroundImage = Properties.Resources.uncheck;

                //modificaciones

                btnconsultarhistorial.BackgroundImage = Properties.Resources.check;

                //iva
                btnmodificariva.BackgroundImage = Properties.Resources.uncheck;
            }


        }

        private void puestoCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        void administrador()
        {
            btninsertararea.BackgroundImage = Properties.Resources.check;
            btnconsultararea.BackgroundImage = Properties.Resources.check;
            btnmodificararea.BackgroundImage = Properties.Resources.check; ;
            btneliminararea.BackgroundImage = Properties.Resources.check;
            btninsertarempresa.BackgroundImage = Properties.Resources.check;
            btnconsultarempresa.BackgroundImage = Properties.Resources.check;
            btnmodificarempresa.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarempresa.BackgroundImage = Properties.Resources.check;
            }
            btninsertarempleado.BackgroundImage = Properties.Resources.check;
            btnconsultarempleado.BackgroundImage = Properties.Resources.check;
            btnmodificarempleado.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarempleado.BackgroundImage = Properties.Resources.check;
            }
            btninsertarcargo.BackgroundImage = Properties.Resources.check;
            btnconsultarcargo.BackgroundImage = Properties.Resources.check;
            btnmodificarcargo.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarcargo.BackgroundImage = Properties.Resources.check;
            }
            btninsertarservicio.BackgroundImage = Properties.Resources.check;
            btnconsultarservicio.BackgroundImage = Properties.Resources.check;
            btnmodificarservicio.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarservicio.BackgroundImage = Properties.Resources.check;
            }
            btninsertartipo.BackgroundImage = Properties.Resources.check;
            btnconsultartipo.BackgroundImage = Properties.Resources.check;
            btnmodificartipo.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminartipo.BackgroundImage = Properties.Resources.check;
            }
            btninsertarincidencia.BackgroundImage = Properties.Resources.check;
            btnconsultarincidencia.BackgroundImage = Properties.Resources.check;
            btnmodificarincidencia.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarincidencia.BackgroundImage = Properties.Resources.check;
            }
            btninsertarestacion.BackgroundImage = Properties.Resources.check; ;
            btnconsultarestacion.BackgroundImage = Properties.Resources.check;
            btnmodificarestacion.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarestacion.BackgroundImage = Properties.Resources.check;
            }
            btninsertarunidad.BackgroundImage = Properties.Resources.check;
            btnconsultarunidad.BackgroundImage = Properties.Resources.check;
            btnmodificarunidad.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarunidad.BackgroundImage = Properties.Resources.check;
            }
            btninsertarmodelo.BackgroundImage = Properties.Resources.check;
            btnconsultarmodelo.BackgroundImage = Properties.Resources.check;
            btnmodificarmodelo.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarmodelo.BackgroundImage = Properties.Resources.check;
            }
            btninsertarol.BackgroundImage = Properties.Resources.check;
            btnconsultarol.BackgroundImage = Properties.Resources.check;
            btnmodificarol.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btndesactivarol.BackgroundImage = Properties.Resources.check;
            }
            btninsertarsuper.BackgroundImage = Properties.Resources.check;
            btnconsultarsuper.BackgroundImage = Properties.Resources.check;
            btnmodificarsuper.BackgroundImage = Properties.Resources.check;
            btninsertarpercances.BackgroundImage = Properties.Resources.check;
            btnconsultarpercances.BackgroundImage = Properties.Resources.check;
            btnmodificarpercances.BackgroundImage = Properties.Resources.check;
            btninsertarrp.BackgroundImage = Properties.Resources.check;
            btnconsultarrp.BackgroundImage = Properties.Resources.check;
            btnmodificarrp.BackgroundImage = Properties.Resources.check;
            btninsertarip.BackgroundImage = Properties.Resources.check;
            btnconsultarip.BackgroundImage = Properties.Resources.check;
            btnmodificarip.BackgroundImage = Properties.Resources.check;
            btnmodificarencabezados.BackgroundImage = Properties.Resources.check;
            btnconsultarhistorial.BackgroundImage = Properties.Resources.check;
             btninsertarfallo.BackgroundImage = Properties.Resources.check;
            btnconsultarfallo.BackgroundImage = Properties.Resources.check;
            btnmodificarfallo.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarfallo.BackgroundImage = Properties.Resources.check;
            }
            btninsertarmante.BackgroundImage = Properties.Resources.check;
            btnconsultarmante.BackgroundImage = Properties.Resources.check;
            btnmodificarmante.BackgroundImage = Properties.Resources.check;
            btninsertarproveedor.BackgroundImage = Properties.Resources.check;
            btnconsultarproveedor.BackgroundImage = Properties.Resources.check;
            btnmodificarproveedor.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarproveedor.BackgroundImage = Properties.Resources.check;
            }
            btninsertarrefaccion.BackgroundImage = Properties.Resources.check;
            btnconsultarrefaccion.BackgroundImage = Properties.Resources.check;
            btnmodificarrefaccion.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarrefaccion.BackgroundImage = Properties.Resources.check;
            }
            btninsertarrequisicion.BackgroundImage = Properties.Resources.check;
            btnconsultarrequisicion.BackgroundImage = Properties.Resources.check;
            btnmodificarrequisicion.BackgroundImage = Properties.Resources.check;
            btninsertarcomparativa.BackgroundImage = Properties.Resources.check;
            btnconsultarcomparativa.BackgroundImage = Properties.Resources.check;
            btnmodificarcomparativa.BackgroundImage = Properties.Resources.check;
            btninsertaralmacen.BackgroundImage = Properties.Resources.check;
            btnconsultaralmacen.BackgroundImage = Properties.Resources.check;
            btnmodificaralmacen.BackgroundImage = Properties.Resources.check;
            btninsertargiro.BackgroundImage = Properties.Resources.check;
            btnconsultargiro.BackgroundImage = Properties.Resources.check;
            btnmodificargiro.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminargiro.BackgroundImage = Properties.Resources.check;
            }
            btnmodificariva.BackgroundImage = Properties.Resources.check;
            btnIncertarRR.BackgroundImage = Properties.Resources.check;
            btnConsultaRR.BackgroundImage = Properties.Resources.check;
            btnEditarRR.BackgroundImage = Properties.Resources.check;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btnEliminarRR.BackgroundImage = Properties.Resources.check;
            }
            btnUnidades.BackgroundImage = Properties.Resources.check;
            btnConsultarUnidades.BackgroundImage = Properties.Resources.check;
            btnmodificaru.BackgroundImage = Properties.Resources.check;
            btnReportes.BackgroundImage = Properties.Resources.check;
            btnInventario.BackgroundImage = Properties.Resources.check;
            btnExternas.BackgroundImage = Properties.Resources.check;
            btnConsultaEntrada.BackgroundImage = Properties.Resources.check;
        }
        void auxiliar_almacen()
        {
            //catalogo personal                                                         
            btninsertarempleado.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarempleado.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarempleado.BackgroundImage = Properties.Resources.uncheck;
            btneliminarempleado.BackgroundImage = Properties.Resources.check;

            //catalogo cargos                                                         
            btninsertarcargo.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarcargo.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarcargo.BackgroundImage = Properties.Resources.uncheck;
            btneliminarcargo.BackgroundImage = Properties.Resources.uncheck;

            //catalogo proveedores
            btninsertarproveedor.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarproveedor.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarproveedor.BackgroundImage = Properties.Resources.uncheck;
            btneliminarproveedor.BackgroundImage = Properties.Resources.uncheck;

            //catalogo refacciones
            btninsertarrefaccion.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarrefaccion.BackgroundImage = Properties.Resources.check;
            btnmodificarrefaccion.BackgroundImage = Properties.Resources.uncheck;
            btneliminarrefaccion.BackgroundImage = Properties.Resources.uncheck;

            //catalogo refacciones recu
            btnIncertarRR.BackgroundImage = Properties.Resources.uncheck;
            btnConsultaRR.BackgroundImage = Properties.Resources.uncheck;
            btnEditarRR.BackgroundImage = Properties.Resources.uncheck;
            btnEliminarRR.BackgroundImage = Properties.Resources.uncheck;

            //catalogo empresas
            btninsertarempresa.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarempresa.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarempresa.BackgroundImage = Properties.Resources.uncheck;
            btneliminarempresa.BackgroundImage = Properties.Resources.uncheck;

            //catalogo giro empresas
            btninsertargiro.BackgroundImage = Properties.Resources.uncheck;
            btnconsultargiro.BackgroundImage = Properties.Resources.uncheck;
            btnmodificargiro.BackgroundImage = Properties.Resources.uncheck;
            btneliminargiro.BackgroundImage = Properties.Resources.uncheck;

            //catalogo tipo lice
            btninsertartipo.BackgroundImage = Properties.Resources.uncheck;
            btnconsultartipo.BackgroundImage = Properties.Resources.uncheck;
            btnmodificartipo.BackgroundImage = Properties.Resources.uncheck;
            btneliminartipo.BackgroundImage = Properties.Resources.uncheck;

            //orden compra
            btninsertarrequisicion.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarrequisicion.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarrequisicion.BackgroundImage = Properties.Resources.uncheck;

            //requerimiento
            btninsertarcomparativa.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarcomparativa.BackgroundImage = Properties.Resources.check;
            btnmodificarcomparativa.BackgroundImage = Properties.Resources.check;

            //reporte almacen
            btninsertaralmacen.BackgroundImage = Properties.Resources.check;
            btnconsultaralmacen.BackgroundImage = Properties.Resources.check;
            btnmodificaralmacen.BackgroundImage = Properties.Resources.check;

            //modificaciones

            btnconsultarhistorial.BackgroundImage = Properties.Resources.uncheck;

            //iva
            btnmodificariva.BackgroundImage = Properties.Resources.uncheck;


            btninsertarestacion.BackgroundImage = Properties.Resources.uncheck; ;
            btnconsultarestacion.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarestacion.BackgroundImage = Properties.Resources.uncheck;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarestacion.BackgroundImage = Properties.Resources.uncheck;
            }
            btninsertarunidad.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarunidad.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarunidad.BackgroundImage = Properties.Resources.uncheck;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarunidad.BackgroundImage = Properties.Resources.uncheck;
            }
            btninsertarmodelo.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarmodelo.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarmodelo.BackgroundImage = Properties.Resources.uncheck;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarmodelo.BackgroundImage = Properties.Resources.uncheck;
            }
            btninsertarol.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarol.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarol.BackgroundImage = Properties.Resources.uncheck;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btndesactivarol.BackgroundImage = Properties.Resources.uncheck;
            }
            btninsertarsuper.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarsuper.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarsuper.BackgroundImage = Properties.Resources.uncheck;
            btninsertarpercances.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarpercances.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarpercances.BackgroundImage = Properties.Resources.uncheck;
            btninsertarrp.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarrp.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarrp.BackgroundImage = Properties.Resources.uncheck;
            btninsertarip.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarip.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarip.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarencabezados.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarhistorial.BackgroundImage = Properties.Resources.uncheck;
            btninsertarfallo.BackgroundImage = Properties.Resources.uncheck;
            btnconsultarfallo.BackgroundImage = Properties.Resources.uncheck;
            btnmodificarfallo.BackgroundImage = Properties.Resources.uncheck;
            if (Convert.ToInt32(privilegios.Length) > 3)
            {
                btneliminarfallo.BackgroundImage = Properties.Resources.uncheck;
            }
        }
    }
}
