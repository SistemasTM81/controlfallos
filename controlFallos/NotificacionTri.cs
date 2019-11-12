using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Threading;

namespace controlFallos
{
    public partial class NotificacionTri : Form
    {
        conexion c = new conexion();
        int tipoFalla;
        ThreadStart tS;
        string SQL;
        Thread t;
        String sql;
        bool res = true;
        validaciones v = new validaciones();
        public NotificacionTri(int tipoFalla)
        {
            InitializeComponent();
            this.tipoFalla = tipoFalla;
            consulta();
   

        }
        public void consulta() {
   
            switch (tipoFalla)
            {
                case 1:
                    sql = "SET lc_Time_names ='es_ES';SELECT t3.Folio as 'FOLIO',concat(t5.identificador,LPAD(t4.consecutivo,4,'0')) as 'ECONÓMICO', t1.Estatus AS 'ESTATUS', UPPER(concat(t2.nombres,' ', t2.ApPaterno)) as 'NOMBRE DE MECÁNICO', t3.TipoFallo as 'TIPO DE FALLO', DATE_FORMAT(t3.FechaReporte,'%W, %d de %M del %Y') as 'FECHA DEL REPORTE', t3.HoraEntrada as 'HORA DE ENTRADA' , t1.HoraTerminoM as 'HORA DE TÉRMINO DE MANTENIMIENTO' FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad INNER JOIN careas as t5 ON t4.areafkcareas=t5.idarea WHERE t1.seen = 0 and (t1.Estatus='Liberada' or t1.Estatus='Reprogramada') AND t3.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                    SQL = "SELECT COUNT(*) FROM reportemantenimiento as t1 INNER JOIN cpersonal as t2 ON t1.mecanicofkPersonal = t2.idpersona INNER JOIN reportesupervicion as t3 ON t1.FoliofkSupervicion = t3.idReporteSupervicion INNER JOIN cunidades as t4 ON t3.UnidadfkCUnidades= t4.idunidad INNER JOIN careas as t5 ON t4.areafkcareas=t5.idarea WHERE t1.seen = 0 and (t1.Estatus='Liberada' or t1.Estatus='Reprogramada' ) AND t3.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();" ;
                    break;
                case 2:
                    sql = "SET lc_Time_names ='es_ES';SELECT T1.Folio as 'FOLIO',concat(t4.identificador,LPAD(t2.consecutivo,4,'0')) as 'ECONÓMICO',T1.TipoFallo as 'TIPO DE FALLO', UPPER(DATE_FORMAT(CONCAT(t1.FechaReporte, ' ',t1.HoraEntrada),'%W, %d de %M del %Y / %H:%i:%s'))  as 'ENTRADA' ,upper(CONCAT(t3.nombres, ' ', t3.ApPaterno)) as 'SUPERVISOR QUE ELABORÓ', IF(t1.DescFalloNoCod is null ,(select UPPER(CONCAT(tab2.falloesp)) FROM reportesupervicion as tab1 INNER JOIN cfallosesp as tab2 ON tab1.CodFallofkcfallosesp = tab2.idfalloEsp WHERE tab1.idReporteSupervicion = t1.idReporteSupervicion), (SELECT UPPER(DescFalloNoCod) FROM reportesupervicion WHERE idReporteSupervicion=t1.idReporteSupervicion)) as 'FALLO DETECTADO', UPPER(t1.ObservacionesSupervision) as 'OBSERVACIONES DE SUPERVISOR' FROM reportesupervicion AS t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cpersonal as t3 ON t1.SupervisorFKCPersonal = t3.idpersona INNER JOIN careas as t4 ON t2.areafkcareas=t4.idarea WHERE t1.seen =0 AND t1.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                    SQL= "SELECT COUNT(*) FROM reportesupervicion AS t1 INNER JOIN cunidades as t2 ON t1.UnidadfkCUnidades = t2.idunidad INNER JOIN cpersonal as t3 ON t1.SupervisorFKCPersonal = t3.idpersona INNER JOIN careas as t4 ON t2.areafkcareas=t4.idarea WHERE t1.seen =0 AND t1.fechaReporte BETWEEN DATE_SUB(curdate(), INTERVAL 1 DAY) AND curdate();";
                    break;
             
            }
      
        }
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1) {
                menuPrincipal m = (menuPrincipal)Owner;
                string idfolio = tbnotif.Rows[e.RowIndex].Cells[0].Value.ToString();
                m.TraerVariable(idfolio);
                this.Close();
            }
        }
        public void busqnotificaciones()
        {
                tbnotif.ClearSelection();
            DataTable ds = new DataTable();
            MySqlDataAdapter cargar = new MySqlDataAdapter(sql, c.dbconection());
            cargar.Fill(ds);
            tbnotif.DataSource = ds;
            tbnotif.ClearSelection();
            c.dbconection().Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void tbnotif_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void NotificacionTri_Load(object sender, EventArgs e)
        {
            tS = new ThreadStart(notif);
            t = new Thread(tS);
            t.Start();
        }
        delegate void internotif();
        void notif()
        {
            while (res) {
                muestra();
                Thread.Sleep(500);
            }
        }
        void muestra()
        {
            if (InvokeRequired)
            {
                internotif t = new internotif(muestra);
                Invoke(t);

            }
            else
            {
                MySqlConnection dbcon = new MySqlConnection("Server = 192.168.1.108; user=UPT; password = UPT2018; database =sistrefaccmant;port=3306");
                dbcon.Open();
                MySqlCommand cm = new MySqlCommand(SQL,dbcon);
                var res = cm.ExecuteScalar();
                dbcon.Close();
                if (Convert.ToInt32(res) != tbnotif.Rows.Count)
                {
                    busqnotificaciones();

                }
            }
     
        }

        private void NotificacionTri_FormClosing(object sender, FormClosingEventArgs e)
        {

            res = false;
            t.Abort();
        }

        private void tbnotif_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender,e);
        }
    }
   
    }
