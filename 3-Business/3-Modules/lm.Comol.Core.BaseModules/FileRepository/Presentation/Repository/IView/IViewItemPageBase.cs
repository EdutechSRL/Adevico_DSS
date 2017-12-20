using lm.Comol.Core.FileRepository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.FileRepository.Presentation 
{
    public interface IViewItemPageBase : IViewBase
    {
        #region "Preload"
            long PreloadIdItem { get; }
            long PreloadIdVersion { get; }
            long PreloadIdLink { get; }
            ItemType PreloadItemType { get; }
            Boolean PreloadSetBackUrl { get; }
            String PreloadBackUrl { get; }
        #endregion

        #region "Context"
            long IdItem { get; set; }
            long IdVersion { get; set; }
            long IdLink { get; set; }
            ItemType ItemType { get; set; }
            String BackUrl { get; set; }
        #endregion

        #region "Common"
            void SetPageBackUrl(String url);
            void DisplaySessionTimeout(String url, Int32 idCommunity);
        #endregion

        #region "Common"
            void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action);
            void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, long idItem, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType objType);
            void SendUserAction(Int32 idCommunity, Int32 idModule, lm.Comol.Core.FileRepository.Domain.ModuleRepository.ActionType action, Dictionary<lm.Comol.Core.FileRepository.Domain.ModuleRepository.ObjectType, long> objects);
        #endregion
    }
}