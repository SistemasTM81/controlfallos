﻿namespace controlFallos
{
    partial class menuPrincipal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(menuPrincipal));
            this.lblnotif = new System.Windows.Forms.Label();
            this.lbltitle = new System.Windows.Forms.Label();
            this.notif = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pbnotif = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.catálogosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDePersonalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeProveedoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeUnidadesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeRefaccionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catálogoDeFallosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualizaciónDeIVAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteSupervicionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteDeSupervisiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteDePercancesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteDePersonalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteDeIncidentesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteDeIndicenciaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteMantenimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteAlmacenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.requisicionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ordenesDeCompraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comparativasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historialDeModificacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lblnumnotificaciones = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbnotif)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblnotif
            // 
            this.lblnotif.AutoSize = true;
            this.lblnotif.BackColor = System.Drawing.Color.Transparent;
            this.lblnotif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblnotif.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblnotif.Font = new System.Drawing.Font("Garamond", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnotif.ForeColor = System.Drawing.Color.White;
            this.lblnotif.Location = new System.Drawing.Point(95, 13);
            this.lblnotif.Name = "lblnotif";
            this.lblnotif.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblnotif.Size = new System.Drawing.Size(26, 30);
            this.lblnotif.TabIndex = 2;
            this.lblnotif.Text = "0";
            this.toolTip1.SetToolTip(this.lblnotif, "Notificaciones");
            this.lblnotif.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.AutoSize = true;
            this.lbltitle.BackColor = System.Drawing.Color.Crimson;
            this.lbltitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbltitle.Font = new System.Drawing.Font("Garamond", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(1623, 11);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(169, 27);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Text = "Menú Principal";
            this.lbltitle.Click += new System.EventHandler(this.lbltitle_DoubleClick);
            this.lbltitle.DoubleClick += new System.EventHandler(this.lbltitle_DoubleClick);
            this.lbltitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // notif
            // 
            this.notif.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notif.BalloonTipTitle = "Nuevas Notificaciones";
            this.notif.Icon = ((System.Drawing.Icon)(resources.GetObject("notif.Icon")));
            this.notif.Text = "Sistema de Control de Fallos";
            this.notif.Visible = true;
            this.notif.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notif.Click += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Crimson;
            this.panel3.Controls.Add(this.lblnotif);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.pbnotif);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 990);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1920, 50);
            this.panel3.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Alertas: ";
            this.label1.Click += new System.EventHandler(this.lbltitle_DoubleClick);
            // 
            // pbnotif
            // 
            this.pbnotif.BackColor = System.Drawing.Color.Crimson;
            this.pbnotif.BackgroundImage = global::controlFallos.Properties.Resources.notification__4_;
            this.pbnotif.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbnotif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbnotif.Location = new System.Drawing.Point(80, 2);
            this.pbnotif.Name = "pbnotif";
            this.pbnotif.Size = new System.Drawing.Size(58, 45);
            this.pbnotif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbnotif.TabIndex = 1;
            this.pbnotif.TabStop = false;
            this.toolTip1.SetToolTip(this.pbnotif, "Notificaciones");
            this.pbnotif.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Crimson;
            this.menuStrip1.Font = new System.Drawing.Font("Garamond", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.catálogosToolStripMenuItem,
            this.reporteSupervicionToolStripMenuItem,
            this.reporteMantenimientoToolStripMenuItem,
            this.reporteAlmacenToolStripMenuItem1,
            this.requisicionesToolStripMenuItem,
            this.historialDeModificacionesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(1920, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbltitle_MouseDown);
            // 
            // catálogosToolStripMenuItem
            // 
            this.catálogosToolStripMenuItem.AutoToolTip = true;
            this.catálogosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.catálogoDePersonalToolStripMenuItem,
            this.catálogoDeProveedoresToolStripMenuItem,
            this.catálogoDeUnidadesToolStripMenuItem,
            this.catálogoDeRefaccionesToolStripMenuItem,
            this.catálogoDeFallosToolStripMenuItem,
            this.actualizaciónDeIVAToolStripMenuItem,
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem});
            this.catálogosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogosToolStripMenuItem.Image = global::controlFallos.Properties.Resources.catalog__1_;
            this.catálogosToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.catálogosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogosToolStripMenuItem.Name = "catálogosToolStripMenuItem";
            this.catálogosToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 10);
            this.catálogosToolStripMenuItem.Size = new System.Drawing.Size(135, 51);
            this.catálogosToolStripMenuItem.Text = "Catálogos";
            this.catálogosToolStripMenuItem.Visible = false;
            this.catálogosToolStripMenuItem.MouseLeave += new System.EventHandler(this.catálogosToolStripMenuItem_MouseLeave);
            this.catálogosToolStripMenuItem.MouseHover += new System.EventHandler(this.catálogosToolStripMenuItem_MouseHover);
            // 
            // catálogoDePersonalToolStripMenuItem
            // 
            this.catálogoDePersonalToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDePersonalToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDePersonalToolStripMenuItem.Image = global::controlFallos.Properties.Resources.presentation;
            this.catálogoDePersonalToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDePersonalToolStripMenuItem.Name = "catálogoDePersonalToolStripMenuItem";
            this.catálogoDePersonalToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDePersonalToolStripMenuItem.ShowShortcutKeys = false;
            this.catálogoDePersonalToolStripMenuItem.Size = new System.Drawing.Size(349, 47);
            this.catálogoDePersonalToolStripMenuItem.Text = "Registro de Personal";
            this.catálogoDePersonalToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.catálogoDePersonalToolStripMenuItem.Visible = false;
            this.catálogoDePersonalToolStripMenuItem.Click += new System.EventHandler(this.catálogoDePersonalToolStripMenuItem_Click);
            this.catálogoDePersonalToolStripMenuItem.EnabledChanged += new System.EventHandler(this.catálogoDePersonalToolStripMenuItem_EnabledChanged);
            // 
            // catálogoDeProveedoresToolStripMenuItem
            // 
            this.catálogoDeProveedoresToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeProveedoresToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeProveedoresToolStripMenuItem.Image = global::controlFallos.Properties.Resources.businessman__4_;
            this.catálogoDeProveedoresToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeProveedoresToolStripMenuItem.Name = "catálogoDeProveedoresToolStripMenuItem";
            this.catálogoDeProveedoresToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeProveedoresToolStripMenuItem.Size = new System.Drawing.Size(349, 47);
            this.catálogoDeProveedoresToolStripMenuItem.Text = "Catálogo de Proveedores";
            this.catálogoDeProveedoresToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.catálogoDeProveedoresToolStripMenuItem.Visible = false;
            this.catálogoDeProveedoresToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeProveedoresToolStripMenuItem_Click);
            // 
            // catálogoDeUnidadesToolStripMenuItem
            // 
            this.catálogoDeUnidadesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeUnidadesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeUnidadesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.delivery_truck;
            this.catálogoDeUnidadesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeUnidadesToolStripMenuItem.Name = "catálogoDeUnidadesToolStripMenuItem";
            this.catálogoDeUnidadesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeUnidadesToolStripMenuItem.ShowShortcutKeys = false;
            this.catálogoDeUnidadesToolStripMenuItem.Size = new System.Drawing.Size(349, 47);
            this.catálogoDeUnidadesToolStripMenuItem.Text = "Catálogo de Unidades";
            this.catálogoDeUnidadesToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.catálogoDeUnidadesToolStripMenuItem.Visible = false;
            this.catálogoDeUnidadesToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeUnidadesToolStripMenuItem_Click);
            // 
            // catálogoDeRefaccionesToolStripMenuItem
            // 
            this.catálogoDeRefaccionesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeRefaccionesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeRefaccionesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.robber_silhouette_trying_to_steal_car_part__1_;
            this.catálogoDeRefaccionesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeRefaccionesToolStripMenuItem.Name = "catálogoDeRefaccionesToolStripMenuItem";
            this.catálogoDeRefaccionesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeRefaccionesToolStripMenuItem.Size = new System.Drawing.Size(349, 47);
            this.catálogoDeRefaccionesToolStripMenuItem.Text = "Catálogo de Refacciones";
            this.catálogoDeRefaccionesToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.catálogoDeRefaccionesToolStripMenuItem.Visible = false;
            this.catálogoDeRefaccionesToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeRefaccionesToolStripMenuItem_Click);
            // 
            // catálogoDeFallosToolStripMenuItem
            // 
            this.catálogoDeFallosToolStripMenuItem.AutoToolTip = true;
            this.catálogoDeFallosToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.catálogoDeFallosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.catálogoDeFallosToolStripMenuItem.Image = global::controlFallos.Properties.Resources.bug__1_;
            this.catálogoDeFallosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.catálogoDeFallosToolStripMenuItem.Name = "catálogoDeFallosToolStripMenuItem";
            this.catálogoDeFallosToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.catálogoDeFallosToolStripMenuItem.ShowShortcutKeys = false;
            this.catálogoDeFallosToolStripMenuItem.Size = new System.Drawing.Size(349, 47);
            this.catálogoDeFallosToolStripMenuItem.Text = "Catálogo de Fallos";
            this.catálogoDeFallosToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.catálogoDeFallosToolStripMenuItem.Visible = false;
            this.catálogoDeFallosToolStripMenuItem.Click += new System.EventHandler(this.catálogoDeFallosToolStripMenuItem_Click);
            // 
            // actualizaciónDeIVAToolStripMenuItem
            // 
            this.actualizaciónDeIVAToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.actualizaciónDeIVAToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.actualizaciónDeIVAToolStripMenuItem.Image = global::controlFallos.Properties.Resources.refresh_page_option;
            this.actualizaciónDeIVAToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.actualizaciónDeIVAToolStripMenuItem.Name = "actualizaciónDeIVAToolStripMenuItem";
            this.actualizaciónDeIVAToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.actualizaciónDeIVAToolStripMenuItem.Size = new System.Drawing.Size(349, 47);
            this.actualizaciónDeIVAToolStripMenuItem.Text = "Actualización de IVA ";
            this.actualizaciónDeIVAToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.actualizaciónDeIVAToolStripMenuItem.Visible = false;
            this.actualizaciónDeIVAToolStripMenuItem.Click += new System.EventHandler(this.actualizaciónDeIVAToolStripMenuItem_Click);
            // 
            // actualizaciónDeEncabezadosDeReportesToolStripMenuItem
            // 
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.sync;
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Name = "actualizaciónDeEncabezadosDeReportesToolStripMenuItem";
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Size = new System.Drawing.Size(349, 47);
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Text = "Actualización de Encabezados";
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Visible = false;
            this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem.Click += new System.EventHandler(this.actualizaciónDeEncabezadosDeReportesToolStripMenuItem_Click);
            // 
            // reporteSupervicionToolStripMenuItem
            // 
            this.reporteSupervicionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reporteDeSupervisiónToolStripMenuItem,
            this.reporteDePercancesToolStripMenuItem,
            this.reporteDePersonalToolStripMenuItem});
            this.reporteSupervicionToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteSupervicionToolStripMenuItem.Image = global::controlFallos.Properties.Resources.manager;
            this.reporteSupervicionToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteSupervicionToolStripMenuItem.Name = "reporteSupervicionToolStripMenuItem";
            this.reporteSupervicionToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.reporteSupervicionToolStripMenuItem.Size = new System.Drawing.Size(120, 51);
            this.reporteSupervicionToolStripMenuItem.Text = "Reportes";
            this.reporteSupervicionToolStripMenuItem.Visible = false;
            // 
            // reporteDeSupervisiónToolStripMenuItem
            // 
            this.reporteDeSupervisiónToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteDeSupervisiónToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteDeSupervisiónToolStripMenuItem.Image = global::controlFallos.Properties.Resources.bus1;
            this.reporteDeSupervisiónToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteDeSupervisiónToolStripMenuItem.Name = "reporteDeSupervisiónToolStripMenuItem";
            this.reporteDeSupervisiónToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteDeSupervisiónToolStripMenuItem.Size = new System.Drawing.Size(281, 39);
            this.reporteDeSupervisiónToolStripMenuItem.Text = "Reporte de Supervisión";
            this.reporteDeSupervisiónToolStripMenuItem.Click += new System.EventHandler(this.reporteDeSupervisiónToolStripMenuItem_Click);
            // 
            // reporteDePercancesToolStripMenuItem
            // 
            this.reporteDePercancesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteDePercancesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteDePercancesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.frontal_crash;
            this.reporteDePercancesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteDePercancesToolStripMenuItem.Name = "reporteDePercancesToolStripMenuItem";
            this.reporteDePercancesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteDePercancesToolStripMenuItem.Size = new System.Drawing.Size(281, 39);
            this.reporteDePercancesToolStripMenuItem.Text = "Reporte de Percances";
            this.reporteDePercancesToolStripMenuItem.Visible = false;
            this.reporteDePercancesToolStripMenuItem.Click += new System.EventHandler(this.reporteDePercancesToolStripMenuItem_Click);
            // 
            // reporteDePersonalToolStripMenuItem
            // 
            this.reporteDePersonalToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteDePersonalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reporteDeIncidentesToolStripMenuItem,
            this.reporteDeIndicenciaToolStripMenuItem});
            this.reporteDePersonalToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteDePersonalToolStripMenuItem.Image = global::controlFallos.Properties.Resources.manager1;
            this.reporteDePersonalToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteDePersonalToolStripMenuItem.Name = "reporteDePersonalToolStripMenuItem";
            this.reporteDePersonalToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteDePersonalToolStripMenuItem.Size = new System.Drawing.Size(281, 39);
            this.reporteDePersonalToolStripMenuItem.Text = "Formatos de  Personal";
            this.reporteDePersonalToolStripMenuItem.Visible = false;
            // 
            // reporteDeIncidentesToolStripMenuItem
            // 
            this.reporteDeIncidentesToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteDeIncidentesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteDeIncidentesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.archive;
            this.reporteDeIncidentesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteDeIncidentesToolStripMenuItem.Name = "reporteDeIncidentesToolStripMenuItem";
            this.reporteDeIncidentesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteDeIncidentesToolStripMenuItem.Size = new System.Drawing.Size(276, 39);
            this.reporteDeIncidentesToolStripMenuItem.Text = "Reporte de Personal";
            this.reporteDeIncidentesToolStripMenuItem.Visible = false;
            this.reporteDeIncidentesToolStripMenuItem.Click += new System.EventHandler(this.reporteDeIncidentesToolStripMenuItem_Click);
            // 
            // reporteDeIndicenciaToolStripMenuItem
            // 
            this.reporteDeIndicenciaToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.reporteDeIndicenciaToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteDeIndicenciaToolStripMenuItem.Image = global::controlFallos.Properties.Resources.magnifier;
            this.reporteDeIndicenciaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.reporteDeIndicenciaToolStripMenuItem.Name = "reporteDeIndicenciaToolStripMenuItem";
            this.reporteDeIndicenciaToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 1, 0, 10);
            this.reporteDeIndicenciaToolStripMenuItem.Size = new System.Drawing.Size(276, 39);
            this.reporteDeIndicenciaToolStripMenuItem.Text = "Indicencia de Personal";
            this.reporteDeIndicenciaToolStripMenuItem.Visible = false;
            this.reporteDeIndicenciaToolStripMenuItem.Click += new System.EventHandler(this.reporteDeIndicenciaToolStripMenuItem_Click);
            // 
            // reporteMantenimientoToolStripMenuItem
            // 
            this.reporteMantenimientoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteMantenimientoToolStripMenuItem.Image = global::controlFallos.Properties.Resources.construction;
            this.reporteMantenimientoToolStripMenuItem.Name = "reporteMantenimientoToolStripMenuItem";
            this.reporteMantenimientoToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.reporteMantenimientoToolStripMenuItem.Size = new System.Drawing.Size(235, 51);
            this.reporteMantenimientoToolStripMenuItem.Text = "Reporte Mantenimiento";
            this.reporteMantenimientoToolStripMenuItem.Visible = false;
            this.reporteMantenimientoToolStripMenuItem.Click += new System.EventHandler(this.reporteNivelMantenimientoToolStripMenuItem_Click);
            // 
            // reporteAlmacenToolStripMenuItem1
            // 
            this.reporteAlmacenToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.reporteAlmacenToolStripMenuItem1.Image = global::controlFallos.Properties.Resources.report;
            this.reporteAlmacenToolStripMenuItem1.Name = "reporteAlmacenToolStripMenuItem1";
            this.reporteAlmacenToolStripMenuItem1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.reporteAlmacenToolStripMenuItem1.Size = new System.Drawing.Size(182, 51);
            this.reporteAlmacenToolStripMenuItem1.Text = "Reporte Almacén";
            this.reporteAlmacenToolStripMenuItem1.Visible = false;
            this.reporteAlmacenToolStripMenuItem1.Click += new System.EventHandler(this.reporteNivelTransisumosToolStripMenuItem_Click);
            // 
            // requisicionesToolStripMenuItem
            // 
            this.requisicionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ordenesDeCompraToolStripMenuItem,
            this.comparativasToolStripMenuItem});
            this.requisicionesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.requisicionesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.shopping_cart;
            this.requisicionesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.requisicionesToolStripMenuItem.Name = "requisicionesToolStripMenuItem";
            this.requisicionesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.requisicionesToolStripMenuItem.Size = new System.Drawing.Size(159, 51);
            this.requisicionesToolStripMenuItem.Text = "Requisiciones";
            this.requisicionesToolStripMenuItem.Visible = false;
            // 
            // ordenesDeCompraToolStripMenuItem
            // 
            this.ordenesDeCompraToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.ordenesDeCompraToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.ordenesDeCompraToolStripMenuItem.Image = global::controlFallos.Properties.Resources.purchase_order__1_;
            this.ordenesDeCompraToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ordenesDeCompraToolStripMenuItem.Name = "ordenesDeCompraToolStripMenuItem";
            this.ordenesDeCompraToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.ordenesDeCompraToolStripMenuItem.Size = new System.Drawing.Size(255, 51);
            this.ordenesDeCompraToolStripMenuItem.Text = "Orden de Compra";
            this.ordenesDeCompraToolStripMenuItem.Click += new System.EventHandler(this.ordenesDeCompraToolStripMenuItem_Click);
            // 
            // comparativasToolStripMenuItem
            // 
            this.comparativasToolStripMenuItem.BackColor = System.Drawing.Color.Crimson;
            this.comparativasToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.comparativasToolStripMenuItem.Image = global::controlFallos.Properties.Resources.weight_balance;
            this.comparativasToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.comparativasToolStripMenuItem.Name = "comparativasToolStripMenuItem";
            this.comparativasToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.comparativasToolStripMenuItem.Size = new System.Drawing.Size(255, 51);
            this.comparativasToolStripMenuItem.Text = "Comparativas";
            this.comparativasToolStripMenuItem.Visible = false;
            this.comparativasToolStripMenuItem.Click += new System.EventHandler(this.comparativasToolStripMenuItem_Click);
            // 
            // historialDeModificacionesToolStripMenuItem
            // 
            this.historialDeModificacionesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.historialDeModificacionesToolStripMenuItem.Image = global::controlFallos.Properties.Resources.antique_building;
            this.historialDeModificacionesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.historialDeModificacionesToolStripMenuItem.Name = "historialDeModificacionesToolStripMenuItem";
            this.historialDeModificacionesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.historialDeModificacionesToolStripMenuItem.Size = new System.Drawing.Size(283, 51);
            this.historialDeModificacionesToolStripMenuItem.Text = "Historial de Modificaciones";
            this.historialDeModificacionesToolStripMenuItem.Visible = false;
            this.historialDeModificacionesToolStripMenuItem.Click += new System.EventHandler(this.historialDeModificacionesToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::controlFallos.Properties.Resources.minimazar;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(16, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 30);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Minimizar");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // button2
            // 
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImage = global::controlFallos.Properties.Resources.logout;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Crimson;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Crimson;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Crimson;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Crimson;
            this.button2.Location = new System.Drawing.Point(37, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 29);
            this.button2.TabIndex = 2;
            this.button2.TabStop = false;
            this.toolTip1.SetToolTip(this.button2, "Cerrar Sesión");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblnumnotificaciones
            // 
            this.lblnumnotificaciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblnumnotificaciones.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.lblnumnotificaciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblnumnotificaciones.Location = new System.Drawing.Point(0, 53);
            this.lblnumnotificaciones.Name = "lblnumnotificaciones";
            this.lblnumnotificaciones.Size = new System.Drawing.Size(1920, 937);
            this.lblnumnotificaciones.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipTitle = "Se Ha Validado El Pedido de Refaccion";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.BalloonTipClosed += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.BalloonTipShown += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_Click);
            // 
            // panel1
            // 
            this.panel1.CausesValidation = false;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(1850, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(67, 47);
            this.panel1.TabIndex = 1;
            // 
            // menuPrincipal
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.Crimson;
            this.ClientSize = new System.Drawing.Size(1920, 1040);
            this.Controls.Add(this.lbltitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblnumnotificaciones);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1040);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1918, 1038);
            this.Name = "menuPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "                                                                                 " +
    "                                                  ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.menuPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.menuPrincipal_Load);
            this.Resize += new System.EventHandler(this.menuPrincipal_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbnotif)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Panel lblnumnotificaciones;
        public System.Windows.Forms.Label lbltitle;
        public System.Windows.Forms.PictureBox pbnotif;
        public System.Windows.Forms.Label lblnotif;
        public System.Windows.Forms.NotifyIcon notif;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem catálogosToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem catálogoDeFallosToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem catálogoDePersonalToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem catálogoDeUnidadesToolStripMenuItem;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.ToolStripMenuItem catálogoDeRefaccionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem catálogoDeProveedoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteSupervicionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteMantenimientoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteAlmacenToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem historialDeModificacionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem requisicionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualizaciónDeIVAToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem ordenesDeCompraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comparativasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteDeSupervisiónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteDePercancesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteDePersonalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteDeIncidentesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteDeIndicenciaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualizaciónDeEncabezadosDeReportesToolStripMenuItem;
    }
}