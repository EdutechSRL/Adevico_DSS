using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Modules.CallForPapers.Business;
using lm.Comol.Modules.CallForPapers.Domain;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public class PublicCallsListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private const int currentPageSize = 25;
        #region "Initialize"
            private ServiceCallOfPapers _Service;

            public virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewPublicCallsList View
            {
                get { return (IViewPublicCallsList)base.View; }
            }
            private ServiceCallOfPapers Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceCallOfPapers(AppContext);
                    return _Service;
                }
            }
            public PublicCallsListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
            public PublicCallsListPresenter(iApplicationContext oContext, IViewPublicCallsList view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            Int32 idUser = UserContext.CurrentUserID;
            litePerson person = GetCurrentUser(ref idUser);
            int idCommunity = SetCallsCurrentCommunity();
            View.InitializeSkin(Service.GetExternalContext(idCommunity, person));
            LoadCalls(idCommunity,  0, currentPageSize);
        }

        private int SetCallsCurrentCommunity()
        {
            int idCommunity = this.UserContext.CurrentCommunityID;
            Community currentCommunity = CurrentManager.GetCommunity(idCommunity);
            Community community = null;
            if (View.PreloadIdCommunity > 0)
                idCommunity = View.PreloadIdCommunity;

            if (idCommunity > 0)
            {
                community = CurrentManager.GetCommunity(idCommunity);
                if (community != null)
                    View.SetContainerName(idCommunity, community.Name);
                else if (currentCommunity != null)
                {
                    idCommunity = this.UserContext.CurrentCommunityID;
                    View.SetContainerName(idCommunity, community.Name);
                }
                else
                {
                    idCommunity = 0;
                    View.SetContainerName(idCommunity, View.Portalname);
                }
            }
            else
                View.SetContainerName(0, View.Portalname);
            View.IdCallCommunity = idCommunity;
            return idCommunity;
        }

        public void LoadCalls(int pageIndex, int pageSize)
        {
            LoadCalls(View.IdCallCommunity, pageIndex, pageSize);
        }
        public void LoadCalls(int idCommunity, int pageIndex, int pageSize)
        {
            Int32 idUser = UserContext.CurrentUserID;
            litePerson person = GetCurrentUser(ref idUser);

            Boolean fromAllcommunities = false;
            PagerBase pager = new PagerBase();
            pager.PageSize = pageSize;

            fromAllcommunities = (idCommunity == 0);
            pager.Count = (int)Service.PublicCallCount(false,fromAllcommunities, (idCommunity == 0), idCommunity, CallStatusForSubmitters.SubmissionOpened)-1;

            pager.PageIndex = pageIndex;// Me.View.CurrentPageIndex
            View.Pager = pager;
            if (pager.Count < 0)
                View.LoadCalls(new List<dtoCallItemPermission>());
            else
            {
                ModuleCallForPaper module = Service.CallForPaperServicePermission(idUser, idCommunity);
                View.LoadCalls(Service.GetCallForPapers(module, CallStandardAction.List, fromAllcommunities, (idCommunity == 0), idCommunity, idUser, CallStatusForSubmitters.SubmissionOpened, FilterCallVisibility.OnlyVisible, pager.PageIndex, pageSize));
            }
        }

        private litePerson GetCurrentUser(ref Int32 idUser)
        {
            litePerson person = null;
            if (UserContext.isAnonymous)
            {
                person = (from p in CurrentManager.GetIQ<litePerson>() where p.TypeID == (int)UserTypeStandard.Guest select p).Skip(0).Take(1).ToList().FirstOrDefault();//CurrentManager.GetLitePerson(UserContext.CurrentUserID);
                idUser = (person != null) ? person.Id : UserContext.CurrentUserID; //if(Person!=null) { IdUser = PersonId} else {IdUser = UserContext...}
            }
            else
                person = CurrentManager.GetLitePerson(idUser);
            return person;
        }
    }
}