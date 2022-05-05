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
    public partial class relacionServicioEstacion : Form
    {
        int idUsuario, servicioAnterior, estacionAnterior, status, idrelacionTemp;
        validaciones v;
        public bool editar { get; protected internal set; }
        bool yaAparecioMensaje;
        public relacionServicioEstacion(int idUsuario,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            cbxgetestacion.DrawItem += v.combos_DrawItem;
            cbxgetservicio.DrawItem += v.combos_DrawItem;
            v.iniCombos("SELECT idservicio as id, UPPER(CONCAT(nombre,' - ',descripcion)) as servicio FROM cservicios WHERE status=1 ORDER BY nombre ASC", cbxgetservicio, "id", "servicio", "--SELECCIONE UN SERVICIO--");
            initializeStations();
            this.idUsuario = idUsuario;
            initializeRelations(null);
        }
        void initializeStations() { v.iniCombos("SELECT idestacion AS id, UPPER(estacion) as estacion FROM cestaciones WHERE status = 1", cbxgetestacion, "id", "estacion", "--SELECCIONE UNA ESTACIÓN--"); }
        public void initializeRelations(string wheres)
        {
            dgvrelaciones.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT t1.idrelacServicioEstacion, UPPER(t2.Nombre), UPPER(t3.estacion), UPPER(CONCAT(coalesce(t4.nombres,''),' ',coalesce(t4.apPaterno,''),' ',coalesce(t4.apMaterno,''))),if(t1.status=1,'ACTIVO','NO ACTIVO'),t2.idservicio,t3.idestacion FROM relacservicioestacion as t1 INNER JOIN cservicios as t2 ON t1.serviciofkcservicios= t2.idservicio INNER JOIN cestaciones as t3 ON t1.estacionfkcestaciones = t3.idestacion INNER JOIN cpersonal as t4 ON t1.usuariofkcpersonal = t4.idpersona " + wheres);
            for (int i = 0; i < dt.Rows.Count; i++) dgvrelaciones.Rows.Add(dt.Rows[i].ItemArray);
            dgvrelaciones.ClearSelection();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            catEstaciones cat = new catEstaciones(idUsuario, v);
            cat.Owner = this;
            cat.ShowDialog();
        }

        private void dgvrelaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (editar && getCambios())
            {
                var res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
                    guardarReporte(e);
                }
                else if (res == DialogResult.No)
                    guardarReporte(e);
            }
            else
            {
                guardarReporte(e);
            }
        }

        private void dgvrelaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvrelaciones.Columns[e.ColumnIndex].Name == "statusDataGrid")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }

        private void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (getCambios())
                    btnsave.Visible = lblsave.Visible = true;
                else
                    btnsave.Visible = lblsave.Visible = false;
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (editar && getCambios())
            {
                var res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
                    limpiar();
                }
                else if (res == DialogResult.No)
                    limpiar();
            }
            else
                limpiar();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (status == 0)
            {

                msg = "Re";
                state = 1;
            }
            else
            {
                state = 0;
                msg = "Des";

            }
            observacionesEdicion obs = new observacionesEdicion(v);
            obs.Owner = this;
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación De La Relación";
            obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 55, obs.lblinfo.Location.Y);
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                try
                {
                    String sql = "UPDATE relacservicioestacion SET status = " + state + " WHERE idrelacServicioEstacion  = " + idrelacionTemp;
                    if (v.c.insertar(sql))
                    {
                        if (v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Estaciones - Relacion Servicio - Estación','" + idrelacionTemp + "','" + idUsuario + "',NOW(),'" + msg + "activación de Relación','" + edicion + "','1','1')"))
                        {
                            MessageBox.Show("La Relación ha sido " + msg + "activada Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            initializeRelations(null);
                            limpiar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbxgetestacion.SelectedIndex > 0 || cbxgetservicio.SelectedIndex > 0)
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((servicioAnterior != (int)cbxgetservicio.SelectedValue || estacionAnterior != (int)cbxgetestacion.SelectedValue) && cbxgetservicio.SelectedIndex > 0 && cbxgetestacion.SelectedIndex > 0)
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                    { DialogResult = DialogResult.None; DialogResult = DialogResult.None; }
                else
                    this.Close();
            }

        }

        void guardarReporte(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //if (Pdesactivar || Peditar)
                {
                    idrelacionTemp = Convert.ToInt32(dgvrelaciones.Rows[e.RowIndex].Cells[0].Value);
                    status = v.getStatusInt(dgvrelaciones.Rows[e.RowIndex].Cells[4].Value.ToString());
                }
                //  if (Pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelete.BackgroundImage = Properties.Resources.up;
                        lbldelete.Text = "Reactivar";
                    }
                    else
                    {
                        btndelete.BackgroundImage = Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar";
                    }
                    pdelete.Visible = true;
                }
                //  if (Peditar)
                {
                    cbxgetservicio.SelectedValue = servicioAnterior = Convert.ToInt32(dgvrelaciones.Rows[e.RowIndex].Cells[5].Value);
                    cbxgetestacion.SelectedValue = estacionAnterior = Convert.ToInt32(dgvrelaciones.Rows[e.RowIndex].Cells[6].Value);
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    gbrelacion.Text = "Actualizar Relación";
                    lblsave.Text = "Guardar";
                    btnsave.Visible = lblsave.Visible = false;
                    /* if (Pinsertar)*/
                    pCancel.Visible = true;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            object servicio = cbxgetservicio.SelectedValue, estacion = cbxgetestacion.SelectedValue;
            if (!v.camposVaciosRelacionServEstacion(Convert.ToInt32(servicio), Convert.ToInt32(estacion)))
            {
                if (!editar)
                {

                    if (!v.existeRelacion(servicio, estacion))
                    {
                        if (v.c.insertar("INSERT INTO relacservicioestacion(serviciofkcservicios, estacionfkcestaciones, usuariofkcpersonal) VALUES('" + servicio + "','" + estacion + "','" + idUsuario + "')"))
                        {
                            MessageBox.Show("Relación Agregada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            limpiar();
                            initializeRelations(null);
                        }
                    }
                }
                else
                {
                    if (!v.existeRelacionActualizar(servicioAnterior, servicio, estacionAnterior, estacion))
                    {
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                            if (v.c.insertar("UPDATE relacservicioestacion SET serviciofkcservicios ='" + servicio + "', estacionfkcestaciones='" + estacion + "' WHERE idrelacServicioEstacion='" + idrelacionTemp + "'"))
                            {
                                if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('Catálogo de Estaciones - Relacion Servicio - Estación','{0}','{1}','{2}',NOW(),'Actualización de Relación','{3}','1','1')", new object[] { idrelacionTemp, (servicioAnterior + ";" + estacionAnterior), idUsuario, observaciones })))
                                {
                                    if (!yaAparecioMensaje) MessageBox.Show("Relación Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    limpiar();
                                    initializeRelations(null);
                                }
                            }
                        }
                    }
                }
            }


        }
        void limpiar()
        {
            cbxgetestacion.SelectedIndex = cbxgetservicio.SelectedIndex = 0;
            // if (Pinsertar)
            {
                btnsave.BackgroundImage = Properties.Resources.save;
                editar = !(lblsave.Visible = btnsave.Visible = true);
            }
            idrelacionTemp = servicioAnterior = estacionAnterior = 0;
            pCancel.Visible = pdelete.Visible = false;
            yaAparecioMensaje = false;
        }
        bool getCambios()
        {
            return (status == 1 && (cbxgetestacion.SelectedIndex > 0 && cbxgetservicio.SelectedIndex > 0) && (servicioAnterior != Convert.ToInt32(cbxgetservicio.SelectedValue) || estacionAnterior != Convert.ToInt32(cbxgetestacion.SelectedValue)));
        }
    }
}
