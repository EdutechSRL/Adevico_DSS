using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Charting;

namespace lm.Comol.Core.BaseModules.Tickets.Domain
{
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
		/// Recupera un placeholder dato un tipo
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static String GetPlaceHolder(PlaceHoldersType type)
		{
			return string.Format("{0}{1}{2}", OpenTag, type.ToString(), CloseTag);
		}

		/// <summary>
		/// Recupera un placeholder dato un tipo
		/// </summary>
		/// <param name="type"></param>
		/// <param name="placeHolderString"></param>
		/// <returns></returns>
		public static String GetPlaceHolder(String placeHolderString)
		{
			return string.Format("{0}{1}{2}", OpenTag, placeHolderString, CloseTag);
		}

		// ------   FUNZIONI COPIATE DA lm.Comol.Modules.Standard.WebConferencing.Domain.TemplatePlaceHolders   ------
		/// <summary>
		/// Dictionary placeholder
		/// </summary>
		/// <returns>Dictionary placeholder con tipo + stringa visualizzata</returns>
		public static Dictionary<PlaceHoldersType, String> PlaceHolders()
		{
			if (placeHolders.Count == 0)
			{
				foreach (PlaceHoldersType p in (from t in Enum.GetValues(typeof(PlaceHoldersType)).Cast<PlaceHoldersType>() where t != PlaceHoldersType.None select t))
				{
					placeHolders.Add(p, String.Format(tagString, OpenTag, p.ToString(), CloseTag));
				}
			}
			return placeHolders;
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
			return (!String.IsNullOrEmpty(content) && 
				(content.Contains(GetPlaceHolder(PlaceHoldersType.UserToken))
				|| content.Contains(GetPlaceHolder(PlaceHoldersType.UserTokenExpiration))
				|| content.Contains(GetPlaceHolder(PlaceHoldersType.UserTokenUrl))
				|| content.Contains(GetPlaceHolder(PlaceHoldersType.UserMail))
				|| content.Contains(GetPlaceHolder(PlaceHoldersType.UserName))
				|| content.Contains(GetPlaceHolder(PlaceHoldersType.UserSurname))
				|| content.Contains(GetPlaceHolder(PlaceHoldersType.UserPassword))
				|| content.Contains(GetPlaceHolder(PlaceHoldersType.UserLanguageCode))
				));
		}

		public static List<PlaceHoldersType> GetPlaceHoldersByType(ModuleTicket.MailSenderActionType ActionType)
		{
			List<PlaceHoldersType> PLHlist = new List<PlaceHoldersType>();
			PLHlist.Add(PlaceHoldersType.None);

			switch (ActionType)
			{

				case ModuleTicket.MailSenderActionType.externalPasswordChanged:
					PLHlist.Add(PlaceHoldersType.UserMail);        
					PLHlist.Add(PlaceHoldersType.UserName);
					PLHlist.Add(PlaceHoldersType.UserSurname);
					PLHlist.Add(PlaceHoldersType.UserLanguageCode);
					PLHlist.Add(PlaceHoldersType.ExternalAccessUrl);
					break;

				case ModuleTicket.MailSenderActionType.externalRecover:
					PLHlist.Add(PlaceHoldersType.UserMail);        
					PLHlist.Add(PlaceHoldersType.UserName);
					PLHlist.Add(PlaceHoldersType.UserSurname);
					PLHlist.Add(PlaceHoldersType.UserLanguageCode);
					PLHlist.Add(PlaceHoldersType.UserPassword);
					PLHlist.Add(PlaceHoldersType.ExternalAccessUrl);
					break;

				case ModuleTicket.MailSenderActionType.externalRegistration:
					PLHlist.Add(PlaceHoldersType.UserMail);
					PLHlist.Add(PlaceHoldersType.UserName);
					PLHlist.Add(PlaceHoldersType.UserSurname);
					PLHlist.Add(PlaceHoldersType.UserPassword);
					PLHlist.Add(PlaceHoldersType.UserLanguageCode);
					PLHlist.Add(PlaceHoldersType.UserToken);
					PLHlist.Add(PlaceHoldersType.UserTokenExpiration);
					PLHlist.Add(PlaceHoldersType.UserTokenUrl);
					PLHlist.Add(PlaceHoldersType.ExternalAccessUrl);
					break;

				//NUOVI
				case ModuleTicket.MailSenderActionType.TicketNewUser:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlUser);
					break;

				case ModuleTicket.MailSenderActionType.TicketSendMessageUser:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlUser);
					break;

				case ModuleTicket.MailSenderActionType.TicketModeratedUser:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlUser);
					break;

				case ModuleTicket.MailSenderActionType.TicketCategoryResetUser:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlUser);

					PLHlist.Add(PlaceHoldersType.CategoryName);
					PLHlist.Add(PlaceHoldersType.CategoryDescription);
					break;

				case ModuleTicket.MailSenderActionType.TicketOwnerChanged:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlUser);
					break;

				case ModuleTicket.MailSenderActionType.TicketNewMan:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlManager);
					break;

				case ModuleTicket.MailSenderActionType.TicketSendMessageMan:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlManager);
					break;

				case ModuleTicket.MailSenderActionType.TicketModeratedMan:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlManager);
					break;

				case ModuleTicket.MailSenderActionType.TicketCategoryResetMan:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlManager);

					PLHlist.Add(PlaceHoldersType.CategoryName);
					PLHlist.Add(PlaceHoldersType.CategoryDescription);
					break;

				case ModuleTicket.MailSenderActionType.TicketCategoryAdd:
					PLHlist.Add(PlaceHoldersType.TicketStatus);
					PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
					PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
					PLHlist.Add(PlaceHoldersType.TicketAssigner);
					PLHlist.Add(PlaceHoldersType.TicketObject);
					PLHlist.Add(PlaceHoldersType.TicketPreview);
					PLHlist.Add(PlaceHoldersType.TicketLongText);
					PLHlist.Add(PlaceHoldersType.TicketSendDate);
					PLHlist.Add(PlaceHoldersType.TicketLanguage);
					PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
					PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

					PLHlist.Add(PlaceHoldersType.TicketUrlManager);

					PLHlist.Add(PlaceHoldersType.CategoryName);
					PLHlist.Add(PlaceHoldersType.CategoryDescription);
					break;

				case ModuleTicket.MailSenderActionType.TicketAssignmentAddAssigner:
						PLHlist.Add(PlaceHoldersType.TicketStatus);
						PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
						PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
						PLHlist.Add(PlaceHoldersType.TicketAssigner);
						PLHlist.Add(PlaceHoldersType.TicketObject);
						PLHlist.Add(PlaceHoldersType.TicketPreview);
						PLHlist.Add(PlaceHoldersType.TicketLongText);
						PLHlist.Add(PlaceHoldersType.TicketSendDate);
						PLHlist.Add(PlaceHoldersType.TicketLanguage);
						PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
						PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

						PLHlist.Add(PlaceHoldersType.TicketUrlManager);
					break;

				case ModuleTicket.MailSenderActionType.TicketAssignmentAddManager:
						PLHlist.Add(PlaceHoldersType.TicketStatus);
						PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
						PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
						PLHlist.Add(PlaceHoldersType.TicketAssigner);
						PLHlist.Add(PlaceHoldersType.TicketObject);
						PLHlist.Add(PlaceHoldersType.TicketPreview);
						PLHlist.Add(PlaceHoldersType.TicketLongText);
						PLHlist.Add(PlaceHoldersType.TicketSendDate);
						PLHlist.Add(PlaceHoldersType.TicketLanguage);
						PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
						PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

						PLHlist.Add(PlaceHoldersType.TicketUrlManager);
					break;

				case ModuleTicket.MailSenderActionType.CategoryModified:
					PLHlist.Add(PlaceHoldersType.CategoryName);
					PLHlist.Add(PlaceHoldersType.CategoryDescription);

					PLHlist.Add(PlaceHoldersType.TicketUrlListManager);
					break;

				case ModuleTicket.MailSenderActionType.CategoryReorder:
					PLHlist.Add(PlaceHoldersType.TicketUrlListManager);
					break;
				//case ModuleTicket.MailSenderActionType.addAnswer:
				//    PLHlist.Add(PlaceHoldersType.TicketStatus);
				//    PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
				//    PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
				//    PLHlist.Add(PlaceHoldersType.TicketAssigner);
				//    PLHlist.Add(PlaceHoldersType.TicketObject);
				//    PLHlist.Add(PlaceHoldersType.TicketPreview);
				//    PLHlist.Add(PlaceHoldersType.TicketLongText);
				//    PLHlist.Add(PlaceHoldersType.TicketSendDate);
				//    PLHlist.Add(PlaceHoldersType.TicketLanguage);
				//    PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
				//    PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

				//    PLHlist.Add(PlaceHoldersType.ActionDisplayName);
				//    PLHlist.Add(PlaceHoldersType.ActionRole);

				//    PLHlist.Add(PlaceHoldersType.AnswerShortText);
				//    PLHlist.Add(PlaceHoldersType.AnswerFullText);
				//    break;

				//case ModuleTicket.MailSenderActionType.assignmentChange:
				//    PLHlist.Add(PlaceHoldersType.TicketStatus);
				//    PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
				//    PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
				//    PLHlist.Add(PlaceHoldersType.TicketAssigner);
				//    PLHlist.Add(PlaceHoldersType.TicketObject);
				//    PLHlist.Add(PlaceHoldersType.TicketPreview);
				//    PLHlist.Add(PlaceHoldersType.TicketLongText);
				//    PLHlist.Add(PlaceHoldersType.TicketSendDate);
				//    PLHlist.Add(PlaceHoldersType.TicketLanguage);
				//    PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
				//    PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

				//    PLHlist.Add(PlaceHoldersType.ActionDisplayName);
				//    PLHlist.Add(PlaceHoldersType.ActionRole);
				//    break;

				//case ModuleTicket.MailSenderActionType.statusChange:
				//    PLHlist.Add(PlaceHoldersType.TicketStatus);
				//    PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
				//    PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
				//    PLHlist.Add(PlaceHoldersType.TicketAssigner);
				//    PLHlist.Add(PlaceHoldersType.TicketObject);
				//    PLHlist.Add(PlaceHoldersType.TicketPreview);
				//    PLHlist.Add(PlaceHoldersType.TicketLongText);
				//    PLHlist.Add(PlaceHoldersType.TicketSendDate);
				//    PLHlist.Add(PlaceHoldersType.TicketLanguage);
				//    PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
				//    PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

				//    PLHlist.Add(PlaceHoldersType.ActionDisplayName);
				//    PLHlist.Add(PlaceHoldersType.ActionRole);
				//    break;


				//case ModuleTicket.MailSenderActionType.categoryChange:
				//    PLHlist.Add(PlaceHoldersType.ActionDisplayName);
					
				//    PLHlist.Add(PlaceHoldersType.CategoryName);
				//    PLHlist.Add(PlaceHoldersType.CategoryDescription);
				//    PLHlist.Add(PlaceHoldersType.CategoryLONGNameAndDescriptionLIST);
				//    PLHlist.Add(PlaceHoldersType.CategoryType);
				//    PLHlist.Add(PlaceHoldersType.CategoryLanguagesCodeList);
				//    break;

				//case ModuleTicket.MailSenderActionType.categoryAssignedChange:
				//    PLHlist.Add(PlaceHoldersType.ActionDisplayName);
					
				//    PLHlist.Add(PlaceHoldersType.CategoryName);
				//    PLHlist.Add(PlaceHoldersType.CategoryDescription);
				//    PLHlist.Add(PlaceHoldersType.CategoryNewAssignerDisplayName);
				//    break;

			}

			//FULL LIST

			//PLHlist.Add(PlaceHoldersType.None);
			//PLHlist.Add(PlaceHoldersType.UserMail);
			//PLHlist.Add(PlaceHoldersType.UserName);
			//PLHlist.Add(PlaceHoldersType.UserSureName);
			//PLHlist.Add(PlaceHoldersType.UserPassword);
			//PLHlist.Add(PlaceHoldersType.UserLanguageCode);
			//PLHlist.Add(PlaceHoldersType.UserToken);
			//PLHlist.Add(PlaceHoldersType.UserTokenExpiration);
			//PLHlist.Add(PlaceHoldersType.UserTokenUrl);
			//PLHlist.Add(PlaceHoldersType.ExternalAccessUrl);

			//PLHlist.Add(PlaceHoldersType.TicketStatus);
			//PLHlist.Add(PlaceHoldersType.TicketCategoryCurrent);
			//PLHlist.Add(PlaceHoldersType.TicketCategoryInitial);
			//PLHlist.Add(PlaceHoldersType.TicketAssigner);
			//PLHlist.Add(PlaceHoldersType.TicketObject);
			//PLHlist.Add(PlaceHoldersType.TicketPreview);
			//PLHlist.Add(PlaceHoldersType.TicketLongText);
			//PLHlist.Add(PlaceHoldersType.TicketSendDate);
			//PLHlist.Add(PlaceHoldersType.TicketLanguage);
			//PLHlist.Add(PlaceHoldersType.TicketLanguageCode);
			//PLHlist.Add(PlaceHoldersType.TicketCreatorDisplayName);

			//PLHlist.Add(PlaceHoldersType.ActionDisplayName);
			//PLHlist.Add(PlaceHoldersType.ActionRole);

			//PLHlist.Add(PlaceHoldersType.AnswerShortText);
			//PLHlist.Add(PlaceHoldersType.AsnwerFullText);

			//PLHlist.Add(PlaceHoldersType.CategoryName);
			//PLHlist.Add(PlaceHoldersType.CategoryDescription);
			//PLHlist.Add(PlaceHoldersType.CategoryLONGNameAndDescriptionLIST);
			//PLHlist.Add(PlaceHoldersType.CategoryType);
			//PLHlist.Add(PlaceHoldersType.CategoryLanguagesCodeList);
			//PLHlist.Add(PlaceHoldersType.CategoryNewAssignerDisplayName);
			



			//return (from t in Enum.GetValues(typeof(PlaceHoldersType)).Cast<PlaceHoldersType>()
			//        where t != PlaceHoldersType.None
			//        select t).ToList();

			return PLHlist;
		}
		// ------   FUNZIONI COPIATE/RIADATTATE DA lm.Comol.Modules.Standard.WebConferencing.Domain.TemplatePlaceHolders   ------

	}



	/// <summary>
	/// Placeholder TICKET
	/// </summary>
	[Serializable()]
	public enum PlaceHoldersType
	{
		/// <summary>
		/// Nessuno/vuoto
		/// </summary>
		None = 0,
		/// <summary>
		/// USED - Indirizzo mail utente
		/// </summary>
		UserMail = 3,
		/// <summary>
		/// USED - Nome Utente
		/// </summary>
		UserName = 4,
		/// <summary>
		/// USED - Cognome Utente
		/// </summary>
		UserSurname = 5,
		/// <summary>
		/// USED - Password assegnata all'utente
		/// </summary>
		UserPassword = 6,
		/// <summary>
		/// USED - Codice lingua associata all'utente
		/// </summary>
		UserLanguageCode = 7,
		/// <summary>
		/// USED - Token attivazione utente
		/// </summary>
		UserToken = 10,
		/// <summary>
		/// USED - Data scadenza TOKEN -> ToDo: controlli scadenza! (Validità 7 giorni HardCoded)
		/// </summary>
		UserTokenExpiration = 11,
		/// <summary>
		/// USED - Url contenente il token
		/// Se il token è null: url della pagina di login
		/// </summary>
		UserTokenUrl = 12,
		/// <summary>
		/// USED - Url Accesso Esterno
		/// </summary>
		ExternalAccessUrl = 13,
		
		// SEGUIRANNO TUTTI GLI ALTRI PLACEHOLDER (quando si svilupperà l'invio "massimo" di mail)
		
		/// <summary>
		/// Stato corrente del Ticket
		/// </summary>
		TicketStatus = 20,
		/// <summary>
		/// Categoria corrente del Ticket
		/// </summary>
		TicketCategoryCurrent = 21,
		/// <summary>
		/// Categoria iniziale del Ticket
		/// </summary>
		TicketCategoryInitial = 22,
		/// <summary>
		/// Eventuale assegnatario Ticket
		/// </summary>
		TicketAssigner = 23,
		/// <summary>
		/// Oggetto/Titolo del Ticket
		/// </summary>
		TicketObject = 25,
		/// <summary>
		/// Testo breve (plain) del primo post del Ticket
		/// </summary>
		TicketPreview = 26,
		/// <summary>
		/// Testo COMPLETO (plain) del primo post del Ticket
		/// </summary>
		TicketLongText = 27,
		/// <summary>
		/// Data invio ticket
		/// </summary>
		TicketSendDate = 28,
		/// <summary>
		/// Lingua corrente ticket
		/// </summary>
		TicketLanguage = 31,
		/// <summary>
		/// Codice lingua del Ticket
		/// </summary>
		TicketLanguageCode = 32,
		/// <summary>
		/// Nome del creatore del Ticket
		/// </summary>
		TicketCreatorDisplayName = 33,
		/// <summary>
		/// Nome del creatore del Ticket
		/// </summary>
		TicketOwnerDisplayName = 34,
		/// <summary>
		/// Nome visualizzato di CHI ha fatto l'azione (tipo per l'utente può anche essere solo "manager"...)
		/// </summary>
		ActionDisplayName = 35,
		/// <summary>
		/// TOGLIERE! "Qualifica" di chi ha fatto l'azione.
		/// </summary>
		ActionRole = 36,

		/// <summary>
		/// Testo breve risposta (plain)
		/// </summary>
		AnswerShortText = 40,
		/// <summary>
		/// Testo completo risposta (HTML)
		/// </summary>
		AnswerFullText = 41,

		/// <summary>
		/// Nome categoria (Lingua Utente)
		/// </summary>
		CategoryName = 50,
		/// <summary>
		/// Descrizione categoria (Lingua Utente)
		/// </summary>
		CategoryDescription = 51,
		///// <summary>
		///// Lista con lingua, nome e descrizione per tutte le lingue impostate
		///// </summary>
		//CategoryLONGNameAndDescriptionLIST = 52,
		/// <summary>
		/// Tipo categoria
		/// </summary>
		CategoryType = 53,
		///// <summary>
		///// Lista con tutte le lingue impostate per quella categoria
		///// </summary>
		//CategoryLanguagesCodeList = 54,
		///// <summary>
		///// ELENCO ASSEGNATARI: eventualmente modificare!
		///// </summary>
		//CategoryNewAssignerDisplayName = 55,
		/// <summary>
		/// Lista semplice (nome con link) di categorie di un utente in una comunità.
		/// </summary>
		CategoryLinkListSimple = 54,
		/// <summary>
		/// Lista avanzata (nome, link descrizione) di categorie di un utente in una comunità.
		/// </summary>
		CategoryLinkListFull = 55,

		/// <summary>
		/// Url al Ticket x utente
		/// </summary>
		TicketUrlUser = 60,
		/// <summary>
		/// Url al Ticket x manager
		/// </summary>
		TicketUrlManager = 61,
		/// <summary>
		/// Url alla lista amministrazione Ticket x manager
		/// </summary>
		TicketUrlListManager = 62,
		/// <summary>
		/// Codice Ticket (al momento ID)
		/// </summary>
		TicketCode = 63,

		/// <summary>
		/// Url per l'impostazione dei messaggi
		/// </summary>
		UserNotificationSettingsUrl = 70,
		/// <summary>
		/// Codice identificativo utente
		/// </summary>
		UserIdCode = 71
	}

	// Full Plain list...
	//None
	//UserMail
	//UserName
	//UserSurname
	//UserPassword
	//UserLanguageCode
	//UserToken
	//UserTokenExpiration
	//UserTokenUrl
	//ExternalAccessUrl
		
	//TicketStatus
	//TicketCategoryCurrent
	//TicketCategoryInitial
	//TicketAssigner
	//TicketObject
	//TicketPreview
	//TicketLongText
	//TicketSendDate
	//TicketLanguage
	//TicketLanguageCode
	//TicketCreatorDisplayName

	//ActionDisplayName
	//ActionRole

	//AnswerShortText
	//AsnwerFullText 

	//CategoryName
	//CategoryDescription
	//CategoryLONGNameAndDescriptionLIST
	//CategoryType
	//CategoryLanguagesCodeList
	//CategoryNewAssignerDisplayName
}

