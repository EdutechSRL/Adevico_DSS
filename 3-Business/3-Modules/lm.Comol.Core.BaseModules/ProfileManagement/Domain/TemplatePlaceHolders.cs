using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement
{
    [Serializable()]
    public static class TemplatePlaceHolders
    {
        private const String tagString = "{0}{1}{2}";
        public static String OpenTag { get { return "["; } }
        public static String CloseTag { get { return "]"; } }

        private static Dictionary<PlaceHoldersType, String> placeHolders = new Dictionary<PlaceHoldersType, String>();
        public static Dictionary<PlaceHoldersType, String> PlaceHolders()
        {
            if (placeHolders.Count == 0)
            {
                foreach(PlaceHoldersType p in (from t in Enum.GetValues(typeof(PlaceHoldersType)).Cast<PlaceHoldersType>() where t != PlaceHoldersType.None select t)){
                    placeHolders.Add(p, String.Format(tagString,OpenTag,p.ToString(), CloseTag));
                }
            }
            return placeHolders;
        }
        public static String GetPlaceHolder(PlaceHoldersType type)
        {
            return "[" + type.ToString() +"]";
        }
    }

    [Serializable()]
    public enum PlaceHoldersType
    { 
        None = 0,
        Login = 1,
        Mail = 8,
        Password = 2,
        PasswordExpireOn = 3,
        ExternalId = 4,
        AuthenticationType = 5,
        InternalLoginUrl = 6,
        ExternalLoginUrl = 7
    }
}