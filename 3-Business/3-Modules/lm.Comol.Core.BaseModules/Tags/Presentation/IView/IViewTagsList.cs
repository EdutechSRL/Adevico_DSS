using lm.Comol.Core.Tag.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tags.Presentation 
{
    public interface IViewTagsList : IViewBase
    {
        #region "Context"
            Int32 CurrentPageSize { get; set; }
            Int32 IdSelectedTagLanguage { get; set; }
            Int32 IdTagsCommunity { get; set; }
            lm.Comol.Core.DomainModel.PagerBase Pager { get; set; }
            dtoFilters CurrentFilters { get; set; }
            Boolean FirstLoad { get; set; }
            Dictionary<Int32, Boolean> FirstLoadForLanguages { get; set; }
            dtoFilters GetSubmittedFilters();
        #endregion
        String GetDefaultLanguageName();
        String GetDefaultLanguageCode();
        String GetUnknownUserName();
        void InitializeControl(ModuleTags permissions,Int32 idCommunity, Boolean fromRecycleBin = false, Boolean fromOrganization = false);
        void LoadLanguages(List<lm.Comol.Core.Dashboard.Domain.dtoItemFilter<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem>> items);
        void LoadTagsInfo(Int32 communititesWithNoTags, Int32 unstranslatedTags, Int32 unusedTags = 0,String bulkUrl="");
        void LoadTags(List<dtoTagItem> items, Int32 idLanguage);
        void DisplayErrorLoadingFromDB();
        void DisplayMessage(ModuleTags.ActionType action);
        void DisplayMessage(ModuleTags.ActionType action, List<String> tags);
        void AllowApplyFilters(Boolean allow);
    }
}