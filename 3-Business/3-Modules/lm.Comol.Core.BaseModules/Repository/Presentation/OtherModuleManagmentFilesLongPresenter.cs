using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.Repository.Presentation
{
    public class OtherModuleManagmentFilesLongPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
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
        protected virtual IViewOtherModuleItemFilesLong View
        {
            get { return (IViewOtherModuleItemFilesLong)base.View; }
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
        public OtherModuleManagmentFilesLongPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public OtherModuleManagmentFilesLongPresenter(iApplicationContext oContext, IViewOtherModuleItemFilesLong view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView(long ItemID,iCoreItemPermission itemPermissions , IList<iCoreItemFileLink<long>> links, IList<TranslatedItem<long>> statusList, String publishUrl)
        {
            View.ItemID = ItemID;
            if (UserContext.isAnonymous)
            {
                View.LoadFiles(new List<iCoreItemFileLinkPermission<long>>());
                View.ShowManagementButtons = false;
            }
            else{
                ServiceCommunityRepository service = new ServiceCommunityRepository(AppContext);
                Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
                IList<iCoreItemFileLinkPermission<long>> list = (from l in links
                                                               select (iCoreItemFileLinkPermission<long>)new dtoCoreItemFilePermission<long>() { 
                                                                AvailableStatus=statusList,
                                                                ItemFileLink= l,
                                                                ItemFileLinkId = l.ItemFileLinkId ,
                                                                Permission = service.GetCoreFilePermission(itemPermissions, l, person) 
                                                               }).ToList();

                View.LoadFiles(list);
            }
        }

        //public void InitTaskDetailView(long ItemID, iCoreItemPermission itemPermissions, IList<iCoreItemFileLink<long>> links, IList<TranslatedItem<long>> statusList)
        //    {
        //        View.ItemID = ItemID;
            
        //        ServiceCommunityRepository service = new ServiceCommunityRepository(AppContext);
        //        Person person = CurrentManager.GetPerson(UserContext.CurrentUserID);
        //        IList<iCoreItemFileLinkPermission<long>> list = (from l in links
        //                                                       select (iCoreItemFileLinkPermission<long>)new dtoCoreItemFilePermission<long>() { 
        //                                                        AvailableStatus=statusList,
        //                                                        ItemFileLink= l,
        //                                                        ItemFileLinkId = l.ItemFileLinkId ,
        //                                                        Permission = service.GetCoreFilePermission(itemPermissions, l, person) 
        //                                                       }).ToList();

        //        View.LoadFiles(list);
            
        //    }

        //public void LoadFiles( IList<iCoreItemFileLinkPermission<long>> list)
        //{
        //    View.LoadFiles(list);
        //}
    
    }
}