//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace lm.Comol.Core.DomainModel.Helpers
//{
//    [Serializable]
//    public class dtoExpiredAccessUrl
//    {
//        public virtual int IdPerson {get;set;}
//        public virtual String DisplayName {get;set;}
//        public virtual int IdCommunity {get;set;}
//        public virtual int IdLanguage {get;set;}
//        public virtual String CodeLanguage {get;set;}
//        public virtual String DestinationUrl {get;set;}
//        public virtual Boolean Preserve { get; set; }
//        public virtual DisplayMode Display { get; set; }

//        public dtoExpiredAccessUrl() {
//            Display = DisplayMode.None;
//        }

//        [Serializable]
//        public enum DisplayMode { 
//            None =0,
//            SameWindow= 1,
//            NewWindow = 2
//        }
//    }
//}