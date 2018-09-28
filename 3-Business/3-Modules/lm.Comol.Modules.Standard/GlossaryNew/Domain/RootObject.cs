using System;
using System.Text;
using System.Web;

namespace lm.Comol.Modules.Standard.GlossaryNew.Domain
{
    public class RootObject
    {
        private const String ModuleHome = "Modules/Glossary/";

        public static String GlossaryQueryString(Int32 idCommunity, Int64 idGlossary = -1, Int64 idTerm = -1, Boolean isFirstOpen = false, Boolean asAnchor = false, Boolean fromListGlossary = false, Boolean manage = false, Boolean fromViewMap = false, GlossaryFilter filter = null, Int32 fromType = 0, Boolean loadFromCookies = false, string idCookies = "")
        {
            var urlForm = new StringBuilder((idCommunity > -1 ? string.Format("idCommunity={0}", idCommunity) : String.Empty));

            if (idGlossary > 0)
                urlForm.Append(string.Format("&idGlossary={0}", idGlossary));

            if (idTerm > 0)
                urlForm.Append(string.Format("&idTerm={0}", idTerm));

            if (!isFirstOpen)
                urlForm.AppendFormat("&isFirstOpen={0}", isFirstOpen);

            if (fromListGlossary)
                urlForm.AppendFormat("&isFromListGlossary={0}", fromListGlossary);

            if (fromViewMap)
                urlForm.AppendFormat("&fromViewMap={0}", fromViewMap);

            if (fromType > 0)
                urlForm.AppendFormat("&fromType={0}", fromType);

            if (loadFromCookies)
                urlForm.AppendFormat("&loadFromCookies={0}", loadFromCookies);

            if (!String.IsNullOrWhiteSpace(idCookies))
                urlForm.AppendFormat("&idCookies={0}", idCookies);

            //0 -> not set
            //1 -> from glossary
            //2 -> from glossary map
            //3 -> from term view


            if (manage)
                urlForm.Append("&manage=True");

            if (idGlossary > 0 && asAnchor)
                urlForm.AppendFormat("#idGlossary{0}", idGlossary);


            if (filter != null)
            {
                if (!String.IsNullOrWhiteSpace(filter.SearchString))
                    urlForm.AppendFormat("&SearchString={0}", HttpUtility.UrlEncode(filter.SearchString));
                if (!String.IsNullOrWhiteSpace(filter.LemmaString))
                    urlForm.AppendFormat("&LemmaString={0}", HttpUtility.UrlEncode(filter.LemmaString));
                if (!String.IsNullOrWhiteSpace(filter.LemmaContentString))
                    urlForm.AppendFormat("&LemmaContentString={0}", HttpUtility.UrlEncode(filter.LemmaContentString));
                urlForm.AppendFormat("&LemmaSearchType={0}", (Int32) filter.LemmaSearchType);
                urlForm.AppendFormat("&LemmaVisibilityType={0}", (Int32) filter.LemmaVisibilityType);
            }

            return urlForm.ToString();
        }

        #region Glossary

        public static String GlossaryList(Int32 idCommunity, Int64 idGlossary = -1, Boolean isFirstOpen = false, Boolean manage = false)
        {
            return string.Format("{0}List.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity, idGlossary, -1, isFirstOpen, false, true, manage));
        }

        public static String GlossarySearch(Int32 idCommunity, GlossaryFilter filter)
        {
            return string.Format("{0}GlossarySearch.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity, -1, -1, false, false, true, false, false, filter));
        }

        public static String GlossaryAdd(Int32 idCommunity)
        {
            return string.Format("{0}Add.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity));
        }

        public static String GlossaryView(long idGlossary, Int32 idCommunity, Boolean isMapView = false, Boolean isManage = false, Boolean loadFromCookies = false, string idCookies = "")
        {
            return string.Format("{0}{1}.aspx?{2}&Page=", ModuleHome, !isMapView ? "View" : "ViewMap", GlossaryQueryString(idCommunity, idGlossary, -1, false, false, false, isManage, false, null, 0, loadFromCookies, idCookies)) + "0";
        }

        public static String GlossaryViewForMap(long idGlossary, Int32 idCommunity, Boolean isMapView = false, Boolean isManage = false, Boolean loadFromCookies = false, string idCookies = "")
        {
            return string.Format("{0}{1}.aspx?{2}&Page=", ModuleHome, !isMapView ? "View" : "ViewMap", GlossaryQueryString(idCommunity, idGlossary, -1, false, false, false, isManage, false, null, 0, loadFromCookies, idCookies)) + "{0}";
        }

        public static String GlossaryViewPage(long idGlossary, Int32 idCommunity, Boolean isMapView = false, Boolean isManage = false, Boolean loadFromCookies = false, string idCookies = "", Int32 page = 0)
        {
            if (page < 0)
                page = 0;
            return string.Format("{0}{1}.aspx?{2}&Page=", ModuleHome, !isMapView ? "View" : "ViewMap", GlossaryQueryString(idCommunity, idGlossary, -1, false, false, false, isManage, false, null, 0, loadFromCookies, idCookies)) + page;
        }

        public static String GlossaryEdit(long idGlossary, Int32 idCommunity, Boolean fromListGlossary, Boolean fromAdd)
        {
            return string.Format("{0}Edit.aspx?{1}{2}", ModuleHome,
                GlossaryQueryString(idCommunity, idGlossary, -1, false, false, fromListGlossary), (fromAdd ? "&fromAdd=true" : ""));
        }

        public static String GlossaryEditShare(long idGlossary, Int32 idCommunity, Boolean fromListGlossary, Boolean fromAdd)
        {
            return string.Format("{0}EditShareSettings.aspx?{1}{2}", ModuleHome, (fromAdd ? "&fromAdd=true" : ""),
                GlossaryQueryString(idCommunity, idGlossary, -1, false, false, fromListGlossary));
        }

        public static string GlossaryImportTerms(int idCommunity, long idGlossary)
        {
            return string.Format("{0}EditImportTerms.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity, idGlossary));
        }

        public static String GlossaryManage(Int32 idCommunity)
        {
            return string.Format("{0}GlossaryListOrder.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity));
        }

        public static object GlossaryImport(int idCommunity)
        {
            return string.Format("{0}Import.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity));
        }

        public static object GlossaryExportToFileCommunity(int idCommunity)
        {
            return string.Format("{0}ExportToFileFromCommunity.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity));
        }

        public static object GlossaryImportFromFileCommunity(int idCommunity)
        {
            return string.Format("{0}ImportFromFileToCommunity.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity));
        }


        public static object GlossaryImportExportXLM(int idCommunity)
        {
            return string.Format("{0}ImportExport.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity));
        }

        #endregion

        #region Term

        public static String TermAdd(long idGlossary, Int32 idCommunity, Boolean fromViewMap)
        {
            return string.Format("{0}AddTerm.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity, idGlossary, -1, false, false, false, false, fromViewMap));
        }

        public static String TermEdit(long idGlossary, Int32 idTerm, Int32 idCommunity, Int32 fromType, Boolean loadFromCookies = false, string idCookies = "", Int32 page = 0)
        {
            return string.Format("{0}EditTerm.aspx?{1}&Page=", ModuleHome, GlossaryQueryString(idCommunity, idGlossary, idTerm, false, false, false, false, false, null, fromType, loadFromCookies, idCookies)) + page;
        }

        public static String TermView(long idGlossary, Int32 idTerm, Int32 idCommunity, Boolean fromViewMap)
        {
            return string.Format("{0}ViewTerm.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity, idGlossary, idTerm, false, false, false, false, fromViewMap));
        }

        #endregion

        #region Recycle Bin

        public static String RecycleBin(Int32 idCommunity)
        {
            return string.Format("{0}RecycleBin.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity));
        }

        public static String RecycleBinTerms(Int32 idCommunity, long idGlossary, Boolean fromViewMap)
        {
            return string.Format("{0}RecycleBinTerms.aspx?{1}", ModuleHome, GlossaryQueryString(idCommunity, idGlossary, -1, false, false, false, false, fromViewMap));
        }

        #endregion
    }
}