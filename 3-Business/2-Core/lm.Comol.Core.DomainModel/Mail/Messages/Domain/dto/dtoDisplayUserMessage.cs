using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoDisplayUserMessage
    {
        public virtual long Id { get; set; }
        public virtual String DisplayName {
            get {  return (!String.IsNullOrEmpty(Subject)) ? Subject :  Id.ToString();}
        }
        public virtual String Subject { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual String CreatedBy { get; set; }
        public virtual Boolean SentBySystem { get; set; }
        public dtoDisplayUserMessage()
        {
        }
    }
}
