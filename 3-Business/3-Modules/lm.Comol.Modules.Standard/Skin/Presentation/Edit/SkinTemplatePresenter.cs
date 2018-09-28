using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Modules.Standard.Skin;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;

namespace lm.Comol.Modules.Standard.Skin.Presentation
{
    public class SkinTemplatePresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        
        #region "Initalize"
            private Business.ServiceSkin _Service;
            private Business.ServiceSkin Service
            {
                get
                {
                    if (_Service == null)
                        _Service = new Business.ServiceSkin(AppContext);
                    return _Service;
                }
            }

            public SkinTemplatePresenter(iApplicationContext oContext, IViewSkinTemplate view)
                : base(oContext, view)
            {
                this.CurrentManager = new BaseModuleManager(oContext);
            }


            protected virtual IViewSkinTemplate View
            {
                get { return (IViewSkinTemplate)base.View; }
            }
        #endregion

            public void InitView(long idSkin,Boolean allowEdit) {
                if (UserContext.isAnonymous)
                    View.AllowEdit = false;
                else
                    View.AllowEdit = allowEdit;
                View.IdSkin = idSkin;
                View.LoadTemplates(Service.GetDtoTemplates(idSkin));
            }
            //public void BindTemplates()
            //{
            //    this.View.BindTemplates(this.Service.GetDtoTemplates());
            //}

            //public void BindTemplates(Int64 SkinId)
            //{
            //    this.View.BindTemplates(this.Service.GetDtoTemplates(SkinId));
            //}

            public void SaveTemplate()
            {
                long idSkin = View.IdSkin;
                if (idSkin > 0)
                    Service.UpdateTemplates(idSkin, View.SelectedIdHeader, View.SelectedIdFooter);
            }
    }
}
