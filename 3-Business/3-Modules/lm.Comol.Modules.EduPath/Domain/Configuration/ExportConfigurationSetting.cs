using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    public class ExportConfigurationSetting : BaseConfigurationSetting
    {
        public virtual IList<ExportField> Fields { get; set; }
        public virtual StatisticsPageType PageType { get; set; }
        public virtual StatisticType StatisticType { get; set; }
        
        public ExportConfigurationSetting()
        {
            ConfigType = ConfigurationType.Export;
            Fields = new List<ExportField>();
        }

        public virtual ExportField GetField(ExportFieldType fType)
        {
            return (Fields.Any() ? Fields.Where(f => f.Type == fType && f.Deleted == BaseStatusDeleted.None).FirstOrDefault() : null);
        }

        public static List<dtoExportField> GetDefaultDtoFields(StatisticsPageType type)
        {
            List<dtoExportField> fields = new List<dtoExportField>();

            switch(type){
                case StatisticsPageType.AdvancedCommunity:

                    break;
                case StatisticsPageType.AdvancedPath:
                    break;
                case StatisticsPageType.AdvancedUser:
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdUser, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdCommunity, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdPath, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.TaxCodeInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.AgencyInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireCertificationInfo, Availability = ExportFieldTypeAvailability.Certification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireAdvancedInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireAttempts, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type = ExportFieldType.FirstActionOn, Availability = ExportFieldTypeAvailability.Always });
                    break;
                case StatisticsPageType.MyPathStatistic:
                    break;
                case StatisticsPageType.PathStatistic:
                    break;
                case StatisticsPageType.PathSummary:
                    break;
                case StatisticsPageType.UsersStatistics:
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdUser, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdCommunity, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdPath, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.TaxCodeInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.AgencyInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireCertificationInfo, Availability = ExportFieldTypeAvailability.Certification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireAdvancedInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireAttempts, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type = ExportFieldType.RoleInfo, Availability = ExportFieldTypeAvailability.Always });
                    fields.Add(new dtoExportField() { Type = ExportFieldType.FirstActionOn, Availability = ExportFieldTypeAvailability.Always });
                    break;
                case StatisticsPageType.UserStatistic:

                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdUser, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdCommunity, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.IdPath, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.TaxCodeInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.AgencyInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireCertificationInfo, Availability = ExportFieldTypeAvailability.Certification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireAdvancedInfo, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type= ExportFieldType.QuestionnaireAttempts, Availability = ExportFieldTypeAvailability.FullAndCertification});
                    fields.Add(new dtoExportField() { Type = ExportFieldType.RoleInfo, Availability = ExportFieldTypeAvailability.Always });
                    fields.Add(new dtoExportField() { Type = ExportFieldType.FirstActionOn, Availability = ExportFieldTypeAvailability.Always });
                    break;
                case StatisticsPageType.FullPathStatistics:
                    fields.Add(new dtoExportField() { Type = ExportFieldType.IdUser, Availability = ExportFieldTypeAvailability.FullAndCertification });
                    fields.Add(new dtoExportField() { Type = ExportFieldType.TaxCodeInfo, Availability = ExportFieldTypeAvailability.FullAndCertification });
                    fields.Add(new dtoExportField() { Type = ExportFieldType.AgencyInfo, Availability = ExportFieldTypeAvailability.FullAndCertification });
                    fields.Add(new dtoExportField() { Type = ExportFieldType.RoleInfo, Availability = ExportFieldTypeAvailability.Always });
                    fields.Add(new dtoExportField() { Type = ExportFieldType.FirstActionOn, Availability = ExportFieldTypeAvailability.Always });
                    break;
            }
            return fields;
        }
    }

    public class ExportField : lm.Comol.Core.DomainModel.DomainBaseObjectIdLiteMetaInfo<long>
    {
        public virtual long IdConfiguration { get; set; }
        public virtual ExportFieldType Type { get; set; }
        public virtual ExportFieldTypeAvailability Availability { get; set; }
        
        public ExportField()
        {

        }
    }
}