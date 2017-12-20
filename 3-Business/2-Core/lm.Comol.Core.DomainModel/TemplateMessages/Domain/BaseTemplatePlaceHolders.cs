using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.TemplateMessages.Domain
{
	[Serializable()]
    public static class BaseTemplatePlaceHolders<T>
	{
		/// <summary>
		/// Per la generazione del placeholer
		/// </summary>
		/// <example>
		/// {0}{1}{2}   diventa     [ Placeholder ]
		/// </example>
		private const String tagString = "{0}{1}{2}";
		/// <summary>
		/// Tag apertura placeholder
		/// </summary>
		public static String OpenTag { get { return "["; } }
		/// <summary>
		/// Tag chiusura placeholder
		/// </summary>
		public static String CloseTag { get { return "]"; } }

		private static Dictionary<T, String> placeHolders = new Dictionary<T, String>();

		/// <summary>
		/// Recupera un placeholder dato un tipo
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static String GetPlaceHolder(T type)
		{
			return OpenTag + type.ToString() + CloseTag;
		}

		/// <summary>
		/// Dictionary placeholder
		/// </summary>
		/// <returns>Dictionary placeholder con tipo + stringa visualizzata</returns>
		public static Dictionary<T, String> PlaceHolders(List<T> removePlaceHolders = null)
		{
			if (placeHolders.Count == 0)
			{
                foreach (T p in (from t in Enum.GetValues(typeof(T)).Cast<T>() where removePlaceHolders == null || !removePlaceHolders.Contains(t) select t))
				{
					placeHolders.Add(p, String.Format(tagString, OpenTag, p.ToString(), CloseTag));
				}
			}
			return placeHolders;
		}

		public static List<T> GetPlaceHoldersType(Boolean full = false, List<T> removePlaceHolders = null)
		{
			return (from t in Enum.GetValues(typeof(T)).Cast<T>()
                    where removePlaceHolders == null || !removePlaceHolders.Contains(t)
					select t).ToList();
		}
        public static Boolean HasUserValues(List<String> subjects, List<String> body, List<T> userPlaceHolders)
        {
            return HasUserValues(String.Join(" ", subjects), String.Join(" ", body), userPlaceHolders);
        }
        public static Boolean HasUserValues(string subject, String body, List<T> userPlaceHolders)
        {
            return HasUserValues(subject, userPlaceHolders) || HasUserValues(body, userPlaceHolders);
        }

        public static Boolean HasUserValues(string content, List<T> userPlaceHolderss)
        {
            return (!String.IsNullOrEmpty(content) && userPlaceHolderss.Where(i=> content.Contains(GetPlaceHolder(i))).Any());
        }
	}
}