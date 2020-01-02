using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using h = Microsoft.Office.Interop.Excel;
using System.Globalization;

namespace controlFallos
{
    public partial class FormFallasMantenimiento : Form
    {

        validaciones v;
        int idUsuario, empresa, area;
        static bool res = true;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }

        public Thread hilo;


        void quitarseen()
        {
            while (res)
            {
                MySqlConnection dbcon = null;
                if (v.c.conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                else
                    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE reportesupervicion SET seen = 1 WHERE seen  = 0", dbcon);
                cmd.ExecuteNonQuery();
                dbcon.Close();
                Thread.Sleep(180000);
            }
        }

        public FormFallasMantenimiento(int idUsuario, int empresa, int area, System.Drawing.Image newimg, validaciones v)
        {
            InitializeComponent();
            this.v = v;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
        }

        private void FormFallasMantenimiento_Load(object sender, EventArgs e)
        {
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();

        }


    }
}