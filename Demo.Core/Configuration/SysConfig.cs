using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Common;

namespace Demo.Core.Configuration
{
    public class SysConfig
    {

        /// <summary>
        /// Indicates whether we should ignore startup tasks
        /// </summary>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// Path to database with user agent strings
        /// </summary>
        public string UserAgentStringsPath { get; set; }

        /// <summary>
        /// Indicates whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; set; }
        /// <summary>
        /// Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; set; }

        /// <summary>
        /// A value indicating whether the site is run on multiple instances (e.g. web farm, Windows Azure with multiple instances, etc).
        /// Do not enable it if you run on Azure but use one instance only
        /// </summary>
        public bool MultipleInstancesEnabled { get; set; }

        /// <summary>
        /// A value indicating whether a store owner can install sample data during installation
        /// </summary>
        public bool DisableSampleDataDuringInstallation { get; set; }
        /// <summary>
        /// By default this setting should always be set to "False" (only for advanced users)
        /// </summary>
        public bool UseFastInstallationService { get; set; }

        /// <summary>
        /// A list of plugins ignored during system installation
        /// </summary>
        public string PluginsIgnoredDuringInstallation { get; set; }


        private static string fileName = "Sys.Config";

        public static void SaveInstance(SysConfig config)
        {
            string filePath = PathUtils.MapPath(fileName);
            ConfigManager.SaveCustomConfig<SysConfig>(filePath, config);
        }


        public static SysConfig GetInstance()
        {
            string filePath = PathUtils.MapPath(fileName);
            return ConfigManager.LoadCustomConfig<SysConfig>(filePath);
        }
    }
}
