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
    public class CertificationUserPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
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
        protected virtual IViewCertificationUser View
        {
            get { return (IViewCertificationUser)base.View; }
        }
        public CertificationUserPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public CertificationUserPresenter(iApplicationContext oContext, IViewCertificationUser view)
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
                long idPath = View.PreloadIdPath;
                View.CurrentIdPath = idPath;
                litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                ModuleEduPath mEduPath = PathService.ServiceStat.GetPermissionForCertification(idCommunity, person, idPath);
                if (mEduPath.Administration || mEduPath.ViewMyStatistics || mEduPath.ViewMyStatistics)
                {
                    View.SendUserAction(idCommunity, IdModule, ModuleEduPath.ActionType.ViewCerticationList);
                    View.isPathManager = mEduPath.Administration;
                    View.CurrentIdPath = View.PreloadIdPath;
                    View.CurrentPathIdCommunity = View.PreloadIdCommunity;
                    View.CurrentIdUser = View.PreloadIdUser;
                    View.CurrentIdCertificate = View.PreloadIdCertificate;
                    View.isPathManager = mEduPath.Administration;
                    LoadCertificates();

                    //LoadRoles();

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
            //View.Certificates = PathService.GetSubactiviesByPathIdAndTypes(View.CurrentIdPath, SubActivityType.Certificate);
        }

        //private void LoadRoles()
        //{
        //    View.Roles = PathService.GetActiveRoles(View.CurrentPathIdCommunity);
        //}

        private DateTime ViewStatBefore
        {
            get
            {
                return (View.EndDate != null && View.EndDate.HasValue) ? View.EndDate.Value : DateTime.Now;
            }
        }

        private void LoadCertificateStat()
        {

            View.CertificatesStats = PathService.ServiceStat.GetUserCertificatesStat(View.CurrentIdPath, View.CurrentIdUser, ViewStatBefore, View.StartDate, View.EndDate);
        }

        public void LoadData()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                //View.InitializeFilters();

                LoadCertificateStat();


                View.Empty = false;

                if (View.CertificatesStats == null || View.CertificatesStats.Certificates.Count == 0)
                {
                    View.Empty = true;
                }

                //if (View.CertificateStats.Count() == 0)
                //{
                //    View.Empty = true;
                //}

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

            //if (View.FilteredCertificate > 0)
            //{
            //    View.CurrentPathIdCertificate = View.FilteredCertificate;
            //}

            //LoadCertificates();
            //LoadRoles();
            LoadData();
        }
    }
}
