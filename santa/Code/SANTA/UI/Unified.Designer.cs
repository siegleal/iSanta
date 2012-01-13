namespace SANTA.UI
{
    partial class Unified
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Unified));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_SingleImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_ImageFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_DatabaseImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_SaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_GenerateReport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Using = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_About = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tools_Global = new System.Windows.Forms.ToolStrip();
            this.button_Save = new System.Windows.Forms.ToolStripButton();
            this.button_SaveAll = new System.Windows.Forms.ToolStripButton();
            this.button_GenerateReport = new System.Windows.Forms.ToolStripButton();
            this.imageSelect = new System.Windows.Forms.ListView();
            this.Images = new System.Windows.Forms.ColumnHeader();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.currentImageLabel = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tab_Details = new System.Windows.Forms.TabPage();
            this.resetFields = new System.Windows.Forms.Button();
            this.clearFields = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.weapon_Notes = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.weapon_SerialNumber = new System.Windows.Forms.TextBox();
            this.weapon_Nomenclature = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ammo_Caliber = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.ammo_ProjectileMass = new System.Windows.Forms.TextBox();
            this.ammo_Notes = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ammo_LotNumber = new System.Windows.Forms.TextBox();
            this.ammo_CaliberUnit = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.range_Place = new System.Windows.Forms.TextBox();
            this.range_TargetDistanceUnits = new System.Windows.Forms.ComboBox();
            this.range_Temperature = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.range_TargetDistance = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.general_ShooterLast = new System.Windows.Forms.TextBox();
            this.general_ShooterFirst = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.general_ShotsFired = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.general_Date = new System.Windows.Forms.DateTimePicker();
            this.tab_Scale = new System.Windows.Forms.TabPage();
            this.measureBox = new SANTA.UI.MeasureBox();
            this.tools_Scale = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.text_Width = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.text_Height = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.button_Perpendicular = new System.Windows.Forms.ToolStripButton();
            this.tab_Holes = new System.Windows.Forms.TabPage();
            this.selectBox = new SANTA.UI.SelectBox();
            this.tools_Holes = new System.Windows.Forms.ToolStrip();
            this.button_AddHole = new System.Windows.Forms.ToolStripButton();
            this.button_RemoveHole = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.button_Reset = new System.Windows.Forms.ToolStripButton();
            this.button_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.button_FindHoles = new System.Windows.Forms.ToolStripButton();
            this.label_HolesIdentified = new System.Windows.Forms.ToolStripLabel();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.mainMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tools_Global.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tab_Details.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tab_Scale.SuspendLayout();
            this.tools_Scale.SuspendLayout();
            this.tab_Holes.SuspendLayout();
            this.tools_Holes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_File,
            this.menu_Help});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(866, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "mainMenu";
            // 
            // menu_File
            // 
            this.menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Open,
            this.toolStripSeparator4,
            this.menu_Save,
            this.menu_SaveAll,
            this.menu_GenerateReport,
            this.toolStripSeparator7,
            this.menu_Settings,
            this.toolStripSeparator6,
            this.menu_Exit});
            this.menu_File.Name = "menu_File";
            this.menu_File.Size = new System.Drawing.Size(37, 20);
            this.menu_File.Text = "&File";
            // 
            // menu_Open
            // 
            this.menu_Open.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_SingleImage,
            this.menu_ImageFolder,
            this.menu_DatabaseImage});
            this.menu_Open.Name = "menu_Open";
            this.menu_Open.Size = new System.Drawing.Size(201, 22);
            this.menu_Open.Text = "&Open";
            // 
            // menu_SingleImage
            // 
            this.menu_SingleImage.Image = ((System.Drawing.Image)(resources.GetObject("menu_SingleImage.Image")));
            this.menu_SingleImage.Name = "menu_SingleImage";
            this.menu_SingleImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menu_SingleImage.Size = new System.Drawing.Size(254, 22);
            this.menu_SingleImage.Text = "Single image";
            this.menu_SingleImage.Click += new System.EventHandler(this.SelectSingleImage);
            // 
            // menu_ImageFolder
            // 
            this.menu_ImageFolder.Image = ((System.Drawing.Image)(resources.GetObject("menu_ImageFolder.Image")));
            this.menu_ImageFolder.Name = "menu_ImageFolder";
            this.menu_ImageFolder.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.O)));
            this.menu_ImageFolder.Size = new System.Drawing.Size(254, 22);
            this.menu_ImageFolder.Text = "Folder of images";
            this.menu_ImageFolder.Click += new System.EventHandler(this.SelectFolder);
            // 
            // menu_DatabaseImage
            // 
            this.menu_DatabaseImage.Name = "menu_DatabaseImage";
            this.menu_DatabaseImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.menu_DatabaseImage.Size = new System.Drawing.Size(254, 22);
            this.menu_DatabaseImage.Text = "Images from the &Database";
            this.menu_DatabaseImage.Click += new System.EventHandler(this.LoadImages);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(198, 6);
            // 
            // menu_Save
            // 
            this.menu_Save.Image = global::SANTA.Properties.Resources.save;
            this.menu_Save.Name = "menu_Save";
            this.menu_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menu_Save.Size = new System.Drawing.Size(201, 22);
            this.menu_Save.Text = "&Save";
            this.menu_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // menu_SaveAll
            // 
            this.menu_SaveAll.Image = global::SANTA.Properties.Resources.save_all;
            this.menu_SaveAll.Name = "menu_SaveAll";
            this.menu_SaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.menu_SaveAll.Size = new System.Drawing.Size(201, 22);
            this.menu_SaveAll.Text = "Save &All";
            this.menu_SaveAll.Click += new System.EventHandler(this.button_SaveAll_Click);
            // 
            // menu_GenerateReport
            // 
            this.menu_GenerateReport.Image = global::SANTA.Properties.Resources.generate_report;
            this.menu_GenerateReport.Name = "menu_GenerateReport";
            this.menu_GenerateReport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menu_GenerateReport.Size = new System.Drawing.Size(201, 22);
            this.menu_GenerateReport.Text = "&Generate Report";
            this.menu_GenerateReport.Click += new System.EventHandler(this.button_GenerateReport_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(198, 6);
            // 
            // menu_Settings
            // 
            this.menu_Settings.Name = "menu_Settings";
            this.menu_Settings.Size = new System.Drawing.Size(201, 22);
            this.menu_Settings.Text = "S&ettings";
            this.menu_Settings.Click += new System.EventHandler(this.OpenSettings);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(198, 6);
            // 
            // menu_Exit
            // 
            this.menu_Exit.Name = "menu_Exit";
            this.menu_Exit.Size = new System.Drawing.Size(201, 22);
            this.menu_Exit.Text = "E&xit";
            // 
            // menu_Help
            // 
            this.menu_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Using,
            this.menu_About});
            this.menu_Help.Name = "menu_Help";
            this.menu_Help.Size = new System.Drawing.Size(44, 20);
            this.menu_Help.Text = "&Help";
            // 
            // menu_Using
            // 
            this.menu_Using.Name = "menu_Using";
            this.menu_Using.Size = new System.Drawing.Size(145, 22);
            this.menu_Using.Text = "&Using SANTA";
            // 
            // menu_About
            // 
            this.menu_About.Name = "menu_About";
            this.menu_About.Size = new System.Drawing.Size(145, 22);
            this.menu_About.Text = "&About";
            this.menu_About.Click += new System.EventHandler(this.menu_About_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(866, 485);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tools_Global);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.imageSelect);
            this.splitContainer2.Size = new System.Drawing.Size(200, 485);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 1;
            // 
            // tools_Global
            // 
            this.tools_Global.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tools_Global.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_Save,
            this.button_SaveAll,
            this.button_GenerateReport});
            this.tools_Global.Location = new System.Drawing.Point(0, 0);
            this.tools_Global.Name = "tools_Global";
            this.tools_Global.Size = new System.Drawing.Size(200, 25);
            this.tools_Global.TabIndex = 0;
            this.tools_Global.Text = "toolStrip2";
            // 
            // button_Save
            // 
            this.button_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_Save.Image = ((System.Drawing.Image)(resources.GetObject("button_Save.Image")));
            this.button_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(23, 22);
            this.button_Save.Text = "&Save Selected to Database";
            // 
            // button_SaveAll
            // 
            this.button_SaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_SaveAll.Image = global::SANTA.Properties.Resources.save_all;
            this.button_SaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_SaveAll.Name = "button_SaveAll";
            this.button_SaveAll.Size = new System.Drawing.Size(23, 22);
            this.button_SaveAll.Text = "Save &All to Database";
            // 
            // button_GenerateReport
            // 
            this.button_GenerateReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_GenerateReport.Image = ((System.Drawing.Image)(resources.GetObject("button_GenerateReport.Image")));
            this.button_GenerateReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_GenerateReport.Name = "button_GenerateReport";
            this.button_GenerateReport.Size = new System.Drawing.Size(23, 22);
            this.button_GenerateReport.Text = "Save and &Generate Report";
            // 
            // imageSelect
            // 
            this.imageSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Images});
            this.imageSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageSelect.FullRowSelect = true;
            this.imageSelect.HideSelection = false;
            this.imageSelect.Location = new System.Drawing.Point(0, 0);
            this.imageSelect.Name = "imageSelect";
            this.imageSelect.Size = new System.Drawing.Size(200, 456);
            this.imageSelect.TabIndex = 0;
            this.imageSelect.UseCompatibleStateImageBehavior = false;
            this.imageSelect.View = System.Windows.Forms.View.Details;
            // 
            // Images
            // 
            this.Images.Text = "Images";
            this.Images.Width = 179;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.currentImageLabel);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tabs);
            this.splitContainer3.Size = new System.Drawing.Size(662, 485);
            this.splitContainer3.SplitterDistance = 25;
            this.splitContainer3.TabIndex = 1;
            // 
            // currentImageLabel
            // 
            this.currentImageLabel.AutoSize = true;
            this.currentImageLabel.ForeColor = System.Drawing.SystemColors.MenuText;
            this.currentImageLabel.Location = new System.Drawing.Point(3, 7);
            this.currentImageLabel.Name = "currentImageLabel";
            this.currentImageLabel.Size = new System.Drawing.Size(73, 13);
            this.currentImageLabel.TabIndex = 0;
            this.currentImageLabel.Text = "Current: None";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tab_Details);
            this.tabs.Controls.Add(this.tab_Scale);
            this.tabs.Controls.Add(this.tab_Holes);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(662, 456);
            this.tabs.TabIndex = 0;
            // 
            // tab_Details
            // 
            this.tab_Details.Controls.Add(this.resetFields);
            this.tab_Details.Controls.Add(this.clearFields);
            this.tab_Details.Controls.Add(this.groupBox3);
            this.tab_Details.Controls.Add(this.groupBox4);
            this.tab_Details.Controls.Add(this.groupBox2);
            this.tab_Details.Controls.Add(this.groupBox1);
            this.tab_Details.Location = new System.Drawing.Point(4, 22);
            this.tab_Details.Name = "tab_Details";
            this.tab_Details.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Details.Size = new System.Drawing.Size(654, 430);
            this.tab_Details.TabIndex = 0;
            this.tab_Details.Text = "Details";
            this.tab_Details.UseVisualStyleBackColor = true;
            // 
            // resetFields
            // 
            this.resetFields.Location = new System.Drawing.Point(492, 291);
            this.resetFields.Name = "resetFields";
            this.resetFields.Size = new System.Drawing.Size(75, 23);
            this.resetFields.TabIndex = 4;
            this.resetFields.Text = "Reset Fields";
            this.resetFields.UseVisualStyleBackColor = true;
            // 
            // clearFields
            // 
            this.clearFields.Location = new System.Drawing.Point(573, 291);
            this.clearFields.Name = "clearFields";
            this.clearFields.Size = new System.Drawing.Size(75, 23);
            this.clearFields.TabIndex = 5;
            this.clearFields.Text = "Clear Fields";
            this.clearFields.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.weapon_Notes);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.weapon_SerialNumber);
            this.groupBox3.Controls.Add(this.weapon_Nomenclature);
            this.groupBox3.Location = new System.Drawing.Point(6, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(318, 149);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Weapon Information";
            // 
            // weapon_Notes
            // 
            this.weapon_Notes.Location = new System.Drawing.Point(9, 94);
            this.weapon_Notes.Name = "weapon_Notes";
            this.weapon_Notes.Size = new System.Drawing.Size(300, 47);
            this.weapon_Notes.TabIndex = 2;
            this.weapon_Notes.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Notes:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Serial number:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Nomenclature:";
            // 
            // weapon_SerialNumber
            // 
            this.weapon_SerialNumber.Location = new System.Drawing.Point(113, 48);
            this.weapon_SerialNumber.Name = "weapon_SerialNumber";
            this.weapon_SerialNumber.Size = new System.Drawing.Size(100, 20);
            this.weapon_SerialNumber.TabIndex = 1;
            // 
            // weapon_Nomenclature
            // 
            this.weapon_Nomenclature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.weapon_Nomenclature.DropDownWidth = 300;
            this.weapon_Nomenclature.FormattingEnabled = true;
            this.weapon_Nomenclature.Location = new System.Drawing.Point(113, 20);
            this.weapon_Nomenclature.Name = "weapon_Nomenclature";
            this.weapon_Nomenclature.Size = new System.Drawing.Size(196, 21);
            this.weapon_Nomenclature.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ammo_Caliber);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.ammo_ProjectileMass);
            this.groupBox4.Controls.Add(this.ammo_Notes);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.ammo_LotNumber);
            this.groupBox4.Controls.Add(this.ammo_CaliberUnit);
            this.groupBox4.Location = new System.Drawing.Point(329, 112);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(319, 176);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Ammunition Information";
            // 
            // ammo_Caliber
            // 
            this.ammo_Caliber.Location = new System.Drawing.Point(113, 22);
            this.ammo_Caliber.Name = "ammo_Caliber";
            this.ammo_Caliber.Size = new System.Drawing.Size(42, 20);
            this.ammo_Caliber.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(168, 78);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 13);
            this.label15.TabIndex = 9;
            this.label15.Text = "grains";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 13);
            this.label14.TabIndex = 7;
            this.label14.Text = "Projectile mass:";
            // 
            // ammo_ProjectileMass
            // 
            this.ammo_ProjectileMass.Location = new System.Drawing.Point(112, 74);
            this.ammo_ProjectileMass.Name = "ammo_ProjectileMass";
            this.ammo_ProjectileMass.Size = new System.Drawing.Size(42, 20);
            this.ammo_ProjectileMass.TabIndex = 3;
            // 
            // ammo_Notes
            // 
            this.ammo_Notes.Location = new System.Drawing.Point(6, 121);
            this.ammo_Notes.Name = "ammo_Notes";
            this.ammo_Notes.Size = new System.Drawing.Size(304, 47);
            this.ammo_Notes.TabIndex = 4;
            this.ammo_Notes.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 105);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Notes:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 51);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Lot number:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(42, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "Caliber:";
            // 
            // ammo_LotNumber
            // 
            this.ammo_LotNumber.Location = new System.Drawing.Point(112, 48);
            this.ammo_LotNumber.Name = "ammo_LotNumber";
            this.ammo_LotNumber.Size = new System.Drawing.Size(100, 20);
            this.ammo_LotNumber.TabIndex = 2;
            // 
            // ammo_CaliberUnit
            // 
            this.ammo_CaliberUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ammo_CaliberUnit.FormattingEnabled = true;
            this.ammo_CaliberUnit.Location = new System.Drawing.Point(171, 22);
            this.ammo_CaliberUnit.Name = "ammo_CaliberUnit";
            this.ammo_CaliberUnit.Size = new System.Drawing.Size(51, 21);
            this.ammo_CaliberUnit.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.range_Place);
            this.groupBox2.Controls.Add(this.range_TargetDistanceUnits);
            this.groupBox2.Controls.Add(this.range_Temperature);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.range_TargetDistance);
            this.groupBox2.Location = new System.Drawing.Point(330, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Range Information";
            // 
            // range_Place
            // 
            this.range_Place.Location = new System.Drawing.Point(113, 19);
            this.range_Place.Name = "range_Place";
            this.range_Place.Size = new System.Drawing.Size(196, 20);
            this.range_Place.TabIndex = 0;
            // 
            // range_TargetDistanceUnits
            // 
            this.range_TargetDistanceUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.range_TargetDistanceUnits.FormattingEnabled = true;
            this.range_TargetDistanceUnits.Location = new System.Drawing.Point(171, 46);
            this.range_TargetDistanceUnits.Name = "range_TargetDistanceUnits";
            this.range_TargetDistanceUnits.Size = new System.Drawing.Size(57, 21);
            this.range_TargetDistanceUnits.TabIndex = 2;
            // 
            // range_Temperature
            // 
            this.range_Temperature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.range_Temperature.FormattingEnabled = true;
            this.range_Temperature.Location = new System.Drawing.Point(113, 73);
            this.range_Temperature.Name = "range_Temperature";
            this.range_Temperature.Size = new System.Drawing.Size(132, 21);
            this.range_Temperature.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Range temperature:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Target distance:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Place:";
            // 
            // range_TargetDistance
            // 
            this.range_TargetDistance.Location = new System.Drawing.Point(113, 46);
            this.range_TargetDistance.Name = "range_TargetDistance";
            this.range_TargetDistance.Size = new System.Drawing.Size(42, 20);
            this.range_TargetDistance.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.general_ShooterLast);
            this.groupBox1.Controls.Add(this.general_ShooterFirst);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.general_ShotsFired);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.general_Date);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 153);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Information";
            // 
            // general_ShooterLast
            // 
            this.general_ShooterLast.Location = new System.Drawing.Point(113, 99);
            this.general_ShooterLast.Name = "general_ShooterLast";
            this.general_ShooterLast.Size = new System.Drawing.Size(196, 20);
            this.general_ShooterLast.TabIndex = 2;
            // 
            // general_ShooterFirst
            // 
            this.general_ShooterFirst.Location = new System.Drawing.Point(113, 72);
            this.general_ShooterFirst.Name = "general_ShooterFirst";
            this.general_ShooterFirst.Size = new System.Drawing.Size(196, 20);
            this.general_ShooterFirst.TabIndex = 1;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(22, 102);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 13);
            this.label18.TabIndex = 7;
            this.label18.Text = "Last name:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(22, 75);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "First name:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(6, 128);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 14;
            this.label16.Text = "Total shots fired:";
            // 
            // general_ShotsFired
            // 
            this.general_ShotsFired.Location = new System.Drawing.Point(113, 125);
            this.general_ShotsFired.Name = "general_ShotsFired";
            this.general_ShotsFired.Size = new System.Drawing.Size(42, 20);
            this.general_ShotsFired.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Shooter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date Fired:";
            // 
            // general_Date
            // 
            this.general_Date.Location = new System.Drawing.Point(113, 19);
            this.general_Date.Name = "general_Date";
            this.general_Date.Size = new System.Drawing.Size(196, 20);
            this.general_Date.TabIndex = 0;
            // 
            // tab_Scale
            // 
            this.tab_Scale.Controls.Add(this.measureBox);
            this.tab_Scale.Controls.Add(this.tools_Scale);
            this.tab_Scale.Location = new System.Drawing.Point(4, 22);
            this.tab_Scale.Name = "tab_Scale";
            this.tab_Scale.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Scale.Size = new System.Drawing.Size(654, 430);
            this.tab_Scale.TabIndex = 2;
            this.tab_Scale.Text = "Scale";
            this.tab_Scale.UseVisualStyleBackColor = true;
            // 
            // measureBox
            // 
            this.measureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.measureBox.Location = new System.Drawing.Point(3, 28);
            this.measureBox.Name = "measureBox";
            this.measureBox.PerpendicularScale = false;
            this.measureBox.Size = new System.Drawing.Size(648, 399);
            this.measureBox.TabIndex = 2;
            this.measureBox.Text = "measureBox1";
            // 
            // tools_Scale
            // 
            this.tools_Scale.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tools_Scale.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel6,
            this.text_Width,
            this.toolStripLabel7,
            this.text_Height,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.button_Perpendicular});
            this.tools_Scale.Location = new System.Drawing.Point(3, 3);
            this.tools_Scale.Name = "tools_Scale";
            this.tools_Scale.Size = new System.Drawing.Size(648, 25);
            this.tools_Scale.TabIndex = 1;
            this.tools_Scale.Text = "toolStrip1";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.ForeColor = System.Drawing.Color.Blue;
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel6.Text = "Width:";
            // 
            // text_Width
            // 
            this.text_Width.Name = "text_Width";
            this.text_Width.Size = new System.Drawing.Size(40, 25);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.ForeColor = System.Drawing.Color.Red;
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabel7.Text = "Height:";
            // 
            // text_Height
            // 
            this.text_Height.Name = "text_Height";
            this.text_Height.Size = new System.Drawing.Size(40, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel1.Text = "inches";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // button_Perpendicular
            // 
            this.button_Perpendicular.CheckOnClick = true;
            this.button_Perpendicular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_Perpendicular.Image = global::SANTA.Properties.Resources.perpendicular;
            this.button_Perpendicular.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_Perpendicular.Name = "button_Perpendicular";
            this.button_Perpendicular.Size = new System.Drawing.Size(23, 22);
            this.button_Perpendicular.Text = "Keep scale markers perpendicular";
            this.button_Perpendicular.ToolTipText = "Keep scale markers perpendicular";
            // 
            // tab_Holes
            // 
            this.tab_Holes.Controls.Add(this.selectBox);
            this.tab_Holes.Controls.Add(this.tools_Holes);
            this.tab_Holes.Location = new System.Drawing.Point(4, 22);
            this.tab_Holes.Name = "tab_Holes";
            this.tab_Holes.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Holes.Size = new System.Drawing.Size(654, 430);
            this.tab_Holes.TabIndex = 3;
            this.tab_Holes.Text = "Bullet Holes";
            this.tab_Holes.UseVisualStyleBackColor = true;
            // 
            // selectBox
            // 
            this.selectBox.BulletHoles = ((System.Collections.Generic.List<System.Drawing.Point>)(resources.GetObject("selectBox.BulletHoles")));
            this.selectBox.CurrentTool = SANTA.UI.SelectBox.Tool.AddSelector;
            this.selectBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectBox.HoleDiameter = 0;
            this.selectBox.Location = new System.Drawing.Point(3, 28);
            this.selectBox.Name = "selectBox";
            this.selectBox.Size = new System.Drawing.Size(648, 399);
            this.selectBox.TabIndex = 2;
            this.selectBox.Text = "selectBox1";
            // 
            // tools_Holes
            // 
            this.tools_Holes.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tools_Holes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_AddHole,
            this.button_RemoveHole,
            this.toolStripSeparator1,
            this.button_Reset,
            this.button_Clear,
            this.toolStripSeparator3,
            this.button_FindHoles,
            this.label_HolesIdentified});
            this.tools_Holes.Location = new System.Drawing.Point(3, 3);
            this.tools_Holes.Name = "tools_Holes";
            this.tools_Holes.Size = new System.Drawing.Size(648, 25);
            this.tools_Holes.TabIndex = 1;
            this.tools_Holes.Text = "toolStrip1";
            // 
            // button_AddHole
            // 
            this.button_AddHole.Checked = true;
            this.button_AddHole.CheckOnClick = true;
            this.button_AddHole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.button_AddHole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_AddHole.Image = ((System.Drawing.Image)(resources.GetObject("button_AddHole.Image")));
            this.button_AddHole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_AddHole.Name = "button_AddHole";
            this.button_AddHole.Size = new System.Drawing.Size(23, 22);
            this.button_AddHole.Text = "&Add Hole";
            // 
            // button_RemoveHole
            // 
            this.button_RemoveHole.CheckOnClick = true;
            this.button_RemoveHole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_RemoveHole.Image = ((System.Drawing.Image)(resources.GetObject("button_RemoveHole.Image")));
            this.button_RemoveHole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_RemoveHole.Name = "button_RemoveHole";
            this.button_RemoveHole.Size = new System.Drawing.Size(23, 22);
            this.button_RemoveHole.Text = "&Remove Hole";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // button_Reset
            // 
            this.button_Reset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_Reset.Image = ((System.Drawing.Image)(resources.GetObject("button_Reset.Image")));
            this.button_Reset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(39, 22);
            this.button_Reset.Text = "R&eset";
            // 
            // button_Clear
            // 
            this.button_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_Clear.Image = ((System.Drawing.Image)(resources.GetObject("button_Clear.Image")));
            this.button_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(38, 22);
            this.button_Clear.Text = "&Clear";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // button_FindHoles
            // 
            this.button_FindHoles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.button_FindHoles.Image = ((System.Drawing.Image)(resources.GetObject("button_FindHoles.Image")));
            this.button_FindHoles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_FindHoles.Name = "button_FindHoles";
            this.button_FindHoles.Size = new System.Drawing.Size(67, 22);
            this.button_FindHoles.Text = "Find Holes";
            this.button_FindHoles.ToolTipText = "Use image recognition to find the holes";
            // 
            // label_HolesIdentified
            // 
            this.label_HolesIdentified.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.label_HolesIdentified.Name = "label_HolesIdentified";
            this.label_HolesIdentified.Size = new System.Drawing.Size(96, 22);
            this.label_HolesIdentified.Text = "[holes identified]";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // Unified
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 509);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(882, 433);
            this.Name = "Unified";
            this.Text = "Small Arms Naval Target Analyzer";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tools_Global.ResumeLayout(false);
            this.tools_Global.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tab_Details.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tab_Scale.ResumeLayout(false);
            this.tab_Scale.PerformLayout();
            this.tools_Scale.ResumeLayout(false);
            this.tools_Scale.PerformLayout();
            this.tab_Holes.ResumeLayout(false);
            this.tab_Holes.PerformLayout();
            this.tools_Holes.ResumeLayout(false);
            this.tools_Holes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem menu_File;
        private System.Windows.Forms.ToolStripMenuItem menu_Help;
        private System.Windows.Forms.ToolStripMenuItem menu_Using;
        private System.Windows.Forms.ToolStripMenuItem menu_About;
        private System.Windows.Forms.ToolStripMenuItem menu_Open;
        private System.Windows.Forms.ToolStripMenuItem menu_SingleImage;
        private System.Windows.Forms.ToolStripMenuItem menu_ImageFolder;
        private System.Windows.Forms.ToolStripMenuItem menu_Settings;
        private System.Windows.Forms.ToolStripMenuItem menu_Exit;
        private System.Windows.Forms.ListView imageSelect;
        private System.Windows.Forms.ColumnHeader Images;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip tools_Global;
        private System.Windows.Forms.ToolStripButton button_Save;
        private System.Windows.Forms.ToolStripButton button_GenerateReport;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label currentImageLabel;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tab_Details;
        private System.Windows.Forms.Button resetFields;
        private System.Windows.Forms.Button clearFields;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox weapon_Notes;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox weapon_SerialNumber;
        private System.Windows.Forms.ComboBox weapon_Nomenclature;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox ammo_Caliber;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox ammo_ProjectileMass;
        private System.Windows.Forms.RichTextBox ammo_Notes;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox ammo_LotNumber;
        private System.Windows.Forms.ComboBox ammo_CaliberUnit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox range_Place;
        private System.Windows.Forms.ComboBox range_TargetDistanceUnits;
        private System.Windows.Forms.ComboBox range_Temperature;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox range_TargetDistance;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox general_ShooterLast;
        private System.Windows.Forms.TextBox general_ShooterFirst;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox general_ShotsFired;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker general_Date;
        private System.Windows.Forms.TabPage tab_Scale;
        private System.Windows.Forms.ToolStrip tools_Scale;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripTextBox text_Width;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripTextBox text_Height;
        private System.Windows.Forms.TabPage tab_Holes;
        private System.Windows.Forms.ToolStrip tools_Holes;
        private System.Windows.Forms.ToolStripButton button_AddHole;
        private System.Windows.Forms.ToolStripButton button_RemoveHole;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton button_Reset;
        private System.Windows.Forms.ToolStripButton button_Clear;
        private MeasureBox measureBox;
        private SelectBox selectBox;
        private System.Windows.Forms.ToolStripMenuItem menu_DatabaseImage;
        private System.Windows.Forms.ToolStripButton button_Perpendicular;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton button_SaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton button_FindHoles;
        private System.Windows.Forms.ToolStripLabel label_HolesIdentified;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolStripMenuItem menu_Save;
        private System.Windows.Forms.ToolStripMenuItem menu_SaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menu_GenerateReport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;

    }
}