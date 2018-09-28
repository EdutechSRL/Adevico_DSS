using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.Menu.Domain;
using lm.Comol.Core.DomainModel.Common;

namespace lm.Comol.Modules.Standard.Menu.Presentation
{
    public interface IViewItemTranslations : iDomainView
    {
        long IdMenuItem { get; set; }
        List<dtoTranslation> GetTranslations { get; }
        void InitalizeControl(List<dtoTranslation> translations, long IdItem, Boolean allowEdit);
    }
}