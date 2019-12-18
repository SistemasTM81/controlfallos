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
    public partial class catPasillos : Form
    {
        validaciones v;
        string idpasilloTemp;
        string pasilloAnterior;
        bool editar;
        int _state;
        int idUsuario, empresa, area;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
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
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (_state == 1 && !string.IsNullOrWhiteSpace(txtpasillo.Text) && pasilloAnterior != txtpasillo.Text.Trim())
                {
                    btnsavemp.Visible = lblsavemp.Visible = true;
                }
                else
                {
                    btnsavemp.Visible = lblsavemp.Visible = false;
                }
            }
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddpasillo.Visible = true;
            }
            if (Pconsultar)
            {
                gbpasillos.Visible = true;
            }
            if (Peditar)
            {
                label2.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Anaquel";
                editar = true;
            }
        }
        public catPasillos(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            tbubicaciones.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
        }
        void _insertarPasillo()
        {

            string pasillo = txtpasillo.Text;
            if (!string.IsNullOrWhiteSpace(pasillo))
            {
                if (!v.existePasillo(pasillo, empresa))
                {

                    string sql = "INSERT INTO cpasillos (pasillo,usuariofkcpersonal,empresa) VALUES(LTRIM(RTRIM('" + pasillo + "')),'" + this.idUsuario + "','" + empresa + "')";
                    if (v.c.insertar(sql))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Pasillos',(SELECT idpasillo From cpasillos WHERE pasillo='" + pasillo + "' and empresa='" + empresa + "'),'" + pasillo + "','" + idUsuario + "',NOW(),'Inserción de Pasillo','" + empresa + "','" + area + "')");
                        ubicaciones u = (ubicaciones)Owner;
                        u.pasilloTemp = v.getaData("SELECT idpasillo From cpasillos WHERE pasillo='" + pasillo + "'").ToString();
                        MessageBox.Show("Pasillo Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var _pasillo = u.cbpasillo.SelectedValue; var _nivel = u.cbniveles.SelectedValue; var _anaquel = u.cbanaquel.SelectedValue;
                        limpiar();
                        ubicaciones ub = (ubicaciones)this.Owner;
                        ub.busqUbic();
                        ub.cbpasillo.SelectedValue = _pasillo;
                        ub.cbniveles.SelectedValue = _nivel;
                        ub.cbanaquel.SelectedValue = _anaquel;
                    }
                }
            }
            else
            {
                MessageBox.Show("El campo \"pasillo\" no puede quedar vacio", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void _editarPasillo()
        {
            if (!string.IsNullOrWhiteSpace(idpasilloTemp))
            {
                string pasillo = txtpasillo.Text;
                if (!string.IsNullOrWhiteSpace(pasillo))
                {
                    if (pasillo.Equals(pasilloAnterior))
                    {
                        MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    }
                    else
                    {

                        if (!v.existePasilloActualizar(pasillo, pasilloAnterior, empresa))
                        {
                            if (_state == 1)
                            {
                                observacionesEdicion obs = new observacionesEdicion(v);
                                obs.Owner = this;
                                if (obs.ShowDialog() == DialogResult.OK)
                                {
                                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                                    string sql = "UPDATE cpasillos SET pasillo= LTRIM(RTRIM('" + pasillo + "')) WHERE idpasillo= '" + this.idpasilloTemp + "'";
                                    if (v.c.insertar(sql))
                                    {
                                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Pasillos','" + idpasilloTemp + "','" + pasilloAnterior + "','" + idUsuario + "',NOW(),'Actualización de Pasillo','" + observaciones + "','" + empresa + "','" + area + "')");
                                        if (!yaAparecioMensaje) MessageBox.Show("Pasillo Acualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        ubicaciones u = (ubicaciones)Owner;
                                        var res = u.cbpasillo.SelectedValue; var _nivel = u.cbniveles.SelectedValue; var _anaquel = u.cbanaquel.SelectedValue;
                                        limpiar();
                                        u.insertarUbicaciones();
                                        u.busqUbic();
                                        u.cbpasillo.SelectedValue = res;
                                        u.cbniveles.SelectedValue = _nivel;
                                        u.cbanaquel.SelectedValue = _anaquel;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se Puede Actualizar un Pasillo Desactivado Para el Sistema", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Se puede Editar el Pasillo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un Pasillo de La Tabla Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                gbaddpasillo.Text = "Agregar Pasillo";
                lblsavemp.Text = "Agregar";
                btnsavemp.Visible = lblsavemp.Visible = true;
                editar = false;
                txtpasillo.Focus();
            }
            if (Pconsultar)
            {
                insertarpasillos();
            }
            txtpasillo.Clear();
            idpasilloTemp = null;
            pdelete.Visible = false;
            pCancelar.Visible = false;
            yaAparecioMensaje = false;
            idpasilloTemp = null;
            pasilloAnterior = null;
            ubicaciones ub = (ubicaciones)Owner;
            ub.iniCombos("SELECT idpasillo, pasillo FROM cpasillos where status='1'  and empresa='" + empresa + "' order by pasillo", ub.cbpasillo, "idpasillo", "pasillo", "-SELECCIONE PASILLO-");
            _state = 0;
        }
        public void insertarpasillos()
        {
            try
            {
                tbubicaciones.Rows.Clear();
                string sql = "SELECT t1.idpasillo as id,t1.pasillo as p,t1.status as s, UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as nombres FROM cpasillos as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal=t2.idpersona where t1.empresa='" + empresa + "'";
                MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
                MySqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbubicaciones.Rows.Add(dr.GetString("id"), dr.GetString("p"), dr.GetString("nombres"), v.getStatusString(dr.GetInt32("s")));
                }
                tbubicaciones.ClearSelection();
                v.c.dbcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtpasillo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsavemp_Click(null, e);
            else v.letrasynumeros(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtpasillo.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (pasilloAnterior != txtpasillo.Text)
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    _insertarPasillo();
                }
                else
                {
                    _editarPasillo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void catPasillos_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pconsultar)
            {
                insertarpasillos();
            }
        }

        private void tbubicaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbubicaciones.Columns[e.ColumnIndex].Name == "Estatus")
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;

                if (this._state == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                }

                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Pasillo";
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    v.c.insertar("UPDATE cpasillos as t1 LEFT JOIN cniveles as t2 On t2.pasillofkcpasillos=t1.idpasillo LEFT JOIN canaqueles as t3 ON t3.nivelfkcniveles=t2.idnivel LEFT JOIN ccharolas as t4 ON t4.anaquelfkcanaqueles=t3.idanaquel SET t1.status='" + status + "', t2.status='" + status + "', t3.status='" + status + "',t4.status='" + status + "' WHERE t1.idpasillo='" + this.idpasilloTemp + "'");


                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Pasillos','" + this.idpasilloTemp + "','" + msg + "activación de Pasillo','" + idUsuario + "',NOW(),'" + msg + "activación de Pasillo','" + edicion + "','" + empresa + "','" + area + "')");

                    MessageBox.Show("El Pasillo se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    msg = null;
                    status = 0;
                    ubicaciones u = (ubicaciones)Owner;
                    u.insertarUbicaciones();
                    u.busqUbic();
                    limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (!pasilloAnterior.Equals(txtpasillo.Text))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(sender, e);
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

        private void tbubicaciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (!string.IsNullOrWhiteSpace(idpasilloTemp) && Peditar && !pasilloAnterior.Equals(txtpasillo.Text) && _state == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null, e);
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
                idpasilloTemp = tbubicaciones.Rows[e.RowIndex].Cells[0].Value.ToString();
                _state = v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (Pdesactivar)
                {


                    if (_state == 0)
                    {

                        btndelpa.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelpa.Text = "Reactivar";
                    }
                    else
                    {

                        btndelpa.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelpa.Text = "Desactivar";
                    }
                    pdelete.Visible = true;

                }
                if (Peditar)
                {
                    pasilloAnterior = txtpasillo.Text = (string)tbubicaciones.Rows[e.RowIndex].Cells[1].Value;
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    if (Pinsertar) pCancelar.Visible = true;
                    gbaddpasillo.Text = "Actualizar Pasillo";
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    if (_state == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gbaddpasillo_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void tbubicaciones_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void gbaddpasillo_Enter(object sender, EventArgs e)
        {

        }

        private void tbubicaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }
    }
}
