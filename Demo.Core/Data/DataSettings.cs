using System;
using System.Collections.Generic;

namespace Demo.Core.Data
{
    /// <summary>
    /// Data settings (connection string information)
    /// </summary>
    public partial class DataSettings
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public DataSettings()
        {
             
        }

        /// <summary>
        /// Data provider
        /// </summary>
        public string DataProvider { get; set; }

        /// <summary>
        /// Connection string
        /// </summary>
        public string DataConnectionString { get; set; }
        
        /// <summary>
        /// A value indicating whether entered information is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.DataProvider) && !String.IsNullOrEmpty(this.DataConnectionString);
        }
    }
}
