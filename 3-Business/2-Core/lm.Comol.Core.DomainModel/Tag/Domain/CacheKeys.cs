using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    public static class CacheKeys
    {
        public static String AllTags
        {
            get { return "Tags_"; }
        }
        public static String AllUserCommunitiesTags
        {
            get { return "UserCommunitiesTags_"; }
        }
        public static String AllCommunityTags
        {
            get { return "CommunityTags_"; }
        }
        public static String Tags(TagType type)
        {
            return AllTags + type.ToString() + "_";
        }
        public static String UserCommunitiesTags(Int32 idUser,TagType type)
        {
            return AllUserCommunitiesTags + idUser.ToString() + "_" + type.ToString() + "_";
        }
    }
}