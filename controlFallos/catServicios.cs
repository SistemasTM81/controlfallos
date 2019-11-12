using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catServicios : Form
    {
        int idservicetemp = 0; bool editarservice = false;
        conexion c = new conexion();
        int idUsuario;
        validaciones v = new validaciones();
        int status, empresa, area;
        string nombreAnterior, descripcionAnterior, empresaAnterior, areaAnterior;
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        public catServicios(int idUsuario, int empresa, int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            dataGridView2.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbempresa.MouseWheel += v.paraComboBox_MouseWheel;
            iniEmpresa();
        }
        public void privilegios()
        {
            string sql = "SELECT insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = '" + Name + "'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            mdr.Read();
            pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
            pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
            peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
            pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            mostrar();
            mdr.Close();
            c.dbconection().Close();
        }
        void iniEmpresa()
        {
            v.iniCombos("SELECT idempresa,UPPER(nombreEmpresa) AS nombreEmpresa FROM cempresas WHERE status=1 AND (empresa='" + empresa + "' AND area='" + area + "') ORDER BY nombreEmpresa ASC", cbempresa, "idempresa", "nombreEmpresa", "--SELECCIONE UNA EMPRESA--");
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editarservice)
            {
                if (status == 1 && (cbempresa.SelectedIndex > 0 && cbarea.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetclave.Text.Trim()) && !string.IsNullOrWhiteSpace(txtgetnombre_s.Text.Trim())) && (!(empresaAnterior ?? "0").Equals(cbempresa.SelectedValue.ToString()) || !(areaAnterior ?? "0").Equals(cbarea.SelectedValue.ToString()) || !nombreAnterior.Equals(v.mayusculas(txtgetclave.Text.Trim().ToLower())) || !descripcionAnterior.Equals(v.mayusculas(txtgetnombre_s.Text.Trim().ToLower()))))
                //if (status == 1 && ((!string.IsNullOrWhiteSpace(txtgetclave.Text) && nombreAnterior != v.mayusculas(txtgetclave.Text.ToLower().Trim()) && cbempresa.SelectedIndex>0 && !string.IsNullOrWhiteSpace(txtgetnombre_s.Text))  && ( !nombreAnterior.Equals(txtgetclave.Text.Trim()) || descripcionAnterior != v.mayusculas(txtgetnombre_s.Text.Trim().ToLower().Trim('-')) || !empresaAnterior.Equals(cbempresa.SelectedValue.ToString()))))
                {
                    btnsaves.Visible = lblsaves.Visible = true;
                }
                else
                {
                    btnsaves.Visible = lblsaves.Visible = false;
                }
            }
        }
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddservice.Visible = true;
            }
            if (pconsultar)
            {
                gbservicios.Visible = true;
            }
            if (peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        private void txtgetclave_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void txtgetnombre_s_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsaves_Click(null, e);
            else
                v.paraUnidades(e);
        }

        private void btnsaves_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editarservice)
                {
                    insertar();
                }
                else
                {
                    editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void editar()
        {
            string nombre = v.mayusculas(txtgetclave.Text.ToLower());
            string des = v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim('-');
            int empresaP = Convert.ToInt32(cbarea.SelectedValue);
            if (!v.formularioServicio(nombre, des, empresaP) && !v.existeServicioActualizar(empresaP, Convert.ToInt32(empresaAnterior), nombre, nombreAnterior, this.idservicetemp, idservicetemp))
            {
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    String sql = "UPDATE cservicios SET Nombre = LTRIM(RTRIM('" + nombre + "')), Descripcion = LTRIM(RTRIM('" + des + "')), AreafkCareas='" + empresaP + "' WHERE idservicio =" + this.idservicetemp;
                    if (v.c.insertar(sql))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Servicios','" + idservicetemp + "','Nombre: " + nombreAnterior + ";Descripción: " + descripcionAnterior + "','" + idUsuario + "',NOW(),'Actualización de Servicio','" + observaciones + "','" + this.empresa + "','" + area + "')");
                        if (!yaAparecioMensaje)
                        {
                            MessageBox.Show("El Servicio Se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        limpiar();
                    }
                }
            }

        }
        void limpiar()
        {
            if (pinsertar)
            {
                editarservice = false;
                btnsaves.BackgroundImage = controlFallos.Properties.Resources.save;
                gbaddservice.Text = "Agregar Servicio";
                lblsaves.Text = "Guardar";
                cbempresa.Focus();
            }
            if (pconsultar)
            {
                busqservices();

            }
            yaAparecioMensaje = false;
            btnsaves.Visible = lblsaves.Visible = true;
            pCancelar.Visible = false;
            idservicetemp = 0;
            pEliminarService.Visible = false;
            txtgetclave.Clear();
            txtgetnombre_s.Clear();

            catUnidades cat = (catUnidades)Owner;
            if (cat.csetEmpresa.SelectedIndex > 0)
            {
                var temp = cat.csetEmpresa.SelectedValue;
                var temp2 = 0; if (cat.cbareas.SelectedIndex > 0) temp2 = int.Parse(cat.cbareas.SelectedValue.ToString());
                object cbtemp3 = 0; if (cat.cbservicio.SelectedIndex > 0) cbtemp3 = cat.cbservicio.SelectedValue;
                cat._servicios();
                cat.csetEmpresa.SelectedValue = temp;
                cat.cbservicio.Enabled = true;
                cat.cbareas.SelectedValue = temp2;
                cat.cbservicio.SelectedValue = cbtemp3;
            }
            else
            {
                cat.cbservicio.DataSource = null;
                cat.cbservicio.Enabled = false;
            }
            cbempresa.SelectedIndex = 0;
            cat.bunidades();
            iniEmpresa();
        }
        void insertar()
        {
            string nombre = v.mayusculas(txtgetclave.Text.ToLower());
            string descripcion = v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim('-');
            int bussiness = Convert.ToInt32(cbarea.SelectedValue);
            if (!v.formularioServicio(nombre, descripcion, bussiness) && !v.yaExisteServicio(cbarea.SelectedValue, nombre))
            {
                String sql = "INSERT INTO cservicios (Nombre,Descripcion,usuariofkcpersonal,AreafkCareas) VALUES (LTRIM(RTRIM('" + nombre + "')),LTRIM(RTRIM('" + descripcion + "')),'" + this.idUsuario + "','" + bussiness + "')";
                if (v.c.insertar(sql))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Servicios',(SELECT idservicio FROM cservicios WHERE AreafkCareas='" + bussiness + "' and Nombre='" + nombre + "' and Descripcion='" + descripcion + "'),'Inserción de Servicio','" + idUsuario + "',NOW(),'Inserción de Servicio','" + empresa + "','" + area + "')");
                    MessageBox.Show("El Servicio se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    limpiar();
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (idservicetemp > 0)
            {
                if (Convert.ToInt32(v.getaData("SELECT (SELECT status FROM careas WHERE idarea=areafkcareas) FROM cservicios WHERE idservicio='" + idservicetemp + "'")) == 1)
                {


                    try
                    {
                        int state;
                        string msg;
                        if (status == 0)
                        {
                            state = 1;
                            msg = "Re";
                        }
                        else
                        {
                            state = 0;
                            msg = "Des";
                        }
                        if (state == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t3.status FROM cservicios as t1 INNER JOIN careas as t2 ON t1.areafkcareas=t2.idarea INNER JOIN cempresas as t3 ON t2.empresafkcempresas=T3.idempresa WHERE t1.idservicio = '{0}'", idservicetemp))) == 0)
                            MessageBox.Show("Error al Reactivar:'\nEmpresa Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (state == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t2.status FROM cservicios as t1 INNER JOIN careas as t2 ON t1.areafkcareas=t2.idarea WHERE t1.idservicio = '{0}'", idservicetemp))) == 0)
                            MessageBox.Show("Error al Reactivar:'\nEmpresa Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Servicio";
                            if (obs.ShowDialog() == DialogResult.OK)
                            {
                                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                String sql = "UPDATE cservicios SET status = '" + state + "' WHERE idservicio = '" + this.idservicetemp + "'";
                                if (v.c.insertar(sql))
                                {
                                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Servicios','" + idservicetemp + "','" + msg + "activación de Servicio','" + idUsuario + "',NOW(),'" + msg + "activación de Servicio','" + edicion + "','" + empresa + "','" + area + "')");
                                    MessageBox.Show("El Servicio se Ha " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    limpiar();
                                }
                                else
                                {
                                    MessageBox.Show("Servicio no Desactivado");
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(v.mayusculas("No Se Puede Reactivar Debido A Que el Área a la Que Pertenece Se Encuentra desactivada".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void servicios_index()
        {
            dataGridView2.Rows.Clear();
            String sql = "SELECT t1.idservicio,UPPER(t1.Nombre) AS Nombre,UPPER(t4.nombreEmpresa) as Empresa,upper(t3.nombreArea) as Area,UPPER(t1.Descripcion) AS Descripcion,t1.status, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) AS persona,AreafkCareas as fk FROM cservicios as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona INNER JOIN careas as t3 on t3.idarea=t1.AreafkCareas inner join cempresas as t4 on t4.idempresa=t3.empresafkcempresas ";
            string wheres = "";
            if (cbempresa.SelectedIndex > 0)
            {
                if (wheres == "")
                {
                    wheres = "  where t4.idempresa='" + cbempresa.SelectedValue + "'";
                }
                else
                {
                    wheres += " and t4.idempresa='" + cbempresa.SelectedValue + "'";
                }
            }
            if (cbarea.SelectedIndex > 0 && cbempresa.SelectedIndex > 0)
            {
                if (wheres == "")
                {
                    wheres = " where t3.idarea='" + cbarea.SelectedValue + "'";
                }
                else
                {
                    wheres += " and t3.idarea='" + cbarea.SelectedValue + "'";
                }
            }
            MySqlCommand cm = new MySqlCommand(sql + wheres, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView2.Rows.Add(dr.GetInt32("idservicio"), dr.GetString("Nombre"), dr.GetString("Empresa"), dr.GetString("Area"), dr.GetString("Descripcion"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")), dr.GetString("fk"));
            }
            dataGridView2.ClearSelection();
            dr.Close();
            c.dbconection().Close();
        }
        public void busqservices()
        {
            dataGridView2.Rows.Clear();
            String sql = "SELECT t1.idservicio,UPPER(t1.Nombre) AS Nombre,UPPER(t4.nombreEmpresa) as Empresa,upper(t3.nombreArea) as Area,UPPER(t1.Descripcion) AS Descripcion,t1.status, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) AS persona,AreafkCareas as fk FROM cservicios as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona INNER JOIN careas as t3 on t3.idarea=t1.AreafkCareas inner join cempresas as t4 on t4.idempresa=t3.empresafkcempresas;";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView2.Rows.Add(dr.GetInt32("idservicio"), dr.GetString("Nombre"), dr.GetString("Empresa"), dr.GetString("Area"), dr.GetString("Descripcion"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")), dr.GetString("fk"));
            }
            dataGridView2.ClearSelection();
            dr.Close();
            c.dbconection().Close();
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (idservicetemp > 0 && peditar && (!v.mayusculas(txtgetclave.Text.ToLower()).Trim().Equals(nombreAnterior) || !v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim().Equals(descripcionAnterior)))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsaves_Click(null, e);
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
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                idservicetemp = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt((string)dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelete.Text = "Reactivar";
                    }
                    else
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar";
                    }
                    pEliminarService.Visible = true;
                }
                if (peditar)
                {
                    editarservice = true;
                    txtgetclave.Text = nombreAnterior = v.mayusculas(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    txtgetnombre_s.Text = descripcionAnterior = v.mayusculas(dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString().ToLower());
                    areaAnterior = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
                    cbempresa.SelectedValue = empresaAnterior = (v.getaData("select t1.idempresa from cempresas as t1 inner join careas as t2 on t1.idempresa=t2.empresafkcempresas where t2.idarea='" + dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString() + "'").ToString());
                    if (cbempresa.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idempresa,UPPER(nombreEmpresa) AS nombreEmpresa FROM cempresas WHERE (status=1 OR idempresa='" + empresaAnterior + "') ORDER BY nombreEmpresa ASC", cbempresa, "idempresa", "nombreEmpresa", "--SELECCIONE UNA EMPRESA--");
                        cbempresa.SelectedValue = empresaAnterior;
                    }
                    cbarea.SelectedValue = areaAnterior;
                    if (cbarea.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idarea, upper(nombreArea) as area FROM careas WHERE (status = 1 OR idarea='" + areaAnterior + "') AND empresafkcempresas='" + cbempresa.SelectedValue + "'", cbarea, "idarea", "area", "--SELECCIONE UN ÁREA--");
                        cbarea.SelectedValue = areaAnterior;
                    }

                    dataGridView2.ClearSelection();
                    btnsaves.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsaves.Text = "Guardar";
                    gbaddservice.Text = "Actualizar Servicio";
                    if (pinsertar) pCancelar.Visible = true;
                    btnsaves.Visible = lblsaves.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Usted No Tiene Privilegios Para Editar En éste Formulario", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btncancelar_Click(object sender, EventArgs e)
        {
            if ((!v.mayusculas(txtgetclave.Text.ToLower()).Trim().Equals(nombreAnterior) || !v.mayusculas(txtgetnombre_s.Text.ToLower()).Trim().Equals(descripcionAnterior)) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsaves_Click(null, e);
                }
                else
                {
                    limpiar();
                }
            }
            else
            {
                limpiar();
            }
        }

        private void dataGridView2_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetclave_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
            TextBox txt = sender as TextBox;
            while (txt.Text.Contains("--"))
            {
                txt.Text = txt.Text.Replace("--", "-").Trim();
            }

        }

        private void gbaddservice_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void cbarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbempresa.SelectedIndex > 0)
            {
                servicios_index();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editarservice)
            {
                if (cbempresa.SelectedIndex > 0 || cbarea.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtgetclave.Text) || !string.IsNullOrWhiteSpace(txtgetnombre_s.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((Convert.ToInt32(empresaAnterior) != (int)cbempresa.SelectedValue || Convert.ToInt32(areaAnterior) != (int)cbarea.SelectedValue || nombreAnterior != v.mayusculas(txtgetclave.Text.Trim().ToLower()) || descripcionAnterior != v.mayusculas(txtgetnombre_s.Text.Trim().ToLower()))&& cbempresa.SelectedIndex>0 && cbarea.SelectedIndex>0 && !string.IsNullOrWhiteSpace(txtgetnombre_s.Text) && !string.IsNullOrWhiteSpace(txtgetclave.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }

        }

        private void cbempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbempresa.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idarea, upper(nombreArea) as area FROM careas WHERE status = 1 AND empresafkcempresas='" + cbempresa.SelectedValue + "'", cbarea, "idarea", "area", "--SELECCIONE UN ÁREA--");
                servicios_index();
                cbarea.Enabled = true;
            }
            else
            {
                busqservices();
                cbarea.DataSource = null;
                cbarea.Enabled = false;
            }

        }

        private void cbempresa_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void catServicios_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                busqservices();

            }
        }

        private void gbadd_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Estatus")
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
