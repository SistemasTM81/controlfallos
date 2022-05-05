namespace controlFallos
{
    partial class catCategorias
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
            this.lbltitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbldeletedesc = new System.Windows.Forms.Label();
            this.pEliminarClasificacion = new System.Windows.Forms.Panel();
            this.btndeletedesc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkLista = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cbgrupo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.txtgetcategoria = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tbcategorias = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categodgv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idclasif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idsubgrupo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbclasif = new System.Windows.Forms.GroupBox();
            this.gbcatego = new System.Windows.Forms.GroupBox();
            this.cbsubgrupo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pEliminarClasificacion.SuspendLayout();
            this.pCancelar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcategorias)).BeginInit();
            this.gbclasif.SuspendLayout();
            this.gbcatego.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(333, 1);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(199, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Categorías";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(844, 27);
            this.panel1.TabIndex = 36;
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
            this.button1.Location = new System.Drawing.Point(812, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 27);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbldeletedesc
            // 
            this.lbldeletedesc.AutoSize = true;
            this.lbldeletedesc.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletedesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletedesc.Location = new System.Drawing.Point(3, 65);
            this.lbldeletedesc.Name = "lbldeletedesc";
            this.lbldeletedesc.Size = new System.Drawing.Size(89, 21);
            this.lbldeletedesc.TabIndex = 26;
            this.lbldeletedesc.Text = "Desactivar";
            // 
            // pEliminarClasificacion
            // 
            this.pEliminarClasificacion.Controls.Add(this.lbldeletedesc);
            this.pEliminarClasificacion.Controls.Add(this.btndeletedesc);
            this.pEliminarClasificacion.Location = new System.Drawing.Point(115, 335);
            this.pEliminarClasificacion.Name = "pEliminarClasificacion";
            this.pEliminarClasificacion.Size = new System.Drawing.Size(98, 94);
            this.pEliminarClasificacion.TabIndex = 33;
            this.pEliminarClasificacion.Visible = false;
            // 
            // btndeletedesc
            // 
            this.btndeletedesc.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeletedesc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeletedesc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeletedesc.FlatAppearance.BorderSize = 0;
            this.btndeletedesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeletedesc.Location = new System.Drawing.Point(23, 6);
            this.btndeletedesc.Name = "btndeletedesc";
            this.btndeletedesc.Size = new System.Drawing.Size(50, 50);
            this.btndeletedesc.TabIndex = 25;
            this.btndeletedesc.UseVisualStyleBackColor = true;
            this.btndeletedesc.Click += new System.EventHandler(this.btndeletedesc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(5, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
            this.label1.TabIndex = 26;
            this.label1.Text = "Nuevo";
            // 
            // lnkLista
            // 
            this.lnkLista.ActiveLinkColor = System.Drawing.Color.Crimson;
            this.lnkLista.AutoSize = true;
            this.lnkLista.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLista.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkLista.LinkColor = System.Drawing.Color.Crimson;
            this.lnkLista.Location = new System.Drawing.Point(562, 99);
            this.lnkLista.Name = "lnkLista";
            this.lnkLista.Size = new System.Drawing.Size(149, 21);
            this.lnkLista.TabIndex = 0;
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
            this.label3.Location = new System.Drawing.Point(279, 426);
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
            this.label23.Location = new System.Drawing.Point(318, 426);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // cbgrupo
            // 
            this.cbgrupo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbgrupo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbgrupo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbgrupo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbgrupo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbgrupo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbgrupo.FormattingEnabled = true;
            this.cbgrupo.Location = new System.Drawing.Point(323, 61);
            this.cbgrupo.Name = "cbgrupo";
            this.cbgrupo.Size = new System.Drawing.Size(388, 26);
            this.cbgrupo.TabIndex = 1;
            this.cbgrupo.SelectedIndexChanged += new System.EventHandler(this.cbgrupo_SelectedIndexChanged);
            this.cbgrupo.SelectedValueChanged += new System.EventHandler(this.cambiosEdicion);
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
            this.pCancelar.Location = new System.Drawing.Point(569, 303);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(72, 90);
            this.pCancelar.TabIndex = 29;
            this.pCancelar.Visible = false;
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(11, 11);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(50, 50);
            this.btnCancelEmpresa.TabIndex = 27;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Location = new System.Drawing.Point(369, 370);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(72, 21);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Guardar";
            this.lblsavemp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtgetcategoria
            // 
            this.txtgetcategoria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetcategoria.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetcategoria.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetcategoria.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetcategoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetcategoria.Location = new System.Drawing.Point(324, 217);
            this.txtgetcategoria.MaxLength = 100;
            this.txtgetcategoria.Name = "txtgetcategoria";
            this.txtgetcategoria.ShortcutsEnabled = false;
            this.txtgetcategoria.Size = new System.Drawing.Size(394, 18);
            this.txtgetcategoria.TabIndex = 3;
            this.txtgetcategoria.TextChanged += new System.EventHandler(this.cambiosEdicion);
            this.txtgetcategoria.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetcategoria_KeyPress);
            this.txtgetcategoria.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetcategoria_Validating);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(78, 220);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(84, 21);
            this.label21.TabIndex = 0;
            this.label21.Text = "Categoría:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(321, 230);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(397, 9);
            this.label22.TabIndex = 0;
            this.label22.Text = "_________________________________________________________________________________" +
    "_________________";
            // 
            // tbcategorias
            // 
            this.tbcategorias.AllowUserToAddRows = false;
            this.tbcategorias.AllowUserToDeleteRows = false;
            this.tbcategorias.AllowUserToResizeColumns = false;
            this.tbcategorias.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbcategorias.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbcategorias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbcategorias.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbcategorias.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbcategorias.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbcategorias.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbcategorias.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbcategorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbcategorias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Descripcion,
            this.categodgv,
            this.usuario,
            this.Estatus,
            this.idclasif,
            this.idsubgrupo});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbcategorias.DefaultCellStyle = dataGridViewCellStyle3;
            this.tbcategorias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcategorias.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbcategorias.EnableHeadersVisualStyles = false;
            this.tbcategorias.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbcategorias.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbcategorias.Location = new System.Drawing.Point(3, 25);
            this.tbcategorias.MultiSelect = false;
            this.tbcategorias.Name = "tbcategorias";
            this.tbcategorias.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbcategorias.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tbcategorias.RowHeadersVisible = false;
            this.tbcategorias.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbcategorias.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.tbcategorias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbcategorias.ShowCellErrors = false;
            this.tbcategorias.ShowCellToolTips = false;
            this.tbcategorias.ShowEditingIcon = false;
            this.tbcategorias.ShowRowErrors = false;
            this.tbcategorias.Size = new System.Drawing.Size(838, 269);
            this.tbcategorias.TabIndex = 1;
            this.tbcategorias.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbcategorias_CellDoubleClick);
            this.tbcategorias.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbcategorias_CellFormatting);
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
            // categodgv
            // 
            this.categodgv.HeaderText = "CATEGORÍA";
            this.categodgv.Name = "categodgv";
            this.categodgv.ReadOnly = true;
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
            // idsubgrupo
            // 
            this.idsubgrupo.HeaderText = "idsubgrupo";
            this.idsubgrupo.Name = "idsubgrupo";
            this.idsubgrupo.ReadOnly = true;
            this.idsubgrupo.Visible = false;
            // 
            // gbclasif
            // 
            this.gbclasif.Controls.Add(this.tbcategorias);
            this.gbclasif.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbclasif.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbclasif.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbclasif.Location = new System.Drawing.Point(0, 499);
            this.gbclasif.Name = "gbclasif";
            this.gbclasif.Size = new System.Drawing.Size(844, 297);
            this.gbclasif.TabIndex = 35;
            this.gbclasif.TabStop = false;
            this.gbclasif.Text = "Consulta de Categorías";
            this.gbclasif.Visible = false;
            // 
            // gbcatego
            // 
            this.gbcatego.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbcatego.Controls.Add(this.cbsubgrupo);
            this.gbcatego.Controls.Add(this.label4);
            this.gbcatego.Controls.Add(this.lnkLista);
            this.gbcatego.Controls.Add(this.label3);
            this.gbcatego.Controls.Add(this.label23);
            this.gbcatego.Controls.Add(this.cbgrupo);
            this.gbcatego.Controls.Add(this.label2);
            this.gbcatego.Controls.Add(this.pCancelar);
            this.gbcatego.Controls.Add(this.lblsavemp);
            this.gbcatego.Controls.Add(this.btnsavemp);
            this.gbcatego.Controls.Add(this.txtgetcategoria);
            this.gbcatego.Controls.Add(this.label21);
            this.gbcatego.Controls.Add(this.label22);
            this.gbcatego.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbcatego.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbcatego.Location = new System.Drawing.Point(0, 33);
            this.gbcatego.Name = "gbcatego";
            this.gbcatego.Size = new System.Drawing.Size(844, 460);
            this.gbcatego.TabIndex = 34;
            this.gbcatego.TabStop = false;
            this.gbcatego.Text = "Agregar Categoría";
            this.gbcatego.Visible = false;
            // 
            // cbsubgrupo
            // 
            this.cbsubgrupo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbsubgrupo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbsubgrupo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbsubgrupo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbsubgrupo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbsubgrupo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbsubgrupo.FormattingEnabled = true;
            this.cbsubgrupo.Location = new System.Drawing.Point(323, 143);
            this.cbsubgrupo.Name = "cbsubgrupo";
            this.cbsubgrupo.Size = new System.Drawing.Size(388, 26);
            this.cbsubgrupo.TabIndex = 2;
            this.cbsubgrupo.SelectedIndexChanged += new System.EventHandler(this.cbsubgrupo_SelectedIndexChanged);
            this.cbsubgrupo.SelectedValueChanged += new System.EventHandler(this.cambiosEdicion);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 21);
            this.label4.TabIndex = 69;
            this.label4.Text = "Subgrupo:";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(378, 310);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(50, 50);
            this.btnsavemp.TabIndex = 4;
            this.btnsavemp.TabStop = false;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // catCategorias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(844, 796);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pEliminarClasificacion);
            this.Controls.Add(this.gbclasif);
            this.Controls.Add(this.gbcatego);
            this.Font = new System.Drawing.Font("Garamond", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "catCategorias";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catCategorias";
            this.Load += new System.EventHandler(this.catCategorias_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pEliminarClasificacion.ResumeLayout(false);
            this.pEliminarClasificacion.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcategorias)).EndInit();
            this.gbclasif.ResumeLayout(false);
            this.gbcatego.ResumeLayout(false);
            this.gbcatego.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbldeletedesc;
        private System.Windows.Forms.Button btndeletedesc;
        private System.Windows.Forms.Panel pEliminarClasificacion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.LinkLabel lnkLista;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cbgrupo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtgetcategoria;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DataGridView tbcategorias;
        private System.Windows.Forms.GroupBox gbclasif;
        private System.Windows.Forms.GroupBox gbcatego;
        private System.Windows.Forms.ComboBox cbsubgrupo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn categodgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn idclasif;
        private System.Windows.Forms.DataGridViewTextBoxColumn idsubgrupo;
    }
}