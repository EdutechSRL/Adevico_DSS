using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.InLineTags.Domain
{
    public static class CacheKeys
    {
        public static String AllTags
        {
            get { return "InLineTags_"; }
        }
        public static String AllCommunityTags
        {
            get { return AllTags + "Community_"; }
        }
        public static String AllUsersTags
        {
            get { return AllTags + "Users_"; }
        }
        public static String UserTags(Int32 idUser)
        {
           return AllUsersTags +idUser.ToString() + "_";
        }
        public static String UserComunityTags(Int32 idUser,Int32 idCommunity)
        {
           return UserTags(idUser) + idCommunity.ToString() + "_";
        }
        public static String ComunityTags(Int32 idCommunity)
        {
            return AllCommunityTags + idCommunity.ToString() + "_";
        }
    }
}