using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks
{
	/// <summary>
	/// Classe helper per l'accesso alle funzioni di eWorks
	/// </summary>
	public class eWAPIConnector
	{
		/// <summary>
		/// Formato utilizzato per le date
		/// </summary>
		public static String DateTimeFormat
		{
			get
			{
				//return "yyyyMMddHHmmss";
				return "yyyy-MM-dd hh:mm:ss";
			}
		}
		
		/// <summary>
		/// Crea la Master Key di un metting (in pratica una stanza)
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="Proxy">Proxy per l'accesso</param>
		/// <param name="MainUserId">ID utente x accesso al server</param>
		/// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
		/// <param name="UserId">ID utente per il quale creare una stanza</param>
		/// <param name="UserDisplayName">Nome da visualizzare per quell'utente in quella stanza</param>
		/// <param name="StartDate">Data inizio</param>
		/// <param name="Title">Titolo Stanza</param>
		/// <param name="MinutesDuration">Durata Meetign prevista in minuti (solo info)</param>
		/// <returns>Chiave esterna stanza</returns>
		public static string CreateMasterKey(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String UserId, String UserDisplayName,
			DateTime? StartDate, String Title,
			Int32 MinutesDuration)
		{

			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}

			if (MinutesDuration <= 0)
				MinutesDuration = 60;

			string downloadString = client.DownloadString(eWurlHelper.CreateMasterKey(BasePath, MainUserId, MainPwd, UserId, UserDisplayName, (StartDate ?? DateTime.Now), Title, MinutesDuration));

			if (!downloadString.StartsWith("OK"))
			{
				throw new Exception(downloadString.Remove(0, 6));
			}
			return downloadString.Remove(0, 3);
		}

		/// <summary>
		/// Crea una chiave d'accesso ad un meeting
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// /// <param name="Proxy">Proxy per l'accesso</param>
		/// <param name="MainUserId">ID utente x accesso al server</param>
		/// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
		/// <param name="MasterKey">Chiave master del meeting a cui aggiungere una nuova chiave (utente)</param>
		/// <param name="UserId">Utente da aggiungere alla stanza</param>
		/// <param name="UserDisplayName">Nome visualizzato per quell'utente</param>
		/// <returns>Chiave esterna utente</returns>
		public static string CreateKey(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey,
			String UserId, String UserDisplayName
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadString = client.DownloadString(eWurlHelper.CreateKey(BasePath, MainUserId, MainPwd, MasterKey, UserId, UserDisplayName));

			if (!downloadString.StartsWith("OK:"))
			{

				throw new Exception(downloadString.Remove(0, 6));
			}

			return downloadString.Remove(0, 3);
		}

		/// <summary>
		/// Recupera l'elenco delle Chiavi master (stanze) di un dato utente.
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="MainUserId">ID utente x accesso al server</param>
		/// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
		/// <returns></returns>
		public static List<DTO.DTOmasterKey> RetrieveMasterKeys(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string[] downloadStrings = client.DownloadString(eWurlHelper.RetrieveMasterKeys(BasePath, MainUserId, MainPwd)).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

			List<DTO.DTOmasterKey> MasterKeys = new List<DTO.DTOmasterKey>();

			if (downloadStrings != null && downloadStrings.Count() > 0)
			{
				if (downloadStrings[0].StartsWith("OK"))
				{
					bool isfirst = true;
					foreach (string str in downloadStrings)
					{
						if (isfirst)
						{
							isfirst = false;
							if (!str.StartsWith("OK"))
							{
								throw new Exception(str);
							}
						}
						else
						{
							string[] strdata = str.Split('|');
							DTO.DTOmasterKey DTOmk = new DTO.DTOmasterKey();
							try
							{
								int param = strdata.Count();

								if (param > 0) DTOmk.MasterKey = strdata[0];
								if (param > 1) DTOmk.Date = Convert.ToDateTime(strdata[1]);
								if (param > 2) DTOmk.Duration = Convert.ToInt32(strdata[2]);
								if (param > 3) DTOmk.Title = strdata[3];

							}
							catch { }

							if (!string.IsNullOrEmpty(DTOmk.MasterKey)) MasterKeys.Add(DTOmk);
						}
					}
				}
			}

			return MasterKeys;
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
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey, String Key
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadString = client.DownloadString(eWurlHelper.DeleteKey(BasePath, MainUserId, MainPwd, MasterKey, Key));

			if (!downloadString.StartsWith("OK:"))
			{
				throw new Exception(downloadString.Remove(0, 6));
			}

			//Se tutto ok, ritorna la chiave cancellata.
			return downloadString.Remove(0, 3);
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
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey, String Key
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadString = client.DownloadString(eWurlHelper.DisableKey(BasePath, MainUserId, MainPwd, MasterKey, Key));

			if (!downloadString.StartsWith("OK:"))
			{
				throw new Exception(downloadString.Remove(0, 6));
			}

			//Se tutto ok, ritorna la chiave disabilitata.

			return downloadString.Remove(0, 3);
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
		public static string EnableKey(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey, String Key
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadString = client.DownloadString(eWurlHelper.EnableKey(BasePath, MainUserId, MainPwd, MasterKey, Key));

			if (!downloadString.StartsWith("OK:"))
			{
				throw new Exception(downloadString.Remove(0, 6));
			}

			//Se tutto ok, ritorna la chiave disabilitata.

			return downloadString.Remove(0, 3);
		}

		/// <summary>
		/// Recupera l'elenco di utenti presenti in un data stanza
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="MainUserId">ID utente che può creare stanze</param>
		/// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
		/// <param name="MasterKey">Chiave Master della stanza</param>
		/// <returns></returns>
		public static List<DTO.DTOuser> RetrieveUsers(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string[] downloadStrings = client.DownloadString(eWurlHelper.RetrieveUsers(BasePath, MainUserId, MainPwd, MasterKey)).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

			List<DTO.DTOuser> Users = new List<DTO.DTOuser>();

			if (downloadStrings != null && downloadStrings.Count() > 0)
			{
				if (downloadStrings[0].StartsWith("OK"))
				{
					bool isfirst = true;
					foreach (string str in downloadStrings)
					{
						if (isfirst)
						{
							isfirst = false;
							if (!str.StartsWith("OK"))
							{
								throw new Exception(str);
							}
						}
						else
						{
							string[] strdata = str.Split('|');
							DTO.DTOuser DTOusr = new DTO.DTOuser();
							try
							{
								int param = strdata.Count();

								if (param > 0) DTOusr.UserId = strdata[0];
								if (param > 1) DTOusr.UserName = strdata[1];
								if (param > 2) DTOusr.Key = strdata[2];
								DTOusr.IsMaster = (DTOusr.Key == MasterKey) ? true : false;
							}
							catch { }

							if (!string.IsNullOrEmpty(DTOusr.Key)) Users.Add(DTOusr);
						}
					}
				}
			}

			return Users;
		}


		/// <summary>
		/// Recupera il report di un dato meeting
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="MainUserId">ID utente che può creare stanze</param>
		/// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
		/// <param name="Key">Una chiave valida per il meeting</param>
		/// <returns></returns>
		public static List<DTO.DTOreport> GetMeetingReport(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String Key
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string[] downloadStrings = client.DownloadString(eWurlHelper.GetMeetingReport(BasePath, MainUserId, MainPwd, Key)).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

			List<DTO.DTOreport> MeetReports = new List<DTO.DTOreport>();

			if (downloadStrings != null && downloadStrings.Count() > 0)
			{
				if (downloadStrings[0].StartsWith("OK"))
				{
					bool isfirst = true;
					foreach (string str in downloadStrings)
					{
						if (isfirst)
						{
							isfirst = false;
							if (!str.StartsWith("OK"))
							{
								throw new Exception(str);
							}
						}
						else
						{
							string[] strdata = str.Split(',');
							DTO.DTOreport DTOrep = new DTO.DTOreport();
							Boolean IsFirst = true;
							try
							{
								int param = strdata.Count();

								if (param > 1) DTOrep.UserId = strdata[0];
								if (param > 2) DTOrep.UserName = strdata[1];
								if (param > 3) DTOrep.StartDate = Convert.ToDateTime(strdata[2]);
								try { if (param > 4) DTOrep.Duration = Convert.ToInt32(strdata[3]); }
								catch { }
								try { if (param > 5) DTOrep.UserPercentage = Convert.ToDecimal(strdata[4].Replace("%", "")) / 100; }
								catch { }
								if (param > 6) DTOrep.UserName = strdata[5];
								try { if (param > 7) DTOrep.Access = Convert.ToInt32(strdata[6]); }
								catch { }
								try { if (param > 8) DTOrep.TotalTransimittedData = Convert.ToInt32(strdata[7]); }
								catch { }
								try { if (param > 9) DTOrep.TotalReceivedData = Convert.ToInt32(strdata[8]); }
								catch { }
								DTOrep.IsHost = IsFirst;
								IsFirst = false;

								if (!string.IsNullOrEmpty(DTOrep.UserId)) MeetReports.Add(DTOrep);
							}
							catch { }

						}
					}
				}
			}

			return MeetReports;
		}

		/// <summary>
		/// Recupera impostazioni stanza
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="Proxy">Proxy per l'accesso</param>
		/// <param name="MainUserId">ID utente x accesso al server</param>
		/// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
		/// <param name="MasterKey">Identificativo esterno stanza</param>
		/// <returns></returns>
		public static eWRoomParameters GetRoomParameters(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey
			)
		{
			eWRoomParameters Param = new eWRoomParameters();

			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string[] downloadStrings = client.DownloadString(eWurlHelper.GetParameters(BasePath, MainUserId, MainPwd, MasterKey)).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

			if (downloadStrings != null && downloadStrings.Count() > 0)
			{
				if (downloadStrings[0].StartsWith("OK"))
				{
					bool isfirst = true;
					foreach (string str in downloadStrings)
					{
						if (!String.IsNullOrEmpty(str))
						{
							if (isfirst)
							{
								isfirst = false;
								if (!str.StartsWith("OK"))
								{
									throw new Exception(str.Remove(0, 6));
								}
							}
							else
							{

								string[] strdata = str.Split('=');
								int items = strdata.Count();

								tmpParameter prm = new tmpParameter();
								if (items > 0) prm.key = strdata[0];
								if (items > 1) prm.value = strdata[1];

								switch (prm.key)
								{
									case "audiocodec": // -> (string, , 100) 
										Param.audiocodec = prm.value;
										break;
									case "bitrate": // -> (integer, elenco bitrate valide per l'utente separato da "|") 
										Param.bitrate = new List<int>();
										string[] btrs = prm.value.Split('|');
										foreach (string btr in btrs)
										{
											try
											{
												Param.bitrate.Add(System.Convert.ToInt32(btr));
											}
											catch { }
										}
										break;
									//case "crypt": // -> (integer, /^(0|1)$/)
									//    Param.crypt = false;
									//    if (prm.value == "1") Param.crypt = true;
									//    break;
									//case "dashboard": // -> (integer, /^(0|1)$/) 
									//    Param.dashboard = false;
									//    if (prm.value == "1") Param.dashboard = true;
									//    break;
									case "description": // -> (string) 
										Param.description = prm.value;
										break;
									case "language": // -> (string, (it|en|de|fr|es), 2) 
										if (!String.IsNullOrEmpty(prm.value))
											Param.language = (eWLanguages)Enum.Parse(typeof(eWLanguages), prm.value, true);
										break;
									case "meetingduration": // -> (integer, /[0-9]/) 
										try
										{
											Param.meetingduration = System.Convert.ToInt16(prm.value);
										}
										catch { }
										break;
									case "meetinglog": // -> (integer, /^(0|1)$/), attiva il log esteso di un meeting 
										Param.meetinglog = false;
										if (prm.value == "1") Param.meetinglog = true;
										break;
									case "meetingstart": // -> (string, /^[0-9]{14}$/)  yyyyMMddHHmmss
										DateTime dt;
										DateTime.TryParseExact(prm.value, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
										Param.meetingstart = dt;
										break;
									case "meetingtitle": // -> (string, , 200) 
										Param.meetingtitle = prm.value;
										break;
									case "meetingpassword": // -> (string, , 50) 
										Param.meetingpassword = prm.value;
										break;
									case "part_language": // -> (string, (it|en|de|fr|es), 2) 
										if (!String.IsNullOrEmpty(prm.value))
											Param.part_language = (eWLanguages)Enum.Parse(typeof(eWLanguages), prm.value, true);
										break;
									case "part_properties": // -> (string, /^(A|V|T){0, 3}$/) 
										Param.part_properties = prm.value;
										break;
									case "properties": // -> (string, /^(A|V|T){0, 3}$/) 
										Param.properties = prm.value;
										break;
									case "recording": // -> (integer, /^(0|1)$/) 
										Param.recording = false;
										if (prm.value == "1") Param.recording = true;
										break;
									case "sharingtype": // -> (string, /^(none|whiteboard|screen|browser|dicom)$/) 
										if (!String.IsNullOrEmpty(prm.value))
											Param.sharingtype = (eWsharingType)Enum.Parse(typeof(eWsharingType), prm.value, true);
										break;
									case "timezone": // -> (integer) 
										try
										{
											Param.timezone = System.Convert.ToInt32(prm.value);
										}
										catch { }

										break;
									//case "udpenabled": // -> (integer, /^(0|1)$/) 
									//    Param.udpenabled = false;
									//    if (prm.value == "1") Param.udpenabled = true;
									//    break;
									case "usedatetime": // -> (integer, /^(0|1)$/) 
										Param.usedatetime = false;
										if (prm.value == "1") Param.usedatetime = true;
										break;
									case "videocodec": // -> (string, , 100) 
										Param.videocodec = prm.value;
										break;
									case "videoheight": // -> (integer) 
										try
										{
											Param.videoheight = System.Convert.ToInt32(prm.value);
										}
										catch { }
										break;
									case "videowidth": // -> (integer) 
										try
										{
											Param.videowidth = System.Convert.ToInt32(prm.value);
										}
										catch { }
										break;
									case "framerate": // -> (integer) 
										try
										{
											Param.framerate = System.Convert.ToInt32(prm.value);
										}
										catch { }
										break;
									//case "needaccount": // -> (integer, /^(0|1)$/)
									//    Param.needaccount = false;
									//    if (prm.value == "1") Param.needaccount = true;
									//    break;
									//case "forcehost": // -> (integer, /^(0|1)$/)
									//    Param.forcehost = false;
									//    if (prm.value == "1") Param.forcehost = true;
									//    break;
									//case "forcecontroller": // -> (integer, /^(0|1)$/)
									//    Param.forcecontroller = false;
									//    if (prm.value == "1") Param.forcecontroller = true;
									//    break;
									//case "controller": // -> (integer, /^(0|1)$/)
									//    Param.controller = false;
									//    if (prm.value == "1") Param.controller = true;
									//    break;
									//case "host": //-> (integer, /^(0|1)$/)
									//    Param.host = false;
									//    if (prm.value == "1") Param.host = true;
									//    break;
								}

							}
						}
					}
				}
			}

			return Param;
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
		public static void SetParameters(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey,
			eWRoomParameters Parameters,
			int MaxUrlChar, Boolean IsCreation,
			String Version
			)
		{

			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}

			string Errors = "";
			foreach (String url in eWurlHelper.SetParameters(BasePath, MainUserId, MainPwd, MasterKey, Parameters, MaxUrlChar, IsCreation, Version))
			{
				string downloadStrings = client.DownloadString(url);
				if (!downloadStrings.StartsWith("OK"))
				{
					Errors += " \r\n " + url + " \r\n ";
					Errors += downloadStrings.Remove(0, 6) + " \r\n ";
				}
			}

			if (!String.IsNullOrEmpty(Errors)) throw new Exception(Errors);

		}


		public static void SetName(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey,
			String Name,
			int MaxUrlChar, 
			String Version
			)
		{

			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}

			string Errors = "";
			foreach (String url in eWurlHelper.SetName(BasePath, MainUserId, MainPwd, MasterKey, Name, MaxUrlChar, Version))
			{
				string downloadStrings = client.DownloadString(url);
				if (!downloadStrings.StartsWith("OK"))
				{
					Errors += " \r\n " + url + " \r\n ";
					Errors += downloadStrings.Remove(0, 6) + " \r\n ";
				}
			}

			if (!String.IsNullOrEmpty(Errors)) throw new Exception(Errors);

		}

		/// <summary>
		/// Recupera i parametri di una conferenza.
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="MainUserId">ID utente che può creare stanze</param>
		/// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
		/// <param name="MasterKey">Chiave Master della stanza</param>
		/// <param name="Parameters">Lista chiave valore dei parametri</param>
		/// <returns></returns>
		/// <remarks>
		/// DA FARE!!!
		/// La versione DEMO restituisce "ERROR:No log data."
		/// anzichè un array json così strutturato: 
		///{
		///"meetingkey":{
		///participant:"nome del partecipante",
		///key:"chiave di accesso"
		///duration:"tempo di partecipazione in secondi",
		///audiocount:"numero di attivazioni dell'audio per l'utente",
		///audiotime:"durata in secondi dell'attivazione dell'audio per l'utente",
		///audiosessions:[
		///{ 
		///audiostart:"data e ora di inizio sessione audio (yyyy-mm-dd hh:mm:ss)",
		///audiostop:"data e ora di fine sessione audio (yyyy-mm-dd hh:mm:ss)",
		///},
		///...
		///]
		///filescount:"numero dei file condivisi dall'utente",
		///chatcount:"numero dei messaggi di chat scritti"
		///},
		///...
		///}
		/// </remarks>
		public static string GetMeetingDetails(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey
			)
		{
			WebClient client = new WebClient();
			return client.DownloadString(eWurlHelper.GetMeetingDetails(BasePath, MainUserId, MainPwd, MasterKey));
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
		public static DTO.DTOKeyInfo GetKeyInfo(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String Key
			)
		{

			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string[] downloadStrings = client.DownloadString(eWurlHelper.GetKeyInfo(BasePath, MainUserId, MainPwd, Key)).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

			DTO.DTOKeyInfo KInfo = new DTO.DTOKeyInfo();

			if (downloadStrings != null && downloadStrings.Count() > 0)
			{
				foreach (string str in downloadStrings)
				{
					if (str.StartsWith("OK"))
					{ }
					else if (str.StartsWith("ERROR")) throw new Exception(str.Remove(0, 6));
					else if (str.StartsWith("email")) KInfo.Email = str.Replace("email=", "");
					else if (str.StartsWith("clientname")) KInfo.ClientName = str.Replace("clientname=", "");
					else if (str.StartsWith("ishost"))
					{
						if (str.EndsWith("0")) { KInfo.IsHost = false; }
						else { KInfo.IsHost = true; } //0 = no host, altrimenti incrementale sul numero di Host...
					}
					else if (str.StartsWith("meetingid")) KInfo.MeetingId = str.Replace("meetingid=", "");
					else if (str.StartsWith("portalname")) KInfo.PortalName = str.Replace("portalname=", "");
					else if (str.StartsWith("hostemail")) KInfo.HosteMail = str.Replace("hostemail=", "");
					else if (str.StartsWith("hostclientname")) KInfo.HostClientName = str.Replace("hostclientname=", "");
				}
			}

			return KInfo;
		}

		/// <summary>
		/// Recupera elenco stanze:
		/// TUTTE le stanze create dall'unico utente, quindi TUTTE quelle presenti in piattaforma
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="Proxy">Proxy accesso</param>
		/// <param name="MainUserId">ID utente che può creare stanze</param>
		/// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
		/// <param name="UserId">Chiave esterna utente</param>
		/// <returns></returns>
		public static List<DTO.DTOMeeting> GetMeetingList(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String UserId
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadStrings = client.DownloadString(eWurlHelper.GetMeetingList(BasePath, MainUserId, MainPwd, UserId)); 

			List<DTO.DTOMeeting> Results = new List<DTO.DTOMeeting>();
			if (downloadStrings.StartsWith("OK"))
			{
				string[] outstr = (downloadStrings.Replace("OK:\r\n", "")
					.Replace("[", "")
					.Replace("]", "")
					.Replace("{", "")
					.Replace("},", "|")
					.Replace("}", "")
					).Split('|');

				foreach (string str in outstr)
				{
					DTO.DTOMeeting Meeting = new DTO.DTOMeeting();

					string[] param = (str.Replace("\"", "")).Split(',');

					foreach (string prm in param)
					{


						string[] vk = prm.Split(':');

						switch (vk[0])
						{
							case "meetingid":
								Meeting.MeetingId = vk[1];
								break;
							case "masterkey":
								Meeting.MasterKey = vk[1];
								break;
							case "subject":
								Meeting.Subject = vk[1];
								break;
							case "date":
								Meeting.StartDate = Convert.ToDateTime(vk[1] + ":" + vk[2] + ":" + vk[3]);
								break;
							case "duration":
								Meeting.DurationSEC = Convert.ToInt32(vk[1]);
								break;
							case "host":
								Meeting.Host = vk[1];
								break;
							case "showcase":
								Meeting.ShowCase = (vk[1] == "1") ? false : true;
								break;
							case "participantscount":
								Meeting.PartecipantCount = Convert.ToInt32(vk[1]);
								break;
						}
					}
					Results.Add(Meeting);
				}
			}
			else
			{
				throw new Exception(downloadStrings.Remove(0, 6));
			}
			return Results;
		}

		/// <summary>
		/// Recupera info sull'utente
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="Proxy">Proxy accesso</param>
		/// <param name="MainUserId">ID utente x accesso al server</param>
		/// <param name="MainPwd">PWD (MD5) dell'utente x accesso al server</param>
		/// <param name="UserId">Mail utente di cui recuperare le info</param>
		/// <returns></returns>
		public static DTO.DTOUserInfo GetUserInfo(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String UserId
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string[] downloadStrings = client.DownloadString(eWurlHelper.GetUserInfo(BasePath, MainUserId, MainPwd, UserId)).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

			DTO.DTOUserInfo UInfo = new DTO.DTOUserInfo();


			if (downloadStrings != null && downloadStrings.Count() > 0)
			{
				if (downloadStrings[0].StartsWith("OK"))
				{
					bool isfirst = true;
					foreach (string str in downloadStrings)
					{
						if (!String.IsNullOrEmpty(str))
						{
							if (isfirst)
							{
								isfirst = false;
								if (!str.StartsWith("OK"))
								{
									throw new Exception(str.Remove(0, 6));
								}
							}
							else
							{
								string[] strdata = str.Split('=');
								int items = strdata.Count();

								String Key = "";
								String Value = "";

								if (items > 0) Key = strdata[0];
								if (items > 1) Value = strdata[1];

								switch (Key)
								{
									case "useremail":
										{
											UInfo.usermail = Value;
											break;
										}
									case "username":
										{
											UInfo.username = Value;
											break;
										}
									case "userpassword":
										{
											UInfo.userpassword = Value;
											break;
										}
									case "status":
										{
											try
											{
												UInfo.status = System.Convert.ToInt16(Value);
											}
											catch { }

											break;
										}
									case "creation_date":
										{
											DateTime dt;
											DateTime.TryParseExact(Value, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
											UInfo.creation_date = dt;
											break;
										}
									case "expiry_date":
										{
											DateTime dt;
											DateTime.TryParseExact(Value, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);
											UInfo.expiry_date = dt;
											break;
										}
									case "language":
										{
											if (!String.IsNullOrEmpty(Value))
												UInfo.language = (eWLanguages)Enum.Parse(typeof(eWLanguages), Value, true);
											break;
										}
									case "timezone":
										{
											try
											{
												UInfo.timezone = System.Convert.ToInt32(Value);
											}
											catch { }
											break;
										}
									case "maxroomsize":
										{
											try
											{
												UInfo.maxroomsize = System.Convert.ToInt32(Value);
											}
											catch { }
											break;
										}
									case "canrecord":
										{
											UInfo.canrecord = false;
											if (Value == "1") UInfo.canrecord = true;
											break;
										}
									case "daysevent":
										{
											try
											{
												UInfo.daysevent = System.Convert.ToInt32(Value);
											}
											catch { }

											break;
										}
									case "portalname":
										{
											UInfo.portalname = Value;
											break;
										}
									case "bitratelist":
										{
											UInfo.bitratelist = new List<int>();

											try
											{
												foreach (string br in Value.Split(','))
												{
													UInfo.bitratelist.Add(System.Convert.ToInt32(br));
												}

											}
											catch { }
											break;
										}
									case "videoformatlist":
										{
											UInfo.videoformatlist = new List<DTO.DTOVideoFormat>();
											try
											{
												String[] VFs = Value.Split(',');
												foreach (string vf in VFs)
												{
													String[] WH = vf.Split('x');
													try
													{
														DTO.DTOVideoFormat dVF = new DTO.DTOVideoFormat();
														dVF.width = System.Convert.ToInt32(WH[0]);
														dVF.height = System.Convert.ToInt32(WH[1]);
														dVF.name = dVF.width.ToString() + "x" + dVF.height.ToString();
														UInfo.videoformatlist.Add(dVF);
													}
													catch
													{ }
												}
											}
											catch { }
											break;
										}
								}
							}
						}
					}
				}
			}
			else
			{
				UInfo = null;
			}

			return UInfo;
		}

		/// <summary>
		/// Recupera i parametri disponibili per una stanza (bitrate, videocodec, etc...)
		/// </summary>
		/// <param name="BasePath"></param>
		/// <param name="Proxy"></param>
		/// <param name="MainUserId"></param>
		/// <param name="MainPwd"></param>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public static DTO.DTOAvailableParameters getAvailableParameters(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String UserId
			)
		{
			DTO.DTOAvailableParameters Param = new DTO.DTOAvailableParameters();

			DTO.DTOUserInfo UI = eWAPIConnector.GetUserInfo(
				BasePath,
				Proxy,
				MainUserId,
				MainPwd,
				UserId);

			Param.Bitrate = UI.bitratelist;

			Param.Framerates = new List<int>();

			for (int i = 1; i < 7; i++)
			{
				Param.Framerates.Add(i * 5);
			}


			Param.Languages = new List<eWLanguages>();
			Param.Languages.Add(eWLanguages.it);
			Param.Languages.Add(eWLanguages.de);
			Param.Languages.Add(eWLanguages.en);
			Param.Languages.Add(eWLanguages.es);
			Param.Languages.Add(eWLanguages.fr);


			Param.SharingTypes = new List<eWsharingType>();
			Param.SharingTypes.Add(eWsharingType.none);
			Param.SharingTypes.Add(eWsharingType.screen);
			Param.SharingTypes.Add(eWsharingType.whiteboard);
			Param.SharingTypes.Add(eWsharingType.browser);
			Param.SharingTypes.Add(eWsharingType.dicom);

			Param.VideoCodec = new List<string>();

			Param.AudioCodec = new List<string>();

			Param.VideoSizes = UI.videoformatlist;

			return Param;
		}

		/// <summary>
		/// Recupera i parametri standard ottimizzati per un dato tipo di stanza
		/// </summary>
		/// <param name="Type"></param>
		/// <returns></returns>
		public static WbRoomParameter GetParameterByType(RoomType Type)
		{
			eWorks.eWRoomParameters param = new eWRoomParameters();

			switch (Type)
			{
				case RoomType.VideoChat:
					param.bitrate = new List<int>();
					param.bitrate.Add(256);
					param.framerate = 15;
					param.language = eWLanguages.it;
					param.meetingduration = 0;
					param.meetinglog = false;
					param.meetingstart = DateTime.Now;
					param.meetingtitle = "";
					param.part_language = eWLanguages.it;
					param.part_properties = "AVT";
					param.properties = "AVT";
					param.recording = false;
					param.sharingtype = eWsharingType.none;
					param.svc = false;
					param.timezone = +1;
					param.usedatetime = false;
					param.vav = false;
					param.videowidth = 640;
					param.videoheight = 480;
					break;
				case RoomType.Meeting:
					param.bitrate = new List<int>();
					param.bitrate.Add(192);
					param.framerate = 15;
					param.language = eWLanguages.it;
					param.meetingduration = 120;
					param.meetinglog = false;
					param.meetingstart = DateTime.Now;
					param.meetingtitle = "";
					param.part_language = eWLanguages.it;
					param.part_properties = "AVT";
					param.properties = "AVT";
					param.recording = true;
					param.sharingtype = eWsharingType.none;
					param.svc = true;
					param.timezone = +1;
					param.usedatetime = true;
					param.vav = true;
					param.videowidth = 640;
					param.videoheight = 360;

					break;
				case RoomType.Lesson:
					param.bitrate = new List<int>();
					param.bitrate.Add(256);
					param.framerate = 15;
					param.language = eWLanguages.it;
					param.meetingduration = 120;
					param.meetinglog = true;
					param.meetingstart = DateTime.Now;
					param.meetingtitle = "";
					param.part_language = eWLanguages.it;
					param.part_properties = "";
					param.properties = "";
					param.recording = false;
					param.sharingtype = eWsharingType.none;
					param.svc = true;
					param.timezone = +1;
					param.usedatetime = true;
					param.vav = false;
					param.videoheight = 640;
					param.videowidth = 480;
					break;
				case RoomType.Conference:
					param.bitrate = new List<int>();
					param.bitrate.Add(128);
					param.framerate = 15;
					param.language = eWLanguages.it;
					param.meetingduration = 120;
					param.meetinglog = false;
					param.meetingstart = DateTime.Now;
					param.meetingtitle = "";
					param.part_language = eWLanguages.it;
					param.part_properties = "";
					param.properties = "";
					param.recording = false;
					param.sharingtype = eWsharingType.none;
					param.svc = true;
					param.timezone = +1;
					param.usedatetime = true;
					param.vav = true;
					param.videoheight = 176;
					param.videowidth = 144;
					break;

				default:    //RoomType.Advance
					param.bitrate = new List<int>();
					param.bitrate.Add(128);
					param.framerate = 15;
					param.language = eWLanguages.it;
					param.meetingduration = 60;
					param.meetinglog = false;
					param.meetingstart = DateTime.Now;
					param.meetingtitle = "";
					param.part_language = eWLanguages.it;
					param.part_properties = "AVT";
					param.properties = "AVT";
					param.recording = false;
					param.sharingtype = eWsharingType.none;
					param.svc = true;
					param.timezone = +1;
					param.usedatetime = true;
					param.vav = false;
					param.videoheight = 176;
					param.videowidth = 144;
					break;
			}

			return param;
		}

		/// <summary>
		/// Imposta parametri utente
		/// </summary>
		/// <param name="BasePath"></param>
		/// <param name="Proxy"></param>
		/// <param name="MainUserId"></param>
		/// <param name="MainPwd"></param>
		/// <param name="Key"></param>
		/// <param name="IsHost"></param>
		/// <param name="IsController"></param>
		/// <param name="Audio"></param>
		/// <param name="Video"></param>
		/// <param name="Chat"></param>
		public static void SetUserParameter(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String Key,
			Boolean IsHost, Boolean IsController,
			Boolean Audio, Boolean Video, Boolean Chat
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadStrings = client.DownloadString(eWurlHelper.SetUserParameter(BasePath, MainUserId, MainPwd, Key, IsHost, IsController, Audio, Video, Chat));

			if (!downloadStrings.StartsWith("OK:"))
			{
				throw new Exception(downloadStrings.Remove(0, 6));
			}
		}

		public static void SetUserInfo(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String UserId,
			String UserName,
			String UserMail
			)
		{

			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadStrings = client.DownloadString(eWurlHelper.SetUserInfo(BasePath, MainUserId, MainPwd, UserId, UserName, UserMail));

			if (!downloadStrings.StartsWith("OK:"))
			{
				throw new Exception(downloadStrings.Remove(0, 6));
			}
		}

		/// <summary>
		/// Il metodo MaxFileSize restituisce la dimensione massima in bytes
		/// dei file uploadabili con il metodo UploadFile. 
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="MainUserId">ID utente che può creare stanze</param>
		/// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
		/// <param name="Proxy"></param>
		/// <returns>
		/// -1: Size lesser than 0
		/// -2: invalid string
		/// -9: invalid answer
		/// </returns>
		public static Int32 GetMaxFileSize(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd)
		{
			Int32 Size = 0;

			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadStrings = "";

			try
			{
				downloadStrings = client.DownloadString(eWurlHelper.GetMaxFileSize(BasePath, MainUserId, MainPwd));
			}
			catch
			{ return -9; }
			

			if (downloadStrings.StartsWith("OK:"))
			{
				downloadStrings = downloadStrings.Remove(0, 3);
				
				try
				{
					Size = System.Convert.ToInt32(downloadStrings);
					if (Size < 0)
					{
						return -1;
					}
				}
				catch
				{
					return -2;
				}
			}
			return Size;
		}

		/// <summary>
		/// Restituisce la chiave associata ad un utente in una stanza.
		/// </summary>
		/// <param name="BasePath">Il percorso base</param>
		/// <param name="MainUserId">ID utente che può creare stanze</param>
		/// <param name="MainPwd">PWD (MD5) dell'user che può creare stanze</param>
		/// <param name="Proxy">I parametri sel proxy</param>
		/// <param name="MasterKey">La chiave della stanza</param>
		/// <param name="UserId">La MAIL dell'utente (identificativo univoco nella stanza)</param>
		/// <returns>
		/// La chiave se esiste,
		/// Stringa vuota se non esiste
		/// </returns>
		public static String RetrieveKey(
			String BasePath, String Proxy,
			String MainUserId, String MainPwd,
			String MasterKey,
			String UserId
			)
		{
			WebClient client = new WebClient();
			if (!String.IsNullOrEmpty(Proxy))
			{
				WebProxy wp = new WebProxy(Proxy);
				client.Proxy = wp;
			}
			string downloadStrings = "";

			try
			{
				downloadStrings = client.DownloadString(eWurlHelper.RetrieveKey(BasePath, MainUserId, MainPwd, MasterKey, UserId));
			}
			catch
			{ return ""; }

			if (downloadStrings.StartsWith("OK:"))
			{
				downloadStrings = downloadStrings.Remove(0, 3);
			}
			else
			{
				return "";
			}

			return downloadStrings;
		}
	}
}