namespace controlFallos
{
    partial class NombresFamilias
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
            this.gbaddfamilia = new System.Windows.Forms.GroupBox();
            this.lblsave = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.pdeletefam = new System.Windows.Forms.Panel();
            this.lbldeletefam = new System.Windows.Forms.Label();
            this.btndeleteuser = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.tbfamilia = new System.Windows.Forms.DataGridView();
            this.idfamilia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.familia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.people = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbaddfamilia.SuspendLayout();
            this.pcancel.SuspendLayout();
            this.pdeletefam.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfamilia)).BeginInit();
            this.SuspendLayout();
            // 
            // gbaddfamilia
            // 
            this.gbaddfamilia.Controls.Add(this.lblsave);
            this.gbaddfamilia.Controls.Add(this.label22);
            this.gbaddfamilia.Controls.Add(this.label23);
            this.gbaddfamilia.Controls.Add(this.pcancel);
            this.gbaddfamilia.Controls.Add(this.btnsave);
            this.gbaddfamilia.Controls.Add(this.txtnombre);
            this.gbaddfamilia.Controls.Add(this.label17);
            this.gbaddfamilia.Controls.Add(this.label18);
            this.gbaddfamilia.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddfamilia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddfamilia.Location = new System.Drawing.Point(89, 33);
            this.gbaddfamilia.Name = "gbaddfamilia";
            this.gbaddfamilia.Size = new System.Drawing.Size(600, 361);
            this.gbaddfamilia.TabIndex = 1;
            this.gbaddfamilia.TabStop = false;
            this.gbaddfamilia.Text = "Agregar Familia de Refacción";
            this.gbaddfamilia.Visible = false;
            this.gbaddfamilia.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddfamilia_Paint);
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblsave.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblsave.Location = new System.Drawing.Point(246, 275);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(78, 21);
            this.lblsave.TabIndex = 0;
            this.lblsave.Text = "Guardar";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Crimson;
            this.label22.Location = new System.Drawing.Point(48, 340);
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
            this.label23.Location = new System.Drawing.Point(87, 340);
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
            this.pcancel.Location = new System.Drawing.Point(415, 213);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(147, 88);
            this.pcancel.TabIndex = 42;
            this.pcancel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(41, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 24;
            this.label1.Text = "Nuevo";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancel.FlatAppearance.BorderSize = 0;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Location = new System.Drawing.Point(53, 8);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(50, 50);
            this.btncancel.TabIndex = 42;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
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
            this.btnsave.Location = new System.Drawing.Point(260, 220);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 3;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtnombre
            // 
            this.txtnombre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtnombre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtnombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtnombre.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtnombre.Location = new System.Drawing.Point(226, 99);
            this.txtnombre.MaxLength = 25;
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.ShortcutsEnabled = false;
            this.txtnombre.Size = new System.Drawing.Size(274, 18);
            this.txtnombre.TabIndex = 1;
            this.txtnombre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtnombre.TextChanged += new System.EventHandler(this.cambios);
            this.txtnombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnombre_KeyPress);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label17.Location = new System.Drawing.Point(224, 112);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(285, 9);
            this.label17.TabIndex = 0;
            this.label17.Text = "______________________________________________________________________";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label18.Location = new System.Drawing.Point(12, 101);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(176, 24);
            this.label18.TabIndex = 0;
            this.label18.Text = "Nombre de Familia:";
            // 
            // pdeletefam
            // 
            this.pdeletefam.Controls.Add(this.lbldeletefam);
            this.pdeletefam.Controls.Add(this.btndeleteuser);
            this.pdeletefam.Location = new System.Drawing.Point(114, 249);
            this.pdeletefam.Name = "pdeletefam";
            this.pdeletefam.Size = new System.Drawing.Size(115, 85);
            this.pdeletefam.TabIndex = 42;
            this.pdeletefam.Visible = false;
            // 
            // lbldeletefam
            // 
            this.lbldeletefam.AutoSize = true;
            this.lbldeletefam.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletefam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletefam.Location = new System.Drawing.Point(10, 57);
            this.lbldeletefam.Name = "lbldeletefam";
            this.lbldeletefam.Size = new System.Drawing.Size(105, 24);
            this.lbldeletefam.TabIndex = 24;
            this.lbldeletefam.Text = "Desactivar";
            // 
            // btndeleteuser
            // 
            this.btndeleteuser.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndeleteuser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeleteuser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteuser.FlatAppearance.BorderSize = 0;
            this.btndeleteuser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeleteuser.Location = new System.Drawing.Point(26, 5);
            this.btndeleteuser.Name = "btndeleteuser";
            this.btndeleteuser.Size = new System.Drawing.Size(50, 50);
            this.btndeleteuser.TabIndex = 23;
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
            this.panel1.Size = new System.Drawing.Size(772, 27);
            this.panel1.TabIndex = 43;
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
            this.button1.Location = new System.Drawing.Point(742, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 27);
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
            this.lbltitle.Location = new System.Drawing.Point(267, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(209, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Familias de Refacciones";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // tbfamilia
            // 
            this.tbfamilia.AllowUserToAddRows = false;
            this.tbfamilia.AllowUserToDeleteRows = false;
            this.tbfamilia.AllowUserToResizeColumns = false;
            this.tbfamilia.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfamilia.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbfamilia.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbfamilia.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbfamilia.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbfamilia.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbfamilia.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfamilia.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbfamilia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbfamilia.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idfamilia,
            this.familia,
            this.people,
            this.Estatus});
            this.tbfamilia.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbfamilia.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.tbfamilia.EnableHeadersVisualStyles = false;
            this.tbfamilia.GridColor = System.Drawing.Color.White;
            this.tbfamilia.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbfamilia.Location = new System.Drawing.Point(0, 402);
            this.tbfamilia.Margin = new System.Windows.Forms.Padding(5);
            this.tbfamilia.MultiSelect = false;
            this.tbfamilia.Name = "tbfamilia";
            this.tbfamilia.ReadOnly = true;
            this.tbfamilia.RowHeadersVisible = false;
            this.tbfamilia.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbfamilia.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbfamilia.RowTemplate.ReadOnly = true;
            this.tbfamilia.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbfamilia.ShowCellErrors = false;
            this.tbfamilia.ShowCellToolTips = false;
            this.tbfamilia.ShowEditingIcon = false;
            this.tbfamilia.ShowRowErrors = false;
            this.tbfamilia.Size = new System.Drawing.Size(772, 253);
            this.tbfamilia.TabIndex = 44;
            this.tbfamilia.Visible = false;
            this.tbfamilia.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbfamilia_CellContentDoubleClick);
            this.tbfamilia.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbfamilia_CellFormatting);
            // 
            // idfamilia
            // 
            this.idfamilia.HeaderText = "idfamilia";
            this.idfamilia.Name = "idfamilia";
            this.idfamilia.ReadOnly = true;
            this.idfamilia.Visible = false;
            // 
            // familia
            // 
            this.familia.HeaderText = "FAMILIA";
            this.familia.Name = "familia";
            this.familia.ReadOnly = true;
            this.familia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // people
            // 
            this.people.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.people.Name = "people";
            this.people.ReadOnly = true;
            this.people.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estatus
            // 
            this.Estatus.HeaderText = "ESTATUS";
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // NombresFamilias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(772, 655);
            this.Controls.Add(this.tbfamilia);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pdeletefam);
            this.Controls.Add(this.gbaddfamilia);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NombresFamilias";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nombres Familias";
            this.Load += new System.EventHandler(this.NombresFamilias_Load);
            this.gbaddfamilia.ResumeLayout(false);
            this.gbaddfamilia.PerformLayout();
            this.pcancel.ResumeLayout(false);
            this.pcancel.PerformLayout();
            this.pdeletefam.ResumeLayout(false);
            this.pdeletefam.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbfamilia)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbaddfamilia;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel pdeletefam;
        private System.Windows.Forms.Label lbldeletefam;
        private System.Windows.Forms.Button btndeleteuser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.DataGridView tbfamilia;
        private System.Windows.Forms.DataGridViewTextBoxColumn idfamilia;
        private System.Windows.Forms.DataGridViewTextBoxColumn familia;
        private System.Windows.Forms.DataGridViewTextBoxColumn people;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
    }
}