using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SANTA.Controller;

namespace SANTA.UI
{
    /// <summary>
    /// The primary form for the application
    /// </summary>
    public partial class Unified : Form
    {
        private Controller.UIController _uic;

        private Datatype.CaliberUnit[] _possibleCalibers;

        // Are there any targets selected
        private bool _active;

        private Controller.UIController.Attributes _modifiedAttributes;

        /// <summary>
        /// Construct an instance of the form
        /// </summary>
        /// <param name="uic">The creating UIController</param>
        public Unified(Controller.UIController uic)
        {
            _uic = uic;

            InitializeComponent();

            SetEventHandlers();

            // Populate with 
            range_TargetDistanceUnits.Items.AddRange(new string[] { "yards", "meters" });
            range_TargetDistanceUnits.SelectedIndex = 0;

            range_Temperature.Items.AddRange(Datatype.ImageData.tempDetails);
            range_Temperature.SelectedIndex = 0;

            text_Height.Text = "1";
            text_Width.Text = "1";

            _active = false;

            // Disable elements of the UI as nothing is selected
            SetEnabled(false);
        }

        #region Event Handlers

        private void SetEventHandlers()
        {
            tabs.SelectedIndexChanged += new EventHandler(tabs_SelectedIndexChanged);

            // Global Toolbar
            button_Save.Click += new EventHandler(button_Save_Click);
            button_SaveAll.Click += new EventHandler(button_SaveAll_Click);
            button_GenerateReport.Click += new EventHandler(button_GenerateReport_Click);

            // Image Selection
            imageSelect.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(imageSelect_SelectionChange);

            // Details
            clearFields.Click += new EventHandler(clearFields_Click);
            resetFields.Click += new EventHandler(resetFields_Click);

            // Most of these are just to determine when the field has changed so
            // that _modifiedAttributes can be updated.
            general_Date.ValueChanged += new EventHandler(general_Date_ValueChanged);
            general_ShooterFirst.TextChanged += new EventHandler(general_ShooterFirst_TextChanged);
            general_ShooterLast.TextChanged += new EventHandler(general_ShooterLast_TextChanged);
            general_ShotsFired.TextChanged += new EventHandler(general_ShotsFired_TextChanged);
            general_ShotsFired.Validating += new CancelEventHandler(general_ShotsFired_Validate);
            weapon_Nomenclature.SelectedIndexChanged += new EventHandler(weapon_Nomenclature_SelectedIndexChanged);
            weapon_Notes.TextChanged += new EventHandler(weapon_Notes_TextChanged);
            weapon_SerialNumber.TextChanged += new EventHandler(weapon_SerialNumber_TextChanged);
            range_Place.TextChanged += new EventHandler(range_Place_TextChanged);
            range_TargetDistance.TextChanged += new EventHandler(range_TargetDistance_TextChanged);
            range_TargetDistance.Validating += new CancelEventHandler(range_TargetDistance_Validate);
            range_TargetDistanceUnits.SelectedIndexChanged += new EventHandler(range_TargetDistanceUnits_SelectedIndexChanged);
            range_Temperature.SelectedIndexChanged += new EventHandler(range_Temperature_SelectedIndexChanged);
            ammo_Caliber.TextChanged += new EventHandler(ammo_Caliber_TextChanged);
            ammo_Caliber.Validating += new CancelEventHandler(ammo_Caliber_Validate);
            ammo_CaliberUnit.SelectedIndexChanged += new EventHandler(ammo_CaliberUnit_SelectedIndexChanged);
            ammo_LotNumber.TextChanged += new EventHandler(ammo_LotNumber_TextChanged);
            ammo_Notes.TextChanged += new EventHandler(ammo_Notes_TextChanged);
            ammo_ProjectileMass.TextChanged += new EventHandler(ammo_ProjectileMass_TextChanged);
            ammo_ProjectileMass.Validating += new CancelEventHandler(ammo_ProjectileMass_Validate);

            // Measuring
            button_Perpendicular.Click += new EventHandler(Perpendicular_Click);
            text_Width.Validating += new CancelEventHandler(text_Width_Validate);
            text_Height.Validating += new CancelEventHandler(text_Height_Validate);

            // Bullet Holes
            button_AddHole.Click += new EventHandler(AddHole_Click);
            button_RemoveHole.Click += new EventHandler(RemoveHole_Click);
            button_Clear.Click += new EventHandler(Clear_Click);
            button_Reset.Click += new EventHandler(Reset_Click);
            button_FindHoles.Click += new EventHandler(button_FindHoles_Click);
            selectBox.Changed += new ChangedEventHandler(holeCount_Changed);
        }

        #region Global Toolbar

        void button_Save_Click(object sender, EventArgs e)
        {
            if (_active)
            {
                CommitData();
                if (_uic.ActiveValid())
                {
                    int[] temp = new int[imageSelect.SelectedIndices.Count];
                    imageSelect.SelectedIndices.CopyTo(temp, 0);
                    _uic.SaveData(temp);
                }
                else
                {
                    MessageBox.Show("There is invalid data for at least one image (highlighted in red).  Please correct it before trying to save.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    HighlightInvalidImages();
                }
            }
        }

        void button_SaveAll_Click(object sender, EventArgs e)
        {
            if (_uic.Valid.Length > 0)
            {
                CommitData();
                if (_uic.AllValid())
                {
                    // The for-loop below can be replaced with the following in .NET 3.0:
                    // int[] range = Enumerable.Range(0, imageSelect.Items.Count - 1);
                    int[] range = new int[imageSelect.Items.Count];
                    for (int i = 0; i < imageSelect.Items.Count; i++)
                    {
                        range[i] = i;
                    }
                    _uic.SaveData(range);
                }
                else
                {
                    MessageBox.Show("There is invalid data for at least one image (highlighted in red).  Please correct it before trying to save.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    HighlightInvalidImages();
                }
            }
        }

        void button_GenerateReport_Click(object sender, EventArgs e)
        {
            if (_active)
            {
                CommitData();
                if (_uic.ActiveValid())
                {
                    int[] temp = new int[imageSelect.SelectedIndices.Count];
                    imageSelect.SelectedIndices.CopyTo(temp, 0);
                    _uic.GenerateReport(temp);
                }
                else
                {
                    MessageBox.Show("There is invalid data for at least one image (highlighted in red).  Please correct it before trying to generate a report.", "Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    HighlightInvalidImages();
                }
            }
        }

        #endregion

        #region Image Selection

        void imageSelect_SelectionChange(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // If there is a previous selection, go ahead and commit the data then
            // empty the fields so they can be repopulated if necessary.
            if (_active)
            {
                CommitData();

                HighlightInvalidImages();

                EraseFields();
            }

            // Make sure there is a selection.
            if (imageSelect.SelectedItems.Count > 0)
            {
                _active = true;
                button_Save.Enabled = true;
                button_GenerateReport.Enabled = true;

                int[] selectedIndices = new int[imageSelect.SelectedIndices.Count];
                imageSelect.SelectedIndices.CopyTo(selectedIndices, 0);
                PopulateFields(_uic.GetImageData(selectedIndices));
                SetEnabled(true);

                _modifiedAttributes = UIController.Attributes.None;

                if (imageSelect.SelectedIndices.Count > 1)
                {
                    measureBox.SetMultipleFileMode(true);
                    measureBox.Enabled = false;
                    selectBox.SetMultipleFileMode(true);
                    selectBox.Enabled = false;

                    currentImageLabel.Text = "Current: Multiple";
                }
                else
                {
                    measureBox.SetMultipleFileMode(false);
                    measureBox.Enabled = true;
                    selectBox.SetMultipleFileMode(false);
                    selectBox.Enabled = true;

                    // Lengths are originally zero and need to be "initialized".
                    // Zero lengths also mess up scaling.
                    if (_uic.CurrentData.scale.verticalLength == 0)
                        _uic.CurrentData.scale.verticalLength = 1;
                    text_Height.Text = Convert.ToString(_uic.CurrentData.scale.verticalLength);

                    if (_uic.CurrentData.scale.horizontalLength == 0)
                        _uic.CurrentData.scale.horizontalLength = 1;
                    text_Width.Text = Convert.ToString(_uic.CurrentData.scale.horizontalLength);

                    Bitmap image = _uic.CurrentData.bitmap;

                    /***** Set values for MeasureBox *****/
                    measureBox.SetImage(image, true);

                    // If the top left point is the same as the bottom right (Points
                    // are initialized to (0,0))
                    if (_uic.CurrentData.regionOfInterest.topLeft.Equals(_uic.CurrentData.regionOfInterest.bottomRight))
                    {
                        // Change the region of interest to be the whole image
                        Datatype.ROI roi;
                        roi.topLeft = new Point();
                        roi.bottomRight = new Point(image.Width, image.Height);
                        measureBox.RegionOfInterest = roi;
                    }
                    else
                        measureBox.RegionOfInterest = _uic.CurrentData.regionOfInterest;

                    // If the two scaling points are the same (Points are initialized
                    // to (0,0))
                    if (_uic.CurrentData.scale.horizontal == _uic.CurrentData.scale.vertical &&
                        _uic.CurrentData.scale.horizontal == _uic.CurrentData.scale.middle)
                        // give it the default scale indicator
                        measureBox.ResetRules();
                    else
                        measureBox.Scaling = _uic.CurrentData.scale;

                    measureBox.Refresh();

                    /***** Set values for SelectBox *****/
                    selectBox.SetImage(image, true);
                    selectBox.BulletHoles = (List<Point>)_uic.CurrentData.points;

                    selectBox.Refresh();

                    // Change the label to indicate which image is selected
                    currentImageLabel.Text = String.Format("Current: {0}", imageSelect.Items[imageSelect.SelectedIndices[0]].Text);
                }
                
                // After populating the fields, validate them so the user knows
                // which are invalid.
                general_ShotsFired_Validate(this, EventArgs.Empty);
                range_TargetDistance_Validate(this, EventArgs.Empty);
                ammo_Caliber_Validate(this, EventArgs.Empty);
                ammo_ProjectileMass_Validate(this, EventArgs.Empty);
                text_Width_Validate(this, EventArgs.Empty);
                text_Height_Validate(this, EventArgs.Empty);
            }
            else
            {
                // Nothing is selected so disable most everything
                _active = false;
                button_Save.Enabled = false;
                menu_Save.Enabled = false;
                button_GenerateReport.Enabled = false;
                menu_GenerateReport.Enabled = false;
                SetEnabled(true, false);
                currentImageLabel.Text = String.Format("Current: None");
            }
        }

        #endregion

        #region Details

        void clearFields_Click(object sender, EventArgs e)
        {
            EraseFields();
        }

        void resetFields_Click(object sender, EventArgs e)
        {
            PopulateFields(_uic.CurrentData);
        }

        void general_Date_ValueChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.DateTimeFired;
        }

        void general_ShooterFirst_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.ShooterFirstName;
        }

        void general_ShooterLast_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.ShooterLastName;
        }

        void general_ShotsFired_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.TotalShotsFired;
        }

        void general_ShotsFired_Validate(object sender, EventArgs e)
        {
            try
            {
                int shotsFired = Convert.ToInt32(general_ShotsFired.Text);
                if (shotsFired <= 0)
                {
                    errorProvider.SetError(general_ShotsFired, "Shots fired cannot be zero.");
                }
                else
                {
                    errorProvider.SetError(general_ShotsFired, "");
                }
            }
            catch (Exception)
            {
                errorProvider.SetError(general_ShotsFired, "Value must be an integer.");
            }
        }

        void weapon_Nomenclature_SelectedIndexChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.WeaponName;
        }

        void weapon_Notes_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.WeaponNotes;
        }

        void weapon_SerialNumber_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.WeaponSerial;
        }

        void range_Place_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.RangeLocation;
        }

        void range_TargetDistance_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.RangeDistance;
        }

        void range_TargetDistance_Validate(object sender, EventArgs e)
        {
            try
            {
                int targetDistance = Convert.ToInt32(range_TargetDistance.Text);
                if (targetDistance <= 0)
                {
                    errorProvider.SetError(range_TargetDistance, "Distance cannot be zero.");
                }
                else
                {
                    errorProvider.SetError(range_TargetDistance, "");
                }
            }
            catch (Exception)
            {
                errorProvider.SetError(range_TargetDistance, "Value must be an integer.");
            }
        }

        void range_TargetDistanceUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.RangeDistanceUnits;
        }

        void range_Temperature_SelectedIndexChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.RangeTemperature;
        }

        void ammo_Caliber_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.AmmoCaliber;
        }

        void ammo_Caliber_Validate(object sender, EventArgs e)
        {
            try
            {
                double ammoCaliber = Convert.ToDouble(ammo_Caliber.Text);
                if (ammoCaliber <= 0)
                {
                    errorProvider.SetError(ammo_Caliber, "Caliber cannot be zero.");
                }
                else
                {
                    errorProvider.SetError(ammo_Caliber, "");
                }
            }
            catch (Exception)
            {
                errorProvider.SetError(ammo_Caliber, "Value must be an decimal number.");
            }
        }

        void ammo_CaliberUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.AmmoCaliberUnits;
        }

        void ammo_LotNumber_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.AmmoLotNumber;
        }

        void ammo_Notes_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.AmmoNotes;
        }

        void ammo_ProjectileMass_TextChanged(object sender, EventArgs e)
        {
            _modifiedAttributes |= UIController.Attributes.AmmoMass;
        }

        void ammo_ProjectileMass_Validate(object sender, EventArgs e)
        {
            try
            {
                int projectileMass = Convert.ToInt32(ammo_ProjectileMass.Text);
                if (projectileMass <= 0)
                {
                    errorProvider.SetError(ammo_ProjectileMass, "Mass cannot be zero.");
                }
                else
                {
                    errorProvider.SetError(ammo_ProjectileMass, "");
                }
            }
            catch (Exception)
            {
                errorProvider.SetError(ammo_ProjectileMass, "Value must be an integer.");
            }
        }

        #endregion

        #region Measuring

        void Perpendicular_Click(object sender, EventArgs e)
        {
            measureBox.PerpendicularScale = button_Perpendicular.Checked;
        }

        void text_Height_Validate(object sender, EventArgs e)
        {
            try
            {
                double height = Convert.ToDouble(text_Height.Text);
                if (height <= 0)
                {
                    text_Height.BackColor = Color.Red;
                    text_Height.ForeColor = Color.White;
                }
                else
                {
                    text_Height.BackColor = Color.White;
                    text_Height.ForeColor = Color.Black;
                }
            }
            catch (Exception)
            {
                text_Height.BackColor = Color.Red;
                text_Height.ForeColor = Color.White;
            }
        }

        void text_Width_Validate(object sender, EventArgs e)
        {
            try
            {
                double width = Convert.ToDouble(text_Width.Text);
                if (width <= 0)
                {
                    text_Width.BackColor = Color.Red;
                    text_Width.ForeColor = Color.White;
                }
                else
                {
                    text_Width.BackColor = Color.White;
                    text_Width.ForeColor = Color.Black;
                }
            }
            catch (Exception)
            {
                text_Width.BackColor = Color.Red;
                text_Width.ForeColor = Color.White;
            }
        }

        #endregion

        #region Bullet Holes

        void AddHole_Click(object sender, EventArgs e)
        {
            selectBox.CurrentTool = SelectBox.Tool.AddSelector;
            button_AddHole.Checked = true;
            button_RemoveHole.Checked = false;
        }

        void RemoveHole_Click(object sender, EventArgs e)
        {
            selectBox.CurrentTool = SelectBox.Tool.RemoveSelector;
            button_AddHole.Checked = false;
            button_RemoveHole.Checked = true;
        }

        void Clear_Click(object sender, EventArgs e)
        {
            selectBox.BulletHoles = new List<Point>();
            selectBox.Refresh();
        }

        void Reset_Click(object sender, EventArgs e)
        {
            selectBox.BulletHoles = new List<Point>(_uic.CurrentData.points);
            selectBox.Refresh();
        }

        void button_FindHoles_Click(object sender, EventArgs e)
        {
            if (selectBox.BulletHoles.Count != 0)
            {
                DialogResult result = MessageBox.Show("There are already bullet holes marked; using image recognition will replace all of these.  Do you want to run image recognition?", "Run image recognition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    selectBox.BulletHoles = _uic.ImageRecResults();
                    selectBox.Refresh();
                }
            }
            else
            {
                selectBox.BulletHoles = _uic.ImageRecResults();
                selectBox.Refresh();
            }
        }

        void holeCount_Changed(object sender, EventArgs e)
        {
            if (selectBox.BulletHoles.Count > ShotsFired)
            {
                label_HolesIdentified.ForeColor = Color.Red;
                label_HolesIdentified.Font = new Font(label_HolesIdentified.Font, FontStyle.Bold);
            }
            else
            {
                label_HolesIdentified.ForeColor = Color.Black;
                label_HolesIdentified.Font = new Font(label_HolesIdentified.Font, FontStyle.Regular);
            }
            label_HolesIdentified.Text = String.Format("{0} of {1} holes", new object[] { selectBox.BulletHoles.Count, ShotsFired });
        }

        #endregion

        private void menu_About_Click(object sender, EventArgs e)
        {
            About about = new About();

            about.ShowDialog();
        }

        private void SelectSingleImage(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "All Supported Images|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _uic.SetFileNames(ofd.FileNames);

                SetEnabled(true, false);
            }
        }

        private void SelectFolder(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
                string[] types = { "*.jpg", "*.jpeg", "*.png" };

                System.Collections.Generic.List<string> filenames = new System.Collections.Generic.List<string>();

                foreach (string type in types)
                {
                    FileInfo[] files = di.GetFiles(type);
                    foreach (FileInfo file in files)
                    {
                        filenames.Add(file.FullName);
                    }
                }
                if (filenames.Count > 0)
                {
                    _uic.SetFileNames(filenames.ToArray());

                    SetEnabled(true, false);
                }
                else
                {
                    MessageBox.Show("No images were found.", "No images found.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedTab == tab_Holes)
            {
                if (!ValidateScale())
                {
                    MessageBox.Show("The scale (width and height) must be a positive decimal numbers.", "Invalid scale", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabs.SelectTab(tab_Scale);
                }
                else if (Caliber <= 0)
                {
                    MessageBox.Show("The caliber must be a positive decimal number.", "Invalid caliber", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabs.SelectTab(tab_Details);
                    ammo_Caliber.Focus();
                    ammo_Caliber.SelectAll();
                }
                else if (ShotsFired <= 0)
                {
                    MessageBox.Show("The number of shots fired must be a positive integer.", "Invalid number of shots fired", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabs.SelectTab(tab_Details);
                    general_ShotsFired.Focus();
                    general_ShotsFired.SelectAll();
                }
                else
                {
                    _uic.UpdateScale(imageSelect.SelectedIndices[0], measureBox.Scaling, Caliber, CaliberUnit, (float)Convert.ToDouble(text_Height.Text), (float)Convert.ToDouble(text_Width.Text), measureBox.RegionOfInterest);
                    selectBox.HoleDiameter = _uic.GetHoleDiameter(imageSelect.SelectedIndices[0]);
                    holeCount_Changed(this, EventArgs.Empty);                    
                }
            }
        }

        #endregion

        private void PopulateFields(Datatype.ImageData imageData)
        {
            if (imageData == null)
            {
                EraseFields();
            }
            else
            {
                // General Information
                DateFired = imageData.dateTimeFired;
                ShooterFirstName = (imageData.shooterFName == null ? "" : imageData.shooterFName);
                ShooterLastName = (imageData.shooterLName == null ? "" : imageData.shooterLName);
                ShotsFired = imageData.shotsFired;

                // Range Information
                Place = (imageData.rangeLocation == null ? "" : imageData.rangeLocation);
                Distance = imageData.distance;
                DistanceUnits = imageData.distanceUnits;
                Temperature = imageData.temperature;

                // Weapon Information
                Nomenclature = (imageData.weaponName == null ? "" : imageData.weaponName);
                SerialNumber = (imageData.serialNumber == null ? "" : imageData.serialNumber);
                WeaponNotes = (imageData.weaponNotes == null ? "" : imageData.weaponNotes);

                // Ammunition Information
                Caliber = imageData.caliberValue;
                if (imageData.caliber == null)
                {
                    ammo_CaliberUnit.SelectedIndex = 0;
                }
                else
                {
                    for (int c = 0; c < _possibleCalibers.Length; c++)
                    {
                        if (imageData.caliber.caliberUnitID == _possibleCalibers[c].caliberUnitID)
                        {
                            ammo_CaliberUnit.SelectedIndex = c;
                            break;
                        }
                    }
                }
                LotNumber = (imageData.lotNumber == null ? "" : imageData.lotNumber);
                Mass = imageData.projectileMassGrains;
                AmmoNotes = (imageData.ammunitionNotes == null ? "" : imageData.ammunitionNotes);
            }
        }

        private void EraseFields()
        {
            // General Information
            DateFired = _uic.DummyDate;
            ShooterFirstName = "";
            ShooterLastName = "";
            general_ShotsFired.Text = "";

            // Range Information
            Place = "";
            range_TargetDistance.Text = "";
            range_TargetDistanceUnits.SelectedIndex = 0;
            range_Temperature.SelectedIndex = 0;

            // Weapon Information
            Nomenclature = "";
            SerialNumber = "";
            WeaponNotes = "";

            // Ammunition Information
            ammo_Caliber.Text = "";
            ammo_CaliberUnit.SelectedIndex = 0;
            LotNumber = "";
            ammo_ProjectileMass.Text = "";
            AmmoNotes = "";
        }

        internal void SetEnabled(bool main, bool tabby)
        {
            imageSelect.Enabled = main;
            tabs.Enabled = tabby;
            tools_Global.Enabled = main;
            menu_Save.Enabled = main;
            menu_SaveAll.Enabled = main;
            menu_GenerateReport.Enabled = main;
        }

        internal void SetEnabled(bool state)
        {
            SetEnabled(state, state);
        }

        /// <summary>
        /// Change the image names to those provided.
        /// </summary>
        /// <param name="imagePaths">Names for new images to display</param>
        public void SetImages(string[] imagePaths)
        {
            _active = false;

            imageSelect.SelectedIndices.Clear();

            // Clear out the old image names
            imageSelect.Items.Clear();

            // and replace them with the new ones
            foreach (string path in imagePaths)
            {
                imageSelect.Items.Add(path.Substring(path.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1));
            }

            button_Save.Enabled = false;
            menu_Save.Enabled = false;
            button_GenerateReport.Enabled = false;
            menu_GenerateReport.Enabled = false;

            tabs.SelectedTab = tab_Details;

            if (imagePaths.Length == 0)
            {
                MessageBox.Show("No images were loaded.", "No images", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Sets the caliber units displayed on the details tab.
        /// </summary>
        /// <param name="calibers">The caliber units to display.</param>
        public void SetCalibers(Datatype.CaliberUnit[] calibers)
        {
            _possibleCalibers = calibers;

            ammo_CaliberUnit.Items.Clear();

            foreach (Datatype.CaliberUnit c in _possibleCalibers)
            {
                ammo_CaliberUnit.Items.Add(c.unitName);
            }

            ammo_CaliberUnit.SelectedIndex = 0;
        }

        private void OpenSettings(object sender, EventArgs e)
        {
            _uic.OpenSettings();
        }

        /// <summary>
        /// Give the updated details to the UIController to pass to the master.
        /// This will not cause the information to be saved to the database, but
        /// most be done before doing so.
        /// </summary>
        private void CommitData()
        {
            Datatype.ImageData updatedData = new Datatype.ImageData(
                null,
                null,
                DateFired,
                ShooterLastName,
                ShooterFirstName,
                Place,
                DistanceUnits,
                Distance,
                Temperature,
                Nomenclature,
                SerialNumber,
                WeaponNotes,
                CaliberUnit,
                Caliber,
                LotNumber,
                Mass,
                AmmoNotes,
                Scaling,
                ShotsFired,
                null);
            if (_uic.Active.Length == 1)
            {
                updatedData.origFilename = _uic.CurrentData.origFilename;
                updatedData.reportFilename = _uic.CurrentData.reportFilename;
                updatedData.points = _uic.CurrentData.points;
                updatedData.regionOfInterest = measureBox.RegionOfInterest;
                updatedData.scale = measureBox.Scaling;
                updatedData.scale.horizontalLength = (float)Convert.ToDouble(text_Width.Text);
                updatedData.scale.verticalLength = (float)Convert.ToDouble(text_Height.Text);
                updatedData.points = selectBox.BulletHoles;
                updatedData.bitmap = _uic.CurrentData.bitmap;
            }
            _uic.SetData(updatedData, _modifiedAttributes);
        }

        /// <summary>
        /// Change the weapon list to the one provided.
        /// </summary>
        /// <param name="weaponNames">The new weapons to display.</param>
        internal void UpdateWeapons(List<String> weaponNames)
        {
            weapon_Nomenclature.Items.Clear();
            weapon_Nomenclature.Items.AddRange(weaponNames.ToArray());

            weapon_Nomenclature.SelectedIndex = 0;
        }

        private void LoadImages(object sender, EventArgs e)
        {
            _uic.LoadImages();
        }

        private bool ValidateScale()
        {
            try
            {
                return (Convert.ToDouble(text_Height.Text) > 0) &&
                    (Convert.ToDouble(text_Width.Text) > 0);
            }
            catch
            {
                return false;
            }
        }

        private void HighlightInvalidImages()
        {
            for (int i = 0; i < imageSelect.Items.Count; i++)
            {
                imageSelect.Items[i].ForeColor = _uic.Valid[i] ? Color.Black : Color.Red;
            }
        }

        #region Details

        #region General

        /// <summary>
        /// Sets or gets the value of the date field
        /// </summary>
        public DateTime DateFired
        {
            get { return general_Date.Value; }
            set { general_Date.Value = value; }
        }

        /// <summary>
        /// Sets or gets the shooter first name text field
        /// </summary>
        public String ShooterFirstName
        {
            get { return general_ShooterFirst.Text; }
            set { general_ShooterFirst.Text = value; }
        }

        /// <summary>
        /// Sets or gets the shooter last name text field
        /// </summary>
        public String ShooterLastName
        {
            get { return general_ShooterLast.Text; }
            set { general_ShooterLast.Text = value; }
        }

        /// <summary>
        /// Sets or gets the shots fired text field
        /// </summary>
        public int ShotsFired
        {
            get
            {
                try { return Convert.ToInt32(general_ShotsFired.Text); }
                catch { return 0; }
            }
            set { general_ShotsFired.Text = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets the scaling from the MeasureBox
        /// </summary>
        public Datatype.Scale Scaling
        {
            get { return measureBox.Scaling; }
        }


        #endregion

        #region Range

        /// <summary>
        /// Sets or gets the range location text field
        /// </summary>
        public String Place
        {
            get { return range_Place.Text; }
            set { range_Place.Text = value; }
        }

        /// <summary>
        /// Sets or gets the range distance text field
        /// </summary>
        public int Distance
        {
            get
            {
                try { return Convert.ToInt32(range_TargetDistance.Text); }
                catch { return 0; }
            }
            set { range_TargetDistance.Text = Convert.ToString(value); }
        }

        /// <summary>
        /// Sets or gets the units for the range distance
        /// </summary>
        public Datatype.UnitsOfMeasure DistanceUnits
        {
            get
            {
                switch (range_TargetDistanceUnits.SelectedIndex)
                {
                    default:
                        throw new ArgumentOutOfRangeException();
                    case 0:
                        return Datatype.UnitsOfMeasure.Yard;
                    case 1:
                        return Datatype.UnitsOfMeasure.Meter;
                }
            }

            set
            {
                switch (value)
                {
                    case Datatype.UnitsOfMeasure.Yard:
                        range_TargetDistanceUnits.SelectedIndex = 0;
                        break;
                    case Datatype.UnitsOfMeasure.Meter:
                        range_TargetDistanceUnits.SelectedIndex = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// Sets or gets the range temperature combo box
        /// </summary>
        public Datatype.ImageData.Temperature Temperature
        {
            get
            {
                switch (range_Temperature.SelectedIndex)
                {
                    default:
                        throw new ArgumentOutOfRangeException();
                    case 0:
                        return Datatype.ImageData.Temperature.Cold;
                    case 1:
                        return Datatype.ImageData.Temperature.Ambient;
                    case 2:
                        return Datatype.ImageData.Temperature.Hot;
                }
            }
            set { range_Temperature.SelectedIndex = Convert.ToInt32(value); }
        }

        #endregion

        #region Weapon

        /// <summary>
        /// Sets or gets the weapon nomenclature text field
        /// </summary>
        public String Nomenclature
        {
            get { return weapon_Nomenclature.Text; }
            set { weapon_Nomenclature.Text = value; }
        }

        /// <summary>
        /// Sets or gets the weapon serial number text field
        /// </summary>
        public String SerialNumber
        {
            get { return weapon_SerialNumber.Text; }
            set { weapon_SerialNumber.Text = value; }
        }

        /// <summary>
        /// Sets or gets the weapon notes text box
        /// </summary>
        public String WeaponNotes
        {
            get { return weapon_Notes.Text; }
            set { weapon_Notes.Text = value; }
        }

        #endregion

        #region Ammuntion

        /// <summary>
        /// Sets or gets the ammunition caliber text field
        /// </summary>
        public double Caliber
        {
            get
            {
                try { return Convert.ToDouble(ammo_Caliber.Text); }
                catch { return 0; }
            }
            set { ammo_Caliber.Text = Convert.ToString(value); }
        }

        /// <summary>
        /// Get the units for the ammunition caliber
        /// </summary>
        public Datatype.CaliberUnit CaliberUnit
        {
            get { return _possibleCalibers[ammo_CaliberUnit.SelectedIndex]; }
        }

        /// <summary>
        /// Sets or gets the ammunition lot number text field
        /// </summary>
        public String LotNumber
        {
            get { return ammo_LotNumber.Text; }
            set { ammo_LotNumber.Text = value; }
        }

        /// <summary>
        /// Sets or gets the ammunition mass text field
        /// </summary>
        public int Mass
        {
            get
            {
                try { return Convert.ToInt32(ammo_ProjectileMass.Text); }
                catch { return 0; }
            }
            set { ammo_ProjectileMass.Text = Convert.ToString(value); }
        }

        /// <summary>
        /// Sets or gets the ammunition notes text field
        /// </summary>
        public String AmmoNotes
        {
            get { return ammo_Notes.Text; }
            set { ammo_Notes.Text = value; }
        }

        #endregion

        #endregion
    }
}
