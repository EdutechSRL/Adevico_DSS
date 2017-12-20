using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewEditTranslations : IViewBaseEdit 
    {
        long IdTranslation { get; }
        List<String> ContentModules { get; set; }
        Boolean AllowDelete { get; set; }
        Boolean AllowPreview { get; set; }
        LanguageItem CurrentTranslation { get; }
        List<lm.Comol.Core.DomainModel.Helpers.TemplatePlaceHolder> GetContentPlaceHolders(List<string> modulesCodes);
      
       
        void DisplayTemplateTranslationSaved(long idTranslation);
        void DisplayTemplateTranslationErrorSaving();
        void DisplayTemplateTranslationDeleted();
        void DisplayTemplateTranslationErrorDeleting();

        void LoadTranslation(dtoTemplateTranslation content, Boolean displayShortText, Boolean onlyShortText, List<BaseLanguageItem> availableLanguages, List<LanguageItem> inUse, LanguageItem current);
        void LoadTranslation(dtoTemplateTranslation content, Boolean displayShortText, Boolean onlyShortText, LanguageItem current);
        LanguageItem RemoveCurrentTranslation();
        
        void UpdateTranslationSelector(List<BaseLanguageItem> availableLanguages, List<LanguageItem> inUse, LanguageItem current);

        void DisplayMessagePreview(Boolean allowSendMail,String languageCode,ItemObjectTranslation translation, List<String> modules, List<ChannelSettings> notifications,Int32 idCommunity, ModuleObject obj=null);
    }
}