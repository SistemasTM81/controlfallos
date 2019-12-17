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
    public partial class catAnaqueles : Form
    {
        validaciones v;
        string idpasilloTemp;
        string pasilloAnterior;
        string pasilloValueAnterior, nivelAnterior;
        bool editar;
        int _state;
        int idUsuario;
        int empresa, area;
        bool yaAparecioMensaje;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        public void establecerPrivilegios()
        {
            object sql = v.getaData("SELECT privilegio  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catRefacciones'");
            if (sql != null)
            {
                string[] privilegios = sql.ToString().Split('/');
                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegios[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegios[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegios[2]));
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegios[3]));
                mostrar();
            }
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
                gbaddanaquel.Visible = true;
            if (Pconsultar)
                gbanaqueles.Visible = true;
            if (Peditar)
            {
                label3.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnsavemp.BackgroundImage = Properties.Resources.pencil;
                lblsavemp.Text = "Editar Anaquel";
                editar = true;
            }
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                int nivel;
                if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue); else nivel = 0;
                if (_state == 1 && ((cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtpasillo.Text) && nivel > 0) && (pasilloValueAnterior != cbpasillo.SelectedValue.ToString() || pasilloAnterior != txtpasillo.Text || !nivelAnterior.Equals(nivel.ToString()))))
                    btnsavemp.Visible = lblsavemp.Visible = true;
                else
                    btnsavemp.Visible = lblsavemp.Visible = false;
            }
        }
        public catAnaqueles(int idUsuario, int empresa, int area, validaciones v)
        {
            InitializeComponent();
            this.v = v;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cbpasillo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbubicaciones.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbubicaciones.ColumnHeadersDefaultCellStyle = d;
            cbnivel.DrawItem += new DrawItemEventHandler(v.combos_DrawItem);
            lbltitle.Left = (panel1.Width - lbltitle.Width) / 2;
            cbnivel.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        void _insertarPasillo()
        {
            int nivel = Convert.ToInt32(cbnivel.SelectedValue);

            string anaquel = txtpasillo.Text.Trim();
            if (v.formularioAnaquees(Convert.ToInt32(cbpasillo.SelectedValue), nivel, anaquel) && !v.existeAnaquel(nivel.ToString(), anaquel, empresa))
            {
                string sql = "INSERT INTO canaqueles (nivelfkcniveles,anaquel,usuariofkcpersonal,empresa) VALUES(LTRIM(RTRIM('" + cbnivel.SelectedValue + "')),LTRIM(RTRIM('" + anaquel + "')),'" + idUsuario + "','" + empresa + "')";
                if (v.c.insertar(sql))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Anaqueles',(SELECT idanaquel From canaqueles WHERE nivelfkcniveles='" + cbnivel.SelectedValue + "' AND anaquel='" + anaquel + "' and empresa='" + empresa + "'),'','" + idUsuario + "',NOW(),'Inserción de Anaquel','" + empresa + "','" + area + "')");
                    ubicaciones u = (ubicaciones)Owner;
                    var _nivel = 0;
                    var _anaquel = 0;
                    MessageBox.Show("Anaquel Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ubicaciones ub = (ubicaciones)this.Owner;
                    var _pasillo = ub.cbpasillo.SelectedValue; if (ub.cbniveles.SelectedIndex > 0) { _nivel = Convert.ToInt32(ub.cbniveles.SelectedValue); }
                    if (ub.cbanaquel.SelectedIndex > 0) { _anaquel = Convert.ToInt32(ub.cbanaquel.SelectedValue); }
                    limpiar();
                    ub.cbpasillo.SelectedValue = _pasillo;
                    if (_nivel > 0) { ub.cbniveles.SelectedValue = _nivel; } else { ub.cbniveles.DataSource = null; }
                    if (_anaquel > 0) { ub.cbanaquel.SelectedValue = _anaquel; } else { ub.cbanaquel.DataSource = null; }
                }
            }
        }
        public void busqUbic()
        {
            cbpasillo.DataSource = null;
            string sql = "SELECT idpasillo id, UPPER(pasillo) as nombre FROM cpasillos WHERE status = 1 and empresa='" + empresa + "' ORDER BY pasillo ASC";
            DataTable dt = new DataTable();
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataAdapter AdaptadorDatos = new MySqlDataAdapter(cm);
            DataRow nuevaFila = dt.NewRow();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbpasillo.DataSource = null;
            AdaptadorDatos.Fill(dt);
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione un Pasillo--".ToUpper();
            cbpasillo.DataSource = dt;
            cbpasillo.ValueMember = "id";
            cbpasillo.DisplayMember = "nombre";
            v.c.dbcon.Close();
            cbpasillo.SelectedIndex = 0;
        }
        object v_pasi, v_nivel, id_pt;
        void limpiar()
        {
            if (Pinsertar)
            {
                gbaddanaquel.Text = "Agregar Anaquel";
                btnsavemp.BackgroundImage = Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
                cbpasillo.Focus();
            }
            if (Pconsultar)
                insertarpasillos();
            v_pasi = cbpasillo.SelectedValue;
            v_nivel = cbnivel.SelectedValue;
            id_pt = idpasilloTemp;
            txtpasillo.Clear();
            idpasilloTemp = null;
            pdelete.Visible = false;
            pCancelar.Visible = false;
            pasilloAnterior = null;
            _state = 0;
            btnsavemp.Visible = lblsavemp.Visible = true;
            cbpasillo.SelectedIndex = 0;
            yaAparecioMensaje = false;
            busqUbic();
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            int nivel;
            if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue); else nivel = 0;
            if (_state == 1 && ((cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtpasillo.Text) && nivel > 0) && (pasilloValueAnterior != cbpasillo.SelectedValue.ToString() || pasilloAnterior != txtpasillo.Text || !nivelAnterior.Equals(nivel.ToString()))))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(sender, e);
                }
                else
                    limpiar();
            }
            else
                limpiar();
        }
        void _editarPasillo()
        {
            if (!string.IsNullOrWhiteSpace(idpasilloTemp))
            {
                string pasillo = txtpasillo.Text.Trim();
                if (v.formularioAnaquees(Convert.ToInt32(cbpasillo.SelectedValue), Convert.ToInt32(cbnivel.SelectedValue), pasillo) && !v.existeAnaquelActualizar(cbnivel.SelectedValue.ToString(), pasillo, pasilloAnterior, empresa))
                {
                    if (_state == 1)
                    {
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                            string sql = "UPDATE canaqueles SET nivelfkcniveles='" + cbnivel.SelectedValue + "',  anaquel= '" + pasillo + "' WHERE idanaquel= '" + idpasilloTemp + "'";
                            if (v.c.insertar(sql))
                            {
                                var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Anaqueles','" + idpasilloTemp + "','" + cbnivel.SelectedValue + ";" + pasilloAnterior + "','" + idUsuario + "',NOW(),'Actualización de Anaquel','" + observaciones + "','" + empresa + "','" + area + "')");
                                if (!yaAparecioMensaje) MessageBox.Show("Anaquel Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                ubicaciones ub = (ubicaciones)this.Owner;
                                var _pasillo = ub.cbpasillo.SelectedValue; var _nivel = ub.cbniveles.SelectedValue; var _anaquel = ub.cbanaquel.SelectedValue;
                                limpiar();
                                ub.cbpasillo.SelectedValue = _pasillo; ub.cbniveles.SelectedValue = _nivel; ub.cbanaquel.SelectedValue = _anaquel;
                            }
                        }
                    }
                    else
                        MessageBox.Show("No se Puede Actualizar un anaquel Desactivado Para el Sistema", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Seleccione Un Anaquel de la Lista Deplegable Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        public void insertarpasillos()
        {
            try
            {
                tbubicaciones.Rows.Clear();
                DataTable anaqueles = (DataTable)v.getData("SELECT idanaquel,(SELECT upper(pasillo) from cpasillos where idpasillo=pasillofkcpasillos),upper(nivel),upper(anaquel),upper((SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal)), if(t1.status=1,'ACTIVO',CONCAT('NO ACTIVO')),(SELECT upper(idpasillo) from cpasillos where idpasillo=pasillofkcpasillos),idnivel FROM canaqueles as t1 INNER JOIN cniveles as t2 ON t1.nivelfkcniveles =t2.idnivel where t1.empresa='" + empresa + "'");
                for (int i = 0; i < anaqueles.Rows.Count; i++)
                    tbubicaciones.Rows.Add(anaqueles.Rows[i].ItemArray);
                tbubicaciones.ClearSelection();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void gbClasificacion_Enter(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbpasillo.SelectedIndex > 0 || cbnivel.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtpasillo.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((Convert.ToInt32(pasilloValueAnterior) != (int)cbpasillo.SelectedValue || Convert.ToInt32(nivelAnterior) != (int)cbnivel.SelectedValue || pasilloAnterior!=txtpasillo.Text) && cbpasillo.SelectedIndex > 0 && cbnivel.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtpasillo.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }
        private void catAnaqueles_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar)
                insertarpasillos();
            if (Pconsultar)
                busqUbic();
            ubicaciones u = (ubicaciones)Owner;
            if (!string.IsNullOrWhiteSpace(u.nivelTemp))
            {
                cbpasillo.SelectedValue = v.getaData("SELECT pasillofkcpasillos FROM cniveles WHERE idnivel='" + u.nivelTemp + "'");
                cbnivel.SelectedValue = u.nivelTemp;
                u.nivelTemp = null;
            }
        }
        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    _insertarPasillo();
                else
                    _editarPasillo();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void tbubicaciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int nivel;
                if (cbnivel.DataSource != null) nivel = Convert.ToInt32(cbnivel.SelectedValue); else nivel = 0;
                if (!string.IsNullOrWhiteSpace(idpasilloTemp) && _state == 1 && ((cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtpasillo.Text) && nivel > 0) && (pasilloValueAnterior != cbpasillo.SelectedValue.ToString() || pasilloAnterior != txtpasillo.Text || !nivelAnterior.Equals(nivel.ToString()))))
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
            if (e.RowIndex >= 0)
            {
                limpiar();
                try
                {
                    idpasilloTemp = tbubicaciones.Rows[e.RowIndex].Cells[0].Value.ToString();
                    _state = v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[5].Value.ToString());
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
                        cbpasillo.SelectedValue = pasilloValueAnterior = tbubicaciones.Rows[e.RowIndex].Cells[6].Value.ToString();
                        if (cbpasillo.SelectedIndex == -1)
                        {
                            v.iniCombos("SELECT idpasillo id, UPPER(pasillo) as nombre FROM cpasillos WHERE (status = 1 OR idpasillo='" + pasilloValueAnterior + "') and empresa='" + empresa + "' ORDER BY pasillo ASC", cbpasillo, "id", "nombre", "-- SLEECCIONE UN PASILLO --");
                            cbpasillo.SelectedValue = pasilloValueAnterior;
                        }
                        cbnivel.SelectedValue = nivelAnterior = tbubicaciones.Rows[e.RowIndex].Cells[7].Value.ToString();
                        if (cbnivel.SelectedIndex == -1)
                        {
                            v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE pasillofkcpasillos='" + cbpasillo.SelectedValue + "' and (status='1' OR idnivel='" + nivelAnterior + "') and empresa='" + empresa + "'", cbnivel, "idnivel", "nivel", "--SELECCIONE UN NIVEL-");
                            cbnivel.SelectedValue = nivelAnterior;
                        }
                        pasilloAnterior = txtpasillo.Text = v.mayusculas(tbubicaciones.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                        btnsavemp.Visible = false;
                        lblsavemp.Visible = false;
                        btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                        lblsavemp.Text = "Guardar";
                        editar = true;
                        if (Pinsertar) pCancelar.Visible = true;
                        gbaddanaquel.Text = "Actualizar Anaquel";
                        btnsavemp.Visible = lblsavemp.Visible = false;
                        if (_state == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                        MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
        private void btndelpa_Click(object sender, EventArgs e)
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
                if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t3.status FROM canaqueles as t1 INNER JOIN cniveles as t2 ON t1.nivelfkcniveles=t2.idnivel INNER JOIN cpasillos as t3 ON t2.pasillofkcpasillos=t3.idpasillo WHERE t1.idanaquel={0}", idpasilloTemp))) == 0) MessageBox.Show("Error Al Reactivar El ANaquel:\nEl Pasillo Ha Sido Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t2.status FROM canaqueles as t1 INNER JOIN cniveles as t2 ON t1.nivelfkcniveles=t2.idnivel WHERE t1.idanaquel={0}", idpasilloTemp))) == 0) MessageBox.Show("Error Al Reactivar El ANaquel:\nEl Nivel Ha Sido Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación del Anaquel";
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        var res = v.c.insertar("UPDATE canaqueles as t3 LEFT JOIN ccharolas as t4 ON t4.anaquelfkcanaqueles=t3.idanaquel SET t3.status='" + status + "',t4.status='" + status + "' WHERE t3.idanaquel='" + this.idpasilloTemp + "'");
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Anaqueles','" + idpasilloTemp + "','" + msg + "activación de Anaquel','" + idUsuario + "',NOW(),'" + msg + "activación de Anaquel','" + edicion + "','" + empresa + "','" + area + "')");
                        MessageBox.Show("El Anaquel se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        msg = null;
                        status = 0;
                        ubicaciones u = (ubicaciones)Owner;
                        u.insertarUbicaciones();
                        u.busqUbic();
                        limpiar();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void tbubicaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbubicaciones.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }
        private void cbpasillo_DrawItem(object sender, DrawItemEventArgs e) { v.combos_DrawItem(sender, e); }
        private void gbaddanaquel_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        private void txtpasillo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsavemp_Click(null, e);
            else
                v.letrasynumeros(e);
        }
        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbpasillo.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE pasillofkcpasillos='" + cbpasillo.SelectedValue + "' and status='1' and empresa='" + empresa + "'", cbnivel, "idnivel", "nivel", "--SELECCIONE UN NIVEL-");
                cbnivel.Enabled = true;
            }
            else
            {
                cbnivel.DataSource = null;
                cbnivel.Enabled = false;
            }
        }
        private void lbltitle_MouseDown(object sender, MouseEventArgs e) { v.mover(sender, e, this); }
    }
}