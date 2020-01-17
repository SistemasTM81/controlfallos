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
    public partial class diferenciaecos : Form
    {
        validaciones v;
        public int cecos;
        int danterior;
        public string[] diferen;
        int c = 1;
        bool editar;
        public diferenciaecos(validaciones v)
        {
            InitializeComponent();
            this.v = v;
        }

        private void diferenciaecos_Load(object sender, EventArgs e)
        {
            lbltitle.Left = (this.Width - lbltitle.Width) / 2;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            diferen[c - 1] = txtdiferencia.Text;
            lbltexto.Text = "Diferencia entre unidad " + (c + 1) + " y " + (c + 2);
            lbdiferencia.Items.Add("Diferencia entre unidad " + c + " y " + (c + 1) + " ----------->" + diferen[c - 1] + " minutos.");
            txtdiferencia.Clear();
            c++;
            paceptar.Visible = !(padd.Visible = pdatos.Visible = ((c - 1) == cecos ? false : true));
        }

        private void txtdiferencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void lbdiferencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            pdatos.Visible = true;
            txtdiferencia.Text = diferen[Convert.ToInt32(lbdiferencia.SelectedIndex)];
        }

        private void btnaceptar_Click(object sender, EventArgs e)
        {
            string aux = "";
            CatRoles c = (CatRoles)Owner;
            for (int i = 0; i < diferen.Length; i++)
                aux = (i < diferen.Length - 1 ? aux += diferen[i] + "," : aux += diferen[i]);
            c.lbltime.Text = aux;
            this.Close();
        }
    }
}
