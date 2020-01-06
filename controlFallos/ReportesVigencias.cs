using System.Globalization;
using MySql.Data.MySqlClient;
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
    public partial class ReportesVigencias : Form
    {
        validaciones v;

        int reporte, empresa, area, idusuario, contadortxt, contadoralltxt, idencabezadoreporte, idUsuarioP;

        //VARIABLES ANTERIORES

        string nombreanterior, codigoanterior, fechaAnterior, revisionanterior;

        bool peditar { get; set; }

        public ReportesVigencias(int empresa, int area, int idUsuarioP,validaciones v)
        {
            InitializeComponent();
            this.v = v;
            this.empresa = empresa;
            this.area = area;
            this.idUsuarioP = idUsuarioP;
        }

        private void ReportesVigencias_Load(object sender, EventArgs e)
        {
            privilegios();
            dtpVigencia.Value = DateTime.Now;
            txtNombreReporte.Focus();
            if (!rbtnHojaPert.Checked)
            {
                rbtnHojaPert.ForeColor = rbtnHojaPert.Checked ? Color.Crimson : Color.Crimson;
                rbtnReporteP.ForeColor = rbtnReporteP.Checked ? Color.Crimson : Color.Crimson;
                rbtnIncidenciaP.ForeColor = rbtnIncidenciaP.Checked ? Color.Crimson : Color.Crimson;
            }
            if (!peditar)
                btnEditar.Visible = label9.Visible = false;
        }

        // EVENTOS

        public void privilegios()
        {
            string sql = "SELECT CONCAT(editar) as privilegios FROM privilegios where usuariofkcpersonal = '" + idUsuarioP + "' and namform = 'repPersonal'";
            string[] privilegios = getaData(sql).ToString().Split(';');
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[0]));
        }

        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }

        public object getaData(string sql)
        {
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            var res = cm.ExecuteScalar();
            v.c.dbconection();
            return res;
        }

        public void txtallnum_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        public void txtallcodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || e.KeyChar == 45 || e.KeyChar == 127 || e.KeyChar == 08)
                e.Handled = false;
            else
            {
                e.Handled = true;
                MessageBox.Show("Sólo se Aceptan Letras, Números y Guiones En Este Campo", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void txtall_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void textBoxUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnEditar.Focus();
            else
                v.letrasynumerossinespacios(e);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarbtn();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (label9.Text == "GUARDAR")
                guardar(reporte);
            else if (label9.Text == "EDITAR")
                editar(reporte);
        }

        private void txtPassUsuario_Leave(object sender, EventArgs e)
        {
            MySqlCommand passusuario = new MySqlCommand("SELECT t1.idPersona, coalesce(UPPER(CONCAT(t1.ApPaterno, ' ', t1.ApMaterno, ' ', t1.nombres)), '') AS nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(txtPassUsuario.Text) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'", v.c.dbconection());
            MySqlDataReader dr = passusuario.ExecuteReader();
            if (dr.Read())
            {
                idusuario = dr.GetInt32("idPersona");
                lblUsuario.Text = dr.GetString("nombre");
                lblUsuario.ForeColor = Color.FromArgb(75, 44, 52);
            }
            else
            {
                lblUsuario.Text = "DATOS INCORRECTOS";
                lblUsuario.ForeColor = Color.Crimson;
            }
            dr.Close();
            v.c.dbconection().Close();
        }

        // MÉTODOS

        public void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            habilitado(true);
            if (!peditar)
                btnEditar.Visible = label9.Visible = false;
            if (rbtnHojaPert.Checked)
            {
                cambioopcion();
                reporte = 1;
                lblHojaPert.ForeColor = Color.Crimson;
                lblReporteP.ForeColor = lblIncidenciaP.ForeColor = Color.FromArgb(75, 44, 52);
                label3.Visible = btnLimpiar.Visible = true;
                validaciongbxs(reporte);
            }
            else if (rbtnReporteP.Checked)
            {
                cambioopcion();
                reporte = 2;
                lblReporteP.ForeColor = Color.Crimson;
                lblHojaPert.ForeColor = lblIncidenciaP.ForeColor = Color.FromArgb(75, 44, 52);
                label3.Visible = btnLimpiar.Visible = true;
                validaciongbxs(reporte);
            }
            else if (rbtnIncidenciaP.Checked)
            {
                cambioopcion();
                reporte = 3;
                lblIncidenciaP.ForeColor = Color.Crimson;
                lblHojaPert.ForeColor = lblReporteP.ForeColor = Color.FromArgb(75, 44, 52);
                label3.Visible = btnLimpiar.Visible = true;
                validaciongbxs(reporte);
            }
        }

        public void guardar(int tipo)
        {
            if (string.IsNullOrWhiteSpace(txtNombreReporte.Text))
            {
                MessageBox.Show("Ingrese el nombre del reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreReporte.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtCodigoReporte.Text))
            {
                MessageBox.Show("Ingrese el código del reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigoReporte.Focus();
            }
            else if (dtpVigencia.Value.Date == DateTime.Now.Date)
            {
                MessageBox.Show("Ingrese la vigencia del reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpVigencia.Focus();
            }
            else if (dtpVigencia.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("La fecha de vigencia no debe de ser menor a la fecha actual, favor de ingresar una fecha diferente", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpVigencia.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtRevisionReporte.Text))
            {
                MessageBox.Show("Ingrese el número de revisión del reporte", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRevisionReporte.Focus();
            }
            else if (string.IsNullOrWhiteSpace(txtPassUsuario.Text))
            {
                MessageBox.Show("Ingrese la contraseña del usuario", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassUsuario.Focus();
            }
            else if (lblUsuario.Text == "DATOS INCORRECTOS")
            {
                MessageBox.Show("Contraseña incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassUsuario.Clear();
                lblUsuario.Text = "";
                txtPassUsuario.Focus();
            }
            else
            {
                MySqlCommand agregar = new MySqlCommand("INSERT INTO encabezadoreportes(reporte, nombrereporte, codigoreporte, vigencia, revision, usuariofkcpersonal, FechaHoraRegistro) VALUES('" + tipo + "', '" + txtNombreReporte.Text + "', '" + txtCodigoReporte.Text + "', '" + dtpVigencia.Value.ToString("yyyy-MM-dd") + "', '" + txtRevisionReporte.Text + "', '" + idusuario + "', now())", v.c.dbconection());
                agregar.ExecuteNonQuery();
                v.c.dbconection().Close();
                MessageBox.Show("Información guardada exitosamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarbtn();
                habilitado(false);
            }
        }

        public void editar(int tipo)
        {
            validartxt();
            if (contadoralltxt == contadortxt)
            {
                MessageBox.Show("Sin modificaciones", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarbtn();
            }
            else if (string.IsNullOrWhiteSpace(txtPassUsuario.Text))
            {
                MessageBox.Show("Ingrese la contraseña del usuario", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassUsuario.Focus();
            }
            else if (lblUsuario.Text == "DATOS INCORRECTOS")
            {
                MessageBox.Show("Contraseña incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassUsuario.Clear();
                lblUsuario.Text = "";
                txtPassUsuario.Focus();
            }
            else if (dtpVigencia.Value.Month + dtpVigencia.Value.Year < DateTime.Now.Month + DateTime.Now.Year)
            {
                MessageBox.Show("La fecha de vigencia no debe de ser menor a la fecha actual, favor de ingresar una fecha diferente", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpVigencia.Focus();
            }
            else
            {
                string consulta = "UPDATE encabezadoreportes SET";
                string set = "";
                if (!string.IsNullOrWhiteSpace(txtNombreReporte.Text))
                {
                    if (string.IsNullOrWhiteSpace(set))
                        set = " nombrereporte = '" + txtNombreReporte.Text + "'";
                    else
                        set += ", nombrereporte = '" + txtNombreReporte.Text + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtCodigoReporte.Text))
                {
                    if (string.IsNullOrWhiteSpace(set))
                        set = " codigoreporte = '" + txtCodigoReporte.Text + "'";
                    else
                        set += ", codigoreporte = '" + txtCodigoReporte.Text + "'";
                }
                if (dtpVigencia.Value.Date != DateTime.Now.Date)
                {
                    if (string.IsNullOrWhiteSpace(set))
                        set = " vigencia = '" + dtpVigencia.Value.ToString("yyyy-MM-dd") + "'";
                    else
                        set += ", vigencia = '" + dtpVigencia.Value.ToString("yyyy-MM-dd") + "'";
                }
                if (!string.IsNullOrWhiteSpace(txtRevisionReporte.Text))
                {
                    if (string.IsNullOrWhiteSpace(set))
                        set = " revision = '" + txtRevisionReporte.Text + "'";
                    else
                        set += ", revision = '" + txtRevisionReporte.Text + "'";
                }
                set += " WHERE idencabezadoreportes = '" + idencabezadoreporte + "'";
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    MySqlCommand editar = new MySqlCommand(consulta + set, v.c.dbconection());
                    editar.ExecuteNonQuery();
                    v.c.dbconection().Close();
                    MessageBox.Show("Información editada exitosamente", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MySqlCommand historial = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('Encabezado de Reportes', '" + idencabezadoreporte + "', '" + nombreanterior + "; " + codigoanterior + "; " + fechaAnterior + "; " + revisionanterior + "', '" + idusuario + "', now(), 'Actualización de Encabezado de Reportes', '" + observaciones + "', '" + empresa + "', '" + area + "')", v.c.dbconection());
                    historial.ExecuteNonQuery();
                    v.c.dbconection().Close();
                    limpiarbtn();
                }
            }
        }

        public void validartxt()
        {
            contadortxt = 0;
            contadoralltxt = 0;
            if (!string.IsNullOrWhiteSpace(txtNombreReporte.Text))
            {
                contadoralltxt++;
                if (nombreanterior == txtNombreReporte.Text)
                    contadortxt++;
            }
            if (!string.IsNullOrWhiteSpace(txtCodigoReporte.Text))
            {
                contadoralltxt++;
                if (codigoanterior == txtCodigoReporte.Text)
                    contadortxt++;
            }
            if (dtpVigencia.Value.Date != DateTime.Now.Date)
            {
                contadoralltxt++;
                if (Convert.ToDateTime("1 " + fechaAnterior).ToString("%M - yyyy") == dtpVigencia.Value.Date.ToString("%M - yyyy")) // CORREGIR EN FECHA ANTERIOR
                    contadortxt++;
            }
            if (!string.IsNullOrWhiteSpace(txtRevisionReporte.Text))
            {
                contadoralltxt++;
                if (revisionanterior == txtRevisionReporte.Text)
                    contadortxt++;
            }
        }

        public void habilitado(bool caracteristica)
        {
            txtNombreReporte.Enabled = txtCodigoReporte.Enabled = dtpVigencia.Enabled = txtRevisionReporte.Enabled = txtPassUsuario.Enabled = caracteristica;
        }

        public void validaciongbxs(int tipo)
        {
            MySqlCommand verificar = new MySqlCommand("SET lc_time_names = 'es_ES'; SELECT idencabezadoreportes AS 'ID', nombreReporte AS 'NREPORTE', codigoreporte AS 'CODIGO', UPPER(DATE_FORMAT(vigencia, '%M %Y')) AS 'VIGENCIA', revision AS 'REVISION' FROM encabezadoreportes WHERE reporte = '" + tipo + "'", v.c.dbconection());
            MySqlDataReader dr = verificar.ExecuteReader();
            if (dr.Read())
            {
                idencabezadoreporte = dr.GetInt32("ID");
                nombreanterior = dr.GetString("NREPORTE");
                codigoanterior = dr.GetString("CODIGO");
                fechaAnterior = dr.GetString("VIGENCIA");
                revisionanterior = dr.GetString("REVISION");
            }
            else
            {
                idencabezadoreporte = 0;
                nombreanterior = codigoanterior = fechaAnterior = revisionanterior = "";
            }
            dr.Close();
            v.c.dbconection().Close();
            if (string.IsNullOrWhiteSpace(nombreanterior) && string.IsNullOrWhiteSpace(codigoanterior) && string.IsNullOrWhiteSpace(fechaAnterior) && string.IsNullOrWhiteSpace(revisionanterior))
            {
                btnEditar.BackgroundImage = Properties.Resources.save1;
                label9.Location = new Point(472, 319);
                label9.Text = "GUARDAR";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(nombreanterior))
                    gbxNombre.Text = "NOMBRE: \"" + nombreanterior + "\"";

                if (!string.IsNullOrWhiteSpace(codigoanterior))
                    gbxCodigo.Text = "CÓDIGO: \"" + codigoanterior + "\"";

                if (!string.IsNullOrWhiteSpace(fechaAnterior))
                    gbxVigencia.Text = "VIGENCIA: \"" + fechaAnterior + "\"";

                if (!string.IsNullOrWhiteSpace(revisionanterior))
                    gbxRevision.Text = "REVISIÓN: \"" + revisionanterior + "\"";

                btnEditar.BackgroundImage = Properties.Resources.document_edit_icon_icons_com_52428;
                label9.Location = new Point(477, 319);
                label9.Text = "EDITAR";
            }
        }

        public void limpiarbtn()
        {
            rbtnHojaPert.Checked = rbtnReporteP.Checked = rbtnIncidenciaP.Checked = false;
            lblHojaPert.ForeColor = lblReporteP.ForeColor = lblIncidenciaP.ForeColor = Color.FromArgb(75, 44, 52);
            lblUsuario.Text = txtNombreReporte.Text = txtCodigoReporte.Text = txtRevisionReporte.Text = txtPassUsuario.Text = "";
            dtpVigencia.Value = DateTime.Now;
            gbxNombre.Text = "NOMBRE";
            gbxCodigo.Text = "CÓDIGO";
            gbxVigencia.Text = "VIGENCIA";
            gbxRevision.Text = "REVISIÓN";
            btnLimpiar.Visible = label3.Visible = btnEditar.Visible = label9.Visible = false;
            habilitado(false);
        }

        public void cambioopcion()
        {
            txtNombreReporte.Text = txtCodigoReporte.Text = txtRevisionReporte.Text = txtPassUsuario.Text = "";
            dtpVigencia.Value = DateTime.Now;
            gbxNombre.Text = "NOMBRE";
            gbxCodigo.Text = "CÓDIGO";
            gbxVigencia.Text = "VIGENCIA";
            gbxRevision.Text = "REVISIÓN";
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        public void btnall_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(59, 59);
        }

        public void btnall_MouseLeave(object sender, EventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(54, 54);
        }

        public void gbxall_Painting(object sender, PaintEventArgs e)
        {
            GroupBox gbxall = sender as GroupBox;
            v.DrawGroupBox(gbxall, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        public void dtpall_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
    }
}