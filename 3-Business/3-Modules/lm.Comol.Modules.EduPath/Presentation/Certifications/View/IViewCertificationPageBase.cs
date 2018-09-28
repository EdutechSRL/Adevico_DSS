using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;
using lm.Comol.Modules.EduPath.Domain.DTO;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public interface IViewCertificationPageBase : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long PreloadIdSubactivity { get; }
        long PreloadIdActivity { get; }
        long PreloadIdPath { get; }
        Int32 PreloadIdCommunity { get; }
        Int32 PreloadIdUser { get; }
        Boolean PreloadReloadOpener { get; }
        Boolean PreloadForManager { get; }
        Boolean PreloadSaveStatistics { get; }
        long PreloadTime { get; }
        long PreloadTimeValidity { get; }
        String PreloadMac { get; }
     

        Boolean ReloadOpener { get; set;}
        Boolean IsOnModalWindow { get; }
        

        void DisplaySessionTimeout();
        void DisplayUnknownItem();
        void DisplayNoPermissions();
        void DisplayUnselectedTemplate();
        void DisplayUnavailableAction(String name);
    }
}