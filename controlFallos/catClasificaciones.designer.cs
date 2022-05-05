namespace controlFallos
{
    partial class catClasificaciones
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
            this.gbaddClalif = new System.Windows.Forms.GroupBox();
            this.pEliminarClasificacion = new System.Windows.Forms.Panel();
            this.lbldelcla = new System.Windows.Forms.Label();
            this.btndelcla = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtgetclasificacion = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.gbclasif = new System.Windows.Forms.GroupBox();
            this.tbfallos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbaddClalif.SuspendLayout();
            this.pEliminarClasificacion.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.gbclasif.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbaddClalif
            // 
            this.gbaddClalif.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddClalif.Controls.Add(this.pEliminarClasificacion);
            this.gbaddClalif.Controls.Add(this.label2);
            this.gbaddClalif.Controls.Add(this.label23);
            this.gbaddClalif.Controls.Add(this.pCancelar);
            this.gbaddClalif.Controls.Add(this.lblsavemp);
            this.gbaddClalif.Controls.Add(this.btnsavemp);
            this.gbaddClalif.Controls.Add(this.txtgetclasificacion);
            this.gbaddClalif.Controls.Add(this.label21);
            this.gbaddClalif.Controls.Add(this.label22);
            this.gbaddClalif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbaddClalif.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddClalif.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddClalif.Location = new System.Drawing.Point(0, 0);
            this.gbaddClalif.Name = "gbaddClalif";
            this.gbaddClalif.Size = new System.Drawing.Size(789, 648);
            this.gbaddClalif.TabIndex = 0;
            this.gbaddClalif.TabStop = false;
            this.gbaddClalif.Text = "Agregar Clasificación de Fallo";
            this.gbaddClalif.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddClalif_Paint);
            // 
            // pEliminarClasificacion
            // 
            this.pEliminarClasificacion.Controls.Add(this.lbldelcla);
            this.pEliminarClasificacion.Controls.Add(this.btndelcla);
            this.pEliminarClasificacion.Location = new System.Drawing.Point(43, 213);
            this.pEliminarClasificacion.Name = "pEliminarClasificacion";
            this.pEliminarClasificacion.Size = new System.Drawing.Size(94, 84);
            this.pEliminarClasificacion.TabIndex = 0;
            this.pEliminarClasificacion.Visible = false;
            // 
            // lbldelcla
            // 
            this.lbldelcla.AutoSize = true;
            this.lbldelcla.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelcla.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelcla.Location = new System.Drawing.Point(-3, 56);
            this.lbldelcla.Name = "lbldelcla";
            this.lbldelcla.Size = new System.Drawing.Size(98, 24);
            this.lbldelcla.TabIndex = 0;
            this.lbldelcla.Text = "Desactivar";
            // 
            // btndelcla
            // 
            this.btndelcla.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelcla.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelcla.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelcla.FlatAppearance.BorderSize = 0;
            this.btndelcla.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelcla.Location = new System.Drawing.Point(19, 3);
            this.btndelcla.Name = "btndelcla";
            this.btndelcla.Size = new System.Drawing.Size(50, 50);
            this.btndelcla.TabIndex = 0;
            this.btndelcla.UseVisualStyleBackColor = true;
            this.btndelcla.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(258, 312);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 18);
            this.label2.TabIndex = 66;
            this.label2.Text = "Nota:";
            this.label2.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(297, 312);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(592, 216);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(87, 84);
            this.pCancelar.TabIndex = 0;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(15, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 24);
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
            this.btnCancelEmpresa.Location = new System.Drawing.Point(19, 3);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(50, 50);
            this.btnCancelEmpresa.TabIndex = 0;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Location = new System.Drawing.Point(326, 272);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(79, 24);
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
            this.btnsavemp.Location = new System.Drawing.Point(341, 219);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(50, 50);
            this.btnsavemp.TabIndex = 2;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtgetclasificacion
            // 
            this.txtgetclasificacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetclasificacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetclasificacion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetclasificacion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetclasificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetclasificacion.Location = new System.Drawing.Point(330, 122);
            this.txtgetclasificacion.MaxLength = 100;
            this.txtgetclasificacion.Name = "txtgetclasificacion";
            this.txtgetclasificacion.ShortcutsEnabled = false;
            this.txtgetclasificacion.Size = new System.Drawing.Size(267, 18);
            this.txtgetclasificacion.TabIndex = 1;
            this.txtgetclasificacion.TextChanged += new System.EventHandler(this.txtgetclasificacion_TextChanged);
            this.txtgetclasificacion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetcempresa_KeyPress);
            this.txtgetclasificacion.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetclasificacion_Validating);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(181, 118);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Grupo:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(328, 135);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(269, 9);
            this.label22.TabIndex = 1;
            this.label22.Text = "__________________________________________________________________";
            // 
            // gbclasif
            // 
            this.gbclasif.Controls.Add(this.tbfallos);
            this.gbclasif.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbclasif.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbclasif.Location = new System.Drawing.Point(0, 340);
            this.gbclasif.Name = "gbclasif";
            this.gbclasif.Size = new System.Drawing.Size(789, 308);
            this.gbclasif.TabIndex = 29;
            this.gbclasif.TabStop = false;
            this.gbclasif.Text = "Consulta Clasificaciones de Fallo";
            this.gbclasif.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddClalif_Paint);
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
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbfallos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbfallos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbfallos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfallos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbfallos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbfallos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.usuario,
            this.Estatus});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbfallos.DefaultCellStyle = dataGridViewCellStyle3;
            this.tbfallos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbfallos.EnableHeadersVisualStyles = false;
            this.tbfallos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfallos.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbfallos.Location = new System.Drawing.Point(3, 25);
            this.tbfallos.MultiSelect = false;
            this.tbfallos.Name = "tbfallos";
            this.tbfallos.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.tbfallos.Size = new System.Drawing.Size(783, 280);
            this.tbfallos.TabIndex = 0;
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
            this.dataGridViewTextBoxColumn1.Width = 300;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 284.7716F;
            this.dataGridViewTextBoxColumn2.HeaderText = "GRUPO";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 350;
            // 
            // usuario
            // 
            this.usuario.FillWeight = 7.614212F;
            this.usuario.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.usuario.Name = "usuario";
            this.usuario.ReadOnly = true;
            this.usuario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.usuario.Width = 270;
            // 
            // Estatus
            // 
            this.Estatus.FillWeight = 7.614212F;
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Estatus.Width = 150;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(789, 27);
            this.panel1.TabIndex = 30;
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
            this.button1.Location = new System.Drawing.Point(757, 0);
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
            this.lbltitle.Location = new System.Drawing.Point(287, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(176, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Grupos";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // catClasificaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(789, 648);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbclasif);
            this.Controls.Add(this.gbaddClalif);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "catClasificaciones";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.catClasificaciones_Load);
            this.gbaddClalif.ResumeLayout(false);
            this.gbaddClalif.PerformLayout();
            this.pEliminarClasificacion.ResumeLayout(false);
            this.pEliminarClasificacion.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.gbclasif.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbfallos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbaddClalif;
        private System.Windows.Forms.Panel pEliminarClasificacion;
        private System.Windows.Forms.Label lbldelcla;
        private System.Windows.Forms.Button btndelcla;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtgetclasificacion;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox gbclasif;
        private System.Windows.Forms.DataGridView tbfallos;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
    }
}