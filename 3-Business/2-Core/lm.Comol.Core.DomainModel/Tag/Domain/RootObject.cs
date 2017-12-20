using System;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Linq;

namespace lm.Comol.Core.Tag.Domain
{
    public class RootObject
    {
        private readonly static String modulehome = "Modules/Tag/";
        public static String List(Boolean fromRecycleBin = false, Boolean fromOrganization = false)
        {
            if (fromRecycleBin)
                return modulehome + "RecycleBin.aspx?recycle=true" + (fromOrganization ? "&forOrganization=" +fromOrganization.ToString():"");
            else
                return modulehome + "List.aspx?recycle=false" + (fromOrganization ? "&forOrganization=" + fromOrganization.ToString() : "");
        }
        public static String BulkTagsAssignment(Int32 idOrganization=0, Boolean fromPortal = true,Boolean fromPage=false, Boolean loadOnlyAssigned = false)
        {
            return modulehome + "BulkTagAssignments.aspx?idOrganization=" + idOrganization.ToString() + (fromPortal ? "&FromPortal=" + fromPortal.ToString() : "") + (fromPage ? "&FromPage=" + fromPage.ToString() : "") + (loadOnlyAssigned ? "&assigned=" + loadOnlyAssigned.ToString() : "");
        }
     }
}