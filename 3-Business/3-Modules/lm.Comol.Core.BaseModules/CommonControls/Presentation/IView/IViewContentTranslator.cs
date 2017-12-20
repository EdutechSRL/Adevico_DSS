using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Helpers;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel;


namespace lm.Comol.Core.BaseModules.CommonControls.Presentation
{
    public interface IViewContentTranslator : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        Boolean IsInitialized { get; set; }
        Boolean ShowName { get; set; }
        Boolean ShowSubject { get; set; }
        Boolean ShowBody { get; set; }
        Boolean ShowShortText { get; set; }
        Boolean AllowValidation { get; set; }
        Boolean MustValidate { get; set; }
        Boolean IsHtml { get; set; }
        lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation Content { get; set; }
        List<TemplatePlaceHolder> MandatoryAttributes { get; set; }

        void InitializeControl(lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation content, lm.Comol.Core.DomainModel.Languages.LanguageItem item);
        void InitializeControl(lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation content, lm.Comol.Core.DomainModel.Languages.LanguageItem item, List<TemplatePlaceHolder> attributes, Boolean validateAttributes);
        void InitializeControl(lm.Comol.Core.DomainModel.Languages.dtoObjectTranslation content, lm.Comol.Core.DomainModel.Languages.LanguageItem item, List<TemplatePlaceHolder> attributes, List<TemplatePlaceHolder> mandatory, Boolean validateAttributes);

        Boolean Validate();
        Boolean Validate(List<TemplatePlaceHolder> attributes);
        void DisplaySessionTimeout();

    }
}