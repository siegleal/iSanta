using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace SANTA.Controller
{
    /// <summary>
    /// The <code>UIController</code> class is responsible for handling user interaction and the UI.
    /// </summary>
    public class UIController
    {
        private Master _master;

        private UI.Unified _unified;

        private int[] _active;

        private bool[] _valid;

        /// <summary>
        /// A dummy (impossible) date to fill the target detail date field with.
        /// </summary>
        public DateTime DummyDate
        {
            get { return new DateTime(1900, 1, 1); }
        }

        /// <summary>
        /// The associated <code>ImageData</code> for the current selection.
        /// </summary>
        public Datatype.ImageData CurrentData { get; set; }

        /// <summary>
        /// A list of indices of the currently selcted items.
        /// </summary>
        public int[] Active { get { return _active; } }

        /// <summary>
        /// A list indicating which of all items have successfully validated.
        /// </summary>
        public bool[] Valid { get { return _valid; } }

        /// <summary>
        /// The current weapon names from the <code>Master</code>.
        /// </summary>
        public List<String> WeaponNames
        {
            get { return _master.GetWeaponNames(); }
        }

        /// <summary>
        /// Constructs a <code>UIController</code> instance and initializes a
        /// <code>Unified</code>  form.
        /// </summary>
        /// <param name="master">The <code>Master</code> class instance to communicate with.</param>
        /// <param name="firstUse">If this is the first time the application has been run.</param>
		public UIController(Master master, bool firstUse)
        {

            _master = master;

            _active = new int[0];

            // If this is the first run, open the settings to get paths required
            // for the program to run
            if (firstUse)
            {
                OpenSettings(true);
            }

            _unified = new UI.Unified(this);
            
            _unified.UpdateWeapons(WeaponNames);

            _unified.SetCalibers(_master.GetCaliberUnits().ToArray());

            Application.Run(_unified);
        }

        /// <summary>
        /// Changes to the set of active files (targets) to those specified.
        /// </summary>
        /// <param name="FileNames">The filenames for the new set of files.</param>
        public void SetFileNames(string[] FileNames)
        {
            _master.SetFileNames(FileNames);

            _unified.SetImages(_master.GetFileNames());

            _valid = new bool[_master.GetFileNames().Length];
        }

        /// <summary>
        /// Gets the <code>ImageData</code> for the current selection of images.
        /// If the selection contains multiple images and they have differing
        /// values, the common values are set and the rest of null (or otherwise
        /// equivalent) value.
        /// </summary>
        /// <param name="images">A list of image indices (typically the current selection).</param>
        /// <returns>The associated <code>ImageData</code> for the set of images.</returns>
        public Datatype.ImageData GetImageData(int[] images)
        {
            _active = images;
            // If there is only one image selected
            if (images.Length == 1)
            {
                CurrentData = _master.GetImageData(images[0]);
                return CurrentData;
            }
            else
            {
                // commonAttributes describes the common attributes of all images selected
                Attributes commonAttributes = Attributes.DateTimeFired | Attributes.ShooterFirstName
                    | Attributes.ShooterLastName | Attributes.TotalShotsFired | Attributes.RangeLocation
                    | Attributes.RangeDistance | Attributes.RangeDistanceUnits | Attributes.RangeTemperature
                    | Attributes.WeaponName | Attributes.WeaponSerial | Attributes.WeaponNotes
                    | Attributes.AmmoCaliber | Attributes.AmmoCaliberUnits | Attributes.AmmoLotNumber
                    | Attributes.AmmoMass | Attributes.AmmoNotes;

                Datatype.ImageData first = null;
                foreach (int image in images)
                {
                    Datatype.ImageData current = _master.GetImageData(image);
                    if (first == null)
                    {
                        first = current;
                        continue;
                    }

                    // Compare each attribute to the first element.  If that does
                    // not match  the first element, remove that from commonAttributes
                    // because we don't want that to be part of the return value.
                    if (DateTime.Compare(first.dateTimeFired, current.dateTimeFired) != 0)
                                                                      commonAttributes &= ~Attributes.DateTimeFired;
                    if (first.shooterFName != current.shooterFName)   commonAttributes &= ~Attributes.ShooterFirstName;
                    if (first.shooterLName != current.shooterLName)   commonAttributes &= ~Attributes.ShooterLastName;
                    if (first.shotsFired != current.shotsFired)       commonAttributes &= ~Attributes.TotalShotsFired;
                    if (first.weaponName != current.weaponName)       commonAttributes &= ~Attributes.WeaponName;
                    if (first.serialNumber != current.serialNumber)   commonAttributes &= ~Attributes.WeaponSerial;
                    if (first.weaponNotes != current.weaponNotes)     commonAttributes &= ~Attributes.WeaponNotes;
                    if (first.rangeLocation != current.rangeLocation) commonAttributes &= ~Attributes.RangeLocation;
                    if (first.distance != current.distance)           commonAttributes &= ~Attributes.RangeDistance;
                    if (first.distanceUnits != current.distanceUnits) commonAttributes &= ~Attributes.RangeDistanceUnits;
                    if (first.temperature != current.temperature)     commonAttributes &= ~Attributes.RangeTemperature;
                    if (first.caliberValue != current.caliberValue)   commonAttributes &= ~Attributes.AmmoCaliber;
                    if (first.caliber != current.caliber)             commonAttributes &= ~Attributes.AmmoCaliberUnits;
                    if (first.lotNumber != current.lotNumber)         commonAttributes &= ~Attributes.AmmoLotNumber;
                    if (first.projectileMassGrains != current.projectileMassGrains)
                                                                      commonAttributes &= ~Attributes.AmmoMass;
                    if (first.ammunitionNotes != current.ammunitionNotes)
                                                                      commonAttributes &= ~Attributes.AmmoNotes;
                }

                // Create and return an ImageData instance with the only common values set.
                Datatype.ImageData common = new Datatype.ImageData();
                if ((commonAttributes & Attributes.DateTimeFired) != 0)      common.dateTimeFired = first.dateTimeFired;
                else common.dateTimeFired = DummyDate;
                if ((commonAttributes & Attributes.ShooterFirstName) != 0)   common.shooterFName = first.shooterFName;
                if ((commonAttributes & Attributes.ShooterLastName) != 0)    common.shooterLName = first.shooterLName;
                if ((commonAttributes & Attributes.WeaponName) != 0)         common.weaponName = first.weaponName;
                if ((commonAttributes & Attributes.TotalShotsFired) != 0)    common.shotsFired = first.shotsFired;
                if ((commonAttributes & Attributes.WeaponSerial) != 0)       common.serialNumber = first.serialNumber;
                if ((commonAttributes & Attributes.WeaponNotes) != 0)        common.weaponNotes = first.weaponNotes;
                if ((commonAttributes & Attributes.RangeLocation) != 0)      common.rangeLocation = first.rangeLocation;
                if ((commonAttributes & Attributes.RangeDistance) != 0)      common.distance = first.distance;
                if ((commonAttributes & Attributes.RangeDistanceUnits) != 0) common.distanceUnits = first.distanceUnits;
                if ((commonAttributes & Attributes.RangeTemperature) != 0)   common.temperature = first.temperature;
                if ((commonAttributes & Attributes.AmmoCaliber) != 0)        common.caliberValue = first.caliberValue;
                if ((commonAttributes & Attributes.AmmoCaliberUnits) != 0)   common.caliber = first.caliber;
                if ((commonAttributes & Attributes.AmmoLotNumber) != 0)      common.lotNumber = first.lotNumber;
                if ((commonAttributes & Attributes.AmmoMass) != 0)           common.projectileMassGrains = first.projectileMassGrains;
                if ((commonAttributes & Attributes.AmmoNotes) != 0)          common.ammunitionNotes = first.ammunitionNotes;

                CurrentData = common;
                return common;
            }
        }

        /// <summary>
        /// Set the data for the current set of active images.
        /// </summary>
        /// <param name="updated">An <code>ImageData</code> instance with the updated data to copy values from.</param>
        /// <param name="modifiedAttributes">The set of attributes to copy to the active items.</param>
        public void SetData(Datatype.ImageData updated, Attributes modifiedAttributes)
        {
            if (_active.Length == 1)
            {
                _master.setImageData(_active[0], updated);

                _valid[_active[0]] = ValidateData(updated);
            }
            else
            {
                // Foreach active image, grab the current data, set the updated
                // values, and writes is back to the master.
                foreach (int image in _active)
                {
                    Datatype.ImageData temp = _master.GetImageData(image);
                    if ((modifiedAttributes & Attributes.DateTimeFired) != 0)      temp.dateTimeFired = updated.dateTimeFired;
                    if ((modifiedAttributes & Attributes.ShooterFirstName) != 0)   temp.shooterFName = updated.shooterFName;
                    if ((modifiedAttributes & Attributes.ShooterLastName) != 0)    temp.shooterLName = updated.shooterLName;
                    if ((modifiedAttributes & Attributes.TotalShotsFired) != 0)    temp.shotsFired = updated.shotsFired;
                    if ((modifiedAttributes & Attributes.WeaponName) != 0)         temp.weaponName = updated.weaponName;
                    if ((modifiedAttributes & Attributes.WeaponSerial) != 0)       temp.serialNumber = updated.serialNumber;
                    if ((modifiedAttributes & Attributes.WeaponNotes) != 0)        temp.weaponNotes = updated.weaponNotes;
                    if ((modifiedAttributes & Attributes.RangeLocation) != 0)      temp.rangeLocation = updated.rangeLocation;
                    if ((modifiedAttributes & Attributes.RangeDistance) != 0)      temp.distance = updated.distance;
                    if ((modifiedAttributes & Attributes.RangeDistanceUnits) != 0) temp.distanceUnits = updated.distanceUnits;
                    if ((modifiedAttributes & Attributes.RangeTemperature) != 0)   temp.temperature = updated.temperature;
                    if ((modifiedAttributes & Attributes.AmmoCaliber) != 0)        temp.caliberValue = updated.caliberValue;
                    if ((modifiedAttributes & Attributes.AmmoCaliberUnits) != 0)   temp.caliber = updated.caliber;
                    if ((modifiedAttributes & Attributes.AmmoLotNumber) != 0)      temp.lotNumber = updated.lotNumber;
                    if ((modifiedAttributes & Attributes.AmmoMass) != 0)           temp.projectileMassGrains = updated.projectileMassGrains;
                    if ((modifiedAttributes & Attributes.AmmoNotes) != 0)          temp.ammunitionNotes = updated.ammunitionNotes;
                    _master.setImageData(image, temp);

                    _valid[image] = ValidateData(temp);
                }
            }
        }

        /// <summary>
        /// Save each of the specified images to the database.
        /// </summary>
        /// <param name="images">The image indices to save.</param>
        public void SaveData(int[] images)
        {
            foreach (int image in images)
            {
                _master.SaveData(image);
            }
        }

        /// <summary>
        /// Save each image to the database and then generate a report with each.
        /// </summary>
        /// <param name="images">The image indices to generate a report with.</param>
        public void GenerateReport(int[] images)
        {
            foreach (int image in images)
            {
                _master.SaveData(image);

                Datatype.ImageData current = _master.GetImageData(image);
                SaveFileDialog save = new SaveFileDialog();
                save.AddExtension = true;
                save.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                save.FileName = String.Format("{0}-{1}-{2}",
                    new object[] {
                        current.weaponName,
                        current.shooterLName,
                        current.dateTimeFired.ToString("yyyyMMdd")});

                foreach (char invalid in Path.GetInvalidFileNameChars())
                    save.FileName = save.FileName.Replace(invalid.ToString(), "");

                if (save.ShowDialog() == DialogResult.OK)
                {
                    _master.GenerateReport(image, save.FileName);
                }
            }
        }

        /// <summary>
        /// Determines the hole diameter in pixels from the <code>Master</code>.
        /// </summary>
        /// <param name="image">The index of the image to reference.</param>
        /// <returns>The hole diameter for the particular image.</returns>
        internal int GetHoleDiameter(int image)
        {
            return _master.GetDiameterInPixels(image);
        }

        /// <summary>
        /// Update the scale information for the current information.
        /// </summary>
        /// <param name="image">The image's index to update</param>
        /// <param name="scale">The updated scale</param>
        /// <param name="caliber">The updated caliber</param>
        /// <param name="caliberUnit">The updated caliber unit</param>
        /// <param name="verticalLength">The updated vertical length</param>
        /// <param name="horizontalLength">The updated horizontal length</param>
        /// <param name="regionOfInterest">The updated region of interest</param>
        internal void UpdateScale(int image, Datatype.Scale scale, double caliber,
            Datatype.CaliberUnit caliberUnit, float verticalLength, float horizontalLength,
            Datatype.ROI regionOfInterest)
        {
            Datatype.ImageData current = _master.GetImageData(image);
            current.shotsFired = _unified.ShotsFired;
            current.caliberValue = caliber;
            current.caliber = caliberUnit;
            current.scale = scale;
            current.scale.verticalLength = verticalLength;
            current.scale.horizontalLength = horizontalLength;
            current.regionOfInterest = regionOfInterest;
            _master.setImageData(image, current);
        }

        /// <summary>
        /// Perorm image recognition on the current image.
        /// </summary>
        /// <returns>The list of points from image recognition</returns>
        internal List<Point> ImageRecResults()
        {
            return (List<Point>) _master.ProcessImage(_active[0], CurrentData);
        }

        /// <summary>
        /// Open the settings window during normal program execution.
        /// </summary>
        internal void OpenSettings()
        {
            OpenSettings(false);
        }

        /// <summary>
        /// Open the settings window with the specified <code>firstUse</code> argument.
        /// </summary>
        /// <param name="firstUse">Treat as this being the first execution of the program</param>
        internal void OpenSettings(bool firstUse)
        {
            UI.Settings settings;
            if (firstUse)
            {
                settings = new UI.Settings(this, _master.GetConfigReader());
            }
            else
            {
                settings = new UI.Settings(this, _master.GetConfigReader(), WeaponNames);
            }
            settings.ShowDialog();

            if (!firstUse)
            {
                _unified.UpdateWeapons(WeaponNames);
            }
        }

        /// <summary>
        /// Try to save each weapon to the database.
        /// </summary>
        /// <param name="addedWeapons">List of added weapon names</param>
        internal void SaveAddedWeapons(List<String> addedWeapons)
        {
            foreach (String w in addedWeapons)
            {
                _master.AddWeaponName(w);
            }
        }

        /// <summary>
        /// Prompt the user for target IDs to open from the database.
        /// </summary>
        internal void LoadImages()
        {
            bool valid = false;
            String prevIDs = "";

            UI.LoadImages load;

            // Prompt the user for target IDs until all IDs are valid, the user
            // ignores the invalid ones, or they stuop trying to load them.
            do
            {
                // Show the dialog box to the user
                load = new UI.LoadImages(prevIDs);
                load.ShowDialog();


                if (load.Result == DialogResult.OK)
                {
                    if (load.ErrorIDs.Count != 0)
                    {
                        String message = "The target IDs listed below were unable to be processed.  Would you like to correct this?\n\nInvalid target IDs:\n" + String.Join("\n", load.ErrorIDs.ToArray());

                        DialogResult fixError = MessageBox.Show(message, "Invalid target IDs", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

                        if (fixError == DialogResult.Yes)
                        {
                            // Set prevIDs so when a new dialog box is opened the previous IDs are pre-loaded
                            prevIDs = load.RawIDs;
                            continue;
                        }
                        else if (fixError == DialogResult.No)
                        {
                            // The user doesn't want to correct invalid target IDs so continue with the
                            // process and ignore the invalid ones.
                            valid = true;
                        }
                        else // fixError == DialogResult.Cancel
                        {
                            // The user clicked "cancel" and stopped trying to open target
                            return;
                        }

                    }
                    else
                    {
                        valid = true;
                    }
                }
                else
                {
                    // The user doesn't want to load targets, so stop prompting the user
                    return;
                }
            } while (!valid);

            if (load.TargetIDs.Count == 0)
            {
                MessageBox.Show("There were no target IDs to load.", "No IDs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Make sure that the entered target IDs exist in the system

                Dictionary<int, String> invalidTargets = _master.LoadData(load.TargetIDs);

                if (invalidTargets.Count > 0)
                {
                    String message = "Unable to load the following images:\n";

                    foreach (KeyValuePair<int, String> pair in invalidTargets)
                    {
                        message += String.Format("\n{0}: {1}", new object[] { pair.Key, pair.Value });
                    }

                    MessageBox.Show(message, "Unable to load images", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Set the current images to those that exist.
                _unified.SetImages(_master.GetFileNames());

                _valid = new bool[_master.GetFileNames().Length];

                _unified.SetEnabled(true, false);
            }
        }

        /// <summary>
        /// Determine if the provided data is valid.  It is valid if the number of shots fired,
        /// target distance, caliber, mass, and scale lengths are positive.
        /// </summary>
        /// <param name="data"><code>ImageData</code> instance to validate.</param>
        /// <returns>If the data is valid</returns>
        private bool ValidateData(Datatype.ImageData data)
        {
            return (data.shotsFired > 0) &&
                (data.distance > 0) &&
                (data.caliberValue > 0) &&
                (data.projectileMassGrains > 0) &&
                (data.scale.horizontalLength > 0) &&
                (data.scale.verticalLength > 0);
        }

        /// <summary>
        /// Details in an <code>ImageData</code> object.  These are used when
        /// determining what attributes have been changed or which to set.
        /// </summary>
        public enum Attributes : int
        {
            /// <summary>No attributes</summary>
            None = 0x0,
            /// <summary>Corresponds with <code>ImageData.dateTimeFired</code></summary>
            DateTimeFired = 0x1,
            /// <summary>Corresponds with <code>ImageData.shooterFName</code></summary>
            ShooterFirstName = 0x2,
            /// <summary>Corresponds with <code>ImageData.shooterLName</code></summary>
            ShooterLastName = 0x4,
            /// <summary>Corresponds with <code>ImageData.shotsFired</code></summary>
            TotalShotsFired = 0x8,
            /// <summary>Corresponds with <code>ImageData.rangeLocation</code></summary>
            RangeLocation = 0x10,
            /// <summary>Corresponds with <code>ImageData.distance</code></summary>
            RangeDistance = 0x20,
            /// <summary>Corresponds with <code>ImageData.distanceUnits</code></summary>
            RangeDistanceUnits = 0x40,
            /// <summary>Corresponds with <code>ImageData.temperature</code></summary>
            RangeTemperature = 0x80,
            /// <summary>Corresponds with <code>ImageData.weaponName</code></summary>
            WeaponName = 0x100,
            /// <summary>Corresponds with <code>ImageData.serialNumber</code></summary>
            WeaponSerial = 0x200,
            /// <summary>Corresponds with <code>ImageData.weaponNotes</code></summary>
            WeaponNotes = 0x400,
            /// <summary>Corresponds with <code>ImageData.caliberValue</code></summary>
            AmmoCaliber = 0x800,
            /// <summary>Corresponds with <code>ImageData.caliber</code></summary>
            AmmoCaliberUnits = 0x1000,
            /// <summary>Corresponds with <code>ImageData.lotNumber</code></summary>
            AmmoLotNumber = 0x2000,
            /// <summary>Corresponds with <code>ImageData.projectileMassGrains</code></summary>
            AmmoMass = 0x4000,
            /// <summary>Corresponds with <code>ImageData.ammunitionNotes</code></summary>
            AmmoNotes = 0x8000
        }

        /// <summary>
        /// Determines if all the active images are valid.
        /// </summary>
        /// <returns>If the active images are valid.</returns>
        internal bool ActiveValid()
        {
            foreach (int i in _active)
            {
                if (!_valid[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if all the images are valid.
        /// </summary>
        /// <returns>If all images are valid.</returns>
        internal bool AllValid()
        {
            foreach (bool v in _valid)
            {
                if (!v) return false;
            }
            return true;
        }
    }
}
