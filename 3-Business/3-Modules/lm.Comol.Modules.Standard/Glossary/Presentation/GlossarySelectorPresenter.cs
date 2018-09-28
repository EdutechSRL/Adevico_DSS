using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;


namespace lm.Comol.Modules.Standard.Glossary.MVP
{
    public class GlossarySelectorPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Initalize"
        
        private Business.ServiceGlossary _Service;
        private Business.ServiceGlossary Service
        {
            get
            {
                if (_Service == null)
                    _Service = new Business.ServiceGlossary(AppContext);
                return _Service;
            }
        }

        public GlossarySelectorPresenter(iApplicationContext oContext, iViewGlossarySelector view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
            
        }

        protected virtual iViewGlossarySelector View
        {
            get { return (iViewGlossarySelector)base.View; }
        }
        #endregion

        #region Bind Dati
        public Boolean BindGlossaries(Int32 CommunityID, Boolean HasPermission)
        {
            Boolean HasElement = false;

            if (HasPermission && !UserContext.isAnonymous)
            {
                IList<Domain.GlossaryGroup> GlossaryGroup = Service.GetGlossaryList(CommunityID);
            
                if (GlossaryGroup == null) { GlossaryGroup = new List<Domain.GlossaryGroup>(); }
                if (GlossaryGroup.Count() > 0) { HasElement = true; }

                this.View.BindGlossary(GlossaryGroup);
            }

            return HasElement;
        }

        #endregion

        #region Copia effettiva
        public Int32 CopyGlossary(Int32 DestCommunityId, Boolean HasPermission)
        {
            if (HasPermission && !UserContext.isAnonymous)
            {
                return Service.CopyGlossary(View.GetSelectedGlossaryIds(), DestCommunityId);
            }
            else { return 0; }
        }

        #endregion
        //private Boolean HasSourcePermission(Int32 CommunityId)
        //{
            
        //    return true;
        //}

        //private Boolean HasDestinationPermission(Int32 CommunityId)
        //{
        //    return true;
        //}

    }
   
}