using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catClasificaciones : Form
    {
        int idUsuario, empresa, area;
        bool editar, yaAparecioMensaje;
        int idFalloTemp;
        string clasificacionAnterior;
        int status;
        validaciones v;
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        public void privilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "catfallosGrales")).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                if (privilegiosTemp.Length > 3)
                {
                    pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }
            }
            mostrar();

        }
        void mostrar()
        {
            if (pinsertar || peditar)
                gbaddClalif.Visible = true;
            if (pconsultar)
                gbclasif.Visible = true;
            if (peditar)
                label23.Visible = label2.Visible = true;
            if (peditar && !pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Anaquel";
                editar = true;
            }
        }
        public catClasificaciones(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            tbfallos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbfallos.ColumnHeadersDefaultCellStyle = d;
            d.Font = new Font("Garamond", 12, FontStyle.Regular);
            tbfallos.DefaultCellStyle = d;
        }
        void iniClasificaciones()
        {
            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.idFalloGral,UPPER(t1.nombreFalloGral) AS nombreFalloGral,t1.status, UPPER(CONCAT(coalesce(t2.nombres,''),' ',coalesce(t2.apPaterno,''),' ',coalesce(t2.apMaterno,''))) as nombre FROM cfallosgrales as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona WHERE t1.empresa = '" + empresa + "'";
                MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                    tbfallos.Rows.Add(dr.GetInt32("idFalloGral"), dr.GetString("nombreFalloGral"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
                dr.Close();
                v.c.dbcon.Close();
                tbfallos.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void catClasificaciones_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
                iniClasificaciones();
        }
        private void txtgetcempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsavemp_Click(null, e);
            else
                v.Sololetras(e);
        }
        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    insertar();
                else
                    editarC();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        void insertar()
        {
            //
            string clasificacion = txtgetclasificacion.Text.ToLower().Trim();
            if (!string.IsNullOrWhiteSpace(clasificacion))
            {
                if (!v.yaExisteFalloGral(clasificacion, empresa))
                {
                    if (v.c.insertar("INSERT INTO cfallosgrales(NombreFalloGral,usuariofkcpersonal,empresa) VALUES (LTRIM(RTRIM('" + v.mayusculas(clasificacion) + "')),'" + idUsuario + "','" + empresa + "')"))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Clasificaciones',(SELECT idfalloGral From cfallosgrales WHERE nombrefalloGral ='" + clasificacion + "'),'Inserción de Grupo de Fallo','" + idUsuario + "',NOW(),'Inserción de Grupo de Fallo','" + empresa + "','" + area + "')");
                        MessageBox.Show("Se Ha Insertado El Grupo de Fallos Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        catfallosGrales catC = (catfallosGrales)this.Owner;
                        var _cla = catC.cbclasificacion.SelectedValue; var _desc = catC.cbdescripcion.SelectedValue;
                        txtgetclasificacion.Clear();
                        iniClasificaciones();
                        catC.iniClasificacionesFallos();
                        catC.iniNombres();
                        catC.busqueda_clasificacion();
                        if (Convert.ToInt32(_cla) > 0)
                        {
                            catC.cbclasificacion.SelectedValue = _cla;
                            catC.cbdescripcion.SelectedValue = _desc;
                            _cla = 0;
                            _desc = 0;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("El Campo \"Grupo\" No Puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtgetclasificacion.Focus();
            }
        }
        void editarC()
        {
            if (idFalloTemp > 0)
            {
                if (status == 0)
                {
                    MessageBox.Show("No se Puede Editar Un Grupo Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        restablecer();
                }
                else
                {
                    string clasificacion = v.mayusculas(txtgetclasificacion.Text.ToLower());
                    if (!string.IsNullOrWhiteSpace(clasificacion))
                    {
                        if (!v.mayusculas(clasificacion).Equals(clasificacionAnterior))
                        {
                            if (!v.existeFalloGralActualizar(clasificacion, clasificacionAnterior, empresa))
                            {
                                observacionesEdicion obs = new observacionesEdicion(v);
                                obs.Owner = this;
                                if (obs.ShowDialog() == DialogResult.OK)
                                {
                                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                    if (v.c.insertar("UPDATE cfallosgrales SET NombreFalloGral = LTRIM(RTRIM('" + v.mayusculas(clasificacion) + "')) WHERE idFalloGral=" + this.idFalloTemp + ""))
                                    {
                                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Fallos - Clasificaciones','" + idFalloTemp + "','" + clasificacionAnterior + "','" + idUsuario + "',NOW(),'Actualización de Grupo de Fallo','" + observaciones + "','" + empresa + "','" + area + "')");
                                        if (!yaAparecioMensaje)
                                        {
                                            MessageBox.Show("Se ha Actualizado la Clasificación del Fallo Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        catfallosGrales catC = (catfallosGrales)this.Owner;
                                        var _cla = catC.cbclasificacion.SelectedValue; var _desc = catC.cbdescripcion.SelectedValue;
                                        catC.iniClasificacionesFallos();
                                        catC.iniNombres();
                                        catC.busqueda_clasificacion();
                                        restablecer();
                                        if (Convert.ToInt32(_cla) > 0)
                                        {
                                            catC.cbclasificacion.SelectedValue = _cla;
                                            catC.cbdescripcion.SelectedValue = _desc;
                                            _cla = 0;
                                            _desc = 0;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                restablecer();
                        }
                    }
                    else MessageBox.Show("No se Puede Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Seleccione una Clasificacion de la Tabla para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void tbfallos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfallos.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }
        private void tbfallos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string clasificacion = v.mayusculas(txtgetclasificacion.Text.ToLower().Trim());
                if (idFalloTemp > 0 && peditar && !v.mayusculas(clasificacion).Equals(clasificacionAnterior) && status == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null, e);
                    }
                    else
                        guardarReporte(e);
                }
                else
                    guardarReporte(e);
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {

                idFalloTemp = Convert.ToInt32(tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (pdesactivar)
                {
                    if (v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[3].Value.ToString()) == 0)
                    {
                        btndelcla.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelcla.Text = "Reactivar";
                    }
                    else
                    {
                        btndelcla.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelcla.Text = "Desactivar";
                    }
                    pEliminarClasificacion.Visible = true;
                }
                if (peditar)
                {
                    txtgetclasificacion.Text = clasificacionAnterior = v.mayusculas(tbfallos.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    gbaddClalif.Text = "Actualizar Clasificación de Fallo";
                    if (pinsertar) pCancelar.Visible = true;
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        void restablecer()
        {
            idFalloTemp = 0;
            clasificacionAnterior = null;
            txtgetclasificacion.Clear();
            if (pinsertar)
            {
                btnsavemp.BackgroundImage = Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
                gbaddClalif.Text = "Agregar Clasificación de Fallo";
                txtgetclasificacion.Focus();
            }
            pCancelar.Visible = false;
            if (pconsultar)
                iniClasificaciones();
            yaAparecioMensaje = false;
            btnsavemp.Visible = lblsavemp.Visible = true;
            pEliminarClasificacion.Visible = false;
            catfallosGrales catC = (catfallosGrales)this.Owner;
            catC.iniClasificacionesFallos();
            catC.iniNombres();
            catC.restablecer();
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            string clasificacion = v.mayusculas(txtgetclasificacion.Text.ToLower().Trim());
            if (!v.mayusculas(clasificacion).Equals(clasificacionAnterior) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    editarC();
                }
                else
                    restablecer();
            }
            else
                restablecer();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                string msg2 = "";
                if (this.status == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                    msg2 = "De igual Manera se Desactivarán las Descripciones y Los Nombres de Fallos Asociados a él";
                }
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación de la Clasificación de Fallo";
                obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 30, obs.lblinfo.Location.Y);
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    var res = v.c.insertar("UPDATE cfallosgrales as t1 left join cdescfallo as t2 ON t1.idfallogral=t2.fallogralfkcfallosgrales LEFT JOIN catcategorias as t4 ON t4.subgrupofkcdescfallo=t2.iddescfallo left join cfallosesp as t3 ON t4.idcategoria=t3.descfallofkcdescfallo  SET t1.status = '" + status + "', t2.status = '" + status + "', t3.status = '" + status + "' , t4.status = '" + status + "' WHERE idFalloGral= " + this.idFalloTemp);
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Fallos - Clasificaciones','" + idFalloTemp + "','" + msg + "activación de Grupo de Fallo','" + idUsuario + "',NOW(),'" + msg + "activación de Grupo de Fallo','" + edicion + "','" + empresa + "','" + area + "')");
                    MessageBox.Show("El Grupo de Fallo se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    msg2 = msg = null;
                    status = 0;
                    restablecer();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void tbfallos_ColumnAdded(object sender, DataGridViewColumnEventArgs e) { v.paraDataGridViews_ColumnAdded(sender, e); }
        private void txtgetclasificacion_Validating(object sender, CancelEventArgs e) { v.espaciosenblanco(sender, e); }
        private void gbaddClalif_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        private void txtgetclasificacion_TextChanged(object sender, EventArgs e) { if (editar) btnsavemp.Visible = lblsavemp.Visible = (status == 1 && (!string.IsNullOrWhiteSpace(txtgetclasificacion.Text) && clasificacionAnterior != v.mayusculas(txtgetclasificacion.Text.ToLower()).Trim())); }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtgetclasificacion.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (clasificacionAnterior != v.mayusculas(txtgetclasificacion.Text.Trim().ToLower()) && !string.IsNullOrWhiteSpace(txtgetclasificacion.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }
        private void lbltitle_MouseDown(object sender, MouseEventArgs e) { v.mover(sender, e, this); }
    }
}