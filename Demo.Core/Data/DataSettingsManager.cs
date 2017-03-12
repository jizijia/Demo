using System;
using System.Collections.Generic;
using System.IO;
using Demo.Core.Common;

namespace Demo.Core.Data
{
    /// <summary>
    /// Manager of data settings (connection string)
    /// </summary>
    public partial class DataSettingsManager
    {
        protected const string filename = "Db.Config";

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="filePath">File path; pass null to use default settings file path</param>
        /// <returns></returns>
        public virtual DataSettings LoadSettings(string filePath = null)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                filePath = PathUtils.MapPath(filename);
            }
            if (File.Exists(filePath))
            {
                DataSettings dataSetting = Demo.Core.Configuration.ConfigManager.LoadCustomConfig<DataSettings>(filePath);
                if (dataSetting!= null)
                {
                    return dataSetting;
                }
            }
            return new DataSettings();
        }

        /// <summary>
        /// Save settings to a file
        /// </summary>
        /// <param name="settings"></param>
        public virtual void SaveSettings(DataSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            string filePath = PathUtils.MapPath(filename);
            Demo.Core.Configuration.ConfigManager.SaveCustomConfig<DataSettings>(filePath, settings);
        }
    }
}
