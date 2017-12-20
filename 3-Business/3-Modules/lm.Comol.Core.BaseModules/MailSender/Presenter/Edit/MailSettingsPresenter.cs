using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;
using lm.Comol.Core.MailCommons.Domain.Messages;
using lm.Comol.Core.MailCommons.Domain;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public class MailSettingsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewMailSettings View
        {
            get { return (IViewMailSettings)base.View; }
        }

        public MailSettingsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public MailSettingsPresenter(iApplicationContext oContext, IViewMailSettings view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(MessageSettings settings, Boolean senderEdit, Boolean subjectEdit, Boolean signatureEdit)
        {
            View.IsInitialized = true;
            if (!UserContext.isAnonymous)
            {
                Int32 idType = UserContext.UserTypeID;
                View.SetUserMail(UserContext.CurrentUser.Mail);
                View.AllowSenderEdit = senderEdit && (idType == (int)UserTypeStandard.Administrator || idType == (int)UserTypeStandard.Administrative || idType == (int)UserTypeStandard.SysAdmin);
                View.AllowSubjectEdit = subjectEdit && (idType == (int)UserTypeStandard.Administrator || idType == (int)UserTypeStandard.Administrative || idType == (int)UserTypeStandard.SysAdmin);
                View.AllowEditSignature = signatureEdit;
                LoadSignatureType(idType,settings);
                View.Settings = settings;
            }
        }

        private void LoadSignatureType(Int32 idType, MessageSettings settings)
        {
            List<Signature> types = new  List<Signature>();

            if (View.AllowNoSignature)
                types.Add(Signature.None);
            types.Add(Signature.FromConfigurationSettings);
            if (View.AllowNoSignature)
                types.Add(Signature.FromNoReplySettings);
            if (View.AllowSignatureFromTemplate)
                types.Add(Signature.FromTemplate);
            if (View.AllowSignatureFromSkin)
                types.Add(Signature.FromSkin);
            if (View.AllowSignatureFromField)
                types.Add(Signature.FromField);
            //if((idType == (int)UserTypeStandard.Administrator || idType == (int)UserTypeStandard.Administrative || idType == (int)UserTypeStandard.SysAdmin)){
                

            //}

            View.LoadSignatureTypes(types, (types.Contains(settings.Signature) ? settings.Signature : Signature.FromConfigurationSettings));
        }
    }
}