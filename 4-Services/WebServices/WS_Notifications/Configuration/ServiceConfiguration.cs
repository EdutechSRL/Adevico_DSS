using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Notifications.Configuration
{
    public class ServiceConfiguration
    {
        public TimeSpan DefaultTimeToLive{get;set;}
        public IList<KeyComposite<KeyType,string>>CacheKeys{get;set;}
        public IList<KeyComposite<KeyType,TimeSpan>>CacheTimesToLive{get;set;}

        public String CacheKey(KeyType oType ){
            return (from KeyComposite<KeyType,string> ck in CacheKeys where ck.Key==oType select ck.Value ).FirstOrDefault<string>();
        }
        public TimeSpan CacheTimeToLive(KeyType oType ){
            TimeSpan oTimeSpan = (from KeyComposite<KeyType,TimeSpan> ck in CacheKeys where ck.Key==oType select ck.Value ).FirstOrDefault<TimeSpan>();
            if (oTimeSpan==null) {
                oTimeSpan =DefaultTimeToLive;
            }

            return oTimeSpan;
        }

        private static String Config(String KeyName){
            return System.Configuration.ConfigurationManager.AppSettings[KeyName];
        }

        public ServiceConfiguration(){
            this.DefaultTimeToLive = new TimeSpan(0, 20, 0);
			this.CacheKeys = new List<KeyComposite<KeyType,string>>();
            this.CacheTimesToLive = new List<KeyComposite<KeyType, TimeSpan>>();
        }

        public static ServiceConfiguration CreateConfigSettings(){
            ServiceConfiguration oConfig = new ServiceConfiguration();
            int seconds = int.Parse(Config("DefaultTimeToLive"));
            oConfig.DefaultTimeToLive = new TimeSpan(0, 0, seconds);
         
            
            return oConfig;

            //
            //

            //oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.Login, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CacheLoginTimeToLive")))})

            //oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.LastAction, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CacheLastActionTimeToLive")))})

            //oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.Action, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CacheActionTimeToLive")))})

            //oConfig.CacheTimesToLive.Add(New KeyComposite(Of KeyType, TimeSpan) With {.Key = KeyType.WebPresence, .Value = New TimeSpan(0, 0, Integer.Parse(Config("CachePresenceTimeToLive")))})

            //oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.Action, .Value = Config("CacheActionKey")})

            //oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.LastAction, .Value = Config("CacheLastActionKey")})

            //oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.Login, .Value = Config("CacheLoginKey")})

            //oConfig.CacheKeys.Add(New KeyComposite(Of KeyType, String) With {.Key = KeyType.WebPresence, .Value = Config("CachePresenceKey")})

            //
            //Return oConfig
        }

    }
}
