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
    public partial class catGiros : Form
    {
        validaciones v = new validaciones();
        conexion c = new conexion();
        int idUsuario, empresa, area, statusAnterior;
        string idclasificacionAnterior, clasificacionAnterior;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        string giro_tem;
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool editar;
        bool yaAparecioMensaje;
        public catGiros(int idUsuario, int empresa, int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            privilegios();
        }

        private void txtgetpuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsave_Click(null, e);
            else
                v.giro_empresa(e);
        }
        public void iniClasificaciones()
        {
            tbgiros.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT idgiro,UPPER(giro),(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) FROM cpersonal WHERE idpersona = usuariofkcpersonal),if(status=1,'ACTIVO',CONCAT('NO ACTIVO')) FROM cgiros WHERE empresa='" + empresa + "' AND area='" + area + "' ORDER BY giro ASC");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tbgiros.Rows.Add(dt.Rows[i].ItemArray);
            }
            tbgiros.ClearSelection();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                }
                else
                {
                    _editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void limpiar()
        {
            txtgetclasificacion.Clear();
            if (Pinsertar)
            {
                btnsave.BackgroundImage = Properties.Resources.save;
                editar = false;
                btnsave.Visible = true;
                lblsave.Visible = true;
                txtgetclasificacion.Focus();
            }
            giro_tem = idclasificacionAnterior;
            idclasificacionAnterior = null;
            clasificacionAnterior = null;
            pcancel.Visible = pdelete.Visible = false;
            catProveedores cat = (catProveedores)Owner;
            v.iniCombos("SELECT idgiro,upper(giro) as giro from cgiros where status='1' order by giro asc", cat.cbgiros, "idgiro", "giro", "-SELECIONE UN GIRO-");
            v.iniCombos("SELECT idgiro, upper(giro) as giro from cgiros where status = '1' order by giro asc", cat.cbgirosb, "idgiro", "giro", " - SELECIONE UN GIRO - ");
            cat.insertarums();
            yaAparecioMensaje = false;

        }
        protected internal void insertar()
        {
            string clasif = v.mayusculas(txtgetclasificacion.Text.Trim().ToLower());
            if (!string.IsNullOrWhiteSpace(clasif))
            {
                if (!v.existeClasificacionEmpresa(clasif))
                {
                    if (v.c.insertar("INSERT INTO cgiros(giro,usuariofkcpersonal,empresa,area) VALUES('" + clasif + "','" + idUsuario + "','" + empresa + "','" + area + "')"))
                    {
                        if (v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Clasificaciones de Empresas',(SELECT idgiro FROM cgiros WHERE giro='" + clasif + "'),'Inserción de Clasificación de Empresa','" + idUsuario + "',NOW(),'Inserción de Clasificación de Empresa','" + empresa + "','" + area + "')"))
                        {
                            MessageBox.Show(" La Clasificación de Empresa se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            iniClasificaciones();
                            catProveedores cat = (catProveedores)Owner;
                            object res = cat.cbgiros.SelectedValue;
                            limpiar();

                            if (Convert.ToInt32(res) > 0)
                            {
                                cat.cbgiros.SelectedValue = res;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("La Clasificación de Empresa No Puede Estar Vacía Para Inserción", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected internal void _editar()
        {
            string clasif = v.mayusculas(txtgetclasificacion.Text.Trim().ToLower());
            if (!string.IsNullOrWhiteSpace(clasif))
            {
                if (!v.existeClasificacionEmpresa(clasif))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        if (v.c.insertar("UPDATE cgiros SET giro = '" + clasif + "' WHERE idgiro='" + idclasificacionAnterior + "'"))
                        {
                            if (v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Clasificaciones de Empresas','" + idclasificacionAnterior + "','" + v.mayusculas(clasificacionAnterior.ToLower()) + "','" + idUsuario + "',NOW(),'Actualización de Clasificación de Empresa','" + observaciones + "','" + empresa + "','" + area + "')"))
                            {
                                if (!yaAparecioMensaje) MessageBox.Show(" La Clasificación de Empresa se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                iniClasificaciones();
                                catProveedores cat = (catProveedores)Owner;
                                object res = cat.cbgiros.SelectedValue;
                                limpiar();

                                if (Convert.ToInt32(res) > 0)
                                {
                                    cat.cbgiros.SelectedValue = res;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("La Clasificación de Empresa No Puede Estar Vacía Para Actualización", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void catGiros_Load(object sender, EventArgs e)
        {
            iniClasificaciones();
        }

        private void tbgiros_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbgiros.Columns[e.ColumnIndex].Name == "statusDataGrid")
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

        private void txtgetclasificacion_TextChanged(object sender, EventArgs e)
        {
            if (editar)
            {
                if (statusAnterior == 1 && !string.IsNullOrWhiteSpace(txtgetclasificacion.Text.Trim()) && !clasificacionAnterior.Equals(txtgetclasificacion.Text.Trim()))
                {
                    btnsave.Visible = lblsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = lblsave.Visible = false;
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgetclasificacion.Text.Trim()) && !clasificacionAnterior.Equals(txtgetclasificacion.Text.Trim()))
            {
                if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsave_Click(null, e);
                }

            }
            limpiar();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (this.statusAnterior == 0)
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
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación De La Clasificación de Empresa";
            obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 55, obs.lblinfo.Location.Y);
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                try
                {
                    String sql = "UPDATE cgiros SET status = " + state + " WHERE idgiro  = " + idclasificacionAnterior;
                    if (v.c.insertar(sql))
                    {
                        if (v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Clasificaciones de Empresas','" + idclasificacionAnterior + "','" + msg + "activación de Clasificación de Empresa','" + idUsuario + "',NOW(),'" + msg + "activación de Clasificación de Empresa','" + edicion + "','" + empresa + "','" + area + "')"))
                        {
                            MessageBox.Show("La Clasificación ha sido " + msg + "activada Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            iniClasificaciones();
                            catProveedores cat = (catProveedores)Owner;
                            object res = cat.cbgiros.SelectedValue;
                            limpiar();
                            cat.giros_desactivados(res.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtgetclasificacion_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtgetclasificacion.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (clasificacionAnterior != txtgetclasificacion.Text && !string.IsNullOrWhiteSpace(txtgetclasificacion.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void tbgiros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(idclasificacionAnterior) && Peditar && !string.IsNullOrWhiteSpace(txtgetclasificacion.Text.Trim()) && !txtgetclasificacion.Text.Trim().Equals(clasificacionAnterior))
            {
                if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsave_Click(null, e);
                }
            }
            guardarReporte(e);
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (Pdesactivar || Peditar)
                {
                    idclasificacionAnterior = tbgiros.Rows[e.RowIndex].Cells[0].Value.ToString();
                    statusAnterior = v.getStatusInt(tbgiros.Rows[e.RowIndex].Cells[3].Value.ToString());
                }
                if (Pdesactivar)
                {
                    if (statusAnterior == 0)
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelete.Text = "Reactivar";
                    }
                    else
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar";
                    }
                    pdelete.Visible = true;
                }
                if (Peditar)
                {
                    clasificacionAnterior = txtgetclasificacion.Text = tbgiros.Rows[e.RowIndex].Cells[1].Value.ToString();
                    editar = true;
                    tbgiros.ClearSelection();
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    gbpuesto.Text = "Actualizar Clasificación";
                    lblsave.Text = "Guardar";
                    btnsave.Visible = lblsave.Visible = false;
                    if (Pinsertar) pcancel.Visible = true;
                    if (statusAnterior == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }
        public void privilegios()
        {
            string sql = "SELECT insertar,consultar,editar,desactivar FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "' and namform = '" + Name + "'";
            MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
            MySqlDataReader mdr = cmd.ExecuteReader();
            mdr.Read();
            Pconsultar = v.getBoolFromInt(mdr.GetInt32("consultar"));
            Pinsertar = v.getBoolFromInt(mdr.GetInt32("insertar"));
            Peditar = v.getBoolFromInt(mdr.GetInt32("editar"));
            Pdesactivar = v.getBoolFromInt(mdr.GetInt32("desactivar"));
            mdr.Close();
            c.dbconection().Close();
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbpuesto.Visible = true;
            }
            if (Pconsultar)
            {
                tbgiros.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsave.Text = "Editar Anaquel";
                editar = true;
            }
        }
    }
}
