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
        public Button buttonSelected;
        int x = 5, y = 10;
        public viewDriver(rolUnidades Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            th = new Thread(dataLoader);
            th.IsBackground = true;
            th.Start();
            th.Join();
        }

        private void dataLoader()
        {
            if (InvokeRequired)
                Invoke(new dataLoad(dataLoader));
            else
            {
                DataTable dt = (DataTable)Owner.Owner.v.getData("select idPersona as id, t1.credencial as Nombre from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.status='1'   AND t2.status='1' and t1.empresa='1' and t1.area='1';");
                foreach (DataRow roe in dt.Rows)
                    createControlDriver(roe.ItemArray[0], roe.ItemArray[1]);
                th.Abort();
            }
        }

        private void createControlDriver(object driverID, object driverCRED)
        {
            Button btn = new Button();
            btn.FlatStyle = FlatStyle.Flat;
            btn.Text = driverCRED.ToString();
            btn.UseMnemonic = false;
            btn.Name = "Button|" + driverID;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Size = new Size(50, 30);
            btn.Location = new Point(x, y);
            btn.Cursor = Cursors.Hand;
            btn.Click += Btn_DoubleClick;
            backgroundPanel.Controls.Add(btn);
            x += 50;
            if (x + 50 >= backgroundPanel.Size.Width){x = 5; y = y+30;}
        }

        private void Btn_DoubleClick(object sender, EventArgs e)
        {
            buttonSelected = ((Button)sender);
            DialogResult = DialogResult.OK;
        }
    }
}
