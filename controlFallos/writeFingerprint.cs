using GriauleFingerprintLibrary;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace controlFallos
{
    public partial class writeFingerprint : Form
    {
        private FingerprintCore fngPrint;
        private GriauleFingerprintLibrary.DataTypes.FingerprintRawImage rawImage;
        public GriauleFingerprintLibrary.DataTypes.FingerprintTemplate _template;
         validaciones v;
        public writeFingerprint(validaciones v)
        {
            this.v = v;
            InitializeComponent();
            fngPrint = new FingerprintCore();
            fngPrint.onStatus += new StatusEventHandler(fngPrint_onStatus);
            fngPrint.onImage += new ImageEventHandler(fngPrint_onImage);
            lbltitle.Left = (this.Width - lbltitle.Width) / 2;
            fngPrint.Initialize();
            fngPrint.CaptureInitialize();
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
            identificar();
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
            if (pgbrHuella.InvokeRequired) this.Invoke(new delSetQuality(SetQualityBar), new object[] { quality });
            else
            {
                switch (quality)
                {
                    case 0:
                        pgbrHuella.Value = pgbrHuella.Maximum / 3;
                        MessageBox.Show("la huella capturada es de mala calidad \n vuelca a intentar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (this.InvokeRequired) { delega d = new delega(limpiar); this.Invoke(d); }
                        break;
                    case 1: pgbrHuella.Value = (pgbrHuella.Maximum / 3) * 2; break;
                    case 2: pgbrHuella.Value = pgbrHuella.Maximum; break;
                    case 3:
                        pgbrHuella.Value = 0;
                        MessageBox.Show("No se pudo capturar la huella. \n Intente de nuevo", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                catch { SetQualityBar(-1); }
            }
        }
        private void DisplayImage(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate template, bool identify)
        {
            IntPtr hdc = FingerprintCore.GetDC();
            IntPtr image = new IntPtr();
            if (identify) fngPrint.GetBiometricDisplay(template, rawImage, hdc, ref image, FingerprintConstants.GR_DEFAULT_CONTEXT);
            else fngPrint.GetBiometricDisplay(template, rawImage, hdc, ref image, FingerprintConstants.GR_NO_CONTEXT);
            SetImage(Bitmap.FromHbitmap(image));
            FingerprintCore.ReleaseDC(hdc);
        }
        void limpiar() { pgbrHuella.Value = 0; ptbxImagen1.Image = null; }
        delegate void delega();
        private void identificar()
        {
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
                            DisplayImage(_template, true);
                            MessageBox.Show("La huella ya se encuentra registrada en el sistema", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            if (this.InvokeRequired)
                            {
                                delega d = new delega(limpiar);
                                this.Invoke(d);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private bool identify(GriauleFingerprintLibrary.DataTypes.FingerprintTemplate testTemplate, out int score) { return fngPrint.Identify(testTemplate, out score) == 1 ? true : false; }
        private void writeFingerprint_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = (_template != null ? DialogResult.OK : DialogResult.Cancel);
            try { fngPrint.Finalizer(); fngPrint.CaptureFinalize(); } catch { }
        }
        private void btnCerrar_Click_1(object sender, EventArgs e) { this.Close(); }
    }
}