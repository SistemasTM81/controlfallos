namespace controlFallos
{
    partial class familias
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
            this.gbfamilias = new System.Windows.Forms.GroupBox();
            this.tbfamilias = new System.Windows.Forms.DataGridView();
            this.idfamilia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.familia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.um = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_um = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idfamili = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbaddfamilia = new System.Windows.Forms.GroupBox();
            this.cbnombreFamilia = new System.Windows.Forms.ComboBox();
            this.lblSimbolo = new System.Windows.Forms.Label();
            this.cbunidad = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.txtdescfamilia = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.paddunidadMedida = new System.Windows.Forms.Panel();
            this.btnaddpasillo = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.pdeletefam = new System.Windows.Forms.Panel();
            this.lbldeletefam = new System.Windows.Forms.Label();
            this.btndeleteuser = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbfamilias.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfamilias)).BeginInit();
            this.gbaddfamilia.SuspendLayout();
            this.pcancel.SuspendLayout();
            this.paddunidadMedida.SuspendLayout();
            this.pdeletefam.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbfamilias
            // 
            this.gbfamilias.Controls.Add(this.tbfamilias);
            this.gbfamilias.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbfamilias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbfamilias.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbfamilias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbfamilias.Location = new System.Drawing.Point(0, 474);
            this.gbfamilias.Name = "gbfamilias";
            this.gbfamilias.Size = new System.Drawing.Size(862, 280);
            this.gbfamilias.TabIndex = 0;
            this.gbfamilias.TabStop = false;
            this.gbfamilias.Text = "Consulta de Familias de Refacciones";
            this.gbfamilias.Visible = false;
            this.gbfamilias.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddfamilia_Paint);
            this.gbfamilias.Enter += new System.EventHandler(this.gbfamilias_Enter);
            // 
            // tbfamilias
            // 
            this.tbfamilias.AllowUserToAddRows = false;
            this.tbfamilias.AllowUserToDeleteRows = false;
            this.tbfamilias.AllowUserToResizeColumns = false;
            this.tbfamilias.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(52)))), ((int)(((byte)(44)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfamilias.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbfamilias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tbfamilias.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbfamilias.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfamilias.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbfamilias.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfamilias.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbfamilias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbfamilias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idfamilia,
            this.familia,
            this.desc,
            this.um,
            this.id_um,
            this.alta,
            this.Estatus,
            this.idfamili});
            this.tbfamilias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbfamilias.EnableHeadersVisualStyles = false;
            this.tbfamilias.Location = new System.Drawing.Point(3, 27);
            this.tbfamilias.Name = "tbfamilias";
            this.tbfamilias.ReadOnly = true;
            this.tbfamilias.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfamilias.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbfamilias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbfamilias.Size = new System.Drawing.Size(856, 250);
            this.tbfamilias.TabIndex = 0;
            this.tbfamilias.TabStop = false;
            this.tbfamilias.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbfamilias_CellContentClick);
            this.tbfamilias.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbfamilias_CellContentDoubleClick);
            this.tbfamilias.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbfamilias_CellFormatting);
            this.tbfamilias.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.tbfamilias_ColumnAdded);
            // 
            // idfamilia
            // 
            this.idfamilia.HeaderText = "Column1";
            this.idfamilia.Name = "idfamilia";
            this.idfamilia.ReadOnly = true;
            this.idfamilia.Visible = false;
            this.idfamilia.Width = 92;
            // 
            // familia
            // 
            this.familia.FillWeight = 96.59056F;
            this.familia.HeaderText = "FAMILIA";
            this.familia.Name = "familia";
            this.familia.ReadOnly = true;
            this.familia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.familia.Width = 99;
            // 
            // desc
            // 
            this.desc.FillWeight = 380.7106F;
            this.desc.HeaderText = "DESCRIPCIÓN";
            this.desc.MaxInputLength = 80;
            this.desc.Name = "desc";
            this.desc.ReadOnly = true;
            this.desc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.desc.Width = 153;
            // 
            // um
            // 
            this.um.FillWeight = 7.56626F;
            this.um.HeaderText = "UNIDAD DE MEDIDA";
            this.um.Name = "um";
            this.um.ReadOnly = true;
            this.um.Width = 221;
            // 
            // id_um
            // 
            this.id_um.HeaderText = "id_um";
            this.id_um.Name = "id_um";
            this.id_um.ReadOnly = true;
            this.id_um.Visible = false;
            this.id_um.Width = 88;
            // 
            // alta
            // 
            this.alta.FillWeight = 7.56626F;
            this.alta.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.alta.Name = "alta";
            this.alta.ReadOnly = true;
            this.alta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.alta.Width = 189;
            // 
            // Estatus
            // 
            this.Estatus.FillWeight = 7.56626F;
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Estatus.Width = 105;
            // 
            // idfamili
            // 
            this.idfamili.HeaderText = "idfamilia";
            this.idfamili.Name = "idfamili";
            this.idfamili.ReadOnly = true;
            this.idfamili.Visible = false;
            this.idfamili.Width = 107;
            // 
            // gbaddfamilia
            // 
            this.gbaddfamilia.Controls.Add(this.cbnombreFamilia);
            this.gbaddfamilia.Controls.Add(this.lblSimbolo);
            this.gbaddfamilia.Controls.Add(this.cbunidad);
            this.gbaddfamilia.Controls.Add(this.label3);
            this.gbaddfamilia.Controls.Add(this.label2);
            this.gbaddfamilia.Controls.Add(this.label22);
            this.gbaddfamilia.Controls.Add(this.label23);
            this.gbaddfamilia.Controls.Add(this.pcancel);
            this.gbaddfamilia.Controls.Add(this.txtdescfamilia);
            this.gbaddfamilia.Controls.Add(this.label4);
            this.gbaddfamilia.Controls.Add(this.label9);
            this.gbaddfamilia.Controls.Add(this.lblsave);
            this.gbaddfamilia.Controls.Add(this.btnsave);
            this.gbaddfamilia.Controls.Add(this.label18);
            this.gbaddfamilia.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddfamilia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddfamilia.Location = new System.Drawing.Point(0, 33);
            this.gbaddfamilia.Name = "gbaddfamilia";
            this.gbaddfamilia.Size = new System.Drawing.Size(862, 438);
            this.gbaddfamilia.TabIndex = 0;
            this.gbaddfamilia.TabStop = false;
            this.gbaddfamilia.Text = "Agregar Familia de Refacciones";
            this.gbaddfamilia.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddfamilia_Paint);
            this.gbaddfamilia.Enter += new System.EventHandler(this.gbaddfamilia_Enter);
            // 
            // cbnombreFamilia
            // 
            this.cbnombreFamilia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbnombreFamilia.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbnombreFamilia.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbnombreFamilia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbnombreFamilia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbnombreFamilia.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbnombreFamilia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbnombreFamilia.FormattingEnabled = true;
            this.cbnombreFamilia.ItemHeight = 20;
            this.cbnombreFamilia.Location = new System.Drawing.Point(391, 51);
            this.cbnombreFamilia.MaxDropDownItems = 15;
            this.cbnombreFamilia.Name = "cbnombreFamilia";
            this.cbnombreFamilia.Size = new System.Drawing.Size(244, 26);
            this.cbnombreFamilia.TabIndex = 1;
            this.cbnombreFamilia.SelectedIndexChanged += new System.EventHandler(this.getCambios);
            // 
            // lblSimbolo
            // 
            this.lblSimbolo.AutoSize = true;
            this.lblSimbolo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblSimbolo.Location = new System.Drawing.Point(388, 264);
            this.lblSimbolo.Name = "lblSimbolo";
            this.lblSimbolo.Size = new System.Drawing.Size(0, 24);
            this.lblSimbolo.TabIndex = 70;
            // 
            // cbunidad
            // 
            this.cbunidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbunidad.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbunidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbunidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbunidad.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbunidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbunidad.FormattingEnabled = true;
            this.cbunidad.Location = new System.Drawing.Point(391, 199);
            this.cbunidad.Name = "cbunidad";
            this.cbunidad.Size = new System.Drawing.Size(244, 26);
            this.cbunidad.TabIndex = 3;
            this.cbunidad.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbunidad_DrawItem);
            this.cbunidad.SelectedIndexChanged += new System.EventHandler(this.cbunidad_SelectedIndexChanged);
            this.cbunidad.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(170, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 24);
            this.label3.TabIndex = 68;
            this.label3.Text = "Simbolo:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.Location = new System.Drawing.Point(170, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 24);
            this.label2.TabIndex = 67;
            this.label2.Text = "Unidad de Medida:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Crimson;
            this.label22.Location = new System.Drawing.Point(115, 415);
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
            this.label23.Location = new System.Drawing.Point(154, 415);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // pcancel
            // 
            this.pcancel.Controls.Add(this.label1);
            this.pcancel.Controls.Add(this.btncancel);
            this.pcancel.Location = new System.Drawing.Point(533, 303);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(147, 88);
            this.pcancel.TabIndex = 0;
            this.pcancel.Visible = false;
            this.pcancel.Paint += new System.Windows.Forms.PaintEventHandler(this.pcancel_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(43, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nuevo";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancel.FlatAppearance.BorderSize = 0;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Location = new System.Drawing.Point(52, 3);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(50, 50);
            this.btncancel.TabIndex = 0;
            this.btncancel.TabStop = false;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtdescfamilia
            // 
            this.txtdescfamilia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtdescfamilia.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtdescfamilia.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtdescfamilia.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdescfamilia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtdescfamilia.Location = new System.Drawing.Point(391, 132);
            this.txtdescfamilia.MaxLength = 40;
            this.txtdescfamilia.Name = "txtdescfamilia";
            this.txtdescfamilia.ShortcutsEnabled = false;
            this.txtdescfamilia.Size = new System.Drawing.Size(244, 18);
            this.txtdescfamilia.TabIndex = 2;
            this.txtdescfamilia.TextChanged += new System.EventHandler(this.getCambios);
            this.txtdescfamilia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnombre_KeyPress);
            this.txtdescfamilia.Validating += new System.ComponentModel.CancelEventHandler(this.txtnombre_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label4.Location = new System.Drawing.Point(389, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(245, 9);
            this.label4.TabIndex = 0;
            this.label4.Text = "____________________________________________________________";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label9.Location = new System.Drawing.Point(170, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(207, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "Descripción de Familia:";
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblsave.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblsave.Location = new System.Drawing.Point(407, 365);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(74, 21);
            this.lblsave.TabIndex = 0;
            this.lblsave.Text = "Agregar";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.btnsave.Location = new System.Drawing.Point(420, 305);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 4;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.button10_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label18.Location = new System.Drawing.Point(170, 50);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(176, 24);
            this.label18.TabIndex = 0;
            this.label18.Text = "Nombre de Familia:";
            // 
            // paddunidadMedida
            // 
            this.paddunidadMedida.Controls.Add(this.btnaddpasillo);
            this.paddunidadMedida.Controls.Add(this.label13);
            this.paddunidadMedida.Location = new System.Drawing.Point(657, 188);
            this.paddunidadMedida.Name = "paddunidadMedida";
            this.paddunidadMedida.Size = new System.Drawing.Size(144, 112);
            this.paddunidadMedida.TabIndex = 0;
            // 
            // btnaddpasillo
            // 
            this.btnaddpasillo.BackgroundImage = global::controlFallos.Properties.Resources._goto;
            this.btnaddpasillo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnaddpasillo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnaddpasillo.FlatAppearance.BorderSize = 0;
            this.btnaddpasillo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnaddpasillo.Location = new System.Drawing.Point(56, 4);
            this.btnaddpasillo.Name = "btnaddpasillo";
            this.btnaddpasillo.Size = new System.Drawing.Size(35, 35);
            this.btnaddpasillo.TabIndex = 0;
            this.btnaddpasillo.TabStop = false;
            this.btnaddpasillo.UseVisualStyleBackColor = true;
            this.btnaddpasillo.Click += new System.EventHandler(this.btnaddpasillo_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label13.Location = new System.Drawing.Point(5, 41);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(141, 96);
            this.label13.TabIndex = 0;
            this.label13.Text = "Ir a Catálogo de\r\nUnidades de \r\nMedida\r\n\r\n";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pdeletefam
            // 
            this.pdeletefam.Controls.Add(this.lbldeletefam);
            this.pdeletefam.Controls.Add(this.btndeleteuser);
            this.pdeletefam.Location = new System.Drawing.Point(171, 342);
            this.pdeletefam.Name = "pdeletefam";
            this.pdeletefam.Size = new System.Drawing.Size(144, 82);
            this.pdeletefam.TabIndex = 41;
            this.pdeletefam.Visible = false;
            // 
            // lbldeletefam
            // 
            this.lbldeletefam.AutoSize = true;
            this.lbldeletefam.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletefam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletefam.Location = new System.Drawing.Point(22, 55);
            this.lbldeletefam.Name = "lbldeletefam";
            this.lbldeletefam.Size = new System.Drawing.Size(105, 24);
            this.lbldeletefam.TabIndex = 0;
            this.lbldeletefam.Text = "Desactivar";
            // 
            // btndeleteuser
            // 
            this.btndeleteuser.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeleteuser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeleteuser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteuser.FlatAppearance.BorderSize = 0;
            this.btndeleteuser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeleteuser.Location = new System.Drawing.Point(52, 2);
            this.btndeleteuser.Name = "btndeleteuser";
            this.btndeleteuser.Size = new System.Drawing.Size(50, 50);
            this.btndeleteuser.TabIndex = 0;
            this.btndeleteuser.TabStop = false;
            this.btndeleteuser.UseVisualStyleBackColor = true;
            this.btndeleteuser.Click += new System.EventHandler(this.btndeleteuser_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(862, 27);
            this.panel1.TabIndex = 83;
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
            this.button1.Location = new System.Drawing.Point(831, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 27);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(223, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(327, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = " Catálogo de Descripciones de Familia";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // familias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(862, 754);
            this.Controls.Add(this.pdeletefam);
            this.Controls.Add(this.paddunidadMedida);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbfamilias);
            this.Controls.Add(this.gbaddfamilia);
            this.Font = new System.Drawing.Font("Garamond", 15.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "familias";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "familias";
            this.Load += new System.EventHandler(this.familias_Load);
            this.gbfamilias.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbfamilias)).EndInit();
            this.gbaddfamilia.ResumeLayout(false);
            this.gbaddfamilia.PerformLayout();
            this.pcancel.ResumeLayout(false);
            this.pcancel.PerformLayout();
            this.paddunidadMedida.ResumeLayout(false);
            this.paddunidadMedida.PerformLayout();
            this.pdeletefam.ResumeLayout(false);
            this.pdeletefam.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbfamilias;
        private System.Windows.Forms.DataGridView tbfamilias;
        private System.Windows.Forms.GroupBox gbaddfamilia;
        private System.Windows.Forms.TextBox txtdescfamilia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pdeletefam;
        private System.Windows.Forms.Label lbldeletefam;
        private System.Windows.Forms.Button btndeleteuser;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cbunidad;
        private System.Windows.Forms.Label lblSimbolo;
        public System.Windows.Forms.ComboBox cbnombreFamilia;
        private System.Windows.Forms.Panel paddunidadMedida;
        private System.Windows.Forms.Button btnaddpasillo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn idfamilia;
        private System.Windows.Forms.DataGridViewTextBoxColumn familia;
        private System.Windows.Forms.DataGridViewTextBoxColumn desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn um;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_um;
        private System.Windows.Forms.DataGridViewTextBoxColumn alta;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn idfamili;
    }
}