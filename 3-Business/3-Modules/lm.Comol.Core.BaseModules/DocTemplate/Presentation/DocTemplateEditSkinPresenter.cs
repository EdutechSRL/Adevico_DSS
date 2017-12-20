using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lm.Comol.Core.DomainModel;
using Templ = lm.Comol.Core.DomainModel.DocTemplateVers;

namespace lm.Comol.Core.BaseModules.DocTemplate.Presentation
{
    public class DocTemplateEditSkinPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        protected Templ.Business.DocTemplateVersManagementService ServiceManagement;

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

        protected virtual IVIewDocTemplateEditSkin View
        {
            get { return (IVIewDocTemplateEditSkin)base.View; }
        }

        public DocTemplateEditSkinPresenter(iApplicationContext oContext)
            : base(oContext)
        {
            this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
            this.ServiceManagement.FileCopy += new Templ.Business.DocTemplateVersManagementService.FileCopyEventHandler(HandleCopyEvent);
            this.ServiceManagement.FileCopyBlock += new Templ.Business.DocTemplateVersManagementService.FileCopyBlockEventHandler(HandleCopyBlockEvent);
            this.ServiceManagement.FileDelete += new Templ.Business.DocTemplateVersManagementService.FileDeleteEventHandler(HandleDeleteEvent);
            this.ServiceManagement.FileRemTmp += new Templ.Business.DocTemplateVersManagementService.FileRemTmpEventHandler(HandleRemTmpEvent);
            this.ServiceManagement.FileRecCopy += new Templ.Business.DocTemplateVersManagementService.FileRecoverCopyEventHandler(HandleRecCopyEvent);
            //this.ServiceGeneric = new Templ.Business.DocTemplateVersService(oContext);
            //this.CurrentManager = new BaseModuleManager(oContext);
            //Service = new Templ.Business.DocTemplateVersService();
        }

        public DocTemplateEditSkinPresenter(iApplicationContext oContext, IVIewDocTemplateEditSkin view)
            : base(oContext, view)
        {
            this.ServiceManagement = new Templ.Business.DocTemplateVersManagementService(oContext);
            this.ServiceManagement.FileCopy += new Templ.Business.DocTemplateVersManagementService.FileCopyEventHandler(HandleCopyEvent);
            this.ServiceManagement.FileCopyBlock += new Templ.Business.DocTemplateVersManagementService.FileCopyBlockEventHandler(HandleCopyBlockEvent);
            this.ServiceManagement.FileDelete += new Templ.Business.DocTemplateVersManagementService.FileDeleteEventHandler(HandleDeleteEvent);
            this.ServiceManagement.FileRemTmp += new Templ.Business.DocTemplateVersManagementService.FileRemTmpEventHandler(HandleRemTmpEvent);
            this.ServiceManagement.FileRecCopy += new Templ.Business.DocTemplateVersManagementService.FileRecoverCopyEventHandler(HandleRecCopyEvent);
            //this.ServiceGeneric = new Templ.Business.DocTemplateVersService(oContext);
            //this.Service = new Templ.Business.DocTemplateVersService(oContext);
            //this.CurrentManager = new BaseModuleManager(oContext);
        }

        public void InitView()
        {
            if (UserContext.isAnonymous)
                View.DisplaySessionTimeout();
            else
            {
                String backurl = View.PreloadBackUrl;
                if (!String.IsNullOrEmpty(backurl))
                    View.SetBackUrl(backurl);


                Int32 idUser = UserContext.CurrentUserID;
                Int32 idCommunity = 0;
                Int32 idModule = ServiceManagement.ServiceModuleID(); // Service.ServiceModuleID();
                Int64 idTemplate = View.TemplateId;
                Int64 idVersion = View.VersionId;

                //I permessi sono controllati direttamente dal service.
                Templ.Domain.DTO.Management.DTO_EditTemplateVersion Version = null;

                //Carico il DTO.
                if (View.IsAdvancedEdit)
                {
                    Version = this.ServiceManagement.VersionGetEditAdvance(idTemplate, idVersion);
                }
                else
                {
                    Version = this.ServiceManagement.VersionGetEdit(idTemplate, idVersion);
                }



                Templ.ModuleDocTemplate.ActionType action = Templ.ModuleDocTemplate.ActionType.GenericError;
                if (Version == null)
                {
                    action = Templ.ModuleDocTemplate.ActionType.GenericError;
                }
                else    //Il DTO non sarà MAI vuoto, ma al massimo contiene eventuali errori: Templ.Domain.DTO.Management.VersionEditError
                {
                    //String backurl = "";

                    if (Version.Error == Templ.Domain.DTO.Management.VersionEditError.NoPremission)
                    {
                        action = Templ.ModuleDocTemplate.ActionType.NoPermission;
                        View.DisplayNoPermission(idCommunity, idModule);

                    }
                    else if (Version.Error == Templ.Domain.DTO.Management.VersionEditError.none && Version.IdTemplate > 0 && Version.Id > 0)
                    {
                        //backurl = rootObject.List(idTemplate);
                        View.LoadAvailableServices(Version.Services);

                        View.SetCurrentVersion(Version);

                        String toListUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.List(idTemplate);
                        if (!String.IsNullOrEmpty(toListUrl))
                            View.SetBackToListUrl(toListUrl);

                        //String toPreviewUrl = lm.Comol.Core.DomainModel.DocTemplateVers.rootObject.PreviewTemplate(idTemplate, idVersion, false);
                        //if (!String.IsNullOrEmpty(toPreviewUrl))
                        //    View.SetPreviewUrl(toPreviewUrl);

                        idVersion = Version.Id;
                        idTemplate = Version.IdTemplate;
                        action = Templ.ModuleDocTemplate.ActionType.Edit;

                    }
                    else
                    {
                        //backurl = rootObject.List();   //???

                        action = Templ.ModuleDocTemplate.ActionType.GenericError;
                        idVersion = 0;
                        idTemplate = 0;
                        View.ShowError(Version.Error);
                    }

                    View.SendUserAction(idCommunity, idModule, idTemplate, action);
                }
            }


        }

        public void ChangePage(ViewEditTemplateSkinElement ViewElement)
        {
            if (this.View.SaveOnChangeView)
            {
                UpdateCurrentData();
                //... Save current data
            }

            this.View.CurrentView = ViewElement;
        }

        public void UpdateCurrentData()
        {
            //Templ.Domain.DTO.Management.DTO_EditTemplateVersion vrs = View.GetCurrentVersion();
            //Int64 Id = vrs.Id;

            //BREAK PER TEST!!!
            lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_EditTemplateVersion CurrentVersion = View.GetCurrentVersion();
            CurrentVersion.HeaderCenter = null;
            CurrentVersion.HeaderLeft = null;
            CurrentVersion.HeaderRight = null;

            CurrentVersion.FooterCenter = null;
            CurrentVersion.FooterLeft = null;
            CurrentVersion.FooterRight = null;

            this.ServiceManagement.VersionUpdate(CurrentVersion);

            this.InitView();
        }

        #region File Event Handlers
        int HandleCopyBlockEvent(object sender, Templ.Business.FileBlockEventArgs a)
        {
            a.Message.BasePath = View.TempalteBasePath;
            a.Message.BaseTempPath = View.TempalteTempPath;
            return Helpers.DocTempalteFileHelper.CopyBlockFile(a.Message);
        }

        bool HandleCopyEvent(object sender, Templ.Business.CopyEventArgs a)
        {
            String Source = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TempalteBasePath,
                a.Message.srcTempalteId,
                a.Message.srcVersionId);

            String Dest = Templ.Business.ImageHelper.GetImagePath(
                a.Message.DestFile,
                View.TempalteBasePath,
                a.Message.dstTempalteId,
                a.Message.dstVersionId);

            return Helpers.DocTempalteFileHelper.CopyFile(Source, Dest);
        }
        bool HandleRecCopyEvent(object sender, Templ.Business.CopyEventArgs a)
        {
            String Source = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TempalteBasePath,
                a.Message.srcTempalteId,
                a.Message.srcVersionId);

            String Dest = Templ.Business.ImageHelper.GetImagePath(
                a.Message.DestFile,
                View.TempalteTempPath,
                a.Message.dstTempalteId,
                a.Message.dstVersionId);

            return Helpers.DocTempalteFileHelper.CopyFile(Source, Dest);
        }
        bool HandleDeleteEvent(object sender, Templ.Business.DeleteRemTmpEventArgs a)
        {
            return Helpers.DocTempalteFileHelper.DeleteFile(
                Templ.Business.ImageHelper.GetImagePath(a.Message.SourceFile, View.TempalteBasePath, a.Message.TempalteId, a.Message.VersionId));
        }

        bool HandleRemTmpEvent(object sender, Templ.Business.DeleteRemTmpEventArgs a)
        {
            String Source = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TempalteTempPath,
                a.Message.TempalteId,
                a.Message.VersionId);

            String Dest = Templ.Business.ImageHelper.GetImagePath(
                a.Message.SourceFile,
                View.TempalteBasePath,
                a.Message.TempalteId,
                a.Message.VersionId);

            return Helpers.DocTempalteFileHelper.MoveFile(Source, Dest);

        }

        #endregion

        
#region Export
        public Boolean ExportToPdf(String clientFileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            Helpers.HelperExportPDF _Helper = new Helpers.HelperExportPDF();
            return _Helper.ExportToPdf(GetExportTemplate(), clientFileName, webResponse, cookie);
        }
        public Boolean ExportToRtf(String clientFileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            Helpers.HelperExportRTF _Helper = new Helpers.HelperExportRTF();
            return _Helper.ExportToRtf(GetExportTemplate(), clientFileName, webResponse, cookie);
        }

        private Templ.Domain.DTO.ServiceExport.DTO_Template GetExportTemplate()
        {
            //View.GetCurrentVersion()
            Templ.Domain.DTO.ServiceExport.DTO_Template Template = new Templ.Domain.DTO.ServiceExport.DTO_Template();

            Templ.Domain.DTO.Management.DTO_EditTemplateVersion CurVers = View.GetCurrentVersion();

            Template.Name = CurVers.TemplateName;
            Template.TemplateId = CurVers.IdTemplate;
            Template.VersionId = CurVers.Id;

            Template.Body = new Templ.Domain.DTO.ServiceExport.DTO_ElementText();
            Template.Body.Id = CurVers.Body.Data.Id;
            Template.Body.Alignment = CurVers.Body.Data.Alignment;
            Template.Body.IsHTML = CurVers.Body.Data.IsHTML;
            Template.Body.Text = CurVers.Body.Data.Text;

            Template.Header = View.GetCurrentHeader; //convertHeaderFooter(CurVers.HeaderLeft.Data, CurVers.HeaderCenter.Data, CurVers.HeaderRight.Data, Template.TemplateId, Template.VersionId);
            Template.Footer = View.GetCurrentFooter; //convertHeaderFooter(CurVers.FooterLeft.Data, CurVers.FooterCenter.Data, CurVers.FooterRight.Data, Template.TemplateId, Template.VersionId);

            Template.Settings = convertSettings(CurVers.Setting.Data, Template.TemplateId, Template.VersionId);

            Template.Signatures = convertSignatures(CurVers.Signatures, Template.TemplateId, Template.VersionId);

            //Rivedere!!!
            Template.UseSkinHeaderFooter = false;

            return Template;
        }

        private Templ.Domain.DTO.ServiceExport.DTO_HeaderFooter convertHeaderFooter(
    Templ.PageElement LeftElement,
    Templ.PageElement CenterElement,
    Templ.PageElement RightElement,
    Int64 TemplateId, Int64 VersionId)
        {
            Templ.Domain.DTO.ServiceExport.DTO_HeaderFooter HeadFoot = new Templ.Domain.DTO.ServiceExport.DTO_HeaderFooter();
            HeadFoot.Left = convertElement(LeftElement, TemplateId, VersionId);
            HeadFoot.Center = convertElement(CenterElement, TemplateId, VersionId);
            HeadFoot.Right = convertElement(RightElement, TemplateId, VersionId);

            return HeadFoot;
        }

        private Templ.Domain.DTO.ServiceExport.DTO_Element convertElement(
    Templ.PageElement element,
    Int64 TemplateId, Int64 VersionId)
        {

            if (element == null || element.GetType() == typeof(Templ.ElementVoid))
            {
                return null;
            }

            Templ.Domain.DTO.ServiceExport.DTO_Element OutElement = new Templ.Domain.DTO.ServiceExport.DTO_Element();
            if (element.GetType() == typeof(Templ.ElementImage))
            {
                Templ.Domain.DTO.ServiceExport.DTO_ElementImage imgel = new Templ.Domain.DTO.ServiceExport.DTO_ElementImage();
                Templ.ElementImage img = (Templ.ElementImage)element;

                imgel.Alignment = img.Alignment;
                imgel.Height = img.Height;
                imgel.Id = img.Id;

                if (img.Path.StartsWith("#"))
                {
                    imgel.Path = Templ.Business.ImageHelper.GetImageUrl(img.Path.Remove(0, 1), View.TemplateBaseTempUrl, TemplateId, VersionId);
                }
                else
                {
                    imgel.Path = Templ.Business.ImageHelper.GetImageUrl(img.Path, View.TemplateBaseUrl, TemplateId, VersionId);
                }


                imgel.Width = img.Width;

                OutElement = imgel;
            }
            else if (element.GetType() == typeof(Templ.ElementText))
            {
                Templ.Domain.DTO.ServiceExport.DTO_ElementText txtel = new Templ.Domain.DTO.ServiceExport.DTO_ElementText();
                Templ.ElementText txt = (Templ.ElementText)element;

                txtel.Alignment = txt.Alignment;
                txtel.Id = txt.Id;
                txtel.IsHTML = txt.IsHTML;
                txtel.Text = txt.Text;

                OutElement = txtel;
            }
            else
            {
                return null;
            }


            return OutElement;
        }


        private Templ.Domain.DTO.ServiceExport.DTO_Settings convertSettings(Templ.Settings settings, Int64 TemplateId, Int64 VersionId)
        {
            Templ.Domain.DTO.ServiceExport.DTO_Settings OutSettings = new Templ.Domain.DTO.ServiceExport.DTO_Settings();
            OutSettings.Author = settings.Author;
            OutSettings.BackgroundAlpha = settings.BackgroundAlpha;
            OutSettings.BackgroundBlue = settings.BackgroundBlue;
            OutSettings.BackgroundGreen = settings.BackgroundGreen;
            OutSettings.BackGroundImageFormat = settings.BackGroundImageFormat;


            //OutSettings.BackgroundImagePath = settings.BackgroundImagePath;

            if (settings.BackgroundImagePath.StartsWith("#"))
            {
                OutSettings.BackgroundImagePath = Templ.Business.ImageHelper.GetImageUrl(settings.BackgroundImagePath.Remove(0, 1), View.TemplateBaseTempUrl, TemplateId, VersionId);
            }
            else
            {
                OutSettings.BackgroundImagePath = Templ.Business.ImageHelper.GetImageUrl(settings.BackgroundImagePath, View.TemplateBaseUrl, TemplateId, VersionId);
            }

            OutSettings.BackgroundRed = settings.BackgroundRed;
            OutSettings.Creator = settings.Creator;
            //OutSettings.HasHeaderOnFirstPage = settings.HasHeaderOnFirstPage;
            OutSettings.Height = settings.Height;
            OutSettings.Id = settings.Id;
            OutSettings.IsActive = settings.IsActive;
            OutSettings.Keywords = settings.Keywords;
            OutSettings.MarginBottom = settings.MarginBottom;
            OutSettings.MarginLeft = settings.MarginLeft;
            OutSettings.MarginRight = settings.MarginRight;
            OutSettings.MarginTop = settings.MarginTop;
            //OutSettings.PageNumberAlignment = settings.PageNumberAlignment;
            OutSettings.Producer = settings.Producer;
            //OutSettings.ShowPageNumber = settings.ShowPageNumber;
            OutSettings.Size = settings.Size;
            OutSettings.Subject = settings.Subject;
            OutSettings.Title = settings.Title;
            OutSettings.Width = settings.Width;

            OutSettings.PagePlacingMask = settings.PagePlacingMask;
            OutSettings.PagePlacingRange = settings.PagePlacingRange;

            return OutSettings;
        }

        private IList<Templ.Domain.DTO.ServiceExport.DTO_Signature> convertSignatures(
            IList<Templ.Domain.DTO.Management.DTO_EditItem<Templ.Signature>> EditSignatures,
            Int64 TemplateId, Int64 VersionId)
        {
            IList<Templ.Domain.DTO.ServiceExport.DTO_Signature> OutSignatures = new List<Templ.Domain.DTO.ServiceExport.DTO_Signature>();

            foreach (Templ.Domain.DTO.Management.DTO_EditItem<Templ.Signature> insgn in EditSignatures)
            {
                if (insgn.Data != null)
                {
                    Templ.Domain.DTO.ServiceExport.DTO_Signature sgn = new Templ.Domain.DTO.ServiceExport.DTO_Signature();
                    sgn.HasImage = insgn.Data.HasImage;
                    sgn.HasPDFPositioning = insgn.Data.HasPDFPositioning;
                    sgn.Height = insgn.Data.Height;
                    sgn.Id = insgn.Data.Id;
                    sgn.IsHTML = insgn.Data.IsHTML;
                    sgn.Order = insgn.Data.Placing;


                    //sgn.Path = Templ.Business.ImageHelper.GetImageUrl(insgn.Data.Path, BaseUrl, TemplateId, VersionId) ;


                    //OutSettings.BackgroundImagePath = settings.BackgroundImagePath;

                    if (insgn.Data.Path.StartsWith("#"))
                    {
                        sgn.Path = Templ.Business.ImageHelper.GetImageUrl(insgn.Data.Path.Remove(0, 1), View.TemplateBaseTempUrl, TemplateId, VersionId);
                    }
                    else
                    {
                        sgn.Path = Templ.Business.ImageHelper.GetImageUrl(insgn.Data.Path, View.TemplateBaseUrl, TemplateId, VersionId);
                    }


                    sgn.PosBottom = insgn.Data.PosBottom;
                    sgn.Position = insgn.Data.Position;
                    sgn.PosLeft = insgn.Data.PosLeft;
                    sgn.Text = insgn.Data.Text;
                    sgn.Width = insgn.Data.Width;

                    OutSignatures.Add(sgn);
                }
            }

            return OutSignatures;
        }
#endregion

#region Versioning
    #region Delete

        public void SettingsDeletePrev(Int64 Id)
        {
            ServiceManagement.SettingDeletePhisical(Id);
            this.InitView();
        }
        public void PageElementDeletePrev(Int64 TemplateId, Int64 VersionId, Int64 Id)
        {
            ServiceManagement.PageElementsDeletePhisical(TemplateId, VersionId, Id);
            this.InitView();
        }
        public void SignatureDeletePrev(Int64 TemplateId, Int64 VersionId, Int64 Id)
        {
            ServiceManagement.SignatureDeletePhisical(TemplateId, VersionId, Id);
            this.InitView();
        }
    #endregion
    #region Recover

        public void SettingsRecoverPrev(Int64 Id)
        {
            this.View.RecoverSettings(ServiceManagement.SettingsGetRecover(Id, View.IsAdvancedEdit, View.TemplateId, View.VersionId));
        }
        public void PageElementRecoverPrev(Int64 Id)
        {
            this.View.RecoverPageElement(ServiceManagement.PageElementGetRecover(Id, View.IsAdvancedEdit, View.TemplateId, View.VersionId));
        }
        public void SignatureRecoverPrev(Int64 Id)
        {
            this.View.RecoverSignature(ServiceManagement.SignatureGetRecover(Id, View.TemplateId, View.VersionId)); //, View.IsAdvancedEdit));
        }

    #endregion
    
    #region Delete List

        public void SettingsDeletePrevs(IList<Int64> Ids)
        {
            if (Ids != null && Ids.Count > 0)
            {
                foreach (Int64 Id in Ids)
                {
                    ServiceManagement.SettingDeletePhisical(Id);
                }
            }

            this.InitView();
        }
        public void PageElementDeletePrevs(Int64 TemplateId, Int64 VersionId, IList<Int64> Ids)
        {
            if (Ids != null && Ids.Count > 0)
            {
                foreach (Int64 Id in Ids)
                {
                    ServiceManagement.PageElementsDeletePhisical(TemplateId, VersionId, Id);
                }
            }
            this.InitView();
        }
        public void SignatureDeletePrevs(Int64 TemplateId, Int64 VersionId, IList<Int64> Ids)
        {
            if (Ids != null && Ids.Count > 0)
            {
                foreach (Int64 Id in Ids)
                {
                    ServiceManagement.SignatureDeletePhisical(TemplateId, VersionId, Id);
                }
            }
            this.InitView();
        }
    #endregion

#endregion
    }
}
