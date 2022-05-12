using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace controlFallos
{
    public partial class menuPrincipal : Form
    {

        string idFolio = "";
        string consultaReportes = "", cadenaEmpresa = "";
        int tipoArea;
        public int idUsuario;
        public String nombre = "";
        public Form form;
        int tipo;
        Point defaultLocation = new Point(1560, 13);
        public Image newimg;
        public validaciones v;
        public int empresa { get; protected internal set; }
        public int area { get; protected internal set; }
        bool res = true;
        public int resAnterior = 0;
        Thread BuscarValidaciones;
        Thread session;
        Thread updates;
        delegate void obtenerNotificacionesD();
        delegate void sesioncaducada();
        Thread hilo;
        Thread hiloMuestraNotificacion;
        int totalAnteriorPedidos, Heightp = 0, Widthp = 0;
        new login Owner;
        public menuPrincipal(int idUsuario, int empresa, int area, Form fh, validaciones v)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.v = v;
            cadenaEmpresa = (empresa == 2 ? " (empresaMantenimiento = '2' or empresaMantenimiento = '1') " : (empresa == 3 ? " (empresaMantenimiento = '3' or empresaMantenimiento = '1') " : null));
            Owner = (login)fh;
            if (idUsuario == 0 || empresa == 0 || area == 0)
            {
                Owner.Show();
                this.Close();
            }
            paraTipo();
            MyRenderer temp = new MyRenderer();
            temp.RoundedEdges = true;
            menuStrip1.Renderer = temp;
        }

        void paraTipo()
        {
            if (empresa == 1)
            {
                tipo = 1;
            }
            else if (empresa == 2)
            {
                if (area == 1)
                    tipo = 2;
                else
                    tipo = 3;
            }
        }
        public void sendUser(string Message, validaciones.MessageBoxTitle type) => MessageBox.Show(Message, type.ToString(), MessageBoxButtons.OK, (type == validaciones.MessageBoxTitle.Advertencia ? MessageBoxIcon.Exclamation : (type == validaciones.MessageBoxTitle.Error ? MessageBoxIcon.Error : MessageBoxIcon.Information)));
        public void irArefacciones(string idRefaccion)
        {
            if (Convert.ToInt32(v.getaData("SELECT ver FROM privilegios WHERE namform='catRefacciones' AND usuariofkcpersonal='" + idUsuario + "'")) == 1)
                iraRefacciones(idRefaccion);
        }
        public void TraerVariable(string f)
        {
            idFolio = f;
            lblnumnotificaciones.BackgroundImage = null;
            lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
            if (this.tipoArea == 1)
            {
                if (reporteSupervicionToolStripMenuItem.Visible)
                    abrirReporte();
            }
            else if (this.tipoArea == 2)
            {
                if (reporteMantenimientoToolStripMenuItem.Visible)
                    abrirMantenimiento();
            }
            else if (tipoArea == 3)
            {
                if (reporteAlmacenToolStripMenuItem1.Visible)
                    abrirAlmacen();
            }
        }
        public void paraPErsonal(string id)
        {
            if (form != null)
            {
                if (form.GetType() != typeof(catPersonal))
                    paraPersonal();
            }
            else
                paraPersonal();
            catPersonal cat = (catPersonal)form;
            cat.BuscarRefaccion(id);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblnotif.Text) > 0)
            {
                Opacity = 0.9;
                DialogResult res;
                //if ((empresa == 1 && area == 1) || (empresa == 2 && area == 1))
                //{
                //    NotificacionSupervision n = new NotificacionSupervision(empresa, area, v);
                //    n.Owner = this;
                //    res = n.ShowDialog();
                //}
                //else
                //{
                //    NotificacionAlmacen n = new NotificacionAlmacen(v);
                //    n.Owner = this;
                //    res = n.ShowDialog();
                //}
                if ((empresa == 1))
                {
                    NotificacionSupervision n = new NotificacionSupervision(empresa, area, v);
                    n.Owner = this;
                    res = n.ShowDialog();
                }
                else if ((empresa == 2 && area == 2) || (empresa == 3 && area == 2))
                {
                    NotificacionAlmacen n = new NotificacionAlmacen(v, empresa);
                    n.Owner = this;
                    res = n.ShowDialog();
                }
                else
                {
                    NotificacionTri n = new NotificacionTri(2, v, empresa, area);
                    n.Owner = this;
                    res = n.ShowDialog();
                }

                if (res == DialogResult.Cancel)
                    Opacity = 1;
            }
        }
        public void AddFormInPanel(Form fh)
        {
            form = fh;
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            lblnumnotificaciones.Controls.Add(fh);
            lblnumnotificaciones.Tag = fh;
            fh.Show();
        }
        public bool cerrar()
        {
            if (this.lblnumnotificaciones.Controls.Count != 0)
            {
                if (MessageBox.Show("¿Está Seguro Que Desea Salir del Formulario?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.lblnumnotificaciones.Controls.Clear();
                    form.Close();
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }
        private class MyRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (!e.Item.Selected) base.OnRenderMenuItemBackground(e);
                else
                {
                    Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                    e.Graphics.FillRectangle(Brushes.Crimson, rc);
                    e.Graphics.DrawRectangle(Pens.White, 1, 0, rc.Width - 2, rc.Height - 1);
                }
            }
        }
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Owner != null)
                {
                    frm.Close();
                    frm.DialogResult = DialogResult.Cancel;
                }
            }
            this.WindowState = FormWindowState.Normal;
            pictureBox3_Click(null, e);
        }
        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
        private void catálogoDeFallosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Catálogo de Fallos";
                this.Text = "Catálogo de Fallos";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form = Application.OpenForms.OfType<catfallosGrales>().FirstOrDefault();
                catfallosGrales hijo = form ?? new catfallosGrales(idUsuario, empresa, area, this, newimg, v);
                AddFormInPanel(hijo);
            }
        }
        private void catálogoDePersonalToolStripMenuItem_Click(object sender, EventArgs e) { paraPersonal(); }
        void paraPersonal()
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Registro de Personal";
                this.Text = "Sistema de Reporte de Fallos - Registro de Personal";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(catálogoDePersonalToolStripMenuItem);
                var form = Application.OpenForms.OfType<catPersonal>().FirstOrDefault();
                catPersonal hijo = form ?? new catPersonal(this.idUsuario, this.empresa, this.area, newimg, this, v);
                AddFormInPanel(hijo);
            }
        }
        private void catálogoDeUnidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Catálogo de Unidades";
                this.Text = "Catálogo de Unidades";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                if (this.empresa == 1)
                {
                    var form = Application.OpenForms.OfType<catUnidades>().FirstOrDefault();
                    catUnidades hijo = form ?? new catUnidades(this.idUsuario, empresa, area, v);
                    AddFormInPanel(hijo);
                }
                else if (empresa == 2 || empresa == 3)
                {
                    var form = Application.OpenForms.OfType<catUnidaesTRI>().FirstOrDefault();
                    catUnidaesTRI hijo = form ?? new catUnidaesTRI(this.idUsuario, newimg, empresa, area, v);
                    AddFormInPanel(hijo);
                }
            }
        }
        public void abrirReporte()
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "Supervisión")
            {
                if (cerrar())
                {
                    resAnterior = 0;
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Reportes Supervisión";
                    this.Text = lbltitle.Text;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //lbltitle.Location = new Point(1591, 13);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    Deshabilitar(reporteDeSupervisiónToolStripMenuItem);
                    var form1 = Application.OpenForms.OfType<Supervisión>().FirstOrDefault();
                    Supervisión hijo = form1 ?? new Supervisión(this.idUsuario, empresa, area, v);
                    AddFormInPanel(hijo);
                }
            }
            else
            {
                Deshabilitar(reporteDeSupervisiónToolStripMenuItem);
                Supervisión p = (Supervisión)form;
                p.cargarDAtos();
            }
        }
        private void reporteNivelTransisumosToolStripMenuItem_Click(object sender, EventArgs e) { }
        void abrirAlmacen()
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "TRI")
            {
                if (cerrar())
                {
                    resAnterior = 0;
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Reportes Almacén";
                    this.Text = lbltitle.Text;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //lbltitle.Location = new Point(1575, 13);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Deshabilitar(reporteAlmacenToolStripMenuItem1);
                    var form2 = Application.OpenForms.OfType<TRI>().FirstOrDefault();
                    TRI hijo = form2 ?? new TRI(this.idUsuario, empresa, area, newimg, v);
                    AddFormInPanel(hijo);
                }
            }
            else
            {
                TRI t = (TRI)form;
                t.CargarDatos();
            }
        }
        private void reporteNivelMantenimientoToolStripMenuItem_Click(object sender, EventArgs e) { abrirMantenimiento(); }
        void abrirMantenimiento()
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "FormFallasMantenimiento")
            {
                if (cerrar())
                {
                    resAnterior = 0;
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Reportes Mantenimiento";
                    this.Text = lbltitle.Text;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //lbltitle.Location = new Point(1575, 13);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    Deshabilitar(reporteMantenimientoToolStripMenuItem);
                    var form3 = Application.OpenForms.OfType<Mantenimiento>().FirstOrDefault();
                    Mantenimiento hijo = form3 ?? new Mantenimiento(idUsuario, empresa, area, newimg, v);
                    AddFormInPanel(hijo);
                }
            }
            else
            {
                Mantenimiento m = (Mantenimiento)form;
                // m.metodoCarga();
            }
        }
        private void button2_Click(object sender, EventArgs e) { this.Close(); }
        private void menuPrincipal_Load(object sender, EventArgs e)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///descomentado es pantalla pequeña
           Screen screen = Screen.PrimaryScreen;
            Widthp = screen.Bounds.Width;
            Heightp = screen.Bounds.Height;
            this.Size = new Size(Convert.ToInt32(Widthp), Convert.ToInt32(Heightp) - 250);
            Screen screen2 = Screen.PrimaryScreen;
            int Widthp2 = screen2.Bounds.Width;
            int Heightp2 = screen2.Bounds.Height;
            lblnumnotificaciones.Size = new Size(Convert.ToInt32(Widthp2) - 50, Convert.ToInt32(Heightp2) - 180);
            menuStrip1.Size = new Size(Widthp, 55);
            panel3.Size = new Size(Widthp, 50);
            lbltitle.Location = new Point(Convert.ToInt32(Widthp) - 500, 11);
            panel1.Location = new Point(Convert.ToInt32(Widthp) - 120, 6);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            int xk = this.area;
            lblnumnotificaciones.BackgroundImage = newimg = (empresa == 1 ? Properties.Resources.transmasivo : (empresa == 2 ? Properties.Resources.Imagen2 : (empresa == 3 && this.area == 1 ? Properties.Resources.TSD : (empresa == 3 && this.area == 2 ? controlFallos.Properties.Resources.transinsumosk : null))));
            v.c.referencia(idUsuario);
            dieselToolStripMenuItem.Visible = (empresa == 3 && this.area == 2 ? true : false);
            reportesToolStripMenuItem.Visible = (empresa == 3 && this.area == 2 ? true : (empresa == 2 && this.area == 2 ? true : false));
            var consultaPrivilegios = v.getaData("SELECT GROUP_CONCAT(namForm SEPARATOR ';') FROM privilegios WHERE usuariofkcpersonal= '" + this.idUsuario + "' and ver > 0").ToString().Split(';');
            foreach (string namForm in consultaPrivilegios)
                PrivilegiosVisibles(namForm);
            obtenerconsulta();
            cambiarstatus(1);
            ///Notificaciones comentado  para mejorar velocidad 
            ThreadStart delegado = new ThreadStart(obtenerNotificaciones);
            hilo = new Thread(delegado);
            hilo.Start();
            timer1.Start();
            if (area == 1)
            //if ((empresa == 2 || empresa == 3) && area == 1)
            {
                BuscarValidaciones = new Thread(new ThreadStart(buscaValidar));
                BuscarValidaciones.Start();
            }
            else
                notifyIcon1.Dispose();
            //ThreadStart delegatse = new ThreadStart(sesion);
            //session = new Thread(delegatse);
            //session.Start();


            //updates = new Thread(new ThreadStart(inserttoglobal));
            //updates.Start();
        }

        //void inserttoglobal()
        //{
        //    while (res)
        //    {
        //        if (!v.c.wait)
        //        {
        //            string[] querys = v.c.readtofile().Split('|');
        //            if (querys.Length > 0)
        //            {
        //                for (int i = 0; i < querys.Length; i++)
        //                {
        //                    if (!string.IsNullOrWhiteSpace(querys[i]))
        //                        v.c.inserttoglobal(v.Desencriptar(querys[i]));
        //                }
        //                v.c.eliminar();
        //            }
        //        }
        //    }
        //}


        //void sesion()
        //{
        //    while (res)
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            Thread.Sleep(10000);
        //            MySqlConnection dbcon = new MySqlConnection("Server = " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + "; database = sistrefaccmant ;port=" + v.c.portLocal);
        //            dbcon.Open();
        //            string sql = "SELECT statusiniciosesion FROM datosistema WHERE usuariofkcpersonal='" + idUsuario + "'";
        //            MySqlCommand cmd = new MySqlCommand(sql, dbcon);
        //            int res1 = Convert.ToInt32(cmd.ExecuteScalar());
        //            if (res1 == 0)
        //            {
        //                sesioncaducada sesioncaducada = new sesioncaducada(sesion);
        //                this.Invoke(sesioncaducada);
        //            }
        //            dbcon.Close();
        //            dbcon.Dispose();
        //            dbcon = null;
        //            Thread.Sleep(5000);
        //        }
        //        else
        //        {
        //            MessageBox.Show("La Sesión Ha Caducado", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            cerrarForm();
        //            Application.Exit();
        //            return;
        //        }
        //    }
        //}
        void buscaValidar()
        {
            try
            {
                while (res)
                {
                    if (v.c.conexionOriginal())
                    {
                        MySqlConnection dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                        //else
                        //    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                        dbcon.Open();

                        string sql = "SELECT COUNT(t1.idestatusValidado) FROM estatusvalidado AS t1 INNER JOIN reportesupervicion as t4 ON t1.idreportefkreportesupervicion = t4.idReporteSupervicion INNER JOIN cunidades as t2 ON t4.UnidadfkCUnidades= t2.idunidad INNER JOIN cmodelos as t3 ON t2.modelofkcmodelos = t3.idmodelo WHERE t1.seen = 0 AND t3.empresaMantenimiento ='" + empresa + "'";
                        //string sql = "SELECT COUNT(t1.idestatusValidado) FROM estatusvalidado AS t1 INNER JOIN reportesupervicion as t4 ON t1.idreportefkreportesupervicion = t4.idReporteSupervicion INNER JOIN cunidades as t2 ON t4.UnidadfkCUnidades= t2.idunidad INNER JOIN careas as t3 on t2.areafkcareas = t3.idarea WHERE t1.seen = 0 AND " + cadenaEmpresa;
                        MySqlCommand cmd = new MySqlCommand(sql, dbcon);
                        int res2 = Convert.ToInt32(cmd.ExecuteScalar());
                        dbcon.Close();
                        dbcon.Dispose();
                        dbcon = null;
                        if (res2 != totalAnteriorPedidos)
                        {
                            mostrarNotificacion = new Thread(new ThreadStart(MostrarNotificacion));
                            mostrarNotificacion.Start();
                        }
                    }
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
            }

        }
        Thread mostrarNotificacion;
        void MostrarNotificacion()
        {
            //<<<<<<< HEAD
            /** MySqlConnection dbcon = null;
             if (v.c.conexionOriginal())
                 dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
             else
                 dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
             dbcon.Open();
             string cadena = "";
             string sql = "SELECT t2.folio FROM estatusValidado as t1 INNER JOIN reportesupervicion as t2 On t1.idreportefkreportesupervicion = t2.idReporteSupervicion WHERE t1.seen='0'";
             MySqlCommand cm = new MySqlCommand(sql, dbcon);
             MySqlDataReader dr = cm.ExecuteReader();
=======
            MySqlConnection dbcon = null;
            if (v.c.conexionOriginal())
                dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
            else
                dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
            dbcon.Open();
            string cadena = "";
            string sql = "SELECT t2.folio FROM estatusValidado as t1 INNER JOIN reportesupervicion as t2 On t1.idreportefkreportesupervicion = t2.idReporteSupervicion";
            MySqlCommand cm = new MySqlCommand(sql, dbcon);
            MySqlDataReader dr = cm.ExecuteReader();
>>>>>>> 289438355dcf9ce0a48126f327236d2313a9d884

             while (dr.Read())
             {
                 cadena = cadena + "\n" + dr.GetString("folio");
             }
             dr.Close();
             dbcon.Close();
             dbcon.Dispose();
             dbcon = null;
             notifyIcon1.BalloonTipText = "Se Han Validado Las Refacciones de Los Reportes: " + cadena;
             notifyIcon1.ShowBalloonTip(5000);**/

        }
        void obtenerconsulta()
        {
            if (empresa == 1 && area == 1)
            {
                consultaReportes = "SELECT (COUNT(t1.idReporte)+(SELECT COUNT(idvigencia) FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona WHERE t1.empresa='" + empresa + "' AND t1.area='" + area + "' AND (DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 or DATEDIFF(t1.fechaVencimientoTarjeton, curdate()) <= 7))) as cuenta FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad WHERE t1.seen = 0 and (t1.Estatus='2' or t1.Estatus='3') AND t3.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                tipoArea = 1;
            }
            else if (empresa == 2 || empresa == 3)
            {
                if (area == 1)
                {
                    consultaReportes = "SELECT (count(idReporteSupervicion)+(SELECT COUNT(*) FROM vigencias_supervision  WHERE empresa='" + empresa + "' and area='" + area + "' AND  DATEDIFF(fechaVencimientoConducir, curdate()) <= 7)) as cuenta FROM reportesupervicion WHERE seen = 0 and (SELECT (SELECT empresaMantenimiento FROM cmodelos WHERE idmodelo = modelofkcmodelos) from cunidades WHERE idunidad = UnidadfkCUnidades) = '" + empresa + "' AND FechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate() ";
                    tipoArea = 2;
                }
                else if (area == 2)
                {
                    consultaReportes = "select count(idrefaccion) as cuenta from crefacciones where (existencias <=media OR existencias<= abastecimiento) and empresa = '" + empresa + "' and status = 1";
                    tipoArea = 3;
                }
                else if (area == 4)
                {
                    consultaReportes = "SELECT count(idcrequicision) as cuenta FROM crequicision  WHERE Estatus = 0 AND empresa = '" + area + "' AND date_format(Fecha, '%Y-%m-%d')  BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate()";
                }
            }

        }

        private void obtenerNotificaciones()
        {
            int res = 0;
            MySqlConnection dbcon = null;
            if (v.c.conexionOriginal())
            {
                dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                dbcon.Open();
                MySqlCommand cm = new MySqlCommand(consultaReportes, dbcon);
                res = Convert.ToInt32(cm.ExecuteScalar());
                if (this.InvokeRequired)
                {
                    obtenerNotificacionesD delegado = new obtenerNotificacionesD(obtenerNotificaciones);
                    this.Invoke(delegado);
                }
                else
                {
                    pbnotif.BackgroundImage = null;
                    if (res > 0)
                    {
                        if (resAnterior != res)
                        {
                            resAnterior = res;
                            ThreadStart delegado = new ThreadStart(MostrarNotifiacacion);
                            hiloMuestraNotificacion = new Thread(delegado);
                            hiloMuestraNotificacion.Start();
                        }
                        pbnotif.BackgroundImage = Properties.Resources.notification__3_1;
                    }
                    else
                        pbnotif.BackgroundImage = Properties.Resources.notification__4_;
                    lblnotif.Text = "" + res;
                    //lo comente para hacer una prueba
                    //if (lblnotif.Text.Length == 1)
                    //    lblnotif.Location = new Point(95, 10);
                    //else if (lblnotif.Text.Length == 2)
                    //    lblnotif.Location = new Point(90, 10);
                }
                //if (hilo != null)
                //    hilo.Abort();
                dbcon.Close();
            }
            //else
            //    dbcon = new MySqlConnection("Server = 192.168.1.67; user=UPT; password = UPT2018; database = sistrefaccmant; port=3306");

        }
        private void MostrarNotifiacacion()
        {
            notif.BalloonTipText = "Tienes Nuevas Notificaciones (" + resAnterior + ")";
            notif.ShowBalloonTip(6000);
        }
        void PrivilegiosVisibles(string nombreForm)
        {
            nombreForm = nombreForm.ToLower();
            if (nombreForm == "catfallosgrales")
            {
                if (!catálogosToolStripMenuItem.Visible)
                    catálogosToolStripMenuItem.Visible = true;
                catálogoDeFallosToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "catpersonal" || nombreForm == "catpuestos" || nombreForm == "CatTipos")
            {
                if (!catálogosToolStripMenuItem.Visible)
                    catálogosToolStripMenuItem.Visible = true;
                catálogoDePersonalToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "catunidades" || nombreForm == "catservicios" || nombreForm == "catempresas" || nombreForm == "catestaciones" || nombreForm == "catmodelos")
            {
                if (area == 1)
                {
                    if (!catálogosToolStripMenuItem.Visible)
                        catálogosToolStripMenuItem.Visible = true;
                    catálogoDeUnidadesToolStripMenuItem.Visible = true;
                }
            }
            else if (nombreForm == "form1")
            {
                if (!reporteSupervicionToolStripMenuItem.Visible) reporteSupervicionToolStripMenuItem.Visible = true;
                reporteDeSupervisiónToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "mantenimiento")
                reporteMantenimientoToolStripMenuItem.Visible = true;
            else if (nombreForm == "almacen")
                reporteAlmacenToolStripMenuItem1.Visible = true;
            else if (nombreForm == "catrefacciones")
            {
                if (!catálogosToolStripMenuItem.Visible)
                    catálogosToolStripMenuItem.Visible = true;
                catálogoDeRefaccionesToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "catrefacc")
            {
                if (!catálogoDeRefaccionesToolStripMenuItem.Visible)
                    catálogoDeRefaccionesToolStripMenuItem.Visible = true;
                catálogoDeRefaccionesRecuperadasToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "percances")
            {
                if (!reporteSupervicionToolStripMenuItem.Visible) reporteSupervicionToolStripMenuItem.Visible = true;
                reporteDePercancesToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "reppersonal")
            {
                if (!reporteSupervicionToolStripMenuItem.Visible) reporteSupervicionToolStripMenuItem.Visible = true;
                else if (!reporteDePersonalToolStripMenuItem.Visible) reporteDePersonalToolStripMenuItem.Visible = true;
                reporteDeIncidentesToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "incidenciapersonal")
            {
                if (!reporteSupervicionToolStripMenuItem.Visible) reporteSupervicionToolStripMenuItem.Visible = true;
                else if (!reporteDePersonalToolStripMenuItem.Visible) reporteDePersonalToolStripMenuItem.Visible = true;
                reporteDeIndicenciaToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "ordencompra")
            {
                if (!requisicionesToolStripMenuItem.Visible) requisicionesToolStripMenuItem.Visible = true;
                ordenesDeCompraToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "comparativas")
            {
                if (!requisicionesToolStripMenuItem.Visible) requisicionesToolStripMenuItem.Visible = true;
                comparativasToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "catproveedores" || nombreForm == "catGiros")
            {
                if (!catálogosToolStripMenuItem.Visible)
                    catálogosToolStripMenuItem.Visible = true;
                catálogoDeProveedoresToolStripMenuItem.Visible = true;
            }
            else if (nombreForm == "historial")
                historialDeModificacionesToolStripMenuItem.Visible = true;
            else if (nombreForm == "changeiva")
                actualizaciónDeIVAToolStripMenuItem.Visible = true;
            else if (nombreForm == "encabezados")
                actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Visible = true;
            else if (nombreForm == "catroles")
                cátalogoDeRolesToolStripMenuItem.Visible = true;
        }
        private void catálogoDeRefaccionesToolStripMenuItem_Click(object sender, EventArgs e) {/* iraRefacciones(null);*/}
        void iraRefacciones(string idref)
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "catRefacciones")
            {
                if (cerrar())
                {
                    lblnumnotificaciones.BackgroundImage = null;
                    lbltitle.Text = nombre + "Catálogo de Refacciones";
                    this.Text = "Catálogo de Refacciones";
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    lbltitle.Location = defaultLocation;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Deshabilitar(catálogoDeRefaccionesToolStripMenuItem);
                    var form4 = Application.OpenForms.OfType<catRefacciones>().FirstOrDefault();
                    catRefacciones hijo;
                    if (!string.IsNullOrWhiteSpace(idref))
                        hijo = form4 ?? new catRefacciones(newimg, this.idUsuario, idref.ToString(), v);
                    else
                        hijo = form4 ?? new catRefacciones(newimg, this.idUsuario, empresa, area, v);
                    AddFormInPanel(hijo);
                }
            }
            else
            {
                catRefacciones c = (catRefacciones)form;
                c.actualizarTabla(idref);
            }
        }

        private void catálogoDeProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lbltitle.Text = nombre + "Catálogo de Proveedores";
                this.Text = "Catálogo de Proveedores";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form = Application.OpenForms.OfType<catProveedores>().FirstOrDefault();
                catProveedores hijo = form ?? new catProveedores(this.idUsuario, newimg, empresa, area, v);
                AddFormInPanel(hijo);
            }
        }
        void cerrarForm()
        {
            cambiarstatus(0);
            res = false;
            if (session != null)
                session.Abort();
            if (empresa == 2 && area == 1)
            {
                if (BuscarValidaciones != null)
                    BuscarValidaciones.Abort();
                if (mostrarNotificacion != null)
                    mostrarNotificacion.Abort();
            }
            if (hilo != null)
                hilo.Abort();
            timer1.Stop();
            if (this.lblnumnotificaciones.Controls.Count != 0)
                this.lblnumnotificaciones.Controls.RemoveAt(0);
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                Form fh = Application.OpenForms[i];
                if (fh.GetType() != typeof(menuPrincipal) && fh.GetType() != typeof(login))
                    fh.Close();
            }
            notifyIcon1.Dispose();
            notif.Visible = false;
            notif.Dispose();
            Owner.Show();
        }
        private void menuPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                if (MessageBox.Show("¿Esta Seguro Que Quiere Cerrar Sesión?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    cerrarForm();
                else
                    e.Cancel = true;
            }
            else
                cerrarForm();
        }
        private void timer1_Tick(object sender, EventArgs e) { obtenerNotificaciones(); }
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            v.c.insertar("UPDATE estatusValidado SET seen = 1 WHERE (SELECT (SELECT (SELECT empresaMantenimiento FROM cmodelos WHERE idmodelo = modelofkcmodelos) from cunidades WHERE idunidad = UnidadfkCUnidades) FROM reportesupervicion WHERE idreportesupervicion = idreportefkreportesupervicion)  and seen = 0");
            totalAnteriorPedidos = 0;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        public void cambiarstatus(object i)
        {
            v.c.insertar("UPDATE datosistema SET statusiniciosesion = " + i + " WHERE usuariofkcpersonal ='" + idUsuario + "'");
            //MySqlConnection localConnection = new MySqlConnection("Server = " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + "; database = sistrefaccmant ;port=" + v.c.portLocal);
            //localConnection.Open();
            //if (localConnection.State != ConnectionState.Open) localConnection.Open();
            //MySqlCommand cmd = new MySqlCommand("UPDATE datosistema SET statusiniciosesion = " + i + " WHERE usuariofkcpersonal ='" + idUsuario + "'", localConnection);
            //int res = cmd.ExecuteNonQuery();
            //localConnection.Close();
            //localConnection.Dispose();
            //localConnection = null;
        }

        private void catálogoDePersonalToolStripMenuItem_EnabledChanged(object sender, EventArgs e) { ((ToolStripMenuItem)sender).ForeColor = Color.White; }
        private void historialDeModificacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = lbltitle.Text = nombre + "Historial de Modificaciones";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form = Application.OpenForms.OfType<modificaciones>().FirstOrDefault();
                modificaciones hijo = form ?? new modificaciones(empresa, area, v);
                AddFormInPanel(hijo);
            }
        }

        private void lbltitle_DoubleClick(object sender, EventArgs e) { CenterToScreen(); }
        private void catálogosToolStripMenuItem_MouseLeave(object sender, EventArgs e) { ((ToolStripMenuItem)sender).BackColor = Color.Crimson; }
        private void catálogosToolStripMenuItem_MouseHover(object sender, EventArgs e) { }
        private void actualizaciónDeIVAToolStripMenuItem_Click(object sender, EventArgs e) { iraIva(); }
        public void iraIva()
        {
            catIVA cat = new catIVA(empresa, area, v);
            cat.Owner = this;
            cat.ShowDialog();
        }
        private void notifyIcon1_Click(object sender, MouseEventArgs e) { }

        private void menuPrincipal_Resize(object sender, EventArgs e)
        {
            lblnumnotificaciones.Left = (this.Width - lblnumnotificaciones.Width) / 2;
            lblnumnotificaciones.Top = (this.Height - lblnumnotificaciones.Height) / 2;
        }

        private void pictureBox1_Click(object sender, EventArgs e) { WindowState = FormWindowState.Minimized; }
        private void ordenesDeCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Orden de Compra";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form = Application.OpenForms.OfType<OrdenDeCompra>().FirstOrDefault();
                OrdenDeCompra hijo = form ?? new OrdenDeCompra(idUsuario, empresa, area, this, newimg, v);
                AddFormInPanel(hijo);
            }
        }

        private void comparativasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reporteDeSupervisiónToolStripMenuItem_Click(object sender, EventArgs e) { abrirReporte(); }

        private void reporteDeIndicenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Incidencias de Personal";
                this.Text = lbltitle.Text;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = new Point(1591, 13);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form1 = Application.OpenForms.OfType<Incidencia_de_Personal>().FirstOrDefault();
                Incidencia_de_Personal hijo = form1 ?? new Incidencia_de_Personal(this.idUsuario, empresa, area, v);
                AddFormInPanel(hijo);
            }
        }

        private void reporteDePercancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Reportes de Percances";
                this.Text = lbltitle.Text;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = new Point(1591, 13);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form1 = Application.OpenForms.OfType<percances>().FirstOrDefault();
                percances hijo = form1 ?? new percances(idUsuario, v/*, empresa, area*/);
                AddFormInPanel(hijo);
            }
        }
        private void reporteDeIncidentesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Reportes de Personal";
                this.Text = lbltitle.Text;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = new Point(1591, 13);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form1 = Application.OpenForms.OfType<ReportePersonal>().FirstOrDefault();
                ReportePersonal hijo = form1 ?? new ReportePersonal(idUsuario, empresa, area, v);
                AddFormInPanel(hijo);
            }
        }
        private void actualizaciónDeEncabezadosDeReportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportesVigencias rp = new ReportesVigencias(empresa, area, idUsuario, v);
            rp.Owner = this;
            rp.ShowDialog();
        }
        void Deshabilitar(ToolStripMenuItem sender)
        {
            catálogoDePersonalToolStripMenuItem.Enabled = (sender == catálogoDePersonalToolStripMenuItem ? false : true);
            catálogoDeFallosToolStripMenuItem.Enabled = (sender == catálogoDeFallosToolStripMenuItem ? false : true);
            catálogoDeProveedoresToolStripMenuItem.Enabled = (sender == catálogoDeProveedoresToolStripMenuItem ? false : true);
            catálogoDeUnidadesToolStripMenuItem.Enabled = (sender == catálogoDeUnidadesToolStripMenuItem ? false : true);
            catálogoDeRefaccionesToolStripMenuItem.Enabled = (sender == catálogoDeRefaccionesToolStripMenuItem ? false : true);
            reporteDeSupervisiónToolStripMenuItem.Enabled = (sender == reporteDeSupervisiónToolStripMenuItem ? false : true);
            reporteMantenimientoToolStripMenuItem.Enabled = (sender == reporteMantenimientoToolStripMenuItem ? false : true);
            comparativasToolStripMenuItem.Enabled = (sender == comparativasToolStripMenuItem ? false : true);
            reporteAlmacenToolStripMenuItem1.Enabled = (sender == reporteAlmacenToolStripMenuItem1 ? false : true);
            requisicionesToolStripMenuItem.Enabled = (sender == requisicionesToolStripMenuItem ? false : true);
            historialDeModificacionesToolStripMenuItem.Enabled = (sender == historialDeModificacionesToolStripMenuItem ? false : true);
            reporteDePercancesToolStripMenuItem.Enabled = (sender == reporteDePercancesToolStripMenuItem ? false : true);
            reporteDeIncidentesToolStripMenuItem.Enabled = (sender == reporteDeIncidentesToolStripMenuItem ? false : true);
            reporteDeIndicenciaToolStripMenuItem.Enabled = (sender == reporteDeIndicenciaToolStripMenuItem ? false : true);
            ordenesDeCompraToolStripMenuItem.Enabled = (sender == ordenesDeCompraToolStripMenuItem ? false : true);
            rolesDeServiciosToolStripMenuItem.Enabled = (sender == rolesDeServiciosToolStripMenuItem ? false : true);
            cátalogoDeRolesToolStripMenuItem.Enabled = (sender == cátalogoDeRolesToolStripMenuItem ? false : true);
        }

        private void rolesDeServiciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Roles de Servicios";
                this.Text = "Roles de Servicios";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form = Application.OpenForms.OfType<workdays>().FirstOrDefault();
                workdays hijo = form ?? new workdays(this);
                AddFormInPanel(hijo);
            }
        }

        private void cátalogoDeRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Catálogo de Roles";
                this.Text = lbltitle.Text;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = new Point(1591, 13);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form1 = Application.OpenForms.OfType<CatRoles>().FirstOrDefault();
                CatRoles hijo = form1 ?? new CatRoles(idUsuario, empresa, area, v);
                AddFormInPanel(hijo);
            }
        }

        private void dieselToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Diesel Die = new Diesel(idUsuario, empresa, area, v);
            Die.Owner = this;
            Die.ShowDialog();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void notif_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void catToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iraRefacciones(null);
        }

        private void catálogoDeRefaccionesRecuperadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iraRefaccionesRecu(null);
        }
        void iraRefaccionesRecu(string idref)
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "catRefaccionesRecu")
            {
                if (cerrar())
                {
                    lblnumnotificaciones.BackgroundImage = null;
                    lbltitle.Text = nombre + "Refacciones Recuperadas";
                    this.Text = "Catálogo de Refacciones Recuperadas";
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    lbltitle.Location = defaultLocation;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    Deshabilitar(catálogoDeRefaccionesRecuperadasToolStripMenuItem);
                    var formnuevo = Application.OpenForms.OfType<Material_Recuperado>().FirstOrDefault();
                    Material_Recuperado hijo;
                    if (!string.IsNullOrWhiteSpace(idref))
                        hijo = formnuevo ?? new Material_Recuperado(newimg, this.idUsuario, idref.ToString(), v);
                    else
                        hijo = formnuevo ?? new Material_Recuperado(newimg, this.idUsuario, empresa, area, v);
                    AddFormInPanel(hijo);
                }
            }
            else
            {
                Material_Recuperado c = (Material_Recuperado)form;
                // c.actualizarTabla(idref);
            }
        }

        private void refaaciónesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Requerimiento";
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = defaultLocation;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Deshabilitar(sender as ToolStripMenuItem);
                var form = Application.OpenForms.OfType<requisicionRefacciones>().FirstOrDefault();
                requisicionRefacciones hijo = form ?? new requisicionRefacciones(empresa, area, v);
                AddFormInPanel(hijo);
            }
        }
        void iraRequcisionHerramienta(object irH)
        {
            string name = "";
            if (form != null) name = form.Name;
            if (name != "RequisicionHerramienta")
            {
                if (cerrar())
                {
                    lblnumnotificaciones.BackgroundImage = null;
                    lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                    lbltitle.Text = nombre + "Requerimiento";
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //lbltitle.Location = defaultLocation;
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    Deshabilitar(irH as ToolStripMenuItem);
                    var form = Application.OpenForms.OfType<RequisicionHerramienta>().FirstOrDefault();
                    RequisicionHerramienta hijo = form ?? new RequisicionHerramienta(empresa, area, this.idUsuario, v);
                    AddFormInPanel(hijo);
                }
            }
            else
            {
                Material_Recuperado c = (Material_Recuperado)form;
                // c.actualizarTabla(idref);
            }
        }
        private void herramientaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iraRequcisionHerramienta(sender);
        }

        private void consultaRetornoMaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirMantenimiento();
        }

        private void consultaRetornoMaterialToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void almacenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirAlmacen();
        }

        private void consultaRetornoMaterialToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Retorno_de_material O = new Retorno_de_material(idUsuario, empresa, area, v, "", "", "", "", "");
            O.Owner = this;
            O.ShowDialog();
        }

        private void totalEntradasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            catREntradas rep = new catREntradas(v, empresa, area, idUsuario);
            rep.Owner = this;
            rep.ShowDialog();
        }

        private void validaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
             Reportes rep = new Reportes(idUsuario, empresa, area, v);
             rep.Owner = this;
             rep.ShowDialog();
        }

        private void totalSalidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            catRSalidas rep = new catRSalidas(v, empresa, area, idUsuario);
            rep.Owner = this;
            rep.ShowDialog();
        }

        private void corrocerosExtermosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Otros rep = new Otros(idUsuario, empresa, area, v);
            rep.Owner = this;
            rep.ShowDialog();
        }

        private void ventaExternaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReporteVenExt venta = new ReporteVenExt(v, empresa, area, idUsuario);
            venta.Owner = this;
            venta.Show();
        }

        private void materialParaProduccionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaterialProduccion venta = new MaterialProduccion(v, empresa, area, idUsuario);
            venta.Owner = this;
            venta.Show();
        }

        private void asistencíaDelDíaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                lblnumnotificaciones.BackgroundImage = null;
                lblnumnotificaciones.BorderStyle = BorderStyle.Fixed3D;
                lbltitle.Text = nombre + "Asistencía del Día";
                this.Text = lbltitle.Text;
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //lbltitle.Location = new Point(1591, 13);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Deshabilitar(sender as ToolStripMenuItem);
                var form1 = Application.OpenForms.OfType<Asistencia>().FirstOrDefault();
                Asistencia hijo = form1 ?? new Asistencia();
                AddFormInPanel(hijo);
            }
        }

        private void lblnumnotificaciones_Paint(object sender, PaintEventArgs e)
        {

        }

        private void catálogosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}