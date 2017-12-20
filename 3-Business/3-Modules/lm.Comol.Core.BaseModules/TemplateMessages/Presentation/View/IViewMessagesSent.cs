using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages;
using lm.Comol.Core.TemplateMessages.Domain;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Mail.Messages;
using lm.Comol.Core.BaseModules.MailSender;
using System.Collections.Generic;
using lm.Comol.Core.Mail;

namespace lm.Comol.Core.BaseModules.TemplateMessages.Presentation
{
    public interface IViewMessagesSent : IViewBaseModuleMessage
    {

        DisplayItems PreloadDisplayBy { get; }
        Boolean PreloadFromCookies { get; }
        String UnknownUserTranslation { get; }
        String AnonymousUserTranslation { get; }
        dtoModuleMessagesContext ContainerContext { get; set; }
        #region "Filters"
            Boolean Ascending { get; set; }
            DisplayItems CurrentDisplayBy { get; set; }
            String CurrentStartWith { get; set; }
            String CurrentSearchValue { get; set; }
            PagerBase Pager { get; set; }
            Int32 PageSize { get; set; }
            List<ColumnMessageGrid> AvailableColumns { get; set; }
            #region "Message filters"
                MessageOrder MessageCurrentOrderBy { get; set; }
            #endregion

            #region "Recipients filters"
                Boolean LoadedNoUsers { get; set; }
                Boolean IsAgencyColumnVisible { get; set; }
                List<lm.Comol.Core.DomainModel.TranslatedItem<Int32>> GetTranslatedRoles { get; }
                List<lm.Comol.Core.DomainModel.TranslatedItem<Int32>> GetTranslatedProfileTypes { get; }
                Int32 CurrentIdRole { get; set; }
                Int32 CurrentIdProfileType { get; set; }
                UserByMessagesOrder UserCurrentOrderBy { get; set; }
                long SelectedIdAgency { get; set; }
                lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy SelectedSearchBy { get; set; }
                dtoUsersByMessageFilters CurrentFilter { get; set; }
                dtoUsersByMessageFilters SelectedFilter { get; }
                void LoadSearchProfilesBy(List<lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy> list, lm.Comol.Core.BaseModules.ProfileManagement.SearchProfilesBy defaultSearch);
                void LoadAvailableProfileType(List<Int32> items, Int32 selected);
                void LoadAvailableRoles(List<Int32> items, Int32 selected);
                void LoadAgencies(Dictionary<long, String> items, long idDefaultAgency);
                void UnLoadAgencies();
            #endregion

            void InitializeFilterSelector(List<DisplayItems> items, DisplayItems selected);
            void InitializeWordSelector(List<String> availableWords);
            void InitializeWordSelector(List<String> availableWords, String activeWord);
        #endregion

        Boolean HasModulePermissions(String moduleCode, long permissions, Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);
        void DisplayMessagePreview(Boolean allowSendMail, String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation translation, List<String> modules, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mSettings, Int32 idCommunity, ModuleObject obj = null);
        void LoadMessages(List<dtoFilteredDisplayMessage> messages);
        void LoadRecipients(List<dtoGenericModuleMessageRecipient> recipients);
        void DisplayMessageFilters(List<String> availableWords, String activeWord);
        void DisplayUserFilters(List<String> availableWords, String activeWord);
        void DisplayNoUsersFound();
        void DisplayNoMessagesFound();
        void DisplayObjectWithNoMessage();
        void GoToUrl(String url);
    }
}