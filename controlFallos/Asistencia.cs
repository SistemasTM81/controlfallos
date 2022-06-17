using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class Asistencia : Form
    {
        validaciones v = new validaciones();
        public Asistencia()
        {
            InitializeComponent();
            cmbmesb.DrawItem += v.combos_DrawItem;
        }

        private void txtcredencial_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtcredencial.Text))
            {
                if (Convert.ToInt32(v.getaData("select count(*) from cpersonal where credencial='" + txtcredencial.Text + "' and empresa='1'")) > 0)
                {
                    string[] datos = v.getaData("select upper(concat(coalesce(t1.appaterno,''),' ',coalesce(t1.apmaterno,''),' ',t1.nombres,'|',t2.puesto)) from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.credencial='" + txtcredencial.Text + "';").ToString().Split('|');
                    lblnombre.Text = datos[0];
                    lblpuesto.Text = datos[1];
                }
                else
                    lblnombre.Text = lblpuesto.Text = "";
            }
            else lblnombre.Text = lblpuesto.Text = "";
        }
        void busqueda()
        {
            v.comboswithuot(cmbmesb, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
        }
        private void Asistencia_Load(object sender, EventArgs e)
        {
            busqueda();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

        }
    }
}
