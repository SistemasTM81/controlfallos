namespace controlFallos
{
    partial class NotificacionSupervision
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificacionSupervision));
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbrefacciones = new System.Windows.Forms.RadioButton();
            this.rbreportes = new System.Windows.Forms.RadioButton();
            this.gbreportes = new System.Windows.Forms.GroupBox();
            this.tbnotif = new System.Windows.Forms.DataGridView();
            this.gbBuscar = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.pActualizar = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbmes = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbreportes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbnotif)).BeginInit();
            this.gbBuscar.SuspendLayout();
            this.pActualizar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Crimson;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.lbltitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1430, 29);
            this.panel2.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
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
            this.button1.Location = new System.Drawing.Point(1398, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 25);
            this.button1.TabIndex = 2;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(596, 3);
            this.lbltitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(136, 24);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = " Notificaciones";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ver:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbrefacciones);
            this.panel3.Controls.Add(this.rbreportes);
            this.panel3.Location = new System.Drawing.Point(167, 58);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(638, 40);
            this.panel3.TabIndex = 6;
            // 
            // rbrefacciones
            // 
            this.rbrefacciones.AutoSize = true;
            this.rbrefacciones.Location = new System.Drawing.Point(420, 9);
            this.rbrefacciones.Name = "rbrefacciones";
            this.rbrefacciones.Size = new System.Drawing.Size(178, 25);
            this.rbrefacciones.TabIndex = 3;
            this.rbrefacciones.Text = "Alertas de Vigencias";
            this.rbrefacciones.UseVisualStyleBackColor = true;
            this.rbrefacciones.CheckedChanged += new System.EventHandler(this.rbrefacciones_CheckedChanged);
            // 
            // rbreportes
            // 
            this.rbreportes.AutoSize = true;
            this.rbreportes.Checked = true;
            this.rbreportes.Location = new System.Drawing.Point(25, 7);
            this.rbreportes.Name = "rbreportes";
            this.rbreportes.Size = new System.Drawing.Size(174, 25);
            this.rbreportes.TabIndex = 0;
            this.rbreportes.TabStop = true;
            this.rbreportes.Text = "Alertas de Reportes";
            this.rbreportes.UseVisualStyleBackColor = true;
            this.rbreportes.CheckedChanged += new System.EventHandler(this.rbreportes_CheckedChanged);
            // 
            // gbreportes
            // 
            this.gbreportes.Controls.Add(this.tbnotif);
            this.gbreportes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbreportes.Location = new System.Drawing.Point(3, 161);
            this.gbreportes.Name = "gbreportes";
            this.gbreportes.Size = new System.Drawing.Size(1425, 351);
            this.gbreportes.TabIndex = 8;
            this.gbreportes.TabStop = false;
            this.gbreportes.Text = "Alertas de Reportes";
            // 
            // tbnotif
            // 
            this.tbnotif.AllowUserToAddRows = false;
            this.tbnotif.AllowUserToDeleteRows = false;
            this.tbnotif.AllowUserToResizeColumns = false;
            this.tbnotif.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbnotif.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tbnotif.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tbnotif.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tbnotif.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.tbnotif.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbnotif.CausesValidation = false;
            this.tbnotif.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 14.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbnotif.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tbnotif.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbnotif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tbnotif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbnotif.EnableHeadersVisualStyles = false;
            this.tbnotif.GridColor = System.Drawing.Color.White;
            this.tbnotif.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tbnotif.Location = new System.Drawing.Point(3, 25);
            this.tbnotif.MultiSelect = false;
            this.tbnotif.Name = "tbnotif";
            this.tbnotif.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 14.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbnotif.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.tbnotif.RowHeadersVisible = false;
            this.tbnotif.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbnotif.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.tbnotif.RowTemplate.ReadOnly = true;
            this.tbnotif.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbnotif.ShowCellErrors = false;
            this.tbnotif.ShowCellToolTips = false;
            this.tbnotif.ShowEditingIcon = false;
            this.tbnotif.ShowRowErrors = false;
            this.tbnotif.Size = new System.Drawing.Size(1419, 323);
            this.tbnotif.TabIndex = 2;
            // 
            // gbBuscar
            // 
            this.gbBuscar.Controls.Add(this.button7);
            this.gbBuscar.Controls.Add(this.pActualizar);
            this.gbBuscar.Controls.Add(this.label3);
            this.gbBuscar.Controls.Add(this.label2);
            this.gbBuscar.Controls.Add(this.cbmes);
            this.gbBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbBuscar.Location = new System.Drawing.Point(835, 29);
            this.gbBuscar.Name = "gbBuscar";
            this.gbBuscar.Size = new System.Drawing.Size(595, 143);
            this.gbBuscar.TabIndex = 0;
            this.gbBuscar.TabStop = false;
            this.gbBuscar.Text = "Buscar Por:";
            this.gbBuscar.Visible = false;
            // 
            // button7
            // 
            this.button7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button7.BackgroundImage")));
            this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(349, 18);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(52, 50);
            this.button7.TabIndex = 23;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // pActualizar
            // 
            this.pActualizar.Controls.Add(this.button4);
            this.pActualizar.Controls.Add(this.label16);
            this.pActualizar.Location = new System.Drawing.Point(421, 17);
            this.pActualizar.Name = "pActualizar";
            this.pActualizar.Size = new System.Drawing.Size(162, 108);
            this.pActualizar.TabIndex = 21;
            this.pActualizar.Visible = false;
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::controlFallos.Properties.Resources._1491313940_repeat_82991;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(58, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(47, 47);
            this.button4.TabIndex = 0;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 51);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(146, 42);
            this.label16.TabIndex = 0;
            this.label16.Text = "Mostrar Próximos\r\na Expirar";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(355, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 21);
            this.label3.TabIndex = 22;
            this.label3.Text = "Buscar";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mes:";
            // 
            // cbmes
            // 
            this.cbmes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbmes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbmes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbmes.DropDownWidth = 250;
            this.cbmes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbmes.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbmes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cbmes.FormattingEnabled = true;
            this.cbmes.Location = new System.Drawing.Point(83, 38);
            this.cbmes.Name = "cbmes";
            this.cbmes.Size = new System.Drawing.Size(250, 23);
            this.cbmes.TabIndex = 7;
            this.cbmes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbmes_DrawItem);
            // 
            // NotificacionSupervision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1430, 518);
            this.Controls.Add(this.gbBuscar);
            this.Controls.Add(this.gbreportes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Garamond", 14.25F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "NotificacionSupervision";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NotificacionSupervision";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NotificacionSupervision_FormClosing);
            this.Load += new System.EventHandler(this.NotificacionSupervision_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.gbreportes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbnotif)).EndInit();
            this.gbBuscar.ResumeLayout(false);
            this.gbBuscar.PerformLayout();
            this.pActualizar.ResumeLayout(false);
            this.pActualizar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbrefacciones;
        private System.Windows.Forms.RadioButton rbreportes;
        private System.Windows.Forms.GroupBox gbreportes;
        private System.Windows.Forms.DataGridView tbnotif;
        private System.Windows.Forms.GroupBox gbBuscar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbmes;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel pActualizar;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label3;
    }
}