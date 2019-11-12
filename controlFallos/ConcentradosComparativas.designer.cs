namespace controlFallos
{
    partial class ConcentradosComparativas
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
            this.cbrefaccion = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnmejor = new System.Windows.Forms.Button();
            this.btnquitar = new System.Windows.Forms.Button();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.lbproveedores = new System.Windows.Forms.ListBox();
            this.lbmejores = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbrefaccion
            // 
            this.cbrefaccion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbrefaccion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbrefaccion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbrefaccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbrefaccion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbrefaccion.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbrefaccion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbrefaccion.FormattingEnabled = true;
            this.cbrefaccion.Location = new System.Drawing.Point(187, 57);
            this.cbrefaccion.Name = "cbrefaccion";
            this.cbrefaccion.Size = new System.Drawing.Size(293, 26);
            this.cbrefaccion.TabIndex = 35;
            this.cbrefaccion.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbrefaccion_DrawItem);
            this.cbrefaccion.SelectedIndexChanged += new System.EventHandler(this.cbrefaccion_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 21);
            this.label1.TabIndex = 34;
            this.label1.Text = "Refacción:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("Garamond", 12F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label9.Location = new System.Drawing.Point(378, 397);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 18);
            this.label9.TabIndex = 172;
            this.label9.Text = "Quitar";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Garamond", 12F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.Location = new System.Drawing.Point(350, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 18);
            this.label2.TabIndex = 174;
            this.label2.Text = "Mejor Opción";
            // 
            // btnmejor
            // 
            this.btnmejor.BackColor = System.Drawing.Color.Transparent;
            this.btnmejor.BackgroundImage = global::controlFallos.Properties.Resources.chevron_sign_to_right;
            this.btnmejor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnmejor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnmejor.Enabled = false;
            this.btnmejor.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnmejor.FlatAppearance.BorderSize = 0;
            this.btnmejor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnmejor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnmejor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnmejor.Location = new System.Drawing.Point(384, 213);
            this.btnmejor.Name = "btnmejor";
            this.btnmejor.Size = new System.Drawing.Size(35, 35);
            this.btnmejor.TabIndex = 173;
            this.btnmejor.UseVisualStyleBackColor = false;
            this.btnmejor.Click += new System.EventHandler(this.btnmejor_Click);
            // 
            // btnquitar
            // 
            this.btnquitar.BackColor = System.Drawing.Color.Transparent;
            this.btnquitar.BackgroundImage = global::controlFallos.Properties.Resources.chevron_sign_left;
            this.btnquitar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnquitar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnquitar.Enabled = false;
            this.btnquitar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnquitar.FlatAppearance.BorderSize = 0;
            this.btnquitar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnquitar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnquitar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnquitar.Location = new System.Drawing.Point(384, 359);
            this.btnquitar.Name = "btnquitar";
            this.btnquitar.Size = new System.Drawing.Size(35, 35);
            this.btnquitar.TabIndex = 171;
            this.btnquitar.UseVisualStyleBackColor = false;
            this.btnquitar.Click += new System.EventHandler(this.btnquitar_Click);
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.Location = new System.Drawing.Point(682, 576);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(77, 21);
            this.lblsave.TabIndex = 177;
            this.lblsave.Text = "Exportar";
            // 
            // btnsave
            // 
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.pdf;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Location = new System.Drawing.Point(692, 524);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(50, 50);
            this.btnsave.TabIndex = 176;
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(804, 27);
            this.panel1.TabIndex = 178;
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(757, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(47, 27);
            this.button2.TabIndex = 3;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(183, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(245, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Generación de Concentrado";
            // 
            // lbproveedores
            // 
            this.lbproveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lbproveedores.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbproveedores.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbproveedores.FormattingEnabled = true;
            this.lbproveedores.ItemHeight = 25;
            this.lbproveedores.Location = new System.Drawing.Point(12, 147);
            this.lbproveedores.Name = "lbproveedores";
            this.lbproveedores.Size = new System.Drawing.Size(314, 354);
            this.lbproveedores.TabIndex = 179;
            this.lbproveedores.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbproveedores_DrawItem);
            this.lbproveedores.SelectedValueChanged += new System.EventHandler(this.lbproveedores_SelectedValueChanged);
            // 
            // lbmejores
            // 
            this.lbmejores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lbmejores.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbmejores.FormattingEnabled = true;
            this.lbmejores.ItemHeight = 18;
            this.lbmejores.Location = new System.Drawing.Point(481, 147);
            this.lbmejores.Name = "lbmejores";
            this.lbmejores.Size = new System.Drawing.Size(292, 346);
            this.lbmejores.TabIndex = 180;
            this.lbmejores.SelectedIndexChanged += new System.EventHandler(this.lbmejores_SelectedIndexChanged);
            this.lbmejores.DataSourceChanged += new System.EventHandler(this.lbmejores_DataSourceChanged);
            // 
            // ConcentradosComparativas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(804, 656);
            this.Controls.Add(this.lbmejores);
            this.Controls.Add(this.lbproveedores);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblsave);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnmejor);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnquitar);
            this.Controls.Add(this.cbrefaccion);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ConcentradosComparativas";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.ConcentradosComparativas_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ComboBox cbrefaccion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnquitar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnmejor;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.ListBox lbproveedores;
        private System.Windows.Forms.ListBox lbmejores;
    }
}