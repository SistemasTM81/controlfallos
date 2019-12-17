using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Threading;
using System.Security.Cryptography;
namespace controlFallos
{
    public class conexion
    {
        public string host { protected internal set; get; }
        public string user { protected internal set; get; }
        public string password { protected internal set; get; }
        public string port { protected internal set; get; }
        public string hostLocal { protected internal set; get; }
        public string userLocal { protected internal set; get; }
        public string passwordLocal { protected internal set; get; }
        public string portLocal { protected internal set; get; }
        public MySqlConnection dbcon;
        MySqlConnection localConnection;
        public conexion()
        {
            string path = Application.StartupPath + @"\conexion.txt";
            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path, true, Encoding.ASCII);
                sw.Write("0yLCd4LvwPo9xeMPa3Xo60R8ubmf9ZS4hs58llM/Lovd0yqGbDjTyz2KnbCOiM+bcf37rsKzAOWK1oy54Z7Zng4ZOiXbUAwqvWJXAPV18ec=");
                sw.Close();
            }
            StreamReader lector = new StreamReader(path);
            var res = lector.ReadLine();
            lector.Close();
            try
            {
                string[] arreglo = Desencriptar(res).Split(';');
                this.host = arreglo[0];
                this.user = arreglo[1];
                this.password = arreglo[2];
                this.port = arreglo[3];
                this.hostLocal = arreglo[4];
                this.userLocal = arreglo[5];
                this.passwordLocal = arreglo[6];
                this.portLocal = arreglo[7];
                localConnection = new MySqlConnection("Server = " + hostLocal + "; user=" + userLocal + "; password = " + passwordLocal + " ; database = sistrefaccmant ;port=" + portLocal);
            }
            catch { }
        }
        public string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "sistemafallos";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception) { }
            return textoEncriptado;
        }
        public MySqlConnection dbconection()
        {
            try
            {
                if (conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { host, user, password, port }));
                else
                    dbcon = new MySqlConnection("Server = " + hostLocal + "; user=" + userLocal + "; password = " + passwordLocal + "; database = sistrefaccmant ;port=" + portLocal);
                if (dbcon.State != System.Data.ConnectionState.Open) dbcon.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (Form frm in Application.OpenForms)
                    frm.Close();
                Application.Exit();
            }
            return dbcon;
        }
        public bool insertar(String sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, dbconection());
                int i = cmd.ExecuteNonQuery();
                if (conexionOriginal())
                {
                    if (localConnection.State != System.Data.ConnectionState.Open) localConnection.Open();
                    cmd = new MySqlCommand(sql, localConnection);
                    i = cmd.ExecuteNonQuery();
                    localConnection.Close();
                    localConnection.Dispose();
                }
                dbcon.Close();
                dbcon.Dispose();
                if (!conexionOriginal())
                    WriteLocalSequence(sql);

                if (i >= 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                dbcon.Close();
                dbcon.Dispose();

                MessageBox.Show(ex.HResult + ": " + ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public void referencia(int idUsuario)
        {
            string path = Application.StartupPath + @"\contains.txt";
            StreamWriter sw = new StreamWriter(path, true, Encoding.ASCII);
            sw.Write(idUsuario + ";");
            sw.Close();
        }
        public void insertarGlobal()
        {
            try
            {
                validaciones v = new validaciones();
                string path = Application.StartupPath + @"\updates.srf";

                using (StreamReader lector = new StreamReader(path))
                {
                    string sql;
                    while (!string.IsNullOrWhiteSpace((sql = lector.ReadLine())))
                        insertar(v.Desencriptar(sql));
                }
                File.Delete(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.HResult + ": " + ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool conexionOriginal()
        {
            try
            {
                Ping p = new Ping();
                return (p.Send(host).Status == IPStatus.Success);
            }
            catch { return false; }
        }
        protected internal void WriteLocalSequence(string seq)
        {
            validaciones v = new validaciones();
            StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath + @"\updates.srf"), true, Encoding.ASCII);
            sw.WriteLine(v.Encriptar(seq));
            sw.Close();
        }
        public string[] tableNames = new string[] {"bloqueologin","canaqueles","careas","catcategorias","catincidencias","cattipos","ccharolas","cdescfallo","cempresas","cestaciones","cfallosesp","cfallosgrales","cfamilias","cgiros","civa","cladas","cmarcas","cmedidas","cmodelos","cnfamilias","cniveles","comparativas","cpasillos","cpersonal","cproveedores","crefacciones","cservicios","cunidades","cunidadmedida","datosistema","detallesordencompra","encabezadoreportes","estatusvalidado","huellasupervision","incidenciapersonal","ladanac","modificaciones_sistema","nombresoc","ordencompra","pedidosrefaccion","privilegios","proveedorescomparativa","puestos","refaccionescomparativa","relacservicioestacion","reportemantenimiento","reportepercance","reportepersonal","reportesupervicion","reportetri","sepomex","vigencias_supervision"};
    }
}