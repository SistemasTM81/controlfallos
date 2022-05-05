using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace controlFallos
{
    public partial class ums : Form
    {
        validaciones v;
        int idumTemp;
        bool reactivar;
        string nombreAnterior, simboloAnterior;
        int idUsuario;
        int status, empresa, area;
        bool editar;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        public ums(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            tbum.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (status == 1 && ((!string.IsNullOrWhiteSpace(txtumedida.Text) && nombreAnterior != v.mayusculas(txtumedida.Text.ToLower().Trim())) || (!string.IsNullOrWhiteSpace(txtsimbolo.Text)) && simboloAnterior != txtsimbolo.Text.Trim()))
                    btnsave.Visible = lblsave.Visible = true;
                else
                    btnsave.Visible = lblsave.Visible = false;
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
                if (privilegiosTemp.Length > 3)
                {
                    Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }
            }
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
                gbaddum.Visible = true;
            if (Pconsultar)
                gbum.Visible = true;
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    insertar();
                else
                    _editar();
            }
            catch (Exception ex){MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);}
        }
        void _editar()
        {
            string um = v.mayusculas(txtumedida.Text.ToLower());
            string _simbolo = txtsimbolo.Text;
            if (this.status == 1)
            {
                if (um.Equals(nombreAnterior) && _simbolo.Equals(simboloAnterior))
                {
                    MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){limpiar();}
                }
                else
                {
                    if (!v.formularioums(um, _simbolo) && !v.existeUMActualizar(um, this.nombreAnterior, _simbolo, this.simboloAnterior, this.idumTemp, empresa))
                    {
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                            var res = v.c.insertar("UPDATE cunidadmedida SET Nombre =LTRIM(RTRIM('" + um + "')), Simbolo =LTRIM(RTRIM('" + _simbolo + "')) WHERE idunidadmedida=" + this.idumTemp);
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Unidad de Medida','" + idumTemp + "','" + nombreAnterior + ";" + simboloAnterior + "','" + idUsuario + "',NOW(),'Actualización de Unidad de Medida','" + observaciones + "','" + empresa + "','" + area + "')");
                            if (!yaAparecioMensaje) MessageBox.Show("Se ha Actualizado la Unidad de Medida Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            familias fm = (familias)this.Owner;
                            object _id = fm.cbunidad.SelectedValue;
                            var _familia = fm.cbnombreFamilia.SelectedValue;
                            fm.Unidad_Medida();
                            fm.cbunidad.SelectedValue = _id;
                            fm.cbnombreFamilia.SelectedValue = _familia;
                        }
                        limpiar();
                    }
                }
            }
            else
            {
                MessageBox.Show("No se Puede Modificar una Unidad de Medida Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar()
        {

            string um = v.mayusculas(txtumedida.Text.ToLower());
            string _simbolo = txtsimbolo.Text.ToLower();
            this.idumTemp = 0;
            if (!v.formularioums(um, _simbolo) && !v.existeUM(um, _simbolo, this.idumTemp, empresa))
            {
                if (v.c.insertar("INSERT INTO cunidadmedida(Nombre,Simbolo,usuariofkcpersonal,empresa) VALUES(LTRIM(RTRIM('" + um + "')),LTRIM(RTRIM('" + _simbolo + "')),'" + this.idUsuario + "','" + empresa + "')"))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Unidad de Medida',(SELECT idunidadmedida FROM cunidadmedida WHERE Nombre='" + um + "' AND Simbolo = '" + _simbolo + "' and empresa='" + empresa + "'),'" + um + ";" + _simbolo + "','" + idUsuario + "',NOW(),'Inserción de Unidad de Medida','" + empresa + "','" + area + "')");
                    familias fm = (familias)this.Owner;
                    object _id = fm.cbunidad.SelectedValue;
                    var _familia = fm.cbnombreFamilia.SelectedValue;
                    fm.Unidad_Medida();
                    fm.cbunidad.SelectedValue = _id;
                    fm.cbnombreFamilia.SelectedValue = _familia;

                    MessageBox.Show("Unidad de Medida Insertada", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }
        private void txtumedida_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                button5_Click(null, e);
            else v.paraUM(e);
        }
        public void insertarums()
        {
            tbum.Rows.Clear();
            string sql = "SELECT t1.idunidadmedida,upper(t1.Nombre) as Nombre,upper(t1.Simbolo) as simbolo,t1.status,UPPER(CONCAT(coalesce(t2.nombres,''),' ',coalesce(t2.apPaterno,''),' ',coalesce(t2.apMaterno,''))) as nombre FROM cunidadmedida as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona where t1.empresa='" + empresa + "'";
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbum.Rows.Add(dr.GetString("idunidadmedida"), dr.GetString("Nombre"), dr.GetString("Simbolo"), dr.GetString("nombre"), v.getStatusString(dr.GetInt32("status")));
            }
            dr.Close();
            v.c.dbcon.Close();
            tbum.ClearSelection();
        }

        private void tbum_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbum.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }

        private void ums_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pconsultar)
                insertarums();
        }

        private void tbum_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string um = v.mayusculas(txtumedida.Text.ToLower());
                string _simbolo = txtsimbolo.Text;
                if (idumTemp > 0 && (!um.Equals(nombreAnterior) || !_simbolo.Equals(simboloAnterior)) && status == 1 && Peditar)

                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        button5_Click(null, e);
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
                idumTemp = Convert.ToInt32(tbum.Rows[e.RowIndex].Cells[0].Value);
                status = v.getStatusInt(tbum.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (Pdesactivar)
                {
                    pdeleteum.Visible = true;
                    if (status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeleteum.Text = "Reactivar";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeleteum.Text = "Deactivar";
                    }
                }
                if (Peditar)
                {
                    editar = true;
                    txtumedida.Text = nombreAnterior = v.mayusculas(tbum.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    txtsimbolo.Text = simboloAnterior = (string)tbum.Rows[e.RowIndex].Cells[2].Value;
                    if (Pinsertar) padd.Visible = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Editar";
                    gbaddum.Text = "Actualizar Unidad de Medida";
                }
            }
            catch (Exception ex){MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);}
        }
        private void btncancel_Click(object sender, EventArgs e)
        {
            string um = v.mayusculas(txtumedida.Text.ToLower());
            string _simbolo = txtsimbolo.Text;
            if ((!um.Equals(nombreAnterior) || !_simbolo.Equals(simboloAnterior)) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button5_Click(null, e);
                }
                else
                    limpiar();
            }
            else
            {
                limpiar();
            }
        }

        private void btndeleteuser_Click(object sender, EventArgs e)
        {

            if (idumTemp > 0)
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
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación De La Unidad de Medida";
                obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 55, obs.lblinfo.Location.Y);
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    try
                    {
                        String sql = "UPDATE cunidadmedida SET status = " + status + " WHERE idunidadmedida  = " + this.idumTemp;
                        if (v.c.insertar(sql))
                        {
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Unidad de Medida','" + this.idumTemp + "','" + msg + "activación de Unidad de Medida','" + idUsuario + "',NOW(),'" + msg + "activación de Unidad de Medida','" + edicion + "','" + empresa + "','" + area + "')");
                            MessageBox.Show("La Unidad de Medida ha sido " + msg + "activado", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            insertarums();
                        }
                        else
                            MessageBox.Show("La Unidad de Medida no ha sido " + msg);

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
        }

        private void gbaddum_Enter(object sender, EventArgs e){}

        private void gbaddum_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtumedida.Text) || !string.IsNullOrWhiteSpace(txtsimbolo.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((nombreAnterior != v.mayusculas(txtumedida.Text.Trim().ToLower()) || simboloAnterior != txtsimbolo.Text.Trim()) && !string.IsNullOrWhiteSpace(txtumedida.Text) && !string.IsNullOrWhiteSpace(txtsimbolo.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void tbum_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtumedida_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsave.Text = "Agregar";
                gbaddum.Text = "Agregar Unidad de Medida";
                txtumedida.Focus();
            }
            if (Pconsultar)
                insertarums();
            padd.Visible = false;
            yaAparecioMensaje = false;
            pdeleteum.Visible = false;
            idumTemp = 0;
            reactivar = false;
            txtsimbolo.Clear();
            txtumedida.Clear();
            btnsave.Visible = lblsave.Visible = true;

        }
    }
}