namespace controlFallos
{
    partial class catDescFallos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbclasif = new System.Windows.Forms.GroupBox();
            this.tbfallos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idclasif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbaddclasif = new System.Windows.Forms.GroupBox();
            this.lnkLista = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cbclasificacion = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtgetdescfallo = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.pEliminarClasificacion = new System.Windows.Forms.Panel();
            this.lbldeletedesc = new System.Windows.Forms.Label();
            this.btndeletedesc = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbclasif.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).BeginInit();
            this.gbaddclasif.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.pEliminarClasificacion.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbclasif
            // 
            this.gbclasif.Controls.Add(this.tbfallos);
            this.gbclasif.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbclasif.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbclasif.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbclasif.Location = new System.Drawing.Point(0, 402);
            this.gbclasif.Name = "gbclasif";
            this.gbclasif.Size = new System.Drawing.Size(840, 297);
            this.gbclasif.TabIndex = 31;
            this.gbclasif.TabStop = false;
            this.gbclasif.Text = "Consulta Descripciones de Fallo";
            this.gbclasif.Visible = false;
            this.gbclasif.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddclasif_Paint);
            // 
            // tbfallos
            // 
            this.tbfallos.AllowUserToAddRows = false;
            this.tbfallos.AllowUserToDeleteRows = false;
            this.tbfallos.AllowUserToResizeColumns = false;
            this.tbfallos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbfallos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbfallos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbfallos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbfallos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbfallos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbfallos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Descripcion,
            this.usuario,
            this.Estatus,
            this.idclasif});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbfallos.DefaultCellStyle = dataGridViewCellStyle3;
            this.tbfallos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbfallos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbfallos.EnableHeadersVisualStyles = false;
            this.tbfallos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbfallos.Location = new System.Drawing.Point(3, 25);
            this.tbfallos.MultiSelect = false;
            this.tbfallos.Name = "tbfallos";
            this.tbfallos.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tbfallos.RowHeadersVisible = false;
            this.tbfallos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.tbfallos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbfallos.ShowCellErrors = false;
            this.tbfallos.ShowCellToolTips = false;
            this.tbfallos.ShowEditingIcon = false;
            this.tbfallos.ShowRowErrors = false;
            this.tbfallos.Size = new System.Drawing.Size(834, 269);
            this.tbfallos.TabIndex = 1;
            this.tbfallos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbfallos_CellContentDoubleClick);
            this.tbfallos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbfallos_CellFormatting);
            this.tbfallos.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.tbfallos_ColumnAdded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idclasificación";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "GRUPO";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Descripcion
            // 
            this.Descripcion.HeaderText = "SUBGRUPO";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.ReadOnly = true;
            this.Descripcion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.usuario.Name = "usuario";
            this.usuario.ReadOnly = true;
            this.usuario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // idclasif
            // 
            this.idclasif.HeaderText = "idclasif";
            this.idclasif.Name = "idclasif";
            this.idclasif.ReadOnly = true;
            this.idclasif.Visible = false;
            // 
            // gbaddclasif
            // 
            this.gbaddclasif.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddclasif.Controls.Add(this.lnkLista);
            this.gbaddclasif.Controls.Add(this.label3);
            this.gbaddclasif.Controls.Add(this.label23);
            this.gbaddclasif.Controls.Add(this.cbclasificacion);
            this.gbaddclasif.Controls.Add(this.label2);
            this.gbaddclasif.Controls.Add(this.pCancelar);
            this.gbaddclasif.Controls.Add(this.lblsavemp);
            this.gbaddclasif.Controls.Add(this.btnsavemp);
            this.gbaddclasif.Controls.Add(this.txtgetdescfallo);
            this.gbaddclasif.Controls.Add(this.label21);
            this.gbaddclasif.Controls.Add(this.label22);
            this.gbaddclasif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbaddclasif.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddclasif.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddclasif.Location = new System.Drawing.Point(0, 0);
            this.gbaddclasif.Name = "gbaddclasif";
            this.gbaddclasif.Size = new System.Drawing.Size(840, 699);
            this.gbaddclasif.TabIndex = 30;
            this.gbaddclasif.TabStop = false;
            this.gbaddclasif.Text = "Agregar Descripción de Fallo";
            this.gbaddclasif.Visible = false;
            this.gbaddclasif.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddclasif_Paint);
            // 
            // lnkLista
            // 
            this.lnkLista.ActiveLinkColor = System.Drawing.Color.Crimson;
            this.lnkLista.AutoSize = true;
            this.lnkLista.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLista.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkLista.LinkColor = System.Drawing.Color.Crimson;
            this.lnkLista.Location = new System.Drawing.Point(562, 101);
            this.lnkLista.Name = "lnkLista";
            this.lnkLista.Size = new System.Drawing.Size(149, 21);
            this.lnkLista.TabIndex = 67;
            this.lnkLista.TabStop = true;
            this.lnkLista.Text = "Restablecer Grupo";
            this.lnkLista.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLista_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Crimson;
            this.label3.Location = new System.Drawing.Point(279, 374);
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
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(318, 374);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // cbclasificacion
            // 
            this.cbclasificacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbclasificacion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbclasificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbclasificacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbclasificacion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbclasificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbclasificacion.FormattingEnabled = true;
            this.cbclasificacion.Location = new System.Drawing.Point(323, 61);
            this.cbclasificacion.Name = "cbclasificacion";
            this.cbclasificacion.Size = new System.Drawing.Size(388, 26);
            this.cbclasificacion.TabIndex = 1;
            this.cbclasificacion.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbclasificacion_DrawItem);
            this.cbclasificacion.SelectedIndexChanged += new System.EventHandler(this.getCambios);
            this.cbclasificacion.SelectedValueChanged += new System.EventHandler(this.cbclasificacion_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 21);
            this.label2.TabIndex = 30;
            this.label2.Text = "Grupo:";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(539, 233);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(199, 129);
            this.pCancelar.TabIndex = 29;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(64, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 26;
            this.label1.Text = "Nuevo";
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(75, 25);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(50, 50);
            this.btnCancelEmpresa.TabIndex = 27;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Location = new System.Drawing.Point(363, 322);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(72, 21);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Guardar";
            this.lblsavemp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(378, 258);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(50, 50);
            this.btnsavemp.TabIndex = 6;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtgetdescfallo
            // 
            this.txtgetdescfallo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetdescfallo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetdescfallo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetdescfallo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetdescfallo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetdescfallo.Location = new System.Drawing.Point(324, 155);
            this.txtgetdescfallo.MaxLength = 100;
            this.txtgetdescfallo.Name = "txtgetdescfallo";
            this.txtgetdescfallo.ShortcutsEnabled = false;
            this.txtgetdescfallo.Size = new System.Drawing.Size(394, 18);
            this.txtgetdescfallo.TabIndex = 2;
            this.txtgetdescfallo.TextChanged += new System.EventHandler(this.getCambios);
            this.txtgetdescfallo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetclasificacion_KeyPress);
            this.txtgetdescfallo.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetdescfallo_Validating);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(78, 149);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(86, 21);
            this.label21.TabIndex = 0;
            this.label21.Text = "Subgrupo:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(321, 168);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(397, 9);
            this.label22.TabIndex = 0;
            this.label22.Text = "_________________________________________________________________________________" +
    "_________________";
            // 
            // pEliminarClasificacion
            // 
            this.pEliminarClasificacion.Controls.Add(this.lbldeletedesc);
            this.pEliminarClasificacion.Controls.Add(this.btndeletedesc);
            this.pEliminarClasificacion.Location = new System.Drawing.Point(28, 239);
            this.pEliminarClasificacion.Name = "pEliminarClasificacion";
            this.pEliminarClasificacion.Size = new System.Drawing.Size(196, 129);
            this.pEliminarClasificacion.TabIndex = 28;
            this.pEliminarClasificacion.Visible = false;
            // 
            // lbldeletedesc
            // 
            this.lbldeletedesc.AutoSize = true;
            this.lbldeletedesc.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletedesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletedesc.Location = new System.Drawing.Point(50, 88);
            this.lbldeletedesc.Name = "lbldeletedesc";
            this.lbldeletedesc.Size = new System.Drawing.Size(89, 21);
            this.lbldeletedesc.TabIndex = 26;
            this.lbldeletedesc.Text = "Desactivar";
            // 
            // btndeletedesc
            // 
            this.btndeletedesc.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeletedesc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeletedesc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeletedesc.FlatAppearance.BorderSize = 0;
            this.btndeletedesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeletedesc.Location = new System.Drawing.Point(68, 25);
            this.btndeletedesc.Name = "btndeletedesc";
            this.btndeletedesc.Size = new System.Drawing.Size(50, 50);
            this.btndeletedesc.TabIndex = 25;
            this.btndeletedesc.UseVisualStyleBackColor = true;
            this.btndeletedesc.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(840, 27);
            this.panel1.TabIndex = 32;
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
            this.button1.Location = new System.Drawing.Point(806, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 27);
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
            this.lbltitle.Location = new System.Drawing.Point(317, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(200, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Subgrupos";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // catDescFallos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(840, 699);
            this.Controls.Add(this.pEliminarClasificacion);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbclasif);
            this.Controls.Add(this.gbaddclasif);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "catDescFallos";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catDescFallos";
            this.Load += new System.EventHandler(this.catDescFallos_Load);
            this.gbclasif.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).EndInit();
            this.gbaddclasif.ResumeLayout(false);
            this.gbaddclasif.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.pEliminarClasificacion.ResumeLayout(false);
            this.pEliminarClasificacion.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbclasif;
        private System.Windows.Forms.DataGridView tbfallos;
        private System.Windows.Forms.GroupBox gbaddclasif;
        private System.Windows.Forms.ComboBox cbclasificacion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Panel pEliminarClasificacion;
        private System.Windows.Forms.Label lbldeletedesc;
        private System.Windows.Forms.Button btndeletedesc;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtgetdescfallo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.LinkLabel lnkLista;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn idclasif;
    }
}