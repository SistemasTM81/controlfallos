namespace controlFallos
{
    partial class catPasillos
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbaddpasillo = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbpasillos = new System.Windows.Forms.GroupBox();
            this.tbubicaciones = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pdelete = new System.Windows.Forms.Panel();
            this.lbldelpa = new System.Windows.Forms.Label();
            this.btndelpa = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtpasillo = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbaddpasillo.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbpasillos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbubicaciones)).BeginInit();
            this.pdelete.SuspendLayout();
            this.pCancelar.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(611, 27);
            this.panel1.TabIndex = 31;
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
            this.button1.Location = new System.Drawing.Point(582, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 27);
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
            this.lbltitle.Location = new System.Drawing.Point(223, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(176, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Pasillos";
            // 
            // gbaddpasillo
            // 
            this.gbaddpasillo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddpasillo.Controls.Add(this.panel2);
            this.gbaddpasillo.Controls.Add(this.pdelete);
            this.gbaddpasillo.Controls.Add(this.label2);
            this.gbaddpasillo.Controls.Add(this.label23);
            this.gbaddpasillo.Controls.Add(this.pCancelar);
            this.gbaddpasillo.Controls.Add(this.lblsavemp);
            this.gbaddpasillo.Controls.Add(this.btnsavemp);
            this.gbaddpasillo.Controls.Add(this.txtpasillo);
            this.gbaddpasillo.Controls.Add(this.label21);
            this.gbaddpasillo.Controls.Add(this.label22);
            this.gbaddpasillo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbaddpasillo.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddpasillo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddpasillo.Location = new System.Drawing.Point(0, 27);
            this.gbaddpasillo.Name = "gbaddpasillo";
            this.gbaddpasillo.Size = new System.Drawing.Size(611, 525);
            this.gbaddpasillo.TabIndex = 32;
            this.gbaddpasillo.TabStop = false;
            this.gbaddpasillo.Text = "Agregar Pasillo";
            this.gbaddpasillo.Visible = false;
            this.gbaddpasillo.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddpasillo_Paint);
            this.gbaddpasillo.Enter += new System.EventHandler(this.gbaddpasillo_Enter);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gbpasillos);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 314);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(605, 208);
            this.panel2.TabIndex = 67;
            // 
            // gbpasillos
            // 
            this.gbpasillos.Controls.Add(this.tbubicaciones);
            this.gbpasillos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbpasillos.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbpasillos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbpasillos.Location = new System.Drawing.Point(0, 0);
            this.gbpasillos.Name = "gbpasillos";
            this.gbpasillos.Size = new System.Drawing.Size(605, 208);
            this.gbpasillos.TabIndex = 33;
            this.gbpasillos.TabStop = false;
            this.gbpasillos.Text = "Consulta de Pasillos";
            this.gbpasillos.Visible = false;
            this.gbpasillos.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddpasillo_Paint);
            // 
            // tbubicaciones
            // 
            this.tbubicaciones.AllowUserToAddRows = false;
            this.tbubicaciones.AllowUserToDeleteRows = false;
            this.tbubicaciones.AllowUserToResizeColumns = false;
            this.tbubicaciones.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbubicaciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbubicaciones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbubicaciones.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbubicaciones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbubicaciones.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbubicaciones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbubicaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbubicaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.usuario,
            this.Estatus});
            this.tbubicaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbubicaciones.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbubicaciones.EnableHeadersVisualStyles = false;
            this.tbubicaciones.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbubicaciones.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbubicaciones.Location = new System.Drawing.Point(3, 25);
            this.tbubicaciones.MultiSelect = false;
            this.tbubicaciones.Name = "tbubicaciones";
            this.tbubicaciones.ReadOnly = true;
            this.tbubicaciones.RowHeadersVisible = false;
            this.tbubicaciones.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.tbubicaciones.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbubicaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbubicaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbubicaciones.ShowCellErrors = false;
            this.tbubicaciones.ShowCellToolTips = false;
            this.tbubicaciones.ShowEditingIcon = false;
            this.tbubicaciones.ShowRowErrors = false;
            this.tbubicaciones.Size = new System.Drawing.Size(599, 180);
            this.tbubicaciones.TabIndex = 0;
            this.tbubicaciones.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbubicaciones_CellContentDoubleClick);
            this.tbubicaciones.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbubicaciones_CellFormatting);
            this.tbubicaciones.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.tbubicaciones_CellMouseClick);
            this.tbubicaciones.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.tbubicaciones_ColumnAdded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idpasillo";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 85;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "PASILLO";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // usuario
            // 
            this.usuario.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.usuario.Name = "usuario";
            this.usuario.ReadOnly = true;
            this.usuario.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.usuario.Width = 350;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Estatus.Width = 150;
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelpa);
            this.pdelete.Controls.Add(this.btndelpa);
            this.pdelete.Location = new System.Drawing.Point(100, 137);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(144, 65);
            this.pdelete.TabIndex = 0;
            this.pdelete.Visible = false;
            // 
            // lbldelpa
            // 
            this.lbldelpa.AutoSize = true;
            this.lbldelpa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelpa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelpa.Location = new System.Drawing.Point(33, 45);
            this.lbldelpa.Name = "lbldelpa";
            this.lbldelpa.Size = new System.Drawing.Size(76, 18);
            this.lbldelpa.TabIndex = 0;
            this.lbldelpa.Text = "Desactivar";
            this.lbldelpa.Click += new System.EventHandler(this.label9_Click);
            // 
            // btndelpa
            // 
            this.btndelpa.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelpa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelpa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelpa.FlatAppearance.BorderSize = 0;
            this.btndelpa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelpa.Location = new System.Drawing.Point(51, 3);
            this.btndelpa.Name = "btndelpa";
            this.btndelpa.Size = new System.Drawing.Size(35, 35);
            this.btndelpa.TabIndex = 0;
            this.btndelpa.UseVisualStyleBackColor = true;
            this.btndelpa.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(12, 208);
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
            this.label23.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(-3, 226);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(434, 17);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(386, 141);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(105, 63);
            this.pCancelar.TabIndex = 0;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(27, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 18);
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
            this.btnCancelEmpresa.Location = new System.Drawing.Point(36, 3);
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.Size = new System.Drawing.Size(35, 35);
            this.btnCancelEmpresa.TabIndex = 0;
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // lblsavemp
            // 
            this.lblsavemp.AutoSize = true;
            this.lblsavemp.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsavemp.Location = new System.Drawing.Point(276, 182);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(57, 18);
            this.lblsavemp.TabIndex = 13;
            this.lblsavemp.Text = "Agregar";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsavemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsavemp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsavemp.Location = new System.Drawing.Point(289, 141);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(35, 35);
            this.btnsavemp.TabIndex = 2;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtpasillo
            // 
            this.txtpasillo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtpasillo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtpasillo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtpasillo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpasillo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtpasillo.Location = new System.Drawing.Point(289, 64);
            this.txtpasillo.MaxLength = 12;
            this.txtpasillo.Name = "txtpasillo";
            this.txtpasillo.ShortcutsEnabled = false;
            this.txtpasillo.Size = new System.Drawing.Size(75, 18);
            this.txtpasillo.TabIndex = 1;
            this.txtpasillo.TextChanged += new System.EventHandler(this.getCambios);
            this.txtpasillo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtpasillo_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(218, 64);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Pasillo:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(287, 77);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 9);
            this.label22.TabIndex = 1;
            this.label22.Text = "__________________";
            // 
            // catPasillos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(611, 552);
            this.Controls.Add(this.gbaddpasillo);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "catPasillos";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catPasillos";
            this.Load += new System.EventHandler(this.catPasillos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbaddpasillo.ResumeLayout(false);
            this.gbaddpasillo.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.gbpasillos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbubicaciones)).EndInit();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.GroupBox gbaddpasillo;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label lbldelpa;
        private System.Windows.Forms.Button btndelpa;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtpasillo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox gbpasillos;
        private System.Windows.Forms.DataGridView tbubicaciones;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.Panel panel2;
    }
}