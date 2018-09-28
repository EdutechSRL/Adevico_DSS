using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class dtoConfigurationSetting : dtoBaseConfigurationSetting
    {
        public virtual Boolean AllowDeleteStatistics { get; set; }
        public virtual Boolean AllowDeleteFullStatistics { get; set; }

        public dtoConfigurationSetting()
        {
            ConfigType = ConfigurationType.Module;
            AllowDeleteFullStatistics = false;
            AllowDeleteStatistics = true;
        }
        public dtoConfigurationSetting(ConfigurationSetting setting)
        {
            ConfigType = setting.ConfigType;
            AllowDeleteFullStatistics = setting.AllowDeleteFullStatistics;
            AllowDeleteStatistics = setting.AllowDeleteStatistics;
            Id = setting.Id;
            Path = setting.Path;
            IdCommunity = setting.IdCommunity;
            IdOrganization = setting.IdOrganization;
            IsEnabled = setting.IsEnabled;
            ForAllPath = setting.ForAllPath;
        }
    }
}