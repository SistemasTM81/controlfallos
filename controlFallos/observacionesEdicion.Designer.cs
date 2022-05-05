namespace controlFallos
{
    partial class observacionesEdicion
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.btndeleteuser = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblinfo = new System.Windows.Forms.Label();
            this.txtgetedicion = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbltitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btndeleteuser);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblinfo);
            this.panel1.Controls.Add(this.txtgetedicion);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 252);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::controlFallos.Properties.Resources.comprobar;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(410, 161);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 50);
            this.button2.TabIndex = 5;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btndeleteuser
            // 
            this.btndeleteuser.BackgroundImage = global::controlFallos.Properties.Resources.cross;
            this.btndeleteuser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndeleteuser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteuser.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btndeleteuser.FlatAppearance.BorderSize = 0;
            this.btndeleteuser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndeleteuser.Location = new System.Drawing.Point(59, 160);
            this.btndeleteuser.Name = "btndeleteuser";
            this.btndeleteuser.Size = new System.Drawing.Size(50, 50);
            this.btndeleteuser.TabIndex = 4;
            this.btndeleteuser.UseVisualStyleBackColor = true;
            this.btndeleteuser.Click += new System.EventHandler(this.btndeleteuser_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(402, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "Aceptar";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cancelar";
            // 
            // lblinfo
            // 
            this.lblinfo.AutoSize = true;
            this.lblinfo.Location = new System.Drawing.Point(61, 34);
            this.lblinfo.Name = "lblinfo";
            this.lblinfo.Size = new System.Drawing.Size(442, 21);
            this.lblinfo.TabIndex = 0;
            this.lblinfo.Text = "Ingrese El Motivo de La Actualización de La Información";
            // 
            // txtgetedicion
            // 
            this.txtgetedicion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetedicion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtgetedicion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtgetedicion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtgetedicion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetedicion.Location = new System.Drawing.Point(88, 71);
            this.txtgetedicion.MaxLength = 100;
            this.txtgetedicion.Multiline = true;
            this.txtgetedicion.Name = "txtgetedicion";
            this.txtgetedicion.ShortcutsEnabled = false;
            this.txtgetedicion.Size = new System.Drawing.Size(355, 84);
            this.txtgetedicion.TabIndex = 1;
            this.txtgetedicion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetedicion_KeyPress);
            this.txtgetedicion.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetedicion_Validating);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Crimson;
            this.panel2.Controls.Add(this.lbltitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(540, 27);
            this.panel2.TabIndex = 0;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(205, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(133, 24);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Text = "Observaciones";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // observacionesEdicion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(544, 252);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "observacionesEdicion";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "observacionesEdicion";
            this.Load += new System.EventHandler(this.observacionesEdicion_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btndeleteuser;
        public System.Windows.Forms.TextBox txtgetedicion;
        public System.Windows.Forms.Label lblinfo;
    }
}