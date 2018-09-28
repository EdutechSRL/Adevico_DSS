using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class dtoExportConfigurationSetting : dtoBaseConfigurationSetting
    {
        public virtual List<dtoExportField> Fields { get; set; }
        public virtual StatisticsPageType PageType { get; set; }
        public virtual StatisticType StatisticType { get; set; }
        
        public dtoExportConfigurationSetting()
        {
            ConfigType = ConfigurationType.Export;
            Fields = new List<dtoExportField>();
        }
        public dtoExportConfigurationSetting(ExportConfigurationSetting setting)
        {
            ConfigType = setting.ConfigType;
            if (setting.Fields.Any() && setting.Fields.Where(f => f.Deleted == BaseStatusDeleted.None).Any())
                Fields = setting.Fields.Where(f => f.Deleted == BaseStatusDeleted.None).Select(f => new dtoExportField(f)).ToList();
            else
                Fields = new List<dtoExportField>();

            PageType = setting.PageType;
            StatisticType = setting.StatisticType;
            Id = setting.Id;
            Path = setting.Path;
            IdCommunity = setting.IdCommunity;
            IdOrganization = setting.IdOrganization;
            IsEnabled = setting.IsEnabled;
            ForAllPath = setting.ForAllPath;
        }

        public virtual Boolean isRequiredField(ExportFieldType fType, ExporPathData exportData)
        {
            Boolean found = false;
            if (Fields.Any())
                found = Fields.Where(f => f.Type == fType && f.isAvailable(exportData)).Any(); 
            return found;
        }
        public virtual Boolean HasRequiredIdentiFiers(ExporPathData exportData)
        {
            Boolean found = false;
            if (Fields.Any())
                found = Fields.Where(f => (f.Type == ExportFieldType.IdUser || f.Type == ExportFieldType.IdUnit || f.Type == ExportFieldType.IdSubActivity || f.Type == ExportFieldType.IdRole || f.Type == ExportFieldType.IdQuestionnaire
            || f.Type == ExportFieldType.IdPath || f.Type == ExportFieldType.IdCommunity || f.Type == ExportFieldType.IdActivity) && f.isAvailable(exportData)).Any();
            return found;
        }
        public static dtoExportConfigurationSetting GetDefaultSetting(StatisticsPageType pagetype, ConfigurationType configurationType, StatisticType statisticType)
        {
            dtoExportConfigurationSetting result = new dtoExportConfigurationSetting();
            result.PageType = pagetype;
            result.ConfigType = configurationType;
            result.StatisticType = statisticType;
            result.Fields = ExportConfigurationSetting.GetDefaultDtoFields(pagetype);
            return result;
        }
    }

    public class dtoExportField
    {
        public virtual long Id { get; set; }
        public virtual long IdConfiguration { get; set; }
        public virtual ExportFieldType Type { get; set; }
        public virtual ExportFieldTypeAvailability Availability { get; set; }

        public dtoExportField()
        {

        }
        public dtoExportField(ExportField field)
        {
            Id = field.Id;
            IdConfiguration = field.IdConfiguration;
            Type = field.Type;
            Availability = field.Availability;
        }

        public virtual Boolean isAvailable(ExporPathData exportData)
        {
            switch (exportData)
            {
                case ExporPathData.Normal:
                    return (Availability == ExportFieldTypeAvailability.Always || Availability== ExportFieldTypeAvailability.Normal);
                case ExporPathData.Full:
                    return (Availability == ExportFieldTypeAvailability.Always || Availability== ExportFieldTypeAvailability.FullData  || Availability== ExportFieldTypeAvailability.FullAndCertification);
                case ExporPathData.Certification:
                    return (Availability == ExportFieldTypeAvailability.Always || Availability == ExportFieldTypeAvailability.Certification || Availability == ExportFieldTypeAvailability.FullAndCertification);
                default:
                    return false;
            }
        }
    }
}