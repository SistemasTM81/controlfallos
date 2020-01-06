using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{

    public partial class Mantenimiento : Form
    {
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }
        validaciones v;
        int idUsuario, empresa, area, idreporte, idmecanico, idmecanicoApoyo, idmecaniAnterior, idmecanicoapoyoAnterior, grupoAnterior, EstatusAnterior, RefaccionesAnterior, idrefaccionAnterior, cantidadAnterior;
        string trabajoAnterior, observacionesAnterior, folioAnterior;
        bool editar, editarRefaccion;
        static bool res = true;
        string consultagral = "SET lc_time_names = 'es_ES';select t1.idReporteSupervicion as 'id',t1.Folio as 'FOLIO',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'ECONOMICO', date_format(t1.fechareporte,'%W %d de %M del %Y') as 'FECHA DE REPORTE',(select concat(coalesce(x1.appaterno,''),' ',coalesce(x1.apmaterno,''),' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCPersonal) as 'SUPERVISOR',t1.KmEntrada as 'KILOMETRAJE DE UNIDAD', t1.HoraEntrada as 'HORA DEREPORTE',coalesce((select codfallo from cfallosesp as x2 where x2.idfalloEsp=t1.CodFallofkcfallosesp ),'')as 'CÓDIGO DE FALLO',coalesce(t1.DescFalloNoCod,'') as 'FALLO NO CODIFICADO',coalesce(t1.ObservacionesSupervision,'') as 'OBSERVACIONES',(select upper(concat(coalesce(x3.appaterno,''),' ',coalesce(x3.apmaterno,''),' ',x3.nombres)) from cpersonal as x3 where x3.idpersona=t2.MecanicofkPersonal) as 'MÉCANICO',(select upper(x4.nombreFalloGral) from cfallosgrales as x4 where x4.idFalloGral=t2.FalloGralfkFallosGenerales) as 'GRUPO DE FALLO',if(t2.StatusRefacciones is null,'',(if(t2.StatusRefacciones=1,'SI','NO'))) as 'SE REQUIEREN REFACCIONES',if(t2.estatus is null,'',(if(t2.Estatus=1,'EN PROCESO',(if(t2.estatus=2,'REPROGRAMADA','LIBERADA'))))) as 'ESTATUS',coalesce(upper(t2.TrabajoRealizado),'') as 'TRABAJO REALIZADO' from reportesupervicion as t1 left join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion inner join cunidades as t3 on t1.UnidadfkCUnidades=t3.idunidad  INNER JOIN careas AS t4 on t4.idarea=t3.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas ";
        public Mantenimiento(int idUsuario, int empresa, int area, System.Drawing.Image newimg, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cmbgrupo.DrawItem += v.combos_DrawItem;
            cmbrefaccion.DrawItem += v.combos_DrawItem;
            cmbrefacciones.DrawItem += v.comboBoxEstatus_DrwaItem;
            cmbestatusb.DrawItem += v.comboBoxEstatusr_DrwaItem;
            cmbestatus.DrawItem += v.comboBoxEstatusr_DrwaItem;
            cmbmesb.DrawItem += v.combos_DrawItem;
            cmbexistencia.DrawItem += v.comboBoxEstatus_DrwaItem;
            cmbunidadb.DrawItem += v.combos_DrawItem;
            cmbgrupob.DrawItem += v.combos_DrawItem;
            cmbmecanicob.DrawItem += v.combos_DrawItem;
        }
        void quitarseen()
        {
            while (res)
            {
                MySqlConnection dbcon = null;
                if (v.c.conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                else
                    dbcon = new MySqlConnection("Server =  " + v.c.hostLocal + "; user=" + v.c.userLocal + "; password = " + v.c.passwordLocal + " ;database = sistrefaccmant ;port=" + v.c.portLocal);
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE reportesupervicion SET seen = 1 WHERE seen  = 0", dbcon);
                cmd.ExecuteNonQuery();
                dbcon.Close();
                Thread.Sleep(180000);
            }
        }
        void combos()
        {
            v.iniCombos("select idFalloGral as id, upper(nombreFalloGral) as grupo from cfallosgrales where empresa='" + empresa + "';", cmbgrupo, "id", "grupo", "--SELECCIONE UN GRUPO--");
            v.comboswithuot(cmbrefacciones, new string[] { "--seleccione una opción--", "se requieren refacciones", "no se requieren refacciones" });
            v.comboswithuot(cmbexistencia, new string[] { "--seleccione una opción", "en existencia", "sin existencia" });
            v.comboswithuot(cmbestatus, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
            v.comboswithuot(cmbestatusb, new string[] { "--seleccione--", "en proceso", "reprogramada", "liberada", });
            v.comboswithuot(cmbmesb, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
        }
        void busqueda()
        {
            v.iniCombos("SELECT t1.idunidad,concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECo FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea order by eco;", cmbunidadb, "idunidad", "ECo", "--SELECCIONE UNIDAD--");
            v.iniCombos("select idFalloGral as id, upper(nombreFalloGral) as grupo from cfallosgrales where empresa='" + empresa + "';", cmbgrupob, "id", "grupo", "--SELECCIONE UN GRUPO--");
            v.iniCombos("select t1.idPersona as id, upper(concat(coalesce(t1.appaterno,''),' ',coalesce(t1.apmaterno,''),' ',t1.nombres)) as nombre from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t2.puesto like '%Mecánico%'", cmbmecanicob, "id", "nombre", "--SELECCIONE UN MECÁNICO");
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            limpiar();
            combos();
            cargardatos();
        }

        private void txtmecanico_TextChanged(object sender, EventArgs e)
        {
            if (peditar & editar)
                pguardar.Visible = (cambios() ? true : false);
        }

        void cargardatos()
        {
            MySqlDataAdapter cargar = new MySqlDataAdapter(consultagral, v.c.dbconection());
            DataSet ds = new DataSet();
            cargar.Fill(ds);
            dgvreportes.DataSource = ds.Tables[0];
            dgvreportes.Columns[0].Visible = false;
            dgvreportes.ClearSelection();
            v.c.dbconection().Close();
            dgvreportes.ClearSelection();
        }
        void cargarrefacciones()
        {
            MySqlDataAdapter r = new MySqlDataAdapter("select t1.idPedRef, t1.NumRefacc as 'NÚMERO',upper(t2.nombreRefaccion)as 'REFACCIÓN',t1.Cantidad as 'CANTIDAD', t1.EstatusRefaccion as 'ESTATUS',t1.CantidadEntregada as 'CANTIDAD ENTREGADA' from pedidosrefaccion as t1 inner join crefacciones as t2 on t1.RefaccionfkCRefaccion=t2.idrefaccion where FolioPedfkSupervicion='" + idreporte + "';", v.c.dbconection());
            DataSet ds = new DataSet();
            r.Fill(ds);
            dgvrefacciones.DataSource = ds.Tables[0];
            dgvrefacciones.Columns[0].Visible = false;
            v.c.dbconection().Close();
            dgvrefacciones.ClearSelection();

        }

        private void btnrefacciones_Click(object sender, EventArgs e)
        {
            gbmantenimiento.Visible = !(gbrefacciones.Visible = true);
            v.iniCombos("select idrefaccion as id,upper(nombreRefaccion) as nombre from crefacciones where status='1';", cmbrefaccion, "id", "nombre", "--seleccione--");
            cargarrefacciones();
        }

        private void btnregresar_Click(object sender, EventArgs e)
        {
            gbrefacciones.Visible = !(gbmantenimiento.Visible = true);
            cmbrefacciones.Enabled = (Convert.ToInt32(v.getaData("select count(*)  from pedidosrefaccion where FolioPedfkSupervicion='" + idreporte + "';")) == 0 ? true : false);
            limpiarRefaccion();
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(v.getaData("select count(numrefacc) from pedidosrefaccion where FolioPedfkSupervicion='" + idreporte + "';"));
            n++;
            if (v.validarefacicion(Convert.ToInt32(cmbrefaccion.SelectedValue), txtcantidad.Text))
                if (v.c.insertar("insert into pedidosrefaccion (FolioPedfkSupervicion,NumRefacc,RefaccionfkCRefaccion,fechaHoraPedido,Cantidad,usuariofkcpersonal)values('" + idreporte + "','" + n + "','" + cmbrefaccion.SelectedValue + "',now(),'" + txtcantidad.Text + "','" + idUsuario + "')"))
                {
                    MessageBox.Show("Refacción agregada de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarRefaccion();
                }
        }
        void limpiarRefaccion()
        {
            cmbrefaccion.SelectedIndex = 0;
            txtcantidad.Clear();
            lblum.Text = "";
            cargarrefacciones();
        }
        private void cmbrefaccion_SelectedValueChanged(object sender, EventArgs e)
        {
            lblum.Text = (cmbrefaccion.SelectedIndex > 0 ? v.getaData("select coalesce(upper(Nombre),'') from crefacciones as t1 inner join cmarcas as t2 on t1.marcafkcmarcas=t2.idmarca inner join cfamilias as t3 on t2.descripcionfkcfamilias=t3.idfamilia inner join cunidadmedida as t4 on t3.umfkcunidadmedida=t4.idunidadmedida where t1.idrefaccion='" + cmbrefaccion.SelectedValue + "'").ToString() : "");
        }

        private void dgvrefacciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string[] drefaccion = v.getaData("select concat(RefaccionfkCRefaccion,'|',Cantidad) from pedidosrefaccion where idPedRef='" + dgvrefacciones.Rows[e.RowIndex].Cells[0].Value + "';").ToString().Split('|');
            cmbrefaccion.SelectedValue = idrefaccionAnterior = Convert.ToInt32(drefaccion[0]);
            txtcantidad.Text = (cantidadAnterior = Convert.ToInt32(drefaccion[1])).ToString();
            pagregar.Visible = !(editarRefaccion = true);
        }

        public bool getBoolFromInt(int i)
        {
            return i == 1;
        }

        private void txtmecanicoapoyo_Validated(object sender, EventArgs e)
        {
            lblmapoyo.Text = ((Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanicoapoyo.Text.Trim()) + "';")) == 0 ? "" : v.getaData("select upper(concat(coalesce(t1.appaterno,''),' ',coalesce(t1.apmaterno,''),' ',t1.nombres)) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanicoapoyo.Text.Trim()) + "';").ToString()));
            idmecanicoApoyo = ((Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanicoapoyo.Text.Trim()) + "';")) == 0 ? 0 : Convert.ToInt32(v.getaData("select t1.idpersona from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanicoapoyo.Text.Trim()) + "';"))));
            mecanicosiguales(txtmecanicoapoyo, lblmapoyo);
        }

        void status(bool enabled)
        {
            cmbgrupo.Enabled = cmbexistencia.Enabled = cmbestatus.Enabled = cmbrefacciones.Enabled = txttrabajo.Enabled = txtmecanico.Enabled = txtmecanicoapoyo.Enabled = txtsupervisor.Enabled = txtfoliof.Enabled = txtobservacionesm.Enabled = enabled;
        }
        void limpiar()
        {
            txtmecanico.Clear();
            txtmecanicoapoyo.Clear();
            txttrabajo.Clear();
            txtobservacionesm.Clear();
            cmbgrupo.SelectedIndex = cmbestatus.SelectedIndex = cmbrefacciones.SelectedIndex = cmbexistencia.SelectedIndex = idmecanico = idmecanicoApoyo = idreporte = 0;
            lblfolio.Text = lblunidad.Text = lblkilometraje.Text = lblsupervisor.Text = lblhorar.Text = lblfechas.Text = lblobservacioness.Text = lblhimant.Text = lbltiempoespera.Text = lbltiempototal.Text = lblmecanico.Text = lblmapoyo.Text = lblhtmant.Text = "";
            status(editar = pguardar.Visible = pcancelar.Visible = false);

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (v.camposmant(txtmecanico.Text, Convert.ToInt32(cmbgrupo.SelectedValue), area, Convert.ToInt32(cmbrefacciones.SelectedValue), txttrabajo.Text, Convert.ToInt32(cmbestatus.SelectedValue)))
                if (!editar)
                {
                    if (v.c.insertar("Insert Into reportemantenimiento (FoliofkSupervicion,FalloGralfkFallosGenerales,TrabajoRealizado,MecanicofkPersonal,FechaReporteM,HoraInicioM,EsperaTiempoM,Estatus,empresa,StatusRefacciones)values('" + idreporte + "','" + cmbgrupo.SelectedValue + "','" + txttrabajo.Text.Trim() + "','" + idmecanico + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + lblhimant.Text + "','','" + cmbestatus.SelectedValue + "','" + empresa + "','" + cmbrefacciones.SelectedValue + "')"))
                    {
                        MessageBox.Show("Se insertaron los datos de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        cargardatos();
                    }
                }
                else { }
        }

        private void cmbrefacciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbrefacciones.SelectedIndex > 0)
                txtfoliof.Enabled = btnrefacciones.Visible = cmbexistencia.Enabled = (Convert.ToInt32(cmbrefacciones.SelectedValue) == 1 ? true : false);
            else
                cmbexistencia.SelectedIndex = 0;
        }

        void mecanicosiguales(TextBox txt, Label lbl)
        {
            if ((idmecanico > 0 && idmecanicoApoyo > 0) && idmecanicoApoyo == idmecanico)
            {
                txt.Clear();
                lbl.Text = "";
                MessageBox.Show("El mecánico y mecánico de apoyo no pueden ser la misma persona", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtmecanico_Validated(object sender, EventArgs e)
        {
            lblmecanico.Text = (Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';")) == 0 ? "" : v.getaData("select upper(concat(coalesce(t1.appaterno,''),' ',coalesce(t1.apmaterno,''),' ',t1.nombres)) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';").ToString());
            idmecanico = ((Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';")) == 0 ? 0 : Convert.ToInt32(v.getaData("select t1.idpersona from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';"))));
            mecanicosiguales(txtmecanico, lblmecanico);
        }
        bool cambios()
        {
            bool res = false;
            if (idmecaniAnterior != idmecanico || idmecanicoApoyo != idmecanicoapoyoAnterior || Convert.ToInt32(cmbgrupo.SelectedValue) != grupoAnterior || trabajoAnterior != txttrabajo.Text.Trim() || (observacionesAnterior ?? "") != txtobservacionesm.Text.Trim())
            {
                res = true;
            }
            return res;
        }

        private void dgvreportes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';select upper(concat(t1.folio,'|',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))),'|',date_format(t1.fechareporte,'%W %d de %M del %Y'),'|',(select concat(coalesce(x1.appaterno,''),' ',coalesce(x1.apmaterno,''),' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCPersonal),'|',t1.KmEntrada,'|',coalesce(t1.CodFallofkcfallosesp,0),'|',coalesce(t1.DescFalloNoCod,''),'|',coalesce(t1.ObservacionesSupervision,''),'|',t1.HoraEntrada,'|',coalesce(t2.MecanicofkPersonal,0),'|',coalesce(t2.MecanicoApoyofkPersonal,0),'|',coalesce(t2.FalloGralfkFallosGenerales,0),'|',coalesce(t2.StatusRefacciones,0),'|',coalesce(t2.TrabajoRealizado,''),'|',coalesce(t2.Estatus,0),'|',coalesce(t2.HoraInicioM,''),'|',coalesce(t2.HoraTerminoM,''),'|',coalesce(t2.EsperaTiempoM,''),'|',coalesce(t2.DiferenciaTiempoM,''),'|',coalesce(t2.ObservacionesM,''),'|',coalesce(t2.FolioFactura,'')))as r from reportesupervicion as t1 left join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion inner join cunidades as t3 on t1.UnidadfkCUnidades=t3.idunidad  INNER JOIN careas AS t4 on t4.idarea=t3.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas where t1.idReporteSupervicion='" + (idreporte = Convert.ToInt32(dgvreportes.Rows[e.RowIndex].Cells[0].Value)) + "';").ToString().Split('|');
            status(pcancelar.Visible = true);
            lblfolio.Text = datos[0];
            lblunidad.Text = datos[1];
            lblfechas.Text = datos[2];
            lblsupervisor.Text = datos[3];
            lblkilometraje.Text = datos[4];
            lblobservacioness.Text = datos[7];
            lblhorar.Text = datos[8];
            lblmecanico.Text = (Convert.ToInt32(datos[9]) > 0 ? v.getaData("select upper(concat(coalesce(appaterno,''),' ',coalesce(apmaterno,''),' ',nombres)) from cpersonal where idpersona='" + (idmecaniAnterior = idmecanico = Convert.ToInt32(datos[9])) + "'").ToString() : "");
            lblmapoyo.Text = (Convert.ToInt32(datos[10]) > 0 ? v.getaData("select upper(concat(coalesce(appaterno,''),' ',coalesce(apmaterno,''),' ',nombres)) from cpersonal where idpersona='" + (idmecanicoapoyoAnterior = idmecanicoApoyo = Convert.ToInt32(datos[10])) + "'").ToString() : "");
            cmbgrupo.SelectedValue = grupoAnterior = Convert.ToInt32(datos[11]);
            cmbrefacciones.SelectedValue = RefaccionesAnterior = Convert.ToInt32(datos[12]);
            txttrabajo.Text = trabajoAnterior = datos[13];
            cmbestatus.SelectedValue = EstatusAnterior = Convert.ToInt32(datos[14]);
            lblhimant.Text = (string.IsNullOrWhiteSpace(datos[15]) ? DateTime.Now.ToString("HH:mm") : datos[15]);
            lblhtmant.Text = datos[16];
            lbltiempoespera.Text = datos[17];
            lbltiempototal.Text = datos[18];
            txtobservacionesm.Text = observacionesAnterior = datos[19];
            txtfoliof.Text = folioAnterior = datos[20];
            pguardar.Visible = (Convert.ToInt32(v.getaData("select count(*) from reportemantenimiento where FoliofkSupervicion='" + idreporte + "';")) == 0 ? true : false);
            if (Convert.ToInt32(v.getaData("select count(*) from reportemantenimiento where FoliofkSupervicion='" + idreporte + "';")) == 0)
                v.comboswithuot(cmbestatus, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada" });
            else editar = true;
        }

        public void privilegios()
        {
            string sql = "SELECT privilegios as privilegios FROM privilegios where usuariofkcpersonal = '" + idUsuario + "' and namform = 'Mantenimiento'";
            string[] privilegios = v.getaData(sql).ToString().Split('/');
            pinsertar = getBoolFromInt(Convert.ToInt32(privilegios[0]));
            pconsultar = getBoolFromInt(Convert.ToInt32(privilegios[1]));
            peditar = getBoolFromInt(Convert.ToInt32(privilegios[2]));
            pdesactivar = getBoolFromInt(Convert.ToInt32(privilegios[3]));
        }

        private void Mantenimiento_Load(object sender, EventArgs e)
        {
            privilegios();
            cargardatos();
            combos();
            busqueda();
        }
    }
}
