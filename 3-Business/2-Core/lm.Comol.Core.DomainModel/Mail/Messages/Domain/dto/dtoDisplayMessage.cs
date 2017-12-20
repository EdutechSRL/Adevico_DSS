using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Mail.Messages
{
    [Serializable]
    public class dtoDisplayMessage
    {
        public virtual long Id { get; set; }
        public virtual long IdTemplate { get; set; }
        public virtual long IdExternalTemplate { get; set; }
        public virtual long IdExternalVersion { get; set; }
        public virtual Boolean ExternalTemplateCompliant { get; set; }
        public virtual String DisplayName {
            get {  return (!String.IsNullOrEmpty(Subject)) ? Subject : ((String.IsNullOrEmpty(Name) ? Id.ToString() : Name));}
        }
        public virtual String Subject { get; set; }
        public virtual String Name { get; set; }
        public virtual String TemplateName { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual litePerson CreatedBy { get; set; }
        public virtual Boolean SentBySystem { get; set; }
        public dtoDisplayMessage()
        {
        }
    }
    [Serializable]
    public class dtoFilteredDisplayMessage : dtoDisplayMessage
    {
        public virtual String FirstLetter { get { return (!String.IsNullOrEmpty(DisplayName)) ? DisplayName.Trim().Substring(0, 1).ToLower() : ""; } }
        public dtoFilteredDisplayMessage() { 
        
        }
        public dtoFilteredDisplayMessage(dtoDisplayMessage message)
        {
            Id = message.Id;
            IdTemplate = message.IdTemplate;
            IdExternalTemplate = message.IdExternalTemplate;
            IdExternalVersion = message.IdExternalVersion;
            ExternalTemplateCompliant = message.ExternalTemplateCompliant;
            Subject = message.Subject;
            Name = message.Name;
            TemplateName = message.TemplateName;
            CreatedOn = message.CreatedOn;
            CreatedBy = message.CreatedBy;
            SentBySystem = message.SentBySystem;
        }
    }
}