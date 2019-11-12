namespace controlFallos
{
    partial class marcas
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
            this.gbadd = new System.Windows.Forms.GroupBox();
            this.cbdesc = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbfamilia = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtmarca = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pdeletefam = new System.Windows.Forms.Panel();
            this.lbldeletefam = new System.Windows.Forms.Label();
            this.btndeleteuser = new System.Windows.Forms.Button();
            this.gbconsulta = new System.Windows.Forms.GroupBox();
            this.tbmarcas = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.paddunidadMedida = new System.Windows.Forms.Panel();
            this.btnaddpasillo = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.gbadd.SuspendLayout();
            this.pcancel.SuspendLayout();
            this.pdeletefam.SuspendLayout();
            this.gbconsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbmarcas)).BeginInit();
            this.panel1.SuspendLayout();
            this.paddunidadMedida.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbadd
            // 
            this.gbadd.Controls.Add(this.cbdesc);
            this.gbadd.Controls.Add(this.label4);
            this.gbadd.Controls.Add(this.cbfamilia);
            this.gbadd.Controls.Add(this.label12);
            this.gbadd.Controls.Add(this.label2);
            this.gbadd.Controls.Add(this.label3);
            this.gbadd.Controls.Add(this.pcancel);
            this.gbadd.Controls.Add(this.lblsave);
            this.gbadd.Controls.Add(this.btnsave);
            this.gbadd.Controls.Add(this.txtmarca);
            this.gbadd.Controls.Add(this.label22);
            this.gbadd.Controls.Add(this.label23);
            this.gbadd.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbadd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbadd.Location = new System.Drawing.Point(0, 0);
            this.gbadd.Name = "gbadd";
            this.gbadd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbadd.Size = new System.Drawing.Size(671, 595);
            this.gbadd.TabIndex = 0;
            this.gbadd.TabStop = false;
            this.gbadd.Text = "Agregar Marca";
            this.gbadd.Visible = false;
            this.gbadd.Paint += new System.Windows.Forms.PaintEventHandler(this.gbadd_Paint);
            this.gbadd.Enter += new System.EventHandler(this.gbadd_Enter);
            // 
            // cbdesc
            // 
            this.cbdesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbdesc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbdesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbdesc.Enabled = false;
            this.cbdesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbdesc.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbdesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbdesc.FormattingEnabled = true;
            this.cbdesc.Location = new System.Drawing.Point(171, 183);
            this.cbdesc.Name = "cbdesc";
            this.cbdesc.Size = new System.Drawing.Size(314, 26);
            this.cbdesc.TabIndex = 2;
            this.cbdesc.SelectedIndexChanged += new System.EventHandler(this.cbdesc_SelectedIndexChanged);
            this.cbdesc.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label4.Location = new System.Drawing.Point(38, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 24);
            this.label4.TabIndex = 91;
            this.label4.Text = "Descripción:";
            // 
            // cbfamilia
            // 
            this.cbfamilia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbfamilia.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbfamilia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbfamilia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbfamilia.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbfamilia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbfamilia.FormattingEnabled = true;
            this.cbfamilia.Location = new System.Drawing.Point(171, 110);
            this.cbfamilia.Name = "cbfamilia";
            this.cbfamilia.Size = new System.Drawing.Size(314, 26);
            this.cbfamilia.TabIndex = 1;
            this.cbfamilia.SelectedIndexChanged += new System.EventHandler(this.cbfamilia_SelectedIndexChanged);
            this.cbfamilia.SelectedValueChanged += new System.EventHandler(this.getCambios);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label12.Location = new System.Drawing.Point(38, 109);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 24);
            this.label12.TabIndex = 90;
            this.label12.Text = "Familia:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(71, 489);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 18);
            this.label2.TabIndex = 66;
            this.label2.Text = "Nota:";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(110, 489);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(474, 18);
            this.label3.TabIndex = 65;
            this.label3.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label3.Visible = false;
            // 
            // pcancel
            // 
            this.pcancel.Controls.Add(this.label1);
            this.pcancel.Controls.Add(this.btncancel);
            this.pcancel.Location = new System.Drawing.Point(384, 376);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(170, 85);
            this.pcancel.TabIndex = 44;
            this.pcancel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(54, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 24;
            this.label1.Text = "Nuevo";
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btncancel.FlatAppearance.BorderSize = 0;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Location = new System.Drawing.Point(63, 3);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(50, 50);
            this.btncancel.TabIndex = 44;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblsave.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblsave.Location = new System.Drawing.Point(267, 431);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(80, 24);
            this.lblsave.TabIndex = 0;
            this.lblsave.Text = "Agregar";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.btnsave.Location = new System.Drawing.Point(281, 379);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 4;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtmarca
            // 
            this.txtmarca.AllowDrop = true;
            this.txtmarca.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtmarca.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtmarca.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtmarca.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtmarca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtmarca.Location = new System.Drawing.Point(171, 256);
            this.txtmarca.MaxLength = 60;
            this.txtmarca.Name = "txtmarca";
            this.txtmarca.ShortcutsEnabled = false;
            this.txtmarca.Size = new System.Drawing.Size(314, 18);
            this.txtmarca.TabIndex = 3;
            this.txtmarca.TextChanged += new System.EventHandler(this.getCambios);
            this.txtmarca.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtmarca_KeyPress);
            this.txtmarca.Validating += new System.ComponentModel.CancelEventHandler(this.txtmarca_Validating);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label22.Location = new System.Drawing.Point(169, 269);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(317, 9);
            this.label22.TabIndex = 0;
            this.label22.Text = "______________________________________________________________________________";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(38, 257);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(60, 21);
            this.label23.TabIndex = 0;
            this.label23.Text = "Marca:";
            // 
            // pdeletefam
            // 
            this.pdeletefam.Controls.Add(this.lbldeletefam);
            this.pdeletefam.Controls.Add(this.btndeleteuser);
            this.pdeletefam.Location = new System.Drawing.Point(69, 379);
            this.pdeletefam.Name = "pdeletefam";
            this.pdeletefam.Size = new System.Drawing.Size(170, 82);
            this.pdeletefam.TabIndex = 43;
            this.pdeletefam.Visible = false;
            // 
            // lbldeletefam
            // 
            this.lbldeletefam.AutoSize = true;
            this.lbldeletefam.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeletefam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldeletefam.Location = new System.Drawing.Point(32, 52);
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
            this.btndeleteuser.Location = new System.Drawing.Point(57, 2);
            this.btndeleteuser.Name = "btndeleteuser";
            this.btndeleteuser.Size = new System.Drawing.Size(50, 50);
            this.btndeleteuser.TabIndex = 23;
            this.btndeleteuser.UseVisualStyleBackColor = true;
            this.btndeleteuser.Click += new System.EventHandler(this.btndeleteuser_Click);
            // 
            // gbconsulta
            // 
            this.gbconsulta.Controls.Add(this.tbmarcas);
            this.gbconsulta.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbconsulta.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbconsulta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbconsulta.Location = new System.Drawing.Point(677, 0);
            this.gbconsulta.Name = "gbconsulta";
            this.gbconsulta.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbconsulta.Size = new System.Drawing.Size(773, 595);
            this.gbconsulta.TabIndex = 0;
            this.gbconsulta.TabStop = false;
            this.gbconsulta.Text = "Consulta de Marcas de Refacciones";
            this.gbconsulta.Visible = false;
            this.gbconsulta.Paint += new System.Windows.Forms.PaintEventHandler(this.gbadd_Paint);
            // 
            // tbmarcas
            // 
            this.tbmarcas.AllowUserToAddRows = false;
            this.tbmarcas.AllowUserToDeleteRows = false;
            this.tbmarcas.AllowUserToResizeColumns = false;
            this.tbmarcas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbmarcas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbmarcas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbmarcas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbmarcas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbmarcas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbmarcas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbmarcas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbmarcas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbmarcas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbmarcas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.tbmarcas.EnableHeadersVisualStyles = false;
            this.tbmarcas.GridColor = System.Drawing.Color.White;
            this.tbmarcas.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbmarcas.Location = new System.Drawing.Point(3, 27);
            this.tbmarcas.MultiSelect = false;
            this.tbmarcas.Name = "tbmarcas";
            this.tbmarcas.ReadOnly = true;
            this.tbmarcas.RowHeadersVisible = false;
            this.tbmarcas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbmarcas.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tbmarcas.RowTemplate.ReadOnly = true;
            this.tbmarcas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbmarcas.ShowCellErrors = false;
            this.tbmarcas.ShowCellToolTips = false;
            this.tbmarcas.ShowEditingIcon = false;
            this.tbmarcas.ShowRowErrors = false;
            this.tbmarcas.Size = new System.Drawing.Size(767, 565);
            this.tbmarcas.TabIndex = 0;
            this.tbmarcas.TabStop = false;
            this.tbmarcas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbmarcas_CellContentDoubleClick);
            this.tbmarcas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbmarcas_CellFormatting);
            this.tbmarcas.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.tbmarcas_ColumnAdded);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(509, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(126, 112);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources._goto;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(49, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 35);
            this.button1.TabIndex = 0;
            this.button1.TabStop = false;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label5.Location = new System.Drawing.Point(0, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 63);
            this.label5.TabIndex = 0;
            this.label5.Text = "Ir a Catálogo \r\nde Nombres \r\nde Familia";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // paddunidadMedida
            // 
            this.paddunidadMedida.Controls.Add(this.btnaddpasillo);
            this.paddunidadMedida.Controls.Add(this.label13);
            this.paddunidadMedida.Location = new System.Drawing.Point(494, 234);
            this.paddunidadMedida.Name = "paddunidadMedida";
            this.paddunidadMedida.Size = new System.Drawing.Size(144, 113);
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
            this.label13.Size = new System.Drawing.Size(138, 63);
            this.label13.TabIndex = 0;
            this.label13.Text = "Ir a Catálogo \r\nde Descripciones\r\nde Familia";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // marcas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1450, 595);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.paddunidadMedida);
            this.Controls.Add(this.pdeletefam);
            this.Controls.Add(this.gbadd);
            this.Controls.Add(this.gbconsulta);
            this.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "marcas";
            this.Text = "marcas";
            this.Load += new System.EventHandler(this.marcas_Load);
            this.gbadd.ResumeLayout(false);
            this.gbadd.PerformLayout();
            this.pcancel.ResumeLayout(false);
            this.pcancel.PerformLayout();
            this.pdeletefam.ResumeLayout(false);
            this.pdeletefam.PerformLayout();
            this.gbconsulta.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbmarcas)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.paddunidadMedida.ResumeLayout(false);
            this.paddunidadMedida.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbadd;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtmarca;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox gbconsulta;
        private System.Windows.Forms.DataGridView tbmarcas;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Panel pdeletefam;
        private System.Windows.Forms.Label lbldeletefam;
        private System.Windows.Forms.Button btndeleteuser;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox cbdesc;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox cbfamilia;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel paddunidadMedida;
        private System.Windows.Forms.Button btnaddpasillo;
        private System.Windows.Forms.Label label13;
    }
}