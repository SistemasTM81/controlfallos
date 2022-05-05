using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    static class Program
    {
        public static validaciones v;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            v = new validaciones();


            //Thread th = new Thread(new ThreadStart(Splash)) { IsBackground = true };
            //th.Start();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new login(v));
        }
        static void obtenerHost()
        {
            foreach (Form frm in Application.OpenForms)
                frm.Close();
            Application.Exit();
        }
        /*static async void Splash()
        {
            try
            {
                if (await System.Threading.Tasks.Task.Run(() => v.c.conexionOriginal()))
                {
                    var clonarTable = new databaseLocalClone(v);
                }
            }
            catch (Exception ex)
            {
            }
        }
        ////static async void IndicaAsync()
        ////{
        ////    await Splash();
        ////}
        ////static async Task Splash() { try { if (new conexion(v).conexionOriginal()) { var clonarTable = new databaseLocalClone(v); } } catch (Exception ex) { throw ex; } }*/
    }
}