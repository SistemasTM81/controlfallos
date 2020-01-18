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
        bool editar;
        int empresa, area, idUsuario, idRegistro, revisionAnterior, idsupervior, reporte;
        string nombreAnterior, codigoANterior;
        DateTime vigenciaAnterior;

        private void rbpercances_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton aux = (RadioButton)sender;
            if ((rbincidencia.Checked || rbpercances.Checked || rbpersonal.Checked) && aux.Checked == true)
            {
                if (!cambios())
                    datestotxt();
                else
                {
                    if (MessageBox.Show("¿Desea " + (editar ? "guardar los cambios" : "concluir el registro?"), validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    { }
                    else { datestotxt(); }
                }
            }
        }
        void datestotxt()
        {
            limpiar();
            reporte = (rbpercances.Checked ? 1 : rbpersonal.Checked ? 2 : rbincidencia.Checked ? 3 : 0);
            editar = Convert.ToInt32(v.getaData("select count(*) from encabezadoreportes where " + v.c.fieldsencabezadoreportes[1] + "='" + reporte + "';")) > 0;
            plimpiar.Visible = !(pguardar.Visible = (editar ? false : true));
            if (editar)
            {
                string[] datos = v.getaData("select upper(concat(" + v.c.fieldsencabezadoreportes[0] + ",'|'," + v.c.fieldsencabezadoreportes[2] + ",'|'," + v.c.fieldsencabezadoreportes[3] + ",'|'," + v.c.fieldsencabezadoreportes[4] + ",'|'," + v.c.fieldsencabezadoreportes[5] + ")) from encabezadoreportes where reporte='" + reporte + "';").ToString().Split('|');
                idRegistro = Convert.ToInt32(datos[0]);
                txtnombre.Text = nombreAnterior = datos[1];
                txtcodigo.Text = codigoANterior = datos[2];
                dtpvigencia.Value = vigenciaAnterior = DateTime.Parse(datos[3]);
                txtrevision.Text = (revisionAnterior = Convert.ToInt32(datos[4])).ToString();
            }
            changeenabled(true);
        }
        void limpiar() /** Method to clean textbox**/
        {
            idRegistro = revisionAnterior = idsupervior = reporte = 0;
            txtnombre.Clear();
            txtcodigo.Clear();
            txtrevision.Clear();
            txtcontraseña.Clear();
            lblusario.Text = nombreAnterior = codigoANterior = "";
            dtpvigencia.Value = vigenciaAnterior = DateTime.Today;
            editar = plimpiar.Visible = pguardar.Visible = false;
            changeenabled(false);
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (v.datosencabezados(txtnombre.Text, txtcodigo.Text, dtpvigencia.Value, txtrevision.Text, txtcontraseña.Text))
                if (!editar)
                {
                    if (v.c.insertar("insert into encabezadoreportes (" + v.c.fieldsencabezadoreportes[1] + "," + v.c.fieldsencabezadoreportes[2] + "," + v.c.fieldsencabezadoreportes[3] + "," + v.c.fieldsencabezadoreportes[4] + "," + v.c.fieldsencabezadoreportes[5] + "," + v.c.fieldsencabezadoreportes[6] + "," + v.c.fieldsencabezadoreportes[7] + ") values('" + reporte + "','" + txtnombre.Text.Trim() + "','" + txtcodigo.Text.Trim() + "','" + dtpvigencia.Value.ToString("yyyy-MM-dd") + "','" + txtrevision.Text + "','" + idsupervior + "',now())"))
                        if (v.c.insertar("insert into modificaciones_sistema (" + v.c.fieldsmodificaciones_sistema[1] + ", " + v.c.fieldsmodificaciones_sistema[2] + ", " + v.c.fieldsmodificaciones_sistema[4] + ", " + v.c.fieldsmodificaciones_sistema[5] + ", " + v.c.fieldsmodificaciones_sistema[6] + ", " + v.c.fieldsmodificaciones_sistema[8] + ", " + v.c.fieldsmodificaciones_sistema[9] + ")values('Catálogo de Encabezados','" + v.getaData("select " + v.c.fieldsencabezadoreportes[0] + " from encabezadoreportes where " + v.c.fieldsencabezadoreportes[1] + "='" + reporte + "';") + "','" + idsupervior + "',now(),'Inserción de Encabezado de Reporte','" + empresa + "','" + area + "')"))
                        {
                            MessageBox.Show("Registro insertado de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            todisableradios();
                        }
                }
                else
                {
                    observacionesEdicion o = new observacionesEdicion(v);
                    o.Owner = this;
                    if (o.ShowDialog() == DialogResult.OK)
                    {
                        string motivo = o.txtgetedicion.Text.Trim();
                        if (v.c.insertar("update encabezadoreportes set " + v.c.fieldsencabezadoreportes[2] + "='" + txtnombre.Text + "', " + v.c.fieldsencabezadoreportes[3] + "='" + txtcodigo.Text + "'," + v.c.fieldsencabezadoreportes[4] + "='" + dtpvigencia.Value.ToString("yyyy-MM-dd") + "'," + v.c.fieldsencabezadoreportes[5] + "='" + txtrevision.Text + "' where " + v.c.fieldsencabezadoreportes[0] + "='" + idRegistro + "'"))
                            if (v.c.insertar("insert into modificaciones_sistema (" + v.c.fieldsmodificaciones_sistema[1] + ", " + v.c.fieldsmodificaciones_sistema[2] + ", " + v.c.fieldsmodificaciones_sistema[3] + ", " + v.c.fieldsmodificaciones_sistema[4] + ", " + v.c.fieldsmodificaciones_sistema[5] + ", " + v.c.fieldsmodificaciones_sistema[6] + ", " + v.c.fieldsmodificaciones_sistema[7] + ", " + v.c.fieldsmodificaciones_sistema[8] + ", " + v.c.fieldsmodificaciones_sistema[9] + ")values('Catálogo de Encabezados','" + idRegistro + "','" + txtnombre.Text + ";" + txtcodigo.Text + ";" + dtpvigencia.Value.ToString("yyyy-MM-dd") + ";" + txtrevision.Text + "','" + idUsuario + "',now(),'Actualización de Encabezado de Reportes','" + motivo + "','" + empresa + "','" + area + "')"))
                            {
                                MessageBox.Show("Los datos se actualizaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                                todisableradios();
                            }
                    }
                }
        }
        void todisableradios()
        {
            rbpersonal.Checked = rbpercances.Checked = rbincidencia.Checked = false;
        }

        private void txtnombre_TextChanged(object sender, EventArgs e)
        {
            if (editar)
                pguardar.Visible = (cambios() ? true : false);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (cambios())
                if (MessageBox.Show("'Desea " + (editar ? "guardar los cambios?" : "concluir el registro?"), validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { }
                else
                { limpiar(); todisableradios(); }
            else
            { limpiar(); todisableradios(); }
        }

        bool cambios()
        {
            if (((txtnombre.Text.Trim() != nombreAnterior || txtcodigo.Text.Trim() != codigoANterior || revisionAnterior != Convert.ToInt32((string.IsNullOrWhiteSpace(txtrevision.Text) ? "0" : txtrevision.Text)) || vigenciaAnterior != dtpvigencia.Value) && !string.IsNullOrWhiteSpace(txtnombre.Text) && !string.IsNullOrWhiteSpace(txtcodigo.Text) && !string.IsNullOrWhiteSpace(txtrevision.Text) && editar) || ((!string.IsNullOrWhiteSpace(txtnombre.Text) || !string.IsNullOrWhiteSpace(txtcodigo.Text) || !string.IsNullOrWhiteSpace(txtrevision.Text) || dtpvigencia.Value > DateTime.Now) && idRegistro == 0))
                return true;
            else return false;
        }
        bool peditar { get; set; }

        private void txtcontraseña_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtcontraseña.Text))
            {
                lblusario.Text = ((Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1." + v.c.fieldscpersonal[0] + "=t2." + v.c.fieldsdatosistema[1] + " where t2." + v.c.fieldsdatosistema[3] + "='" + v.Encriptar(txtcontraseña.Text.Trim()) + "';")) == 0 ? "" : v.getaData("select upper(concat(coalesce(t1." + v.c.fieldscpersonal[2] + ",''),' ',coalesce(t1." + v.c.fieldscpersonal[3] + ",''),' ',t1." + v.c.fieldscpersonal[4] + ")) from cpersonal as t1 inner join datosistema as t2 on t1." + v.c.fieldscpersonal[0] + "=t2." + v.c.fieldsdatosistema[1] + " where t2." + v.c.fieldsdatosistema[3] + "='" + v.Encriptar(txtcontraseña.Text.Trim()) + "' and " + v.c.fieldscpersonal[6] + "='1';").ToString()));

                idsupervior = ((Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1." + v.c.fieldscpersonal[0] + "=t2." + v.c.fieldsdatosistema[1] + " where t2." + v.c.fieldsdatosistema[3] + "='" + v.Encriptar(txtcontraseña.Text.Trim()) + "';")) == 0 ? 0 : Convert.ToInt32(v.getaData("select t1." + v.c.fieldscpersonal[0] + " from cpersonal as t1 inner join datosistema as t2 on t1." + v.c.fieldscpersonal[0] + "=t2." + v.c.fieldsdatosistema[1] + " where t2." + v.c.fieldsdatosistema[3] + "='" + v.Encriptar(txtcontraseña.Text.Trim()) + "' and " + v.c.fieldscpersonal[6] + "='1';"))));
            }
        }

        public ReportesVigencias(int empresa, int area, int idUsuario, validaciones v)
        {
            InitializeComponent();
            this.v = v;
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idUsuario;
        }
        public void changeenabled(bool enabled)
        {
            txtnombre.Enabled = txtcodigo.Enabled = txtrevision.Enabled = txtcontraseña.Enabled = dtpvigencia.Enabled = enabled;
        }
        private void ReportesVigencias_Load(object sender, EventArgs e)
        {
            privilegios();
            changeenabled(false);
        }
        public void privilegios()
        {
            string sql = "SELECT " + v.c.fieldsprivilegios[4] + " FROM privilegios where " + v.c.fieldsprivilegios[1] + " = '" + idUsuario + "' and " + v.c.fieldsprivilegios[2] + " = 'repPersonal'";
            string[] privilegios = v.getaData(sql).ToString().Split('/');
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[2]));
        }
        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }
    }
}