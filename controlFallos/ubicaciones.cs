using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class ubicaciones : Form
    {
        string idCharolaAnterior;
        int pasilloAnterior, anaquelAnterior, nivelAnterior;
        string charolaAnterior;
        int _status;
        bool editar;
        int idUsuario;
        int empresa, area;
        public string pasilloTemp, nivelTemp;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;

        validaciones v;
        new catRefacciones Owner;
        public ubicaciones(int idUsuario, int empresa, int area, Form fh, validaciones v)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cbpasillo.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbanaquel.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbubicaciones.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbniveles.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.Owner = (catRefacciones)fh;
            this.v = v;
        }
        public void iniCombos(string sql, ComboBox cbx, string ValueMember, string DisplayMember, string TextoInicial)
        {
            cbx.DataSource = null;
            DataTable dt = (DataTable)v.getData(sql);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila[ValueMember] = 0;
            nuevaFila[DisplayMember] = TextoInicial.ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbx.DisplayMember = DisplayMember;
            cbx.ValueMember = ValueMember;
            cbx.DataSource = dt;
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
                int nivel = 0; if (cbniveles.DataSource != null) nivel = Convert.ToInt32(cbniveles.SelectedValue);
                int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
                string charola = txtcharola.Text.Trim();
                if (_status == 1 && (pasillo > 0 && nivel > 0 && anaquel > 0 && !string.IsNullOrWhiteSpace(charola)) && (pasilloAnterior != pasillo || nivel != nivelAnterior || anaquel != anaquelAnterior || !charola.Equals(charolaAnterior)))
                {
                    btnsave.Visible = lblsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = lblsave.Visible = false;
                }
            }
            else
            {
                btnsave.Visible = lblsave.Visible = true;

            }
        }
        public void establecerPrivilegios()
        {
            object sql = v.getaData("SELECT CONCAT(insertar,';',consultar,';',editar,';',desactivar)  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catRefacciones'");
            if (sql != null)
            {
                string[] privilegios = sql.ToString().Split(';');
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
            gbaddubicacion.Visible = (Pinsertar || Peditar);
            gbubicaciones.Visible = (Pconsultar);
            label23.Visible = label22.Visible = (Peditar);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            catPasillos cP = new catPasillos(this.idUsuario, empresa, area);
            cP.Owner = this;
            cP.ShowDialog();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int nivel = 0; if (cbniveles.DataSource != null) nivel = Convert.ToInt32(cbniveles.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            string charola = txtcharola.Text.Trim();
            if (_status == 1 && (pasillo > 0 && nivel > 0 && anaquel > 0 && !string.IsNullOrWhiteSpace(charola)) && (pasilloAnterior != pasillo || nivel != nivelAnterior || anaquel != anaquelAnterior || !charola.Equals(charolaAnterior)))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button9_Click(null, e);
                }
            }
            limpiar();
        }

        private void ubicaciones_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar)
                busqUbic();
            if (Pconsultar)
                insertarUbicaciones();
        }
        public void busqUbic(){v.iniCombos("SELECT idpasillo,pasillo FROM cpasillos WHERE status='1' and empresa='" + empresa + "' ORDER BY pasillo ASC", cbpasillo, "idpasillo", "pasillo", "-- seleccione pasillo --");}
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    _insertarCharola();
                else
                    _editarCharola();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void _insertarCharola()
        {
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int nivel = 0; if (cbniveles.DataSource != null) nivel = Convert.ToInt32(cbniveles.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            string charola = txtcharola.Text.Trim();
            if (v.formularioUbicaciones(pasillo, nivel, anaquel, charola) && !v.existeCharola(anaquel, charola, empresa))
            {
                if (v.c.insertar("INSERT INTO ccharolas (charola, anaquelfkcanaqueles,empresa) VALUES (LTRIM(RTRIM('" + charola + "')),'" + anaquel + "','" + empresa + "')"))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones',(SELECT idcharola FROM ccharolas WHERE anaquelfkcanaqueles='" + cbanaquel.SelectedValue + "' and charola='" + charola + "' and empresa='" + empresa + "'),'','" + idUsuario + "',NOW(),'Inserción de Ubicación','" + empresa + "','" + area + "')");
                    Owner.charola = v.getaData("SELECT idcharola FROM ccharolas WHERE anaquelfkcanaqueles='" + cbanaquel.SelectedValue + "' and charola='" + charola + "'").ToString();

                    MessageBox.Show("Ubicacion Insertada Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busqUbic();
                    limpiar();
                }
            }

        }
        void _editarCharola()
        {
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            int nivel = 0; if (cbniveles.DataSource != null) nivel = Convert.ToInt32(cbniveles.SelectedValue);
            int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
            string charola = txtcharola.Text.Trim();
            if (v.formularioUbicaciones(pasillo, nivel, anaquel, charola) && !v.existeCharolaActualizar(anaquel, charola, this.charolaAnterior, empresa))
            {
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    var res = v.c.insertar("UPDATE ccharolas SET anaquelfkcanaqueles='" + anaquel + "', charola= LTRIM(RTRIM('" + charola + "')) WHERE idcharola ='" + idCharolaAnterior + "'");
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones','" + idCharolaAnterior + "','" + anaquelAnterior + ";" + charolaAnterior + "','" + idUsuario + "',NOW(),'Actualización de Ubicación','" + observaciones + "','" + empresa + "','" + area + "')");
                    if (!yaAparecioMensaje) MessageBox.Show("Ubicación Actualizada", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }

        }
        private void cbpasillo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cniveles where pasillofkcpasillos ='" + cbpasillo.SelectedValue + "' and empresa='" + empresa + "'")) > 0)
            {

                v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE status='1' and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "' and empresa='" + empresa + "' ORDER BY nivel ASC", cbniveles, "idnivel", "nivel", "--SELECCIONE UN NIVEL");
                cbniveles.Enabled = true;
            }
            else
            {

                cbniveles.DataSource = null;
                cbniveles.Enabled = false;
            }
        }

        private void btnaddanaquel_Click(object sender, EventArgs e)
        {
            catAnaqueles cat = new catAnaqueles(this.idUsuario, empresa, area,v);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void txtcharola_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button9_Click(null, e);
            else
                v.letrasynumeros(e);
        }

        void limpiar()
        {
            cbpasillo.SelectedIndex = 0;
            txtcharola.Clear();

            pdelete.Visible = false;
            padd.Visible = false;
            idCharolaAnterior = null;
            pasilloAnterior = 0;
            anaquelAnterior = 0;
            nivelAnterior = 0;
            charolaAnterior = null;
            btnsave.Visible = lblsave.Visible = true;
            if (Pinsertar)
            {
                btnsave.BackgroundImage = Properties.Resources.save;
                gbaddubicacion.Text = "Agregar Ubicación";
                lblsave.Text = "Agregar";
                editar = false;
                cbpasillo.Focus();
            }
            if (Pconsultar)
            {
                insertarUbicaciones();
            }
            busqUbic();


        }

        public void insertarUbicaciones()
        {
            tbubicaciones.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT t1.idcharola,UPPER(t4.pasillo),UPPER(t3.nivel),UPPER(t2.anaquel),UPPER(t1.charola),if(t1.status=1,UPPER('Activo'),UPPER('No Activo')),t4.idpasillo,t3.idnivel,t2.idanaquel FROM ccharolas AS t1 INNER JOIN canaqueles AS t2 ON t1.anaquelfkcanaqueles=t2.idanaquel INNER JOIN cniveles as t3 ON t2.nivelfkcniveles=t3.idnivel INNER JOIN cpasillos as t4 ON t3.pasillofkcpasillos=t4.idpasillo where t1.empresa='" + empresa + "' ORDER BY pasillo,nivel,anaquel,charola ASC");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tbubicaciones.Rows.Add(dt.Rows[i].ItemArray);
            }
            tbubicaciones.ClearSelection();
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

        private void pEliminarClasificacion_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btndelcla_Click(object sender, EventArgs e)
        {
            if (Pdesactivar)
            {
                if (!string.IsNullOrWhiteSpace(idCharolaAnterior))
                {
                    try
                    {
                        string msg;
                        int status;

                        if (this._status == 0)
                        {
                            msg = "Re";
                            status = 1;
                        }
                        else
                        {
                            msg = "Des";

                            status = 0;
                        }

                        if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t4.status from ccharolas as t1 INNER JOIN canaqueles as t2 On t1.anaquelfkcanaqueles = t2.idanaquel INNER JOIN cniveles as t3 ON t2.nivelfkcniveles=t3.idnivel INNER JOIN cpasillos as t4 On t3.pasillofkcpasillos =t4.idpasillo WHERE t4.empresa='" + empresa + "' and idcharola='{0}'", idCharolaAnterior))) == 0) MessageBox.Show("Error Al Reactivar:\nPasillo Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t3.status from ccharolas as t1 INNER JOIN canaqueles as t2 On t1.anaquelfkcanaqueles = t2.idanaquel INNER JOIN cniveles as t3 ON t2.nivelfkcniveles=t3.idnivel WHERE t3.empresa='" + empresa + "' and idcharola='{0}'", idCharolaAnterior))) == 0) MessageBox.Show("Error Al Reactivar:\nNivel Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t2.status from ccharolas as t1 INNER JOIN canaqueles as t2 On t1.anaquelfkcanaqueles = t2.idanaquel WHERE t2.empresa='" + empresa + "' and idcharola='{0}'", idCharolaAnterior))) == 0) MessageBox.Show("Error Al Reactivar:\nAnaquel Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación De La Ubicación ";
                            obs.lblinfo.Location = new Point(obs.lblinfo.Location.X + 5, obs.lblinfo.Location.Y);
                            if (obs.ShowDialog() == DialogResult.OK)
                            {
                                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                v.c.insertar("UPDATE ccharolas SET status = '" + status + "' WHERE idcharola=" + this.idCharolaAnterior);
                                var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones','" + idCharolaAnterior + "','" + msg + "activación de Ubicacón','" + idUsuario + "',NOW(),'" + msg + "activación de Ubicación','" + edicion + "','" + empresa + "','" + area + "')");
                                limpiar();
                                MessageBox.Show("La Ubicacion se ha " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    MessageBox.Show("Seleccione una Ubicacion para Desactivar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbpasillo_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void tbubicaciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void tbubicaciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
                int nivel = 0; if (cbniveles.DataSource != null) nivel = Convert.ToInt32(cbniveles.SelectedValue);
                int anaquel = 0; if (cbanaquel.DataSource != null) anaquel = Convert.ToInt32(cbanaquel.SelectedValue);
                string charola = txtcharola.Text.Trim();
                if (_status == 1 && (pasillo > 0 && nivel > 0 && anaquel > 0 && !string.IsNullOrWhiteSpace(charola)) && (pasilloAnterior != pasillo || nivel != nivelAnterior || anaquel != anaquelAnterior || !charola.Equals(charolaAnterior)))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        button9_Click(null, e);
                    }
                }
                guardarReporte(e);
            }
        }

        private void gbubicaciones_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            catNiveles cat = new catNiveles(empresa, area, idUsuario);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void gbaddubicacion_Enter(object sender, EventArgs e)
        {

        }

        private void cbniveles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0 && Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM canaqueles where nivelfkcniveles ='" + cbniveles.SelectedValue + "' and empresa='" + empresa + "'")) > 0)
            {

                v.iniCombos("SELECT idanaquel,UPPER(anaquel) AS anaquel FROM canaqueles WHERE status='1' and nivelfkcniveles= '" + cbniveles.SelectedValue + "' and empresa='" + empresa + "' ORDER BY anaquel ASC", cbanaquel, "idanaquel", "anaquel", "--SELECCIONE UN ANAQUEL");
                cbanaquel.Enabled = true;
            }
            else
            {

                cbanaquel.DataSource = null;
                cbanaquel.Enabled = false;
            }
        }

        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                idCharolaAnterior = tbubicaciones.Rows[e.RowIndex].Cells[0].Value.ToString();
                _status = v.getStatusInt(tbubicaciones.Rows[e.RowIndex].Cells[5].Value.ToString());
                if (Pdesactivar)
                {
                    if (_status == 0)
                    {

                        btndelcha.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelcha.Text = "Reactivar";
                    }
                    else
                    {

                        btndelcha.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelcha.Text = "Desactivar";

                    }
                    pdelete.Visible = true;
                }
                if (Peditar)
                {
                    cbpasillo.SelectedValue = pasilloAnterior = Convert.ToInt32(tbubicaciones.Rows[e.RowIndex].Cells[6].Value);
                    if (cbpasillo.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idpasillo,pasillo FROM cpasillos WHERE (status='1' OR idpasillo='" + pasilloAnterior + "') and empresa='" + empresa + "' ORDER BY pasillo ASC", cbpasillo, "idpasillo", "pasillo", "-- seleccione pasillo --");
                        cbpasillo.SelectedValue = pasilloAnterior;
                    }
                    cbniveles.SelectedValue = nivelAnterior = Convert.ToInt32(tbubicaciones.Rows[e.RowIndex].Cells[7].Value);
                    if (cbniveles.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idnivel,UPPER(nivel) AS nivel FROM cniveles WHERE (status='1' OR idnivel='" + nivelAnterior + "') and pasillofkcpasillos = '" + cbpasillo.SelectedValue + "' and empresa='" + empresa + "' ORDER BY nivel ASC", cbniveles, "idnivel", "nivel", "--SELECCIONE UN NIVEL");
                        cbniveles.SelectedValue = nivelAnterior;
                    }

                    cbanaquel.SelectedValue = anaquelAnterior = Convert.ToInt32(tbubicaciones.Rows[e.RowIndex].Cells[8].Value);
                    if (cbanaquel.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idanaquel,UPPER(anaquel) AS anaquel FROM canaqueles WHERE (status='1' OR idanaquel ='" + anaquelAnterior + "') and nivelfkcniveles= '" + cbniveles.SelectedValue + "' and empresa='" + empresa + "' ORDER BY anaquel ASC", cbanaquel, "idanaquel", "anaquel", "--SELECCIONE UN ANAQUEL");
                        cbanaquel.SelectedValue = anaquelAnterior;
                    }

                    txtcharola.Text = charolaAnterior = (string)tbubicaciones.Rows[e.RowIndex].Cells[4].Value;
                    editar = true;
                    if (Pinsertar) padd.Visible = true;
                    btnsave.BackgroundImage = Properties.Resources.pencil;
                    lblsave.Text = "Guardar";
                    gbaddubicacion.Text = "Actualizar Ubicación";
                    btnsave.Visible = lblsave.Visible = false;
                    if (_status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
