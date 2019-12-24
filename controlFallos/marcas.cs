using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace controlFallos
{
    public partial class marcas : Form
    {
        validaciones v;
        int idmarcaTemp, idUsuario, status, empresa, area;
        bool reactivar;
        public bool editar { protected internal set; get; }
        string marcaAnterior;
        bool Pinsertar { set; get; }
        bool Pconsultar { set; get; }
        bool Peditar { set; get; }
        bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        string familiaAnterior, DescripcionAnterior;
        new catRefacciones Owner;
        public marcas(int idUsuario, int empresa, int area, Form fh, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            tbmarcas.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbfamilia.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbdesc.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbfamilia.DrawItem += v.combos_DrawItem;
            cbdesc.DrawItem += v.combos_DrawItem;
            this.empresa = empresa;
            this.area = area;
            Owner = (catRefacciones)fh;
        }
        void getCambios(object sender, EventArgs e)
        {
            if (editar)
            {
                int descfamilia = 0; if (cbdesc.DataSource != null) descfamilia = Convert.ToInt32(cbdesc.SelectedValue);
                if (status == 1 && (cbfamilia.SelectedIndex > 0 && descfamilia > 0 && !string.IsNullOrWhiteSpace(txtmarca.Text)) && (!familiaAnterior.Equals(cbfamilia.SelectedValue.ToString()) || !DescripcionAnterior.Equals(cbdesc.SelectedValue.ToString()) || marcaAnterior != v.mayusculas(txtmarca.Text.ToLower().Trim())))
                    btnsave.Visible = lblsave.Visible = true;
                else
                    btnsave.Visible = lblsave.Visible = false;
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
                Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
            }
            mostrar();
        }
        void mostrar()
        {
            gbadd.Visible = Pinsertar || Peditar;
            gbconsulta.Visible = Pconsultar;
            label2.Visible = label3.Visible = Peditar;
        }
        private void marcas_Load(object sender, EventArgs e)
        {
            establecerPrivilegios();
            if (Pconsultar)
            {
                insertarums();
            }
            if (Pinsertar || Peditar)
            {
                inifamilias();
                txtmarca.AutoCompleteCustomSource = cargarMarcas();
                txtmarca.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtmarca.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
        }
        void inifamilias()
        {
            v.iniCombos("SELECT idcnfamilia as id, familia FROM cnfamilias WHERE status='1' ORDER BY familia ASC", cbfamilia, "id", "familia", "--SELECCIONE UNA FAMILIA--");
        }
        private AutoCompleteStringCollection cargarMarcas()
        {
            AutoCompleteStringCollection data = new AutoCompleteStringCollection();

            MySqlCommand cm = new MySqlCommand("SELECT UPPER(marca) as m FROM cmarcas WHERE marca LIKE '" + v.mayusculas(txtmarca.Text.Trim().ToLower()) + "%'", v.c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                data.Add(dr.GetString("m"));
            }
            dr.Close();
            v.c.dbcon.Close();
            return data;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                    _insertar();
                else
                    _editar();
            }
            catch (Exception ex){MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);}
        }
        void _editar()
        {
            string marca = v.mayusculas(txtmarca.Text.ToLower());
            int familia = Convert.ToInt32(cbfamilia.SelectedValue);
            int descfamilia = 0; if (cbdesc.DataSource != null) descfamilia = Convert.ToInt32(cbdesc.SelectedValue);
            if (!v.camposVaciosCatMarcas(familia, descfamilia, marca) && !v.existeMarcaActualizar(descfamilia, Convert.ToInt32(DescripcionAnterior), marca, this.marcaAnterior, empresa))
            {
                observacionesEdicion obs = new observacionesEdicion(v);
                obs.Owner = this;
                if (obs.ShowDialog() == DialogResult.OK)
                {
                    string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                    if (v.c.insertar("UPDATE cmarcas SET descripcionfkcfamilias='" + descfamilia + "', marca=LTRIM(RTRIM('" + marca + "')) WHERE idmarca='" + this.idmarcaTemp + "'"))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Marcas','" + idmarcaTemp + "','" + familiaAnterior + ";" + marcaAnterior + "','" + idUsuario + "',NOW(),'Actualización de Marca','" + observaciones + "','" + empresa + "','" + area + "')");
                        if (!yaAparecioMensaje) MessageBox.Show("Marca Actualizada Existosamante", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                    }
                }
            }
        }
        void _insertar()
        {
            string marca = v.mayusculas(txtmarca.Text.ToLower());
            int familia = Convert.ToInt32(cbfamilia.SelectedValue);
            int descfamilia = 0; if (cbdesc.DataSource != null) descfamilia = Convert.ToInt32(cbdesc.SelectedValue);
            if (!v.camposVaciosCatMarcas(familia, descfamilia, marca) && !v.existeMarca(descfamilia, marca, empresa))
            {
                if (v.c.insertar("INSERT INTO cmarcas(descripcionfkcfamilias,marca,personafkcpersonal,empresa) VALUES('" + descfamilia + "',LTRIM(RTRIM('" + marca + "')),'" + this.idUsuario + "','"+empresa+"')"))
                {
                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Refacciones - Marcas',(SELECT idmarca FROM cmarcas WHERE descripcionfkcfamilias='" + descfamilia + "' AND marca='" + marca + "'),'" + descfamilia + "," + marca + "','" + idUsuario + "',NOW(),'Inserción de Marca','" + empresa + "','" + area + "')");
                    Owner.marca = v.getaData("SELECT idmarca FROM cmarcas WHERE descripcionfkcfamilias='" + descfamilia + "' AND marca='" + marca + "'").ToString();
                    MessageBox.Show("Marca Agregada Existosamante", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();

                }
                else
                    MessageBox.Show("La Marca no se Agrego", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public void insertarums()
        {
            tbmarcas.DataSource = null;
            tbmarcas.DataSource = v.getData("SELECT idmarca,upper(t3.Familia) as 'FAMILIA',upper(t2.descripcionFamilia) as 'DESCRIPCIÓN FAMILIA', upper(t1.marca) as 'MARCA', upper(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) AS 'PERSONA QUE DIÓ DE ALTA', if (t1.status = 1,'ACTIVO','NO ACTIVO') AS 'ESTATUS', idcnFamilia,idfamilia FROM cmarcas as t1 INNER JOIN cfamilIas as t2 ON t1.descripcionfkcfamilias=t2.idfamilia INNER JOIN cnfamilias as t3 ON t2.familiafkcnfamilias=t3.idcnfamilia INNER JOIN cpersonal as t4 ON t1.personafkcpersonal=t4.idpersona;");
            if (tbmarcas.DataSource != null) tbmarcas.Columns[0].Visible = tbmarcas.Columns[6].Visible = tbmarcas.Columns[7].Visible = false;
            tbmarcas.ClearSelection();
        }

        private void tbmarcas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbmarcas.Columns[e.ColumnIndex].HeaderText == "ESTATUS")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            int descfamilia = 0; if (cbdesc.DataSource != null) descfamilia = Convert.ToInt32(cbdesc.SelectedValue);
            if (status == 1 && (cbfamilia.SelectedIndex > 0 && descfamilia > 0 && !string.IsNullOrWhiteSpace(txtmarca.Text)) && (!familiaAnterior.Equals(cbfamilia.SelectedValue.ToString()) || !DescripcionAnterior.Equals(cbdesc.SelectedValue.ToString()) || marcaAnterior != v.mayusculas(txtmarca.Text.ToLower().Trim())))
            {
                if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    button2_Click(null, e);
                }
                else
                    limpiar();
            }
            else
                limpiar();
        }

        private void txtmarca_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) button2_Click(null, e);
            else
                v.letrasynumeros(e);
        }

        private void tbmarcas_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void gbadd_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NombresFamilias nm = new NombresFamilias(idUsuario, empresa, area, v);
            nm.Owner = this;
            nm.Show();
        }

        private void cbfamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbfamilia.SelectedIndex > 0)
            {
                if (Convert.ToInt32(v.getaData("SELECT count(*) FROM cfamilias WHERE familiafkcnfamilias='" + cbfamilia.SelectedValue + "' AND status='1'")) > 0)
                {
                    v.iniCombos("SELECT idfamilia as id, UPPER(descripcionFamilia) as descr FROM cfamilias WHERE familiafkcnfamilias='" + cbfamilia.SelectedValue + "' AND status='1' ORDER BY descripcionFamilia ASC", cbdesc, "id", "descr", "--SELECCIONE UNA DESCRIPCIÓN--");
                    cbdesc.Enabled = true;
                }
                else
                {
                    cbdesc.DataSource = null;
                    cbdesc.Enabled = false;
                }
            }
            else
            {
                cbdesc.DataSource = null;
                cbdesc.Enabled = false;
            }
        }

        private void btnaddpasillo_Click(object sender, EventArgs e)
        {
            familias f = new familias(idUsuario, empresa, area, v);
            f.Owner = this;
            f.ShowDialog();

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            NombresFamilias n = new NombresFamilias(idUsuario, empresa, area, v);
            n.Owner = this;
            n.ShowDialog();
        }

        private void gbadd_Enter(object sender, EventArgs e){}
        private void cbdesc_SelectedIndexChanged(object sender, EventArgs e){}

        private void tbmarcas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int descfamilia = 0; if (cbdesc.DataSource != null) descfamilia = Convert.ToInt32(cbdesc.SelectedValue);
                if (idmarcaTemp > 0 && status == 1 && (cbfamilia.SelectedIndex > 0 && descfamilia > 0 && !string.IsNullOrWhiteSpace(txtmarca.Text)) && (!familiaAnterior.Equals(cbfamilia.SelectedValue.ToString()) || !DescripcionAnterior.Equals(cbdesc.SelectedValue.ToString()) || marcaAnterior != v.mayusculas(txtmarca.Text.ToLower().Trim())))
                {
                    if (MessageBox.Show("¿Desea Guardar la Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        button2_Click(null, e);
                    }
                    else
                        guardarReporte(e);
                }
                else
                    guardarReporte(e);
            }
        }

        private void txtmarca_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                this.idmarcaTemp = Convert.ToInt32(tbmarcas.Rows[e.RowIndex].Cells[0].Value);
                status = v.getStatusInt(tbmarcas.Rows[e.RowIndex].Cells[5].Value.ToString());
                if (Pdesactivar)
                {
                    if (status == 0)
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
                    pdeletefam.Visible = true;
                }
                if (Peditar)
                {
                    cbfamilia.SelectedValue = familiaAnterior = tbmarcas.Rows[e.RowIndex].Cells[6].Value.ToString();
                    if (cbfamilia.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idcnfamilia as id, familia FROM cnfamilias WHERE (status='1' OR idcnfamilia='" + familiaAnterior + "') ORDER BY familia ASC", cbfamilia, "id", "familia", "--SELECCIONE UNA FAMILIA--");
                        cbfamilia.SelectedValue = familiaAnterior;
                    }

                    cbdesc.SelectedValue = DescripcionAnterior = tbmarcas.Rows[e.RowIndex].Cells[7].Value.ToString();
                    if (cbdesc.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idfamilia as id, UPPER(descripcionFamilia) as descr FROM cfamilias WHERE familiafkcnfamilias='" + cbfamilia.SelectedValue + "' AND (status='1' OR idfamilia='" + DescripcionAnterior + "') ORDER BY descripcionFamilia ASC", cbdesc, "id", "descr", "--SELECCIONE UNA DESCRIPCIÓN--");
                        cbdesc.SelectedValue = DescripcionAnterior;
                        cbdesc.Enabled = true;
                    }
                    txtmarca.Text = marcaAnterior = v.mayusculas(tbmarcas.Rows[e.RowIndex].Cells[3].Value.ToString().ToLower());
                    if (Pinsertar) pcancel.Visible = true;
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    lblsave.Text = "Guardar";
                    gbadd.Text = "Actualizar Marca";
                    tbmarcas.ClearSelection();
                    btnsave.Visible = lblsave.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex){MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);}
        }
        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            if (idmarcaTemp > 0)
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
                if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t3.status FROM cmarcas as t1 INNER JOIN cfamilias as t2 ON t1.descripcionfkcfamilias=t2.idfamilia INNER JOIN cnfamilias as t3 ON t2.familiafkcnfamilias=t3.idcnfamilia WHERE idmarca='{0}'", idmarcaTemp))) == 0) MessageBox.Show("Error Al Reactivar:\n Nombre de Familia Desactivado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (status == 1 && Convert.ToInt32(v.getaData(string.Format("SELECT t2.status FROM cmarcas as t1 INNER JOIN cfamilias as t2 ON t1.descripcionfkcfamilias=t2.idfamilia WHERE idmarca='{0}'", idmarcaTemp))) == 0) MessageBox.Show("Error Al Reactivar:\n Descripción de Familia Desactivada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación De La Marca de Refacción";
                    obs.lblinfo.Location = new Point(obs.lblinfo.Location.X - 35, obs.lblinfo.Location.Y);
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                        try
                        {
                            String sql = "UPDATE cmarcas SET status = " + status + " WHERE idmarca  = " + this.idmarcaTemp;
                            if (v.c.insertar(sql))
                            {
                                var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones - Marcas','" + idmarcaTemp + "','" + msg + "activación de Marca','" + idUsuario + "',NOW(),'" + msg + "activación de Marca','" + edicion + "','" + empresa + "','" + area + "')");
                                MessageBox.Show("La Marca ha sido " + msg + "activada Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                                insertarums();
                            }
                            else
                                MessageBox.Show("La Marca no ha sido " + msg);

                        }
                        catch (Exception ex){MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);}
                    }
                }
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                gbadd.Text = "Agregar Marca";
                lblsave.Text = "Agregar";
                btnsave.Visible = lblsave.Visible = true;
                editar = false;
                cbfamilia.Focus();
            }
            if (Pconsultar)
                insertarums();
            txtmarca.Clear();
            this.idmarcaTemp = 0;
            reactivar = false;
            pdeletefam.Visible = false;
            pcancel.Visible = false;
            yaAparecioMensaje = false;
            btnsave.Visible = lblsave.Visible = true;
            marcaAnterior = null;
            cbfamilia.SelectedIndex = 0;
            inifamilias();
        }
    }
}