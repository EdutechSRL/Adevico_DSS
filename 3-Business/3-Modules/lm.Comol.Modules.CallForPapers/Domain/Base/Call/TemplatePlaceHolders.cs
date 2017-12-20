using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    public static class TemplatePlaceHolders
    {
        private const String tagString = "{0}{1}{2}";
        public static String OpenTag { get { return "["; } }
        public static String CloseTag { get { return "]"; } }

        private static Dictionary<PlaceHoldersType, String> placeHolders = new Dictionary<PlaceHoldersType, String>();
        public static Dictionary<PlaceHoldersType, String> PlaceHolders(Boolean full = false)
        {
            if (placeHolders.Count == 0)
            {
                foreach (PlaceHoldersType p in GetPlaceHoldersType(full))
                {
                    placeHolders.Add(p, String.Format(tagString, OpenTag, p.ToString(), CloseTag));
                }
            }
            return placeHolders;
        }
        public static List<PlaceHoldersType> GetPlaceHoldersType(Boolean full = false)
        {
            return (from t in Enum.GetValues(typeof(PlaceHoldersType)).Cast<PlaceHoldersType>()
                    where t != PlaceHoldersType.None && (full || (!full && t!= PlaceHoldersType.CallInternalUrl && t!= PlaceHoldersType.CallPublicUrl))
                    select t).ToList();
        }
        public static String GetPlaceHolder(PlaceHoldersType type)
        {
            return "[" + type.ToString() +"]";
        }

        public static Boolean HasUserValues(List<String> subjects, List<String> body)
        {
            return HasUserValues(String.Join(" ", subjects), String.Join(" ", body));
        }
        public static Boolean HasUserValues(string subject, String body)
        {
            return HasUserValues(subject) || HasUserValues(body);
        }

        public static Boolean HasUserValues(string content)
        {
            return (!String.IsNullOrEmpty(content) && (content.Contains(GetPlaceHolder( PlaceHoldersType.AllFields)) || content.Contains(GetPlaceHolder( PlaceHoldersType.CallInternalUrl))
                || content.Contains(GetPlaceHolder(PlaceHoldersType.SubmissionDay)) || content.Contains(GetPlaceHolder(PlaceHoldersType.SubmissionTime))
                || content.Contains(GetPlaceHolder(PlaceHoldersType.SubmitterName)) || content.Contains(GetPlaceHolder(PlaceHoldersType.SubmitterSurname))
                || content.Contains(GetPlaceHolder(PlaceHoldersType.SubmissionUrl))
                || content.Contains(GetPlaceHolder(PlaceHoldersType.SubmitterMail)) || content.Contains(GetPlaceHolder(PlaceHoldersType.SubmitterType))));
        }
    }

    [Serializable()]
    public enum PlaceHoldersType
    { 
        None = 0,
        CallName = 1,
        CallEdition = 2,
        SubmissionDay = 3,
        SubmissionTime = 4,
        CallCommunity = 5,
        SubmitterName = 6,
        SubmitterSurname = 7,
        SubmitterMail = 8,
        SubmitterType = 9,
        AllFields = 10,
        CallPublicUrl = 11,
        CallInternalUrl = 12,
        SubmissionUrl = 13
    }
}