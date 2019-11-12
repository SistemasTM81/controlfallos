using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GriauleFingerprintLibrary;
using System.Threading;
namespace controlFallos
{
    public partial class readFingerprint : Form
    {
        validaciones v ;
        private FingerprintCore fngPrint;
        private GriauleFingerprintLibrary.DataTypes.FingerprintRawImage rawImage;
        GriauleFingerprintLibrary.DataTypes.FingerprintTemplate _template;
        string where,cargo;
        public Thread hilo;
        public object idPersona;
        int diferenciales;
        public readFingerprint(string where, int diferenciales,string cargo,validaciones v)
        {
            InitializeComponent();
            this.v = v;
            fngPrint = new FingerprintCore();
            fngPrint.onStatus += new StatusEventHandler(fngPrint_onStatus);
            fngPrint.onImage += new ImageEventHandler(fngPrint_onImage);
            this.where = where;
            fngPrint.Initialize();
            fngPrint.CaptureInitialize();
            this.diferenciales = diferenciales;
            this.cargo = cargo;
        }
        void fngPrint_onStatus(object source, GriauleFingerprintLibrary.Events.StatusEventArgs ex)
        {
            if (ex.StatusEventType == GriauleFingerprintLibrary.Events.StatusEventType.SENSOR_PLUG)
                fngPrint.StartCapture(source.ToString());
            else
                fngPrint.StopCapture(source);
        }
        delegate void delSetQuality(string text);
        void escribir(string args)
        {
            label1.Text = args;
        }
        void fngPrint_onImage(object source, GriauleFingerprintLibrary.Events.ImageEventArgs ax)
        {
            if (label1.InvokeRequired) this.Invoke(new delSetQuality(escribir), new object[] { "Identificando . . ."});
            rawImage = ax.RawImage;
            extractTemplate();
            ThreadStart identificarhuella = new ThreadStart(identificar);
            hiloEx = new Thread(identificarhuella);
            hiloEx.Start();
        }
        Thread hiloEx;
        delegate void huella();
        private void extractTemplate() { if (rawImage != null) { try { _template = null; fngPrint.Extract(rawImage, ref _template); } catch { } } }
        bool MessageBoxActiveNoEncontrada, MessageBoxActiveEsDiferente;
        private void identificar()
        {
            bool res=false;
            try
            {
                if ((_template != null) && (_template.Size > 0))
                {
                    fngPrint.IdentifyPrepare(_template);
                    DataTable dt = (DataTable)v.getData("Set names 'utf8';SELECT t2.idpersona,CONVERT(t1.template using utf8) as template, t1.calidad FROM huellasupervision AS t1 INNER JOIN cpersonal AS t2 ON t2.idPersona = t1.Personafkcpersonal INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto ");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        byte[] buff = Convert.FromBase64String(dt.Rows[i].ItemArray[1].ToString());
                        int calidad = Convert.ToInt32(dt.Rows[i].ItemArray[2]);
                        GriauleFingerprintLibrary.DataTypes.FingerprintTemplate tesTemplate = new GriauleFingerprintLibrary.DataTypes.FingerprintTemplate();
                        tesTemplate.Size = buff.Length;
                        tesTemplate.Buffer = buff;
                        tesTemplate.Quality = calidad;
                        int score;
                        if (identify(tesTemplate, out score))
                        {
                            idPersona = dt.Rows[i].ItemArray[0];
                            if (!identifyDiferent(Convert.ToInt32(idPersona)))
                            {
                               
                                this.DialogResult = DialogResult.OK;
                                return;
                            }
                            else
                            {
                                idPersona = 0;
                                if (!MessageBoxActiveEsDiferente && !MessageBoxActiveNoEncontrada)
                                {
                                    MessageBoxActiveEsDiferente = true;
                                    MessageBox.Show("La Huella Dactilar No Pertenece Al "+cargo, validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    res = true;
                                    MessageBoxActiveEsDiferente = false;
                                    if (label1.InvokeRequired) this.Invoke(new delSetQuality(escribir), new object[] { "Coloca La Huella Registrada En \nEl Lector Para Continuar" });
                                    DialogResult = DialogResult.None;
                                }
                            }
                        }
                    }
                     if (!MessageBoxActiveNoEncontrada && !MessageBoxActiveEsDiferente && Convert.ToInt32(idPersona) == 0 && !res)
                    {
                        MessageBoxActiveNoEncontrada = true;
                        MessageBox.Show("Huella no encontrada/registrada", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        MessageBoxActiveNoEncontrada = false;
                        if (label1.InvokeRequired) this.Invoke(new delSetQuality(escribir), new object[] { "Coloca La Huella Registrada En \nEl Lector Para Continuar" }); 
                        DialogResult = DialogResult.None;
                    }
                    res = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        bool identifyDiferent(int id) { return diferenciales != id; }
        private bool identify(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate testTemplate, out int score) { return fngPrint.Identify(testTemplate, out score) == 1; }
        private void btnCerrar_Click(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; }
        private void readFingerprint_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                fngPrint.Finalizer();
                fngPrint.CaptureFinalize();
            }
            catch { }
        }

    }
}