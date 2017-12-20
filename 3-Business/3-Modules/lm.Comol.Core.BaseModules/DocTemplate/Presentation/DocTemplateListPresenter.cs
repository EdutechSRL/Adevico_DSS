using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using Templ = lm.Comol.Core.DomainModel.DocTemplateVers;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public class DocTemplateListPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region initClass
            //protected DomainModel.DocTemplate.Business.DocTemplateService Service;
        protected Templ.Business.DocTemplateVersManagementService ServiceManagement;
        protected Templ.Business.DocTemplateVersService ServiceGeneric;


        #region Handler
        ////Copy Block
        ////public delegate int CopyBlockEventHandler(Templ.Domain.Core.dtoFileCopyBlock dtoFile);
        ////public event CopyBlockEventHandler OnCopyBlock;
        //public int HandleOnCopyBlock(Templ.Domain.Core.dtoFileCopyBlock dtoFile)
        //{
        //    return Helpers.DocTempalteFileHelper.CopyBlockFile(dtoFile);
        //}

        #endregion

            protected virtual IViewDocTemplateList View
            {
                get { return (IViewDocTemplateList)base.View; }
            }
            public DocTemplateListPresenter(iApplicationContext oContext)
                : base(oContext)
            {
                this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
                InitService();

                this.ServiceGeneric = new Templ.Business.DocTemplateVersService(oContext);
                //this.CurrentManager = new BaseModuleManager(oContext);
                //Service = new DomainModel.DocTemplate.Business.DocTemplateService();
            }

            public DocTemplateListPresenter(iApplicationContext oContext, IViewDocTemplateList view)
                : base(oContext, view)
            {
                this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
                InitService();
                this.ServiceGeneric = new Templ.Business.DocTemplateVersService(oContext);
                //this.Service = new DomainModel.DocTemplate.Business.DocTemplateService(oContext);
                //this.CurrentManager = new BaseModuleManager(oContext);
            }
        #endregion

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                Int32 idUser = UserContext.CurrentUserID;
                Int32 idCommunity = 0;
                Int32 idModule = ServiceManagement.ServiceModuleID();
                //ModuleDocTemplate module = Service.GetServicePermission(idUser, idCommunity);
                //View.CurrentPermission = Module;

                if (Module.ManageTemplates || Module.EditBuiltInTemplates || Module.AddTemplate || Module.ViewTemplates)
                {
                    View.LoadTemplates(ServiceManagement.TemplateListGet(View.Filter, View.OrderBy, View.OrderAscending));
                    View.SendUserAction(idCommunity, idModule, Templ.ModuleDocTemplate.ActionType.List);
                }
                else
                {
                    View.DisplayNoPermission(idCommunity, ServiceManagement.ServiceModuleID());
                    View.SendUserAction(idCommunity, idModule, Templ.ModuleDocTemplate.ActionType.NoPermission);
                }
            }
            //Int64 TemplateId = this.View.CurrentTemplateId;
            //if (TemplateId > 0)
            //{
            //    this.View.Template = this.DocTemplService.GetDocTemplate(TemplateId);
            //}
            //else
            //{
            //    this.View.Template = this.DocTemplService.GetNewDocTemplate();
            //}
        }

        #region ItemCommand

        //Case "PDF"
        public Boolean ExportPDF(Int64 TemplateId, Int64 VersionId, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            //CHECK PERMISSION!!!

            Helpers.HelperExportPDF _Helper = new Helpers.HelperExportPDF();

            Templ.Domain.DTO.ServiceExport.DTO_Template Template = ServiceGeneric.TemplateGet(TemplateId, VersionId, View.TemplateBaseUrl);

            if (Template == null)
                return false;

            String clientFileName = Template.Name;

            if (VersionId > 0)
            {
                clientFileName += "v" + Template.Version.ToString();
            }

            if (Template.UseSkinHeaderFooter)
            {
                Template.Header = this.View.GetHeader;
                Template.Footer = this.View.GetFooter;
            }

            return _Helper.ExportToPdf(Template, clientFileName, webResponse, cookie); //, View.GetSysHeader(), View.GetSysFooter());

        }

        //Case "RTF"
        public Boolean ExportRTF(Int64 TemplateId, Int64 VersionId, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            //CHECK PERMISSION!!!
            Helpers.HelperExportRTF _Helper = new Helpers.HelperExportRTF();
            Templ.Domain.DTO.ServiceExport.DTO_Template Template = ServiceGeneric.TemplateGet(TemplateId, VersionId, View.TemplateBaseUrl);

            String clientFileName = Template.Name;

            if (Template == null)
                return false;

            if (VersionId > 0)
            {
                clientFileName += "v" + Template.Version.ToString();
            }

            return _Helper.ExportToRtf(Template, clientFileName, webResponse, cookie);
        }

        //      ----------         T E M P L A T E         ---------------

        //Case "confirmDelete"
        public void TemplatePhysicalDelete(Int64 TemplateId)
        {
            if (Module.DeleteTemplates && Module.EditBuiltInTemplates)
            {
                ServiceManagement.TemplateDeletePhisical(TemplateId);
                //Service.DeletePhysicalTemplate(TemplateId);
                this.InitView();
            }
        }

        //Case "virtualdelete"
        public void TemplateVirtualDelete(Int64 TemplateId)
        {
            if (Module.DeleteTemplates)
            {
                ServiceManagement.TemplateDeleteLogical(TemplateId);//, AppContext.UserContext.CurrentUserID);
                //Service.DeleteLogicalTemplate(TemplateId, AppContext.UserContext.CurrentUserID);
                this.InitView();
            }
        }

        //Case "recover"
        public void TemplateRecover(Int64 TemplateId)
        {
            if (Module.DeleteTemplates)
            {
                ServiceManagement.TemplateUnDelete(TemplateId);
                //Service.RecoverTemplate(TemplateId, AppContext.UserContext.CurrentUserID);
                this.InitView();
            }
        }

        //case "Disable"
        public void TemplateDisable(Int64 TemplateId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.TemplateSetActive(TemplateId, false);
                this.InitView();
            }
        }

        //Case Enable
        public void TemplateEnable(Int64 TemplateId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.TemplateSetActive(TemplateId, true);
                this.InitView();
            }
        }

        //      ----------         V E R S I O N         ---------------

        //Case "Version.DelVirt" -dt
        public void VersionVirtuallDelete(Int64 TemplateId, Int64 VersionId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.VersionDeleteLogical(VersionId);
                this.InitView();
            }
        }

        //Case "Version.DelPhis" -dt
        public void VersionPhysicalDelete(Int64 TemplateId, Int64 VersionId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.VersionDeletePhisical(VersionId);
                this.InitView();
            }
        }

        //Case "Version.Recover" -dt
        public void VersionRecover(Int64 TemplateId, Int64 VersionId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.VersionUnDelete(VersionId);
                this.InitView();
            }
        }

        //Case "Version.Copy" -dt
        public void VersionCopy(Int64 TemplateId, Int64 VersionId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.VersionCreateCopy(TemplateId, VersionId);
                this.InitView();
            }
        }

        //Case "Version.Enable"
        public void VersionEnable(Int64 TemplateId, Int64 VersionId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.VersionSetActive(TemplateId, VersionId);
                this.InitView();
            }
        }

        //Case "Version.Disable"
        public void VersionDisable(Int64 TemplateId, Int64 VersionId)
        {
            if (Module.EditTemplates)
            {
                ServiceManagement.VersionSetDeActive(TemplateId, VersionId);
                this.InitView();
                //ServiceManagement.VersionCreateCopy(TemplateId, VersionId);
            }
        }

        #endregion

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

       public void InitService()
        {
            if (this.ServiceManagement != null)
            {
                this.ServiceManagement.FileCopy += new Templ.Business.DocTemplateVersManagementService.FileCopyEventHandler(HandleCopyEvent);
                this.ServiceManagement.FileCopyBlock += new Templ.Business.DocTemplateVersManagementService.FileCopyBlockEventHandler(HandleCopyBlockEvent);
                this.ServiceManagement.FileDelete += new Templ.Business.DocTemplateVersManagementService.FileDeleteEventHandler(HandleDeleteEvent);
                this.ServiceManagement.FileRemTmp += new Templ.Business.DocTemplateVersManagementService.FileRemTmpEventHandler(HandleRemTmpEvent);
                this.ServiceManagement.FileRecCopy += new Templ.Business.DocTemplateVersManagementService.FileRecoverCopyEventHandler(HandleRecCopyEvent);
            }
        }

       #region File Event Handlers
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

       //public String CopySkin(String Source)
       //{
       //    //int lastSlash = Source.LastIndexOf("\\");
       //    int lastDot = Source.LastIndexOf(".");

       //    String DestFile = System.Guid.NewGuid().ToString() + Source.Remove(0, lastDot);

       //    String DestPath = Templ.Business.ImageHelper.GetImagePath(DestFile, View.TemplateTempPath, View.TemplateId, View.VersionId);

       //    Boolean success = Helpers.DocTempalteFileHelper.CopyFile(Source, DestPath);

       //    return "#" + DestFile;
       //}
       #endregion
    }
}
