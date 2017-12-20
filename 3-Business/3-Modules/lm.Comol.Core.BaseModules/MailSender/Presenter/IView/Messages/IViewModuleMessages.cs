using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Mail;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.Mail.Messages;

namespace lm.Comol.Core.BaseModules.MailSender.Presentation
{
    public interface IViewModuleMessages : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        Int32 PreloadIdCommunity { get; }
        Int32 PreloadIdModule { get; }
        String PreloadModuleCode { get; }
        lm.Comol.Core.DomainModel.ModuleObject PreloadModuleObject { get; }
       


        Boolean HasModulePermissions(String moduleCode, long permissions,Int32 idCommunity, Int32 profileType, lm.Comol.Core.DomainModel.ModuleObject obj);

        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule, String moduleCode);
    }
}