using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.BusinessLogic
{
    public partial class RootObjectMoocs
    {

        #region Page

        #region Summary
            private static String _SummaryIndex = "Modules/Moocs/EPSummaryIndex.aspx?summarytype={0}{1}";
            private static String _SummaryUser  = "Modules/Moocs/EPSummaryUser.aspx?from={0}";
            private static String _MySummary = "Modules/Moocs/EPSummaryUser.aspx";
            private static String _ParamCmnt = "idcommunity={0}";
            private static String _ParamUser = "iduser={0}";
            private static String _ParamFromSummary = "from={0}";
            private static String _ParamFromSummaryCommunity = "fromIdCommunity={0}";
            private static String _ParamFromSummaryIndex = "fromIndex={0}";
            private static String _ParamSummarytype= "summarytype={0}";

            private static String _SummaryCommunity = "Modules/Moocs/EPSummaryCommunity.aspx?from={0}&idcommunity={1}&type={2}";
            private static String _SummaryPath = "Modules/Moocs/EPSummaryMooc.aspx?from={0}&idcommunity={1}";

            public static string SummaryIndex(SummaryType type)
            {
                return string.Format(_SummaryIndex, type.ToString(),"");
            }
            public static string SummaryIndex(SummaryType type, Int32 idCommunity)
            {
                return string.Format(_SummaryIndex, type.ToString(), (idCommunity > 0) ? string.Format(_ParamCmnt, idCommunity) : "");
            }

            public static string SummaryGeneric(SummaryType fromType, Int32 idCommunity, SummaryType current)
            {
                return string.Format(_SummaryCommunity, fromType.ToString(), idCommunity, current.ToString());
            }
            public static string SummaryCommunity(SummaryType fromType, Int32 idCommunity)
            {
                return string.Format(_SummaryCommunity, fromType.ToString(), idCommunity, SummaryType.Community.ToString());
            }
            public static string SummaryOrganization(SummaryType fromType,Int32 idCommunity)
            {
                return string.Format(_SummaryCommunity, fromType.ToString(),  idCommunity, SummaryType.Organization.ToString());
            }
            public static string SummaryPath(SummaryType fromType,  Int32 idCommunity)
            {
                return string.Format(_SummaryPath, fromType.ToString(), idCommunity);
            }
            public static string SummaryUser(SummaryType fromType,  Int32 userId = 0, Int32 idCommunity = 0)
            {
                string mybaseurl = String.Format(_SummaryUser,fromType.ToString());

                mybaseurl += (userId == 0) ? "" : "&" +String.Format(_ParamUser, userId);
                mybaseurl += (idCommunity == 0) ? "" : "&" + String.Format(_ParamCmnt, idCommunity);
                return mybaseurl;
            }
            public static string MySummary(Int32 userId = 0, Int32 idCommunity = 0)
            {
                string mybaseurl = _MySummary;

                string userPrm = (userId == 0) ? "" : "&" + String.Format(_ParamUser, userId);
                string cmntPrm = (idCommunity == 0) ? "" : "&" + String.Format(_ParamCmnt, idCommunity);

                if (userPrm == "" && cmntPrm == "")
                {
                    return mybaseurl;
                }
                if (userPrm != "" && cmntPrm == "")
                {
                    return string.Format("{0}?{1}", mybaseurl, userPrm);
                }
                if (userPrm != "" && cmntPrm != "")
                {
                    return string.Format("{0}?{1}&{2}", mybaseurl, userPrm, cmntPrm);
                }
                if (userPrm == "" && cmntPrm != "")
                {
                    return string.Format("{0}?{1}", mybaseurl, cmntPrm);
                }
                return mybaseurl;
            }

            private static String _SearchUsersForModule = "Modules/ProfileManagement/SearchUsersForModule.aspx";
            private static String _SearchUsersForModuleParams = _SearchUsersForModule + "?module={0}&from={1}&idCommunity={2}";


        // ATTENZIONE SE LA COM§UNITA <> 0 ALLORA FORSE DOVRA ESSERE SVILUPPATO UN SEARCHE USER PER IL MODULO "EDUPATH".. DA PENSARCI SU CON CALMA
            public static string SearchUsersForModule()
            {
                return _SearchUsersForModule;
            }

            public static string SearchUsersForModule(SummaryType type, Int32 idCommunity = 0)
            {
                return String.Format(_SearchUsersForModuleParams, ModuleEduPath.UniqueCode, type.ToString(), idCommunity.ToString());
            }
        #endregion

        #region "Path Statistics"
            private static String _PathStatistics = "Modules/Moocs/MoocStatistics.aspx?PId={0}&ComId={1}&ShowAll={2}";
            private static String _PathSummaryGen = "Modules/Moocs/MoocSummary.aspx";
            private static String _PathSummary = "Modules/Moocs/MoocSummary.aspx?PId={0}&UserId={1}&ComId={2}";
            private static String _PathSummaryCommunity = "Modules/Moocs/MoocSummary.aspx?ComId={0}";
            private static String _PathSummarySearch = "Modules/Moocs/MoocSummary.aspx?PId={0}&UserId={1}&ComId={2}&Page={3}&RId={4}&Search={5}";
            
            private static String _UserPathSummary = "Modules/Moocs/UserMoocSummary.aspx?UserId={0}&ComId={1}&from={2}";
            private static String _UserPathSummarySearch = "Modules/Moocs/UserMoocSummary.aspx?UserId={0}&ComId={1}&Page={2}&RId={3}&Search={4}";
            private static String _CurrentUserPathSummary = "Modules/Moocs/UserMoocSummary.aspx?ComId={0}";
            private static String _PathCertifications = "Modules/Moocs/MoocCertifications.aspx?PId={0}&ComId={1}";
            private static String _UsersStatistics = "Modules/Moocs/UserListStatistics.aspx?ItemId={0}&ComId={1}&It={2}&Page={3}&ShowAll={4}&ST={5}";
            private static String _UserStatistics = "Modules/Moocs/UserStatistics.aspx?IId={0}&ComId={1}&ViewMode={2}&Itype={3}&ShowAll={4}";
            private static String _UserActivityStatistics = "Modules/Moocs/UserActivityStatistics.aspx?PId={0}&AId={1}&ComId={2}&ViewMode={3}&ShowAll={4}";

            private static String _UserPath = "#u{0}p{1}";

            public static string UserPathAnchor(Int32 UserId, Int64 PathId)
            {
                return String.Format(_UserPath, UserId.ToString(), PathId.ToString());
            }

            public static string PathSummary()
            {
                return _PathSummaryGen;
            }

            public static string UserPathSummary(Int32 ComId, Boolean isMooc)
            {
                return String.Format(_CurrentUserPathSummary, ComId.ToString()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }

            public static string UserPathSummary(Int32 UserId, Int32 ComId, String from, Boolean isMooc)
            {
                return String.Format(_UserPathSummary,UserId.ToString(), ComId.ToString(),from) +(isMooc? "&isMooc=" + isMooc.ToString(): "");
            }

            public static string UserPathSummary(Int32 UserId, Int32 ComId, Int32 Page, Int32 RoleId, String Search, Boolean isMooc)
            {
                return String.Format(_UserPathSummarySearch, UserId.ToString(), ComId.ToString(), Page.ToString(), RoleId.ToString(), Search) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }

            public static string PathSummary(Int32 ComId, Int32 UserId, Int64 PathId, Int32 Page, Int32 RoleId, String Search, Boolean isMooc)
            {
                return String.Format(_PathSummarySearch, PathId.ToString(), UserId.ToString(), ComId.ToString(), Page.ToString(), RoleId.ToString(), Search) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }

            public static string PathSummary(Int32 ComId, Boolean isMooc)
            {
                return String.Format(_PathSummaryCommunity, ComId.ToString()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }

            public static string PathSummary(Int32 ComId, Int32 UserId, Int64 PathId, Boolean isMooc, Boolean anchor = false)
            {
                if (!anchor)
                {
                    return String.Format(_PathSummary, PathId.ToString(), UserId.ToString(), ComId.ToString());
                }
                else
                {
                    return String.Format(_PathSummary, PathId.ToString(), UserId.ToString(), ComId.ToString()) + UserPathAnchor(UserId, PathId);
                }
            }

            public static string PathStatistics(Int64 idPath, int idCommunity, DateTime? dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                String url = String.Format(_PathStatistics, idPath.ToString(), idCommunity.ToString(), showAllStatistics.ToString().ToLower()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                if (dateToView != null)
                    return url + "&ST=" + ((DateTime)dateToView).Ticks;
                else
                    return url;
            }
            public static string PathStatistics(Int64 idPath, int idCommunity, DateTime? dateToView, Boolean showAllStatistics, SummaryType fromSummary, Int32 fromSummaryIdCommunity, SummaryType fromSummaryIndex, Boolean isMooc)
            {
                String url = PathStatistics(idPath, idCommunity, dateToView, showAllStatistics, isMooc);
                if (fromSummary != SummaryType.Unknown)
                    url += "&" + String.Format(_ParamFromSummary, fromSummary.ToString()) + "&" + String.Format(_ParamFromSummaryCommunity, fromSummaryIdCommunity) + "&" + String.Format(_ParamFromSummaryIndex, fromSummaryIndex.ToString());
                return url;
            }
            public static string PathCertifications(Int64 idPath, int idCommunity, DateTime? dateToView, Boolean isMooc)
            {
                String url = String.Format(_PathCertifications, idPath.ToString(), idCommunity.ToString()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                if (dateToView != null)
                    return url + "&ST=" + ((DateTime)dateToView).Ticks;
                else
                    return url;
            }
            public static string PathSubActivityCertifications(Int64 idPath, Int64 idSubActivity, int idCommunity, DateTime? dateToView, Boolean isMooc)
            {
                String url = String.Format(_PathCertifications, idPath.ToString(), idCommunity.ToString()) + "&idSubA=" + idSubActivity.ToString() + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                if (dateToView != null)
                    return url + "&ST=" + ((DateTime)dateToView).Ticks;
                else
                    return url;
            }
            public static string UsersStatistics(Int64 idItem, int idCommunity, ItemType itemType, Int16 pageIndex, DateTime dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                return String.Format(_UsersStatistics, idItem.ToString(), idCommunity.ToString(), (int)itemType, pageIndex.ToString(), showAllStatistics.ToString().ToLower(), ((DateTime)dateToView).Ticks) + (isMooc ? "&isMooc=" + isMooc.ToString() : ""); ;
            }

            public static string UserStatisticsManage(Int64 idItem, int idCommunity, int UserId, ItemType type, Int16 PageIndex, DateTime? dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                String url = String.Format(_UserStatistics, idItem.ToString(), idCommunity.ToString(), (int)EpViewModeType.Manage, (Int16)type, showAllStatistics.ToString().ToLower()) + "&UsId=" + UserId + "&Pidx=" + PageIndex + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                if (dateToView != null)
                    return url + "&ST=" + ((DateTime)dateToView).Ticks;
                else
                    return url;
            }
            public static string UserStatisticsManage(Int64 idItem, int idCommunity, int UserId, ItemType type, Int16 PageIndex, DateTime? dateToView, Boolean showAllStatistics,  SummaryType fromSummary, Int32 fromSummaryIdCommunity, SummaryType fromSummaryIndex, Boolean isMooc)
            {
                String url = UserStatisticsManage(idItem, idCommunity, UserId, type, PageIndex, dateToView, showAllStatistics,  isMooc);
                if (fromSummary != SummaryType.Unknown)
                    url += "&" + String.Format(_ParamFromSummary, fromSummary.ToString()) + "&" + String.Format(_ParamFromSummaryCommunity, fromSummaryIdCommunity) + "&" + String.Format(_ParamFromSummaryIndex, fromSummaryIndex.ToString());
                return url;
            }
            public static string UserStatisticsManage(Int64 idItem, int idCommunity, int UserId, ItemType type, Int16 PageIndex, DateTime? dateToView, Boolean showAllStatistics, String returnurl, Boolean isMooc)
            {
                String url = String.Format(_UserStatistics, idItem.ToString(), idCommunity.ToString(), (int)EpViewModeType.Manage, (Int16)type, showAllStatistics.ToString().ToLower()) + "&UsId=" + UserId + "&Pidx=" + PageIndex;
                url += (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                url +="&ReturnUrl=" + returnurl;
                if (dateToView != null)
                    return url + "&ST=" + ((DateTime)dateToView).Ticks;
                else
                    return url;
            }
            public static string UserStatisticsView(Int64 idPath, int idCommunity, DateTime? dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                String url = String.Format(_UserStatistics, idPath.ToString(), idCommunity.ToString(), (int)EpViewModeType.View, (Int16)ItemType.Path, showAllStatistics.ToString().ToLower()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                if (dateToView != null)
                    return url + "&ST=" + ((DateTime)dateToView).Ticks;
                else
                    return url;
            }
            public static string UserStatisticsView(Int64 idPath, int idCommunity, DateTime? dateToView, Boolean showAllStatistics, SummaryType fromSummary, Int32 fromSummaryIdCommunity, SummaryType fromSummaryIndex, Boolean isMooc)
            {
                String url = UserStatisticsView(idPath, idCommunity, dateToView, showAllStatistics, isMooc);
                if (fromSummary != SummaryType.Unknown)
                    url += "&" + String.Format(_ParamFromSummary, fromSummary.ToString()) + "&" + String.Format(_ParamFromSummaryCommunity, fromSummaryIdCommunity) + "&" + String.Format(_ParamFromSummaryIndex, fromSummaryIndex.ToString());
                return url;
            }
            public static string UserActivityStatManage_PrevUsersStat(Int64 idPath, Int64 idActivity, int idCommunity, int idUser, Int16 pageIndex, DateTime dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                String url = String.Format(_UserActivityStatistics, idPath.ToString(), idActivity.ToString(), idCommunity.ToString(), (int)EpViewModeType.Manage, showAllStatistics.ToString().ToLower()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                url += "&UsId=" + idUser + "&PrevS=" + (Int16)EpStatPage.UsersStat + "&Pidx=" + pageIndex;
                return TimeView(url, dateToView);
            }
            public static string UserActivityStatManage_PrevUsersStat(Int64 idPath, Int64 idActivity, int idCommunity, int idUser, DateTime dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                String url = String.Format(_UserActivityStatistics, idPath.ToString(), idActivity.ToString(), idCommunity.ToString(), (int)EpViewModeType.Manage, showAllStatistics.ToString().ToLower()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                url += "&UsId=" + idUser + "&PrevS=" + (Int16)EpStatPage.UserStat;
                return TimeView(url, dateToView);
            }
            public static string UserActivityStatManage(Int64 idPath, Int64 idActivity, int idCommunity, int idUser, EpStatPage previusPage, DateTime dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                String url = String.Format(_UserActivityStatistics, idPath.ToString(), idActivity.ToString(), idCommunity.ToString(), (int)EpViewModeType.Manage, showAllStatistics.ToString().ToLower()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                url += "&UsId=" + idUser + "&PrevS=" + (Int16)previusPage;
                return TimeView(url, dateToView);
            }
            public static string UserActivityStatView(Int64 idPath, Int64 idActivity, int idCommunity, DateTime dateToView, Boolean showAllStatistics, Boolean isMooc)
            {
                String url = String.Format(_UserActivityStatistics, idPath.ToString(), idActivity.ToString(), idCommunity.ToString(), (int)EpViewModeType.View, showAllStatistics.ToString().ToLower()) + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
                return TimeView(url, dateToView);
            }

            private static string TimeView(String url,DateTime dateToView)
            {
                if (dateToView != null)
                    return url + "&ST=" + ((DateTime)dateToView).Ticks;
                else
                    return url;
            }
        #endregion

        #region "Path Evaluation"
            public static string EvaluateGlobal(Int64 pathId, int CommunityID, DateTime? certifiedDate, Boolean isMooc)
            {
                if (certifiedDate == null)
                {
                    certifiedDate = DateTime.Now;
                }
                return "Modules/Moocs/GlobalStat.aspx?PId=" + pathId + "&Eval=1&ComId=" + CommunityID + "&ST=" + ((DateTime)certifiedDate).Ticks.ToString() + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }

            public static string Validate(Int64 PathId, int CommunityId, ItemType type, Boolean isMooc, Boolean isFromReadOnly)
            {
                return "Modules/Moocs/Validate.aspx?ComId=" + CommunityId + "&PId=" + PathId + "&It=" + (int)type + "&isMooc=" + isMooc.ToString() + "&freadonly=" + isFromReadOnly.ToString();
            }
            public static string EvaluateSubAct(Int64 ItemId, int CommunityID, EvaluationFilter filter, Int16 pageIndex, DateTime? certifiedDate, Boolean isMooc)
            {
                if (certifiedDate == null)
                {
                    certifiedDate = DateTime.Now;
                }
                return "Modules/Moocs/UsersStat.aspx?ItemId=" + ItemId + "&ComId=" + CommunityID + "&It=" + (int)ItemType.SubActivity + "&Page=" + pageIndex + "&Eval=1&Filter=" + (int)filter + "&ST=" + ((DateTime)certifiedDate).ToString() + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }

            public static string EvaluateSubAct(Int64 ItemId, int CommunityID, EvaluationFilter filter, DateTime? certifiedDate, Boolean isMooc)
            {
                if (certifiedDate == null)
                {
                    certifiedDate = DateTime.Now;
                }
                return "Modules/Moocs/UsersStat.aspx?ItemId=" + ItemId + "&ComId=" + CommunityID + "&It=" + (int)ItemType.SubActivity + "&Page={0}&Eval=1&Filter=" + (int)filter + "&ST=" + ((DateTime)certifiedDate).Ticks.ToString() + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }
        #endregion

        #region Path navigation
            public static string EduPathList(int CurrentCommunityID, EpViewModeType ViewMode, Boolean isMooc)
            {
                return "Modules/Moocs/Moocs.aspx?ComId=" + CurrentCommunityID + "&ViewMode=" + (int)ViewMode;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="CurrentCommunityID"></param>
            /// <param name="ViewMode"></param>
            /// <param name="disableRedirect">when true, you can see this page if there's one EP only too</param>
            /// <returns></returns>
            public static string EduPathList(int CurrentCommunityID, EpViewModeType ViewMode, Boolean isMooc, Boolean disableRedirect)
            {
                if (disableRedirect)
                    return "Modules/Moocs/Moocs.aspx?ComId=" + CurrentCommunityID + "&ViewMode=" + (int)ViewMode + "&Red=-1" ;
                else
                    return "Modules/Moocs/Moocs.aspx?ComId=" + CurrentCommunityID + "&ViewMode=" + (int)ViewMode ;
            }

            public static string PathView(Int64 idPath, int idCommunity, EpViewModeType ViewMode, Boolean isPlayMode, Boolean isMooc, Boolean isReadOnly = false)
            {
                if (isPlayMode && ViewMode == EpViewModeType.View)
                    return ViewFullPlay(idPath, idCommunity, isMooc); //(fullPlayView) ? ViewFullPlay(idPath, idCommunity) : ViewPlay(idPath, idCommunity);
                else
                    return EduPathView(idPath, idCommunity, ViewMode, isMooc, isReadOnly);
            }
            public static string EduPathView(Int64 idPath, int idCommunity, EpViewModeType ViewMode, Boolean isMooc, Boolean isReadOnly = false)
            {
                return "Modules/Moocs/MoocView.aspx?ComId=" + idCommunity.ToString() + "&PId=" + idPath.ToString() + "&ViewMode=" + (int)ViewMode + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + (isReadOnly ? "&readonly=" + isReadOnly.ToString() : "");
            }
            public static string ViewFullPlay(Int64 idPath, int idCommunity, Boolean isMooc)
            {
                return "Modules/Moocs/MoocPlay.aspx?ComId=" + idCommunity.ToString() + "&PId=" + idPath.ToString() + (isMooc ? "&isMooc=" + isMooc.ToString() : ""); ;
            }
        #endregion

        #region "Path Settings editing"
            public static string PathManagement(Int64 CurrentCommunityID, Int64 pathId, string StepPathManagement, EPType EduPathType, Boolean isMooc)
            {
                return "Modules/Moocs/MoocManagement.aspx?ComId=" + CurrentCommunityID + "&PId=" + pathId + "&Step=" + StepPathManagement + "&Type=" + (int)EduPathType + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }
            public static string PathManagementSelectCommunity()
            {
                return "Modules/Moocs/MoocManagement.aspx?Step=4";
            }
            public static string SubActivityManagement()
            {
                return "";
            }

            public static string ActivityManagement(Int64 ActivityId, Int64 UnitId, int CommunityID, string StepActivityManagement, Boolean isMooc, Boolean isFromReadOnly)
            {
                return "Modules/Moocs/ActivityManagement.aspx?AId=" + ActivityId + "&Step=" + StepActivityManagement + "&UId=" + UnitId + "&ComId=" + CommunityID + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
            }

            public static string UnitManagement(Int64 UnitId, Int64 PathId, int CommunityID, string StepUnitManagement, Boolean isMooc, Boolean isFromReadOnly)
            {
                return "Modules/Moocs/UnitManagement.aspx?PId=" + PathId + "&Step=" + StepUnitManagement + "&UId=" + UnitId + "&ComId=" + CommunityID + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
            }
            public static string ViewActivity(Int64 ActivityId, Int64 UnitId, Int64 PathId, int CommunityId, EpViewModeType ViewMode, Boolean isMooc, Boolean isFromReadOnly)
            {
                return "Modules/Moocs/ViewActivity.aspx?AId=" + ActivityId + "&UId=" + UnitId + "&PId=" + PathId + "&ComId=" + CommunityId + "&ViewMode=" + (int)ViewMode + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
            }
            public static string ViewActivity(Int64 idSubactivity, Int64 idActivity, Int64 idUnit, Int64 idPath, int idCommunity, EpViewModeType ViewMode, Boolean isMooc, Boolean isFromReadOnly)
            {
                return ViewActivity(idActivity, idUnit, idPath, idCommunity, ViewMode, isMooc, isFromReadOnly) + ((idSubactivity == 0) ? "" : "#subact-" + idSubactivity.ToString()) ;
            }
        #endregion

        #region "Certifications"
            public static String EPCertificationList(Int32 idCommunity, Int64 idPath, Boolean isMooc)
            {
                return String.Format("Modules/Moocs/EpCertificationList.aspx?idcommunity={0}&idpath={1}&isMooc={2}", idCommunity, idPath,isMooc.ToString());
            }

            public static String EPCertification(Int32 idCommunity, Int64 idPath, Int64 idCertificate, Boolean isMooc)
            {
                return String.Format("Modules/Moocs/EpCertification.aspx?idcommunity={0}&idpath={1}&idcertificate={2}&isMooc={3}", idCommunity, idPath, idCertificate, isMooc.ToString());
            }

            public static String EPCertificationUser(Int32 idCommunity, Int64 idPath, Int32 IdUser, Boolean isMooc)
            {
                return String.Format("Modules/Moocs/EpUserCertification.aspx?idcommunity={0}&idpath={1}&iduser={2}&isMooc={3}", idCommunity, idPath, IdUser, isMooc.ToString());
            }

            public static String EPCertificationUser(Int32 idCommunity, Int64 idPath, Int32 IdUser, Int64 IdCertificate, Boolean isMooc)
            {
                return String.Format("Modules/Moocs/EpUserCertification.aspx?idcommunity={0}&idpath={1}&iduser={2}&idcertificate={3}&isMooc={4}", idCommunity, idPath, IdUser, IdCertificate, isMooc.ToString());
            }

            private static String GetTimeMacItems(String mac, DateTime time, TimeSpan timeValidity)
            {
                return "st=" + time.Ticks.ToString()
                    + "&tv=" + timeValidity.Ticks.ToString()
                    + "&mac=" + System.Web.HttpUtility.UrlEncode(mac);
            }
            private static String GetTimeMacItems(String mac, long time, long timeValidity)
            {
                return "st=" + time.ToString()
                    + "&tv=" + timeValidity.ToString()
                    + "&mac=" + System.Web.HttpUtility.UrlEncode(mac);
            }
        

        #endregion

         #region EditPath
            //public static string SelectNewPath(Int64 CurrentCommunityID)
            //{
            //    return "Modules/Moocs/EduPathSelect.aspx?ComId=" + CurrentCommunityID;
            //}
            /// <summary>
            /// URL to create a new Path, after EPType selection has done.
            /// </summary>
            /// <param name="CurrentCommunityID"></param>
            /// <param name="EduPathType"></param>
            /// <returns></returns>
            public static string AddPath(Int64 idCommunity, EPType EduPathType, Boolean isMooc)
            {
                return "Modules/Moocs/MoocManagement.aspx?ComId=" + idCommunity + "&Step=-1&Type=" + (int)EduPathType + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }
            public static string UpdateUnit(int idCommunity, Int64 UnitId, Int64 PathId, Boolean isMooc, Boolean isFromReadOnly)
            {
                return "Modules/Moocs/UnitManagement.aspx?ComId=" + idCommunity + "&PId=" + PathId + "&UId=" + UnitId + "&Step=-2" + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();

            }
            public static string NewUnit(int idCommunity, Int64 PathId, Boolean isMooc)
            {
                return "Modules/Moocs/UnitManagement.aspx?ComId=" + idCommunity + "&PId=" + PathId + "&Step=-1" + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }
            public static string UpdateActivity(int idCommunity, Int64 ActivityId, Int64 UnitId, Boolean isMooc, Boolean isFromReadOnly)
            {
                return "Modules/Moocs/ActivityManagement.aspx?ComId=" + idCommunity + "&AId=" + ActivityId + "&UId=" + UnitId + "&Step=-2" + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();

            }
            public static string NewActivity(int CurrentCommunityID, Int64 UnitId, Boolean isMooc)
            {
                return "Modules/Moocs/ActivityManagement.aspx?ComId=" + CurrentCommunityID + "&UId=" + UnitId + "&Step=-1" + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }
            /// <summary>
            /// URL to EduPathSelect
            /// </summary>
            /// <param name="CommunityId">CurrentCommunityId</param>
            /// <returns></returns>
            public static string NewPath(int CommunityId, Boolean isMooc)
            {
                return "Modules/Moocs/MoocSelect.aspx?ComId=" + CommunityId + (isMooc ? "&isMooc=" + isMooc.ToString() : "");
            }
        #endregion

            #region TextAction - ToReDo
                public static string DisplayTextActionControl()
                {
                    return "Modules/Moocs/UC/UC_TextAction.ascx";
                }

                public static string AddTextItem(Int64 parentId, ItemType type, int CommunityID, Boolean isMooc, Boolean isFromReadOnly)
                {
                    return "Modules/Moocs/TextItem.aspx?ItemId=0&ComId=" + CommunityID + "&ParentId=" + parentId + "&It=" + (int)type + "&Step=0" + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
                }

                public static string UpdateTextItem(Int64 itemId, Int64 parentId, ItemType type, int CommunityID, Boolean isMooc, Boolean isFromReadOnly)
                {
                    return "Modules/Moocs/TextItem.aspx?ItemId=" + itemId + "&ComId=" + CommunityID + "&ParentId=" + parentId + "&It=" + (int)type + "&Step=3" + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
                }

                public static string TextItem(Int64 itemId, Int64 parentId, ItemType type, int CommunityID, string step, Boolean isMooc, Boolean isFromReadOnly)
                {
                    return "Modules/Moocs/TextItem.aspx?ItemId=" + itemId + "&ComId=" + CommunityID + "&ParentId=" + parentId + "&It=" + (int)type + "&Step=" + step + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
                }

                public static string AddSubActText(Int64 ActivityId, int CommunityID, Boolean isMooc, Boolean isFromReadOnly)
                {
                    return "Modules/Moocs/SubActText.aspx?AId=" + ActivityId + "&ComId=" + CommunityID + "&SId=0" + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
                }

                public static string UpdateSubActText(Int64 SubActId, Int64 ActivityId, int CommunityID, Boolean isMooc, Boolean isFromReadOnly)
                {
                    return "Modules/Moocs/SubActText.aspx?AId=" + ActivityId + "&ComId=" + CommunityID + "&SId=" + SubActId + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
                }
                public static string UpdateSubActCertification(Int64 idPath, Int64 idUnit, Int64 idActivity, Int64 idSubActivity, int idCommunity, Boolean isMooc, Boolean isFromReadOnly)
                {
                    return "Modules/Moocs/CertificationItem.aspx?PId=" + idPath.ToString() + "&UId=" + idUnit.ToString() + "&AId=" + idActivity.ToString() + "&SId=" + idSubActivity + "&ComId=" + idCommunity.ToString() + (isMooc ? "&isMooc=" + isMooc.ToString() : "") + "&freadonly=" + isFromReadOnly.ToString();
                }
            #endregion

            #region Quiz Action
                public static string CreateQuiz(Int64 idOwner, Int64 owType)
                {
                    return "Questionari/QuestionarioAdmin.aspx?type=0&owType=" + owType + "&owId=" + idOwner;
                }

                public static string UpdateQuiz(Int64 idActivity, Int64 idQuiz)
                {
                    return "Questionari/QuestionarioAdmin.aspx?type=0&owType=3&owId=" + idActivity + "&idQ=" + idQuiz;
                }
            #endregion

            //#region "Render Action Controls"
            //    public static string RenderTextAction()
            //    {
            //        return "Modules/Moocs/UC/UC_TextAction.ascx";
            //    }
            //    public static string RenderCertificationAction()
            //    {
            //        return "Modules/Moocs/UC/UC_CertificationAction.ascx";
            //    }
            //#endregion
        #endregion

        #region Images 
            public static string ImgContentType(string BaseUrl, SubActivityType ContentType)
            {
                switch (ContentType)
                {
                    case SubActivityType.Quiz:
                        return ImgQuiz(BaseUrl);

                    case SubActivityType.Forum:
                        return ImgForum(BaseUrl);

                    case SubActivityType.File:
                        return ImgFile(BaseUrl);

                    case SubActivityType.Wiki:
                        return ImgWiki(BaseUrl);

                    case SubActivityType.Text:
                        return ImgText(BaseUrl);

                    case SubActivityType.Certificate:
                        return imgCertificate(BaseUrl);

                    default:
                        return "";
                }
            }

     
            public static string ImgTextNoteItem(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/text_note_20x20.png";
            }

            public static string ImgStatusGreySmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Grey_light_Small.png";
            }

            public static string ImgStatusGreyMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Grey_light.png";
            }

            public static string ImgStatusGreenSmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Green_light_Small.png";
            }

            public static string ImgStatusGreenMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Green_light.png";
            }

            public static string ImgStatusRedSmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Red_light_Small.png";
            }

            public static string ImgStatusRedMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Red_light.png";
            }

            public static string ImgStatusYellowSmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Yellow_light_Small.png";
            }

            public static string ImgStatusYellowMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Yellow_light.png";
            }

            public static string ImgBlindSmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/blind_16x16.png";
            }

            public static string ImgBlindMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/blind_20x20.png";
            }

            public static string ImgText(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/SubActText.png";
            }

            public static string ImgMandatorySubItem(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/mandatory_stats_24x24.png";
            }

            public static string ImgRectangleGreen(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Green.png";
            }

            public static string ImgRectangleYellow(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Yellow.png";
            }

            public static string ImgRectangleRed(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/Red.png";
            }

            public static string ImgGreen(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/verde.gif";
            }

            public static string ImgRed(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/rosso.gif";
            }

            public static string ImgQuiz(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/quiz.png";
            }
            public static string ImgFile(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/file.png";
            }
            public static string ImgForum(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/forum.png";
            }
            public static string ImgWiki(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/wiki.png";
            }

            public static string imgCertificate(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/certificate.png";
            }

            public static string ImgUpActive(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/up_active.gif";
            }

            public static string ImgUpPassive(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/up_passive.gif";
            }

            public static string ImgDownActive(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/down_active.gif";
            }

            public static string ImgDownPassive(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/down_passive.gif";
            }

            public static string ImgStatus(string BaseUrl, StatusStatistic StatusStat)
            {
                switch (StatusStat)
                {
                    case StatusStatistic.Passed:
                        return BaseUrl + "Modules/Moocs/img/"; //tondo o batteria grigia 
                    case StatusStatistic.Completed:
                        return BaseUrl + "Modules/Moocs/img/"; //tondo o batteria gialla piena
                    case StatusStatistic.Started:
                        return BaseUrl + "Modules/Moocs/img/"; //tondo o batteria gialla mezza piena 
                    case StatusStatistic.Browsed:
                        return BaseUrl + "Modules/Moocs/img/"; //tondo o batteria grigia 
                    case StatusStatistic.None:
                        return BaseUrl + "Modules/Moocs/img/"; //tondo o batteria grigia        
                    default:
                        return "";
                }
            }


            public static string ImgStatusCompleted(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/delete.gif";
            }

            public static string ImgStatusStarted(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/delete.gif";
            }
            public static string ImgStatusNotStarted(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/delete.gif";
            }
            public static string ImgStatusPassed(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/delete.gif";
            }



            public static string ImgEvaluateTrue(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/delete.gif";
            }
            public static string ImgEvaluateFalse(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/delete.gif";
            }



            public static string ImgMandatoryMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/mandatory.png";
            }

            public static string ImgMandatorySmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/mandatory_small.png";
            }

            public static string ImgSetMandatoryMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/no-mandatory.png";
            }

            public static string ImgSetMandatorySmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/no-mandatory_small.png";
            }

            public static string ImgRemoveMandatoryMedium(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/mandatory.png";
            }

            public static string ImgRemoveMandatorySmall(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/mandatory_small.png";
            }

            public static string ImgDefault(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/tag-green.png";
            }

            public static string ImgNotDefault(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/";
            }

            public static string ImgSetDefault(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/tag-green_add.png";
            }

            public static string ImgEvaluate(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/evaluate.gif";
            }

            public static string ImgTextNote(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/text-note.png";
            }

            public static string ImgBtnBlocked(string BaseUrl, bool isVisible)
            {
                if (isVisible)
                {
                    return ImgBtnBlocked_On(BaseUrl);
                }
                else
                {
                    return ImgBtnBlocked_Off(BaseUrl);
                }
            }
            public static string ImgBtnBlocked_On(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/unlocked16.png";
            }
            public static string ImgBtnBlocked_Off(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/locked16.png";
            }
            public static string ImgItemBlocked(string BaseUrl, bool isVisible)
            {
                if (isVisible)
                {
                    return ImgItemBlocked_On(BaseUrl);
                }
                else
                {
                    return ImgItemBlocked_Off(BaseUrl);
                }
            }
            public static string ImgItemBlocked_On(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/unlocked16.png";
            }
            public static string ImgItemBlocked_Off(string BaseUrl)
            {
                return BaseUrl +  "Modules/Moocs/img/locked16.png";
            }
            public static string ImgUsersStat(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/icona_users_sm.jpg";
            }
            public static string ImgEduPathStat(string BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/ep_stat.png";
            }

            public static string ImgAddRule(String BaseUrl)
            {
                return BaseUrl + "Modules/Moocs/img/add-criteria.png";
            }
        #endregion
    }
}