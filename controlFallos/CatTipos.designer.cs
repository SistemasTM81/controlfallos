namespace controlFallos
{
    partial class CatTipos
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
            this.tbtipos = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puesto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.people = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbpuesto = new System.Windows.Forms.GroupBox();
            this.txtgetdescripción = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtgettipo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pdelete = new System.Windows.Forms.Panel();
            this.lbldelete = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbtipos)).BeginInit();
            this.gbpuesto.SuspendLayout();
            this.pcancel.SuspendLayout();
            this.pdelete.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(1000, 27);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown_1);
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
            this.button1.Location = new System.Drawing.Point(967, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 27);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(332, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(255, 24);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Text = "Catálogo deTipos de Licencia";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown_1);
            // 
            // tbtipos
            // 
            this.tbtipos.AllowUserToAddRows = false;
            this.tbtipos.AllowUserToDeleteRows = false;
            this.tbtipos.AllowUserToResizeColumns = false;
            this.tbtipos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbtipos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbtipos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbtipos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbtipos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbtipos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbtipos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbtipos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.puesto,
            this.descripcion,
            this.people,
            this.Estatus});
            this.tbtipos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbtipos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbtipos.EnableHeadersVisualStyles = false;
            this.tbtipos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbtipos.Location = new System.Drawing.Point(0, 347);
            this.tbtipos.MultiSelect = false;
            this.tbtipos.Name = "tbtipos";
            this.tbtipos.ReadOnly = true;
            this.tbtipos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.tbtipos.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbtipos.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbtipos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbtipos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbtipos.ShowCellErrors = false;
            this.tbtipos.ShowCellToolTips = false;
            this.tbtipos.ShowEditingIcon = false;
            this.tbtipos.Size = new System.Drawing.Size(1000, 186);
            this.tbtipos.TabIndex = 0;
            this.tbtipos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbtipos_CellDoubleClick);
            this.tbtipos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbtipos_CellFormatting);
            // 
            // id
            // 
            this.id.HeaderText = "idtipo";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 300;
            // 
            // puesto
            // 
            this.puesto.HeaderText = "TIPO";
            this.puesto.Name = "puesto";
            this.puesto.ReadOnly = true;
            this.puesto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.puesto.Width = 120;
            // 
            // descripcion
            // 
            this.descripcion.HeaderText = "DESCRIPCIÓN";
            this.descripcion.Name = "descripcion";
            this.descripcion.ReadOnly = true;
            this.descripcion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.descripcion.Width = 350;
            // 
            // people
            // 
            this.people.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.people.Name = "people";
            this.people.ReadOnly = true;
            this.people.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.people.Width = 350;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Estatus.Width = 180;
            // 
            // gbpuesto
            // 
            this.gbpuesto.AutoSize = true;
            this.gbpuesto.Controls.Add(this.txtgetdescripción);
            this.gbpuesto.Controls.Add(this.label4);
            this.gbpuesto.Controls.Add(this.label5);
            this.gbpuesto.Controls.Add(this.label22);
            this.gbpuesto.Controls.Add(this.label23);
            this.gbpuesto.Controls.Add(this.pcancel);
            this.gbpuesto.Controls.Add(this.lblsave);
            this.gbpuesto.Controls.Add(this.btnsave);
            this.gbpuesto.Controls.Add(this.txtgettipo);
            this.gbpuesto.Controls.Add(this.label1);
            this.gbpuesto.Controls.Add(this.label2);
            this.gbpuesto.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbpuesto.Location = new System.Drawing.Point(0, 27);
            this.gbpuesto.Name = "gbpuesto";
            this.gbpuesto.Size = new System.Drawing.Size(994, 317);
            this.gbpuesto.TabIndex = 0;
            this.gbpuesto.TabStop = false;
            this.gbpuesto.Text = "Nuevo Tipo De Licencia";
            this.gbpuesto.Visible = false;
            this.gbpuesto.Paint += new System.Windows.Forms.PaintEventHandler(this.gbpuesto_Paint);
            // 
            // txtgetdescripción
            // 
            this.txtgetdescripción.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetdescripción.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetdescripción.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetdescripción.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetdescripción.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetdescripción.Location = new System.Drawing.Point(309, 119);
            this.txtgetdescripción.MaxLength = 50;
            this.txtgetdescripción.Name = "txtgetdescripción";
            this.txtgetdescripción.ShortcutsEnabled = false;
            this.txtgetdescripción.Size = new System.Drawing.Size(568, 18);
            this.txtgetdescripción.TabIndex = 2;
            this.txtgetdescripción.TextChanged += new System.EventHandler(this.modificaciones);
            this.txtgetdescripción.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetdescripción_KeyPress);
            this.txtgetdescripción.Validating += new System.ComponentModel.CancelEventHandler(this.txtgettipo_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(171, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 24);
            this.label4.TabIndex = 31;
            this.label4.Text = "Descripción:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(308, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(569, 9);
            this.label5.TabIndex = 0;
            this.label5.Text = "_________________________________________________________________________________" +
    "____________________________________________________________";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Crimson;
            this.label22.Location = new System.Drawing.Point(475, 272);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(47, 18);
            this.label22.TabIndex = 27;
            this.label22.Text = "Nota:";
            this.label22.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(514, 272);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 28;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // pcancel
            // 
            this.pcancel.Controls.Add(this.label3);
            this.pcancel.Controls.Add(this.btncancel);
            this.pcancel.Location = new System.Drawing.Point(696, 178);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(127, 80);
            this.pcancel.TabIndex = 0;
            this.pcancel.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(33, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Nuevo";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancel.FlatAppearance.BorderSize = 0;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Location = new System.Drawing.Point(41, 0);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(50, 50);
            this.btncancel.TabIndex = 24;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.Location = new System.Drawing.Point(418, 234);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(79, 24);
            this.lblsave.TabIndex = 23;
            this.lblsave.Text = "Guardar";
            this.lblsave.Click += new System.EventHandler(this.lblsave_Click);
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Location = new System.Drawing.Point(432, 178);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 3;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtgettipo
            // 
            this.txtgettipo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgettipo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgettipo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgettipo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgettipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgettipo.Location = new System.Drawing.Point(310, 59);
            this.txtgettipo.MaxLength = 5;
            this.txtgettipo.Name = "txtgettipo";
            this.txtgettipo.ShortcutsEnabled = false;
            this.txtgettipo.Size = new System.Drawing.Size(92, 18);
            this.txtgettipo.TabIndex = 1;
            this.txtgettipo.TextChanged += new System.EventHandler(this.modificaciones);
            this.txtgettipo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgettipo_KeyPress);
            this.txtgettipo.Validating += new System.ComponentModel.CancelEventHandler(this.txtgettipo_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(171, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 24);
            this.label1.TabIndex = 18;
            this.label1.Text = "Tipo:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(309, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 9);
            this.label2.TabIndex = 0;
            this.label2.Text = "______________________";
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelete);
            this.pdelete.Controls.Add(this.btndelete);
            this.pdelete.Location = new System.Drawing.Point(58, 205);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(159, 80);
            this.pdelete.TabIndex = 0;
            this.pdelete.Visible = false;
            // 
            // lbldelete
            // 
            this.lbldelete.AutoSize = true;
            this.lbldelete.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelete.Location = new System.Drawing.Point(28, 58);
            this.lbldelete.Name = "lbldelete";
            this.lbldelete.Size = new System.Drawing.Size(98, 24);
            this.lbldelete.TabIndex = 0;
            this.lbldelete.Text = "Desactivar";
            // 
            // btndelete
            // 
            this.btndelete.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelete.FlatAppearance.BorderSize = 0;
            this.btndelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelete.Location = new System.Drawing.Point(54, 0);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(50, 50);
            this.btndelete.TabIndex = 0;
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // CatTipos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1000, 533);
            this.Controls.Add(this.pdelete);
            this.Controls.Add(this.tbtipos);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbpuesto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CatTipos";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CatTipos";
            this.Load += new System.EventHandler(this.CatTipos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbtipos)).EndInit();
            this.gbpuesto.ResumeLayout(false);
            this.gbpuesto.PerformLayout();
            this.pcancel.ResumeLayout(false);
            this.pcancel.PerformLayout();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.DataGridView tbtipos;
        private System.Windows.Forms.GroupBox gbpuesto;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtgettipo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtgetdescripción;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label lbldelete;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn puesto;
        private System.Windows.Forms.DataGridViewTextBoxColumn descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn people;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
    }
}