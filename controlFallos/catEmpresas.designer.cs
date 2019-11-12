namespace controlFallos
{
    partial class catEmpresas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(catEmpresas));
            this.gbcempresa = new System.Windows.Forms.GroupBox();
            this.busqempresa = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Usu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Estatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.empresaa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.areaa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbaddbussiness = new System.Windows.Forms.GroupBox();
            this.lnkrestablecer = new System.Windows.Forms.LinkLabel();
            this.pblogo = new System.Windows.Forms.PictureBox();
            this.lbllogo = new System.Windows.Forms.Label();
            this.pCancel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancelEmpresa = new System.Windows.Forms.Button();
            this.txtgetnempresa = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblsavemp = new System.Windows.Forms.Label();
            this.btnsavemp = new System.Windows.Forms.Button();
            this.pEliminarEmpresa = new System.Windows.Forms.Panel();
            this.lbldelete = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.gbcempresa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.busqempresa)).BeginInit();
            this.gbaddbussiness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pblogo)).BeginInit();
            this.pCancel.SuspendLayout();
            this.pEliminarEmpresa.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbcempresa
            // 
            this.gbcempresa.Controls.Add(this.busqempresa);
            resources.ApplyResources(this.gbcempresa, "gbcempresa");
            this.gbcempresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbcempresa.Name = "gbcempresa";
            this.gbcempresa.TabStop = false;
            this.gbcempresa.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddbussiness_Paint);
            // 
            // busqempresa
            // 
            this.busqempresa.AllowUserToAddRows = false;
            this.busqempresa.AllowUserToDeleteRows = false;
            this.busqempresa.AllowUserToResizeColumns = false;
            this.busqempresa.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.busqempresa.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.busqempresa.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.busqempresa.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.busqempresa.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.busqempresa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.busqempresa.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 15.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.busqempresa.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.busqempresa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.busqempresa.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.Usu,
            this.Estatus,
            this.empresaa,
            this.areaa});
            resources.ApplyResources(this.busqempresa, "busqempresa");
            this.busqempresa.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.busqempresa.EnableHeadersVisualStyles = false;
            this.busqempresa.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.busqempresa.MultiSelect = false;
            this.busqempresa.Name = "busqempresa";
            this.busqempresa.ReadOnly = true;
            this.busqempresa.RowHeadersVisible = false;
            this.busqempresa.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.busqempresa.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.busqempresa.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.busqempresa.ShowCellErrors = false;
            this.busqempresa.ShowCellToolTips = false;
            this.busqempresa.ShowEditingIcon = false;
            this.busqempresa.ShowRowErrors = false;
            this.busqempresa.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.busqempresa_CellContentDoubleClick);
            this.busqempresa.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.busqempresa_CellFormatting);
            this.busqempresa.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.busqempresa_ColumnAdded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Usu
            // 
            resources.ApplyResources(this.Usu, "Usu");
            this.Usu.Name = "Usu";
            this.Usu.ReadOnly = true;
            this.Usu.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Estatus
            // 
            resources.ApplyResources(this.Estatus, "Estatus");
            this.Estatus.Name = "Estatus";
            this.Estatus.ReadOnly = true;
            this.Estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // empresaa
            // 
            resources.ApplyResources(this.empresaa, "empresaa");
            this.empresaa.Name = "empresaa";
            this.empresaa.ReadOnly = true;
            // 
            // areaa
            // 
            resources.ApplyResources(this.areaa, "areaa");
            this.areaa.Name = "areaa";
            this.areaa.ReadOnly = true;
            // 
            // gbaddbussiness
            // 
            this.gbaddbussiness.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.gbaddbussiness.Controls.Add(this.lnkrestablecer);
            this.gbaddbussiness.Controls.Add(this.pblogo);
            this.gbaddbussiness.Controls.Add(this.lbllogo);
            this.gbaddbussiness.Controls.Add(this.pCancel);
            this.gbaddbussiness.Controls.Add(this.txtgetnempresa);
            this.gbaddbussiness.Controls.Add(this.label16);
            this.gbaddbussiness.Controls.Add(this.label19);
            this.gbaddbussiness.Controls.Add(this.lblsavemp);
            this.gbaddbussiness.Controls.Add(this.btnsavemp);
            resources.ApplyResources(this.gbaddbussiness, "gbaddbussiness");
            this.gbaddbussiness.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbaddbussiness.Name = "gbaddbussiness";
            this.gbaddbussiness.TabStop = false;
            this.gbaddbussiness.Paint += new System.Windows.Forms.PaintEventHandler(this.gbaddbussiness_Paint);
            // 
            // lnkrestablecer
            // 
            resources.ApplyResources(this.lnkrestablecer, "lnkrestablecer");
            this.lnkrestablecer.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.lnkrestablecer.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lnkrestablecer.Name = "lnkrestablecer";
            this.lnkrestablecer.TabStop = true;
            this.lnkrestablecer.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lnkrestablecer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkrestablecer_LinkClicked);
            // 
            // pblogo
            // 
            this.pblogo.BackgroundImage = global::controlFallos.Properties.Resources.image;
            resources.ApplyResources(this.pblogo, "pblogo");
            this.pblogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pblogo.Name = "pblogo";
            this.pblogo.TabStop = false;
            this.pblogo.BackgroundImageChanged += new System.EventHandler(this.txtgetnempresa_TextChanged);
            this.pblogo.Click += new System.EventHandler(this.pblogo_Click);
            // 
            // lbllogo
            // 
            resources.ApplyResources(this.lbllogo, "lbllogo");
            this.lbllogo.Name = "lbllogo";
            // 
            // pCancel
            // 
            this.pCancel.Controls.Add(this.label2);
            this.pCancel.Controls.Add(this.btnCancelEmpresa);
            resources.ApplyResources(this.pCancel, "pCancel");
            this.pCancel.Name = "pCancel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label2.Name = "label2";
            // 
            // btnCancelEmpresa
            // 
            this.btnCancelEmpresa.BackgroundImage = global::controlFallos.Properties.Resources.add;
            resources.ApplyResources(this.btnCancelEmpresa, "btnCancelEmpresa");
            this.btnCancelEmpresa.FlatAppearance.BorderSize = 0;
            this.btnCancelEmpresa.Name = "btnCancelEmpresa";
            this.btnCancelEmpresa.UseVisualStyleBackColor = true;
            this.btnCancelEmpresa.Click += new System.EventHandler(this.btnCancelEmpresa_Click);
            // 
            // txtgetnempresa
            // 
            this.txtgetnempresa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtgetnempresa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtgetnempresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtgetnempresa, "txtgetnempresa");
            this.txtgetnempresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtgetnempresa.Name = "txtgetnempresa";
            this.txtgetnempresa.ShortcutsEnabled = false;
            this.txtgetnempresa.TextChanged += new System.EventHandler(this.txtgetnempresa_TextChanged);
            this.txtgetnempresa.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgetnempresa_KeyPress);
            this.txtgetnempresa.Validating += new System.ComponentModel.CancelEventHandler(this.txtgetnempresa_Validating);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // lblsavemp
            // 
            resources.ApplyResources(this.lblsavemp, "lblsavemp");
            this.lblsavemp.Name = "lblsavemp";
            // 
            // btnsavemp
            // 
            this.btnsavemp.BackColor = System.Drawing.Color.Transparent;
            this.btnsavemp.BackgroundImage = global::controlFallos.Properties.Resources.save;
            resources.ApplyResources(this.btnsavemp, "btnsavemp");
            this.btnsavemp.FlatAppearance.BorderSize = 0;
            this.btnsavemp.Name = "btnsavemp";
            this.btnsavemp.UseVisualStyleBackColor = false;
            this.btnsavemp.Click += new System.EventHandler(this.btnsavemp_Click);
            // 
            // pEliminarEmpresa
            // 
            this.pEliminarEmpresa.Controls.Add(this.lbldelete);
            this.pEliminarEmpresa.Controls.Add(this.btndelete);
            resources.ApplyResources(this.pEliminarEmpresa, "pEliminarEmpresa");
            this.pEliminarEmpresa.Name = "pEliminarEmpresa";
            // 
            // lbldelete
            // 
            resources.ApplyResources(this.lbldelete, "lbldelete");
            this.lbldelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelete.Name = "lbldelete";
            // 
            // btndelete
            // 
            this.btndelete.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            resources.ApplyResources(this.btndelete, "btndelete");
            this.btndelete.FlatAppearance.BorderSize = 0;
            this.btndelete.Name = "btndelete";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Name = "label1";
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Name = "label23";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::controlFallos.Properties.Resources.delete;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbltitle
            // 
            resources.ApplyResources(this.lbltitle, "lbltitle");
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // catEmpresas
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pEliminarEmpresa);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gbcempresa);
            this.Controls.Add(this.gbaddbussiness);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "catEmpresas";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.catEmpresas_Load);
            this.gbcempresa.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.busqempresa)).EndInit();
            this.gbaddbussiness.ResumeLayout(false);
            this.gbaddbussiness.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pblogo)).EndInit();
            this.pCancel.ResumeLayout(false);
            this.pCancel.PerformLayout();
            this.pEliminarEmpresa.ResumeLayout(false);
            this.pEliminarEmpresa.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbcempresa;
        private System.Windows.Forms.DataGridView busqempresa;
        private System.Windows.Forms.GroupBox gbaddbussiness;
        private System.Windows.Forms.Panel pEliminarEmpresa;
        private System.Windows.Forms.Label lbldelete;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Button btnCancelEmpresa;
        private System.Windows.Forms.TextBox txtgetnempresa;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblsavemp;
        private System.Windows.Forms.Button btnsavemp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Panel pCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.Label lbllogo;
        public System.Windows.Forms.PictureBox pblogo;
        private System.Windows.Forms.LinkLabel lnkrestablecer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Usu;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn empresaa;
        private System.Windows.Forms.DataGridViewTextBoxColumn areaa;
    }
}