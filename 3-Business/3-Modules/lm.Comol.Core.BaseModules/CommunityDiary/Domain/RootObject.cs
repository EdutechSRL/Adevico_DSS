using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Domain
{
    public class RootObject
    {
        public static string CommunityDiary(int idCommunity) 
        {
            return "Modules/CommunityDiary/CommunityDiary.aspx?idCommunity=" + idCommunity.ToString();
        }
        public static string CommunityDiary(int idCommunity, long idItem) 
        {
            return "Modules/CommunityDiary/CommunityDiary.aspx?idCommunity=" + idCommunity.ToString() + "#" + idItem.ToString();
        }

        public static string EditDiaryItem(int idCommunity, long idItem)
        {
            return "Modules/CommunityDiary/DiaryItem.aspx?idItem=" + idItem.ToString() + "&idCommunity=" + idCommunity.ToString();
        }
    }
}