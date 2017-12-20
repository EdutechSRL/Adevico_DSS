using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewPageBase : IViewBase
    {
        #region "Preload"
            Int32 PreloadIdCommunity { get; }

        #endregion

        #region "Context"
            Int32 RepositoryIdCommunity { get; set; }
        #endregion

        #region "Common"
            void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action);
            void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, long idItem, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType objType);
            void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, Dictionary<lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType,List<long>> objects);
        #endregion            
    }
}