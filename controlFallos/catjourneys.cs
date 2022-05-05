using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
namespace controlFallos
{
    public partial class catjourneys : Form
    {
        bool editar, activo;
        object journeyIDTemp;
        Thread th;
        string _journeyName, _journeyDuration;
        new rolUnidades Owner;
        public bool AlreadyshowsMessage { get; private set; }
        delegate void loaderdata();
        public catjourneys(rolUnidades Owner)
        {
            this.Owner = Owner;
            InitializeComponent();
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
                dgvjorneys.Rows.Clear();
                DataTable dt = (DataTable)Owner.Owner.v.getData("SELECT journeyID, UPPER(journeyname), TIME_FORMAT(duration,'%H:%i'),(SELECT UPPER(CONCAT(coalesce(nombres,''),' ',coalesce(apPaterno,''),' ',coalesce(apMaterno,''))) FROM cpersonal WHERE idpersona= userfkcpersonal),if(status=1,'ACTIVO','INACTIVO') FROM cjourneys ");
                foreach (DataRow row in dt.Rows)
                    dgvjorneys.Rows.Add(row.ItemArray);
                dgvjorneys.ClearSelection();
                th.Abort();
            }
        }
        #region Validations
        bool getCambios() => (activo && (!Owner.Owner.v.mayusculas(txtjourneyname.Text.ToLower()).Equals(_journeyName) || !Owner.Owner.v.mayusculas(txtjourneyduration.Text.ToLower()).Equals(_journeyDuration)));
        /// <summary>
        /// function that return if exists some error as null text, if exists the workday name in the database or the duration has the pattern indicate (HH:mm) 
        /// </summary>
        /// <returns>Some Error </returns>
        object causesValidation() => (!string.IsNullOrWhiteSpace(txtjourneyname.Text.Trim()) ? (Convert.ToInt64(Owner.Owner.v.getaData("SELECT COUNT(*) FROM cjourneys WHERE journeyname='" + Owner.Owner.v.mayusculas(txtjourneyname.Text.ToLower()) + "' " + (editar ? "AND journeyID!=" + journeyIDTemp.ToString() : ""))) > 0 ? "Error: El Nombre ya existe" : (!string.IsNullOrWhiteSpace(txtjourneyduration.Text.Trim()) ? (new Regex(@"^([01]\d?|2[0-4]):[0-5]\d(:[0-5]\d)?$").IsMatch(txtjourneyduration.Text) ? null : "La Duración de La Jornada No Tiene Formato Valido (HH:mm)") : "La Duración de La Jornada No Puede Estar Vacía")) : "El Nombre de Jornada No Puede Estar Vacío");
        /// <summary>
        /// function that return the UPDATE MySQL sentence from the workday catalogue
        /// </summary>
        /// <returns>Sentence for Update in database</returns>
        string getCambiosString()
        {
            StringBuilder cambios = new StringBuilder();
            if (editar)
            {
                if (!Owner.Owner.v.mayusculas(txtjourneyname.Text.ToLower()).Equals(_journeyName))
                    cambios.Append((!string.IsNullOrWhiteSpace(cambios.ToString()) ? ", " : "") + "journeyname='" + Owner.Owner.v.mayusculas(txtjourneyname.Text.Trim().ToLower()) + "'");
                if (!Owner.Owner.v.mayusculas(txtjourneyduration.Text.ToLower()).Equals(_journeyDuration))
                    cambios.Append((!string.IsNullOrWhiteSpace(cambios.ToString()) ? ", " : "") + "duration= TIME('" + txtjourneyduration.Text + "')");
            }
            return cambios.ToString();
        }
        /// <summary>
        /// Method that allow clean controls and variables after to perform an action corrrectly
        /// </summary>
        void clearControls()
        {
            journeyIDTemp = txtjourneyduration.Text = txtjourneyname.Text = null;
            btnsave.Visible = lblsave.Visible = !(AlreadyshowsMessage = editar = pcancel.Visible = pdelete.Visible = false);
            _journeyName = _journeyDuration = string.Empty;
            txtjourneyduration.TextChanged -= changeVisibility;
            txtjourneyname.TextChanged -= changeVisibility;
            btnsave.BackgroundImage = Properties.Resources.save;
        }
        private void LoadRecord(DataGridViewCellEventArgs e)
        {
            activo = dgvjorneys.Rows[e.RowIndex].Cells[4].Value.ToString().Equals("ACTIVO");
            /**if (Pdesactivar)
                    {*/
            btndelete.BackgroundImage = activo ? Properties.Resources.delete__4_ : Properties.Resources.up;
            lbldelete.Text = activo ? "Desactivar" : "Reactivar";
            pdelete.Visible = true;
            //}
            /**   if (Peditar)
               {*/
            journeyIDTemp = dgvjorneys.Rows[e.RowIndex].Cells[0].Value;
            txtjourneyname.Text = _journeyName = Owner.Owner.v.mayusculas(dgvjorneys.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
            txtjourneyduration.Text = _journeyDuration = dgvjorneys.Rows[e.RowIndex].Cells[2].Value.ToString();
            /**if (Pinsertar)*/
            pcancel.Visible = true;
            btnsave.BackgroundImage = Properties.Resources.pencil;
            gbjourney.Text = "Actualizar Jornada";
            txtjourneyduration.TextChanged += changeVisibility;
            txtjourneyname.TextChanged += changeVisibility;
            btnsave.Visible = lblsave.Visible = !(editar = true);
            if (!activo) Owner.Owner.sendUser(Owner.Owner.v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia);
            /**  }
              else
              {
                  MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
              }*/
        }
        #endregion
    }
}