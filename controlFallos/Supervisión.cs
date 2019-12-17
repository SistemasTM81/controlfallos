using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using h = Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace controlFallos
{

    public partial class Supervisión : Form
    {

        int empresa, area, idUsuario; public Thread hilo;
        static bool res = true;
        validaciones v = new validaciones();
        conexion c = new conexion();
        Thread th;
        int _grupoanterior = 0, _subgrupoanterior = 0, _categoriaanterior = 0;

        public Supervisión(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            cmbUnidad.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbServicio.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbTipoFallo.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cbSubGrupo.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbCodFallo.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbBuscarUnidad.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbBuscarDescripcion.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbBuscStatus.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbMeses.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            cmbSupervisores.MouseWheel += new MouseEventHandler(cmbUnidad_MouseWheel);
            this.empresa = empresa; this.area = area; this.idUsuario = idUsuario;
            v.ChangeControlStyles(btnGuardar, ControlStyles.Selectable, true);
        }

        void quitarseen()
        {
            while (res)
            {
                MySqlConnection dbcon;
                if (v.c.conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { v.c.host, v.c.user, v.c.password, v.c.port }));
                else
                    dbcon = new MySqlConnection("Server =  "+v.c.hostLocal+"; user="+ v.c.userLocal +"; password = "+ v.c.passwordLocal +" ;database = sistrefaccmant ;port="+ v.c.portLocal);
                dbcon.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE reportemantenimiento SET seen = 1 WHERE seen  = 0 AND (Estatus='LIBERADA' || Estatus='REPROGRAMADA')", dbcon);
                cmd.ExecuteNonQuery();
                dbcon.Close();
                dbcon.Dispose();
                Thread.Sleep(180000);
            }
        }

        string estatus, ID, SUnidad, credencial, servicio, km, tipofallo, descfallo, codfallo, desfallonot, observaciones, supervissor, IdRepor, es, HoraReporte, puesto_usuario;
        int id_unidad;
        bool bandera = false, bandera_c = false, bandera_nuevo = false, bandera_editar = false, editar = false, mensaje = false, exportando = false;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool getboolfromint(int i)
        {
            return i == 1;
        }

        public string mayusculas(string texto)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(texto);
        }
        public void privilegios()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, "Form1")).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));   
            }
        }
        void cmbUnidad_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        void Muestra_empresas()
        {
            v.iniCombos("select UPPER(t1.nombreEmpresa)as nom,t1.idempresa as id from cempresas as t1 where empresa ='1' order by t1.nombreEmpresa;", cmbEmpresa, "id", "nom", "--SELECCIONE EMPRESA--");
        }
        public void cargar_supervisor()
        {
            v.iniCombos("SELECT UPPER(CONCAT(T1.ApPaterno,' ',T1.ApMaterno,' ',T1.nombres)) AS NOMBRE,idpersona FROM cpersonal AS T1 INNER JOIN datosistema AS T2 on T2.usuariofkcpersonal=T1.idpersona WHERE t1.area='1' and t1.empresa='1' ORDER BY ApPaterno;", cmbSupervisores, "idpersona", "NOMBRE", "--SELECCIONE SUPERVISOR--");
        }

        public void CargarUnidades() //Metodo que funciona para agregar las unidades de la tabla cunidades en el comboBox de busqueda por unidad para seleccionar unidad y para agregar una opción por default.
        {
            v.iniCombos("SELECT concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECO,t1.idunidad FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea order by eco;", cmbBuscarUnidad, "idunidad", "ECO", "--SELECCIONE UNIDAD--");
        }
        public void Unidades_sin_estatus()
        {
            v.iniCombos("SELECT concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECo,t1.idunidad FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea order by eco;", cmbUnidad, "idunidad", "ECo", "--SELECCIONE UNIDAD--");
        }
        public void Unidades() //Metodo para aregar las unidades de la tabla cunidades y mostrarlas en el comboBox para seleccionar una unidad el hacer un nuevo reporte o editar alguno.
        {
            v.iniCombos("SELECT concat(t2.identificador,LPAD(consecutivo,4,'0')) as ECo,t1.idunidad FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea where t1.status='1' order by eco;", cmbUnidad, "idunidad", "ECo", "--SELECCIONE UNIDAD--");
        }

        public void combos_para_otros_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background 
                e.DrawBackground();

                // Drawing one of the items? 
                if (e.Index >= 0)
                {
                    // Set the string alignment. Choices are Center, Near and Far 
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings 
                    // Assumes Brush is solid 
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush 
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        // Draw the string 

                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        // Draw the string 
                        e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }
        public void combos_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background 
                e.DrawBackground();

                // Drawing one of the items? 
                if (e.Index >= 0)
                {
                    // Set the string alignment. Choices are Center, Near and Far 
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings 
                    // Assumes Brush is solid 
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush 
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Crimson);
                        e.DrawBackground();
                        // Draw the string 
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, new SolidBrush(Color.White), e.Bounds, sf);
                        e.DrawFocusRectangle();
                    }
                    else
                    {
                        // Draw the string 
                        DataTable f = (DataTable)cbx.DataSource;
                        e.Graphics.DrawString(f.Rows[e.Index].ItemArray[0].ToString(), cbx.Font, brush, e.Bounds, sf);
                    }
                }
            }
        }
        void Buscar_descripcion()
        {
            v.iniCombos("Select upper(descfallo) as d,iddescfallo as id from cdescfallo order by descfallo;;", cmbBuscarDescripcion, "id", "d", "--SELECCIONE SUBGRUPO--");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbServicio.Enabled = false;
            hilo = new Thread(new ThreadStart(quitarseen));
            hilo.Start();
            privilegios();
            cmbMeses.SelectedIndex = 0;
            dtpFechaDe.Enabled = false;
            dtpFechaA.Enabled = false;
            lblFechaReporte.Text = DateTime.Now.ToLongDateString().ToUpper();
            cargarDAtos();
            Genera_Folio();
            cargar_supervisor();
            CargarUnidades();
            cmbTipoFallo.SelectedIndex = 0;
            cmbBuscStatus.SelectedIndex = 0;
            Buscar_descripcion();
            consulta_descripciones();
            Unidades();
            Mostrar();
            //CargarServicios();
            Muestra_empresas();
            consulta_grupos();
            DgvTabla.ClearSelection();
            btnEditar.Visible = false;
            LblPDF.Visible = false;
            bPdf.Visible = false;
            lblactualizar.Visible = false;
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

        public void Mostrar()
        {
            if (pinsertar)
            {
                GpbSupervisión.Visible = true;
                LblNuevoR.Visible = true;
                btnNuevo.Visible = true;
                DgvTabla.Visible = true;
                DgvTabla.Size = new Size(1920, 400);
            }
            if (peditar)
            {
                GpbBusquedas.Visible = true;
                btnEditar.Visible = true;
                lblactualizar.Visible = true;
                LblNota.Visible = true;
                LblNota1.Visible = true;
                DgvTabla.Visible = true;
                DgvTabla.Size = new Size(1920, 282);
            }
            if (pconsultar)
            {
                GbpMantenimiento.Visible = true;
                GpbBusquedas.Visible = true;
                DgvTabla.Visible = true;
                DgvTabla.Size = new Size(1920, 282);
            }
            if (pinsertar && pconsultar && peditar)
            {
                LblPDF.Visible = true;
                bPdf.Visible = true;
            }
        }
        void Genera_Folio()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT CONCAT(SUBSTRING(Folio,LENGTH(FOLIO)-6,7)+1)AS Folio from reportesupervicion WHERE idReporteSupervicion = (SELECT MAX(idReporteSupervicion) FROM reportesupervicion);", c.dbconection());
            string Folio = (string)cmd.ExecuteScalar();
            if (Folio == null)
            {
                Folio = "0000001";
            }
            else
            {
                while (Folio.Length < 7)
                {
                    Folio = "0" + Folio;
                }
            }
            lblFolio.Text = "TRA" + Folio.ToString();
            c.dbconection().Close();
        }
        public void cargarDAtos()//Metodo para cargar los reportes que se encuentra en la base de datos en el datagridview y para generar el folio de reporte autoincrementable
        {
            //Conusulta para pbtener reportes almacenados en la base de datos
            MySqlDataAdapter cargar = new MySqlDataAdapter("SET lc_time_names = 'es_ES'; select t1.Folio AS 'FOLIO',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'ECO',(select UPPER(Date_format(t1.FechaReporte,'%W %d de %M del %Y'))) AS 'FECHA DEL REPORTE',(select UPPER(concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres))from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)as 'PERSONA QUE INSERTÓ',coalesce((SELECT x2.Credencial FROM cpersonal AS x2 WHERE  x2.idpersona=t1.CredencialConductorfkCPersonal),'')as 'CREDENCIAL DE CONDUCTOR',(select if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios)))as 'SERVICIO', TIME_FORMAT(t1.HoraEntrada,'%r') as 'HORA DEL REPORTE', t1.KmEntrada as 'KILOMETRAJE DE REPORTE',UPPER(t1.TipoFallo) as 'TIPO DE FALLO',COALESCE((select UPPER(x3.descfallo) from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo),'')as 'SUBGRUPO DE FALLO',COALESCE((select UPPER(x4.codfallo) from cfallosesp as x4 where x4.idfalloEsp=t1.CodFallofkcfallosesp),'')as 'CÓDIGO DE FALLO',UPPER(t1.DescFalloNoCod) as 'DESCRIPCIÓN DE FALLO NO CODIFICADO',UPPER(t1.ObservacionesSupervision) as 'OBSERVACIONES DE SUPERVISIÓN',(select upper(concat(date_format(x5.HoraInicioM,'%W %d de %M del %Y'),' / ',time_format(x5.HoraInicioM,'%H:%i'))) from reportemantenimiento as x5 where x5.FoliofkSupervicion=t1.idReporteSupervicion) as 'FECHA/HORA INICIO MANTENIMIENTO',(select upper(concat(date_format(x6.HoraTerminoM,'%W %d de %M del %Y'),' / ',time_format(x6.HoraTerminoM,'%H:%i'))) from reportemantenimiento as x6 where x6.FoliofkSupervicion=t1.idReporteSupervicion)as 'FECHA/HORA TERMINO MANTENIMIENTO',COALESCE((SELECT UPPER(x13.EsperaTiempoM) FROM reportemantenimiento AS x13 WHERE x13.FoliofkSupervicion=t1.idReporteSupervicion),'00:00:00' ) AS 'ESPERA DE MANTENIMIENTO',COALESCE((select UPPER(x7.DiferenciaTiempoM)  from reportemantenimiento as x7 where x7.FoliofkSupervicion=t1.idReporteSupervicion),'00:00:00')as 'TIEMPO MANTENIMIENTO', COALESCE((select UPPER(x8.Estatus) from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'ESTATUS',COALESCE((select UPPER(x9.TrabajoRealizado) from reportemantenimiento as x9 where x9.FoliofkSupervicion=t1.idReporteSupervicion),'') as 'TRABAJO REALIZADO',COALESCE((select UPPER(concat(x11.ApPaterno,' ',x11.ApMaterno,' ',x11.nombres)) from cpersonal as x11 inner join reportemantenimiento as x12 on x11.idPersona=x12.MecanicofkPersonal where x12.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'MECÁNICO QUE REALIZÓ EL MANTENIMIENTO',COALESCE((select UPPER(x10.ObservacionesM) from reportemantenimiento as x10 where x10.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'OBSERVACIONES DE MANTENIMIENTO' from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas AS t4 on t4.idarea=t2.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas WHERE FechaReporte BETWEEN (DATE_ADD(CURDATE() , INTERVAL -1 DAY))AND  curdate() order by t1.FechaReporte desc, t1.HoraEntrada desc;", c.dbconection());
            DataSet ds = new DataSet();
            cargar.Fill(ds);
            DgvTabla.DataSource = ds.Tables[0];
            DgvTabla.Columns[0].Frozen = true;// mostramos reportes en datagridview
            DgvTabla.ClearSelection();
            c.dbconection().Close();
            DgvTabla.ClearSelection();
        }

        void guardar_reporte()
        {
            //Validaciones de campos vacios al momento de dar click al boton guardar
            if (cmbUnidad.SelectedIndex == 0)
            {
                MessageBox.Show("El campo \"unidad\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtSupervisor.Text))
                {
                    MessageBox.Show("El campo \"contraseña\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtConductor.Text))
                    {
                        MessageBox.Show("El campo \"credencial de conductor\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (cmbServicio.SelectedIndex == 0)
                        {
                            MessageBox.Show("El campo \"servicio\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(txtKilometraje.Text))
                            {
                                MessageBox.Show("El campo \"kilometraje de entrada a patio\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                            }
                            else
                            {
                                if (cmbTipoFallo.SelectedIndex == 0)
                                {
                                    MessageBox.Show("El campo \"tipo de falla\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    if (cbgrupo.SelectedIndex == 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))//validación de campos vacios en sección de fallos.
                                    {
                                        MessageBox.Show("Campos vacios en \"la sección de fallos\"".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        if (cbSubGrupo.SelectedIndex == 0)
                                        {
                                            MessageBox.Show("El campo \"Subgrupo\" se encuentra vacio".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                        else
                                        {
                                            if (cbcategoria.SelectedIndex == 0)
                                            {
                                                MessageBox.Show("El campo \"Categoria\" se encuentra vacio".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            }
                                            else
                                            {
                                                if (cmbCodFallo.SelectedIndex == 0)
                                                {
                                                    MessageBox.Show("El campo \"Código de fallo\" se encuentra vacio".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                }
                                                else
                                                {
                                                    if (cbgrupo.SelectedIndex > 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text.Trim()))
                                                    {
                                                        validacionRegistro();
                                                    }
                                                    else
                                                    {
                                                        if (cbgrupo.SelectedIndex == 0 && !string.IsNullOrWhiteSpace(txtDescFalloNoC.Text.Trim()))
                                                        {
                                                            validacionRegistro();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void limpia_act()
        {
            btnActualizar.Visible = false;
            LblActTabla.Visible = false;
        }
        public void validacionRegistro()
        {//Metodo para validar que el numero de credencial exita en la base de datos, que el kilometraje sea mayor a 0 y para guardar el registro cuando se cumplan esas condiciones
            try
            {
                MySqlCommand sql = new MySqlCommand("SELECT concat(t1.ApPaterno,' ',t1.ApMAterno,' ',t1.Nombres)as supervisor ,t1.idPersona,t2.puesto from cpersonal as t1  inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal=t1.idPersona where t3.password='" + v.Encriptar(txtSupervisor.Text) + "'and t1.status='1' and t2.status='1' and t1.empresa='1' and t1.area='1' ", c.dbconection());
                MySqlDataReader cmd = sql.ExecuteReader();
                if (cmd.Read())
                {
                    MySqlCommand cmd1 = new MySqlCommand("select t1.credencial,t2.puesto,t1.idPersona from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.credencial='" + txtConductor.Text + "' AND t1.status='1'   AND t2.status='1' and t1.empresa='1' and t1.area='1'", c.dbconection());
                    MySqlDataReader lee = cmd1.ExecuteReader();//Validamos si la credencial ingresa es valida en la base de datos 
                    if (lee.Read())
                    {
                        float km = float.Parse(txtKilometraje.Text);//Validamos que el kilometraje ingresado sea mayor a 0
                        if (km <= 0)
                        {
                            MessageBox.Show("El kilometraje debe ser mayor a 0".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtKilometraje.Focus();
                            txtKilometraje.Clear();
                        }
                        else
                        {
                            string campos = "Insert into reportesupervicion (Folio,UnidadfkCUnidades,FechaReporte, SupervisorfkCPersonal, CredencialConductorfkCPersonal, Serviciofkcservicios,HoraEntrada,KmEntrada,TipoFallo,ObservacionesSupervision";
                            string valores = "('" + lblFolio.Text + "' , '" + cmbUnidad.SelectedValue + "' ,(select curdate()) , '" + Convert.ToInt32(idsupervisor) + "' , '" + Convert.ToInt32(idconductor) + "' , '" + cmbServicio.SelectedValue + "',(select curtime()) , '" + txtKilometraje.Text + "' , '" + cmbTipoFallo.Text + "','" + txtObserSupervicion.Text.Trim() + "' ";
                            if (cbgrupo.SelectedIndex == 0)
                            {
                                campos += ",DescFalloNoCod)";
                                valores += " ,'" + txtDescFalloNoC.Text.Trim() + "')";
                            }
                            else
                            {
                                campos += ",DescrFallofkcdescfallo,CodFallofkcfallosesp)";
                                valores += " ,'" + cbSubGrupo.SelectedValue + "','" + cmbCodFallo.SelectedValue + "')";
                            }
                            string consulta = campos + " values " + valores;
                            if (v.c.insertar(consulta))
                                MessageBox.Show("Reporte guardado exitosamente ", "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            esta_exportando();
                            limpia_act();
                            cargarDAtos();
                            Genera_Folio();
                            LimpiarReporte();
                        }
                        c.dbconection().Close();
                        c.dbconection().Close();
                        lee.Close();
                    }
                    else
                    {
                        //Mensaje en caso de que no se encuentra la credencial ingresada.
                        MessageBox.Show("El número de credencial es incorrecto".ToUpper(), "VERIFICAR CREDENCIAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtConductor.Focus();
                        txtConductor.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("La contraseña ingresada es incorrecta".ToUpper(), "CONTRASEÑA INCORRECTA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtSupervisor.Focus();
                    txtSupervisor.Clear();
                }
                cmd.Close();
                c.dbconection().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        string idsupervisor;

        private void txtDescFalloNoC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (bandera_editar) btnGuardar_Click(null, e); else btnEditar_Click(null, e);
            }
            else
            {
                {// Validación de letras, números, y carácteres permitidos para ingresar en la caja de texto
                    if (Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || (e.KeyChar == 47) || (e.KeyChar == 35) || (e.KeyChar == 44) || (e.KeyChar == 46) || (e.KeyChar == 249 || e.KeyChar == 127 || e.KeyChar == 08 || e.KeyChar == 32))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                        MessageBox.Show("Solo se aceptan letras, números  #  /  ,  .  en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

        }
        public void ValidarLetra(KeyPressEventArgs e)//Método para validación de letras en cajas de texto.
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsSeparator(e.KeyChar) || Char.IsControl(e.KeyChar) || (e.KeyChar == 44) || (e.KeyChar == 46))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan letras en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtConductor_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validación para solo permitir ingresar números en la caja de texto.
            if (Char.IsNumber(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan números en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void limpiarmant()//Creamos método para limpiar campos donde se muestra información de mantenimiento
        {
            lblHIM.Text = "";
            lblHTM.Text = "";
            lblTM.Text = "";
            lblestatus.Text = "";
            LblTrabajoRealizado.Text = "";
            lblMecanico.Text = "";
            LblObsevacionesMantenimiento.Text = "";
            lblEsperaDeMan.Text = "";
        }
        void realiza_busquedas()
        {
            string cargar = "SET lc_time_names = 'es_ES'; select t1.Folio AS 'FOLIO',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'ECO',(select UPPER(Date_format(t1.FechaReporte,'%W %d de %M del %Y'))) AS 'FECHA DEL REPORTE',(select UPPER(concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres))from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)as 'PERSONA QUE INSERTÓ',coalesce((SELECT x2.Credencial FROM cpersonal AS x2 WHERE  x2.idpersona=t1.CredencialConductorfkCPersonal),'')as 'CREDENCIAL DE CONDUCTOR',(select if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios)))as 'SERVICIO', TIME_FORMAT(t1.HoraEntrada,'%r') as 'HORA DEL REPORTE', t1.KmEntrada as 'KILOMETRAJE DE REPORTE',UPPER(t1.TipoFallo) as 'TIPO DE FALLO',COALESCE((select UPPER(x3.descfallo) from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo),'')as 'SUBGRUPO DE FALLO',COALESCE((select UPPER(x4.codfallo) from cfallosesp as x4 where x4.idfalloEsp=t1.CodFallofkcfallosesp),'')as 'CÓDIGO DE FALLO',UPPER(t1.DescFalloNoCod) as 'DESCRIPCIÓN DE FALLO NO CODIFICADO',UPPER(t1.ObservacionesSupervision) as 'OBSERVACIONES DE SUPERVISIÓN',(select upper(concat(date_format(x5.HoraInicioM,'%W %d de %M del %Y'),' / ',time_format(x5.HoraInicioM,'%H:%i'))) from reportemantenimiento as x5 where x5.FoliofkSupervicion=t1.idReporteSupervicion) as 'FECHA/HORA INICIO MANTENIMIENTO',(select upper(concat(date_format(x6.HoraTerminoM,'%W %d de %M del %Y'),' / ',time_format(x6.HoraTerminoM,'%H:%i'))) from reportemantenimiento as x6 where x6.FoliofkSupervicion=t1.idReporteSupervicion)as 'FECHA/HORA TERMINO MANTENIMIENTO',COALESCE((SELECT UPPER(x13.EsperaTiempoM) FROM reportemantenimiento AS x13 WHERE x13.FoliofkSupervicion=t1.idReporteSupervicion),'00:00:00' ) AS 'ESPERA DE MANTENIMIENTO',COALESCE((select UPPER(x7.DiferenciaTiempoM)  from reportemantenimiento as x7 where x7.FoliofkSupervicion=t1.idReporteSupervicion),'00:00:00')as 'TIEMPO MANTENIMIENTO', COALESCE((select UPPER(x8.Estatus) from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'ESTATUS',COALESCE((select UPPER(x9.TrabajoRealizado) from reportemantenimiento as x9 where x9.FoliofkSupervicion=t1.idReporteSupervicion),'') as 'TRABAJO REALIZADO',COALESCE((select UPPER(concat(x11.ApPaterno,' ',x11.ApMaterno,' ',x11.nombres)) from cpersonal as x11 inner join reportemantenimiento as x12 on x11.idPersona=x12.MecanicofkPersonal where x12.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'MECÁNICO QUE REALIZÓ EL MANTENIMIENTO',COALESCE((select UPPER(x10.ObservacionesM) from reportemantenimiento as x10 where x10.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'OBSERVACIONES DE MANTENIMIENTO' from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas AS t4 on t4.idarea=t2.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas ";
            String F1 = "";
            String F2 = "";
            if (checkBox1.Checked == true)
            {
                F1 = dtpFechaDe.Value.ToString("yyyy-MM-dd");
                F2 = dtpFechaA.Value.ToString("yyyy-MM-dd");
                if (dtpFechaA.Value.Date < dtpFechaDe.Value.Date || dtpFechaA.Value.Date > DateTime.Now)
                {
                    MessageBox.Show("Las fechas seleccionadas son incorrectas".ToUpper(), "VERIFICAR FECHAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string wheres = "";
                    if (wheres == "")
                    {
                        wheres = "WHERE (SELECT t1.FechaReporte BETWEEN '" + F1.ToString() + "' AND '" + F2.ToString() + "')";
                    }
                    else
                    {
                        wheres += " AND (SELECT t1.FechaReporte BETWEEN '" + F1.ToString() + "' AND '" + F2.ToString() + "')";
                    }
                    if (cmbBuscarUnidad.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (select concat(t4.identificador,LPAD(consecutivo,4,'0')))='" + cmbBuscarUnidad.Text + "' ";
                        }
                        else
                        {
                            wheres += " AND (select concat(t4.identificador,LPAD(consecutivo,4,'0')))='" + cmbBuscarUnidad.Text + "'";
                        }
                    }
                    if (cmbBuscarDescripcion.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)='" + cmbBuscarDescripcion.Text + "'";
                        }
                        else
                        {
                            wheres += " AND (select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)='" + cmbBuscarDescripcion.Text + "'";
                        }
                    }
                    if (cmbSupervisores.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)='" + cmbSupervisores.Text + "'";
                        }
                        else
                        {
                            wheres += " AND (select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)='" + cmbSupervisores.Text + "'";
                        }
                    }
                    if (cmbBuscStatus.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE (select x8.Estatus from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion)='" + cmbBuscStatus.Text + "'";
                        }
                        else
                        {
                            wheres += " AND (select x8.Estatus from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion)='" + cmbBuscStatus.Text + "'";
                        }
                    }
                    if (cmbEmpresa.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE T5.idempresa='" + cmbEmpresa.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " AND T5.idempresa='" + cmbEmpresa.SelectedValue + "' ";
                        }
                    }
                    if (wheres != "")
                    {
                        wheres += " order by FechaReporte desc, t1.HoraEntrada desc ";
                    }
                    MySqlDataAdapter DT = new MySqlDataAdapter(cargar + wheres, c.dbconection());
                    DataSet ds = new DataSet();
                    DT.Fill(ds);
                    DgvTabla.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron reportes".ToUpper(), "NINGÚN REPORTE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cargarDAtos();
                        esta_exportando();
                        btnActualizar.Visible = false;
                        LblActTabla.Visible = false;
                    }
                    else
                    {
                        if (peditar && pconsultar && peditar)
                        {
                            if (!estado)
                            {
                                btnExcel.Visible = true;
                            }
                            LblExcel.Visible = true;
                        }
                        btnActualizar.Visible = true;
                        LblActTabla.Visible = true;
                    }

                    c.dbconection().Close();
                    limpiarbusqueda();
                    checkBox1.Checked = false;
                }
            }
            else
            {
                if (cmbSupervisores.SelectedIndex == 0 && cmbBuscarUnidad.SelectedIndex <= 0 && cmbBuscarDescripcion.SelectedIndex == 0 && cmbBuscStatus.SelectedIndex == 0 && cmbMeses.SelectedIndex == 0 && cmbEmpresa.SelectedIndex == 0)
                {
                    MessageBox.Show("Seleccione un criterio de búsqueda".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string wheres = "";
                    if (cmbBuscarUnidad.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (select concat(t4.identificador,LPAD(consecutivo,4,'0')))='" + cmbBuscarUnidad.Text + "' ";
                        }
                        else
                        {
                            wheres += " and (select concat(t4.identificador,LPAD(consecutivo,4,'0')))='" + cmbBuscarUnidad.Text + "' ";
                        }

                    }
                    if (cmbBuscarDescripcion.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)='" + cmbBuscarDescripcion.Text + "'";
                        }
                        else
                        {
                            wheres += " AND (select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo)='" + cmbBuscarDescripcion.Text + "'";
                        }
                    }
                    if (cmbSupervisores.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " Where (select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)='" + cmbSupervisores.Text + "'";
                        }
                        else
                        {
                            wheres += " AND (select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)='" + cmbSupervisores.Text + "'";
                        }
                    }
                    if (cmbBuscStatus.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE (select x8.Estatus from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion)='" + cmbBuscStatus.Text + "'";
                        }
                        else
                        {
                            wheres += " AND (select x8.Estatus from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion)='" + cmbBuscStatus.Text + "'";
                        }
                    }
                    if (cmbMeses.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE (select Date_format(t1.FechaReporte,'%W %d %M %Y') like '%" + cmbMeses.Text + "%' and (select year(t1.FechaReporte))=( select year(now())))";
                        }
                        else
                        {
                            wheres += " AND (select Date_format(t1.FechaReporte,'%W %d %M %Y') like '%" + cmbMeses.Text + "%' and (select year(t1.FechaReporte))=( select year(now())))";
                        }
                    }
                    if (cmbEmpresa.SelectedIndex > 0)
                    {
                        if (wheres == "")
                        {
                            wheres = " WHERE T5.idempresa='" + cmbEmpresa.SelectedValue + "'";
                        }
                        else
                        {
                            wheres += " AND T5.idempresa='" + cmbEmpresa.SelectedValue + "' ";
                        }
                    }
                    if (wheres != "")
                    {
                        wheres += " and (select year(t1.FechaReporte))=( select year(now())) order by FechaReporte desc, t1.HoraEntrada desc ";
                    }
                    MySqlDataAdapter cargar2 = new MySqlDataAdapter(cargar + wheres, c.dbconection());
                    DataSet ds = new DataSet();
                    cargar2.Fill(ds);
                    DgvTabla.DataSource = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron reportes".ToUpper(), "NINGÚN REPORTE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        esta_exportando();
                        cargarDAtos();
                        btnActualizar.Visible = false;
                        LblActTabla.Visible = false;
                    }
                    else
                    {
                        if (peditar && pconsultar && peditar)
                        {
                            if (!estado)
                            {
                                btnExcel.Visible = true;
                            }
                            LblExcel.Visible = true;
                        }
                        btnActualizar.Visible = true;
                        LblActTabla.Visible = true;
                    }
                    c.dbconection().Close();
                    limpiarbusqueda();
                }
            }
        }

        public void limpiarbusqueda()//Creamos método para limpiar campos de busqueda.
        {
            cmbBuscarDescripcion.SelectedIndex = 0;
            cmbEmpresa.SelectedIndex = 0;
            cmbBuscStatus.SelectedIndex = 0;
            //cmbBuscarUnidad.SelectedIndex = 0;
            dtpFechaDe.ResetText();
            dtpFechaA.ResetText();
            cmbMeses.SelectedIndex = 0;
            cmbSupervisores.SelectedIndex = 0;
        }
        private void txtSupervisor_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validamos que se permitan ingresar letras, números y ciertos carácteres en la caja de texto.
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan letras y números en este campo".ToUpper(), "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        public void LimpiarReporte()//Creamos metodo para limpiar campos de reporte de supervisión
        {
            cmbUnidad.SelectedIndex = 0;
            txtSupervisor.Clear();
            lblSupervisor.Text = "";
            lblid.Text = "";
            txtConductor.Clear();
            lblCredCond.Text = "";
            idsupervisor = "";
            txtKilometraje.Clear();
            cmbTipoFallo.SelectedIndex = 0;
            cbgrupo.SelectedIndex = 0;
            txtDescFalloNoC.Clear();
            txtObserSupervicion.Clear();
            lblFechaReporte.Text = DateTime.Now.ToLongDateString().ToUpper();
            cmbCodFallo.Enabled = false;
            bandera = false;
            bandera_c = false;
            bandera_nuevo = false;
            editar = false;
            mensaje = false;
            btnEditar.Visible = false;
            lblactualizar.Visible = false;
            bPdf.Visible = false;
            LblPDF.Visible = false;
            DgvTabla.ClearSelection();
            btnGuardar.Visible = true;
            txtKilometraje.MaxLength = 12;
            LblGuardar.Visible = true;
            _unidad = "";
            esta_exportando();
            cmbServicio.Enabled = false;
        }

        string _fallonoc;
        string folio, unidad, fecha, supervisor, conductor, serviciou, kmR, hora, tipo, descFallo, codFallo, DesFalloNo, ObservacionesSUp, TiempoE, HoraIni, HoraTer, TiempoMan, Estatus, TrabajoR, MecánicoRT, ObservacionesMante, lbldesc;
        private void To_pdf()//Método generar PDF
        {
            MySqlCommand cargar = new MySqlCommand("SET lc_time_names = 'es_ES'; select t1.Folio,(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS Económico,(select Date_format(t1.FechaReporte,'%W %d de %M del %Y')) AS 'Fecha Del Reporte',(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres)from cpersonal as x1 where x1.idpersona=t1.SupervisorfkCpersonal)as'Supervisor',(SELECT x2.Credencial FROM cpersonal AS x2 WHERE  x2.idpersona=t1.CredencialConductorfkCPersonal)as 'Credencial Conductor',(select if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios)))as 'SERVICIO', t1.HoraEntrada as 'Hora Del Reporte', t1.KmEntrada as 'Kilometraje Del Reporte', t1.TipoFallo as 'Tipo de Fallo',COALESCE((select x3.descfallo from cdescfallo as x3 where x3.iddescfallo=t1.DescrFallofkcdescfallo),'')as 'Descripción Del Fallo',COALESCE((select x4.codfallo from cfallosesp as x4 where x4.idfalloEsp=t1.CodFallofkcfallosesp),'')as 'Código De Fallo',t1.DescFalloNoCod as 'Descripción De Fallo No Códificado', t1.ObservacionesSupervision as 'Observaciones De Supervisión',COALESCE((SELECT x13.EsperaTiempoM FROM reportemantenimiento AS x13 WHERE x13.FoliofkSupervicion=t1.idReporteSupervicion),'' ) AS 'Espera De Mantenimiento' ,coalesce((select concat(date_format(x5.HoraInicioM,'%W %d de %M del %Y'),' / ',time_format(x5.HoraInicioM,'%H:%i')) from reportemantenimiento as x5 where x5.FoliofkSupervicion=t1.idReporteSupervicion),'') as 'Hora Inicio Mantenimiento',coalesce((select date_format(x6.HoraTerminoM,'%W %d de %M del %Y') from reportemantenimiento as x6 where x6.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'Hora Termino Mantenimiento' ,COALESCE((select x7.DiferenciaTiempoM  from reportemantenimiento as x7 where x7.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'Tiempo Mantenimiento', COALESCE((select x8.Estatus from reportemantenimiento as x8 where x8.FoliofkSupervicion=t1.idReporteSupervicion),'')as Estatus,COALESCE((select x9.TrabajoRealizado from reportemantenimiento as x9 where x9.FoliofkSupervicion=t1.idReporteSupervicion),'') as 'Trabajo Realizado',COALESCE((select concat(x11.ApPaterno,' ',x11.ApMaterno,' ',x11.nombres) from cpersonal as x11 inner join reportemantenimiento as x12 on x11.idPersona=x12.MecanicofkPersonal where x12.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'Mecánico Que Realizo El Mantenimiento',COALESCE((select x10.ObservacionesM from reportemantenimiento as x10 where x10.FoliofkSupervicion=t1.idReporteSupervicion),'')as 'Observaciones Mantenimiento' from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades=t2.idunidad  INNER JOIN careas as t4 on t4.idarea=t2.areafkcareas WHERE t1.folio='" + lblFolio.Text + "'", c.dbconection());
            MySqlDataReader dr = cargar.ExecuteReader();
            if (dr.Read())
            {
                folio = Convert.ToString(dr["Folio"]).ToUpper();
                unidad = Convert.ToString(dr["Económico"]).ToUpper();
                fecha = Convert.ToString(dr["Fecha Del Reporte"]).ToUpper(); ;
                supervisor = Convert.ToString(dr["Supervisor"]).ToUpper(); ;
                conductor = Convert.ToString(dr["Credencial Conductor"]).ToUpper(); ;
                serviciou = Convert.ToString(dr["Servicio"]).ToUpper(); ;
                kmR = Convert.ToString(dr["Kilometraje Del Reporte"]).ToUpper(); ;
                hora = Convert.ToString(dr["Hora Del Reporte"]).ToUpper(); ;
                tipo = Convert.ToString(dr["Tipo de Fallo"]).ToUpper();
                descFallo = Convert.ToString(dr["Descripción Del Fallo"]).ToUpper();
                codFallo = Convert.ToString(dr["Código De Fallo"]);
                DesFalloNo = Convert.ToString(dr["Descripción De Fallo No Códificado"]).ToUpper();
                ObservacionesSUp = Convert.ToString(dr["Observaciones De Supervisión"]).ToUpper();
                TiempoE = Convert.ToString(dr["Espera De Mantenimiento"]).ToUpper();
                HoraIni = Convert.ToString(dr["Hora Inicio Mantenimiento"]).ToUpper();
                HoraTer = Convert.ToString(dr["Hora Termino Mantenimiento"]).ToUpper();
                TiempoMan = Convert.ToString(dr["Tiempo Mantenimiento"]).ToUpper();
                Estatus = Convert.ToString(dr["Estatus"]).ToUpper();
                TrabajoR = Convert.ToString(dr["Trabajo Realizado"]).ToUpper();
                MecánicoRT = Convert.ToString(dr["Mecánico Que Realizo El Mantenimiento"]).ToUpper();
                ObservacionesMante = Convert.ToString(dr["Observaciones Mantenimiento"]).ToUpper();
                if (!string.IsNullOrWhiteSpace(descFallo))
                {
                    lbldesc = v.getaData("SELECT falloesp from cfallosesp where codfallo='" + codFallo + "' ").ToString().ToUpper();
                }
            }
            dr.Close();
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
                    {
                        filename = filename + ".pdf";
                    }
                    while (filename.ToLower().Contains(".pdf.pdf"))
                    {
                        filename = filename.ToLower().Replace(".pdf.pdf", ".pdf").Trim();
                    }
                }
                if (filename.Trim() != "")
                {
                    FileStream file = new FileStream(filename,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
                    PdfWriter.GetInstance(doc, file);
                    iTextSharp.text.Font arial = FontFactory.GetFont("Calibri", 9, BaseColor.BLACK);
                    iTextSharp.text.Font arial2 = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD);
                    doc.Open();
                    Chunk chunk = new Chunk("REPORTE SUPERVISIÓN", FontFactory.GetFont("Calibri", 18, iTextSharp.text.Font.BOLD));

                    byte[] img = Convert.FromBase64String(v.transmasivo);
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img);
                    imagen.ScalePercent(16f);
                    imagen.SetAbsolutePosition(400f, 705f);
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(new Paragraph(chunk));
                    doc.Add(imagen);
                    PdfPTable tabla = new PdfPTable(3);
                    tabla.DefaultCell.Border = 1;
                    tabla.WidthPercentage = 100;
                    PdfPCell cell1 = new PdfPCell();
                    cell1.Border = 0;
                    Phrase LblFolio = new Phrase(folio, arial);
                    Phrase Folio = new Phrase("FOLIO:", arial2);
                    Phrase LSupervisor = new Phrase(supervisor, arial);
                    Phrase Supervisor = new Phrase("SUPERVISOR:", arial2);
                    Phrase SaltoLinea = new Phrase("           ");
                    Phrase KM = new Phrase("KILOMETRAJE:", arial2);
                    Phrase LHoraReporte = new Phrase("HORA DEL REPORTE:", arial2);
                    Phrase HoraRep = new Phrase(hora, arial);
                    Phrase LKM = new Phrase(kmR, arial);
                    cell1.AddElement(Folio);
                    cell1.AddElement(LblFolio);
                    cell1.AddElement(SaltoLinea);
                    cell1.AddElement(Supervisor);
                    cell1.AddElement(LSupervisor);
                    cell1.AddElement(SaltoLinea);
                    cell1.AddElement(LHoraReporte);
                    cell1.AddElement(HoraRep);
                    cell1.AddElement(SaltoLinea);
                    PdfPCell cell2 = new PdfPCell();
                    cell2.Border = 0;
                    Phrase lUnidad = new Phrase(unidad, arial);
                    Phrase Unidad = new Phrase("UNIDAD:", arial2);
                    Phrase LCredencial = new Phrase(conductor, arial);
                    Phrase Credencial = new Phrase("CREDENCIAL DE CONDUCTOR:", arial2);
                    Phrase LtipoFAllo = new Phrase("TIPO DE FALLO: ", arial2);
                    Phrase TipoFAllo = new Phrase(tipo, arial);
                    cell2.AddElement(Unidad);
                    cell2.AddElement(lUnidad);
                    cell2.AddElement(SaltoLinea);
                    cell2.AddElement(Credencial);
                    cell2.AddElement(LCredencial);
                    cell2.AddElement(SaltoLinea);
                    cell2.AddElement(KM);
                    cell2.AddElement(LKM);
                    PdfPCell cell3 = new PdfPCell();
                    cell3.Border = 0;
                    Phrase LFecha = new Phrase(fecha, arial);
                    Phrase Fecha = new Phrase("FECHA DEL REPORTE:", arial2);
                    Phrase LServicio = new Phrase(serviciou, arial);
                    Phrase Servicio = new Phrase("SERVICIO:", arial2);
                    cell3.AddElement(Fecha);
                    cell3.AddElement(LFecha);
                    cell3.AddElement(SaltoLinea);
                    cell3.AddElement(Servicio);
                    cell3.AddElement(LServicio);
                    cell3.AddElement(SaltoLinea);
                    cell3.AddElement(LtipoFAllo);
                    cell3.AddElement(TipoFAllo);
                    doc.Add(SaltoLinea);
                    PdfPTable tabla2 = new PdfPTable(3);
                    tabla2.DefaultCell.Border = 0;
                    tabla2.WidthPercentage = 100;
                    PdfPCell celda = new PdfPCell();
                    celda.Border = 0;
                    PdfPTable tablafallos = new PdfPTable(3);
                    tablafallos.DefaultCell.Border = 0;
                    tablafallos.WidthPercentage = 100;
                    PdfPCell celdaf1 = new PdfPCell();
                    PdfPCell celdaf2 = new PdfPCell();
                    PdfPCell celdaf3 = new PdfPCell();
                    celdaf1.Border = 0;
                    celdaf2.Border = 0;
                    celdaf3.Border = 0;
                    if (string.IsNullOrWhiteSpace(descFallo))
                    {
                        Phrase DescNoC = new Phrase("DESCRIPCIÓN DE FALLO NO CÓDIFICADO", arial2);
                        Phrase LDescNoc = new Phrase(DesFalloNo, arial);
                        celda.Colspan = 3;
                        celda.AddElement(DescNoC);
                        celda.AddElement(LDescNoc);
                        celda.AddElement(SaltoLinea);
                        tabla2.AddCell(celda);
                    }
                    else
                    {
                        Phrase Desc = new Phrase("GRUPO:", arial2);
                        Phrase LDesc = new Phrase(v.getaData("select upper(nombreFalloGral) from cfallosgrales where idFalloGral='" + _grupoanterior + "'").ToString(), arial);
                        celda.AddElement(Desc);
                        celda.AddElement(LDesc);
                        celda.AddElement(SaltoLinea);
                        PdfPCell celdas = new PdfPCell();
                        celdas.AddElement(new Phrase("SUBGRUPO:", arial2));
                        celdas.AddElement(new Phrase(descFallo, arial));
                        celdas.AddElement(SaltoLinea);
                        PdfPCell celdas_1 = new PdfPCell();
                        celdas_1.AddElement(new Phrase("CATEGORIA:", arial2));
                        celdas_1.AddElement(new Phrase(v.getaData("select upper(categoria) from catcategorias where idcategoria='" + _categoriaanterior + "'").ToString(), arial));
                        celdas_1.AddElement(SaltoLinea);
                        Phrase Codigo = new Phrase("CÓDIGO DE FALLO:", arial2);
                        Phrase Nombre = new Phrase("NOMBRE DE FALLO:", arial2);
                        Phrase LCodigo = new Phrase(codFallo, arial);
                        Phrase descfallo = new Phrase(lbldesc, arial);
                        celdas.Border = celdas_1.Border = 0;
                        celdaf1.AddElement(Codigo);
                        celdaf1.AddElement(LCodigo);
                        celdaf1.AddElement(SaltoLinea);
                        celdaf2.AddElement(Nombre);
                        celdaf2.AddElement(descfallo);
                        celdaf3.AddElement(SaltoLinea);
                        tabla2.AddCell(celda);
                        tabla2.AddCell(celdas);
                        tabla2.AddCell(celdas_1);
                    }
                    PdfPTable tablaobservaciones = new PdfPTable(1);
                    tablaobservaciones.DefaultCell.Border = 0;
                    tablaobservaciones.WidthPercentage = 100;
                    PdfPCell celda_obser = new PdfPCell();
                    celda_obser.Border = 0;
                    Phrase Obser = new Phrase("OBSERVACIONES DE SUPERVISIÓN:", arial2);
                    Phrase LObser = new Phrase(ObservacionesSUp, arial);
                    celda_obser.AddElement(Obser);
                    celda_obser.AddElement(LObser);
                    celda_obser.AddElement(SaltoLinea);
                    PdfPTable tabla3 = new PdfPTable(2);
                    tabla3.DefaultCell.Border = 0;
                    tabla3.WidthPercentage = 100;
                    PdfPCell celda2 = new PdfPCell();
                    celda2.Border = 0;
                    Phrase HoraInicio = new Phrase("FECHA / HORA DE INICIO:", arial2);
                    Phrase LHoraInicio = new Phrase(HoraIni, arial);
                    Phrase TiemEsp = new Phrase("TIEMPO DE ESPERA:", arial2);
                    Phrase LTiemEsp = new Phrase(TiempoE, arial);
                    Phrase Estatus = new Phrase("ESTATUS DE UNIDAD:", arial2);
                    Phrase LEstatus = new Phrase(lblestatus.Text, arial);
                    celda2.AddElement(HoraInicio);
                    celda2.AddElement(LHoraInicio);
                    celda2.AddElement(SaltoLinea);
                    celda2.AddElement(TiemEsp);
                    celda2.AddElement(LTiemEsp);
                    celda2.AddElement(SaltoLinea);
                    celda2.AddElement(Estatus);
                    celda2.AddElement(LEstatus);
                    celda2.AddElement(SaltoLinea);
                    PdfPCell celda2_2 = new PdfPCell();
                    celda2_2.Border = 0;
                    Phrase HoraLib = new Phrase("FECHA / HORA DE LIBERACIÓN", arial2);
                    Phrase LhoraLib = new Phrase(HoraTer, arial);
                    Phrase TiempMan = new Phrase("TIEMPO DE MANTENIMIENTO:", arial2);
                    Phrase LTiempMan = new Phrase(TiempoMan, arial);
                    Phrase Trabajo = new Phrase("TRABAJO REALIZADO: ", arial2);
                    Phrase Ltrabajo = new Phrase(TrabajoR, arial);
                    celda2_2.AddElement(HoraLib);
                    celda2_2.AddElement(LhoraLib);
                    celda2_2.AddElement(SaltoLinea);
                    celda2_2.AddElement(TiempMan);
                    celda2_2.AddElement(LTiempMan);
                    celda2_2.AddElement(SaltoLinea);
                    celda2_2.AddElement(Trabajo);
                    celda2_2.AddElement(Ltrabajo);
                    PdfPTable tabla4 = new PdfPTable(1);
                    tabla4.DefaultCell.Border = 0;
                    tabla4.WidthPercentage = 100;
                    PdfPCell celda4 = new PdfPCell();
                    celda4.Border = 0;
                    Phrase Mecanico = new Phrase("MECÁNICO QUE REALIZÓ MANTENIMIENTO:", arial2);
                    Phrase Lmecanico = new Phrase(MecánicoRT, arial);
                    Phrase ObserM = new Phrase("OBSERVACIONES DE MANTENIMIENTO:", arial2);
                    Phrase LobserM = new Phrase(ObservacionesMante, arial);
                    celda4.AddElement(Mecanico);
                    celda4.AddElement(Lmecanico);
                    celda4.AddElement(SaltoLinea);
                    celda4.AddElement(ObserM);
                    celda4.AddElement(LobserM);
                    tabla.AddCell(cell1);
                    tabla.AddCell(cell2);
                    tabla.AddCell(cell3);
                    tablafallos.AddCell(celdaf1);
                    tablafallos.AddCell(celdaf2);
                    tablafallos.AddCell(celdaf3);
                    tablaobservaciones.AddCell(celda_obser);
                    tabla3.AddCell(celda2);
                    tabla3.AddCell(celda2_2);
                    tabla4.AddCell(celda4);
                    doc.Add(tabla);
                    doc.Add(tabla2);
                    doc.Add(tablafallos);
                    doc.Add(tablaobservaciones);
                    doc.Add(new Paragraph("             "));
                    doc.Add(new Chunk("DATOS DE MANTENIMIENTO", FontFactory.GetFont("ARIAL", 18, iTextSharp.text.Font.BOLD)));
                    doc.Add(new Paragraph("             "));
                    doc.Add(tabla3);
                    doc.Add(tabla4);
                    doc.Close();
                    //Exportacion(;)
                    System.Diagnostics.Process.Start(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HabilitarCampos()
        {
            txtSupervisor.Enabled = true;
            txtConductor.Enabled = true;
            cmbServicio.Enabled = true;
            txtKilometraje.Enabled = true;
            cmbTipoFallo.Enabled = true;
            cbgrupo.Enabled = true;
            if (cbgrupo.SelectedIndex > 0)
            {
                txtDescFalloNoC.Enabled = false;
            }
            else
            {
                txtDescFalloNoC.Enabled = true;
            }
            txtObserSupervicion.Enabled = true;
        }
        public void DeshabilitarCampos()
        {
            txtSupervisor.Enabled = false;
            txtConductor.Enabled = false;
            cmbServicio.Enabled = false;
            txtKilometraje.Enabled = false;
            cmbTipoFallo.Enabled = false;
            cbgrupo.Enabled = cbcategoria.Enabled = false;
            cbSubGrupo.Enabled = false;
            cmbCodFallo.Enabled = false;
            txtDescFalloNoC.Enabled = false;
            txtObserSupervicion.Enabled = false;
        }
        string obs, _unidad;
        int _idservicio = 0, grupo_anterior=0;
        void restaurar_datos(DataGridViewCellEventArgs e)
        {
            if (DgvTabla.Rows.Count > 0)
            {
                if (bandera_c == false)
                {
                    btnGuardar.Visible = false;
                    LblGuardar.Visible = false;
                    lblFolio.Text = DgvTabla.Rows[e.RowIndex].Cells[0].Value.ToString();
                    IdRepor = v.getaData("SELECT idreportesupervicion FROM reportesupervicion WHERE folio='" + lblFolio.Text + "'").ToString();
                    cmbUnidad.Text = _unidad = DgvTabla.Rows[e.RowIndex].Cells[1].Value.ToString();
                    lblSupervisor.Text = DgvTabla.Rows[e.RowIndex].Cells[3].Value.ToString();
                    lblFechaReporte.Text = DgvTabla.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtConductor.Text = DgvTabla.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txtKilometraje.Text = DgvTabla.Rows[e.RowIndex].Cells[7].Value.ToString();
                    cmbTipoFallo.Text = DgvTabla.Rows[e.RowIndex].Cells[8].Value.ToString();
                    HoraReporte = DgvTabla.Rows[e.RowIndex].Cells[6].Value.ToString();
                    txtDescFalloNoC.Text = DgvTabla.Rows[e.RowIndex].Cells[11].Value.ToString();
                    txtObserSupervicion.Text = DgvTabla.Rows[e.RowIndex].Cells[12].Value.ToString();
                    lblEsperaDeMan.Text = DgvTabla.Rows[e.RowIndex].Cells[15].Value.ToString();
                    lblHIM.Text = DgvTabla.Rows[e.RowIndex].Cells[13].Value.ToString();
                    lblHTM.Text = DgvTabla.Rows[e.RowIndex].Cells[14].Value.ToString();
                    lblTM.Text = DgvTabla.Rows[e.RowIndex].Cells[16].Value.ToString();
                    lblestatus.Text = DgvTabla.Rows[e.RowIndex].Cells[17].Value.ToString();
                    LblTrabajoRealizado.Text = DgvTabla.Rows[e.RowIndex].Cells[18].Value.ToString();
                    lblMecanico.Text = DgvTabla.Rows[e.RowIndex].Cells[19].Value.ToString();
                    LblObsevacionesMantenimiento.Text = DgvTabla.Rows[e.RowIndex].Cells[20].Value.ToString();
                    credencial = DgvTabla.Rows[e.RowIndex].Cells[4].Value.ToString();
                    supervissor = DgvTabla.Rows[e.RowIndex].Cells[3].Value.ToString();
                    servicio = DgvTabla.Rows[e.RowIndex].Cells[5].Value.ToString();
                    if (servicio.Equals("SIN SERVICIO")) { _idservicio = 1; }
                    else { _idservicio = Convert.ToInt32(v.getaData("select idservicio from cservicios as t1 inner join reportesupervicion as t2 on t2.Serviciofkcservicios=t1.idservicio where Upper(t1.nombre)='" + servicio + "' and t2.idReporteSupervicion='" + IdRepor + "';")); }
                    km = DgvTabla.Rows[e.RowIndex].Cells[7].Value.ToString();
                    tipofallo = DgvTabla.Rows[e.RowIndex].Cells[8].Value.ToString();
                    descfallo = DgvTabla.Rows[e.RowIndex].Cells[9].Value.ToString();
                    codfallo = DgvTabla.Rows[e.RowIndex].Cells[10].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(descfallo))
                        grupo_anterior = Convert.ToInt32(v.getaData("select coalesce(t1.idFalloGral,'0') from cfallosgrales as t1 inner join cdescfallo as t2 on t2.falloGralfkcfallosgrales=t1.idFalloGral inner join reportesupervicion as t3 on t3.DescrFallofkcdescfallo=t2.iddescfallo where t3.idReporteSupervicion='" + IdRepor + "';").ToString());
                    int iddescf = Convert.ToInt32(v.getaData("select idfalloEsp from cfallosesp where codfallo='" + DgvTabla.Rows[e.RowIndex].Cells[10].Value.ToString() + "';"));
                    desfallonot = DgvTabla.Rows[e.RowIndex].Cells[11].Value.ToString();
                    obs = DgvTabla.Rows[e.RowIndex].Cells[12].Value.ToString();
                    btnEditar.Visible = false;
                    lblactualizar.Visible = false;
                    cbgrupo.SelectedValue = _grupoanterior = Convert.ToInt32(v.getaData("select t4.idFalloGral as id from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t1.idfalloEsp='" + iddescf + "';"));
                    cbSubGrupo.SelectedValue = _subgrupoanterior = Convert.ToInt32(v.getaData("select t3.iddescfallo as id from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t1.idfalloEsp='" + iddescf + "';"));
                    cbcategoria.SelectedValue = _categoriaanterior = Convert.ToInt32(v.getaData("select t2.idcategoria as id from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t1.idfalloEsp='" + iddescf + "';"));
                    MySqlCommand comando = new MySqlCommand("select UPPER(concat (t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres)) as nombre,t1.idpersona from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos  where t1.credencial='" + txtConductor.Text + "' and t1.empresa='1' and t1.area='1' ", c.dbconection());
                    MySqlDataReader datareader = comando.ExecuteReader();
                    if (datareader.Read())
                    {
                        lblCredCond.Text = Convert.ToString(datareader["nombre"]);
                        idconductor = Convert.ToString(datareader["idpersona"]);
                    }
                    else
                    {
                        lblCredCond.Text = "";
                        idconductor = "";
                    }
                    datareader.Close();
                    c.dbconection().Close();
                    MySqlCommand cmd1 = new MySqlCommand("select UPPER(t1.nombre) as nombre, t1.idservicio from cservicios as t1 inner join reportesupervicion as t2 on t1.idservicio=t2.Serviciofkcservicios where t1.nombre='" + DgvTabla.Rows[e.RowIndex].Cells[5].Value.ToString() + "' and t1.status='0' order by nombre", c.dbconection());
                    MySqlDataReader dr1 = cmd1.ExecuteReader();
                    if (dr1.Read())
                    {
                        cmbServicio.DataSource = null;
                        MySqlCommand cmd2 = new MySqlCommand("Select UPPER(nombre) as servicio, idservicio from cservicios where status='1'", c.dbconection());
                        MySqlDataAdapter da3 = new MySqlDataAdapter(cmd2);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        DataRow row3 = dt3.NewRow();
                        DataRow row4 = dt3.NewRow();
                        DataRow row5 = dt3.NewRow();
                        row3["idservicio"] = dr1["idservicio"];
                        row3["servicio"] = dr1["nombre"].ToString();
                        row4["idservicio"] = 0;
                        row4["servicio"] = "--  SELECCIONE UN SERVICIO  --";
                        row5["idservicio"] = 1;
                        row5["servicio"] = "SIN SERVICIO FIJO";
                        dt3.Rows.InsertAt(row4, 0);
                        dt3.Rows.InsertAt(row5, 1);
                        dt3.Rows.InsertAt(row3, 5);
                        cmbServicio.ValueMember = "idservicio";
                        cmbServicio.DisplayMember = "servicio";
                        cmbServicio.DataSource = dt3;
                        cmbServicio.SelectedIndex = 1;
                        cmbServicio.Text = dr1["nombre"].ToString();
                        c.dbconection().Close();
                    }
                    else
                    {
                        cmbServicio.SelectedValue = _idservicio;
                    }
                    MySqlCommand cmd3 = new MySqlCommand("select t1.iddescfallo,UPPER(t1.descfallo) as descfallo from cdescfallo as t1 inner join reportesupervicion as t2 on t1.iddescfallo=t2.DescrFallofkcdescfallo where t1.descfallo='" + DgvTabla.Rows[e.RowIndex].Cells[9].Value.ToString() + "' and t1.status='0'order by t1.descfallo", c.dbconection());
                    MySqlDataReader dr2 = cmd3.ExecuteReader();
                    if (dr2.Read())
                    {
                        cbSubGrupo.DataSource = null;
                        MySqlCommand cmd4 = new MySqlCommand("Select UPPER(descfallo) as descfallo , iddescfallo from cdescfallo where status='1' order by descfallo", c.dbconection());
                        MySqlDataAdapter da3 = new MySqlDataAdapter(cmd4);
                        DataTable dt3 = new DataTable();
                        da3.Fill(dt3);
                        DataRow row3 = dt3.NewRow();
                        DataRow row4 = dt3.NewRow();
                        row3["iddescfallo"] = dr2["iddescfallo"];
                        row3["descfallo"] = dr2["descfallo"];
                        row4["iddescfallo"] = 0;
                        row4["descfallo"] = "--  SELECCIONE UNA DESCRIPCIÓN  --";
                        dt3.Rows.InsertAt(row4, 0);
                        dt3.Rows.InsertAt(row3, 25);
                        cbSubGrupo.ValueMember = "iddescfallo";
                        cbSubGrupo.DisplayMember = "descfallo";
                        cbSubGrupo.DataSource = dt3;
                        cbSubGrupo.SelectedIndex = 1;
                        cbSubGrupo.Text = dr2["descfallo"].ToString();
                        cbSubGrupo.Text = DgvTabla.Rows[e.RowIndex].Cells[9].Value.ToString();
                        c.dbconection().Close();
                    }
                    else
                    {
                        cbSubGrupo.Text = DgvTabla.Rows[e.RowIndex].Cells[9].Value.ToString();
                    }
                    MySqlCommand cmd5 = new MySqlCommand("select UPPER(t1.codfallo) as Fallo, idfalloEsp from cfallosesp as t1 inner join cdescfallo as t2 on t2.iddescfallo=t1.descfallofkcdescfallo where descfallo='" + DgvTabla.Rows[e.RowIndex].Cells[9].Value.ToString() + "'and t1.status='0' ORDER BY SUBSTRING(codfallo,LENGTh(codFALLO)-3,4)asc;",
                c.dbconection());
                    MySqlDataReader dr3 = cmd5.ExecuteReader();
                    if (dr3.Read())
                    {
                        cmbCodFallo.DataSource = null;
                        MySqlCommand cmd6 = new MySqlCommand("select UPPER(t1.codfallo) as Fallo, idfalloEsp from cfallosesp as t1 inner join cdescfallo as t2 on t2.iddescfallo=t1.descfallofkcdescfallo where descfallo='" + DgvTabla.Rows[e.RowIndex].Cells[9].Value.ToString() + "'and t1.status='1' ORDER BY SUBSTRING(codfallo,LENGTh(codFALLO)-3,4)asc;", c.dbconection());
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd6);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        DataRow row = dt.NewRow();
                        DataRow row1 = dt.NewRow();
                        row1["idfalloEsp"] = dr3["idfalloEsp"];
                        row1["Fallo"] = dr3["Fallo"];
                        row["idfalloEsp"] = 0;
                        row["Fallo"] = "--  SELECCIONE UN CÓDIGO  --";//agregamos una opción más
                        dt.Rows.InsertAt(row, 0);
                        dt.Rows.InsertAt(row1, 1);
                        cmbCodFallo.ValueMember = "idfalloEsp";
                        cmbCodFallo.DisplayMember = "Fallo";
                        cmbCodFallo.DataSource = dt;
                        cmbCodFallo.SelectedIndex = 0;
                        cmbCodFallo.Text = dr3["Fallo"].ToString();
                        cmbCodFallo.Text = DgvTabla.Rows[e.RowIndex].Cells[10].Value.ToString();
                    }
                    else
                    {
                        cmbCodFallo.Text = DgvTabla.Rows[e.RowIndex].Cells[10].Value.ToString();
                    }
                    dr3.Close();
                    c.dbconection().Close();
                    dr2.Close();
                    //dr.Close();
                    dr1.Close();
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                editar = false;
                bandera_editar = false;
                if (peditar)
                {
                    Verifica_modificaciones();
                }
                if (!bandera_c && !bandera_nuevo && !bandera_editar)
                {
                    if (peditar)
                    {
                        Unidades_sin_estatus();
                        cmbUnidad.Enabled = false;
                        restaurar_datos(e);
                        editar = true;
                        bPdf.Visible = true;
                        LblPDF.Visible = true;
                        if (lblestatus.Text == "LIBERADA")
                        {
                            if (pinsertar && peditar && pconsultar)
                            {
                                cmbUnidad.Enabled = true;
                                HabilitarCampos();
                            }
                            else
                            {
                                btnEditar.Visible = false;
                                lblactualizar.Visible = false;
                                DeshabilitarCampos();
                            }
                        }
                        else
                        {
                            HabilitarCampos();
                            txtKilometraje.MaxLength = 12;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No cuenta con los privilegios para editar un reporte".ToUpper(), "SIN PRIVILEGIOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        public void ActualizarReporte()
        {
            try
            {

                MySqlCommand sql = new MySqlCommand("SELECT concat(t1.ApPaterno,' ',t1.ApMAterno,' ',t1.Nombres)as supervisor ,t1.idPersona,t2.puesto from cpersonal as t1  inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal=t1.idPersona where t3.password='" + v.Encriptar(txtSupervisor.Text) + "'and t1.status='1' and t2.status='1' and t1.empresa='1' and t1.area='1' ", c.dbconection());
                MySqlDataReader DTR = sql.ExecuteReader();
                if (DTR.Read())
                {
                    MySqlCommand cmd1 = new MySqlCommand("select t1.credencial, t2.puesto, t1.idPersona from cpersonal as t1 inner join puestos as t2 on t2.idpuesto = t1.cargofkcargos where t1.credencial = '" + txtConductor.Text + "' AND t1.status = '1'   AND t2.status = '1' and t1.empresa='1' and t1.area='1'", c.dbconection());
                    MySqlDataReader lee = cmd1.ExecuteReader();
                    if (lee.Read())
                    {
                        double km1 = double.Parse(txtKilometraje.Text);

                        if (km1 == 0)
                        {
                            MessageBox.Show("El kilometraje debe ser mayor a 0".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            //if(condicionando formato)
                            Editar_reporte();
                        }
                    }
                    else
                    {
                        MessageBox.Show("El número de credencial es incorrecto".ToUpper(), "VERIFICAR CREDENCIAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtConductor.Focus();
                        txtConductor.Clear();
                        btnGuardar.Enabled = false;
                        btnEditar.Enabled = true;
                        bPdf.Enabled = true;
                    }
                    lee.Close();
                    c.dbconection().Close();
                }
                else
                {
                    MessageBox.Show("La contraseña ingresada es incorrecta".ToUpper(), "CONTRASEÑA INCORRECTA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtSupervisor.Focus();
                    txtSupervisor.Clear();
                }
                DTR.Close();
                c.dbconection().Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR AL ACTUALIZAR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void oculta_botones()
        {
            btnEditar.Visible = false;
            lblactualizar.Visible = false;
            bPdf.Visible = false;
            LblPDF.Visible = false;
        }
        void Editar_reporte()
        {
            observacionesEdicion obs = new observacionesEdicion(v);
            obs.Owner = this;
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string motivo = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                MySqlCommand ValidarModificaciones = new MySqlCommand("SELECT (SELECT X1.credencial FROM CPERSONAL AS X1 WHERE X1.IDPERSONA=T1.CredencialConductorfkCPersonal)AS CONDUCTOR,(SELECT UPPER(CONCAT(X3.APPATERNO,' ',X3.APMATERNO,' ',X3.NOMBRES)) FROM CPERSONAL AS X3 WHERE X3.IdPersona=T1.SupervisorfkCPersonal) AS Supervisor,(select if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios)))as 'SERVICIO', T1.KMENTRADA AS KM, UPPER(T1.TipoFallo) AS TFALLO,(SELECT UPPER(X2.descfallo) FROM cdescfallo AS X2 WHERE X2.iddescfallo=T1.DescrFallofkcdescfallo)AS DESCFALLO,T1.idreportesupervicion as id, (SELECT (UPPER(X3.codfallo)) FROM cfallosesp AS X3 WHERE X3.idfalloEsp=T1.CodFallofkcfallosesp) AS CODFALLO,UPPER(T1.DescFalloNoCod) AS DESFALLONOT, UPPER(T1.ObservacionesSupervision) AS OBSER FROM REPORTESUPERVICION AS T1 INNER JOIN CUNIDADES AS T2 ON T1.UNIDADFKCUNIDADES=T2.IDUNIDAD WHERE T1.FOLIO='" + lblFolio.Text + "' and T1.idreportesupervicion=T1.idreportesupervicion;", c.dbconection());
                MySqlDataReader DR = ValidarModificaciones.ExecuteReader();
                if (DR.Read())
                {
                    IdRepor = Convert.ToString(DR["id"]);
                    credencial = Convert.ToString(DR["CONDUCTOR"]);
                    supervissor = Convert.ToString(DR["Supervisor"]);
                    servicio = Convert.ToString(DR["SERVICIO"]);
                    km = Convert.ToString(DR["KM"]);
                    tipofallo = Convert.ToString(DR["TFALLO"]);
                    descfallo = Convert.ToString(DR["DESCFALLO"]);
                    codfallo = Convert.ToString(DR["CODFALLO"]);
                    desfallonot = Convert.ToString(DR["DESFALLONOT"]);
                    observaciones = Convert.ToString(DR["OBSER"]);
                    if (credencial == txtConductor.Text && supervissor == lblSupervisor.Text && servicio == cmbServicio.Text && km == txtKilometraje.Text && tipo == cmbTipoFallo.Text && observaciones == txtObserSupervicion.Text && ((desfallonot == txtDescFalloNoC.Text && cbgrupo.SelectedIndex == 0) || ((int)cbgrupo.SelectedValue == grupo_anterior && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))))
                    {
                        DialogResult oDlgRes;
                        MessageBox.Show("No se realizaron modificaciones".ToUpper(), "SIN MODIFICACIONES", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        oDlgRes = MessageBox.Show("¿Desea limpiar todos los campos?".ToUpper(), "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (oDlgRes == DialogResult.Yes)
                        {
                            esta_exportando();
                            btnEditar.Enabled = false;
                            btnGuardar.Enabled = true;
                            cmbUnidad.Enabled = true;
                            cargarDAtos();
                            limpia_act();
                            Genera_Folio();
                            HabilitarCampos();
                            oculta_botones();
                            LimpiarReporte();
                        }
                    }
                    else
                    {

                        string consulta = "Update reportesupervicion as t1 set t1.CredencialConductorfkCPersonal='" + Convert.ToInt32(idconductor) + "', t1.Serviciofkcservicios='" + cmbServicio.SelectedValue + "', t1.KmEntrada='" + txtKilometraje.Text + "', t1.TipoFallo='" + cmbTipoFallo.Text + "', t1.ObservacionesSupervision='" + txtObserSupervicion.Text.Trim() + "'";
                        if (cbgrupo.SelectedIndex == 0)
                        {
                            consulta += ",t1.DescFalloNoCod = '" + txtDescFalloNoC.Text.Trim() + "',t1.DescrFallofkcdescfallo=null,t1.CodFallofkcfallosesp=null";
                        }
                        else
                        {
                            consulta += ",t1.DescrFallofkcdescfallo='" + cbSubGrupo.SelectedValue + "', t1.CodFallofkcfallosesp='" + cmbCodFallo.SelectedValue + "',t1.DescFalloNoCod=null";
                        }
                        consulta += " WHERE t1.Folio='" + lblFolio.Text + "';";
                        if (v.c.insertar(consulta))
                            MessageBox.Show("Registro actualizado exitosamente ".ToUpper(), "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        c.dbconection().Close();
                        Modificaciones_tabla(motivo);
                        esta_exportando();
                        cmbCodFallo.Enabled = false;
                        oculta_botones();
                        bandera_editar = true;
                        btnGuardar.Enabled = true;
                        cmbUnidad.Enabled = true;
                        limpiarmant();
                        cargarDAtos();
                        limpia_act();
                        Genera_Folio();
                        HabilitarCampos();
                        DgvTabla.ClearSelection();
                        LimpiarReporte();
                    }
                }
                DR.Close();
            }
        }
        void Modificaciones_tabla(string motivo)
        {
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Reporte de Supervisión','" + IdRepor + "','" + _unidad + ";" + supervissor + ";" + credencial + ";" + servicio + ";" + km + ";" + tipofallo + ";";
            if (!string.IsNullOrWhiteSpace(desfallonot))
            {
                sql += desfallonot + ";";
            }
            else
            {
                sql += descfallo + ";" + codfallo + ";";
            }

            if (!string.IsNullOrWhiteSpace(observaciones))
            {
                sql += observaciones;
            }
            else
            {
                sql += "";
            }
            sql += "','" + idsupervisor + "',NOW(),'Actualización de Reporte de Supervisión','" + motivo + "','1','1')";
            v.c.insertar(sql);
        }
        void Exportacion()
        {
            MySqlCommand exportacion = new MySqlCommand("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Reporte de Supervisión','" + IdRepor + "','Exportación de reporte en archivo pdf','" + '1' + "',NOW(),'Exportación a PDF de reporte de supervisión','1','1')", c.dbconection());
            exportacion.ExecuteNonQuery();
        }
        string id, Folio;
        void Exportación_Excel()
        {
            int contador = 0;
            string sql = "INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Reporte de Supervisión','0','";
            foreach (DataRow row in dt.Rows)
            {
                contador++;
                id = row[0].ToString();
                Folio = v.getaData("Select t1.idreportesupervicion from reportesupervicion as t1 where '" + id + "'=t1.folio").ToString();
                if (contador < dt.Rows.Count)
                {
                    Folio += ";";
                }
                sql += Folio;
            }
            sql += "','1',NOW(),'Exportación a Excel de reportes de supervisión','1','1')";
            v.c.insertar(sql);
            dt = new DataTable();
        }
        void actualiza_datos()
        {
            if (string.IsNullOrWhiteSpace(lblFolio.Text))
            {
                MessageBox.Show("El campo \"folio\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bandera_c = true;
            }
            else
            {
                if (cmbUnidad.SelectedIndex == 0)
                {
                    MessageBox.Show("El campo \"unidad\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    bandera_c = true;
                    cmbUnidad.Focus();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtSupervisor.Text))
                    {
                        MessageBox.Show("El campo \"contraseña\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        bandera_c = true;
                        txtSupervisor.Focus();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtConductor.Text))
                        {
                            MessageBox.Show("El campo \"credencial de conductor\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            bandera_c = true;
                            txtConductor.Focus();
                        }
                        else
                        {
                            if (cmbServicio.SelectedIndex == 0)
                            {
                                MessageBox.Show("El campo \"servicio\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                bandera_c = true;
                                cmbServicio.Focus();
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(txtKilometraje.Text))
                                {
                                    MessageBox.Show("El campo \"kilometraje de entrada a patio\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    bandera_c = true;
                                    txtKilometraje.Focus();
                                }
                                else
                                {
                                    if (cmbTipoFallo.SelectedIndex == 0)
                                    {
                                        MessageBox.Show("El campo \"tipo de falla\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        bandera_c = true;
                                        cmbTipoFallo.Focus();
                                    }
                                    else
                                    {
                                        if (cmbCodFallo.SelectedIndex == 0 && cbSubGrupo.SelectedIndex == 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))
                                        {
                                            MessageBox.Show("Campos vacios en \"la sección de fallos\"".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            bandera_c = true;
                                        }
                                        else
                                        {
                                            if (cbSubGrupo.SelectedIndex == 0)
                                            {
                                                MessageBox.Show("El campo \"SUbgrupo\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            }
                                            else
                                            {
                                                if (cbcategoria.SelectedIndex == 0)
                                                {
                                                    MessageBox.Show("El campo \"Categoria\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                }
                                                else
                                                {
                                                    if (cmbCodFallo.SelectedIndex == 0)
                                                    {
                                                        MessageBox.Show("El campo \"Código de fallo\" se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    }
                                                    else
                                                    {
                                                        if (cbgrupo.SelectedIndex > 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text.Trim()))
                                                        {
                                                            ActualizarReporte();
                                                        }
                                                        else
                                                        {
                                                            if (cbgrupo.SelectedIndex == 0 && !string.IsNullOrWhiteSpace(txtDescFalloNoC.Text.Trim()))
                                                            {
                                                                ActualizarReporte();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        Thread exportar;
        delegate void El_Delegado();
        void cargando()
        {
            pictureBox2.Image = Properties.Resources.loader;
            btnExcel.Visible = false;
            LblExcel.Text = "EXPORTANDO";
        }
        delegate void El_Delegado1();
        public void consulta_categorias()
        {
            v.iniCombos("select upper(t3.categoria) as c,t3.idcategoria as id from cdescfallo as t1 inner join catcategorias as t3 on t3.subgrupofkcdescfallo=t1.iddescfallo where iddescfallo='" + cbSubGrupo.SelectedValue + "' order by categoria;", cbcategoria, "id", "c", "--SELECCIONE CATEGORIA--");
        }
        private void cmbDescFallo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSubGrupo.SelectedIndex > 0)
            {
                consulta_categorias();
                cbcategoria.Enabled = true;
            }
            else
            {
                cbcategoria.DataSource = null;
                cbcategoria.Enabled = false;
            }
        }

        private void txtObserSupervicion_Validating(object sender, CancelEventArgs e)
        {
            txtObserSupervicion.Text = txtObserSupervicion.Text.Replace(Environment.NewLine, "");
            while (txtObserSupervicion.Text.Contains("  "))
            {
                txtObserSupervicion.Text = txtObserSupervicion.Text.Replace("  ", " ").Trim();
            }
        }
        public void consulta_subgrupos()
        {
            v.iniCombos("select upper(descfallo) as d,iddescfallo as id from cdescfallo as t1 inner join cfallosgrales as t2 on t2.idfallogral=t1.falloGralfkcfallosgrales where t2.idfallogral='" + cbgrupo.SelectedValue + "' order by descfallo;", cbSubGrupo, "id", "d", "--SELECCIONE SUBGRUPO--");
        }
        private void cbgrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbgrupo.SelectedIndex > 0)
            {
                consulta_subgrupos();
                cbSubGrupo.Enabled = true;
                txtDescFalloNoC.Clear();
                txtDescFalloNoC.Enabled = false;
            }
            else
            {
                txtDescFalloNoC.Enabled = true;
                cbSubGrupo.DataSource = null;
                cbSubGrupo.Enabled = false;
            }
        }

        private void cmbCodFallo_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlCommand descfallo_o = new MySqlCommand("Select UPPER(falloesp) as fallo from cfallosesp where codfallo='" + cmbCodFallo.Text + "';", c.dbconection());
            MySqlDataReader DR = descfallo_o.ExecuteReader();
            if (DR.Read())
            {
                lblDescFallo.Text = (Convert.ToString(DR["fallo"]));
            }
            else
            {
                lblDescFallo.Text = "";
            }
            DR.Close();
            c.dbconection().Close();
        }

        public void consulta_codigos()
        {
            v.iniCombos("select upper(t1.codfallo)as c,t1.idfalloEsp as id from cfallosesp as t1 inner join catcategorias as t2 on t2.idcategoria=t1.descfallofkcdescfallo inner join cdescfallo as t3 on t2.subgrupofkcdescfallo=t3.iddescfallo inner join cfallosgrales as t4 on t3.falloGralfkcfallosgrales=t4.idFalloGral where t2.idcategoria='" + cbcategoria.SelectedValue + "';", cmbCodFallo, "id", "c", "--SELECCIONE CÓDIGO");
        }
        private void cbcategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbcategoria.SelectedIndex > 0)
            {
                cmbCodFallo.Enabled = true;
                consulta_codigos();
            }
            else
            {
                cmbCodFallo.DataSource = null;
                cmbCodFallo.Enabled = false;
            }
        }

        private void txtDescFalloNoC_Validating(object sender, CancelEventArgs e)
        {
            while (txtDescFalloNoC.Text.Contains("  "))
            {
                txtDescFalloNoC.Text = txtDescFalloNoC.Text.Replace("  ", " ").Trim();
            }
        }

        private void Supervisión_FormClosing(object sender, FormClosingEventArgs e)
        {
            hilo.Abort();
        }

        private void DgvTabla_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GpbBusquedas_Enter(object sender, EventArgs e)
        {

        }

        private void cmbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmpresa.SelectedIndex > 0)
            {
                string sql = "SELECT idunidad ,concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where upper(nombreEmpresa)='" + cmbEmpresa.Text + "';";
                MySqlCommand cmd = new MySqlCommand(sql, c.dbconection());
                if (Convert.ToInt32(cmd.ExecuteScalar()) != 0)
                {
                    cmbBuscarUnidad.DataSource = null;
                    DataTable dt = (DataTable)v.getData("SELECT concat(t2.identificador,LPAD(consecutivo,4,'0')) as eco, idunidad  FROM cunidades as t1 INNER JOIN careas as t2 ON t1.areafkcareas= t2.idarea inner join cempresas as t3 on t3.idempresa=t2.empresafkcempresas where upper(nombreEmpresa)='" + cmbEmpresa.Text + "';");
                    DataRow nuevaFila = dt.NewRow();
                    nuevaFila["idunidad"] = 0;
                    nuevaFila["eco"] = "--Seleccione un ECO--".ToUpper();
                    dt.Rows.InsertAt(nuevaFila, 0);
                    cmbBuscarUnidad.DisplayMember = "eco";
                    cmbBuscarUnidad.ValueMember = "idunidad";
                    cmbBuscarUnidad.DataSource = dt;
                    cmbBuscarUnidad.Enabled = true;
                }
                else
                {
                    MessageBox.Show("La empresa seleccionada no cuenta con unidades registradas en el sistema", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbEmpresa.SelectedIndex = 0;
                    cmbBuscarUnidad.Enabled = false;
                }
            }
            else
            {
                cmbBuscarUnidad.DataSource = null;
                cmbBuscarUnidad.Enabled = false;
            }
        }

        private void cmbUnidad_SelectedValueChanged(object sender, EventArgs e)
        {
            cmbBuscarUnidad.DataSource = null;
            DataTable dt = (DataTable)v.getData("select upper(Nombre) as nombre,idservicio as id from cservicios as t1 inner join careas as t2 on t1.AreafkCareas=t2.idarea inner join cunidades as t3 on t3.areafkcareas=t2.idarea where idunidad ='" + cmbUnidad.SelectedValue + "' order by nombre;");
            DataRow nuevaFila = dt.NewRow();
            DataRow _sin = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["nombre"] = "--Seleccione un servicio--".ToUpper();
            _sin["id"] = 1;
            _sin["nombre"] = "sin servicio".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            dt.Rows.InsertAt(_sin, 1);
            cmbServicio.DisplayMember = "nombre";
            cmbServicio.ValueMember = "id";
            cmbServicio.DataSource = dt;
            if (cmbUnidad.SelectedIndex == 0)
            {
                cmbServicio.DataSource = null;
                cmbServicio.Enabled = false;
            }
            else
            {
                cmbServicio.Enabled = true;
            }
        }
        public void TextoLargo(object sender, EventArgs e)
        {
            if (((Label)sender).Text.Length >= 30)
            {
                ((Label)sender).Font = new System.Drawing.Font("Garamond", 10);
            }
            else
            {
                ((Label)sender).Font = new System.Drawing.Font("Garamond", 12);
            }
        }

        public void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            realiza_busquedas();
        }
        void esta_exportando()
        {
            if (peditar && pinsertar && pconsultar)
            {
                if (LblExcel.Text.Equals("EXPORTANDO"))
                {
                    exportando = true;
                }
                else
                {
                    btnExcel.Visible = false;
                    LblExcel.Visible = false;
                }
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarDAtos();
            btnActualizar.Visible = false;
            LblActTabla.Visible = false;
            esta_exportando();
        }
        bool estado = false;
        private void btnExcel_Click(object sender, EventArgs e)
        {
            estado = true;
            ThreadStart delegado = new ThreadStart(exporta_a_excel);
            exportar = new Thread(delegado);
            exportar.Start();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardar_reporte();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            bandera = true;
            editar = false;
            mensaje = false;
            Verifica_modificaciones();
        }

        private void bPdf_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblSupervisor.Text))
            {
                MessageBox.Show("El campo supervisor se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtConductor.Text))
                {
                    MessageBox.Show("El campo credencial de conductor se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (cmbServicio.SelectedIndex == 0)
                    {
                        MessageBox.Show("El campo servicio se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(txtKilometraje.Text))
                        {
                            MessageBox.Show("El campo kilometraje se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (cmbTipoFallo.SelectedIndex == 0)
                            {
                                MessageBox.Show("El campo tipo de fallo se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                if (cbSubGrupo.SelectedIndex == 0 && cmbCodFallo.SelectedIndex == 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))
                                {
                                    MessageBox.Show("Campos vacios en la sección de fallos".ToUpper(), "CAMPOS VACIOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    if (cmbCodFallo.SelectedIndex == 0 && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text))
                                    {
                                        MessageBox.Show("EL campo código de fallo se encuentra vacio".ToUpper(), "CAMPO VACIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        To_pdf();//Llamamos a nuestro método To_pdf               
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            MySqlCommand Verifica_Estatus = new MySqlCommand("select coalesce((select x1.estatus from reportemantenimiento as x1 where x1.FoliofkSupervicion=t1.idreportesupervicion),'') AS ESTATUS FROM reportesupervicion as t1 where t1.folio='" + lblFolio.Text + "';", c.dbconection());
            MySqlDataReader DR = Verifica_Estatus.ExecuteReader();
            if (DR.Read())
            {
                string es = DR["ESTATUS"].ToString();
                if (es == "LIBERADA" && (!peditar && !pinsertar && !pconsultar))
                {
                    MessageBox.Show("La unidad ya se encuentra liberada, ya no es posible realizar modificaciones".ToUpper(), "UNIDAD LIBERADA", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimpiarReporte();
                    Genera_Folio();
                    limpiarmant();
                    esta_exportando();
                    cargarDAtos();
                    limpia_act();
                }
                else
                {
                    actualiza_datos();
                }
            }
        }

        private void btnGuardar_MouseMove(object sender, MouseEventArgs e)
        {
            ((Button)sender).Size = new Size(58, 53);
        }

        private void btnGuardar_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).Size = new Size(55, 50);
        }

        void cargando1()
        {
            pictureBox2.Image = null;
            btnExcel.Visible = true;
            LblExcel.Text = "EXPORTAR";
            if (exportando)
            {
                LblExcel.Visible = false;
                btnExcel.Visible = false;
            }
            exportando = false;
            estado = false;
        }

        private void cmbDescFallo_Validating(object sender, CancelEventArgs e)
        {
            if (cbSubGrupo.SelectedIndex > 0)
            {
                txtDescFalloNoC.Enabled = false;
            }
        }

        private void cmbUnidad_DrawItem(object sender, DrawItemEventArgs e)
        {
            combos_DrawItem(sender, e);
        }

        private void DgvTabla_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        DataTable dt = new DataTable();
        public void exporta_a_excel()//Método para exportar a EXCEL.
        {
            if (DgvTabla.Rows.Count > 0)
            {

                dt = (DataTable)DgvTabla.DataSource;

                if (this.InvokeRequired)
                {
                    El_Delegado delega = new El_Delegado(cargando);
                    this.Invoke(delega);
                }
                Microsoft.Office.Interop.Excel.Application X = new Microsoft.Office.Interop.Excel.Application();
                X.Application.Workbooks.Add(Type.Missing);
                h.Worksheet sheet = X.ActiveSheet;
                X.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                X.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    h.Range rng = (h.Range)sheet.Cells[1, i + 1];
                    sheet.Cells[1, i + 1] = dt.Columns[i].ColumnName.ToUpper();
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
                        //foreach (DataGridViewColumn colk in dataGridView3.Columns)
                        //{
                        //if (DgvTabla.Columns[j].Visible == true)
                        //{
                        try
                        {

                            h.Range rng = (h.Range)sheet.Cells[i + 2, j + 1];
                            sheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                            rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(231, 230, 230));
                            rng.Borders.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            rng.Cells.Font.Name = "Calibri";
                            rng.Cells.Font.Size = 11;
                            rng.Font.Bold = false;
                            if (dt.Rows[i][j].ToString() == "PREVENTIVO" || dt.Rows[i][j].ToString() == "LIBERADA")
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.PaleGreen);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dt.Rows[i][j].ToString() == "CORRECTIVO" || dt.Rows[i][j].ToString() == "EN PROCESO")
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.Khaki);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dt.Rows[i][j].ToString() == "REITERATIVO" || dt.Rows[i][j].ToString() == "REPROGRAMADA")
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightCoral);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dt.Rows[i][j].ToString() == "REPROGRAMADO")
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightBlue);
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                            if (dt.Rows[i][j].ToString() == "SEGUIMIENTO")
                            {
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.FromArgb(246, 144, 123));
                                rng.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Black);
                            }
                        }
                        catch (System.NullReferenceException EX)
                        {
                            MessageBox.Show(EX.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //}
                        //else
                        //{
                        //}
                        //}
                    }
                }
                Thread.Sleep(400);
                X.Columns.AutoFit();
                X.Rows.AutoFit();
                X.Visible = true;
                // Exportación_Excel();
                try
                {
                    if (this.InvokeRequired)
                    {
                        El_Delegado1 delega2 = new El_Delegado1(cargando1);
                        this.Invoke(delega2);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No hay registros en la tabla para exportar".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtKilometraje_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbBuscStatus_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color c = Color.BlueViolet;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color color = Color.FromArgb(246, 144, 123);
            SolidBrush s = new SolidBrush(color);
            Color fondo = Color.FromArgb(200, 200, 200);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds);
                    e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
                    break;
                case 1:
                    e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
                case 2:
                    e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
                case 3:
                    e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);
                    e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
            }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
            {
                e.Graphics.DrawString(cmbBuscStatus.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
            }
        }
        string contraseña;
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (editar && peditar)
            {
                //if(!string.IsNullOrWhiteSpace(txtSupervisor.Text) && !string.IsNullOrWhiteSpace(supervissor))contraseña = v.Desencriptar(v.getaData("SELECT coalesce(PASSWORD,'') FROM DATOSISTEMA AS T1 INNER JOIN CPERSONAL AS T2 ON T2.IDPERSONA=T1.usuariofkcpersonal WHERE upper(concat(APPATERNO,' ',APMATERNO,' ',NOMBRES))='" + supervissor + "'").ToString());
                if ((obs != txtObserSupervicion.Text.Trim() || tipofallo != cmbTipoFallo.Text || km != txtKilometraje.Text || servicio != cmbServicio.Text || credencial != txtConductor.Text || (((_grupoanterior != Convert.ToInt32(cbgrupo.SelectedValue) && cbgrupo.SelectedIndex > 0) || (_subgrupoanterior != Convert.ToInt32(cbSubGrupo.SelectedValue) && cbSubGrupo.SelectedIndex > 0) || (_categoriaanterior != Convert.ToInt32(cbcategoria.SelectedValue) && cbcategoria.SelectedIndex > 0)) && string.IsNullOrWhiteSpace(txtDescFalloNoC.Text)) || (desfallonot != txtDescFalloNoC.Text.Trim() && !string.IsNullOrWhiteSpace(txtDescFalloNoC.Text) && cbgrupo.SelectedIndex == 0)) && (cmbUnidad.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtConductor.Text) && cmbServicio.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtKilometraje.Text) && cmbTipoFallo.SelectedIndex > 0))
                {
                    btnEditar.Visible = true;
                    lblactualizar.Visible = true;
                }
                else
                {
                    btnEditar.Visible = false;
                    lblactualizar.Visible = false;
                }
            }
        }
        private void txtObserSupervicion_Validating_1(object sender, CancelEventArgs e)
        {
            while (txtObserSupervicion.Text.Contains("  ") || txtObserSupervicion.Text.Contains("\n"))
            {
                txtObserSupervicion.Text = txtObserSupervicion.Text.Replace("\n", "").Trim();
                txtObserSupervicion.Text = txtObserSupervicion.Text.Replace("  ", " ").Trim();
                txtObserSupervicion.SelectionStart = txtObserSupervicion.TextLength + 1;
            }
        }

        private void txtDescFalloNoC_Validating_1(object sender, CancelEventArgs e)
        {
            while (txtDescFalloNoC.Text.Contains("  "))
            {
                txtDescFalloNoC.Text = txtDescFalloNoC.Text.Replace("  ", " ").Trim();
                txtDescFalloNoC.SelectionStart = txtDescFalloNoC.TextLength;
            }
        }
        public void consulta_grupos()
        {
            v.iniCombos("select upper(nombreFalloGral) as n, idFalloGral as id from cfallosgrales where status='1' order by nombreFalloGral;", cbgrupo, "id", "n", "--SELECCIONE GRUPO--");
        }
        public void consulta_descripciones()
        {
            v.iniCombos("SELECT UPPER(t1.descfallo) as descfallo,(t1.iddescfallo) from cdescfallo as t1 where t1.status='1'  order by descfallo;", cbSubGrupo, "iddescfallo", "descfallo", "--SELECCIONE DESCRIPCIÓN--");
        }

        void Verifica_modificaciones()
        {
            if (cmbUnidad.SelectedIndex > 0 || lblSupervisor.Text != "" || txtConductor.Text != "" || cmbServicio.SelectedIndex > 0 || txtKilometraje.Text != "" || cmbTipoFallo.SelectedIndex > 0 || ((cbSubGrupo.SelectedIndex > 0 || cmbCodFallo.SelectedIndex > 0) || (txtDescFalloNoC.Text != "")) || txtObserSupervicion.Text != "" || cbgrupo.SelectedIndex > 0 || cbcategoria.SelectedIndex > 0)
            {
                DialogResult respuesta = new DialogResult();
                MySqlCommand CMD = new MySqlCommand("SELECT T1.IDREPORTESUPERVICION AS ID, (SELECT X1.credencial FROM CPERSONAL AS X1 WHERE X1.IDPERSONA = T1.CredencialConductorfkCPersonal)AS CONDUCTOR,(SELECT UPPER(CONCAT(X3.APPATERNO, ' ', X3.APMATERNO, ' ', X3.NOMBRES)) FROM CPERSONAL AS X3 WHERE X3.IdPersona = T1.SupervisorfkCPersonal) AS Supervisor,(select if(t1.Serviciofkcservicios=1,'SIN SERVICIO',(select upper(x13.Nombre) from cservicios as x13 where x13.idservicio=t1.Serviciofkcservicios))) AS SERVICIO, T1.KMENTRADA AS KM, UPPER(T1.TipoFallo) AS TFALLO,COALESCE((SELECT UPPER(X2.descfallo) FROM cdescfallo AS X2 WHERE X2.iddescfallo = T1.DescrFallofkcdescfallo),'')AS DESCFALLO, coalesce((SELECT UPPER(X3.codfallo) FROM cfallosesp AS X3 WHERE X3.idfalloEsp = T1.CodFallofkcfallosesp),'') AS CODFALLO, UPPER(T1.DescFalloNoCod) AS DESFALLONOT, UPPER(T1.ObservacionesSupervision) AS OBSER,COALESCE((SELECT S1.ESTATUS FROM reportemantenimiento AS S1 WHERE S1.FoliofkSupervicion=T1.idReporteSupervicion),'') AS ESTATUS FROM REPORTESUPERVICION AS T1 INNER JOIN CUNIDADES AS T2 ON T1.UNIDADFKCUNIDADES = T2.IDUNIDAD  WHERE T1.FOLIO = '" + lblFolio.Text + "' and T1.idreportesupervicion = T1.idreportesupervicion; ", c.dbconection());
                MySqlDataReader DR = CMD.ExecuteReader();
                if (DR.Read())
                {
                    IdRepor = Convert.ToString(DR["ID"]);
                    credencial = Convert.ToString(DR["CONDUCTOR"]);
                    supervissor = Convert.ToString(DR["Supervisor"]);
                    servicio = Convert.ToString(DR["SERVICIO"]);
                    km = Convert.ToString(DR["KM"]);
                    tipofallo = Convert.ToString(DR["TFALLO"]);
                    descfallo = Convert.ToString(DR["DESCFALLO"]);
                    codfallo = Convert.ToString(DR["CODFALLO"]);
                    desfallonot = Convert.ToString(DR["DESFALLONOT"]);
                    observaciones = Convert.ToString(DR["OBSER"]);
                    es = DR["ESTATUS"].ToString();
                    if (!string.IsNullOrWhiteSpace(IdRepor) && (!string.IsNullOrWhiteSpace(lblSupervisor.Text) && !string.IsNullOrWhiteSpace(lblCredCond.Text)) && (es != "LIBERADA" || (es == "LIBERADA" && peditar && pinsertar && pconsultar)) && (credencial != txtConductor.Text || servicio != cmbServicio.Text || km != txtKilometraje.Text || tipofallo != cmbTipoFallo.Text || observaciones != txtObserSupervicion.Text) || (desfallonot != txtDescFalloNoC.Text && (string.IsNullOrWhiteSpace(descfallo) && string.IsNullOrWhiteSpace(codfallo) && _grupoanterior == 0 && _subgrupoanterior == 0 && _categoriaanterior == 0)) || ((descfallo != cbSubGrupo.Text || codfallo != cmbCodFallo.Text || _grupoanterior != Convert.ToInt32(cbgrupo.SelectedValue) || _subgrupoanterior != Convert.ToInt32(cbSubGrupo.SelectedValue) || _categoriaanterior != Convert.ToInt32(cbcategoria.SelectedValue)) && string.IsNullOrWhiteSpace(desfallonot)))
                    {
                        respuesta = MessageBox.Show("¿Desea guardar las modificaciones?".ToUpper(), "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (respuesta == DialogResult.Yes)
                        {
                            mensaje = true;
                            actualiza_datos();
                            bPdf.Visible = false;
                            LblPDF.Visible = false;
                            btnEditar.Visible = false;
                            lblactualizar.Visible = false;
                        }
                        else
                        {
                            Genera_Folio();
                            cmbUnidad.Enabled = true;
                            btnGuardar.Enabled = true;
                            cmbCodFallo.Enabled = false;
                            LimpiarReporte();
                            limpiarmant();
                            oculta_botones();
                            HabilitarCampos();
                            cmbUnidad.Focus();
                        }
                    }
                    else
                    {
                        Genera_Folio();
                        cmbUnidad.Enabled = true;
                        HabilitarCampos();
                        btnGuardar.Enabled = true;
                        cmbCodFallo.Enabled = false;
                        LimpiarReporte();
                        oculta_botones();
                        limpiarmant();
                    }
                }
                else
                {
                    respuesta = MessageBox.Show("¿Desea concluir el reporte?".ToUpper(), "ADVERTENCIA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta != DialogResult.Yes)
                    {
                        if (bandera)
                        {
                            LimpiarReporte();
                            limpiarmant();
                        }
                    }
                }
            }
        }
        private void txtObserSupervicion_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (bandera_editar) btnGuardar_Click(null, e); else btnEditar_Click(null, e);
            }
            else
            {
                //Validación de letras y carácteres permitidos para ingresar en la caja de texto
                if (e.KeyChar == 44 || e.KeyChar == 46 || e.KeyChar == 127 || e.KeyChar == 08 || e.KeyChar == 32 || Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("SOLO SE ACEPTAN LETRAS, NUMEROS   ,   Y    .   ", "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void dtpFechaDe_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void cmbTipoFallo_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color c = Color.BlueViolet;
            Color color_fuente = Color.FromArgb(75, 44, 52);
            Color color = Color.FromArgb(246, 144, 123);
            SolidBrush s = new SolidBrush(color);
            Color fondo = Color.FromArgb(200, 200, 200);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(fondo), e.Bounds);
                    e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
                    break;
                case 1:
                    e.Graphics.FillRectangle(Brushes.Khaki, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
                case 2:
                    e.Graphics.FillRectangle(Brushes.PaleGreen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
                case 3:
                    e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);
                    e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
                case 4:
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
                    e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
                case 5:
                    e.Graphics.FillRectangle(s, e.Bounds);
                    e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
                    break;
            }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Crimson, e.Bounds);
                e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds, sf);
            }
            else
            {
                e.Graphics.DrawString(cmbTipoFallo.Items[e.Index].ToString(), e.Font, new SolidBrush(color_fuente), e.Bounds, sf);
            }
        }
        private void txtKilometraje_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtKilometraje.Text))
                {
                    double km = double.Parse(txtKilometraje.Text);
                    if (txtKilometraje.TextLength <= 3)
                    {
                        txtKilometraje.Text = string.Format("{0:F2}", km);
                    }
                    else
                    {
                        txtKilometraje.Text = Convert.ToString((Math.Floor(km * 100) / 100));
                        km = double.Parse(txtKilometraje.Text);
                        txtKilometraje.Text = string.Format("{0:N2}", km);
                        if (km > 2000000)
                        {
                            txtKilometraje.Text = "2,000,000.00";
                        }
                    }
                    Regex r4 = new Regex(@"^\d{1,3}\,\d{3}\.\d{2,2}$");
                    Regex r5 = new Regex(@"^\d{1,3}\,\d{3}\,\d{3}\.\d{2,2}");
                    Regex r1 = new Regex(@"^\d{0,3}\.\d{1,2}$");
                    if (!r1.IsMatch(txtKilometraje.Text) && !r4.IsMatch(txtKilometraje.Text) && !r5.IsMatch(txtKilometraje.Text))
                    {
                        MessageBox.Show("El formato del kilometraje ingresado es incorrecto".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtKilometraje.Focus();
                        txtKilometraje.Clear();
                    }

                }
            }
            catch
            {
                MessageBox.Show("El formato del kilometraje ingresado es incorrecto".ToUpper(), "KILOMETRAJE INCORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKilometraje.Focus();
                txtKilometraje.Clear();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                dtpFechaDe.Enabled = true;
                dtpFechaA.Enabled = true;
                cmbMeses.Enabled = false;
                cmbMeses.SelectedIndex = 0;
            }
            else
            {
                dtpFechaA.Enabled = false;
                dtpFechaDe.Enabled = false;
                cmbMeses.Enabled = true;
            }
        }

        private void txtKilometraje_KeyPress(object sender, KeyPressEventArgs e)
        {
            char signo_decimal = (char)46;
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == 46)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Solo se aceptan: numéros y ( . ) en este campo".ToUpper(), "CARACTERES NO PERMITIDOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (e.KeyChar == 46)
            {
                if (txtKilometraje.Text.LastIndexOf(signo_decimal) >= 0)
                {
                    e.Handled = true; // Interceptamos la pulsación para que no permitirla.
                }
            }
        }

        private void txtSupervisor_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSupervisor.Text))
            {
                //Validación de la contraseña del supervisor, 
                MySqlCommand sql = new MySqlCommand("SELECT UPPER(concat(t1.ApPaterno,' ',t1.ApMAterno,' ',t1.Nombres))as supervisor ,t1.idPersona,t2.puesto from cpersonal as t1  inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal=t1.idPersona inner join cempresas as t4 on t4.idempresa=t1.empresa where t3.password='" + v.Encriptar(txtSupervisor.Text) + "'and t1.status='1' and t2.status='1' and t1.empresa='1' and t1.area='1' ", c.dbconection());
                MySqlDataReader cmd = sql.ExecuteReader();
                if (cmd.Read())
                {
                    //en caso correcto mostramos su nombre en en label 
                    idsupervisor = Convert.ToString(cmd["idpersona"]);
                    if ((Convert.ToInt32(v.getaData("select count(insertar) from privilegios as t1 inner join cpersonal as t2 on t2.idpersona=t1.usuariofkcpersonal where t2.idpersona='" + idsupervisor + "' and namform='Form1';").ToString()) > 0) || (Convert.ToInt32(v.getaData("select count(editar) from privilegios as t1 inner join cpersonal as t2 on t2.idpersona=t1.usuariofkcpersonal where t2.idpersona='" + idsupervisor + "' and namform='Form1';").ToString()) > 0))
                    {
                        lblSupervisor.Text = Convert.ToString(cmd["supervisor"]);
                    }
                    else lblSupervisor.Text = "";
                }
                else
                {
                    //En caso contrario no mostramos nada
                    idsupervisor = "";
                    lblSupervisor.Text = "";
                }
                c.dbconection().Close();//Cerramos la conexión con la base de datos
                cmd.Close();
            }
        }

        private void txtConductor_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtConductor.Text))
            {
                string creden = txtConductor.Text;
                //COnsulta para validar la credencial de conductor, con cargo y estatus.
                MySqlCommand sql1 = new MySqlCommand("select UPPER(concat(t1.ApPaterno,' ',t1.ApMaterno,' ',t1.nombres))as Conductor,t2.puesto,t1.idPersona from cpersonal as t1 inner join puestos as t2 on t2.idpuesto=t1.cargofkcargos where t1.credencial='" + Convert.ToString(creden) + "' AND t1.status='1'   AND t2.status='1' and t1.empresa='1' and t1.area='1' ;", c.dbconection());
                MySqlDataReader cmd1 = sql1.ExecuteReader();
                if (cmd1.Read())
                {
                    //En caso correcto mostramos nombre en label           
                    lblCredCond.Text = (Convert.ToString(cmd1["Conductor"]));
                    idconductor = Convert.ToString(cmd1["idpersona"]);
                }
                else
                {
                    //En caso contrario no mostramos nada.
                    idconductor = "";
                    lblCredCond.Text = "";
                }
                cmd1.Close();
                c.dbconection().Close();//Cerramos la conexión      
            }
        }

        private void dtpFechaA_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
        private void dataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.DgvTabla.Columns[e.ColumnIndex].Name == "TIPO DE FALLO")
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
            if (this.DgvTabla.Columns[e.ColumnIndex].Name == "ESTATUS")
            {
                if (Convert.ToString(e.Value) == "EN PROCESO")
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "LIBERADA")
                    {
                        e.CellStyle.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        if (Convert.ToString(e.Value) == "REPROGRAMADA")
                        {
                            e.CellStyle.BackColor = Color.LightCoral;
                        }
                    }
                }
            }
        }

        string idconductor;
    }
}

