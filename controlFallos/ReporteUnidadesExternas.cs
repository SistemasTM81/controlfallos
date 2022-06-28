
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using h = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;


namespace controlFallos
{
    public partial class ReporteUnidadesExternas : Form
    {
        public void ubica()
        {
            pPdf.Location = new Point(gbxbusqueda.Location.X + gbxbusqueda.Width + 10, gbxbusqueda.Location.Y); pguardar.Location = new Point(pPdf.Location.X + pPdf.Width + 10, gbxbusqueda.Location.Y); pfinalizar.Location = new Point(pguardar.Location.X + pguardar.Width + 10, gbxbusqueda.Location.Y); pcancelar.Location = new Point(pfinalizar.Location.X + pfinalizar.Width + 10, gbxbusqueda.Location.Y);
            gbrefacciones.Location = new Point(gbxDiag.Location.X, 11);
        }
        bool pinsertar { get; set; }
        bool peditar { get; set; }

        bool editar, cambiosEnFolios = false, editarRefaccion, EnviarAlmacen = false;

        validaciones v;
        conexion con;

        int idUsuario, empresa, area, EstatusAnterior, idmecanico, idmecanicoApoyo, idmecaniAnterior, idmecanicoapoyoAnterior, idreporte, retornos = 0, conteoListFolios = 0, idrefaccionAnterior, cantidadAnterior, idref, entregaRefacciones = 0;

        string observacionesAnterior, folioAnterior, trabajoAnterior, RefaccionesSolicitadas = "";

        string[] ArraryInformativo;



        public ReporteUnidadesExternas(int idUsuario, int empresa, int area, validaciones v)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.v = v;

            DateTime fecha = DateTime.Now;
            tbxHoraEnvio.Text = fecha.ToString();

            cmbEstatus1.DrawItem += v.combos_DrawItem;
            EstatusRepa.DrawItem += v.combos_DrawItem;
            cmbEstatus.DrawItem += v.combos_DrawItem;
            cmbTipoR.DrawItem += v.combos_DrawItem;
            cmbEstRep.DrawItem += v.combos_DrawItem;
            cmbRefacciones1.DrawItem += v.combos_DrawItem;
            cmbReTip.DrawItem += v.combos_DrawItem;
        }

        private void cboxRango_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxRango.Checked)
            {
                dtpFechaA.Enabled = dtpFechaDe.Enabled = !(cmbmes1.Enabled = false);
                cmbmes1.SelectedIndex = 0;
            }
            else
            {
                cmbmes1.Enabled = !(dtpFechaDe.Enabled = dtpFechaA.Enabled = false);
                dtpFechaA.Value = dtpFechaDe.Value = dtpFechaDe.MaxDate;
            }
        }

        private void btnpdf_Click(object sender, EventArgs e)
        {
            // pdf();
        }
        /*/////////////////////PRUEBAS////////////////////*/
        private void button1_Click(object sender, EventArgs e)
        {

            gbxbusqueda.Visible = true;
            gbxDiag.Visible = true;
            gbxUnidad.Visible = true;

            pictureBox1.Visible = false;
            lblText.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            gbxbusqueda.Visible = false;
            gbxDiag.Visible = false;
            gbxUnidad.Visible = false;

            pictureBox1.Visible = true;
            lblText.Visible = true;
        }


        void combo()
        {
            //refacciones
            v.comboswithuot(cmbRefacciones1, new string[] { "--seleccione una opción--", "se requieren refacciones", "no se requieren refacciones" });
            //diagnostico
            v.comboswithuot(cmbEstatus1, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
            v.comboswithuot(cmbEstatus, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada", });
            //mes
            v.comboswithuot(cmbmes1, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
            //tipo de reparacion
            v.comboswithuot(cmbTipoR, new string[] { "--seleccione el Tipo --", "preventivo", "correctivo", "reiterativo" });
            v.comboswithuot(cmbReTip, new string[] { "--seleccione el Tipo --", "preventivo", "correctivo", "reiterativo" });
            //eststus reparacion
            v.comboswithuot(cmbEstRep, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
            v.comboswithuot(EstatusRepa, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada", });
            //empresa
            v.comboswithuot(cbxEmpresaS, new string[] { "--seleccione una empresa--", "atreyo tour", "browser", "cometa de oro", "nezahualpilli", "travelers" });
            v.comboswithuot(cbxSempresa, new string[] { "--seleccione una empresa--", "atreyo tour", "browser", "cometa de oro", "nezahualpilli", "travelers" });
            //unidades
            v.comboswithuot(cmbUnidad, new string[] { "--seleccione unidad--"});
        }

        private void cmbmes1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbEstatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbDiagTip_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void EstatusRepa_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbTipoR_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbRefacciones1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbEstatus1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbEstRep_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbUnidad1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbMecanicob1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbUnidad1_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cbxSempresa_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cbxEmpresaS_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void cmbUnidad_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        private void pintarcombos(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cbxSempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSempresa.Text == "--SELECCIONE UNA EMPRESA--")
            { 
                cmbUnidad.Enabled = false;
            }

            if (cbxSempresa.Text == "ATREYO TOUR")
            {
                cmbUnidad.DataSource = null;
                DataTable dt = (DataTable)v.getData("Select t1.idunidad as id, concat(t1.consecutivo,'-', t1.descripcioneco) as unidad from cunidades as t1 where t1.descripcioneco like '%atreyo tours%'");
                DataRow nuevaFila = dt.NewRow();
                nuevaFila["id"] = 0;
                nuevaFila["unidad"] = "--SELECCIONE UNIDAD--".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                cmbUnidad.DisplayMember = "id";
                cmbUnidad.ValueMember = "unidad";
                cmbUnidad.DataSource = dt;

                cmbUnidad.Enabled = true;
            }
            if (cbxSempresa.Text == "COMETA DE ORO")
            {
                cmbUnidad.DataSource = null;
                DataTable dt = (DataTable)v.getData("Select t1.idunidad as id, concat(t1.consecutivo,'-', t1.descripcioneco) as unidad from cunidades as t1 where t1.descripcioneco like '%cometa de oro%'");
                DataRow nuevaFila = dt.NewRow();
                nuevaFila["id"] = 0;
                nuevaFila["unidad"] = "--SELECCIONE UNIDAD--".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                cmbUnidad.DisplayMember = "id";
                cmbUnidad.ValueMember = "unidad";
                cmbUnidad.DataSource = dt;

                cmbUnidad.Enabled = true;
            }
            if (cbxSempresa.Text == "BROWSER")
            {
                cmbUnidad.DataSource = null;
                DataTable dt = (DataTable)v.getData("Select t1.idunidad as id, concat(t1.consecutivo,'-', t1.descripcioneco) as unidad from cunidades as t1 where t1.descripcioneco like '%browser%'");
                DataRow nuevaFila = dt.NewRow();
                nuevaFila["id"] = 0;
                nuevaFila["unidad"] = "--SELECCIONE UNIDAD--".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                cmbUnidad.DisplayMember = "id";
                cmbUnidad.ValueMember = "unidad";
                cmbUnidad.DataSource = dt;

                cmbUnidad.Enabled = true;
            }
            if (cbxSempresa.Text == "NEZAHUALPILLI")
            {
                cmbUnidad.DataSource = null;
                DataTable dt = (DataTable)v.getData("Select t1.idunidad as id, concat(t1.consecutivo,'-', t1.descripcioneco) as unidad from cunidades as t1 where t1.descripcioneco like '%nezahualpilli%'");
                DataRow nuevaFila = dt.NewRow();
                nuevaFila["id"] = 0;
                nuevaFila["unidad"] = "--SELECCIONE UNIDAD--".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                cmbUnidad.DisplayMember = "id";
                cmbUnidad.ValueMember = "unidad";
                cmbUnidad.DataSource = dt;

                cmbUnidad.Enabled = true;
            }
            if (cbxSempresa.Text == "TRAVELERS")
            {
                cmbUnidad.DataSource = null;
                DataTable dt = (DataTable)v.getData("Select t1.idunidad as id, concat(t1.consecutivo,'-', t1.descripcioneco) as unidad from cunidades as t1 where t1.descripcioneco like '%travelers%'");
                DataRow nuevaFila = dt.NewRow();
                nuevaFila["id"] = 0;
                nuevaFila["unidad"] = "--SELECCIONE UNIDAD--".ToUpper();
                dt.Rows.InsertAt(nuevaFila, 0);
                cmbUnidad.DisplayMember = "id";
                cmbUnidad.ValueMember = "unidad";
                cmbUnidad.DataSource = dt;

                cmbUnidad.Enabled = true;
            }

        }

        private void txtMecanico2_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtMecanico2.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanicoApoyo = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblmapoyo.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtMecanico2, lblmapoyo);
        }

        private void txtMeca_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtMeca.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanico = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblMecanicoU.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtMeca, lblMecanicoU);
        }

        private void txtMeca_TextChanged(object sender, EventArgs e)
        {
            if (txtMeca.Focused)
                txtMeca_Validated(sender, e);
            if (peditar & editar)
                pguardar.Visible = (cambios() && !finaliza() ? true : false);
            if (pinsertar && EstatusAnterior != 3)
                pfinalizar.Visible = (finaliza() ? true : false);
            ubica();
        }

        private void txtMeca2_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtMeca2.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanicoApoyo = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblMeca2.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtMeca2, lblMeca2);
        }

        private void btnregresar_Click(object sender, EventArgs e)
        {
            gbrefacciones.Visible = !(gbxDiag.Visible = true);
            cmbRefacciones1.Enabled = (Convert.ToInt32(v.getaData("select count(*)  from pedidosrefaccion where " + v.c.fieldspedidosrefaccion[1] + "='" + idreporte + "';")) == 0 ? true : false);
            limpiarRefaccion();
            pguardar.Visible = true;
            pRetorno.Visible = false;
            ubica();
        }

        void cargarrefacciones()
        {
            MySqlDataAdapter r = new MySqlDataAdapter("select t1." + v.c.fieldspedidosrefaccion[0] + ", t1." + v.c.fieldspedidosrefaccion[2] + " as 'NÚMERO',upper(t2." + v.c.fieldscrefacciones[2] + ") as 'REFACCIÓN',t1." + v.c.fieldspedidosrefaccion[7] + " as 'CANTIDAD', if(t2.existencias >= Cantidad, 'EXISTENCIA','SIN EXISTENCIA') as 'EXISTENCIA',t1." + v.c.fieldspedidosrefaccion[9] + " as 'CANTIDAD ENTREGADA',UPPER((select if(envio='0', 'ENVIA', if(seen='0', 'Sin Lectura', if(AutorizaAlmacen ='0', 'Evaluando', if(AutorizaAlmacen ='1', 'Correcto', 'Incorrecto')))) from refacciones_standby as x1 where t1.idpedRef = x1.refaccionfkpedidosRefaccion)) as 'ESTATUS RETORNO' from pedidosrefaccion as t1 inner join crefacciones as t2 on t1." + v.c.fieldspedidosrefaccion[3] + "=t2." + v.c.fieldscrefacciones[0] + " where " + v.c.fieldspedidosrefaccion[1] + "='" + idreporte + "' ORDER BY t1.NumRefacc asc;", v.c.dbconection());
            DataSet ds = new DataSet();
            r.Fill(ds);
            dgvrefacciones.DataSource = ds.Tables[0];
            dgvrefacciones.Columns[0].Visible = false;
            v.c.dbconection().Close();
            dgvrefacciones.ClearSelection();
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            //AGREGA REFACCIONES
            if (!editarRefaccion)
            {
                int n = Convert.ToInt32(v.getaData("select count(" + v.c.fieldspedidosrefaccion[2] + ") from pedidosrefaccion where " + v.c.fieldspedidosrefaccion[1] + "='" + idreporte + "';"));
                n++;
                if (v.validarefacicion(Convert.ToInt32(cmbrefaccion.SelectedValue), txtcantidad.Text))
                    if (v.c.insertar("insert into pedidosrefaccion (" + v.c.fieldspedidosrefaccion[1] + "," + v.c.fieldspedidosrefaccion[2] + "," + v.c.fieldspedidosrefaccion[3] + "," + v.c.fieldspedidosrefaccion[4] + "," + v.c.fieldspedidosrefaccion[7] + "," + v.c.fieldspedidosrefaccion[10] + ") values('" + idreporte + "','" + n + "','" + cmbrefaccion.SelectedValue + "',now(),'" + txtcantidad.Text + "','" + idUsuario + "')"))
                    {
                        entregaRefacciones = 1;
                        string idRefk = v.getaData("select concat(idPedRef,';') as id from pedidosrefaccion as t1 where FolioPedfkSupervicion='" + idreporte + "' order by idPedRef desc limit 1").ToString();
                        RefaccionesSolicitadas += idRefk;
                        ArraryInformativo = separarRepetidos();
                        string cadenaK = Convert.ToInt32(cmbrefaccion.SelectedValue) + ";" + txtcantidad.Text;
                        Modificacion_Crear(idRefk.Replace(";", ""), "", cadenaK, "Inserción de Refacción en Reporte de Mantenimiento");

                        MessageBox.Show("Refacción agregada de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarRefaccion();
                    }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtcantidad.Text) && cmbrefaccion.SelectedIndex > 0)
                {
                    if (v.c.insertar("update pedidosrefaccion set " + v.c.fieldspedidosrefaccion[3] + "='" + cmbrefaccion.SelectedValue + "', " + v.c.fieldspedidosrefaccion[7] + "='" + txtcantidad.Text + "' where " + v.c.fieldspedidosrefaccion[0] + "='" + idref + "';"))
                    {
                        entregaRefacciones = 1;
                        string idRefk = v.getaData("select concat(idPedRef,';') as id from pedidosrefaccion as t1 where FolioPedfkSupervicion='" + idreporte + "'").ToString();
                        RefaccionesSolicitadas += idRefk;
                        ArraryInformativo = separarRepetidos();
                        string cadenaK = Convert.ToInt32(cmbrefaccion.SelectedValue) + ";" + txtcantidad.Text;
                        Modificacion_Crear(idRefk.Replace(";", ""), "", cadenaK, "Actualización de Refacción en Reporte de Mantenimiento");
                        MessageBox.Show("Los datos de actualizaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiarRefaccion();
                    }
                }
                else
                {
                    MessageBox.Show("Complete sus datos", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtcantidad_TextChanged(object sender, EventArgs e)
        {
            if (editarRefaccion && peditar)
            {
                bool qh = ((idrefaccionAnterior != Convert.ToInt32(cmbrefaccion.SelectedValue) || cantidadAnterior != Convert.ToInt32((string.IsNullOrWhiteSpace(txtcantidad.Text) ? "0" : txtcantidad.Text)) && cmbrefaccion.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtcantidad.Text)) ? true : false);
                pagregar.Visible = qh;
                txtRetorno.Enabled = txtObsRetorno.Enabled = (qh == true ? false : true);
                if (qh == true)
                {
                    txtObsRetorno.Text = txtRetorno.Text = "";
                    pRetorno.Visible = false;
                }
            }
        }

        private void cmbrefaccion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        string[] separarRepetidos()
        {
            return RefaccionesSolicitadas.Split(';').Distinct().ToArray();
        }
        /**********************************************************************************************************************/
        void Modificacion_Crear(string folio, string porquemodificacion, string ultimaModificacion, string tipo)
        {
            string vark = (tipo == "Actualización de Refacción en Reporte de Mantenimiento" || tipo == "Inserción de Refacción en Reporte de Mantenimiento" ? folio : "(select IdReporte from reportesupervicion as t1 inner join reportemantenimiento as t2 on t1.idReporteSupervicion = t2.FoliofkSupervicion where t2.FoliofkSupervicion='" + folio + "')");
            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area, motivoActualizacion) VALUES('Reporte de Mantenimiento', " + vark + ",'" + ultimaModificacion + "','" + idUsuario + "',NOW(),'" + tipo + "','" + empresa + "','" + area + "', '" + porquemodificacion + "')");
        }

        private void btnRetorno_Click(object sender, EventArgs e)
        {
            //retorna refacciones provisionalmente, hasta que almacen corrobore
            //refaccionfkpedidosRefaccion, cantEntregada, cantRetorno, FechaHoraSR, ObservacionMant, usuarioSR, AutorizaAlmacen, FechahoraR, ObservacionAlm, usuarioR
            if (!string.IsNullOrWhiteSpace(txtcantidad.Text) && !string.IsNullOrWhiteSpace(idrefaccionAnterior.ToString()) && idrefaccionAnterior > 0 && !string.IsNullOrWhiteSpace(idref.ToString()) && idref > 0)
            {
                string cadena = "";
                FormContraFinal o = new FormContraFinal(empresa, area, this, v, "1");
                o.Owner = this;
                if (o.ShowDialog() == DialogResult.OK)
                {
                    string IdGuardo = o.id;
                    cadena = (Convert.ToInt32(v.getaData("select count(*) from refacciones_standby where refaccionfkpedidosrefaccion='" + idref + "'").ToString()) > 0) ? "update refacciones_standby set cantRetorno='" + txtcantidad.Text + "', ObservacionMant='" + txtObsRetorno.Text + "', usuarioSR='" + IdGuardo + "' where idStanby='" + v.getaData("select idStanby from refacciones_standby where refaccionfkpedidosrefaccion='" + idref + "'").ToString() + "'" : "insert into refacciones_standby(refaccionfkpedidosRefaccion, cantEntregada, cantRetorno, FechaHoraSR, ObservacionMant, usuarioSR) values('" + idref + "', '" + txtcantidad.Text + "', '" + txtRetorno.Text + "', now(),'" + txtObsRetorno.Text + "', '" + IdGuardo + "');";
                    if (!string.IsNullOrWhiteSpace(cadena) && !string.IsNullOrWhiteSpace(IdGuardo))
                    {
                        if (v.c.insertar(cadena))
                        {
                            retornos = 1;
                            EnviarAlmacen = true;
                            limpiarRefaccion();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Datos Faltantes, complete la información", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvrefacciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dgvrefacciones_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string[] drefaccion = v.getaData("select concat(" + v.c.fieldspedidosrefaccion[3] + ",'|'," + v.c.fieldspedidosrefaccion[7] + ") from pedidosrefaccion where " + v.c.fieldspedidosrefaccion[0] + "='" + (idref = Convert.ToInt32(dgvrefacciones.Rows[e.RowIndex].Cells[0].Value)) + "';").ToString().Split('|');
                if (Convert.ToInt32(v.getaData("select if(" + v.c.fieldspedidosrefaccion[9] + "=" + v.c.fieldspedidosrefaccion[7] + ",1,0) as r from pedidosrefaccion where " + v.c.fieldspedidosrefaccion[0] + "='" + idref + "';")) == 0)
                {
                    cmbrefaccion.SelectedValue = idrefaccionAnterior = Convert.ToInt32(drefaccion[0]);
                    txtcantidad.Text = (cantidadAnterior = Convert.ToInt32(drefaccion[1])).ToString();
                    cmbrefaccion.Enabled = true;
                    muestraRetornos(false);
                    txtcantidad.Enabled = true;
                    pagregar.Visible = !(editarRefaccion = true);

                }
                else
                {
                    MessageBox.Show("La refacción no puede ser editada debido a que ya fue entregada por el área de almacén", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (dgvrefacciones.Rows[e.RowIndex].Cells[6].Value.ToString() != "CORRECTO")
                    {
                        pagregar.Visible = false;
                        cmbrefaccion.SelectedValue = idrefaccionAnterior = Convert.ToInt32(drefaccion[0]);
                        txtcantidad.Text = (cantidadAnterior = Convert.ToInt32(drefaccion[1])).ToString();
                        cmbrefaccion.Enabled = false;
                        muestraRetornos(true);
                    }
                    else
                    {
                        idref = 0; editarRefaccion = false;
                        label60.Visible = label67.Visible = label65.Visible = txtRetorno.Visible = txtObsRetorno.Visible = pRetorno.Visible = btnRetorno.Visible = false;
                        txtcantidad.Enabled = true;
                    }
                }
                txtObsRetorno.Text = txtRetorno.Text = ""; txtRetorno.Enabled = txtObsRetorno.Enabled = true;
            }
        }

        void muestraRetornos(bool q)
        {
            label60.Visible = label67.Visible = label65.Visible = txtRetorno.Visible = txtObsRetorno.Visible = pRetorno.Visible = btnRetorno.Visible = q == true ? true : false;
            txtcantidad.Enabled = q == true ? false : false;
        }

        void limpiarRefaccion()
        {
            cmbrefaccion.SelectedIndex = 0;
            editarRefaccion = false;
            txtcantidad.Enabled = cmbrefaccion.Enabled = true;
            txtcantidad.Clear();
            lblum.Text = txtRetorno.Text = txtObsRetorno.Text = txtCodigoRef.Text = "";
            cargarrefacciones();
        }

        private void txtmecanico_TextChanged(object sender, EventArgs e)
        {
            if (txtmecanico.Focused)
                txtmecanico_Validated(sender, e);
            if (peditar & editar)
                pguardar.Visible = (cambios() && !finaliza() ? true : false);
            if (pinsertar && EstatusAnterior != 3)
                pfinalizar.Visible = (finaliza() ? true : false);
            ubica();
        }

        private void txtmecanico_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtmecanico.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanico = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblmecanico.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtmecanico, lblmecanico);
        }

        void mecanicosiguales(TextBox txt, Label lbl)
        {
            if (((idmecanico > 0 && (idmecanicoApoyo > 0 || idmecanico > 0)) || (idmecanicoApoyo > 0 && idmecanico > 0)) && (idmecanicoApoyo == idmecanico || idmecanico == idmecanicoApoyo || idmecanico == idmecanicoApoyo))
            {
                MessageBox.Show("El" + (idmecanico == idmecanicoApoyo ? " el mecánico principal y mecánico de apoyo" : idmecanico == idmecanicoApoyo ? " el mecánico principal y mecanico apoyo" : idmecanicoApoyo == idmecanico ? " mecanico de apoyo y el mecánico principal " : "") + " no pueden ser la misma persona", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt.Clear();
                lbl.Text = "";
            }
        }

        /*PRUEBA CONEXION A BASE DE DATOS*/

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            button2.Visible = true;
        }

        private void btnrefacciones_Click(object sender, EventArgs e)
        {
            gbxDiag.Visible = !(gbrefacciones.Visible = true);
            v.iniCombos("select " + v.c.fieldscrefacciones[0] + " as id,upper(" + v.c.fieldscrefacciones[2] + ") as nombre from crefacciones where " + v.c.fieldscrefacciones[13] + "='1' and empresa='" + empresa + "' order by nombre asc;", cmbrefaccion, "id", "nombre", "--seleccione--");

            pguardar.Visible = false;
            ubica();
        }

        bool cambios()
        {
            bool res = false;
            if ((idmecaniAnterior != idmecanico || idmecaniAnterior != idmecanico || idmecanicoApoyo != idmecanicoapoyoAnterior || trabajoAnterior != txtRepaReal.Text.Trim() || (observacionesAnterior ?? "") != txtFallos.Text.Trim() || EstatusAnterior != Convert.ToInt32(cmbEstRep.SelectedValue) || folioAnterior != txtfoliof.Text) && idreporte > 0 || (LBxRefacc.Items.Count > conteoListFolios) || retornos != 0 || cambiosEnFolios == true)
                res = true;
            return res;
        }

        bool finaliza()
        {
            bool res = false;
            if (Convert.ToInt32(cmbEstRep.SelectedValue) == 3 && idmecanico > 0 && !string.IsNullOrWhiteSpace(txtRepaReal.Text) && ((cmbRefacciones1.SelectedIndex == 2) || (Convert.ToInt32(cmbRefacciones1.SelectedIndex) == 1)) && cmbEstRep.SelectedIndex > 0)
            {
                if (verificaantes())
                {
                    res = true;
                }
            }
            return res;
        }

        public bool verificaantes()
        {
            if (Convert.ToInt32(cmbRefacciones1.SelectedIndex) == 1)
            {
                if (LBxRefacc.Items.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private void cmbRefacciones1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRefacciones1.SelectedIndex > 0 && EstatusAnterior < 3)
                txtfoliof.Enabled = btnrefacciones.Visible = (Convert.ToInt32(cmbRefacciones1.SelectedValue) == 1 ? true : false);
            txtfoliof.Enabled = numUpDownDE.Enabled = numUpDownHASTA.Enabled = LBxRefacc.Enabled = btnFolioFactura.Enabled = btnCancelFact.Enabled = (cmbRefacciones1.SelectedIndex == 1) ? true : false;
        }

        /*/////////////////////PRUEBAS//////////////////*/
        private void ReporteUnidadesExternas_Load(object sender, EventArgs e)
        {
            combo();
            busqueda();
            unidad();
           // unidad2();
        }

        /* void pdf()
         {
             Document doc = new Document(PageSize.LETTER);
             doc.SetMargins(20f, 20f, 10f, 10f);
             SaveFileDialog saveFileDialog1 = new SaveFileDialog();
             saveFileDialog1.InitialDirectory = "@C:";
             saveFileDialog1.Title = "Guardar Reporte";
             saveFileDialog1.AddExtension = true;
             saveFileDialog1.DefaultExt = "*.pdf";
             saveFileDialog1.Filter = "Archivos PDF(*.pdf)|*.pdf";
             saveFileDialog1.FilterIndex = 1;
             saveFileDialog1.RestoreDirectory = true;
             string filename = "";
             try
             {
                 if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                 {
                     filename = saveFileDialog1.FileName;
                     string p = Path.GetExtension(filename);
                     if (p.ToLower() != ".pdf")
                         filename = filename + ".pdf";
                     while (filename.ToLower().Contains(".pdf.pdf"))
                         filename = filename.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
                 }
                 if (filename.Trim() != "")
                 {
                     FileStream file = new FileStream(filename,
                 FileMode.Create,
                 FileAccess.ReadWrite,
                 FileShare.ReadWrite);
                     PdfWriter.GetInstance(doc, file);
                     doc.Open();
                     byte[] logo = Convert.FromBase64String((empresa == 2 ? v.tri : v.TSD));
                     iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(logo);
                     imagen.ScalePercent(20f);
                     imagen.SetAbsolutePosition(460f, 720f);
                     imagen.Alignment = Element.ALIGN_RIGHT;
                     doc.Add(new Phrase(new Chunk((rbngeneral.Checked ? "\n\n INFORME DE MANTENIMIENTO" : "\n\n INFORME DE UNIDAD"), FontFactory.GetFont("calibri", 17, iTextSharp.text.Font.BOLD))));
                     doc.Add(imagen);
                     doc.Add(new Phrase("\n"));
                     doc.Add((rbngeneral.Checked ? general() : unidad()));
                     doc.Add(new Phrase("\n", arial));
                     doc.Add(new Phrase((dgvrefacciones.Rows.Count > 0 ? "REFACCIONES SOLICITADAS" : "NO SE REQUIEREN REFACCIONES"), encabezados));
                     if (dgvrefacciones.Rows.Count > 0)
                         Refacciones(doc);
                     doc.Close();
                     System.Diagnostics.Process.Start(filename);
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
         }*/

        void busqueda()
        {
            /* v.iniCombos("SELECT t1." + v.c.fieldscunidades[0] + ",concat(t2." + v.c.fieldscareas[2] + ",LPAD(" + v.c.fieldscunidades[1] + ",4,'0')) as ECo FROM cunidades as t1 INNER JOIN careas as t2 ON t1." + v.c.fieldscunidades[3] + "= t2." + v.c.fieldscareas[0] + " order by eco;", cmbUnidad1, "idunidad", "ECo", "--SELECCIONE UNIDAD--");*/

            v.iniCombos("select t1." + v.c.fieldscpersonal[0] + " as id, upper(concat(coalesce(t1." + v.c.fieldscpersonal[2] + ",''),' ',coalesce(t1." + v.c.fieldscpersonal[3] + ",''),' ',t1." + v.c.fieldscpersonal[4] + ")) as nombre from cpersonal as t1 inner join puestos as t2 on t2." + v.c.fieldspuestos[0] + "=t1." + v.c.fieldscpersonal[5] + " where t2." + v.c.fieldspuestos[1] + " like '%Mecánico%'", cmbMecanicob1, "id", "nombre", "--SELECCIONE UN MECÁNICO--");
            
        }
        public void unidad()
        {
            cmbUnidad1.DataSource = null;
            DataTable dt = (DataTable)v.getData("Select t1.idunidad as id, concat(t1.consecutivo,'-', t1.descripcioneco) as unidad from cunidades as t1 where t1.descripcioneco like '%Cometa%'  or t1.descripcioneco like '%browser%' or t1.descripcioneco like '%travelers%'  or t1.descripcioneco like '%nezahualpilli%' or t1.descripcioneco like '%atreyo tours%'");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["unidad"] = "--SELECCIONE UNIDAD--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbUnidad1.DisplayMember = "id";
            cmbUnidad1.ValueMember = "unidad";
            cmbUnidad1.DataSource = dt;
        }
        public void unidad2()
        {
            cmbUnidad.DataSource = null;
            DataTable dt = (DataTable)v.getData("Select t1.idunidad as id, concat(t1.consecutivo,'-', t1.descripcioneco) as unidad from cunidades as t1 where t1.descripcioneco like '%Cometa%'  or t1.descripcioneco like '%browser%' or t1.descripcioneco like '%travelers%'  or t1.descripcioneco like '%nezahualpilli%' or t1.descripcioneco like '%atreyo tours %'"); 
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["unidad"] = "--SELECCIONE UNIDAD--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbUnidad.DisplayMember = "id";
            cmbUnidad.ValueMember = "unidad";
            cmbUnidad.DataSource = dt;
        }
    }
}
/*ACTUALIZACION 26-06-2022 REPORTE UNIDADES EXTERNAS*/
