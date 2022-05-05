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
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;

namespace controlFallos
{
    public partial class catEmpresas : Form
    {
        validaciones v;
        bool editbussines = false;
        int bussinestemp, idUsuario, status, empresa, area;
        bool yaAparecioMensaje = false;
        string nombreAnterior, imgserialAnt;
        public catEmpresas(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            busqempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel); DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            busqempresa.ColumnHeadersDefaultCellStyle = d;
        }
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        public void privilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                if (Convert.ToInt32(privilegiosTemp.Length) > 3)
                {
                    pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }
            }
            mostrar();
        }
        void mostrar()
        {
            if (pinsertar || peditar)
                gbaddbussiness.Visible = true;
            if (pconsultar)
                gbcempresa.Visible = true;
            if (peditar)
                label23.Visible = label1.Visible = true;
        }

        private void txtgetnempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsavemp_Click(null, e);
            else
                v.paraEmpresas(e);
        }

        private void txtgetcempresa_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraUnidades(e);
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editbussines)
                    insertarEmpresa();
                else
                {
                    if ((empresa == 2 || empresa == 3) && area == 2)
                        editarlogo();
                    else
                        editarEmpresa();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void limpiar()
        {
            if (pinsertar)
            {
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                gbaddbussiness.Text = "Agregar Empresa";
                editbussines = false;
                txtgetnempresa.Focus();
            }
            yaAparecioMensaje = false;
            txtgetnempresa.Clear();
            pEliminarEmpresa.Visible = false;
            pCancel.Visible = false;
            btnsavemp.Visible = true;
            lblsavemp.Visible = true;
            busqempresa.ClearSelection();
            bussinestemp = 0;

            txtgetnempresa.ReadOnly = false;
            status = 0;
            nombreAnterior = null;
            if (empresa == 1 && area == 1)
            {
                catUnidades cat = (catUnidades)this.Owner; btnsavemp.Visible = true;
                lblsavemp.Visible = true;
                cat.busqempresas();
                cat.busqempresasBusq();
                cat.bunidades();
                if (cat.editar)
                    cat.csetEmpresa.SelectedValue = cat.EmpresaAnterior;
            }
            else if ((empresa == 2 || empresa == 3) && area == 2)
            {
                lnkrestablecer.Visible = false;
                pblogo.BackgroundImage = controlFallos.Properties.Resources.image;
                imgserial = imgserialAnt = null;
            }

        }

        public void insertarEmpresa()
        {
            Regex valida = new Regex(@"[a-z0-9]");
            Regex valida2 = new Regex(@"[a-z0-9]\.\[a-z]");

            string nombre = v.mayusculas(txtgetnempresa.Text.ToLower()).Trim();
            if (valida.IsMatch(nombre))
            {
                if (!v.formularioEmpresa(nombre) && !v.existenombreEmpresa(nombre))
                {
                    string sql = "";
                    if (empresa == 2 & area == 2)
                        sql = "INSERT INTO cempresas (nombreEmpresa,usuariofkcpersonal,logo,empresa,area) VALUES ('" + nombre.Trim() + "','" + this.idUsuario + "','" + imgserial + "','" + empresa + "','" + area + "')";
                    else
                        sql = "INSERT INTO cempresas (nombreEmpresa,usuariofkcpersonal,empresa,area) VALUES ('" + nombre.Trim() + "','" + this.idUsuario + "','" + empresa + "','" + area + "')";
                    if (v.c.insertar(sql))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Empresas',(SELECT idempresa FROM cempresas WHERE nombreEmpresa='" + nombre + "'),'Inserción de Empresa','" + idUsuario + "',NOW(),'Inserción de Empresa','" + empresa + "','" + area + "')");
                        var id = v.getaData("SELECT idempresa FROM cempresas WHERE nombreEmpresa='" + nombre + "'");
                        limpiar();
                        if (empresa == 1 && area == 1)
                        {
                            catUnidades cat = (catUnidades)Owner;
                            object _idanterior = cat.csetEmpresa.SelectedValue;
                            var _area = 0;
                            if (Convert.ToInt32(cat.cbareas.SelectedValue) > 0) { _area = Convert.ToInt32(cat.cbareas.SelectedValue); }
                            cat.busqempresas();
                            if (Convert.ToInt32(_idanterior) > 0) { cat.csetEmpresa.SelectedValue = _idanterior ?? 0; }
                            if (_area > 0) { cat.cbareas.SelectedValue = _area; } else { if (Convert.ToInt32(_idanterior) > 0) { cat.cbareas.SelectedValue = 0; } else { cat.cbareas.DataSource = null; } }
                        }
                        else if ((empresa == 2 || empresa == 3) && area == 2)
                        {
                            OrdenDeCompra odc = (OrdenDeCompra)Owner;
                            odc.CargarEmpresas();
                            //odc.comboBoxFacturar.SelectedValue = id;

                        }
                        MessageBox.Show("La Empresa se Ha Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar(); if (pconsultar)
                            busqempr();
                    }
                }
            }
            else
                MessageBox.Show("EL nombre de la empresa no es valido", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void editarlogo()
        {
            if (bussinestemp > 0)
            {
                if (status == 0)
                {
                    MessageBox.Show("No se Puede Actualizar una Empresa Desactivada Para El Sistema", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        limpiar();
                }
                else
                {

                    string nombre = v.mayusculas(txtgetnempresa.Text.ToLower());
                    if (!v.formularioEmpresa(nombre))
                    {
                        if (nombre.Equals(nombreAnterior) && imgserial.Equals(imgserialAnt ?? ""))
                        {
                            MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                limpiar();
                        }
                        else
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            if (obs.ShowDialog() == DialogResult.OK)
                            {
                                string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                String sql = "UPDATE cempresas SET  logo =LTRIM(RTRIM('" + imgserial + "')),nombreEmpresa='" + nombre.Trim() + "' WHERE idEmpresa = " + this.bussinestemp;
                                if (v.c.insertar(sql))
                                {
                                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Empresas','" + bussinestemp + "','" + imgserialAnt + "','" + idUsuario + "',NOW(),'Actualización de Empresa','" + observaciones + "','" + empresa + "','" + area + "')");
                                    if (!yaAparecioMensaje)
                                        MessageBox.Show("Se Ha Actualizado La Empresa Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    limpiar();
                                    if (pconsultar)
                                        busqempr();

                                }
                            }
                        }
                    }
                }
            }
            else
                MessageBox.Show("Seleccione una Empresa Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void editarEmpresa()
        {
            if (bussinestemp > 0)
            {
                string nombre = v.mayusculas(txtgetnempresa.Text.ToLower());
                if (!v.formularioEmpresa(nombre))
                {

                    if (!v.existeEmpresaActualizar(nombre, nombreAnterior))
                    {
                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                            object _empresa = null, _area = 0;
                            catUnidades cuni = null;
                            if (empresa == 1 && area == 1)
                            {
                                cuni = (catUnidades)Owner;
                                _empresa = cuni.csetEmpresa.SelectedValue ?? 0;
                                _area = 0;
                                if (Convert.ToInt32(cuni.cbareas.SelectedValue) > 0) { _area = Convert.ToInt32(cuni.cbareas.SelectedValue); }
                            }
                            String sql = "UPDATE cempresas SET  nombreEmpresa =LTRIM(RTRIM('" + nombre.Trim() + "')) WHERE idEmpresa = " + this.bussinestemp;
                            if (v.c.insertar(sql))
                            {
                                var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Empresas','" + bussinestemp + "','" + nombreAnterior + "','" + idUsuario + "',NOW(),'Actualización de Empresa','" + observaciones + "','" + empresa + "','" + area + "')");
                                if (!yaAparecioMensaje)

                                    MessageBox.Show("Se Ha Actualizado La Empresa Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                                if (empresa == 1 && area == 1)
                                {
                                    if (Convert.ToInt32(_empresa) > 0) { cuni.csetEmpresa.SelectedValue = _empresa; }
                                    if (Convert.ToInt32(_area) > 0) { cuni.cbareas.SelectedValue = _area; } else { if (Convert.ToInt32(_empresa) > 0) { cuni.cbareas.SelectedValue = 0; } else { cuni.cbareas.DataSource = null; } }
                                }
                                if (pconsultar) busqempr();
                            }
                        }
                    }
                }
            }
        }
        public void busqempr()
        {
            busqempresa.Rows.Clear();

            String sql = "";
            if (empresa == 1 && area == 1)
                sql = "SELECT t1.idempresa,upper(t1.nombreEmpresa) as nombreEmpresa, UPPER(CONCAT(coalesce(t2.nombres,''),' ',coalesce(t2.apPaterno,''),' ',coalesce(t2.apMaterno,''))) as persona, t1.status,t1.empresa,t1.area FROM cempresas as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona WHERE t1.empresa='" + empresa + "' AND t1.area='" + area + "' ORDER BY nombreEmpresa ASC";
            else
                sql = "SELECT t1.idempresa,upper(t1.nombreEmpresa) as nombreEmpresa, UPPER(CONCAT(coalesce(t2.nombres,''),' ',coalesce(t2.apPaterno,''),' ',coalesce(t2.apMaterno,''))) as persona, t1.status,t1.empresa,t1.area FROM cempresas as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona  ORDER BY nombreEmpresa ASC";
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                busqempresa.Rows.Add(dr.GetInt32("idempresa"), dr.GetString("nombreEmpresa"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")), dr.GetString("empresa"), dr.GetString("area"));
            }
            dr.Close();
            v.c.dbcon.Close();
            busqempresa.ClearSelection();
        }

        private void busqempresa_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (empresa == 1 && area == 1)
                {
                    if (bussinestemp > 0 && peditar && !v.mayusculas(txtgetnempresa.Text.ToLower()).Trim().Equals(nombreAnterior) && status == 1)
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            btnsavemp_Click(sender, e);
                        }
                    }
                }
                else
                {
                    if (bussinestemp > 0 && peditar && !imgserialAnt.Equals(imgserial))
                    {
                        if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            yaAparecioMensaje = true;
                            btnsavemp_Click(sender, e);
                        }
                    }
                }
                guardarReporte(e);
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                limpiar();
                this.bussinestemp = Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(busqempresa.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (pdesactivar)
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
                    pEliminarEmpresa.Visible = (Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[4].Value) == empresa && Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[5].Value) == area);
                }
                if (peditar)
                {
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    gbaddbussiness.Text = "Actualizar Empresa";
                    txtgetnempresa.Text = nombreAnterior = v.mayusculas(busqempresa.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    editbussines = true;
                    if (pinsertar) pCancel.Visible = true;
                    busqempresa.ClearSelection();
                    txtgetnempresa.ReadOnly = (Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[4].Value) != empresa && Convert.ToInt32(busqempresa.Rows[e.RowIndex].Cells[5].Value) != area);
                    if ((empresa == 2 || empresa == 3) && area == 2)
                    {
                        imgserialAnt = imgserial = v.getaData("SELECT COALESCE(logo,'') FROM cempresas WHERE idempresa= '" + bussinestemp + "'").ToString();
                        if (imgserial != "")
                        {
                            pblogo.BackgroundImage = v.StringToImage2(imgserial);
                            lnkrestablecer.Visible = true;
                        }
                        else
                            pblogo.BackgroundImage = controlFallos.Properties.Resources.image;
                    }
                    lblsavemp.Visible = btnsavemp.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Usted No Cuenta con Privilegios para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (!v.mayusculas(txtgetnempresa.Text.ToLower()).Trim().Equals(nombreAnterior) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    editarEmpresa();
                }
                else
                    limpiar();
            }
            else
                limpiar();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (status == 0)
            {
                state = 1;
                msg = "Re";
            }
            else
            {
                state = 0;
                msg = "Des";
            }

            observacionesEdicion obs = new observacionesEdicion(v);
            obs.Owner = this;
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación la Empresa";
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                string sql = "UPDATE cempresas SET status = '" + state + "' WHERE idEmpresa = '" + this.bussinestemp + "'";
                if (v.c.insertar(sql))
                {
                    if (this.empresa == 1 && area == 1)
                    {
                        v.c.insertar(string.Format("UPDATE cempresas as t1 LEFT JOIN careas as t2 ON t2.empresafkcempresas=t1.idempresa LEFT JOIN cunidades as t3 ON t3.areafkcareas=t2.idarea SET t1.status={0},t2.status={0},t3.status={0} WHERE t1.idempresa={1}", state, bussinestemp));
                        v.c.insertar(string.Format("UPDATE cempresas as t1 LEFT JOIN  careas as t2 ON t2.empresafkcempresas=t1.idempresa LEFT JOIN cservicios as t3 ON t3.areafkcareas=t2.idarea SET t3.status={0} WHERE t1.idempresa={1}", state, bussinestemp));
                    }
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Empresas','" + bussinestemp + "','" + msg + "activación de Empresa','" + idUsuario + "',NOW(),'" + msg + "activación de Empresa','" + edicion + "','" + empresa + "','" + area + "')");
                    MessageBox.Show("La Empresa se ha " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }
        }

        private void busqempresa_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetnempresa_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetnempresa_TextChanged(object sender, EventArgs e)
        {
            if (editbussines)
            {
                if (empresa == 1 && area == 1)
                {
                    lblsavemp.Visible = btnsavemp.Visible = (status == 1 && (!string.IsNullOrWhiteSpace(txtgetnempresa.Text) && nombreAnterior != v.mayusculas(txtgetnempresa.Text.ToLower()).Trim()));
                }
                else if ((empresa == 2 || empresa == 3) && area == 2)
                {
                    lblsavemp.Visible = btnsavemp.Visible = (status == 1 && !string.IsNullOrWhiteSpace(txtgetnempresa.Text) && (!nombreAnterior.Equals(v.mayusculas(txtgetnempresa.Text.ToLower()).Trim()) || !(imgserialAnt ?? "").Equals(imgserial)));
                }

            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
        string imgserial = null;
        private void pblogo_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Title = "Seleccione Logo Para Empresa";
                openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openFileDialog1.InitialDirectory = "Documents";
                openFileDialog1.FileName = null;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    imgserial = v.ImageToString(openFileDialog1.FileName);
                    pblogo.BackgroundImage = v.StringToImage2(imgserial);
                    lnkrestablecer.Visible = true;
                }
                else
                {
                    lnkrestablecer_LinkClicked(sender, (LinkLabelLinkClickedEventArgs)e);
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147024809)
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkrestablecer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            imgserial = "";
            pblogo.BackgroundImage = Properties.Resources.image;
            lnkrestablecer.Visible = false;
        }

        private void gbaddbussiness_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void pblogo_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (editbussines)
                btnsavemp.Visible = (status == 1 && (v.ImageToString(pblogo.BackgroundImage) != imgserialAnt));
        }
        private void catEmpresas_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
                busqempr();
        }

        private void busqempresa_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (busqempresa.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editbussines)
            {
                if (!string.IsNullOrWhiteSpace(txtgetnempresa.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (v.mayusculas(txtgetnempresa.Text.Trim().ToLower()) != nombreAnterior && !string.IsNullOrWhiteSpace(txtgetnempresa.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }
    }
}
