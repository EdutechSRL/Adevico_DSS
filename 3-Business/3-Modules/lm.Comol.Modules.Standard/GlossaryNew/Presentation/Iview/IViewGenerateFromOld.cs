using System;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview
{
    public interface IViewGenerateFromOld : IViewPageBase
    {
        void LoadViewData(List<BaseLanguageItem> languages, Int32 defaultLanguage, GlossaryCounterInfo glossaryInfo);
    }
}