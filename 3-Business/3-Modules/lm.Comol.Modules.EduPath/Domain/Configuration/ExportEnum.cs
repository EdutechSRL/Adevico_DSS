using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;
using System.ComponentModel;

namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public enum ConfigurationType
    {
        None = 0,
        Module = 1,
        Export = 2,
        TaskService = 3
    }
    [Serializable]
    public enum StatisticType
    {
        UiDefined = 0,
        //UserStatistics = 1,
        UsersStatistics = 2,
        //PathBaseStatistics = 3,
        //PathStatistics = 4,
        //PathCompletionStatistics = 5,
        //PathUsersCompletionStatistics = 6,
    }

    [Serializable]
    public enum StatisticsPageType
    {
        AdvancedUser = 0,
        AdvancedCommunity = 1,
        AdvancedPath = 2,
        UserStatistic = 3,
        UsersStatistics = 4,
        PathStatistic = 5,
        PathSummary = 6,
        MyPathStatistic = 7,
        FullPathStatistics = 8,
        AdvancedPathStatistics = 9,
        FullCertificationsStatistics = 10,
        Unknown = -3,
        Undefined = -2
    }

    [Serializable]
    public enum ExportFieldType
    {
        None = 0,
        IdUser = 1,
        IdCommunity = 2,
        IdRole = 3,
        IdAgency = 4,
        IdPath = 5,
        IdUnit = 6,
        IdActivity = 7,
        IdSubActivity = 8,
        IdQuestionnaire = 9,
        TaxCodeInfo = 10,
        AgencyInfo = 11,
        QuestionnaireAdvancedInfo = 12,
        QuestionnaireCertificationInfo = 13,
        RoleInfo = 14,
        QuestionnaireAttempts = 15,
        CommunityName = 18,
        FirstAccessOn = 19,
        FirstActionOn = 20,
        QuestionnaireFullAttempts = 21,
        OrganizationName = 16,
        IdOrganization = 17,
    }

    [Serializable]
    public enum ExportFieldTypeAvailability
    {
        Normal = 1,
        Certification = 2,
        FullData = 4,
        FullAndCertification = 6,
        Always = 8,
    }

    [Serializable]
    public enum ExporPathData
    {
        Normal = 1,
        Full = 2,
        Certification = 4
    }
}