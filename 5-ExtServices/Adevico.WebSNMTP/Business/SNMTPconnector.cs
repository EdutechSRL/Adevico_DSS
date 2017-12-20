using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Management;
using Adevico.WebSNMTP.dto;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;


namespace Adevico.WebSNMTP.Business
{
    /// <summary>
    /// Permette di impostare e leggere singole chiavi via SNMTP.
    /// 
    /// Per maggiori informaizoni:
    /// SNMP documentation:
    /// http://www.faqs.org/rfcs/rfc1157.html
    /// 
    /// Servizio SNMP – Windows:
    /// http://www.poorperformance.com/wiki/Install,_Enable_and_Configure_SNMP_on_Windows
    /// 
    /// .NET #SNMP library documentation: 
    /// https://docs.sharpsnmp.com/en/latest/tutorials/v3-operations.html
    /// 
    /// NOTA:
    /// in caso di successo errore, verificare la stringa ErrorString per maggiori dettagli.
    /// </summary>
    /// <remarks>
    /// Il connettore è stato testato sul protocollo V2.
    /// Il funzionamento della versione V1 è simile alla V2.
    /// Al contrario il protocollo V3 non è stato testato,
    /// in quanto in locale non è disponibile.
    /// </remarks>
    
    
    public class SNMTPconnector
    {
        #region internal parameters
        
        // Per i dettagli, vedi costruttore

        private SHA1AuthenticationProvider Auth { get; set; }
        private DESPrivacyProvider Priv { get; set; }
        private OctetString UName { get; set; }

        private IPAddress IpRequestSender { get; set; }
        private IPAddress IpRequestManager { get; set; }

        private VersionCode VersionCode { get; set; }

        private Encoding Encoding { get; set; }

        private string DefaultoIdRoot { get; set; }
        private string ObjectoIdFormat { get; set; }
        private string ObjectoIdDefault { get; set; }

        /// <summary>
        /// Stringa di errore.
        /// Se vuota, l'operazione è andata a buon fine.
        /// Altrimenti contiene l'eccezione.
        /// </summary>
        public string ErrorString { get; set; }



        private int IpRequestPORT { get; set; }
        #endregion

        #region costruttori

        /// <summary>
        /// Crea un nuovo "connettore"
        /// </summary>
        /// <param name="authPwd">non testato: authority password </param>
        /// <param name="privPwd">non testato: </param>
        /// <param name="userName">Nome COMMUNITY (es: public)</param>
        /// <param name="senderIp">IP del mittente (localhost) ToDo: parametrizzare la funziona di check per l'invio da IP specifici!</param>
        /// <param name="receiverIp">IP a cui inviare il messaggio</param>
        /// <param name="receiverPort">Porta a cui inviare il messaggio</param>
        /// <param name="version">Versione: V1, V2, V3</param>
        public SNMTPconnector(
            string authPwd,
            string privPwd,
            string userName,
            string senderIp,
            string receiverIp,
            string receiverPort,
            string version,
            string encoding,
            string objectoIdFormat,
            string objectoIdDefault,
            string defaultoIdRoot)
        {
            switch (encoding)
            {
                case "UTF7":
                    Encoding = Encoding.UTF7;
                    break;
                case "BigEndianUnicode":
                    Encoding = Encoding.BigEndianUnicode;
                    break;
                case "Unicode":
                    Encoding = Encoding.Unicode;
                    break;
                case "ASCII":
                    Encoding = Encoding.ASCII;
                    break;
                case "UTF8":
                    Encoding = Encoding.UTF8;
                    break;
                case "UTF32":
                    Encoding = Encoding.UTF32;
                    break;
                default:
                    Encoding = Encoding.Default;
                    break;
            }

            ObjectoIdFormat = objectoIdFormat;
            ObjectoIdDefault = objectoIdDefault;
            DefaultoIdRoot = defaultoIdRoot;

            try
            {
                IpRequestPORT = System.Convert.ToInt32(receiverPort);
            }
            catch (Exception)
            {
                IpRequestPORT = 161;
            }


            ErrorString = "";

            Auth = new SHA1AuthenticationProvider(new OctetString(authPwd, Encoding));
            Priv = new DESPrivacyProvider(new OctetString(privPwd, Encoding), Auth);
            UName = new OctetString(userName, Encoding);

            try
            {
                IpRequestSender = IPAddress.Parse(senderIp);
            }
            catch (Exception ex)
            {
                ErrorString = String.Format("{0}\r\n{1}", ErrorString, ex.ToString());
            }

            try
            {
                IpRequestManager = IPAddress.Parse(receiverIp);
            }
            catch (Exception ex)
            {
                ErrorString = String.Format("{0}\r\n{1}", ErrorString, ex.ToString());
            }

            switch (version)
            {
                case "V1":
                    VersionCode = VersionCode.V1;
                    break;
                case "V2":
                    VersionCode = VersionCode.V2;
                    break;
                case "V3":
                    VersionCode = VersionCode.V3;
                    break;
            }

        }
        #endregion

        #region OLD - SNMP info get/set

        /// <summary>
        /// Data una chiave, ne recupera il valore.
        /// </summary>
        /// <param name="objectId">Id chiave</param>
        /// <returns>
        /// Lista di oggetti chiave valore.
        /// In realtà uno solo,
        /// ma tecnicamente dovrebbe essere possibile recuperare un intero "gruppo",
        /// dato in Id di inizo ed un Id di fine.
        /// </returns>
        public IList<KeyValuePair<string, string>> GetMessageKvp(string objectId)
        {
            IList<KeyValuePair<string, string>> kvpOut = new List<KeyValuePair<string, string>>();

            return kvpOut;


            string output = "";

            switch (VersionCode)
            {
                case VersionCode.V3:
                    {
                        ISnmpMessage reply = GetMessageV3(objectId);

                        string Response = "";

                        if (reply == null)
                        {
                            ErrorString = String.Format("{0}\r\nNo response on {1}.",
                                ErrorString, IpRequestManager);
                            return kvpOut;
                        }

                        if (reply.Pdu().ErrorStatus.ToInt32() != 0) // != ErrorCode.NoError
                        {
                            ErrorString = String.Format("{0}\r\nResponse error on {1}:\r\n{2}",
                                ErrorString, IpRequestManager, reply.Parameters.ToString());

                        }

                        output = reply.Parameters.ToString();
                        break;
                    }
                default:
                    {
                        try
                        {
                            var result = Messenger.Get(VersionCode,
                                new IPEndPoint(IpRequestManager, IpRequestPORT),
                                UName,
                                new List<Variable> { new Variable(new ObjectIdentifier(objectId)) },
                                15000); //"1.3.6.1.2.1.1.1.0"

                            foreach (Variable var in result)
                            {
                                //output = string.Format("{0}\r\n{1}:{2}", output, var.Id, var.Data);
                                kvpOut.Add(new KeyValuePair<string, string>(var.Id.ToString(), var.Data.ToString()));
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorString = String.Format("Message: {0}\r\nSource: {1}\r\nStackTrace:{2}", ex.Message, ex.Source, ex.StackTrace);
                        }


                        break;
                    }
            }

            return kvpOut;
        }

        /// <summary>
        /// Get specifico per la V3
        /// ToDo: testare!
        /// </summary>
        /// <param name="objectId">Id oggetto</param>
        /// <returns>SNMP message</returns>
        private ISnmpMessage GetMessageV3(string objectId)
        {
            return null;

            ErrorString = "";

            //Discover
            Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
            ReportMessage report;

            try
            {
                report = discovery.GetResponse(10000, new IPEndPoint(IpRequestManager, IpRequestPORT));
            }
            catch (Exception ex)
            {
                ErrorString = String.Format("Message: {0}\r\nSource: {1}\r\nStackTrace:{2}", ex.Message, ex.Source, ex.StackTrace);
                return null;
            }

            GetRequestMessage request = new GetRequestMessage(
                VersionCode,
                Messenger.NextMessageId,
                Messenger.NextRequestId,
                UName,
                new List<Variable>
            {
                new Variable(new ObjectIdentifier(objectId))
            },
                Priv,
                Messenger.MaxMessageSize,
                report);

            ISnmpMessage reply;

            try
            {
                reply = request.GetResponse(10000, new IPEndPoint(IpRequestManager, IpRequestPORT));
            }
            catch (Exception ex)
            {
                ErrorString = String.Format("Message: {0}\r\nSource: {1}\r\nStackTrace:{2}", ex.Message, ex.Source, ex.StackTrace);
                return null;
            }

            //if (reply.Pdu().ErrorStatus.ToInt32() != 0) // != ErrorCode.NoError
            //{
            //    throw ErrorException.Create(
            //        "error in response",
            //        IpRequestManager,
            //        reply);
            //}

            return reply;
        }

        /// <summary>
        /// Get di un valore dato un Id
        /// Nota: in realtà viene recuperata una sola chiave per volta.
        /// </summary>
        /// <param name="objectId">Id oggetto</param>
        /// <returns>
        /// Recupera una stringa, composta per ogni riga da una coppia chiave/valore, separate da :
        /// Esempio:
        /// 1.3.6.1.2.1.1.6.0:valore di test 1
        /// </returns>
        public string GetMessageStr(string objectId)
        {
            return "";

            string output = "";

            IList<KeyValuePair<string, string>> kvpOut = GetMessageKvp(objectId);

            if (!string.IsNullOrEmpty(ErrorString))
            {
                return "";
            }

            if (kvpOut.Any())
            {
                foreach (var kvp in kvpOut)
                {
                    output = string.Format("{0}{1}:{2}\r\n", output, kvp.Key, kvp.Value);
                }
            }

            return output;
        }

        /// <summary>
        /// Imposta un valore per una data chiave (Id)
        /// </summary>
        /// <param name="objectId">Id/Chiave</param>
        /// <param name="value">valore</param>
        /// <param name="result">risultato operazione</param>
        /// <returns>
        /// True: success
        /// False: fail
        /// </returns>
        public bool SetMessageStr(string objectId, string value, out string result)
        {
            result = "";
            return true;

            string output = "";
            bool success = false;

            switch (VersionCode)
            {
                case VersionCode.V3:
                    {
                        //Discover
                        Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
                        ReportMessage report;

                        try
                        {
                            report = discovery.GetResponse(10000, new IPEndPoint(IpRequestManager, IpRequestPORT));
                        }
                        catch (Exception ex)
                        {
                            ErrorString = String.Format("Message: {0}\r\nSource: {1}\r\nStackTrace:{2}", ex.Message, ex.Source, ex.StackTrace);
                            result = output;
                            return false;
                        }

                        SetRequestMessage request = new SetRequestMessage(
                            VersionCode.V3,
                            Messenger.NextMessageId,
                            Messenger.NextRequestId,
                            UName,
                            new List<Variable>
                    {
                        new Variable(new ObjectIdentifier(objectId), new OctetString(value, Encoding))
                    },
                            Priv,
                            Messenger.MaxMessageSize,
                            report);

                        ISnmpMessage reply = request.GetResponse(60000, new IPEndPoint(IpRequestManager, IpRequestPORT));

                        if (reply.Pdu().ErrorStatus.ToInt32() != 0) // != ErrorCode.NoError
                        {
                            ErrorString += "error in response: " + reply;
                            result = "";
                            return false;
                        }
                        success = true;
                        break;
                    }
                default:
                    {
                        try
                        {
                            IList<Variable> results = Messenger.Set(VersionCode,
                                new IPEndPoint(IpRequestManager, IpRequestPORT),
                                UName,
                                new List<Variable> { new Variable(new ObjectIdentifier(objectId), new OctetString(value, Encoding)) },
                                15000);

                            output = results.Aggregate("", (current, var) => String.Format("{0}\r\nId: {1}\r\nData: {2}", current, var.Id, var.Data));
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            ErrorString = String.Format("Message: {0}\r\nSource: {1}\r\nStackTrace:{2}", ex.Message, ex.Source, ex.StackTrace);
                            //return false;
                        }
                        break;
                    }
            }

            result = output;
            return success;
        }
        
        #endregion


        /// <summary>
        /// Send snmp trap
        /// </summary>
        /// <param name="id"></param>
        /// <param name="variables">
        ///     couple split by |
        ///     key value split by :
        /// Sample:
        /// key1:value1|key2:value2
        /// </param>
        /// <returns></returns>
        public bool SendTrapStr(string id, string variable, string objId = "")
        {
            if (String.IsNullOrEmpty(id))
            {
                id = DefaultoIdRoot;
            }

            if (String.IsNullOrEmpty(objId))
            {
                objId = ObjectoIdDefault;
            }


            //objId = string.Format(ObjectoIdFormat, id, objId);

            List<Variable> vars = new List<Variable>();

            vars.Add(
                new Variable(
                    new ObjectIdentifier(String.Format(ObjectoIdFormat, id, objId)),
                    new OctetString(variable, Encoding)
                    )
                );

            //if (!string.IsNullOrEmpty(variables))
            //{
            //    string[] varstr = variables.Split('|');

            //    if (varstr != null && varstr.Any())
            //    {
            //        foreach (string v in varstr)
            //        {
            //            string[] vint = v.Split(':');

            //            if (vint.Count() == 2 && !String.IsNullOrEmpty(vint[0]) && !String.IsNullOrEmpty(vint[1]))
            //            {
            //                try
            //                {
            //                    vars.Add(new Variable(new ObjectIdentifier(vint[0]), new OctetString(vint[1], Encoding)));
            //                }
            //                catch { }
            //            }
            //        }
            //    }
            //}

            return SendTrap(id, vars);
        }

        //public bool SendTrapStrSingle(string id, string variables)
        //{
        //    //

        //    ObjectIdentifier obid = null;

        //    try
        //    {
        //        obid = new ObjectIdentifier(id);
        //    } catch
        //    {

        //    }
            
        //    if(obid == null)
        //    {
        //        id = string.Format("1.3.6.1.2.1.1.{0}", id);
        //        obid = new ObjectIdentifier(id);
        //    }
            

        //    List<Variable> vars = new List<Variable>();

        //    if (!string.IsNullOrEmpty(variables))
        //    {
        //        vars.Add(new Variable(new ObjectIdentifier(id), new OctetString(variables, Encoding)));
        //    }

        //    return SendTrap(id, vars);
        //}

        //public bool SendTrapList(string id, List<dtoIdValue> values)
        //{
        //    List<Variable> vars = new List<Variable>();

        //    if (values != null && values.Any())
        //    {
        //        foreach (dtoIdValue val in values)
        //        {
        //            try
        //            {
        //                vars.Add(new Variable(new ObjectIdentifier(val.Id), new OctetString(val.Value, Encoding)));
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }
        //    }

        //    return SendTrap(id, vars);
        //}

        /// <summary>
        /// Invio effettivo trap
        /// </summary>
        /// <param name="id"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public bool SendTrap(string id, List<Variable> variables)
        {
            bool success = false;



            try
            {
                switch (VersionCode)
                {
                    case VersionCode.V1:
                        Messenger.SendTrapV1(
                            new IPEndPoint(IpRequestManager, IpRequestPORT),
                            IpRequestSender,
                            UName,
                            new ObjectIdentifier(id),
                            GenericCode.ColdStart,
                            0,  //specific
                            0,  //timestamp
                            variables
                            );

                        break;

                    case VersionCode.V2:

                        Messenger.SendTrapV2(
                            0,      //request Id
                            VersionCode.V2,
                            new IPEndPoint(IpRequestManager, IpRequestPORT),
                            UName,
                            new ObjectIdentifier(id),
                            0,      //timestamp
                            variables
                            );

                        break;

                    case VersionCode.V3:

                        var trap = new TrapV2Message(
                          VersionCode.V3,
                          528732060,        //messageId
                          1905687779,       //request Id
                          UName,
                          new ObjectIdentifier(id),
                          0,                //time
                          variables,
                          DefaultPrivacyProvider.DefaultPair,
                          0x10000,          //MaxMessageSize
                          new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),      //Engine id
                          0,                //engine boots
                          0);               //engine time

                        trap.Send(new IPEndPoint(IpRequestManager, IpRequestPORT));

                        break;
                }

                success = true;
            }
            catch (Exception ex)
            {
                ErrorString = String.Format("Message: {0}\r\nSource: {1}\r\nStackTrace:{2}", ex.Message, ex.Source, ex.StackTrace);

            }

            return success;
        }
        
    }
}