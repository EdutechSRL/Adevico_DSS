using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using lm.Comol.Core.Cache;
using WS_Notifications.Configuration;

namespace WS_Notifications.Business
{
    public class ServiceUtility
    {
      public static ServiceConfiguration Config = ServiceConfiguration.CreateConfigSettings();
      public static lm.Comol.Core.Cache.iCache CurrentCache = CacheFactory.CacheSetup(Config.DefaultTimeToLive);
    }
}