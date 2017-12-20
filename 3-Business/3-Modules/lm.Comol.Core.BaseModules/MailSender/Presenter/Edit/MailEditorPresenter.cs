using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public class MailEditorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewMailEditor View
        {
            get { return (IViewMailEditor)base.View; }
        }

        public MailEditorPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public MailEditorPresenter(iApplicationContext oContext, IViewMailEditor view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(dtoMailContent dto, Boolean senderEdit, Boolean subjectEdit)
        {
            if (!UserContext.isAnonymous)
            {
                Int32 idType = UserContext.UserTypeID;
                View.SetUserMail(UserContext.CurrentUser.Mail);
                View.Settings = dto.Settings;
                View.AllowSenderEdit = senderEdit && (idType == (int)UserTypeStandard.Administrator || idType == (int)UserTypeStandard.Administrative || idType == (int)UserTypeStandard.SysAdmin);
                View.AllowSubjectEdit = subjectEdit && (idType == (int)UserTypeStandard.Administrator || idType == (int)UserTypeStandard.Administrative || idType == (int)UserTypeStandard.SysAdmin);
                View.isInitialized = true;             
            }
        }
        //public Boolean Validate(string mail, List<lm.Comol.Core.Authentication.ProfileAttributeType> attributes) {
        //    Boolean result = false;

        //    return result;
        //}
    }
}
