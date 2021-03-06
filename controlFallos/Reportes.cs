using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using h = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Diagnostics;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight.Drawing;

namespace controlFallos
{
    public partial class Reportes : Form
    {
        validaciones v;
        MySqlDataAdapter adaptador = new MySqlDataAdapter();
        MySqlDataAdapter adaptador2 = new MySqlDataAdapter();
        int empresa, area, IdUsuario;
        intermedio comunicador = new intermedio(); DataSet Ds = new DataSet();
        DataSet Ds2 = new DataSet();
        DataTable dt = new DataTable();
        DataSet DsE = new DataSet();
        DataSet Ds2E = new DataSet();
        DataTable dtE = new DataTable();
        DataSet dsConsulta = new DataSet();
        int total = 0;
        string[] meses = { "---Seleccione una opcion--", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        string unidades = "SET lc_time_names = 'es_ES';select convert(idunidad,char) as idunidad, convert(consecutivo,char) as Eco from cunidades order by consecutivo asc";
        string CosultaCarrocerosE = "SET lc_time_names = 'es_ES';convert(x1.Folio,char) as 'Folio Factura',convert(t4.CostoUni, char) as Costo, convert(t1.motivoActualizacion, char) as Actualizacion, convert(t2.usuario, char) as Usuario,  upper(date_format(convert(t1.fechaHora, char),'%d/%M/%Y')) as Fecha,UPPER(coalesce(convert(if(t5.empresa = '',concat(t5.aPaterno, ' ', t5.aMaterno, ' ', t5.nombres), t5.empresa),char),'')) as Proveedor FROM modificaciones_sistema as t1 inner join datosistema as t2 on t1.usuariofkcpersonal = t2.usuariofkcpersonal inner join cempresas as t3 on t1.empresa = t3.idempresa inner join crefacciones as t4 on t4.idrefaccion = t1.idregistro inner join cproveedores as t5 on t5.idproveedor = t4.proveedrofkCProveedores inner join cfoliosfactura x1 on x1.codrefaccionfkcrefacciones = t4.idrefaccion";
        int inicio = 0;

        

        private void Reportes_Load(object sender, EventArgs e)
        {
            cargaEcoBusq();
            CargarProveedor();
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
            v.comboswithuot(cmbEmpresa, new string[] { "--Seleccione Empresa", "TRANSINSUMOS", "TRANSMASIVO", "PRODUCCION" });
            cmbMes.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbBuscarUnidad.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbEmpresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dtpFechaDe.MaxDate = DateTime.Now;
            dtpFechaA.MaxDate = DateTime.Now;
            valoresDTP();
            //comunicador.llenarCombo(unidades, cmbBuscarUnidad, "idunidad", "ECo", "----Seleccione unidad----");
        }
        void valoresDTP()
        {
            try
            {
                dtpFechaDe.Value = DateTime.Now.Subtract(TimeSpan.Parse("1"));
            }
            catch
            {
                dtpFechaDe.Value = dtpFechaDe.MinDate;
            }
            dtpFechaA.Value = dtpFechaA.MaxDate;
        }
        public void cargaEcoBusq()
        {
            cmbBuscarUnidad.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas order by eco");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "--SELECCIONE ECONÓMICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbBuscarUnidad.DisplayMember = "eco";
            cmbBuscarUnidad.ValueMember = "idunidad";
            cmbBuscarUnidad.DataSource = dt;
        }

        public void CargarProveedor()
        {
            cmbProveedorB.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';SELECT convert(idproveedor,char) as idproveedor,if(empresa = '', concat(aPaterno, ' ', aMaterno, ' ', nombres), empresa) as empresa FROM cproveedores where empresaS = '" + empresa + "' order by concat(aPaterno, ' ', aMaterno, ' ', nombres) asc, empresa asc");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idproveedor"] = 0;
            nuevaFila["empresa"] = "--SELECCIONE PROVEEDOR--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbProveedorB.DisplayMember = "idproveedor";
            cmbProveedorB.ValueMember = "empresa";
            cmbProveedorB.DataSource = dt;
        }

        private void rbEntradas_CheckedChanged(object sender, EventArgs e)
        {
            cmbBuscarUnidad.Enabled = false;
            cmbMes.Enabled = true;
            cbFecha.Enabled = true;
            txtcodigo.Enabled = true;
            btnBuscar.Enabled = true;
            cmbProveedorB.Enabled = true;
        }

        public Reportes(int idusuario, int empresa, int area, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.IdUsuario = idusuario;
        }
        public Reportes()
        {
            InitializeComponent();
        }
        public void Entradas()
        {
            inicio = 0;
            DataSet DsEntradas = new DataSet();
            if (!string.IsNullOrEmpty(txtcodigo.Text) && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {
                DsEntradas.Clear();
                DsEntradas = cargaGridE(" where t4.codrefaccion ='" + txtcodigo.Text + "' and t1.empresa = '" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            if (cmbMes.SelectedIndex != 0 && string.IsNullOrEmpty(txtcodigo.Text) && cbFecha.Checked == false)
            {
                string messel = "";
                DsEntradas.Clear();
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }

                DsEntradas = cargaGridE(" where  MONTH(t1.FechaHora) = '" + messel + "' AND YEAR(t1.FechaHora) = '" + DateTime.Now.Year + "' and t1.Empresa ='" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cbFecha.Checked == true && cmbMes.SelectedIndex == 0 && string.IsNullOrEmpty(txtcodigo.Text))
            {
                DsEntradas = cargaGridE(" where date_format(convert(t1.FechaHora, char),'%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "' and t1.empresa ='" + empresa + "'", inicio);

                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbProveedorB.SelectedIndex != 0 && cbFecha.Checked == false && cmbMes.SelectedIndex == 0 && string.IsNullOrEmpty(txtcodigo.Text))
            {
                cmbProveedorB.SelectedValue.ToString();
                DsEntradas = cargaGridE("  where t1.Proveedor = '" + cmbProveedorB.SelectedValue.ToString() + "' and t1.empresa = '" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar e la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (!string.IsNullOrEmpty(txtcodigo.Text) && cmbMes.SelectedIndex !=0)
            {
                string messel = "";
                DsEntradas.Clear();
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                DsEntradas = cargaGridE(" where t4.codrefaccion ='" + txtcodigo.Text + "' and MONTH(t1.FechaHora) = '" + messel + "' AND YEAR(t1.FechaHora) = '" + DateTime.Now.Year + "' and t1.empresa = '" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            
            if (!string.IsNullOrEmpty(txtcodigo.Text) && cbFecha.Checked == true)
            {
                DsEntradas.Clear();
               
                DsEntradas = cargaGridE(" where t4.codrefaccion ='" + txtcodigo.Text + "' and date_format(convert(t1.FechaHora, char),'%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "' and t1.empresa = '" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            if (cmbProveedorB.SelectedIndex != 0 && cbFecha.Checked == true && cmbMes.SelectedIndex == 0 && string.IsNullOrEmpty(txtcodigo.Text))
            {
                cmbProveedorB.SelectedValue.ToString();
                DsEntradas = cargaGridE("  where t1.Proveedor = '" + cmbProveedorB.SelectedValue.ToString() + "' and date_format(convert(t1.FechaHora, char),'%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "' and t1.empresa = '" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar e la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbProveedorB.SelectedIndex != 0 && cbFecha.Checked == false && cmbMes.SelectedIndex != 0 && string.IsNullOrEmpty(txtcodigo.Text))
            {
                string messel = "";
                DsEntradas.Clear();
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                DsEntradas = cargaGridE("  where t1.Proveedor = '" + cmbProveedorB.SelectedValue.ToString() + "' and MONTH(t1.FechaHora) = '" + messel + "' AND YEAR(t1.FechaHora) = '" + DateTime.Now.Year + "' and t1.empresa = '" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbProveedorB.SelectedIndex != 0 && cbFecha.Checked == false && cmbMes.SelectedIndex == 0 && !string.IsNullOrEmpty(txtcodigo.Text))
            {
                cmbProveedorB.SelectedValue.ToString();
                DsEntradas = cargaGridE("  where t1.Proveedor = '" + cmbProveedorB.SelectedValue.ToString() + "' and t4.codrefaccion ='" + txtcodigo.Text + "' and t1.empresa = '" + empresa + "'", inicio);
                if (DsEntradas.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = DsEntradas.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar e la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public void Salidas()
        {
            inicio = 0;
            if (!string.IsNullOrEmpty(txtcodigo.Text) && cmbBuscarUnidad.SelectedIndex == 0 && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {

                cargaGridS(" where t1.codrefaccion = '" + txtcodigo.Text + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
                dsConsulta = cargarGridSC(" Where t3.codrefaccion = '" + txtcodigo.Text + "' and  t3.empresa = '" + empresa + "' and t2.Cancelado='0'  and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbBuscarUnidad.SelectedIndex != 0 && string.IsNullOrEmpty(txtcodigo.Text) && cmbMes.SelectedIndex == 0 && cbFecha.Checked == false)
            {
                dsConsulta.Clear();
               cargaGridS(" where t2.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA'  and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
               dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0' and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbMes.SelectedIndex != 0 && cmbBuscarUnidad.SelectedIndex == 0 && string.IsNullOrEmpty(txtcodigo.Text) && cbFecha.Checked == false)
            {
                string messel = "";
                dsConsulta.Clear();
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }

                cargaGridS(" where left(right(t5.FechaReporte,5),2) = '" + messel + "' AND YEAR(t5.FechaReporte) = '" + DateTime.Now.Year + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
                dsConsulta = cargarGridSC(" where date_format(convert(t2.FechaHora, char),'%m')='" + messel + "'AND YEAR(t2.FechaHora) = '" + DateTime.Now.Year + "'and t2.empresa = '" + empresa + "' and t2.Cancelado='0'  and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cbFecha.Checked == true && cmbMes.SelectedIndex == 0 && cmbBuscarUnidad.SelectedIndex == 0 && string.IsNullOrEmpty(txtcodigo.Text))
            {
               cargaGridS(" Where date_format(t3.fechaHoraPedido,'%Y/%m/%d') between '" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
               dsConsulta = cargarGridSC("  where date_format(convert(t2.FechaHora, char),'%Y/%m/%d') between'" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t2.empresa = '" + empresa + "' and t2.Cancelado='0' and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            ///consultas convinadas
            if (cmbBuscarUnidad.SelectedIndex != 0 &&  cbFecha.Checked == true)
            {
                dsConsulta.Clear();
                cargaGridS(" where t2.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and date_format(t3.fechaHoraPedido,'%Y/%m/%d') between '" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
                dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and date_format(convert(t2.FechaHora, char),'%Y/%m/%d') between'" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0'  and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (cmbBuscarUnidad.SelectedIndex != 0 && cmbMes.SelectedIndex != 0)
            {
                dsConsulta.Clear();
                string messel = "";
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                cargaGridS(" where t2.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and left(right(t5.FechaReporte,5),2) = '" + messel + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
                dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and date_format(convert(t2.FechaHora, char),'%m')='" + messel + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0' and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           if (cmbBuscarUnidad.SelectedIndex != 0 && !string.IsNullOrEmpty(txtcodigo.Text))
            {
                dsConsulta.Clear();
                cargaGridS(" where t2.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and t1.codrefaccion = '" + txtcodigo.Text + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
                dsConsulta = cargarGridSC(" Where t1.idunidad = '" + cmbBuscarUnidad.SelectedValue + "' and t3.codrefaccion = '" + txtcodigo.Text + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0' and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!string.IsNullOrEmpty(txtcodigo.Text) && cbFecha.Checked == true)
            {
                dsConsulta.Clear();
                cargaGridS(" where t1.codrefaccion = '" + txtcodigo.Text + "'and date_format(t3.fechaHoraPedido,'%Y/%m/%d') between '" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
                dsConsulta = cargarGridSC(" Where t3.codrefaccion = '" + txtcodigo.Text + "'and date_format(convert(t2.FechaHora, char),'%Y/%m/%d') between'" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0' and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (!string.IsNullOrEmpty(txtcodigo.Text) && cmbMes.SelectedIndex != 0)
            {
                dsConsulta.Clear();
                string messel = "";
                if (int.Parse(cmbMes.SelectedIndex.ToString()) == 1 || int.Parse(cmbMes.SelectedIndex.ToString()) == 2 || int.Parse(cmbMes.SelectedIndex.ToString()) == 3 || int.Parse(cmbMes.SelectedIndex.ToString()) == 4 || int.Parse(cmbMes.SelectedIndex.ToString()) == 5 || int.Parse(cmbMes.SelectedIndex.ToString()) == 6 || int.Parse(cmbMes.SelectedIndex.ToString()) == 7 || int.Parse(cmbMes.SelectedIndex.ToString()) == 8 || int.Parse(cmbMes.SelectedIndex.ToString()) == 9)
                {
                    messel = "0" + cmbMes.SelectedIndex.ToString();
                }
                else
                {
                    messel = cmbMes.SelectedIndex.ToString();
                }
                cargaGridS(" where t1.codrefaccion = '" + txtcodigo.Text + "'and left(right(t5.FechaReporte,5),2) = '" + messel + "'  and t1.empresa = '" + empresa + "' and t3.EstatusRefaccion is not null and t3.EstatusRefaccion != 'SIN EXISTENCIA' and t1.Tipo='" + cmbEmpresa.SelectedValue + "'", inicio);
                dsConsulta = cargarGridSC(" Where t3.codrefaccion = '" + txtcodigo.Text + "'and date_format(convert(t2.FechaHora, char),'%m')='" + messel + "' and t2.empresa ='" + empresa + "' and t2.Cancelado='0' and t3.Tipo='" + cmbEmpresa.SelectedValue + "' and t2.TipoSalida = '1'");
                if (dsConsulta.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = dsConsulta.Tables[0];
                    activarPaginado();
                }
                else
                {
                    MessageBox.Show("No hay datos que mostrar en la tabla".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void rbSalidas_CheckedChanged(object sender, EventArgs e)
        {
            cmbProveedorB.Enabled = false;
            activarControles();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (rbSalidas.Checked == true)
            {
                Salidas();
            }
            else if (rbEntradas.Checked == true)
            {
                Entradas();
            }
            cmbMes.SelectedIndex = 0;
            cmbBuscarUnidad.SelectedIndex = 0;
            cmbProveedorB.SelectedIndex = 0;
            txtcodigo.Text = "";
            dtpFechaA.Enabled = false;
            dtpFechaDe.Enabled = false;
            cbFecha.Checked = false;
            buttonExcel.Visible = true;
            buttonExcel.Enabled = true;
            label35.Visible = true;
        }

        

        private void buttonExcel_Click( object sender, EventArgs e)
        {
            if (rbSalidas.Checked == true)
            {
                dataGridView2.DataSource = dsConsulta.Tables[0];
                MessageBox.Show("Iniciando exportacion", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                exportar_excel();
                

            }
            else if (rbEntradas.Checked == true)
            {
                dataGridView2.DataSource = DsE.Tables[0];
                MessageBox.Show("Iniciando exportacion", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                exportar_excel2();
            }
        }

        public void activarPaginado()
        {
            btnAnterior.Enabled = true;
            btnSiguiente.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void activarControles()
        {
            cmbBuscarUnidad.Enabled = true;
            cmbMes.Enabled = true;
            cbFecha.Enabled = true;
            txtcodigo.Enabled = true;
            btnBuscar.Enabled = true;
        }
        void exportar_excel2()
        {
            //Pruebas 

            //Empezar a usar excel
            SLDocument sl = new SLDocument();

            //Importar imagen

          /*  System.Drawing.Bitmap bm = new System.Drawing.Bitmap(@"C:\Users\Ing. Osky Lopez\repos\controlfallos\controlFallos\Resources\logo.png");
            byte[] ba;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Close();
                ba = ms.ToArray();
            }

            SLPicture pic = new SLPicture(ba, DocumentFormat.OpenXml.Packaging.ImagePartType.Png);
            pic.SetPosition(0, 0);
            pic.ResizeInPixels(165, 85);
            sl.InsertPicture(pic);
            */
            //Importar imagen

            //Para saber en que celda iniciar
            int celdaCabecera = 8, celdaInicial = 8;


            //Consulta en columnas

            int ic = 2;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;


            }
            //Consulta en columnas

            //Consulta en filas
            int ir = 9;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                sl.SetCellValue(ir, 2, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[5].Value.ToString());
                sl.SetCellValue(ir, 8, row.Cells[6].Value.ToString());
                sl.SetCellValue(ir, 9, row.Cells[7].Value.ToString());
                sl.SetCellValue(ir, 10, row.Cells[8].Value.ToString());
                sl.SetCellValue(ir, 11, row.Cells[9].Value.ToString());

                ir++;

                celdaInicial++;
            }
            //Consulta en filas

            //Nombre de la Hoja de Excel
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Reporte Entradas");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "K" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "REPORTE ENTRADAS");
            SLStyle estiloT = sl.CreateStyle();
            estiloT.Font.FontName = "Arial";
            estiloT.Font.FontSize = 15;
            estiloT.Font.Bold = true;
            sl.SetCellStyle("D4", estiloT);
            sl.MergeWorksheetCells("D4", "E4");

            //Estilo Titulo

            //Estilos Para bordes de la tabla

            SLStyle EstiloB = sl.CreateStyle();

            EstiloB.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.LeftBorder.Color = System.Drawing.Color.Black;

            EstiloB.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            sl.SetCellStyle("B" + celdaInicial, "K" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "K");
            //Estilos Para bordes de la tabla

            //Extraer fecha

            sl.SetCellValue("F3", "RANGO CONSULTA DE:");
            SLStyle estiloF = sl.CreateStyle();
            estiloF.Font.FontName = "Arial";
            estiloF.Font.FontSize = 9;
            estiloF.Font.Bold = true;
            sl.SetCellStyle("F3", estiloF);
            sl.MergeWorksheetCells("F3", "G3");

            sl.SetCellValue("F4", "RANGO CONSULTA A:");
            SLStyle estiloF2 = sl.CreateStyle();
            estiloF2.Font.FontName = "Arial";
            estiloF2.Font.FontSize = 9;
            estiloF2.Font.Bold = true;
            sl.SetCellStyle("F4", estiloF2);
            sl.MergeWorksheetCells("F4", "G4");


            //Obtener Fecha
            var datestring = dtpFechaDe.Value.ToLongDateString();

            sl.SetCellValue("H3", datestring);
            SLStyle fechaDe = sl.CreateStyle();
            fechaDe.Font.FontName = "Arial";
            fechaDe.Font.FontSize = 10;
            fechaDe.Font.Bold = true;
            sl.SetCellStyle("H3", fechaDe);

            var datestring2 = dtpFechaA.Value.ToLongDateString();

            sl.SetCellValue("H4", datestring2);
            SLStyle fechaA = sl.CreateStyle();
            fechaA.Font.FontName = "Arial";
            fechaA.Font.FontSize = 10;
            fechaA.Font.Bold = true;
            sl.SetCellStyle("H4", fechaA);

            //Obtener Fecha

            //Extraer fecha

            //Directorio para Guardar el Excel

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "GUARDAR ARCHIVO";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "*.xlsx";
            saveFileDialog1.Filter = "Archivos de Excel (*.xlsx)|*.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sl.SaveAs(saveFileDialog1.FileName);
                    MessageBox.Show("   **  ARCHIVO EXPORTADO CON EXITO  **");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "   **  NO SE GUARGO EL ARCHIVO  **");
                }
            }
            //Directorio para Guardar el Excel

            //Abrir Excel


            //Abrir Excel

            buttonExcel.Visible = false;
            label35.Visible = false;

        }

        void exportar_excel()
        {
            /*
            if (dataGridView2.Rows.Count > 0)
            {
                //isexporting = true;
                dt = (DataTable)dataGridView2.DataSource;
                /*  if (this.InvokeRequired)
                  {
                      uno delega = new uno(inicio);
                      this.Invoke(delega);
                  }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                h.Worksheet sheet = (h.Worksheet)X.ActiveSheet;
                X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                for (int i = 1; i <= dt.Columns.Count; i++)
                {
                    h.Range rng = (h.Range)sheet.Cells[1, i];
                    sheet.Cells[1, i] = dt.Columns[i - 1].ColumnName.ToUpper();
                    rng.Interior.Color = System.Drawing.Color.Crimson;
                    rng.Borders.Color = System.Drawing.Color.Black;
                    rng.Font.Color = System.Drawing.Color.White;
                    rng.Cells.Font.Name = "Calibri";
                    rng.Cells.Font.Size = 12;
                    rng.Font.Bold = true;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        try
                        {
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                            sheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Cells.Font.Name = "Calibri";
                            rng.Cells.Font.Size = 11;
                            rng.Font.Bold = false;
                            rng.Interior.Color = Color.FromArgb(231, 230, 230);
                        }
                        catch (System.NullReferenceException EX)
                        { MessageBox.Show(EX.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                }
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                /*if (this.InvokeRequired) 
                {
                    dos delega2 = new dos(termino);
                    this.Invoke(delega2);
                }
                buttonExcel.Visible = false;
                label35.Visible = false;
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);*/


            //Pruebas 

            //Empezar a usar excel
            SLDocument sl = new SLDocument();

            //Importar imagen

            // System.Drawing.Bitmap bm = new System.Drawing.Bitmap(@"C:\Users\Ing. Osky Lopez\Documents\Pruebas\controlfallos\controlFallos\Resources\logo.png");
            //byte[] ba = null;


            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
            // ba = Convert.FromBase64String(v.trainsumos);
            // bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //ms.Close();
            //ba = ms.ToArray();
            // }
            /*  byte[] ba = null;

              var res = v.getaData("SELECT COALESCE(logo,'') FROM cempresas WHERE idempresa='3'").ToString();

              if (res == "")
              {
                  if (empresa == 2)
                      ba = Convert.FromBase64String(v.tri);

                  else if (empresa == 3)
                      ba = Convert.FromBase64String(v.trainsumos);

              }
              else
              {
                  System.Drawing.Image temp = v.StringToImage2(res);
                  temp = v.CambiarTamanoImagen(temp, 50, 50);
                  ba = Convert.FromBase64String(v.SerializarImg(temp));
              }

              SLPicture pic = new SLPicture(ba, DocumentFormat.OpenXml.Packaging.ImagePartType.Png);
              pic.SetPosition(0, 0);
              pic.ResizeInPixels(400, 250);
              sl.InsertPicture(pic);*/
            //Importar imagen



            //Para saber en que celda iniciar
            int celdaCabecera = 8, celdaInicial = 8;

            int ic = 2;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {

                sl.SetCellValue(8, ic, column.HeaderText.ToString());
                ic++;

            }

            int ir = 9;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                sl.SetCellValue(ir, 2, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[5].Value.ToString());
                sl.SetCellValue(ir, 8, row.Cells[6].Value.ToString());
                sl.SetCellValue(ir, 9, row.Cells[7].Value.ToString());
                sl.SetCellValue(ir, 10, row.Cells[8].Value.ToString());
                sl.SetCellValue(ir, 11, row.Cells[9].Value.ToString());

                ir++;

                celdaInicial++;
            }



            //Nombre de la Hoja de Excel
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, "Reporte Salidas");


            //Estilos de la tabla 
            SLStyle estiloCa = sl.CreateStyle();
            estiloCa.Font.FontName = "Arial";
            estiloCa.Font.FontSize = 14;
            estiloCa.Font.Bold = true;
            estiloCa.Font.FontColor = System.Drawing.Color.White;
            estiloCa.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Crimson, System.Drawing.Color.Crimson);
            sl.SetCellStyle("B" + celdaCabecera, "K" + celdaCabecera, estiloCa);
            //Estilos de la tabla 


            //Estilo Titulo

            sl.SetCellValue("D4", "REPORTE SALIDAS");
            SLStyle estiloT = sl.CreateStyle();
            estiloT.Font.FontName = "Arial";
            estiloT.Font.FontSize = 15;
            estiloT.Font.Bold = true;
            sl.SetCellStyle("D4", estiloT);
            sl.MergeWorksheetCells("D4", "E4");

            //Estilo Titulo

            //Estilos Para bordes de la tabla

            SLStyle EstiloB = sl.CreateStyle();

            EstiloB.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.LeftBorder.Color = System.Drawing.Color.Black;

            EstiloB.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            EstiloB.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            sl.SetCellStyle("B" + celdaInicial, "K" + celdaCabecera, EstiloB);

            //Ajustar celdas

            sl.AutoFitColumn("B", "K");
            //Estilos Para bordes de la tabla

            //Extraer fecha

            sl.SetCellValue("F3", "RANGO CONSULTA DE:");
            SLStyle estiloF = sl.CreateStyle();
            estiloF.Font.FontName = "Arial";
            estiloF.Font.FontSize = 9;
            estiloF.Font.Bold = true;
            sl.SetCellStyle("F3", estiloF);
            sl.MergeWorksheetCells("F3", "G3");

            sl.SetCellValue("F4", "RANGO CONSULTA A:");
            SLStyle estiloF2 = sl.CreateStyle();
            estiloF2.Font.FontName = "Arial";
            estiloF2.Font.FontSize = 9;
            estiloF2.Font.Bold = true;
            sl.SetCellStyle("F4", estiloF2);
            sl.MergeWorksheetCells("F4", "G4");


            //Obtener Fecha
            var datestring = dtpFechaDe.Value.ToLongDateString();

            sl.SetCellValue("H3", datestring);
            SLStyle fechaDe = sl.CreateStyle();
            fechaDe.Font.FontName = "Arial";
            fechaDe.Font.FontSize = 10;
            fechaDe.Font.Bold = true;
            sl.SetCellStyle("H3", fechaDe);

            var datestring2 = dtpFechaA.Value.ToLongDateString();

            sl.SetCellValue("H4", datestring2);
            SLStyle fechaA = sl.CreateStyle();
            fechaA.Font.FontName = "Arial";
            fechaA.Font.FontSize = 10;
            fechaA.Font.Bold = true;
            sl.SetCellStyle("H4", fechaA);

            //Obtener Fecha

            //Extraer fecha

            //Guardar el Excel
            //Directorio para Guardar el Excel

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "GUARDAR ARCHIVO";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "*.xlsx";
            saveFileDialog1.Filter = "Archivos de Excel (*.xlsx)|*.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    sl.SaveAs(saveFileDialog1.FileName);
                    MessageBox.Show("   **  ARCHIVO EXPORTADO CON EXITO  **   ");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "   **  NO SE GUARGO EL ARCHIVO  **   ");
                }
            }
            //Directorio para Guardar el Excel


            buttonExcel.Visible = false;
            label35.Visible = false;
        }

        private void cmbBuscarUnidad_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbMes_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }
       /* public DataSet datosG(string sql, int valor)
        {
            dt.Clear();
            Ds.Clear();
            Ds2.Clear();
            adaptador = mt.obtener_informacion(sql);
            adaptador.Fill(dt);
            total = dt.Rows.Count;
            adaptador.Fill(Ds2);
            adaptador.Fill(Ds, valor, 10, "reportemantenimiento");
            return Ds;
        }

        public DataSet datosEn(string sql, int valor)
        {
            dtE.Clear();
            DsE.Clear();
            Ds2E.Clear();
            adaptador2 = mt.obtener_informacion(sql);
            adaptador2.Fill(dtE);
            total = dtE.Rows.Count;
            adaptador2.Fill(DsE);
            adaptador2.Fill(Ds2E, valor, 10, "Entradas");
            return Ds2E;
        }*/
        public void recorrerS(int valor)
        {
            if (rbSalidas.Checked == true)
            {
                if (total >= valor && valor > 0)
                {
                    Ds2.Clear();
                    adaptador.Fill(Ds, valor, 10, "reportemantenimiento");
                    if (Ds.Tables[0].Rows.Count != 0)
                    {
                        dataGridView1.DataSource = Ds.Tables[0];
                        btnSiguiente.Enabled = true;
                        btnAnterior.Enabled = true;
                    }
                    else
                    {
                        btnAnterior.Enabled = true;
                        btnSiguiente.Enabled = false;
                    }

                }
            }
            else if (rbEntradas.Checked == true)
            {
                if (total >= valor && valor > 0)
                {
                    Ds2E.Clear();
                    adaptador2.Fill(Ds2E, valor, 10, "Entradas");
                    if (Ds2E.Tables[0].Rows.Count != 0)
                    {
                        dataGridView1.DataSource = Ds2E.Tables[0];
                        btnSiguiente.Enabled = true;
                        btnAnterior.Enabled = true;
                    }
                    else
                    {
                        btnAnterior.Enabled = true;
                        btnSiguiente.Enabled = false;
                    }
                }
            }

        }

        public void recorrerE(int valor)
        {
            if (rbSalidas.Checked == true)
            {
                if (total >= valor && valor > 0)
                {
                    Ds.Clear();
                    adaptador.Fill(Ds, valor, 10, "reportemantenimiento");
                    if (Ds.Tables[0].Rows.Count != 0)
                    {
                        dataGridView1.DataSource = Ds.Tables[0];
                        btnSiguiente.Enabled = true;
                        btnAnterior.Enabled = true;
                    }
                    else
                    {
                        btnAnterior.Enabled = true;
                        btnSiguiente.Enabled = false;
                    }

                }
            }
            else if (rbEntradas.Checked == true)
            {
                if (total >= valor && valor > 0)
                {
                    Ds2E.Clear();
                    adaptador2.Fill(Ds2E, valor, 10, "Entradas");
                    if (Ds2E.Tables[0].Rows.Count != 0)
                    {
                        dataGridView1.DataSource = Ds2E.Tables[0];
                        btnSiguiente.Enabled = true;
                        btnAnterior.Enabled = true;
                    }
                    else
                    {
                        btnAnterior.Enabled = true;
                        btnSiguiente.Enabled = false;
                    }
                }
            }
        }
        public DataSet cargaGridS(string busquedas, int valor)
        {
            dt.Clear();
            Ds.Clear();
            Ds2.Clear();
            adaptador = v.getReport("SET lc_time_names = 'es_ES';select convert(t2.consecutivo,char) as Economico,convert(t1.codrefaccion,char) as Codigo, convert(t1.nombreRefaccion,char) as Nombre,convert(t3.CantidadEntregada,char) as Cantidad,convert(t1.CostoUni,char) as 'Costo Compra',t6.Simbolo as 'Moneda',ROUND((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) ,2) as 'Costo Venta',ROUND(((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)))* t3.CantidadEntregada,2) as 'Total',upper(date_format(convert(t3.fechaHoraPedido, char),'%d/%M/%Y %H:%i %p')) as Salida, if(t1.Tipo = 1, 'NUEVA', if(t1.Tipo = 2, 'REMANOFACTURADA', if(t1.Tipo = 3, 'PODUCCION',''))) as 'TIPO' from pedidosrefaccion as t3 inner join reportemantenimiento as t4 on t4.FoliofkSupervicion = t3.FolioPedfkSupervicion inner join reportesupervicion as t5 on t5.idReporteSupervicion = t4.FoliofkSupervicion inner join   crefacciones as t1 on t1.idrefaccion = t3.RefaccionfkCRefaccion inner join cunidades as t2 on t2.idunidad = t5.UnidadfkCUnidades inner join ctipocambio as t6 on t1.tipoMonedafkCTipoCambio = t6.idtipoCambio" + busquedas + " order by t3.fechaHoraPedido desc");
            dt.Dispose();
            dt.EndInit();
            adaptador.Fill(Ds);
            return Ds;    
        }
        public DataSet cargarGridSC(string Busqueda)
        {
            DataSet ds = new DataSet();
            adaptador = v.getReport("SET lc_time_names = 'es_ES';Select  convert(concat(t1.consecutivo, '-', t1.descripcioneco),char) as Economico, convert(t3.codrefaccion,char) as Codigo, convert(t3.nombreRefaccion,char) as Nombre, convert(t2.CantidadEntregada,char) as Cantidad, convert(t3.CostoUni,char) as 'Costo Compra', convert(t5.Simbolo, char) as 'Moneda', ROUND(((t3.CostoUni * (select Costo from ctipocambio where idtipoCambio = t3.tipoMonedafkCTipoCambio)) * (5 /100)) + (t3.CostoUni * (select Costo from ctipocambio where idtipoCambio = t3.tipoMonedafkCTipoCambio)),2) as 'Costo Venta', ROUND(((t3.CostoUni * (select Costo from ctipocambio where idtipoCambio = t3.tipoMonedafkCTipoCambio)) * (5 /100) + (t3.CostoUni * (select Costo from ctipocambio where idtipoCambio = t3.tipoMonedafkCTipoCambio)))* t2.CantidadEntregada,2)as 'Total',upper(date_format(convert(t2.FechaHora, char),'%d/%M/%Y %H:%i %p')) as Salida, if(t3.Tipo = 1, 'NUEVA', if(t3.Tipo = 2, 'REMANOFACTURADA', if(t3.Tipo = 3, 'PODUCCION',''))) as 'TIPO'   from cunidades as t1 inner join ccarrocero as t2 on t1.idunidad = t2.unidadfkCUnidades inner join crefacciones as t3 on t3.idrefaccion = t2.refaccionfkCRefacciones inner join datosistema as t4 on t4.usuariofkcpersonal = t2.usuariofkCPersonal inner join ctipocambio as t5 on t3.tipoMonedafkCTipoCambio = t5.idtipoCambio" + Busqueda);
            dt.Dispose();
            dt.EndInit();
            adaptador.Fill(ds);
            Ds.Merge(ds);
            return Ds;
        }


        public DataSet cargaGridE(string busquedas, int valor)
        {
            dtE.Clear();
            DsE.Clear();
            Ds2E.Clear();

            adaptador2 = v.getReport("SET lc_time_names='es_ES';select convert(t2.FolioOrdCompra,char) as 'Folio Orden De Compra', convert(t3.Folio,char) as 'Folio Requerimiento', convert(t4.codrefaccion,char) as 'Coidog', convert(t4.nombreRefaccion,char) as 'Refaccion', convert(t1.CantidadIngresa,char) as 'Cantidad Ingresada',convert(t1.Costo,char) as 'Costo Compra',convert(t1.FolioFactura,char) as 'Folio Factura', convert(t1.proveedor,char) as 'Provedor', convert(t1.FechaHora,char) as 'Fecha/Hora Alta', convert(t5.Usuario,char) as Usuario from centradasm as t1  inner join crefacciones as t4 on t1.refaccionfkCRefacciones = t4.idrefaccion inner join datosistema as t5 on t1.UsuariofkCPersonal = t5.usuariofkcpersonal left join  ordencompra as t2 on t2.idOrdCompra = t1.ordcomprafkordencompra left join crequicision as t3 on t1.crequicisionfkcrequisicion = t3.idcrequicision " + busquedas + " order by t1.FechaHora desc;");

            /*adaptador2 = v.getReport("SET lc_time_names='es_ES';Select t1.codrefaccion as Codigo, t1.nombreRefaccion as Refaccion, t2.FolioFactura as 'Folio de Factura', t2.CantidadIngresa as 'Cantidad Ingresada', convert(t2.Costo,char) as Costo, t4.Simbolo as 'Moneda', t2.Proveedor as Proveedor, date_format(t2.FechaHora, '%d-%M-%Y') as 'Fecha Entrada', t3.Usuario as Usuario From crefacciones as t1 inner join centradasm as t2 on t1.idrefaccion =t2.refaccionfkCRefacciones inner join datosistema as t3 on t2.UsuariofkCPersonal = t3.usuariofkcpersonal inner join ctipocambio as t4 on t2.tipomonedafkCTipoCambio = t4.idtipoCambio" + busquedas + " order by t2.FechaHora desc;");*/

            dt.Dispose();
            dt.EndInit();
            adaptador2.Fill(dtE);
            total = dtE.Rows.Count;
            adaptador2.Fill(DsE);

            /*
            adaptador2.Fill(Ds2E);
            Ds2E.Merge(Ds2E);

            */
            int v1 = 0;


            if (total == 1)
            {
                adaptador2.Fill(Ds2E);
            }
            else
            {
                adaptador2.Fill(Ds2E, valor, v1, "Entradas");
                v1++;
            }
            return Ds2E;

        }



        private void cbFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFecha.Checked == true)
            {
                dtpFechaA.Enabled = true;
                dtpFechaDe.Enabled = true;
            }
            else
            {
                dtpFechaA.Enabled = false;
                dtpFechaDe.Enabled = false;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbProveedorB_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbEmpresa_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            inicio = inicio - 10;
            if (rbSalidas.Checked == true)
            {
                recorrerS(inicio);
            }
            else if (rbEntradas.Checked == true)
            {
                recorrerE(inicio);
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            inicio = inicio + 10;
            if (rbSalidas.Checked == true)
            {
                 recorrerS(inicio);
            }
            else if (rbEntradas.Checked == true)
            {
                recorrerE(inicio);
            }
        }
    }

}
