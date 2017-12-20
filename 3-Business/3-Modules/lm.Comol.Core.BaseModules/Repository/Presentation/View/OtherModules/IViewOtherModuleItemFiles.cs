using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.CommunityDiary.Domain;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewOtherModuleItemFiles : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        int ModuleID {get;set;}
        String ModuleCode {get;set;}
        String PublishUrl { get; set; }
        int ItemCommunityID {get;set;}
        iCoreItemPermission ItemPermissions { get; set; }
        Boolean AutoUpdate {get;set;}
        Boolean ShowManagementButtons {get;set;}
        Boolean ShowStatus { get; set; }
        void RequireUpdate();
    }
}