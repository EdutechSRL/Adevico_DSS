using System;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Core.DomainModel;
using System.Collections.Generic;
namespace lm.Comol.Core.ModuleLinks
{
	[CLSCompliant(true)]
    public interface IGenericModuleDisplayAction : iDomainView
	{
        DisplayActionMode Display { get; set; }
        String ContainerCSS { get; set; }
        Boolean EnableAnchor { get; set; }
        Boolean ShortDescription { get; set; }
        lm.Comol.Core.DomainModel.Helpers.IconSize IconSize { get; set; }

        void InitializeControl(dtoModuleDisplayActionInitializer initializer);
        void InitializeControl(int idUser, dtoModuleDisplayActionInitializer initializer);
        void InitializeControl(dtoModuleDisplayActionInitializer initializer, StandardActionType actionType);
        void InitializeControl(int idUser, dtoModuleDisplayActionInitializer initializer, StandardActionType actionType);

        List<dtoModuleActionControl> InitializeRemoteControl(dtoModuleDisplayActionInitializer initializer, StandardActionType actionsToDisplay);
        List<dtoModuleActionControl> InitializeRemoteControl(int idUser, dtoModuleDisplayActionInitializer initializer, StandardActionType actionsToDisplay);

        void DisplayRemovedObject();
        String getDescriptionByLink(ModuleLink link, Boolean isGeneric);
        String GetInLineDescriptionByLink(ModuleLink link, Boolean isGeneric);
        //String getDescriptionByLink(ModuleLink link);
        //String GetInLineDescriptionByLink(ModuleLink link);
	}
}