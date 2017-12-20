using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewEditPassword : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
       
        void DisplayEditPassword();
        void DisplayPasswordNotChanged();
        void DisplayInvalidPassword();
        void DisplayProviders();
        void DisplayProviders(List<dtoUserProvider> providers); 
        void LoadUserUnknown();
    }
}