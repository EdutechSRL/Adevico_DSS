using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    /// <summary>
    /// Per l'aggiunta di un nuovo utente esterno
    /// </summary>
    public class DTO_ExtUser
    {
        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public DTO_ExtUser()
        {
            Name = "";
            SName = "";
            Mail = "";
            InsertError = ErrorAddExtUser.none;
            Langcode = "";

            Audio = false;
            Video = false;
            Chat = false;
            Admin = false;
        }

        /// <summary>
        /// Converte l'oggetto corrente in un WbUser (utente WebConference)
        /// </summary>
        /// <param name="Enabled"></param>
        /// <returns></returns>
        public Domain.WbUser ToWbUser(Boolean Enabled)
        {
            Domain.WbUser usr = new WbUser();

            //usr.DisplayName = Name + " " + SName;
            usr.Name = Name;
            usr.SName = SName;

            usr.LanguageCode = Langcode;

            usr.Mail = Mail;
            usr.MailChecked = false;

            usr.Audio = Audio;
            usr.Video = Video;
            usr.Chat = Chat;
            usr.IsController = Admin;
            usr.IsHost = Admin;
            usr.Enabled = Enabled;

            return usr;
        }

        /// <summary>
        /// Nome utente
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Cognome utente
        /// </summary>
        public String SName { get; set; }
        /// <summary>
        /// eMail utente
        /// </summary>
        public String Mail { get; set; }

        /// <summary>
        /// Audio abilitato
        /// </summary>
        public Boolean Audio { get; set; }
        /// <summary>
        /// Video abilitato
        /// </summary>
        public Boolean Video { get; set; }
        /// <summary>
        /// Chat abilitata
        /// </summary>
        public Boolean Chat { get; set; }

        /// <summary>
        /// Amministratore stanza
        /// </summary>
        public Boolean Admin { get; set; }

        /// <summary>
        /// Per gestione errori
        /// </summary>
        public ErrorAddExtUser InsertError { get; set; }

        /// <summary>
        /// Codice lingua
        /// </summary>
        public String Langcode { get; set; }

        /// <summary>
        /// Controlla che il formato mail sia valido
        /// </summary>
        /// <returns>
        /// True: fomato mail corretto
        /// False: formato mail non valido
        /// </returns>
        public Boolean MailCheckFormat()
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(Mail);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
