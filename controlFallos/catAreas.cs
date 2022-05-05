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
    public partial class catAreas : Form
    {
        validaciones v;
        int idUsuario, statusAnterior, empresa, area;
        bool editar = false, yaAparecioMensaje = false;
        string idareaAnterior, empresaanterior, identificadorAnterior, nombreAreaAnterior, cadenaEmpresa;
        public catAreas(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            cbempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbareas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 14, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True; d.BackColor = Color.FromArgb(200, 200, 200);
            tbareas.ColumnHeadersDefaultCellStyle = d;
            d.Font = new Font("Garamond", 12, FontStyle.Regular);
            tbareas.DefaultCellStyle = d;
            privilegios();
            if (pinsertar || peditar)
                iniempresas();
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
                if (privilegiosTemp.Length > 3)
                {
                    pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }
            }
            mostrar();
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (statusAnterior == 1 && ((cbempresa.SelectedIndex > 0 && empresaanterior != cbempresa.SelectedValue.ToString()) || (!string.IsNullOrWhiteSpace(txtid.Text) && identificadorAnterior != txtid.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtnombre.Text) && nombreAreaAnterior != v.mayusculas(txtnombre.Text.Trim().ToLower()))))
                    btnsavemp.Visible = lblsavemp.Visible = true;
                else
                    btnsavemp.Visible = lblsavemp.Visible = false;
            }
        }
        void mostrar()
        {
            if (pinsertar || peditar)
                gbaddarea.Visible = true;
            if (pconsultar)
                gbareas.Visible = true;
            if (peditar)
                label23.Visible = label3.Visible = true;
        }

        private void catAreas_Load(object sender, EventArgs e)
        {
            if (pconsultar)
                busqueda();
        }

        private void txtid_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumeros(e);
        }

        private void txtnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsavemp_Click(null, e);
            else
                v.paraUM(e);
        }
        void iniempresas() { v.iniCombos("SELECT idempresa, upper(nombreEmpresa) AS nombreEmpresa FROM cempresas WHERE status ='1' AND (empresa='" + empresa + "' AND area ='" + area + "') ORDER BY nombreEmpresa ASC", cbempresa, "idempresa", "nombreEmpresa", "--SELECCIONE UNA EMPRESA--"); }
        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    insertar();
                else
                    _editar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, v.sistema(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }


        void insertar()
        {
            string nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();
            //string rbtn = (Rbtn1.Checked) ? "1" : (Rbtn2.Checked) ? "2" : (Rbtn0.Checked) ? "0" : null;
            if (!v.formularioAreas(cbempresa.SelectedIndex, txtid.Text.Trim(), nombre) && !v.existeArea(cbempresa.SelectedValue.ToString(), txtid.Text.Trim(), nombre))
            {
                if (v.c.insertar("INSERT INTO careas (empresafkcempresas,identificador,nombreArea,usuariofkcpersonal) VALUES('" + cbempresa.SelectedValue + "','" + txtid.Text.Trim() + "','" + nombre + "','" + idUsuario + "')"))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Areas',(SELECT idarea FROM careas WHERE nombreArea='" + nombre + "' and empresafkcempresas='" + cbempresa.SelectedValue + "' AND identificador='" + txtid.Text + "'),'Inserción de Area','" + idUsuario + "',NOW(),'Inserción de Área','" + empresa + "','" + area + "')");
                    MessageBox.Show("El Área Se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    int id = Convert.ToInt32(v.getaData("Select idarea from careas as t1 inner join cempresas as t2 on t2.idempresa=t1.empresafkcempresas and nombreArea='" + nombre + "'"));
                    catUnidades cat = (catUnidades)this.Owner;
                    var _empresa = cat.csetEmpresa.SelectedValue ?? 0;
                    var _area = cat.cbareas.SelectedValue;
                    limpiar();
                    cat.iniareas();
                    if (Convert.ToInt32(_empresa) > 0) { cat.csetEmpresa.SelectedValue = _empresa; }
                    if (Convert.ToInt32(_area) > 0) { cat.cbareas.SelectedValue = _area; } else { if (Convert.ToInt32(_empresa) > 0) { cat.cbareas.SelectedValue = 0; } else { cat.cbareas.DataSource = null; } }
                }
            }
        }
        void _editar()
        {
            string id = txtid.Text.Trim();
            string nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();
            //string rbtn = (Rbtn1.Checked) ? "1" : (Rbtn2.Checked) ? "2" : (Rbtn0.Checked) ? "0" : null;
            if (id.Equals(identificadorAnterior) && nombre.Equals(nombreAreaAnterior))
            {
                if (MessageBox.Show("No Se Realizaron Cambios. \n ¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    limpiar();
            }
            else
            {
                if (!v.formularioAreas(cbempresa.SelectedIndex, id, nombre) && !v.existeAreaActualizar(cbempresa.SelectedValue.ToString(), id, this.identificadorAnterior, nombre, nombreAreaAnterior))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        if (v.c.insertar("UPDATE careas SET empresafkcempresas='" + cbempresa.SelectedValue + "',identificador ='" + id + "',nombreArea = '" + nombre + "' WHERE idarea='" + idareaAnterior + "'"))
                        {
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Areas','" + idareaAnterior + "','" + empresaanterior + ";" + identificadorAnterior + ";" + nombreAreaAnterior + "','" + idUsuario + "',NOW(),'Actualización de Área','" + observaciones + "','" + empresa + "','" + area + "')");
                            if (!yaAparecioMensaje)
                                MessageBox.Show("El Área Se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            catUnidades cat = (catUnidades)Owner;
                            var _empresa = cat.csetEmpresa.SelectedValue ?? 0;
                            var _area = cat.cbareas.SelectedValue;
                            limpiar();
                            cat.iniareas();
                            if (Convert.ToInt32(_empresa) > 0) { cat.csetEmpresa.SelectedValue = _empresa; }
                            if (Convert.ToInt32(_area) > 0) { cat.cbareas.SelectedValue = _area; } else { if (Convert.ToInt32(_empresa) > 0) { cat.cbareas.SelectedValue = 0; } else { cat.cbareas.DataSource = null; } }
                        }
                    }
                }
            }

        }
        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            string id = txtid.Text.Trim();
            string nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();
            if (cbempresa.SelectedValue.Equals(empresaanterior) || !id.Equals(identificadorAnterior) || !nombre.Equals(nombreAreaAnterior) && statusAnterior == 1)
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
                }
                else
                    limpiar();
            }
            else
                limpiar();
        }

        private void cbempresa_DrawItem(object sender, DrawItemEventArgs e) { v.combos_DrawItem(sender, e); }
        private void tbareas_ColumnAdded(object sender, DataGridViewColumnEventArgs e) { v.paraDataGridViews_ColumnAdded(sender, e); }
        private void btndelpa_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(idareaAnterior))
            {
                try
                {
                    string msg;
                    int status;
                    if (this.statusAnterior == 0)
                    {
                        msg = "Re";
                        status = 1;
                    }
                    else
                    {
                        msg = "Des";
                        status = 0;
                    }
                    if (this.statusAnterior == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cempresas WHERE idempresa=(SELECT empresafkcempresas FROM careas WHERE idarea='" + idareaAnterior + "')")) == 0)
                        MessageBox.Show("Error Al Reactivar:\nEmpresa Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {

                        observacionesEdicion obs = new observacionesEdicion(v);
                        obs.Owner = this;
                        obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación del Área";
                        if (obs.ShowDialog() == DialogResult.OK)
                        {
                            string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                            v.c.insertar(string.Format("UPDATE careas as t1 LEFT JOIN cservicios as t2 ON t2.areafkcareas=t1.idarea SET t1.status={0}, t2.status={0} WHERE idarea={1}", status, idareaAnterior));
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Areas','" + idareaAnterior + "','" + msg + "activación de Area','" + idUsuario + "',NOW(),'" + msg + "activación de Area','" + edicion + "','" + empresa + "','" + area + "')");
                            MessageBox.Show("El Área se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                        }

                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void gbaddarea_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        private void cbempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbempresa.SelectedIndex > 0)
                    empresas_index();
                else
                    busqueda();
            }
        }
        private void txtid_Validating(object sender, CancelEventArgs e) { v.espaciosenblanco(sender, e); }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbempresa.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtid.Text) || !string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                }
                else
                    this.Close();
            }
            else
            {
                if ((Convert.ToInt32(empresaanterior) != (int)cbempresa.SelectedValue || identificadorAnterior != txtid.Text.Trim() || nombreAreaAnterior != v.mayusculas(txtnombre.Text.Trim().ToLower()))&& cbempresa.SelectedIndex>0 && !string.IsNullOrWhiteSpace(txtid.Text)&& !string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                }
                else
                    this.Close();
            }
        }

        private void gbaddarea_Enter(object sender, EventArgs e){}

        private void panel1_MouseDown(object sender, MouseEventArgs e) { v.mover(sender, e, this); }
        public void empresas_index()
        {
            tbareas.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT t1.idarea,upper(t2.nombreEmpresa),UPPER(t1.identificador),UPPER(t1.nombreArea),UPPER(CONCAT(coalesce(t3.nombres,''),' ',coalesce(t3.apPaterno,''),' ',coalesce(t3.apMaterno,''))) as nombres, UPPER(if(t1.status=1,'Activo','Inactivo')),t1.empresafkcempresas FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas = t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idPersona where t2.idempresa='" + cbempresa.SelectedValue + "'");
            int numFilas = dt.Rows.Count;
            for (int i = 0; i < numFilas; i++)
                tbareas.Rows.Add(dt.Rows[i].ItemArray);
            tbareas.ClearSelection();
        }

        void busqueda()
        {
            tbareas.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT t1.idarea,upper(t2.nombreEmpresa),UPPER(t1.identificador),UPPER(t1.nombreArea),UPPER(CONCAT(coalesce(t3.nombres,''),' ',coalesce(t3.apPaterno,''),' ',coalesce(t3.apMaterno,''))) as nombres, UPPER(if(t1.status=1,'Activo','Inactivo')),t1.empresafkcempresas FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas = t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idPersona");
            int numFilas = dt.Rows.Count;
            for (int i = 0; i < numFilas; i++)
                tbareas.Rows.Add(dt.Rows[i].ItemArray);
            tbareas.ClearSelection();
        }

        private void tbareas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = txtid.Text.Trim();
                string nombre = v.mayusculas(txtnombre.Text.ToLower()).Trim();
                if (!string.IsNullOrWhiteSpace(idareaAnterior) && peditar && (cbempresa.SelectedValue.Equals(empresaanterior) || !id.Equals(identificadorAnterior) || !nombre.Equals(nombreAreaAnterior)) && statusAnterior == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null, e);
                    }
                    else
                        guardarReporte(e);
                }
                else
                    guardarReporte(e);
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                idareaAnterior = tbareas.Rows[e.RowIndex].Cells[0].Value.ToString();
                statusAnterior = v.getStatusInt(tbareas.Rows[e.RowIndex].Cells[5].Value.ToString());
                
                if (pdesactivar)
                {
                    if (statusAnterior == 0)
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
                if (peditar)
                {
                    editar = true;
                    cbempresa.SelectedValue = empresaanterior = tbareas.Rows[e.RowIndex].Cells[6].Value.ToString();
                    //string dato = v.getaData("SELECT MantTsdTri FROM careas where empresafkcempresas='" + empresaanterior + "'").ToString();
                    //Rbtn0.Checked = dato == "0" ? true : false;
                    //Rbtn1.Checked = dato == "1" ? true : false;
                    //Rbtn2.Checked = dato == "2" ? true : false;

                    txtid.Text = identificadorAnterior = tbareas.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtnombre.Text = nombreAreaAnterior = v.mayusculas(tbareas.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    if (cbempresa.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idempresa, upper(nombreEmpresa) AS nombreEmpresa FROM cempresas WHERE (status ='1' OR idempresa='" + empresaanterior + "') ORDER BY nombreEmpresa ASC", cbempresa, "idempresa", "nombreEmpresa", "--SELECCIONE UNA EMPRESA--");
                        cbempresa.SelectedValue = empresaanterior;
                    }
                    if (pinsertar) pCancelar.Visible = true;
                    gbaddarea.Text = "Actualizar Área";
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    empresas_index();
                    if (statusAnterior == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar en Este Formulario", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void tbareas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbareas.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }
        void limpiar()
        {
            if (pinsertar)
            {
                gbaddarea.Text = "Agregar Area";
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
                cbempresa.Focus();
            }
            if (pinsertar || peditar)
                cbempresa.SelectedIndex = 0;
            txtid.Clear();
            txtnombre.Clear();
            pCancelar.Visible = false;
            idareaAnterior = null;
            nombreAreaAnterior = null;
            statusAnterior = 0;
            pdelete.Visible = false; btnsavemp.Visible = lblsavemp.Visible = true; yaAparecioMensaje = false;
            if (pconsultar)
                busqueda();
            catUnidades cat = (catUnidades)Owner;
            cat.busqempresas();
            cat.bunidades();
            iniempresas();
        }
    }
}
