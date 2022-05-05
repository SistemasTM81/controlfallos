namespace controlFallos
{
    partial class LectorHuellas
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
            this.ptbxImagen1 = new System.Windows.Forms.PictureBox();
            this.pgbrHuella = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.gbxReporte = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblPuesto = new System.Windows.Forms.Label();
            this.lblNombres = new System.Windows.Forms.Label();
            this.lblAMaterno = new System.Windows.Forms.Label();
            this.lblAPaterno = new System.Windows.Forms.Label();
            this.lblCredencial = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ptbxImagen1)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbxReporte.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ptbxImagen1
            // 
            this.ptbxImagen1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ptbxImagen1.Location = new System.Drawing.Point(12, 133);
            this.ptbxImagen1.Name = "ptbxImagen1";
            this.ptbxImagen1.Size = new System.Drawing.Size(185, 172);
            this.ptbxImagen1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbxImagen1.TabIndex = 0;
            this.ptbxImagen1.TabStop = false;
            // 
            // pgbrHuella
            // 
            this.pgbrHuella.Location = new System.Drawing.Point(12, 338);
            this.pgbrHuella.Name = "pgbrHuella";
            this.pgbrHuella.Size = new System.Drawing.Size(185, 23);
            this.pgbrHuella.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.btnCerrar);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 29);
            this.panel1.TabIndex = 32;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCerrar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(525, 0);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(47, 29);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(189, 3);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(160, 24);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Text = "Huellas Dactilares";
            // 
            // gbxReporte
            // 
            this.gbxReporte.Controls.Add(this.pictureBox1);
            this.gbxReporte.Controls.Add(this.lblPuesto);
            this.gbxReporte.Controls.Add(this.lblNombres);
            this.gbxReporte.Controls.Add(this.lblAMaterno);
            this.gbxReporte.Controls.Add(this.lblAPaterno);
            this.gbxReporte.Controls.Add(this.lblCredencial);
            this.gbxReporte.Controls.Add(this.label6);
            this.gbxReporte.Controls.Add(this.label5);
            this.gbxReporte.Controls.Add(this.label4);
            this.gbxReporte.Controls.Add(this.label3);
            this.gbxReporte.Controls.Add(this.label2);
            this.gbxReporte.Controls.Add(this.label11);
            this.gbxReporte.Controls.Add(this.ptbxImagen1);
            this.gbxReporte.Controls.Add(this.pgbrHuella);
            this.gbxReporte.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxReporte.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbxReporte.Location = new System.Drawing.Point(7, 31);
            this.gbxReporte.Name = "gbxReporte";
            this.gbxReporte.Size = new System.Drawing.Size(559, 379);
            this.gbxReporte.TabIndex = 35;
            this.gbxReporte.TabStop = false;
            this.gbxReporte.Text = "REPORTE ";
            this.gbxReporte.Paint += new System.Windows.Forms.PaintEventHandler(this.gbxReporte_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::controlFallos.Properties.Resources.transmasivo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(185, 81);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 137;
            this.pictureBox1.TabStop = false;
            // 
            // lblPuesto
            // 
            this.lblPuesto.AutoSize = true;
            this.lblPuesto.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblPuesto.Location = new System.Drawing.Point(327, 344);
            this.lblPuesto.Name = "lblPuesto";
            this.lblPuesto.Size = new System.Drawing.Size(0, 18);
            this.lblPuesto.TabIndex = 87;
            this.lblPuesto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNombres
            // 
            this.lblNombres.AutoSize = true;
            this.lblNombres.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombres.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblNombres.Location = new System.Drawing.Point(327, 270);
            this.lblNombres.Name = "lblNombres";
            this.lblNombres.Size = new System.Drawing.Size(0, 18);
            this.lblNombres.TabIndex = 86;
            this.lblNombres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAMaterno
            // 
            this.lblAMaterno.AutoSize = true;
            this.lblAMaterno.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAMaterno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblAMaterno.Location = new System.Drawing.Point(327, 188);
            this.lblAMaterno.Name = "lblAMaterno";
            this.lblAMaterno.Size = new System.Drawing.Size(0, 18);
            this.lblAMaterno.TabIndex = 85;
            this.lblAMaterno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAPaterno
            // 
            this.lblAPaterno.AutoSize = true;
            this.lblAPaterno.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAPaterno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblAPaterno.Location = new System.Drawing.Point(327, 106);
            this.lblAPaterno.Name = "lblAPaterno";
            this.lblAPaterno.Size = new System.Drawing.Size(0, 18);
            this.lblAPaterno.TabIndex = 84;
            this.lblAPaterno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCredencial
            // 
            this.lblCredencial.AutoSize = true;
            this.lblCredencial.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredencial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lblCredencial.Location = new System.Drawing.Point(327, 34);
            this.lblCredencial.Name = "lblCredencial";
            this.lblCredencial.Size = new System.Drawing.Size(0, 18);
            this.lblCredencial.TabIndex = 83;
            this.lblCredencial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label6.Location = new System.Drawing.Point(206, 344);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 18);
            this.label6.TabIndex = 82;
            this.label6.Text = "PUESTO:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label5.Location = new System.Drawing.Point(206, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 18);
            this.label5.TabIndex = 81;
            this.label5.Text = "NOMBRE(S):";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label4.Location = new System.Drawing.Point(206, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 18);
            this.label4.TabIndex = 80;
            this.label4.Text = "A. MATERNO:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(206, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 18);
            this.label3.TabIndex = 79;
            this.label3.Text = "A. PATERNO:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.Location = new System.Drawing.Point(206, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 18);
            this.label2.TabIndex = 78;
            this.label2.Text = "CREDENCIAL:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label11.Location = new System.Drawing.Point(13, 320);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(183, 18);
            this.label11.TabIndex = 76;
            this.label11.Text = "CALIDAD DE HUELLA";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LectorHuellas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(572, 417);
            this.Controls.Add(this.gbxReporte);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LectorHuellas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmHuellas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LectorHuellas_FormClosing);
            this.Load += new System.EventHandler(this.LectorHuellas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ptbxImagen1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbxReporte.ResumeLayout(false);
            this.gbxReporte.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ptbxImagen1;
        private System.Windows.Forms.ProgressBar pgbrHuella;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCerrar;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.GroupBox gbxReporte;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblCredencial;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPuesto;
        private System.Windows.Forms.Label lblNombres;
        private System.Windows.Forms.Label lblAMaterno;
        private System.Windows.Forms.Label lblAPaterno;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}