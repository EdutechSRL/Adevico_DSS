using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class RepositoryDisplayInLinePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewModuleToRepositoryDisplayInLine View
            {
                get { return (IViewModuleToRepositoryDisplayInLine)base.View; }
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
            public RepositoryDisplayInLinePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public RepositoryDisplayInLinePresenter(iApplicationContext oContext, IViewModuleToRepositoryDisplayInLine view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(ModuleLink pLink)
        {
            if (pLink == null)
                View.DisplayNoAction();
            else if (pLink.DestinationItem.ServiceCode == CoreModuleRepository.UniqueID)
                AnalyzeAction(pLink);
            else
                View.DisplayNoAction();
        }
        public void InitView(long IdLink)
        {
            ModuleLink Link = Service.GetModuleLink(IdLink);
            if (Link == null)
                View.DisplayNoAction();
            else if (Link.DestinationItem.ServiceCode == CoreModuleRepository.UniqueID)
                AnalyzeAction(Link);
            else
                View.DisplayNoAction();
        }
        //public void InitView(dtoDisplayItemRepository item)
        //{
        //    if (item == null)
        //        View.DisplayNoAction();
        //    else
        //        AnalyzeAction(item);
        //}

        private void AnalyzeAction(ModuleLink pLink)
        {
            int ActionID = pLink.Action;
            BaseCommunityFile item = null;
            if ((pLink.DestinationItem != null))
            {
                item = Service.GetItem(pLink.DestinationItem.ObjectLongID);
                if ((item == null) || (pLink.DestinationItem.ObjectLongID == 0 && (ActionID == (int)CoreModuleRepository.ActionType.CreateFolder || ActionID == (int)CoreModuleRepository.ActionType.UploadFile)))
                    View.DisplayRemovedObject();
                else
                {
                    int IdCommunity = 0;
                    if (item.CommunityOwner != null)
                        IdCommunity = item.CommunityOwner.Id;
                    if (typeof(CommunityFile) == item.GetType())
                    {
                        View.ServiceCode = pLink.DestinationItem.ServiceCode;
                        View.ServiceID = pLink.DestinationItem.ServiceID;
                    }
                    else
                    {
                        View.ServiceCode = pLink.SourceItem.ServiceCode;
                        View.ServiceID = pLink.SourceItem.ServiceID;
                    }

                    if (item is ModuleInternalFile)
                    {
                        ModuleInternalFile oInternal = (ModuleInternalFile)item;
                        View.DisplayLinkForModule(pLink.Id, IdCommunity, item, oInternal.ServiceActionAjax);
                    }
                    else
                        View.DisplayLink(pLink.Id, IdCommunity, item);
                }
            }
            else
                View.DisplayNoAction();
        }

        //private void AnalyzeAction(dtoDisplayItemRepository item)
        //{
        //    if ((item == null))
        //        View.DisplayRemovedObject();
        //    else if (item.File.isFile)
        //    {
        //        if (typeof(CommunityFile) == item.File.GetType())
        //        {
        //            View.ServiceCode = CoreModuleRepository.UniqueID;
        //            View.ServiceID = ModuleID;
        //        }
        //        else
        //            View.DisplayRemovedObject();
        //    }

        //}
    }
}