using System;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;
namespace lm.Comol.Core.ModuleLinks
{
    [CLSCompliant(true)]
    public interface IViewModuleRenderAction : iDomainView
    {
        Boolean InsideOtherModule { get; set; }
        String SourceModuleCode { get; set; }
        Int32 SourceIdModule { get; set; }
        Int32 ForIdUser { get; set; }

        DisplayActionMode Display { get; set; }
        String CssClass { get; set; }
        Boolean EnableAnchor { get; set; }
        Boolean ShortDescription { get; set; }
        Int32 IdRequiredAction { get; set; }

        void InitializeControl(dtoObjectRenderInitializer initializer, DisplayActionMode action, String cssClass = "");
        void InitializeControl(Int32 idUser, dtoObjectRenderInitializer initializer, DisplayActionMode action, String cssClass = "");
        void InitializeControl(dtoObjectRenderInitializer initializer, StandardActionType actionType, DisplayActionMode action, String cssClass = "");
        void InitializeControl(Int32 idUser, dtoObjectRenderInitializer initializer, StandardActionType actionType, DisplayActionMode action, String cssClass = "");

        List<dtoModuleActionControl> InitializeRemoteControl(dtoObjectRenderInitializer initializer, StandardActionType actionsToDisplay, DisplayActionMode action);
        List<dtoModuleActionControl> InitializeRemoteControl(Int32 idUser, dtoObjectRenderInitializer initializer, StandardActionType actionsToDisplay, DisplayActionMode action);

        void DisplayRemovedObject();
        void DisplayEmptyAction();
        String GetDescriptionByLink(ModuleLink link, Boolean isGeneric);
        String GetDescriptionByLink(liteModuleLink link, Boolean isGeneric);
        String GetInLineDescriptionByLink(liteModuleLink link, Boolean isGeneric);
    }
}