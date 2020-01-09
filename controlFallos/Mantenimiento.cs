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

    public partial class Mantenimiento : Form
    {
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }
        validaciones v;
        int idUsuario, empresa, area, idreporte, idmecanico, idmecanicoApoyo, idmecaniAnterior, idmecanicoapoyoAnterior, grupoAnterior, EstatusAnterior, RefaccionesAnterior, idrefaccionAnterior, cantidadAnterior, idref, existenciaAnterior, idsupervisor, idsupervisorAnterior;
        string trabajoAnterior, observacionesAnterior, folioAnterior;
        bool editar, editarRefaccion, isexporting, aux;
        Thread excel;
        DataTable dt;
        iTextSharp.text.Font arial = FontFactory.GetFont("Calibri", 9, BaseColor.BLACK);
        iTextSharp.text.Font arial2 = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
        iTextSharp.text.Font encabezados = FontFactory.GetFont("Calibri", 14, iTextSharp.text.Font.BOLD);
        delegate void uno();
        delegate void dos();
        static bool res = true;
        string consultagral = "SET lc_time_names = 'es_ES';select t1.idReporteSupervicion as 'id',t1.Folio as 'FOLIO',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'ECONOMICO', upper(date_format(t1.fechareporte,'%W %d de %M del %Y')) as 'FECHA DE REPORTE',upper((select concat(coalesce(x1.appaterno,''),' ',coalesce(x1.apmaterno,''),' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCPersonal)) as 'SUPERVISOR',t1.KmEntrada as 'KILOMETRAJE DE UNIDAD', t1.HoraEntrada as 'HORA DEREPORTE',coalesce((select codfallo from cfallosesp as x2 where x2.idfalloEsp=t1.CodFallofkcfallosesp ),'')as 'CÓDIGO DE FALLO',coalesce(t1.DescFalloNoCod,'') as 'FALLO NO CODIFICADO',coalesce(t1.ObservacionesSupervision,'') as 'OBSERVACIONES',(select upper(concat(coalesce(x3.appaterno,''),' ',coalesce(x3.apmaterno,''),' ',x3.nombres)) from cpersonal as x3 where x3.idpersona=t2.MecanicofkPersonal) as 'MÉCANICO',(select upper(x4.nombreFalloGral) from cfallosgrales as x4 where x4.idFalloGral=t2.FalloGralfkFallosGenerales) as 'GRUPO DE FALLO',if(t2.StatusRefacciones is null,'',(if(t2.StatusRefacciones=1,'SI','NO'))) as 'SE REQUIEREN REFACCIONES',if(t2.estatus is null,'',(if(t2.Estatus=1,'EN PROCESO',(if(t2.estatus=2,'REPROGRAMADA','LIBERADA'))))) as 'ESTATUS',coalesce(upper(t2.TrabajoRealizado),'') as 'TRABAJO REALIZADO' from reportesupervicion as t1 left join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion inner join cunidades as t3 on t1.UnidadfkCUnidades=t3.idunidad  INNER JOIN careas AS t4 on t4.idarea=t3.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas ";
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
            cmbunidadb.DrawItem += v.combos_DrawItem;
            cmbgrupob.DrawItem += v.combos_DrawItem;
            cmbmecanicob.DrawItem += v.combos_DrawItem;
        }
        void minandmaxdate()
        {
            string[] date = v.getaData("select concat(MIN(" + v.c.fieldsreportesupervicion[3] + "),'|',MAX(" + v.c.fieldsreportesupervicion[3] + ")) as fechas from reportesupervicion;").ToString().Split('|');
            dtpfechade.MinDate = dtpfechaa.MinDate = DateTime.Parse(date[0]);
            dtpfechade.MaxDate = dtpfechaa.MaxDate = DateTime.Parse(date[1]);
        }
        void obtenerReportes()
        {
            string[] estatus = v.getaData("select concat(count(*),'|',(select count(*) from reportemantenimiento as t3 where t3.estatus='2'),'|',(select count(*) from reportemantenimiento as t5 where t5.estatus='3'),'|',(select count(*) from reportesupervicion as t5 left join reportemantenimiento as t1 on t5.idReporteSupervicion=t1.FoliofkSupervicion where t1.estatus is null))as r from reportemantenimiento as t1 inner join reportesupervicion as t2 on t1.FoliofkSupervicion=t2.idReporteSupervicion where t1.estatus='1';").ToString().Split('|');
            lblenproceso.Text = estatus[0];
            lblreprogramadas.Text = estatus[1];
            lblliberadas.Text = estatus[2];
            lblenesepera.Text = estatus[3];

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
            v.iniCombos("select " + v.c.fieldsfallosgrales[0] + " as id, upper(" + v.c.fieldsfallosgrales[1] + ") as grupo from cfallosgrales where " + v.c.fieldsfallosgrales[3] + "='" + empresa + "';", cmbgrupo, "id", "grupo", "--SELECCIONE UN GRUPO--");
            v.comboswithuot(cmbrefacciones, new string[] { "--seleccione una opción--", "se requieren refacciones", "no se requieren refacciones" });
            v.comboswithuot(cmbestatus, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
            v.comboswithuot(cmbestatusb, new string[] { "--seleccione--", "en proceso", "reprogramada", "liberada", });
            v.comboswithuot(cmbmesb, new string[] { "--seleccione mes--", "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" });
        }
        void busqueda()
        {
            v.iniCombos("SELECT t1." + v.c.fieldscunidades[0] + ",concat(t2." + v.c.fieldscareas[2] + ",LPAD(" + v.c.fieldscunidades[1] + ",4,'0')) as ECo FROM cunidades as t1 INNER JOIN careas as t2 ON t1." + v.c.fieldscunidades[3] + "= t2." + v.c.fieldscareas[0] + " order by eco;", cmbunidadb, "idunidad", "ECo", "--SELECCIONE UNIDAD--");
            v.iniCombos("select " + v.c.fieldsfallosgrales[0] + " as id, upper(" + v.c.fieldsfallosgrales[1] + ") as grupo from cfallosgrales where " + v.c.fieldsfallosgrales[3] + "='" + empresa + "';", cmbgrupob, "id", "grupo", "--SELECCIONE UN GRUPO--");
            v.iniCombos("select t1." + v.c.fieldscpersonal[0] + " as id, upper(concat(coalesce(t1." + v.c.fieldscpersonal[2] + ",''),' ',coalesce(t1." + v.c.fieldscpersonal[3] + ",''),' ',t1." + v.c.fieldscpersonal[4] + ")) as nombre from cpersonal as t1 inner join puestos as t2 on t2." + v.c.fieldspuestos[0] + "=t1." + v.c.fieldscpersonal[5] + " where t2." + v.c.fieldspuestos[1] + " like '%Mecánico%'", cmbmecanicob, "id", "nombre", "--SELECCIONE UN MECÁNICO");
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            if (!cambios())
            {
                remove();
                limpiar();
                combos();
                cargardatos();
            }
            else if (MessageBox.Show("Desea " + (editar ? "guardar los cambios" : "concluir con el registro"), validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                remove();
                limpiar();
                combos();
                cargardatos();
            }

        }

        private void txtmecanico_TextChanged(object sender, EventArgs e)
        {
            if (txtsupervisor.Focused)
                txtsupervisor_Validated(sender, e);
            if (peditar & editar)
                pguardar.Visible = (cambios() && !finaliza() ? true : false);
            if (pinsertar && EstatusAnterior != 3)
                pfinalizar.Visible = (finaliza() ? true : false);
        }

        void cargardatos()
        {
            MySqlDataAdapter cargar = new MySqlDataAdapter(consultagral, v.c.dbconection());
            DataSet ds = new DataSet();
            cargar.Fill(ds);
            dgvreportes.DataSource = ds.Tables[0];
            dgvreportes.Columns[1].Frozen = true;
            dgvreportes.Columns[0].Visible = false;
            dgvreportes.ClearSelection();
            v.c.dbconection().Close();
            dgvreportes.ClearSelection();
            minandmaxdate();
        }
        void cargarrefacciones()
        {
            MySqlDataAdapter r = new MySqlDataAdapter("select t1.idPedRef, t1.NumRefacc as 'NÚMERO',upper(t2.nombreRefaccion)as 'REFACCIÓN',t1.Cantidad as 'CANTIDAD', t1.EstatusRefaccion as 'EXISTENCIA',t1.CantidadEntregada as 'CANTIDAD ENTREGADA' from pedidosrefaccion as t1 inner join crefacciones as t2 on t1.RefaccionfkCRefaccion=t2.idrefaccion where FolioPedfkSupervicion='" + idreporte + "';", v.c.dbconection());
            DataSet ds = new DataSet();
            r.Fill(ds);
            dgvrefacciones.DataSource = ds.Tables[0];
            dgvrefacciones.Columns[0].Visible = false;
            v.c.dbconection().Close();
            dgvrefacciones.ClearSelection();
        }

        private void btnrefacciones_Click(object sender, EventArgs e)
        {
            lblexportar.Visible = !(gbrefacciones.Visible = true);
            v.iniCombos("select idrefaccion as id,upper(nombreRefaccion) as nombre from crefacciones where status='1';", cmbrefaccion, "id", "nombre", "--seleccione--");
        }

        private void btnregresar_Click(object sender, EventArgs e)
        {
            gbrefacciones.Visible = !(lblexportar.Visible = true);
            cmbrefacciones.Enabled = (Convert.ToInt32(v.getaData("select count(*)  from pedidosrefaccion where FolioPedfkSupervicion='" + idreporte + "';")) == 0 ? true : false);
            limpiarRefaccion();
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            if (!editarRefaccion)
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
            else
            {
                if (v.c.insertar("update pedidosrefaccion set RefaccionfkCRefaccion='" + cmbrefaccion.SelectedValue + "', Cantidad='" + txtcantidad.Text + "' where idPedRef='" + idref + "';"))
                {
                    MessageBox.Show("Los datos de actualizaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiarRefaccion();
                }
            }
        }

        private void txtcantidad_TextChanged(object sender, EventArgs e)
        {
            if (editarRefaccion && peditar)
                pagregar.Visible = ((idrefaccionAnterior != Convert.ToInt32(cmbrefaccion.SelectedValue) || cantidadAnterior != Convert.ToInt32((string.IsNullOrWhiteSpace(txtcantidad.Text) ? "0" : txtcantidad.Text)) && cmbrefaccion.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtcantidad.Text)) ? true : false);
        }

        private void txtcantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.numerosDecimales(e);
        }
        string timetowait(DateTime initialdate, DateTime finaldate)
        {
            TimeSpan ts = finaldate.Subtract(initialdate);
            return (ts.Days + " días, " + ts.Hours + " horas, " + ts.Minutes + " minutos").ToUpper();

        }
        private void txttrabajo_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.enGeneral(e);
        }

        private void txtfoliob_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.letrasynumerossinespacios(e);
        }
        bool validarefacciones()
        {
            if (cmbrefacciones.SelectedIndex == 2 || (cmbrefacciones.SelectedIndex == 1 && Convert.ToInt32(v.getaData("select count(*) from pedidosrefaccion where FolioPedfkSupervicion='" + idreporte + "';")) > 0))
                return true;
            else
            {
                MessageBox.Show("No hay refacciones agregadas, en el reporte", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private void btnfinalizar_Click(object sender, EventArgs e)
        {
            if (validarefacciones())
            {
                if (Convert.ToInt32(v.getaData("select count(*) from pedidosrefaccion where FolioPedfkSupervicion='" + idreporte + "';")) == 0 || Convert.ToInt32(v.getaData("select count(*) from pedidosrefaccion where FolioPedfkSupervicion='" + idreporte + "' and Cantidad!=CantidadEntregada;")) == 0)
                {
                    FormContraFinal o = new FormContraFinal(empresa, area, this, v);
                    o.Owner = this;
                    if (o.ShowDialog() == DialogResult.OK)
                        if (v.c.insertar("update reportemantenimiento set Estatus='3',PersonaFinal='" + o.id + "', FechaHoraT=now() where FoliofkSupervicion='" + idreporte + "';"))
                        {
                            MessageBox.Show("El reporte se finalizo de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            limpiar();
                            cargardatos();
                        }
                }
                else
                {
                    MessageBox.Show("No se puede finalizar el reporte debido a que faltan refacciones por entregar", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbestatus.SelectedValue = EstatusAnterior;
                }
            }
        }

        private void Mantenimiento_FormClosing(object sender, FormClosingEventArgs e)
        {
            remove();
        }

        private void cmbestatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbestatus.SelectedIndex == 3 && !finaliza() && EstatusAnterior != 3)
            {
                MessageBox.Show("El reporte no se puede finalizar porque aún faltan campos por llenar ", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbestatus.SelectedValue = EstatusAnterior;
            }
        }
        void limpiarbusqueda()
        {
            txtfoliob.Clear();
            cmbunidadb.SelectedIndex = cmbmecanicob.SelectedIndex = cmbestatusb.SelectedIndex = cmbmesb.SelectedIndex = cmbgrupob.SelectedIndex = 0;
            cbrango.Checked = false;
            dtpfechaa.Value = dtpfechade.Value = dtpfechade.MaxDate;
        }
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtfoliob.Text) && cmbunidadb.SelectedIndex == 0 && cmbmecanicob.SelectedIndex == 0 && cmbestatusb.SelectedIndex == 0 && cmbmesb.SelectedIndex == 0 && cmbgrupob.SelectedIndex == 0 && !cbrango.Checked)
                MessageBox.Show("Seleccione un criterio de búsqueda", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                string wheres = "";
                if (cbrango.Checked)
                    wheres = (wheres == "" ? " where t1.FechaReporte between '" + dtpfechade.Value.ToString("yyyy-MM-dd") + "' and '" + dtpfechaa.Value.ToString("yyyy-MM-dd") + "'" : " and (t1.FechaReporte between '" + dtpfechade.Value.ToString("yyyy-MM-dd") + "' and '" + dtpfechaa.Value.ToString("yyyy-MM-dd") + "')");
                if (cmbunidadb.SelectedIndex > 0)
                    wheres = (wheres == "" ? " where t1.UnidadfkCUnidades='" + cmbunidadb.SelectedValue + "'" : " and t1.UnidadfkCUnidades='" + cmbunidadb.SelectedValue + "'");
                if (cmbmecanicob.SelectedIndex > 0)
                    wheres = (wheres == "" ? " where t2.MecanicofkPersonal='" + cmbmecanicob.SelectedValue + "'" : " and MecanicofkPersonal='´" + cmbmecanicob.SelectedValue + "'");
                if (cmbestatusb.SelectedIndex > 0)
                    wheres = (wheres == "" ? " where t2.Estatus='" + cmbestatusb.SelectedValue + "'" : " and t2.Estatus='" + cmbestatusb.SelectedValue + "'");
                if (cmbgrupob.SelectedIndex > 0)
                    wheres = (wheres == "" ? " where (select t1.idFalloGral from cfallosgrales as t1 inner join cdescfallo as t2 on t1.idFalloGral=t2.falloGralfkcfallosgrales inner join catcategorias as t3 on t2.iddescfallo=t3.subgrupofkcdescfallo inner join cfallosesp as t4 on t3.idcategoria=t4.descfallofkcdescfallo where t1.CodFallofkcfallosesp=t4.idfalloEsp)='" + cmbgrupob.SelectedValue + "'" : " and (select t1.idFalloGral from cfallosgrales as t1 inner join cdescfallo as t2 on t1.idFalloGral=t2.falloGralfkcfallosgrales inner join catcategorias as t3 on t2.iddescfallo=t3.subgrupofkcdescfallo inner join cfallosesp as t4 on t3.idcategoria=t4.descfallofkcdescfallo where t1.CodFallofkcfallosesp=t4.idfalloEsp)='" + cmbgrupob.SelectedValue + "'");
                if (cmbmesb.SelectedIndex > 0)
                    wheres = (wheres == "" ? " where date_format(t1.FechaReporte,'%c')='" + cmbmesb.SelectedValue + "' and date_format(t1.FechaReporte,'%Y')=date_format(now(),'%Y')" : "");
                MySqlDataAdapter da = new MySqlDataAdapter(consultagral + wheres, v.c.dbconection());
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvreportes.DataSource = ds.Tables[0];
                if (dgvreportes.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron resultados", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cargardatos();
                }
                else
                {
                    pactualizar.Visible = pexcel.Visible = true;
                    btnexportar.Visible = (isexporting ? false : true);
                }
                limpiarbusqueda();
            }
        }
        void exporting()
        {
            if (!isexporting)
                pexcel.Visible = false;
            else
                aux = true;

        }
        private void btnactualizar_Click(object sender, EventArgs e)
        {
            exporting();
            pactualizar.Visible = false;
            cargardatos();
        }

        private void cbrango_CheckedChanged(object sender, EventArgs e)
        {
            if (cbrango.Checked)
            {
                dtpfechaa.Enabled = dtpfechade.Enabled = !(cmbmesb.Enabled = false);
                cmbmesb.SelectedIndex = 0;
            }
            else
            {
                cmbmesb.Enabled = !(dtpfechade.Enabled = dtpfechaa.Enabled = false);
                dtpfechaa.Value = dtpfechade.Value = dtpfechade.MaxDate;
            }
        }

        void remove()
        {
            if (Convert.ToInt32(v.getaData("select count(*) from reportemantenimiento where FoliofkSupervicion='" + idreporte + "';")) == 0)
                v.getaData("delete from pedidosrefaccion where FolioPedfkSupervicion='" + idreporte + "'");
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            excel = new Thread(new ThreadStart(exportar_excel));
            excel.Start();
        }
        void inicio()
        {
            btnexportar.Visible = !(pbgif.Visible = true);
            lblexcel.Text = "Exportando";
        }
        void termino()
        {
            pbgif.Visible = isexporting = false;
            lblexcel.Text = "Exportar";
            pexcel.Visible = (aux ? false : true);
        }
        void exportar_excel()
        {
            if (dgvreportes.Rows.Count > 0)
            {
                isexporting = true;
                dt = (DataTable)dgvreportes.DataSource;
                if (this.InvokeRequired)
                {
                    uno delega = new uno(inicio);
                    this.Invoke(delega);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                h.Worksheet sheet = X.ActiveSheet;
                X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    h.Range rng = (h.Range)sheet.Cells[1, i];
                    sheet.Cells[1, i] = dt.Columns[i].ColumnName.ToUpper();
                    rng.Interior.Color = System.Drawing.Color.Crimson;
                    rng.Borders.Color = System.Drawing.Color.Black;
                    rng.Font.Color = System.Drawing.Color.White;
                    rng.Cells.Font.Name = "Calibri";
                    rng.Cells.Font.Size = 12;
                    rng.Font.Bold = true;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        try
                        {
                            h.Range rng = (h.Range)sheet.Cells[i + 2, j];
                            sheet.Cells[i + 2, j] = dt.Rows[i][j].ToString();
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
                if (this.InvokeRequired)
                {
                    dos delega2 = new dos(termino);
                    this.Invoke(delega2);
                }
                excel.Abort();
            }
            else
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnpdf_Click(object sender, EventArgs e)
        {
            pdf();
        }
        void pdf()
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
        }
        PdfPTable general()
        {
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';select upper(concat(t1.Folio,'|',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))),'|',t1.KmEntrada,'|',date_format(t1.FechaReporte,'%W %d de %M del %Y'),'/',t1.HoraEntrada,'|',if(t1.DescFalloNoCod is not null,t1.DescFalloNoCod,(select concat(x3.descfallo,'|',x1.codfallo) from cfallosesp as x1 inner join catcategorias as x2 on x2.idcategoria=x1.descfallofkcdescfallo inner join cdescfallo as x3 on x2.subgrupofkcdescfallo=x3.iddescfallo where x1.idfalloEsp=t1.CodFallofkcfallosesp)),'|',(select concat(coalesce(x6.appaterno,''),' ',coalesce(x6.apmaterno,''),' ',x6.nombres) from cpersonal as x6 where x6.idpersona=t1.SupervisorfkCPersonal),'|',coalesce(t1.ObservacionesSupervision,''),'|',coalesce((select x7.nombreFalloGral from cfallosgrales as x7 where x7.idFalloGral=t2.FalloGralfkFallosGenerales),''),'|',coalesce(t2.estatus,0),'|',coalesce((select concat(coalesce(x8.appaterno,''),' ',coalesce(x8.apmaterno,''),' ',x8.nombres) from cpersonal as x8 where x8.idpersona=t2.MecanicofkPersonal),''),'|',coalesce((select concat(coalesce(x9.appaterno,''),' ',coalesce(x9.apmaterno,''),' ',x9.nombres) from cpersonal as x9 where x9.idpersona=t2.MecanicoApoyofkPersonal),''),'|',coalesce(t2.FechaHoraI,'00:00:00'),' / ',coalesce(t2.FechaHoraT,'00:00:00'),'|',coalesce(t2.TrabajoRealizado,''),'|',coalesce(t2.ObservacionesM,''))) as r from reportesupervicion as t1 left join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion inner join cunidades as t3 on t1.UnidadfkCUnidades=t3.idunidad  INNER JOIN careas AS t4 on t4.idarea=t3.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas where idReporteSupervicion='" + idreporte + "';").ToString().Split('|');
            PdfPTable tabla = new PdfPTable(20);
            tabla.WidthPercentage = 100;
            bool havecode = datos.Length > 14;
            tabla.AddCell(v.valorCampo("DATOS DE SUPERVISIÓN", 20, 1, 0, encabezados));
            tabla.AddCell(v.valorCampo("\n\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("FOLIO: ", 2, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[0], 3, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 2, 0, 0, arial));
            tabla.AddCell(v.valorCampo("UNIDAD: ", 3, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[1], 3, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 2, 0, 0, arial));
            tabla.AddCell(v.valorCampo("KILOMETRAJE: ", 3, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[2], 2, 1, 1, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("FECHA / HORA: ", 4, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[3], 10, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 6, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo((havecode ? "SUBGRUPO: " : "FALLO NO CODIFICADO: "), (havecode ? 3 : 5), 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[4], (havecode ? 5 : 10), 1, 1, arial));
            tabla.AddCell(v.valorCampo("", (havecode ? 12 : 5), 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            if (havecode)
            {
                tabla.AddCell(v.valorCampo("CÓDIGO DE FALLO: ", 5, 0, 0, arial2));
                tabla.AddCell(v.valorCampo(datos[5], 6, 1, 1, arial));
                tabla.AddCell(v.valorCampo("", 9, 0, 0, arial));
                tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            }
            tabla.AddCell(v.valorCampo("SUPERVISOR: ", 3, 0, 0, arial2));
            tabla.AddCell(v.valorCampo((havecode ? datos[6] : datos[5]), 8, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 9, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("OBSERVACIONES: ", 4, 0, 0, arial2));
            tabla.AddCell(v.valorCampo((havecode ? datos[7] : datos[6]), 16, 0, 1, arial));
            tabla.AddCell(v.valorCampo("\n\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("DATOS DE MANTENIMIENTO", 20, 1, 0, encabezados));
            tabla.AddCell(v.valorCampo("\n\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("GRUPO DE FALLO: ", 4, 0, 0, arial2));
            tabla.AddCell(v.valorCampo((havecode ? datos[8] : datos[7]), 4, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 2, 0, 0, arial));
            tabla.AddCell(v.valorCampo("ESTATUS DE MANTENIMIENTO: ", 6, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(changestatus(havecode ? Convert.ToInt32(datos[9]) : Convert.ToInt32(datos[8])), 4, 1, 1, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("MECÁNICO: ", 3, 0, 0, arial2));
            tabla.AddCell(v.valorCampo((havecode ? datos[10] : datos[9]), 8, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 9, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("MECÁNICO DE APOYO: ", 5, 0, 0, arial2));
            tabla.AddCell(v.valorCampo((havecode ? datos[11] : datos[10]), 8, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 7, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("TIEMPO DE ESPERA: ", 4, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(timetowait(DateTime.Parse(v.getaData("select concat(FechaReporte,' ',HoraEntrada) from reportesupervicion where idReporteSupervicion='" + idreporte + "'").ToString()), DateTime.Parse(v.getaData("select concat(FechaHoraI) from reportemantenimiento where FoliofkSupervicion='" + idreporte + "'").ToString())), 8, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 8, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("FECHA / HORA INICIO - TERMINO: ", 6, 0, 0, arial2));
            tabla.AddCell(v.valorCampo((havecode ? datos[12] : datos[11]), 7, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 7, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            return tabla;
        }
        PdfPTable unidad()
        {
            string[] datos = v.getaData("select concat(identificador,LPAD(consecutivo,4,'0'),'|',coalesce(t1.bin,''),'|',coalesce(t1.marca,''),'|',coalesce(t1.nmotor,''),'|',coalesce(t1.ntransmision,''),'|',coalesce(t4.TrabajoRealizado,''),'|',coalesce(t4.estatus,'0')) from cunidades as t1 inner join reportesupervicion as t2 on t2.UnidadfkCUnidades=t1.idunidad inner join careas as t3 on t3.idarea=t1.areafkcareas left join reportemantenimiento as t4 on t4.FoliofkSupervicion=t2.idReporteSupervicion where t2.idReporteSupervicion='" + idreporte + "';").ToString().Split('|');
            PdfPTable tabla = new PdfPTable(20);
            tabla.WidthPercentage = 100;
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial2));
            tabla.AddCell(v.valorCampo("UNIDAD: ", 3, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[0], 3, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 1, 0, 0, arial));
            tabla.AddCell(v.valorCampo("BIN: ", 2, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[1], 4, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 1, 0, 0, arial));
            tabla.AddCell(v.valorCampo("MARCA: ", 2, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[2], 4, 1, 1, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("N° DE SERIE DE MOTOR: ", 6, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[3], 8, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 6, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("N° DE SERIE DE TRANSMISIÓN: ", 6, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[4], 8, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 6, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("ESTATUS: ", 2, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(changestatus(Convert.ToInt32(datos[6])), 5, 1, 1, arial));
            tabla.AddCell(v.valorCampo("", 13, 0, 0, arial));
            tabla.AddCell(v.valorCampo("\n", 20, 0, 0, arial));
            tabla.AddCell(v.valorCampo("TRABAJO REALIZADO: ", 5, 0, 0, arial2));
            tabla.AddCell(v.valorCampo(datos[5], 15, 1, 1, arial));
            return tabla;
        }
        public void Refacciones(Document document)
        {
            int i, j;
            PdfPTable datatable = new PdfPTable(dgvrefacciones.ColumnCount);
            datatable.DefaultCell.Padding = 4;
            float[] headerwidths = { 0, 50, 200, 50, 100, 100 };
            datatable.SetWidths(headerwidths);
            datatable.WidthPercentage = 100;
            datatable.DefaultCell.BorderWidth = 1;
            datatable.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(234, 231, 231);
            datatable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            for (i = 0; i < dgvrefacciones.ColumnCount; i++)
            {
                datatable.AddCell(new Phrase(dgvrefacciones.Columns[i].HeaderText.ToString(), FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD)));
            }
            datatable.HeaderRows = 1;
            datatable.DefaultCell.BorderWidth = 1;
            for (i = 0; i < dgvrefacciones.RowCount; i++)
            {
                for (j = 0; j < dgvrefacciones.ColumnCount; j++)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(dgvrefacciones[j, i].Value.ToString(), FontFactory.GetFont("ARIAL", 8)));
                    celda.HorizontalAlignment = celda.VerticalAlignment = 1;
                    celda.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                    if (dgvrefacciones[j, i].Value != null)
                        datatable.AddCell(celda);
                }
                datatable.CompleteRow();
            }
            document.Add(datatable);
        }

        private void txtsupervisor_Validated(object sender, EventArgs e)
        {
            lblsupmant.Text = (Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtsupervisor.Text.Trim()) + "';")) == 0 ? "" : v.getaData("select upper(concat(coalesce(t1.appaterno,''),' ',coalesce(t1.apmaterno,''),' ',t1.nombres)) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtsupervisor.Text.Trim()) + "';").ToString());
            idsupervisor = ((Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtsupervisor.Text.Trim()) + "';")) == 0 ? 0 : Convert.ToInt32(v.getaData("select t1.idpersona from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtsupervisor.Text.Trim()) + "';"))));
            mecanicosiguales(txtsupervisor, lblsupmant);
        }

        private void dgvrefacciones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvrefacciones.Columns[e.ColumnIndex].Name == "EXISTENCIA")
                e.CellStyle.BackColor = (e.Value.ToString() == "EXISTENCIA" ? Color.PaleGreen : e.Value.ToString() == "SIN EXISTENCIA" ? Color.LightCoral : e.Value.ToString() == "INCOMPLETO" ? Color.Orange : Color.Khaki);
        }

        private void dgvreportes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvreportes.Columns[e.ColumnIndex].Name == "ESTATUS")
                e.CellStyle.BackColor = (e.Value.ToString() == "EN PROCESO" ? Color.Khaki : e.Value.ToString() == "LIBERADA" ? Color.PaleGreen : e.Value.ToString() == "REPROGRAMADA" ? Color.LightCoral : Color.LightBlue);
        }

        public string changestatus(int status)
        {
            string estatus = (status == 0 ? "" : status == 1 ? "EN PROCESO" : status == 2 ? "REPROGRAMADA" : "LIBERADA");
            return estatus;
        }
        private void dgvreportes_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void dgvrefacciones_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            string[] drefaccion = v.getaData("select concat(RefaccionfkCRefaccion,'|',Cantidad) from pedidosrefaccion where idPedRef='" + (idref = Convert.ToInt32(dgvrefacciones.Rows[e.RowIndex].Cells[0].Value)) + "';").ToString().Split('|');
            if (Convert.ToInt32(v.getaData("select if(cantidad=cantidadentregada,1,0)as r from pedidosrefaccion where idPedRef='" + idref + "';")) == 0)
            {
                cmbrefaccion.SelectedValue = idrefaccionAnterior = Convert.ToInt32(drefaccion[0]);
                txtcantidad.Text = (cantidadAnterior = Convert.ToInt32(drefaccion[1])).ToString();
            }
            else MessageBox.Show("La refacción no puede ser editada debido a que ya fue entregada por el área de almacén", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            cmbgrupo.Enabled = cmbestatus.Enabled = cmbrefacciones.Enabled = txttrabajo.Enabled = txtmecanico.Enabled = txtmecanicoapoyo.Enabled = txtsupervisor.Enabled = txtfoliof.Enabled = txtobservacionesm.Enabled = enabled;
        }
        void limpiar()
        {
            txtmecanico.Clear();
            txtsupervisor.Clear();
            txtfoliof.Clear();
            txtmecanicoapoyo.Clear();
            txttrabajo.Clear();
            txtobservacionesm.Clear();
            cmbgrupo.SelectedIndex = cmbestatus.SelectedIndex = cmbrefacciones.SelectedIndex = idmecanico = idmecaniAnterior = idmecanicoapoyoAnterior = grupoAnterior = RefaccionesAnterior = existenciaAnterior = idmecanicoApoyo = idreporte = 0;
            lblfolio.Text = lblunidad.Text = lblkilometraje.Text = lblsupervisor.Text = lblhorar.Text = lblfechas.Text = lblobservacioness.Text = lblhimant.Text = lbltiempoespera.Text = lbltiempototal.Text = lblmecanico.Text = lblmapoyo.Text = folioAnterior = trabajoAnterior = observacionesAnterior = lblsupmant.Text = lblhtmant.Text = "";
            status(editar = pguardar.Visible = pcancelar.Visible = pPdf.Visible = false);
            obtenerReportes();
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (v.camposmant(txtmecanico.Text, Convert.ToInt32(cmbgrupo.SelectedValue), area, Convert.ToInt32(cmbrefacciones.SelectedValue), Convert.ToInt32(cmbestatus.SelectedValue), txtfoliof.Text, idreporte, EstatusAnterior))
                if (!editar)
                {
                    if (v.c.insertar("Insert Into reportemantenimiento (FoliofkSupervicion,FalloGralfkFallosGenerales,TrabajoRealizado,MecanicofkPersonal,FechaHoraI,Estatus,empresa,StatusRefacciones)values('" + idreporte + "','" + cmbgrupo.SelectedValue + "','" + txttrabajo.Text.Trim() + "','" + idmecanico + "',now(),'" + cmbestatus.SelectedValue + "','" + empresa + "','" + cmbrefacciones.SelectedValue + "')"))
                    {
                        MessageBox.Show("Se insertaron los datos de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        cargardatos();
                    }
                }
                else
                {
                    bool add = newinformation(), aux;
                    observacionesEdicion o = new observacionesEdicion(v);
                    o.Owner = this;
                    if (!add)
                        aux = o.ShowDialog() == DialogResult.OK;
                    if (v.c.insertar("update reportemantenimiento set FalloGralfkFallosGenerales='" + cmbgrupo.SelectedValue + "',TrabajoRealizado='" + txttrabajo.Text.Trim() + "',Estatus='" + cmbestatus.SelectedValue + "',StatusRefacciones='" + cmbrefacciones.SelectedValue + "',FolioFactura='" + txtfoliof.Text + "',ObservacionesM='" + txtobservacionesm.Text.Trim() + "',SupervisofkPersonal='" + idsupervisor + "' where FoliofkSupervicion='" + idreporte + "'"))
                        MessageBox.Show("La información se " + (add ? "agrego" : "modifico") + " de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                    cargardatos();
                }
        }
        bool newinformation()
        {
            bool res = false;
            if ((txttrabajo.Text.Trim() != trabajoAnterior && string.IsNullOrWhiteSpace(trabajoAnterior)) || (folioAnterior != txtfoliof.Text && string.IsNullOrWhiteSpace(folioAnterior)) || (idsupervisor != idsupervisorAnterior && idsupervisorAnterior == 0) || (txtobservacionesm.Text.Trim() != observacionesAnterior && string.IsNullOrWhiteSpace(observacionesAnterior)))
                res = true;
            return res;
        }
        private void cmbrefacciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbrefacciones.SelectedIndex > 0 && EstatusAnterior < 3)
                txtfoliof.Enabled = btnrefacciones.Visible = (Convert.ToInt32(cmbrefacciones.SelectedValue) == 1 ? true : false);
        }

        void mecanicosiguales(TextBox txt, Label lbl)
        {
            if (((idmecanico > 0 && (idmecanicoApoyo > 0 || idsupervisor > 0)) || (idmecanicoApoyo > 0 && idsupervisor > 0)) && (idmecanicoApoyo == idmecanico || idmecanico == idsupervisor || idsupervisor == idmecanicoApoyo))
            {
                txt.Clear();
                lbl.Text = "";
                MessageBox.Show("El" + (idmecanico == idmecanicoApoyo ? " mecánico y mecánico de apoyo" : idmecanico == idsupervisor ? " mecánico y supervisor" : " mecanico de apoyo y supervisor") + " no pueden ser la misma persona", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtmecanico_Validated(object sender, EventArgs e)
        {
            lblmecanico.Text = (Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';")) == 0 ? "" : v.getaData("select upper(concat(coalesce(t1.appaterno,''),' ',coalesce(t1.apmaterno,''),' ',t1.nombres)) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';").ToString());
            idmecanico = ((Convert.ToInt32(v.getaData("select count(*) from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';")) == 0 ? 0 : Convert.ToInt32(v.getaData("select t1.idpersona from cpersonal as t1 inner join datosistema as t2 on t1.idpersona=t2.usuariofkcpersonal where t2.password='" + v.Encriptar(txtmecanico.Text.Trim()) + "';"))));
            mecanicosiguales(txtmecanico, lblmecanico);
        }
        bool finaliza()
        {
            bool res = false;
            if (Convert.ToInt32(cmbestatus.SelectedValue) == 3 && idmecanico > 0 && cmbgrupo.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txttrabajo.Text) && (cmbrefacciones.SelectedIndex == 2 || (Convert.ToInt32(cmbrefacciones.SelectedIndex) == 1 && !string.IsNullOrWhiteSpace(txtfoliof.Text))) && cmbestatus.SelectedIndex > 0)
                res = true;
            return res;
        }
        bool cambios()
        {
            bool res = false;
            if ((idmecaniAnterior != idmecanico || idsupervisorAnterior != idsupervisor || idmecanicoApoyo != idmecanicoapoyoAnterior || Convert.ToInt32(cmbgrupo.SelectedValue) != grupoAnterior || trabajoAnterior != txttrabajo.Text.Trim() || (observacionesAnterior ?? "") != txtobservacionesm.Text.Trim() || EstatusAnterior != Convert.ToInt32(cmbestatus.SelectedValue) || folioAnterior != txtfoliof.Text) && idreporte > 0)
                res = true;
            return res;
        }
        void doblegridview(DataGridViewCellEventArgs e)
        {
            string[] datos = v.getaData("SET lc_time_names = 'es_ES';select upper(concat(t1.folio,'|',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))),'|',date_format(t1.fechareporte,'%W %d de %M del %Y'),'|',(select concat(coalesce(x1.appaterno,''),' ',coalesce(x1.apmaterno,''),' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCPersonal),'|',t1.KmEntrada,'|',coalesce(t1.CodFallofkcfallosesp,0),'|',coalesce(t1.DescFalloNoCod,''),'|',coalesce(t1.ObservacionesSupervision,''),'|',t1.HoraEntrada,'|',coalesce(t2.MecanicofkPersonal,0),'|',coalesce(t2.MecanicoApoyofkPersonal,0),'|',coalesce(t2.FalloGralfkFallosGenerales,0),'|',coalesce(t2.StatusRefacciones,0),'|',coalesce(t2.TrabajoRealizado,''),'|',coalesce(t2.Estatus,0),'|',coalesce(t2.FechaHoraI,''),'|',coalesce(t2.FechaHoraT,''),'|',coalesce(t2.ObservacionesM,''),'|',coalesce(t2.FolioFactura,''),'|',coalesce(t2.SupervisofkPersonal,0)))as r from reportesupervicion as t1 left join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion inner join cunidades as t3 on t1.UnidadfkCUnidades=t3.idunidad  INNER JOIN careas AS t4 on t4.idarea=t3.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas where t1.idReporteSupervicion='" + (idreporte = Convert.ToInt32(dgvreportes.Rows[e.RowIndex].Cells[0].Value)) + "';").ToString().Split('|');
            status(pcancelar.Visible = true);
            if (Convert.ToInt32(v.getaData("select count(*) from reportemantenimiento where FoliofkSupervicion='" + idreporte + "';")) == 0 || Convert.ToInt32(datos[14]) == 2)
            {
                v.comboswithuot(cmbestatus, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada" });
                editar = false;
            }

            else
            {
                v.comboswithuot(cmbestatus, new string[] { "--seleccione un estatus--", "en proceso", "reprogramada", "liberada" });
                editar = true;
            }
            lblfolio.Text = datos[0];
            lblunidad.Text = datos[1];
            lblfechas.Text = datos[2];
            lblsupervisor.Text = datos[3];
            lblkilometraje.Text = datos[4];
            lblobservacioness.Text = datos[7];
            lblhorar.Text = datos[8];
            lblmecanico.Text = ((idmecaniAnterior = idmecanico = Convert.ToInt32(datos[9])) > 0 ? v.getaData("select upper(concat(coalesce(appaterno,''),' ',coalesce(apmaterno,''),' ',nombres)) from cpersonal where idpersona='" + idmecanico + "'").ToString() : "");
            lblmapoyo.Text = ((idmecanicoapoyoAnterior = idmecanicoApoyo = Convert.ToInt32(datos[10])) > 0 ? v.getaData("select upper(concat(coalesce(appaterno,''),' ',coalesce(apmaterno,''),' ',nombres)) from cpersonal where idpersona='" + idmecanicoApoyo + "'").ToString() : "");
            cmbgrupo.SelectedValue = grupoAnterior = Convert.ToInt32(datos[11]);
            cmbrefacciones.SelectedValue = RefaccionesAnterior = Convert.ToInt32(datos[12]);
            txttrabajo.Text = trabajoAnterior = datos[13];
            cmbestatus.SelectedValue = EstatusAnterior = Convert.ToInt32(datos[14]);
            lblhimant.Text = (string.IsNullOrWhiteSpace(datos[15]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : datos[15]);
            lblhtmant.Text = datos[16];
            lbltiempoespera.Text = timetowait(DateTime.Parse(v.getaData("select concat(t2.FechaReporte,' ',t2.HoraEntrada) from reportesupervicion as t2 where t2.idReporteSupervicion='" + idreporte + "';").ToString()), (string.IsNullOrWhiteSpace(datos[15]) ? DateTime.Now : DateTime.Parse(datos[15])));
            lbltiempototal.Text = (string.IsNullOrWhiteSpace(datos[16]) ? "" : timetowait(DateTime.Parse(v.getaData("select concat(t2.FechaReporte,' ',t2.HoraEntrada) from reportesupervicion as t2 where t2.idReporteSupervicion='" + idreporte + "';").ToString()), DateTime.Parse(v.getaData("select FechaHoraT from reportemantenimiento where FoliofkSupervicion='" + idreporte + "';").ToString())));
            txtobservacionesm.Text = observacionesAnterior = datos[17];
            txtfoliof.Text = folioAnterior = datos[18];
            lblsupmant.Text = ((idsupervisorAnterior = idsupervisor = Convert.ToInt32(datos[19])) == 0 ? "" : v.getaData("select upper(concat(coalesce(appaterno,''),' ',coalesce(apmaterno,''),' ',nombres)) from cpersonal where idpersona='" + idsupervisor + "'").ToString());
            pguardar.Visible = (Convert.ToInt32(v.getaData("select count(*) from reportemantenimiento where FoliofkSupervicion='" + idreporte + "';")) == 0 ? true : false);
            if (EstatusAnterior == 3)
                status(btnrefacciones.Visible = !(pPdf.Visible = true));
            txtsupervisor.Enabled = (Convert.ToInt32(datos[19]) == 0 ? true : false);
            cargarrefacciones();
        }
        private void dgvreportes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            limpiar();
            if (!cambios())
                doblegridview(e);
            else if (MessageBox.Show("Desea " + (editar ? "guardar los cambios" : "concluir con el registro"), validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                doblegridview(e);

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
            obtenerReportes();
            busqueda();
        }
    }
}
