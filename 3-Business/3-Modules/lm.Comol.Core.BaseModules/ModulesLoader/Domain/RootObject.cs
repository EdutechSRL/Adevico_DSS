using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ModulesLoader
{
    public class RootObject
    {
        //public static String UserSessionExpired()
        //{
        //    return "Modules/Common/UserSessionExpired.aspx";
        //}
        public static String AnonymousAccess()
        {
            return "Modules/Common/AnonymousAccess.aspx";
        }
        public static String AnonymousAccess(Int32 idCommunity)
        {
            return "Modules/Common/AnonymousAccess.aspx?idCommunity=" + idCommunity.ToString();
        }
        public static String CommunityLoader(Int32 idCommunity)
        {
            return "Modules/Common/CommunityLoader.aspx?idCommunity=" + idCommunity.ToString();
        }
        public static String PublicAccess()
        {
            return "Modules/Common/PublicAccess.aspx";
        }
        public static String PublicAccess(Int32 idCommunity)
        {
            return "Modules/Common/PublicAccess.aspx?IdC=" + idCommunity.ToString();
        }
        public static String PublicAccess(Int32 idUser, Int32 idCommunity)
        {
            return "Modules/Common/PublicAccess.aspx?idCommunity=" + idCommunity.ToString() + "&IdU=" + idUser.ToString();
        }
    }
}