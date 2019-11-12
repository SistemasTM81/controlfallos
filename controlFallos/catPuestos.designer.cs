namespace controlFallos
{
    partial class catPuestos
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.pdelete = new System.Windows.Forms.Panel();
            this.lbldelete = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.tbpuestos = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puesto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.people = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbpuesto = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtgetpuesto = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pdelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbpuestos)).BeginInit();
            this.gbpuesto.SuspendLayout();
            this.pcancel.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(849, 27);
            this.panel1.TabIndex = 31;
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
            this.button1.Location = new System.Drawing.Point(816, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 27);
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
            this.lbltitle.Location = new System.Drawing.Point(332, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(177, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Puestos";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pdelete);
            this.panel2.Controls.Add(this.tbpuestos);
            this.panel2.Controls.Add(this.gbpuesto);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(849, 493);
            this.panel2.TabIndex = 32;
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelete);
            this.pdelete.Controls.Add(this.btndelete);
            this.pdelete.Location = new System.Drawing.Point(144, 149);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(159, 80);
            this.pdelete.TabIndex = 25;
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
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click_1);
            // 
            // tbpuestos
            // 
            this.tbpuestos.AllowUserToAddRows = false;
            this.tbpuestos.AllowUserToDeleteRows = false;
            this.tbpuestos.AllowUserToResizeColumns = false;
            this.tbpuestos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbpuestos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbpuestos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbpuestos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbpuestos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbpuestos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbpuestos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbpuestos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbpuestos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.puesto,
            this.people,
            this.Estatus,
            this.tipo});
            this.tbpuestos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbpuestos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbpuestos.EnableHeadersVisualStyles = false;
            this.tbpuestos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbpuestos.Location = new System.Drawing.Point(0, 303);
            this.tbpuestos.MultiSelect = false;
            this.tbpuestos.Name = "tbpuestos";
            this.tbpuestos.ReadOnly = true;
            this.tbpuestos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.tbpuestos.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbpuestos.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbpuestos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbpuestos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbpuestos.ShowCellErrors = false;
            this.tbpuestos.ShowCellToolTips = false;
            this.tbpuestos.ShowEditingIcon = false;
            this.tbpuestos.Size = new System.Drawing.Size(845, 186);
            this.tbpuestos.TabIndex = 0;
            this.tbpuestos.Visible = false;
            this.tbpuestos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gbpuestos_CellContentDoubleClick);
            this.tbpuestos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gbpuestos_CellFormatting_1);
            this.tbpuestos.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.gbpuestos_ColumnAdded);
            // 
            // id
            // 
            this.id.HeaderText = "idpuesto";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 300;
            // 
            // puesto
            // 
            this.puesto.HeaderText = "PUESTO";
            this.puesto.Name = "puesto";
            this.puesto.ReadOnly = true;
            this.puesto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.puesto.Width = 300;
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
            // tipo
            // 
            this.tipo.HeaderText = "tipo";
            this.tipo.Name = "tipo";
            this.tipo.ReadOnly = true;
            this.tipo.Visible = false;
            // 
            // gbpuesto
            // 
            this.gbpuesto.AutoSize = true;
            this.gbpuesto.Controls.Add(this.label22);
            this.gbpuesto.Controls.Add(this.label23);
            this.gbpuesto.Controls.Add(this.pcancel);
            this.gbpuesto.Controls.Add(this.lblsave);
            this.gbpuesto.Controls.Add(this.btnsave);
            this.gbpuesto.Controls.Add(this.txtgetpuesto);
            this.gbpuesto.Controls.Add(this.label1);
            this.gbpuesto.Controls.Add(this.label2);
            this.gbpuesto.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbpuesto.Location = new System.Drawing.Point(0, 0);
            this.gbpuesto.Name = "gbpuesto";
            this.gbpuesto.Size = new System.Drawing.Size(847, 297);
            this.gbpuesto.TabIndex = 33;
            this.gbpuesto.TabStop = false;
            this.gbpuesto.Text = "Nuevo Puesto";
            this.gbpuesto.Visible = false;
            this.gbpuesto.Paint += new System.Windows.Forms.PaintEventHandler(this.gbpuesto_Paint);
            this.gbpuesto.Enter += new System.EventHandler(this.gbpuesto_Enter);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Crimson;
            this.label22.Location = new System.Drawing.Point(310, 252);
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
            this.label23.Location = new System.Drawing.Point(349, 252);
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
            this.pcancel.Location = new System.Drawing.Point(544, 151);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(127, 80);
            this.pcancel.TabIndex = 26;
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
            this.lblsave.Location = new System.Drawing.Point(391, 207);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(74, 24);
            this.lblsave.TabIndex = 23;
            this.lblsave.Text = "Agregar";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Location = new System.Drawing.Point(405, 151);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 22;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtgetpuesto
            // 
            this.txtgetpuesto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetpuesto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetpuesto.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetpuesto.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetpuesto.Location = new System.Drawing.Point(365, 85);
            this.txtgetpuesto.MaxLength = 30;
            this.txtgetpuesto.Name = "txtgetpuesto";
            this.txtgetpuesto.ShortcutsEnabled = false;
            this.txtgetpuesto.Size = new System.Drawing.Size(289, 18);
            this.txtgetpuesto.TabIndex = 1;
            this.txtgetpuesto.TextChanged += new System.EventHandler(this.txtgetpuesto_TextChanged);
            this.txtgetpuesto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetpuesto_KeyPress_1);
            this.txtgetpuesto.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetpuesto_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(190, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 24);
            this.label1.TabIndex = 18;
            this.label1.Text = "Nombre del Puesto:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(361, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 9);
            this.label2.TabIndex = 19;
            this.label2.Text = "________________________________________________________________________";
            // 
            // catPuestos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(849, 520);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "catPuestos";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catPuestos";
            this.Load += new System.EventHandler(this.catPuestos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbpuestos)).EndInit();
            this.gbpuesto.ResumeLayout(false);
            this.gbpuesto.PerformLayout();
            this.pcancel.ResumeLayout(false);
            this.pcancel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView tbpuestos;
        private System.Windows.Forms.GroupBox gbpuesto;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label lbldelete;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtgetpuesto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn puesto;
        private System.Windows.Forms.DataGridViewTextBoxColumn people;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipo;
    }
}