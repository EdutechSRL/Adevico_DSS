using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Domain
{
    [Serializable]
    public class dtoModuleEvents :lm.Comol.Core.TemplateMessages.Domain.dtoBase
    {
        public virtual String ModuleCode { get; set; }
        public virtual String ModuleName {get;set;}
        public virtual Int32 EventsEnabled { get { return (Events == null || !Events.Any()) ? 0 : Events.Where(e => e.IsEnabled).Count(); } }
        public virtual Int32 EventsTotal { get { return (Events == null || !Events.Any()) ? 0 : Events.Count; } }
        public virtual lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Permissions { get; set; }
        public virtual List<dtoModuleEvent> Events { get; set; }
        public virtual List<EventError> Errors { get { return (Events == null || !Events.Any()) ? new List<EventError>() : Events.Where(e=> e.Error != EventError.None).Select(e=>e.Error).ToList(); } }
        public dtoModuleEvents() {
            Events = new List<dtoModuleEvent>();
        }
    }

    [Serializable]
    public class dtoModuleEvent : lm.Comol.Core.TemplateMessages.Domain.dtoBase
    {
        public virtual Int32 IdModule {get;set;}
        public virtual String ModuleCode { get; set; }
        public virtual long IdEvent {get;set;}
        public virtual String Name {get;set;}
        public virtual lm.Comol.Core.TemplateMessages.Domain.dtoSelectorContext Context {get;set;}

        public virtual Boolean IsMandatory { get; set; }
        public virtual Boolean IsEnabled { get; set; }
        public virtual Boolean EditEnabled { get; set; }
        public virtual long IdTemplate { get; set; }
        public virtual long IdVersion { get; set; }
        public virtual lm.Comol.Core.TemplateMessages.Domain.TemplateLevel ItemLevel { get; set; }
        public virtual EventError Error { get; set; }
        public virtual lm.Comol.Core.TemplateMessages.ModuleGenericTemplateMessages Permissions { get; set; }
        public virtual List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateItem> Templates { get; set; }
        public dtoModuleEvent()
        {
            Context = new lm.Comol.Core.TemplateMessages.Domain.dtoSelectorContext();
            Templates = new List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateItem>();
        }
        public String ToString()
        {
            return IdEvent.ToString() + ' ' + Name + ' ' + IsMandatory.ToString() + ' ' + Error.ToString();
        }
    }

    [Serializable]
    public class dtoModuleAction 
    {
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public virtual long IdEvent { get; set; }
        public virtual String Name { get; set; }
        public virtual Boolean IsMandatory { get; set; }
    }

    [Serializable]
    public enum EventError { 
        None = 0,
        TemplateRemoved = 1,
        VersionRemoved = 2,
        NoActionSelected = 3,
        TemplateUnselected = 4
    }
}