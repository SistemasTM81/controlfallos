namespace controlFallos
{
    partial class catServicios
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
            this.gbaddservice = new System.Windows.Forms.GroupBox();
            this.cbarea = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbempresa = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancelar = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtgetnombre_s = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblsaves = new System.Windows.Forms.Label();
            this.btnsaves = new System.Windows.Forms.Button();
            this.txtgetclave = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.pEliminarService = new System.Windows.Forms.Panel();
            this.lbldelete = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.gbservicios = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.idServicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Clave = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.people = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empresafkcempresas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbaddservice.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.pEliminarService.SuspendLayout();
            this.gbservicios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbaddservice
            // 
            this.gbaddservice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddservice.Controls.Add(this.cbarea);
            this.gbaddservice.Controls.Add(this.label3);
            this.gbaddservice.Controls.Add(this.cbempresa);
            this.gbaddservice.Controls.Add(this.label2);
            this.gbaddservice.Controls.Add(this.pCancelar);
            this.gbaddservice.Controls.Add(this.label22);
            this.gbaddservice.Controls.Add(this.label23);
            this.gbaddservice.Controls.Add(this.txtgetnombre_s);
            this.gbaddservice.Controls.Add(this.label13);
            this.gbaddservice.Controls.Add(this.label15);
            this.gbaddservice.Controls.Add(this.lblsaves);
            this.gbaddservice.Controls.Add(this.btnsaves);
            this.gbaddservice.Controls.Add(this.txtgetclave);
            this.gbaddservice.Controls.Add(this.label17);
            this.gbaddservice.Controls.Add(this.label18);
            this.gbaddservice.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddservice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddservice.Location = new System.Drawing.Point(0, 27);
            this.gbaddservice.Name = "gbaddservice";
            this.gbaddservice.Size = new System.Drawing.Size(1050, 447);
            this.gbaddservice.TabIndex = 0;
            this.gbaddservice.TabStop = false;
            this.gbaddservice.Text = "Nuevo Servicio";
            this.gbaddservice.Visible = false;
            this.gbaddservice.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddservice_Paint);
            this.gbaddservice.Enter += new System.EventHandler(this.gbadd_Enter);
            // 
            // cbarea
            // 
            this.cbarea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbarea.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbarea.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbarea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbarea.DropDownWidth = 400;
            this.cbarea.Enabled = false;
            this.cbarea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbarea.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbarea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbarea.FormattingEnabled = true;
            this.cbarea.Location = new System.Drawing.Point(411, 110);
            this.cbarea.MaxDropDownItems = 15;
            this.cbarea.Name = "cbarea";
            this.cbarea.Size = new System.Drawing.Size(396, 26);
            this.cbarea.TabIndex = 68;
            this.cbarea.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbempresa_DrawItem);
            this.cbarea.SelectedIndexChanged += new System.EventHandler(this.cbarea_SelectedIndexChanged);
            this.cbarea.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 24);
            this.label3.TabIndex = 69;
            this.label3.Text = "Área:";
            // 
            // cbempresa
            // 
            this.cbempresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbempresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbempresa.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbempresa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbempresa.DropDownWidth = 400;
            this.cbempresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbempresa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbempresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbempresa.FormattingEnabled = true;
            this.cbempresa.Location = new System.Drawing.Point(411, 36);
            this.cbempresa.MaxDropDownItems = 15;
            this.cbempresa.Name = "cbempresa";
            this.cbempresa.Size = new System.Drawing.Size(396, 26);
            this.cbempresa.TabIndex = 1;
            this.cbempresa.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbempresa_DrawItem);
            this.cbempresa.SelectedIndexChanged += new System.EventHandler(this.cbempresa_SelectedIndexChanged);
            this.cbempresa.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 24);
            this.label2.TabIndex = 67;
            this.label2.Text = "Empresa:";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btncancelar);
            this.pCancelar.Location = new System.Drawing.Point(694, 303);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(173, 84);
            this.pCancelar.TabIndex = 27;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(59, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 26;
            this.label1.Text = "Nuevo";
            // 
            // btncancelar
            // 
            this.btncancelar.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancelar.FlatAppearance.BorderSize = 0;
            this.btncancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancelar.Location = new System.Drawing.Point(63, 3);
            this.btncancelar.Name = "btncancelar";
            this.btncancelar.Size = new System.Drawing.Size(50, 50);
            this.btncancelar.TabIndex = 26;
            this.btncancelar.UseVisualStyleBackColor = true;
            this.btncancelar.Click += new System.EventHandler(this.btncancelar_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Crimson;
            this.label22.Location = new System.Drawing.Point(265, 404);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(47, 18);
            this.label22.TabIndex = 66;
            this.label22.Text = "Nota:";
            this.label22.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(318, 404);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // txtgetnombre_s
            // 
            this.txtgetnombre_s.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetnombre_s.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetnombre_s.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetnombre_s.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetnombre_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetnombre_s.Location = new System.Drawing.Point(411, 266);
            this.txtgetnombre_s.MaxLength = 45;
            this.txtgetnombre_s.Name = "txtgetnombre_s";
            this.txtgetnombre_s.ShortcutsEnabled = false;
            this.txtgetnombre_s.Size = new System.Drawing.Size(395, 18);
            this.txtgetnombre_s.TabIndex = 3;
            this.txtgetnombre_s.TextChanged += new System.EventHandler(this.getCambios);
            this.txtgetnombre_s.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetnombre_s_KeyPress);
            this.txtgetnombre_s.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetclave_Validating);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(195, 189);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(156, 24);
            this.label13.TabIndex = 14;
            this.label13.Text = "Nombre Servicio:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(409, 279);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(397, 9);
            this.label15.TabIndex = 0;
            this.label15.Text = "_________________________________________________________________________________" +
    "_________________";
            // 
            // lblsaves
            // 
            this.lblsaves.AutoSize = true;
            this.lblsaves.Location = new System.Drawing.Point(479, 366);
            this.lblsaves.Name = "lblsaves";
            this.lblsaves.Size = new System.Drawing.Size(79, 24);
            this.lblsaves.TabIndex = 13;
            this.lblsaves.Text = "Guardar";
            // 
            // btnsaves
            // 
            this.btnsaves.BackColor = System.Drawing.Color.Transparent;
            this.btnsaves.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsaves.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsaves.FlatAppearance.BorderSize = 0;
            this.btnsaves.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsaves.Location = new System.Drawing.Point(493, 308);
            this.btnsaves.Name = "btnsaves";
            this.btnsaves.Size = new System.Drawing.Size(50, 50);
            this.btnsaves.TabIndex = 6;
            this.btnsaves.UseVisualStyleBackColor = false;
            this.btnsaves.Click += new System.EventHandler(this.btnsaves_Click);
            // 
            // txtgetclave
            // 
            this.txtgetclave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetclave.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetclave.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetclave.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetclave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetclave.Location = new System.Drawing.Point(411, 187);
            this.txtgetclave.MaxLength = 40;
            this.txtgetclave.Name = "txtgetclave";
            this.txtgetclave.ShortcutsEnabled = false;
            this.txtgetclave.Size = new System.Drawing.Size(395, 18);
            this.txtgetclave.TabIndex = 2;
            this.txtgetclave.TextChanged += new System.EventHandler(this.getCambios);
            this.txtgetclave.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetclave_KeyPress);
            this.txtgetclave.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetclave_Validating);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(195, 262);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(212, 24);
            this.label17.TabIndex = 0;
            this.label17.Text = "Descripción de Servicio:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(409, 200);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(397, 9);
            this.label18.TabIndex = 0;
            this.label18.Text = "_________________________________________________________________________________" +
    "_________________";
            // 
            // pEliminarService
            // 
            this.pEliminarService.Controls.Add(this.lbldelete);
            this.pEliminarService.Controls.Add(this.btndelete);
            this.pEliminarService.Location = new System.Drawing.Point(204, 332);
            this.pEliminarService.Name = "pEliminarService";
            this.pEliminarService.Size = new System.Drawing.Size(173, 84);
            this.pEliminarService.TabIndex = 17;
            this.pEliminarService.Visible = false;
            // 
            // lbldelete
            // 
            this.lbldelete.AutoSize = true;
            this.lbldelete.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelete.Location = new System.Drawing.Point(32, 60);
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
            this.btndelete.Location = new System.Drawing.Point(60, 3);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(50, 50);
            this.btndelete.TabIndex = 25;
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.button6_Click);
            // 
            // gbservicios
            // 
            this.gbservicios.Controls.Add(this.dataGridView2);
            this.gbservicios.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbservicios.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbservicios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbservicios.Location = new System.Drawing.Point(0, 470);
            this.gbservicios.Name = "gbservicios";
            this.gbservicios.Size = new System.Drawing.Size(1053, 358);
            this.gbservicios.TabIndex = 0;
            this.gbservicios.TabStop = false;
            this.gbservicios.Text = "Consulta de Servicios";
            this.gbservicios.Visible = false;
            this.gbservicios.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddservice_Paint);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idServicio,
            this.Nombre,
            this.emp,
            this.ar,
            this.Clave,
            this.people,
            this.Estatus,
            this.empresafkcempresas});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView2.EnableHeadersVisualStyles = false;
            this.dataGridView2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dataGridView2.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.dataGridView2.Location = new System.Drawing.Point(3, 27);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.ShowCellErrors = false;
            this.dataGridView2.ShowCellToolTips = false;
            this.dataGridView2.ShowEditingIcon = false;
            this.dataGridView2.ShowRowErrors = false;
            this.dataGridView2.Size = new System.Drawing.Size(1047, 328);
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentDoubleClick);
            this.dataGridView2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView2_CellFormatting);
            this.dataGridView2.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView2_ColumnAdded);
            // 
            // idServicio
            // 
            this.idServicio.HeaderText = "idservicio";
            this.idServicio.Name = "idServicio";
            this.idServicio.ReadOnly = true;
            this.idServicio.Visible = false;
            // 
            // Nombre
            // 
            this.Nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Nombre.HeaderText = "NOMBRE SERVICIO";
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            this.Nombre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Nombre.Width = 198;
            // 
            // emp
            // 
            this.emp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.emp.HeaderText = "EMPRESA PERTENECIENTE";
            this.emp.Name = "emp";
            this.emp.ReadOnly = true;
            this.emp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.emp.Width = 280;
            // 
            // ar
            // 
            this.ar.HeaderText = "ÁREA";
            this.ar.Name = "ar";
            this.ar.ReadOnly = true;
            // 
            // Clave
            // 
            this.Clave.FillWeight = 142.1053F;
            this.Clave.HeaderText = "DESCRIPCIÓN";
            this.Clave.Name = "Clave";
            this.Clave.ReadOnly = true;
            this.Clave.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // people
            // 
            this.people.FillWeight = 78.94736F;
            this.people.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.people.Name = "people";
            this.people.ReadOnly = true;
            this.people.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estatus
            // 
            this.Estatus.FillWeight = 78.94736F;
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // empresafkcempresas
            // 
            this.empresafkcempresas.HeaderText = "empresafkcempresas";
            this.empresafkcempresas.Name = "empresafkcempresas";
            this.empresafkcempresas.ReadOnly = true;
            this.empresafkcempresas.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1053, 27);
            this.panel1.TabIndex = 68;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
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
            this.button1.Location = new System.Drawing.Point(1021, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 27);
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
            this.lbltitle.Location = new System.Drawing.Point(345, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(187, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Servicios";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // catServicios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1053, 828);
            this.Controls.Add(this.pEliminarService);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbaddservice);
            this.Controls.Add(this.gbservicios);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "catServicios";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catServicios";
            this.Load += new System.EventHandler(this.catServicios_Load);
            this.gbaddservice.ResumeLayout(false);
            this.gbaddservice.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.pEliminarService.ResumeLayout(false);
            this.pEliminarService.PerformLayout();
            this.gbservicios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbaddservice;
        private System.Windows.Forms.Button btncancelar;
        private System.Windows.Forms.Panel pEliminarService;
        private System.Windows.Forms.Label lbldelete;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.TextBox txtgetnombre_s;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblsaves;
        private System.Windows.Forms.Button btnsaves;
        private System.Windows.Forms.TextBox txtgetclave;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox gbservicios;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cbempresa;
        public System.Windows.Forms.ComboBox cbarea;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn idServicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn emp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Clave;
        private System.Windows.Forms.DataGridViewTextBoxColumn people;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn empresafkcempresas;
    }
}