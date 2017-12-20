using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewSelectUsersForMessageService : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Initializer"
        Boolean isInitialized { get; set; }
        Boolean FromPortal { get; set; }
        Boolean RaiseEvents { get; set; }
        CallForPaperType CallType { get; set; }
        List<Int32> RemoveUsers { get; set; }
        List<ColumnMessageGrid> AvailableColumns { get; set; }
        ModuleObject CurrentObject { get; set; }
        Int32 IdCommunityContainer { get; set; }
        Int32 IdModuleContainer { get; set; }
        String CodeModuleContainer { get; set; }
        Boolean HasUserValues { get; set; }
        String UnknownUserTranslation { get; }
        String AnonymousUserTranslation { get; }
        Boolean WasGridInitialized { get; set; }
        #endregion

        #region "Filters"
        Int32 UsersPageSize { get; set; }
        PagerBase Pager { get; set; }
        Boolean Ascending { get; set; }
        String CurrentStartWith { get; set; }
        UserByMessagesOrder CurrentOrderBy { get; set; }
        long SelectedIdAgency { get; set; }
        lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy SelectedSearchBy { get; set; }
        dtoUsersByMessageFilter CurrentFilter { get; set; }
        dtoUsersByMessageFilter SelectedFilter { get; }

        Boolean IsInitializedForSubmitters { get; set; }
        Boolean IsInitializedForNoSubmitters { get; set; }
        Boolean SelectAll { get; set; }
        Boolean LoadedNoUsers { get; set; }
        List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> SelectedItems { get; set; }
        #endregion
           
      
        #region "Initialize filters"
        void InitializeMessagesSelector(Boolean fromPortal, Int32 idCommunity, String modulecode, Int32 idModule, ModuleObject obj);
        void LoadAccessType(List<AccessTypeFilter> types, AccessTypeFilter selected);
        void InitializeWordSelector(List<String> availableWords);
        void InitializeWordSelector(List<String> availableWords, String activeWord);

        void LoadSearchProfilesBy(List<lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy> list, lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy defaultSearch);
        void LoadAgencies(Dictionary<long, String> items, long idDefaultAgency);
        void UnLoadAgencies();
        void LoadSubmittersType(Dictionary<long, string> submitters, long dSubmitter);
        void LoadAvailableStatus(List<SubmissionFilterStatus> items, SubmissionFilterStatus selected);

        void DisplaySubmittersFilter();
        void DisplayNoSubmittersFilter();
        #endregion

       

        void InitializeControl(CallForPaperType type, Boolean fromPortal,Int32 idCommunity, ModuleObject obj, long idTemplate, long idVersion, Boolean isTemplateCompliant, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translation = null );
        List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> GetSelectedRecipients();
        List<dtoSelectItem<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem>> GetCurrentSelectedItems();
        void LoadUsers(List<dtoCallMessageRecipient> recipients);
       // void LoadUsers(List<lm.Comol.Core.Mail.Messages.dtoModuleMessageRecipient> recipients);
        void DisplayNoUsersFound();
        void DisplaySessionTimeout();
    }
}