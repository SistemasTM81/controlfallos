namespace controlFallos
{
    partial class catIVA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(catIVA));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxEdicion = new System.Windows.Forms.GroupBox();
            this.buttonEditar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxUsuario = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.labelidMecanicoApo = new System.Windows.Forms.Label();
            this.txtMoneda = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.cmbMoneda = new System.Windows.Forms.ComboBox();
            this.textBoxIVA = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxEdicion.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(429, 27);
            this.panel1.TabIndex = 32;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
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
            this.button1.Location = new System.Drawing.Point(399, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 27);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Crimson;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(89, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Actualización De  Moneda\r\n";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBoxEdicion);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 255);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            // 
            // groupBoxEdicion
            // 
            this.groupBoxEdicion.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxEdicion.Controls.Add(this.textBoxIVA);
            this.groupBoxEdicion.Controls.Add(this.label4);
            this.groupBoxEdicion.Controls.Add(this.label5);
            this.groupBoxEdicion.Controls.Add(this.label6);
            this.groupBoxEdicion.Controls.Add(this.cmbMoneda);
            this.groupBoxEdicion.Controls.Add(this.buttonEditar);
            this.groupBoxEdicion.Controls.Add(this.label3);
            this.groupBoxEdicion.Controls.Add(this.label2);
            this.groupBoxEdicion.Controls.Add(this.textBoxUsuario);
            this.groupBoxEdicion.Controls.Add(this.label25);
            this.groupBoxEdicion.Controls.Add(this.labelidMecanicoApo);
            this.groupBoxEdicion.Controls.Add(this.txtMoneda);
            this.groupBoxEdicion.Controls.Add(this.label19);
            this.groupBoxEdicion.Controls.Add(this.label46);
            this.groupBoxEdicion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxEdicion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxEdicion.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEdicion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.groupBoxEdicion.Location = new System.Drawing.Point(3, -20);
            this.groupBoxEdicion.Name = "groupBoxEdicion";
            this.groupBoxEdicion.Size = new System.Drawing.Size(423, 272);
            this.groupBoxEdicion.TabIndex = 228;
            this.groupBoxEdicion.TabStop = false;
            this.groupBoxEdicion.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBoxEdicion_Paint);
            this.groupBoxEdicion.Enter += new System.EventHandler(this.groupBoxEdicion_Enter);
            // 
            // buttonEditar
            // 
            this.buttonEditar.BackColor = System.Drawing.Color.Transparent;
            this.buttonEditar.BackgroundImage = global::controlFallos.Properties.Resources.document_edit_icon_icons_com_52428;
            this.buttonEditar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonEditar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonEditar.FlatAppearance.BorderSize = 0;
            this.buttonEditar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonEditar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.buttonEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditar.Location = new System.Drawing.Point(206, 174);
            this.buttonEditar.Name = "buttonEditar";
            this.buttonEditar.Size = new System.Drawing.Size(54, 51);
            this.buttonEditar.TabIndex = 233;
            this.buttonEditar.UseVisualStyleBackColor = false;
            this.buttonEditar.Click += new System.EventHandler(this.buttonEditar_Click);
            this.buttonEditar.MouseLeave += new System.EventHandler(this.buttonEditar_MouseLeave);
            this.buttonEditar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.buttonEditar_MouseMove);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(198, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 18);
            this.label3.TabIndex = 232;
            this.label3.Text = "EDITAR";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 36);
            this.label2.TabIndex = 231;
            this.label2.Text = "CONTRASEÑA \r\nDEL USUARIO:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxUsuario
            // 
            this.textBoxUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.textBoxUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxUsuario.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.textBoxUsuario.Location = new System.Drawing.Point(165, 151);
            this.textBoxUsuario.MaxLength = 18;
            this.textBoxUsuario.Name = "textBoxUsuario";
            this.textBoxUsuario.PasswordChar = '*';
            this.textBoxUsuario.ShortcutsEnabled = false;
            this.textBoxUsuario.Size = new System.Drawing.Size(210, 18);
            this.textBoxUsuario.TabIndex = 1;
            this.textBoxUsuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxUsuario.TextChanged += new System.EventHandler(this.getCambios);
            this.textBoxUsuario.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxUsuario_KeyPress);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label25.Location = new System.Drawing.Point(163, 162);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(213, 9);
            this.label25.TabIndex = 229;
            this.label25.Text = "____________________________________________________";
            // 
            // labelidMecanicoApo
            // 
            this.labelidMecanicoApo.AutoSize = true;
            this.labelidMecanicoApo.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelidMecanicoApo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.labelidMecanicoApo.Location = new System.Drawing.Point(54, 188);
            this.labelidMecanicoApo.Name = "labelidMecanicoApo";
            this.labelidMecanicoApo.Size = new System.Drawing.Size(0, 24);
            this.labelidMecanicoApo.TabIndex = 230;
            // 
            // txtMoneda
            // 
            this.txtMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtMoneda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMoneda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMoneda.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtMoneda.Location = new System.Drawing.Point(165, 43);
            this.txtMoneda.MaxLength = 5;
            this.txtMoneda.Name = "txtMoneda";
            this.txtMoneda.ShortcutsEnabled = false;
            this.txtMoneda.Size = new System.Drawing.Size(90, 18);
            this.txtMoneda.TabIndex = 2;
            this.txtMoneda.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMoneda.TextChanged += new System.EventHandler(this.getCambios);
            this.txtMoneda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxIVA_KeyPress);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(33, 43);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(112, 18);
            this.label19.TabIndex = 209;
            this.label19.Text = "Costo Moneda:";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(163, 54);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(97, 9);
            this.label46.TabIndex = 224;
            this.label46.Text = "_______________________";
            // 
            // cmbMoneda
            // 
            this.cmbMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbMoneda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbMoneda.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbMoneda.DropDownHeight = 100;
            this.cmbMoneda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMoneda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMoneda.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbMoneda.FormattingEnabled = true;
            this.cmbMoneda.IntegralHeight = false;
            this.cmbMoneda.ItemHeight = 20;
            this.cmbMoneda.Location = new System.Drawing.Point(264, 40);
            this.cmbMoneda.Name = "cmbMoneda";
            this.cmbMoneda.Size = new System.Drawing.Size(116, 26);
            this.cmbMoneda.TabIndex = 235;
            this.cmbMoneda.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbMoneda_DrawItem);
            this.cmbMoneda.SelectedIndexChanged += new System.EventHandler(this.cmbMoneda_SelectedIndexChanged);
            this.cmbMoneda.SelectedValueChanged += new System.EventHandler(this.cmbMoneda_SelectedValueChanged);
            // 
            // textBoxIVA
            // 
            this.textBoxIVA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.textBoxIVA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxIVA.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxIVA.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIVA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.textBoxIVA.Location = new System.Drawing.Point(165, 91);
            this.textBoxIVA.MaxLength = 5;
            this.textBoxIVA.Name = "textBoxIVA";
            this.textBoxIVA.ShortcutsEnabled = false;
            this.textBoxIVA.Size = new System.Drawing.Size(90, 18);
            this.textBoxIVA.TabIndex = 236;
            this.textBoxIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(33, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 18);
            this.label4.TabIndex = 237;
            this.label4.Text = "IVA:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(266, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 18);
            this.label5.TabIndex = 239;
            this.label5.Text = "%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(163, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 9);
            this.label6.TabIndex = 238;
            this.label6.Text = "_______________________";
            // 
            // catIVA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(429, 282);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "catIVA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "catIVA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.catIVA_FormClosing);
            this.Load += new System.EventHandler(this.catIVA_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBoxEdicion.ResumeLayout(false);
            this.groupBoxEdicion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxEdicion;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtMoneda;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUsuario;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label labelidMecanicoApo;
        private System.Windows.Forms.Button buttonEditar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMoneda;
        private System.Windows.Forms.TextBox textBoxIVA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}