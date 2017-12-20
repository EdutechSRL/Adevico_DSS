using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.Mail;
using lm.Comol.Core.MailCommons.Domain.Messages;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public interface IViewMailEditor : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }

        Boolean AllowNotifyToSender { get; set; }
        Boolean AllowCopyToSender { get; set; }
        Boolean AllowValidation { get; set; }
        Boolean AllowSenderEdit { get; set; }
        Boolean AllowSubjectEdit { get; set; }
        Boolean AllowFormatSelection { get; set; }
        Boolean MustValidate { get; set; }
        Boolean DisplayNotifyToSender { get; set; }
        Boolean DisplayCopyToSender { get; set; }
        String ContainerLeft { get; set; }
        String ContainerRight { get; set; }
        MessageSettings Settings { get; set; }
        dtoMailContent Mail { get; }


        List<TranslatedItem<String>> MandatoryAttributes { get; set; }
        void SetUserMail(String userMail);
        void InitializeControl(dtoMailContent dtoContent, Boolean senderEdit, Boolean subjectEdit);
        void InitializeControl(dtoMailContent dtoContent, Boolean senderEdit, Boolean subjectEdit, List<TranslatedItem<String>> attributes, Boolean validateAttributes);
        void InitializeControl(dtoMailContent dtoContent, Boolean senderEdit, Boolean subjectEdit, List<TranslatedItem<String>> attributes, List<TranslatedItem<String>> mandatory, Boolean validateAttributes);

        Boolean Validate();
        Boolean Validate( List<TranslatedItem<String>> attributes);
    }
}