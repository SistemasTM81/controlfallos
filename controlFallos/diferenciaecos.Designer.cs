namespace controlFallos
{
    partial class diferenciaecos
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
            this.btnCerrar = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.lbdiferencia = new System.Windows.Forms.ListBox();
            this.txtdiferencia = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbltexto = new System.Windows.Forms.Label();
            this.padd = new System.Windows.Forms.Panel();
            this.btnadd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pdatos = new System.Windows.Forms.Panel();
            this.paceptar = new System.Windows.Forms.Panel();
            this.btnaceptar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.padd.SuspendLayout();
            this.pdatos.SuspendLayout();
            this.paceptar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.btnCerrar);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 24);
            this.panel1.TabIndex = 32;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackgroundImage = global::controlFallos.Properties.Resources.delete;
            this.btnCerrar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCerrar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(481, 0);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(38, 24);
            this.btnCerrar.TabIndex = 0;
            this.btnCerrar.UseVisualStyleBackColor = true;
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(55, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(311, 24);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Text = "Diferencia de tiempo entre unidades";
            // 
            // lbdiferencia
            // 
            this.lbdiferencia.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbdiferencia.FormattingEnabled = true;
            this.lbdiferencia.ItemHeight = 14;
            this.lbdiferencia.Location = new System.Drawing.Point(12, 101);
            this.lbdiferencia.Name = "lbdiferencia";
            this.lbdiferencia.Size = new System.Drawing.Size(413, 144);
            this.lbdiferencia.TabIndex = 33;
            this.lbdiferencia.SelectedIndexChanged += new System.EventHandler(this.lbdiferencia_SelectedIndexChanged);
            // 
            // txtdiferencia
            // 
            this.txtdiferencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtdiferencia.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtdiferencia.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtdiferencia.Font = new System.Drawing.Font("Garamond", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdiferencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtdiferencia.Location = new System.Drawing.Point(217, 3);
            this.txtdiferencia.MaxLength = 2;
            this.txtdiferencia.Name = "txtdiferencia";
            this.txtdiferencia.ShortcutsEnabled = false;
            this.txtdiferencia.Size = new System.Drawing.Size(109, 20);
            this.txtdiferencia.TabIndex = 71;
            this.txtdiferencia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtdiferencia_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(215, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 9);
            this.label4.TabIndex = 73;
            this.label4.Text = "____________________________";
            // 
            // lbltexto
            // 
            this.lbltexto.AutoSize = true;
            this.lbltexto.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltexto.Location = new System.Drawing.Point(3, 5);
            this.lbltexto.Name = "lbltexto";
            this.lbltexto.Size = new System.Drawing.Size(196, 18);
            this.lbltexto.TabIndex = 72;
            this.lbltexto.Text = "Diferencia entre unidad 1 y 2:";
            // 
            // padd
            // 
            this.padd.Controls.Add(this.btnadd);
            this.padd.Location = new System.Drawing.Point(436, 45);
            this.padd.Name = "padd";
            this.padd.Size = new System.Drawing.Size(38, 39);
            this.padd.TabIndex = 89;
            // 
            // btnadd
            // 
            this.btnadd.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btnadd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnadd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnadd.FlatAppearance.BorderSize = 0;
            this.btnadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnadd.Location = new System.Drawing.Point(6, 7);
            this.btnadd.Name = "btnadd";
            this.btnadd.Size = new System.Drawing.Size(25, 25);
            this.btnadd.TabIndex = 1;
            this.btnadd.UseVisualStyleBackColor = true;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(338, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 18);
            this.label1.TabIndex = 90;
            this.label1.Text = "minutos.";
            // 
            // pdatos
            // 
            this.pdatos.Controls.Add(this.lbltexto);
            this.pdatos.Controls.Add(this.label1);
            this.pdatos.Controls.Add(this.label4);
            this.pdatos.Controls.Add(this.txtdiferencia);
            this.pdatos.Location = new System.Drawing.Point(12, 52);
            this.pdatos.Name = "pdatos";
            this.pdatos.Size = new System.Drawing.Size(413, 32);
            this.pdatos.TabIndex = 91;
            // 
            // paceptar
            // 
            this.paceptar.Controls.Add(this.label2);
            this.paceptar.Controls.Add(this.btnaceptar);
            this.paceptar.Location = new System.Drawing.Point(433, 219);
            this.paceptar.Name = "paceptar";
            this.paceptar.Size = new System.Drawing.Size(76, 65);
            this.paceptar.TabIndex = 90;
            this.paceptar.Visible = false;
            // 
            // btnaceptar
            // 
            this.btnaceptar.BackgroundImage = global::controlFallos.Properties.Resources.check;
            this.btnaceptar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnaceptar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnaceptar.FlatAppearance.BorderSize = 0;
            this.btnaceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnaceptar.Location = new System.Drawing.Point(18, 7);
            this.btnaceptar.Name = "btnaceptar";
            this.btnaceptar.Size = new System.Drawing.Size(32, 29);
            this.btnaceptar.TabIndex = 1;
            this.btnaceptar.UseVisualStyleBackColor = true;
            this.btnaceptar.Click += new System.EventHandler(this.btnaceptar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 18);
            this.label2.TabIndex = 91;
            this.label2.Text = "Aceptar";
            // 
            // diferenciaecos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(519, 290);
            this.Controls.Add(this.paceptar);
            this.Controls.Add(this.pdatos);
            this.Controls.Add(this.padd);
            this.Controls.Add(this.lbdiferencia);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "diferenciaecos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "diferenciaecos";
            this.Load += new System.EventHandler(this.diferenciaecos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.padd.ResumeLayout(false);
            this.pdatos.ResumeLayout(false);
            this.pdatos.PerformLayout();
            this.paceptar.ResumeLayout(false);
            this.paceptar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCerrar;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.ListBox lbdiferencia;
        private System.Windows.Forms.TextBox txtdiferencia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbltexto;
        private System.Windows.Forms.Panel padd;
        private System.Windows.Forms.Button btnadd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pdatos;
        private System.Windows.Forms.Panel paceptar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnaceptar;
    }
}