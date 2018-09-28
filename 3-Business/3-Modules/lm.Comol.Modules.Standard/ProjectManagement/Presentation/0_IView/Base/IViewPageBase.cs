using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewPageBase : lm.Comol.Core.DomainModel.Common.iDomainView 
    {
        #region "Preload"
            Int32 PreloadIdContainerCommunity { get; }
            Int32 PreloadIdCommunity { get; }
            Boolean PreloadForPortal { get; }
            Boolean PreloadIsPersonal { get; }
            PageListType PreloadFromPage { get; }
        #endregion

        #region "Context"
            Int32 ProjectIdCommunity { get; set; }
            Boolean forPortal { get; set; }
            Boolean isPersonal { get; set; }
            Int32 IdContainerCommunity { get; set; }
        #endregion

        #region "Common"
            void DisplayNoPermission(int idCommunity, int idModule);
            void SendUserAction(int idCommunity, int idModule, long idProject, ModuleProjectManagement.ActionType action);
        #endregion            
    }
}