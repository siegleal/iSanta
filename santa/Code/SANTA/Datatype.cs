using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SANTA
{
	/// <summary>
	/// A class of various datatypes used in SANTA.
	/// </summary>
	public class Datatype
	{
		/// <summary>
		/// A struct of various statistics for a particular target.
		/// </summary>
		public struct Stats
		{
			// TODO:  Label all fields
			/// <summary>Mean radius.</summary>
			public double meanRadius;
			public double highestRound;
			public double lowestRound;
			public double furthestLeft;
			public double furthestRight;
			/// <summary>Extreme spread in the x direction.</summary>
			public double extremeSpreadX;
			/// <summary>Extreme spread in the y direction.</summary>
			public double extremeSpreadY;
			/// <summary>Extreme spread.</summary>
			public double extremeSpread;
			/// <summary>Standard deviation in the x direction.</summary>
			public double sigmaX;
			/// <summary>Standard deviation in the y direction.</summary>
			public double sigmaY;
			/// <summary>The center point of the cluster.</summary>
			public Datatype.DoublePoint center;
			/// <summary>Whether or not the statistics are current for the target.</summary>
			public bool current;
			/// <summary>The circular error probable radius.</summary>
			public double CEPRadius;
			/// <summary>An array of extreme points.</summary>
		    public Datatype.DoublePoint[] extremePoints;
		}

		/// <summary>
		/// A structure for possible units of caliber.
		/// </summary>
		public class CaliberUnit
		{
			/// <summary>The database ID for the caliber unit.</summary>
			public int caliberUnitID;
			/// <summary>The name of the unit (cal., in., mm., etc.).</summary>
			public String unitName;
			/// <summary>The number of caliber units that are in one inch for type conversion purposes.</summary>
			public double unitsPerInch;

			/// <summary>Constructs an empty caliber unit, effectively null.</summary>
			public CaliberUnit() {

			}

			/// <summary>
			/// Constructs a caliber unit.
			/// </summary>
			/// <param name="caliberUnitID">The database ID for the caliber unit.</param>
			/// <param name="unitName">The name of the unit (cal., in., mm., etc.).</param>
			/// <param name="unitsPerInch">The number of caliber units that are in one inch
			/// for type conversion purposes.</param>
			public CaliberUnit(int caliberUnitID, String unitName, double unitsPerInch) {
				this.caliberUnitID = caliberUnitID;
				this.unitName = unitName;
				this.unitsPerInch = unitsPerInch;
			}
		}

		/// <summary>
		/// A structure to hold scale information.
		/// </summary>
		public struct Scale
		{
			/// <summary>The point which gives horizontal scaling information.</summary>
			public Point horizontal;
			/// <summary>The point which gives scaling information in both directions.</summary>
			public Point middle;
			/// <summary>The point which gives vertical scaling information.</summary>
			public Point vertical;
			/// <summary>The length in inches that the distance between <code>horizontal</code> and 
			/// <code>middle</code> represents.</summary>
			public float horizontalLength; // in inches
			/// <summary>The length in inches that the distance between <code>vertical</code> and 
			/// <code>middle</code> represents.</summary>
			public float verticalLength; // in inches
		}

		/// <summary>
		/// A structure that holds a point using double-precision numerics.
		/// </summary>
        public struct DoublePoint
        {
			/// <summary>The x-coordinate of the point.</summary>
            public double X;
			/// <summary>The y-coordinate of the point.</summary>
            public double Y;

            /// <summary>
            /// Constructor to set points when created.
            /// </summary>
            /// <param name="newXPoint">X Coordinate</param>
            /// <param name="newYPoint">Y Coordinate</param>
            public DoublePoint(double newXPoint, double newYPoint)
            {
                X = newXPoint;
                Y = newYPoint;
            }
        }

		/// <summary>
		/// The region of interest used to crop the image during image recognition.
		/// </summary>
        public struct ROI
        {
			/// <summary>The top left point of the region of interest.</summary>
            public Point topLeft;
			/// <summary>The bottom right point of the region of interest.</summary>
            public Point bottomRight;
        }

		/// <summary>
		/// A structure that holds all information about a particular target image.
		/// </summary>
		public class ImageData
		{
			/// <summary>The database's target ID.</summary>
			public int targetID;
			/// <summary>The filename of the image.</summary>
			public String origFilename;
			/// <summary>The filename where the Excel report is stored.</summary>
			public String reportFilename;
			/// <summary>The date and time the target was fired upon.</summary>
			public DateTime dateTimeFired;
			/// <summary>The date and time the target was prcoessed by the system.</summary>
			public DateTime dateTimeProcessed;
			/// <summary>The shooter's last name.</summary>
			public String shooterLName;
			/// <summary>The shooter's first name.</summary>
			public String shooterFName;
			/// <summary>The location of the range.</summary>
			public String rangeLocation;
			/// <summary>The units that the distance to target is measured in.</summary>
			public UnitsOfMeasure distanceUnits;
			/// <summary>The distance to the target.</summary>
			public int distance;
			/// <summary>The temperature on the range.</summary>
			public Temperature temperature;
			/// <summary>The name of the weapon.</summary>
			public String weaponName;
			/// <summary>The serial number of the weapon.</summary>
			public String serialNumber;
			/// <summary>Notes on the weapon.</summary>
			public String weaponNotes;
			/// <summary>The units of caliber of the ammunition used.</summary>
			public CaliberUnit caliber;
			/// <summary>The caliber of the ammunition used.</summary>
			public double caliberValue;
			/// <summary>The lot number of the ammunition.</summary>
			public String lotNumber;
			/// <summary>The projectile mass in units of grains.</summary>
			public int projectileMassGrains;
			/// <summary>Notes on the ammunition.</summary>
			public String ammunitionNotes;
			/// <summary>A list of points where bullet holes are located.</summary>
			public IList<Point> points;
			/// <summary>The number of shots fired.</summary>
			public int shotsFired;
			/// <summary>The scale information for the target.</summary>
			public Scale scale;
			/// <summary>The region of interest for the target.</summary>
            public ROI regionOfInterest;
			/// <summary>The bitmap image of the target.</summary>
			public Bitmap bitmap;

			/// <summary>
			/// Constructs a blank <c>ImageData</c> object.
			/// </summary>
			public ImageData() {
				this.targetID = -1;
				this.caliber = new CaliberUnit();
				this.points = new List<Point>();
			}

			/// <summary>
			/// Constructs an <c>ImageData</c> object with given parameters.
			/// </summary>
			/// <param name="origFilename">The filename of the image.</param>
			/// <param name="reportFilename">The filename where the Excel report is stored.</param>
			/// <param name="dateTimeFired">The date and time the target was fired upon.</param>
			/// <param name="shooterLName">The shooter's last name.</param>
			/// <param name="shooterFName">The shooter's first name.</param>
			/// <param name="rangeLocation">The location of the range.</param>
			/// <param name="distanceUnits">The units that the distance to target is measured in.</param>
			/// <param name="distance">The distance to the target.</param>
			/// <param name="temperature">The temperature on the range.</param>
			/// <param name="weaponName">The name of the weapon.</param>
			/// <param name="serialNumber">The serial number of the weapon.</param>
			/// <param name="weaponNotes">Notes on the weapon.</param>
			/// <param name="caliber">The units of caliber of the ammunition used.</param>
			/// <param name="caliberValue">The caliber of the ammunition used.</param>
			/// <param name="lotNumber">The lot number of the ammunition.</param>
			/// <param name="projectileMassGrains">The projectile mass in units of grains.</param>
			/// <param name="ammunitionNotes">Notes on the ammunition.</param>
			/// <param name="scale">The scale information for the target.</param>
			/// <param name="shotsFired">The number of shots fired.</param>
			/// <param name="points">A list of points where bullet holes are located.</param>
			public ImageData(String origFilename, String reportFilename, DateTime dateTimeFired,
					String shooterLName, String shooterFName, String rangeLocation,
					UnitsOfMeasure distanceUnits, int distance, Temperature temperature,
					String weaponName, String serialNumber, String weaponNotes, CaliberUnit caliber,
					double caliberValue, String lotNumber, int projectileMassGrains,
					String ammunitionNotes, Scale scale, int shotsFired, IList<Point> points) {
				this.targetID = -1;
				this.origFilename = origFilename;
				this.reportFilename = reportFilename;
				this.dateTimeFired = dateTimeFired;
				this.shooterLName = shooterLName;
				this.shooterFName = shooterFName;
				this.rangeLocation = rangeLocation;
				this.distanceUnits = distanceUnits;
				this.distance = distance;
				this.temperature = temperature;
				this.weaponName = weaponName;
				this.serialNumber = serialNumber;
				this.weaponNotes = weaponNotes;
                this.caliber = caliber;
                this.caliberValue = caliberValue;
				this.lotNumber = lotNumber;
				this.projectileMassGrains = projectileMassGrains;
				this.ammunitionNotes = ammunitionNotes;
                this.scale = scale;
				this.shotsFired = shotsFired;
				this.points = points;
			}

			/// <summary>
			/// Constructs an <c>ImageData</c> object with given parameters.
			/// </summary>
			/// <param name="origFilename">The filename of the image.</param>
			/// <param name="reportFilename">The filename where the Excel report is stored.</param>
			/// <param name="dateTimeFired">The date and time the target was fired upon.</param>
			/// <param name="shooterLName">The shooter's last name.</param>
			/// <param name="shooterFName">The shooter's first name.</param>
			/// <param name="rangeLocation">The location of the range.</param>
			/// <param name="distanceUnits">The units that the distance to target is measured in.</param>
			/// <param name="distance">The distance to the target.</param>
			/// <param name="temperature">The temperature on the range.</param>
			/// <param name="weaponName">The name of the weapon.</param>
			/// <param name="serialNumber">The serial number of the weapon.</param>
			/// <param name="weaponNotes">Notes on the weapon.</param>
			/// <param name="caliber">The units of caliber of the ammunition used.</param>
			/// <param name="caliberValue">The caliber of the ammunition used.</param>
			/// <param name="lotNumber">The lot number of the ammunition.</param>
			/// <param name="projectileMassGrains">The projectile mass in units of grains.</param>
			/// <param name="ammunitionNotes">Notes on the ammunition.</param>
			/// <param name="scale">The scale information for the target.</param>
			/// <param name="shotsFired">The number of shots fired.</param>
			/// <param name="points">A list of points where bullet holes are located.</param>
			/// <param name="roi">The region of interest for the target.</param>
			public ImageData(String origFilename, String reportFilename, DateTime dateTimeFired,
					String shooterLName, String shooterFName, String rangeLocation,
					UnitsOfMeasure distanceUnits, int distance, Temperature temperature,
					String weaponName, String serialNumber, String weaponNotes, CaliberUnit caliber,
					double caliberValue, String lotNumber, int projectileMassGrains,
					String ammunitionNotes, Scale scale, int shotsFired, IList<Point> points, ROI roi) {
				this.targetID = -1;
				this.origFilename = origFilename;
				this.reportFilename = reportFilename;
				this.dateTimeFired = dateTimeFired;
				this.shooterLName = shooterLName;
				this.shooterFName = shooterFName;
				this.rangeLocation = rangeLocation;
				this.distanceUnits = distanceUnits;
				this.distance = distance;
				this.temperature = temperature;
				this.weaponName = weaponName;
				this.serialNumber = serialNumber;
				this.weaponNotes = weaponNotes;
				this.caliber = caliber;
				this.caliberValue = caliberValue;
				this.lotNumber = lotNumber;
				this.projectileMassGrains = projectileMassGrains;
				this.ammunitionNotes = ammunitionNotes;
				this.scale = scale;
				this.shotsFired = shotsFired;
				this.points = points;
				this.regionOfInterest = roi;
			}

			/// <summary>
			/// Generates test data for an image.
			/// </summary>
			/// <returns>An <c>ImageData</c> object with test data.</returns>
            public static ImageData GenerateTestData()
            {
                Scale s;
                s.horizontal = new Point(100, 100);
                s.vertical = new Point(0, 0);
                s.middle = new Point(0, 100);
				s.horizontalLength = 1.25f;
				s.verticalLength = 1.50f;
				ROI roi;
				roi.topLeft = new Point(100, 150);
				roi.bottomRight = new Point(200, 250);
                return new ImageData("origFilename", "reportFilename", DateTime.Now, "shooterLName",
					"shooterFName", "rangeLocation", UnitsOfMeasure.Yard, 100, Temperature.Hot, "carbine, 5.56mm G36 Commando", "serialNumber",
                    "weaponNotes", new CaliberUnit(2, "cal", 1), 0.45, "lotNumber", 50, "ammunitionNotes", s, 10,
                    new Point[] { new Point(1, 2), new Point(3, 4) }, roi);
            }

			/// <summary>
			/// Sample temperatures to choose from.
			/// </summary>
			public enum Temperature : byte {
				/// <summary>Cold temperature (&lt; 50°F)</summary>
				Cold = 0,
				/// <summary>Ambient temperature (50°F to 95°F)</summary>
				Ambient = 1,
				/// <summary>Hot temperature (&gt; 95°F)</summary>
				Hot = 2 };

			/// <summary>
			/// Visible temperature descriptions corresponding to <c>Temperature</c> values.
			/// </summary>
			public static String[] tempDetails = { "Cold (< 50°F)", "Ambient (50°F to 95°F)", "Hot (> 95°F)" };
		}

		/// <summary>
		/// Units of measure for distances.
		/// </summary>
        public enum UnitsOfMeasure : byte {
			/// <summary>Yards</summary>
			Yard = 0,
			/// <summary>Meters</summary>
			Meter = 1 };
	}
}
