using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class viewDriver : Form
    {
        Thread th;
        new rolUnidades Owner;
        delegate void dataLoad();
        public viewDriver(rolUnidades Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            lstbxDrivers.DrawItem += Owner.Owner.v.listbox_DrawItem;
            th = new Thread(dataLoader);
            th.IsBackground = true;
            th.Start();
            th.Join();
        }

        private void dataLoader()
        {
            if (lstbxDrivers.InvokeRequired)
            {
                Invoke(new dataLoad(dataLoader));
            }
            else
            {
                lstbxDrivers.ValueMember = "id";
                lstbxDrivers.DisplayMember = "Nombre";
                lstbxDrivers.DataSource = Owner.Owner.v.getData("select idPersona as id, t1.credencial as Nombre from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.status='1'   AND t2.status='1' and t1.empresa='1' and t1.area='1';");
                th.Abort();
            }
        }
    }
}
