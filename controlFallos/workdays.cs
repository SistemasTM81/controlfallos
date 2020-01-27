using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        Thread th, thload;
        public workdays(menuPrincipal Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            th = new Thread(dataLoader) { IsBackground = true};
            th.Start();
            Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiod, new string[] { "DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --", 1);
        }
        private void dataLoader()
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
        private void button2_Click(object sender, EventArgs e){}
        private void dgvcycles_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell cell in dgvcycles.SelectedCells)
                { if (!dgvcycles.Columns[cell.ColumnIndex].HeaderText.Equals("HORA")) cell.Value = string.Empty; }
            }
        }
        private void dgvcycles_SelectionChanged(object sender, EventArgs e)
        {
               foreach (DataGridViewCell cell in dgvcycles.SelectedCells)
                { if (dgvcycles.Columns[cell.ColumnIndex].HeaderText.Equals("HORA") || dgvcycles.Columns[cell.ColumnIndex].HeaderText.Equals("CICLO")) cell.Selected = false; }
        }
        private void button3_Click(object sender, EventArgs e){}
        private void createControlDriver(object driverID, object driverCRED, bool? existe)
        {
            if (InvokeRequired)
                Invoke(new dataLoad(dataLoader));
            else
            {
                bool canIBuild = true;
                if (existe.HasValue) canIBuild = !existe.Value;
                if (canIBuild)
                {
                    Button btn = new Button() { FlatStyle = FlatStyle.Flat, Name = "Button | " + driverID, TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand, UseMnemonic = false };
                    btn.Click += Btn_Click;
                    btn.MouseDown += Btn_MouseDown;
                    backgroundPanel.Controls.Add(addContol(btn, canIBuild, driverCRED.ToString()));
                }
                else
                {
                    Label lbl = new Label() { FlatStyle = FlatStyle.Flat, Name = "Label|" + driverID, UseMnemonic = false, BorderStyle = BorderStyle.FixedSingle, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.White, UseCompatibleTextRendering = true };
                    backgroundPanel.Controls.Add(addContol(lbl, canIBuild, driverCRED.ToString()));
                }
                x += 50;
                if (x + 50 >= backgroundPanel.Size.Width) { x = 0; y += 30; }
            }
        }
        /// <summary>
        ///Method that allow Specify some properties such as: size, Location or background that have in common a button and a label
        /// </summary>
        /// <param name="control"></param>
        /// <param name="existe"></param>
        /// <param name="Text"></param>
        /// <returns>The Control With the new properties</returns>
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