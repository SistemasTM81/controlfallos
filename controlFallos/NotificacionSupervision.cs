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
    public partial class NotificacionSupervision : Form
    {
        bool mientras = true;
        int opcion;
        validaciones v ;
        ThreadStart ts;
        Thread t;
        string sql = "";
        int empresa, area;
        public NotificacionSupervision(int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            tbnotif.ColumnAdded += v.paraDataGridViews_ColumnAdded;
            tbnotif.CellDoubleClick += new DataGridViewCellEventHandler(reportes_CellContentDoubleClick);
            this.empresa = empresa;
            this.area = area;
        }

        private void NotificacionSupervision_Load(object sender, EventArgs e)
        {
            Hilo(0);
            buscarMeses();
        }
        void cargarReportes()
        {
            tbnotif.DataSource = null;
            tbnotif.ClearSelection();
            DataTable ds = (DataTable) v.getData(sql);
            tbnotif.DataSource = ds;
            if (mientras) if (opcion == 1 && tbnotif.Columns[0].Visible) tbnotif.Columns[0].Visible = false;

        }
        private void reportes_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {

                menuPrincipal m = (menuPrincipal)this.Owner;
                string idfolio = tbnotif.Rows[e.RowIndex].Cells[0].Value.ToString();
                mientras = false;
                if (opcion == 0) m.TraerVariable(idfolio); else m.paraPErsonal(idfolio);

                if (m.form.GetType() == typeof(catPersonal))
                    this.Close();
            }
        }
        private void reporttes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbnotif.Columns[e.ColumnIndex].Name == "TIPO DE FALLO")
            {
                if (Convert.ToString(e.Value) == "PREVENTIVO")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "CORRECTIVO")
                    {
                        e.CellStyle.BackColor = Color.Khaki;
                    }
                    else
                    {
                        if (Convert.ToString(e.Value) == "REITERATIVO")
                        {
                            e.CellStyle.BackColor = Color.LightCoral;
                        }
                        else
                        {
                            if (Convert.ToString(e.Value) == "REPROGRAMADO")
                            {
                                e.CellStyle.BackColor = Color.LightBlue;
                            }
                            else
                            {
                                if (Convert.ToString(e.Value) == "SEGUIMIENTO")
                                {
                                    e.CellStyle.BackColor = Color.FromArgb(246, 144, 123);
                                }
                            }
                        }
                    }
                }
            }

            if (this.tbnotif.Columns[e.ColumnIndex].Name == "Estatus".ToUpper())
            {
                if (Convert.ToString(e.Value) == "En Proceso".ToUpper())
                {

                    e.CellStyle.BackColor = Color.Khaki;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "LIBERADA".ToUpper())
                    {

                        e.CellStyle.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        if (Convert.ToString(e.Value) == "REPROGRAMADA".ToUpper())
                        {

                            e.CellStyle.BackColor = Color.LightCoral;
                        }
                    }
                }
            }
        }

        delegate void delegado();
        void paraReportes()
        {

            while (mientras)
            {
                reportes();
                Thread.Sleep(500);
            }
        }
        bool buscar = false;
        string contador = "";
        void reportes()
        {
            try
            {
                if (InvokeRequired)
                {
                    delegado t = new delegado(reportes);
                    Invoke(t);
                }
                else
                {
                    MySqlConnection dbcon = new MySqlConnection("Server = 192.168.1.67; user=73__p0_UJ2020; password = Upt_FJU2016; database =sistrefaccmant;port=3306");
                    dbcon.Open();

                    if (!buscar)
                    {
                        if (opcion == 0)
                        {

                            if (empresa == 1 && area == 1)
                            {
                                contador = "SELECT COUNT(*) FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad INNER JOIN careas as t5 ON t4.areafkcareas=t5.idarea WHERE t1.seen = 0 and (t1.Estatus='2' or t1.Estatus='3' ) AND t3.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                            }
                            else if (empresa == 2 && area == 1)
                            {
                                contador = "SELECT COUNT(*) FROM reportesupervicion AS t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cpersonal as t3 ON t1.SupervisorFKCPersonal = t3.idpersona INNER JOIN careas as t4 ON t2.areafkcareas=t4.idarea WHERE t1.seen =0 AND t1.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                            }
                        }
                        else
                        {
                            if (empresa == 1 && area == 1)
                            {
                                contador = "SELECT COUNT(idvigencia) FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona WHERE t1.empresa='" + empresa + "' AND t1.area='" + area + "' AND t2.status='1' AND (DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 or DATEDIFF(t1.fechaVencimientoTarjeton, curdate()) <= 7) AND t2.status='1'";
                            }
                            else
                            {
                                contador = "SELECT COUNT(idvigencia) FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona WHERE t1.empresa='" + empresa + "' AND t1.area='" + area + "' AND t2.status='1' AND (DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 AND t2.status='1'";

                            }
                        }
                    }
                    MySqlCommand cm = new MySqlCommand(contador, dbcon);
                    var res = cm.ExecuteScalar();
                    dbcon.Close();
                    if (Convert.ToInt32(res) != tbnotif.Rows.Count)
                    {
                        cargarReportes();

                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void rbreportes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbreportes.Checked)
            {
                gbreportes.Text = "Alertas de Reportes";
                gbBuscar.Visible = false;
                opcion = 0;
                Hilo(opcion);
            }
        }
        void Hilo(int tipo)
        {
            if (t != null) t.Abort();
            tbnotif.DataSource = null;
            if (tipo == 0)
            {

                tbnotif.CellFormatting += new DataGridViewCellFormattingEventHandler(reporttes_CellFormatting);
                if (empresa == 1 && area == 1)
                {
                    //sql = "SET lc_Time_names ='es_ES';SELECT t3.Folio as 'FOLIO',concat(t5.identificador,LPAD(t4.consecutivo,4,'0')) as 'ECONÓMICO', t1.Estatus AS 'ESTATUS', UPPER(concat(coalesce(t2.nombres,''),' ', coalesce(t2.ApPaterno,''))) as 'NOMBRE DE MECÁNICO', t3.TipoFallo as 'TIPO DE FALLO', DATE_FORMAT(t3.FechaReporte,'%W, %d de %M del %Y') as 'FECHA DEL REPORTE', t3.HoraEntrada as 'HORA DE ENTRADA' , t1.HoraTerminoM as 'HORA DE TÉRMINO DE MANTENIMIENTO' FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad INNER JOIN careas as t5 ON t4.areafkcareas=t5.idarea WHERE t1.seen = 0 and (t1.Estatus='2' or t1.Estatus='3') AND t3.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                    sql = "SET lc_Time_names ='es_ES';SELECT t3.Folio as 'FOLIO',concat(t5.identificador,LPAD(t4.consecutivo,4,'0')) as 'ECONÓMICO', UPPER(if(t1.Estatus ='1','En Proceso', if(t1.Estatus ='2', 'Reprogramada', if(t1.Estatus ='3','Liberada','')))) AS 'ESTATUS', UPPER(concat(coalesce(t2.nombres,''),' ', coalesce(t2.ApPaterno,''))) as 'NOMBRE DE MECÁNICO', if(t3.TipoFallo='1','CORRECTIVO',if(t3.TipoFallo='2','PREVENTIVO',if(t3.TipoFallo='3','REITERATIVO',if(t3.TipoFallo='4','REPROGRAMADO',if(t3.TipoFallo='5','SEGUIMIENTO',''))))) as 'TIPO DE FALLO', DATE_FORMAT(t3.FechaReporte,'%W, %d de %M del %Y') as 'FECHA DEL REPORTE', t3.HoraEntrada as 'HORA DE ENTRADA' , coalesce(Right(t1.FechaHoraT,8),'') as 'HORA DE TÉRMINO DE MANTENIMIENTO' FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad INNER JOIN careas as t5 ON t4.areafkcareas=t5.idarea WHERE t1.seen = 0 and (t1.Estatus='2' or t1.Estatus='3') AND t3.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                }
                else if (empresa == 2 && area == 1)
                {
                    sql = "SET lc_Time_names ='es_ES';SELECT T1.Folio as 'FOLIO',concat(t4.identificador,LPAD(t2.consecutivo,4,'0')) as 'ECONÓMICO',T1.TipoFallo as 'TIPO DE FALLO', UPPER(DATE_FORMAT(CONCAT(t1.FechaReporte, ' ',t1.HoraEntrada),'%W, %d de %M del %Y / %H:%i:%s'))  as 'ENTRADA' ,upper(CONCAT(coalesce(t3.nombres,''), ' ', coalesce(t3.ApPaterno,''))) as 'SUPERVISOR QUE ELABORÓ', IF(t1.DescFalloNoCod is null ,(select UPPER(CONCAT(tab2.falloesp)) FROM reportesupervicion as tab1 INNER JOIN cfallosesp as tab2 ON tab1.CodFallofkcfallosesp = tab2.idfalloEsp WHERE tab1.idReporteSupervicion = t1.idReporteSupervicion), (SELECT UPPER(DescFalloNoCod) FROM reportesupervicion WHERE idReporteSupervicion=t1.idReporteSupervicion)) as 'FALLO DETECTADO', UPPER(t1.ObservacionesSupervision) as 'OBSERVACIONES DE SUPERVISOR' FROM reportesupervicion AS t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cpersonal as t3 ON t1.SupervisorFKCPersonal = t3.idpersona INNER JOIN careas as t4 ON t2.areafkcareas=t4.idarea WHERE t1.seen =0 AND t1.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                }
                ts = new ThreadStart(paraReportes);
            }
            else
            {
                if (empresa == 1 && area == 1)
                {
                    sql = "SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionTarjeton, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE TARJETÓN', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoTarjeton, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE TARJETÓN' FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE t2.status = 1 AND (DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 or DATEDIFF(t1.fechaVencimientoTarjeton, curdate()) <= 7) and t1.empresa='" + empresa + "' AND t1.area='" + area + "'";
                }
                else if (empresa == 2 && area == 1)
                {
                    sql = "SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR' FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE t1.empresa='" + empresa + "' AND t1.area='" + area + "' and  DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 AND t2.status='1'";
                }
                tbnotif.CellFormatting += new DataGridViewCellFormattingEventHandler(vigencias_CellFormating);

            }
            t = new Thread(ts);
            t.Start();

        }

        private void vigencias_CellFormating(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbnotif.Columns[e.ColumnIndex].Name.Equals("FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR") || tbnotif.Columns[e.ColumnIndex].Name.Equals("FECHA DE VENCIMIENTO DE TARJETÓN"))
            {
                if ((DateTime.Parse(e.Value.ToString()) - DateTime.Today) <= TimeSpan.FromDays(7))
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }

        }

        private void NotificacionSupervision_FormClosing(object sender, FormClosingEventArgs e)
        {
            mientras = false;
            t.Abort();
        }

        private void rbrefacciones_CheckedChanged(object sender, EventArgs e)
        {
            if (rbrefacciones.Checked)
            {
                gbreportes.Text = "Alerta De Vigencias";
                gbBuscar.Visible = true;
                opcion = 1;
                pActualizar.Visible = false;
                cbmes.SelectedIndex = 0;
                Hilo(opcion);

            }
        }

        private void cbmes_DrawItem(object sender, DrawItemEventArgs e){v.combos_DrawItem(sender, e);}

        private void button7_Click(object sender, EventArgs e)
        {
            if (cbmes.SelectedIndex > 0)
            {
                if (empresa == 1 && area == 1)
                {
                    if (Convert.ToInt32(v.getaData("SET lc_time_names = 'es_ES'; SELECT COUNT(*) FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE ((MONTH(t1.fechaVencimientoConducir)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoConducir)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "') OR (MONTH(t1.fechaVencimientoTarjeton)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoTarjeton)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "'))  and t1.empresa='1' AND t1.area='1'")) > 0)
                    {
                        sql = "SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionTarjeton, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE TARJETÓN', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoTarjeton, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE TARJETÓN' FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE ((MONTH(t1.fechaVencimientoConducir)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoConducir)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "') OR (MONTH(t1.fechaVencimientoTarjeton)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoTarjeton)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "'))  and t1.empresa='1' AND t1.area='1'";
                        contador = "SET lc_time_names = 'es_ES'; SELECT COUNT(*) FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE ((MONTH(t1.fechaVencimientoConducir)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoConducir)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "') OR (MONTH(t1.fechaVencimientoTarjeton)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoTarjeton)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "'))  and t1.empresa='1' AND t1.area='1'";
                        buscar = true;
                        pActualizar.Visible = true;
                        cbmes.SelectedIndex = 0;
                        tbnotif.DataSource = null;

                    }
                    else
                        MessageBox.Show("No Se Encontraron Fechas de Vencimiento con el Mes Ingresado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (empresa == 2 && area == 1)
                {
                    if (Convert.ToInt32(v.getaData(string.Format("SET lc_time_names = 'es_ES';SELECT COUNT(*) FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE t1.empresa='{0}' AND t1.area='{1}' and (MONTH(t1.fechaVencimientoConducir)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoConducir)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "')", empresa, area))) > 0)
                    {
                        sql = "SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR' FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE (MONTH(t1.fechaVencimientoConducir)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoConducir)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "')  and t1.empresa='2' AND t1.area='1'";
                        buscar = true;
                        pActualizar.Visible = true;
                        cbmes.SelectedIndex = 0;
                        tbnotif.Rows.Clear();
                    }
                    else
                    {
                        MessageBox.Show("No Se Encontraron Fechas de Vencimiento con el Mes Ingresado", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
                MessageBox.Show("Seleccione Un Mes Para Realizar La Búsqueda", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (empresa == 1 && area == 1)
                sql = "SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionTarjeton, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE TARJETÓN', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoTarjeton, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE TARJETÓN' FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE (DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 or DATEDIFF(t1.fechaVencimientoTarjeton, curdate()) <= 7) and t1.empresa='" + empresa + "' AND t1.area='" + area + "'";
            else
                sql = "SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR' FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE t1.empresa='" + empresa + "' AND t1.area='" + area + "' and  DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 ";
            buscar = false;
            pActualizar.Visible = false;
            tbnotif.DataSource = null;
        }

        void buscarMeses()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("numMes");
            dt.Columns.Add("mes");
            DataRow newRow1 = dt.NewRow();
            newRow1["numMes"] = 0;
            newRow1["mes"] = "-- SELECCIONE MES --";
            dt.Rows.InsertAt(newRow1, 0);
            for (int i = 0; i <= 6; i++)
            {
                DataRow newRow = dt.NewRow();
                newRow["numMes"] = DateTime.Today.Month + i;
                newRow["mes"] = DateTime.Today.AddMonths(i).ToString(" MMMMM DE yyyy").ToUpper();
                dt.Rows.InsertAt(newRow, i + 1);
            }
            cbmes.ValueMember = "numMes";
            cbmes.DisplayMember = "mes";
            cbmes.DataSource = dt;
        }
    }
}