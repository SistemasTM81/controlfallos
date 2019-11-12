using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using h = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catUnidades : Form
    {
        int idUsuario, empresaa, _area;
        String ecotemp = "";

        string ecoAnterior;
        string descAnterior;
        public int empresaAnterior, areaAnterior, servicioAnterior, modeloAnterior;
        int statusUnidad;
        bool yaAparecioMensaje = false;
        validaciones v = new validaciones();
        public bool editar = false;
        conexion c = new conexion();
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        public int EmpresaAnterior { get { return empresaAnterior; } set { empresaAnterior = value; } }
        void iniecos()
        {
            v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea order by concat(t2.identificador,LPAD(consecutivo,4,'0'))", cbeco, "idunidad", "eco", "--SELECCIONE ECONÓMICO--");
        }
    public void initializeModels()
        {
            v.iniCombos("SELECT idmodelo as id, modelo FROM cmodelos WHERE status=1 ORDER BY modelo ASC", cbxgetmodelo, "id", "modelo", "-- SELECCIONE MODELO --");
        }
        Thread exportar;
        public int ServicioAnterior
        {
            get
            {
                return servicioAnterior;
            }

            set
            {
                servicioAnterior = value;
            }
        }
        Thread th;
        public catUnidades(int idUsuario, int empresaa, int area)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.idUsuario = idUsuario;
            csetEmpresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbservicio.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbstatus.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbeco.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dataGridView1.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbareas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresaa = empresaa;
            this._area = area;
            cbstatus.DrawItem += new DrawItemEventHandler(v.comboBoxEstatus_DrwaItem);
            cbxgetmodelo.MouseWheel += v.paraComboBox_MouseWheel;
            cbxgetmodelo.DrawItem += v.combos_DrawItem;
        }
        public void privilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT CONCAT(insertar,' ',consultar,' ',editar, ' ',desactivar) FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split(' ');
            if (privilegiosTemp.Length > 0)
            {

                pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
            }
            mostrar();
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddunidad.Visible = true;
            }
            if (pconsultar)
            {
                gbUnidades.Visible = true;
                gbbuscar.Visible = true;
            }
            if (peditar)
            {
                label12.Visible = true;
                label10.Visible = true;
            }
        }
        public void iniareas()
        {
            if (csetEmpresa.SelectedIndex > 0)
            {
                String sql1 = "SELECT idarea, upper(nombreArea) as nombreArea FROM careas where empresafkcempresas='" + csetEmpresa.SelectedValue + "' ORDER BY nombreArea asc";
                MySqlCommand cmd = new MySqlCommand(sql1, c.dbconection());
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    bool temp = false;
                    for (int i = 0; i < Application.OpenForms.Count; i++)
                    {
                        if (Application.OpenForms[i].GetType() == typeof(catEmpresas)) { temp = true; break; }
                    }
                    if (!temp)
                    {
                        MessageBox.Show("No se encuentran áreas registradas con la empresa seleccionada", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cbareas.Enabled = false;
                        cbareas.DataSource = null;
                    }
                }
                else
                {
                    v.iniCombos("SELECT idarea, upper(nombreArea) as nombreArea FROM careas WHERE status ='1' and empresafkcempresas='" + csetEmpresa.SelectedValue + "' ORDER BY nombreArea asc", cbareas, "idarea", "nombreArea", "-- SELECCIONE UN ÁREA --");
                    cbareas.Enabled = true;
                }
            }
            else
            {
                cbareas.Enabled = false;
                cbareas.DataSource = null;
            }

        }
        public void bunidades()
        {
            dataGridView1.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT idunidad, UPPER(t3.nombreEmpresa),UPPER(t2.nombreArea), concat(t2.identificador,LPAD(consecutivo,4,'0')),t4.modelo,coalesce(UPPER((select if(t1.serviciofkcservicios = '1', 'SIN SERVICIO FIJO', (select CONCAT(t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad)))),'SIN SERVICIO FIJO') as servicio,UPPER(descripcioneco),if(t1.status=1,'ACTIVO','NO ACTIVO'),t2.empresafkcempresas,t1.areafkcareas,t1.serviciofkcservicios,t1.consecutivo,t1.modelofkcmodelos FROM cunidades as t1 INNER JOIN careas as t2 On t1.areafkcareas=t2.idarea INNER JOIN cempresas AS t3 ON t2.empresafkcempresas = t3.idempresa INNER JOIN cmodelos as t4 ON t1.modelofkcmodelos = t4.idmodelo ORDER BY t3.nombreEmpresa ASC, t2.nombreArea ASC,concat(t2.identificador,LPAD(consecutivo,4,'0')) DESC;");
            foreach (DataRow row in dt.Rows) dataGridView1.Rows.Add(row.ItemArray);

            dataGridView1.ClearSelection();
        }
        private void btnsaveu_Click(object sender, EventArgs e)
        {
            string eco = txtgeteco.Text.Trim().TrimStart('0');

            string desc = v.mayusculas(txtgetdesc.Text.Trim().ToLower());
            int empresa = Convert.ToInt32(csetEmpresa.SelectedValue);
            int servicio = Convert.ToInt32(cbservicio.SelectedValue);
            while (eco.Length < 4)
            {
                eco = "0" + eco;
            }
            object areaEmpresa = cbareas.SelectedValue;
            if (!editar)
            {
                if (!v.formularioUnidades(eco, empresa, Convert.ToInt32(areaEmpresa), cbservicio.SelectedIndex, cbxgetmodelo.SelectedIndex) && !v.yaExisteECO(areaEmpresa.ToString(), lblid.Text + eco))
                {
                    String sql;
                    if (cbservicio.SelectedIndex > 0)
                        sql = "INSERT INTO cunidades (consecutivo, descripcioneco, areafkcareas,serviciofkcservicios,usuariofkcpersonal,modelofkcmodelos) VALUES ('" + eco.Trim() + "'," + (!string.IsNullOrWhiteSpace(desc) ? "'" + desc + "'" : "NULL") + ",'" + areaEmpresa + "','" + cbservicio.SelectedValue + "','" + this.idUsuario + "','" + cbxgetmodelo.SelectedValue + "')";
                    else
                        sql = "INSERT INTO cunidades (consecutivo, descripcioneco, areafkcareas,usuariofkcpersonal,modelofkcmodelos) VALUES ('" + eco.Trim() + "'" + (!string.IsNullOrWhiteSpace(desc) ? "'" + desc + "'" : "NULL") + "'" + areaEmpresa + "','" + this.idUsuario + "','" + cbxgetmodelo.SelectedValue + "')";
                    if (v.c.insertar(sql))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Unidades',(SELECT idunidad FROM cunidades WHERE consecutivo='" + eco.Trim() + "' AND areafkcareas='" + areaEmpresa + "' and status=1 and serviciofkcservicios='" + cbservicio.SelectedValue + "' AND modelofkcmodelos= '" + cbxgetmodelo.SelectedValue + "'),'" + idUsuario + "',NOW(),'Inserción de Unidad','" + empresaa + "','" + _area + "')");
                        MessageBox.Show("La Unidad se Ha Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        esta_exportando();
                        limpiar();
                        bunidades();
                    }
                    else
                    {
                        MessageBox.Show("Unidad no Insertada");
                    }
                }
            }
            else
            {
                if (cambios())
                {
                    if (this.statusUnidad == 0)
                    {
                        MessageBox.Show("No se Puede Modificar Una Unidad Desactivada para el Sistema", "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        if (v.mayusculas(eco).Equals(ecoAnterior) && v.mayusculas(desc).Equals(descAnterior) && Convert.ToInt32(csetEmpresa.SelectedValue) == EmpresaAnterior && Convert.ToInt32(cbareas.SelectedValue) == areaAnterior && Convert.ToInt32(cbservicio.SelectedValue) == ServicioAnterior)
                        {
                            MessageBox.Show("No se Relizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                esta_exportando();
                                limpiar();
                            }
                        }
                        else
                        {
                            if (!v.formularioUnidades(eco, empresa, Convert.ToInt32(cbareas.SelectedValue), cbservicio.SelectedIndex, cbxgetmodelo.SelectedIndex) && !v.yaExisteECOActualizar(areaEmpresa.ToString(), areaAnterior.ToString(), areaAnterior + eco, lblid.Text + this.ecoAnterior))
                            {
                                try
                                {
                                    observacionesEdicion obs = new observacionesEdicion(v);
                                    obs.Owner = this;
                                    if (obs.ShowDialog() == DialogResult.OK)
                                    {
                                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                        string sql = "UPDATE cunidades SET consecutivo = '" + eco + "', descripcioneco = '" + v.mayusculas(desc) + "', areafkcareas ='" + cbareas.SelectedValue + "', serviciofkcservicios='" + cbservicio.SelectedValue.ToString() + "', modelofkcmodelos='" + cbxgetmodelo.SelectedValue + "'  WHERE idUnidad= '" + ecotemp + "'";
                                        if (v.c.insertar(sql))
                                        {
                                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Unidades','" + ecotemp + "','" + ecoAnterior + ";" + areaAnterior + ";" + descAnterior + ";" + ServicioAnterior + ";" + modeloAnterior + "','" + idUsuario + "',NOW(),'Actualización de Unidad','" + observaciones + "','" + empresaa + "','" + this._area + "')");
                                            if (!yaAparecioMensaje)
                                            {
                                                MessageBox.Show("La Unidad se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            esta_exportando();
                                            limpiar();
                                            bunidades();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Unidad no actualizada");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            }
                        }
                    }
                }
            }
        }
        public void limpiar()
        {
            if (pinsertar)
            {
                lblsaveu.Text = "Guardar";
                btnsaveu.BackgroundImage = controlFallos.Properties.Resources.save;
                gbaddunidad.Text = "Agregar Unidad";
                editar = false;
                cbempresa.Focus();
            }
            esta_exportando();

            iniecos();
            csetEmpresa.SelectedIndex = 0;
            lblid.Text = "";
            pcancelu.Visible = false;
            ecotemp = null;
            EmpresaAnterior = 0;
            ServicioAnterior = 0;
            areaAnterior = 0;
            modeloAnterior = cbxgetmodelo.SelectedIndex = 0;
            peliminar.Visible = false;
            txtgeteco.Clear();
            txtgetdesc.Clear(); yaAparecioMensaje = false;
            txtgeteco.Visible = false;
            lblgeteco.Visible = false;
            btnsaveu.Visible = true;
            lblsaveu.Visible = true;
            cbareas.DataSource = null;
            pActualizar.Visible = false;
            busqempresas();
            busqempresasBusq();
            _servicios();

        }
        private void catUnidadess_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {

                bunidades();
                iniecos();
            }
            busqempresas();
            busqempresasBusq();
            if (pinsertar || peditar) initializeModels();
            Estatus();
            catalogos();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(SplashScreen))
                {
                    if (frm.InvokeRequired)
                    {

                        validaciones.delgado dm = new validaciones.delgado(v.cerrarForm);

                        Invoke(dm, frm);
                    }

                    break;
                }
            }
            th.Abort();
        }
        void catalogos()
        {
            var consultaPrivilegios = "SELECT namForm FROM privilegios WHERE usuariofkcpersonal= '" + this.idUsuario + "' and ver =1 and (namForm= 'catEmpresas' OR namForm ='catServicios' OR namForm ='catAreas')";
            MySqlCommand cm = new MySqlCommand(consultaPrivilegios, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                PrivilegiosVisibles(dr.GetString("namForm").ToLower());
            }
            dr.Close();
            c.dbconection().Close();
        }
        void PrivilegiosVisibles(string nameform)
        {
            if (nameform == "catempresas")
            {
                pEmpresas.Visible = true;
            }
            if (nameform == "catservicios")
            {
                pServicios.Visible = true;
            }
            if (nameform == "catareas")
            {
                pAreas.Visible = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int stat;
                if (statusUnidad == 1)
                {
                    msg = "Des";
                    stat = 0;
                }
                else
                {
                    msg = "Re";
                    stat = 1;
                }
                if (statusUnidad == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cempresas WHERE idempresa=(SELECT empresafkcempresas FROM careas WHERE idarea=(SELECT areafkcareas FROM cunidades WHERE idunidad='" + ecotemp + "'))")) == 0)
                {
                    MessageBox.Show("Error Al Reactivar La Unidad:\nEmpresa Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (statusUnidad == 0 && Convert.ToInt32(v.getaData("SELECT status FROM careas WHERE idarea=(SELECT areafkcareas from cunidades WHERE idunidad='" + ecotemp + "')")) == 0)
                {
                    MessageBox.Show("Error Al Reactivar La Unidad:\nArea Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (statusUnidad == 0 && ServicioAnterior > 1 && Convert.ToInt32(v.getaData("SELECT status FROM cservicios WHERE idservicio=(SELECT serviciofkcservicios FROM cunidades WHERE idunidad='" + ecotemp + "')")) == 0)
                {
                    MessageBox.Show("Error Al Reactivar La Unidad:\nServicio Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Económico";
                    obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 5, obs.lblinfo.Location.Y);
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                        String sql = "UPDATE cunidades SET status = " + stat + " WHERE idUnidad  = " + this.ecotemp;
                        if (v.c.insertar(sql))
                        {
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Unidades','" + ecotemp + "','" + msg + "activación de Unidad','" + idUsuario + "',NOW(),'" + msg + "activación de Unidad','" + edicion + "','" + empresaa + "','" + _area + "')");
                            MessageBox.Show("La Unidad se ha " + msg + "activado Existosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            esta_exportando();
                            limpiar();
                            bunidades();
                        }
                        else
                        {
                            MessageBox.Show("hubo un Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancelu_Click(object sender, EventArgs e)
        {
            if (cambios())
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsaveu_Click(null, e);
                }
                else
                {
                    esta_exportando();
                    limpiar();
                }
            }
            else
            {
                esta_exportando();
                limpiar();
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string eco = txtgeteco.Text.Trim().TrimStart('0');
                string desc = v.mayusculas(txtgetdesc.Text.ToLower());
                if (cambios())
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsaveu_Click(null, e);
                        guardarReporte(e);
                    }
                    else
                    {
                        guardarReporte(e);
                    }
                }
                else
                {
                    guardarReporte(e);
                }
            }
        }
        string U_eco;
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            limpiar();
            try
            {
                if (pdesactivar)
                {
                    this.ecotemp = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    statusUnidad = v.getStatusInt(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());

                    if (statusUnidad == 0)
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelete.Text = "Reactivar";
                    }
                    else
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar";
                    }
                    peliminar.Visible = true;

                }
                if (peditar)
                {
                    try
                    {
                        statusUnidad = v.getStatusInt(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                        this.ecotemp = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        this.U_eco = v.getaData("Select consecutivo from cunidades where idunidad='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'").ToString();
                        csetEmpresa.SelectedValue = EmpresaAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
                        cbservicio.Enabled = true;

                        if (csetEmpresa.SelectedIndex == -1)
                        {
                            v.iniCombos("SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE (status = '1' OR idempresa='" + EmpresaAnterior + "') AND empresa='" + empresaa + "' AND area='" + _area + "' ORDER BY nombreEmpresa ASC", csetEmpresa, "idempresa", "nombreEmpresa", "-- SELECCIONE EMPRESA --");
                            csetEmpresa.SelectedValue = EmpresaAnterior;
                        }
                        cbareas.SelectedValue = areaAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
                        if (cbareas.SelectedIndex == -1)
                        {
                            v.iniCombos("SELECT idarea, upper(nombreArea) as nombreArea FROM careas WHERE (status ='1' OR idarea='" + areaAnterior + "') and empresafkcempresas='" + csetEmpresa.SelectedValue + "' ORDER BY nombreArea asc", cbareas, "idarea", "nombreArea", "-- SELECCIONE UN ÁREA --");
                            cbareas.SelectedValue = areaAnterior;
                        }
                        txtgeteco.Text = this.ecoAnterior = U_eco;
                        txtgetdesc.Text = descAnterior = v.mayusculas(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString().ToLower());


                        cbservicio.SelectedValue = ServicioAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
                        if (csetEmpresa.DataSource != null && csetEmpresa.SelectedIndex > 0)
                        {
                            cbservicio.SelectedValue = ServicioAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());
                            if (cbservicio.SelectedIndex == -1)
                            {
                                _serviciosDesactivados();
                                cbservicio.SelectedValue = ServicioAnterior;
                            }
                        }
                        cbxgetmodelo.SelectedValue = modeloAnterior = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
                        btnsaveu.Visible = false;
                        lblsaveu.Visible = false;
                        pcancelu.Visible = true;
                        btnsaveu.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        gbaddunidad.Text = "Actualizar Unidad: " + dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                        lblsaveu.Text = "Guardar";
                        editar = true;
                        dataGridView1.ClearSelection();
                        if (statusUnidad == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source + "\n" + ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Usted no tiene los privilegios para Modificar Unidades", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void busqempresas()
        {
            v.iniCombos("SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE status = '1' AND empresa='" + empresaa + "' AND area='" + _area + "' ORDER BY nombreEmpresa ASC", csetEmpresa, "idempresa", "nombreEmpresa", "-- SELECCIONE EMPRESA --");
            csetEmpresa.SelectedIndex = 0;
        }
        public void busqempresasBusq()
        {
            v.iniCombos("SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE  empresa='" + empresaa + "' AND area='" + _area + "' ORDER BY nombreEmpresa ASC", cbempresa, "idempresa", "nombreEmpresa", "-- SELECCIONE EMPRESA --");
            cbempresa.SelectedIndex = 0;
        }
        private void txtgeteco_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtgetdesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13)
                v.enGeneral(e);
            else
            {
                if (cambios())
                    btnsaveu_Click(null, e);
                else
                    e.Handled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (cbempresa.SelectedIndex > 0 || cbeco.SelectedIndex > 0 || cbstatus.SelectedIndex > 0)
            {
                try
                {
                    string wheres = "";
                    String sql = @"SELECT idunidad, UPPER(t3.nombreEmpresa),UPPER(t2.nombreArea), concat(t2.identificador,LPAD(consecutivo,4,'0')),t4.modelo,coalesce(UPPER((select if(t1.serviciofkcservicios = '1', 'SIN SERVICIO FIJO', (select CONCAT(t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad)))),'SIN SERVICIO FIJO') as servicio,UPPER(descripcioneco),if(t1.status=1,'ACTIVO','NO ACTIVO'),t2.empresafkcempresas,t1.areafkcareas,t1.serviciofkcservicios,t1.consecutivo,t1.modelofkcmodelos FROM cunidades as t1 INNER JOIN careas as t2 On t1.areafkcareas=t2.idarea INNER JOIN cempresas AS t3 ON t2.empresafkcempresas = t3.idempresa INNER JOIN cmodelos as t4 ON t1.modelofkcmodelos = t4.idmodelo WHERE ";
                    if (Convert.ToInt32(cbeco.SelectedValue.ToString()) > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = "t1.idUnidad = '" + cbeco.SelectedValue + "' ";
                        }
                        else
                        {
                            wheres += " AND t1.idUnidad = '" + cbeco.SelectedValue + "' ";
                        }
                    }
                    if (Convert.ToInt32(cbempresa.SelectedValue) > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = "empresafkcempresas = '" + cbempresa.SelectedValue.ToString() + "'";
                        }
                        else
                        {
                            wheres += "AND empresafkcempresas = '" + cbempresa.SelectedValue.ToString() + "'";
                        }
                    }
                    if (cbstatus.SelectedIndex > 0)
                    {
                        if (wheres == "")
                            wheres = " t1.status = " + v.statusinv(cbstatus.SelectedIndex - 1) + " ORDER BY t3.nombreEmpresa ASC, t2.nombreArea ASC ,concat(t2.identificador,LPAD(consecutivo,4,'0')) DESC";
                        else
                            wheres += "and  t1.status = " + v.statusinv(cbstatus.SelectedIndex - 1) + " ORDER BY t3.nombreEmpresa ASC , t2.nombreArea ASC,concat(t2.identificador,LPAD(consecutivo,4,'0')) DESC";
                    }
                    sql += wheres;
                    cbeco.SelectedIndex = 0;
                    cbempresa.SelectedIndex = 0;
                    cbstatus.SelectedIndex = 0;
                    dataGridView1.Rows.Clear();
                    cbstatus.SelectedIndex = 0;
                    DataTable dt = (DataTable)v.getData(sql);
                    if (dt.Rows.Count == 0)
                    {
                        esta_exportando();
                        MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        bunidades();
                        pActualizar.Visible = false;
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows) dataGridView1.Rows.Add(row.ItemArray);
                        dataGridView1.ClearSelection();
                        pActualizar.Visible = true;
                        if (!est_expor)
                            btnExcel.Visible = true;
                        LblExcel.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("La Búsqueda Debe Contener Por Lo Menos Un Filtro", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Estatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idnivel");
            dt.Columns.Add("Nombre");

            DataRow row = dt.NewRow();
            row["idnivel"] = 0;
            row["Nombre"] = "--SELECCIONE ESTATUS--".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 1;
            row["Nombre"] = "activo".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 2;
            row["Nombre"] = "inactivo".ToUpper();
            dt.Rows.Add(row);
            cbstatus.ValueMember = "idnivel".ToUpper();
            cbstatus.DisplayMember = "Nombre";
            cbstatus.DataSource = dt;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void gbaddunidad_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            catEmpresas cat = new catEmpresas(idUsuario, empresaa, _area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            catServicios cat = new catServicios(idUsuario, empresaa, _area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            catAreas cat = new catAreas(idUsuario, empresaa, _area);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void cbservicio_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtgeteco_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetdesc_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (cambios())
                {
                    btnsaveu.Visible = true;
                    lblsaveu.Visible = true;
                }
                else
                {
                    btnsaveu.Visible = false;
                    lblsaveu.Visible = false;
                }
            }
        }
        bool cambios()
        {
            int areaActual = 0;
            if (cbareas.DataSource != null)
                areaActual = Convert.ToInt32(cbareas.SelectedValue);
            return (statusUnidad == 1 && ((modeloAnterior != Convert.ToInt32(cbxgetmodelo.SelectedValue) || areaAnterior != areaActual || (csetEmpresa.SelectedIndex > 0 && empresaAnterior != (int)csetEmpresa.SelectedValue) || (cbservicio.SelectedIndex > 0 && ServicioAnterior != (int)cbservicio.SelectedValue) || U_eco != txtgeteco.Text.Trim() || descAnterior != v.mayusculas(txtgetdesc.Text.Trim().ToLower()))) && (!string.IsNullOrWhiteSpace(txtgeteco.Text) && cbareas.SelectedIndex > 0));
        }
        void Selecciona_unidades()
        {
            if (cbempresa.SelectedIndex > 0)
            {
                string sql = "SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where upper(nombreEmpresa)='" + cbempresa.Text + "';";
                MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
                {
                    cbeco.DataSource = null;
                    DataTable dt = (DataTable)v.getData("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where upper(nombreEmpresa)='" + cbempresa.Text + "';");
                    DataRow nuevaFila = dt.NewRow();
                    nuevaFila["idunidad"] = 0;
                    nuevaFila["eco"] = "--SELECCIONE ECONÓMICO--".ToUpper();
                    dt.Rows.InsertAt(nuevaFila, 0);
                    cbeco.DisplayMember = "eco";
                    cbeco.ValueMember = "idunidad";
                    cbeco.DataSource = dt;
                    cbeco.Enabled = true;
                }
                else
                {
                    MessageBox.Show("La empresa seleccionada no cuenta con unidades registradas en el sistema", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbempresa.SelectedIndex = 0;
                }
            }
            else
            {
                iniecos();
            }
        }
        private void cbempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selecciona_unidades();
        }

        private void gbaddunidad_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        bool exportando = false;
        void esta_exportando()
        {
            if (!LblExcel.Text.Equals("Exportando"))
                btnExcel.Visible = LblExcel.Visible = false;
            else
                exportando = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            bunidades();
            pActualizar.Visible = false;
            esta_exportando();
            cbempresa.SelectedIndex = 0;
            cbstatus.SelectedIndex = 0;

        }
        delegate void El_Delegado();
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "Exportando";
        }
        delegate void El_Delegado1();
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            if (exportando)
            {
                LblExcel.Visible = false;
                btnExcel.Visible = false;
            }
            exportando = false;
            est_expor = false;
            LblExcel.Text = "Exportar";
        }
        void _UnidadesExportadas()
        {
            string id;
            int contador = 0;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Unidades','0','";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                contador++;
                id = row.Cells[0].Value.ToString();
                if (contador < dataGridView1.RowCount)
                {
                    id += ";";
                }
                sql += id;
            }
            sql += "','" + this.idUsuario + "',NOW(),'Exportación a Excel de Catálogo de Unidades','" + this.empresaa + "','" + this._area + "')";
            MySqlCommand exportacion = new MySqlCommand(sql, c.dbconection());
            exportacion.ExecuteNonQuery();
        }
        void ExportarExcel()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < dataGridView1.Columns.Count; i++) if (dataGridView1.Columns[i].Visible) dt.Columns.Add(dataGridView1.Columns[i].HeaderText);
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {

                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {

                        if (dataGridView1.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = dataGridView1.Rows[j].Cells[i].Value;
                            indice++;
                        }

                    }
                    dt.Rows.Add(row);
                }
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando);
                    this.Invoke(delega);
                }

                v.exportaExcel(dt);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando1);
                    this.Invoke(delega);
                }
                // _UnidadesExportadas();
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool est_expor = false;
        private void button5_Click(object sender, EventArgs e)
        {
            est_expor = true;
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        }

        private void dataGridView1_CellContentDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void cbservicio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbservicio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsaveu_Click(null, e);

        }

        public void _servicios()
        {

            cbservicio.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idservicio as id, UPPER(concat(nombre,' - ',Descripcion)) as nombre FROM cservicios WHERE status=1 and AreafkCareas='" + cbareas.SelectedValue + "' ORDER BY nombre ASC ");
            DataRow nuevaFila = dt.NewRow();
            DataRow fila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione un servicio--".ToUpper();
            fila["id"] = 1;
            fila["nombre"] = "Sin servicio fijo".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            dt.Rows.InsertAt(fila, 1);
            cbservicio.ValueMember = "id";
            cbservicio.DisplayMember = "nombre";
            cbservicio.DataSource = dt;
            cbservicio.SelectedIndex = 0;
            cbservicio.Enabled = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            relacionServicioEstacion cat = new relacionServicioEstacion(idUsuario);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void gbbuscar_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            CatModelos cat = new CatModelos(idUsuario, v);
            cat.Owner = this;
            cat.ShowDialog();
        }

        public void _serviciosDesactivados()
        {
            cbservicio.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idservicio as id, UPPER(nombre) as nombre FROM cservicios WHERE (status=1 OR idservicio='" + ServicioAnterior + "')and AreafkCareas='" + cbareas.SelectedValue + "' ORDER BY nombre ASC ");
            DataRow nuevaFila = dt.NewRow();
            DataRow fila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione un servicio--".ToUpper();
            fila["id"] = 1;
            fila["nombre"] = "Sin servicio fijo".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            dt.Rows.InsertAt(fila, 1);
            cbservicio.ValueMember = "id";
            cbservicio.DisplayMember = "nombre";
            cbservicio.DataSource = dt;
            cbservicio.SelectedIndex = 0;
            cbservicio.Enabled = true;
        }
        private void csetEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniareas();
        }

        private void cbareas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbareas.SelectedIndex > 0)
                {

                    lblid.Text = v.getaData("SELECT COALESCE(identificador,0) FROM careas WHERE idarea='" + cbareas.SelectedValue + "'").ToString();
                    if (!editar)
                    {
                        //txtgeteco.Text = v.getaData("SELECT COALESCE(Max(consecutivo),0)+1 as id FROM cunidades WHERE areafkcareas ='" + cbareas.SelectedValue + "'").ToString();
                    }
                    else
                    {
                        if (areaAnterior == (int)cbareas.SelectedValue)
                        {
                            txtgeteco.Text = ecoAnterior;
                        }
                        else
                        {
                            //txtgeteco.Text = v.getaData("SELECT COALESCE(Max(consecutivo),0)+1 as id FROM cunidades WHERE areafkcareas ='" + cbareas.SelectedValue + "'").ToString();
                        }
                    }
                    txtgeteco.Visible = true;
                    lblgeteco.Visible = true;
                    _servicios();
                }
                else
                {
                    lblid.Text = "";
                    txtgeteco.Clear();
                    txtgeteco.Visible = false;
                    lblgeteco.Visible = false;
                    cbservicio.DataSource = null;
                    cbservicio.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), v.sistema(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "status")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                {

                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }
    }
}