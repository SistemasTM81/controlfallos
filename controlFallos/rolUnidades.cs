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
    public partial class rolUnidades : Form
    {
        new public menuPrincipal Owner;
        private  int totalServices = 6;
        protected internal delegate void delegater(object Loaders);
        Thread th;
        int? positionX=null;
        private void button9_Click(object sender, EventArgs e)
        {
            viewDriver view = new viewDriver(this);
            view.Owner = this;
            view.ShowDialog();
        }

        public rolUnidades(menuPrincipal Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            cmbxeco.DrawItem += Owner.v.combos_DrawItem;
            cmbxTimeperiod.DrawItem += Owner.v.combos_DrawItem;
           th = new Thread(dataLoader);
            th.IsBackground = true;
            th.Start(new object[] {true,true});
            th.Join();
            Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiod, new string[] {"DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --");
        }
        /// <summary>
        /// Method that allow load the data of econnomicos, servicios etc.
        /// </summary>
        /// <param name="Loaders">Set of Boolean values that indicate if load the data of each control</param>
        private void dataLoader(object Loaders)
        {
            
            if (InvokeRequired)
                Invoke(new delegater(dataLoader),Loaders);
            else
            {
                object[] Loader = Loaders as object[];
                if (Convert.ToBoolean(Loader[0]))
                    Owner.v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1"/** AND t2.idarea='" + cbxgetarea.SelectedValue + "'"*/, cmbxeco, "idunidad", "eco", "--SELECCIONE ECONÓMICO--", this);
                if(Convert.ToBoolean(Loader[1]))
                {
                    DataTable dt = (DataTable)Owner.v.getData("SELECT idrol,UPPER(CONCAT(Nombre,'(',Descripcion,')')) as servicio FROM croles as t1 INNER JOIN cservicios as t2 On t1.serviciofkcservicios = t2.idservicio WHERE t2.status='1';");

                    for (int i = 0; i < dt.Rows.Count; i++)
                        createServices(dt.Rows[i].ItemArray[0], dt.Rows[i].ItemArray[1],dt.Rows.Count<=totalServices, (dt.Rows.Count <= totalServices? (int?)dt.Rows.Count :null), (dt.Rows.Count <= totalServices? (int?)i : null));
                }
                th.Abort();
            }
        }
        /// <summary>
        ///Method that allow get the perod of time that will last the role
        ///sender ==1 => 1 day
        ///sender ==2 => 5 days
        ///sender == 4 => 13 days
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbxTimeperiod_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbxTimeperiod.SelectedIndex <= 0) { lblfinaldate.Text = string.Empty; return; }
            lblfinaldate.Text = (cmbxTimeperiod.SelectedIndex == 1 ? dtpinitialDate.Value.ToString("dd/MMMM/yyyy"):(cmbxTimeperiod.SelectedIndex == 2 ? dtpinitialDate.Value.AddDays(5).ToString("dd/MMMM/yyyy") : dtpinitialDate.Value.AddDays(13).ToString("dd/MMMM/yyyy")) );
        }

        /// <summary>
        /// Method that allow to create the dynamic buttons for the services registered
        /// </summary>
        /// <param name="idService">ID from the database, it's going to be in the button's name</param>
        /// <param name="ServiceName">Service's name, it's going to be in the Label's Text</param>
        /// <param name="x">This a boolean valor that allow to know if the position in x can be defined by a formula or by a incremental valor</param>
        void createServices(object idService, object ServiceNamex, bool x, int? rowsCount,int? actualRow)
        {
            Panel backgroundP = new Panel();
            Button button = new Button();
            Label lbl = new Label();
            backgroundP.Name = "panel" + idService + (ServiceNamex.ToString().Replace(" ", string.Empty));
            button.Name = "button" + (ServiceNamex.ToString().Replace(" ", string.Empty)) + "|" + idService;
            button.Image = Properties.Resources.mexibus;
            button.Dock = DockStyle.Top;
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 200, 200);
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            button.AutoSize = true;
            button.UseMnemonic = false;
            button.FlatStyle = FlatStyle.Flat;
            button.ImageAlign = ContentAlignment.MiddleCenter;
            button.Click += Services_OnClick;
            lbl.UseMnemonic = false;
            lbl.Name = "Label" + idService + (ServiceNamex.ToString().Replace(" ", string.Empty));
            lbl.Font = new Font(new FontFamily("Garamond"), 9);
            lbl.Text = ServiceNamex.ToString();
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.AutoSize = false;
            lbl.Dock = DockStyle.Bottom;
            backgroundP.AutoSize = true;
            backgroundP.Controls.Add(button);
            backgroundP.Controls.Add(lbl);
            backgroundpanel.Controls.Add(backgroundP);
            var res = x? ((((backgroundpanel.Width - backgroundP.Width)/ (rowsCount.Value+1)) * (actualRow.Value+1))-(backgroundP.Width) / (rowsCount.Value + 1))   : (int)(positionX.HasValue ? (positionX = positionX.Value + 250) : (positionX = 100));
            backgroundP.Location = new Point(res,0);
        }

        private void Services_OnClick(object sender, EventArgs e)
        {
            MessageBox.Show("ID: "+(sender as Button).Name.Split('|')[1]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            catjourneys cat = new catjourneys(this);
            cat.Owner = this;
            cat.ShowDialog();
        }
    }
}