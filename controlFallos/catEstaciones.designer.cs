namespace controlFallos
{
    partial class catEstaciones
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbanaqueles = new System.Windows.Forms.GroupBox();
            this.tbestaciones = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estaciondgv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.txtestacion = new System.Windows.Forms.TextBox();
            this.lbldelpa = new System.Windows.Forms.Label();
            this.pdelete = new System.Windows.Forms.Panel();
            this.btndelpa = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.gbaddanaquel = new System.Windows.Forms.GroupBox();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.gbanaqueles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbestaciones)).BeginInit();
            this.pdelete.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.gbaddanaquel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(865, 27);
            this.panel1.TabIndex = 37;
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
            this.button1.Location = new System.Drawing.Point(828, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 27);
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
            this.lbltitle.Location = new System.Drawing.Point(330, 3);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(203, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Estaciones";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(349, 191);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(434, 17);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(215, 35);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(88, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Estación:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(320, 50);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(293, 9);
            this.label22.TabIndex = 1;
            this.label22.Text = "________________________________________________________________________";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Crimson;
            this.label3.Location = new System.Drawing.Point(310, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 18);
            this.label3.TabIndex = 66;
            this.label3.Text = "Nota:";
            this.label3.Visible = false;
            // 
            // gbanaqueles
            // 
            this.gbanaqueles.Controls.Add(this.tbestaciones);
            this.gbanaqueles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbanaqueles.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbanaqueles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbanaqueles.Location = new System.Drawing.Point(0, 245);
            this.gbanaqueles.Name = "gbanaqueles";
            this.gbanaqueles.Size = new System.Drawing.Size(865, 262);
            this.gbanaqueles.TabIndex = 39;
            this.gbanaqueles.TabStop = false;
            this.gbanaqueles.Text = "Consulta de Estaciones";
            // 
            // tbestaciones
            // 
            this.tbestaciones.AllowUserToAddRows = false;
            this.tbestaciones.AllowUserToDeleteRows = false;
            this.tbestaciones.AllowUserToResizeColumns = false;
            this.tbestaciones.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbestaciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbestaciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbestaciones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbestaciones.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbestaciones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbestaciones.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbestaciones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbestaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbestaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.estaciondgv,
            this.usuario,
            this.Estatus});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbestaciones.DefaultCellStyle = dataGridViewCellStyle3;
            this.tbestaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbestaciones.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbestaciones.EnableHeadersVisualStyles = false;
            this.tbestaciones.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbestaciones.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbestaciones.Location = new System.Drawing.Point(3, 25);
            this.tbestaciones.MultiSelect = false;
            this.tbestaciones.Name = "tbestaciones";
            this.tbestaciones.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbestaciones.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.tbestaciones.RowHeadersVisible = false;
            this.tbestaciones.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbestaciones.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.tbestaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbestaciones.ShowCellErrors = false;
            this.tbestaciones.ShowCellToolTips = false;
            this.tbestaciones.ShowEditingIcon = false;
            this.tbestaciones.ShowRowErrors = false;
            this.tbestaciones.Size = new System.Drawing.Size(859, 234);
            this.tbestaciones.TabIndex = 0;
            this.tbestaciones.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbestaciones_CellDoubleClick);
            this.tbestaciones.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbestaciones_CellFormatting);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idpasillo";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // estaciondgv
            // 
            this.estaciondgv.HeaderText = "ESTACIÓN";
            this.estaciondgv.Name = "estaciondgv";
            this.estaciondgv.ReadOnly = true;
            this.estaciondgv.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsavemp.Location = new System.Drawing.Point(388, 133);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(60, 18);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Guardar";
            // 
            // txtestacion
            // 
            this.txtestacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtestacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtestacion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtestacion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtestacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtestacion.Location = new System.Drawing.Point(322, 39);
            this.txtestacion.MaxLength = 50;
            this.txtestacion.Name = "txtestacion";
            this.txtestacion.ShortcutsEnabled = false;
            this.txtestacion.Size = new System.Drawing.Size(291, 18);
            this.txtestacion.TabIndex = 1;
            this.txtestacion.TextChanged += new System.EventHandler(this.txtestacion_TextChanged);
            this.txtestacion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtestacion_KeyPress);
            // 
            // lbldelpa
            // 
            this.lbldelpa.AutoSize = true;
            this.lbldelpa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelpa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelpa.Location = new System.Drawing.Point(31, 40);
            this.lbldelpa.Name = "lbldelpa";
            this.lbldelpa.Size = new System.Drawing.Size(76, 18);
            this.lbldelpa.TabIndex = 0;
            this.lbldelpa.Text = "Desactivar";
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelpa);
            this.pdelete.Controls.Add(this.btndelpa);
            this.pdelete.Location = new System.Drawing.Point(216, 93);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(137, 65);
            this.pdelete.TabIndex = 0;
            this.pdelete.Visible = false;
            // 
            // btndelpa
            // 
            this.btndelpa.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelpa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelpa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelpa.FlatAppearance.BorderSize = 0;
            this.btndelpa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelpa.Location = new System.Drawing.Point(52, 3);
            this.btndelpa.Name = "btndelpa";
            this.btndelpa.Size = new System.Drawing.Size(35, 35);
            this.btndelpa.TabIndex = 0;
            this.btndelpa.UseVisualStyleBackColor = true;
            this.btndelpa.Click += new System.EventHandler(this.btndelpa_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(31, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nuevo";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(499, 95);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(114, 63);
            this.pCancelar.TabIndex = 0;
            this.pCancelar.Visible = false;
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnCancelEmpresa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelEmpresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelEmpresa.Location = new System.Drawing.Point(40, 3);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(35, 35);
            this.btnCancelEmpresa.TabIndex = 0;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // gbaddanaquel
            // 
            this.gbaddanaquel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddanaquel.Controls.Add(this.pdelete);
            this.gbaddanaquel.Controls.Add(this.pCancelar);
            this.gbaddanaquel.Controls.Add(this.lblsavemp);
            this.gbaddanaquel.Controls.Add(this.btnsavemp);
            this.gbaddanaquel.Controls.Add(this.txtestacion);
            this.gbaddanaquel.Controls.Add(this.label21);
            this.gbaddanaquel.Controls.Add(this.label22);
            this.gbaddanaquel.Controls.Add(this.label3);
            this.gbaddanaquel.Controls.Add(this.label23);
            this.gbaddanaquel.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddanaquel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddanaquel.Location = new System.Drawing.Point(0, 27);
            this.gbaddanaquel.Name = "gbaddanaquel";
            this.gbaddanaquel.Size = new System.Drawing.Size(865, 230);
            this.gbaddanaquel.TabIndex = 38;
            this.gbaddanaquel.TabStop = false;
            this.gbaddanaquel.Text = "Agregar Estación";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(398, 95);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(35, 35);
            this.btnsavemp.TabIndex = 2;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // catEstaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(865, 507);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbanaqueles);
            this.Controls.Add(this.gbaddanaquel);
            this.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(22)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "catEstaciones";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catEstaciones";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbanaqueles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbestaciones)).EndInit();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.gbaddanaquel.ResumeLayout(false);
            this.gbaddanaquel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbanaqueles;
        private System.Windows.Forms.DataGridView tbestaciones;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.TextBox txtestacion;
        private System.Windows.Forms.Label lbldelpa;
        private System.Windows.Forms.Button btndelpa;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.GroupBox gbaddanaquel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn estaciondgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
    }
}