
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
       
        
        validaciones v;
        conexion con;

        
        int idUsuario, empresa, area, idmecanico, idmecanicoApoyo, idmecaniAnterior, idmecanicoapoyoAnterior, EstatusAnterior, idreporte;
        bool editarRefaccion;

        public ReporteUnidadesExternas(int idUsuario, int empresa, int area, validaciones v)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.v = v;

            DateTime fecha = DateTime.Now;
            tbxHoraEnvio.Text = fecha.ToString();

        }

      
        private void button1_Click(object sender, EventArgs e)
        {

            gbxbusqueda.Visible = true;
            gbxDiag.Visible = true;
            gbxUnidad.Visible = true;
            gbxAlertas.Visible = true;
            pguardar.Visible = true;
            pcancelar.Visible = true;
            pfinalizar.Visible = true;

            pictureBox1.Visible = false;
            lblText.Visible = false;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            gbxbusqueda.Visible = false;
            gbxDiag.Visible = false;
            gbxUnidad.Visible = false;
            gbxAlertas.Visible = false;

            pictureBox1.Visible = true;
            lblText.Visible = true;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
            button2.Visible = true;
        }

        /************************************************************************************************************************************************************************************************************************/

        /*/////////////////////INICIO PROGRAMA////////////////////*/

        //CRAGA AUTOMATICA AL INICIAR INTERFAZ
        private void ReporteUnidadesExternas_Load(object sender, EventArgs e)
        {
            combo();
            Genera_Folio();
        }

        //METODO GARGA DE COMBOS
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
            v.comboswithuot(cmbTipoR, new string[] { "--seleccione el tipo--", "preventivo", "correctivo", "reiterativo" });
            v.comboswithuot(cmbReTip, new string[] { "--seleccione el tipo--", "preventivo", "correctivo", "reiterativo" });
            //eststus reparacion
            v.comboswithuot(cmbEstRep, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
            v.comboswithuot(EstatusRepa, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada", });
            //empresa
            v.comboswithuot(cbxEmpresaS, new string[] { "--seleccione un tipo--", "atreyo tour", "browser", "cometa de oro", "nezahualpilli", "travelers" });
            v.comboswithuot(cbxSempresa, new string[] { "--seleccione un tipo--", "atreyo tour", "browser", "cometa de oro", "nezahualpilli", "travelers" });
            //unidades
            v.comboswithuot(cmbUnidad, new string[] { "--seleccione unidad--" });
        }

        //PINTAR COMBOS
        private void pintarcombos(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
        private void cmbEstatus1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }
        private void cbxSempresa_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }
        private void cmbUnidad_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }
        private void cmbTipoR_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }
        private void cmbEstRep_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }
        private void cmbRefacciones1_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }
        private void cmbrefaccion_DrawItem(object sender, DrawItemEventArgs e)
        {
            pintarcombos(sender, e);
        }

        //SELECCION DE UNIDAD-COMBO TIPO UNIDAD
        private void cbxSempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSempresa.Text == "--SELECCIONE UN TIPO--")
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

        //VALIDACION SOLO LETRAS Y NUMEROS 
        private void txtMeca_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloLetrasNum(sender,e);
        }
        private void txtMeca2_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloLetrasNum(sender, e);
        }
        private void txtFallos_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloLetrasNum2(sender, e);
        }
        private void txtmecanico_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloLetrasNum(sender, e);
        }
        private void txtMecanico2_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloLetrasNum(sender, e);
        }
        private void txtDiagMeca_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloLetrasNum2(sender, e);
        }

        //VALIDACION SOLO NUMEROS
        private void tbxKilome_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(sender, e);
        }

        //VALIDACION SOLO LETRAS
        private void tbxPersonaIngreso_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloLetras(sender, e);
        }

        //METODO VALIDACION SOLO NUMEROS
        void soloNumeros(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 33 && e.KeyChar <= 47 || e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permiten NUMEROS en este Campo", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        //METODO VALIDACION SOLO LETRAS    
        void soloLetras(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 33 && e.KeyChar <= 64 || e.KeyChar >= 91 && e.KeyChar <= 96 || e.KeyChar >= 123 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permiten LETRAS en este Campo", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        //METODO VALIDACION SOLO LETRAS Y NUMEROS    
        void soloLetrasNum(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47 || e.KeyChar >= 58 && e.KeyChar <= 64 || e.KeyChar >= 91 && e.KeyChar <= 96 || e.KeyChar >= 123 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permiten LETRAS Y NUMEROS en este Campo", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        //METODO VALIDACION SOLO LETRAS Y NUMEROS    
        void soloLetrasNum2(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 33 && e.KeyChar <= 47 || e.KeyChar >= 58 && e.KeyChar <= 64 || e.KeyChar >= 91 && e.KeyChar <= 96 || e.KeyChar >= 123 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se permiten LETRAS Y NUMEROS en este Campo", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        //BOTON SIGUIENTE (DESBLOQUEO GROUP BOX DIAGNOSTICO)
        private void btnSig_Click(object sender, EventArgs e)
        {
            BorrarMensajeError();

            if (ValidarCampos())
            {
                MessageBox.Show("Datos Ingresados Correctamente");

                gbxDiag.Enabled = true;
                DateTime fecha1 = DateTime.Now;
                txtIniDiag.Text = fecha1.ToString();
                gbxUnidad.Enabled = false;               
            }
        }

        //BOTON SIGUIENTE (DIAGNOSTICO)
        private void btnRepa_Click(object sender, EventArgs e)
        {
            BorrarMensajeError2();

            if (ValidarCampos2())
            {
                MessageBox.Show("Datos Ingresados Correctamente");
                
                cmbRefacciones1.Enabled = true;
                btnrefacciones.Enabled = true;
                txtRepaReal.Enabled = true;
                txtfoliof.Enabled = true;
                numUpDownDE.Enabled = true;
                numUpDownHASTA.Enabled = true;
                btnFolioFactura.Enabled = true;
                btnCancelFact.Enabled = true;

                txtmecanico.Enabled = false;
                txtMecanico2.Enabled = false;
                txtDiagMeca.Enabled = false;
                cmbTipoR.Enabled = false;
                cmbEstRep.Enabled = false;
            }
        }

        //GENERACION DE FOLIO AUTOMATICO
        void Genera_Folio()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(SUBSTRING(folioR,LENGTH(FOLIOR)-6,7)+1)AS Folio from reporteuniexternas WHERE idRUEX = (SELECT MAX(idRUEX) FROM reporteuniexternas);", v.c.dbconection());
            string Folio = (string)cmd.ExecuteScalar();
            if (Folio == null)
                Folio = "0000001";
            else
                while (Folio.Length < 7)
                    Folio = "0" + Folio;
            tbxFolio.Text = "RUEX-" + Folio.ToString();
            v.c.dbconection().Close();
        }

        //METODO VALIDACION CAMPOS VACIOS DATOS DE LA UNIDAD
        private bool ValidarCampos()
        {
            bool ok = true;

            //VALIDACIONES TEXBOX DATOS DE LA UNIDAD
            if (tbxPersonaIngreso.Text == "")
            {
                ok = false;
                errorProvider1.SetError(tbxPersonaIngreso,"Ingrese Nombre que Ingresa la Unidad");
            }

            if (tbxKilome.Text == "")
            {
                ok = false;
                errorProvider1.SetError(tbxKilome, "Ingrese el Kilometraje de la Unidad");
            }

            if (txtMeca.Text == "")
            {
                ok = false;
                errorProvider1.SetError(txtMeca, "Ingrese la Contraseña del Mecanico");
            }

            if (txtFallos.Text == "")
            {
                ok = false;
                errorProvider1.SetError(txtFallos, "Ingrese los Fallos Reportados");
            }      

            //VALIDACIONES COMBOBOX DATOS DE LA UNIDAD
            if (cbxSempresa.Text == "--SELECCIONE UN TIPO--")
            {
                ok = false;
                errorProvider1.SetError(cbxSempresa, "Seleccione el Tipo de la Unidad");
            }

            if (cmbUnidad.Text == "--SELECCIONE UNIDAD--")
            {
                ok = false;
                errorProvider1.SetError(cmbUnidad, "Seleccione una Unidad");
            }

            if (cmbEstatus1.Text == "--SELECCIONE UN ESTATUS--")
            {
                ok = false;
                errorProvider1.SetError(cmbEstatus1, "Seleccione un Estatus de Diagnostico");
            }
                 
            return ok;
        }

        //METODO VALIDACION CAMPOS VACIOS DIAGNOSTICO
        private bool ValidarCampos2()
        {
            bool ok = true;

            //VALIDACIONES TEXTBOX DIAGNOSTICO
            if (txtmecanico.Text == "")
            {
                ok = false;
                errorProvider1.SetError(txtmecanico, "Ingrese la Contraseña del Mecanico");
            }

            if (txtDiagMeca.Text == "")
            {
                ok = false;
                errorProvider1.SetError(txtDiagMeca, "Ingrese el Diagnostico Correspondiente");
            }

            //VALIDADCIONES COMBOBOX DIAGNOSTICO
            if (cmbTipoR.Text == "--SELECCIONE EL TIPO--")
            {
                ok = false;
                errorProvider1.SetError(cmbTipoR, "Seleccione Tipo de Reparacion");
            }

            if (cmbEstRep.Text == "--SELECCIONE UN ESTATUS--")
            {
                ok = false;
                errorProvider1.SetError(cmbEstRep, "Seleccione un Estatus de Reparacion");
            }

            return ok;
        }

        //BORRAR ERRORPROVIDER CAMPOS VALIDACION DATOS DE LA UNIDAD
        private void BorrarMensajeError()
        {
            //TEXTBOX
            errorProvider1.SetError(tbxPersonaIngreso,"");
            errorProvider1.SetError(tbxKilome, "");
            errorProvider1.SetError(txtMeca, "");
            errorProvider1.SetError(txtFallos, "");

            //COMBOBOX
            errorProvider1.SetError(cbxSempresa,"");
            errorProvider1.SetError(cmbUnidad, "");
            errorProvider1.SetError(cmbEstatus1, "");
        }

        //BORRAR ERRORPROVIDER CAMPOS VALIDACION DIAGNOSTICO 
        private void BorrarMensajeError2()
        {
            //TEXTBOX
            errorProvider1.SetError(txtmecanico, "");
            errorProvider1.SetError(txtDiagMeca, "");

            //COMBOBOX
            errorProvider1.SetError(cmbTipoR, "");
            errorProvider1.SetError(cmbEstRep, "");
        }

        //OBTENER MECANICO CON CONTRASEÑA (DATOS DE LA UNIDAD)
        private void txtMeca_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtMeca.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanico = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblMecanicoU.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtMeca, lblMecanicoU);
        }

        private void txtMeca2_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtMeca2.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanicoApoyo = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblMeca2.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtMeca2, lblMeca2);
        }

        //METODO VALIDADCION MECANICOS IGUALES (DATOS DE LA UNIDAD)
        void mecanicosiguales(TextBox txt, Label lbl)
        {
            if (((idmecanico > 0 && (idmecanicoApoyo > 0 || idmecanico > 0)) || (idmecanicoApoyo > 0 && idmecanico > 0)) && (idmecanicoApoyo == idmecanico || idmecanico == idmecanicoApoyo || idmecanico == idmecanicoApoyo))
            {
                MessageBox.Show("El" + (idmecanico == idmecanicoApoyo ? " el mecánico principal y mecánico de apoyo" : idmecanico == idmecanicoApoyo ? " el mecánico principal y mecanico apoyo" : idmecanicoApoyo == idmecanico ? " mecanico de apoyo y el mecánico principal " : "") + " no pueden ser la misma persona", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt.Clear();
                lbl.Text = "";
            }
        }

        //OBTENER MECANICO CON CONTRASEÑA (DIAGNOSTICO)
        private void txtmecanico_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtmecanico.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanico = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblmecanico.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtmecanico, lblmecanico);
        }

        private void txtMecanico2_Validated(object sender, EventArgs e)
        {
            string[] valoreslk = Convert.ToString(v.getaData("SELECT UPPER(CONCAT(coalesce(t1.idPersona,''), '|', coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,''))) AS Nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal INNER JOIN puestos as t3 On t1.cargofkcargos = t3.idpuesto WHERE t2.password = '" + v.Encriptar(txtMecanico2.Text.Trim()) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'")).Split('|');
            idmecanicoApoyo = (Convert.ToInt32(valoreslk.Length) > 1) ? Convert.ToInt32(valoreslk[0]) : 0;
            lblmapoyo.Text = (Convert.ToInt32(valoreslk.Length) > 1) ? valoreslk[1] : "";
            mecanicosiguales(txtMecanico2, lblmapoyo);
        }

        //COMBO REFACCIONES 
        private void cmbRefacciones1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRefacciones1.SelectedIndex > 0 && EstatusAnterior < 3)
                txtfoliof.Enabled = btnrefacciones.Visible = (Convert.ToInt32(cmbRefacciones1.SelectedValue) == 1 ? true : false);
            txtfoliof.Enabled = numUpDownDE.Enabled = numUpDownHASTA.Enabled = LBxRefacc.Enabled = btnFolioFactura.Enabled = btnCancelFact.Enabled = (cmbRefacciones1.SelectedIndex == 1) ? true : false;
        }

        //BOTON AGREGAR REFACCIONES
        private void btnrefacciones_Click(object sender, EventArgs e)
        {
            gbxDiag.Visible = !(gbrefacciones.Visible = true);
            v.iniCombos("select " + v.c.fieldscrefacciones[0] + " as id,upper(" + v.c.fieldscrefacciones[2] + ") as nombre from crefacciones where " + v.c.fieldscrefacciones[13] + "='1' and empresa='" + empresa + "' order by nombre asc;", cmbrefaccion, "id", "nombre", "--seleccione--");

            pguardar.Visible = false;
            ubica();
        }

        //METODO UBICACION BOTONES
        public void ubica()
        {
            pPdf.Location = new Point(gbxbusqueda.Location.X + gbxbusqueda.Width + 10, gbxbusqueda.Location.Y); pguardar.Location = new Point(pPdf.Location.X + pPdf.Width + 10, gbxbusqueda.Location.Y); pfinalizar.Location = new Point(pguardar.Location.X + pguardar.Width + 10, gbxbusqueda.Location.Y); pcancelar.Location = new Point(pfinalizar.Location.X + pfinalizar.Width + 10, gbxbusqueda.Location.Y);
            gbrefacciones.Location = new Point(gbxDiag.Location.X, 11);
        }

        //BOTON REGRESAR DIAGNOSTICO
        private void btnregresar_Click(object sender, EventArgs e)
        {
            gbrefacciones.Visible = !(gbxDiag.Visible = true);
            cmbRefacciones1.Enabled = (Convert.ToInt32(v.getaData("select count(*)  from pedidosrefaccion where " + v.c.fieldspedidosrefaccion[1] + "='" + idreporte + "';")) == 0 ? true : false);
            limpiarRefaccion();
            pguardar.Visible = true;
            pRetorno.Visible = false;
            ubica();
        }

        //METODO PARA LIMPIAR REFACCION
        void limpiarRefaccion()
        {
            cmbrefaccion.SelectedIndex = 0;
            editarRefaccion = false;
            txtcantidad.Enabled = cmbrefaccion.Enabled = true;
            txtcantidad.Clear();
            lblum.Text = txtRetorno.Text = txtObsRetorno.Text = txtCodigoRef.Text = "";
            cargarrefacciones();
        }

        //METODO PARA CARGAR REFACCIONES
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

        //BUSCAR REFACCION POR CODIGO
        private void txtCodigoRef_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                buscaref(txtCodigoRef.Text);
            }
        }

        //METODO BUSCAR REFACCIONES
        public void buscaref(string codigo)
        {
            //string[] cadenaR = v.getaData("SET lc_time_names = 'es_ES';SELECT concat(convert(t1.nombreRefaccion,char), '|', convert(t1.idrefaccion,char)) as id from crefacciones as t1  where t1.codrefaccion = '" + codigo + "' and t1.empresa  = '" + empresa + "' and t1.existencias > 0").ToString().Split('|');
            string[] cadenaR = v.getaData("SET lc_time_names = 'es_ES';SELECT concat(if(count(convert(t1.nombreRefaccion,char))>0,convert(t1.nombreRefaccion,char),'0'), '|',if(count(convert(t1.idrefaccion,char))>0,convert(t1.idrefaccion,char),'0')) as id from crefacciones as t1  where t1.codrefaccion = '" + codigo + "' and t1.empresa  = '" + empresa + "' and t1.existencias > 0").ToString().Split('|');
            if (cadenaR[0].ToString().Equals("").Equals("0"))
            {
                MessageBox.Show("No se encontro la refaccion y/o No hay en existencia".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCodigoRef.Text = "";
                cmbrefaccion.Enabled = false;
            }
            else
            {
                cmbrefaccion.SelectedValue = int.Parse(cadenaR[1].ToString());

            }
        }

        //GUARDAR REGISTRO
        private void btnguardar_Click(object sender, EventArgs e)
        {
            agregar();
        }

        //METODO AGREGAR REGSITRO
        void agregar()
        {           
                v.c.insertar("insert into reporteuniexternas(folioR,personaIngreso,mecanicoD,fallosRepor,mecanicoR,diagnosticoMeca,folioFact,reparacionesRa,empresaU,unidad,estatusDiag,tipoRepa,estatusRepa,refacciones) values ('" + tbxFolio.Text + "','" + tbxPersonaIngreso.Text + "','" + lblMecanicoU.Text + "','" + txtFallos.Text + "','" + lblmecanico.Text + "','" + txtDiagMeca.Text + "','" + txtfoliof.Text + "','" + txtRepaReal.Text + "','" + cbxSempresa.SelectedValue + "','" + cmbUnidad.SelectedValue + "','" + cmbEstatus1.SelectedValue + "','" + cmbTipoR.SelectedValue + "','" + cmbEstRep.SelectedValue + "','" + cmbRefacciones1.SelectedValue + "')");
                MessageBox.Show("Reporte Guardado con Exito");
                //Limpiar();
           
        }



    }
}
/*ACTUALIZACION 18 -07-2022 REPORTE UNIDADES EXTERNAS*/
