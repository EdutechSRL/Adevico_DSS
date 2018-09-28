using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.EduPath.Domain
{
    [Serializable]
    public class dtoExportStatistics
    {
        public virtual DateTime? StatisticsDate { get; set; }
        public virtual Boolean isAutoPath { get; set; }
        public virtual Boolean isTimePath { get; set; }
        public virtual lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction QuizAction { get; set; }
        public virtual lm.Comol.Core.ModuleLinks.IViewModuleRenderAction RepositoryAction { get; set; }

        public virtual lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction TextAction { get; set; }
        public virtual lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction CertAction { get; set; }
        public virtual dtoEpGlobalStat PathStatistics { get; set; }
        public virtual dtoListUserStat UsersStatistics { get; set; }
        public virtual dtoSubActListUserStat SubActivityStatistics { get; set; }
        public virtual dtoSubActivity SubActivity { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual String Filename { get; set; }
        public virtual String ClientFilename { get; set; }
        public virtual litePerson PrintBy { get; set; }
        public virtual dtoExportConfigurationSetting ExportSettings { get; set; }
        public virtual ExporPathData ExportData { get; set; }

        public dtoExportStatistics() { }

    }

    [Serializable]
    public class dtoUserAdvancedExportStatistics
    {
        public virtual Int32 IdUser { get; set; }
        public virtual litePerson User { get; set; }

        public virtual DateTime? StatisticsDate { get; set; }
        public virtual dtoUserPaths Statistics { get; set; }
        public virtual List<dtoUserPathQuiz> QuestionnaireInfos { get; set; }

        public virtual String Filename { get; set; }
        public virtual String ClientFilename { get; set; }
        public virtual litePerson PrintBy { get; set; }

        public dtoUserAdvancedExportStatistics() { }

    }

    [Serializable]
    public class dtoUserPathExportStatistics
    {
        public virtual Int32 IdUser { get; set; }
        public virtual Int32 IdCurrentUser { get; set; }
        public virtual DateTime StatisticsDate { get; set; }
        public virtual dtoItemWithStatistic Statistics { get; set; }
        public virtual lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction QuizAction { get; set; }
        public virtual lm.Comol.Core.ModuleLinks.IViewModuleRenderAction RepositoryAction { get; set; }
        public virtual lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction TextAction { get; set; }
        public virtual lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction CertAction { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual long IdPath { get; set; }

        public virtual String Filename { get; set; }
        public virtual String ClientFilename { get; set; }
        public virtual litePerson PrintBy { get; set; }

        public dtoUserPathExportStatistics() { }

    }
}