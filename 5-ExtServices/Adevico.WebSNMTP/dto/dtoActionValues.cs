using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adevico.WebSNMTP.dto
{
    /// <summary>
    /// Dto che gestisce i dati inviati via trap
    /// </summary>
    [CLSCompliant(true)]
    public class dtoActionValues
    {
        /// <summary>
        /// Data creazione
        /// </summary>
        private DateTime createdOn {get;set;}
        /// <summary>
        /// Sistema che invia i trap
        /// </summary>
        public String System { get; set; }
        /// <summary>
        /// Progressivo Trap.
        /// NOTA: al riavvio del servizio viene azzerato!
        /// </summary>
        public long Progressive { get; set; }
        /// <summary>
        /// Id evento
        /// </summary>
        public long EventId { get; set; }
        /// <summary>
        /// Id/valore che identifica l'utente
        /// </summary>
        public dtoUserValues User { get; set; }

        public dtoActionData Action { get; set; }

        public dtoActionValues() {
            createdOn = DateTime.Now;
        }

        public dtoActionValues
            (
                long progressive,
                int UserId,
                string Usermail,
                string ActionCodeId,
                int CommunityId,
                long EventId,
                string UsertaxCode = "",
                string UserLogin = "",
                string UserName = "",
                string UserSurname = "",
                string ActionModuleId = "",
                string ActionModuleCode = "",
                string ActionActionTypeId = "",
                bool ActionCommunityIsFederated = false,
                string ActionInteractionType = "",
                string ActionObjectType = "",
                string ActionObjectId = ""
            )
        {
            createdOn = DateTime.Now;
            //System = system;
            Progressive = Progressive;
            EventId = EventId;
            User = new dtoUserValues(
                UserId,
                Usermail,
                UsertaxCode,
                UserLogin,
                UserName,
                UserSurname
                );


        }

        public string GetString(
            string ActionSystem,
            string stringformat, 
            string UserStringFormat, 
            string ActionStringFormat, 
            string DateTimeFormat)
        {
            System = ActionSystem;
                
            /*
            LoggedInfo data:
                0	DateTime
                1	System
                2	Progressivo
                3   UserData
                4   ActionData
            */
            return string.Format(stringformat,
                createdOn.ToString(DateTimeFormat),
                System,
                Progressive,
                EventId,
                User.GetString(UserStringFormat),
                Action.GetString(ActionStringFormat)
                );
        }

        /*
        LoggedInfo data:
            0	DateTime
            1	System
            2	Progressivo
            3   UserData
            4	ActionData


        UserData    {3}
            0   Id
            1	mail
            2	codice fiscale
            3	login
            4	nome
            5	cognome


        ActionData  {4}
            0	ModuleId
            2	ModuleCode
            3   ActionCodeId
            4	ActionTypeId
            5	CommunityId
            6	CommunityIsFederated
            7	InteractionType
            8	ObjectType
            9	ObjectId

    */
    }

    public class dtoUserValues
    {
        public int id { get; set; }
        public string mail { get; set; }
        public string taxCode  { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string Ip { get; set; }
        public string ProxyIp { get; set; }

        public dtoUserValues() { }

        public dtoUserValues(
            int UserId,
            string Usermail,
            string UsertaxCode = "",
            string UserLogin = "",
            string UserName = "",
            string UserSurname = "",
            string UserIp = "",
            string UserProxyIp = ""
            )
        {
            id = UserId;
            mail = Usermail;
            taxCode = UsertaxCode;
            login = UserLogin;
            name = UserName;
            surname = UserSurname;
            Ip = UserIp;
            ProxyIp = UserProxyIp;
        }

        public string GetString(string stringformat)
        {
            /*
            UserData    {3}
                0   Id
                1	mail
                2	codice fiscale
                3	login
                4	nome
                5	cognome
                6   Ip
                7   ProxyIp
             */
            return string.Format(stringformat,
                id,
                mail,
                taxCode,
                login,
                name,
                surname,
                Ip,
                ProxyIp
                );
        }
    }

    /// <summary>
    /// Dati azione
    /// </summary>
    public class dtoActionData
    {
        /// <summary>
        /// Id modulo/servizio (diverso per le varie istanze)
        /// </summary>
        public string ModuleId { get; set; }
        /// <summary>
        /// Codice modulo/servizio (univoco su tutte le istanze
        /// </summary>
        public string ModuleCode { get; set; }
        /// <summary>
        /// Codice azione
        /// </summary>
        public string ActionCodeId { get; set; }
        /// <summary>
        /// Tipo di azione
        /// </summary>
        public string ActionTypeId { get; set; }
        /// <summary>
        /// Comunità in cui si è svolta l'azione
        /// </summary>
        public int CommunityId { get; set; }
        /// <summary>
        /// Se la comunità è federata
        /// </summary>
        public bool CommunityIsFederated { get; set; }
        /// <summary>
        /// Tipo di interazione
        /// </summary>
        public string InteractionType { get; set; }
        /// <summary>
        /// Tipo di oggetto interessato
        /// </summary>
        public string ObjectType { get; set; }
        /// <summary>
        /// Id oggetto
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Messaggi in caso di errore nell'azione
        /// </summary>
        public string SuccessInfo { get; set; }
        /// <summary>
        /// Per ulteriori informaizoni
        /// </summary>
        public string GenericInfo { get; set; }
        
        /// <summary>
        /// Costruttore vuoto
        /// </summary>
        public dtoActionData() { }

        /// <summary>
        /// Costruttore parametrizzato per tutte le proprietà dell'oggetto
        /// </summary>
        /// <param name="ActionActionCodeId">Id codice azione</param>
        /// <param name="ActionCommunityId">Id comunità</param>
        /// <param name="ActionModuleId">Id modulo</param>
        /// <param name="ActionModuleCode">Codice modulo</param>
        /// <param name="ActionActionTypeId">Tipo di azione</param>
        /// <param name="ActionCommunityIsFederated">Se la comunità è federata</param>
        /// <param name="ActionInteractionType">Tipo di interazione</param>
        /// <param name="ActionObjectType">Tipo di oggetto</param>
        /// <param name="ActionObjectId">Id dell'oggetto</param>
        /// <param name="ActionSuccessInfo">Informazioni in caso di errore</param>
        /// <param name="ActionGenericInfo">Informaizoni generiche</param>
        public dtoActionData(
            string ActionActionCodeId,
            int ActionCommunityId,
            string ActionModuleId = "",
            string ActionModuleCode = "",
            string ActionActionTypeId = "",
            bool ActionCommunityIsFederated = false,
            string ActionInteractionType = "",
            string ActionObjectType = "",
            string ActionObjectId = "",
            string ActionSuccessInfo = "",
            string ActionGenericInfo = ""
            )
        {
            ModuleId = ActionModuleId;
            ModuleCode = ActionModuleCode;
            ActionTypeId = ActionActionTypeId;
            CommunityId = CommunityId;
            ActionCodeId = ActionActionCodeId;
            CommunityIsFederated = ActionCommunityIsFederated;
            InteractionType = ActionInteractionType;
            ObjectType = ActionObjectType;
            ObjectId = ActionObjectId;
            SuccessInfo = ActionSuccessInfo;
            GenericInfo = ActionGenericInfo;
        }
        /*
        
            */
        /// <summary>
        /// Recupera l'oggetto in formato stringa
        /// </summary>
        /// <param name="stringformat">Stringa con il formato previsto</param>
        /// <returns></returns>
        /// <remarks>
        /// Il formato stringa permette di trasformare l'oggetto in una qualunque stringa: json, html, xml, etc...
        /// Viene sostituita ActionData  {4} ed i valori nella stringa con il formato sono i seguenti:
        ///     0	ModuleId
        ///     1	ModuleCode
        ///     2   ActionCodeId
        ///     3	ActionTypeId
        ///     4	CommunityId
        ///     5	CommunityIsFederated
        ///     6	InteractionType
        ///     7	ObjectType
        ///     8	ObjectId
        ///     9   SuccessInfo
        ///     10  GenericInfo
        /// </remarks>
        public string GetString(string stringformat)
        {
            return string.Format(stringformat,
                ModuleId,
                ModuleCode,
                ActionCodeId,
                ActionTypeId,
                CommunityId,
                CommunityIsFederated,
                InteractionType,
                ObjectType,
                ObjectId,
                SuccessInfo,
                GenericInfo);
        }

    }
}