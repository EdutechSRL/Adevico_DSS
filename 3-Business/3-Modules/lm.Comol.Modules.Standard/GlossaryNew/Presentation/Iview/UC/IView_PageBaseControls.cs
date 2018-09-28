using System;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.GlossaryNew.Domain;

namespace lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
{
    public interface IView_PageBaseControls : iDomainView
    {
        Boolean DisableUserAction { get; set; }
        Int32 PreloadIdCommunity { get; }
        long PreloadIdGlossary { get; }
        Int32 IdCommunity { get; set; }
        long IdGlossary { get; set; }
        long IdTerm { get; set; }
        Boolean ForManage { get; set; }
        Boolean ForManageEnabled { get; set; }
        void DisplaySessionTimeout();
        void DisplayNoPermission(int idCommunity, int idModule);
        void SendUserAction(int idCommunity, int idModule, ModuleGlossaryNew.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idGlossary, ModuleGlossaryNew.ActionType action);
        void SendUserAction(int idCommunity, int idModule, long idGlossary, long idTerm, ModuleGlossaryNew.ActionType action);
    }
}