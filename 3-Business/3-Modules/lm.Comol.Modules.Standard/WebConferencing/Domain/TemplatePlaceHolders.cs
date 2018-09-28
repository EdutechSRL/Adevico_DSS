using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Placeholder per gestione mail template per inviti a stanze
    /// </summary>
    [Serializable()]
    public static class TemplatePlaceHolders
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

        private static Dictionary<PlaceHoldersType, String> placeHolders = new Dictionary<PlaceHoldersType, String>();

        /// <summary>
        /// Dictionary placeholder
        /// </summary>
        /// <returns>Dictionary placeholder con tipo + stringa visualizzata</returns>
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

        /// <summary>
        /// Recupera un placeholder dato un tipo
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String GetPlaceHolder(PlaceHoldersType type)
        {
            return "[" + type.ToString() +"]";
        }
        public static List<PlaceHoldersType> GetPlaceHoldersType(Boolean full = false)
        {
            return (from t in Enum.GetValues(typeof(PlaceHoldersType)).Cast<PlaceHoldersType>()
                    where t != PlaceHoldersType.None 
                    select t).ToList();
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
            return (!String.IsNullOrEmpty(content) && (content.Contains(GetPlaceHolder(PlaceHoldersType.UserKey)) || content.Contains(GetPlaceHolder(PlaceHoldersType.UserDisplayName))
                || content.Contains(GetPlaceHolder(PlaceHoldersType.UserMail)) || content.Contains(GetPlaceHolder(PlaceHoldersType.UserLanguageCode))));
        }
    }

    /// <summary>
    /// Placeholder WebConferencing
    /// </summary>
    [Serializable()]
    public enum PlaceHoldersType
    {
        /// <summary>
        /// Nessuno/vuoto
        /// </summary>
        None = 0,
        /// <summary>
        /// Nome stanza
        /// </summary>
        RoomName = 1,
        /// <summary>
        /// Descrizione stanza
        /// </summary>
        RoomDescription = 2,
        /// <summary>
        /// Data inizio prevista
        /// </summary>
        RoomStartDate = 3,
        /// <summary>
        /// Data fine prevista
        /// </summary>
        RoomEndDate = 4,

        ///// <summary>
        ///// Durata
        ///// </summary>
        //RoomDuration = 5,
        ///// <summary>
        ///// Tipo stanza
        ///// </summary>
        //RoomType = 6,
        /// <summary>
        /// URL accesso esterno
        /// </summary>
        RoomUrl = 7,
        ///// <summary>
        ///// URL accesso utente (superfluo)
        ///// </summary>
        //UserLink = 8,
        ///// <summary>
        ///// Chiave accesso utente (MAIL!)
        ///// </summary>
        //UserKey = 9,
        /// <summary>
        /// Password utente (WbUser Code)
        /// </summary>
        UserKey = 10,
        /// <summary>
        /// Nome utente interno/esterno
        /// </summary>
        UserDisplayName = 11,
        /// <summary>
        /// Indirizzo mail utente interno/esterno
        /// </summary>
        UserMail = 12,
        /// <summary>
        /// Comunità di appartenenza della stanza
        /// </summary>
        RoomCommunity = 13,
        ///// <summary>
        ///// Nome piattaforma
        ///// </summary>
        IstanceName = 14,

        //AGGIUNTI:

        /// <summary>
        /// Data creazione
        /// </summary>
        RoomCreateDate = 21,
        /// <summary>
        /// Codice stanza
        /// </summary>
        RoomCode = 22,
        /// <summary>
        /// Creatore della stanza
        /// </summary>
        RoomCreatedBy = 23,
        /// <summary>
        /// Indirizzo mail utente interno/esterno
        /// </summary>
        UserLanguageCode = 24,
    }
}