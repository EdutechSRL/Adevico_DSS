using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel.Repository;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class DisplayRepositoryItemPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private ServiceCommunityRepository _Service;
            private int ModuleID
            {
                get
                {
                    if (_ModuleID <= 0)
                    {
                        _ModuleID = this.Service.ServiceModuleID();
                    }
                    return _ModuleID;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewDisplayRepositoryItem View
            {
                get { return (IViewDisplayRepositoryItem)base.View; }
            }
            private ServiceCommunityRepository Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCommunityRepository(AppContext);
                    return _Service;
                }
            }
            public DisplayRepositoryItemPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public DisplayRepositoryItemPresenter(iApplicationContext oContext, IViewDisplayRepositoryItem view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion
        public void InitView(dtoDisplayItemRepository item, String currentUrl, int IdModule, int IdCommunity, int idAction)
        {
            ItemAvailableCommand commands = (ItemAvailableCommand.Download | ItemAvailableCommand.Play | ItemAvailableCommand.Settings | ItemAvailableCommand.Statistics);
            this.InitView(item, currentUrl, IdModule, IdCommunity, idAction, ItemDisplayView.multilineFull, ItemDisplayMode.inline, ItemDescriptionDisplayMode.AsTooltip, commands);
        }
        public void InitView(dtoDisplayItemRepository item, String currentUrl, int IdModule, int IdCommunity, int idAction, ItemDisplayView view, ItemDisplayMode mode, ItemDescriptionDisplayMode descriptionDisplay, ItemAvailableCommand commands)
        {
            View.DisplayView = view;
            View.DisplayMode = mode;
            View.DescriptionDisplayMode = descriptionDisplay;
            View.AvailableCommands = commands;
            if (item == null || item.File == null)
                View.DisplayUnknownItem();
            // codice file cancellato
            //else if (={}
            else if (UserContext.isAnonymous)
                View.DisplayItemName(item);
            else
            {
                switch (item.File.RepositoryItemType)
                {
                    case RepositoryItemType.ScormPackage:
                    case RepositoryItemType.FileStandard:
                    case RepositoryItemType.None:
                        View.DisplayItem(item,currentUrl, IdModule, IdCommunity, idAction);
                        break;
                    case RepositoryItemType.Folder:
                        View.DisplayFolder(item, View.FolderNavigationUrl, IdModule, IdCommunity, idAction);
                        break;
                    case RepositoryItemType.VideoStreaming:
                        View.DisplayItem(item, currentUrl, IdModule, IdCommunity, idAction);
                        break;
                    case RepositoryItemType.Multimedia:
                        MultimediaFileTransfer fileTransfer = Service.GetMultimediaFileTransfer(item.File);
                        String url = "";
                        if (fileTransfer == null || fileTransfer.DefaultDocument == null)
                            item.Permission.Play = false;
                        else
                            url = fileTransfer.DefaultDocument.Fullname;
                        View.DisplayMultimediaItem(item, currentUrl, url, IdModule, IdCommunity, idAction);
                        break;
                    default:
                        break;
                }
            }    
        }
    }
}