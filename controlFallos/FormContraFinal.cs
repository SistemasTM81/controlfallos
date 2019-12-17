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
    public partial class FormContraFinal : Form
    {
        validaciones val;
        conexion co = new conexion();
        Form FFM;
        OrdenDeCompra ODC;
        percances PC;
        ReportePersonal RP;
        Incidencia_de_Personal IP;
        Form f;

        int empresa, area;
        public string id, tipobtn;

        public FormContraFinal(int empresa, int area, Form F,validaciones v)
        {
            this.val = v;
            InitializeComponent();
            if (empresa == 1 && area == 1)
                if (F.Name == "ReportePersonal")
                    RP = (ReportePersonal)F;
                else if (F.Name == "percances")
                    PC = (percances)F;
                else if (F.Name == "Incidencia de Personal")
                    IP = (Incidencia_de_Personal)F;
                else if (empresa == 2 && area == 2)
                    ODC = (OrdenDeCompra)F;
            this.empresa = empresa;
            this.area = area;
            f = F;
        }

        public void buttonAceptar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblNombreFinal.Text))
            {
                if (empresa == 1 && area == 1)
                {
                    if (f.Name == "ReportePersonal")
                        RP.idFinal = Convert.ToInt32(id);
                    else if (f.Name == "Incidencia de Personal")
                        IP.idf = Convert.ToInt32(id);
                }
                else if (empresa == 2 && area == 2)
                    ODC.labelidFinal.Text = id;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Contraseña Incorrrecta", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                id = "0";
                textBoxUsuFinal.Clear();
                textBoxUsuFinal.Focus();
                DialogResult = DialogResult.None;
                DialogResult = DialogResult.None;
            }
        }

        public void textBoxUsuFinal_TextChanged(object sender, EventArgs e)
        {
            busquedageneral("SELECT t1.idPersona, UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + val.Encriptar(textBoxUsuFinal.Text) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'", "idPersona", "Nombre");
        }

        public void busquedageneral(string consulta, string tituloid, string nombre)
        {
            MySqlCommand busqueda = new MySqlCommand(consulta, co.dbconection());
            MySqlDataReader dr = busqueda.ExecuteReader();
            if (dr.Read())
            {
                id = dr.GetString(tituloid);
                lblNombreFinal.Text = dr.GetString(nombre);
            }
            else
                lblNombreFinal.Text = "";
            dr.Close();
            co.dbconection().Close();
        }

        public void btnall_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(46, 45);
        }

        private void FormContraFinal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(id)) DialogResult = DialogResult.OK;
            else DialogResult = DialogResult.Abort;
        }

        public void btnall_MouseLeave(object sender, EventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(41, 40);
        }

        private void textBoxUsuFinal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                buttonAceptar_Click(null, e);
            else
                val.letrasynumerossinespacios(e);
        }
    }
}
