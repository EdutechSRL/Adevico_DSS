
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public class HelperExportTo
    {
        private lm.Comol.Core.Business.BaseModuleManager Manager { get; set; }
        //public HelperExportToCsv() : base() {
        //    CommonTranslations = new Dictionary<EduPathTranslations, string>();
        //}
        public HelperExportTo(lm.Comol.Core.Business.BaseModuleManager manager)
            : base()
        {
            Manager = manager;
        }
        public Dictionary<CellType, Boolean> GetAvaliableCells(Int32 idUser, litePerson user, DateTime nDate, dtoUserPaths statistics, List<dtoUserPathQuiz> qInfos, dtoExportConfigurationSetting settings, ExporPathData exportData)
            {
                Dictionary<CellType, Boolean> cells = GetAllCells();
                cells[CellType.PathName] = true;
                cells[CellType.Auto] =  statistics.Paths.Where(p => CheckEpType(p.PathType, EPType.Auto)).Any();
                cells[CellType.Time] =  statistics.Paths.Where(p => CheckEpType(p.PathType, EPType.Time)).Any();
                cells[CellType.Mark] =  statistics.Paths.Where(p => CheckEpType(p.PathType, EPType.Mark)).Any();

                cells[CellType.Deadline] =  statistics.Paths.Where(p => !String.IsNullOrEmpty(p.Deadline)).Any();
                cells[CellType.StartDate] =  statistics.Paths.Where(p => p.PathInfo != null && p.PathInfo.StartDate.HasValue).Any();
                cells[CellType.EndDate] =  statistics.Paths.Where(p => p.PathInfo != null && p.PathInfo.EndDate.HasValue).Any();
                cells[CellType.QuizCells] =  (qInfos != null && qInfos.Any());
                cells[CellType.UserTaxCode] =  settings.isRequiredField(ExportFieldType.TaxCodeInfo, exportData);
                cells[CellType.FirstActivityOn] = true;
                Boolean advancedInfo = settings.isRequiredField(ExportFieldType.QuestionnaireAdvancedInfo, exportData);
                Boolean certificationInfo = settings.isRequiredField(ExportFieldType.QuestionnaireCertificationInfo, exportData);


                if (cells[CellType.QuizCells])
                {
                    cells[CellType.QuizCompleted] =  qInfos.Where(q => q.Answers.Any() && q.Answers.Where(a => a.CompletedOn.HasValue).Any()).Any();
                    cells[CellType.SemiCorrectAnswers] =  (advancedInfo || certificationInfo) && qInfos.Where(q => q.QuestionnaireInfo.EvaluationActive && q.Answers.Any() && q.Answers.Where(a => a.SemiCorrectAnswers.HasValue).Any()).Any();
                    cells[CellType.UngradedAnswers] =  (advancedInfo || certificationInfo) && qInfos.Where(q => q.QuestionnaireInfo.EvaluationActive && q.Answers.Any() && q.Answers.Where(a => a.UngradedAnswers.HasValue).Any()).Any();
                    cells[CellType.QuestionsSkipped] =  (advancedInfo || certificationInfo) && qInfos.Where(q => q.QuestionnaireInfo.EvaluationActive && q.Answers.Any() && q.Answers.Where(a => a.QuestionsSkipped.HasValue).Any()).Any();
                    cells[CellType.QuizAttempts] =  (advancedInfo || certificationInfo) && qInfos.Where(q => q.QuestionnaireInfo.AllowMultipleAttempts).Any();
                    cells[CellType.NoEvaluations] =  qInfos.Where(q => !q.QuestionnaireInfo.EvaluationActive).Any();
                    cells[CellType.WithEvaluations] =  qInfos.Where(q => q.QuestionnaireInfo.EvaluationActive).Any();
                    cells[CellType.QuestionsCount] =  (advancedInfo || certificationInfo);
                    cells[CellType.WrongAnswers] =  (advancedInfo || certificationInfo);
                    cells[CellType.CorrectAnswers] =  (advancedInfo || certificationInfo);
                }


                Boolean loadAgencyInfo = settings.isRequiredField(ExportFieldType.AgencyInfo, exportData);
                cells[CellType.AgencyStart] =  loadAgencyInfo && statistics.Paths.Where(p => p.StStartDate.HasValue).Any() && Manager.UserHasAgencyAffiliations(idUser, statistics.Paths.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Paths.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Max());
                cells[CellType.AgencyEnd] =  loadAgencyInfo && statistics.Paths.Where(p => p.StEndDate.HasValue).Any() && Manager.UserHasAgencyAffiliations(idUser, statistics.Paths.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Paths.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Max());
                cells[CellType.AgencyCurrent] =  loadAgencyInfo && Manager.UserHasAgencyAffiliations(idUser, nDate);

                cells[CellType.IdUser] =  settings.isRequiredField(ExportFieldType.IdUser, exportData);
                cells[CellType.IdCommunity] =  settings.isRequiredField(ExportFieldType.IdCommunity, exportData);
                cells[CellType.IdRole] =  settings.isRequiredField(ExportFieldType.IdRole, exportData);
                cells[CellType.IdAgency] =  settings.isRequiredField(ExportFieldType.IdAgency, exportData);
                cells[CellType.IdPath] =  settings.isRequiredField(ExportFieldType.IdPath, exportData);
                cells[CellType.CommunityName] =  settings.isRequiredField(ExportFieldType.CommunityName, exportData);
                cells[CellType.IdOrganization] = settings.isRequiredField(ExportFieldType.IdOrganization, exportData);
                cells[CellType.OrganizationName] =  settings.isRequiredField(ExportFieldType.OrganizationName, exportData);
                return cells;
            }
        public Dictionary<CellType, Boolean> GetAvaliableCells(DateTime nDate, dtoPathUsers statistics, List<dtoBaseUserPathQuiz> qInfos, dtoExportConfigurationSetting settings, ExporPathData exportData)
        {
            Dictionary<CellType, Boolean> cells = GetAllCells();
            cells[CellType.Auto] = CheckEpType(statistics.PathType, EPType.Auto);
            cells[CellType.Time] = CheckEpType(statistics.PathType, EPType.Time);
            cells[CellType.Mark] = CheckEpType(statistics.PathType, EPType.Mark);

            cells[CellType.Deadline] = statistics.Users.Where(p => p.StDeadline.HasValue).Any();
            cells[CellType.StartDate] = statistics.Users.Where(u => u.StStartDate.HasValue).Any();
            cells[CellType.EndDate] =statistics.Users.Where(u => u.StEndDate.HasValue).Any();
            cells[CellType.QuizCells] =(qInfos != null && qInfos.Any());
            cells[CellType.UserTaxCode] = settings.isRequiredField(ExportFieldType.TaxCodeInfo, exportData);
            cells[CellType.PathName] = true;
            cells[CellType.FirstActivityOn] = true;
            
            Boolean advancedInfo = settings.isRequiredField(ExportFieldType.QuestionnaireAdvancedInfo, exportData);
            Boolean certificationInfo = settings.isRequiredField(ExportFieldType.QuestionnaireCertificationInfo, exportData);

            if (cells[CellType.QuizCells])
            {
                Boolean eActive = statistics.HasQuestionnaires(true);
                cells[CellType.QuizCompleted] = qInfos.Where(q => q.Answers.Any() && q.Answers.Where(a => a.CompletedOn.HasValue).Any()).Any();
                cells[CellType.SemiCorrectAnswers] = (advancedInfo || certificationInfo) && eActive && qInfos.Where(q => statistics.GetIdQuestionnaires(true).Contains(q.IdQuestionnaire) && q.Answers.Any() && q.Answers.Where(a => a.SemiCorrectAnswers.HasValue).Any()).Any();
                cells[CellType.UngradedAnswers] = (advancedInfo || certificationInfo) && eActive && qInfos.Where(q => statistics.GetIdQuestionnaires(true).Contains(q.IdQuestionnaire) && q.Answers.Any() && q.Answers.Where(a => a.UngradedAnswers.HasValue).Any()).Any();
                cells[CellType.QuestionsSkipped] = (advancedInfo || certificationInfo) && eActive && qInfos.Where(q => statistics.GetIdQuestionnaires(true).Contains(q.IdQuestionnaire) && q.Answers.Any() && q.Answers.Where(a => a.QuestionsSkipped.HasValue).Any()).Any();
                cells[CellType.QuizAttempts] = (advancedInfo || certificationInfo) && statistics.Questionnaires.Where(q => q.QuestionnaireInfo.AllowMultipleAttempts).Any();
                cells[CellType.NoEvaluations] = statistics.HasQuestionnaires(false);
                cells[CellType.WithEvaluations] = eActive;
                cells[CellType.QuestionsCount] = (advancedInfo || certificationInfo);
                cells[CellType.WrongAnswers] = (advancedInfo || certificationInfo);
                cells[CellType.CorrectAnswers] = (advancedInfo || certificationInfo);
            }
            if (statistics.Users.Any())
            {
                Boolean loadAgencyInfo = settings.isRequiredField(ExportFieldType.AgencyInfo, exportData);
                cells[CellType.AgencyStart] = loadAgencyInfo && statistics.Users.Where(p => p.StStartDate.HasValue).Any() && Manager.UsersHasAgencyAffiliations(statistics.Users.Select(u => u.IdPerson).ToList(), statistics.Users.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Users.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Max());
                cells[CellType.AgencyEnd] = loadAgencyInfo && statistics.Users.Where(p => p.StEndDate.HasValue).Any() && Manager.UsersHasAgencyAffiliations(statistics.Users.Select(u => u.IdPerson).ToList(), statistics.Users.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Users.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Max());
                cells[CellType.AgencyCurrent] = loadAgencyInfo && Manager.UsersHasAgencyAffiliations(statistics.Users.Select(u => u.IdPerson).ToList(), nDate);
            }
            cells[CellType.IdUser] = settings.isRequiredField(ExportFieldType.IdUser, exportData);
            cells[CellType.IdCommunity] = settings.isRequiredField(ExportFieldType.IdCommunity, exportData);
            cells[CellType.IdRole] = settings.isRequiredField(ExportFieldType.IdRole, exportData);
            cells[CellType.IdAgency] = settings.isRequiredField(ExportFieldType.IdAgency, exportData);
            cells[CellType.IdPath] = settings.isRequiredField(ExportFieldType.IdPath, exportData);
            cells[CellType.CommunityName] = settings.isRequiredField(ExportFieldType.CommunityName, exportData);
            cells[CellType.IdOrganization] = settings.isRequiredField(ExportFieldType.IdOrganization, exportData);
            cells[CellType.OrganizationName] = settings.isRequiredField(ExportFieldType.OrganizationName, exportData);
            return cells;
        }

        public Dictionary<CellType, Boolean> GetAvaliableCells(DateTime nDate, List<dtoUserStat> statistics, ItemType type, Boolean isAutoEp, Boolean isTimeBased, dtoExportConfigurationSetting settings, ExporPathData exportData)
        {
            Dictionary<CellType, Boolean> cells = GetAvaliableCells(nDate, type, isAutoEp, isTimeBased, settings, exportData);
            //cells[CellType.Deadline] = statistics.Users.Where(p => p.StDeadline.HasValue).Any();
            //cells[CellType.StartDate] = statistics.Users.Where(u => u.StStartDate.HasValue).Any();
            //cells[CellType.EndDate] =statistics.Users.Where(u => u.StEndDate.HasValue).Any();
    
            //cells[CellType.PathName] = true;
            //cells[CellType.FirstActivityOn] = true;
            
            //Boolean advancedInfo = settings.isRequiredField(ExportFieldType.QuestionnaireAdvancedInfo, exportData);
            //Boolean certificationInfo = settings.isRequiredField(ExportFieldType.QuestionnaireCertificationInfo, exportData);


            if (statistics.Any())
            {
                Boolean loadAgencyInfo = settings.isRequiredField(ExportFieldType.AgencyInfo, exportData);
               // cells[CellType.AgencyStart] = loadAgencyInfo && statistics.usersStat.Where(p => p.StStartDate.HasValue).Any() && Manager.UsersHasAgencyAffiliations(statistics.UserId.Select(u => u.UserId).ToList(), statistics.Users.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Users.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Max());
               // cells[CellType.AgencyEnd] = loadAgencyInfo && statistics.usersStat.Where(p => p.StEndDate.HasValue).Any() && Manager.UsersHasAgencyAffiliations(statistics.UserId.Select(u => u.UserId).ToList(), statistics.Users.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Users.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Max());
                cells[CellType.AgencyCurrent] = loadAgencyInfo && Manager.UsersHasAgencyAffiliations(statistics.Select(u => u.UserId).ToList(), nDate);
            }
            return cells;
        }
        public Dictionary<CellType, Boolean> GetAvaliableCells(DateTime nDate, List<dtoUserStatExtended> statistics, ItemType type, Boolean isAutoEp, Boolean isTimeBased, dtoExportConfigurationSetting settings, ExporPathData exportData)
        {
            Dictionary<CellType, Boolean> cells = GetAvaliableCells(nDate, type, isAutoEp, isTimeBased, settings, exportData);
            //cells[CellType.Deadline] = statistics.Users.Where(p => p.StDeadline.HasValue).Any();
            //cells[CellType.StartDate] = statistics.Users.Where(u => u.StStartDate.HasValue).Any();
            //cells[CellType.EndDate] =statistics.Users.Where(u => u.StEndDate.HasValue).Any();

            //cells[CellType.PathName] = true;
            //cells[CellType.FirstActivityOn] = true;

            //Boolean advancedInfo = settings.isRequiredField(ExportFieldType.QuestionnaireAdvancedInfo, exportData);
            //Boolean certificationInfo = settings.isRequiredField(ExportFieldType.QuestionnaireCertificationInfo, exportData);


            if (statistics.Any())
            {
                Boolean loadAgencyInfo = settings.isRequiredField(ExportFieldType.AgencyInfo, exportData);
                // cells[CellType.AgencyStart] = loadAgencyInfo && statistics.usersStat.Where(p => p.StStartDate.HasValue).Any() && Manager.UsersHasAgencyAffiliations(statistics.UserId.Select(u => u.UserId).ToList(), statistics.Users.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Users.Where(p => p.StStartDate.HasValue).Select(p => p.StStartDate.Value).Max());
                // cells[CellType.AgencyEnd] = loadAgencyInfo && statistics.usersStat.Where(p => p.StEndDate.HasValue).Any() && Manager.UsersHasAgencyAffiliations(statistics.UserId.Select(u => u.UserId).ToList(), statistics.Users.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Min(), statistics.Users.Where(p => p.StEndDate.HasValue).Select(p => p.StStartDate.Value).Max());
                cells[CellType.AgencyCurrent] = loadAgencyInfo && Manager.UsersHasAgencyAffiliations(statistics.Select(u => u.UserId).ToList(), nDate);
            }
            return cells;
        }
        private Dictionary<CellType, Boolean> GetAvaliableCells(DateTime nDate,  ItemType type, Boolean isAutoEp, Boolean isTimeBased, dtoExportConfigurationSetting settings, ExporPathData exportData)
        {
            Dictionary<CellType, Boolean> cells = GetAllCells();

            cells[CellType.Auto] = isAutoEp;
            cells[CellType.Mark] = !isAutoEp;

            cells[CellType.QuizCells] = false;
            cells[CellType.UserTaxCode] = settings.isRequiredField(ExportFieldType.TaxCodeInfo, exportData);
            cells[CellType.IdUser] = settings.isRequiredField(ExportFieldType.IdUser, exportData);
            cells[CellType.IdRole] = settings.isRequiredField(ExportFieldType.IdRole, exportData);
            cells[CellType.IdAgency] = settings.isRequiredField(ExportFieldType.IdAgency, exportData);
            cells[CellType.IdPath] = settings.isRequiredField(ExportFieldType.IdPath, exportData);
            switch (type)
            {
                case ItemType.SubActivity:
                    cells[CellType.Time] = false;
                    cells[CellType.IdCommunity] = false;
                    cells[CellType.CommunityName] = false;
                    cells[CellType.IdOrganization] = false;
                    cells[CellType.OrganizationName] = false;
                    break;
                default:
                    cells[CellType.Time] = isTimeBased;
                    cells[CellType.IdCommunity] = settings.isRequiredField(ExportFieldType.IdCommunity, exportData);
                    cells[CellType.CommunityName] = settings.isRequiredField(ExportFieldType.CommunityName, exportData);
                    cells[CellType.IdOrganization] = settings.isRequiredField(ExportFieldType.IdOrganization, exportData);
                    cells[CellType.OrganizationName] = settings.isRequiredField(ExportFieldType.OrganizationName, exportData);
                    break;
            }
          
            return cells;
        }

        public static Dictionary<CellType, Boolean> GetAvaliableCells(ExportConfigurationSetting settings, IList<dtoPathUsers> statistics)
        {
            Boolean hasFields = (settings != null && settings.Fields.Any());
            List<CellType> availableCells = GetDefaultCells(settings, statistics);
            Dictionary<CellType, Boolean> cells = new Dictionary<CellType, Boolean>();
            List<CellType> cTypes = Enum.GetValues(typeof(CellType)).Cast<CellType>().ToList();
            foreach(CellType t in cTypes){
                cells.Add(t, availableCells.Contains(t));
            }
            cells[CellType.PathName] = true;
            cells[CellType.Auto] = statistics.Where(s=> CheckEpType(s.PathType, EPType.Auto)).Any();
            cells[CellType.Time] = statistics.Where(s=> CheckEpType(s.PathType, EPType.Time)).Any();
            cells[CellType.Mark] = statistics.Where(s=> CheckEpType(s.PathType, EPType.Mark)).Any();
            cells[CellType.Deadline] = true;
            cells[CellType.EndDate] = true;
      
            return cells;
        }

        public static Dictionary<CellType, Boolean> GetAllCells()
        {
            Dictionary<CellType, Boolean> cells = new Dictionary<CellType, Boolean>();
            List<CellType> cTypes = Enum.GetValues(typeof(CellType)).Cast<CellType>().ToList();
            foreach (CellType t in cTypes)
            {
                cells.Add(t,false);
            }

            return cells;
        }

        private static List<CellType> GetDefaultCells(ExportConfigurationSetting settings, IList<dtoPathUsers> statistics)
        {
            Boolean eActive = (statistics!=null) && statistics.Where(s=>s.HasQuestionnaires(true)).Any();
            Boolean eNotActive = (statistics != null) &&  statistics.Where(s => s.HasQuestionnaires(false)).Any();
            List<CellType> cells = new List<CellType>();
            if (settings.Fields != null && settings.Fields.Where(f => f.Deleted == BaseStatusDeleted.None).Count() > 0)
            {
               cells.Add(CellType.PathName);
               foreach (ExportFieldType type in settings.Fields.Where(f => f.Deleted == BaseStatusDeleted.None).Select(f => f.Type).ToList())
               {
                    switch (type ){
                        case ExportFieldType.IdUser:
                            cells.Add(CellType.IdUser);
                            break; 
                        case ExportFieldType.IdCommunity:
                            cells.Add(CellType.IdCommunity);
                            break;
                        case ExportFieldType.IdRole:
                            cells.Add(CellType.IdRole);
                            break;
                        case ExportFieldType.IdAgency:
                            cells.Add(CellType.IdAgency);
                            break;
                        case ExportFieldType.IdPath:
                            cells.Add(CellType.IdPath);
                            break;
                        case ExportFieldType.IdUnit:
                            cells.Add(CellType.IdUnit);
                            break;
                        case ExportFieldType.IdActivity:
                            cells.Add(CellType.IdActivity);
                            break;
                        case ExportFieldType.IdSubActivity:
                            cells.Add(CellType.IdSubActivity);
                            break;
                        case ExportFieldType.IdQuestionnaire:
                            cells.Add(CellType.IdQuestionnaire);
                            break;
                        case ExportFieldType.TaxCodeInfo:
                            cells.Add(CellType.UserTaxCode);
                            break;
                        case ExportFieldType.AgencyInfo:
                            cells.Add(CellType.AgencyCurrent);
                            cells.Add(CellType.AgencyEnd);
                            cells.Add(CellType.AgencyStart);
                            break;
                        case ExportFieldType.QuestionnaireAdvancedInfo:
                            cells.Add(CellType.QuizCompleted);
                            cells.Add(CellType.SemiCorrectAnswers);
                            cells.Add(CellType.UngradedAnswers);
                            cells.Add(CellType.QuestionsSkipped);

                            if (eNotActive)
                                cells.Add(CellType.NoEvaluations);
                            if (eActive)
                                cells.Add(CellType.WithEvaluations);
                            cells.Add(CellType.QuestionsCount);
                            cells.Add(CellType.WrongAnswers);
                            cells.Add(CellType.CorrectAnswers);
                            cells.Add(CellType.QuizCells);
                            break;
                        case ExportFieldType.QuestionnaireCertificationInfo:
                            cells.Add(CellType.QuizCompleted);
                            if (eNotActive)
                                cells.Add(CellType.NoEvaluations);
                            if (eActive)
                                cells.Add(CellType.WithEvaluations);

                            //cells.Add(CellType.QuestionsCount);
                            //cells.Add(CellType.WrongAnswers);
                            //cells.Add(CellType.CorrectAnswers);
                              cells.Add(CellType.QuizCells);
                            break;
                        case ExportFieldType.RoleInfo:
                            cells.Add(CellType.RoleName);
                            break;
                        case ExportFieldType.QuestionnaireAttempts:
                            cells.Add(CellType.QuizAttempts);
                            break;
                        case ExportFieldType.IdOrganization:
                            cells.Add(CellType.IdOrganization);
                            break;
                        case ExportFieldType.OrganizationName:
                            cells.Add(CellType.OrganizationName);
                            break;
                        case ExportFieldType.CommunityName:
                            cells.Add(CellType.CommunityName);
                            break;
                        case ExportFieldType.FirstAccessOn:
                            cells.Add(CellType.ViewedOn);
                            break;
                        case ExportFieldType.FirstActionOn:
                            cells.Add(CellType.FirstActivityOn);
                            cells.Add(CellType.StartDate);
                            break;
	                }
                }
            }
            return cells.Distinct().ToList();
        }
        public static Boolean CheckStatusStatistic(StatusStatistic Actual, StatusStatistic Expected)
        {
            return (Actual & Expected) == Expected;
        }
        public static String GetMinTime(long totMin, long minCompletion)
        {
            return GetTime((long)(totMin * minCompletion / 100));

        }
        public static Boolean CheckEpType(EPType Actual, EPType Expected)
        {
            return (Actual & Expected) == Expected;
        }
        public static String GetSubActivityName(Dictionary<EduPathTranslations, String> translations, dtoSubActivity subActivity, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction)
        {
            String subActName = "";
            if (subActivity != null)
            {
                subActName = subActivity.Name;
                switch (subActivity.ContentType)
                {
                    case SubActivityType.Text:
                        if (String.IsNullOrEmpty(subActName))
                        {
                            subActName = (subActivity == null || String.IsNullOrEmpty(subActivity.Description)) ?
                                translations[EduPathTranslations.GenericTextAction] : ((subActivity.Description.Length > 60) ? subActivity.Description.Substring(0, 60) + "..." : subActivity.Description);
                        }
                        break;
                    case SubActivityType.Certificate:
                        subActName = GetContent(subActivity.ContentType, translations) + cAction.getDescriptionByActivity(subActivity);
                        break;
                    case SubActivityType.File:
                        subActName = GetContent(subActivity.ContentType, translations) + repAction.GetDescriptionByLink(subActivity.ModuleLink, true);
                        break;
                    case SubActivityType.Forum:
                    case SubActivityType.Quiz:
                    case SubActivityType.Wiki:
                        subActName = GetContent(subActivity.ContentType, translations) + quizAction.getDescriptionByLink(subActivity.ModuleLink,true );
                        break;
                }
            }
            return subActName;
        }
        public static String GetStatus(StatusStatistic status, Dictionary<EduPathTranslations, String> translations)
            {
                switch (status)
                {

                    case StatusStatistic.Started:
                        return translations[EduPathTranslations.Started];

                    case StatusStatistic.Completed:
                        return translations[EduPathTranslations.Completed];

                    case StatusStatistic.Passed:
                        return translations[EduPathTranslations.Passed];

                    case StatusStatistic.CompletedPassed:
                        return translations[EduPathTranslations.CompletedPassed];

                    default:
                        return translations[EduPathTranslations.NotStarted];
                }

            }

        public static String GetItemType(ItemType type, Dictionary<EduPathTranslations, String> translations)
        {
            switch (type)
            {

                case ItemType.Path:
                    return translations[EduPathTranslations.Path];

                case ItemType.Unit:
                    return translations[EduPathTranslations.Unit];

                case ItemType.Activity:
                    return translations[EduPathTranslations.Activity];

                case ItemType.SubActivity:
                    return translations[EduPathTranslations.SubActivity];
            }
            return "";
        }
        public static String GetContent(SubActivityType type, Dictionary<EduPathTranslations, String> translations)
        {
            switch (type)
            {

                case SubActivityType.Certificate:
                    return translations[EduPathTranslations.Certification] + ": ";

                case SubActivityType.File:
                    return translations[EduPathTranslations.File] + ": ";

                case SubActivityType.Forum:
                    return translations[EduPathTranslations.Forum] + ": ";

                case SubActivityType.Quiz:
                    return translations[EduPathTranslations.Quiz] + ": ";
                case SubActivityType.Wiki:
                    return translations[EduPathTranslations.Wiki] + ": ";
            }
            return "";
        }
        public static Boolean CheckStatus(Status Actual, Status Expected)
        {
            return (Actual & Expected) == Expected;
        }

        public static String GetTime(Int64 totMin)
        {
            Int64 h = totMin / 60;
            Int64 min = totMin % 60;
            return h + ":" + (min < 10 ? "0" : "") + min;
        }

        public static String GetType(ItemType type, Dictionary<EduPathTranslations, String> translations)
        {
            switch (type)
            {
                case ItemType.Path:
                    return translations[EduPathTranslations.Path];

                case ItemType.Unit:
                    return translations[EduPathTranslations.Unit];

                case ItemType.Activity:
                    return translations[EduPathTranslations.Activity];

                case ItemType.SubActivity:
                    return translations[EduPathTranslations.SubActivity];

                default:
                    return "";
            }

        }

        public static StatusStatistic GetStatusForUnit(ItemType type, dtoItemWithStatistic statistic, Boolean isTimeBased)
        {
            switch (type)
            {
                case ItemType.Unit:
                    if (isTimeBased)
                    {
                        if (statistic.Children == null || !statistic.Children.Any())
                            return StatusStatistic.None;
                        else
                        {
                            List<StatusStatistic> status = statistic.Children.Select(c => c.StatusStat).Distinct().ToList();
                            if (!status.Any())
                                return StatusStatistic.None;
                            else if (status.Count == 1)
                                return status[0];
                            else if (status.Contains(StatusStatistic.Started) || status.Where(s => HelperExportTo.CheckStatusStatistic(s, StatusStatistic.Browsed) && !HelperExportTo.CheckStatusStatistic(s, StatusStatistic.Started)).Any())
                                return StatusStatistic.Started;
                            else if (status.Any(s => HelperExportTo.CheckStatusStatistic(s, StatusStatistic.CompletedPassed)) && !status.Any(s => !HelperExportTo.CheckStatusStatistic(s, StatusStatistic.CompletedPassed)))
                                return StatusStatistic.CompletedPassed;
                            else
                                return StatusStatistic.Started;
                        }
                    }
                    else
                        return statistic.StatusStat;
                default:
                    return statistic.StatusStat;
            }
        }

    }
}