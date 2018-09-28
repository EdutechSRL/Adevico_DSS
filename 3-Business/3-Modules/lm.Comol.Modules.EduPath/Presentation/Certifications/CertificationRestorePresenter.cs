using lm.Comol.Modules.EduPath.BusinessLogic;
using lm.Comol.Core.Business;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.ModuleLinks;
using lm.Comol.Modules.EduPath.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
using lm.Comol.Core.Certifications;
namespace lm.Comol.Modules.EduPath.Presentation
{
    public class CertificationRestorePresenter  : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;
            private Service _Service;
            private lm.Comol.Core.Certifications.Business.CertificationsService _ServiceCertifications;
            public lm.Comol.Core.Certifications.Business.CertificationsService ServiceCertifications
            {
                get
                {
                    if (_ServiceCertifications == null)
                        _ServiceCertifications = new lm.Comol.Core.Certifications.Business.CertificationsService(AppContext);
                    return _ServiceCertifications;
                }
            }
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCertificationRestore View
            {
                get { return (IViewCertificationRestore)base.View; }
            }
            private Service Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new Service(AppContext);
                    return _Service;
                }
            }
            public CertificationRestorePresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public CertificationRestorePresenter(iApplicationContext oContext, IViewCertificationRestore view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        #region "Initializer view"
            public void InitView(Boolean isOnModalWindow, long time, long timeValidity, String mac, long idPath, long idActivity, long idSubactivity, Int32 idUser = -1)
            {
                if (UserContext.isAnonymous)
                    View.DisplaySessionTimeout();
                else
                {
                    Int32 idCommunity = Service.GetPathIdCommunity(idPath);
                    dtoSubActivityCertificate dto = Service.GetDtoSubActivityCertificate(idSubactivity);
                    if (dto == null || (dto.IdPath!= idPath))
                        View.DisplayUnknownItem();
                    else {
                        lm.Comol.Core.Authentication.liteGenericEncryption sEncryptor = null;
                        Boolean canRestore = false;
                        litePerson person = CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                        if (person != null){
                            canRestore = (person.TypeID==(int)UserTypeStandard.SysAdmin || person.TypeID==(int)UserTypeStandard.Administrator);
                            if (!canRestore){
                                ModuleEduPath mEduPath = Service.ServiceStat.GetPermissionForCertification(idCommunity,person,idPath);
                                canRestore = mEduPath.Administration;
                            }
                            if (canRestore)
                            {
                                sEncryptor= CurrentManager.GetUrlMacEncryptor();
                                canRestore = (sEncryptor != null && sEncryptor.Validate(mac, UserContext.WorkSessionID, time, timeValidity, idPath, idSubactivity,idUser));
                            }
                        }
                        if (canRestore){
                            View.AddAutoRedirectoToDownloadPage(RootObject.CertificationGenerateAndDownloadPage(sEncryptor.Encrypt(UserContext.WorkSessionID, time, timeValidity, idPath, idSubactivity, idUser), time, timeValidity, idPath, idActivity, idSubactivity, idUser, isOnModalWindow));
                        }
                        else
                            View.DisplayNoPermissions();
                    }
                }
            }
        #endregion
    }
}