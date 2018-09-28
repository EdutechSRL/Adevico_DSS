using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.eWorks
{
    public class eWSystemParameter : WbSystemParameter
    {
        public string BaseUrl { get; set; }
        public string MainUserId { get; set; }
        public string MainUserPwd { get; set; }
        public bool UseProxy { get; set; }
        public int MaxUrlChars { get; set; }

        private string _ProxyUrl { get; set; }
        
        public string ProxyUrl
        {
            get
            {
                if (UseProxy) return _ProxyUrl;
                else return "";
            }
            set
            {
                _ProxyUrl = value;
            }
        }

        /// <summary>
        /// Versione eWorks.
        /// Attualmente in uso SOLO "7.0", poichè è stato tolto un parametro di accesso...
        /// </summary>
        public string Version { get; set; }
    }
}
