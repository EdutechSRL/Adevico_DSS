using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.Standard.ProjectManagement.Domain;

namespace lm.Comol.Modules.Standard.ProjectManagement.Presentation
{
    public interface IViewProjectListBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        #region "Context"
            //SummaryTimeLine CurrentTimeLine { get; set; }
            PageListType CurrentPageType { get; set; }
           
            Int32 IdCurrentCommunityForList { get; set; }
            Boolean IsInitialized { get; set; }
            Dictionary<ActivityRole, String> RoleTranslations { get; }
            
            Boolean CurrentAscending { get; set; }
            ProjectOrderBy CurrentOrderBy { get; set; }
        #endregion
        String UnknownUserTranslation { get; }
        void SendUserAction(int idCommunity, int idModule, long idProject, ModuleProjectManagement.ActionType action);
        void SendUserAction(int idCommunity, int idModule, ModuleProjectManagement.ActionType action);
    }
}
