using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Mail;
using lm.Comol.Core.Authentication;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewMailTemplate : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean isInitialized { get; set; }
        Boolean isValid { get; }
        Boolean SendMailToUsers { get; set; }
        dtoMailContent MailContent { get; }
        List<ProfileAttributeType> MandatoryAttributes { get; set; }
        void InitializeControl(dtoImportSettings settings);
        void DisplayMail(List<ProfileAttributeType> attributes);
        void DisplaySessionTimeout();
    }
}