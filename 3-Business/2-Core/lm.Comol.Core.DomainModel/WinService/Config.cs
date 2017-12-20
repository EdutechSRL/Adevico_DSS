using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.WinService.Configurations
{
    [Serializable]
    public class Config
    {
        /// <summary>
        /// Name to Display on "Windows Services View"
        /// </summary>
        public String ServiceName { get; set; }
        /// <summary>
        /// Description to Display on "Windows Services View"
        /// </summary>
        public String ServiceDescription { get; set; }

        public Config()
        {
            Guid id = Guid.NewGuid();
            ServiceName = "Service_" + id.ToString();
            ServiceDescription = "Description_" + id.ToString();
        }
    }
}
