using System;
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
        int? positionX = null,rolfkCRoles;
        long? periodID;
        bool editar;
        public rolUnidades(menuPrincipal Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            LoadDataCrontrols(true, true, true, true,false,false);
        }
        private void LoadDataCrontrols(bool initialData, bool ECObusq, bool driversBusq, bool LoadGridView,bool LoadServices,bool LoadecoService)
        {
            th = new Thread(dataLoader);
            th.IsBackground = true;
            th.Start(new object[] { initialData, ECObusq, driversBusq, LoadGridView,LoadServices,LoadecoService});
            //    th.Join();
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
                
                if (Loader[0]) { Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiod, new string[] { "DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --"); Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiodBusq, new string[] { "DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --"); Owner.v.inimeses(cmbxmonthBusq); dtpFechaDe.MinDate = dtpFechaA.MinDate = Convert.ToDateTime(Owner.v.getaData("SELECT COALESCE(min(initialDate),CURDATE()) FROM roltimeperiod")); }
                if (Loader[1]) Owner.v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea ", cmbxEcoBusq, "idunidad", "eco", "--SELECCIONE ECONÓMICO--", this);
                if (Loader[2]) Owner.v.iniCombos("SELECT idpersona, UPPER(CONCAT(nombres,' ',COALESCE(apPaterno,' '),' ',COALESCE(apMaterno,' '))) as nombres FROM cpersonal WHERE status ='1' AND empresa='1' AND area='1' ORDER BY nombres,apPaterno,apMaterno ASC", cmbxdriverBusq, "idpersona", "nombres", "--SELECCIONE CONDUCTOR--", this);
                if (Loader[3])/*Carga de datos en DataGridView*/ dtpFechaDe.MaxDate = dtpFechaA.MaxDate = Convert.ToDateTime(Owner.v.getaData("SELECT COALESCE(MAX(initialDate),CURDATE()) FROM roltimeperiod")); ;
                if (Loader[4])
                {
                    DataTable dt = (DataTable)Owner.v.getData("SELECT idrol,UPPER(CONCAT(Nombre,'(',Descripcion,')')) as servicio FROM croles as t1 INNER JOIN cservicios as t2 On t1.serviciofkcservicios = t2.idservicio WHERE t2.status='1';");
                    for (int i = 0; i < dt.Rows.Count; i++)
                        createServices(dt.Rows[i].ItemArray[0], dt.Rows[i].ItemArray[1], dt.Rows.Count <= totalServices, (dt.Rows.Count <= totalServices ? (int?)dt.Rows.Count : null), (dt.Rows.Count <= totalServices ? (int?)i : null));
                }
                if (Loader[5])
                {
                    Owner.v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1 AND t2.idarea= (SELECT Areafkcareas FROM cservicios WHERE idservicio=(SELECT serviciofkcservicios FROM croles WHERE idrol='" + rolfkCRoles + "'))", cmbxEco, "idunidad", "eco", "--SELECCIONE ECONÓMICO--", this);
                    Owner.v.iniCombos("SELECT journeyID AS id, UPPER(journeyname) as name FROM cjourneys WHERE status=1 ORDER BY journeyname ASC;", cmbxjourney,"id","name","--SELECCIONE JORNADA--",this);
                }
                th.Abort();
            }
        }
        private string causesValidation() => (cmbxEco.SelectedIndex > 0 ? (cmbxjourney.SelectedIndex > 0 ? (driverID != null ? string.Empty : "Seleccione Conductor") :"Seleccione Jornada De La Lista Deplegable"):"Seleccione Económico de la lista desplegable");
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

        private void Services_OnClick(object sender, EventArgs e)
        {
            rolfkCRoles = Convert.ToInt32((sender as Button).Name.Split('|')[1]);
            gbxCiclo.Text = "Agregar Ciclo Para Servicio: "+backgroundpanel.Controls.Find("Label" + rolfkCRoles, true)[0].Text;
            pRoles.Visible = true;
            cloarControls();
            LoadDataCrontrols(false, false, false, false, false, true);
        }
        void cloarControls()
        {
            if (cmbxEco.DataSource != null)
                cmbxEco.SelectedIndex = cmbxjourney.SelectedIndex = 0;
             driverID = null;
            lbldriverSelected.Text = "No Seleccionado";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            catjourneys cat = new catjourneys(this);
            cat.Owner = this;
            cat.ShowDialog();
        }
    }
}