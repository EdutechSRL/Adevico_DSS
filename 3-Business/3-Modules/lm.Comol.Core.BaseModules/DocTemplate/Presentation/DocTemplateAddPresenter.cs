using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using Templ = lm.Comol.Core.DomainModel.DocTemplateVers;
//using lm.Comol.Core.DomainModel.DocTemplate;        //SOLO PER ACTION!!!  POI TOGLIERE!

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public class DocTemplateAddPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        
        #region initClass
            //protected DomainModel.DocTemplate.Business.DocTemplateService Service;
        protected Templ.Business.DocTemplateVersManagementService ServiceManagement;
        protected Templ.Business.DocTemplateVersService ServiceGeneric;

        protected virtual IViewDocTemplateAdd View
        {
            get { return (IViewDocTemplateAdd)base.View; }
        }

        public DocTemplateAddPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
            this.ServiceGeneric = new Templ.Business.DocTemplateVersService(oContext);

            this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
            this.ServiceManagement.FileCopy += new Templ.Business.DocTemplateVersManagementService.FileCopyEventHandler(HandleCopyEvent);
            this.ServiceManagement.FileCopyBlock += new Templ.Business.DocTemplateVersManagementService.FileCopyBlockEventHandler(HandleCopyBlockEvent);
            this.ServiceManagement.FileDelete += new Templ.Business.DocTemplateVersManagementService.FileDeleteEventHandler(HandleDeleteEvent);
            this.ServiceManagement.FileRemTmp += new Templ.Business.DocTemplateVersManagementService.FileRemTmpEventHandler(HandleRemTmpEvent);
            this.ServiceManagement.FileRecCopy += new Templ.Business.DocTemplateVersManagementService.FileRecoverCopyEventHandler(HandleRecCopyEvent);
            //this.CurrentManager = new BaseModuleManager(oContext);
            //Service = new DomainModel.DocTemplate.Business.DocTemplateService();
        }

        public DocTemplateAddPresenter(iApplicationContext oContext, IViewDocTemplateAdd view)
                : base(oContext, view)
            {
                this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
                this.ServiceGeneric = new Templ.Business.DocTemplateVersService(oContext);

                this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
                this.ServiceManagement.FileCopy += new Templ.Business.DocTemplateVersManagementService.FileCopyEventHandler(HandleCopyEvent);
                this.ServiceManagement.FileCopyBlock += new Templ.Business.DocTemplateVersManagementService.FileCopyBlockEventHandler(HandleCopyBlockEvent);
                this.ServiceManagement.FileDelete += new Templ.Business.DocTemplateVersManagementService.FileDeleteEventHandler(HandleDeleteEvent);
                this.ServiceManagement.FileRemTmp += new Templ.Business.DocTemplateVersManagementService.FileRemTmpEventHandler(HandleRemTmpEvent);
                this.ServiceManagement.FileRecCopy += new Templ.Business.DocTemplateVersManagementService.FileRecoverCopyEventHandler(HandleRecCopyEvent);
                //this.Service = new DomainModel.DocTemplate.Business.DocTemplateService(oContext);
                //this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        #region base
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
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                String toListUrl, backUrl;
                backUrl = View.PreloadBackUrl;
 
                Int32 idUser = UserContext.CurrentUserID;
                Int32 idCommunity = 0;
                Int32 idModule = ServiceManagement.ServiceModuleID();
                //ModuleDocTemplate module = Service.GetServicePermission(idUser, idCommunity);
                View.CurrentPermission = Module;

                if (Module.AddTemplate) //() <- ONLY FOR TEST!!!
                {
                    Templ.Domain.DTO.Management.DTO_AddTemplate dtoTmpl = ServiceManagement.TemplateGetForAdd(View.PreloadedTemplateId);
                    
                    dtoTmpl.IsActiveDefault = true;
                    dtoTmpl.IsActiveSystem = Module.EditBuiltInTemplates && dtoTmpl.IsActiveSystem;
                    dtoTmpl.IsActiveSkin = true;
                    dtoTmpl.IsActiveExternal = false;
                    
                    
                    View.BindView(dtoTmpl);

                    //View.LoadTemplates(ServiceManagement.TemplateListGet(""));
                    View.SendUserAction(idCommunity, idModule, Templ.ModuleDocTemplate.ActionType.List);

                     toListUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.List(View.PreloadedTemplateId);
                     View.SetBackUrls(backUrl, toListUrl);
                }
                else
                {
                    View.DisplayNoPermission(idCommunity, ServiceManagement.ServiceModuleID());
                    View.SendUserAction(idCommunity, idModule, Templ.ModuleDocTemplate.ActionType.NoPermission);
                    View.SetBackUrls(backUrl, "");
                }
            }
        }

        public void AddTemplate()
        {
            if (String.IsNullOrEmpty(View.TemplateName))
            {
                View.ShowError(AddError.invalidName);
                return;
            }

            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();

            else if (Module.AddTemplate) 
            {

                //Templ.TemplateType tmplType = Templ.TemplateType.Standard;

                //switch (View.TemplateType)
                //{
                //    case Domain.dtoAddTemplateType.External:
                //        tmplType = Templ.TemplateType.External;
                //        break;
                //    case Domain.dtoAddTemplateType.Skin:
                //        tmplType = Templ.TemplateType.Skin;
                //        break;
                //    case Domain.dtoAddTemplateType.System:
                //        tmplType = Templ.TemplateType.System;
                //        break;
                //}
                Boolean IsSystem = false;

                Int64 NewTemplId = ServiceManagement.TemplateCopyCreateNew(View.PreloadedTemplateId, View.TemplateName, View.TemplateType, View.SelectedServicesId, ref IsSystem);


                if (NewTemplId > 0)
                {
                    if (IsSystem)
                    {
                        String toListUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.List(NewTemplId);
                        View.GoToList(toListUrl);
                    }
                    else
                        View.GoToEdit(NewTemplId);
                }
                else
                {
                    View.ShowError(AddError.genericError);
                }
                    
            }
        }

        #region File Event Handlers

        //Ci sono tutti, anche se servono solo quelli specifici alla copia.

        int HandleCopyBlockEvent(object sender, Templ.Business.FileBlockEventArgs a)
        {
            a.Message.BasePath = View.TemplateBasePath;
            a.Message.BaseTempPath = View.TemplateTempPath;
            return Helpers.DocTempalteFileHelper.CopyBlockFile(a.Message);
        }

        bool HandleCopyEvent(object sender, Templ.Business.CopyEventArgs a)
        {
            String Source = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TemplateBasePath,
                a.Message.srcTempalteId,
                a.Message.srcVersionId);

            String Dest = Templ.Business.ImageHelper.GetImagePath(
                a.Message.DestFile,
                View.TemplateBasePath,
                a.Message.dstTempalteId,
                a.Message.dstVersionId);

            return Helpers.DocTempalteFileHelper.CopyFile(Source, Dest);
        }

        bool HandleRecCopyEvent(object sender, Templ.Business.CopyEventArgs a)
        {
            String Source = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TemplateBasePath,
                a.Message.srcTempalteId,
                a.Message.srcVersionId);

            String Dest = Templ.Business.ImageHelper.GetImagePath(
                a.Message.DestFile,
                View.TemplateTempPath,
                a.Message.dstTempalteId,
                a.Message.dstVersionId);

            return Helpers.DocTempalteFileHelper.CopyFile(Source, Dest);
        }

        bool HandleDeleteEvent(object sender, Templ.Business.DeleteRemTmpEventArgs a)
        {
            return Helpers.DocTempalteFileHelper.DeleteFile(
                Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TemplateBasePath,
                a.Message.TempalteId,
                a.Message.VersionId));
        }

        bool HandleRemTmpEvent(object sender, Templ.Business.DeleteRemTmpEventArgs a)
        {
            String Source = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TemplateTempPath,
                a.Message.TempalteId,
                a.Message.VersionId);

            String Dest = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TemplateBasePath,
                a.Message.TempalteId,
                a.Message.VersionId);

            return Helpers.DocTempalteFileHelper.MoveFile(Source, Dest);

        }
        #endregion

        public bool ServiceIsEnable(Int32 ServiceId)
        {
            return ServiceManagement.ServiceIsEnabled(ServiceId);
        }

    }
}
