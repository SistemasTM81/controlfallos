using System;
using System.Threading.Tasks;

namespace controlFallos
{
    partial class workdays
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
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(workdays));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pDias = new System.Windows.Forms.Panel();
            this.nupDurationWorkDay = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnsearchRol = new System.Windows.Forms.Button();
            this.lblfinaldate = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.dtpinitialDate = new System.Windows.Forms.DateTimePicker();
            this.cmbxTimeperiod = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblloadrol = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.cmbxRolService = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvcycles = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.backgroundPanel = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.groupBox2.SuspendLayout();
            this.pDias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupDurationWorkDay)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvcycles)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pDias);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.lblfinaldate);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.dtpinitialDate);
            this.groupBox2.Controls.Add(this.cmbxTimeperiod);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1216, 77);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Registro de Roles";
            // 
            // pDias
            // 
            this.pDias.Controls.Add(this.nupDurationWorkDay);
            this.pDias.Controls.Add(this.label1);
            this.pDias.Location = new System.Drawing.Point(445, 28);
            this.pDias.Name = "pDias";
            this.pDias.Size = new System.Drawing.Size(200, 23);
            this.pDias.TabIndex = 38;
            this.pDias.Visible = false;
            // 
            // nupDurationWorkDay
            // 
            this.nupDurationWorkDay.AutoSize = true;
            this.nupDurationWorkDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.nupDurationWorkDay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nupDurationWorkDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.nupDurationWorkDay.Location = new System.Drawing.Point(86, 0);
            this.nupDurationWorkDay.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nupDurationWorkDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupDurationWorkDay.Name = "nupDurationWorkDay";
            this.nupDurationWorkDay.Size = new System.Drawing.Size(48, 21);
            this.nupDurationWorkDay.TabIndex = 21;
            this.nupDurationWorkDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nupDurationWorkDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupDurationWorkDay.ValueChanged += new System.EventHandler(this.nupDurationWorkDay_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(38, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "Dias:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1092, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(121, 53);
            this.panel2.TabIndex = 37;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(54, 53);
            this.panel3.TabIndex = 37;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(0, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 18);
            this.label3.TabIndex = 36;
            this.label3.Text = "Nuevo";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::controlFallos.Properties.Resources._new;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 32);
            this.button1.TabIndex = 35;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnsearchRol);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(67, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(54, 53);
            this.panel1.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 18);
            this.label2.TabIndex = 36;
            this.label2.Text = "Buscar";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnsearchRol
            // 
            this.btnsearchRol.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsearchRol.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearchRol.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnsearchRol.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnsearchRol.FlatAppearance.BorderSize = 0;
            this.btnsearchRol.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnsearchRol.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnsearchRol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsearchRol.Image = ((System.Drawing.Image)(resources.GetObject("btnsearchRol.Image")));
            this.btnsearchRol.Location = new System.Drawing.Point(0, 0);
            this.btnsearchRol.Name = "btnsearchRol";
            this.btnsearchRol.Size = new System.Drawing.Size(54, 32);
            this.btnsearchRol.TabIndex = 35;
            this.btnsearchRol.UseVisualStyleBackColor = true;
            this.btnsearchRol.Click += new System.EventHandler(this.btnsearchRol_Click);
            // 
            // lblfinaldate
            // 
            this.lblfinaldate.AutoSize = true;
            this.lblfinaldate.Location = new System.Drawing.Point(894, 28);
            this.lblfinaldate.Name = "lblfinaldate";
            this.lblfinaldate.Size = new System.Drawing.Size(150, 18);
            this.lblfinaldate.TabIndex = 19;
            this.lblfinaldate.Text = "27 / diciembre / 2019";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(858, 28);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(23, 18);
            this.label17.TabIndex = 18;
            this.label17.Text = "A:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(614, 28);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(31, 18);
            this.label16.TabIndex = 16;
            this.label16.Text = "De:";
            // 
            // dtpinitialDate
            // 
            this.dtpinitialDate.CustomFormat = "dd/MMMM/yyyy";
            this.dtpinitialDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpinitialDate.Location = new System.Drawing.Point(654, 26);
            this.dtpinitialDate.Name = "dtpinitialDate";
            this.dtpinitialDate.Size = new System.Drawing.Size(171, 25);
            this.dtpinitialDate.TabIndex = 15;
            this.dtpinitialDate.ValueChanged += new System.EventHandler(this.dtpinitialDate_ValueChanged);
            // 
            // cmbxTimeperiod
            // 
            this.cmbxTimeperiod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbxTimeperiod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbxTimeperiod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxTimeperiod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbxTimeperiod.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxTimeperiod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbxTimeperiod.FormattingEnabled = true;
            this.cmbxTimeperiod.Items.AddRange(new object[] {
            "--SELECCIONE RANGO--",
            "DÍA",
            "SEMANA",
            "QUINCENA"});
            this.cmbxTimeperiod.Location = new System.Drawing.Point(239, 28);
            this.cmbxTimeperiod.Name = "cmbxTimeperiod";
            this.cmbxTimeperiod.Size = new System.Drawing.Size(200, 23);
            this.cmbxTimeperiod.TabIndex = 13;
            this.cmbxTimeperiod.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbxTimeperiod_DrawItem);
            this.cmbxTimeperiod.SelectedValueChanged += new System.EventHandler(this.cmbxTimeperiod_SelectedValueChanged);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(105, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(137, 21);
            this.label15.TabIndex = 14;
            this.label15.Text = "Periodo de Tiempo:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblloadrol);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.cmbxRolService);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.groupBox1.Location = new System.Drawing.Point(1223, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(685, 77);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registro de Roles";
            // 
            // lblloadrol
            // 
            this.lblloadrol.AutoSize = true;
            this.lblloadrol.ForeColor = System.Drawing.Color.Crimson;
            this.lblloadrol.Location = new System.Drawing.Point(439, 31);
            this.lblloadrol.Name = "lblloadrol";
            this.lblloadrol.Size = new System.Drawing.Size(0, 18);
            this.lblloadrol.TabIndex = 39;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(606, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(76, 53);
            this.panel4.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Location = new System.Drawing.Point(0, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 18);
            this.label4.TabIndex = 36;
            this.label4.Text = "Guardar";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = global::controlFallos.Properties.Resources.diskette__1_;
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 32);
            this.button2.TabIndex = 35;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmbxRolService
            // 
            this.cmbxRolService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbxRolService.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbxRolService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxRolService.Enabled = false;
            this.cmbxRolService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbxRolService.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxRolService.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbxRolService.FormattingEnabled = true;
            this.cmbxRolService.Items.AddRange(new object[] {
            "--SELECCIONE RANGO--",
            "DÍA",
            "SEMANA",
            "QUINCENA"});
            this.cmbxRolService.Location = new System.Drawing.Point(125, 28);
            this.cmbxRolService.Name = "cmbxRolService";
            this.cmbxRolService.Size = new System.Drawing.Size(288, 23);
            this.cmbxRolService.TabIndex = 13;
            this.cmbxRolService.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbxTimeperiod_DrawItem);
            this.cmbxRolService.SelectedValueChanged += new System.EventHandler(this.cmbxRolService_SelectedValueChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(21, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 21);
            this.label8.TabIndex = 14;
            this.label8.Text = "Servicio:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvcycles
            // 
            this.dgvcycles.AllowDrop = true;
            this.dgvcycles.AllowUserToAddRows = false;
            this.dgvcycles.AllowUserToDeleteRows = false;
            this.dgvcycles.AllowUserToResizeColumns = false;
            this.dgvcycles.AllowUserToResizeRows = false;
            this.dgvcycles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvcycles.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvcycles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvcycles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvcycles.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvcycles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvcycles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvcycles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvcycles.EnableHeadersVisualStyles = false;
            this.dgvcycles.GridColor = System.Drawing.Color.Crimson;
            this.dgvcycles.Location = new System.Drawing.Point(0, 0);
            this.dgvcycles.Name = "dgvcycles";
            this.dgvcycles.ReadOnly = true;
            this.dgvcycles.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvcycles.RowHeadersVisible = false;
            this.dgvcycles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            this.dgvcycles.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvcycles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvcycles.Size = new System.Drawing.Size(1904, 622);
            this.dgvcycles.StandardTab = true;
            this.dgvcycles.TabIndex = 36;
            this.dgvcycles.DataSourceChanged += new System.EventHandler(this.dgvcycles_DataSourceChanged);
            this.dgvcycles.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvcycles_CellContentClick);
            this.dgvcycles.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvcycles_ColumnAdded);
            this.dgvcycles.SelectionChanged += new System.EventHandler(this.dgvcycles_SelectionChanged);
            this.dgvcycles.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvcycles_DragDrop);
            this.dgvcycles.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvcycles_DragEnter);
            this.dgvcycles.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvcycles_DragOver);
            this.dgvcycles.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgvcycles_PreviewKeyDown);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.panel10);
            this.panel5.Controls.Add(this.backgroundPanel);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 703);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1908, 234);
            this.panel5.TabIndex = 37;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.panel12);
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel10.Location = new System.Drawing.Point(1825, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(81, 232);
            this.panel10.TabIndex = 7;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.label11);
            this.panel12.Controls.Add(this.button4);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel12.Location = new System.Drawing.Point(0, 139);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(81, 93);
            this.panel12.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label11.Location = new System.Drawing.Point(0, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 26);
            this.label11.TabIndex = 1;
            this.label11.Text = "Exportar";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button4
            // 
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Image = global::controlFallos.Properties.Resources.excelrol;
            this.button4.Location = new System.Drawing.Point(0, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(81, 64);
            this.button4.TabIndex = 0;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label5);
            this.panel11.Controls.Add(this.button3);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(81, 93);
            this.panel11.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Location = new System.Drawing.Point(0, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 26);
            this.label5.TabIndex = 1;
            this.label5.Text = "Imprimir";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Image = global::controlFallos.Properties.Resources.print;
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 64);
            this.button3.TabIndex = 0;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.AllowDrop = true;
            this.backgroundPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.backgroundPanel.Location = new System.Drawing.Point(580, 82);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Size = new System.Drawing.Size(702, 140);
            this.backgroundPanel.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1305, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 18);
            this.label9.TabIndex = 5;
            this.label9.Text = "Firma 14:00 Horas";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Location = new System.Drawing.Point(580, 52);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(702, 20);
            this.panel7.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(432, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(146, 18);
            this.label10.TabIndex = 3;
            this.label10.Text = "Posturero Vespertino:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1305, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 18);
            this.label7.TabIndex = 2;
            this.label7.Text = "Firma 05:30 Horas";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Location = new System.Drawing.Point(580, 21);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(702, 20);
            this.panel6.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(432, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Posturero Matutino:";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.groupBox2);
            this.panel8.Controls.Add(this.groupBox1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1908, 77);
            this.panel8.TabIndex = 38;
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel9.Controls.Add(this.dgvcycles);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 77);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1908, 626);
            this.panel9.TabIndex = 39;
            // 
            // workdays
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1908, 937);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel5);
            this.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "workdays";
            this.Text = "workdays";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pDias.ResumeLayout(false);
            this.pDias.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupDurationWorkDay)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvcycles)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        #region Events of Windows From Controls
        private void btnsearchRol_Click(object sender, System.EventArgs e)
        {
            searchworkday searchWork = new searchworkday(this);
            var DialogResult = searchWork.ShowDialog();
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                periodID = System.Convert.ToInt64(searchWork.dgvroles.CurrentRow.Cells[0].Value);
                if (!periodID.HasValue) { Owner.sendUser("Ocurrio Un Error al Obtener Los Datos. Intente de Nuevo", validaciones.MessageBoxTitle.Error); return; }
                var dataTimeRol = Owner.v.getaData("CALL getperiod(" + periodID.Value + ")").ToString().Split('|');
                dtpinitialDate.Value = System.Convert.ToDateTime(dataTimeRol[1]);
                cmbxTimeperiod.SelectedValue = dataTimeRol[0];
            }
        }
        private void cmbxTimeperiod_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (cmbxTimeperiod.SelectedIndex <= 0) { dtpinitialDate.Enabled = cmbxRolService.Enabled = false; cmbxRolService.DataSource = null; lblfinaldate.Text = string.Empty; dtpinitialDate.Value = System.DateTime.Today; return; }
            dtpinitialDate.Enabled = cmbxRolService.Enabled = true;
            pDias.Visible = cmbxTimeperiod.SelectedIndex == 1;
            addTime();
            Owner.v.iniCombos("SELECT * FROM rolServicesActive", cmbxRolService, "idrol", "servicio", "-- SELECCIONE SERVICIO --");
        }
        private void dtpinitialDate_ValueChanged(object sender, System.EventArgs e) => addTime();
        private void addTime() => lblfinaldate.Text = (cmbxTimeperiod.SelectedIndex == 1 ? dtpinitialDate.Value.ToString("dd/MMMM/yyyy") : (cmbxTimeperiod.SelectedIndex == 2 ? dtpinitialDate.Value.AddDays(4).ToString("dd/MMMM/yyyy") : dtpinitialDate.Value.AddDays(12).ToString("dd/MMMM/yyyy")));
        private void cmbxRolService_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if ((sender as System.Windows.Forms.ComboBox).SelectedIndex <= 0) { dgvcycles.DataSource = null; return; }
            rolfkCRoles = System.Convert.ToInt64((sender as System.Windows.Forms.ComboBox).SelectedValue);
            createRol();
            dgvcycles.Focus();
        }
        private async void createRol()
        {
            lblloadrol.Text = "Generando Rol";
            dgvcycles.DataSource = null;
            var activity = await Task.Run(() => rol());
            dgvcycles.DataSource = activity;

        }

        private object configureR(int dtECOS)
        {
            if (InvokeRequired)
                Invoke(new configureRows(configureR), dtECOS);
            else
            {
                lblloadrol.Text = "Aplicando Estilos";
                dgvcycles.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle() { Font = new System.Drawing.Font("Garamond", (dtECOS >= 11 ? 9.5f : 12f), System.Drawing.FontStyle.Bold), BackColor = System.Drawing.Color.FromArgb(200, 200, 200), ForeColor = System.Drawing.Color.FromArgb(75, 44, 52), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter };
                dgvcycles.RowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle() { Font = new System.Drawing.Font("Garamond", (dtECOS >= 11 ? 8 : 10), System.Drawing.FontStyle.Bold), BackColor = System.Drawing.Color.FromArgb(200, 200, 200), ForeColor = System.Drawing.Color.FromArgb(75, 44, 52), Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, WrapMode = System.Windows.Forms.DataGridViewTriState.True, SelectionBackColor = System.Drawing.Color.Crimson, SelectionForeColor = System.Drawing.Color.White };
                if (dtECOS >= 11) dgvcycles.Columns[0].Width = 55;
                for (int column = 2; column < dgvcycles.Columns.Count; column += 3)
                {

                    dgvcycles.Columns[column - 1].Visible = false;
                    if (dtECOS >= 11)
                    {
                        dgvcycles.Columns[column].Width = 60;
                        dgvcycles.Columns[column - 1].Width = 60;
                    }
                    dgvcycles.Columns[column].HeaderText = dgvcycles.Columns[column].HeaderText.Trim();
                }
                lblloadrol.Text = string.Empty;
            }
            return null;
        }

        System.Data.DataTable rol()
        {
            if (periodID.HasValue)
            {
                var res = Owner.v.getaData("CALL getIdServiceROL('" + rolfkCRoles + "','" + periodID + "')");
                if (res != null) { serviceID = System.Convert.ToInt64(res); } else serviceID = null;
            }
            System.Data.DataTable dt = new System.Data.DataTable("Rol De Servicio");
            dt.Columns.Add(new System.Data.DataColumn() { AutoIncrement = true, ColumnName = "CICLO", DataType = typeof(long), AutoIncrementSeed = 1, AutoIncrementStep = 1 });
            var dtECOS = Owner.v.getaData("call sistrefaccmant.getECOSCount(" + rolfkCRoles + ");").ToString().Split('¬');
            foreach (string row in dtECOS)
            {
                System.Data.DataTable dt2 = new System.Data.DataTable();
                var eco = row.Split('|');
                dt2.Columns.Add(eco[0]);
                System.Data.DataColumn columnhour = new System.Data.DataColumn();
                dt2.Columns.Add(new System.Data.DataColumn() { ColumnName = eco[1] + "|" + eco[0] }.Caption = "HORA");
                dt2.Columns.Add(eco[2]);
                var rows = Owner.v.getaData("call sistrefaccmant.getAllcycles(" + eco[0] + ");");
                if (!string.IsNullOrWhiteSpace(rows.ToString()))
                {
                    var drows = rows.ToString().Split('¬');
                    foreach (var rowrol in drows)
                        dt2.Rows.Add(rowrol.Split('|'));
                }
                dt = Owner.v.JoinDataTables(dt, dt2);
            }
            for (int i = dt.Rows.Count; i < System.Convert.ToInt32(Owner.v.getaData("CALL sistrefaccmant.getMaxRoles(" + rolfkCRoles + ")")); i++)
                dt.Rows.Add(dt.NewRow());
            var result = Owner.v.getaData("call sistrefaccmant.getDataRol(" + rolfkCRoles.Value + ") ").ToString().Split('|');
            System.TimeSpan increment = System.TimeSpan.Parse(result[0]); System.TimeSpan incorporeHour = System.TimeSpan.Parse(result[0]);
            int cyclesDiference = System.Convert.ToInt32(result[1]), cont = 0;
            int[] diferencies = System.Array.ConvertAll(result[2].Split(','), b => System.Convert.ToInt32(b));
            for (int column = 2; column < dt.Columns.Count; column += 3)
            {
                increment = increment.Add(System.TimeSpan.FromMinutes((column > 2 ? diferencies[cont++] : 0)));
                incorporeHour = increment;
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    dt.Rows[row][column] = (incorporeHour = incorporeHour.Add(System.TimeSpan.FromMinutes((row > 0 ? cyclesDiference : 0)))).ToString(@"hh\:mm");
                }
            }
            return dt;
        }
        private void Btn_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right) return;
            (sender as System.Windows.Forms.Button).DoDragDrop((sender as System.Windows.Forms.Button).Text, System.Windows.Forms.DragDropEffects.All);
        }
        private void nupDurationWorkDay_ValueChanged(object sender, System.EventArgs e) => lblfinaldate.Text = dtpinitialDate.Value.AddDays((System.Convert.ToDouble(nupDurationWorkDay.Value) - 1)).ToString("dd /MMMM/yyyy");
        private void dgvcycles_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (dgvcycles.CurrentCell == null) return;
            dgvcycles.CurrentCell.Value = e.Data.GetData(System.Windows.Forms.DataFormats.Text);
        }
        private void dgvcycles_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(System.Windows.Forms.DataFormats.Text) ? System.Windows.Forms.DragDropEffects.Copy : e.Effect = System.Windows.Forms.DragDropEffects.None;
        }
        private void dgvcycles_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            System.Drawing.Point dclient = dgvcycles.PointToClient(new System.Drawing.Point(e.X, e.Y));
            System.Windows.Forms.DataGridView.HitTestInfo hitTest = dgvcycles.HitTest(dclient.X, dclient.Y);
            if ((hitTest.RowIndex) >= 0 && hitTest.ColumnIndex >= 3 && !dgvcycles.Columns[hitTest.ColumnIndex].HeaderText.Equals("HORA")) { dgvcycles.CurrentCell = dgvcycles.Rows[(hitTest.RowIndex)].Cells[hitTest.ColumnIndex]; e.Effect = System.Windows.Forms.DragDropEffects.Copy; }
            else { dgvcycles.CurrentCell = null; e.Effect = System.Windows.Forms.DragDropEffects.None; }
        }
        private void Btn_Click(object sender, System.EventArgs e)
        {
            foreach (System.Windows.Forms.DataGridViewCell cell in dgvcycles.SelectedCells)
                cell.Value = (sender as System.Windows.Forms.Button).Text;
            dgvcycles.Focus();
        }
        private void dgvcycles_ColumnAdded(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e) => Owner.v.paraDataGridViews_ColumnAdded(sender, e);

        private void cmbxTimeperiod_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) => Owner.v.combos_DrawItem(sender, e);
        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Label lblfinaldate;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmbxTimeperiod;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpinitialDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbxRolService;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnsearchRol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pDias;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dgvcycles;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel backgroundPanel;
        private System.Windows.Forms.NumericUpDown nupDurationWorkDay;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblloadrol;
    }
}