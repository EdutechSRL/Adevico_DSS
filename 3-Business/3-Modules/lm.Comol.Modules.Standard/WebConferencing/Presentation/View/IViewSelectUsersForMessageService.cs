using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.Standard.WebConferencing.Domain;

namespace lm.Comol.Modules.Standard.WebConferencing.Presentation
{
    public interface IViewSelectUsersForMessageService : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Initializer"
        Boolean isInitialized { get; set; }
        Boolean FromPortal { get; set; }
        Boolean RaiseEvents { get; set; }
        List<long> RemoveUsers { get; set; }
        Boolean IsAgencyColumnVisible { get; set; }
        ModuleObject CurrentObject { get; set; }
        Int32 IdCommunityContainer { get; set; }
        Int32 IdModuleContainer { get; set; }
        String CodeModuleContainer { get; set; }

        String UnknownUserTranslation { get; }
        String AnonymousUserTranslation { get; }
        WbSystemParameter SysParameter { get; }
        List<ColumnMessageGrid> AvailableColumns { get; set; }
        #endregion

        #region "Filters"
        Int32 UsersPageSize { get; set; }
        PagerBase Pager { get; set; }
        Boolean Ascending { get; set; }
        String CurrentStartWith { get; set; }
        UserByMessagesOrder CurrentOrderBy { get; set; }
        MailStatus CurrentMailStatus { get; set; }
        UserStatus CurrentUserStatus { get; set; }
        Int32 CurrentIdRole { get; set; }
        Int32 CurrentIdProfileType { get; set; }
        UserTypeFilter CurrentUserType { get; set; }

        long SelectedIdAgency { get; set; }
        lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy CurrentSearchBy { get; set; }
        dtoUsersByMessageFilter CurrentFilter { get; set; }
        dtoUsersByMessageFilter SelectedFilter { get; }

        Boolean SelectAll { get; set; }
        Boolean LoadedNoUsers { get; set; }
        List<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem> SelectedItems { get; set; }
        List<lm.Comol.Core.DomainModel.TranslatedItem<Int32>> GetTranslatedRoles {get;}
        List<lm.Comol.Core.DomainModel.TranslatedItem<Int32>> GetTranslatedProfileTypes { get; }
        #endregion
           
      
        #region "Initialize filters"
        void InitializeMessagesSelector(Boolean fromPortal, Int32 idCommunity, String modulecode, Int32 idModule, ModuleObject obj);
        void InitializeWordSelector(List<String> availableWords);
        void InitializeWordSelector(List<String> availableWords, String activeWord);

        void LoadSearchProfilesBy(List<lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy> list, lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy defaultSearch);
        void LoadAgencies(Dictionary<long, String> items, long idDefaultAgency);
        void UnLoadAgencies();
        void LoadAvailableStatus(List<MailStatus> items, MailStatus selected);
        void LoadAvailableUserType(List<UserTypeFilter> items, UserTypeFilter selected);
        void LoadAvailableUserStatus(List<UserStatus> items, UserStatus selected);
        void LoadAvailableRoles(List<Int32> items, Int32 selected);
        void LoadAvailableProfileType(List<Int32> items, Int32 selected);
        void HideCommunityFilters();
        #endregion

        void InitializeControl(Boolean fromPortal,Int32 idCommunity, ModuleObject obj, long idTemplate, long idVersion, Boolean isTemplateCompliant, List<lm.Comol.Core.TemplateMessages.Domain.dtoTemplateTranslation> translation = null );
        List<lm.Comol.Core.Mail.Messages.dtoBaseMessageRecipient> GetSelectedRecipients();
        List<dtoSelectItem<lm.Comol.Core.Mail.Messages.dtoModuleRecipientItem>> GetCurrentSelectedItems();
        void LoadUsers(List<dtoWebConferenceMessageRecipient> recipients);
        void DisplayNoUsersFound();
        void DisplaySessionTimeout();
    }
}