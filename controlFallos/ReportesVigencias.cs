using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class ReportesVigencias : Form
    {
        validaciones v;
        int reporte, empresa, area, idsupervisor, idencabezadoreporte, idUsuario, revisionanterior;
        //VARIABLES ANTERIORES
        string nombreanterior, codigoanterior;
        DateTime fechaAnterior;
        bool modificar;
        bool peditar { get; set; }
        public ReportesVigencias(int empresa, int area, int idUsuario, validaciones v)
        {
            InitializeComponent();
            this.v = v;
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idUsuario;
        }
        private void ReportesVigencias_Load(object sender, EventArgs e)
        {
            privilegios();
            dtpVigencia.Value = DateTime.Now;
            txtnombre.Focus();
            if (!peditar)
                btnEditar.Visible = label9.Visible = false;
        }
        // EVENTOS
        public void privilegios()
        {
            string sql = "SELECT privilegios FROM privilegios where usuariofkcpersonal = '" + idUsuario + "' and namform = 'repPersonal'";
            string[] privilegios = v.getaData(sql).ToString().Split('/');
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[0]));
        }
        public bool getBoolFromInt(int i) { return i == 1; }
        public void txtallnum_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
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
            if (cambios())
                if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) { }
                else
                    limpiarbtn();
            else limpiarbtn();
        }
        private void rbpercances_CheckedChanged(object sender, EventArgs e)
        {
            if (rbincidencia.Checked || rbpercances.Checked || rbreportep.Checked)
            {
                habilitado(true);
                reporte = (rbpercances.Checked ? 1 : rbreportep.Checked ? 2 : rbincidencia.Checked ? 3 : 0);
                if (Convert.ToInt32(v.getaData("select count(*) from encabezadoreportes where reporte='" + reporte + "';")) > 0)
                {
                    string[] anterior = v.getaData("select concat(idencabezadoreportes,'|',nombrereporte,'|',codigoreporte,'|',vigencia,'|',revision) from encabezadoreportes where reporte='" + reporte + "';").ToString().Split('|');
                    idencabezadoreporte = Convert.ToInt32(anterior[0]);
                    txtnombre.Text = nombreanterior = anterior[1];
                    txtCodigoReporte.Text = codigoanterior = anterior[2];
                    dtpVigencia.Value = fechaAnterior = DateTime.Parse(anterior[3]);
                    txtRevisionReporte.Text = (revisionanterior = Convert.ToInt32(anterior[4])).ToString();
                    pguardar.Visible = !(modificar = plimpiar.Visible = true);
                    btnEditar.BackgroundImage = controlFallos.Properties.Resources.document_edit_icon_icons_com_52428;
                }
                else
                {
                    change();
                    pguardar.Visible = (Convert.ToInt32(v.getaData("select count(*) from encabezadoreportes where reporte='" + reporte + "';")) == 0 && peditar ? true : false);
                }
            }
        }

        private void txtnombre_TextChanged(object sender, EventArgs e)
        {
            if (peditar)
                if (modificar)
                    pguardar.Visible = (cambios() ? true : false);
        }
        public bool cambios()
        {
            if ((txtnombre.Text.Trim() != nombreanterior || txtCodigoReporte.Text.Trim() != codigoanterior || dtpVigencia.Value.ToString("yyyy-MM-dd") != fechaAnterior.ToString("yyyy-MM-dd") || (string.IsNullOrWhiteSpace(txtRevisionReporte.Text) ? 0 : Convert.ToInt32(txtRevisionReporte.Text ?? "0")) != revisionanterior) && !string.IsNullOrWhiteSpace(txtnombre.Text) && !string.IsNullOrWhiteSpace(txtCodigoReporte.Text) && dtpVigencia.Value > DateTime.Today && !string.IsNullOrWhiteSpace(txtRevisionReporte.Text) && Convert.ToInt32(txtRevisionReporte.Text) > 0)
                return true;
            else return false;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (v.datosencabezados(txtnombre.Text.Trim(), txtCodigoReporte.Text.Trim(), dtpVigencia.Value, txtRevisionReporte.Text, txtPassUsuario.Text))
            {
                if (!modificar)
                {
                    if (v.c.insertar("insert into encabezadoreportes (reporte,nombrereporte,codigoreporte,vigencia,revision,usuariofkcpersonal,fechahoraregistro)values(" + reporte + ",'" + txtnombre.Text.Trim() + "','" + txtCodigoReporte.Text.Trim() + "','" + dtpVigencia.Value.ToString("yyyy-MM-dd") + "','" + Convert.ToInt32(txtRevisionReporte.Text) + "','" + idUsuario + "',now())"))
                        if (v.c.insertar("insert into modificaciones_sistema (form,idregistro,usuariofkcpersonal,fechaHora,tipo,empresa,area)values('Catálogo de Encabezados','" + v.getaData("select idencabezadoreportes from encabezadoreportes order by idencabezadoreportes desc limit 1") + "','" + idsupervisor + "',now(),'Inserción de Encabezado de Reporte','1','1')"))
                            MessageBox.Show("Datos registrados de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    observacionesEdicion o = new observacionesEdicion(v);
                    o.Owner = this;
                    if (o.ShowDialog() == DialogResult.OK)
                    {
                        string motivo = o.txtgetedicion.Text.Trim();
                        if (v.c.insertar("update encabezadoreportes set nombrereporte='" + txtnombre.Text + "',codigoreporte='" + txtCodigoReporte.Text + "',vigencia='" + dtpVigencia.Value.ToString("yyyy-MM-dd") + "',revision='" + Convert.ToInt32(txtRevisionReporte.Text) + "' where idencabezadoreportes='" + idencabezadoreporte + "'"))
                            if (v.c.insertar("insert into modificaciones_sistema (form,idregistro,ultimamodificacion,usuariofkcpersonal,fechaHora,tipo,motivoActualizacion,empresa,area)values('Catálogo de Encabezados','" + idencabezadoreporte + "','" + nombreanterior + ";" + codigoanterior + ";" + fechaAnterior.ToString("MMMM yyyy") + ";" + revisionanterior + "','" + idsupervisor + "',now(),'Actualización de Encabezado de Reportes','" + motivo + "','1','1')"))
                                MessageBox.Show("Los datos se actualizaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                limpiarbtn();
                habilitado(false);
            }
        }

        private void txtCodigoReporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }

        private void txtPassUsuario_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPassUsuario.Text))
            {
<<<<<<< HEAD
                idsupervisor = Convert.ToInt32(v.getaData("select idPersona from cpersonal as t1 inner join datosistema as t2 on t1.idPersona=t2.usuariofkcpersonal where password='" + v.Encriptar(txtPassUsuario.Text) + "' and t1.status='1';"));
                lblusuario.Text = (idsupervisor > 0 ? v.getaData("select concat(coalesce(apPaterno,''),' ',coalesce(apMaterno,''),' ',nombres) from cpersonal where idpersona='" + idsupervisor + "';").ToString() : "");
=======
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
>>>>>>> 289438355dcf9ce0a48126f327236d2313a9d884
            }
        }
        public void habilitado(bool caracteristica) { txtnombre.Enabled = txtCodigoReporte.Enabled = dtpVigencia.Enabled = txtRevisionReporte.Enabled = txtPassUsuario.Enabled = caracteristica; }
        public void change()
        {
            nombreanterior = codigoanterior = lblusuario.Text = "";
            revisionanterior = idsupervisor = 0;
            dtpVigencia.Value = fechaAnterior = DateTime.Today;
            txtCodigoReporte.Clear();
            txtnombre.Clear();
            txtPassUsuario.Clear();
            txtRevisionReporte.Clear();
            btnEditar.BackgroundImage = controlFallos.Properties.Resources.guardar__6_;
            plimpiar.Visible = modificar = false;
        }
        public void limpiarbtn()
        {
            rbpercances.Checked = rbreportep.Checked = rbincidencia.Checked = pguardar.Visible = pguardar.Visible = false;
            reporte = 0;
            change();
            habilitado(false);
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e) { v.mover(sender, e, this); }
        public void gbxall_Painting(object sender, PaintEventArgs e)
        {
            GroupBox gbxall = sender as GroupBox;
            v.DrawGroupBox(gbxall, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        public void dtpall_KeyDown(object sender, KeyEventArgs e) { e.SuppressKeyPress = true; }
    }
}