using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using Templ = lm.Comol.Core.DomainModel.DocTemplateVers;
//using lm.Comol.Core.DomainModel.DocTemplate;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public class DocTemplatePreviewPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region initClass
        protected Templ.Business.DocTemplateVersService Service; //DomainModel.DocTemplate.Business.DocTemplateService Service;

            //public virtual BaseModuleManager CurrentManager { get; set; }

            protected virtual IViewDocTemplatePreview View
            {
                get { return (IViewDocTemplatePreview)base.View; }
            }

            public DocTemplatePreviewPresenter(iApplicationContext oContext):base(oContext){
                
                this.Service = new Templ.Business.DocTemplateVersService(oContext);
                this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
                //this.CurrentManager = new BaseModuleManager(oContext);
                Service = new Templ.Business.DocTemplateVersService();
            }

            public DocTemplatePreviewPresenter(iApplicationContext oContext, IViewDocTemplatePreview view)
                : base(oContext, view)
            {
                this.Service = new Templ.Business.DocTemplateVersService(oContext);
                this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
                //this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            //String backurl = View.PreloadBackUrl;
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                //if (!String.IsNullOrEmpty(backurl))
                //    View.SetBackUrl(backurl);

                Int32 idUser = UserContext.CurrentUserID;
                //Int32 idCommunity = 0;
                //Int32 idModule = Service.ServiceModuleID();
                Int64 idTemplate = View.PreloadIdTemplate;
                Int64 idVersion = View.PreloadIdVersion;

                //ModuleDocTemplate module = Service.GetServicePermission(idUser, idCommunity);
               
                //ModuleDocTemplate.ActionType action = ModuleDocTemplate.ActionType.GenericError;
                View.CurrentIdTemplate = idTemplate;
                View.CurrentIdVersion = idVersion;

                //Template template = Service.GetDocTemplate(idTemplate);
                Templ.Domain.DTO.ServiceExport.DTO_Template template = Service.TemplateGet(idTemplate, idVersion, View.TemplateBaseUrl);

                if (template == null)
                    View.DisplayUnknownTemplate();
                else
                {
                    //action = ModuleDocTemplate.ActionType.Preview;
                    View.LoadTemplate(template);
                }

                //if (String.IsNullOrEmpty(backurl))
                //    backurl = (View.PreloadFromList) ? rootObject.List(idTemplate) : rootObject.EditTemplate(idTemplate);

                if (Module.ManageTemplates || Module.EditBuiltInTemplates || Module.EditTemplates || Module.AddTemplate)
                {
                    String toListUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.List(idTemplate);
                    //if (!String.IsNullOrEmpty(toListUrl))
                    //    View.SetBackToListUrl(toListUrl);

                    
                    String toEditUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.EditTemplate(idTemplate);
                    //if (!String.IsNullOrEmpty(toEditUrl) && template.IsEditable)
                       // View.SetEditUrl(toEditUrl);
                    //else
                        //View.SetEditUrl("");
                    
                //View.SendUserAction(idCommunity, idModule, idTemplate, action);
                }
                //else
                //{
                //    View.DisplayNoPermission(idCommunity, Service.ServiceModuleID());
                //    View.SendUserAction(idCommunity, idModule, idTemplate, ModuleDocTemplate.ActionType.NoPermission);
                //}
            }
        }

#region Export

        public Boolean ExportToPdf(String clientFileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            Helpers.HelperExportPDF _Helper = new Helpers.HelperExportPDF();
            //lm.Comol.Core.DomainModel.DocTemplate.Helper.HelperExportPDF _Helper = new lm.Comol.Core.DomainModel.DocTemplate.Helper.HelperExportPDF();
            
            return _Helper.ExportToPdf(GetTemplate(), clientFileName, webResponse, cookie); //, View.GetSysHeader(), View.GetSysFooter());
        }
        public Boolean ExportToRtf(String clientFileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            //lm.Comol.Core.DomainModel.DocTemplate.Helper.HelperExportRTF _Helper = new lm.Comol.Core.DomainModel.DocTemplate.Helper.HelperExportRTF();
            //return false;
            //return _Helper.ExportToRtf(GetTemplate(), clientFileName, webResponse); //, View.GetSysHeader(), View.GetSysFooter());
            Helpers.HelperExportRTF _Helper = new Helpers.HelperExportRTF();
            
            //Helpers.HelperExportRTF _Helper = new Helpers.HelperExportRTF();
            //lm.Comol.Core.DomainModel.DocTemplate.Helper.HelperExportPDF _Helper = new lm.Comol.Core.DomainModel.DocTemplate.Helper.HelperExportPDF();

            return _Helper.ExportToRtf(GetTemplate(), clientFileName, webResponse, cookie);
        }

        private Templ.Domain.DTO.ServiceExport.DTO_Template GetTemplate()//lm.Comol.Core.DomainModel.DocTemplate.Template GetTemplate()
        {
            //lm.Comol.Core.DomainModel.DocTemplate.Template Template = Service.GetDocTemplate(this.View.CurrentIdTemplate);

            // IN questo modo NON devo riverificare i permessi in quanto già validati all'init della view
            Templ.Domain.DTO.ServiceExport.DTO_Template Template = Service.TemplateGet(View.CurrentIdTemplate, View.CurrentIdVersion, View.TemplateBaseUrl);

            //if (Template != null && Template.UseSkinHeaderFooter)
            //{ 
            //    Template.Header = View.GetSysHeader();
            //    Template.Footer = View.GetSysFooter();
            //}
            //Conversione body

            return Template;
        }

#endregion

        protected Templ.Business.DocTemplateVersManagementService ServiceManagement;
        private Templ.ModuleDocTemplate _module;
        private Templ.ModuleDocTemplate Module
        {
            get
            {
                if ((_module == null))
                {
                    Int32 idUser = UserContext.CurrentUserID;
                    Int32 idCommunity = 0;
                    _module = ServiceManagement.GetServicePermission(idUser, idCommunity);
                }
                return _module;
            }
        }
        
        #region Only for test!!!!
        //public IList<lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_TemplateDDL> GetTemplates4DDL()
        //{
        //    return null; // Service.TemplateGetForDDL("");
        //}
        

        #endregion
    }
}