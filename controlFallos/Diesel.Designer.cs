namespace controlFallos
{
    partial class Diesel
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCant = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblUnidad = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.gbareas = new System.Windows.Forms.GroupBox();
            this.tbCargas = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._p = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombreArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label35 = new System.Windows.Forms.Label();
            this.cmbBuscarUnidad = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbMes = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.dtpFechaDe = new System.Windows.Forms.DateTimePicker();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.dtpFechaA = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.buttonExcel = new System.Windows.Forms.Button();
            this.pictureBoxExcelLoad = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGuarda = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbAgrega = new System.Windows.Forms.ComboBox();
            this.cmbEco = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.gbareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCargas)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcelLoad)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(1159, 27);
            this.panel1.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Crimson;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(396, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Diesel";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // lblt
            // 
            this.lblt.AutoSize = true;
            this.lblt.Font = new System.Drawing.Font("Garamond", 15.75F);
            this.lblt.Location = new System.Drawing.Point(4, 56);
            this.lblt.Name = "lblt";
            this.lblt.Size = new System.Drawing.Size(112, 24);
            this.lblt.TabIndex = 210;
            this.lblt.Text = "Económico:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 15.75F);
            this.label2.Location = new System.Drawing.Point(4, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 24);
            this.label2.TabIndex = 211;
            this.label2.Text = "Agrega:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Garamond", 15.75F);
            this.label3.Location = new System.Drawing.Point(4, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 24);
            this.label3.TabIndex = 212;
            this.label3.Text = "Cantidad:";
            // 
            // txtCant
            // 
            this.txtCant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtCant.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCant.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCant.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCant.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtCant.Location = new System.Drawing.Point(116, 128);
            this.txtCant.MaxLength = 10;
            this.txtCant.Name = "txtCant";
            this.txtCant.ShortcutsEnabled = false;
            this.txtCant.Size = new System.Drawing.Size(248, 18);
            this.txtCant.TabIndex = 216;
            this.txtCant.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCant_KeyPress);
            this.txtCant.Validating += new System.ComponentModel.CancelEventHandler(this.txtCant_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(114, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(253, 9);
            this.label5.TabIndex = 215;
            this.label5.Text = "______________________________________________________________";
            // 
            // lblUnidad
            // 
            this.lblUnidad.AutoSize = true;
            this.lblUnidad.Font = new System.Drawing.Font("Garamond", 14F);
            this.lblUnidad.Location = new System.Drawing.Point(373, 129);
            this.lblUnidad.Name = "lblUnidad";
            this.lblUnidad.Size = new System.Drawing.Size(0, 21);
            this.lblUnidad.TabIndex = 217;
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label4);
            this.pCancelar.Controls.Add(this.btnCancel);
            this.pCancelar.Location = new System.Drawing.Point(1072, 56);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(57, 54);
            this.pCancelar.TabIndex = 218;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 9.75F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label4.Location = new System.Drawing.Point(1, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "NUEVO";
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Font = new System.Drawing.Font("Garamond", 9.75F);
            this.lblsavemp.Location = new System.Drawing.Point(973, 93);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(70, 14);
            this.lblsavemp.TabIndex = 220;
            this.lblsavemp.Text = "GUARDAR";
            // 
            // gbareas
            // 
            this.gbareas.Controls.Add(this.tbCargas);
            this.gbareas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbareas.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbareas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbareas.Location = new System.Drawing.Point(0, 279);
            this.gbareas.Name = "gbareas";
            this.gbareas.Size = new System.Drawing.Size(1159, 221);
            this.gbareas.TabIndex = 221;
            this.gbareas.TabStop = false;
            // 
            // tbCargas
            // 
            this.tbCargas.AllowUserToAddRows = false;
            this.tbCargas.AllowUserToDeleteRows = false;
            this.tbCargas.AllowUserToResizeColumns = false;
            this.tbCargas.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbCargas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.tbCargas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbCargas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbCargas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbCargas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCargas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbCargas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.tbCargas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbCargas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this._p,
            this.dataGridViewTextBoxColumn2,
            this.nombreArea,
            this.Uni,
            this.usuario,
            this.Estatus});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbCargas.DefaultCellStyle = dataGridViewCellStyle8;
            this.tbCargas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCargas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbCargas.EnableHeadersVisualStyles = false;
            this.tbCargas.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbCargas.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbCargas.Location = new System.Drawing.Point(3, 25);
            this.tbCargas.MultiSelect = false;
            this.tbCargas.Name = "tbCargas";
            this.tbCargas.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbCargas.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.tbCargas.RowHeadersVisible = false;
            this.tbCargas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.Crimson;
            this.tbCargas.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.tbCargas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbCargas.ShowCellErrors = false;
            this.tbCargas.ShowCellToolTips = false;
            this.tbCargas.ShowEditingIcon = false;
            this.tbCargas.ShowRowErrors = false;
            this.tbCargas.Size = new System.Drawing.Size(1153, 193);
            this.tbCargas.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idDispDiesel";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // _p
            // 
            this._p.HeaderText = "ECONÓMICO";
            this._p.Name = "_p";
            this._p.ReadOnly = true;
            this._p.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "AGREGA";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nombreArea
            // 
            this.nombreArea.HeaderText = "CANTIDAD";
            this.nombreArea.Name = "nombreArea";
            this.nombreArea.ReadOnly = true;
            this.nombreArea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Uni
            // 
            this.Uni.HeaderText = "UNIDAD";
            this.Uni.Name = "Uni";
            this.Uni.ReadOnly = true;
            this.Uni.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "FECHA - HORA";
            this.usuario.Name = "usuario";
            this.usuario.ReadOnly = true;
            this.usuario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "USUARIO";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label35.Location = new System.Drawing.Point(1054, 74);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(75, 14);
            this.label35.TabIndex = 231;
            this.label35.Text = "EXPORTAR";
            this.label35.Visible = false;
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
            this.cmbBuscarUnidad.Location = new System.Drawing.Point(62, 43);
            this.cmbBuscarUnidad.Name = "cmbBuscarUnidad";
            this.cmbBuscarUnidad.Size = new System.Drawing.Size(259, 26);
            this.cmbBuscarUnidad.TabIndex = 234;
            this.cmbBuscarUnidad.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbBuscarUnidad_DrawItem);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Garamond", 12F);
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(9, 46);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(38, 18);
            this.label15.TabIndex = 233;
            this.label15.Text = "Eco:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.buttonExcel);
            this.groupBox1.Controls.Add(this.pictureBoxExcelLoad);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label35);
            this.groupBox1.Controls.Add(this.cmbMes);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.dtpFechaDe);
            this.groupBox1.Controls.Add(this.cmbBuscarUnidad);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Controls.Add(this.dtpFechaA);
            this.groupBox1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 178);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1153, 110);
            this.groupBox1.TabIndex = 235;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtros de busqueda:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label18.Font = new System.Drawing.Font("Garamond", 9.75F);
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label18.Location = new System.Drawing.Point(968, 75);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 14);
            this.label18.TabIndex = 244;
            this.label18.Text = "BUSCAR";
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
            this.cmbMes.Location = new System.Drawing.Point(62, 77);
            this.cmbMes.Name = "cmbMes";
            this.cmbMes.Size = new System.Drawing.Size(259, 25);
            this.cmbMes.TabIndex = 241;
            this.cmbMes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbMes_DrawItem);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label25.Location = new System.Drawing.Point(9, 82);
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
            this.dtpFechaDe.Location = new System.Drawing.Point(405, 47);
            this.dtpFechaDe.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.dtpFechaDe.Name = "dtpFechaDe";
            this.dtpFechaDe.Size = new System.Drawing.Size(252, 25);
            this.dtpFechaDe.TabIndex = 242;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Garamond", 12F);
            this.checkBox1.Location = new System.Drawing.Point(435, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(138, 22);
            this.checkBox1.TabIndex = 239;
            this.checkBox1.Text = "Rango De Fechas";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Font = new System.Drawing.Font("Garamond", 12F);
            this.label31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label31.Location = new System.Drawing.Point(368, 52);
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
            this.label32.Location = new System.Drawing.Point(368, 84);
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
            this.dtpFechaA.Location = new System.Drawing.Point(405, 79);
            this.dtpFechaA.MinDate = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
            this.dtpFechaA.Name = "dtpFechaA";
            this.dtpFechaA.Size = new System.Drawing.Size(252, 25);
            this.dtpFechaA.TabIndex = 236;
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
            this.btnBuscar.Location = new System.Drawing.Point(983, 37);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(31, 32);
            this.btnBuscar.TabIndex = 243;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
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
            this.buttonExcel.Location = new System.Drawing.Point(1073, 37);
            this.buttonExcel.Name = "buttonExcel";
            this.buttonExcel.Size = new System.Drawing.Size(35, 35);
            this.buttonExcel.TabIndex = 230;
            this.buttonExcel.UseVisualStyleBackColor = false;
            this.buttonExcel.Visible = false;
            this.buttonExcel.Click += new System.EventHandler(this.buttonExcel_Click);
            // 
            // pictureBoxExcelLoad
            // 
            this.pictureBoxExcelLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxExcelLoad.Location = new System.Drawing.Point(1073, 37);
            this.pictureBoxExcelLoad.Name = "pictureBoxExcelLoad";
            this.pictureBoxExcelLoad.Size = new System.Drawing.Size(35, 35);
            this.pictureBoxExcelLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxExcelLoad.TabIndex = 232;
            this.pictureBoxExcelLoad.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(9, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(35, 35);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGuarda
            // 
            this.btnGuarda.BackColor = System.Drawing.Color.Transparent;
            this.btnGuarda.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnGuarda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGuarda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuarda.FlatAppearance.BorderSize = 0;
            this.btnGuarda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuarda.Location = new System.Drawing.Point(988, 56);
            this.btnGuarda.Name = "btnGuarda";
            this.btnGuarda.Size = new System.Drawing.Size(35, 35);
            this.btnGuarda.TabIndex = 219;
            this.btnGuarda.UseVisualStyleBackColor = false;
            this.btnGuarda.Click += new System.EventHandler(this.btnGuarda_Click);
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
            this.button1.Location = new System.Drawing.Point(1129, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 27);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbAgrega
            // 
            this.cmbAgrega.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbAgrega.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAgrega.DropDownHeight = 100;
            this.cmbAgrega.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAgrega.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbAgrega.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAgrega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbAgrega.FormattingEnabled = true;
            this.cmbAgrega.IntegralHeight = false;
            this.cmbAgrega.ItemHeight = 21;
            this.cmbAgrega.Location = new System.Drawing.Point(116, 89);
            this.cmbAgrega.Name = "cmbAgrega";
            this.cmbAgrega.Size = new System.Drawing.Size(257, 27);
            this.cmbAgrega.TabIndex = 214;
            this.cmbAgrega.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbAgrega_DrawItem);
            this.cmbAgrega.SelectedValueChanged += new System.EventHandler(this.cmbAgrega_SelectedValueChanged);
            // 
            // cmbEco
            // 
            this.cmbEco.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbEco.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEco.DropDownHeight = 100;
            this.cmbEco.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEco.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbEco.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEco.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbEco.FormattingEnabled = true;
            this.cmbEco.IntegralHeight = false;
            this.cmbEco.ItemHeight = 21;
            this.cmbEco.Location = new System.Drawing.Point(116, 53);
            this.cmbEco.Name = "cmbEco";
            this.cmbEco.Size = new System.Drawing.Size(257, 27);
            this.cmbEco.TabIndex = 213;
            this.cmbEco.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbEco_DrawItem);
            // 
            // Diesel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1159, 500);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbareas);
            this.Controls.Add(this.pCancelar);
            this.Controls.Add(this.lblsavemp);
            this.Controls.Add(this.btnGuarda);
            this.Controls.Add(this.lblUnidad);
            this.Controls.Add(this.txtCant);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbAgrega);
            this.Controls.Add(this.cmbEco);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblt);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Diesel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "        cv";
            this.Load += new System.EventHandler(this.Diesel_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.gbareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbCargas)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExcelLoad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCant;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblUnidad;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnGuarda;
        private System.Windows.Forms.GroupBox gbareas;
        private System.Windows.Forms.DataGridView tbCargas;
        private System.Windows.Forms.Button buttonExcel;
        private System.Windows.Forms.PictureBox pictureBoxExcelLoad;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox cmbBuscarUnidad;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbMes;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.DateTimePicker dtpFechaDe;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.DateTimePicker dtpFechaA;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn _p;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uni;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.ComboBox cmbAgrega;
        private System.Windows.Forms.ComboBox cmbEco;
    }
}