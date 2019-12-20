using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{

    public partial class catRefacciones : Form
    {
        Form form;
        Size sizqPanel = new Size(1450, 595);
        Point locationPanel = new Point(125, 117);
        Point LocationActualTitulo = new Point(377, 12);
        Point LocationNoActualTitulo = new Point(941, 12);
        Point LocationActualAtributos = new Point(143, 13);
        Point LocationNoActualAtributos = new Point(650, 13);
        validaciones v;
        int idUsuario;
        public string idRefaccionAbast, codref, modref, nomref, marca, charola, observaciones;
        int empresa, area;
        decimal cantidadingresadaAalmacen, media, abastecimiento;
        DateTime proximpAbastecimiento;
        Font actual = new Font("Garamond", 14, FontStyle.Bold);
        Font noActual = new Font("Garamond", 14, FontStyle.Regular);
        public catRefacciones(Image logo, int idUsuario, int empresa, int area, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            pblogo.BackgroundImage = logo;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
        }
        public catRefacciones(Image logo, int idUsuario, string idefaccion, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            pblogo.BackgroundImage = logo;
            this.idUsuario = idUsuario;
            this.idRefaccionAbast = idefaccion;

        }
        public void actualizarTabla(string folio)
        {
            if (form.Name == "nuevaRefaccion")
            {
                nuevaRefaccion n = (nuevaRefaccion)form;
                n.insertarRefacciones();
                n.idRefaccionMediaAbast = folio;
                n.BuscarRefaccion();
            }
            else
            {
                pContenedor.Controls.Clear();
                pContenedor.Dock = DockStyle.Fill;
                gbsubmenu.Visible = false;
                var form1 = Application.OpenForms.OfType<nuevaRefaccion>().FirstOrDefault();
                nuevaRefaccion hijo = form1 ?? new nuevaRefaccion(idUsuario, empresa, area,v);
                AddFormInPanel(hijo);
                btnadd.Enabled = false;
                btnatrib.Enabled = true;
                hijo.insertarRefacciones();
                hijo.idRefaccionMediaAbast = folio;
                hijo.BuscarRefaccion();
            }
        }

        private void lblatributos_FontChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(idRefaccionAbast))
            {
                if (cerrar())
                {
                    pContenedor.Dock = DockStyle.Fill;
                    gbsubmenu.Visible = false;
                    var form = Application.OpenForms.OfType<nuevaRefaccion>().FirstOrDefault();
                    nuevaRefaccion hijo = form ?? new nuevaRefaccion(idUsuario, idRefaccionAbast);
                    AddFormInPanel(hijo);
                    btnadd.Enabled = false;
                    btnatrib.Enabled = true;
                    idRefaccionAbast = null;
                }
            }
            else
            {
                if (cerrar())
                {
                    pContenedor.Dock = DockStyle.Fill;
                    gbsubmenu.Visible = false;
                    var form = Application.OpenForms.OfType<nuevaRefaccion>().FirstOrDefault();
                    nuevaRefaccion hijo = form ?? new nuevaRefaccion(idUsuario, empresa, area,v);
                    AddFormInPanel(hijo);
                    if (!string.IsNullOrWhiteSpace(codref)) hijo.txtcodrefaccion.Text = codref;
                    if (!string.IsNullOrWhiteSpace(nomref)) hijo.txtnombrereFaccion.Text = nomref;
                    if (!string.IsNullOrWhiteSpace(modref)) hijo.txtmodeloRefaccion.Text = modref;
                    if (proximpAbastecimiento.Date > DateTime.Now) hijo.proxabastecimiento.Value = proximpAbastecimiento;

                    if (!string.IsNullOrWhiteSpace(marca)) hijo.cbmarcas.SelectedValue = marca;
                    if (!string.IsNullOrWhiteSpace(charola))
                    {
                        hijo.cbpasillo.SelectedValue = v.getaData("SELECT (SELECT (SELECT (SELECT idpasillo FROM cpasillos WHERE idpasillo=pasillofkcpasillos) FROM cniveles WHERE idnivel= nivelfkcniveles) FROM canaqueles WHERE idanaquel=anaquelfkcanaqueles) FROM ccharolas WHERE idcharola='" + charola + "'");
                        hijo.cbnivel.SelectedValue = v.getaData("SELECT (SELECT (SELECT idnivel FROM cniveles WHERE idnivel= nivelfkcniveles) FROM canaqueles WHERE idanaquel=anaquelfkcanaqueles) FROM ccharolas WHERE idcharola='" + charola + "'");
                        hijo.cbanaquel.SelectedValue = v.getaData("SELECT (SELECT idanaquel FROM canaqueles WHERE idanaquel=anaquelfkcanaqueles) FROM ccharolas WHERE idcharola='" + charola + "'"); hijo.cbcharola.SelectedValue = charola;

                    }
                    if (cantidadingresadaAalmacen > 0) hijo.cantidada.Text = cantidadingresadaAalmacen.ToString();
                    if (media > 0) hijo.notifmedia.Text = media.ToString();
                    if (abastecimiento > 0) hijo.notifabastecimiento.Text = abastecimiento.ToString();
                    if (!string.IsNullOrWhiteSpace(observaciones)) hijo.txtdesc.Text = observaciones;
                    if (!string.IsNullOrWhiteSpace(marca))
                    {
                        hijo.cbfamilia.SelectedValue = v.getaData("SELECT t3.idcnfamilia FROM cmarcas as t1 INNER JOIN cfamilias as t2 ON t1.descripcionfkcfamilias=t2.idfamilia INNER JOIN cnfamilias as t3 On t2.familiafkcnfamilias=t3.idcnFamilia WHERE t1.idmarca='" + marca + "'");
                        hijo.cbdescfamilia.SelectedValue = v.getaData("SELECT t2.idfamilia FROM cmarcas as t1 INNER JOIN cfamilias as t2 ON t1.descripcionfkcfamilias=t2.idfamilia WHERE t1.idmarca='" + marca+ "'");
                        hijo.cbmarcas.SelectedValue = marca;
                    }
                    codref = null;
                    nomref = null;
                    modref = null;

                    marca = null;

                    charola = null;
                    cantidadingresadaAalmacen = 0;
                    media = 0;
                    abastecimiento = 0;
                    observaciones = null;
                    lblmuestracod.Text = null;
                    lblmuestramod.Text = null;
                    lbmuestranom.Text = null;
                    panel1.Visible = false;
                    btnadd.Enabled = false;
                    btnatrib.Enabled = true;

                }
            }
            lblrefacciones.Font = actual;
            lblatributos.Font = lblmarcas.Font = lblubicaciones.Font = noActual;
            prefacciones.Location = LocationActualTitulo;
            patributos.Location = LocationNoActualTitulo;
        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (this.pContenedor.Controls.Count != 0)
            {

                try
                {
                    if (form.Name == "nuevaRefaccion")
                    {
                        nuevaRefaccion nuevaRef = (nuevaRefaccion)form;
                        if (!nuevaRef.editar)
                        {
                            if (!string.IsNullOrWhiteSpace(nuevaRef.txtcodrefaccion.Text)) lblmuestracod.Text = codref = nuevaRef.txtcodrefaccion.Text.Trim();
                            if (!string.IsNullOrWhiteSpace(nuevaRef.txtnombrereFaccion.Text)) lblmuestramod.Text = nomref = nuevaRef.txtnombrereFaccion.Text.Trim();
                            if (!string.IsNullOrWhiteSpace(nuevaRef.txtmodeloRefaccion.Text)) lbmuestranom.Text = modref = nuevaRef.txtmodeloRefaccion.Text.Trim();
                            if (nuevaRef.proxabastecimiento.Value > DateTime.Now) proximpAbastecimiento = nuevaRef.proxabastecimiento.Value;
                            if (nuevaRef.cbmarcas.SelectedIndex > 0) marca = nuevaRef.cbmarcas.SelectedValue.ToString();
                            if (nuevaRef.cbcharola.DataSource != null) if (nuevaRef.cbcharola.SelectedIndex > 0) charola = nuevaRef.cbcharola.SelectedValue.ToString();
                            cantidadingresadaAalmacen = 0; if (!string.IsNullOrWhiteSpace(nuevaRef.cantidada.Text.Trim())) cantidadingresadaAalmacen = Convert.ToDecimal(nuevaRef.cantidada.Text.Trim());
                            if (!string.IsNullOrWhiteSpace(nuevaRef.notifmedia.Text)) { if (Convert.ToDecimal(nuevaRef.notifmedia.Text) > 0) media = Convert.ToDecimal(nuevaRef.notifmedia.Text); } else media = 0;
                            if (!string.IsNullOrWhiteSpace(nuevaRef.notifabastecimiento.Text)) { if (Convert.ToDecimal(nuevaRef.notifabastecimiento.Text) > 0) abastecimiento = Convert.ToDecimal(nuevaRef.notifabastecimiento.Text); } else abastecimiento = 0;
                            if (!string.IsNullOrWhiteSpace(nuevaRef.txtdesc.Text.Trim())) observaciones = nuevaRef.txtdesc.Text.Trim();
                            if (!string.IsNullOrWhiteSpace(codref) || !string.IsNullOrWhiteSpace(nomref) || !string.IsNullOrWhiteSpace(modref)) panel1.Visible = true; else panel1.Visible = false;

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                lblatributos.Font = actual;
                lblrefacciones.Font = noActual;
                this.pContenedor.Controls.RemoveAt(0);
                this.form.Close();
                gbsubmenu.Visible = true;
                pContenedor.Dock = DockStyle.None;
                pContenedor.Size = sizqPanel;
                pContenedor.Location = locationPanel;
                btnatrib.Enabled = false;
                btnadd.Enabled = true;
                button5_Click(null, e);

            }
            prefacciones.Location = LocationNoActualTitulo;
            patributos.Location = LocationActualTitulo;


        }
        public void AddFormInPanel(Form fh)
        {
            this.form = fh;
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.pContenedor.Controls.Add(fh);
            this.pContenedor.Tag = fh;
            fh.Show();
        }
        public bool cerrar()
        {
            if (this.pContenedor.Controls.Count != 0)
            {
                this.pContenedor.Controls.RemoveAt(0);
                form.Close();
                return true;
            }
            else
            {
                return true;
            }
        }

        private void catRefacciones_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);

        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (cerrar())
            {
                var form = Application.OpenForms.OfType<ubicaciones>().FirstOrDefault();
                ubicaciones hijo = form ?? new ubicaciones(idUsuario, empresa, area, this,v);
                AddFormInPanel(hijo);
                btnubic.Enabled = false;
                btnmarca.Enabled = true;
                lblubicaciones.Font = actual;
                lblmarcas.Font = noActual;
                pUbicaciones.Location = LocationActualAtributos;
                pCaracteristicas.Location = LocationNoActualAtributos;
            }
        }


        private void gbsubmenu_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (cerrar())
            {
                var form = Application.OpenForms.OfType<ums>().FirstOrDefault();
                ums hijo = form ?? new ums(this.idUsuario, empresa, area,v);
                AddFormInPanel(hijo);
                lblubicaciones.Font = lblmarcas.Font = noActual;
                btnubic.Enabled = true;
                btnmarca.Enabled = true;
            }
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cerrar())
            {
                var form = Application.OpenForms.OfType<marcas>().FirstOrDefault();
                marcas hijo = form ?? new marcas(this.idUsuario, empresa, area, this,v);

                AddFormInPanel(hijo);
                lblmarcas.Font = actual;
                lblubicaciones.Font = noActual;
                btnubic.Enabled = true;
                btnmarca.Enabled = false;
                pUbicaciones.Location = LocationNoActualAtributos;
                pCaracteristicas.Location = LocationActualAtributos;
            }
        }
    }
}
