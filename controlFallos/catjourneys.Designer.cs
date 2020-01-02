
using System;

namespace controlFallos
{
    partial class catjourneys
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
            this.lblsave = new System.Windows.Forms.Label();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtjourneyname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pcancel = new System.Windows.Forms.Panel();
            this.gbjourney = new System.Windows.Forms.GroupBox();
            this.txtjourneyduration = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvjorneys = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dgvjourneyname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dgvjourneyduration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.people = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dgvestatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbldelete = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.pdelete = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pcancel.SuspendLayout();
            this.gbjourney.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvjorneys)).BeginInit();
            this.pdelete.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblsave
            // 
            this.lblsave.AutoSize = true;
            this.lblsave.Location = new System.Drawing.Point(337, 211);
            this.lblsave.Name = "lblsave";
            this.lblsave.Size = new System.Drawing.Size(60, 18);
            this.lblsave.TabIndex = 23;
            this.lblsave.Text = "Guardar";
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.Transparent;
            this.btnsave.BackgroundImage = global::controlFallos.Properties.Resources.save;
            this.btnsave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnsave.FlatAppearance.BorderSize = 0;
            this.btnsave.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Location = new System.Drawing.Point(346, 166);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(40, 40);
            this.btnsave.TabIndex = 22;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.Btnsave_Click);
            // 
            // txtjourneyname
            // 
            this.txtjourneyname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtjourneyname.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtjourneyname.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtjourneyname.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtjourneyname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtjourneyname.Location = new System.Drawing.Point(341, 47);
            this.txtjourneyname.MaxLength = 30;
            this.txtjourneyname.Name = "txtjourneyname";
            this.txtjourneyname.ShortcutsEnabled = false;
            this.txtjourneyname.Size = new System.Drawing.Size(289, 15);
            this.txtjourneyname.TabIndex = 1;
            this.txtjourneyname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Invalidate);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(337, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 9);
            this.label2.TabIndex = 19;
            this.label2.Text = "________________________________________________________________________";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label3.Location = new System.Drawing.Point(0, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Nuevo";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btncancel
            // 
            this.btncancel.BackgroundImage = global::controlFallos.Properties.Resources.add;
            this.btncancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btncancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btncancel.FlatAppearance.BorderSize = 0;
            this.btncancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btncancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btncancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Location = new System.Drawing.Point(10, 0);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(40, 40);
            this.btncancel.TabIndex = 24;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Crimson;
            this.label22.Location = new System.Drawing.Point(186, 252);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(47, 18);
            this.label22.TabIndex = 27;
            this.label22.Text = "Nota:";
            this.label22.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.label23.Location = new System.Drawing.Point(225, 252);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(474, 18);
            this.label23.TabIndex = 28;
            this.label23.Text = " Para Actualizar la Información de Doble Clic sobre el registro de la Tabla";
            this.label23.Visible = false;
            // 
            // pcancel
            // 
            this.pcancel.Controls.Add(this.label3);
            this.pcancel.Controls.Add(this.btncancel);
            this.pcancel.Location = new System.Drawing.Point(514, 164);
            this.pcancel.Name = "pcancel";
            this.pcancel.Size = new System.Drawing.Size(59, 65);
            this.pcancel.TabIndex = 26;
            this.pcancel.Visible = false;
            // 
            // gbjourney
            // 
            this.gbjourney.AutoSize = true;
            this.gbjourney.Controls.Add(this.txtjourneyduration);
            this.gbjourney.Controls.Add(this.label5);
            this.gbjourney.Controls.Add(this.label4);
            this.gbjourney.Controls.Add(this.label22);
            this.gbjourney.Controls.Add(this.label23);
            this.gbjourney.Controls.Add(this.pcancel);
            this.gbjourney.Controls.Add(this.lblsave);
            this.gbjourney.Controls.Add(this.btnsave);
            this.gbjourney.Controls.Add(this.txtjourneyname);
            this.gbjourney.Controls.Add(this.label1);
            this.gbjourney.Controls.Add(this.label2);
            this.gbjourney.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbjourney.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.gbjourney.Location = new System.Drawing.Point(0, 0);
            this.gbjourney.Name = "gbjourney";
            this.gbjourney.Size = new System.Drawing.Size(733, 303);
            this.gbjourney.TabIndex = 33;
            this.gbjourney.TabStop = false;
            this.gbjourney.Text = "Agregar Jornada";
            // 
            // txtjourneyduration
            // 
            this.txtjourneyduration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtjourneyduration.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtjourneyduration.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtjourneyduration.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtjourneyduration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.txtjourneyduration.Location = new System.Drawing.Point(341, 100);
            this.txtjourneyduration.MaxLength = 5;
            this.txtjourneyduration.Name = "txtjourneyduration";
            this.txtjourneyduration.ShortcutsEnabled = false;
            this.txtjourneyduration.Size = new System.Drawing.Size(102, 15);
            this.txtjourneyduration.TabIndex = 31;
            this.txtjourneyduration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtjourneyduration_KeyPress);
            this.txtjourneyduration.Validating += new System.ComponentModel.CancelEventHandler(this.txtjourneyduration_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Garamond", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(337, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 9);
            this.label5.TabIndex = 32;
            this.label5.Text = "__________________________";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 18);
            this.label4.TabIndex = 30;
            this.label4.Text = "Duración de Jornada (HH:mm):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 18);
            this.label1.TabIndex = 18;
            this.label1.Text = "Nombre de Jornada:";
            // 
            // dgvjorneys
            // 
            this.dgvjorneys.AllowUserToAddRows = false;
            this.dgvjorneys.AllowUserToDeleteRows = false;
            this.dgvjorneys.AllowUserToResizeColumns = false;
            this.dgvjorneys.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvjorneys.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvjorneys.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvjorneys.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvjorneys.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvjorneys.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvjorneys.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.RaisedVertical;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvjorneys.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvjorneys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvjorneys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this._dgvjourneyname,
            this._dgvjourneyduration,
            this.people,
            this._dgvestatus});
            this.dgvjorneys.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvjorneys.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvjorneys.EnableHeadersVisualStyles = false;
            this.dgvjorneys.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dgvjorneys.Location = new System.Drawing.Point(0, 309);
            this.dgvjorneys.MultiSelect = false;
            this.dgvjorneys.Name = "dgvjorneys";
            this.dgvjorneys.ReadOnly = true;
            this.dgvjorneys.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvjorneys.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvjorneys.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvjorneys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvjorneys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvjorneys.ShowCellErrors = false;
            this.dgvjorneys.ShowCellToolTips = false;
            this.dgvjorneys.ShowEditingIcon = false;
            this.dgvjorneys.Size = new System.Drawing.Size(731, 186);
            this.dgvjorneys.TabIndex = 0;
            this.dgvjorneys.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvjorneys_CellDoubleClick);
            this.dgvjorneys.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvjorneys_CellFormatting);
            // 
            // id
            // 
            this.id.HeaderText = "journeyID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // _dgvjourneyname
            // 
            this._dgvjourneyname.HeaderText = "JORNADA";
            this._dgvjourneyname.Name = "_dgvjourneyname";
            this._dgvjourneyname.ReadOnly = true;
            this._dgvjourneyname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _dgvjourneyduration
            // 
            this._dgvjourneyduration.HeaderText = "DURACIÓN";
            this._dgvjourneyduration.Name = "_dgvjourneyduration";
            this._dgvjourneyduration.ReadOnly = true;
            // 
            // people
            // 
            this.people.HeaderText = "PERSONA QUE DIÓ DE ALTA";
            this.people.Name = "people";
            this.people.ReadOnly = true;
            this.people.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _dgvestatus
            // 
            this._dgvestatus.HeaderText = "ESTATUS";
            this._dgvestatus.Name = "_dgvestatus";
            this._dgvestatus.ReadOnly = true;
            this._dgvestatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lbldelete
            // 
            this.lbldelete.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbldelete.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.lbldelete.Location = new System.Drawing.Point(0, 47);
            this.lbldelete.Name = "lbldelete";
            this.lbldelete.Size = new System.Drawing.Size(82, 18);
            this.lbldelete.TabIndex = 0;
            this.lbldelete.Text = "Desactivar";
            this.lbldelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btndelete
            // 
            this.btndelete.BackgroundImage = global::controlFallos.Properties.Resources.delete__4_;
            this.btndelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btndelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btndelete.FlatAppearance.BorderSize = 0;
            this.btndelete.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btndelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btndelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btndelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelete.Location = new System.Drawing.Point(20, 2);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(40, 40);
            this.btndelete.TabIndex = 0;
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // pdelete
            // 
            this.pdelete.Controls.Add(this.lbldelete);
            this.pdelete.Controls.Add(this.btndelete);
            this.pdelete.Location = new System.Drawing.Point(134, 164);
            this.pdelete.Name = "pdelete";
            this.pdelete.Size = new System.Drawing.Size(82, 65);
            this.pdelete.TabIndex = 25;
            this.pdelete.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pdelete);
            this.panel2.Controls.Add(this.dgvjorneys);
            this.panel2.Controls.Add(this.gbjourney);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(735, 499);
            this.panel2.TabIndex = 34;
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
            this.button1.Location = new System.Drawing.Point(705, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 27);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(267, 4);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(184, 21);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Catálogo de Jornadas";
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForme);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lbltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 27);
            this.panel1.TabIndex = 33;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moveForme);
            // 
            // catjourneys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(735, 526);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "catjourneys";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "catjourneys";
            this.pcancel.ResumeLayout(false);
            this.gbjourney.ResumeLayout(false);
            this.gbjourney.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvjorneys)).EndInit();
            this.pdelete.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #region Eventos From Controls
        /// <summary>
        /// Method that allow add or update a record after to perfom the appropiate validations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnsave_Click(object sender, System.EventArgs e)
        {
            bool res = false;
            object validationResult = causesValidation();
            if (validationResult != null) { Owner.Owner.sendUser(validationResult.ToString(), validaciones.MessageBoxTitle.Error); return; }
            if (editar)
            {
                observacionesEdicion obs = new observacionesEdicion(Owner.Owner.v);
                if (obs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    if (Owner.Owner.v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Jornadas','" + journeyIDTemp + "','" + _journeyName + "','" + _journeyDuration + "',NOW(),'Actualización de Jornada','" + Owner.Owner.v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower()) + "','" + Owner.Owner.empresa + "','" + Owner.Owner.area + "')"))
                        res = Owner.Owner.v.c.insertar("UPDATE cjourneys SET " + getCambiosString() + " WHERE journeyID = '" + journeyIDTemp.ToString() + "'");
                }
            }
            else res = Owner.Owner.v.c.insertar(string.Format("INSERT INTO cjourneys(journeyname, duration, userfkcpersonal) VALUES('{0}',TIME('{1}'),'{2}')", new object[] { Owner.Owner.v.mayusculas(txtjourneyname.Text.Trim().ToLower()), txtjourneyduration.Text.Trim(), Owner.Owner.idUsuario }));
            if (res)
            {
                if (!AlreadyshowsMessage) Owner.Owner.sendUser("Información " + (editar ? "Actualizada" : "Agregada") + " Exitosamente", validaciones.MessageBoxTitle.Información);
                LoadData();
                clearControls();
            }
        }
        /// <summary>
        /// Method that allow invalidate keywords that are not words or numbers
        /// </summary>
        /// <param name="sender">The Textbox </param>
        /// <param name="e">Event for invalidate</param>
        private void Invalidate(object sender, System.Windows.Forms.KeyPressEventArgs e) => Owner.Owner.v.letrasynumeros(e);
        /// <summary>
        /// Method that allow invalidate keywords that are not numbers or colon
        /// </summary>
        /// <param name="sender">The Textboc</param>
        /// <param name="e">Event o invalidate</param>
        private void txtjourneyduration_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) => e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == 58 || char.IsControl(e.KeyChar));
        /// <summary>
        /// Methods That allow Select a record from the datagridview for its later updated
        /// </summary>
        /// <param name="sender">The Datagridview</param>
        /// <param name="e">event launched when it is click in a row from the datagridview</param>
        private void dgvjorneys_CellDoubleClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (editar)
            {
                if (getCambios())
                {

                    var DialogResult = (System.Windows.Forms.MessageBox.Show("Desea Guardar La Infromación", validaciones.MessageBoxTitle.Confirmar.ToString(), System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question));
                    if (DialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        AlreadyshowsMessage = true;
                        Btnsave_Click(null, e);
                        LoadRecord(e);
                    }
                    else if (DialogResult == System.Windows.Forms.DialogResult.No)
                    {
                        clearControls();
                        LoadRecord(e);
                    }
                }
                else
                    LoadRecord(e);
            }
            else
                LoadRecord(e);
        }
        /// <summary>
        /// Method Thats allos add or update a record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btndelete_Click(object sender, System.EventArgs e)
        {
            observacionesEdicion obs = new observacionesEdicion(Owner.Owner.v);
            obs.Owner = this;
            obs.lblinfo.Text = "Ingrese el Motivo de la " + (activo ? "Des" : "Re") + "activación de la Jornada";
            obs.lblinfo.Location = new System.Drawing.Point(obs.lblinfo.Location.X + 15, obs.lblinfo.Location.Y);
            if (obs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (Owner.Owner.v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro,  usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion ,empresa,area) VALUES('Catálogo de Jornadas','" + journeyIDTemp + "','" + Owner.Owner.idUsuario + "',NOW(),'" + (activo ? "Des" : "Re") + "activación de Jornada','" + Owner.Owner.v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower()) + "','" + Owner.Owner.empresa + "','" + Owner.Owner.area + "')"))
                {
                    if (Owner.Owner.v.c.insertar("UPDATE cjourneys SET status = " + (activo ? 0 : 1) + " WHERE journeyID  = " + journeyIDTemp))
                    {
                        Owner.Owner.sendUser("La Jornada ha sido " + (activo ? "Des" : "Re") + "activada Correctamente", validaciones.MessageBoxTitle.Información);
                        clearControls();
                        LoadData();
                    }
                }
            }
        }
        private void dgvjorneys_CellFormatting(object sender, System.Windows.Forms.DataGridViewCellFormattingEventArgs e) { if (e.ColumnIndex == 4) e.CellStyle.BackColor = (e.Value.ToString().Equals("ACTIVO") ? System.Drawing.Color.PaleGreen : System.Drawing.Color.LightCoral); }
        private void btncancel_Click(object sender, System.EventArgs e)
        {
            if (editar)
            {
                if (getCambios())
                {

                    var DialogResult = (System.Windows.Forms.MessageBox.Show("Desea Guardar La Infromación", validaciones.MessageBoxTitle.Confirmar.ToString(), System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question));
                    if (DialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        AlreadyshowsMessage = true;
                        Btnsave_Click(null, e);
                        clearControls();
                        LoadData();
                    }
                    else if (DialogResult == System.Windows.Forms.DialogResult.No)
                    {
                        clearControls();
                        clearControls();
                        LoadData();
                    }
                }
                else
                {
                    clearControls();
                    LoadData();
                }
            }
            else
            {
                clearControls();
                LoadData();
            }
        }
        private void moveForme(object sender, System.Windows.Forms.MouseEventArgs e) =>
            Owner.Owner.v.mover(sender, e, this);
        /// <summary>
        /// Change the visivility of the button "save"
        /// </summary>
        private void changeVisibility(object sender, System.EventArgs e) => btnsave.Visible = lblsave.Visible = getCambios();
        private void txtjourneyduration_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtjourneyduration.Text))
            {
                System.DateTime outTime;
                if (System.DateTime.TryParse(txtjourneyduration.Text, out outTime))
                    txtjourneyduration.Text = outTime.ToString("HH:mm");
            }
        }
        #endregion
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtjourneyname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel pcancel;
        private System.Windows.Forms.GroupBox gbjourney;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvjorneys;
        private System.Windows.Forms.Label lbldelete;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Panel pdelete;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtjourneyduration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn _dgvjourneyname;
        private System.Windows.Forms.DataGridViewTextBoxColumn _dgvjourneyduration;
        private System.Windows.Forms.DataGridViewTextBoxColumn people;
        private System.Windows.Forms.DataGridViewTextBoxColumn _dgvestatus;
    }
}