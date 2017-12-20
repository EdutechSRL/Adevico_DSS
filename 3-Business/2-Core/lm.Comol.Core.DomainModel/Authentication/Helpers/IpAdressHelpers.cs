using System;
using System.Collections.Generic;
using System.Text;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication.Helpers
{
    /// <summary>
    /// Classe che presenta tutti metodi Share/Static per la cifratura e decifratura.
    /// </summary>
    public class IpAdressHelpers
    {
        public static Boolean CheckIsInRange(List<String> range, String ip) {
            Boolean result = false;
            foreach (String r in range) {
                if (r.Contains("-"))
                    result = CheckIfIpValid(r.Split('-')[0], r.Split('-')[1], ip);
                else if (r.Contains("*"))
                    result = CheckIfIpValid(r.Replace("*", "0"), r.Replace("*", "255"), ip);
                else
                    result = (r == ip);
                if (result)
                    return result;
            }
            return result;
        }

        public static Boolean CheckIfIpValid(string allowedStartIp, string allowedEndIp, string ip)
        {
                // if both start and end ip's are null, every user with these credential can log in, no ip restriction needed.

            if (string.IsNullOrEmpty(allowedStartIp) && string.IsNullOrEmpty(allowedEndIp))
                return true;
            bool isStartNull = string.IsNullOrEmpty(allowedStartIp),
                isEndNull = string.IsNullOrEmpty(allowedEndIp);
            string[] startIpBlocks, endIpBlocks, userIp = ip.Split('.');
            if (!string.IsNullOrEmpty(allowedStartIp))
                startIpBlocks = allowedStartIp.Split('.');
            else
                startIpBlocks = "0.0.0.0".Split('.');
            if (!string.IsNullOrEmpty(allowedEndIp))
                endIpBlocks = allowedEndIp.Split('.');
            else
                endIpBlocks = "999.999.999.999".Split('.');

            for (int i = 0; i < userIp.Length; i++)
            {
                // if current block is smaller than allowed minimum, ip is not valid.
                if (Convert.ToInt32(userIp[i]) < Convert.ToInt32(startIpBlocks[i]))
                    return false;
                // if current block is greater than allowed maximum, ip is not valid.
                else if (Convert.ToInt32(userIp[i]) > Convert.ToInt32(endIpBlocks[i]))
                    return false;
                // if current block is greater than allowed minimum, ip is valid.
                else if ((Convert.ToInt32(userIp[i]) > Convert.ToInt32(startIpBlocks[i])) && !isStartNull)
                    return true;

            }
            return true;
        }
    }
}