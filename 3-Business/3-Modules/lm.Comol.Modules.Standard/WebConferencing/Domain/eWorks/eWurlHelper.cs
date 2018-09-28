using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks
{
    /// <summary>
    /// Helper creazione URL
    /// </summary>
    /// <remarks>
    /// eMail o UserId dell'utente è inteso SEMPRE come    M A I L    dell'UTENTE!!!
    /// </remarks>
    public class eWurlHelper
    {
        /// <summary>
        /// Crea la Master KEy di un metting (in pratica una stanza)
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <param name="UserId">ID utente per il quale creare una stanza</param>
        /// <param name="UserDisplayName">Nome da visualizzare per quell'utente in quella stanza</param>
        /// <param name="StartDate">Data inizio</param>
        /// <param name="Title">Titolo Stanza</param>
        /// <param name="MinutesDuration">Durata Meetign prevista in minuti (solo info)</param>
        /// <returns></returns>
        public static string CreateMasterKey(
            String BasePath,
            String MainUserId, String MainPwd, 
            String UserId, String UserDisplayName,
            DateTime? StartDate, String Title,
            Int32 MinutesDuration)
        {
            String url = BasePath +
                "manager.php?" +
                "method=CreateMasterKey" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&username=" + UserDisplayName +
                "&userid=" + UserId +
                "&duration=" + MinutesDuration.ToString() +
                "&title=" + Title;
            if(StartDate != null)
            {
                url += "&startdate=" + (StartDate ?? DateTime.Now).ToString("yyyyMMddHHmmss");
            }

            return url;
        }

        /// <summary>
        /// Crea una chiave d'accesso ad un meeting
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <param name="MasterKey">Chiave master del meeting a cui aggiungere una nuova chiave (utente)</param>
        /// <param name="UserId">Utente da aggiungere alla stanza</param>
        /// <param name="UserDisplayName">Nome visualizzato per quell'utente</param>
        /// <returns></returns>
        public static string CreateKey(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey,
            String UserId, String UserDisplayName
            )
        {
            return BasePath +
                "manager.php?" +
                "method=CreateKey" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey +
                "&userid=" + UserId +
                "&username=" + UserDisplayName;
        }

        /// <summary>
        /// Recupera l'elenco delle Chiavi master (stanze) di un dato utente.
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <returns></returns>
        public static string RetrieveMasterKeys(
            String BasePath,
            String MainUserId, String MainPwd
            )
        {
            return BasePath +
                "manager.php?" +
                "method=RetrieveMasterKeys" +
                "&login=" + MainUserId +
                "&password=" + MainPwd;
        }

        /// <summary>
        /// Cancella una chiave d'accesso (utente)
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <param name="MasterKey">Chiave Master della stanza</param>
        /// <param name="Key">Chiave da eliminare</param>
        /// <returns></returns>
        public static string DeleteKey(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey, String Key
            )
        {
            return BasePath +
                "manager.php?" +
                "method=DeleteKey" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey +
                "&key=" + Key
                ;
        }

        /// <summary>
        /// Disabilita una chiave d'accesso (utente)
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <param name="MasterKey">Chiave Master della stanza</param>
        /// <param name="Key">Chiave da disabilitare</param>
        /// <returns></returns>
        public static string DisableKey(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey, String Key
            )
        {
            return BasePath +
                "manager.php?" +
                "method=DisableKey" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey +
                "&key=" + Key
                ;
        }

        /// <summary>
        /// Abilita una chiave d'accesso (utente)
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <param name="MasterKey">Chiave Master della stanza</param>
        /// <param name="Key">Chiave da disabilitare</param>
        /// <returns></returns>
        public static string EnableKey(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey, String Key
            )
        {
            return BasePath +
                "manager.php?" +
                "method=EnableKey" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey +
                "&key=" + Key
                ;
        }

        /// <summary>
        /// Recupera l'elenco di utenti presenti in un data stanza
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente che può creare stanze</param>
        /// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
        /// <param name="MasterKey">Chiave Master della stanza</param>
        /// <returns></returns>
        public static string RetrieveUsers(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey
            )
        {
            return BasePath +
                "manager.php?" +
                "method=RetrieveUsers" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey
                ;
        }


        /// <summary>
        /// Recupera il report di un dato meeting
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente che può creare stanze</param>
        /// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
        /// <param name="Key">Una chiave valida per il meeting</param>
        /// <returns></returns>
        public static string GetMeetingReport(
            String BasePath,
            String MainUserId, String MainPwd,
            String Key
            )
        {
            return BasePath +
                "manager.php?" +
                "method=GetMeetingReport" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&key=" + Key
                ;
        }

        /// <summary>
        /// Recupera i parametri di un meeting
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <param name="MasterKey">Chiave Master della stanza</param>
        /// <returns></returns>
        public static string GetParameters(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey
            )
        {
            return BasePath +
                "manager.php?" +
                "method=GetParameters" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey
                ;
        }

        /// <summary>
        /// Imposta uno o più parametri per una conferenza.
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente che può creare stanze</param>
        /// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
        /// <param name="MasterKey">Chiave Master della stanza</param>
        /// <param name="Parameters">Lista chiave valore dei parametri</param>
        /// <returns></returns>
        /// <remarks>
        /// Viene controllata SOLAMENTE l'esistenza della chiave.
        /// I valori dei relativi formati NON vengono controllati!
        /// </remarks>
        public static List<string> SetParameters(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey,
            eWRoomParameters Parameters,
            int MaxUrlChars, Boolean IsForCreation,
            string Version
            )
        {

             List<string> OutStrs = new List<string>();

            String PrimaryUrl = BasePath +
                "manager.php?" +
                "method=SetParameters" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey;

            int PrimaryUrlChars = PrimaryUrl.Length;

            String CurrentUrl = PrimaryUrl;

            bool IsFirst = true;

            foreach (string str in eWRoomParameterToUrls(Parameters, IsForCreation, Version))
             {
                 if (IsFirst)
                 {
                     CurrentUrl += str;
                     IsFirst = false;
                 }
                 else
                 {
                     if (PrimaryUrlChars + str.Length <= MaxUrlChars)
                     {
                         CurrentUrl += str;
                     }
                     else
                     {
                         OutStrs.Add(CurrentUrl);
                         CurrentUrl = PrimaryUrl + str;
                     }
                 }
             }

             OutStrs.Add(CurrentUrl);

            return OutStrs;
        }

        public static List<string> SetName(
           String BasePath,
           String MainUserId, String MainPwd,
           String MasterKey,
           String NewName,
           int MaxUrlChars,
           string Version
           )
        {

            List<string> OutStrs = new List<string>();

            String PrimaryUrl = BasePath +
                "manager.php?" +
                "method=SetParameters" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey;

            //int PrimaryUrlChars = PrimaryUrl.Length;

            if (!String.IsNullOrEmpty(NewName)) { OutStrs.Add(PrimaryUrl + "&meetingtitle=" + NewName); }
            
            //bool IsFirst = true;

            //foreach (string str in eWRoomParameterToUrls(Parameters, IsForCreation, Version))
            //{
            //    if (IsFirst)
            //    {
            //        CurrentUrl += str;
            //        IsFirst = false;
            //    }
            //    else
            //    {
            //        if (PrimaryUrlChars + str.Length <= MaxUrlChars)
            //        {
            //            CurrentUrl += str;
            //        }
            //        else
            //        {
            //            OutStrs.Add(CurrentUrl);
            //            CurrentUrl = PrimaryUrl + str;
            //        }
            //    }
            //}

            //OutStrs.Add(CurrentUrl);

            return OutStrs;
        }
        
        /// <summary>
        /// Recupera i dettagli per una conferenza.
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente che può creare stanze</param>
        /// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
        /// <param name="MasterKey">Chiave Master della stanza</param>
        /// <param name="Parameters">Lista chiave valore dei parametri</param>
        /// <returns></returns>
        /// <remarks>
        /// Viene controllata SOLAMENTE l'esistenza della chiave.
        /// I valori dei relativi formati NON vengono controllati!
        /// </remarks>
        public static string GetMeetingDetails(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey
            )
        {
            String Url = BasePath +
                "manager.php?" +
                "method=GetMeetingDetails" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey
                ;

            return Url;
        }

        /// <summary>
        /// Recupera informazioni su una data chiave
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente che può creare stanze</param>
        /// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
        /// <param name="Key">Una chiave valida</param>
        /// <param name="Parameters">Lista chiave valore dei parametri</param>
        /// <returns></returns>
        /// <remarks>
        /// Viene controllata SOLAMENTE l'esistenza della chiave.
        /// I valori dei relativi formati NON vengono controllati!
        /// </remarks>
        public static string GetKeyInfo(
            String BasePath,
            String MainUserId, String MainPwd,
            String Key
            )
        {
            return BasePath +
                "manager.php?" +
                "method=GetKeyInfo" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&key=" + Key
                ;

        }

        /// <summary>
        /// Il metodo GetMeetingList permette di ottenere un elenco di meeting relativo a un utente della piattaforma. 
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente che può creare stanze</param>
        /// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
        /// <param name="UserId">Identificativo utente piattaforma</param>
        /// <returns></returns>
         public static string GetMeetingList(String BasePath,
            String MainUserId, String MainPwd,
            String UserId
            )
        {
            return BasePath +
               "manager.php?" +
               "method=GetMeetingList" +
               "&login=" + MainUserId +
               "&password=" + MainPwd +
               "&userid=" + UserId
               ;
        }

        /// <summary>
        /// Imposta parametri utente
        /// </summary>
        /// <param name="BasePath"></param>
        /// <param name="MainUserId"></param>
        /// <param name="MainPwd"></param>
        /// <param name="Key"></param>
        /// <param name="IsHost"></param>
        /// <param name="IsController"></param>
        /// <param name="Audio"></param>
        /// <param name="Video"></param>
        /// <param name="Chat"></param>
        /// <returns></returns>
        public static string SetUserParameter(String BasePath,
            String MainUserId, String MainPwd,
            String Key,
            Boolean IsHost, Boolean IsController,
            Boolean Audio, Boolean Video, Boolean Chat
            )
         {
             String url = BasePath +
                "manager.php?" +
                "method=SetParameters" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + Key
                ;
            
            if (IsHost) { url += "&host=1";  }
             else {url += "&host=0"; }

            if (IsController) { url += "&controller=1"; }
            else { url += "&controller=0"; }

            url += "&properties=";

            if (Audio) { url += "A"; }
            if (Video) { url += "V"; }
            if (Chat) { url += "T"; }
            
             return url;
         }
       
        /// <summary>
        /// Modifica i parametri dell'utente.
        /// </summary>
        /// <param name="BasePath"></param>
        /// <param name="MainUserId"></param>
        /// <param name="MainPwd"></param>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <param name="UserMail"></param>
        /// <returns></returns>
        /// <remarks>
        /// Internamente eventuali altri parametri impostabili.
        /// </remarks>
         public static string SetUserInfo(String BasePath,
            String MainUserId, String MainPwd,
            String UserId,
            String UserName,
             String UserMail)
         {

             String url = BasePath +
                "manager.php?" +
                "method=SetUserInfo" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&email=" + UserId
                ;

             if(!String.IsNullOrEmpty(UserName))
                 url += "&username=" + UserName;

             if (!String.IsNullOrEmpty(UserMail))
                 url += "&useremail=" + UserMail;

             return url;

            //Il metodo richiede in ingresso i seguenti parametri obbligatori: 
            //* <login> login di accesso amministratore al server 
            //* <password> password di accesso al server 
            //* <email> e-mail dell’utente, oppure <uid> identificativo numerico dell’utente 
            //Il metodo richiede almeno uno dei seguenti parametri facoltativi: 
 
            //* <username> nome dell'utente 
            //* <useremail> email dell'utente 
            //* <userpassword> password dell'utente (minimo 6 caratteri) 
            //* <status> stato dell'utente (0=non attivo, 1=attivo, 2=sospeso) 
            //* <expiry_date> data di scadenza dell'utente, nel formato yyyymmdd 
            //* <language> lingua di default per l'utente, a scelta tra it,en,de,fr,es 
            //* <timezone> differenza in ore dal fuso GMT 
            //* <maxroomsize> numero massimo di partecipanti per i meeting dell'utente (-1 = nessuna 
            //restrizione) 
            //* <canrecord> abilita la registrazione per l'utente (0=non abilitata, 1=abilitata) 
            //* <maxvideonum> numero massimo di video visualizzabili dall'utente (-1 = nessuna restrizione, >-1 
            //numero video) 
            //* <daysevent> attiva/disattiva la gestione a eventi (-1=disattiva, >-1 imposta i giorni disponibili) 
            //* <portalname> portale di assegnazione dell'utente, deve essere un portale già creato sulla 
            //piattaforma 
            //* <bitratelist> elenco delle bitrate da assegnare all'utente separate da |, devono essere bitrate 
            //configurate sulla piattaforma 

         }
         /// <summary>
         /// Crea una lista di parametri STANDARD, validi per un utente EWorks
         /// </summary>
         /// <returns></returns>
         private static List<String> GetAvailableUserParameters()
         {
             List<String> parameters = new List<String>();

             parameters.Add("userename"); // -> ?(string, , 200) 
             parameters.Add("useremail"); // -> ?(string, , 200) 
             parameters.Add("userpassword"); // -> ?(string, , 200)  - min 6 char
             parameters.Add("status"); // -> (integer, /^(0|1)$/),          0 = non attivo, 1 = attivo
             parameters.Add("expiry_date"); // -> (string, yyyymmdd),       scadenza utente
             parameters.Add("language"); // -> (string, it|en|de|fr|es),    lingua utente
             parameters.Add("timezone"); // -> (integer, ),                 differenza in ore dal fuso orario GTM
             parameters.Add("maxroomsize"); // -> (integer, ),              numero massimo partecipanti per i meeting dell'utente (-1 = unlimited)
             parameters.Add("canrecord"); // -> (integer, /^(0|1)$/),       abilita registrazione utente (1 = true, 0 = false)
             parameters.Add("maxvideonum"); // -> (integer, ),              numero massimo video meeting dell'utente (-1 = unlimited)
             parameters.Add("daysevent"); // -> (integer, ),                (-1=disattiva, >-1 imposta i giorni disponibili)
             parameters.Add("portalname"); // -> ?(string, , 200)           portale di assegnazione dell'utente (deve già esistere)
             parameters.Add("bitratelist"); // -> ???                       elenco delle bitrate da assegnare all'utente separate da |, devono essere bitrate configurate sulla piattaforma

             return parameters;
         }

         /// <summary>
         /// Recupera info utente
         /// </summary>
         /// <param name="BasePath">Il percorso base</param>
         /// <param name="MainUserId">ID utente x accesso al server</param>
         /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
         /// <param name="MasterKey">Chiave Master della stanza</param>
         /// <returns></returns>
         public static string GetUserInfo(
             String BasePath,
             String MainUserId, String MainPwd,
             String UserId
             )
         {
             return BasePath +
                 "manager.php?" +
                 "method=GetUserInfo" +
                 "&login=" + MainUserId +
                 "&password=" + MainPwd +
                 "&email=" + UserId
                 ;
         }

        /// <summary>
        /// Converte i parametri in ingresso per la modifica di una stanza in un elenco di stringhe di lunghezza consona
        /// </summary>
        /// <param name="Parameters">Parametri da impostare</param>
        /// <param name="IsForCreation">
        /// True: la stringa è per la creazione
        /// False: la stringa è per la modifica
        /// </param>
        /// <returns></returns>
         private static List<string> eWRoomParameterToUrls(eWRoomParameters Parameters, Boolean IsForCreation, string version = "")
         {
             List<string> OutStrs = new List<string>();
             
             String output = "";
             
            if (!String.IsNullOrEmpty(Parameters.audiocodec))
                { OutStrs.Add("&audiocodec=" + Parameters.audiocodec); }

            if (Parameters.bitrate != null && Parameters.bitrate.Count() > 0)
            {
                output = "&bitrate=";
                foreach (int br in Parameters.bitrate)
                { output += br.ToString() + "|"; }

                output.Remove(output.Length - 1, 1);
                if(!String.IsNullOrEmpty(output)) OutStrs.Add(output);
            }
             

            if(!String.IsNullOrEmpty(Parameters.description)) OutStrs.Add("&description=" + Parameters.description);
            OutStrs.Add("&language=" + Parameters.language.ToString()); 
            OutStrs.Add("&meetingduration=" + Parameters.meetingduration.ToString());

            
            output = "&meetinglog=";
            output += (Parameters.meetinglog) ? "1" : "0";
            OutStrs.Add(output);
            
            // RIVEDERE FORMATO DATA!!!


            if (Parameters.meetingstart != null) { 
                DateTime startTime = Parameters.meetingstart ?? DateTime.Now;
                OutStrs.Add("&meetingstart=" + startTime.ToString("yyyyMMddHHmmss"));
                    //+ startTime.Year + "-" + startTime.Month + "-" + startTime.Day + " " + startTime.Hour + ":" + startTime.Minute + ":" +startTime.Second); 
            } 


            if(!String.IsNullOrEmpty(Parameters.meetingtitle)) { OutStrs.Add("&meetingtitle=" + Parameters.meetingtitle); }
            if(!String.IsNullOrEmpty(Parameters.meetingpassword)) { OutStrs.Add("&meetingpassword=" + Parameters.meetingpassword); }

            if (version != "7.0")
            {
                OutStrs.Add("&part_language=" + Parameters.part_language);

                if (!String.IsNullOrEmpty(Parameters.meetingpassword))
                {
                    OutStrs.Add("&meetingpassword=" + Parameters.meetingpassword);
                }
            }

             

            if(!String.IsNullOrEmpty(Parameters.part_properties)) { OutStrs.Add("&part_properties=" + Parameters.part_properties); }
            if(!String.IsNullOrEmpty(Parameters.properties)) { OutStrs.Add("&properties=" + Parameters.properties); }

            output = "&recording=";
            output += (Parameters.recording) ? "1" : "0";
            OutStrs.Add(output);

            OutStrs.Add("&sharingtype=" + Parameters.sharingtype.ToString()); 
            OutStrs.Add("&timezone=" + Parameters.timezone.ToString());



            output = "&usedatetime=";
            output += (Parameters.usedatetime) ? "1" : "0";
            OutStrs.Add(output);

           
            if(Parameters.videoheight > 0 && Parameters.videowidth > 0)
            {
                OutStrs.Add("&videoheight=" + Parameters.videoheight.ToString());
                OutStrs.Add("&videowidth=" + Parameters.videowidth.ToString());
            }

             //Boh, cmq in SET funziona!

            output = "&framerate=";
            output += Parameters.framerate.ToString();
            OutStrs.Add(output);

            //Aggiungere i parametri mancanti e non documentati?
            /*Nuovi (da 6.0)*/
            //public string clientname { get; set; } //???
            //public string email { get; set; } //???
            //public int framerate { get; set; }

            //public bool vav { get; set; }
            //public bool svc { get; set; }
            //public int audiopayload { get; set; }
            //public string endsessionurl { get; set; }

            /*Solo in documentaizone, get*/
            //public object collaborationtype { get; set; }   //=-> SharingType?


             //Parametri che vengono impostati PER SICUREZZA in creazione, ma che poi non vengono modificati!
            if (IsForCreation)
            {
                output = "&forcehost=0";
                //output += (Parameters.forcehost) ? "1" : "0";
                OutStrs.Add(output);

                output = "&forcecontroller=0";
                //output += (Parameters.forcecontroller) ? "1" : "0";
                OutStrs.Add(output);

                //Configurare su Key
                output = "&controller=0";
                //output += (Parameters.controller) ? "1" : "0";
                OutStrs.Add(output);

                output = "&host=0";
                //output += (Parameters.host) ? "1" : "0";
                OutStrs.Add(output);

                output = "&needaccount=0";
                //output += (Parameters.needaccount) ? "1" : "0";
                OutStrs.Add(output);

                //SEGARE
                output = "&crypt=0";
                //output += (Parameters.crypt) ? "1" : "0";
                OutStrs.Add(output);

                output = "&dashboard=0";
                //output += (Parameters.dashboard) ? "1" : "0";
                OutStrs.Add(output);
            }

             //ELIMINATI:
            //if (!String.IsNullOrEmpty(Parameters.videocodec)) OutStrs.Add("&videocodec=" + Parameters.videocodec);
            //output = "&udpenabled=";
            //output += (Parameters.udpenabled) ? "1" : "0";
            //OutStrs.Add(output);


            return OutStrs;
         }

        /// <summary>
        /// Recupera le impostazioni a livello di portale (parametri disponibili per quella piattaforma)
        /// </summary>
        /// <param name="BasePath"></param>
        /// <param name="MainUserId"></param>
        /// <param name="MainPwd"></param>
        /// <param name="PortalHost"></param>
        /// <returns></returns>
        public static string GetPortalSettings(
            String BasePath,
            String MainUserId,
            String MainPwd,
            String PortalHost)
         {
             return BasePath +
                  "manager.php?" +
                  "method=GetPortalSettings" +
                  "&login=" + MainUserId +
                  "&password=" + MainPwd +
                  "&portal=" + PortalHost
                  ;
         }

        /// <summary>
        /// Recupera indirizzo accesso stanza
        /// </summary>
        /// <param name="BasePath"></param>
        /// <param name="MainUserId"></param>
        /// <param name="Key"></param>
        /// <param name="Version">Da 7.0 è rimossa la mail dall'indirizzo di accesso...</param>
        /// <returns></returns>
        public static string GetAccessUrl(
            String BasePath,
            String MainUserId,
            String Key,
            String Version)
        {

            String Url = BasePath;
            if (!Url.EndsWith("/"))
                Url += "/";
            Url += "join/";

            if(Version != "7.0")
            {
                Url += MainUserId;
                Url += "/";
            
            }
            //http://192.168.222.212:1080/join/mirco.borsato@unitn.it/vjkPeuOPRrpPRqk4wNuIZpKx

            Url += Key;

            return Url;



        }

        /// <summary>
        /// Il metodo MaxFileSize restituisce la dimensione massima in bytes
        /// dei file uploadabili con il metodo UploadFile. 
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <returns></returns>
        public static string GetMaxFileSize(
            String BasePath,
            String MainUserId, String MainPwd)
        {
            //http://testnovasvideo.trentinonetwork.it/manager.php?method=MaxFileSize&login=mirco.borsato@unitn.it&password=e3bc4fcaad999086fa3b5731d3c89c75
            return BasePath +
                "manager.php?" +
                "method=MaxFileSize" +
                "&login=" + MainUserId +
                "&password=" + MainPwd;
        }

        /// <summary>
        /// Il metodo recupera la chiave di un utente associato ad una stanza
        /// </summary>
        /// <param name="BasePath">Il percorso base</param>
        /// <param name="MainUserId">ID utente x accesso al server</param>
        /// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
        /// <param name="MasterKey">La chiave della stanza</param>
        /// <param name="UserId">La MAIL dell'utente (identificativo univoco nella stanza)</param>
        /// <returns></returns>
        public static string RetrieveKey(
            String BasePath,
            String MainUserId, String MainPwd,
            String MasterKey,
            String UserId)
        {
            //http://testnovasvideo.trentinonetwork.it/manager.php?method=MaxFileSize&login=mirco.borsato@unitn.it&password=e3bc4fcaad999086fa3b5731d3c89c75
            return BasePath +
                "manager.php?" +
                "method=RetrieveKey" +
                "&login=" + MainUserId +
                "&password=" + MainPwd +
                "&masterkey=" + MasterKey +
                "&userid=" + UserId;
        }
    }
}