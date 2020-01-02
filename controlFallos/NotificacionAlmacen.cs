using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class NotificacionAlmacen : Form
    {
        validaciones v;
        bool res = true;
        Thread t; ThreadStart tS;
        public NotificacionAlmacen(validaciones v)
        {
            this.v = v;
            InitializeComponent();
            cbmes.DrawItem += new DrawItemEventHandler(v.combos_DrawItem);
        }
       void busqvigencias()
        {
            tbnotvigencias.Rows.Clear();
            string sql = "SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR'FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE t2.empresa='2' and t2.area='2' AND DATEDIFF(t1.fechaVencimientoConducir, curdate()) <= 7 AND t2.status='1'";
            MySqlCommand cmd = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string[] filas = { dr.GetString("idpersona"), dr.GetString("NOMBRE"), dr.GetString("TIPO DE LICENCIA"), dr.GetString("FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR"), dr.GetString("FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR") };
                tbnotvigencias.Rows.Add(filas);
            }
            tbnotvigencias.ClearSelection();
        }
        void busqnotificacionesFolio()
        {
            tbnotiffolios.Rows.Clear();
            string sql = "SELECT t2.idReporteSupervicion as id, t2.Folio as folio,concat(t5.identificador,LPAD(t3.consecutivo,4,'0')) as eco, CONCAT(T4.nombres,' ',T4.ApPaterno) as mecanico,t1.FechaReporteM as fechas,(SELECT count(idPedRef) FROM pedidosrefaccion WHERE FolioPedfkSupervicion= t2.idReporteSupervicion) as total FROM reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.FoliofkSupervicion= t2.idReporteSupervicion INNER JOIN cunidades as t3 ON t2.UnidadfkCUnidades = t3.idunidad INNER JOIN cpersonal AS t4 ON t1.MecanicofkPersonal = t4.idPersona INNER JOIN careas as t5 ON t3.areafkcareas=t5.idarea  WHERE StatusRefacciones = 'Se Requieren Refacciones' and t1.seenAlmacen=0 AND t2.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                string[] filas = { dr.GetString("id"), dr.GetString("folio"), dr.GetString("eco"), dr.GetString("mecanico"),DateTime.Parse(dr.GetString("fechas")).ToLongDateString(), dr.GetString("total") };
                tbnotiffolios.Rows.Add(filas);
            }
            dr.Close();
            v.c.dbcon.Close();
            tbnotiffolios.ClearSelection();
        }
         void busqnotificacionesAlertas()
        {
            tbnotifrefacc.Rows.Clear();
            string sql = "SET lc_time_names='es_ES';SELECT t1.idrefaccion,t1.codrefaccion,t1.nombreRefaccion,t1.modeloRefaccion, COALESCE(DATE_FORMAT(t1.proximoAbastecimiento,'%W, %d de %M del %Y ' ),'') AS proximoAbastecimiento, CONCAT(t1.existencias,' ',t2.Simbolo) AS existencias, t1.existencias as exist, t1.media AS media,t1.abastecimiento AS abastecimiento,COALESCE(datediff(t1.proximoAbastecimiento,curdate()),'') as dif FROM crefacciones as t1 INNER JOIN cmarcas as t4 ON t1.marcafkcmarcas= t4.idmarca INNER JOIN Cfamilias as t3 ON t4.descripcionfkcfamilias = t3.idfamilia  INNER JOIN cunidadmedida as t2 ON t3.umfkcunidadmedida=t2.idunidadmedida WHERE t1.existencias <=t1.media OR t1.existencias<= t1.abastecimiento OR datediff(t1.proximoAbastecimiento,curdate()) <=20 and t1.status=1";
            MySqlCommand cm = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                string[] filas = { dr.GetString("idrefaccion"), dr.GetString("codrefaccion"), dr.GetString("nombreRefaccion"), dr.GetString("modeloRefaccion"), dr.GetString("proximoAbastecimiento"), dr.GetString("existencias"),dr.GetString("exist"), dr.GetString("media"), dr.GetString("abastecimiento"),dr.GetString("dif") };
                tbnotifrefacc.Rows.Add(filas);
            }
            dr.Close();
            v.c.dbcon.Close();
            tbnotifrefacc.ClearSelection();
            colorearCeldas();
    }

        private void NotificacionAlmacen_Load(object sender, EventArgs e)
        {
            busqvigencias();
            busqnotificacionesFolio();
            busqnotificacionesAlertas();
            buscarMeses();
            tbnotifrefacc.ClearSelection();
            tS = new ThreadStart(notif);
            t = new Thread(tS);
            t.Start();
        }
        void notif()
        {
            while (res)
            {
                muestra();
                Thread.Sleep(500);
            }
        }
        delegate void internotif();
        void muestra()
        {
            if (InvokeRequired)
            {
                internotif t = new internotif(muestra);
                Invoke(t);

            }
            else
            {
                MySqlConnection dbcon = new MySqlConnection("Server = 192.168.1.108; user=controlFallos; password = controlFallos; database =sistrefaccmant;port=3306");
                dbcon.Open();
                MySqlCommand cm = new MySqlCommand("SELECT COUNT(*) FROM reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.FoliofkSupervicion= t2.idReporteSupervicion INNER JOIN cunidades as t3 ON t2.UnidadfkCUnidades = t3.idunidad INNER JOIN cpersonal AS t4 ON t1.MecanicofkPersonal = t4.idPersona INNER JOIN careas as t5 ON t3.areafkcareas=t5.idarea  WHERE StatusRefacciones = 'Se Requieren Refacciones' and t1.seenAlmacen=0 AND t2.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();", dbcon);
                var res = cm.ExecuteScalar();
                dbcon.Close();
                if (Convert.ToInt32(res) != tbnotiffolios.Rows.Count)
                {
                    busqnotificacionesFolio();

                }
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        private void rbreportes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbreportes.Checked)
            {
                gbreportes.Visible = true;
                gbrefacciones.Visible = false;
                gbvigencias.Visible = false;
                gbBuscar.Visible = false;
            }
        }

        private void rbrefacciones_CheckedChanged(object sender, EventArgs e)
        {
            if (rbrefacciones.Checked)
            {
                gbvigencias.Visible = false;
                gbreportes.Visible = false;
                gbrefacciones.Visible = true;
                gbBuscar.Visible = false;
            }
        }
        void colorearCeldas()
        {
            for (int x= 0;x<tbnotifrefacc.Rows.Count;x++)
            {
                if (Convert.ToDecimal(tbnotifrefacc.Rows[x].Cells[6].Value)<= Convert.ToDecimal(tbnotifrefacc.Rows[x].Cells[7].Value) && Convert.ToDecimal(tbnotifrefacc.Rows[x].Cells[6].Value)>= Convert.ToDecimal(tbnotifrefacc.Rows[x].Cells[8].Value))
                {
                    tbnotifrefacc.Rows[x].DefaultCellStyle.BackColor = Color.Khaki;

                }
                else if (Convert.ToDecimal(tbnotifrefacc.Rows[x].Cells[6].Value) <= Convert.ToDecimal(tbnotifrefacc.Rows[x].Cells[8].Value))
                {
                    tbnotifrefacc.Rows[x].DefaultCellStyle.BackColor = Color.LightCoral;
                }else
                {
                    
                        tbnotifrefacc.Rows[x].DefaultCellStyle.BackColor = Color.FromArgb(75, 44, 52);
                        tbnotifrefacc.Rows[x].DefaultCellStyle.ForeColor = Color.FromArgb(200,200,200);
                   
                }              
            }
            tbnotifrefacc.ClearSelection();
        }

        private void tbnotifrefacc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tbnotifrefacc.ClearSelection();
        }

        private void tbnotifrefacc_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1) {
                string idRefaccion = tbnotifrefacc.Rows[e.RowIndex].Cells[0].Value.ToString();
                menuPrincipal menu = (menuPrincipal)Owner;
                menu.irArefacciones(idRefaccion);
                this.Close();
            }
        }

        private void tbnotiffolios_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1) {
                menuPrincipal m = (menuPrincipal)Owner;
                m.TraerVariable(tbnotiffolios.Rows[e.RowIndex].Cells[0].Value.ToString());
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void NotificacionAlmacen_FormClosing(object sender, FormClosingEventArgs e)
        {
            t.Abort();
            res = false;
        }

        private void tbnotifrefacc_ColumnAdded_1(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void rbvigencias_CheckedChanged(object sender, EventArgs e)
        {
            if (rbvigencias.Checked)
            {
                gbvigencias.Visible = true;
                gbrefacciones.Visible = false;
                gbreportes.Visible = false;
                gbBuscar.Visible = true;
                cbmes.SelectedIndex = 0;
            }
        }

        private void tbnotvigencias_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbnotvigencias.Columns[e.ColumnIndex].Name.Equals("vencimiento"))
            {
                if ((DateTime.Parse(e.Value.ToString()) - DateTime.Today) <= TimeSpan.FromDays(7))
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tbnotvigencias.Rows.Clear();
            DataTable dt = (DataTable) v.getData("SET lc_time_names = 'es_ES'; SELECT t2.idPersona,UPPER(coalesce(CONCAT(t2.ApPaterno, ' ', t2.ApMaterno, ' ', t2.nombres), '')) AS NOMBRE,  COALESCE(CONCAT(T3.Tipo,' - ',t3.Descripcion),'') AS 'TIPO DE LICENCIA', UPPER(coalesce(DATE_FORMAT(t1.fechaEmisionConducir, '%W %d %M %Y'), '')) AS 'FECHA DE EMISIÓN DE LICENCIA DE CONDUCIR', UPPER(coalesce(DATE_FORMAT(t1.fechaVencimientoConducir, '%W %d %M %Y'), '')) AS 'FECHA DE VENCIMIENTO DE LICENCIA DE CONDUCIR'FROM vigencias_supervision AS t1 INNER JOIN cpersonal AS t2 ON t1.usuariofkcpersonal = t2.idPersona inner join cattipos as t3 On t1.tipolicenciafkcattipos=t3.idcattipos WHERE t2.empresa='2' and t2.area='2' AND (MONTH(t1.fechaVencimientoConducir)= '" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? (Convert.ToInt32(cbmes.SelectedValue) - 12).ToString() : cbmes.SelectedValue.ToString()) + "' AND year(t1.fechaVencimientoConducir)='" + (Convert.ToInt32(cbmes.SelectedValue) > 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year) + "')");
            for(int i=0; i<dt.Rows.Count;i++) 
                tbnotvigencias.Rows.Add(dt.Rows[i].ItemArray);
            
            tbnotvigencias.ClearSelection();
            pActualizar.Visible = true;
            cbmes.SelectedIndex = 0;
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
                dt.Rows.InsertAt(newRow, i+1);
            }
            cbmes.ValueMember = "numMes";
            cbmes.DisplayMember = "mes";
            cbmes.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            busqvigencias();
            cbmes.SelectedIndex = 0;
        }

        private void tbnotvigencias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {

                menuPrincipal m = (menuPrincipal)this.Owner;
                string idfolio = tbnotvigencias.Rows[e.RowIndex].Cells[0].Value.ToString();
               
               m.paraPErsonal(idfolio);

                if (m.form.GetType() == typeof(catPersonal))
                    this.Close();
            }
        }
    }
}
