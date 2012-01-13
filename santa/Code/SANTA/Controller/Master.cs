using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using SANTA.IO;

namespace SANTA.Controller
{
	/// <remarks>
	/// The <code>Master</code> controller class which controls all flow in the SANTA system.
	/// </remarks>
	public class Master
	{
		/// <summary>
		/// Constructs a <code>Master</code> object by initializing the
		/// <code>ConfigReader</code> and the <code>UIController</code>.
		/// </summary>
        public Master() {
            _configReader = new ConfigReader();
			bool firstUse = _configReader.getValue("First Use").Equals("True");
			if (firstUse) {
				_configReader.setValue("First Use", "False");
			}
			new UIController(this, firstUse);
		}

		/// <summary>
		/// Sets the filenames to the array of input filenames.
		/// </summary>
		/// <param name="fileNames">The filenames of the target images.</param>
		public void SetFileNames(String[] fileNames) {
			if (fileNames.Length <= 0) {
				throw new ArgumentException("fileNames must have Length > 0");
			}

			List<String> newFileNames = new List<String>();
			List<Datatype.ImageData> imageData = new List<Datatype.ImageData>();

			for (int i = 0; i < fileNames.Length; i++ ) {
				if (!File.Exists(fileNames[i])) {
					Log.LogError("Invalid filename input from user.", "filename", fileNames[i]);
				}
				try {
					Bitmap bmp = IO.LoadBitmap(fileNames[i]);
					newFileNames.Add(fileNames[i]);
					Datatype.ImageData imageDatum = new Datatype.ImageData();
					imageDatum.origFilename = fileNames[i];
					imageDatum.bitmap = bmp;
					imageDatum.dateTimeFired = DateTime.Today;
					imageData.Add(imageDatum);
				} catch (ArgumentException) {
					Log.LogInfo("Invalid filename (not an image) input from user.", "filename", fileNames[i]);
				}
			}

			_fileNames = newFileNames.ToArray();
			_imageData = imageData.ToArray();
            _stats = new Datatype.Stats[imageData.Count];
		}

		/// <summary>
		/// Returns the list of filenames being processed.
		/// </summary>
		/// <returns>The list of filenames being processed.</returns>
		public String[] GetFileNames() {
			return _fileNames;
		}

		/// <summary>
		/// Returns the image data for a particular image.
		/// </summary>
		/// <param name="index">The index of the image for which to return data.</param>
		/// <returns>The image data for the given image index.</returns>
		public Datatype.ImageData GetImageData(int index) {
			return _imageData[index];
		}

		/// <summary>
		/// Sets the image data at a particular index.
		/// </summary>
		/// <param name="index">The index at which to set to image data.</param>
		/// <param name="imageData">The image data to set.</param>
		public void setImageData(int index, Datatype.ImageData imageData) {
			int targetID = _imageData[index].targetID;
			_imageData[index] = imageData;
			_imageData[index].targetID = targetID;
		}

		/// <summary>
		/// Returns the number of images to be processed.
		/// </summary>
		/// <returns>The number of images to be processed.</returns>
		public int GetNumberOfImages() {
			return _fileNames.Length;
		}

		/// <summary>
		/// Processes the given image and returns a list of potential bullet holes.
		/// </summary>
		/// <param name="index">The index of the image to process.</param>
		/// <param name="partialImageData">The partial image data specifically containing the filename, number of shots
		/// fired, caliber information, scaling information, and region of interest.</param>
		/// <returns>A list of point coordinates for potential bullet holes.</returns>
		/// <exception cref="ArgumentException">Thrown if there is a problem with the file, region of interest,
		/// caliber, or scale.</exception>
		public IList<Point> ProcessImage(int index, Datatype.ImageData partialImageData) {
			if (!File.Exists(partialImageData.origFilename)) {
				Log.LogWarning("Image file not found.", "filename", partialImageData.origFilename);
				throw new ArgumentException("Image file not found.", "partialImageData.origFilename");
			} else if (partialImageData.regionOfInterest.topLeft.Equals(partialImageData.regionOfInterest.bottomRight)) {
				Log.LogError("Region of interest of size zero.", "topLeft",
					partialImageData.regionOfInterest.topLeft.ToString(), "bottomRight",
					partialImageData.regionOfInterest.bottomRight.ToString());
				throw new ArgumentException("Region of interest cannot have zero size.", "partialImageData.regionOfInterest");
			}

			setImageData(index, partialImageData);
			_imageData[index].points = ImageRecognition.ProcessImage(partialImageData.origFilename,
					partialImageData.shotsFired, Stats.PixelsPerRadius(partialImageData.caliber, partialImageData.scale,
					partialImageData.caliberValue), partialImageData.regionOfInterest.topLeft.X,
					partialImageData.regionOfInterest.topLeft.Y,
					partialImageData.regionOfInterest.bottomRight.Y - partialImageData.regionOfInterest.topLeft.Y,
					partialImageData.regionOfInterest.bottomRight.X - partialImageData.regionOfInterest.topLeft.X);
			return _imageData[index].points;
		}

		/// <summary>See <code>IO.AddCaliberUnit()</code>.</summary>
		public void AddCaliberUnit(Datatype.CaliberUnit caliberUnit) {
			IO.AddCaliberUnit(caliberUnit, _configReader);
		}

		/// <summary>See <code>IO.AddWeaponName()</code>.</summary>
		public void AddWeaponName(String weaponName) {
			IO.AddWeaponName(weaponName, _configReader);
		}

		/// <summary>See <code>IO.GetCaliberUnits()</code>.</summary>
		public List<Datatype.CaliberUnit> GetCaliberUnits() {
			return IO.GetCaliberUnits(_configReader);
		}

		/// <summary>See <code>IO.GetWeaponNames()</code>.</summary>
		public List<String> GetWeaponNames() {
			return IO.GetWeaponNames(_configReader);
		}

		/// <summary>
		/// Saves the image data for a particular image.
		/// </summary>
		/// <param name="index">The index of the image to save.</param>
		/// <returns>The TargetID where the image is stored.</returns>
		public int SaveData(int index) {
			CheckStats(index);
			_imageData[index].targetID = IO.SaveData(_imageData[index], _configReader);
			return _imageData[index].targetID;
		}

		/// <summary>
		/// Loads the list of <code>TargetID</code>s from the database.
		/// </summary>
		/// <param name="targetIDs">The list of <code>TargetID</code>s to load.</param>
		/// <returns>A dictionary of invalid <c>TargetID</c>s mapped to filenames (or null if the <c>TargetID</c>
		/// could not be found).</returns>
		/// <exception cref="ArgumentException">The list of targetIDs is empty.</exception>
		public Dictionary<int, String> LoadData(List<int> targetIDs) {
			if (targetIDs.Count <= 0) {
				throw new ArgumentException("targetIDs must have Length > 0");
			}
			
			List<String> fileNames = new List<String>();
			List<Datatype.ImageData> imageData = new List<Datatype.ImageData>();
			Dictionary<int, String> invalidTargets = new Dictionary<int, string>();

			for (int i = 0; i < targetIDs.Count; i++) {
				Datatype.ImageData imageDatum = IO.LoadData(targetIDs[i], _configReader);
				if (imageDatum == null) {
					Log.LogInfo("TargetID not found.", "TargetID", targetIDs[i].ToString());
					invalidTargets.Add(targetIDs[i], null);
				} else if (!File.Exists(imageDatum.origFilename)) {
					Log.LogInfo("Image file not found.", "origFileName", imageDatum.origFilename);
					invalidTargets.Add(imageDatum.targetID, imageDatum.origFilename);
				} else {
					try {
						imageDatum.bitmap = IO.LoadBitmap(imageDatum.origFilename);
						imageData.Add(imageDatum);
						fileNames.Add(imageDatum.origFilename);
					} catch (ArgumentException) {
						Log.LogError("Invalid filename (not an image) in database.", "origFilename", imageDatum.origFilename);
						invalidTargets.Add(imageDatum.targetID, imageDatum.origFilename);
					}
				}
			}
			_fileNames = fileNames.ToArray();
			_imageData = imageData.ToArray();
            _stats = new Datatype.Stats[imageData.Count];

			return invalidTargets;
		}

		/// <summary>
		/// Sets the point coordinates of bullet holes for an image.
		/// </summary>
		/// <param name="index">The index of the image.</param>
		/// <param name="points">The point coordinates of the bullet holes.</param>
		public void SetPoints(int index, List<Point> points) {
			_imageData[index].points = points;
			_stats[index].current = false;
		}

		/// <summary>
		/// Returns the diameter of a bullet hole in pixels.
		/// </summary>
		/// <param name="index">The index of the image.</param>
		/// <returns>The diameter of a bullet hole in pixels.</returns>
		public int GetDiameterInPixels(int index) {
			return 2 * Stats.PixelsPerRadius(_imageData[index].caliber, _imageData[index].scale,
				_imageData[index].caliberValue);
		}

		/// <summary>
		/// Generates an Excel report of the image data.
		/// </summary>
		/// <param name="index">The index of the image.</param>
		/// <param name="reportPath">The filepath to save the report.</param>
		public void GenerateReport(int index, String reportPath) {
			CheckStats(index);
			_imageData[index].reportFilename = reportPath;
			IO.GenerateReport(_imageData[index], _stats[index], _configReader);
		}

		/// <summary>
		/// Checks to see if the statistics for an image are current and updates them if necessary.
		/// </summary>
		/// <param name="index">The index of the image.</param>
		protected void CheckStats(int index) {
			if (!_stats[index].current) {
				_stats[index] = Stats.GenerateStatistics(_imageData[index].points);
				_stats[index].current = true;
			}
		}

		/// <summary>
		/// Returns the local <code>ConfigReader</code> object.
		/// </summary>
		/// <returns>The local <code>ConfigReader</code> object.</returns>
		internal ConfigReader GetConfigReader() {
			return _configReader;
		}

		/// <summary>The array of filenames being processed.</summary>
		protected String[] _fileNames;

		/// <summary>The array of image data for each image being processed.</summary>
		protected Datatype.ImageData[] _imageData;

		/// <summary>The array of statistical information for each image being processed.</summary>
		protected Datatype.Stats[] _stats;

		/// <summary>The <code>ConfigReader</code> for the configuration file.</summary>
		protected ConfigReader _configReader;
    }
}
