using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class Diesel : Form
    {
        validaciones v;
        int empresa, area, IdUsuario;
        double cantidad_actual = 0.0;
        public Diesel(int idusuario, int empresa, int area, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.IdUsuario = idusuario;
        }

        private void Diesel_Load(object sender, EventArgs e)
        {
            cargaGrid(" WHERE left(x1.fechaHoraReg,10) BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY)) AND curdate() ");
            cargaEco();
            Aditivos();
            cargaEcoBusq();
            v.comboswithuot(cmbMes, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
            cmbMes.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbEco.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbAgrega.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbBuscarUnidad.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);

            dtpFechaDe.MaxDate = DateTime.Now;
            dtpFechaA.MaxDate = DateTime.Now;
            valoresDTP();
            dtpFechaA.MinDate = dtpFechaDe.MinDate = Convert.ToDateTime(v.getaData("SELECT date(coalesce(MIN(fechaHoraReg),NOW())) FROM dispensadordiesel"));
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
        public void cargaEco()
        {
            cmbEco.DataSource = null;
            DataTable dt = (DataTable)v.getData("SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas order by eco");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["idunidad"] = 0;
            nuevaFila["eco"] = "--SELECCIONE ECONÓMICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbEco.DisplayMember = "eco";
            cmbEco.ValueMember = "idunidad";
            cmbEco.DataSource = dt;
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
        public void Aditivos()
        {
            cmbAgrega.DataSource = null;
            DataTable dt = (DataTable)v.getData("select convert(idrefaccion,Char) as id, UPPER(nombreRefaccion) as Refaccion from crefacciones where formDiesel='1' order by id");
            DataRow nuevaFila = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["Refaccion"] = "--SELECCIONE--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cmbAgrega.DisplayMember = "Refaccion";
            cmbAgrega.ValueMember = "id";
            cmbAgrega.DataSource = dt;
        }
        public void cargaGrid(string busquedas)
        {
            tbCargas.Rows.Clear();
            DataTable dt = (DataTable)v.getData("select convert(idDispDiesel,char) as id, UPPER((SELECT concat(t2.identificador,LPAD(consecutivo,4,'0')) FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where t1.idunidad= x1.EcofkCUnidades)) as eco, UPPER(x2.nombreRefaccion) as 'Refacción', convert(x1.Cant, char) as Cant,(select coalesce(upper(t4.Nombre),'') from crefacciones as t1 inner join cmarcas as t2 on t1.marcafkcmarcas=t2.idmarca inner join cfamilias as t3 on t2.descripcionfkcfamilias=t3.idfamilia inner join cunidadmedida as t4 on t3.umfkcunidadmedida = t4.idunidadmedida where t1.idrefaccion = x1.refaccionfkCRefacciones limit 1) as Unidad, x1.fechaHoraReg as Fecha,UPPER((select convert(z1.usuario, char) from datosistema as z1 where z1.iddato = x1.usuariofkdatosistema)) as usuario from dispensadordiesel as x1 inner join crefacciones as x2 on x1.refaccionfkCRefacciones=x2.idrefaccion " + busquedas + " order by x1.fechaHoraReg desc");
            for (int i = 0; i < dt.Rows.Count; i++)
                tbCargas.Rows.Add(dt.Rows[i].ItemArray);
            dt.Dispose();
            dt.EndInit();
            tbCargas.ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            limpia();
        }
        public void limpia()
        {
            txtCant.Clear(); lblUnidad.Text = ""; cmbAgrega.SelectedIndex = cmbEco.SelectedIndex = 0;
            cargaGrid(" WHERE left(x1.fechaHoraReg,10) BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY)) AND curdate() ");
        }

        private void btnGuarda_Click(object sender, EventArgs e)
        {
            if (cmbEco.SelectedIndex > 0 && cmbAgrega.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtCant.Text))
            {
                if (v.c.insertar("insert into dispensadordiesel(EcofkCunidades,refaccionfkCRefacciones,Cant,fechaHoraReg,usuariofkdatosistema) values('" + cmbEco.SelectedValue + "','" + cmbAgrega.SelectedValue + "','" + txtCant.Text.Trim() + "',now(),'" + IdUsuario + "')"))
                {
                    double actualizar = cantidad_actual - Double.Parse(txtCant.Text);
                    if (v.c.insertar("update crefacciones set existencias = '" + actualizar + "' where idrefaccion='" + cmbAgrega.SelectedValue + "'")) { }
                    string id = v.getaData("select if (count(convert(idDispDiesel, char)) > 0, convert(idDispDiesel, char), '0') as id from dispensadordiesel where EcofkCunidades = '" + cmbEco.SelectedValue + "' and refaccionfkCRefacciones = '" + cmbAgrega.SelectedValue + "' order by iddispdiesel desc limit 1").ToString();
                    v.c.insertar("insert into modificaciones_sistema(form, idregistro, ultimaModificacion,usuariofkcpersonal,fechaHora, Tipo, empresa, area) values('Dispensador de Diesel','" + id + "', '" + cmbEco.SelectedValue + ";" + cmbAgrega.SelectedValue + ";" + txtCant.Text.Trim() + "', '" + IdUsuario + "', now(), 'Registro', '" + empresa + "', '" + area + "')");
                    MessageBox.Show("Registro Correcto", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpia();
                }
                else
                {
                    MessageBox.Show("Intente más tarde", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Complete los datos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbEco_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbAgrega_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void txtCant_Validating(object sender, CancelEventArgs e)
        {
           /* TextBox txtCanti = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(txtCanti.Text.Trim()))
            {
                while (txtCanti.Text.Contains(".."))
                    txtCanti.Text = txtCanti.Text.Replace("..", ".").Trim();
                txtCanti.Text = txtCanti.Text.Trim();
                txtCanti.Text = string.Format("{0:F2}", txtCanti.Text);
                try
                {
                    if (Convert.ToDouble(txtCanti.Text) > 0)
                    {
                        CultureInfo ti = new CultureInfo("es-MX"); ti.NumberFormat.CurrencyDecimalDigits = 2; ti.NumberFormat.CurrencyDecimalSeparator = "."; txtCanti.Text = string.Format("{0:N2}", Convert.ToDouble(txtCanti.Text, ti));
                    }
                    else txtCanti.Text = "0";
                }
                catch (Exception ex)
                {
                    txtCanti.Clear(); MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }*/
        }
        bool activado = false;
        private void buttonExcel_Click(object sender, EventArgs e)
        {
            activado = true;
            ThreadStart excel = new ThreadStart(exporta_a_excel);
            hiloEx = new Thread(excel);
            hiloEx.Start();
        }
        bool exportando = false;
        public void esta_exportando()
        {
            if (label35.Text.Equals("EXPORTANDO"))
                exportando = true;
            else
                label35.Visible = buttonExcel.Visible = false;
        }
        Thread hiloEx;
        delegate void Loading();
        public void cargando1()
        {
            pictureBoxExcelLoad.Image = Properties.Resources.loader;
            buttonExcel.Visible = false;
            label35.Text = "EXPORTANDO";
            label35.Visible = true;
            label35.Location = new Point(455, 778);
        }
        delegate void Loading1();
        public void cargando2()
        {
            pictureBoxExcelLoad.Image = null;
            label35.Visible = buttonExcel.Visible = true;
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
            if (tbCargas.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                for (int i = 0; i < tbCargas.Columns.Count; i++) if (tbCargas.Columns[i].Visible) dt.Columns.Add(tbCargas.Columns[i].HeaderText);
                for (int j = 0; j < tbCargas.Rows.Count; j++)
                {

                    DataRow row = dt.NewRow();
                    int indice = 0;
                    for (int i = 0; i < tbCargas.Columns.Count; i++)
                    {

                        if (tbCargas.Columns[i].Visible)
                        {
                            row[dt.Columns[indice]] = tbCargas.Rows[j].Cells[i].Value;
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cmbMes.Enabled = !(dtpFechaA.Enabled = dtpFechaDe.Enabled = true);
                cmbMes.SelectedIndex = 0;
            }
            else
                cmbMes.Enabled = !(dtpFechaDe.Enabled = dtpFechaA.Enabled = false);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            realiza_busquedas();
        }
        void realiza_busquedas()
        {
            if (checkBox1.Checked == true || cmbMes.SelectedIndex > 0 || cmbBuscarUnidad.SelectedIndex > 0)
            {
                //Verificar si el chechBox esta seleccionado para realizar busqueda por rango de fechas
                if ((dtpFechaA.Value.Date < dtpFechaDe.Value.Date || dtpFechaA.Value.Date > DateTime.Now) && checkBox1.Checked) //Validar que las fechas seleccionadas sean correctas, que la fecha 1 no sea mayor a la fecha 2
                {
                    MessageBox.Show("Las fechas seleccionadas son incorrectas".ToUpper(), "VERIFICAR FECHAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpFechaDe.Value = DateTime.Now;
                    dtpFechaA.ResetText();
                }
                else
                {
                    string wheres = "";
                    if (checkBox1.Checked)
                        wheres = (wheres == "" ? " Where date_format(x1.fechaHoraReg,'%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'" : wheres += " AND date_format(x1.fechaHoraReg,'%Y-%m-%d') between '" + dtpFechaDe.Value.ToString("yyyy-MM-dd") + "' and '" + dtpFechaA.Value.ToString("yyyy-MM-dd") + "'");

                    if (cmbBuscarUnidad.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where x1.EcofkCUnidades='" + cmbBuscarUnidad.SelectedValue + "'" : wheres += " And x1.EcofkCUnidades='" + cmbBuscarUnidad.SelectedValue + "'");
                    if (cmbMes.SelectedIndex > 0)
                        wheres = (wheres == "" ? " Where (select month(x1.fechaHoraReg)='" + cmbMes.SelectedIndex + "' and (select year(x1.fechaHoraReg))=( select year(now())))" : wheres += " AND (select month(x1.fechaHoraReg)='" + cmbMes.SelectedIndex + "' and (select year(x1.fechaHoraReg))=( select year(now())))");
                    /*if (wheres != "")
                        wheres += " and (select year(x1.fechaHoraReg))=( select year(now())) ";*/
                    cargaGrid(" " + wheres + " ");
                    if (tbCargas.Rows.Count == 0)// si no existen cargas de diesel en el datagridview mandamos un mensaje
                    {
                        MessageBox.Show("No se encontraron reportes".ToUpper(), "NINGÚN REPORTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cargaGrid(" WHERE left(x1.fechaHoraReg,10) BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY)) AND curdate() ");
                    }
                    else
                    {
                        label35.Visible = true;
                        if (!exportando)
                        {
                            buttonExcel.Visible = true;
                        }
                    }
                    checkBox1.Checked = false;
                    LimpiarBusqueda();//LLamamos al metodo LimpiarBusqueda.
                }
            }
            else
            {
                //Mandamos mensaje en caso de que se encuentren vacios los campos
                MessageBox.Show("Seleccione un criterio de búsqueda".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void LimpiarBusqueda()//Metodo para limpiar todos los campos que se encuentran en la sección de busqueda.
        {
            cmbBuscarUnidad.SelectedIndex = cmbMes.SelectedIndex = 0;
            dtpFechaDe.Value = dtpFechaA.Value = dtpFechaDe.MaxDate;
        }

        private void cmbBuscarUnidad_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void cmbMes_DrawItem(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void txtCant_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.numerosDecimales(e);
        }

        private void cmbAgrega_SelectedValueChanged(object sender, EventArgs e)
        {
            lblUnidad.Text = (cmbAgrega.SelectedIndex > 0 ? v.getaData("select coalesce(upper(t4." + v.c.fieldscunidadmedida[1] + "),'') from crefacciones as t1 inner join cmarcas as t2 on t1." + v.c.fieldscrefacciones[7] + "=t2." + v.c.fieldscmarcas[0] + " inner join cfamilias as t3 on t2." + v.c.fieldscmarcas[1] + "=t3." + v.c.fieldscfamilias[0] + " inner join cunidadmedida as t4 on t3." + v.c.fieldscfamilias[5] + " = t4." + v.c.fieldscunidadmedida[0] + " where t1." + v.c.fieldscrefacciones[0] + " = '" + cmbAgrega.SelectedValue + "'").ToString() : "");
            if (cmbAgrega.SelectedIndex > 0)
            {
                cantidad_actual = Double.Parse(v.getaData("select existencias from crefacciones where idrefaccion='" + cmbAgrega.SelectedValue.ToString() + "' and empresa = '" + empresa + "'").ToString());
            }
        }
    }
}
