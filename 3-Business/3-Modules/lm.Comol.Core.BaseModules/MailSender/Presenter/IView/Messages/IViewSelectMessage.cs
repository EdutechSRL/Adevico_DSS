using System;
using System.Linq;
using lm.Comol.Core.TemplateMessages.Domain;
using System.Collections.Generic;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using lm.Comol.Core.BaseModules.TemplateMessages.Domain;
using lm.Comol.Core.Mail.Messages;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public interface IViewSelectMessage : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Boolean IsInitialized { get; set; }
        Int32 IdCommunityContainer { get; set; }
        Int32 IdModuleContainer { get; set; }
        String CodeModuleContainer { get; set; }
        ModuleObject ObjContainer { get; set; }
        List<long> SelectedItems { get;}

        void InitializeControl(ModuleObject obj);
        void InitializeControl(Boolean isPortal,Int32 idCommunity, String modulecode, Int32 idModule,ModuleObject obj);
        void InitializeControl(Boolean isPortal,Int32 idCommunity, String modulecode, Int32 idModule = 0);
        void InitializeControl(Boolean isPortal, Int32 idCommunity, Int32 idModule, String modulecode = "");

        void DisplaySessionTimeout();
        void LoadMessages(List<dtoDisplayMessage> items);
    }
}