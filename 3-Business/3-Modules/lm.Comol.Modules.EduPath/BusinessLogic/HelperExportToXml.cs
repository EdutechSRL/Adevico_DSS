using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public class HelperExportToXml //: lm.Comol.Core.DomainModel.Helpers.Export.ExportXmlBaseHelper 
    {
        private HelperExportTo _Helper;
        private HelperExportTo Helper
        {
            get
            {
                if (_Helper == null)
                    _Helper = new HelperExportTo(Manager);
                return _Helper;
            }
        }
        private lm.Comol.Core.Business.BaseModuleManager Manager { get; set; }
        private Dictionary<EduPathTranslations, String> CommonTranslations { get; set; }
        private Dictionary<Int32, String> RoleTranslations { get; set; }

        //public HelperExportToXml()
        //{
        //    CommonTranslations = new Dictionary<EduPathTranslations, string>();
        //    RoleTranslations = new Dictionary<Int32, String>();
        //}
        //public HelperExportToXml(Dictionary<EduPathTranslations, string> translations, Dictionary<Int32, String> roleTranslations)
        //    : this()
        //{
        //    CommonTranslations = translations;
        //    RoleTranslations = roleTranslations;
        //}
        public HelperExportToXml(lm.Comol.Core.Business.BaseModuleManager manager, Dictionary<EduPathTranslations, string> translations, Dictionary<Int32, String> roleTranslations)
            //: this(translations, roleTranslations)
        {
            Manager = manager;
            CommonTranslations = translations;
            RoleTranslations = roleTranslations;
        }
        #region "Path Statistics"
        public String PathStatistics(dtoEpGlobalStat statistics, DateTime? onDate, Boolean isAutoEp, Boolean isTimeBased, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction cTextAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction certAction)
            {
                String export =" ";
                String content = "";
                //content += WriteStatisticsHeader(CommonTranslations[EduPathTranslations.StatisticsInfo], onDate);
                String header = PathStatisticsHeader(isAutoEp,isTimeBased);
                int rowNumber = 0;

                content += PathStatisticsRow(statistics, ItemType.Path, isAutoEp,isTimeBased,rowNumber, ItemType.Path);
                rowNumber++;
                //set Units values
                foreach (dtoUnitGlobalStat dtoUnit in statistics.childrenStat)
                {
                    if (!HelperExportTo.CheckStatus(dtoUnit.status, Status.Text))
                    {
                        content += PathStatisticsRow(dtoUnit,ItemType.Unit, isAutoEp, isTimeBased, rowNumber, ItemType.Unit );
                        rowNumber++;

                        foreach (dtoActivityGlobalStat dtoAct in dtoUnit.childrenStat)
                        {
                            if (!HelperExportTo.CheckStatus(dtoAct.status, Status.Text))
                            {
                                content += PathStatisticsRow(dtoAct, ItemType.Activity, isAutoEp, isTimeBased, rowNumber, ItemType.Activity);
                                rowNumber++;

                                foreach (dtoSubActGlobalStat dtoSubAct in dtoAct.childrenStat)
                                {
                                    long fatherWeight = -1;
                                    if (dtoAct.childrenStat.Count == 1)
                                        fatherWeight = dtoAct.Weight;

                                    content += PathItemStatisticRow(dtoSubAct, ItemType.SubActivity, isAutoEp, isTimeBased, rowNumber, quizAction,repAction, cTextAction, certAction, fatherWeight);
                                    rowNumber++;
                                }
                            }
                        }
                    }
                }

                export += BuilderXmlDocument.AddWorkSheet("--", header + content);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }

            private String PathStatisticsHeader(Boolean isAutoEp, Boolean isTimeBased)
            {
                String tableData = "";
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.LevelType]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Type]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Name]);
                if (isTimeBased)
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Time]);
                else
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Weight]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CompletedPassed]);
                if (!isAutoEp)
                {
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Completed]);
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Passed]);
                }
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Started]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.NotStarted]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.ParticipantNumber]);
                return BuilderXmlDocument.AddRow(tableData);
            }
            private String PathStatisticsRow(dtoGenericGlobalStat genericStat, ItemType itemtype, Boolean isAutoEp, Boolean isTimeBased, int rowNumber, ItemType type, long fatherWeight = -1)
            {
                DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem);

                String row = "";
                row += BuilderXmlDocument.AddData((int)itemtype , rowstyle.ToString());
                row += BuilderXmlDocument.AddData(HelperExportTo.GetType(itemtype,CommonTranslations), rowstyle.ToString());
                row += BuilderXmlDocument.AddData(genericStat.itemName, rowstyle.ToString());
                switch (type)
                {
                    case ItemType.Unit:
                        if (isTimeBased)
                            row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                        else
                            row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
//                            row += BuilderXmlDocument.AddData(genericStat.Weight, rowstyle.ToString());

                        row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                        if (!isAutoEp)
                        {
                            row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                            row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                        }
                        row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                        row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                        row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                        break;
                    default:
                        if (isTimeBased)
                            row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(genericStat.Weight), rowstyle.ToString());
                        else
                            row += BuilderXmlDocument.AddData(genericStat.Weight, rowstyle.ToString());

                        row += BuilderXmlDocument.AddData(genericStat.compPassedCount, rowstyle.ToString());
                        if (!isAutoEp)
                        {
                            row += BuilderXmlDocument.AddData(genericStat.completedCount, rowstyle.ToString());
                            row += BuilderXmlDocument.AddData(genericStat.passedCount, rowstyle.ToString());
                        }
                        row += BuilderXmlDocument.AddData(genericStat.startedCount, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(genericStat.notStartedCount, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(genericStat.userCount, rowstyle.ToString());
                        break;
                }
               

                return BuilderXmlDocument.AddRow(row);
            }
            private String PathItemStatisticRow(dtoSubActGlobalStat statistic, ItemType itemtype, Boolean isAutoEp, Boolean isTimeBased, int rowNumber, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction cTextAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction certAction, long fatherWeight = -1)
            {
                DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem);

                String row = "";
                row += BuilderXmlDocument.AddData((int)itemtype, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(HelperExportTo.GetType(itemtype, CommonTranslations), rowstyle.ToString());

                String actionName = statistic.itemName;

                switch (statistic.ContentType)
                {
                    case SubActivityType.Certificate:
                        actionName = HelperExportTo.GetContent(statistic.ContentType, CommonTranslations) + certAction.getDescriptionByActivity(statistic.SubActivity);
                        break;
                    case SubActivityType.File:
                        actionName = HelperExportTo.GetContent(statistic.ContentType, CommonTranslations) + repAction.GetDescriptionByLink(statistic.ModuleLink, true);
                        break;
                    case SubActivityType.Forum:
                    case SubActivityType.Quiz:
                    case SubActivityType.Wiki:
                        actionName = HelperExportTo.GetContent(statistic.ContentType, CommonTranslations) + quizAction.getDescriptionByLink(statistic.ModuleLink, true);
                        break;
                }
                row += BuilderXmlDocument.AddData(actionName, rowstyle.ToString());
                if (fatherWeight == -1)
                {
                    if (isTimeBased)
                        row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
                    else
                        row += BuilderXmlDocument.AddData(statistic.Weight, rowstyle.ToString());
                }
                else
                {
                    if (isTimeBased)
                        row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(fatherWeight), rowstyle.ToString());
                    else
                        row += BuilderXmlDocument.AddData(fatherWeight, rowstyle.ToString());
                }

                row += BuilderXmlDocument.AddData(statistic.compPassedCount, rowstyle.ToString());
                if (!isAutoEp)
                {
                    row += BuilderXmlDocument.AddData(statistic.completedCount, rowstyle.ToString());
                    row += BuilderXmlDocument.AddData(statistic.passedCount, rowstyle.ToString());
                }
                row += BuilderXmlDocument.AddData(statistic.startedCount, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(statistic.notStartedCount, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(statistic.userCount, rowstyle.ToString());

                return BuilderXmlDocument.AddRow(row);
            }

        #endregion
        #region "Users Statistics"
            public String UsersStatistics(dtoListUserStat itemStat, DateTime? onDate, ItemType itemType, Boolean isAutoEp, Boolean isTimeBased, dtoExportConfigurationSetting settings, ExporPathData exportData)
            {
                String export = "";
                String content = "";
                String row = "";
                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells((onDate.HasValue) ? onDate.Value : DateTime.Now, (itemStat.usersStat == null) ? new List<dtoUserStatExtended>() : itemStat.usersStat.ToList(), itemType, isAutoEp, isTimeBased, settings, exportData);

                row = BuilderXmlDocument.AddData(HelperExportTo.GetItemType(itemType, CommonTranslations) + ": " + itemStat.ItemName);
                content += BuilderXmlDocument.AddRow(row);
                content += BuilderXmlDocument.AddEmptyRow();

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Mandatory]);
                row += BuilderXmlDocument.AddData((HelperExportTo.CheckStatus(itemStat.Status, Status.Mandatory) ? CommonTranslations[EduPathTranslations.YesOption] : CommonTranslations[EduPathTranslations.NoOption]));

                content += BuilderXmlDocument.AddRow(row);
                content += WriteStatisticsHeader(CommonTranslations[EduPathTranslations.StatisticsInfo], onDate);
                content += BuilderXmlDocument.AddEmptyRows(1);
                content += UsersStatisticsTableHeader(isAutoEp, isTimeBased, itemType, cells);

                int rowNumber = 0;
                foreach (dtoUserStatExtended item in itemStat.usersStat)
                {
                    content += WriteUserStatistics(item,onDate, itemType, isAutoEp, isTimeBased, rowNumber, cells);
                    rowNumber++;
                }

                export += BuilderXmlDocument.AddWorkSheet("--", content);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());

            }

            private string UsersStatisticsTableHeader(Boolean isAutoEp, Boolean isTimeBased, ItemType type, Dictionary<CellType, Boolean> cells)
            {
                String row = "";
                
                if (cells[CellType.IdUser])
                    row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleIdUser], DefaultXmlStyleElement.HeaderTable.ToString());
                row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.SurnameAndName], DefaultXmlStyleElement.HeaderTable.ToString());

                if (cells[CellType.UserTaxCode])
                    row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleTaxCode], DefaultXmlStyleElement.HeaderTable.ToString());

                #region "Agency"
                if (cells[CellType.AgencyStart])
                {
                    if (cells[CellType.IdAgency])
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleIdStartAgency], DefaultXmlStyleElement.HeaderTable.ToString());
                    row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleStartAgencyName], DefaultXmlStyleElement.HeaderTable.ToString());
                }
                if (cells[CellType.AgencyEnd])
                {
                    if (cells[CellType.IdAgency])
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleIdEndAgency], DefaultXmlStyleElement.HeaderTable.ToString());
                    row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleEndAgencyName], DefaultXmlStyleElement.HeaderTable.ToString());
                }
                if (cells[CellType.AgencyCurrent])
                {
                    if (cells[CellType.IdAgency])
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleIdCurrentAgency], DefaultXmlStyleElement.HeaderTable.ToString());
                    row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CellTitleCurrentAgencyName], DefaultXmlStyleElement.HeaderTable.ToString());
                }
                #endregion

                if (type != ItemType.SubActivity)
                {
                    if (isTimeBased)
                    {
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Time], DefaultXmlStyleElement.HeaderTable.ToString());
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.MinTime], DefaultXmlStyleElement.HeaderTable.ToString());
                    }
                    else
                    {
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Completion], DefaultXmlStyleElement.HeaderTable.ToString());
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.MinCompletion], DefaultXmlStyleElement.HeaderTable.ToString());
                    }
                    if (!isAutoEp)
                    {
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Mark], DefaultXmlStyleElement.HeaderTable.ToString());
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.MinMark], DefaultXmlStyleElement.HeaderTable.ToString());
                    }
                }
                else
                {
                    if (!isTimeBased)
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Completion], DefaultXmlStyleElement.HeaderTable.ToString());
                    //if (isTimeBased)
                    //    row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Time], DefaultXmlStyleElement.HeaderTable.ToString());
                    //else
                    //    row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Completion], DefaultXmlStyleElement.HeaderTable.ToString());
                    if (!isAutoEp)
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Mark], DefaultXmlStyleElement.HeaderTable.ToString());
                }

                row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Status], DefaultXmlStyleElement.HeaderTable.ToString());
                return BuilderXmlDocument.AddRow(row);
            }
            private string WriteUserStatistics(dtoUserStatExtended item, DateTime? onDate, ItemType type, Boolean isAutoEp, Boolean isTimeBased, int rowNumber, Dictionary<CellType, Boolean> cells)
            {
                String row = "";
                String rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem.ToString() : DefaultXmlStyleElement.RowItem.ToString());

                if (cells[CellType.IdUser])
                    row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.UserId), rowstyle);
                row += BuilderXmlDocument.AddData(item.SurnameAndName, rowstyle);
                if (cells[CellType.UserTaxCode])
                    row += BuilderXmlDocument.AddData((String.IsNullOrEmpty(item.TaxCode)) ? "" : item.TaxCode, rowstyle);

                #region "Agency"
                //if (cells[CellType.AgencyStart])
                //    returnString += GetAgencyCells(item.UserId, (item.StFirstActivity.HasValue) ? item.StFirstActivity : item.StStartDate, cells[CellType.IdAgency]);
                //if (cells[CellType.AgencyEnd])
                //    returnString += GetAgencyCells(item.UserId, item.StEndDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyCurrent])
                    row += GetAgencyCells(item.UserId, onDate, cells[CellType.IdAgency], (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem));
                #endregion

               

                if (isTimeBased)
                {
                    switch (type)
                    {
                        case ItemType.SubActivity:
                            break;
                        default:
                              Int32 minCompletion = (Int32)(item.MinCompletion*item.Weight /100);
                    //returnString += AppendItem(HelperExportTo.GetTime(item.Completion)) + AppendItem(HelperExportTo.GetTime(minCompletion));
                            row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.Completion), rowstyle);
                            row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(minCompletion), rowstyle);
                            //row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.Completion), rowstyle);
                            //row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.MinCompletion), rowstyle);
                            break;
                    }

                }
                else
                {
                    row += BuilderXmlDocument.AddData(item.Completion, rowstyle);
                    row += BuilderXmlDocument.AddData(item.MinCompletion, rowstyle);
                }
                if (!isAutoEp)
                {
                    row += BuilderXmlDocument.AddData(item.Mark, rowstyle);
                    row += BuilderXmlDocument.AddData(item.MinMark, rowstyle);
                }

                row += BuilderXmlDocument.AddData(HelperExportTo.GetStatus(item.StatusStat, CommonTranslations), rowstyle);
                return BuilderXmlDocument.AddRow(row);
            }
        #endregion
        #region "SubActivity Statistics"
            public string UsersSubActivityStatistics(dtoSubActivity subActivity, DateTime? onDate, dtoSubActListUserStat itemStat, Boolean isAutoEp, Boolean isTimeBased, dtoExportConfigurationSetting settings, ExporPathData exportData, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction)
            {
                String export = "";
                String content = "";
                String row = "";
                String name = HelperExportTo.GetSubActivityName(CommonTranslations, subActivity, quizAction, repAction, tAction, cAction);
                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells((onDate.HasValue) ? onDate.Value : DateTime.Now, (itemStat.usersStat == null) ? new List<dtoUserStat>() : itemStat.usersStat.ToList(), ItemType.SubActivity, isAutoEp,isTimeBased , settings, exportData);

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.SubActivity] + ": " + name);
                content += BuilderXmlDocument.AddRow(row);
                content += BuilderXmlDocument.AddEmptyRow();

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Mandatory]);
                row += BuilderXmlDocument.AddData((HelperExportTo.CheckStatus(itemStat.Status, Status.Mandatory) ? CommonTranslations[EduPathTranslations.YesOption] : CommonTranslations[ EduPathTranslations.NoOption]));

                content += BuilderXmlDocument.AddRow(row);
                content += WriteStatisticsHeader(CommonTranslations[EduPathTranslations.StatisticsInfo], onDate);
                content += BuilderXmlDocument.AddEmptyRows(1);
                content += UsersStatisticsTableHeader(isAutoEp, isTimeBased, ItemType.SubActivity, cells);

                int rowNumber = 0;

                foreach (dtoUserStat item in itemStat.usersStat)
                {
                    content += WriteSubActivityStatistics(item,onDate, ItemType.SubActivity, isAutoEp, isTimeBased, rowNumber, cells);
                    rowNumber++;
                }

                export += BuilderXmlDocument.AddWorkSheet("--", content);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private string WriteSubActivityStatistics(dtoUserStat item, DateTime? onDate, ItemType type, Boolean isAutoEp, Boolean isTimeBased, int rowNumber, Dictionary<CellType, Boolean> cells)
            {
                String row = "";
                String rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem.ToString() : DefaultXmlStyleElement.RowItem.ToString());


                if (cells[CellType.IdUser])
                    row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.UserId), rowstyle);
                row += BuilderXmlDocument.AddData(item.SurnameAndName, rowstyle);
                if (cells[CellType.UserTaxCode])
                    row += BuilderXmlDocument.AddData((String.IsNullOrEmpty(item.TaxCode)) ? "" : item.TaxCode, rowstyle);

                #region "Agency"
                //if (cells[CellType.AgencyStart])
                //    returnString += GetAgencyCells(item.UserId, (item.StFirstActivity.HasValue) ? item.StFirstActivity : item.StStartDate, cells[CellType.IdAgency]);
                //if (cells[CellType.AgencyEnd])
                //    returnString += GetAgencyCells(item.UserId, item.StEndDate, cells[CellType.IdAgency]);
                if (cells[CellType.AgencyCurrent])
                    row += GetAgencyCells(item.UserId, onDate, cells[CellType.IdAgency], (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem));
                #endregion


                if (!isTimeBased)
                    row += BuilderXmlDocument.AddData(item.Completion, rowstyle);
                //if (isTimeBased)
                //row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.Completion), rowstyle);
                //else
                //    row += BuilderXmlDocument.AddData(item.Completion, rowstyle);
                if (!isAutoEp)
                    row += BuilderXmlDocument.AddData(item.Mark, rowstyle);

                row += BuilderXmlDocument.AddData(HelperExportTo.GetStatus(item.StatusStat, CommonTranslations), rowstyle);
                return BuilderXmlDocument.AddRow(row);
            }
        #endregion

        #region "User Paths Statistics"
            public String UserPathsStatistics(Int32 idUser, litePerson user, dtoUserPaths statistics, List<dtoUserPathQuiz> qInfos, dtoExportConfigurationSetting settings, ExporPathData exportData)
            {
                DateTime nDate = DateTime.Now;
                String export = " ";
                String content = "";
                String displayName = GetUserDisplayName(idUser, user);
                String header = UserPathsStatisticsDisplayInfo(displayName, statistics);
                int rowNumber = 0;


                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells(idUser, user, nDate, statistics, qInfos, settings, exportData);

                content += UserPathsStatisticsTableHeader(statistics, cells);
                rowNumber++;
                foreach (dtoUserPathInfo item in statistics.Paths.OrderBy(p=>p.CommunityName).ThenBy(p=>p.IdCommunity).ThenBy(p=> p.PathName).ToList())
                {
                    content += UserPathsStatisticsRow(idUser, user,displayName,nDate, item, cells, rowNumber);
                    rowNumber++;
                }

                export += BuilderXmlDocument.AddWorkSheet("--", header + content);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private String GetUserDisplayName(Int32 idUser, litePerson user)
            {
                String displayName = "";

                if (user == null)
                    displayName = String.Format(CommonTranslations[EduPathTranslations.DeletedUser], idUser);
                else if (user.TypeID == (int)UserTypeStandard.Guest)
                    displayName = CommonTranslations[EduPathTranslations.AnonymousUser];
                else
                    displayName = user.SurnameAndName;

                return displayName;
            }
            private String UserPathsStatisticsDisplayInfo(String displayName, dtoUserPaths statistics)
            {
                String tableData = "";
                String row = "";
                row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayUserInfo], DefaultXmlStyleElement.Title.ToString());
                row += BuilderXmlDocument.AddData(displayName, DefaultXmlStyleElement.Title.ToString());
                tableData = BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathStatisticInfos], DefaultXmlStyleElement.Title.ToString());
                tableData += BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayCompletedPathsInfos], DefaultXmlStyleElement.Title.ToString());
                row += BuilderXmlDocument.AddData(statistics.Completed, DefaultXmlStyleElement.Title.ToString());
                tableData += BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayStartedPathsInfos], DefaultXmlStyleElement.Title.ToString());
                row += BuilderXmlDocument.AddData(statistics.Started, DefaultXmlStyleElement.Title.ToString());
                tableData += BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayNotStartedPathsInfos], DefaultXmlStyleElement.Title.ToString());
                row += BuilderXmlDocument.AddData(statistics.NotStarted, DefaultXmlStyleElement.Title.ToString());
                tableData += BuilderXmlDocument.AddRow(row);

                return tableData;
            }
            private String UserPathsStatisticsTableHeader(dtoUserPaths statistics, Dictionary<CellType, Boolean> cells)
            {
                String tableData = "";
                if (cells[CellType.IdCommunity])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdUser]);
                if (cells[CellType.UserTaxCode])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleTaxCode]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.SurnameAndName]);
                
                #region "Agency"
                if (cells[CellType.AgencyStart]){
                    if (cells[CellType.IdAgency])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdStartAgency]);
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleStartAgencyName]);
                }
                if (cells[CellType.AgencyEnd])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdEndAgency]);
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleEndAgencyName]);
                }
                if (cells[CellType.AgencyCurrent] )
                {
                    if (cells[CellType.IdAgency])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdCurrentAgency]);
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleCurrentAgencyName]);
                }
                #endregion

                if (cells[CellType.IdOrganization])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdOrganization]);
                if (cells[CellType.OrganizationName])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleOrganizationName]);
                if (cells[CellType.IdCommunity])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdCommunity]);
                if (cells[CellType.CommunityName])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleCommunityName]);
                if (cells[CellType.IdPath])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdPath]);
                if (cells[CellType.PathName])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitlePathName]);

              
                if (cells[CellType.StartDate])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitlePathAvailableFrom]);
                if (cells[CellType.EndDate])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitlePathAvailableTo]);

                if (cells[CellType.Time])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Time]);
                if (cells[CellType.Mark])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Weight]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.MinCompletion]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Completion]);

                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Status]);
                if (cells[CellType.ViewedOn])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleViewedOn]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleStartedOn]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleCompletedOn]);
                if (cells[CellType.QuizCells])
                {
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizName]);
                    if (cells[CellType.WithEvaluations])
                    {
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleMinScore]);
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleMaxScore]);

                    }
                    if (cells[CellType.QuizAttempts])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptsNumber]);

                    if (cells[CellType.WithEvaluations])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleAttemptScore]);
                    if (cells[CellType.QuizAttempts])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptNumber]);
                    if (cells[CellType.QuizCompleted])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptDate]);

                    if (cells[CellType.QuestionsCount])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsCount]);

                    if (cells[CellType.NoEvaluations])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAnswersCount]);
                    if (cells[CellType.WithEvaluations])
                    {
                        if (cells[CellType.CorrectAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizCorrectAnswers]);
                        if (cells[CellType.SemiCorrectAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizSemiCorrectAnswers]);
                        if (cells[CellType.UngradedAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizUngradedAnswers]);
                        if (cells[CellType.QuestionsSkipped])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsSkipped]);
                        if (cells[CellType.WrongAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizWrongAnswers]);

                    }
                }
                return BuilderXmlDocument.AddRow(tableData);
            }
            private String UserPathsStatisticsRow(Int32 idUser, litePerson person, String displayName, DateTime nDate, dtoUserPathInfo item, Dictionary<CellType, Boolean> cells,int rowNumber)
            {
                DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem);

                String row = "";
                if (cells[CellType.IdUser])
                    row += BuilderXmlDocument.AddData(idUser, rowstyle.ToString());
                if (cells[CellType.UserTaxCode])
                    row += BuilderXmlDocument.AddData((person == null) ? "" : person.TaxCode, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(displayName, rowstyle.ToString());
               
                #region "Agency"
                if (cells[CellType.AgencyStart])
                    row += GetAgencyCells(idUser, (item.Ps == null) ? null : item.Ps.StartDate, cells[CellType.IdAgency], rowstyle);
                if (cells[CellType.AgencyEnd])
                    row += GetAgencyCells(idUser, (item.Ps == null) ? null : item.Ps.EndDate, cells[CellType.IdAgency], rowstyle);
                if (cells[CellType.AgencyCurrent])
                    row += GetAgencyCells(idUser, nDate, cells[CellType.IdAgency], rowstyle);
                #endregion

                if (cells[CellType.IdOrganization])
                    row += BuilderXmlDocument.AddData(item.IdOrganization, rowstyle.ToString());
                if (cells[CellType.OrganizationName])
                    row += BuilderXmlDocument.AddData((String.IsNullOrEmpty(item.OrganizationName) ? String.Format(CommonTranslations[EduPathTranslations.DeletedCommunity], item.IdOrganization) : item.OrganizationName), rowstyle.ToString());
                if (cells[CellType.IdCommunity])
                    row += BuilderXmlDocument.AddData(item.IdCommunity, rowstyle.ToString());
                if (cells[CellType.CommunityName])
                    row += BuilderXmlDocument.AddData((String.IsNullOrEmpty(item.CommunityName) ? String.Format(CommonTranslations[EduPathTranslations.DeletedCommunity], item.IdCommunity) : item.CommunityName), rowstyle.ToString());
                if (cells[CellType.IdPath])
                    row += BuilderXmlDocument.AddData(item.IdPath, rowstyle.ToString());
                if (cells[CellType.PathName])
                    row += BuilderXmlDocument.AddData(item.PathName, rowstyle.ToString());

                if (cells[CellType.StartDate]){
                    if (item.PathInfo== null || !item.PathInfo.StartDate.HasValue)
                        row += BuilderXmlDocument.AddData("",rowstyle.ToString());
                    else
                        row += BuilderXmlDocument.AddData(item.PathInfo.StartDate.Value,rowstyle.ToString());
                }
                if (cells[CellType.EndDate])
                {
                    if (item.EndDate == null || !item.PathInfo.EndDate.HasValue)
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    else
                        row += BuilderXmlDocument.AddData(item.PathInfo.EndDate.Value, rowstyle.ToString());
                }

                if (HelperExportTo.CheckEpType(item.PathType, EPType.Time))
                    row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.PathInfo.Weight), rowstyle.ToString());
                else if (cells[CellType.Time])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (HelperExportTo.CheckEpType(item.PathType, EPType.Mark))
                    row += BuilderXmlDocument.AddData(item.PathInfo.Weight, rowstyle.ToString());
                else if (cells[CellType.Mark])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                row += BuilderXmlDocument.AddData(item.PathInfo.MinCompletion, rowstyle.ToString());
                if (item.Ps !=null)
                    row += BuilderXmlDocument.AddData(item.Ps.Completion, rowstyle.ToString());
                else 
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                
                switch(item.ItemStatus){
                    case StatusStatistic.Completed:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Completed], rowstyle.ToString());
                        break;
                    case StatusStatistic.Passed:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Passed], rowstyle.ToString());
                        break;
                    case StatusStatistic.CompletedPassed:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CompletedPassed], rowstyle.ToString());
                        break;
                    case StatusStatistic.Started:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Started], rowstyle.ToString());
                        break;
                    default:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.NotStarted], rowstyle.ToString());
                        break;
                }

                if (cells[CellType.ViewedOn])
                {
                    if (item.Ps != null && item.Ps.StartDate.HasValue)
                        row += BuilderXmlDocument.AddData(item.Ps.StartDate.Value, rowstyle.ToString());
                    else
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                if (item.PsFirstActivity != null && item.PsFirstActivity.CreatedOn.HasValue)
                    row += BuilderXmlDocument.AddData(item.PsFirstActivity.CreatedOn.Value, rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (item.Ps != null && item.Ps.EndDate.HasValue)
                    row += BuilderXmlDocument.AddData(item.Ps.EndDate.Value, rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                String itemRow = row;

                if (cells[CellType.QuizCells])
                {
                    if (item.Questionnaires.Any())
                    {
                        String rows = "";
                        foreach (dtoUserPathQuiz questionnaire in item.Questionnaires)
                        {
                            rows += GetQuestionnaireRow(row, questionnaire, cells, rowstyle);
                        }
                        return rows;
                    }
                    else
                    {
                        row += GetNoQuestionnaireRow(cells, rowstyle);
                        return BuilderXmlDocument.AddRow(row);
                    }
                }
                else
                    return BuilderXmlDocument.AddRow(row);
            }
            private String GetQuestionnaireRow(String itemRow, dtoUserPathQuiz questionnaire,Dictionary<CellType, Boolean> cells, DefaultXmlStyleElement rowstyle)
            {
                String result = "";
                String questRow = itemRow;
                questRow += BuilderXmlDocument.AddData(questionnaire.QuestionnaireInfo.Name, rowstyle.ToString());
                if (cells[CellType.WithEvaluations])
                {
                    if (questionnaire.QuestionnaireInfo.EvaluationActive)
                    {
                        questRow += BuilderXmlDocument.AddData(questionnaire.QuestionnaireInfo.MinScore, rowstyle.ToString());
                        questRow += BuilderXmlDocument.AddData(questionnaire.QuestionnaireInfo.EvaluationScale, rowstyle.ToString());
                    }
                    else {
                        questRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                        questRow += BuilderXmlDocument.AddData("", rowstyle.ToString()); 
                    }
                }

                if (cells[CellType.QuizAttempts] && questionnaire.QuestionnaireInfo.AllowMultipleAttempts){
                    if (questionnaire.QuestionnaireInfo.MaxAttemptsNumber == 0)
                        questRow += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.QuestionnaireAttemptsNoLimits] , rowstyle.ToString());
                    else
                        questRow += BuilderXmlDocument.AddData(questionnaire.QuestionnaireInfo.MaxAttemptsNumber, rowstyle.ToString());
                }
                else if (cells[CellType.QuizAttempts])
                    questRow += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (questionnaire.Answers.Any())
                {
                    foreach (dtoUserQuizAnswer answer in questionnaire.Answers)
                    {
                        result += GetAnswerRow(questRow,questionnaire.QuestionnaireInfo, answer, cells, rowstyle);
                    }
                }
                else
                {
                    questRow += GetNoAnswerRow(cells, rowstyle);
                    result = BuilderXmlDocument.AddRow(questRow);
                }
                return result;
            }
            private String GetNoQuestionnaireRow(Dictionary<CellType, Boolean> cells, DefaultXmlStyleElement rowstyle)
            {
                String row = "";
                row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.WithEvaluations])
                {
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }

                if (cells[CellType.QuizAttempts])
                {
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                if (cells[CellType.QuizCompleted])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (cells[CellType.NoEvaluations])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.WithEvaluations])
                {
                    if (cells[CellType.CorrectAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.SemiCorrectAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.UngradedAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.QuestionsSkipped])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.WrongAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                return row;
            }
            private String GetAnswerRow(String questRow, dtoUserQuizDetails info, dtoUserQuizAnswer answer, Dictionary<CellType, Boolean> cells, DefaultXmlStyleElement rowstyle)
            {
                String result = "";
                String answerRow = questRow;
                if (cells[CellType.WithEvaluations] && info.EvaluationActive && answer.RelativeScore.HasValue)
                    answerRow += BuilderXmlDocument.AddData(answer.RelativeScore, rowstyle.ToString());
                else if (cells[CellType.WithEvaluations])
                    answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.QuizAttempts] && info.AllowMultipleAttempts)
                    answerRow += BuilderXmlDocument.AddData(answer.AttemptNumber, rowstyle.ToString());
                else if (cells[CellType.QuizAttempts])
                    answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.QuizCompleted] && answer.CompletedOn.HasValue)
                    answerRow += BuilderXmlDocument.AddData(answer.CompletedOn.Value, rowstyle.ToString());
                else if (cells[CellType.QuizCompleted])
                    answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (cells[CellType.QuestionsCount] && answer.CompletedOn.HasValue)
                    answerRow += BuilderXmlDocument.AddData(answer.QuestionsCount, rowstyle.ToString());

                if (cells[CellType.NoEvaluations] && !info.EvaluationActive)
                    answerRow += BuilderXmlDocument.AddData(answer.QuestionAnswers, rowstyle.ToString());
                else if (cells[CellType.NoEvaluations])
                    answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (cells[CellType.WithEvaluations])
                {
                    if (info.EvaluationActive && cells[CellType.CorrectAnswers])
                        answerRow += BuilderXmlDocument.AddData((answer.CorrectAnswers.HasValue) ? answer.CorrectAnswers.Value : 0, rowstyle.ToString());
                    else if (cells[CellType.CorrectAnswers])
                        answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());

                    if (cells[CellType.SemiCorrectAnswers] && info.EvaluationActive)
                        answerRow += BuilderXmlDocument.AddData((answer.SemiCorrectAnswers.HasValue) ? answer.SemiCorrectAnswers : 0, rowstyle.ToString());
                    else if (cells[CellType.SemiCorrectAnswers])
                        answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());

                    if (cells[CellType.UngradedAnswers] && info.EvaluationActive)
                        answerRow += BuilderXmlDocument.AddData((answer.UngradedAnswers.HasValue) ? answer.UngradedAnswers : 0, rowstyle.ToString());
                    else if (cells[CellType.UngradedAnswers])
                        answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.QuestionsSkipped] && info.EvaluationActive)
                        answerRow += BuilderXmlDocument.AddData((answer.QuestionsSkipped.HasValue) ? answer.QuestionsSkipped : 0, rowstyle.ToString());
                    else if (cells[CellType.QuestionsSkipped])
                        answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (info.EvaluationActive && cells[CellType.WrongAnswers])
                        answerRow += BuilderXmlDocument.AddData((answer.WrongAnswers.HasValue) ? answer.WrongAnswers : 0 , rowstyle.ToString());
                    else if (cells[CellType.WrongAnswers])
                        answerRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                result = BuilderXmlDocument.AddRow(answerRow);
                return result;
            }
            private String GetNoAnswerRow(Dictionary<CellType, Boolean> cells, DefaultXmlStyleElement rowstyle)
            {
                String row = "";
                if (cells[CellType.WithEvaluations])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.QuizAttempts])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.QuizCompleted])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.QuestionsCount])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (cells[CellType.NoEvaluations])
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (cells[CellType.WithEvaluations])
                {
                    if (cells[CellType.CorrectAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.SemiCorrectAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.UngradedAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.QuestionsSkipped])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (cells[CellType.WrongAnswers])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                return row;
            }

            private String GetAgencyCells(Int32 idUser, DateTime? date, Boolean showIdentifiers, DefaultXmlStyleElement rowstyle)
            {
                String cells = "";
                if (date.HasValue)
                {
                    LazyAffiliation af = Manager.GetUserAffiliation(idUser, date.Value);

                    if (showIdentifiers && af != null && af.Agency != null)
                        cells += BuilderXmlDocument.AddData(af.Agency.Id, rowstyle.ToString());
                    else if (showIdentifiers)
                        cells += BuilderXmlDocument.AddData("", rowstyle.ToString());

                    if (af != null && af.Agency != null)
                        cells += BuilderXmlDocument.AddData(af.Agency.Name, rowstyle.ToString());
                    else
                        cells += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                else {
                    if (showIdentifiers)
                        cells += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    cells += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                return cells;
            }
        #endregion

        #region "User Path Statistics"
            public String UserPathStatistics(ServiceStat service, long idPath, ItemType type, Int32 idUser, Int32 idCurrentUser, dtoItemWithStatistic statistics, DateTime onDate, dtoExportConfigurationSetting settings, ExporPathData exportData, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction)
            {
                String export = "";
                Boolean isTimeBased = false;
                Boolean isAutoEp = false;
                Path p = Manager.Get<Path>(idPath);
                if (p != null)
                {
                    isAutoEp = HelperExportTo.CheckEpType(p.EPType, EPType.Auto);
                    isTimeBased = HelperExportTo.CheckEpType(p.EPType, EPType.Time);
                }
                String header = UserPathStatisticsDisplayInfo(p, type, idUser, onDate, settings,exportData, statistics);
                String content = UserPathStatisticsTableHeader(isTimeBased, isAutoEp, type);
                Int32 rowNumber = 0;
                content += WriteUserPathStatisticsRows(service, p, type, idUser, idCurrentUser, onDate, statistics, quizAction, repAction, tAction, cAction, isTimeBased, isAutoEp, rowNumber);

                export += BuilderXmlDocument.AddWorkSheet("--", header + content);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private String UserPathStatisticsDisplayInfo(Path path, ItemType type, Int32 idUser, DateTime onDate, dtoExportConfigurationSetting settings, ExporPathData exportData, dtoItemWithStatistic statistics)
            {
                String tableData = "";
                String row = "";
                litePerson user = Manager.GetLitePerson(idUser);

                row = BuilderXmlDocument.AddData(HelperExportTo.GetItemType(type, CommonTranslations), DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(statistics.Name, DefaultXmlStyleElement.TitleItem.ToString());
                tableData = BuilderXmlDocument.AddRow(row);

                if (type != ItemType.Path)
                {
                    row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Mandatory], DefaultXmlStyleElement.TitleItem.ToString());
                    row += BuilderXmlDocument.AddData((HelperExportTo.CheckStatus(statistics.Status, Status.Mandatory) ? CommonTranslations[EduPathTranslations.YesOption] : CommonTranslations[EduPathTranslations.NoOption]), DefaultXmlStyleElement.TitleItem.ToString());
                    tableData += BuilderXmlDocument.AddRow(row);
                }

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.TitleUserInfo], DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(GetUserDisplayName(idUser, user), DefaultXmlStyleElement.TitleItem.ToString());
                tableData += BuilderXmlDocument.AddRow(row);

                if (settings.isRequiredField(ExportFieldType.TaxCodeInfo, exportData) && (user != null && user.TypeID == (int)UserTypeStandard.Guest))
                {
                    row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.TitleUserInfoTaxCode], DefaultXmlStyleElement.TitleItem.ToString());
                    row += BuilderXmlDocument.AddData(user.TaxCode, DefaultXmlStyleElement.TitleItem.ToString());
                    tableData += BuilderXmlDocument.AddRow(row);
                }
                //if (type == ItemType.Path) { 
                //    statistics.

                //}

                tableData += BuilderXmlDocument.AddRow("");

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.TitleStatisticsDate], DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(onDate.ToShortDateString() + " " + onDate.ToShortTimeString(), DefaultXmlStyleElement.TitleItem.ToString());
                tableData += BuilderXmlDocument.AddRow(row);

                tableData += BuilderXmlDocument.AddRow("");

                return tableData;
            }
            private String UserPathStatisticsTableHeader(Boolean isTimeBased, Boolean isAutoEp, ItemType type)
            {
                String tableData = "";
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.LevelType]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Type]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Name]);

                if (type != ItemType.SubActivity)
                {
                    if (isTimeBased)
                    {
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Time]);
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.MinTime]);
                    }
                    else {
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Completion]);
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.MinCompletion]);
                    }
                    if (!isAutoEp) {
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Mark]);
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.MinMark]);
                    }
                }
                else
                {
                    if (isTimeBased)
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Time]);
                    else
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Completion]);
                    if (!isAutoEp)
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Mark]);
                }
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Status]);
                return BuilderXmlDocument.AddRow(tableData);
            }
            private String WriteUserPathStatisticsRows(ServiceStat service, Path path, ItemType type, Int32 idUser, Int32 idCurrentUser, DateTime onDate, dtoItemWithStatistic statistics, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction, Boolean isTimeBased, Boolean isAutoEp, Int32 rowNumber)
            {
                String tableData = "";
                String row = "";
                DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem);
                rowNumber++;
                row += BuilderXmlDocument.AddData((int)type, rowstyle.ToString());
                row += BuilderXmlDocument.AddData(HelperExportTo.GetType(type, CommonTranslations), rowstyle.ToString());
                dtoActivityStatistic subStatistics = null;

                if (type == ItemType.Activity)
                {
                    Int32 idCommunity = (path != null && path.Community != null) ? path.Community.Id : 0;
                    Int32 idRole = (idCommunity == 0) ? 0 : Manager.GetSubscriptionIdRole(idCurrentUser, idCommunity);

                    subStatistics = service.GetDtoActivityStatistic_ByUser(statistics.Id, idCommunity, idUser, idCurrentUser, idRole, "", "", onDate);
                }
                if (subStatistics != null && subStatistics.SubActivities != null && subStatistics.SubActivities.Count == 1 && subStatistics.SubActivities[0].SubActivity != null)
                    row += BuilderXmlDocument.AddData(HelperExportTo.GetSubActivityName(CommonTranslations, subStatistics.SubActivities[0].SubActivity, quizAction, repAction, tAction, cAction), rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData(statistics.Name, rowstyle.ToString());
                switch (type)
                {
                    case ItemType.Unit:
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                        if (!isAutoEp) {
                            row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                            row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                        }
                        break;
                    case ItemType.SubActivity:
                        if (isTimeBased)
                            row += BuilderXmlDocument.AddData("-", rowstyle.ToString());
//                            row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(statistics.Completion), rowstyle.ToString());
                        else
                            row += BuilderXmlDocument.AddData(statistics.Completion, rowstyle.ToString());
                        if (!isAutoEp)
                            row += BuilderXmlDocument.AddData(statistics.Mark, rowstyle.ToString());

                        break;
                    default:
                         if (isTimeBased){
                            if (type == ItemType.Unit)
                            {
                                row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                                row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                            }
                            else
                            {
                                row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(statistics.Completion), rowstyle.ToString());
                                row += BuilderXmlDocument.AddData(HelperExportTo.GetMinTime(statistics.Weight, statistics.MinCompletion), rowstyle.ToString());
                            }
                        }
                        else {
                            row += BuilderXmlDocument.AddData(statistics.Completion, rowstyle.ToString());
                            row += BuilderXmlDocument.AddData(statistics.MinCompletion, rowstyle.ToString());
                        }
                        if (!isAutoEp) {
                            row += BuilderXmlDocument.AddData(statistics.Mark, rowstyle.ToString());
                            row += BuilderXmlDocument.AddData(statistics.MinMark, rowstyle.ToString());
                        }
                        break;
                }
                row += BuilderXmlDocument.AddData(HelperExportTo.GetStatus(HelperExportTo.GetStatusForUnit(type,statistics,isTimeBased ), CommonTranslations), rowstyle.ToString());
                tableData += BuilderXmlDocument.AddRow(row);

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
                        tableData += WriteUserPathStatisticsRows(service, path, childrenType, idUser, idCurrentUser, onDate, childStatistics, quizAction, repAction, tAction, cAction, isTimeBased, isAutoEp, rowNumber);
                    }
                }
                else if (childrenType == ItemType.SubActivity && subStatistics.SubActivities != null && subStatistics.SubActivities.Count > 1)
                    tableData += WriteUserPathSubActivityStatistics(service, path, subStatistics, idUser, idCurrentUser, onDate, quizAction,repAction, tAction, cAction, isTimeBased, isAutoEp,rowNumber );

                return tableData;
            }
            private String WriteUserPathSubActivityStatistics(ServiceStat service, Path path, dtoActivityStatistic statistics, Int32 idUser, Int32 idCurrentUser, DateTime onDate, lm.Comol.Core.ModuleLinks.IGenericModuleDisplayAction quizAction, lm.Comol.Core.ModuleLinks.IViewModuleRenderAction repAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleTextAction tAction, lm.Comol.Modules.EduPath.Presentation.IViewModuleCertificationAction cAction, Boolean isTimeBased, Boolean isAutoEp, Int32 rowNumber)
            {
                DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem);
                String result = "";
                if (statistics != null && statistics.SubActivities.Any())
                {
                    foreach (var stat in statistics.SubActivities)
                    {
                        String row = "";
                        row += BuilderXmlDocument.AddData((int)ItemType.SubActivity, rowstyle.ToString());
                        row += BuilderXmlDocument.AddData(HelperExportTo.GetType(ItemType.SubActivity, CommonTranslations), rowstyle.ToString());

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
                        row += BuilderXmlDocument.AddData(name, rowstyle.ToString());
                        if (isTimeBased){
                            row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                            row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                        }
                        else
                            row += BuilderXmlDocument.AddData(stat.Completion, rowstyle.ToString());
                        if (!isAutoEp)
                            row += BuilderXmlDocument.AddData(stat.Mark, rowstyle.ToString());

                        row += BuilderXmlDocument.AddData(HelperExportTo.GetStatus(stat.StatusStat, CommonTranslations), rowstyle.ToString());
                        result += BuilderXmlDocument.AddRow(row);
                        rowNumber++;
                    }
                }
                return result;
            }

        #endregion
        #region "Path Statistics"
            public String PathUsersStatistics(Int32 idUser, DateTime onDate, dtoPathUsers statistics, List<dtoBaseUserPathQuiz> qInfos, dtoExportConfigurationSetting settings, ExporPathData exportData)
            {
                DateTime nDate = DateTime.Now;
                String export = " ";
                String content = "";
                litePerson user = Manager.GetLitePerson(idUser);
                String displayName = GetUserDisplayName(idUser, user);
                String header = PathUsersStatisticsDisplayInfo(statistics.PathName, onDate, statistics);
                int rowNumber = 0;

                Dictionary<CellType, Boolean> cells = Helper.GetAvaliableCells(nDate, statistics, qInfos, settings, exportData);

                content += PathUsersStatisticsTableHeader(statistics, cells);
                rowNumber++;
                foreach (dtoPathUserInfo item in statistics.Users)
                {
                    content += PathUsersStatisticsRow(nDate, statistics.PathType,statistics.Questionnaires, item, cells, rowNumber);
                    rowNumber++;
                }

                export += BuilderXmlDocument.AddWorkSheet("--", header + content);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private String PathUsersStatisticsDisplayInfo(String pathName, DateTime onDate, dtoPathUsers statistics)
            {
                String tableData = "";
                String row = "";

                row = BuilderXmlDocument.AddData(String.Format(CommonTranslations[EduPathTranslations.StatisticsInfo], onDate.ToShortDateString() + " " + onDate.ToShortTimeString()), DefaultXmlStyleElement.TitleItem.ToString(), 4);
                tableData += BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathNameInfo], DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(pathName, DefaultXmlStyleElement.TitleItem.ToString(), 4);
                tableData = BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathMinCompletionInfo], DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(statistics.PathInfo.MinCompletion + "%", DefaultXmlStyleElement.TitleItem.ToString(), 4);
                tableData += BuilderXmlDocument.AddRow(row);

                if( HelperExportTo.CheckEpType(statistics.PathType, EPType.Time)){
                    row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathTimeInfo], DefaultXmlStyleElement.TitleItem.ToString());
                    row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(statistics.PathInfo.Weight), DefaultXmlStyleElement.TitleItem.ToString(), 4);
                    tableData += BuilderXmlDocument.AddRow(row);
                }
                
                if( HelperExportTo.CheckEpType(statistics.PathType, EPType.Mark)){
                    row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathMinMarkInfo], DefaultXmlStyleElement.TitleItem.ToString());
                    row += BuilderXmlDocument.AddData(statistics.PathInfo.Weight, DefaultXmlStyleElement.TitleItem.ToString(), 4);
                    tableData += BuilderXmlDocument.AddRow(row);
                }



                row = BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathStatisticInfos], DefaultXmlStyleElement.TitleItem.ToString(), 4);
                tableData += BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData("");
                row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathUsersCompleted], DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(statistics.Completed, DefaultXmlStyleElement.TitleItem.ToString(),2);
                tableData += BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData("");
                row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathUsersStarted], DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(statistics.Started, DefaultXmlStyleElement.TitleItem.ToString(), 2);
                tableData += BuilderXmlDocument.AddRow(row);

                row = BuilderXmlDocument.AddData("");
                row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.DisplayPathUsersNotStarted], DefaultXmlStyleElement.TitleItem.ToString());
                row += BuilderXmlDocument.AddData(statistics.NotStarted, DefaultXmlStyleElement.TitleItem.ToString(), 2);
                tableData += BuilderXmlDocument.AddRow(row);

                return tableData;
            }
            private String PathUsersStatisticsTableHeader(dtoPathUsers statistics, Dictionary<CellType, Boolean> cells)
            {
                String tableData = "";
                if (cells[CellType.IdUser])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdUser]);
                if (cells[CellType.UserTaxCode])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleTaxCode]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.SurnameAndName]);
                
                #region "Agency"
                if (cells[CellType.AgencyStart]){
                    if (cells[CellType.IdAgency])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdStartAgency]);
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleStartAgencyName]);
                }
                if (cells[CellType.AgencyEnd])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdEndAgency]);
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleEndAgencyName]);
                }
                if (cells[CellType.AgencyCurrent])
                {
                    if (cells[CellType.IdAgency])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdCurrentAgency]);
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleCurrentAgencyName]);
                }
                #endregion

                //if (cells[CellType.Identifiers])
                //    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdCommunity]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleCommunityName]);
                //if (cells[CellType.Identifiers])
                //    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleIdPath]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitlePathName]);
               
                //if (cells[CellType.StartDate])
                //    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitlePathAvailableFrom]);
                //if (cells[CellType.EndDate])
                //    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitlePathAvailableTo]);

                if (cells[CellType.Time])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Time]);
                if (cells[CellType.Mark])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Weight]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.MinCompletion]);
                //tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Completion]);

                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.Status]);
                if (cells[CellType.ViewedOn])
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleViewedOn]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleStartedOn]);
                tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleCompletedOn]);
                if (cells[CellType.QuizCells])
                {
                    tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizName]);
                    if (cells[CellType.WithEvaluations])
                    {
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleMinScore]);
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleMaxScore]);

                    }
                    if (cells[CellType.QuizAttempts])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptsNumber]);

                    if (cells[CellType.WithEvaluations])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleAttemptScore]);
                    if (cells[CellType.QuizAttempts])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptNumber]);
                    if (cells[CellType.QuizCompleted])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAttemptDate]);

                    if (cells[CellType.QuestionsCount])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsCount]);

                    if (cells[CellType.NoEvaluations])
                        tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizAnswersCount]);
                    if (cells[CellType.WithEvaluations])
                    {
                        if (cells[CellType.CorrectAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizCorrectAnswers]);
                        
                        if (cells[CellType.SemiCorrectAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizSemiCorrectAnswers]);
                        if (cells[CellType.UngradedAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizUngradedAnswers]);
                        if (cells[CellType.QuestionsSkipped])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizQuestionsSkipped]);

                        if (cells[CellType.WrongAnswers])
                            tableData += AddTableCellTitle(CommonTranslations[EduPathTranslations.CellTitleQuizWrongAnswers]);

                    }
                }
                return BuilderXmlDocument.AddRow(tableData);
            }
            private String PathUsersStatisticsRow(DateTime nDate,EPType pathType,IList<dtoSubActivityQuestionnaire> questionnaires, dtoPathUserInfo item, Dictionary<CellType, Boolean> cells, int rowNumber)
            {
                DefaultXmlStyleElement rowstyle = (rowNumber % 2 == 0 ? DefaultXmlStyleElement.RowAlternatingItem : DefaultXmlStyleElement.RowItem);

                String row = "";
                if (cells[CellType.IdUser])
                    row += BuilderXmlDocument.AddData(item.IdPerson, rowstyle.ToString());
                if (cells[CellType.UserTaxCode])
                    row += BuilderXmlDocument.AddData((item.User == null) ? "" : item.User.TaxCode, rowstyle.ToString());
                row += BuilderXmlDocument.AddData((item.User==null) ? String.Format(CommonTranslations[EduPathTranslations.DeletedUser], item.IdPerson) : item.User.SurnameAndName, rowstyle.ToString());
               
                #region "Agency"

                if (cells[CellType.AgencyStart])
                    row += GetAgencyCells(item.IdPerson, item.StStartDate, cells[CellType.IdAgency], rowstyle);
                if (cells[CellType.AgencyEnd])
                    row += GetAgencyCells(item.IdPerson, item.StEndDate, cells[CellType.IdAgency], rowstyle);
                if (cells[CellType.AgencyCurrent])
                    row += GetAgencyCells(item.IdPerson, nDate, cells[CellType.IdAgency], rowstyle);
                #endregion

                //if (cells[CellType.Identifiers])
                //    row += BuilderXmlDocument.AddData(idCommunity, rowstyle.ToString());
                //row += BuilderXmlDocument.AddData((String.IsNullOrEmpty(item.CommunityName) ? String.Format(CommonTranslations[EduPathTranslations.DeletedCommunity], item.IdCommunity) : item.CommunityName), rowstyle.ToString());
                //if (cells[CellType.Identifiers])
                //    row += BuilderXmlDocument.AddData(item.IdPath, rowstyle.ToString());
                //row += BuilderXmlDocument.AddData(item.PathName, rowstyle.ToString());

                //if (cells[CellType.StartDate]){
                //    if (item.PathInfo== null || !item.PathInfo.StartDate.HasValue)
                //        row += BuilderXmlDocument.AddData("",rowstyle.ToString());
                //    else
                //        row += BuilderXmlDocument.AddData(item.PathInfo.StartDate.Value,rowstyle.ToString());
                //}
                //if (cells[CellType.EndDate])
                //{
                //    if (item.EndDate == null || !item.PathInfo.EndDate.HasValue)
                //        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                //    else
                //        row += BuilderXmlDocument.AddData(item.PathInfo.EndDate.Value, rowstyle.ToString());
                //}

                //if (HelperExportTo.CheckEpType(item.PathType, EPType.Time))
                //    row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.PathInfo.Weight), rowstyle.ToString());
                //else if (cells[CellType.Time])
                //    row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                //if (HelperExportTo.CheckEpType(item.PathType, EPType.Mark))
                //    row += BuilderXmlDocument.AddData(item.PathInfo.Weight, rowstyle.ToString());
                //else if (cells[CellType.Mark])
                //    row += BuilderXmlDocument.AddData("", rowstyle.ToString());

                //row += BuilderXmlDocument.AddData(item.PathInfo.MinCompletion, rowstyle.ToString());
                if (item.Completion != null)
                {
                    if (HelperExportTo.CheckEpType(pathType, EPType.Time))
                        row += BuilderXmlDocument.AddData(HelperExportTo.GetTime(item.Completion), rowstyle.ToString());
                    else if (cells[CellType.Time])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    if (HelperExportTo.CheckEpType(pathType, EPType.Mark))
                        row += BuilderXmlDocument.AddData(item.Completion, rowstyle.ToString());
                    else if (cells[CellType.Mark])
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                else
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                
                switch(item.ItemStatus){
                    case StatusStatistic.Completed:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Completed], rowstyle.ToString());
                        break;
                    case StatusStatistic.Passed:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Passed], rowstyle.ToString());
                        break;
                    case StatusStatistic.CompletedPassed:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.CompletedPassed], rowstyle.ToString());
                        break;
                    case StatusStatistic.Started:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.Started], rowstyle.ToString());
                        break;
                    default:
                        row += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.NotStarted], rowstyle.ToString());
                        break;
                }

                if (cells[CellType.ViewedOn])
                {
                    if (item.StStartDate.HasValue)
                        row += BuilderXmlDocument.AddData(item.StStartDate.Value, rowstyle.ToString());
                    else
                        row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                }
                if (item.StFirstActivity.HasValue)
                    row += BuilderXmlDocument.AddData(item.StFirstActivity.Value, rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                if (item.StEndDate.HasValue)
                    row += BuilderXmlDocument.AddData(item.StEndDate.Value, rowstyle.ToString());
                else
                    row += BuilderXmlDocument.AddData("", rowstyle.ToString());
                String itemRow = row;

                if (cells[CellType.QuizCells])
                {
                    if (item.Questionnaires.Any())
                    {
                        String rows = "";
                        foreach (dtoBaseUserPathQuiz questionnaire in item.Questionnaires)
                        {
                            rows += GetQuestionnaireRow(row,questionnaires.Where(q=> q.IdQuestionnaire== questionnaire.IdQuestionnaire).Select(q=>q.QuestionnaireInfo).FirstOrDefault(), questionnaire, cells, rowstyle);
                        }
                        return rows;
                    }
                    else
                    {
                        row += GetNoQuestionnaireRow(cells, rowstyle);
                        return BuilderXmlDocument.AddRow(row);
                    }
                }
                else
                    return BuilderXmlDocument.AddRow(row);
            }
            private String GetQuestionnaireRow(String itemRow,dtoUserQuizDetails qInfo, dtoBaseUserPathQuiz questionnaire, Dictionary<CellType, Boolean> cells, DefaultXmlStyleElement rowstyle)
            {
                String result = "";
                String questRow = itemRow;
                questRow += BuilderXmlDocument.AddData(qInfo.Name, rowstyle.ToString());
                if (cells[CellType.WithEvaluations])
                {
                    if (qInfo.EvaluationActive)
                    {
                        questRow += BuilderXmlDocument.AddData(qInfo.MinScore, rowstyle.ToString());
                        questRow += BuilderXmlDocument.AddData(qInfo.EvaluationScale, rowstyle.ToString());
                    }
                    else
                    {
                        questRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                        questRow += BuilderXmlDocument.AddData("", rowstyle.ToString());
                    }
                }

                if (cells[CellType.QuizAttempts] && qInfo.AllowMultipleAttempts)
                {
                    if (qInfo.MaxAttemptsNumber == 0)
                        questRow += BuilderXmlDocument.AddData(CommonTranslations[EduPathTranslations.QuestionnaireAttemptsNoLimits], rowstyle.ToString());
                    else
                        questRow += BuilderXmlDocument.AddData(qInfo.MaxAttemptsNumber, rowstyle.ToString());
                }
                else if (cells[CellType.QuizAttempts])
                    questRow += BuilderXmlDocument.AddData("", rowstyle.ToString());

                if (questionnaire.Answers.Any())
                {
                    foreach (dtoUserQuizAnswer answer in questionnaire.Answers)
                    {
                        result += GetAnswerRow(questRow, qInfo, answer, cells, rowstyle);
                    }
                }
                else
                {
                    questRow += GetNoAnswerRow(cells, rowstyle);
                    result = BuilderXmlDocument.AddRow(questRow);
                }
                return result;
            }
        #endregion
        #region "Styles"
            private String AddTableCellTitle(String data){
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.HeaderTable.ToString());    
            }
            private String AddTableCellTitle(String data, Int32 colspan)
            {
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.HeaderTable.ToString(), colspan);
            }
            private String AddTableCellSubTitle(String data)
            {
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.SubHeaderTable.ToString());
            }
            private String AddTableCellSubTitle(String data, Int32 colspan)
            {
                return BuilderXmlDocument.AddData(data, DefaultXmlStyleElement.SubHeaderTable.ToString(), colspan);
            }
            //public enum EvaluationsStyle
            //{
            //    HeaderTable = 1,
            //    RowItem = 2,
            //    RowAlternatingItem = 3,
            //    RowDeleted = 4,
            //    ItemEmpty = 5,
            //    ItemEmptyAlternate = 6,
            //    ItemYellow = 7,
            //    ItemRed = 8,
            //    CallForPaperTitle = 9,
            //    PrintInfo = 10
            //}
        #endregion

        public enum ExportContentType { 
            PathStatistics = 1,
            UsersStatistics = 2,
        }

        #region Common
            public static String GetErrorDocument(String translation,String statInfoTranslation, DateTime? statisticsDate)
            {
                String export = "";
                String content = "";
                content += WriteStatisticsHeader(statInfoTranslation,statisticsDate);
                String row = BuilderXmlDocument.AddData(translation, DefaultXmlStyleElement.TitleItem.ToString(), 6);
                content += BuilderXmlDocument.AddRow(row);
                content += BuilderXmlDocument.AddEmptyRows(1);
                export += BuilderXmlDocument.AddWorkSheet("--", content);
                return BuilderXmlDocument.AddMain(export, BuilderXmlStyle.GetDefaulSettings());
            }
            private static String WriteStatisticsHeader(String statInfoTranslation,DateTime? statisticsDate)
            {
                String content = "";
                String row = "";
                if (!statisticsDate.HasValue)
                    statisticsDate = DateTime.Now;
                row = BuilderXmlDocument.AddData(String.Format(statInfoTranslation, statisticsDate.Value.ToShortDateString() + " " + statisticsDate.Value.ToShortTimeString()));
                content += BuilderXmlDocument.AddEmptyRows(1);
                content += BuilderXmlDocument.AddRow(row);
                content += BuilderXmlDocument.AddEmptyRow();
                return content;
            }
        #endregion
     }
}