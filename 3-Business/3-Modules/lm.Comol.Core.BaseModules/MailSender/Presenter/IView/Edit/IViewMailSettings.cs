using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;
using lm.Comol.Core.MailCommons.Domain.Messages;
using lm.Comol.Core.MailCommons.Domain;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public interface IViewMailSettings : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean IsInitialized { get; set; }
        Boolean AllowEditSignature { get; set; }
        Boolean AllowNoSignature { get; set; }
        Boolean AllowSignatureFromTemplate { get; set; }
        Boolean AllowSignatureFromSkin { get; set; }
        Boolean AllowSignatureFromField { get; set; }
        Boolean AllowNotifyToSender { get; set; }
        Boolean AllowCopyToSender { get; set; }
        Boolean AllowSenderEdit { get; set; }
        Boolean AllowSubjectEdit { get; set; }
        Boolean AllowFormatSelection { get; set; }
        Boolean DisplayNotifyToSender { get; set; }
        Boolean DisplayCopyToSender { get; set; }
        MessageSettings Settings { get; set; }
        void SetUserMail(String userMail);
        void InitializeControl(MessageSettings settings, Boolean senderEdit, Boolean subjectEdit, Boolean signatureEdit, Boolean editing = true);
        void LoadSignatureTypes(List<Signature> types, Signature defaultType);
    }
}