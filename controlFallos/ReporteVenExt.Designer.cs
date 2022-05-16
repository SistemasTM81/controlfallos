namespace controlFallos
{
    partial class ReporteVenExt
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtcodigo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.buttonExcel = new System.Windows.Forms.Button();
            this.pictureBoxExcelLoad = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.cmbMes = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.dtpFechaDe = new System.Windows.Forms.DateTimePicker();
            this.cmbBuscarUnidad = new System.Windows.Forms.ComboBox();
            this.cbFecha = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.dtpFechaA = new System.Windows.Forms.DateTimePicker();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcelLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 27);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Crimson;
            this.label2.Font = new System.Drawing.Font("Garamond", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(241, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Reportes De Venta Externa";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Crimson;
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources.delete;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(780, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 27);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Cerrar);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.txtcodigo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.buttonExcel);
            this.groupBox1.Controls.Add(this.pictureBoxExcelLoad);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.cmbMes);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.dtpFechaDe);
            this.groupBox1.Controls.Add(this.cmbBuscarUnidad);
            this.groupBox1.Controls.Add(this.cbFecha);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Controls.Add(this.dtpFechaA);
            this.groupBox1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(29, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(742, 237);
            this.groupBox1.TabIndex = 241;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtros de busqueda:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label27.Location = new System.Drawing.Point(92, 57);
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
            this.txtcodigo.Location = new System.Drawing.Point(95, 39);
            this.txtcodigo.MaxLength = 10;
            this.txtcodigo.Name = "txtcodigo";
            this.txtcodigo.ShortcutsEnabled = false;
            this.txtcodigo.Size = new System.Drawing.Size(227, 18);
            this.txtcodigo.TabIndex = 246;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 21);
            this.label5.TabIndex = 245;
            this.label5.Text = "Codigo:";
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
            this.btnBuscar.Location = new System.Drawing.Point(463, 154);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(31, 32);
            this.btnBuscar.TabIndex = 243;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.buscar);
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
            this.buttonExcel.Location = new System.Drawing.Point(553, 151);
            this.buttonExcel.Name = "buttonExcel";
            this.buttonExcel.Size = new System.Drawing.Size(35, 35);
            this.buttonExcel.TabIndex = 230;
            this.buttonExcel.UseVisualStyleBackColor = false;
            this.buttonExcel.Visible = false;
            this.buttonExcel.Click += new System.EventHandler(this.Exportar);
            // 
            // pictureBoxExcelLoad
            // 
            this.pictureBoxExcelLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxExcelLoad.Location = new System.Drawing.Point(553, 151);
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
            this.label18.Location = new System.Drawing.Point(448, 192);
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
            this.label35.Location = new System.Drawing.Point(534, 191);
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
            this.cmbMes.Location = new System.Drawing.Point(84, 116);
            this.cmbMes.Name = "cmbMes";
            this.cmbMes.Size = new System.Drawing.Size(259, 25);
            this.cmbMes.TabIndex = 241;
            this.cmbMes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbDrawItem);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label25.Location = new System.Drawing.Point(10, 122);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(38, 18);
            this.label25.TabIndex = 240;
            this.label25.Text = "Mes:";
            // 
            // dtpFechaDe
            // 
            this.dtpFechaDe.CalendarFont = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaDe.Enabled = false;
            this.dtpFechaDe.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaDe.Location = new System.Drawing.Point(420, 63);
            this.dtpFechaDe.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.dtpFechaDe.Name = "dtpFechaDe";
            this.dtpFechaDe.Size = new System.Drawing.Size(252, 25);
            this.dtpFechaDe.TabIndex = 242;
            // 
            // cmbBuscarUnidad
            // 
            this.cmbBuscarUnidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbBuscarUnidad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbBuscarUnidad.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBuscarUnidad.DropDownHeight = 100;
            this.cmbBuscarUnidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBuscarUnidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBuscarUnidad.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBuscarUnidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbBuscarUnidad.FormattingEnabled = true;
            this.cmbBuscarUnidad.IntegralHeight = false;
            this.cmbBuscarUnidad.ItemHeight = 20;
            this.cmbBuscarUnidad.Location = new System.Drawing.Point(84, 82);
            this.cmbBuscarUnidad.Name = "cmbBuscarUnidad";
            this.cmbBuscarUnidad.Size = new System.Drawing.Size(259, 26);
            this.cmbBuscarUnidad.TabIndex = 234;
            this.cmbBuscarUnidad.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbDrawItem);
            // 
            // cbFecha
            // 
            this.cbFecha.AutoSize = true;
            this.cbFecha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbFecha.Font = new System.Drawing.Font("Garamond", 12F);
            this.cbFecha.Location = new System.Drawing.Point(450, 35);
            this.cbFecha.Name = "cbFecha";
            this.cbFecha.Size = new System.Drawing.Size(138, 22);
            this.cbFecha.TabIndex = 239;
            this.cbFecha.Text = "Rango De Fechas";
            this.cbFecha.UseVisualStyleBackColor = true;
            this.cbFecha.CheckedChanged += new System.EventHandler(this.cbFecha_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Garamond", 12F);
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(10, 86);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(38, 18);
            this.label15.TabIndex = 233;
            this.label15.Text = "Eco:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Font = new System.Drawing.Font("Garamond", 12F);
            this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label31.Location = new System.Drawing.Point(383, 68);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(31, 18);
            this.label31.TabIndex = 237;
            this.label31.Text = "De:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Font = new System.Drawing.Font("Garamond", 12F);
            this.label32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label32.Location = new System.Drawing.Point(383, 100);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(23, 18);
            this.label32.TabIndex = 238;
            this.label32.Text = "A:";
            // 
            // dtpFechaA
            // 
            this.dtpFechaA.CalendarFont = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaA.Enabled = false;
            this.dtpFechaA.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaA.Location = new System.Drawing.Point(420, 95);
            this.dtpFechaA.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.dtpFechaA.Name = "dtpFechaA";
            this.dtpFechaA.Size = new System.Drawing.Size(252, 25);
            this.dtpFechaA.TabIndex = 236;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(41, 295);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(240, 150);
            this.dataGridView2.TabIndex = 254;
            this.dataGridView2.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(22)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.dataGridView1.Location = new System.Drawing.Point(29, 285);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.Crimson;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(742, 238);
            this.dataGridView1.TabIndex = 253;
            // 
            // ReporteVenExt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(813, 540);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ReporteVenExt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReporteVenExt";
            this.Load += new System.EventHandler(this.ReporteVentaExt_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcelLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtcodigo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button buttonExcel;
        private System.Windows.Forms.PictureBox pictureBoxExcelLoad;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox cmbMes;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.DateTimePicker dtpFechaDe;
        private System.Windows.Forms.ComboBox cmbBuscarUnidad;
        private System.Windows.Forms.CheckBox cbFecha;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.DateTimePicker dtpFechaA;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}