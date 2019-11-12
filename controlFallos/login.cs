using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using System.IO;
namespace controlFallos
{
    public partial class login : Form
    {
        validaciones v;
        bloqueoLogin bl;

        TimeSpan diferencia;
        //Creamos el delegado 
        ThreadStart delegado;
        //Creamos la instancia del hilo 
        Thread hilo;
        public login(validaciones v)
        {
            InitializeComponent();
         
            this.v = v;
            bl = new bloqueoLogin(v);
            delegado = new ThreadStart(desbloquearUsuarios);
            hilo = new Thread(delegado);
            lbltitle.Left = (status.Width - lbltitle.Width) / 2;
            lbltitle.Top = (status.Height - status.Height) / 2;
            v.ChangeControlStyles(btnlogin, ControlStyles.Selectable, false);
            //Iniciamos el hilo 
            if (hilo.ThreadState == ThreadState.Stopped || hilo.ThreadState == ThreadState.Unstarted)
                hilo.Start();
            string path = Application.StartupPath + @"\contains.txt";
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                string line = sr.ReadLine();
                string[] idUsuarios = line.Trim(';').Split(';');
                for (int i = 0; i < idUsuarios.Length; i++)
               if(v.c.conexionOriginal())v.c.insertar("UPDATE datosistema SET statusiniciosesion = 0 WHERE usuariofkcpersonal ='" + idUsuarios[i] + "'");
                sr.Close();
                File.Delete(path);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.numeroyLetrasSinAcentos(e);
            if (e.KeyChar == 13)
                button1_Click(null, e);
        }
        private void status_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgetusu.Text) && !string.IsNullOrWhiteSpace(txtgetpass.Text))
            {

                string usu = txtgetusu.Text;
                string pass = v.Encriptar(txtgetpass.Text);
                object data = v.getaData("SELECT CONCAT(t2.idpersona ,';',t2.empresa,';', t2.area) FROM datosistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idPersona WHERE  t1.usuario COLLATE utf8_bin ='" + usu + "' and t1.password COLLATE utf8_bin ='" + pass + "' and status = '1'");
                if (data != null)
                {
                    if (Convert.ToInt32(v.getaData("SELECT t2.status as cuenta FROM datosistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal =t2.idpersona WHERE t1.usuario ='" + usu + "' AND password ='" + pass + "'")) > 0)
                    {
                        if (!bl.usuarionobloqueado(usu))
                        {
                        
                       
                                object[] datos = data.ToString().Split(';');
                                if (!bl.noHainiciadoSesion(datos[0].ToString()))
                                {
                                    if (Convert.ToInt32(v.getaData("SELECT count(idprivilegio) FROM privilegios WHERE usuariofkcpersonal= '" + datos[0] + "' ")) > 0)
                                    {
                                        bl.intentos = 0;

                                        menuPrincipal m = new menuPrincipal(Convert.ToInt32(datos[0]), Convert.ToInt32(datos[1]), Convert.ToInt32(datos[2]), this, v);
                                        Hide();
                                        m.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show("No tiene privilegios para navegar por el sistema. Contacte a su administrador de area", v.sistema(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("El usuario Tiene una Sesión Activa", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                           
                        }
                        else
                        {
                            MessageBox.Show("El usuario ha sido Bloqueado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El usuario ingresado ha sido desactivado por el Administrador", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (bl.intentos == 2)
                    {
                        bl.bloquear(txtgetusu.Text);
                        if (bl.tipoBloqueo)
                        {
                            MessageBox.Show("El Sistema se ha bloqueado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            plogin.Visible = false;
                            lblsistemaBloqueado.Visible = true;
                            btnlogin.Visible = false;
                            lbllogin.Visible = false;
                            bl.intentos = 0;
                            lblintentos.Text = "";
                            diferencia = new TimeSpan(0, 5, 0);
                            timer1.Start();
                        }
                        else
                        {
                            MessageBox.Show("El usuario se ha bloqueado por exceso de intentos Fallidos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            bl.intentos = 0;
                            lblintentos.Text = "";
                        }
                    }
                    else
                    {
                        bl.intentos = bl.intentos + 1;
                        lblintentos.Text = "Intentos Fallidos: " + bl.intentos;
                        bl.yaexisteUsuario(txtgetusu.Text);
                        MessageBox.Show("Acceso Incorrecto", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtgetusu.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("El usuario o la contraseña no pueden estar vacíos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
            txtgetusu.Clear();
            txtgetpass.Clear();
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            diferencia = diferencia.Subtract(TimeSpan.FromSeconds(1));
            lblintentos.Text = "El Sistema se Desbloqueara en : " + diferencia.Minutes + ":" + diferencia.Seconds;

            if (diferencia.Minutes == 0 && diferencia.Seconds == 0)
            {
             if(v.c.conexionOriginal())   v.c.insertar("UPDATE bloqueologin SET statusbloqueo ='0' WHERE ipclient = '" + bl.GetIPAddress() + "'");
                plogin.Visible = true;
                lblsistemaBloqueado.Visible = false;
                btnlogin.Visible = true;
                lbllogin.Visible = true;
                bl.intentos = 0;
                lblintentos.Text = "";
                timer1.Stop();
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            try
            {
                var res= v.getaData("SELECT TIMEDIFF(TIME(NOW()),TIME(fechaHora)) as tiempo FROM bloqueologin WHERE ipclient = '" + bl.GetIPAddress() + "' and statusbloqueo = 1 and tipobloqueo =1");
                if (res!=null)
                {
                    plogin.Visible = false;
                    lblsistemaBloqueado.Visible = true;
                    btnlogin.Visible = false;
                    lbllogin.Visible = false;
                    bl.intentos = 0;
                    diferencia = new TimeSpan(0, 5, 0);
                    diferencia = diferencia.Subtract(TimeSpan.Parse(res.ToString()));
                    timer1.Start();
                }
            }
            catch
            {
               // this.Hide();
            }
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(SplashScreen))
                {
                    if (frm.InvokeRequired)
                    {

                        validaciones.delgado dm = new validaciones.delgado(v.cerrarForm);

                        Invoke(dm, frm);
                    }

                    break;
                }
            }
        }

        private void txtgetusu_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUsuarios(e);
        }
         bool newe = true;
     
         void desbloquearUsuarios()
        {
            try
            {
                while (newe)
                {
                    try
                    {
                        MySqlConnection dbcon = null;
                        if (v.c.conexionOriginal())
                        {
                            dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                            if (File.Exists(Application.StartupPath + "/updates.srf"))
                                v.c.insertarGlobal();
                        }
                        else
                            dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                        dbcon.Open();

                        MySqlCommand cmd = new MySqlCommand("UPDATE bloqueologin SET statusbloqueo = 0 WHERE TIME_TO_SEC(TIMEDIFF(TIME(NOW()),TIME(fechaHora))) > 300 and statusbloqueo=1", dbcon);
                        cmd.ExecuteNonQuery();
                        dbcon.Close();
                        dbcon.Dispose();
                        Thread.Sleep(500);
                    }
                    catch { continue; }
                }
            }
            catch
            {
               
            }
        }

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            newe = false;
            hilo.Abort();
            Application.ExitThread();
            Application.Exit();
        }
        private void login_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
