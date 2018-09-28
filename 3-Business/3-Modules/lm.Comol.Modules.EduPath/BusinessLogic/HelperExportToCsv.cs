using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public class HelperExportToCsv : lm.Comol.Core.DomainModel.Helpers.Export.ExportCsvBaseHelper
    {
        private HelperExportTo _Helper;
        private HelperExportTo Helper { get {
            if (_Helper == null)
                _Helper = new HelperExportTo(Manager);
            return _Helper;
        } }
        private lm.Comol.Core.Business.BaseModuleManager Manager { get; set; }
        private Dictionary<EduPathTranslations, String> CommonTranslations { get; set; }
        private Dictionary<Int32, String> RoleTranslations { get; set; }

        //public HelperExportToCsv() : base() {
        //    CommonTranslations = new Dictionary<EduPathTranslations, string>();
        //}
        //public HelperExportToCsv(lm.Comol.Core.Business.BaseModuleManager manager) : base() {
        //    Manager = manager;
        //    CommonTranslations = new Dictionary<EduPathTranslations, string>();
        //}
        //public HelperExportToCsv(Dictionary<EduPathTranslations, string> translations, Dictionary<Int32, String> roleTranslations)
        //    : base()
        //{
        //    CommonTranslations = translations;
        //    RoleTranslations = roleTranslations;
        //}
        public HelperExportToCsv(lm.Comol.Core.Business.BaseModuleManager manager, Dictionary<EduPathTranslations, string> translations,Dictionary<Int32, String> roleTranslations)
           // : this(translations, roleTranslations)
        {
            Manager = manager;
            CommonTranslations = translations;
            RoleTranslations = roleTranslations;
        }


        public HelperExportToCsv(String endRow, char delimiter, char endField) : base(endRow, delimiter, endField) { }

        #region "Global"
            public String PathStatistics(dtoEpGlobalStat eduPathStat, Boolean isAutoEp, Boolean isTimeBased, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction cTextAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction certAction)
            {
                String returnString = "";

                //set header
                returnString += AppendItem(CommonTranslations[EduPathTranslations.LevelType]) + AppendItem(CommonTranslations[EduPathTranslations.Type]) +
                                 AppendItem(CommonTranslations[EduPathTranslations.Name]);
                if (isTimeBased)
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.Time]);
                else
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.Weight]);

                returnString += AppendItem(CommonTranslations[EduPathTranslations.CompletedPassed]);

                if (!isAutoEp)
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.Completed]) + AppendItem(CommonTranslations[EduPathTranslations.Passed]);

                returnString += AppendItem(CommonTranslations[EduPathTranslations.Started]) + AppendItem(CommonTranslations[EduPathTranslations.NotStarted]) +
                     AppendItem(CommonTranslations[EduPathTranslations.ParticipantNumber] ) + EndRowItem;

                //setEPvalue
                returnString += AppendItem(((int)ItemType.Path).ToString()) + AppendItem(CommonTranslations[EduPathTranslations.Path]) + AppendItem(eduPathStat.itemName) + PathStatisticRowItem(eduPathStat, isAutoEp, isTimeBased, ItemType.Path) + EndRowItem;

                //set Units values
                foreach (dtoUnitGlobalStat unitStat in eduPathStat.childrenStat)
                {
                    if (!HelperExportTo.CheckStatus(unitStat.status, Status.Text))
                    {
                        returnString += AppendItem(((int)ItemType.Unit).ToString()) + AppendItem(CommonTranslations[EduPathTranslations.Unit]) + AppendItem(unitStat.itemName) + PathStatisticRowItem(unitStat, isAutoEp, isTimeBased, ItemType.Unit ) + EndRowItem;

                        //set Activities values
                        foreach (dtoActivityGlobalStat actStat in unitStat.childrenStat)
                        {
                            if (!HelperExportTo.CheckStatus(unitStat.status, Status.Text))
                            {
                                returnString += AppendItem(((int)ItemType.Activity).ToString()) + AppendItem(CommonTranslations[EduPathTranslations.Activity] ) + AppendItem(actStat.itemName) + PathStatisticRowItem(actStat, isAutoEp, isTimeBased, ItemType.Activity) + EndRowItem;

                                //set SubActivities
                                foreach (dtoSubActGlobalStat subActStat in actStat.childrenStat)
                                {
                                    String item = "";
                                    String subActName = subActStat.itemName;

                                    returnString += AppendItem((int)ItemType.SubActivity);
                                    returnString += AppendItem(CommonTranslations[EduPathTranslations.SubActivity]);

                                    switch (subActStat.ContentType)
                                    {
                                        case SubActivityType.Text:
                                            if (String.IsNullOrEmpty(subActName)) {
                                                item += AppendItem((subActStat.SubActivity == null || String.IsNullOrEmpty(subActStat.SubActivity.Description)) ?
                                                    CommonTranslations[EduPathTranslations.GenericTextAction] : (subActStat.SubActivity.Description.Length > 60) ? subActStat.SubActivity.Description.Substring(0, 60) + "..." : subActStat.SubActivity.Description);
                                            }
                                            break;
                                        case SubActivityType.Certificate:
                                            subActName = HelperExportTo.GetContent(subActStat.ContentType, CommonTranslations) + certAction.getDescriptionByActivity(subActStat.SubActivity);
                                            break;
                                        case SubActivityType.File:
                                            subActName = HelperExportTo.GetContent(subActStat.ContentType, CommonTranslations) + repAction.GetDescriptionByLink(subActStat.ModuleLink, true);
                                            break;
                                        case SubActivityType.Forum:
                                        case SubActivityType.Quiz:
                                        case SubActivityType.Wiki:
                                            subActName = HelperExportTo.GetContent(subActStat.ContentType, CommonTranslations) + quizAction.getDescriptionByLink(subActStat.ModuleLink, true);
                                            break;
                                    }
                                    long fatherWeight = -1;
                                    if (actStat.childrenStat.Count==1)
                                        fatherWeight = actStat.Weight;

                                    returnString += AppendItem(subActName) + PathStatisticRowItem(subActStat, isAutoEp, isTimeBased, ItemType.SubActivity, fatherWeight) + EndRowItem;
                                }
                            }
                        }
                    }
                }
                return returnString;
            }
            private String PathStatisticRowItem(dtoGenericGlobalStat dtoStat, Boolean isAutoEp, Boolean isTimeBased,ItemType type, long fatherWeight = -1)
            {
                string returnStr = "";
                switch (type)
                {
                    case ItemType.Unit:
                        returnStr += AppendItem("-");
                        returnStr += AppendItem("-");
                        if (!isAutoEp)
                        {
                            returnStr += AppendItem("-");
                            returnStr += AppendItem("-");
                        }
                        returnStr += AppendItem("-");
                        returnStr += AppendItem("-");
                        returnStr += AppendItem("-");
                        break;
                    default:
                        if (isTimeBased)
                        {
                            switch (type)
                            {
                                case ItemType.SubActivity:
                                    if (fatherWeight!=-1)
                                        returnStr += AppendItem(HelperExportTo.GetTime(fatherWeight));
                                    else
                                        returnStr += AppendItem("-");
                                    break;
                                default:
                                    returnStr += AppendItem(HelperExportTo.GetTime(dtoStat.Weight));
                                    break;
                            }
                        }
                        else
                            returnStr += AppendItem(dtoStat.Weight); // +EndFieldItem;

                        returnStr += dtoStat.compPassedCount.ToString() + EndFieldItem;
                        if (!isAutoEp)
                            returnStr += dtoStat.completedCount.ToString() + EndFieldItem + dtoStat.passedCount.ToString() + EndFieldItem;

                        returnStr += dtoStat.startedCount.ToString() + EndFieldItem + dtoStat.notStartedCount.ToString() + EndFieldItem + dtoStat.userCount.ToString() + EndFieldItem;
                        break;
                }

              
                return returnStr;
            }
        #endregion

        #region "Users"
            public String UsersStatistics(dtoListUserStat itemStat,DateTime onDate,  ItemType type, Boolean isAutoEp, Boolean isTimeBased, dtoExportConfigurationSetting settings, ExporPathData exportData)
            {
                String returnString = "";
                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells(onDate, (itemStat.usersStat == null) ? new List<dtoUserStatExtended>() : itemStat.usersStat.ToList(), type, isAutoEp, isTimeBased, settings, exportData);

                returnString += AppendItem(HelperExportTo.GetItemType(type,CommonTranslations) + ": " + itemStat.ItemName) + EndRowItem ;
                returnString += AppendItem(CommonTranslations[EduPathTranslations.Mandatory]) + AppendItem(HelperExportTo.CheckStatus(itemStat.Status, Status.Mandatory) ? CommonTranslations[EduPathTranslations.YesOption] : CommonTranslations[EduPathTranslations.NoOption]) + EndRowItem;
                returnString += UsersStatisticsTableHeader(isTimeBased, isAutoEp, type, CommonTranslations, cells);

                foreach (dtoUserStatExtended item in itemStat.usersStat)
                {
                    returnString += UserStatistics(item,onDate, type, isAutoEp, isTimeBased, CommonTranslations, cells);
                }

                return returnString;
            }
            private String UsersStatisticsTableHeader(Boolean isTimeBased, Boolean isAutoEp, ItemType type, Dictionary<EduPathTranslations, String> translations, Dictionary<CellType, Boolean> cells)
            {
                String returnString = "";

                if (cells[CellType.IdUser])
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdUser]);
                returnString += AppendItem(translations[EduPathTranslations.SurnameAndName] );

                if (cells[CellType.UserTaxCode])
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleTaxCode]);

                #region "Agency"
                if (cells[CellType.AgencyStart])
                {
                    if (cells[CellType.IdAgency])
                        returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdStartAgency]);
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleStartAgencyName]);
                }
                if (cells[CellType.AgencyEnd])
                {
                    if (cells[CellType.IdAgency])
                        returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdEndAgency]);
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleEndAgencyName]);
                }
                if (cells[CellType.AgencyCurrent])
                {
                    if (cells[CellType.IdAgency])
                        returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdCurrentAgency]);
                    returnString += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCurrentAgencyName]);
                }
                #endregion

                if (type != ItemType.SubActivity)
                {
                    if (isTimeBased)
                        returnString += AppendItem(translations[EduPathTranslations.Time])+ AppendItem(translations[EduPathTranslations.MinTime]);
                    else
                        returnString += AppendItem(translations[EduPathTranslations.Completion]) + AppendItem(translations[EduPathTranslations.MinCompletion]);
                    if (!isAutoEp)
                        returnString += AppendItem(translations[EduPathTranslations.Mark]) + AppendItem(translations[EduPathTranslations.MinMark]);
                }
                else
                {
                    if (!isTimeBased)
                        returnString += AppendItem(translations[EduPathTranslations.Completion]);

                    //if (isTimeBased)
                    //    returnString += AppendItem(translations[EduPathTranslations.Time] );
                    //else
                    //    returnString += AppendItem(translations[EduPathTranslations.Completion] );
                    if (!isAutoEp)
                        returnString += AppendItem(translations[EduPathTranslations.Mark]);
                }

                returnString += AppendItem(translations[EduPathTranslations.Status]);
                return AddRow(returnString);
            }
            public String UsersSubActivityStatistics(dtoSubActivity subActivity, DateTime onDate, dtoSubActListUserStat itemStat, Boolean isAutoEp, Boolean isTimeBased, dtoExportConfigurationSetting settings, ExporPathData exportData, Dictionary<EduPathTranslations, String> translations, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction)
            {
                String returnString = "";
                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells(onDate, (itemStat.usersStat == null) ? new List<dtoUserStat>() : itemStat.usersStat.ToList(), ItemType.SubActivity, isAutoEp, isTimeBased, settings, exportData);

                returnString += AppendItem(translations[EduPathTranslations.SubActivity] + ": " + HelperExportTo.GetSubActivityName(translations,subActivity, quizAction,repAction,tAction,cAction)) + EndRowItem;
                returnString += translations[EduPathTranslations.Mandatory] + EndFieldItem + (HelperExportTo.CheckStatus(itemStat.Status, Status.Mandatory) ? translations[EduPathTranslations.YesOption] : translations[EduPathTranslations.NoOption])+ EndFieldItem + EndRowItem;
                returnString += UsersStatisticsTableHeader(isTimeBased, isAutoEp, ItemType.SubActivity, translations,cells);

                foreach (dtoUserStat item in itemStat.usersStat)
                {
                    returnString += WriteUserStatistics(item, onDate,ItemType.SubActivity, isAutoEp, isTimeBased, translations, cells);
                }

                return returnString;
            }
            private String WriteUserStatistics(dtoUserStat item, DateTime nDate, ItemType type, Boolean isAutoEp, Boolean isTimeBased, Dictionary<EduPathTranslations, String> translations, Dictionary<CellType, Boolean> cells)
            {
                string returnString = "";

                if (cells[CellType.IdUser])
                    returnString += AppendItem(item.UserId);
                returnString += AppendItem(item.SurnameAndName);
                if (cells[CellType.UserTaxCode])
                    returnString += AppendItem((String.IsNullOrEmpty(item.TaxCode)) ? "" : item.TaxCode);

                #region "Agency"
                if (cells[CellType.AgencyStart])
                    returnString += AppendItem("");
                    //returnString += GetAgencyCells(item.UserId, (item.StFirstActivity.HasValue) ? item.StFirstActivity : item.StStartDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyEnd])
                    returnString += AppendItem("");
                    //returnString += GetAgencyCells(item.UserId, item.StEndDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyCurrent])
                    returnString += GetAgencyCells(item.UserId, nDate, cells[CellType.IdAgency]);
                #endregion

                if (isTimeBased)
                {
                    switch( type){
                        case ItemType.SubActivity:
                            break;
                        default:
                            returnString += AppendItem(HelperExportTo.GetTime(item.Completion));
                            break;
                    }
 
                }
                else
                    returnString += AppendItem(item.Completion);
                if (!isAutoEp)
                    returnString += AppendItem(item.Mark);

                returnString += HelperExportTo.GetStatus(item.StatusStat, translations) + EndFieldItem;
                returnString += EndRowItem;
                return returnString;
            }
        #endregion

        #region "Users"
            public String UserStatistics(dtoUserStatExtended item, DateTime nDate, ItemType type, Boolean isAutoEp, Boolean isTimeBased, Dictionary<EduPathTranslations, String> translations, Dictionary<CellType, Boolean> cells)
            {
                string returnString = "";
                if (cells[CellType.IdUser])
                    returnString += AppendItem(item.UserId);
                returnString += AppendItem(item.SurnameAndName);
                if (cells[CellType.UserTaxCode])
                    returnString += AppendItem((String.IsNullOrEmpty(item.TaxCode)) ? "" : item.TaxCode);

                #region "Agency"
                //if (cells[CellType.AgencyStart])
                //    returnString += GetAgencyCells(item.UserId, (item.StFirstActivity.HasValue) ? item.StFirstActivity : item.StStartDate, cells[CellType.IdAgency]);
                //if (cells[CellType.AgencyEnd])
                //    returnString += GetAgencyCells(item.UserId, item.StEndDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyCurrent])
                    returnString += GetAgencyCells(item.UserId, nDate, cells[CellType.IdAgency]);
                #endregion

                if (isTimeBased)
                {
                    Int32 minCompletion = (Int32)(item.MinCompletion*item.Weight /100);
                    returnString += AppendItem(HelperExportTo.GetTime(item.Completion)) + AppendItem(HelperExportTo.GetTime(minCompletion));
                    //returnString += AppendItem(HelperExportTo.GetTime(item.Completion)) + AppendItem(HelperExportTo.GetTime(item.MinCompletion));
                }    
                else
                    returnString += AppendItem(item.Completion) + AppendItem(item.MinCompletion);
                if (!isAutoEp)
                    returnString += AppendItem(item.Mark) + AppendItem(item.MinMark);

                returnString += HelperExportTo.GetStatus(item.StatusStat, translations) + EndFieldItem;
                returnString += EndRowItem;
                return returnString;
            }

        #endregion

        #region "User Paths Statistics"
            public String UserPathsStatistics(Int32 idUser, litePerson user, dtoUserPaths statistics, List<dtoUserPathQuiz> qInfos, dtoExportConfigurationSetting settings, ExporPathData exportData)
            {
                DateTime nDate = DateTime.Now;
                String export = "";
                String displayName = GetUserDisplayName(idUser, user, CommonTranslations);
                export += UserPathsStatisticsDisplayInfo(displayName, statistics);

                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells(idUser, user, nDate, statistics, qInfos, settings, exportData);

                export += UserPathsStatisticsTableHeader(statistics, cells);
                foreach (dtoUserPathInfo item in statistics.Paths.OrderBy(p => p.CommunityName).ThenBy(p => p.IdCommunity).ThenBy(p => p.PathName).ToList())
                {
                    export += UserPathsStatisticsRow(idUser, user, displayName, nDate, item, cells);
                }
                return export;
            }
            private String GetUserDisplayName(Int32 idUser, litePerson user, Dictionary<EduPathTranslations, String> translations)
            {
                String displayName = "";

                if (user == null)
                    displayName = String.Format(translations[EduPathTranslations.DeletedUser], idUser);
                else if (user.TypeID == (int)UserTypeStandard.Guest)
                    displayName = translations[EduPathTranslations.AnonymousUser];
                else
                    displayName = user.SurnameAndName;

                return displayName;
            }
            private String UserPathsStatisticsDisplayInfo(String displayName, dtoUserPaths statistics)
            {
                String tableData = "";
                String row = "";
                row += AppendItem(CommonTranslations[EduPathTranslations.DisplayUserInfo] + ": " + displayName) + EndRowItem;
                row += AppendItem(CommonTranslations[EduPathTranslations.DisplayPathsInfos]) + EndRowItem;
                row += AppendItem(String.Format(CommonTranslations[EduPathTranslations.DisplayNotStartedPathsInfos], statistics.NotStarted)) + EndRowItem;
                row += AppendItem(String.Format(CommonTranslations[EduPathTranslations.DisplayStartedPathsInfos], statistics.Started)) + EndRowItem;
                row += AppendItem(String.Format(CommonTranslations[EduPathTranslations.DisplayCompletedPathsInfos], statistics.Completed)) + EndRowItem;

                return tableData;
            }
            private String UserPathsStatisticsTableHeader(dtoUserPaths statistics, Dictionary<CellType, Boolean> cells)
            {
                String tableData = "";
                if (cells[CellType.IdUser])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdUser]);
                if (cells[CellType.UserTaxCode])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleTaxCode]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.SurnameAndName]);
               
                #region "Agency"
                if (cells[CellType.AgencyStart])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdStartAgency]);
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleStartAgencyName]);
                }
                if (cells[CellType.AgencyEnd])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdEndAgency]);
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleEndAgencyName]);
                }
                if (cells[CellType.AgencyCurrent])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdCurrentAgency]);
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCurrentAgencyName]);
                }
                #endregion
                if (cells[CellType.IdOrganization])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdOrganization]);
                if (cells[CellType.OrganizationName])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleOrganizationName]);
                if (cells[CellType.IdCommunity])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdCommunity]);
                if (cells[CellType.CommunityName])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCommunityName]);
                if (cells[CellType.IdPath])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdPath]);
                if (cells[CellType.PathName])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitlePathName]);

                if (cells[CellType.StartDate])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitlePathAvailableFrom]);
                if (cells[CellType.EndDate])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitlePathAvailableTo]);

                if (cells[CellType.Time])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.Time]);
                if (cells[CellType.Mark])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.Weight]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.MinCompletion]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.Completion]);

                tableData += AppendItem(CommonTranslations[EduPathTranslations.Status]);
                if (cells[CellType.ViewedOn])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleViewedOn]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleStartedOn]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCompletedOn]);
                if (cells[CellType.QuizCells])
                {
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizName]);
                    if (cells[CellType.WithEvaluations])
                    {
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleMinScore]);
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleMaxScore]);

                    }
                    if (cells[CellType.QuizAttempts])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptsNumber]);

                    if (cells[CellType.WithEvaluations])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleAttemptScore]);
                    if (cells[CellType.QuizAttempts])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptNumber]);
                    if (cells[CellType.QuizCompleted])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptDate]);

                    if (cells[CellType.QuestionsCount])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsCount]);
                    
                    if (cells[CellType.NoEvaluations])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAnswersCount]);
                    if (cells[CellType.WithEvaluations])
                    {
                        if (cells[CellType.CorrectAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizCorrectAnswers]);
                        if (cells[CellType.SemiCorrectAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizSemiCorrectAnswers]);
                        if (cells[CellType.UngradedAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizUngradedAnswers]);
                        if (cells[CellType.QuestionsSkipped])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsSkipped]);
                        if (cells[CellType.WrongAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizWrongAnswers]);

                    }
                }
                return tableData + EndRowItem;
            }
            private String UserPathsStatisticsRow(Int32 idUser, litePerson person, String displayName, DateTime nDate, dtoUserPathInfo item, Dictionary<CellType, Boolean> cells)
            {
                String row = "";
                if (cells[CellType.IdUser])
                    row += AppendItem(idUser);
                if (cells[CellType.UserTaxCode])
                    row += AppendItem((person == null) ? "" : person.TaxCode);
                row += AppendItem(displayName);
              
                #region "Agency"
                if (cells[CellType.AgencyStart])
                    row += GetAgencyCells(idUser, (item.Ps == null) ? null : item.Ps.StartDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyEnd])
                    row += GetAgencyCells(idUser, (item.Ps == null) ? null : item.Ps.EndDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyCurrent])
                    row += GetAgencyCells(idUser, nDate, cells[CellType.IdAgency]);
                #endregion
                if (cells[CellType.IdOrganization])
                    row += AppendItem(item.IdOrganization);
                if (cells[CellType.OrganizationName])
                    row += AppendItem((String.IsNullOrEmpty(item.OrganizationName) ? String.Format(CommonTranslations[EduPathTranslations.DeletedCommunity], item.IdOrganization) : item.OrganizationName));
                
                if (cells[CellType.IdCommunity])
                    row += AppendItem(item.IdCommunity);
                if (cells[CellType.CommunityName])
                    row += AppendItem((String.IsNullOrEmpty(item.CommunityName) ? String.Format(CommonTranslations[EduPathTranslations.DeletedCommunity], item.IdCommunity) : item.CommunityName));
                if (cells[CellType.IdPath])
                    row += AppendItem(item.IdPath);
                if (cells[CellType.PathName])
                    row += AppendItem(item.PathName);

                if (cells[CellType.StartDate])
                {
                    if (item.PathInfo == null || !item.PathInfo.StartDate.HasValue)
                        row += AppendItem("");
                    else
                        row += AppendItem(item.PathInfo.StartDate.Value);
                }
                if (cells[CellType.EndDate])
                {
                    if (item.EndDate == null || !item.PathInfo.EndDate.HasValue)
                        row += AppendItem("");
                    else
                        row += AppendItem(item.PathInfo.EndDate.Value);
                }

                if (HelperExportTo.CheckEpType(item.PathType, EPType.Time))
                    row += AppendItem(HelperExportTo.GetTime(item.PathInfo.Weight));
                else if (cells[CellType.Time])
                    row += AppendItem("");

                if (HelperExportTo.CheckEpType(item.PathType, EPType.Mark))
                    row += AppendItem(item.PathInfo.Weight);
                else if (cells[CellType.Mark])
                    row += AppendItem("");

                row += AppendItem(item.PathInfo.MinCompletion);
                if (item.Ps != null)
                    row += AppendItem(item.Ps.Completion);
                else
                    row += AppendItem("");

                switch (item.ItemStatus)
                {
                    case StatusStatistic.Completed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Completed]);
                        break;
                    case StatusStatistic.Passed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Passed]);
                        break;
                    case StatusStatistic.CompletedPassed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.CompletedPassed]);
                        break;
                    case StatusStatistic.Started:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Started]);
                        break;
                    default:
                        row += AppendItem(CommonTranslations[EduPathTranslations.NotStarted]);
                        break;
                }

                if (cells[CellType.ViewedOn])
                {
                    if (item.Ps != null && item.Ps.StartDate.HasValue)
                        row += AppendItem(item.Ps.StartDate.Value);
                    else
                        row += AppendItem("");
                }
                if (item.PsFirstActivity != null && item.PsFirstActivity.CreatedOn.HasValue)
                    row += AppendItem(item.PsFirstActivity.CreatedOn.Value);
                else
                    row += AppendItem("");
                if (item.Ps != null && item.Ps.EndDate.HasValue)
                    row += AppendItem(item.Ps.EndDate.Value);
                else
                    row += AppendItem("");
                String itemRow = row;

                if (cells[CellType.QuizCells])
                {
                    if (item.Questionnaires.Any())
                    {
                        String rows = "";
                        foreach (dtoUserPathQuiz questionnaire in item.Questionnaires)
                        {
                            rows += GetQuestionnaireRow(row, questionnaire, cells, CommonTranslations);
                        }
                        return rows;
                    }
                    else
                    {
                        row += GetNoQuestionnaireRow(cells);
                        return row + EndRowItem;
                    }
                }
                else
                    return row + EndRowItem;
            }
            private String GetQuestionnaireRow(String itemRow, dtoUserPathQuiz questionnaire, Dictionary<CellType, Boolean> cells, Dictionary<EduPathTranslations, String> translations)
            {
                String result = "";
                String questRow = itemRow;
                questRow += AppendItem(questionnaire.QuestionnaireInfo.Name);
                if (cells[CellType.WithEvaluations])
                {
                    if (questionnaire.QuestionnaireInfo.EvaluationActive)
                    {
                        questRow += AppendItem(questionnaire.QuestionnaireInfo.MinScore);
                        questRow += AppendItem(questionnaire.QuestionnaireInfo.EvaluationScale);
                    }
                    else
                    {
                        questRow += AppendItem("");
                        questRow += AppendItem("");
                    }
                }

                if (cells[CellType.QuizAttempts] && questionnaire.QuestionnaireInfo.AllowMultipleAttempts)
                {
                    if (questionnaire.QuestionnaireInfo.MaxAttemptsNumber == 0)
                        questRow += AppendItem(translations[EduPathTranslations.QuestionnaireAttemptsNoLimits]);
                    else
                        questRow += AppendItem(questionnaire.QuestionnaireInfo.MaxAttemptsNumber);
                }
                else if (cells[CellType.QuizAttempts])
                    questRow += AppendItem("");

                if (questionnaire.Answers.Any())
                {
                    foreach (dtoUserQuizAnswer answer in questionnaire.Answers)
                    {
                        result += GetAnswerRow(questRow, questionnaire.QuestionnaireInfo, answer, cells);
                    }
                }
                else
                {
                    questRow += GetNoAnswerRow(cells);
                    result = questRow + EndRowItem;
                }
                return result;
            }
            private String GetNoQuestionnaireRow(Dictionary<CellType, Boolean> cells)
            {
                String row = "";
                row += AppendItem("");
                if (cells[CellType.WithEvaluations])
                {
                    row += AppendItem("");
                    row += AppendItem("");
                    row += AppendItem("");
                }

                if (cells[CellType.QuizAttempts])
                {
                    row += AppendItem("");
                    row += AppendItem("");
                }
                if (cells[CellType.QuizCompleted])
                    row += AppendItem("");

                row += AppendItem("");

                if (cells[CellType.NoEvaluations])
                    row += AppendItem("");
                if (cells[CellType.WithEvaluations])
                {
                    if (cells[CellType.CorrectAnswers])
                        row += AppendItem("");
                    if (cells[CellType.SemiCorrectAnswers])
                        row += AppendItem("");
                    if (cells[CellType.UngradedAnswers])
                        row += AppendItem("");
                    if (cells[CellType.QuestionsSkipped])
                        row += AppendItem("");
                    if (cells[CellType.WrongAnswers])
                        row += AppendItem("");
                }
                return row;
            }
            private String GetAnswerRow(String questRow, dtoUserQuizDetails info, dtoUserQuizAnswer answer, Dictionary<CellType, Boolean> cells)
            {
                String result = "";
                String answerRow = questRow;
                if (cells[CellType.WithEvaluations] && info.EvaluationActive)
                    answerRow += AppendItem((answer.RelativeScore.HasValue) ? answer.RelativeScore.Value.ToString() : "");
                else if (cells[CellType.WithEvaluations])
                    answerRow += AppendItem("");
                if (cells[CellType.QuizAttempts] && info.AllowMultipleAttempts)
                    answerRow += AppendItem(answer.AttemptNumber);
                else if (cells[CellType.QuizAttempts])
                    answerRow += AppendItem("");
                if (cells[CellType.QuizCompleted] && answer.CompletedOn.HasValue)
                    answerRow += AppendItem(answer.CompletedOn.Value);
                else if (cells[CellType.QuizCompleted])
                    answerRow += AppendItem("");

                if (cells[CellType.QuestionsCount])
                    answerRow += AppendItem(answer.QuestionsCount);

                if (cells[CellType.NoEvaluations] && !info.EvaluationActive)
                    answerRow += AppendItem(answer.QuestionAnswers);
                else if (cells[CellType.NoEvaluations])
                    answerRow += AppendItem("");

                if (cells[CellType.WithEvaluations])
                {
                    if (info.EvaluationActive && cells[CellType.CorrectAnswers])
                        answerRow += AppendItem((answer.CorrectAnswers.HasValue) ? answer.CorrectAnswers.Value : 0);
                    else if (cells[CellType.CorrectAnswers])
                        answerRow += AppendItem("");

                    if (cells[CellType.SemiCorrectAnswers] && info.EvaluationActive)
                        answerRow += AppendItem((answer.SemiCorrectAnswers.HasValue) ? answer.SemiCorrectAnswers : 0);
                    else if (cells[CellType.SemiCorrectAnswers])
                        answerRow += AppendItem("");

                    if (cells[CellType.UngradedAnswers] && info.EvaluationActive)
                        answerRow += AppendItem((answer.UngradedAnswers.HasValue) ? answer.UngradedAnswers : 0);
                    else if (cells[CellType.UngradedAnswers])
                        answerRow += AppendItem("");
                    if (cells[CellType.QuestionsSkipped] && info.EvaluationActive)
                        answerRow += AppendItem((answer.QuestionsSkipped.HasValue) ? answer.QuestionsSkipped : 0);
                    else if (cells[CellType.QuestionsSkipped])
                        answerRow += AppendItem("");
                    if (info.EvaluationActive && cells[CellType.WrongAnswers])
                        answerRow += AppendItem((answer.WrongAnswers.HasValue) ? answer.WrongAnswers : 0);
                    else if (cells[CellType.WrongAnswers])
                        answerRow += AppendItem("");
                }
                result = answerRow + EndRowItem;
                return result;
            }
            private String GetNoAnswerRow(Dictionary<CellType, Boolean> cells)
            {
                String row = "";
                if (cells[CellType.WithEvaluations])
                    row += AppendItem("");
                if (cells[CellType.QuizAttempts])
                    row += AppendItem("");
                if (cells[CellType.QuizCompleted])
                    row += AppendItem("");
                if (cells[CellType.QuestionsCount])
                    row += AppendItem("");

                if (cells[CellType.NoEvaluations])
                    row += AppendItem("");
                if (cells[CellType.WithEvaluations])
                {
                    if (cells[CellType.CorrectAnswers])
                        row += AppendItem("");
                    if (cells[CellType.SemiCorrectAnswers])
                        row += AppendItem("");
                    if (cells[CellType.UngradedAnswers])
                        row += AppendItem("");
                    if (cells[CellType.QuestionsSkipped])
                        row += AppendItem("");
                    if (cells[CellType.WrongAnswers])
                        row += AppendItem("");
                }
                return row;
            }

            private String GetAgencyCells(Int32 idUser, DateTime? date, Boolean showIdentifiers)
            {
                String cells = "";
                if (date.HasValue)
                {
                    LazyAffiliation af = Manager.GetUserAffiliation(idUser, date.Value);

                    if (showIdentifiers && af != null && af.Agency != null)
                        cells += AppendItem(af.Agency.Id);
                    else if (showIdentifiers)
                        cells += AppendItem("");

                    if (af != null && af.Agency != null)
                        cells += AppendItem(af.Agency.Name);
                    else
                        cells += AppendItem("");
                }
                else {
                    if (showIdentifiers)
                        cells += AppendItem("");
                    cells += AppendItem("");
                }
                return cells;
            }
            //private String GetAgencyCells(Employee employee,DateTime? date, Boolean showIdentifiers, DefaultXmlStyleElement rowstyle)
            //{
            //    String cells = "";
            //    AgencyAffiliation af = (employee == null || !date.HasValue) ? null : employee.GetAffiliation(date.Value);

            //    if (showIdentifiers && af != null && af.Agency != null)
            //        cells += BuilderXmlDocument.AddData(af.Agency.Id, rowstyle.ToString());
            //    else if (showIdentifiers)
            //        cells += BuilderXmlDocument.AddData("", rowstyle.ToString());

            //    if (af != null && af.Agency != null)
            //        cells += BuilderXmlDocument.AddData(af.Agency.Name, rowstyle.ToString());
            //    else
            //        cells += BuilderXmlDocument.AddData("", rowstyle.ToString());
            //    return cells;
            //}

        #endregion

        #region "User Path Statistics"
            public String UserPathStatistics(ServiceStat service, long idPath, ItemType type, Int32 idUser, Int32 idCurrentUser, dtoItemWithStatistic statistics, DateTime onDate, dtoExportConfigurationSetting settings, ExporPathData exportData, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction)
            {
                String returnString = "";
                Boolean isTimeBased = false;
                Boolean isAutoEp = false;
                Path p = Manager.Get<Path>(idPath);
                if (p!=null){
                    isAutoEp =HelperExportTo.CheckEpType(p.EPType, EPType.Auto);
                    isTimeBased =HelperExportTo.CheckEpType(p.EPType, EPType.Time);
                }
                returnString += UserPathStatisticsDisplayInfo(p, type, idUser, onDate, settings, exportData, statistics);
                returnString += UserPathStatisticsTableHeader(isTimeBased, isAutoEp, type);
                returnString += WriteUserPathStatisticsRows(service, p, type, idUser, idCurrentUser,onDate, statistics, quizAction,repAction,tAction,cAction, isTimeBased, isAutoEp);
                return returnString;
            }
            private String UserPathStatisticsDisplayInfo(Path path, ItemType type, Int32 idUser, DateTime onDate, dtoExportConfigurationSetting settings, ExporPathData exportData, dtoItemWithStatistic statistics)
            {
                String result = "";

                result += AddRow(AppendItem(HelperExportTo.GetItemType(type, CommonTranslations)) + AppendItem(statistics.Name));
                if (type !=  ItemType.Path )
                    result += AddRow(AppendItem(CommonTranslations[EduPathTranslations.Mandatory]) + AppendItem((HelperExportTo.CheckStatus(statistics.Status, Status.Mandatory) ? CommonTranslations[EduPathTranslations.YesOption] : CommonTranslations[EduPathTranslations.NoOption])));

                Person user = Manager.GetPerson(idUser);
                if (user == null || (user!= null && user.TypeID == (int)UserTypeStandard.Guest))
                    result += AddRow(AppendItem(CommonTranslations[EduPathTranslations.TitleUserInfo]) + CommonTranslations[EduPathTranslations.AnonymousUser]);
                else
                    result += AddRow(AppendItem(CommonTranslations[EduPathTranslations.TitleUserInfo]) + AppendItem(user.SurnameAndName));
                if (settings.isRequiredField(ExportFieldType.TaxCodeInfo,exportData) && (user != null && user.TypeID != (int)UserTypeStandard.Guest))
                    result += AddRow(AppendItem(CommonTranslations[EduPathTranslations.TitleUserInfoTaxCode]) + AppendItem(user.TaxCode));

                result += AddRow("");
                result += AddRow(AppendItem(CommonTranslations[EduPathTranslations.TitleStatisticsDate]) + AppendItem(onDate.ToShortDateString() + " " + onDate.ToShortTimeString()));               
                result += AddRow("");
                return result;
            }
            private String UserPathStatisticsTableHeader(Boolean isTimeBased, Boolean isAutoEp, ItemType type)
            {
                String tHeader = "";

                tHeader += AppendItem(CommonTranslations[EduPathTranslations.LevelType]);
                tHeader += AppendItem(CommonTranslations[EduPathTranslations.Type]);
                tHeader += AppendItem(CommonTranslations[EduPathTranslations.Name]);

                if (type != ItemType.SubActivity)
                {
                    if (isTimeBased){
                        if (type == ItemType.Unit)
                            tHeader += AppendItem("") + AppendItem("");
                        else
                            tHeader += AppendItem(CommonTranslations[EduPathTranslations.Time]) + AppendItem(CommonTranslations[EduPathTranslations.MinTime]);
                        }
                    else
                        tHeader += AppendItem(CommonTranslations[EduPathTranslations.Completion]) + AppendItem(CommonTranslations[EduPathTranslations.MinCompletion]);
                    if (!isAutoEp)
                        tHeader += AppendItem(CommonTranslations[EduPathTranslations.Mark]) + AppendItem(CommonTranslations[EduPathTranslations.MinMark]);
                }
                else
                {
                    if (isTimeBased)
                        tHeader += AppendItem(CommonTranslations[EduPathTranslations.Time]);
                    else
                        tHeader += AppendItem(CommonTranslations[EduPathTranslations.Completion]);
                    if (!isAutoEp)
                        tHeader += AppendItem(CommonTranslations[EduPathTranslations.Mark]);
                }

                tHeader += AppendItem(CommonTranslations[EduPathTranslations.Status]);
                return AddRow(tHeader);
            }
            private String WriteUserPathStatisticsRows(ServiceStat service, Path path, ItemType type, Int32 idUser, Int32 idCurrentUser, DateTime onDate, dtoItemWithStatistic statistics, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction, Boolean isTimeBased, Boolean isAutoEp)
            {
                String result = "";
                String row = "";
                row += AppendItem((int)type);
                row += AppendItem(HelperExportTo.GetType(type, CommonTranslations));
                dtoActivityStatistic subStatistics = null;

                if (type == ItemType.Activity) {
                    Int32 idCommunity = (path != null && path.Community != null) ? path.Community.Id : 0;
                    Int32 idRole = (idCommunity == 0) ? 0 : Manager.GetSubscriptionIdRole(idCurrentUser, idCommunity);

                    subStatistics = service.GetDtoActivityStatistic_ByUser(statistics.Id, idCommunity, idUser, idCurrentUser, idRole, "", "", onDate);
                }
                if (subStatistics != null && subStatistics.SubActivities != null && subStatistics.SubActivities.Count == 1 && subStatistics.SubActivities[0].SubActivity !=null )
                    row += AppendItem(HelperExportTo.GetSubActivityName(CommonTranslations, subStatistics.SubActivities[0].SubActivity, quizAction,repAction, tAction, cAction));
                else
                    row += AppendItem(statistics.Name);

                switch (type)
                {
                    case ItemType.Unit:
                        if (isTimeBased)
                            row += AppendItem("-") + AppendItem("-");
                        else
                            row += AppendItem("-");
                        if (!isAutoEp)
                            row += AppendItem("-");
                        break;
                    case ItemType.SubActivity:
                        if (isTimeBased)
                            row += AppendItem("-");
                            //row += AppendItem(HelperExportTo.GetTime(statistics.Completion));
                        else
                            row += AppendItem(statistics.Completion);
                        if (!isAutoEp)
                            row += AppendItem(statistics.Mark);
                        break;
                    default:
                        if (isTimeBased)
                            row += AppendItem(HelperExportTo.GetTime(statistics.Completion)) + AppendItem(HelperExportTo.GetMinTime(statistics.Weight, statistics.MinCompletion));
                        else
                            row += AppendItem(statistics.Completion) + AppendItem(statistics.MinCompletion);
                        if (!isAutoEp)
                            row += AppendItem(statistics.Mark) + AppendItem(statistics.MinMark);
                        break;
                }


                row += AppendItem(HelperExportTo.GetStatus(HelperExportTo.GetStatusForUnit(type, statistics, isTimeBased), CommonTranslations));
                result += AddRow(row);

                ItemType childrenType = ItemType.None;
                switch (type)
                {
                    case ItemType.Path:
                        childrenType = ItemType.Unit;
                        break;
                    case ItemType.Unit:
                        childrenType = ItemType.Activity;
                        break;
                    case ItemType.Activity:
                        childrenType = ItemType.SubActivity;
                        break;
                }
                if (childrenType != ItemType.None && childrenType != ItemType.SubActivity)
                {
                    foreach (dtoItemWithStatistic childStatistics in statistics.Children)
                    {
                        result += WriteUserPathStatisticsRows(service, path, childrenType, idUser, idCurrentUser, onDate, childStatistics, quizAction,repAction, tAction, cAction, isTimeBased, isAutoEp);
                    }
                }
                else if (childrenType == ItemType.SubActivity && subStatistics.SubActivities != null && subStatistics.SubActivities.Count >1)
                    result += WriteUserPathSubActivityStatistics(service, path, subStatistics, idUser, idCurrentUser, onDate, quizAction,repAction, tAction, cAction, isTimeBased, isAutoEp);

                return result;
            }
            private String WriteUserPathSubActivityStatistics(ServiceStat service, Path path, dtoActivityStatistic statistics, Int32 idUser, Int32 idCurrentUser, DateTime onDate, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction, Boolean isTimeBased, Boolean isAutoEp)
            {
                String result = "";
                if (statistics != null && statistics.SubActivities.Any())
                {
                    foreach (var stat in statistics.SubActivities)
                    {
                        String row = "";
                        row += AppendItem((int)ItemType.SubActivity);
                        row += AppendItem(HelperExportTo.GetType(ItemType.SubActivity, CommonTranslations));

                        String name = stat.SubActivity.Name;
                        switch (stat.SubActivity.ContentType)
                        {
                            case SubActivityType.Text:
                                if (String.IsNullOrEmpty(name))
                                {
                                    name = (stat.SubActivity == null || String.IsNullOrEmpty(stat.SubActivity.Description)) ?
                                        CommonTranslations[EduPathTranslations.GenericTextAction] : (stat.SubActivity.Description.Length > 60) ? stat.SubActivity.Description.Substring(0, 60) + "..." : stat.SubActivity.Description;
                                }
                                break;
                            case SubActivityType.Certificate:
                                name = HelperExportTo.GetContent(stat.SubActivity.ContentType, CommonTranslations) + cAction.getDescriptionByActivity(stat.SubActivity);
                                break;
                            case SubActivityType.File:
                                name = HelperExportTo.GetContent(stat.SubActivity.ContentType, CommonTranslations) + repAction.GetDescriptionByLink(stat.SubActivity.ModuleLink, true);
                                break;
                            case SubActivityType.Forum:
                            case SubActivityType.Quiz:
                            case SubActivityType.Wiki:
                                name = HelperExportTo.GetContent(stat.SubActivity.ContentType, CommonTranslations) + quizAction.getDescriptionByLink(stat.SubActivity.ModuleLink, true);
                                break;
                        }
                        row += AppendItem(name);
                        if (isTimeBased)
                            row += AppendItem("") + AppendItem("");
                        else
                            row += AppendItem(stat.Completion);
                        if (!isAutoEp)
                            row += AppendItem(stat.Mark);

                        row += AppendItem(HelperExportTo.GetStatus(stat.StatusStat, CommonTranslations));
                        result += AddRow(row);
                    }
                }
                return result;
            }
            
            //private String WriteUserPathSubActivityStatistics(ServiceStat service, Path path, dtoActivityStatistic statistics, Int32 idUser, Int32 idCurrentUser, DateTime onDate, lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction gAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction, Boolean isTimeBased, Boolean isAutoEp)
            //{
            //    String result = "";
            //    if (statistics != null && statistics.SubActivities.Any())
            //    {
            //        foreach (var stat in statistics.SubActivities)
            //        {
            //            String row = "";
            //            row += AppendItem((int)ItemType.SubActivity);
            //            row += AppendItem(HelperExportTo.GetType(ItemType.SubActivity, CommonTranslations));

            //            String name = stat.SubActivity.Name;
            //            switch (stat.SubActivity.ContentType)
            //            {
            //                case SubActivityType.Text:
            //                    if (String.IsNullOrEmpty(name))
            //                    {
            //                        row += AppendItem((stat.SubActivity == null || String.IsNullOrEmpty(stat.SubActivity.Description)) ?
            //                            CommonTranslations[EduPathTranslations.GenericTextAction] : (stat.SubActivity.Description.Length > 60) ? stat.SubActivity.Description.Substring(0, 60) + "..." : stat.SubActivity.Description);
            //                    }
            //                    break;
            //                case SubActivityType.Certificate:
            //                    name = HelperExportTo.GetContent(stat.SubActivity.ContentType, CommonTranslations) + cAction.getDescriptionByActivity(stat.SubActivity);
            //                    break;
            //                case SubActivityType.File:
            //                case SubActivityType.Forum:
            //                case SubActivityType.Quiz:
            //                case SubActivityType.Wiki:
            //                    name = HelperExportTo.GetContent(stat.SubActivity.ContentType, CommonTranslations) + gAction.getDescriptionByLink(stat.SubActivity.ModuleLink);
            //                    break;
            //            }
            //            row += AppendItem(name);
            //            if (isTimeBased)
            //                row += AppendItem(HelperExportTo.GetTime(stat.Completion)) + AppendItem("");
            //            else
            //                row += AppendItem(stat.Completion);
            //            if (!isAutoEp)
            //                row += AppendItem(stat.Mark);

            //            row += AppendItem(HelperExportTo.GetStatus(stat.StatusStat, CommonTranslations));
            //            result += AddRow(row);
            //        }
            //    }
            //    return result;
            //}
    #endregion

        #region "Path Statistics"
            public String PathUsersStatistics(Int32 idUser, DateTime onDate, dtoPathUsers statistics, List<dtoBaseUserPathQuiz> qInfos, dtoExportConfigurationSetting settings, ExporPathData exportData)
            {
                DateTime nDate = DateTime.Now;
                String content = "";
                litePerson user = Manager.GetLitePerson(idUser);
                String displayName = GetUserDisplayName(idUser, user, CommonTranslations);
                String header = PathUsersStatisticsDisplayInfo(statistics.PathName, onDate, statistics);
                int rowNumber = 0;

                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells(nDate, statistics, qInfos, settings, exportData);

                content += PathUsersStatisticsTableHeader(statistics, cells);
                rowNumber++;
                foreach (dtoPathUserInfo item in statistics.Users)
                {
                    content += PathUsersStatisticsRow(nDate, statistics.PathType, statistics.Questionnaires, item, cells, rowNumber);
                    rowNumber++;
                }

                return content;
            }
            private String PathUsersStatisticsDisplayInfo(String pathName, DateTime onDate, dtoPathUsers statistics)
            {
                String tableData = "";
                String row = "";

                tableData += AddRow(String.Format(CommonTranslations[EduPathTranslations.StatisticsInfo], onDate.ToShortDateString() + " " + onDate.ToShortTimeString()));

                row = AppendItem(CommonTranslations[EduPathTranslations.DisplayPathNameInfo]);
                row += AppendItem(pathName);
                tableData = AddRow(row);

                row = AppendItem(CommonTranslations[EduPathTranslations.DisplayPathMinCompletionInfo]);
                row += AppendItem(statistics.PathInfo.MinCompletion + "%");
                tableData = AddRow(row);

                if (HelperExportTo.CheckEpType(statistics.PathType, EPType.Time))
                {
                    row = AppendItem(CommonTranslations[EduPathTranslations.DisplayPathTimeInfo]);
                    row += AppendItem(HelperExportTo.GetTime(statistics.PathInfo.Weight));
                    tableData = AddRow(row);
                }

                if (HelperExportTo.CheckEpType(statistics.PathType, EPType.Mark))
                {
                    row = AppendItem(CommonTranslations[EduPathTranslations.DisplayPathMinMarkInfo]);
                    row += AppendItem(statistics.PathInfo.Weight);
                    tableData = AddRow(row);
                }

                row = AppendItem(CommonTranslations[EduPathTranslations.DisplayPathStatisticInfos]);
                tableData = AddRow(row);

                row = AppendItem("");
                row += AppendItem(CommonTranslations[EduPathTranslations.DisplayPathUsersCompleted]);
                row += AppendItem(statistics.Completed);
                tableData = AddRow(row);

                row = AppendItem("");
                row += AppendItem(CommonTranslations[EduPathTranslations.DisplayPathUsersStarted]);
                row += AppendItem(statistics.Started);
                tableData = AddRow(row);

                row = AppendItem("");
                row += AppendItem(CommonTranslations[EduPathTranslations.DisplayPathUsersNotStarted]);
                row += AppendItem(statistics.NotStarted);
                tableData = AddRow(row);

                return tableData;
            }
            private String PathUsersStatisticsTableHeader(dtoPathUsers statistics, Dictionary<CellType, Boolean> cells)
            {
                String tableData = "";
                if (cells[CellType.IdUser])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdUser]);
                if (cells[CellType.UserTaxCode])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleTaxCode]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.SurnameAndName]);

                #region "Agency"
                if (cells[CellType.AgencyStart])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdStartAgency]);
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleStartAgencyName]);
                }
                if (cells[CellType.AgencyEnd])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdEndAgency]);
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleEndAgencyName]);
                }
                if (cells[CellType.AgencyCurrent])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdCurrentAgency]);
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCurrentAgencyName]);
                }
                #endregion

                //if (cells[CellType.IdOrganization])
                //    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdOrganization]);
                //if (cells[CellType.OrganizationName])
                //    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleOrganizationName]);
                //if (cells[CellType.IdCommunity])
                //    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdCommunity]);
                //if (cells[CellType.CommunityName])
                //    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCommunityName]);
                //if (cells[CellType.IdPath])
                //    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdPath]);
                //if (cells[CellType.PathName])
                //    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitlePathName]);


                if (cells[CellType.Time])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.Time]);
                if (cells[CellType.Mark])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.Weight]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.MinCompletion]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Completion]);

                tableData += AppendItem(CommonTranslations[EduPathTranslations.Status]);
                if (cells[CellType.ViewedOn])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleViewedOn]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleStartedOn]);
                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCompletedOn]);
                if (cells[CellType.QuizCells])
                {
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizName]);
                    if (cells[CellType.WithEvaluations])
                    {
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleMinScore]);
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleMaxScore]);

                    }
                    if (cells[CellType.QuizAttempts])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptsNumber]);

                    if (cells[CellType.WithEvaluations])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleAttemptScore]);
                    if (cells[CellType.QuizAttempts])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptNumber]);
                    if (cells[CellType.QuizCompleted])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptDate]);

                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsCount]);

                    if (cells[CellType.NoEvaluations])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAnswersCount]);
                    if (cells[CellType.WithEvaluations])
                    {
                        if (cells[CellType.CorrectAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizCorrectAnswers]);
                        if (cells[CellType.SemiCorrectAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizSemiCorrectAnswers]);
                        if (cells[CellType.UngradedAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizUngradedAnswers]);
                        if (cells[CellType.QuestionsSkipped])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsSkipped]);
                        if (cells[CellType.WrongAnswers])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizWrongAnswers]);

                    }
                }
                return AddRow(tableData);
            }
            private String PathUsersStatisticsRow(DateTime nDate, EPType pathType, IList<dtoSubActivityQuestionnaire> questionnaires, dtoPathUserInfo item, Dictionary<CellType, Boolean> cells, int rowNumber)
            {
                String row = "";
                if (cells[CellType.IdUser])
                    row += AppendItem(item.IdPerson);
                if (cells[CellType.UserTaxCode])
                    row += AppendItem((item.User == null) ? "" : item.User.TaxCode);
                row += AppendItem((item.User == null) ? String.Format(CommonTranslations[EduPathTranslations.DeletedUser], item.IdPerson) : item.User.SurnameAndName);

                #region "Agency"

                if (cells[CellType.AgencyStart])
                    row += GetAgencyCells(item.IdPerson, (item.StFirstActivity.HasValue) ? item.StFirstActivity : item.StStartDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyEnd])
                    row += GetAgencyCells(item.IdPerson, item.StEndDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyCurrent])
                    row += GetAgencyCells(item.IdPerson, nDate, cells[CellType.IdAgency]);
                #endregion

                if (item.Completion != null)
                {
                    if (HelperExportTo.CheckEpType(pathType, EPType.Time))
                        row += AppendItem(HelperExportTo.GetTime(item.Completion));
                    else if (cells[CellType.Time])
                        row += AppendItem("");
                    if (HelperExportTo.CheckEpType(pathType, EPType.Mark))
                        row += AppendItem(item.Completion);
                    else if (cells[CellType.Mark])
                        row += AppendItem("");
                }
                else
                    row += AppendItem("");

                switch (item.ItemStatus)
                {
                    case StatusStatistic.Completed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Completed]);
                        break;
                    case StatusStatistic.Passed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Passed]);
                        break;
                    case StatusStatistic.CompletedPassed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.CompletedPassed]);
                        break;
                    case StatusStatistic.Started:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Started]);
                        break;
                    default:
                        row += AppendItem(CommonTranslations[EduPathTranslations.NotStarted]);
                        break;
                }
                if (cells[CellType.ViewedOn])
                {
                    if (item.StStartDate.HasValue)
                        row += AppendItem(item.StStartDate.Value);
                    else
                        row += AppendItem("");
                }
                if (item.StFirstActivity.HasValue)
                    row += AppendItem(item.StFirstActivity.Value);
                else
                    row += AppendItem("");
                if (item.StEndDate.HasValue)
                    row += AppendItem(item.StEndDate.Value);
                else
                    row += AppendItem("");
                String itemRow = row;

                if (cells[CellType.QuizCells])
                {
                    if (item.Questionnaires.Any())
                    {
                        String rows = "";
                        foreach (dtoBaseUserPathQuiz questionnaire in item.Questionnaires)
                        {
                            rows += GetQuestionnaireRow(row, questionnaires.Where(q => q.IdQuestionnaire == questionnaire.IdQuestionnaire).Select(q => q.QuestionnaireInfo).FirstOrDefault(), questionnaire, cells);
                        }
                        return rows;
                    }
                    else
                    {
                        row += GetNoQuestionnaireRow(cells);
                        return AddRow(row);
                    }
                }
                else
                    return AddRow(row);
            }
            private String GetQuestionnaireRow(String itemRow, dtoUserQuizDetails qInfo, dtoBaseUserPathQuiz questionnaire, Dictionary<CellType, Boolean> cells)
            {
                String result = "";
                String questRow = itemRow;
                questRow += AppendItem(qInfo.Name);
                if (cells[CellType.WithEvaluations])
                {
                    if (qInfo.EvaluationActive)
                    {
                        questRow += AppendItem(qInfo.MinScore);
                        questRow += AppendItem(qInfo.EvaluationScale);
                    }
                    else
                    {
                        questRow += AppendItem("");
                        questRow += AppendItem("");
                    }
                }

                if (cells[CellType.QuizAttempts] && qInfo.AllowMultipleAttempts)
                {
                    if (qInfo.MaxAttemptsNumber == 0)
                        questRow += AppendItem(CommonTranslations[EduPathTranslations.QuestionnaireAttemptsNoLimits]);
                    else
                        questRow += AppendItem(qInfo.MaxAttemptsNumber);
                }
                else if (cells[CellType.QuizAttempts])
                    questRow += AppendItem("");

                if (questionnaire.Answers.Any())
                {
                    foreach (dtoUserQuizAnswer answer in questionnaire.Answers)
                    {
                        result += GetAnswerRow(questRow, qInfo, answer, cells);
                    }
                }
                else
                {
                    questRow += GetNoAnswerRow(cells);
                    result = AddRow(questRow);
                }
                return result;
            }
        #endregion

        #region "Reports"
            public Dictionary<CellType, Boolean> GetReportCells(ExportConfigurationSetting settings, IList<dtoPathUsers> statistics)
            {
                return HelperExportTo.GetAvaliableCells(settings, statistics);
            }
            public String GetReportHeader(ExportConfigurationSetting settings, IList<dtoPathUsers> statistics)
            {
                return GetReportHeader(GetReportCells(settings,statistics), settings.StatisticType);
            }
            public String GetReportHeader(Dictionary<CellType, Boolean> cells, StatisticType sType)
            {
                String tableData = "";
                if (sType == StatisticType.UsersStatistics ) { 
                    if (cells[CellType.IdUser])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdUser]);
                    if (cells[CellType.UserTaxCode])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleTaxCode]);
                
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.SurnameAndName]);

                #region "Agency"
                    if (cells[CellType.AgencyStart])
                    {
                        if (cells[CellType.IdAgency])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdStartAgency]);
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleStartAgencyName]);
                    }
                    if (cells[CellType.AgencyEnd])
                    {
                        if (cells[CellType.IdAgency])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdEndAgency]);
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleEndAgencyName]);
                    }
                    if (cells[CellType.AgencyCurrent])
                    {
                        if (cells[CellType.IdAgency])
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdCurrentAgency]);
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCurrentAgencyName]);
                    }
                #endregion
                }
                if (cells[CellType.IdOrganization])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdOrganization]);
                if (cells[CellType.OrganizationName])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleOrganizationName]);
                if (cells[CellType.IdCommunity])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdCommunity]);
                if (cells[CellType.CommunityName])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCommunityName]);
                if (cells[CellType.IdPath])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleIdPath]);
                if (cells[CellType.PathName])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitlePathName]);


                if (cells[CellType.Time])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.Time]);
                if (cells[CellType.Mark])
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.Weight]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.MinCompletion]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Completion]);

                tableData += AppendItem(CommonTranslations[EduPathTranslations.Status]);

                if (sType == StatisticType.UsersStatistics )
                {
                    if (cells[CellType.ViewedOn])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleViewedOn]);
                    if (cells[CellType.FirstActivityOn])
                        tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleStartedOn]);
                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleCompletedOn]);
                }

                switch (sType) { 
                    case StatisticType.UiDefined:
                    case StatisticType.UsersStatistics:
                        if (cells[CellType.QuizCells])
                        {
                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizName]);
                            if (cells[CellType.WithEvaluations])
                            {
                                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleMinScore]);
                                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleMaxScore]);

                            }
                            if (cells[CellType.QuizAttempts])
                                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptsNumber]);

                            if (cells[CellType.WithEvaluations])
                                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleAttemptScore]);
                            if (cells[CellType.QuizAttempts])
                                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptNumber]);
                            if (cells[CellType.QuizCompleted])
                                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptDate]);

                            tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsCount]);

                            if (cells[CellType.NoEvaluations])
                                tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizAnswersCount]);
                            if (cells[CellType.WithEvaluations])
                            {
                                if (cells[CellType.CorrectAnswers])
                                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizCorrectAnswers]);
                                if (cells[CellType.SemiCorrectAnswers])
                                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizSemiCorrectAnswers]);
                                if (cells[CellType.UngradedAnswers])
                                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizUngradedAnswers]);
                                if (cells[CellType.QuestionsSkipped])
                                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsSkipped]);
                                if (cells[CellType.WrongAnswers])
                                    tableData += AppendItem(CommonTranslations[EduPathTranslations.CellTitleQuizWrongAnswers]);

                            }
                        }
                        break;
                }
                
                return AddRow(tableData);
            }
            public String GetReportUsersStatistics(dtoPathUsers statistics, List<dtoBaseUserPathQuiz> qInfos, Dictionary<CellType, Boolean> cells, Boolean writeHeader)
            {
                DateTime nDate = DateTime.Now;
                String content = "";
                int rowNumber = 0;

                if (writeHeader)
                    content += GetReportHeader(cells, StatisticType.UsersStatistics);
                rowNumber++;
                foreach (dtoPathUserInfo item in statistics.UsersFiltered)
                {
                    content += ReportUsersStatisticsRow(nDate,statistics, statistics.PathType, statistics.Questionnaires, item, cells, rowNumber);
                    rowNumber++;
                }

                return content;
            }
            private String ReportUsersStatisticsRow(DateTime nDate, dtoPathUsers statistics,EPType pathType, IList<dtoSubActivityQuestionnaire> questionnaires, dtoPathUserInfo item, Dictionary<CellType, Boolean> cells, int rowNumber)
            {
                String row = "";
                if (cells[CellType.IdUser])
                    row += AppendItem(item.IdPerson);
                if (cells[CellType.UserTaxCode])
                    row += AppendItem((item.User == null) ? "" : item.User.TaxCode);
                row += AppendItem((item.User == null) ? String.Format(CommonTranslations[EduPathTranslations.DeletedUser], item.IdPerson) : item.User.SurnameAndName);

                #region "Agency"

                if (cells[CellType.AgencyStart])
                    row += GetAgencyCells(item.IdPerson, (item.StFirstActivity.HasValue) ? item.StFirstActivity : item.StStartDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyEnd])
                    row += GetAgencyCells(item.IdPerson, item.StEndDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyCurrent])
                    row += GetAgencyCells(item.IdPerson, nDate, cells[CellType.IdAgency]);
                #endregion


                if (cells[CellType.IdOrganization])
                    row += AppendItem(statistics.IdOrganization);
                if (cells[CellType.OrganizationName])
                    row += AppendItem(statistics.OrganizationName);
                if (cells[CellType.IdCommunity])
                    row += AppendItem(statistics.IdCommunity);
                if (cells[CellType.CommunityName])
                    row += AppendItem((String.IsNullOrEmpty(statistics.CommunityName) ? String.Format(CommonTranslations[EduPathTranslations.DeletedCommunity], statistics.IdCommunity) : statistics.CommunityName));
                if (cells[CellType.IdPath])
                    row += AppendItem(statistics.IdPath);
                if (cells[CellType.PathName])
                    row += AppendItem(statistics.PathName);

                if (item.Completion != null)
                {
                    if (HelperExportTo.CheckEpType(pathType, EPType.Time))
                        row += AppendItem(HelperExportTo.GetTime(item.Completion));
                    else if (cells[CellType.Time])
                        row += AppendItem("");
                    if (HelperExportTo.CheckEpType(pathType, EPType.Mark))
                        row += AppendItem(item.Completion);
                    else if (cells[CellType.Mark])
                        row += AppendItem("");
                }
                else
                    row += AppendItem("");

                switch (item.ItemStatus)
                {
                    case StatusStatistic.Completed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Completed]);
                        break;
                    case StatusStatistic.Passed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Passed]);
                        break;
                    case StatusStatistic.CompletedPassed:
                        row += AppendItem(CommonTranslations[EduPathTranslations.CompletedPassed]);
                        break;
                    case StatusStatistic.Started:
                        row += AppendItem(CommonTranslations[EduPathTranslations.Started]);
                        break;
                    default:
                        row += AppendItem(CommonTranslations[EduPathTranslations.NotStarted]);
                        break;
                }
                if (cells[CellType.ViewedOn])
                {
                    if (item.StStartDate.HasValue)
                        row += AppendItem(item.StStartDate.Value);
                    else
                        row += AppendItem("");
                }
                if (cells[CellType.FirstActivityOn]) {
                    if (item.StFirstActivity.HasValue)
                        row += AppendItem(item.StFirstActivity.Value);
                    else
                        row += AppendItem("");
                }
               
                if (item.StEndDate.HasValue)
                    row += AppendItem(item.StEndDate.Value);
                else
                    row += AppendItem("");
                String itemRow = row;

                if (cells[CellType.QuizCells])
                {
                    if (item.Questionnaires.Any())
                    {
                        String rows = "";
                        foreach (dtoBaseUserPathQuiz questionnaire in item.Questionnaires)
                        {
                            rows += GetQuestionnaireRow(row, questionnaires.Where(q => q.IdQuestionnaire == questionnaire.IdQuestionnaire).Select(q => q.QuestionnaireInfo).FirstOrDefault(), questionnaire, cells);
                        }
                        return rows;
                    }
                    else
                    {
                        row += GetNoQuestionnaireRow(cells);
                        return AddRow(row);
                    }
                }
                else
                    return AddRow(row);
            }
        #endregion

            #region Common
            //public static Boolean HelperExportTo.CheckStatusStatistic(StatusStatistic Actual, StatusStatistic Expected)
            //{
            //    return (Actual & Expected) == Expected;
            //}
            //private static String HelperExportTo.GetMinTime(long totMin, long minCompletion)
            //{
            //    return HelperExportTo.GetTime((long)(totMin * minCompletion / 100));

            //}
            public static String GetErrorDocument(String translation)
            {
                return translation;
            }
            public static String GetErrorDocument(String translation, String statInfoTranslation, DateTime onDate)
            {
                String export = "";
                if (!String.IsNullOrEmpty(statInfoTranslation))
                    export += String.Format(statInfoTranslation, onDate.ToShortDateString() + " " + onDate.ToShortTimeString()) + dEndRowChars;
                export += dEndRowChars + dEndRowChars + dEndRowChars;
                export += translation;
                return export;
            }
            //public static Boolean HelperExportTo.CheckEpType(EPType Actual, EPType Expected)
            //{
            //    return (Actual & Expected) == Expected;
            //}
            //private static String GetSubActivityName(Dictionary<EduPathTranslations, String> translations, dtoSubActivity subActivity, lm.Comol.Core.ModuleLinks.IExternalModuleDisplayAction gAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction)
            //{
            //    String subActName = "";
            //    if (subActivity != null)
            //    {
            //        subActName = subActivity.Name;
            //        switch (subActivity.ContentType)
            //        {
            //            case SubActivityType.Text:
            //                if (String.IsNullOrEmpty(subActName))
            //                {
            //                    subActName = (subActivity == null || String.IsNullOrEmpty(subActivity.Description)) ?
            //                        translations[EduPathTranslations.GenericTextAction] : ((subActivity.Description.Length > 60) ? subActivity.Description.Substring(0, 60) + "..." : subActivity.Description);
            //                }
            //                break;
            //            case SubActivityType.Certificate:
            //                subActName = HelperExportTo.GetContent(subActivity.ContentType, translations) + cAction.getDescriptionByActivity(subActivity);
            //                break;
            //            case SubActivityType.File:
            //            case SubActivityType.Forum:
            //            case SubActivityType.Quiz:
            //            case SubActivityType.Wiki:
            //                subActName = HelperExportTo.GetContent(subActivity.ContentType, translations) + gAction.getDescriptionByLink(subActivity.ModuleLink);
            //                break;
            //        }
            //    }
            //    return subActName;
            //}
            //private static String HelperExportTo.GetStatus(StatusStatistic status, Dictionary<EduPathTranslations, String> translations)
            //    {
            //        switch (status)
            //        {

            //            case StatusStatistic.Started:
            //                return translations[EduPathTranslations.Started];

            //            case StatusStatistic.Completed:
            //                return translations[EduPathTranslations.Completed];

            //            case StatusStatistic.Passed:
            //                return translations[EduPathTranslations.Passed];

            //            case StatusStatistic.CompletedPassed:
            //                return translations[EduPathTranslations.CompletedPassed];

            //            default:
            //                return translations[EduPathTranslations.NotStarted];
            //        }

            //    }

            //private static String HelperExportTo.GetItemType(ItemType type, Dictionary<EduPathTranslations, String> translations)
            //{
            //    switch (type)
            //    {

            //        case ItemType.Path:
            //            return translations[EduPathTranslations.Path];

            //        case ItemType.Unit:
            //            return translations[EduPathTranslations.Unit];

            //        case ItemType.Activity:
            //            return translations[EduPathTranslations.Activity];

            //        case ItemType.SubActivity:
            //            return translations[EduPathTranslations.SubActivity];
            //    }
            //    return "";
            //}
            //private static String HelperExportTo.GetContent(SubActivityType type, Dictionary<EduPathTranslations, String> translations)
            //{
            //    switch (type)
            //    {

            //        case SubActivityType.Certificate:
            //            return translations[EduPathTranslations.Certification] + ": ";

            //        case SubActivityType.File:
            //            return translations[EduPathTranslations.File] + ": ";

            //        case SubActivityType.Forum:
            //            return translations[EduPathTranslations.Forum] + ": ";

            //        case SubActivityType.Quiz:
            //            return translations[EduPathTranslations.Quiz] + ": ";
            //        case SubActivityType.Wiki:
            //            return translations[EduPathTranslations.Wiki] + ": ";
            //    }
            //    return "";
            //}
            //public static Boolean HelperExportTo.CheckStatus(Status Actual, Status Expected)
            //{
            //    return (Actual & Expected) == Expected;
            //}

            //public static String HelperExportTo.GetTime(Int64 totMin)
            //{
            //    Int64 h = totMin / 60;
            //    Int64 min = totMin % 60;
            //    return h + ":" + (min < 10 ? "0" : "") + min;
            //}

            //private static String HelperExportTo.GetType(ItemType type, Dictionary<EduPathTranslations, String> translations)
            //{
            //    switch (type)
            //    {
            //        case ItemType.Path:
            //            return translations[EduPathTranslations.Path];

            //        case ItemType.Unit:
            //            return translations[EduPathTranslations.Unit];

            //        case ItemType.Activity:
            //            return translations[EduPathTranslations.Activity];

            //        case ItemType.SubActivity:
            //            return translations[EduPathTranslations.SubActivity];

            //        default:
            //            return "";
            //    }

            //}
        #endregion
    }
}