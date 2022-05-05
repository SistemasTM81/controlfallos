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
    public partial class NombresFamilias : Form
    {
        validaciones v;
        string familia_anteriror;
        int idUsuario, _status, _idtemp, empresa, area;
        bool editar = false, reactivar;
        public NombresFamilias(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            this.idUsuario = idUsuario;
            InitializeComponent();

            this.empresa = empresa; this.area = area;
        }
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddfamilia.Visible = true;
            }
            if (Pconsultar)
            {
                tbfamilia.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
        }
        public void establecerPrivilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "catRefacciones")).ToString().Split('/');
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
        public void limpiar()
        {
            _idtemp = 0;
            txtnombre.Clear();
            pdeletefam.Visible = false;
            pcancel.Visible = false;
            btnsave.Visible = true;
            lblsave.Visible = true;
            editar = false;
            btnsave.BackgroundImage = controlFallos.Properties.Resources.save; muestra_familias();
            marcas m = (marcas)Owner;
            m.insertarums();
            if (Pinsertar) txtnombre.Focus();
        }
        public void muestra_familias()
        {
            tbfamilia.Rows.Clear();
            string sql = "Select t1.idcnFamilia as id,upper(t1.familia) as fam,upper(concat(coalesce(t2.ApPaterno,''),' ',coalesce(t2.ApMaterno,''),' ',coalesce(t2.nombres,''))) as nom, t1.status as estatus from cnfamilias as t1 inner join cpersonal as t2 on t2.idpersona=t1.usuariofkcpersonal where t1.empresa='" + empresa + "' order by t1.familia asc;";
            MySqlCommand cmd = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tbfamilia.Rows.Add(dr.GetString("id"), dr.GetString("fam"), dr.GetString("nom"), v.getStatusString(dr.GetInt32("estatus")));
            }
            tbfamilia.ClearSelection();
        }
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    _insertar();
                }
                else
                {
                    _editar();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void cambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (_idtemp > 0 && (!txtnombre.Text.Trim().Equals(familia_anteriror)) && !string.IsNullOrWhiteSpace(txtnombre.Text))
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
        void _editar()
        {
            string familia = txtnombre.Text.Trim();
            try
            {
                if (v.NombreFamilia(familia))
                {
                    if (!v._existeFamilia(familia, empresa))
                    {
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                            MySqlCommand cmd = new MySqlCommand("update cnfamilias set familia='" + familia + "' where idcnFamilia='" + _idtemp + "'", v.c.dbconection());
                            cmd.ExecuteNonQuery();
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Nombres de Familias','" + _idtemp + "','" + familia_anteriror + "','" + idUsuario + "',NOW(),'Actualización de Familia','" + observaciones + "','" + empresa + "','" + area + "')");
                            MessageBox.Show("Se ha Actualizado La Familia Existosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            marcas cat = (marcas)Owner;
                            var _familia = cat.cbfamilia.SelectedValue; var _desc = cat.cbdesc.SelectedValue ?? 0;
                            limpiar();
                            v.iniCombos("SELECT idcnfamilia as id, familia FROM cnfamilias WHERE status='1'", cat.cbfamilia, "id", "familia", "--SELECCIONE UNA FAMILIA--");
                            if (Convert.ToInt32(_familia) > 0)
                            {
                                cat.cbfamilia.SelectedValue = _familia;
                                cat.cbdesc.SelectedValue = _desc;
                                _familia = 0; _desc = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void _insertar()
        {
            string familia = txtnombre.Text.Trim();
            if (v.NombreFamilia(familia))
            {
                if (!v._existeFamilia(familia, empresa))
                {
                    try
                    {
                        if (v.c.insertar("Insert into cnfamilias(familia,usuariofkcpersonal,empresa)values('" + familia + "','" + this.idUsuario + "','" + empresa + "')"))
                        {
                            v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Nombres de Familias',(SELECT idcnFamilia FROM cnfamilias WHERE upper(familia)='" + familia.ToUpper() + "' and empresa='" + empresa + "'),'" + familia + "','" + idUsuario + "',NOW(),'Inserción de Familia de Refacción','" + empresa + "','" + area + "')");
                            marcas cat = (marcas)Owner;
                            var _familia = cat.cbfamilia.SelectedValue; var _desc = cat.cbdesc.SelectedValue ?? 0;
                            v.iniCombos("SELECT idcnfamilia as id, familia FROM cnfamilias WHERE status='1'", cat.cbfamilia, "id", "familia", "--SELECCIONE UNA FAMILIA--");
                            if (Convert.ToInt32(_familia) > 0)
                            {
                                cat.cbfamilia.SelectedValue = _familia;
                                cat.cbdesc.SelectedValue = _desc;
                            }
                            MessageBox.Show("Se ha Insertado La Familia Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        limpiar();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtnombre.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (familia_anteriror != txtnombre.Text.Trim() && !string.IsNullOrWhiteSpace(txtnombre.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void NombresFamilias_Load(object sender, EventArgs e)
        {
            muestra_familias();
            establecerPrivilegios();
        }

        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            if (_idtemp > 0)
            {
                string msg;
                int status;
                if (reactivar)
                {
                    status = 1;
                    msg = "Re";
                }
                else
                {
                    status = 0;
                    msg = "Des";

                }
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Nombre de familia";
                obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 30, obs.lblinfo.Location.Y);
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    try
                    {
                        v.c.insertar("UPDATE cnfamilias as t1 left JOIN cfamilias as t2 ON t2.familiafkcnfamilias = t1. idcnfamilia left JOIN cmarcas as t3 ON t3.descripcionfkcfamilias=t2.idfamilia SET t1.status='" + status + "', t2.status='" + status + "' , t3.status='" + status + "' WHERE t1.idcnfamilia='" + this._idtemp + "'");
                        MessageBox.Show("La Familia de Refacciones ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        v.c.insertar("insert into modificaciones_sistema (form,idregistro,ultimaModificacion,usuariofkcpersonal,fechaHora,Tipo,motivoActualizacion,empresa,area) values('Catálogo de Refacciones - Nombres de Familias','" + _idtemp + "','','" + idUsuario + "',now(),'" + msg + "activación de Nombre de Familia','" + edicion + "','" + this.empresa + "','" + this.area + "')");

                        limpiar();




                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }


        }

        private void btncancel_Click(object sender, EventArgs e)
        {

            if (_idtemp > 0 && _status == 1 && Peditar && (!txtnombre.Text.Equals(familia_anteriror)) && !string.IsNullOrWhiteSpace(txtnombre.Text))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
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

        private void panel1_MouseDown(object sender, MouseEventArgs e) { v.mover(sender, e, this); }
        private void gbaddfamilia_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void txtnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsave_Click(null, e);
            else
                v.letrasynumeros(e);
        }

        private void tbfamilia_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (_idtemp > 0 && _status == 1 && Peditar && (!txtnombre.Text.Equals(familia_anteriror)) && !string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        btnsave_Click(null, e);
                    }
                    else
                    {
                        guardar_reporte(e);
                    }
                }
                else
                {
                    guardar_reporte(e);
                }

            }
        }
        void guardar_reporte(DataGridViewCellEventArgs e)
        {
            try
            {
                _idtemp = Convert.ToInt32(tbfamilia.Rows[e.RowIndex].Cells[0].Value.ToString());
                _status = v.getStatusInt(tbfamilia.Rows[e.RowIndex].Cells[3].Value.ToString());
                btnsave.Visible = lblsave.Visible = false;
                if (Pdesactivar)
                {
                    pdeletefam.Visible = true;
                    if (_status == 0)
                    {
                        reactivar = true;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletefam.Text = "Reactivar";
                    }
                    else
                    {
                        reactivar = false;
                        btndeleteuser.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletefam.Text = "Deactivar";
                    }
                }
                if (Peditar)
                {
                    txtnombre.Text = familia_anteriror = tbfamilia.Rows[e.RowIndex].Cells[1].Value.ToString();
                    btnsave.BackgroundImage = Properties.Resources.pencil;
                    lblsave.Text = "Guardar";
                    gbaddfamilia.Text = "Actualizar Familia de Refacciones";
                    pcancel.Visible = true;
                    editar = true; btnsave.Visible = lblsave.Visible = false;
                    if (_status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbfamilia_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfamilia.Columns[e.ColumnIndex].Name == "Estatus")
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
    }
}
