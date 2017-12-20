using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Adevico.SNMPSenderConnector;
using Adevico.WebSNMTP.dto;

namespace Adevico.WebSNMTP
{
    /// <summary>
    /// Service che riceve le richieste di notifica e le gira via SNMTP
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WsSnmtp : System.Web.Services.WebService
    {

        #region Private property
        /// <summary>
        /// Classe privata LexarConnector
        /// </summary>
        private LextConnector _connector;
        
        /// <summary>
        /// Classe di accesso a LexarConnector, con relativo inizializzatore se null
        /// </summary>
        private LextConnector connector
        {
            get
            {
                if (_connector == null)
                {
                    string authPwd = "";
                    string privPwd = "";
                    string comName = "";
                    string receiverIp = "";
                    string receiverPort = "";
                    string version = "";

                    string encoding = "";

                    string objectoIdformat = "{0}.{1}";
                    string objectoIdDefault = "1";

                    string defaultoIdRoot = "";

                    try
                    {
                        authPwd = System.Configuration.ConfigurationManager.AppSettings["SNMTP.AuthPwd"];
                        privPwd = System.Configuration.ConfigurationManager.AppSettings["SNMTP.PrivPwd"];
                        comName = System.Configuration.ConfigurationManager.AppSettings["SNMTP.ComName"];

                        //senderIp = System.Configuration.ConfigurationManager.AppSettings["SNMTP.SenderIp"];
                        receiverIp = System.Configuration.ConfigurationManager.AppSettings["SNMTP.ReceiverIp"];
                        version = System.Configuration.ConfigurationManager.AppSettings["SNMTP.Version"];
                        receiverPort = System.Configuration.ConfigurationManager.AppSettings["SNMTP.ReceiverPort"];

                        

                        encoding = System.Configuration.ConfigurationManager.AppSettings["SNMTP.Encoding"];
                        defaultoIdRoot = System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.DefaultTrapId"];
                        objectoIdformat = System.Configuration.ConfigurationManager.AppSettings["SNMTP.Object.uId.Format"];
                        objectoIdDefault = System.Configuration.ConfigurationManager.AppSettings["SNMTP.Object.uId.Default"];

                    }
                    catch (Exception)
                    {

                        throw;
                    }


                    _connector = new LextConnector(authPwd, privPwd, comName, SenderIp, receiverIp, receiverPort, version, encoding); //, objectoIdformat, objectoIdDefault, defaultoIdRoot);
                }
                return _connector;
            }
        }

        private string _senderIp = "";

        /// <summary>
        /// Recupera l'IP chiamante.
        /// </summary>
        /// <remarks>
        /// Se localhost, il valore risulta essere ::1, che viene convertito in 127.0.0.1
        /// </remarks>
        private string SenderIp
        {
            get
            {
                if (String.IsNullOrEmpty(_senderIp))
                {
                    try
                    {
                        _senderIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                        if (string.IsNullOrEmpty(_senderIp))
                        {
                            _senderIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        }
                        else
                        {
                            _senderIp = _senderIp.Split(',')[0];
                        }
                    }
                    catch (Exception)
                    {
                        _senderIp = "";
                    }
                }

                if (_senderIp == "::1")
                    _senderIp = "127.0.0.1";

                return _senderIp;
            }
        }

        #endregion

        #region Public WebMethod

        ///// <summary>
        ///// Recupera il valore di una data chiave
        ///// </summary>
        ///// <param name="id">chiave</param>
        ///// <returns></returns>
        //[WebMethod]
        //public List<dtoIdValue> Get(string id)
        //{
        //    List<dtoIdValue> kvpOut = new List<dtoIdValue>();

        //    if (!CheckSender())
        //    {
        //        kvpOut.Add(new dtoIdValue { Id = "-1", Value = "No permission" });
        //        return kvpOut;
        //    }


        //    kvpOut = (from KeyValuePair<string, string> kv in connector.GetMessageKvp(id)
        //              select new dtoIdValue()
        //              {
        //                  Id = kv.Key,
        //                  Value = kv.Value
        //              }).ToList();


        //    if (String.IsNullOrEmpty(connector.ErrorString))
        //    {
        //        return kvpOut;
        //    }

        //    if (kvpOut == null)
        //    {
        //        kvpOut = new List<dtoIdValue>();
        //    }

        //    kvpOut.Add(new dtoIdValue { Id = "Error", Value = connector.ErrorString });




        //    return kvpOut;
        //}

        ///// <summary>
        ///// Imposta la chiave ID sul valore value
        ///// </summary>
        ///// <param name="value">dto tipo id, valore</param>
        //[WebMethod]
        //public void Set(dto.dtoIdValue value)
        //{
        //    if (!CheckSender())
        //        return;

        //    string result = "";
        //    bool success = connector.SetMessageStr(value.Id, value.Value, out result);
        //}

        ///// <summary>
        ///// Imposta la chiave ID sul valore value
        ///// </summary>
        ///// <param name="id">Chiave</param>
        ///// <param name="value">Valore</param>
        //[WebMethod]
        //public void SetString(String id, String value)
        //{
        //    if (!CheckSender())
        //        return;

        //    string result = "";
        //    bool success = connector.SetMessageStr(id, value, out result);
        //}

        /// <summary>
        /// Imposta la chiave ID sul valore value
        /// </summary>
        /// <param name="id">Chiave</param>
        /// <param name="value">Valore</param>
        [WebMethod]
        public void SendTrapString(String id, String value)
        {
            if (!CheckSender())
                return;

            string result = "";

            if(string.IsNullOrEmpty(id))
            {
                id = System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.DefaultTrapId"];
            }
            
            bool success = connector.SendTrapStr(id, new string[] { value });
        }

        /// <summary>
        /// Invia un trap
        /// </summary>
        /// <param name="id">
        /// Id del trap.
        /// SE non contiene "." sarà usato così com'è,
        /// altrimenti verrà utilizzato come ultimo valore di quanto in configurazione.
        /// </param>
        /// <param name="value">Action value da inviare</param>
        [WebMethod]
        public void SendTrapActionValue(String id, dtoActionValues value)
        {
            if(!id.Contains('.'))
            {
                id = string.Format(System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.DefaultTrapId"], id);
                //
            }

            string valueStr = value.GetString(
                System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.System"],
                System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.MessageFormat"],
                System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.UserData"],
                System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.ActionData"],
                System.Configuration.ConfigurationManager.AppSettings["SNMTP.LoggedInfo.DateTimeFormat"]
                );
            
            SendTrapString(id, valueStr);
        }
        
        #endregion


        #region Private Function
        /// <summary>
        /// Controlla che il chiamante sia LocalHost
        /// </summary>
        /// <returns></returns>
        private bool CheckSender()
        {
            if (SenderIp == "127.0.0.1")
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
