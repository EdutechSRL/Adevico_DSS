using System;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;

namespace lm.Comol.Core.BaseModules.ProfileManagement.Presentation
{
    public interface IViewStepProfileInfo : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        int SelectedIdOrganization { get; set; }
        void DisableInput(List<lm.Comol.Core.Authentication.ProfileAttributeType> items);
    }
}