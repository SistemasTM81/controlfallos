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
    public partial class catNiveles : Form
    {
        validaciones v;
        bool editar;
        string idnivelAnterior, idpasilloAnterior, NivelAnterior;
        int idUsuario, empresa, area;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        int _state;
        public catNiveles(int empresa, int area, int idUsuario, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = idUsuario;
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
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbaddnivel.Visible = true;
            }
            if (Pconsultar)
            {
                gbniveles.Visible = true;
            }
            if (Peditar)
            {
                label3.Visible = true;
                label23.Visible = true;
            }
        }
        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                }
                else
                {
                    Editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void Editar()
        {
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            string nivel = v.mayusculas(txtnivel.Text.Trim().ToLower());
            if (!v.existeNivelActualizar(pasillo, Convert.ToInt32(idpasilloAnterior), nivel, NivelAnterior))
            {
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                    if (v.c.insertar("UPDATE cniveles SET pasillofkcpasillos='" + pasillo + "', nivel='" + nivel + "' WHERE idnivel='" + idnivelAnterior + "'"))
                    {
                        var _nivel = 0; var _Nivel = 0;
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Niveles','" + idnivelAnterior + "','" + idpasilloAnterior + ";" + nivel + "','" + idUsuario + "',NOW(),'Actualización de Nivel','" + observaciones + "','" + empresa + "','" + area + "')");
                        if (!yaAparecioMensaje) MessageBox.Show("El Nivel Ha Sido Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ubicaciones ub = (ubicaciones)Owner;
                        var _pasillo = ub.cbpasillo.SelectedValue; if (ub.cbanaquel.SelectedIndex > 0) { _nivel = Convert.ToInt32(ub.cbniveles.SelectedValue); }
                        if (ub.cbanaquel.SelectedIndex > 0) { _Nivel = Convert.ToInt32(ub.cbanaquel.SelectedValue); }
                        limpiar();
                        ub.cbpasillo.SelectedValue = _pasillo;
                        if (_nivel > 0) { ub.cbniveles.SelectedValue = _nivel; } else { ub.cbniveles.DataSource = null; }
                        if (_Nivel > 0) { ub.cbanaquel.SelectedValue = _Nivel; } else { ub.cbanaquel.DataSource = null; }
                    }
                }
            }

        }
        private void cbpasillo_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        void cargarNiveles()
        {
            dtniveles.Rows.Clear();
            DataTable dt = (DataTable)v.getData("SELECT t1.idnivel,upper(t2.pasillo),upper(t1.nivel),(SELECT UPPER(CONCAT(coalesce(nombres,''),' ',coalesce(apPaterno,''),' ',coalesce(apMaterno,''))) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),if(t1.status=1,'ACTIVO',CONCAT('NO ACTIVO')),idpasillo FROM cniveles as t1 INNER JOIN cpasillos AS t2 ON t1.pasillofkcpasillos=t2.idpasillo and t1.empresa='" + empresa + "' ORDER BY t2.pasillo, t1.nivel ASC");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dtniveles.Rows.Add(dt.Rows[i].ItemArray);
            }
            dtniveles.ClearSelection();
        }
        object cb_pasillo, id_ant;
        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false; btnsavemp.BackgroundImage = controlFallos.Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                cbpasillo.Focus();
                btnsavemp.Visible = lblsavemp.Visible = true;
            }
            cb_pasillo = cbpasillo.SelectedValue;
            cbpasillo.SelectedIndex = 0;
            txtnivel.Clear();
            yaAparecioMensaje = false;
            if (Pconsultar) cargarNiveles();
            ubicaciones u = (ubicaciones)Owner;
            u.insertarUbicaciones();
            //u.busqUbic();
            id_ant = idnivelAnterior;
            pdelete.Visible = false;
            pCancelar.Visible = false;
            idnivelAnterior = null;
            NivelAnterior = null;
            idpasilloAnterior = null;
            u.iniCombos("SELECT idnivel, UPPER(nivel) AS nivel FROM cniveles WHERE status = '1' and pasillofkcpasillos = '" + u.cbpasillo.SelectedValue + "' and empresa='" + empresa + "' ORDER BY nivel ASC", u.cbniveles, "idnivel", "nivel", "--SELECCIONE NIVEL--");
            inipasillos();
        }

        private void dtniveles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtniveles.Columns[e.ColumnIndex].Name == "estatus")
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

        private void btndelpa_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                int status;
                if (this._state == 0)
                {
                    msg = "Re";
                    status = 1;
                }
                else
                {
                    msg = "Des";
                    status = 0;
                }
                if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t2.status FROM cniveles as t1 INNER JOIN cpasillos as t2 ON t1.pasillofkcpasillos=t2.idpasillo WHERE t1.idnivel='{0}'", idnivelAnterior))) == 0)
                {
                    MessageBox.Show("Error Al Reactivar El Nivel:\nPasillo Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Nivel";
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        var res = v.c.insertar("UPDATE cniveles as t2 LEFT JOIN canaqueles as t3 ON t3.nivelfkcniveles=t2.idnivel LEFT JOIN ccharolas as t4 ON t4.anaquelfkcanaqueles=t3.idanaquel SET t2.status='" + status + "', t3.status='" + status + "',t4.status='" + status + "' WHERE t2.idnivel='" + this.idnivelAnterior + "'");
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Ubicaciones - Niveles','" + idnivelAnterior + "','" + msg + "activación de nivel','" + idUsuario + "',NOW(),'" + msg + "activación de Nivel','" + edicion + "','" + empresa + "','" + area + "')");
                        MessageBox.Show("El Nivel se " + msg + "activó Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        msg = null;
                        status = 0;
                        limpiar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) v.mover(sender, e, this);
        }

        private void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                if (_state == 1 && (cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtnivel.Text.Trim())) && (!idpasilloAnterior.Equals(cbpasillo.SelectedValue.ToString()) || !NivelAnterior.Equals(txtnivel.Text.Trim()))) btnsavemp.Visible = lblsavemp.Visible = true; else btnsavemp.Visible = lblsavemp.Visible = false;
            }
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (_state == 1 && (cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtnivel.Text.Trim())) && (!idpasilloAnterior.Equals(cbpasillo.SelectedValue.ToString()) || !NivelAnterior.Equals(txtnivel.Text.Trim())))
            {
                if (MessageBox.Show("Desea Guardar La Información", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsavemp_Click(null, e);
                }
            }

            limpiar();
        }

        private void txtnivel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsavemp_Click(null, e);
            else
                v.letrasynumeros(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (cbpasillo.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtnivel.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if ((Convert.ToInt32(idpasilloAnterior) != (int)cbpasillo.SelectedValue || NivelAnterior != txtnivel.Text) && cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtnivel.Text))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void dtniveles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (!string.IsNullOrWhiteSpace(idnivelAnterior) && editar && _state == 1 && (cbpasillo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtnivel.Text.Trim())) && (!idpasilloAnterior.Equals(cbpasillo.SelectedValue.ToString()) || !NivelAnterior.Equals(txtnivel.Text.Trim())))
                {
                    if (MessageBox.Show("Desea Guardar La Información", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsavemp_Click(null, e);
                    }
                }
                guardarReporte(e);
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            limpiar();
            try
            {
                idnivelAnterior = dtniveles.Rows[e.RowIndex].Cells[0].Value.ToString();
                _state = v.getStatusInt(dtniveles.Rows[e.RowIndex].Cells[4].Value.ToString());
                if (Pdesactivar)
                {

                    if (_state == 0)
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
                    cbpasillo.SelectedValue = idpasilloAnterior = dtniveles.Rows[e.RowIndex].Cells[5].Value.ToString();
                    if (cbpasillo.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idpasillo,UPPER(pasillo) as pasillo FROM cpasillos WHERE (status=1 OR idpasillo='" + idpasilloAnterior + "') and empresa='" + empresa + "'", cbpasillo, "idpasillo", "pasillo", "--SELECCIONE UN PASILLO--");
                        cbpasillo.SelectedValue = idpasilloAnterior;
                    }
                    txtnivel.Text = NivelAnterior = dtniveles.Rows[e.RowIndex].Cells[2].Value.ToString();
                    btnsavemp.Visible = false;
                    lblsavemp.Visible = false;
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";
                    editar = true;
                    if (Pinsertar) pCancelar.Visible = true;
                    gbaddnivel.Text = "Actualizar Nivel";
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    if (_state == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                if (!Pdesactivar && !Peditar)
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar o Desactivar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            int pasillo = Convert.ToInt32(cbpasillo.SelectedValue);
            string nivel = v.mayusculas(txtnivel.Text.Trim().ToLower());
            if (v.formularioNiveles(pasillo, nivel) && !v.existeNivel(pasillo, nivel))
            {
                if (v.c.insertar("INSERT INTO cniveles(nivel, pasillofkcpasillos, usuariofkcpersonal,empresa) VALUES('" + nivel + "','" + pasillo + "','" + idUsuario + "','" + empresa + "')"))
                {
                    if (v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) VALUES ('Catálogo de Refacciones - Ubicaciones - Niveles',(SELECT idnivel FROM cniveles WHERE pasillofkcpasillos='" + pasillo + "' and nivel='" + nivel + "' and empresa='" + empresa + "'),'Inserción de Nivel','" + idUsuario + "',NOW(),'Inserción de Nivel','" + empresa + "','" + area + "')"))
                    {
                        var _Nivel = 0;
                        var _nivel = 0;
                        ubicaciones ub = (ubicaciones)Owner;
                        var _pasillo = ub.cbpasillo.SelectedValue; if (ub.cbniveles.SelectedIndex > 0) { _nivel = Convert.ToInt32(ub.cbniveles.SelectedValue); }
                        if (ub.cbanaquel.SelectedIndex > 0) { _Nivel = Convert.ToInt32(ub.cbanaquel.SelectedValue); }
                        MessageBox.Show("El Nivel se Ha Agregado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        ub.cbpasillo.SelectedValue = _pasillo;
                        if (_nivel > 0) { ub.cbniveles.SelectedValue = _nivel; } else { ub.cbniveles.DataSource = null; }
                        if (_Nivel > 0) { ub.cbanaquel.SelectedValue = _Nivel; } else { ub.cbanaquel.DataSource = null; }
                    }
                }
            }
        }

        private void catNiveles_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pinsertar || Peditar)
                inipasillos();
            if (Pconsultar) cargarNiveles();
        }
        void inipasillos()
        {
            v.iniCombos("SELECT idpasillo,UPPER(pasillo) as pasillo FROM cpasillos WHERE status=1 and empresa='" + empresa + "'", cbpasillo, "idpasillo", "pasillo", "--SELECCIONE UN PASILLO--");
        }
    }
}
