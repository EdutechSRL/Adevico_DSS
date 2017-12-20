using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;



namespace Adevico.WebSNMTP.Business
{
    public class SNMTPconnector
    {
        private SHA1AuthenticationProvider Auth { get; set; }
        private DESPrivacyProvider Priv { get; set; }
        private OctetString UName { get; set; }

        private IPAddress IpRequestSender { get; set; }
        private IPAddress IpRequestManager { get; set; }

        private VersionCode VersionCode { get; set; }
        private string myVersion { get; set; }
        public string ErrorString { get; set; }

        private int IpRequestPORT { get; set; }

        private Encoding Encoding { get; set; }

        public SNMTPconnector(
            string authPwd,
            string privPwd,
            string userName,
            string senderIp,
            string receiverIp,
            string receiverPort,
            string version,
            string encoding)
        {

            myVersion = version;

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

        private ISnmpMessage GetMessageV3(string objectId)
        {
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

        public IList<KeyValuePair<string, string>> GetMessageKvp(string objectId)
        {
            IList<KeyValuePair<string, string>> kvpOut = new List<KeyValuePair<string, string>>();

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

        public string GetMessageStr(string objectId)
        {
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

        public bool SetMessageStr(string objectId, string value, out string result)
        {
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
        public bool SendTrapStr(string id, string[] variables)
        {
            List<Variable> vars = new List<Variable>();

            //if (!string.IsNullOrEmpty(variables))
            //{
            //    string[] varstr = variables.Split('|');

            //    if (varstr != null && varstr.Any())
            //    {
            foreach (string v in variables)
            {
                string[] vint = v.Split('|');
                int i = 0;
                string objoId = String.Format("{0}.2.3.{1}", id, i);
                vars.Add(new Variable(new ObjectIdentifier(objoId), new OctetString(v, Encoding)));

                //if (vint.Count() == 2 && !String.IsNullOrEmpty(vint[0]) && !String.IsNullOrEmpty(vint[1]))
                //{
                //    try
                //    {


                //    }
                //    catch { }
                //}
                i++;
            }
            //}
            //}

            return SendTrap(id, vars);
        }

        /// <summary>
        /// Invio effettivo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public bool SendTrap(string id, List<Variable> variables)
        {
            bool success = false;



            try
            {

                switch (myVersion)
                {
                    case "V1": // Lextm.SharpSnmpLib.VersionCode.V1:
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

                    case "V2": // Lextm.SharpSnmpLib.VersionCode.V2:

                        Messenger.SendTrapV2(
                            0,      //request Id
                            Lextm.SharpSnmpLib.VersionCode.V2,
                            new IPEndPoint(IpRequestManager, IpRequestPORT),
                            UName,
                            new ObjectIdentifier(id),
                            0,      //timestamp
                            variables
                            );

                        //var trap = new TrapV2Message(
                        //  VersionCode.V3,
                        //  1,
                        //  1,
                        //  UName,
                        //  new ObjectIdentifier(id),
                        //  0,
                        //  variables,
                        //  DefaultPrivacyProvider.DefaultPair,
                        //  0x10000,
                        //  new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                        //  0,
                        //  0);

                        //trap.Send(new IPEndPoint(IpRequestManager, IpRequestPORT));
                        break;

                    case "V3": // Lextm.SharpSnmpLib.VersionCode.V3:

                        var trap = new TrapV2Message(
                          Lextm.SharpSnmpLib.VersionCode.V3,
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