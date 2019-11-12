using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    class bloqueoLogin
    {

        validaciones v;
        bool existeUsuario;
        
        public bool tipoBloqueo{set;get;}
        public int intentos{set;get;}
        public bloqueoLogin(validaciones v){this.v = v; intentos = 0;}
        public string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")localIP = ip.ToString();
                
            }
            return localIP;

        }
        public void yaexisteUsuario(string usuario)
        {
            existeUsuario = Convert.ToInt32(v.getaData("SELECT count(iddato) as cuenta FROM datosistema WHERE usuario = '" + usuario + "'")) > 0;
        }

        public void bloquear(string usuario)
        {
            try
            {
                if (existeUsuario)
                {
                    v.c.insertar("INSERT INTO bloqueologin(usuario, fechaHora, ipclient, statusbloqueo, tipobloqueo) VALUES('" + usuario + "',NOW(),'" + GetIPAddress() + "','1','0')");
                    tipoBloqueo = false;
                }
                else
                {
                    v.c.insertar("INSERT INTO bloqueologin(usuario, fechaHora, ipclient, statusbloqueo, tipobloqueo) VALUES('" + usuario + "',NOW(),'" + GetIPAddress() + "','1','1')");
                    tipoBloqueo = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool usuarionobloqueado(string usuario)
        {
            return Convert.ToInt32(v.getaData("SELECT COUNT(idloginstatus) as cuenta FROM bloqueologin WHERE tipobloqueo = 0 and statusbloqueo = 1 and usuario='" + usuario + "'")) > 0;
        }
        public bool noHainiciadoSesion(string usuario)
        {
            return Convert.ToInt32(v.getaData("SELECT statusiniciosesion FROM datosistema WHERE usuariofkcpersonal= '" + usuario + "'")) > 0;
        }
    }
}
