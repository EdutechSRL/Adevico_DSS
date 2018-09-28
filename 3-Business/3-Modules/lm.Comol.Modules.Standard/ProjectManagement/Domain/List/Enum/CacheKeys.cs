using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public static class CacheKeys
    {
        public static String All { get { return "ProjectManagement_"; } }
        public static String SummaryUser(Int32 idUser)
        {
            return All + "_" + idUser.ToString();
        }
        public static String SummaryUser(Int32 idUser, long idProject)
        {
             return All + "_" + idUser.ToString() + "_project_" + idProject.ToString();
        }
        public static String SummaryUser(Int32 idUser, ProjectFilterBy by, ItemListStatus status, Int32 idCommunity = -100)
        {
            return All + "_" + idUser.ToString() + "_" + by.ToString() + "_" + status.ToString() + ((idCommunity == -100 || (by!= ProjectFilterBy.AllPersonalFromCurrentCommunity && by != ProjectFilterBy.CurrentCommunity)) ? "" : "_" + idCommunity.ToString());
        }
    }
}