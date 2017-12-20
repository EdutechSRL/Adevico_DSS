using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public interface IViewGenericUploadFileErrors : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        String BaseFolder {get;}
        String PortalHome { get; }
        String PreloadedBackUrl { get; }
        String PreloadedModuleOwnerCode { get; }
        List<String> PreloadedModuleFiles { get; }
        List<String> PreloadedRepositoryFiles { get; }
        int ModuleOwnerID { get; set; }
        String ModuleOwnerCode { get; set; }
        String DeafultBackUrl { get; set; }
        
        void LoadFiles(List<String> moduleFiles, List<String> repositoryFiles );
        void ShowSessionTimeout();
        void ReturnToManagement();
        void NoPermission(int IdCommunity, int IdModule, String moduleCode);
    }
}
