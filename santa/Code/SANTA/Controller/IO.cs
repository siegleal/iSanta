using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SANTA.IO;
using SANTA.Utility;

namespace SANTA.Controller
{
	static class IO
	{
		/*public static void GenerateTestReport(ConfigReader configReader) {
			Console.WriteLine("~~Generating Test Report~~");
			Console.WriteLine("Generating Dummy Report Data...");
			Datatype.ImageData testData = GenerateTestData();
			Datatype.Stats testStats = GenerateTestStatistics(testData.points);

			ExcelReportGenerator.GenerateReport(testData, testStats, configReader);
		}
      
        private static Datatype.ImageData GenerateTestData()
        {
            Datatype.ImageData data = new Datatype.ImageData();
            data.shooterFName = "Jeff";
            data.shooterLName = "Johnson";
            data.dateTimeFired = new DateTime(2009, 01, 13);
            data.rangeLocation = "Hulbert Field";
            data.temperature = Datatype.ImageData.Temperature.Ambient;
            data.distance = 100;
            data.shotsFired = 20;

            data.weaponName = "Glock 19 Jet 192";
            data.serialNumber = "8AZK018";

            data.caliber = new Datatype.CaliberUnit(1, "0.45 cal", 40);
            data.caliber.caliberUnitID = 1;
            data.caliber.caliber = "0.45 cal";
            data.lotNumber = "59A";
            data.projectileMassGrains = 10;

            data.points = new List<Point>();
            Random generator = new Random();

            for (int i = 0; i < 20; i++)
            {
                if (i < 6)
                {
                    Point p = new Point(generator.Next(1, 100), generator.Next(1, 100));
                    data.points.Add(p);
                }
                else if (i < 14)
                {
                    Point p = new Point(3 * generator.Next(1, 100), 3 * generator.Next(1, 100));
                    data.points.Add(p);
                }
                else
                {
                    Point p = new Point(5 * generator.Next(1, 100), 5 * generator.Next(1, 100));
                    data.points.Add(p);
                }
            }

            return data;
        }

        private static Datatype.Stats GenerateTestStatistics(IList<Point> points)
        {
            Datatype.Stats stats = new Datatype.Stats();
            Random generator = new Random();
            stats.meanRadius = 3;
            stats.highestRound = 1;
            stats.lowestRound = 1;
            stats.furthestLeft = 2;
            stats.furthestRight = 3;
            stats.extremeSpreadX = 2;
            stats.extremeSpreadY = 2;
            stats.extremeSpread = 1;
            stats.sigmaX = 2;
            stats.sigmaY = 2;
            stats.center = new Point(generator.Next(1, 100), generator.Next(1, 100));
            stats = Stats.GenerateStatistics(points);
            return stats;
        } */

		/// <summary>
		/// Tests the MSAccess class.
		/// </summary>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		public static void TestMSAccess(ConfigReader configReader) {
			// Test insert
			Datatype.ImageData savedData = Datatype.ImageData.GenerateTestData();
			int targetID = TestMSAccessSaveAndLoadData(savedData, configReader);

			// Test update
			savedData.targetID = targetID;
			int targetID2 = TestMSAccessSaveAndLoadData(savedData, configReader);
		}

		/// <summary>
		/// Tests saving and loading the same data.
		/// </summary>
		/// <param name="savedData">The data to save and load back out.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <returns>The <c>TargetID</c> of the loaded data.</returns>
		public static int TestMSAccessSaveAndLoadData(Datatype.ImageData savedData, ConfigReader configReader) {
			int targetID = SaveData(savedData, configReader);
			if (targetID < 0) {
				throw new Exception("Save data failed");
			}
			Datatype.ImageData loadedData = LoadData(targetID, configReader);
			if (loadedData.targetID != targetID
					|| !loadedData.origFilename.Equals(savedData.origFilename)
					|| !loadedData.reportFilename.Equals(savedData.reportFilename)
					|| !loadedData.dateTimeFired.Date.Equals(savedData.dateTimeFired.Date)
					|| !loadedData.shooterLName.Equals(savedData.shooterLName)
					|| !loadedData.shooterFName.Equals(savedData.shooterFName)
					|| !loadedData.rangeLocation.Equals(savedData.rangeLocation)
					|| loadedData.distanceUnits != savedData.distanceUnits
					|| loadedData.distance != savedData.distance
					|| loadedData.temperature != savedData.temperature
					|| !loadedData.weaponName.Equals(savedData.weaponName)
					|| !loadedData.serialNumber.Equals(savedData.serialNumber)
					|| !loadedData.weaponNotes.Equals(savedData.weaponNotes)
					|| loadedData.caliber.caliberUnitID != savedData.caliber.caliberUnitID
					|| loadedData.caliberValue != savedData.caliberValue
					|| !loadedData.lotNumber.Equals(savedData.lotNumber)
					|| loadedData.projectileMassGrains != savedData.projectileMassGrains
					|| !loadedData.weaponNotes.Equals(savedData.weaponNotes)
					|| !loadedData.ammunitionNotes.Equals(savedData.ammunitionNotes)
					|| loadedData.shotsFired != savedData.shotsFired) {
				throw new Exception("Loaded data not identical to saved data");
			}
			if (!loadedData.scale.horizontal.Equals(savedData.scale.horizontal)
					|| !loadedData.scale.middle.Equals(savedData.scale.middle)
					|| !loadedData.scale.vertical.Equals(savedData.scale.vertical)
					|| loadedData.scale.horizontalLength != savedData.scale.horizontalLength
					|| loadedData.scale.verticalLength != savedData.scale.verticalLength) {
				throw new Exception("Loaded data not identical to saved data");
			}
			if (!loadedData.regionOfInterest.topLeft.Equals(savedData.regionOfInterest.topLeft)
					|| !loadedData.regionOfInterest.bottomRight.Equals(savedData.regionOfInterest.bottomRight)) {
				throw new Exception("Loaded data not identical to saved data");
			}
			// TODO:  Points untested
			System.Console.WriteLine("Success");
			return targetID;
		}

		/// <summary>
		/// Loads a bitmap image from the given filename.
		/// </summary>
		/// <param name="filename">The filename to load.</param>
		/// <returns>A bitmap image from the given filename.</returns>
		public static Bitmap LoadBitmap(String filename) {
			Bitmap temp = new Bitmap(filename);
			Bitmap image = new Bitmap(temp);
			temp.Dispose();
			return image;
		}

		/// <summary>
		/// Generates a report to an excel file.
		/// </summary>
		/// <param name="data">Data to save.</param>
		/// <param name="stats">The statistics on the data.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the sample report.</param>
		public static void GenerateReport(Datatype.ImageData data, Datatype.Stats stats, ConfigReader configReader) {
			UnitConverter.GenerateReport(data, stats, configReader);
		}

		/// <summary>See <c>MSAccess.AddCaliberUnit()</c>.</summary>
		public static void AddCaliberUnit(Datatype.CaliberUnit caliberUnit, ConfigReader configReader) {
			MSAccess.AddCaliberUnit(caliberUnit, configReader);
		}

		/// <summary>See <c>MSAccess.AddWeaponName()</c>.</summary>
		public static void AddWeaponName(String weaponName, ConfigReader configReader) {
			MSAccess.AddWeaponName(weaponName, configReader);
		}

		/// <summary>See <c>MSAccess.GetCaliberUnits()</c>.</summary>
		public static List<Datatype.CaliberUnit> GetCaliberUnits(ConfigReader configReader) {
			return MSAccess.GetCaliberUnits(configReader);
		}

		/// <summary>See <c>MSAccess.GetWeaponNames()</c>.</summary>
		public static List<String> GetWeaponNames(ConfigReader configReader) {
			return MSAccess.GetWeaponNames(configReader);
		}

		/// <summary>See <c>MSAccess.SaveData()</c>.</summary>
		public static int SaveData(Datatype.ImageData imageData, ConfigReader configReader) {
			return MSAccess.SaveData(imageData, configReader);
		}

		/// <summary>See <c>MSAccess.LoadData()</c>.</summary>
		public static Datatype.ImageData LoadData(int targetID, ConfigReader configReader) {
			return MSAccess.LoadData(targetID, configReader);
		}
	}
}
