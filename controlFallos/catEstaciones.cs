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
    public partial class catEstaciones : Form
    {

        validaciones v;
        int idUsuario, idestacionTemp, status;
        string estacionAnterior;
        bool editar, yaAparecioMensaje;
        public catEstaciones(int idUsuario, validaciones v)
        {
            InitializeComponent();
            this.v = v;

            this.idUsuario = idUsuario;
            initializeData();
            establecerPrivilegios();
        }
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        public void establecerPrivilegios()
        {
            string sql = v.getaData("SELECT Concat(insertar,',',consultar,',',editar,',',desactivar)  FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = 'catestaciones'").ToString();
            if (!string.IsNullOrWhiteSpace(sql))
            {
                string[] mdr = sql.Split(',');
                Pconsultar = v.getBoolFromInt(Convert.ToInt32(mdr[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(mdr[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(mdr[2]));
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(mdr[3]));
            }
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddanaquel.Visible = true;
            }
            if (Pconsultar)
            {
                gbanaqueles.Visible = true;
            }
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

        void actualizar()
        {
            if (getCambios())
            {
                string estacion = v.mayusculas(txtestacion.Text.Trim().ToLower());
                bool res = false;
                if (!estacionAnterior.Equals(v.mayusculas(txtestacion.Text.Trim().ToLower())))
                    res = (Convert.ToInt32(v.getaData(string.Format("SELECT COUNT(*) FROM cestaciones WHERE estacion='{0}'", estacion))) > 0);
                if (!string.IsNullOrWhiteSpace(estacion))
                {
                    if (!res)
                    {
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                            if (v.c.insertar(string.Format("UPDATE cestaciones SET estacion='{1}' WHERE idestacion='{0}'", idestacionTemp, estacion)))
                            {
                                if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('{0}','{1}','{2}','{3}',NOW(),'{4}','{5}','{6}','{7}')", new object[8] { "Catálogo de Estaciones", idestacionTemp, estacionAnterior, idUsuario, "Actualización de Estación", observaciones, 1, 1 })))
                                {
                                    if (!yaAparecioMensaje) MessageBox.Show("La Estación se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    limpiar();
                                }
                            }
                        }
                    }
                    else
                        MessageBox.Show("La Estación Ingresada Ya Se Encuentra En El Sistema", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("El Campo \"Estación\" No Puede Estar Vacío", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void tbestaciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbestaciones.Columns[e.ColumnIndex].Name == "Estatus")
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
        bool getCambios()
        {
            if (editar)
            {
                if (status == 1 && !string.IsNullOrWhiteSpace(txtestacion.Text.Trim()) && !estacionAnterior.Equals(v.mayusculas(txtestacion.Text.Trim().ToLower())))
                    return true;
                else
                    return false;
            }
            else
                return false;

        }
        private void tbestaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (getCambios())
            {
                var res = MessageBox.Show("Desea Guardar La Información", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
                    getInfo(e);
                }
                else getInfo(e);
            }
            else
                getInfo(e);

        }
        void getInfo(DataGridViewCellEventArgs e)
        {
            try
            {
                idestacionTemp = Convert.ToInt32(tbestaciones.Rows[e.RowIndex].Cells[0].Value);
                status = v.getStatusInt(tbestaciones.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (Pdesactivar)
                {

                    if (status == 0)
                    {
                        btndelpa.BackgroundImage = Properties.Resources.up;
                        lbldelpa.Text = "Reactivar";
                    }
                    else
                    {
                        btndelpa.BackgroundImage = Properties.Resources.delete__4_;
                        lbldelpa.Text = "Desactivar";
                    }
                    pdelete.Visible = true;
                }
                if (Peditar)
                {

                    estacionAnterior = txtestacion.Text = v.mayusculas(tbestaciones.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());

                    btnsavemp.BackgroundImage = Properties.Resources.pencil;
                    editar = true;
                    if (Pinsertar) pCancelar.Visible = true;
                    gbaddanaquel.Text = "Actualizar Estación: " + estacionAnterior;
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtestacion_TextChanged(object sender, EventArgs e)
        {
            if (editar)
            {
                if (getCambios())
                    btnsavemp.Visible = lblsavemp.Visible = true;
                else
                    btnsavemp.Visible = lblsavemp.Visible = false;
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (getCambios())
            {
                var res = MessageBox.Show("Desea Guardar La Información", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
                    limpiar();
                }
                else limpiar();
            }
            else
                limpiar();
        }

        private void btndelpa_Click(object sender, EventArgs e)
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


                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación de la Estación ";
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    if (v.c.insertar(string.Format("UPDATE cestaciones SET status='{0}' WHERE idestacion='{1}'", status, idestacionTemp)))
                    {
                        if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema(form, idregistro,usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Estaciones','{0}','{2}',NOW(),'{1}activación de Estación','{3}','{4}','{5}')", new object[6] { idestacionTemp, msg, idUsuario, edicion, 1, 1 })))
                        {
                            MessageBox.Show("La Estación se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtestacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnsavemp_Click(null, e);
            else
                v.letrasnumerosdiagonalyguion(e);
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtestacion.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (estacionAnterior != v.mayusculas(txtestacion.Text.Trim().ToLower()) && !string.IsNullOrWhiteSpace(txtestacion.Text))
                    if (MessageBox.Show("¿Desea guardar las moidifcaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        void insertar()
        {
            string estacion = v.mayusculas(txtestacion.Text.Trim().ToLower());
            if (!string.IsNullOrWhiteSpace(estacion))
            {
                if (Convert.ToInt32(v.getaData(string.Format("SELECT COUNT(*) FROM cestaciones WHERE estacion='{0}'", estacion))) == 0)
                {
                    if (v.c.insertar(string.Format("INSERT INTO cestaciones(estacion,usuariofkcpersonal) VALUES('{0}','{1}')", estacion, idUsuario)))
                    {
                        if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema(form, idregistro, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES('{0}',(SELECT idestacion FROM cestaciones WHERE estacion='{1}'),'{2}',NOW(),'{3}','{4}','{5}')", new object[6] { "Catálogo de Estaciones", estacion, idUsuario, "Inserción de Estación", 1, 1 })))
                        {
                            MessageBox.Show("La Estación se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            limpiar();
                        }
                    }
                }
                else
                    MessageBox.Show("La Estación Ingresada Ya Se Encuentra En El Sistema", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("El Campo \"Estación\" No Puede Estar Vacío", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        void limpiar()
        {
            editar = false;
            gbaddanaquel.Text = "Agregar Estación";
            btnsavemp.BackgroundImage = Properties.Resources.save;
            lblsavemp.Text = "Guardar";
            editar = false;
            txtestacion.Clear();
            idestacionTemp = 0;
            pdelete.Visible = false;
            pCancelar.Visible = false;
            status = 0;
            relacionServicioEstacion rel = (relacionServicioEstacion)Owner;
            if (!rel.editar)
                v.iniCombos("SELECT idestacion AS id, UPPER(estacion) as estacion FROM cestaciones WHERE status = 1 ORDER BY estacion ASC", rel.cbxgetestacion, "id", "estacion", "--SELECCIONE UNA ESTACIÓN--");
            rel.initializeRelations(null);
            btnsavemp.Visible = lblsavemp.Visible = true;
            yaAparecioMensaje = false;
            initializeData();
            txtestacion.Focus();
        }
        void initializeData()
        {
            tbestaciones.Rows.Clear();
            string sql = "SELECT t1.idestacion as id,UPPER(t1.estacion) as estacion,UPPER(CONCAT(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno)) as usuario,if(t1.status=1,'ACTIVO','NO ACTIVO') as estatus FROM cestaciones as t1 LEFT JOIN cpersonal as t2 ON t1.usuariofkcpersonal=t2.idpersona";
            MySqlCommand m = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = m.ExecuteReader();
            while (dr.Read())
            {
                tbestaciones.Rows.Add(dr.GetString("id"), dr.GetString("estacion"), dr.GetString("usuario"), dr.GetString("estatus"));
            }
            tbestaciones.ClearSelection();
            if (tbestaciones.Rows.Count > 0)
                tbestaciones.Rows[0].Cells[0].Selected = false;

        }
    }
}
