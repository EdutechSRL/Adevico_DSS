using System;
using System.Collections.Generic;
using System.Linq;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Mail.Messages;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public class SelectMessagePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Initialize"
            private MailMessagesService service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewSelectMessage View
            {
                get { return (IViewSelectMessage)base.View; }
            }
            private MailMessagesService Service
            {
                get
                {
                    if (service == null)
                        service = new MailMessagesService(AppContext);
                    return service;
                }
            }
            public SelectMessagePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public SelectMessagePresenter(iApplicationContext oContext, IViewSelectMessage view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(dtoOwnership owner)
        {
            View.IsInitialized = true;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                if (owner.IdCommunity == -1 && owner.ModuleObject != null)
                    owner.IdCommunity = owner.ModuleObject.CommunityID;

                owner.IsPortal = owner.IsPortal || (owner.IdCommunity == 0);
                if (owner.IdModule > 0 && String.IsNullOrEmpty(owner.ModuleCode))
                    owner.ModuleCode = CurrentManager.GetModuleCode(owner.IdModule);
                else if (owner.IdModule == 0 && !String.IsNullOrEmpty(owner.ModuleCode))
                    owner.IdModule = CurrentManager.GetModuleID(owner.ModuleCode);
                View.IdCommunityContainer = owner.IdCommunity;
                View.IdModuleContainer = owner.IdModule;
                View.CodeModuleContainer = owner.ModuleCode;
                View.ObjContainer = owner.ModuleObject;
                View.LoadMessages(Service.GetDisplayMessages(owner,UserContext.CurrentUserID));
            }
        }
    }
}