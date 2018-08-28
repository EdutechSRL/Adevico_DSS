using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    public class DataHelpers
    {
        public static string ConnectionString(){
            return System.Configuration.ConfigurationManager.AppSettings["DBconnection"];    
        }
    }
}