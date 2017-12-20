using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Scorm.Business;
using lm.Comol.Core.BaseModules.FileStatistics.Domain;

namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public class DisplayDetailFilePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private NHibernate.ISession _IcoSession;

        protected virtual IViewDisplayDetailfile View
        {
            get { return (IViewDisplayDetailfile)base.View; }
        }

        #region New
        public DisplayDetailFilePresenter(iApplicationContext oContext)
            : base(oContext)
        {
            throw new ArgumentNullException("IcoSession", "Icodeon Session must be setting.");
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public DisplayDetailFilePresenter(iApplicationContext oContext, IViewDisplayDetailfile view)
            : base(oContext, view)
        {
            throw new ArgumentNullException("IcoSession", "Icodeon Session must be setting.");
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public DisplayDetailFilePresenter(iApplicationContext oContext, IViewDisplayDetailfile view, ref NHibernate.ISession IcoSession)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
            this._IcoSession = IcoSession;
        }
        
        #endregion

        /// <summary>
        /// Richiamato dalla View gli vengono passati TUTTI i parametri necessari...
        /// </summary>
        public void BindView(Int32 UserId, Int64 FileId, IDictionary<String, String> translateServiceCode, Int32 Perm)
        {
            IList<Int32> UsersId = new List<Int32>();
            UsersId.Add(UserId);
            if (Perm > 0)
            {
                this.View.BindData(Service.GetDetailFile(UsersId, FileId, translateServiceCode), Perm);
            }
            else {
                this.View.InitializeNoPermission(base.UserContext.CurrentCommunityID);
            }
        }

        private ServiceFileStatistics _Service;

        private ServiceFileStatistics Service
        {
            get
            {
                if (_Service == null)
                {
                    //NHibernate.ISession ComolSession = SessionHelpers.CurrentDataContext.GetCurrentSession;
                    NHibernate.ISession ComolSession = base.DataContext.GetCurrentSession(); // this.SessionHelpers.CurrentDataContext.GetCurrentSession;

                    //NHibernate.ISession IcodeonSession = //IcoFactory().OpenSession();
                    _Service = new ServiceFileStatistics(AppContext, ComolSession, _IcoSession);
                }
                return _Service;
            }
        }
    }
}
