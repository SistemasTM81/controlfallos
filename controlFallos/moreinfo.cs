using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class moreinfo : Form
    {
        validaciones v;
        int? y1 = null;
        string id, ct;
        int empresa, area;
        string sqlFolio;
        bool historial;
        public moreinfo(string id, string ct, int empresa, int area, bool historial, validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.id = id;
            this.ct = ct;
            this.empresa = empresa;
            this.area = area;
            this.historial = historial;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }
        void Buscar()
        {
            if (historial)
            {
                gmmodifs.Visible = true;
                tbmodif.Rows.Clear();
                tbmodif.DataSource = v.getData("SET lc_time_names='es_ES';SELECT t1.idmodificacion as 'Identificador', concat(t2.nombres,' ',t2.apPaterno,' ',t2.apMaterno) as 'Usuario que Modifica',  DATE_FORMAT( t1.fechaHora,'%W, %d de %M del %Y') as 'Fecha de Modificación', time(t1.fechaHora) as 'Hora de Modificación',t1.Tipo as 'Tipo de Modificación' FROM modificaciones_sistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal= t2.idpersona WHERE idregistro='" + id + "' and form='" + v.mayusculas(ct.ToLower()) + "' and t1.empresa='" + empresa + "' and t1.area='" + area + "'");
                tbmodif.Columns[0].Visible = false;
                tbmodif.ClearSelection();
            }
            else
            {
                this.Size = new Size(1225, 517);
                lbltitle.Location = new Point(0, 0);
                lbltitle.Left = (this.panel1.Width - lbltitle.Size.Width) / 2;
                lbltitle.Top = (panel1.Height - lbltitle.Size.Height) / 2;
                CenterToParent();
                gbadd.Dock = DockStyle.Fill;
                string rec = v.getaData("SELECT CONCAT(Tipo,';',idmodificacion) FROM modificaciones_sistema WHERE idmodificacion= '" + id + "' and form='" + ct + "'").ToString();
                string[] dos = rec.Split(';');
                crear(dos[0], dos[1]);
                CenterToParent();
            }
        }

        private void moreinfo_Load(object sender, EventArgs e)
        {
            Buscar();
            acomodarLabel();
        }
        void acomodarLabel()
        {
            lbltitle.Left = (panel1.Width - lbltitle.Width) / 2;
            lbltitle.Top = (panel1.Height - lbltitle.Height) / 2;
        }

        private void panel2_Paint(object sender, PaintEventArgs e) { }
        void crearListBox(string[] data, string texto)
        {
            cerrar();
            Label lbl = new Label();
            lbl.Text = (texto + " Que Fueron Exportados a Formato Excel:").ToUpper();
            lbl.AutoSize = true;
            lbl.Name = "lbl1s";
            lbl.Font = new Font(new FontFamily("Garamond"), 16, FontStyle.Bold);
            lbl.Location = new Point(40, 30);
            gbadd.Controls.Add(lbl);
            int y = 330;
            for (int i = 0; i < data.Length - 1; i++)
            {
                y += 40;
                Label l = new Label();
                l.UseMnemonic = false;
                l.Text = v.mayusculas(data[i]);
                l.AutoSize = true;

                l.Location = new Point(((gbadd.Size.Width - l.Size.Width) / 2), y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;
                gbadd.Controls.Add(l);
                l.Left = (gbadd.Width - l.Size.Width) / 2;
            }
            string[] ids = data[data.Length - 1].Split(';');
            string[] folio = new string[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                folio[i] = UPPER(v.getaData(sqlFolio + "'" + ids[i] + "'").ToString());
            }
            if (texto != "ECONÓMICOS" && texto != "Empleados" && texto != "Proveedores" && texto != "Refacciones".ToUpper()) folio = MetodoBurbuja(folio);
            ListBox list = new ListBox();
            list.Location = new Point(10, 100);
            list.Size = new Size(gbadd.Width - 50, 250);
            list.BackColor = Color.FromArgb(200, 200, 200);
            list.ForeColor = Color.FromArgb(75, 44, 52);
            list.BorderStyle = BorderStyle.None;
            list.MultiColumn = true;
            list.IntegralHeight = true;
            list.ColumnWidth = 500;
            list.DrawMode = DrawMode.OwnerDrawFixed;
            list.Name = "listfolios";
            list.DrawItem += new DrawItemEventHandler(v.listbox_DrawItem);
            list.ItemHeight = 25;
            for (int i = 0; i < folio.Length; i++) list.Items.Add(folio[i]);
            gbadd.Controls.Add(list);

        }

        private void MostrarDatosEspecificosListBox(object sender, EventArgs e)
        {
            var res = ((ListBox)sender).SelectedItem;
            mostrarFolio m = new mostrarFolio(res.ToString(), empresa, area, v);
            m.Owner = this;
            m.ShowDialog();

        }
        private void tbmodif_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            if (row > -1)
            {
                try
                {
                    var tipo = tbmodif.Rows[e.RowIndex].Cells[4].Value.ToString();
                    var id = tbmodif.Rows[e.RowIndex].Cells[0].Value.ToString();
                    crear(tipo, id);
                    CenterToParent();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, v.sistema(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        int tempp4;
        void paraNuevasRefacciones(string[,] data)
        {
            DataGridView tbrefacciones = new DataGridView();
            tbrefacciones.BackgroundColor = Color.FromArgb(200, 200, 200);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.BackColor = Color.FromArgb(180, 180, 180);
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 12, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True;
            tbrefacciones.AlternatingRowsDefaultCellStyle = d;
            tbrefacciones.BorderStyle = BorderStyle.None;
            tbrefacciones.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            d.BackColor = Color.FromArgb(200, 200, 200);
            tbrefacciones.ColumnHeadersDefaultCellStyle = d;
            tbrefacciones.EnableHeadersVisualStyles = false;
            tbrefacciones.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            tbrefacciones.RowHeadersDefaultCellStyle = d;
            tbrefacciones.RowHeadersVisible = false;
            tbrefacciones.RowsDefaultCellStyle = d;
            tbrefacciones.AllowDrop = false;
            tbrefacciones.AllowUserToAddRows = false;
            tbrefacciones.AllowUserToDeleteRows = false;
            tbrefacciones.AllowUserToOrderColumns = false;
            tbrefacciones.AllowUserToResizeColumns = false;
            tbrefacciones.AllowUserToResizeRows = false;
            tbrefacciones.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            tbrefacciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbrefacciones.EditMode = DataGridViewEditMode.EditProgrammatically;
            tbrefacciones.MultiSelect = false;
            tbrefacciones.ReadOnly = true;
            tbrefacciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbrefacciones.Name = "tbrefacciones";
            tbrefacciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tbrefacciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbrefacciones.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView2_CellFormatting);
            tbrefacciones.CellClick += new DataGridViewCellEventHandler(clear);
            tbrefacciones.Columns.Add("codref", "Código de Refacción".ToUpper());
            tbrefacciones.Columns.Add("nomref", "Nombre de Refacción".ToUpper());
            tbrefacciones.Columns.Add(v.mayusculas("ESTATUS DE REFACCIÓN".ToLower()), "Existencia de Refacción".ToUpper());
            tbrefacciones.Columns.Add("cantidadentregada", "Cantidad Entregada".ToUpper());
            gbadd.Controls.Add(tbrefacciones);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                tbrefacciones.Rows.Add(data[i, 0], data[i, 1], data[i, 2], data[i, 3]);
            }
            tbrefacciones.Size = new Size(gbadd.Width - 180, 100);
            tbrefacciones.Location = new Point(0, 200);
            tbrefacciones.Left = (gbadd.Width - tbrefacciones.Width) / 2;
            tbrefacciones.ClearSelection();

        }
        void imagenes(string[] evidencias)
        {
            Point[] locations = new Point[] { new Point(20, 140), new Point(375, 140), new Point(20, 335), new Point(375, 335), new Point(765, 140), new Point(1120, 140), new Point(765, 335), new Point(1120, 335) };
            for (int i = 0; i < evidencias.Length; i++)
            {
                PictureBox p = new PictureBox();
                p.BorderStyle = BorderStyle.Fixed3D;
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                p.Width = 350;
                p.Height = 180;
                p.Location = new Point(locations[i].X, locations[i].Y);
                p.Image = (!string.IsNullOrWhiteSpace(evidencias[i]) ? v.StringToImage(evidencias[i]) : null);
                gbadd.Controls.Add(p);
            }
        }
        void CrearAntesDespuesLabels()
        {
            FontFamily fontFamily = new FontFamily("Garamond");
            Label lbl = new Label();
            lbl.Font = new Font(fontFamily, 16, FontStyle.Bold);
            lbl.Text = v.mayusculas("Anterior").ToUpper();
            lbl.AutoSize = true;
            lbl.Location = new Point((this.Width - lbl.Width) / 4, y1 ?? 40);
            lbl.Name = "lblAntes";
            gbadd.Controls.Add(lbl);
            Label l1 = new Label();
            l1.Text = v.mayusculas("Actual").ToUpper();
            l1.AutoSize = true;
            l1.Location = new Point(((this.Width - lbltitle.Width) / 4) + ((this.Width - l1.Width) / 2), y1 ?? 40);
            l1.Font = new Font(fontFamily, 16, FontStyle.Bold);
            l1.Name = "lblActual";
            gbadd.Controls.Add(l1);
        }
        void crearFormularioPersonalcambios(string[] arreglo)
        {
            cerrar();
            int x = 10;
            int y = 40;

            for (int i = 0; i < 5; i++)
            {
                y += 40;
                Label l = new Label();
                l.UseMnemonic = false;
                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;

                gbadd.Controls.Add(l);
                if (arreglo.Length <= 5)
                {
                    l.Left = (gbadd.Width - l.Size.Width) / 2;
                }
            }
            if (arreglo.Length > 5)
            {


                x = 600;
                y = 40;
                for (int i = 5; i < arreglo.Length; i++)
                {
                    y += 40;
                    Label l = new Label();
                    l.UseMnemonic = false;
                    if (arreglo[i].Length > 40)
                    {
                        int c = 0;
                        string temp2 = "";
                        string temp = "";
                        string[] temp4 = arreglo[i].Split(' ');
                        for (int j = 0; j < temp4.Length; j++)
                        {
                            if ((temp + " " + temp4[j]).Length < 40)
                            {
                                temp += " " + temp4[j];
                            }
                            else
                            {
                                temp2 += temp + Environment.NewLine;
                                temp = "";
                                j--;
                                c += 25;
                            }
                        }
                        if (temp != null)
                        {
                            temp2 += temp;
                            temp = null;
                        }
                        l.Text = v.mayusculas(temp2);

                    }
                    else
                    {
                        l.Text = v.mayusculas(arreglo[i]);
                    }
                    l.AutoSize = true;
                    l.Location = new Point(x, y);
                    l.Font = new Font(this.Font, FontStyle.Bold);
                    l.Name = "lbl" + i;
                    gbadd.Controls.Add(l);
                }
            }
        }
        void mitad1mitad(string[] arreglo)
        {
            cerrar();
            int x = 80;
            int y = y1 ?? 40;

            for (int i = 0; i < arreglo.Length / 2; i++)
            {
                y += 40;
                Label l = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 30;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;

                gbadd.Controls.Add(l);
                if (arreglo.Length <= 5)
                {
                    l.Left = (gbadd.Width - l.Size.Width) / 2;
                }
            }


            x = 600;
            y = y1 ?? 40;
            for (int i = (arreglo.Length / 2); i < arreglo.Length; i++)
            {
                y += 40;
                Label l = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                gbadd.Controls.Add(l);
            }
        }
        void mitad1mitad2(string[] arreglo)
        {
            Panel p = new Panel();
            if (scroll)
            {
                gbadd.Controls.Add(p);
                p.AutoScroll = true;
                p.Size = new Size(1231, 860);
                p.Dock = DockStyle.Bottom;
                p.BorderStyle = BorderStyle.None;
            }
            int x = 40;
            int y = y1 ?? 40;
            if (scroll)
                y = -20;
            for (int i = 0; i < arreglo.Length / 2; i++)
            {

                y += 40;
                if (tempp4 > 0)
                {
                    y += tempp4;
                    tempp4 = 0;
                }
                Label l = new Label();

                if (arreglo[i].Length >= 55)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length <= 55)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 20;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                        tempp4 = 20;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;
                if (scroll)
                    p.Controls.Add(l);
                else
                    gbadd.Controls.Add(l);

            }
            x = (this.Width / 2) + 40;
            y = y1 ?? 40;
            if (scroll)
                y = -20;
            for (int i = (arreglo.Length / 2); i < arreglo.Length; i++)
            {
                y += 40;
                if (tempp4 > 0)
                {
                    y += tempp4;
                    tempp4 = 0;
                }
                Label l = new Label();

                if (arreglo[i].Length >= 55)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length <= 55)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 20;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);
                    tempp4 = 20;
                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                if (scroll)
                    p.Controls.Add(l);
                else
                    gbadd.Controls.Add(l);
            }
        }

        void crearCatalogoPuesto(string[] arreglo)
        {
            cerrar();
            int x = 40;
            int y = 0;
            int c = 0;
            int contador = 0;
            for (int i = 0; i < arreglo.Length; i++)
            {
                y += 40;
                Label l = new Label();
                l.UseMnemonic = false;
                if (arreglo[i].Length > 65)
                {
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 65)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                            contador += 12;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);

                }
                else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }
                l.AutoSize = true;
                l.Location = new Point(x, y);
                l.Font = new Font(this.Font, FontStyle.Bold);
                l.Name = "lbl" + i;
                l.TabIndex = i;
                gbadd.Controls.Add(l);
                l.Left = (this.gbadd.Width - l.Size.Width) / 2;
            }
            y1 = y + 28 + contador;
        }
        void paralabelsImpares(string[] arreglo)
        {
            cerrar();
            int x = 40;
            int y = 40;
            int media = (int)Math.Floor(Decimal.Parse((arreglo.Length / 2).ToString()));
            for (int i = 0; i < media; i++)
            {
                y += 40;
                Label label = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    label.Text = v.mayusculas(temp2);

                }
                else
                {
                    label.Text = v.mayusculas(arreglo[i]);
                }
                label.AutoSize = true;
                label.Location = new Point(x, y);
                label.Font = new Font(this.Font, FontStyle.Bold);
                label.Name = "lbl" + i;
                label.TabIndex = i;

                gbadd.Controls.Add(label);

            }
            x = 600;
            y = 40;
            for (int i = media; i < arreglo.Length - 1; i++)
            {
                y += 40;
                Label label = new Label();

                if (arreglo[i].Length > 40)
                {
                    int c = 0;
                    string temp2 = "";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 40)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 25;
                        }
                    }
                    if (temp != null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    label.Text = v.mayusculas(temp2);

                }
                else
                {
                    label.Text = v.mayusculas(arreglo[i]);
                }
                label.AutoSize = true;
                label.Location = new Point(x, y);
                label.Font = new Font(this.Font, FontStyle.Bold);
                label.Name = "lbl" + i;
                label.TabIndex = i;
                gbadd.Controls.Add(label);
            }
            Label l = new Label();
            l.Text = v.mayusculas(arreglo[arreglo.Length - 1]);
            l.AutoSize = true;
            l.Location = new Point(600, y + 40);
            // l.Left = (this.gbadd.Width - l.Size.Width) / 3;
            l.Font = new Font(Font, FontStyle.Bold);
            l.Name = "lbl" + (arreglo.Length - 1);
            gbadd.Controls.Add(l);

        }
        void crearDataGrid(DataTable dt, Point p)
        {
            DataGridView tbrefacciones = new DataGridView();
            tbrefacciones.BackgroundColor = Color.FromArgb(200, 200, 200);
            DataGridViewCellStyle d = new DataGridViewCellStyle();
            d.Alignment = DataGridViewContentAlignment.MiddleCenter;
            d.BackColor = Color.FromArgb(180, 180, 180);
            d.ForeColor = Color.FromArgb(75, 44, 52);
            d.SelectionBackColor = Color.Crimson;
            d.SelectionForeColor = Color.White;
            d.Font = new Font("Garamond", 12, FontStyle.Bold);
            d.WrapMode = DataGridViewTriState.True;
            tbrefacciones.AlternatingRowsDefaultCellStyle = d;
            tbrefacciones.BorderStyle = BorderStyle.None;
            tbrefacciones.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            d.BackColor = Color.FromArgb(200, 200, 200);
            tbrefacciones.ColumnHeadersDefaultCellStyle = d;
            tbrefacciones.EnableHeadersVisualStyles = false;
            tbrefacciones.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            tbrefacciones.RowHeadersDefaultCellStyle = d;
            tbrefacciones.RowHeadersVisible = false;
            tbrefacciones.RowsDefaultCellStyle = d;
            tbrefacciones.AllowDrop = false;
            tbrefacciones.AllowUserToAddRows = false;
            tbrefacciones.AllowUserToDeleteRows = false;
            tbrefacciones.AllowUserToOrderColumns = false;
            tbrefacciones.AllowUserToResizeColumns = false;
            tbrefacciones.AllowUserToResizeRows = false;
            tbrefacciones.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            tbrefacciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbrefacciones.EditMode = DataGridViewEditMode.EditProgrammatically;
            tbrefacciones.MultiSelect = false;
            tbrefacciones.ReadOnly = true;
            tbrefacciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbrefacciones.Name = "tbrefacciones";
            tbrefacciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tbrefacciones.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbrefacciones.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView2_CellFormatting);
            tbrefacciones.CellClick += new DataGridViewCellEventHandler(clear);
            tbrefacciones.DataSource = dt;
            gbadd.Controls.Add(tbrefacciones);
            tbrefacciones.Size = new Size(gbadd.Width - 180, 200);
            tbrefacciones.Location = p;
            tbrefacciones.Left = (gbadd.Width - tbrefacciones.Width) / 2;
            tbrefacciones.ClearSelection();

        }
        void crearPicturesBox(string[] imagenes)
        {
            int x = 300;
            for (int i = 0; i < imagenes.Length; i++)
            {
                if (imagenes[i] != "")
                {
                    PictureBox p1 = new PictureBox();
                    p1.BackgroundImageLayout = ImageLayout.Stretch;
                    p1.Size = new Size(100, 100);

                    p1.Location = new Point(x, 350);
                    p1.BackgroundImage = v.StringToImage(imagenes[i]);
                    gbadd.Controls.Add(p1);
                    if (imagenes.Length == 1)
                    {
                        p1.Left = (gbadd.Width - p1.Width) / 2;
                    }
                }
                x += 500;
            }
        }
        private void clear(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView tbRefacciones = sender as DataGridView;
            tbRefacciones.ClearSelection();
        }
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView tbRefacciones = sender as DataGridView;
            if (tbRefacciones.Columns[e.ColumnIndex].Name == v.mayusculas("ESTATUS DE REFACCIÓN".ToLower()))
            {
                if (Convert.ToString(e.Value) == "EXISTENCIA")
                {
                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    if (Convert.ToString(e.Value) == "SIN EXISTENCIA")
                    {
                        e.CellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
            if (tbRefacciones.Columns[e.ColumnIndex].Name == v.mayusculas("CANTIDAD FALTANTE".ToLower()))
            {
                if (Convert.ToInt32(e.Value ?? 0) > 0)
                {
                    e.CellStyle.BackColor = Color.Khaki;
                }
                else
                {
                    if (Convert.ToInt32(e.Value) == 0)
                    {
                        e.CellStyle.BackColor = Color.PaleGreen;
                    }
                }
            }
        }

        void crear(string tipo, string id)
        {
            try
            {
                switch (tipo)
                {
                    case "Actualización de Datos Personales":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ', t1.Tipo)) FROM modificaciones_sistema  as t1 INNER JOIN cpersonal as t2 On t1.idregistro = t2.idpersona WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));

                        string cadena = v.getaData("SET lc_time_names = 'es_mx'; SELECT UPPER(concat(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/Hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Datos Personales' or x4.Tipo='Inserción de Empleado') and x4.idregistro=t2.idpersona and(x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1), ';CREDENCIAL: ', t2.credencial, ';APELLIDO P: ', t2.apPaterno, ';APELLIDO M: ', t2.apMaterno, ';nombre: ', t2.nombres,';tipo: ',coalesce((select if(x1.descripcion!='',concat(x1.tipo,' - ',x1.descripcion),x1.tipo) from cattipos as x1 inner join vigencias_supervision as x2 on x2.tipolicenciafkcattipos=x1.idcattipos where x2.usuariofkcpersonal=t2.idpersona),''),';Expedición: ',coalesce((select date_format(t3.fechaEmisionConducir,'%W, %d de %M del %Y') from vigencias_supervision as t3 where t3.usuariofkcpersonal=t2.idpersona),''),';Vencimiento: ',coalesce((select date_format(t4.fechaVencimientoConducir,'%W, %d de %M del %Y') from vigencias_supervision as t4 where t4.usuariofkcpersonal=t2.idpersona),''),(select if(t1.empresa='1' and t1.area='1',(select concat(';expedición: ',date_format(t5.fechaEmisionTarjeton,'%W, %d de %M del %Y')) from vigencias_supervision as t5 where t5.usuariofkcpersonal=t2.idpersona),'')),(select if(t1.empresa='1' and t1.area='1',(select concat(';vencimiento: ',date_format(t6.fechaVencimientoTarjeton,'%W, %d de %M del %Y')) from vigencias_supervision as t6 where t6.usuariofkcpersonal=t2.idpersona),'')) ,';PUESTO: ', (select puesto FROM puestos WHERE idpuesto = t2.cargofkcargos),';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where t1.usuariofkcpersonal=x2.idPersona),';fecha/hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y '),'/',time(t1.fechahora))) FROM modificaciones_sistema  as t1 INNER JOIN cpersonal as t2 On t1.idregistro = t2.idpersona WHERE idmodificacion = '" + id + "'").ToString();

                        string[] datos = cadena.Split(';');
                        if (!string.IsNullOrWhiteSpace(datos[5])) { datos[5] = string.Format("{0:D}", DateTime.Parse(datos[5])); datos[6] = string.Format("{0:D}", DateTime.Parse(datos[6])); }
                        datos[0] = ("CREDENCIAL: " + datos[0]).ToUpper();
                        datos[1] = ("APELLIDO P: " + datos[1]).ToUpper();
                        datos[2] = ("APELLIDO M: " + datos[2]).ToUpper();
                        datos[3] = ("NOMBRE: " + datos[3]).ToUpper();
                        datos[4] = ("tipo: " + v.getaData("select if(descripcion!='',concat(tipo,' - ',descripcion),tipo) from cattipos where idcattipos='" + datos[4] + "'".ToString())).ToUpper();
                        datos[5] = ("expedición licencia: " + datos[5]).ToUpper();
                        datos[6] = ("vencimiento licencia: " + datos[6]).ToUpper();
                        if (datos.Length == 24)
                        {
                            datos[7] = ("expedición tarjeton: " + string.Format("{0:D}", DateTime.Parse(datos[7]))).ToUpper();
                            datos[8] = ("vencimiento tarjeton: " + string.Format("{0:D}", DateTime.Parse(datos[8]))).ToUpper();
                            datos[9] = ("PUESTO: " + v.getaData("SELECT puesto FROM puestos WHERE idpuesto='" + datos[9] + "'").ToString()).ToUpper();
                            datos[10] = ("usuario que modificó: " + datos[10]).ToUpper();
                            datos[11] = ("" + datos[11]).ToUpper();
                            datos[12] = ("" + datos[12]).ToUpper();
                            datos[13] = ("" + datos[13]).ToUpper();
                            datos[14] = ("" + datos[14]).ToUpper();
                            datos[15] = ("" + datos[15]).ToUpper();
                            datos[16] = ("" + datos[16]).ToUpper();
                            datos[17] = ("" + datos[17]).ToUpper();
                            datos[18] = ("" + datos[18]).ToUpper();
                            datos[19] = ("" + datos[19]).ToUpper();
                            datos[20] = ("" + datos[20]).ToUpper();
                            datos[21] = ("" + datos[21]).ToUpper();
                            datos[22] = ("" + datos[22]).ToUpper();
                            this.Size = new Size(1325, 940);
                            gbadd.Size = new Size(1231, 655);
                            CenterToParent();
                        }
                        else
                        {
                            datos[7] = ("PUESTO: " + v.getaData("SELECT puesto FROM puestos WHERE idpuesto='" + datos[7] + "'").ToString()).ToUpper();
                            datos[8] = ("Usuario que modificó: " + datos[8]).ToUpper();
                            datos[9] = ("" + datos[9]).ToUpper();
                            datos[10] = ("" + datos[10]).ToUpper();
                            datos[11] = ("" + datos[11]).ToUpper();
                            datos[12] = ("" + datos[12]).ToUpper();
                            datos[13] = ("" + datos[13]).ToUpper();
                            datos[14] = ("" + datos[14]).ToUpper();
                            datos[15] = ("" + datos[15]).ToUpper();
                            datos[16] = ("" + datos[16]).ToUpper();
                            datos[17] = ("" + datos[17]).ToUpper();
                            datos[18] = ("" + datos[18]).ToUpper();
                            this.Size = new Size(1325, 850);
                        }
                        y1 = 130;
                        CrearAntesDespuesLabels();
                        y1 = 150;
                        mitad1mitad2(datos);
                        y1 = null;
                        break;
                    case "Actualización de Usuario":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((SELECT UPPER(CONCAT('CREDENCIAL: ', credencial, ';NOMBRE: ',nombres,' ',apPaterno,' ', apMaterno,';Puesto: ', t3.puesto,';')) FROM cpersonal INNER JOIN puestos as t3 ON cargofkcargos=t3.idpuesto WHERE t1.idregistro=idpersona ),'Motivo de actualización: ',t1.motivoActualizacion)) FROM modificaciones_sistema  as t1 INNER JOIN cpersonal as t2 On t1.idregistro = t2.idpersona WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        string[] actusuario = v.getaData(@"SET lc_time_names = 'es_ES';SELECT CONCAT(CONCAT( t1.ultimamodificacion,';Usuario que modificó: ',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Usuario' or x4.Tipo='Inserción de Usuario' or x4.Tipo='Inserción de Empleado') and x4.idregistro=t2.idpersona and(x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1), (SELECT  CONCAT(upper(';Usuario: '), usuario,';')  FROM  datosistema WHERE usuariofkcpersonal = t1.idregistro), (SELECT password FROM datosistema WHERE usuariofkcpersonal = t1.idregistro),';usuario que modificó: ',(select concat(x1.nombres,' ',x1.apPaterno,' ',x1.apMaterno) from cpersonal as x1 where x1.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t1.idregistro = t2.idpersona  INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        actusuario[0] = "Usuario anterior: ".ToUpper() + actusuario[0];
                        actusuario[1] = "Contraseña anterior: ".ToUpper() + v.Desencriptar(actusuario[1]);
                        actusuario[2] = "" + actusuario[2].ToUpper();
                        actusuario[3] = "" + actusuario[3].ToUpper();
                        actusuario[4] = "" + actusuario[4];
                        actusuario[5] = "Contraseña: ".ToUpper() + v.Desencriptar(actusuario[5]);
                        actusuario[6] = "" + actusuario[6].ToUpper();
                        actusuario[7] = "" + actusuario[7].ToUpper();
                        y1 = 180;
                        CrearAntesDespuesLabels();
                        y1 = 190;
                        mitad1mitad2(actusuario);
                        break;
                    case "Desactivación de Empleado":
                    case "Reactivación de Empleado":
                    case "Desactivación":
                    case "Reactivación":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),(select concat(';Nombre: ',x1.nombres,' ',x1.apPaterno,' ',x1.ApMaterno,';credencial: ',x1.credencial) from cpersonal as x1 where x1.idpersona=t1.idregistro),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cpersonal as t2 on t1.idregistro=t2.idPersona WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        y1 = 180;
                        string[] _empleado = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Empleado','Usuario que activo: ',if(x4.tipo='Reactivación de Empleado','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Empleado','Activación de Empleado',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Empleado') or (x4.Tipo='Reactivación de Empleado') or (x4.Tipo='Inserción de Empleado')) and (x4.idregistro=t2.idpersona) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Nombre de Familia',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cpersonal as t2 on t1.idregistro=t2.idpersona WHERE idmodificacion = '" + id + "';").ToString().Split(';'); CrearAntesDespuesLabels();
                        y1 = 190;
                        mitad1mitad2(_empleado);
                        break;

                    case "Inserción de Empleado":
                        string[] empleado = v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT(ultimamodificacion,';Usuario que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',Tipo)) FROM modificaciones_sistema WHERE idmodificacion = '" + id + "'").ToString().Split(';');
                        empleado[0] = ("Credencial: " + empleado[0]).ToUpper();
                        empleado[1] = ("Apellido Paterno: " + empleado[1]).ToUpper();
                        empleado[2] = ("Apellido Materno: " + empleado[2]).ToUpper();
                        empleado[3] = ("Nombres: " + empleado[3]).ToUpper();
                        empleado[4] = ("Puesto: " + v.getaData("SELECT puesto From puestos WHERE idpuesto='" + empleado[4] + "'")).ToUpper();
                        empleado[5] = ("Tipo De Licencia: " + v.getaData("Select if(descripcion!='',concat(tipo,' - ',descripcion),tipo) as tipo from cattipos where idcattipos='" + empleado[5] + "'")).ToUpper();
                        empleado[6] = ("Fecha expedición licencia :" + empleado[6]).ToUpper();
                        empleado[7] = ("Fecha vencimiento licencia :" + empleado[7]).ToUpper();
                        if (empleado.Length == 13)
                        {
                            if (empleado[8] == "") empleado[8] = null;
                            empleado[8] = ("Usuario: " + (empleado[8] ?? "\"Sin Usuario\"")).ToUpper();
                            if (empleado[9] == "") empleado[9] = null;
                            empleado[9] = ("Contraseña: " + (empleado[9] ?? "\"Sin Contraseña\"")).ToUpper();
                            this.Size = new Size(1325, 850);
                            CenterToParent();
                        }
                        else
                        {
                            empleado[8] = ("Fecha expedición tarjeton: " + empleado[8]).ToUpper();
                            empleado[9] = ("fecha vencimiento tarjeton " + empleado[9]).ToUpper();
                            if (empleado[10] == "") empleado[10] = null;
                            empleado[10] = ("Usuario: " + (empleado[10] ?? "\"Sin Usuario\"")).ToUpper();
                            if (empleado[11] == "") empleado[11] = null;
                            empleado[11] = ("Contraseña: " + (empleado[11] ?? "\"Sin Contraseña\"")).ToUpper();
                            this.Size = new Size(1325, 920);
                            CenterToParent();
                        }

                        crearCatalogoPuesto(empleado);
                        break;
                    case "Inserción de Usuario":
                        string[] dato = v.getaData("SET lc_time_names = 'es_ES';SELECT CONCAT(upper(CONCAT('Credencial: ', t2.credencial, ';','Nombre: ',  t2.nombres,  ';Apellido P.:',   t2.apPaterno,';Apellido M: ', t2.apMaterno,';Puesto: ', t3.puesto,';')),ultimamodificacion,UPPER(CONCAT('; Usuario que Modifica: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) From CPERSONAL WHERE idpersona=t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')))) AS M FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t1.idregistro = t2.idpersona  INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        if (dato[5] != "")
                        {
                            dato[5] = "USUARIO: " + dato[5];
                            dato[6] = "CONTRASEÑA: " + v.Desencriptar(dato[6]);
                        }
                        crearCatalogoPuesto(dato);
                        break;
                    case "Eliminación de Usuario":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT(CONCAT('Credencial: ', t2.credencial, ';'),CONCAT('Nombre: ',  t2.nombres,  ';Apellido P.:',   t2.apPaterno,';Apellido M: ', t2.apMaterno,   ';'),  CONCAT('Puesto: ', t3.puesto),';Usuario que Modifica: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) From CPERSONAL WHERE idpersona=t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'))) AS M FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t1.idregistro = t2.idpersona  INNER JOIN puestos AS t3 ON t2.cargofkcargos = t3.idpuesto WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';'));

                        break;
                    case "Desactivación de Puesto":
                    case "Reactivación de Puesto":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 650);
                            gbadd.Size = new Size(1231, 350);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';Puesto: ',t2.puesto,';Tipo: ', t1.Tipo))FROM modificaciones_sistema as t1 INNER join puestos as t2 on t2.idpuesto=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _puesto = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Puesto','Usuario que activo: ',if(x4.tipo='Reactivación de Puesto','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Puesto','Activación de Puesto',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Puesto') or (x4.Tipo='Reactivación de Puesto') or (x4.Tipo='Inserción de Puesto')) and (x4.idregistro=t2.idpuesto) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Puesto',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join puestos as t2 on t2.idpuesto=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_puesto);
                        break;
                    case "Actualización de Puesto":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 650);
                            gbadd.Size = new Size(1231, 350);
                            CenterToParent();
                        }

                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(concat('Motivo de modificación: ',coalesce(t1.motivoActualizacion),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER JOIN puestos as t2 ON t1.idregistro = t2.idpuesto WHERE idmodificacion='" + id + "'").ToString().Split(';'));

                        string[] arreglo = v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT('Puesto Anterior: ',t1.ultimaModificacion,';usuario que modificó: ',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Puesto' or x4.TIpo='Inserción de Puesto') and x4.idregistro=t2.idpuesto and(x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Puesto Actual: ',t2.puesto,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where t1.usuariofkcpersonal=x2.idPersona),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN puestos as t2 ON t1.idregistro= t2.idpuesto WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        arreglo[0] = "" + arreglo[0];
                        arreglo[1] = "" + arreglo[1];
                        arreglo[2] = "" + arreglo[2];
                        arreglo[3] = "" + arreglo[3];
                        arreglo[4] = "" + arreglo[4];
                        arreglo[5] = "" + arreglo[5];
                        y1 = 120;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(arreglo);
                        break;
                    case "Inserción de Puesto":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 650);
                            gbadd.Size = new Size(1231, 350);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT('Nombre del Puesto: ',t2.puesto,';','Usuario que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),'; FECHA / HORA: ',(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')))) FROM modificaciones_sistema as t1 INNER JOIN puestos as t2 ON t1.idregistro= t2.idpuesto WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Desactivación de Unidad":
                    case "Reactivación de Unidad":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';unidad: ',(select CONCAT(x2.identificador, LPAD(x1.consecutivo, 4, '0')) from cunidades as x1 inner join careas as x2 on x2.idarea=x1.areafkcareas inner join cempresas as x3 on x2.empresafkcempresas=x3.idempresa where t1.idregistro=x1.idunidad),' - descripción: ',t2.descripcioneco,' - Empresa: ',(select x3.nombreEmpresa from cunidades as x1 inner join careas as x2 on x2.idarea=x1.areafkcareas inner join cempresas as x3 on x2.empresafkcempresas=x3.idempresa where t1.idregistro=x1.idunidad),' - área: ',(select x2.nombreArea from cunidades as x1 inner join careas as x2 on x2.idarea=x1.areafkcareas  where t1.idregistro=x1.idunidad)),';Tipo: ', t1.Tipo))FROM modificaciones_sistema as t1 INNER join cunidades as t2 on t2.idunidad=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _unidad = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Unidad','Usuario que activo: ',if(x4.tipo='Reactivación de Unidad','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Unidad','Activación de Unidad',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Unidad') or (x4.Tipo='Reactivación de Unidad') or (x4.Tipo='Inserción de Unidad')) and (x4.idregistro=t2.idunidad) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Unidad',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cunidades as t2 on t2.idunidad=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_unidad);
                        break;
                    case "Inserción de Especificaciones":
                        string[] esp = v.getaData("SET lc_time_names='es_ES';select upper(concat('ECO: ',(select concat(t3.identificador,LPAD(consecutivo,4,'0')) from careas as t3 where t2.areafkcareas = t3.idArea),';',t1.ultimaModificacion,';Fecha/hora: ',concat(DATE_FORMAT( t1.fechaHora,'%W, %d de %M del %Y'),' - ',time(t1.fechaHora)),';Usuario que inserto: ',(select concat(p1.apPaterno,' ',p1.apMaterno,' ',p1.nombres) from cpersonal as p1 where p1.idpersona=t1.usuariofkcpersonal),';tipo: ',t1.tipo)) from modificaciones_sistema as t1 inner join cunidades as t2 on t2.idunidad=t1.idregistro where t1.idmodificacion='" + id + "';").ToString().Split(';');
                        esp[0] = ("" + esp[0]).ToUpper(); ;
                        esp[1] = ("VIn: " + esp[1]).ToUpper();
                        esp[2] = ("N° de serie de motor: " + esp[2]).ToUpper(); ;
                        esp[3] = ("N° de serie de transmisión: " + esp[3]).ToUpper();
                        esp[5] = ("Marca: " + esp[5]).ToUpper(); ;
                        esp[4] = ("Modelo: " + esp[4]).ToUpper();
                        esp[6] = ("" + esp[6]).ToUpper(); ;
                        esp[7] = ("" + esp[7]).ToUpper(); ;
                        esp[8] = ("" + esp[8]).ToUpper(); ;
                        crearCatalogoPuesto(esp);
                        break;
                    case "Inserción de Unidad":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 700);
                            gbadd.Size = new Size(1231, 400);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT((SELECT CONCAT('ECO: ',concat(t2.identificador,LPAD(consecutivo,4,'0')),';Descripcion: ',t1.descripcioneco,';Empresa: ',t3.nombreEmpresa,';Area: ',t2.nombreArea,(select if(t1.serviciofkcservicios = '1', '',(select CONCAT(';Servicio: ',t22.Nombre, ': ',t22.Descripcion) FROM cunidades as t11 INNER JOIN cservicios as t22 ON t11.serviciofkcservicios = t22.idservicio where t11.idunidad =t1.idunidad)))) FROM cunidades  as t1 INNER JOIN careas as t2 ON t1.areafkcareas = t2.idArea INNER JOIN cempresas as t3 ON t2.empresafkcempresas= t3.idEmpresa WHERE idunidad=modsis.idregistro),';Usuario que Modifica: ',CONCAT(cp.nombres,' ',cp.apPaterno,' ',cp.apMaterno),';Fecha: ',DATE_FORMAT( modsis.fechaHora,'%W, %d de %M del %Y'),';Hora: ',time(modsis.fechaHora),';Tipo: ',modsis.Tipo))FROM modificaciones_sistema as modsis INNER JOIN cpersonal as cp ON modsis.usuariofkcpersonal=cp.idpersona where idmodificacion= '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Unidad":
                        acomodarLabel();
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES'; SELECT CONCAT('ECONÓMICO: ', CONCAT(t4.identificador, LPAD(t3.consecutivo, 4, '0')), ';MOTIVO DE ACTUALIZACIÓN: ', UPPER(t1.motivoActualizacion)) FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t2.idpersona = t1.usuariofkcpersonal INNER JOIN cunidades AS t3 ON t1.idregistro = t3.idunidad INNER JOIN careas AS t4 ON t3.areafkcareas = t4.idarea WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        if (empresa == 1)
                        {
                            if (this.Size == new Size(1296, 905))
                            {
                                this.Size = new Size(1295, 700);
                                gbadd.Size = new Size(1231, 400);
                                CenterToParent();
                            }
                            string[] unidad = v.getaData("SET lc_time_names = 'es_ES'; SELECT CONCAT(t1.ultimaModificacion, ';USUARIO QUE MODIFICÓ: ',(select UPPER(concat(x3.nombres, ' ', x3.ApPaterno, ' ', x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora))) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal = x3.idpersona WHERE (x4.Tipo = 'Actualización de Unidad' or x4.tipo='Inserción de Unidad') and x4.idregistro = t2.idunidad and(x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1, 1), ';ECONÓMICO: ', CONCAT(t3.identificador, LPAD(t2.consecutivo, 4, '0')), ';ÁREA: ', UPPER(CONCAT(t4.nombreEmpresa, ' -> ', t3.nombreArea)), ';DESCRIPCIÓN: ', UPPER(t2.descripcioneco), ';SERVICIO: ', (SELECT IF(t2.serviciofkcservicios = '0', 'SIN SERVICIO FIJO', (SELECT UPPER(t5.nombre) FROM cservicios AS t5 WHERE t2.serviciofkcservicios = t5.idservicio))),';USUARIO QUE MODIFICA: ',(SELECT UPPER(CONCAT(nombres, ' ', apPaterno, ' ', apMaterno)) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';FECHA/HORA: ',upper(date_format(t1.fechaHora,'%W, %d de %M del %Y')),'/',time(t1.fechahora)) FROM modificaciones_sistema AS t1 INNER JOIN cunidades AS t2 ON t1.idregistro = t2.idunidad INNER JOIN careas AS t3 ON t2.areafkcareas = t3.idarea INNER JOIN cempresas AS t4 ON t3.empresafkcempresas = t4.idempresa WHERE t1.idmodificacion = '" + id + "'; ").ToString().Split(';');

                            unidad[0] = "ECONÓMICO: " + v.getaData("SELECT concat(identificador, LPAD('" + unidad[0] + "', 4, '0')) from careas WHERE idarea='" + unidad[1] + "';");
                            unidad[1] = ("ÁREA: " + v.getaData("SELECT concat(t2.nombreEmpresa,' -> ',t1.nombreArea) FROM careas as t1 INNER JOIN cempresas as t2 On t1.empresafkcempresas=t2.idempresa WHERE t1.idarea='" + unidad[1] + "'")).ToUpper();
                            unidad[2] = ("DESCRIPCIÓN: " + unidad[2]).ToUpper();
                            unidad[3] = ("SERVICIO: " + (v.getaData("SELECT COALESCE(Nombre,'SIN SERVICIO') FROM cservicios where idservicio = '" + unidad[3] + "';") ?? "SIN SERVICIO FIJO")).ToUpper();
                            y1 = 120;
                            CrearAntesDespuesLabels();
                            mitad1mitad2(unidad);
                            y1 = null;
                        }
                        else
                        {
                            if (this.Size == new Size(1296, 905))
                            {
                                this.Size = new Size(1295, 740);
                                gbadd.Size = new Size(1231, 440);
                                CenterToParent();
                            }
                            string[] unidadtri = v.getaData("SET lc_time_names = 'es_ES'; SELECT CONCAT(t1.ultimaModificacion,coalesce((select upper(concat(if(x4.tipo='Inserción de Especificaciones',';Usuario que inserto: ',';usuario que modificó: '),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';FECHA/HORA: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora))) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Unidad' or x4.tipo='Inserción de Especificaciones') and x4.idregistro=t2.idunidad and(x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select upper(concat(x5.nombres,' ',x5.apPaterno,' ',x5.apMaterno)) from cpersonal as x5 where t2.usuariofkcpersonaltri=x5.idPersona)), ';VIN: ', UPPER(t2.bin), ';NO. DE SERIE DE MOTOR: ', t2.nmotor, ';NO. DE SERIE DE TRANSMISIÓN: ', t2.ntransmision, ';MODELO: ', UPPER(t2.modelo), ';MARCA: ', UPPER(t2.marca),';USUARIO QUE MODIFICA: ',(SELECT upper(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';FECHA/HORA: ',concat(upper(DATE_FORMAT( t1.fechaHora,'%W, %d de %M del %Y')),'/',time(t1.fechaHora))) FROM modificaciones_sistema AS t1 INNER JOIN cunidades AS t2 ON t1.idregistro = t2.idunidad WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';');
                            unidadtri[0] = ("VIN: " + unidadtri[0]).ToUpper();
                            unidadtri[1] = ("NO. DE SERIE DE MOTOR: " + unidadtri[1]).ToUpper();
                            unidadtri[2] = ("NO. DE SERIE DE TRANSMISIÓN: " + unidadtri[2]).ToUpper();
                            unidadtri[3] = ("MODELO: " + unidadtri[3]).ToUpper();
                            unidadtri[4] = ("MARCA: " + unidadtri[4]).ToUpper();
                            y1 = 110;
                            CrearAntesDespuesLabels();
                            mitad1mitad2(unidadtri);
                            y1 = null;
                        }
                        break;
                    case "Desactivación de Empresa":
                    case "Reactivación de Empresa":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';empresa: ',t2.nombreEmpresa,';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cempresas as t2 on t2.idempresa=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _empresa = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Empresa','Usuario que activo: ',if(x4.tipo='Reactivación de Empresa','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Empresa','Activación de Empresa',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Empresa') or (x4.Tipo='Reactivación de Empresa') or (x4.Tipo='Inserción de Empresa')) and (x4.idregistro=t2.idempresa) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Empresa',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cempresas as t2 on t2.idempresa=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_empresa);
                        break;
                    case "Inserción de Empresa":
                        if (empresa == 1 && area == 1)
                        {
                            if (this.Size == new Size(1296, 905))
                            {
                                this.Size = new Size(1296, 625);
                                gbadd.Size = new Size(1231, 325);
                            }

                            crearFormularioPersonalcambios(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Empresa: ', t2.nombreEmpresa, ';Usuario que Modifica: ', CONCAT(t3.nombres, ' ', t3.apPaterno, ' ', t3.apMaterno),';Tipo de Modificación: ', t1.Tipo, ';Estatus Actual: ', if (t2.status = 1,'Activo','Inactivo'),';FECHA / HORA: ',UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')))) FROM modificaciones_sistema as t1 INNER JOIN cempresas as t2 on t1.idregistro = t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idpersona WHERE idmodificacion = '" + id + "' ").ToString().Split(';'));
                        }
                        else
                        {
                            crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat('Nombre de Empresa Anterior: ',t1.ultimamodificacion,';Nombre de Empresa Actual: ',t2.nombreEmpresa,';Usuario que Modifica: ',CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';FECHA / HORA: ',UPPER(DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s')),';Tipo de Modificación: ',t1.Tipo,';Estatus Actual: ', if(t2.status=1,'Activo','Inactivo'),';Logo:')) FROM  modificaciones_sistema as t1 INNER JOIN cempresas as t2 On t1.idregistro= t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion='" + id + "'").ToString().Split(';'));
                            string[] imagenes = v.getaData("SELECT CONCAT(COALESCE(logo,'')) AS M FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cempresas as t2 ON t1.idregistro= t2.idempresa where idmodificacion ='" + id + "' ;").ToString().Split('*');
                            crearPicturesBox(imagenes);
                        }
                        break;
                    case "Actualización de Empresa":

                        if (empresa == 1 && area == 1)
                        {
                            if (this.Size == new Size(1296, 905))
                            {
                                this.Size = new Size(1296, 625);
                                gbadd.Size = new Size(1231, 325);
                            }
                            crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat('motivo de actualización: ',t1.motivoActualizacion,';Tipo de Modificación: ',t1.Tipo,';Estatus Actual: ', if(t2.status=1,'Activo','Inactivo'))) FROM  modificaciones_sistema as t1 INNER JOIN cempresas as t2 On t1.idregistro= t2.idempresa INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion='" + id + "'").ToString().Split(';'));

                            string[] empresas = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat(t1.ultimaModificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Empresa' or x4.tipo='Inserción de Empresa') and x4.idregistro=t2.idempresa and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Empresa: ',t2.nombreEmpresa,';Usuario que modificó: ',concat(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno,';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora)))) FROM modificaciones_sistema  as t1 inner join cempresas as t2 on t2.idempresa=t1.idregistro inner join cpersonal as t3 on t3.idpersona=t1.usuariofkcpersonal where t1.idmodificacion='" + id + "'").ToString().Split(';');
                            empresas[0] = "Empresa: ".ToUpper() + empresas[0];
                            empresas[1] = "usuario que modificó: ".ToUpper() + empresas[1].ToUpper();
                            empresas[2] = "" + empresas[2];
                            empresas[3] = "" + empresas[3];
                            empresas[4] = "" + empresas[4];
                            empresas[5] = "" + empresas[5];
                            y1 = 150;
                            CrearAntesDespuesLabels();
                            mitad1mitad2(empresas);
                        }
                        else if (empresa == 2 && area == 2)
                        {
                            crearCatalogoPuesto(v.getaData("SET lc_time_names ='es_ES';SELECT upper(CONCAT('Nombre de Empresa: ',nombreEmpresa,';Usuario que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cempresas as t2 ON t1.idregistro= t2.idempresa where idmodificacion ='" + id + "' ;").ToString().Split(';'));
                            y1 = 230;
                            CrearAntesDespuesLabels();
                            y1 = 250;
                            mitad1mitad2(v.getaData("SET lc_time_names ='es_ES';SELECT UPPER(concat(coalesce((select concat('USUARIO QUE MODIFICÓ: ',x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Empresa' or x4.TIpo='Inserción de Empresa') and x4.idregistro=t2.idempresa and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1)),';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN cempresas as t2 On t1.idregistro=t2.idempresa WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                            string[] imagenes = v.getaData("SELECT CONCAT(COALESCE(ultimaModificacion,''),'*',COALESCE(logo,'')) AS M FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cempresas as t2 ON t1.idregistro= t2.idempresa where idmodificacion ='" + id + "' ;").ToString().Split('*');
                            crearPicturesBox(imagenes);
                        }
                        break;
                    case "Desactivación de Area":
                    case "Reactivación de Area":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';empresa: ',(select nombreEmpresa from cempresas as x1 inner join careas as x2 on x2.EmpresafkcEmpresas=x1.idempresa where x2.idarea=t1.idregistro),' - área: ',t2.nombreArea,' - identificador: ',t2.identificador),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join careas as t2 on t2.idarea=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] _area = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Area','Usuario que activo: ',if(x4.tipo='Reactivación de Area','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Area','Activación de Area',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Area') or (x4.Tipo='Reactivación de Area') or (x4.Tipo='Inserción de Area')) and (x4.idregistro=t2.idarea) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Area',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join careas as t2 on t2.idarea=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_area);
                        break;
                    case "Inserción de Área":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 710);
                            gbadd.Size = new Size(1231, 410);
                            CenterToParent();
                        }
                        y1 = 190;
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT('Empresa: ', (SELECT nombreEmpresa FROM cempresas WHERE idempresa = t2.empresafkcempresas), ';Área: ', t2.nombreArea, ';Usuario que Inserta: ', CONCAT(t3.nombres, ' ', t3.apPaterno, ' ', t3.apMaterno), ';Fecha/Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'), ';Tipo de Modificación: ', t1.Tipo, ';Estatus Actual: ', if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN careas as t2 On t1.idregistro = t2.idarea INNER JOIN cpersonal as t3 ON t1.usuariofkcpersonal = t3.idpersona WHERE idmodificacion = '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Área":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 710);
                            gbadd.Size = new Size(1231, 410);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat('Motivo de modificación: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN careas as t2 On t1.idregistro=t2.idarea INNER JOIN cpersonal as t3 On t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';'));

                        string[] areas = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat(t1.ultimaModificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Area' or x4.Tipo='Inserción de Área') and x4.idregistro=t2.idarea and(x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),';Empresa: ',(select x1.nombreEmpresa from cempresas as x1 where x1.idempresa=t2.empresafkcempresas),';Identificador: ',t2.identificador,';área: ',t2.nombreArea,';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) from modificaciones_sistema as t1 inner join careas as t2 On t1.idregistro=t2.idarea INNER JOIN cpersonal as t3 On t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');

                        areas[0] = "Empresa: ".ToUpper() + v.getaData("select nombreEmpresa from cempresas where idempresa='" + areas[0] + "'").ToString().ToUpper();
                        areas[1] = "Identificador: ".ToUpper() + areas[1].ToUpper();
                        areas[2] = "Área: ".ToUpper() + areas[2].ToUpper();
                        areas[3] = "Usuario que modificó: ".ToUpper() + areas[3].ToUpper();
                        areas[4] = "" + areas[4].ToUpper();
                        areas[5] = "" + areas[5].ToUpper();
                        areas[6] = "" + areas[6].ToUpper();
                        areas[7] = "" + areas[7].ToUpper();
                        areas[8] = "" + areas[8].ToUpper();
                        areas[9] = "" + areas[9].ToUpper();
                        y1 = 150;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(areas);
                        break;
                    case "Desactivación de Servicio":
                    case "Reactivación de Servicio":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';empresa: ',(select x1.nombreEmpresa from cempresas as x1 inner join careas as x2 on x2.empresafkcempresas=x1.idempresa inner join cservicios as x3 on x3.AreafkCareas=x2.idarea where x3.idservicio=t1.idregistro),' - Área: ',(select nombreArea from careas as x4 inner join cservicios as x5 on x5.AreafkCareas=x4.idarea where x5.idservicio=t1.idregistro),' - Servicio: ',t2.Nombre,' - Descripción: ',t2.Descripcion),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cservicios as t2 on t2.idservicio=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _servicio = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Servicio','Usuario que activo: ',if(x4.tipo='Reactivación de Servicio','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Servicio','Activación de Servicio',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Servicio') or (x4.Tipo='Reactivación de Servicio') or (x4.Tipo='Inserción de Servicio')) and (x4.idregistro=t2.idservicio) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Servicio',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo))FROM modificaciones_sistema as t1 INNER join cservicios as t2 on t2.idservicio=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_servicio);
                        break;
                    case "Inserción de Servicio":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Nombre de Servicio: ', t2.Nombre,';Descripción de Servicio: ',t2.descripcion,';Usuario que Inserta: ',CONCAT(t3.nombres,' ',t3.apPaterno,' ',t3.apMaterno),';Tipo: ',t1.Tipo,';Fecha/Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cservicios as t2 On t1.idregistro=t2.idservicio INNER JOIN  cpersonal as t3 On t1.usuariofkcpersonal=t3.idpersona WHERE idmodificacion = '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Servicio":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 650);
                            gbadd.Size = new Size(1231, 350);
                            CenterToParent();
                        }

                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cservicios as t2 ON t1.idregistro=t2.idservicio INNER JOIN cpersonal AS t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE idmodificacion='" + id + "'").ToString().Split(';'));

                        string[] servicios = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(concat(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Servicio' or x4.tipo='Inserción de Servicio') and x4.idregistro=t2.idservicio and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Nombre: ',t2.Nombre,';Descripción: ',t2.Descripcion,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN cservicios as t2 ON t1.idregistro=t2.idservicio INNER JOIN cpersonal AS t3 ON t1.usuariofkcpersonal=t3.idpersona WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');

                        servicios[0] = "".ToUpper() + servicios[0].ToUpper();
                        servicios[1] = ("" + servicios[1]).ToUpper();
                        servicios[2] = ("usuario que modificó: " + servicios[2]).ToUpper();
                        servicios[3] = ("" + servicios[3]).ToUpper();
                        servicios[4] = ("" + servicios[4]).ToUpper();
                        servicios[5] = ("" + servicios[5]).ToUpper();
                        servicios[6] = ("" + servicios[6]).ToUpper();
                        servicios[7] = ("" + servicios[7]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(servicios);
                        break;

                    case "Exportación a PDF de reporte de supervisión":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 600);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.Folio,';',(SELECT concat('ECO: ',ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t2.UnidadfkCUnidades),'; Fecha: ', DATE_FORMAT(t2.FechaReporte, '%W, %d de %M del %Y'),';Supervisor: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t2.SupervisorfkCPersonal),';Conductor: ',(SELECT CONCAT(credencial,': ',nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t2.CredencialConductorfkCPersonal),';Servicio: ',(select concat(nombre,': ',descripcion) FROM cservicios WHERE idservicio=t2.Serviciofkcservicios),';Hora de Entrada: ',t2.HoraEntrada,';Kilometraje: ',t2.KmEntrada,';Tipo de Fallo:',t2.TipoFallo,if((SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo) is null, Concat(';Fallo No Codificado: ',DescFalloNoCod), CONCAT(';Descripción de Fallo: ',(SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo),';Fallo: ',(select concat(codfallo,' - ',falloesp) FROM cfallosesp WHERE idfalloesp=t2.CodFallofkcfallosesp))),';Observaciones: ',coalesce(if(LENGTH(t2.ObservacionesSupervision)>=25,CONCAT(SUBSTRING(t2.ObservacionesSupervision,1,25),'...'),ObservacionesSupervision),'null'),';Tipo: " + tipo.ToUpper() + "' )) FROM modificaciones_sistema as t1 INNER join reportesupervicion as t2 on t1.idregistro=t2.idreportesupervicion WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Exportación a Excel de reportes de supervisión":
                    case "Exportación a Excel de reportes de almacén":
                    case "Exportación a Excel de reportes de mantenimiento":
                    case "Exportación a Excel de ordenes de compra":
                    case "Exportación a Excel de Catálogo de Unidades":
                    case "Exportación a Excel de Registro de Personal":
                    case "Exportación a Excel de Catálogo de Proveedores":
                    case "Exportación a Excel de Catálogo de Refacciones":
                        string texto = "";
                        if (tipo == "Exportación a Excel de Catálogo de Unidades")
                        {
                            sqlFolio = "SELECT UPPER(CONCAT(t2.identificador, LPAD(t1.consecutivo, 4, '0'))) FROM cunidades AS t1 INNER JOIN careas AS t2 ON t1.areafkcareas = t2.idarea WHERE t1.idUnidad = ";
                            texto = "ECONÓMICOS";
                        }
                        else if (tipo == "Exportación a Excel de Registro de Personal")
                        {
                            sqlFolio = "SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idPersona = ";
                            texto = "Empleados";
                        }
                        else if (tipo == "Exportación a Excel de Catálogo de Proveedores")
                        {
                            sqlFolio = "SELECT UPPER(empresa) FROM cproveedores WHERE idproveedor = ";
                            texto = "Proveedores";
                        }
                        else if (tipo == "Exportación a Excel de Catálogo de Refacciones")
                        {
                            sqlFolio = "SELECT nombreRefaccion FROM crefacciones WHERE idrefaccion  = ";
                            texto = "REFACCIONES";
                        }
                        else if (tipo != "Exportación a Excel de ordenes de compra")
                        {
                            sqlFolio = "SELECT folio FROM reportesupervicion WHERE idReporteSupervicion= ";
                            texto = "REPORTES";
                        }

                        else
                        {
                            sqlFolio = "SELECT FolioOrdCompra FROM ordencompra where idOrdCompra=";
                            texto = "REQUISICIONES";
                        }
                        crearListBox(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Usuario que Exportó: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),'^Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%m:%s'), '^Tipo: ', Tipo, '^', ultimaModificacion)) FROM modificaciones_sistema WHERE idmodificacion = '" + id + "'; ").ToString().Split('^'), texto);
                        sqlFolio = null;
                        break;

                    case "Actualización de Reporte de Supervisión":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1295, 950))
                        {
                            this.Size = new Size(1295, 950);
                            gbadd.Size = new Size(1231, 650);
                        }
                        else this.Size = new Size(1295, 680);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('motivo de actualización: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Folio: ',t2.folio))FROM modificaciones_sistema as t1 INNER JOIN reportesupervicion as t2 ON t1.idregistro=t2.idreportesupervicion where t1.idmodificacion='" + id + "'").ToString().Split(';'));

                        string[] reporte = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,coalesce((select concat(';Persona Que Modificó: ',x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/Hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Actualización de Reporte de Supervisión' and x4.idregistro=t2.idreportesupervicion and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),(concat(';Usuario que inserto: ',(select concat(p1.appaterno,' ',p1.apmaterno,' ',p1.nombres) from cpersonal as p1 where p1.idpersona=t2.SupervisorfkCPersonal),';Fecha/Hora: ',date_format(t2.FechaReporte,'%W, %d de %M del %Y'),'/',(t2.HoraEntrada)))),';Unidad: ',(select concat(x11.identificador,LPAD(x10.consecutivo,4,'0')) from cunidades as x10 inner join careas as x11 on x11.idarea=x10.areafkcareas where x10.idunidad=t2.UnidadfkCUnidades),';Supervisor: ',(select concat(x1.apPaterno,' ',x1.apMaterno,' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t2.SupervisorfkCPersonal),';credencial de conductor: ',t3.credencial,';Servicio: ',(select if(t2.Serviciofkcservicios='1','SIN SERVICIO',(select upper(concat(x13.Nombre,' ',x13.Descripcion)) from cservicios as x13 where x13.idservicio=t2.Serviciofkcservicios))),';Kilometraje: ',t2.KmEntrada,';tipo de fallo: ',if(t2.TipoFallo='1','correctivo',(if(t2.tipofallo='2','preventivo',(if(t2.tipofallo='3','reiterativo',(if(t2.tipofallo='4','reprogramado','Seguimiento'))))))),if(t2.DescrFallofkcdescfallo is null,concat(';Fallo no códificado: ',t2.DescFalloNoCod),concat(';Descripción: ',(select descfallo from cdescfallo as x14 where x14.iddescfallo=t2.DescrFallofkcdescfallo),';código: ',(select codfallo from cfallosesp as x15 where x15.idfalloEsp=t2.CodFallofkcfallosesp))),'; Observaciones: ',t2.ObservacionesSupervision,';Persona Que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora)))FROM modificaciones_sistema as t1 INNER JOIN reportesupervicion as t2 ON t1.idregistro=t2.idreportesupervicion inner join cpersonal as t3 on t3.idpersona=t2.CredencialConductorfkCPersonal where t1.idmodificacion='" + id + "';").ToString().Split(';');
                        reporte[0] = ("unidad: " + v.getaData("select concat(x11.identificador,LPAD(x10.consecutivo,4,'0')) from cunidades as x10 inner join careas as x11 on x11.idarea=x10.areafkcareas where x10.idunidad='" + reporte[0] + "';")).ToUpper();
                        reporte[1] = ("SUpervisor: " + v.getaData("select concat(appaterno,' ',apmaterno,' ',nombres) from cpersonal where idpersona='" + reporte[1] + "';")).ToUpper();
                        reporte[2] = ("credencial de conductor: " + v.getaData("select credencial from cpersonal where idpersona='" + reporte[2] + "'")).ToUpper();
                        reporte[3] = ("Servicio: " + v.getaData("select concat(nombre,' ', descripcion) from cservicios where idservicio='" + reporte[3] + "';")).ToUpper();
                        reporte[4] = ("Kilometraje: " + reporte[4]).ToUpper();
                        reporte[5] = ("Tipo de fallo: " + (reporte[5] == "1" ? "correctivo" : reporte[5] == "2" ? "preventivo" : reporte[5] == "3" ? "reiterativo" : reporte[5] == "4" ? "reprogramado" : "seguimiento")).ToUpper();
                        if (reporte.Length == 22)
                        {
                            reporte[6] = ("Descripción: " + v.getaData("select descfallo from cdescfallo where iddescfallo='" + reporte[6] + "';")).ToUpper();
                            reporte[7] = ("Código: " + v.getaData("select codfallo from cfallosesp where idfalloEsp='" + reporte[7] + "';")).ToUpper();
                            reporte[8] = ("Observaciones: " + reporte[8]).ToUpper();
                            reporte[9] = ("" + reporte[9]).ToUpper();
                            reporte[10] = ("" + reporte[10]).ToUpper();
                            reporte[11] = ("" + reporte[11]).ToUpper();
                            reporte[12] = ("" + reporte[12]).ToUpper();
                            reporte[13] = ("" + reporte[13]).ToUpper();
                            reporte[14] = ("" + reporte[14]).ToUpper();
                            reporte[15] = ("" + reporte[15]).ToUpper();
                            reporte[16] = ("" + reporte[16]).ToUpper();
                            reporte[17] = ("" + reporte[17]).ToUpper();
                            reporte[18] = ("" + reporte[18]).ToUpper();
                            reporte[19] = ("" + reporte[19]).ToUpper();
                            reporte[20] = ("" + reporte[20]).ToUpper();
                            reporte[21] = ("" + reporte[21]).ToUpper();
                        }
                        else
                        {
                            reporte[6] = ("fallo no codificado: " + reporte[6]).ToUpper();
                            reporte[7] = ("obervaciones: " + reporte[7]).ToUpper();
                            reporte[8] = ("" + reporte[8]).ToUpper();
                            reporte[9] = ("" + reporte[9]).ToUpper();
                            reporte[10] = ("" + reporte[10]).ToUpper();
                            reporte[11] = ("" + reporte[11]).ToUpper();
                            reporte[12] = ("" + reporte[12]).ToUpper();
                            reporte[13] = ("" + reporte[13]).ToUpper();
                            reporte[14] = ("" + reporte[14]).ToUpper();
                            reporte[15] = ("" + reporte[15]).ToUpper();
                            reporte[16] = ("" + reporte[16]).ToUpper();
                            reporte[17] = ("" + reporte[17]).ToUpper();
                            reporte[18] = ("" + reporte[18]).ToUpper();
                            reporte[19] = ("" + reporte[19]).ToUpper();
                        }
                        y1 = 165;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(reporte);
                        CenterToParent();
                        break;
                    case "Desactivación de Clasificación":
                    case "Reactivación de Clasificación":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';Clasificación: ',t2.nombreFalloGral,';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cfallosgrales as t2 on t2.idFalloGral=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _clasi = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Clasificación','Usuario que activo: ',if(x4.tipo='Reactivación de Clasificación','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Clasificación','Activación de Clasificación',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Clasificación') or (x4.Tipo='Reactivación de Clasificación') or (x4.Tipo='Inserción de Clasificación')) and (x4.idregistro=t2.idFalloGral) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Clasificación',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo))FROM modificaciones_sistema as t1 INNER join cfallosgrales as t2 on t2.idFalloGral=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_clasi);
                        break;
                    case "Inserción de Clasificación":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Clasificación de Fallo: ',t2.nombreFalloGral,';Usuario que Inserta: ',(SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ', DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo, ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) AS M FROM sistrefaccmant.modificaciones_sistema AS t1 INNER JOIN cfallosgrales AS t2 ON t1.idregistro = t2.idfallogral WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        break;

                    case "Actualización de Clasificación":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo, ';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'))) AS M FROM sistrefaccmant.modificaciones_sistema AS t1 INNER JOIN cfallosgrales AS t2 ON t1.idregistro = t2.idfallogral WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] clas = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Clasificación' or x4.Tipo='Inserción de Clasificación') and x4.idregistro=t2.idfallogral and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Clasificación: ',t2.nombreFalloGral,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) AS M FROM sistrefaccmant.modificaciones_sistema AS t1 INNER JOIN cfallosgrales AS t2 ON t1.idregistro = t2.idfallogral WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';');
                        clas[0] = ("Clasificación: " + clas[0]).ToUpper();
                        clas[1] = ("usuario que modificó: " + clas[1]).ToUpper();
                        clas[2] = ("" + clas[2]).ToUpper();
                        clas[3] = ("" + clas[3]).ToUpper();
                        clas[4] = ("" + clas[4]).ToUpper();
                        clas[5] = ("" + clas[5]).ToUpper();
                        y1 = 175;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(clas);
                        break;
                    case "Desactivación de Descripción":
                    case "Reactivación de Descripción":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';Clasificación: ',(select x1.nombreFalloGral from cfallosgrales as x1 inner join cdescfallo as x2 on x1.idFalloGral=x2.falloGralfkcfallosgrales where x2.iddescfallo=t1.idregistro),' - Descripción: ',t2.descfallo),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cdescfallo as t2 on t1.idregistro=t2.iddescfallo WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _descripcion = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Descripción','Usuario que activo: ',if(x4.tipo='Reactivación de Descripción','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Descripción','Activación de Descripción',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Descripción') or (x4.Tipo='Reactivación de Descripción') or (x4.Tipo='Inserción de Descripción')) and (x4.idregistro=t2.iddescfallo) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Descripción',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo))FROM modificaciones_sistema as t1 INNER join cdescfallo as t2 on t1.idregistro=t2.iddescfallo WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_descripcion);
                        break;
                    case "Inserción de Descripción":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Clasificación de Fallo: ',(SELECT nombreFalloGral from cfallosgrales WHERE idfallogral = t2.falloGralfkcfallosgrales),';Descripción de Fallo: ',t2.descfallo,';Usuario que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Estatus AcTual: ',if (t2.status = 1,'Activo','Inactivo')))  FROM modificaciones_sistema as t1 INNER JOIN cdescfallo as t2 On t1.idregistro=t2.iddescfallo WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Descripción de Fallo":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ', t1.Tipo, ';Estatus Acual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cdescfallo as t2 On t1.idregistro = t2.iddescfallo WHERE idmodificacion = '" + id + "'; ").ToString().Split(';'));

                        string[] descfallo = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Descripción de Fallo' or x4.Tipo='Inserción de Descripción') and x4.idregistro=t2.iddescfallo and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Clasificación: ',(select nombreFalloGral from cfallosgrales as x1 where t2.falloGralfkcfallosgrales=x1.idFalloGral),';Descripción: ',t2.descfallo,';usuario que modificó: ',(SELECT concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) FROM cpersonal AS X2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN cdescfallo as t2 On t1.idregistro = t2.iddescfallo WHERE idmodificacion = '" + id + "';").ToString().Split(';');

                        descfallo[0] = ("Clasificación: " + v.getaData("select nombreFalloGral from cfallosgrales where idFalloGral='" + descfallo[0] + "';")).ToUpper();
                        descfallo[1] = ("Descripción: " + descfallo[1]).ToUpper();
                        descfallo[2] = ("usuario que modificó: " + descfallo[2]).ToUpper(); ;
                        descfallo[3] = ("" + descfallo[3]).ToUpper();
                        descfallo[4] = ("" + descfallo[4]).ToUpper();
                        descfallo[5] = ("" + descfallo[5]).ToUpper();
                        descfallo[6] = ("" + descfallo[6]).ToUpper();
                        descfallo[7] = ("" + descfallo[7]).ToUpper();
                        y1 = 150;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(descfallo);
                        break;
                    case "Desactivación de Nombre de Fallo":
                    case "Reactivación de Nombre de Fallo":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';Clasificación: ',(select x1.nombreFalloGral from cfallosgrales as x1 inner join cdescfallo as x2 on x1.idFalloGral=x2.falloGralfkcfallosgrales inner join cfallosesp as x3 on x2.iddescfallo=x3.descfallofkcdescfallo where x3.idfalloEsp=t1.idregistro),' - descripción: ',(select x4.descfallo from cdescfallo as x4 inner join cfallosesp as x5 on x4.iddescfallo=x5.descfallofkcdescfallo where x5.idfalloEsp=t1.idregistro),' - Nombre: ',t2.falloesp,' - Código: ',t2.codfallo),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cfallosesp as t2 on t1.idregistro=t2.idfalloEsp WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _nombrefallo = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Nombre de Fallo','Usuario que activo: ',if(x4.tipo='Reactivación de Nombre de Fallo','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Nombre de Fallo','Activación de Nombre de Fallo',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Nombre de Fallo') or (x4.Tipo='Reactivación de Nombre de Fallo') or (x4.Tipo='Inserción de Nombre de Fallo')) and (x4.idregistro=t2.idfalloEsp) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Nombre de Fallo',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cfallosesp as t2 on t1.idregistro=t2.idfalloEsp WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_nombrefallo);
                        break;
                    case "Inserción de Nombre de Fallo":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 715);
                            gbadd.Size = new Size(1231, 415);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT('Clasificación de Fallo: ', (SELECT CONCAT((SELECT nombrefallogral From cfallosgrales where idfallogral = x1.fallogralfkcfallosgrales), ';Descripción de Fallo: ', x1.descfallo) FROM cdescfallo as x1 WHERE x1.iddescfallo = t2.descfallofkcdescfallo),';Código de Fallo: ',t2.codfallo,'; Nombre de Fallo: ',falloesp,';Usuario que Modifica: ',(SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t2.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora, '%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo,';Estatus AcTual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cfallosesp as t2 On t1.idregistro = t2.idfalloesp WHERE idmodificacion = '" + id + "'").ToString().Split(';'));
                        break;
                    case "Actualización de Nivel":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ', t1.Tipo, ';Estatus Acual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema  as t1 INNER JOIN cniveles as t2 On t1.idregistro = t2.idnivel WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] nivel = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Nivel' or x4.Tipo='Inserción de Nivel') and x4.idregistro=t2.idnivel and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Pasillo: ',(select pasillo from cpasillos as x1 where x1.idpasillo=t2.pasillofkcpasillos),';Nivel: ',t2.nivel,';usario que modifico: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema  as t1 INNER JOIN cniveles as t2 On t1.idregistro = t2.idnivel WHERE t1.idmodificacion='" + id + "';").ToString().Split(';');

                        nivel[0] = ("Pasillo: " + v.getaData("select pasillo from cpasillos where idpasillo='" + nivel[0] + "';")).ToUpper();
                        nivel[1] = ("Nivel: " + nivel[1]).ToUpper();
                        nivel[2] = ("usuario que modificó: " + nivel[2]).ToUpper();
                        nivel[3] = ("" + nivel[3]).ToUpper();
                        nivel[4] = ("" + nivel[4]).ToUpper();
                        nivel[5] = ("" + nivel[5]).ToUpper();
                        nivel[6] = ("" + nivel[6]).ToUpper();
                        nivel[7] = ("" + nivel[7]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(nivel);
                        break;
                    case "Actualización de Nombre de Fallo":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 715);
                            gbadd.Size = new Size(1231, 415);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT('Motivo de modificación: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Estatus AcTual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema as t1 INNER JOIN cfallosesp as t2 On t1.idregistro= t2.idfalloesp WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] nombres = v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Nombre de Fallo' or x4.Tipo='Inserción de Nombre de Fallo') and x4.idregistro=t2.idfalloesp and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Descripción de fallo: ',(select x1.descfallo from cdescfallo as x1 where x1.iddescfallo=t2.descfallofkcdescfallo),';Código de fallo: ',t2.codfallo,';Nombre de fallo: ',t2.falloesp,';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN cfallosesp as t2 On t1.idregistro= t2.idfalloesp WHERE idmodificacion='" + id + "';").ToString().Split(';');

                        nombres[0] = ("Descripción de fallo: " + v.getaData("select descfallo from cdescfallo where iddescfallo = '" + nombres[0] + "'")).ToUpper();
                        nombres[1] = ("Código de fallo: " + nombres[1]).ToUpper();
                        nombres[2] = ("Nombre de fallo: " + nombres[2]).ToUpper();
                        nombres[3] = ("usuario que modificó: " + nombres[3]).ToUpper();
                        nombres[4] = ("" + nombres[4]).ToUpper();
                        nombres[5] = ("" + nombres[5]).ToUpper();
                        nombres[6] = ("" + nombres[6]).ToUpper();
                        nombres[7] = ("" + nombres[7]).ToUpper();
                        nombres[8] = ("" + nombres[8]).ToUpper();
                        nombres[9] = ("" + nombres[9]).ToUpper();
                        y1 = 190;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(nombres);
                        break;
                    case "Desactivación de Proveedor":
                    case "Reactivación de Proveedor":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';Proveedor: ',t2.empresa,';Tipo: ', t1.Tipo))FROM modificaciones_sistema as t1 INNER join cproveedores as t2 on t2.idproveedor=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _proveedor = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Proveedor','Usuario que activo: ',if(x4.tipo='Reactivación de Proveedor','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Proveedor','Activación de Proveedor',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Proveedor') or (x4.Tipo='Reactivación de Proveedor') or (x4.Tipo='Inserción de Proveedor')) and (x4.idregistro=t2.idproveedor) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Proveedor',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cproveedores as t2 on t2.idproveedor=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_proveedor);
                        break;
                    case "Inserción de Proveedor":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 600);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SELECT CONCAT('EMPRESA: ',upper(t2.empresa),';PAGINA WEB: ',COALESCE(t2.paginaweb,'SIN PAGINA WEB'),';CLASIFICACIÓN: ',UPPER(COALESCE(((select COALESCE(giro,'') from cgiros as x1 where x1.idgiro=t2.Clasificacionfkcgiros)),'\"SIN CLASIFICACION\"')), UPPER(';Telefonos de empresa: '),coalesce(concat(if(t2.idlada is null,concat(t2.lada1,' ',t2.telefonoEmpresaUno,if(t2.ext1 is null,'',concat(' Ext. ',t2.ext1))),coalesce((select concat('+',x1.clave,' ',t2.telefonoEmpresaUno,if(t2.ext1 is null,'',concat(' Ext. ',t2.ext1))) from ladanac as x1 where x1.idLadaNac=t2.idlada),'')),' ',coalesce(concat(if(t2.idladados is null,concat(t2.lada2,' ',t2.TelefonoEmpresaDos,if(t2.ext2 is null,'',concat(' Ext. ',t2.ext2))),coalesce((select concat('+',x1.clave,' ',t2.TelefonoEmpresaDos,if(t2.ext2 is null,'',concat(' Ext. ',t2.ext2))) from ladanac as x1 where t2.idladados=x1.idLadaNac),''))),'')),'\"SIN TELÉFONOS\"'),  UPPER(';Observaciones: '),(select coalesce(t2.observaciones,'\"SIN OBSERVACIONES\"')),UPPER(';Domicilio: '), UPPER(coalesce((select concat('Calle: ', t2.calle, ', Número: ', t2.Numero, ', ', x2.tipo, ' ', x2.asentamiento, ', ', x2.municipio, ', ', x2.estado, '. C. P. ', x2.cp) from sepomex as x2 where x2.id=t2.domiciliofksepomex ),'\"SIN DOMICILIO\"')), UPPER(';Persona de contacto: '),coalesce( UPPER(concat(t2.aPaterno,' ',t2.aMaterno,' ',t2.nombres)),'\"SIN PERSONA DE CONTACTO\"'),';CORREO ELECTRÓNICO: ',coalesce(t2.correo,'\"SIN CORREO ELECTRÓNICO\"'), UPPER(';Telefonos de contacto: '),coalesce(concat(if(t2.idladatres is null,concat(t2.lada3,' ',t2.TelefonoContactoUno,if(t2.ext3 is null,'',concat(' Ext. ',t2.ext3))),coalesce((select concat('+',x1.clave,' ',t2.TelefonoContactoUno,if(t2.ext3 is null,'',concat(' Ext. ',t2.ext3))) from ladanac as x1 where x1.idLadaNac=t2.idladatres),'')),' ',coalesce(concat(if(t2.idladacuatro is null,concat(t2.lada4,' ',t2.TelefonoContactoDos,if(t2.ext4 is null,'',concat(' Ext. ',t2.ext4))),coalesce((select concat('+',x1.clave,' ',t2.TelefonoContactoDos,if(t2.ext4 is null,'',concat(' Ext. ',t2.ext4))) from ladanac as x1 where t2.idladacuatro=x1.idLadaNac),''))),'')),'\"SIN TELÉFONOS\"'), UPPER(';Estatus Actual: '),if (t2.status = 1, UPPER('Activo'), UPPER('Inactivo')),UPPER(CONCAT(';Tipo: ',t1.Tipo))) FROM modificaciones_sistema as t1 INNER JOIN cproveedores as t2 ON t1.idregistro=t2.idproveedor WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;


                    case "Actualización de Proveedor":
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';SELECT UPPER(CONCAT('Motivo de modificación: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,(SELECT CONCAT(';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo'))) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal))) FROM modificaciones_sistema as t1 INNER JOIN cproveedores as t2 ON t1.idregistro=t2.idproveedor WHERE idmodificacion='" + id + "'").ToString().Split(';'));

                        string[] actuprov = v.getaData("SET lc_time_names = 'es_ES'; SELECT CONCAT(t1.ultimaModificacion,';USUARIO QUE MODIFICÓ: ',(select concat(upper(x3.nombres),' ',upper(x3.ApPaterno),' ',upper(x3.apMaterno),';FECHA/HORA: ',upper(date_format(x4.fechaHora,'%W, %d de %M del %Y')),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Proveedor' or x4.Tipo='Inserción de Proveedor') and x4.idregistro=t2.idproveedor and(x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),UPPER(CONCAT(';Empresa: ', t2.empresa)), concat(';PÁGINA WEB: ',coalesce(t2.paginaweb,'')), upper(concat(';clasificación: ',coalesce((select giro from cgiros as x1 where x1.idgiro = t2.Clasificacionfkcgiros),''), ';Telefonos de empresa: ',coalesce(concat(if(t2.idlada is null,concat('(+',t2.lada1,')',t2.telefonoEmpresaUno,if(t2.ext1 is null,'',concat(' Ext. ',t2.ext1))),coalesce((select concat('(+',x1.clave,')',t2.telefonoEmpresaUno,if(t2.ext1 is null,'',concat(' Ext. ',t2.ext1))) from ladanac as x1 where x1.idLadaNac=t2.idlada),'')),' ',coalesce(concat(if(t2.idladados is null,concat('(+',t2.lada2,')',t2.TelefonoEmpresaDos,if(t2.ext2 is null,'',concat(' Ext. ',t2.ext2))),coalesce((select concat('(+',x1.clave,')',t2.TelefonoEmpresaDos,if(t2.ext2 is null,'',concat(' Ext. ',t2.ext2))) from ladanac as x1 where t2.idladados=x1.idLadaNac),''))),'')),''),';Observaciones: ',coalesce(t2.observaciones, ''),';Domicilio:',coalesce((select concat('Calle: ', t2.calle, ', Número: ', t2.Numero, ', ', x2.tipo, ' ', x2.asentamiento, ', ', x2.municipio, ', ', x2.estado, '. C. P. ', x2.cp) from sepomex as x2 where x2.id = t2.domiciliofksepomex ),''))),';PERSONA DE CONTACTO: ',upper(coalesce(concat(t2.nombres, ' ', t2.apaterno, ' ', t2.aMaterno),'')),concat(';CORREO ELECTRÓNICO: ', coalesce(t2.correo, '')),';USUARIO QUE MODIFICÓ: ',(select concat(upper(x2.nombres),' ',upper(x2.apPaterno),' ',upper(x2.apMaterno)) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';FECHA/HORA: ',upper(date_format(t1.fechaHora,'%W, %d de %M del %Y')),'/',time(t1.fechahora)) AS cs FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cproveedores as t2 On T1.idregistro = t2.idproveedor WHERE idmodificacion = '" + id + "'; ").ToString().Split(';');
                        actuprov[0] = UPPER("EMPRESA: " + actuprov[0]);
                        actuprov[1] = UPPER("Pagina Web: ") + actuprov[1];
                        actuprov[2] = UPPER("Clasificación: " + v.getaData("SELECT giro FROM cgiros WHERE idgiro='" + actuprov[2] + "'"));
                        actuprov[3] = UPPER("Teléfono: " + actuprov[3]);
                        actuprov[4] = UPPER("Observaciones: " + actuprov[4]);
                        actuprov[5] = UPPER("Domicilio: " + actuprov[5]);
                        actuprov[6] = UPPER("Persona de Contacto: " + actuprov[6]);
                        actuprov[7] = UPPER("Correo Electrónico: ") + actuprov[7];
                        this.Size = new Size(1300, 920);
                        gbadd.Size = new Size(1270, 630);
                        CenterToParent();
                        y1 = 165;
                        CrearAntesDespuesLabels();
                        y1 = 165;
                        mitad1mitad2(actuprov);
                        y1 = null;
                        break;
                    case "Desactivación de Refacción":
                    case "Reactivación de Refacción":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 650); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';código: ',t2.codrefaccion,' - nombre: ',t2.nombreRefaccion,' - modelo: ',t2.modeloRefaccion),';Tipo: ', t1.Tipo))FROM modificaciones_sistema as t1 INNER join crefacciones as t2 on t2.idrefaccion=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _refaccion = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Refacción','Usuario que activo: ',if(x4.tipo='Reactivación de Refacción','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Refacción','Activación de Refacción',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Refacción') or (x4.Tipo='Reactivación de Refacción') or (x4.Tipo='Inserción de Refacción')) and (x4.idregistro=t2.idrefaccion) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Refacción',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join crefacciones as t2 on t2.idrefaccion=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_refaccion);
                        break;

                    case "Inserción de Refacción":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 750); CenterToParent(); acomodarLabel();
                        }
                        string[] ress = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Usuario Que Inserta: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ', DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from crefacciones WHERE idrefaccion=idregistro),';Tipo: ',Tipo)) FROM modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        ress[0] = ("Código de Refacción: " + ress[0]).ToUpper();
                        ress[1] = ("Nombre de Refacción: " + ress[1]).ToUpper();
                        ress[2] = ("Modelo de Refacción: " + ress[2]).ToUpper();
                        ress[3] = ("Próximo Abastecimiento: " + DateTime.Parse(ress[3]).ToString("dddd, dd MMMM yyyy")).ToUpper();
                        ress[4] = ("Familia: " + v.getaData("select t1.Familia from cnfamilias as t1 inner join cfamilias as t2 on t2.familiafkcnfamilias=t1.idcnFamilia inner join cmarcas as t3 on t3.descripcionfkcfamilias=t2.idfamilia where t3.idmarca='" + ress[4] + "'") + " - Descripción: " + v.getaData("select t1.descripcionFamilia from cfamilias as t1 inner join cmarcas as t2 on t2.descripcionfkcfamilias=t1.idfamilia where t2.idmarca='" + ress[4] + "'") + " - UM: " + v.getaData("select concat(t1.nombre,' ( ',t1.Simbolo,' )') from cunidadmedida as t1 inner join cfamilias as t2 on t2.umfkcunidadmedida=t1.idunidadmedida inner join cmarcas as t3 on t3.descripcionfkcfamilias=t2.idfamilia where t3.idmarca='" + ress[4] + "'") + " - Marca: " + v.getaData("select marca from cmarcas where idmarca='" + ress[4] + "'")).ToUpper();
                        ress[5] = ("Ubicación: " + v.getaData("SELECT CONCAT((SELECT CONCAT((SELECT CONCAT((SELECT CONCAT('Pasillo: ',t4.pasillo)),' - Nivel:',t3.nivel)),' - Anaquel: ',t2.anaquel)),' - Charola: ',t1.charola) FROM ccharolas as t1 inner join canaqueles as t2 on t2.idanaquel=t1.anaquelfkcanaqueles inner join cniveles as t3 on t2.nivelfkcniveles=t3.idnivel inner join cpasillos as t4 on t3.pasillofkcpasillos=t4.idpasillo WHERE idcharola='" + ress[5] + "'")).ToUpper();
                        ress[6] = ("Cantidad Que Ingresó a Almacén: " + ress[6]).ToUpper();
                        ress[7] = "NOTIFICACIÓN DE MEDIA: " + ress[7];
                        ress[8] = "NOTIFICACIÓN DE ABASTECIMIENTO: " + ress[8];
                        if (ress[9] == "") ress[9] = null;
                        ress[9] = ("Observaciones de Refacción: " + (ress[9] ?? "\"Sin observaciones\"")).ToUpper();
                        crearCatalogoPuesto(ress);
                        break;

                    case "Actualización de Refacción":

                        this.Size = new Size(1296, 1005); CenterToParent(); acomodarLabel();
                        gbadd.Size = new Size(1231, 725);
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ', t1.Tipo, ';Estatus Acual: ',if (t2.status = 1,'Activo','Inactivo'))) FROM modificaciones_sistema  as t1 INNER JOIN crefacciones as t2 On t1.idregistro = t2.idrefaccion WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] resul = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';usuario que modificó: ',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Refacción' or x4.Tipo='Inserción de Refacción') and x4.idregistro=t2.idrefaccion and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Código De Refacción: ', t2.codrefaccion, ';Nombre De La Refacción: ', t2.nombreRefaccion, ';Modelo De La Refacción: ', t2.modeloRefaccion, ';Próximo Abastecimiento: ', DATE_FORMAT(t2.proximoAbastecimiento, '%W, %d de %M del %Y'), /*';Familia: ', t5.Familia,*/ ';Marca: ', t3.Marca, ';Ubicación: ', CONCAT('Pasillo: ', t10.pasillo, ', Nivel: ', t9.nivel, ', Anaquel: ', t8.anaquel, ', Charola: ', t7.charola), ';Notificación De Media: ', t2.media, ';Notificación De Abastecimiento: ', t2.abastecimiento, ';Descripción De Refacción: ', COALESCE(t2.descripcionRefaccion, '\"Sin Descripción\"'),';usuario que modicó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';Fecha/hora:',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema AS t1 INNER JOIN crefacciones AS t2 ON t1.idregistro = t2.idrefaccion INNER JOIN cmarcas AS t3 ON t2.marcafkcmarcas = t3.idmarca INNER JOIN cfamilias AS t4 ON t3.descripcionfkcfamilias = t4.idfamilia INNER JOIN cnfamilias AS t5 ON t4.familiafkcnfamilias = t5.idcnfamilia INNER JOIN cunidadmedida AS t6 ON t6.idunidadmedida = t4.umfkcunidadmedida INNER JOIN ccharolas AS t7 ON t7.idcharola = t2.charolafkcharolas INNER JOIN canaqueles AS t8 ON t7.anaquelfkcanaqueles = t8.idanaquel INNER JOIN cniveles AS t9 ON t8.nivelfkcniveles = t9.idnivel INNER JOIN cpasillos AS t10 ON t9.pasillofkcpasillos = t10.idpasillo WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';');

                        resul[0] = ("Código De Refacción: " + resul[0]).ToUpper();
                        resul[1] = ("Nombre De La Refacción: " + resul[1]).ToUpper();
                        resul[2] = ("Modelo De La Refacción: " + resul[2]).ToUpper();
                        resul[3] = ("Próximo Abastecimiento: " + DateTime.Parse(resul[3]).ToString("dddd, dd" + " D" + "e MMMM " + 'D' + "el yyyy")).ToUpper();
                        resul[4] = ("Marca: " + v.getaData("SELECT marca FROM cmarcas WHERE idmarca = '" + resul[4] + "'")).ToUpper();
                        resul[5] = ("Ubicación: " + v.getaData("SELECT CONCAT('Pasillo: ', t4.pasillo, ', Nivel: ', t3.nivel, ', Anaquel : ', t2.anaquel, ', Charola: ', t1.charola) FROM ccharolas AS t1 INNER JOIN canaqueles AS t2 ON t1.anaquelfkcanaqueles = t2.idanaquel INNER JOIN cniveles AS t3 ON t2.nivelfkcniveles = t3.idnivel INNER JOIN cpasillos AS t4 ON t3.pasillofkcpasillos = t4.idpasillo WHERE t1.idcharola = '" + resul[5] + "'")).ToUpper();
                        resul[6] = ("Notificación De Media: " + resul[6]).ToUpper();
                        resul[7] = ("Notificación De Abastecimiento: " + resul[7]).ToUpper();
                        resul[8] = ("Descripción De Refacción: " + resul[8]).ToUpper();
                        y1 = 170;
                        mitad1mitad2(resul);
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        y1 = null;
                        break;

                    case "Desactivación de Marca":
                    case "Reactivación de Marca":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }

                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';familia: ',(select x1.familia from cnfamilias as x1 inner join cfamilias as x2 on x1.idcnFamilia=x2.familiafkcnfamilias inner join cmarcas as x3 on x3.descripcionfkcfamilias=x2.idfamilia where x3.idmarca=t1.idregistro),' - Descripcion: ',(select x4.descripcionFamilia from cfamilias as x4 inner join cmarcas as x5 on x5.descripcionfkcfamilias=x4.idfamilia where x5.idmarca=t1.idregistro),' - Marca: ',t2.marca),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cmarcas as t2 on t1.idregistro=t2.idmarca WHERE idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] _marca = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Marca','Usuario que activo: ',if(x4.tipo='Reactivación de Marca','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Marca','Activación de Marca',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Marca') or (x4.Tipo='Reactivación de Marca') or (x4.Tipo='Inserción de Marca')) and (x4.idregistro=t2.idmarca) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Marca',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cmarcas as t2 on t1.idregistro=t2.idmarca WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_marca);
                        break;
                    case "Inserción de Marca":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 705);
                            gbadd.Size = new Size(1231, 405);
                            CenterToParent();
                        }
                        string[] res = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(UltimaModificacion,';Usuario que Inserta: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from cmarcas WHERE idmarca=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        string[] res3 = res[0].Split(',');
                        res3[0] = "DESCRIPCIÓN DE FAMILIA: " + v.getaData("select upper(t1.descripcionFamilia) from cfamilias as t1 inner join cnfamilias as t2 on t2.idcnfamilia=t1.familiafkcnfamilias where idcnfamilia='" + res3[0] + "'").ToString();
                        res[0] = res3[0] + "- MARCA:" + res3[1];
                        crearCatalogoPuesto(res);
                        break;
                    case "Actualización de Marca":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 705);
                            gbadd.Size = new Size(1231, 405);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo')))) FROM modificaciones_sistema as t1 INNER JOIN cmarcas as t2 on t2.idmarca=t1.idregistro WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] marcas = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(';',t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Marca' or x4.Tipo='Inserción de Marca' )and x4.idregistro=t2.idmarca and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';Familia: ',t4.Familia,';Descripción: ',t3.descripcionFamilia,';Marca: ',t2.marca,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN cmarcas as t2 on t2.idmarca=t1.idregistro inner join cfamilias as t3 on t3.idfamilia=t2.descripcionfkcfamilias inner join cnfamilias as t4 on t4.idcnFamilia=t3.familiafkcnfamilias WHERE t1.idmodificacion='" + id + "';").ToString().Split(';');

                        marcas[0] = ("Familia: " + v.getaData("select Familia from cnfamilias as t1 where t1.idcnfamilia='" + marcas[1] + "'")).ToUpper();
                        marcas[1] = ("Descripción: " + v.getaData("select t1.descripcionFamilia from cfamilias as t1 inner join cnfamilias as t2 on t2.idcnfamilia=t1.familiafkcnfamilias where idcnfamilia='" + marcas[1] + "'")).ToUpper(); ;
                        marcas[2] = ("Marca: " + marcas[2]).ToUpper();
                        marcas[3] = ("usuario que modificó: " + marcas[3]).ToUpper();
                        marcas[4] = ("" + marcas[4]).ToUpper();
                        marcas[5] = ("" + marcas[5]).ToUpper();
                        marcas[6] = ("" + marcas[6]).ToUpper();
                        marcas[7] = ("" + marcas[7]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(marcas);
                        break;
                    case "Inserción de Unidad de Medida":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        string[] uno = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(UltimaModificacion,';Usuario que Inserta: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from cunidadmedida WHERE idunidadmedida=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        uno[0] = UPPER("Nombre de Unidad de Medida: " + uno[0]);
                        uno[1] = UPPER("Simbolo: " + uno[1]);
                        crearCatalogoPuesto(uno);
                        break;
                    case "Desactivación de Unidad de Medida":
                    case "Reactivación de Unidad de Medida":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(800, 300); CenterToParent(); acomodarLabel();
                        }

                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';unidad de medida: ',t2.Nombre,' - ',t2.Simbolo),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cunidadmedida as t2 on t1.idregistro=t2.idunidadmedida WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _um = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Unidad de Medida','Usuario que activo: ',if(x4.tipo='Reactivación de Unidad de Medida','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Unidad de Medida','Activación de Unidad de Medida',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Unidad de Medida') or (x4.Tipo='Reactivación de Unidad de Medida') or (x4.Tipo='Inserción de Unidad de Medida')) and (x4.idregistro=t2.idunidadmedida) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Unidad de Medida',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cunidadmedida as t2 on t1.idregistro=t2.idunidadmedida WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        y1 = 180;
                        mitad1mitad2(_um);
                        break;

                    case "Actualización de Unidad de Medida":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('motivo de actualización: ',t1.motivoActualizacion,';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cunidadmedida as t2 ON t1.idregistro=t2.idunidadmedida WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] um = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Unidad de Medida' or x4.Tipo='Inserción de Unidad de Medida') and x4.idregistro=t2.idunidadmedida and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';nombre: ',t2.Nombre,';Simbolo: ',t2.Simbolo,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cunidadmedida as t2 ON t1.idregistro=t2.idunidadmedida WHERE idmodificacion='" + id + "';").ToString().Split(';');

                        um[0] = ("Nombre: " + um[0]).ToUpper();
                        um[1] = ("Simbolo: " + um[1]).ToUpper();
                        um[2] = ("usuario que modifico: " + um[2]).ToUpper();
                        um[3] = ("" + um[3]).ToUpper();
                        um[4] = ("" + um[4]).ToUpper();
                        um[5] = ("" + um[5]).ToUpper();
                        um[6] = ("" + um[6]).ToUpper();
                        um[7] = ("" + um[7]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(um);
                        break;
                    case "Desactivación de Descripción de Familia":
                    case "Reactivación de Descripción de Familia":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';descripción de familia: ',t2.descripcionFamilia,';familia: ',(select familia from cnfamilias as x1 where x1.idcnFamilia=t2.familiafkcnfamilias),';unidad de medida: ',(select concat(x2.Nombre,' - ',x2.Simbolo) from cunidadmedida as x2 where x2.idunidadmedida=t2.umfkcunidadmedida),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cfamilias as t2 on t1.idregistro=t2.idfamilia WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _descfamilia = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Descripción de Familia','Usuario que activo: ',if(x4.tipo='Reactivación de Descripción de Familia','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Descripción de Familia','Activación de Descripción de Familia',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Descripción de Familia') or (x4.Tipo='Reactivación de Descripción de Familia') or (x4.Tipo='Inserción de Descripción de Familia')) and (x4.idregistro=t2.idfamilia) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Descripción de Familia',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cfamilias as t2 on t1.idregistro=t2.idfamilia WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 225;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_descfamilia);
                        break;
                    case "Inserción de Descripción de Familia":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 675);
                            gbadd.Size = new Size(1231, 375);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("select upper(concat('Familia: ',(select Familia from cnfamilias as x1 inner join cfamilias as x2 on x1.idcnFamilia=x2.familiafkcnfamilias where x2.idfamilia=t1.idregistro),';Descripción de familia: ',t2.descripcionFamilia,';unidad de medida: ',(select concat(x1.nombre,' - ',x1.simbolo) from cunidadmedida as x1 inner join cfamilias as x2 on x2.umfkcunidadmedida=x1.idunidadmedida where x2.idfamilia=t1.idregistro),';Usuario que inserto: ',(select concat(x1.nombres,' ',x1.ApPaterno,' ',x1.ApMaterno) from cpersonal as x1 where x1.idPersona=t1.usuariofkcpersonal),';Fecha/hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%m:%s'),';tipo: ',t1.tipo)) from modificaciones_sistema as t1 inner join cfamilias as t2 on t2.idfamilia=t1.idregistro where t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Descripción de Familia":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 675);
                            gbadd.Size = new Size(1231, 375);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo De Modificación: |',coalesce(t1.motivoActualizacion,''), ';Tipo: ', t1.Tipo)) FROM modificaciones_sistema  as t1 INNER JOIN cfamilias as t2 On t1.idregistro = t2.idfamilia WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        string[] descripciones = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.tipo='Actualización de Descripción de Familia' or x4.Tipo='Inserción de Descripción de Familia') and x4.idregistro=t2.idfamilia and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';familia: ',(select Familia from cnfamilias as f1 where f1.idcnFamilia=t2.familiafkcnfamilias),';Descripción : ',t2.descripcionFamilia,';unidad de medida: ',(select concat(um.Nombre,' - ',um.Simbolo) from cunidadmedida as um where um.idunidadmedida=t2.umfkcunidadmedida),';usuario que modificó: ',(select concat(p.nombres,' ',p.apPaterno,' ',p.apMaterno) from cpersonal as p where p.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN cfamilias as t2 On t1.idregistro = t2.idfamilia WHERE t1.idmodificacion='" + id + "';").ToString().Split(';');

                        descripciones[0] = ("Familia: " + v.getaData("select Familia from cnfamilias where idcnFamilia='" + descripciones[0] + "';")).ToUpper();
                        descripciones[1] = ("Descripción: " + descripciones[1]).ToUpper();
                        descripciones[2] = ("Unidad de medida: " + v.getaData("select concat(Nombre,'-',simbolo) from cunidadmedida where idunidadmedida='" + descripciones[2] + "';")).ToUpper();
                        descripciones[3] = ("usuario que modificó: " + descripciones[3]).ToUpper();
                        descripciones[4] = ("" + descripciones[4]).ToUpper();
                        descripciones[5] = ("" + descripciones[5]).ToUpper();
                        descripciones[6] = ("" + descripciones[6]).ToUpper();
                        descripciones[7] = ("" + descripciones[7]).ToUpper();
                        descripciones[8] = ("" + descripciones[8]).ToUpper();
                        descripciones[9] = ("" + descripciones[9]).ToUpper();
                        y1 = 120;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(descripciones);

                        break;
                    case "Actualización de Familia":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 605);
                            gbadd.Size = new Size(1231, 305);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cnfamilias as t2 ON t1.idregistro=t2.idcnfamilia  WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] familia = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Familia' or x4.Tipo='Inserción De Familia de Refacción') and x4.idregistro=t2.idcnfamilia and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';familia: ',t2.Familia,';usuario que modifico: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cnfamilias as t2 ON t1.idregistro=t2.idcnfamilia  WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        familia[0] = ("Familia: " + familia[0]).ToUpper();
                        familia[1] = ("usuario que modificó: " + familia[1]).ToUpper();
                        familia[2] = ("" + familia[2]).ToUpper();
                        familia[3] = ("" + familia[3]).ToUpper();
                        familia[4] = ("" + familia[4]).ToUpper();
                        familia[5] = ("" + familia[5]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(familia);
                        break;
                    case "Desactivación de Pasillo":
                    case "Reactivación de Pasillo":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 650);
                            gbadd.Size = new Size(1231, 350);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';pasillo: ',t2.pasillo,';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cpasillos as t2 on t1.idregistro=t2.idpasillo WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _pasillo = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Pasillo','Usuario que activo: ',if(x4.tipo='Reactivación de Pasillo','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Pasillo','Activación de Pasillo',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Pasillo') or (x4.Tipo='Reactivación de Pasillo') or (x4.Tipo='Inserción de Pasillo')) and (x4.idregistro=t2.idpasillo) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Pasillo',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo))FROM modificaciones_sistema as t1 INNER join cpasillos as t2 on t1.idregistro=t2.idpasillo WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        y1 = 180;
                        mitad1mitad2(_pasillo);
                        break;
                    case "Inserción de Pasillo":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 650);
                            gbadd.Size = new Size(1231, 350);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',UltimaModificacion,';Usuario que Inserta: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from cpasillos WHERE idpasillo=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema WHERE idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Pasillo":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 650);
                            gbadd.Size = new Size(1231, 350);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('motivo de actualización: ',t1.motivoActualizacion,';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',t1.Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cpasillos as t2 ON t1.idregistro = t2.idpasillo WHERE idmodificacion = '" + id + "'; ").ToString().Split(';'));

                        string[] pasillos = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',coalesce((select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Pasillo' or x4.Tipo='Inserción de Pasillo') and x4.idregistro=t2.idpasillo and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select concat(x5.nombres,' ',x5.apPaterno,' ',x5.apMaterno) from cpersonal as x5 where t1.usuariofkcpersonal=x5.idPersona)),';PASIllo: ',t2.pasillo,';usuario que modifico: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cpasillos as t2 ON t1.idregistro = t2.idpasillo WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        pasillos[0] = ("Pasillo: " + pasillos[0]).ToUpper();
                        pasillos[1] = ("usuario que modificó: " + pasillos[1]).ToUpper();
                        pasillos[2] = ("" + pasillos[2]).ToUpper();
                        pasillos[3] = ("" + pasillos[3]).ToUpper();
                        pasillos[4] = ("" + pasillos[4]).ToUpper();
                        pasillos[5] = ("" + pasillos[5]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(pasillos);
                        break;
                    case "Desactivación de Anaquel":
                    case "Reactivación de Anaquel":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ', coalesce(t1.motivoActualizacion, ''), concat(';pasillo: ', (select x1.pasillo from cpasillos as x1 inner join cniveles as x2 on x2.pasillofkcpasillos = x1.idpasillo inner join canaqueles as x3 on x3.nivelfkcniveles = x2.idnivel where x3.idanaquel = t1.idregistro), ' - nivel: ', (select x1.nivel from cniveles as x1 inner join canaqueles as x2 on x2.nivelfkcniveles = x1.idnivel where x2.idanaquel = t1.idregistro), ' - anaquel: ', t2.anaquel), ';Tipo: ', t1.Tipo))FROM modificaciones_sistema as t1 INNER join canaqueles as t2 on t2.idanaquel = t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _anaquel = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Anaquel','Usuario que activo: ',if(x4.tipo='Reactivación de Anaquel','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Anaquel','Activación de Anaquel',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Anaquel') or (x4.Tipo='Reactivación de Anaquel') or (x4.Tipo='Inserción de Anaquel')) and (x4.idregistro=t2.idanaquel) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Anaquel',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join canaqueles as t2 on t2.idanaquel=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_anaquel);
                        break;
                    case "Inserción de Anaquel":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 715);
                            gbadd.Size = new Size(1231, 415);
                            CenterToParent();
                        }
                        string[] cuatro = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',t4.pasillo,' - Nivel: ',t3.nivel,' - Anaquel: ',t2.anaquel,';Usuario que Inserta: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',(SELECT if (status = 1,'Activo','Inactivo') from canaqueles WHERE idanaquel=idregistro),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN canaqueles AS t2 On t1.idregistro=t2.idanaquel INNER JOIN cniveles as t3 ON t2.nivelfkcniveles=t3.idnivel INNER JOIN cpasillos as t4 On t3.pasillofkcpasillos=t4.idpasillo WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        crearCatalogoPuesto(cuatro);
                        break;
                    case "Reactivación de Nivel":
                    case "Desactivación de Nivel":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),concat(';pasillo: ',(select x1.pasillo from cpasillos as x1 inner join cniveles as x2 on x2.pasillofkcpasillos=x1.idpasillo where x2.idnivel=t1.idregistro),' - nivel: ',t2.nivel),';Tipo: ', t1.Tipo))FROM modificaciones_sistema as t1 INNER join cniveles as t2 on t2.idnivel=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _nivel = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Nivel','Usuario que activo: ',if(x4.tipo='Reactivación de Nivel','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Nivel','Activación de Nivel',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Nivel') or (x4.Tipo='Reactivación de Nivel') or (x4.Tipo='Inserción de Nivel')) and (x4.idregistro=t2.idnivel) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Nivel',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cniveles as t2 on t2.idnivel=t1.idregistro where idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_nivel);
                        break;
                    case "Inserción de Nivel":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 655);
                            gbadd.Size = new Size(1231, 355);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',t4.pasillo,' - Nivel: ',t3.nivel,';Usuario que Inserta: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t3.status = 1,'Activo','Inactivo'),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN cniveles AS t3 On t1.idregistro=t3.idnivel INNER JOIN cpasillos as t4 On t3.pasillofkcpasillos=t4.idpasillo WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                        break;
                    case "Actualización de Anaquel":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 715);
                            gbadd.Size = new Size(1231, 415);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo')))) FROM modificaciones_sistema as t1 INNER JOIN canaqueles as t2 ON t1.idregistro=t2.idanaquel WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] anaquel = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('',';',t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Anaquel' or x4.Tipo='Inserción de Anaquel') and x4.idregistro=t2.idanaquel and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';pasillo: ',t4.pasillo,';nivel: ',t3.nivel,';anaquel: ',t2.anaquel,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN canaqueles as t2 ON t1.idregistro=t2.idanaquel inner join cniveles as t3 on t2.nivelfkcniveles=t3.idnivel inner join cpasillos as t4 on t3.pasillofkcpasillos=t4.idpasillo WHERE idmodificacion='" + id + "';").ToString().Split(';');

                        anaquel[0] = ("Pasillo: " + v.getaData("select t1.pasillo from cpasillos as t1 inner join cniveles as t2 on t2.pasillofkcpasillos=t1.idpasillo where idnivel='" + anaquel[1] + "';")).ToUpper();
                        anaquel[1] = ("Nivel: " + v.getaData("select nivel from cniveles where idnivel='" + anaquel[1] + "'")).ToUpper();
                        anaquel[2] = ("Anaquel: " + anaquel[2]).ToUpper();
                        anaquel[3] = ("usuario que módifico: " + anaquel[3]).ToUpper();
                        anaquel[4] = ("" + anaquel[4]).ToUpper();
                        anaquel[5] = ("" + anaquel[5]).ToUpper();
                        anaquel[6] = ("" + anaquel[6]).ToUpper();
                        anaquel[7] = ("" + anaquel[7]).ToUpper();
                        anaquel[8] = ("" + anaquel[8]).ToUpper();
                        anaquel[9] = ("" + anaquel[9]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(anaquel);
                        y1 = null;
                        break;


                    case "Inserción de Ubicación":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 675);
                            gbadd.Size = new Size(1231, 375);
                            CenterToParent();
                        }
                        string[] seis = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Pasillo: ',t5.pasillo,' - Nivel: ',t4.nivel,' - Anaquel: ',t3.anaquel,' - Charola: ',t2.charola,';Usuario que Inserta: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona =t1. usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Estatus Actual: ',if (t2.status = 1,'Activo','Inactivo'),';Tipo: ',Tipo)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ccharolas as t2 ON t1.idregistro=t2.idcharola INNER JOIN canaqueles as t3 ON t2.anaquelfkcanaqueles = t3.idanaquel INNER JOIN cniveles as t4 ON t3.nivelfkcniveles=t4.idnivel INNER JOIN cpasillos as t5 ON  t4.pasillofkcpasillos = t5.idpasillo WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        crearCatalogoPuesto(seis);
                        break;
                    case "Actualización de Ubicación":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 675);
                            gbadd.Size = new Size(1231, 375);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo')))) FROM modificaciones_sistema as t1 INNER JOIN ccharolas as t2 on t2.idcharola=t1.idregistro WHERE idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] siete = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT(t1.UltimaModificacion, ';',(select concat(x3.nombres, ' ', x3.ApPaterno, ' ', x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal = x3.idpersona WHERE (x4.Tipo = 'Actualización de Ubicación' or x4.Tipo='Inserción de Ubicación') and x4.idregistro = t2.idcharola and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1, 1), ';Pasillo - Nivel - Anaquel Actual: ', (SELECT CONCAT((SELECT concat((SELECT pasillo FROM cpasillos WHERE idpasillo = pasillofkcpasillos), ' - ', nivel) FROM cniveles WHERE idnivel = nivelfkcniveles),' - ',anaquel) FROM canaqueles WHERE idanaquel = t2.anaquelfkcanaqueles),';Charola: ',t2.charola,';Usuario que Modificó: ',(SELECT CONCAT(nombres, ' ', apPaterno, ' ', apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ccharolas as t2 On t1.idregistro = t2.idcharola WHERE idmodificacion = '" + id + "'; ").ToString().Split(';');
                        siete[0] = UPPER("Pasillo - NIVEL - Anaquel ANTERIOR: " + v.getaData("SELECT CONCAT((SELECT CONCAT((SELECT pasillo FROM cpasillos WHERE idpasillo = pasillofkcpasillos),' - ',nivel)  FROM cniveles WHERE idnivel = nivelfkcniveles),' - ',anaquel) FROM canaqueles WHERE idanaquel='" + siete[0] + "'"));
                        siete[1] = UPPER("Charola: " + siete[1]);
                        siete[2] = UPPER("usuario que modifió: " + siete[2]);
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(siete);
                        y1 = null;
                        break;
                    case "Desactivación de Ubicación":
                    case "Reactivación de Ubicación":
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';ubicación: ',(select concat('Pasillo: ',x1.pasillo,' - ','Nivel: ',x2.nivel,' - ','Anaquel: ',x3.anaquel,' - ','Charola: ',x4.charola) from cpasillos as x1 inner join cniveles as x2 on x1.idpasillo=x2.pasillofkcpasillos inner join canaqueles as x3 on x2.idnivel=x3.nivelfkcniveles inner join ccharolas as x4 on x3.idanaquel=x4.anaquelfkcanaqueles where x4.idcharola=t1.idregistro),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join ccharolas as t2 on t1.idregistro=t2.idcharola WHERE idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] _ubicacion = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Ubicación','Usuario que activo: ',if(x4.tipo='Reactivación de Ubicación','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Ubicación','Activación de Ubicación',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Ubicación') or (x4.Tipo='Reactivación de Ubicación') or (x4.Tipo='Inserción de Ubicación')) and (x4.idregistro=t2.idcharola) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Ubicación',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join ccharolas as t2 on t1.idregistro=t2.idcharola WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        y1 = 180;
                        mitad1mitad2(_ubicacion);
                        break;
                    case "Inserción de reporte de almacén":
                    case "Validación De Refacciones":
                    case "Exportación a PDF de reporte de almacén":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = (this.Size + new Size(0, 150)); CenterToParent(); acomodarLabel();
                        }
                        else
                        {
                            if (Size.Height < 500)
                            {
                                this.Size = (this.Size + new Size(0, 100));
                                gbadd.Size = (gbadd.Size + new Size(0, 100));
                            }
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',T3.Folio,';UNIDAD: ',(SELECT concat('ECO: ',ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Fecha De Solicitud de Refacción: ',DATE_FORMAT(t4.FechaReporteM,'%W, %d de %M del %Y'),';Mecánico Que Solicita La Refacción: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t4.MecanicofkPersonal),';Folio de Factura Emitido Por Mantenimiento: ',if(t4.FolioFactura ='','Sin Folio de Factura Aún',t4.FolioFactura),';Folio de Factura Emitido Por Almacén: ',t2.FolioFactura,';Fecha de Entrega de Refacción: ',DATE_FORMAT(t2.FechaEntrega,'%W, %d de %M del %Y'),';Persona Que Entregó la Refacción: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.PersonaEntregafkcpersonal),';Observaciones: ',if(t2.ObservacionesTrans='','\"Sin Observaciones\"',t2.ObservacionesTrans)))AS m FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro=t2.idReporteTransinsumos INNER JOIN reportesupervicion as t3 ON t2.idreportemfkreportemantenimiento=t3.idReporteSupervicion INNER JOIN reportemantenimiento as t4 ON t4.FoliofkSupervicion =t3.idReporteSupervicion WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        crearDataGrid((DataTable)v.getData("SELECT(SELECT codrefaccion FROM crefacciones WHERE idrefaccion = t3.RefaccionfkCRefaccion) as 'Código de Refacción', (SELECT nombreRefaccion FROM crefacciones WHERE idrefaccion = t3.RefaccionfkCRefaccion) as 'Nombre de Refacción', t3.Cantidad as 'Cantidad Solicitada', COALESCE(t3.CantidadEntregada,'0') as 'Cantidad Entregada', COALESCE(t3.EstatusRefaccion,'SIN EXISTENCIA') as 'Estatus De Refacción', COALESCE((t3.Cantidad - t3.CantidadEntregada),Cantidad) as 'Cantidad Faltante' FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro = t2.idReporteTransinsumos INNER JOIN pedidosrefaccion as t3 ON t3.FolioPedfkSupervicion = t2.idreportemfkreportemantenimiento WHERE idmodificacion = '" + id + "'; "), new Point(0, 400));
                        break;
                    case "Actualización de Reporte de Almacén":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = (this.Size + new Size(100, 150)); CenterToParent(); acomodarLabel();
                        }
                        else
                        {
                            if (Size.Height < 500)
                            {
                                this.Size = (this.Size + new Size(100, 100));
                                gbadd.Size = (gbadd.Size + new Size(100, 100));
                            }
                            CenterToParent();
                        }
                        string[] nueve = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Persona Que Modificó: ',coalesce((select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/Hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Actualización de Reporte de Almacén' and x4.idregistro=t2.idReporteTransinsumos and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select concat(x5.nombres,' ',x5.apPaterno,' ',x5.apMaterno,';fecha/hora: ',date_format(t3.FechaReporte,'%W, %d de %M del %Y'),'/',t3.HoraEntrada) from cpersonal as x5 where t1.usuariofkcpersonal=x5.idPersona)),';Folio de Factura: ',t2.FolioFactura,';Persona Que Dispenso: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.PersonaEntregafkcpersonal),';Observaciones: ',t2.ObservacionesTrans,';Persona Que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) AS m FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro=t2.idReporteTransinsumos INNER JOIN reportesupervicion as t3 ON t2.idreportemfkreportemantenimiento=t3.idReporteSupervicion INNER JOIN reportemantenimiento as t4 ON t4.FoliofkSupervicion=t3.idReporteSupervicion WHERE idmodificacion = '" + id + "';").ToString().Split(';');

                        string[] diez = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Folio: ',T3.Folio,';UNIDAD: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Fecha de Solicitud de Refaccion: ',DATE_FORMAT(t4.fechaReporteM, '%W, %d de %M del %Y'))) AS m FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro=t2.idReporteTransinsumos INNER JOIN reportesupervicion as t3 ON t2.idreportemfkreportemantenimiento=t3.idReporteSupervicion INNER JOIN reportemantenimiento as t4 ON t4.FoliofkSupervicion =t3.idReporteSupervicion WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        crearCatalogoPuesto(diez);
                        nueve[0] = UPPER("Folio de Factura: " + nueve[0]);
                        nueve[1] = UPPER("Persona que dispenso: " + (v.getaData("(SELECT UPPER(CONCAT(nombres,' ',apPaterno,' ',apMaterno)) FROM cpersonal WHERE idpersona = '" + nueve[1] + "')") ?? nueve[1]));
                        nueve[2] = UPPER("oBSERVACIONES DE SUPervisión: " + nueve[2]);
                        y1 = 200;
                        CrearAntesDespuesLabels();
                        y1 = 250;
                        mitad1mitad2(nueve);
                        y1 = null;
                        break;
                    case "Exportación a PDF de reporte en Mantenimiento":
                        string[] once = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('folio: ',t2.Folio,';Unidad: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t2.UnidadfkCUnidades),';Fecha de Reporte: ',DATE_FORMAT(t2.FechaReporte,'%W, %d de %M del %Y'),';Estatus Del Mantenimiento: ',t1.Estatus,';Código de Fallo: ',(SELECT CONCAT(codfallo,' - ',falloesp) FROM cfallosesp WHERE idfalloesp=t2.CodFallofkcfallosesp),';Fecha de Reporte de Mantenimiento: ',DATE_FORMAT(t1.FechaReporteM,'%W, %d de %M del %Y'),';Mecánico: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.MecanicofkPersonal),';Mecánico de Apoyo: ',if(t1.MecanicoApoyofkPersonal is null,'\"Sin Mecánico de Apoyo\"',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.MecanicofkPersonal)),';Supervisor: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.SupervisorfkCPersonal),';Hora de Entrada: ',t2.HoraEntrada,';Tipo de Fallo: ',t2.TipoFallo,if(t2.DescrFallofkcdescfallo is null,CONCAT(';Descripción de Fallo No Codificado:',t2.DescFalloNoCod),CONCAT('DeSCRIPCIÓN DE Fallo: ',(SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo))),';Observaciones de Supervisión: ',t2.ObservacionesSupervision,';')) FROM modificaciones_sistema as mega INNER JOIN reportesupervicion as t2 ON mega.idregistro = t2.idreportesupervicion INNER JOIN sistrefaccmant.reportemantenimiento as t1 on t2.idreportesupervicion = t1.FoliofkSupervicion  WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        crearCatalogoPuesto(once);
                        break;
                    case "Actualización de Reporte de Mantenimiento":
                        string[] doce = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t3.Folio,';Económico: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportemantenimiento as t2 ON t1.idregistro=t2.IdReporte INNER JOIN reportesupervicion as t3 ON t2.FoliofkSupervicion=t3.idreportesupervicion WHERE idmodificacion='" + id + "';").ToString().Split(';');


                        string[] trece = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimaModificacion,coalesce((select concat(';usuario que modificó: ',x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Actualización de Reporte de Mantenimiento' and x4.idregistro=t2.IdReporte AND (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),(select concat(';usuario que modificó: ',x7.nombres,' ',x7.apPaterno,' ',x7.apMaterno,';Fecha/hora: ',date_format(t2.FechaReporteM,'%W, %d de %M del %Y'),'/',(t2.HoraInicioM)) from cpersonal as x7 where t2.MecanicofkPersonal=x7.idPersona)),';Clasificación de Fallo: ',(SELECT nombreFalloGral FROM cfallosgrales WHERE idfallogral=t2.FalloGralfkFallosGenerales),';Folio de Factura: ',t2.FolioFactura,';Trabajo Realizado: ',t2.TrabajoRealizado,';Observaciones: ',t2.ObservacionesM,';usuario que modicó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';Fecha/hora:',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN reportemantenimiento as t2 ON t1.idregistro=t2.IdReporte INNER JOIN reportesupervicion as t3 ON t2.FoliofkSupervicion=t3.idreportesupervicion WHERE idmodificacion='" + id + "';").ToString().Split(';');

                        trece[0] = UPPER("Clasificación de Fallo: " + v.getaData("SELECT nombreFalloGral FROM cfallosgrales WHERE idfallogral='" + trece[0] + "'"));
                        trece[1] = UPPER("Folio de Factura: " + trece[1]);
                        trece[2] = UPPER("Trabajo Realizado: " + trece[2]);
                        trece[3] = UPPER("Observaciones: " + trece[3]);
                        y1 = 120;
                        crearCatalogoPuesto(doce);
                        CrearAntesDespuesLabels();
                        mitad1mitad2(trece);
                        y1 = null;
                        break;
                    case "Exportación a PDF de orden de compra de almacen":
                        y1 = 200;
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 700); CenterToParent(); acomodarLabel();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.FolioOrdCompra,';Proveedor: ',(SELECT empresa FROM cproveedores WHERE idproveedor=t2.ProveedorfkCProveedores),';Empresa a Facturar: ',(select nombreEmpresa FROM cempresas WHERE idempresa=t2.FacturadafkCEmpresas),';Fecha de Orden de Compra: ',DATE_FORMAT(t2.FechaOCompra,'%W, %d de %M del %Y'),';Fecha de Entrega: ',DATE_FORMAT(t2.FechaEntregaOCompra,'%W, %d de %M del %Y'),';IVA: ',t2.IVA,';Estatus: ',t2.Estatus)) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ordencompra as t2 ON t1.idregistro=t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';'));
                        y1 = 500;
                        crearDataGrid((DataTable)v.getData("SET lc_time_names='es_ES';SELECT NumRefacc as 'Núm. Refaccion',(SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion=ClavefkCRefacciones) as 'Refacción',Cantidad,Precio,Total,ObservacionesRefacc as 'Observaciones' FROM sistrefaccmant.detallesordencompra WHERE OrdfkOrdenCompra='" + v.getaData("SELECT idregistro FROM modificaciones_sistema WHERE idmodificacion='" + id + "'") + "';"), new Point(0, 300));
                        break;
                    case "Actualización de Orden de Compra":
                        this.Size = new Size(1390, 900); CenterToParent(); acomodarLabel();
                        gbadd.Size = new Size(1380, 880);
                        string[] catorce = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.FolioOrdCompra,';IVA: ',t2.IVA,';Estatus: ',COALESCE(t2.Estatus,'EN PROCESO'))) FROM sistrefaccmant.modificaciones_sistema as t1 INNER JOIN ordencompra as t2 ON t1.idregistro=t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';');

                        string[] quince = v.getaData("SET lc_time_names='es_ES';SELECT upper(CONCAT(t1.ultimaModificacion,';usuario que modificó: ',coalesce((select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona where x4.Tipo='Actualización de Orden de Compra' and x4.idregistro=t2.idOrdCompra and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select concat(x7.nombres,' ',x7.apPaterno,' ',x7.apMaterno,';Fecha: ',date_format(t2.FechaOCompra,'%W, %d de %M del %Y')) from cpersonal as x7 where t2.usuariofkcpersonal=x7.idPersona)),';Proveedor: ',(SELECT empresa FROM cproveedores WHERE idproveedor=t2.ProveedorfkCProveedores),';Empresa a Facturar: ',(select nombreEmpresa FROM cempresas WHERE idempresa=t2.FacturadafkCEmpresas),';Fecha de Entrega: ',DATE_FORMAT(t2.FechaEntregaOCompra,'%W, %d de %M del %Y'),';Observaciones: ',t2.ObservacionesOC,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';Fecha: ',DATE_FORMAT(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora)))FROM modificaciones_sistema as t1 INNER JOIN ordencompra as t2 ON t1.idregistro=t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';');

                        quince[0] = UPPER("Proveedor: " + v.getaData("Select empresa From cproveedores WHERE idproveedor='" + quince[0] + "'"));
                        quince[1] = UPPER("Empresa a Facturar: " + v.getaData("SELECT nombreEmpresa FROM cempresas WHERE idempresa='" + quince[1] + "'"));
                        quince[2] = UPPER("FECHA DE ENTREGA: " + DateTime.Parse(quince[2]).ToString("dddd, MMMM dd yyyy"));
                        quince[3] = UPPER("Observaciones: " + quince[3]);
                        y1 = 170;
                        crearCatalogoPuesto(catorce);
                        CrearAntesDespuesLabels();
                        mitad1mitad2(quince);
                        y1 = null;
                        break;
                    case "Actualización de Refacción de Orden de Compra":

                        this.Size = new Size(1300, 800); CenterToParent(); acomodarLabel();

                        string[] diesciseis = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.FolioOrdCompra,';Fecha de Orden de Compra: ',DATE_FORMAT(t2.FechaOCompra,'%W, %d de %M del %Y'),';Fecha de Entrega: ',DATE_FORMAT(t2.FechaEntregaOCompra,'%W, %d de %M del %Y'),';IVA: ',CONCAT(COALESCE(t2.IVA,'0.0'),'%'),';Estatus: ',COALESCE(t2.Estatus,''))) FROM modificaciones_sistema as t1 INNER JOIN detallesordencompra as det ON t1.idregistro=det.idDetOrdCompra INNER JOIN ordencompra as t2 ON det.OrdfkOrdenCompra = t2.idOrdCompra WHERE idmodificacion='" + id + "'").ToString().Split(';');
                        string[] diescisiete = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,';Refacción: ',(SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion=ClavefkCRefacciones),';Precio: ',COALESCE(Precio,'0.0'),';Cantidad: ',Cantidad,';SubTotal: ',COALESCE(Total,'0.0'),';Observaciones: ',COALESCE(ObservacionesRefacc,''))) FROM modificaciones_sistema as t1 INNER JOIN detallesordencompra AS t2 On t1.idregistro=t2.idDetOrdCompra WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        float var = float.Parse(diescisiete[1]);
                        var = var * float.Parse(diescisiete[2]);
                        diescisiete[0] = UPPER("Refacción: " + v.getaData("SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion='" + diescisiete[0] + "'"));
                        diescisiete[1] = UPPER("Precio: " + diescisiete[1]);
                        diescisiete[2] = UPPER("Cantidad: " + diescisiete[2]);
                        diescisiete[3] = UPPER("Subtotal: " + var);
                        diescisiete[4] = UPPER(diescisiete[4]);
                        y1 = 250;
                        crearCatalogoPuesto(diesciseis);
                        CrearAntesDespuesLabels();
                        y1 = 300;
                        mitad1mitad2(diescisiete);
                        y1 = null;
                        break;
                    case "Actualización de Refacción en Reporte de Mantenimiento":
                        if (this.Size == new Size(1225, 517))
                        {
                            this.Size = new Size(1225, 600); CenterToParent(); acomodarLabel();
                        }

                        y1 = 250; crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',T3.Folio,';ECONÓMICO: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Usuario que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%m:%s'))) FROM  sistrefaccmant.modificaciones_sistema as t1 Inner join pedidosrefaccion as t2 ON t1.idregistro=t2.idPedRef inner join reportesupervicion as t3 ON t2.FolioPedfkSupervicion=t3.idReporteSupervicion WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));

                        string[] diescinueve = v.getaData("SELECT upper(CONCAT(ultimaModificacion,';Refacción: ',(SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion=t2.RefaccionfkCRefaccion) ,';Cantidad: ',t2.Cantidad)) FROM sistrefaccmant.modificaciones_sistema as t1 Inner join pedidosrefaccion as t2 ON t1.idregistro=t2.idPedRef WHERE idmodificacion='" + id + "';").ToString().Split(';');

                        diescinueve[0] = UPPER("Refacción: " + v.getaData("SELECT CONCAT(codrefaccion,' - ',nombreRefaccion) FROM crefacciones WHERE idrefaccion='" + diescinueve[0] + "'"));
                        diescinueve[1] = UPPER("Cantidad: " + diescinueve[1]);
                        CrearAntesDespuesLabels();
                        y1 = 270;
                        mitad1mitad2(diescinueve);
                        y1 = null;
                        break;
                    case "Inserción de Clasificación de Empresa":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 625);
                            gbadd.Size = new Size(1231, 325);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Clasificación: ',t2.giro,';usuario que inserto: ',(select concat(x1.nombres,' ',x1.apPaterno,' ',x1.apMaterno) from cpersonal as x1 where x1.idpersona=t1.usuariofkcpersonal),';Fecha / Hora: ',concat(date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora)),';tipo: ',t1.tipo)) FROM modificaciones_sistema  as t1 INNER JOIN cgiros as t2 On t1.idregistro = t2.idgiro WHERE t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Reactivación de Clasificación de Empresa":
                    case "Desactivación de Clasificación de Empresa":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 645);
                            gbadd.Size = new Size(1231, 345);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';clasificación: ',t2.giro,';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cgiros as t2 on t1.idregistro=t2.idgiro WHERE idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] _licencia = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Clasificación de Empresa','Usuario que activo: ',if(x4.tipo='Reactivación de Clasificación de Empresa','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Clasificación de Empresa','Activación de Clasificación de Empresa',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Clasificación de Empresa') or (x4.Tipo='Reactivación de Clasificación de Empresa') or (x4.Tipo='Inserción de Clasificación de Empresa')) and (x4.idregistro=t2.idgiro) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Clasificacion de Empresa',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cgiros as t2 on t1.idregistro=t2.idgiro WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 170;
                        CrearAntesDespuesLabels();
                        y1 = 175;
                        mitad1mitad2(_licencia);
                        break;
                    case "Actualización de Clasificación de Empresa":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 625);
                            gbadd.Size = new Size(1231, 325);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Motivo de actualización: ',t1.motivoActualizacion,';Tipo: ',t1.Tipo,';Estatus Actual: ',if(t2.status=1,'Activo',CONCAT('No Activo')))) FROM modificaciones_sistema as t1 INNER JOIN cgiros as t2 ON t1.idregistro=t2.idgiro WHERE idmodificacion='" + id + "'").ToString().Split(';'));

                        string[] clasi = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Clasificación de Empresa' or x4.Tipo='Inserción de Clasificación de Empresa') and x4.idregistro=t2.idgiro and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),';clasificación: ',t2.giro,';usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER JOIN cgiros as t2 ON t1.idregistro=t2.idgiro WHERE idmodificacion='" + id + "';").ToString().Split(';');
                        clasi[0] = ("clasificación: " + clasi[0]).ToUpper();
                        clasi[1] = ("usuario que modificó: " + clasi[1]).ToUpper();
                        clasi[2] = ("" + clasi[2]).ToUpper();
                        clasi[3] = ("" + clasi[3]).ToUpper();
                        clasi[4] = ("" + clasi[4]).ToUpper();
                        clasi[5] = ("" + clasi[5]).ToUpper();
                        y1 = 160;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(clasi);
                        y1 = null;
                        break;
                    case "Modificación De IVA":
                        y1 = 140;
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 620);
                            gbadd.Size = new Size(1231, 330);
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Tipo: ', t1.Tipo,';Razón de actualización: ', COALESCE(t1.motivoActualizacion, ''))) FROM modificaciones_sistema AS t1 INNER JOIN civa AS t2 ON t1.idregistro = t2.idiva WHERE idmodificacion = '" + id + "'").ToString().Split(';'));

                        string[] veintiuno = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(t1.ultimamodificacion,';',coalesce((select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Modificación De IVA' and x4.idregistro=t2.idiva and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),concat('usuario: ',';fecha/hora: ')),';iva: ',t2.iva,';usuario que modifico: ',(select concat(x1.nombres,' ',x1.apPaterno,' ',x1.ApMaterno) from cpersonal as x1 where x1.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema as t1 INNER join civa as t2 ON t1.idregistro=t2.idiva WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        veintiuno[0] = ("IVA: " + veintiuno[0]).ToUpper();
                        veintiuno[1] = ("Usuario que modificó: " + veintiuno[1]).ToUpper();
                        veintiuno[2] = ("" + veintiuno[2]).ToUpper();
                        CrearAntesDespuesLabels();
                        y1 = 160;
                        mitad1mitad2(veintiuno);
                        y1 = null;
                        break;
                    case "Desactivación de Tipo De Licencia":
                    case "Reactivación de Tipo De Licencia":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 645);
                            gbadd.Size = new Size(1231, 345);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';tipo de licencia: ',t2.Tipo,';Descripción: ',t2.Descripcion,';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cattipos as t2 on t1.idregistro=t2.idcattipos WHERE idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] _tipos = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Tipo De Licencia','Usuario que activo: ',if(x4.tipo='Reactivación de Tipo De Licencia','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Tipo de Licencia','Activación de Tipo de Licencia',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Tipo De Licencia') or (x4.Tipo='Reactivación de Tipo De Licencia') or (x4.Tipo='Inserción de Tipo De Licencia')) and (x4.idregistro=t2.idcattipos) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Tipo De Licencia',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cattipos as t2 on t1.idregistro=t2.idcattipos WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 190;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_tipos);
                        break;
                    case "Actualización De Reporte Incidencia Personal":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 950))
                        {
                            this.Size = new Size(1296, 950);
                            gbadd.Size = new Size(1231, 650);
                            CenterToParent();
                        }
                        else this.Size = new Size(1296, 685);
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';consecutivo: ',t2.consecutivo,';Tipo: ', t1.Tipo)) as r FROM modificaciones_sistema as t1 INNER join incidenciapersonal as t2 on t2.idIncidencia=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] editar_R = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,coalesce((select concat(';Persona Que Modificó: ',x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/Hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Actualización De Reporte Incidencia Personal' and x4.idregistro=t2.idIncidencia and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select concat(';Persona Que Inserto: ',x5.appaterno,' ',x5.apmaterno,' ',x5.nombres,';Fecha/Hora: ',date_format(t2.FechaHoraRegistro,'%W, %d de %M del %Y'),'/',time(t2.FechaHoraRegistro)) from cpersonal as x5 where x5.idpersona=t2.UsuarioFkCpersonal)),';colaborador: ',(select concat(x6.appaterno,' ',x6.apmaterno,' ',x6.nombres,';credencial: ',x6.credencial) from cpersonal as x6 where x6.idpersona=t2.ColaboradorfkCpersonal),';Fecha: ',date_format(t2.Fecha,'%W, %d de %M del %Y'),';Hora: ',time_format(t2.hora,'%H:%i'),';Incidencia número: ',coalesce((select x7.numeroIncidencia from catincidencias as x7 where x7.idincidencia=t2.IncidenciafkCatIncidencias),''),';lugar del incidente: ',coalesce(t2.Lugar),';Acta N°: ',coalesce(t2.Acta),';síntesis: ',coalesce(t2.Sintesis),';comentario: ',coalesce(t2.Comentario),';Persona Que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) AS m FROM modificaciones_sistema as t1 INNER JOIN incidenciapersonal as t2 on t2.idIncidencia=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        editar_R[0] = ("Colaborador: " + editar_R[0]).ToUpper();
                        editar_R[1] = ("credencial: " + editar_R[1]).ToUpper();
                        editar_R[2] = ("Fecha: " + DateTime.Parse(editar_R[2]).ToLongDateString()).ToUpper();
                        editar_R[3] = ("Hora: " + DateTime.Parse(editar_R[3]).ToString("HH:mm")).ToUpper();
                        editar_R[4] = ("incidencia número: " + v.getaData("select numeroIncidencia from catincidencias where idincidencia='" + editar_R[4] + "'")).ToUpper();
                        editar_R[5] = ("lugar del incidente: " + editar_R[5]).ToUpper();
                        editar_R[6] = ("Acta N°: " + editar_R[6]).ToUpper();
                        editar_R[7] = ("Síntesis: " + editar_R[7]).ToUpper();
                        editar_R[8] = ("Comentario: " + editar_R[8]).ToUpper();
                        CrearAntesDespuesLabels();
                        mitad1mitad2(editar_R);
                        CenterToParent();
                        break;
                    case "Inserción de Incidencia":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 700))
                        {
                            this.Size = new Size(1296, 700);
                            gbadd.Size = new Size(1231, 400);
                        }
                        else this.Size = new Size(1296, 345);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';select upper(concat('Incidencia N°: ',t2.numeroIncidencia,'; Concepto: ',t2.concepto,'; Usuario que inserto: ',(select concat(x1.ApPaterno,' ',x1.ApMaterno,' ',x1.nombres) from cpersonal as x1 where x1.idPersona=t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo)) from modificaciones_sistema as t1 inner join catincidencias as t2 on t1.idregistro=t2.idincidencia where t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Incidencia":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 700))
                        {
                            this.Size = new Size(1296, 700);
                            gbadd.Size = new Size(1231, 400);
                        }
                        else this.Size = new Size(1296, 450);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT('Motivo de actualización: ',coalesce(t1.motivoActualizacion,''),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t2.idPersona = t1.usuariofkcpersonal WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] incidencia = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Incidencia' or x4.Tipo='Inserción de Incidencia') and x4.idregistro=t2.idincidencia and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1), ';N° Incidencia: ', t2.numeroIncidencia, ';Concepto: ', t2.concepto,';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where t1.usuariofkcpersonal=x2.idPersona),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema AS t1 INNER JOIN catincidencias AS t2 ON t1.idregistro = t2.idincidencia WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        incidencia[0] = ("N° Incidencia: " + incidencia[0]).ToUpper();
                        incidencia[1] = ("Concepto: " + incidencia[1]).ToUpper();
                        incidencia[2] = ("usuario que modificó: " + incidencia[2]).ToUpper();
                        CrearAntesDespuesLabels();
                        mitad1mitad2(incidencia);
                        CenterToParent();
                        y1 = null;
                        break;
                    case "Desactivación de Incidencia":
                    case "Reactivación de Incidencia":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 700))
                        {
                            this.Size = new Size(1296, 700);
                            gbadd.Size = new Size(1231, 400);
                        }
                        else this.Size = new Size(1296, 500);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo: ',t1.motivoActualizacion,';Incidencia número: ',t2.numeroIncidencia,';Concepto: ', t2.concepto,';tipo: ',t1.Tipo)) as r FROM modificaciones_sistema as t1 INNER join catincidencias as t2 on t2.idincidencia=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] _desc = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Incidencia','Usuario que activo: ',if(x4.tipo='Reactivación de Incidencia','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Incidencia','Activación de Incidencia',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Incidencia') or (x4.Tipo='Reactivación de Incidencia') or (x4.Tipo='Inserción de Incidencia')) and (x4.idregistro=t2.idincidencia) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Incidencia',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join catincidencias as t2 on t2.idincidencia=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        CrearAntesDespuesLabels();
                        mitad1mitad2(_desc);
                        break;
                    case "Inserción de Tipo De Licencia":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1326, 645);
                            gbadd.Size = new Size(1261, 345);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT('Tipo: ',t2.tipo,';Descripción: ',t2.descripcion,';Usuario Que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';Fecha / Hora: ',DATE_FORMAT(t1.fechahora,'%W, %d de %M del %Y / %H:%i:%s'),';Tipo: ',t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cattipos as t2 ON t1.idregistro = t2.idcattipos WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        break;

                    case "Actualización de Tipo De Licencia":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 645);
                            gbadd.Size = new Size(1231, 345);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT('Motivo de actualización: ',coalesce(t1.motivoActualizacion,''),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema AS t1 INNER JOIN cpersonal AS t2 ON t2.idPersona = t1.usuariofkcpersonal WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] veinte = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Tipo De Licencia' or x4.Tipo='Inserción de Tipo de Licencia') and x4.idregistro=t2.idcattipos and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1), ';Tipo: ', t2.tipo, ';Descripción: ', t2.descripcion,';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where t1.usuariofkcpersonal=x2.idPersona),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema AS t1 INNER JOIN cattipos AS t2 ON t1.idregistro = t2.idcattipos WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        veinte[0] = ("Tipo: " + veinte[0]).ToUpper();
                        veinte[1] = ("Descripción: " + veinte[1]).ToUpper();
                        veinte[2] = ("Usuario que modificó: " + veinte[2]).ToUpper();
                        y1 = 120;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(veinte);
                        CenterToParent();
                        y1 = null;
                        break;
                    case "Inserción de Familia de Refacción":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1295, 605);
                            gbadd.Size = new Size(1231, 305);
                            CenterToParent();
                        }
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES';select upper(concat('Familia: ',t2.familia,';usuario que inserto: ',(select concat(x1.nombres,' ',x1.apPaterno,' ',x1.apMaterno) from cpersonal as x1 where x1.idPersona=t2.usuariofkcpersonal),';Fecha/Hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) as f from modificaciones_sistema as t1 inner join cnfamilias as t2 on t2.idcnfamilia=t1.idregistro where t1.idmodificacion='" + id + "';").ToString().Split(';'));
                        break;
                    case "Desactivación de Nombre de Familia":
                    case "Reactivación de Nombre de Familia":
                        if (this.Size == new Size(1296, 905))
                        {
                            this.Size = new Size(1296, 630);
                            gbadd.Size = new Size(1231, 330);
                            CenterToParent();
                        }
                        y1 = 160;
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';nombre de familia: ',(select Familia from cnfamilias as x1 where x1.idcnFamilia=t1.idregistro),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cnfamilias as t2 on t1.idregistro=t2.idcnFamilia WHERE idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] _des = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Familia de Refacción','Usuario que activo: ',if(x4.tipo='Reactivación de Nombre de Familia','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Familia de Refacción','Activación de Nombre de Familia',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Nombre de Familia') or (x4.Tipo='Reactivación de Nombre de Familia') or (x4.Tipo='Inserción de Familia de Refacción')) and (x4.idregistro=t2.idcnfamilia) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Nombre de Familia',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cnfamilias as t2 on t1.idregistro=t2.idcnFamilia WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        CrearAntesDespuesLabels();
                        y1 = 170;
                        mitad1mitad2(_des);
                        break;
                    case "Actualización de Existencias":
                        y1 = 160;
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';Refacción: ',(select nombreRefaccion from crefacciones x1 where x1.idrefaccion=t1.idregistro),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join crefacciones as t2 on t1.idregistro=t2.idrefaccion WHERE idmodificacion = '" + id + "';").ToString().Split(';'));

                        string[] _stock = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT(t1.ultimamodificacion,';', (select concat('USUARIO QUE MODIFICÓ: ',x3.nombres, ' ', x3.ApPaterno, ' ', x3.apMaterno, ';Fecha/hora: ', date_format(x4.fechaHora, '%W, %d de %M del %Y'), '/', time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal = x3.idpersona WHERE(x4.Tipo = 'Actualización de Existencias' or x4.Tipo = 'Inserción de Refacción') and x4.idregistro = t2.idrefaccion and(x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1, 1), ';Existencias: ', t2.existencias,((SELECT UPPER(t5.Simbolo) FROM modificaciones_sistema as t1 INNER JOIN crefacciones as t2 ON t1.idregistro=t2.idrefaccion INNER JOIN cmarcas as t3 ON t2.marcafkcmarcas=t3.idmarca INNER JOIN cfamilias as t4 ON t3.descripcionfkcfamilias =t4.idfamilia INNER JOIN cunidadmedida as t5 ON  t4.umfkcunidadmedida=t5.idunidadmedida WHERE t1.idmodificacion='" + id + "')), ';Usuario que modificó: ', (select concat(x2.nombres, ' ', x2.apPaterno, ' ', x2.apMaterno) from cpersonal as x2 where t1.usuariofkcpersonal = x2.idPersona),';fecha/hora: ',date_format(t1.fechaHora, '%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema AS t1 INNER JOIN crefacciones AS t2 ON t1.idregistro = t2.idrefaccion WHERE t1.idmodificacion = '" + id + "'; ").ToString().Split(';');
                        _stock[0] = "Existencias: " + _stock[0] + v.getaData("SELECT UPPER(t5.Simbolo) FROM modificaciones_sistema as t1 INNER JOIN crefacciones as t2 ON t1.idregistro=t2.idrefaccion INNER JOIN cmarcas as t3 ON t2.marcafkcmarcas=t3.idmarca INNER JOIN cfamilias as t4 ON t3.descripcionfkcfamilias =t4.idfamilia INNER JOIN cunidadmedida as t5 ON  t4.umfkcunidadmedida=t5.idunidadmedida WHERE t1.idmodificacion='" + id + "'"); ;
                        CrearAntesDespuesLabels();
                        y1 = 170;
                        mitad1mitad2(_stock);
                        break;
                    case "Actualización de Reporte de Personal":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 950))
                        {
                            this.Size = new Size(1296, 950);
                            gbadd.Size = new Size(1231, 650);
                            CenterToParent();
                        }
                        else this.Size = new Size(1296, 765);
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join reportepersonal as t2 on t2.idreportepersonal=t1.idregistro WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] personal = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,coalesce((select concat(';Persona Que Modificó: ',x3.ApPaterno,' ',x3.apMaterno,' ',x3.nombres,';Fecha/Hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Actualización de Reporte de Personal' and x4.idregistro=t2.idreportepersonal and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select concat(';Persona Que Inserto: ',x5.appaterno,' ',x5.apmaterno,' ',x5.nombres,';Fecha/Hora: ',date_format(t2.FechaHoraRegistro,'%W, %d de %M del %Y'),'/',time(t2.FechaHoraRegistro)) from cpersonal as x5 where x5.idpersona=t2.UsuarioFkCpersonal)),';colaborador: ',(select concat(x6.appaterno,' ',x6.apmaterno,' ',x6.nombres,';credencial: ',x6.credencial) from cpersonal as x6 where x6.idpersona=t2.credencialfkcpersonal),';Fecha: ',date_format(t2.Fecha,'%W, %d de %M del %Y'),';Hora: ',time_format(t2.Hora,'%H:%i'),';Lugar del incidente: ',t2.LugarIncidente,';tipo de vehículo u objeto: ',coalesce(t2.TipoVehObj,''),';kilometraje actual: ',if(t2.Kilometraje>0,t2.Kilometraje,''),';defectos: ',coalesce(t2.Observaciones,''),';responsable: ',coalesce((select concat(x7.appaterno,' ',x7.apmaterno,' ',x7.nombres) from cpersonal as x7 where x7.idpersona=t2.responsablefkcpersonal),''),';coordinador: ',coalesce((select concat(x8.appaterno,' ',x8.apmaterno,' ',x8.nombres) from cpersonal as x8 where x8.idpersona=t2.coordinadorfkcpersonal),''),';Persona Que Modifica: ',(SELECT CONCAT(apPaterno,' ',apMaterno,' ',nombres) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) AS m FROM modificaciones_sistema as t1 INNER JOIN reportepersonal as t2 on t2.idreportepersonal=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        personal[0] = ("colaborador: " + personal[0]).ToUpper();
                        personal[1] = ("Credencial: " + personal[1]).ToUpper();
                        personal[2] = ("Fecha: " + DateTime.Parse(personal[2]).ToLongDateString()).ToUpper();
                        personal[3] = ("Hora: " + DateTime.Parse(personal[3]).ToString("HH:mm")).ToUpper();
                        personal[4] = ("Lugar del incidente:" + personal[4]).ToUpper();
                        personal[5] = ("tipo de vehículo u objeto: " + personal[5]).ToUpper();
                        if (Convert.ToDouble(personal[6]) == 0) personal[6] = "";
                        personal[6] = ("kilometraje actual: " + personal[6]).ToUpper();
                        personal[7] = ("defectos: " + personal[7]).ToUpper();
                        if (Convert.ToInt32(personal[8]) == 0) personal[8] = "";
                        if (Convert.ToInt32(personal[9]) == 0) personal[9] = "";
                        personal[8] = ("responsable: " + personal[8]).ToUpper();
                        personal[9] = ("coordinador: " + v.getaData("select concat(appaterno,' ',apmaterno,' ',nombres) from cpersonal where idpersona='" + personal[9] + "'")).ToUpper();
                        y1 = 120;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(personal);
                        CenterToParent();
                        y1 = null;
                        break;
                    case "Actualización de Encabezado de Reportes":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 750))
                        {
                            this.Size = new Size(1296, 750);
                            gbadd.Size = new Size(1231, 450);
                            CenterToParent();
                        }
                        else this.Size = new Size(1296, 450);
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join encabezadoreportes as t2 on t2.idencabezadoreportes=t1.idregistro  WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] encabezados = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(ultimaModificacion,coalesce((select concat(';Persona Que Modificó: ',x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/Hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Actualización de Encabezado de Reportes' and x4.idregistro=t2.idencabezadoreportes and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select concat(';Persona Que Inserto: ',x5.appaterno,' ',x5.apmaterno,' ',x5.nombres,';Fecha/Hora: ',date_format(t2.FechaHoraRegistro,'%W, %d de %M del %Y'),'/',time(t2.FechaHoraRegistro)) from cpersonal as x5 where x5.idpersona=t2.UsuarioFkCpersonal)),';nombre de reporte: ',t2.nombrereporte,';código de reporte: ',t2.codigoreporte,';vigencia: ',date_format(t2.vigencia,'%M %Y'),';revisión: ',t2.revision,';Persona Que Modifica: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) AS m FROM modificaciones_sistema as t1 INNER JOIN encabezadoreportes as t2 on t2.idencabezadoreportes=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        encabezados[0] = ("nombre de reporte: " + encabezados[0]).ToUpper();
                        encabezados[1] = ("código de reporte: " + encabezados[1]).ToUpper();
                        encabezados[2] = ("vigencia: " + encabezados[2]).ToUpper();
                        encabezados[3] = ("revisión: " + encabezados[3]).ToUpper();
                        y1 = 120;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(encabezados);
                        CenterToParent();
                        y1 = null;
                        break;
                    case "Inserción de Estación":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1295, 625))
                        {
                            this.Size = new Size(1295, 625);
                            gbadd.Size = new Size(1231, 325);
                        }
                        else this.Size = new Size(1295, 385);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Estación: ',t2.estacion,';usuario que inserta: ',(select concat(x1.apPaterno,' ',x1.ApMaterno,' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.usuariofkcpersonal),';Fecha/Hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join cestaciones as t2 on t2.idestacion=t1.idregistro  WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        break;
                    case "Actualización de Estación":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1295, 625))
                        {
                            this.Size = new Size(1295, 625);
                            gbadd.Size = new Size(1231, 325);
                        }
                        else this.Size = new Size(1295, 385);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT('Motivo de actualización: ',coalesce(t1.motivoActualizacion,''),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema AS t1 INNER JOIN cestaciones as t2 on t2.idestacion=t1.idregistro WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] estacion = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Estación' or x4.Tipo='Inserción de Estación') and x4.idregistro=t2.idestacion and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),'Estación: ',t2.estacion,';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where t1.usuariofkcpersonal=x2.idPersona),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema AS t1 INNER JOIN cestaciones as t2 on t2.idestacion=t1.idregistro WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        y1 = 120;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(estacion);
                        CenterToParent();
                        y1 = null;
                        break;
                    case "Desactivación de Estación":
                    case "Reactivación de Estación":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1295, 625))
                        {
                            this.Size = new Size(1295, 625);
                            gbadd.Size = new Size(1231, 325);
                        }
                        else this.Size = new Size(1295, 385);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Estación: ',t2.estacion,';Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';tipo: ',t1.Tipo)) as r FROM modificaciones_sistema as t1 INNER join cestaciones as t2 on t2.idestacion=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] estacion_d = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Estación','Usuario que activo: ',if(x4.tipo='Reactivación de Estación','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Estación','Activación de Estación',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Estación') or (x4.Tipo='Reactivación de Estación') or (x4.Tipo='Inserción de Estación')) and (x4.idregistro=t2.idestacion) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Estación',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join cestaciones as t2 on t2.idestacion=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 150;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(estacion_d);
                        y1 = null;
                        break;
                    case "Inserción de Relación":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 630))
                        {
                            this.Size = new Size(1296, 630);
                            gbadd.Size = new Size(1231, 430);
                            CenterToParent();
                        }
                        else this.Size = new Size(1296, 430);
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Estación: ',(select estacion from cestaciones as x3 where x3.idestacion=t2.estacionfkcestaciones),';Servicio: ',(select concat(x2.Nombre,' - ',x2.Descripcion) from cservicios as x2 where x2.idservicio=t2.serviciofkcservicios),';usuario que inserta: ',(select concat(x1.apPaterno,' ',x1.ApMaterno,' ',x1.nombres) from cpersonal as x1 where x1.idpersona=t1.usuariofkcpersonal),';Fecha/Hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';Tipo: ', t1.Tipo)) FROM modificaciones_sistema as t1 INNER join relacservicioestacion as t2 on t2.idrelacServicioEstacion=t1.idregistro  WHERE t1.idmodificacion = '1165';").ToString().Split(';'));
                        break;
                    case "Actualización de Relación":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 630))
                        {
                            this.Size = new Size(1296, 630);
                            gbadd.Size = new Size(1231, 430);
                            CenterToParent();
                        }
                        else this.Size = new Size(1296, 430);
                        crearCatalogoPuesto(v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT('Motivo de actualización: ',coalesce(t1.motivoActualizacion,''),';Tipo: ', t1.Tipo)) as r FROM modificaciones_sistema AS t1 INNER JOIN relacservicioestacion as t2 on t2.idrelacServicioEstacion=t1.idregistro WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] rel = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT(t1.ultimamodificacion,';',(select concat(x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';Fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE (x4.Tipo='Actualización de Relación' or x4.Tipo='Inserción de Relación') and x4.idregistro=t2.idrelacServicioEstacion and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),';servicio: ',(select concat(x6.Nombre,' - ',x6.Descripcion) from cservicios as x6 where x6.idservicio=t2.serviciofkcservicios),';estación: ',(select estacion from cestaciones as x5 where x5.idestacion=t2.estacionfkcestaciones),';Usuario que modificó: ',(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where t1.usuariofkcpersonal=x2.idPersona),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora))) FROM modificaciones_sistema AS t1 INNER JOIN relacservicioestacion as t2 on t2.idrelacServicioEstacion=t1.idregistro WHERE t1.idmodificacion = '" + id + "'").ToString().Split(';');
                        rel[0] = ("servicio: " + v.getaData("select concat(Nombre,' - ',Descripcion) from cservicios where idservicio='" + rel[0] + "'")).ToUpper();
                        rel[1] = ("estación: " + v.getaData("select estacion from cestaciones where idestacion='" + rel[1] + "'")).ToUpper();
                        rel[2] = ("usuario: " + rel[2]).ToUpper();
                        y1 = 130;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(rel);
                        CenterToParent();
                        y1 = null;
                        break;
                    case "Desactivación de Relación":
                    case "Reactivación de Relación":
                        if (this.Size == new Size(1296, 905) || this.Size == new Size(1296, 630))
                        {
                            this.Size = new Size(1296, 630);
                            gbadd.Size = new Size(1231, 430);
                            CenterToParent();
                        }
                        else this.Size = new Size(1296, 430);
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Relación: ',concat((select concat(x1.Nombre,' / ',x1.Descripcion) from cservicios as x1 where x1.idservicio=t2.serviciofkcservicios ),' - ',(select x2.estacion from cestaciones as x2 where x2.idestacion=t2.estacionfkcestaciones)),';Motivo De Modificación: ',coalesce(t1.motivoActualizacion,''),';tipo: ',t1.Tipo)) as r FROM modificaciones_sistema as t1 INNER join relacservicioestacion as t2 on t2.idrelacServicioEstacion=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] des_r = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT((select concat(if(x4.tipo='Inserción de Relación','Usuario que activo: ',if(x4.tipo='Reactivación de Relación','Usuario que reactivo: ','usuario que desactivo: ')),x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,';fecha/hora: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'/',time(x4.fechahora),';Tipo: ',if(x4.tipo='Inserción de Relación','Activación de Relación',x4.tipo)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE ((x4.Tipo='Desactivación de Relación') or (x4.Tipo='Reactivación de Relación') or (x4.Tipo='Inserción de Relación')) and (x4.idregistro=t2.idrelacServicioEstacion) and (x4.idmodificacion between '1' and '" + id + "')  order by x4.idmodificacion desc limit 1,1),if(t1.tipo='Reactivación de Relación',';Usuario que reactivo: ',';usuario que desactivo: '),(select concat(x2.nombres,' ',x2.apPaterno,' ',x2.apMaterno) from cpersonal as x2 where x2.idpersona=t1.usuariofkcpersonal),';fecha/hora: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'/',time(t1.fechahora),';tipo: ',t1.tipo)) FROM modificaciones_sistema as t1 INNER join relacservicioestacion as t2 on t2.idrelacServicioEstacion=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split(';');
                        y1 = 150;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(des_r);
                        CenterToParent();
                        y1 = null;
                        break;
                    case "Actualización de Evidencias en Reporte de Percance":
                        if (this.Size == new Size(1496, 855) || this.Size == new Size(1496, 715))
                        {
                            this.Size = new Size(1496, 900);
                            gbadd.Size = new Size(1431, 600);
                            CenterToParent();
                        }
                        else this.Size = new Size(1496, 715);
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ', COALESCE(t1.motivoActualizacion, ''), ';Tipo: ', t1.Tipo)) FROM modificaciones_sistema AS t1 INNER JOIN reportepercance AS t2 ON t2.idreportePercance = t1.idregistro WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] evidencias = v.getaData("SET lc_time_names='es_ES';SELECT CONVERT(CONCAT(ultimaModificacion,'|',coalesce(t2.evidencia1,''),'|',coalesce(t2.evidencia2,''),'|',coalesce(t2.evidencia3,''),'|',coalesce(t2.evidencia4,''))using utf8) AS m FROM modificaciones_sistema as t1 INNER JOIN reportepercance as t2 on t2.idreportepercance=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split('|');

                        string[] dates = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT(coalesce((select concat('Persona Que Modificó: ',x3.nombres,' ',x3.ApPaterno,' ',x3.apMaterno,'|Fecha: ',date_format(x4.fechaHora,'%W, %d de %M del %Y'),'|Hora: ',time(x4.fechahora)) from cpersonal as x3 inner join modificaciones_sistema as x4 on x4.usuariofkcpersonal=x3.idpersona WHERE x4.Tipo='Actualización de Evidencias en Reporte de Percance' and x4.idregistro=t2.idreportepercance and (x4.idmodificacion between '1' and '" + id + "') order by x4.idmodificacion desc limit 1,1),(select concat('Persona Que Inserto: ',x5.appaterno,' ',x5.apmaterno,' ',x5.nombres,'|Fecha: ',date_format(t2.fechaHoraInsercion,'%W, %d de %M del %Y'),'|Hora: ',time(t2.fechaHoraInsercion)) from cpersonal as x5 where x5.idpersona=t2.usuarioinsertofkcpersonal)),'|Persona que modificá: ',(select concat(coalesce(p1.appaterno,''),' ',coalesce(p1.apmaterno,''),' ',p1.nombres) from cpersonal as p1 where p1.idpersona=t1.usuariofkcpersonal),'|Fecha: ',date_format(t1.fechaHora,'%W, %d de %M del %Y'),'|Hora: ',time(t1.fechaHora))) AS m FROM modificaciones_sistema as t1 INNER JOIN reportepercance as t2 on t2.idreportepercance=t1.idregistro WHERE idmodificacion = '" + id + "';").ToString().Split('|');
                        CrearAntesDespuesLabels();
                        imagenes(evidencias);
                        y1 = 500;
                        mitad1mitad2(dates);
                        break;
                    case "Actualización de Reporte de Percance":
                        scroll = true;
                        this.Size = new Size(1296, 1050);
                        gbadd.Size = new Size(1231, 750);
                        CenterToParent();
                        crearCatalogoPuesto(v.getaData("SELECT UPPER(CONCAT('Motivo De Modificación: ', COALESCE(t1.motivoActualizacion, ''), ';Tipo: ', t1.Tipo)) FROM modificaciones_sistema AS t1 INNER JOIN reportepercance AS t2 ON t2.idreportePercance = t1.idregistro WHERE t1.idmodificacion = '" + id + "';").ToString().Split(';'));
                        string[] percances = v.getaData("SET lc_time_names = 'es_ES'; SELECT UPPER(CONCAT(ultimaModificacion, COALESCE((SELECT CONCAT('|Persona Que Modificó: ', x3.nombres, ' ', x3.ApPaterno, ' ', x3.apMaterno, '|Fecha/Hora: ', DATE_FORMAT(x4.fechaHora, '%W, %d de %M del %Y'), '/', TIME(x4.fechahora)) FROM cpersonal AS x3 INNER JOIN modificaciones_sistema AS x4 ON x4.usuariofkcpersonal = x3.idpersona WHERE x4.Tipo = 'Actualización de Reporte de Percance' AND x4.idregistro = t2.idreportepercance AND (x4.idmodificacion BETWEEN '1' AND '" + id + "') ORDER BY x4.idmodificacion DESC LIMIT 1,1), (SELECT CONCAT('|Persona Que Inserto: ', x5.appaterno, ' ', x5.apmaterno, ' ', x5.nombres, '|Fecha/Hora: ', DATE_FORMAT(t2.fechaHoraInsercion, '%W, %d de %M del %Y'), '/', TIME(t2.fechaHoraInsercion)) FROM cpersonal AS x5 WHERE x5.idpersona = t2.usuarioinsertofkcpersonal)),  '|Económico: ', COALESCE((SELECT CONCAT(z2.identificador, LPAD(z1.consecutivo, 4, '0')) FROM cunidades AS z1 INNER JOIN careas AS z2 ON z1.areafkcareas = z2.idarea WHERE z1.idunidad = t2.ecofkcunidades), ''), (SELECT CONCAT('|Conductor: ', z3.ApPaterno, ' ', z3.ApMaterno, ' ', z3.nombres) FROM cpersonal AS z3 WHERE z3.idPersona = t2.Conductorfkcpersonal), '|Fecha/Hora de Accidente: ', COALESCE(t2.fechaHoraAccidente, ''), '|Servicio: ', COALESCE((SELECT CONCAT(z4.nombre, ' ', z4.Descripcion) FROM cservicios AS z4 WHERE z4.idservicio = t2.servicioenlaborfkcservicios), ''), '|Lugar del Accidente: ', COALESCE(t2.lugaraccidente, ''), '|Dirección: ', IF(t2.direccion != 0, IF(t2.direccion != 2, 'NORTE', 'SUR'), ''), '|De estación: ', COALESCE((SELECT z5.estacion FROM cestaciones AS z5 WHERE z5.idestacion = t2.estacion1fkcestaciones), ''), '|A estación: ', COALESCE((SELECT z6.estacion FROM cestaciones AS z6 WHERE z6.idestacion = t2.estacion2fkcestaciones), ''), '|De estación: ', COALESCE((SELECT z7.estacion FROM cestaciones AS z7 WHERE z7.idestacion = t2.estacion3fkcestaciones), ''), '|A estación: ', COALESCE((SELECT z8.estacion FROM cestaciones AS z8 WHERE z8.idestacion = t2.estacion4fkcestaciones), ''), '|Económico recuperado: ', COALESCE((SELECT CONCAT(z10.identificador, LPAD(z9.consecutivo, 4, '0')) FROM cunidades AS z9 INNER JOIN careas AS z10 ON z9.areafkcareas = z10.idarea WHERE z9.idunidad = t2.ecorecuperacionfkcunidades), ''), '|Estación: ', COALESCE((SELECT z11.estacion FROM cestaciones AS z11 WHERE z11.idestacion = t2.estacionfkcestaciones), ''), '|Síntesis de lo ocurrido: ', COALESCE(t2.sintesisocurrido, ''), '|Descripción: ', COALESCE(t2.descripcion, ''), '|Marca de vehículo: ', COALESCE(t2.marcavehiculotercero, ''), '|Año de vehículo: ', COALESCE(t2.yearvehiculotercero, ''), '|Placas de vehículo: ', COALESCE(t2.placasvehiculotercero, ''), '|Nombre del conductor: ', COALESCE(t2.nombreconductortercero, ''), '|Teléfono del conductor: ', COALESCE(t2.telefonoconductortercero, ''), '|Domicilio del conductor: ', COALESCE(t2.domicilioconductortercero, ''), '|Número de reporte del seguro: ', COALESCE(t2.numreporteseguro, '0'), '|Hora de otorgamiento: ', COALESCE(t2.horaotorgamiento, ''), '|Hora de llegada del seguro: ', COALESCE(t2.horallegadaseguro, ''), '|Nombre del ajustador: ', COALESCE(t2.nombreajustador, ''), '|Solución: ', COALESCE(t2.solucion, ''), '|Número de acta: ', COALESCE(t2.numacta, ''), '|Supervisor: ', COALESCE((SELECT CONCAT( z12.ApPaterno, ' ', z12.ApMaterno, ' ', z12.nombres) FROM cpersonal AS z12 WHERE z12.idPersona = t2.supervisorkcpersonal), ''), '|Unidad de asistencia médica: ', COALESCE(t2.unidadmedica, ''), '|Perteneciente de asistencia médica: ', COALESCE(t2.perteneceunidad, ''), '|Nombre del responsable médico: ', COALESCE(t2.nombreResponsableunidad, ''), '|En caso de lesionados: ', COALESCE(t2.encasolesionados, ''), '|Comentarios: ', COALESCE(t2.comentarios, ''), (SELECT CONCAT('|Persona Que Modifica: ', nombres, ' ', apPaterno,' ', apMaterno, '|Fecha/hora: ', DATE_FORMAT(t1.fechaHora,'%W, %d de %M del %Y'),'/', TIME(t1.fechahora)) FROM cpersonal WHERE idpersona = t1.usuariofkcpersonal))) AS m FROM modificaciones_sistema AS t1 INNER JOIN reportepercance AS t2 ON t1.idregistro = t2.idreportepercance WHERE t1.idmodificacion = '" + id + "';").ToString().Split('|');
                        percances[0] = UPPER("Económico: " + v.getaData("SELECT CONCAT(z2.identificador, LPAD(z1.consecutivo, 4, '0')) FROM cunidades AS z1 INNER JOIN careas AS z2 ON z1.areafkcareas = z2.idarea WHERE z1.idunidad = '" + percances[0] + "'")).ToString();
                        percances[1] = UPPER("Conductor: " + v.getaData("SELECT CONCAT(z3.ApPaterno, ' ', z3.ApMaterno, ' ', z3.nombres) FROM cpersonal AS z3 WHERE z3.idPersona = '" + percances[1] + "'")).ToString();
                        percances[2] = UPPER("Fecha/Hora de Accidente: " + percances[2]);
                        percances[3] = UPPER("Servicio: " + v.getaData("SELECT CONCAT(z4.nombre, ' ', z4.Descripcion) FROM cservicios AS z4 WHERE z4.idservicio = '" + percances[3] + "'")).ToString();
                        percances[4] = UPPER("Lugar del Accidente: " + percances[4]);
                        if (Convert.ToInt32(percances[5]) == 0)
                            percances[5] = "";
                        percances[5] = UPPER("Dirección: " + percances[5]);
                        percances[6] = UPPER("De estación: " + v.getaData("SELECT z5.estacion FROM cestaciones AS z5 WHERE z5.idestacion = '" + percances[6] + "'")).ToString();
                        percances[7] = UPPER("A estación: " + v.getaData("SELECT z6.estacion FROM cestaciones AS z6 WHERE z6.idestacion = '" + percances[7] + "'")).ToString();
                        percances[8] = UPPER("De estación: " + v.getaData("SELECT z7.estacion FROM cestaciones AS z7 WHERE z7.idestacion = '" + percances[8] + "'")).ToString();
                        percances[9] = UPPER("A estación: " + v.getaData("SELECT z8.estacion FROM cestaciones AS z8 WHERE z8.idestacion = '" + percances[9] + "'")).ToString();
                        percances[10] = UPPER("Económico recuperado: " + v.getaData("SELECT CONCAT(z10.identificador, LPAD(z9.consecutivo, 4, '0')) FROM cunidades AS z9 INNER JOIN careas AS z10 ON z9.areafkcareas = z10.idarea WHERE z9.idunidad = '" + percances[10] + "'")).ToString();
                        percances[11] = UPPER("Estación: " + v.getaData("SELECT z11.estacion FROM cestaciones AS z11 WHERE z11.idestacion = '" + percances[11] + "'")).ToString();
                        percances[12] = UPPER("Síntesis de lo ocurrido: " + percances[12]);
                        percances[13] = UPPER("Descripción: " + percances[13]);
                        percances[14] = UPPER("Marca de vehículo: " + percances[14]);
                        percances[15] = UPPER("Año de vehículo: " + percances[15]);
                        percances[16] = UPPER("Placas de vehículo: " + percances[16]);
                        percances[17] = UPPER("Nombre del conductor: " + percances[17]);
                        percances[18] = UPPER("Teléfono del conductor: " + percances[18]);
                        percances[19] = UPPER("Domicilio del conductor: " + percances[19]);
                        if (Convert.ToInt32(percances[20]) == 0)
                            percances[20] = percances[21] = percances[22] = "";
                        percances[20] = UPPER("Número de reporte del seguro: " + percances[20]);
                        percances[21] = UPPER("Hora de otorgamiento: " + percances[21]);
                        percances[22] = UPPER("Hora de llegada del seguro: " + percances[22]);
                        percances[23] = UPPER("Nombre del ajustador: " + percances[23]);
                        percances[24] = UPPER("Solución: " + percances[24]);
                        if (Convert.ToInt32(percances[25]) == 0)
                            percances[25] = "";
                        percances[25] = UPPER("Número de acta: " + percances[25]);
                        percances[26] = UPPER("Supervisor: " + v.getaData("SELECT CONCAT(z12.ApPaterno, ' ', z12.ApMaterno, ' ', z12.nombres) FROM cpersonal AS z12 WHERE z12.idPersona = '" + percances[26] + "'")).ToString();
                        if (Convert.ToInt32(percances[27]) == 0)
                            percances[27] = "";
                        percances[27] = UPPER("Unidad de asistencia médica: " + percances[27]);
                        percances[28] = UPPER("Perteneciente de asistencia médica: " + percances[28]);
                        percances[29] = UPPER("Nombre del responsable médico: " + percances[29]);
                        percances[30] = UPPER("En caso de lesionados: " + percances[30]);
                        percances[31] = UPPER("Comentarios: " + percances[31]);
                        percances[32] = UPPER(percances[32]);
                        y1 = 120;
                        CrearAntesDespuesLabels();
                        mitad1mitad2(percances);
                        CenterToParent();
                        y1 = null;
                        break;
                    default:
                        Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        bool scroll = false;
        string UPPER(string str)
        {
            return str.ToUpper();
        }
        public string[] MetodoBurbuja(string[] vector)
        {
            string t;
            for (int a = 1; a < vector.Length; a++)
                for (int b = vector.Length - 1; b >= a; b--)
                {
                    string temp = vector[b - 1].Substring(vector[b - 1].Length - 7);
                    int primero = Convert.ToInt32(temp);
                    temp = vector[b].Substring(vector[b].Length - 7);
                    int segundo = Convert.ToInt32(temp);
                    if (primero > segundo)
                    {
                        t = vector[b - 1];
                        vector[b - 1] = vector[b];
                        vector[b] = t;
                    }
                }
            return vector;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        void cerrar()
        {
            gbadd.Controls.Clear();
        }

    }
}