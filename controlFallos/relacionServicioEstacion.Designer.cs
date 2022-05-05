namespace controlFallos
{
    partial class relacionServicioEstacion
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
            this.pGeneral = new System.Windows.Forms.Panel();
            this.dgvrelaciones = new System.Windows.Forms.DataGridView();
            this.idrelacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.giro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvestacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuariofkcpersonal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGrid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvidservicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvidestacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pdelete = new System.Windows.Forms.Panel();
            this.lbldelete = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.gbrelacion = new System.Windows.Forms.GroupBox();
            this.pGiros = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.label63 = new System.Windows.Forms.Label();
            this.pCancel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.label62 = new System.Windows.Forms.Label();
            this.cbxgetestacion = new System.Windows.Forms.ComboBox();
            this.cbxgetservicio = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvrelaciones)).BeginInit();
            this.pdelete.SuspendLayout();
            this.gbrelacion.SuspendLayout();
            this.pGiros.SuspendLayout();
            this.pCancel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(805, 35);
            this.panel1.TabIndex = 2;
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
            this.button1.Location = new System.Drawing.Point(766, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 31);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(222, 0);
            this.lbltitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(243, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Relación Estación - Servicio";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // pGeneral
            // 
            this.pGeneral.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pGeneral.Controls.Add(this.dgvrelaciones);
            this.pGeneral.Controls.Add(this.pdelete);
            this.pGeneral.Controls.Add(this.gbrelacion);
            this.pGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGeneral.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pGeneral.Location = new System.Drawing.Point(0, 35);
            this.pGeneral.Name = "pGeneral";
            this.pGeneral.Size = new System.Drawing.Size(805, 649);
            this.pGeneral.TabIndex = 3;
            // 
            // dgvrelaciones
            // 
            this.dgvrelaciones.AllowUserToAddRows = false;
            this.dgvrelaciones.AllowUserToDeleteRows = false;
            this.dgvrelaciones.AllowUserToResizeColumns = false;
            this.dgvrelaciones.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvrelaciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvrelaciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvrelaciones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvrelaciones.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvrelaciones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvrelaciones.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvrelaciones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvrelaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvrelaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idrelacion,
            this.giro,
            this.dgvestacion,
            this.usuariofkcpersonal,
            this.statusDataGrid,
            this.dgvidservicio,
            this.dgvidestacion});
            this.dgvrelaciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvrelaciones.EnableHeadersVisualStyles = false;
            this.dgvrelaciones.Location = new System.Drawing.Point(0, 321);
            this.dgvrelaciones.MultiSelect = false;
            this.dgvrelaciones.Name = "dgvrelaciones";
            this.dgvrelaciones.ReadOnly = true;
            this.dgvrelaciones.RowHeadersVisible = false;
            this.dgvrelaciones.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvrelaciones.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvrelaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvrelaciones.Size = new System.Drawing.Size(801, 324);
            this.dgvrelaciones.TabIndex = 37;
            this.dgvrelaciones.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvrelaciones_CellDoubleClick);
            this.dgvrelaciones.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvrelaciones_CellFormatting);
            // 
            // idrelacion
            // 
            this.idrelacion.HeaderText = "idgiro";
            this.idrelacion.Name = "idrelacion";
            this.idrelacion.ReadOnly = true;
            this.idrelacion.Visible = false;
            // 
            // giro
            // 
            this.giro.HeaderText = "SERVICIO";
            this.giro.Name = "giro";
            this.giro.ReadOnly = true;
            this.giro.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvestacion
            // 
            this.dgvestacion.HeaderText = "ESTACIÓN";
            this.dgvestacion.Name = "dgvestacion";
            this.dgvestacion.ReadOnly = true;
            // 
            // usuariofkcpersonal
            // 
            this.usuariofkcpersonal.HeaderText = "USUARIO QUE DIÓ DE ALTA";
            this.usuariofkcpersonal.Name = "usuariofkcpersonal";
            this.usuariofkcpersonal.ReadOnly = true;
            this.usuariofkcpersonal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // statusDataGrid
            // 
            this.statusDataGrid.HeaderText = "ESTATUS";
            this.statusDataGrid.Name = "statusDataGrid";
            this.statusDataGrid.ReadOnly = true;
            this.statusDataGrid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvidservicio
            // 
            this.dgvidservicio.HeaderText = "idservicio";
            this.dgvidservicio.Name = "dgvidservicio";
            this.dgvidservicio.ReadOnly = true;
            this.dgvidservicio.Visible = false;
            // 
            // dgvidestacion
            // 
            this.dgvidestacion.HeaderText = "idestacion";
            this.dgvidestacion.Name = "dgvidestacion";
            this.dgvidestacion.ReadOnly = true;
            this.dgvidestacion.Visible = false;
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelete);
            this.pdelete.Controls.Add(this.btndelete);
            this.pdelete.Location = new System.Drawing.Point(108, 216);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(99, 91);
            this.pdelete.TabIndex = 15;
            this.pdelete.Visible = false;
            // 
            // lbldelete
            // 
            this.lbldelete.AutoSize = true;
            this.lbldelete.Font = new System.Drawing.Font("Garamond", 15.75F);
            this.lbldelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbldelete.Location = new System.Drawing.Point(0, 59);
            this.lbldelete.Name = "lbldelete";
            this.lbldelete.Size = new System.Drawing.Size(98, 24);
            this.lbldelete.TabIndex = 26;
            this.lbldelete.Text = "Desactivar";
            // 
            // btndelete
            // 
            this.btndelete.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelete.FlatAppearance.BorderSize = 0;
            this.btndelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btndelete.Location = new System.Drawing.Point(24, 9);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(50, 50);
            this.btndelete.TabIndex = 25;
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // gbrelacion
            // 
            this.gbrelacion.Controls.Add(this.pGiros);
            this.gbrelacion.Controls.Add(this.pCancel);
            this.gbrelacion.Controls.Add(this.lblsave);
            this.gbrelacion.Controls.Add(this.btnsave);
            this.gbrelacion.Controls.Add(this.label62);
            this.gbrelacion.Controls.Add(this.cbxgetestacion);
            this.gbrelacion.Controls.Add(this.cbxgetservicio);
            this.gbrelacion.Controls.Add(this.label1);
            this.gbrelacion.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbrelacion.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.gbrelacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbrelacion.Location = new System.Drawing.Point(0, 0);
            this.gbrelacion.Name = "gbrelacion";
            this.gbrelacion.Size = new System.Drawing.Size(801, 315);
            this.gbrelacion.TabIndex = 8;
            this.gbrelacion.TabStop = false;
            this.gbrelacion.Text = "Agregar Relación";
            // 
            // pGiros
            // 
            this.pGiros.Controls.Add(this.button6);
            this.pGiros.Controls.Add(this.label63);
            this.pGiros.Location = new System.Drawing.Point(598, 64);
            this.pGiros.Name = "pGiros";
            this.pGiros.Size = new System.Drawing.Size(134, 98);
            this.pGiros.TabIndex = 18;
            // 
            // button6
            // 
            this.button6.BackgroundImage = global::controlFallos.Properties.Resources._goto;
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(49, 8);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(40, 40);
            this.button6.TabIndex = 0;
            this.button6.TabStop = false;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.label63.Location = new System.Drawing.Point(9, 51);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(113, 42);
            this.label63.TabIndex = 0;
            this.label63.Text = "Ir a Catálogo\r\nde Estaciones";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pCancel
            // 
            this.pCancel.Controls.Add(this.label2);
            this.pCancel.Controls.Add(this.btnCancelEmpresa);
            this.pCancel.Location = new System.Drawing.Point(529, 211);
            this.pCancel.Name = "pCancel";
            this.pCancel.Size = new System.Drawing.Size(148, 100);
            this.pCancel.TabIndex = 14;
            this.pCancel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 15.75F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(44, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 24);
            this.label2.TabIndex = 26;
            this.label2.Text = "Nuevo";
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(51, 14);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(50, 50);
            this.btnCancelEmpresa.TabIndex = 27;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblsave.Location = new System.Drawing.Point(349, 279);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(72, 21);
            this.lblsave.TabIndex = 17;
            this.lblsave.Text = "Guardar";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnsave.Location = new System.Drawing.Point(360, 226);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 16;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.label62.Location = new System.Drawing.Point(73, 31);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(72, 21);
            this.label62.TabIndex = 4;
            this.label62.Text = "Servicio:";
            // 
            // cbxgetestacion
            // 
            this.cbxgetestacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbxgetestacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxgetestacion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxgetestacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxgetestacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxgetestacion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxgetestacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbxgetestacion.FormattingEnabled = true;
            this.cbxgetestacion.ItemHeight = 20;
            this.cbxgetestacion.Location = new System.Drawing.Point(169, 120);
            this.cbxgetestacion.MaxDropDownItems = 15;
            this.cbxgetestacion.Name = "cbxgetestacion";
            this.cbxgetestacion.Size = new System.Drawing.Size(411, 26);
            this.cbxgetestacion.TabIndex = 7;
            this.cbxgetestacion.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // cbxgetservicio
            // 
            this.cbxgetservicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbxgetservicio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxgetservicio.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxgetservicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxgetservicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxgetservicio.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxgetservicio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbxgetservicio.FormattingEnabled = true;
            this.cbxgetservicio.ItemHeight = 20;
            this.cbxgetservicio.Location = new System.Drawing.Point(169, 31);
            this.cbxgetservicio.MaxDropDownItems = 15;
            this.cbxgetservicio.Name = "cbxgetservicio";
            this.cbxgetservicio.Size = new System.Drawing.Size(411, 26);
            this.cbxgetservicio.TabIndex = 5;
            this.cbxgetservicio.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.label1.Location = new System.Drawing.Point(73, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "Estación:";
            // 
            // relacionServicioEstacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(805, 684);
            this.Controls.Add(this.pGeneral);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "relacionServicioEstacion";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvrelaciones)).EndInit();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            this.gbrelacion.ResumeLayout(false);
            this.gbrelacion.PerformLayout();
            this.pGiros.ResumeLayout(false);
            this.pGiros.PerformLayout();
            this.pCancel.ResumeLayout(false);
            this.pCancel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Panel pGeneral;
        public System.Windows.Forms.ComboBox cbxgetservicio;
        private System.Windows.Forms.Label label62;
        public System.Windows.Forms.ComboBox cbxgetestacion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbrelacion;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label lbldelete;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Panel pCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Panel pGiros;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.DataGridView dgvrelaciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn idrelacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn giro;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvestacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuariofkcpersonal;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvidservicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvidestacion;
    }
}