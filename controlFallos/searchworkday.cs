using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class searchworkday : Form
    {
        Thread th;
        new workdays Owner;
        delegate void loaderdata();
        public searchworkday(workdays Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
            Owner.Owner.v.creatItemsPersonalizadosCombobox(cmbxTimeperiodBusq, new string[] { "DÍA", "SEMANA", "QUINCENA" }, "-- SELECCIONE RANGO --", 1); Owner.Owner.v.inimeses(cmbxmonthBusq);
            Owner.Owner.v.inimeses(cmbxmonthBusq);
            LoadData();
        }
        protected virtual void LoadData()
        {
            th = new Thread(LoaderData);
            th.IsBackground = true;
            th.Start();
        }
        private void LoaderData()
        {
            if (InvokeRequired)
                Invoke(new loaderdata(LoaderData));
            else
            {
                dgvroles.Rows.Clear();
                dgvroles.DataSource = Owner.Owner.v.getData("CALL sistrefaccmant.getAllRoles();");
                if (dgvroles.Columns.Count > 0) dgvroles.Columns[0].Visible = false;
                dgvroles.ClearSelection();
                th.Abort();
            }
        }

        private void searchworkday_Load(object sender, System.EventArgs e)
        {

        }
    }
}
