using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(String.Format("Program start: {0}", DateTime.Now));

            String TrapId = "4620";

            SNMPCheck.WsSnmtp.dtoActionValues actionvalue = new SNMPCheck.WsSnmtp.dtoActionValues
            {
                Progressive = -1,
                EventId = 4620,
                User = new WsSnmtp.dtoUserValues
                {
                    id = -1,
                    login = "System",
                    mail = "system@mail.not",
                    name = "System",
                    surname = "System",
                    taxCode = "--",
                    Ip = "172.31.16.197",
                    ProxyIp = ""
                },
                Action = new WsSnmtp.dtoActionData
                {
                    ActionCodeId = TrapId,
                    ActionTypeId = "System.Check",
                    CommunityId = 0,
                    CommunityIsFederated = false,
                    InteractionType = "6",
                    ModuleCode = "System",
                    ModuleId = "-1",
                    SuccessInfo = "Success",
                    ObjectId = "0",
                    ObjectType = "0"
                }
            };

            try
            {
                WsSnmtp.WsSnmtpSoapClient TrapSenderClient;
                TrapSenderClient = new WsSnmtp.WsSnmtpSoapClient();


                Console.WriteLine(String.Format("Send to: {0}", TrapSenderClient.Endpoint.ListenUri.AbsoluteUri));

                //WsSnmtp.SendTrapActionValueResponse WsSnmtpresponse = 
                TrapSenderClient.SendTrapActionValue(TrapId, actionvalue); 


            } catch (Exception ex)
            {
                Console.WriteLine(String.Format("Error: {0}", ex));
            }


            Console.WriteLine(String.Format("Program end: {0}", DateTime.Now));

        }
    }
}
