using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoTemplateStep 
    {
        public virtual WizardTemplateStep Type { get; set; }
        public virtual String Message { get; set; }
        public virtual List<EditingErrors> Errors { get; set; }
         public lm.Comol.Core.Wizard.WizardItemStatus Status { get; set; }
        public virtual String Url { get; set; }
        public dtoTemplateStep() {
            Message = "";
            Errors = new List<EditingErrors>();
            Status = Core.Wizard.WizardItemStatus.none;
        }
        public dtoTemplateStep(WizardTemplateStep type)
            : this()
        {
            Type = type;
            Errors = new List<EditingErrors>();
        }
    }
    
    [Serializable]
    public class dtoSettingsStep : dtoTemplateStep
    {
        public virtual TemplateStatus VersionStatus { get; set; }
        public virtual long NotificationTypes { get; set; }
        public dtoSettingsStep() :base()
        {
        }
        public dtoSettingsStep(WizardTemplateStep type)
                : base(type)
        {
        }
    }
    [Serializable]
    public class dtoTranslationsStep : dtoTemplateStep
    {
        public virtual long Count { get; set; }
        public virtual long Completed { get; set; }
        public virtual long Empty { get; set; }
        public virtual long InvalidItems { get {return Count-Completed-Empty;}}
        public virtual Boolean HasMultilingua { get; set; }
        public dtoTranslationsStep() :base()
        {

        }
        public dtoTranslationsStep(WizardTemplateStep type)
                : base(type)
        {

        }
    }
    [Serializable]
    public class dtoPermissionStep : dtoTemplateStep
    {
        public virtual long NoPermissionsCount { get; set; }
        public virtual long Count { get; set; }
        public dtoPermissionStep()
            : base()
        {
        }
        public dtoPermissionStep(WizardTemplateStep type)
            : base(type)
        {
        }
        public dtoPermissionStep(WizardTemplateStep type, lm.Comol.Core.Wizard.WizardItemStatus status)
            : base(type)
        {
            Status = status;
        }
    }

}