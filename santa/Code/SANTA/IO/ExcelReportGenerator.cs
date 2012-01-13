using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Management.Instrumentation;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using GemBox.Spreadsheet;

namespace SANTA.IO
{
	static class ExcelReportGenerator
	{
	    private static double minutesOfAngleConverter;
        private const double minutesOfAngleConversionFactor = 1.0471975511966;  //TODO: This is for 100 yards.  100 yards/distance * Conversion factor

        private const int reportImageWidth  = 300,
                          reportImageHeight = 183;

		public static void GenerateReport(IList<Datatype.DoublePoint> newPoints, Datatype.ImageData data, Datatype.Stats stats, ConfigReader reader) {
		    string imagePath = data.origFilename;
		    string savePath = data.reportFilename;

            if (savePath == null)
            {
                throw new NullReferenceException("A save path must be provided in order for the report to save");
            }

			//Open Excel File
            ExcelFile excelFile = new ExcelFile();

            try
            {
                excelFile.LoadXlsx(reader.getValue("Template Location"),
                                  XlsxOptions.PreserveKeepOpen);
            }
            //Catch and report error if directory is not valid
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("A report could not be generated.  Please check the configuration file for errors.  " +
                "The path to the default template contains a directory that doesn't seem to exist.", "Directory Not Found", 
                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                throw new System.InvalidOperationException("DirectoryNotFoundException");
            }
            //Catch and return error if file is not found.
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("A report could not be generated.  Please check the configuration file for errors.  " +
                "The file name given doesn't seem to exist.", "File Not Found",
                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                throw new System.InvalidOperationException("FileNotFoundException");
            }
            //Catch and report error if the file is already opened and locked
            catch (System.IO.IOException)
            {
                MessageBox.Show("A report could not be generated because the template file is locked.  " +
                "Please close any application that may be using the file and try again.", "File Could Not Load",
                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                throw new System.InvalidOperationException("FileLoadException");
            } 
            ExcelWorksheet reportWorksheet = excelFile.Worksheets[0];
            ExcelWorksheet otherStatsWorksheet = excelFile.Worksheets[1];

            //Set row fill color.
            Color rowFillColor = Color.FromArgb(219, 229, 241);
            
            //Check that data.distance != 0
            if (data.distance == 0)
            {
                throw new DivideByZeroException("Data.distance cannot be 0");
            }

            //Determine minutes of angle factor.  If yards, use the literal data.distance; else convert to yards.
            if (data.distanceUnits == SANTA.Datatype.UnitsOfMeasure.Yard)
            {
                minutesOfAngleConverter = minutesOfAngleConversionFactor*100/data.distance;
            } else if (data.distanceUnits == SANTA.Datatype.UnitsOfMeasure.Meter)
            {
                minutesOfAngleConverter = minutesOfAngleConversionFactor*100/(data.distance*1.0936133);
            } else
            {
                //Should never get here, but exception will trigger if we do.
                throw new InstanceNotFoundException("Distance Units are not of a known datatype.");
            }

		    // Column width of 8, 30, 16, 9, 9, 9, 9, 4 and 5 characters.
            reportWorksheet.Columns[0].Width = 285/7*256;
            reportWorksheet.Columns[1].Width = 107/7*256;
            reportWorksheet.Columns[2].Width = 12*(256 - 24);
            reportWorksheet.Columns[3].Width = 12*(256 - 24);
            reportWorksheet.Columns[4].Width = 12*(256 - 24);
            reportWorksheet.Columns[5].Width = 12*(256 - 24);
            reportWorksheet.Columns[6].Width = 12*(256 - 24);
            reportWorksheet.Columns[7].Width = 12*(256 - 24);

            // Fill in cell data

            placeValue(reportWorksheet, "Target Distance", data.distance);
            
            reportWorksheet.Cells[reader.getValue("Shooter Name")].Value = data.shooterFName + " " + data.shooterLName;
            reportWorksheet.Cells[reader.getValue("Shoot Date")].Value = data.dateTimeFired;
            reportWorksheet.Cells[reader.getValue("Shoot Date")].Style.NumberFormat = reader.getValue("Date Format");
            reportWorksheet.Cells[reader.getValue("Range Name")].Value = data.rangeLocation;
            reportWorksheet.Cells[reader.getValue("Temperature")].Value = Datatype.ImageData.tempDetails[(int) data.temperature];
            reportWorksheet.Cells[reader.getValue("Target Distance")].Value = data.distance + " " + data.distanceUnits;

            if (data.shotsFired == newPoints.Count)
            {
                reportWorksheet.Cells[reader.getValue("Shots Fired")].Value = data.shotsFired;
            }
            else
            {
                reportWorksheet.Cells[reader.getValue("Shots Fired")].Value = newPoints.Count.ToString() + " found of " + data.shotsFired.ToString() + " indicated";
            }

            reportWorksheet.Cells[reader.getValue("Weapon Name")].Value = data.weaponName;
            reportWorksheet.Cells[reader.getValue("Weapon Serial Number")].Value = data.serialNumber;
            reportWorksheet.Cells[reader.getValue("Ammo Caliber")].Value = data.caliberValue + " " + data.caliber.unitName;
            reportWorksheet.Cells[reader.getValue("Ammo Lot Number")].Value = data.lotNumber;
            reportWorksheet.Cells[reader.getValue("Ammo Mass")].Value = data.projectileMassGrains + " grains";

            reportWorksheet.Cells[reader.getValue("Target ID")].Value = "Target ID: " + data.targetID;
            reportWorksheet.Cells[reader.getValue("Weapon Notes")].Value = "Weapon Notes: " + data.weaponNotes;
            reportWorksheet.Cells[reader.getValue("Ammo Notes")].Value = "Ammo Notes: " + data.ammunitionNotes;


            //Fill in the shot records
            Random generator = new Random();


                for (int i = 0; i < newPoints.Count && i < 50; i++)
                {
                    //TODO: Set first point of x, y, z values (or specify each point)
                    insertPoints(reportWorksheet, "G" + (i + 15).ToString(), newPoints[i].X, reader);
                    insertPoints(reportWorksheet, "H" + (i + 15).ToString(), newPoints[i].Y, reader);

                    //Color every other row in the shot record table.
                    if (i % 2 == 1)
                    {
                        reportWorksheet.Cells["F" + (i + 15).ToString()].Style.FillPattern.SetSolid(rowFillColor);
                        reportWorksheet.Cells["G" + (i + 15).ToString()].Style.FillPattern.SetSolid(rowFillColor);
                        reportWorksheet.Cells["H" + (i + 15).ToString()].Style.FillPattern.SetSolid(rowFillColor);
                    }

                }

            //Fill in the color for 
                for (int i = newPoints.Count; i < 50; i = i + 2)
            {
                //if the first time through, we are on an odd instead of an even, put us back on evens.
                if (i%2 == 0)
                {
                    i++;
                }
                reportWorksheet.Cells["F" + (i + 15).ToString()].Style.FillPattern.SetSolid(rowFillColor);
                reportWorksheet.Cells["G" + (i + 15).ToString()].Style.FillPattern.SetSolid(rowFillColor);
                reportWorksheet.Cells["H" + (i + 15).ToString()].Style.FillPattern.SetSolid(rowFillColor);
            }

            //Display stats
            insertStat(reportWorksheet, "Extreme Spread X", stats.extremeSpreadX, reader);
            insertStat(reportWorksheet, "Extreme Spread Y", stats.extremeSpreadY, reader);
            insertStat(reportWorksheet, "Extreme Spread", stats.extremeSpread, reader);
            insertStat(reportWorksheet, "Mean Radius", stats.meanRadius, reader);
            insertStat(reportWorksheet, "Sigma X", stats.sigmaX, reader);
            insertStat(reportWorksheet, "Sigma Y", stats.sigmaY, reader);
            insertStat(reportWorksheet, "Furthest Left", stats.furthestLeft, reader);
            insertStat(reportWorksheet, "Furthest Right", stats.furthestRight, reader);
            insertStat(reportWorksheet, "Highest Round", stats.highestRound, reader);
            insertStat(reportWorksheet, "Lowest Round", stats.lowestRound, reader);
            insertStat(reportWorksheet, "CEP", stats.CEPRadius, reader);

            //Add a small image of the pic in the upper right corner.
            Bitmap image = new Bitmap(imagePath);
            int width = image.Width;
            int height = image.Height;

            double aspectRatio = (double)width / (double)height;

            image.Dispose();

            if (aspectRatio > (double)reportImageWidth / (double)reportImageHeight)
            {
                double newHeight = (double)reportImageWidth / aspectRatio * 0.9; // The 0.9 is a fudge factor because GemBox doesn't know how to scale things properly.
                reportWorksheet.Pictures.Add(imagePath, new Rectangle(0, (int) ((reportImageHeight - newHeight) * 0.5), reportImageWidth, (int) (newHeight + 0.5)));
            }
            else
            {
                double newWidth = (double)reportImageHeight * aspectRatio / 0.9; // The 0.9 is a fudge factor because GemBox doesn't know how to scale things properly.
                reportWorksheet.Pictures.Add(imagePath, new Rectangle((int) ((reportImageWidth - newWidth) * 0.5), 0, (int) (newWidth + 0.5), reportImageHeight));
            }

            //Add CEP Data
            otherStatsWorksheet.Cells[reader.getValue("X Offset")].Value = stats.center.X;
            otherStatsWorksheet.Cells[reader.getValue("Y Offset")].Value = stats.center.Y;
            otherStatsWorksheet.Cells[reader.getValue("Mean X Offset")].Value = stats.center.X;
            otherStatsWorksheet.Cells[reader.getValue("Mean Y Offset")].Value = stats.center.Y;
            otherStatsWorksheet.Cells[reader.getValue("CEP Radius")].Formula = "=IF(Report!" + reader.getValue("CEP Toggle Cell") + " =\"CEP Off\", " + 0 + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in inches)\", " + stats.CEPRadius + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in centimeters)\", " + (stats.CEPRadius * 2.54) + ", " + (stats.CEPRadius * minutesOfAngleConverter) + ")))";
            otherStatsWorksheet.Cells[reader.getValue("Mean Radius Circle")].Formula = "=IF(Report!" + reader.getValue("Mean Radius Toggle Cell") + " =\"Mean Radius Off\", " + 0 + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in inches)\", " + stats.meanRadius + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in centimeters)\", " + (stats.meanRadius * 2.54) + ", " + (stats.meanRadius * minutesOfAngleConverter) + ")))";
            otherStatsWorksheet.Cells[reader.getValue("Extreme Spread x1")].Formula = "=IF(Report!" + reader.getValue("Extreme Spread Toggle Cell") + " =\"Extreme Spread Off\", " + 0 + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in inches)\", " + stats.extremePoints[0].X + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in centimeters)\", " + (stats.extremePoints[0].X * 2.54) + ", " + (stats.extremePoints[0].X * minutesOfAngleConverter) + ")))";
            otherStatsWorksheet.Cells[reader.getValue("Extreme Spread x2")].Formula = "=IF(Report!" + reader.getValue("Extreme Spread Toggle Cell") + " =\"Extreme Spread Off\", " + 0 + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in inches)\", " + stats.extremePoints[1].X + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in centimeters)\", " + (stats.extremePoints[1].X * 2.54) + ", " + (stats.extremePoints[1].X * minutesOfAngleConverter) + ")))";
            otherStatsWorksheet.Cells[reader.getValue("Extreme Spread y1")].Formula = "=IF(Report!" + reader.getValue("Extreme Spread Toggle Cell") + " =\"Extreme Spread Off\", " + 0 + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in inches)\", " + stats.extremePoints[0].Y + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in centimeters)\", " + (stats.extremePoints[0].Y * 2.54) + ", " + (stats.extremePoints[0].Y * minutesOfAngleConverter) + ")))";
            otherStatsWorksheet.Cells[reader.getValue("Extreme Spread y2")].Formula = "=IF(Report!" + reader.getValue("Extreme Spread Toggle Cell") + " =\"Extreme Spread Off\", " + 0 + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in inches)\", " + stats.extremePoints[1].Y + ", IF(Report!" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in centimeters)\", " + (stats.extremePoints[1].Y * 2.54) + ", " + (stats.extremePoints[1].Y * minutesOfAngleConverter) + ")))";
            
            //Generate CEP Circle formula data
            for (int cell = 0; cell <= 39; cell++)
            {
                otherStatsWorksheet.Cells["B" + (cell + 9).ToString()].Formula = "=$B$3+A" + (cell + 9).ToString();
                otherStatsWorksheet.Cells["C" + (cell + 9).ToString()].Formula = "=SQRT(ABS(POWER($B$5,2)-POWER(B" + (cell + 9).ToString() + ",2))) +$B$4";
                otherStatsWorksheet.Cells["D" + (cell + 9).ToString()].Formula = "=$B$4 - SQRT(ABS(POWER($B$5,2)-POWER(B" + (cell + 9).ToString() + ",2)))";
            }

            //Generate Mean Circle formula data
            for (int cell = 0; cell <= 39; cell++)
            {
                otherStatsWorksheet.Cells["M" + (cell + 9).ToString()].Formula = "=$M$3+L" + (cell + 9).ToString();
                otherStatsWorksheet.Cells["N" + (cell + 9).ToString()].Formula = "=SQRT(ABS(POWER($M$5,2)-POWER(M" + (cell + 9).ToString() + ",2))) +$M$4";
                otherStatsWorksheet.Cells["O" + (cell + 9).ToString()].Formula = "=$M$4 - SQRT(ABS(POWER($M$5,2)-POWER(M" + (cell + 9).ToString() + ",2)))";
            }

            try
            {
                //Write Report
                excelFile.SaveXlsx(savePath);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("A report could not be generated.  The file you attempted to save to is busy.  Please close all applications using  " + savePath +
                " and press ok to continue.", "File Open", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                throw new System.InvalidOperationException("DirectoryNotFoundException");
            }
            finally
            {
                //Close Report
                excelFile.ClosePreservedXlsx();
            }
		    //Check to see what to do with report now
                switch (reader.getValue("Open Excel By Default").ToString())
                {
                    case "nothing":
                        break;
                    case "prompt":
                        DialogResult response = MessageBox.Show("Would you like to open the report you just created?", "Open report?",
                                        MessageBoxButtons.YesNo);
                        if (response == DialogResult.Yes)
                        {
                            openReportWithExcel(savePath);
                        }
                        break;
                    case "open":
                        openReportWithExcel(savePath);
                        break;
                }
		}

        private static void insertStat(ExcelWorksheet reportWorksheet, string configName, double statValue, ConfigReader reader) 
        {
            String output = "=IF(" + reader.getValue("Cell To Determine Unit of Measure") + " =\"Statistics (in inches):\", " + statValue + ", IF(" + reader.getValue("Cell To Determine Unit of Measure") + " =\"Statistics (in centimeters):\", " + (statValue * 2.54) + ", " + (statValue * minutesOfAngleConverter) + "))";
            
            reportWorksheet.Cells[reader.getValue(configName)].Formula = output;
                    
        }

        private static void insertPoints(ExcelWorksheet reportWorksheet, string configName, double statValue, ConfigReader reader)
        {
            String output = "=IF(" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in inches)\", " + statValue + ", IF(" + reader.getValue("Cell To Determine Unit of Points") + " =\"Shot Record (in centimeters)\", " + (statValue * 2.54) + ", " + (statValue * minutesOfAngleConverter) + "))";
            
            reportWorksheet.Cells[configName].Formula = output;
        }

        private static bool openReportWithExcel(string fileName)
        {
			try
			{
				System.Diagnostics.Process.Start(fileName);
			}
			catch(Exception)
			{
				Console.WriteLine(fileName + " created in application folder.");
			}
            return false;
        }

        private static void placeValue(ExcelWorksheet excelWorksheet, string configKey, Object dataToInsert)
        {
            
        }
	}
}
