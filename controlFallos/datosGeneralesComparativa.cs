using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

namespace controlFallos
{
    public partial class datosGeneralesComparativa : Form
    {
        Point formulario = new Point(32, 21);
        int banderaComparativa = 0, idUsuario, empresa, area, idComparativaTemp, idRefaccionTemp, idProveedorTemp, cbrefaccionAnterior, cbproveedorAnterior, statusComparativa;
        bool editarComparativa, editarRefaccion, editarProveedor, mejorOpcionAnterior, exportando, yaAparecioMensaje;
        string nombreComparativaAnterior, DescripcionComparativaAnterior, observacionesComparativaAnterior, refaccionAnterior, observacionesRefaccionAnterior, observacionesProveedorAnterior;
        double cantidadRefaccion, precioUnitario;
        validaciones v;
        public Thread hilo,th;
        public datosGeneralesComparativa(int idUsuario, int empresa, int area,Image logo,validaciones v)
        {
            this.v = v;
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            pblogoright.Image = logo;
            reestablecer();
        }

        private void datosGeneralesComparativa_Load(object sender, EventArgs e)
        {
            iniComparativas();
            InitializespareParts();
            Initializeproviders();

            if (cbxFechasBusq.Checked == false)
            {
                cbxFechasBusq.ForeColor = cbxFechasBusq.Checked ? Color.Crimson : Color.Crimson;
                checkmejor.ForeColor = checkmejor.Checked ? Color.Crimson : Color.Crimson;
            }
            th.Abort();
        }

        private void dgvrefacciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (getCambiosComparativa() || getCambiosRefaccion() || getCambiosProveedor())
                {
                    DialogResult res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        actualizarComparativa();
                        actualizarRefaccion();
                        actualizarProveedor();
                    }
                    else if (res == DialogResult.No)
                        cargarRefaccion(e);
                }
                else
                    cargarRefaccion(e);


            }
        }
        void cargarRefaccion(DataGridViewCellEventArgs e)
        {
            button2_Click(null, e);
            idRefaccionTemp = Convert.ToInt32(dgvrefacciones.Rows[e.RowIndex].Cells[0].Value);
            dgvproveedores.Visible = true;
            iniProveedores();
            pproveedor.Visible = true;
            pComparativas.Visible = prefacciones.Visible = false;
            banderaComparativa = 2;
            limpiar();
            btnnewrefacc.BackgroundImage = Properties.Resources.nut;
            pnewrefpro.Visible = true;
        }
        void InitializespareParts()
        {
            v.iniCombos("SELECT idrefaccion,UPPER(CONCAT(codrefaccion,' - ',nombreRefaccion)) as refaccion FROM crefacciones WHERE status=1 ORDER BY codrefaccion ASC", cbrefaccion, "idrefaccion", "refaccion", "-- SELECCIONE REFACCIÓN --");
        }

        void Initializeproviders()
        {
            v.iniCombos("SELECT idproveedor,UPPER(empresa) as empresa FROM cproveedores WHERE status=1 ORDER BY empresa ASC", cbproveedor, "idproveedor", "empresa", "-- SELECCIONE PROVEEDOR--");
        }

        void iniComparativas()
        {
            dgvcompara.Rows.Clear();
            string sql = @"SET lc_time_names = 'es_ES'; SELECT t1.idcomparativa, UPPER(t1.nombreComparativa), UPPER(t1.descripcionComparativa), UPPER(t1.observacionesComparativa), (SELECT COUNT(*) FROM refaccionescomparativa AS x1 WHERE x1.comparativafkcomparativas = t1.idcomparativa), UPPER(DATE_FORMAT(t1.fechaHoraCreacion, '%d de %M del %Y / %H:%i:%s')), CONCAT(t1.iva, ' %'), CONVERT(CONCAT('$ ', if(t1.status = 3, TRUNCATE(((SELECT COALESCE(SUM((x1.precioUnitario * x2.cantidad)), '0') FROM proveedorescomparativa AS x1 INNER JOIN refaccionescomparativa AS x2 ON x1.refaccionfkrefaccionesComparativa = x2.idrefaccioncomparativa WHERE x2.comparativafkcomparativas = t1.idcomparativa AND x1.mejorOpcion = 1) * (1 + (t1.iva / 100))), 2), '0')) USING utf8) AS total, UPPER(CONCAT(coalesce(t2.nombres,''), ' ', coalesce(t2.apPaterno,''), ' ', coalesce(t2.apMaterno,''))), UPPER(if(t1.status = 1, CONCAT('AGREGANDO REFACCIONES'), if(t1.status = 2, CONCAT('GENERANDO CONCENTRADOS'), if(t1.status = 3, CONCAT('ELIGIENDO MEJOR OPCIÓN'), 'FINALIZADO')))), t1.status FROM comparativas AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona where t1.empresa='" + empresa+"' ORDER BY t1.idcomparativa DESC";
            DataTable t = (DataTable)v.getData(sql);
            for (int i = 0; i < t.Rows.Count; i++) dgvcompara.Rows.Add(t.Rows[i].ItemArray);
            dgvcompara.ClearSelection();
        }

        void iniRefacciones()
        {
            dgvrefacciones.Rows.Clear();
            string sql = @"SELECT t1.idrefaccioncomparativa,UPPER(COALESCE(CONCAT(t3.nombreRefaccion),t1.nombreRefaccion)), cantidad,upper(observaciones), UPPER(CONCAT(coalesce(t2.nombres,''),' ',coalesce(t2.apPaterno,''),' ',coalesce(t2.apMaterno,''))),COALESCE(t1.refaccionfkcrefacciones,''),COALESCE(t1.nombreRefaccion,'') FROM refaccionescomparativa as t1 LEFT JOIN cpersonal as t2 ON t1.usuariofkcpersonal=t2.idpersona LEFT JOIN crefacciones as t3 ON t1.refaccionfkcrefacciones = t3.idrefaccion where comparativafkcomparativas='" + idComparativaTemp + "' and t1.empresa='"+empresa+"' ORDER BY COALESCE(CONCAT(t3.nombreRefaccion),t1.nombreRefaccion) ASC";
            DataTable t = (DataTable)v.getData(sql);
            for (int i = 0; i < t.Rows.Count; i++) dgvrefacciones.Rows.Add(t.Rows[i].ItemArray);
            dgvrefacciones.ClearSelection();
        }

        void iniProveedores()
        {
            dgvproveedores.Rows.Clear();
            string sql = @"SELECT idproveedorComparativa,upper(t2.empresa),precioUnitario,(SELECT (t1.precioUnitario * cantidad) FROM refaccionesComparativa WHERE idrefaccionComparativa=t1.refaccionfkrefaccionesComparativa),t1.observaciones,COALESCE( if(mejorOpcion='1','MEJOR OPCIÓN',''),''), UPPER(CONCAT(coalesce(t3.nombres,''),' ',coalesce(t3.apPaterno,''),' ',coalesce(t3.apMaterno,''))) as nombre,t1.proveedorfkcproveedores FROM proveedorescomparativa as t1 INNER JOIN cproveedores as t2 ON t1.proveedorfkcproveedores= t2.idproveedor INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE t1.refaccionfkrefaccionesComparativa='" + idRefaccionTemp + "' and t1.empresa='"+empresa+"' ORDER BY t2.empresa ASC";
            DataTable t = (DataTable)v.getData(sql);
            for (int i = 0; i < t.Rows.Count; i++) dgvproveedores.Rows.Add(t.Rows[i].ItemArray);
            dgvproveedores.ClearSelection();
        }

        void limpiar()
        {
            if (banderaComparativa == 0)
            {
                txtnombreComparativa.Clear();
                txtdescripcionComparativa.Clear();
                txtobservacionesComparativa.Clear();
                editarRefaccion = false;
                nombreComparativaAnterior = "";
                DescripcionComparativaAnterior = "";
                observacionesComparativaAnterior = "";
            }
            else if (banderaComparativa == 1)
            {
                cbrefaccion.SelectedIndex = 0;
                txtnewrefaccion.Clear();
                txtcantidad.Clear();
                txtobservacionesRefaccion.Clear();
                InitializespareParts();
                editarRefaccion = false;
                gbRef.Text = null;
                InitializespareParts();
                cbrefaccionAnterior = 0;
                refaccionAnterior = null;
                cantidadRefaccion = 0;
                observacionesRefaccionAnterior = "";
                idRefaccionTemp = 0;
                cbrefaccion.Visible = !(txtnewrefaccion.Visible = lblnewrefaccion.Visible = false); lnkLada1.Text = "AGREGAR REFACCIÓN"; lnkLada1.Location = new Point(284, 84);
            }
            else if (banderaComparativa == 2)
            {
                cbproveedor.SelectedIndex = 0;
                txtprecioUnitario.Clear();
                txtobservacionesProveedor.Clear();
                Initializeproviders();
                iniProveedores();
                editarProveedor = false;
                gbproveedores.Text = null;
            }
            yaAparecioMensaje = false;
            iniComparativas();
            btnsave.Visible = lblsave.Visible = true;
            btnsave.BackgroundImage = Properties.Resources.save1;
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (banderaComparativa == 0)
                {
                    if (!editarComparativa)
                        insertarComparativa();
                    else
                        actualizarComparativa();
                }
                else if (banderaComparativa == 1)
                {
                    if (!editarRefaccion)
                        insertarRefaccion();
                    else
                        actualizarRefaccion();
                }
                else if (banderaComparativa == 2)
                {
                    if (!editarProveedor)
                        insertarProveedor();
                    else
                        actualizarProveedor();

                }
                limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK);
            }
        }

        void insertarProveedor()
        {
            object proveedor = cbproveedor.SelectedValue;
            string precioUnitario = txtprecioUnitario.Text.Trim();
            string observaciones = txtobservacionesProveedor.Text.Trim();
            if (!v.camposvaciosProveedorComparativa(Convert.ToInt32(proveedor), precioUnitario) && !v.existeProveedorComparativa(idRefaccionTemp, proveedor))
            {
                if (v.c.insertar(string.Format("INSERT INTO proveedorescomparativa (refaccionfkrefaccionesComparativa, proveedorfkcproveedores, precioUnitario, observaciones, usuariofkcpersonal,empresa) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')", new object[6] { idRefaccionTemp, proveedor, precioUnitario, observaciones, idUsuario,empresa })))
                {
                    MessageBox.Show("Proveedor Agregado a la Refacción de La Comparativa Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                }
            }
        }

        void actualizarProveedor()
        {
            if (getCambiosProveedor())
            {
                object proveedor = cbproveedor.SelectedValue;
                string precioUnitario = txtprecioUnitario.Text.Trim();
                string observaciones = txtobservacionesProveedor.Text.Trim();
                if (!v.camposvaciosProveedorComparativa(Convert.ToInt32(proveedor), precioUnitario) && !v.existeProveedorComparativaActualizar(idRefaccionTemp, cbproveedorAnterior, proveedor))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    string edicion = "";
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        edicion = v.mayusculas(obs.txtgetedicion.Text.Trim());
                        if (v.c.insertar(string.Format("UPDATE proveedorescomparativa SET proveedorfkcproveedores='{1}', precioUnitario='{2}', observaciones='{3}',mejorOpcion='{4}'  where idproveedorComparativa= '{0}'", new object[5] { idProveedorTemp, proveedor, precioUnitario, observaciones, v.getIntFrombool(checkmejor.Checked) })))
                        {
                            if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema (form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('{0}','{1}','{2}','{3}',now(),'{4}','{5}','{6}','{7}')", new object[8] { "Comparativas", idRefaccionTemp, cbproveedorAnterior + ";" + precioUnitario + ";" + observacionesProveedorAnterior + ";" + mejorOpcionAnterior, idUsuario, "Actualización de Refación de Comparativa", edicion, empresa, area })))
                            {
                                if (!yaAparecioMensaje) MessageBox.Show("Proveedor Actualizado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                            }
                        }
                    }
                }
            }
        }

        void insertarRefaccion()
        {
            string refaccion = cbrefaccion.SelectedValue.ToString();
            double cantidad = 0;
            string observaciones = txtobservacionesRefaccion.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtcantidad.Text.Trim())) cantidad = Convert.ToDouble(txtcantidad.Text.Trim());
            if (!v.camposVaciosRefaccionComparativa(Convert.ToInt32(refaccion), cantidad, txtnewrefaccion.Text.Trim()) && !v.existeRefaccionComparativa(idComparativaTemp, refaccion, txtnewrefaccion.Text.Trim()))
            {
                string sql = "INSERT INTO refaccionescomparativa(comparativafkcomparativas, {0}, cantidad, observaciones, usuariofkcpersonal,empresa) VALUES('" + idComparativaTemp + "','{1}','" + cantidad + "','" + observaciones + "','" + idUsuario + "','"+empresa+"')";

                if (cbrefaccion.Visible) sql = string.Format(sql, "refaccionfkcrefacciones", refaccion);
                else
                    sql = string.Format(sql, "nombreRefaccion", txtnewrefaccion.Text.Trim());
                if (v.c.insertar(sql))
                {
                    MessageBox.Show("Refacción Agregada a la Comparativa Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    iniRefacciones();
                    iniComparativas();
                }
            }
        }

        void actualizarRefaccion()
        {
            if (getCambiosRefaccion())
            {
                string refaccion = cbrefaccion.SelectedValue.ToString();
                double cantidad = 0;
                string observaciones = txtobservacionesRefaccion.Text.Trim();
                if (!string.IsNullOrWhiteSpace(txtcantidad.Text.Trim())) cantidad = Convert.ToDouble(txtcantidad.Text.Trim());
                if (!v.camposVaciosRefaccionComparativa(Convert.ToInt32(refaccion), cantidad, txtnewrefaccion.Text.Trim()) && !v.existeRefaccionComparativaActualizar(idComparativaTemp, cbrefaccionAnterior, Convert.ToInt32(refaccion), refaccionAnterior, txtnewrefaccion.Text.Trim()))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    string edicion = "";
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        edicion = v.mayusculas(obs.txtgetedicion.Text.Trim());
                        if (v.c.insertar(string.Format("UPDATE refaccionescomparativa SET refaccionfkcrefacciones={0}, nombreRefaccion={1}, cantidad='{2}', observaciones='{3}' WHERE idrefaccioncomparativa='{4}'", new object[5] { !string.IsNullOrWhiteSpace(txtnewrefaccion.Text.Trim()) ? "NULL" : "'" + refaccion + "'", (cbrefaccion.SelectedIndex > 0) ? "NULL" : "'" + txtnewrefaccion.Text.Trim() + "'", cantidad, observaciones, idRefaccionTemp })))
                        {
                            if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema (form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('{0}','{1}','{2}','{3}',now(),'{4}','{5}','{6}','{7}')", new object[8] { "Comparativas", idRefaccionTemp, refaccionAnterior ?? v.getaData("SELECT nombreRefaccion FROM crefacciones WHERE idrefaccion='" + cbrefaccionAnterior + "'") + ";" + cantidadRefaccion + ";" + observacionesRefaccionAnterior, idUsuario, "Actualización de Refación de Comparativa", edicion, empresa, area })))
                            {
                                if (!yaAparecioMensaje) MessageBox.Show("Refacción de la Comparativa Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                                iniRefacciones();
                            }
                        }
                    }
                }
            }
        }

        void limpiarRefaccion()
        {
            limpiar();
            prefacciones.Visible = true;
            dgvproveedores.Rows.Clear();
            dgvproveedores.Visible = false;
            banderaComparativa = 1;
            dgvrefacciones.ClearSelection();
            pComparativas.Visible = pproveedor.Visible = false;
            pnewrefpro.Visible = false;
        }

        bool getCambiosComparativa()
        {
            if ((!string.IsNullOrWhiteSpace(txtnombreComparativa.Text) && !string.IsNullOrWhiteSpace(txtdescripcionComparativa.Text)) && (!nombreComparativaAnterior.Equals(v.mayusculas(txtnombreComparativa.Text.Trim().ToLower())) || !DescripcionComparativaAnterior.Equals(v.mayusculas(txtdescripcionComparativa.Text.Trim().ToLower())) || !observacionesComparativaAnterior.Equals(v.mayusculas(txtobservacionesComparativa.Text.Trim().ToLower()))))
                return true;
            else
                return false;
        }

        bool getCambiosRefaccion()
        {
            if (((!string.IsNullOrWhiteSpace(txtnewrefaccion.Text) || cbrefaccion.SelectedIndex > 0) && !string.IsNullOrWhiteSpace(txtcantidad.Text)) && (Convert.ToInt32(cbrefaccion.SelectedValue) != cbrefaccionAnterior || !refaccionAnterior.Equals(txtnewrefaccion.Text) || cantidadRefaccion != Convert.ToDouble(txtcantidad.Text) || !observacionesRefaccionAnterior.Equals(txtobservacionesRefaccion.Text)))
                return true;
            else
                return false;
        }

        bool getCambiosProveedor()
        {
            if ((cbproveedor.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtprecioUnitario.Text.Trim())) && (cbproveedorAnterior != Convert.ToInt32(cbproveedor.SelectedValue) || precioUnitario != Convert.ToDouble(txtprecioUnitario.Text.Trim()) || !observacionesProveedorAnterior.Equals(v.mayusculas(txtobservacionesProveedor.Text.ToLower().Trim())) || (statusComparativa == 3 ? mejorOpcionAnterior != checkmejor.Checked : false)))
                return true;
            else
                return false;
        }

        void reestablecer()
        {
            gbTodo.Left = (groupBox1.Width - gbTodo.Width) / 2;
            dgvproveedores.Visible = false;
            dgvrefacciones.Visible = false;
        }

        void mostrarComparativa(DataGridViewCellEventArgs e)
        {
            nuevaComparativa();
            prefacciones.Visible = true;
            pComparativas.Visible = pproveedor.Visible = false;
            idComparativaTemp = Convert.ToInt32(dgvcompara.Rows[e.RowIndex].Cells[0].Value);
            statusComparativa = Convert.ToInt32(v.getaData(string.Format("SELECT status FROM comparativas WHERE idcomparativa='{0}'", idComparativaTemp)));
            gbTodo.Location = formulario;
            dgvproveedores.Visible = false;
            dgvrefacciones.Visible = true;
            iniRefacciones();
            pGenerar.Visible = pnewcompara.Visible = true;
            gbTodo.Text = "COMPARATIVA: " + v.mayusculas(dgvcompara.Rows[e.RowIndex].Cells[1].Value.ToString());
            banderaComparativa = 1;
        }

        void nuevaComparativa()
        {
            txtnombreComparativa.Clear();
            txtdescripcionComparativa.Clear();
            txtobservacionesComparativa.Clear();
            editarRefaccion = false;
            cbrefaccion.SelectedIndex = 0;
            txtnewrefaccion.Clear();
            txtcantidad.Clear();
            txtobservacionesRefaccion.Clear();
            InitializespareParts();
            iniRefacciones();
            editarRefaccion = false;
            cbproveedor.SelectedIndex = 0;
            txtprecioUnitario.Clear();
            txtobservacionesProveedor.Clear();
            Initializeproviders();

            iniProveedores();
            editarProveedor = false;
            reestablecer();
            nombreComparativaAnterior = null;
            DescripcionComparativaAnterior = null;
            observacionesComparativaAnterior = null;
            idComparativaTemp = 0;
            idRefaccionTemp = 0;
            idProveedorTemp = 0;
            txtnombreComparativa.Focus();
            gbTodo.Text = "AGREGAR COMPARATIVA";
            banderaComparativa = 0;
            pnewcompara.Visible = pGenerar.Visible = pnewrefpro.Visible = false;
            editarComparativa = false;
            editarRefaccion = false;
            editarProveedor = false;
            pComparativas.Visible = true;
            cbrefaccion.Visible = !(txtnewrefaccion.Visible = lblnewrefaccion.Visible = false); lnkLada1.Text = "AGREGAR REFACCIÓN"; lnkLada1.Location = new Point(284, 84);
            pproveedor.Visible = prefacciones.Visible = false;
            btnnewrefacc.BackgroundImage = Properties.Resources.ajustes;
            lblnewrefacc.Text = "NUEVA\nREFACCIÓN";
            btnsave.Visible = lblsave.Visible = true;
            btnsave.BackgroundImage = Properties.Resources.save1;
            nombreComparativaAnterior = null;
            DescripcionComparativaAnterior = null;
            observacionesComparativaAnterior = null;
            cbrefaccionAnterior = 0;
            cantidadRefaccion = 0;
            observacionesRefaccionAnterior = null;
            refaccionAnterior = null;
            yaAparecioMensaje = false;
        }

        void insertarComparativa()
        {
            string nombre = v.mayusculas(txtnombreComparativa.Text.Trim().ToLower());
            string descripcion = v.mayusculas(txtdescripcionComparativa.Text.Trim().ToLower());
            string observaciones = v.mayusculas(txtobservacionesComparativa.Text.Trim().ToLower());
            if (!v.camposVaciosComparativa(nombre, descripcion) && !v.existeComparativa(nombre,empresa))
            {
                if (v.c.insertar("INSERT INTO comparativas (nombreComparativa, descripcionComparativa,observacionesComparativa,fechaHoraCreacion, IVA, usuariofkcpersonal,empresa) VALUES('" + nombre + "','" + descripcion + "','" + observaciones + "',NOW(),COALESCE((SELECT iva FROM civa LIMIT 0,1),'0')," + idUsuario + ",'"+empresa+"')"))
                {
                    MessageBox.Show("La Comparativa Se Ha Agregado Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    limpiar();
                }
            }
        }

        private void menuCompara(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (getCambiosComparativa() || getCambiosRefaccion() || getCambiosProveedor())
                {
                    DialogResult res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        actualizarComparativa();
                        actualizarRefaccion();
                        actualizarProveedor();
                        edicion(e.ClickedItem.Name, e);
                    }
                    else if (res == DialogResult.No)
                        edicion(e.ClickedItem.Name, e);
                }
                else
                {
                    edicion(e.ClickedItem.Name, e);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void edicion(string Name, EventArgs e)
        {
            if (Name == "dgvcomparaEDITAR")
            {
                editarComparativa = true;
                nuevaComparativa();
                idComparativaTemp = Convert.ToInt32(dgvcompara.Rows[dgvcompara.CurrentCell.RowIndex].Cells[0].Value);
                txtnombreComparativa.Text = nombreComparativaAnterior = v.mayusculas(dgvcompara.Rows[dgvcompara.CurrentCell.RowIndex].Cells[1].Value.ToString().ToLower());
                txtdescripcionComparativa.Text = DescripcionComparativaAnterior = v.mayusculas(dgvcompara.Rows[dgvcompara.CurrentCell.RowIndex].Cells[2].Value.ToString().ToLower());
                txtobservacionesComparativa.Text = observacionesComparativaAnterior = v.mayusculas(dgvcompara.Rows[dgvcompara.CurrentCell.RowIndex].Cells[3].Value.ToString().ToLower());
                pnewcompara.Visible = true;
                gbTodo.Text = "Actualizar Comparativa: " + nombreComparativaAnterior;
                editarComparativa = true;
            }
            else if (Name == "dgvrefaccionesEDITAR")
            {
                limpiar();
                pproveedor.Visible = !(prefacciones.Visible = true);
                idRefaccionTemp = Convert.ToInt32(dgvrefacciones.Rows[dgvrefacciones.CurrentCell.RowIndex].Cells[0].Value);
                if (!string.IsNullOrWhiteSpace(dgvrefacciones.Rows[dgvrefacciones.CurrentCell.RowIndex].Cells[5].Value.ToString()))
                {
                    cbrefaccion.Visible = !(txtnewrefaccion.Visible = lblnewrefaccion.Visible = false); lnkLada1.Text = "AGREGAR REFACCIÓN"; lnkLada1.Location = new Point(284, 84); txtnewrefaccion.Clear();
                    cbrefaccion.SelectedValue = cbrefaccionAnterior = Convert.ToInt32(dgvrefacciones.Rows[dgvrefacciones.CurrentCell.RowIndex].Cells[5].Value);
                    if (cbrefaccion.SelectedIndex == -1)
                    {
                        v.iniCombos("SELECT idrefaccion,UPPER(CONCAT(codrefaccion,' - ',nombreRefaccion)) as refaccion FROM crefacciones WHERE (status=1 || idrefaccion='" + cbrefaccionAnterior + "') ORDER BY codrefaccion ASC", cbrefaccion, "idrefaccion", "refaccion", "-- SELECCIONE REFACCIÓN --");
                        cbrefaccion.SelectedValue = cbrefaccionAnterior;
                    }
                    }
                else
                {
                    cbrefaccion.Visible = !(txtnewrefaccion.Visible = lblnewrefaccion.Visible = true); lnkLada1.Text = "SELECCIONAR REFACCIÓN"; lnkLada1.Location = new Point(246, 84); cbrefaccion.SelectedIndex = 0;
                    txtnewrefaccion.Text = refaccionAnterior = dgvrefacciones.Rows[dgvrefacciones.CurrentCell.RowIndex].Cells[6].Value.ToString();
                }
                txtcantidad.Text = (cantidadRefaccion = Convert.ToDouble(dgvrefacciones.Rows[dgvrefacciones.CurrentCell.RowIndex].Cells[2].Value)).ToString();
                txtobservacionesRefaccion.Text = dgvrefacciones.Rows[dgvrefacciones.CurrentCell.RowIndex].Cells[3].Value.ToString();
                gbRef.Text = "Actualización de Refacción: " + (cbrefaccionAnterior == 0 ? v.mayusculas(refaccionAnterior.ToLower()) : v.getaData("SELECT nombreRefaccion FROM crefacciones WHERE idrefaccion= '" + cbrefaccionAnterior + "'"));
                banderaComparativa = 1;
                editarRefaccion = true;
                btnnewrefacc.BackgroundImage = Properties.Resources.nut;
                pnewrefpro.Visible = true;
            }
            else if (Name == "dgvproveedoresEDITAR")
            {
                pproveedor.Visible = true;
                pComparativas.Visible = prefacciones.Visible = false;
                btnnewrefacc.BackgroundImage = Properties.Resources.team;
                lblnewrefacc.Text = "Nueva\nProveedor";
                idProveedorTemp = Convert.ToInt32(dgvproveedores.Rows[dgvproveedores.CurrentCell.RowIndex].Cells[0].Value);
                cbproveedor.SelectedValue = cbproveedorAnterior = Convert.ToInt32(dgvproveedores.Rows[dgvproveedores.CurrentCell.RowIndex].Cells[7].Value);
                if (cbproveedor.SelectedIndex == -1)
                {
                    v.iniCombos("SELECT idproveedor,UPPER(empresa) as empresa FROM cproveedores WHERE (status=1 || idproveedor='" + cbproveedorAnterior + "') ORDER BY empresa ASC", cbproveedor, "idproveedor", "empresa", "-- SELECCIONE PROVEEDOR--");
                    cbproveedor.SelectedValue = cbproveedorAnterior;

                }
                txtprecioUnitario.Text = (precioUnitario = Convert.ToDouble(dgvproveedores.Rows[dgvproveedores.CurrentCell.RowIndex].Cells[2].Value)).ToString();
                txtobservacionesProveedor.Text = observacionesProveedorAnterior = v.mayusculas(dgvproveedores.Rows[dgvproveedores.CurrentCell.RowIndex].Cells[4].Value.ToString().ToLower());
                mejorOpcionAnterior = checkmejor.Checked = checkmejor.Visible = (!string.IsNullOrWhiteSpace(dgvproveedores.Rows[dgvproveedores.CurrentCell.RowIndex].Cells[5].Value.ToString()) && dgvproveedores.Rows[dgvproveedores.CurrentCell.RowIndex].Cells[5].Value.ToString().Equals("MEJOR OPCIÓN"));
                gbproveedores.Text = "Actualización Del Proveedor : " + v.getaData("SELECT empresa FROM cproveedores WHERE idproveedor = '" + cbproveedorAnterior + "'");
                editarProveedor = true;
                btnnewrefacc.BackgroundImage = Properties.Resources.team;
                pnewrefpro.Visible = true;
            }
            btnsave.BackgroundImage = Properties.Resources.pencil;
            lblsave.Visible = btnsave.Visible = false;
        }
        private void Cambios(object sender, EventArgs e)
        {
            if (editarComparativa || editarRefaccion || editarProveedor)
            {
                if (getCambiosComparativa() || getCambiosRefaccion() || getCambiosProveedor())

                    btnsave.Visible = lblsave.Visible = true;
                else
                    btnsave.Visible = lblsave.Visible = false;
            }
        }

        void actualizarComparativa()
        {
            if (getCambiosComparativa())
            {
                string nombre = v.mayusculas(txtnombreComparativa.Text.Trim().ToLower());
                string descripcion = v.mayusculas(txtdescripcionComparativa.Text.Trim().ToLower());
                string observaciones = v.mayusculas(txtobservacionesComparativa.Text.Trim().ToLower());
                if (!v.camposVaciosComparativa(nombre, descripcion) && !v.existeComparativaActualizar(nombreComparativaAnterior, nombre,empresa))
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    string edicion = "";
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        edicion = v.mayusculas(obs.txtgetedicion.Text.Trim());
                        if (v.c.insertar(string.Format("UPDATE comparativas SET nombreComparativa='{0}', descripcionComparativa='{1}', observacionesComparativa ='{2}' WHERE idcomparativa='{3}'", new object[4] { nombre, descripcion, observaciones, idComparativaTemp })))
                        {
                            if (v.c.insertar(string.Format("INSERT INTO modificaciones_sistema (form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, motivoActualizacion, empresa, area) VALUES('{0}','{1}','{2}','{3}',now(),'{4}','{5}','{6}','{7}')", new object[8] { "Comparativas", idComparativaTemp, nombreComparativaAnterior + ";" + DescripcionComparativaAnterior + ";" + observacionesComparativaAnterior, idUsuario, "Actualización de Comparativa", edicion, empresa, area })))
                            {
                                if (!yaAparecioMensaje) MessageBox.Show("Comparativa Actualizada Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                limpiar();
                            }
                        }
                    }
                }
            }
        }

        void valfinalexcel()
        {
            if (LblExcel.Text == "EXPORTANDO")
                exportando = true;
            else
                btnExcel.Visible = LblExcel.Visible = false;
        }

        public void exporta_a_excel()
        {
            if (dgvcompara.Rows.Count > 0)
            {
                DataTable dtexcel = new DataTable();
                for (int i = 0; i < dgvcompara.Columns.Count; i++)
                {
                    if (dgvcompara.Columns[i].Visible)
                    {
                        dtexcel.Columns.Add(dgvcompara.Columns[i].HeaderText);
                    }
                }
                for (int j = 0; j < dgvcompara.Rows.Count; j++)
                {
                    DataRow row = dtexcel.NewRow();
                    int indice = 0;
                    for (int i = 0; i < dgvcompara.Columns.Count; i++)
                    {
                        if (dgvcompara.Columns[i].Visible)
                        {
                            row[dtexcel.Columns[indice]] = dgvcompara.Rows[j].Cells[i].Value.ToString().Replace("\n", " ");
                            indice++;
                        }
                    }
                    dtexcel.Rows.Add(row);
                }
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando1);
                    this.Invoke(delega);
                }
                v.exportaExcel(dtexcel);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando2);
                    this.Invoke(delega);
                }
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void limpiarbusquedas()
        {
            txtNombreBusq.Clear();
            txtDescripcionBusq.Clear();
            cbxFechasBusq.Checked = false;
        }
        Thread hiloEx;
        delegate void El_Delegado();
        public void cargando1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "Exportando";
        }
        delegate void El_Delegado1();
        public void cargando2()
        {
            pictureBoxExcelLoad.Image = null;
            LblExcel.Text = "Exportar";
            btnExcel.Visible = LblExcel.Visible = true;
            if (exportando)
            {
                btnExcel.Visible = LblExcel.Visible = false;
            }
            exportando = false;
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
        }

        private void txtprecioUnitario_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtCantidad = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(txtCantidad.Text.Trim()))
            {
                while (txtCantidad.Text.Contains(".."))
                    txtCantidad.Text = txtCantidad.Text.Replace("..", ".").Trim();
                txtCantidad.Text = txtCantidad.Text.Trim();
                txtCantidad.Text = string.Format("{0:F2}", txtCantidad.Text);
                try
                {
                    if (Convert.ToDouble(txtCantidad.Text) > 0)
                    {
                        CultureInfo ti = new CultureInfo("es-MX"); ti.NumberFormat.CurrencyDecimalDigits = 2; ti.NumberFormat.CurrencyDecimalSeparator = "."; txtCantidad.Text = string.Format("{0:N2}", Convert.ToDouble(txtCantidad.Text, ti));
                    }
                    else txtCantidad.Text = "0";
                }
                catch (Exception ex)
                {
                    txtCantidad.Clear(); MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void White(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            dgvcompara.Rows.Clear();
            string dpini, dpfin;
            dpini = dtpFechaDe.Value.ToString("dd/MM/yyyy");
            dpfin = dtpFechaA.Value.ToString("dd/MM/yyyy");
            if (string.IsNullOrWhiteSpace(txtNombreBusq.Text) && string.IsNullOrWhiteSpace(txtDescripcionBusq.Text) && !cbxFechasBusq.Checked)
            {
                MessageBox.Show("Los campos de búsqueda están vacíos", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                iniComparativas();
                pActualizar.Visible = false;
                btnExcel.Visible = LblExcel.Visible = false;
            }
            else
            {
                if (Convert.ToDateTime(dtpFechaDe.Value.ToString("dd/MM/yyyy")) > Convert.ToDateTime(dtpFechaA.Value.ToString("dd/MM/yyyy")))
                {
                    MessageBox.Show("La fecha inicial no debe superar la fecha final", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    iniComparativas();
                }
                else if (Convert.ToDateTime(dtpFechaDe.Value.ToString("dd/MM/yyyy")) > DateTime.Now)
                {
                    MessageBox.Show("La fecha inicial no puede superar la fecha de hoy", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    iniComparativas();
                }
                else
                {
                    string consulta = "SET lc_time_names = 'es_ES'; SELECT t1.idcomparativa, UPPER(t1.nombreComparativa), UPPER(t1.descripcionComparativa), UPPER(t1.observacionesComparativa), (SELECT COUNT(*) FROM refaccionescomparativa AS x1 WHERE x1.comparativafkcomparativas = t1.idcomparativa), UPPER(DATE_FORMAT(t1.fechaHoraCreacion, '%d de %M del %Y / %H:%i:%s')), CONCAT(t1.iva, ' %'), CONVERT(CONCAT('$ ', if(t1.status = 3, TRUNCATE(((SELECT COALESCE(SUM((x1.precioUnitario * x2.cantidad)), '0') FROM proveedorescomparativa AS x1 INNER JOIN refaccionescomparativa AS x2 ON x1.refaccionfkrefaccionesComparativa = x2.idrefaccioncomparativa WHERE x2.comparativafkcomparativas = t1.idcomparativa AND x1.mejorOpcion = 1) * (1 + (t1.iva / 100))), 2), '0')) USING utf8) AS total, UPPER(CONCAT(coalesce(t2.nombres,''), ' ', coalesce(t2.apPaterno,''), ' ', coalesce(t2.apMaterno,''))), UPPER(if(t1.status = 1, CONCAT('AGREGANDO REFACCIONES'), if(t1.status = 2, CONCAT('GENERANDO CONCENTRADOS'), if(t1.status = 3, CONCAT('ELIGIENDO MEJOR OPCIÓN'), 'FINALIZADO')))), t1.status FROM comparativas AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona";
                    string where = "";
                    if (!string.IsNullOrWhiteSpace(txtNombreBusq.Text))
                    {
                        if (string.IsNullOrWhiteSpace(where))
                            where = " WHERE t1.nombreComparativa = '" + txtNombreBusq.Text + "'";
                        else
                            where += " AND t1.nombreComparativa = '" + txtNombreBusq.Text + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(txtDescripcionBusq.Text))
                    {
                        if (string.IsNullOrWhiteSpace(where))
                            where = " WHERE t1.descripcionComparativa = '" + txtDescripcionBusq.Text + "'";
                        else
                            where += " AND t1.descripcionComparativa = '" + txtDescripcionBusq.Text + "'";
                    }
                    if (cbxFechasBusq.Checked == true)
                    {
                        if (string.IsNullOrWhiteSpace(where))
                            where = " WHERE DATE_FORMAT(t1.fechaHoraCreacion, '%d/%m/%Y') BETWEEN '" + dpini + "' AND '" + dpfin + "'";
                        else
                            where += " AND DATE_FORMAT(t1.fechaHoraCreacion, '%d/%m/%Y') BETWEEN '" + dpini + "' AND '" + dpfin + "'";
                    }
                    where += " and empresa='"+empresa+"' ORDER BY t1.idcomparativa DESC";
                    DataTable dt = (DataTable)v.getData(consulta + where);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se Encontraron Resultados con los Criterios Seleccionados", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        iniComparativas();
                        pActualizar.Visible = false;
                        btnExcel.Visible = LblExcel.Visible = false;
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                            dgvcompara.Rows.Add(dt.Rows[i].ItemArray);
                        pActualizar.Visible = true;
                        btnExcel.Visible = LblExcel.Visible = true;
                    }
                    limpiarbusquedas();
                    dgvcompara.ClearSelection();
                }
            }
        }

        private void btnMTodo_Click(object sender, EventArgs e)
        {
            iniComparativas();
            valfinalexcel();
            pActualizar.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (banderaComparativa == 2)
            {
                limpiarRefaccion();
            }
            if (editarRefaccion)
            {
                if (getCambiosRefaccion())
                {
                    DialogResult res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        buttonAgregar_Click(null, e);
                    }
                    if (res == DialogResult.Yes || res == DialogResult.No)
                        limpiarRefaccion();
                }
                else
                {
                    limpiarRefaccion();
                }
            }
            else if (editarProveedor)
            {
                if (getCambiosProveedor())
                {
                    DialogResult res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        buttonAgregar_Click(null, e);
                    }
                    if (res == DialogResult.Yes || res == DialogResult.No)
                        limpiar();
                }
                else
                {
                    limpiar();
                }
            }
        }

        private void cbrefaccion_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbrefaccion.SelectedIndex > 0)
                lblum.Text = v.getaData(string.Format("SELECT UPPER(t4.Simbolo) FROM crefacciones as t1 INNER JOIN cmarcas as t2 ON t1.marcafkcmarcas=t2.idmarca INNER JOIN cfamilias as t3 ON t2.descripcionfkcfamilias=t3.idfamilia INNER JOIN cunidadmedida as t4 ON t3.umfkcunidadmedida=t4.idunidadmedida where idrefaccion='{0}'", cbrefaccion.SelectedValue)).ToString();
            else
                lblum.Text = "";
        }

        private void lnkLada1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cbrefaccion.Visible)
            {
                cbrefaccion.Visible = !(txtnewrefaccion.Visible = lblnewrefaccion.Visible = true); lnkLada1.Text = v.mayusculas("SELECCIONAR REFACCIÓN".ToLower()); lnkLada1.Location = new Point(246, 84); cbrefaccion.SelectedIndex = 0;
                if (!string.IsNullOrWhiteSpace(refaccionAnterior)) txtnewrefaccion.Text = refaccionAnterior;
            }
            else
            {
                cbrefaccion.Visible = !(txtnewrefaccion.Visible = lblnewrefaccion.Visible = false); lnkLada1.Text = v.mayusculas("AGREGAR REFACCIÓN".ToLower()); lnkLada1.Location = new Point(284, 84); txtnewrefaccion.Clear();
                if (cbrefaccionAnterior > 0) cbrefaccion.SelectedValue = cbrefaccionAnterior;
            }
        }

        private void dgvcompara_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (getCambiosComparativa() || getCambiosRefaccion() || getCambiosProveedor())
                {
                    DialogResult res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        actualizarComparativa();
                        actualizarProveedor();
                        actualizarProveedor();
                        mostrarComparativa(e);
                    }
                    else if (res == DialogResult.No)
                        mostrarComparativa(e);
                }
                else
                    mostrarComparativa(e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConcentradosComparativas conc = new ConcentradosComparativas(idComparativaTemp,v);
            conc.Owner = this;
            if (Convert.ToInt32(v.getaData("SELECT status FROM comparativas WHERE idcomparativa='" + idComparativaTemp + "'")) == 1)
            {
                v.c.insertar("UPDATE comparativas SET status=2 WHERE idcomparativa='" + idComparativaTemp + "'");
                iniComparativas();
            }
            conc.ShowDialog();
        }

        private void dgvcompara_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                if (Convert.ToInt32(dgvcompara.Rows[e.RowIndex].Cells[10].Value) == 1)
                    dgvcompara.Rows[e.RowIndex].Cells[9].Style.BackColor = Color.SkyBlue;
                else if (Convert.ToInt32(dgvcompara.Rows[e.RowIndex].Cells[10].Value) == 2)
                    dgvcompara.Rows[e.RowIndex].Cells[9].Style.BackColor = Color.Khaki;
                else if (Convert.ToInt32(dgvcompara.Rows[e.RowIndex].Cells[10].Value) == 3)
                    dgvcompara.Rows[e.RowIndex].Cells[9].Style.BackColor = Color.LightGreen;
            }
        }
        private void buttonNuevoOC_Click(object sender, EventArgs e)
        {
            if (getCambiosComparativa() || getCambiosRefaccion() || getCambiosProveedor())
            {
                DialogResult res = MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    actualizarComparativa();
                    actualizarRefaccion();
                    actualizarProveedor();
                    nuevaComparativa();
                }
                else if (res == DialogResult.No)
                    nuevaComparativa();
            }
            else
            {
                nuevaComparativa();
            }
        }

        private void cbxFechasBusq_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxFechasBusq.Checked)
                dtpFechaDe.Enabled = dtpFechaA.Enabled = true;
            else
            {
                dtpFechaDe.Enabled = dtpFechaA.Enabled = false;
                dtpFechaDe.Value = dtpFechaA.Value = DateTime.Now;
            }
        }
        private void dgvproveedores_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.Value.ToString().Equals("MEJOR OPCIÓN"))
                e.CellStyle.BackColor = Color.PaleGreen;
        }

        private void cbrefaccion_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        public void btnall_MouseLeave(object sender, EventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(54, 54);
        }

        public void btnall_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnall = sender as Button;
            btnall.Size = new Size(59, 59);
        }

        public void btnallb_MouseLeave(object sender, EventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(42, 42);
        }

        public void btnallb_MouseMove(object sender, MouseEventArgs e)
        {
            Button btnallb = sender as Button;
            btnallb.Size = new Size(47, 47);
        }
        public void txtall_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }
        public void txtnumall_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                MessageBox.Show("Solo se pueden introducir números y un solo punto decimal", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                MessageBox.Show("Ya existe un punto decimal en la caja de texto", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void dgvrefacciones_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView dgv = sender as DataGridView;
                int xy = dgv.HitTest(e.X, e.Y).RowIndex;
                if (xy >= 0)
                {
                    ContextMenuStrip menu = new ContextMenuStrip();
                    menu.Items.Add("Actualizar", Properties.Resources.edit1).Name = dgv.Name + "EDITAR";
                    menu.BackColor = Color.FromArgb(200, 200, 200);
                    menu.ForeColor = Color.FromArgb(75, 44, 52);
                    menu.Font = this.Font;
                    menu.Cursor = Cursors.Hand;
                    menu.Name = dgv.Name + "1";
                    dgv.CurrentCell = dgv.Rows[xy].Cells[1];
                    menu.Show(dgv, new Point(e.X, e.Y));
                    menu.ItemClicked += new ToolStripItemClickedEventHandler(menuCompara);
                }
            }
        }
        public void groupBoxAll_Paint(object sender, PaintEventArgs e)
        {
            GroupBox gbxall = sender as GroupBox;
            v.DrawGroupBox(gbxall, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
    }
}