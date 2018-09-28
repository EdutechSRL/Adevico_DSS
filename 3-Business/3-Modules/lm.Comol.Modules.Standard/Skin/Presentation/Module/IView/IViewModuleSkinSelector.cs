using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel.Common;
using lm.Comol.Modules.Standard.Skin.Domain;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public interface IViewModuleSkinSelector : iDomainView
    {
        lm.Comol.Core.DomainModel.ModuleObject Source { get; set; }
        LoadItemsBy LoadModuleSkinBy { get; set; }
        Boolean AllowPreview {  set; }
        Boolean AllowEdit { get; set; }
        Boolean AllowDelete { get; set; }
        Boolean AllowAdd { get; set; }
        Boolean isInitialized { get; set; }
        Boolean isValid { get; }
        Boolean AllowEditSelection { get; set; }
        Boolean FullSkinManagement { get; }
        String DestinationUrl { get; }
        DtoDisplaySkin SelectedItem { get;}
        List<DtoDisplaySkin> Items { get; set; }

        void InitializeControl(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName, LoadItemsBy loadModuleSkinBy);
        void InitializeControl(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName, Boolean allowAdd, Boolean allowEdit, LoadItemsBy loadModuleSkinBy);

        Boolean ObjectHasSkinAssociation(Int32 idModule, Int32 idCommunity, long idModuleItem, Int32 idItemType, String fullyQualifiedName);
        Boolean SaveSkinAssociation();
        void LoadSkins(List<DtoDisplaySkin> skins,DtoDisplaySkin selectedItem );
        void LoadEmptySkin();
        void DisplaySessionTimeout();
    }
}