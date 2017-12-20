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
    public class ModuleToRepositoryDisplayPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
            protected virtual IViewModuleModuleToRepositoryDisplay View
            {
                get { return (IViewModuleModuleToRepositoryDisplay)base.View; }
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
            public ModuleToRepositoryDisplayPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public ModuleToRepositoryDisplayPresenter(iApplicationContext oContext, IViewModuleModuleToRepositoryDisplay view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(long idLink)
        {
           InitView(idLink,null);
        }
        public void InitView(ModuleLink link)
        {
           InitView(link,null);
        }
        public void InitView(long idLink, iCoreFilePermission permission)
        {
           InitView(CurrentManager.Get<ModuleLink>(idLink),permission);
        }
        public void InitView(ModuleLink link, iCoreFilePermission permission)
        {
           View.FileDisplayName="";
            if (link != null && link.DestinationItem !=null && link.DestinationItem.ServiceCode== CoreModuleRepository.UniqueID)
                AnalyzeAction(link, permission);
            else
                View.DisplayNoAction();
        }
        //Public Sub InitView(ByVal oItem As dtoCommunityItemRepository)
        //    Me.View.FileDisplayName = ""
        //    If oItem Is Nothing Then
        //        Me.View.DisplayNoAction()
        //    Else : AnalyzeAction(oItem)
        //    End If
        //End Sub

        public void AnalyzeAction(ModuleLink link, iCoreFilePermission permission){
            Int32 idAction = link.Action;
            BaseCommunityFile item = null;
            if (link.DestinationItem !=null){
                item = Service.GetItem(link.DestinationItem.ObjectLongID);
                if (item == null || (link.DestinationItem.ObjectLongID==0 && (idAction == (Int32) CoreModuleRepository.ActionType.CreateFolder || idAction == (Int32) CoreModuleRepository.ActionType.UploadFile)))
                    View.DisplayRemovedObject();
                else{
                    Int32 idCommunity = 0;
                    switch( idAction){
                        case (Int32) CoreModuleRepository.ActionType.DownloadFile:
                            if (typeof(CommunityFile) == item.GetType()){
                               View.ModuleCode = link.DestinationItem.ServiceCode;
                               View.IdModule = link.DestinationItem.ServiceID;
                            }
                            else{
                                View.ModuleCode = link.SourceItem.ServiceCode;
                                View.IdModule = link.SourceItem.ServiceID;
                            }
                            if (item.CommunityOwner!=null)
                                idCommunity= item.CommunityOwner.Id;

                            View.FileDisplayName = item.DisplayName;
                            View.DisplayLinkDownload(link.Id, idCommunity, item, permission);
                            break;
                        case (Int32) CoreModuleRepository.ActionType.PlayFile:
                            
                            if (typeof(CommunityFile) == item.GetType()){
                               View.ModuleCode = link.DestinationItem.ServiceCode;
                               View.IdModule = link.DestinationItem.ServiceID;
                            }
                            else{
                                View.ModuleCode = link.SourceItem.ServiceCode;
                                View.IdModule = link.SourceItem.ServiceID;
                            }
                            if (item.CommunityOwner!=null)
                                idCommunity= item.CommunityOwner.Id;
                            View.FileDisplayName = item.DisplayName;

                            if (typeof(ModuleInternalFile) == item.GetType()){
                                ModuleInternalFile intItem  = (ModuleInternalFile)item;
                                View.DisplayLinkForPlayInternal(link.Id, idCommunity, item, permission, intItem.ServiceActionAjax);
                            }
                            else
                                View.DisplayLinkForPlay(link.Id, idCommunity, item, permission);
                            break;
                        default:
                            View.DisplayNoAction();
                            break;
                    }
                }
            }
            else
                View.DisplayNoAction();
        }
    }
}

//        Private Sub AnalyzeAction(ByVal oItem As dtoCommunityItemRepository)
//            If IsNothing(oItem) Then
//                Me.View.DisplayRemovedObject()
//            ElseIf oItem.File.isFile Then
//                If TypeOf oItem.File Is CommunityFile Then
//                    Me.View.ServiceCode = UCServices.Services_File.Codex
//                    Me.View.ServiceID = Me.RepositoryModuleID
//                End If
//            Else
//                Me.View.DisplayRemovedObject()
//            End If
//        End Sub
//    End Class
//End Namespace