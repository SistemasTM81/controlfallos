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
                string[] ocupados = { };
                if (Owner.periodID.HasValue)
                    ocupados = Owner.Owner.v.getaData("CALL getAllDrivers(" + Owner.periodID.Value + ");").ToString().Split('|');
                backgroundPanel.Controls.Clear();
                foreach (DataRow roe in dt.Rows)
                {
                    int index = Array.IndexOf(ocupados, roe.ItemArray[0].ToString());
                    createControlDriver(roe.ItemArray[0], roe.ItemArray[1], (ocupados != null && ocupados.GetLength(0) > 0 ? (bool?)(index >= 0) : null));
                }

                th.Abort();
            }
        }

        private void createControlDriver(object driverID, object driverCRED, bool? existe)
        {
            bool canIBuild = true;
            if (existe.HasValue) canIBuild = !existe.Value;
            if (canIBuild)
            {
                Button btn = new Button();
                btn.FlatStyle = FlatStyle.Flat;
                btn.Name = "Button|" + driverID;
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Cursor = Cursors.Hand; 
                btn.UseMnemonic = false;
                btn.Click += Btn_DoubleClick;
                backgroundPanel.Controls.Add(addContol(btn, canIBuild, driverCRED.ToString()));
            }
            else
            {
                Label lbl = new Label();
                lbl.FlatStyle = FlatStyle.Flat;
                lbl.Name = "Label|" + driverID;
                lbl.UseMnemonic = false;
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.ForeColor = Color.White;
                lbl.UseCompatibleTextRendering = true;
                backgroundPanel.Controls.Add(addContol(lbl, canIBuild, driverCRED.ToString()));
            }
            x += 50;
            if (x + 50 >= backgroundPanel.Size.Width) { x = 5; y = y + 30; }

        }
        Control addContol(Control control, bool existe, string Text)
        {
            control.Size = new Size(50, 30);
            control.Location = new Point(x, y);
            control.BackColor = (!existe ? Color.Crimson : Color.FromArgb(200, 200, 200));
            control.Text = Text;
            return control;
        }

        private void viewDriver_Load(object sender, EventArgs e)
        {

        }

        private void Btn_DoubleClick(object sender, EventArgs e)
        {
            buttonSelected = ((Button)sender);
            DialogResult = DialogResult.OK;
        }
    }
}
