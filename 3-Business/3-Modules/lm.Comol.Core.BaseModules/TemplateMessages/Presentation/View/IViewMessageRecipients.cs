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
    public interface IViewMessageRecipients :  lm.Comol.Core.DomainModel.Common.iDomainView
    {
        /// <summary>
        /// code
        /// </summary>
        String PreloadModuleCode {get;}
        long PreloadFromModulePermissions { get; }

        Int32 PreloadIdCommunity {get;}
        Int32 PreloadIdModule { get; }
        lm.Comol.Core.DomainModel.ModuleObject PreloadModuleObject { get; }
        
        String CurrentModuleCode { get; set; }
        System.Guid CurrentSessionId { get; set; }
        Int32 CurrentIdCommunity { get; set; }
        Int32 CurrentIdModule { get; set; }
       
        lm.Comol.Core.DomainModel.ModuleObject CurrentModuleObject { get; set; }

        ModuleGenericTemplateMessages GetModulePermissions(string moduleCode, Int32 idModule,long permissions, Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);
        void DisplaySessionTimeout(String url);
        void DisplayNoPermission(int idCommunity, int idModule);
        void DisplayNoPermission(int idCommunity, int idModule, String moduleCode);


        long PreloadIdMessage { get; }
        String UnknownUserTranslation { get; }
        String AnonymousUserTranslation { get; }
        dtoModuleMessagesContext ContainerContext { get; set; }
        long CurrentIdMessage { get; set; }
        #region "Filters"
            Boolean Ascending { get; set; }
            String CurrentStartWith { get; set; }
            String CurrentSearchValue { get; set; }
            PagerBase Pager { get; set; }
            Int32 PageSize { get; set; }
            List<ColumnMessageGrid> AvailableColumns { get; set; }

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

            void InitializeWordSelector(List<String> availableWords);
            void InitializeWordSelector(List<String> availableWords, String activeWord);
        #endregion

        Boolean HasModulePermissions(String moduleCode, long permissions, Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);
        void DisplayMessagePreview(Boolean allowSendMail, String languageCode, lm.Comol.Core.DomainModel.Languages.ItemObjectTranslation translation, List<String> modules, lm.Comol.Core.MailCommons.Domain.Messages.MessageSettings mSettings, Int32 idCommunity, ModuleObject obj = null);
        void LoadRecipients(List<dtoModuleRecipientMessage> recipients);
        void DisplayNoUsersFound();
        void DisplayUnknownMessage();
        void DisplayMessageInfo(MailMessage message);
    }
}