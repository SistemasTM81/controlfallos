namespace controlFallos
{
    partial class catAreas
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
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.cbempresa = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.gbaddarea = new System.Windows.Forms.GroupBox();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pCancelar = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.txtid = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.pdelete = new System.Windows.Forms.Panel();
            this.lbldelpa = new System.Windows.Forms.Label();
            this.btndelpa = new System.Windows.Forms.Button();
            this.gbareas = new System.Windows.Forms.GroupBox();
            this.tbareas = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._p = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombreArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idpasillo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.gbaddarea.SuspendLayout();
            this.pCancelar.SuspendLayout();
            this.pdelete.SuspendLayout();
            this.gbareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbareas)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(879, 27);
            this.panel1.TabIndex = 37;
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
            this.button1.Location = new System.Drawing.Point(843, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 27);
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
            this.lbltitle.Location = new System.Drawing.Point(284, 3);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(165, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = " Catálogo de Areas";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Crimson;
            this.label3.Location = new System.Drawing.Point(169, 336);
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
            this.label23.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(212, 338);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(434, 17);
            this.label23.TabIndex = 65;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // cbempresa
            // 
            this.cbempresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbempresa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbempresa.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbempresa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbempresa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbempresa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbempresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbempresa.FormattingEnabled = true;
            this.cbempresa.Location = new System.Drawing.Point(361, 56);
            this.cbempresa.MaxDropDownItems = 50;
            this.cbempresa.Name = "cbempresa";
            this.cbempresa.Size = new System.Drawing.Size(314, 26);
            this.cbempresa.TabIndex = 1;
            this.cbempresa.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbempresa_DrawItem);
            this.cbempresa.SelectedIndexChanged += new System.EventHandler(this.cbempresa_SelectedIndexChanged);
            this.cbempresa.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(190, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "Empresa:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(359, 119);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(169, 9);
            this.label22.TabIndex = 0;
            this.label22.Text = "_________________________________________";
            // 
            // gbaddarea
            // 
            this.gbaddarea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddarea.Controls.Add(this.txtnombre);
            this.gbaddarea.Controls.Add(this.label4);
            this.gbaddarea.Controls.Add(this.label5);
            this.gbaddarea.Controls.Add(this.label3);
            this.gbaddarea.Controls.Add(this.label23);
            this.gbaddarea.Controls.Add(this.cbempresa);
            this.gbaddarea.Controls.Add(this.label2);
            this.gbaddarea.Controls.Add(this.pCancelar);
            this.gbaddarea.Controls.Add(this.lblsavemp);
            this.gbaddarea.Controls.Add(this.btnsavemp);
            this.gbaddarea.Controls.Add(this.txtid);
            this.gbaddarea.Controls.Add(this.label21);
            this.gbaddarea.Controls.Add(this.label22);
            this.gbaddarea.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbaddarea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddarea.Location = new System.Drawing.Point(0, 30);
            this.gbaddarea.Name = "gbaddarea";
            this.gbaddarea.Size = new System.Drawing.Size(879, 382);
            this.gbaddarea.TabIndex = 0;
            this.gbaddarea.TabStop = false;
            this.gbaddarea.Text = "Agregar Área";
            this.gbaddarea.Visible = false;
            this.gbaddarea.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddarea_Paint);
            this.gbaddarea.Enter += new System.EventHandler(this.gbaddarea_Enter);
            // 
            // txtnombre
            // 
            this.txtnombre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtnombre.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtnombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtnombre.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtnombre.Location = new System.Drawing.Point(361, 165);
            this.txtnombre.MaxLength = 40;
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.ShortcutsEnabled = false;
            this.txtnombre.Size = new System.Drawing.Size(271, 18);
            this.txtnombre.TabIndex = 3;
            this.txtnombre.TextChanged += new System.EventHandler(this.getCambios);
            this.txtnombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtnombre_KeyPress);
            this.txtnombre.Validating += new System.ComponentModel.CancelEventHandler(this.txtid_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(190, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 24);
            this.label4.TabIndex = 67;
            this.label4.Text = "Nombre del Área:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(359, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(273, 9);
            this.label5.TabIndex = 0;
            this.label5.Text = "___________________________________________________________________";
            // 
            // pCancelar
            // 
            this.pCancelar.Controls.Add(this.label1);
            this.pCancelar.Controls.Add(this.btnCancelEmpresa);
            this.pCancelar.Location = new System.Drawing.Point(560, 248);
            this.pCancelar.Name = "pCancelar";
            this.pCancelar.Size = new System.Drawing.Size(114, 63);
            this.pCancelar.TabIndex = 0;
            this.pCancelar.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(34, 41);
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
            this.btnCancelEmpresa.Location = new System.Drawing.Point(40, 3);
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
            this.lblsavemp.Location = new System.Drawing.Point(426, 290);
            this.lblsavemp.Name = "lblsavemp";
            this.lblsavemp.Size = new System.Drawing.Size(60, 18);
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
            this.btnsavemp.Location = new System.Drawing.Point(438, 252);
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.Size = new System.Drawing.Size(35, 35);
            this.btnsavemp.TabIndex = 4;
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // txtid
            // 
            this.txtid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtid.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtid.Location = new System.Drawing.Point(363, 107);
            this.txtid.MaxLength = 10;
            this.txtid.Name = "txtid";
            this.txtid.ShortcutsEnabled = false;
            this.txtid.Size = new System.Drawing.Size(160, 18);
            this.txtid.TabIndex = 2;
            this.txtid.TextChanged += new System.EventHandler(this.getCambios);
            this.txtid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtid_KeyPress);
            this.txtid.Validating += new System.ComponentModel.CancelEventHandler(this.txtid_Validating);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(190, 104);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(123, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "Identificador:";
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelpa);
            this.pdelete.Controls.Add(this.btndelpa);
            this.pdelete.Location = new System.Drawing.Point(222, 278);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(137, 65);
            this.pdelete.TabIndex = 0;
            this.pdelete.Visible = false;
            // 
            // lbldelpa
            // 
            this.lbldelpa.AutoSize = true;
            this.lbldelpa.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelpa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelpa.Location = new System.Drawing.Point(27, 41);
            this.lbldelpa.Name = "lbldelpa";
            this.lbldelpa.Size = new System.Drawing.Size(76, 18);
            this.lbldelpa.TabIndex = 0;
            this.lbldelpa.Text = "Desactivar";
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
            // gbareas
            // 
            this.gbareas.Controls.Add(this.tbareas);
            this.gbareas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbareas.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbareas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbareas.Location = new System.Drawing.Point(0, 418);
            this.gbareas.Name = "gbareas";
            this.gbareas.Size = new System.Drawing.Size(879, 364);
            this.gbareas.TabIndex = 39;
            this.gbareas.TabStop = false;
            this.gbareas.Text = "Consulta de Areas";
            this.gbareas.Visible = false;
            this.gbareas.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddarea_Paint);
            // 
            // tbareas
            // 
            this.tbareas.AllowUserToAddRows = false;
            this.tbareas.AllowUserToDeleteRows = false;
            this.tbareas.AllowUserToResizeColumns = false;
            this.tbareas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbareas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbareas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbareas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbareas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbareas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbareas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbareas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbareas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbareas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this._p,
            this.dataGridViewTextBoxColumn2,
            this.nombreArea,
            this.usuario,
            this.Estatus,
            this.idpasillo});
            this.tbareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbareas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tbareas.EnableHeadersVisualStyles = false;
            this.tbareas.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbareas.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbareas.Location = new System.Drawing.Point(3, 25);
            this.tbareas.MultiSelect = false;
            this.tbareas.Name = "tbareas";
            this.tbareas.ReadOnly = true;
            this.tbareas.RowHeadersVisible = false;
            this.tbareas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            this.tbareas.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbareas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbareas.ShowCellErrors = false;
            this.tbareas.ShowCellToolTips = false;
            this.tbareas.ShowEditingIcon = false;
            this.tbareas.ShowRowErrors = false;
            this.tbareas.Size = new System.Drawing.Size(873, 336);
            this.tbareas.TabIndex = 0;
            this.tbareas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbareas_CellContentDoubleClick);
            this.tbareas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbareas_CellFormatting);
            this.tbareas.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.tbareas_ColumnAdded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "idarea";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // _p
            // 
            this._p.HeaderText = "EMPRESA PERTENECIENTE";
            this._p.Name = "_p";
            this._p.ReadOnly = true;
            this._p.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "IDENTIFICADOR";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nombreArea
            // 
            this.nombreArea.HeaderText = "NOMBRE DEL ÁREA";
            this.nombreArea.Name = "nombreArea";
            this.nombreArea.ReadOnly = true;
            this.nombreArea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // idpasillo
            // 
            this.idpasillo.HeaderText = "idempresa";
            this.idpasillo.Name = "idpasillo";
            this.idpasillo.ReadOnly = true;
            this.idpasillo.Visible = false;
            // 
            // catAreas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(879, 782);
            this.Controls.Add(this.pdelete);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbaddarea);
            this.Controls.Add(this.gbareas);
            this.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "catAreas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catAreas";
            this.Load += new System.EventHandler(this.catAreas_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbaddarea.ResumeLayout(false);
            this.gbaddarea.PerformLayout();
            this.pCancelar.ResumeLayout(false);
            this.pCancelar.PerformLayout();
            this.pdelete.ResumeLayout(false);
            this.pdelete.PerformLayout();
            this.gbareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbareas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox gbaddarea;
        private System.Windows.Forms.Panel pCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Label lbldelpa;
        private System.Windows.Forms.Button btndelpa;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox gbareas;
        private System.Windows.Forms.DataGridView tbareas;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn _p;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn usuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn idpasillo;
        public System.Windows.Forms.ComboBox cbempresa;
    }
}