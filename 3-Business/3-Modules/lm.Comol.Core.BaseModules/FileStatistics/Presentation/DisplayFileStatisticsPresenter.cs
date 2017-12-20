using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

using lm.Comol.Core.BaseModules.Scorm.Business;
using lm.Comol.Core.Business;
namespace lm.Comol.Core.BaseModules.FileStatistics.Presentation
{
    public class DisplayFileStatisticsPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        private NHibernate.ISession _IcoSession;

        protected virtual IViewDisplayFileStatistics View
        {
            get { return (IViewDisplayFileStatistics)base.View; }
        }

        #region New
        public DisplayFileStatisticsPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            throw new ArgumentNullException("IcoSession", "Icodeon Session must be setting.");
            this.CurrentManager = new BaseModuleManager(oContext);
        }  public DisplayFileStatisticsPresenter(iApplicationContext oContext, IViewDisplayFileStatistics view)
            : base(oContext, view)
        {
            throw new ArgumentNullException("IcoSession", "Icodeon Session must be setting.");
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public DisplayFileStatisticsPresenter(iApplicationContext oContext, IViewDisplayFileStatistics view, ref NHibernate.ISession IcoSession)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
            this._IcoSession = IcoSession;
        }

        #endregion

        

        
        /// <summary>
        /// Richiamato dalla View gli vengono passati TUTTI i parametri necessari...
        /// </summary>
        public void BindView(IList<StatFileTreeLeaf> Files, IList<Int32> UsersIds, Int32 CurrentUserId)
        {
            this.View.SetData(Service.GetFileStat(Files, UsersIds, CurrentUserId));
            //service etc...
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


        public void SendReport(IList<StatFileTreeLeaf> Files, IList<Int32> UsersIds, Int32 CurrentUserId, String CommunityName, Boolean isPdf)
        {
            this.View.SendReport(Service.GetFileStat(Files, UsersIds, CurrentUserId), CommunityName, isPdf);
        }
    }
}
