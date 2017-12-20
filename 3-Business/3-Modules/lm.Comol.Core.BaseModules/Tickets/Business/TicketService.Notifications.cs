using System;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;

namespace lm.Comol.Core.BaseModules.Tickets
{
    public partial class TicketService : CoreServices
    {
        
        /// <summary>
        /// Invio notifiche automatiche
        /// </summary>
        /// <param name="NotificationType">Tipo notifica</param>
        /// <param name="OtherParameters">Altri parametri da definire...</param>
        /// <returns></returns>
        /// <remarks>
        /// ATTENZIONE! Sarà necessario creare funzioni "ad hoc" con relativi parametri necessari.
        /// </remarks>
        public bool NotificationSend(
            ModuleTicket.MailSenderActionType ActionType,
            Domain.DTO.DTO_NotificationSettings Settings,
            Domain.DTO.DTO_NotificationData Data,
            String Address)
        {
            //Recupero messaggio
            lm.Comol.Core.Notification.Domain.dtoNotificationMessage msg =
               ServiceTemplate.GetNotificationMessage(
                   Data.User.LanguageCode,
                   ModuleTicket.UniqueCode,
                   (Int64)ModuleTicket.MailSenderActionType.externalRegistration);

            if (msg == null)
            {
                return false;
            }

            // Invio Mail.
            return NotificationSendMail(Settings, Data, msg, Address);
        }

        /// <summary>
        /// Invio Token/URL per la validazione dell'utente
        /// </summary>
        /// <param name="NotificationType">Tipo notifica</param>
        /// <returns></returns>
        public bool NotificationSendToken(TicketUser User, Domain.DTO.DTO_NotificationSettings Settings, Token Token, String Password)
        {
            //Testo esempio:

            /* Gentile [PlaceHoldersType.UserSureName] [PlaceHoldersType.UserName]
             * Il suo account è stato registrato nel sistema.
             * 
             * Dati Account:
             * Mail: [PlaceHoldersType.UserMail]
             * Password: [PlaceHoldersType.UserPassword]
             * Lingua: [PlaceHoldersType.UserLanguageCode]
             * 
             * Per attivarlo è necessario accedere con le proprie credenziali al seguente indirizzo:
             * [PlaceHoldersType.UserTokenUrl]
             * 
             * Oppure dalla pagine di accesso, al primo tentativo verrà richiesto il seguente codice di convalida:
             * [PlaceHoldersType.UserToken]
             * 
             * La conferma dell'account può essere effettuata entro il: [PlaceHoldersType.UserTokenExpiration]
             */


            //To Do!!!
            //Domain.Enums.Notification NotificationType = Domain.Enums.Notification.ExternalRegistration;

            //Creo oggetto SETTINGS (me lo farò passare...)
            //Domain.DTO.DTO_NotificationSettings Settings = new Domain.DTO.DTO_NotificationSettings
            //{
            //    BaseUrl = "http://blablalba",
            //    DateTimeFormat = "0:dd/MM/yy H:mm:ss",
            //    LangCode = User.LanguageCode,
            //    CategoriesTemplate = "",
            //    IntCategoryType = null, IntTicketStatus= null,
            //    SmtpConfig = null
            //}; 

            //Creo oggetto DATA
            Domain.DTO.DTO_NotificationData Data = new Domain.DTO.DTO_NotificationData();
            bool IsPerson = (User.Person == null) ? false : true;
            String Mail = IsPerson ? User.Person.Mail : User.mail;
            Data.User = new Domain.DTO.Notification.DTO_User
            {
                LanguageCode = User.LanguageCode,
                Mail = Mail,
                Name = IsPerson ? User.Person.Name : User.Name,
                Surname = IsPerson ? User.Person.Surname : User.Sname,
                Password = Password,
                Token = new Domain.DTO.Notification.DTO_User.DTO_Token
                {
                    Code = Token.Code.ToString(),
                    Creation = Token.CreatedOn
                }
            };

            //Recupero messaggio
            lm.Comol.Core.Notification.Domain.dtoNotificationMessage msg =
               ServiceTemplate.GetNotificationMessage(
                   Data.User.LanguageCode,
                   ModuleTicket.UniqueCode,
                   (Int64)ModuleTicket.MailSenderActionType.externalRegistration);

            if (msg == null)
            {
                return false;
            }

            // Invio Mail.
            return NotificationSendMail(Settings, Data, msg, Mail);
        }

        /// <summary>
        /// Invio mail con password - ToDo
        /// </summary>
        /// <param name="NotificationType">Tipo notifica</param>
        /// <returns></returns>
        public bool NotificationSendRecover(
            Domain.TicketUser User,
            Domain.DTO.DTO_NotificationSettings Settings,
            String NewPassword)
        {

            //Testo esempio:

            /* Le sue credenziali per l'accesso al servizio ticket sono le seguenti:
             * Mail: [PlaceHoldersType.UserMail]
             * Password: [PlaceHoldersType.UserPassword]
             * Indirizzo per l'accesso: [PlaceHoldersType.UserTokenUrl]
             */

            //Creo oggetto SETTINGS (me lo farò passare...)
            //Settings.LangCode = User.LanguageCode;

            // = new Domain.DTO.DTO_NotificationSettings
            //{
            //    BaseUrl = "http://blablalba",
            //    DateTimeFormat = "0:dd/MM/yy H:mm:ss",
            //    LangCode = User.LanguageCode,
            //    CategoriesTemplate = "",
            //    IntCategoryType = null,
            //    IntTicketStatus = null
            //};

            //Creo oggetto DATA
            Domain.DTO.DTO_NotificationData Data = new Domain.DTO.DTO_NotificationData();
            bool IsPerson = (User.Person == null) ? true : false;
            String Mail = IsPerson ? User.Person.Mail : User.mail;
            Data.User = new Domain.DTO.Notification.DTO_User
            {
                LanguageCode = User.LanguageCode,
                Mail = Mail,
                Name = IsPerson ? User.Person.Name : User.Name,
                Surname = IsPerson ? User.Person.Surname : User.Sname,
                Password = NewPassword,
                Token = null
            };

            //Recupero messaggio
            lm.Comol.Core.Notification.Domain.dtoNotificationMessage msg =
               ServiceTemplate.GetNotificationMessage(
                   Data.User.LanguageCode,
                   ModuleTicket.UniqueCode,
                   (Int64)ModuleTicket.MailSenderActionType.externalRecover);

            if (msg == null)
            {
                return false;
            }

            // Invio Mail.
            return NotificationSendMail(Settings, Data, msg, Mail);
        }

        public bool NotificationSendPasswordChanged(Int64 UserId, Domain.DTO.DTO_NotificationSettings Settings)
        {
            if (UserId < 0)
                return false;

            Domain.TicketUser user = Manager.Get<Domain.TicketUser>(UserId);

            if (user == null || user.Id <= 0)
                return false;

            return NotificationSendPasswordChanged(user, Settings);
        }
        /// <summary>
        /// Invio mail per reset password - ToDo
        /// </summary>
        /// <param name="NotificationType">Tipo notifica</param>
        /// <returns></returns>
        public bool NotificationSendPasswordChanged(Domain.TicketUser User, Domain.DTO.DTO_NotificationSettings Settings)
        {

            ////Creo oggetto SETTINGS (me lo farò passare...)
            //Domain.DTO.DTO_NotificationSettings Settings = new Domain.DTO.DTO_NotificationSettings
            //{
            //    BaseUrl = "http://blablalba",
            //    DateTimeFormat = "0:dd/MM/yy H:mm:ss",
            //    LangCode = User.LanguageCode,
            //    CategoriesTemplate = "",
            //    IntCategoryType = null,
            //    IntTicketStatus = null
            //};

            //Creo oggetto DATA
            Domain.DTO.DTO_NotificationData Data = new Domain.DTO.DTO_NotificationData();
            bool IsPerson = (User.Person == null) ? true : false;
            String Mail = IsPerson ? User.Person.Mail : User.mail;

            Data.User = new Domain.DTO.Notification.DTO_User
            {
                LanguageCode = User.LanguageCode,
                Mail = Mail,
                Name = IsPerson ? User.Person.Name : User.Name,
                Surname = IsPerson ? User.Person.Surname : User.Sname,
                Password = "",
                Token = null
            };

            //Recupero messaggio
            lm.Comol.Core.Notification.Domain.dtoNotificationMessage msg =
               ServiceTemplate.GetNotificationMessage(
                   Data.User.LanguageCode,
                   ModuleTicket.UniqueCode,
                   (Int64)ModuleTicket.MailSenderActionType.externalPasswordChanged);

            if (msg != null)
            {
                return NotificationSendMail(Settings, Data, msg, Mail);
            }


            return true;

            // Invio Mail.

        }


        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation NotificationGetTemplatePreview(
            Boolean isHtml,
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content,
            Domain.DTO.DTO_NotificationSettings settings, Domain.DTO.DTO_NotificationData fullData,
            ModuleTicket.MailSenderActionType Action)
        {

            Domain.DTO.DTO_NotificationData Data = new Domain.DTO.DTO_NotificationData
            {
                Action = null,
                Answer = null,
                Category = null,
                Ticket = null,
                User = null
            };

            //Creato l'oggetto "DATA" secondo i casi:
            switch (Action)
            {
                case ModuleTicket.MailSenderActionType.externalPasswordChanged:
                    Data.User = fullData.User;
                    break;
                case ModuleTicket.MailSenderActionType.externalRecover:
                    Data.User = fullData.User;
                    break;
                case ModuleTicket.MailSenderActionType.externalRegistration:
                    Data.User = fullData.User;
                    break;


                //////////////////////////case ModuleTicket.MailSenderActionType.addAnswer:
                //////////////////////////    Data.Ticket = fullData.Ticket;
                //////////////////////////    Data.Action = fullData.Action;
                //////////////////////////    break;
                //////////////////////////case ModuleTicket.MailSenderActionType.assignmentChange:
                //////////////////////////    Data.Ticket = fullData.Ticket;
                //////////////////////////    Data.Action = fullData.Action;
                //////////////////////////    break;
                //////////////////////////case ModuleTicket.MailSenderActionType.statusChange:
                //////////////////////////    Data.Ticket = fullData.Ticket;
                //////////////////////////    Data.Action = fullData.Action;
                //////////////////////////    break;
                //////////////////////////case ModuleTicket.MailSenderActionType.categoryChange:
                //////////////////////////    Data.Category = fullData.Category;
                //////////////////////////    Data.Action = fullData.Action;
                //////////////////////////    break;
                //////////////////////////case ModuleTicket.MailSenderActionType.categoryAssignedChange:
                //////////////////////////    Data.Category = fullData.Category;
                //////////////////////////    Data.Action = fullData.Action;
                //////////////////////////    break;
            }

            return NotificationAnalyzeContent(
                isHtml,
                content,
                settings,
                Data);
        }

        #region FAKE Notification Object
        //private Domain.DTO.Notification.DTO_Answer NotificationGetFakeAnswer(String SampleTextShort, String SampleTextLong)
        //{
        //    return new Domain.DTO.Notification.DTO_Answer
        //        {
        //            FullText = SampleTextLong,
        //            ShortText = SampleTextShort
        //        };
        //}
        //private Domain.DTO.Notification.DTO_Action NotificationGetFakeAction()
        //{
        //    return new Domain.DTO.Notification.DTO_Action
        //    {
        //        UserDisplayName = "User DisplayName",
        //        UserRole = "User Role"
        //    };
        //}
        //private Domain.DTO.Notification.DTO_User NotificationGetFakeUser(bool HasToken)
        //{
        //    return new Domain.DTO.Notification.DTO_User
        //        {
        //            LanguageCode = "IT-it",
        //            Mail = "TestMail@mail.net",
        //            Name = "User_Name",
        //            Surename = "User_SureName",
        //            Password = "Password",
        //            Token = HasToken ? new Domain.DTO.Notification.DTO_User.DTO_Token
        //            {
        //                Code = new Guid().ToString(),
        //                Creation = DateTime.Now
        //            } : null
        //        };
        //}
        //private Domain.DTO.Notification.DTO_Ticket NotificationGetFaketicket(String SampleTextShort, String SampleTextLong)
        //{
        //    Dictionary<string, string> DictCategory = new Dictionary<string, string>();
        //    DictCategory.Add(LangMultiCODE, "Category Name (multi)");
        //    DictCategory.Add("it-IT", "Nome Categoria");
        //    DictCategory.Add("en-US", "Category Name");

        //    return new Domain.DTO.Notification.DTO_Ticket
        //        {
        //            Assigner = "Assigner Displayname",
        //            CategoryCurrent = DictCategory,
        //            CategoryInitial = DictCategory,
        //            CreatorDisplayName = "Creator DisplayName",
        //            Language = "Italiano",
        //            LanguageCode = "it-IT",
        //            LongText = SampleTextLong,
        //            Preview = SampleTextShort,
        //            SendDate = DateTime.Now,
        //            Status = Domain.Enums.TicketStatus.open,
        //            Title = "Ticket Title"
        //        };
        //}
        //private Domain.DTO.Notification.DTO_Category NotificationGetFakeCategory(String SampleTextShort)
        //{
        //    IList<String> Languages = new List<String>();
        //    Languages.Add(LangMultiCODE);
        //    Languages.Add("Italiano");
        //    Languages.Add("English");

        //    IList<Domain.DTO.Notification.DTO_CategoryLocalization> CatLocals = new List<Domain.DTO.Notification.DTO_CategoryLocalization>();

        //    CatLocals.Add(new Domain.DTO.Notification.DTO_CategoryLocalization
        //    {
        //        Description = SampleTextShort,
        //        Name = "Category Name",
        //        Language = LangMultiCODE,
        //        LanguageCode = LangMultiCODE
        //    });
        //    CatLocals.Add(new Domain.DTO.Notification.DTO_CategoryLocalization
        //    {
        //        Description = SampleTextShort,
        //        Name = "Nome Categoria",
        //        Language = "Italiano",
        //        LanguageCode = "it-IT"
        //    });
        //    CatLocals.Add(new Domain.DTO.Notification.DTO_CategoryLocalization
        //    {
        //        Description = SampleTextShort,
        //        Name = "Category Name",
        //        Language = "Italiano",
        //        LanguageCode = "it-IT"
        //    });

        //    return new Domain.DTO.Notification.DTO_Category
        //        {
        //            Description = SampleTextShort,
        //            LanguagesCodeList = Languages,
        //            Name = "Category Name",
        //            NewAssignerDisplayName = "New Assigner DisplayName",
        //            Type = Domain.Enums.CategoryType.Public,
        //            NameAndDescriptionLIST = CatLocals
        //        };
        //}
        #endregion

        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation NotificationGetTemplatePreview(
            Boolean isHtml,
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content,
            Domain.DTO.DTO_NotificationSettings settings,
            Domain.DTO.DTO_NotificationData data)
        {
            return NotificationAnalyzeContent(
                isHtml,
                content,
                settings,
                data);
        }

        /// <summary>
        /// Analizza e converte i Tag di Content in valori
        /// </summary>
        /// <param name="isHtml">NON UTILIZZATO</param>
        /// <param name="content">Testo da analizzare</param>
        /// <param name="Data">Dati da utilizzare: SE NULL, rimuove TUTTI i tag!</param>
        /// <param name="Settings">Impostazioni</param>
        /// <returns></returns>
        public lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation NotificationAnalyzeContent(
            Boolean isHtml,
            lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation content,
            Domain.DTO.DTO_NotificationSettings Settings,
            Domain.DTO.DTO_NotificationData Data)
        {

            if (Data == null)
            {
                content.Body = Domain.DTO.DTO_NotificationData.RemoveTags(content.Body);
                content.Subject = Domain.DTO.DTO_NotificationData.RemoveTags(content.Subject);
            }
            else
            {
                content.Body = Data.AnalyzeContent(content.Body, Settings);
                content.Subject = Data.AnalyzeContent(content.Subject, Settings);
            }

            return content;
        }

        /// <summary>
        /// INVIA MAIL
        /// Intanto così, poi si aggiungerà quello che serve, ma almeno centralizzo l'invio (almeno per quel che mi serve ora...)
        /// </summary>
        /// <param name="Content">TEsto mail</param>
        /// <param name="Address">Indirizzo</param>
        /// <param name="OtherParameters">Gli altri parametri che serviranno per l'invio</param>
        /// <returns>
        /// True: mail inviata
        /// False: errori invio...
        /// </returns>
        private bool NotificationSendMail(
            Domain.DTO.DTO_NotificationSettings Settings,
            Domain.DTO.DTO_NotificationData Data,
            lm.Comol.Core.Notification.Domain.dtoNotificationMessage Message,
            String Address)
        {
            if (Data == null)
            {
                Message.Translation.Body = Domain.DTO.DTO_NotificationData.RemoveTags(Message.Translation.Body);
                Message.Translation.Subject = Domain.DTO.DTO_NotificationData.RemoveTags(Message.Translation.Subject);
            }
            else
            {
                Message.Translation.Body = Data.AnalyzeContent(Message.Translation.Body, Settings);
                Message.Translation.Subject = Data.AnalyzeContent(Message.Translation.Subject, Settings);
            }

            bool sentMail = ServiceTemplate.SendMail(
                    this.CurrentPerson,
                    Settings.SmtpConfig,
                    Message.MailSettings,
                    Message.Translation.Subject,
                    Message.Translation.Body,
                    Address);

            return sentMail;

        }
        /// <summary>
        /// Ottiene il destinatario di un messaggio inviato dal sistema centrale
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public lm.Comol.Core.Mail.dtoRecipient UserGetRecipient(Int64 idUser)
        {
            Domain.TicketUser user = UserGet(idUser);
            if (user == null || user.Id <= 0)
                return null;

            lm.Comol.Core.Mail.dtoRecipient recipient = new Core.Mail.dtoRecipient();

            if (user.Person != null)
            {
                recipient.DisplayName = user.Person.SurnameAndName;
                recipient.MailAddress = user.Person.Mail;
            }

            if (String.IsNullOrEmpty(recipient.DisplayName))
            {
                recipient.DisplayName = user.Sname + " " + user.Name;
                recipient.MailAddress = user.mail;
            }

            return recipient;
        }

    }
}
