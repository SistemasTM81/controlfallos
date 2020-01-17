namespace controlFallos
{
    partial class searchworkday
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(searchworkday));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cmbxmonthBusq = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gbxFechas = new System.Windows.Forms.GroupBox();
            this.dtpFechaDe = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaA = new System.Windows.Forms.DateTimePicker();
            this.label54 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.pActualizar = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label56 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.cmbxEcoBusq = new System.Windows.Forms.ComboBox();
            this.label59 = new System.Windows.Forms.Label();
            this.cmbxTimeperiodBusq = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvroles = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbxFechas.SuspendLayout();
            this.pActualizar.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvroles)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(318, 104);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 105;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cmbxmonthBusq
            // 
            this.cmbxmonthBusq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbxmonthBusq.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbxmonthBusq.DropDownHeight = 180;
            this.cmbxmonthBusq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxmonthBusq.DropDownWidth = 224;
            this.cmbxmonthBusq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbxmonthBusq.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxmonthBusq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbxmonthBusq.FormattingEnabled = true;
            this.cmbxmonthBusq.IntegralHeight = false;
            this.cmbxmonthBusq.Location = new System.Drawing.Point(739, 61);
            this.cmbxmonthBusq.Name = "cmbxmonthBusq";
            this.cmbxmonthBusq.Size = new System.Drawing.Size(225, 23);
            this.cmbxmonthBusq.TabIndex = 115;
            this.cmbxmonthBusq.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbxTimeperiodBusq_DrawItem);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(697, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 18);
            this.label8.TabIndex = 114;
            this.label8.Text = "Mes:";
            // 
            // gbxFechas
            // 
            this.gbxFechas.Controls.Add(this.dtpFechaDe);
            this.gbxFechas.Controls.Add(this.dtpFechaA);
            this.gbxFechas.Controls.Add(this.label54);
            this.gbxFechas.Controls.Add(this.label55);
            this.gbxFechas.Enabled = false;
            this.gbxFechas.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbxFechas.Location = new System.Drawing.Point(146, 100);
            this.gbxFechas.Name = "gbxFechas";
            this.gbxFechas.Size = new System.Drawing.Size(448, 73);
            this.gbxFechas.TabIndex = 108;
            this.gbxFechas.TabStop = false;
            this.gbxFechas.Text = "Rango de Fechas:   ";
            // 
            // dtpFechaDe
            // 
            this.dtpFechaDe.CalendarFont = new System.Drawing.Font("Garamond", 12F);
            this.dtpFechaDe.CustomFormat = "dd/MMMM/yyyy";
            this.dtpFechaDe.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaDe.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaDe.Location = new System.Drawing.Point(39, 31);
            this.dtpFechaDe.Name = "dtpFechaDe";
            this.dtpFechaDe.Size = new System.Drawing.Size(194, 25);
            this.dtpFechaDe.TabIndex = 34;
            // 
            // dtpFechaA
            // 
            this.dtpFechaA.CalendarFont = new System.Drawing.Font("Garamond", 12F);
            this.dtpFechaA.CustomFormat = "dd/MMMM/yyyy";
            this.dtpFechaA.Font = new System.Drawing.Font("Garamond", 12F);
            this.dtpFechaA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaA.Location = new System.Drawing.Point(261, 30);
            this.dtpFechaA.Name = "dtpFechaA";
            this.dtpFechaA.Size = new System.Drawing.Size(184, 25);
            this.dtpFechaA.TabIndex = 35;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BackColor = System.Drawing.Color.Transparent;
            this.label54.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label54.Location = new System.Drawing.Point(6, 35);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(33, 18);
            this.label54.TabIndex = 33;
            this.label54.Text = "De:";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.BackColor = System.Drawing.Color.Transparent;
            this.label55.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label55.Location = new System.Drawing.Point(237, 33);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(23, 18);
            this.label55.TabIndex = 32;
            this.label55.Text = "A:";
            // 
            // pActualizar
            // 
            this.pActualizar.Controls.Add(this.button1);
            this.pActualizar.Controls.Add(this.label56);
            this.pActualizar.Location = new System.Drawing.Point(731, 110);
            this.pActualizar.Name = "pActualizar";
            this.pActualizar.Size = new System.Drawing.Size(61, 65);
            this.pActualizar.TabIndex = 112;
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::controlFallos.Properties.Resources.refresh;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 42);
            this.button1.TabIndex = 55;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label56
            // 
            this.label56.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label56.Location = new System.Drawing.Point(0, 44);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(61, 21);
            this.label56.TabIndex = 56;
            this.label56.Text = "Mostrar";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(639, 153);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(51, 18);
            this.label57.TabIndex = 111;
            this.label57.Text = "Buscar";
            // 
            // cmbxEcoBusq
            // 
            this.cmbxEcoBusq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbxEcoBusq.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbxEcoBusq.DropDownHeight = 180;
            this.cmbxEcoBusq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxEcoBusq.DropDownWidth = 250;
            this.cmbxEcoBusq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbxEcoBusq.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxEcoBusq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbxEcoBusq.FormattingEnabled = true;
            this.cmbxEcoBusq.IntegralHeight = false;
            this.cmbxEcoBusq.Location = new System.Drawing.Point(462, 60);
            this.cmbxEcoBusq.Name = "cmbxEcoBusq";
            this.cmbxEcoBusq.Size = new System.Drawing.Size(225, 23);
            this.cmbxEcoBusq.TabIndex = 107;
            this.cmbxEcoBusq.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbxTimeperiodBusq_DrawItem);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(392, 61);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(63, 18);
            this.label59.TabIndex = 106;
            this.label59.Text = "Servicio:";
            // 
            // cmbxTimeperiodBusq
            // 
            this.cmbxTimeperiodBusq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbxTimeperiodBusq.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbxTimeperiodBusq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxTimeperiodBusq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbxTimeperiodBusq.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbxTimeperiodBusq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.cmbxTimeperiodBusq.FormattingEnabled = true;
            this.cmbxTimeperiodBusq.Location = new System.Drawing.Point(149, 60);
            this.cmbxTimeperiodBusq.Name = "cmbxTimeperiodBusq";
            this.cmbxTimeperiodBusq.Size = new System.Drawing.Size(225, 23);
            this.cmbxTimeperiodBusq.TabIndex = 103;
            this.cmbxTimeperiodBusq.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbxTimeperiodBusq_DrawItem);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 18);
            this.label7.TabIndex = 104;
            this.label7.Text = "Periodo de Tiempo:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvroles);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Garamond", 12F);
            this.panel1.Location = new System.Drawing.Point(0, 214);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 275);
            this.panel1.TabIndex = 116;
            // 
            // dgvroles
            // 
            this.dgvroles.AllowUserToAddRows = false;
            this.dgvroles.AllowUserToDeleteRows = false;
            this.dgvroles.AllowUserToResizeColumns = false;
            this.dgvroles.AllowUserToResizeRows = false;
            this.dgvroles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvroles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvroles.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvroles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvroles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenHorizontal;
            this.dgvroles.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvroles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvroles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvroles.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvroles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvroles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvroles.EnableHeadersVisualStyles = false;
            this.dgvroles.GridColor = System.Drawing.Color.Crimson;
            this.dgvroles.Location = new System.Drawing.Point(0, 0);
            this.dgvroles.MultiSelect = false;
            this.dgvroles.Name = "dgvroles";
            this.dgvroles.ReadOnly = true;
            this.dgvroles.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvroles.RowHeadersVisible = false;
            this.dgvroles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvroles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvroles.Size = new System.Drawing.Size(974, 275);
            this.dgvroles.TabIndex = 1;
            this.dgvroles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvroles_CellDoubleClick);
            this.dgvroles.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvroles_ColumnAdded);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Crimson;
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.lbltitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(974, 27);
            this.panel2.TabIndex = 117;
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::controlFallos.Properties.Resources.delete;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Dock = System.Windows.Forms.DockStyle.Right;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(942, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(32, 27);
            this.button3.TabIndex = 3;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // lbltitle
            // 
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(0, 0);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(974, 27);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Búsqueda de Roles";
            this.lbltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(642, 110);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(42, 42);
            this.button2.TabIndex = 110;
            this.button2.TabStop = false;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(528, 186);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(432, 17);
            this.label23.TabIndex = 118;
            this.label23.Text = " Para Visualizar la Información de Doble Clic sobre el registro de la Tabla";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Crimson;
            this.label3.Location = new System.Drawing.Point(485, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 18);
            this.label3.TabIndex = 119;
            this.label3.Text = "Nota:";
            // 
            // searchworkday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(974, 489);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cmbxmonthBusq);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gbxFechas);
            this.Controls.Add(this.pActualizar);
            this.Controls.Add(this.label57);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cmbxEcoBusq);
            this.Controls.Add(this.label59);
            this.Controls.Add(this.cmbxTimeperiodBusq);
            this.Controls.Add(this.label7);
            this.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(800, 100);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "searchworkday";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "searchworkday";
            this.gbxFechas.ResumeLayout(false);
            this.gbxFechas.PerformLayout();
            this.pActualizar.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvroles)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        #region Events FRom the Windows  Form Controls
        private void cmbxTimeperiodBusq_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) => Owner.Owner.v.combos_DrawItem(sender, e);
        private void lbltitle_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) { if (e.Button == System.Windows.Forms.MouseButtons.Left) Owner.Owner.v.mover(sender, e, this); }

        private void dgvroles_ColumnAdded(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e) => Owner.Owner.v.paraDataGridViews_ColumnAdded(sender,e);
        private void dgvroles_CellDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.ComboBox cmbxmonthBusq;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gbxFechas;
        private System.Windows.Forms.DateTimePicker dtpFechaDe;
        private System.Windows.Forms.DateTimePicker dtpFechaA;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Panel pActualizar;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.ComboBox cmbxEcoBusq;
        private System.Windows.Forms.Label label59;
        public System.Windows.Forms.ComboBox cmbxTimeperiodBusq;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.DataGridView dgvroles;
    }
}