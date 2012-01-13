using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SANTA.IO
{
	/// <remarks>
	/// A class that writes information to a log file.
	/// </remarks>
	static class Log
	{
		/// <summary>
		/// Writes a log message to the log file.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="parameters">Parameter list where even entries are parameter names
		/// and odd entries are parameter values.</param>
		private static void WriteLog(String message, String[] parameters) {
			if (parameters.Length % 2 != 0) {
				LogWarning("Invalid number of log parameters.");
			}
			using (StreamWriter writer = new StreamWriter(_configReader.getValue("Log Location"), true)) {
				writer.Write("[" + DateTime.Now + "] " + message);
				if (parameters.Length > 1) {
					writer.Write(" [" + parameters[0] + "=" + parameters[1]);
					for (int i = 1; i < parameters.Length / 2; i ++) {
						writer.Write(", " + parameters[2 * i] + "=" + parameters[2 * i + 1]);
					}
					writer.Write("].");
				}
				writer.WriteLine();
			}
		}

		/// <summary>
		/// Initializes the log by copying the existing log to a backup location.
		/// </summary>
		public static void InitLog() {
			String path = _configReader.getValue("Log Location");
			File.Delete(path + ".temp");
            if (File.Exists(path)) {
			    File.Copy(path, path + ".temp");
			    File.Delete(path);
            }
		}

		/// <summary>
		/// Writes an error message to the log file.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="parameters">Parameter list where even entries are parameter names
		/// and odd entries are parameter values.</param>
		public static void LogError(String message, params String[] parameters) {
			WriteLog("ERROR:  " + message, parameters);
		}

		/// <summary>
		/// Writes a warning message to the log file.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="parameters">Parameter list where even entries are parameter names
		/// and odd entries are parameter values.</param>
		public static void LogWarning(String message, params String[] parameters) {
			WriteLog("WARNING:  " + message, parameters);
		}

		/// <summary>
		/// Writes an informatory message to the log file.
		/// </summary>
		/// <param name="message">The message to write.</param>
		/// <param name="parameters">Parameter list where even entries are parameter names
		/// and odd entries are parameter values.</param>
		public static void LogInfo(String message, params String[] parameters) {
			WriteLog("INFO:  " + message, parameters);
		}

		/// <summary>
		/// A test method for the log file to make sure it writes messages and parameters correctly.
		/// </summary>
		public static void TestLog() {
			LogError("This is a test", "param 1", "value 1", "param 2", "value 2");
			LogWarning("This is a test 2", "param 1", "value 1");
			LogInfo("This is a test 3");
			LogError("This is a test 4", "param 1");
			LogError("This is a test 5", "param 1", "value 1", "param 2");
		}

		/// <summary>
		/// The configuration reader that gives the log file path.
		/// </summary>
		private static ConfigReader _configReader = new ConfigReader();
	}
}
