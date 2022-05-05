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
    public partial class CatTipos : Form
    {
        validaciones v;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }

        int idUsuario, empresa, area, idtipo, status;
        string tipoAnterior, descrAnterior;
        bool editar = false, mensaje = false;
        public CatTipos(int idUsuario, int empresa, int area, validaciones v)
        {
            this.v = v;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            InitializeComponent();
        }
        public void mostrar()
        {
            if (Pinsertar || Peditar)
            {
                gbpuesto.Visible = true;
            }
            if (Pconsultar)
            {
                tbtipos.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                btnsave.Visible = lblsave.Visible = false;
                editar = true;

            }
        }
        public void privilegiosPuestos()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {
                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                if (privilegiosTemp.Length > 3)
                {
                    Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }
            }
            mostrar();
        }
        private void txtgettipo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }
        public void MostrarDatos()
        {
            tbtipos.Rows.Clear();
            String sql = "select idcattipos as id,t1.tipo as tipo,t1.Descripcion as descripcion,upper(concat(coalesce(t2.ApPaterno,''),' ',coalesce(t2.ApMaterno,''),' ',coalesce(t2.nombres,'')))as nombre,if(t1.status=0,'NO ACTIVO','ACTIVO') as estatus from cattipos as t1 inner join cpersonal as t2 on t2.idPersona=t1.usuariofkcpersonal WHERE t1.empresa='" + empresa + "' and t1.area='" + area + "' order by tipo";
            MySqlCommand cmd = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tbtipos.Rows.Add(dr.GetString("id"), dr.GetString("tipo"), dr.GetString("descripcion"), dr.GetString("nombre"), dr.GetString("estatus"));
            }
            dr.Close();
            v.c.dbcon.Close();
            tbtipos.ClearSelection();
        }
        void _editar()
        {
            string tipo = v.mayusculas(txtgettipo.Text.ToLower());
            string descrip = v.mayusculas(txtgetdescripción.Text.ToLower());
            if (string.IsNullOrWhiteSpace(txtgettipo.Text.Trim()))
            {
                MessageBox.Show(v.mayusculas("El campo \"Tipo\" no puede Estar vacio".ToLower()), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!v.yaExisteLicencia(tipo, idtipo, empresa, area))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        var res = v.c.insertar("Update cattipos set tipo='" + txtgettipo.Text.Trim() + "',Descripcion='" + txtgetdescripción.Text.Trim() + "' where idcattipos='" + idtipo + "' ");
                        var res2 = v.c.insertar("Insert into modificaciones_sistema (form,idregistro,ultimaModificacion,usuariofkcpersonal,fechaHora,tipo,motivoActualizacion,empresa,area) values ('Catálogo de Tipos','" + idtipo + "','" + tipoAnterior + ";" + descrAnterior + "','" + this.idUsuario + "',now(),'Actualización de Tipo De Licencia','" + observaciones + "','" + this.empresa + "','" + this.area + "')");
                        if (!mensaje) MessageBox.Show("Se Ha Actualizado el Tipo de Licencia Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        catPersonal cat = (catPersonal)Owner;
                        var _tipo = cat.cbtipo.SelectedValue ?? 0;
                        limpiar();
                        cat.mostrarLicencias();
                        cat.busemp();
                        cat.cbtipo.SelectedValue = _tipo;
                        _tipo = 0;
                    }
                }
            }


        }
        private void btnsave_Click(object sender, EventArgs e)
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


        void insertar()
        {
            string tipo = v.mayusculas(txtgettipo.Text.ToLower());
            if (string.IsNullOrWhiteSpace(txtgettipo.Text.Trim()))
            {
                MessageBox.Show("El campo tipo no puede quedar vacio", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!v.yaExisteLicencia(tipo, idtipo, empresa, area))
                {
                    var res = v.c.insertar("Insert into cattipos (tipo,descripcion,usuariofkcpersonal,empresa,area) values ('" + txtgettipo.Text.Trim() + "','" + txtgetdescripción.Text.Trim() + "','" + this.idUsuario + "','" + empresa + "','" + area + "')");
                    MessageBox.Show("Se ha insertado el tipo de licencia correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var res2 = v.c.insertar("Insert into modificaciones_sistema (form,idregistro,usuariofkcpersonal,fechaHora,tipo,empresa,area) values ('Catálogo de Tipos',(Select idcattipos as id from cattipos as t1 where t1.Tipo='" + tipo + "' and t1.empresa='" + empresa + "' and t1.area='" + area + "' ),'" + this.idUsuario + "',now(),'Inserción de Tipo De Licencia','" + this.empresa + "','" + this.area + "')");
                    catPersonal cat = (catPersonal)Owner;
                    var _tipo = cat.cbtipo.SelectedValue ?? 0;
                    limpiar();
                    cat.mostrarLicencias();
                    cat.busemp();
                    cat.cbtipo.SelectedValue = _tipo;
                    _tipo = 0;
                }
            }
        }
        private void CatTipos_Load(object sender, EventArgs e)
        {
            privilegiosPuestos();
            if (Pconsultar)
            {
                MostrarDatos();
            }
        }

        private void tbtipos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbtipos.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "ACTIVO")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }


        private void txtgettipo_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            if (!v.mayusculas(txtgettipo.Text.Trim().ToLower()).Equals(tipoAnterior) || !v.mayusculas(txtgetdescripción.Text.Trim().ToLower()).Equals(descrAnterior) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    mensaje = true;
                    btnsave_Click(null, e);
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

        private void btndelete_Click(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (this.status == 0)
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
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Tipo de Licencia";
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                try
                {
                    var res = v.c.insertar("Update cattipos set status='" + state + "' where idcattipos='" + idtipo + "'");
                    var res2 = v.c.insertar("Insert into modificaciones_sistema (form,idregistro,usuariofkcpersonal,fechaHora,tipo,motivoActualizacion,empresa,area) values ('Catálogo de Tipos','" + idtipo + "','" + this.idUsuario + "',now(),'" + msg + "activación de Tipo De Licencia','" + edicion + "','" + this.empresa + "','" + this.area + "')");
                    MessageBox.Show("El Tipo ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                    MostrarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lblsave_Click(object sender, EventArgs e)
        {
        }

        private void gbpuesto_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void lbltitle_MouseDown_1(object sender, MouseEventArgs e) { v.mover(sender, e, this); }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtgettipo.Text) || !string.IsNullOrWhiteSpace(txtgetdescripción.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (tipoAnterior != v.mayusculas(txtgettipo.Text.Trim().ToLower()) || descrAnterior != v.mayusculas(txtgetdescripción.Text.Trim().ToLower()))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                gbpuesto.Text = "Nuevo Tipo De Licencia";
                btnsave.Visible = lblsave.Visible = true;
                txtgettipo.Focus();
                _limpiaVariables();
            }
            if (Pconsultar)
            {
                MostrarDatos();
            }
            txtgetdescripción.Clear();
            txtgettipo.Clear();
            txtgettipo.Focus();
            pdelete.Visible = pcancel.Visible = false;
            mensaje = false;
            catPersonal cat = (catPersonal)Owner;
            if (cat.cbtipo.Enabled == false)
            {
                cat.cbtipo.Enabled = true;
            }
        }
        private void txtgetdescripción_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsave_Click(null, e);
            else
                v.letrasnumerosdiagonalyguion(e);
        }
        void PasarDatos(DataGridViewCellEventArgs e)
        {
            try
            {
                idtipo = Convert.ToInt32(tbtipos.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(tbtipos.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (Pdesactivar)
                {
                    if (status == 0)
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
                    txtgettipo.Text = tipoAnterior = v.mayusculas(tbtipos.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    txtgetdescripción.Text = descrAnterior = v.mayusculas(tbtipos.Rows[e.RowIndex].Cells[2].Value.ToString().ToLower());
                    tbtipos.ClearSelection();
                    gbpuesto.Visible = true;
                    if (Pinsertar) pcancel.Visible = true;
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    gbpuesto.Text = "Actualizar Tipo De Licencia";
                    lblsave.Text = "Guardar";
                    btnsave.Visible = lblsave.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void _limpiaVariables()
        {
            tipoAnterior = "";
            descrAnterior = "";
        }
        private void tbtipos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (idtipo > 0 && Peditar && (!v.mayusculas(txtgettipo.Text.Trim().ToLower()).Equals(tipoAnterior) || !v.mayusculas(txtgetdescripción.Text.Trim().ToLower()).Equals(descrAnterior)) && status == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        mensaje = true;
                        btnsave_Click(null, e);
                    }
                    else
                    {
                        PasarDatos(e);
                    }
                }
                else
                {
                    PasarDatos(e);
                }
            }
        }
        public void modificaciones(object sender, EventArgs e)
        {
            if (editar)
            {
                if ((txtgetdescripción.Text.Trim() != (descrAnterior ?? "").ToUpper() || txtgettipo.Text.Trim() != tipoAnterior.ToUpper()) && (!string.IsNullOrWhiteSpace(txtgettipo.Text.Trim())))
                {
                    btnsave.Visible = true;
                    lblsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = false;
                    lblsave.Visible = false;
                }
            }
        }
    }
}
