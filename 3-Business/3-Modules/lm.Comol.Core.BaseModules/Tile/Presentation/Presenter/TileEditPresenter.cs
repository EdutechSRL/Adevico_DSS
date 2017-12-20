using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.Dashboard.Domain;
using lm.Comol.Core.BaseModules.Tiles.Business;
using lm.Comol.Core.BaseModules.Tiles.Domain;

namespace lm.Comol.Core.BaseModules.Tiles.Presentation
{
    public class TileEditPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags servicetag;
            private ServiceTiles service;
            public virtual BaseModuleManager CurrentManager { get; set; }
            private Int32 currentIdModule;
            protected virtual IViewTileEdit View
            {
                get { return (IViewTileEdit)base.View; }
            }
            private ServiceTiles Service
            {
                get
                {
                    if (service == null)
                        service = new ServiceTiles(AppContext);
                    return service;
                }
            }
            private lm.Comol.Core.BaseModules.Tags.Business.ServiceTags ServiceTags
            {
                get
                {
                    if (servicetag == null)
                        servicetag = new lm.Comol.Core.BaseModules.Tags.Business.ServiceTags(AppContext);
                    return servicetag;
                }
            }
            private Int32 CurrentIdModule
            {
                get
                {
                    if (currentIdModule == 0)
                        currentIdModule = CurrentManager.GetModuleID(ModuleDashboard.UniqueCode);
                    return currentIdModule;
                }
            }
            public TileEditPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public TileEditPresenter(iApplicationContext oContext, IViewTileEdit view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion
        public void InitView(dtoEditTile tile, String cssFile, String imagesFolder)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            { 
                List<lm.Comol.Core.DomainModel.Languages.dtoLanguageItem> items = CurrentManager.GetAllLanguages().OrderByDescending(l => l.isDefault).ThenBy(l => l.Name).Select(l =>
                    new lm.Comol.Core.DomainModel.Languages.dtoLanguageItem() { IdLanguage = l.Id, LanguageCode = l.Code, LanguageName = l.Name } ).ToList();

                items.Insert(0, new lm.Comol.Core.DomainModel.Languages.dtoLanguageItem() { IdLanguage = 0, LanguageCode =  View.GetDefaultLanguageCode(), LanguageName = View.GetDefaultLanguageName(), IsMultiLanguage = true});
                View.CurrentType = tile.Type;
                View.LoadTranslations(items, items.Select(l=> new dtoTileFullTranslation(l,tile)).ToList());
                View.LoadCssAndImages(ExtractIconsCss(cssFile, tile.ImageCssClass), ExtractTileImages(imagesFolder, tile.ImageUrl), true);
            }
        }
        public void ChangeType(TileType type)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                View.CurrentType = type;
                switch (type)
                {
                    case TileType.CombinedTags:
                        Language dLanguage = CurrentManager.GetDefaultLanguage();
                        List<lm.Comol.Core.Tag.Domain.liteTagItem> items = ServiceTags.CacheGetTags(Tag.Domain.TagType.Community, true).Where(t => t.Status == lm.Comol.Core.Dashboard.Domain.AvailableStatus.Available).ToList();
                        List<lm.Comol.Core.DomainModel.TranslatedItem<long>> tags = items.Select(t => new lm.Comol.Core.DomainModel.TranslatedItem<long>() { Id = t.Id, Translation = t.GetTitle(UserContext.Language.Id, dLanguage.Id) }).ToList();
                        View.ReloadTags(tags);
                        break;
                    default:
                        break;
                }
                //View.ReloadType(type);
            }
        }

        private List<dtoItemFilter<String>> ExtractIconsCss(String file, String current="", String pattern = @"\.comtype_64\.(?<class>.*) {")
        {
            List<String> list = new List<String>();
            String code = lm.Comol.Core.File.ContentOf.TextFile(file);
            //using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
            //{
            //    String code = sr.ReadToEnd();

                System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(pattern);

                System.Text.RegularExpressions.MatchCollection mc = rx.Matches(code);

                if (mc != null && mc.Count > 0)
                {
                    foreach (System.Text.RegularExpressions.Match match in mc)
                    {
                        list.Add(match.Groups["class"].Value);
                    }
                     List<dtoItemFilter<String>> items = list.OrderBy(f=>f).Select(f => new dtoItemFilter<String>() { Value = f, DisplayAs = ItemDisplayOrder.item, Selected = (!String.IsNullOrEmpty(current) && f.ToLower() == current.ToLower()) }).ToList();
                    if (items.Count==1)
                        items[0].DisplayAs= ItemDisplayOrder.first | ItemDisplayOrder.last;
                    else{
                        items[0].DisplayAs= ItemDisplayOrder.first;
                        items.Last().DisplayAs= ItemDisplayOrder.last;
                    }
                    return items;
                }
                else
                {
                    return new List<dtoItemFilter<String>>();
                }
            //}

        }

        private List<dtoItemFilter<String>> ExtractTileImages(String path, String current="")
        {
            String[] files = new String[] { };
            if (lm.Comol.Core.File.ContentOf.Directory(path, ref files, false) == File.FileMessage.Read)
            {
                if (files.Any()){
                    List<dtoItemFilter<String>> items = files.Select(f => new dtoItemFilter<String>() { Value = f, DisplayAs = ItemDisplayOrder.item, Selected = (!String.IsNullOrEmpty(current) && f.ToLower() == current.ToLower() )}).ToList();
                    if (items.Count==1)
                        items[0].DisplayAs= ItemDisplayOrder.first | ItemDisplayOrder.last;
                    else{
                        items[0].DisplayAs= ItemDisplayOrder.first;
                        items.Last().DisplayAs= ItemDisplayOrder.last;
                    }
                    return items;
                }
            }
            return new List<dtoItemFilter<String>>();
        }


        public void UploadTileImage(String path, String sourcefile, String extension)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            { 
                ModuleDashboard.ActionType action = ModuleDashboard.ActionType.TileImageNotUploaded;
                System.Guid uniqueId = System.Guid.NewGuid();
                if (!lm.Comol.Core.File.Exists.Directory(path))
                    lm.Comol.Core.File.Create.Directory(path);
                if (lm.Comol.Core.File.Create.CopyFile(sourcefile, path + uniqueId.ToString() + extension))
                {
                    action = ModuleDashboard.ActionType.TileImageUploaded;
                    View.LoadImages(ExtractTileImages(path, uniqueId.ToString() + extension));
                    lm.Comol.Core.File.Delete.File(sourcefile);
                }
                View.DisplayMessage( View.IdTileCommunity, Service.ServiceModuleID(),View.IdTile,action);
            }
        }
        public Int32 GetIdModule()
        {
            return Service.ServiceModuleID();
        }
        public void SetStatus(long idTile, AvailableStatus status)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (status == AvailableStatus.Available) ? ModuleDashboard.ActionType.TileEnable : ModuleDashboard.ActionType.TileDisable;
                Tile item = Service.TileSetStatus(idTile, status);
                if (item == null || item.Status != status)
                    action = (status == AvailableStatus.Available) ? ModuleDashboard.ActionType.TileUnableToEnable : ModuleDashboard.ActionType.TileUnableToDisable;
                View.DisplayMessage(View.IdTileCommunity, Service.ServiceModuleID(),idTile, action);
                if (item != null)
                    View.UpdateStatus(item.Deleted, item.Status);
            }
        }
        public void VirtualDelete(long idTile, Boolean delete)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = (delete) ? ModuleDashboard.ActionType.TileVirtualDelete : ModuleDashboard.ActionType.TileVirtualUndelete;
                Tile item = Service.TileVirtualDelete(idTile, delete);
                if (item == null)
                    action = (delete) ? ModuleDashboard.ActionType.TileUnableToVirtualDelete : ModuleDashboard.ActionType.TileUnableToUndelete;
                View.DisplayMessage(View.IdTileCommunity, Service.ServiceModuleID(), idTile, action);
                View.UpdateStatus(item.Deleted, item.Status);
            }
        }

        public void Save(long idTile, dtoEditTile item, String cssFile, String imagesFolder)
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                ModuleDashboard.ActionType action = action = (idTile > 0) ? ModuleDashboard.ActionType.TileUnableToAdd : ModuleDashboard.ActionType.TileUnableToSave;
                if (item.Type == TileType.CombinedTags && item.IdTags.Count < 2)
                    View.DisplayMessage(View.IdTileCommunity, Service.ServiceModuleID(), idTile, action, ErrorMessageType.MissingTags);
                else
                {
                    Tile tile = null;
                    try
                    {
                        tile = Service.SaveTile(item);
                        if (tile != null)
                            action = (idTile > 0) ? ModuleDashboard.ActionType.TileSaved : ModuleDashboard.ActionType.TileAdded;
                        if (action == ModuleDashboard.ActionType.TileAdded)
                            View.RedirectToEdit(View.IdTileCommunity, Service.ServiceModuleID(), tile.Id, action);
                        else
                            View.DisplayMessage(View.IdTileCommunity, Service.ServiceModuleID(), idTile, action);
                        if (tile != null)
                            View.LoadCssAndImages(ExtractIconsCss(cssFile, tile.ImageCssClass), ExtractTileImages(imagesFolder, tile.ImageUrl), true);
                    }
                    catch (TileException ex)
                    {
                        View.DisplayMessage(View.IdTileCommunity, Service.ServiceModuleID(), idTile, action, ex.ErrorType);
                    }
                    catch (Exception ex)
                    {
                        View.DisplayMessage(View.IdTileCommunity, Service.ServiceModuleID(), idTile, action);
                    }
                }
            }
        }
    }
}