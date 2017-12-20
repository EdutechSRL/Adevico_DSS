using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO
{
    [Serializable, CLSCompliant(true)]
    public class DTO_NotificationData
    {
        public Notification.DTO_User User { get; set; }

        public Notification.DTO_Ticket Ticket { get; set; }
        public Notification.DTO_Category Category { get; set; }

        public Notification.DTO_Answer Answer { get; set; }
        public Notification.DTO_Action Action { get; set; }

        /// <summary>
        /// Analizza il contenuto di content e converte i Tag in Valori
        /// </summary>
        /// <param name="content">Testo da analizzare</param>
        /// <param name="LangCode">Codice lingua</param>
        /// <param name="DateTimeFormat">Formato data/ora</param>
        /// <param name="BaseUrl">BaseUrl dell'applicazione</param>
        /// <param name="IntTicketStatus"></param>
        /// <param name="IntCategoryType"></param>
        /// <param name="CategoriesTemplate"></param>
        /// <returns></returns>
        public String AnalyzeContent(
            String content,
            DTO_NotificationSettings Settings
            )
        {
            if (User != null)
                content = User.AnalyzeContent(content, Settings.DateTimeFormat, Settings.BaseUrl);
            else
                content = Notification.DTO_User.RemoveTags(content);

            if (Ticket != null)
                content = Ticket.AnalyzeContent(content, Settings.DateTimeFormat, Settings.AvailableTicketStatus, Settings.LangCode, Settings.BaseUrl);
            else
                content = Notification.DTO_Ticket.RemoveTags(content);

            if (Category != null)
                content = Category.AnalyzeContent(content, Settings.AvailableCategoryTypes, Settings.CategoriesTemplate);
            else
                content = Notification.DTO_Category.RemoveTags(content);

            if (Answer != null)
                content = Answer.AnalyzeContent(content);
            else
                content = Notification.DTO_Answer.RemoveTags(content);

            if (Action != null)
                content = Action.AnalyzeContent(content);
            else
                content = Notification.DTO_Action.RemoveTags(content);

            return content;
        }

        /// <summary>
        /// Rimuove tutti i TAG da content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static String RemoveTags(String content)
        {
            content = Notification.DTO_User.RemoveTags(content);
            content = Notification.DTO_Ticket.RemoveTags(content);
            content = Notification.DTO_Category.RemoveTags(content);
            content = Notification.DTO_Answer.RemoveTags(content);
            return content;
        }
    }
}

namespace lm.Comol.Core.BaseModules.Tickets.Domain.DTO.Notification
{
    [Serializable, CLSCompliant(true)]
    public class DTO_Action
    {
        public String UserDisplayName { get; set; }
        public String UserRole { get; set; }

        /// <summary>
        /// Analizza contente
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public String AnalyzeContent(String content)
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ActionDisplayName), UserDisplayName);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ActionRole), UserRole);

            return content;
        }

        /// <summary>
        /// Rimuove tutti i TAG da content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static String RemoveTags(String content)
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ActionDisplayName), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ActionRole), "");

            return content;
        }
    }

    [Serializable, CLSCompliant(true)]
    public class DTO_User
    {
        public String Mail { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Password { get; set; }
        public String LanguageCode { get; set; }
        public DTO_Token Token { get; set; }

        /// <summary>
        /// Sostituisce tutti i Tag di Content con i relativi valori
        /// </summary>
        /// <param name="content">Testo da analizzare</param>
        /// <param name="DateTimeFormat"></param>
        /// <param name="BaseUrl"></param>
        /// <returns></returns>
        public String AnalyzeContent(String content, String DateTimeFormat, String BaseUrl)
        {

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserLanguageCode), LanguageCode);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserMail), Mail);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserName), Name);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserSurname), Surname);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserPassword), Password); //DecryptPwd(User.Code));
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ExternalAccessUrl), BaseUrl + RootObject.ExternalLogin());
            

            if (Token != null)
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserToken), Token.Code);
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserTokenExpiration), Token.Creation.Add(TicketService.TokenLifeTime).ToString(DateTimeFormat));

                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserTokenUrl),
                    BaseUrl + RootObject.ExternalLogin(Token.Code.ToString()));
            }
            else
            {
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserToken), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserTokenExpiration), "");
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserTokenUrl),
                    BaseUrl + RootObject.ExternalLogin());
            }

            return content;
        }

        [Serializable, CLSCompliant(true)]
        public class DTO_Token
        {
            public String Code { get; set; }
            public DateTime Creation { get; set; }
        }

        /// <summary>
        /// Rimuove tutti i TAG da content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static String RemoveTags(String content)
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserLanguageCode), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserMail), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserName), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserSurname), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserPassword), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.ExternalAccessUrl), "");

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserToken), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserTokenExpiration), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.UserTokenUrl), "");
            return content;
        }
    }

    [Serializable, CLSCompliant(true)]
    public class DTO_Answer
    {
        /// <summary>
        /// Data invio EFFETTIVO!
        /// </summary>
        /// <remarks>
        /// Serve "SOLO" per il primo messaggio, nel momento in cui il Ticket è creato "in bozza".
        /// Altrimenti corrisponde solo alla data di creazione!
        /// </remarks>
        public DateTime SendDate { get; set; }
        /// <summary>
        /// Tipo messaggio - richiesta, risposta, systema, ...
        /// </summary>
        public virtual Enums.MessageType Type { get; set; }
        public String ShortText { get; set; }
        public String FullText { get; set; }

        /// <summary>
        /// Analizza contente
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public String AnalyzeContent(String content)
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.AnswerShortText), ShortText);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.AnswerFullText), FullText);

            return content;
        }

        /// <summary>
        /// Rimuove tutti i TAG da content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static String RemoveTags(String content)
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.AnswerShortText), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.AnswerFullText), "");

            return content;
        }
    }

    [Serializable, CLSCompliant(true)]
    public class DTO_Ticket
    {
        public Domain.Enums.TicketStatus Status { get; set; }
        /// <summary>
        /// Chiave: Codice Lingua
        /// Valore: nome localizzato
        /// </summary>
        public Dictionary<string, string> CategoryCurrent { get; set; }
        /// <summary>
        /// Chiave: Codice Lingua
        /// Valore: nome localizzato
        /// </summary>
        public Dictionary<string, string> CategoryInitial { get; set; }

        public String Assigner { get; set; }
        public String Title { get; set; }
        public String Preview { get; set; }
        public String LongText { get; set; }
        public DateTime SendDate { get; set; }
        public String Language { get; set; }
        public String LanguageCode { get; set; }
        public String CreatorDisplayName { get; set; }
        public String Code { get; set; }
        public Int64 Id { get; set; }

        /// <summary>
        ///  Converte i tag di content in valori
        /// </summary>
        /// <param name="content"></param>
        /// <param name="DateTimeFormat">Formato Data/Ora</param>
        /// <param name="IntTicketStatus">Dictionary di TicketStatus e relativo valore internazionalizzato</param>
        /// <param name="LangCode">Codice lingua</param>
        /// <returns></returns>
        public String AnalyzeContent(String content, String DateTimeFormat, IDictionary<Domain.Enums.TicketStatus, String> IntTicketStatus, String LangCode, String BaseUrl)
        {
            if (!BaseUrl.EndsWith("/"))
                BaseUrl = BaseUrl + "/";

            if (IntTicketStatus != null || IntTicketStatus.ContainsKey(Status))
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketStatus), IntTicketStatus[Status]);
            else
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketStatus), Status.ToString());

            String Category = "";
            if(CategoryCurrent != null)
            {
                //Se c'è la lingua corrente, uso quella
                if (CategoryCurrent.ContainsKey(LangCode))
                    Category = CategoryCurrent[LangCode];

                //Se non c'è uso MULTI
                if (String.IsNullOrEmpty(Category) && CategoryCurrent.ContainsKey(TicketService.LangMultiCODE))
                    Category = CategoryCurrent[TicketService.LangMultiCODE];

                //Altrimenti LASCIO VUOTO e TOLGO IL TAG
            }
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketCategoryCurrent), Category);

            Category = "";
            if (CategoryInitial != null)
            {
                //Se c'è la lingua corrente, uso quella
                if (CategoryInitial.ContainsKey(LangCode))
                    Category = CategoryInitial[LangCode];

                //Se non c'è uso MULTI
                if (String.IsNullOrEmpty(Category) && CategoryInitial.ContainsKey(TicketService.LangMultiCODE))
                    Category = CategoryInitial[TicketService.LangMultiCODE];

                //Altrimenti LASCIO VUOTO e TOLGO IL TAG
            }
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketCategoryInitial), Category);

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketAssigner), Assigner);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketObject), Title);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketPreview), Preview);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketLongText), LongText);

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketLanguage), Language);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketLanguageCode), LanguageCode);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketCreatorDisplayName), CreatorDisplayName);


            //URL TICKET: ToDo!!!
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketUrlUser),
                BaseUrl + RootObject.TicketEditUser(0, Code));
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketUrlManager),
                BaseUrl + RootObject.TicketEditResolver(0, Code));
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketUrlListManager), BaseUrl + RootObject.TicketListResolver(0));


            if (String.IsNullOrEmpty(DateTimeFormat))
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketSendDate), SendDate.ToString());
            else
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketSendDate), DateTimeFormat.ToString());

            return content;
        }

        /// <summary>
        /// Rimuove tutti i TAG ca content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static String RemoveTags(String content)
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketStatus), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketCategoryCurrent), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketCategoryInitial), "");

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketAssigner), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketObject), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketPreview), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketLongText), "");

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketLanguage), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketLanguageCode), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketCreatorDisplayName), "");

            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketSendDate), "");

            //URL TICKET
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketUrlUser), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketUrlManager), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.TicketUrlListManager), "");

            return content;
        }
    }

    /// <summary>
    /// Rappresenta la categoria
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class DTO_Category
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public IList<DTO_CategoryLocalization> Translations { get; set; }
        public Domain.Enums.CategoryType Type { get; set; }
        public IList<String> LanguagesCode { get; set; }
        public String NewAssignerDisplayName { get; set; }

        /// <summary>
        /// Converte i tag di content in valori
        /// </summary>
        /// <param name="content">Testo da convertire</param>
        /// <param name="IntCategoryType">Dictionary di CategoryType e relativo valore internazionalizzato</param>
        /// <param name="CategoriesTemplate">Eventuale template per la lista delle internazionalizzazioni di categorie</param>
        /// <returns></returns>
        public String AnalyzeContent(
            String content,
            IDictionary<Domain.Enums.CategoryType, String> IntCategoryType,
            String CategoriesTemplate = "")
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryName), Name);
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryDescription), Description);

            //String ListText = "";
            //foreach (DTO_CategoryLocalization dCL in Translations)
            //{
            //    ListText += dCL.Serialize(CategoriesTemplate);
            //}
            //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryLONGNameAndDescriptionLIST), ListText);


            if (IntCategoryType != null && IntCategoryType.ContainsKey(Type))
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryType), IntCategoryType[Type]);
            else
                content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryType), Type.ToString());

            //ListText = "";
            //foreach (String value in LanguagesCode)
            //{
            //    ListText += value + ", ";
            //}
            //ListText = ListText.Remove(ListText.LastIndexOf(','));
            //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryLanguagesCodeList), ListText);

            //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryNewAssignerDisplayName), NewAssignerDisplayName);

            return content;
        }

        /// <summary>
        /// Rimuove tutti i TAG ca content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static String RemoveTags(String content)
        {
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryName), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryDescription), "");
            //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryLONGNameAndDescriptionLIST), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryType), "");
            //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryLanguagesCodeList), "");
            //content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryNewAssignerDisplayName), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryLinkListSimple), "");
            content = content.Replace(TemplatePlaceHolders.GetPlaceHolder(PlaceHoldersType.CategoryLinkListFull), "");
            //
            return content;
        }
    }

    /// <summary>
    /// Per elenco nome/descrizione/lingua
    /// </summary>
    [Serializable, CLSCompliant(true)]
    public class DTO_CategoryLocalization
    {
        public String LanguageCode { get; set; }
        public String Language { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }

        /// <summary>
        /// Serializza con "template" standard.
        /// </summary>
        /// <returns>
        /// LANG (Code): Name - Description
        /// Example:
        /// Italiano (It-it): Categoria Uno - Prima categoria creata per test
        /// </returns>
        public String Serialize()
        {
            String serTxt = "";

            serTxt += Language + " (" + LanguageCode + "): ";
            serTxt += Name + " - " + Description;

            return serTxt;
        }

        /// <summary>
        /// Serializza con il template predefinito
        /// </summary>
        /// <param name="Template">
        /// String di template, che potrà contenere:
        /// [LangCode]      Codice lingua
        /// [Language]      Nome lingua
        /// [Name]          Nome categoria nella lingua corrente
        /// [Description]   Descrisione categoria nella lingua corrente
        /// </param>
        /// <returns></returns>
        /// 
        public String Serialize(String Template)
        {
            if (String.IsNullOrEmpty(Template))
                return Serialize();

            return Template
                .Replace("[LangCode]", LanguageCode)
                .Replace("[Language]", Language)
                .Replace("[Name]", Name)
                .Replace("[Description]", Description);

        }
    }

    [Serializable, CLSCompliant(true)]
    public class DTO_UserNotificationData
    {
        public Int32 PersonId { get; set; }
        public Int64 UserId { get; set; }

        /// <summary>
        /// Codice lingua. SE impostato a NULL, viene impostato a "MULTI"
        /// </summary>
        public String LanguageCode
        {
            get
            {
                return _langCode;
            }
            set
            {
                _langCode = !string.IsNullOrEmpty(value) ? value : TicketService.LangMultiCODE;
            }
        }

        /// <summary>
        /// Es: mail address, phone number, etc...
        /// </summary>
        public String ChannelAddress { get; set; }

        /// <summary>
        /// Per ora SOLO MAIL!
        /// </summary>
        public Core.Notification.Domain.NotificationChannel Channel { get; set; }

        public String FullUserName { get; set; }
         
        private string _langCode = TicketService.LangMultiCODE;


            
    }
}