using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewActivateUserMail : lm.Comol.Core.DomainModel.Common.iDomainView 
    {

        System.Guid PreloadedIdentifier { get; }
        System.Guid Identifier { get; set; }
        
        void DisplayProfileUnknown();
        void DisplaySessionTimeout();
        void DisplayError(ErrorMessages error);
        void DisplayActivationComplete();
    }
}