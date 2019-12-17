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
    public partial class catDescFallos : Form
    {
        bool editar, yaAparecioMensaje = false;
        string descripcionAnterior;
        string idDescripcion;
        string clasifAnterior;
        conexion c = new conexion();
        int idUsuario, empresa, area;
        int status;
        validaciones v = new validaciones();
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        public catDescFallos(int idUsuario, int empresa, int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cbclasificacion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbfallos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (((cbclasificacion.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetdescfallo.Text)) && (!cbclasificacion.SelectedValue.ToString().Equals(clasifAnterior) || !v.mayusculas(txtgetdescfallo.Text.ToString()).Equals(this.descripcionAnterior))) && status == 1)
                    btnsavemp.Visible = lblsavemp.Visible = true;
                else
                    btnsavemp.Visible = lblsavemp.Visible = false;
            }
        }

        public void privilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "catfallosGrales")).ToString().Split('/');
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
                gbaddclasif.Visible = true;
            if (pconsultar)
                gbclasif.Visible = true;
            if (peditar)
                label23.Visible = label3.Visible = true;
            if (peditar && !pinsertar)
            {
                btnsavemp.BackgroundImage = Properties.Resources.pencil;
                editar = true;
            }
        }
        void iniClasificacionesFallos()
        {
            object res = null;
            if (cbclasificacion.SelectedIndex > 0) res = cbclasificacion.SelectedValue;
            v.iniCombos("SELECT idFalloGral as id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales WHERE status = 1 AND empresa ='" + empresa + "'", cbclasificacion, "id", "nombre", "-- SELECCIONE UN GRUPO --");
            if (res != null) cbclasificacion.SelectedValue = res;
        }
        void iniDescripciones(string complex)
        {

            try
            {
                tbfallos.Rows.Clear();
                String sql = "SELECT t1.iddescfallo, UPPER(t3.NombreFalloGral) AS NombreFalloGral,upper(t1.descfallo) as descfallo,UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno))as nombre ,if(t1.status=1,'ACTIVO','NO ACTIVO'), t1.falloGralfkcfallosgrales as idclasif FROM cdescfallo as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona INNER JOIN cfallosgrales as t3 ON  t1.falloGralfkcfallosgrales= t3.idFalloGral WHERE t1.empresa='" + empresa + "'";
                DataTable dt = (DataTable)v.getData((complex == null ? sql : complex));
                for (int i = 0; i < dt.Rows.Count; i++)
                    tbfallos.Rows.Add(dt.Rows[i].ItemArray);
                tbfallos.ClearSelection();
                dt.Dispose();
                dt.EndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void catDescFallos_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pinsertar || peditar)
                iniClasificacionesFallos();

            if (pconsultar)
                iniDescripciones(null);

        }

        private void tbfallos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfallos.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void txtgetclasificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsavemp_Click(null, e);
            else
                v.letrasnumerosdiagonalyguion(e);
        }
        public bool mensaje = false;
        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                mensaje = true;
                if (!editar)
                    insertar();
                else
                    editarDe();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            string descfallo = txtgetdescfallo.Text.ToLower();
            string clasif = cbclasificacion.SelectedValue.ToString();
            if (cbclasificacion.SelectedIndex > 0)
            {
                if (!string.IsNullOrWhiteSpace(descfallo))
                {
                    if (!v.yaExisteDescFallo(clasif, descfallo, empresa))
                    {
                        if (v.c.insertar("INSERT INTO cdescfallo (falloGralfkcfallosgrales, descfallo, usuariofkcpersonal,empresa) VALUES ('" + clasif + "',LTRIM(RTRIM('" + v.mayusculas(descfallo) + "')),'" + this.idUsuario + "','" + empresa + "')"))
                        {
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Descripciones',(SELECT iddescfallo From cdescfallo WHERE falloGralfkcfallosgrales='" + clasif + "' AND descfallo='" + descfallo + "'),'Inserción de Subgrupo de Fallo','" + idUsuario + "',NOW(),'Inserción de Descripción','" + empresa + "','" + area + "')");
                            MessageBox.Show("El Subgrupo se ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            catfallosGrales cat = (catfallosGrales)Owner;
                            var _clas = cat.cbclasificacion.SelectedValue; var _desc = cat.cbdescripcion.SelectedValue ?? 0;
                            restablecer();
                            if (Convert.ToInt32(_clas) > 0) { cat.cbclasificacion.SelectedValue = _clas; }
                            if (Convert.ToInt32(_desc) > 0) { cat.cbdescripcion.SelectedValue = _desc; }
                            txtgetdescfallo.Focus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El Campo \"SubGrupo\" No puede estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtgetdescfallo.Focus();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un Grupo de la Lista Desplegable", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbclasificacion.Focus();
            }

        }
        void editarDe()
        {
            if (!string.IsNullOrWhiteSpace(idDescripcion))
            {
                if (status == 1)
                {
                    string desc = txtgetdescfallo.Text.ToLower();
                    string clasif = cbclasificacion.SelectedValue.ToString();
                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        if (v.mayusculas(desc).Equals(this.descripcionAnterior) && clasif.Equals(clasifAnterior))
                        {
                            MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                restablecer();
                        }
                        else
                        {
                            if (!v.existeDescFalloActualizar(Convert.ToInt32(clasif), Convert.ToInt32(clasifAnterior), v.mayusculas(desc), descripcionAnterior, empresa))
                            {
                                observacionesEdicion obs = new observacionesEdicion(v);
                                obs.Owner = this;
                                if (obs.ShowDialog() == DialogResult.OK)
                                {
                                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                    if (v.c.insertar("UPDATE cdescfallo SET falloGralfkcfallosgrales='" + clasif + "', descfallo = LTRIM(RTRIM('" + v.mayusculas(desc) + "')) WHERE iddescfallo=" + this.idDescripcion))
                                    {
                                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Fallos - Descripciones','" + idDescripcion + "','" + clasifAnterior + ";" + descripcionAnterior + "','" + idUsuario + "',NOW(),'Actualización de Subgrupo de Fallo','" + observaciones + "','" + empresa + "','" + area + "')");
                                        if (!yaAparecioMensaje) MessageBox.Show("Descripcion Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        catfallosGrales cat = (catfallosGrales)Owner;
                                        var _clas = cat.cbclasificacion.SelectedValue; var _desc = cat.cbdescripcion.SelectedValue;
                                        restablecer();
                                        var clasifTemp = cbclasificacion.SelectedValue;
                                        cbclasificacion.SelectedIndex = 0;
                                        cbclasificacion.SelectedValue = clasifTemp;
                                        if (Convert.ToInt32(_clas) > 0)
                                        {
                                            cat.cbclasificacion.SelectedValue = _clas;
                                            cat.cbdescripcion.SelectedValue = _desc;
                                            _clas = 0; _desc = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("El campo Descripción de Fallo no puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No se Puede Editar una Descripción Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una Descripcion de Fallos de la Tabla Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void tbfallos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string desc = txtgetdescfallo.Text.ToLower().Trim();
                string clasif = cbclasificacion.SelectedValue.ToString();
                if ((cbclasificacion.SelectedIndex > 0 && !clasif.Equals(clasifAnterior) || (!string.IsNullOrWhiteSpace(desc) && !v.mayusculas(desc).Equals(this.descripcionAnterior))) && status == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(sender, e);
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
            restablecer();
            try
            {
                idDescripcion = tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString();
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletedesc.Text = "Reactivar";
                    }
                    else
                    {

                        btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletedesc.Text = "Desactivar";
                    }
                }
                if (peditar)
                {
                    txtgetdescfallo.Text = descripcionAnterior = v.mayusculas(tbfallos.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    cbclasificacion.SelectedValue = clasifAnterior = tbfallos.Rows[e.RowIndex].Cells[5].Value.ToString();
                    pEliminarClasificacion.Visible = true;
                    if (cbclasificacion.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idFalloGral as id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales WHERE (status = 1 OR idFalloGral='" + clasifAnterior + "') ORDER BY NombreFalloGral ASC", cbclasificacion, "id", "nombre", "-- SELECCIONE UN GRUPO --");
                        cbclasificacion.SelectedValue = clasifAnterior;
                    }
                    btnsavemp.BackgroundImage = Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    gbaddclasif.Text = "Actualizar Subgrupo";
                    if (pinsertar) pCancelar.Visible = true;
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Usted no Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void restablecer()
        {
            if (pinsertar)
            {
                editar = false;
                btnsavemp.BackgroundImage = Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                gbaddclasif.Text = "Agregar Subgrupo";
                cbclasificacion.Focus();
            }
            idDescripcion = null;
            descripcionAnterior = null;
            txtgetdescfallo.Clear();
            pCancelar.Visible = false;
            pEliminarClasificacion.Visible = false;
            yaAparecioMensaje = false;
            if (pconsultar)
                iniDescripciones(null);
            catfallosGrales catF = (catfallosGrales)this.Owner;
            catF.iniDescripcionesFallos();
            catF.iniNombres();
            catF.restablecer();
            btndeletedesc.BackgroundImage = Properties.Resources.delete__4_;
            lbldeletedesc.Text = "Desactivar";
            btnsavemp.Visible = true;
            lblsavemp.Visible = true;
            mensaje = false;
            iniClasificacionesFallos();
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            string desc = txtgetdescfallo.Text.ToLower().Trim();
            string clasif = cbclasificacion.SelectedValue.ToString();
            if ((cbclasificacion.SelectedIndex > 0 && !clasif.Equals(clasifAnterior) || (!string.IsNullOrWhiteSpace(desc) && !v.mayusculas(desc).Equals(this.descripcionAnterior))) && status == 1 && peditar)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(sender, e);
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

                if (this.status == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                }
                if (this.status == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cfallosgrales WHERE idFalloGral=(SELECT falloGralfkcfallosgrales FROM cdescfallo WHERE iddescfallo='" + this.idDescripcion + "')")) == 0)
                {
                    MessageBox.Show("Error Al Reactivar La Descripción:\nGrupo Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación del Subgrupo de Fallo";
                    obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 30, obs.lblinfo.Location.Y);
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        var res = v.c.insertar("UPDATE cdescfallo as t2 LEFT JOIN catcategorias as t4 ON t4.subgrupofkcdescfallo=t2.iddescfallo left join cfallosesp as t3 ON t4.idcategoria=t3.descfallofkcdescfallo  SET t2.status = '" + status + "', t3.status = '" + status + "' , t4.status = '" + status + "' WHERE iddescfallo= " + this.idDescripcion);
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Fallos - Descripciones','" + idDescripcion + "','" + msg + "activación de Descripción','" + idUsuario + "',NOW(),'" + msg + "activación de Subgrupo de Fallo','" + edicion + "','" + empresa + "','" + area + "')");
                        restablecer();
                        MessageBox.Show("El Subgrupo y Todos sus Componentes se han " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void gbaddclasif_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void cbclasificacion_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbclasificacion.SelectedIndex > 0)
            {
                lnkLista.Visible = true;
                iniDescripciones("SELECT t1.iddescfallo, UPPER(t3.NombreFalloGral) AS NombreFalloGral,upper(t1.descfallo) as descfallo,UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno))as nombre ,if(t1.status=1,'ACTIVO','NO ACTIVO'), t1.falloGralfkcfallosgrales as idclasif FROM cdescfallo as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona INNER JOIN cfallosgrales as t3 ON  t1.falloGralfkcfallosgrales= t3.idFalloGral WHERE t3.idfalloGral = '" + cbclasificacion.SelectedValue + "' AND t1.empresa = '" + empresa + "'");
            }
            else
            {
                lnkLista.Visible = false;
                iniDescripciones(null);
            }


        }

        private void lnkLista_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cbclasificacion.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbclasificacion.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtgetdescfallo.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((Convert.ToInt32(clasifAnterior) != (int)cbclasificacion.SelectedValue || descripcionAnterior!=v.mayusculas(txtgetdescfallo.Text.Trim().ToLower())) && cbclasificacion.SelectedIndex>0 && !string.IsNullOrWhiteSpace(txtgetdescfallo.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void txtgetdescfallo_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }
        private void cbclasificacion_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        private void tbfallos_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }
    }
}