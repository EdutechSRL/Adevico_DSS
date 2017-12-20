using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.Authentication.Business;
namespace lm.Comol.Core.BaseModules.AuthenticationManagement.Presentation
{
    public class PublicAccessPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
            private int _ModuleID;

            private InternalAuthenticationService _InternalService;
            //private int ModuleID
            //{
            //    get
            //    {
            //        if (_ModuleID <= 0)
            //        {
            //            _ModuleID = this.Service.ServiceModuleID();
            //        }
            //        return _ModuleID;
            //    }
            //}
            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewPublicAccess View
            {
                get { return (IViewPublicAccess)base.View; }
            }
            private InternalAuthenticationService InternalService
            {
                get
                {
                    if (_InternalService == null)
                        _InternalService = new InternalAuthenticationService(AppContext);
                    return _InternalService;
                }
            }
            public PublicAccessPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PublicAccessPresenter(iApplicationContext oContext, IViewPublicAccess view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView(Int32 idUser,Int32 idCommunity)
        {
            Boolean accessAvailable = !(View.isSystemOutOfOrder );
            if (!accessAvailable)
                View.DisplaySystemOutOfOrder();
            else
            {
                Person person = null;
                if ( idUser > 0)
                    person = CurrentManager.GetPerson(idUser);
                //else if (!UserContext.isAnonymous)
                //    person = CurrentManager.GetPerson(UserContext.CurrentUserID);

                if (person == null || (person != null && person.TypeID != (int)UserTypeStandard.PublicUser))
                    person = InternalService.GetDefaultUser(UserTypeStandard.PublicUser);

                if (person != null)
                {
                    if (idCommunity==0)
                        idCommunity= InternalService.GetDefaultLogonCommunity(person);

                    Community community = CurrentManager.GetCommunity(idCommunity);
                    if (community == null)
                        View.DisplayUnknownCommunity();
                    else if (!community.AllowPublicAccess)
                    {
                        View.DisplayCommunityName(community.Name);
                        View.DisplayNotAllowedCommunity();
                    }
                    else
                    {
                        View.DisplayCommunityName(community.Name);
                        View.LogonUser(person, person.IdDefaultProvider, lm.Comol.Core.BaseModules.ModulesLoader.RootObject.PublicAccess(View.PreloadedIdUser, View.PreloadedIdCommunity));
                        View.LogonIntoCommunity(person.Id, idCommunity);
                    }
                }
                else
                    View.DisplayUnknownUser();
            }
        }
    }
}