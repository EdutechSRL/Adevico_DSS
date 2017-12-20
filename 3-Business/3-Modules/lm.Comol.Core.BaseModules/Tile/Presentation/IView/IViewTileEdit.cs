using lm.Comol.Core.BaseModules.Tiles.Domain;
using lm.Comol.Core.Dashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation 
{
    public interface IViewTileEdit :lm.Comol.Core.DomainModel.Common.iDomainView
    {
        long IdTile { get; set; }
        Int32 IdTileCommunity { get; set; }
        Int32 IdContainerCommunity { get; set; }
        TileType CurrentType { get; set; }

        void DisplaySessionTimeout();
        void InitalizeControl(dtoEditTile tile, long idTile, Int32 idCommunity, Int32 idTileCommunity, List< lm.Comol.Core.DomainModel.TranslatedItem<long>> tags, List<TileType> types = null);
        void InitalizeControl(dtoEditTile tile, long idTile, Int32 idCommunity, Int32 idTileCommunity, List<TileType> types = null);
        void InitalizeControl(dtoEditTile tile, long idTile, Int32 idCommunity, Int32 idTileCommunity, String itemName);

        void ReloadTags(List<lm.Comol.Core.DomainModel.TranslatedItem<long>> tags);
        void ReloadType(TileType type);
        String GetDefaultLanguageName();
        String GetDefaultLanguageCode();
        void LoadTranslations(List<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem> languages, List<dtoTileFullTranslation> translations);
        void LoadCssAndImages(List<dtoItemFilter<String>> cssClasses, List<dtoItemFilter<String>> images, Boolean allowUpload);
        void LoadImages( List<dtoItemFilter<String>> images);
        void DisplayMessage(Int32 idCommunity,Int32 idModule,long idTile,lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType action);
        void DisplayMessage(Int32 idCommunity, Int32 idModule, long idTile, lm.Comol.Core.Dashboard.Domain.ModuleDashboard.ActionType action, lm.Comol.Core.BaseModules.Tiles.Business.ErrorMessageType errorType);

        void UpdateStatus(lm.Comol.Core.DomainModel.BaseStatusDeleted deleted, AvailableStatus status);
        void VirtualDelete(long idTile, Boolean delete);
        void SetStatus(long idTile, AvailableStatus status);
        void Save();
        void RedirectToEdit(int idCommunity, int idModule, long idTile, ModuleDashboard.ActionType action);
        dtoEditTile GetTile();
    }
}