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
    public partial class familias : Form
    {
        validaciones v;
        int idfamTemp;
        bool reactivar;
        string familiaAnterior, descAnterior;
        int _status, empresa, area, umanterior;
        bool editar, yaAparecioMensaje;
        int idUsuario;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        public familias(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            cbnombreFamilia.Focus();
            this.idUsuario = idUsuario;
            tbfamilias.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbnombreFamilia.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbunidad.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            cbnombreFamilia.DrawItem += v.combos_DrawItem;
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbfamilias.ColumnHeadersDefaultCellStyle = d;
            Unidad_Medida();
        }
        public void Unidad_Medida()
        {
            v.iniCombos("Select t1.idunidadmedida as id, upper(t1.nombre) as simbolo from cunidadmedida as t1 where t1.status='1' and empresa='" + empresa + "' order by t1.nombre asc;", cbunidad, "id", "simbolo", "-- Seleccione --");
            v.iniCombos("SELECT idcnfamilia as id,familia  FROM cnfamilias WHERE status=1 and empresa='" + empresa + "' ORDER BY familia ASC", cbnombreFamilia, "id", "familia", "--SELECCIONE UN NOMBRE DE FAMILIA--");
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (_status == 1 && ((cbnombreFamilia.SelectedIndex > 0) && familiaAnterior != cbnombreFamilia.SelectedValue.ToString()) || (!string.IsNullOrWhiteSpace(txtdescfamilia.Text) && descAnterior != v.mayusculas(txtdescfamilia.Text.ToLower().Trim())) || (cbunidad.SelectedIndex > 0 && umanterior != Convert.ToInt32(cbunidad.SelectedValue)))
                {
                    btnsave.Visible = lblsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = lblsave.Visible = false;
                }
            }
        }
        public void establecerPrivilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "catRefacciones")).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
            }
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddfamilia.Visible = true;
            }
            if (Pconsultar)
            {
                gbfamilias.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    _insertar(); ;
                }
                else
                {
                    _editar();
                }
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void _insertar()
        {
            int nombre = Convert.ToInt32(cbnombreFamilia.SelectedValue);
            string desc = v.mayusculas(txtdescfamilia.Text.ToLower());
            int um = Convert.ToInt32(cbunidad.SelectedValue);
            if (!v.formulariofamilias(nombre, desc, um) && !v.existeFamilia(nombre, desc, empresa))
            {

                if (v.c.insertar("INSERT INTO cfamilias (familiafkcnfamilias,descripcionFamilia,usuariofkcpersonal,umfkcunidadmedida,empresa) VALUES('" + nombre + "',LTRIM(RTRIM('" + v.mayusculas(desc.ToLower()) + "')),'" + idUsuario + "','" + um + "','" + empresa + "')"))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Familias',(SELECT idfamilia FROM cfamilias WHERE familiafkcnfamilias='" + nombre + "' AND descripcionFamilia='" + desc + "' and empresa='" + empresa + "'),'" + nombre + ";" + desc + ";" + um + "','" + idUsuario + "',NOW(),'Inserción de Descripción de Familia','" + empresa + "','" + area + "')");
                    marcas cat = (marcas)Owner;
                    var ant = cat.cbfamilia.SelectedValue; var _desc = cat.cbdesc.SelectedValue;
                    v.iniCombos("SELECT idcnfamilia as id, familia FROM cnfamilias WHERE status='1'", cat.cbfamilia, "id", "familia", "--SELECCIONE UNA FAMILIA--");
                    cat.cbfamilia.SelectedValue = ant;
                    if (_desc != null) if (Convert.ToInt32(_desc) > 0) cat.cbdesc.SelectedValue = _desc;
                    MessageBox.Show("Familia Insertada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        void _editar()
        {
            int nombre = Convert.ToInt32(cbnombreFamilia.SelectedValue);
            string desc = v.mayusculas(txtdescfamilia.Text.ToLower()).Trim();
            int um = Convert.ToInt32(cbunidad.SelectedValue);
            if (this._status == 1)
            {
                if (!v.formulariofamilias(nombre, desc, um) && !v.existefamiliaActualizar(nombre, familiaAnterior, desc, descAnterior, empresa))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        if (v.c.insertar("UPDATE cfamilias SET familiafkcnfamilias ='" + nombre + "', descripcionFamilia='" + v.mayusculas(desc.ToLower()) + "',umfkcunidadmedida='" + um + "' WHERE idfamilia=" + this.idfamTemp))
                        {
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Familias','" + idfamTemp + "','" + familiaAnterior + ";" + descAnterior + ";" + umanterior + "','" + idUsuario + "',NOW(),'Actualización de Descripción de Familia','" + observaciones + "','" + empresa + "','" + area + "')"); marcas cat = (marcas)Owner;
                            var ant = cat.cbfamilia.SelectedValue;
                            object descAnt = null; if (cat.cbdesc.DataSource != null) descAnt = cat.cbdesc.SelectedValue;
                            v.iniCombos("SELECT idcnfamilia as id, familia FROM cnfamilias WHERE status='1'", cat.cbfamilia, "id", "familia", "--SELECCIONE UNA FAMILIA--");

                            cat.cbfamilia.SelectedValue = ant;
                            if (cat.cbdesc.DataSource != null) cat.cbdesc.SelectedValue = descAnt;
                            if (!yaAparecioMensaje) MessageBox.Show("Familia Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No se Puede Modificar una Familia Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button10_Click(null, e);
            else
                v.letrasynumeros(e);
        }
        public void insertarfamilias()
        {
            try
            {
                tbfamilias.Rows.Clear();
                string sql = "SELECT t1.idfamilia,upper(t4.Familia) as familia,UPPER(t1.descripcionfamilia) as descripcionfamilia,upper(t3.Simbolo) as simbolo,t3.idunidadmedida as id,t1.status,UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as nombre,t4.idcnfamilia FROM cfamilias as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona inner join cunidadmedida as t3 on t3.idunidadmedida=t1.umfkcunidadmedida INNER JOIN cnfamilias as t4 ON t1.familiafkcnfamilias = t4.idcnfamilia where t1.empresa='" + empresa + "' ORDER BY familia,descripcionfamilia ASC";
                MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbfamilias.Rows.Add(dr.GetString("idfamilia"), dr.GetString("familia"), dr.GetString("descripcionfamilia"), dr.GetString("simbolo"), dr.GetInt32("id"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")), dr.GetString("idcnfamilia"));
                }
                dr.Close();
                v.c.dbconection().Close();
                tbfamilias.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbfamilias_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfamilias.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void familias_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();

            if (Pconsultar)
            {
                insertarfamilias();
            }
        }

        private void tbfamilias_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (idfamTemp > 0 && _status == 1 && ((cbnombreFamilia.SelectedIndex > 0) && familiaAnterior != cbnombreFamilia.SelectedValue.ToString()) || (!string.IsNullOrWhiteSpace(txtdescfamilia.Text) && descAnterior != v.mayusculas(txtdescfamilia.Text.ToLower().Trim())) || (cbunidad.SelectedIndex > 0 && umanterior != Convert.ToInt32(cbunidad.SelectedValue)))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        button10_Click(null, e);
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
            limpiar();
            try
            {
                this.idfamTemp = Convert.ToInt32(tbfamilias.Rows[e.RowIndex].Cells[0].Value);
                _status = v.getStatusInt(tbfamilias.Rows[e.RowIndex].Cells[6].Value.ToString());

                if (Pdesactivar)
                {
                    pdeletefam.Visible = true;
                    if (_status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletefam.Text = "Reactivar";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletefam.Text = "Deactivar";
                    }

                }
                if (Peditar)
                {
                    cbnombreFamilia.SelectedValue = familiaAnterior = tbfamilias.Rows[e.RowIndex].Cells[7].Value.ToString();
                    if (cbnombreFamilia.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idcnfamilia as id,familia  FROM cnfamilias WHERE (status=1 OR idcnfamilia='" + familiaAnterior + "') and empresa='" + empresa + "' ORDER BY familia ASC", cbnombreFamilia, "id", "familia", "--SELECCIONE UN NOMBRE DE FAMILIA--");
                        cbnombreFamilia.SelectedValue = familiaAnterior;
                    }
                    txtdescfamilia.Text = descAnterior = v.mayusculas(tbfamilias.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    cbunidad.SelectedValue = umanterior = Convert.ToInt32(tbfamilias.Rows[e.RowIndex].Cells[4].Value.ToString());
                    if (cbunidad.SelectedIndex == -1)
                    {
                        v.iniCombos("Select t1.idunidadmedida as id, upper(t1.nombre) as simbolo from cunidadmedida as t1 where (t1.status='1' OR  idunidadmedida='" + umanterior + "') and empresa='" + empresa + "' order by t1.nombre asc;", cbunidad, "id", "simbolo", "-- Seleccione --");
                        cbunidad.SelectedValue = umanterior;
                    }
                    btnsave.BackgroundImage = Properties.Resources.pencil;
                    lblsave.Text = "Guardar";
                    gbaddfamilia.Text = "Actualizar Familia de Refacciones";
                    if (Pinsertar) pcancel.Visible = true;
                    editar = true; btnsave.Visible = lblsave.Visible = false;
                    if (_status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (_status == 1 && ((cbnombreFamilia.SelectedIndex > 0) && familiaAnterior != cbnombreFamilia.SelectedValue.ToString()) || (!string.IsNullOrWhiteSpace(txtdescfamilia.Text) && descAnterior != v.mayusculas(txtdescfamilia.Text.ToLower().Trim())) || (cbunidad.SelectedIndex > 0 && umanterior != Convert.ToInt32(cbunidad.SelectedValue)))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button10_Click(null, e);
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

        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            if (idfamTemp > 0)
            {
                string msg;

                int status;
                if (reactivar)
                {
                    status = 1;
                    msg = "Re";
                }
                else
                {
                    status = 0;
                    msg = "Des";

                }
                if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t2.status FROM cfamilias as t1 INNER JOIN cnfamilias as t2 ON t1.familiafkcnfamilias=t2.idcnfamilia WHERE t2.emprea='" + empresa + "' and t1.idfamilia='{0}'", idfamTemp))) == 0) MessageBox.Show("Error Al Reactivar la Descripcion de Familia:\nNombre de Familia Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t2.status FROM cfamilias as t1 INNER JOIN cunidadmedida as t2 ON t1.umfkcunidadmedida=t2.idunidadmedida WHERE t2.empresa='" + empresa + "' and t1.idfamilia='{0}'", idfamTemp))) == 0) MessageBox.Show("Error Al Reactivar la Descripcion de Familia:\nUnidad de Medida Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación De La Descripción de Familia";
                    obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 50, obs.lblinfo.Location.Y);
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                        try
                        {
                            String sql = "UPDATE cfamilias as t1 INNER JOIN cmarcas as t2 ON t2.descripcionfkcfamilias = t1.idfamilia SET t1.status = " + status + ",t2.status = " + status + " WHERE idfamilia  = " + this.idfamTemp;
                            if (v.c.insertar(sql))
                            {
                                var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Familias','" + this.idfamTemp + "','" + msg + "activación de Familia','" + idUsuario + "',NOW(),'" + msg + "activación de Unidad de Medida','" + edicion + "','" + empresa + "','" + area + "')");
                                MessageBox.Show("La Familia de Refacciones ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                                insertarfamilias();
                            }
                            else
                            {
                                MessageBox.Show("La Familia de Refacciones no ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void gbaddfamilia_Enter(object sender, EventArgs e)
        {

        }

        private void tbfamilias_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void gbaddfamilia_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void cbunidad_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cbunidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbunidad.SelectedIndex > 0)
            {
                lblSimbolo.Text = v.getaData("select upper(t1.simbolo) as simbolo from cunidadmedida as t1 where t1.idunidadmedida='" + cbunidad.SelectedValue + "' and empresa='" + empresa + "'").ToString();
            }
            else
            {
                lblSimbolo.Text = "";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void gbfamilias_Enter(object sender, EventArgs e)
        {

        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbnombreFamilia.SelectedIndex > 0 || cbunidad.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtdescfamilia.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((Convert.ToInt32(familiaAnterior) != (int)cbnombreFamilia.SelectedValue || umanterior != (int)cbunidad.SelectedValue || descAnterior!=v.mayusculas(txtdescfamilia.Text.Trim().ToLower())) && cbnombreFamilia.SelectedIndex > 0 && cbunidad.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtdescfamilia.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void btnaddpasillo_Click(object sender, EventArgs e)
        {
            ums cP = new ums(this.idUsuario, empresa, area,v);
            cP.Owner = this;
            cP.ShowDialog();
        }

        private void tbfamilias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pcancel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtnombre_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsave.Text = "Guardar";

                gbaddfamilia.Text = "Agregar Familia de Refacciones";
                cbnombreFamilia.Focus();
            }
            if (Pconsultar)
            {
                insertarfamilias();

            }
            cbunidad.SelectedIndex = 0;
            btnsave.Visible = true;
            lblsave.Visible = true;
            cbnombreFamilia.SelectedIndex = 0;
            txtdescfamilia.Clear();
            reactivar = false;
            idfamTemp = 0;
            pcancel.Visible = false;
            pdeletefam.Visible = false;
            yaAparecioMensaje = false;
            Unidad_Medida();
            marcas m = (marcas)Owner;
            m.insertarums();
        }
    }
}
