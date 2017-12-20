using System;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;
namespace lm.Comol.Core.ModuleLinks
{
    [CLSCompliant(true)]
    public interface IExternalModuleDisplayAction : iDomainView
    {
        DisplayActionMode Display { get; set; }
        Boolean EnableAnchor { get; set; }
        Boolean ShortDescription { get; set; }

        String ContainerCSS { get; set; }
        lm.Comol.Core.DomainModel.Helpers.IconSize IconSize { get; set; }
        void InitializeControl(dtoExternalModuleInitializer initializer);
        void InitializeControl(int idUser, dtoExternalModuleInitializer initializer);
        void InitializeControl(dtoExternalModuleInitializer initializer, StandardActionType actionType);
        void InitializeControl(int idUser, dtoExternalModuleInitializer initializer, StandardActionType actionType);

        List<dtoModuleActionControl> InitializeRemoteControl(dtoExternalModuleInitializer initializer, StandardActionType actionsToDisplay);
        List<dtoModuleActionControl> InitializeRemoteControl(int idUser, dtoExternalModuleInitializer initializer, StandardActionType actionsToDisplay);

        void DisplayRemovedObject();
        String getDescriptionByLink(ModuleLink link, Boolean isGeneric);
        String GetInLineDescriptionByLink(ModuleLink link, Boolean isGeneric);
        //String getDescriptionByLink(ModuleLink link);
        //String GetInLineDescriptionByLink(ModuleLink link);
    }
}