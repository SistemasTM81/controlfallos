namespace controlFallos
{
    partial class catNiveles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbaddnivel = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbniveles = new System.Windows.Forms.GroupBox();
            this.dtniveles = new System.Windows.Forms.DataGridView();
            this.idnivel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pasilloo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.niv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuariofkcpersonal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idpasillodatgrid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pdelete = new System.Windows.Forms.Panel();
            this.lbldelpa = new System.Windows.Forms.Label();
            this.btndelpa = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cbpasillo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtnivel = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbaddnivel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbniveles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtniveles)).BeginInit();
            this.pdelete.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 27);
            this.panel1.TabIndex = 35;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources.delete;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(740, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 27);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(273, 3);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(175, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Niveles";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // gbaddnivel
            // 
            this.gbaddnivel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddnivel.Controls.Add(this.panel2);
            this.gbaddnivel.Controls.Add(this.pdelete);
            this.gbaddnivel.Controls.Add(this.label3);
            this.gbaddnivel.Controls.Add(this.label23);
            this.gbaddnivel.Controls.Add(this.cbpasillo);
            this.gbaddnivel.Controls.Add(this.label2);
            this.gbaddnivel.Controls.Add(this.pCancelar);
            this.gbaddnivel.Controls.Add(this.lblsavemp);
            this.gbaddnivel.Controls.Add(this.btnsavemp);
            this.gbaddnivel.Controls.Add(this.txtnivel);
            this.gbaddnivel.Controls.Add(this.label21);
            this.gbaddnivel.Controls.Add(this.label22);
            this.gbaddnivel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbaddnivel.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddnivel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddnivel.Location = new System.Drawing.Point(0, 27);
            this.gbaddnivel.Name = "gbaddnivel";
            this.gbaddnivel.Size = new System.Drawing.Size(776, 609);
            this.gbaddnivel.TabIndex = 36;
            this.gbaddnivel.TabStop = false;
            this.gbaddnivel.Text = "Agregar Nivel";
            this.gbaddnivel.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gbniveles);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(3, 355);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(770, 251);
            this.panel2.TabIndex = 70;
            // 
            // gbniveles
            // 
            this.gbniveles.Controls.Add(this.dtniveles);
            this.gbniveles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbniveles.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbniveles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbniveles.Location = new System.Drawing.Point(0, 0);
            this.gbniveles.Name = "gbniveles";
            this.gbniveles.Size = new System.Drawing.Size(770, 251);
            this.gbniveles.TabIndex = 69;
            this.gbniveles.TabStop = false;
            this.gbniveles.Text = "Consulta de Niveles";
            this.gbniveles.Visible = false;
            // 
            // dtniveles
            // 
            this.dtniveles.AllowUserToAddRows = false;
            this.dtniveles.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtniveles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtniveles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtniveles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dtniveles.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dtniveles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtniveles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtniveles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtniveles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtniveles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idnivel,
            this.Pasilloo,
            this.niv,
            this.usuariofkcpersonal,
            this.estatus,
            this.idpasillodatgrid});
            this.dtniveles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtniveles.EnableHeadersVisualStyles = false;
            this.dtniveles.Location = new System.Drawing.Point(3, 25);
            this.dtniveles.Name = "dtniveles";
            this.dtniveles.ReadOnly = true;
            this.dtniveles.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtniveles.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dtniveles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtniveles.Size = new System.Drawing.Size(764, 223);
            this.dtniveles.TabIndex = 0;
            this.dtniveles.TabStop = false;
            this.dtniveles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtniveles_CellDoubleClick);
            this.dtniveles.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dtniveles_CellFormatting);
            // 
            // idnivel
            // 
            this.idnivel.HeaderText = "idnivel";
            this.idnivel.Name = "idnivel";
            this.idnivel.ReadOnly = true;
            this.idnivel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.idnivel.Visible = false;
            // 
            // Pasilloo
            // 
            this.Pasilloo.HeaderText = "PASILLO";
            this.Pasilloo.Name = "Pasilloo";
            this.Pasilloo.ReadOnly = true;
            this.Pasilloo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // niv
            // 
            this.niv.HeaderText = "NIVEL";
            this.niv.Name = "niv";
            this.niv.ReadOnly = true;
            this.niv.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // usuariofkcpersonal
            // 
            this.usuariofkcpersonal.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.usuariofkcpersonal.Name = "usuariofkcpersonal";
            this.usuariofkcpersonal.ReadOnly = true;
            this.usuariofkcpersonal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // estatus
            // 
            this.estatus.HeaderText = "ESTATUS";
            this.estatus.Name = "estatus";
            this.estatus.ReadOnly = true;
            this.estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // idpasillodatgrid
            // 
            this.idpasillodatgrid.HeaderText = "idpasillo";
            this.idpasillodatgrid.Name = "idpasillodatgrid";
            this.idpasillodatgrid.ReadOnly = true;
            this.idpasillodatgrid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.idpasillodatgrid.Visible = false;
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelpa);
            this.pdelete.Controls.Add(this.btndelpa);
            this.pdelete.Location = new System.Drawing.Point(145, 207);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(137, 65);
            this.pdelete.TabIndex = 0;
            this.pdelete.Visible = false;
            // 
            // lbldelpa
            // 
            this.lbldelpa.AutoSize = true;
            this.lbldelpa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelpa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelpa.Location = new System.Drawing.Point(31, 40);
            this.lbldelpa.Name = "lbldelpa";
            this.lbldelpa.Size = new System.Drawing.Size(76, 18);
            this.lbldelpa.TabIndex = 0;
            this.lbldelpa.Text = "Desactivar";
            // 
            // btndelpa
            // 
            this.btndelpa.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelpa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelpa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelpa.FlatAppearance.BorderSize = 0;
            this.btndelpa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelpa.Location = new System.Drawing.Point(52, 3);
            this.btndelpa.Name = "btndelpa";
            this.btndelpa.Size = new System.Drawing.Size(35, 35);
            this.btndelpa.TabIndex = 0;
            this.btndelpa.UseVisualStyleBackColor = true;
            this.btndelpa.Click += new System.EventHandler(this.btndelpa_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Crimson;
            this.label3.Location = new System.Drawing.Point(301, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 18);
            this.label3.TabIndex = 66;
            this.label3.Text = "Nota:";
            this.label3.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(320, 292);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(434, 17);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // cbpasillo
            // 
            this.cbpasillo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbpasillo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbpasillo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbpasillo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbpasillo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbpasillo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbpasillo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbpasillo.FormattingEnabled = true;
            this.cbpasillo.Location = new System.Drawing.Point(265, 81);
            this.cbpasillo.MaxDropDownItems = 50;
            this.cbpasillo.Name = "cbpasillo";
            this.cbpasillo.Size = new System.Drawing.Size(245, 26);
            this.cbpasillo.TabIndex = 1;
            this.cbpasillo.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbpasillo_DrawItem);
            this.cbpasillo.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "Pasillo:";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(428, 209);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(114, 63);
            this.pCancelar.TabIndex = 0;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(31, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nuevo";
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(40, 3);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(35, 35);
            this.btnCancelEmpresa.TabIndex = 0;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsavemp.Location = new System.Drawing.Point(317, 247);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(60, 18);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Guardar";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(327, 209);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(35, 35);
            this.btnsavemp.TabIndex = 3;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtnivel
            // 
            this.txtnivel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtnivel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtnivel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtnivel.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnivel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtnivel.Location = new System.Drawing.Point(265, 145);
            this.txtnivel.MaxLength = 20;
            this.txtnivel.Name = "txtnivel";
            this.txtnivel.ShortcutsEnabled = false;
            this.txtnivel.Size = new System.Drawing.Size(238, 18);
            this.txtnivel.TabIndex = 2;
            this.txtnivel.TextChanged += new System.EventHandler(this.getCambios);
            this.txtnivel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnivel_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(175, 141);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(60, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Nivel:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(263, 158);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(241, 9);
            this.label22.TabIndex = 1;
            this.label22.Text = "___________________________________________________________";
            // 
            // catNiveles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(776, 636);
            this.Controls.Add(this.gbaddnivel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Garamond", 12F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "catNiveles";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catNiveles";
            this.Load += new System.EventHandler(this.catNiveles_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbaddnivel.ResumeLayout(false);
            this.gbaddnivel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.gbniveles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtniveles)).EndInit();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.GroupBox gbaddnivel;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label lbldelpa;
        private System.Windows.Forms.Button btndelpa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cbpasillo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtnivel;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox gbniveles;
        private System.Windows.Forms.DataGridView dtniveles;
        private System.Windows.Forms.DataGridViewTextBoxColumn idnivel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pasilloo;
        private System.Windows.Forms.DataGridViewTextBoxColumn niv;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuariofkcpersonal;
        private System.Windows.Forms.DataGridViewTextBoxColumn estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn idpasillodatgrid;
        private System.Windows.Forms.Panel panel2;
    }
}