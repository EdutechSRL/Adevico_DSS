using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_Notifications.Configuration
{
    public class KeyComposite<T,Q>
    {
        public T Key{get;set;}
        public Q Value{get;set;}

        public KeyComposite(){}
        public KeyComposite(T oKey , Q oValue ){
            this.Key=oKey;
            this.Value=oValue;
        }
    }
}