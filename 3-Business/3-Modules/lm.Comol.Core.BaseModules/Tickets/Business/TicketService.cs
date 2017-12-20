using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

using lm.Comol.Core.BaseModules.Tickets.Domain;
using NHibernate.Mapping;


using AuthHelper = lm.Comol.Core.Authentication.Helpers;


namespace lm.Comol.Core.BaseModules.Tickets
{
	public partial class TicketService : CoreServices
	{

	#region Costanti

		/// <summary>
		/// Codice lingua "Multi"
		/// </summary>
		public const string LangMultiCODE = "MULTI";
		/// <summary>
		/// ID lingua multi
		/// </summary>
		public const int LangMultiID = -1;
		/// <summary>
		/// ??? Inutile?
		/// </summary>
		public const int PreviewMaxLenght = 250;
		/// <summary>
		/// Costante che mi indica che la comunità indicata è il portale
		/// </summary>
		public const String ComPortalName = "-Portal-";
		/// <summary>
		/// Errore caricamento comunità
		/// </summary>
		public const String ComErrName = "-Error-";
		/// <summary>
		/// Massimo di record per "WHERE" in con ListaId.Contains(obj.Id),
		/// per evitare errori in generazione query troppo grandi
		/// </summary>
		protected const Int32 maxItemsForQuery = 200;
		/// <summary>
		/// Numero massimo di categorie per una comunità (interferisce con il riordino)
		/// </summary>
		private const int maxCategoryPerCommunity = 1000;
		/// <summary>
		/// PlaceHolder che va a sostituire il nome utente nei messaggi
		/// </summary>
		public const String MessageUserPlaceHolder = "{User}";
		/// <summary>
		/// Context
		/// </summary>
		protected iApplicationContext _Context;
		/// <summary>
		/// Prefisso generico chiave CACHE
		/// </summary>
		private const String CacheKey = "Ticket";
		/// <summary>
		/// Chiave CACHE per accesso a TUTTE le categorie.
		/// </summary>
		private const String CacheKeyCategory = "TicketCat";
		/// <summary>
		/// Tempo di vita della cache
		/// </summary>
		public static readonly TimeSpan CacheExpiration = new TimeSpan(7, 0, 0, 0);
		/// <summary>
		/// Vita di un token. Al termine non sarà più considerato valido (e cancellato)
		/// </summary>
		public static readonly TimeSpan TokenLifeTime = new TimeSpan(7, 0, 0, 0);

		private Int32 idModule;
	#endregion
		
	#region Inizializzazione/Costruttori

		public TicketService() :base() { }

		public TicketService(iApplicationContext oContext) :base(oContext.DataContext) {
			_Context = oContext;
			this.Manager = new BaseModuleManager(oContext.DataContext);
			this.ServiceTemplate = new Core.TemplateMessages.Business.TemplatesForOtherService(oContext);
			this.UC = oContext.UserContext;
            _ServiceRepository = new FileRepository.Business.ServiceRepository(oContext);
		}

		public TicketService(iDataContext oDC)
			: base(oDC)
		{
			this.Manager = new BaseModuleManager(oDC);

			//Provo prima con solo oDC...
			this.ServiceTemplate = new Core.TemplateMessages.Business.TemplatesForOtherService(oDC);

			_Context = new ApplicationContext() { DataContext = oDC };
            _ServiceRepository = new FileRepository.Business.ServiceRepository(oDC);
		}

		private lm.Comol.Core.TemplateMessages.Business.TemplatesForOtherService ServiceTemplate;
	#endregion

	#region Permission
        private Int32 _IdTicketModule = 0;
		/// <summary>
		/// Get Service Module Id
		/// </summary>
		/// <returns>Service Module Id</returns>
		public Int32 ServiceModuleID()
		{
            if (_IdTicketModule <= 0)
            {
                _IdTicketModule = Manager.GetModuleID(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode);
            }
            return _IdTicketModule;
		}

		

		//public Int32 GetIdModule()
		//{
		//    if (idModule == 0)
		//        idModule = Manager.GetModuleID(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode);
		//    return idModule;
		//}



		/// <summary>
		/// Indica SE l'utente PUO' creare un Ticket.
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Si basa sui ticket "aperti" (tutti quelli creati dall'utente che non sono in BOZZA nè CHIUSI)
		/// ed alla configurazione di sistema (es: max 5 Ticket per utente)
		/// </remarks>
		public Tickets.Domain.Enums.TicketAddCondition PermissionTicketUsercanCreate()
		{
  

			Domain.DTO.DTO_Access Access = this.SettingsAccessGet(true);

			if (!Access.CanEditTicket)
				return Domain.Enums.TicketAddCondition.NoPermission;

			if (this.CurrentUser == null)
				return Domain.Enums.TicketAddCondition.NoUser;

			if (this.UserIsManagerOrResolver())
				return Domain.Enums.TicketAddCondition.CanCreate;


			SettingsPortal settings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();

			if (!settings.HasInternalLimitation)
				return Domain.Enums.TicketAddCondition.CanCreate;

			return Domain.Enums.TicketAddCondition.CheckCount;

			//Int64 CurrentUserId = this.CurrentUser.Id;

			//int countOpen = (from Ticket Tk in Manager.GetIQ<Ticket>() 
			//                 where Tk.Creator.Id == CurrentUserId 
			//                 && (Tk.Status == Domain.Enums.TicketStatus.open
			//                 || Tk.Status == Domain.Enums.TicketStatus.inProcess
			//                 || Tk.Status == Domain.Enums.TicketStatus.userRequest)
			//                 select Tk.Id).Count();

			//int countDraft = (from Ticket Tk in Manager.GetIQ<Ticket>() where Tk.Creator.Id == CurrentUserId && Tk.Status == Domain.Enums.TicketStatus.draft select Tk.Id).Count();

			//Tickets.Domain.Enums.TicketAddCondition Cond = Domain.Enums.TicketAddCondition.CanCreate;

			//if(countOpen)

			//return ;
		}

		public Tickets.Domain.Enums.TicketAddCondition PermissionTicketUsercanCreateExternal()
		{
			
			SettingsPortal settings = Manager.GetAll<SettingsPortal>().Skip(0).Take(1).ToList().FirstOrDefault();

			if(settings == null)
			{
				settings = new SettingsPortal();
			}

			if (!settings.HasExternalLimitation)
				return Domain.Enums.TicketAddCondition.CanCreate;
			else
				return Domain.Enums.TicketAddCondition.CheckCount;
			
		}

		/// <summary>
		/// Get Specific Service Permission
		/// </summary>
		/// <param name="personId">
		/// Specific Person ID
		/// </param>
		/// <param name="idCommunity">
		/// Specific Community ID
		/// </param>
		/// <returns>ModuleDocTemplate permission</returns>
		public lm.Comol.Core.BaseModules.Tickets.ModuleTicket PermissionGetService(int personId, int idCommunity)
		{
			Person person = Manager.GetPerson(personId);
			return PermissionGetService(person, idCommunity);
		}

		/// <summary>
		/// Get Specific Service Permission
		/// </summary>
		/// <param name="person">
		/// Specific Person
		/// </param>
		/// <param name="idCommunity">
		/// Specific Community ID
		/// </param>
		/// <returns>ModuleDocTemplate permission</returns>
		public lm.Comol.Core.BaseModules.Tickets.ModuleTicket PermissionGetService(Person person, int idCommunity)
		{
			lm.Comol.Core.BaseModules.Tickets.ModuleTicket module = new lm.Comol.Core.BaseModules.Tickets.ModuleTicket();
			if (person == null)
				person = (from p in Manager.GetIQ<Person>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();
			if (idCommunity == 0)
				module = lm.Comol.Core.BaseModules.Tickets.ModuleTicket.CreatePortalmodule((person == null) ? (int)UserTypeStandard.Guest : person.TypeID);
			else
				module = new lm.Comol.Core.BaseModules.Tickets.ModuleTicket(Manager.GetModulePermission(person.Id, idCommunity, ServiceModuleID()));
			return module;
		}
		
	#endregion

	#region Community
				
		/// <summary>
		/// Indica se la comunità CORRENTE può visualizzare categorie di tipo Ticket
		/// </summary>
		/// <returns>TRUE: può vedere categorie di tipo Ticket</returns>
		public Boolean CommunityViewTicket()
		{
			return CommunityViewTicket(UC.CurrentCommunityID);
		}

		/// <summary>
		/// Indica se la comunità indicata può vedere categorie di tipo Ticket
		/// </summary>
		/// <param name="CommunityId">IdComunità</param>
		/// <returns></returns>
		public Boolean CommunityViewTicket(int CommunityId)
		{
			if (CommunityId <= 0)
				return false;

			Community Com = Manager.GetCommunity(CommunityId);

			if (Com == null)
				return false;

			SettingsComType ct = Manager.GetAll<SettingsComType>(sct => sct.CommunityType.Id == Com.IdTypeOfCommunity).FirstOrDefault();

			if (ct == null)
				return false;

			return ct.ViewTicket;
		}

		/// <summary>
		/// Restituisce il NOME della comunità indicata
		/// </summary>
		/// <param name="CommunityId"></param>
		/// <returns>
		///  - Nome comunità indicata
		///  - Errore
		///  - Portale
		/// </returns>
		public String CommunityNameGet(int CommunityId)
		{
			if (CommunityId > 0)
			{
				Community com = Manager.GetCommunity(CommunityId);
				if (com == null)
					return ComErrName;
				return com.Name;
			}
			else if (CommunityId == 0)
				return ComPortalName;

			return "";

		}


	#endregion
		
	#region Languages

		/// <summary>
		/// Recupera le lingue disponibili nel sistema (per modifica categorie)
		/// </summary>
		/// <returns></returns>
		public List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> LanguagesGetAvailableSys()
		{
			List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> items = new List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem>();

			try
			{
				List<Language> languages = Manager.GetAllLanguages().ToList();
				items.AddRange(languages.Select(l => new lm.Comol.Core.DomainModel.Languages.BaseLanguageItem(l)).ToList());
			}
			catch (Exception ex)
			{

			}

			return items;
		}

		/// <summary>
		/// ID Lingua utente corrente
		/// </summary>
		private int LanguageIdCurrentUser
		{
			get
			{
				return UC.Language.Id;
			}
		}

		/// <summary>
		/// ID Lingua sistema
		/// </summary>
		private int LanguageIdSystem
		{
			get
			{
				if (_LanguageIdSystem <= 0)
					_LanguageIdSystem = Manager.GetDefaultLanguage().Id;

				return _LanguageIdSystem;
			}
		}
		private int _LanguageIdSystem = -1;

		/// <summary>
		/// Recupera le lingue attualmente in uso nei Ticket
		/// </summary>
		/// <returns></returns>
		public IList<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> LanguagesGetCurrent()
			
		{
			IList<String> TkLanguages = (from Domain.liteTicket tk in Manager.GetIQ<Domain.liteTicket>()
									   select tk.LanguageCode).Distinct().ToList();

			IList<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem> languages = null;

			if(TkLanguages.Any())
			{
				try
				{

					languages = Manager.GetAllLanguages()
						.Where(l => TkLanguages.Contains(l.Code))
						.Select(l => new lm.Comol.Core.DomainModel.Languages.BaseLanguageItem(l))
						.ToList();
				}
				catch (Exception ex)
				{

				}

				if(languages == null || !languages.Any())
				{
					languages = new List<lm.Comol.Core.DomainModel.Languages.BaseLanguageItem>();
					foreach(String lng in TkLanguages)
					{
						lm.Comol.Core.DomainModel.Languages.BaseLanguageItem bli = new lm.Comol.Core.DomainModel.Languages.BaseLanguageItem();
						bli.Code = lng;
						bli.Name = lng;
						languages.Add(bli);
					}
				}
			}


			return languages;
		}

	#endregion

	#region Token
	
		/// <summary>
		/// Crea un nuovo token
		/// </summary>
		/// <param name="UserId">Id Utente</param>
		/// <returns>Oggetto Token creato o vecchio Token</returns>
		public Token TokenCreate(Int64 UserId, Domain.Enums.TokenType Type)
		{
			if(UserId <= 0)
				return new Token();

			TicketUser Usr = this.UserGet(UserId);
			if(Usr == null || Usr.Id <= 0)
				return new Token();


			Token Tok = Manager.GetAll<Token>(tok => tok.User.Id == UserId).FirstOrDefault();
				//(from Token tok in Manager.GetIQ<Token>() where tok.User.Id == UserId select tok.Code).FirstOrDefault();

			if (Tok == null || Tok.Id <= 0)
			{
				Tok = new Token(Usr, true);
				Tok.Type = Type;
				Manager.SaveOrUpdate<Token>(Tok);
			}   
	
			return Tok;
		}

		/// <summary>
		/// Recupera TOKEN
		/// </summary>
		/// <param name="Code">Codice Token</param>
		/// <returns></returns>
		public Token TokenGet(Guid Code)
		{
			return Manager.GetAll<Token>(tok => tok.Code == Code).FirstOrDefault();
		}

		/// <summary>
		/// Ricrea il Token per l'utente indicato
		/// </summary>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public Token TokenRegenerate(Int64 UserId)
		{
			if (UserId <= 0)
				return new Token();

			TicketUser Usr = this.UserGet(UserId);
			if (Usr == null || Usr.Id <= 0)
				return new Token();


			Token Tok = Manager.GetAll<Token>(tok => tok.User.Id == UserId).FirstOrDefault();

			if (Tok == null || Tok.Id <= 0)
			{
				return new Token();
			}

			Tok.Code = System.Guid.NewGuid();
			Manager.SaveOrUpdate<Token>(Tok);

			return Tok;
		}

		/// <summary>
		/// Convalida un Token: se User valido lo cancella.
		/// </summary>
		/// <param name="Code"></param>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public Domain.Enums.TokenValidationResult TokenValidate(Guid Code, Int64 UserId, Domain.Enums.TokenType Type)
		{
			if (Code == Guid.Empty)
				return Domain.Enums.TokenValidationResult.InvalidFormat;

			if (UserId <= 0)
				return Domain.Enums.TokenValidationResult.UserNotFound;

			TicketUser User = Manager.Get<TicketUser>(UserId);
			if (User == null || User.Id <= 0)
				return Domain.Enums.TokenValidationResult.UserNotFound;

			IList<Token> Tokens = Manager.GetAll<Token>(tok => tok.Code == Code && tok.User.Id == UserId && tok.Type == Type);

			if(User.MailChecked == true)
			{
				Manager.DeletePhysicalList<Token>(Tokens);
				return Domain.Enums.TokenValidationResult.Validated;
			}

			if (Tokens != null && Tokens.Any())
			{
				User.MailChecked = true;
				Manager.SaveOrUpdate<TicketUser>(User);
				Manager.DeletePhysicalList<Token>(Tokens);
				return Domain.Enums.TokenValidationResult.Validated;
			}

			return Domain.Enums.TokenValidationResult.TokenNotFound;
			
		}

		public Domain.Enums.TokenValidationResult TokenValidate(String Code, Int64 UserId, Domain.Enums.TokenType Type)
		{
			if (String.IsNullOrEmpty(Code))
				return Domain.Enums.TokenValidationResult.InvalidFormat;

			System.Guid TokGuid = Guid.Empty;
			try
			{
				TokGuid = new Guid(Code);
			}
			catch { }

			if (TokGuid == null || TokGuid == Guid.Empty)
				return Domain.Enums.TokenValidationResult.InvalidFormat;

			return TokenValidate(TokGuid, UserId, Type);

		}
		
	#endregion

		/// <summary>
		/// ToDo - Converte una string HTML in "testo piano", limitandone la lunghezza
		/// </summary>
		/// <param name="Html"></param>
		/// <param name="MaxChar"></param>
		/// <returns></returns>
		/// <remarks>
		/// No, non converte l'html in testo piano, ma si limita a "ridimensionare" la stringa
		/// </remarks>
		public static String HTMLtoString(String Html, int MaxChar)
		{
			if (String.IsNullOrEmpty(Html))
				return "";
			if (MaxChar > 0 && MaxChar < Html.Length)
				return Html.Substring(0, MaxChar);

			return Html;
		}

		public static bool MailCheckFormat(String Mail)
		{
			
			try
			{
				System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(Mail);
			}
			catch (FormatException)
			{
				return false;
			}

			return true;
		}


		//ToDo!!!!!
		// Vedere pagine di Login di COMOL relativamente alla cifratura...

		///// <summary>
		///// Cifra una stringa
		///// </summary>
		///// <param name="Password">Stringa da cifrare</param>
		///// <returns></returns>
		//public string PasswordSet(String Password)
		//{
		//    return 
		//}


		private AuthHelper.InternalEncryptor _authenticationHelper;
		private AuthHelper.InternalEncryptor AuthenticationHelper
		{
			get
			{
				if(_authenticationHelper == null)
					_authenticationHelper = new AuthHelper.InternalEncryptor();

				return _authenticationHelper;
			}
		}
		
		
		//To Do!
		private String PasswordGetNew()
		{
			return "NewPWD";
		}

		public int ModuleID
		{
			get
			{
				return Manager.GetModuleID(ModuleTicket.UniqueCode);
			}
		}
	}
}
//EOF: 4642 righe (dopo pulizia del 28/04/2014, erano 5250)
//EOF: 5172 righe (al 13/05/2014. Necessaria pulizia)
//EOF: 5446 righe (al 14/05/2014. Necessaria pulizia)