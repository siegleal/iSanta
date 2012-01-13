using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

//TODO: If key does not exist, throw exception.

namespace SANTA.IO
{
    public class ConfigReader
    {

        public ConfigReader()
        {

        }

        public HashSet<string> getConfigValue(string[] values)
        {
            HashSet<string> returnVal = new HashSet<string>();

            for (int c = 0; c < values.Length; c++ )
            {
                returnVal.Add(getValue(values[c]));
            }
            return returnVal;
        }

        /// <summary>
        /// Determines if the config file has the specified key.  Compares it to the default config file.
        /// </summary>
        /// <param name="key">Name of the key to check</param>
        /// <returns>Returns true if the key is found, false otherwise.</returns>
        public bool hasValue(string key)
        {
            List<string> allKeys = new List<string>(ConfigurationManager.AppSettings.AllKeys);

            return allKeys.Contains(key);
        }

        /// <summary>
        /// This method allows the program to access the configuration file using a single interface.
        /// Checks the configuration file for the key given and returns either the value, if found, or
        /// an exception should the key not exist.
        /// </summary>
        /// <param name="key">Key to check the value of.</param>
        /// <returns>Returns the value held by the key if it exists, else it throws a KeyNotFoundException</returns>
        public string getValue(string key)
        {
            if (hasValue(key))
            {
                ConfigSettings.loadConfigDocument();
                ConfigurationManager.RefreshSection("appSettings");
                return ConfigSettings.ReadSetting(key);
            } 

            throw new KeyNotFoundException();
        }

        public bool setValue(string key, string value)
        {
            ConfigSettings.loadConfigDocument();
            ConfigSettings.WriteSetting(key, value);
            return true;
        }

    }
}
