using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Threading;

namespace SANTA.IO
{
	/// <remarks>
	/// A class that performs IO operations on a Microsoft Access database.
	/// </remarks>
	static class MSAccess
	{
		/// <summary>
		/// Sets a string parameter to an <c>OleDbCommand</c>.  If the parameter value is null, it is replaced with the
		/// empty string.
		/// </summary>
		/// <param name="command">The SQL <c>OleDbCommand</c>.</param>
		/// <param name="parameterName">The relational database attribute (column) name.</param>
		/// <param name="parameterValue">The value to put in the field.</param>
		private static void setStringParameter(OleDbCommand command, String parameterName,
				String parameterValue) {
			command.Parameters.Add(parameterName, OleDbType.VarWChar).Value = (parameterValue == null ? "" : parameterValue);
		}

		/// <summary>
		/// Opens a connection to the database specified in the <c>OleDbConnection</c> <paramref name="conn"/>.
		/// </summary>
		/// <param name="conn">The connection to open.</param>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		private static void openConnection(OleDbConnection conn) {
			try {
				conn.Open();
			} catch (InvalidOperationException) {
				// The connection is already open, so we simply log it and continue
				Log.LogWarning("Tried to open an already-open connection.");
			} catch (OleDbException e) {
				Log.LogError("Could not open the MSAccess database.");
				throw e;
			}
		}

		/// <summary>
		/// Adds a caliber unit to the database.
		/// </summary>
		/// <param name="caliberUnit">The <c>CaliberUnit</c> to add to the database.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		/// <exception cref="InvalidOperationException">The INSERT INTO Caliber command failed.</exception>
		public static void AddCaliberUnit(Datatype.CaliberUnit caliberUnit, ConfigReader configReader) {
            using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location")))
            {
				openConnection(conn);

				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "INSERT INTO Caliber (UnitName, UnitsPerInch) VALUES (?, ?)";
					command.Prepare();
					setStringParameter(command, "UnitName", caliberUnit.unitName);
					command.Parameters.Add("UnitsPerInch", OleDbType.Double).Value = caliberUnit.unitsPerInch;

					try {
						command.ExecuteNonQuery();
					} catch (InvalidOperationException e) {
						Log.LogError("Could not perform INSERT INTO Caliber.", "SQL", command.CommandText);
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// Adds a weapon name to the database.
		/// </summary>
		/// <param name="weaponName">The weapon name to add to the database.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		/// <exception cref="InvalidOperationException">The INSERT INTO Weapon command failed.</exception>
		public static void AddWeaponName(String weaponName, ConfigReader configReader) {
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);

				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "INSERT INTO Weapon (WeaponName) VALUES (?)";
					command.Prepare();
					setStringParameter(command, "WeaponName", weaponName);

					try {
						command.ExecuteNonQuery();
					} catch (InvalidOperationException e) {
						Log.LogError("Could not perform INSERT INTO Weapon.", "SQL", command.CommandText);
						throw e;
					}
				}
			}
		}

		/// <summary>
		/// Returns all of the caliber options already in the database with their IDs.
		/// </summary>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <returns>A Hashtable where the keys are the caliberIDs as ints and the values are
		/// caliber names as Strings.</returns>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		public static List<Datatype.CaliberUnit> GetCaliberUnits(ConfigReader configReader) {
			List<Datatype.CaliberUnit> calibers = new List<Datatype.CaliberUnit>();
            using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location")))
            {
				openConnection(conn);

				using (OleDbCommand command = new OleDbCommand("SELECT * FROM Caliber", conn)) {
					using (OleDbDataReader reader = command.ExecuteReader()) {
						while (reader.Read()) {
							// We could trust that the columns are index 0, 1, and 2,
							// but let's use the specific column name for safety.
							int caliberUnitID = reader.GetInt32(reader.GetOrdinal("CaliberID"));
							String unitName = reader.GetString(reader.GetOrdinal("UnitName"));
							double unitsPerInch = reader.GetDouble(reader.GetOrdinal("UnitsPerInch"));
							calibers.Add(new Datatype.CaliberUnit(caliberUnitID, unitName, unitsPerInch));
						}
					}
				}
			}

			return calibers;
		}

		/// <summary>
		/// Loads a <c>CaliberUnit</c> from the database.
		/// </summary>
		/// <param name="caliberID">The <paramref name="caliberID"/> of the <c>CaliberUnit</c>.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <returns>The <c>CaliberUnit</c> from the database.</returns>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		private static Datatype.CaliberUnit GetCaliberUnit(int caliberID, ConfigReader configReader) {
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);
				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "SELECT * FROM Caliber WHERE CaliberID = ?";
					command.Prepare();
					command.Parameters.Add("CaliberID", OleDbType.Integer).Value = caliberID;
					using (OleDbDataReader reader = command.ExecuteReader()) {
						if (reader.Read()) {
							// We could trust that the columns are index 0, 1, and 2,
							// but let's use the specific column name for safety.
							int caliberUnitID = reader.GetInt32(reader.GetOrdinal("CaliberID"));
							String unitName = reader.GetString(reader.GetOrdinal("UnitName"));
							double unitsPerInch = reader.GetDouble(reader.GetOrdinal("UnitsPerInch"));
							if (caliberID != caliberUnitID) {
								Log.LogError("IDs do not match in GetCaliberUnit().", "caliberID", caliberID.ToString(),
									"caliberUnitID", caliberUnitID.ToString());
							}
							return new Datatype.CaliberUnit(caliberUnitID, unitName, unitsPerInch);
						}
					}
				}
			}
			Log.LogError("Caliber unit not found in database.", "unitID", caliberID.ToString());
			throw new Exception("Error in GetCaliberUnit()");
		}

		/// <summary>
		/// Loads a list of weapon names from the database.
		/// </summary>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <returns>A list of all weapon names in the database.</returns>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		public static List<String> GetWeaponNames(ConfigReader configReader) {
			List<String> weaponNames = new List<String>();
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);

				using (OleDbCommand command = new OleDbCommand("SELECT WeaponName FROM Weapon", conn)) {
					using (OleDbDataReader reader = command.ExecuteReader()) {
						while (reader.Read()) {
							// We could trust that the column is index 0,
							// but let's use the specific column name for safety.
							weaponNames.Add(reader.GetString(reader.GetOrdinal("WeaponName")));
						}
					}
				}
			}

			return weaponNames;
		}

		/// <summary>
		/// Returns true if the given <c>caliberID</c> is in the database.
		/// </summary>
		/// <param name="caliberID">The <c>caliberID</c> to find.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <returns>True if the given <c>caliberID</c> is in the database.</returns>
		private static Boolean CaliberIDExists(int caliberID, ConfigReader configReader) {
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);
				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "SELECT CaliberID FROM Caliber WHERE CaliberID = ?";
					command.Prepare();
					command.Parameters.Add("CaliberID", OleDbType.Integer).Value = caliberID;
					using (OleDbDataReader reader = command.ExecuteReader()) {
						return reader.HasRows;
					}
				}
			}
		}

		/// <summary>
		/// Returns true if the given <c>weaponName</c> is in the database.
		/// </summary>
		/// <param name="caliberID">The <c>weaponName</c> to find.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <returns>True if the given <c>weaponName</c> is in the database.</returns>
		private static Boolean WeaponExists(String weaponName, ConfigReader configReader) {
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);
				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "SELECT WeaponName FROM Weapon WHERE WeaponName = ?";
					command.Prepare();
					setStringParameter(command, "WeaponName", weaponName);
					using (OleDbDataReader reader = command.ExecuteReader()) {
						return reader.HasRows;
					}
				}
			}
		}

		/// <summary>
		/// Inserts an <c>ImageData</c> object into the <c>Target</c> table.
		/// </summary>
		/// <param name="imageData">The <c>ImageData</c> to insert.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		/// <exception cref="InvalidOperationException">The INSERT INTO Target command failed.</exception>
		/// <exception cref="ArgumentException">The caliber or weapon name was not found in the database.</exception>
		/// <returns>The <c>targetID</c> of the newly-inserted record.</returns>
		private static int InsertData(Datatype.ImageData imageData, ConfigReader configReader) {
			int targetID = -1;
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);

				if (!CaliberIDExists(imageData.caliber.caliberUnitID, configReader)) {
					Log.LogError("Caliber to insert not found in database.", "unitID",
						imageData.caliber.caliberUnitID.ToString(), "unitName", imageData.caliber.unitName,
						"unitsPerInch", imageData.caliber.unitsPerInch.ToString());
					throw new ArgumentException("Caliber to insert does not exist.", "caliber");
				}
				if (!WeaponExists(imageData.weaponName, configReader)) {
					Log.LogError("Weapon name to insert not found in database.", "weaponName", imageData.weaponName);
					throw new ArgumentException("Weapon name to insert not found in database.", "weaponName");
				}

				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "INSERT INTO Target (OrigFilename, ReportFilename, DateTimeFired, DateTimeProcessed, ShooterLName, ShooterFName, RangeLocation, DistanceUnits, Distance, Temperature, WeaponName, SerialNumber, WeaponNotes, CaliberID, CaliberValue, LotNumber, ProjectileMassGrains, AmmunitionNotes, ShotsFired, ShotLocations, Scale, ROI) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
					command.Prepare();
					setStringParameter(command, "OrigFilename", imageData.origFilename);
					setStringParameter(command, "ReportFilename", imageData.reportFilename);
					command.Parameters.Add("DateTimeFired", OleDbType.Date).Value = imageData.dateTimeFired;
					command.Parameters.Add("DateTimeProcessed", OleDbType.Date).Value = DateTime.Now;
					setStringParameter(command, "ShooterLName", imageData.shooterLName);
					setStringParameter(command, "ShooterFName", imageData.shooterFName);
					setStringParameter(command, "RangeLocation", imageData.rangeLocation);
					command.Parameters.Add("DistanceUnits", OleDbType.UnsignedTinyInt).Value = imageData.distanceUnits;
					command.Parameters.Add("Distance", OleDbType.Integer).Value = imageData.distance;
					command.Parameters.Add("Temperature", OleDbType.UnsignedTinyInt).Value = imageData.temperature;
					setStringParameter(command, "WeaponName", imageData.weaponName);
					setStringParameter(command, "SerialNumber", imageData.serialNumber);
					command.Parameters.Add("WeaponNotes", OleDbType.LongVarWChar).Value = imageData.weaponNotes == null ? "" : imageData.weaponNotes;
					command.Parameters.Add("CaliberID", OleDbType.Integer).Value = imageData.caliber.caliberUnitID;
					command.Parameters.Add("CaliberValue", OleDbType.Double).Value = imageData.caliberValue;
					setStringParameter(command, "LotNumber", imageData.lotNumber);
					command.Parameters.Add("ProjectileMassGrains", OleDbType.Integer).Value = imageData.projectileMassGrains;
					command.Parameters.Add("AmmunitionNotes", OleDbType.LongVarWChar).Value = imageData.ammunitionNotes == null ? "" : imageData.ammunitionNotes;
					command.Parameters.Add("ShotsFired", OleDbType.Integer).Value = imageData.shotsFired;

					StringBuilder pointStringBuilder = new StringBuilder();
					foreach (Point point in imageData.points) {
						pointStringBuilder.Append(point.X);
						pointStringBuilder.Append(",");
						pointStringBuilder.Append(point.Y);
						pointStringBuilder.Append(",");
					}
					command.Parameters.Add("ShotLocations", OleDbType.LongVarWChar).Value = pointStringBuilder.ToString();

					StringBuilder scaleStringBuilder = new StringBuilder();
					scaleStringBuilder.Append(imageData.scale.horizontal.X);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.horizontal.Y);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.middle.X);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.middle.Y);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.vertical.X);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.vertical.Y);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.horizontalLength);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.verticalLength);
					setStringParameter(command, "Scale", scaleStringBuilder.ToString());

					StringBuilder ROIStringBuilder = new StringBuilder();
					ROIStringBuilder.Append(imageData.regionOfInterest.topLeft.X);
					ROIStringBuilder.Append(",");
					ROIStringBuilder.Append(imageData.regionOfInterest.topLeft.Y);
					ROIStringBuilder.Append(",");
					ROIStringBuilder.Append(imageData.regionOfInterest.bottomRight.X);
					ROIStringBuilder.Append(",");
					ROIStringBuilder.Append(imageData.regionOfInterest.bottomRight.Y);
					setStringParameter(command, "ROI", ROIStringBuilder.ToString());

					try {
						command.ExecuteNonQuery();
					} catch (InvalidOperationException e) {
						Log.LogError("Could not perform INSERT INTO Target.", "SQL", command.CommandText);
						throw e;
					}
				}

				using (OleDbCommand command = new OleDbCommand("SELECT MAX(TargetID) FROM Target", conn)) {
					using (OleDbDataReader reader = command.ExecuteReader()) {
						if (reader.Read()) {
							targetID = reader.GetInt32(0);
						} else {
							Log.LogError("Unable to get the maximum TargetID.", "SQL", command.CommandText);
						}
					}
				}
			}

			return targetID;
		}

		/// <summary>
		/// Updates an existing <c>ImageData</c> object in the <c>Target</c> table.
		/// </summary>
		/// <param name="imageData">The <c>ImageData</c> to update.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		/// <exception cref="InvalidOperationException">The UPDATE Target command failed.</exception>
		/// <exception cref="ArgumentException">The caliber or weapon name was not found in the database.</exception>
		/// <returns>The <c>targetID</c> of the updated record.</returns>
		private static int UpdateData(Datatype.ImageData imageData, ConfigReader configReader) {
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);

				if (!CaliberIDExists(imageData.caliber.caliberUnitID, configReader)) {
					Log.LogError("Caliber to insert not found in database.", "unitID",
						imageData.caliber.caliberUnitID.ToString(), "unitName", imageData.caliber.unitName,
						"unitsPerInch", imageData.caliber.unitsPerInch.ToString());
					throw new ArgumentException("Caliber to insert does not exist.", "caliber");
				}
				if (!WeaponExists(imageData.weaponName, configReader)) {
					Log.LogError("Weapon name to insert not found in database.", "weaponName", imageData.weaponName);
					throw new ArgumentException("Weapon name to insert not found in database.", "weaponName");
				}

				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "UPDATE Target SET OrigFilename = ?, ReportFilename = ?, DateTimeFired = ?, DateTimeProcessed = ?, ShooterLName = ?, ShooterFName = ?, RangeLocation = ?, DistanceUnits = ?, Distance = ?, Temperature = ?, WeaponName = ?, SerialNumber = ?, WeaponNotes = ?, CaliberID = ?, CaliberValue = ?, LotNumber = ?, ProjectileMassGrains = ?, AmmunitionNotes = ?, ShotsFired = ?, ShotLocations = ?, Scale = ?, ROI = ? WHERE TargetID = ?";
					command.Prepare();
					setStringParameter(command, "OrigFilename", imageData.origFilename);
					setStringParameter(command, "ReportFilename", imageData.reportFilename);
					command.Parameters.Add("DateTimeFired", OleDbType.Date).Value = imageData.dateTimeFired;
					command.Parameters.Add("DateTimeProcessed", OleDbType.Date).Value = DateTime.Now;
					setStringParameter(command, "ShooterLName", imageData.shooterLName);
					setStringParameter(command, "ShooterFName", imageData.shooterFName);
					setStringParameter(command, "RangeLocation", imageData.rangeLocation);
					command.Parameters.Add("DistanceUnits", OleDbType.UnsignedTinyInt).Value = imageData.distanceUnits;
					command.Parameters.Add("Distance", OleDbType.Integer).Value = imageData.distance;
					command.Parameters.Add("Temperature", OleDbType.UnsignedTinyInt).Value = imageData.temperature;
					setStringParameter(command, "WeaponName", imageData.weaponName);
					setStringParameter(command, "SerialNumber", imageData.serialNumber);
					command.Parameters.Add("WeaponNotes", OleDbType.LongVarWChar).Value = imageData.weaponNotes;
					command.Parameters.Add("CaliberID", OleDbType.Integer).Value = imageData.caliber.caliberUnitID;
					command.Parameters.Add("CaliberValue", OleDbType.Double).Value = imageData.caliberValue;
					setStringParameter(command, "LotNumber", imageData.lotNumber);
					command.Parameters.Add("ProjectileMassGrains", OleDbType.Integer).Value = imageData.projectileMassGrains;
					command.Parameters.Add("AmmunitionNotes", OleDbType.LongVarWChar).Value = imageData.ammunitionNotes;
					command.Parameters.Add("ShotsFired", OleDbType.Integer).Value = imageData.shotsFired;

					StringBuilder pointStringBuilder = new StringBuilder();
					foreach (Point point in imageData.points) {
						pointStringBuilder.Append(point.X);
						pointStringBuilder.Append(",");
						pointStringBuilder.Append(point.Y);
						pointStringBuilder.Append(",");
					}
					command.Parameters.Add("ShotLocations", OleDbType.LongVarWChar).Value = pointStringBuilder.ToString();

					StringBuilder scaleStringBuilder = new StringBuilder();
					scaleStringBuilder.Append(imageData.scale.horizontal.X);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.horizontal.Y);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.middle.X);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.middle.Y);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.vertical.X);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.vertical.Y);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.horizontalLength);
					scaleStringBuilder.Append(",");
					scaleStringBuilder.Append(imageData.scale.verticalLength);
					setStringParameter(command, "Scale", scaleStringBuilder.ToString());

					StringBuilder ROIStringBuilder = new StringBuilder();
					ROIStringBuilder.Append(imageData.regionOfInterest.topLeft.X);
					ROIStringBuilder.Append(",");
					ROIStringBuilder.Append(imageData.regionOfInterest.topLeft.Y);
					ROIStringBuilder.Append(",");
					ROIStringBuilder.Append(imageData.regionOfInterest.bottomRight.X);
					ROIStringBuilder.Append(",");
					ROIStringBuilder.Append(imageData.regionOfInterest.bottomRight.Y);
					setStringParameter(command, "ROI", ROIStringBuilder.ToString());

					command.Parameters.Add("TargetID", OleDbType.Integer).Value = imageData.targetID;

					try {
						command.ExecuteNonQuery();
					} catch (InvalidOperationException e) {
						Log.LogError("Could not perform UPDATE Target.", "SQL", command.CommandText);
						throw e;
					}
				}
			}

			return imageData.targetID;
		}

		/// <summary>
		/// Saves an <c>ImageData</c> object to the <c>Target</c> table.
		/// </summary>
		/// <param name="imageData">The <c>ImageData</c> to save.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		/// <exception cref="InvalidOperationException">The SQL command failed.</exception>
		/// <exception cref="ArgumentException">The caliber or weapon name was not found in the database.</exception>
		/// <returns>The <c>targetID</c> of the saved record.</returns>
		public static int SaveData(Datatype.ImageData imageData, ConfigReader configReader) {
			if (imageData.targetID == -1) {
				return InsertData(imageData, configReader);
			} else {
				return UpdateData(imageData, configReader);
			}
		}

		/// <summary>
		/// Loads an <c>ImageData</c> from the database.
		/// </summary>
		/// <param name="targetID">The <paramref name="targetID"/> of the target to load.</param>
		/// <param name="configReader">The <c>ConfigReader</c> with the path to the database.</param>
		/// <returns>The <c>ImageData</c> corresponding to the <paramref name="targetID"/>.</returns>
		/// <exception cref="OleDbException">A connection-level error occurred while opening the connection.</exception>
		public static Datatype.ImageData LoadData(int targetID, ConfigReader configReader) {
			Datatype.ImageData imageData = new Datatype.ImageData();
			using (OleDbConnection conn = new OleDbConnection(configReader.getValue("Database Location"))) {
				openConnection(conn);

				using (OleDbCommand command = new OleDbCommand()) {
					command.Connection = conn;
					command.CommandText = "SELECT * FROM Target WHERE TargetID = ?";
					command.Prepare();
					command.Parameters.Add("TargetID", OleDbType.Integer).Value = targetID;

					using (OleDbDataReader reader = command.ExecuteReader()) {
						if (reader.Read()) {
							imageData.targetID = reader.GetInt32(reader.GetOrdinal("TargetID"));
							if (imageData.targetID != targetID) {
								throw new Exception("Error in LoadData()");
							}

							imageData.origFilename = reader.GetString(reader.GetOrdinal("OrigFilename"));
							imageData.reportFilename = reader.GetString(reader.GetOrdinal("ReportFilename"));
							imageData.dateTimeFired = reader.GetDateTime(reader.GetOrdinal("DateTimeFired"));
							imageData.dateTimeProcessed = reader.GetDateTime(reader.GetOrdinal("DateTimeProcessed"));
							imageData.shooterLName = reader.GetString(reader.GetOrdinal("ShooterLName"));
							imageData.shooterFName = reader.GetString(reader.GetOrdinal("ShooterFName"));
							imageData.rangeLocation = reader.GetString(reader.GetOrdinal("RangeLocation"));
							imageData.distanceUnits = (Datatype.UnitsOfMeasure)reader.GetByte(reader.GetOrdinal("DistanceUnits"));
							imageData.distance = reader.GetInt32(reader.GetOrdinal("Distance"));
							imageData.temperature = (Datatype.ImageData.Temperature)reader.GetByte(reader.GetOrdinal("Temperature"));
							imageData.weaponName = reader.GetString(reader.GetOrdinal("WeaponName"));
							imageData.serialNumber = reader.GetString(reader.GetOrdinal("SerialNumber"));
							imageData.weaponNotes = reader.GetString(reader.GetOrdinal("WeaponNotes"));
							imageData.caliber = GetCaliberUnit(reader.GetInt32(reader.GetOrdinal("CaliberID")), configReader);
							imageData.caliberValue = reader.GetDouble(reader.GetOrdinal("CaliberValue"));
							imageData.lotNumber = reader.GetString(reader.GetOrdinal("LotNumber"));
							imageData.projectileMassGrains = reader.GetInt32(reader.GetOrdinal("ProjectileMassGrains"));
							imageData.ammunitionNotes = reader.GetString(reader.GetOrdinal("AmmunitionNotes"));
							imageData.shotsFired = reader.GetInt32(reader.GetOrdinal("ShotsFired"));

							String points = reader.GetString(reader.GetOrdinal("ShotLocations"));
							String[] pointElements = points.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							if (pointElements.Length % 2 != 0) {
								Log.LogWarning("A Points field has invalid values.", "targetID", targetID.ToString(),
									"SQL", command.CommandText);
							} else {
								for (int i = 0; i < pointElements.Length; i += 2) {
									imageData.points.Add(new Point(Int32.Parse(pointElements[i]), Int32.Parse(pointElements[i + 1])));
								}
							}

							String scale = reader.GetString(reader.GetOrdinal("Scale"));
							String[] scaleElements = scale.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							if (scaleElements.Length != 8) {
								Log.LogWarning("A Scale field has invalid values.", "targetID", targetID.ToString(),
									"SQL", command.CommandText);
							} else {
								imageData.scale.horizontal = new Point(Int32.Parse(scaleElements[0]), Int32.Parse(scaleElements[1]));
								imageData.scale.middle = new Point(Int32.Parse(scaleElements[2]), Int32.Parse(scaleElements[3]));
								imageData.scale.vertical = new Point(Int32.Parse(scaleElements[4]), Int32.Parse(scaleElements[5]));
								imageData.scale.horizontalLength = float.Parse(scaleElements[6]);
								imageData.scale.verticalLength = float.Parse(scaleElements[7]);
							}

							String ROI = reader.GetString(reader.GetOrdinal("ROI"));
							String[] ROIElements = ROI.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							if (ROIElements.Length != 4) {
								Log.LogWarning("An ROI field has invalid values.", "targetID", targetID.ToString(),
									"SQL", command.CommandText);
							} else {
								imageData.regionOfInterest.topLeft = new Point(Int32.Parse(ROIElements[0]), Int32.Parse(ROIElements[1]));
								imageData.regionOfInterest.bottomRight = new Point(Int32.Parse(ROIElements[2]), Int32.Parse(ROIElements[3]));
							}
						} else {
							Log.LogInfo("Target ID not found.", "targetID", targetID.ToString(), "SQL", command.CommandText);
							return null;
						}
					}
				}
			}

			return imageData;
		}
	}
}
