using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace controlFallos
{
    public partial class CatRoles : Form
    {
        validaciones v;
        Button boton;
        int idUsuario, idRol, empresaAnterior, areaArenterior, servicioAnterior, ciclosAnterior, ecosAnterior, lapsoAnterior, statusAnterior, x = 5, y = 5, contador = 0, c = 0, diferenciaA;
        string img, imganterior;
        List<int> unidades;
        List<int> unidadesAnterior;
        List<string> diferenciaAnterior;
        List<string> diferencia;
        DateTime horaAnterior;
        delegate void empre();
        delegate void d1();
        delegate void d2();
        Thread hempresas, th, thunidades;
        DataTable dt;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }
        public bool editar = false, nuevo, editardif = false, editareco = false;


        public CatRoles(int idUsuario, int empresa, int area, validaciones v)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            this.v = v;
            this.idUsuario = idUsuario;
            InitializeComponent();
            cmbempresa.DrawItem += v.combos_DrawItem;
            cmbarea.DrawItem += v.combos_DrawItem;
            cmbservicio.DrawItem += v.combos_DrawItem;
            cmbempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbarea.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbservicio.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        bool getboolfromint(int i)
        {
            return i == 1;
        }
        public void privilegios()
        {
            string sql = "SELECT privilegios as privilegios FROM privilegios where usuariofkcpersonal='" + idUsuario + "' and namform='CatRoles'";
            string[] privilegios = v.getaData(sql).ToString().Split('/');
            pinsertar = getboolfromint(Convert.ToInt32(privilegios[0]));
            pconsultar = getboolfromint(Convert.ToInt32(privilegios[1]));
            peditar = getboolfromint(Convert.ToInt32(privilegios[2]));
            pdesactivar = getboolfromint(Convert.ToInt32(privilegios[3]));
        }
        void mostrar()
        {
            privilegios();
            ptabla.Visible = (pconsultar ? true : false);
            gbRol.Visible = gbecos.Visible = gbxdiferencia.Visible = (pinsertar || peditar ? true : false);
            psave.Visible = (pinsertar ? true : false);
        }
        void mostrarEmpresas()
        {
            if (this.InvokeRequired)
            {
                empre e = new empre(mostrarEmpresas);
                this.Invoke(e);
            }
            v.iniCombos("call sistrefaccmant.companieswithstatus();", cmbempresa, "id", "nombre", "-- SELECCIONE UNA EMPRESA --");
            hempresas.Abort();
        }
        void loadgif()
        {
            pictureBox2.Image = Properties.Resources.loader;
        }
        void loadecos()
        {
            pgif.Controls.Clear(); x = y = 5;
            dt = (DataTable)v.getData("call sistrefaccmant.ecosbyservice('" + cmbarea.SelectedValue + "');");
            foreach (DataRow item in dt.Rows)
                createcontrols(item.ItemArray[0], item.ItemArray[1]);
        }
        void pecos()
        {
            if (this.InvokeRequired)
            {
                d1 d = new d1(loadecos);
                pgif.Invoke(d);
            }
        }
        void createcontrols(object id, object text)
        {
            Button l = new Button();
            l.FlatStyle = FlatStyle.Flat;
            l.Name = "lbl" + "|" + id;
            l.AutoSize = false;
            l.BackColor = (Convert.ToInt32(v.getaData("call sistrefaccmant.ecoinuse('" + id + "');")) > 0 ? Color.Khaki : Color.PaleGreen);
            l.Enabled = (Convert.ToInt32(v.getaData("call sistrefaccmant.ecoinuse('" + id + "');")) == 0 ? true : Convert.ToInt32(v.getaData("call sistrefaccmant.ecowithrol('" + idRol + "', '" + id + "');")) > 0 ? true : false);
            l.ForeColor = Color.FromArgb(75, 44, 52);
            l.Font = new Font("garamond", 10, FontStyle.Bold);
            l.Size = new Size(80, 24);
            l.Click += btn_click;
            y = (x + 87 >= (pgif.Size.Width - 10) ? y += 29 : y);
            x = (x + 87 >= (pgif.Size.Width - 10) ? 5 : x);
            l.Location = new Point(x, y);
            x += 87;
            l.Text = text.ToString();
            pgif.Controls.Add(l);
        }
        private void btn_click(object sender, EventArgs e)
        {
            boton = ((Button)sender);
            object id = boton.Name.Split('|')[1];
            if (contador == Convert.ToInt32(txtecos.Text) && !existinarray(Convert.ToInt32(id)))
                MessageBox.Show("Todos los economicos ya fueron seleccionados", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                if (existinarray(Convert.ToInt32(id)))
                {
                    contador = unidades.IndexOf(Convert.ToInt32(id));
                    unidades.Remove(Convert.ToInt32(id));
                    editareco = true;
                    boton.BackColor = Color.PaleGreen;
                }
                else
                {
                    unidades.Insert(contador, Convert.ToInt32(id));
                    boton.BackColor = Color.Khaki;
                    contador = unidades.Count;
                }
                lblunidades.Text = "Unidades: " + texto(unidades);
                cmbempresa_SelectedValueChanged(sender, e);
            }
        }
        bool existinarray(int id)
        {
            bool res = false;
            for (int i = 0; i < unidades.Count; i++)
                if (!string.IsNullOrWhiteSpace(unidades[i].ToString()))
                    if (unidades[i] == id)
                        res = true;
            return res;
        }
        string texto(List<int> lista)
        {
            string cadena = "";
            for (int i = 0; i < lista.Count; i++)
                if (!string.IsNullOrWhiteSpace(lista[i].ToString()))
                    cadena = (i == 0 ? cadena += v.getaData("call sistrefaccmant.getecobyid('" + lista[i] + "');") : cadena += (", " + v.getaData("call sistrefaccmant.getecobyid('" + lista[i] + "');")));
            return cadena;
        }
        private void CatRoles_Load(object sender, EventArgs e)
        {
            hempresas = new Thread(new ThreadStart(mostrarEmpresas));
            hempresas.Start();
            cargarroles();
            mostrar();
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
        void cargarroles()
        {
            dgvroles.Rows.Clear();
            string sql = "select t1.idrol as id,(select upper(nombreEmpresa) from cempresas as x1 inner join careas as x2 on x1.idempresa=x2.empresafkcempresas where t2.AreafkCareas=x2.idarea)as empresa,(select upper(x1.nombreArea) from careas as x1 where x1.idarea=t2.AreafkCareas)as area, upper(concat(t2.nombre,' ',t2.descripcion)) as servicio,nciclos as ciclos, necos as ecos, date_format(horaincorporo,'%H:%i') as hora, CONCAT(diffciclos,' MINUTOS') as diferencia,(select upper(concat(coalesce(x1.apPaterno,''),' ',coalesce(x1.apMaterno,''),' ',x1.nombres)) from cpersonal as x1 where x1.idPersona=t1.usuariofkcpersonal)as persona, if(t1.status=1,'ACTIVO','NO ACTIVO') as estatus from croles as t1 inner join cservicios as t2 on t2.idservicio=t1.serviciofkcservicios";
            MySqlCommand cmd = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
                dgvroles.Rows.Add(dr.GetString("id"), dr.GetString("empresa"), dr.GetString("area"), dr.GetString("servicio"), dr.GetString("ciclos"), dr.GetString("ecos"), dr.GetString("hora"), dr.GetString("diferencia"), dr.GetString("persona"), dr.GetString("estatus"));
            dr.Close();
            v.c.dbcon.Close();
            dgvroles.ClearSelection();
        }
        private void cmbempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbempresa.SelectedIndex > 0)
                v.iniCombos("select idarea as id, upper(nombreArea) as nombre from careas as t1 inner join cempresas as t2 on t2.idempresa=t1.empresafkcempresas where t1.status='1' and empresafkcempresas='" + cmbempresa.SelectedValue + "' order by nombreArea;", cmbarea, "id", "nombre", "-- SELECCIONE UN ÁREA --");
            else cmbarea.DataSource = null;
            cmbarea.Enabled = (cmbempresa.SelectedIndex > 0 ? true : false);
        }

        private void cmbarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbarea.SelectedIndex > 0)
                v.iniCombos("select idservicio as id, upper(concat(Nombre,' ',Descripcion)) as nombre from cservicios where AreafkCareas='" + cmbarea.SelectedValue + "' and status='1' order by Nombre;", cmbservicio, "id", "nombre", "-- SELECCIONE UN SERVICIO--");
            else cmbservicio.DataSource = null;
            cmbservicio.Enabled = (cmbarea.SelectedIndex > 0 ? true : false);
        }

        private void btnecos_Click(object sender, EventArgs e)
        {
            if (cmbservicio.SelectedIndex > 0)
            {
                gbecos.Enabled = (!string.IsNullOrWhiteSpace(txtecos.Text) && Convert.ToInt32(txtecos.Text) > 0 ? true : false);
                if (!editar)
                {
                    object aux = null;
                    List<int> respaldo = null;
                    if (unidades != null && unidades.Count > 0)
                    {
                        aux = unidades.Count;
                        if (Convert.ToInt32(aux ?? 0) != Convert.ToInt32(txtecos.Text) && Convert.ToInt32(aux ?? 0) > 0)
                        {
                            respaldo = new List<int>(Convert.ToInt32(aux));
                            for (int i = 0; i < Convert.ToInt32(aux); i++)
                                respaldo.Add(unidades[i]);
                        }
                    }
                    unidades = new List<int>(Convert.ToInt32(txtecos.Text));
                    if (Convert.ToInt32(aux) != Convert.ToInt32(txtecos.Text) && Convert.ToInt32(aux) > 0)
                        for (int i = 0; i < (Convert.ToInt32(aux) < Convert.ToInt32(txtecos.Text) ? Convert.ToInt32(aux) : Convert.ToInt32(txtecos.Text)); i++)
                            unidades.Add(respaldo[i]);
                    if (Convert.ToInt32(txtecos.Text) < Convert.ToInt32(aux))
                        lblunidades.Text = "Unidades: " + texto(unidades);
                }
                else
                {
                    if (Convert.ToInt32(txtecos.Text) != ecosAnterior)
                    {
                        unidades = new List<int>(Convert.ToInt32(txtecos.Text));
                        for (int i = 0; i < (Convert.ToInt32(txtecos.Text) <= ecosAnterior ? Convert.ToInt32(txtecos.Text) : unidadesAnterior.Count); i++)
                            unidades.Add(unidadesAnterior[i]);
                        lblunidades.Text = "Unidades: " + texto(unidades);

                    }

                }
            }
            else
                MessageBox.Show("Debe seleccionar un servicio de la lista desplegable", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        void deletefromlist()
        {
            if (editar)
            {
                if (Convert.ToInt32(txtecos.Text) != ecosAnterior)
                {
                    string restantes = "";
                    unidades = new List<int>();
                    diferencia = new List<string>();
                    for (int i = 0; i < (Convert.ToInt32(txtecos.Text) <= ecosAnterior ? Convert.ToInt32(txtecos.Text) : unidadesAnterior.Count); i++)
                    {
                        unidades.Add(unidadesAnterior[i]);
                        if (i < (Convert.ToInt32(txtecos.Text) <= ecosAnterior ? Convert.ToInt32(txtecos.Text) - 1 : diferenciaAnterior.Count))
                            diferencia.Add(diferenciaAnterior[i]);
                    }
                    lblunidades.Text = "Unidades: " + texto(unidades);
                    differencebetween(diferencia);
                    contador = unidades.Count;
                    if (contador < unidadesAnterior.Count)
                    {
                        for (int i = contador; i < unidadesAnterior.Count; i++)
                            restantes = (unidadesAnterior.Count - i > 1 ? restantes += unidadesAnterior[i].ToString() + "," : restantes += unidadesAnterior[i].ToString());
                        resetcolor(restantes.ToString().Split(','));
                    }
                }
            }
        }
        void resetcolor(string[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                Button b = pgif.Controls.Find("lbl|" + ids[i], true).FirstOrDefault() as Button;
                b.BackColor = Color.PaleGreen;
            }
        }
        private void cmbservicio_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbservicio.SelectedIndex > 0)
            {
                thunidades = new Thread(new ThreadStart(pecos));
                thunidades.Start();
            }
            else
            { pgif.Controls.Clear(); x = y = 5; }
        }

        private void txtdiferencia_TextChanged(object sender, EventArgs e)
        {
            if (editardif)
                padd.Visible = (!string.IsNullOrWhiteSpace(txtdiferencia.Text) && diferenciaA != Convert.ToInt32(txtdiferencia.Text) ? true : false);
        }

        private void txtecos_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtecos.Text))
                deletefromlist();
        }

        private void gbRol_Paint(object sender, PaintEventArgs e)
        {
            GroupBox gbxall = sender as GroupBox;
            v.DrawGroupBox(gbxall, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        Bitmap redimesionar(string img, int p)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(img);
            float nperncent = ((float)p / 100);
            int destinoWidth = (int)(bmp.Width * nperncent);
            int destinoHeight = (int)(bmp.Height * nperncent);
            Bitmap imagen2 = new Bitmap(destinoWidth, destinoHeight);
            using (Graphics g = Graphics.FromImage((Image)imagen2))
            {
                g.DrawImage(bmp, 0, 0, destinoWidth, destinoHeight);
            }
            bmp.Dispose();
            return (imagen2);
        }
        private void btnimg_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Title = "Seleccionar imagen";
            dialogo.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dialogo.RestoreDirectory = true;
            dialogo.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                img = v.ImageToString(dialogo.FileName);
                pictureBox1.BackgroundImage = redimesionar(dialogo.FileName, 100);
                lblimg.Text = "Cambiar imágen";
                cmbempresa_SelectedValueChanged(sender, e);
            }
        }
        private void lbxdiferencias_DoubleClick(object sender, EventArgs e)
        {
            padd.Visible = !(pdatos.Visible = editardif = true);
            btnadd.BackgroundImage = Properties.Resources.pencil;
            txtdiferencia.Text = (diferenciaA = Convert.ToInt32(diferencia[c = lbxdiferencias.SelectedIndex])).ToString();
            lbltexto.Text = "Diferencia entre unidad " + (lbxdiferencias.SelectedIndex + 1) + " y " + (lbxdiferencias.SelectedIndex + 2) + ": ";
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtdiferencia.Text))
            {
                if (Convert.ToInt32(txtdiferencia.Text) > 0)
                {
                    if (editardif)
                    {
                        diferencia.RemoveAt(c);
                        lbxdiferencias.Items.RemoveAt(c);
                    }
                    diferencia.Insert(c, txtdiferencia.Text);
                    differencebetween(diferencia);
                    txtdiferencia.Clear();
                    txtdiferencia.Focus();
                    c++;
                    lbltexto.Text = "Diferencia entre unidad " + (c + 1) + " y " + (c + 2) + ": ";
                    padd.Visible = pdatos.Visible = (c == Convert.ToInt32(txtecos.Text) - 1 ? false : true);
                    if (editardif)
                        pdatos.Visible = padd.Visible = false;
                    cmbempresa_SelectedValueChanged(sender, e);
                    btnadd.BackgroundImage = Properties.Resources.add;
                }
                else
                    MessageBox.Show("La diferencia debe ser mayor a 0", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("El campo de diferencia entre unidades se encuentra vacío", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btntime_Click(object sender, EventArgs e)
        {
            gbxdiferencia.Enabled = true;
            if (!editar)
            {
                diferencia = new List<string>();
                for (int i = 0; i < Convert.ToInt32(txtecos.Text) - 1; i++)
                    diferencia.Add("5");
                differencebetween(diferencia);
            }
            else
            {
                if ((Convert.ToInt32(txtecos.Text) - 1) != diferencia.Count)
                {
                    diferencia = new List<string>();
                    for (int i = 0; i < Convert.ToInt32(txtecos.Text) - 1; i++)
                        diferencia.Add("5");
                    differencebetween(diferencia);
                }
            }
        }
        private void txtciclos_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void cmbempresa_SelectedValueChanged(object sender, EventArgs e)
        {
            if (editar && peditar)
                psave.Visible = (cambios() ? true : false);
            if (!string.IsNullOrWhiteSpace(txtecos.Text))
            {
                ptime.Visible = pselectecos.Visible = ((Convert.ToInt32(txtecos.Text) > 0 && (statusAnterior > 0 || !editar)) ? true : false);
                pselectecos.Visible = pselectecos.Visible = ((Convert.ToInt32(txtecos.Text) > 0 && (statusAnterior > 0 || !editar)) ? true : false);
            }
            else ptime.Visible = pselectecos.Visible = false;

        }
        public string cadena(List<string> lista)
        {
            string cadena = "";
            for (int i = 0; i < lista.Count; i++)
                cadena = (i == 0 ? (cadena += lista[i]) : cadena += ("," + lista[i]));
            return cadena;
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (v.camposRol(Convert.ToInt32(cmbempresa.SelectedValue), Convert.ToInt32(cmbarea.SelectedValue), Convert.ToInt32(cmbservicio.SelectedValue), txtciclos.Text, txtecos.Text, dtpincorporo.Value, Convert.ToInt32(nudlapso.Value), unidades, diferencia))
                if (!v.existeRol(Convert.ToInt32(cmbservicio.SelectedValue), idRol))
                    if (!editar)
                    {
                        if (v.c.insertar("Insert into croles(serviciofkcservicios, nciclos, necos, horaincorporo, diffciclos, timediference, usuariofkcpersonal,image) values('" + cmbservicio.SelectedValue + "', '" + Convert.ToInt32(txtciclos.Text) + "', '" + Convert.ToInt32(txtecos.Text) + "', '" + dtpincorporo.Value.ToString("HH:mm:ss") + "', '" + nudlapso.Value + "','" + cadena(diferencia) + "', '" + idUsuario + "','" + img + "')"))
                            if (insertre())
                                if (v.c.insertar("insert into modificaciones_sistema(form,idregistro,usuariofkcpersonal,fechaHora,Tipo,empresa,area) values('Catálogo de Roles','" + v.getaData("select idrol from croles where serviciofkcservicios='" + cmbservicio.SelectedValue + "';") + "','" + idUsuario + "',now(),'Inserción de Rol','1','1')"))
                                {
                                    MessageBox.Show("Los datos se insertaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    limpiar();
                                    cargarroles();
                                }
                                else MessageBox.Show("Error al registrar los datos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        observacionesEdicion o = new observacionesEdicion(v);
                        o.Owner = this;
                        if (o.ShowDialog() == DialogResult.OK)
                        {
                            string motivo = o.txtgetedicion.Text.Trim();
                            if (v.c.insertar("update croles set serviciofkcservicios='" + cmbservicio.SelectedValue + "',nciclos='" + Convert.ToInt32(txtciclos.Text) + "',necos='" + Convert.ToInt32(txtecos.Text) + "',horaincorporo='" + dtpincorporo.Value.ToString("HH:mm:ss") + "',diffciclos='" + nudlapso.Value + "',timediference='" + cadena(diferencia) + "'" + (!string.IsNullOrWhiteSpace(img) ? ",image='" + img + "'" : "") + " where idrol='" + idRol + "'"))
                                if (insertre())
                                    if (v.c.insertar("insert into modificaciones_sistema(form,idregistro,ultimaModificacion,usuariofkcpersonal,fechaHora,Tipo,motivoActualizacion,empresa,area) values('Catálogo de Roles','" + idRol + "','" + servicioAnterior + ";" + ciclosAnterior + ";" + ecosAnterior + ";" + horaAnterior.ToString("HH:mm") + ";" + lapsoAnterior + ";" + cadena(diferenciaAnterior) + "','" + idUsuario + "',now(),'Actualización de Rol','" + v.mayusculas(motivo) + "','1','1')"))
                                    {
                                        MessageBox.Show("Los datos se actualizaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        limpiar();
                                        cargarroles();
                                    }
                                    else MessageBox.Show("Error al modificar los datos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
        }
        bool insertre()
        {
            bool res = false;
            if (Convert.ToInt32(txtecos.Text) != ecosAnterior)
            {
                v.getaData("delete from rolecosbyservices where rolfkcroles='" + idRol + "'");
                editar = false;
            }
            for (int i = 0; i < Convert.ToInt32(txtecos.Text); i++)
                if (v.c.insertar((editar ? "update rolecosbyservices set unidadfkcunidades='" + unidades[i] + "' where unidadfkcunidades='" + unidadesAnterior[i] + "'" : "call sistrefaccmant.insertecos('" + Convert.ToInt32(v.getaData("select idrol from croles where serviciofkcservicios='" + cmbservicio.SelectedValue + "';")) + "', '" + unidades[i] + "', '" + idUsuario + "');")))
                    res = true;
            return res;
        }

        private void btnstatus_Click(object sender, EventArgs e)
        {
            observacionesEdicion o = new observacionesEdicion(v);
            o.Owner = this;
            o.lblinfo.Text = "Ingrese el motivo de la " + (statusAnterior == 1 ? "desactivación" : "reactivación");
            if (o.ShowDialog() == DialogResult.OK)
            {
                string motivo = o.txtgetedicion.Text.Trim();
                if (v.c.insertar("update croles set status='" + (statusAnterior == 1 ? 0 : 1) + "' where idrol='" + idRol + "'"))
                    if (v.c.insertar("insert into modificaciones_sistema(form,idregistro,usuariofkcpersonal,fechaHora,Tipo,motivoActualizacion,empresa,area) values('Catálogo de Roles','" + idRol + "','" + idUsuario + "',now(),'" + (statusAnterior == 1 ? "Desactivación" : "Reactivación") + " de Rol','" + v.mayusculas(motivo) + "','1','1')"))
                    {
                        MessageBox.Show("El servicio se ha " + (statusAnterior == 1 ? "desactivado" : "reactivado") + " correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        cargarroles();
                    }
            }
        }

        private void dgvroles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvroles.Columns[e.ColumnIndex].Name == "status")
                e.CellStyle.BackColor = (e.Value.ToString() == "ACTIVO" ? Color.PaleGreen : Color.LightCoral);
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (cambios())
                if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { }
                else
                    limpiar();
            else
                limpiar();
        }

        bool cambios()
        {
            bool res = false;
            if ((Convert.ToInt32(cmbempresa.SelectedValue) != empresaAnterior || Convert.ToInt32(cmbarea.SelectedValue) != areaArenterior || img != imganterior || Convert.ToInt32(cmbservicio.SelectedValue) != servicioAnterior || Convert.ToInt32((string.IsNullOrWhiteSpace(txtciclos.Text) ? "0" : txtciclos.Text)) != ciclosAnterior || Convert.ToInt32((string.IsNullOrWhiteSpace(txtecos.Text) ? "0" : txtecos.Text)) != ecosAnterior || dtpincorporo.Value.ToString("HH:mm") != horaAnterior.ToString("HH:mm") || nudlapso.Value != lapsoAnterior || (unidadesAnterior != null && unidadesAnterior != null && diferenciaAnterior != null && diferencia != null && changesinlist())))
            {
                if (empresaAnterior == 0 || areaArenterior == 0 || servicioAnterior == 0 || ciclosAnterior == 0 || ecosAnterior == 0 || horaAnterior.Hour == 0 || lapsoAnterior == 0)
                    nuevo = true;
                if (cmbempresa.SelectedIndex > 0 && cmbarea.SelectedIndex > 0 && cmbservicio.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtciclos.Text) && !string.IsNullOrWhiteSpace(txtecos.Text) && statusAnterior > 0)
                    res = true;
                return res;
            }
            else return res;
        }
        bool changesinlist()
        {
            bool res = false;
            if (Convert.ToInt32(string.IsNullOrWhiteSpace(txtecos.Text) ? "0" : txtecos.Text) == ecosAnterior)
            {
                for (int i = 0; i < unidades.Count; i++)
                    if (unidades[i] != unidadesAnterior[i])
                        res = true;
                for (int i = 0; i < diferencia.Count; i++)
                    if (diferencia[i] != diferenciaAnterior[i])
                        res = true;
                if (unidades.Count != unidadesAnterior.Count || diferencia.Count != diferenciaAnterior.Count)
                    res = true;
            }
            else res = true;
            return res;
        }
        void limpiar()
        {
            if (editar)
            {
                unidades.Clear();
                unidadesAnterior.Clear();
                diferencia.Clear();
                diferenciaAnterior.Clear();
            }
            lblunidades.Text = img = imganterior = "";
            mostrarEmpresas();
            nudlapso.Value = cmbempresa.SelectedIndex = empresaAnterior = areaArenterior = servicioAnterior = ciclosAnterior = ecosAnterior = lapsoAnterior = contador = idRol = 0;
            txtecos.Clear();
            txtciclos.Clear();
            lbxdiferencias.Items.Clear();
            dtpincorporo.Value = horaAnterior = DateTime.Parse("00:00");
            btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
            dgvroles.ClearSelection();
            lblimg.Text = "Seleccionar imagen: ";
            lblecos.Text = "seleccionar ecos";
            lbldiff.Text = "establecer diferencias de tiempo";
            psave.Visible = (pinsertar ? true : false);
            pnuevo.Visible = pstatus.Visible = pselectecos.Visible = gbecos.Enabled = gbxdiferencia.Enabled = editar = editardif = editareco = nuevo = !(psave.Visible = true);
            pictureBox1.BackgroundImage = null;
        }

        private void dgvroles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cambios() || nuevo)
                if (MessageBox.Show("¿Desesa " + (nuevo ? "concluir el registro" : "guardar las modificaciones"), validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { }
                else
                    cargarDatos(e);
            else cargarDatos(e);
        }
        void differencebetween(List<string> lista)
        {
            lbxdiferencias.Items.Clear();
            for (int i = 0; i < lista.Count; i++)
                if (!string.IsNullOrWhiteSpace(lista[i]))
                    lbxdiferencias.Items.Add("Diferencia entre unidad " + (i + 1) + " y " + (i + 2) + "-----------> : " + lista[i] + " minutos.");

        }
        void cargarDatos(DataGridViewCellEventArgs e)
        {
            limpiar();
            string[] datosrol = v.getaData("select concat(serviciofkcservicios,'|',nciclos,'|',necos,'|',date_format(horaincorporo,'%H:%i'),'|',diffciclos,'|',status,'|',timediference,'|',image)  from croles where idrol='" + (idRol = Convert.ToInt32(dgvroles.Rows[e.RowIndex].Cells[0].Value)) + "'").ToString().Split('|');
            empresaAnterior = Convert.ToInt32(v.getaData("select idempresa from cempresas as t1 inner join careas as t2 on t1.idempresa=t2.empresafkcempresas inner join cservicios as t3 on t2.idarea=t3.AreafkCareas where t3.idservicio='" + (servicioAnterior = Convert.ToInt32(datosrol[0])) + "';"));
            if (Convert.ToInt32(v.getaData("select status from cempresas where idempresa='" + empresaAnterior + "'")) == 0)
                v.iniCombos("select idempresa as id, upper(nombreEmpresa)as nombre from cempresas where status='1' or idempresa='" + empresaAnterior + "' order by nombreEmpresa;", cmbempresa, "id", "nombre", "-- SELECCIONE UNA EMPRESA --");
            cmbempresa.SelectedValue = empresaAnterior;
            areaArenterior = Convert.ToInt32(v.getaData("select idarea from careas as t1 inner join cservicios as t2 on t2.AreafkCareas=t1.idarea where t2.idservicio='" + servicioAnterior + "';"));
            if (Convert.ToInt32(v.getaData("select status from careas where idarea='" + areaArenterior + "';")) == 0)
                v.iniCombos("select idarea as id, upper(nombreArea) as nombre from careas as t1 inner join cempresas as t2 on t2.idempresa=t1.empresafkcempresas where  empresafkcempresas='" + empresaAnterior + "' and t1.status='1' or idarea='" + areaArenterior + "' order by nombreArea;", cmbarea, "id", "nombre", "-- SELECCIONE UN ÁREA --");
            cmbarea.SelectedValue = areaArenterior;
            if (Convert.ToInt32(v.getaData("select status from cservicios where idservicio='" + servicioAnterior + "';")) == 0)
                v.iniCombos("select idservicio as id, upper(concat(Nombre,' ',Descripcion)) as nombre from cservicios where AreafkCareas='" + areaArenterior + "' and status='1' or idservicio='" + servicioAnterior + "' order by Nombre;", cmbservicio, "id", "nombre", "-- SELECCIONE UN SERVICIO--");
            cmbservicio.SelectedValue = servicioAnterior;
            txtciclos.Text = (ciclosAnterior = Convert.ToInt32(datosrol[1])).ToString();
            txtecos.Text = (ecosAnterior = contador = Convert.ToInt32(datosrol[2])).ToString();
            dtpincorporo.Value = horaAnterior = DateTime.Parse(datosrol[3]);
            nudlapso.Value = lapsoAnterior = Convert.ToInt32(datosrol[4]);
            lblstatus.Text = ((statusAnterior = Convert.ToInt32(datosrol[5])) == 1 ? "Desactivar" : "Reactivar");
            btnstatus.BackgroundImage = (statusAnterior == 1 ? controlFallos.Properties.Resources.sw : controlFallos.Properties.Resources.swv);
            btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
            pstatus.Visible = (pdesactivar ? true : false);
            unidades = new List<int>(ecosAnterior);
            unidadesAnterior = new List<int>(ecosAnterior);
            diferenciaAnterior = new List<string>(ecosAnterior - 1);
            diferencia = new List<string>(ecosAnterior - 1);
            diferenciaAnterior = datosrol[6].Split(',').ToList();
            diferencia = datosrol[6].Split(',').ToList();
            pictureBox1.BackgroundImage = (!string.IsNullOrWhiteSpace(datosrol[7]) ? v.StringToImage(datosrol[7]) : null);
            lblimg.Text = (!string.IsNullOrWhiteSpace((img = imganterior = datosrol[7])) ? "cambiar imagen" : "seleccionar imagen");
            DataTable dt = (DataTable)v.getData("call sistrefaccmant.ecosforlist('" + idRol + "');");
            foreach (DataRow item in dt.Rows)
            { unidadesAnterior.Add(Convert.ToInt32(item.ItemArray[0])); unidades.Add(Convert.ToInt32(item.ItemArray[0])); }
            lblunidades.Text = "Unidades: " + texto(unidadesAnterior);
            lblecos.Text = "Cambiar ecos";
            lbldiff.Text = "Cambiar diferencias de tiempo";
            differencebetween(diferenciaAnterior);
            pnuevo.Visible = editar = !(psave.Visible = false);
            pselectecos.Visible = ptime.Visible = (statusAnterior == 0 ? false : true);
            if (statusAnterior == 0 && peditar && pdesactivar)
                MessageBox.Show("Para editar el registro es necesario reactivar primero el rol", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}