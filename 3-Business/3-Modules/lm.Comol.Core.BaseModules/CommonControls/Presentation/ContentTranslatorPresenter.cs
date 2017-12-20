using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;
using lm.Comol.Core.TemplateMessages.Domain;

namespace lm.Comol.Core.BaseModules.CommonControls.Presentation
{
    public class ContentTranslatorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewContentTranslator View
        {
            get { return (IViewContentTranslator)base.View; }
        }

        public ContentTranslatorPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public ContentTranslatorPresenter(iApplicationContext oContext, IViewContentTranslator view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(dtoTemplateTranslation content)
        {
            if (!UserContext.isAnonymous)
            {
                View.IsInitialized = true;
                View.IsHtml = content.Translation.IsHtml;
                View.Content = content;
            }
            else
                View.DisplaySessionTimeout();
        }
    }
}