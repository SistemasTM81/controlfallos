using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class catCategorias : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario, empresa, area, idCategoriaTemp, grupoAnterior, subgrupoAnterior, status;
        bool editar, yaAparecioMensaje;
        string categoriaAnterior;
        public catCategorias(int idUsuario, int empresa, int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cbgrupo.DrawItem += v.combos_DrawItem;
            cbsubgrupo.DrawItem += v.combos_DrawItem;
            cbgrupo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbsubgrupo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }

        private void catCategorias_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
                initializeData(null);
            initializeGroups();
        }
        bool pdesactivar { set; get; }
        bool getCambios()
        {
            if (editar)
            {
                if (status == 1 && (cbsubgrupo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetcategoria.Text.Trim())) && (subgrupoAnterior != Convert.ToInt32(cbsubgrupo.SelectedValue) || !categoriaAnterior.Equals(v.mayusculas(txtgetcategoria.Text.Trim().ToLower()))))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private void cbgrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbgrupo.SelectedIndex > 0)
            {
                initializeData("SELECT t1.idcategoria,UPPER(t4.nombreFalloGral),upper(t2.descfallo),UPPER(categoria),UPPER(concat(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno)),if(t1.status=1,'ACTIVO','NO ACTIVO'), t4.idfallogral ,t2.iddescfallo FROM catcategorias as t1 INNER JOIN cdescfallo as t2 ON t1.subgrupofkcdescfallo=t2.iddescfallo LEFT JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona INNER JOIN cfallosgrales as t4 ON t2.falloGralfkcfallosgrales=t4.idfalloGral WHERE t4.idfallogral='" + cbgrupo.SelectedValue + "' AND  t1.empresa='" + empresa + "'");
                v.iniCombos("SELECT iddescfallo as id, UPPER(descfallo) as nombre FROM cdescfallo WHERE falloGralfkcfallosgrales='" + cbgrupo.SelectedValue + "' AND empresa='" + empresa + "' AND status=1", cbsubgrupo, "id", "nombre", "-- SELECCIONE UN SUBGRUPO--");
                cbsubgrupo.Enabled = true;
                lnkLista.Visible = true;
            }
            else
            {
                cbsubgrupo.DataSource = null;
                cbsubgrupo.Enabled = false;
                lnkLista.Visible = false;
                initializeData(null);
            }
        }
        private void tbcategorias_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbcategorias.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }
        private void lnkLista_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { cbgrupo.SelectedIndex = 0; }
        public void privilegios()
        {
            object sql = v.getaData("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catfallosGrales'");
            if (sql != null)
            {
                string[] privilegios = sql.ToString().Split('/');
                pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegios[1]));
                pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegios[0]));
                peditar = v.getBoolFromInt(Convert.ToInt32(privilegios[2]));
                pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegios[3]));
                mostrar();
            }
        }
        private void cambiosEdicion(object sender, EventArgs e) { if (editar) btnsavemp.Visible = lblsavemp.Visible = getCambios(); }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (getCambios())
            {
                var res = MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(sender, e);
                }
                else if (res == DialogResult.No)
                {
                    limpiar();
                }
            }
            else
            {
                limpiar();
            }
        }

        private void tbcategorias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (getCambios())
                {
                    var res = MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(sender, e);
                    }
                    else if (res == DialogResult.No)
                        aEditar(e);
                }
                else
                    aEditar(e);
            }
        }
        void aEditar(DataGridViewCellEventArgs e)
        {
            try
            {
                idCategoriaTemp = Convert.ToInt32(tbcategorias.Rows[e.RowIndex].Cells[0].Value);
                status = v.getStatusInt(tbcategorias.Rows[e.RowIndex].Cells[5].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        btndeletedesc.BackgroundImage = Properties.Resources.up;
                        lbldeletedesc.Text = "Reactivar";
                    }
                    else
                    {
                        btndeletedesc.BackgroundImage = Properties.Resources.delete__4_;
                        lbldeletedesc.Text = "Desactivar";
                    }
                }
                if (peditar)
                {
                    txtgetcategoria.Text = categoriaAnterior = v.mayusculas(tbcategorias.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                    cbgrupo.SelectedValue = grupoAnterior = Convert.ToInt32(tbcategorias.Rows[e.RowIndex].Cells[6].Value);
                    if (cbgrupo.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idfallogral as id, UPPER(nombrefallogral) as nombre FROM cfallosgrales WHERE status=1 OR idfallogral='" + grupoAnterior + "'", cbgrupo, "id", "nombre", "--SELECCIONE UN GRUPO--");
                        cbgrupo.SelectedValue = grupoAnterior;
                    }

                    if (pinsertar)
                        pEliminarClasificacion.Visible = true;
                    cbsubgrupo.SelectedValue = subgrupoAnterior = Convert.ToInt32(tbcategorias.Rows[e.RowIndex].Cells[7].Value);
                    if (cbsubgrupo.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT iddescfallo as id, UPPER(descfallo) as nombre FROM cdescfallo WHERE falloGralfkcfallosgrales='" + cbgrupo.SelectedValue + "' AND (status=1 OR iddescfallo='" + subgrupoAnterior + "')", cbsubgrupo, "id", "nombre", "-- SELECCIONE UN SUBGRUPO--");
                        cbsubgrupo.SelectedValue = subgrupoAnterior;
                    }
                    btnsavemp.BackgroundImage = Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    gbcatego.Text = "Actualizar Categoría";
                    if (pinsertar) pCancelar.Visible = true;
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Usted no Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void btndeletedesc_Click(object sender, EventArgs e)
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
                if (this.status == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cfallosgrales WHERE idFalloGral='" + grupoAnterior + "'")) == 0)
                    MessageBox.Show("Error Al Reactivar La Categoría:\nGrupo Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                else if (this.status == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cdescfallo WHERE iddescfallo='" + subgrupoAnterior + "'")) == 0)
                    MessageBox.Show("Error Al Reactivar La Categoría:\nSubgrupo Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {

                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación de LA cATEGORÍA de Fallo";
                    obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 30, obs.lblinfo.Location.Y);
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        v.c.insertar("UPDATE catcategorias SET status = '" + status + "' WHERE idcategoria=" + this.idCategoriaTemp);
                        v.c.insertar("UPDATE cfallosesp SET status = '" + status + "' WHERE descfallofkcdescfallo=" + this.idCategoriaTemp);
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Fallos - Categorías','" + idCategoriaTemp + "','" + msg + "activación de Categoría','" + idUsuario + "',NOW(),'" + msg + "activación de Categoría','" + edicion + "','" + empresa + "','" + area + "')");
                        MessageBox.Show("La Categoría y Todos sus Componentes se han " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();

                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void txtgetcategoria_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnsavemp_Click(null, e);
            else
                v.paraUM(e);
        }
        private void cbsubgrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbsubgrupo.SelectedIndex > 0)

                initializeData("SELECT t1.idcategoria,UPPER(t4.nombreFalloGral),upper(t2.descfallo),UPPER(categoria),UPPER(concat(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno)),if(t1.status=1,'ACTIVO','NO ACTIVO'), t4.idfallogral ,t2.iddescfallo FROM catcategorias as t1 INNER JOIN cdescfallo as t2 ON t1.subgrupofkcdescfallo=t2.iddescfallo LEFT JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona INNER JOIN cfallosgrales as t4 ON t2.falloGralfkcfallosgrales=t4.idfalloGral WHERE t2.iddescfallo='" + cbsubgrupo.SelectedValue + "' AND t1.empresa='" + empresa + "'");
            else
                initializeData("SELECT t1.idcategoria,UPPER(t4.nombreFalloGral),upper(t2.descfallo),UPPER(categoria),UPPER(concat(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno)),if(t1.status=1,'ACTIVO','NO ACTIVO'), t4.idfallogral ,t2.iddescfallo FROM catcategorias as t1 INNER JOIN cdescfallo as t2 ON t1.subgrupofkcdescfallo=t2.iddescfallo LEFT JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona INNER JOIN cfallosgrales as t4 ON t2.falloGralfkcfallosgrales=t4.idfalloGral WHERE t4.idfallogral='" + cbgrupo.SelectedValue + "' AND t1.empresa='" + empresa + "'");
        }

        private void txtgetcategoria_Validating(object sender, CancelEventArgs e) { v.espaciosenblanco(sender, e); }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e) { v.mover(sender, e, this); }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbgrupo.SelectedIndex > 0 && cbsubgrupo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtgetcategoria.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((grupoAnterior != (int)cbgrupo.SelectedValue || subgrupoAnterior != (int)cbsubgrupo.SelectedValue || categoriaAnterior != v.mayusculas(txtgetcategoria.Text.Trim().ToLower())) && cbgrupo.SelectedIndex > 0 && cbsubgrupo.SelectedIndex > 0)
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        void mostrar()
        {
            if (pinsertar || peditar)
                gbcatego.Visible = true;
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
        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    insertar();
                else
                    actualizar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            object subgrupo = cbsubgrupo.SelectedValue;
            string categoria = v.mayusculas(txtgetcategoria.Text.Trim().ToLower());
            if (!v.camposVaciosCategorias(Convert.ToInt32(cbgrupo.SelectedValue), Convert.ToInt32(subgrupo), categoria) && !v.existecategoria(subgrupo, categoria, empresa))
            {
                if (v.c.insertar(string.Format("INSERT INTO catcategorias (subgrupofkcdescfallo, categoria, usuariofkcpersonal,empresa) VALUES ('{0}','{1}','{2}','{3}')", new object[] { subgrupo, categoria, idUsuario, empresa })))
                {
                    if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema (form, idregistro, usuariofkcpersonal, fechaHora, Tipo, empresa, area) values('{0}',{1},'{2}',NOW(),'{3}','{4}','{5}')", new object[6] { "Catálogo de Fallos - Categorías", "(SELECT idcategoria FROM catcategorias WHERE subgrupofkcdescfallo='" + subgrupo + "' AND categoria='" + categoria + "')", idUsuario, "Inserción de Categoría", empresa, area })))
                    {
                        MessageBox.Show("Categoria Agregada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        limpiar();
                    }
                }
            }
        }
        void actualizar()
        {
            if (getCambios())
            {
                object subgrupo = cbsubgrupo.SelectedValue;
                string categoria = v.mayusculas(txtgetcategoria.Text.Trim().ToLower());
                if (!v.camposVaciosCategorias(Convert.ToInt32(cbgrupo.SelectedValue), Convert.ToInt32(subgrupo), categoria) && !v.existeCategoriaActualizar(subgrupoAnterior, subgrupo, categoriaAnterior, categoria, empresa))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        if (v.c.insertar(string.Format("UPDATE catcategorias SET subgrupofkcdescfallo='{0}', categoria='{1}' where idcategoria ={2}", new object[3] { subgrupo, categoria, idCategoriaTemp })))
                        {
                            if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema (form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) values('{0}',{1},'{2}','{3}',NOW(),'{4}','{5}','{6}','{7}')", new object[8] { "Catálogo de Fallos - Categorías", idCategoriaTemp, subgrupoAnterior + ";" + categoriaAnterior, idUsuario, "Actualización de Categoría", observaciones, empresa, area })))
                            {
                                MessageBox.Show("Categoria Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                limpiar();
                            }
                        }
                    }
                }
            }
        }
        void limpiar()
        {
            if (pinsertar)
            {
                editar = false;
                btnsavemp.BackgroundImage = Properties.Resources.save;
                gbcatego.Text = "Agregar Categoría";
                cbgrupo.Focus();
            }
            subgrupoAnterior = grupoAnterior = idCategoriaTemp = 0;
            categoriaAnterior = null;
            txtgetcategoria.Clear();
            if (pconsultar)
                initializeData(null);
            catfallosGrales catF = (catfallosGrales)this.Owner;
            catF.iniDescripcionesFallos();
            catF.iniNombres();
            catF.restablecer();
            btndeletedesc.BackgroundImage = Properties.Resources.delete__4_;
            lbldeletedesc.Text = "Desactivar";
            pCancelar.Visible = pEliminarClasificacion.Visible = yaAparecioMensaje = !(btnsavemp.Visible = lblsavemp.Visible = true);
            initializeGroups();
            tbcategorias.ClearSelection();
            txtgetcategoria.Focus();
        }
        void initializeData(string sql)
        {
            tbcategorias.Rows.Clear();
            DataTable dt = (DataTable)v.getData((sql == null ? "SELECT t1.idcategoria,UPPER(t4.nombreFalloGral),upper(t2.descfallo),UPPER(categoria),UPPER(concat(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno)),if(t1.status=1,'ACTIVO','NO ACTIVO'), t4.idfallogral ,t2.iddescfallo FROM catcategorias as t1 INNER JOIN cdescfallo as t2 ON t1.subgrupofkcdescfallo=t2.iddescfallo LEFT JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona INNER JOIN cfallosgrales as t4 ON t2.falloGralfkcfallosgrales=t4.idfalloGral WHERE t1.empresa='" + empresa + "'" : sql));
            for (int i = 0; i < dt.Rows.Count; i++)
                tbcategorias.Rows.Add(dt.Rows[i].ItemArray);
            dt.Dispose();
            dt.EndInit();
            tbcategorias.ClearSelection();
        }
        void initializeGroups()
        {
            var _group = cbgrupo.SelectedValue;
            var _subGroup = cbsubgrupo.SelectedValue;
            v.iniCombos("SELECT idfallogral as id, UPPER(nombrefallogral) as nombre FROM cfallosgrales WHERE status=1 AND empresa ='" + empresa + "'", cbgrupo, "id", "nombre", "--SELECCIONE UN GRUPO--");
            if (_group != null)
            {
                cbgrupo.SelectedValue = _group;
                if (_subGroup != null)
                    cbsubgrupo.SelectedValue = _subGroup;
            }
        }
    }
}
