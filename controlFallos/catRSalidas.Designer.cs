﻿namespace controlFallos
{
    partial class catRSalidas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpFechaDe = new System.Windows.Forms.DateTimePicker();
            this.cbFecha = new System.Windows.Forms.CheckBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.dtpFechaA = new System.Windows.Forms.DateTimePicker();
            this.cmbEmpresa = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtcodigo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.buttonExcel = new System.Windows.Forms.Button();
            this.pictureBoxExcelLoad = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.cmbMes = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.dgvSalidas = new System.Windows.Forms.DataGridView();
            this.gvimprimir = new System.Windows.Forms.DataGridView();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.idrefaccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refaccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tota = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.simbolo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masinfo = new System.Windows.Forms.DataGridViewLinkColumn();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcelLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalidas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvimprimir)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 27);
            this.panel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Crimson;
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources.delete;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(767, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 27);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Cerrar);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Crimson;
            this.label1.Font = new System.Drawing.Font("Garamond", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(264, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total De Salidas";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpFechaDe);
            this.groupBox1.Controls.Add(this.cbFecha);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Controls.Add(this.dtpFechaA);
            this.groupBox1.Controls.Add(this.cmbEmpresa);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.txtcodigo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.buttonExcel);
            this.groupBox1.Controls.Add(this.pictureBoxExcelLoad);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.cmbMes);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(756, 239);
            this.groupBox1.TabIndex = 241;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtros de busqueda:";
            // 
            // dtpFechaDe
            // 
            this.dtpFechaDe.CalendarFont = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaDe.Enabled = false;
            this.dtpFechaDe.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaDe.Location = new System.Drawing.Point(415, 74);
            this.dtpFechaDe.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.dtpFechaDe.Name = "dtpFechaDe";
            this.dtpFechaDe.Size = new System.Drawing.Size(252, 25);
            this.dtpFechaDe.TabIndex = 261;
            // 
            // cbFecha
            // 
            this.cbFecha.AutoSize = true;
            this.cbFecha.Font = new System.Drawing.Font("Garamond", 12F);
            this.cbFecha.Location = new System.Drawing.Point(445, 46);
            this.cbFecha.Name = "cbFecha";
            this.cbFecha.Size = new System.Drawing.Size(138, 22);
            this.cbFecha.TabIndex = 260;
            this.cbFecha.Text = "Rango De Fechas";
            this.cbFecha.UseVisualStyleBackColor = true;
            this.cbFecha.CheckedChanged += new System.EventHandler(this.cbFecha_CheckedChanged);
            this.cbFecha.Click += new System.EventHandler(this.Buscar_Fecha);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Font = new System.Drawing.Font("Garamond", 12F);
            this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label31.Location = new System.Drawing.Point(378, 79);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(31, 18);
            this.label31.TabIndex = 258;
            this.label31.Text = "De:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Font = new System.Drawing.Font("Garamond", 12F);
            this.label32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label32.Location = new System.Drawing.Point(378, 111);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(23, 18);
            this.label32.TabIndex = 259;
            this.label32.Text = "A:";
            // 
            // dtpFechaA
            // 
            this.dtpFechaA.CalendarFont = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaA.Enabled = false;
            this.dtpFechaA.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaA.Location = new System.Drawing.Point(415, 106);
            this.dtpFechaA.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.dtpFechaA.Name = "dtpFechaA";
            this.dtpFechaA.Size = new System.Drawing.Size(252, 25);
            this.dtpFechaA.TabIndex = 257;
            // 
            // cmbEmpresa
            // 
            this.cmbEmpresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbEmpresa.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEmpresa.DropDownHeight = 100;
            this.cmbEmpresa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbEmpresa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEmpresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbEmpresa.FormattingEnabled = true;
            this.cmbEmpresa.IntegralHeight = false;
            this.cmbEmpresa.ItemHeight = 20;
            this.cmbEmpresa.Location = new System.Drawing.Point(85, 111);
            this.cmbEmpresa.Name = "cmbEmpresa";
            this.cmbEmpresa.Size = new System.Drawing.Size(259, 26);
            this.cmbEmpresa.TabIndex = 256;
            this.cmbEmpresa.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.PintarCombo);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 12F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(11, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 18);
            this.label4.TabIndex = 255;
            this.label4.Text = "Empresa:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label27.Location = new System.Drawing.Point(95, 54);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(229, 9);
            this.label27.TabIndex = 252;
            this.label27.Text = "________________________________________________________";
            // 
            // txtcodigo
            // 
            this.txtcodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtcodigo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtcodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtcodigo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcodigo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtcodigo.Location = new System.Drawing.Point(97, 37);
            this.txtcodigo.MaxLength = 10;
            this.txtcodigo.Name = "txtcodigo";
            this.txtcodigo.ShortcutsEnabled = false;
            this.txtcodigo.Size = new System.Drawing.Size(227, 18);
            this.txtcodigo.TabIndex = 246;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 21);
            this.label3.TabIndex = 245;
            this.label3.Text = "Codigo:";
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.Transparent;
            this.btnBuscar.BackgroundImage = global::controlFallos.Properties.Resources.xmag_search_find_export_locate_5984;
            this.btnBuscar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBuscar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnBuscar.Location = new System.Drawing.Point(415, 137);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(31, 32);
            this.btnBuscar.TabIndex = 243;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.Buscar);
            // 
            // buttonExcel
            // 
            this.buttonExcel.AutoSize = true;
            this.buttonExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonExcel.BackgroundImage = global::controlFallos.Properties.Resources.excel;
            this.buttonExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonExcel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonExcel.FlatAppearance.BorderSize = 0;
            this.buttonExcel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonExcel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExcel.Font = new System.Drawing.Font("Garamond", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExcel.Location = new System.Drawing.Point(505, 134);
            this.buttonExcel.Name = "buttonExcel";
            this.buttonExcel.Size = new System.Drawing.Size(35, 35);
            this.buttonExcel.TabIndex = 230;
            this.buttonExcel.UseVisualStyleBackColor = false;
            this.buttonExcel.Visible = false;
            this.buttonExcel.Click += new System.EventHandler(this.Excel);
            // 
            // pictureBoxExcelLoad
            // 
            this.pictureBoxExcelLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxExcelLoad.Location = new System.Drawing.Point(505, 134);
            this.pictureBoxExcelLoad.Name = "pictureBoxExcelLoad";
            this.pictureBoxExcelLoad.Size = new System.Drawing.Size(35, 35);
            this.pictureBoxExcelLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxExcelLoad.TabIndex = 232;
            this.pictureBoxExcelLoad.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label18.Font = new System.Drawing.Font("Garamond", 9.75F);
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label18.Location = new System.Drawing.Point(400, 175);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 14);
            this.label18.TabIndex = 244;
            this.label18.Text = "BUSCAR";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label35.Location = new System.Drawing.Point(486, 174);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(75, 14);
            this.label35.TabIndex = 231;
            this.label35.Text = "EXPORTAR";
            this.label35.Visible = false;
            // 
            // cmbMes
            // 
            this.cmbMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbMes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMes.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbMes.FormattingEnabled = true;
            this.cmbMes.Location = new System.Drawing.Point(85, 75);
            this.cmbMes.Name = "cmbMes";
            this.cmbMes.Size = new System.Drawing.Size(259, 25);
            this.cmbMes.TabIndex = 241;
            this.cmbMes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.PintarCombo);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label25.Location = new System.Drawing.Point(11, 79);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(38, 18);
            this.label25.TabIndex = 240;
            this.label25.Text = "Mes:";
            // 
            // dgvSalidas
            // 
            this.dgvSalidas.AllowUserToAddRows = false;
            this.dgvSalidas.AllowUserToDeleteRows = false;
            this.dgvSalidas.AllowUserToResizeColumns = false;
            this.dgvSalidas.AllowUserToResizeRows = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(22)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSalidas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvSalidas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSalidas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSalidas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvSalidas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSalidas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSalidas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvSalidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSalidas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idrefaccion,
            this.Codigo,
            this.refaccion,
            this.tota,
            this.simbolo,
            this.masinfo});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSalidas.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvSalidas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSalidas.EnableHeadersVisualStyles = false;
            this.dgvSalidas.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvSalidas.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.dgvSalidas.Location = new System.Drawing.Point(12, 290);
            this.dgvSalidas.MultiSelect = false;
            this.dgvSalidas.Name = "dgvSalidas";
            this.dgvSalidas.ReadOnly = true;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSalidas.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvSalidas.RowHeadersVisible = false;
            this.dgvSalidas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.Crimson;
            this.dgvSalidas.RowsDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvSalidas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSalidas.ShowCellErrors = false;
            this.dgvSalidas.ShowCellToolTips = false;
            this.dgvSalidas.ShowEditingIcon = false;
            this.dgvSalidas.ShowRowErrors = false;
            this.dgvSalidas.Size = new System.Drawing.Size(766, 238);
            this.dgvSalidas.TabIndex = 255;
            this.dgvSalidas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSalidas_CellContentClick);
            // 
            // gvimprimir
            // 
            this.gvimprimir.AllowUserToAddRows = false;
            this.gvimprimir.AllowUserToDeleteRows = false;
            this.gvimprimir.AllowUserToResizeColumns = false;
            this.gvimprimir.AllowUserToResizeRows = false;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(22)))));
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvimprimir.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle18;
            this.gvimprimir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gvimprimir.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvimprimir.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvimprimir.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gvimprimir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvimprimir.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvimprimir.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.gvimprimir.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvimprimir.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvimprimir.DefaultCellStyle = dataGridViewCellStyle20;
            this.gvimprimir.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gvimprimir.EnableHeadersVisualStyles = false;
            this.gvimprimir.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gvimprimir.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.gvimprimir.Location = new System.Drawing.Point(12, 534);
            this.gvimprimir.MultiSelect = false;
            this.gvimprimir.Name = "gvimprimir";
            this.gvimprimir.ReadOnly = true;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvimprimir.RowHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.gvimprimir.RowHeadersVisible = false;
            this.gvimprimir.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.Color.Crimson;
            this.gvimprimir.RowsDefaultCellStyle = dataGridViewCellStyle22;
            this.gvimprimir.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvimprimir.ShowCellErrors = false;
            this.gvimprimir.ShowCellToolTips = false;
            this.gvimprimir.ShowEditingIcon = false;
            this.gvimprimir.ShowRowErrors = false;
            this.gvimprimir.Size = new System.Drawing.Size(233, 213);
            this.gvimprimir.TabIndex = 256;
            this.gvimprimir.Visible = false;
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.BackColor = System.Drawing.Color.Transparent;
            this.btnSiguiente.BackgroundImage = global::controlFallos.Properties.Resources.next;
            this.btnSiguiente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSiguiente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSiguiente.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnSiguiente.FlatAppearance.BorderSize = 0;
            this.btnSiguiente.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSiguiente.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguiente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnSiguiente.Location = new System.Drawing.Point(415, 534);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(31, 32);
            this.btnSiguiente.TabIndex = 258;
            this.btnSiguiente.UseVisualStyleBackColor = false;
            this.btnSiguiente.Click += new System.EventHandler(this.Siguiente);
            // 
            // btnAnterior
            // 
            this.btnAnterior.BackColor = System.Drawing.Color.Transparent;
            this.btnAnterior.BackgroundImage = global::controlFallos.Properties.Resources.previous;
            this.btnAnterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAnterior.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnterior.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnAnterior.FlatAppearance.BorderSize = 0;
            this.btnAnterior.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAnterior.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnterior.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnAnterior.Location = new System.Drawing.Point(216, 534);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(31, 32);
            this.btnAnterior.TabIndex = 257;
            this.btnAnterior.UseVisualStyleBackColor = false;
            this.btnAnterior.Click += new System.EventHandler(this.Anterior);
            // 
            // idrefaccion
            // 
            this.idrefaccion.HeaderText = "idRefaccion";
            this.idrefaccion.Name = "idrefaccion";
            this.idrefaccion.ReadOnly = true;
            this.idrefaccion.Visible = false;
            this.idrefaccion.Width = 104;
            // 
            // Codigo
            // 
            this.Codigo.HeaderText = "CODIGO";
            this.Codigo.Name = "Codigo";
            this.Codigo.ReadOnly = true;
            this.Codigo.Width = 114;
            // 
            // refaccion
            // 
            this.refaccion.HeaderText = "REFACCION";
            this.refaccion.Name = "refaccion";
            this.refaccion.ReadOnly = true;
            this.refaccion.Width = 144;
            // 
            // tota
            // 
            this.tota.HeaderText = "TOTAL";
            this.tota.Name = "tota";
            this.tota.ReadOnly = true;
            this.tota.Width = 98;
            // 
            // simbolo
            // 
            this.simbolo.HeaderText = "SIMBOLO";
            this.simbolo.Name = "simbolo";
            this.simbolo.ReadOnly = true;
            this.simbolo.Width = 120;
            // 
            // masinfo
            // 
            this.masinfo.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.masinfo.DefaultCellStyle = dataGridViewCellStyle14;
            this.masinfo.HeaderText = "MÁS INFORMACÓN";
            this.masinfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.masinfo.Name = "masinfo";
            this.masinfo.ReadOnly = true;
            this.masinfo.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.masinfo.Width = 171;
            // 
            // catRSalidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(800, 636);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.btnAnterior);
            this.Controls.Add(this.gvimprimir);
            this.Controls.Add(this.dgvSalidas);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "catRSalidas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "catRSalidas";
            this.Load += new System.EventHandler(this.CargarSalidas);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcelLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalidas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvimprimir)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpFechaDe;
        private System.Windows.Forms.CheckBox cbFecha;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.DateTimePicker dtpFechaA;
        private System.Windows.Forms.ComboBox cmbEmpresa;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtcodigo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button buttonExcel;
        private System.Windows.Forms.PictureBox pictureBoxExcelLoad;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox cmbMes;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.DataGridView dgvSalidas;
        private System.Windows.Forms.DataGridView gvimprimir;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.DataGridViewTextBoxColumn idrefaccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn refaccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn tota;
        private System.Windows.Forms.DataGridViewTextBoxColumn simbolo;
        private System.Windows.Forms.DataGridViewLinkColumn masinfo;
    }
}