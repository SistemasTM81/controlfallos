using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using h = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catfallosGrales : Form
    {
        validaciones v;
        string codfalloAnterior;
        string nombrefalloAnterior;
        public string idclasfallo;
        public string iddescfallo;
        int status, empresa, area, categoriaAnterior;
        public bool editar, yaAparecioMensaje;
        string idnombrefalloTemp;
        int idUsuario;
        public Form MenuPrincipal;
        Thread th;
        public catfallosGrales(int idUsuario, int empresa, int area, Form MenuPrincipal,Image logo,validaciones v)
        {
            this.v = v;
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.idUsuario = idUsuario;
            cbclasificacion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbdescripcion.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbcategoria.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbClasificacionb.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            tbfallos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbDescFallob.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbcategoriasb.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbnombrefb.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.empresa = empresa;
            this.area = area;
            this.MenuPrincipal = MenuPrincipal;
            cbcategoria.DrawItem += v.combos_DrawItem;
            cbcategoriasb.DrawItem += v.combos_DrawItem;
            pictureBox1.BackgroundImage = logo;
        }
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                int descripcion = 0, categoria = 0;
                if (cbdescripcion.DataSource != null)
                {
                    descripcion = Convert.ToInt32(cbdescripcion.SelectedValue);
                    if (cbdescripcion.DataSource != null)
                        categoria = Convert.ToInt32(cbcategoria.SelectedValue);
                }
                    btnsavemp.Visible = lblsavemp.Visible = (status == 1 && (categoria > 0 && !string.IsNullOrWhiteSpace(txtgetdescfallo.Text.Trim())) && (categoriaAnterior != categoria || !nombrefalloAnterior.Equals(v.mayusculas(txtgetdescfallo.Text.ToLower()).Trim())));
            }
        }
        public void privilegios()
        { string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario,Name)).ToString().Split('/');
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
        void mostrar()
        {
            if (pinsertar || peditar)
            {
                gbaddnomfallo.Visible = true;
            }
            if (pconsultar)
            {
                gbconsulta.Visible = true;
            }
            if (peditar)
            {
                label9.Visible = true;
                label23.Visible = true;
            }
            if (peditar && !pinsertar)
            {
                editar = true;
                btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsavemp.Text = "Editar Nombre";
            }
        }
        public void iniNombres()
        {

            try
            {
                tbfallos.Rows.Clear();
                DataTable dt = (DataTable)v.getData("SELECT t1.idfalloEsp,UPPER(t3.nombreFalloGral),UPPER(t2.descfallo),UPPER(t5.categoria),UPPER(t1.codfallo) AS codfallo,UPPER(t1.falloesp) AS falloesp,UPPER(CONCAT(coalesce(t4.nombres,''),' ',coalesce(t4.ApPaterno,''),' ',coalesce(t4.ApMaterno,''))) AS nombre,if(t1.status=1,'ACTIVO','NO ACTIVO'),t3.idFalloGral,t2.iddescfallo,t5.idcategoria FROM cfallosesp AS t1 INNER JOIN catcategorias AS t5 ON t1.descfallofkcdescfallo = t5.idcategoria INNER JOIN cdescfallo AS t2 ON t5.subgrupofkcdescfallo = t2.iddescfallo INNER JOIN cfallosgrales AS t3 ON t2.falloGralfkcfallosgrales = t3.idFalloGral INNER JOIN cpersonal AS t4 ON t1.usuariofkcpersonal = t4.idpersona WHERE t1.empresa='" + empresa+"' ORDER BY SUBSTRING(codfallo,LENGTH(codFALLO) - 3,4)");
              foreach(DataRow row in dt.Rows)
                    tbfallos.Rows.Add(row.ItemArray);
                dt.Dispose();
                dt.EndInit();
                tbfallos.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void iniClasificacionesFallos()
        {
            object res = null, res2 = null,res3=null;
            if (cbclasificacion.DataSource != null)
            {
                if (cbclasificacion.SelectedIndex > 0)
                {
                    res = cbclasificacion.SelectedValue;
                    if (cbdescripcion.DataSource!=null) {
                        if (cbdescripcion.SelectedIndex > 0)
                            res2 = cbdescripcion.SelectedValue;
                        if (cbcategoria.DataSource!=null)
                        {
                            if (cbcategoria.SelectedIndex > 0)
                                res3 = cbcategoria.SelectedValue;
                        }
                    }
                }
            }
            v.iniCombos("SELECT idFalloGral id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales WHERE status = 1 AND empresa='"+empresa+"'", cbclasificacion, "id", "nombre", "-- SELECCIONE UN grupo --");
            if (res != null) cbclasificacion.SelectedValue = res;
            if (res2 != null) cbdescripcion.SelectedValue = res2;
            if (res3 != null) cbcategoria.SelectedValue = res3;
        }
        public void busqueda_clasificacion()
        {
            v.iniCombos("SELECT idFalloGral id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales WHERE empresa ='"+empresa+"'",cbClasificacionb,"id","nombre","-- SELECCIONE GRUPO --");
        }

        public void iniDescripcionesFallos()
        {
            String sql = "SELECT iddescfallo id,UPPER(descfallo) as nombre FROM cdescfallo WHERE status = 1 and falloGralfkcfallosgrales='" + cbclasificacion.SelectedValue + "' AND empresa='"+empresa+"'";
            if (cbclasificacion.SelectedIndex > 0)
            {
                if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cdescfallo WHERE status = 1 and falloGralfkcfallosgrales='" + cbclasificacion.SelectedValue + "'")) == 0)
                {
                    cbdescripcion.DataSource = null;
                    cbdescripcion.Enabled = false;
                }
                else
                {
                    v.iniCombos(sql, cbdescripcion, "id", "nombre", "-- SELECCIONE UN SUBGRUPO --");
                    cbdescripcion.Enabled = true;
                }
            }
            else
            {
                cbdescripcion.DataSource = null;
                cbdescripcion.Enabled = false;
            }


        }
        private void txtgetdescfallo_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtgetdescfallo.Text))
            {
                lblcodfallo.Text = null;
                v.folio = "";
            }
            else
            {
                if (v.folio == "")
                    v.setFolio();

                lblcodfallo.Text = v.codFalla(cbclasificacion.Text + " " + cbdescripcion.Text + " " + cbcategoria.Text + " " + txtgetdescfallo.Text);
            }

            getCambios(sender, e);
        }

        private void catNombresFallos_Load(object sender, EventArgs e)
        {
            privilegios();
            cbDescFallob.Enabled = false;
            cbnombrefb.Enabled = false;
            if (pconsultar)

                iniNombres();
            if (pinsertar || peditar)
            {
                iniClasificacionesFallos();
                busqueda_clasificacion();
            }
            if (pconsultar && !pinsertar && !peditar) { busqueda_clasificacion(); }
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(SplashScreen))
                {
                    if (frm.InvokeRequired)
                    {

                        validaciones.delgado dm = new validaciones.delgado(v.cerrarForm);

                        Invoke(dm, frm);
                    }

                    break;
                }
            }
            th.Abort();
        }




        string clasificacionAnterior = "0";

        private void cbclasificacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbclasificacion.DataSource != null)
            {
                if (cbclasificacion.SelectedIndex > -1)
                {
                    if (!cbclasificacion.SelectedValue.ToString().Equals(clasificacionAnterior))
                    {
                        if (cbclasificacion.SelectedIndex == 0)
                        {
                            txtgetdescfallo.Clear();
                            cbdescripcion.DataSource = null;
                            cbdescripcion.Enabled = false;
                        }
                        else
                        {
                            if (!editar && v.folio != "")
                            {
                                lblcodfallo.Text = v.codFalla(cbclasificacion.Text + " " + cbdescripcion.Text + " " + cbcategoria.Text + " " + txtgetdescfallo.Text);

                            }
                            if (txtgetdescfallo.Text == "")
                            {
                                lblcodfallo.Text = null;
                                v.folio = "";
                            }
                            if (cbclasificacion.SelectedIndex > 0)
                                iniDescripcionesFallos();
                            else
                            {
                                cbdescripcion.DataSource = null;
                                cbdescripcion.Enabled = false;
                            }
                        }
                    }
                    clasificacionAnterior = cbclasificacion.SelectedValue.ToString();

                    if (cbclasificacion.SelectedIndex > 0)
                        lnkLista.Visible = true;
                    else
                        lnkLista.Visible = false;
                }
            }
        }

        private void cbdescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbdescripcion.SelectedIndex > 0)
            {
                v.iniCombos("SELECT idcategoria as id , UPPER(categoria) as categoria FROM catcategorias WHERE subgrupofkcdescfallo='" + cbdescripcion.SelectedValue + "' AND STATUS=1 AND empresa='"+empresa+"'", cbcategoria, "id", "categoria", "-- SELECCIONE UNA CATEGORIA--");
                cbcategoria.Enabled = true;
            }
            else
            {
                cbcategoria.DataSource = null;
                cbcategoria.Enabled = false;
            }
        }

        private void txtgetdescfallo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbclasificacion.SelectedIndex > 0)
            {
                if (cbdescripcion.SelectedIndex > 0)
                {
                    if (cbcategoria.SelectedIndex > 0)
                    {
                        if (e.KeyChar == 13) btnsavemp_Click(null, e);
                        else
                        {
                            v.paraNombreFallo(e);
                            if (txtgetdescfallo.Text.Equals(null))
                            {
                                lblcodfallo.Text = null;
                                v.folio = "";
                            }
                        }
                    }
                    else
                    {
                        e.Handled = true;
                        MessageBox.Show("Seleccione una Categoría Para Continuar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtgetdescfallo.Clear();
                    }
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("Seleccione un Subgrupo Para Continuar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtgetdescfallo.Clear();
                }
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Seleccione un Grupo Para Continuar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtgetdescfallo.Clear();
            }
        }

        private void btnsavemp_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    insertar();
                else
                    editarN();
                iniNombres();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.HelpLink, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void insertar()
        {
            if (!v.formularioNombreFallos(cbclasificacion.SelectedIndex, cbdescripcion.SelectedIndex, cbcategoria.SelectedIndex, txtgetdescfallo.Text.ToLower()) && !v.yaExisteFalloEsp(cbcategoria.SelectedValue.ToString(), txtgetdescfallo.Text.ToLower()))
            {
                string iddescfallo = cbcategoria.SelectedValue.ToString();
                string nomfallo = txtgetdescfallo.Text.ToLower();
                string codfallo = lblcodfallo.Text;


                var res = v.c.insertar("INSERT INTO cfallosesp (descfallofkcdescfallo, codfallo,falloesp,usuariofkcpersonal,empresa) VALUES('" + iddescfallo + "','" + codfallo + "','" + v.mayusculas(nomfallo) + "','" + this.idUsuario + "','"+empresa+"')");
                var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Fallos - Nombres de Fallos',(SELECT idfalloesp From cfallosesp Where codfallo='" + codfallo + "' AND empresa='"+empresa+"'),'Inserción de Fallo','" + idUsuario + "',NOW(),'Inserción de Nombre de Fallo','" + empresa + "','" + area + "')");
                MessageBox.Show("Se ha Insertado el Nombre del Fallo Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                restablecer();
                pActualizar.Visible = false;
            }
            else
                txtgetdescfallo.Clear();
        }
        void editarN()
        {
            if (!string.IsNullOrWhiteSpace(idnombrefalloTemp))
            {
                string iddescfallo = cbcategoria.SelectedValue.ToString();
                string nomfallo = txtgetdescfallo.Text.ToLower();
                string codfallo = lblcodfallo.Text;
                if (!v.formularioNombreFallos(cbclasificacion.SelectedIndex, cbdescripcion.SelectedIndex, cbcategoria.SelectedIndex, nomfallo))
                {
                    if (this.iddescfallo.Equals(iddescfallo) && this.nombrefalloAnterior.Equals(v.mayusculas(nomfallo)) && codfalloAnterior.Equals(codfallo))
                    {
                        MessageBox.Show("No se Realizaron Cambios", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            restablecer();

                    }
                    else
                    {
                        if (status == 1)
                        {
                            if (!v.existenomfalloActualizar(iddescfallo, this.iddescfallo, nomfallo, nombrefalloAnterior))
                            {
                                observacionesEdicion obs = new observacionesEdicion(v);
                                obs.Owner = this;
                                if (obs.ShowDialog() == DialogResult.OK)
                                {
                                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                    v.c.insertar("UPDATE cfallosesp SET descfallofkcdescfallo = '" + iddescfallo + "', codfallo = LTRIM(RTRIM('" + codfallo + "')), falloesp = LTRIM(RTRIM('" + v.mayusculas(nomfallo) + "')) WHERE idfalloEsp='" + this.idnombrefalloTemp + "'");
                                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Fallos - Nombres de Fallos','" + idnombrefalloTemp + "','" + iddescfallo + ";" + codfalloAnterior + ";" + nombrefalloAnterior + "','" + idUsuario + "',NOW(),'Actualización de Nombre de Fallo','" + observaciones + "','" + empresa + "','" + area + "')");
                                    if (!yaAparecioMensaje)
                                        MessageBox.Show("Se ha Actualizado el Nombre del Fallo Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    restablecer();
                                    pActualizar.Visible = false;
                                    iniCombos("SELECT idfalloEsp id,UPPER(falloesp) as fallo FROM cfallosesp  ORDER BY falloesp ASC", cbnombrefb, "id", "fallo", "-SELECIONE UN NOMBRE-");
                                }
                            }
                        }
                        else
                            MessageBox.Show("No se Puede Modificar un Fallo Inactivo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un Nombre de Fallo Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void restablecer()
        {

            if (pinsertar)
            {
                btnsavemp.BackgroundImage = Properties.Resources.save;
                lblsavemp.Text = "Guardar";
                editar = false;
                gbaddnomfallo.Text = "Agregar";
                txtgetdescfallo.Focus();
            }
            btnsavemp.Visible = lblsavemp.Visible = true;
            idnombrefalloTemp = null;
            nombrefalloAnterior = null;
            txtgetdescfallo.Clear();
            pCancelar.Visible = false;
            status = 0;
            yaAparecioMensaje = false;
            btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
            lbldeletedesc.Text = "Desactivar";
            pEliminarClasificacion.Visible = false;
            txtgetdescfallo.Clear();
            iniClasificacionesFallos();

        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            int descripcion = 0; if (cbdescripcion.DataSource != null) descripcion = Convert.ToInt32(cbdescripcion.SelectedValue);
            if (status == 1 && (cbclasificacion.SelectedIndex > 0 && descripcion > 0 && !string.IsNullOrWhiteSpace(txtgetdescfallo.Text.Trim())) && (!idclasfallo.Equals(cbclasificacion.SelectedValue.ToString()) || !iddescfallo.Equals(cbdescripcion.SelectedValue.ToString()) || !nombrefalloAnterior.Equals(v.mayusculas(txtgetdescfallo.Text.ToLower()).Trim())))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    editarN();
                }
                else
                    restablecer();
            }
            else
                restablecer();
        }

        private void tbfallos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbfallos.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }

        private void tbfallos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int descripcion = 0; if (cbdescripcion.DataSource != null) descripcion = Convert.ToInt32(cbdescripcion.SelectedValue);
                if (!string.IsNullOrWhiteSpace(this.idnombrefalloTemp) && status == 1 && (cbclasificacion.SelectedIndex > 0 && descripcion > 0 && !string.IsNullOrWhiteSpace(txtgetdescfallo.Text.Trim())) && (!idclasfallo.Equals(cbclasificacion.SelectedValue.ToString()) || !iddescfallo.Equals(cbdescripcion.SelectedValue.ToString()) || !nombrefalloAnterior.Equals(v.mayusculas(txtgetdescfallo.Text.ToLower()).Trim())))

                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        editarN();
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
            restablecer();
            try
            {
                editar = true;
                this.idnombrefalloTemp = tbfallos.Rows[e.RowIndex].Cells[0].Value.ToString();
                status = v.getStatusInt(tbfallos.Rows[e.RowIndex].Cells[7].Value.ToString());
                if (pdesactivar)
                {
                    if (status == 0)
                    {
                        btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldeletedesc.Text = "Reactivar";
                    }
                    else
                    {
                        btndeletedesc.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldeletedesc.Text = "Desactivar";
                    }
                    pEliminarClasificacion.Visible = true;
                }
                if (peditar)
                {
                    this.codfalloAnterior = tbfallos.Rows[e.RowIndex].Cells[4].Value.ToString();
                    this.nombrefalloAnterior = v.mayusculas(tbfallos.Rows[e.RowIndex].Cells[5].Value.ToString().ToLower());
                    cbclasificacion.SelectedValue = idclasfallo = tbfallos.Rows[e.RowIndex].Cells[8].Value.ToString();
                    cbdescripcion.SelectedValue = iddescfallo = tbfallos.Rows[e.RowIndex].Cells[9].Value.ToString();
                    if (cbclasificacion.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idFalloGral id,UPPER(NombreFalloGral) as nombre FROM cfallosgrales WHERE (status = 1 OR idFalloGral = '" + idclasfallo + "')ORDER BY NombreFalloGral ASC", cbclasificacion, "id", "nombre", "-- SELECCIONE UNA CLASIFICACIÓN --");
                        cbclasificacion.SelectedValue = idclasfallo;
                    }
                    if (cbdescripcion.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT iddescfallo id,UPPER(descfallo) as nombre FROM cdescfallo WHERE (status = 1 OR iddescfallo='" + iddescfallo + "') and falloGralfkcfallosgrales='" + cbclasificacion.SelectedValue + "' ORDER BY descfallo ASC", cbdescripcion, "id", "nombre", "-- SELECCIONE UNA DESCRIPCIÓN --");
                        cbdescripcion.Enabled = true;
                        cbdescripcion.SelectedValue = iddescfallo;
                    }
                    cbcategoria.SelectedValue = categoriaAnterior = Convert.ToInt32(tbfallos.Rows[e.RowIndex].Cells[10].Value);
                    if (cbcategoria.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idcategoria as id , UPPER(categoria) as categoria FROM catcategorias WHERE subgrupofkcdescfallo='" + cbdescripcion.SelectedValue + "' AND (STATUS=1 or idcategoria='" + categoriaAnterior + "')", cbcategoria, "id", "categoria", "-- SELECCIONE UNA CATEGORIA--");
                        cbcategoria.Enabled = true;
                        cbcategoria.SelectedValue = categoriaAnterior;
                    }
                    txtgetdescfallo.Text = nombrefalloAnterior;
                    v.setFolio(codfalloAnterior);
                    lblcodfallo.Text = codfalloAnterior;
                    btnsavemp.Visible = lblsavemp.Visible = false;
                    if (pinsertar) pCancelar.Visible = true;
                    btnsavemp.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsavemp.Text = "Guardar";

                    gbaddnomfallo.Text = "Actualizar Nombre de Fallo: " + nombrefalloAnterior;
                    txtgetdescfallo.Focus();
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            catClasificaciones catC = new catClasificaciones(this.idUsuario, empresa, area,v);
            catC.Owner = this;
            catC.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            catDescFallos catC = new catDescFallos(this.idUsuario, empresa, area,v);
            catC.Owner = this;
            catC.ShowDialog();
        }

        private void btndeletedesc_Click(object sender, EventArgs e)
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
                if (this.status == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cfallosgrales WHERE idfallogral=(SELECT falloGralfkcfallosgrales FROM cdescfallo WHERE iddescfallo=(SELECT descfallofkcdescfallo FROM cfallosesp WHERE idfalloEsp='" + idnombrefalloTemp + "'))")) == 0)
                {
                    MessageBox.Show("Error Al Reactivar El Nombre de Fallo:\nClasificación de Fallo Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (this.status == 0 && Convert.ToInt32(v.getaData("SELECT status FROM cdescfallo WHERE iddescfallo=(SELECT descfallofkcdescfallo FROM cfallosEsp WHERE idFalloEsp='" + idnombrefalloTemp + "')")) == 0)
                {

                    MessageBox.Show("Error Al Reactivar El Nombre de Fallo:\n Descripción de Fallo Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Nombre De Fallo";
                    obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 30, obs.lblinfo.Location.Y);
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        v.c.insertar("UPDATE cfallosesp SET status = '" + status + "' WHERE idfalloEsp=" + this.idnombrefalloTemp);
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Fallos - Nombres de Fallos','" + idnombrefalloTemp + "','" + msg + "activación de Fallo','" + idUsuario + "',NOW(),'" + msg + "activación de Nombre de Fallo','" + edicion + "','" + empresa + "','" + area + "')");
                        restablecer();
                        iniNombres();
                        MessageBox.Show("El Nombre de Fallo se ha " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtgetecoBusq_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }
        bool est_expor = false;
        private void button3_Click(object sender, EventArgs e)
        {

            if (cbClasificacionb.SelectedIndex > 0 || cbDescFallob.SelectedIndex > 0 || cbnombrefb.SelectedIndex > 0 || !string.IsNullOrWhiteSpace(txtgetcodbusq.Text.Trim()))
            {
                try
                {
                    string codfallo = txtgetcodbusq.Text;

                    tbfallos.Rows.Clear();
                    string sql = "SELECT t1.idfalloEsp,UPPER(t3.nombreFalloGral),UPPER(t2.descfallo),UPPER(t5.categoria),UPPER(t1.codfallo) AS codfallo,UPPER(t1.falloesp) AS falloesp,UPPER(CONCAT(coalesce(t4.nombres,''),' ',coalesce(t4.ApPaterno,''),' ',coalesce(t4.ApMaterno,''))) AS nombre,if(t1.status=1,'ACTIVO','NO ACTIVO'),t3.idFalloGral,t2.iddescfallo,t5.idcategoria FROM cfallosesp AS t1 INNER JOIN catcategorias AS t5 ON t1.descfallofkcdescfallo = t5.idcategoria INNER JOIN cdescfallo AS t2 ON t5.subgrupofkcdescfallo = t2.iddescfallo INNER JOIN cfallosgrales AS t3 ON t2.falloGralfkcfallosgrales = t3.idFalloGral INNER JOIN cpersonal AS t4 ON t1.usuariofkcpersonal = t4.idpersona ";
                    string wheres = "";
                    if (cbClasificacionb.SelectedIndex > 0)
                    {
                        if (wheres == "")
                            wheres = " WHERE T3.idFalloGral='" + cbClasificacionb.SelectedValue + "'";
                        else

                            wheres += " AND t3.idFalloGral='" + cbClasificacionb.SelectedValue + "'";

                        if (cbDescFallob.SelectedIndex > 0)
                        {
                            if (wheres == "")
                                wheres = " WHERE t2.iddescfallo='" + cbDescFallob.SelectedValue + "'";
                            else
                                wheres += " AND t2.iddescfallo='" + cbDescFallob.SelectedValue + "'";
                            if (cbcategoriasb.SelectedIndex > 0)
                            {

                                if (wheres == "")
                                    wheres = " WHERE t5.idcategoria='" + cbcategoriasb.SelectedValue + "'";
                                else
                                    wheres += " AND t5.idcategoria='" + cbcategoriasb.SelectedValue + "'";

                                if (cbnombrefb.SelectedIndex > 0)
                                {
                                    if (wheres == "")
                                        wheres = " Where t1.idfalloEsp='" + cbnombrefb.SelectedValue + "'";
                                    else
                                        wheres += " ANd t1.idfalloEsp='" + cbnombrefb.SelectedValue + "' ";
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(codfallo))
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE t1.codfallo LIKE '" + codfallo + "%' ";
                        }
                        else
                        {
                            wheres += "AND t1.codfallo LIKE '" + codfallo + "%' ";
                        }
                    }
                    sql += wheres += "ORDER BY SUBSTRING(codfallo,LENGTh(codFALLO)-3,4)";
                    txtgetcodbusq.Clear();
                    cbClasificacionb.SelectedIndex = 0;
                    DataTable dt = (DataTable)v.getData(sql);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se Encontraron Resultados", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        iniNombres();
                    }
                    else
                    {
                        tbfallos.Rows.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                            tbfallos.Rows.Add(dt.Rows[i].ItemArray);
                        tbfallos.ClearSelection();
                        if (!est_expor)
                            btnExcel.Visible = true;
                        pActualizar.Visible = true;
                        LblExcel.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Seleccione Un Criterio De Busqueda", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cbclasificacion_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        private void tbfallos_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }
        private void txtgetcodbusq_TextChanged(object sender, EventArgs e)
        {

        }
        private void gbaddnomfallo_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        delegate void El_Delegado();
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "Exportando";
        }
        delegate void El_Delegado1();
        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Visible = true;
            est_expor = false;
            LblExcel.Text = "Exportar";
        }
        void ExportarExcel()
        {
            if (tbfallos.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < tbfallos.Columns.Count; i++) if (tbfallos.Columns[i].Visible) dt.Columns.Add(tbfallos.Columns[i].HeaderText);
                for (int j = tbfallos.Rows.Count - 1; j >= 0; j--)
                {

                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < tbfallos.Columns.Count; i++)
                    {

                        if (tbfallos.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = tbfallos.Rows[j].Cells[i].Value;
                            indice++;
                        }

                    }
                    dt.Rows.Add(row);
                }

                dt.Columns[4].SetOrdinal(5);

                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando);
                    this.Invoke(delega);
                }

                v.exportaExcel(dt);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando1);
                    this.Invoke(delega);
                }
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        Thread exportar;
        private void button5_Click(object sender, EventArgs e)
        {
            est_expor = true;
            ThreadStart delegado = new ThreadStart(ExportarExcel);
            exportar = new Thread(delegado);
            exportar.Start();
        
          
        }
        public void iniCombos(string sql, ComboBox cbx, string ValueMember, string DisplayMember, string TextoInicial)
        {
            cbx.DataSource = null;
            DataTable dt = (DataTable)v.getData(sql);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila[ValueMember] = 0;
            nuevaFila[DisplayMember] = TextoInicial.ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbx.DisplayMember = DisplayMember;
            cbx.ValueMember = ValueMember;
            cbx.DataSource = dt;
        }

        private void cbClasificacionb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbClasificacionb.SelectedIndex > 0)
            {
                cbDescFallob.Enabled = true;
                iniCombos("SELECT iddescfallo id,UPPER(descfallo) as nombre FROM cdescfallo WHERE  falloGralfkcfallosgrales='" + cbClasificacionb.SelectedValue + "' ORDER BY descfallo ASC", cbDescFallob, "id", "nombre", "-SELECIONE UN SUBGRUPO-");
            }
            else
            {
                cbDescFallob.DataSource = null;
                cbDescFallob.Enabled = false;
            }
        }

        private void cbDescFallob_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDescFallob.SelectedIndex > 0)
            {
                cbcategoriasb.Enabled = true;
                v.iniCombos("SELECT idcategoria as id , UPPER(categoria) as categoria FROM catcategorias WHERE subgrupofkcdescfallo='" + cbDescFallob.SelectedValue + "'", cbcategoriasb, "id", "categoria", "-- SELECCIONE UNA CATEGORIA--");
            }
            else
            {
                cbcategoriasb.DataSource = null;
                cbcategoriasb.Enabled = false;
            }
        }

        private void cbnombrefb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0)
                txtgetcodbusq.Text = v.getaData("SELECT codfallo FROM cfallosesp WHERE idfalloEsp = '" + ((ComboBox)sender).SelectedValue + "'").ToString();
            else
                txtgetcodbusq.Clear();

        }

        private void txtgetcodbusq_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.paraNombreFallo(e);
        }
        private void lnkLista_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cbclasificacion.SelectedIndex = 0;
        }
        private void gbbuscar_Enter(object sender, EventArgs e)
        {
        }

        private void pActualizar_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            catCategorias catC = new catCategorias(idUsuario, empresa, area,v);
            catC.Owner = this;
            catC.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (cbcategoriasb.SelectedIndex > 0)
            {
                cbnombrefb.Enabled = true;
                iniCombos("SELECT idfalloEsp id,UPPER(falloesp) as fallo FROM cfallosesp where descfallofkcdescfallo='" + cbcategoriasb.SelectedValue + "'  ORDER BY falloesp ASC", cbnombrefb, "id", "fallo", "-SELECIONE UN NOMBRE-");
            }
            else
            {
                cbnombrefb.DataSource = null;
                cbnombrefb.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbdescripcion.SelectedIndex == 0)
                txtgetdescfallo.Clear();
            else
            {
                if (v.folio != "")
                    lblcodfallo.Text = v.codFalla(cbclasificacion.Text + " " + cbdescripcion.Text + " " + cbcategoria.Text + " " + txtgetdescfallo.Text);


                if (txtgetdescfallo.Text == "")
                {
                    lblcodfallo.Text = null;
                    v.folio = "";
                }
            }
            getCambios(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            iniNombres();
            pActualizar.Visible = false;
            txtgetcodbusq.Clear();
            cbClasificacionb.SelectedIndex = 0;
        }
        private void txtgetdescfallo_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }
        private void gbClasificacion_Enter(object sender, EventArgs e)
        {

        }
    }
}