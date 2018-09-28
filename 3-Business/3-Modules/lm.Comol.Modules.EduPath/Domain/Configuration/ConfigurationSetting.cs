using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class ConfigurationSetting : BaseConfigurationSetting
    {
        public virtual Boolean AllowDeleteStatistics { get; set; }
        public virtual Boolean AllowDeleteFullStatistics { get; set; }

        public ConfigurationSetting()
        {
            ConfigType = ConfigurationType.Module;
            AllowDeleteFullStatistics = false;
            AllowDeleteStatistics = false;
        }
    }
}