namespace controlFallos
{
    partial class MaterialProduccion
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCancelar = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.LblGuardar = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.groupBoxObs = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.labelidFinal = new System.Windows.Forms.Label();
            this.lblNomUsuario = new System.Windows.Forms.Label();
            this.txtDispenso = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.lblusuario = new System.Windows.Forms.Label();
            this.cmbMecanico = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNomMecanico = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtFecha = new System.Windows.Forms.DateTimePicker();
            this.lblFecha = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtcodigo = new System.Windows.Forms.TextBox();
            this.lblMedida = new System.Windows.Forms.Label();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.lblNomRef = new System.Windows.Forms.Label();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.dgImprimir = new System.Windows.Forms.DataGridView();
            this.COD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RECACCION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CAN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FEC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PERSONAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.motivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxObs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgImprimir)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 27);
            this.panel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Crimson;
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources.delete;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(757, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 27);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Cerrar);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Crimson;
            this.label1.Font = new System.Drawing.Font("Garamond", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(214, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Material Para Producir";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCancelar);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.LblGuardar);
            this.groupBox1.Controls.Add(this.btnGuardar);
            this.groupBox1.Controls.Add(this.groupBoxObs);
            this.groupBox1.Controls.Add(this.lblNomUsuario);
            this.groupBox1.Controls.Add(this.txtDispenso);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.lblusuario);
            this.groupBox1.Controls.Add(this.cmbMecanico);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtNomMecanico);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtFecha);
            this.groupBox1.Controls.Add(this.lblFecha);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCantidad);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.txtcodigo);
            this.groupBox1.Controls.Add(this.lblMedida);
            this.groupBox1.Controls.Add(this.lblCantidad);
            this.groupBox1.Controls.Add(this.lblNomRef);
            this.groupBox1.Controls.Add(this.lblCodigo);
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 437);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // lblCancelar
            // 
            this.lblCancelar.AutoSize = true;
            this.lblCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCancelar.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblCancelar.Location = new System.Drawing.Point(324, 394);
            this.lblCancelar.Name = "lblCancelar";
            this.lblCancelar.Size = new System.Drawing.Size(98, 18);
            this.lblCancelar.TabIndex = 285;
            this.lblCancelar.Text = "CANCELAR";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.BackgroundImage = global::controlFallos.Properties.Resources.cross;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.Location = new System.Drawing.Point(247, 373);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(55, 50);
            this.button2.TabIndex = 283;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // LblGuardar
            // 
            this.LblGuardar.AutoSize = true;
            this.LblGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LblGuardar.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.LblGuardar.Location = new System.Drawing.Point(88, 394);
            this.LblGuardar.Name = "LblGuardar";
            this.LblGuardar.Size = new System.Drawing.Size(89, 18);
            this.LblGuardar.TabIndex = 284;
            this.LblGuardar.Text = "GUARDAR";
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnGuardar.BackgroundImage = global::controlFallos.Properties.Resources.save1;
            this.btnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGuardar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnGuardar.Location = new System.Drawing.Point(24, 373);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(55, 50);
            this.btnGuardar.TabIndex = 282;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btn_Guardar);
            // 
            // groupBoxObs
            // 
            this.groupBoxObs.Controls.Add(this.label5);
            this.groupBoxObs.Controls.Add(this.txtMotivo);
            this.groupBoxObs.Controls.Add(this.labelidFinal);
            this.groupBoxObs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxObs.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxObs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.groupBoxObs.Location = new System.Drawing.Point(10, 237);
            this.groupBoxObs.Name = "groupBoxObs";
            this.groupBoxObs.Size = new System.Drawing.Size(748, 120);
            this.groupBoxObs.TabIndex = 281;
            this.groupBoxObs.TabStop = false;
            this.groupBoxObs.Text = "MOTIVO DE SALIDA:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(953, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 18);
            this.label5.TabIndex = 174;
            this.label5.Text = "0";
            // 
            // txtMotivo
            // 
            this.txtMotivo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtMotivo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMotivo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMotivo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMotivo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtMotivo.Location = new System.Drawing.Point(6, 18);
            this.txtMotivo.MaxLength = 50;
            this.txtMotivo.Multiline = true;
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.ShortcutsEnabled = false;
            this.txtMotivo.Size = new System.Drawing.Size(736, 95);
            this.txtMotivo.TabIndex = 5;
            // 
            // labelidFinal
            // 
            this.labelidFinal.AutoSize = true;
            this.labelidFinal.Location = new System.Drawing.Point(11, 22);
            this.labelidFinal.Name = "labelidFinal";
            this.labelidFinal.Size = new System.Drawing.Size(87, 18);
            this.labelidFinal.TabIndex = 215;
            this.labelidFinal.Text = "Facturar A:";
            // 
            // lblNomUsuario
            // 
            this.lblNomUsuario.AutoSize = true;
            this.lblNomUsuario.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.lblNomUsuario.Location = new System.Drawing.Point(103, 203);
            this.lblNomUsuario.Name = "lblNomUsuario";
            this.lblNomUsuario.Size = new System.Drawing.Size(0, 21);
            this.lblNomUsuario.TabIndex = 280;
            // 
            // txtDispenso
            // 
            this.txtDispenso.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtDispenso.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDispenso.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDispenso.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtDispenso.Location = new System.Drawing.Point(107, 178);
            this.txtDispenso.MaxLength = 18;
            this.txtDispenso.Name = "txtDispenso";
            this.txtDispenso.PasswordChar = '*';
            this.txtDispenso.ShortcutsEnabled = false;
            this.txtDispenso.Size = new System.Drawing.Size(271, 18);
            this.txtDispenso.TabIndex = 4;
            this.txtDispenso.Validated += new System.EventHandler(this.nombrealmacen);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label15.Location = new System.Drawing.Point(105, 191);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(273, 9);
            this.label15.TabIndex = 279;
            this.label15.Text = "___________________________________________________________________";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblusuario
            // 
            this.lblusuario.AutoSize = true;
            this.lblusuario.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.lblusuario.Location = new System.Drawing.Point(6, 176);
            this.lblusuario.Name = "lblusuario";
            this.lblusuario.Size = new System.Drawing.Size(99, 21);
            this.lblusuario.TabIndex = 278;
            this.lblusuario.Text = "Contraseña:";
            // 
            // cmbMecanico
            // 
            this.cmbMecanico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbMecanico.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbMecanico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMecanico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMecanico.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMecanico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbMecanico.FormattingEnabled = true;
            this.cmbMecanico.Location = new System.Drawing.Point(447, 126);
            this.cmbMecanico.Name = "cmbMecanico";
            this.cmbMecanico.Size = new System.Drawing.Size(259, 25);
            this.cmbMecanico.TabIndex = 3;
            this.cmbMecanico.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbDrawable);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label4.Location = new System.Drawing.Point(445, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(249, 9);
            this.label4.TabIndex = 276;
            this.label4.Text = "_____________________________________________________________";
            this.label4.Visible = false;
            // 
            // txtNomMecanico
            // 
            this.txtNomMecanico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtNomMecanico.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNomMecanico.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNomMecanico.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomMecanico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtNomMecanico.Location = new System.Drawing.Point(447, 125);
            this.txtNomMecanico.MaxLength = 50;
            this.txtNomMecanico.Name = "txtNomMecanico";
            this.txtNomMecanico.ShortcutsEnabled = false;
            this.txtNomMecanico.Size = new System.Drawing.Size(259, 18);
            this.txtNomMecanico.TabIndex = 275;
            this.txtNomMecanico.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.label3.Location = new System.Drawing.Point(322, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 42);
            this.label3.TabIndex = 274;
            this.label3.Text = "Mecanico Que\r\nSolicita:";
            // 
            // dtFecha
            // 
            this.dtFecha.CalendarFont = new System.Drawing.Font("Garamond", 12F);
            this.dtFecha.Enabled = false;
            this.dtFecha.Font = new System.Drawing.Font("Garamond", 12F);
            this.dtFecha.Location = new System.Drawing.Point(61, 120);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Size = new System.Drawing.Size(255, 25);
            this.dtFecha.TabIndex = 273;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.lblFecha.Location = new System.Drawing.Point(6, 121);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(59, 21);
            this.lblFecha.TabIndex = 272;
            this.lblFecha.Text = "Fecha:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.Location = new System.Drawing.Point(321, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 9);
            this.label2.TabIndex = 264;
            this.label2.Text = "_____________________________________";
            // 
            // txtCantidad
            // 
            this.txtCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtCantidad.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCantidad.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCantidad.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtCantidad.Location = new System.Drawing.Point(323, 17);
            this.txtCantidad.MaxLength = 10;
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.ShortcutsEnabled = false;
            this.txtCantidad.Size = new System.Drawing.Size(151, 18);
            this.txtCantidad.TabIndex = 2;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label27.Location = new System.Drawing.Point(68, 35);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(153, 9);
            this.label27.TabIndex = 257;
            this.label27.Text = "_____________________________________";
            // 
            // txtcodigo
            // 
            this.txtcodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtcodigo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtcodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtcodigo.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcodigo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtcodigo.Location = new System.Drawing.Point(70, 18);
            this.txtcodigo.MaxLength = 15;
            this.txtcodigo.Name = "txtcodigo";
            this.txtcodigo.ShortcutsEnabled = false;
            this.txtcodigo.Size = new System.Drawing.Size(151, 18);
            this.txtcodigo.TabIndex = 1;
            this.txtcodigo.Validated += new System.EventHandler(this.codigo_Validate);
            // 
            // lblMedida
            // 
            this.lblMedida.AutoSize = true;
            this.lblMedida.Location = new System.Drawing.Point(495, 21);
            this.lblMedida.Name = "lblMedida";
            this.lblMedida.Size = new System.Drawing.Size(0, 13);
            this.lblMedida.TabIndex = 262;
            // 
            // lblCantidad
            // 
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.lblCantidad.Location = new System.Drawing.Point(239, 15);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(82, 21);
            this.lblCantidad.TabIndex = 261;
            this.lblCantidad.Text = "Cantidad:";
            // 
            // lblNomRef
            // 
            this.lblNomRef.Location = new System.Drawing.Point(7, 47);
            this.lblNomRef.Name = "lblNomRef";
            this.lblNomRef.Size = new System.Drawing.Size(183, 56);
            this.lblNomRef.TabIndex = 260;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.lblCodigo.Location = new System.Drawing.Point(6, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(69, 21);
            this.lblCodigo.TabIndex = 259;
            this.lblCodigo.Text = "Codigo:";
            // 
            // dgImprimir
            // 
            this.dgImprimir.AllowUserToAddRows = false;
            this.dgImprimir.AllowUserToDeleteRows = false;
            this.dgImprimir.AllowUserToResizeColumns = false;
            this.dgImprimir.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(22)))));
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgImprimir.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgImprimir.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgImprimir.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgImprimir.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgImprimir.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgImprimir.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgImprimir.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgImprimir.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgImprimir.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COD,
            this.RECACCION,
            this.CAN,
            this.UM,
            this.FEC,
            this.MAN,
            this.PERSONAL,
            this.motivo});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgImprimir.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgImprimir.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgImprimir.EnableHeadersVisualStyles = false;
            this.dgImprimir.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgImprimir.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.dgImprimir.Location = new System.Drawing.Point(12, 485);
            this.dgImprimir.MultiSelect = false;
            this.dgImprimir.Name = "dgImprimir";
            this.dgImprimir.ReadOnly = true;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgImprimir.RowHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgImprimir.RowHeadersVisible = false;
            this.dgImprimir.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.Crimson;
            this.dgImprimir.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dgImprimir.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgImprimir.ShowCellErrors = false;
            this.dgImprimir.ShowCellToolTips = false;
            this.dgImprimir.ShowEditingIcon = false;
            this.dgImprimir.ShowRowErrors = false;
            this.dgImprimir.Size = new System.Drawing.Size(766, 218);
            this.dgImprimir.TabIndex = 275;
            // 
            // COD
            // 
            this.COD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.COD.FillWeight = 37.7339F;
            this.COD.HeaderText = "CODIGO";
            this.COD.Name = "COD";
            this.COD.ReadOnly = true;
            this.COD.Width = 114;
            // 
            // RECACCION
            // 
            this.RECACCION.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RECACCION.HeaderText = "NOMBRE REFACCION";
            this.RECACCION.Name = "RECACCION";
            this.RECACCION.ReadOnly = true;
            this.RECACCION.Width = 210;
            // 
            // CAN
            // 
            this.CAN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CAN.FillWeight = 6.944829F;
            this.CAN.HeaderText = "CANTIDAD";
            this.CAN.Name = "CAN";
            this.CAN.ReadOnly = true;
            this.CAN.Width = 137;
            // 
            // UM
            // 
            this.UM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.UM.FillWeight = 6.944829F;
            this.UM.HeaderText = "UNIDAD MEDIDA";
            this.UM.Name = "UM";
            this.UM.ReadOnly = true;
            this.UM.Width = 178;
            // 
            // FEC
            // 
            this.FEC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FEC.FillWeight = 6.944829F;
            this.FEC.HeaderText = "FECHA";
            this.FEC.Name = "FEC";
            this.FEC.ReadOnly = true;
            this.FEC.Width = 97;
            // 
            // MAN
            // 
            this.MAN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.MAN.FillWeight = 6.944829F;
            this.MAN.HeaderText = "MANTENIMIENTO";
            this.MAN.Name = "MAN";
            this.MAN.ReadOnly = true;
            this.MAN.Width = 202;
            // 
            // PERSONAL
            // 
            this.PERSONAL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.PERSONAL.FillWeight = 6.944829F;
            this.PERSONAL.HeaderText = "PERSONA ENTREGA";
            this.PERSONAL.Name = "PERSONAL";
            this.PERSONAL.ReadOnly = true;
            this.PERSONAL.Width = 198;
            // 
            // motivo
            // 
            this.motivo.HeaderText = "MOTIVO";
            this.motivo.Name = "motivo";
            this.motivo.ReadOnly = true;
            // 
            // MaterialProduccion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(800, 712);
            this.Controls.Add(this.dgImprimir);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MaterialProduccion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MaterialProduccion";
            this.Load += new System.EventHandler(this.MaterialP_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxObs.ResumeLayout(false);
            this.groupBoxObs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgImprimir)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtcodigo;
        private System.Windows.Forms.Label lblMedida;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.Label lblNomRef;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.ComboBox cmbMecanico;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNomMecanico;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtFecha;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblNomUsuario;
        private System.Windows.Forms.TextBox txtDispenso;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblusuario;
        private System.Windows.Forms.GroupBox groupBoxObs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMotivo;
        public System.Windows.Forms.Label labelidFinal;
        private System.Windows.Forms.Label lblCancelar;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label LblGuardar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.DataGridView dgImprimir;
        private System.Windows.Forms.DataGridViewTextBoxColumn COD;
        private System.Windows.Forms.DataGridViewTextBoxColumn RECACCION;
        private System.Windows.Forms.DataGridViewTextBoxColumn CAN;
        private System.Windows.Forms.DataGridViewTextBoxColumn UM;
        private System.Windows.Forms.DataGridViewTextBoxColumn FEC;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAN;
        private System.Windows.Forms.DataGridViewTextBoxColumn PERSONAL;
        private System.Windows.Forms.DataGridViewTextBoxColumn motivo;
    }
}