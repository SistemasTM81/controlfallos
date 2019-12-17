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
    public partial class CatIncidencias : Form
    {
        validaciones v;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }

        string _numeroAnterior, _conceptoAnterior;
        int _idAnterior = 0, idusuario = 0, empresa, area;
        int _estatus = 0;
        bool editar, mensaje = false;

        public CatIncidencias(int idusuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            this.idusuario = idusuario;
            this.empresa = empresa;
            this.area = area;
            InitializeComponent();
        }
        private void CatIncidencias_Load(object sender, EventArgs e)
        {
            privilegiosPuestos();
            lbltitle.Left = (this.Width - lbltitle.Width) / 2;
            Incidencias();
        }
        public void privilegiosPuestos()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idusuario, this.Name)).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
            }
            mostrar();
        }
        public void mostrar()
        {
            if (Pinsertar)
            {
                gbDatos.Visible = true;
                pGuardar.Visible = true;
                pNuevo.Visible = true;
            }
            if (Pconsultar)
            {
                DgvIncidencias.Visible = true;
            }
            if (Peditar)
            {
                label5.Visible = label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                pGuardar.Visible = false;
                editar = true;
            }
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtIncidencia.Text) || !string.IsNullOrWhiteSpace(txtConcepto.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (Convert.ToInt32(_numeroAnterior) != Convert.ToInt32(txtIncidencia.Text.Trim()) || _conceptoAnterior!=txtConcepto.Text.Trim())
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }
        public void limpiar()
        {
            txtIncidencia.Clear();
            txtConcepto.Clear();
            txtIncidencia.Focus();
            pNuevo.Visible = true;
            pGuardar.Visible = true;
            pDesactivar.Visible = false;
            _limpiarVariables();
            editar = nuevo = false;
            mensaje = false;
            _idAnterior = 0;
        }
        public void _limpiarVariables()
        {
            _idAnterior = _estatus = 0;
            _numeroAnterior = _conceptoAnterior = "";
        }
        public void Incidencias()
        {
            DgvIncidencias.Rows.Clear();
            MySqlCommand datos = new MySqlCommand("select t1.idincidencia as id,t1.numeroIncidencia as 'N° de Incidencia',t1.concepto as 'Concepto',upper(concat(t2.ApPaterno,' ',t2.ApMaterno,' ',t2.nombres)) as 'Persona',if(t1.status=1,'ACTIVO','NO ACTIVO') as estatus from catincidencias as t1 inner join cpersonal as t2 on t2.idPersona=t1.personaFKcpersonal order by t1.numeroIncidencia asc;", v.c.dbconection());
            MySqlDataReader dr = datos.ExecuteReader();
            while (dr.Read())
            {
                DgvIncidencias.Rows.Add(dr.GetString("id"), dr.GetString("N° de Incidencia"), dr.GetString("Concepto"), dr.GetString("Persona"), dr.GetString("estatus"));
            }
            dr.Close();
            v.c.dbconection().Close();
            DgvIncidencias.ClearSelection();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
        }
        void guardar()
        {
            if (string.IsNullOrWhiteSpace(txtIncidencia.Text.Trim()))
            {
                MessageBox.Show("El campo \"incidencia\" se encuentra vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtConcepto.Text.Trim()))
                {
                    MessageBox.Show("El campo \"Concepto\" se encuentra vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
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
            }
        }
        public void _editar()
        {
            try
            {
                if (!v.existe_incidencia(Convert.ToInt32(txtIncidencia.Text.Trim()), _idAnterior))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        MySqlCommand editar = new MySqlCommand("update catincidencias set numeroIncidencia='" + txtIncidencia.Text.Trim() + "',concepto='" + txtConcepto.Text.Trim() + "' where idincidencia='" + _idAnterior + "'", v.c.dbconection());
                        editar.ExecuteNonQuery();
                        MySqlCommand modificaciones = new MySqlCommand("insert into modificaciones_sistema (form,idregistro,ultimaModificacion,usuariofkcpersonal,fechaHora,Tipo,empresa,area,motivoActualizacion) values('Catálogo de Incidencias','" + _idAnterior + "','" + _numeroAnterior + ";" + _conceptoAnterior + "','" + this.idusuario + "',now(),'Actualización de Incidencia','" + empresa + "','" + area + "','" + observaciones + "')", v.c.dbconection());
                        modificaciones.ExecuteNonQuery();
                        Incidencia_de_Personal cat = (Incidencia_de_Personal)Owner;
                        cat.incidencias();
                        cat._bincidencias();
                        limpiar();
                        Incidencias();
                        if (!mensaje) MessageBox.Show("Incidencia editada correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                       v.c.dbcon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void insertar()
        {
            try
            {
                if (!v.existe_incidencia(Convert.ToInt32(txtIncidencia.Text.Trim()), _idAnterior))
                {
                    MySqlCommand insertar = new MySqlCommand("insert into catincidencias (numeroIncidencia,concepto,personafkcpersonal,status)values('" + Convert.ToInt32(txtIncidencia.Text.Trim()) + "','" + txtConcepto.Text.Trim() + "','" + this.idusuario + "','1')", v.c.dbconection());
                    insertar.ExecuteNonQuery();
                    MySqlCommand modificaciones = new MySqlCommand("insert into modificaciones_sistema (form,idregistro,usuariofkcpersonal,fechaHora,Tipo,empresa,area) values('Catálogo de Incidencias',(select idincidencia from catincidencias where numeroIncidencia='" + txtIncidencia.Text + "'),'" + this.idusuario + "',now(),'Incerción de Incidencia','" + empresa + "','" + area + "');", v.c.dbconection());
                    modificaciones.ExecuteNonQuery();
                    Incidencia_de_Personal cat = (Incidencia_de_Personal)Owner;
                    cat.incidencias();
                    cat._bincidencias();
                    v.c.dbcon.Close();
                    limpiar();
                    Incidencias();
                    MessageBox.Show("Incidencia insertada correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtIncidencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void txtConcepto_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }

        private void txtIncidencia_TextChanged(object sender, EventArgs e)
        {
            if (editar && Peditar)
            {
                if ((_numeroAnterior != txtIncidencia.Text.Trim() || _conceptoAnterior != txtConcepto.Text.Trim()) && (!string.IsNullOrEmpty(txtConcepto.Text.Trim()) && !string.IsNullOrWhiteSpace(txtIncidencia.Text.Trim())))
                {
                    pGuardar.Visible = true;
                }
                else
                {
                    pGuardar.Visible = false;
                }
            }
        }
        void detecta_mod(DataGridViewCellEventArgs ex, EventArgs e)
        {
            if (editar)
            {
                if ((_numeroAnterior != txtIncidencia.Text || _conceptoAnterior != txtConcepto.Text.Trim()) && (!string.IsNullOrWhiteSpace(txtIncidencia.Text) && !string.IsNullOrWhiteSpace(txtConcepto.Text.Trim())))
                {
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        guardar();
                    }
                    else
                    {
                        _restaurarDatos(ex);
                    }
                }
                else
                {
                    limpiar();
                    _limpiarVariables();
                    Incidencias();
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtIncidencia.Text) || !string.IsNullOrWhiteSpace(txtConcepto.Text.Trim()))
                {
                    if (MessageBox.Show("¿Desea guardar la información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        guardar();
                    }
                    else
                    {
                        limpiar();
                        _limpiarVariables();
                        Incidencias();
                    }
                }
            }
        }
        bool nuevo = false;
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            nuevo = true;
            detecta_mod(null, e);

        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            string msg;
            if (_estatus == 0)
            {
                msg = "Re";
                _estatus = 1;
            }
            else
            {
                msg = "Des";
                _estatus = 0;
            }
            observacionesEdicion obs = new observacionesEdicion(v);
            obs.Owner = this;
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Tipo de Licencia";
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                try
                {
                    var res = v.c.insertar("Update catincidencias set status='" + _estatus + "' where idincidencia='" + _idAnterior + "'");
                    var res2 = v.c.insertar("Insert into modificaciones_sistema (form,idregistro,usuariofkcpersonal,fechaHora,tipo,motivoActualizacion,empresa,area) values ('Catálogo de Incidencias','" + _idAnterior + "','" + this.idusuario + "',now(),'" + msg + "activación de Incidencia','" + edicion + "','" + this.empresa + "','" + this.area + "')");
                    Incidencia_de_Personal inci = (Incidencia_de_Personal)Owner;
                    inci.incidencias();
                    MessageBox.Show("La incidencia ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                    Incidencias();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtConcepto_Validating(object sender, CancelEventArgs e)
        {
            while (txtConcepto.Text.Contains("  ") || txtConcepto.Text.Contains("\n") || txtConcepto.Text.Contains("\r\n"))
            {
                txtConcepto.Text = txtConcepto.Text.Replace("  ", " ").Trim().Replace("\n", " ").Replace("\r\n", " ");
                txtConcepto.SelectionStart = txtConcepto.TextLength + 1;
            }
        }

        private void DgvIncidencias_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DgvIncidencias.Columns[e.ColumnIndex].Name == "estatus")
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
        public void _restaurarDatos(DataGridViewCellEventArgs e)
        {
            _idAnterior = Convert.ToInt32(DgvIncidencias.Rows[e.RowIndex].Cells[0].Value.ToString());
            _estatus = v.getStatusInt(DgvIncidencias.Rows[e.RowIndex].Cells[4].Value.ToString());
            if (Pdesactivar)
            {
                pDesactivar.Visible = true;
                if (_estatus == 0)
                {
                    btnDesactivar.BackgroundImage = controlFallos.Properties.Resources.up;
                    lblDesactivar.Text = "Reactivar";
                }
                else
                {
                    btnDesactivar.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                    lblDesactivar.Text = "Desactivar";
                }
            }
            if (Peditar)
            {
                editar = true;
                txtIncidencia.Text = _numeroAnterior = DgvIncidencias.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtConcepto.Text = _conceptoAnterior = DgvIncidencias.Rows[e.RowIndex].Cells[2].Value.ToString();
                DgvIncidencias.ClearSelection();
                btnGuardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
                if (Pinsertar) { pNuevo.Visible = true; }
                if (_estatus == 0) { MessageBox.Show("Para Modificar La Información Necesita Reactivar El Registro", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }
        private void DgvIncidencias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (_idAnterior > 0 && ((_numeroAnterior != txtIncidencia.Text.Trim()) || (_conceptoAnterior != txtConcepto.Text.Trim())))
                {
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        mensaje = true;
                        btnGuardar_Click(null, e);
                    }
                    else
                    {
                        _restaurarDatos(e);
                    }
                }
                else
                {
                    _restaurarDatos(e);
                }
            }
        }
    }
}
