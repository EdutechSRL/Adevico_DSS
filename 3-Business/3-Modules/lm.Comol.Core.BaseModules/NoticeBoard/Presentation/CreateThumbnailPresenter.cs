using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.NoticeBoard.Business;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Editor;
using lm.Comol.Core.BaseModules.Editor.Business;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public class CreateThumbnailPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
         #region "Init / Default"
            private int _ModuleID;
            private ServiceNoticeBoard _Service;
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

            protected virtual BaseModuleManager CurrentManager { get; set; }
            protected virtual IViewCreateThumbnail View
            {
                get { return (IViewCreateThumbnail)base.View; }
            }
            private ServiceNoticeBoard Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new ServiceNoticeBoard(AppContext);
                    return _Service;
                }
            }
        #endregion

        public CreateThumbnailPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public CreateThumbnailPresenter(iApplicationContext oContext, IViewCreateThumbnail view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView(long idMessage, Int32 idCommunity){
            View.GenerateThumbnail(idMessage, idCommunity);
        }
    }
}