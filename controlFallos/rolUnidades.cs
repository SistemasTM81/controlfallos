using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class rolUnidades : Form
    {
        new public menuPrincipal Owner;
        private int totalServices = 6;
        object driverID;
        protected internal delegate void delegater(object Loaders);
        Thread th;
        int? positionX = null, rolfkCRoles;
        public long? periodID, serviceID, ecoID;
        bool editar;
        public rolUnidades(menuPrincipal Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            LoadDataCrontrols(true, true, true, true, false, false, false);
        }
        public void LoadDataCrontrols(bool initialData, bool ECObusq, bool driversBusq, bool LoadGridView, bool LoadServices, bool LoadecoService, bool Loadroles)
        {
            th = new Thread(dataLoader);
            th.IsBackground = true;
            th.Start(new object[] { initialData, ECObusq, driversBusq, LoadGridView, LoadServices, LoadecoService, Loadroles });
        }
        /// <summary>
        /// Method that allow load the data of econnomicos, servicios etc.
        /// </summary>
        /// <param name="Loaders">Set of Boolean values that indicate if load the data of each control</param>
        private void dataLoader(object Loaders)
        {

            if (InvokeRequired)
                Invoke(new delegater(dataLoader), Loaders);
            else
            {
                bool[] Loader = Array.ConvertAll((Loaders as object[]), b => (bool)b);
                if (Loader[0]) { Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiod, new string[] { "DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --", 1); Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiodBusq, new string[] { "DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --", 1); Owner.v.inimeses(cmbxmonthBusq); dtpFechaDe.MinDate = dtpFechaA.MinDate = Convert.ToDateTime(Owner.v.getaData("SELECT COALESCE(min(initialDate),CURDATE()) FROM roltimeperiod")); }
                if (Loader[1]) Owner.v.iniCombos("SELECT * FROM getECOS ", cmbxEcoBusq, "idunidad", "eco", "--SELECCIONE ECONÓMICO--", this);
                if (Loader[2]) Owner.v.iniCombos("CALL getPersonalActivo(1,1)", cmbxdriverBusq, "idpersona", "nombres", "--SELECCIONE CONDUCTOR--", this);
                if (Loader[3]) { initialLoad(); /**dtpFechaDe.MinDate = dtpFechaA.MinDate = Convert.ToDateTime(Owner.v.getaData("SELECT COALESCE(MIN(initialDate),CURDATE()) FROM roltimeperiod")); dtpFechaDe.MaxDate = dtpFechaA.MaxDate = Convert.ToDateTime(Owner.v.getaData("SELECT COALESCE(MAX(initialDate),CURDATE()) FROM roltimeperiod"));*/ }
                if (Loader[4])
                {
                    DataTable dt = (DataTable)Owner.v.getData("SELECT * FROM rolServicesActive");
                    for (int i = 0; i < dt.Rows.Count; i++)
                        createServices(dt.Rows[i].ItemArray[0], dt.Rows[i].ItemArray[1], dt.Rows.Count <= totalServices, (dt.Rows.Count <= totalServices ? (int?)dt.Rows.Count : null), (dt.Rows.Count <= totalServices ? (int?)i : null));
                }
                if (Loader[5])
                {
                        Owner.v.iniCombos("CALL getECOS('" + rolfkCRoles + "')", cmbxEco, "idunidad", "eco", "--SELECCIONE ECONÓMICO--", this);
                    Owner.v.iniCombos("select * from jouurneyscbx", cmbxjourney, "id", "name", "--SELECCIONE JORNADA--", this);
                }
                if (Loader[6])
                {
                    dgvcycles.DataSource = null;
                    if (serviceID.HasValue)
                    {
                        DataTable dt = new DataTable("Rol De Servicio");
                        DataColumn dc = new DataColumn("CICLO");
                        dc.AutoIncrementSeed = 1;
                        dc.AutoIncrement = true;
                        dc.Unique = true;
                        dt.Columns.Add(dc);

                        var dtECOS = Owner.v.getaData("call sistrefaccmant.getECOSCount(" + serviceID + ");").ToString().Split('¬');
                        foreach (string row in dtECOS)
                        {
                            DataTable dt2 = new DataTable();
                            var eco = row.Split('|');
                            dt2.Columns.Add(eco[0]);
                            DataColumn columnhour = new DataColumn();
                            dt2.Columns.Add(eco[1] + "|" + eco[0]);
                            dt2.Columns.Add(eco[2]);
                            var drows = Owner.v.getaData("call sistrefaccmant.getAllcycles(" + eco[0] + ");").ToString().Split('¬');
                            foreach (var rowrol in drows)
                                dt2.Rows.Add(rowrol.Split('|'));
                            dt = Owner.v.JoinDataTables(dt, dt2);
                        }
                        dgvcycles.DataSource = dt;
                        foreach (DataGridViewColumn column in dgvcycles.Columns) { if (column.HeaderText.Contains("HORA")) { column.Name = column.HeaderText.Replace("|", string.Empty); column.HeaderText = "HORA"; } }
                        for (int i = 1; i < dgvcycles.Columns.Count; i += 3) { try { dgvcycles.Columns[i].Visible = false; } catch { } }
                    }
                }
                th.Abort();
            }
        }
        /// <summary>
        /// Method thats allow get the workdays and incruste in the datasource from the datagridview
        /// </summary>
        private void initialLoad()
        {
            dgvroles.DataSource = Owner.v.getData("CALL sistrefaccmant.getAllRoles();");
            if (dgvroles.Columns.Count > 0) dgvroles.Columns[0].Visible = false;
        }

        private string causesValidation() => (cmbxEco.SelectedIndex > 0 ? (cmbxjourney.SelectedIndex > 0 ? (driverID != null ? string.Empty : "Seleccione Conductor") : "Seleccione Jornada De La Lista Deplegable") : "Seleccione Económico de la lista desplegable");

        private void dgvcycles_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var distinctNames = (from row in (dgvcycles.DataSource as DataTable).AsEnumerable() select row.Field < string > (""));

        }

        /// <summary>
        /// Method that allow to create the dynamic buttons for the services registered
        /// </summary>
        /// <param name="idService">ID from the database, it's going to be in the button's name</param>
        /// <param name="ServiceName">Service's name, it's going to be in the Label's Text</param>
        /// <param name="x">This a boolean valor that allow to know if the position in x can be defined by a formula or by a incremental valor</param>
        void createServices(object idService, object ServiceNamex, bool x, int? rowsCount, int? actualRow)
        {
            Panel backgroundP = new Panel();
            Button button = new Button();
            Label lbl = new Label();
            backgroundP.Name = "panel" + idService;
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
            lbl.Name = "Label" + idService;
            lbl.Font = new Font(new FontFamily("Garamond"), 9);
            lbl.Text = ServiceNamex.ToString();
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.AutoSize = false;
            lbl.Dock = DockStyle.Bottom;
            backgroundP.AutoSize = true;
            backgroundP.Controls.Add(button);
            backgroundP.Controls.Add(lbl);
            backgroundpanel.Controls.Add(backgroundP);
            var res = x ? ((((backgroundpanel.Width - backgroundP.Width) / (rowsCount.Value + 1)) * (actualRow.Value + 1)) - (backgroundP.Width) / (rowsCount.Value + 1)) : (int)(positionX.HasValue ? (positionX = positionX.Value + 250) : (positionX = 100));
            backgroundP.Location = new Point(res, 0);
        }
        
        void disableService(object id)
        {
            foreach (Control c in backgroundpanel.Controls)
            {
                if (c.GetType() == typeof(Panel))
                    c.Enabled = !c.Name.Equals("panel" + id);
            }
        }
        /// <summary>
        /// Method that clear the part of adding roles
        /// </summary>
        void cloarControls()
        {
            if (cmbxEco.DataSource != null)
                cmbxEco.SelectedIndex = cmbxjourney.SelectedIndex = 0;
            driverID = null;
            lbldriverSelected.Text = "No Seleccionado";
            lbldriverSelected.TextAlign = ContentAlignment.MiddleLeft;
            lblinitialhour.Visible = dtpinitialhour.Visible = false;
        }
    }
}