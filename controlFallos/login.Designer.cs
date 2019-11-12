namespace controlFallos
{
    partial class login
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            this.status = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbllogin = new System.Windows.Forms.Label();
            this.lblintentos = new System.Windows.Forms.Label();
            this.lblsistemaBloqueado = new System.Windows.Forms.Label();
            this.plogin = new System.Windows.Forms.Panel();
            this.txtgetpass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtgetusu = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnlogin = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.status.SuspendLayout();
            this.plogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.Color.Crimson;
            this.status.Controls.Add(this.button1);
            this.status.Controls.Add(this.lbltitle);
            this.status.Controls.Add(this.panel2);
            this.status.Dock = System.Windows.Forms.DockStyle.Top;
            this.status.Location = new System.Drawing.Point(0, 0);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(637, 26);
            this.status.TabIndex = 0;
            this.status.MouseDown += new System.Windows.Forms.MouseEventHandler(this.status_MouseDown);
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
            this.button1.Location = new System.Drawing.Point(608, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 26);
            this.button1.TabIndex = 4;
            this.button1.TabStop = false;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(233, -1);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(254, 24);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Text = "Sistema de Reporte de Fallos ";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SteelBlue;
            this.panel2.Location = new System.Drawing.Point(8, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(204, 171);
            this.panel2.TabIndex = 1;
            // 
            // lbllogin
            // 
            this.lbllogin.AutoSize = true;
            this.lbllogin.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllogin.Location = new System.Drawing.Point(536, 226);
            this.lbllogin.Name = "lbllogin";
            this.lbllogin.Size = new System.Drawing.Size(93, 18);
            this.lbllogin.TabIndex = 52;
            this.lbllogin.Text = "Iniciar Sesión";
            // 
            // lblintentos
            // 
            this.lblintentos.AutoSize = true;
            this.lblintentos.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblintentos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblintentos.Location = new System.Drawing.Point(321, 210);
            this.lblintentos.Name = "lblintentos";
            this.lblintentos.Size = new System.Drawing.Size(0, 21);
            this.lblintentos.TabIndex = 53;
            this.lblintentos.Visible = false;
            // 
            // lblsistemaBloqueado
            // 
            this.lblsistemaBloqueado.AutoSize = true;
            this.lblsistemaBloqueado.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsistemaBloqueado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblsistemaBloqueado.Location = new System.Drawing.Point(389, 116);
            this.lblsistemaBloqueado.Name = "lblsistemaBloqueado";
            this.lblsistemaBloqueado.Size = new System.Drawing.Size(155, 21);
            this.lblsistemaBloqueado.TabIndex = 54;
            this.lblsistemaBloqueado.Text = "Sistema Bloqueado!";
            this.lblsistemaBloqueado.Visible = false;
            // 
            // plogin
            // 
            this.plogin.Controls.Add(this.txtgetpass);
            this.plogin.Controls.Add(this.label4);
            this.plogin.Controls.Add(this.txtgetusu);
            this.plogin.Controls.Add(this.label2);
            this.plogin.Controls.Add(this.label1);
            this.plogin.Controls.Add(this.label3);
            this.plogin.Location = new System.Drawing.Point(296, 55);
            this.plogin.Name = "plogin";
            this.plogin.Size = new System.Drawing.Size(330, 124);
            this.plogin.TabIndex = 0;
            // 
            // txtgetpass
            // 
            this.txtgetpass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetpass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetpass.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetpass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetpass.Location = new System.Drawing.Point(120, 81);
            this.txtgetpass.MaxLength = 20;
            this.txtgetpass.Name = "txtgetpass";
            this.txtgetpass.ShortcutsEnabled = false;
            this.txtgetpass.Size = new System.Drawing.Size(189, 22);
            this.txtgetpass.TabIndex = 2;
            this.txtgetpass.UseSystemPasswordChar = true;
            this.txtgetpass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label4.Location = new System.Drawing.Point(19, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "Contraseña:";
            // 
            // txtgetusu
            // 
            this.txtgetusu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetusu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetusu.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetusu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetusu.Location = new System.Drawing.Point(120, 20);
            this.txtgetusu.MaxLength = 20;
            this.txtgetusu.Name = "txtgetusu";
            this.txtgetusu.ShortcutsEnabled = false;
            this.txtgetusu.Size = new System.Drawing.Size(189, 22);
            this.txtgetusu.TabIndex = 1;
            this.txtgetusu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetusu_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.Location = new System.Drawing.Point(118, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "_______________________________";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label1.Location = new System.Drawing.Point(47, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuario:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(118, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "_______________________________";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // btnlogin
            // 
            this.btnlogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnlogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnlogin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnlogin.FlatAppearance.BorderSize = 0;
            this.btnlogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnlogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnlogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnlogin.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnlogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.btnlogin.Image = global::controlFallos.Properties.Resources.login;
            this.btnlogin.Location = new System.Drawing.Point(560, 185);
            this.btnlogin.Name = "btnlogin";
            this.btnlogin.Size = new System.Drawing.Size(42, 38);
            this.btnlogin.TabIndex = 3;
            this.btnlogin.UseVisualStyleBackColor = true;
            this.btnlogin.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::controlFallos.Properties.Resources.FRONTAL_SOMBRA__1_;
            this.pictureBox1.Location = new System.Drawing.Point(11, 66);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(265, 165);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(637, 257);
            this.ControlBox = false;
            this.Controls.Add(this.plogin);
            this.Controls.Add(this.lblsistemaBloqueado);
            this.Controls.Add(this.lblintentos);
            this.Controls.Add(this.lbllogin);
            this.Controls.Add(this.btnlogin);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.status);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Reporte de Fallos";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.login_FormClosing);
            this.Load += new System.EventHandler(this.login_Load);
            this.Resize += new System.EventHandler(this.login_Resize);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.plogin.ResumeLayout(false);
            this.plogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel status;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnlogin;
        private System.Windows.Forms.Label lbllogin;
        private System.Windows.Forms.Label lbltitle;
        public System.Windows.Forms.Label lblintentos;
        public System.Windows.Forms.Label lblsistemaBloqueado;
        private System.Windows.Forms.Panel plogin;
        private System.Windows.Forms.TextBox txtgetpass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtgetusu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}

