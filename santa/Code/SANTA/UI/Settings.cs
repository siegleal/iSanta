using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SANTA.IO;
using System.IO;

namespace SANTA.UI
{
    /// <summary>
    /// A form to set configuration values.
    /// </summary>
    public partial class Settings : Form
    {
        private Controller.UIController _uic;

        private ConfigReader _reader;

        private String[] settings;

        private List<TextBox> textBoxes;

        private List<String> _weapons, _addedWeapons;

        private bool firstRun = false;

        /// <summary>
        /// Construct a <code>Settings</code> form.
        /// </summary>
        /// <param name="uic">Calling UIController</param>
        /// <param name="configReader">A configuration reader</param>
        /// <param name="weapons">The current weapon list</param>
        public Settings(Controller.UIController uic, ConfigReader configReader, List<String> weapons)
        {
            InitializeComponent();

            _uic = uic;
            _reader = configReader;
            _weapons = weapons;

            InitializeValues();

            this.Closing += new CancelEventHandler(Settings_Closing);
        }

        /// <summary>
        /// Construct a <code>Settings</code> form with no weapon list
        /// </summary>
        /// <param name="uic">Calling UIController</param>
        /// <param name="configReader">A configuration reader</param>
        public Settings(Controller.UIController uic, ConfigReader configReader) : this(uic, configReader, new List<string>())
        {
            MessageBox.Show("This is the first time you have run SANTA.  Please specify the paths for the report template, database, and log file.", "First run", MessageBoxButtons.OK, MessageBoxIcon.Information);

            firstRun = true;
        }

        void Settings_Closing(object sender, CancelEventArgs e)
        {
            CloseAndSignalExit();
        }

        void CloseAndSignalExit()
        {
            if (!ValidateSettings())
            {
                DialogResult close = MessageBox.Show("You can close the settings but because this is the first run you will not be able to run the program until the settings are set.  Do you want to close the settings?", "Close settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (close == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void InitializeValues()
        {
            settings = new string[]
                                    {
                                        "Template Location", "Database Location", "Log Location",
                                        "Shooter Name", "Shoot Date", "Range Name", "Temperature", "Target Distance",
                                        "Shots Fired", "Weapon Name", "Weapon Serial Number", "Ammo Caliber",
                                        "Ammo Lot Number", "Ammo Mass", "Weapon Notes", "Ammo Notes", "Extreme Spread X",
                                        "Extreme Spread Y", "Extreme Spread", "Mean Radius", "Sigma X", "Sigma Y",
                                        "Furthest Left", "Furthest Right", "Highest Round", "Lowest Round",
                                        "Cell To Determine Unit of Measure", "Cell To Determine Unit of Points", "X Offset"
                                        , "Y Offset", "CEP Radius", "Target ID", "Extreme Spread x1", "Extreme Spread x2",
                                        "Extreme Spread y1", "Extreme Spread y2", "Date Format"
                                    };
            textBoxes = new List<TextBox>
                                          {
                                              txtReportLocation,
                                              txtDBLocation,
                                              txtLogFile,
                                              txtShooterName,
                                              txtShootDate,
                                              txtRangeName,
                                              txtTemperature,
                                              txtTargetDistance,
                                              txtShotsFired,
                                              txtWeaponName,
                                              txtWeaponSerialNumber,
                                              txtAmmoCaliber,
                                              txtAmmoLotNumber,
                                              txtAmmoMass,
                                              txtWeaponNotes,
                                              txtAmmoNotes,
                                              txtExtremeSpreadX,
                                              txtExtremeSpreadY,
                                              txtExtremeSpread,
                                              txtMeanRadius,
                                              txtSigmaX,
                                              txtSigmaY,
                                              txtFurthestLeft,
                                              txtFurthestRight,
                                              txtHighestRound,
                                              txtLowestRound,
                                              txtUnitMeasure,
                                              txtUnitPoints,
                                              txtXOffset,
                                              txtYOffset,
                                              txtCEPRadius,
                                              txtTargetID,
                                              txtExtremeSpreadX1,
                                              txtExtremeSpreadX2,
                                              txtExtremeSpreadY1,
                                              txtExtremeSpreadY2,
                                              txtDateFormat
                                          };
            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (settings[i].Equals("Database Location"))
                {
                    textBoxes[i].Text = _reader.getValue(settings[i]).Substring(45);
                }
                else
                {
                    textBoxes[i].Text = _reader.getValue(settings[i]);
                }
            }

            //Excel Value Setting
            string excel = _reader.getValue("Open Excel By Default");
            switch (excel)
            {
                case "nothing":
                    reportOpenBehavior.SelectedIndex = 0;
                    break;
                case "prompt":
                    reportOpenBehavior.SelectedIndex = 1;
                    break;
                case "open":
                    reportOpenBehavior.SelectedIndex = 2;
                    break;
                default:
                    reportOpenBehavior.SelectedIndex = 0;
                    break;
            }

            existingWeapons.Items.Clear();

            foreach (String w in _weapons)
            {
                existingWeapons.Items.Add(new ListViewItem(w));
            }
            
            _addedWeapons = new List<String>();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeValues();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings())
            {
                return;
            }

            string excelSetting;
            //Excel Value Setting
            switch (reportOpenBehavior.SelectedIndex)
            {
                case 0:
                    excelSetting = "nothing";
                    break;
                case 1:
                    excelSetting = "prompt";
                    break;
                case 2:
                    excelSetting = "open";
                    break;
                default:
                    excelSetting = "nothing";
                    break;
            }

            _reader.setValue("Open Excel By Default", excelSetting);

            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (settings[i].Equals("Database Location"))
                {
                    _reader.setValue(settings[i], "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxes[i].Text);
                }
                else
                {
                    _reader.setValue(settings[i], textBoxes[i].Text);
                }
            }

            _uic.SaveAddedWeapons(_addedWeapons);

            _weapons.AddRange(_addedWeapons);

            MessageBox.Show("The settings were saved successfully.", "Successful save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateSettings()
        {
            String errors = "";

            if (!File.Exists(txtReportLocation.Text))
            {
                errors += "\nReport template could not be found.";
            }

            if (!File.Exists(txtDBLocation.Text))
            {
                errors += "\nDatabase could not be found.";
            }

            if (!File.Exists(txtLogFile.Text))
            {
                errors += "\nLog file could not be found.";
            }

            if (errors != "")
            {
                MessageBox.Show("The following errors were found:\n" + errors, "Errors found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!firstRun)
            {
                Close();
            }
            else
            {
                CloseAndSignalExit();
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            String trimmedNom = add_Nomenclature.Text.Trim();
            if (trimmedNom.Length == 0)
            {
                MessageBox.Show("Please enter a name that contains more than just white-space characters.", "Invalid name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                add_Nomenclature.SelectAll();
            }
            else if (_weapons.Contains(trimmedNom) || _addedWeapons.Contains(trimmedNom))
            {
                MessageBox.Show("That weapon already exists.", "Weapon exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                add_Nomenclature.SelectAll();
            }
            else
            {
                _addedWeapons.Add(trimmedNom);
                existingWeapons.Items.Add(new ListViewItem(trimmedNom));
                add_Nomenclature.Clear();
                add_Nomenclature.Focus();
            }
        }

        private void btnDBLocation_Click(object sender, EventArgs e)
        {
            OpenFileDialog location = new OpenFileDialog();
            location.Filter = "Access Database (*.mdb)|*.mdb";
            if (location.ShowDialog() == DialogResult.OK)
            {
                txtDBLocation.Text = location.FileName;
            }
        }

        private void btnLogLocation_Click(object sender, EventArgs e)
        {
            OpenFileDialog location = new OpenFileDialog();
            location.Filter = "Text Log File (*.txt)|*.txt";
            if (location.ShowDialog() == DialogResult.OK)
            {
                txtLogFile.Text = location.FileName;
            }
        }

        private void btnExcelTemplateLocation_Click(object sender, EventArgs e)
        {
            OpenFileDialog location = new OpenFileDialog();
            location.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            if (location.ShowDialog() == DialogResult.OK)
            {
                txtReportLocation.Text = location.FileName;
            }
        }
    }
}
