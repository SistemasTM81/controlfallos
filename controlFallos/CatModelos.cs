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
    public partial class CatModelos : Form
    {
        int idUsuario, idModelo, _idmodeloAnterior, _empresaAnterior, est;
        bool editar;
        string _modeloAnterior;
        validaciones v;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        public void privilegiosPuestos()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
            }
            mostrar_p();
        }
        void mostrar_p()
        {
            if (Pinsertar || Peditar)
                gbModelos.Visible = pCancelar.Visible = true;
            if (Pconsultar)
                gbconsultar.Visible = true;
            if (Peditar && !Pinsertar)
                btnsavemp.Visible = lblsavemp.Visible = false;

        }
        public CatModelos(int idUsuario, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            cbEmpresa.DrawItem += v.combos_DrawItem;
            v.creatItemsPersonalizadosCombobox(cbEmpresa, new string[] { "TRI VEHICULOS FUNCIONALES S. A DE C. V.", "TECNOSISTEMAS DIESEL S. A. DE C. V." }, "-- SELECCIONE UNA EMPRESA --",null);
        }
        void mostrar()
        {
            tbModelos.Rows.Clear();
            string sql = "select t1.idmodelo as id,upper(t1.modelo) as modelo,if(empresaMantenimiento=2,'TRI VEHICULOS FUNCIONALES S. A DE C. V.','TECNOSISTEMAS DIESEL S. A. DE C. V.') as empresa,upper(concat(t2.ApPaterno,' ',t2.ApMaterno,' ',t2.nombres)) as usuario,if(t1.status=1,'ACTIVO','NO ACTIVO') as Estatus,t1.empresaMantenimiento as idempresa from cmodelos as t1 inner join cpersonal as t2 on t2.idpersona=t1.usuariofkcpersonal;";
            MySqlCommand m = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = m.ExecuteReader();
            while (dr.Read())
            {
                tbModelos.Rows.Add(dr.GetString("id"), dr.GetString("modelo"), dr.GetString("empresa"), dr.GetString("usuario"), dr.GetString("Estatus"), dr.GetString("idempresa"));
            }
            dr.Close();
            v.c.dbcon.Close();
            tbModelos.ClearSelection();
        }
        private void txtgeteco_KeyPress(object sender, KeyPressEventArgs e) { v.letrasNumerosGuiones(e); }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e) { v.mover(sender, e, this); }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            if (!campos_vacios())
            {
                if (!editar)
                    insertar();
                else
                    edita();
            }

        }
        void edita()
        {
            string modelo = txtModelo.Text.Trim();
            int empresa = Convert.ToInt32(cbEmpresa.SelectedValue);
            if (!v.existeModelo(_idmodeloAnterior, modelo, empresa))
            {
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    var edit = v.c.insertar("update cmodelos set modelo='" + modelo + "', empresaMantenimiento='" + empresa + "' where idmodelo='" + _idmodeloAnterior + "';");
                    MessageBox.Show("Se edito el modelo correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var modif = v.c.insertar("Insert into modificaciones_sistema (form,idregistro,ultimaModificacion,usuariofkcpersonal,fechaHora,tipo,motivoActualizacion,empresa,area) values ('Catálogo de Modelos','" + _idmodeloAnterior + "','" + _modeloAnterior + ";" + _empresaAnterior + "','" + this.idUsuario + "',now(),'Actualización de Modelo','" + observaciones + "','1','1')");
                    limpiar();
                    mostrar();
                }
            }
        }
        void insertar()
        {
            string modelo = txtModelo.Text.Trim();
            int empresa = Convert.ToInt32(cbEmpresa.SelectedValue);
            if (!v.existeModelo(idModelo, modelo, empresa))
            {
                var inserta = v.c.insertar("insert into cmodelos (modelo,empresaMantenimiento,usuariofkcpersonal)values('" + txtModelo.Text + "','" + Convert.ToInt32(cbEmpresa.SelectedValue) + "','" + idUsuario + "')");
                MySqlCommand insrta_m = new MySqlCommand("Insert into modificaciones_sistema(form, idregistro, usuariofkcpersonal, fechaHora, tipo, empresa, area) values('Catálogo de Modelos', (select idmodelo from cmodelos where modelo = '" + modelo + "' and empresaMantenimiento = '" + empresa + "'), '" + this.idUsuario + "', now(), 'Inserción de Modelo', '1', '1')", v.c.dbconection());
                insrta_m.ExecuteNonQuery();
                MessageBox.Show("El modelo se inserto correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiar();
                mostrar();
            }
        }
        void limpiar()
        {
            txtModelo.Clear();
            cbEmpresa.SelectedIndex = 0;
            editar = false;
            pdelete.Visible = false;
            btnsavemp.Visible = lblsavemp.Visible = true;
            catUnidades cat = (catUnidades)Owner;
            cat.initializeModels();
            cat.bunidades();
        }
        void cargarDatos(DataGridViewCellEventArgs e)
        {
            _idmodeloAnterior = Convert.ToInt32(tbModelos.Rows[e.RowIndex].Cells[0].Value.ToString());
            est = v.getStatusInt(tbModelos.Rows[e.RowIndex].Cells[4].Value.ToString());
            if (Pdesactivar)
            {
                if (est == 0)
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
                txtModelo.Text = _modeloAnterior = tbModelos.Rows[e.RowIndex].Cells[1].Value.ToString();
                cbEmpresa.SelectedValue = _empresaAnterior = Convert.ToInt32(tbModelos.Rows[e.RowIndex].Cells[5].Value.ToString());
                tbModelos.ClearSelection();
                gbModelos.Visible = true;
                if (Pinsertar) pdelete.Visible = true;
                editar = true;
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                gbModelos.Text = "Actualizar Tipo De Licencia";
                lblsavemp.Text = "Guardar";
                btnsavemp.Visible = lblsavemp.Visible = false;
                if (est == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (Pinsertar)
                pdelete.Visible = true;
            else
            {
                MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelpa_Click(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (this.est == 0)
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
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Modelo";
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                try
                {
                    var res = v.c.insertar("update cmodelos set status='" + state + "' where idmodelo='" + _idmodeloAnterior + "';");
                    var res2 = v.c.insertar("Insert into modificaciones_sistema (form,idregistro,usuariofkcpersonal,fechaHora,motivoActualizacion,tipo,empresa,area) values ('Catálogo de Modelos','" + _idmodeloAnterior + "','" + this.idUsuario + "',now(),'" + edicion + "','" + msg + "activación de Modelo','1','1')");
                    MessageBox.Show("El Modelo ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                    mostrar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtModelo_TextChanged(object sender, EventArgs e)
        {
            if (editar)
                if ((_modeloAnterior != txtModelo.Text.Trim() || _empresaAnterior != Convert.ToInt32(cbEmpresa.SelectedValue)) && !string.IsNullOrWhiteSpace(txtModelo.Text) && cbEmpresa.SelectedIndex > 0)
                    btnsavemp.Visible = lblsavemp.Visible = true;
                else
                    btnsavemp.Visible = lblsavemp.Visible = false;
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if ((_modeloAnterior != txtModelo.Text.Trim() || _empresaAnterior != Convert.ToInt32(cbEmpresa.SelectedValue)) && !string.IsNullOrWhiteSpace(txtModelo.Text) && cbEmpresa.SelectedIndex > 0)
                if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                    btnsavemp_Click(null, e);
                else
                    limpiar();
            else
                limpiar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtModelo.Text) || cbEmpresa.SelectedIndex > 0)
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((_modeloAnterior != txtModelo.Text.Trim() || _empresaAnterior != Convert.ToInt32(cbEmpresa.SelectedValue)) && !string.IsNullOrWhiteSpace(txtModelo.Text) && cbEmpresa.SelectedIndex > 0)
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                    { DialogResult = DialogResult.None; DialogResult = DialogResult.None; }
                else
                    this.Close();
            }
        }

        private void tbModelos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tbModelos.Rows.Count > 0)
            {
                if ((_modeloAnterior != txtModelo.Text.Trim() || _empresaAnterior != Convert.ToInt32(cbEmpresa.SelectedValue)) && !string.IsNullOrWhiteSpace(txtModelo.Text) && cbEmpresa.SelectedIndex > 0)
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                        limpiar();
                    else
                        cargarDatos(e);
                else
                    cargarDatos(e);
            }
        }

        public bool campos_vacios()
        {
            if (!string.IsNullOrWhiteSpace(txtModelo.Text))
                if (cbEmpresa.SelectedIndex > 0)
                    return false;
                else
                {
                    MessageBox.Show("Seleccione una empresa de la lista desplegable.", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            else
            {
                MessageBox.Show("El campo \"Modelo\" se encuentra vacío.", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }

        private void CatModelos_Load(object sender, EventArgs e)
        {
            privilegiosPuestos();
            mostrar();
        }

        private void tbModelos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbModelos.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "ACTIVO")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }
    }
}
