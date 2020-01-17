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
    public partial class workdays : Form
    {
        public long? periodID { protected internal set; get; }
        public long? rolfkCRoles { protected internal set; get; }
        public long? serviceID { protected internal set; get; }
        new public menuPrincipal Owner { protected internal set; get; }
        delegate void dataLoad();
        int x = 0, y = 0;
        System.Threading.Thread th;
        public workdays(menuPrincipal Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            th = new System.Threading.Thread(dataLoader);
            th.IsBackground = true;
            th.Start();
            th.Join();
            Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiod, new string[] { "DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --", 1);
        }
        private void dataLoader()
        {
            if (InvokeRequired)
                Invoke(new dataLoad(dataLoader));
            else
            {
                DataTable dt = (DataTable)Owner.v.getData("SELECT * FROM getdriversrol");
                string[] ocupados = { };
                if (periodID.HasValue)
                    ocupados = Owner.v.getaData("CALL getAllDrivers(" + periodID.Value + ");").ToString().Split('|');
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
                //    btn.Click += Btn_DoubleClick;
                btn.MouseDown += Btn_MouseDown;
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
            if (x + 50 >= backgroundPanel.Size.Width) { x = 0; y+= 30; }

        }

  

        Control addContol(Control control, bool existe, string Text)
        {
            control.Size = new Size(50, 30);
            control.Location = new Point(x, y);
            control.BackColor = (!existe ? Color.Crimson : Color.FromArgb(200, 200, 200));
            control.Text = Text;
            return control;
        }

    }
}
