using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/ProjectManagement/";

        #region "Edit"
            public static String AddProject(int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Add.aspx?cId=" + idCommunity.ToString() + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String EditProject(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Edit.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String PhisicalDeleteProject(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Delete.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            /// <summary>
            /// url di accesso all'editing del progetti
            /// </summary>
            /// <param name="idProject"></param>
            /// <param name="idCommunity"></param>
            /// <param name="forPortal"></param>
            /// <param name="isPersonal"></param>
            /// <param name="added">indica se il progetto è appena stato creato</param>
            /// <param name="rActions">numero di attività da creare</param>
            /// <param name="aActions">numero di attività aggiunte</param>
            /// <returns></returns>
            public static String EditProject(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, Boolean added, long rActions, long aActions, PageListType view, Int32 idContainerCommunity)
            {
                return EditProject(idProject, idCommunity, forPortal, isPersonal, view, idContainerCommunity) + "&cId=" + idCommunity.ToString() + "&added=" + added.ToString() + (rActions > 0 ? "&rActions=" + rActions.ToString() + "&aActions=" + aActions.ToString() : "");
            }

            public static String ProjectResources(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Resources.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String ProjectResources(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return ProjectResources(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
            public static String ProjectCalendars(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Calendars.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String ProjectCalendars(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return ProjectCalendars(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
            public static String ProjectAttachments(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Attachments.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String ProjectAttachments(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return ProjectAttachments(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
            public static String MapReorder(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "MapReorder.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String MapReorder(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return MapReorder(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
            public static String MapBulk(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "MapBulk.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String MapBulk(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return MapBulk(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }

        #endregion
        #region "List"
            public static String ProjectListManager(int idCommunity, Boolean forPortal, Boolean isPersonal, Boolean fromCookies = false, long idProject = 0, ItemsGroupBy groupBy = ItemsGroupBy.None, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, SummaryTimeLine timeline = SummaryTimeLine.Week, SummaryDisplay display = SummaryDisplay.All)
            {
                return modulehome + "ListManager.aspx?" + (idCommunity > 0 ? "&cId=" + idCommunity.ToString() : "") + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString() + (fromCookies ? "&fromCookies=true" : "") + GetFilterParameters(groupBy,filterBy,filterStatus,timeline, display)  + (idProject == 0 ? "" : "#" + idProject.ToString());
            }
            public static String ProjectListResource(int idCommunity, Boolean forPortal, Boolean isPersonal, Boolean fromCookies = false, long idProject = 0, ItemsGroupBy groupBy = ItemsGroupBy.None, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, SummaryTimeLine timeline = SummaryTimeLine.Week, SummaryDisplay display = SummaryDisplay.All)
            {
                return modulehome + "ListResource.aspx?" + (idCommunity > 0 ? "&cId=" + idCommunity.ToString() : "") + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString() + (fromCookies ? "&fromCookies=true" : "") + GetFilterParameters(groupBy, filterBy, filterStatus, timeline, display) + (idProject == 0 ? "" : "#" + idProject.ToString());
            }
            public static String ProjectListAdministrator(int idCommunity, Boolean forPortal, Boolean isPersonal, Boolean fromCookies = false, long idProject = 0, ItemsGroupBy groupBy = ItemsGroupBy.None, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, SummaryTimeLine timeline = SummaryTimeLine.Week, SummaryDisplay display = SummaryDisplay.All)
            {
                return modulehome + "ListAdministrator.aspx?" + (idCommunity > 0 ? "&cId=" + idCommunity.ToString() : "") + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString() + (fromCookies ? "&fromCookies=true" : "") + GetFilterParameters(groupBy, filterBy, filterStatus, timeline, display) + (idProject == 0 ? "" : "#" + idProject.ToString());
            }

            public static String DashboardFromCookies(dtoProjectContext context, Int32 idContainerCommunity, PageListType toPage, long idProject)
            {
                string page = GetDashboardPlainPage(toPage);

                return page + "?fromCookies=true" + GetPageParameters(PageListType.None, idContainerCommunity) + ((idProject > 0) ? "#prj" + idProject.ToString() : "");
            }
            public static String Dashboard(dtoProjectContext context, Int32 idContainerCommunity, PageContainerType container, PageListType fromPage, PageListType toPage, SummaryTimeLine timeline, SummaryDisplay display, ProjectFilterBy filterBy, ItemsGroupBy groupBy, ItemListStatus status, UserActivityStatus actStatus = UserActivityStatus.Ignore , SummaryTimeLine actTimeline = SummaryTimeLine.Week )
            {
                return Dashboard(new dtoDisplayTimelineContext(context, idContainerCommunity, container, fromPage, timeline, display, filterBy, groupBy, status), toPage, actStatus, actTimeline);
            }
            public static String Dashboard(dtoProjectContext context, long idProject, Int32 idContainerCommunity, PageContainerType container, PageListType fromPage, PageListType toPage, SummaryTimeLine timeline, SummaryDisplay display, ProjectFilterBy filterBy, ItemsGroupBy groupBy, ItemListStatus status, UserActivityStatus actStatus = UserActivityStatus.Ignore, SummaryTimeLine actTimeline = SummaryTimeLine.Week)
            {
                return Dashboard(new dtoDisplayTimelineContext(context, idContainerCommunity, container, fromPage, timeline, display, filterBy, groupBy, status), idProject, toPage, actStatus, actTimeline);
            }
            public static String Dashboard(dtoDisplayTimelineContext context, PageListType toPage, UserActivityStatus actStatus, SummaryTimeLine actTimeline)
            {
                return Dashboard(context, 0, toPage, actStatus, actTimeline);
            }
            public static String Dashboard(dtoDisplayTimelineContext context, long idProject, PageListType toPage, UserActivityStatus actStatus, SummaryTimeLine actTimeline)
            {
                String page = GetDashboardPage(context.ProjectContext, idProject, toPage);
                page += GetPageParameters(context.FromPage, context.IdContainerCommunity) + GetFilterParameters(context) + GetDashboardActivityParametes(actStatus, actTimeline);
                return page;
            }
            private static String GetFilterParameters(dtoDisplayTimelineContext context)
            {
                switch(context.Display){
                    case SummaryDisplay.All:
                        switch (context.Container) { 
                            case PageContainerType.ProjectsList:
                                return GetFilterParameters(ItemsGroupBy.Plain, ProjectFilterBy.All, ItemListStatus.Active, context.ContainerTimeline, context.Display);
                            case PageContainerType.Dashboard:
                                return GetFilterParameters(context.GroupBy, context.FilterBy, context.Status, context.ContainerTimeline, context.Display);
                        }
                        break;
                    case SummaryDisplay.Filtered:
                        switch (context.Container)
                        {
                            case PageContainerType.ProjectsList:
                                return GetFilterParameters(ItemsGroupBy.Plain, context.FilterBy, ItemListStatus.Active, context.ContainerTimeline, context.Display);
                            case PageContainerType.Dashboard:
                                return GetFilterParameters(context.GroupBy, context.FilterBy, context.Status, context.ContainerTimeline, context.Display);
                        }
                        break;
                    case SummaryDisplay.Project:
                        return GetFilterParameters(context.GroupBy, context.FilterBy, context.Status, context.ContainerTimeline, context.Display);
                }
                return "";
            }
            public static String GetDashboardPlainPage(PageListType toPage) {
                return GetDashboardPlainPage(0, toPage);
            }
            public static String GetDashboardPlainPage(long idProject, PageListType toPage)
            {
                return modulehome + ((idProject == 0) ? (toPage== PageListType.DashboardResource? "ResourceDashboard.aspx" : "ManagerDashboard.aspx") : (toPage== PageListType.ProjectDashboardResource? "ProjectResourceDashboard.aspx" : "ProjectDashboard.aspx"));
            }
            private static String GetDashboardPage(dtoProjectContext context,  long idProject,PageListType toPage){
                String url = GetDashboardPlainPage(idProject, toPage) + GetStartContextParameters(context);
                if (idProject > 0)
                    url = url + "&pId=" + idProject.ToString();// +"&v=" + toPage.ToString();
                return url;
            }
            private static String GetDashboardActivityParametes(UserActivityStatus actStatus, SummaryTimeLine actTimeline){
                return (actStatus ==  UserActivityStatus.Ignore) ? "" : "&aSt=" + actStatus.ToString() + "&aTml=" +actTimeline.ToString();
            }

            public static String ProjectDashboardFromCookies(dtoProjectContext context, Int32 idContainerCommunity, PageListType toPage, long idProject)
            {
                string page = GetDashboardPlainPage(idProject,toPage);

                return page + "?fromCookies=true" + GetPageParameters(PageListType.None, idContainerCommunity);
            }
            public static String ProjectDashboard(Int32 idCommunity, Boolean isPortal, Boolean isPersonal, Int32 idContainerCommunity, long idProject, PageListType fromPageType, PageListType toPage, SummaryTimeLine timeline, UserActivityStatus actStatus = UserActivityStatus.Expired, SummaryTimeLine actTimeline = SummaryTimeLine.Week)
            {
                return ProjectDashboard(new dtoProjectContext() { IdCommunity = idCommunity, isForPortal = isPortal, isPersonal = isPersonal }, idContainerCommunity, idProject, fromPageType, toPage, timeline, actStatus, actTimeline);
            }
            public static String ProjectDashboard(dtoProjectContext context, Int32 idContainerCommunity, long idProject, PageListType fromPageType, PageListType toPage, SummaryTimeLine timeline, UserActivityStatus actStatus = UserActivityStatus.Expired, SummaryTimeLine actTimeline = SummaryTimeLine.Week)
            {
                return GetDashboardPage(context, idProject, toPage) + GetPageParameters(fromPageType, idContainerCommunity) + "&tml=" + timeline.ToString() + GetDashboardActivityParametes(actStatus, actTimeline);
            }
            public static String ProjectDashboard(dtoProjectContext context, Int32 idContainerCommunity, long idProject, PageListType fromPageType, PageListType toPage, ItemsGroupBy groupBy, ItemListStatus filterStatus, SummaryTimeLine timeline, UserActivityStatus actStatus = UserActivityStatus.Expired, SummaryTimeLine actTimeline = SummaryTimeLine.Week)
            {
                return GetDashboardPage(context, idProject, toPage) + GetPageParameters(fromPageType, idContainerCommunity) + GetFilterParameters(groupBy, ProjectFilterBy.All, filterStatus, timeline, SummaryDisplay.Project) + GetDashboardActivityParametes(actStatus, actTimeline);
            }
            public static String GetFilterParameters(ItemsGroupBy groupBy = ItemsGroupBy.None, ProjectFilterBy filterBy = ProjectFilterBy.All, ItemListStatus filterStatus = ItemListStatus.All, SummaryTimeLine timeline = SummaryTimeLine.Week, SummaryDisplay display = SummaryDisplay.All)
            {
                return (groupBy == ItemsGroupBy.None ? "" : "&grp=" + groupBy.ToString() + "&fltBy=" + filterBy.ToString() + "&fltSts=" + filterStatus.ToString() + "&tml=" + timeline.ToString() + "&smd=" + display.ToString());
            }
        #endregion
        #region "Map"
            public static String ProjectMap(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Map.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String ProjectMap(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return ProjectMap(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
            public static String ViewProjectMap(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "ViewMap.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String ViewProjectMap(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return ViewProjectMap(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
            public static String Gantt(long idProject, int idCommunity, PageListType view, Int32 idContainerCommunity)
            {
                return modulehome + "Gantt.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + GetPageParameters(view, idContainerCommunity);
            }
            public static String Gantt(long idProject, int idCommunity, Boolean forPortal, Boolean isPersonal, PageListType view, Int32 idContainerCommunity)
            {
                return Gantt(idProject, idCommunity, view, idContainerCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
            public static String GetGanttXML(long idProject, Boolean withPath, int idCommunity)
            {
                return ((withPath) ? modulehome : "") + "GenerateGanttXml.aspx?pId=" + idProject.ToString() + "&cId=" + idCommunity.ToString() + "&time=" + DateTime.Now.Ticks.ToString();
            }
            public static String GetGanttXML(long idProject, Boolean withPath, int idCommunity, Boolean forPortal, Boolean isPersonal)
            {
                return GetGanttXML(idProject, withPath, idCommunity) + "&isPortal=" + forPortal.ToString() + "&isPersonal=" + isPersonal.ToString();
            }
        #endregion
        private static String GetStartContextParameters(dtoProjectContext context)
        {
            return "?cId=" + context.IdCommunity.ToString() + (context.isForPortal ? "&isPortal=" + context.isForPortal.ToString() : "") + (context.isPersonal ? "&isPersonal=" + context.isPersonal.ToString() : "");
        }
        private static String GetStartContextParameters(int idCommunity, Boolean forPortal, Boolean isPersonal)
        {
            return "?cId=" + idCommunity.ToString() + (forPortal ? "&isPortal=" + forPortal.ToString() : "")+ (isPersonal ? "&isPersonal=" + isPersonal.ToString() :"");
        }
        private static String GetPageParameters(PageListType pageType, Int32 idContainerCommunity)
        {
            return ((pageType == PageListType.None || pageType== PageListType.Ignore) ? "" : "&fromView=" + pageType.ToString()) + "&idCC=" + idContainerCommunity.ToString();
        }
      }
}