using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Modules.EduPath.Domain;

namespace lm.Comol.Modules.EduPath.Presentation
{
    public class EduPathIndexPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initialize"
        private int _ModuleID;
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
        protected virtual IViewEduPathIndex View
        {
            get { return (IViewEduPathIndex)base.View; }
        }
        public EduPathIndexPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EduPathIndexPresenter(iApplicationContext oContext, IViewEduPathIndex view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
        }

        public List<dtoSubscriptionInfo> GetSubscriptionsInfo(String roles)
        {
            List<dtoSubscriptionInfo> subscriptions = new List<dtoSubscriptionInfo>();
            List<Int32> idRoles = GetIdRoles(roles);
            try
            {
                CurrentManager.BeginTransaction();
                foreach (Int32 idRole in idRoles)
                {
                    dtoSubscriptionInfo sub = new dtoSubscriptionInfo();
                    sub.IdRole = idRole;
                    sub.Count = (from LazySubscription s in CurrentManager.GetIQ<LazySubscription>() where s.IdRole == idRole && s.Accepted && s.IdCommunity == UserContext.CurrentCommunityID select s.Id).Count();
                    subscriptions.Add(sub);
                }
                CurrentManager.Commit();
            }
            catch (Exception ex)
            {
                CurrentManager.RollBack();
            }
            return subscriptions;
        }

        private List<Int32> GetIdRoles(String roles)
        {
            List<Int32> idRoles = new List<Int32>();
            Char splitChar = ' ';
            if (roles.Contains(","))
                splitChar = ',';
            else if (roles.Contains(";"))
                splitChar = ';';
            else if (roles.Contains("-"))
                splitChar = '-';

            Int32 idRole = -999;
            if (splitChar != ' ')
            {
                foreach (String s in roles.Split(splitChar).Where(r => !string.IsNullOrEmpty(r)).ToList())
                {
                    Int32.TryParse(s, out idRole);
                    if (idRole != -999 && idRole != 0)
                        idRoles.Add(idRole);
                }
            }
            else
            {
                Int32.TryParse(roles, out idRole);
                if (idRole != -999 && idRole != 0)
                    idRoles.Add(idRole);
            }
            return idRoles;
        }
    }
}