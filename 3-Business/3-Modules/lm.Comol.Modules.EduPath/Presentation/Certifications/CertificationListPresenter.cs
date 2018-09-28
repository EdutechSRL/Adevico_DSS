using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.EduPath.BusinessLogic;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public class CertificationListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _IdModule;
        private Service _Service;
        private int IdModule
        {
            get
            {
                if (_IdModule <= 0)
                {
                    _IdModule = this.PathService.ServiceModuleID();
                }
                return _IdModule;
            }
        }

        private Service PathService
        {
            get
            {
                if (_Service == null)
                    _Service = new Service(AppContext);
                return _Service;
            }
        }
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewCertificationList View
        {
            get { return (IViewCertificationList)base.View; }
        }
        public CertificationListPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public CertificationListPresenter(iApplicationContext oContext, IViewCertificationList view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idCommunity = View.PreloadIdCommunity;
                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                ModuleEduPath mEduPath = PathService.ServiceStat.GetPermissionForCertification(idCommunity, person, View.PreloadIdPath);
                if (mEduPath.Administration || mEduPath.ViewMyStatistics || mEduPath.ViewMyStatistics)
                {
                    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.ViewCerticationList) ;

                    View.CurrentIdPath = View.PreloadIdPath;
                    View.CurrentPathIdCommunity = View.PreloadIdCommunity;

                    LoadCertificates();

                    LoadRoles();

                    View.InitializeFilters();

                    LoadData();
                }
                else
                {
                    View.DisplayWrongPageAccess();
                    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.WrongPageAccess);
                }
            }
        }

        private void LoadCertificates()
        {
            View.Certificates = PathService.GetSubactiviesByPathIdAndTypes(View.CurrentIdPath, SubActivityType.Certificate);
        }

        private void LoadRoles()
        {
            View.Roles = PathService.GetActiveRoles(View.CurrentPathIdCommunity);
        }

        private DateTime ViewStatBefore
        {
            get
            {
                return (View.EndDate != null && View.EndDate.HasValue) ? View.EndDate.Value : DateTime.Now;
            }
        }

        private void LoadCertificatesStat()
        {            

            View.CertificateStats = PathService.ServiceStat.GetCertificatesStat(View.CurrentIdPath, ViewStatBefore, View.AllStatistics, View.StartDate, View.EndDate, View.FilteredCertificate, View.FilteredRole  );
        }

        public void LoadData()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                //View.InitializeFilters();
                
                LoadCertificatesStat();

                View.OnlyOne = false;
                View.Empty = false;

                if (View.CertificateStats.Count() == 1)
                {
                    View.OnlyOne = true;
                }
                else if (View.CertificateStats.Count() == 0)
                {
                    View.Empty = true;
                }
                
                View.SendUserAction(View.CurrentPathIdCommunity, IdModule, ModuleEduPath.ActionType.ViewCerticationList);
            }
        }
        public ModuleEduPath GetModulePermission(Int32 idCommunity)
        {
            return new ModuleEduPath(CurrentManager.GetModulePermission(UserContext.CurrentUserID, idCommunity, IdModule));
        }

        public void UpdateFilters()
        {
            //View.CurrentIdPath = View.PreloadIdPath;
            //View.
            LoadCertificates();
            LoadRoles();
            LoadData();
        }
    }


    
}
