using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using h = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace controlFallos
{
    public partial class catUnidaesTRI : Form
    {
        validaciones v;

        int idUsuario, empresa, area, idcusu;
        string binanterior, nmotoranterior, ntransmisionAnterior, modeloAnterior, marcaAnterior, eco, nempresa, narea, usuarioact, cadenaEmpresa;
        int ckempresa, ckarea;
        Thread th;
        bool pconsultar { set; get; }
        bool pinsertar { set; get; }
        bool peditar { set; get; }
        bool pdesactivar { set; get; }

        public void privilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "catUnidades")).ToString().Split('/');
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

        public catUnidaesTRI(int idUsuario, Image logo, int empresa, int area,validaciones v)
        {
            this.v = v;
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            InitializeComponent();
            pictureBox1.BackgroundImage = logo;
            csetEmpresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            csetarea.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbeco.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cbEstatus.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            cadenaEmpresa = (empresa == 2 ? " (empresaMantenimiento = '2' or empresaMantenimiento = '1') " : (empresa == 3 ? " (empresaMantenimiento = '3' or empresaMantenimiento = '1') " : null));
            cbEstatus.DrawItem += new DrawItemEventHandler(v.comboBoxEstatus_DrwaItem);
        }
        private void catUnidaesTRI_Load(object sender, EventArgs e)
        {
            privilegios();
            if (pconsultar)
            {
                bunidades();
                actualizarcbx();
                Estatus();
            }
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
        void Estatus()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idnivel");
            dt.Columns.Add("Nombre");
            DataRow row = dt.NewRow();
            row["idnivel"] = 0;
            row["Nombre"] = "--Seleccione estatus--".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 1;
            row["Nombre"] = "Activo".ToUpper();
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["idnivel"] = 2;
            row["Nombre"] = "No activo".ToUpper();
            dt.Rows.Add(row);
            cbEstatus.ValueMember = "idnivel".ToUpper();
            cbEstatus.DisplayMember = "Nombre";
            cbEstatus.DataSource = dt;
        }
        public void valcbxbusq()
        {
            if (ckempresa != 0)
            {
                ckempresa = 0;
                if (csetEmpresa.SelectedIndex != 0)
                {
                    v.iniCombos("SELECT t1.idarea,UPPER(CONCAT(t2.NombreEmpresa,' - ',t1.nombreArea)) as area FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t2.idempresa='" + csetEmpresa.SelectedValue + "' AND t1.status=1 ORDER BY t2.nombreEmpresa,' - ',t1.nombreArea ASC", csetarea, "idarea", "area", "--SELECCIONE UNA ÁREA--");
                    v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea INNER JOIN cempresas as t3 ON t2.empresafkcempresas=t3.idempresa inner join cmodelos as t4 on t1.modelofkcmodelos=t4.idmodelo WHERE t1.status =1 AND t3.idempresa='" + csetEmpresa.SelectedValue + "' AND " + cadenaEmpresa, cbeco, "idunidad", "eco", "--SELECCIONE UN ECO--");
                    csetarea.Enabled = true;
                    cbeco.Enabled = false;
                }
                else
                {
                    v.iniCombos("SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE status=1 ORDER BY nombreEmpresa ASC", csetEmpresa, "idempresa", "nombreEmpresa", "--SELECCIONE UNA EMPRESA--");
                    v.iniCombos("SELECT t1.idarea,UPPER(CONCAT(t2.nombreEmpresa,' - ',t1.nombreArea)) as area FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t1.status=1 ORDER BY t2.nombreEmpresa,' - ',t1.nombreArea ASC", csetarea, "idarea", "area", "--SELECCIONE UNA ÁREA--");
                    v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cmodelos as t4 on t1.modelofkcmodelos=t4.idmodelo WHERE t1.status =1 AND " + cadenaEmpresa, cbeco, "idunidad", "eco", "--SELECCIONE UN ECO--");
                    csetarea.Enabled = false;
                    cbeco.Enabled = false;
                }
            }
            else if (ckarea != 0)
            {
                ckarea = 0;
                if (csetarea.SelectedIndex != 0)
                {
                    if (Convert.ToInt32(v.getaData("SELECT COUNT(idunidad) FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea WHERE t1.status =1 AND t2.idarea='" + csetarea.SelectedValue + "'")) > 0)
                    {
                        v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cmodelos as t4 on t1.modelofkcmodelos=t4.idmodelo WHERE t1.status =1 AND t2.idarea='" + csetarea.SelectedValue + "' AND " + cadenaEmpresa, cbeco, "idunidad", "eco", "--SELECCIONE UN ECO--");
                        cbeco.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No Hay Económicos Registrados con el Area Seleccionada", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cbeco.Enabled = true;
                    }
                }
                else
                {
                    v.iniCombos("SELECT t1.idarea,UPPER(CONCAT(t2.nombreEmpresa,' - ',t1.nombreArea)) as area FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t1.status=1 ORDER BY t2.nombreEmpresa,' - ',t1.nombreArea ASC", csetarea, "idarea", "area", "--SELECCIONE UNA ÁREA--");
                    v.iniCombos("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cmodelos as t4 on t1.modelofkcmodelos=t4.idmodelo WHERE t1.status =1 AND " + cadenaEmpresa, cbeco, "idunidad", "eco", "--SELECCIONE UN ECO--");
                    cbeco.Enabled = false;
                }
            }
        }

        public void actualizarcbx()
        {
            iniecos();
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) as nombreEmpresa FROM cempresas WHERE status=1 ")) > 0)
            {
                v.iniCombos("SELECT idempresa,Upper(nombreEmpresa) as nombreEmpresa FROM cempresas WHERE status=1 ORDER BY nombreEmpresa ASC", csetEmpresa, "idempresa", "nombreEmpresa", "--Seleccione una Empresa--");
                if (Convert.ToInt32(v.getaData("SELECT COUNT(idarea)FROM careas")) > 0)
                {
                    v.iniCombos("SELECT t1.idarea,UPPER(CONCAT(t2.nombreEmpresa,' - ',t1.nombreArea)) as area FROM careas as t1 INNER JOIN cempresas as t2 ON t1.empresafkcempresas=t2.idempresa WHERE t1.status=1 ORDER BY t2.nombreEmpresa,' - ',t1.nombreArea ASC", csetarea, "idarea", "area", "--Seleccione un Área--");
                }
                else
                    MessageBox.Show("No Hay Areas Activas", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            csetarea.DataSource = null;
            cbeco.DataSource = null;
            csetarea.Enabled = false;
            cbeco.Enabled = false;
            cbeco.Enabled = false;
        }
        public void bunidades()
        {
            DataTable dt = (DataTable)v.getData("SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) AS 'ECONÓMICO', UPPER(t3.nombreEmpresa) AS EMPRESA, UPPER(t2.nombreArea) AS 'ÁREA', UPPER(COALESCE(t1.bin,'')) AS VIN, UPPER(coalesce(t1.nmotor, '')) AS 'NÚMERO DE MOTOR', UPPER(coalesce(t1.ntransmision, '')) AS 'NÚMERO DE TRANSMISIÓN', UPPER(coalesce(t1.Marca, '')) AS MARCA, UPPER(coalesce(t1.modelo, '')) AS MODELO, (SELECT UPPER(coalesce(CONCAT(coalesce(t3.ApPaterno,''), ' ', coalesce(t3.ApMaterno,''), ' ',coalesce(t3.nombres,'')), '')) FROM cpersonal AS t3 WHERE t1.usuariofkcpersonaltri = t3.idPersona) as 'USUARIO QUE DIÓ DE ALTA',if(t1.status='1','ACTIVO','NO ACTIVO') as ESTATUS FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea INNER JOIN cempresas AS t3 ON t3.idempresa = t2.empresafkcempresas inner join cmodelos as t4 on t1.modelofkcmodelos=t4.idmodelo WHERE " + cadenaEmpresa + " ORDER BY CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) ASC");
            dataGridViewUnidadesTRI.DataSource = dt;
            dataGridViewUnidadesTRI.ClearSelection();
            dataGridViewUnidadesTRI.Columns[0].Frozen = true;
        }
        void iniecos() { v.iniCombos("SELECT idunidad, CONCAT(t2.identificador, LPAD(consecutivo, 4, '0')) AS eco FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea WHERE t1.status = 1", cbeco, "idunidad", "eco", "-- seleccione económico --"); }
        void restablecer()
        {
            idUnidadTemp = "";
            txtgetbin.Clear();
            txtgetnmotor.Clear();
            txtgettransmision.Clear();
            txtgetmodelo.Clear();
            txtgetmarca.Clear();
            idUnidadTemp = modeloAnterior = ntransmisionAnterior = nmotoranterior = binanterior = null;
            actualizarcbx();
            lblModificación.Visible = label13.Visible = btncancelu.Visible = label9.Visible = buttonGuardar.Visible = gbECO.Enabled = false;
            textBoxCUsu.Clear();
            gbECO.Text = labelNomCUsu.Text = "";
        }
        void restablecebusq()
        {
            cbEstatus.SelectedIndex = csetEmpresa.SelectedIndex = 0;
            cbeco.DataSource = csetarea.DataSource = null;
            cbeco.Enabled = csetarea.Enabled = false;
            valcbxbusq();
            txtgetbinBusq.Clear();
            txtgetnmotorbusq.Clear();
            txtgetmarcaBusq.Clear();
            txtgettransmisionbusq.Clear();
            txtgetmodelobusq.Clear();
        }
        void getCambios(object sender, EventArgs e)
        {
            buttonGuardar.Visible = label9.Visible = (!string.IsNullOrWhiteSpace(txtgetbin.Text) && binanterior != txtgetbin.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtgetnmotor.Text.Trim()) && nmotoranterior != txtgetnmotor.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtgettransmision.Text) && ntransmisionAnterior != txtgettransmision.Text.Trim()) || (!string.IsNullOrWhiteSpace(txtgetmodelo.Text) && modeloAnterior != (txtgetmodelo.Text.Trim())) || (!string.IsNullOrWhiteSpace(txtgetmarca.Text) && marcaAnterior != txtgetmarca.Text.Trim());
        }
        void mostrar() { label11.Visible = label12.Visible = peditar; }
        private string idUnidadTemp { get; set; }
        private void textBoxCUsu_TextChanged(object sender, EventArgs e)
        {
            idcusu = 0;
            labelNomCUsu.Text = null;
            object res = v.getaData("SELECT CONCAT(t1.idPersona,';',UPPER(CONCAT(coalesce(t1.ApPaterno,''), ' ', coalesce(t1.ApMaterno,''), ' ', coalesce(t1.nombres,'')))) AS nombre FROM cpersonal AS t1 INNER JOIN datosistema AS t2 ON t1.idPersona = t2.usuariofkcpersonal WHERE t2.password = '" + v.Encriptar(textBoxCUsu.Text) + "' AND t1.empresa = '" + empresa + "' AND t1.area = '" + area + "' AND t1.status = '1'");
            if (res != null)
            {
                string[] ress = res.ToString().Split(';');
                idcusu = Convert.ToInt32(ress[0]);
                labelNomCUsu.Text = ress[1];
            }
            else
            {
                idcusu = 0;
                labelNomCUsu.Text = null;
            }
        }
        Thread hiloEx;
        delegate void Loading();
        public void cargando1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            buttonExcel.Visible = false;
            label35.Text = "EXPORTANDO";
            label35.Location = new Point(455, 778);
        }
        delegate void Loading1();
        public void cargando2()
        {
            pictureBoxExcelLoad.Image = null;
            buttonExcel.Visible = true;
            label35.Text = "EXPORTAR";
            if (exportando)
            {
                buttonExcel.Visible = false;
                label35.Visible = false;
            }
            exportando = false;
            activado = false;
            label35.Location = new Point(475, 778);
        }
        delegate void El_Delegado(); delegate void El_Delegado1();
        public void exporta_a_excel()
        {
            if (dataGridViewUnidadesTRI.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < dataGridViewUnidadesTRI.Columns.Count; i++) if (dataGridViewUnidadesTRI.Columns[i].Visible) dt.Columns.Add(dataGridViewUnidadesTRI.Columns[i].HeaderText);
                for (int j = 0; j < dataGridViewUnidadesTRI.Rows.Count; j++)
                {

                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < dataGridViewUnidadesTRI.Columns.Count; i++)
                    {

                        if (dataGridViewUnidadesTRI.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = dataGridViewUnidadesTRI.Rows[j].Cells[i].Value;
                            indice++;
                        }

                    }
                    dt.Rows.Add(row);
                }
                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando1);
                    this.Invoke(delega);
                }

                v.exportaExcel(dt);
                if (this.InvokeRequired)
                {
                    El_Delegado1 delega = new El_Delegado1(cargando2);
                    this.Invoke(delega);
                }
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {
            try
            {
                if (peditar || pinsertar)
                {
                    if (Convert.ToInt32(v.getaData("select distinct t1.status from cunidades as t1 inner join careas as t2 ON t1.areafkcareas = t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas  where CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) = '" + dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[0].Value.ToString() + "' and upper(t2.nombreArea)='" + dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[2].Value.ToString() + "' and t3.nombreEmpresa='" + dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[1].Value.ToString() + "';")) == 0)
                    {
                        MessageBox.Show("La unidad se encuentra desactivada, por lo tanto no se puede modificar su información", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        restablecer();
                        eco = dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[0].Value.ToString();
                        narea = dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[2].Value.ToString();
                        usuarioact = dataGridViewUnidadesTRI.Rows[e.RowIndex].Cells[8].Value.ToString();
                        MySqlCommand cmdid = new MySqlCommand("SELECT t1.idunidad FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea WHERE t2.nombreArea = '" + narea + "' AND CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) = '" + eco + "'", v.c.dbconection());
                        MySqlDataReader drid = cmdid.ExecuteReader();
                        if (drid.Read())
                        {
                            idUnidadTemp = Convert.ToString(drid.GetString("idunidad"));
                        }
                        drid.Close();
                        v.c.dbcon.Close();
                        MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(t2.identificador,LPAD(t1.consecutivo,4,'0')) AS ECO, (SELECT UPPER(nombreEmpresa) FROM cempresas WHERE idempresa = t2.empresafkcempresas) AS EMPRESA, coalesce(t1.bin, '') AS bin, coalesce(t1.nmotor, '') AS nmotor, coalesce(t1.ntransmision, '') AS ntransmision, coalesce(t1.modelo, '') AS modelo, coalesce(t1.Marca, '') AS Marca FROM cunidades as t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea WHERE t1.idunidad = '" + idUnidadTemp + "'", v.c.dbconection());
                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            txtgetbin.Text = this.binanterior = Convert.ToString(dr.GetString("bin"));
                            txtgetnmotor.Text = this.nmotoranterior = Convert.ToString(dr.GetString("nmotor"));
                            txtgettransmision.Text = this.ntransmisionAnterior = Convert.ToString(dr.GetString("ntransmision"));
                            txtgetmodelo.Text = this.modeloAnterior = Convert.ToString(dr.GetString("modelo"));
                            txtgetmarca.Text = this.marcaAnterior = Convert.ToString(dr.GetString("Marca"));
                            nempresa = dr.GetString("EMPRESA");
                            gbECO.Text = " " + nempresa + " - " + narea + " / ECONÓMICO: '" + eco + "'";
                            if (usuarioact != "")
                            {
                                if (pinsertar && peditar && pconsultar)
                                {
                                    lblModificación.Visible = true;
                                    labelNomCUsu.Text = usuarioact;
                                }
                            }
                            else
                            {
                                lblModificación.Visible = false;
                                labelNomCUsu.Text = "";
                            }
                        }
                        dr.Close();
                        v.c.dbcon.Close();
                        gbECO.Enabled = true;
                        btncancelu.Visible = true;
                        label13.Visible = true;
                        buttonGuardar.Visible = false;
                        label9.Visible = false;
                        txtgetbin.Focus();
                        txtgetbin.SelectionStart = txtgetbin.Text.Length;
                    }
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewUnidadesTRI_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string vin = txtgetbin.Text;
                string motor = txtgetnmotor.Text;
                string trans = txtgettransmision.Text;
                string modelo = txtgetmodelo.Text;
                string marca = txtgetmarca.Text;
                if (!string.IsNullOrWhiteSpace(idUnidadTemp) && (!vin.Equals(binanterior) || !motor.Equals(nmotoranterior) || !trans.Equals(ntransmisionAnterior) || !modelo.Equals(modeloAnterior) || !marca.Equals(marcaAnterior)))
                {
                    if (MessageBox.Show("Se Detectaron Modificaciones en los Datos del Económico. ¿Desea Guardar Los Cambios?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        buttonGuardar_Click(null, e);
                    else
                        guardarReporte(e);
                }
                else
                    guardarReporte(e);
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                object eco = cbeco.SelectedValue;
                string vin = txtgetbinBusq.Text;
                string motor = txtgetnmotorbusq.Text;
                string trans = txtgettransmisionbusq.Text;
                string modelo = txtgetmodelobusq.Text;
                string marca = txtgetmarcaBusq.Text;
                int area = 0; if (csetarea.DataSource != null) area = Convert.ToInt32(csetarea.SelectedValue);
                int econ = 0; if (cbeco.DataSource != null) econ = Convert.ToInt32(cbeco.SelectedValue);
                if (csetEmpresa.SelectedIndex > 0 || area > 0 || econ > 0 || !string.IsNullOrWhiteSpace(vin) || !string.IsNullOrWhiteSpace(motor) || !string.IsNullOrWhiteSpace(trans) || !string.IsNullOrWhiteSpace(modelo) || !string.IsNullOrWhiteSpace(marca) || cbEstatus.SelectedIndex > 0)
                {
                    //dataGridViewUnidadesTRI.Rows.Clear();
                    string wheres = " WHERE " + cadenaEmpresa;
                    string sql = "SET NAMES 'utf8';SET lc_time_names = 'es_ES';SELECT CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) AS 'ECONÓMICO', UPPER(t3.nombreEmpresa) AS EMPRESA, UPPER(t2.nombreArea) AS 'ÁREA', UPPER(COALESCE(t1.bin,'')) AS VIN, UPPER(coalesce(t1.nmotor, '')) AS 'NÚMERO DE MOTOR', UPPER(coalesce(t1.ntransmision, '')) AS 'NÚMERO DE TRANSMISIÓN', UPPER(coalesce(t1.Marca, '')) AS MARCA, UPPER(coalesce(t1.modelo, '')) AS MODELO, (SELECT UPPER(coalesce(CONCAT(coalesce(t3.ApPaterno,''), ' ', coalesce(t3.ApMaterno,''), ' ',coalesce(t3.nombres,'')), '')) FROM cpersonal AS t3 WHERE t1.usuariofkcpersonaltri = t3.idPersona) as 'USUARIO QUE DIÓ DE ALTA',if(t1.status='1','ACTIVO','NO ACTIVO') as ESTATUS FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea INNER JOIN cempresas AS t3 ON t3.idempresa = t2.empresafkcempresas inner join cmodelos as t4 on t1.modelofkcmodelos=t4.idmodelo  ";
                    if (csetEmpresa.DataSource != null && csetEmpresa.SelectedIndex > 0)
                    {
                        if (wheres == "")
                            wheres = "AND ((SELECT idempresa FROM careas as t11 INNER JOIN cempresas as t22 ON t11.empresafkcempresas = t22.idempresa WHERE ((t11.idarea=t2.idarea)))) = '" + csetEmpresa.SelectedValue + "' ";
                        else
                            wheres += "AND ((SELECT idempresa FROM careas as t11 INNER JOIN cempresas as t22 ON t11.empresafkcempresas = t22.idempresa WHERE t11.idarea=t2.idarea)) = '" + csetEmpresa.SelectedValue + "' ";
                    }
                    if (csetarea.DataSource != null && csetarea.SelectedIndex > 0)
                    {
                        if (wheres == "")
                            wheres = "AND (t2.idarea= '" + csetarea.SelectedValue + "'";
                        else
                            wheres += "AND t2.idarea = '" + csetarea.SelectedValue + "'";
                    }
                    if (Convert.ToInt32(eco) > 0)
                    {
                        if (wheres == "")
                            wheres = "AND (t1.idUnidad = '" + eco + "%'";
                        else
                            wheres += " AND t1.idUnidad = '" + eco + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(vin))
                    {
                        if (wheres == "")
                            wheres = "AND (bin LIKE '" + vin + "%')";
                        else
                            wheres += " AND bin LIKE '" + vin + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(motor))
                    {
                        if (wheres == "")
                            wheres = "AND nmotor LIKE '" + motor + "%'";
                        else
                            wheres += " AND nmotor LIKE '" + motor + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(trans))
                    {
                        if (wheres == "")
                            wheres = "AND ntransmision LIKE '" + trans + "%'";
                        else
                            wheres += " AND ntransmision LIKE '" + trans + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(modelo))
                    {
                        if (wheres == "")
                            wheres = "AND modelo LIKE '" + modelo + "%'";
                        else
                            wheres += " AND modelo LIKE '" + modelo + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(marca))
                    {
                        if (wheres == "")
                            wheres = "AND marca LIKE '" + marca + "%'";
                        else
                            wheres += " AND  marca LIKE '" + marca + "%'";
                    }
                    if (cbEstatus.SelectedIndex > 0)
                    {
                        if (wheres == "")
                            wheres = " WHERE t1.status='" + v.statusinv(cbEstatus.SelectedIndex - 1) + "'";
                        else
                            wheres += " AND t1.status='" + v.statusinv(cbEstatus.SelectedIndex - 1) + "'";
                    }
                    if (wheres != "") wheres += "";
                    sql += wheres + " ORDER BY EMPRESA, 'ÁREA', 'ECONÓMICO' DESC";
                    MySqlDataAdapter cm = new MySqlDataAdapter(sql, v.c.dbconection());
                    DataSet ds = new DataSet();
                    cm.Fill(ds);
                    dataGridViewUnidadesTRI.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron Económicos con los Criterios Seleccionados", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        esta_exportando();
                        bunidades();
                        restablecebusq();
                    }
                    else
                    {
                        restablecebusq();
                        restablecer();
                        if (!activado)
                        {
                            buttonExcel.Visible = true;
                        }
                        label35.Visible = true;
                    }
                }
                else
                    MessageBox.Show("Seleccione un Criterio de Búsqueda", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        bool exportando = false;
        public void esta_exportando()
        {
            if (label35.Text.Equals("EXPORTANDO"))
                exportando = true;
            else
                label35.Visible = buttonExcel.Visible = false;
        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            esta_exportando();
            if (Convert.ToInt32(v.getaData("SELECT COUNT(*) FROM cunidades as t1 inner join cmodelos as t4 on t1.modelofkcmodelos=t4.idmodelo WHERE " + cadenaEmpresa)) != dataGridViewUnidadesTRI.Rows.Count)
                bunidades();
            restablecer();
            restablecebusq();
            actualizarcbx();
        }
        private void btncancelu_Click(object sender, EventArgs e)
        {
            string vin = txtgetbin.Text;
            string motor = txtgetnmotor.Text;
            string trans = txtgettransmision.Text;
            string modelo = txtgetmodelo.Text;
            string marca = txtgetmarca.Text;
            restablecer();
            buttonGuardar.Visible = false;
            label9.Visible = false;
            btncancelu.Visible = false;
            label13.Visible = false;
            dataGridViewUnidadesTRI.ClearSelection();
        }
        bool activado = false;
        private void buttonExcel_Click(object sender, EventArgs e)
        {
            activado = true;
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
        }
        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string vin = txtgetbin.Text;
                string motor = txtgetnmotor.Text;
                string trans = txtgettransmision.Text;
                string modelo = txtgetmodelo.Text;
                string marca = txtgetmarca.Text;
                if (!v.formularioUnidadesTRI(vin, motor, trans, modelo, marca) && !v.yaexisteunidad(vin, motor, trans, eco, idUnidadTemp, empresa))
                {
                    if ((labelNomCUsu.Text != "") && (!string.IsNullOrWhiteSpace(textBoxCUsu.Text)))
                    {
                        if (v.mayusculas(vin).Equals(this.binanterior) && v.mayusculas(motor).Equals(this.nmotoranterior) && v.mayusculas(trans).Equals(this.ntransmisionAnterior) && v.mayusculas(modeloAnterior).Equals(modelo) && v.mayusculas(marcaAnterior).Equals(marca))
                        {
                            MessageBox.Show("No se hicieron Modificaciones", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (MessageBox.Show("¿Desea Limpiar Todos los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                restablecer();
                                bunidades();
                                esta_exportando();
                            }
                        }
                        else
                        {
                            if (binanterior == "")
                            {
                                /* AÑADIR CONSULTA PARA ADICIONAR POR PRIMERA VEZ */
                                v.c.insertar("UPDATE cunidades SET bin = LTRIM(RTRIM('" + vin + "')), nmotor = LTRIM(RTRIM('" + motor + "')), ntransmision = LTRIM(RTRIM('" + trans + "')), modelo = LTRIM(RTRIM('" + modelo + "')), Marca = LTRIM(RTRIM('" + marca + "')), usuariofkcpersonaltri = '" + idcusu + "' WHERE idunidad = '" + idUnidadTemp + "'");
                                v.c.insertar("INSERT INTO modificaciones_sistema (form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo, empresa, area) values('Catálogo de Unidades', '" + v.getaData("select idunidad from cunidades as t1 inner join careas as t2 ON t1.areafkcareas = t2.idarea  where CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0')) = '" + eco + "'") + "','" + vin + ";" + motor + ";" + trans + ";" + modelo + ";" + marca + "','" + idUsuario + "',NOW(),'Inserción de Especificaciones','" + empresa + "','" + area + "') ");
                                MessageBox.Show("Especificaciones de la Unidad Guardadas", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                restablecer();
                                bunidades();
                                esta_exportando();
                            }
                            else
                            {
                                if (peditar)
                                {
                                    if (string.IsNullOrWhiteSpace(textBoxCUsu.Text.Trim()))
                                    {
                                        MessageBox.Show("Ingresa tu contraseña para guardar el registro", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        textBoxCUsu.Focus();
                                    }
                                    else
                                    {
                                        observacionesEdicion obs = new observacionesEdicion(v);
                                        obs.Owner = this;
                                        DialogResult resobj = obs.ShowDialog();
                                        if (resobj == DialogResult.OK)
                                        {
                                            string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                            v.c.insertar("UPDATE cunidades SET bin=LTRIM(RTRIM('" + vin + "')), nmotor=LTRIM(RTRIM('" + motor + "')) ,ntransmision=LTRIM(RTRIM('" + trans + "')), modelo=LTRIM(RTRIM('" + modelo + "')), Marca=LTRIM(RTRIM('" + marca + "')) WHERE idunidad = '" + idUnidadTemp + "'");
                                            MessageBox.Show("Especificaciones de la Unidad Actualizadas", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                                            v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion, empresa, area) VALUES('Catálogo de Unidades', '" + idUnidadTemp + "','" + binanterior + ";" + nmotoranterior + ";" + ntransmisionAnterior + ";" + modeloAnterior + ";" + marcaAnterior + "','" + idUsuario + "',NOW(),'Actualización de Unidad','" + edicion + "','" + empresa + "','" + area + "')");
                                            bunidades();
                                            restablecer();
                                            esta_exportando();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Usted no cuenta con privilegios para editar información.", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(textBoxCUsu.Text))
                    {
                        MessageBox.Show("Ingresa tu contraseña para guardar el registro", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxCUsu.Focus();
                    }
                    else if ((labelNomCUsu.Text == "") && (!string.IsNullOrWhiteSpace(textBoxCUsu.Text)))
                    {
                        MessageBox.Show("Contraseña incorrecta", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxCUsu.Clear();
                        textBoxCUsu.Focus();
                    }
                }
                else
                {
                    idcusu = 0;
                    textBoxCUsu.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void csetEmpresa_SelectedIndexChanged(object sender, EventArgs e) { valcbxbusq(); }
        private void csetarea_SelectedIndexChanged(object sender, EventArgs e) { valcbxbusq(); }
        private void cbeco_SelectedIndexChanged(object sender, EventArgs e) { valcbxbusq(); }
        private void csetEmpresa_Click(object sender, EventArgs e) { ckempresa = 1; }
        private void csetarea_Click(object sender, EventArgs e) { ckarea = 1; }
        private void dataGridViewUnidadesTRI_ColumnAdded(object sender, DataGridViewColumnEventArgs e) { v.paraDataGridViews_ColumnAdded(sender, e); }
        private void textBoxAll_Validating(object sender, CancelEventArgs e) { v.espaciosenblanco(sender, e); }
        private void txtgetnmotor_KeyPress(object sender, KeyPressEventArgs e) { v.letrasnumerosdiagonalypunto(e); }
        private void txtgetbin_KeyPress(object sender, KeyPressEventArgs e) { v.letrasynumerossinespacios(e); }
        private void txtgetmodelo_KeyPress(object sender, KeyPressEventArgs e) { e.Handled = !(e.KeyChar == 32 || e.KeyChar == 45 || char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == 08 || e.KeyChar == 127 || e.KeyChar == 47); }
        private void textBoxCUsu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                buttonGuardar.Focus();
            else
                v.paraUsuarios(e);
        }
        private void cbeco_DrawItem(object sender, DrawItemEventArgs e) { v.combos_DrawItem(sender, e); }
        public void combos_para_otros_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    Brush brush = new SolidBrush(cbx.ForeColor);
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }
        private void groupBoxAll_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }
        private void buttonBuscar_MouseMove(object sender, MouseEventArgs e) { buttonBuscar.Size = new Size(57, 47); }
        private void buttonBuscar_MouseLeave(object sender, EventArgs e) { buttonBuscar.Size = new Size(52, 42); }
        private void buttonActualizar_MouseMove(object sender, MouseEventArgs e) { buttonActualizar.Size = new Size(68, 62); }

        private void dataGridViewUnidadesTRI_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridViewUnidadesTRI.Columns[e.ColumnIndex].Name == "ESTATUS")
            {
                if (Convert.ToString(e.Value) == "ACTIVO")
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }
        private void buttonExcel_MouseMove(object sender, MouseEventArgs e) { buttonExcel.Size = new Size(63, 63); }
        private void buttonGuardar_MouseMove(object sender, MouseEventArgs e) { buttonGuardar.Size = new Size(68, 62); }
        private void btncancelu_MouseMove(object sender, MouseEventArgs e) { btncancelu.Size = new Size(63, 63); }
        private void button5858_MouseLeave(object sender, EventArgs e)
        {
            Button btn58 = sender as Button;
            btn58.Size = new Size(58, 58);
        }
        private void button6357_MouseLeave(object sender, EventArgs e)
        {
            Button btn63 = sender as Button;
            btn63.Size = new Size(63, 57);
        }
        private void csetEmpresa_Leave(object sender, EventArgs e) { ckempresa = 0; }
        private void csetarea_Leave(object sender, EventArgs e) { ckarea = 0; }
    }
}