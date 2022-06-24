using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace controlFallos
{
    public partial class Serie_Llantas : Form
    {
        validaciones v;
        int idUsuario, empresa, area, Contador = 0;
        double cantidad = 0.0;
        string codigo = "";
        DataTable dt;

        public Serie_Llantas()
        {
            InitializeComponent();
        }
        public Serie_Llantas(validaciones v, int idUsuario, int empresa, int area,double cantidad, string codigo)
        {
            this.v = v;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.cantidad = cantidad;
            this.codigo = codigo;
            InitializeComponent();
        }


        private void Load_Series(object sender, EventArgs e)
        {
            txtCantidad.Text = Convert.ToString(cantidad);
        }

        private void AgregarSeries(object sender, EventArgs e)
        {
            if (Contador <= cantidad)
            {
                string a = v.getaDataR("SET NAMES 'utf8';SET lc_time_names = 'es_ES';select idcseries_llantas from cseries_llantas  where serie ='" + txtSeries.Text + "'").ToString();
                if (a.ToString().Equals(""))
                {
                    v.c.insertar("INSERT INTO cseries_llantas(refaccionfkcrefacciones,serie, FechaHora) values ('" + codigo.ToString() + "','" + txtSeries.Text + "', now())");
                    Contador++;
                    Limpiar();
                }
                else
                {
                    MessageBox.Show("Nuemero de serie ya registrado", "!Importante¡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
               
            }
            else
            {
                MessageBox.Show("Ya no es posible agregar mas series", "!Importante¡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
            
        }
        private void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ValidarSeries(object sender, KeyPressEventArgs e)
        {
            v.letrasNumerosGuiones(e);
        }

        void ConsultarAgregados()
        {
            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = v.getReport("SET NAMES 'utf8';SET lc_time_names = 'es_ES';select serie as 'Num. Serie', FechaHora from cseries_llantas where date_format(FechaHora, '%Y-%m-%d') = date_format(now(), '%Y-%m-%d')");
            adapter.Fill(ds);
            dgvSeries.DataSource = ds.Tables[0];
        }
        void Limpiar()
        {
            ConsultarAgregados();
            txtSeries.Text = "";
        }






    }
}
