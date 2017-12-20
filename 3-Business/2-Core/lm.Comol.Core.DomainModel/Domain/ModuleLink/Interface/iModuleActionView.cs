
using System;
using System.Collections.Generic;
namespace lm.Comol.Core.DomainModel.Common
{
	[CLSCompliant(true)]
	public interface iModuleActionView : iDomainView
	{

        void InitializeControlByLink(ModuleLink pLink);
        void InitializeControlByActionType(ModuleLink pLink, StandardActionType actionType);
        void InitializeControlForUserByActionType(ModuleLink pLink,int idUser, StandardActionType actionType);

        List<dtoModuleActionControl> InitializeRemoteControlByLink(bool DescriptionOnly, ModuleLink pLink);


        void InitializeControlInlineByLink(ModuleLink pLink);
        void InitializeControlInlineForUserByLink(ModuleLink pLink, int idUser);
        List<dtoModuleActionControl> InitializeRemoteControlInlineByLink(bool DescriptionOnly, ModuleLink pLink);
        List<dtoModuleActionControl> InitializeRemoteControlInlineForUserByLink(bool DescriptionOnly, ModuleLink pLink, int idUser);

		void DisplayRemovedObject();
        String getDescriptionByLink(ModuleLink link);
        String GetInLineDescriptionByLink(ModuleLink link);
	}
}