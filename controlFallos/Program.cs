using System;
using System.Threading;
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
            Thread th = new Thread(new ThreadStart(Splash));
            th.Start();                        
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
        static void Splash(){try{if (new conexion().conexionOriginal()) { var clonarTable = new databaseLocalClone(v); }}catch { }}
    }
}