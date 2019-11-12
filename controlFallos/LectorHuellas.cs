using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GriauleFingerprintLibrary;
using GriauleFingerprintLibrary.Exceptions;
using System.Web;
using MySql.Data.MySqlClient;
using System.Threading;

namespace controlFallos
{
    public partial class LectorHuellas : Form
    {
        validaciones val = new validaciones();
        conexion c = new conexion();
        Thread th;
        ReportePersonal rper = new ReportePersonal(1, 1, 1);
        private FingerprintCore fngPrint;
        private GriauleFingerprintLibrary.DataTypes.FingerprintRawImage rawImage;
        GriauleFingerprintLibrary.DataTypes.FingerprintTemplate _template;

        int reporte, numhuella, validMsbx = 0;
        string lbl1, lbl2, lbl3, idcred;
        public int id;
        public string credencial, paterno, materno, nombres, puesto;
        public string tiporeporte1, tiporeporte2, tiporeporte3, tipo, tipoidreporte;
        bool inci = false;
        public LectorHuellas(int reporte, int numhuella, string tipo, string lbl1, string lbl2, string lbl3, string idcred)
        {
            th = new Thread(new ThreadStart(val.Splash));
            th.Start();
            InitializeComponent();
            fngPrint = new FingerprintCore();
            fngPrint.onStatus += new StatusEventHandler(fngPrint_onStatus);
            fngPrint.onImage += new ImageEventHandler(fngPrint_onImage);
            this.reporte = reporte;
            this.numhuella = numhuella;
            this.tipo = tipo;
            this.lbl1 = lbl1;
            this.lbl2 = lbl2;
            this.lbl3 = lbl3;
            this.idcred = idcred;
        }

        private void LectorHuellas_Load(object sender, EventArgs e)
        {
            fngPrint.Initialize();
            fngPrint.CaptureInitialize();
            String titulo1;
            //Incidencia_de_Personal i = (Incidencia_de_Personal)Owner;
            if (reporte == 1)
            {
                titulo1 = "HOJA DE PERCANCES - ";
                if (numhuella == 1)
                    gbxReporte.Text = titulo1 + tipo;
                else
                    gbxReporte.Text = titulo1 + tipo;
            }
            else if (reporte == 2)
            {
                titulo1 = "REPORTE DE PERSONAL - ";
                if (numhuella == 1)
                    gbxReporte.Text = titulo1 + tipo;
                else
                    gbxReporte.Text = titulo1 + tipo;
            }
            else
            {
                Incidencia_de_Personal i = (Incidencia_de_Personal)Owner;
                inci = true;
                titulo1 = "INCIDENCIA DE PERSONAL";
                gbxReporte.Text = titulo1 + tipo;
            }
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(SplashScreen))
                {
                    if (frm.InvokeRequired)
                    {

                        validaciones.delgado dm = new validaciones.delgado(val.cerrarForm);

                        Invoke(dm, frm);
                    }

                    break;
                }
            }
            th.Abort();
        }

        void fngPrint_onStatus(object source, GriauleFingerprintLibrary.Events.StatusEventArgs ex)
        {
            if (ex.StatusEventType == GriauleFingerprintLibrary.Events.StatusEventType.SENSOR_PLUG)
                fngPrint.StartCapture(source.ToString());
            else
                fngPrint.StopCapture(source);
        }

        void fngPrint_onImage(object source, GriauleFingerprintLibrary.Events.ImageEventArgs ax)
        {
            rawImage = ax.RawImage;
            SetImage(ax.RawImage.Image);
            extractTemplate();
            ThreadStart identificarhuella = new ThreadStart(identificar);
            hiloEx = new Thread(identificarhuella);
            hiloEx.Start();
        }

        private delegate void delSetImage(Image img);
        void SetImage(Image img)
        {
            if (this.InvokeRequired)
                this.Invoke(new delSetImage(SetImage), new object[] { img });
            else
            {
                Bitmap bmp = new Bitmap(img, ptbxImagen1.Width, ptbxImagen1.Height);
                ptbxImagen1.Image = bmp;
            }
        }

        delegate void delSetQuality(int quality);
        private void SetQualityBar(int quality)
        {
            if (pgbrHuella.InvokeRequired)
                this.Invoke(new delSetQuality(SetQualityBar), new object[] { quality });
            else
            {
                switch (quality)
                {
                    case 0:
                        pgbrHuella.Value = pgbrHuella.Maximum / 3;
                        break;

                    case 1:
                        pgbrHuella.Value = (pgbrHuella.Maximum / 3) * 2;
                        break;

                    case 2:
                        pgbrHuella.Value = pgbrHuella.Maximum;
                        break;

                    case 3:
                        pgbrHuella.Value = 0;
                        break;
                }
            }
        }

        private void extractTemplate()
        {
            if (rawImage != null)
            {
                try
                {
                    _template = null;
                    fngPrint.Extract(rawImage, ref _template);
                    SetQualityBar(_template.Quality);
                    DisplayImage(_template, false);
                }
                catch
                {
                    SetQualityBar(-1);
                }
            }
        }

        private void DisplayImage(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate template, bool identify)
        {
            IntPtr hdc = FingerprintCore.GetDC();
            IntPtr image = new IntPtr();

            if (identify)
                fngPrint.GetBiometricDisplay(template, rawImage, hdc, ref image, FingerprintConstants.GR_DEFAULT_CONTEXT);
            else
                fngPrint.GetBiometricDisplay(template, rawImage, hdc, ref image, FingerprintConstants.GR_NO_CONTEXT);
            SetImage(Bitmap.FromHbitmap(image));
            FingerprintCore.ReleaseDC(hdc);
        }

        private void validacionexportarinfo()
        {
            if (reporte == 1)
            {
                tipoidreporte = "hperc." + "h" + tipo;
                tipoidreporte = id.ToString();
                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1)
                    {
                        tiporeporte1 = "hperc.lbl" + lbl1 + tipo + ".Text";
                        tiporeporte1 = nombres;
                    }
                    else if (i == 2)
                    {
                        tiporeporte2 = "hperc.lbl" + lbl2 + tipo + ".Text";
                        tiporeporte2 = paterno;
                    }
                    else
                    {
                        tiporeporte3 = "hperc.lbl" + lbl3 + tipo + ".Text";
                        tiporeporte3 = materno;
                    }
                }
            }
            else if (reporte == 2)
            {
                tipoidreporte = "rper." + "h" + val.mayusculas(tipo);
                tipoidreporte = id.ToString();
                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1)
                    {
                        tiporeporte1 = "rper.lbl" + lbl1 + tipo + ".Text";
                        tiporeporte1 = nombres;
                    }
                    else if (i == 2)
                    {
                        tiporeporte2 = "rper.lbl" + lbl2 + tipo + ".Text";
                        tiporeporte2 = paterno;
                    }
                    else
                    {
                        tiporeporte3 = "rper.lbl" + lbl3 + tipo + ".Text";
                        tiporeporte3 = materno;
                    }
                }
            }
            else
            {
                Incidencia_de_Personal i = (Incidencia_de_Personal)Owner;
                if (i.incidencia_personal)
                {
                    if (i.supervisor)
                    {
                        if (id > 0)
                        {
                            i.ids = id;
                            if (!duplicados()) i.lblSupervisor.Text = val.mayusculas(paterno.ToLower()) + " " + val.mayusculas(materno.ToLower()) + " " + val.mayusculas(nombres.ToLower());
                        }
                        else
                        {
                            i.ids = 0;
                            i.lblSupervisor.Text = "";
                        }
                    }

                    if (i.conductor)
                    {
                        if (id > 0)
                        {
                            i.idc = id;
                            if (!duplicados()) i.lblConductor.Text = val.mayusculas(paterno.ToLower()) + " " + val.mayusculas(materno.ToLower()) + " " + val.mayusculas(nombres.ToLower());
                        }
                        else
                        {
                            i.idc = 0;
                            i.lblConductor.Text = "";
                        }
                    }

                    if (i.jefe_grupo)
                    {
                        if (id > 0)
                        {
                            i.idj = id;
                            if (!duplicados()) i.lblJefe.Text = val.mayusculas(paterno.ToLower()) + " " + val.mayusculas(materno.ToLower()) + " " + val.mayusculas(nombres.ToLower());
                        }
                        else
                        {
                            i.idj = 0;
                            i.lblJefe.Text = "";
                        }
                    }

                    if (i.c_operativo)
                    {

                        if (id > 0)
                        {
                            i.ido = id;
                            if (!duplicados()) i.lblOperativo.Text = val.mayusculas(paterno.ToLower()) + " " + val.mayusculas(materno.ToLower()) + " " + val.mayusculas(nombres.ToLower());
                        }
                        else
                        {
                            i.ido = 0;
                            i.lblOperativo.Text = "";
                        }
                    }

                    if (i.testigo)
                    {
                        if (id > 0)
                        {
                            i.idt = id;
                            if (!duplicados()) i.lblTestigo.Text = val.mayusculas(paterno.ToLower() ?? "") + " " + val.mayusculas(materno.ToLower() ?? "") + " " + val.mayusculas(nombres.ToLower() ?? "");
                        }
                        else
                        {
                            i.idt = 0;
                            i.lblTestigo.Text = "";
                        }
                    }
                    limpiar_variables();
                    falsos();
                }
            }
        }
        bool incorrecto = false;
        bool duplicados()
        {
            Incidencia_de_Personal i = (Incidencia_de_Personal)Owner;
            if (!i._editar)
            {
                if ((i.ids != i.idt) || (i.ids == 0 && i.idt == 0) || (!i.supervisor && !i.testigo))
                {
                    if ((i.idc != i.idt) || (i.idc == 0 && i.idt == 0) || (!i.conductor & !i.testigo))
                    {
                        if ((i.idj != i.idt) || (i.idj == 0 && i.idt == 0) || (!i.jefe_grupo && !i.testigo))
                        {
                            if ((i.ido != i.idt) || (i.ido == 0 && i.idt == 0) || (!i.c_operativo && !i.testigo))
                            {
                                return false;
                            }
                            else
                            {
                                MessageBox.Show("El \"c.operativo\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                incorrecto = true;
                                return true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El \"jefe de grupo\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            incorrecto = true;
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("El \"conductor\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        incorrecto = true;
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("El \"supervisor\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    incorrecto = true;
                    return true;
                }
            }
            else
            {
                if ((i.ids != i.idt && i._idsAnterior == 0 && i._idtAnterior == 0) || (i.ids == 0 && i.idt == 0) || (i._idsAnterior != i.idt && i._idsAnterior > 0) || (i._idtAnterior != i.ids && i._idtAnterior > 0) || (!i.supervisor && !i.testigo))
                {
                    if ((i.idc != i.idt && i._idcAnterior == 0 && i._idtAnterior == 0) || (i.idc == 0 && i.idt == 0) || (i._idcAnterior != i.idt && i._idcAnterior > 0) || (i._idtAnterior != i.idc && i._idtAnterior > 0) || (!i.conductor && !i.testigo))
                    {
                        if ((i.idj != i.idt && i._idjAnterior == 0 && i._idtAnterior == 0) || (i.idj == 0 && i.idt == 0) || (i._idjAnterior != i.idt && i._idjAnterior > 0) || (i._idtAnterior != i.idj && i._idtAnterior > 0) || (!i.jefe_grupo && !i.testigo))
                        {
                            if ((i.ido != i.idt && i._idoAnterior == 0 && i._idtAnterior == 0) || (i.ido == 0 && i.idt == 0) || (i._idoAnterior != i.idt && i._idoAnterior > 0) || (i._idtAnterior != i.ido && i._idtAnterior > 0) || (!i.c_operativo && !i.testigo))
                            {
                                return false;
                            }
                            else
                            {
                                if (i.c_operativo) i.ido = 0;
                                else i.idt = 0;
                                MessageBox.Show("El \"c.operativo\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                incorrecto = true;
                                return true;
                            }
                        }
                        else
                        {
                            if (i.jefe_grupo) i.idj = 0;
                            else i.idt = 0;
                            MessageBox.Show("El \"jefe de grupo\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            incorrecto = true;
                            return true;
                        }
                    }
                    else
                    {
                        if (i.conductor) i.idc = 0;
                        else i.idt = 0;
                        MessageBox.Show("El \"conductor\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        incorrecto = true;
                        return true;
                    }
                }
                else
                {
                    if (i.supervisor) i.ids = 0;
                    else i.idt = 0;
                    MessageBox.Show("El \"supervisor\" y \"testigo\" no pueden ser la misma persona", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    incorrecto = true;
                    return true;
                }
            }
        }

        void falsos()
        {
            Incidencia_de_Personal i = (Incidencia_de_Personal)Owner;
            i.supervisor = i.conductor = i.jefe_grupo = i.c_operativo = i.testigo = i.incidencia_personal = false;
        }

        public void limpiar_variables()
        {
            id = 0;
            credencial = paterno = materno = nombres = puesto = "";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
        }

        private void gbxReporte_Paint(object sender, PaintEventArgs e)
        {
            val.DrawGroupBox(gbxReporte, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        Thread hiloEx;

        delegate void huella();

        public void baseardatos()
        {
            lblCredencial.Text = credencial;
            lblAPaterno.Text = paterno;
            lblAMaterno.Text = materno;
            lblNombres.Text = nombres;
            lblPuesto.Text = puesto;
        }

        delegate void clean();

        public void limpiar()
        {
            lblCredencial.Text = lblAPaterno.Text = lblAMaterno.Text = lblNombres.Text = lblPuesto.Text = tipoidreporte = tiporeporte1 = tiporeporte2 = tiporeporte3 = "";
            ptbxImagen1.Image = null;
            pgbrHuella.Value = 0;
            id = 0;
        }

        private void identificar()
        {
            int validacion = 0;
            GriauleFingerprintLibrary.DataTypes.FingerprintTemplate tesTemplate = null;
            try
            {
                if ((_template != null) && (_template.Size > 0))
                {
                    fngPrint.IdentifyPrepare(_template);
                    string consulta = "SELECT t2.idpersona, t2.credencial, UPPER(t2.ApPaterno) AS 'PATERNO', UPPER(t2.ApMaterno) AS 'MATERNO', UPPER(t2.nombres) AS 'NOMBRES', UPPER(t3.puesto) AS 'PUESTO', t1.template, t1.calidad FROM huellasupervision AS t1 INNER JOIN cpersonal AS t2 ON t2.idPersona = t1.Personafkcpersonal INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto";
                    string where = "";
                    if (reporte == 1 && numhuella == 1)
                        where = " WHERE UPPER(t3.puesto) LIKE '%CONDUCTOR%'";
                    else if (reporte == 1 && numhuella == 2)
                        where = " WHERE (UPPER(t3.puesto) LIKE '%SUPERVISOR%') OR (UPPER(t3.puesto) LIKE '%COORDINADOR%')";
                    else if (reporte == 2 && numhuella == 1)
                        where = "";
                    else if (reporte == 2 && numhuella == 2)
                        where = " WHERE UPPER(t3.puesto) LIKE '%COORDINADOR%'";
                    if (!string.IsNullOrWhiteSpace(where))
                        where += " AND t2.status = 1";
                    else
                        where += " WHERE t2.status = 1";
                    if (inci)
                    {
                        Incidencia_de_Personal i = (Incidencia_de_Personal)Owner;
                        if (i.supervisor) where = " where upper(t3.puesto)='SUPERVISOR'";
                        if (i.conductor) where = " where t3.puesto like '%Conductor%'";
                        if (i.jefe_grupo) where = " where upper(t3.puesto)='JEFE DE GRUPO' or upper(t3.puesto)='LIDER DE GRUPO'";
                    }
                    MySqlCommand traer = new MySqlCommand(consulta + where, c.dbconection());
                    MySqlDataReader dr = traer.ExecuteReader();
                    while (dr.Read())
                    {
                        if (validacion != 1)
                        {
                            byte[] buff = Convert.FromBase64String(dr.GetString("template"));
                            id = dr.GetInt32("idpersona");
                            credencial = dr.GetString("credencial");
                            paterno = dr.GetString("PATERNO");
                            materno = dr.GetString("MATERNO");
                            nombres = dr.GetString("NOMBRES");
                            puesto = dr.GetString("PUESTO");
                            int calidad = dr.GetInt32("calidad");
                            tesTemplate = new GriauleFingerprintLibrary.DataTypes.FingerprintTemplate();
                            tesTemplate.Size = buff.Length;
                            tesTemplate.Buffer = buff;
                            tesTemplate.Quality = calidad;
                            int score;
                            if (identify(tesTemplate, out score))
                            {
                                validMsbx++;
                                if (id != Convert.ToInt32(idcred) && (reporte == 2 && numhuella == 1))
                                {
                                    validacion++;
                                    if (this.InvokeRequired)
                                    {
                                        clean cl = new clean(limpiar);
                                        this.Invoke(cl);
                                    }
                                }
                                else if (this.InvokeRequired)
                                {
                                    huella huella = new huella(baseardatos);
                                    this.Invoke(huella);
                                    DisplayImage(_template, true);
                                    return;
                                }
                            }
                        }
                    }
                    dr.Read();
                }
                c.dbconection().Close();
                Thread.Sleep(500);
                if (validMsbx == 0)
                {
                    validMsbx = 1;
                    if (MessageBox.Show("Huella no encontrada/registrada", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                        validMsbx = 0;
                    if (this.InvokeRequired)
                    {
                        clean cl = new clean(limpiar);
                        this.Invoke(cl);
                    }
                }
                else if (validMsbx == 1 && validacion == 1)
                {
                    if (MessageBox.Show("La huella ingresada es incorrecta, la huella debe de ser del responsable", "ADVERTENCIA", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                        validMsbx = 0;
                    if (this.InvokeRequired)
                    {
                        clean cl = new clean(limpiar);
                        this.Invoke(cl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            hiloEx.Abort();
        }

        private bool identify(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate testTemplate, out int score)
        {
            return fngPrint.Identify(testTemplate, out score) == 1 ? true : false;
        }

        int cerrar = 0;
        private void LectorHuellas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cerrar == 0)
            {
                cerrar++;
                validacionexportarinfo();
                fngPrint.Finalizer();
                fngPrint.CaptureFinalize();
                this.Close();
            }
        }
    }
}