using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using Templ = lm.Comol.Core.DomainModel.DocTemplateVers;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public class DocTemplateAssociationPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region initClass
            protected Templ.Business.DocTemplateVersService Service; //DomainModel.DocTemplate.Business.DocTemplateService Service;

            protected virtual IViewDocTemplateAssociation View
            {
                get { return (IViewDocTemplateAssociation)base.View; }
            }

            public DocTemplateAssociationPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                Service = new Templ.Business.DocTemplateVersService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }

            public DocTemplateAssociationPresenter(iApplicationContext oContext, IViewDocTemplateAssociation view)
                : base(oContext, view)
            {
                this.Service = new Templ.Business.DocTemplateVersService(oContext);
                this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion


        public void InitView(long idTemplate, long idVersion, long idModule)
        {
            View.isInitialized = true;
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                View.AllowPreview = false;
            }
            else
            {
                Int32 idUser = UserContext.CurrentUserID;
                List<lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_sTemplate> templates = Service.GetAvailableTemplates(idTemplate, idVersion, idModule);
                if (templates.Count == 0)
                {
                    View.LoadEmptyTemplate();
                    View.AllowPreview = false;
                }
                else
                    View.LoadTemplates(templates);
            }
        }
    }
}