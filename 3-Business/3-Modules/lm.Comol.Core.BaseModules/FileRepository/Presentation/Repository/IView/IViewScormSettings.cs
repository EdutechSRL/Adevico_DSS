using lm.Comol.Core.BaseModules.FileRepository.Presentation.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.FileRepository.Domain;
using lm.Comol.Core.FileRepository.Domain.ScormSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewScormSettings : IViewScormSettingsBase
    {
        #region "Context"
            Boolean AllowSave{ set; }
        #endregion
    }
}