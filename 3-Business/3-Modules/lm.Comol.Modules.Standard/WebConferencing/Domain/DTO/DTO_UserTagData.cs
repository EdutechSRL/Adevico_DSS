using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    [Serializable]
    public class DTO_UserTagData
    {
        //public String FullName { get; set; }
        public String Name { get; set; }
        public String SName { get; set; }
        public String FullName { 
            get
            {
                return Name + " " + SName;
            }
        }

        public String Mail { get; set; }
        public String AccessKey { get; set; }
        public String LanguageCode { get; set; }
    }
}
